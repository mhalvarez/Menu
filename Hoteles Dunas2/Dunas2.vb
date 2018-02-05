'Option Strict On
Imports System.IO
Imports System.Globalization

' FALTA :

' CARGAR CCEX_CODI ,RESE_CODI,RESE_ANCI  O ALOJ_CODI   EN LOS MOVIMIENTOS DE ANTICIPO 
' EN EL ASIENTO DE PRODUCCION SEPARAR LA VENTA POSITIVA DE LA NEGATIVA ( OJO VA A OTRA CUENTA ) 

Public Class Dunas2
    'La Clase se Construye pasandole 
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


    ' PARAMETROS INDICADORES  DE SERVICIOS DE ALOJAMIENTO Y PENSION 
    '
    '   ALOJAMIENTO             PARA.SERV_COAP
    '   CAMA EXTRA              PARA.1SERV_CAME
    '   DESAYUNO                PARA.SERV_COPA
    '   ALMUERZO                PARA.SERV_CORE
    '       BEBIDAS ALMUERCO    PARA1.SERV_BERE
    '   CENA                    PARA1.SERV_COCE
    '       BEBIDAS CENA        PARA1.SERV_BECE
    '
    '
    '
    '



    Declare Function CharToOem Lib "user32" Alias "CharToOemA" (ByVal lpszSrc As String, ByVal lpszDst As String) As Long
    Declare Function OemToChar Lib "user32" Alias "OemToCharA" (ByVal lpszSrc As String, ByVal lpszDst As String) As Long


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


    Private mCuentaOpeCaja As String



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



    ' OTROS 
    Private iASCII(63) As Integer       'Para conversión a MS-DOS
    Private AuxCif As String
    Private AuxCuenta As String


    Private mCtaSerieCredito As String
    Private mCtaSerieContado As String
    Private mCtaSerieAnulacion As String
    Private mCtaSerieNotaCredito As String


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

    Private mHayHistorico As Boolean = False
    Private mStrHayHistorico As String
    Private mPerfilCobtable As String
    Dim TotalRegistros As Integer
    Dim Multidiario As Boolean = False
    Dim DiarioVariable As String
    Dim mTrataCaja As Boolean

    ' ASUNTO DEPOSITOS 

    Private mParaUsaCta4b As Boolean
    Private mParaCta4b As String
    Private mParaCta4b2Efectivo As String
    Private mParaCta4b3Visa As String
    Private mParaSecc_DepNh As String

    Private mAuxStr As String

    Private DepartamentosForFait As String = ""


#Region "CONSTRUCTOR"
    Public Sub New(ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vStrConexionCentral As String, _
    ByVal vStrConexionHotel As String, ByVal vFecha As Date, ByVal vFileName As String, ByVal vDebug As Boolean, _
    ByVal vConrolDebug As System.Windows.Forms.TextBox, ByVal vListBox As System.Windows.Forms.ListBox, _
    ByVal vStrConexionSpyro As String, ByVal vProgress As System.Windows.Forms.ProgressBar, ByVal vTrataDebitoNoFacturadoTpv As Boolean, _
    ByVal vUsaTnhtMoviAuto As Boolean, ByVal vEmpNum As Integer, ByVal vForm As System.Windows.Forms.Form, _
    ByVal vMuestraIncidencias As Boolean, ByVal vPerfilContable As String, ByVal vTrataCaja As Boolean, _
    ByVal vCuentaOpeCaja As String)


        MyBase.New()

        Me.mDebug = vDebug
        Me.mEmpGrupoCod = vEmpGrupoCod
        Me.mEmpCod = vEmpCod
        Me.mEmpNum = vEmpNum
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

        Me.mForm = vForm

        Me.mMuestraIncidencias = vMuestraIncidencias

        Me.mListBoxDebug.Items.Clear()
        Me.mListBoxDebug.Update()

        Me.mUsaTnhtMoviAuto = vUsaTnhtMoviAuto

        Me.mPerfilCobtable = vPerfilContable

        Me.mTrataCaja = vTrataCaja

        Me.mCuentaOpeCaja = vCuentaOpeCaja


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

    Public Property TotalProduccion() As Double
        Get
            Return Me.mTotalProduccion
        End Get
        Set(ByVal Value As Double)
            Me.mTotalProduccion = Value
        End Set
    End Property

    Public Property TotalFacturado() As Double
        Get
            Return Me.mTotalFacturacion
        End Get
        Set(ByVal Value As Double)
            Me.mTotalFacturacion = Value
        End Set
    End Property



#End Region
#Region "METODOS Privados"



    Private Function GetTotalNotaCredito(ByVal vNota As Integer, ByVal vSerie As String) As Double
        Try
            Dim Retorno As String

            SQL = "SELECT MOVI_VLIQ + NCRE_VIMP  FROM QWE_CONT_NCIM"
            SQL += " WHERE NCRE_CODI = " & vNota
            SQL += " AND SEFA_CODI = '" & vSerie & "'"

            Retorno = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            If Retorno <> "" Then
                Return CDbl(Retorno)
            Else
                Return 0
            End If

            Return CDbl(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL))
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Private Function GetTotalBaseNotaCredito(ByVal vNota As Integer, ByVal vSerie As String) As Double
        Try
            Dim Retorno As String

            SQL = "SELECT MOVI_VLIQ  FROM QWE_CONT_NCIM"
            SQL += " WHERE NCRE_CODI = " & vNota
            SQL += " AND SEFA_CODI = '" & vSerie & "'"

            Retorno = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            If Retorno <> "" Then
                Return CDbl(Retorno)
            Else
                Return 0
            End If

            Return CDbl(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL))
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Private Function GetTotalIgicNotaCredito(ByVal vNota As Integer, ByVal vSerie As String) As Double
        Try
            Dim Retorno As String

            SQL = "SELECT NCRE_VIMP  FROM QWE_CONT_NCIM"
            SQL += " WHERE NCRE_CODI = " & vNota
            SQL += " AND SEFA_CODI = '" & vSerie & "'"

            Retorno = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            If Retorno <> "" Then
                Return CDbl(Retorno)
            Else
                Return 0
            End If

            Return CDbl(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL))
        Catch ex As Exception
            Return 0
        End Try

    End Function
   


    Private Sub CrearFichero(ByVal vFile As String)

        Try
            'Filegraba = New StreamWriter(vFile, False, System.Text.Encoding.UTF8)
            Filegraba = New StreamWriter(vFile, False, System.Text.Encoding.ASCII)





            Filegraba.WriteLine("")
            FileEstaOk = True
        Catch ex As Exception
            FileEstaOk = False
            MsgBox("No dispone de acceso al Fichero " & vFile & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Information, "Atención")
        End Try
    End Sub
    Private Function MyCharToOem1(ByVal vStr1 As String, ByVal vLongitud As Integer) As String
        Try
            Dim Fijo As Long
            Dim StrConv As String = Space(vLongitud)
            Fijo = CharToOem(vStr1, StrConv)
            If mParaToOem = True Then
                'MsgBox(vStr1 & vbCrLf & StrConv)
                Return StrConv
            Else
                Return vStr1
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Char to Oem")
            Return ""
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
            SQL += "NVL(PARA_CTA_SERIE_ANUL,'0') PARA_CTA_SERIE_ANUL,"


            SQL += "NVL(PARA_CTA_56DIGITO,'0') PARA_CTA_56DIGITO,"

            SQL += "NVL(PARA_USA_CTA4B,'0') AS PARA_USA_CTA4B, "
            SQL += "NVL(PARA_CTA4B,'<Ninguno>') AS PARA_CTA4B,  "
            SQL += "NVL(PARA_CTA4B2,'<Ninguno>') AS PARA_CTA4B2,  "
            SQL += "NVL(PARA_CTA4B3,'<Ninguno>') AS PARA_CTA4B3,  "
            SQL += "NVL(PARA_SECC_DEPNH,'<Ninguno>') AS PARA_SECC_DEPNH  "







            SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
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
                Me.mCtaSerieAnulacion = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA_SERIE_ANUL"), String)

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
    Private Sub BorraRegistros()
        Try


            Me.mTextDebug.Text = "Borrando Incidencias y Registros"
            Me.mTextDebug.Update()

            SQL = "SELECT COUNT(*) FROM TH_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum
            If CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Integer) > 0 Then
                If mMuestraIncidencias = True Then
                    MsgBox("Ya existen Movimientos de Integración para esta Fecha", MsgBoxStyle.Information, "Atención")
                End If
            End If
            SQL = "DELETE TH_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum
            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            SQL = "DELETE TH_ERRO WHERE ERRO_F_ATOCAB =  '" & Me.mFecha & "'"
            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)


            SQL = "DELETE TH_INCI WHERE INCI_DATR =  '" & Me.mFecha & "'"
            SQL += " AND INCI_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND INCI_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND INCI_EMP_NUM =  " & Me.mEmpNum

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ")"




            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
            Me.mTextDebug.Update()

            If vCfcta_Cod.Length < 2 And vCfcta_Cod <> "NO TRATAR" Then
                Me.mTexto = "NEWHOTEL: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                Me.mListBoxDebug.Items.Add(Me.mTexto)
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

            End If


            If vTipo = "FV" Then
                If vCif.Length = 0 Or vCif = "0" Then
                    Me.mTexto = "NEWHOTEL: " & "CIF no válido para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                    Me.mListBoxDebug.Items.Add(Me.mTexto)

                    'SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    'SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    'Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                End If
            End If

            If vCfcta_Cod <> "NO TRATAR" Then
                If Me.mParaValidaSpyro = 1 Then
                    '         Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod, vAmpcpto, vNombre)
                End If
            End If



        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    ' igic notas de credito
    'aqui 
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                       ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String _
                                      , ByVal vNotaNumero As String, ByVal vNotaSerie As String, ByVal vTipoNota As String, ByVal vFacturaqueAnula As String, ByVal vNada As String,vTipoIgic As Double ,vBase As Double )

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_AUXILIAR_STRING,ASNT_AUXILIAR_STRING2,ASNT_LIN_VLIQ,ASNT_LIN_TIIMP) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" _
            & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vNotaNumero & "','" & vNotaSerie & "','" _
            & vTipoNota & "','" & vFacturaqueAnula & "'," & vBase & "," & vTipoIgic & ")"



            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
            Me.mTextDebug.Update()

            If vCfcta_Cod.Length < 2 And vCfcta_Cod <> "NO TRATAR" Then
                Me.mTexto = "NEWHOTEL: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                Me.mListBoxDebug.Items.Add(Me.mTexto)
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

            End If


            If vTipo = "FV" Then
                If vCif.Length = 0 Or vCif = "0" Then
                    Me.mTexto = "NEWHOTEL: " & "CIF no válido para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                    Me.mListBoxDebug.Items.Add(Me.mTexto)

                    'SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    'SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    'Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                End If
            End If

            If vCfcta_Cod <> "NO TRATAR" Then
                If Me.mParaValidaSpyro = 1 Then
                    '         Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod, vAmpcpto, vNombre)
                End If
            End If



        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    ' AGUSTIN
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                      ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                      , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                        ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vSerie As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_FACTURA_SERIE) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vSerie & "')"




            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
            Me.mTextDebug.Update()

            If vCfcta_Cod.Length < 2 And vCfcta_Cod <> "NO TRATAR" Then
                Me.mTexto = "NEWHOTEL: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                Me.mListBoxDebug.Items.Add(Me.mTexto)
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

            End If


            If vTipo = "FV" Then
                If vCif.Length = 0 Or vCif = "0" Then
                    Me.mTexto = "NEWHOTEL: " & "CIF no válido para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                    Me.mListBoxDebug.Items.Add(Me.mTexto)

                    'SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    'SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    'Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                End If
            End If

            If vCfcta_Cod <> "NO TRATAR" Then
                If Me.mParaValidaSpyro = 1 Then
                    '         Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod, vAmpcpto, vNombre)
                End If
            End If



        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub

    ' AGUSTIN aqui ''
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                      ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                      , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                        ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, _
                                        ByVal vServCodi As String, ByVal vServDesc As String, ByVal vTipoVenta As String, ByVal vNada As String, ByVal vSeccion As String, ByVal vCosto As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_DPTO_CODI,ASNT_DPTO_DESC,ASNT_TIPO_VENTA,ASNT_AUXILIAR_STRING2,ASNT_AUXILIAR_STRING) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'"

            SQL += vServCodi & "','" & vServDesc & "','" & vTipoVenta & "','" & vSeccion & "','" & vCosto & "')"



            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
            Me.mTextDebug.Update()

            If vCfcta_Cod.Length < 2 And vCfcta_Cod <> "NO TRATAR" Then
                Me.mTexto = "NEWHOTEL: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                Me.mListBoxDebug.Items.Add(Me.mTexto)
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

            End If


            If vTipo = "FV" Then
                If vCif.Length = 0 Or vCif = "0" Then
                    Me.mTexto = "NEWHOTEL: " & "CIF no válido para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                    Me.mListBoxDebug.Items.Add(Me.mTexto)

                    'SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    'SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    'Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                End If
            End If

            If vCfcta_Cod <> "NO TRATAR" Then
                If Me.mParaValidaSpyro = 1 Then
                    '         Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod, vAmpcpto, vNombre)
                End If
            End If



        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub

    Private Sub InsertaOracleGustavo(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                      ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                      , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                        ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vFactura As String, _
                                        ByVal vSerie As String, ByVal vTipoFactura As String, ByVal vBase As Double, _
                                        ByVal vTipoIgic As Double, ByVal vCcexCodi As String, ByVal vReseCodi As String, ByVal vReseAnci As String, ByVal vAlojCodi As String, ByVal vFaanCodi As String)

        Try

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If


            If IsNothing(vCcexCodi) = False Then
                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_AUXILIAR_STRING,ASNT_LIN_VLIQ,ASNT_LIN_TIIMP,ASNT_CCEX_CODI,ASNT_AUXILIAR_STRING2) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += Me.mCfatodiari_Cod & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vFactura & "','" & vSerie & "','" & vTipoFactura & "'," & vBase & "," & vTipoIgic & ",'" & vCcexCodi & "','" & vFaanCodi & "')"


                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()
            ElseIf IsNothing(vReseCodi) = False Then
                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_AUXILIAR_STRING,ASNT_LIN_VLIQ,ASNT_LIN_TIIMP,ASNT_CCEX_CODI,ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_ALOJ_CODI,ASNT_AUXILIAR_STRING2 ) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += Me.mCfatodiari_Cod & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vFactura & "','" & vSerie & "','" & vTipoFactura & "'," & vBase & "," & vTipoIgic & ",'" & vCcexCodi & "'," & vReseCodi & "," & vReseAnci & ",'" & vAlojCodi & "','" & vFaanCodi & "')"




                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()


            Else
                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_AUXILIAR_STRING,ASNT_LIN_VLIQ,ASNT_LIN_TIIMP,ASNT_AUXILIAR_STRING2) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += Me.mCfatodiari_Cod & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vFactura & "','" & vSerie & "','" & vTipoFactura & "'," & vBase & "," & vTipoIgic & ",'" & vFaanCodi & "')"




                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()

            End If





            If vCfcta_Cod.Length < 2 And vCfcta_Cod <> "NO TRATAR" Then
                Me.mTexto = "NEWHOTEL: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                Me.mListBoxDebug.Items.Add(Me.mTexto)
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

            End If


            If vTipo = "FV" Then
                If vCif.Length = 0 Or vCif = "0" Then
                    Me.mTexto = "NEWHOTEL: " & "CIF no válido para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                    Me.mListBoxDebug.Items.Add(Me.mTexto)

                    'SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    'SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    'Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                End If
            End If

            If vCfcta_Cod <> "NO TRATAR" Then
                If Me.mParaValidaSpyro = 1 Then
                    '         Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod, vAmpcpto, vNombre)
                End If
            End If



        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                       ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vFactura As String, ByVal vSerie As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vFactura & "','" & vSerie & "')"




            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
            Me.mTextDebug.Update()

            If vCfcta_Cod.Length < 2 And vCfcta_Cod <> "NO TRATAR" Then
                Me.mTexto = "NEWHOTEL: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                Me.mListBoxDebug.Items.Add(Me.mTexto)
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

            End If


            If vTipo = "FV" Then
                If vCif.Length = 0 Or vCif = "0" Then
                    Me.mTexto = "NEWHOTEL: " & "CIF no válido para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                    Me.mListBoxDebug.Items.Add(Me.mTexto)

                    'SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    'SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    'Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                End If
            End If

            If vCfcta_Cod <> "NO TRATAR" Then
                If Me.mParaValidaSpyro = 1 Then
                    '         Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod, vAmpcpto, vNombre)
                End If
            End If


        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    ' GUSTAVO
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                       ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vFactura As String, ByVal vSerie As String, ByVal vTipoFactura As String, ByVal vNada As String _
                                       , ByVal vCcexCodi As String, ByVal vReseCodi As String, ByVal vReseAnci As String, ByVal vAlojCodi As String, _
                                       ByVal vFaanCodi As String, ByVal vDescuento As Double)

        Try

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If






            If IsNothing(vCcexCodi) = False Then

                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_AUXILIAR_STRING,ASNT_CCEX_CODI,ASNT_AUXILIAR_STRING2,ASNT_AUXILIAR_NUMERICO ) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += Me.mCfatodiari_Cod & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vFactura & "','" & vSerie & "','" & vTipoFactura & "','" & vCcexCodi & "','" & vFaanCodi & "'," & vDescuento & ")"

                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()

            ElseIf IsNothing(vReseCodi) = False Then

                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_AUXILIAR_STRING,ASNT_CCEX_CODI,ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_ALOJ_CODI,ASNT_AUXILIAR_STRING2,ASNT_AUXILIAR_NUMERICO ) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += Me.mCfatodiari_Cod & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vFactura & "','" & vSerie & "','" & vTipoFactura & "','" & vCcexCodi & "'," & vReseCodi & "," & vReseAnci & ",'" & vAlojCodi & "','" & vFaanCodi & "'," & vDescuento & ")"

                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()

            Else

                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_AUXILIAR_STRING,ASNT_AUXILIAR_STRING2,ASNT_AUXILIAR_NUMERICO) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += Me.mCfatodiari_Cod & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vFactura & "','" & vSerie & "','" & vTipoFactura & "','" & vFaanCodi & "'," & vDescuento & ")"

                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()

            End If






            If vCfcta_Cod.Length < 2 And vCfcta_Cod <> "NO TRATAR" Then
                Me.mTexto = "NEWHOTEL: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                Me.mListBoxDebug.Items.Add(Me.mTexto)
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

            End If


            If vTipo = "FV" Then
                If vCif.Length = 0 Or vCif = "0" Then
                    Me.mTexto = "NEWHOTEL: " & "CIF no válido para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                    Me.mListBoxDebug.Items.Add(Me.mTexto)

                    'SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    'SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    'Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                End If
            End If

            If vCfcta_Cod <> "NO TRATAR" Then
                If Me.mParaValidaSpyro = 1 Then
                    '         Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod, vAmpcpto, vNombre)
                End If
            End If


        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    ' devoluciones 
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                       ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, _
                                       ByVal vImprimir As String, ByVal vAuxiliarString As String, _
                                       ByVal vMultiDiario As Boolean, ByVal vDptoNh As String, ByVal vCcexCodi As String, ByVal vReseCodi As String, ByVal vReseAnci As String, ByVal vAlojCodi As String, ByVal vNada As String, ByVal vNada2 As String)

        Try

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            If vMultiDiario = True Then
                Me.DiarioVariable = Me.mCfatodiari_Cod_2
            Else
                Me.DiarioVariable = Me.mCfatodiari_Cod
            End If

            ' Localiza la Descripcion de la Seccion de NewHotel para algunos Asientos 
            If vDptoNh <> "" Then
                SQL = "SELECT SECC_DESC FROM TNHT_SECC WHERE SECC_CODI = '" & vDptoNh & "'"
                Me.mAuxStr = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            Else
                Me.mAuxStr = ""
            End If

            If IsNothing(vCcexCodi) = False Then
                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_DPTO_CODI,ASNT_DPTO_DESC,ASNT_CCEX_CODI ) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += DiarioVariable & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste
                SQL += "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vDptoNh & "','" & Me.mAuxStr & "','" & vCcexCodi & "')"

                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()
            ElseIf IsNothing(vReseCodi) = False Then
                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_DPTO_CODI,ASNT_DPTO_DESC,ASNT_CCEX_CODI,ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_ALOJ_CODI ) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += DiarioVariable & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste
                SQL += "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vDptoNh & "','" & Me.mAuxStr & "','" & vCcexCodi & "'," & vReseCodi & "," & vReseAnci & ",'" & vAlojCodi & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()
            Else
                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_DPTO_CODI,ASNT_DPTO_DESC ) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += DiarioVariable & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste
                SQL += "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vDptoNh & "','" & Me.mAuxStr & "')"

                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()
            End If





            If vCfcta_Cod.Length < 2 And vCfcta_Cod <> "NO TRATAR" Then
                Me.mTexto = "NEWHOTEL: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                Me.mListBoxDebug.Items.Add(Me.mTexto)
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

            End If


            If vTipo = "FV" Then
                If vCif.Length = 0 Or vCif = "0" Then
                    Me.mTexto = "NEWHOTEL: " & "CIF no válido para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                    Me.mListBoxDebug.Items.Add(Me.mTexto)

                    'SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    'SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    'Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                End If
            End If

            If vCfcta_Cod <> "NO TRATAR" Then
                If Me.mParaValidaSpyro = 1 Then
                    '         Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod, vAmpcpto, vNombre)
                End If
            End If


        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    ' agustin pagos a cuenta 2
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                    ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                    , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                      ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, _
                                      ByVal vImprimir As String, ByVal vAuxiliarString As String, _
                                      ByVal vMultiDiario As Boolean, ByVal vDptoNh As String, ByVal vCcexCodi As String, ByVal vNada As String, ByVal vReseCodi As String, ByVal vReseAnci As String, ByVal vAlojCodi As String)

        Try

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            If vMultiDiario = True Then
                Me.DiarioVariable = Me.mCfatodiari_Cod_2
            Else
                Me.DiarioVariable = Me.mCfatodiari_Cod
            End If

            ' Localiza la Descripcion de la Seccion de NewHotel para algunos Asientos 
            If vDptoNh <> "" Then
                SQL = "SELECT SECC_DESC FROM TNHT_SECC WHERE SECC_CODI = '" & vDptoNh & "'"
                Me.mAuxStr = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            Else
                Me.mAuxStr = ""
            End If


            If IsNothing(vCcexCodi) = False Then
                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_DPTO_CODI,ASNT_DPTO_DESC,ASNT_CCEX_CODI ) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += DiarioVariable & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste
                SQL += "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vDptoNh & "','" & Me.mAuxStr & "','" & vCcexCodi & "')"

                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()

            ElseIf IsNothing(vReseCodi) = False Then
                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_DPTO_CODI,ASNT_DPTO_DESC,ASNT_CCEX_CODI,ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_ALOJ_CODI ) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += DiarioVariable & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste
                SQL += "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vDptoNh & "','" & Me.mAuxStr & "','" & vCcexCodi & "'," & vReseCodi & "," & vReseAnci & ",'" & vAlojCodi & "')"

                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()
            Else
                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_DPTO_CODI,ASNT_DPTO_DESC ) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += DiarioVariable & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste
                SQL += "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vDptoNh & "','" & Me.mAuxStr & "')"

                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()
            End If



            If vCfcta_Cod.Length < 2 And vCfcta_Cod <> "NO TRATAR" Then
                Me.mTexto = "NEWHOTEL: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                Me.mListBoxDebug.Items.Add(Me.mTexto)
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

            End If


            If vTipo = "FV" Then
                If vCif.Length = 0 Or vCif = "0" Then
                    Me.mTexto = "NEWHOTEL: " & "CIF no válido para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                    Me.mListBoxDebug.Items.Add(Me.mTexto)

                    'SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    'SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    'Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                End If
            End If

            If vCfcta_Cod <> "NO TRATAR" Then
                If Me.mParaValidaSpyro = 1 Then
                    '         Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod, vAmpcpto, vNombre)
                End If
            End If


        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    ' agustin pagos a cuenta 9
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                       ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, _
                                       ByVal vImprimir As String, ByVal vAuxiliarString As String, _
                                       ByVal vMultiDiario As Boolean, ByVal vDptoNh As String, ByVal vCcexCodi As String, ByVal vReseCodi As String, ByVal vReseAnci As String, ByVal vAlojCodi As String)

        Try

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            If vMultiDiario = True Then
                Me.DiarioVariable = Me.mCfatodiari_Cod_2
            Else
                Me.DiarioVariable = Me.mCfatodiari_Cod
            End If

            ' Localiza la Descripcion de la Seccion de NewHotel para algunos Asientos 
            If vDptoNh <> "" Then
                SQL = "SELECT SECC_DESC FROM TNHT_SECC WHERE SECC_CODI = '" & vDptoNh & "'"
                Me.mAuxStr = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            Else
                Me.mAuxStr = ""
            End If


            If IsNothing(vCcexCodi) = False Then
                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_DPTO_CODI,ASNT_DPTO_DESC,ASNT_CCEX_CODI ) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += DiarioVariable & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste
                SQL += "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vDptoNh & "','" & Me.mAuxStr & "','" & vCcexCodi & "')"

                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()

            ElseIf IsNothing(vReseCodi) = False Then
                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_DPTO_CODI,ASNT_DPTO_DESC,ASNT_CCEX_CODI,ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_ALOJ_CODI ) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += DiarioVariable & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste
                SQL += "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vDptoNh & "','" & Me.mAuxStr & "','" & vCcexCodi & "'," & vReseCodi & "," & vReseAnci & ",'" & vAlojCodi & "')"

                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()
            Else
                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_DPTO_CODI,ASNT_DPTO_DESC ) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += DiarioVariable & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste
                SQL += "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vDptoNh & "','" & Me.mAuxStr & "')"

                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()
            End If



            If vCfcta_Cod.Length < 2 And vCfcta_Cod <> "NO TRATAR" Then
                Me.mTexto = "NEWHOTEL: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                Me.mListBoxDebug.Items.Add(Me.mTexto)
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)
            End If


            If vTipo = "FV" Then
                If vCif.Length = 0 Or vCif = "0" Then
                    Me.mTexto = "NEWHOTEL: " & "CIF no válido para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                    Me.mListBoxDebug.Items.Add(Me.mTexto)

                    'SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    'SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    'Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                End If
            End If

            If vCfcta_Cod <> "NO TRATAR" Then
                If Me.mParaValidaSpyro = 1 Then
                    '         Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod, vAmpcpto, vNombre)
                End If
            End If


        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    '**
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                    ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                    , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                      ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vTipoNota As String, ByVal vFactura As String, ByVal vSerie As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vTipoNota & "','" & vFactura & "','" & vSerie & "')"




            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
            Me.mTextDebug.Update()

            If vCfcta_Cod.Length < 2 And vCfcta_Cod <> "NO TRATAR" Then
                Me.mTexto = "NEWHOTEL: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                Me.mListBoxDebug.Items.Add(Me.mTexto)
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

            End If


            If vTipo = "FV" Then
                If vCif.Length = 0 Or vCif = "0" Then
                    Me.mTexto = "NEWHOTEL: " & "CIF no válido para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                    Me.mListBoxDebug.Items.Add(Me.mTexto)

                    'SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    'SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    'Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                End If
            End If

            If vCfcta_Cod <> "NO TRATAR" Then
                If Me.mParaValidaSpyro = 1 Then
                    '         Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod, vAmpcpto, vNombre)
                End If
            End If


        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    ' Notas de credito
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                    ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                    , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                      ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, _
                                      ByVal vAuxiliarString As String, ByVal vFactura As String, ByVal vSerie As String, ByVal vFacturaAnulada As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_AUXILIAR_STRING2) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vFactura & "','" & vSerie & "','" & vFacturaAnulada & "')"




            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
            Me.mTextDebug.Update()

            If vCfcta_Cod.Length < 2 And vCfcta_Cod <> "NO TRATAR" Then
                Me.mTexto = "NEWHOTEL: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                Me.mListBoxDebug.Items.Add(Me.mTexto)
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

            End If


            If vTipo = "FV" Then
                If vCif.Length = 0 Or vCif = "0" Then
                    Me.mTexto = "NEWHOTEL: " & "CIF no válido para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                    Me.mListBoxDebug.Items.Add(Me.mTexto)

                    'SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    'SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    'Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                End If
            End If

            If vCfcta_Cod <> "NO TRATAR" Then
                If Me.mParaValidaSpyro = 1 Then
                    '         Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod, vAmpcpto, vNombre)
                End If
            End If


        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    ' AGuSTIN FACTURAS DE CONTADO

    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                   ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                   , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                     ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String, ByVal vFactura As String, ByVal vSerie As String _
                                      , ByVal vCcexCodi As String, ByVal vReseCodi As String, ByVal vReseAnci As String, ByVal vAlojCodi As String)

        Try

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            If IsNothing(vCcexCodi) = False Then

                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_CCEX_CODI) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += Me.mCfatodiari_Cod & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vFactura & "','" & vSerie & "','" & vCcexCodi & "')"


                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()


            ElseIf IsNothing(vReseCodi) = False Then

                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_CCEX_CODI,ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_ALOJ_CODI ) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += Me.mCfatodiari_Cod & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vFactura & "','" & vSerie & "','" & vCcexCodi & "'," & vReseCodi & "," & vReseAnci & ",'" & vAlojCodi & "')"




                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()

            Else

                SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
                SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
                SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE) values ('"
                SQL += vTipo & "','"
                SQL += vEmpGrupoCod & "','"
                SQL += vEmpCod & "','"
                SQL += vCefejerc_Cod & "','"
                SQL += Me.mCfatodiari_Cod & "',"
                SQL += vAsiento & ","
                SQL += Linea & ",'"
                SQL += vCfcta_Cod & "','"
                SQL += vCfcptos_Cod & "','"
                SQL += Mid(vAmpcpto, 1, 40).Replace("'", ",") & "',"
                SQL += vImonep & ","
                SQL += "'N','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vFactura & "','" & vSerie & "')"




                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
                Me.mTextDebug.Update()

            End If


            If vCfcta_Cod.Length < 2 And vCfcta_Cod <> "NO TRATAR" Then
                Me.mTexto = "NEWHOTEL: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                Me.mListBoxDebug.Items.Add(Me.mTexto)
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

            End If


            If vTipo = "FV" Then
                If vCif.Length = 0 Or vCif = "0" Then
                    Me.mTexto = "NEWHOTEL: " & "CIF no válido para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & " " & vNombre.Replace("'", "''")
                    Me.mListBoxDebug.Items.Add(Me.mTexto)

                    'SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    'SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    'Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                End If
            End If

            If vCfcta_Cod <> "NO TRATAR" Then
                If Me.mParaValidaSpyro = 1 Then
                    '         Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod, vAmpcpto, vNombre)
                End If
            End If


        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    Private Sub SpyroCompruebaCuentas()
        Try
            SQL = "SELECT ASNT_CFCTA_COD,ASNT_TIPO_REGISTRO,ASNT_CFATOCAB_REFER,ASNT_LINEA,ASNT_CFCPTOS_COD,"
            SQL += "NVL(ASNT_AMPCPTO,'?') AS ASNT_AMPCPTO,NVL(ASNT_NOMBRE,'?') AS ASNT_NOMBRE, "
            SQL += " NVL(ASNT_FACTURA_NUMERO,'?') AS NUMERO,NVL(ASNT_FACTURA_SERIE,'?') AS SERIE "
            SQL += "FROM TH_ASNT WHERE "
            SQL += "     ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum
            SQL += " AND ASNT_F_VALOR = '" & Me.mFecha & "'"
            SQL += " AND ASNT_CFCTA_COD <> 'NO TRATAR'"

            Me.DbLeeCentral.TraerLector(SQL)
            While Me.DbLeeCentral.mDbLector.Read
                Me.SpyroCompruebaCuenta(CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_CFCTA_COD")), _
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_TIPO_REGISTRO")), _
                                        CInt(Me.DbLeeCentral.mDbLector.Item("ASNT_CFATOCAB_REFER")), _
                                        CInt(Me.DbLeeCentral.mDbLector.Item("ASNT_LINEA")), _
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_CFCPTOS_COD")), _
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_AMPCPTO")), _
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_NOMBRE")), _
                                        CStr(Me.DbLeeCentral.mDbLector.Item("NUMERO")), _
                                        CStr(Me.DbLeeCentral.mDbLector.Item("SERIE")))


            End While
            Me.DbLeeCentral.mDbLector.Close()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub SpyroCompruebaCuenta(ByVal vCuenta As String, ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vLinea As Integer, ByVal vDebeHaber As String, ByVal vAmpcpto As String, ByVal vNombre As String, ByVal vFactura As String, ByVal vSerie As String)
        Try

            Me.mTextDebug.Text = "Validando Plan de Cuentas Spyro " & vCuenta.PadRight(20, CChar(" ")) & " Longitud : " & vCuenta.Length

            Me.mTextDebug.Update()
            '    Me.mForm.Update()


            SQL = "SELECT COD FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND COD = '" & vCuenta & "'"



            If IsNothing(Me.DbSpyro.EjecutaSqlScalar(SQL)) = True Then
                Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & "  No se localiza en Plan de Cuentas de Spyro")
                Me.mListBoxDebug.Update()
                Me.mTexto = "SPYRO   : " & vCuenta & "  No se localiza en Plan de Cuentas de Spyro"
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & " " & vAmpcpto & " " & vNombre & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto & " + " & vAmpcpto & " + " & vNombre)

                Exit Sub
            End If


            SQL = "SELECT APTESDIR_SN FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND COD = '" & vCuenta & "'"



            If Me.DbSpyro.EjecutaSqlScalar(SQL) <> "S" Then
                Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & "  No es una Cuenta de Apuntes Directos en Plan de Cuentas Spyro")
                Me.mListBoxDebug.Update()
                Me.mTexto = "SPYRO   : " & vCuenta & "  No es una Cuenta de Apuntes Directos en Plan de Cuentas Spyro"
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)


                Exit Sub
            End If

            If vTipo = "AC" And (vDebeHaber = Me.mIndicadorDebeFac Or vDebeHaber = Me.mIndicadorHaberFac) Then
                SQL = "SELECT 'X' FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
                SQL += "  AND COD = '" & vCuenta & "'"
                SQL += " AND RSOCIAL_COD IS NULL"



                If Me.DbSpyro.EjecutaSqlScalar(SQL) = "X" Then
                    Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & "  No tiene definida Razón Social  en Plan de Cuentas de Spyro")
                    Me.mListBoxDebug.Update()
                    Me.mTexto = "SPYRO   : " & vCuenta & " No tiene definida Razón Social  en Plan de Cuentas de Spyro"
                    SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                    Exit Sub
                End If

            End If

            If vTipo = "AC" And (vDebeHaber = Me.mIndicadorDebeFac Or vDebeHaber = Me.mIndicadorHaberFac) Then
                SQL = "SELECT 'X' FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
                SQL += "  AND COD = '" & vCuenta & "'"
                SQL += " AND CFIVALIBRO_COD IS NULL"



                If Me.DbSpyro.EjecutaSqlScalar(SQL) = "X" Then
                    Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & "  No tiene definido Libro de Iva   en Plan de Cuentas de Spyro")
                    Me.mListBoxDebug.Update()
                    Me.mTexto = "SPYRO   : " & vCuenta & "  No tiene definido Libro de Iva   en Plan de Cuentas de Spyro"
                    SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                    Exit Sub
                End If

            End If
            If vTipo = "AC" And (vDebeHaber = Me.mIndicadorDebeFac Or vDebeHaber = Me.mIndicadorHaberFac) Then
                SQL = "SELECT 'X' FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
                SQL += "  AND COD = '" & vCuenta & "'"
                SQL += " AND CFIVACLASE_COD IS NULL"



                If Me.DbSpyro.EjecutaSqlScalar(SQL) = "X" Then
                    Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & "  No tiene definido Clase de Iva   en Plan de Cuentas de Spyro")
                    Me.mListBoxDebug.Update()
                    Me.mTexto = "SPYRO   : " & vCuenta & "  No tiene definido Clase de Iva   en Plan de Cuentas de Spyro"
                    SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                    Exit Sub
                End If

            End If

            If vTipo = "AC" And (vDebeHaber = Me.mIndicadorDebeFac Or vDebeHaber = Me.mIndicadorHaberFac) Then
                SQL = "SELECT 'X' FROM CFCTACONDI WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
                SQL += "  AND CFCTA_COD = '" & vCuenta & "'"

                If IsNothing(Me.DbSpyro.EjecutaSqlScalar(SQL)) = True Then
                    Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & "  No tiene definido Forma de pago en Plan de Cuentas de Spyro")
                    Me.mListBoxDebug.Update()
                    Me.mTexto = "SPYRO   : " & vCuenta & "  No tiene definido Forma de pago en Plan de Cuentas de Spyro"
                    SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                    Exit Sub
                End If

            End If


            If vTipo = "AC" And (vDebeHaber = Me.mIndicadorDebeFac Or vDebeHaber = Me.mIndicadorHaberFac) Then
                SQL = "SELECT 'X' FROM FACTUTIPO WHERE"
                SQL += " COD = '" & vSerie & "'"


                If IsNothing(Me.DbSpyro.EjecutaSqlScalar(SQL)) = True Then
                    Me.mListBoxDebug.Items.Add("SPYRO   : " & vSerie & "  Serie NO Definida")
                    Me.mListBoxDebug.Update()
                    Me.mTexto = "SPYRO   : " & vSerie & "  Serie NO Definida"
                    SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

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

            TotalRegistros = TotalRegistros + 1

            '-------------------------------------------------------------------------------------------------
            '  Apuntes Contables(CFATOLIN)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) & _
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
            Me.mCfatotip_Cod.PadRight(4, CChar(" ")))



            Me.mForm.ParentForm.Text = CStr(TotalRegistros)
        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
        End Try
    End Sub
    Private Sub GeneraFileACMultiDiario(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
     ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, ByVal vDiario As Boolean)
        Try
            Dim FechaAsiento As String
            If Me.mParaFechaRegistroAc = "V" Then
                FechaAsiento = Format(Me.mFecha, "ddMMyyyy")
            ElseIf Me.mParaFechaRegistroAc = "R" Then
                FechaAsiento = Format(Now, "ddMMyyyy")
            Else
                FechaAsiento = Format(Me.mFecha, "ddMMyyyy")
            End If

            If vDiario = True Then
                Me.DiarioVariable = Me.mCfatodiari_Cod_2
            Else
                Me.DiarioVariable = Me.mCfatodiari_Cod
            End If

            TotalRegistros = TotalRegistros + 1

            '-------------------------------------------------------------------------------------------------
            '  Apuntes Contables(CFATOLIN)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) & _
            vEmpGrupoCod.PadRight(4, CChar(" ")) & _
            vEmpCod.PadRight(4, CChar(" ")) & _
            Mid(FechaAsiento, 5, 4) & _
            Me.DiarioVariable.PadRight(4, CChar(" ")) & _
            " ".PadLeft(8, CChar(" ")) & _
            " ".PadLeft(4, CChar(" ")) & _
            vCfcta_Cod.PadRight(15, CChar(" ")) & _
            vCfcptos_Cod.PadRight(4, CChar(" ")) & _
            Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & _
            CType(vImonep, String).PadLeft(16, CChar(" ")) & _
            "N" & FechaAsiento & _
            Format(Me.mFecha, "ddMMyyyy") & _
            " ".PadRight(40, CChar(" ")) & _
            Me.mCfatotip_Cod.PadRight(4, CChar(" ")))

            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

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


            TotalRegistros = TotalRegistros + 1
            '-------------------------------------------------------------------------------------------------
            '  Apuntes Contables(CFATOLIN)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) & _
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
            CType(vNfactura, String).PadRight(8, CChar(" ")))

            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

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

            TotalRegistros = TotalRegistros + 1
            '-------------------------------------------------------------------------------------------------
            '  Apuntes Contables(CFATOLIN)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) & _
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
            "*")

            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

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

            TotalRegistros = TotalRegistros + 1

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

            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAA")
        End Try
    End Sub
    Private Sub GeneraFileFV(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, _
    ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String, ByVal vPendiente As Double)

        Try

            TotalRegistros = TotalRegistros + 1
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

            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileFV")
        End Try
    End Sub
    Private Sub GeneraFileFV2(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, _
   ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String)

        Try

            TotalRegistros = TotalRegistros + 1
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


            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileFV")
        End Try
    End Sub
    Private Sub GeneraFileVF(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, _
   ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String)

        Try

            TotalRegistros = TotalRegistros + 1
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

            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileVF")
        End Try
    End Sub
    Private Sub GeneraFileIV(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vFactutipo_cod As String, _
    ByVal vNfactura As Integer, ByVal vI_basmonemp As Double, ByVal vPj_iva As Double, ByVal vI_ivamonemp As Double, ByVal vX As String)


        Try

            TotalRegistros = TotalRegistros + 1
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

            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

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
    Public Sub CierraConexiones()
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

    Private Sub GestionIncidencia(ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vEmpNum As Integer, ByVal vDescripcion As String)

        Try

            SQL = "INSERT INTO TH_INCI (INCI_DATR,INCI_EMPGRUPO_COD,INCI_EMP_COD,INCI_EMP_NUM,INCI_ORIGEN,INCI_DESCRIPCION) "
            SQL += " VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "','" & Me.mEmpGrupoCod & "','" & Me.mEmpCod & "'," & Me.mEmpNum & ",'NEWHOTEL VENTAS','" & vDescripcion & "')"

            Me.DbGrabaCentral.IniciaTransaccion()

            Me.DbGrabaCentral.EjecutaSql(SQL)

            Me.DbGrabaCentral.ConfirmaTransaccion()

        Catch ex As Exception
            Me.DbGrabaCentral.CancelaTransaccion()
        End Try

    End Sub

