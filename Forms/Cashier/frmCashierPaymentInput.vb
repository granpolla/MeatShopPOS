Imports System.Text.RegularExpressions
Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Diagnostics

Public Class frmCashierPaymentInput

    Private Sub frmCashierPaymentInput_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Populate payment method combo
        cboPaymentMethod.Items.Clear()
        cboPaymentMethod.Items.Add("cash")
        cboPaymentMethod.Items.Add("online")
        cboPaymentMethod.SelectedIndex = 0

        ' 💥 Change DropDownStyle to prevent typing
        cboPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList

        ' Setup DataGridView columns
        If dgvPaymentEntries.Columns.Count = 0 Then
            dgvPaymentEntries.Columns.Add("Method", "Payment")
            dgvPaymentEntries.Columns.Add("RefNum", "Ref No.")
            dgvPaymentEntries.Columns.Add("Amount", "Amount")
            dgvPaymentEntries.Columns("Amount").DefaultCellStyle.Format = "N2"
        End If

        dgvPaymentEntries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvPaymentEntries.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvPaymentEntries.ReadOnly = True
        dgvPaymentEntries.AllowUserToResizeColumns = False
        dgvPaymentEntries.MultiSelect = False
        dgvPaymentEntries.RowHeadersVisible = False
        dgvPaymentEntries.ScrollBars = ScrollBars.None
        dgvPaymentEntries.Columns("Method").FillWeight = 55
        dgvPaymentEntries.Columns("Amount").FillWeight = 70


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

    ' ✅ Add Payment Entry
    Private Sub btnAddPayment_Click(sender As Object, e As EventArgs) Handles btnAddPayment.Click
        Dim method As String = cboPaymentMethod.SelectedItem.ToString().Trim().ToLower()
        Dim refNum As String = txtRefNum.Text.Trim()
        Dim amountText As String = txtAmount.Text.Trim()

        ' Amount validation (moved to PaymentHelpers)
        If Not PaymentHelpers.ValidateAmountInput(amountText) Then
            MessageBox.Show("Please enter a valid amount greater than 0.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' RefNum validation for Online
        If method = "online" AndAlso String.IsNullOrWhiteSpace(refNum) Then
            MessageBox.Show("Reference number is required for online payments.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' ✅ Payment method combination rules (use PaymentHelpers)
        Dim comboResult = PaymentHelpers.ValidatePaymentCombination(dgvPaymentEntries, method)
        If Not comboResult.Item1 Then
            MessageBox.Show(comboResult.Item2, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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

        If parentForm.dgvCustomerBalancePreview.SelectedRows.Count > 4 Then
            MessageBox.Show("You can only settle up to 4 balances per transaction. Please deselect some balances.",
                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' 1️# Validate order & payments
        If Not ValidateOrderAndPayments(parentForm) Then Exit Sub

        ' 2️# Validate customer & get ID
        Dim customerID As Integer = -1
        If Not ValidateCustomerInfo(parentForm, customerID) Then Exit Sub

        ' 3️# Payment + status
        Dim grandTotal As Decimal = Decimal.Parse(parentForm.txtGrandTotal.Text)
        Dim totalPaid As Decimal, unpaidBalance As Decimal, status As String = ""
        If Not ComputePaymentsAndStatus(parentForm, grandTotal, totalPaid, unpaidBalance, status) Then Exit Sub

        ' 4️# Show summary + confirm
        Dim details As String = TransactionSummaryModule.BuildTransactionSummary(
        CType(parentForm.dgvOrderItemPreview.DataSource, DataTable),
        parentForm.dgvCustomerBalancePreview,
        dgvPaymentEntries,
        grandTotal,
        totalPaid,
        status,
        unpaidBalance,
        parentForm.txtFirstName.Text,
        parentForm.txtLastName.Text,
        parentForm.txtCustomerAddress.Text)

        If MessageBox.Show(details & vbCrLf & vbCrLf & "Do you want to SAVE and PRINT this transaction?",
                   "Save + Print Confirmation",
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Question) <> DialogResult.OK Then Exit Sub

        ' 5️# Save to DB
        Try
            Dim orderNumber As String = ""

            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()
                Using tx As MySqlTransaction = conn.BeginTransaction()

                    ' 🔹 Generate unique order number
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

                    ' 🔹 Normalize names to match DB values
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
                    Dim invoicePdf As String = orderNumber & "-receipt"
                    Dim cashReceived As Decimal = totalPaid
                    Dim amountApplied As Decimal = Math.Min(totalPaid, grandTotal)
                    Dim changeGiven As Decimal = Math.Max(totalPaid - grandTotal, 0)

                    Using cmd As New MySqlCommand("
                            INSERT INTO sales_transaction
                                (order_number, user_id, customer_id, total_amount, payment_status_id, cash_received, amount_paid, change_given, invoice_pdf)
                            VALUES
                                (
                                    @ord, @uid, @cid, @total,
                                    (SELECT id FROM payment_status WHERE LOWER(payment_status_name) = @status_key LIMIT 1),
                                    @cash, @applied, @chg, @inv
                                );", conn, tx)

                        cmd.Parameters.AddWithValue("@ord", orderNumber)
                        cmd.Parameters.AddWithValue("@uid", modSession.LoggedInUserID)
                        cmd.Parameters.AddWithValue("@cid", customerID)
                        cmd.Parameters.AddWithValue("@total", grandTotal)
                        cmd.Parameters.AddWithValue("@status_key", statusKey)
                        cmd.Parameters.AddWithValue("@cash", cashReceived)
                        cmd.Parameters.AddWithValue("@applied", amountApplied)
                        cmd.Parameters.AddWithValue("@chg", changeGiven)
                        cmd.Parameters.AddWithValue("@inv", invoicePdf)
                        cmd.ExecuteNonQuery()
                    End Using

                    ' Get last insert id
                    Using cmdGetId As New MySqlCommand("SELECT LAST_INSERT_ID();", conn, tx)
                        transId = Convert.ToInt32(cmdGetId.ExecuteScalar())
                    End Using

                    ' 🔹 Insert transaction items
                    Dim orderTable As DataTable = CType(parentForm.dgvOrderItemPreview.DataSource, DataTable)


                    For Each r As DataRow In orderTable.Rows
                        Try
                            Using cmd As New MySqlCommand("
                                INSERT INTO transaction_item
                                    (transaction_id, product_id, number_of_box, unit_weight_kg, unit_price_php, total_weight_kg, subtotal)
                                VALUES
                                    (@tid, @pid, @box, @uw, @up, @tw, @sub);", conn, tx)

                                cmd.Parameters.AddWithValue("@tid", transId)
                                cmd.Parameters.AddWithValue("@pid", Convert.ToInt32(r("ProductID")))

                                ' Defensive column access (to prevent missing/renamed headers)
                                Dim boxVal As Decimal = If(orderTable.Columns.Contains("Total Box"), Convert.ToDecimal(r("Total Box")), 0D)
                                Dim unitWeightVal As Decimal = If(orderTable.Columns.Contains("Unit Weight"), Convert.ToDecimal(r("Unit Weight")), 0D)
                                Dim unitPriceVal As Decimal = If(orderTable.Columns.Contains("Unit Price"), Convert.ToDecimal(r("Unit Price")), 0D)
                                Dim totalWeightVal As Decimal = If(orderTable.Columns.Contains("Total Weight"), Convert.ToDecimal(r("Total Weight")), 0D)
                                Dim totalVal As Decimal = If(orderTable.Columns.Contains("Total"), Convert.ToDecimal(r("Total")), 0D)

                                cmd.Parameters.AddWithValue("@box", boxVal)
                                cmd.Parameters.AddWithValue("@uw", unitWeightVal)
                                cmd.Parameters.AddWithValue("@up", unitPriceVal)
                                cmd.Parameters.AddWithValue("@tw", totalWeightVal)
                                cmd.Parameters.AddWithValue("@sub", totalVal)

                                cmd.ExecuteNonQuery()
                            End Using
                        Catch ex As Exception
                            MessageBox.Show($"Error saving transaction item for product ID {r("ProductID")}: " & ex.Message,
                            "Transaction Item Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            tx.Rollback()
                            Return
                        End Try
                    Next

                    ' 🔹 Insert payments
                    For Each row As DataGridViewRow In dgvPaymentEntries.Rows
                        If row.IsNewRow Then Continue For

                        Dim methodRaw As String = row.Cells("Method").Value.ToString()
                        Dim methodKey As String = methodRaw.ToLower().Replace(" "c, "_"c)
                        Dim refNumValue As Object = DBNull.Value

                        ' Require RefNum if not cash
                        If Not methodKey.Contains("cash") Then
                            If row.Cells("RefNum").Value IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(row.Cells("RefNum").Value.ToString()) Then
                                refNumValue = row.Cells("RefNum").Value
                            Else
                                MessageBox.Show($"Payment method '{methodRaw}' requires a Reference Number.",
                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                                        (SELECT id FROM payment_method
                                         WHERE REPLACE(LOWER(payment_method_name),' ','_') = @mkey
                                         LIMIT 1),
                                        @ref, @amt
                                    );", conn, tx)
                            cmd.Parameters.AddWithValue("@tid", transId)
                            cmd.Parameters.AddWithValue("@mkey", methodKey)
                            cmd.Parameters.AddWithValue("@ref", refNumValue)
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
                                cmd.Parameters.AddWithValue("@amt", -Convert.ToDecimal(row.Cells("Balance").Value))
                                cmd.ExecuteNonQuery()
                            End Using
                        Next
                    End If

                    ' ✅ Commit transaction
                    tx.Commit()
                End Using
            End Using

            Try
                ' Ensure receipts folder exists
                PrinterModule.EnsureReceiptsFolderExists()
                Dim receiptsFolder As String = PrinterModule.ReceiptsFolder

                ' Build file path
                Dim filePath As String = Path.Combine(receiptsFolder, orderNumber & ".pdf")

                ' Customer info
                Dim customerFullName As String = parentForm.txtFirstName.Text.Trim() & " " & parentForm.txtLastName.Text.Trim()
                Dim customerAddress As String = parentForm.txtCustomerAddress.Text.Trim()

                ' Order table
                Dim originalOrderTable As DataTable = CType(parentForm.dgvOrderItemPreview.DataSource, DataTable)
                Dim orderTable As DataTable = ReceiptTableBuilder.BuildReceiptTable(originalOrderTable)

                ' === Compute Totals ===
                Dim totalPurchase As Decimal = 0D
                If orderTable IsNot Nothing AndAlso orderTable.Rows.Count > 0 Then
                    totalPurchase = orderTable.AsEnumerable().Sum(Function(rr) Convert.ToDecimal(rr("AMOUNT")))
                End If
                Dim computedGrandTotal As Decimal = totalPurchase

                ' 🖨️ Generate receipt
                GenerateReceiptPDF(filePath,
                orderNumber,
                DateTime.Now.ToString("M/d/yyyy HH:mm"),
                customerFullName,
                customerAddress,
                totalPurchase.ToString("N2"),
                computedGrandTotal.ToString("N2"),
                orderTable,
                "", "", "", "", "")

                ' 🗂️ Update DB with the PDF filename
                Using conn As New MySqlConnection(My.Settings.DBConnection)
                    conn.Open()
                    Using cmd As New MySqlCommand("UPDATE sales_transaction SET invoice_pdf = @pdf WHERE order_number=@ord;", conn)
                        cmd.Parameters.AddWithValue("@pdf", orderNumber & ".pdf")
                        cmd.Parameters.AddWithValue("@ord", orderNumber)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                MessageBox.Show($"Transaction saved successfully. PDF receipt saved to: {filePath}",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
                MessageBox.Show("Transaction saved, but failed to generate receipt PDF: " & ex.Message,
                        "PDF Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try

        Catch ex As Exception
            MessageBox.Show("Error saving transaction: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' 6️# Cleanup after save
        CleanupForms(parentForm)
    End Sub


    ' 🔹 Simple change calculator (GrandTotal - payments) — now uses PaymentHelpers
    Private Sub UpdateChange()
        Dim parentForm As frmCashierDashboard = CType(Me.ParentForm, frmCashierDashboard)
        Dim grandTotal As Decimal = 0
        Decimal.TryParse(parentForm.txtGrandTotal.Text, grandTotal)

        Dim paid As Decimal = PaymentHelpers.SumPayments(dgvPaymentEntries)
        Dim change As Decimal = PaymentHelpers.ComputeChange(grandTotal, paid)

        txtChange.Text = change.ToString("N2")
    End Sub

    'HELPERS
    ' 🔹 VALIDATE ORDER & PAYMENTS
    Private Function ValidateOrderAndPayments(parentForm As frmCashierDashboard) As Boolean
        If parentForm.dgvOrderItemPreview.DataSource Is Nothing OrElse
       CType(parentForm.dgvOrderItemPreview.DataSource, DataTable).Rows.Count = 0 Then
            MessageBox.Show("No order items added. Please add products before saving and printing.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If dgvPaymentEntries.Rows.Count = 0 Then
            MessageBox.Show("No payment entries to save.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Return True
    End Function

    ' 🔹 VALIDATE CUSTOMER INFO
    Private Function ValidateCustomerInfo(parentForm As frmCashierDashboard, ByRef customerID As Integer) As Boolean
        Dim firstName As String = parentForm.txtFirstName.Text.Trim()
        Dim lastName As String = parentForm.txtLastName.Text.Trim()
        Dim address As String = parentForm.txtCustomerAddress.Text.Trim()

        If String.IsNullOrWhiteSpace(firstName) OrElse
       String.IsNullOrWhiteSpace(lastName) OrElse
       String.IsNullOrWhiteSpace(address) Then
            MessageBox.Show("Customer information is incomplete. Please fill First Name, Last Name, and Address.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        ' 🔹 Check if customer exists
        Try
            customerID = CustomerModule.GetCustomerID(firstName, lastName, address)
            If customerID <> -1 Then
                Return True
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try

        MessageBox.Show("This customer is not yet saved in the database. Please save the customer before proceeding.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Return False
    End Function

    ' 🔹 COMPUTE PAYMENTS + STATUS
    Private Function ComputePaymentsAndStatus(parentForm As frmCashierDashboard,
                                          grandTotal As Decimal,
                                          ByRef totalPaid As Decimal,
                                          ByRef unpaidBalance As Decimal,
                                          ByRef status As String) As Boolean

        totalPaid = 0
        unpaidBalance = 0
        status = "Fully Paid"

        ' 👉 Compute total paid
        For Each row As DataGridViewRow In dgvPaymentEntries.Rows
            If row.Cells("Amount").Value IsNot Nothing Then
                totalPaid += Convert.ToDecimal(row.Cells("Amount").Value)
            End If
        Next

        ' 👉 Require minimum payment
        If totalPaid < 100 Then
            MessageBox.Show("Minimum payment required is 100.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        ' 👉 Check if partial
        If totalPaid < grandTotal Then
            unpaidBalance = grandTotal - totalPaid

            ' 🚫 Not allowed if settling balances
            If parentForm.dgvCustomerBalancePreview.SelectedRows.Count > 0 Then
                MessageBox.Show("Partial payment is not allowed when settling previous balances.",
                            "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If

            ' ✅ Allow with confirmation
            Dim confirm = MessageBox.Show(
            $"Payment is insufficient!" & Environment.NewLine &
            $"Grand Total: {grandTotal:N2}" & Environment.NewLine &
            $"Paid: {totalPaid:N2}" & Environment.NewLine &
            $"Unpaid Balance: {unpaidBalance:N2}" & Environment.NewLine &
            Environment.NewLine & "Do you want to proceed and mark this as PARTIAL?",
            "Partial Payment Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If confirm = DialogResult.No Then Return False
            status = "Partial"
        End If

        Return True
    End Function

    ' 🔹 CLEANUP AFTER SAVE
    Private Sub CleanupForms(parentForm As frmCashierDashboard)
        dgvPaymentEntries.Rows.Clear()
        txtChange.Clear()

        parentForm.txtGrandTotal.Clear()
        parentForm.txtFirstName.Clear()
        parentForm.txtLastName.Clear()
        parentForm.txtCustomerAddress.Clear()

        If TypeOf parentForm.dgvOrderItemPreview.DataSource Is DataTable Then
            CType(parentForm.dgvOrderItemPreview.DataSource, DataTable).Rows.Clear()
        End If
        If TypeOf parentForm.dgvCustomerBalancePreview.DataSource Is DataTable Then
            CType(parentForm.dgvCustomerBalancePreview.DataSource, DataTable).Rows.Clear()
        End If

        parentForm.LoadProducts()
        parentForm.LoadCustomers()
    End Sub

End Class