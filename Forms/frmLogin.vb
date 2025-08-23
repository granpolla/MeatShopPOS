Imports MySql.Data.MySqlClient

Public Class frmLogin

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtUsername.Text = "Username"
        txtUsername.ForeColor = Color.DarkGray

        txtPassword.Text = "Password"
        txtPassword.ForeColor = Color.DarkGray
    End Sub


    'For login button
    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        ' 🔎 Validate input first
        If String.IsNullOrWhiteSpace(txtUsername.Text) OrElse String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MessageBox.Show("Please enter both username and password.", "Missing Information")
            Return
        End If

        Dim query As String = "SELECT u.id, u.full_name, r.role_name 
                               FROM USER u 
                               INNER JOIN ROLE r ON u.role_id = r.id 
                               WHERE u.username = @username AND u.password_hash = @password"

        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim())
                    cmd.Parameters.AddWithValue("@password", ComputeSHA256Hash(txtPassword.Text.Trim())) ' 🔐 hash

                    conn.Open()
                    Dim reader As MySqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        ' ✅ Login success: store in session
                        LoggedInUserID = reader("id")
                        LoggedInFullName = reader("full_name").ToString()
                        LoggedInRole = reader("role_name").ToString()

                        MessageBox.Show("Welcome " & LoggedInFullName & "!", "Login Successful")

                        Me.Hide()
                        frmMainMenu.Show()
                    Else
                        MessageBox.Show("Invalid username or password.", "Login Failed")
                        txtPassword.Clear()
                        txtPassword.Focus()
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error connecting to database: " & ex.Message, "Database Error")
        End Try
    End Sub


    'For cancel button
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Application.Exit()
    End Sub



    'For login UI
    Private Sub txtUsername_GotFocus(sender As Object, e As EventArgs) Handles txtUsername.GotFocus
        If txtUsername.Text = "Username" Then
            txtUsername.Text = ""
            txtUsername.ForeColor = Color.Black
        End If
    End Sub

    Private Sub txtUsername_LostFocus(sender As Object, e As EventArgs) Handles txtUsername.LostFocus
        If txtUsername.Text = "" Then
            txtUsername.Text = "Username"
            txtUsername.ForeColor = Color.DarkGray
        End If
    End Sub

    Private Sub txtPassword_GotFocus(sender As Object, e As EventArgs) Handles txtPassword.GotFocus
        If txtPassword.Text = "Password" Then
            txtPassword.Text = ""
            txtPassword.PasswordChar = "•"
            txtPassword.ForeColor = Color.Black
        End If
    End Sub

    Private Sub txtPassword_LostFocus(sender As Object, e As EventArgs) Handles txtPassword.LostFocus
        If txtPassword.Text = "" Then
            txtPassword.Text = "Password"
            txtPassword.PasswordChar = ControlChars.NullChar ' Disable password char
            txtPassword.ForeColor = Color.DarkGray
        End If
    End Sub


End Class