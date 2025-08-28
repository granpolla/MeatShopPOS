Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions

Public Class frmCashierDashboard

    Private Sub frmCashierDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCustomers() ' optional: preload all customers on load
    End Sub


    ' ✅ Normalize name (remove extra spaces + proper case)
    Private Function NormalizeName(input As String) As String
        Dim cleaned As String = Regex.Replace(input.Trim(), "\s+", " ")
        Return StrConv(cleaned, VbStrConv.ProperCase)
    End Function


    ' ✅ Load all customers (fullname + address only)
    Private Sub LoadCustomers()
        Dim query As String = "
        SELECT customer_name AS 'Full Name',
               address AS 'Address'
        FROM customer
        ORDER BY customer_name ASC"
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                Using cmd As New MySqlCommand(query, conn)
                    Dim adapter As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    adapter.Fill(dt)
                    dgvCustomer.DataSource = dt
                    FormatCustomerGrid()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading customers: " & ex.Message)
        End Try
    End Sub


    ' ✅ Format dgvCustomer
    Private Sub FormatCustomerGrid()
        With dgvCustomer
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
        End With
    End Sub


    ' ✅ Search Button
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim searchName As String = NormalizeName(txtCustomerName.Text)

        If String.IsNullOrWhiteSpace(searchName) Then
            MessageBox.Show("Please enter a customer name to search.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim query As String = "
    SELECT customer_name AS 'Full Name',
           address AS 'Address'
    FROM customer
    WHERE LOWER(REPLACE(customer_name, ' ', '')) = LOWER(REPLACE(@name, ' ', ''))"

        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@name", searchName.Replace(" ", ""))

                    Dim adapter As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    adapter.Fill(dt)

                    If dt.Rows.Count > 0 Then
                        ' ✅ Found customer → show in grid
                        dgvCustomer.DataSource = dt
                        FormatCustomerGrid()
                    Else
                        ' ❌ Not found → Ask cashier to insert
                        Dim result As DialogResult = MessageBox.Show(
                        $"Customer ""{searchName}"" is not saved in our database. Do you want to insert this customer?",
                        "Customer Not Found",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    )

                        If result = DialogResult.Yes Then
                            ' ✅ Open AddCustomer form with name prefilled
                            Dim addForm As New frmCashierAddCustomer()
                            addForm.txtCustomerName.Text = searchName
                            addForm.txtAddress.Clear()
                            addForm.ShowDialog()

                            ' After closing addForm, refresh list
                            LoadCustomers()
                        End If
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error searching customer: " & ex.Message)
        End Try
    End Sub



    ' ✅ Refresh Button
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadCustomers()
    End Sub


    ' ✅ Clear Button
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtCustomerName.Clear()
        txtCustomerAddress.Clear()
        dgvCustomer.DataSource = Nothing
        dgvCustomerBalancePreview.DataSource = Nothing
    End Sub


    ' ✅ Select row in dgvCustomer → autofill textboxes
    Private Sub dgvCustomer_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCustomer.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvCustomer.Rows(e.RowIndex)
            txtCustomerName.Text = row.Cells("Full Name").Value.ToString()
            txtCustomerAddress.Text = row.Cells("Address").Value.ToString()
        End If
    End Sub

End Class