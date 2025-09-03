Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Module ReceiptGenerator

    Public Sub GenerateReceiptPDF(filePath As String,
                                  orderNo As String,
                                  orderDate As String,
                                  soldTo As String,
                                  address As String,
                                  totalPurchase As String,
                                  grandTotal As String,
                                  orderItemTable As DataTable,
                                  Optional balances As List(Of Tuple(Of String, Decimal)) = Nothing)

        Dim doc As New Document(PageSize.A5, 30, 30, 10, 10)
        PdfWriter.GetInstance(doc, New FileStream(filePath, FileMode.Create))
        doc.Open()

        Dim boldFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13)
        Dim normalFont As Font = FontFactory.GetFont(FontFactory.HELVETICA, 9)

        Dim redBoldFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.RED)

        ' === Header ===
        Dim header As New Paragraph("LASH FROZEN MEAT TRADING, INC.", boldFont)
        header.Alignment = Element.ALIGN_CENTER
        header.SpacingAfter = 15
        doc.Add(header)

        ' === Order Info (3 columns, 2 rows) ===
        Dim orderInfoTable As New PdfPTable(3)
        orderInfoTable.WidthPercentage = 100
        orderInfoTable.SpacingAfter = 10
        orderInfoTable.DefaultCell.Border = Rectangle.NO_BORDER
        orderInfoTable.SetWidths(New Single() {3.5F, 1.5F, 2.1F})

        ' Order No.
        orderInfoTable.AddCell(New PdfPCell(New Phrase(" ", normalFont)) With {.Border = Rectangle.NO_BORDER})
        orderInfoTable.AddCell(New PdfPCell(New Phrase("Order No.:", normalFont)) With {.Border = Rectangle.NO_BORDER, .HorizontalAlignment = Element.ALIGN_RIGHT})
        orderInfoTable.AddCell(New PdfPCell(New Phrase(orderNo, redBoldFont)) With {.Border = Rectangle.BOTTOM_BORDER, .BorderWidthBottom = 0.5F})

        ' Date
        orderInfoTable.AddCell(New PdfPCell(New Phrase(" ", normalFont)) With {.Border = Rectangle.NO_BORDER})
        orderInfoTable.AddCell(New PdfPCell(New Phrase("Date:", normalFont)) With {.Border = Rectangle.NO_BORDER, .HorizontalAlignment = Element.ALIGN_RIGHT})
        orderInfoTable.AddCell(New PdfPCell(New Phrase(orderDate, normalFont)) With {.Border = Rectangle.BOTTOM_BORDER, .BorderWidthBottom = 0.5F})

        doc.Add(orderInfoTable)

        ' === Customer Info (2 rows, 2 columns) ===
        Dim customerTable As New PdfPTable(2)
        customerTable.WidthPercentage = 100
        customerTable.SpacingAfter = 4
        customerTable.DefaultCell.Border = Rectangle.NO_BORDER
        customerTable.SetWidths(New Single() {0.5F, 3.5F}) ' adjust ratio

        ' Row 1: Sold To
        customerTable.AddCell(New PdfPCell(New Phrase("Sold To:", normalFont)) With {.Border = Rectangle.NO_BORDER, .HorizontalAlignment = Element.ALIGN_RIGHT})
        customerTable.AddCell(New PdfPCell(New Phrase(soldTo, normalFont)) With {.Border = Rectangle.BOTTOM_BORDER, .BorderWidthBottom = 0.5F, .HorizontalAlignment = Element.ALIGN_LEFT})

        ' Row 2: Address
        customerTable.AddCell(New PdfPCell(New Phrase("Address:", normalFont)) With {.Border = Rectangle.NO_BORDER, .HorizontalAlignment = Element.ALIGN_RIGHT})
        customerTable.AddCell(New PdfPCell(New Phrase(address, normalFont)) With {.Border = Rectangle.BOTTOM_BORDER, .BorderWidthBottom = 0.5F, .HorizontalAlignment = Element.ALIGN_LEFT})

        doc.Add(customerTable)



        ' === Product Table ===
        Dim table As New PdfPTable(5)
        table.WidthPercentage = 100
        table.SetWidths(New Single() {1.5F, 1.2F, 3.5F, 2.0F, 2.0F})

        ' Header Row
        Dim headers = {"Total KG", "No. Box", "Product Name", "Unit Price", "Total"}
        For Each h In headers
            Dim headerCell As New PdfPCell(New Phrase(h, normalFont)) With {
                .HorizontalAlignment = Element.ALIGN_CENTER,
                .BackgroundColor = BaseColor.LIGHT_GRAY
            }
            table.AddCell(headerCell)
        Next

        ' Data Rows
        Dim rowCount As Integer = 0
        For Each row As DataRow In orderItemTable.Rows
            table.AddCell(New PdfPCell(New Phrase(row("Total KG").ToString(), normalFont)))
            table.AddCell(New PdfPCell(New Phrase(row("No. Box").ToString(), normalFont)))
            table.AddCell(New PdfPCell(New Phrase(row("Product Name").ToString(), normalFont)))
            table.AddCell(New PdfPCell(New Phrase("₱" & Convert.ToDecimal(row("Unit Price")).ToString("N2"), normalFont)))
            table.AddCell(New PdfPCell(New Phrase("₱" & Convert.ToDecimal(row("Total")).ToString("N2"), normalFont)))
            rowCount += 1
        Next

        ' Fill to 18 rows
        While rowCount < 18
            For i As Integer = 1 To 5
                Dim emptyCell As New PdfPCell(New Phrase(" ", normalFont)) With {
                    .FixedHeight = 14,
                    .Border = Rectangle.BOX
                }
                table.AddCell(emptyCell)
            Next
            rowCount += 1
        End While

        ' Total Purchase
        table.AddCell(New PdfPCell(New Phrase("Total Purchase", normalFont)) With {.Colspan = 4, .HorizontalAlignment = Element.ALIGN_CENTER})
        table.AddCell(New PdfPCell(New Phrase("₱" & totalPurchase, normalFont)) With {.HorizontalAlignment = Element.ALIGN_RIGHT})

        ' Balances
        Dim maxBalanceRows As Integer = 4
        Dim currentBalanceRow As Integer = 0
        If balances IsNot Nothing AndAlso balances.Count > 0 Then
            For Each bal In balances
                If currentBalanceRow >= maxBalanceRows Then Exit For
                table.AddCell(New PdfPCell(New Phrase("Balance", normalFont)))
                table.AddCell(New PdfPCell(New Phrase(bal.Item1, normalFont)) With {.Colspan = 3})
                table.AddCell(New PdfPCell(New Phrase("₱" & bal.Item2.ToString("N2"), normalFont)) With {.HorizontalAlignment = Element.ALIGN_RIGHT})
                currentBalanceRow += 1
            Next
        End If
        While currentBalanceRow < maxBalanceRows
            table.AddCell(New PdfPCell(New Phrase("Balance", normalFont)))
            table.AddCell(New PdfPCell(New Phrase("", normalFont)) With {.Colspan = 3})
            table.AddCell(New PdfPCell(New Phrase(" ", normalFont)))
            currentBalanceRow += 1
        End While

        ' Grand Total
        table.AddCell(New PdfPCell(New Phrase("Grand Total", normalFont)) With {.Colspan = 4, .HorizontalAlignment = Element.ALIGN_CENTER})
        table.AddCell(New PdfPCell(New Phrase("₱" & grandTotal, normalFont)) With {.HorizontalAlignment = Element.ALIGN_RIGHT})

        doc.Add(table)
        doc.Close()
    End Sub

End Module
