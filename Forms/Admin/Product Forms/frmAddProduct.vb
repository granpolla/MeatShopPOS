Imports MySql.Data.MySqlClient
Imports System.Globalization
Imports System.Text.RegularExpressions

Public Class frmAddProduct

    ' ✅ Clean input: trim, collapse multiple spaces → one, and Title Case
    Private Function CleanText(input As String) As String
        Dim cleaned As String = Regex.Replace(input.Trim(), "\s+", " ")
        Return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cleaned.ToLower())
    End Function

    ' ✅ Save product
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim rawProductName As String = txtProductName.Text
        Dim rawBrand As String = txtBrand.Text
        Dim rawWeight As String = txtWeight.Text
        Dim rawPrice As String = txtPrice.Text

        ' 🔹 Validation
        If String.IsNullOrWhiteSpace(rawProductName) OrElse
           String.IsNullOrWhiteSpace(rawBrand) OrElse
           String.IsNullOrWhiteSpace(rawWeight) OrElse
           String.IsNullOrWhiteSpace(rawPrice) Then
            MessageBox.Show("All fields are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' 🔹 Validate numeric fields
        Dim weightValue As Decimal
        Dim priceValue As Decimal
        If Not Decimal.TryParse(rawWeight, weightValue) OrElse weightValue <= 0 Then
            MessageBox.Show("Weight must be a valid positive number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If Not Decimal.TryParse(rawPrice, priceValue) OrElse priceValue <= 0 Then
            MessageBox.Show("Price must be a valid positive number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' 🔹 Clean text fields
        Dim productName As String = CleanText(rawProductName)
        Dim brand As String = CleanText(rawBrand)

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

                ' 🔹 Check duplicate (Product Name + Brand + Weight + Price)
                Dim checkQuery As String = "SELECT COUNT(*) 
                            FROM PRODUCT 
                            WHERE product_name=@name 
                              AND brand=@brand 
                              AND unit_weight_kg=@weight 
                              AND unit_price_php=@price"

                Using checkCmd As New MySqlCommand(checkQuery, conn)
                    checkCmd.Parameters.AddWithValue("@name", productName)
                    checkCmd.Parameters.AddWithValue("@brand", brand)
                    checkCmd.Parameters.AddWithValue("@weight", weightValue)
                    checkCmd.Parameters.AddWithValue("@price", priceValue)

                    Dim exists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                    If exists > 0 Then
                        MessageBox.Show("This product already exists with the same weight and price.",
                        "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
