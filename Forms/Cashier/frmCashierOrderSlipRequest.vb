Imports MySql.Data.MySqlClient
Imports System.IO

Public Class frmCashierOrderSlipRequest
    Inherits System.Windows.Forms.Form
    Private _orderNumber As String
    Private _transactionId As Integer

    Public Sub New(orderNumber As String, transactionId As Integer)
        InitializeComponent()
        _orderNumber = orderNumber
        _transactionId = transactionId
    End Sub

    Private Const OrderSlipFolder As String = "C:\POS_OrderSlipRequest"
    ' TODO: Replace with your actual logged-in user variable
    Private LoggedInUserID As Integer = 1

    Private Sub btnPrintOrderSlip_Click(sender As Object, e As EventArgs) Handles btnPrintOrderSlip.Click
        Dim tin As String = txtTin.Text.Trim()
        Dim busStyle As String = txtBusStyle.Text.Trim()

        If tin = "" Or busStyle = "" Then
            MessageBox.Show("Please fill in both TIN and Business Style.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        btnPrintOrderSlip.Enabled = False
        Cursor = Cursors.WaitCursor

        Try
            ' 🔹 Generate the next order slip ID (0001–9999)
            Dim slipId As String = GetNextOrderSlipId()

            ' 🔹 Save the request record to DB
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                Using cmd As New MySqlCommand("
            INSERT INTO order_slip_request (transaction_id, cashier_id, order_slip_id)
            VALUES (@tid, @cid, @slip)", conn)
                    cmd.Parameters.AddWithValue("@tid", _transactionId)
                    cmd.Parameters.AddWithValue("@cid", LoggedInUserID)
                    cmd.Parameters.AddWithValue("@slip", slipId)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            ' 🔹 Fetch data again from sales_transaction to print the order slip
            Dim customerName As String = ""
            Dim customerAddress As String = ""
            Dim totalAmount As Decimal = 0D
            Dim orderItems As New DataTable()

            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                ' Customer info
                Using cmd As New MySqlCommand("
                SELECT c.customer_name, c.address, st.total_amount
                FROM sales_transaction st
                JOIN customer c ON st.customer_id = c.id
                WHERE st.id = @tid", conn)
                    cmd.Parameters.AddWithValue("@tid", _transactionId)
                    Using rdr = cmd.ExecuteReader()
                        If rdr.Read() Then
                            customerName = rdr("customer_name").ToString()
                            customerAddress = rdr("address").ToString()
                            totalAmount = Convert.ToDecimal(rdr("total_amount"))
                        End If
                    End Using
                End Using

                ' Transaction items
                Using cmd As New MySqlCommand("
                SELECT p.product_name AS ITEM,
                       ti.number_of_box AS QTY,
                       ti.total_weight_kg AS WEIGHT,
                       ti.unit_price_php AS PRICE,
                       ti.subtotal AS AMOUNT
                FROM transaction_item ti
                JOIN product p ON ti.product_id = p.id
                WHERE ti.transaction_id = @tid", conn)
                    cmd.Parameters.AddWithValue("@tid", _transactionId)
                    Using adapter As New MySqlDataAdapter(cmd)
                        adapter.Fill(orderItems)
                    End Using
                End Using
            End Using

            ' 🔹 Ensure folder exists
            If Not Directory.Exists(OrderSlipFolder) Then
                Directory.CreateDirectory(OrderSlipFolder)
            End If

            ' 🔹 Normalize columns for GenerateReceiptPDF
            Dim normalizedItems As New DataTable()
            normalizedItems.Columns.Add("QTY", GetType(Decimal))
            normalizedItems.Columns.Add("UNIT", GetType(Decimal))
            normalizedItems.Columns.Add("ARTICLES", GetType(String))
            normalizedItems.Columns.Add("UNIT PRICE", GetType(Decimal))
            normalizedItems.Columns.Add("AMOUNT", GetType(Decimal))

            For Each row As DataRow In orderItems.Rows
                Dim qty As Decimal = 0D
                Dim unit As Decimal = 0D
                Dim article As String = row("ITEM").ToString()
                Dim unitPrice As Decimal = 0D
                Dim amount As Decimal = 0D

                ' UNIT = number_of_box (boxes)
                If orderItems.Columns.Contains("QTY") AndAlso Not IsDBNull(row("QTY")) Then
                    unit = Convert.ToDecimal(row("QTY"))
                End If

                ' QTY = total_weight_kg (weight sold)
                If orderItems.Columns.Contains("WEIGHT") AndAlso Not IsDBNull(row("WEIGHT")) Then
                    qty = Convert.ToDecimal(row("WEIGHT"))

                    ' ✅ Format QTY as whole number if possible
                    If qty = Math.Truncate(qty) Then
                        qty = Math.Truncate(qty)
                    End If
                End If

                ' Keep product name with brand info if available
                article = article & " (" & GetProductBrand(row("ITEM").ToString()) & ")"

                ' UNIT PRICE = unit_price_php
                If orderItems.Columns.Contains("PRICE") AndAlso Not IsDBNull(row("PRICE")) Then
                    unitPrice = Convert.ToDecimal(row("PRICE"))
                End If

                ' AMOUNT = subtotal
                If orderItems.Columns.Contains("AMOUNT") AndAlso Not IsDBNull(row("AMOUNT")) Then
                    amount = Convert.ToDecimal(row("AMOUNT"))
                End If

                normalizedItems.Rows.Add(qty, unit, article, unitPrice, amount)
            Next




            ' 🔹 Build and generate order slip (only declare filePath ONCE)
            Dim filePath As String = Path.Combine(OrderSlipFolder, slipId & "-orderslip.pdf")

            GenerateReceiptPDF(filePath,
            slipId,
            DateTime.Now.ToString("M/d/yyyy HH:mm"),
            customerName,
            customerAddress,
            totalAmount.ToString("N2"),
            totalAmount.ToString("N2"),
            normalizedItems,
            tin,
            busStyle,
            "", "", "", Nothing)


            MessageBox.Show("Order Slip printed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error printing order slip: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnPrintOrderSlip.Enabled = True
            Cursor = Cursors.Default
        End Try
    End Sub


    Private Function GetProductBrand(productName As String) As String
        Using conn As New MySqlConnection(My.Settings.DBConnection)
            conn.Open()
            Using cmd As New MySqlCommand("SELECT brand FROM product WHERE product_name = @pname", conn)
                cmd.Parameters.AddWithValue("@pname", productName)
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                    Return result.ToString()
                End If
            End Using
        End Using
        Return ""
    End Function




    ' ========================================
    ' 🔹 Generate next Order Slip ID (0001–9999)
    ' ========================================
    Private Function GetNextOrderSlipId() As String
        Dim nextId As Integer = 1

        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                Using cmd As New MySqlCommand("
                SELECT COALESCE(MAX(CAST(order_slip_id AS UNSIGNED)), 0) + 1
                FROM order_slip_request
                WHERE created_date = CURDATE()", conn)

                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        nextId = Convert.ToInt32(result)
                    End If
                End Using
            End Using

            ' ✅ Roll over if greater than 9999
            If nextId > 9999 Then
                nextId = 1
            End If

        Catch ex As Exception
            MessageBox.Show("Error generating Order Slip ID: " & ex.Message)
        End Try

        ' Pad to 4 digits
        Return nextId.ToString("D4")
    End Function
End Class
