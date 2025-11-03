'Imports MySql.Data.MySqlClient
'Imports System.Drawing
'Imports System.Windows.Forms.DataVisualization.Charting

'Public Class frmDashboard

'    ' 1. Module-level variable to store the selected filter date
'    Private selectedSalesDate As Date = Date.Today

'    Private Sub frmDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        ' Initialize DatePicker
'        dtpSalesDate.Value = Date.Today
'        selectedSalesDate = Date.Today

'        ' ✅ Populate ComboBox
'        ComboBox1.Items.Clear()
'        ComboBox1.Items.Add("Revenue")
'        ComboBox1.Items.Add("Payments Received")
'        ComboBox1.SelectedIndex = 0 ' Default selection
'        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList

'        LoadCashiers()
'        LoadMonthlySalesChart()
'        LoadTopProductsChart()
'        LoadTotals()
'        LoadMonthlyTotal() ' ✅ New call: txtSales total is now loaded here (monthly)
'    End Sub

'    ' 3. New event handler for the DateTimePicker
'    Private Sub dtpSalesDate_ValueChanged(sender As Object, e As EventArgs) Handles dtpSalesDate.ValueChanged
'        ' Update the filter date and refresh the cashier list (Day Filter)
'        selectedSalesDate = dtpSalesDate.Value.Date
'        LoadCashiers()
'        ' Note: txtSales is NOT updated here, as it is now monthly.
'    End Sub

'    Private Sub RefreshAll()
'        LoadCashiers()
'        LoadMonthlySalesChart()
'        LoadTopProductsChart()
'        LoadTotals()
'        LoadMonthlyTotal() ' ✅ New call
'    End Sub

'    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
'        ' ComboBox1 affects Cashiers (Daily Filter) and Charts/txtSales (Monthly Filter)
'        LoadCashiers()
'        LoadMonthlySalesChart()
'        LoadMonthlyTotal() ' ✅ New call
'    End Sub

'    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
'        RefreshAll()
'    End Sub

'    ' -----------------------------------------------------------
'    ' NEW SUBROUTINE: Calculates the Monthly Total for txtSales (Filtered by ComboBox1)
'    ' -----------------------------------------------------------
'    Private Sub LoadMonthlyTotal()
'        Try
'            Using conn As New MySqlConnection(My.Settings.DBConnection)
'                conn.Open()

'                Dim isRevenue As Boolean = If(ComboBox1.SelectedItem IsNot Nothing, ComboBox1.SelectedItem.ToString() = "Revenue", True)
'                Dim query As String

'                If isRevenue Then
'                    ' Monthly Revenue = sum of total_amount for the current month/year
'                    query = "
'                    SELECT IFNULL(SUM(st.total_amount),0) AS overall_total
'                    FROM sales_transaction st
'                    WHERE MONTH(st.order_datetime) = MONTH(CURDATE())
'                      AND YEAR(st.order_datetime) = YEAR(CURDATE());"
'                Else
'                    ' Monthly Payments Received = sum of amount_paid + settled balances for the current month/year
'                    query = "
'                    SELECT IFNULL(SUM(st.amount_paid + COALESCE(cl.amount * -1,0)),0) AS overall_total
'                    FROM sales_transaction st
'                    LEFT JOIN customer_ledger cl ON cl.related_transaction_id = st.id
'                    WHERE MONTH(st.order_datetime) = MONTH(CURDATE())
'                      AND YEAR(st.order_datetime) = YEAR(CURDATE());"
'                End If

'                Using cmd As New MySqlCommand(query, conn)
'                    Dim monthlyTotal As Decimal = Convert.ToDecimal(cmd.ExecuteScalar())
'                    ' Update txtSales with the MONTHLY total
'                    txtSales.Text = monthlyTotal.ToString("N2")
'                End Using
'            End Using

'        Catch ex As Exception
'            MessageBox.Show("Error loading monthly total: " & ex.Message,
'                        "Error",
'                        MessageBoxButtons.OK,
'                        MessageBoxIcon.Error)
'        End Try
'    End Sub
'    ' -----------------------------------------------------------

