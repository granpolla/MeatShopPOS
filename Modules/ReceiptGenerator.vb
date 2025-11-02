Imports System.Data
Imports System.Drawing.Printing
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
                                  Optional tin As String = "",
                                  Optional busStyle As String = "",
                                  Optional preparedBy As String = "",
                                  Optional approvedBy As String = "",
                                  Optional receivedBy As String = "",
                                  Optional balances As List(Of Tuple(Of String, Decimal)) = Nothing)

        ' --- DOCUMENT SETUP ---
        Dim doc As New Document(PageSize.A5, 25, 25, 15, 10) ' (Left, Right, Top, Bottom)
        If File.Exists(filePath) Then File.Delete(filePath)
        PdfWriter.GetInstance(doc, New FileStream(filePath, FileMode.Create))
        doc.Open()

        ' --- FONT DEFINITIONS ---
        Dim boldFontHeader As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13)
        Dim normalSubHeader As Font = FontFactory.GetFont(FontFactory.HELVETICA, 6)
        Dim normalFontHeader As Font = FontFactory.GetFont(FontFactory.HELVETICA, 8)
        Dim boldFontBody As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9)
        Dim normalFontBody As Font = FontFactory.GetFont(FontFactory.HELVETICA, 7)
        Dim footerNoteFont As Font = FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC, 7)
        Dim articleValueFont As Font = FontFactory.GetFont(FontFactory.HELVETICA, 8)
        Dim boldFontOrderNo As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.BLACK)
        Dim orderSlipFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.BLACK)
        Dim labelSmallFont As Font = FontFactory.GetFont(FontFactory.HELVETICA, 5)
        Dim articleHeaderFont As Font = FontFactory.GetFont(FontFactory.TIMES_BOLD, 8)
        Dim timesRomanRegular As Font = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 7)

        ' ---------------------------------------------------
        ' --- RECEIPT HEADER (Company Name, Address, TIN) ---
        ' ---------------------------------------------------
        Dim headerParagraph As New Paragraph()
        headerParagraph.Alignment = Element.ALIGN_CENTER
        headerParagraph.Leading = 8.0F

        ' Line 1: Company Name
        headerParagraph.Add(New Chunk("LASH FROZEN MEAT TRADING, INC.", boldFontHeader))
        headerParagraph.Add(Chunk.NEWLINE)

        ' Line 2: Address
        Dim addressText As String = "112 Macabagdal St. Brgy. 86 Dist. II 1400 Caloocan City NCR, Third District Philippines"
        headerParagraph.Add(New Chunk(addressText, normalSubHeader))
        headerParagraph.Add(Chunk.NEWLINE)

        ' Line 3: Non VAT Reg. TIN
        Dim tinText As String = "Non VAT Reg. TIN: 010-561-596-00000"
        headerParagraph.Add(New Chunk(tinText, normalSubHeader))
        headerParagraph.SpacingAfter = 15
        doc.Add(headerParagraph)



        ' ---------------------------------------------------
        ' --- ORDER SLIP TITLE & NUMBER ---
        ' ---------------------------------------------------
        Dim orderTitleTable As New PdfPTable(3)
        orderTitleTable.WidthPercentage = 100
        orderTitleTable.SetWidths(New Single() {1.8F, 2.0F, 6.2F})
        orderTitleTable.SpacingAfter = 2

        ' Cell 1: ORDER SLIP Box (Gray Background)
        Dim orderSlipCell As New PdfPCell(New Phrase("ORDER SLIP", orderSlipFont)) With {
            .BackgroundColor = New BaseColor(150, 150, 150), ' Dark Gray
            .HorizontalAlignment = Element.ALIGN_CENTER,
            .VerticalAlignment = Element.ALIGN_MIDDLE,
            .Border = Rectangle.NO_BORDER,
            .FixedHeight = 16 ' Adjust height to look like a box
        }
        orderTitleTable.AddCell(orderSlipCell)

        ' Cell 2: Empty Spacer
        orderTitleTable.AddCell(New PdfPCell(New Phrase(" ", normalFontBody)) With {.Border = Rectangle.NO_BORDER})

        ' Cell 3: No. and Number (Right Aligned)
        Dim orderNoPhrase As New Phrase()
        orderNoPhrase.Add(New Chunk("No. ", boldFontBody))
        orderNoPhrase.Add(New Chunk(orderNo, boldFontOrderNo))
        Dim orderNoCell As New PdfPCell(orderNoPhrase) With {
            .Border = Rectangle.NO_BORDER,
            .HorizontalAlignment = Element.ALIGN_RIGHT,
            .PaddingTop = 0, .PaddingBottom = 0 ' Tighter fit
        }
        orderTitleTable.AddCell(orderNoCell)

        doc.Add(orderTitleTable)



        ' ------------------------------------------------------------
        ' --- CUSTOMER INFO (Name, Address, Date, TIN, Bus. Style) ---
        ' ------------------------------------------------------------
        Dim infoTable As New PdfPTable(2)
        infoTable.WidthPercentage = 100
        infoTable.SpacingAfter = 3
        infoTable.SetWidths(New Single() {5.5F, 3.5F}) ' Left side wider than right

        Const FIXED_ROW_HEIGHT As Single = 20.0F
        Const RIGHT_ROW_HEIGHT As Single = (FIXED_ROW_HEIGHT * 2.0F) / 3.0F

        ' --- Left Side: Customer Name & Address (Inner Table) ---
        Dim leftInfoTable As New PdfPTable(2)
        leftInfoTable.WidthPercentage = 100
        leftInfoTable.DefaultCell.Border = Rectangle.NO_BORDER
        leftInfoTable.SetWidths(New Single() {1.2F, 4.3F}) ' Label vs Value ratio

        ' Customer Name Row (APPLY BOTTOM BORDER TO BOTH CELLS)
        leftInfoTable.AddCell(New PdfPCell(New Phrase("Customer Name:", labelSmallFont)) With {
            .Border = Rectangle.BOTTOM_BORDER, .BorderWidthBottom = 0.5F,
            .MinimumHeight = FIXED_ROW_HEIGHT
        })
        leftInfoTable.AddCell(New PdfPCell(New Phrase(soldTo, normalFontBody)) With {
            .Border = Rectangle.BOTTOM_BORDER,
            .BorderWidthBottom = 0.5F,
            .MinimumHeight = FIXED_ROW_HEIGHT,
            .VerticalAlignment = Element.ALIGN_MIDDLE
        })

        ' Address Row
        leftInfoTable.AddCell(New PdfPCell(New Phrase("Address:", labelSmallFont)) With {
            .Border = Rectangle.NO_BORDER,
            .MinimumHeight = FIXED_ROW_HEIGHT
        })
        leftInfoTable.AddCell(New PdfPCell(New Phrase(address, normalFontBody)) With {
            .Border = Rectangle.NO_BORDER,
            .MinimumHeight = FIXED_ROW_HEIGHT,
            .VerticalAlignment = Element.ALIGN_MIDDLE
        })

        ' Left Info Container Cell - NOW WITH BORDER
        Dim leftCell As New PdfPCell(leftInfoTable) With {
            .Border = Rectangle.BOX,
            .BorderWidth = 0.5F
        }
        infoTable.AddCell(leftCell)


        ' --- Right Side: Date, TIN, BUS. STYLE (Inner Table) ---
        Dim rightInfoTable As New PdfPTable(2)
        rightInfoTable.WidthPercentage = 100
        rightInfoTable.DefaultCell.Border = Rectangle.NO_BORDER
        rightInfoTable.SetWidths(New Single() {1.5F, 4.5F})


        ' DATE Row
        rightInfoTable.AddCell(New PdfPCell(New Phrase("DATE:", labelSmallFont)) With {
            .PaddingLeft = 2,
            .Border = Rectangle.BOTTOM_BORDER, .BorderWidthBottom = 0.5F,
            .MinimumHeight = RIGHT_ROW_HEIGHT ' <<< FIX: Apply height
        })
        rightInfoTable.AddCell(New PdfPCell(New Phrase(orderDate, normalFontBody)) With {
            .Border = Rectangle.BOTTOM_BORDER, .BorderWidthBottom = 0.5F,
            .MinimumHeight = RIGHT_ROW_HEIGHT ' <<< FIX: Apply height
        })

        ' TIN Row
        rightInfoTable.AddCell(New PdfPCell(New Phrase("TIN:", labelSmallFont)) With {
            .PaddingLeft = 2,
            .Border = Rectangle.BOTTOM_BORDER, .BorderWidthBottom = 0.5F,
            .MinimumHeight = RIGHT_ROW_HEIGHT ' <<< FIX: Apply height
        })
        rightInfoTable.AddCell(New PdfPCell(New Phrase(tin, normalFontBody)) With {
            .Border = Rectangle.BOTTOM_BORDER, .BorderWidthBottom = 0.5F,
            .MinimumHeight = RIGHT_ROW_HEIGHT ' <<< FIX: Apply height
        })

        ' BUS. STYLE Row - NO BOTTOM BORDER
        rightInfoTable.AddCell(New PdfPCell(New Phrase("BUS. STYLE:", labelSmallFont)) With {
            .PaddingLeft = 2,
            .Border = Rectangle.NO_BORDER,
            .MinimumHeight = RIGHT_ROW_HEIGHT ' <<< FIX: Apply height
        })
        rightInfoTable.AddCell(New PdfPCell(New Phrase(busStyle, normalFontBody)) With {
            .Border = Rectangle.NO_BORDER,
            .MinimumHeight = RIGHT_ROW_HEIGHT ' <<< FIX: Apply height
        })

        ' Right Info Container Cell (Outer container)
        Dim rightCell As New PdfPCell(rightInfoTable) With {
            .Border = Rectangle.BOX,
            .BorderWidth = 0.5F,
            .PaddingLeft = 0
        }
        infoTable.AddCell(rightCell)

        doc.Add(infoTable)



        ' ---------------------------------------------------
        ' --- MAIN ARTICLES TABLE ---
        ' ---------------------------------------------------
        Dim productTable As New PdfPTable(5)
        productTable.WidthPercentage = 100
        productTable.SetWidths(New Single() {0.8F, 0.8F, 4.1F, 1.7F, 1.6F}) ' QTY, UNIT, ARTICLES, UNIT PRICE, AMOUNT

        ' Set common border style for data and empty cells
        Dim cellBorder As Single = 0.5F
        Dim fixedRowHeight As Single = 16.0F

        ' Header Row - NOW WITH FULL BORDERS (Rectangle.BOX)
        Dim headers = {"QTY.", "UNIT", "ARTICLES", "UNIT PRICE", "AMOUNT"}
        For Each h In headers
            Dim headerCell As New PdfPCell(New Phrase(h, articleHeaderFont)) With {
                .HorizontalAlignment = Element.ALIGN_CENTER,
                .VerticalAlignment = Element.ALIGN_MIDDLE,
                .Border = Rectangle.BOX,
                .BorderWidth = 0.5F,
                .PaddingBottom = 5, .PaddingTop = 5
            }
            productTable.AddCell(headerCell)
        Next

        ' Data Rows
        Dim rowCount As Integer = 0
        For Each row As DataRow In orderItemTable.Rows
            ' Data Cells: Set border using Rectangle.BOX
            productTable.AddCell(New PdfPCell(New Phrase(row("QTY").ToString(), articleValueFont)) With {
                .HorizontalAlignment = Element.ALIGN_CENTER,
                .FixedHeight = fixedRowHeight,
                .Border = Rectangle.BOX, .BorderWidth = cellBorder
            })
            productTable.AddCell(New PdfPCell(New Phrase(row("UNIT").ToString(), articleValueFont)) With {
                .HorizontalAlignment = Element.ALIGN_CENTER,
                .Border = Rectangle.BOX, .BorderWidth = cellBorder
            })
            productTable.AddCell(New PdfPCell(New Phrase(row("ARTICLES").ToString(), normalFontBody)) With {
                .Border = Rectangle.BOX, .BorderWidth = cellBorder
            })
            productTable.AddCell(New PdfPCell(New Phrase("₱" & Convert.ToDecimal(row("UNIT PRICE")).ToString("N2"), articleValueFont)) With {
                .HorizontalAlignment = Element.ALIGN_RIGHT,
                .Border = Rectangle.BOX, .BorderWidth = cellBorder
            })
            productTable.AddCell(New PdfPCell(New Phrase("₱" & Convert.ToDecimal(row("AMOUNT")).ToString("N2"), articleValueFont)) With {
                .HorizontalAlignment = Element.ALIGN_RIGHT,
                .Border = Rectangle.BOX, .BorderWidth = cellBorder
            })
            rowCount += 1
        Next

        ' Fill Empty Rows up to 22
        Dim maxRows As Integer = 22
        While rowCount < maxRows
            For i As Integer = 1 To 5
                Dim emptyCell As New PdfPCell(New Phrase(" ", articleValueFont)) With {
                    .FixedHeight = fixedRowHeight,
                    .Border = Rectangle.BOX,
                    .BorderWidth = cellBorder
                }
                productTable.AddCell(emptyCell)
            Next
            rowCount += 1
        End While

        ' Add Article Table to document
        doc.Add(productTable)

        ' --- TOTAL AMOUNT DUE ROW (REVISED TO 5 COLUMNS) ---
        ' Use a new table with 5 columns matching the product table widths
        Dim totalDueTable As New PdfPTable(5)
        totalDueTable.WidthPercentage = 100
        ' Use the same widths as the product table to ensure alignment
        totalDueTable.SetWidths(New Single() {0.8F, 0.8F, 4.1F, 1.7F, 1.6F}) '8

        ' Column 1 & 2: Empty spacers (QTY, UNIT columns)
        totalDueTable.AddCell(New PdfPCell(New Phrase(" ", normalFontBody)) With {
            .Border = Rectangle.BOX, .BorderWidth = cellBorder
        })
        totalDueTable.AddCell(New PdfPCell(New Phrase(" ", normalFontBody)) With {
            .Border = Rectangle.BOX, .BorderWidth = cellBorder
        })

        ' Column 3 (Merged): TOTAL AMOUNT DUE label (Articles + Unit Price columns)
        Dim totalLabelCell As New PdfPCell(New Phrase("TOTAL AMOUNT DUE", articleHeaderFont)) With {
            .Colspan = 2, ' Merges 3rd (ARTICLES) and 4th (UNIT PRICE) column spaces
            .HorizontalAlignment = Element.ALIGN_RIGHT,
            .Border = Rectangle.BOX,
            .BorderWidth = 0.5F,
            .PaddingTop = 4, .PaddingBottom = 4, .PaddingRight = 5
        }
        totalDueTable.AddCell(totalLabelCell)

        ' Column 4 (Actual 5th Column): Amount
        Dim totalAmountCell As New PdfPCell(New Phrase("₱" & grandTotal, boldFontBody)) With {
            .Colspan = 1,
            .HorizontalAlignment = Element.ALIGN_RIGHT,
            .Border = Rectangle.BOX,
            .BorderWidth = 0.5F,
            .PaddingTop = 4, .PaddingBottom = 4,
            .PaddingRight = 5
        }
        totalDueTable.AddCell(totalAmountCell)

        ' Add Total Due Table to document
        doc.Add(totalDueTable)



        ' ---------------------------------------------------
        ' --- SIGNATURE SECTION ---
        ' ---------------------------------------------------
        Dim sigTable As New PdfPTable(2)
        sigTable.WidthPercentage = 100
        sigTable.SpacingBefore = 2
        sigTable.SetWidths(New Single() {5.0F, 5.0F})

        ' Define a constant for guaranteed signature row height
        Const SIG_ROW_HEIGHT As Single = 25.0F

        ' --- Left Column (Prepared/Approved) - 1 Column, 2 Rows (Labels Only) ---
        Dim leftSigTable As New PdfPTable(1)
        leftSigTable.WidthPercentage = 100
        leftSigTable.DefaultCell.Border = Rectangle.NO_BORDER

        ' Row 1: PREPARED BY:
        leftSigTable.AddCell(New PdfPCell(New Phrase("PREPARED BY:", timesRomanRegular)) With {
            .Border = Rectangle.BOTTOM_BORDER, .BorderWidthBottom = 0.5F,
            .PaddingTop = 2, .PaddingBottom = 2, .PaddingLeft = 2, .PaddingRight = 2,
            .MinimumHeight = SIG_ROW_HEIGHT
        })

        ' Row 2: APPROVED BY:
        leftSigTable.AddCell(New PdfPCell(New Phrase("APPROVED BY:", timesRomanRegular)) With {
            .Border = Rectangle.NO_BORDER,
            .PaddingTop = 2, .PaddingBottom = 2, .PaddingLeft = 2, .PaddingRight = 2,
            .MinimumHeight = SIG_ROW_HEIGHT
        })

        ' Outer container cell for the left side
        Dim leftSigCell As New PdfPCell(leftSigTable) With {
            .Border = Rectangle.BOX,
            .BorderWidth = 0.5F
        }
        sigTable.AddCell(leftSigCell)


        ' --- Right Column (Received By) ---
        Dim rightSigTable As New PdfPTable(1)
        rightSigTable.WidthPercentage = 100
        rightSigTable.DefaultCell.Border = Rectangle.NO_BORDER

        ' Row 1: "RECEIVED BY:" Label
        rightSigTable.AddCell(New PdfPCell(New Phrase("RECEIVED BY:", timesRomanRegular)) With {
            .Border = Rectangle.NO_BORDER, .Padding = 2
        })

        ' Row 2: Empty cell for signature (NO border bottom now)
        rightSigTable.AddCell(New PdfPCell(New Phrase(" ", normalFontBody)) With {
            .Border = Rectangle.NO_BORDER,
            .FixedHeight = 15,
            .Padding = 2
        })

        ' Row 3: Signature and Date Row (as a nested table)
        Dim sigDateTable As New PdfPTable(2)
        sigDateTable.WidthPercentage = 100
        sigDateTable.SetWidths(New Single() {7.0F, 3.0F})
        sigDateTable.DefaultCell.Border = Rectangle.NO_BORDER
        sigDateTable.SpacingBefore = 5 ' Space above this row in its container

        sigDateTable.AddCell(New PdfPCell(New Phrase("Signature Over printed Name", normalSubHeader)) With {
            .Border = Rectangle.NO_BORDER,
            .HorizontalAlignment = Element.ALIGN_CENTER,
            .VerticalAlignment = Element.ALIGN_BOTTOM
        })
        sigDateTable.AddCell(New PdfPCell(New Phrase("DATE:", normalSubHeader)) With {
            .Border = Rectangle.NO_BORDER,
            .HorizontalAlignment = Element.ALIGN_LEFT,
            .VerticalAlignment = Element.ALIGN_BOTTOM
        })

        ' Add the nested sigDateTable as the third row
        rightSigTable.AddCell(sigDateTable)

        ' Outer container cell for the right side
        Dim rightSigCell As New PdfPCell(rightSigTable) With {
            .Border = Rectangle.BOX,
            .BorderWidth = 0.5F
        }
        sigTable.AddCell(rightSigCell)


        ' --- NEW SECOND ROW (Spanning both columns) ---
        sigTable.AddCell(New PdfPCell(New Phrase("Received the above goods in good order & condition", footerNoteFont)) With {
            .Colspan = 2, ' Span both columns
            .Border = Rectangle.NO_BORDER, ' No border
            .HorizontalAlignment = Element.ALIGN_RIGHT, ' Right alignment
            .PaddingTop = 4, ' Small gap above the text
            .PaddingBottom = 4,
            .PaddingRight = 5 ' Padding on the right edge
        })

        ' Add the main signature table to the document
        doc.Add(sigTable)



        doc.Close()
    End Sub

End Module