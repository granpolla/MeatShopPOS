Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions

Public Class frmCashierDashboard

    Private Sub frmCashierDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCustomers()
        LoadProducts()
        LoadInputOrderItemForm()
        LoadPaymentInput()

        InitializeOrderItemPreview()
        InitializeCustomerBalancePreview()
    End Sub

    Private Sub frmCashierDashboard_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        ' Ensure no active rows are selected on form load
        dgvCustomer.ClearSelection()
        dgvCustomer.CurrentCell = Nothing

        dgvProductsPreview.ClearSelection()
        dgvProductsPreview.CurrentCell = Nothing
    End Sub

    ' ✅ Normalize name (remove extra spaces + proper case)
    Public Function NormalizeName(input As String) As String
        Dim cleaned As String = Regex.Replace(input.Trim(), "\s+", " ")
        Return StrConv(cleaned, VbStrConv.ProperCase)
    End Function

    ' ✅ Add order item row to dgvOrderItemPreview
    Public Sub AddOrderItem(productID As Integer, productName As String, brand As String,
                        unitWeight As Decimal, unitPrice As Decimal,
                        totalBox As Decimal, totalWeight As Decimal, total As Decimal)

        Dim orderTable As DataTable = CType(dgvOrderItemPreview.DataSource, DataTable)

        ' Include ProductID in the row
        orderTable.Rows.Add(productID, productName, brand, unitWeight, unitPrice, totalBox, totalWeight, total)

        UpdateGrandTotal()
    End Sub

    Private Sub UpdateGrandTotal()
        UpdateTotals()
    End Sub

    Private Sub UpdateTotals()
        Dim orderTotal As Decimal = 0
        Dim balanceTotal As Decimal = 0

        ' 🔹 Sum from OrderItemPreview
        If dgvOrderItemPreview.DataSource IsNot Nothing Then
            Dim orderTable As DataTable = CType(dgvOrderItemPreview.DataSource, DataTable)
            For Each row As DataRow In orderTable.Rows
                orderTotal += Convert.ToDecimal(row("Total"))
            Next
        End If

        ' 🔹 Sum from selected rows in BalancePreview
        For Each row As DataGridViewRow In dgvCustomerBalancePreview.SelectedRows
            balanceTotal += Convert.ToDecimal(row.Cells("Balance").Value)
        Next

        ' Show totals
        txtCustomerBalanceTotal.Text = balanceTotal.ToString("N2")
        txtGrandTotal.Text = (orderTotal + balanceTotal).ToString("N2")
    End Sub

    ' ✅ Whenever user changes balance selection → recalc totals
    Private Sub dgvCustomerBalancePreview_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCustomerBalancePreview.SelectionChanged
        UpdateTotals()
    End Sub

    ' 🔹 Initialize dgvOrderItemPreview with headers only
    Private Sub InitializeOrderItemPreview()
        Dim dt As New DataTable()
        dt.Columns.Add("ProductID", GetType(Integer)) ' Add hidden ProductID column for database reference
        dt.Columns.Add("Product Name", GetType(String))
        dt.Columns.Add("Brand", GetType(String))
        dt.Columns.Add("Unit Weight", GetType(Decimal))
        dt.Columns.Add("Unit Price", GetType(Decimal))
        dt.Columns.Add("Total Box", GetType(Decimal))
        dt.Columns.Add("Total Weight", GetType(Decimal))
        dt.Columns.Add("Total", GetType(Decimal))

        dgvOrderItemPreview.DataSource = dt

        ' Apply formatting once
        With dgvOrderItemPreview
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
            .RowHeadersVisible = False

            .Columns("ProductID").Visible = False

            .Columns("Unit Price").DefaultCellStyle.Format = "N2"
            .Columns("Unit Weight").DefaultCellStyle.Format = "N2"
            .Columns("Total Box").DefaultCellStyle.Format = "N0"
            .Columns("Total Weight").DefaultCellStyle.Format = "N2"
            .Columns("Total").DefaultCellStyle.Format = "N2"

            .Columns("Unit Price").FillWeight = 70
            .Columns("Unit Weight").FillWeight = 70
            .Columns("Total Box").FillWeight = 70
            .Columns("Total Weight").FillWeight = 70
            .Columns("Total").FillWeight = 90
            .Columns("Product Name").FillWeight = 200
            .Columns("Brand").FillWeight = 120
        End With
    End Sub

    ' 🔹 Initialize dgvCustomerBalancePreview with headers only
    Private Sub InitializeCustomerBalancePreview()
        Dim dt As New DataTable()
        dt.Columns.Add("Date", GetType(Date))
        dt.Columns.Add("Description", GetType(String))
        dt.Columns.Add("Balance", GetType(Decimal))

        dgvCustomerBalancePreview.DataSource = dt

        With dgvCustomerBalancePreview
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = True
            .ReadOnly = True
            .RowHeadersVisible = False
            .Columns("Balance").DefaultCellStyle.Format = "N2"

            .Columns("Date").FillWeight = 35
            .Columns("Balance").FillWeight = 20

        End With
    End Sub








    ' ✅ Load all customers
    Public Sub LoadCustomers()
        Dim query As String = "
        SELECT id AS 'CustomerID',
               customer_name AS 'Full Name',
               address AS 'Address'
        FROM customer
        ORDER BY customer_name ASC"
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                Using cmd As New MySqlCommand(query, conn)
                    Dim adapter As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    adapter.Fill(dt)
                    dgvCustomer.DataSource = dt
                    FormatCustomerGrid()

                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading customers: " & ex.Message)
        End Try
    End Sub

    ' ✅ Load customer balance preview
    Private Sub LoadCustomerBalance(customerID As Integer)
        Dim query As String = "
            SELECT cl.transaction_id AS 'TransactionID',
                   cl.entry_date AS 'Date',
                   cl.description AS 'Description',
                   cl.amount AS 'Balance'
            FROM customer_ledger cl
            WHERE cl.customer_id = @custID
              AND cl.amount > 0
              AND cl.related_transaction_id IS NULL  -- 🔹 exclude settlement rows
              AND NOT EXISTS (                       -- 🔹 exclude if already settled
                  SELECT 1
                  FROM customer_ledger cl2
                  WHERE cl2.related_transaction_id = cl.transaction_id
              )
            ORDER BY cl.entry_date DESC"

        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@custID", customerID)

                    Dim adapter As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    adapter.Fill(dt)

                    If dt.Rows.Count = 0 Then
                        Dim emptyTable As DataTable = CType(dgvCustomerBalancePreview.DataSource, DataTable)
                        emptyTable.Rows.Clear()
                    Else
                        dgvCustomerBalancePreview.DataSource = dt
                    End If

                    ' ✅ Hide TransactionID column
                    If dgvCustomerBalancePreview.Columns.Contains("TransactionID") Then
                        dgvCustomerBalancePreview.Columns("TransactionID").Visible = False
                    End If

                    dgvCustomerBalancePreview.ClearSelection()
                    dgvCustomerBalancePreview.CurrentCell = Nothing
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading balance: " & ex.Message)
        End Try
    End Sub



    ' ✅ Load all products into dgvProductsPreview
    Public Sub LoadProducts()
        Dim query As String = "
        SELECT id AS 'ProductID',
               product_name AS 'Product Name',
               brand AS 'Brand',
               unit_weight_kg AS 'Unit Weight',
               unit_price_php AS 'Unit Price'
        FROM product
        ORDER BY product_name ASC"
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                Using cmd As New MySqlCommand(query, conn)
                    Dim adapter As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    adapter.Fill(dt)
                    dgvProductsPreview.DataSource = dt
                    FormatProductsGrid()

                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading products: " & ex.Message)
        End Try
    End Sub

    ' ✅ Filter products (prefix search)
    Public Sub LoadProductsPreview(searchText As String)
        Dim query As String = "
        SELECT id AS 'ProductID',
               product_name AS 'Product Name',
               brand AS 'Brand',
               unit_weight_kg AS 'Unit Weight',
               unit_price_php AS 'Unit Price'
        FROM product
        WHERE product_name LIKE @searchText
        ORDER BY product_name ASC"

        Using conn As New MySqlConnection(My.Settings.DBConnection)
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@searchText", searchText & "%")
                Dim adapter As New MySqlDataAdapter(cmd)
                Dim dt As New DataTable()
                adapter.Fill(dt)
                dgvProductsPreview.DataSource = dt
                FormatProductsGrid() ' ✅ keep formatting consistent
            End Using
        End Using
    End Sub

    Private Sub LoadInputOrderItemForm()
        pnlInputOrderItem.Controls.Clear()

        Dim inputForm As New frmCashierInputOrderItem()
        inputForm.TopLevel = False
        inputForm.FormBorderStyle = FormBorderStyle.None
        inputForm.Dock = DockStyle.Fill

        pnlInputOrderItem.Controls.Add(inputForm)
        inputForm.Show()
    End Sub

    Private Sub LoadPaymentInput()
        pnlCashierPaymentInput.Controls.Clear()

        Dim inputForm As New frmCashierPaymentInput()
        inputForm.TopLevel = False
        inputForm.FormBorderStyle = FormBorderStyle.None
        inputForm.Dock = DockStyle.Fill

        pnlCashierPaymentInput.Controls.Add(inputForm)
        inputForm.Show()
    End Sub






    ' ✅ Format dgvCustomer
    Private Sub FormatCustomerGrid()
        With dgvCustomer
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
            .RowHeadersVisible = False

            If .Columns.Contains("CustomerID") Then
                .Columns("CustomerID").Visible = False
            End If
        End With
    End Sub

    ' ✅ Format dgvProductsPreview
    Private Sub FormatProductsGrid()
        With dgvProductsPreview
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
            .RowHeadersVisible = False

            ' Hide ProductID
            If .Columns.Contains("ProductID") Then
                .Columns("ProductID").Visible = False
            End If

            ' Format numeric columns and make them thinner
            If .Columns.Contains("Unit Price") Then
                .Columns("Unit Price").DefaultCellStyle.Format = "N2"
                .Columns("Unit Price").FillWeight = 70   ' thinner
            End If

            If .Columns.Contains("Unit Weight") Then
                .Columns("Unit Weight").DefaultCellStyle.Format = "N2"
                .Columns("Unit Weight").FillWeight = 70  ' thinner
            End If

            ' Example: make product name wider
            If .Columns.Contains("Product Name") Then
                .Columns("Product Name").FillWeight = 130
            End If
        End With
    End Sub






    ' ✅ Search Button
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim firstName As String = NormalizeName(txtFirstName.Text)
        Dim lastName As String = NormalizeName(txtLastName.Text)

        ' Check if both are filled
        If String.IsNullOrWhiteSpace(firstName) OrElse String.IsNullOrWhiteSpace(lastName) Then
            MessageBox.Show("Please input both First Name and Last Name to search.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Combine into full name
        Dim fullName As String = firstName & " " & lastName

        Dim query As String = "
    SELECT id AS 'CustomerID',
           customer_name AS 'Full Name',
           address AS 'Address'
    FROM customer
    WHERE LOWER(REPLACE(customer_name, ' ', '')) = LOWER(REPLACE(@name, ' ', ''))"

        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@name", fullName.Replace(" ", ""))

                    Dim adapter As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    adapter.Fill(dt)

                    If dt.Rows.Count > 0 Then
                        dgvCustomer.DataSource = dt
                        FormatCustomerGrid()
                    Else
                        Dim result As DialogResult = MessageBox.Show(
                        $"Customer ""{fullName}"" is not saved in our database. Do you want to insert this customer?",
                        "Customer Not Found",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    )

                        If result = DialogResult.Yes Then
                            Dim addForm As New frmCashierAddCustomer()
                            addForm.txtCustomerName.Text = fullName
                            addForm.txtAddress.Clear()

                            If addForm.ShowDialog() = DialogResult.OK Then
                                ' 🔹 Query the newly added customer back
                                Dim dtNew As New DataTable()
                                Using cmdNew As New MySqlCommand(query, conn)
                                    cmdNew.Parameters.AddWithValue("@name", fullName.Replace(" ", ""))
                                    Dim adapterNew As New MySqlDataAdapter(cmdNew)
                                    dtNew.Clear()
                                    adapterNew.Fill(dtNew)
                                End Using

                                If dtNew.Rows.Count > 0 Then
                                    dgvCustomer.DataSource = dtNew
                                    FormatCustomerGrid()

                                    ' 🔹 Auto-fill textboxes (like dgv click)
                                    Dim row As DataRow = dtNew.Rows(0)
                                    Dim nameParts() As String = row("Full Name").ToString().Split(New Char() {" "c}, 2, StringSplitOptions.RemoveEmptyEntries)

                                    If nameParts.Length > 0 Then txtFirstName.Text = nameParts(0)
                                    If nameParts.Length > 1 Then txtLastName.Text = nameParts(1)

                                    txtCustomerAddress.Text = row("Address").ToString()

                                    ' Load balance preview as well
                                    Dim customerID As Integer = Convert.ToInt32(row("CustomerID"))
                                    LoadCustomerBalance(customerID)
                                End If
                            End If
                        End If
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error searching customer: " & ex.Message)
        End Try
    End Sub


    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        ' Load customers from the database.
        LoadCustomers()
        LoadProducts()

        ' Clear customer info text boxes.
        txtFirstName.Clear()
        txtLastName.Clear()
        txtCustomerAddress.Clear()

        ' 💥 Correctly clear data-bound DataGridViews
        ' Clear the rows from the underlying DataTable.
        If dgvCustomerBalancePreview.DataSource IsNot Nothing Then
            CType(dgvCustomerBalancePreview.DataSource, DataTable).Rows.Clear()
        End If
        If dgvOrderItemPreview.DataSource IsNot Nothing Then
            CType(dgvOrderItemPreview.DataSource, DataTable).Rows.Clear()
        End If



        ' Refresh grand total to reflect the empty order/balance tables.
        UpdateGrandTotal()

        ' Reset readonly status to allow new customer input.
        txtFirstName.ReadOnly = False
        txtLastName.ReadOnly = False
        txtCustomerAddress.ReadOnly = False
    End Sub

    Private Sub btnRemoveItem_Click(sender As Object, e As EventArgs) Handles btnRemoveItem.Click
        ' Validate selection
        If dgvOrderItemPreview.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select an item to remove.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Confirm removal
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to remove the selected item?",
                                                 "Remove Item",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Dim selectedRow As DataGridViewRow = dgvOrderItemPreview.SelectedRows(0)

            ' Remove from DataTable (since DataGridView is bound to it)
            Dim orderTable As DataTable = CType(dgvOrderItemPreview.DataSource, DataTable)
            orderTable.Rows.RemoveAt(selectedRow.Index)

            ' ✅ Update Grand Total
            UpdateGrandTotal()
        End If
    End Sub

    ' ✅ Clear selected balances + reset totals
    Private Sub btnClearBalanceTxt_Click(sender As Object, e As EventArgs) Handles btnClearBalanceTxt.Click
        ' Clear selection
        dgvCustomerBalancePreview.ClearSelection()
        dgvCustomerBalancePreview.CurrentCell = Nothing

        ' Reset text
        txtCustomerBalanceTotal.Clear()

        ' Recalculate grand total (only order items remain)
        UpdateGrandTotal()
    End Sub







    ' ✅ When selecting customer → autofill + load balances
    Private Sub dgvCustomer_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCustomer.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvCustomer.Rows(e.RowIndex)

            Dim fullName As String = row.Cells("Full Name").Value.ToString()
            Dim nameParts() As String = fullName.Split(New Char() {" "c}, 2, StringSplitOptions.RemoveEmptyEntries)

            If nameParts.Length > 0 Then
                txtFirstName.Text = nameParts(0)
            End If
            If nameParts.Length > 1 Then
                txtLastName.Text = nameParts(1)
            Else
                txtLastName.Clear()
            End If

            txtCustomerAddress.Text = row.Cells("Address").Value.ToString()

            ' ✅ Make textboxes readonly after selecting customer
            txtFirstName.ReadOnly = True
            txtLastName.ReadOnly = True
            txtCustomerAddress.ReadOnly = True

            ' Get the ID for balance lookup
            Dim customerID As Integer = Convert.ToInt32(row.Cells("CustomerID").Value)
            LoadCustomerBalance(customerID)
        End If
    End Sub

    Private Sub dgvProductsPreview_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProductsPreview.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvProductsPreview.Rows(e.RowIndex)

            ' Get product details
            Dim productID As Integer = Convert.ToInt32(row.Cells("ProductID").Value)
            Dim productName As String = row.Cells("Product Name").Value.ToString()
            Dim productBrand As String = row.Cells("Brand").Value.ToString()
            Dim unitWeight As Decimal = Convert.ToDecimal(row.Cells("Unit Weight").Value)
            Dim unitPrice As Decimal = Convert.ToDecimal(row.Cells("Unit Price").Value)

            ' Find frmCashierInputOrderItem inside pnlInputOrderItem
            For Each ctrl As Control In pnlInputOrderItem.Controls
                If TypeOf ctrl Is frmCashierInputOrderItem Then
                    Dim inputForm As frmCashierInputOrderItem = CType(ctrl, frmCashierInputOrderItem)
                    ' ✅ Now pass productID as well
                    inputForm.FillProductDetails(productID, productName, productBrand, unitWeight, unitPrice)
                    Exit For
                End If
            Next
        End If
    End Sub




End Class