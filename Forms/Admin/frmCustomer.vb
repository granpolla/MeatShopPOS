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
End Class
