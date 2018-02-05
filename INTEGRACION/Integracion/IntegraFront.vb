
' La Clase se Construye pasandole 
'          Cadena Conexion Central
'          Grupo de Empresas 
'          Codigo de Empresa 
'          Cadena de ConexionHotel  
'          Fecha a procesar 
'          Path y nombre para el fichero a generar
'
' Luego llamar al metodo "PROCESAR"
'
'

Option Strict On

Imports System.IO
Imports System.Globalization


Public Class IntegraFront

    Declare Function CharToOem Lib "user32" Alias "CharToOemA" (ByVal lpszSrc As String, ByVal lpszDst As String) As Long
    Declare Function OemToChar Lib "user32" Alias "OemToCharA" (ByVal lpszSrc As String, ByVal lpszDst As String) As Long


    Private mDebug As Boolean = False
    Private mStrConexionHotel As String
    Private mStrConexionCentral As String
    Private mStrConexionSpyro As String

    Private mFecha As Date
    Private mEmpGrupoCod As String
    Private mEmpCod As String

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
    Private mCfivaLibro_Cod As String
    Private mCfivaClase_Cod As String
    Private mMonedas_Cod As String
    Private mCfatodiari_Cod As String

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

    ' OTROS 
    Private iASCII(63) As Integer       'Para conversión a MS-DOS
    Private AuxCif As String
    Private AuxCuenta As String


    Private SQL As String
    Private Linea As Integer
    Private mTexto As String
    Private Filegraba As StreamWriter
    Private FileEstaOk As Boolean = False
    Private DbLeeCentral As C_DATOS.C_DatosOledb
    Private DbLeeHotel As C_DATOS.C_DatosOledb
    Private DbLeeHotelAux As C_DATOS.C_DatosOledb
    Private DbGrabaCentral As C_DATOS.C_DatosOledb
    Private DbSpyro As C_DATOS.C_DatosOledb


    Private NEWHOTEL As NewHotel.NewHotelData


#Region "CONSTRUCTOR"
    Public Sub New(ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vStrConexionCentral As String, _
    ByVal vStrConexionHotel As String, ByVal vFecha As Date, ByVal vFileName As String, ByVal vDebug As Boolean, _
    ByVal vConrolDebug As System.Windows.Forms.TextBox, ByVal vListBox As System.Windows.Forms.ListBox, _
    ByVal vStrConexionSpyro As String, ByVal vProgress As System.Windows.Forms.ProgressBar, ByVal vTrataDebitoNoFacturadoTpv As Boolean, ByVal vUsaTnhtMoviAuto As Boolean)


        MyBase.New()

        Me.mDebug = vDebug
        Me.mEmpGrupoCod = vEmpGrupoCod
        Me.mEmpCod = vEmpCod
        Me.mStrConexionHotel = vStrConexionHotel
        Me.mStrConexionCentral = vStrConexionCentral
        Me.mStrConexionSpyro = vStrConexionSpyro
        Me.mFecha = vFecha
        Me.mTrataDebitoTpvnoFacturado = vTrataDebitoNoFacturadoTpv

        Me.mTextDebug = vConrolDebug

        Me.mProgress = vProgress
        Me.mProgress.Value = 0
        Me.mProgress.Maximum = 100

        Me.mListBoxDebug = vListBox

        Me.mListBoxDebug.Items.Clear()
        Me.mListBoxDebug.Update()

        Me.mUsaTnhtMoviAuto = vUsaTnhtMoviAuto




        Me.AbreConexiones()
        Me.CargaParametros()

        If Me.mParaValidaSpyro = 1 Then
            Me.DbSpyro = New C_DATOS.C_DatosOledb(Me.mStrConexionSpyro)
            Me.DbSpyro.AbrirConexion()
        Else
            '      MsgBox("No hay proceso de validación de cuentas en Spyro", MsgBoxStyle.Exclamation, "Atención")
        End If

        Me.BorraRegistros()
        Me.CrearFichero(Me.mParaFilePath & vFileName)


        'Dim Texto As String

        'Texto = "Ojo tratamiento de devoluciones de pagos a cuenta asiento 21 en FASE DE PRUEBAS" & vbCrLf & vbCrLf
        'Texto += "Ojo tratamiento de Notas de credito asiento 51 en FASE DE PRUEBAS(falta asisnto de cancelacion de Notas de credito)" & vbCrLf & vbCrLf
        'Texto += "Ojo falta devoluciones de pagos a cuenta de CUENTAS DE NO ALOJADO"
        'MsgBox(Texto, MsgBoxStyle.Critical, "Atención")

        ' auditoria 
        'Me.FacturasSinCuentaContable()

    End Sub
#End Region
#Region "PROPIEDADES"
    Public Property LiquidoServiciosConIgic() As Double
        Get
            Return mLiquidoServiciosConIgic
        End Get
        Set(ByVal Value As Double)
            mLiquidoServiciosConIgic = Value
        End Set
    End Property
    Public Property LiquidoServiciosSinIgic() As Double
        Get
            Return mLiquidoServiciosSinIgic
        End Get
        Set(ByVal Value As Double)
            mLiquidoServiciosSinIgic = Value
        End Set
    End Property
    Public Property LiquidoDesembolsos() As Double
        Get
            Return mLiquidoDesembolsos
        End Get
        Set(ByVal Value As Double)
            mLiquidoDesembolsos = Value
        End Set
    End Property
    Public Property CancelacionAnticipos() As Double
        Get
            Return mCancelacionAnticipos
        End Get
        Set(ByVal Value As Double)
            mCancelacionAnticipos = Value
        End Set
    End Property
    Public Property DevolucionAnticipos() As Double
        Get
            Return mDevolucionAnticipos
        End Get
        Set(ByVal Value As Double)
            mDevolucionAnticipos = Value
        End Set
    End Property



#End Region
#Region "METODOS Privados"
    Private Sub CrearFichero(ByVal vFile As String)

        Try
            'Filegraba = New StreamWriter(vFile, False, System.Text.Encoding.UTF8)
            Filegraba = New StreamWriter(vFile, False, System.Text.Encoding.ASCII)

            Filegraba.WriteLine("")
            FileEstaOk = True
        Catch ex As Exception
            FileEstaOk = False
            MsgBox("No dispone de acceso al Fichero " & vFile, MsgBoxStyle.Information, "Atención")
        End Try
    End Sub
    Private Function MyCharToOem1(ByVal vStr1 As String, ByVal vLongitud As Integer) As String
        Try
            Dim Fijo As Long
            Dim StrConv As String = Space(vLongitud)
            Fijo = CharToOem(vStr1, StrConv)
            If mParaToOem = True Then

                Return StrConv
            Else
                Return vStr1
            End If

        Catch ex As Exception
            Return " "
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Char to Oem")
        End Try

    End Function
    Private Sub IniciarFiltroMSDOS()
        'Convertir de ANSI (windows) a ASCII (dos)
        Dim i As Integer
        Dim p As Integer
        '
        p = 0
        For i = 128 To 156          'de Ç a £   29
            p = p + 1
            iASCII(p) = i
        Next
        For i = 160 To 168          'de á a ¿   9
            p = p + 1
            iASCII(p) = i
        Next
        For i = 170 To 175          'de ¬ a »   6
            p = p + 1
            iASCII(p) = i
        Next
        '44 códigos asignados hasta aquí
        iASCII(45) = 225            'ß
        iASCII(46) = 230            'µ
        iASCII(47) = 241            '±
        iASCII(48) = 246            '÷
        iASCII(49) = 253            '²
        iASCII(50) = 65             'Á (A)
        iASCII(51) = 73             'Í (I)
        iASCII(52) = 79             'Ó (O)
        iASCII(53) = 85             'Ú (U)
        iASCII(54) = 73             'Ï (I)
        iASCII(55) = 65             'À (A)
        iASCII(56) = 69             'È (E)
        iASCII(57) = 73             'Ì (I)
        iASCII(58) = 79             'Ò (O)
        iASCII(59) = 85             'Ù (U)
        iASCII(60) = 69             'Ë (E)
        For i = 61 To 63            ''`´ (')
            iASCII(i) = 39
        Next
    End Sub
    Private Function MyCharToOem(ByVal sWIN As String) As String
        'Filtrar la cadena para convertirla en compatible MS-DOS
        Dim sANSI As String = "ÇüéâäàåçêëèïîìÄÅÉæÆôöòûùÿÖÜø£áíóúñÑªº¿¬½¼¡«»ßµ±÷²ÁÍÓÚÏÀÈÌÒÙË'`´"
        '
        Dim i As Integer
        Dim p As Integer
        Dim sC As Integer
        Dim sMSD As String

        'Aquí se puede poner esta comparación para saber
        'si el array está inicializado.
        'De esta forma no será necesario llamar al procedimiento
        'de inicialización antes de usar esta función.
        '(deberás quitar los comentarios)
        If iASCII(1) = 0 Then       'El primer valor debe ser 128
            IniciarFiltroMSDOS()
        End If

        sMSD = ""
        For i = 1 To Len(sWIN)
            sC = Asc(Mid$(sWIN, i, 1))
            p = InStr(sANSI, Chr(sC))
            If p > 0 Then
                sC = iASCII(p)
            End If
            sMSD = sMSD & Chr(sC)
        Next


        If mParaToOem = True Then

            Return sWIN
        Else
            Return sMSD
        End If
    End Function


    Private Sub AbreConexiones()
        Try
            Me.DbLeeCentral = New C_DATOS.C_DatosOledb(Me.mStrConexionCentral)
            Me.DbLeeCentral.AbrirConexion()
            Me.DbLeeCentral.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbGrabaCentral = New C_DATOS.C_DatosOledb(Me.mStrConexionCentral)
            Me.DbGrabaCentral.AbrirConexion()
            Me.DbGrabaCentral.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbLeeHotel = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
            Me.DbLeeHotel.AbrirConexion()
            Me.DbLeeHotel.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbLeeHotelAux = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
            Me.DbLeeHotelAux.AbrirConexion()
            Me.DbLeeHotelAux.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbSpyro = New C_DATOS.C_DatosOledb
            ' LA APERTURA se hace mas abajo ahora si existe contabilidad spyro para validar cuentas
            'Me.DbSpyro.AbrirConexion()
            'Me.DbSpyro.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Abrir conexiones")
        End Try
    End Sub
    Private Sub CargaParametros()
        Try

            Me.mTextDebug.Text = "Cargando Parámetros"
            Me.mTextDebug.Update()

            SQL = "SELECT NVL(PARA_CTA1,'0') PARA_CTA1, "
            SQL += "NVL(PARA_CTA3,'0') PARA_CTA3, "
            SQL += "NVL(PARA_CTA4,'0') PARA_CTA4, "
            SQL += "NVL(PARA_CTA5,'0') PARA_CTA5, "
            SQL += "NVL(PARA_CFIVALIBRO_COD,'?') LIBROIVA,"
            SQL += "NVL(PARA_CFIVACLASE_COD,'?') CLASEIVA,"
            SQL += "NVL(PARA_MONEDAS_COD,'?') MONEDA,"
            SQL += "NVL(PARA_CFATODIARI_COD,'?') DIARIO,"
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
            SQL += "NVL(PARA_USA_CTACOMISION,'0') USACTACOMISION"



            SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
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
            End If
            Me.DbLeeCentral.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Exclamation, "Carga de Parámetros en Constructor de la Clase")
        End Try
    End Sub
    Private Sub BorraRegistros()
        Try
            SQL = "SELECT COUNT(*) FROM TH_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
            If CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Integer) > 0 Then
                MsgBox("Ya existen Movimientos de Integración para esta Fecha", MsgBoxStyle.Information, "Atención")
            End If
            SQL = "DELETE TH_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            SQL = "DELETE TH_ERRO WHERE ERRO_F_ATOCAB =  '" & Me.mFecha & "'"
            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Borra Registros")
        End Try

    End Sub

    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                      ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                      , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                        ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String)

        Try

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40) & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "')"




            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  "
            Me.mTextDebug.Update()

            If vCfcta_Cod.Length < 2 And vCfcta_Cod <> "NO TRATAR" Then
                Me.mTexto = "NEWHOTEL: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40)
                Me.mListBoxDebug.Items.Add(Me.mTexto)
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            End If


            If vTipo = "FV" Then
                If vCif.Length = 0 Then
                    Me.mTexto = "NEWHOTEL: " & "CIF no válido para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40)
                    Me.mListBoxDebug.Items.Add(Me.mTexto)
                    SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                End If
            End If

            If vCfcta_Cod <> "NO TRATAR" Then
                If Me.mParaValidaSpyro = 1 Then
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    Private Sub SpyroCompruebaCuenta(ByVal vCuenta As String, ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vLinea As Integer, ByVal vDebeHaber As String)
        Try

            Me.mTextDebug.Text = "Validando Plan de Cuentas Spyro " & vCuenta
            Me.mTextDebug.Update()

            SQL = "SELECT COD FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND COD = '" & vCuenta & "'"



            If IsNothing(Me.DbSpyro.EjecutaSqlScalar(SQL)) = True Then
                Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & " no se localiza en Plan de Cuentas de Spyro")
                Me.mListBoxDebug.Update()
                Me.mTexto = "SPYRO   : " & vCuenta & " no se localiza en Plan de Cuentas de Spyro"
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Exit Sub
            End If


            SQL = "SELECT APTESDIR_SN FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND COD = '" & vCuenta & "'"



            If Me.DbSpyro.EjecutaSqlScalar(SQL) <> "S" Then
                Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & " No es una Cuenta de Apuntes Directos en Plan de Cuentas Spyro")
                Me.mListBoxDebug.Update()
                Me.mTexto = "SPYRO   : " & vCuenta & " No es una Cuenta de Apuntes Directos en Plan de Cuentas Spyro"
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Exit Sub
            End If

            If vTipo = "AC" And (vDebeHaber = Me.mIndicadorDebeFac Or vDebeHaber = Me.mIndicadorHaberFac) Then
                SQL = "SELECT 'X' FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
                SQL += "  AND COD = '" & vCuenta & "'"
                SQL += " AND RSOCIAL_COD IS NULL"



                If Me.DbSpyro.EjecutaSqlScalar(SQL) = "X" Then
                    Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & " No tiene definida Razón Social  en Plan de Cuentas de Spyro")
                    Me.mListBoxDebug.Update()
                    Me.mTexto = "SPYRO   : " & vCuenta & " No tiene definida Razón Social  en Plan de Cuentas de Spyro"
                    SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                    Exit Sub
                End If

            End If

            If vTipo = "AC" And (vDebeHaber = Me.mIndicadorDebeFac Or vDebeHaber = Me.mIndicadorHaberFac) Then
                SQL = "SELECT 'X' FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
                SQL += "  AND COD = '" & vCuenta & "'"
                SQL += " AND CFIVALIBRO_COD IS NULL"



                If Me.DbSpyro.EjecutaSqlScalar(SQL) = "X" Then
                    Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & " No tiene definido Libro de Iva   en Plan de Cuentas de Spyro")
                    Me.mListBoxDebug.Update()
                    Me.mTexto = "SPYRO   : " & vCuenta & " No tiene definido Libro de Iva   en Plan de Cuentas de Spyro"
                    SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                    Exit Sub
                End If

            End If
            If vTipo = "AC" And (vDebeHaber = Me.mIndicadorDebeFac Or vDebeHaber = Me.mIndicadorHaberFac) Then
                SQL = "SELECT 'X' FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
                SQL += "  AND COD = '" & vCuenta & "'"
                SQL += " AND CFIVACLASE_COD IS NULL"



                If Me.DbSpyro.EjecutaSqlScalar(SQL) = "X" Then
                    Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & " No tiene definido Clase de Iva   en Plan de Cuentas de Spyro")
                    Me.mListBoxDebug.Update()
                    Me.mTexto = "SPYRO   : " & vCuenta & " No tiene definido Clase de Iva   en Plan de Cuentas de Spyro"
                    SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                    Exit Sub
                End If

            End If

            If vTipo = "AC" And (vDebeHaber = Me.mIndicadorDebeFac Or vDebeHaber = Me.mIndicadorHaberFac) Then
                SQL = "SELECT 'X' FROM CFCTACONDI WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
                SQL += "  AND CFCTA_COD = '" & vCuenta & "'"

                If IsNothing(Me.DbSpyro.EjecutaSqlScalar(SQL)) = True Then
                    Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & " No tiene definido Forma de pago en Plan de Cuentas de Spyro")
                    Me.mListBoxDebug.Update()
                    Me.mTexto = "SPYRO   : " & vCuenta & "No tiene definido Forma de pago en Plan de Cuentas de Spyro"
                    SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                    Exit Sub
                End If

            End If

        Catch ex As OleDb.OleDbException
            MsgBox(ex.Message, MsgBoxStyle.Information, " Localiza Cuenta Contable SPYRO")
        End Try
    End Sub
    Private Sub GeneraFileAC(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
     ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double)
        Try
            Dim FechaAsiento As String
            If Me.mParaFechaRegistroAc = "V" Then
                FechaAsiento = Format(Me.mFecha, "ddMMyyyy")
            ElseIf Me.mParaFechaRegistroAc = "R" Then
                FechaAsiento = Format(Now, "ddMMyyyy")
            Else
                FechaAsiento = Format(Me.mFecha, "ddMMyyyy")
            End If

            '-------------------------------------------------------------------------------------------------
            '  Apuntes Contables(CFATOLIN)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(MyCharToOem(vTipo.PadRight(2, CChar(" ")) & _
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
            Format(Me.mFecha, "ddMMyyyy") & _
            " ".PadRight(40, CChar(" ")) & _
            Me.mCfatotip_Cod.PadRight(4, CChar(" "))))

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
        End Try
    End Sub
    Private Sub GeneraFileAC2(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
  ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, ByVal vFactuTipo_cod As String, ByVal vNfactura As Integer)
        Try
            Dim FechaAsiento As String
            If Me.mParaFechaRegistroAc = "V" Then
                FechaAsiento = Format(Me.mFecha, "ddMMyyyy")
            ElseIf Me.mParaFechaRegistroAc = "R" Then
                FechaAsiento = Format(Now, "ddMMyyyy")
            Else
                FechaAsiento = Format(Me.mFecha, "ddMMyyyy")
            End If

            '-------------------------------------------------------------------------------------------------
            '  Apuntes Contables(CFATOLIN)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(MyCharToOem(vTipo.PadRight(2, CChar(" ")) & _
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
            Format(Me.mFecha, "ddMMyyyy") & _
            " ".PadRight(40, CChar(" ")) & _
            Me.mCfatotip_Cod.PadRight(4, CChar(" ")) & _
            mCfivaLibro_Cod.PadRight(2, CChar(" ")) & _
            vFactuTipo_cod.PadRight(6, CChar(" ")) & _
            CType(vNfactura, String).PadRight(8, CChar(" "))))

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
        End Try
    End Sub
    Private Sub GeneraFileAC3(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
 ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double)
        Try

            Dim FechaAsiento As String
            If Me.mParaFechaRegistroAc = "V" Then
                FechaAsiento = Format(Me.mFecha, "ddMMyyyy")
            ElseIf Me.mParaFechaRegistroAc = "R" Then
                FechaAsiento = Format(Now, "ddMMyyyy")
            Else
                FechaAsiento = Format(Me.mFecha, "ddMMyyyy")
            End If
            '-------------------------------------------------------------------------------------------------
            '  Apuntes Contables(CFATOLIN)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(MyCharToOem(vTipo.PadRight(2, CChar(" ")) & _
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
            Format(Me.mFecha, "ddMMyyyy") & _
            " ".PadRight(40, CChar(" ")) & _
            Me.mCfatotip_Cod.PadRight(4, CChar(" ")) & _
            "*"))


        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
        End Try
    End Sub
    Private Sub GeneraFileAA(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                 ByVal vCfatocab_Refer As Integer, _
                                  ByVal vCfcta_Cod As String, ByVal vCfcctip_Cod As String, ByVal vCfcCosto_Cod As String, _
                                  ByVal vImonep As Double)
        Try

            '-------------------------------------------------------------------------------------------------
            '  Apuntes Contables(CFATOLINCC)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) & _
            vEmpGrupoCod.PadRight(4, CChar(" ")) & _
            vEmpCod.PadRight(4, CChar(" ")) & _
            Mid(Format(Me.mFecha, "ddMMyyyy"), 5, 4) & _
            Me.mCfatodiari_Cod.PadRight(4, CChar(" ")) & _
            " ".PadLeft(8, CChar(" ")) & _
            " ".PadLeft(4, CChar(" ")) & _
            vCfcta_Cod.PadRight(15, CChar(" ")) & _
            vCfcctip_Cod.PadRight(4, CChar(" ")) & _
            vCfcCosto_Cod.PadRight(15, CChar(" ")) & _
            CType(vImonep, String).PadLeft(16, CChar(" ")) & Format(Me.mFecha, "ddMMyyyy"))

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAA")
        End Try
    End Sub
    Private Sub GeneraFileFV(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, _
    ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String, ByVal vPendiente As Double)

        Try
            '-------------------------------------------------------------------------------------------------
            '  Facturas(FACTURAS)
            '-------------------------------------------------------------------------------------------------
            ' MsgBox(vSfactura)
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) & _
            vEmpGrupoCod.PadRight(4, CChar(" ")) & _
            vEmpCod.PadRight(4, CChar(" ")) & _
            Me.mCfivaLibro_Cod.PadRight(2, CChar(" ")) & _
            vSerie.PadRight(6, CChar(" ")) & _
            CType(vNfactura, String).PadLeft(8, CChar(" ")) & _
            " ".PadRight(8, CChar(" ")) & _
            Format(Me.mFecha, "ddMMyyyy") & _
            Me.mCfivaClase_Cod.PadRight(2, CChar(" ")) & _
            CType(vImonep, String).PadLeft(16, CChar(" ")) & _
            Me.mMonedas_Cod.PadRight(4, CChar(" ")) & _
            CType(vImonep, String).PadLeft(16, CChar(" ")) & _
            Mid(vSfactura, 1, 15).PadRight(15, CChar("-")) & _
            vEmpGrupoCod.PadRight(4, CChar(" ")) & _
            vEmpCod.PadRight(4, CChar(" ")) & _
            Format(Me.mFecha, "yyyy") & _
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
    Private Sub GeneraFileFV2(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, _
   ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String)

        Try
            '-------------------------------------------------------------------------------------------------
            '  Facturas(FACTURAS)
            '-------------------------------------------------------------------------------------------------
            ' MsgBox(vSfactura)
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) & _
            vEmpGrupoCod.PadRight(4, CChar(" ")) & _
            vEmpCod.PadRight(4, CChar(" ")) & _
            Me.mCfivaLibro_Cod.PadRight(2, CChar(" ")) & _
            vSerie.PadRight(6, CChar(" ")) & _
            CType(vNfactura, String).PadLeft(8, CChar(" ")) & _
            " ".PadRight(8, CChar(" ")) & _
            Format(Me.mFecha, "ddMMyyyy") & _
            Me.mCfivaClase_Cod.PadRight(2, CChar(" ")) & _
            CType(vImonep, String).PadLeft(16, CChar(" ")) & _
            Me.mMonedas_Cod.PadRight(4, CChar(" ")) & _
            CType(vImonep, String).PadLeft(16, CChar(" ")) & _
            Mid(vSfactura, 1, 15).PadRight(15, CChar("-")) & _
            vEmpGrupoCod.PadRight(4, CChar(" ")) & _
            vEmpCod.PadRight(4, CChar(" ")) & _
            Format(Me.mFecha, "yyyy") & _
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
            "0".PadRight(16, CChar(" ")) & _
            "0".PadRight(16, CChar(" ")) & "NN")


        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileFV")
        End Try
    End Sub
    Private Sub GeneraFileVF(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, _
   ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String)

        Try
            '-------------------------------------------------------------------------------------------------
            '  Facturas(FACTURAS)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) & _
            vEmpGrupoCod.PadRight(4, CChar(" ")) & _
            vEmpCod.PadRight(4, CChar(" ")) & _
            Me.mCfivaLibro_Cod.PadRight(2, CChar(" ")) & _
            vSerie.PadRight(6, CChar(" ")) & _
            CType(vNfactura, String).PadLeft(8, CChar(" ")) & _
            vEmpGrupoCod.PadRight(4, CChar(" ")) & _
            vEmpCod.PadRight(4, CChar(" ")) & _
            Format(Me.mFecha, "yyyy") & _
            Me.mCfatodiari_Cod.PadRight(4, CChar(" ")))

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileVF")
        End Try
    End Sub
    Private Sub GeneraFileIV(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vFactutipo_cod As String, _
    ByVal vNfactura As Integer, ByVal vI_basmonemp As Double, ByVal vPj_iva As Double, ByVal vI_ivamonemp As Double, ByVal vX As String)


        Try
            '-------------------------------------------------------------------------------------------------
            '  Libro de Iva(CFIVALIN)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) & _
            vEmpGrupoCod.PadRight(4, CChar(" ")) & _
            vEmpCod.PadRight(4, CChar(" ")) & _
            Me.mCfivaLibro_Cod.PadRight(2, CChar(" ")) & _
            vFactutipo_cod.PadRight(6, CChar(" ")) & _
            CType(vNfactura, String).PadRight(8, CChar(" ")) & _
            Me.mCfivatimpu_Cod.PadRight(2, CChar(" ")) & _
            vX.PadRight(2, CChar(" ")) & _
            CType(vI_basmonemp, String).PadRight(16, CChar(" ")) & _
            CType(vPj_iva, String).PadRight(10, CChar(" ")) & _
            CType(vI_ivamonemp, String).PadRight(16, CChar(" ")) & _
            CType(vI_basmonemp, String).PadRight(16, CChar(" ")) & _
            CType(vI_ivamonemp, String).PadRight(16, CChar(" ")))

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileIV")
        End Try
    End Sub
    Private Sub CerrarFichero()
        Try
            Filegraba.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub CierraConexiones()
        Try
            Me.DbLeeCentral.CerrarConexion()
            Me.DbGrabaCentral.CerrarConexion()
            Me.DbLeeHotel.CerrarConexion()
            Me.DbLeeHotelAux.CerrarConexion()
            Me.DbSpyro.CerrarConexion()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub FacturasSinCuentaContable()
        Try
            SQL = "SELECT DECODE(FACT_ANUL,'0','EMITIDA','1','ANULADA') AS ESTADO ,FACT_CODI,SEFA_CODI,FACT_TITU FROM TNHT_FACT WHERE FACT_DAEM = '" & Me.mFecha & "'"
            SQL += " AND FACT_STAT IN('2','3') AND ENTI_CODI IS NULL AND CCEX_CODI IS NULL"
            Me.DbLeeHotel.TraerLector(SQL)
            While Me.DbLeeHotel.mDbLector.Read
                Me.mTexto = "Factura de Crédito sin cuenta Contable Localizable" & vbCrLf
                Me.mTexto += CType(Me.DbLeeHotel.mDbLector.Item("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector.Item("SEFA_CODI"), String) & vbCrLf
                Me.mTexto += CType(Me.DbLeeHotel.mDbLector.Item("FACT_TITU"), String) & vbCrLf
                Me.mTexto += "Estado Actual  =" & CType(Me.DbLeeHotel.mDbLector.Item("ESTADO"), String) & vbCrLf
                MsgBox(Me.mTexto, MsgBoxStyle.Exclamation, "Atención")
            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Facturas sin Cuenta Contable")
        End Try
    End Sub
#Region "ASIENTO-1"
    Private Sub PendienteFacturarTotal()

        Try


            Dim Total As Double
            SQL = "SELECT "
            SQL += "ROUND (SUM (MOVI_VLIQ), 2)"
            SQL += " FROM TNHT_MOVI ,TNHT_SERV"
            SQL += " WHERE MOVI_DATR= '" & Me.mFecha & "'"
            SQL += " AND TNHT_MOVI.SERV_CODI(+) = TNHT_SERV.SERV_CODI "
            '  SQL += " AND TNHT_SERV.SERV_CTB1 <> '#'"


            If IsNumeric(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = True Then
                Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)


            Else
                Total = 0
            End If
            If Total <> 0 Then
                Linea = 1
                Me.mTipoAsiento = "DEBE"

                Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorDebe, "PENDIENTE DE FACTURAR", Total, "SI", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorDebe, "PENDIENTE DE FACTURAR", Total)

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub VentasDepartamento()



        Dim Total As Double
        Dim vCentroCosto As String
        SQL = "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI SERVICIO,TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CTB1,'0') CUENTA ,"
        SQL += "ROUND (SUM (MOVI_VLIQ), 2) TOTAL"
        SQL += " FROM TNHT_MOVI,TNHT_SERV"
        SQL += " WHERE (TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI) AND MOVI_DATR= '" & Me.mFecha & "'"
        ' SQL += " AND TNHT_SERV.SERV_CTB1 <> '#'"
        SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1"
        SQL += " ORDER BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total, "NO", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total)
                If vCentroCosto <> "0" Then
                    Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", vCentroCosto, Total)
                End If
            End If
        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
#End Region
#Region "ASIENTO-2"
    Private Sub TotalPagosaCuentaVisas()
        Try
            Dim Total As Double
            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CONT,'0') CUENTA"
            SQL = SQL & " FROM TNHT_MOVI,TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"
            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' excluir depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

            If Me.mUsaTnhtMoviAuto = True Then
                SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
            End If
            '
            SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CONT"


            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
        End Try

    End Sub
    Private Sub TotalPagosaCuentaOtrasFormas()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
       

        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"
        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetallePagosaCuentaVisas()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
        SQL = SQL & " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA FROM TNHT_MOVI,"
        SQL = SQL & " TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Reserv= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Reserv= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetallePagosaCuentaOtrasFormas()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE,TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_DAVA FROM TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
       
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Reserv= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Reserv= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
#End Region
#Region "ASIENTO-21 DEVOLUCIONES "
    Private Sub TotalDevolucionesVisas()
        Try
            Dim Total As Double
            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CONT,'0') CUENTA"
            SQL = SQL & " FROM TNHT_MOVI,TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"
            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 5"
            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' excluir depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

            If Me.mUsaTnhtMoviAuto = True Then
                SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
            End If
            '
            SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CONT"


            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 21, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
        End Try

    End Sub
    Private Sub TotalDevolucionesOtrasFormas()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 5"


        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"
        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 21, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetalleDevolucionesVisas()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
        SQL = SQL & " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA FROM TNHT_MOVI,"
        SQL = SQL & " TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 5"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 21, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Reserv= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Reserv= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetalleDevolucionesOtrasFormas()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE,TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_DAVA FROM TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 5"

        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 21, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Reserv= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Reserv= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
