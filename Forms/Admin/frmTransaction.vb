Imports MySql.Data.MySqlClient

Public Class frmTransaction

    Private Sub frmTransaction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupTransactionGrid()
        LoadTransactions()
    End Sub

    Private Sub btnRefreshTransaction_Click(sender As Object, e As EventArgs) Handles btnRefreshTransaction.Click
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

    Private Sub LoadTransactions()
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()
                Dim query As String = "
                    SELECT 
                        st.order_number AS 'Order Number',
                        u.full_name AS 'User',
                        c.customer_name AS 'Customer',
                        st.total_amount AS 'Total Amount',
                        st.cash_received AS 'Cash Received',
                        st.amount_paid AS 'Amount Paid',
                        st.change_given AS 'Change',
                        -- ✅ BALANCE COLUMN LOGIC: Display balance only if > 0
                        IF(st.total_amount > st.amount_paid, 
                           st.total_amount - st.amount_paid, 
                           NULL) AS 'Balance',
                        -- ✅ Cash Payment
                        (SELECT IFNULL(SUM(pd.amount), 0)
                         FROM payment_detail pd
                         INNER JOIN payment_method pm ON pd.payment_method_id = pm.id
                         WHERE pd.transaction_id = st.id
                           AND pm.payment_method_name = 'Cash') AS 'Cash Payment',
                        -- ✅ Online Payment
                        (SELECT IFNULL(SUM(pd.amount), 0)
                         FROM payment_detail pd
                         INNER JOIN payment_method pm ON pd.payment_method_id = pm.id
                         WHERE pd.transaction_id = st.id
                           AND pm.payment_method_name <> 'Cash') AS 'Online Payment',
                        -- ✅ Ref No (only if online exists)
                        (SELECT GROUP_CONCAT(pd.ref_num SEPARATOR ', ')
                         FROM payment_detail pd
                         INNER JOIN payment_method pm ON pd.payment_method_id = pm.id
                         WHERE pd.transaction_id = st.id
                           AND pm.payment_method_name <> 'Cash'
                           AND pd.ref_num IS NOT NULL
                           AND pd.ref_num <> ''
                        ) AS 'Ref No',
                        ps.payment_status_name AS 'Payment Status',
                        -- ❌ REMOVED: st.invoice_pdf AS 'Invoice PDF',
                        DATE_FORMAT(st.order_datetime, '%Y-%m-%d %H:%i') AS 'Order Datetime'
                    FROM sales_transaction st
                    INNER JOIN user u ON st.user_id = u.id
                    INNER JOIN customer c ON st.customer_id = c.id
                    INNER JOIN payment_status ps ON st.payment_status_id = ps.id
                    ORDER BY st.order_datetime DESC;
                    "

                Dim adapter As New MySqlDataAdapter(query, conn)
                Dim dt As New DataTable()
                adapter.Fill(dt)

                dgvTransaction.DataSource = dt

                ' Set FillWeights (No change needed here, as Invoice PDF didn't have a specific FillWeight)
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