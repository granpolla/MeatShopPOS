Imports MySql.Data.MySqlClient

Module modDatabase
    Public Conn As MySqlConnection

    Public Sub OpenConnection()
        Try
            If Conn Is Nothing Then
                Dim connStr As String = My.Settings.DBConnection
                Conn = New MySqlConnection(connStr)
            End If
            If Conn.State = ConnectionState.Closed Then
                Conn.Open()
                MessageBox.Show("Database Connected!")
            End If
        Catch ex As MySqlException
            MessageBox.Show("Database connection failed: " & ex.Message)
        End Try
    End Sub

End Module