#Region "ASIENTO-1"
    Private Sub PendienteFacturarTotal()

        Try


            Dim Total As Double
            SQL = "SELECT "
            SQL += "ROUND (SUM (MOVI_VLIQ), 2)"
            SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI ,TNHT_SERV"
            SQL += " WHERE MOVI_DATR= '" & Me.mFecha & "'"

            If Me.mSerruchaDepartamentos = True Then
                SQL += " AND DECODE(SERV_PCRM,null,0,SERV_PCRM) <> 99 "
            End If

            SQL += " AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "


            If IsNumeric(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = True Then
                Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            Else
                Total = 0
            End If

            If Total <> 0 Then
                Linea = 1
                Me.mTipoAsiento = "DEBE"

                Me.mTotalProduccion = Total

                Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorDebe, "PENDIENTE DE FACTURAR", Total, "SI", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorDebe, "PENDIENTE DE FACTURAR", Total)

            End If




        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub PendienteFacturarTotalRound()

        Try


            Dim Total As Double
            Dim Texto As String = ""

            'Texto = "PENDIENTE DE FACTURAR"
            Texto = "PRODUCCIÓN DÍA " & Me.mFecha

            'SQL = "SELECT "
            'SQL += "ROUND (SUM (MOVI_VLIQ), 2)"
            'SQL += " FROM TNHT_MOVI ,TNHT_SERV"
            'SQL += " WHERE MOVI_DATR= '" & Me.mFecha & "'"
            'SQL += " AND TNHT_MOVI.SERV_CODI(+) = TNHT_SERV.SERV_CODI "


            SQL = "SELECT "
            SQL += "ROUND (SUM (MOVI_VLIQ), 2) AS LIQUIDO "
            'SQL += " SUM(ROUND (MOVI_VLIQ, 2)) AS LIQUIDO "
            SQL += " FROM VNHT_MOVH TNHT_MOVI ,TNHT_SERV"
            SQL += " WHERE MOVI_DATR= '" & Me.mFecha & "'"
            SQL += " AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "
            SQL += " GROUP BY TNHT_MOVI.SERV_CODI "






            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                Total = Total + CType(Me.DbLeeHotel.mDbLector("LIQUIDO"), Double)

            End While
            Me.DbLeeHotel.mDbLector.Close()


            If Total <> 0 Then
                Linea = 1
                Me.mTipoAsiento = "DEBE"

                Me.mTotalProduccion = Total

                Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorDebe, Texto, Total, "SI", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorDebe, Texto, Total)

            End If




        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub VentasDepartamentoAlojamiento()

        Dim Total As Double
        Dim vCentroCosto As String
        SQL = "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI SERVICIO,TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CMBD,'0') CUENTA ,"
        SQL += "ROUND (SUM (MOVI_VLIQ), 2) TOTAL ,NVL(MOPE_DESC,'Ningun') AS REGIMEN"
        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_SERV,TNHT_MOPE"
        SQL += " WHERE TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "
        SQL += " AND TNHT_MOVI.MOPE_CODI = TNHT_MOPE.MOPE_CODI(+)"
        SQL += " AND MOVI_DATR= '" & Me.mFecha & "'"

        If mDesglosaAlojamientoporRegimen = True Then
            SQL += " AND TNHT_SERV.SERV_CODI = '" & Me.mParaServicioAlojamiento & "'"
        End If

        SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CMBD,TNHT_SERV.SERV_COMS,TNHT_MOPE.MOPE_DESC"
        SQL += " ORDER BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CMBD"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " + " & CType(Me.DbLeeHotel.mDbLector("REGIMEN"), String), Total, "NO", "", vCentroCosto, "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " + " & CType(Me.DbLeeHotel.mDbLector("REGIMEN"), String), Total)
                If vCentroCosto <> "0" Then
                    '        Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", vCentroCosto, Total)
                End If
            End If
        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub VentasDepartamento()

        ' PARAMETROS INDICADORES  DE SERVICIOS DE ALOJAMIENTO Y PENSION 
        '
        '   ALOJAMIENTO             PARA.SERV_COAP
        '   CAMA EXTRA              PARA.1SERV_CAME
        '   DESAYUNO                PARA.SERV_COPA
        '   ALMUERZO                PARA.SERV_CORE
        '       BEBIDAS ALMUERCO    PARA1.SERV_BERE
        '   CENA                    PARA1.SERV_COCE
        '       BEBIDAS CENA        PARA1.SERV_BECE
        '
        '
        '
        '





        Dim Total As Double
        Dim vCentroCosto As String
        SQL = "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI SERVICIO,TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CTB1,'0') CUENTA ,"
        SQL += "ROUND (SUM (MOVI_VLIQ), 2) TOTAL "
        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_SERV"
        SQL += " WHERE (TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI) AND MOVI_DATR= '" & Me.mFecha & "'"

        If mDesglosaAlojamientoporRegimen = True Then
            SQL += " AND TNHT_SERV.SERV_CODI <> '" & Me.mParaServicioAlojamiento & "'"
        End If

        If Me.mSerruchaDepartamentos = True Then
            SQL += " AND DECODE(SERV_PCRM,null,0,SERV_PCRM) <> 99 "
        End If


        SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,TNHT_SERV.SERV_COMS"
        SQL += " ORDER BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total, "NO", "", vCentroCosto, "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total)
                If vCentroCosto <> "0" Then
                    '           Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", vCentroCosto, Total)
                End If
            End If
        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub VentasDepartamentoDunasDobleManoCorriente()

        ' PARAMETROS INDICADORES  DE SERVICIOS DE ALOJAMIENTO Y PENSION 
        '
        '   ALOJAMIENTO             PARA.SERV_COAP
        '   CAMA EXTRA              PARA.1SERV_CAME
        '   DESAYUNO                PARA.SERV_COPA
        '   ALMUERZO                PARA.SERV_CORE
        '       BEBIDAS ALMUERCO    PARA1.SERV_BERE
        '   CENA                    PARA1.SERV_COCE
        '       BEBIDAS CENA        PARA1.SERV_BECE
        '
        '
        '
        '-------------------------------------------------------------

        ' ULTIMA SQL 




        Me.DepartamentosForFait = ""

        SQL = "SELECT SERV_COAP ,SERV_COPA,SERV_CORE FROM TNHT_PARA "

        Me.DbLeeHotel.TraerLector(SQL)
        Me.DbLeeHotel.mDbLector.Read()

        If Me.DbLeeHotel.mDbLector.HasRows Then
            DepartamentosForFait = "'" & CStr(Me.DbLeeHotel.mDbLector.Item("SERV_COAP")) & "','" & CStr(Me.DbLeeHotel.mDbLector.Item("SERV_COPA")) _
            & "','" & CStr(Me.DbLeeHotel.mDbLector.Item("SERV_CORE")) & "','"
        End If
        Me.DbLeeHotel.mDbLector.Close()


        SQL = "SELECT SERV_CAME ,SERV_BERE,SERV_COCE,SERV_BECE FROM TNHT_PAR1 "

        Me.DbLeeHotel.TraerLector(SQL)
        Me.DbLeeHotel.mDbLector.Read()

        If Me.DbLeeHotel.mDbLector.HasRows Then
            DepartamentosForFait += CStr(Me.DbLeeHotel.mDbLector.Item("SERV_CAME")) & "','" & CStr(Me.DbLeeHotel.mDbLector.Item("SERV_BERE")) _
            & "','" & CStr(Me.DbLeeHotel.mDbLector.Item("SERV_COCE")) & "','" & CStr(Me.DbLeeHotel.mDbLector.Item("SERV_BECE")) & "'"
        End If
        Me.DbLeeHotel.mDbLector.Close()

        ' SE AÑADE EL  servicio "002"  = pension que inicialmente formo parte del forfait antes del desglose 
        DepartamentosForFait = "(" & DepartamentosForFait & ",'002'" & ")"


        '**********************************************************
        '
        'FORFAIT CLIENTES DIRECTOS RESE_TRES = 1     O DE AGENCIAS DE PAGO   ENTIFAMA = 2
        '
        '**********************************************************
        Dim Total As Double
        Dim vCentroCosto As String
        Dim Cuenta As String = ""
        Dim TipoCuenta As String = ""



        SQL = "SELECT  "
        SQL += "DECODE(ENTI_FAMA,'1','ENTIDAD','2','PRIVADO','PRIVADO') AS ENTI_FAMA, "
        SQL += "TNHT_MOVI.SECC_CODI, "
        SQL += "         TNHT_MOVI.SERV_CODI SERVICIO, "
        SQL += "         TNHT_MOVI.SECC_CODI SECCION, "

        SQL += "         TNHT_SERV.SERV_DESC DEPARTAMENTO, "
        SQL += "         NVL (TNHT_SERV.SERV_CMBD, '0') CUENTAAG, "
        SQL += "         NVL (TNHT_SERV.SERV_CTB1, '0') CUENTADIR, "
        SQL += "         NVL (TNHT_SERV.SERV_COMS, '0') COMS, "
        SQL += "        ROUND (SUM (MOVI_VLIQ), 2) TOTAL "
        SQL += "    FROM TNHT_MOVI, "
        SQL += "         TNHT_SERV, "
        SQL += "         TNHT_RESE, "
        SQL += "         TNHT_ENTI "
        SQL += "   WHERE     TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "
        SQL += "         AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL += "         AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
        SQL += "         AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+) "
        SQL += "  AND MOVI_DATR= '" & Me.mFecha & "'"

        ' SOLO DEPARTAMENTOS DE PENSION
        SQL += " AND TNHT_MOVI.SERV_CODI IN " & Me.DepartamentosForFait

        SQL += "GROUP BY TNHT_MOVI.SECC_CODI, "
        SQL += "         TNHT_MOVI.SERV_CODI, "
        SQL += "         TNHT_MOVI.SECC_CODI, "
        SQL += "         TNHT_SERV.SERV_DESC, "
        SQL += "         TNHT_SERV.SERV_CMBD, "
        SQL += "         TNHT_SERV.SERV_CTB1, "
        SQL += "         TNHT_SERV.SERV_COMS, "
        SQL += "        DECODE(ENTI_FAMA,'1','ENTIDAD','2','PRIVADO','PRIVADO')  "
        SQL += " ,TNHT_SERV.SERV_TICO "
        SQL += " ORDER BY TNHT_SERV.SERV_TICO "


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            If CStr(Me.DbLeeHotel.mDbLector("ENTI_FAMA")) = "ENTIDAD" Then
                Cuenta = CStr(Me.DbLeeHotel.mDbLector("CUENTAAG"))
                TipoCuenta = "AG"
            Else
                Cuenta = CStr(Me.DbLeeHotel.mDbLector("CUENTADIR"))
                TipoCuenta = "CL"
            End If



            If vCentroCosto = "0" Then
                vCentroCosto = ""
            End If


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " [" & CType(Me.DbLeeHotel.mDbLector("ENTI_FAMA"), String) & "]", Total, "NO", "", vCentroCosto, "SI", CStr(Me.DbLeeHotel.mDbLector("SERVICIO")), CStr(Me.DbLeeHotel.mDbLector("DEPARTAMENTO")), TipoCuenta, "", CStr(Me.DbLeeHotel.mDbLector("SECCION")), vCentroCosto)
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total)
                If vCentroCosto <> "0" Then
                    '           Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", vCentroCosto, Total)
                End If
            End If
        End While
        Me.DbLeeHotel.mDbLector.Close()


        ' RESTO DE SERVICIOS


        SQL = "SELECT  "
        SQL += "TNHT_MOVI.SECC_CODI, "
        SQL += "         TNHT_MOVI.SERV_CODI SERVICIO, "
        SQL += "         TNHT_MOVI.SECC_CODI SECCION, "
        SQL += "         TNHT_SERV.SERV_DESC DEPARTAMENTO, "
        SQL += "         NVL (TNHT_SERV.SERV_CTB1, '0') CUENTA, "
        SQL += "         NVL (TNHT_SERV.SERV_COMS, '0') COMS, "
        SQL += "        ROUND (SUM (MOVI_VLIQ), 2) TOTAL "
        SQL += "    FROM TNHT_MOVI, "
        SQL += "         TNHT_SERV, "
        SQL += "         TNHT_RESE, "
        SQL += "         TNHT_ENTI "
        SQL += "   WHERE     TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "
        SQL += "         AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL += "         AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
        SQL += "         AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+) "
        SQL += "  AND MOVI_DATR= '" & Me.mFecha & "'"

        ' SOLO DEPARTAMENTOS DE PENSION
        SQL += " AND TNHT_MOVI.SERV_CODI NOT IN " & Me.DepartamentosForFait

        SQL += "GROUP BY TNHT_MOVI.SECC_CODI, "
        SQL += "         TNHT_MOVI.SERV_CODI, "
        SQL += "         TNHT_MOVI.SECC_CODI, "
        SQL += "         TNHT_SERV.SERV_DESC, "
        SQL += "         TNHT_SERV.SERV_CTB1, "
        SQL += "         TNHT_SERV.SERV_COMS "


        '  SQL += " ORDER BY TNHT_SERV.SERV_TICO "


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            TipoCuenta = "CL"

            If vCentroCosto = "0" Then
                vCentroCosto = ""
            End If


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total, "NO", "", vCentroCosto, "SI", CStr(Me.DbLeeHotel.mDbLector("SERVICIO")), CStr(Me.DbLeeHotel.mDbLector("DEPARTAMENTO")), TipoCuenta, "", CStr(Me.DbLeeHotel.mDbLector("SECCION")), vCentroCosto)
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total)
                If vCentroCosto <> "0" Then
                    '           Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", vCentroCosto, Total)
                End If
            End If
        End While
        Me.DbLeeHotel.mDbLector.Close()








    End Sub

    Private Sub VentasDepartamentoDunasDobleManoCorrienteAgrupado()

        ' PARAMETROS INDICADORES  DE SERVICIOS DE ALOJAMIENTO Y PENSION 
        '
        '   ALOJAMIENTO             PARA.SERV_COAP
        '   CAMA EXTRA              PARA.1SERV_CAME
        '   DESAYUNO                PARA.SERV_COPA
        '   ALMUERZO                PARA.SERV_CORE
        '       BEBIDAS ALMUERCO    PARA1.SERV_BERE
        '   CENA                    PARA1.SERV_COCE
        '       BEBIDAS CENA        PARA1.SERV_BECE
        '
        '
        '
        '-------------------------------------------------------------

        ' ULTIMA SQL 


        Dim Total2 As Double
        Dim Texto As String = ""

        'Texto = "PENDIENTE DE FACTURAR"
        Texto = "PRODUCCIÓN DÍA " & Me.mFecha


        Me.DepartamentosForFait = ""

        SQL = "SELECT SERV_COAP ,SERV_COPA,SERV_CORE FROM TNHT_PARA "

        Me.DbLeeHotel.TraerLector(SQL)
        Me.DbLeeHotel.mDbLector.Read()

        If Me.DbLeeHotel.mDbLector.HasRows Then
            DepartamentosForFait = "'" & CStr(Me.DbLeeHotel.mDbLector.Item("SERV_COAP")) & "','" & CStr(Me.DbLeeHotel.mDbLector.Item("SERV_COPA")) _
            & "','" & CStr(Me.DbLeeHotel.mDbLector.Item("SERV_CORE")) & "','"
        End If
        Me.DbLeeHotel.mDbLector.Close()


        SQL = "SELECT SERV_CAME ,SERV_BERE,SERV_COCE,SERV_BECE FROM TNHT_PAR1 "

        Me.DbLeeHotel.TraerLector(SQL)
        Me.DbLeeHotel.mDbLector.Read()

        If Me.DbLeeHotel.mDbLector.HasRows Then
            DepartamentosForFait += CStr(Me.DbLeeHotel.mDbLector.Item("SERV_CAME")) & "','" & CStr(Me.DbLeeHotel.mDbLector.Item("SERV_BERE")) _
            & "','" & CStr(Me.DbLeeHotel.mDbLector.Item("SERV_COCE")) & "','" & CStr(Me.DbLeeHotel.mDbLector.Item("SERV_BECE")) & "'"
        End If
        Me.DbLeeHotel.mDbLector.Close()

        ' SE AÑADE EL  servicio "002"  = pension que inicialmente formo parte del forfait antes del desglose 
        DepartamentosForFait = "(" & DepartamentosForFait & ",'002'" & ")"


        '**********************************************************
        '
        'FORFAIT CLIENTES DIRECTOS RESE_TRES = 1     O DE AGENCIAS DE PAGO   ENTIFAMA = 2
        '
        '**********************************************************
        Dim Total As Double
        Dim vCentroCosto As String
        Dim Cuenta As String = ""
        Dim TipoCuenta As String = ""



        SQL = "SELECT  "
        SQL += "DECODE(ENTI_FAMA,'1','ENTIDAD','2','PRIVADO','PRIVADO') AS ENTI_FAMA, "
        SQL += "TNHT_MOVI.SECC_CODI, "
        SQL += "         TNHT_MOVI.SERV_CODI SERVICIO, "
        SQL += "         TNHT_MOVI.SECC_CODI SECCION, "

        SQL += "         TNHT_SERV.SERV_DESC DEPARTAMENTO, "
        SQL += "         NVL (TNHT_SERV.SERV_CMBD, '0') CUENTAAG, "
        SQL += "         NVL (TNHT_SERV.SERV_CTB1, '0') CUENTADIR, "
        SQL += "         NVL (TNHT_SERV.SERV_COMS, '0') COMS, "
        SQL += "        ROUND (SUM (MOVI_VLIQ), 2) TOTAL "
        SQL += "    FROM TNHT_MOVI, "
        SQL += "         TNHT_SERV, "
        SQL += "         TNHT_RESE, "
        SQL += "         TNHT_ENTI "
        SQL += "   WHERE     TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "
        SQL += "         AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL += "         AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
        SQL += "         AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+) "
        SQL += "  AND MOVI_DATR= '" & Me.mFecha & "'"

        ' SOLO DEPARTAMENTOS DE PENSION
        SQL += " AND TNHT_MOVI.SERV_CODI IN " & Me.DepartamentosForFait

        SQL += "GROUP BY TNHT_MOVI.SECC_CODI, "
        SQL += "         TNHT_MOVI.SERV_CODI, "
        SQL += "         TNHT_MOVI.SECC_CODI, "
        SQL += "         TNHT_SERV.SERV_DESC, "
        SQL += "         TNHT_SERV.SERV_CMBD, "
        SQL += "         TNHT_SERV.SERV_CTB1, "
        SQL += "         TNHT_SERV.SERV_COMS, "
        SQL += "        DECODE(ENTI_FAMA,'1','ENTIDAD','2','PRIVADO','PRIVADO')  "
        SQL += " ,TNHT_SERV.SERV_TICO "
        SQL += " ORDER BY TNHT_SERV.SERV_TICO "


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            If CStr(Me.DbLeeHotel.mDbLector("ENTI_FAMA")) = "ENTIDAD" Then
                Cuenta = CStr(Me.DbLeeHotel.mDbLector("CUENTAAG"))
                TipoCuenta = "AG"
            Else
                Cuenta = CStr(Me.DbLeeHotel.mDbLector("CUENTADIR"))
                TipoCuenta = "CL"
            End If



            If vCentroCosto = "0" Then
                vCentroCosto = ""
            End If


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Total2 = Total2 + Total
            End If
        End While
        Me.DbLeeHotel.mDbLector.Close()


        ' RESTO DE SERVICIOS


        SQL = "SELECT  "
        SQL += "TNHT_MOVI.SECC_CODI, "
        SQL += "         TNHT_MOVI.SERV_CODI SERVICIO, "
        SQL += "         TNHT_MOVI.SECC_CODI SECCION, "
        SQL += "         TNHT_SERV.SERV_DESC DEPARTAMENTO, "
        SQL += "         NVL (TNHT_SERV.SERV_CTB1, '0') CUENTA, "
        SQL += "         NVL (TNHT_SERV.SERV_COMS, '0') COMS, "
        SQL += "        ROUND (SUM (MOVI_VLIQ), 2) TOTAL "
        SQL += "    FROM TNHT_MOVI, "
        SQL += "         TNHT_SERV, "
        SQL += "         TNHT_RESE, "
        SQL += "         TNHT_ENTI "
        SQL += "   WHERE     TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "
        SQL += "         AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL += "         AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
        SQL += "         AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+) "
        SQL += "  AND MOVI_DATR= '" & Me.mFecha & "'"

        ' SOLO DEPARTAMENTOS DE PENSION
        SQL += " AND TNHT_MOVI.SERV_CODI NOT IN " & Me.DepartamentosForFait

        SQL += "GROUP BY TNHT_MOVI.SECC_CODI, "
        SQL += "         TNHT_MOVI.SERV_CODI, "
        SQL += "         TNHT_MOVI.SECC_CODI, "
        SQL += "         TNHT_SERV.SERV_DESC, "
        SQL += "         TNHT_SERV.SERV_CTB1, "
        SQL += "         TNHT_SERV.SERV_COMS "


        '  SQL += " ORDER BY TNHT_SERV.SERV_TICO "


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            TipoCuenta = "CL"

            If vCentroCosto = "0" Then
                vCentroCosto = ""
            End If


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Total2 = Total2 + Total
            End If
        End While


        If Total2 <> 0 Then
            Linea = 1
            Me.mTipoAsiento = "DEBE"

            Me.mTotalProduccion = Total

            Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorDebe, Texto, Total2, "SI", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorDebe, Texto, Total2)

        End If

        Me.DbLeeHotel.mDbLector.Close()








    End Sub

    Private Sub VentasDepartamentoBloque()

        Dim Total As Double
        Dim vCentroCosto As String

        SQL = "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI SERVICIO,TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CMBD,'0') CUENTA ,"
        SQL += "ROUND (SUM (MOVI_VLIQ), 2) TOTAL "
        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_SERV"
        SQL += " WHERE (TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI) AND MOVI_DATR= '" & Me.mFecha & "'"
        '    SQL += " AND TNHT_SERV.SERV_CODI <> '" & Me.mParaIngresoPorhabitacionDpto & "'"
        SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CMBD,TNHT_SERV.SERV_COMS"
        SQL += " ORDER BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CMBD"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total, "NO", "", vCentroCosto, "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total)
                If vCentroCosto <> "0" Then
                    Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", vCentroCosto, Total)
                End If
            End If
        End While
        Me.DbLeeHotel.mDbLector.Close()


        ' INGRESO DE ALOJAMIENTO POR BLOQUE

        SQL = "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI SERVICIO,TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CMBD,'0') CUENTA ,NVL(BLAL_DESC,'OTROS INGRESOS') AS BLOQUE,"
        SQL += "ROUND (SUM (MOVI_VLIQ), 2) TOTAL "
        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_SERV,TNHT_ALOJ,TNHT_BLAL"
        SQL += " WHERE TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "
        SQL += " AND TNHT_MOVI.ALOJ_CODI = TNHT_ALOJ.ALOJ_CODI(+) "
        SQL += " AND TNHT_ALOJ.BLAL_CODI = TNHT_BLAL.BLAL_CODI(+) "
        SQL += " AND MOVI_DATR= '" & Me.mFecha & "'"
        '      SQL += " AND TNHT_SERV.SERV_CODI = '" & Me.mParaIngresoPorhabitacionDpto & "'"
        SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CMBD,TNHT_SERV.SERV_COMS,BLAL_DESC"
        SQL += " ORDER BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CMBD"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("BLOQUE"), String), Total, "NO", "", vCentroCosto, "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("BLOQUE"), String), Total)
                If vCentroCosto <> "0" Then
                    Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", vCentroCosto, Total)
                End If
            End If
        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub VentasDepartamentoBloqueTest()

        Dim Total As Double
        Dim vCentroCosto As String

        Try


            ' INGRESO DE ALOJAMIENTO POR BLOQUE

            SQL = "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI SERVICIO,TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CMBD,'0') CUENTA ,NVL(BLAL_DESC,'OTROS INGRESOS') AS BLOQUE,TNHT_BLAL.BLAL_CODI,"
            SQL += "ROUND(SUM(MOVI_VLIQ), 2) TOTAL "
            SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_SERV,TNHT_ALOJ,TNHT_BLAL"
            SQL += " WHERE TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "
            SQL += " AND TNHT_MOVI.ALOJ_CODI = TNHT_ALOJ.ALOJ_CODI(+) "
            SQL += " AND TNHT_ALOJ.BLAL_CODI = TNHT_BLAL.BLAL_CODI(+) "
            SQL += " AND MOVI_DATR= '" & Me.mFecha & "'"
            SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CMBD,TNHT_SERV.SERV_COMS,BLAL_DESC,TNHT_BLAL.BLAL_CODI"
            SQL += " ORDER BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CMBD"


            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                If IsDBNull(Me.DbLeeHotel.mDbLector("BLAL_CODI")) = False Then
                    SQL = "SELECT NVL(SERV_COMS,'0') FROM TH_SERV_BLAL WHERE "
                    SQL += " HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND "
                    SQL += " HOTEL_EMP_COD = '" & Me.mEmpCod & "' AND "
                    SQL += "SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "' AND "
                    SQL += "BLAL_CODI = '" & CType(Me.DbLeeHotel.mDbLector("BLAL_CODI"), String) & "'"
                    vCentroCosto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)
                Else
                    ' OTROS INGRESOS ( se coge centro de costo por defecto de la tabla de servicios de newhotel)
                    SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
                    vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                End If


                ' EL DEPARTAMENTO NO EXISTE EN LA TABLA DE CENTROS DE COSTO MIA

                If IsNothing(vCentroCosto) = True Then
                    MsgBox("Atención  el Departamento " & CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " no existe en la Tabla de Centros de Costo por Bloques de Alojamiento " & vbCrLf & vbCrLf & "Se asume sin Centro de Costo", MsgBoxStyle.Information, "Atención Posible  nuevo Departamento Creado ")
                    vCentroCosto = "0"
                End If


                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("BLOQUE"), String), Total, "NO", "", vCentroCosto, "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("BLOQUE"), String), Total)
                    If vCentroCosto <> "0" Then
                        Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", vCentroCosto, Total)
                    End If
                End If
            End While
            Me.DbLeeHotel.mDbLector.Close()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
