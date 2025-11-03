Imports System.Data

Module ReceiptTableBuilder

    ' Build a DataTable that matches ReceiptGenerator expectations:
    ' Columns: "QTY", "UNIT", "ARTICLES", "UNIT PRICE", "AMOUNT"
    Public Function BuildReceiptTable(originalOrderTable As DataTable) As DataTable
        Dim orderTable As New DataTable()
        orderTable.Columns.Add("QTY", GetType(String))
        orderTable.Columns.Add("UNIT", GetType(String))
        orderTable.Columns.Add("ARTICLES", GetType(String))
        orderTable.Columns.Add("UNIT PRICE", GetType(Decimal))
        orderTable.Columns.Add("AMOUNT", GetType(Decimal))

        If originalOrderTable Is Nothing Then
            Return orderTable
        End If

        For Each r As DataRow In originalOrderTable.Rows
            Dim qtyVal As String = ""
            Dim unitVal As String = ""
            Dim articleVal As String = ""
            Dim unitPriceVal As Decimal = 0D
            Dim amountVal As Decimal = 0D
            Dim totalBoxDecimal As Decimal = 0D
            Dim totalWeightDecimal As Decimal = 0D

            ' Read Total Box numeric
            If originalOrderTable.Columns.Contains("Total Box") Then
                Decimal.TryParse(Convert.ToString(r("Total Box")), totalBoxDecimal)
            End If

            ' Read Total Weight numeric
            If originalOrderTable.Columns.Contains("Total Weight") Then
                Decimal.TryParse(Convert.ToString(r("Total Weight")), totalWeightDecimal)
            End If

            ' UNIT should equal Total Box (integer style)
            If totalBoxDecimal > 0D Then
                unitVal = totalBoxDecimal.ToString("N0")
            ElseIf originalOrderTable.Columns.Contains("UNIT") Then
                unitVal = Convert.ToString(r("UNIT"))
            Else
                unitVal = ""
            End If

            ' QTY should equal Total Weight (preserve integer when whole)
            If totalWeightDecimal <> 0D Then
                If totalWeightDecimal = Math.Truncate(totalWeightDecimal) Then
                    qtyVal = totalWeightDecimal.ToString("N0")
                Else
                    qtyVal = totalWeightDecimal.ToString("N2")
                End If
            ElseIf originalOrderTable.Columns.Contains("QTY") Then
                qtyVal = Convert.ToString(r("QTY"))
            ElseIf originalOrderTable.Columns.Contains("Quantity") Then
                qtyVal = Convert.ToString(r("Quantity"))
            Else
                qtyVal = "1"
            End If

            ' ARTICLES mapping
            If originalOrderTable.Columns.Contains("ARTICLES") Then
                articleVal = Convert.ToString(r("ARTICLES"))
            ElseIf originalOrderTable.Columns.Contains("Article") Then
                articleVal = Convert.ToString(r("Article"))
            ElseIf originalOrderTable.Columns.Contains("Product Name") AndAlso originalOrderTable.Columns.Contains("Brand") Then
                articleVal = $"{r("Product Name")} - {r("Brand")}"
            ElseIf originalOrderTable.Columns.Contains("ProductName") AndAlso originalOrderTable.Columns.Contains("Brand") Then
                articleVal = $"{r("ProductName")} - {r("Brand")}"
            ElseIf originalOrderTable.Columns.Contains("Product Name") Then
                articleVal = Convert.ToString(r("Product Name"))
            ElseIf originalOrderTable.Columns.Contains("ProductName") Then
                articleVal = Convert.ToString(r("ProductName"))
            ElseIf originalOrderTable.Columns.Contains("Description") Then
                articleVal = Convert.ToString(r("Description"))
            ElseIf originalOrderTable.Columns.Contains("ProductID") Then
                articleVal = Convert.ToString(r("ProductID"))
            Else
                articleVal = ""
            End If



            ' UNIT PRICE mapping
            If originalOrderTable.Columns.Contains("Unit Price") Then
                Decimal.TryParse(Convert.ToString(r("Unit Price")), unitPriceVal)
            ElseIf originalOrderTable.Columns.Contains("UnitPrice") Then
                Decimal.TryParse(Convert.ToString(r("UnitPrice")), unitPriceVal)
            Else
                unitPriceVal = 0D
            End If

            ' AMOUNT mapping (use Total if present, otherwise fallback to unitPrice * totalBox)
            If originalOrderTable.Columns.Contains("Total") Then
                Decimal.TryParse(Convert.ToString(r("Total")), amountVal)
            ElseIf originalOrderTable.Columns.Contains("AMOUNT") Then
                Decimal.TryParse(Convert.ToString(r("AMOUNT")), amountVal)
            ElseIf originalOrderTable.Columns.Contains("Subtotal") Then
                Decimal.TryParse(Convert.ToString(r("Subtotal")), amountVal)
            Else
                If totalBoxDecimal > 0D Then
                    amountVal = unitPriceVal * totalBoxDecimal
                Else
                    amountVal = unitPriceVal
                End If
            End If

            orderTable.Rows.Add(qtyVal, unitVal, articleVal, unitPriceVal, amountVal)
        Next

        Return orderTable
    End Function

End Module