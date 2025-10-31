Imports MySql.Data.MySqlClient
Imports BCrypt.Net
Imports System.Text.RegularExpressions

Public Class frmEditUser
    Private userId As Integer
    Private originalRole As String
    Private originalFullName As String

    ' Constructor to receive values from frmUser
    Public Sub New(id As Integer, fullName As String, role As String)
        InitializeComponent()
        userId = id
        txtFullName.Text = fullName
        originalFullName = fullName
        originalRole = role
    End Sub

    Private Sub frmEditUser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadRoles()
        cmbRole.SelectedIndex = cmbRole.FindString(originalRole)
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

    ' ✅ Save changes
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' Validation for fullname
        If txtFullName.Text.Trim() = "" Then
            MessageBox.Show("Please enter a full name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' --- 🔹 Normalize Full Name ---
        Dim collapsedName As String = Regex.Replace(txtFullName.Text.Trim(), "\s+", " ") ' collapse multiple spaces
        Dim properFullName As String = Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(collapsedName.ToLower())

        ' Generate new username based on role + firstname
        Dim rolePrefix As String = ""
        Select Case cmbRole.Text.ToLower()
            Case "admin" : rolePrefix = "adm"
            Case "cashier" : rolePrefix = "csh"
            Case Else : rolePrefix = "usr"
        End Select

        Dim firstName As String = properFullName.Split(" "c)(0).ToLower()
        Dim newUsername As String = rolePrefix & firstName

        ' --- 🔹 Restriction Check ---
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                Dim refCheckQuery As String = "SELECT COUNT(*) FROM SALES_TRANSACTION WHERE user_id=@id"
                Using refCmd As New MySqlCommand(refCheckQuery, conn)
                    refCmd.Parameters.AddWithValue("@id", userId)
                    Dim count As Integer = Convert.ToInt32(refCmd.ExecuteScalar())
                    If count > 0 Then
                        ' If user is referenced → block fullname & role change
                        If properFullName <> originalFullName OrElse cmbRole.Text <> originalRole Then
                            MessageBox.Show("This user is already referenced in transactions. You can only reset their password.", "Restriction", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Return
                        End If
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error checking references: " & ex.Message)
            Return
        End Try
        ' --- 🔹 End Restriction Check ---

        ' --- 🔹 Handle password update ---
        Dim updatePassword As Boolean = False
        Dim hashedPassword As String = ""

        If txtPassword.Text.Trim() <> "" OrElse txtConfirmPassword.Text.Trim() <> "" Then
            If txtPassword.Text.Trim() = "" Or txtConfirmPassword.Text.Trim() = "" Then
                MessageBox.Show("Please fill in both password fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            If txtPassword.Text.Trim() <> txtConfirmPassword.Text.Trim() Then
                MessageBox.Show("Passwords do not match!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            hashedPassword = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text.Trim())
            updatePassword = True
        End If

        ' Confirmation message
        Dim msg As String = $"Full Name: {properFullName}{vbCrLf}" &
                            $"Username: {newUsername}{vbCrLf}" &
                            $"Role: {cmbRole.Text}"

        If updatePassword Then
            msg &= vbCrLf & $"Password: (will be changed)"
        Else
            msg &= vbCrLf & "(Password unchanged)"
        End If

        If MessageBox.Show(msg, "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        ' --- Build SQL dynamically ---
        Dim updateQuery As String
        If updatePassword Then
            updateQuery = "
                UPDATE USER
                SET full_name = @FullName,
                    username = @Username,
                    password_hash = @Password,
                    role_id = @RoleID,
                    updated_at = NOW()
                WHERE id = @UserID"
        Else
            updateQuery = "
                UPDATE USER
                SET full_name = @FullName,
                    username = @Username,
                    role_id = @RoleID,
                    updated_at = NOW()
                WHERE id = @UserID"
        End If

        ' --- Run update ---
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()
                Using cmd As New MySqlCommand(updateQuery, conn)
                    cmd.Parameters.AddWithValue("@FullName", properFullName)
                    cmd.Parameters.AddWithValue("@Username", newUsername)
                    cmd.Parameters.AddWithValue("@RoleID", cmbRole.SelectedValue)
                    cmd.Parameters.AddWithValue("@UserID", userId)

                    If updatePassword Then
                        cmd.Parameters.AddWithValue("@Password", hashedPassword)
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error updating user: " & ex.Message)
        End Try
    End Sub

    ' ✅ Cancel button
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
