Imports MySql.Data.MySqlClient

Public Class frmCustomer

    Private Sub frmCustomer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCustomers()
    End Sub

    ' ✅ Load customers into DataGridView
    Public Sub LoadCustomers()
        Dim query As String = "
            SELECT id,
                   customer_name AS 'Customer Name',
                   address AS 'Address',
                   created_at AS 'Created At',
                   updated_at AS 'Updated At'
            FROM customer
            ORDER BY created_at DESC"
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                Using cmd As New MySqlCommand(query, conn)
                    Dim adapter As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    adapter.Fill(dt)
                    dgvCustomer.DataSource = dt

                    FormatGrid()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading customers: " & ex.Message)
        End Try
    End Sub

    ' ✅ Format grid
    Private Sub FormatGrid()
        With dgvCustomer
            .Columns("id").Visible = False   ' hide primary key
            .Columns("Customer Name").Width = 200
            .Columns("Address").Width = 250
            .Columns("Created At").Width = 150
            .Columns("Updated At").Width = 150

            ' General styles
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .RowHeadersVisible = False
        End With
    End Sub



    ' ✅ Refresh button
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadCustomers()
    End Sub


    'For add customer button
    Private Sub btnAddCustomer_Click(sender As Object, e As EventArgs) Handles btnAddCustomer.Click
        Dim f As New frmAddCustomer()
        If f.ShowDialog() = DialogResult.OK Then
            LoadCustomers() ' Refresh grid after adding product
        End If
    End Sub


    'For updating customer
    Private Sub btnEditCustomer_Click(sender As Object, e As EventArgs) Handles btnEditCustomer.Click
        If dgvCustomer.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a customer to edit.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Get selected Customer ID
        Dim customerId As Integer = Convert.ToInt32(dgvCustomer.SelectedRows(0).Cells("id").Value)

        ' Open edit form
        Dim editForm As New frmEditCustomer(customerId)
        If editForm.ShowDialog() = DialogResult.OK Then
            LoadCustomers() ' 🔄 Refresh DataGridView after update
        End If
    End Sub


    ' ✅ Delete customer button
    Private Sub btnDeleteCustomer_Click(sender As Object, e As EventArgs) Handles btnDeleteCustomer.Click
        If dgvCustomer.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a customer to delete.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Get selected customer ID and name
        Dim customerId As Integer = Convert.ToInt32(dgvCustomer.SelectedRows(0).Cells("id").Value)
        Dim customerName As String = dgvCustomer.SelectedRows(0).Cells("Customer Name").Value.ToString()

        ' Confirm deletion
        Dim confirm As DialogResult = MessageBox.Show($"Are you sure you want to delete customer '{customerName}'?",
                                                  "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirm <> DialogResult.Yes Then
            Return
        End If

        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                ' 🔹 Check if customer is referenced in sales_transaction
                Dim checkQuery As String = "SELECT COUNT(*) FROM sales_transaction WHERE customer_id=@id"
                Using checkCmd As New MySqlCommand(checkQuery, conn)
                    checkCmd.Parameters.AddWithValue("@id", customerId)
                    Dim refCount As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                    If refCount > 0 Then
                        MessageBox.Show("This customer cannot be deleted because it is already referenced in transactions.",
                                    "Delete Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                End Using

                ' 🔹 Proceed with delete
                Dim deleteQuery As String = "DELETE FROM CUSTOMER WHERE id=@id"
                Using deleteCmd As New MySqlCommand(deleteQuery, conn)
                    deleteCmd.Parameters.AddWithValue("@id", customerId)
                    deleteCmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Customer deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadCustomers() ' 🔄 Refresh DataGridView

        Catch ex As Exception
            MessageBox.Show("Error deleting customer: " & ex.Message)
        End Try
    End Sub


End Class
