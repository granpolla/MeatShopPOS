<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmCashierDashboard
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCashierDashboard))
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblUserRole = New System.Windows.Forms.Label()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblUserFullName = New System.Windows.Forms.Label()
        Me.lblHeader = New System.Windows.Forms.Label()
        Me.txtFirstName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnl1 = New System.Windows.Forms.Panel()
        Me.pnlInputOrderItem = New System.Windows.Forms.Panel()
        Me.dgvCustomerBalancePreview = New System.Windows.Forms.DataGridView()
        Me.dgvProducts = New System.Windows.Forms.DataGridView()
        Me.dgvCustomer = New System.Windows.Forms.DataGridView()
        Me.pnlTransactionHeader = New System.Windows.Forms.Panel()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtCustomerAddress = New System.Windows.Forms.TextBox()
        Me.pnl2 = New System.Windows.Forms.Panel()
        Me.DataGridView3 = New System.Windows.Forms.DataGridView()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtLastName = New System.Windows.Forms.TextBox()
        Me.Panel2.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl1.SuspendLayout()
        CType(Me.dgvCustomerBalancePreview, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvProducts, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvCustomer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTransactionHeader.SuspendLayout()
        Me.pnl2.SuspendLayout()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Controls.Add(Me.lblUserRole)
        Me.Panel2.Controls.Add(Me.PictureBox2)
        Me.Panel2.Controls.Add(Me.btnLogout)
        Me.Panel2.Controls.Add(Me.PictureBox1)
        Me.Panel2.Controls.Add(Me.lblUserFullName)
        Me.Panel2.Controls.Add(Me.lblHeader)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1537, 61)
        Me.Panel2.TabIndex = 6
        '
        'lblUserRole
        '
        Me.lblUserRole.AutoSize = True
        Me.lblUserRole.Location = New System.Drawing.Point(1084, 33)
        Me.lblUserRole.Name = "lblUserRole"
        Me.lblUserRole.Size = New System.Drawing.Size(30, 16)
        Me.lblUserRole.TabIndex = 11
        Me.lblUserRole.Text = "role"
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(1038, 11)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(38, 38)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 10
        Me.PictureBox2.TabStop = False
        '
        'btnLogout
        '
        Me.btnLogout.BackColor = System.Drawing.Color.Red
        Me.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLogout.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogout.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.btnLogout.Location = New System.Drawing.Point(1435, 14)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(90, 33)
        Me.btnLogout.TabIndex = 0
        Me.btnLogout.Text = "Logout"
        Me.btnLogout.UseVisualStyleBackColor = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(61, 61)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 7
        Me.PictureBox1.TabStop = False
        '
        'lblUserFullName
        '
        Me.lblUserFullName.AutoSize = True
        Me.lblUserFullName.Font = New System.Drawing.Font("Segoe UI Semibold", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserFullName.Location = New System.Drawing.Point(1082, 11)
        Me.lblUserFullName.Name = "lblUserFullName"
        Me.lblUserFullName.Size = New System.Drawing.Size(50, 25)
        Me.lblUserFullName.TabIndex = 0
        Me.lblUserFullName.Text = "User"
        '
        'lblHeader
        '
        Me.lblHeader.AutoSize = True
        Me.lblHeader.Font = New System.Drawing.Font("Segoe UI", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeader.Location = New System.Drawing.Point(67, 11)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(288, 38)
        Me.lblHeader.TabIndex = 7
        Me.lblHeader.Text = "LASH FROZEN MEAT"
        '
        'txtFirstName
        '
        Me.txtFirstName.Location = New System.Drawing.Point(129, 14)
        Me.txtFirstName.Name = "txtFirstName"
        Me.txtFirstName.Size = New System.Drawing.Size(220, 22)
        Me.txtFirstName.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(45, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 16)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "First Name:"
        '
        'pnl1
        '
        Me.pnl1.Controls.Add(Me.pnlInputOrderItem)
        Me.pnl1.Controls.Add(Me.dgvCustomerBalancePreview)
        Me.pnl1.Controls.Add(Me.dgvProducts)
        Me.pnl1.Controls.Add(Me.dgvCustomer)
        Me.pnl1.Controls.Add(Me.pnlTransactionHeader)
        Me.pnl1.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnl1.Location = New System.Drawing.Point(0, 61)
        Me.pnl1.Name = "pnl1"
        Me.pnl1.Size = New System.Drawing.Size(1005, 742)
        Me.pnl1.TabIndex = 9
        '
        'pnlInputOrderItem
        '
        Me.pnlInputOrderItem.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.pnlInputOrderItem.Location = New System.Drawing.Point(12, 538)
        Me.pnlInputOrderItem.Name = "pnlInputOrderItem"
        Me.pnlInputOrderItem.Size = New System.Drawing.Size(393, 192)
        Me.pnlInputOrderItem.TabIndex = 15
        '
        'dgvCustomerBalancePreview
        '
        Me.dgvCustomerBalancePreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCustomerBalancePreview.Location = New System.Drawing.Point(411, 6)
        Me.dgvCustomerBalancePreview.Name = "dgvCustomerBalancePreview"
        Me.dgvCustomerBalancePreview.RowHeadersWidth = 51
        Me.dgvCustomerBalancePreview.RowTemplate.Height = 24
        Me.dgvCustomerBalancePreview.Size = New System.Drawing.Size(591, 166)
        Me.dgvCustomerBalancePreview.TabIndex = 14
        '
        'dgvProducts
        '
        Me.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvProducts.Location = New System.Drawing.Point(411, 178)
        Me.dgvProducts.Name = "dgvProducts"
        Me.dgvProducts.RowHeadersWidth = 51
        Me.dgvProducts.RowTemplate.Height = 24
        Me.dgvProducts.Size = New System.Drawing.Size(591, 236)
        Me.dgvProducts.TabIndex = 0
        '
        'dgvCustomer
        '
        Me.dgvCustomer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCustomer.Location = New System.Drawing.Point(12, 6)
        Me.dgvCustomer.Name = "dgvCustomer"
        Me.dgvCustomer.RowHeadersWidth = 51
        Me.dgvCustomer.RowTemplate.Height = 24
        Me.dgvCustomer.Size = New System.Drawing.Size(393, 248)
        Me.dgvCustomer.TabIndex = 13
        '
        'pnlTransactionHeader
        '
        Me.pnlTransactionHeader.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.pnlTransactionHeader.Controls.Add(Me.txtLastName)
        Me.pnlTransactionHeader.Controls.Add(Me.Label3)
        Me.pnlTransactionHeader.Controls.Add(Me.btnClear)
        Me.pnlTransactionHeader.Controls.Add(Me.btnRefresh)
        Me.pnlTransactionHeader.Controls.Add(Me.txtFirstName)
        Me.pnlTransactionHeader.Controls.Add(Me.Label1)
        Me.pnlTransactionHeader.Controls.Add(Me.btnSearch)
        Me.pnlTransactionHeader.Controls.Add(Me.Label2)
        Me.pnlTransactionHeader.Controls.Add(Me.txtCustomerAddress)
        Me.pnlTransactionHeader.Location = New System.Drawing.Point(12, 260)
        Me.pnlTransactionHeader.Name = "pnlTransactionHeader"
        Me.pnlTransactionHeader.Size = New System.Drawing.Size(393, 216)
        Me.pnlTransactionHeader.TabIndex = 12
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(201, 160)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(148, 23)
        Me.btnClear.TabIndex = 14
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(19, 160)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(176, 23)
        Me.btnRefresh.TabIndex = 13
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(19, 131)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(330, 23)
        Me.btnSearch.TabIndex = 11
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(59, 73)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(61, 16)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Address:"
        '
        'txtCustomerAddress
        '
        Me.txtCustomerAddress.Location = New System.Drawing.Point(129, 70)
        Me.txtCustomerAddress.Name = "txtCustomerAddress"
        Me.txtCustomerAddress.Size = New System.Drawing.Size(220, 22)
        Me.txtCustomerAddress.TabIndex = 10
        '
        'pnl2
        '
        Me.pnl2.Controls.Add(Me.DataGridView3)
        Me.pnl2.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnl2.Location = New System.Drawing.Point(1011, 61)
        Me.pnl2.Name = "pnl2"
        Me.pnl2.Size = New System.Drawing.Size(526, 742)
        Me.pnl2.TabIndex = 10
        '
        'DataGridView3
        '
        Me.DataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView3.Location = New System.Drawing.Point(3, 6)
        Me.DataGridView3.Name = "DataGridView3"
        Me.DataGridView3.RowHeadersWidth = 51
        Me.DataGridView3.RowTemplate.Height = 24
        Me.DataGridView3.Size = New System.Drawing.Size(511, 334)
        Me.DataGridView3.TabIndex = 16
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(45, 45)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 16)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "Last Name:"
        '
        'txtLastName
        '
        Me.txtLastName.Location = New System.Drawing.Point(129, 42)
        Me.txtLastName.Name = "txtLastName"
        Me.txtLastName.Size = New System.Drawing.Size(220, 22)
        Me.txtLastName.TabIndex = 16
        '
        'frmCashierDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1537, 803)
        Me.Controls.Add(Me.pnl2)
        Me.Controls.Add(Me.pnl1)
        Me.Controls.Add(Me.Panel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmCashierDashboard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cashier"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl1.ResumeLayout(False)
        CType(Me.dgvCustomerBalancePreview, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvProducts, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvCustomer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTransactionHeader.ResumeLayout(False)
        Me.pnlTransactionHeader.PerformLayout()
        Me.pnl2.ResumeLayout(False)
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblUserRole As Label
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents btnLogout As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents lblUserFullName As Label
    Friend WithEvents lblHeader As Label
    Friend WithEvents pnl1 As Panel
    Friend WithEvents txtFirstName As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtCustomerAddress As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btnSearch As Button
    Friend WithEvents pnl2 As Panel
    Friend WithEvents pnlTransactionHeader As Panel
    Friend WithEvents dgvCustomer As DataGridView
    Friend WithEvents btnRefresh As Button
    Friend WithEvents dgvCustomerBalancePreview As DataGridView
    Friend WithEvents DataGridView3 As DataGridView
    Friend WithEvents btnClear As Button
    Friend WithEvents dgvProducts As DataGridView
    Friend WithEvents pnlInputOrderItem As Panel
    Friend WithEvents txtLastName As TextBox
    Friend WithEvents Label3 As Label
End Class
