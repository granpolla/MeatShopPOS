Imports System.Text.RegularExpressions

Public Class frmCashierPaymentInput

    Private Sub frmCashierPaymentInput_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Populate payment method combo
        cboPaymentMethod.Items.Clear()
        cboPaymentMethod.Items.Add("Cash")
        cboPaymentMethod.Items.Add("Online")
        cboPaymentMethod.SelectedIndex = 0

        ' Setup DataGridView columns
        If dgvPaymentEntries.Columns.Count = 0 Then
            dgvPaymentEntries.Columns.Add("Method", "Payment Method")
            dgvPaymentEntries.Columns.Add("RefNum", "Reference No.")
            dgvPaymentEntries.Columns.Add("Amount", "Amount")
            dgvPaymentEntries.Columns("Amount").DefaultCellStyle.Format = "N2"
        End If

        dgvPaymentEntries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvPaymentEntries.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvPaymentEntries.ReadOnly = True
        dgvPaymentEntries.MultiSelect = False

        txtChange.ReadOnly = True
        txtRefNum.Enabled = (cboPaymentMethod.SelectedItem.ToString() = "Online")
    End Sub

    ' Enable RefNum only when Online is selected
    Private Sub cboPaymentMethod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPaymentMethod.SelectedIndexChanged
        If cboPaymentMethod.SelectedItem.ToString() = "Online" Then
            txtRefNum.Enabled = True
        Else
            txtRefNum.Clear()
            txtRefNum.Enabled = False
        End If
    End Sub

    ' Validate only numbers > 0 in Amount
    Private Function ValidateAmountInput(amountText As String) As Boolean
        Dim amount As Decimal
        If Decimal.TryParse(amountText, amount) Then
            Return amount > 0
        End If
        Return False
    End Function

    ' ✅ Add Payment Entry
    Private Sub btnAddPayment_Click(sender As Object, e As EventArgs) Handles btnAddPayment.Click
        Dim method As String = cboPaymentMethod.SelectedItem.ToString()
        Dim refNum As String = txtRefNum.Text.Trim()
        Dim amountText As String = txtAmount.Text.Trim()

        ' Validation
        If Not ValidateAmountInput(amountText) Then
            MessageBox.Show("Please enter a valid amount greater than 0.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If method = "Online" AndAlso String.IsNullOrWhiteSpace(refNum) Then
            MessageBox.Show("Reference number is required for online payments.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Add row
        Dim rowIndex As Integer = dgvPaymentEntries.Rows.Add(method, refNum, Convert.ToDecimal(amountText).ToString("N2"))

        ' Clear inputs
        txtAmount.Clear()
        txtRefNum.Clear()
        cboPaymentMethod.SelectedIndex = 0

        ' Recalculate change (just test mode: assume txtGrandTotal exists in Dashboard)
        UpdateChange()
    End Sub

    ' ✅ Clear all entries
    Private Sub btnClearDgvPaymentEntries_Click(sender As Object, e As EventArgs) Handles btnClearDgvPaymentEntries.Click
        dgvPaymentEntries.Rows.Clear()
        txtChange.Clear()
    End Sub

    ' For now just simulate save + print
    Private Sub btnSaveAndPrint_Click(sender As Object, e As EventArgs) Handles btnSaveAndPrint.Click
        If dgvPaymentEntries.Rows.Count = 0 Then
            MessageBox.Show("No payment entries to save.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' ✅ Get grand total from Dashboard
        Dim parentForm As frmCashierDashboard = CType(Me.ParentForm, frmCashierDashboard)
        Dim grandTotal As Decimal = 0
        Decimal.TryParse(parentForm.txtGrandTotal.Text, grandTotal)

        ' ✅ Compute total paid
        Dim totalPaid As Decimal = 0
        For Each row As DataGridViewRow In dgvPaymentEntries.Rows
            totalPaid += Convert.ToDecimal(row.Cells("Amount").Value)
        Next

        ' ✅ Validate payment sufficiency
        If totalPaid < grandTotal Then
            MessageBox.Show($"Insufficient payment! Grand Total is {grandTotal:N2}, but only {totalPaid:N2} was paid.",
                        "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' For now just simulate save + print
        Dim details As String = "Payments Entered:" & Environment.NewLine
        For Each row As DataGridViewRow In dgvPaymentEntries.Rows
            details &= $"{row.Cells("Method").Value} | {row.Cells("RefNum").Value} | {row.Cells("Amount").Value}" & Environment.NewLine
        Next
        details &= Environment.NewLine & $"Grand Total: {grandTotal:N2}" & Environment.NewLine & $"Total Paid: {totalPaid:N2}" & Environment.NewLine & $"Change: {(totalPaid - grandTotal):N2}"

        MessageBox.Show(details, "Save + Print (Simulation)", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ' Clear after save
        dgvPaymentEntries.Rows.Clear()
        txtChange.Clear()
    End Sub


    ' 🔹 Simple change calculator (GrandTotal - payments)
    Private Sub UpdateChange()
        ' Get grand total from Dashboard
        Dim parentForm As frmCashierDashboard = CType(Me.ParentForm, frmCashierDashboard)
        Dim grandTotal As Decimal = 0
        Decimal.TryParse(parentForm.txtGrandTotal.Text, grandTotal)

        Dim paid As Decimal = 0
        For Each row As DataGridViewRow In dgvPaymentEntries.Rows
            paid += Convert.ToDecimal(row.Cells("Amount").Value)
        Next

        Dim change As Decimal = paid - grandTotal
        If change >= 0 Then
            txtChange.Text = change.ToString("N2")
        Else
            txtChange.Text = "0.00"
        End If
    End Sub

End Class
