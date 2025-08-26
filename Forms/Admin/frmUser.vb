Imports MySql.Data.MySqlClient

Public Class frmUser

    Private Sub frmUser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadUsers()
    End Sub

    ' ✅ Load users into DataGridView
    Private Sub LoadUsers()
        Dim query As String = "
            SELECT u.id, u.full_name AS 'Full Name', u.username AS 'Username',
                   r.role_name AS 'Role', u.created_at AS 'Created At'
            FROM USER u
            INNER JOIN ROLE r ON u.role_id = r.id
            ORDER BY u.created_at DESC"
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                Using cmd As New MySqlCommand(query, conn)
                    Dim adapter As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    adapter.Fill(dt)
                    dgvUsers.DataSource = dt

                    FormatGrid()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading users: " & ex.Message)
        End Try
    End Sub

    Private Sub FormatGrid()
        With dgvUsers
            .Columns("id").Visible = False   ' hide primary key
            .Columns("Full Name").Width = 200
            .Columns("Username").Width = 150
            .Columns("Role").Width = 100
            .Columns("Created At").Width = 150
        End With
    End Sub

    ' ✅ Refresh button
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadUsers()
    End Sub


    ' For adding new user
    Private Sub btnAddUser_Click(sender As Object, e As EventArgs) Handles btnAddUser.Click
        Dim f As New frmAddUser()
        If f.ShowDialog() = DialogResult.OK Then
            LoadUsers() ' Refresh grid after adding user
        End If
    End Sub


    ' Edit user
    Private Sub btnEditUser_Click(sender As Object, e As EventArgs) Handles btnEditUser.Click
        If dgvUsers.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a user to edit.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Get selected row values
        Dim selectedRow As DataGridViewRow = dgvUsers.SelectedRows(0)
        Dim userId As Integer = Convert.ToInt32(selectedRow.Cells("id").Value)
        Dim fullName As String = selectedRow.Cells("Full Name").Value.ToString()
        Dim role As String = selectedRow.Cells("Role").Value.ToString()

        ' Open Edit User form
        Dim f As New frmEditUser(userId, fullName, role)
        If f.ShowDialog() = DialogResult.OK Then
            LoadUsers() ' Refresh grid after editing
        End If
    End Sub


    ' Delete user
    Private Sub btnDeleteUser_Click(sender As Object, e As EventArgs) Handles btnDeleteUser.Click
        If dgvUsers.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a user to delete.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Get selected row
        Dim selectedRow As DataGridViewRow = dgvUsers.SelectedRows(0)
        Dim userId As Integer = Convert.ToInt32(selectedRow.Cells("id").Value)
        Dim fullName As String = selectedRow.Cells("Full Name").Value.ToString()
        Dim username As String = selectedRow.Cells("Username").Value.ToString()

        ' Confirmation
        Dim confirmMsg As String = $"Are you sure you want to delete this user?" & vbCrLf &
                                   $"Full Name: {fullName}" & vbCrLf &
                                   $"Username: {username}"
        If MessageBox.Show(confirmMsg, "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        ' Delete from DB
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()
                Dim query As String = "DELETE FROM USER WHERE id = @UserID"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@UserID", userId)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("User deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadUsers() ' Refresh grid

        Catch ex As MySqlException
            ' ✅ Handle foreign key error
            If ex.Number = 1451 Then
                MessageBox.Show("This user cannot be deleted because they are already linked to transactions or records.", "Delete Blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Else
                MessageBox.Show("Database error while deleting user: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


End Class