#End Region
#Region "ASIENTO-99 ANTICIPOS RECIBIDOS CUENTAS NO ALOJADO"
    Private Sub TotalPagosaCuentaVisasNoAlojados()
        Try
            Dim Total As Double
            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CONT,'0') CUENTA"
            SQL = SQL & " FROM TNHT_MOVI,TNHT_CACR WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI  IS NOT NULL"
            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' excluir depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

            If Me.mUsaTnhtMoviAuto = True Then
                SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
            End If
            '
            SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CONT"


            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 99, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
        End Try

    End Sub
    Private Sub TotalPagosaCuentaOtrasFormasNoAlojados()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM TNHT_MOVI,TNHT_FORE WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI IS NOT NULL"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"
        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 99, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetallePagosaCuentaVisasNoAlojados()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.CCEX_CODI CCEX,NVL(CCEX_TITU,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
        SQL = SQL & " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA FROM TNHT_MOVI,"
        SQL = SQL & " TNHT_CACR,TNHT_CCEX WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI "
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 99, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Cuenta = " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''") & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Cuenta= " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''") & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetallePagosaCuentaOtrasFormasNoAlojados()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.CCEX_CODI CCEX,NVL(CCEX_TITU,'?') CLIENTE,TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_DAVA FROM TNHT_MOVI,TNHT_FORE,TNHT_CCEX WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI "
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 99, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Cuenta= " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''") & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Cuenta= " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''") & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
#End Region
#Region "ASIENTO-3"
    Private Sub FacturasSalidaTotalLiquido()

        Dim Total As Double
        Dim TotalComisiones As Double
        Dim SQL As String


        SQL = "SELECT (SUM(FAIV_INCI) - SUM(FAIV_VIMP)) TOTAL ,SUM(FACT_TOTA) TOTAL1,FACT_DAEM "
        SQL += "FROM TNHT_FAIV, TNHT_FACT "
        SQL += "WHERE TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
        SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
        SQL += "AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        SQL += "AND (FACT_DAAN IS NULL OR FACT_DAEM < FACT_DAAN)"
        SQL += "AND TNHT_FACT.FACT_STAT = '1' AND FACT_CLEN = '1'"
        SQL += "GROUP BY TNHT_FACT.FACT_DAEM"




        If IsDBNull(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = False Then
            Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double) + Me.FacturacionSalidasServiciosSinIgic + Me.FacturacionSalidaDesembolsos



            If mDebug = True Then
                Me.LiquidoServiciosConIgic = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
                Me.LiquidoServiciosSinIgic = Me.FacturacionSalidasServiciosSinIgic
                Me.LiquidoDesembolsos = Me.FacturacionSalidaDesembolsos

            End If
        Else
            Total = 0
        End If


        ' tinglado de sumar las comisiones al total liquido

        SQL = "SELECT NVL(SUM(TNHT_DESF.DESF_VALO),'0')TOTAL "
        SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE WHERE"
        SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
        SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1'  AND FACT_CLEN = '1' "
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND (FACT_DAAN IS NULL OR FACT_DAEM < FACT_DAAN)"
        If Me.mParaSantana = 1 Then
            SQL = SQL & " AND FACT_DAEM > '15-03-2007' "
        End If



        If Me.mParaComisiones = True Then
            TotalComisiones = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            Total = Total + TotalComisiones
        End If
        Total = Decimal.Round(CType(Total, Decimal), 2)






        If Total <> 0 Then
            Linea = 1
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "FACTURACIÓN LÍQUIDA " & Me.mFecha & " + Comisiones ", Total, "SI", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, "Total Liquido + Comisiones", Total)

            ' TRUCO TIBERIO 
            Me.GeneraFileAC3("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaClientesContado, Me.mIndicadorDebeFac, "FACTURACION CONTADO", Total)
            ' truco para vincular mas de una factura a un solo asiento 
            Me.FacturasSalidaTotalFActuraVF()
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaClientesContado, Me.mIndicadorHaber, "FACTURACION CONTADO", Total)
            ' FIN TRUCO

            Linea = Linea + 1
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaClientesContado, Me.mIndicadorDebe, "FACTURACION CONTADO", Total, "NO", "", "", "SI")


            Linea = Linea + 1
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaClientesContado, Me.mIndicadorHaber, "FACTURACION CONTADO", Total, "NO", "", "", "SI")

            ' FIN TRUCO 2 
        End If



    End Sub
    Private Sub FacturasSalidaTotalLiquidoTahona()

        Dim Total As Double
        Dim TotalComisiones As Double
        Dim SQL As String


        SQL = "SELECT (SUM(FAIV_INCI) - SUM(FAIV_VIMP)) TOTAL ,SUM(FACT_TOTA) TOTAL1,FACT_DAEM "
        SQL += "FROM TNHT_FAIV, TNHT_FACT "
        SQL += "WHERE TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
        SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
        SQL += "AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        SQL += "AND (FACT_DAAN IS NULL OR FACT_DAEM < FACT_DAAN)"
        SQL += "AND TNHT_FACT.FACT_STAT = '1' AND FACT_CLEN = '1'"
        SQL += "GROUP BY TNHT_FACT.FACT_DAEM"




        If IsDBNull(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = False Then
            Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double) + Me.FacturacionSalidasServiciosSinIgic + Me.FacturacionSalidaDesembolsos



            If mDebug = True Then
                Me.LiquidoServiciosConIgic = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
                Me.LiquidoServiciosSinIgic = Me.FacturacionSalidasServiciosSinIgic
                Me.LiquidoDesembolsos = Me.FacturacionSalidaDesembolsos

            End If
        Else
            Total = 0
        End If


        ' tinglado de sumar las comisiones al total liquido

        SQL = "SELECT NVL(SUM(TNHT_DESF.DESF_VALO),'0')TOTAL "
        SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE WHERE"
        SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
        SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1'  AND FACT_CLEN = '1' "
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND (FACT_DAAN IS NULL OR FACT_DAEM < FACT_DAAN)"
       


        If Me.mParaComisiones = True Then
            TotalComisiones = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            Total = Total + TotalComisiones
        End If
        Total = Decimal.Round(CType(Total, Decimal), 2)


        If Total <> 0 Then
            Linea = 1
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "FACTURACIÓN LÍQUIDA " & Me.mFecha & " + Comisiones ", Total, "SI", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, "FACTURACIÓN LÍQUIDA " & Me.mFecha & " + Comisiones ", Total)
        End If

    End Sub
   
    Private Sub FacturasSalidaTotalLiquidoDetalladoTahona()

        Dim Total As Double
        Dim TotalComisiones As Double
        Dim SQL As String

        Dim Cuenta As String
        Dim Dni As String


        SQL = "SELECT tnht_fact.fact_codi,tnht_fact.sefa_codi,(FAIV_INCI - FAIV_VIMP) TOTAL ,FACT_TOTA TOTAL1,FACT_DAEM "
        SQL += " ,tnht_fact.fact_codi || '/' || tnht_fact.sefa_codi AS FACTURA"
        SQL += " ,tnht_fact.enti_codi,tnht_fact.ccex_codi, "
        SQL += " NVL(TNHT_FACT.FACT_TITU,'?')  AS TITULAR "
        SQL += " FROM TNHT_FAIV, TNHT_FACT "
        SQL += " WHERE TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
        SQL += " AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
        SQL += " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " AND (FACT_DAAN IS NULL OR FACT_DAEM < FACT_DAAN)"
        SQL += " AND TNHT_FACT.FACT_STAT = '1' AND FACT_CLEN = '1'"
       
        Linea = Linea + 1


        Me.DbLeeHotel.TraerLector(SQL)


        Me.NEWHOTEL = New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)
        While Me.DbLeeHotel.mDbLector.Read

            'Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double) + Me.FacturacionSalidasServiciosSinIgic + Me.FacturacionSalidaDesembolsos
            Total = CType(Me.DbLeeHotel.mDbLector.Item("TOTAL"), Double)

            SQL = "SELECT NVL(SUM(TNHT_DESF.DESF_VALO),'0')TOTAL "
            SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE WHERE"
            SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
            SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
            SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
            SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1'  AND FACT_CLEN = '1' "
            SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND (FACT_DAAN IS NULL OR FACT_DAEM < FACT_DAAN)"
            SQL += " AND TNHT_FACT.FACT_CODI = " & CType(Me.DbLeeHotel.mDbLector.Item("FACT_CODI"), String)
            SQL += " AND TNHT_FACT.SEFA_CODI = '" & CType(Me.DbLeeHotel.mDbLector.Item("SEFA_CODI"), String) & "'"

            If Me.mParaComisiones = True Then
                TotalComisiones = CType(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL), Double)
                Total = Total + TotalComisiones
            End If
            Total = Decimal.Round(CType(Total, Decimal), 2)

            Cuenta = Me.NEWHOTEL.DevuelveCuentaContabledeFactura(CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String))
            Dni = Me.NEWHOTEL.DevuelveDniCifContabledeFactura(CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String))




            If Total <> 0 Then

                ' Me.mTipoAsiento = "HABER"
                ' Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector.Item("FACTURA"), String), Total, "SI", "", "", "SI")
                ' Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector.Item("FACTURA"), String), Total)
                ' Linea = Linea + 1

                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector.Item("FACTURA"), String), Total, "SI", "", CType(Me.DbLeeHotel.mDbLector.Item("TITULAR"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector.Item("FACTURA"), String), Total)
                Linea = Linea + 1

                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector.Item("FACTURA"), String), Total, "SI", "", CType(Me.DbLeeHotel.mDbLector.Item("TITULAR"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector.Item("FACTURA"), String), Total)
                Linea = Linea + 1

            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
        Me.NEWHOTEL.CerrarConexiones()


    End Sub
    Private Sub FacturasSalidaTotalFActuraVF()
        Try
            Dim TotalFactura As Double
            SQL = "SELECT   TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,TNHT_FACT.FACT_TOTA TOTAL "
            SQL += "FROM TNHT_FACT "
            SQL += "WHERE "
            SQL += "TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
            SQL += " AND (FACT_DAAN IS NULL OR FACT_DAEM < FACT_DAAN)"
            SQL += " AND (TNHT_FACT.FACT_STAT = '1' AND FACT_CLEN = '1' ) "
            SQL += "ORDER BY TNHT_FACT.SEFA_CODI ASC, TNHT_FACT.FACT_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1


                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)



                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("VF", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, "NO TRATAR", Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Me.mClientesContadoCif, "", "NO")
                Me.GeneraFileVF("VF", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalFactura, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String).PadRight(15, CChar(" ")), "TIBERIO", Me.mClientesContadoCif)


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas")
        End Try
    End Sub


    Private Sub FacturasSalidaTotalVisas()
        Dim Total As Double
        SQL = "SELECT SUM(MOVI_VDEB) TOTAL,CACR_DESC TARJETA,nvl(CACR_CONT,'0') CUENTA FROM TNHT_MOVI,TNHT_CACR,TNHT_FACT,TNHT_FAMO WHERE"

        SQL = SQL & " TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1' AND FACT_CLEN = '1' "
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND (FACT_DAAN IS NULL OR FACT_DAEM < FACT_DAAN)"
        SQL = SQL & " GROUP BY TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CONT"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasSalidaTotaLOtrasFormas()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM TNHT_MOVI,TNHT_FORE,TNHT_FACT,TNHT_FAMO WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "

        SQL = SQL & " AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1'  AND FACT_CLEN = '1' "
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND (FACT_DAAN IS NULL OR FACT_DAEM < FACT_DAAN)"

        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasSalidaTotaLDescuentos()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_DESF.DESF_VALO)TOTAL,TNHT_TIDE.TIDE_DESC TIPO,NVL(TNHT_TIDE.TIDE_CTB1,'0') CUENTA "
        SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE WHERE"
        SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
        SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1'  AND FACT_CLEN = '1' "
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND (FACT_DAAN IS NULL OR FACT_DAEM < FACT_DAAN)"
        SQL = SQL & " GROUP BY TNHT_TIDE.TIDE_DESC,TNHT_TIDE.TIDE_CTB1"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

            Me.GeneraFileAA("AA", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", Me.mParaCentroCostoAlojamiento, Total)


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasSalidaTotalFActura()
        Try
            Dim TotalFactura As Double
            Dim Dni As String

            SQL = "SELECT   TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,TNHT_FACT.FACT_TOTA TOTAL,NVL(CLIE_NUID,'0') AS DNI "
            SQL += "FROM TNHT_FACT,TNHT_CLIE "
            SQL += "WHERE "
            SQL += "(TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "
            SQL += " AND (FACT_DAAN IS NULL OR FACT_DAEM < FACT_DAAN) "
            SQL += "AND (TNHT_FACT.FACT_STAT = '1'  AND TNHT_FACT.FACT_CLEN = '1') "
            SQL += "AND TNHT_FACT.CLIE_CODI = TNHT_CLIE.CLIE_CODI(+) "
            SQL += "ORDER BY TNHT_FACT.SEFA_CODI ASC, TNHT_FACT.FACT_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1

                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

                If CType(Me.DbLeeHotel.mDbLector("DNI"), String) = "0" Then
                    Dni = Me.mClientesContadoCif
                Else
                    Dni = CType(Me.DbLeeHotel.mDbLector("DNI"), String).Trim
                End If

                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaClientesContado, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Dni, Dni, "NO")
                Me.GeneraFileFV2("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalFactura, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String).PadRight(15, CChar(" ")), Me.mCtaClientesContado, Dni)


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas")
        End Try
    End Sub
    Private Sub FacturasSalidaDetalleIgic()

        Try

            Dim TotalIva As Double
            Dim TotalBase As Double
            Dim TotalFactura As Double
            SQL = "SELECT   TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "TNHT_FAIV.FAIV_TAXA AS TIPO, TNHT_FAIV.FAIV_INCI,ROUND((FAIV_INCI-FAIV_VIMP),2) BASE, ROUND(TNHT_FAIV.FAIV_VIMP,2) IGIC,NVL(TIVA_CTB1,'0') CUENTA, '"
            SQL += Me.mParaTextoIva & " ' || FAIV_TAXA ||'%  '|| TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,ROUND(TNHT_FACT.FACT_TOTA,2) TOTAL,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X "
            SQL += "FROM TNHT_FAIV, TNHT_FACT,TNHT_TIVA "
            SQL += "WHERE TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "AND TNHT_FAIV.TIVA_CODI = TNHT_TIVA.TIVA_CODI "
            SQL += "AND (TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "
            SQL += "AND (FACT_DAAN IS NULL OR FACT_DAEM < FACT_DAAN) "
            SQL += "AND (TNHT_FACT.FACT_STAT = '1'  AND TNHT_FACT.FACT_CLEN = '1') "
            SQL += "ORDER BY TNHT_FACT.SEFA_CODI ASC, TNHT_FACT.FACT_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
                TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)


                TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
                TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

                ' TIBERIO ( EL TOTAL FACTURA ES LA BASE MAS EL IGIC )   O LOS MAS LOS DESEMBOSOS Y SERVICIOS SIN IGIC 
                'TotalFactura = Decimal.Round(CType(TotalBase + TotalIva, Decimal), 2)
                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)



                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva, "NO", Me.mClientesContadoCif, "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva)

                Me.GeneraFileIV("IV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalBase, CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), TotalIva, CType(Me.DbLeeHotel.mDbLector("X"), String))


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Detalle de Impuesto")
        End Try
    End Sub
    Private Sub FacturasSalidaCancelaciondeAnticipos()



        Dim Total As Double
        Dim TotalCancelados As Double

        Dim SQL As String

        SQL = "SELECT 'Anticipo Reserv= ' ||TNHT_MOVI.RESE_CODI||'/'||TNHT_MOVI.RESE_ANCI RESERVA,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA, "
        'SQL = "SELECT "
        SQL += "SUM(TNHT_MOVI.MOVI_VDEB) TOTAL,"
        SQL = SQL & "TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA"
        SQL = SQL & " FROM TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO"

        SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        ' NUEVO POR AJUSTE DE RENDIMIENTO 
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) AND "
        SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) AND"
     
        ' FIN DE NUEVO 

        

        SQL = SQL & " TNHT_MOVI.MOVI_DAVA  < TNHT_FACT.FACT_DAEM      AND"
        SQL = SQL & " TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND (FACT_DAAN IS NULL OR FACT_DAEM < FACT_DAAN)"
        SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '1'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0 AND"

        '  SQL = SQL & " AND TNHT_MOVI.RESE_CODI IS NOT NULL"
        SQL = SQL & " TNHT_FACT.FACT_STAT = '1'  AND TNHT_FACT.FACT_CLEN = '1' "

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If
        ' EXCLUYE LIQUIDACIONES DE CONTADO NO FACTURADAS
        SQL += "AND TNHT_MOVI.UTIL_CODI <> 'POS'"

        SQL = SQL & " GROUP BY TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI,TNHT_MOVI.MOVI_DAVA,TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI,TNHT_MOVI.MOVI_CODI"
        SQL += " ORDER BY TNHT_MOVI.MOVI_DAVA "

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalCancelados = TotalCancelados + Total
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String) & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
        Me.mCancelacionAnticipos = TotalCancelados
    End Sub

    Private Sub FacturasSalidaDevoluciondeAnticipos()


        Dim Total As Double
        Dim TotalDevueltos As Double

        SQL = "SELECT 'Devolución Anticipo Reserv= ' ||TNHT_MOVI.RESE_CODI||'/'||TNHT_MOVI.RESE_ANCI RESERVA,SUM(TNHT_MOVI.MOVI_VDEB) TOTAL"
        SQL = SQL & " FROM TNHT_MOVI,TNHT_FACT,TNHT_FAMO"

        SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR  <= TNHT_FACT.FACT_DAEM      AND"
        SQL = SQL & " TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND (FACT_DAAN IS NULL OR FACT_DAEM < FACT_DAAN)"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '5'"
        '  SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"

        ' excluir depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        SQL = SQL & " AND TNHT_MOVI.RESE_CODI IS NOT NULL"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1'  AND TNHT_FACT.FACT_CLEN = '1'"
        SQL = SQL & " GROUP BY TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalDevueltos = TotalDevueltos + Total
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
        Me.mDevolucionAnticipos = TotalDevueltos
    End Sub
    Private Sub FacturasSalidaTpvNoFacturado()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,RPAD(SECC_DESC,15) || 'Val. ' || MOVI_DATR || '  ' || FORE_DESC AS DESCRIPCION, "
        SQL += "FACT_DAEM,TIRE_CODI,TNHT_FORE.FORE_CODI,FORE_DESC,NVL(FORE_CTB1,'0') AS CUENTA "
        SQL += "FROM TNHT_MOVI, TNHT_FACT, TNHT_SECC, TNHT_FORE "
        SQL += "WHERE TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND "
        SQL += "TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI AND "
        SQL += "TNHT_MOVI.SECC_CODI = TNHT_SECC.SECC_CODI(+) AND "
        SQL += "TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI AND "
        SQL += "TNHT_MOVI.MOVI_DATR  < TNHT_FACT.FACT_DAEM AND "
        SQL += "TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        SQL += "AND (FACT_DAAN IS NULL OR FACT_DAEM < FACT_DAAN) "

        SQL += "AND TNHT_MOVI.MOVI_TIMO = '2' "
        SQL += "AND TNHT_MOVI.TIRE_CODI  = '1' "
        SQL += "AND TNHT_MOVI.MOVI_VDEB <> 0 "
        SQL += "AND TNHT_MOVI.MOVI_DEAN ='0' "
        '  SQL += "AND TNHT_MOVI.MOVI_CORR = 0 "
        SQL += "AND TNHT_FACT.FACT_STAT = '1' AND TNHT_FACT.FACT_CLEN = '1' "
        SQL += "AND TNHT_MOVI.UTIL_CODI = 'POS'"



        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, "* ==>> !!!! " & CType(Me.DbLeeHotel.mDbLector("FORE_DESC"), String), Total, "NO", "", CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("FORE_DESC"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
#End Region
#Region "ASIENTO-3A CANCELACION DE FACTURAS DE CONTADO"
    Private Sub FacturasSalidaTotalLiquidoAnuladas()
        Try

            Dim Total As Double
            Dim TotalComisiones As Double
            Dim SQL As String


            SQL = "SELECT (SUM(FAIV_INCI) - SUM(FAIV_VIMP)) TOTAL ,SUM(FACT_TOTA) TOTAL1,FACT_DAAN "
            SQL += "FROM TNHT_FAIV, TNHT_FACT "
            SQL += "WHERE TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "
            SQL += "AND FACT_DAEM < FACT_DAAN "
            SQL += "AND TNHT_FACT.FACT_STAT = '1' AND FACT_CLEN = '1'"
            SQL += "GROUP BY TNHT_FACT.FACT_DAAN"


            ' OJO LLAMADA A SERVICIOS SIN IGIC  Y DESEMBOLSOS ANULADOS 

            If IsDBNull(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = False Then
                Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double) + Me.FacturacionSalidasServiciosSinIgic + Me.FacturacionSalidaDesembolsos

                If mDebug = True Then
                    Me.LiquidoServiciosConIgic = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
                    Me.LiquidoServiciosSinIgic = Me.FacturacionSalidasServiciosSinIgic
                    Me.LiquidoDesembolsos = Me.FacturacionSalidaDesembolsos

                End If
            Else
                Total = 0
            End If

            ' tinglado de sumar las comisiones al total liquido

            SQL = "SELECT nvl(SUM(TNHT_DESF.DESF_VALO),'0')TOTAL "
            SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE WHERE"
            SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
            SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
            SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
            SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1'  AND FACT_CLEN = '1' "
            SQL = SQL & " AND TNHT_FACT.FACT_DAAN= " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND FACT_DAEM < FACT_DAAN"
            '' solo santana cazorla 
            If Me.mParaSantana = 1 Then
                SQL = SQL & " AND FACT_DAEM > '15-03-2007' "
            End If




            If Me.mParaComisiones = True Then
                TotalComisiones = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
                Total = Total + TotalComisiones
            End If
            Total = Decimal.Round(CType(Total, Decimal), 2)




            If Total <> 0 Then
                Linea = 1
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorDebe, "FACTURACIÓN LÍQUIDA " & Me.mFecha & " + Comisiones ", Total, "SI", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorDebe, "Total Liquido", Total)

                ' TRUCO TIBERIO 
                Me.GeneraFileAC3("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaClientesContado, Me.mIndicadorHaberFac, "FACTURACION CONTADO", Total)
                ' truco para vincular mas de una factura a un solo asiento 
                Me.FacturasSalidaTotalFActuraVFAnuladas()
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaClientesContado, Me.mIndicadorDebe, "FACTURACION CONTADO", Total)
                ' FIN TRUCO

                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaClientesContado, Me.mIndicadorHaber, "FACTURACION CONTADO", Total, "NO", "", "", "SI")


                Linea = Linea + 1
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaClientesContado, Me.mIndicadorDebe, "FACTURACION CONTADO", Total, "NO", "", "", "SI")
            End If

        Catch EX As Exception
            MsgBox(EX.Message)
        End Try



    End Sub
    Private Sub FacturasSalidaTotalFActuraVFAnuladas()
        Try
            Dim TotalFactura As Double
            SQL = "SELECT   TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,TNHT_FACT.FACT_TOTA TOTAL "
            SQL += "FROM TNHT_FACT "
            SQL += "WHERE "
            SQL += "TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "
            SQL += "AND FACT_DAEM < FACT_DAAN "
            SQL += "AND (TNHT_FACT.FACT_STAT = '1' AND FACT_CLEN = '1' ) "
            SQL += "ORDER BY TNHT_FACT.SEFA_CODI ASC, TNHT_FACT.FACT_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1


                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)



                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("VF", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, "NO TRATAR", Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Me.mClientesContadoCif, "", "NO")
                Me.GeneraFileVF("VF", 31, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSerieAnulacion & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalFactura, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String).PadRight(15, CChar(" ")), "TIBERIO", Me.mClientesContadoCif)


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas")
        End Try
    End Sub


    Private Sub FacturasSalidaTotalVisasAnuladas()
        Dim Total As Double
        SQL = "SELECT SUM(MOVI_VDEB) TOTAL,CACR_DESC TARJETA,nvl(CACR_CONT,'0') CUENTA FROM TNHT_MOVI,TNHT_CACR,TNHT_FACT,TNHT_FAMO WHERE"

        SQL = SQL & " TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1' AND FACT_CLEN = '1' "
        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = FACT_DAEM"
        SQL = SQL & " AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND FACT_DAEM < FACT_DAAN"
        SQL = SQL & " GROUP BY TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CONT"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasSalidaTotaLOtrasFormasAnuladas()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM TNHT_MOVI,TNHT_FORE,TNHT_FACT,TNHT_FAMO WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "

        SQL = SQL & " AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1'  AND FACT_CLEN = '1' "
        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = FACT_DAEM"
        SQL = SQL & " AND TNHT_FACT.FACT_DAAN= " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND FACT_DAEM < FACT_DAAN"

        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasSalidaTotaLDescuentosAnuladas()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_DESF.DESF_VALO)TOTAL,TNHT_TIDE.TIDE_DESC TIPO,NVL(TNHT_TIDE.TIDE_CTB1,'0') CUENTA "
        SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE WHERE"
        SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
        SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1'  AND FACT_CLEN = '1' "
        SQL = SQL & " AND TNHT_FACT.FACT_DAAN= " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND FACT_DAEM < FACT_DAAN"
        SQL = SQL & " GROUP BY TNHT_TIDE.TIDE_DESC,TNHT_TIDE.TIDE_CTB1"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)
            Me.GeneraFileAA("AA", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", Me.mParaCentroCostoAlojamiento, Total)


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasSalidaTotalFActuraAnuladas()
        Try
            Dim TotalFactura As Double
            Dim Dni As String

            SQL = "SELECT   TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,TNHT_FACT.FACT_TOTA TOTAL,NVL(CLIE_NUID,'0') AS DNI "
            SQL += "FROM TNHT_FACT,TNHT_CLIE "
            SQL += "WHERE "
            SQL += "TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "
            SQL += "AND FACT_DAEM < FACT_DAAN "
            SQL += "AND (TNHT_FACT.FACT_STAT = '1'  AND TNHT_FACT.FACT_CLEN = '1') "
            SQL += "AND TNHT_FACT.CLIE_CODI = TNHT_CLIE.CLIE_CODI(+) "
            SQL += "ORDER BY TNHT_FACT.SEFA_CODI ASC, TNHT_FACT.FACT_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1

                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

                If CType(Me.DbLeeHotel.mDbLector("DNI"), String) = "0" Then
                    Dni = Me.mClientesContadoCif
                Else
                    Dni = CType(Me.DbLeeHotel.mDbLector("DNI"), String).Trim
                End If

                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("FV", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaClientesContado, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), (TotalFactura * -1), "NO", Dni, Dni, "NO")
                Me.GeneraFileFV2("FV", 31, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSerieAnulacion & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), (TotalFactura * -1), CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String).PadRight(15, CChar(" ")), Me.mCtaClientesContado, Dni)


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas")
        End Try
    End Sub
    Private Sub FacturasSalidaDetalleIgicAnuladas()

        Try

            Dim TotalIva As Double
            Dim TotalBase As Double
            Dim TotalFactura As Double
            SQL = "SELECT   TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE,"
            SQL += "TNHT_FAIV.FAIV_TAXA AS TIPO, TNHT_FAIV.FAIV_INCI,(FAIV_INCI-FAIV_VIMP) BASE, TNHT_FAIV.FAIV_VIMP IGIC,NVL(TIVA_CTB1,'0') CUENTA, '"
            SQL += Me.mParaTextoIva & " ' || FAIV_TAXA ||'%  '|| TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,TNHT_FACT.FACT_TOTA TOTAL,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X "
            SQL += "FROM TNHT_FAIV, TNHT_FACT,TNHT_TIVA "
            SQL += "WHERE TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "AND TNHT_FAIV.TIVA_CODI = TNHT_TIVA.TIVA_CODI "
            SQL += "AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "
            SQL += "AND FACT_DAEM < FACT_DAAN "
            SQL += "AND (TNHT_FACT.FACT_STAT = '1'  AND TNHT_FACT.FACT_CLEN = '1') "
            SQL += "ORDER BY TNHT_FACT.SEFA_CODI ASC, TNHT_FACT.FACT_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
                TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)

                TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
                TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

                ' TIBERIO ( EL TOTAL FACTURA ES LA BASE MAS EL IGIC )   O LOS MAS LOS DESEMBOSOS Y SERVICIOS SIN IGIC 
                'TotalFactura = Decimal.Round(CType(TotalBase + TotalIva, Decimal), 2)
                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)



                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva, "NO", Me.mClientesContadoCif, "", "SI")

                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva)
                Me.GeneraFileIV("IV", 31, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSerieAnulacion & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), (TotalBase * -1), CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), (TotalIva * -1), CType(Me.DbLeeHotel.mDbLector("X"), String))


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Detalle de Impuesto")
        End Try
    End Sub
    Private Sub FacturasSalidaCancelaciondeAnticiposAnuladas()



        Dim Total As Double
        Dim TotalCancelados As Double

        Dim SQL As String

        SQL = "SELECT 'Anticipo Reserv= ' ||TNHT_MOVI.RESE_CODI||'/'||TNHT_MOVI.RESE_ANCI RESERVA,SUM(TNHT_MOVI.MOVI_VDEB) TOTAL,"
        SQL = SQL & "TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA"
        SQL = SQL & " FROM TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO"

        SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        ' NUEVO POR AJUSTE DE RENDIMIENTO 
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) AND "
        SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) AND "
      
        ' FIN DE NUEVO 

        'SQL = SQL & " TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        'SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI AND"

        SQL = SQL & " TNHT_MOVI.MOVI_DAVA  < TNHT_FACT.FACT_DAEM      AND"
        SQL = SQL & " TNHT_FACT.FACT_DAAN= " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND FACT_DAEM < FACT_DAAN"
        SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '1'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0 "

        'SQL = SQL & " AND TNHT_MOVI.RESE_CODI IS NOT NULL"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1'  AND TNHT_FACT.FACT_CLEN = '1' "

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        ' EXCLUYE LIQUIDACIONES DE CONTADO NO FACTURADAS
        SQL += "AND TNHT_MOVI.UTIL_CODI <> 'POS'"

        SQL = SQL & " GROUP BY TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI,TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalCancelados = TotalCancelados + Total
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
        Me.mCancelacionAnticipos = TotalCancelados
    End Sub

    Private Sub FacturasSalidaDevoluciondeAnticiposAnuladas()


        Dim Total As Double
        Dim TotalDevueltos As Double

        SQL = "SELECT 'Devolución Anticipo Reserv= ' ||TNHT_MOVI.RESE_CODI||'/'||TNHT_MOVI.RESE_ANCI RESERVA,SUM(TNHT_MOVI.MOVI_VDEB) TOTAL"
        SQL = SQL & " FROM TNHT_MOVI,TNHT_FACT,TNHT_FAMO"

        SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR  <= TNHT_FACT.FACT_DAEM      AND"
        SQL = SQL & " TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND FACT_DAEM < FACT_DAAN"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '5'"
        '  SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"

        ' excluir depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        SQL = SQL & " AND TNHT_MOVI.RESE_CODI IS NOT NULL"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1'  AND TNHT_FACT.FACT_CLEN = '1'"
        SQL = SQL & " GROUP BY TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalDevueltos = TotalDevueltos + Total
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
        Me.mDevolucionAnticipos = TotalDevueltos
    End Sub
    Private Sub FacturasSalidaTpvNoFacturadoAnuladas()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,RPAD(SECC_DESC,15) || 'Val. ' || MOVI_DATR || '  ' || FORE_DESC AS DESCRIPCION, "
        SQL += "FACT_DAEM,TIRE_CODI,TNHT_FORE.FORE_CODI,FORE_DESC,NVL(FORE_CTB1,'0') AS CUENTA "
        SQL += "FROM TNHT_MOVI, TNHT_FACT, TNHT_SECC, TNHT_FORE "
        SQL += "WHERE TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND "
        SQL += "TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI AND "
        SQL += "TNHT_MOVI.SECC_CODI = TNHT_SECC.SECC_CODI(+) AND "
        SQL += "TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI AND "
        SQL += "TNHT_MOVI.MOVI_DATR  < TNHT_FACT.FACT_DAEM AND "
        SQL += "TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "
        SQL += "AND FACT_DAEM < FACT_DAAN "

        SQL += "AND TNHT_MOVI.MOVI_TIMO = '2' "
        SQL += "AND TNHT_MOVI.TIRE_CODI  = '1' "
        SQL += "AND TNHT_MOVI.MOVI_VDEB <> 0 "
        SQL += "AND TNHT_MOVI.MOVI_DEAN ='0' "
        '  SQL += "AND TNHT_MOVI.MOVI_CORR = 0 "
        SQL += "AND TNHT_FACT.FACT_STAT = '1' AND TNHT_FACT.FACT_CLEN = '1' "
        SQL += "AND TNHT_MOVI.UTIL_CODI = 'POS'"



        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "* ==>> !!!! " & CType(Me.DbLeeHotel.mDbLector("FORE_DESC"), String), Total, "NO", "", CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("FORE_DESC"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
#End Region
#Region "ASIENTO-4"
    Private Sub Desembolsos()



        Dim Resultado As String
        Dim TotalDesembolsos As Double
        '__________________________________________________________________________________________
        ' CALCULO DEL TOTAL DE DESEMBOLSOS REALIZADOS EN EL DIA 
        '__________________________________________________________________________________________
        Dim TotalDia As Double

        SQL = "SELECT ROUND(SUM(TNHT_MOVI.MOVI_VDEB),2) TOTAL"
        SQL = SQL & " FROM TNHT_MOVI WHERE"
        SQL = SQL & " TNHT_MOVI.MOVI_DATR =" & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TIRE_CODI = '4'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        'DEBAJO PARA CONTROL DE ERROR RARO DE DESEMBOLSOS POSITIVOS NO SE POR QUE LOS HAY
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB < 0"


        Resultado = Me.DbLeeHotel.EjecutaSqlScalar(SQL)
        If (IsNumeric(Resultado) = True) Then
            TotalDia = CType(Resultado, Double) * -1
        Else
            TotalDia = 0
        End If

        '__________________________________________________________________________________________
        ' CALCULO DEL TOTAL DE DESEMBOLSOS ANULADOS EN EL DIA 
        '__________________________________________________________________________________________

        Dim TotalDiaAnulados As Double

        SQL = "SELECT ROUND(SUM(TNHT_MOVI.MOVI_VDEB),2) TOTAL"
        SQL = SQL & " FROM TNHT_MOVI WHERE"
        SQL = SQL & " TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA < " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TIRE_CODI = '4'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 1"


        Resultado = Me.DbLeeHotel.EjecutaSqlScalar(SQL)
        If (IsNumeric(Resultado) = True) Then
            TotalDiaAnulados = CType(Resultado, Double) * -1
        Else
            TotalDiaAnulados = 0
        End If

        '--------------------------------------------------------------
        TotalDesembolsos = TotalDia + TotalDiaAnulados

        If TotalDesembolsos = 0 Then Exit Sub
        If TotalDesembolsos > 0 Then
            Linea = 1
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 4, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorDebe, "Desembolsos +", TotalDesembolsos, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorDebe, "Desembolsos +", TotalDesembolsos)
            Linea = 2
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 4, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaDesembolsos, Me.mIndicadorHaber, "Desembolsos +", TotalDesembolsos, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaDesembolsos, Me.mIndicadorHaber, "Desembolsos +", TotalDesembolsos)

        Else
            Linea = 1
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 4, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "Desembolsos -", TotalDesembolsos, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, "Desembolsos -", TotalDesembolsos)

            Linea = 2
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 4, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaDesembolsos, Me.mIndicadorDebe, "Desembolsos -", TotalDesembolsos, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaDesembolsos, Me.mIndicadorDebe, "Desembolsos -", TotalDesembolsos)

        End If

    End Sub