#End Region
#Region "ASIENTO-2 PAGOS A CUENTA"
    Private Sub TotalPagosaCuentaVisas()
        Try
            Dim Total As Double
            Dim Cuenta As String
            Dim CcExCodi As String

            Dim ReseCodi As String
            Dim ReseAnci As String
            Dim AlojCodi As String



            SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA,"
            SQL = SQL & "NVL(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),'?') RECI_COBR,NVL(MOVI_NUDO,' ') MOVI_NUDO,NVL(MOVI_DESC,' ') MOVI_DESC,NVL(SECC_CODI,'?') AS SECC_CODI"
            SQL = SQL & " ,TNHT_MOVI.CCEX_CODI AS CCEX_CODI,NVL(TNHT_CCEX.CCEX_TITU,' ') AS CCEX_TITU "

            SQL = SQL & ",TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI,TNHT_MOVI.ALOJ_CODI "

            SQL = SQL & " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_CACR,TNHT_RESE,TNHT_CCEX  "
            SQL = SQL & " WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"

            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"

            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' excluir depositos anticipados 
            'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"


            If Me.mTrataDebitoTpvnoFacturado = True Then
                ' EXCLUYE CIERRE DE CONTADO DE TPV
                SQL += " AND TNHT_MOVI.UTIL_CODI <> 'POS'"
                ' EXCLUYE CIERRE DE CONTADO DE GOLF
                SQL += " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"
            End If


            If Me.mUsaTnhtMoviAuto = True Then
                SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
            End If

            SQL = SQL & " ORDER BY TNHT_MOVI.MOVI_HORE ASC "
            '
            '  SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CTBA"


            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read

                If Me.mParaUsaCta4b = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                    Cuenta = Me.mParaCta4b3Visa
                Else
                    Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
                End If

                If IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = False Then
                    CcExCodi = CStr(Me.DbLeeHotel.mDbLector("CCEX_CODI"))
                Else
                    CcExCodi = Nothing
                End If

                If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_CODI")) = False Then
                    ReseCodi = CInt(Me.DbLeeHotel.mDbLector("RESE_CODI"))
                Else
                    ReseCodi = Nothing
                End If

                If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_ANCI")) = False Then
                    ReseAnci = CInt(Me.DbLeeHotel.mDbLector("RESE_ANCI"))
                Else
                    ReseAnci = Nothing
                End If


                If IsDBNull(Me.DbLeeHotel.mDbLector("ALOJ_CODI")) = False Then
                    AlojCodi = CStr(Me.DbLeeHotel.mDbLector("ALOJ_CODI"))
                Else
                    AlojCodi = ""
                End If


                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Linea = Linea + 1
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", "", "SI", "ANTICIPO RECIBIDO", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), CcExCodi, "", ReseCodi, ReseAnci, AlojCodi)
                    Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, Me.Multidiario)
                End If


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
        End Try

    End Sub
    Private Sub TotalPagosaCuentaOtrasFormas()
        Dim Total As Double
        Dim Cuenta As String

        Dim CcExCodi As String

        Dim ReseCodi As String
        Dim ReseAnci As String
        Dim AlojCodi As String

        SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,"
        SQL = SQL & "NVL(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),'?') RECI_COBR,NVL(MOVI_NUDO,' ') MOVI_NUDO,NVL(MOVI_DESC,' ') MOVI_DESC,NVL(SECC_CODI,'?') AS SECC_CODI "
        SQL = SQL & " ,TNHT_MOVI.CCEX_CODI AS CCEX_CODI,NVL(TNHT_CCEX.CCEX_TITU,' ') AS CCEX_TITU "
        SQL = SQL & ",TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI,TNHT_MOVI.ALOJ_CODI "
        SQL = SQL & " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FORE,TNHT_RESE,TNHT_CCEX "
        SQL = SQL & " WHERE TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"

        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"

        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"


        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"


        If Me.mTrataDebitoTpvnoFacturado = True Then
            ' EXCLUYE CIERRE DE CONTADO DE TPV
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'POS'"
            ' EXCLUYE CIERRE DE CONTADO DE GOLF
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"
        End If



        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If
        SQL = SQL & " ORDER BY TNHT_MOVI.MOVI_HORE ASC "

        '   SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"
        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read

            If Me.mParaUsaCta4b = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b2Efectivo
            Else
                Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = False Then
                CcExCodi = CStr(Me.DbLeeHotel.mDbLector("CCEX_CODI"))
            Else
                CcExCodi = Nothing
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_CODI")) = False Then
                ReseCodi = CInt(Me.DbLeeHotel.mDbLector("RESE_CODI"))
            Else
                ReseCodi = Nothing
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_ANCI")) = False Then
                ReseAnci = CInt(Me.DbLeeHotel.mDbLector("RESE_ANCI"))
            Else
                ReseAnci = Nothing
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("ALOJ_CODI")) = False Then
                AlojCodi = CStr(Me.DbLeeHotel.mDbLector("ALOJ_CODI"))
            Else
                AlojCodi = ""
            End If


            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", "", "SI", "ANTICIPO RECIBIDO", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), CcExCodi, "", ReseCodi, ReseAnci, AlojCodi)
                Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, Me.Multidiario)
            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetallePagosaCuentaVisas()
        Dim Total As Double
        Dim Cuenta As String = ""
        Dim CcExCodi As String

        Dim ReseCodi As String
        Dim ReseAnci As String
        Dim AlojCodi As String

        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,NVL(MOVI_DESC,' ') MOVI_DESC,"
        SQL = SQL & " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA,NVL(SECC_CODI,'?') AS SECC_CODI "
        SQL = SQL & " ,TNHT_MOVI.CCEX_CODI AS CCEX_CODI,NVL(TNHT_CCEX.CCEX_TITU,' ') AS CCEX_TITU "
        SQL = SQL & ",TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI,TNHT_MOVI.ALOJ_CODI "
        SQL = SQL & " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,"
        SQL = SQL & " TNHT_CACR,TNHT_RESE,TNHT_CCEX  WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"

        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"


        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mTrataDebitoTpvnoFacturado = True Then
            ' EXCLUYE CIERRE DE CONTADO DE TPV
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'POS'"
            ' EXCLUYE CIERRE DE CONTADO DE GOLF
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"
        End If



        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If
        SQL = SQL & " ORDER BY TNHT_MOVI.MOVI_HORE ASC "

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read

            If Me.mParaUsaCta4b = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b
            Else
                Cuenta = Me.mCtaPagosACuenta
            End If

            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

            If IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = False Then
                CcExCodi = CStr(Me.DbLeeHotel.mDbLector("CCEX_CODI"))
            Else
                CcExCodi = Nothing
            End If



            If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_CODI")) = False Then
                ReseCodi = CInt(Me.DbLeeHotel.mDbLector("RESE_CODI"))
            Else
                ReseCodi = Nothing
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_ANCI")) = False Then
                ReseAnci = CInt(Me.DbLeeHotel.mDbLector("RESE_ANCI"))
            Else
                ReseAnci = Nothing
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("ALOJ_CODI")) = False Then
                AlojCodi = CStr(Me.DbLeeHotel.mDbLector("ALOJ_CODI"))
            Else
                AlojCodi = ""
            End If

            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), CcExCodi, ReseCodi, ReseAnci, AlojCodi)
                Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, Me.Multidiario)
            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetallePagosaCuentaOtrasFormas()
        Dim Total As Double
        Dim Cuenta As String = ""
        Dim CcExCodi As String

        Dim ReseCodi As String
        Dim ReseAnci As String
        Dim AlojCodi As String

        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE,TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_DAVA,NVL(MOVI_DESC,' ') MOVI_DESC,NVL(SECC_CODI,'?') AS SECC_CODI "

        SQL = SQL & " ,TNHT_MOVI.CCEX_CODI AS CCEX_CODI ,NVL(TNHT_CCEX.CCEX_TITU,' ') AS CCEX_TITU "

        SQL = SQL & ",TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI,TNHT_MOVI.ALOJ_CODI "
        SQL = SQL & " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FORE,TNHT_RESE,TNHT_CCEX WHERE "

        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"

        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"

        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"

        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"


        If Me.mTrataDebitoTpvnoFacturado = True Then
            ' EXCLUYE CIERRE DE CONTADO DE TPV
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'POS'"
            ' EXCLUYE CIERRE DE CONTADO DE GOLF
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"
        End If



        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        SQL = SQL & " ORDER BY TNHT_MOVI.MOVI_HORE ASC "

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read

            If Me.mParaUsaCta4b = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b
            Else
                Cuenta = Me.mCtaPagosACuenta
            End If

            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

            If IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = False Then
                CcExCodi = CStr(Me.DbLeeHotel.mDbLector("CCEX_CODI"))
            Else
                CcExCodi = Nothing
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_CODI")) = False Then
                ReseCodi = CInt(Me.DbLeeHotel.mDbLector("RESE_CODI"))
            Else
                ReseCodi = Nothing
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_ANCI")) = False Then
                ReseAnci = CInt(Me.DbLeeHotel.mDbLector("RESE_ANCI"))
            Else
                ReseAnci = Nothing
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("ALOJ_CODI")) = False Then
                AlojCodi = CStr(Me.DbLeeHotel.mDbLector("ALOJ_CODI"))
            Else
                AlojCodi = ""
            End If


            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                ' LLAMADA ORIGINAL BLOQUEADA 
                'Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), CcExCodi)
                Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), CcExCodi, ReseCodi, ReseAnci, AlojCodi)
                Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, Me.Multidiario)
            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub TotalPagosaCuentaVisasComision()
        Try
            Dim Total As Double
            Dim TotalComision As Double

            Dim vCentroCosto As String

            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA,NVL(TNHT_CACR.CACR_CONT,'0') CUENTAGASTO,TNHT_CACR.CACR_COMI"
            SQL = SQL & " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' SOLO VISAS CON COMISION
            SQL = SQL & " AND TNHT_CACR.CACR_COMI > 0 "
            ' excluir depositos anticipados 
            'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"


            If Me.mTrataDebitoTpvnoFacturado = True Then
                ' EXCLUYE CIERRE DE CONTADO DE TPV
                SQL += " AND TNHT_MOVI.UTIL_CODI <> 'POS'"
                ' EXCLUYE CIERRE DE CONTADO DE GOLF
                SQL += " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"
            End If



            If Me.mUsaTnhtMoviAuto = True Then
                SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
            End If
            '
            SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CTBA,TNHT_CACR.CACR_CONT,TNHT_CACR.CACR_COMI"




            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read


                SQL = "SELECT NVL(PARA_CENTRO_COSTO_COMI,'0') FROM TH_PARA "
                SQL += " WHERE  PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                vCentroCosto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)


                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Linea = Linea + 1
                    TotalComision = (CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * CType(Me.DbLeeHotel.mDbLector("CACR_COMI"), Double)) / 100
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", "", "SI", "", Me.Multidiario, "")
                    Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, Me.Multidiario)

                    Linea = Linea + 1
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTAGASTO"), String), Me.mIndicadorDebe, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", vCentroCosto, "SI", "", Me.Multidiario, "")
                    Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTAGASTO"), String), Me.mIndicadorDebe, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, Me.Multidiario)
                End If



            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
        End Try

    End Sub
#End Region

