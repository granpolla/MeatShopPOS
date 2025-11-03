Imports MySql.Data.MySqlClient

Public Class frmTransaction

    Private Sub frmTransaction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupTransactionGrid()
        ' Load transactions with an empty search term initially
        LoadTransactions()
    End Sub

    Private Sub btnRefreshTransaction_Click(sender As Object, e As EventArgs) Handles btnRefreshTransaction.Click
        ' 1. Clear the search box
        txtSearchTransaction.Clear()
        ' 2. Reload all transactions
        LoadTransactions()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        ' Load transactions using the text entered in the search box
        LoadTransactions(txtSearchTransaction.Text.Trim())
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

    ' Modified to accept an optional search term
    Private Sub LoadTransactions(Optional searchTerm As String = "")
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
                        -- This logic is correct: returns NULL if balance is zero or negative
                        IF(st.total_amount > st.amount_paid, 
                            st.total_amount - st.amount_paid, 
                            NULL) AS 'Balance', 
                        -- (Rest of subqueries and joins remain the same)
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
                "

                ' Add WHERE clause for searching if a search term is provided
                If Not String.IsNullOrWhiteSpace(searchTerm) Then
                    query &= "
                        WHERE st.order_number LIKE @SearchTerm 
                        OR c.customer_name LIKE @SearchTerm 
                        OR u.full_name LIKE @SearchTerm 
                        OR ps.payment_status_name LIKE @SearchTerm -- <-- FIX: Added Payment Status search
                    "
                End If

                query &= " ORDER BY st.order_datetime DESC;"


                Dim cmd As New MySqlCommand(query, conn)

                ' Add parameter if searching
                If Not String.IsNullOrWhiteSpace(searchTerm) Then
                    ' Use LIKE operator to search anywhere in the fields
                    cmd.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%")
                End If

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