#End Region
#Region "ASIENTO-5"
    Private Sub FacturasEntidadTotalLiquido()


        Dim Total As Double
        Dim TotalComisiones As Double


        SQL = "SELECT (SUM(FAIV_INCI) - SUM(FAIV_VIMP)) TOTAL ,FACT_DAEM "
        SQL += "FROM TNHT_FAIV, TNHT_FACT,TNHT_ENTI "
        SQL += "WHERE TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL += "AND TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
        SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
        SQL += "AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        SQL += "AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += "GROUP BY TNHT_FACT.FACT_DAEM"


        If IsDBNull(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = False Then
            Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            '  + Me.FacturacionCreditoDesembolsos + Me.FacturacionCreditoServiciosSinIgic
        Else
            Total = 0
        End If

        ' tinglado de sumar las comisiones al total liquido
        SQL = "SELECT nvl(SUM(TNHT_DESF.DESF_VALO),'0')TOTAL "
        SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE,TNHT_ENTI WHERE"
        SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
        SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        If Me.mParaSantana = 1 Then
            SQL = SQL & " AND FACT_DAEM > '15-03-2007' "
        End If


        If Me.mParaComisiones = True Then
            TotalComisiones = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            Total = Total + TotalComisiones
        End If
        Total = Decimal.Round(CType(Total, Decimal), 2)


        If Total <> 0 Then
            Linea = 1
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "FACTURACIÓN LÍQUIDA " & Me.mFecha & " + Comisiones ", Total, "SI", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, "Total Liquido", Total)
        End If



    End Sub
    Private Sub FacturasEntidadTotalVisas()
        Dim Total As Double
        SQL = "SELECT SUM(MOVI_VDEB) TOTAL,CACR_DESC TARJETA,nvl(CACR_CONT,'0') CUENTA FROM TNHT_MOVI,TNHT_CACR,TNHT_FACT,TNHT_FAMO,TNHT_ENTI WHERE"
        SQL = SQL & " TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = TNHT_FACT.FACT_DAEM"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL = SQL & " GROUP BY TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CONT"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasEntidadTotaLOtrasFormas()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM TNHT_MOVI,TNHT_FORE,TNHT_FACT,TNHT_FAMO,TNHT_ENTI WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = TNHT_FACT.FACT_DAEM"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasEntidadTotaLDescuentos()
        Dim Total As Double

        SQL = "SELECT SUM(TNHT_DESF.DESF_VALO)TOTAL,TNHT_TIDE.TIDE_DESC TIPO,NVL(TNHT_TIDE.TIDE_CTB1,'0') CUENTA,TNHT_FACT.FACT_CODI FACTURA,NVL(TNHT_ENTI.ENTI_COCO,'0') CUENTA2 "
        SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE,TNHT_ENTI WHERE"
        SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
        SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL = SQL & " GROUP BY TNHT_TIDE.TIDE_DESC,TNHT_TIDE.TIDE_CTB1,TNHT_ENTI.ENTI_COCO,TNHT_FACT.FACT_CODI"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            ' USA LA CUENTA CONTABLE DEL DESCUENTO FINANCIERO 
            If mParaUsaCtaComision = 0 Then
                Me.InsertaOracle("AC", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)
                Me.GeneraFileAA("AA", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", Me.mParaCentroCostoAlojamiento, Total)
            Else
                'USA LA CUENTA CONTABLE DE COMISIONES DEFINIDA EN LA ENTIDAD  
                Me.InsertaOracle("AC", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)
                Me.GeneraFileAA("AA", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), "0", Me.mParaCentroCostoAlojamiento, Total)
            End If

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasEntidadCredito()


        Dim Total As Double
        Dim TotalPendiente As Double
        Dim TotalDiferencia As Double

        SQL = "SELECT"
        SQL += " TNHT_FACT.SEFA_CODI AS SERIE, TNHT_FACT.FACT_CODI AS NUMERO,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA,FACT_TOTA TOTAL,TNHT_FACT.FACT_CONT PENDIENTE, "
        SQL += " FACT_TITU, FACT_DAEM,NVL(ENTI_NCON,0) CUENTA ,NVL(ENTI_NUCO,0) CIF,NVL(ENTI_NOME,'?') AS NOMBRE"
        SQL += " FROM TNHT_FACT, TNHT_ENTI"
        SQL += " WHERE TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL += " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += " ORDER BY TNHT_FACT.FACT_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalPendiente = CType(Me.DbLeeHotel.mDbLector("PENDIENTE"), Double)
            Total = Decimal.Round(CType(Total, Decimal), 2)
            TotalPendiente = Decimal.Round(CType(TotalPendiente, Decimal), 2)

            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI")
            Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer))
            '  Me.GeneraFileFV("FV", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), Total, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String).PadRight(15, CChar(" ")), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), CType(Me.DbLeeHotel.mDbLector("CIF"), String), TotalPendiente)
            ' h lopez
            Me.GeneraFileFV("FV", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), Total, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String).PadRight(15, CChar(" ")), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), CType(Me.DbLeeHotel.mDbLector("CIF"), String), Total)

            ' nunca en lopez 
            If Total > TotalPendiente Then
                Linea = Linea + 1
                TotalDiferencia = Total - TotalPendiente
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), TotalDiferencia, "NO", CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), TotalDiferencia)
            End If
        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasEntidadCreditoDetalleIgic()
        Dim TotalIva As Double
        Dim TotalBase As Double
        Dim Totalfactura As Double
        SQL = "SELECT"
        SQL += " TNHT_FACT.SEFA_CODI AS SERIE , TNHT_FACT.FACT_CODI AS NUMERO,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI,FAIV_INCI TOTAL,(FAIV_INCI-FAIV_VIMP) BASE,FAIV_VIMP IGIC, '"
        SQL += Me.mParaTextoIva & " ' || FAIV_TAXA ||'%  '|| TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION, FACT_DAEM,FAIV_TAXA TIPO,NVL(TIVA_CTB1,'0') "
        SQL += " CUENTA ,NVL(ENTI_NCON,0) CUENTACLIENTE ,NVL(ENTI_NUCO,0) CIF ,TNHT_ENTI.ENTI_CODI TOTAL,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X  FROM TNHT_FACT, TNHT_ENTI,TNHT_FAIV,TNHT_TIVA"
        SQL += " WHERE TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI AND "
        SQL += " TNHT_FACT.FACT_CODI = TNHT_FAIV.FACT_CODI    AND"
        SQL += " TNHT_FACT.SEFA_CODI = TNHT_FAIV.SEFA_CODI"
        SQL += " AND TNHT_FAIV.TIVA_CODI = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += " ORDER BY TNHT_FAIV.FACT_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
            TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva)
            Me.GeneraFileIV("IV", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalBase, CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), TotalIva, CType(Me.DbLeeHotel.mDbLector("X"), String))


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasEntidadCancelaciondeAnticipos()



        Dim Total As Double
        Dim TotalCancelados As Double



        SQL = "SELECT 'Anticipo Reserv= ' ||TNHT_MOVI.RESE_CODI||'/'||TNHT_MOVI.RESE_ANCI RESERVA,SUM(TNHT_MOVI.MOVI_VDEB) TOTAL,"
        SQL = SQL & "TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA "
        SQL = SQL & " FROM TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO,TNHT_ENTI"

        SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        ' NUEVO POR AJUSTE DE RENDIMIENTO 
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI AND "
        SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI AND "
        SQL = SQL & " TNHT_MOVI.MOVI_DAVA < TNHT_RESE.RESE_DASA AND  "
        ' FIN DE NUEVO 

        'SQL = SQL & " TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        'SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI AND"

        SQL = SQL & " TNHT_MOVI.MOVI_DAVA  < TNHT_FACT.FACT_DAEM      AND"
        SQL = SQL & " TNHT_FACT.FACT_DAEM= " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '1'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        'SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI IS NOT NULL"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT IN ('2','3')"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        SQL = SQL & " GROUP BY TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI,TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA"
        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalCancelados = TotalCancelados + Total
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
        Me.mCancelacionAnticipos = TotalCancelados
    End Sub

