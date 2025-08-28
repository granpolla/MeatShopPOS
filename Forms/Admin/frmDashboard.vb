Imports MySql.Data.MySqlClient

Public Class frmDashboard

    Private Sub frmDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCashiers()
    End Sub

    Private Sub LoadCashiers()
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                Dim query As String = "
                    SELECT u.full_name
                    FROM user u
                    INNER JOIN role r ON u.role_id = r.id
                    WHERE r.role_name = 'cashier'
                    ORDER BY u.id ASC
                    LIMIT 6;
                "

                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        Dim cashierIndex As Integer = 1

                        While reader.Read()
                            Dim fullName As String = reader("full_name").ToString()

                            Select Case cashierIndex
                                Case 1 : txtCashierOne.Text = fullName
                                Case 2 : txtCashierTwo.Text = fullName
                                Case 3 : txtCashierThree.Text = fullName
                                Case 4 : txtCashierFour.Text = fullName
                                Case 5 : txtCashierFive.Text = fullName
                                Case 6 : txtCashierSix.Text = fullName
                            End Select

                            cashierIndex += 1
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading cashiers: " & ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

End Class
