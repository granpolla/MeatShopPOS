Imports MySql.Data.MySqlClient
Imports System.Drawing
Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmDashboard

    Private Sub frmDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ✅ Populate ComboBox
        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("Revenue")
        ComboBox1.Items.Add("Payments Received")
        ComboBox1.SelectedIndex = 0 ' Default selection
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList

        LoadCashiers()
        LoadMonthlySalesChart()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        LoadCashiers()
        LoadMonthlySalesChart()
    End Sub

    Private Sub LoadCashiers()
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                ' ✅ Decide which column to SUM based on selection
                Dim salesColumn As String
                If ComboBox1.SelectedItem.ToString() = "Revenue" Then
                    salesColumn = "st.total_amount"
                Else
                    salesColumn = "st.amount_paid"
                End If

                Dim query As String = $"
                    SELECT u.id, u.full_name, 
                           IFNULL(SUM({salesColumn}), 0) AS total_sales
                    FROM user u
                    INNER JOIN role r ON u.role_id = r.id
                    LEFT JOIN sales_transaction st ON u.id = st.user_id
                    WHERE r.role_name = 'cashier'
                    GROUP BY u.id, u.full_name
                    ORDER BY u.id ASC;
                "

                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()

                        ' ✅ Clear previous controls
                        For i As Integer = Panel1.Controls.Count - 1 To 0 Step -1
                            Dim ctrl = Panel1.Controls(i)
                            If TypeOf ctrl Is TextBox AndAlso ctrl.Name.StartsWith("txtCashier") Then
                                Panel1.Controls.Remove(ctrl)
                            End If
                        Next

                        Dim yPos As Integer = 82   ' starting Y position inside Panel1
                        Dim index As Integer = 1
                        Dim grandTotal As Decimal = 0

                        While reader.Read()
                            Dim fullName As String = reader("full_name").ToString()
                            Dim totalSales As Decimal = Convert.ToDecimal(reader("total_sales"))
                            grandTotal += totalSales

                            ' ✅ Cashier Name TextBox
                            Dim txtName As New TextBox()
                            txtName.Name = "txtCashier" & index
                            txtName.Text = fullName
                            txtName.ReadOnly = True
                            txtName.Width = 110
                            txtName.BorderStyle = BorderStyle.None
                            txtName.BackColor = Color.White
                            txtName.Font = New Font("Segoe UI", 10)
                            txtName.Location = New Point(3, yPos)

                            ' ✅ Sales Total TextBox
                            Dim txtSalesBox As New TextBox()
                            txtSalesBox.Name = "txtCashierSales" & index
                            txtSalesBox.Text = totalSales.ToString("N2") ' format 12,000.00
                            txtSalesBox.ReadOnly = True
                            txtSalesBox.Width = 70
                            txtSalesBox.TextAlign = HorizontalAlignment.Right
                            txtSalesBox.Font = New Font("Segoe UI", 10, FontStyle.Bold)

                            txtSalesBox.BorderStyle = BorderStyle.None
                            txtSalesBox.BackColor = Color.White
                            txtSalesBox.Location = New Point(130, yPos)

                            ' Add them to Panel1
                            Panel1.Controls.Add(txtName)
                            Panel1.Controls.Add(txtSalesBox)

                            yPos += 30  ' spacing between rows
                            index += 1
                        End While

                        ' ✅ Show grand total in txtSales
                        txtSales.Text = grandTotal.ToString("N2")

                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading cashiers: " & ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadMonthlySalesChart()
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                ' ✅ Decide column for chart
                Dim salesColumn As String
                If ComboBox1.SelectedItem.ToString() = "Revenue" Then
                    salesColumn = "st.total_amount"
                Else
                    salesColumn = "st.amount_paid"
                End If

                ' ✅ Current year monthly sales
                Dim query As String = $"
                    SELECT MONTH(st.order_datetime) AS sales_month,
                           IFNULL(SUM({salesColumn}), 0) AS monthly_total
                    FROM sales_transaction st
                    WHERE YEAR(st.order_datetime) = YEAR(CURDATE())
                    GROUP BY MONTH(st.order_datetime)
                    ORDER BY sales_month;
                "

                Dim salesData As New Dictionary(Of Integer, Decimal)()

                ' Initialize months with 0
                For m As Integer = 1 To 12
                    salesData(m) = 0
                Next

                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim monthNum As Integer = Convert.ToInt32(reader("sales_month"))
                            Dim total As Decimal = Convert.ToDecimal(reader("monthly_total"))
                            salesData(monthNum) = total
                        End While
                    End Using
                End Using

                ' ✅ Configure Chart
                Chart1.Series.Clear()
                Chart1.ChartAreas(0).AxisX.Interval = 1
                Chart1.ChartAreas(0).AxisX.MajorGrid.LineWidth = 0
                Chart1.ChartAreas(0).AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash

                Dim series As New Series("Monthly Sales")
                series.ChartType = SeriesChartType.Column
                series.Color = Color.SteelBlue
                series.IsValueShownAsLabel = True

                For m As Integer = 1 To 12
                    Dim monthName As String = New DateTime(2025, m, 1).ToString("MMM")
                    series.Points.AddXY(monthName, salesData(m))
                Next

                Chart1.Series.Add(series)

            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading monthly chart: " & ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

End Class