#End Region
#Region "ASIENTO-6"
    Private Sub FacturasEntidadTotalLiquidoAnuladas()



        Dim Total As Double
        Dim TotalComisiones As Double

        'SQL = "SELECT SUM(MOVI_VLIQ)TOTAL "
        'SQL += "FROM VH_MIVA,TNHT_FACT,TNHT_ENTI "
        'SQL += "WHERE "
        'SQL += "VH_MIVA.SEFA_CODI = TNHT_FACT.SEFA_CODI "
        'SQL += "AND VH_MIVA.FACT_CODI = TNHT_FACT.FACT_CODI "
        'SQL += "AND TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        'SQL += "AND MOVI_CORR = '0' "
        'SQL += "AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        'SQL += "AND TNHT_FACT.FACT_STAT = '2' "
        'SQL += "AND (TNHT_FACT.FACT_ANUL = '0' OR TNHT_FACT.FACT_DAEM < FACT_DAAN) "
        'SQL += "GROUP BY TNHT_FACT.FACT_DAEM"





        SQL = "SELECT (SUM(FAIV_INCI) - SUM(FAIV_VIMP)) TOTAL ,FACT_DAAN "
        SQL += "FROM TNHT_FAIV, TNHT_FACT,TNHT_ENTI "
        SQL += "WHERE TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL += "AND TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
        SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
        SQL += "AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "
        SQL += "AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += "GROUP BY TNHT_FACT.FACT_DAAN"


        If IsDBNull(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = False Then
            Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            '  + Me.FacturacionCreditoDesembolsos + Me.FacturacionCreditoServiciosSinIgic
            Total = Decimal.Round(CType(Total, Decimal), 2)
        Else
            Total = 0
        End If

        ' tinglado de sumar las comisiones al total liquido
        SQL = "SELECT nvl(SUM(TNHT_DESF.DESF_VALO),'0')TOTAL "
        SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE,TNHT_ENTI WHERE"
        SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
        SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_FACT.FACT_DAAN= " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        '' solo santana cazorla 
        If Me.mParaSantana = 1 Then
            SQL = SQL & " AND FACT_DAEM > '15-03-2007' "
        End If




        If Me.mParaComisiones = True Then
            TotalComisiones = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            Total = Total + TotalComisiones
        End If
        Total = Decimal.Round(CType(Total, Decimal), 2)


        If Total <> 0 Then
            Linea = 1
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorDebe, "FACTURACIÓN LÍQUIDA " & Me.mFecha & " + Comisiones ", Total, "SI", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorDebe, "Total Liquido + Comisiones", Total)
        End If



    End Sub
    Private Sub FacturasEntidadTotalVisasAnuladas()
        Dim Total As Double
        SQL = "SELECT SUM(MOVI_VDEB) TOTAL,CACR_DESC TARJETA,nvl(CACR_CONT,'0') CUENTA FROM TNHT_MOVI,TNHT_CACR,TNHT_FACT,TNHT_FAMO,TNHT_ENTI WHERE"
        SQL = SQL & " TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = TNHT_FACT.FACT_DAEM"
        SQL = SQL & " AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL = SQL & " GROUP BY TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CONT"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasEntidadTotaLOtrasFormasAnuladas()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM TNHT_MOVI,TNHT_FORE,TNHT_FACT,TNHT_FAMO,TNHT_ENTI WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = TNHT_FACT.FACT_DAEM"
        SQL = SQL & " AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasEntidadTotaLDescuentosAnuladas()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_DESF.DESF_VALO)TOTAL,TNHT_TIDE.TIDE_DESC TIPO,NVL(TNHT_TIDE.TIDE_CTB1,'0') CUENTA,TNHT_FACT.FACT_CODI FACTURA,NVL(TNHT_ENTI.ENTI_COCO,'0') CUENTA2 "
        SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE,TNHT_ENTI WHERE"
        SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
        SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_FACT.FACT_DAAN= " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL = SQL & " GROUP BY TNHT_TIDE.TIDE_DESC,TNHT_TIDE.TIDE_CTB1,TNHT_ENTI.ENTI_COCO,TNHT_FACT.FACT_CODI"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            ' USA LA CUENTA CONTABLE DEL DESCUENTO FINANCIERO 
            If mParaUsaCtaComision = 0 Then
                Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)
                Me.GeneraFileAA("AA", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", Me.mParaCentroCostoAlojamiento, Total)
            Else
                'USA LA CUENTA CONTABLE DE COMISIONES DEFINIDA EN LA ENTIDAD  
                Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)
                Me.GeneraFileAA("AA", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), "0", Me.mParaCentroCostoAlojamiento, Total)
            End If

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasEntidadCreditoAnuladas()


        Dim Total As Double
        Dim TotalPendiente As Double
        Dim TotalDiferencia As Double
        SQL = "SELECT"
        SQL += " TNHT_FACT.SEFA_CODI AS SERIE, TNHT_FACT.FACT_CODI AS NUMERO,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA,FACT_TOTA TOTAL,TNHT_FACT.FACT_CONT PENDIENTE, "
        SQL += " FACT_TITU, FACT_DAEM,NVL(ENTI_NCON,0) CUENTA ,NVL(ENTI_NUCO,0) CIF,NVL(ENTI_NOME,'?') AS NOMBRE"
        SQL += " FROM TNHT_FACT, TNHT_ENTI"
        SQL += " WHERE TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL += " AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "
        SQL += " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += " ORDER BY TNHT_FACT.FACT_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalPendiente = CType(Me.DbLeeHotel.mDbLector("PENDIENTE"), Double)
            Total = Decimal.Round(CType(Total, Decimal), 2)
            TotalPendiente = Decimal.Round(CType(TotalPendiente, Decimal), 2)

            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaberFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI")
            Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaberFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, Me.mParaSerieAnulacion & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer))
            ' OJO !!!!!!!
            Me.InsertaOracle("FV", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaClientesContado, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), (Total * -1), "NO", CType(Me.DbLeeHotel.mDbLector("CIF"), String), "", "NO")
            Me.GeneraFileFV("FV", 6, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSerieAnulacion & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), (Total * -1), CType(Me.DbLeeHotel.mDbLector("FACTURA"), String).PadRight(15, CChar(" ")), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), CType(Me.DbLeeHotel.mDbLector("CIF"), String), (TotalPendiente * -1))

            If Total <> TotalPendiente Then
                Linea = Linea + 1
                TotalDiferencia = Total - TotalPendiente
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), TotalDiferencia, "NO", CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), TotalDiferencia)
            End If

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub

    Private Sub FacturasEntidadCreditoDetalleIgicAnuladas()
        Dim TotalIva As Double
        Dim TotalBase As Double
        Dim Totalfactura As Double
        SQL = "SELECT"
        SQL += " TNHT_FACT.SEFA_CODI AS SERIE , TNHT_FACT.FACT_CODI AS NUMERO,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI,FAIV_INCI TOTAL,(FAIV_INCI-FAIV_VIMP) BASE,FAIV_VIMP IGIC, '"
        SQL += Me.mParaTextoIva & " ' || FAIV_TAXA ||'%  '|| TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION, FACT_DAEM,FAIV_TAXA TIPO,NVL(TIVA_CTB1,'0') "
        SQL += " CUENTA ,NVL(ENTI_NCON,0) CUENTACLIENTE ,NVL(ENTI_NUCO,0) CIF ,TNHT_ENTI.ENTI_CODI TOTAL,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X  FROM TNHT_FACT, TNHT_ENTI,TNHT_FAIV,TNHT_TIVA"
        SQL += " WHERE TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI AND "
        SQL += " TNHT_FACT.FACT_CODI = TNHT_FAIV.FACT_CODI    AND"
        SQL += " TNHT_FACT.SEFA_CODI = TNHT_FAIV.SEFA_CODI"
        SQL += " AND TNHT_FAIV.TIVA_CODI = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "
        SQL += " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += " ORDER BY TNHT_FAIV.FACT_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
            TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva)
            Me.GeneraFileIV("IV", 6, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSerieAnulacion & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), (TotalBase * -1), CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), (TotalIva * -1), CType(Me.DbLeeHotel.mDbLector("X"), String))


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasEntidadCancelaciondeAnticiposAnuladas()



        Dim Total As Double
        Dim TotalCancelados As Double

        SQL = "SELECT 'Anticipo Reserv= ' ||TNHT_MOVI.RESE_CODI||'/'||TNHT_MOVI.RESE_ANCI RESERVA,SUM(TNHT_MOVI.MOVI_VDEB) TOTAL,"
        SQL = SQL & "TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA"
        SQL = SQL & " FROM TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO,TNHT_ENTI"

        SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        ' NUEVO POR AJUSTE DE RENDIMIENTO 
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI AND "
        SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI AND "
        SQL = SQL & " TNHT_MOVI.MOVI_DAVA < TNHT_RESE.RESE_DASA AND  "
        ' FIN DE NUEVO 

        'SQL = SQL & " TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        'SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI AND"

        SQL = SQL & " TNHT_MOVI.MOVI_DAVA  < TNHT_FACT.FACT_DAEM      AND"
        SQL = SQL & " TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '1'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        'SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI IS NOT NULL"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT IN ('2','3')"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        SQL = SQL & " GROUP BY TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI,TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalCancelados = TotalCancelados + Total
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
        Me.mCancelacionAnticipos = TotalCancelados
    End Sub

