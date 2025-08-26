Imports MySql.Data.MySqlClient
Imports System.Globalization

Public Class frmAddProduct

    ' ✅ Capitalize words (e.g., "my meat" → "My Meat")
    Private Function ToTitleCase(input As String) As String
        Return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower())
    End Function

    ' ✅ Save product
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

        ' 🔹 Validate numeric fields
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

        ' 🔹 Confirm popup
        Dim confirmMsg As String = $"Please confirm product details before saving:" & vbCrLf & vbCrLf &
                                   $"Product Name: {productName}" & vbCrLf &
                                   $"Brand: {brand}" & vbCrLf &
                                   $"Weight: {weightValue} kg" & vbCrLf &
                                   $"Price: ₱{priceValue:F2}"
        Dim result As DialogResult = MessageBox.Show(confirmMsg, "Confirm Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.No Then Return

        Try
            Using conn As New MySqlConnection(My.Settings.DBConnection)
                conn.Open()

                ' 🔹 Check duplicate (same Product Name + Brand)
                Dim checkQuery As String = "SELECT COUNT(*) FROM PRODUCT WHERE product_name=@name AND brand=@brand"
                Using checkCmd As New MySqlCommand(checkQuery, conn)
                    checkCmd.Parameters.AddWithValue("@name", productName)
                    checkCmd.Parameters.AddWithValue("@brand", brand)
                    Dim exists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                    If exists > 0 Then
                        MessageBox.Show("This product already exists.", "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                End Using

                ' 🔹 Insert new product
                Dim insertQuery As String = "
                    INSERT INTO PRODUCT (product_name, brand, unit_weight_kg, unit_price_php, created_at, updated_at)
                    VALUES (@name, @brand, @weight, @price, NOW(), NOW())"
                Using cmd As New MySqlCommand(insertQuery, conn)
                    cmd.Parameters.AddWithValue("@name", productName)
                    cmd.Parameters.AddWithValue("@brand", brand)
                    cmd.Parameters.AddWithValue("@weight", weightValue)
                    cmd.Parameters.AddWithValue("@price", priceValue)

                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error adding product: " & ex.Message)
        End Try
    End Sub

    ' ✅ Cancel button
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
