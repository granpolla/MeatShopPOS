<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmCashierPaymentInput
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
        Me.cboPaymentMethod = New System.Windows.Forms.ComboBox()
        Me.dgvPaymentEntries = New System.Windows.Forms.DataGridView()
        Me.txtRefNum = New System.Windows.Forms.TextBox()
        Me.txtAmount = New System.Windows.Forms.TextBox()
        Me.btnAddPayment = New System.Windows.Forms.Button()
        Me.btnSaveAndPrint = New System.Windows.Forms.Button()
        Me.btnClearDgvPaymentEntries = New System.Windows.Forms.Button()
        Me.txtChange = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        CType(Me.dgvPaymentEntries, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cboPaymentMethod
        '
        Me.cboPaymentMethod.FormattingEnabled = True
        Me.cboPaymentMethod.Location = New System.Drawing.Point(127, 133)
        Me.cboPaymentMethod.Name = "cboPaymentMethod"
        Me.cboPaymentMethod.Size = New System.Drawing.Size(141, 24)
        Me.cboPaymentMethod.TabIndex = 0
        '
        'dgvPaymentEntries
        '
        Me.dgvPaymentEntries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPaymentEntries.Location = New System.Drawing.Point(12, 12)
        Me.dgvPaymentEntries.Name = "dgvPaymentEntries"
        Me.dgvPaymentEntries.RowHeadersWidth = 51
        Me.dgvPaymentEntries.RowTemplate.Height = 24
        Me.dgvPaymentEntries.Size = New System.Drawing.Size(337, 115)
        Me.dgvPaymentEntries.TabIndex = 1
        '
        'txtRefNum
        '
        Me.txtRefNum.Location = New System.Drawing.Point(127, 163)
        Me.txtRefNum.Name = "txtRefNum"
        Me.txtRefNum.Size = New System.Drawing.Size(141, 22)
        Me.txtRefNum.TabIndex = 2
        '
        'txtAmount
        '
        Me.txtAmount.Location = New System.Drawing.Point(127, 191)
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.Size = New System.Drawing.Size(141, 22)
        Me.txtAmount.TabIndex = 3
        '
        'btnAddPayment
        '
        Me.btnAddPayment.Location = New System.Drawing.Point(274, 134)
        Me.btnAddPayment.Name = "btnAddPayment"
        Me.btnAddPayment.Size = New System.Drawing.Size(75, 23)
        Me.btnAddPayment.TabIndex = 4
        Me.btnAddPayment.Text = "Add"
        Me.btnAddPayment.UseVisualStyleBackColor = True
        '
        'btnSaveAndPrint
        '
        Me.btnSaveAndPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSaveAndPrint.BackColor = System.Drawing.Color.Lime
        Me.btnSaveAndPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSaveAndPrint.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveAndPrint.ForeColor = System.Drawing.SystemColors.WindowText
        Me.btnSaveAndPrint.Location = New System.Drawing.Point(12, 265)
        Me.btnSaveAndPrint.Name = "btnSaveAndPrint"
        Me.btnSaveAndPrint.Size = New System.Drawing.Size(337, 42)
        Me.btnSaveAndPrint.TabIndex = 5
        Me.btnSaveAndPrint.Text = "Save and Print"
        Me.btnSaveAndPrint.UseVisualStyleBackColor = False
        '
        'btnClearDgvPaymentEntries
        '
        Me.btnClearDgvPaymentEntries.Location = New System.Drawing.Point(274, 163)
        Me.btnClearDgvPaymentEntries.Name = "btnClearDgvPaymentEntries"
        Me.btnClearDgvPaymentEntries.Size = New System.Drawing.Size(75, 22)
        Me.btnClearDgvPaymentEntries.TabIndex = 6
        Me.btnClearDgvPaymentEntries.Text = "Clear"
        Me.btnClearDgvPaymentEntries.UseVisualStyleBackColor = True
        '
        'txtChange
        '
        Me.txtChange.Location = New System.Drawing.Point(127, 219)
        Me.txtChange.Name = "txtChange"
        Me.txtChange.Size = New System.Drawing.Size(141, 22)
        Me.txtChange.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 134)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(108, 16)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Payment Method"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(39, 166)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 16)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Ref Number:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(69, 192)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(52, 16)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Amount"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(69, 220)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(54, 16)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Change"
        '
        'frmCashierPaymentInput
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.ClientSize = New System.Drawing.Size(361, 320)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtChange)
        Me.Controls.Add(Me.btnClearDgvPaymentEntries)
        Me.Controls.Add(Me.btnSaveAndPrint)
        Me.Controls.Add(Me.btnAddPayment)
        Me.Controls.Add(Me.txtAmount)
        Me.Controls.Add(Me.txtRefNum)
        Me.Controls.Add(Me.dgvPaymentEntries)
        Me.Controls.Add(Me.cboPaymentMethod)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmCashierPaymentInput"
        Me.Text = "frmCashierPaymentInput"
        CType(Me.dgvPaymentEntries, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cboPaymentMethod As ComboBox
    Friend WithEvents dgvPaymentEntries As DataGridView
    Friend WithEvents txtRefNum As TextBox
    Friend WithEvents txtAmount As TextBox
    Friend WithEvents btnAddPayment As Button
    Friend WithEvents btnSaveAndPrint As Button
    Friend WithEvents btnClearDgvPaymentEntries As Button
    Friend WithEvents txtChange As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
End Class
