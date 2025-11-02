Imports System.Windows.Forms
Imports System.Data

Module PaymentHelpers

    Public Function ValidateAmountInput(amountText As String) As Boolean
        Dim amount As Decimal
        If Decimal.TryParse(amountText, amount) Then
            Return amount > 0
        End If
        Return False
    End Function

    Public Function SumPayments(dgvPaymentEntries As DataGridView) As Decimal
        Dim paid As Decimal = 0D
        For Each row As DataGridViewRow In dgvPaymentEntries.Rows
            If row.IsNewRow Then Continue For
            If row.Cells("Amount").Value IsNot Nothing Then
                Decimal.TryParse(Convert.ToString(row.Cells("Amount").Value), paid)
            End If
        Next
        ' safer: recompute properly
        paid = 0D
        For Each row As DataGridViewRow In dgvPaymentEntries.Rows
            If row.IsNewRow Then Continue For
            Dim v As Decimal = 0D
            If Decimal.TryParse(Convert.ToString(row.Cells("Amount").Value), v) Then
                paid += v
            End If
        Next
        Return paid
    End Function

    Public Function ComputeChange(grandTotal As Decimal, paid As Decimal) As Decimal
        Dim change As Decimal = paid - grandTotal
        If change >= 0 Then
            Return change
        End If
        Return 0D
    End Function

    ' Validate combination rules: returns (isValid, message)
    Public Function ValidatePaymentCombination(dgvPaymentEntries As DataGridView,
                                               methodToAdd As String) As Tuple(Of Boolean, String)
        Dim cashCount As Integer = 0
        Dim onlineCount As Integer = 0
        For Each row As DataGridViewRow In dgvPaymentEntries.Rows
            If row.IsNewRow Then Continue For
            If row.Cells("Method").Value IsNot Nothing Then
                Dim m = row.Cells("Method").Value.ToString().Trim().ToLower()
                If m = "cash" Then cashCount += 1
                If m = "online" Then onlineCount += 1
            End If
        Next

        methodToAdd = methodToAdd.Trim().ToLower()
        If methodToAdd = "cash" AndAlso cashCount >= 1 Then
            Return Tuple.Create(False, "You can only add one CASH payment.")
        End If
        If methodToAdd = "online" AndAlso onlineCount >= 1 Then
            Return Tuple.Create(False, "You can only add one ONLINE payment.")
        End If
        If cashCount >= 1 AndAlso onlineCount >= 1 Then
            Return Tuple.Create(False, "You can only have one CASH and one ONLINE payment in total.")
        End If

        Return Tuple.Create(True, String.Empty)
    End Function

End Module