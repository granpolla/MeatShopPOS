Imports MySql.Data.MySqlClient
Imports System.Drawing

Public Class frmDashboard

    Private Sub frmDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ✅ Populate ComboBox
        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("Revenue")
        ComboBox1.Items.Add("Payments Received")
        ComboBox1.SelectedIndex = 0 ' Default selection
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList

        LoadCashiers()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        LoadCashiers()
    End Sub

    Private Sub LoadCashiers()
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                ' ✅ Decide which column to SUM based on selection
                Dim salesColumn As String
                If ComboBox1.SelectedItem.ToString() = "Revenue" Then
                    salesColumn = "st.total_amount"
                Else
                    salesColumn = "st.amount_paid"
                End If

                Dim query As String = $"
                    SELECT u.id, u.full_name, 
                           IFNULL(SUM({salesColumn}), 0) AS total_sales
                    FROM user u
                    INNER JOIN role r ON u.role_id = r.id
                    LEFT JOIN sales_transaction st ON u.id = st.user_id
                    WHERE r.role_name = 'cashier'
                    GROUP BY u.id, u.full_name
                    ORDER BY u.id ASC;
                "

                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()

                        ' ✅ Clear previous controls
                        For i As Integer = Panel1.Controls.Count - 1 To 0 Step -1
                            Dim ctrl = Panel1.Controls(i)
                            If TypeOf ctrl Is TextBox AndAlso ctrl.Name.StartsWith("txtCashier") Then
                                Panel1.Controls.Remove(ctrl)
                            End If
                        Next

                        Dim yPos As Integer = 82   ' starting Y position inside Panel1
                        Dim index As Integer = 1
                        Dim grandTotal As Decimal = 0

                        While reader.Read()
                            Dim fullName As String = reader("full_name").ToString()
                            Dim totalSales As Decimal = Convert.ToDecimal(reader("total_sales"))
                            grandTotal += totalSales

                            ' ✅ Cashier Name TextBox
                            Dim txtName As New TextBox()
                            txtName.Name = "txtCashier" & index
                            txtName.Text = fullName
                            txtName.ReadOnly = True
                            txtName.Width = 110
                            txtName.BorderStyle = BorderStyle.None
                            txtName.BackColor = Color.White
                            txtName.Font = New Font("Segoe UI", 10)
                            txtName.Location = New Point(3, yPos)

                            ' ✅ Sales Total TextBox
                            Dim txtSalesBox As New TextBox()
                            txtSalesBox.Name = "txtCashierSales" & index
                            txtSalesBox.Text = totalSales.ToString("N2") ' format 12,000.00
                            txtSalesBox.ReadOnly = True
                            txtSalesBox.Width = 70
                            txtSalesBox.TextAlign = HorizontalAlignment.Right
                            txtSalesBox.Font = New Font("Segoe UI", 10, FontStyle.Bold)

                            txtSalesBox.BorderStyle = BorderStyle.None
                            txtSalesBox.BackColor = Color.White
                            txtSalesBox.Location = New Point(130, yPos)

                            ' Add them to Panel1
                            Panel1.Controls.Add(txtName)
                            Panel1.Controls.Add(txtSalesBox)

                            yPos += 30  ' spacing between rows
                            index += 1
                        End While

                        ' ✅ Show grand total in txtSales
                        txtSales.Text = grandTotal.ToString("N2")

                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading cashiers: " & ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

End Class
