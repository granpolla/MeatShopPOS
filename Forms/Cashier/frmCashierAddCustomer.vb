Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions

Public Class frmCashierAddCustomer

    ' ✅ Normalize name/address (remove extra spaces + proper case)
    Private Function NormalizeText(input As String, Optional properCase As Boolean = True) As String
        ' Remove extra spaces
        Dim cleaned As String = Regex.Replace(input.Trim(), "\s+", " ")
        ' Convert to Proper Case (e.g., "mark jones" -> "Mark Jones")
        If properCase Then
            cleaned = StrConv(cleaned, VbStrConv.ProperCase)
        End If
        Return cleaned
    End Function

    ' ✅ Save Button
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim name As String = NormalizeText(txtCustomerName.Text, True)    ' proper case
        Dim address As String = NormalizeText(txtAddress.Text, True)      ' proper case too (optional, you can set False if you want all caps or raw)

        If String.IsNullOrWhiteSpace(name) OrElse String.IsNullOrWhiteSpace(address) Then
            MessageBox.Show("Please enter both name and address.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()
                Dim query As String = "INSERT INTO customer (customer_name, address) VALUES (@name, @address)"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@name", name)
                    cmd.Parameters.AddWithValue("@address", address)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Customer saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error saving customer: " & ex.Message)
        End Try
    End Sub

    ' ✅ Cancel Button
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class
