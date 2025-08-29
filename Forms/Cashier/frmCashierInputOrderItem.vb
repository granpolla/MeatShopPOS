Public Class frmCashierInputOrderItem

    Private Sub frmCashierInputOrderItem_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    ' ✅ Called by Dashboard when product row is clicked
    Public Sub FillProductDetails(pName As String, pBrand As String, weight As Decimal, price As Decimal)
        txtProductName.Text = pName
        txtProductBrand.Text = pBrand
        txtUnitWeight.Text = weight.ToString()
        txtUnitPrice.Text = price.ToString("N2")
        txtTotal.Text = price.ToString("N2") ' default for 1 weight
    End Sub





    Private Sub txtTotalWeightPurchase_TextChanged(sender As Object, e As EventArgs) Handles txtTotalWeightPurchase.TextChanged
        Dim unitPrice As Decimal
        Dim weightPurchase As Decimal

        Decimal.TryParse(txtUnitPrice.Text, unitPrice)
        Decimal.TryParse(txtTotalWeightPurchase.Text, weightPurchase)

        Dim total As Decimal = unitPrice * weightPurchase
        txtTotal.Text = total.ToString("N2")
    End Sub






    ' ✅ Search product by name (starts with)
    Private Sub btnSearchProduct_Click(sender As Object, e As EventArgs) Handles btnSearchProduct.Click
        Dim searchText As String = txtProductName.Text.Trim()

        If String.IsNullOrWhiteSpace(searchText) Then
            MessageBox.Show("Please type a product name to search.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Call frmCashierDashboard to update dgvProductsPreview
        Dim parentForm As frmCashierDashboard = CType(Me.ParentForm, frmCashierDashboard)
        parentForm.LoadProductsPreview(searchText)
    End Sub

End Class
