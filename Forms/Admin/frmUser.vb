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
    ' Delete user (with manual check)
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

        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                ' 🔹 Step 1: Check if user is referenced in sales_transaction
                Dim checkQuery As String = "SELECT COUNT(*) FROM sales_transaction WHERE user_id = @id"
                Using checkCmd As New MySqlCommand(checkQuery, conn)
                    checkCmd.Parameters.AddWithValue("@id", userId)
                    Dim refCount As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                    If refCount > 0 Then
                        MessageBox.Show("This user cannot be deleted because they are already referenced in transactions.",
                                        "Delete Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                End Using

                ' 🔹 Step 2: Safe to delete
                Dim delQuery As String = "DELETE FROM USER WHERE id = @UserID"
                Using delCmd As New MySqlCommand(delQuery, conn)
                    delCmd.Parameters.AddWithValue("@UserID", userId)
                    delCmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("User deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadUsers() ' Refresh grid

        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



End Class
