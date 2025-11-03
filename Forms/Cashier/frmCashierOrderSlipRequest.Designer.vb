<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCashierOrderSlipRequest
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
        Me.btnPrintOrderSlip = New System.Windows.Forms.Button()
        Me.txtTin = New System.Windows.Forms.TextBox()
        Me.txtBusStyle = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnPrintOrderSlip
        '
        Me.btnPrintOrderSlip.BackColor = System.Drawing.Color.Lime
        Me.btnPrintOrderSlip.Font = New System.Drawing.Font("Segoe UI Semibold", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrintOrderSlip.Location = New System.Drawing.Point(13, 265)
        Me.btnPrintOrderSlip.Name = "btnPrintOrderSlip"
        Me.btnPrintOrderSlip.Size = New System.Drawing.Size(406, 57)
        Me.btnPrintOrderSlip.TabIndex = 0
        Me.btnPrintOrderSlip.Text = "PRINT"
        Me.btnPrintOrderSlip.UseVisualStyleBackColor = False
        '
        'txtTin
        '
        Me.txtTin.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTin.Font = New System.Drawing.Font("Segoe UI", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTin.Location = New System.Drawing.Point(13, 53)
        Me.txtTin.Name = "txtTin"
        Me.txtTin.Size = New System.Drawing.Size(406, 31)
        Me.txtTin.TabIndex = 1
        '
        'txtBusStyle
        '
        Me.txtBusStyle.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtBusStyle.Font = New System.Drawing.Font("Segoe UI", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBusStyle.Location = New System.Drawing.Point(13, 150)
        Me.txtBusStyle.Name = "txtBusStyle"
        Me.txtBusStyle.Size = New System.Drawing.Size(406, 31)
        Me.txtBusStyle.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(9, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 20)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "TIN:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(9, 127)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(85, 20)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "BUS STYLE:"
        '
        'frmCashierOrderSlipRequest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(431, 336)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtBusStyle)
        Me.Controls.Add(Me.txtTin)
        Me.Controls.Add(Me.btnPrintOrderSlip)
        Me.Name = "frmCashierOrderSlipRequest"
        Me.Text = "Order Slip Request"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnPrintOrderSlip As Button
    Friend WithEvents txtTin As TextBox
    Friend WithEvents txtBusStyle As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
End Class
