Public Module ValidationModule
    Public Function ValidatePaymentRules(paymentMethod As String,
                                       existingPayments As DataGridView) As (IsValid As Boolean, Message As String)
        Dim cashCount As Integer = 0
        Dim onlineCount As Integer = 0

        For Each row As DataGridViewRow In existingPayments.Rows
            If row.Cells("Method").Value IsNot Nothing Then
                Dim existingMethod = row.Cells("Method").Value.ToString().Trim().ToLower()
                If existingMethod = "cash" Then cashCount += 1
                If existingMethod = "online" Then onlineCount += 1
            End If
        Next

        If paymentMethod = "cash" AndAlso cashCount >= 1 Then
            Return (False, "You can only add one CASH payment.")
        End If

        If paymentMethod = "online" AndAlso onlineCount >= 1 Then
            Return (False, "You can only add one ONLINE payment.")
        End If

        If cashCount >= 1 AndAlso onlineCount >= 1 Then
            Return (False, "You can only have one CASH and one ONLINE payment in total.")
        End If

        Return (True, String.Empty)
    End Function
End Module