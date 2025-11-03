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

    Private Sub btnPrintOrderSlip_Click(sender As Object, e As EventArgs) Handles btnPrintOrderSlip.Click
        Dim tin As String = txtTin.Text.Trim()
        Dim busStyle As String = txtBusStyle.Text.Trim()

        If tin = "" Or busStyle = "" Then
            MessageBox.Show("Please fill in both TIN and Business Style.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

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
                    cmd.Parameters.AddWithValue("@cid", LoggedInUserID) ' Use your cashier's user ID variable
                    cmd.Parameters.AddWithValue("@slip", slipId)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            ' 🔹 Fetch data again from sales_transaction to print the order slip
            Dim customerName As String = ""
            Dim customerAddress As String = ""
            Dim totalPurchase As Decimal = 0D
            Dim totalAmount As Decimal = 0D
            Dim orderItems As New DataTable()

            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                ' Get customer info and totals
                Using cmd As New MySqlCommand("
                    SELECT c.customer_name,
                           c.address,
                           st.total_amount
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

                ' Get transaction items
                Using cmd As New MySqlCommand("
                    SELECT p.product_name AS ITEM,
                           ti.number_of_box AS BOX,
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

            ' 🔹 Build Order Slip file path
            Dim filePath As String = Path.Combine(PrinterModule.ReceiptsFolder, slipId & "-orderslip.pdf")

            ' 🔹 Generate Order Slip PDF (reuse your existing generator)
            GenerateReceiptPDF(filePath,
                slipId,
                DateTime.Now.ToString("M/d/yyyy HH:mm"),
                customerName,
                customerAddress,
                totalAmount.ToString("N2"),
                totalAmount.ToString("N2"),
                orderItems,
                tin,
                busStyle,
                "", "", "", Nothing)

            MessageBox.Show("Order Slip printed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error printing order slip: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    ' ========================================
    ' 🔹 Generate next Order Slip ID (0001–9999)
    ' ========================================
    Private Function GetNextOrderSlipId() As String
        Dim nextId As String = "0001"

        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                Using cmd As New MySqlCommand("
                    SELECT LPAD(COALESCE(MAX(CAST(order_slip_id AS UNSIGNED)) + 1, 1), 4, '0') AS next_slip
                    FROM order_slip_request
                    WHERE created_date = CURDATE()", conn)

                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        nextId = result.ToString()
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error generating Order Slip ID: " & ex.Message)
        End Try

        Return nextId
    End Function
End Class
