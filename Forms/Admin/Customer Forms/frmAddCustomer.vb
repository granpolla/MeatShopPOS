Imports MySql.Data.MySqlClient
Imports System.Globalization
Imports System.Text.RegularExpressions

Public Class frmAddCustomer

    ' ✅ Capitalize words (Title Case) + Remove extra spaces
    Private Function CleanName(input As String) As String
        ' Trim and collapse multiple spaces into one
        Dim cleaned As String = Regex.Replace(input.Trim(), "\s+", " ")
        ' Convert to Title Case
        Return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cleaned.ToLower())
    End Function

    ' ✅ Save button
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim rawName As String = txtCustomerName.Text
        Dim rawAddress As String = txtAddress.Text

        ' 🔹 Validation
        If String.IsNullOrWhiteSpace(rawName) OrElse String.IsNullOrWhiteSpace(rawAddress) Then
            MessageBox.Show("All fields are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' 🔹 Clean values
        Dim customerName As String = CleanName(rawName)
        Dim address As String = Regex.Replace(rawAddress.Trim(), "\s+", " ") ' just collapse spaces

        ' 🔹 Confirm add
        Dim confirmMsg As String = $"Add this customer?" & vbCrLf & vbCrLf &
                                   $"Customer Name: {customerName}" & vbCrLf &
                                   $"Address: {address}"
        If MessageBox.Show(confirmMsg, "Confirm Add", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                ' 🔹 Check duplicate by NAME only
                Dim checkQuery As String = "SELECT COUNT(*) FROM CUSTOMER WHERE customer_name=@name"
                Using checkCmd As New MySqlCommand(checkQuery, conn)
                    checkCmd.Parameters.AddWithValue("@name", customerName)
                    Dim exists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                    If exists > 0 Then
                        MessageBox.Show("Customer name already exists.", "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                End Using

                ' 🔹 Insert new customer
                Dim insertQuery As String = "
                    INSERT INTO CUSTOMER (customer_name, address, created_at, updated_at)
                    VALUES (@name, @address, NOW(), NOW())"
                Using cmd As New MySqlCommand(insertQuery, conn)
                    cmd.Parameters.AddWithValue("@name", customerName)
                    cmd.Parameters.AddWithValue("@address", address)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Customer added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error adding customer: " & ex.Message)
        End Try
    End Sub

    ' ✅ Cancel button
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