'    ' 4. MODIFIED LoadCashiers: **Removes the update of txtSales (Grand Total)**
'    Private Sub LoadCashiers()
'        Try
'            Using conn As New MySqlConnection(My.Settings.DBConnection)
'                conn.Open()

'                ' Cashiers list is filtered by selectedSalesDate (Day) and ComboBox1 (Metric)
'                Dim isRevenue As Boolean = If(ComboBox1.SelectedItem IsNot Nothing, ComboBox1.SelectedItem.ToString() = "Revenue", True)

'                Dim query As String
'                ' ... (rest of query logic remains the same) ...
'                If isRevenue Then
'                    query = "
'                    SELECT u.id, u.full_name,
'                            IFNULL(SUM(st.total_amount),0) AS total_sales
'                    FROM user u
'                    INNER JOIN role r ON u.role_id = r.id
'                    LEFT JOIN sales_transaction st ON u.id = st.user_id AND DATE(st.order_datetime) = @SalesDate
'                    WHERE r.role_name = 'cashier'
'                    GROUP BY u.id, u.full_name
'                    ORDER BY u.id ASC;"
'                Else
'                    query = "
'                    SELECT u.id, u.full_name,
'                            IFNULL(SUM(st.amount_paid + COALESCE(cl.amount * -1,0)),0) AS total_sales
'                    FROM user u
'                    INNER JOIN role r ON u.role_id = r.id
'                    LEFT JOIN sales_transaction st ON u.id = st.user_id AND DATE(st.order_datetime) = @SalesDate
'                    LEFT JOIN customer_ledger cl ON cl.related_transaction_id = st.id
'                    WHERE r.role_name = 'cashier'
'                    GROUP BY u.id, u.full_name
'                    ORDER BY u.id ASC;"
'                End If

'                Using cmd As New MySqlCommand(query, conn)
'                    cmd.Parameters.AddWithValue("@SalesDate", selectedSalesDate.Date)

'                    Using reader As MySqlDataReader = cmd.ExecuteReader()
'                        ' ... (code for clearing existing textboxes) ...
'                        For i As Integer = Panel1.Controls.Count - 1 To 0 Step -1
'                            Dim ctrl = Panel1.Controls(i)
'                            If TypeOf ctrl Is TextBox AndAlso ctrl.Name.StartsWith("txtCashier") Then
'                                Panel1.Controls.Remove(ctrl)
'                            End If
'                        Next

'                        Dim yPos As Integer = 82
'                        Dim index As Integer = 1
'                        ' Removed: Dim grandTotal As Decimal = 0

'                        While reader.Read()
'                            Dim fullName As String = reader("full_name").ToString()
'                            Dim totalSales As Decimal = Convert.ToDecimal(reader("total_sales"))
'                            ' Removed: grandTotal += totalSales

'                            ' Cashier Name
'                            Dim txtName As New TextBox() With {
'                            .Name = "txtCashier" & index,
'                            .Text = fullName,
'                            .ReadOnly = True,
'                            .Width = 110,
'                            .BorderStyle = BorderStyle.None,
'                            .BackColor = Color.White,
'                            .Font = New Font("Segoe UI", 10),
'                            .Location = New Point(30, yPos)
'                            }

'                            ' Sales Total
'                            Dim txtSalesBox As New TextBox() With {
'                            .Name = "txtCashierSales" & index,
'                            .Text = totalSales.ToString("N2"),
'                            .ReadOnly = True,
'                            .Width = 70,
'                            .TextAlign = HorizontalAlignment.Right,
'                            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
'                            .BorderStyle = BorderStyle.None,
'                            .BackColor = Color.White,
'                            .Location = New Point(150, yPos)
'                            }

'                            Panel1.Controls.Add(txtName)
'                            Panel1.Controls.Add(txtSalesBox)

'                            yPos += 30
'                            index += 1
'                        End While

'                        ' **REMOVED: txtSales.Text = grandTotal.ToString("N2")**
'                        ' The total is now updated by LoadMonthlyTotal() to reflect the monthly figure.
'                    End Using
'                End Using
'            End Using

'        Catch ex As Exception
'            MessageBox.Show("Error loading cashiers: " & ex.Message,
'                        "Error",
'                        MessageBoxButtons.OK,
'                        MessageBoxIcon.Error)
'        End Try
'    End Sub


