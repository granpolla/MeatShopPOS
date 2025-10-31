Public Class frmAdminDashboard

    Private Sub frmAdminDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Show the full name of the logged-in user
        lblUserFullName.Text = LoggedInFullName
        lblUserRole.Text = LoggedInRole

        ' Load Dashboard by default
        HighlightButton(btnDashboard)
        LoadFormInPanel(New frmDashboard())
    End Sub

    ' 🔹 Load form inside panel
    Private Sub LoadFormInPanel(form As Form)
        pnlContainer.Controls.Clear()
        form.TopLevel = False
        form.FormBorderStyle = FormBorderStyle.None
        form.Dock = DockStyle.Fill
        pnlContainer.Controls.Add(form)
        form.Show()
    End Sub

    ' 🔹 Highlight active button
    Private Sub HighlightButton(activeBtn As Button)
        ' Reset all buttons first
        For Each ctrl As Control In pnlSidebar.Controls
            If TypeOf ctrl Is Button Then
                Dim btn As Button = DirectCast(ctrl, Button)
                btn.BackColor = Color.FromArgb(45, 45, 48) ' default sidebar color
                btn.ForeColor = Color.White
            End If
        Next

        ' Apply active style
        activeBtn.BackColor = Color.FromArgb(0, 122, 204) ' highlight blue
        activeBtn.ForeColor = Color.White
    End Sub

    ' 🔹 Buttons
    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        HighlightButton(btnDashboard)
        LoadFormInPanel(New frmDashboard())
    End Sub

    Private Sub btnProduct_Click(sender As Object, e As EventArgs) Handles btnProduct.Click
        HighlightButton(btnProduct)
        LoadFormInPanel(New frmProduct())
    End Sub

    Private Sub btnCustomer_Click(sender As Object, e As EventArgs) Handles btnCustomer.Click
        HighlightButton(btnCustomer)
        LoadFormInPanel(New frmCustomer())
    End Sub

    Private Sub btnUser_Click(sender As Object, e As EventArgs) Handles btnUser.Click
        HighlightButton(btnUser)
        LoadFormInPanel(New frmUser())
    End Sub

    Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click
        HighlightButton(btnTransaction)
        LoadFormInPanel(New frmTransaction())
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to logout?",
                                                "Logout Confirmation",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            ' 🔹 Clear session variables
            LoggedInUserID = 0
            LoggedInFullName = ""
            LoggedInRole = ""

            ' 🔹 Show login form again
            Dim loginForm As New frmLogin()
            loginForm.Show()

            ' 🔹 Close current dashboard
            Me.Close()
        End If
    End Sub

End Class
