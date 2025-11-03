Imports MySql.Data.MySqlClient
Imports System.Text

Public Class frmTransaction

    Private Sub frmTransaction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupTransactionGrid()
        ' Reset controls on load
        txtSearchTransaction.Clear()
        ' Attempt to disable date filter on load (requires ShowCheckBox = True)
        Try
            dtpDateFilter.Checked = False
        Catch
            ' Ignore if .Checked property is not available
        End Try
        LoadTransactions()
    End Sub

    Private Sub btnRefreshTransaction_Click(sender As Object, e As EventArgs) Handles btnRefreshTransaction.Click
        ' 1. Clear the search box
        txtSearchTransaction.Clear()

        ' 2. Reset the date filter (uncheck the box)
        Try
            dtpDateFilter.Checked = False
        Catch
            ' If Checkbox not available, setting value to now is the usual non-filter state
            dtpDateFilter.Value = DateTime.Now
        End Try

        ' 3. Reload all transactions (no filters applied)
        LoadTransactions()
    End Sub

    ' The Search button now just calls LoadTransactions, which reads all filters
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        LoadTransactions()
    End Sub

    ' NEW: Automatically reload when the date filter control is interacted with
    Private Sub dtpDateFilter_ValueChanged(sender As Object, e As EventArgs) Handles dtpDateFilter.ValueChanged
        LoadTransactions()
    End Sub

    Private Sub SetupTransactionGrid()
        With dgvTransaction
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .RowHeadersVisible = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
        End With
    End Sub

    ' Modified to read filtering values directly from controls
    Private Sub LoadTransactions()

        ' Read the current filter values from the controls
        Dim searchTerm As String = txtSearchTransaction.Text.Trim()

        ' Check if date filtering should be active
        Dim useDateFilter As Boolean = False
        Dim selectedDate As Date = dtpDateFilter.Value

        Try
            ' Check if the DTP's CheckBox is checked (standard WinForms optional filter)
            useDateFilter = dtpDateFilter.Checked
        Catch
            ' If there's no checkbox, we assume the DTP value is always used for simplicity
            ' unless the refresh button is used.
        End Try

        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                Dim queryBuilder As New StringBuilder("
                    SELECT 
                        st.order_number AS 'Order Number',
                        u.full_name AS 'User',
                        c.customer_name AS 'Customer',
                        st.total_amount AS 'Total Amount',
                        st.cash_received AS 'Cash Received',
                        st.amount_paid AS 'Amount Paid',
                        st.change_given AS 'Change',
                        IF(st.total_amount > st.amount_paid, 
                            st.total_amount - st.amount_paid, 
                            NULL) AS 'Balance', 
                        (SELECT IFNULL(SUM(pd.amount), 0)
                            FROM payment_detail pd
                            INNER JOIN payment_method pm ON pd.payment_method_id = pm.id
                            WHERE pd.transaction_id = st.id
                                AND pm.payment_method_name = 'Cash') AS 'Cash Payment',
                        (SELECT IFNULL(SUM(pd.amount), 0)
                            FROM payment_detail pd
                            INNER JOIN payment_method pm ON pd.payment_method_id = pm.id
                            WHERE pd.transaction_id = st.id
                                AND pm.payment_method_name <> 'Cash') AS 'Online Payment',
                        (SELECT GROUP_CONCAT(pd.ref_num SEPARATOR ', ')
                            FROM payment_detail pd
                            INNER JOIN payment_method pm ON pd.payment_method_id = pm.id
                            WHERE pd.transaction_id = st.id
                                AND pm.payment_method_name <> 'Cash'
                                AND pd.ref_num IS NOT NULL
                                AND pd.ref_num <> ''
                        ) AS 'Ref No',
                        ps.payment_status_name AS 'Payment Status',
                        DATE_FORMAT(st.order_datetime, '%Y-%m-%d %H:%i') AS 'Order Datetime'
                    FROM sales_transaction st
                    INNER JOIN user u ON st.user_id = u.id
                    INNER JOIN customer c ON st.customer_id = c.id
                    INNER JOIN payment_status ps ON st.payment_status_id = ps.id
                ")

                Dim whereClauses As New List(Of String)
                Dim cmd As New MySqlCommand()
                cmd.Connection = conn

                ' 1. DATE FILTER LOGIC
                If useDateFilter Then
                    ' We use DATE() to only compare the day, month, and year
                    whereClauses.Add(" DATE(st.order_datetime) = @FilterDate ")
                    cmd.Parameters.AddWithValue("@FilterDate", selectedDate.Date)
                End If

                ' 2. SEARCH TERM LOGIC
                If Not String.IsNullOrWhiteSpace(searchTerm) Then
                    whereClauses.Add(" (st.order_number LIKE @SearchTerm OR c.customer_name LIKE @SearchTerm OR u.full_name LIKE @SearchTerm OR ps.payment_status_name LIKE @SearchTerm) ")
                    cmd.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%")
                End If

                ' Apply WHERE clause, combining any active filters with AND
                If whereClauses.Count > 0 Then
                    queryBuilder.Append(" WHERE ")
                    queryBuilder.Append(String.Join(" AND ", whereClauses))
                End If

                ' Add ordering
                queryBuilder.Append(" ORDER BY st.order_datetime DESC;")

                cmd.CommandText = queryBuilder.ToString()


                Dim adapter As New MySqlDataAdapter(cmd)
                Dim dt As New DataTable()
                adapter.Fill(dt)

                dgvTransaction.DataSource = dt

                ' Set FillWeights
                dgvTransaction.Columns("Payment Status").FillWeight = 55
                dgvTransaction.Columns("Change").FillWeight = 55
                dgvTransaction.Columns("Total Amount").FillWeight = 65
                dgvTransaction.Columns("Cash Received").FillWeight = 65
                dgvTransaction.Columns("Amount Paid").FillWeight = 65
                dgvTransaction.Columns("Cash Payment").FillWeight = 65
                dgvTransaction.Columns("Online Payment").FillWeight = 65
                dgvTransaction.Columns("Ref No").FillWeight = 80
                dgvTransaction.Columns("User").FillWeight = 80
                dgvTransaction.Columns("Customer").FillWeight = 80
                dgvTransaction.Columns("Balance").FillWeight = 55

                ' Format numeric columns
                If dgvTransaction.Columns.Contains("Total Amount") Then
                    dgvTransaction.Columns("Total Amount").DefaultCellStyle.Format = "N2"
                End If
                If dgvTransaction.Columns.Contains("Cash Received") Then
                    dgvTransaction.Columns("Cash Received").DefaultCellStyle.Format = "N2"
                End If
                If dgvTransaction.Columns.Contains("Amount Paid") Then
                    dgvTransaction.Columns("Amount Paid").DefaultCellStyle.Format = "N2"
                End If
                If dgvTransaction.Columns.Contains("Change") Then
                    dgvTransaction.Columns("Change").DefaultCellStyle.Format = "N2"
                End If
                If dgvTransaction.Columns.Contains("Cash Payment") Then
                    dgvTransaction.Columns("Cash Payment").DefaultCellStyle.Format = "N2"
                End If
                If dgvTransaction.Columns.Contains("Online Payment") Then
                    dgvTransaction.Columns("Online Payment").DefaultCellStyle.Format = "N2"
                End If
                If dgvTransaction.Columns.Contains("Balance") Then
                    dgvTransaction.Columns("Balance").DefaultCellStyle.Format = "N2"
                End If

            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading transactions: " & ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

End Class
