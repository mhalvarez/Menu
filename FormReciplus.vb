
'
'
'
' Pendiente Edelia
' La cuenta de Igic viene de Redciplus asi : 4770000  hablar con Edelia para ponerla en formato 12 digitos



Imports System.IO

Public Class FormReciplus
    Private m_InputFile As System.IO.StreamReader
    Private m_Filegraba As StreamWriter
    Private FileEstaOk As Boolean = False

    Private SQL As String

    Private DbLeeCentral As C_DATOS.C_DatosOledb
    Private DbSpyro As C_DATOS.C_DatosOledb
    Dim MyIni As New cIniArray

    ' 
    Private mDebug As Boolean = False
    Private mStrConexionHotel As String
    Private mStrConexionCentral As String
    Private mStrConexionSpyro As String

    Private mFecha As Date
    Private mEmpGrupoCod As String
    Private mEmpCod As String
    Private mEmpNum As Integer

    Private mIndicadorDebe As String
    Private mIndicadorHaber As String

    Private mIndicadorDebeFac As String
    Private mIndicadorHaberFac As String

    Private mTipoAsiento As String
    Private mDebe As Double
    Private mHbaber As Double
    Private mTextDebug As System.Windows.Forms.TextBox
    Private mListBoxDebug As System.Windows.Forms.ListBox
    Private mProgress As System.Windows.Forms.ProgressBar

    Private mForm As System.Windows.Forms.Form
    Private mTrataDebitoTpvnoFacturado As Boolean = False

    Private mParaFilePath As String
    Private mParaFechaRegistroAc As String
    Private mParaSerieAnulacion As String
    Private mParaCentroCostoAlojamiento As String
    Private mParaToOem As Boolean = False
    Private mParaComisiones As Boolean = False
    Private mParaSantana As Integer
    Private mParaValidaSpyro As Integer
    Private mParaTextoIva As String
    Private mParaTipoAnulacion As Integer

    Private mParaUsaCtaComision As Integer



    Private mUsaTnhtMoviAuto As Boolean






    Private mCtaManoCorriente As String
    ' Private mCtaIngresosAnticipados As String
    Private mCtaEfectivo As String
    Private mCtaPagosACuenta As String
    Private mCtaDesembolsos As String
    Private mCtaIgic As String
    Private mCtaRedondeo As String
    Private mCta56DigitoCuentaClientes As String
    Private mCfivaLibro_Cod As String
    Private mCfivaClase_Cod As String
    Private mMonedas_Cod As String
    Private mCfatodiari_Cod As String
    Private mCfatodiari_Cod_2 As String


    Private mCfivatimpu_Cod As String
    Private mCfivatip_Cod As String
    Private mCfatotip_Cod As String
    Private mGvagente_Cod As String
    Private mCtaClientesContado As String
    Private mClientesContadoCif As String


    ' Valores de retorno para debug
    Private mLiquidoServiciosConIgic As Double
    Private mLiquidoServiciosSinIgic As Double
    Private mLiquidoDesembolsos As Double
    Private mCancelacionAnticipos As Double
    Private mDevolucionAnticipos As Double

    Private mTotalProduccion As Double
    Private mTotalFacturacion As Double


    ' cuentas para contabilizar por series de factura 

    Private mCtaFacturasEmitidas As String
    Private mCtaFacturasAnuladas As String
    Private mCtaNotasDeCredito As String

    ' 
    Private mDesglosaAlojamientoporRegimen As Boolean
    Private mParaServicioAlojamiento As String

    Private mSerruchaDepartamentos As Boolean
    Private mMuestraIncidencias As Boolean = True

    Private mCtaSerieCredito As String
    Private mCtaSerieContado As String
    Private mCtaSerieNotaCredito As String

    ' ASUNTO DEPOSITOS 

    Private mParaUsaCta4b As Boolean
    Private mParaCta4b As String
    Private mParaCta4b2Efectivo As String
    Private mParaCta4b3Visa As String
    Private mParaSecc_DepNh As String

    Dim Multidiario As Boolean = False

    Private Sub FormReciplus_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If IsNothing(DbLeeCentral) = False Then
            Me.DbLeeCentral.CerrarConexion()
        End If
        If IsNothing(DbSpyro) = False Then
            Me.DbSpyro.CerrarConexion()
        End If

    End Sub



    Private Sub FormReciplus_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.DateTimePicker1.Value = (CType(Format(Now, "dd/MM/yyyy"), Date))

            Me.mStrConexionCentral = STRCONEXIONCENTRAL

            Me.mEmpGrupoCod = MyIni.IniGet(Application.StartupPath & "\menu.ini", "PARAMETER", "PARA_EMPGRUPO_COD")


            Me.Cursor = Cursors.WaitCursor
            Me.AbreConexiones()
            Me.CargaParametros()

            If Me.DbLeeCentral.EstadoConexion = ConnectionState.Open And Me.DbSpyro.EstadoConexion = ConnectionState.Open Then
                Me.Text += " Conexiones Ok "
            Else
                MsgBox("Alguna de las Conexiones a la Base de Datos NO esta Disponible", MsgBoxStyle.Exclamation, "Atención")
                Me.Close()
            End If


        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub ButtonFicheroEntrada_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFicheroEntrada.Click
        Try
            OpenFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            OpenFileDialog1.FilterIndex = 1

            '  OpenFileDialog1.RestoreDirectory = True

            If OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

                If Me.OpenFileDialog1.FileName.Length > 0 Then
                    Me.TextBoxFicheroEntrada.Text = Me.OpenFileDialog1.FileName
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonFicheroSalida_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFicheroSalida.Click
        Try
            Me.FolderBrowserDialog1.ShowDialog()
            If IsNothing(Me.FolderBrowserDialog1.SelectedPath) = False Then
                If Me.FolderBrowserDialog1.SelectedPath.Length > 0 Then

                    If Mid(FolderBrowserDialog1.SelectedPath, FolderBrowserDialog1.SelectedPath.Length, 1) <> "\" Then
                        Me.TextBoxFicheroSalida.Text = Me.FolderBrowserDialog1.SelectedPath & "\"
                    Else
                        Me.TextBoxFicheroSalida.Text = Me.FolderBrowserDialog1.SelectedPath
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try
            If Me.TextBoxSerie.Text = "" Then
                MsgBox("Serie de Facturas NO puede ser NULO", MsgBoxStyle.Exclamation, "Atención")
                Exit Sub
            ElseIf Me.TextBoxSerieNotasdeCredito.Text = "" Then
                MsgBox("Serie de Abonos NO puede ser NULO", MsgBoxStyle.Exclamation, "Atención")
                Exit Sub
            ElseIf Me.TextBoxEmpresa.Text = "" Then
                MsgBox("Empresa NO puede ser NULO", MsgBoxStyle.Exclamation, "Atención")
                Exit Sub
            End If


            ' VALIDA EMPRESA
            SQL = "SELECT NVL(COUNT(*),0) AS TOTAL  FROM EMP "
            SQL += " WHERE EMPGRUPO_COD = '" & Me.TextBoxGrupoEmpresa.Text & "'"
            SQL += "  AND COD = '" & Me.TextBoxEmpresa.Text & "'"

            If Me.DbSpyro.EjecutaSqlScalar(SQL) = "0" Then
                MsgBox("No se Localiza empresa " & Me.TextBoxEmpresa.Text, MsgBoxStyle.Exclamation, "Atención")
                Exit Sub
            End If



            ' VALIDA las serie de Facturas 
            SQL = "SELECT NVL(COUNT(*),0) AS TOTAL  FROM FACTUTIPO "
            SQL += " WHERE CFIVALIBRO_COD = '" & Me.TextBoxLibroIva.Text & "'"
            SQL += "  AND COD = '" & Me.TextBoxSerie.Text & "'"

            If Me.DbSpyro.EjecutaSqlScalar(SQL) = "0" Then
                MsgBox("No se Localiza la serie de Documentos " & Me.TextBoxSerie.Text, MsgBoxStyle.Exclamation, "Atención")
                Exit Sub
            End If

            ' VALIDA las serie de NOTAS D ABONO
            SQL = "SELECT NVL(COUNT(*),0) AS TOTAL  FROM FACTUTIPO "
            SQL += " WHERE CFIVALIBRO_COD = '" & Me.TextBoxLibroIva.Text & "'"
            SQL += "  AND COD = '" & Me.TextBoxSerieNotasdeCredito.Text & "'"

            If Me.DbSpyro.EjecutaSqlScalar(SQL) = "0" Then
                MsgBox("No se Localiza la serie de Documentos " & Me.TextBoxSerieNotasdeCredito.Text, MsgBoxStyle.Exclamation, "Atención")
                Exit Sub
            End If


            Me.CrearFichero(Me.TextBoxFicheroSalida.Text)
            Me.TextBoxDebug.Text = ""

            If FileEstaOk = True Then
                Me.FileRead()
            End If
            ' cerra
            If FileEstaOk Then
                Me.m_Filegraba.Close()
            End If

            Me.TextBoxDebug.Text = Me.TextBoxFicheroSalida.Text & "RECIPLUS.TXT"
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub FileRead()
        Try

            Dim Linea As String
            ' Dim LineaArray() As String
            Dim LineaArray(50) As String
            Me.ListBoxInput.Items.Clear()
            Me.ListBoxInput.Update()

            Me.ListBoxOutPut.Items.Clear()
            Me.ListBoxOutPut.Update()

            Me.ListBoxSpyro.Items.Clear()
            Me.ListBoxSpyro.Update()

            Dim Serie As String

            Dim Nif As String

            If File.Exists(Me.TextBoxFicheroEntrada.Text) = False Then
                Exit Sub
            End If


            Me.mEmpCod = Me.TextBoxEmpresa.Text

            Me.m_InputFile = New System.IO.StreamReader(Me.TextBoxFicheroEntrada.Text, System.Text.Encoding.Default)

            Do While Me.m_InputFile.Peek() >= 0
                Linea = Me.m_InputFile.ReadLine
                Me.ListBoxInput.Items.Add(Linea)
                '  ReDim LineaArray(UBound(Split(Linea, vbTab)))

                '  LineaArray = Split(Linea, vbTab)

                LineaArray(0) = Mid(Linea, 1, 6)
                LineaArray(1) = Mid(Linea, 7, 8)
                LineaArray(2) = Mid(Linea, 15, 12)
                LineaArray(3) = Mid(Linea, 27, 12)
                LineaArray(4) = Mid(Linea, 39, 16)
                LineaArray(5) = Mid(Linea, 55, 25)

                LineaArray(6) = Mid(Linea, 80, 16)
                LineaArray(7) = Mid(Linea, 96, 8)
                LineaArray(8) = Mid(Linea, 104, 16)
                LineaArray(9) = Mid(Linea, 120, 5)
                LineaArray(10) = Mid(Linea, 125, 5)
                LineaArray(11) = Mid(Linea, 130, 10)
                LineaArray(12) = Mid(Linea, 140, 3)
                LineaArray(13) = Mid(Linea, 143, 6)
                LineaArray(14) = Mid(Linea, 149, 1)
                LineaArray(15) = Mid(Linea, 150, 6)
                LineaArray(16) = Mid(Linea, 156, 1)
                LineaArray(17) = Mid(Linea, 157, 6)
                LineaArray(18) = Mid(Linea, 163, 16)
                LineaArray(19) = Mid(Linea, 179, 16)
                LineaArray(20) = Mid(Linea, 195, 16)
                LineaArray(21) = Mid(Linea, 211, 1)
                LineaArray(22) = Mid(Linea, 212, 1)
                LineaArray(23) = Mid(Linea, 213, 4)
                LineaArray(24) = Mid(Linea, 217, 5)
                LineaArray(25) = Mid(Linea, 222, 16)
                LineaArray(26) = Mid(Linea, 238, 1)
                LineaArray(27) = Mid(Linea, 239, 16)
                LineaArray(28) = Mid(Linea, 255, 16)
                LineaArray(29) = Mid(Linea, 271, 16)
                LineaArray(30) = Mid(Linea, 287, 1)


                If CStr(LineaArray(4)).Trim <> "0.00" Then
                    ' ES UNA FACTURA 
                    If CType(LineaArray(27), Integer) < 0 Then
                        Serie = Me.TextBoxSerieNotasdeCredito.Text
                    Else
                        Serie = Me.TextBoxSerie.Text
                    End If


                    ' VALIDA que no exista ya la factura en Spyro
                    SQL = "SELECT NVL(COUNT(*),0) AS TOTAL  FROM FACTURAS "
                    SQL += " WHERE EMP_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"

                    SQL += "  AND EMP_COD = '" & Me.mEmpCod & "'"
                    SQL += "  AND CFIVALIBRO_COD = '" & Me.TextBoxLibroIva.Text & "'"
                    SQL += "  AND FACTUTIPO_COD = '" & Serie & "'"
                    SQL += "  AND N_FACTURA  = " & CType(LineaArray(7), Integer)

                    If Me.DbSpyro.EjecutaSqlScalar(SQL) <> "0" Then
                        MsgBox("Documento ya Contabilizado !!!!!  " & CType(LineaArray(7), String) & "/" & Me.TextBoxSerie.Text, MsgBoxStyle.Exclamation, "Atención")
                        Me.ListBoxSpyro.Items.Add("SPYRO   : " & " Documento ya Contabilizado !!!!!  " & CType(LineaArray(7), String) & "/" & Me.TextBoxSerie.Text)
                        Me.ListBoxSpyro.Update()
                        Exit Sub
                    End If


                    ' se busca el nif en spyro a traves de la cuenta 
                    SQL = "SELECT NVL(RSOCIAL_COD,'?') FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                    SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
                    SQL += "  AND COD = '" & LineaArray(2) & "'"

                    Nif = Me.DbSpyro.EjecutaSqlScalar2(SQL)

                    If Nif = "?" Then
                        MsgBox("No se localiza Nif en Spyro a partir de la cuenta : " & LineaArray(2))
                        Me.ListBoxSpyro.Items.Add("SPYRO   : " & CStr(LineaArray(2).PadRight(12, " ") & "  No se localiza Nif en Spyro a partir de la cuenta"))
                        Me.ListBoxSpyro.Update()
                    End If


                    Me.GeneraFileFV("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, Serie, CType(LineaArray(7), Integer), LineaArray(27), CType(LineaArray(7), String).PadRight(15, CChar(" ")), LineaArray(2), Nif, LineaArray(27))
                    ' 2012
                    Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CStr(LineaArray(2)), Me.mIndicadorDebeFac, LineaArray(5).Replace("?", "u"), LineaArray(27), Serie, CType(LineaArray(7), Integer))

                    '

                    Me.ListBoxOutPut.Items.Add("Factura = " & LineaArray(2) & " " & LineaArray(5) & " " & LineaArray(27))
                    Me.SpyroCompruebaCuenta(LineaArray(2), "FV", 1, 1, "", LineaArray(5), "NOMBRE", "", "")
                End If
                If CStr(LineaArray(3)).Trim = "" And CStr(LineaArray(4)).Trim = "0.00" Then
                    ' ES VENTA 
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), LineaArray(2), Me.mIndicadorHaber, LineaArray(5).Replace("?", "u"), LineaArray(28))
                    Me.ListBoxOutPut.Items.Add("Venta =   " & LineaArray(2) & " " & LineaArray(5) & " " & LineaArray(28))
                    Me.SpyroCompruebaCuenta(LineaArray(2), "FV", 1, 1, "", LineaArray(5), "NOMBRE", "", "")
                End If
                If CStr(LineaArray(3)).Trim.Length > 1 And CStr(LineaArray(4)).Trim = "0.00" Then
                    ' ES IGIC
                    If CType(LineaArray(28), Integer) < 0 Then
                        Serie = Me.TextBoxSerieNotasdeCredito.Text
                    Else
                        Serie = Me.TextBoxSerie.Text
                    End If

                    Me.GeneraFileIV("IV", 3, Me.mEmpGrupoCod, Me.mEmpCod, Serie, CType(LineaArray(7), Integer), LineaArray(29), LineaArray(9), LineaArray(28), Me.mCfivatip_Cod)
                    ' 2012
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), LineaArray(2), Me.mIndicadorHaber, LineaArray(5).Replace("?", "u"), LineaArray(28))

                    '

                    Me.ListBoxOutPut.Items.Add("Igic =    " & LineaArray(2) & " " & LineaArray(5) & " " & LineaArray(28))
                    Me.SpyroCompruebaCuenta(LineaArray(2), "FV", 1, 1, "", LineaArray(5), "NOMBRE", "", "")
                End If

            Loop
            Me.m_InputFile.Close()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub GeneraFileFV(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, _
   ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String, ByVal vPendiente As Double)

        Try


            '-------------------------------------------------------------------------------------------------
            '  Facturas(FACTURAS)
            '-------------------------------------------------------------------------------------------------
            ' MsgBox(vSfactura)
            Me.m_Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) & _
            vEmpGrupoCod.PadRight(4, CChar(" ")) & _
            Me.TextBoxEmpresa.Text.PadRight(4, CChar(" ")) & _
            Me.mCfivaLibro_Cod.PadRight(2, CChar(" ")) & _
            vSerie.PadRight(6, CChar(" ")) & _
            CType(vNfactura, String).PadLeft(8, CChar(" ")) & _
            " ".PadRight(8, CChar(" ")) & _
            Format(Me.DateTimePicker1.Value, "ddMMyyyy") & _
            Me.mCfivaClase_Cod.PadRight(2, CChar(" ")) & _
            CType(vImonep, String).PadLeft(16, CChar(" ")) & _
            Me.mMonedas_Cod.PadRight(4, CChar(" ")) & _
            CType(vImonep, String).PadLeft(16, CChar(" ")) & _
            Mid(vSfactura, 1, 15).PadRight(15, CChar("-")) & _
            vEmpGrupoCod.PadRight(4, CChar(" ")) & _
             Me.TextBoxEmpresa.Text.PadRight(4, CChar(" ")) & _
            Format(Me.DateTimePicker1.Value, "yyyy") & _
            Me.mCfatodiari_Cod.PadRight(4, CChar(" ")) & _
            " ".PadLeft(8, CChar(" ")) & _
            " ".PadLeft(4, CChar(" ")) & _
            vCuenta.PadRight(15, CChar(" ")) & _
            vCif.PadRight(20, CChar(" ")) & _
            " ".PadRight(6, CChar(" ")) & _
            " ".PadRight(1, CChar(" ")) & _
            " ".PadRight(8, CChar(" ")) & _
            " ".PadRight(8, CChar(" ")) & _
            Me.mGvagente_Cod.PadRight(8, CChar(" ")) & _
            CType(vPendiente, String).PadRight(16, CChar(" ")) & _
            CType(vPendiente, String).PadRight(16, CChar(" ")) & "NN")



        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileFV")
        End Try
    End Sub
    Private Sub GeneraFileAC(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
    ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double)
        Try
            Dim FechaAsiento As String
            If Me.mParaFechaRegistroAc = "V" Then
                FechaAsiento = Format(Me.DateTimePicker1.Value, "ddMMyyyy")
            ElseIf Me.mParaFechaRegistroAc = "R" Then
                FechaAsiento = Format(Now, "ddMMyyyy")
            Else
                FechaAsiento = Format(Me.DateTimePicker1.Value, "ddMMyyyy")
            End If



            '-------------------------------------------------------------------------------------------------
            '  Apuntes Contables(CFATOLIN)
            '-------------------------------------------------------------------------------------------------
            Me.m_Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) & _
            vEmpGrupoCod.PadRight(4, CChar(" ")) & _
            Me.TextBoxEmpresa.Text.PadRight(4, CChar(" ")) & _
            Mid(FechaAsiento, 5, 4) & _
            Me.mCfatodiari_Cod.PadRight(4, CChar(" ")) & _
            " ".PadLeft(8, CChar(" ")) & _
            " ".PadLeft(4, CChar(" ")) & _
            vCfcta_Cod.PadRight(15, CChar(" ")) & _
            vCfcptos_Cod.PadRight(4, CChar(" ")) & _
            Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & _
            CType(vImonep, String).PadLeft(16, CChar(" ")) & _
            "N" & FechaAsiento & _
            Format(Me.DateTimePicker1.Value, "ddMMyyyy") & _
            " ".PadRight(40, CChar(" ")) & _
            Me.mCfatotip_Cod.PadRight(4, CChar(" ")))


        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
        End Try
    End Sub
    Private Sub GeneraFileAC2(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, ByVal vFactuTipo_cod As String, ByVal vNfactura As Integer)
        Try
            Dim FechaAsiento As String
            If Me.mParaFechaRegistroAc = "V" Then
                FechaAsiento = Format(Me.DateTimePicker1.Value, "ddMMyyyy")
            ElseIf Me.mParaFechaRegistroAc = "R" Then
                FechaAsiento = Format(Now, "ddMMyyyy")
            Else
                FechaAsiento = Format(Me.DateTimePicker1.Value, "ddMMyyyy")
            End If



            '-------------------------------------------------------------------------------------------------
            '  Apuntes Contables(CFATOLIN)
            '-------------------------------------------------------------------------------------------------
            Me.m_Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) & _
            vEmpGrupoCod.PadRight(4, CChar(" ")) & _
            vEmpCod.PadRight(4, CChar(" ")) & _
            Mid(FechaAsiento, 5, 4) & _
            Me.mCfatodiari_Cod.PadRight(4, CChar(" ")) & _
            " ".PadLeft(8, CChar(" ")) & _
            " ".PadLeft(4, CChar(" ")) & _
            vCfcta_Cod.PadRight(15, CChar(" ")) & _
            vCfcptos_Cod.PadRight(4, CChar(" ")) & _
            Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & _
            CType(vImonep, String).PadLeft(16, CChar(" ")) & _
            "N" & FechaAsiento & _
             Format(Me.DateTimePicker1.Value, "ddMMyyyy") & _
            " ".PadRight(40, CChar(" ")) & _
            Me.mCfatotip_Cod.PadRight(4, CChar(" ")) & _
            mCfivaLibro_Cod.PadRight(2, CChar(" ")) & _
            vFactuTipo_cod.PadRight(6, CChar(" ")) & _
            CType(vNfactura, String).PadRight(8, CChar(" ")))



        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc2")
        End Try
    End Sub
    Private Sub GeneraFileIV(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vFactutipo_cod As String, _
   ByVal vNfactura As Integer, ByVal vI_basmonemp As Double, ByVal vPj_iva As Double, ByVal vI_ivamonemp As Double, ByVal vX As String)


        Try


            '-------------------------------------------------------------------------------------------------
            '  Libro de Iva(CFIVALIN)
            '-------------------------------------------------------------------------------------------------
            Me.m_Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
             Me.TextBoxEmpresa.Text.PadRight(4, CChar(" ")) &
            Me.mCfivaLibro_Cod.PadRight(2, CChar(" ")) &
            vFactutipo_cod.PadRight(6, CChar(" ")) &
            CType(vNfactura, String).PadRight(8, CChar(" ")) &
            Me.mCfivatimpu_Cod.PadRight(4, CChar(" ")) &
            vX.PadRight(2, CChar(" ")) &
            CType(vI_basmonemp, String).PadRight(16, CChar(" ")) &
            CType(vPj_iva, String).PadRight(10, CChar(" ")) &
            CType(vI_ivamonemp, String).PadRight(16, CChar(" ")) &
            CType(vI_basmonemp, String).PadRight(16, CChar(" ")) &
            CType(vI_ivamonemp, String).PadRight(16, CChar(" ")))


        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileIV")
        End Try
    End Sub
    Private Sub CrearFichero(ByVal vFile As String)

        Try
            'Filegraba = New StreamWriter(vFile, False, System.Text.Encoding.UTF8)
            Me.m_Filegraba = New StreamWriter(vFile & "RECIPLUS.TXT", False, System.Text.Encoding.ASCII)

            Me.m_Filegraba.WriteLine("")
            FileEstaOk = True
        Catch ex As Exception
            FileEstaOk = False
            MsgBox("No dispone de acceso al Fichero " & vFile, MsgBoxStyle.Information, "Atención")
        End Try
    End Sub

    Private Sub CargaParametros()
        Try

            SQL = "SELECT NVL(PARA_CTA1,'0') PARA_CTA1, "
            SQL += "NVL(PARA_CTA3,'0') PARA_CTA3, "
            SQL += "NVL(PARA_CTA4,'0') PARA_CTA4, "
            SQL += "NVL(PARA_CTA5,'0') PARA_CTA5, "
            SQL += "NVL(PARA_CFIVALIBRO_COD,'?') LIBROIVA,"
            SQL += "NVL(PARA_CFIVACLASE_COD,'?') CLASEIVA,"
            SQL += "NVL(PARA_MONEDAS_COD,'?') MONEDA,"
            SQL += "NVL(PARA_CFATODIARI_COD,'?') DIARIO,"
            SQL += "NVL(PARA_CFATODIARI_COD_2,'?') DIARIO2,"
            SQL += "NVL(PARA_CFIVATIMPU_COD,'?') TIPOIMPUESTO,"
            SQL += "NVL(PARA_CFIVATIP_COD,'?') TIPOIVA,"
            SQL += "NVL(PARA_CFATOTIP_COD,'?') TIPOASIENTO,"
            SQL += "NVL(PARA_GVAGENTE_COD,'0') AGENTE,"
            SQL += "NVL(PARA_DEBE,'?') DEBE,"
            SQL += "NVL(PARA_HABER,'?') HABER,"
            SQL += "NVL(PARA_DEBE_FAC,'?') DEBEFAC,"
            SQL += "NVL(PARA_HABER_FAC,'?') HABERFAC,"
            SQL += "NVL(PARA_CLIENTES_CONTADO,'?') CLIENTESCONTADO,"
            SQL += "NVL(PARA_CLIENTES_CONTADO_CIF,'?') CLIENTESCONTADOCIF,"
            SQL += "NVL(PARA_FILE_SPYRO_PATH,'?') FILEPATH,"
            SQL += "NVL(PARA_CTA_REDONDEO,'0') REDONDEO,"
            SQL += "NVL(PARA_FECHA_REGISTRO_AC,'?') FECHAAC,"
            SQL += "NVL(PARA_SERIE_ANULACION,'?') SERIEANULACION,"
            SQL += "NVL(PARA_CENTRO_COSTO_AL,'?') CCOSTOAL,"
            SQL += "NVL(PARA_CHARTOOEM,'0') TOOEM,"
            SQL += "NVL(PARA_COMISIONES,'0') COMISIONES,"
            SQL += "NVL(PARA_SANTANACAZORLA,'0') SANTANA,"
            SQL += "NVL(PARA_VALIDA_SPYRO,'0') VALIDASPYRO,"
            SQL += "NVL(PARA_TEXTO_IVA,'0') TEXTOIVA,"
            SQL += "NVL(PARA_TIPO_ANULACION,'0') TIPOANULACION,"
            SQL += "NVL(PARA_USA_CTACOMISION,'0') USACTACOMISION,"

            SQL += "NVL(PARA_CTA_SERIE_FAC,'0') CTAFACTURAS,"
            SQL += "NVL(PARA_CTA_SERIE_ANUL,'0') CTAFACTURASANULADAS,"
            SQL += "NVL(PARA_CTA_SERIE_NOTAS,'0') CTANOTAS,"
            SQL += "NVL(PARA_DESGLO_ALOJA_REGIMEN,'0') PARA_DESGLO_ALOJA_REGIMEN,"
            SQL += "NVL(PARA_INGRESO_HABITACION_DPTO,'?') PARA_INGRESO_HABITACION_DPTO,"
            SQL += "NVL(PARA_SERRUCHA_DPTO,'0') PARA_SERRUCHA_DPTO,"

            SQL += "NVL(PARA_CTA_SERIE_CRE,'0') PARA_CTA_SERIE_CRE,"
            SQL += "NVL(PARA_CTA_SERIE_CON,'0') PARA_CTA_SERIE_CON,"
            SQL += "NVL(PARA_CTA_SERIE_NCRE,'0') PARA_CTA_SERIE_NCRE,"

            SQL += "NVL(PARA_CTA_56DIGITO,'0') PARA_CTA_56DIGITO,"

            SQL += "NVL(PARA_USA_CTA4B,'0') AS PARA_USA_CTA4B, "
            SQL += "NVL(PARA_CTA4B,'<Ninguno>') AS PARA_CTA4B,  "
            SQL += "NVL(PARA_CTA4B2,'<Ninguno>') AS PARA_CTA4B2,  "
            SQL += "NVL(PARA_CTA4B3,'<Ninguno>') AS PARA_CTA4B3,  "
            SQL += "NVL(PARA_SECC_DEPNH,'<Ninguno>') AS PARA_SECC_DEPNH  "





            SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.TextBoxEmpresaParametros.Text & "'"
            ' SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum

            Me.DbLeeCentral.TraerLector(SQL)
            If Me.DbLeeCentral.mDbLector.Read Then
                Me.mCtaManoCorriente = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA1"), String)
                Me.mCtaEfectivo = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA3"), String)
                Me.mCtaPagosACuenta = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA4"), String)
                Me.mCtaDesembolsos = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA5"), String)
                Me.mCfivaLibro_Cod = CType(Me.DbLeeCentral.mDbLector.Item("LIBROIVA"), String)
                Me.mCfivaClase_Cod = CType(Me.DbLeeCentral.mDbLector.Item("CLASEIVA"), String)
                Me.mMonedas_Cod = CType(Me.DbLeeCentral.mDbLector.Item("MONEDA"), String)
                Me.mCfatodiari_Cod = CType(Me.DbLeeCentral.mDbLector.Item("DIARIO"), String)
                Me.mCfatodiari_Cod_2 = CType(Me.DbLeeCentral.mDbLector.Item("DIARIO2"), String)
                Me.mCfivatimpu_Cod = CType(Me.DbLeeCentral.mDbLector.Item("TIPOIMPUESTO"), String)
                Me.mCfivatip_Cod = CType(Me.DbLeeCentral.mDbLector.Item("TIPOIVA"), String)
                Me.mCfatotip_Cod = CType(Me.DbLeeCentral.mDbLector.Item("TIPOASIENTO"), String)
                Me.mGvagente_Cod = CType(Me.DbLeeCentral.mDbLector.Item("AGENTE"), String)
                Me.mIndicadorDebe = CType(Me.DbLeeCentral.mDbLector.Item("DEBE"), String)
                Me.mIndicadorHaber = CType(Me.DbLeeCentral.mDbLector.Item("HABER"), String)
                Me.mIndicadorDebeFac = CType(Me.DbLeeCentral.mDbLector.Item("DEBEFAC"), String)
                Me.mIndicadorHaberFac = CType(Me.DbLeeCentral.mDbLector.Item("HABERFAC"), String)
                Me.mCtaClientesContado = CType(Me.DbLeeCentral.mDbLector.Item("CLIENTESCONTADO"), String)
                Me.mClientesContadoCif = CType(Me.DbLeeCentral.mDbLector.Item("CLIENTESCONTADOCIF"), String)
                Me.mParaFilePath = CType(Me.DbLeeCentral.mDbLector.Item("FILEPATH"), String)
                Me.mCtaRedondeo = CType(Me.DbLeeCentral.mDbLector.Item("REDONDEO"), String)
                Me.mParaFechaRegistroAc = CType(Me.DbLeeCentral.mDbLector.Item("FECHAAC"), String)
                Me.mParaSerieAnulacion = CType(Me.DbLeeCentral.mDbLector.Item("SERIEANULACION"), String)
                Me.mParaCentroCostoAlojamiento = CType(Me.DbLeeCentral.mDbLector.Item("CCOSTOAL"), String)
                Me.mParaToOem = CType(Me.DbLeeCentral.mDbLector.Item("TOOEM"), Boolean)
                Me.mParaComisiones = CType(Me.DbLeeCentral.mDbLector.Item("COMISIONES"), Boolean)
                Me.mParaSantana = CType(Me.DbLeeCentral.mDbLector.Item("SANTANA"), Integer)
                Me.mParaValidaSpyro = CType(Me.DbLeeCentral.mDbLector.Item("VALIDASPYRO"), Integer)
                Me.mParaTextoIva = CType(Me.DbLeeCentral.mDbLector.Item("TEXTOIVA"), String)
                Me.mParaTipoAnulacion = CType(Me.DbLeeCentral.mDbLector.Item("TIPOANULACION"), Integer)
                Me.mParaUsaCtaComision = CType(Me.DbLeeCentral.mDbLector.Item("USACTACOMISION"), Integer)

                Me.mCtaFacturasEmitidas = CType(Me.DbLeeCentral.mDbLector.Item("CTAFACTURAS"), String)
                Me.mCtaFacturasAnuladas = CType(Me.DbLeeCentral.mDbLector.Item("CTAFACTURASANULADAS"), String)
                Me.mCtaNotasDeCredito = CType(Me.DbLeeCentral.mDbLector.Item("CTANOTAS"), String)

                Me.mDesglosaAlojamientoporRegimen = CType(Me.DbLeeCentral.mDbLector.Item("PARA_DESGLO_ALOJA_REGIMEN"), Boolean)
                Me.mSerruchaDepartamentos = CType(Me.DbLeeCentral.mDbLector.Item("PARA_SERRUCHA_DPTO"), Boolean)

                Me.mParaServicioAlojamiento = CType(Me.DbLeeCentral.mDbLector.Item("PARA_INGRESO_HABITACION_DPTO"), String)

                Me.mCtaSerieCredito = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA_SERIE_CRE"), String)
                Me.mCtaSerieContado = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA_SERIE_CON"), String)
                Me.mCtaSerieNotaCredito = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA_SERIE_NCRE"), String)

                Me.mCta56DigitoCuentaClientes = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA_56DIGITO"), String)



                If CType(Me.DbLeeCentral.mDbLector.Item("PARA_USA_CTA4B"), String) = "1" Then
                    Me.mParaUsaCta4b = True
                Else
                    Me.mParaUsaCta4b = False
                End If

                Me.mParaCta4b = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA4B"), String)
                Me.mParaCta4b2Efectivo = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA4B2"), String)
                Me.mParaCta4b3Visa = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA4B3"), String)
                Me.mParaSecc_DepNh = CType(Me.DbLeeCentral.mDbLector.Item("PARA_SECC_DEPNH"), String)


                Me.TextBoxGrupoEmpresa.Text = EMPGRUPO_COD
                Me.TextBoxLibroIva.Text = Me.mCfivaLibro_Cod
                Me.TextBoxClaseIva.Text = Me.mCfivaClase_Cod
                Me.TextBoxMoneda.Text = Me.mMonedas_Cod
                Me.TextBoxDiario.Text = Me.mCfatodiari_Cod


                Me.TextBoxTipoIva1.Text = Me.mCfivatimpu_Cod
                Me.TextBoxTipoIva2.Text = Me.mCfivatip_Cod



            Else
                Me.TextBoxGrupoEmpresa.Text = ""
                Me.TextBoxLibroIva.Text = ""
                Me.TextBoxClaseIva.Text = ""
                Me.TextBoxMoneda.Text = ""
                Me.TextBoxDiario.Text = ""

            End If

            ' LEE DE LA TABLA DE HOTELES
            SQL = "SELECT HOTEL_SPYRO  FROM TH_HOTEL,TH_PARA "
            SQL += " WHERE HOTEL_EMPGRUPO_COD = PARA_EMPGRUPO_COD"
            SQL += " AND   HOTEL_EMP_COD = PARA_EMP_COD"
            SQL += " AND   HOTEL_EMP_NUM = PARA_EMP_NUM"

            SQL += " AND  HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.TextBoxEmpresaParametros.Text & "'"


            Me.mStrConexionSpyro = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

            If Me.mStrConexionSpyro <> "" Then
                Me.DbSpyro = New C_DATOS.C_DatosOledb(Me.mStrConexionSpyro)
                Me.DbSpyro.AbrirConexion()
            Else
                MsgBox("No Hay Cadena de Conexión para Spyro", MsgBoxStyle.Exclamation, "Atención")
            End If


          

            If Me.mCfatodiari_Cod_2 = "?" Then
                Me.Multidiario = False
            Else
                Multidiario = True
            End If

            Me.DbLeeCentral.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Exclamation, "Carga de Parámetros en Constructor de la Clase")
        End Try
    End Sub

    Private Sub AbreConexiones()
        Try
            Me.DbLeeCentral = New C_DATOS.C_DatosOledb(Me.mStrConexionCentral)
            Me.DbLeeCentral.AbrirConexion()
            Me.DbLeeCentral.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


           

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Abrir conexiones")
        End Try
    End Sub
    Private Sub SpyroCompruebaCuenta(ByVal vCuenta As String, ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vLinea As Integer, ByVal vDebeHaber As String, ByVal vAmpcpto As String, ByVal vNombre As String, ByVal vFactura As String, ByVal vSerie As String)
        Try

            SQL = "SELECT COD FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND COD = '" & vCuenta & "'"



            If IsNothing(Me.DbSpyro.EjecutaSqlScalar(SQL)) = True Then
                Me.ListBoxSpyro.Items.Add("SPYRO   : " & vCuenta & "  No se localiza en Plan de Cuentas de Spyro")
                Me.ListBoxSpyro.Update()
                Exit Sub
            End If


            SQL = "SELECT APTESDIR_SN FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND COD = '" & vCuenta & "'"



            If Me.DbSpyro.EjecutaSqlScalar(SQL) <> "S" Then
                Me.ListBoxSpyro.Items.Add("SPYRO   : " & vCuenta & "  No es una Cuenta de Apuntes Directos en Plan de Cuentas Spyro")
                Me.ListBoxSpyro.Update()
                Exit Sub
            End If

            If vTipo = "AC" And (vDebeHaber = Me.mIndicadorDebeFac Or vDebeHaber = Me.mIndicadorHaberFac) Then
                SQL = "SELECT 'X' FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
                SQL += "  AND COD = '" & vCuenta & "'"
                SQL += " AND RSOCIAL_COD IS NULL"



                If Me.DbSpyro.EjecutaSqlScalar(SQL) = "X" Then
                    Me.ListBoxSpyro.Items.Add("SPYRO   : " & vCuenta & "  No tiene definida Razón Social  en Plan de Cuentas de Spyro")
                    Me.ListBoxSpyro.Update()
                    Exit Sub
                End If

            End If

            If vTipo = "AC" And (vDebeHaber = Me.mIndicadorDebeFac Or vDebeHaber = Me.mIndicadorHaberFac) Then
                SQL = "SELECT 'X' FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
                SQL += "  AND COD = '" & vCuenta & "'"
                SQL += " AND CFIVALIBRO_COD IS NULL"



                If Me.DbSpyro.EjecutaSqlScalar(SQL) = "X" Then
                    Me.ListBoxSpyro.Items.Add("SPYRO   : " & vCuenta & "  No tiene definido Libro de Iva   en Plan de Cuentas de Spyro")
                    Me.ListBoxSpyro.Update()
                    Exit Sub
                End If

            End If
            If vTipo = "AC" And (vDebeHaber = Me.mIndicadorDebeFac Or vDebeHaber = Me.mIndicadorHaberFac) Then
                SQL = "SELECT 'X' FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
                SQL += "  AND COD = '" & vCuenta & "'"
                SQL += " AND CFIVACLASE_COD IS NULL"



                If Me.DbSpyro.EjecutaSqlScalar(SQL) = "X" Then
                    Me.ListBoxSpyro.Items.Add("SPYRO   : " & vCuenta & "  No tiene definido Clase de Iva   en Plan de Cuentas de Spyro")
                    Me.ListBoxSpyro.Update()
                    Exit Sub
                End If

            End If

            If vTipo = "AC" And (vDebeHaber = Me.mIndicadorDebeFac Or vDebeHaber = Me.mIndicadorHaberFac) Then
                SQL = "SELECT 'X' FROM CFCTACONDI WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
                SQL += "  AND CFCTA_COD = '" & vCuenta & "'"

                If IsNothing(Me.DbSpyro.EjecutaSqlScalar(SQL)) = True Then
                    Me.ListBoxSpyro.Items.Add("SPYRO   : " & vCuenta & "  No tiene definido Forma de pago en Plan de Cuentas de Spyro")
                    Me.ListBoxSpyro.Update()
                    Exit Sub
                End If

            End If


            If vTipo = "AC" And (vDebeHaber = Me.mIndicadorDebeFac Or vDebeHaber = Me.mIndicadorHaberFac) Then
                SQL = "SELECT 'X' FROM FACTUTIPO WHERE"
                SQL += " COD = '" & vSerie & "'"


                If IsNothing(Me.DbSpyro.EjecutaSqlScalar(SQL)) = True Then
                    Me.ListBoxSpyro.Items.Add("SPYRO   : " & vSerie & "  Serie NO Definida")
                    Me.ListBoxSpyro.Update()
                    Exit Sub
                End If

            End If

        Catch ex As OleDb.OleDbException
            MsgBox(ex.Message, MsgBoxStyle.Information, " Localiza Cuenta Contable SPYRO")
        End Try
    End Sub

    Private Sub ButtonCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancelar.Click
        Try
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonLeeParametros_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonLeeParametros.Click
        Try
            Me.CargaParametros()
        Catch ex As Exception

        End Try
    End Sub
End Class