'    Private Sub LoadMonthlySalesChart()
'        Try
'            Using conn As New MySqlConnection(My.Settings.DBConnection)
'                conn.Open()

'                ' This function already uses the monthly filter, correctly
'                Dim isRevenue As Boolean = If(ComboBox1.SelectedItem IsNot Nothing, ComboBox1.SelectedItem.ToString() = "Revenue", True)

'                Dim query As String

'                If isRevenue Then
'                    ' Revenue = sum of total_amount
'                    query = "
'                    SELECT MONTH(st.order_datetime) AS sales_month,
'                            IFNULL(SUM(st.total_amount),0) AS monthly_total
'                    FROM sales_transaction st
'                    WHERE YEAR(st.order_datetime) = YEAR(CURDATE())
'                    GROUP BY MONTH(st.order_datetime)
'                    ORDER BY sales_month;"
'                Else
'                    ' Payments Received = sum of amount_paid + settled balances
'                    query = "
'                    SELECT MONTH(st.order_datetime) AS sales_month,
'                            IFNULL(SUM(st.amount_paid + COALESCE(cl.amount * -1,0)),0) AS monthly_total
'                    FROM sales_transaction st
'                    LEFT JOIN customer_ledger cl ON cl.related_transaction_id = st.id
'                    WHERE YEAR(st.order_datetime) = YEAR(CURDATE())
'                    GROUP BY MONTH(st.order_datetime)
'                    ORDER BY sales_month;"
'                End If

'                Dim salesData As New Dictionary(Of Integer, Decimal)
'                For m As Integer = 1 To 12
'                    salesData(m) = 0
'                Next

'                Using cmd As New MySqlCommand(query, conn)
'                    Using reader As MySqlDataReader = cmd.ExecuteReader()
'                        While reader.Read()
'                            Dim monthNum As Integer = Convert.ToInt32(reader("sales_month"))
'                            Dim total As Decimal = Convert.ToDecimal(reader("monthly_total"))
'                            salesData(monthNum) = total
'                        End While
'                    End Using
'                End Using

'                ' Configure Chart
'                Chart1.Series.Clear()
'                Chart1.ChartAreas(0).AxisX.Interval = 1
'                Chart1.ChartAreas(0).AxisX.MajorGrid.LineWidth = 0
'                Chart1.ChartAreas(0).AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash
'                Chart1.ChartAreas(0).AxisX.LabelStyle.Angle = -45

'                Dim series As New Series("Monthly Sales") With {
'                .ChartType = SeriesChartType.Column,
'                .Color = Color.SteelBlue,
'                .IsValueShownAsLabel = True
'                }

'                Dim currentYear As Integer = DateTime.Now.Year
'                For m As Integer = 1 To 12
'                    Dim monthName As String = New DateTime(currentYear, m, 1).ToString("MMM")
'                    series.Points.AddXY(monthName, salesData(m))
'                Next

'                Chart1.Series.Add(series)

'                Chart1.Titles.Clear()
'                Dim title As New Title() With {
'                .Text = $"Monthly Sales for {currentYear}",
'                .Font = New Font("Segoe UI", 12, FontStyle.Bold),
'                .ForeColor = Color.DarkBlue
'                }
'                Chart1.Titles.Add(title)
'            End Using

'        Catch ex As Exception
'            MessageBox.Show("Error loading monthly chart: " & ex.Message,
'                        "Error",
'                        MessageBoxButtons.OK,
'                        MessageBoxIcon.Error)
'        End Try
'    End Sub


'    Private Sub LoadTopProductsChart()
'        Try
'            Using conn As New MySqlConnection(My.Settings.DBConnection)
'                conn.Open()

'                ' Current month and year
'                Dim currentMonthName As String = DateTime.Now.ToString("MMMM")
'                Dim currentYear As Integer = DateTime.Now.Year

'                ' Query top 10 products this month (This is already monthly filtered)
'                Dim query As String = "
'                SELECT p.product_name,
'                        IFNULL(SUM(ti.subtotal), 0) AS total_sales
'                FROM product p
'                LEFT JOIN transaction_item ti ON p.id = ti.product_id
'                LEFT JOIN sales_transaction st ON ti.transaction_id = st.id
'                    AND MONTH(st.order_datetime) = MONTH(CURDATE())
'                    AND YEAR(st.order_datetime) = YEAR(CURDATE())
'                GROUP BY p.id, p.product_name
'                ORDER BY total_sales DESC
'                LIMIT 10;
'            "

