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


    ' Edit product
    Private Sub btnEditProduct_Click(sender As Object, e As EventArgs) Handles btnEditProduct.Click
        If dgvProducts.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a product to edit.", "Edit Product", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Get selected row
        Dim row As DataGridViewRow = dgvProducts.SelectedRows(0)
        Dim productId As Integer = Convert.ToInt32(row.Cells("id").Value)
        Dim productName As String = row.Cells("Product Name").Value.ToString()
        Dim brand As String = row.Cells("Brand").Value.ToString()
        Dim weight As Decimal = Convert.ToDecimal(row.Cells("Unit Weight (kg)").Value)
        Dim price As Decimal = Convert.ToDecimal(row.Cells("Price (₱)").Value)

        ' Open frmEditProduct and pass values
        Dim f As New frmEditProduct()
        f.ProductID = productId
        f.txtProductName.Text = productName
        f.txtBrand.Text = brand
        f.txtWeight.Text = weight.ToString()
        f.txtPrice.Text = price.ToString()

        If f.ShowDialog() = DialogResult.OK Then
            LoadProducts() ' Refresh grid after updating
        End If
    End Sub


    ' Delete product
    Private Sub btnDeleteProduct_Click(sender As Object, e As EventArgs) Handles btnDeleteProduct.Click
        If dgvProducts.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a product to delete.", "Delete Product", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Get selected product details
        Dim row As DataGridViewRow = dgvProducts.SelectedRows(0)
        Dim productId As Integer = Convert.ToInt32(row.Cells("id").Value)
        Dim productName As String = row.Cells("Product Name").Value.ToString()
        Dim brand As String = row.Cells("Brand").Value.ToString()

        ' Ask for confirmation
        Dim result As DialogResult = MessageBox.Show(
            $"Are you sure you want to delete this product?" & vbCrLf &
            $"Product: {productName}" & vbCrLf &
            $"Brand: {brand}",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        )

        If result = DialogResult.Yes Then
            Try
                Using conn As New MySqlConnection(My.Settings.DBConnection)
                    conn.Open()
                    Dim query As String = "DELETE FROM PRODUCT WHERE id=@id"
                    Using cmd As New MySqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@id", productId)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                MessageBox.Show("Product deleted successfully.", "Delete Product", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadProducts() ' ✅ Refresh grid
            Catch ex As MySqlException
                ' Check if error is due to foreign key constraint
                If ex.Number = 1451 Then ' Cannot delete or update a parent row: a foreign key constraint fails
                    MessageBox.Show("This product cannot be deleted because it is already used in a transaction.", "Delete Blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Else
                    MessageBox.Show("Error deleting product: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Catch ex As Exception
                MessageBox.Show("Unexpected error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub




End Class
