Imports MySql.Data.MySqlClient

Public Class frmProduct

    Private Sub frmProduct_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadProducts()
    End Sub

    ' ✅ Load products into DataGridView
    Private Sub LoadProducts()
        Dim query As String = "
            SELECT id, product_name AS 'Product Name',
                   brand AS 'Brand',
                   unit_weight_kg AS 'Unit Weight (kg)',
                   unit_price_php AS 'Price (₱)',
                   created_at AS 'Created At'
            FROM PRODUCT
            ORDER BY created_at DESC"
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                Using cmd As New MySqlCommand(query, conn)
                    Dim adapter As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    adapter.Fill(dt)
                    dgvProducts.DataSource = dt

                    FormatGrid()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading products: " & ex.Message)
        End Try
    End Sub

    ' ✅ Format grid
    Private Sub FormatGrid()
        With dgvProducts
            .Columns("id").Visible = False   ' hide primary key
            .Columns("Product Name").Width = 200
            .Columns("Brand").Width = 150
            .Columns("Unit Weight (kg)").Width = 100
            .Columns("Price (₱)").Width = 100
            .Columns("Created At").Width = 150

            ' Make read-only
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
        End With
    End Sub

    ' ✅ Refresh button
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadProducts()
    End Sub

    ' Add product
    Private Sub btnAddProduct_Click(sender As Object, e As EventArgs) Handles btnAddProduct.Click
        Dim f As New frmAddProduct()
        If f.ShowDialog() = DialogResult.OK Then
            LoadProducts() ' Refresh grid after adding product
        End If
    End Sub

End Class