#Region "ASIENTO-3"
    Private Sub NFacturasTotalLiquidoDepartamentoAlojamiento(ByVal vTipo As String)
        Try
            Dim Total As Double
            Dim vCentroCosto As String


            SQL = "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI AS SERVICIO, 'FACTURADO ' || TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CTB1,'0') CUENTA,NVL(TNHT_SERV.SERV_CMBD,'0') CUENTA2,ROUND (SUM (MOVI_VLIQ), 2) TOTAL,NVL(MOPE_DESC,'Ningun') AS REGIMEN"
            SQL += " FROM TNHT_FAMO, TNHT_MOVI, TNHT_FACT, TNHT_SERV,TNHT_MOPE "
            SQL += " WHERE TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"
            SQL += " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            SQL += " AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
            SQL += " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
            SQL += " AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI"
            SQL += " AND TNHT_MOVI.MOPE_CODI = TNHT_MOPE.MOPE_CODI(+)"
            SQL += " AND FACT_DAEM = " & "'" & Me.mFecha & "'"

            If mDesglosaAlojamientoporRegimen = True Then
                SQL += " AND TNHT_SERV.SERV_CODI = '" & Me.mParaServicioAlojamiento & "'"
            End If

            SQL += " AND MOVI_TIMO = 1"
            SQL += " AND FAAN_CODI IS NULL"
            SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,TNHT_MOPE.MOPE_DESC,TNHT_SERV.SERV_CMBD"

            SQL += " UNION "

            SQL += "SELECT   TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI AS SERVICIO, 'DES-FACTURADO ' || TNHT_SERV.SERV_DESC DEPARTAMENTO  ,NVL(TNHT_SERV.SERV_CTB1,'0') CUENTA,NVL(TNHT_SERV.SERV_CMBD,'0') CUENTA2,ROUND (SUM (MOVI_VLIQ * -1), 2) TOTAL,NVL(MOPE_DESC,'Ningun') AS REGIMEN"
            SQL += " FROM TNHT_FAMO, TNHT_MOVI, TNHT_FACT, TNHT_SERV,TNHT_MOPE "
            SQL += " WHERE TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            SQL += " AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
            SQL += " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
            SQL += " AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI"
            SQL += " AND TNHT_MOVI.MOPE_CODI = TNHT_MOPE.MOPE_CODI(+)"
            SQL += " AND FACT_DAEM = " & "'" & Me.mFecha & "'"

            If mDesglosaAlojamientoporRegimen = True Then
                SQL += " AND TNHT_SERV.SERV_CODI = '" & Me.mParaServicioAlojamiento & "'"
            End If

            SQL += " AND MOVI_TIMO = 1"
            SQL += " AND FAAN_CODI IS NOT NULL "
            SQL += " GROUP BY  TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,TNHT_MOPE.MOPE_DESC,TNHT_SERV.SERV_CMBD"



            Me.DbLeeHotel.TraerLector(SQL)



            While Me.DbLeeHotel.mDbLector.Read
                SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
                vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    If vTipo = "HABER" Then
                        Me.mTipoAsiento = "HABER"
                        Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " + " & CType(Me.DbLeeHotel.mDbLector("REGIMEN"), String), Total, "NO", "", vCentroCosto, "SI")
                        Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " + " & CType(Me.DbLeeHotel.mDbLector("REGIMEN"), String), Total)
                        If vCentroCosto <> "0" Then
                            Me.GeneraFileAA("AA", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", vCentroCosto, Total)
                        End If

                    Else
                        Me.mTipoAsiento = "DEBE"
                        Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, "*" & CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " + " & CType(Me.DbLeeHotel.mDbLector("REGIMEN"), String), Total, "NO", "", vCentroCosto, "SI")
                        Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), "*" & CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " + " & CType(Me.DbLeeHotel.mDbLector("REGIMEN"), String), Total)

                    End If



                End If
            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try


    End Sub
    Private Sub NFacturasTotalLiquidoDepartamento(ByVal vTipo As String)
        Try
            Dim Total As Double
            Dim vCentroCosto As String


            '  SQL = "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI AS SERVICIO, 'FACTURADO ' || TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CTB1,'0') CUENTA,NVL(TNHT_SERV.SERV_CMBD,'0') CUENTA2,ROUND (SUM (MOVI_VLIQ), 2) TOTAL"
            '  SQL += " FROM TNHT_FAMO, TNHT_MOVI, TNHT_FACT, TNHT_SERV "
            '  SQL += " WHERE TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"
            '  SQL += " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            '  SQL += " AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
            '  SQL += " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
            '  SQL += " AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI"
            '  SQL += " AND FACT_DAEM = " & "'" & Me.mFecha & "'"

            '  If mDesglosaAlojamientoporRegimen = True Then
            ' SQL += " AND TNHT_SERV.SERV_CODI <> '" & Me.mParaServicioAlojamiento & "'"
            ' End If

            ' SQL += " AND MOVI_TIMO = 1"
            ' SQL += " AND FAAN_CODI IS NULL"
            ' SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,TNHT_SERV.SERV_CMBD"

            ' SQL += " UNION "

            'SQL += "SELECT   TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI AS SERVICIO, 'DES-FACTURADO ' || TNHT_SERV.SERV_DESC DEPARTAMENTO  ,NVL(TNHT_SERV.SERV_CTB1,'0') CUENTA,NVL(TNHT_SERV.SERV_CMBD,'0') CUENTA2,ROUND (SUM (MOVI_VLIQ * -1), 2) TOTAL"
            'SQL += " FROM TNHT_FAMO, TNHT_MOVI, TNHT_FACT, TNHT_SERV"
            'SQL += " WHERE TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            'SQL += " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            'SQL += " AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
            'SQL += " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
            'SQL += " AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI"
            'SQL += " AND FACT_DAEM = " & "'" & Me.mFecha & "'"

            'If mDesglosaAlojamientoporRegimen = True Then
            'SQL += " AND TNHT_SERV.SERV_CODI <> '" & Me.mParaServicioAlojamiento & "'"
            'End If

            'SQL += " AND MOVI_TIMO = 1"
            'SQL += " AND FAAN_CODI IS NOT NULL "
            'SQL += " GROUP BY  TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,TNHT_SERV.SERV_CMBD"



            SQL = "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI AS SERVICIO, 'FACTURADO ' || TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CTB1,'0') CUENTA,NVL(TNHT_SERV.SERV_CMBD,'0') CUENTA2,ROUND (SUM (MOVI_VLIQ), 2) TOTAL"

            SQL += "    FROM TNHT_FAMO, TNHT_MOVI, TNHT_FACT, TNHT_SERV "
            SQL += "   WHERE TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += "     AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += "     AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "     AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "     AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "
            SQL += "     AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
            SQL += "     AND MOVI_TIMO = 1 "
            SQL += " AND FAAN_CODI IS  NULL "

            If mDesglosaAlojamientoporRegimen = True Then
                SQL += " AND TNHT_SERV.SERV_CODI <> '" & Me.mParaServicioAlojamiento & "'"
            End If


            'If Me.mSerruchaDepartamentos = True Then
            ' SQL += " AND DECODE(SERV_PCRM,null,0,SERV_PCRM) <> 99 "
            ' End If


            SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,TNHT_SERV.SERV_CMBD"


            SQL += " UNION "

            SQL += "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI AS SERVICIO, 'DES-FACTURADO ' || TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CTB1,'0') CUENTA,NVL(TNHT_SERV.SERV_CMBD,'0') CUENTA2,ROUND (SUM (MOVI_VLIQ * -1), 2) TOTAL"

            SQL += "    FROM TNHT_FAMO, TNHT_MOVI, TNHT_FACT, TNHT_SERV "
            SQL += "   WHERE TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += "     AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += "     AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "     AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "     AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "
            SQL += "     AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
            SQL += "     AND MOVI_TIMO = 1 "
            SQL += " AND FAAN_CODI IS NOT NULL "

            If mDesglosaAlojamientoporRegimen = True Then
                SQL += " AND TNHT_SERV.SERV_CODI <> '" & Me.mParaServicioAlojamiento & "'"
            End If


            If Me.mSerruchaDepartamentos = True Then
                SQL += " AND DECODE(SERV_PCRM,null,0,SERV_PCRM) <> 99 "
            End If


            SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,TNHT_SERV.SERV_CMBD"

            Me.DbLeeHotel.TraerLector(SQL)



            While Me.DbLeeHotel.mDbLector.Read
                SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
                vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    If vTipo = "HABER" Then
                        Me.mTipoAsiento = "HABER"
                        Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total, "NO", "", vCentroCosto, "SI")
                        Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total)
                        If vCentroCosto <> "0" Then
                            Me.GeneraFileAA("AA", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", vCentroCosto, Total)
                        End If

                    Else
                        Me.mTipoAsiento = "DEBE"
                        Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, "*" & CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total, "NO", "", vCentroCosto, "SI")
                        Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), "*" & CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total)

                    End If



                End If
            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try


    End Sub
    Private Sub NFacturasTotalLiquidoAgrupado()
        Try
            Dim Total As Double

            Dim Descripcion As String


            SQL = "SELECT '0' AS ANULADO, NVL(ROUND (SUM (MOVI_VLIQ), 2),0) TOTAL"
            SQL += " FROM TNHT_FAMO, TNHT_MOVI, TNHT_FACT, TNHT_SERV "
            SQL += " WHERE TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"
            SQL += " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            SQL += " AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
            SQL += " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
            SQL += " AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI"
            SQL += " AND FACT_DAEM = " & "'" & Me.mFecha & "'"
            SQL += " AND MOVI_TIMO = 1"
            SQL += " AND FAAN_CODI IS NULL"

            SQL += " UNION "

            SQL += "SELECT  '1' AS ANULADO,NVL(ROUND (SUM (MOVI_VLIQ * -1), 2),0) TOTAL"
            SQL += " FROM TNHT_FAMO, TNHT_MOVI, TNHT_FACT, TNHT_SERV"
            SQL += " WHERE TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            SQL += " AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
            SQL += " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
            SQL += " AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI"
            SQL += " AND FACT_DAEM = " & "'" & Me.mFecha & "'"
            SQL += " AND MOVI_TIMO = 1"
            SQL += " AND FAAN_CODI IS NOT NULL "


            Me.DbLeeHotel.TraerLector(SQL)



            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

                If CType(Me.DbLeeHotel.mDbLector("ANULADO"), String) = "0" Then
                    Descripcion = "PENDIENTE DE FACTURAR "
                Else
                    Descripcion = "PENDIENTE DE FACTURAR ANULADO "
                End If
                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, Descripcion & Me.mFecha, Total, "NO", "", "", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, Descripcion & Me.mFecha, Total)
                End If
            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try


    End Sub
    Private Sub NFacturasTotalLiquidoPorSerie()
        Try

            Dim Total As Double
            Dim TotalComisiones As Double
            Dim SQL As String


            SQL = "SELECT (SUM(FAIV_INCI) - SUM(FAIV_VIMP)) TOTAL ,SUM(FACT_TOTA) TOTAL1,FACT_DAEM,TNHT_FACT.SEFA_CODI AS SERIE "
            SQL += "FROM TNHT_FAIV, TNHT_FACT "
            SQL += "WHERE TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
            SQL += "GROUP BY TNHT_FACT.FACT_DAEM,TNHT_FACT.SEFA_CODI"


            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) + Me.FacturacionSalidasServiciosSinIgic + Me.FacturacionSalidaDesembolsos

                SQL = "SELECT NVL(SUM(TNHT_DESF.DESF_VALO),'0')TOTAL "
                SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE WHERE"
                SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
                SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
                SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
                SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
                SQL = SQL & " AND TNHT_FACT.SEFA_CODI = " & "'" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String) & "'"


                If Me.mParaComisiones = True Then
                    TotalComisiones = CType(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL), Double)
                    Total = Total + TotalComisiones
                End If
                Total = Decimal.Round(CType(Total, Decimal), 2)

                If Total <> 0 Then

                    Me.mTipoAsiento = "HABER"
                    Me.mTotalFacturacion = Total

                    Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "FACTURACION " & Me.mFecha & " " & CType(Me.DbLeeHotel.mDbLector("SERIE"), String) & " + Dto Financieros", Total, "SI", "", "", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, "FACTURACION " & " " & CType(Me.DbLeeHotel.mDbLector("SERIE"), String) & Me.mFecha, Total)

                End If

            End While

            Me.DbLeeHotel.mDbLector.Close()


        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención ")

        End Try



    End Sub

    Private Sub NFacturasSalidaTotaLDescuentos()
        Try
            Dim Total As Double
            SQL = "SELECT  TIDE_PORC,( SUM(MOVI_VLIQ) * TIDE_PORC) / 100 AS TOTAL,NVL(TNHT_TIDE.TIDE_CTB1,'0') CUENTA,TNHT_TIDE.TIDE_DESC TIPO  "
            SQL += "       FROM TNHT_FAMO, TNHT_MOVI,TNHT_FACT,TNHT_SERV,TNHT_DESF,TNHT_TIDE "
            SQL += "      WHERE TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += "        AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += "        AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "        AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "        AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "
            SQL += "        "
            SQL += "        AND TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI "
            SQL += "        AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI "
            SQL += "        AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI "
            SQL += "         "
            SQL += "  AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
            SQL += "        AND MOVI_TIMO = 1 "
            SQL += "        AND FAAN_CODI IS NULL "
            SQL += "         "
            SQL += "        GROUP BY TIDE_PORC,NVL(TNHT_TIDE.TIDE_CTB1,'0'),TNHT_TIDE.TIDE_DESC "







            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

                    Me.GeneraFileAA("AA", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", Me.mParaCentroCostoAlojamiento, Total)
                End If

            End While
            Me.DbLeeHotel.mDbLector.Close()


            SQL = "SELECT  TIDE_PORC,( SUM(MOVI_VLIQ) * TIDE_PORC) / 100 AS TOTAL,NVL(TNHT_TIDE.TIDE_CTB1,'0') CUENTA,TNHT_TIDE.TIDE_DESC TIPO  "
            SQL += "       FROM TNHT_FAMO, TNHT_MOVI,TNHT_FACT,TNHT_SERV,TNHT_DESF,TNHT_TIDE "
            SQL += "      WHERE TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += "        AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += "        AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "        AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "        AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "
            SQL += "        "
            SQL += "        AND TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI "
            SQL += "        AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI "
            SQL += "        AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI "
            SQL += "         "
            SQL += "  AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
            SQL += "        AND MOVI_TIMO = 1 "
            SQL += "        AND FAAN_CODI IS NOT NULL "
            SQL += "         "
            SQL += "        GROUP BY TIDE_PORC,NVL(TNHT_TIDE.TIDE_CTB1,'0'),TNHT_TIDE.TIDE_DESC "







            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

                    Me.GeneraFileAA("AA", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", Me.mParaCentroCostoAlojamiento, Total)
                End If

            End While
            Me.DbLeeHotel.mDbLector.Close()

        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub
    Private Sub NFacturasSalidaTotalFActura()
        Try
            Dim TotalFactura As Double

            Dim TotalPendiente As Double
            Dim TotalDiferencia As Double

            Dim Dni As String
            Dim Cuenta As String = "0"
            Dim Titular As String
            ' TOTAL FACTURA DESPUES DEL DESCUENTO FONANCIERO 
            SQL = "SELECT  TNHT_FACT.FACT_STAT AS ESTADO, TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,TNHT_FACT.FACT_TOTA TOTAL,TNHT_FACT.FACT_VALO VALOR,TNHT_FACT.FACT_CONT PENDIENTE,NVL(ENTI_CODI,'') AS ENTI_CODI,NVL(CCEX_CODI,'?') AS CCEX_CODI,NVL(CLIE_CODI,'0') AS CLIENTE "
            SQL += " , NVL(TNHT_FACT.FACT_TITU,'') TITULAR ,TNHT_FACT.FAAN_CODI "
            SQL += "FROM TNHT_FACT "
            SQL += "WHERE "
            SQL += "(TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "
            SQL += " ORDER BY TNHT_FACT.FACT_CODI ASC"




            '       Dim GetNewHotel As New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)


            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read


                Linea = Linea + 1

                Cuenta = ""
                Dni = "0"

                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)
                TotalPendiente = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("PENDIENTE"), Decimal), 2)

                ' DETERMINAR EL TIPO DE FACTURA 
                ' FACTURA DE CONTADO 

                If CType(Me.DbLeeHotel.mDbLector("ESTADO"), String) = "1" Then
                    SQL = "SELECT NVL(CLIE_NUID,'0') FROM TNHT_CLIE WHERE CLIE_CODI = " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), Integer)
                    Dni = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    If Dni = "0" Or IsNothing(Dni) = True Then
                        Dni = Me.mClientesContadoCif
                    End If
                    Cuenta = Me.mCtaClientesContado
                Else
                    Dni = ""
                End If

                ' FACTURA DE ENTIDAD

                If CType(Me.DbLeeHotel.mDbLector("ESTADO"), String) = "2" And IsDBNull(Me.DbLeeHotel.mDbLector("ENTI_CODI")) = False Then
                    SQL = "SELECT NVL(ENTI_NCON_AF,0) CUENTA FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                    Cuenta = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    If Cuenta = "0" Or IsNothing(Cuenta) = True Then
                        Cuenta = "0"
                    End If

                    SQL = "SELECT NVL(ENTI_NUCO,'0') FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                    Dni = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    If Dni = "0" Or IsNothing(Dni) = True Then
                        Dni = "0"
                    End If



                End If


                ' FACTURA DE CUENTA NO ALOJADO 

                If CType(Me.DbLeeHotel.mDbLector("ESTADO"), String) = "2" And CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) <> "?" Then
                    SQL = "SELECT NVL(CCEX_NCON,0) CUENTA FROM TNHT_CCEX WHERE CCEX_CODI = '" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "'"
                    Cuenta = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    If Cuenta = "0" Or IsNothing(Cuenta) = True Then
                        Cuenta = "0"
                    End If


                    SQL = "SELECT NVL(CCEX_NUCO,'0') FROM TNHT_CCEX WHERE CCEX_CODI = '" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "'"
                    Dni = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    If Dni = "0" Or IsNothing(Dni) = True Then
                        Dni = "0"
                    End If
                End If


                ' compone 5 y 6 digito cuenta de cliente 
                '   If Cuenta <> Me.mCtaClientesContado Then
                'Cuenta = Mid(Cuenta, 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(Cuenta, 5, 6)
                'End If

                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)






                Me.mTipoAsiento = "DEBE"
                '     Me.InsertaOracle("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Dni, Titular, "SI")
                Me.GeneraFileFV("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalFactura, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String).PadRight(15, CChar(" ")), Cuenta, Dni, TotalFactura)


                Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer))
                Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Dni, Titular, "SI")



                If TotalFactura > TotalPendiente Then
                    TotalDiferencia = TotalFactura - TotalPendiente
                    '       MsgBox(TotalDiferencia)
                End If

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas")
        End Try
    End Sub
    Private Sub NFacturasSalidaTotalFActuraNuevo()
        Try
            Dim TotalFactura As Double

            Dim TotalPendiente As Double
            Dim TotalDiferencia As Double

            Dim TipoFactura As String

            Dim Dni As String
            Dim Cuenta As String = "0"
            Dim Titular As String

            Dim CcExCodi As String

            Dim ReseCodi As String
            Dim ReseAnci As String
            Dim AlojCodi As String

            Dim FacturaAnulada As String



            Dim AuxDtoFinanciero As Double

            ' TOTAL FACTURA DESPUES DEL DESCUENTO FONANCIERO 
            SQL = "SELECT  TNHT_FACT.FACT_STAT AS ESTADO, TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,TNHT_FACT.FACT_TOTA TOTAL,TNHT_FACT.FACT_VALO VALOR,TNHT_FACT.FACT_CONT PENDIENTE,NVL(ENTI_CODI,'') AS ENTI_CODI,NVL(CCEX_CODI,'') AS CCEX_CODI,NVL(CLIE_CODI,'0') AS CLIENTE,NVL(TACO_CODI,'') AS TACO_CODI "
            SQL += " , NVL(TNHT_FACT.FACT_TITU,'') TITULAR ,TNHT_FACT.FAAN_CODI, FAAN_SEFA,"

            SQL += "RESE_CODI,RESE_ANCI,ALOJ_CODI "


            SQL += "FROM TNHT_FACT "
            SQL += "WHERE "
            SQL += "(TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "
            SQL += " ORDER BY TNHT_FACT.FACT_CODI ASC"


            Dim GetNewHotel As New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)


            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read


                Linea = Linea + 1

                Cuenta = ""
                Dni = "0"

                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)
                TotalPendiente = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("PENDIENTE"), Decimal), 2)

                ' DETERMINAR EL TIPO DE FACTURA 




                ' DEBUG 


                Cuenta = GetNewHotel.DevuelveCuentaContabledeFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))
                Dni = GetNewHotel.DevuelveDniCifContabledeFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))




                ' Algunos Controles


                If Dni = "0" Then
                    Me.mTexto = "NEWHOTEL: " & "CIF no válido para descripción de Movimiento =  " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String) & " " & CType(Me.DbLeeHotel.mDbLector("TITULAR"), String).Replace("'", "''")
                    Me.mListBoxDebug.Items.Add(Me.mTexto)

                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                End If


                ' FACTURAS TRANSFERIDAS A CONTABILIDAD SIBN CODIGO DE ENTIDAD NI CUENTA NO ALOJADO
                If CType(Me.DbLeeHotel.mDbLector("ESTADO"), String) = "2" Or CType(Me.DbLeeHotel.mDbLector("ESTADO"), String) = "3" Then
                    If IsDBNull(Me.DbLeeHotel.mDbLector("ENTI_CODI")) = True And IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = True And IsDBNull(Me.DbLeeHotel.mDbLector("TACO_CODI")) = True Then
                        Cuenta = InputBox("No se puede Determinar una Cuenta Contable , Factura = " & CStr(Me.DbLeeHotel.mDbLector("DESCRIPCION")) & " Titular = " & CStr(Me.DbLeeHotel.mDbLector("TITULAR")), "Atención Ingrese Cuenta (10 Dígitos)")
                        Dni = InputBox("No se puede Determinar un DNI/CIF , Factura = " & CStr(Me.DbLeeHotel.mDbLector("DESCRIPCION")) & " Titular = " & CStr(Me.DbLeeHotel.mDbLector("TITULAR")), "Atención Ingrese un Nif / Cif")
                        Me.mForm.Update()

                    End If
                End If




                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)
                '  Cuenta = Mid(Cuenta, 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(Cuenta, 5, 6)


                ' 29042014
                ' indicar si la factura es de credito o de contado 

                ' MsgBox("LLAMAR A FUNCION GetValorDescotandoFinanciero PARA CARGAR LA DESRODUCCIONPOR DESCUETO FINANCIERO  EN ALGUNA COLUMNA LIBRE ")



                ' Busca Descuentos Financieros
                AuxDtoFinanciero = Me.GetValorDescotandoFinanciero(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))



                If AuxDtoFinanciero = 0 Then

                    If CStr(Me.DbLeeHotel.mDbLector("ESTADO")) = "1" Then
                        TipoFactura = "CONTADO"
                    ElseIf CStr(Me.DbLeeHotel.mDbLector("ESTADO")) = "2" Then
                        TipoFactura = "CREDITO"
                    ElseIf CStr(Me.DbLeeHotel.mDbLector("ESTADO")) = "3" Then
                        TipoFactura = "MIXTA"
                    ElseIf CStr(Me.DbLeeHotel.mDbLector("ESTADO")) = "4" Then
                        TipoFactura = "PENDIENTE"
                    Else
                        TipoFactura = "INDETERMINADA"
                    End If
                Else
                    If CStr(Me.DbLeeHotel.mDbLector("ESTADO")) = "1" Then
                        TipoFactura = "CONTADO-DTO"
                    ElseIf CStr(Me.DbLeeHotel.mDbLector("ESTADO")) = "2" Then
                        TipoFactura = "CREDITO-DTO"
                    ElseIf CStr(Me.DbLeeHotel.mDbLector("ESTADO")) = "3" Then
                        TipoFactura = "MIXTA-DTO"
                    ElseIf CStr(Me.DbLeeHotel.mDbLector("ESTADO")) = "4" Then
                        TipoFactura = "PENDIENTE-DTO"
                    Else
                        TipoFactura = "INDETERMINADA-DTO"
                    End If
                End If



                If IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = False Then
                    CcExCodi = CStr(Me.DbLeeHotel.mDbLector("CCEX_CODI"))
                Else
                    CcExCodi = Nothing
                End If

                If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_CODI")) = False Then
                    ReseCodi = CInt(Me.DbLeeHotel.mDbLector("RESE_CODI"))
                Else
                    ReseCodi = Nothing
                End If

                If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_ANCI")) = False Then
                    ReseAnci = CInt(Me.DbLeeHotel.mDbLector("RESE_ANCI"))
                Else
                    ReseAnci = Nothing
                End If


                If IsDBNull(Me.DbLeeHotel.mDbLector("ALOJ_CODI")) = False Then
                    AlojCodi = CStr(Me.DbLeeHotel.mDbLector("ALOJ_CODI"))
                Else
                    AlojCodi = ""
                End If


                If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = False Then
                    FacturaAnulada = Me.DbLeeHotel.mDbLector("FAAN_CODI") & "/" & Me.DbLeeHotel.mDbLector("FAAN_SEFA")
                Else
                    FacturaAnulada = ""
                End If



                Me.mTipoAsiento = "DEBE"
                '     Me.InsertaOracle("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Dni, Titular, "SI")
                Me.GeneraFileFV("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalFactura, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String).PadRight(15, CChar(" ")), Cuenta, Dni, TotalFactura)


                Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer))
                Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Dni, Titular, "SI", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), TipoFactura, "", CcExCodi, ReseCodi, ReseAnci, AlojCodi, FacturaAnulada, AuxDtoFinanciero)



                If TotalFactura > TotalPendiente Then
                    TotalDiferencia = TotalFactura - TotalPendiente
                    '       MsgBox(TotalDiferencia)
                End If

            End While
            GetNewHotel.CerrarConexiones()
            '    MsgBox("CONEXIONES CERRADAS")
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas")
        End Try
    End Sub
    Private Sub NFacturasSalidaDetalleIgic()

        Try

            Dim TotalIva As Double
            Dim TotalBase As Double
            Dim TotalFactura As Double

            Dim TipoFactura As String


            Dim CcExCodi As String

            Dim ReseCodi As String
            Dim ReseAnci As String
            Dim AlojCodi As String

            Dim FacturaAnulada As String


            SQL = "SELECT   TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "TNHT_FAIV.FAIV_TAXA AS TIPO, TNHT_FAIV.FAIV_INCI,ROUND((FAIV_INCI-FAIV_VIMP),2) BASE, ROUND(TNHT_FAIV.FAIV_VIMP,2) IGIC,NVL(TIVA_CTB1,'0') CUENTA, '"
            SQL += Me.mParaTextoIva & " ' || FAIV_TAXA ||'%  '|| TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,ROUND(TNHT_FACT.FACT_TOTA,2) TOTAL,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X "

            SQL += " ,TNHT_FACT.FACT_STAT AS ESTADO "

            SQL += ",TNHT_FACT.CCEX_CODI,TNHT_FACT.RESE_CODI,TNHT_FACT.RESE_ANCI,TNHT_FACT.ALOJ_CODI,TNHT_FACT.FAAN_CODI,TNHT_FACT.FAAN_SEFA "

            SQL += "FROM TNHT_FAIV, TNHT_FACT,TNHT_TIVA "
            SQL += "WHERE TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "AND TNHT_FAIV.TIVA_CODI = TNHT_TIVA.TIVA_CODI "
            SQL += "AND (TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "
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


                ' TIBERIO 2010
                ' VALOR DE X ??  VIENE CUENTA DE "LIQUIDO"  DE TASAS DE IMPUESTOS NEHOTEL


                ' 29042014
                ' indicar si la factura es de credito o de contado 

                If CStr(Me.DbLeeHotel.mDbLector("ESTADO")) = "1" Then
                    TipoFactura = "CONTADO"
                ElseIf CStr(Me.DbLeeHotel.mDbLector("ESTADO")) = "2" Then
                    TipoFactura = "CREDITO"
                ElseIf CStr(Me.DbLeeHotel.mDbLector("ESTADO")) = "3" Then
                    TipoFactura = "MIXTA"
                ElseIf CStr(Me.DbLeeHotel.mDbLector("ESTADO")) = "4" Then
                    TipoFactura = "PENDIENTE"
                Else
                    TipoFactura = "INDETERMINADA"
                End If



                If IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = False Then
                    CcExCodi = CStr(Me.DbLeeHotel.mDbLector("CCEX_CODI"))
                Else
                    CcExCodi = Nothing
                End If


                If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_CODI")) = False Then
                    ReseCodi = CInt(Me.DbLeeHotel.mDbLector("RESE_CODI"))
                Else
                    ReseCodi = Nothing
                End If

                If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_ANCI")) = False Then
                    ReseAnci = CInt(Me.DbLeeHotel.mDbLector("RESE_ANCI"))
                Else
                    ReseAnci = Nothing
                End If


                If IsDBNull(Me.DbLeeHotel.mDbLector("ALOJ_CODI")) = False Then
                    AlojCodi = CStr(Me.DbLeeHotel.mDbLector("ALOJ_CODI"))
                Else
                    AlojCodi = ""
                End If


                If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = False Then
                    FacturaAnulada = Me.DbLeeHotel.mDbLector("FAAN_CODI") & "/" & Me.DbLeeHotel.mDbLector("FAAN_SEFA")
                Else
                    FacturaAnulada = ""
                End If


                Me.mTipoAsiento = "HABER"
                Me.InsertaOracleGustavo("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva, "NO", Me.mClientesContadoCif, "", "SI", CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), TipoFactura, TotalBase, CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), CcExCodi, ReseCodi, ReseAnci, AlojCodi, FacturaAnulada)
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva)

                Me.GeneraFileIV("IV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalBase, CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), TotalIva, CType(Me.DbLeeHotel.mDbLector("X"), String))
                Me.mTextDebug.Text = "Detalle Igic Factura " & CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String)
                Me.mTextDebug.Update()


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Detalle de Impuesto")
        End Try
    End Sub
    Private Sub NFacturasSalidaIgicAgrupado()

        Try

            Dim TotalIva As Double
            Dim TotalBase As Double

            Dim DescripcionAsiento As String = "I.G.I.C FACTURACION "

            SQL = "SELECT "
            SQL += "TNHT_FAIV.FAIV_TAXA AS TIPO,SUM((FAIV_INCI-FAIV_VIMP)) BASE, ROUND(SUM(TNHT_FAIV.FAIV_VIMP),2) IGIC,NVL(TIVA_CTB1,'0') CUENTA, '"
            SQL += Me.mParaTextoIva & " ' || FAIV_TAXA ||'%  ' "
            SQL += "FROM TNHT_FAIV, TNHT_FACT,TNHT_TIVA "
            SQL += "WHERE TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "AND TNHT_FAIV.TIVA_CODI = TNHT_TIVA.TIVA_CODI "
            SQL += "AND (TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "
            SQL += "GROUP BY TNHT_FAIV.FAIV_TAXA,TIVA_CTB1"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
                TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)


                TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
                TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

                ' TIBERIO ( EL TOTAL FACTURA ES LA BASE MAS EL IGIC )   O LOS MAS LOS DESEMBOSOS Y SERVICIOS SIN IGIC 
                'TotalFactura = Decimal.Round(CType(TotalBase + TotalIva, Decimal), 2)
                ' TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

                '  DescripcionAsiento += " " & CStr(Me.DbLeeHotel.mDbLector("TIPO")) & "% " & Me.mFecha
                DescripcionAsiento = "I.G.I.C FACTURACION  " & CStr(Me.DbLeeHotel.mDbLector("TIPO")) & "% " & Me.mFecha

                Me.mTipoAsiento = "HABER"
                ' Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, DescripcionAsiento, TotalIva, "NO", Me.mClientesContadoCif, "", "SI")
                'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, DescripcionAsiento, TotalIva)

                '  Me.GeneraFileIV("IV", 3, Me.mEmpGrupoCod, Me.mEmpCod, "SERIE", 0, TotalBase, CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), TotalIva, "X")


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Detalle de Impuesto")
        End Try
    End Sub
    Private Sub NFacturasSalidaBaseImponibleAgrupado()

        Try

            Dim TotalIva As Double
            Dim TotalBase As Double

            Dim DescripcionAsiento As String = ""

            Dim Cuenta As String
            Dim TipoSerie As String = ""

            SQL = "SELECT "
            SQL += "TNHT_FACT.SEFA_CODI AS SERIE,SUM((FAIV_INCI-FAIV_VIMP)) BASE, ROUND(SUM(TNHT_FAIV.FAIV_VIMP),2) IGIC  "
            SQL += "  ,SUM ( (round(FAIV_INCI,2) - round(FAIV_VIMP,2))) BASER "

            SQL += "FROM TNHT_FAIV, TNHT_FACT,TNHT_TIVA "
            SQL += "WHERE TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "AND TNHT_FAIV.TIVA_CODI = TNHT_TIVA.TIVA_CODI "
            SQL += "AND (TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "
            SQL += "GROUP BY TNHT_FACT.SEFA_CODI"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                ' Averigua Cuenta de Serie de Facturas

                SQL = "SELECT SEFA_TIFA FROM TNHT_SEFA WHERE SEFA_CODI = '" & CStr(Me.DbLeeHotel.mDbLector("SERIE")) & "'"
                Cuenta = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

                If IsDBNull(Cuenta) = False Then
                    If Cuenta = "1" Then
                        '  Cuenta = Mid(mCtaSerieCredito, 1, 5) & Me.mCta56DigitoCuentaClientes & Mid(mCtaSerieCredito, 6, 5)
                        Cuenta = mCtaSerieCredito
                        TipoSerie = " Crédito "
                    ElseIf Cuenta = "2" Then
                        '   Cuenta = Mid(mCtaSerieContado, 1, 5) & Me.mCta56DigitoCuentaClientes & Mid(mCtaSerieContado, 6, 5)
                        Cuenta = mCtaSerieContado
                        TipoSerie = " Contado "
                    ElseIf Cuenta = "6" Then
                        '   Cuenta = Mid(mCtaSerieAnulacion, 1, 5) & Me.mCta56DigitoCuentaClientes & Mid(mCtaSerieAnulacion, 6, 5)
                        Cuenta = mCtaSerieAnulacion
                        TipoSerie = " Anulación "

                    End If
                Else
                    Cuenta = "0"
                End If




                Linea = Linea + 1
                TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
                TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)


                TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
                TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)



                '29042014

                ' BASE REDONDEADA PARA TRATAR DE EVITAR AJUSTE DE REDONDEO 


                TotalBase = CType(Me.DbLeeHotel.mDbLector("BASER"), Double)
                TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)




                DescripcionAsiento = "Total Serie " & CStr(Me.DbLeeHotel.mDbLector("SERIE")) & TipoSerie

                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, DescripcionAsiento, TotalBase, "NO", Me.mClientesContadoCif, "", "SI", CStr(Me.DbLeeHotel.mDbLector("SERIE")))
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, DescripcionAsiento, TotalBase)



            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Detalle de Impuesto")
        End Try
    End Sub