#End Region

#Region "ASIENTO-7 FACTURAS NO ALOJADO"
    Private Sub FacturasNoAlojadoTotalLiquido()
        Dim Total As Double
        Dim TotalComisiones As Double

        SQL = "SELECT (SUM(FAIV_INCI) - SUM(FAIV_VIMP)) TOTAL ,FACT_DAEM "
        SQL += "FROM TNHT_FAIV, TNHT_FACT,TNHT_CCEX "
        SQL += "WHERE TNHT_FACT.CCEX_CODI = TNHT_CCEX.CCEX_CODI AND TNHT_FACT.ENTI_CODI IS NULL "
        SQL += "AND TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
        SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
        SQL += "AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        SQL += "AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += "GROUP BY TNHT_FACT.FACT_DAEM"


        If IsDBNull(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = False Then
            Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            '  + Me.FacturacionCreditoDesembolsos + Me.FacturacionCreditoServiciosSinIgic
            Total = Decimal.Round(CType(Total, Decimal), 2)
        Else
            Total = 0
        End If

        ' tinglado de sumar las comisiones al total liquido
        SQL = "SELECT nvl(SUM(TNHT_DESF.DESF_VALO),'0')TOTAL "
        SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE,TNHT_CCEX WHERE"
        SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
        SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_FACT.CCEX_CODI = TNHT_CCEX.CCEX_CODI  AND TNHT_FACT.ENTI_CODI IS NULL "
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        If Me.mParaSantana = 1 Then
            SQL = SQL & " AND FACT_DAEM > '15-03-2007' "
        End If


        If Me.mParaComisiones = True Then
            TotalComisiones = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            Total = Total + TotalComisiones
        End If
        Total = Decimal.Round(CType(Total, Decimal), 2)

        If Total <> 0 Then
            Linea = 1
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "Total Liquido + Total Comisiones", Total, "SI", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, "Total Liquido + Total Comisiones", Total)
        End If



    End Sub
    Private Sub FacturasNoAlojadoTotalVisas()
        Dim Total As Double
        SQL = "SELECT SUM(MOVI_VDEB) TOTAL,CACR_DESC TARJETA,nvl(CACR_CONT,'0') CUENTA FROM TNHT_MOVI,TNHT_CACR,TNHT_FACT,TNHT_FAMO,TNHT_CCEX WHERE"
        SQL = SQL & " TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL = SQL & " AND TNHT_FACT.CCEX_CODI = TNHT_CCEX.CCEX_CODI  AND TNHT_FACT.ENTI_CODI IS NULL "
        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = TNHT_FACT.FACT_DAEM"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " GROUP BY TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CONT"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasNoAlojadoTotaLOtrasFormas()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM TNHT_MOVI,TNHT_FORE,TNHT_FACT,TNHT_FAMO,TNHT_CCEX WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_FACT.CCEX_CODI = TNHT_CCEX.CCEX_CODI  AND TNHT_FACT.ENTI_CODI IS NULL "
        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = TNHT_FACT.FACT_DAEM"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasNoAlojadoTotaLDescuentos()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_DESF.DESF_VALO)TOTAL,TNHT_TIDE.TIDE_DESC TIPO,NVL(TNHT_TIDE.TIDE_CTB1,'0') CUENTA,NVL(TNHT_CCEX.CCEX_CDES,'0') CUENTA2 "
        SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE,TNHT_CCEX WHERE"
        SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
        SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_FACT.CCEX_CODI = TNHT_CCEX.CCEX_CODI  AND TNHT_FACT.ENTI_CODI IS NULL "
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " GROUP BY TNHT_TIDE.TIDE_DESC,TNHT_TIDE.TIDE_CTB1,TNHT_CCEX.CCEX_CDES"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            ' USA LA CUENTA CONTABLE DEL DESCUENTO FINANCIERO 
            If mParaUsaCtaComision = 0 Then
                Me.InsertaOracle("AC", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)
                Me.GeneraFileAA("AA", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", Me.mParaCentroCostoAlojamiento, Total)
            Else
                'USA LA CUENTA CONTABLE DE COMISIONES DEFINIDA EN LA CUENTA NO ALOJADO 

                Me.InsertaOracle("AC", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)
                Me.GeneraFileAA("AA", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), "0", Me.mParaCentroCostoAlojamiento, Total)
            End If



        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasNoAlojadoCredito()


        Dim Total As Double
        Dim TotalPendiente As Double
        Dim TotalDiferencia As Double

        SQL = "SELECT"
        SQL += " TNHT_FACT.SEFA_CODI AS SERIE, TNHT_FACT.FACT_CODI AS NUMERO,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA,FACT_TOTA TOTAL,TNHT_FACT.FACT_CONT PENDIENTE, "
        SQL += " FACT_TITU, FACT_DAEM,NVL(CCEX_NCON,0) CUENTA ,NVL(CCEX_NUCO,0) CIF,NVL(CCEX_TITU,'?') AS NOMBRE"
        SQL += " FROM TNHT_FACT, TNHT_CCEX"
        SQL += " WHERE TNHT_FACT.CCEX_CODI = TNHT_CCEX.CCEX_CODI  AND TNHT_FACT.ENTI_CODI IS NULL "
        SQL += " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += " ORDER BY TNHT_FACT.FACT_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalPendiente = CType(Me.DbLeeHotel.mDbLector("PENDIENTE"), Double)
            Total = Decimal.Round(CType(Total, Decimal), 2)
            TotalPendiente = Decimal.Round(CType(TotalPendiente, Decimal), 2)

            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI")
            Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer))
            Me.GeneraFileFV("FV", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), Total, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String).PadRight(15, CChar(" ")), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), CType(Me.DbLeeHotel.mDbLector("CIF"), String), TotalPendiente)

            If Total > TotalPendiente Then
                Linea = Linea + 1
                TotalDiferencia = Total - TotalPendiente
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), TotalDiferencia, "NO", CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), TotalDiferencia)
            End If
        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasNoAlojadoDetalleIgic()
        Dim TotalIva As Double
        Dim TotalBase As Double
        SQL = "SELECT"
        SQL += " TNHT_FACT.SEFA_CODI AS SERIE , TNHT_FACT.FACT_CODI AS NUMERO,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI,FAIV_INCI TOTAL,(FAIV_INCI-FAIV_VIMP) BASE,FAIV_VIMP IGIC, '"
        SQL += Me.mParaTextoIva & " ' || FAIV_TAXA ||'%  '|| TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION, FACT_DAEM,FAIV_TAXA TIPO,NVL(TIVA_CTB1,'0') "
        SQL += " CUENTA ,NVL(CCEX_NCON,0) CUENTACLIENTE ,NVL(CCEX_NUCO,0) CIF ,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X  FROM TNHT_FACT, TNHT_CCEX,TNHT_FAIV,TNHT_TIVA"
        SQL += " WHERE TNHT_FACT.CCEX_CODI = TNHT_CCEX.CCEX_CODI  AND TNHT_FACT.ENTI_CODI IS NULL AND "
        SQL += " TNHT_FACT.FACT_CODI = TNHT_FAIV.FACT_CODI    AND"
        SQL += " TNHT_FACT.SEFA_CODI = TNHT_FAIV.SEFA_CODI"
        SQL += " AND TNHT_FAIV.TIVA_CODI = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += " ORDER BY TNHT_FAIV.FACT_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
            TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            'Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva)
            Me.GeneraFileIV("IV", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalBase, CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), TotalIva, CType(Me.DbLeeHotel.mDbLector("X"), String))


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasNoAlojadoCancelaciondeAnticipos()



        Dim Total As Double
        Dim TotalCancelados As Double



        SQL = "SELECT 'Anticipo Ccex= ' ||TNHT_MOVI.CCEX_CODI CCEX,SUM(TNHT_MOVI.MOVI_VDEB) TOTAL,"
        SQL = SQL & "TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA"
        SQL = SQL & " FROM TNHT_MOVI,TNHT_FACT,TNHT_FAMO,TNHT_CCEX"

        SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI AND "

        ' NUEVO POR AJUSTE DE RENDIMIENTO 
        'SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI AND "
        'SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI AND "
        'SQL = SQL & " TNHT_MOVI.MOVI_DAVA < TNHT_RESE.RESE_DASA AND  "
        ' FIN DE NUEVO 

        'SQL = SQL & " TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        'SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI AND"

        SQL = SQL & " TNHT_MOVI.MOVI_DAVA  < TNHT_FACT.FACT_DAEM      AND"
        SQL = SQL & " TNHT_FACT.FACT_DAEM= " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '1'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        'SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI IS NOT NULL"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT IN ('2','3')"
        SQL = SQL & " AND TNHT_FACT.CCEX_CODI = TNHT_CCEX.CCEX_CODI  AND TNHT_FACT.ENTI_CODI IS NULL "

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        SQL = SQL & " GROUP BY TNHT_MOVI.CCEX_CODI,TNHT_MOVI.RESE_ANCI,TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA"
        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalCancelados = TotalCancelados + Total
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("CCEX"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("CCEX"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
        Me.mCancelacionAnticipos = TotalCancelados
    End Sub



#End Region
#Region "ASIENTO-7 FACTURAS NO ALOJADO ANULADAS"
    Private Sub FacturasNoAlojadoTotalLiquidoAnuladas()
        Dim Total As Double
        Dim TotalComisiones As Double

        SQL = "SELECT (SUM(FAIV_INCI) - SUM(FAIV_VIMP)) TOTAL ,FACT_DAAN "
        SQL += "FROM TNHT_FAIV, TNHT_FACT,TNHT_CCEX "
        SQL += "WHERE TNHT_FACT.CCEX_CODI = TNHT_CCEX.CCEX_CODI  AND TNHT_FACT.ENTI_CODI IS NULL "
        SQL += "AND TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
        SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
        SQL += "AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "
        SQL += "AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += "GROUP BY TNHT_FACT.FACT_DAAN"


        If IsDBNull(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = False Then
            Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            '  + Me.FacturacionCreditoDesembolsos + Me.FacturacionCreditoServiciosSinIgic
            Total = Decimal.Round(CType(Total, Decimal), 2)
        Else
            Total = 0
        End If

        ' tinglado de sumar las comisiones al total liquido
        SQL = "SELECT nvl(SUM(TNHT_DESF.DESF_VALO),'0')TOTAL "
        SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE,TNHT_CCEX WHERE"
        SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
        SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_FACT.CCEX_CODI = TNHT_CCEX.CCEX_CODI  AND TNHT_FACT.ENTI_CODI IS NULL "
        SQL = SQL & " AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "'"
        '' solo santana cazorla 
        If Me.mParaSantana = 1 Then
            SQL = SQL & " AND FACT_DAEM > '15-03-2007' "
        End If


        If Me.mParaComisiones = True Then
            TotalComisiones = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            Total = Total + TotalComisiones
        End If
        Total = Decimal.Round(CType(Total, Decimal), 2)

        If Total <> 0 Then
            Linea = 1
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 71, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorDebe, "Total Liquido + Comisiones", Total, "SI", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorDebe, "FACTURACIÓN LÍQUIDA " & Me.mFecha & " + Comisiones ", Total)
        End If



    End Sub
    Private Sub FacturasNoAlojadoTotalVisasAnuladas()
        Dim Total As Double
        SQL = "SELECT SUM(MOVI_VDEB) TOTAL,CACR_DESC TARJETA,nvl(CACR_CONT,'0') CUENTA FROM TNHT_MOVI,TNHT_CACR,TNHT_FACT,TNHT_FAMO,TNHT_CCEX WHERE"
        SQL = SQL & " TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL = SQL & " AND TNHT_FACT.CCEX_CODI = TNHT_CCEX.CCEX_CODI  AND TNHT_FACT.ENTI_CODI IS NULL "
        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = TNHT_FACT.FACT_DAEM"
        SQL = SQL & " AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "'"
        SQL = SQL & " GROUP BY TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CONT"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 71, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasNoAlojadoTotaLOtrasFormasAnuladas()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM TNHT_MOVI,TNHT_FORE,TNHT_FACT,TNHT_FAMO,TNHT_CCEX WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_FACT.CCEX_CODI = TNHT_CCEX.CCEX_CODI  AND TNHT_FACT.ENTI_CODI IS NULL "
        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = TNHT_FACT.FACT_DAEM"
        SQL = SQL & " AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "'"
        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 71, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasNoAlojadoTotaLDescuentosAnuladas()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_DESF.DESF_VALO)TOTAL,TNHT_TIDE.TIDE_DESC TIPO,NVL(TNHT_TIDE.TIDE_CTB1,'0') CUENTA,NVL(TNHT_CCEX.CCEX_CDES,'0') CUENTA2 "
        SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE,TNHT_CCEX WHERE"
        SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
        SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_FACT.CCEX_CODI = TNHT_CCEX.CCEX_CODI  AND TNHT_FACT.ENTI_CODI IS NULL "
        SQL = SQL & " AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "'"
        SQL = SQL & " GROUP BY TNHT_TIDE.TIDE_DESC,TNHT_TIDE.TIDE_CTB1,TNHT_CCEX.CCEX_CDES"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            ' USA LA CUENTA CONTABLE DEL DESCUENTO FINANCIERO 
            If mParaUsaCtaComision = 0 Then
                Me.InsertaOracle("AC", 71, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)
                Me.GeneraFileAA("AA", 71, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", Me.mParaCentroCostoAlojamiento, Total)
            Else
                'USA LA CUENTA CONTABLE DE COMISIONES DEFINIDA EN LA CUENTA NO ALOJADO 
                Me.InsertaOracle("AC", 71, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)
                Me.GeneraFileAA("AA", 71, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), "0", Me.mParaCentroCostoAlojamiento, Total)
            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasNoAlojadoCreditoAnuladas()


        Dim Total As Double
        Dim TotalPendiente As Double
        Dim TotalDiferencia As Double

        SQL = "SELECT"
        SQL += " TNHT_FACT.SEFA_CODI AS SERIE, TNHT_FACT.FACT_CODI AS NUMERO,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA,FACT_TOTA TOTAL,TNHT_FACT.FACT_CONT PENDIENTE, "
        SQL += " FACT_TITU, FACT_DAEM,NVL(CCEX_NCON,0) CUENTA ,NVL(CCEX_NUCO,0) CIF,NVL(CCEX_TITU,'?') AS NOMBRE"
        SQL += " FROM TNHT_FACT, TNHT_CCEX"
        SQL += " WHERE TNHT_FACT.CCEX_CODI = TNHT_CCEX.CCEX_CODI  AND TNHT_FACT.ENTI_CODI IS NULL "
        SQL += " AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "
        SQL += " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += " ORDER BY TNHT_FACT.FACT_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalPendiente = CType(Me.DbLeeHotel.mDbLector("PENDIENTE"), Double)
            Total = Decimal.Round(CType(Total, Decimal), 2)
            TotalPendiente = Decimal.Round(CType(TotalPendiente, Decimal), 2)

            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 71, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaberFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI")
            Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaberFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, Me.mParaSerieAnulacion & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer))
            Me.GeneraFileFV("FV", 71, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSerieAnulacion & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), (Total * -1), CType(Me.DbLeeHotel.mDbLector("FACTURA"), String).PadRight(15, CChar(" ")), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), CType(Me.DbLeeHotel.mDbLector("CIF"), String), (TotalPendiente * -1))

            If Total > TotalPendiente Then
                Linea = Linea + 1
                TotalDiferencia = Total - TotalPendiente
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 71, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), TotalDiferencia, "NO", CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), TotalDiferencia)
            End If
        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasNoAlojadoDetalleIgicAnuladas()
        Dim TotalIva As Double
        Dim TotalBase As Double

        SQL = "SELECT"
        SQL += " TNHT_FACT.SEFA_CODI AS SERIE , TNHT_FACT.FACT_CODI AS NUMERO,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI,FAIV_INCI TOTAL,(FAIV_INCI-FAIV_VIMP) BASE,FAIV_VIMP IGIC, '"
        SQL += Me.mParaTextoIva & " ' || FAIV_TAXA ||'%  '|| TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION, FACT_DAEM,FAIV_TAXA TIPO,NVL(TIVA_CTB1,'0') "
        SQL += " CUENTA ,NVL(CCEX_NCON,0) CUENTACLIENTE ,NVL(CCEX_NUCO,0) CIF ,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X  FROM TNHT_FACT, TNHT_CCEX,TNHT_FAIV,TNHT_TIVA"
        SQL += " WHERE TNHT_FACT.CCEX_CODI = TNHT_CCEX.CCEX_CODI  AND TNHT_FACT.ENTI_CODI IS NULL AND "
        SQL += " TNHT_FACT.FACT_CODI = TNHT_FAIV.FACT_CODI    AND"
        SQL += " TNHT_FACT.SEFA_CODI = TNHT_FAIV.SEFA_CODI"
        SQL += " AND TNHT_FAIV.TIVA_CODI = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "
        SQL += " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += " ORDER BY TNHT_FAIV.FACT_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
            TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            'Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 71, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva)
            Me.GeneraFileIV("IV", 71, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSerieAnulacion & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), (TotalBase * -1), CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), (TotalIva * -1), CType(Me.DbLeeHotel.mDbLector("X"), String))

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasNoAlojadoCancelaciondeAnticiposAnuladas()



        Dim Total As Double
        Dim TotalCancelados As Double



        SQL = "SELECT 'Anticipo Ccex= ' ||TNHT_MOVI.CCEX_CODI CCEX,SUM(TNHT_MOVI.MOVI_VDEB) TOTAL,"
        SQL = SQL & "TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA"
        SQL = SQL & " FROM TNHT_MOVI,TNHT_FACT,TNHT_FAMO,TNHT_CCEX"

        SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI AND"

        ' NUEVO POR AJUSTE DE RENDIMIENTO 
        'SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI AND "
        'SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI AND "
        'SQL = SQL & " TNHT_MOVI.MOVI_DAVA < TNHT_RESE.RESE_DASA AND  "
        ' FIN DE NUEVO 

        'SQL = SQL & " TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        'SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI AND"

        SQL = SQL & " TNHT_MOVI.MOVI_DAVA  < TNHT_FACT.FACT_DAEM      AND"
        SQL = SQL & " TNHT_FACT.FACT_DAAN= " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '1'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        'SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI IS NOT NULL"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT IN ('2','3')"
        SQL = SQL & " AND TNHT_FACT.CCEX_CODI = TNHT_CCEX.CCEX_CODI  AND TNHT_FACT.ENTI_CODI IS NULL "

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        SQL = SQL & " GROUP BY TNHT_MOVI.CCEX_CODI,TNHT_MOVI.RESE_ANCI,TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalCancelados = TotalCancelados + Total
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 71, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("CCEX"), String).Replace("'", "''"), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("CCEX"), String).Replace("'", "''"), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
        Me.mCancelacionAnticipos = TotalCancelados
    End Sub



