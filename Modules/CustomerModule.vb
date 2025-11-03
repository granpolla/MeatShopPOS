Imports MySql.Data.MySqlClient

Public Module CustomerModule
    ' Extracted from ValidateCustomerInfo
    Public Function GetCustomerID(firstName As String, lastName As String, address As String) As Integer
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
                    Return If(result IsNot Nothing, Convert.ToInt32(result), -1)
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error checking customer: " & ex.Message)
        End Try
    End Function
End Module