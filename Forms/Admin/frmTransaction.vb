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
                        st.change_given AS 'Change',
                        -- build Payment Summary (grouped methods per transaction)
                        (SELECT GROUP_CONCAT(CONCAT(pm.payment_method_name, ': ', 
                                                  FORMAT(pd.amount, 2)) SEPARATOR ', ')
                         FROM payment_detail pd
                         INNER JOIN payment_method pm ON pd.payment_method_id = pm.id
                         WHERE pd.transaction_id = st.id
                        ) AS 'Payment Summary',
                        ps.payment_status_name AS 'Payment Status',
                        st.invoice_pdf AS 'Invoice PDF',
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

                ' ✅ Format numeric columns
                If dgvTransaction.Columns.Contains("Total Amount") Then
                    dgvTransaction.Columns("Total Amount").DefaultCellStyle.Format = "N2"
                End If
                If dgvTransaction.Columns.Contains("Cash Received") Then
                    dgvTransaction.Columns("Cash Received").DefaultCellStyle.Format = "N2"
                End If
                If dgvTransaction.Columns.Contains("Change") Then
                    dgvTransaction.Columns("Change").DefaultCellStyle.Format = "N2"
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
