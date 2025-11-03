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
        Me.txtSearchTransaction = New System.Windows.Forms.TextBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.dtpDateFilter = New System.Windows.Forms.DateTimePicker()
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
        Me.dgvTransaction.Location = New System.Drawing.Point(12, 56)
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
        Me.btnRefreshTransaction.Location = New System.Drawing.Point(1127, 666)
        Me.btnRefreshTransaction.Name = "btnRefreshTransaction"
        Me.btnRefreshTransaction.Size = New System.Drawing.Size(204, 72)
        Me.btnRefreshTransaction.TabIndex = 5
        Me.btnRefreshTransaction.Text = "Refresh"
        Me.btnRefreshTransaction.UseVisualStyleBackColor = False
        '
        'txtSearchTransaction
        '
        Me.txtSearchTransaction.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSearchTransaction.Font = New System.Drawing.Font("Segoe UI", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearchTransaction.Location = New System.Drawing.Point(12, 12)
        Me.txtSearchTransaction.Name = "txtSearchTransaction"
        Me.txtSearchTransaction.Size = New System.Drawing.Size(249, 31)
        Me.txtSearchTransaction.TabIndex = 6
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(272, 12)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(141, 31)
        Me.btnSearch.TabIndex = 7
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'dtpDateFilter
        '
        Me.dtpDateFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtpDateFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpDateFilter.Location = New System.Drawing.Point(909, 9)
        Me.dtpDateFilter.Name = "dtpDateFilter"
        Me.dtpDateFilter.Size = New System.Drawing.Size(422, 34)
        Me.dtpDateFilter.TabIndex = 8
        '
        'frmTransaction
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.GrayText
        Me.ClientSize = New System.Drawing.Size(1343, 770)
        Me.Controls.Add(Me.dtpDateFilter)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.txtSearchTransaction)
        Me.Controls.Add(Me.btnRefreshTransaction)
        Me.Controls.Add(Me.dgvTransaction)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmTransaction"
        Me.Text = "frmTransaction"
        CType(Me.dgvTransaction, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvTransaction As DataGridView
    Friend WithEvents btnRefreshTransaction As Button
    Friend WithEvents txtSearchTransaction As TextBox
    Friend WithEvents btnSearch As Button
    Friend WithEvents dtpDateFilter As DateTimePicker
End Class