#End Region

#Region "ASIENTO-51"
    Private Sub NotasDeCreditoEntidadTotalLiquido()
        Dim Total As Double
        SQL = "SELECT NCRE_DAEM AS FECHA,SUM(TNHT_MOVI.MOVI_VLIQ) AS TOTAL "
        SQL += "FROM " & Me.mStrHayHistorico & " TNHT_MOVI , TNHT_SECC,TNHT_SERV, TNHT_FORE, TNHT_TIRE, TNHT_RECI,TNHT_MCRE,TNHT_NCRE "
        SQL += "WHERE TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI(+) AND "
        SQL += "TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+) AND "
        SQL += "TNHT_MOVI.SECC_CODI = TNHT_SECC.SECC_CODI AND "
        SQL += "TNHT_MOVI.TIRE_CODI = TNHT_TIRE.TIRE_CODI(+) AND "
        SQL += "TNHT_MOVI.MOVI_CODI = TNHT_RECI.MOVI_CODI(+) AND "
        SQL += "TNHT_MOVI.MOVI_DARE = TNHT_RECI.MOVI_DATR(+) AND "
        SQL += "TNHT_MOVI.PAPO_CODI = TNHT_RECI.PAPO_CODI(+) AND "
        SQL += "TNHT_MOVI.MOVI_CODI = TNHT_MCRE.MOVI_CODI AND "
        SQL += "TNHT_MOVI.MOVI_DARE = TNHT_MCRE.MOVI_DARE AND "
        SQL += "TNHT_MCRE.NCRE_CODI = TNHT_NCRE.NCRE_CODI AND "
        SQL += "TNHT_MCRE.SEFA_CODI = TNHT_NCRE.SEFA_CODI AND "
        SQL += "TNHT_MOVI.MOVI_TIMO = 1 AND "
        SQL += "TNHT_MOVI.PAPO_CODI = 1 AND "
        SQL += "TNHT_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
        SQL += "GROUP BY NCRE_DAEM "
        SQL += "ORDER BY NCRE_DAEM ASC "


        Me.DbLeeHotel.TraerLector(SQL)


        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Total = Decimal.Round(CType(Total, Decimal), 2)


            If Total <> 0 Then
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "PENDIENTE DE FACTURAR ", Total, "SI", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, "PENDIENTE DE FACTURAR ", Total)
            End If

        End While
        Me.DbLeeHotel.mDbLector.Close()

        '' ANULADAS
        SQL = "SELECT NCRE_DAEM AS FECHA,SUM(TNHT_MOVI.MOVI_VLIQ) AS TOTAL "
        SQL += "FROM " & Me.mStrHayHistorico & " TNHT_MOVI , TNHT_SECC,TNHT_SERV, TNHT_FORE, TNHT_TIRE, TNHT_RECI,TNHT_MCRE,TNHT_NCRE "
        SQL += "WHERE TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI(+) AND "
        SQL += "TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+) AND "
        SQL += "TNHT_MOVI.SECC_CODI = TNHT_SECC.SECC_CODI AND "
        SQL += "TNHT_MOVI.TIRE_CODI = TNHT_TIRE.TIRE_CODI(+) AND "
        SQL += "TNHT_MOVI.MOVI_CODI = TNHT_RECI.MOVI_CODI(+) AND "
        SQL += "TNHT_MOVI.MOVI_DARE = TNHT_RECI.MOVI_DATR(+) AND "
        SQL += "TNHT_MOVI.PAPO_CODI = TNHT_RECI.PAPO_CODI(+) AND "
        SQL += "TNHT_MOVI.MOVI_CODI = TNHT_MCRE.MOVI_CODI AND "
        SQL += "TNHT_MOVI.MOVI_DARE = TNHT_MCRE.MOVI_DARE AND "
        SQL += "TNHT_MCRE.NCRE_CODI = TNHT_NCRE.NCRE_CODI AND "
        SQL += "TNHT_MCRE.SEFA_CODI = TNHT_NCRE.SEFA_CODI AND "
        SQL += "TNHT_MOVI.MOVI_TIMO = 1 AND "
        SQL += "TNHT_MOVI.PAPO_CODI = 1 AND "
        SQL += "TNHT_NCRE.NCRE_DAAN = " & "'" & Me.mFecha & "' "
        SQL += "GROUP BY NCRE_DAEM "
        SQL += "ORDER BY NCRE_DAEM ASC "


        Me.DbLeeHotel.TraerLector(SQL)


        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Total = Decimal.Round(CType(Total, Decimal), 2)


            If Total <> 0 Then
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorDebe, "PENDIENTE DE FACTURAR ANULADO", Total, "SI", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorDebe, "PENDIENTE DE FACTURAR ANULADO", Total)
            End If

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub

    Private Sub NotasDeCreditoEntidadCredito()


        Dim Total As Double
        Dim TotalPendiente As Double

        Dim Cuenta As String
        Dim TipoNota As String

        Dim FacturaAnulada As String


        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEDO_CODI AS SERIE, TNHT_NCRE.NCRE_DOCU AS NUMERO,TNHT_NCRE.SEFA_CODI AS SERIE2, TNHT_NCRE.NCRE_CODI AS NUMERO2,TNHT_NCRE.NCRE_DOCU||'/'||TNHT_NCRE.SEDO_CODI FACTURA,(NCRE_VALO * -1) TOTAL, "
        SQL += " NCRE_TITU, NCRE_DAEM,NVL(ENTI_NCON_AF,0) CUENTA ,NVL(ENTI_NUCO,0) CIF,NVL(NCRE_TITU,'?') AS NOMBRE,NCRE_ANUL AS ANULADA"
        SQL += " ,TNHT_NCRE.NCRE_NECO AS ESTADO "

        SQL += " ,TNHT_NCRE.FACT_CODI AS FNUMERO "
        SQL += " ,TNHT_NCRE.FACT_SEFA AS FSERIE "

        SQL += " FROM TNHT_NCRE, TNHT_ENTI"
        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+) "
        SQL += " AND TNHT_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " ORDER BY TNHT_NCRE.NCRE_CODI"

        Me.DbLeeHotel.TraerLector(SQL)


        Linea = 0
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            'Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Total = GetTotalNotaCredito(CType(Me.DbLeeHotel.mDbLector("NUMERO2"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE2"), String))
            TotalPendiente = 0
            Total = Decimal.Round(CType(Total, Decimal), 2) * -1
            TotalPendiente = Decimal.Round(CType(TotalPendiente, Decimal), 2)


            ' indicar si la NOTA es de credito o de contado 

            If IsDBNull(Me.DbLeeHotel.mDbLector("ESTADO")) = True Then
                TipoNota = "CONTADO"
            Else
                TipoNota = "CREDITO"
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("FNUMERO")) = False Then
                FacturaAnulada = Me.DbLeeHotel.mDbLector("FNUMERO") & "/" & Me.DbLeeHotel.mDbLector("FSERIE")
            Else
                FacturaAnulada = ""
            End If

            ' compone 5 y 6 digito cuenta de cliente 
            ' Cuenta = Mid(CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), 5, 6)


            If IsDBNull(Me.DbLeeHotel.mDbLector("ESTADO")) = False Then
                Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
            Else
                Cuenta = Me.mCtaClientesContado
            End If




            Me.mTipoAsiento = "DEBE"
            ' Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI", "NOTA DE CREDITO", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
            Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String) & " Factura = " & FacturaAnulada, "SI", TipoNota, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), FacturaAnulada)



            'Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer))
            'Me.GeneraFileFV("FV", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), Total, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String).PadRight(15, CChar(" ")), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), CType(Me.DbLeeHotel.mDbLector("CIF"), String), TotalPendiente)


            Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer))
            Me.GeneraFileFV("FV", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), Total, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String).PadRight(15, CChar(" ")), Cuenta, CType(Me.DbLeeHotel.mDbLector("CIF"), String), Total)




        End While
        Me.DbLeeHotel.mDbLector.Close()


        '' ANULADAS 
        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEDO_CODI AS SERIE, TNHT_NCRE.NCRE_DOCU AS NUMERO,TNHT_NCRE.SEFA_CODI AS SERIE2, TNHT_NCRE.NCRE_CODI AS NUMERO2,TNHT_NCRE.NCRE_DOCU||'/'||TNHT_NCRE.SEDO_CODI FACTURA,(NCRE_VALO * -1) TOTAL, "
        SQL += " NCRE_TITU, NCRE_DAEM,NVL(ENTI_NCON_AF,0) CUENTA ,NVL(ENTI_NUCO,0) CIF,NVL(NCRE_TITU,'?') AS NOMBRE,NCRE_ANUL AS ANULADA"
        SQL += " ,TNHT_NCRE.NCRE_NECO AS ESTADO "

        SQL += " ,TNHT_NCRE.FACT_CODI AS FNUMERO "
        SQL += " ,TNHT_NCRE.FACT_SEFA AS FSERIE "


        SQL += " FROM TNHT_NCRE, TNHT_ENTI"
        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+) "
        SQL += " AND TNHT_NCRE.NCRE_DAAN = " & "'" & Me.mFecha & "' "
        SQL += " ORDER BY TNHT_NCRE.NCRE_CODI"

        Me.DbLeeHotel.TraerLector(SQL)



        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            'Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Total = GetTotalNotaCredito(CType(Me.DbLeeHotel.mDbLector("NUMERO2"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE2"), String))
            TotalPendiente = 0
            Total = Decimal.Round(CType(Total, Decimal), 2) * -1
            TotalPendiente = Decimal.Round(CType(TotalPendiente, Decimal), 2)


            ' indicar si la NOTA es de credito o de contado 

            If IsDBNull(Me.DbLeeHotel.mDbLector("ESTADO")) = True Then
                TipoNota = "CONTADO"
            Else
                TipoNota = "CREDITO"
            End If



            If IsDBNull(Me.DbLeeHotel.mDbLector("FNUMERO")) = False Then
                FacturaAnulada = Me.DbLeeHotel.mDbLector("FNUMERO") & "/" & Me.DbLeeHotel.mDbLector("FSERIE")
            Else
                FacturaAnulada = ""
            End If


            ' compone 5 y 6 digito cuenta de cliente 
            'Cuenta = Mid(CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), 5, 6)


            If IsDBNull(Me.DbLeeHotel.mDbLector("ESTADO")) = False Then
                Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
            Else
                Cuenta = Me.mCtaClientesContado
            End If


            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaberFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String) & " Anulada ", Total, "NO", CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String) & " Factura =  " & FacturaAnulada, "SI", TipoNota, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), FacturaAnulada)

            Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaberFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String) & " Anulada ", Total, mParaSerieAnulacion & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer))
            ' total con signo invertido  SOLO en el fichero de facturas
            Me.GeneraFileFV("FV", 51, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSerieAnulacion & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), Total * -1, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String).PadRight(15, CChar(" ")), Cuenta, CType(Me.DbLeeHotel.mDbLector("CIF"), String), Total * -1)


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
        SQL += " CUENTA ,NVL(ENTI_NCON_AF,0) CUENTACLIENTE ,NVL(ENTI_NUCO,0) CIF ,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X,NCRE_ANUL AS ANULADA FROM TNHT_NCRE,TNHT_ENTI,TNHT_TIVA,VNHT_NIVA"
        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI AND "
        SQL += " TNHT_NCRE.NCRE_CODI = VNHT_NIVA.NCRE_CODI    AND"
        SQL += " TNHT_NCRE.SEFA_CODI = VNHT_NIVA.SEFA_CODI"
        SQL += " AND VNHT_NIVA.TIVA = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " GROUP BY TNHT_NCRE.SEFA_CODI,TNHT_NCRE.NCRE_CODI,NCRE_VALO,TIVA_PERC,TNHT_NCRE.NCRE_DAEM,TIVA_CTB1,ENTI_NCON_AF,"
        SQL += "ENTI_NUCO, TNHT_TIVA.TIVA_CCVL,NCRE_ANUL "
        SQL += "ORDER BY TNHT_NCRE.NCRE_CODI ASC"

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

        '' ANULADAS
        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEFA_CODI AS SERIE , TNHT_NCRE.NCRE_CODI AS NUMERO,(NCRE_VALO * -1)TOTAL,SUM(MOVI_VLIQ) BASE,SUM(VIVA) IGIC, '"
        SQL += Me.mParaTextoIva & " ' || TIVA_PERC ||'%  '|| TNHT_NCRE.NCRE_CODI ||'/'|| TNHT_NCRE.SEFA_CODI DESCRIPCION, TNHT_NCRE.NCRE_DAEM,TIVA_PERC TIPO,NVL(TIVA_CTB1,'0') "
        SQL += " CUENTA ,NVL(ENTI_NCON_AF,0) CUENTACLIENTE ,NVL(ENTI_NUCO,0) CIF ,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X,NCRE_ANUL AS ANULADA FROM TNHT_NCRE,TNHT_ENTI,TNHT_TIVA,VNHT_NIVA"
        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI AND "
        SQL += " TNHT_NCRE.NCRE_CODI = VNHT_NIVA.NCRE_CODI    AND"
        SQL += " TNHT_NCRE.SEFA_CODI = VNHT_NIVA.SEFA_CODI"
        SQL += " AND VNHT_NIVA.TIVA = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_NCRE.NCRE_DAAN = " & "'" & Me.mFecha & "' "
        SQL += " GROUP BY TNHT_NCRE.SEFA_CODI,TNHT_NCRE.NCRE_CODI,NCRE_VALO,TIVA_PERC,TNHT_NCRE.NCRE_DAEM,TIVA_CTB1,ENTI_NCON_AF,"
        SQL += "ENTI_NUCO, TNHT_TIVA.TIVA_CCVL,NCRE_ANUL "
        SQL += "ORDER BY TNHT_NCRE.NCRE_CODI ASC"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
            TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            If TotalIva <> 0 Then
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String) & " Anulada ", TotalIva, "NO", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String) & " Anulada ", TotalIva)
                ' total con signo invertido  SOLO en el fichero de facturas / impuesto
                Me.GeneraFileIV("IV", 51, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSerieAnulacion & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalBase * -1, CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), TotalIva * -1, CType(Me.DbLeeHotel.mDbLector("X"), String))
            End If

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub

    Private Sub NotasDeCreditoEntidadCreditoDetalleIgic2()
        Dim TotalIva As Double
        Dim TotalBase As Double
        Dim Totalfactura As Double

        Dim TipoNota As String = ""
        Dim FacturaAnulada As String = ""


        SQL = "SELECT"
        ' SQL += " TNHT_NCRE.SEDO_CODI AS SERIE , TNHT_NCRE.NCRE_DOCU AS NUMERO,(V.NCRE_VALO * -1)TOTAL,((V.NCRE_VALO - V.NCRE_VIMP) * -1) BASE,(V.NCRE_VIMP * -1) IGIC, '"
        SQL += " TNHT_NCRE.SEDO_CODI AS SERIE , TNHT_NCRE.NCRE_DOCU AS NUMERO,((V.MOVI_VLIQ + V.NCRE_VIMP) * -1) TOTAL,(V.MOVI_VLIQ * -1) BASE,(V.NCRE_VIMP * -1) IGIC, '"

        SQL += Me.mParaTextoIva & " ' || TIVA_PERC ||'%  '|| TNHT_NCRE.NCRE_DOCU ||'/'|| TNHT_NCRE.SEDO_CODI DESCRIPCION, TNHT_NCRE.NCRE_DAEM,TIVA_PERC TIPO,NVL(TIVA_CTB1,'0') "
        SQL += " CUENTA ,NVL(ENTI_NCON_AF,0) CUENTACLIENTE ,NVL(ENTI_NUCO,0) CIF ,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X,NCRE_ANUL AS ANULADA "


        SQL += " ,TNHT_NCRE.NCRE_NECO AS ESTADO "
        SQL += " ,TNHT_NCRE.FACT_CODI AS FNUMERO "
        SQL += " ,TNHT_NCRE.FACT_SEFA AS FSERIE "


        SQL += "FROM TNHT_NCRE,TNHT_ENTI,TNHT_TIVA,QWE_CONT_NCIM V"
        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+) AND "
        SQL += " TNHT_NCRE.NCRE_CODI = V.NCRE_CODI    AND"
        SQL += " TNHT_NCRE.SEFA_CODI = V.SEFA_CODI"
        SQL += " AND V.TIVA = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
        SQL += "ORDER BY TNHT_NCRE.NCRE_CODI ASC"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
            TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)




            If IsDBNull(Me.DbLeeHotel.mDbLector("ESTADO")) = True Then
                TipoNota = "CONTADO"
            Else
                TipoNota = "CREDITO"
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("FNUMERO")) = False Then
                FacturaAnulada = Me.DbLeeHotel.mDbLector("FNUMERO") & "/" & Me.DbLeeHotel.mDbLector("FSERIE")
            Else
                FacturaAnulada = ""
            End If



            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva, "NO", "", "", "SI", CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), TipoNota, FacturaAnulada, "", CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), TotalBase)
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva)
            Me.GeneraFileIV("IV", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalBase, CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), TotalIva, CType(Me.DbLeeHotel.mDbLector("X"), String))
        End While
        Me.DbLeeHotel.mDbLector.Close()

        '' ANULADAS
        SQL = "SELECT"
        'SQL += " TNHT_NCRE.SEDO_CODI AS SERIE , TNHT_NCRE.NCRE_DOCU AS NUMERO,(V.NCRE_VALO * -1)TOTAL,((V.NCRE_VALO - V.NCRE_VIMP) * -1) BASE,(V.NCRE_VIMP * -1) IGIC, '"
        SQL += " TNHT_NCRE.SEDO_CODI AS SERIE , TNHT_NCRE.NCRE_DOCU AS NUMERO,((V.MOVI_VLIQ + V.NCRE_VIMP) * -1) TOTAL,(V.MOVI_VLIQ * -1) BASE,(V.NCRE_VIMP * -1) IGIC, '"
        SQL += Me.mParaTextoIva & " ' || TIVA_PERC ||'%  '|| TNHT_NCRE.NCRE_DOCU ||'/'|| TNHT_NCRE.SEDO_CODI DESCRIPCION, TNHT_NCRE.NCRE_DAEM,TIVA_PERC TIPO,NVL(TIVA_CTB1,'0') "
        SQL += " CUENTA ,NVL(ENTI_NCON_AF,0) CUENTACLIENTE ,NVL(ENTI_NUCO,0) CIF ,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X,NCRE_ANUL AS ANULADA "

        SQL += " ,TNHT_NCRE.NCRE_NECO AS ESTADO "
        SQL += " ,TNHT_NCRE.FACT_CODI AS FNUMERO "
        SQL += " ,TNHT_NCRE.FACT_SEFA AS FSERIE "

        SQL += "FROM TNHT_NCRE,TNHT_ENTI,TNHT_TIVA,QWE_CONT_NCIM V"
        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+) AND "
        SQL += " TNHT_NCRE.NCRE_CODI = V.NCRE_CODI    AND"
        SQL += " TNHT_NCRE.SEFA_CODI = V.SEFA_CODI"
        SQL += " AND V.TIVA = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_NCRE.NCRE_DAAN = " & "'" & Me.mFecha & "' "
        SQL += "ORDER BY TNHT_NCRE.NCRE_CODI ASC"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
            TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            If TotalIva <> 0 Then
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String) & " Anulada ", TotalIva, "NO", "", "", "SI", CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), TipoNota, FacturaAnulada, "", CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), TotalBase)
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String) & " Anulada ", TotalIva)
                ' total con signo invertido  SOLO en el fichero de facturas / impuesto
                Me.GeneraFileIV("IV", 51, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSerieAnulacion & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalBase * -1, CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), TotalIva * -1, CType(Me.DbLeeHotel.mDbLector("X"), String))
            End If

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub NotasDeCreditoEntidadCreditoBaseImponible()
        Dim TotalIva As Double
        Dim TotalBase As Double
        Dim Totalfactura As Double

        Dim Descripcion As String = ""

        Dim Cuenta As String = ""

        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEFA_CODI AS SERIE ,SUM((NCRE_VALO )) TOTAL,SUM(MOVI_VLIQ * -1) BASE,SUM(VIVA) IGIC "
        SQL += " FROM  TNHT_NCRE,TNHT_ENTI,TNHT_TIVA,VNHT_NIVA"

        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI AND "
        SQL += " TNHT_NCRE.NCRE_CODI = VNHT_NIVA.NCRE_CODI    AND"
        SQL += " TNHT_NCRE.SEFA_CODI = VNHT_NIVA.SEFA_CODI"
        SQL += " AND VNHT_NIVA.TIVA = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " GROUP BY TNHT_NCRE.SEFA_CODI"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
            TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            Descripcion = "Total Serie " & CStr(Me.DbLeeHotel.mDbLector("SERIE"))


            'Cuenta = Mid(mCtaSerieNotaCredito, 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(mCtaSerieNotaCredito, 5, 5)
            Cuenta = mCtaSerieNotaCredito



            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, TotalBase, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Descripcion, TotalBase)
        End While
        Me.DbLeeHotel.mDbLector.Close()

        '' ANULADAS
        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEFA_CODI AS SERIE ,SUM((NCRE_VALO * -1)) TOTAL,SUM(MOVI_VLIQ) BASE,SUM(VIVA) IGIC "
        SQL += " FROM  TNHT_NCRE,TNHT_ENTI,TNHT_TIVA,VNHT_NIVA"

        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI AND "
        SQL += " TNHT_NCRE.NCRE_CODI = VNHT_NIVA.NCRE_CODI    AND"
        SQL += " TNHT_NCRE.SEFA_CODI = VNHT_NIVA.SEFA_CODI"
        SQL += " AND VNHT_NIVA.TIVA = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_NCRE.NCRE_DAAN = " & "'" & Me.mFecha & "' "
        SQL += " GROUP BY TNHT_NCRE.SEFA_CODI"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
            TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)
            Descripcion = "Total Serie Anulada " & CStr(Me.DbLeeHotel.mDbLector("SERIE"))

            'Cuenta = Mid(mCtaSerieNotaCredito, 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(mCtaSerieNotaCredito, 5, 5)
            Cuenta = mCtaSerieNotaCredito



            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, TotalBase, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Descripcion, TotalBase)
        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub NotasDeCreditoEntidadCreditoBaseImponible2()
        Dim TotalIva As Double
        Dim TotalBase As Double
        Dim Totalfactura As Double

        Dim Descripcion As String = ""

        Dim Cuenta As String = ""

        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEDO_CODI AS SERIE ,SUM((QWE_CONT_NCIM.NCRE_VALO )) TOTAL,SUM(QWE_CONT_NCIM.MOVI_VLIQ) BASE,SUM(QWE_CONT_NCIM.NCRE_VIMP) IGIC "
        SQL += " FROM  TNHT_NCRE,TNHT_ENTI,TNHT_TIVA,QWE_CONT_NCIM"

        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+) AND "
        SQL += " TNHT_NCRE.NCRE_CODI = QWE_CONT_NCIM.NCRE_CODI    AND"
        SQL += " TNHT_NCRE.SEFA_CODI = QWE_CONT_NCIM.SEFA_CODI"
        SQL += " AND QWE_CONT_NCIM.TIVA = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " GROUP BY TNHT_NCRE.SEDO_CODI"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
            TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2) * -1

            Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            Descripcion = "Total Serie " & CStr(Me.DbLeeHotel.mDbLector("SERIE"))


            'Cuenta = Mid(mCtaSerieNotaCredito, 1, 5) & Me.mCta56DigitoCuentaClientes & Mid(mCtaSerieNotaCredito, 6, 5)
            Cuenta = mCtaSerieNotaCredito



            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Descripcion, TotalBase, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Descripcion, TotalBase)
        End While
        Me.DbLeeHotel.mDbLector.Close()

        '' ANULADAS
        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEDO_CODI AS SERIE ,SUM((QWE_CONT_NCIM.NCRE_VALO * -1)) TOTAL,SUM(QWE_CONT_NCIM.MOVI_VLIQ *-1) BASE,SUM(QWE_CONT_NCIM.NCRE_VIMP) IGIC "
        SQL += " FROM  TNHT_NCRE,TNHT_ENTI,TNHT_TIVA,QWE_CONT_NCIM"

        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+) AND "
        SQL += " TNHT_NCRE.NCRE_CODI = QWE_CONT_NCIM.NCRE_CODI    AND"
        SQL += " TNHT_NCRE.SEFA_CODI = QWE_CONT_NCIM.SEFA_CODI"
        SQL += " AND QWE_CONT_NCIM.TIVA = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_NCRE.NCRE_DAAN = " & "'" & Me.mFecha & "' "
        SQL += " GROUP BY TNHT_NCRE.SEDO_CODI"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
            TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)
            Descripcion = "Total Serie Anulada " & CStr(Me.DbLeeHotel.mDbLector("SERIE"))

            'Cuenta = Mid(mCtaSerieNotaCredito, 1, 5) & Me.mCta56DigitoCuentaClientes & Mid(mCtaSerieNotaCredito, 6, 5)
            Cuenta = mCtaSerieNotaCredito



            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, TotalBase, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Descripcion, TotalBase)
        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub NotasDeCreditoEntidadTotalLiquidoDepartamento(ByVal vTipo As String)
        Dim Total As Double
        Dim vCentroCosto As String


        SQL = "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI AS SERVICIO, 'ABONADO ' || TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CTB1,'0') CUENTA,NVL(TNHT_SERV.SERV_CMBD,'0') CUENTA2,ROUND (SUM (MOVI_VLIQ), 2) TOTAL"

        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI , TNHT_SECC,TNHT_SERV, TNHT_FORE, TNHT_TIRE, TNHT_RECI,TNHT_MCRE,TNHT_NCRE "
        SQL += "WHERE TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI(+) AND "
        SQL += "TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+) AND "
        SQL += "TNHT_MOVI.SECC_CODI = TNHT_SECC.SECC_CODI AND "
        SQL += "TNHT_MOVI.TIRE_CODI = TNHT_TIRE.TIRE_CODI(+) AND "
        SQL += "TNHT_MOVI.MOVI_CODI = TNHT_RECI.MOVI_CODI(+) AND "
        SQL += "TNHT_MOVI.MOVI_DARE = TNHT_RECI.MOVI_DATR(+) AND "
        SQL += "TNHT_MOVI.PAPO_CODI = TNHT_RECI.PAPO_CODI(+) AND "
        SQL += "TNHT_MOVI.MOVI_CODI = TNHT_MCRE.MOVI_CODI AND "
        SQL += "TNHT_MOVI.MOVI_DARE = TNHT_MCRE.MOVI_DARE AND "
        SQL += "TNHT_MCRE.NCRE_CODI = TNHT_NCRE.NCRE_CODI AND "
        SQL += "TNHT_MCRE.SEFA_CODI = TNHT_NCRE.SEFA_CODI AND "
        SQL += "TNHT_MOVI.MOVI_TIMO = 1 AND "
        SQL += "TNHT_MOVI.PAPO_CODI = 1 AND "
        SQL += "TNHT_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "

        SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,TNHT_SERV.SERV_CMBD"



        Me.DbLeeHotel.TraerLector(SQL)


        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Total = Decimal.Round(CType(Total, Decimal), 2)

            SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                If vTipo = "HABER" Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total, "NO", "", vCentroCosto, "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total)
                Else
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, "*" & CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total, "NO", "", vCentroCosto, "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), "*" & CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total)

                End If

                If vCentroCosto <> "0" Then
                    '        Me.GeneraFileAA("AA", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", vCentroCosto, Total)
                End If


            End If

        End While
        Me.DbLeeHotel.mDbLector.Close()

        '' ANULADAS
        SQL = "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI AS SERVICIO, 'DES-ABONADO ' || TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CTB1,'0') CUENTA,NVL(TNHT_SERV.SERV_CMBD,'0') CUENTA2,ROUND (SUM (MOVI_VLIQ * -1), 2) TOTAL"

        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI , TNHT_SECC,TNHT_SERV, TNHT_FORE, TNHT_TIRE, TNHT_RECI,TNHT_MCRE,TNHT_NCRE "
        SQL += "WHERE TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI(+) AND "
        SQL += "TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+) AND "
        SQL += "TNHT_MOVI.SECC_CODI = TNHT_SECC.SECC_CODI AND "
        SQL += "TNHT_MOVI.TIRE_CODI = TNHT_TIRE.TIRE_CODI(+) AND "
        SQL += "TNHT_MOVI.MOVI_CODI = TNHT_RECI.MOVI_CODI(+) AND "
        SQL += "TNHT_MOVI.MOVI_DARE = TNHT_RECI.MOVI_DATR(+) AND "
        SQL += "TNHT_MOVI.PAPO_CODI = TNHT_RECI.PAPO_CODI(+) AND "
        SQL += "TNHT_MOVI.MOVI_CODI = TNHT_MCRE.MOVI_CODI AND "
        SQL += "TNHT_MOVI.MOVI_DARE = TNHT_MCRE.MOVI_DARE AND "
        SQL += "TNHT_MCRE.NCRE_CODI = TNHT_NCRE.NCRE_CODI AND "
        SQL += "TNHT_MCRE.SEFA_CODI = TNHT_NCRE.SEFA_CODI AND "
        SQL += "TNHT_MOVI.MOVI_TIMO = 1 AND "
        SQL += "TNHT_MOVI.PAPO_CODI = 1 AND "
        SQL += "TNHT_NCRE.NCRE_DAAN = " & "'" & Me.mFecha & "' "

        SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,TNHT_SERV.SERV_CMBD"



        Me.DbLeeHotel.TraerLector(SQL)


        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Total = Decimal.Round(CType(Total, Decimal), 2)

            SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                If vTipo = "HABER" Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total, "NO", "", vCentroCosto, "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total)
                Else
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, "*" & CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total, "NO", "", vCentroCosto, "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), "*" & CType(Me.DbLeeHotel.mDbLector("CUENTA2"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total)

                End If

                If vCentroCosto <> "0" Then
                    '        Me.GeneraFileAA("AA", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", vCentroCosto, Total)
                End If


            End If

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub



