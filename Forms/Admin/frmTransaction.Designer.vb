<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmTransaction
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
        Me.dgvTransaction = New System.Windows.Forms.DataGridView()
        Me.btnRefreshTransaction = New System.Windows.Forms.Button()
        CType(Me.dgvTransaction, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvTransaction
        '
        Me.dgvTransaction.AllowUserToAddRows = False
        Me.dgvTransaction.AllowUserToDeleteRows = False
        Me.dgvTransaction.AllowUserToResizeRows = False
        Me.dgvTransaction.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvTransaction.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvTransaction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTransaction.Location = New System.Drawing.Point(12, 12)
        Me.dgvTransaction.MultiSelect = False
        Me.dgvTransaction.Name = "dgvTransaction"
        Me.dgvTransaction.ReadOnly = True
        Me.dgvTransaction.RowHeadersVisible = False
        Me.dgvTransaction.RowHeadersWidth = 51
        Me.dgvTransaction.RowTemplate.Height = 24
        Me.dgvTransaction.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTransaction.Size = New System.Drawing.Size(1319, 596)
        Me.dgvTransaction.TabIndex = 4
        '
        'btnRefreshTransaction
        '
        Me.btnRefreshTransaction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefreshTransaction.BackColor = System.Drawing.Color.White
        Me.btnRefreshTransaction.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRefreshTransaction.Font = New System.Drawing.Font("Segoe UI", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefreshTransaction.Location = New System.Drawing.Point(1127, 614)
        Me.btnRefreshTransaction.Name = "btnRefreshTransaction"
        Me.btnRefreshTransaction.Size = New System.Drawing.Size(204, 72)
        Me.btnRefreshTransaction.TabIndex = 5
        Me.btnRefreshTransaction.Text = "Refresh"
        Me.btnRefreshTransaction.UseVisualStyleBackColor = False
        '
        'frmTransaction
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.GrayText
        Me.ClientSize = New System.Drawing.Size(1343, 770)
        Me.Controls.Add(Me.btnRefreshTransaction)
        Me.Controls.Add(Me.dgvTransaction)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmTransaction"
        Me.Text = "frmTransaction"
        CType(Me.dgvTransaction, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvTransaction As DataGridView
    Friend WithEvents btnRefreshTransaction As Button
End Class