#End Region
#Region "ASIENTO-20 DEPOSITOS ANTICIPADOS ENTIDAD"
    Private Sub TotalDepositosAnticipadosVisasEntidad()
        Try
            Dim Total As Double
            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CONT,'0') CUENTA"
            SQL = SQL & " FROM TNHT_MOVI,TNHT_CACR,TNHT_RESE,TNHT_ENTI WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"

            ' SOLO DE AGENCIAS
            SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI"

            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' solo depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='1'"
            '
            SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CONT"


            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
        End Try

    End Sub
    Private Sub TotalDepositosAnticipadosOtrasFormasEntidad()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM TNHT_MOVI,TNHT_FORE,TNHT_RESE,TNHT_ENTI WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"

        ' SOLO DE AGENCIAS
        SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI"

        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' solo depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='1'"

        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"
        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetalleDepositosAnticipadosVisasEntidad()
        Dim Total As Double
        Dim Cuenta As String

        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
        SQL = SQL & " TNHT_CACR.CACR_DESC TARJETA,NVL(ENTI_DEAN,'0') AS CUENTA,MOVI_DAVA FROM TNHT_MOVI,"
        SQL = SQL & " TNHT_CACR,TNHT_RESE,TNHT_ENTI WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"
        ' SOLO DE AGENCIAS
        SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI"

        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' solo depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='1'"

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"

            ' CONTROL DE CUENTA DE DEPOSITOS ANTICIPADOS POR AGENCIA
            If CType(Me.DbLeeHotel.mDbLector("CUENTA"), String) = "0" Then
                Cuenta = Me.mCtaPagosACuenta
            Else
                Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
            End If

            Me.InsertaOracle("AC", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Reserv= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, "Reserv= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetalleDepositosAnticipadosOtrasFormasEntidad()
        Dim Total As Double
        Dim Cuenta As String

        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE,"
        SQL = SQL & " TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(ENTI_DEAN,'0') AS CUENTA,MOVI_DAVA FROM TNHT_MOVI,TNHT_FORE,TNHT_RESE,TNHT_ENTI WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"

        ' SOLO DE AGENCIAS
        SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI"

        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' solo depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='1'"

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"

            ' CONTROL DE CUENTA DE DEPOSITOS ANTICIPADOS POR AGENCIA
            If CType(Me.DbLeeHotel.mDbLector("CUENTA"), String) = "0" Then
                Cuenta = Me.mCtaPagosACuenta
            Else
                Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
            End If

            Me.InsertaOracle("AC", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Reserv= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, "Reserv= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
#End Region
#Region "ASIENTO-30 DEPOSTITOS DE DIRECTOS"
    Private Sub TotalDepositosAnticipadosVisasOtros()
        Try
            Dim Total As Double
            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CONT,'0') CUENTA"
            SQL = SQL & " FROM TNHT_MOVI,TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"

            ' NO DE AGENCIAS
            SQL = SQL & " AND TNHT_RESE.RESE_TRES = '1'"


            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' solo depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='1'"
            '
            SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CONT"


            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 30, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
        End Try

    End Sub
    Private Sub TotalDepositosAnticipadosOtrasFormasOtros()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"

        ' NO DE AGENCIAS
        SQL = SQL & " AND TNHT_RESE.RESE_TRES = '1'"


        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' solo depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='1'"

        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"
        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 30, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub DetalleDepositosAnticipadosVisasOtros()
        Try
            Dim Total As Double


            SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
            SQL = SQL & " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA FROM TNHT_MOVI,"
            SQL = SQL & " TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"
            ' NO DE AGENCIAS
            SQL = SQL & " AND TNHT_RESE.RESE_TRES = '1'"

            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' solo depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='1'"

            Me.DbLeeHotel.TraerLector(SQL)
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "HABER"



                Me.InsertaOracle("AC", 30, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Reserv= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Reserv= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub DetalleDepositosAnticipadosOtrasFormasOtros()
        Try
            Dim Total As Double


            SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE,"
            SQL = SQL & " TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,MOVI_DAVA FROM TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
            SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"

            ' NO DE AGENCIAS
            SQL = SQL & " AND TNHT_RESE.RESE_TRES = '1'"


            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
            SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' solo depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='1'"

            Me.DbLeeHotel.TraerLector(SQL)
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "HABER"



                Me.InsertaOracle("AC", 30, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Reserv= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Reserv= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
#End Region
#Region "ASIENTO 32 DEPOSITOS ANTICIPADOS NO ALOJADOS"
    Private Sub TotalDepositosAnticipadosVisasOtrosNoAlojados()
        Try
            Dim Total As Double
            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CONT,'0') CUENTA"
            SQL = SQL & " FROM TNHT_MOVI,TNHT_CACR WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"

            ' CUENTAS NO ALOJADO
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI IS NOT NULL"



            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' solo depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='1'"
            '
            SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CONT"


            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 32, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
        End Try

    End Sub
    Private Sub TotalDepositosAnticipadosOtrasFormasOtrosNoAlojados()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM TNHT_MOVI,TNHT_FORE WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "


        ' CUENTAS NO ALOJADO
        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI IS NOT NULL"




        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' solo depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='1'"

        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"
        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 32, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub DetalleDepositosAnticipadosVisasOtrosNoAlojados()
        Try
            Dim Total As Double


            SQL = "SELECT TNHT_MOVI.CCEX_CODI  CCEX,NVL(CCEX_TITU,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
            SQL = SQL & " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA FROM TNHT_MOVI,"
            SQL = SQL & " TNHT_CACR,TNHT_CCEX WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI "

            ' CUENTAS NO ALOJADO
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI IS NOT NULL"


            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' solo depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='1'"

            Me.DbLeeHotel.TraerLector(SQL)
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "HABER"



                Me.InsertaOracle("AC", 32, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Cuenta= " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''") & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Cuenta= " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub DetalleDepositosAnticipadosOtrasFormasOtrosNoAlojados()
        Try
            Dim Total As Double


            SQL = "SELECT TNHT_MOVI.CCEX_CODI CCEX,NVL(CCEX_TITU,'?') CLIENTE,"
            SQL = SQL & " TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,MOVI_DAVA FROM TNHT_MOVI,TNHT_FORE,TNHT_CCEX WHERE"
            SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI "


            ' CUENTAS NO ALOJADO
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI IS NOT NULL"


            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
            SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' solo depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='1'"

            Me.DbLeeHotel.TraerLector(SQL)
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "HABER"



                Me.InsertaOracle("AC", 32, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Cuenta= " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''") & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Cuenta= " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
#End Region

#Region "ASIENTO-40 FACTURAS X"
    Private Sub FacturasXTotalLiquido()


        Dim Total As Double
        Dim TotalComisiones As Double

        SQL = "SELECT (SUM(FAIV_INCI) - SUM(FAIV_VIMP)) TOTAL ,FACT_DAEM "
        SQL += "FROM TNHT_FAIV, TNHT_FACT "
        SQL += "WHERE TNHT_FACT.ENTI_CODI IS NULL AND TNHT_FACT.CCEX_CODI IS NULL  "
        SQL += "AND TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
        SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
        SQL += "AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        SQL += "AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += "GROUP BY TNHT_FACT.FACT_DAEM"


        If IsDBNull(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = False Then
            Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            '  + Me.FacturacionCreditoDesembolsos + Me.FacturacionCreditoServiciosSinIgic
            Total = Decimal.Round(CType(Total, Decimal), 2)
        Else
            Total = 0
        End If

        ' tinglado de sumar las comisiones al total liquido
        SQL = "SELECT nvl(SUM(TNHT_DESF.DESF_VALO),'0')TOTAL "
        SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE WHERE"
        SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
        SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI IS NULL AND TNHT_FACT.CCEX_CODI IS NULL "
        If Me.mParaSantana = 1 Then
            SQL = SQL & " AND FACT_DAEM > '15-03-2007' "
        End If


        If Me.mParaComisiones = True Then
            TotalComisiones = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            Total = Total + TotalComisiones
        End If
        Total = Decimal.Round(CType(Total, Decimal), 2)

        If Total <> 0 Then
            Linea = 1
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 40, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "FACTURACIÓN LÍQUIDA " & Me.mFecha & " + Comisiones ", Total, "SI", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, "FACTURACIÓN LÍQUIDA " & Me.mFecha & " + Comisiones ", Total)
        End If



    End Sub
    Private Sub FacturasXTotalVisas()
        Dim Total As Double
        SQL = "SELECT SUM(MOVI_VDEB) TOTAL,CACR_DESC TARJETA,nvl(CACR_CONT,'0') CUENTA FROM TNHT_MOVI,TNHT_CACR,TNHT_FACT,TNHT_FAMO WHERE"
        SQL = SQL & " TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = TNHT_FACT.FACT_DAEM"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI IS NULL AND TNHT_FACT.CCEX_CODI IS NULL "
        SQL = SQL & " GROUP BY TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CONT"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 40, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasXTotaLOtrasFormas()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM TNHT_MOVI,TNHT_FORE,TNHT_FACT,TNHT_FAMO WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = TNHT_FACT.FACT_DAEM"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI IS NULL AND TNHT_FACT.CCEX_CODI IS NULL "
        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 40, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasXTotaLDescuentos()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_DESF.DESF_VALO)TOTAL,TNHT_TIDE.TIDE_DESC TIPO,NVL(TNHT_TIDE.TIDE_CTB1,'0') CUENTA "
        SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE WHERE"
        SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
        SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI IS NULL AND TNHT_FACT.CCEX_CODI IS NULL "
        SQL = SQL & " GROUP BY TNHT_TIDE.TIDE_DESC,TNHT_TIDE.TIDE_CTB1"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 40, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)
            Me.GeneraFileAA("AA", 40, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", Me.mParaCentroCostoAlojamiento, Total)


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasXCredito()


        Dim Total As Double
        Dim TotalPendiente As Double
        Dim TotalDiferencia As Double

        Dim Cuenta As String
        Dim Cif As String
        Dim Texto As String

        SQL = "SELECT"
        SQL += " TNHT_FACT.SEFA_CODI AS SERIE, TNHT_FACT.FACT_CODI AS NUMERO,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA,FACT_TOTA TOTAL,TNHT_FACT.FACT_CONT PENDIENTE, "
        SQL += " FACT_TITU, FACT_DAEM,NVL(FACT_TITU,'?') AS NOMBRE"
        SQL += " FROM TNHT_FACT"
        SQL += " WHERE TNHT_FACT.ENTI_CODI IS NULL AND TNHT_FACT.CCEX_CODI IS NULL "
        SQL += " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += " ORDER BY TNHT_FACT.FACT_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            Texto = "Ingrese una Cuenta Contable Válida para Factura número : "
            Texto += CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String) & vbCrLf & vbCrLf
            Texto += "Titular de la Factura = " & CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String)

            Cuenta = InputBox(Texto, "Atención", AuxCuenta)
            If IsDBNull(Cuenta) = True Then Cuenta = "0"
            AuxCuenta = Cuenta


            Texto = "Ingrese un CIF Válido para Factura número : "
            Texto += CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String) & vbCrLf & vbCrLf
            Texto += "Titular de la Factura = " & CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String)

            Cif = InputBox(Texto, "Atención", AuxCif)

            If IsDBNull(Cif) = True Then Cif = "0"
            AuxCif = Cif







            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalPendiente = CType(Me.DbLeeHotel.mDbLector("PENDIENTE"), Double)
            Total = Decimal.Round(CType(Total, Decimal), 2)
            TotalPendiente = Decimal.Round(CType(TotalPendiente, Decimal), 2)

            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 40, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, "NO", Cif, CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI")
            Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer))
            Me.GeneraFileFV("FV", 40, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), Total, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String).PadRight(15, CChar(" ")), Cuenta, Cif, TotalPendiente)

            If Total <> TotalPendiente Then
                Linea = Linea + 1
                TotalDiferencia = Total - TotalPendiente
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 40, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), TotalDiferencia, "NO", Cif, CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), TotalDiferencia)
            End If
        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasXCreditoDetalleIgic()
        Dim TotalIva As Double
        Dim TotalBase As Double

        SQL = "SELECT"
        SQL += " TNHT_FACT.SEFA_CODI AS SERIE , TNHT_FACT.FACT_CODI AS NUMERO,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI,FAIV_INCI TOTAL,(FAIV_INCI-FAIV_VIMP) BASE,FAIV_VIMP IGIC, '"
        SQL += Me.mParaTextoIva & " ' || FAIV_TAXA ||'%  '|| TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION, FACT_DAEM,FAIV_TAXA TIPO,NVL(TIVA_CTB1,'0') "
        SQL += " CUENTA ,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X  FROM TNHT_FACT,TNHT_FAIV,TNHT_TIVA"
        SQL += " WHERE TNHT_FACT.ENTI_CODI IS NULL AND TNHT_FACT.CCEX_CODI IS NULL AND "
        SQL += " TNHT_FACT.FACT_CODI = TNHT_FAIV.FACT_CODI    AND"
        SQL += " TNHT_FACT.SEFA_CODI = TNHT_FAIV.SEFA_CODI"
        SQL += " AND TNHT_FAIV.TIVA_CODI = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += " ORDER BY TNHT_FAIV.FACT_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
            TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            ' Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 40, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva)
            Me.GeneraFileIV("IV", 40, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalBase, CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), TotalIva, CType(Me.DbLeeHotel.mDbLector("X"), String))


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasXCancelaciondeAnticipos()



        Dim Total As Double
        Dim TotalCancelados As Double



        SQL = "SELECT 'Anticipo Reserv= ' ||TNHT_MOVI.RESE_CODI||'/'||TNHT_MOVI.RESE_ANCI RESERVA,SUM(TNHT_MOVI.MOVI_VDEB) TOTAL,"
        SQL = SQL & "TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA"
        SQL = SQL & " FROM TNHT_MOVI,TNHT_FACT,TNHT_FAMO"

        SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI AND "

        ' NUEVO POR AJUSTE DE RENDIMIENTO 
        'SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI AND "
        'SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI AND "
        'SQL = SQL & " TNHT_MOVI.MOVI_DAVA < TNHT_RESE.RESE_DASA AND  "
        ' FIN DE NUEVO 

        'SQL = SQL & " TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        'SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI AND"

        SQL = SQL & " TNHT_MOVI.MOVI_DAVA  < TNHT_FACT.FACT_DAEM      AND"
        SQL = SQL & " TNHT_FACT.FACT_DAEM= " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI IS NULL AND TNHT_FACT.CCEX_CODI IS NULL "
        SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '1'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        'SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        'SQL = SQL & " AND TNHT_MOVI.RESE_CODI IS NOT NULL"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT IN ('2','3')"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        SQL = SQL & " GROUP BY TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI,TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalCancelados = TotalCancelados + Total
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 40, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
        Me.mCancelacionAnticipos = TotalCancelados
    End Sub

