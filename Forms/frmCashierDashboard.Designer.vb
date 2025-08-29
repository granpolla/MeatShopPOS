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
        Me.pnlInputOrderItem = New System.Windows.Forms.Panel()
        Me.dgvCustomerBalancePreview = New System.Windows.Forms.DataGridView()
        Me.dgvProductsPreview = New System.Windows.Forms.DataGridView()
        Me.dgvCustomer = New System.Windows.Forms.DataGridView()
        Me.dgvOrderItemPreview = New System.Windows.Forms.DataGridView()
        Me.pnlTransactionHeader = New System.Windows.Forms.Panel()
        Me.txtLastName = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.txtFirstName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtCustomerAddress = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txtCustomerBalanceTotal = New System.Windows.Forms.TextBox()
        Me.txtGrandTotal = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnRemoveItem = New System.Windows.Forms.Button()
        Me.btnClearBalanceTxt = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvCustomerBalancePreview, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvProductsPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvCustomer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvOrderItemPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTransactionHeader.SuspendLayout()
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
        Me.Panel2.Size = New System.Drawing.Size(1531, 61)
        Me.Panel2.TabIndex = 6
        '
        'lblUserRole
        '
        Me.lblUserRole.AutoSize = True
        Me.lblUserRole.Location = New System.Drawing.Point(1223, 33)
        Me.lblUserRole.Name = "lblUserRole"
        Me.lblUserRole.Size = New System.Drawing.Size(30, 16)
        Me.lblUserRole.TabIndex = 11
        Me.lblUserRole.Text = "role"
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(1177, 11)
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
        Me.btnLogout.Location = New System.Drawing.Point(1432, 16)
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
        Me.lblUserFullName.Location = New System.Drawing.Point(1221, 11)
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
        'pnlInputOrderItem
        '
        Me.pnlInputOrderItem.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.pnlInputOrderItem.Location = New System.Drawing.Point(12, 524)
        Me.pnlInputOrderItem.Name = "pnlInputOrderItem"
        Me.pnlInputOrderItem.Size = New System.Drawing.Size(460, 295)
        Me.pnlInputOrderItem.TabIndex = 15
        '
        'dgvCustomerBalancePreview
        '
        Me.dgvCustomerBalancePreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCustomerBalancePreview.Location = New System.Drawing.Point(478, 321)
        Me.dgvCustomerBalancePreview.Name = "dgvCustomerBalancePreview"
        Me.dgvCustomerBalancePreview.RowHeadersWidth = 51
        Me.dgvCustomerBalancePreview.RowTemplate.Height = 24
        Me.dgvCustomerBalancePreview.Size = New System.Drawing.Size(638, 197)
        Me.dgvCustomerBalancePreview.TabIndex = 14
        '
        'dgvProductsPreview
        '
        Me.dgvProductsPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvProductsPreview.Location = New System.Drawing.Point(478, 524)
        Me.dgvProductsPreview.Name = "dgvProductsPreview"
        Me.dgvProductsPreview.RowHeadersWidth = 51
        Me.dgvProductsPreview.RowTemplate.Height = 24
        Me.dgvProductsPreview.Size = New System.Drawing.Size(638, 295)
        Me.dgvProductsPreview.TabIndex = 0
        '
        'dgvCustomer
        '
        Me.dgvCustomer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCustomer.Location = New System.Drawing.Point(12, 67)
        Me.dgvCustomer.Name = "dgvCustomer"
        Me.dgvCustomer.RowHeadersWidth = 51
        Me.dgvCustomer.RowTemplate.Height = 24
        Me.dgvCustomer.Size = New System.Drawing.Size(460, 248)
        Me.dgvCustomer.TabIndex = 13
        '
        'dgvOrderItemPreview
        '
        Me.dgvOrderItemPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvOrderItemPreview.Location = New System.Drawing.Point(478, 67)
        Me.dgvOrderItemPreview.Name = "dgvOrderItemPreview"
        Me.dgvOrderItemPreview.RowHeadersWidth = 51
        Me.dgvOrderItemPreview.RowTemplate.Height = 24
        Me.dgvOrderItemPreview.Size = New System.Drawing.Size(1041, 248)
        Me.dgvOrderItemPreview.TabIndex = 16
        '
        'pnlTransactionHeader
        '
        Me.pnlTransactionHeader.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.pnlTransactionHeader.Controls.Add(Me.txtLastName)
        Me.pnlTransactionHeader.Controls.Add(Me.Label3)
        Me.pnlTransactionHeader.Controls.Add(Me.btnRefresh)
        Me.pnlTransactionHeader.Controls.Add(Me.txtFirstName)
        Me.pnlTransactionHeader.Controls.Add(Me.Label1)
        Me.pnlTransactionHeader.Controls.Add(Me.btnSearch)
        Me.pnlTransactionHeader.Controls.Add(Me.Label2)
        Me.pnlTransactionHeader.Controls.Add(Me.txtCustomerAddress)
        Me.pnlTransactionHeader.Location = New System.Drawing.Point(12, 321)
        Me.pnlTransactionHeader.Name = "pnlTransactionHeader"
        Me.pnlTransactionHeader.Size = New System.Drawing.Size(460, 197)
        Me.pnlTransactionHeader.TabIndex = 12
        '
        'txtLastName
        '
        Me.txtLastName.Location = New System.Drawing.Point(129, 42)
        Me.txtLastName.Name = "txtLastName"
        Me.txtLastName.Size = New System.Drawing.Size(316, 22)
        Me.txtLastName.TabIndex = 16
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
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(19, 160)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(426, 23)
        Me.btnRefresh.TabIndex = 13
        Me.btnRefresh.Text = "Refresh Customer"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'txtFirstName
        '
        Me.txtFirstName.Location = New System.Drawing.Point(129, 14)
        Me.txtFirstName.Name = "txtFirstName"
        Me.txtFirstName.Size = New System.Drawing.Size(316, 22)
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
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(19, 131)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(426, 23)
        Me.btnSearch.TabIndex = 11
        Me.btnSearch.Text = "Search Customer"
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
        Me.txtCustomerAddress.Size = New System.Drawing.Size(316, 22)
        Me.txtCustomerAddress.TabIndex = 10
        '
        'Panel1
        '
        Me.Panel1.Location = New System.Drawing.Point(1122, 524)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(400, 295)
        Me.Panel1.TabIndex = 17
        '
        'txtCustomerBalanceTotal
        '
        Me.txtCustomerBalanceTotal.Location = New System.Drawing.Point(1251, 408)
        Me.txtCustomerBalanceTotal.Name = "txtCustomerBalanceTotal"
        Me.txtCustomerBalanceTotal.Size = New System.Drawing.Size(187, 22)
        Me.txtCustomerBalanceTotal.TabIndex = 18
        '
        'txtGrandTotal
        '
        Me.txtGrandTotal.Location = New System.Drawing.Point(1251, 436)
        Me.txtGrandTotal.Name = "txtGrandTotal"
        Me.txtGrandTotal.Size = New System.Drawing.Size(187, 22)
        Me.txtGrandTotal.TabIndex = 19
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(1185, 411)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 16)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Balance:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(1156, 439)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(89, 16)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "Total Amount:"
        '
        'btnRemoveItem
        '
        Me.btnRemoveItem.Location = New System.Drawing.Point(1387, 321)
        Me.btnRemoveItem.Name = "btnRemoveItem"
        Me.btnRemoveItem.Size = New System.Drawing.Size(132, 23)
        Me.btnRemoveItem.TabIndex = 22
        Me.btnRemoveItem.Text = "Remove Item"
        Me.btnRemoveItem.UseVisualStyleBackColor = True
        '
        'btnClearBalanceTxt
        '
        Me.btnClearBalanceTxt.Location = New System.Drawing.Point(1444, 408)
        Me.btnClearBalanceTxt.Name = "btnClearBalanceTxt"
        Me.btnClearBalanceTxt.Size = New System.Drawing.Size(75, 23)
        Me.btnClearBalanceTxt.TabIndex = 23
        Me.btnClearBalanceTxt.Text = "Clear"
        Me.btnClearBalanceTxt.UseVisualStyleBackColor = True
        '
        'frmCashierDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1531, 831)
        Me.Controls.Add(Me.btnClearBalanceTxt)
        Me.Controls.Add(Me.btnRemoveItem)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtGrandTotal)
        Me.Controls.Add(Me.txtCustomerBalanceTotal)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pnlTransactionHeader)
        Me.Controls.Add(Me.pnlInputOrderItem)
        Me.Controls.Add(Me.dgvOrderItemPreview)
        Me.Controls.Add(Me.dgvCustomer)
        Me.Controls.Add(Me.dgvProductsPreview)
        Me.Controls.Add(Me.dgvCustomerBalancePreview)
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
        CType(Me.dgvCustomerBalancePreview, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvProductsPreview, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvCustomer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvOrderItemPreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTransactionHeader.ResumeLayout(False)
        Me.pnlTransactionHeader.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblUserRole As Label
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents btnLogout As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents lblUserFullName As Label
    Friend WithEvents lblHeader As Label
    Friend WithEvents dgvCustomer As DataGridView
    Friend WithEvents dgvCustomerBalancePreview As DataGridView
    Friend WithEvents dgvOrderItemPreview As DataGridView
    Friend WithEvents dgvProductsPreview As DataGridView
    Friend WithEvents pnlInputOrderItem As Panel
    Friend WithEvents pnlTransactionHeader As Panel
    Friend WithEvents txtLastName As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents btnRefresh As Button
    Friend WithEvents txtFirstName As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnSearch As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents txtCustomerAddress As TextBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents txtCustomerBalanceTotal As TextBox
    Friend WithEvents txtGrandTotal As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents btnRemoveItem As Button
    Friend WithEvents btnClearBalanceTxt As Button
End Class