'                Dim productSales As New Dictionary(Of String, Decimal)()

'                Using cmd As New MySqlCommand(query, conn)
'                    Using reader As MySqlDataReader = cmd.ExecuteReader()
'                        While reader.Read()
'                            Dim name As String = reader("product_name").ToString()
'                            Dim total As Decimal = Convert.ToDecimal(reader("total_sales"))
'                            productSales(name) = total
'                        End While
'                    End Using
'                End Using

'                ' Fallback if no data
'                If productSales.Count = 0 Then
'                    For i As Integer = 1 To 5
'                        productSales("Product " & i) = 0
'                    Next
'                End If

'                ' Configure Chart2
'                Chart2.Series.Clear()
'                Chart2.ChartAreas(0).AxisX.Interval = 1
'                Chart2.ChartAreas(0).AxisX.MajorGrid.LineWidth = 0
'                Chart2.ChartAreas(0).AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash

'                Dim series As New Series("Top Products")
'                series.ChartType = SeriesChartType.Column
'                series.Color = Color.OrangeRed
'                series.IsValueShownAsLabel = True

'                For Each kvp In productSales
'                    series.Points.AddXY(kvp.Key, kvp.Value)
'                Next

'                Chart2.Series.Add(series)

'                ' ✅ Add dynamic title
'                Chart2.Titles.Clear()
'                Dim title As New Title()
'                title.Text = $"Top 10 Products of {currentMonthName} {currentYear}"
'                title.Font = New Font("Segoe UI", 12, FontStyle.Bold)
'                title.ForeColor = Color.DarkRed
'                Chart2.Titles.Add(title)

'            End Using
'        Catch ex As Exception
'            MessageBox.Show("Error loading top products chart: " & ex.Message,
'                            "Error",
'                            MessageBoxButtons.OK,
'                            MessageBoxIcon.Error)
'        End Try
'    End Sub

'    Private Sub LoadTotals()
'        Try
'            Using conn As New MySqlConnection(My.Settings.DBConnection)
'                conn.Open()

'                ' Total Customers
'                Using cmd As New MySqlCommand("SELECT COUNT(*) FROM customer;", conn)
'                    Dim totalCustomers As Integer = Convert.ToInt32(cmd.ExecuteScalar())
'                    txtTotalCustomer.Text = totalCustomers.ToString("N0") ' formats with commas
'                End Using

'                ' Total Products
'                Using cmd As New MySqlCommand("SELECT COUNT(*) FROM product;", conn)
'                    Dim totalProducts As Integer = Convert.ToInt32(cmd.ExecuteScalar())
'                    txtTotalProduct.Text = totalProducts.ToString("N0") ' formats with commas
'                End Using

'                ' Total Unpaid 
'                Using cmd As New MySqlCommand("SELECT IFNULL(SUM(amount),0) FROM customer_ledger;", conn)
'                    Dim totalBalance As Decimal = Convert.ToDecimal(cmd.ExecuteScalar())
'                    txtOverallTotalBalance.Text = totalBalance.ToString("N2") ' with commas + 2 decimals

'                    ' ✅ Set ForeColor based on balance
'                    If totalBalance > 0 Then
'                        txtOverallTotalBalance.ForeColor = Color.Red   ' means receivable
'                    Else
'                        txtOverallTotalBalance.ForeColor = Color.Green ' means settled / no receivable
'                    End If
'                End Using


'            End Using
'        Catch ex As Exception
'            MessageBox.Show("Error loading totals: " & ex.Message,
'                            "Error",
'                            MessageBoxButtons.OK,
'                            MessageBoxIcon.Error)
'        End Try
'    End Sub

'End Class


