Imports MySql.Data.MySqlClient
Imports System.Globalization

Public Class frmEditProduct
    Public Property ProductID As Integer

    ' ✅ Capitalize words
    Private Function ToTitleCase(input As String) As String
        Return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower())
    End Function

    ' ✅ Save changes
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim productName As String = txtProductName.Text.Trim()
        Dim brand As String = txtBrand.Text.Trim()
        Dim weight As String = txtWeight.Text.Trim()
        Dim price As String = txtPrice.Text.Trim()

        ' 🔹 Validation
        If String.IsNullOrWhiteSpace(productName) OrElse
           String.IsNullOrWhiteSpace(brand) OrElse
           String.IsNullOrWhiteSpace(weight) OrElse
           String.IsNullOrWhiteSpace(price) Then
            MessageBox.Show("All fields are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' 🔹 Numeric validation
        Dim weightValue As Decimal
        Dim priceValue As Decimal
        If Not Decimal.TryParse(weight, weightValue) OrElse weightValue <= 0 Then
            MessageBox.Show("Weight must be a valid positive number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If Not Decimal.TryParse(price, priceValue) OrElse priceValue <= 0 Then
            MessageBox.Show("Price must be a valid positive number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' 🔹 Format names
        productName = ToTitleCase(productName)
        brand = ToTitleCase(brand)

        ' 🔹 Confirm update
        Dim confirmMsg As String = $"Update this product?" & vbCrLf & vbCrLf &
                                   $"Product Name: {productName}" & vbCrLf &
                                   $"Brand: {brand}" & vbCrLf &
                                   $"Weight: {weightValue} kg" & vbCrLf &
                                   $"Price: ₱{priceValue:F2}"
        If MessageBox.Show(confirmMsg, "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                ' 🔹 Check for duplicates (excluding the current product)
                Dim checkQuery As String = "SELECT COUNT(*) FROM PRODUCT WHERE product_name=@name AND brand=@brand AND id<>@id"
                Using checkCmd As New MySqlCommand(checkQuery, conn)
                    checkCmd.Parameters.AddWithValue("@name", productName)
                    checkCmd.Parameters.AddWithValue("@brand", brand)
                    checkCmd.Parameters.AddWithValue("@id", ProductID)
                    Dim exists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                    If exists > 0 Then
                        MessageBox.Show("Another product with the same name and brand already exists.", "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                End Using

                ' 🔹 Update product
                Dim updateQuery As String = "
                    UPDATE PRODUCT
                    SET product_name=@name, brand=@brand, unit_weight_kg=@weight, unit_price_php=@price, updated_at=NOW()
                    WHERE id=@id"
                Using cmd As New MySqlCommand(updateQuery, conn)
                    cmd.Parameters.AddWithValue("@name", productName)
                    cmd.Parameters.AddWithValue("@brand", brand)
                    cmd.Parameters.AddWithValue("@weight", weightValue)
                    cmd.Parameters.AddWithValue("@price", priceValue)
                    cmd.Parameters.AddWithValue("@id", ProductID)

                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error updating product: " & ex.Message)
        End Try
    End Sub

    ' ✅ Cancel button
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
