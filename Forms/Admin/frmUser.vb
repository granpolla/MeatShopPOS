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

End Class
