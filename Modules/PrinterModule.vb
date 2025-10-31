Imports System.IO
Imports System.Diagnostics
Imports System.Linq
Imports System.Windows.Forms

Module PrinterModule

    ' Toggle printing without changing form logic (disabled by default)
    Public PrintEnabled As Boolean = False

    ' Centralize receipts folder path
    Public ReadOnly Property ReceiptsFolder As String
        Get
            Return "C:\POS_Receipts"
        End Get
    End Property

    Public Sub EnsureReceiptsFolderExists()
        If Not Directory.Exists(ReceiptsFolder) Then
            Directory.CreateDirectory(ReceiptsFolder)
        End If
    End Sub

    Public Function IsPrinterInstalled(printerName As String) As Boolean
        If String.IsNullOrWhiteSpace(printerName) Then Return False
        For Each installedPrinter As String In Printing.PrinterSettings.InstalledPrinters
            If installedPrinter.Trim().ToLower() = printerName.Trim().ToLower() Then
                Return True
            End If
        Next
        Return False
    End Function

    ' Print PDF using Foxit (kept available). If PrintEnabled = False the call returns True and does nothing.
    Public Function PrintPDFWithFoxit(filePath As String, printerName As String) As Boolean
        If Not PrintEnabled Then
            ' Printing is intentionally disabled — allow the workflow to continue
            Return True
        End If

        Try
            If Not File.Exists(filePath) Then
                MessageBox.Show("PDF file does not exist: " & filePath, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            Dim foxitPaths As String() = {
                "C:\Program Files\Foxit Software\Foxit PDF Reader\FoxitPDFReader.exe",
                "C:\Program Files (x86)\Foxit Software\Foxit PDF Reader\FoxitPDFReader.exe"
            }

            Dim foxitPath As String = foxitPaths.FirstOrDefault(Function(p) File.Exists(p))
            If String.IsNullOrEmpty(foxitPath) Then
                MessageBox.Show("Foxit PDF Reader not found. Please install Foxit.", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            Dim psi As New ProcessStartInfo(foxitPath) With {
                .Arguments = $"/t ""{filePath}"" ""{printerName}""",
                .UseShellExecute = False,
                .CreateNoWindow = True
            }

            Using proc As Process = Process.Start(psi)
                proc.WaitForExit(10000)
            End Using

            Return True
        Catch ex As Exception
            MessageBox.Show("Failed to print: " & ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

End Module