Imports System.Text
Imports System.Windows.Forms

Public Module TransactionSummaryModule
    ''' <summary>
    ''' Builds a detailed transaction summary including order items, settled balances, payments and totals
    ''' </summary>
    Public Function BuildTransactionSummary(orderTable As DataTable,
                                          balanceGridView As DataGridView,
                                          paymentGridView As DataGridView,
                                          grandTotal As Decimal,
                                          totalPaid As Decimal,
                                          status As String,
                                          unpaidBalance As Decimal,
                                          firstName As String,
                                          lastName As String,
                                          address As String) As String

        Dim details As New StringBuilder("=== ORDER ITEMS ===" & Environment.NewLine)

        ' Order Items
        If orderTable.Columns.Contains("Product Name") AndAlso orderTable.Columns.Contains("Brand") Then
            For Each r As DataRow In orderTable.Rows
                Dim prod As String = If(r("Product Name"), "").ToString()
                Dim brand As String = If(r("Brand"), "").ToString()
                r("Product Name") = prod & " - " & brand
            Next
        End If

        ' Settled Balances
        Dim balanceTotal As Decimal = 0
        details.AppendLine().AppendLine("=== SETTLED BALANCES ===")
        If balanceGridView.SelectedRows.Count > 0 Then
            For Each row As DataGridViewRow In balanceGridView.SelectedRows
                details.AppendFormat("{0:yyyy-MM-dd} | {1} | {2:N2}",
                                   CDate(row.Cells("Date").Value),
                                   row.Cells("Description").Value,
                                   Convert.ToDecimal(row.Cells("Balance").Value)).AppendLine()
                balanceTotal += Convert.ToDecimal(row.Cells("Balance").Value)
            Next
        Else
            details.AppendLine("No balances settled (0.00)")
        End If

        ' Payments
        details.AppendLine().AppendLine("=== PAYMENTS ENTERED ===")
        For Each row As DataGridViewRow In paymentGridView.Rows
            details.AppendFormat("{0} | {1} | {2}",
                               row.Cells("Method").Value,
                               row.Cells("RefNum").Value,
                               row.Cells("Amount").Value).AppendLine()
        Next

        ' Customer + Totals
        details.AppendLine().AppendFormat("Customer: {0} {1}, {2}", firstName, lastName, address).AppendLine()
        details.AppendFormat("Order Subtotal: {0:N2}", grandTotal - balanceTotal).AppendLine()
        details.AppendFormat("Settled Balances: {0:N2}", balanceTotal).AppendLine()
        details.AppendFormat("Grand Total: {0:N2}", grandTotal).AppendLine()
        details.AppendFormat("Total Paid: {0:N2}", totalPaid).AppendLine()
        details.AppendFormat("Change: {0:N2}", Math.Max(totalPaid - grandTotal, 0)).AppendLine()
        details.AppendFormat("Status: {0}", status).AppendLine()

        If status = "Partial" Then
            details.AppendFormat("Unpaid Balance (to ledger): {0:N2}", unpaidBalance).AppendLine()
        End If

        Return details.ToString()
    End Function

    ''' <summary>
    ''' Shows the transaction summary in a MessageBox
    ''' </summary>
    Public Sub ShowTransactionSummary(details As String)
        MessageBox.Show(details, "Save + Print", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

End Module