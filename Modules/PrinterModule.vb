Imports System.IO
Imports System.Diagnostics
Imports System.Linq
Imports System.Windows.Forms
Imports System.Drawing.Printing
Imports System.Management

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

    Public Function IsPrinterReadyNow(printerName As String) As Boolean
        If String.IsNullOrWhiteSpace(printerName) Then Return False

        ' First, check if the printer name is known (The simple check)
        Dim settings As New PrinterSettings()
        settings.PrinterName = printerName

        If Not settings.IsValid Then
            ' The printer driver is not installed on the system.
            ' This is the state that worked for you when you removed it manually.
            Return False
        End If

        Try
            ' Second, use WMI to check the physical status (Offline/Error)
            Dim query As New SelectQuery("SELECT * FROM Win32_Printer WHERE Name = '" & printerName.Replace("\", "\\") & "'")
            Using searcher As New ManagementObjectSearcher(query)
                For Each printer As ManagementObject In searcher.Get()

                    ' FIX: Use Convert.ToUInt16 instead of the non-existent ToUShort
                    Dim status As UShort = Convert.ToUInt16(printer("ExtendedPrinterStatus"))

                    ' 7 = Offline, 8 = Error, 12 = Paper Out, 13 = Paper Problem
                    If status = 7 OrElse status = 8 OrElse status = 12 OrElse status = 13 Then
                        Return False ' Printer is reported as offline, error, or out of paper
                    End If

                    ' Check if the "Work Offline" box is checked in Windows settings
                    Dim isOffline As Boolean = Convert.ToBoolean(printer("WorkOffline"))
                    If isOffline Then
                        Return False
                    End If

                    Return True ' Passes both checks
                Next
            End Using

            ' Should not happen if settings.IsValid passed, but as a fallback:
            Return False

        Catch ex As Exception
            Debug.WriteLine($"WMI Error checking printer status: {ex.Message}")
            ' If the WMI check fails for any reason, treat it as "Not Ready"
            Return False
        End Try
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