#End Region

#Region "ASIENTO-35"
    Private Sub FacturasContadoTotal()

        Dim Total As Double

        Dim TotalComisiones As Double
        Dim SQL As String
        Dim Cuenta As String


        Dim CcExCodi As String

        Dim ReseCodi As String
        Dim ReseAnci As String
        Dim AlojCodi As String



        SQL = "SELECT SUM(MOVI_VDEB) AS TOTAL ,TNHT_FACT.FACT_CODI AS NUMERO,TNHT_FACT.SEFA_CODI AS SERIE "
        SQL += " ,TNHT_FACT.CCEX_CODI,TNHT_FACT.RESE_CODI,TNHT_FACT.RESE_ANCI,TNHT_FACT.ALOJ_CODI "
        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI , TNHT_FACT,TNHT_FAMO "
        SQL += " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI AND "
        SQL += "       TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI AND "

        SQL = SQL & "     TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "

        SQL += "AND    TNHT_MOVI.MOVI_TIMO = '2'                 AND "
        SQL += "      (TNHT_MOVI.MOVI_AUTO = '1' OR TNHT_MOVI.MOVI_AUTO = '0' AND TNHT_MOVI.CCEX_CODI = 'TPV') "
        SQL += "AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        SQL += "AND TNHT_FACT.FACT_STAT = " & "'1'"
        SQL += "AND TNHT_FACT.FAAN_CODI IS  NULL "

        ' NUEVO PARA QUE NO TRATE LAS DEVOLUCIONES SI YA SE TRATAN EN UN ASIENTO PROPIO 20090219
        SQL += " AND TNHT_MOVI.MOVI_VDEB > 0  "
        SQL += " GROUP BY TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI"
        SQL += ",TNHT_FACT.CCEX_CODI,TNHT_FACT.RESE_CODI,TNHT_FACT.RESE_ANCI,TNHT_FACT.ALOJ_CODI"

        SQL = SQL & " ORDER BY TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI"


        Me.NEWHOTEL = New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)


        Me.DbLeeHotel.TraerLector(SQL)


        Total = 0
        Linea = 0
        While Me.DbLeeHotel.mDbLector.Read
            Total = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            Cuenta = Me.NEWHOTEL.DevuelveCuentaContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
            '  Cuenta = Mid(Cuenta, 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(Cuenta, 5, 6)


            If IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = False Then
                CcExCodi = CStr(Me.DbLeeHotel.mDbLector("CCEX_CODI"))
            Else
                CcExCodi = Nothing
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_CODI")) = False Then
                ReseCodi = CInt(Me.DbLeeHotel.mDbLector("RESE_CODI"))
            Else
                ReseCodi = Nothing
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_ANCI")) = False Then
                ReseAnci = CInt(Me.DbLeeHotel.mDbLector("RESE_ANCI"))
            Else
                ReseAnci = Nothing
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("ALOJ_CODI")) = False Then
                AlojCodi = CStr(Me.DbLeeHotel.mDbLector("ALOJ_CODI"))
            Else
                AlojCodi = ""
            End If


            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), Total, "SI", "", "", "SI", "", Me.Multidiario, "", CcExCodi, ReseCodi, ReseAnci, AlojCodi)
                Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), Total, Me.Multidiario)
            End If

        End While
        Me.DbLeeHotel.mDbLector.Close()


        SQL = "SELECT SUM((MOVI_VDEB * -1)) AS TOTAL,TNHT_FACT.FACT_CODI AS NUMERO,TNHT_FACT.SEFA_CODI AS SERIE "
        SQL += " ,TNHT_FACT.CCEX_CODI,TNHT_FACT.RESE_CODI,TNHT_FACT.RESE_ANCI,TNHT_FACT.ALOJ_CODI "
        SQL += "  FROM " & Me.mStrHayHistorico & " TNHT_MOVI , TNHT_FACT,TNHT_FAMO "
        SQL += " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI AND "
        SQL += "       TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI AND "

        SQL = SQL & "     TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "

        SQL += "AND    TNHT_MOVI.MOVI_TIMO = '2'                 AND "
        SQL += "      (TNHT_MOVI.MOVI_AUTO = '1' OR TNHT_MOVI.MOVI_AUTO = '0' AND TNHT_MOVI.CCEX_CODI = 'TPV') "
        SQL += "AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        SQL += "AND TNHT_FACT.FACT_STAT = " & "'1'"
        SQL += "AND TNHT_FACT.FAAN_CODI IS NOT  NULL "

        ' NUEVO PARA QUE NO TRATE LAS DEVOLUCIONES SI YA SE TRATAN EN UN ASIENTO PROPIO 20090219
        SQL += " AND TNHT_MOVI.MOVI_VDEB > 0  "
        SQL += " GROUP BY TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI"
        SQL += ",TNHT_FACT.CCEX_CODI,TNHT_FACT.RESE_CODI,TNHT_FACT.RESE_ANCI,TNHT_FACT.ALOJ_CODI"

        SQL = SQL & " ORDER BY TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI"



        Me.DbLeeHotel.TraerLector(SQL)



        While Me.DbLeeHotel.mDbLector.Read


            Total = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)
            Cuenta = Me.NEWHOTEL.DevuelveCuentaContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
            ' Cuenta = Mid(Cuenta, 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(Cuenta, 5, 6)


            If IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = False Then
                CcExCodi = CStr(Me.DbLeeHotel.mDbLector("CCEX_CODI"))
            Else
                CcExCodi = Nothing
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_CODI")) = False Then
                ReseCodi = CInt(Me.DbLeeHotel.mDbLector("RESE_CODI"))
            Else
                ReseCodi = Nothing
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_ANCI")) = False Then
                ReseAnci = CInt(Me.DbLeeHotel.mDbLector("RESE_ANCI"))
            Else
                ReseAnci = Nothing
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("ALOJ_CODI")) = False Then
                AlojCodi = CStr(Me.DbLeeHotel.mDbLector("ALOJ_CODI"))
            Else
                AlojCodi = ""
            End If
            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), Total, "SI", "", "", "SI", "", Me.Multidiario, "", CcExCodi, ReseCodi, ReseAnci, AlojCodi)
                Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), Total, Me.Multidiario)
            End If
        End While
        Me.DbLeeHotel.mDbLector.Close()

        Me.NEWHOTEL.CerrarConexiones()

        ' tinglado de sumar las comisiones al total liquido

        SQL = "SELECT NVL(SUM(TNHT_DESF.DESF_VALO),'0')TOTAL "
        SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE WHERE"
        SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
        SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1' "
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"



        If Me.mParaComisiones = True Then
            TotalComisiones = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            Total = TotalComisiones
        End If
        Total = Decimal.Round(CType(Total, Decimal), 2)


        ' Total = Total + FacturacionContadoServiciosSinIgic()

        'MsgBox(FacturacionContadoServiciosSinIgic)



        If Total <> 0 Then
            Linea = Linea + 1
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaClientesContado, Me.mIndicadorHaber, "COBROS FACTURACION " & Me.mFecha, Total, "SI", "", "", "SI", "", Me.Multidiario, "")
            Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaClientesContado, Me.mIndicadorHaber, "COBROS FACTURACION " & Me.mFecha, Total, Me.Multidiario)
        End If

    End Sub


    Private Sub FacturasContadoTotalVisas()
        Dim Total As Double
        Dim Descripcion As String
        SQL = "SELECT MOVI_VDEB TOTAL,CACR_DESC TARJETA,nvl(CACR_CTBA,'0') CUENTA,"
        SQL += " TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI,NVL(TNHT_FACT.FACT_TITU,' ') AS TITULAR,NVL(FAAN_CODI,'0') AS FAAN_CODI  "
        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_CACR,TNHT_FACT,TNHT_FAMO WHERE"

        SQL = SQL & " TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & "  AND (TNHT_MOVI.MOVI_AUTO = '1' OR TNHT_MOVI.MOVI_AUTO = '0' AND TNHT_MOVI.CCEX_CODI = 'TPV')"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"

        ' NUEVO PARA QUE NO TRATE LAS DEVOLUCIONES SI YA SE TRATAN EN UN ASIENTO PROPIO 20090219
        SQL += " AND TNHT_MOVI.MOVI_VDEB > 0  "

        SQL = SQL & " ORDER BY TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            If CType(Me.DbLeeHotel.mDbLector("FAAN_CODI"), Integer) = 0 Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            End If

            Descripcion = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion, Total, "NO", "", "", "SI", "COBRO", Me.Multidiario, "")
            Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion, Total, Me.Multidiario)
            'Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String), Total, CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String), CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), Integer))


        End While
        Me.DbLeeHotel.mDbLector.Close()

        Exit Sub
        ' *************** VISAS anulado de dias anteriores 
        SQL = ""
        SQL = "SELECT MOVI_VDEB TOTAL,CACR_DESC TARJETA,nvl(CACR_CTBA,'0') CUENTA,"
        SQL += " TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI,NVL(TNHT_FACT.FACT_TITU,' ') AS TITULAR,NVL(FAAN_CODI,'0') AS FAAN_CODI  FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_CACR,TNHT_FACT,TNHT_FAMO WHERE"

        SQL = SQL & " TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"

        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"

        ' 20080620 ( BLOQUEO ABAJO PARA QUE COJA DE DEBITOS DE FACTURAS DE CREDITO 
        'SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1'  "

        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA < FACT_DAEM"

        ' si activo de bajo que deberia no coge los cobros de la liquidacion de contado ( revisar este tema )
        'SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '1' "
        SQL = SQL & " AND TNHT_MOVI.MOVI_ANUL = '1' "

        ' NUEVO XXXXXXXXXXXXXXX

        SQL = SQL & "  AND (TNHT_MOVI.MOVI_AUTO = '1' OR TNHT_MOVI.MOVI_AUTO = '0' AND TNHT_MOVI.CCEX_CODI = 'TPV')"


        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"

        ' NUEVO PARA QUE NO TRATE LAS DEVOLUCIONES SI YA SE TRATAN EN UN ASIENTO PROPIO 20090219
        SQL += " AND TNHT_MOVI.MOVI_VDEB > 0  "

        SQL = SQL & " ORDER BY TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            If CType(Me.DbLeeHotel.mDbLector("FAAN_CODI"), Integer) = 0 Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            End If

            Descripcion = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion & " Deducido", Total, "NO", "", "", "SI", "COBRO", Me.Multidiario, "")
            Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion & " Deducido", Total, Me.Multidiario)
            'Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String), Total, CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String), CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), Integer))


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasContadoTotaLOtrasFormas()
        Dim Total As Double
        Dim SQL As String

        Dim Descripcion As String


        SQL = ""
        SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI,NVL(TNHT_FACT.FACT_TITU,' ') AS TITULAR,NVL(FAAN_CODI,'0') AS FAAN_CODI  FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FORE,TNHT_FACT,TNHT_FAMO WHERE"

        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "


        SQL = SQL & " AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"


        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"


        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"

        ' 20080620 ( BLOQUEO ABAJO PARA QUE COJA DE DEBITOS DE FACTURAS DE CREDITO 
        'SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1'  "


        ' BLOQUEAR Y DESBLOQUEAR ESTA LINEA CON LOS DIAS 18-05   Y 16-07  DEL SALOBRE
        '  SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = FACT_DAEM"


        ' si activo de bajo que deberia no coge los cobros de la liquidacion de contado ( revisar este tema )
        'SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '1' "
        'SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = '0' "


        ' NUEVO XXXXXXXXXXXXXXX

        SQL = SQL & "  AND (TNHT_MOVI.MOVI_AUTO = '1' OR TNHT_MOVI.MOVI_AUTO = '0' AND TNHT_MOVI.CCEX_CODI = 'TPV')"

        SQL = SQL & " AND TNHT_FACT.FACT_DAEM= " & "'" & Me.mFecha & "'"

        ' NUEVO PARA QUE NO TRATE LAS DEVOLUCIONES SI YA SE TRATAN EN UN ASIENTO PROPIO 20090219
        SQL += " AND TNHT_MOVI.MOVI_VDEB > 0  "


        '   SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1,FAAN_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            If CType(Me.DbLeeHotel.mDbLector("FAAN_CODI"), Integer) = 0 Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            End If

            Descripcion = CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)


            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion, Total, "NO", "", "", "SI", "COBRO", Me.Multidiario, "")
            Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion, Total, Me.Multidiario)
            'Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String), Total, CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String), CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), Integer))


        End While
        Me.DbLeeHotel.mDbLector.Close()

        Exit Sub

        ' *************** contado anulado de dias anteriores 
        SQL = ""
        SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI,NVL(TNHT_FACT.FACT_TITU,' ') AS TITULAR,NVL(FAAN_CODI,'0') AS FAAN_CODI  FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FORE,TNHT_FACT,TNHT_FAMO WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "

        SQL = SQL & " AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"



        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"


        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"

        ' 20080620 ( BLOQUEO ABAJO PARA QUE COJA DE DEBITOS DE FACTURAS DE CREDITO 
        'SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1'  "


        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA < FACT_DAEM"


        ' si activo de bajo que deberia no coge los cobros de la liquidacion de contado ( revisar este tema )
        'SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '1' "


        ' NUEVO XXXXXXXXXXXXXXX

        SQL = SQL & "  AND (TNHT_MOVI.MOVI_AUTO = '1' OR TNHT_MOVI.MOVI_AUTO = '0' AND TNHT_MOVI.CCEX_CODI = 'TPV')"


        '
        SQL = SQL & " AND TNHT_MOVI.MOVI_ANUL = '1' "

        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"

        ' NUEVO PARA QUE NO TRATE LAS DEVOLUCIONES SI YA SE TRATAN EN UN ASIENTO PROPIO 20090219
        SQL += " AND TNHT_MOVI.MOVI_VDEB > 0  "


        '      SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1,FAAN_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            If CType(Me.DbLeeHotel.mDbLector("FAAN_CODI"), Integer) = 0 Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            End If

            Descripcion = CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)


            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion & " Deducido", Total, "NO", "", "", "SI", "COBRO", Me.Multidiario, "")
            Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion & " Deducido", Total, Me.Multidiario)
            'Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String), Total, CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String), CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), Integer))


        End While
        Me.DbLeeHotel.mDbLector.Close()


    End Sub

    Private Sub FacturasContadoCancelaciondeAnticipos()
        Dim Total As Double
        '    Dim TotalCancelados As Double

        Dim SQL As String
        Dim Cuenta As String



        SQL = "SELECT 'Anticipo RVA= ' ||TNHT_MOVI.RESE_CODI||'/'||TNHT_MOVI.RESE_ANCI RESERVA,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA, "
        SQL += "TNHT_MOVI.MOVI_VDEB TOTAL,"
        SQL += " TNHT_FACT.FACT_CODI AS NUMERO ,TNHT_FACT.SEFA_CODI SERIE,"
        SQL = SQL & "TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA,TNHT_FACT.FACT_DAEM,TNHT_MOVI.MOVI_DATR,TNHT_FACT.FAAN_CODI,TNHT_FACT.ENTI_CODI,NVL(MOVI_DESC,' ') MOVI_DESC "
        SQL = SQL & " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO"

        SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        ' NUEVO POR AJUSTE DE RENDIMIENTO 
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) AND "
        SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) AND"


        SQL = SQL & " TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '1'"

        ' SQL = SQL & " AND (TNHT_MOVI.TIRE_CODI  = '1' OR TNHT_MOVI.TIRE_CODI = '5') "
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0 "




        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If


        If Me.mTrataDebitoTpvnoFacturado = True Then
            ' EXCLUYE CIERRE DE CONTADO DE TPV
            SQL += "AND TNHT_MOVI.UTIL_CODI <> 'POS'"
            ' EXCLUYE CIERRE DE CONTADO DE GOLF
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"
        End If


        SQL += " ORDER BY TNHT_MOVI.MOVI_DAVA "

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = (CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1)
            End If


            ' Determinar si el anticipo era de cliente contado o de entidad 
            If IsDBNull(Me.DbLeeHotel.mDbLector("ENTI_CODI")) = True Then
                Cuenta = Me.mCtaClientesContado
            Else
                SQL = "SELECT NVL(ENTI_NCON_AF,0) CUENTA FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                Cuenta = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                If Cuenta = "0" Or IsNothing(Cuenta) = True Then
                    Cuenta = "0"
                End If
            End If


            Me.mCancelacionAnticipos = Me.mCancelacionAnticipos + Total
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String) & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI", "ANTICIPO FACTURADO", Me.Multidiario, "")
            Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, Me.Multidiario)

            Linea = Linea + 1
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String) & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI", "", Me.Multidiario, "")
            Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, Me.Multidiario)




        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub

    Private Sub SaldoAnticiposFacturas()
        Try
            '    Dim Total As Double
            Dim Cuenta As String

            Dim Saldo As Double


            SQL = "SELECT FACT_CODI || '/' || SEFA_CODI AS FACTURA ,"
            SQL += " FACT_CODI,SEFA_CODI,FAAN_CODI,ENTI_CODI  FROM TNHT_FACT WHERE  "
            SQL += " TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"

            Me.DbLeeHotel.TraerLector(SQL)


            Dim GetNewHotel As New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)




            While Me.DbLeeHotel.mDbLector.Read

                Saldo = SaldoAnticiposalFacturar(CInt(Me.DbLeeHotel.mDbLector("FACT_CODI")), CStr(Me.DbLeeHotel.mDbLector("SEFA_CODI")))

                Linea = Linea + 1

                If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                    Saldo = Saldo
                Else
                    Saldo = Saldo * -1
                End If



                If Saldo <> 0 Then
                    Me.mCancelacionAnticipos = Me.mCancelacionAnticipos + Saldo
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorDebe, "Saldo de Anticipos al Facturar " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Saldo, "NO", "", "Recibido " & "" & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI", "SALDO ANTICIPO FACTURADO", Me.Multidiario, "")
                    Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorDebe, "Saldo de Anticipos al Facturar " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Saldo, Me.Multidiario)
                    'Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String), Saldo, CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String), CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), Integer))


                    Linea = Linea + 1

                    Cuenta = GetNewHotel.DevuelveCuentaContabledeFactura(CInt(Me.DbLeeHotel.mDbLector("FACT_CODI")), CStr(Me.DbLeeHotel.mDbLector("SEFA_CODI")))
                    ' Cuenta = Mid(Cuenta, 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(Cuenta, 5, 6)

                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Saldo de Anticipos al Facturar " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Saldo, "NO", "", "Recibido " & "" & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI", "", Me.Multidiario, "")
                    Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, "Saldo de Anticipos al Facturar " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Saldo, Me.Multidiario)
                End If




            End While
            Me.DbLeeHotel.mDbLector.Close()
            GetNewHotel.CerrarConexiones()



        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub SaldoAnticiposFacturasPorSeccion()
        Try
            '    Dim Total As Double
            Dim Cuenta As String

            Dim Saldo As Double


            SQL = "SELECT FACT_CODI || '/' || SEFA_CODI AS FACTURA ,"
            SQL += " FACT_CODI,SEFA_CODI,FAAN_CODI,ENTI_CODI  FROM TNHT_FACT WHERE  "
            SQL += " TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"

            Me.DbLeeHotel.TraerLector(SQL)


            Dim GetNewHotel As New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)




            While Me.DbLeeHotel.mDbLector.Read


                'SQL = "SELECT SECC_CODI FROM TNHT_SECC"

                SQL = " SELECT SECC_CODI "
                SQL = SQL & "  FROM TNHT_SECC "
                SQL = SQL & " WHERE SECC_CODI IN ( "
                SQL = SQL & "          SELECT   TNHT_MOVI.SECC_CODI "
                SQL = SQL & "              FROM TNHT_MOVI, TNHT_FACT, TNHT_FAMO "
                SQL = SQL & "             WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI "
                SQL = SQL & "               AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI "
                SQL = SQL & "               AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
                SQL = SQL & "               AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
                SQL = SQL & "               AND TNHT_MOVI.MOVI_TIMO = '2' "
                SQL = SQL & "               AND TNHT_FACT.FACT_CODI =  " & CInt(Me.DbLeeHotel.mDbLector("FACT_CODI"))
                SQL = SQL & "               AND TNHT_FACT.SEFA_CODI = '" & CStr(Me.DbLeeHotel.mDbLector("SEFA_CODI")) & "'"

                SQL = SQL & "          GROUP BY TNHT_MOVI.SECC_CODI) "





                Me.DbLeeHotelAux.TraerLector(SQL)
                While Me.DbLeeHotelAux.mDbLector.Read

                    Saldo = SaldoAnticiposalFacturarSeccion(CInt(Me.DbLeeHotel.mDbLector("FACT_CODI")), CStr(Me.DbLeeHotel.mDbLector("SEFA_CODI")), CStr(Me.DbLeeHotelAux.mDbLector("SECC_CODI")))

                    Linea = Linea + 1

                    If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                        Saldo = Saldo
                    Else
                        Saldo = Saldo * -1
                    End If



                    If Saldo <> 0 Then
                        Me.mCancelacionAnticipos = Me.mCancelacionAnticipos + Saldo

                        If Me.mParaUsaCta4b = True And CType(Me.DbLeeHotelAux.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                            Cuenta = Me.mParaCta4b
                        Else
                            Cuenta = Me.mCtaPagosACuenta
                        End If

                        Me.mTipoAsiento = "DEBE"
                        Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, "Saldo de Anticipos al Facturar " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Saldo, "NO", "", "Recibido " & "" & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI", "SALDO ANTICIPO FACTURADO", Me.Multidiario, CStr(Me.DbLeeHotelAux.mDbLector("SECC_CODI")))
                        Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, "Saldo de Anticipos al Facturar " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Saldo, Me.Multidiario)


                        Linea = Linea + 1

                        Cuenta = GetNewHotel.DevuelveCuentaContabledeFactura(CInt(Me.DbLeeHotel.mDbLector("FACT_CODI")), CStr(Me.DbLeeHotel.mDbLector("SEFA_CODI")))
                        '   Cuenta = Mid(Cuenta, 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(Cuenta, 5, 6)

                        Me.mTipoAsiento = "HABER"
                        Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Saldo de Anticipos al Facturar " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Saldo, "NO", "", "Recibido " & "" & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI", "", Me.Multidiario, CStr(Me.DbLeeHotelAux.mDbLector("SECC_CODI")))
                        Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, "Saldo de Anticipos al Facturar " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Saldo, Me.Multidiario)
                    End If

                End While
                Me.DbLeeHotelAux.mDbLector.Close()


            End While
            Me.DbLeeHotel.mDbLector.Close()
            GetNewHotel.CerrarConexiones()



        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Function SaldoAnticiposalFacturar(ByVal vFactura As Integer, ByVal vSerie As String) As Double
        Try
            Dim Anticipos As Double
            Dim Devoluciones As Double
            Dim CobrosNegativos As Double

            Dim Saldo As Double



            ' ANTICIPOS EN FACTURA
            SQL = "SELECT       "
            SQL += "         SUM(TNHT_MOVI.MOVI_VDEB)  TOTAL, TNHT_FACT.FACT_CODI AS NUMERO, "
            SQL += "         TNHT_FACT.SEFA_CODI SERIE "
            SQL += "    FROM " & Me.mStrHayHistorico & " TNHT_MOVI, TNHT_FACT, TNHT_RESE, TNHT_FAMO "
            SQL += "   WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "     AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "     AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += "     AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += "     AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += "     AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
            SQL += "     AND TNHT_MOVI.MOVI_TIMO = '2' "
            SQL += "     AND TNHT_MOVI.TIRE_CODI = '1' "
            SQL += "     AND TNHT_MOVI.MOVI_VDEB <> 0 "
            SQL += "     AND TNHT_MOVI.MOVI_AUTO = '0' "
            SQL += " AND TNHT_FACT.FACT_CODI = " & vFactura
            SQL += " AND TNHT_FACT.SEFA_CODI = '" & vSerie & "'"
            SQL += " GROUP BY TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI"


            Me.mTextDebug.Text = "Calculando Saldo Anticipos(*) Facturas de Salida (contado)" & vFactura & "/" & vSerie
            Me.mTextDebug.Update()

            Anticipos = CDbl(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL))



            ' DEVOLUCIONES CON RECIBO EN FACTURA TIPO 5
            SQL = "SELECT     "
            SQL += "         SUM(TNHT_MOVI.MOVI_VDEB)  TOTAL, TNHT_FACT.FACT_CODI AS NUMERO, "
            SQL += "         TNHT_FACT.SEFA_CODI SERIE "
            SQL += "    FROM " & Me.mStrHayHistorico & " TNHT_MOVI, TNHT_FACT, TNHT_RESE, TNHT_FAMO "
            SQL += "   WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "     AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "     AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += "     AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += "     AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += "     AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
            SQL += "     AND TNHT_MOVI.MOVI_TIMO = '2' "
            SQL += "     AND TNHT_MOVI.TIRE_CODI = '5' "
            SQL += "     AND TNHT_MOVI.MOVI_VDEB <> 0 "
            SQL += "     AND TNHT_MOVI.MOVI_AUTO = '0' "
            SQL += " AND TNHT_FACT.FACT_CODI = " & vFactura
            SQL += " AND TNHT_FACT.SEFA_CODI = '" & vSerie & "'"
            SQL += " GROUP BY TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI"

            Me.mTextDebug.Text = "Calculando Saldo Devoluciones(*) Facturas de Salida (contado)" & vFactura & "/" & vSerie
            Me.mTextDebug.Update()


            Devoluciones = CDbl(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)) * -1


            ' DEVOLUCUONES AL FACTURAR COBROS NEGATICOS 
            SQL = "SELECT      "
            SQL += "         SUM(TNHT_MOVI.MOVI_VDEB)  TOTAL, TNHT_FACT.FACT_CODI AS NUMERO, "
            SQL += "         TNHT_FACT.SEFA_CODI SERIE "
            SQL += "    FROM " & Me.mStrHayHistorico & " TNHT_MOVI, TNHT_FACT, TNHT_RESE, TNHT_FAMO "
            SQL += "   WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "     AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "     AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += "     AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += "     AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += "     AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
            SQL += "     AND TNHT_MOVI.MOVI_TIMO = '2' "
            SQL += "     AND TNHT_MOVI.TIRE_CODI = '1' "
            SQL += "     AND TNHT_MOVI.MOVI_VDEB < 0 "
            SQL += "     AND TNHT_MOVI.MOVI_AUTO = '1' "
            SQL += " AND TNHT_FACT.FACT_CODI = " & vFactura
            SQL += " AND TNHT_FACT.SEFA_CODI = '" & vSerie & "'"
            SQL += " GROUP BY TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI"

            Me.mTextDebug.Text = "Calculando Saldo Cobros(*) Facturas de Salida (contado)" & vFactura & "/" & vSerie
            Me.mTextDebug.Update()


            CobrosNegativos = CDbl(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)) * -1


            Saldo = Anticipos - (Devoluciones + CobrosNegativos)

            Return Saldo



        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Function
    Private Function SaldoAnticiposalFacturarSeccion(ByVal vFactura As Integer, ByVal vSerie As String, ByVal vSeccion As String) As Double
        Try
            Dim Anticipos As Double
            Dim Devoluciones As Double
            Dim CobrosNegativos As Double

            Dim Saldo As Double



            ' ANTICIPOS EN FACTURA
            SQL = "SELECT       "
            SQL += "         SUM(TNHT_MOVI.MOVI_VDEB)  TOTAL, TNHT_FACT.FACT_CODI AS NUMERO, "
            SQL += "         TNHT_FACT.SEFA_CODI SERIE "
            SQL += "    FROM " & Me.mStrHayHistorico & " TNHT_MOVI, TNHT_FACT, TNHT_RESE, TNHT_FAMO "
            SQL += "   WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "     AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "     AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += "     AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += "     AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += "     AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
            SQL += "     AND TNHT_MOVI.MOVI_TIMO = '2' "
            SQL += "     AND TNHT_MOVI.TIRE_CODI = '1' "
            SQL += "     AND TNHT_MOVI.MOVI_VDEB <> 0 "
            SQL += "     AND TNHT_MOVI.MOVI_AUTO = '0' "
            SQL += "     AND TNHT_MOVI.SECC_CODI = '" & vSeccion & "'"
            SQL += " AND TNHT_FACT.FACT_CODI = " & vFactura
            SQL += " AND TNHT_FACT.SEFA_CODI = '" & vSerie & "'"
            SQL += " GROUP BY TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI"


            Me.mTextDebug.Text = "Calculando Saldo Anticipos(*) Facturas de Salida (contado)" & vFactura & "/" & vSerie & " + " & vSeccion
            Me.mTextDebug.Update()

            Anticipos = CDbl(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL))



            ' DEVOLUCIONES CON RECIBO EN FACTURA TIPO 5
            SQL = "SELECT     "
            SQL += "         SUM(TNHT_MOVI.MOVI_VDEB)  TOTAL, TNHT_FACT.FACT_CODI AS NUMERO, "
            SQL += "         TNHT_FACT.SEFA_CODI SERIE "
            SQL += "    FROM " & Me.mStrHayHistorico & " TNHT_MOVI, TNHT_FACT, TNHT_RESE, TNHT_FAMO "
            SQL += "   WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "     AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "     AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += "     AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += "     AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += "     AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
            SQL += "     AND TNHT_MOVI.MOVI_TIMO = '2' "
            SQL += "     AND TNHT_MOVI.TIRE_CODI = '5' "
            SQL += "     AND TNHT_MOVI.MOVI_VDEB <> 0 "
            SQL += "     AND TNHT_MOVI.MOVI_AUTO = '0' "
            SQL += "     AND TNHT_MOVI.SECC_CODI = '" & vSeccion & "'"
            SQL += " AND TNHT_FACT.FACT_CODI = " & vFactura
            SQL += " AND TNHT_FACT.SEFA_CODI = '" & vSerie & "'"
            SQL += " GROUP BY TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI"

            Me.mTextDebug.Text = "Calculando Saldo Devoluciones(*) Facturas de Salida (contado)" & vFactura & "/" & vSerie & " + " & vSeccion
            Me.mTextDebug.Update()


            Devoluciones = CDbl(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)) * -1


            ' DEVOLUCUONES AL FACTURAR COBROS NEGATICOS 
            SQL = "SELECT      "
            SQL += "         SUM(TNHT_MOVI.MOVI_VDEB)  TOTAL, TNHT_FACT.FACT_CODI AS NUMERO, "
            SQL += "         TNHT_FACT.SEFA_CODI SERIE "
            SQL += "    FROM " & Me.mStrHayHistorico & " TNHT_MOVI, TNHT_FACT, TNHT_RESE, TNHT_FAMO "
            SQL += "   WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "     AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "     AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += "     AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += "     AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += "     AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
            SQL += "     AND TNHT_MOVI.MOVI_TIMO = '2' "
            SQL += "     AND TNHT_MOVI.TIRE_CODI = '1' "
            SQL += "     AND TNHT_MOVI.MOVI_VDEB < 0 "
            SQL += "     AND TNHT_MOVI.MOVI_AUTO = '1' "
            SQL += "     AND TNHT_MOVI.SECC_CODI = '" & vSeccion & "'"
            SQL += " AND TNHT_FACT.FACT_CODI = " & vFactura
            SQL += " AND TNHT_FACT.SEFA_CODI = '" & vSerie & "'"
            SQL += " GROUP BY TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI"

            Me.mTextDebug.Text = "Calculando Saldo Cobros(*) Facturas de Salida (contado)" & vFactura & "/" & vSerie & " + " & vSeccion
            Me.mTextDebug.Update()


            CobrosNegativos = CDbl(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)) * -1


            Saldo = Anticipos - (Devoluciones + CobrosNegativos)

            Return Saldo



        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Function
    Private Sub FacturasContadoTotalVisasComision()
        Dim Total As Double

        Dim TotalComision As Double
        Dim vCentroCosto As String

        SQL = "SELECT SUM(MOVI_VDEB) TOTAL,CACR_DESC TARJETA,nvl(CACR_CTBA,'0') CUENTA,'Parámetro' as  CUENTAGASTO,TNHT_CACR.CACR_COMI,"
        SQL += " NVL(FAAN_CODI,'0') AS FAAN_CODI  FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_CACR,TNHT_FACT,TNHT_FAMO WHERE"

        SQL = SQL & " TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"

        ' 20080620 ( BLOQUEO ABAJO PARA QUE COJA DE DEBITOS DE FACTURAS DE CREDITO 
        'SQL = SQL & " AND TNHT_FACT.FACT_STAT = '1'  "


        SQL = SQL & " AND TNHT_MOVI.MOVI_DAVA = FACT_DAEM"
        ' si activo de bajo que deberia no coge los cobros de la liquidacion de contado ( revisar este tema )
        'SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '1' "
        'SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = '0' "
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " GROUP BY TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CTBA,TNHT_CACR.CACR_COMI,TNHT_FACT.FAAN_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read


            SQL = "SELECT NVL(PARA_CENTRO_COSTO_COMI,'0') FROM TH_PARA "
            SQL += " WHERE  PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            vCentroCosto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)



            If CType(Me.DbLeeHotel.mDbLector("FAAN_CODI"), Integer) = 0 Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            End If

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalComision = (CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * CType(Me.DbLeeHotel.mDbLector("CACR_COMI"), Double)) / 100

            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", "", "SI")
            '     Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision)

            Linea = Linea + 1
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTAGASTO"), String), Me.mIndicadorDebe, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", vCentroCosto, "SI")
            '     Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTAGASTO"), String), Me.mIndicadorDebe, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision)



        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