#End Region
#Region "ASIENTO-41 FACTURAS X ANULADAS"
    Private Sub FacturasXTotalLiquidoAnuladas()



        Dim Total As Double
        Dim TotalComisiones As Double

        'SQL = "SELECT SUM(MOVI_VLIQ)TOTAL "
        'SQL += "FROM VH_MIVA,TNHT_FACT,TNHT_ENTI "
        'SQL += "WHERE "
        'SQL += "VH_MIVA.SEFA_CODI = TNHT_FACT.SEFA_CODI "
        'SQL += "AND VH_MIVA.FACT_CODI = TNHT_FACT.FACT_CODI "
        'SQL += "AND TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        'SQL += "AND MOVI_CORR = '0' "
        'SQL += "AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        'SQL += "AND TNHT_FACT.FACT_STAT = '2' "
        'SQL += "AND (TNHT_FACT.FACT_ANUL = '0' OR TNHT_FACT.FACT_DAEM < FACT_DAAN) "
        'SQL += "GROUP BY TNHT_FACT.FACT_DAEM"





        SQL = "SELECT (SUM(FAIV_INCI) - SUM(FAIV_VIMP)) TOTAL ,FACT_DAAN "
        SQL += "FROM TNHT_FAIV, TNHT_FACT "
        SQL += "WHERE  TNHT_FACT.ENTI_CODI IS NULL AND TNHT_FACT.CCEX_CODI IS NULL  "
        SQL += "AND TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
        SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
        SQL += "AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "
        SQL += "AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += "GROUP BY TNHT_FACT.FACT_DAAN"


        If IsDBNull(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = False Then
            Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            '  + Me.FacturacionCreditoDesembolsos + Me.FacturacionCreditoServiciosSinIgic
            Total = Decimal.Round(CType(Total, Decimal), 2)
        Else
            Total = 0
        End If

        ' tinglado de sumar las comisiones al total liquido

        SQL = "SELECT nvl(SUM(TNHT_DESF.DESF_VALO),'0')TOTAL "
        SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE WHERE"
        SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
        SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_FACT.FACT_DAAN= " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI IS NULL AND TNHT_FACT.CCEX_CODI IS NULL "
        '' solo santana cazorla 
        If Me.mParaSantana = 1 Then
            SQL = SQL & " AND FACT_DAEM > '15-03-2007' "
        End If

        ' Fin solo santana cazorla 
        SQL = SQL & " GROUP BY TNHT_TIDE.TIDE_DESC,TNHT_TIDE.TIDE_CTB1"

        If Me.mParaComisiones = True Then
            TotalComisiones = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            Total = Total + TotalComisiones
        End If
        Total = Decimal.Round(CType(Total, Decimal), 2)

        If Total <> 0 Then
            Linea = 1
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 41, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorDebe, "FACTURACIÓN LÍQUIDA " & Me.mFecha & " + Comisiones ", Total, "SI", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorDebe, "FACTURACIÓN LÍQUIDA " & Me.mFecha & " + Comisiones ", Total)
        End If



    End Sub
    Private Sub FacturasXTotalVisasAnuladas()
        Dim Total As Double
        SQL = "SELECT SUM(MOVI_VDEB) TOTAL,CACR_DESC TARJETA,nvl(CACR_CONT,'0') CUENTA FROM TNHT_MOVI,TNHT_CACR,TNHT_FACT,TNHT_FAMO WHERE"
        SQL = SQL & " TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = TNHT_FACT.FACT_DAEM"
        SQL = SQL & " AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI IS NULL AND TNHT_FACT.CCEX_CODI IS NULL "
        SQL = SQL & " GROUP BY TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CONT"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 41, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasXTotaLOtrasFormasAnuladas()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM TNHT_MOVI,TNHT_FORE,TNHT_FACT,TNHT_FAMO WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = TNHT_FACT.FACT_DAEM"
        SQL = SQL & " AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI IS NULL AND TNHT_FACT.CCEX_CODI IS NULL "
        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 41, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasXTotaLDescuentosAnuladas()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_DESF.DESF_VALO)TOTAL,TNHT_TIDE.TIDE_DESC TIPO,NVL(TNHT_TIDE.TIDE_CTB1,'0') CUENTA "
        SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE WHERE"
        SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
        SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT  IN ('2','3') "
        SQL = SQL & " AND TNHT_FACT.FACT_DAAN= " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI IS NULL AND TNHT_FACT.CCEX_CODI IS NULL "
        SQL = SQL & " GROUP BY TNHT_TIDE.TIDE_DESC,TNHT_TIDE.TIDE_CTB1"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 41, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)
            Me.GeneraFileAA("AA", 41, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", Me.mParaCentroCostoAlojamiento, Total)


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasXCreditoAnuladas()


        Dim Total As Double
        Dim TotalPendiente As Double
        Dim TotalDiferencia As Double

        Dim Cuenta As String
        Dim Cif As String
        Dim Texto As String

        SQL = "SELECT"
        SQL += " TNHT_FACT.SEFA_CODI AS SERIE, TNHT_FACT.FACT_CODI AS NUMERO,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA,FACT_TOTA TOTAL,TNHT_FACT.FACT_CONT PENDIENTE, "
        SQL += " FACT_TITU, FACT_DAEM,NVL(FACT_TITU,'?') AS NOMBRE"
        SQL += " FROM TNHT_FACT"
        SQL += " WHERE TNHT_FACT.ENTI_CODI IS NULL AND TNHT_FACT.CCEX_CODI IS NULL  "
        SQL += " AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "
        SQL += " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += " ORDER BY TNHT_FACT.FACT_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            Texto = "Ingrese una Cuenta Contable Válida para Factura número : "
            Texto += CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String) & vbCrLf & vbCrLf
            Texto += "Titular de la Factura = " & CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String)

            Cuenta = InputBox(Texto, "Atención", AuxCuenta)
            If IsDBNull(Cuenta) = True Then Cuenta = "0"
            AuxCuenta = Cuenta

            Texto = "Ingrese un CIF Válido para Factura número : "
            Texto += CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String) & vbCrLf & vbCrLf
            Texto += "Titular de la Factura = " & CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String)

            Cif = InputBox(Texto, "Atención", AuxCif)

            If IsDBNull(Cif) = True Then Cif = "0"
            AuxCif = Cif

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalPendiente = CType(Me.DbLeeHotel.mDbLector("PENDIENTE"), Double)
            Total = Decimal.Round(CType(Total, Decimal), 2)
            TotalPendiente = Decimal.Round(CType(TotalPendiente, Decimal), 2)

            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 41, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaberFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, "NO", Cif, CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI")
            Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaberFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, Me.mParaSerieAnulacion & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer))
            ' OJO !!!!!!!
            Me.InsertaOracle("FV", 41, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaClientesContado, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), (Total * -1), "NO", Cif, "", "NO")
            Me.GeneraFileFV("FV", 41, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSerieAnulacion & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), (Total * -1), CType(Me.DbLeeHotel.mDbLector("FACTURA"), String).PadRight(15, CChar(" ")), Cuenta, Cif, (TotalPendiente * -1))

            If Total <> TotalPendiente Then
                Linea = Linea + 1
                TotalDiferencia = Total - TotalPendiente
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 41, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), TotalDiferencia, "NO", Cif, CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), TotalDiferencia)
            End If

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub

    Private Sub FacturasXDetalleIgicAnuladas()
        Dim TotalIva As Double
        Dim TotalBase As Double

        SQL = "SELECT"
        SQL += " TNHT_FACT.SEFA_CODI AS SERIE , TNHT_FACT.FACT_CODI AS NUMERO,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI,FAIV_INCI TOTAL,(FAIV_INCI-FAIV_VIMP) BASE,FAIV_VIMP IGIC, '"
        SQL += Me.mParaTextoIva & " ' || FAIV_TAXA ||'%  '|| TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION, FACT_DAEM,FAIV_TAXA TIPO,NVL(TIVA_CTB1,'0') "
        SQL += " CUENTA ,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X  FROM TNHT_FACT, TNHT_FAIV,TNHT_TIVA"
        SQL += " WHERE TNHT_FACT.ENTI_CODI IS NULL AND TNHT_FACT.CCEX_CODI IS NULL AND "
        SQL += " TNHT_FACT.FACT_CODI = TNHT_FAIV.FACT_CODI    AND"
        SQL += " TNHT_FACT.SEFA_CODI = TNHT_FAIV.SEFA_CODI"
        SQL += " AND TNHT_FAIV.TIVA_CODI = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "
        SQL += " AND TNHT_FACT.FACT_STAT IN ('2','3') "
        SQL += " ORDER BY TNHT_FAIV.FACT_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
            TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            'Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 41, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva)
            Me.GeneraFileIV("IV", 41, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSerieAnulacion & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), (TotalBase * -1), CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), (TotalIva * -1), CType(Me.DbLeeHotel.mDbLector("X"), String))


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasXCancelaciondeAnticiposAnuladas()



        Dim Total As Double
        Dim TotalCancelados As Double

        SQL = "SELECT 'Anticipo Reserv= ' ||TNHT_MOVI.RESE_CODI||'/'||TNHT_MOVI.RESE_ANCI RESERVA,SUM(TNHT_MOVI.MOVI_VDEB) TOTAL,"
        SQL = SQL & "TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA"
        SQL = SQL & " FROM TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO "

        SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI AND "

        ' NUEVO POR AJUSTE DE RENDIMIENTO 
        'SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI AND "
        'SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI AND "
        'SQL = SQL & " TNHT_MOVI.MOVI_DAVA < TNHT_RESE.RESE_DASA AND  "
        ' FIN DE NUEVO 

        'SQL = SQL & " TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        'SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI AND"

        SQL = SQL & " TNHT_MOVI.MOVI_DAVA  < TNHT_FACT.FACT_DAEM      AND"
        SQL = SQL & " TNHT_FACT.FACT_DAAN = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_FACT.ENTI_CODI IS NULL AND TNHT_FACT.CCEX_CODI IS NULL "
        SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '1'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        'SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI IS NOT NULL"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT IN ('2','3')"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        SQL = SQL & " GROUP BY TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI,TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalCancelados = TotalCancelados + Total
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 41, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
        Me.mCancelacionAnticipos = TotalCancelados
    End Sub

#End Region

#Region "ASIENTO-51"
    Private Sub NotasDeCreditoEntidadTotalLiquido()
        Dim Total As Double

        SQL = "SELECT (SUM(MOVI_VLIQ)) TOTAL ,NCRE_DAEM "
        SQL += "FROM VH_NIVA, TNHT_ENTI "
        SQL += "WHERE VH_NIVA.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL += "AND VH_NIVA.NCRE_DAEM = " & "'" & Me.mFecha & "' "
        SQL += "GROUP BY VH_NIVA.NCRE_DAEM"


        If IsDBNull(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = False Then
            Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            '  + Me.FacturacionCreditoDesembolsos + Me.FacturacionCreditoServiciosSinIgic
        Else
            Total = 0
        End If


        Total = Decimal.Round(CType(Total, Decimal), 2)


        If Total <> 0 Then
            Linea = 1
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "Total Liquido", Total, "SI", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, "Total Liquido", Total)
        End If



    End Sub

    Private Sub NotasDeCreditoEntidadCredito()


        Dim Total As Double
        Dim TotalPendiente As Double
        Dim TotalDiferencia As Double

        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEFA_CODI AS SERIE, TNHT_NCRE.NCRE_CODI AS NUMERO,TNHT_NCRE.NCRE_CODI||'/'||TNHT_NCRE.SEFA_CODI FACTURA,(NCRE_VALO * -1) TOTAL, "
        SQL += " NCRE_TITU, NCRE_DAEM,NVL(ENTI_NCON,0) CUENTA ,NVL(ENTI_NUCO,0) CIF,NVL(ENTI_NOME,'?') AS NOMBRE"
        SQL += " FROM TNHT_NCRE, TNHT_ENTI"
        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL += " AND TNHT_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " ORDER BY TNHT_NCRE.NCRE_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalPendiente = 0
            Total = Decimal.Round(CType(Total, Decimal), 2)
            TotalPendiente = Decimal.Round(CType(TotalPendiente, Decimal), 2)

            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI")
            Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer))
            Me.GeneraFileFV("FV", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), Total, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String).PadRight(15, CChar(" ")), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), CType(Me.DbLeeHotel.mDbLector("CIF"), String), TotalPendiente)

            If Total > TotalPendiente Then
                Linea = Linea + 1
                TotalDiferencia = Total - TotalPendiente
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), TotalDiferencia, "NO", CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), TotalDiferencia)
            End If
        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub NotasDeCreditoEntidadCreditoDetalleIgic()
        Dim TotalIva As Double
        Dim TotalBase As Double
        Dim Totalfactura As Double
        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEFA_CODI AS SERIE , TNHT_NCRE.NCRE_CODI AS NUMERO,(NCRE_VALO * -1)TOTAL,SUM(MOVI_VLIQ) BASE,SUM(VIVA) IGIC, '"
        SQL += Me.mParaTextoIva & " ' || TIVA_PERC ||'%  '|| TNHT_NCRE.NCRE_CODI ||'/'|| TNHT_NCRE.SEFA_CODI DESCRIPCION, TNHT_NCRE.NCRE_DAEM,TIVA_PERC TIPO,NVL(TIVA_CTB1,'0') "
        SQL += " CUENTA ,NVL(ENTI_NCON,0) CUENTACLIENTE ,NVL(ENTI_NUCO,0) CIF ,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X  FROM TNHT_NCRE,TNHT_ENTI,TNHT_TIVA,VH_NIVA"
        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI AND "
        SQL += " TNHT_NCRE.NCRE_CODI = VH_NIVA.NCRE_CODI    AND"
        SQL += " TNHT_NCRE.SEFA_CODI = VH_NIVA.SEFA_CODI"
        SQL += " AND VH_NIVA.TIVA = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " GROUP BY TNHT_NCRE.SEFA_CODI,TNHT_NCRE.NCRE_CODI,NCRE_VALO,TIVA_PERC,TNHT_NCRE.NCRE_DAEM,TIVA_CTB1,ENTI_NCON,"
        SQL += "ENTI_NUCO, TNHT_TIVA.TIVA_CCVL"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
            TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva)
            Me.GeneraFileIV("IV", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalBase, CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), TotalIva, CType(Me.DbLeeHotel.mDbLector("X"), String))


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
   
#End Region


#Region "RUTINAS PRIVADAS"

    Private Function FacturacionSalidaDesembolsos() As Double
        Dim Resultado As String
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB) TOTAL"
        SQL = SQL & " FROM TNHT_MOVI,TNHT_FACT"
        SQL = SQL & " WHERE TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & " '" & Me.mFecha & "'"
        SQL = SQL & " AND TIRE_CODI = '4'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        'DEBAJO PARA CONTROL DE ERROR RARO DE DESEMBOLSOS POSITIVOS NO SE POR QUE LOS HAY
        'SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB < 0"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT <> '2' AND TNHT_FACT.FACT_CLEN = '1' AND TNHT_FACT.FACT_ANUL = '0'"


        Resultado = Me.DbLeeHotel.EjecutaSqlScalar(SQL)
        If IsNumeric(Resultado) = True Then
            Total = CType(Resultado, Double)
            Return Total * -1
        Else
            Return 0
        End If

    End Function
    Private Function FacturacionSalidasServiciosSinIgic() As Double
        Dim Resultado As String
        Dim Total As Double
        '__________________________________________________________________________________________
        ' CALCULO DEl TOTAL DE LOS SERVICIOS SIN IGIC DE LAs FACTURA
        '__________________________________________________________________________________________
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VCRE) TOTAL"
        SQL = SQL & " FROM TNHT_MOVI,TNHT_FACT,TNHT_SERV,TNHT_TIVA"
        SQL = SQL & " WHERE TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI AND"
        SQL = SQL & " TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI AND"
        SQL = SQL & " TNHT_SERV.TIVA_CODI = TNHT_TIVA.TIVA_CODI AND "
        SQL = SQL & " TNHT_TIVA.TIVA_PERC = 0"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & " '" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT <> '2' AND TNHT_FACT.FACT_CLEN = '1' AND TNHT_FACT.FACT_ANUL = '0'"



        Resultado = Me.DbLeeHotel.EjecutaSqlScalar(SQL)
        If IsNumeric(Resultado) = True Then
            Total = CType(Resultado, Double)
            Return Total
        Else
            Return 0
        End If

    End Function
    Private Function FacturacionCreditoDesembolsos() As Double
        Dim Resultado As String
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB) TOTAL"
        SQL = SQL & " FROM TNHT_MOVI,TNHT_FACT,TNHT_ENTI"
        SQL = SQL & " WHERE TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL = SQL & " AND  TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & " '" & Me.mFecha & "'"
        SQL = SQL & " AND TIRE_CODI = '4'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        SQL += "AND TNHT_FACT.FACT_STAT = '2' "
        SQL += "AND (TNHT_FACT.FACT_ANUL = '0' OR FACT_DAEM < FACT_DAAN) "


        Resultado = Me.DbLeeHotel.EjecutaSqlScalar(SQL)
        If IsNumeric(Resultado) = True Then
            Total = CType(Resultado, Double)
            Return Total * -1
        Else
            Return 0
        End If

    End Function
    Private Function FacturacionCreditoServiciosSinIgic() As Double
        Dim Resultado As String
        Dim Total As Double
        '__________________________________________________________________________________________
        ' CALCULO DEl TOTAL DE LOS SERVICIOS SIN IGIC DE LAs FACTURA
        '__________________________________________________________________________________________
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VCRE) TOTAL"
        SQL = SQL & " FROM TNHT_MOVI,TNHT_FACT,TNHT_SERV,TNHT_TIVA,TNHT_ENTI"
        SQL = SQL & " WHERE TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL = SQL & " AND  TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI AND"
        SQL = SQL & " TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI AND"
        SQL = SQL & " TNHT_SERV.TIVA_CODI = TNHT_TIVA.TIVA_CODI AND "
        SQL = SQL & " TNHT_TIVA.TIVA_PERC = 0"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & " '" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        SQL += "AND TNHT_FACT.FACT_STAT = '2' "
        SQL += "AND (TNHT_FACT.FACT_ANUL = '0' OR FACT_DAEM < FACT_DAAN) "



        Resultado = Me.DbLeeHotel.EjecutaSqlScalar(SQL)
        If IsNumeric(Resultado) = True Then
            Total = CType(Resultado, Double)
            Return Total
        Else
            Return 0
        End If

    End Function


    Private Function FacturacionNoAlojadoDesembolsos() As Double
        Dim Resultado As String
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB) TOTAL"
        SQL = SQL & " FROM TNHT_MOVI,TNHT_FACT,TNHT_CCEX"
        SQL = SQL & " WHERE TNHT_FACT.TACO_CODI = TNHT_CCEX.CCEX_CODI "
        SQL = SQL & " AND  TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & " '" & Me.mFecha & "'"
        SQL = SQL & " AND TIRE_CODI = '4'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        'DEBAJO PARA CONTROL DE ERROR RARO DE DESEMBOLSOS POSITIVOS NO SE POR QUE LOS HAY
        'SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB < 0"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT = '2' AND TNHT_FACT.FACT_CLEN = '1' AND TNHT_FACT.FACT_ANUL = '0'"


        Resultado = Me.DbLeeHotel.EjecutaSqlScalar(SQL)
        If IsNumeric(Resultado) = True Then
            Total = CType(Resultado, Double)
            Return Total * -1
        Else
            Return 0
        End If

    End Function
    Private Function FacturacionNoAlojadoServiciosSinIgic() As Double
        Dim Resultado As String
        Dim Total As Double
        '__________________________________________________________________________________________
        ' CALCULO DEl TOTAL DE LOS SERVICIOS SIN IGIC DE LAs FACTURA
        '__________________________________________________________________________________________
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VCRE) TOTAL"
        SQL = SQL & " FROM TNHT_MOVI,TNHT_FACT,TNHT_SERV,TNHT_TIVA,TNHT_CCEX"
        SQL = SQL & " WHERE TNHT_FACT.TACO_CODI = TNHT_CCEX.CCEX_CODI "
        SQL = SQL & " AND  TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI AND"
        SQL = SQL & " TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI AND"
        SQL = SQL & " TNHT_SERV.TIVA_CODI = TNHT_TIVA.TIVA_CODI AND "
        SQL = SQL & " TNHT_TIVA.TIVA_PERC = 0"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & " '" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT = '2' AND TNHT_FACT.FACT_CLEN = '1' AND TNHT_FACT.FACT_ANUL = '0'"




        Resultado = Me.DbLeeHotel.EjecutaSqlScalar(SQL)
        If IsNumeric(Resultado) = True Then
            Total = CType(Resultado, Double)
            Return Total
        Else
            Return 0
        End If

    End Function

    Private Function FacturacionNoAlojadoDesembolsosFactura(ByVal vSerie As String, ByVal vFactura As Integer) As Double
        Dim Resultado As String
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB) TOTAL"
        SQL += " FROM TNHT_MOVI,TNHT_FACT,TNHT_CCEX"
        SQL += " WHERE  TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI "
        SQL += " AND TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI "
        SQL += " AND  TNHT_MOVI.FACT_CODI = " & vFactura
        SQL += " AND TNHT_MOVI.SEFA_CODI = '" & vSerie & "'"
        SQL += " AND TNHT_FACT.TACO_CODI = TNHT_CCEX.CCEX_CODI "
        SQL += " AND TNHT_FACT.FACT_DAEM = " & " '" & Me.mFecha & "'"
        SQL += " AND TIRE_CODI = '4'"
        SQL += " AND TNHT_MOVI.MOVI_CORR = 0"
        'DEBAJO PARA CONTROL DE ERROR RARO DE DESEMBOLSOS POSITIVOS NO SE POR QUE LOS HAY
        'SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB < 0"
        SQL += " AND TNHT_FACT.FACT_STAT = '2' AND TNHT_FACT.FACT_CLEN = '1' AND TNHT_FACT.FACT_ANUL = '0'"
        SQL += " GROUP BY TNHT_MOVI.SEFA_CODI,TNHT_MOVI.FACT_CODI"


        Resultado = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
        If IsNumeric(Resultado) = True Then
            Total = CType(Resultado, Double)
            Return Total * -1
        Else
            Return 0
        End If

    End Function
    Private Function FacturacionNoAlojadoServiciosSinIgicFactura(ByVal vSerie As String, ByVal vFactura As Integer) As Double
        Dim Resultado As String
        Dim Total As Double
        '__________________________________________________________________________________________
        ' CALCULO DEl TOTAL DE LOS SERVICIOS SIN IGIC DE UNA FACTURA
        '__________________________________________________________________________________________
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VCRE) TOTAL"
        SQL += " FROM TNHT_MOVI,TNHT_FACT,TNHT_SERV,TNHT_TIVA,TNHT_CCEX"
        SQL += " WHERE  TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI "
        SQL += " AND TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI "
        SQL += " AND  TNHT_MOVI.FACT_CODI = " & vFactura
        SQL += " AND TNHT_MOVI.SEFA_CODI = '" & vSerie & "'"
        SQL += " AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI"
        SQL += " AND TNHT_SERV.TIVA_CODI = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_TIVA.TIVA_PERC = 0"
        SQL += " AND TNHT_FACT.FACT_DAEM = " & " '" & Me.mFecha & "'"
        SQL += " AND TNHT_MOVI.MOVI_CORR = 0"
        SQL += " AND TNHT_FACT.FACT_STAT = '2' AND TNHT_FACT.FACT_CLEN = '1' AND TNHT_FACT.FACT_ANUL = '0'"
        SQL += " GROUP BY TNHT_MOVI.SEFA_CODI,TNHT_MOVI.FACT_CODI"




        Resultado = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
        If IsNumeric(Resultado) = True Then
            Total = CType(Resultado, Double)

            Return Total
        Else
            Return 0
        End If

    End Function


#End Region
#End Region

