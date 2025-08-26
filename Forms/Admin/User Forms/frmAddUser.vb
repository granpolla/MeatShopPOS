Imports MySql.Data.MySqlClient
Imports BCrypt.Net

Public Class frmAddUser

    Private Sub frmAddUser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadRoles()
        cmbRole.DropDownStyle = ComboBoxStyle.DropDownList
    End Sub

    Private Sub LoadRoles()
        Dim query As String = "SELECT id, role_name FROM ROLE"
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                Using cmd As New MySqlCommand(query, conn)
                    Dim adapter As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    adapter.Fill(dt)
                    cmbRole.DataSource = dt
                    cmbRole.DisplayMember = "role_name"
                    cmbRole.ValueMember = "id"
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading roles: " & ex.Message)
        End Try
    End Sub


    ' ✅ Save button
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' 1. Validation
        If txtFullName.Text.Trim() = "" Or txtPassword.Text.Trim() = "" Or txtConfirmPassword.Text.Trim() = "" Then
            MessageBox.Show("Please fill in all fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If txtPassword.Text.Trim() <> txtConfirmPassword.Text.Trim() Then
            MessageBox.Show("Passwords do not match!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' 2. Proper-case full name
        Dim properFullName As String = Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtFullName.Text.Trim().ToLower())

        ' Extract first name for username generation
        Dim firstName As String = properFullName.Split(" "c)(0).ToLower()

        ' 3. Generate username based on role
        Dim selectedRole As String = cmbRole.Text.ToLower()
        Dim generatedUsername As String = ""

        If selectedRole = "admin" Then
            generatedUsername = "adm" & firstName
        ElseIf selectedRole = "cashier" Then
            generatedUsername = "csh" & firstName
        End If

        ' 4. Check if full name already exists
        Dim checkQuery As String = "SELECT COUNT(*) FROM USER WHERE full_name = @FullName"
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                Using checkCmd As New MySqlCommand(checkQuery, conn)
                    checkCmd.Parameters.AddWithValue("@FullName", properFullName)
                    Dim exists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                    If exists > 0 Then
                        MessageBox.Show("A user with this full name already exists.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                End Using

                ' 5. Confirmation message
                Dim confirmationMsg As String =
                    $"Full Name: {properFullName}" & vbCrLf &
                    $"Username: {generatedUsername}" & vbCrLf &
                    $"Password: {txtPassword.Text.Trim()}" & vbCrLf &
                    $"Role: {cmbRole.Text}"

                Dim confirmResult = MessageBox.Show(confirmationMsg & vbCrLf & vbCrLf & "Do you want to save this user?",
                                                    "Confirm User",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question)

                If confirmResult = DialogResult.No Then
                    Return
                End If

                ' 6. Save user
                Dim hashedPassword As String = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text.Trim())

                Dim insertQuery As String =
                    "INSERT INTO USER (full_name, username, password_hash, role_id, created_at, updated_at)
                     VALUES (@FullName, @Username, @Password, @RoleID, NOW(), NOW())"

                Using cmd As New MySqlCommand(insertQuery, conn)
                    cmd.Parameters.AddWithValue("@FullName", properFullName)
                    cmd.Parameters.AddWithValue("@Username", generatedUsername)
                    cmd.Parameters.AddWithValue("@Password", hashedPassword)
                    cmd.Parameters.AddWithValue("@RoleID", cmbRole.SelectedValue)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' ✅ Close the form
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error saving user: " & ex.Message)
        End Try
    End Sub


    ' ✅ Cancel button
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class
