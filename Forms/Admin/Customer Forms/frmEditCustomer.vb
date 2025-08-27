Imports MySql.Data.MySqlClient
Imports System.Globalization
Imports System.Text.RegularExpressions

Public Class frmEditCustomer
    Private CustomerId As Integer
    Private OriginalCustomerName As String
    Private OriginalAddress As String

    ' ✅ Constructor (pass customer ID when opening form)
    Public Sub New(customerId As Integer)
        InitializeComponent()
        Me.CustomerId = customerId
    End Sub

    ' ✅ Normalize spacing and capitalize words
    Private Function NormalizeText(input As String) As String
        ' Remove extra spaces and keep only single spaces
        Dim clean As String = Regex.Replace(input.Trim(), "\s+", " ")
        ' Capitalize each word
        Return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(clean.ToLower())
    End Function

    ' ✅ Load customer data on form load
    Private Sub frmEditCustomer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                Dim query As String = "SELECT customer_name, address FROM CUSTOMER WHERE id=@id"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", CustomerId)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            txtCustomerName.Text = reader("customer_name").ToString()
                            txtAddress.Text = reader("address").ToString()

                            ' Save original values for later comparison
                            OriginalCustomerName = txtCustomerName.Text
                            OriginalAddress = txtAddress.Text
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading customer: " & ex.Message)
        End Try
    End Sub


    'For save button
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim customerName As String = NormalizeText(txtCustomerName.Text)
        Dim address As String = NormalizeText(txtAddress.Text)

        ' 🔹 Validation
        If String.IsNullOrWhiteSpace(customerName) OrElse String.IsNullOrWhiteSpace(address) Then
            MessageBox.Show("Customer Name and Address are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                ' 🔹 Check if customer is referenced in sales_transaction
                Dim refQuery As String = "SELECT COUNT(*) FROM sales_transaction WHERE customer_id=@id"
                Dim isReferenced As Boolean
                Using refCmd As New MySqlCommand(refQuery, conn)
                    refCmd.Parameters.AddWithValue("@id", CustomerId)
                    isReferenced = Convert.ToInt32(refCmd.ExecuteScalar()) > 0
                End Using

                If isReferenced Then
                    ' ✅ Customer is referenced → name cannot be changed
                    If customerName <> OriginalCustomerName Then
                        MessageBox.Show("Customer name cannot be changed because it is already referenced in transactions.", "Restricted Update", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If

                    ' ✅ Only update address if changed
                    If address <> OriginalAddress Then
                        Dim updateQuery As String = "UPDATE CUSTOMER SET address=@address, updated_at=NOW() WHERE id=@id"
                        Using cmd As New MySqlCommand(updateQuery, conn)
                            cmd.Parameters.AddWithValue("@address", address)
                            cmd.Parameters.AddWithValue("@id", CustomerId)
                            cmd.ExecuteNonQuery()
                        End Using
                        MessageBox.Show("Address updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Me.DialogResult = DialogResult.OK
                        Me.Close()
                    Else
                        MessageBox.Show("No changes were made.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                Else
                    ' ✅ Customer not referenced → allow full update

                    ' 🔹 If no actual changes were made, skip update
                    If customerName = OriginalCustomerName AndAlso address = OriginalAddress Then
                        MessageBox.Show("No changes were made.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If

                    ' 🔹 Check for duplicate names
                    Dim checkQuery As String = "SELECT COUNT(*) FROM CUSTOMER WHERE customer_name=@name AND id<>@id"
                    Using checkCmd As New MySqlCommand(checkQuery, conn)
                        checkCmd.Parameters.AddWithValue("@name", customerName)
                        checkCmd.Parameters.AddWithValue("@id", CustomerId)
                        Dim exists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                        If exists > 0 Then
                            MessageBox.Show("Another customer with this name already exists.", "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return
                        End If
                    End Using

                    ' 🔹 Perform full update
                    Dim updateQueryFull As String = "UPDATE CUSTOMER SET customer_name=@name, address=@address, updated_at=NOW() WHERE id=@id"
                    Using cmd As New MySqlCommand(updateQueryFull, conn)
                        cmd.Parameters.AddWithValue("@name", customerName)
                        cmd.Parameters.AddWithValue("@address", address)
                        cmd.Parameters.AddWithValue("@id", CustomerId)
                        cmd.ExecuteNonQuery()
                    End Using

                    MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                End If
            End Using

        Catch ex As Exception
            MessageBox.Show("Error updating customer: " & ex.Message)
        End Try
    End Sub





    ' ✅ Cancel button
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
