Public Class frmCashierInputOrderItem

    Private Sub frmCashierInputOrderItem_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ✅ Make some textboxes read-only
        txtProductBrand.ReadOnly = True
        txtUnitWeight.ReadOnly = True
        txtUnitPrice.ReadOnly = True
        txtTotal.ReadOnly = True
    End Sub

    ' ✅ Called by Dashboard when product row is clicked
    Public Sub FillProductDetails(pName As String, pBrand As String, weight As Decimal, price As Decimal)
        txtProductName.Text = pName
        txtProductBrand.Text = pBrand
        txtUnitWeight.Text = weight.ToString()
        txtUnitPrice.Text = price.ToString("N2")
        txtTotal.Text = price.ToString("N2") ' default for 1 weight
    End Sub

    ' ✅ Auto-calc total when weight changes
    Private Sub txtTotalWeightPurchase_TextChanged(sender As Object, e As EventArgs) Handles txtTotalWeightPurchase.TextChanged
        Dim unitPrice As Decimal
        Dim weightPurchase As Decimal

        Decimal.TryParse(txtUnitPrice.Text, unitPrice)
        Decimal.TryParse(txtTotalWeightPurchase.Text, weightPurchase)

        If weightPurchase > 0 Then
            Dim total As Decimal = unitPrice * weightPurchase
            txtTotal.Text = total.ToString("N2")
        Else
            txtTotal.Text = "0.00"
        End If
    End Sub

    ' ✅ Allow only positive numbers (no 0, no letters) in numeric inputs
    Private Sub txtTotalBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTotalBox.KeyPress
        ValidateNumericInput(e, txtTotalBox)
    End Sub

    Private Sub txtTotalWeightPurchase_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTotalWeightPurchase.KeyPress
        ValidateNumericInput(e, txtTotalWeightPurchase)
    End Sub

    Private Sub ValidateNumericInput(e As KeyPressEventArgs, txt As TextBox)
        ' Allow control keys (e.g., Backspace)
        If Char.IsControl(e.KeyChar) Then Return

        ' Allow digits only
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "."c Then
            e.Handled = True
            Return
        End If

        ' Prevent multiple decimals
        If e.KeyChar = "."c AndAlso txt.Text.Contains(".") Then
            e.Handled = True
            Return
        End If
    End Sub

    ' ✅ btnSearchProduct → call frmCashierDashboard
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

    ' ✅ btnRefresh → clear + reload
    Private Sub btnRefreshProduct_Click(sender As Object, e As EventArgs) Handles btnRefreshProduct.Click
        txtProductName.Clear()
        txtProductBrand.Clear()
        txtUnitWeight.Clear()
        txtUnitPrice.Clear()
        txtTotal.Clear()
        txtTotalBox.Clear()
        txtTotalWeightPurchase.Clear()

        ' Refresh DataGridView in frmCashierDashboard
        Dim parentForm As frmCashierDashboard = CType(Me.ParentForm, frmCashierDashboard)
        parentForm.LoadProductsPreview("") ' empty string = load all products
    End Sub

    ' ✅ btnAddItem → show values for now
    Private Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        ' Validation: must have numbers > 0
        Dim totalBox As Decimal
        Dim weightPurchase As Decimal

        If Not Decimal.TryParse(txtTotalBox.Text, totalBox) OrElse totalBox <= 0 Then
            MessageBox.Show("Enter a valid quantity greater than 0.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not Decimal.TryParse(txtTotalWeightPurchase.Text, weightPurchase) OrElse weightPurchase <= 0 Then
            MessageBox.Show("Enter a valid weight greater than 0.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' ✅ Just pop-up values for now
        MessageBox.Show(
            $"Product: {txtProductName.Text}" & Environment.NewLine &
            $"Brand: {txtProductBrand.Text}" & Environment.NewLine &
            $"Unit Weight: {txtUnitWeight.Text}" & Environment.NewLine &
            $"Unit Price: {txtUnitPrice.Text}" & Environment.NewLine &
            $"Weight Purchased: {txtTotalWeightPurchase.Text}" & Environment.NewLine &
            $"Quantity: {txtTotalBox.Text}" & Environment.NewLine &
            $"Total: {txtTotal.Text}",
            "Added Item Preview",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        )
    End Sub

End Class