#End Region
#Region "ASIENTO-21 DEVOLUCIONES "
    Private Sub TotalDevolucionesVisas()
        Try
            Dim Total As Double
            Dim Cuenta As String
            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA,NVL(SECC_CODI,'?') AS SECC_CODI"
            SQL = SQL & " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
            'SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 5"
            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI IN(4,5) "
            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' excluir depositos anticipados 
            'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

            If Me.mUsaTnhtMoviAuto = True Then
                SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
            End If
            '
            SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CTBA,SECC_CODI"


            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read

                If Me.mParaUsaCta4b = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                    Cuenta = Me.mParaCta4b3Visa
                Else
                    Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
                End If

                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 21, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI", "COBRO", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String))
                Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, Me.Multidiario)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
        End Try

    End Sub
    Private Sub TotalDevolucionesVisasDetalle()
        Try
            Dim Total As Double
            Dim Cuenta As String


            Dim ReseCodi As String
            Dim ReseAnci As String
            Dim AlojCodi As String
            Dim CcExCodi As String

            SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA,NVL(SECC_CODI,'?') AS SECC_CODI"

            SQL = SQL & " ,TNHT_MOVI.CCEX_CODI AS CCEX_CODI,NVL(TNHT_CCEX.CCEX_TITU,' ') AS CCEX_TITU "

            SQL = SQL & ",TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI,TNHT_MOVI.ALOJ_CODI "



            SQL = SQL & " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_CACR,TNHT_RESE,TNHT_CCEX WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"

            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"

            'SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 5"
            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI IN(4,5) "
            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' excluir depositos anticipados 
            'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

            If Me.mUsaTnhtMoviAuto = True Then
                SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
            End If
            '



            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read

                If Me.mParaUsaCta4b = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                    Cuenta = Me.mParaCta4b3Visa
                Else
                    Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
                End If

                If IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = False Then
                    CcExCodi = CStr(Me.DbLeeHotel.mDbLector("CCEX_CODI"))
                Else
                    CcExCodi = Nothing
                End If

                If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_CODI")) = False Then
                    ReseCodi = CInt(Me.DbLeeHotel.mDbLector("RESE_CODI"))
                Else
                    ReseCodi = Nothing
                End If

                If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_ANCI")) = False Then
                    ReseAnci = CInt(Me.DbLeeHotel.mDbLector("RESE_ANCI"))
                Else
                    ReseAnci = Nothing
                End If


                If IsDBNull(Me.DbLeeHotel.mDbLector("ALOJ_CODI")) = False Then
                    AlojCodi = CStr(Me.DbLeeHotel.mDbLector("ALOJ_CODI"))
                Else
                    AlojCodi = ""
                End If


                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 21, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI", "COBRO", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), CcExCodi, ReseCodi, ReseAnci, AlojCodi, "", "")
                Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, Me.Multidiario)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
        End Try

    End Sub
    Private Sub TotalDevolucionesOtrasFormas()
        Dim Total As Double
        Dim Cuenta As String
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,NVL(SECC_CODI,'?') AS SECC_CODI FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
        'SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 5"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI IN(4,5) "



        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1,SECC_CODI"
        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            If Me.mParaUsaCta4b = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b2Efectivo
            Else
                Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
            End If


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 21, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI", "COBRO", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String))
            Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, Me.Multidiario)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub TotalDevolucionesOtrasFormasDetalle()
        Dim Total As Double
        Dim Cuenta As String


        Dim ReseCodi As String
        Dim ReseAnci As String
        Dim AlojCodi As String
        Dim CcExCodi As String

        SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,NVL(SECC_CODI,'?') AS SECC_CODI"

        SQL = SQL & " ,TNHT_MOVI.CCEX_CODI AS CCEX_CODI,NVL(TNHT_CCEX.CCEX_TITU,' ') AS CCEX_TITU "

        SQL = SQL & ",TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI,TNHT_MOVI.ALOJ_CODI "


        SQL = SQL & " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FORE,TNHT_RESE,TNHT_CCEX  WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "

        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"

        'SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 5"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI IN(4,5) "



        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If


        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            If Me.mParaUsaCta4b = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b2Efectivo
            Else
                Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = False Then
                CcExCodi = CStr(Me.DbLeeHotel.mDbLector("CCEX_CODI"))
            Else
                CcExCodi = Nothing
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_CODI")) = False Then
                ReseCodi = CInt(Me.DbLeeHotel.mDbLector("RESE_CODI"))
            Else
                ReseCodi = Nothing
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_ANCI")) = False Then
                ReseAnci = CInt(Me.DbLeeHotel.mDbLector("RESE_ANCI"))
            Else
                ReseAnci = Nothing
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("ALOJ_CODI")) = False Then
                AlojCodi = CStr(Me.DbLeeHotel.mDbLector("ALOJ_CODI"))
            Else
                AlojCodi = ""
            End If


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 21, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI", "COBRO", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), CcExCodi, ReseCodi, ReseAnci, AlojCodi, "", "")
            Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, Me.Multidiario)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetalleDevolucionesVisas()
        Dim Total As Double
        Dim Cuenta As String = ""


        Dim ReseCodi As String
        Dim ReseAnci As String
        Dim AlojCodi As String
        Dim CcExCodi As String


        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
        SQL = SQL & " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA,NVL(MOVI_DESC,' ') MOVI_DESC,NVL(SECC_CODI,'?') AS SECC_CODI "


        SQL = SQL & " ,TNHT_MOVI.CCEX_CODI AS CCEX_CODI,NVL(TNHT_CCEX.CCEX_TITU,' ') AS CCEX_TITU "

        SQL = SQL & ",TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI,TNHT_MOVI.ALOJ_CODI "


        SQL = SQL & "FROM " & Me.mStrHayHistorico & " TNHT_MOVI,"
        SQL = SQL & " TNHT_CACR,TNHT_RESE,TNHT_CCEX WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
        'SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 5"

        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"

        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI IN(4,5) "

        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            If Me.mParaUsaCta4b = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b
            Else
                Cuenta = Me.mCtaPagosACuenta
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = False Then
                CcExCodi = CStr(Me.DbLeeHotel.mDbLector("CCEX_CODI"))
            Else
                CcExCodi = Nothing
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_CODI")) = False Then
                ReseCodi = CInt(Me.DbLeeHotel.mDbLector("RESE_CODI"))
            Else
                ReseCodi = Nothing
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_ANCI")) = False Then
                ReseAnci = CInt(Me.DbLeeHotel.mDbLector("RESE_ANCI"))
            Else
                ReseAnci = Nothing
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("ALOJ_CODI")) = False Then
                AlojCodi = CStr(Me.DbLeeHotel.mDbLector("ALOJ_CODI"))
            Else
                AlojCodi = ""
            End If


            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 21, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Devolución " & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), CcExCodi, ReseCodi, ReseAnci, AlojCodi, "", "")
            Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, "Devolución " & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, Me.Multidiario)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetalleDevolucionesOtrasFormas()
        Dim Total As Double
        Dim Cuenta As String = ""


        Dim ReseCodi As String
        Dim ReseAnci As String
        Dim AlojCodi As String
        Dim CcExCodi As String


        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE,TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_DAVA,NVL(MOVI_DESC,' ') MOVI_DESC,NVL(SECC_CODI,'?') AS SECC_CODI "

        SQL = SQL & " ,TNHT_MOVI.CCEX_CODI AS CCEX_CODI,NVL(TNHT_CCEX.CCEX_TITU,' ') AS CCEX_TITU "

        SQL = SQL & ",TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI,TNHT_MOVI.ALOJ_CODI "


        SQL = SQL & "FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FORE,TNHT_RESE,TNHT_CCEX WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "

        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"

        'SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 5"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI IN(4,5) "


        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            If Me.mParaUsaCta4b = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b
            Else
                Cuenta = Me.mCtaPagosACuenta
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = False Then
                CcExCodi = CStr(Me.DbLeeHotel.mDbLector("CCEX_CODI"))
            Else
                CcExCodi = Nothing
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_CODI")) = False Then
                ReseCodi = CInt(Me.DbLeeHotel.mDbLector("RESE_CODI"))
            Else
                ReseCodi = Nothing
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RESE_ANCI")) = False Then
                ReseAnci = CInt(Me.DbLeeHotel.mDbLector("RESE_ANCI"))
            Else
                ReseAnci = Nothing
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("ALOJ_CODI")) = False Then
                AlojCodi = CStr(Me.DbLeeHotel.mDbLector("ALOJ_CODI"))
            Else
                AlojCodi = ""
            End If

            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 21, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Devolución " & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), CcExCodi, ReseCodi, ReseAnci, AlojCodi, "", "")
            Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, "Devolución " & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, Me.Multidiario)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
