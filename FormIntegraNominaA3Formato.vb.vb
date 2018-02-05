Public Class FormIntegraNominaA3Formato


    ' excel
    Dim WithEvents ExcelApp As Microsoft.Office.Interop.Excel.Application
    Dim ExcelBook As Microsoft.Office.Interop.Excel.Workbook
    Dim ExcelSheet As Microsoft.Office.Interop.Excel.Worksheet

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try
            If Me.RadioButtonNomina.Checked Then
                TIPONOMINA = "NOMINA"
            ElseIf Me.RadioButtonRemesa.Checked Then
                TIPONOMINA = "REMESA"
                PASOSTRING = Me.TextBoxCtaBanco.Text
                PASOSTRING2 = Me.TextBoxRutaLibroExcel.Text
                If Me.TextBoxCtaBanco.TextLength = 0 Then
                    MsgBox("Falta la Cuenta del Banco ", MsgBoxStyle.Information, "Atención")
                    PASOSTRING = "?"
                End If
                If Me.TextBoxRutaLibroExcel.TextLength = 0 Then
                    MsgBox("Falta la  Ruta del Libro Excel de Nifs ", MsgBoxStyle.Information, "Atención")
                    PASOSTRING2 = "?"
                End If

            Else
                MsgBox("No hay seleccionado Formato")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Close()
        End Try
    End Sub

    Private Sub ButtonCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancelar.Click
        Try
            If Me.RadioButtonNomina.Checked Then
                TIPONOMINA = "NOMINA"
            ElseIf Me.RadioButtonRemesa.Checked Then
                TIPONOMINA = "REMESA"
                PASOSTRING = Me.TextBoxCtaBanco.Text
                PASOSTRING2 = Me.TextBoxRutaLibroExcel.Text
                If Me.TextBoxCtaBanco.TextLength = 0 Then
                    MsgBox("Falta la Cuenta del Banco ", MsgBoxStyle.Information, "Atención")
                    PASOSTRING = "?"
                End If
                If Me.TextBoxRutaLibroExcel.TextLength = 0 Then
                    MsgBox("Falta la Ruta del Libro Excel de Nifs ", MsgBoxStyle.Information, "Atención")
                    PASOSTRING2 = "?"
                End If

            Else
                MsgBox("No hay seleccionado Formato")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Close()
        End Try
    End Sub

    Private Sub FormIntegraNominaA3Formato_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            Me.TextBoxCtaBanco.Enabled = False
            Me.TextBoxRutaLibroExcel.Enabled = False

        Catch ex As Exception

        End Try
    End Sub

    Private Sub RadioButtonRemesa_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonRemesa.CheckedChanged
        Try
            If Me.RadioButtonRemesa.Checked Then
                Me.TextBoxCtaBanco.Enabled = True
                Me.TextBoxRutaLibroExcel.Enabled = True
                Me.TextBoxRutaLibroExcel.Focus()
                Exit Sub
            Else
                Me.TextBoxCtaBanco.Enabled = False
                Me.TextBoxRutaLibroExcel.Enabled = False

            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonRutaExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRutaExcel.Click
        Try
            OpenFileDialog1.CheckPathExists = True
            '  OpenFileDialog1.InitialDirectory = Me.m_ParaFileOrigen

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Posible Ruta No Accesible")
        End Try


        Try
            OpenFileDialog1.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*"
            OpenFileDialog1.FilterIndex = 1

            '  OpenFileDialog1.RestoreDirectory = True

            If OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

                If Me.OpenFileDialog1.FileName.Length > 0 Then
                    Me.TextBoxRutaLibroExcel.Text = Me.OpenFileDialog1.FileName
                    Me.ContarHojasExcel(Me.OpenFileDialog1.FileName)
                End If
            End If



        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub ContarHojasExcel(ByVal vLibro As String)
        Try
            Me.ListBoxLibrosNif.Items.Clear()
            Me.ListBoxLibrosNif.Update()

            If IsNothing(Me.ExcelApp) = True Then
                Me.ExcelApp = New Microsoft.Office.Interop.Excel.Application
            End If

            Me.ExcelApp.IgnoreRemoteRequests = True
            '  Me.ExcelBook = Me.ExcelApp.Workbooks.Open(Me.TextBoxRutaLibroExcel.Text, , True, , , , True, )

            Me.ExcelBook = Me.ExcelApp.Workbooks.Add(vLibro)

            '  Me.ExcelApp.Visible = False
            Dim i As Integer
            Dim P As Integer

            For Each Me.ExcelSheet In Me.ExcelApp.Sheets
                Me.ListBoxLibrosNif.Items.Add(Me.ExcelSheet.Name)
                i = i + 1

                For P = 1 To Me.ExcelSheet.Name.Length

                    If Mid(Me.ExcelSheet.Name, P, 1) = "." Then
                        MsgBox("No use punto(.) en el nombre de la Hoja de Excel a Tratar = " & Me.ExcelSheet.Name, MsgBoxStyle.Exclamation, "Atención")
                    End If

                Next P
            Next Me.ExcelSheet

            '   Me.ExcelApp.Workbooks.Close()

        Catch e As System.Runtime.InteropServices.COMException
            MsgBox("Es posible que esta Computadora no tenga instalado Microsoft Excel " & vbCrLf & e.Message, MsgBoxStyle.Information, " Exepción COM Puede Continuar ...")

        Catch ex As Exception
            MsgBox("Es posible que esta Computadora no tenga instalado Microsoft Excel " & vbCrLf & ex.Message, MsgBoxStyle.Information, "Exepción Genérica Puede Continuar ...")
        End Try

    End Sub

    Private Sub ListBoxLibrosNif_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBoxLibrosNif.SelectedIndexChanged
        Try
            Me.TextBoxRutaNif.Text = Me.ListBoxLibrosNif.SelectedItem
            Me.TextBoxRutaNif.Update()
            PASOSTRING3 = Me.TextBoxRutaNif.Text
        Catch ex As Exception

        End Try
    End Sub
End Class