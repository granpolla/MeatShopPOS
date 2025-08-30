Imports System.Text.RegularExpressions
Imports MySql.Data.MySqlClient

Public Class frmCashierPaymentInput

    Private Sub frmCashierPaymentInput_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Populate payment method combo
        cboPaymentMethod.Items.Clear()
        cboPaymentMethod.Items.Add("cash")
        cboPaymentMethod.Items.Add("online")
        cboPaymentMethod.SelectedIndex = 0

        ' Setup DataGridView columns
        If dgvPaymentEntries.Columns.Count = 0 Then
            dgvPaymentEntries.Columns.Add("Method", "Payment Mode")
            dgvPaymentEntries.Columns.Add("RefNum", "Ref No.")
            dgvPaymentEntries.Columns.Add("Amount", "Amount")
            dgvPaymentEntries.Columns("Amount").DefaultCellStyle.Format = "N2"
        End If

        dgvPaymentEntries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvPaymentEntries.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvPaymentEntries.ReadOnly = True
        dgvPaymentEntries.MultiSelect = False

        txtChange.ReadOnly = True
        txtRefNum.Enabled = (cboPaymentMethod.SelectedItem.ToString() = "online")
    End Sub

    ' Enable RefNum only when Online is selected
    Private Sub cboPaymentMethod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPaymentMethod.SelectedIndexChanged
        If cboPaymentMethod.SelectedItem.ToString() = "online" Then
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
    ' ✅ Add Payment Entry
    Private Sub btnAddPayment_Click(sender As Object, e As EventArgs) Handles btnAddPayment.Click
        Dim method As String = cboPaymentMethod.SelectedItem.ToString().Trim().ToLower()
        Dim refNum As String = txtRefNum.Text.Trim()
        Dim amountText As String = txtAmount.Text.Trim()

        ' Amount validation
        If Not ValidateAmountInput(amountText) Then
            MessageBox.Show("Please enter a valid amount greater than 0.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' RefNum validation for Online
        If method = "online" AndAlso String.IsNullOrWhiteSpace(refNum) Then
            MessageBox.Show("Reference number is required for online payments.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' ✅ Payment method combination rules
        Dim cashCount As Integer = 0
        Dim onlineCount As Integer = 0
        For Each row As DataGridViewRow In dgvPaymentEntries.Rows
            If row.Cells("Method").Value IsNot Nothing Then
                Dim existingMethod = row.Cells("Method").Value.ToString().Trim().ToLower()
                If existingMethod = "cash" Then cashCount += 1
                If existingMethod = "online" Then onlineCount += 1
            End If
        Next

        ' Reject duplicates of same method
        If method = "cash" AndAlso cashCount >= 1 Then
            MessageBox.Show("You can only add one CASH payment.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If method = "online" AndAlso onlineCount >= 1 Then
            MessageBox.Show("You can only add one ONLINE payment.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' ✅ Allow max 1 cash + 1 online (do NOT block the valid combination)
        If cashCount >= 1 AndAlso onlineCount >= 1 Then
            MessageBox.Show("You can only have one CASH and one ONLINE payment in total.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If


        ' ✅ Passed → Add row
        dgvPaymentEntries.Rows.Add(method, refNum, Convert.ToDecimal(amountText).ToString("N2"))

        ' Clear inputs
        txtAmount.Clear()
        txtRefNum.Clear()
        cboPaymentMethod.SelectedIndex = 0

        ' Update change
        UpdateChange()
    End Sub

    ' ✅ Clear all entries
    Private Sub btnClearDgvPaymentEntries_Click(sender As Object, e As EventArgs) Handles btnClearDgvPaymentEntries.Click
        dgvPaymentEntries.Rows.Clear()
        txtChange.Clear()
    End Sub

    Private Sub btnSaveAndPrint_Click(sender As Object, e As EventArgs) Handles btnSaveAndPrint.Click
        ' ✅ Get grand total + customer info from Dashboard
        Dim parentForm As frmCashierDashboard = CType(Me.ParentForm, frmCashierDashboard)

        If parentForm.dgvOrderItemPreview.DataSource Is Nothing OrElse
       CType(parentForm.dgvOrderItemPreview.DataSource, DataTable).Rows.Count = 0 Then
            MessageBox.Show("No order items added. Please add products before saving and printing.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If dgvPaymentEntries.Rows.Count = 0 Then
            MessageBox.Show("No payment entries to save.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' ✅ Get grand total + customer info
        Dim grandTotal As Decimal = 0
        Decimal.TryParse(parentForm.txtGrandTotal.Text, grandTotal)

        Dim firstName As String = ParentForm.txtFirstName.Text.Trim()
        Dim lastName As String = ParentForm.txtLastName.Text.Trim()
        Dim address As String = parentForm.txtCustomerAddress.Text.Trim()

        ' 🔹 Validate customer fields
        If String.IsNullOrWhiteSpace(firstName) OrElse String.IsNullOrWhiteSpace(lastName) OrElse String.IsNullOrWhiteSpace(address) Then
            MessageBox.Show("Customer information is incomplete. Please make sure First Name, Last Name, and Address are filled.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' 🔹 Check if this customer exists in the database
        Dim exists As Boolean = False
        Dim customerID As Integer = -1
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                Dim query As String = "
            SELECT id 
            FROM customer 
            WHERE LOWER(TRIM(REPLACE(customer_name,' ',''))) = LOWER(REPLACE(@name,' ','')) 
              AND LOWER(TRIM(address)) = LOWER(@address)
            LIMIT 1"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@name", (firstName & " " & lastName).Replace(" ", ""))
                    cmd.Parameters.AddWithValue("@address", address.Trim().ToLower())

                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        exists = True
                        customerID = Convert.ToInt32(result)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error checking customer: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        If Not exists Then
            MessageBox.Show("This customer is not yet saved in the database. Please save the customer before proceeding.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' 🔹 Compute total paid
        Dim totalPaid As Decimal = 0
        For Each row As DataGridViewRow In dgvPaymentEntries.Rows
            If row.Cells("Amount").Value IsNot Nothing Then
                totalPaid += Convert.ToDecimal(row.Cells("Amount").Value)
            End If
        Next

        ' 🔹 Validate sufficiency
        If totalPaid < grandTotal Then
            MessageBox.Show($"Insufficient payment! Grand Total is {grandTotal:N2}, but only {totalPaid:N2} was paid.",
                        "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' ✅ Passed all validations → simulate Save + Print
        Dim details As String = "Payments Entered:" & Environment.NewLine
        For Each row As DataGridViewRow In dgvPaymentEntries.Rows
            details &= $"{row.Cells("Method").Value} | {row.Cells("RefNum").Value} | {row.Cells("Amount").Value}" & Environment.NewLine
        Next
        details &= Environment.NewLine & $"Customer: {firstName} {lastName}, {address}" & Environment.NewLine
        details &= $"Grand Total: {grandTotal:N2}" & Environment.NewLine
        details &= $"Total Paid: {totalPaid:N2}" & Environment.NewLine
        details &= $"Change: {(totalPaid - grandTotal):N2}"

        MessageBox.Show(details, "Save + Print (Simulation)", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ' ✅ Clear after save
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
