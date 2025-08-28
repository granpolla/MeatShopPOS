<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCashierInputOrderItem
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
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtTotalBox = New System.Windows.Forms.TextBox()
        Me.btnSearchProduct = New System.Windows.Forms.Button()
        Me.txtProductName = New System.Windows.Forms.TextBox()
        Me.txtUnitWeight = New System.Windows.Forms.TextBox()
        Me.txtUnitPrice = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtTotal = New System.Windows.Forms.TextBox()
        Me.btnRefreshProduct = New System.Windows.Forms.Button()
        Me.btnAddItem = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(266, 223)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(67, 16)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "Total Box:"
        '
        'txtTotalBox
        '
        Me.txtTotalBox.Location = New System.Drawing.Point(339, 220)
        Me.txtTotalBox.Name = "txtTotalBox"
        Me.txtTotalBox.Size = New System.Drawing.Size(264, 22)
        Me.txtTotalBox.TabIndex = 24
        '
        'btnSearchProduct
        '
        Me.btnSearchProduct.Location = New System.Drawing.Point(620, 108)
        Me.btnSearchProduct.Name = "btnSearchProduct"
        Me.btnSearchProduct.Size = New System.Drawing.Size(133, 23)
        Me.btnSearchProduct.TabIndex = 23
        Me.btnSearchProduct.Text = "Search"
        Me.btnSearchProduct.UseVisualStyleBackColor = True
        '
        'txtProductName
        '
        Me.txtProductName.Location = New System.Drawing.Point(339, 108)
        Me.txtProductName.Name = "txtProductName"
        Me.txtProductName.Size = New System.Drawing.Size(264, 22)
        Me.txtProductName.TabIndex = 22
        '
        'txtUnitWeight
        '
        Me.txtUnitWeight.Location = New System.Drawing.Point(339, 136)
        Me.txtUnitWeight.Name = "txtUnitWeight"
        Me.txtUnitWeight.Size = New System.Drawing.Size(264, 22)
        Me.txtUnitWeight.TabIndex = 21
        '
        'txtUnitPrice
        '
        Me.txtUnitPrice.Location = New System.Drawing.Point(339, 164)
        Me.txtUnitPrice.Name = "txtUnitPrice"
        Me.txtUnitPrice.Size = New System.Drawing.Size(264, 22)
        Me.txtUnitPrice.TabIndex = 20
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(293, 195)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(41, 16)
        Me.Label6.TabIndex = 19
        Me.Label6.Text = "Total:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(267, 167)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(67, 16)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "Unit Price:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(256, 139)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(78, 16)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "Unit Weight:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(238, 111)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 16)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "Product Name:"
        '
        'txtTotal
        '
        Me.txtTotal.Location = New System.Drawing.Point(339, 192)
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.Size = New System.Drawing.Size(264, 22)
        Me.txtTotal.TabIndex = 15
        '
        'btnRefreshProduct
        '
        Me.btnRefreshProduct.Location = New System.Drawing.Point(620, 136)
        Me.btnRefreshProduct.Name = "btnRefreshProduct"
        Me.btnRefreshProduct.Size = New System.Drawing.Size(133, 23)
        Me.btnRefreshProduct.TabIndex = 26
        Me.btnRefreshProduct.Text = "Refresh"
        Me.btnRefreshProduct.UseVisualStyleBackColor = True
        '
        'btnAddItem
        '
        Me.btnAddItem.Location = New System.Drawing.Point(259, 272)
        Me.btnAddItem.Name = "btnAddItem"
        Me.btnAddItem.Size = New System.Drawing.Size(352, 29)
        Me.btnAddItem.TabIndex = 27
        Me.btnAddItem.Text = "Add Item"
        Me.btnAddItem.UseVisualStyleBackColor = True
        '
        'frmCashierInputOrderItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(990, 350)
        Me.Controls.Add(Me.btnAddItem)
        Me.Controls.Add(Me.btnRefreshProduct)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtTotalBox)
        Me.Controls.Add(Me.btnSearchProduct)
        Me.Controls.Add(Me.txtProductName)
        Me.Controls.Add(Me.txtUnitWeight)
        Me.Controls.Add(Me.txtUnitPrice)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtTotal)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmCashierInputOrderItem"
        Me.Text = "frmCashierInputOrderItem"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label7 As Label
    Friend WithEvents txtTotalBox As TextBox
    Friend WithEvents btnSearchProduct As Button
    Friend WithEvents txtProductName As TextBox
    Friend WithEvents txtUnitWeight As TextBox
    Friend WithEvents txtUnitPrice As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtTotal As TextBox
    Friend WithEvents btnRefreshProduct As Button
    Friend WithEvents btnAddItem As Button
End Class