#End Region
#Region "ASIENTO-22 DEVOLUCIONES EN FACTURA "
    Private Sub TotalDevolucionesVisasFacturado()
        Try
            Dim Total As Double
            Dim Factura As String
            Dim Cuenta As String
            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA,MOVI_CORR,MOVI_ANUL,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI,TNHT_FACT.FAAN_CODI,NVL(SECC_CODI,'?') AS SECC_CODI "
            SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_CACR,TNHT_RESE,TNHT_FAMO,TNHT_FACT WHERE "

            SQL += "     TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
            SQL += " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
            SQL += " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            SQL += " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI AND "

            SQL += " TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL += " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
            SQL += " AND (TNHT_MOVI.TIRE_CODI = 1 AND TNHT_MOVI.MOVI_AUTO = 1  AND TNHT_MOVI.MOVI_VDEB < 0 ) "
            '   SQL += " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL += " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"






            SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CTBA,MOVI_CORR,MOVI_ANUL,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI,TNHT_FACT.FAAN_CODI,SECC_CODI"


            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read

                If Me.mParaUsaCta4b = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                    Cuenta = Me.mParaCta4b3Visa
                Else
                    Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
                End If


                Linea = Linea + 1

                If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                    Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Else
                    Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
                End If


                Factura = CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String)

                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & Factura, Total, "NO", "", "", "SI", "COBRO", Me.Multidiario, "")
                Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & Factura, Total, Me.Multidiario)
                'Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String), Total, CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String), CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), Integer))


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
        End Try

    End Sub
    Private Sub TotalDevolucionesOtrasFormasFacturado()
        Dim Total As Double
        Dim Factura As String
        Dim Cuenta As String
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_CORR,MOVI_ANUL,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI,TNHT_FACT.FAAN_CODI,NVL(SECC_CODI,'?') AS SECC_CODI FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FORE,TNHT_RESE,TNHT_FAMO,TNHT_FACT WHERE"
        SQL += "     TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL += " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL += " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL += " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI AND "


        SQL += " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL += " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL += " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
        SQL += " AND (TNHT_MOVI.TIRE_CODI = 1 AND TNHT_MOVI.MOVI_AUTO = 1  AND TNHT_MOVI.MOVI_VDEB < 0 ) "

        SQL += " AND TNHT_MOVI.CACR_CODI IS NULL"
        '   SQL += " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL += " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"



        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1,MOVI_CORR,MOVI_ANUL,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI,TNHT_FACT.FAAN_CODI,SECC_CODI"
        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read

            If Me.mParaUsaCta4b = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b2Efectivo
            Else
                Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
            End If

            Linea = Linea + 1

            If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            End If

            Factura = CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String)

            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " " & Factura, Total, "NO", "", "", "SI", "COBRO", Me.Multidiario, "")
            Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " " & Factura, Total, Me.Multidiario)
            'Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String), Total, CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String), CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), Integer))


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetalleDevolucionesVisasFacturado()

        Dim Total As Double
        Dim Cuenta As String = ""
        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(TNHT_RESE.RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
        SQL += " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA,MOVI_CORR,MOVI_ANUL,TNHT_FACT.FACT_CODI AS NUMERO ,TNHT_FACT.SEFA_CODI AS SERIE,TNHT_FACT.FAAN_CODI,NVL(SECC_CODI,'?') AS SECC_CODI FROM " & Me.mStrHayHistorico & " TNHT_MOVI,"
        SQL += " TNHT_CACR,TNHT_RESE,TNHT_FAMO,TNHT_FACT WHERE "

        SQL += "     TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL += " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL += " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL += " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI AND "

        SQL += " TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL += " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL += " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
        SQL += " AND (TNHT_MOVI.TIRE_CODI = 1 AND TNHT_MOVI.MOVI_AUTO = 1  AND TNHT_MOVI.MOVI_VDEB < 0 ) "
        '   SQL += " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL += " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"




        Me.NEWHOTEL = New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)


        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1

            If Me.mParaUsaCta4b = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b
            Else
                Cuenta = Me.mCtaPagosACuenta
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            End If

            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Devol. Pag A Cuenta RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "", Me.Multidiario, "")
            Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, "Devol. Pag A Cuenta RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total, Me.Multidiario)


            ' CUADRE P A CUENTA 
            '    Linea = Linea + 1
            '    Me.mTipoAsiento = "DEBE"
            '    Me.InsertaOracle("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorDebe, "(*)Devol. Pag A Cuenta RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            '    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorDebe, "(*)Devol. Pag A Cuenta RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)


            ' CUADRECLIENTE
            '   Cuenta = Me.NEWHOTEL.DevuelveCuentaContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))

            '   Linea = Linea + 1
            '   Me.mTipoAsiento = "HABER"
            '   Me.InsertaOracle("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "(*)Devol. Pag A Cuenta RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            '   Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, "(*)Devol. Pag A Cuenta RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)



        End While
        Me.DbLeeHotel.mDbLector.Close()
        Me.NEWHOTEL.CerrarConexiones()
    End Sub
    Private Sub DetalleDevolucionesOtrasFormasFacturado()
        Dim Total As Double
        Dim Cuenta As String = ""

        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(TNHT_RESE.RESE_ANPH,'?') CLIENTE,TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_DAVA,MOVI_CORR,MOVI_ANUL,TNHT_FACT.FACT_CODI AS NUMERO ,TNHT_FACT.SEFA_CODI AS SERIE,TNHT_FACT.FAAN_CODI,NVL(SECC_CODI,'?') AS SECC_CODI FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FORE,TNHT_RESE,TNHT_FAMO,TNHT_FACT WHERE"
        SQL += "     TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL += " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL += " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL += " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI AND "

        SQL += " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL += " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL += " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
        SQL += " AND (TNHT_MOVI.TIRE_CODI = 1 AND TNHT_MOVI.MOVI_AUTO = 1  AND TNHT_MOVI.MOVI_VDEB < 0 ) "

        SQL += " AND TNHT_MOVI.CACR_CODI IS NULL"
        '   SQL += " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL += " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"




        Me.NEWHOTEL = New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)



        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1

            If Me.mParaUsaCta4b = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b
            Else
                Cuenta = Me.mCtaPagosACuenta
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            End If

            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Devol. Pag A Cuenta RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "", Me.Multidiario, "")
            Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, "Devol. Pag A Cuenta RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total, Me.Multidiario)



            ' CUADRE P A CUENTA 
            '   Linea = Linea + 1
            '   Me.mTipoAsiento = "DEBE"
            '   Me.InsertaOracle("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorDebe, "(*)Devol. Pag A Cuenta RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            '   Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorDebe, "(*)Devol. Pag A Cuenta RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

            ' CUADRECLIENTE

            ' CUADRECLIENTE
            '  Cuenta = Me.NEWHOTEL.DevuelveCuentaContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))

            'Linea = Linea + 1
            'Me.mTipoAsiento = "HABER"
            'Me.InsertaOracle("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "(*)Devol. Pag A Cuenta RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, "(*)Devol. Pag A Cuenta RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)



        End While
        Me.DbLeeHotel.mDbLector.Close()
        Me.NEWHOTEL.CerrarConexiones()
    End Sub
#End Region

#Region "ASIENTO-60 OPERECIONES DE CAJA"
    Private Sub OperacionesDeCaja()
        Try
            Dim Total As Double
            Dim Cuenta As String = ""


            ' ENTRADAS DE CAJA 
            SQL = "SELECT MOCA_VCRE AS TOTAL,NVL(MOCA_OBSE,'?') AS MOCA_OBSE FROM TNHT_MOCA "
            SQL += " WHERE  TNHT_MOCA.MOCA_DATR = " & "'" & Me.mFecha & "'"
            SQL += " AND MOCA_OBSE <> 'Fondo Inicial de Caja EUR'"
            SQL += " AND MOCA_VCRE  IS NOT NULL"
            SQL += " AND MOVI_CODI IS NULL"


            Me.DbLeeHotel.TraerLector(SQL)
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1


                Cuenta = Me.mCuentaOpeCaja
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 60, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, "Entradas de Caja " & CType(Me.DbLeeHotel.mDbLector("MOCA_OBSE"), String), Total, "NO", "", "", "SI", "", Me.Multidiario, "")
                Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, "Entradas de Caja " & CType(Me.DbLeeHotel.mDbLector("MOCA_OBSE"), String), Total, Me.Multidiario)

            End While
            Me.DbLeeHotel.mDbLector.Close()



            ' SALIDAS DE CAJA 
            SQL = "SELECT MOCA_VDEB AS TOTAL,NVL(MOCA_OBSE,'?') AS MOCA_OBSE FROM TNHT_MOCA "
            SQL += " WHERE  TNHT_MOCA.MOCA_DATR = " & "'" & Me.mFecha & "'"
            SQL += " AND MOCA_OBSE <> 'Fondo Inicial de Caja EUR'"
            SQL += " AND MOCA_VDEB  IS NOT NULL"
            SQL += " AND MOVI_CODI IS NULL"


            Me.DbLeeHotel.TraerLector(SQL)
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1

                Cuenta = Me.mCuentaOpeCaja
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)


                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 61, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Salidas de Caja " & CType(Me.DbLeeHotel.mDbLector("MOCA_OBSE"), String), Total, "NO", "", "", "SI", "", Me.Multidiario, "")
                Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, "Salidas de Caja " & CType(Me.DbLeeHotel.mDbLector("MOCA_OBSE"), String), Total, Me.Multidiario)

            End While
            Me.DbLeeHotel.mDbLector.Close()

        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "RUTINAS PRIVADAS"

    Private Function FacturacionSalidaDesembolsos() As Double
        Dim Resultado As String
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB) TOTAL"
        SQL = SQL & " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FACT"
        SQL = SQL & " WHERE TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & " '" & Me.mFecha & "'"
        SQL = SQL & " AND TIRE_CODI = '4'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        'DEBAJO PARA CONTROL DE ERROR RARO DE DESEMBOLSOS POSITIVOS NO SE POR QUE LOS HAY
        'SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB < 0"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT <> '2' AND TNHT_FACT.FACT_CLEN = '1' AND TNHT_FACT.FACT_ANUL = '0'"


        Resultado = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
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
        SQL = SQL & " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FACT,TNHT_SERV,TNHT_TIVA"
        SQL = SQL & " WHERE TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI AND"
        SQL = SQL & " TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI AND"
        SQL = SQL & " TNHT_SERV.TIVA_CODI = TNHT_TIVA.TIVA_CODI AND "
        SQL = SQL & " TNHT_TIVA.TIVA_PERC = 0"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & " '" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT <> '2' AND TNHT_FACT.FACT_CLEN = '1' AND TNHT_FACT.FACT_ANUL = '0'"



        Resultado = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
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
        SQL = SQL & " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FACT,TNHT_ENTI"
        SQL = SQL & " WHERE TNHT_FACT.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL = SQL & " AND  TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & " '" & Me.mFecha & "'"
        SQL = SQL & " AND TIRE_CODI = '4'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        SQL += "AND TNHT_FACT.FACT_STAT = '2' "
        SQL += "AND (TNHT_FACT.FACT_ANUL = '0' OR FACT_DAEM < FACT_DAAN) "


        Resultado = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
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
        SQL = SQL & " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FACT,TNHT_SERV,TNHT_TIVA,TNHT_ENTI"
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



        Resultado = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
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
        SQL = SQL & " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FACT,TNHT_CCEX"
        SQL = SQL & " WHERE TNHT_FACT.TACO_CODI = TNHT_CCEX.CCEX_CODI "
        SQL = SQL & " AND  TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & " '" & Me.mFecha & "'"
        SQL = SQL & " AND TIRE_CODI = '4'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        'DEBAJO PARA CONTROL DE ERROR RARO DE DESEMBOLSOS POSITIVOS NO SE POR QUE LOS HAY
        'SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB < 0"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT = '2' AND TNHT_FACT.FACT_CLEN = '1' AND TNHT_FACT.FACT_ANUL = '0'"


        Resultado = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
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
        SQL = SQL & " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FACT,TNHT_SERV,TNHT_TIVA,TNHT_CCEX"
        SQL = SQL & " WHERE TNHT_FACT.TACO_CODI = TNHT_CCEX.CCEX_CODI "
        SQL = SQL & " AND  TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI AND"
        SQL = SQL & " TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI AND"
        SQL = SQL & " TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI AND"
        SQL = SQL & " TNHT_SERV.TIVA_CODI = TNHT_TIVA.TIVA_CODI AND "
        SQL = SQL & " TNHT_TIVA.TIVA_PERC = 0"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & " '" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_CORR = 0"
        SQL = SQL & " AND TNHT_FACT.FACT_STAT = '2' AND TNHT_FACT.FACT_CLEN = '1' AND TNHT_FACT.FACT_ANUL = '0'"




        Resultado = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
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
        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FACT,TNHT_CCEX"
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
        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FACT,TNHT_SERV,TNHT_TIVA,TNHT_CCEX"
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
    Private Function GetValorDescotandoFinanciero(ByVal vFactura As Integer, ByVal vSerie As String) As Double
        Try

            Dim SumaDescuentos As Double
            Dim Resultado As Double



            SQL = "SELECT  SUM(TIDE_PORC)  "
            SQL += "             FROM TNHT_DESF,TNHT_TIDE  "
            SQL += "             WHERE "
            SQL += "                  TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI  "
            SQL += "           AND TNHT_DESF.FACT_CODI = " & vFactura
            SQL += "           AND TNHT_DESF.SEFA_CODI = '" & vSerie & "'"
            SQL += "                GROUP BY TNHT_DESF.FACT_CODI,TNHT_DESF.SEFA_CODI  "


            SumaDescuentos = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            If SumaDescuentos = 0 Then
                Return 0
            End If



            SQL = "SELECT  ( SUM(MOVI_VLIQ) * " & SumaDescuentos & ") / 100 AS TOTAL ,TIDE_PORC  "
            SQL += "             FROM TNHT_FAMO, TNHT_MOVI,TNHT_FACT,TNHT_SERV,TNHT_DESF,TNHT_TIDE  "
            SQL += "             WHERE TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI  "
            SQL += "               AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE  "
            SQL += "             AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI  "
            SQL += "               AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI  "
            SQL += "                AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI  "
            SQL += "               "
            SQL += "                AND TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI  "
            SQL += "                AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI  "
            SQL += "                AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI  "
            SQL += "                 "
            SQL += "           AND TNHT_FACT.FACT_CODI = " & vFactura
            SQL += "           AND TNHT_FACT.SEFA_CODI = '" & vSerie & "'"
            SQL += "                AND MOVI_TIMO = 1  "

            SQL += "                   "
            SQL += "                GROUP BY TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI,TIDE_PORC  "


            Resultado = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            If IsNothing(Resultado) = False Then
                If IsNumeric(Resultado) Then
                    Return Math.Round(Resultado, 2)
                Else
                    Return 0
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "GetValorDescotandoFinanciero")
            Return 0
        End Try
    End Function

#End Region
#End Region
#Region "METODOS PUBLICOS"
    Public Sub Procesar()
        Try

            ' MsgBox("Ojo revisar  COMISION de visas , en depositos antincipados de agencias si los hubiera ", MsgBoxStyle.Exclamation, "Atención")
            If Me.FileEstaOk = False Then Exit Sub

            ' verifica si hay historico de movimientos 
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then

                Me.mTextDebug.Text = "Verificando Histórico " & Me.mFecha
                Me.mTextDebug.Update()

                Dim Control As String
                SQL = "SELECT MAX(MOVI_DATR) FROM TNHT_MOVH "
                SQL += " WHERE  TNHT_MOVH.MOVI_DATR = " & "'" & Me.mFecha & "'"

                Control = Me.DbLeeHotel.EjecutaSqlScalar(SQL)

                If Control = "" Then
                    Me.mHayHistorico = False
                    Me.mStrHayHistorico = " "
                Else
                    Me.mHayHistorico = True
                    Me.mStrHayHistorico = " VNHT_MOVH "
                End If

            End If


            System.Windows.Forms.Application.DoEvents()
            Me.mForm.Update()


            ' ---------------------------------------------------------------
            ' Asiento de Ventas 1
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                'Me.PendienteFacturarTotal()
                '  Me.PendienteFacturarTotalRound()
                Me.VentasDepartamentoDunasDobleManoCorrienteAgrupado()
                Me.mTextDebug.Text = "Calculando Pdte. Facturar"
                Me.mTextDebug.Update()

                'Me.VentasDepartamento()


                Me.VentasDepartamentoDunasDobleManoCorriente()

                Me.mTextDebug.Text = "Calculando Ventas por Departamento"
                Me.mTextDebug.Update()


                Me.mProgress.Value = 10
                Me.mProgress.Update()
            End If




            ' ---------------------------------------------------------------
            ' Asiento de Pagos a Cuenta 2
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                If Me.mTrataCaja Then
                    If Me.mPerfilCobtable = "CAJA" Or Me.mPerfilCobtable = "AMBOS" Then
                        Me.TotalPagosaCuentaVisas()
                        Me.mTextDebug.Text = "Pagos a Cuenta Visas"
                        Me.mTextDebug.Update() '
                        Me.TotalPagosaCuentaOtrasFormas()
                        Me.mTextDebug.Text = "Pagos a Cuenta Otras Formas de Pago"
                        Me.mTextDebug.Update() '

                        Me.DetallePagosaCuentaVisas()
                        Me.mTextDebug.Text = "Detalle de Pagos a Cuenta Visas"
                        Me.mTextDebug.Update()

                        Me.DetallePagosaCuentaOtrasFormas()
                        Me.mTextDebug.Text = "Detalle de Pagos a Cuenta Otras Formas"
                        Me.mTextDebug.Update()

                        '   Me.TotalPagosaCuentaVisasComision()
                        Me.mTextDebug.Text = "COMISION Visas  de Pagos a Cuenta "
                        Me.mTextDebug.Update()


                        Me.mProgress.Value = 20
                        Me.mProgress.Update()
                    End If
                End If
            End If

            System.Windows.Forms.Application.DoEvents()
            Me.mForm.Update()


            ' ---------------------------------------------------------------
            ' Asiento de DEVOLUCIONES  21
            '----------------------------------------------------------------

            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then

                If Me.mPerfilCobtable = "CAJA" Or Me.mPerfilCobtable = "AMBOS" Then
                    If Me.mTrataCaja Then
                        ' Me.TotalDevolucionesVisas()
                        Me.TotalDevolucionesVisasDetalle()
                        Me.mTextDebug.Text = "Devoluciones Visas"
                        Me.mTextDebug.Update()

                        'Me.TotalDevolucionesOtrasFormas()
                        Me.TotalDevolucionesOtrasFormasDetalle()
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


            End If

            System.Windows.Forms.Application.DoEvents()
            Me.mForm.Update()


            ' ---------------------------------------------------------------
            ' Asiento de DEVOLUCIONES HECHAS POR NEWHOTEL AUTOMATICAS AL FACTURAR  22 
            '----------------------------------------------------------------

            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                If Me.mTrataCaja Then
                    If Me.mPerfilCobtable = "CAJA" Or Me.mPerfilCobtable = "AMBOS" Then
                        Me.TotalDevolucionesVisasFacturado()
                        Me.mTextDebug.Text = "Devoluciones Visas en Factura"
                        Me.mTextDebug.Update()

                        Me.TotalDevolucionesOtrasFormasFacturado()
                        Me.mTextDebug.Text = "Devoluciones Otras Formas de Pago en Factura"
                        Me.mTextDebug.Update()

                        Me.DetalleDevolucionesVisasFacturado()
                        Me.mTextDebug.Text = "Detalle de Devoluciones Visas en Factura"
                        Me.mTextDebug.Update()

                        Me.DetalleDevolucionesOtrasFormasFacturado()
                        Me.mTextDebug.Text = "Detalle de Devoluciones Otras Formas en Factura"
                        Me.mTextDebug.Update()

                        Me.mProgress.Value = 28
                        Me.mProgress.Update()
                    End If
                End If

            End If

            System.Windows.Forms.Application.DoEvents()
            Me.mForm.Update()

            ' ---------------------------------------------------------------
            ' Asiento Facturacion total del dia 3
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then

                If Me.mPerfilCobtable = "FACTURAS" Or Me.mPerfilCobtable = "AMBOS" Then
                    Me.NFacturasSalidaTotalFActuraNuevo()
                    Me.mTextDebug.Text = "Calculando Total Pendiente de Facturar"
                    Me.mTextDebug.Update()


                    Me.mProgress.Value = 10
                    Me.mProgress.Update()


                    Me.mTextDebug.Text = "Calculando Descuentos Financieros y Comisiones Facturas"
                    Me.mTextDebug.Update()
                    '2A
                    '   Me.NFacturasSalidaTotaLDescuentos()
                    Me.mProgress.Value = 20
                    Me.mProgress.Update()



                    Me.mTextDebug.Text = "Detalle de Impuesto Facturas "
                    Me.mTextDebug.Update()
                    '3
                    Me.NFacturasSalidaIgicAgrupado()
                    Me.NFacturasSalidaDetalleIgic()



                    Me.NFacturasSalidaBaseImponibleAgrupado()

                    Me.mProgress.Value = 30
                    Me.mProgress.Update()

                End If
            Else
                MsgBox("No dispone de Conexión a la Base de Datos", MsgBoxStyle.Information, "Atención")

            End If

            System.Windows.Forms.Application.DoEvents()
            Me.mForm.Update()


            ' ---------------------------------------------------------------
            ' Asiento Facturacion Contado del dia   CAJA      35 
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                If Me.mPerfilCobtable = "CAJA" Or Me.mPerfilCobtable = "AMBOS" Then

                    Me.mTextDebug.Text = "Calculando Total Líquido Facturas de Salida (contado)"
                    Me.mTextDebug.Update()
                    Me.FacturasContadoTotal()


                    Me.mTextDebug.Text = "Calculando Total Visas Facturas de Salida (contado)"
                    Me.mTextDebug.Update()
                    Me.FacturasContadoTotalVisas()

                    Me.mTextDebug.Text = "Calculando Total Otras Formas Facturas de Salida (contado)"
                    Me.mTextDebug.Update()

                    Me.FacturasContadoTotaLOtrasFormas()




                    'Me.FacturasContadoCancelaciondeAnticipos()

                    Me.mTextDebug.Text = "Calculando Saldo de Anticipos Facturas de Salida (contado)"
                    Me.mTextDebug.Update()

                    If Me.mParaUsaCta4b = False Then
                        Me.SaldoAnticiposFacturas()
                    Else
                        Me.SaldoAnticiposFacturasPorSeccion()
                    End If


                    Me.mTextDebug.Text = "Cancelación de Anticipos  Facturas de Salida"
                    Me.mTextDebug.Update()

                    '     Me.FacturasContadoTotalVisasComision()

                    Me.mProgress.Value = 30
                    Me.mProgress.Update()
                End If

            End If


            System.Windows.Forms.Application.DoEvents()
            Me.mForm.Update()


            ' ---------------------------------------------------------------
            ' Asiento Notas de Credito de Credito Entidades 51
            '----------------------------------------------------------------



            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then

                If Me.mPerfilCobtable = "FACTURAS" Or Me.mPerfilCobtable = "AMBOS" Then

                    ' evitar lenmtitud
                    SQL = "SELECT COUNT(*) AS TOTAL FROM TNHT_NCRE WHERE "
                    SQL += "  TNHT_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
                    SQL += " OR  TNHT_NCRE.NCRE_DAAN = " & "'" & Me.mFecha & "' "

                    Me.TotalRegistros = CInt(Me.DbLeeHotel.EjecutaSqlScalar(SQL))

                    If Me.TotalRegistros > 0 Then

                        Me.mTextDebug.Text = "Notas de Crédito"
                        Me.mTextDebug.Update()

                        Me.NotasDeCreditoEntidadCredito()

                        Me.mProgress.Value = 50
                        Me.mProgress.Update()



                        Me.mTextDebug.Text = "Detalle de Impuesto Notas de Crédito"
                        Me.mTextDebug.Update()


                        ' MAS RAPIDO USA UNA VISTA QWERTY
                        Me.NotasDeCreditoEntidadCreditoDetalleIgic2()

                        Me.mTextDebug.Text = "Acumulado Base Imponible Notas de Crédito de la Serie de Documento"
                        Me.mTextDebug.Update()



                        Me.NotasDeCreditoEntidadCreditoBaseImponible2()

                        Me.mProgress.Value = 70
                        Me.mProgress.Update()



                        Me.mTextDebug.Text = "Líquido Notas de Crédito"
                        Me.mTextDebug.Update()



                        Me.mProgress.Value = 80
                        Me.mProgress.Update()
                    End If


                End If

            Else
                MsgBox("No dispone de Conexión a la Base de Datos", MsgBoxStyle.Information, "Atención")
            End If


            System.Windows.Forms.Application.DoEvents()
            Me.mForm.Update()


            ' ---------------------------------------------------------------
            ' Asiento de OPERACIONES DE CAJA 90
            '----------------------------------------------------------------

            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then

                If Me.mPerfilCobtable = "CAJA" Or Me.mPerfilCobtable = "AMBOS" Then
                    If Me.mTrataCaja Then

                        Me.OperacionesDeCaja()
                        Me.mTextDebug.Text = "OPERACIONES DE CAJA "
                        Me.mTextDebug.Update()


                        Me.mProgress.Value = 100
                        Me.mProgress.Update()
                    End If
                End If


            End If

            System.Windows.Forms.Application.DoEvents()
            Me.mForm.Update()


            ' VALIDACION DE CUENTAS EB SPYRO TODAS JUNTAS AL FINAL

            '   MsgBox("SE VALIDAN CUENTAS AL FINAL")

            If Me.mParaValidaSpyro = 1 Then
                Me.SpyroCompruebaCuentas()
            End If




            ' Me.AjustarDecimales()
            Me.mProgress.Value = 100
            Me.mProgress.Update()

            Me.CerrarFichero()
            '  Me.CierraConexiones()
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
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum
            SQL += " AND ASNT_IMPRIMIR = 'SI'"



            If IsNumeric(Me.DbLeeCentral.EjecutaSqlScalar(SQL)) Then
                TotalDebe = CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Decimal)
            Else
                TotalDebe = 0
            End If


            SQL = "SELECT ROUND(SUM(round(NVL(ASNT_HABER,'0'),2)),2) FROM TH_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum

            SQL += " AND ASNT_IMPRIMIR = 'SI'"
            If IsNumeric(Me.DbLeeCentral.EjecutaSqlScalar(SQL)) Then
                TotalHaber = CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Decimal)
            Else
                TotalHaber = 0
            End If




            If TotalHaber > TotalDebe Then
                TotalDiferencia = TotalHaber - TotalDebe
                If mMuestraIncidencias = True Then
                    MsgBox("Se va ha producir un Ajuste Decimal  de " & TotalDiferencia & "  " & vbCrLf & vbCrLf & "No Integre con valores superiores a 0.05", MsgBoxStyle.Information, "Atención")
                End If
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 999, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaRedondeo, Me.mIndicadorDebe, "AJUSTE REDONDEO", TotalDiferencia, "SI", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaRedondeo, Me.mIndicadorDebe, "AJUSTE REDONDEO", TotalDiferencia)

                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, "Se va ha producir un Ajuste Decimal al Debe de " & TotalDiferencia & "  " & "No Integre con valores superiores a 0.05")

            End If

            If TotalHaber < TotalDebe Then
                TotalDiferencia = TotalDebe - TotalHaber
                If mMuestraIncidencias = True Then
                    MsgBox("Se va ha producir un Ajuste Decimal  de " & TotalDiferencia & "  " & vbCrLf & vbCrLf & "No Integre con valores superiores a 0.05", MsgBoxStyle.Information, "Atención")
                End If
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 999, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaRedondeo, Me.mIndicadorHaber, "AJUSTE REDONDEO", TotalDiferencia, "SI", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaRedondeo, Me.mIndicadorHaber, "AJUSTE REDONDEO", TotalDiferencia)

                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, "Se va ha producir un Ajuste Decimal al Haber de " & TotalDiferencia & "  " & "No Integre con valores superiores a 0.05")

            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

#End Region
End Class
