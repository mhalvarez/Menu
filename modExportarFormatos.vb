Imports System.IO
Imports iTextSharp.texT
Imports iTextSharp.text.pdf
Module modExportarFormatos

    ''Código para exportar DataGridView a PDF usando iTextSharp
    ''Evento clic del Botón Exportar

    Public Sub Exportar_pdf(ByRef FicheroDestino As String, _
                            ByVal dg As DataGridView, _
                            Optional ByVal MostrarDocumentoGenerado As Boolean = True, _
                            Optional ByVal MostrarDirectorio As Boolean = False, _
                            Optional ByRef TamañoFuenteCabeceraRejilla As Single = 9, _
                            Optional ByRef TamañoFuenteRejilla As Single = 8, _
                            Optional ByRef TamañoFuenteDocumento As Single = 9, _
                            Optional ByRef Cabecera As String = "", _
                            Optional ByRef Pie As String = "", _
                            Optional ByVal MostrarInvisibles As Boolean = False)
        Try

            Dim doc As New Document(PageSize.A4.Rotate(), 10, 10, 10, 10)
            Dim filename As String = FicheroDestino
            Dim file As FileStream

            file = New FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite)

            PdfWriter.GetInstance(doc, file)
            doc.Open()

            GenerarDocumento(doc, dg, TamañoFuenteCabeceraRejilla, TamañoFuenteRejilla, TamañoFuenteDocumento, Cabecera, Pie, MostrarInvisibles)
            doc.Close()

            If MostrarDocumentoGenerado Then
                Process.Start(filename)
            Else
                If MostrarDirectorio = True Then
                    Shell("explorer.exe root = " & System.IO.Path.GetDirectoryName(FicheroDestino), AppWinStyle.NormalFocus)
                Else
                    '
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub


    'Función que genera el documento Pdf
    Public Sub GenerarDocumento(ByVal document As Document, _
                                ByVal DataGridView1 As DataGridView, _
                                Optional ByRef TamañoFuenteCabeceraRejilla As Single = 9, _
                                Optional ByRef TamañoFuenteRejilla As Single = 8, _
                                Optional ByRef TamañoFuenteDocumento As Single = 9, _
                                Optional ByRef Cabecera As String = "", _
                                Optional ByRef Pie As String = "", _
                                Optional ByVal MostrarInvisibles As Boolean = False)
        'se crea un objeto PdfTable con el numero de columnas del
        'dataGridView"

        Dim datatable As New PdfPTable(DataGridView1.ColumnCount)

        'Dim bfR As iTextSharp.text.pdf.BaseFont
        'bfR = iTextSharp.text.pdf.BaseFont.CreateFont("verdana.ttf", iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED)

        'Dim cell As New PdfPCell(New Phrase("some text", mainFont))
        'DATATABLE.Add(cell)
        Dim font As New iTextSharp.text.Font
        font = FontFactory.GetFont(FontFactory.HELVETICA, TamañoFuenteDocumento)
        font.SetStyle(font.BOLD)

        document.Add(New Paragraph(Cabecera, font))
        If Cabecera <> "" Then document.Add(New Paragraph(" ", font))

        'document.AddHeader("Cabecera", "Texto de Cabecera")

        'asignamos algunas propiedades para el diseño del pdf
        datatable.DefaultCell.Padding = 3
        'Dim Font As iTextSharp.text.Font
        '
        'ESTILO PARA LA CABECERA
        Dim headerwidths As Single() = GetTamañoColumnas(DataGridView1, MostrarInvisibles)

        datatable.SetWidths(headerwidths)
        datatable.WidthPercentage = 100
        datatable.DefaultCell.BorderWidth = 1
        datatable.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY
        datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER

        font = FontFactory.GetFont(FontFactory.HELVETICA, TamañoFuenteCabeceraRejilla)
        font.SetStyle(font.BOLD)

        'SE GENERA EL ENCABEZADO DE LA TABLA EN EL PDF
        For i As Integer = 0 To DataGridView1.ColumnCount - 1
            Select Case DataGridView1.Columns(i).InheritedStyle.Alignment
                Case DataGridViewContentAlignment.NotSet, DataGridViewContentAlignment.BottomLeft, _
                  DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.TopLeft
                    '
                    datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT

                Case DataGridViewContentAlignment.BottomCenter, DataGridViewContentAlignment.MiddleCenter, _
                  DataGridViewContentAlignment.TopCenter
                    '
                    datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER

                Case DataGridViewContentAlignment.BottomRight, DataGridViewContentAlignment.MiddleRight, _
                  DataGridViewContentAlignment.TopRight
                    '
                    datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT

                Case Else
                    datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT

            End Select

            datatable.AddCell(New Phrase(DataGridView1.Columns(i).HeaderText, font))
        Next
        datatable.HeaderRows = 1
        'ESTILO PARA EL CUERPO
        datatable.DefaultCell.BorderWidth = 1
        datatable.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE
        font = FontFactory.GetFont(FontFactory.HELVETICA, TamañoFuenteRejilla)

        'SE GENERA EL CUERPO DEL PDF
        For i As Integer = 0 To DataGridView1.RowCount - 1
            For j As Integer = 0 To DataGridView1.ColumnCount - 1
                Select Case DataGridView1.Rows(j).Cells(j).InheritedStyle.Alignment
                    Case DataGridViewContentAlignment.NotSet, DataGridViewContentAlignment.BottomLeft, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.TopLeft
                        datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT

                    Case DataGridViewContentAlignment.BottomCenter, DataGridViewContentAlignment.MiddleCenter, DataGridViewContentAlignment.TopCenter
                        datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER

                    Case DataGridViewContentAlignment.BottomRight, DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.TopRight
                        datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT

                    Case Else
                        datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT
                End Select
                '
                datatable.AddCell(New Phrase(DataGridView1(j, i).Value.ToString(), font))
                '
            Next
            datatable.CompleteRow()
        Next
        '
        'document.AddHeader("Cabecera", "Texto de la cabecera. Pag." & document.PageNumber & " de " & " ")

        'SE AGREGAR LA PDFPTABLE AL DOCUMENTO
        document.Add(datatable)
        '
        'If Pie <> "" Then document.Add(New Paragraph(" ", font))
        document.Add(New Paragraph(Pie, font))
        ''
    End Sub

    'Función que obtiene los tamaños de las columnas del grid
    Public Function GetTamañoColumnas(ByVal dg As DataGridView, Optional ByVal MostrarInvisibles As Boolean = False) As Single()

        Dim values As Single() = New Single(dg.ColumnCount - 1) {}

        For i As Integer = 0 To dg.ColumnCount - 1
            If (dg.Columns(i).Visible = True) Or (MostrarInvisibles = True) Then
                values(i) = CSng(dg.Columns(i).Width)

            Else
                values(i) = 0
            End If
        Next
        Return values
    End Function
End Module
