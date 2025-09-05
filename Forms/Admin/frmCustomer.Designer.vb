<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCustomer
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.dgvCustomer = New System.Windows.Forms.DataGridView()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnDeleteCustomer = New System.Windows.Forms.Button()
        Me.btnEditCustomer = New System.Windows.Forms.Button()
        Me.btnAddCustomer = New System.Windows.Forms.Button()
        CType(Me.dgvCustomer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvCustomer
        '
        Me.dgvCustomer.AllowUserToAddRows = False
        Me.dgvCustomer.AllowUserToDeleteRows = False
        Me.dgvCustomer.AllowUserToResizeRows = False
        Me.dgvCustomer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvCustomer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvCustomer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCustomer.Location = New System.Drawing.Point(12, 12)
        Me.dgvCustomer.MultiSelect = False
        Me.dgvCustomer.Name = "dgvCustomer"
        Me.dgvCustomer.ReadOnly = True
        Me.dgvCustomer.RowHeadersVisible = False
        Me.dgvCustomer.RowHeadersWidth = 51
        Me.dgvCustomer.RowTemplate.Height = 24
        Me.dgvCustomer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvCustomer.Size = New System.Drawing.Size(1319, 596)
        Me.dgvCustomer.TabIndex = 4
        '
        'btnRefresh
        '
        Me.btnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefresh.BackColor = System.Drawing.Color.White
        Me.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRefresh.Font = New System.Drawing.Font("Segoe UI Semibold", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefresh.Location = New System.Drawing.Point(1164, 614)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(167, 57)
        Me.btnRefresh.TabIndex = 11
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'btnDeleteCustomer
        '
        Me.btnDeleteCustomer.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteCustomer.BackColor = System.Drawing.Color.Red
        Me.btnDeleteCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDeleteCustomer.Font = New System.Drawing.Font("Segoe UI Semibold", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDeleteCustomer.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.btnDeleteCustomer.Location = New System.Drawing.Point(991, 614)
        Me.btnDeleteCustomer.Name = "btnDeleteCustomer"
        Me.btnDeleteCustomer.Size = New System.Drawing.Size(167, 57)
        Me.btnDeleteCustomer.TabIndex = 14
        Me.btnDeleteCustomer.Text = "Delete Customer"
        Me.btnDeleteCustomer.UseVisualStyleBackColor = False
        '
        'btnEditCustomer
        '
        Me.btnEditCustomer.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditCustomer.BackColor = System.Drawing.Color.Lime
        Me.btnEditCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEditCustomer.Font = New System.Drawing.Font("Segoe UI Semibold", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEditCustomer.Location = New System.Drawing.Point(818, 614)
        Me.btnEditCustomer.Name = "btnEditCustomer"
        Me.btnEditCustomer.Size = New System.Drawing.Size(167, 57)
        Me.btnEditCustomer.TabIndex = 13
        Me.btnEditCustomer.Text = "Edit Customer"
        Me.btnEditCustomer.UseVisualStyleBackColor = False
        '
        'btnAddCustomer
        '
        Me.btnAddCustomer.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddCustomer.BackColor = System.Drawing.Color.Blue
        Me.btnAddCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddCustomer.Font = New System.Drawing.Font("Segoe UI Semibold", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddCustomer.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.btnAddCustomer.Location = New System.Drawing.Point(645, 614)
        Me.btnAddCustomer.Name = "btnAddCustomer"
        Me.btnAddCustomer.Size = New System.Drawing.Size(167, 57)
        Me.btnAddCustomer.TabIndex = 12
        Me.btnAddCustomer.Text = "Add Customer"
        Me.btnAddCustomer.UseVisualStyleBackColor = False
        '
        'frmCustomer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.GrayText
        Me.ClientSize = New System.Drawing.Size(1343, 770)
        Me.Controls.Add(Me.btnDeleteCustomer)
        Me.Controls.Add(Me.btnEditCustomer)
        Me.Controls.Add(Me.btnAddCustomer)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.dgvCustomer)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmCustomer"
        Me.Text = "frmCustomer"
        CType(Me.dgvCustomer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvCustomer As DataGridView
    Friend WithEvents btnRefresh As Button
    Friend WithEvents btnDeleteCustomer As Button
    Friend WithEvents btnEditCustomer As Button
    Friend WithEvents btnAddCustomer As Button
End Class
