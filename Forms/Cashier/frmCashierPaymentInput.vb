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
        dgvPaymentEntries.RowHeadersVisible = False
        dgvPaymentEntries.ScrollBars = ScrollBars.None

        txtChange.ReadOnly = True
        txtRefNum.Enabled = (cboPaymentMethod.SelectedItem.ToString() = "online")
    End Sub

    ' 🔹 Build Transaction Summary String
    Private Function BuildTransactionSummary(parentForm As frmCashierDashboard,
                                         grandTotal As Decimal,
                                         totalPaid As Decimal,
                                         status As String,
                                         unpaidBalance As Decimal,
                                         customerID As Integer,
                                         firstName As String,
                                         lastName As String,
                                         address As String) As String

        Dim details As String = "=== ORDER ITEMS ===" & Environment.NewLine

        ' 👉 Order Items
        Dim orderTable As DataTable = CType(parentForm.dgvOrderItemPreview.DataSource, DataTable)
        For Each row As DataRow In orderTable.Rows
            details &= $"{row("Product Name")} | {row("Brand")} | " &
                   $"Unit: {Convert.ToDecimal(row("Unit Weight")):N2}kg @ {Convert.ToDecimal(row("Unit Price")):N2} | " &
                   $"Box: {Convert.ToDecimal(row("Total Box")):N0} | " &
                   $"Weight: {Convert.ToDecimal(row("Total Weight")):N2}kg | " &
                   $"Total: {Convert.ToDecimal(row("Total")):N2}" & Environment.NewLine
        Next

        ' 👉 Settled Balances
        Dim balanceTotal As Decimal = 0
        details &= Environment.NewLine & "=== SETTLED BALANCES ===" & Environment.NewLine
        If parentForm.dgvCustomerBalancePreview.SelectedRows.Count > 0 Then
            For Each row As DataGridViewRow In parentForm.dgvCustomerBalancePreview.SelectedRows
                details &= $"{CDate(row.Cells("Date").Value):yyyy-MM-dd} | " &
                       $"{row.Cells("Description").Value} | " &
                       $"{Convert.ToDecimal(row.Cells("Balance").Value):N2}" & Environment.NewLine
                balanceTotal += Convert.ToDecimal(row.Cells("Balance").Value)
            Next
        Else
            details &= "No balances settled (0.00)" & Environment.NewLine
        End If

        ' 👉 Payments
        details &= Environment.NewLine & "=== PAYMENTS ENTERED ===" & Environment.NewLine
        For Each row As DataGridViewRow In dgvPaymentEntries.Rows
            details &= $"{row.Cells("Method").Value} | {row.Cells("RefNum").Value} | {row.Cells("Amount").Value}" & Environment.NewLine
        Next

        ' 👉 Customer + Totals
        details &= Environment.NewLine & $"Customer: {firstName} {lastName}, {address}" & Environment.NewLine
        details &= $"Order Subtotal: {(grandTotal - balanceTotal):N2}" & Environment.NewLine
        details &= $"Settled Balances: {balanceTotal:N2}" & Environment.NewLine
        details &= $"Grand Total: {grandTotal:N2}" & Environment.NewLine
        details &= $"Total Paid: {totalPaid:N2}" & Environment.NewLine
        details &= $"Change: {Math.Max(totalPaid - grandTotal, 0):N2}" & Environment.NewLine
        details &= $"Status: {status}" & Environment.NewLine

        If status = "Partial" Then
            details &= $"Unpaid Balance (to ledger): {unpaidBalance:N2}" & Environment.NewLine
        End If

        Return details
    End Function

    ' 🔹 Show Transaction Summary in MessageBox
    Private Sub ShowTransactionSummary(details As String)
        MessageBox.Show(details, "Save + Print", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
        Dim parentForm As frmCashierDashboard = CType(Me.ParentForm, frmCashierDashboard)

        ' ================================
        ' 🔹 SECTION 1: VALIDATE ORDER & PAYMENTS
        ' ================================
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

        ' ================================
        ' 🔹 SECTION 2: CUSTOMER INFO VALIDATION
        ' ================================
        Dim grandTotal As Decimal = 0
        Decimal.TryParse(parentForm.txtGrandTotal.Text, grandTotal)

        Dim firstName As String = parentForm.txtFirstName.Text.Trim()
        Dim lastName As String = parentForm.txtLastName.Text.Trim()
        Dim address As String = parentForm.txtCustomerAddress.Text.Trim()

        If String.IsNullOrWhiteSpace(firstName) OrElse String.IsNullOrWhiteSpace(lastName) OrElse String.IsNullOrWhiteSpace(address) Then
            MessageBox.Show("Customer information is incomplete. Please make sure First Name, Last Name, and Address are filled.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' ================================
        ' 🔹 SECTION 3: CHECK IF CUSTOMER EXISTS IN DATABASE
        ' ================================
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

        ' ================================
        ' 🔹 SECTION 4: PAYMENT CALCULATIONS & VALIDATION
        ' ================================
        ' 👉 Compute total paid
        Dim totalPaid As Decimal = 0
        For Each row As DataGridViewRow In dgvPaymentEntries.Rows
            If row.Cells("Amount").Value IsNot Nothing Then
                totalPaid += Convert.ToDecimal(row.Cells("Amount").Value)
            End If
        Next

        ' 👉 Require minimum payment
        If totalPaid < 100 Then
            MessageBox.Show("Minimum payment required is 100. Please enter a valid payment before saving.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' 👉 Payment status
        Dim status As String = "Fully Paid"
        Dim unpaidBalance As Decimal = 0

        If totalPaid < grandTotal Then
            unpaidBalance = grandTotal - totalPaid

            ' 🚫 Rule: Cannot partially pay if previous balances are selected
            If parentForm.dgvCustomerBalancePreview.SelectedRows.Count > 0 Then
                MessageBox.Show("Partial payment is not allowed when settling previous balances." &
                            Environment.NewLine & Environment.NewLine &
                            "👉 Either pay the FULL AMOUNT (Order + Previous Balance)" & Environment.NewLine &
                            "👉 Or just pay for the NEW ORDER only and leave the previous balance for now.",
                            "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' ✅ Otherwise allow partial, with confirmation
            Dim confirmResult = MessageBox.Show(
            $"Payment is insufficient!" & Environment.NewLine &
            $"Grand Total: {grandTotal:N2}" & Environment.NewLine &
            $"Paid: {totalPaid:N2}" & Environment.NewLine &
            $"Unpaid Balance: {unpaidBalance:N2}" & Environment.NewLine &
            Environment.NewLine & "Do you want to proceed and mark this as PARTIAL?",
            "Partial Payment Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If confirmResult = DialogResult.No Then
                Return
            End If

            status = "Partial"
        End If

        ' ================================
        ' 🔹 SECTION 5: SAVE TO DATABASE
        ' ================================
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()
                Using tx As MySqlTransaction = conn.BeginTransaction()

                    ' 🔹 Generate unique order number
                    Dim orderNumber As String = ""
                    Dim rand As New Random()
                    Dim existsOrd As Boolean = True
                    While existsOrd
                        Dim randomNum As Integer = rand.Next(1000, 9999)
                        orderNumber = $"ORD-{randomNum}-{modSession.LoggedInUsername.ToUpper()}"
                        Using cmdCheck As New MySqlCommand("SELECT COUNT(*) FROM sales_transaction WHERE order_number=@ord", conn, tx)
                            cmdCheck.Parameters.AddWithValue("@ord", orderNumber)
                            existsOrd = (Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0)
                        End Using
                    End While

                    ' 🔹 Normalize names to match DB values (handles spaces vs underscores)
                    Dim statusKey As String = ""
                    Select Case status.Trim().ToLower()
                        Case "full", "fully paid", "paid"
                            statusKey = "full"
                        Case "partial", "partially paid"
                            statusKey = "partial"
                        Case Else
                            MessageBox.Show("Unknown payment status: " & status, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return
                    End Select

                    ' 🔹 Insert sales_transaction
                    Dim transId As Integer
                    Using cmd As New MySqlCommand("
                INSERT INTO sales_transaction
                    (order_number, user_id, customer_id, total_amount, payment_status_id, amount_paid)
                VALUES
                    (
                        @ord, @uid, @cid, @total,
                        (SELECT id FROM payment_status WHERE LOWER(payment_status_name) = @status_key LIMIT 1),
                        @paid
                    );", conn, tx)
                        cmd.Parameters.AddWithValue("@ord", orderNumber)
                        cmd.Parameters.AddWithValue("@uid", modSession.LoggedInUserID)
                        cmd.Parameters.AddWithValue("@cid", customerID)
                        cmd.Parameters.AddWithValue("@total", grandTotal)
                        cmd.Parameters.AddWithValue("@status_key", statusKey) ' will be "full" or "partial"
                        cmd.Parameters.AddWithValue("@paid", totalPaid)
                        cmd.ExecuteNonQuery()
                    End Using

                    ' Get last insert id safely
                    Using cmdGetId As New MySqlCommand("SELECT LAST_INSERT_ID();", conn, tx)
                        transId = Convert.ToInt32(cmdGetId.ExecuteScalar())
                    End Using

                    ' 🔹 Insert transaction items
                    Dim orderTable As DataTable = CType(parentForm.dgvOrderItemPreview.DataSource, DataTable)
                    For Each r As DataRow In orderTable.Rows
                        Using cmd As New MySqlCommand("
                INSERT INTO transaction_item
                    (transaction_id, product_id, number_of_box, unit_weight_kg, unit_price_php, total_weight_kg, subtotal)
                VALUES
                    (@tid, @pid, @box, @uw, @up, @tw, @sub);", conn, tx)
                            cmd.Parameters.AddWithValue("@tid", transId)
                            cmd.Parameters.AddWithValue("@pid", Convert.ToInt32(r("ProductID")))
                            cmd.Parameters.AddWithValue("@box", Convert.ToDecimal(r("Total Box")))
                            cmd.Parameters.AddWithValue("@uw", Convert.ToDecimal(r("Unit Weight")))
                            cmd.Parameters.AddWithValue("@up", Convert.ToDecimal(r("Unit Price")))
                            cmd.Parameters.AddWithValue("@tw", Convert.ToDecimal(r("Total Weight")))
                            cmd.Parameters.AddWithValue("@sub", Convert.ToDecimal(r("Total")))
                            cmd.ExecuteNonQuery()
                        End Using
                    Next

                    ' 🔹 Insert payments
                    For Each row As DataGridViewRow In dgvPaymentEntries.Rows
                        If row.IsNewRow Then Continue For

                        Dim methodRaw As String = row.Cells("Method").Value.ToString()
                        Dim methodKey As String = methodRaw.ToLower().Replace(" "c, "_"c)
                        Dim refNumValue As Object = DBNull.Value

                        ' 🔹 Only require RefNum if NOT cash
                        If Not methodKey.Contains("cash") Then
                            If row.Cells("RefNum").Value IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(row.Cells("RefNum").Value.ToString()) Then
                                refNumValue = row.Cells("RefNum").Value
                            Else
                                MessageBox.Show($"Payment method '{methodRaw}' requires a Reference Number.",
                            "Validation Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
                                tx.Rollback()
                                Return
                            End If
                        End If

                        Using cmd As New MySqlCommand("
                        INSERT INTO payment_detail
                            (transaction_id, payment_method_id, ref_num, amount)
                        VALUES
                            (
                                @tid,
                                (SELECT id
                                   FROM payment_method
                                  WHERE REPLACE(LOWER(payment_method_name),' ','_') = @mkey
                                  LIMIT 1),
                                @ref, @amt
                            );", conn, tx)
                            cmd.Parameters.AddWithValue("@tid", transId)
                            cmd.Parameters.AddWithValue("@mkey", methodKey)
                            cmd.Parameters.AddWithValue("@ref", refNumValue)  ' ✅ CASH → NULL, OTHERS → value
                            cmd.Parameters.AddWithValue("@amt", Convert.ToDecimal(row.Cells("Amount").Value))
                            cmd.ExecuteNonQuery()
                        End Using
                    Next


                    ' 🔹 Ledger entry for NEW order
                    If status = "Partial" AndAlso unpaidBalance > 0 Then
                        Using cmd As New MySqlCommand("
                        INSERT INTO customer_ledger
                            (customer_id, transaction_id, related_transaction_id, description, amount)
                        VALUES
                            (@cid, @tid, NULL, @desc, @amt);", conn, tx)
                            cmd.Parameters.AddWithValue("@cid", customerID)
                            cmd.Parameters.AddWithValue("@tid", transId)
                            cmd.Parameters.AddWithValue("@desc", $"Balance from purchase {orderNumber}")
                            cmd.Parameters.AddWithValue("@amt", unpaidBalance)
                            cmd.ExecuteNonQuery()
                        End Using
                    End If


                    ' 🔹 Ledger entries for SETTLED balances
                    If parentForm.dgvCustomerBalancePreview.SelectedRows.Count > 0 Then
                        For Each row As DataGridViewRow In parentForm.dgvCustomerBalancePreview.SelectedRows
                            Dim oldTransId As Integer = Convert.ToInt32(row.Cells("TransactionID").Value)
                            Dim oldOrderNum As String = ""

                            ' Get the old order number
                            Using cmdFetch As New MySqlCommand("SELECT order_number FROM sales_transaction WHERE id=@rid LIMIT 1;", conn, tx)
                                cmdFetch.Parameters.AddWithValue("@rid", oldTransId)
                                oldOrderNum = Convert.ToString(cmdFetch.ExecuteScalar())
                            End Using

                            Dim descText As String = $"Paid old balance from {oldOrderNum} during {orderNumber}"

                            Using cmd As New MySqlCommand("
                                INSERT INTO customer_ledger
                                    (customer_id, transaction_id, related_transaction_id, description, amount)
                                VALUES
                                    (@cid, @tid, @rid, @desc, @amt);", conn, tx)
                                cmd.Parameters.AddWithValue("@cid", customerID)
                                cmd.Parameters.AddWithValue("@tid", transId)
                                cmd.Parameters.AddWithValue("@rid", oldTransId)
                                cmd.Parameters.AddWithValue("@desc", descText)
                                cmd.Parameters.AddWithValue("@amt", Convert.ToDecimal(row.Cells("Balance").Value))
                                cmd.ExecuteNonQuery()
                            End Using
                        Next

                    End If

                    ' ✅ Commit 
                    tx.Commit()
                End Using
            End Using

            ' ================================
            ' 🔹 SECTION 6: SHOW SUMMARY (USING HELPER)
            ' ================================
            Dim details As String = BuildTransactionSummary(parentForm, grandTotal, totalPaid, status, unpaidBalance, customerID, firstName, lastName, address)
            ShowTransactionSummary(details)

            ' Cleanup
            dgvPaymentEntries.Rows.Clear()
            txtChange.Clear()

        Catch ex As Exception
            MessageBox.Show("Error saving transaction: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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
