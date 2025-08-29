Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions

Public Class frmCashierDashboard

    Private Sub frmCashierDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCustomers()
        LoadProducts()
        LoadInputOrderItemForm()
    End Sub

    ' ✅ Normalize name (remove extra spaces + proper case)
    Private Function NormalizeName(input As String) As String
        Dim cleaned As String = Regex.Replace(input.Trim(), "\s+", " ")
        Return StrConv(cleaned, VbStrConv.ProperCase)
    End Function

    ' ✅ Add order item row to dgvOrderItemPreview
    Public Sub AddOrderItem(productName As String, brand As String, unitWeight As Decimal, unitPrice As Decimal,
                        totalBox As Decimal, totalWeight As Decimal, total As Decimal)

        ' Create DataTable if dgvOrderItemPreview is empty
        If dgvOrderItemPreview.DataSource Is Nothing Then
            Dim dt As New DataTable()
            dt.Columns.Add("Product Name", GetType(String))
            dt.Columns.Add("Brand", GetType(String))
            dt.Columns.Add("Unit Weight", GetType(Decimal))
            dt.Columns.Add("Unit Price", GetType(Decimal))
            dt.Columns.Add("Total Box", GetType(Decimal))
            dt.Columns.Add("Total Weight", GetType(Decimal))
            dt.Columns.Add("Total", GetType(Decimal))
            dgvOrderItemPreview.DataSource = dt
        End If

        ' Cast DataSource back to DataTable
        Dim orderTable As DataTable = CType(dgvOrderItemPreview.DataSource, DataTable)

        ' Add the new row
        orderTable.Rows.Add(productName, brand, unitWeight, unitPrice, totalBox, totalWeight, total)

        ' Optional formatting
        With dgvOrderItemPreview
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True

            ' Format numeric columns
            .Columns("Unit Price").DefaultCellStyle.Format = "N2"
            .Columns("Unit Weight").DefaultCellStyle.Format = "N2"
            .Columns("Total Box").DefaultCellStyle.Format = "N2"
            .Columns("Total Weight").DefaultCellStyle.Format = "N2"
            .Columns("Total").DefaultCellStyle.Format = "N2"
        End With
    End Sub






    ' ✅ Load all customers
    Private Sub LoadCustomers()
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
        SELECT cl.entry_date AS 'Date',
               cl.description AS 'Description',
               cl.amount AS 'Balance'
        FROM customer_ledger cl
        WHERE cl.customer_id = @custID
          AND cl.amount > 0
        ORDER BY cl.entry_date DESC"

        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@custID", customerID)

                    Dim adapter As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    adapter.Fill(dt)

                    dgvCustomerBalancePreview.DataSource = dt

                    ' If no rows → show empty table
                    If dt.Rows.Count = 0 Then
                        dgvCustomerBalancePreview.DataSource = Nothing
                    End If

                    ' format
                    With dgvCustomerBalancePreview
                        .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                        .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                        .MultiSelect = False
                        .ReadOnly = True
                    End With
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading balance: " & ex.Message)
        End Try
    End Sub

    ' ✅ Load all products into dgvProductsPreview
    Private Sub LoadProducts()
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






    ' ✅ Format dgvCustomer
    Private Sub FormatCustomerGrid()
        With dgvCustomer
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True

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

            ' Hide ProductID
            If .Columns.Contains("ProductID") Then
                .Columns("ProductID").Visible = False
            End If

            ' Format numeric columns
            If .Columns.Contains("Unit Price") Then
                .Columns("Unit Price").DefaultCellStyle.Format = "N2"
            End If
            If .Columns.Contains("Unit Weight") Then
                .Columns("Unit Weight").DefaultCellStyle.Format = "N2"
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
        LoadCustomers()
        txtFirstName.Clear()
        txtLastName.Clear()
        txtCustomerAddress.Clear()
        dgvCustomerBalancePreview.DataSource = Nothing
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtFirstName.Clear()
        txtLastName.Clear()
        txtCustomerAddress.Clear()
        dgvCustomerBalancePreview.DataSource = Nothing
    End Sub





    ' ✅ When selecting customer → autofill + load balances
    Private Sub dgvCustomer_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCustomer.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvCustomer.Rows(e.RowIndex)

            Dim fullName As String = row.Cells("Full Name").Value.ToString()
            ' Use a space separator and max 2 parts
            Dim nameParts() As String = fullName.Split(New Char() {" "c}, 2, StringSplitOptions.RemoveEmptyEntries)

            ' Split into First and Last Name
            If nameParts.Length > 0 Then
                txtFirstName.Text = nameParts(0) ' First word
            End If
            If nameParts.Length > 1 Then
                txtLastName.Text = nameParts(1) ' Everything after the first space
            Else
                txtLastName.Clear()
            End If

            txtCustomerAddress.Text = row.Cells("Address").Value.ToString()

            ' get the ID for balance lookup
            Dim customerID As Integer = Convert.ToInt32(row.Cells("CustomerID").Value)
            LoadCustomerBalance(customerID)
        End If
    End Sub

    ' ✅ When clicking a row in dgvCustomerBalancePreview → show popup
    Private Sub dgvCustomerBalancePreview_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCustomerBalancePreview.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvCustomerBalancePreview.Rows(e.RowIndex)

            Dim entryDate As String = row.Cells("Date").Value.ToString()
            Dim description As String = row.Cells("Description").Value.ToString()
            Dim balance As Decimal = Convert.ToDecimal(row.Cells("Balance").Value)

            MessageBox.Show($"Date: {entryDate}" & vbCrLf &
                        $"Description: {description}" & vbCrLf &
                        $"Balance: ₱{balance:N2}",
                        "Customer Balance Click",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information)
        End If
    End Sub


    Private Sub dgvProductsPreview_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProductsPreview.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvProductsPreview.Rows(e.RowIndex)

            ' Get product details
            Dim productName As String = row.Cells("Product Name").Value.ToString()
            Dim productBrand As String = row.Cells("Brand").Value.ToString()
            Dim unitWeight As Decimal = Convert.ToDecimal(row.Cells("Unit Weight").Value)
            Dim unitPrice As Decimal = Convert.ToDecimal(row.Cells("Unit Price").Value)

            ' Find frmCashierInputOrderItem inside pnlInputOrderItem
            For Each ctrl As Control In pnlInputOrderItem.Controls
                If TypeOf ctrl Is frmCashierInputOrderItem Then
                    Dim inputForm As frmCashierInputOrderItem = CType(ctrl, frmCashierInputOrderItem)
                    inputForm.FillProductDetails(productName, productBrand, unitWeight, unitPrice)
                    Exit For
                End If
            Next
        End If
    End Sub



End Class