Imports MySql.Data.MySqlClient
Imports System.Drawing
Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmDashboard

    ' 1. Module-level variable to store the selected filter date
    Private selectedSalesDate As Date = Date.Today

    Private Sub frmDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize DatePicker
        dtpSalesDate.Value = Date.Today
        selectedSalesDate = Date.Today

        ' ✅ Populate ComboBox
        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("Revenue")
        ComboBox1.Items.Add("Payments Received")
        ComboBox1.SelectedIndex = 0 ' Default selection
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList

        LoadCashiers()
        LoadMonthlySalesChart()
        LoadTopProductsChart()
        LoadTotals()
        LoadMonthlyTotal() ' ✅ txtSales total is now loaded here (monthly)
    End Sub

    ' 3. New event handler for the DateTimePicker
    Private Sub dtpSalesDate_ValueChanged(sender As Object, e As EventArgs) Handles dtpSalesDate.ValueChanged
        ' Update the filter date and refresh the cashier list (Day Filter)
        selectedSalesDate = dtpSalesDate.Value.Date
        LoadCashiers()
        ' Note: txtSales is NOT updated here, as it is now monthly.
    End Sub

    Private Sub RefreshAll()
        LoadCashiers()
        LoadMonthlySalesChart()
        LoadTopProductsChart()
        LoadTotals()
        LoadMonthlyTotal()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        ' ComboBox1 affects:
        ' 1. The metric used in Cashiers (Daily) - Must be updated by btnRefresh or dtpSalesDate.
        ' 2. The metric used in Monthly Chart (Monthly) - Refresh NOW.
        ' 3. The metric used in Monthly Total (Monthly) - Refresh NOW.

        ' **MODIFICATION: Removed LoadCashiers() from this event.**
        ' LoadCashiers()

        LoadMonthlySalesChart() ' Refresh Chart (Monthly filter, Metric changed)
        LoadMonthlyTotal()      ' Refresh txtSales (Monthly filter, Metric changed)
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        RefreshAll()
    End Sub

    ' -----------------------------------------------------------
    ' NEW SUBROUTINE: Calculates the Monthly Total for txtSales (Filtered by ComboBox1)
    ' -----------------------------------------------------------
    Private Sub LoadMonthlyTotal()
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                Dim isRevenue As Boolean = If(ComboBox1.SelectedItem IsNot Nothing, ComboBox1.SelectedItem.ToString() = "Revenue", True)
                Dim query As String

                If isRevenue Then
                    ' Monthly Revenue = sum of total_amount for the current month/year
                    query = "
                    SELECT IFNULL(SUM(st.total_amount),0) AS overall_total
                    FROM sales_transaction st
                    WHERE MONTH(st.order_datetime) = MONTH(CURDATE())
                      AND YEAR(st.order_datetime) = YEAR(CURDATE());"
                Else
                    ' Monthly Payments Received = sum of amount_paid + settled balances for the current month/year
                    query = "
                    SELECT IFNULL(SUM(st.amount_paid + COALESCE(cl.amount * -1,0)),0) AS overall_total
                    FROM sales_transaction st
                    LEFT JOIN customer_ledger cl ON cl.related_transaction_id = st.id
                    WHERE MONTH(st.order_datetime) = MONTH(CURDATE())
                      AND YEAR(st.order_datetime) = YEAR(CURDATE());"
                End If

                Using cmd As New MySqlCommand(query, conn)
                    Dim monthlyTotal As Decimal = Convert.ToDecimal(cmd.ExecuteScalar())
                    ' Update txtSales with the MONTHLY total
                    txtSales.Text = monthlyTotal.ToString("N2")
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading monthly total: " & ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error)
        End Try
    End Sub
    ' -----------------------------------------------------------

    ' 4. MODIFIED LoadCashiers: **Removes the update of txtSales (Grand Total)**
    Private Sub LoadCashiers()
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                ' Cashiers list is filtered by selectedSalesDate (Day) and ComboBox1 (Metric)
                Dim isRevenue As Boolean = If(ComboBox1.SelectedItem IsNot Nothing, ComboBox1.SelectedItem.ToString() = "Revenue", True)

                Dim query As String

                If isRevenue Then
                    query = "
                    SELECT u.id, u.full_name,
                            IFNULL(SUM(st.total_amount),0) AS total_sales
                    FROM user u
                    INNER JOIN role r ON u.role_id = r.id
                    LEFT JOIN sales_transaction st ON u.id = st.user_id AND DATE(st.order_datetime) = @SalesDate
                    WHERE r.role_name = 'cashier'
                    GROUP BY u.id, u.full_name
                    ORDER BY u.id ASC;"
                Else
                    query = "
                    SELECT u.id, u.full_name,
                            IFNULL(SUM(st.amount_paid + COALESCE(cl.amount * -1,0)),0) AS total_sales
                    FROM user u
                    INNER JOIN role r ON u.role_id = r.id
                    LEFT JOIN sales_transaction st ON u.id = st.user_id AND DATE(st.order_datetime) = @SalesDate
                    LEFT JOIN customer_ledger cl ON cl.related_transaction_id = st.id
                    WHERE r.role_name = 'cashier'
                    GROUP BY u.id, u.full_name
                    ORDER BY u.id ASC;"
                End If

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@SalesDate", selectedSalesDate.Date)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        ' ... (code for clearing existing textboxes) ...
                        For i As Integer = Panel1.Controls.Count - 1 To 0 Step -1
                            Dim ctrl = Panel1.Controls(i)
                            If TypeOf ctrl Is TextBox AndAlso ctrl.Name.StartsWith("txtCashier") Then
                                Panel1.Controls.Remove(ctrl)
                            End If
                        Next

                        Dim yPos As Integer = 82
                        Dim index As Integer = 1

                        While reader.Read()
                            Dim fullName As String = reader("full_name").ToString()
                            Dim totalSales As Decimal = Convert.ToDecimal(reader("total_sales"))

                            ' Cashier Name
                            Dim txtName As New TextBox() With {
                            .Name = "txtCashier" & index,
                            .Text = fullName,
                            .ReadOnly = True,
                            .Width = 110,
                            .BorderStyle = BorderStyle.None,
                            .BackColor = Color.White,
                            .Font = New Font("Segoe UI", 10),
                            .Location = New Point(30, yPos)
                            }

                            ' Sales Total
                            Dim txtSalesBox As New TextBox() With {
                            .Name = "txtCashierSales" & index,
                            .Text = totalSales.ToString("N2"),
                            .ReadOnly = True,
                            .Width = 70,
                            .TextAlign = HorizontalAlignment.Right,
                            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
                            .BorderStyle = BorderStyle.None,
                            .BackColor = Color.White,
                            .Location = New Point(150, yPos)
                            }

                            Panel1.Controls.Add(txtName)
                            Panel1.Controls.Add(txtSalesBox)

                            yPos += 30
                            index += 1
                        End While

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

                ' This function already uses the monthly filter, correctly
                Dim isRevenue As Boolean = If(ComboBox1.SelectedItem IsNot Nothing, ComboBox1.SelectedItem.ToString() = "Revenue", True)

                Dim query As String

                If isRevenue Then
                    ' Revenue = sum of total_amount
                    query = "
                    SELECT MONTH(st.order_datetime) AS sales_month,
                            IFNULL(SUM(st.total_amount),0) AS monthly_total
                    FROM sales_transaction st
                    WHERE YEAR(st.order_datetime) = YEAR(CURDATE())
                    GROUP BY MONTH(st.order_datetime)
                    ORDER BY sales_month;"
                Else
                    ' Payments Received = sum of amount_paid + settled balances
                    query = "
                    SELECT MONTH(st.order_datetime) AS sales_month,
                            IFNULL(SUM(st.amount_paid + COALESCE(cl.amount * -1,0)),0) AS monthly_total
                    FROM sales_transaction st
                    LEFT JOIN customer_ledger cl ON cl.related_transaction_id = st.id
                    WHERE YEAR(st.order_datetime) = YEAR(CURDATE())
                    GROUP BY MONTH(st.order_datetime)
                    ORDER BY sales_month;"
                End If

                Dim salesData As New Dictionary(Of Integer, Decimal)
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

                ' Configure Chart
                Chart1.Series.Clear()
                Chart1.ChartAreas(0).AxisX.Interval = 1
                Chart1.ChartAreas(0).AxisX.MajorGrid.LineWidth = 0
                Chart1.ChartAreas(0).AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash
                Chart1.ChartAreas(0).AxisX.LabelStyle.Angle = -45

                Dim series As New Series("Monthly Sales") With {
                .ChartType = SeriesChartType.Column,
                .Color = Color.SteelBlue,
                .IsValueShownAsLabel = True
                }

                Dim currentYear As Integer = DateTime.Now.Year
                For m As Integer = 1 To 12
                    Dim monthName As String = New DateTime(currentYear, m, 1).ToString("MMM")
                    series.Points.AddXY(monthName, salesData(m))
                Next

                Chart1.Series.Add(series)

                Chart1.Titles.Clear()
                Dim title As New Title() With {
                .Text = $"Monthly Sales for {currentYear}",
                .Font = New Font("Segoe UI", 12, FontStyle.Bold),
                .ForeColor = Color.DarkBlue
                }
                Chart1.Titles.Add(title)
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading monthly chart: " & ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub LoadTopProductsChart()
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                ' Current month and year
                Dim currentMonthName As String = DateTime.Now.ToString("MMMM")
                Dim currentYear As Integer = DateTime.Now.Year

                ' Query top 10 products this month (This is already monthly filtered)
                Dim query As String = "
                SELECT p.product_name,
                        IFNULL(SUM(ti.subtotal), 0) AS total_sales
                FROM product p
                LEFT JOIN transaction_item ti ON p.id = ti.product_id
                LEFT JOIN sales_transaction st ON ti.transaction_id = st.id
                    AND MONTH(st.order_datetime) = MONTH(CURDATE())
                    AND YEAR(st.order_datetime) = YEAR(CURDATE())
                GROUP BY p.id, p.product_name
                ORDER BY total_sales DESC
                LIMIT 10;
            "

                Dim productSales As New Dictionary(Of String, Decimal)()

                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim name As String = reader("product_name").ToString()
                            Dim total As Decimal = Convert.ToDecimal(reader("total_sales"))
                            productSales(name) = total
                        End While
                    End Using
                End Using

                ' Fallback if no data
                If productSales.Count = 0 Then
                    For i As Integer = 1 To 5
                        productSales("Product " & i) = 0
                    Next
                End If

                ' Configure Chart2
                Chart2.Series.Clear()
                Chart2.ChartAreas(0).AxisX.Interval = 1
                Chart2.ChartAreas(0).AxisX.MajorGrid.LineWidth = 0
                Chart2.ChartAreas(0).AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash

                Dim series As New Series("Top Products")
                series.ChartType = SeriesChartType.Column
                series.Color = Color.OrangeRed
                series.IsValueShownAsLabel = True

                For Each kvp In productSales
                    series.Points.AddXY(kvp.Key, kvp.Value)
                Next

                Chart2.Series.Add(series)

                ' ✅ Add dynamic title
                Chart2.Titles.Clear()
                Dim title As New Title()
                title.Text = $"Top 10 Products of {currentMonthName} {currentYear}"
                title.Font = New Font("Segoe UI", 12, FontStyle.Bold)
                title.ForeColor = Color.DarkRed
                Chart2.Titles.Add(title)

            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading top products chart: " & ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadTotals()
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                ' Total Customers
                Using cmd As New MySqlCommand("SELECT COUNT(*) FROM customer;", conn)
                    Dim totalCustomers As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    txtTotalCustomer.Text = totalCustomers.ToString("N0") ' formats with commas
                End Using

                ' Total Products
                Using cmd As New MySqlCommand("SELECT COUNT(*) FROM product;", conn)
                    Dim totalProducts As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    txtTotalProduct.Text = totalProducts.ToString("N0") ' formats with commas
                End Using

                ' Total Unpaid 
                Using cmd As New MySqlCommand("SELECT IFNULL(SUM(amount),0) FROM customer_ledger;", conn)
                    Dim totalBalance As Decimal = Convert.ToDecimal(cmd.ExecuteScalar())
                    txtOverallTotalBalance.Text = totalBalance.ToString("N2") ' with commas + 2 decimals

                    ' ✅ Set ForeColor based on balance
                    If totalBalance > 0 Then
                        txtOverallTotalBalance.ForeColor = Color.Red   ' means receivable
                    Else
                        txtOverallTotalBalance.ForeColor = Color.Green ' means settled / no receivable
                    End If
                End Using


            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading totals: " & ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

End Class