#Region "METODOS PUBLICOS"
    Public Sub Procesar()
        Try
            If Me.FileEstaOk = False Then Exit Sub
            ' ---------------------------------------------------------------
            ' Asiento de Ventas 1
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                Me.PendienteFacturarTotal()
                Me.mTextDebug.Text = "Calculando Pdte. Facturar"
                Me.mTextDebug.Update()

                Me.VentasDepartamento()
                Me.mTextDebug.Text = "Calculando Ventas por Departamento"
                Me.mTextDebug.Update()


                Me.mProgress.Value = 10
                Me.mProgress.Update()
            End If

            ' ---------------------------------------------------------------
            ' Asiento de Pagos a Cuenta 2
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                Me.TotalPagosaCuentaVisas()
                Me.mTextDebug.Text = "Pagos a Cuenta Visas"
                Me.mTextDebug.Update()

                Me.TotalPagosaCuentaOtrasFormas()
                Me.mTextDebug.Text = "Pagos a Cuenta Otras Formas de Pago"
                Me.mTextDebug.Update()

                Me.DetallePagosaCuentaVisas()
                Me.mTextDebug.Text = "Detalle de Pagos a Cuenta Visas"
                Me.mTextDebug.Update()

                Me.DetallePagosaCuentaOtrasFormas()
                Me.mTextDebug.Text = "Detalle de Pagos a Cuenta Otras Formas"
                Me.mTextDebug.Update()

                Me.mProgress.Value = 20
                Me.mProgress.Update()
            End If

            ' ---------------------------------------------------------------
            ' Asiento de DEVOLUCIONES  21
            '----------------------------------------------------------------
            If Me.mDebug = True Then
                If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                    Me.TotalDevolucionesVisas()
                    Me.mTextDebug.Text = "Devoluciones Visas"
                    Me.mTextDebug.Update()

                    Me.TotalDevolucionesOtrasFormas()
                    Me.mTextDebug.Text = "Devoluciones Otras Formas de Pago"
                    Me.mTextDebug.Update()

                    Me.DetalleDevolucionesVisas()
                    Me.mTextDebug.Text = "Detalle de Devoluciones Visas"
                    Me.mTextDebug.Update()

                    Me.DetalleDevolucionesOtrasFormas()
                    Me.mTextDebug.Text = "Detalle de Devoluciones Otras Formas"
                    Me.mTextDebug.Update()

                    Me.mProgress.Value = 25
                    Me.mProgress.Update()
                End If
            End If



            ' ---------------------------------------------------------------
            ' Asiento de Pagos a Cuenta cuentas no alojados 99
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                Me.TotalPagosaCuentaVisasNoAlojados()
                Me.mTextDebug.Text = "Pagos a Cuenta Visas No Alojados"
                Me.mTextDebug.Update()

                Me.TotalPagosaCuentaOtrasFormasNoAlojados()
                Me.mTextDebug.Text = "Pagos a Cuenta Otras Formas de Pago No Alojados"
                Me.mTextDebug.Update()

                Me.DetallePagosaCuentaVisasNoAlojados()
                Me.mTextDebug.Text = "Detalle de Pagos a Cuenta Visas No Alojados"
                Me.mTextDebug.Update()

                Me.DetallePagosaCuentaOtrasFormasNoAlojados()
                Me.mTextDebug.Text = "Detalle de Pagos a Cuenta Otras Formas No Alojados"
                Me.mTextDebug.Update()

                Me.mProgress.Value = 26
                Me.mProgress.Update()
            End If
            ' ---------------------------------------------------------------
            ' Asiento Facturacion de Salida 3
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                If Me.mEmpGrupoCod <> "TAHO" Then
                    Me.FacturasSalidaTotalLiquido()
                Else
                    Me.FacturasSalidaTotalLiquidoTahona()
                    Me.FacturasSalidaTotalLiquidoDetalladoTahona()
                End If

                Me.mTextDebug.Text = "Calculando Total Luíquido Facturas de Salida"
                Me.mTextDebug.Update()

                Me.FacturasSalidaTotalVisas()
                Me.mTextDebug.Text = "Calculando Total Visas Facturas de Salida"
                Me.mTextDebug.Update()


                Me.FacturasSalidaTotaLOtrasFormas()
                Me.mTextDebug.Text = "Calculando Total Otras Formas de Cobro Facturas de Salida"
                Me.mTextDebug.Update()

                Me.FacturasSalidaTotaLDescuentos()
                Me.mTextDebug.Text = "Calculando Descuentos Financieros y Comisiones Facturas de Salida"
                Me.mTextDebug.Update()



                Me.FacturasSalidaTotalFActura()


                Me.FacturasSalidaDetalleIgic()
                Me.mTextDebug.Text = "Detalle de Impuesto Facturas de Salida"
                Me.mTextDebug.Update()

                Me.FacturasSalidaCancelaciondeAnticipos()
                Me.mTextDebug.Text = "Cancelación de Anticipos  Facturas de Salida"
                Me.mTextDebug.Update()


                Me.FacturasSalidaDevoluciondeAnticipos()
                Me.mTextDebug.Text = "Devolución de Excedente  de Anticipos  Facturas de Salida"
                Me.mTextDebug.Update()

                If Me.mTrataDebitoTpvnoFacturado = True Then
                    Me.FacturasSalidaTpvNoFacturado()
                End If

                Me.mProgress.Value = 30
                Me.mProgress.Update()
            End If



                ' ---------------------------------------------------------------
                ' Asiento Facturacion de Salida Anuladas 3a
                '----------------------------------------------------------------
                If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open And Me.mParaTipoAnulacion = 1 Then
                    Me.FacturasSalidaTotalLiquidoAnuladas()
                    Me.mTextDebug.Text = "Calculando Total Luíquido Facturas de Salida"
                    Me.mTextDebug.Update()

                    Me.FacturasSalidaTotalVisasAnuladas()
                    Me.mTextDebug.Text = "Calculando Total Visas Facturas de Salida"
                    Me.mTextDebug.Update()


                    Me.FacturasSalidaTotaLOtrasFormasAnuladas()
                    Me.mTextDebug.Text = "Calculando Total Otras Formas de Cobro Facturas de Salida"
                    Me.mTextDebug.Update()


                    Me.FacturasSalidaTotaLDescuentosAnuladas()
                    Me.mTextDebug.Text = "Calculando Total Descuentos Financieros Facturas de Salida"
                    Me.mTextDebug.Update()



                    Me.FacturasSalidaTotalFActuraAnuladas()

                    Me.FacturasSalidaDetalleIgicAnuladas()
                    Me.mTextDebug.Text = "Detalle de Impuesto Facturas de Salida"
                    Me.mTextDebug.Update()

                    Me.FacturasSalidaCancelaciondeAnticiposAnuladas()
                    Me.mTextDebug.Text = "Cancelación de Anticipos  Facturas de Salida"
                    Me.mTextDebug.Update()


                    Me.FacturasSalidaDevoluciondeAnticiposAnuladas()
                    Me.mTextDebug.Text = "Devolución de Excedente  de Anticipos  Facturas de Salida"
                    Me.mTextDebug.Update()

                    If Me.mTrataDebitoTpvnoFacturado = True Then
                        Me.FacturasSalidaTpvNoFacturadoAnuladas()
                    End If

                    Me.mProgress.Value = 35
                    Me.mProgress.Update()
                End If
                ' ---------------------------------------------------------------
                ' Asiento Desembolsos 4
                '----------------------------------------------------------------
                If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                    Me.Desembolsos()
                    Me.mTextDebug.Text = "Saldo de Desembolsos"
                    Me.mTextDebug.Update()
                    Me.mProgress.Value = 40
                    Me.mProgress.Update()
                End If


                ' ---------------------------------------------------------------
                ' Asiento Facturacion de Credito Entidades 5
                '----------------------------------------------------------------
                If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                    Me.FacturasEntidadTotalLiquido()
                    Me.mTextDebug.Text = "Líquido Facturas de Crédito"
                    Me.mTextDebug.Update()

                    Me.FacturasEntidadTotalVisas()
                    Me.FacturasEntidadTotaLOtrasFormas()
                    Me.FacturasEntidadTotaLDescuentos()

                    Me.FacturasEntidadCredito()
                    Me.mTextDebug.Text = "Facturas de Crédito"
                    Me.mTextDebug.Update()

                    Me.FacturasEntidadCreditoDetalleIgic()
                    Me.mTextDebug.Text = "Detalle de Impuesto Facturas de Crédito"
                    Me.mTextDebug.Update()
                    Me.mProgress.Value = 50
                    Me.mProgress.Update()
                    '20.10.2006
                    Me.FacturasEntidadCancelaciondeAnticipos()
                End If

                ' ---------------------------------------------------------------
                ' Asiento Facturacion de Credito Entidades Anuladas 6
                '----------------------------------------------------------------
                If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open And Me.mParaTipoAnulacion = 1 Then
                    Me.FacturasEntidadTotalLiquidoAnuladas()
                    Me.mTextDebug.Text = "Líquido Facturas de Crédito Anuladas"
                    Me.mTextDebug.Update()

                    Me.FacturasEntidadCreditoAnuladas()
                    Me.mTextDebug.Text = "Facturas de Crédito Anuladas"
                    Me.mTextDebug.Update()

                    Me.FacturasEntidadCreditoDetalleIgicAnuladas()
                    Me.mTextDebug.Text = "Detalle de Impuesto Facturas de Crédito Anuladas"
                    Me.mTextDebug.Update()
                    Me.mProgress.Value = 60
                    Me.mProgress.Update()
                    '20.10.2006
                    Me.FacturasEntidadTotalVisasAnuladas()
                    Me.FacturasEntidadTotaLOtrasFormasAnuladas()
                    Me.FacturasEntidadTotaLDescuentosAnuladas()
                    Me.FacturasEntidadCancelaciondeAnticiposAnuladas()
                End If

                ' ---------------------------------------------------------------
                ' Asiento Facturacion de Credito Cuentas No Alojados 7
                '----------------------------------------------------------------
                If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                    Me.FacturasNoAlojadoTotalLiquido()
                    Me.mTextDebug.Text = "Líquido Facturas de Crédito"
                    Me.mTextDebug.Update()

                    Me.FacturasNoAlojadoTotalVisas()
                    Me.FacturasNoAlojadoTotaLOtrasFormas()
                    Me.FacturasNoAlojadoTotaLDescuentos()

                    Me.FacturasNoAlojadoCredito()
                    Me.mTextDebug.Text = "Facturas de Crédito"
                    Me.mTextDebug.Update()

                    Me.FacturasNoAlojadoDetalleIgic()
                    Me.mTextDebug.Text = "Detalle de Impuesto Facturas de Crédito"
                    Me.mTextDebug.Update()
                    Me.mProgress.Value = 65
                    Me.mProgress.Update()
                    '20.10.2006
                    Me.FacturasNoAlojadoCancelaciondeAnticipos()
                End If
                ' ---------------------------------------------------------------
                ' Asiento Facturacion de Credito Cuentas No Alojados ANULADA 71
                '----------------------------------------------------------------
                If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open And Me.mParaTipoAnulacion = 1 Then
                    Me.FacturasNoAlojadoTotalLiquidoAnuladas()
                    Me.mTextDebug.Text = "Líquido Facturas de Crédito"
                    Me.mTextDebug.Update()

                    Me.FacturasNoAlojadoTotalVisasAnuladas()
                    Me.FacturasNoAlojadoTotaLOtrasFormasAnuladas()
                    Me.FacturasNoAlojadoTotaLDescuentosAnuladas()

                    Me.FacturasNoAlojadoCreditoAnuladas()
                    Me.mTextDebug.Text = "Facturas de Crédito"
                    Me.mTextDebug.Update()

                    Me.FacturasNoAlojadoDetalleIgicAnuladas()
                    Me.mTextDebug.Text = "Detalle de Impuesto Facturas de Crédito"
                    Me.mTextDebug.Update()
                    Me.mProgress.Value = 70
                    Me.mProgress.Update()
                    '20.10.2006
                    Me.FacturasNoAlojadoCancelaciondeAnticiposAnuladas()
                End If
                ' ---------------------------------------------------------------
                ' Asiento de Depositos Anticipados  de entidad 20
                '----------------------------------------------------------------
                If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                    Me.TotalDepositosAnticipadosVisasEntidad()
                    Me.mTextDebug.Text = "Depósitos Anticpados Visas"
                    Me.mTextDebug.Update()

                    Me.TotalDepositosAnticipadosOtrasFormasEntidad()
                    Me.mTextDebug.Text = "Depósitos Anticpados Otras Formas de Pago"
                    Me.mTextDebug.Update()

                    Me.DetalleDepositosAnticipadosVisasEntidad()
                    Me.mTextDebug.Text = "Detalle Depósitos Anticpados Visas"
                    Me.mTextDebug.Update()

                    Me.DetalleDepositosAnticipadosOtrasFormasEntidad()
                    Me.mTextDebug.Text = "Detalle Depósitos Anticpados Otras Formas"
                    Me.mTextDebug.Update()

                    Me.mProgress.Value = 80
                    Me.mProgress.Update()
                End If

                ' ---------------------------------------------------------------
                ' Asiento de Depositos Anticipados  de RESERVAS SIN ENTIDAD
                '----------------------------------------------------------------
                If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                    Me.TotalDepositosAnticipadosVisasOtros()
                    Me.mTextDebug.Text = "Depósitos Anticpados Visas"
                    Me.mTextDebug.Update()

                    Me.TotalDepositosAnticipadosOtrasFormasOtros()
                    Me.mTextDebug.Text = "Depósitos Anticpados Otras Formas de Pago"
                    Me.mTextDebug.Update()

                    Me.DetalleDepositosAnticipadosVisasOtros()
                    Me.mTextDebug.Text = "Detalle Depósitos Anticpados Visas"
                    Me.mTextDebug.Update()

                    Me.DetalleDepositosAnticipadosOtrasFormasOtros()
                    Me.mTextDebug.Text = "Detalle Depósitos Anticpados Otras Formas"
                    Me.mTextDebug.Update()

                    Me.mProgress.Value = 85
                    Me.mProgress.Update()
                End If

                ' ---------------------------------------------------------------
                ' Asiento de Depositos Anticipados  de NO ALOJADOS
                '----------------------------------------------------------------
                If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                    Me.TotalDepositosAnticipadosVisasOtrosNoAlojados()
                    Me.mTextDebug.Text = "Depósitos Anticpados Visas"
                    Me.mTextDebug.Update()

                    Me.TotalDepositosAnticipadosOtrasFormasOtrosNoAlojados()
                    Me.mTextDebug.Text = "Depósitos Anticpados Otras Formas de Pago"
                    Me.mTextDebug.Update()

                    Me.DetalleDepositosAnticipadosVisasOtrosNoAlojados()
                    Me.mTextDebug.Text = "Detalle Depósitos Anticpados Visas"
                    Me.mTextDebug.Update()

                    Me.DetalleDepositosAnticipadosOtrasFormasOtrosNoAlojados()
                    Me.mTextDebug.Text = "Detalle Depósitos Anticpados Otras Formas"
                    Me.mTextDebug.Update()

                    Me.mProgress.Value = 86
                    Me.mProgress.Update()
                End If

                ' ---------------------------------------------------------------
                ' Asiento Facturacion de Credito SIN CUENTA CONTABLE  40
                '----------------------------------------------------------------
                If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                    Me.FacturasXTotalLiquido()
                    Me.mTextDebug.Text = "Líquido Facturas de Crédito"
                    Me.mTextDebug.Update()

                    Me.FacturasXTotalVisas()
                    Me.FacturasXTotaLOtrasFormas()
                    Me.FacturasXTotaLDescuentos()

                    Me.FacturasXCredito()
                    Me.mTextDebug.Text = "Facturas de Crédito"
                    Me.mTextDebug.Update()

                    Me.FacturasXCreditoDetalleIgic()
                    Me.mTextDebug.Text = "Detalle de Impuesto Facturas de Crédito"
                    Me.mTextDebug.Update()
                    Me.mProgress.Value = 90
                    Me.mProgress.Update()
                    '20.10.2006
                    Me.FacturasXCancelaciondeAnticipos()
                End If

                ' ---------------------------------------------------------------
                ' Asiento Facturacion de Credito SIN CUENTA CONTABLE  ANULADA 41
                '----------------------------------------------------------------
                If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open And Me.mParaTipoAnulacion = 1 Then
                    Me.FacturasXTotalLiquidoAnuladas()
                    Me.mTextDebug.Text = "Líquido Facturas de Crédito Anuladas"
                    Me.mTextDebug.Update()

                    Me.FacturasXCreditoAnuladas()
                    Me.mTextDebug.Text = "Facturas de Crédito Anuladas"
                    Me.mTextDebug.Update()

                    Me.FacturasXDetalleIgicAnuladas()
                    Me.mTextDebug.Text = "Detalle de Impuesto Facturas de Crédito Anuladas"
                    Me.mTextDebug.Update()

                    '20.10.2006
                    Me.FacturasXTotalVisasAnuladas()
                    Me.FacturasXTotaLOtrasFormasAnuladas()
                    Me.FacturasXTotaLDescuentosAnuladas()

                    Me.mProgress.Value = 91
                    Me.mProgress.Update()
                End If

                ' ---------------------------------------------------------------
                ' Asiento Notas de Credito de Credito Entidades 51
                '----------------------------------------------------------------
                If Me.mDebug = True Then
                    If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                        Me.NotasDeCreditoEntidadTotalLiquido()
                        Me.mTextDebug.Text = "Líquido Notas de Crédito"
                        Me.mTextDebug.Update()

                        Me.NotasDeCreditoEntidadCredito()
                        Me.mTextDebug.Text = "Notas de Crédito"
                        Me.mTextDebug.Update()

                        Me.NotasDeCreditoEntidadCreditoDetalleIgic()
                        Me.mTextDebug.Text = "Detalle de Impuesto Notas de Crédito"
                        Me.mTextDebug.Update()
                        Me.mProgress.Value = 92
                        Me.mProgress.Update()

                    End If
                End If



                Me.AjustarDecimales()
                Me.mProgress.Value = 100
                Me.mProgress.Update()

                Me.CerrarFichero()
                Me.CierraConexiones()
                Me.mTextDebug.Text = "Fin de Integración"
                Me.mTextDebug.Update()

        Catch EX As Exception
            MsgBox(EX.Message)
        End Try

    End Sub
    Private Sub AjustarDecimales()
        Try

            Dim TotalDebe As Decimal
            Dim TotalHaber As Decimal
            Dim TotalDiferencia As Decimal

            'SQL = "SELECT ROUND(SUM(ASNT_DEBE),2) FROM TH_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
            'SQL += " AND ASNT_IMPRIMIR = 'SI'"
            'TotalDebe = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Decimal)

            'SQL = "SELECT ROUND(SUM(ASNT_HABER),2) FROM TH_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
            'SQL += " AND ASNT_IMPRIMIR = 'SI'"
            'TotalHaber = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Decimal)

            SQL = "SELECT ROUND(SUM(round(NVL(ASNT_DEBE,'0'),2)),2) FROM TH_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
            SQL += " AND ASNT_IMPRIMIR = 'SI'"



            If IsNumeric(Me.DbLeeCentral.EjecutaSqlScalar(SQL)) Then
                TotalDebe = CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Decimal)
            Else
                TotalDebe = 0
            End If


            SQL = "SELECT ROUND(SUM(round(NVL(ASNT_HABER,'0'),2)),2) FROM TH_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
            SQL += " AND ASNT_IMPRIMIR = 'SI'"
            If IsNumeric(Me.DbLeeCentral.EjecutaSqlScalar(SQL)) Then
                TotalHaber = CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Decimal)
            Else
                TotalHaber = 0
            End If




            If TotalHaber > TotalDebe Then
                TotalDiferencia = TotalHaber - TotalDebe
                MsgBox("Se va ha producir un Ajuste Decimal  de " & TotalDiferencia & "  " & vbCrLf & vbCrLf & "No Integre con valores superiores a 0.05", MsgBoxStyle.Information, "Atención")
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 999, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaRedondeo, Me.mIndicadorDebe, "AJUSTE REDONDEO", TotalDiferencia, "SI", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaRedondeo, Me.mIndicadorDebe, "AJUSTE REDONDEO", TotalDiferencia)
            End If

            If TotalHaber < TotalDebe Then
                TotalDiferencia = TotalDebe - TotalHaber
                MsgBox("Se va ha producir un Ajuste Decimal  de " & TotalDiferencia & "  " & vbCrLf & vbCrLf & "No Integre con valores superiores a 0.05", MsgBoxStyle.Information, "Atención")
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 999, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaRedondeo, Me.mIndicadorHaber, "AJUSTE REDONDEO", TotalDiferencia, "SI", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaRedondeo, Me.mIndicadorHaber, "AJUSTE REDONDEO", TotalDiferencia)
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

#End Region

    ' ojo cerrar conexiones a la base de datos
End Class


