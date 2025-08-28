<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmDashboard
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txtCashierSix = New System.Windows.Forms.TextBox()
        Me.txtCashierFive = New System.Windows.Forms.TextBox()
        Me.txtCashierFour = New System.Windows.Forms.TextBox()
        Me.txtCashierThree = New System.Windows.Forms.TextBox()
        Me.txtCashierTwo = New System.Windows.Forms.TextBox()
        Me.txtCashierOne = New System.Windows.Forms.TextBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.MySqlDataAdapter1 = New MySql.Data.MySqlClient.MySqlDataAdapter()
        Me.Panel1.SuspendLayout()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.txtCashierSix)
        Me.Panel1.Controls.Add(Me.txtCashierFive)
        Me.Panel1.Controls.Add(Me.txtCashierFour)
        Me.Panel1.Controls.Add(Me.txtCashierThree)
        Me.Panel1.Controls.Add(Me.txtCashierTwo)
        Me.Panel1.Controls.Add(Me.txtCashierOne)
        Me.Panel1.Location = New System.Drawing.Point(846, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(261, 601)
        Me.Panel1.TabIndex = 0
        '
        'txtCashierSix
        '
        Me.txtCashierSix.BackColor = System.Drawing.Color.White
        Me.txtCashierSix.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtCashierSix.Location = New System.Drawing.Point(3, 182)
        Me.txtCashierSix.Name = "txtCashierSix"
        Me.txtCashierSix.ReadOnly = True
        Me.txtCashierSix.Size = New System.Drawing.Size(183, 15)
        Me.txtCashierSix.TabIndex = 5
        '
        'txtCashierFive
        '
        Me.txtCashierFive.BackColor = System.Drawing.Color.White
        Me.txtCashierFive.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtCashierFive.Location = New System.Drawing.Point(3, 154)
        Me.txtCashierFive.Name = "txtCashierFive"
        Me.txtCashierFive.ReadOnly = True
        Me.txtCashierFive.Size = New System.Drawing.Size(183, 15)
        Me.txtCashierFive.TabIndex = 4
        '
        'txtCashierFour
        '
        Me.txtCashierFour.BackColor = System.Drawing.Color.White
        Me.txtCashierFour.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtCashierFour.Location = New System.Drawing.Point(3, 126)
        Me.txtCashierFour.Name = "txtCashierFour"
        Me.txtCashierFour.ReadOnly = True
        Me.txtCashierFour.Size = New System.Drawing.Size(183, 15)
        Me.txtCashierFour.TabIndex = 3
        '
        'txtCashierThree
        '
        Me.txtCashierThree.BackColor = System.Drawing.Color.White
        Me.txtCashierThree.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtCashierThree.Location = New System.Drawing.Point(3, 98)
        Me.txtCashierThree.Name = "txtCashierThree"
        Me.txtCashierThree.ReadOnly = True
        Me.txtCashierThree.Size = New System.Drawing.Size(183, 15)
        Me.txtCashierThree.TabIndex = 2
        '
        'txtCashierTwo
        '
        Me.txtCashierTwo.BackColor = System.Drawing.Color.White
        Me.txtCashierTwo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtCashierTwo.Location = New System.Drawing.Point(3, 70)
        Me.txtCashierTwo.Name = "txtCashierTwo"
        Me.txtCashierTwo.ReadOnly = True
        Me.txtCashierTwo.Size = New System.Drawing.Size(183, 15)
        Me.txtCashierTwo.TabIndex = 1
        '
        'txtCashierOne
        '
        Me.txtCashierOne.BackColor = System.Drawing.Color.White
        Me.txtCashierOne.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtCashierOne.Location = New System.Drawing.Point(3, 42)
        Me.txtCashierOne.Name = "txtCashierOne"
        Me.txtCashierOne.ReadOnly = True
        Me.txtCashierOne.Size = New System.Drawing.Size(183, 15)
        Me.txtCashierOne.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Location = New System.Drawing.Point(12, 12)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(828, 197)
        Me.Panel2.TabIndex = 1
        '
        'Chart1
        '
        ChartArea1.Name = "ChartArea1"
        Me.Chart1.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.Chart1.Legends.Add(Legend1)
        Me.Chart1.Location = New System.Drawing.Point(12, 215)
        Me.Chart1.Name = "Chart1"
        Series1.ChartArea = "ChartArea1"
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.Chart1.Series.Add(Series1)
        Me.Chart1.Size = New System.Drawing.Size(828, 398)
        Me.Chart1.TabIndex = 2
        Me.Chart1.Text = "Chart1"
        '
        'MySqlDataAdapter1
        '
        Me.MySqlDataAdapter1.DeleteCommand = Nothing
        Me.MySqlDataAdapter1.InsertCommand = Nothing
        Me.MySqlDataAdapter1.SelectCommand = Nothing
        Me.MySqlDataAdapter1.UpdateCommand = Nothing
        '
        'frmDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1119, 625)
        Me.Controls.Add(Me.Chart1)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmDashboard"
        Me.Text = "frmDashboard"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Chart1 As DataVisualization.Charting.Chart
    Friend WithEvents txtCashierSix As TextBox
    Friend WithEvents txtCashierFive As TextBox
    Friend WithEvents txtCashierFour As TextBox
    Friend WithEvents txtCashierThree As TextBox
    Friend WithEvents txtCashierTwo As TextBox
    Friend WithEvents txtCashierOne As TextBox
    Friend WithEvents MySqlDataAdapter1 As MySql.Data.MySqlClient.MySqlDataAdapter
End Class
