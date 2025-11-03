Imports MySql.Data.MySqlClient

Public Module TransactionModule
    Public Function GenerateOrderNumber(username As String) As String
        Using conn As New MySqlConnection(My.Settings.DBConnection)
            conn.Open()
            Dim rand As New Random()
            Dim orderNumber As String
            Dim exists As Boolean = True

            While exists
                Dim randomNum As Integer = rand.Next(1000, 9999)
                orderNumber = $"ORD-{randomNum}-{username.ToUpper()}"
                Using cmdCheck As New MySqlCommand("SELECT COUNT(*) FROM sales_transaction WHERE order_number=@ord", conn)
                    cmdCheck.Parameters.AddWithValue("@ord", orderNumber)
                    exists = (Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0)
                End Using
            End While

            Return orderNumber
        End Using
    End Function

    Public Function NormalizePaymentStatus(status As String) As String
        Select Case status.Trim().ToLower()
            Case "full", "fully paid", "paid"
                Return "full"
            Case "partial", "partially paid"
                Return "partial"
            Case Else
                Throw New ArgumentException("Unknown payment status: " & status)
        End Select
    End Function
End Module
