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
        Me.SuspendLayout()
        '
        'btnPrintOrderSlip
        '
        Me.btnPrintOrderSlip.Location = New System.Drawing.Point(81, 193)
        Me.btnPrintOrderSlip.Name = "btnPrintOrderSlip"
        Me.btnPrintOrderSlip.Size = New System.Drawing.Size(352, 23)
        Me.btnPrintOrderSlip.TabIndex = 0
        Me.btnPrintOrderSlip.Text = "Button1"
        Me.btnPrintOrderSlip.UseVisualStyleBackColor = True
        '
        'txtTin
        '
        Me.txtTin.Location = New System.Drawing.Point(81, 52)
        Me.txtTin.Name = "txtTin"
        Me.txtTin.Size = New System.Drawing.Size(100, 22)
        Me.txtTin.TabIndex = 1
        '
        'txtBusStyle
        '
        Me.txtBusStyle.Location = New System.Drawing.Point(81, 106)
        Me.txtBusStyle.Name = "txtBusStyle"
        Me.txtBusStyle.Size = New System.Drawing.Size(100, 22)
        Me.txtBusStyle.TabIndex = 2
        '
        'frmCashierOrderSlipRequest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.txtBusStyle)
        Me.Controls.Add(Me.txtTin)
        Me.Controls.Add(Me.btnPrintOrderSlip)
        Me.Name = "frmCashierOrderSlipRequest"
        Me.Text = "frmCashierOrderSlipRequest"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnPrintOrderSlip As Button
    Friend WithEvents txtTin As TextBox
    Friend WithEvents txtBusStyle As TextBox
End Class
