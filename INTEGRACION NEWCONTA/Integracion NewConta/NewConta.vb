Option Strict On
Imports System.IO
Imports System.Globalization
Public Class NewConta
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
    ' pat001
    '   Las reclamaciones van a la cuenta del cliente 430...
    ' Las notas de credito que se usan para cancelar facturas NO se Contabilizan
    ' Otros Debitos No se contabilizan a al menos los "descuentos indebidos" 


    Declare Function CharToOem Lib "user32" Alias "CharToOemA" (ByVal lpszSrc As String, ByVal lpszDst As String) As Long
    Declare Function OemToChar Lib "user32" Alias "OemToCharA" (ByVal lpszSrc As String, ByVal lpszDst As String) As Long


    Private mDebug As Boolean = False
    Private mStrConexionHotel As String
    Private mStrConexionCentral As String
    Private mStrConexionSpyro As String
    Private mStrConexionConta As String

    

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

    Private mForm As System.Windows.Forms.Form


    Private mProgress As System.Windows.Forms.ProgressBar
    Private mTrataDebitoTpvnoFacturado As Boolean = False

    Private mParaFilePath As String
    Private mParaFileName As String
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

    Private mEstablecimientoNewConta As String
    Private mOrigenCuentasNewConta As Integer

    Private mHoteCodiNewCentral As Integer






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
    '  Private mCfatodiari_Cod2 As String

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

    Private mCta56DigitoCuentaClientes As String


    Private mOtrosDebitos As Boolean
    Private mOtrosCreditos As Boolean

    Private mCodigoReclamaciones As String
    Private mCodigoNotasCredito As String

    Private mDbSahara As String






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
    Private DbLeeNewHotel As C_DATOS.C_DatosOledb
    Private DbLeeNewHotel2 As C_DATOS.C_DatosOledb
    Private DbNewConta As C_DATOS.C_DatosOledb
    Private DbNewContaAux As C_DATOS.C_DatosOledb
    Private DbGrabaCentral As C_DATOS.C_DatosOledb
    Private DbSpyro As C_DATOS.C_DatosOledb


    Private mTransferenciaComprobante As Integer
    Private mTransferenciaFactura As Integer
    Private mTransferenciaFacturaSerie As String
    Private mTransferenciaFacturaCif As String
    Private mTransferenciaCfbcotmov As String
    Private mTransferenciaFPagoCod As String

    Private mTransferenciaBancosCod As String


    Private mResultStr As String
    Private mResultInt As Integer
    Private mComprobante As Integer
    Private mBanco As String
    Private mTipoComprobantesVersion As Integer
    Private mTipodeEfecto As String



#Region "CONSTRUCTOR"
    Public Sub New(ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vStrConexionCentral As String, _
    ByVal vStrConexionNewConta As String, ByVal vFecha As Date, ByVal vFileName As String, ByVal vDebug As Boolean, _
    ByVal vConrolDebug As System.Windows.Forms.TextBox, ByVal vListBox As System.Windows.Forms.ListBox, _
    ByVal vStrConexionSpyro As String, ByVal vProgress As System.Windows.Forms.ProgressBar, _
    ByVal vEstableciomientoNewConta As String, ByVal vEmpNum As Integer, ByVal vStrConexionHotel As String, _
    ByVal vOtrosCreditos As Boolean, ByVal vOtrosDebitos As Boolean, ByVal vCodigoReclamaciones As String, _
    ByVal vCodigoNotasCredito As String, ByVal vForm As System.Windows.Forms.Form, ByVal vHoteCodiNewCentral As Integer, ByVal vDbSahara As String)


        MyBase.New()

        Me.mDebug = vDebug

        Me.mEmpGrupoCod = vEmpGrupoCod
        Me.mEmpCod = vEmpCod
        Me.mEmpNum = vEmpNum

        Me.mStrConexionConta = vStrConexionNewConta
        Me.mStrConexionCentral = vStrConexionCentral
        Me.mStrConexionSpyro = vStrConexionSpyro

        Me.mStrConexionHotel = vStrConexionHotel


        Me.mOtrosCreditos = vOtrosCreditos
        Me.mOtrosDebitos = vOtrosDebitos

        Me.mCodigoReclamaciones = vCodigoReclamaciones
        Me.mCodigoNotasCredito = vCodigoNotasCredito

        Me.mFecha = vFecha

        Me.mParaFileName = vFileName

        Me.mEstablecimientoNewConta = vEstableciomientoNewConta

        Me.mHoteCodiNewCentral = vHoteCodiNewCentral

        Me.mDbSahara = vDbSahara


        Me.mTextDebug = vConrolDebug

        Me.mProgress = vProgress
        Me.mProgress.Value = 0
        Me.mProgress.Maximum = 100

        Me.mListBoxDebug = vListBox

        Me.mListBoxDebug.Items.Clear()
        Me.mListBoxDebug.Update()

        Me.mForm = vForm




        Me.AbreConexiones()
        Me.CargaParametros()
        Me.CargaParametrosNewConta()

        If Me.mOrigenCuentasNewConta = 0 Then
            Me.DbLeeNewHotel = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
            Me.DbLeeNewHotel.AbrirConexion()
            Me.DbLeeNewHotel.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")
            ' CHAPUZA LOPEZ 
            ' Me.DbLeeNewHotel2 = New C_DATOS.C_DatosOledb("Provider=MSDAORA.1;User ID=RMC2;PASSWORD=RMC2;Data Source=DBSAHARA")
            Me.DbLeeNewHotel2 = New C_DATOS.C_DatosOledb(Me.mDbSahara)
            Me.DbLeeNewHotel2.AbrirConexion()
            Me.DbLeeNewHotel2.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


        End If

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

            Me.DbNewConta = New C_DATOS.C_DatosOledb(Me.mStrConexionConta)
            Me.DbNewConta.AbrirConexion()
            Me.DbNewConta.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbNewContaAux = New C_DATOS.C_DatosOledb(Me.mStrConexionConta)
            Me.DbNewContaAux.AbrirConexion()
            Me.DbNewContaAux.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

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
            SQL += "NVL(PARA_USA_CTACOMISION,'0') USACTACOMISION,"

            SQL += "NVL(PARA_CTA_SERIE_FAC,'0') CTAFACTURAS,"
            SQL += "NVL(PARA_CTA_SERIE_ANUL,'0') CTAFACTURASANULADAS,"
            SQL += "NVL(PARA_CTA_SERIE_NOTAS,'0') CTANOTAS,"

            SQL += "NVL(PARA_CTA_56DIGITO,'0') PARA_CTA_56DIGITO"

            SQL += ",NVL(PARA_LOPEZ_TIPO_COMPROBANTES,'0') PARA_LOPEZ_TIPO_COMPROBANTES"
            SQL += ", NVL(PARA_TEFECT_COD,'?') PARA_TEFECT_COD "


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
                '    Me.mCfatodiari_Cod = CType(Me.DbLeeCentral.mDbLector.Item("DIARIO"), String)
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

                Me.mCta56DigitoCuentaClientes = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA_56DIGITO"), String)

                Me.mTipoComprobantesVersion = CInt(Me.DbLeeCentral.mDbLector.Item("PARA_LOPEZ_TIPO_COMPROBANTES"))
                Me.mTipodeEfecto = CStr(Me.DbLeeCentral.mDbLector.Item("PARA_TEFECT_COD"))
            End If
            Me.DbLeeCentral.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Exclamation, "Carga de Parámetros en Constructor de la Clase")
        End Try
    End Sub
    Private Sub CargaParametrosNewConta()
        Try


            Me.mTextDebug.Text = "Cargando Parámetros NewConta"
            Me.mTextDebug.Update()

            SQL = "SELECT  NVL(PARA_ORIGENCUENTAS,0) AS PARA_ORIGENCUENTAS,NVL(PARA_CFATODIARI_COD,'?') AS  DIARIO "
            SQL += "  FROM TC_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum

            Me.DbLeeCentral.TraerLector(SQL)
            If Me.DbLeeCentral.mDbLector.Read Then
                Me.mOrigenCuentasNewConta = CType(Me.DbLeeCentral.mDbLector.Item("PARA_ORIGENCUENTAS"), Integer)
                Me.mCfatodiari_Cod = CType(Me.DbLeeCentral.mDbLector.Item("DIARIO"), String)

            Else
                Me.mOrigenCuentasNewConta = 0
                Me.mCfatodiari_Cod = "?"

            End If
            Me.DbLeeCentral.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Exclamation, "Carga de Parámetros en Constructor de la Clase")
        End Try
    End Sub
    Private Sub BorraRegistros()
        Try
            SQL = "SELECT COUNT(*) FROM TC_ASNT WHERE ASNT_F_ATOCAB = '" & Me.mFecha & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum
            If CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Integer) > 0 Then
                MsgBox("Ya existen Movimientos de Integración para esta Fecha", MsgBoxStyle.Information, "Atención")
            End If
            Me.mForm.Update()

            SQL = "DELETE TC_ASNT WHERE ASNT_F_ATOCAB = '" & Me.mFecha & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum
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
                                        ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliar As String)

        Try

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TC_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING) values ('"
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
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliar & "')"




            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
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
                    'SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    'SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    'Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                End If
            End If

            If vCfcta_Cod <> "NO TRATAR" Then
                If Me.mParaValidaSpyro = 1 Then
                    '    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                      ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                      , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
                                        ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliar As String, ByVal vAuxiliar2 As String, vComprobante As String, vBancosCod2 As String)

        Try

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TC_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_AUXILIAR_STRING2,ASNT_CFBCOCOMP_COMPROB,ASNT_BANCOS_NOT) values ('"
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
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliar & "','" & vAuxiliar2 & "'," & vComprobante & ",'" & vBancosCod2 & "')"




            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
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
                    'SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    'SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    'Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                End If
            End If

            If vCfcta_Cod <> "NO TRATAR" Then
                If Me.mParaValidaSpyro = 1 Then
                    '        Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                      ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                      , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
                                        ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vFechaValor As Date, ByVal vAuxiliar As String, vAuxiliar2 As String, vBanco As String, vTipoMov As String, vComprobante As String, vConcil As String, vBancosNot As String)

        Try

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TC_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_AUXILIAR_STRING2,ASNT_BANCOS_COD,ASNT_CFBCOTMOV_COD, ASNT_CFBCOCOMP_COMPROB,ASNT_BANCOS_NOT) values ('"
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
            SQL += vImonep & ",'"

            SQL += vConcil & "','"

            '  SQL += "'N','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(vFechaValor, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliar & "','" & vAuxiliar2 & "','" & vBanco & "','" & vTipoMov & "'," & vComprobante & ",'" & vBancosNot & "')"




            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40) & " " & Me.mFecha
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
                    'SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    'SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                    'Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                End If
            End If

            If vCfcta_Cod <> "NO TRATAR" Then
                If Me.mParaValidaSpyro = 1 Then
                    '      Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    Private Sub SpyroCompruebaCuentaOld(ByVal vCuenta As String, ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vLinea As Integer, ByVal vDebeHaber As String)
        Try

            Me.mTextDebug.Text = "Validando Plan de Cuentas Spyro " & vCuenta & "  " & Format(Now, "dd/MM/yyyy H:mm:ss")
            Me.mTextDebug.Update()
            '  System.Windows.Forms.Application.DoEvents()



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
    Private Sub SpyroCompruebaCuentas()
        Try
            SQL = "SELECT ASNT_CFCTA_COD,ASNT_TIPO_REGISTRO,ASNT_CFATOCAB_REFER,ASNT_LINEA,ASNT_CFCPTOS_COD,NVL(ASNT_AMPCPTO,'?') AS ASNT_AMPCPTO,NVL(ASNT_NOMBRE,'?') AS ASNT_NOMBRE FROM TC_ASNT WHERE "
            SQL += "     ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum
            '  SQL += " AND ASNT_F_VALOR = '" & Me.mFecha & "'"
            SQL += " AND ASNT_F_ATOCAB = '" & Me.mFecha & "'"
            SQL += " AND ASNT_CFCTA_COD <> 'NO TRATAR'"

            Me.DbLeeCentral.TraerLector(SQL)
            While Me.DbLeeCentral.mDbLector.Read
                Me.SpyroCompruebaCuenta(CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_CFCTA_COD")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_TIPO_REGISTRO")),
                                        CInt(Me.DbLeeCentral.mDbLector.Item("ASNT_CFATOCAB_REFER")),
                                        CInt(Me.DbLeeCentral.mDbLector.Item("ASNT_LINEA")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_CFCPTOS_COD")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_AMPCPTO")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_NOMBRE")))

            End While
            Me.DbLeeCentral.mDbLector.Close()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub SpyroCompruebaCuentasCorto()
        Try
            SQL = "SELECT ASNT_CFCTA_COD,ASNT_TIPO_REGISTRO,ASNT_CFCPTOS_COD FROM TC_ASNT WHERE "
            SQL += "     ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum
            '  SQL += " AND ASNT_F_VALOR = '" & Me.mFecha & "'"
            SQL += " AND ASNT_F_ATOCAB = '" & Me.mFecha & "'"
            SQL += " AND ASNT_CFCTA_COD <> 'NO TRATAR'"
            SQL += " GROUP BY ASNT_CFCTA_COD,ASNT_TIPO_REGISTRO,ASNT_CFCPTOS_COD "

            Me.DbLeeCentral.TraerLector(SQL)
            While Me.DbLeeCentral.mDbLector.Read
                Me.SpyroCompruebaCuentaCorto(CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_CFCTA_COD")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_TIPO_REGISTRO")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_CFCPTOS_COD")))


            End While
            Me.DbLeeCentral.mDbLector.Close()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub SpyroCompruebaBancos()
        Try
            SQL = "SELECT ASNT_CFCTA_COD,ASNT_TIPO_REGISTRO,ASNT_CFATOCAB_REFER,ASNT_LINEA,ASNT_CFCPTOS_COD,NVL(ASNT_AMPCPTO,'?') AS ASNT_AMPCPTO,NVL(ASNT_NOMBRE,'?') AS ASNT_NOMBRE ,NVL(ASNT_BANCOS_COD,'?') AS ASNT_BANCOS_COD FROM TC_ASNT WHERE "
            SQL += "     ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum
            '  SQL += " AND ASNT_F_VALOR = '" & Me.mFecha & "'"
            SQL += " AND ASNT_F_ATOCAB = '" & Me.mFecha & "'"
            ' SOLO PAGOS
            SQL += " AND ASNT_CFATOCAB_REFER = '" & "111" & "'"
            SQL += " And ASNT_CFCTA_COD <> 'NO TRATAR'"
            SQL += " And ASNT_CFCPTOS_COD = 'D'"

            Me.DbLeeCentral.TraerLector(SQL)
            While Me.DbLeeCentral.mDbLector.Read
                Me.SpyroCompruebaBanco(CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_CFCTA_COD")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_TIPO_REGISTRO")),
                                        CInt(Me.DbLeeCentral.mDbLector.Item("ASNT_CFATOCAB_REFER")),
                                        CInt(Me.DbLeeCentral.mDbLector.Item("ASNT_LINEA")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_CFCPTOS_COD")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_AMPCPTO")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_NOMBRE")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_BANCOS_COD")))

            End While
            Me.DbLeeCentral.mDbLector.Close()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub SpyroCompruebaBanco(ByVal vCuenta As String, ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vLinea As Integer, ByVal vDebeHaber As String, ByVal vAmpcpto As String, ByVal vNombre As String, vBanco As String)
        Try

            Me.mTextDebug.Text = "Validando Bancos Spyro " & vCuenta.PadRight(20, CChar(" ")) & " Longitud : " & vCuenta.Length & "  " & Format(Now, "dd/MM/yyyy H:mm:ss")

            Me.mTextDebug.Update()
            Me.mForm.Update()


            SQL = "SELECT COD FROM BANCOS WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND COD = '" & vBanco & "'"



            If IsNothing(Me.DbSpyro.EjecutaSqlScalar(SQL)) = True Then
                Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & "  No se localiza Código de BANCO Spyro :" & vBanco)
                Me.mListBoxDebug.Update()
                Me.mTexto = "SPYRO   : " & vCuenta & "  No se localiza Código de BANCO Spyro :" & vBanco
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & " " & vAmpcpto & " " & vNombre & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto & " + " & vAmpcpto & " + " & vNombre)

                Exit Sub
            End If




        Catch ex As OleDb.OleDbException
            MsgBox(ex.Message, MsgBoxStyle.Information, " Localiza Cuenta Contable SPYRO")
        End Try
    End Sub
    Private Sub SpyroCompruebaCuenta(ByVal vCuenta As String, ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vLinea As Integer, ByVal vDebeHaber As String, ByVal vAmpcpto As String, ByVal vNombre As String)
        Try

            Me.mTextDebug.Text = "Validando Plan de Cuentas Spyro " & vCuenta.PadRight(20, CChar(" ")) & " Longitud : " & vCuenta.Length & " Para : " & vAmpcpto & " " & vNombre & "  " & Format(Now, "dd/MM/yyyy H:mm:ss")

            Me.mTextDebug.Update()
            Me.mForm.Update()


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

        Catch ex As OleDb.OleDbException
            MsgBox(ex.Message, MsgBoxStyle.Information, " Localiza Cuenta Contable SPYRO")
        End Try
    End Sub
    Private Sub SpyroCompruebaCuentaCorto(ByVal vCuenta As String, ByVal vTipo As String, ByVal vDebeHaber As String)
        Try

            Me.mTextDebug.Text = "Validando Plan de Cuentas Spyro " & vCuenta.PadRight(20, CChar(" ")) & " Longitud : " & vCuenta.Length & " Para : " & vDebeHaber & "  " & Format(Now, "dd/MM/yyyy H:mm:ss")

            Me.mTextDebug.Update()
            Me.mForm.Update()


            SQL = "SELECT COD FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND COD = '" & vCuenta & "'"



            If IsNothing(Me.DbSpyro.EjecutaSqlScalar(SQL)) = True Then
                Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & "  No se localiza en Plan de Cuentas de Spyro")
                Me.mListBoxDebug.Update()
                Me.mTexto = "SPYRO   : " & vCuenta & "  No se localiza en Plan de Cuentas de Spyro"
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & "0" & "," & Linea & ",'" & Me.mTexto & " " & "vAmpcpto" & " " & "vNombre" & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto & " + " & "vAmpcpto" & " + " & "vNombre")

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
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & "0" & "," & Linea & ",'" & Me.mTexto & "')"
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
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & "0" & "," & Linea & ",'" & Me.mTexto & "')"
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
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & "0" & "," & Linea & ",'" & Me.mTexto & "')"
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
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & "0" & "," & Linea & ",'" & Me.mTexto & "')"
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
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & "0" & "," & Linea & ",'" & Me.mTexto & "')"
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
    Private Sub GeneraFileACconFechaValor(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
     ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, ByVal vFechaValor As Date)
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
            Format(vFechaValor, "ddMMyyyy") & _
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
    Private Sub GeneraFileIV(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vFactutipo_cod As String,
    ByVal vNfactura As Integer, ByVal vI_basmonemp As Double, ByVal vPj_iva As Double, ByVal vI_ivamonemp As Double, ByVal vX As String)


        Try
            '-------------------------------------------------------------------------------------------------
            '  Libro de Iva(CFIVALIN)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Me.mCfivaLibro_Cod.PadRight(2, CChar(" ")) &
            vFactutipo_cod.PadRight(6, CChar(" ")) &
            CType(vNfactura, String).PadRight(8, CChar(" ")) &
            Me.mCfivatimpu_Cod.PadRight(2, CChar(" ")) &
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
    Private Sub GeneraFileVV(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String,
    ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String,
    ByVal vPendiente As Double, vCfbcotmovCod As String, vBancosCod As String, vComprobante As String, vNsec As Integer, vHistorico As String)

        Try

            '    TotalRegistros = TotalRegistros + 1
            '-------------------------------------------------------------------------------------------------
            '  Vencimientos
            '-------------------------------------------------------------------------------------------------

            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Me.mCfivaLibro_Cod.PadRight(2, CChar(" ")) &
            vSerie.PadRight(6, CChar(" ")) &
            CType(vNfactura, String).PadLeft(8, CChar(" ")) &
            CType(vNsec, String).PadLeft(2, CChar(" ")) &
            Format(Me.mFecha, "ddMMyyyy") &
            CType(vImonep, String).PadLeft(16, CChar(" ")) &
            Me.mMonedas_Cod.PadRight(4, CChar(" ")) &
            CType(vImonep, String).PadLeft(16, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Mid(vBancosCod, 1, 4).PadRight(4, CChar(" ")) &
            vCuenta.PadRight(15, CChar(" ")) &
            Me.mTipodeEfecto.PadRight(4, CChar(" ")) &
            " ".PadRight(20, CChar(" ")) &
            " ".PadRight(4, CChar(" ")) &  ' pdte
            CType(vPendiente, String).PadLeft(16, CChar(" ")) &
            CType(vPendiente, String).PadLeft(16, CChar(" ")) &
            CType(0, String).PadLeft(16, CChar(" ")) &
            CType(0, String).PadLeft(16, CChar(" ")) &
            "N" &
           vHistorico.PadRight(1, CChar(" ")) &
            " ".PadRight(4, CChar(" ")) ' pdte 
               )




            '     Me.mForm.ParentForm.Text = CStr(TotalRegistros)

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileVV")
        End Try
    End Sub
    Private Sub GeneraFileACconFechaValorComprobanteBancario(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
     ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, ByVal vFechaValor As Date, vBancosCod As String, vCfbCotMov As String, vComprobante As Integer)
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
            Me.Filegraba.WriteLine(MyCharToOem(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Mid(FechaAsiento, 5, 4) &
            Me.mCfatodiari_Cod.PadRight(4, CChar(" ")) &
            " ".PadLeft(8, CChar(" ")) &
            " ".PadLeft(4, CChar(" ")) &
            vCfcta_Cod.PadRight(15, CChar(" ")) &
            vCfcptos_Cod.PadRight(4, CChar(" ")) &
            Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) &
            CType(vImonep, String).PadLeft(16, CChar(" ")) &
            "S" & FechaAsiento &
            Format(vFechaValor, "ddMMyyyy") &
            " ".PadRight(40, CChar(" ")) &
            Me.mCfatotip_Cod.PadRight(4, CChar(" "))) &
            " ".PadRight(2, CChar(" ")) &
            " ".PadRight(6, CChar(" ")) &
            " ".PadLeft(8, CChar(" ")) &
           vBancosCod.PadRight(4, CChar(" ")) &
           vCfbCotMov.PadRight(4, CChar(" ")) &
           CStr(vComprobante).PadLeft(8, CChar(" ")))



        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
        End Try
    End Sub
    Private Sub GeneraFileFVDiariodeCobros(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String,
    ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String, ByVal vPendiente As Double, vFPago As String)

        Try


            '-------------------------------------------------------------------------------------------------
            '  Facturas(FACTURAS)
            '-------------------------------------------------------------------------------------------------
            ' MsgBox(vSfactura)
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Me.mCfivaLibro_Cod.PadRight(2, CChar(" ")) &
            vSerie.PadRight(6, CChar(" ")) &
            CType(vNfactura, String).PadLeft(8, CChar(" ")) &
            " ".PadRight(8, CChar(" ")) &
            Format(Me.mFecha, "ddMMyyyy") &
            Me.mCfivaClase_Cod.PadRight(2, CChar(" ")) &
            CType(vImonep, String).PadLeft(16, CChar(" ")) &
            Me.mMonedas_Cod.PadRight(4, CChar(" ")) &
            CType(vImonep, String).PadLeft(16, CChar(" ")) &
            Mid(vSfactura, 1, 15).PadRight(15, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            "    " &
            " ".PadRight(4, CChar(" ")) &
            " ".PadLeft(8, CChar(" ")) &
            " ".PadLeft(4, CChar(" ")) &
            vCuenta.PadRight(15, CChar(" ")) &
            vCif.PadRight(20, CChar(" ")) &
            vFPago.PadRight(6, CChar(" ")) &
            " ".PadRight(1, CChar(" ")) &
            " ".PadRight(8, CChar(" ")) &
            " ".PadRight(8, CChar(" ")) &
            Me.mGvagente_Cod.PadRight(8, CChar(" ")) &
            CType(vPendiente, String).PadRight(16, CChar(" ")) &
            CType(vPendiente, String).PadRight(16, CChar(" ")) & "NS")



        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileFV")
        End Try
    End Sub
    Private Sub GeneraFileCB(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String,
    ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String,
                             ByVal vCuenta As String, ByVal vCif As String, ByVal vPendiente As Double,
                             vBancosCod As String, vCfbcotmovCod As String, vComprobante As Integer, vConcilSN As String)

        Try


            '-------------------------------------------------------------------------------------------------
            '  Movimientos Bancarios Cabecera
            '-------------------------------------------------------------------------------------------------
            ' MsgBox(vSfactura)
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Mid(vBancosCod, 1, 4).PadRight(4, CChar(" ")) &
            Mid(vCfbcotmovCod, 1, 4).PadRight(4, CChar(" ")) &
            CType(vComprobante, String).PadLeft(8, CChar(" ")) &
             Format(Me.mFecha, "ddMMyyyy") &
            CType(vImonep, String).PadLeft(16, CChar(" ")) &
            Me.mMonedas_Cod.PadRight(4, CChar(" ")) &
            CType(vImonep, String).PadLeft(16, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Format(Me.mFecha, "yyyy") &
             Me.mCfatodiari_Cod.PadRight(4, CChar(" ")) &
            " ".PadLeft(8, CChar(" ")) &
            " ".PadLeft(4, CChar(" ")) &
            "N" &
            Format(Me.mFecha, "ddMMyyyy") &
            vConcilSN.PadRight(1, CChar(" ")))




        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileCB")
        End Try
    End Sub
    Private Sub GeneraFileMG_old(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String,
    ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String, ByVal vPendiente As Double, vCfbcotmovCod As String, vBancosCod As String, vComprobante As String)

        Try


            '-------------------------------------------------------------------------------------------------
            '  Movimientos Bancarios Lineas
            '-------------------------------------------------------------------------------------------------
            ' MsgBox(vSfactura)
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Me.mCfivaLibro_Cod.PadRight(2, CChar(" ")) &
            vSerie.PadRight(6, CChar(" ")) &
            CType(vNfactura, String).PadLeft(8, CChar(" ")) &
            "1 " &
            "1   " &
             Format(Me.mFecha, "ddMMyyyy") &
             Format(Me.mFecha, "ddMMyyyy") &
             Mid(vCfbcotmovCod, 1, 4).PadRight(4, CChar(" ")) &
             CType(vImonep, String).PadLeft(16, CChar(" ")) &
             CType(vImonep, String).PadLeft(16, CChar(" ")) &
             "".PadRight(15, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Format(Me.mFecha, "yyyy") &
              Me.mCfatodiari_Cod.PadRight(4, CChar(" ")) &
             " ".PadLeft(8, CChar(" ")) &
             " ".PadLeft(4, CChar(" ")) &
             Mid(vBancosCod, 1, 4).PadRight(4, CChar(" ")) &
             vComprobante.PadLeft(8, CChar(" ")) &
             " ".PadRight(40, CChar(" ")) &
             "N")



        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileFV")
        End Try
    End Sub
    Private Sub GeneraFileMG(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String,
    ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String, ByVal vPendiente As Double, vCfbcotmovCod As String, vBancosCod As String, vComprobante As String)

        Try


            '-------------------------------------------------------------------------------------------------
            '  Movimientos Bancarios Lineas
            '-------------------------------------------------------------------------------------------------
            ' MsgBox(vSfactura)
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Me.mCfivaLibro_Cod.PadRight(2, CChar(" ")) &
            vSerie.PadRight(6, CChar(" ")) &
            CType(vNfactura, String).PadLeft(8, CChar(" ")) &
            "1 " &
            "1   " &
             Format(Me.mFecha, "ddMMyyyy") &
             Format(Me.mFecha, "ddMMyyyy") &
             Mid(vCfbcotmovCod, 1, 4).PadRight(4, CChar(" ")) &
             CType(vImonep, String).PadLeft(16, CChar(" ")) &
             CType(vImonep, String).PadLeft(16, CChar(" ")) &
             "".PadRight(15, CChar(" ")) &
             " ".PadRight(4, CChar(" ")) &
             " ".PadRight(4, CChar(" ")) &
             " ".PadRight(4, CChar(" ")) &
             " ".PadRight(4, CChar(" ")) &
             " ".PadLeft(8, CChar(" ")) &
             " ".PadLeft(4, CChar(" ")) &
             Mid(vBancosCod, 1, 4).PadRight(4, CChar(" ")) &
             vComprobante.PadLeft(8, CChar(" ")) &
             " ".PadRight(40, CChar(" ")) &
             "N")



        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileFV")
        End Try
    End Sub
    Private Sub CerrarFichero()
        Try
            Filegraba.Close()


            ' BORRA EL FICHERO DE SPYRO SI NO SE VA HA USAR 

            ' borra fichero standard

            If mParaValidaSpyro = 0 Then
                Dim FileToDelete As String = Me.mParaFilePath & Me.mParaFileName
                If System.IO.File.Exists(FileToDelete) = True Then
                    System.IO.File.Delete(FileToDelete)
                End If
            End If
            


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub CierraConexiones()
        Try
            If IsNothing(Me.DbLeeCentral) = False Then
                If Me.DbLeeCentral.EstadoConexion = ConnectionState.Open Then
                    Me.DbLeeCentral.CerrarConexion()
                End If
            End If
            If IsNothing(Me.DbLeeNewHotel) = False Then
                If Me.DbLeeNewHotel.EstadoConexion = ConnectionState.Open Then
                    Me.DbLeeNewHotel.CerrarConexion()
                End If
            End If
            If IsNothing(Me.DbLeeNewHotel2) = False Then
                If Me.DbLeeNewHotel2.EstadoConexion = ConnectionState.Open Then
                    Me.DbLeeNewHotel2.CerrarConexion()
                End If
            End If
            If IsNothing(Me.DbNewConta) = False Then
                If Me.DbNewConta.EstadoConexion = ConnectionState.Open Then
                    Me.DbNewConta.CerrarConexion()
                End If
            End If

            If IsNothing(Me.DbNewContaAux) = False Then
                If Me.DbNewContaAux.EstadoConexion = ConnectionState.Open Then
                    Me.DbNewContaAux.CerrarConexion()
                End If
            End If

            If IsNothing(Me.DbGrabaCentral) = False Then
                If Me.DbGrabaCentral.EstadoConexion = ConnectionState.Open Then
                    Me.DbGrabaCentral.CerrarConexion()
                End If
            End If

            If IsNothing(Me.DbSpyro) = False Then
                If Me.DbSpyro.EstadoConexion = ConnectionState.Open Then
                    Me.DbSpyro.CerrarConexion()
                End If
            End If
           
        Catch ex As Exception

        End Try

    End Sub
    Private Sub FacturasSinCuentaContable()
        Try
            SQL = "SELECT DECODE(FACT_ANUL,'0','EMITIDA','1','ANULADA') AS ESTADO ,FACT_CODI,SEFA_CODI,FACT_TITU FROM TNHT_FACT WHERE FACT_DAEM = '" & Me.mFecha & "'"
            SQL += " AND FACT_STAT IN('2','3') AND ENTI_CODI IS NULL AND CCEX_CODI IS NULL"
            Me.DbNewConta.TraerLector(SQL)
            While Me.DbNewConta.mDbLector.Read
                Me.mTexto = "Factura de Crédito sin cuenta Contable Localizable" & vbCrLf
                Me.mTexto += CType(Me.DbNewConta.mDbLector.Item("FACT_CODI"), String) & "/" & CType(Me.DbNewConta.mDbLector.Item("SEFA_CODI"), String) & vbCrLf
                Me.mTexto += CType(Me.DbNewConta.mDbLector.Item("FACT_TITU"), String) & vbCrLf
                Me.mTexto += "Estado Actual  =" & CType(Me.DbNewConta.mDbLector.Item("ESTADO"), String) & vbCrLf
                MsgBox(Me.mTexto, MsgBoxStyle.Exclamation, "Atención")
            End While
            Me.DbNewConta.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Facturas sin Cuenta Contable")
        End Try
    End Sub
    Private Sub GestionIncidencia(ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vEmpNum As Integer, ByVal vDescripcion As String)

        Try

            SQL = "INSERT INTO TH_INCI (INCI_DATR,INCI_EMPGRUPO_COD,INCI_EMP_COD,INCI_EMP_NUM,INCI_ORIGEN,INCI_DESCRIPCION) "
            SQL += " VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "','" & Me.mEmpGrupoCod & "','" & Me.mEmpCod & "'," & Me.mEmpNum & ",'NEWCONTA COBROS','" & vDescripcion & "')"

            Me.DbGrabaCentral.IniciaTransaccion()

            Me.DbGrabaCentral.EjecutaSql(SQL)

            Me.DbGrabaCentral.ConfirmaTransaccion()

        Catch ex As Exception
            Me.DbGrabaCentral.CancelaTransaccion()
        End Try

    End Sub




#Region "ASIENTOS NEWCONTA"
#Region "ASIENTO 111 PAGOS RECIBIDOS"
    Private Sub NCPagosRecibidos()
        Try
            Dim Total As Double
            Dim Cuenta As String = " "

            Linea = 0

            SQL = "SELECT  'MOVIMIENTOS DE CREDITO TIPO PAGO' , 'DEBE',KEY_FIELD, VNCO_MOCO.HOTE_CODI,VNCO_MOCO.TACO_CODI,NVL(TNCO_TIMO.TIMO_COCO,'0') AS CUENTA, VNCO_MOCO.TIMO_CODI, "
            SQL += "         VNCO_MOCO.MOCO_DOCU, VNCO_MOCO.MOCO_DOC1, VNCO_MOCO.UNMO_CODI, "
            SQL += "         TRUNC (VNCO_MOCO.MOCO_DAVA) DAVA, VNCO_MOCO.MOCO_VAOR, "
            SQL += "         VNCO_MOCO.MOCO_CAMB, VNCO_MOCO.MOCO_DEBI, VNCO_MOCO.MOCO_CRED AS TOTAL, "
            SQL += "         VNCO_MOCO.MOCO_ANUL, NVL(VNCO_MOCO.TACO_NOME,' ') AS TACO_NOME, VNCO_MOCO.MOCO_RECI, "
            SQL += "         VNCO_MOCO.MOCO_CODI, NVL(VNCO_MOCO.MOCO_DESC,' ') AS MOCO_DESC, VNCO_MOCO.MOCO_EXTE, "
            SQL += "         VNCO_MOCO.MOCO_OBSE, VNCO_MOCO.DIFE_RATE, VNCO_MOCO.MOCO_DIFE, "
            SQL += "         ABS (VNCO_MOCO.MOCO_VADE) MOCO_VADE, VNCO_MOCO.PACO_PAAS, "
            SQL += "         VNCO_MOCO.TACO_ORIG, VNCO_MOCO.MOCO_DATR, VNCO_MOCO.HOTE_CODI, "
            SQL += "         HOTE_DESC, NVL(MOLI_DESC,' ') AS MOLI_DESC, VNCO_MOCO.FACT_CODI,TIMO_CECO "
            SQL += "    FROM VNCO_MOCO, TNCO_HOTE, TNCO_UTIL, TNCO_MOLI,TNCO_TIMO "
            SQL += "   WHERE VNCO_MOCO.HOTE_CODI = TNCO_HOTE.HOTE_CODI "
            SQL += "     AND VNCO_MOCO.UTIL_CODI = TNCO_UTIL.UTIL_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "
            SQL += "     AND TNCO_MOLI.LICL_CODI = 1 "
            '    -- SOLO MOV DE CREDITO "
            SQL += "     AND VNCO_MOCO.MOCO_DEBI = 0 "
            '    -- SOLO MOV DE TIPO PAGO "
            SQL += "     AND TNCO_TIMO.TIMO_PAGA = 1 "
            '--
            'SQL += "     AND VNCO_MOCO.MOCO_DAVA = '" & Me.mFecha & "'"
            SQL += "     AND VNCO_MOCO.MOCO_DATR = '" & Me.mFecha & "'"

            SQL += "   AND VNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            SQL += "    ORDER BY VNCO_MOCO.MOCO_DAVA DESC, VNCO_MOCO.MOCO_ANUL "


            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                If CType(Me.DbNewConta.mDbLector("TIMO_CODI"), String) <> mCodigoReclamaciones Then
                    ' Localizar la Cuenta Cobtable de la forma de Cobro
                    SQL = "SELECT NVL(TIMO_COCO,'0') FROM TNCO_TIMO WHERE TIMO_CODI = '" & CType(Me.DbNewConta.mDbLector("TIMO_CODI"), String) & "'"
                    Cuenta = Me.DbNewContaAux.EjecutaSqlScalar(SQL)
                Else
                    ' si es una reclamacion se busca la cuenta del cliente 
                    If mOrigenCuentasNewConta = 1 Then
                        Cuenta = BuscaCuentaClienteCentral(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))
                    Else
                        Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))
                    End If
                End If





                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"

                    If IsDBNull(Me.DbNewConta.mDbLector("TIMO_CECO")) = False Then
                        'FASE 2 2016 COMPROBANTES BANCARIOS 

                        If Me.mTipoComprobantesVersion = 0 Then
                            Me.GeneraComprobanteBanco(111, Total, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), CType(Me.DbNewConta.mDbLector("TIMO_CECO"), String), CType(Me.DbNewConta.mDbLector("TACO_CODI"), String), CInt(Me.DbNewConta.mDbLector("MOCO_CODI")))
                        Else
                            Me.GeneraComprobanteBanco2(111, Total, CType(Me.DbNewConta.mDbLector("TIMO_CECO"), String), CType(Me.DbNewConta.mDbLector("TACO_CODI"), String), CInt(Me.DbNewConta.mDbLector("MOCO_CODI")), 9999, "serie factura")

                        End If
                        Me.InsertaOracle("AC", 111, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, "NO", "", CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", CDate(Me.DbNewConta.mDbLector("DAVA")), CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), "Comprobante Bancario Nº: " & Me.mTransferenciaComprobante, CType(Me.DbNewConta.mDbLector("TIMO_CECO"), String), Me.mTransferenciaCfbcotmov, CStr(Me.mTransferenciaComprobante), "S", Me.mTransferenciaBancosCod)
                        Me.GeneraFileACconFechaValorComprobanteBancario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, CDate(Me.DbNewConta.mDbLector("DAVA")), CStr(Me.DbNewConta.mDbLector("TIMO_CECO")), Me.mTransferenciaCfbcotmov, Me.mTransferenciaComprobante)
                        '   Me.GeneraFileACconFechaValorComprobanteBancario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, CDate(Me.DbNewConta.mDbLector("DAVA")), Me.mTransferenciaBancosCod, Me.mTransferenciaCfbcotmov, Me.mTransferenciaComprobante)


                    Else
                        Me.InsertaOracle("AC", 111, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, "NO", "", CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", CDate(Me.DbNewConta.mDbLector("DAVA")), CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), "", "", "", "null", "N", Me.mTransferenciaBancosCod)
                         Me.GeneraFileACconFechaValor("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, CDate(Me.DbNewConta.mDbLector("DAVA")))


                    End If







                End If
            End While
            Me.DbNewConta.mDbLector.Close()

            '' lo mismo al reves ( deposito anticipado ) 

            SQL = "SELECT  'MOVIMIENTOS DE CREDITO TIPO PAGO' , 'DEBE',KEY_FIELD, VNCO_MOCO.HOTE_CODI,VNCO_MOCO.TACO_CODI AS TACO_CODI ,NVL(TNCO_TACO.TACO_CODA,'0') AS CUENTA, VNCO_MOCO.TIMO_CODI, "
            SQL += "         VNCO_MOCO.MOCO_DOCU, VNCO_MOCO.MOCO_DOC1, VNCO_MOCO.UNMO_CODI, "
            SQL += "         TRUNC (VNCO_MOCO.MOCO_DAVA) DAVA, VNCO_MOCO.MOCO_VAOR, "
            SQL += "         VNCO_MOCO.MOCO_CAMB, VNCO_MOCO.MOCO_DEBI, VNCO_MOCO.MOCO_CRED AS TOTAL, "
            SQL += "         VNCO_MOCO.MOCO_ANUL, NVL(VNCO_MOCO.TACO_NOME,' ') AS TACO_NOME , VNCO_MOCO.MOCO_RECI, "
            SQL += "         VNCO_MOCO.MOCO_CODI, NVL(VNCO_MOCO.MOCO_DESC,' ') AS MOCO_DESC, VNCO_MOCO.MOCO_EXTE, "
            SQL += "         VNCO_MOCO.MOCO_OBSE, VNCO_MOCO.DIFE_RATE, VNCO_MOCO.MOCO_DIFE, "
            SQL += "         ABS (VNCO_MOCO.MOCO_VADE) MOCO_VADE, VNCO_MOCO.PACO_PAAS, "
            SQL += "         VNCO_MOCO.TACO_ORIG, VNCO_MOCO.MOCO_DATR, VNCO_MOCO.HOTE_CODI, "
            SQL += "         HOTE_DESC, NVL(MOLI_DESC,' ') AS MOLI_DESC, VNCO_MOCO.FACT_CODI , NVL(TNCO_TACO.TACO_NUCO,'?') AS NIF"
            SQL += "    FROM VNCO_MOCO, TNCO_HOTE, TNCO_UTIL, TNCO_MOLI,TNCO_TIMO,TNCO_TACO"
            SQL += "   WHERE VNCO_MOCO.HOTE_CODI = TNCO_HOTE.HOTE_CODI "
            SQL += "     AND VNCO_MOCO.UTIL_CODI = TNCO_UTIL.UTIL_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "
            SQL += "     AND VNCO_MOCO.TACO_CODI = TNCO_TACO.TACO_CODI "
            SQL += "     AND TNCO_MOLI.LICL_CODI = 1 "
            '    -- SOLO MOV DE CREDITO "
            SQL += "     AND VNCO_MOCO.MOCO_DEBI = 0 "
            '    -- SOLO MOV DE TIPO PAGO "
            SQL += "     AND TNCO_TIMO.TIMO_PAGA = 1 "
            '--
            ' SQL += "     AND VNCO_MOCO.MOCO_DAVA = '" & Me.mFecha & "'"
            SQL += "     AND VNCO_MOCO.MOCO_DATR= '" & Me.mFecha & "'"

            SQL += "     AND VNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            SQL += "    ORDER BY VNCO_MOCO.MOCO_DAVA DESC, VNCO_MOCO.MOCO_ANUL "


            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                If mOrigenCuentasNewConta = 1 Then
                    Cuenta = BuscaCuentaPagosAnticipadosCentral(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))
                Else
                    Cuenta = BuscaCuentaPagosAnticipadosNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))
                End If



                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 111, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", CDate(Me.DbNewConta.mDbLector("DAVA")), CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), "", "", "", "null", "N", "")
                    Me.GeneraFileACconFechaValor("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, CDate(Me.DbNewConta.mDbLector("DAVA")))
                End If
            End While
            Me.DbNewConta.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try


    End Sub
#End Region
#Region "ASIENTO 222 PAGOS DE MAS"
    Private Sub NCPagosDeMas()
        Try
            Dim Total As Double
            Dim Cuenta As String = " "

            Dim TipoMovimientoPagodeMas As String


            SQL = "SELECT PACO_PAAS FROM TNCO_PACO"
            TipoMovimientoPagodeMas = Me.DbNewContaAux.EjecutaSqlScalar(SQL)

            Linea = 0

            SQL = "SELECT  'MOVIMIENTOS DE CREDITO TIPO PAGO DE MAS' , 'DEBE',KEY_FIELD, VNCO_MOCO.HOTE_CODI,VNCO_MOCO.TACO_CODI,NVL(TNCO_TIMO.TIMO_COCO,'0') AS CUENTA, VNCO_MOCO.TIMO_CODI, "
            SQL += "         VNCO_MOCO.MOCO_DOCU, VNCO_MOCO.MOCO_DOC1, VNCO_MOCO.UNMO_CODI, "
            SQL += "         TRUNC (VNCO_MOCO.MOCO_DAVA) DAVA, VNCO_MOCO.MOCO_VAOR, "
            SQL += "         VNCO_MOCO.MOCO_CAMB, VNCO_MOCO.MOCO_DEBI, VNCO_MOCO.MOCO_DEBI AS TOTAL, "
            SQL += "         VNCO_MOCO.MOCO_ANUL, NVL(VNCO_MOCO.TACO_NOME,' ') AS TACO_NOME, VNCO_MOCO.MOCO_RECI, "
            SQL += "         VNCO_MOCO.MOCO_CODI, NVL(VNCO_MOCO.MOCO_DESC,' ') AS MOCO_DESC, VNCO_MOCO.MOCO_EXTE, "
            SQL += "         VNCO_MOCO.MOCO_OBSE, VNCO_MOCO.DIFE_RATE, VNCO_MOCO.MOCO_DIFE, "
            SQL += "         ABS (VNCO_MOCO.MOCO_VADE) MOCO_VADE, VNCO_MOCO.PACO_PAAS, "
            SQL += "         VNCO_MOCO.TACO_ORIG, VNCO_MOCO.MOCO_DATR, VNCO_MOCO.HOTE_CODI, "
            SQL += "         HOTE_DESC, NVL(MOLI_DESC,' ') AS MOLI_DESC, VNCO_MOCO.FACT_CODI "
            SQL += "    FROM VNCO_MOCO, TNCO_HOTE, TNCO_UTIL, TNCO_MOLI,TNCO_TIMO "
            SQL += "   WHERE VNCO_MOCO.HOTE_CODI = TNCO_HOTE.HOTE_CODI "
            SQL += "     AND VNCO_MOCO.UTIL_CODI = TNCO_UTIL.UTIL_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "
            SQL += "     AND TNCO_MOLI.LICL_CODI = 1 "

            SQL += " AND VNCO_MOCO.TIMO_CODI = '" & TipoMovimientoPagodeMas & "'"

            SQL += "     AND VNCO_MOCO.MOCO_DATR = '" & Me.mFecha & "'"

            SQL += "   AND VNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            SQL += "    ORDER BY VNCO_MOCO.MOCO_DAVA DESC, VNCO_MOCO.MOCO_ANUL "


            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                ' Localizar la Cuenta Cobtable de la forma de Cobro
                SQL = "SELECT NVL(TIMO_COCO,'0') FROM TNCO_TIMO WHERE TIMO_CODI = '" & CType(Me.DbNewConta.mDbLector("TIMO_CODI"), String) & "'"
                Cuenta = Me.DbNewContaAux.EjecutaSqlScalar(SQL)

                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 222, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, "NO", "", CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", CDate(Me.DbNewConta.mDbLector("DAVA")), CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), "", "", "", "null", "N", "")
                    Me.GeneraFileACconFechaValor("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, CDate(Me.DbNewConta.mDbLector("DAVA")))
                End If
            End While
            Me.DbNewConta.mDbLector.Close()

            '' lo mismo al reves ( deposito anticipado ) 

            SQL = "SELECT  'MOVIMIENTOS DE CREDITO TIPO PAGO' , 'DEBE',KEY_FIELD, VNCO_MOCO.HOTE_CODI,VNCO_MOCO.TACO_CODI AS TACO_CODI ,NVL(TNCO_TACO.TACO_CODA,'0') AS CUENTA, VNCO_MOCO.TIMO_CODI, "
            SQL += "         VNCO_MOCO.MOCO_DOCU, VNCO_MOCO.MOCO_DOC1, VNCO_MOCO.UNMO_CODI, "
            SQL += "         TRUNC (VNCO_MOCO.MOCO_DAVA) DAVA, VNCO_MOCO.MOCO_VAOR, "
            SQL += "         VNCO_MOCO.MOCO_CAMB, VNCO_MOCO.MOCO_DEBI, VNCO_MOCO.MOCO_DEBI AS TOTAL, "
            SQL += "         VNCO_MOCO.MOCO_ANUL, NVL(VNCO_MOCO.TACO_NOME,' ') AS TACO_NOME , VNCO_MOCO.MOCO_RECI, "
            SQL += "         VNCO_MOCO.MOCO_CODI, NVL(VNCO_MOCO.MOCO_DESC,' ') AS MOCO_DESC, VNCO_MOCO.MOCO_EXTE, "
            SQL += "         VNCO_MOCO.MOCO_OBSE, VNCO_MOCO.DIFE_RATE, VNCO_MOCO.MOCO_DIFE, "
            SQL += "         ABS (VNCO_MOCO.MOCO_VADE) MOCO_VADE, VNCO_MOCO.PACO_PAAS, "
            SQL += "         VNCO_MOCO.TACO_ORIG, VNCO_MOCO.MOCO_DATR, VNCO_MOCO.HOTE_CODI, "
            SQL += "         HOTE_DESC, NVL(MOLI_DESC,' ') AS MOLI_DESC, VNCO_MOCO.FACT_CODI , NVL(TNCO_TACO.TACO_NUCO,'?') AS NIF"
            SQL += "    FROM VNCO_MOCO, TNCO_HOTE, TNCO_UTIL, TNCO_MOLI,TNCO_TIMO,TNCO_TACO"
            SQL += "   WHERE VNCO_MOCO.HOTE_CODI = TNCO_HOTE.HOTE_CODI "
            SQL += "     AND VNCO_MOCO.UTIL_CODI = TNCO_UTIL.UTIL_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "
            SQL += "     AND VNCO_MOCO.TACO_CODI = TNCO_TACO.TACO_CODI "
            SQL += "     AND TNCO_MOLI.LICL_CODI = 1 "

            SQL += " AND VNCO_MOCO.TIMO_CODI = '" & TipoMovimientoPagodeMas & "'"

            SQL += "     AND VNCO_MOCO.MOCO_DATR= '" & Me.mFecha & "'"

            SQL += "     AND VNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            SQL += "    ORDER BY VNCO_MOCO.MOCO_DAVA DESC, VNCO_MOCO.MOCO_ANUL "


            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                If mOrigenCuentasNewConta = 1 Then
                    Cuenta = BuscaCuentaPagosAnticipadosCentral(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))
                Else
                    Cuenta = BuscaCuentaPagosAnticipadosNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))
                End If


                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 222, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, "*** " & CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", CDate(Me.DbNewConta.mDbLector("DAVA")), CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), "", "", "", "null", "N", "")
                    Me.GeneraFileACconFechaValor("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, " " & CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), Total, CDate(Me.DbNewConta.mDbLector("DAVA")))
                End If
            End While
            Me.DbNewConta.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try


    End Sub
#End Region

#Region "ASIENTO 333 FACTURAS REGULARIZADAS"
    Private Sub NCFacturasRegularizadas()
        Try
            Dim Total As Double
            Dim Cuenta As String
            Linea = 0
            Dim EsFormaDePago As Boolean = False


            SQL = "SELECT '1', /*+ INDEX(TNCO_MORE TNCO_MORE_PRIMARY) */ TAC1.TACO_CODA "
            SQL += "                                                                    TACO_DACR, "
            SQL += "       TAC1.TACO_COCO TACO_COCR , TAC2.TACO_CODA TACO_DADB, "
            SQL += "       TAC2.TACO_COCO TACO_CODB, MOCO.MOCO_CODI MOCO_DEBI, "
            SQL += "       MOCO.TIMO_CODI TIMO_DEBI, MOCO.MOCO_DOCU DOCU_DEBI, "
            SQL += "       NVL(MOCO.MOCO_DESC,'?') DESC_DEBI, MOC1.MOCO_CODI MOCO_CRED, "
            SQL += "       MOC1.TIMO_CODI TIMO_CRED, MOC1.MOCO_DOCU DOCU_CRED, "
            SQL += "       NVL(MOC1.MOCO_DESC,'?') DESC_CRED, MORE_DARE, MORE_VAPA AS TOTAL, MORE_DEAN, "
            SQL += "       TIMO_DEBI.TIMO_CDIA DEBI_CDIA, TIMO_DEBI.TIMO_CDOC DEBI_CDOC, "
            SQL += "       TIMO_CRED.TIMO_CDIA CRED_CDIA, TIMO_CRED.TIMO_CDOC CRED_CDOC, "
            SQL += "       MOCO.TACO_CODI TACO_DEBI, MOC1.TACO_CODI TACO_CRED,"
            SQL += "       NVL(TAC1.TACO_COCO,'0') AS CUENTA, NVL(TAC1.TACO_NUCO,'?') AS NIF, "
            SQL += "       TAC1.TACO_CODI AS CODIGO, NVL(TAC1.TACO_NOME,'?') AS NOMBRE "
            SQL += "       ,NVL(TIMO_CRED.TIMO_PAGA,'0') AS TIPOPAGO ,"
            SQL += "        NVL(TNCO_MOLI.MOLI_DESC,' ') AS MOLI_DESC,"
            SQL += "        NVL(TNCO_MOLI2.MOLI_DESC,' ') AS MOLI2_DESC"


            SQL += "        ,MOC1.MOCO_CODI"
            SQL += "        ,NVL(MOCO.FACT_CODI,'0') AS FACT_CODI "
            SQL += "        ,MOCO.SEFA_CODI"

            SQL += "       FROM TNCO_MORE, "
            SQL += "       TNCO_MOCO MOCO, "
            SQL += "       TNCO_MOCO MOC1, "
            SQL += "       TNCO_TACO TAC1, "
            SQL += "       TNCO_TACO TAC2, "
            SQL += "       TNCO_TIMO TIMO_DEBI, "
            SQL += "       TNCO_TIMO TIMO_CRED, "
            SQL += "       TNCO_MOLI, "

            SQL += "       TNCO_MOLI TNCO_MOLI2"

            SQL += " WHERE TNCO_MORE.TACO_COD1 = TAC1.TACO_CODI "
            SQL += "   AND TNCO_MORE.TACO_COD2 = TAC2.TACO_CODI "
            SQL += "   AND TNCO_MORE.TACO_COD2 = MOCO.TACO_CODI "
            SQL += "   AND TNCO_MORE.MOCO_COD2 = MOCO.MOCO_CODI "
            SQL += "   AND MOCO.TIMO_CODI = TIMO_DEBI.TIMO_CODI "
            SQL += "   AND TNCO_MORE.TACO_COD1 = MOC1.TACO_CODI "
            SQL += "   AND TNCO_MORE.MOCO_COD1 = MOC1.MOCO_CODI "
            SQL += "   AND MOC1.TIMO_CODI = TIMO_CRED.TIMO_CODI "

            SQL += "   AND MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI "
            SQL += "   AND  MOC1.TIMO_CODI = TNCO_MOLI2.TIMO_CODI "


            SQL += "   AND TNCO_MOLI.LICL_CODI = 1 "
            SQL += "   AND TNCO_MOLI2.LICL_CODI = 1 "


            SQL += "   AND TNCO_MORE.MORE_REGU = 1 "

            ' segun chany/edelia no se tratan notas de credito 
            SQL += "   AND MOC1.TIMO_CODI <> '" & Me.mCodigoNotasCredito & "'"

            SQL += "  AND MORE_DARE = '" & Me.mFecha & "'"
            SQL += "  AND MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"




            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                Linea = Linea + 1

                Cuenta = Mid(CType(Me.DbNewConta.mDbLector("CUENTA"), String), 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(CType(Me.DbNewConta.mDbLector("CUENTA"), String), 5, 6)

                If mOrigenCuentasNewConta = 1 Then
                    Cuenta = BuscaCuentaClienteCentral(CType(Me.DbNewConta.mDbLector("TACO_DEBI"), String))
                Else
                    Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_DEBI"), String))
                End If

                If CType(Me.DbNewConta.mDbLector("TIPOPAGO"), String) = "0" Then
                    EsFormaDePago = False
                Else
                    EsFormaDePago = True
                End If


                ' Si el documento o parte del documento se saldo con una nota de credito el importe va negativo
                'If EsFormaDePago = True Then
                'Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                'Else
                'Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double) * -1
                'End If

                ' Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                Total = Math.Round(CType(Me.DbNewConta.mDbLector("TOTAL"), Double), 2)


                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"



                    ' Busca Comprobante de Cobro en  TC_COMP con el que se van a cobrar las facturas

                    SQL = "SELECT COMP_NUME  "
                    SQL += " FROM TC_COMP WHERE TACO_CODI = '" & CStr(Me.DbNewConta.mDbLector("TACO_DEBI")) & "'"
                    SQL += " AND MOCO_CODI = " & CInt(Me.DbNewConta.mDbLector("MOCO_CODI"))

                    Me.mResultStr = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

                    If IsNothing(Me.mResultStr) = False Then
                        Me.mComprobante = CInt(Me.mResultStr)
                    Else
                        Me.mComprobante = 0
                    End If


                    If Me.mTipoComprobantesVersion = 1 Then
                        Me.GeneraComprobanteBancoRegularizacion(333, Total, CType(Me.DbNewConta.mDbLector("TACO_DEBI"), String), CInt(Me.DbNewConta.mDbLector("MOCO_CODI")), CInt(Me.DbNewConta.mDbLector("FACT_CODI")), CStr(Me.DbNewConta.mDbLector("SEFA_CODI")))

                    End If

                    '20180327
                    ' ANTES SE HACIA ANTES DE GENERAR EL COMPROBANTE 

                    Me.InsertaOracle("AC", 333, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("DESC_DEBI"), String), Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("CODIGO"), String) & " " & CType(Me.DbNewConta.mDbLector("NOMBRE"), String), "SI", CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String).PadRight(50, CChar(" ")), "  Pagada con : [" & CType(Me.DbNewConta.mDbLector("MOLI2_DESC"), String) & "] + " & CType(Me.DbNewConta.mDbLector("DESC_CRED"), String), CStr(Me.mComprobante), "")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("DESC_DEBI"), String), Total)


                End If
            End While
            Me.DbNewConta.mDbLector.Close()

            '' lo mismo al reves ( deposito anticipado ) 

            SQL = "SELECT '1', /*+ INDEX(TNCO_MORE TNCO_MORE_PRIMARY) */ TAC1.TACO_CODA "
            SQL += "                                                                    TACO_DACR, "
            SQL += "       TAC1.TACO_COCO TACO_COCR , TAC2.TACO_CODA TACO_DADB, "
            SQL += "       TAC2.TACO_COCO TACO_CODB, MOCO.MOCO_CODI MOCO_DEBI, "
            SQL += "       MOCO.TIMO_CODI TIMO_DEBI, MOCO.MOCO_DOCU DOCU_DEBI, "
            SQL += "       NVL(MOCO.MOCO_DESC,'?') DESC_DEBI, MOC1.MOCO_CODI MOCO_CRED, "
            SQL += "       MOC1.TIMO_CODI TIMO_CRED, MOC1.MOCO_DOCU DOCU_CRED, "
            SQL += "       NVL(MOC1.MOCO_DESC,'?') DESC_CRED, MORE_DARE, MORE_VAPA AS TOTAL, MORE_DEAN, "
            SQL += "       TIMO_DEBI.TIMO_CDIA DEBI_CDIA, TIMO_DEBI.TIMO_CDOC DEBI_CDOC, "
            SQL += "       TIMO_CRED.TIMO_CDIA CRED_CDIA, TIMO_CRED.TIMO_CDOC CRED_CDOC, "
            SQL += "       MOCO.TACO_CODI TACO_DEBI, MOC1.TACO_CODI TACO_CRED ,"
            '  SQL += "       NVL(TIMO_CRED.TIMO_COCO,'0')  AS CUENTA "
            SQL += "        NVL(TAC1.TACO_CODA,'0') AS CUENTA ,"
            SQL += "       TAC1.TACO_CODI AS CODIGO, NVL(TAC1.TACO_NOME,'?') AS NOMBRE ,NVL(TAC1.TACO_NUCO,'?') AS NIF"

            SQL += "       ,NVL(TIMO_CRED.TIMO_PAGA,'0') AS TIPOPAGO , "
            SQL += "        NVL(MOLI_DESC,' ') AS MOLI_DESC"


            SQL += "  FROM TNCO_MORE, "
            SQL += "       TNCO_MOCO MOCO, "
            SQL += "       TNCO_MOCO MOC1, "
            SQL += "       TNCO_TACO TAC1, "
            SQL += "       TNCO_TACO TAC2, "
            SQL += "       TNCO_TIMO TIMO_DEBI, "
            SQL += "       TNCO_TIMO TIMO_CRED, "

            SQL += "       TNCO_MOLI "

            SQL += " WHERE TNCO_MORE.TACO_COD1 = TAC1.TACO_CODI "
            SQL += "   AND TNCO_MORE.TACO_COD2 = TAC2.TACO_CODI "
            SQL += "   AND TNCO_MORE.TACO_COD2 = MOCO.TACO_CODI "
            SQL += "   AND TNCO_MORE.MOCO_COD2 = MOCO.MOCO_CODI "
            SQL += "   AND MOCO.TIMO_CODI = TIMO_DEBI.TIMO_CODI "
            SQL += "   AND TNCO_MORE.TACO_COD1 = MOC1.TACO_CODI "
            SQL += "   AND TNCO_MORE.MOCO_COD1 = MOC1.MOCO_CODI "
            SQL += "   AND MOC1.TIMO_CODI = TIMO_CRED.TIMO_CODI "

            SQL += "     AND MOC1.TIMO_CODI = TNCO_MOLI.TIMO_CODI "
            SQL += "     AND TNCO_MOLI.LICL_CODI = 1 "

            SQL += "   AND TNCO_MORE.MORE_REGU = 1 "

            ' segun chany/edelia no se tratan notas de credito 
            SQL += "   AND MOC1.TIMO_CODI <> '" & Me.mCodigoNotasCredito & "'"

            SQL += "  AND MORE_DARE = '" & Me.mFecha & "'"
            SQL += "  AND MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"


            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                Linea = Linea + 1



                If mOrigenCuentasNewConta = 1 Then
                    Cuenta = BuscaCuentaPagosAnticipadosCentral(CType(Me.DbNewConta.mDbLector("CODIGO"), String))
                Else
                    Cuenta = BuscaCuentaPagosAnticipadosNewHotel(CType(Me.DbNewConta.mDbLector("CODIGO"), String))
                End If




                ' Si el documento o parte del documento se saldo con una nota de credito el importe va negativo

                If CType(Me.DbNewConta.mDbLector("TIPOPAGO"), String) = "0" Then
                    EsFormaDePago = False
                Else
                    EsFormaDePago = True
                End If


                'If EsFormaDePago = True Then
                ' Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                'Else
                'Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double) * -1
                'End If

                'Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                Total = Math.Round(CType(Me.DbNewConta.mDbLector("TOTAL"), Double), 2)

                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 333, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbNewConta.mDbLector("DESC_CRED"), String), Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("CODIGO"), String) & " " & CType(Me.DbNewConta.mDbLector("NOMBRE"), String), "SI", CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String).PadRight(50, CChar(" ")) & " [Es Forma de PAgo = " & EsFormaDePago.ToString & "]", "  Paga a : " & CType(Me.DbNewConta.mDbLector("DESC_DEBI"), String), "NULL", "")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, " " & CType(Me.DbNewConta.mDbLector("DESC_CRED"), String), Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try


    End Sub

#End Region
#Region "ASIENTO 333-A FACTURAS DES-REGULARIZADAS AL ANULAR COBROS"
    Private Sub NCFacturasRegularizadasAnuladas()
        Try
            Dim Total As Double
            Dim Cuenta As String
            Linea = 0
            Dim EsFormaDePago As Boolean = False


            SQL = "SELECT '1', /*+ INDEX(TNCO_MORE TNCO_MORE_PRIMARY) */ TAC1.TACO_CODA "
            SQL += "                                                                    TACO_DACR, "
            SQL += "       TAC1.TACO_COCO TACO_COCR , TAC2.TACO_CODA TACO_DADB, "
            SQL += "       TAC2.TACO_COCO TACO_CODB, MOCO.MOCO_CODI MOCO_DEBI, "
            SQL += "       MOCO.TIMO_CODI TIMO_DEBI, MOCO.MOCO_DOCU DOCU_DEBI, "
            SQL += "       NVL(MOCO.MOCO_DESC,'?') DESC_DEBI, MOC1.MOCO_CODI MOCO_CRED, "
            SQL += "       MOC1.TIMO_CODI TIMO_CRED, MOC1.MOCO_DOCU DOCU_CRED, "
            SQL += "       NVL(MOC1.MOCO_DESC,'?') DESC_CRED, MORE_DARE, MORE_VAPA AS TOTAL, MORE_DEAN, "
            SQL += "       TIMO_DEBI.TIMO_CDIA DEBI_CDIA, TIMO_DEBI.TIMO_CDOC DEBI_CDOC, "
            SQL += "       TIMO_CRED.TIMO_CDIA CRED_CDIA, TIMO_CRED.TIMO_CDOC CRED_CDOC, "
            SQL += "       MOCO.TACO_CODI TACO_DEBI, MOC1.TACO_CODI TACO_CRED,"
            SQL += "       NVL(TAC1.TACO_COCO,'0') AS CUENTA, NVL(TAC1.TACO_NUCO,'?') AS NIF, "
            SQL += "       TAC1.TACO_CODI AS CODIGO, NVL(TAC1.TACO_NOME,'?') AS NOMBRE "
            SQL += "       ,NVL(TIMO_CRED.TIMO_PAGA,'0') AS TIPOPAGO ,"
            SQL += "        NVL(TNCO_MOLI.MOLI_DESC,' ') AS MOLI_DESC,"
            SQL += "        NVL(TNCO_MOLI2.MOLI_DESC,' ') AS MOLI2_DESC"



            SQL += "        ,MOC1.MOCO_CODI"
            SQL += "        ,MOCO.FACT_CODI"
            SQL += "        ,MOCO.SEFA_CODI"

            SQL += "       FROM TNCO_MORE, "
            SQL += "       TNCO_MOCO MOCO, "
            SQL += "       TNCO_MOCO MOC1, "
            SQL += "       TNCO_TACO TAC1, "
            SQL += "       TNCO_TACO TAC2, "
            SQL += "       TNCO_TIMO TIMO_DEBI, "
            SQL += "       TNCO_TIMO TIMO_CRED, "
            SQL += "       TNCO_MOLI, "

            SQL += "       TNCO_MOLI TNCO_MOLI2"

            SQL += " WHERE TNCO_MORE.TACO_COD1 = TAC1.TACO_CODI "
            SQL += "   AND TNCO_MORE.TACO_COD2 = TAC2.TACO_CODI "
            SQL += "   AND TNCO_MORE.TACO_COD2 = MOCO.TACO_CODI "
            SQL += "   AND TNCO_MORE.MOCO_COD2 = MOCO.MOCO_CODI "
            SQL += "   AND MOCO.TIMO_CODI = TIMO_DEBI.TIMO_CODI "
            SQL += "   AND TNCO_MORE.TACO_COD1 = MOC1.TACO_CODI "
            SQL += "   AND TNCO_MORE.MOCO_COD1 = MOC1.MOCO_CODI "
            SQL += "   AND MOC1.TIMO_CODI = TIMO_CRED.TIMO_CODI "

            SQL += "   AND MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI "
            SQL += "   AND  MOC1.TIMO_CODI = TNCO_MOLI2.TIMO_CODI "


            SQL += "   AND TNCO_MOLI.LICL_CODI = 1 "
            SQL += "   AND TNCO_MOLI2.LICL_CODI = 1 "


            SQL += "   AND TNCO_MORE.MORE_REGU = 1 "

            ' segun chany/edelia no se tratan notas de credito 
            SQL += "   AND MOC1.TIMO_CODI <> '" & Me.mCodigoNotasCredito & "'"

            SQL += "  AND MORE_DAAN = '" & Me.mFecha & "'"
            SQL += "  AND MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"




            Me.DbNewConta.TraerLector(SQL)

            If Me.DbNewConta.mDbLector.HasRows Then
                MsgBox("Existen Documentos que han sido Afectados por la Anulación de Pagos ", MsgBoxStyle.Information, "Atención")
            End If

            While Me.DbNewConta.mDbLector.Read

                Linea = Linea + 1

                Cuenta = Mid(CType(Me.DbNewConta.mDbLector("CUENTA"), String), 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(CType(Me.DbNewConta.mDbLector("CUENTA"), String), 5, 6)

                If mOrigenCuentasNewConta = 1 Then
                    Cuenta = BuscaCuentaClienteCentral(CType(Me.DbNewConta.mDbLector("TACO_DEBI"), String))
                Else
                    Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_DEBI"), String))
                End If

                If CType(Me.DbNewConta.mDbLector("TIPOPAGO"), String) = "0" Then
                    EsFormaDePago = False
                Else
                    EsFormaDePago = True
                End If


                ' Si el documento o parte del documento se saldo con una nota de credito el importe va negativo
                'If EsFormaDePago = True Then
                'Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                'Else
                'Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double) * -1
                'End If

                ' Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double) * -1
                Total = Math.Round(CType(Me.DbNewConta.mDbLector("TOTAL"), Double), 2) * -1


                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"


                    ' Busca Comprobante de Cobro en  TC_COMP con el que se van a cobrar las facturas

                    SQL = "SELECT COMP_NUME  "
                    SQL += " FROM TC_COMP WHERE TACO_CODI = '" & CStr(Me.DbNewConta.mDbLector("TACO_DEBI")) & "'"
                    SQL += " AND MOCO_CODI = " & CInt(Me.DbNewConta.mDbLector("MOCO_CODI"))

                    Me.mResultStr = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

                    If IsNothing(Me.mResultStr) = False Then
                        Me.mComprobante = CInt(Me.mResultStr)
                    Else
                        Me.mComprobante = 0
                    End If
                    Me.InsertaOracle("AC", 777, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("DESC_DEBI"), String), Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("CODIGO"), String) & " " & CType(Me.DbNewConta.mDbLector("NOMBRE"), String), "SI", CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String).PadRight(50, CChar(" ")), " Pagada con : [" & CType(Me.DbNewConta.mDbLector("MOLI2_DESC"), String) & "] + " & CType(Me.DbNewConta.mDbLector("DESC_CRED"), String), CStr(Me.mComprobante), "")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("DESC_DEBI"), String), Total)


                    If Me.mTipoComprobantesVersion = 1 Then
                        Me.GeneraComprobanteBancoRegularizacion(777, Total, CType(Me.DbNewConta.mDbLector("TACO_DEBI"), String), CInt(Me.DbNewConta.mDbLector("MOCO_CODI")), CInt(Me.DbNewConta.mDbLector("FACT_CODI")), CStr(Me.DbNewConta.mDbLector("SEFA_CODI")))

                    End If

                End If
            End While
            Me.DbNewConta.mDbLector.Close()

            '' lo mismo al reves ( deposito anticipado ) 

            SQL = "SELECT '1', /*+ INDEX(TNCO_MORE TNCO_MORE_PRIMARY) */ TAC1.TACO_CODA "
            SQL += "                                                                    TACO_DACR, "
            SQL += "       TAC1.TACO_COCO TACO_COCR , TAC2.TACO_CODA TACO_DADB, "
            SQL += "       TAC2.TACO_COCO TACO_CODB, MOCO.MOCO_CODI MOCO_DEBI, "
            SQL += "       MOCO.TIMO_CODI TIMO_DEBI, MOCO.MOCO_DOCU DOCU_DEBI, "
            SQL += "       NVL(MOCO.MOCO_DESC,'?') DESC_DEBI, MOC1.MOCO_CODI MOCO_CRED, "
            SQL += "       MOC1.TIMO_CODI TIMO_CRED, MOC1.MOCO_DOCU DOCU_CRED, "
            SQL += "       NVL(MOC1.MOCO_DESC,'?') DESC_CRED, MORE_DARE, MORE_VAPA AS TOTAL, MORE_DEAN, "
            SQL += "       TIMO_DEBI.TIMO_CDIA DEBI_CDIA, TIMO_DEBI.TIMO_CDOC DEBI_CDOC, "
            SQL += "       TIMO_CRED.TIMO_CDIA CRED_CDIA, TIMO_CRED.TIMO_CDOC CRED_CDOC, "
            SQL += "       MOCO.TACO_CODI TACO_DEBI, MOC1.TACO_CODI TACO_CRED ,"
            '  SQL += "       NVL(TIMO_CRED.TIMO_COCO,'0')  AS CUENTA "
            SQL += "        NVL(TAC1.TACO_CODA,'0') AS CUENTA ,"
            SQL += "       TAC1.TACO_CODI AS CODIGO, NVL(TAC1.TACO_NOME,'?') AS NOMBRE ,NVL(TAC1.TACO_NUCO,'?') AS NIF"

            SQL += "       ,NVL(TIMO_CRED.TIMO_PAGA,'0') AS TIPOPAGO , "
            SQL += "        NVL(MOLI_DESC,' ') AS MOLI_DESC"


            SQL += "  FROM TNCO_MORE, "
            SQL += "       TNCO_MOCO MOCO, "
            SQL += "       TNCO_MOCO MOC1, "
            SQL += "       TNCO_TACO TAC1, "
            SQL += "       TNCO_TACO TAC2, "
            SQL += "       TNCO_TIMO TIMO_DEBI, "
            SQL += "       TNCO_TIMO TIMO_CRED, "

            SQL += "       TNCO_MOLI "

            SQL += " WHERE TNCO_MORE.TACO_COD1 = TAC1.TACO_CODI "
            SQL += "   AND TNCO_MORE.TACO_COD2 = TAC2.TACO_CODI "
            SQL += "   AND TNCO_MORE.TACO_COD2 = MOCO.TACO_CODI "
            SQL += "   AND TNCO_MORE.MOCO_COD2 = MOCO.MOCO_CODI "
            SQL += "   AND MOCO.TIMO_CODI = TIMO_DEBI.TIMO_CODI "
            SQL += "   AND TNCO_MORE.TACO_COD1 = MOC1.TACO_CODI "
            SQL += "   AND TNCO_MORE.MOCO_COD1 = MOC1.MOCO_CODI "
            SQL += "   AND MOC1.TIMO_CODI = TIMO_CRED.TIMO_CODI "

            SQL += "     AND MOC1.TIMO_CODI = TNCO_MOLI.TIMO_CODI "
            SQL += "     AND TNCO_MOLI.LICL_CODI = 1 "

            SQL += "   AND TNCO_MORE.MORE_REGU = 1 "

            ' segun chany/edelia no se tratan notas de credito 
            SQL += "   AND MOC1.TIMO_CODI <> '" & Me.mCodigoNotasCredito & "'"

            SQL += "  AND MORE_DAAN = '" & Me.mFecha & "'"
            SQL += "  AND MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"


            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                Linea = Linea + 1



                If mOrigenCuentasNewConta = 1 Then
                    Cuenta = BuscaCuentaPagosAnticipadosCentral(CType(Me.DbNewConta.mDbLector("CODIGO"), String))
                Else
                    Cuenta = BuscaCuentaPagosAnticipadosNewHotel(CType(Me.DbNewConta.mDbLector("CODIGO"), String))
                End If




                ' Si el documento o parte del documento se saldo con una nota de credito el importe va negativo

                If CType(Me.DbNewConta.mDbLector("TIPOPAGO"), String) = "0" Then
                    EsFormaDePago = False
                Else
                    EsFormaDePago = True
                End If


                'If EsFormaDePago = True Then
                ' Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                'Else
                'Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double) * -1
                'End If

                ' Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double) * -1
                Total = Math.Round(CType(Me.DbNewConta.mDbLector("TOTAL"), Double), 2) * -1

                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 777, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbNewConta.mDbLector("DESC_CRED"), String), Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("CODIGO"), String) & " " & CType(Me.DbNewConta.mDbLector("NOMBRE"), String), "SI", CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String).PadRight(50, CChar(" ")) & " [Es Forma de PAgo = " & EsFormaDePago.ToString & "]", " Paga a : " & CType(Me.DbNewConta.mDbLector("DESC_DEBI"), String), "NULL", "")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, " " & CType(Me.DbNewConta.mDbLector("DESC_CRED"), String), Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try


    End Sub

#End Region
#Region "ASIENTO 444 OTROS MOVIMIENTOS de CREDITO"
    Private Sub NCOtrosMovimientosCredito()
        Try
            Dim Total As Double
            Dim Cuenta As String = " "

            Dim TipoMovimientoNotadeCredito As String


            SQL = "SELECT PACO_NCRE FROM TNCO_PACO"
            TipoMovimientoNotadeCredito = Me.DbNewContaAux.EjecutaSqlScalar(SQL)

            Linea = 0

            SQL = "SELECT  'MOVIMIENTOS DE CREDITO ' , 'DEBE',KEY_FIELD, VNCO_MOCO.HOTE_CODI,VNCO_MOCO.TACO_CODI,NVL(TNCO_TIMO.TIMO_COCO,'0') AS CUENTA, VNCO_MOCO.TIMO_CODI, "
            SQL += "         VNCO_MOCO.MOCO_DOCU, VNCO_MOCO.MOCO_DOC1, VNCO_MOCO.UNMO_CODI, "
            SQL += "         TRUNC (VNCO_MOCO.MOCO_DAVA) DAVA, VNCO_MOCO.MOCO_VAOR, "
            SQL += "         VNCO_MOCO.MOCO_CAMB, VNCO_MOCO.MOCO_DEBI, VNCO_MOCO.MOCO_CRED AS TOTAL, "
            SQL += "         VNCO_MOCO.MOCO_ANUL, NVL(VNCO_MOCO.TACO_NOME,' ') AS TACO_NOME, VNCO_MOCO.MOCO_RECI, "
            SQL += "         VNCO_MOCO.MOCO_CODI, NVL(VNCO_MOCO.MOCO_DESC,' ') AS MOCO_DESC, VNCO_MOCO.MOCO_EXTE, "
            SQL += "         VNCO_MOCO.MOCO_OBSE, VNCO_MOCO.DIFE_RATE, VNCO_MOCO.MOCO_DIFE, "
            SQL += "         ABS (VNCO_MOCO.MOCO_VADE) MOCO_VADE, VNCO_MOCO.PACO_PAAS, "
            SQL += "         VNCO_MOCO.TACO_ORIG, VNCO_MOCO.MOCO_DATR, VNCO_MOCO.HOTE_CODI, "
            SQL += "         HOTE_DESC, NVL(MOLI_DESC,' ') AS MOLI_DESC, VNCO_MOCO.FACT_CODI "
            SQL += "    FROM VNCO_MOCO, TNCO_HOTE, TNCO_UTIL, TNCO_MOLI,TNCO_TIMO "
            SQL += "   WHERE VNCO_MOCO.HOTE_CODI = TNCO_HOTE.HOTE_CODI "
            SQL += "     AND VNCO_MOCO.UTIL_CODI = TNCO_UTIL.UTIL_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "
            SQL += "     AND TNCO_MOLI.LICL_CODI = 1 "

            '    -- SOLO MOV DE CREDITO "
            SQL += "     AND VNCO_MOCO.MOCO_DEBI = 0 "
            '    -- NO ES DE TIPO PAGO "
            SQL += "     AND TNCO_TIMO.TIMO_PAGA = 0 "
            '    -- NO ES UNA NOTA DE CREDITO"

            SQL += " AND VNCO_MOCO.TIMO_CODI <> '" & TipoMovimientoNotadeCredito & "'"

            SQL += "     AND VNCO_MOCO.MOCO_DATR = '" & Me.mFecha & "'"

            SQL += "   AND VNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            SQL += "    ORDER BY VNCO_MOCO.MOCO_DAVA DESC, VNCO_MOCO.MOCO_ANUL "


            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                ' Localizar la Cuenta Cobtable de la forma de Cobro
                SQL = "SELECT NVL(TIMO_COCO,'0') FROM TNCO_TIMO WHERE TIMO_CODI = '" & CType(Me.DbNewConta.mDbLector("TIMO_CODI"), String) & "'"
                Cuenta = Me.DbNewContaAux.EjecutaSqlScalar(SQL)

                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 444, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " +  " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, "NO", "", CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", CDate(Me.DbNewConta.mDbLector("DAVA")), CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), "", "", "", "null", "N", "")
                    Me.GeneraFileACconFechaValor("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, CDate(Me.DbNewConta.mDbLector("DAVA")))
                End If
            End While
            Me.DbNewConta.mDbLector.Close()

            '' lo mismo al reves ( deposito anticipado ) 

            SQL = "SELECT  'MOVIMIENTOS DE CREDITO TIPO PAGO' , 'DEBE',KEY_FIELD, VNCO_MOCO.HOTE_CODI,VNCO_MOCO.TACO_CODI AS TACO_CODI ,NVL(TNCO_TACO.TACO_CODA,'0') AS CUENTA, VNCO_MOCO.TIMO_CODI, "
            SQL += "         VNCO_MOCO.MOCO_DOCU, VNCO_MOCO.MOCO_DOC1, VNCO_MOCO.UNMO_CODI, "
            SQL += "         TRUNC (VNCO_MOCO.MOCO_DAVA) DAVA, VNCO_MOCO.MOCO_VAOR, "
            SQL += "         VNCO_MOCO.MOCO_CAMB, VNCO_MOCO.MOCO_DEBI, VNCO_MOCO.MOCO_CRED AS TOTAL, "
            SQL += "         VNCO_MOCO.MOCO_ANUL, NVL(VNCO_MOCO.TACO_NOME,' ') AS TACO_NOME , VNCO_MOCO.MOCO_RECI, "
            SQL += "         VNCO_MOCO.MOCO_CODI, NVL(VNCO_MOCO.MOCO_DESC,' ') AS MOCO_DESC, VNCO_MOCO.MOCO_EXTE, "
            SQL += "         VNCO_MOCO.MOCO_OBSE, VNCO_MOCO.DIFE_RATE, VNCO_MOCO.MOCO_DIFE, "
            SQL += "         ABS (VNCO_MOCO.MOCO_VADE) MOCO_VADE, VNCO_MOCO.PACO_PAAS, "
            SQL += "         VNCO_MOCO.TACO_ORIG, VNCO_MOCO.MOCO_DATR, VNCO_MOCO.HOTE_CODI, "
            SQL += "         HOTE_DESC, NVL(MOLI_DESC,' ') AS MOLI_DESC, VNCO_MOCO.FACT_CODI , NVL(TNCO_TACO.TACO_NUCO,'?') AS NIF"
            SQL += "    FROM VNCO_MOCO, TNCO_HOTE, TNCO_UTIL, TNCO_MOLI,TNCO_TIMO,TNCO_TACO"
            SQL += "   WHERE VNCO_MOCO.HOTE_CODI = TNCO_HOTE.HOTE_CODI "
            SQL += "     AND VNCO_MOCO.UTIL_CODI = TNCO_UTIL.UTIL_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "
            SQL += "     AND VNCO_MOCO.TACO_CODI = TNCO_TACO.TACO_CODI "
            SQL += "     AND TNCO_MOLI.LICL_CODI = 1 "

            '    -- SOLO MOV DE CREDITO "
            SQL += "     AND VNCO_MOCO.MOCO_DEBI = 0 "
            '    -- NO ES DE TIPO PAGO "
            SQL += "     AND TNCO_TIMO.TIMO_PAGA = 0 "
            '    -- NO ES UNA NOTA DE CREDITO"

            SQL += " AND VNCO_MOCO.TIMO_CODI <> '" & TipoMovimientoNotadeCredito & "'"

            SQL += "     AND VNCO_MOCO.MOCO_DATR= '" & Me.mFecha & "'"

            SQL += "     AND VNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            SQL += "    ORDER BY VNCO_MOCO.MOCO_DAVA DESC, VNCO_MOCO.MOCO_ANUL "


            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                If mOrigenCuentasNewConta = 1 Then
                    Cuenta = BuscaCuentaPagosAnticipadosCentral(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))
                Else
                    Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))
                End If


                '   Cuenta = "Pendiente"


                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 444, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", CDate(Me.DbNewConta.mDbLector("DAVA")), CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), "", "", "", "null", "N", "")
                    Me.GeneraFileACconFechaValor("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, CDate(Me.DbNewConta.mDbLector("DAVA")))
                End If
            End While
            Me.DbNewConta.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try


    End Sub
#End Region
#Region "ASIENTO 555 OTROS MOVIMIENTOS de DEBITO QUE NO SON FACTURAS"
    Private Sub NCOtrosMovimientosDebito()
        Try
            Dim Total As Double
            Dim Cuenta As String = " "

            Dim TipoMovimientoFacturas As String


            SQL = "SELECT PACO_FACT FROM TNCO_PACO"
            TipoMovimientoFacturas = Me.DbNewContaAux.EjecutaSqlScalar(SQL)

            Linea = 0

            SQL = "SELECT  'MOVIMIENTOS DE CREDITO ' , 'DEBE',KEY_FIELD, VNCO_MOCO.HOTE_CODI,VNCO_MOCO.TACO_CODI,NVL(TNCO_TIMO.TIMO_COCO,'0') AS CUENTA, VNCO_MOCO.TIMO_CODI, "
            SQL += "         VNCO_MOCO.MOCO_DOCU, VNCO_MOCO.MOCO_DOC1, VNCO_MOCO.UNMO_CODI, "
            SQL += "         TRUNC (VNCO_MOCO.MOCO_DAVA) DAVA, VNCO_MOCO.MOCO_VAOR, "
            SQL += "         VNCO_MOCO.MOCO_CAMB, VNCO_MOCO.MOCO_DEBI, VNCO_MOCO.MOCO_DEBI AS TOTAL, "
            SQL += "         VNCO_MOCO.MOCO_ANUL, NVL(VNCO_MOCO.TACO_NOME,' ') AS TACO_NOME, VNCO_MOCO.MOCO_RECI, "
            SQL += "         VNCO_MOCO.MOCO_CODI, NVL(VNCO_MOCO.MOCO_DESC,' ') AS MOCO_DESC, VNCO_MOCO.MOCO_EXTE, "
            SQL += "         VNCO_MOCO.MOCO_OBSE, VNCO_MOCO.DIFE_RATE, VNCO_MOCO.MOCO_DIFE, "
            SQL += "         ABS (VNCO_MOCO.MOCO_VADE) MOCO_VADE, VNCO_MOCO.PACO_PAAS, "
            SQL += "         VNCO_MOCO.TACO_ORIG, VNCO_MOCO.MOCO_DATR, VNCO_MOCO.HOTE_CODI, "
            SQL += "         HOTE_DESC, NVL(MOLI_DESC,' ') AS MOLI_DESC, VNCO_MOCO.FACT_CODI "
            SQL += "    FROM VNCO_MOCO, TNCO_HOTE, TNCO_UTIL, TNCO_MOLI,TNCO_TIMO "
            SQL += "   WHERE VNCO_MOCO.HOTE_CODI = TNCO_HOTE.HOTE_CODI "
            SQL += "     AND VNCO_MOCO.UTIL_CODI = TNCO_UTIL.UTIL_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "
            SQL += "     AND TNCO_MOLI.LICL_CODI = 1 "

            '    -- SOLO MOV DE DEBITO "
            SQL += "     AND VNCO_MOCO.MOCO_CRED = 0 "
            '    -- NO ES DE TIPO PAGO "
            SQL += "     AND TNCO_TIMO.TIMO_PAGA = 0 "
            '    -- NO ES UNA NOTA DE CREDITO"

            SQL += " AND VNCO_MOCO.TIMO_CODI <> '" & TipoMovimientoFacturas & "'"

            SQL += "     AND VNCO_MOCO.MOCO_DATR = '" & Me.mFecha & "'"

            SQL += "   AND VNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            SQL += "    ORDER BY VNCO_MOCO.MOCO_DAVA DESC, VNCO_MOCO.MOCO_ANUL "


            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                ' Localizar la Cuenta Cobtable de la forma de Cobro
                SQL = "SELECT NVL(TIMO_COCO,'0') FROM TNCO_TIMO WHERE TIMO_CODI = '" & CType(Me.DbNewConta.mDbLector("TIMO_CODI"), String) & "'"
                Cuenta = Me.DbNewContaAux.EjecutaSqlScalar(SQL)

                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 555, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " +  " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, "NO", "", CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", CDate(Me.DbNewConta.mDbLector("DAVA")), CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), "", "", "", "null", "N", "")
                    Me.GeneraFileACconFechaValor("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, CDate(Me.DbNewConta.mDbLector("DAVA")))
                End If
            End While
            Me.DbNewConta.mDbLector.Close()

            '' lo mismo al reves (  ) 

            SQL = "SELECT  'MOVIMIENTOS DE CREDITO TIPO PAGO' , 'DEBE',KEY_FIELD, VNCO_MOCO.HOTE_CODI,VNCO_MOCO.TACO_CODI AS TACO_CODI ,NVL(TNCO_TACO.TACO_CODA,'0') AS CUENTA, VNCO_MOCO.TIMO_CODI, "
            SQL += "         VNCO_MOCO.MOCO_DOCU, VNCO_MOCO.MOCO_DOC1, VNCO_MOCO.UNMO_CODI, "
            SQL += "         TRUNC (VNCO_MOCO.MOCO_DAVA) DAVA, VNCO_MOCO.MOCO_VAOR, "
            SQL += "         VNCO_MOCO.MOCO_CAMB, VNCO_MOCO.MOCO_DEBI, VNCO_MOCO.MOCO_DEBI AS TOTAL, "
            SQL += "         VNCO_MOCO.MOCO_ANUL, NVL(VNCO_MOCO.TACO_NOME,' ') AS TACO_NOME , VNCO_MOCO.MOCO_RECI, "
            SQL += "         VNCO_MOCO.MOCO_CODI, NVL(VNCO_MOCO.MOCO_DESC,' ') AS MOCO_DESC, VNCO_MOCO.MOCO_EXTE, "
            SQL += "         VNCO_MOCO.MOCO_OBSE, VNCO_MOCO.DIFE_RATE, VNCO_MOCO.MOCO_DIFE, "
            SQL += "         ABS (VNCO_MOCO.MOCO_VADE) MOCO_VADE, VNCO_MOCO.PACO_PAAS, "
            SQL += "         VNCO_MOCO.TACO_ORIG, VNCO_MOCO.MOCO_DATR, VNCO_MOCO.HOTE_CODI, "
            SQL += "         HOTE_DESC, NVL(MOLI_DESC,' ') AS MOLI_DESC, VNCO_MOCO.FACT_CODI , NVL(TNCO_TACO.TACO_NUCO,'?') AS NIF"
            SQL += "    FROM VNCO_MOCO, TNCO_HOTE, TNCO_UTIL, TNCO_MOLI,TNCO_TIMO,TNCO_TACO"
            SQL += "   WHERE VNCO_MOCO.HOTE_CODI = TNCO_HOTE.HOTE_CODI "
            SQL += "     AND VNCO_MOCO.UTIL_CODI = TNCO_UTIL.UTIL_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "
            SQL += "     AND VNCO_MOCO.TACO_CODI = TNCO_TACO.TACO_CODI "
            SQL += "     AND TNCO_MOLI.LICL_CODI = 1 "

            '    -- SOLO MOV DE DEBITO "
            SQL += "     AND VNCO_MOCO.MOCO_CRED = 0 "
            '    -- NO ES DE TIPO PAGO "
            SQL += "     AND TNCO_TIMO.TIMO_PAGA = 0 "
            '    -- NO ES UNA NOTA DE CREDITO"

            SQL += " AND VNCO_MOCO.TIMO_CODI <> '" & TipoMovimientoFacturas & "'"

            SQL += "     AND VNCO_MOCO.MOCO_DATR= '" & Me.mFecha & "'"

            SQL += "     AND VNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            SQL += "    ORDER BY VNCO_MOCO.MOCO_DAVA DESC, VNCO_MOCO.MOCO_ANUL "


            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                Cuenta = BuscaCuentaClienteCentral(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))


                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 555, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, "*** " & CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", CDate(Me.DbNewConta.mDbLector("DAVA")), CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), "", "", "", "null", "N", "")
                    Me.GeneraFileACconFechaValor("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, " " & CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), Total, CDate(Me.DbNewConta.mDbLector("DAVA")))
                End If
            End While
            Me.DbNewConta.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try


    End Sub
#End Region
#End Region

#End Region
#Region "METODOS PUBLICOS"
    Public Sub Procesar()
        Try

            ' MsgBox("Ojo revisar  COMISION de visas , en depositos antincipados de agencias si los hubiera ", MsgBoxStyle.Exclamation, "Atención")
            If Me.FileEstaOk = False Then Exit Sub




            ' ---------------------------------------------------------------
            ' Asiento Facturacion total del dia 
            '----------------------------------------------------------------
            If Me.DbNewConta.EstadoConexion = ConnectionState.Open Then
                Me.mTextDebug.Text = "Calculando Pagos Recibidos"
                Me.mTextDebug.Update()

                Me.NCPagosRecibidos()

                Me.mProgress.Value = 10
                Me.mProgress.Update()

                Me.mTextDebug.Text = "Calculando Facturas Regularizadas"
                Me.mTextDebug.Update()

                Me.NCFacturasRegularizadas()

                Me.mProgress.Value = 20
                Me.mProgress.Update()


                If Me.mDebug = True Then
                    Me.mTextDebug.Text = "Calculando Facturas Des-Regularizadas por Pagos anulados"
                    Me.mTextDebug.Update()

                    Me.NCFacturasRegularizadasAnuladas()

                    Me.mProgress.Value = 30
                    Me.mProgress.Update()
                End If



                Me.mTextDebug.Text = "Calculando Pagos de Más"
                Me.mTextDebug.Update()

                Me.NCPagosDeMas()

                Me.mProgress.Value = 40
                Me.mProgress.Update()


                If Me.mOtrosCreditos = True Then
                    Me.mTextDebug.Text = "Calculando Otros Movimientos"
                    Me.mTextDebug.Update()

                    Me.NCOtrosMovimientosCredito()

                    Me.mProgress.Value = 50
                    Me.mProgress.Update()
                End If



                If Me.mOtrosDebitos = True Then
                    Me.mTextDebug.Text = "Calculando Otros Movimientos"
                    Me.mTextDebug.Update()

                    Me.NCOtrosMovimientosDebito()

                    Me.mProgress.Value = 60
                    Me.mProgress.Update()

                End If


            End If


            ' VALIDACION DE CUENTAS EB SPYRO TODAS JUNTAS AL FINAL

            '   MsgBox("SE VALIDAN CUENTAS AL FINAL")



            'FASE 2 2016 COMPROBANTES BANCARIOS 
            '  Me.SpyroCompruebaCuentas()
            Me.SpyroCompruebaCuentasCorto()
            Me.SpyroCompruebaBancos()




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


            SQL = "SELECT ROUND(SUM(round(NVL(ASNT_DEBE,'0'),2)),2) FROM TC_ASNT WHERE ASNT_F_ATOCAB = '" & Me.mFecha & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum
            '      SQL += " AND ASNT_IMPRIMIR = 'SI'"



            If IsNumeric(Me.DbLeeCentral.EjecutaSqlScalar(SQL)) Then
                TotalDebe = CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Decimal)
            Else
                TotalDebe = 0
            End If


            SQL = "SELECT ROUND(SUM(round(NVL(ASNT_HABER,'0'),2)),2) FROM TC_ASNT WHERE ASNT_F_ATOCAB = '" & Me.mFecha & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum
            '   SQL += " AND ASNT_IMPRIMIR = 'SI'"
            If IsNumeric(Me.DbLeeCentral.EjecutaSqlScalar(SQL)) Then
                TotalHaber = CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Decimal)
            Else
                TotalHaber = 0
            End If




            If TotalHaber > TotalDebe Then
                TotalDiferencia = TotalHaber - TotalDebe
                MsgBox("Se va ha producir un Ajuste Decimal  de " & TotalDiferencia & "  " & vbCrLf & vbCrLf & "No Integre con valores superiores a 0.05", MsgBoxStyle.Information, "Atención")
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 999, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaRedondeo, Me.mIndicadorDebe, "AJUSTE REDONDEO", TotalDiferencia, "SI", "", "", "SI", "Ajuste Decimales")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaRedondeo, Me.mIndicadorDebe, "AJUSTE REDONDEO", TotalDiferencia)
            End If

            If TotalHaber < TotalDebe Then
                TotalDiferencia = TotalDebe - TotalHaber
                MsgBox("Se va ha producir un Ajuste Decimal  de " & TotalDiferencia & "  " & vbCrLf & vbCrLf & "No Integre con valores superiores a 0.05", MsgBoxStyle.Information, "Atención")
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 999, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaRedondeo, Me.mIndicadorHaber, "AJUSTE REDONDEO", TotalDiferencia, "SI", "", "", "SI", "Ajuste Decimales")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaRedondeo, Me.mIndicadorHaber, "AJUSTE REDONDEO", TotalDiferencia)
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

#End Region
#Region "RUTINAS PRIVADAS"
    Private Sub GeneraComprobanteBanco(vNumAsiento As Integer, vTotal As Double, vDescripcion As String, vBancosCod As String, vTacoCodi As String, vMocoCodi As Integer)
        Try
            SQL = "SELECT TH_COMPROBANTES.NEXTVAL FROM DUAL"
            Me.mTransferenciaComprobante = CInt(Me.DbLeeCentral.EjecutaSqlScalar2(SQL))

            SQL = "SELECT TH_FACTURAS.NEXTVAL FROM DUAL"
            Me.mTransferenciaFactura = CInt(Me.DbLeeCentral.EjecutaSqlScalar2(SQL))

            SQL = "SELECT NVL(PARA_FACTUTIPO_COD,'?')  "
            SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
            Me.mTransferenciaFacturaSerie = CStr(Me.DbLeeCentral.EjecutaSqlScalar(SQL))



            SQL = "SELECT NVL(PARA_CFBCOTMOV_COD2,'?')  "
            SQL += " FROM TC_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
            Me.mTransferenciaCfbcotmov = CStr(Me.DbLeeCentral.EjecutaSqlScalar(SQL))



            SQL = "SELECT NVL(PARA_FPAGO_COD2,'?')  "
            SQL += " FROM TC_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
            Me.mTransferenciaFPagoCod = CStr(Me.DbLeeCentral.EjecutaSqlScalar(SQL))


            ' Control tabla de Cobros/Comprobantes TC_COMP

            SQL = "SELECT COUNT(*) AS TOTAL  "
            SQL += " FROM TC_COMP WHERE TACO_CODI = '" & vTacoCodi & "'"
            SQL += " AND MOCO_CODI = " & vMocoCodi

            Me.mResultInt = CInt(Me.DbLeeCentral.EjecutaSqlScalar(SQL))

            If Me.mResultInt = 0 Then
                SQL = "INSERT INTO TC_COMP (TACO_CODI,MOCO_CODI,COMP_NUME,TIMO_CECO) VALUES ("
                SQL += "'" & vTacoCodi & "'," & vMocoCodi & "," & Me.mTransferenciaComprobante & ",'" & vBancosCod & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Else
                SQL = "UPDATE  TC_COMP "
                SQL += "SET COMP_NUME = " & Me.mTransferenciaComprobante
                SQL += ", TIMO_CECO = '" & vBancosCod & "'"
                SQL += " WHERE TACO_CODI = '" & vTacoCodi & "'"
                SQL += " AND MOCO_CODI = " & vMocoCodi
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            End If





            'Dim Cuenta As String = Me.mCtaClientesContado
            'Cuenta = Mid(Cuenta, 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(Cuenta, 5, 6)

            ' Busca Cuenta Cliente 430
            Dim Cuenta As String
            Dim Cif As String
            If mOrigenCuentasNewConta = 1 Then
                Cuenta = BuscaCuentaClienteCentral(vTacoCodi)
                Cif = BuscaCifClienteCentral(vTacoCodi)
            Else
                Cuenta = BuscaCuentaClienteNewHotel(vTacoCodi)
                Cif = BuscaCifClienteNewHotel(vTacoCodi)
            End If


            Me.GeneraFileFVDiariodeCobros("FV", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, Me.mTransferenciaFacturaSerie, Me.mTransferenciaFactura, vTotal, Me.mTransferenciaFactura & "/" & Me.mTransferenciaFacturaSerie, Cuenta, Cif, 0, Me.mTransferenciaFPagoCod)
            Me.GeneraFileCB("CB", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, Me.mTransferenciaFacturaSerie, Me.mTransferenciaFactura, vTotal, "", "", "", vTotal, vBancosCod, Me.mTransferenciaCfbcotmov, Me.mTransferenciaComprobante, "N")
            Me.GeneraFileMG("MG", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, Me.mTransferenciaFacturaSerie, Me.mTransferenciaFactura, vTotal, "", "", "", 0, Me.mTransferenciaCfbcotmov, vBancosCod, CStr(Me.mTransferenciaComprobante))


        Catch ex As Exception

        End Try

    End Sub
    Private Sub GeneraComprobanteBanco2(vNumAsiento As Integer, vTotal As Double, vBancosCod As String, vTacoCodi As String, vMocoCodi As Integer, vFactura As Integer, vSerie As String)
        Try


            SQL = "SELECT TH_COMPROBANTES.NEXTVAL FROM DUAL"
            Me.mTransferenciaComprobante = CInt(Me.DbLeeCentral.EjecutaSqlScalar2(SQL))

            SQL = "SELECT TH_FACTURAS.NEXTVAL FROM DUAL"
            Me.mTransferenciaFactura = CInt(Me.DbLeeCentral.EjecutaSqlScalar2(SQL))

            SQL = "SELECT NVL(PARA_FACTUTIPO_COD,'?')  "
            SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
            Me.mTransferenciaFacturaSerie = CStr(Me.DbLeeCentral.EjecutaSqlScalar(SQL))



            '  SQL = "SELECT NVL(PARA_CFBCOTMOV_COD2,'?')  "
            SQL = "SELECT NVL(PARA_CFBCOTMOV_COD,'?')  "
            SQL += " FROM TC_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
            Me.mTransferenciaCfbcotmov = CStr(Me.DbLeeCentral.EjecutaSqlScalar(SQL))



            SQL = "SELECT NVL(PARA_FPAGO_COD2,'?')  "
            SQL += " FROM TC_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
            Me.mTransferenciaFPagoCod = CStr(Me.DbLeeCentral.EjecutaSqlScalar(SQL))


            ' BANCO NOTIFICACION
            SQL = "SELECT NVL(PARA_BANCOS_COD,'?')  "
            SQL += " FROM TC_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
            Me.mTransferenciaBancosCod = CStr(Me.DbLeeCentral.EjecutaSqlScalar(SQL))



            ' Control tabla de Cobros/Comprobantes TC_COMP

            SQL = "SELECT COUNT(*) AS TOTAL  "
            SQL += " FROM TC_COMP WHERE TACO_CODI = '" & vTacoCodi & "'"
            SQL += " AND MOCO_CODI = " & vMocoCodi

            Me.mResultInt = CInt(Me.DbLeeCentral.EjecutaSqlScalar(SQL))

            If Me.mResultInt = 0 Then
                SQL = "INSERT INTO TC_COMP (TACO_CODI,MOCO_CODI,COMP_NUME,TIMO_CECO) VALUES ("
                SQL += "'" & vTacoCodi & "'," & vMocoCodi & "," & Me.mTransferenciaComprobante & ",'" & vBancosCod & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Else
                SQL = "UPDATE  TC_COMP "
                SQL += "SET COMP_NUME = " & Me.mTransferenciaComprobante
                SQL += ", TIMO_CECO = '" & vBancosCod & "'"
                SQL += " WHERE TACO_CODI = '" & vTacoCodi & "'"
                SQL += " AND MOCO_CODI = " & vMocoCodi
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            End If







            ' Busca Cuenta Cliente 430
            Dim CuentaCliente As String
            Dim CifCliente As String
            If mOrigenCuentasNewConta = 1 Then
                CuentaCliente = BuscaCuentaClienteCentral(vTacoCodi)
                CifCliente = BuscaCifClienteCentral(vTacoCodi)
            Else
                CuentaCliente = BuscaCuentaClienteNewHotel(vTacoCodi)
                CifCliente = BuscaCifClienteNewHotel(vTacoCodi)
            End If


            ' Busca Cuenta Anticipos del Cliente 438

            Dim CuentaAnticipos As String
            Dim CifAnticipos As String
            If mOrigenCuentasNewConta = 1 Then
                CuentaAnticipos = BuscaCuentaPagosAnticipadosCentral(vTacoCodi)
                CifAnticipos = BuscaCifClienteCentral(vTacoCodi)
            Else
                CuentaAnticipos = BuscaCuentaPagosAnticipadosNewHotel(vTacoCodi)
                CifAnticipos = BuscaCifClienteNewHotel(vTacoCodi)
            End If



            Me.GeneraFileFVDiariodeCobros("FV", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, Me.mTransferenciaFacturaSerie, Me.mTransferenciaFactura, vTotal, Me.mTransferenciaFactura & "/" & Me.mTransferenciaFacturaSerie, CuentaCliente, CifCliente, 0, Me.mTransferenciaFPagoCod)

            '  Me.GeneraFileVV("VV", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, Me.mTransferenciaFacturaSerie, Me.mTransferenciaFactura, vTotal, "", CuentaCliente, CifCliente, 0, Me.mTransferenciaCfbcotmov, vBancosCod, CStr(Me.mTransferenciaComprobante), 1, "S")
            Me.GeneraFileVV("VV", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, Me.mTransferenciaFacturaSerie, Me.mTransferenciaFactura, vTotal, "", CuentaCliente, CifCliente, vTotal, Me.mTransferenciaCfbcotmov, vBancosCod, CStr(Me.mTransferenciaComprobante), 1, "S")
            Me.GeneraFileVV("VV", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, Me.mTransferenciaFacturaSerie, Me.mTransferenciaFactura, vTotal, "", CuentaAnticipos, CifAnticipos, vTotal, Me.mTransferenciaCfbcotmov, vBancosCod, CStr(Me.mTransferenciaComprobante), 2, "N")


            Me.GeneraFileCB("CB", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, Me.mTransferenciaFacturaSerie, Me.mTransferenciaFactura, vTotal, "", "", "", vTotal, Me.mTransferenciaBancosCod, Me.mTransferenciaCfbcotmov, Me.mTransferenciaComprobante, "N")
            Me.GeneraFileMG("MG", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, Me.mTransferenciaFacturaSerie, Me.mTransferenciaFactura, vTotal, "", "", "", 0, Me.mTransferenciaCfbcotmov, Me.mTransferenciaBancosCod, CStr(Me.mTransferenciaComprobante))


        Catch ex As Exception

        End Try

    End Sub
    Private Sub GeneraComprobanteBancoRegularizacion(vNumAsiento As Integer, vTotal As Double, vTacoCodi As String, vMocoCodi As Integer, vFactura As Integer, vSerie As String)
        Try


            SQL = "SELECT NVL(PARA_CFBCOTMOV_COD2,'?')  "
            SQL += " FROM TC_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
            Me.mTransferenciaCfbcotmov = CStr(Me.DbLeeCentral.EjecutaSqlScalar(SQL))




            ' Busca Comprobante de Cobro en  TC_COMP

            SQL = "SELECT COMP_NUME  "
            SQL += " FROM TC_COMP WHERE TACO_CODI = '" & vTacoCodi & "'"
            SQL += " AND MOCO_CODI = " & vMocoCodi

            Me.mResultStr = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

            If IsNothing(Me.mResultStr) = False Then
                Me.mComprobante = CInt(Me.mResultStr)
            Else
                Me.mComprobante = 0
            End If


            ' Busca Banco del Comprobante de Cobro en  TC_COMP

            SQL = "SELECT TIMO_CECO  "
            SQL += " FROM TC_COMP WHERE TACO_CODI = '" & vTacoCodi & "'"
            SQL += " AND MOCO_CODI = " & vMocoCodi

            Me.mResultStr = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

            If IsNothing(Me.mResultStr) = False Then
                Me.mBanco = Me.mResultStr
            Else
                Me.mBanco = ""
            End If





            'Dim Cuenta As String = Me.mCtaClientesContado
            'Cuenta = Mid(Cuenta, 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(Cuenta, 5, 6)

            ' Busca Cuenta Cliente 430
            Dim Cuenta As String
            Dim Cif As String
            If mOrigenCuentasNewConta = 1 Then
                Cuenta = BuscaCuentaClienteCentral(vTacoCodi)
                Cif = BuscaCifClienteCentral(vTacoCodi)
            Else
                Cuenta = BuscaCuentaClienteNewHotel(vTacoCodi)
                Cif = BuscaCifClienteNewHotel(vTacoCodi)
            End If


            '      Me.GeneraFileFVDiariodeCobros("FV", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, Me.mTransferenciaFacturaSerie, Me.mTransferenciaFactura, vTotal, Me.mTransferenciaFactura & "/" & Me.mTransferenciaFacturaSerie, Cuenta, Cif, 0, Me.mTransferenciaFPagoCod)
            Me.GeneraFileCB("CB", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, vSerie, vFactura, vTotal, "", "", "", vTotal, Me.mBanco, Me.mTransferenciaCfbcotmov, Me.mComprobante, "N")
            Me.GeneraFileMG("MG", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, vSerie, vFactura, vTotal, "", "", "", 0, Me.mTransferenciaCfbcotmov, Me.mBanco, CStr(Me.mComprobante))


        Catch ex As Exception

        End Try

    End Sub
    ''' <summary>
    ''' OJO ESTAS SQLS USAN EL USUARIO NCC. A TIMBAQUE  REVISAR DESDE QUE SE PUEDA 
    ''' </summary>
    ''' <param name="vTacoCodi"></param>
    ''' <returns></returns>
    Private Function BuscaCuentaPagosAnticipadosCentral(ByVal vTacoCodi As String) As String


        Dim Cuenta As String = " "
        Dim Control As Integer = 0
        Dim ControlCuenta As String = " "
        Dim Primerregistro As Boolean = True
        Dim Texto As String = " "

        Dim Avisa As Boolean = False
        Dim Entidades As String = ""



        Try


            ' Localizar la Cuenta Cobtable de la Pago Anticipado entidad
            ' Ojo esta qwery trata de buscar la cuenta contable de la entidad de la central y puede devolver varios registros
            ' si hay varias entidades con el mismo codigo de newconta y distinta cuenta contable


            SQL = "SELECT DISTINCT OPER_NECO,TNCC_ENHO.ENTI_CODI, NVL (ENTI_DEAN_AF, '?') AS ENTI_DEAN_AF"
            SQL += " FROM NCC.TNCC_OPER, NCC.TNCC_ENHO "
            SQL += " WHERE TNCC_OPER.ENTI_CODI = TNCC_ENHO.ENTI_CODI "
            SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
            '    SQL += " AND TNCC_ENHO.HOTE_CODI <> 1 "
            SQL += " AND TNCC_ENHO.HOTE_CODI = " & Me.mHoteCodiNewCentral

            SQL += " GROUP BY TNCC_ENHO.ENTI_CODI,OPER_NECO,ENTI_DEAN_AF "

            Me.DbNewContaAux.TraerLector(SQL)
            While Me.DbNewContaAux.mDbLector.Read
                If Primerregistro = True Then
                    Primerregistro = False
                    ControlCuenta = CType(Me.DbNewContaAux.mDbLector("ENTI_DEAN_AF"), String)
                End If

                Entidades += " " & CType(Me.DbNewContaAux.mDbLector("ENTI_CODI"), String)

                If CType(Me.DbNewContaAux.mDbLector("ENTI_DEAN_AF"), String) <> ControlCuenta Then

                    Texto = "Más de Un ttoo de NewCentral usa la misma Cuenta : " & vTacoCodi & " para la Gestión de Cobros" & vbCrLf
                    Texto += " Sin embargo alguno de ellos NO tiene o Difiere en la Cuenta de Depósitos Anticipados"
                    Texto += vbCrLf & vbCrLf
                    Texto += " Revise También que la Cuenta dentro de Un Mismo TTOO sea la misma para TODOS los Hoteles "
                    '  MsgBox(Texto & vbCrLf & "Entidades = " & Entidades, MsgBoxStyle.Information, "Atención")
                    Cuenta = "0"
                    Avisa = True
                End If

            End While
            Me.DbNewContaAux.mDbLector.Close()



            If Avisa = False Then
                SQL = "SELECT DISTINCT NVL(ENTI_DEAN_AF,'?') FROM NCC.TNCC_OPER,NCC.TNCC_ENHO WHERE "
                SQL += "  TNCC_OPER.ENTI_CODI = TNCC_ENHO.ENTI_CODI"
                SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
                '  SQL += " AND TNCC_ENHO.HOTE_CODI <> 1 "
                SQL += " AND TNCC_ENHO.HOTE_CODI = " & Me.mHoteCodiNewCentral
                SQL += " AND ENTI_DEAN_AF IS NOT NULL"
                Cuenta = Me.DbNewContaAux.EjecutaSqlScalar(SQL)
            Else
                MsgBox("Entidades a revisar Cuenta Pagos Anticipados NewConta = " & vTacoCodi & vbCrLf & vbCrLf & "NewCentral  = " & Entidades)
            End If


            ' compone 5 y 6 digito cuenta de cliente 
            Cuenta = Mid(Cuenta, 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(Cuenta, 5, 6)

            Return Cuenta

        Catch ex As Exception
            MsgBox(ex.Message)
            Return "0"
        End Try

    End Function
    Private Function BuscaCuentaPagosAnticipadosNewHotel(ByVal vTacoCodi As String) As String


        Dim Cuenta As String = " "
        Dim Control As Integer = 0
        Dim ControlCuenta As String = " "
        Dim Primerregistro As Boolean = True
        Dim Texto As String = " "

        Dim Avisa As Boolean = False
        Dim Entidades As String = ""


        Try


            ' CHAPUZA LOPEZ
            ' SI ES EL SAHARA PLAYA Y SI EL CODIGO NEWCONTA ES > 8999 BUSCA LAS CUENTAS EN EL ESQUEMA DE MEDIAS PENSIONES RMC2
            If Me.mEmpCod = "13" And CInt(vTacoCodi) > 8999 Then
                Return Me.BuscaCuentaPagosAnticipadosNewHotel2(vTacoCodi)
            End If


            ' Localizar la Cuenta Cobtable de la Pago Anticipado entidad
            ' Ojo esta qwery trata de buscar la cuenta contable de la entidad de la central y puede devolver varios registros
            ' si hay varias entidades con el mismo codigo de newconta y distinta cuenta contable


            SQL = "SELECT DISTINCT OPER_NECO,TNHT_ENTI.ENTI_CODI, NVL (ENTI_DEAN_AF, '0') AS ENTI_DEAN_AF"
            SQL += " FROM TNHT_OPER, TNHT_ENTI "
            SQL += " WHERE TNHT_OPER.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
            SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
            SQL += " GROUP BY TNHT_ENTI.ENTI_CODI,OPER_NECO,ENTI_DEAN_AF "

            Me.DbLeeNewHotel.TraerLector(SQL)
            While Me.DbLeeNewHotel.mDbLector.Read
                If Primerregistro = True Then
                    Primerregistro = False
                    ControlCuenta = CType(Me.DbLeeNewHotel.mDbLector("ENTI_DEAN_AF"), String)
                End If

                Entidades += " " & CType(Me.DbLeeNewHotel.mDbLector("ENTI_CODI"), String)

                If CType(Me.DbLeeNewHotel.mDbLector("ENTI_DEAN_AF"), String) <> ControlCuenta Then

                    Texto = "Más de Un ttoo de NewCentral usa la misma Cuenta : " & vTacoCodi & " para la Gestión de Cobros" & vbCrLf
                    Texto += " Sin embargo alguno de ellos NO tiene o Difiere en la Cuenta de Depósitos Anticipados"
                    Texto += vbCrLf & vbCrLf
                    Texto += " Revise También que la Cuenta dentro de Un Mismo TTOO sea la misma para TODOS los Hoteles "
                    '   MsgBox(Texto & vbCrLf & "Entidades = " & Entidades, MsgBoxStyle.Information, "Atención")
                    Cuenta = "0"
                    Avisa = True
                End If

            End While
            Me.DbLeeNewHotel.mDbLector.Close()


            ' si solo hay una cuenta la toma 
            If Avisa = False Then
                SQL = "SELECT DISTINCT NVL(ENTI_DEAN_AF,'0') FROM TNHT_OPER,TNHT_ENTI WHERE "
                SQL += "  TNHT_OPER.ENTI_CODI = TNHT_ENTI.ENTI_CODI"
                SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
                SQL += " AND ENTI_DEAN_AF IS NOT NULL"
                Cuenta = Me.DbLeeNewHotel.EjecutaSqlScalar(SQL)
            Else
                MsgBox("Entidades a revisar  Cuenta Pagos Anticipados NewConta = " & vTacoCodi & vbCrLf & vbCrLf & "NewHotel = " & Entidades)
            End If



            ' compone 5 y 6 digito cuenta de cliente 
            Cuenta = Mid(Cuenta, 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(Cuenta, 5, 6)

            Return Cuenta

        Catch ex As Exception
            MsgBox(ex.Message)
            Return "0"
        End Try

    End Function
    Private Function BuscaCuentaPagosAnticipadosNewHotel2(ByVal vTacoCodi As String) As String


        Dim Cuenta As String = " "
        Dim Control As Integer = 0
        Dim ControlCuenta As String = " "
        Dim Primerregistro As Boolean = True
        Dim Texto As String = " "

        Dim Avisa As Boolean = False
        Dim Entidades As String = ""


        Try



            ' Localizar la Cuenta Cobtable de la Pago Anticipado entidad
            ' Ojo esta qwery trata de buscar la cuenta contable de la entidad de la central y puede devolver varios registros
            ' si hay varias entidades con el mismo codigo de newconta y distinta cuenta contable


            SQL = "SELECT DISTINCT OPER_NECO,TNHT_ENTI.ENTI_CODI, NVL (ENTI_DEAN_AF, '0') AS ENTI_DEAN_AF"
            SQL += " FROM TNHT_OPER, TNHT_ENTI "
            SQL += " WHERE TNHT_OPER.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
            SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
            SQL += " GROUP BY TNHT_ENTI.ENTI_CODI,OPER_NECO,ENTI_DEAN_AF "


            '  MsgBox(Me.DbLeeNewHotel2.StrConexion)
            ' MsgBox(SQL)


            Me.DbLeeNewHotel2.TraerLector(SQL)
            While Me.DbLeeNewHotel2.mDbLector.Read
                If Primerregistro = True Then
                    Primerregistro = False
                    ControlCuenta = CType(Me.DbLeeNewHotel2.mDbLector("ENTI_DEAN_AF"), String)
                End If

                Entidades += " " & CType(Me.DbLeeNewHotel2.mDbLector("ENTI_CODI"), String)

                If CType(Me.DbLeeNewHotel2.mDbLector("ENTI_DEAN_AF"), String) <> ControlCuenta Then

                    Texto = "Más de Un ttoo de NewCentral usa la misma Cuenta : " & vTacoCodi & " para la Gestión de Cobros" & vbCrLf
                    Texto += " Sin embargo alguno de ellos NO tiene o Difiere en la Cuenta de Depósitos Anticipados"
                    Texto += vbCrLf & vbCrLf
                    Texto += " Revise También que la Cuenta dentro de Un Mismo TTOO sea la misma para TODOS los Hoteles "
                    '   MsgBox(Texto & vbCrLf & "Entidades = " & Entidades, MsgBoxStyle.Information, "Atención")
                    Cuenta = "0"
                    Avisa = True
                End If

            End While
            Me.DbLeeNewHotel2.mDbLector.Close()


            ' si solo hay una cuenta la toma 
            If Avisa = False Then
                SQL = "SELECT DISTINCT NVL(ENTI_DEAN_AF,'0') FROM TNHT_OPER,TNHT_ENTI WHERE "
                SQL += "  TNHT_OPER.ENTI_CODI = TNHT_ENTI.ENTI_CODI"
                SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
                SQL += " AND ENTI_DEAN_AF IS NOT NULL"
                Cuenta = Me.DbLeeNewHotel2.EjecutaSqlScalar(SQL)
            Else
                MsgBox("Entidades a revisar  Cuenta Pagos Anticipados NewConta = " & vTacoCodi & vbCrLf & vbCrLf & "NewHotel = " & Entidades)
            End If



            ' compone 5 y 6 digito cuenta de cliente 
            Cuenta = Mid(Cuenta, 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(Cuenta, 5, 6)

            Return Cuenta

        Catch ex As Exception
            MsgBox(ex.Message)
            Return "0"
        End Try

    End Function


    Private Function BuscaCuentaClienteCentral(ByVal vTacoCodi As String) As String


        Dim Cuenta As String = " "
        Dim Control As Integer = 0
        Dim ControlCuenta As String = " "
        Dim Primerregistro As Boolean = True
        Dim Texto As String = " "

        Dim Avisa As Boolean = False
        Dim Entidades As String = ""



        Try



            ' Localizar la Cuenta Cobtable de la Pago Anticipado entidad
            ' Ojo esta qwery trata de buscar la cuenta contable de la entidad de la central y puede devolver varios registros
            ' si hay varias entidades con el mismo codigo de newconta y distinta cuenta contable


            SQL = "SELECT DISTINCT OPER_NECO,TNCC_ENHO.ENTI_CODI, NVL (ENTI_NCON_AF, '?') AS ENTI_NCON_AF"
            SQL += " FROM NCC.TNCC_OPER, NCC.TNCC_ENHO "
            SQL += " WHERE TNCC_OPER.ENTI_CODI = TNCC_ENHO.ENTI_CODI "
            SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
            ' SQL += " AND TNCC_ENHO.HOTE_CODI <> 1 "
            SQL += " AND TNCC_ENHO.HOTE_CODI = " & Me.mHoteCodiNewCentral
            SQL += " GROUP BY TNCC_ENHO.ENTI_CODI,OPER_NECO,ENTI_NCON_AF "

            Me.DbNewContaAux.TraerLector(SQL)
            While Me.DbNewContaAux.mDbLector.Read
                If Primerregistro = True Then
                    Primerregistro = False
                    ControlCuenta = CType(Me.DbNewContaAux.mDbLector("ENTI_NCON_AF"), String)
                End If

                Entidades += " " & CType(Me.DbNewContaAux.mDbLector("ENTI_CODI"), String) & " (" & CType(Me.DbNewContaAux.mDbLector("ENTI_NCON_AF"), String) & ")"

                If CType(Me.DbNewContaAux.mDbLector("ENTI_NCON_AF"), String) <> ControlCuenta Then
                    Texto = "Más de Un ttoo de NewCentral usa la misma Cuenta : " & vTacoCodi & " para la Gestión de Cobros" & vbCrLf
                    Texto += " Sin embargo alguno de ellos NO tiene o Difiere en la Cuenta de Cliente "
                    Texto += vbCrLf & vbCrLf
                    Texto += " Revise También que la Cuenta dentro de Un Mismo TTOO sea la misma para TODOS los Hoteles "
                    '       MsgBox(Texto & vbCrLf & "Entidades = " & Entidades, MsgBoxStyle.Information, "Atención")
                    Cuenta = "0"
                    Avisa = True
                End If

            End While
            Me.DbNewContaAux.mDbLector.Close()



            If Avisa = False Then
                SQL = "SELECT DISTINCT NVL(ENTI_NCON_AF,'?') FROM NCC.TNCC_OPER,NCC.TNCC_ENHO WHERE "
                SQL += "  TNCC_OPER.ENTI_CODI = TNCC_ENHO.ENTI_CODI"
                SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
                '    SQL += " AND TNCC_ENHO.HOTE_CODI <> 1 "
                SQL += " AND TNCC_ENHO.HOTE_CODI = " & Me.mHoteCodiNewCentral
                SQL += " AND ENTI_NCON_AF IS NOT NULL"
                Cuenta = Me.DbNewContaAux.EjecutaSqlScalar(SQL)

            Else
                MsgBox("Entidades a revisar Cuenta Cliente NewConta = " & vTacoCodi & vbCrLf & vbCrLf & "NewCentral = " & Entidades)
            End If


            ' compone 5 y 6 digito cuenta de cliente 
            Cuenta = Mid(Cuenta, 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(Cuenta, 5, 6)

            Return Cuenta

        Catch ex As Exception
            MsgBox(ex.Message)
            Return "0"
        End Try

    End Function
    Private Function BuscaCuentaClienteNewHotel(ByVal vTacoCodi As String) As String


        Dim Cuenta As String = " "
        Dim Control As Integer = 0
        Dim ControlCuenta As String = " "
        Dim Primerregistro As Boolean = True
        Dim Texto As String = " "

        Dim Avisa As Boolean = False
        Dim Entidades As String = ""




        Try


            ' CHAPUZA LOPEZ
            ' SI ES EL SAHARA PLAYA Y SI EL CODIGO NEWCONTA ES > 8999 BUSCA LAS CUENTAS EN EL ESQUEMA DE MEDIAS PENSIONES RMC2
            If Me.mEmpCod = "13" And CInt(vTacoCodi) > 8999 Then
                Return Me.BuscaCuentaClienteNewHotel2(vTacoCodi)
            End If


            ' Localizar la Cuenta Cobtable de la Pago Anticipado entidad
            ' Ojo esta qwery trata de buscar la cuenta contable de la entidad de la central y puede devolver varios registros
            ' si hay varias entidades con el mismo codigo de newconta y distinta cuenta contable


            SQL = "SELECT DISTINCT OPER_NECO,TNHT_ENTI.ENTI_CODI, NVL (ENTI_NCON_AF, '0') AS ENTI_NCON_AF"
            SQL += " FROM TNHT_OPER, TNHT_ENTI "
            SQL += " WHERE TNHT_OPER.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
            SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
            SQL += " GROUP BY TNHT_ENTI.ENTI_CODI,OPER_NECO,ENTI_NCON_AF "

            Me.DbLeeNewHotel.TraerLector(SQL)
            While Me.DbLeeNewHotel.mDbLector.Read
                If Primerregistro = True Then
                    Primerregistro = False
                    ControlCuenta = CType(Me.DbLeeNewHotel.mDbLector("ENTI_NCON_AF"), String)
                End If

                Entidades += " " & CType(Me.DbLeeNewHotel.mDbLector("ENTI_CODI"), String)

                If CType(Me.DbLeeNewHotel.mDbLector("ENTI_NCON_AF"), String) <> ControlCuenta Then

                    Texto = "Más de Un ttoo de NewCentral usa la misma Cuenta : " & vTacoCodi & " para la Gestión de Cobros" & vbCrLf
                    Texto += " Sin embargo alguno de ellos NO tiene o Difiere en la Cuenta de Cliente "
                    Texto += vbCrLf & vbCrLf
                    Texto += " Revise También que la Cuenta dentro de Un Mismo TTOO sea la misma para TODOS los Hoteles "
                    '     MsgBox(Texto & vbCrLf & "Entidades = " & Entidades, MsgBoxStyle.Information, "Atención")
                    Cuenta = "0"
                    Avisa = True
                End If

            End While
            Me.DbLeeNewHotel.mDbLector.Close()


            If Avisa = False Then
                SQL = "SELECT DISTINCT NVL(ENTI_NCON_AF,'0') FROM TNHT_OPER,TNHT_ENTI WHERE "
                SQL += "  TNHT_OPER.ENTI_CODI = TNHT_ENTI.ENTI_CODI"
                SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
                SQL += " AND ENTI_NCON_AF IS NOT NULL"
                Cuenta = Me.DbLeeNewHotel.EjecutaSqlScalar(SQL)
            Else
                MsgBox("Entidades a revisar Cuenta Cliente NewConta = " & vTacoCodi & vbCrLf & vbCrLf & "NewHotel = " & Entidades)
            End If


            ' compone 5 y 6 digito cuenta de cliente 
            Cuenta = Mid(Cuenta, 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(Cuenta, 5, 6)

            Return Cuenta

        Catch ex As Exception
            MsgBox(ex.Message)
            Return "0"
        End Try

    End Function
    Private Function BuscaCuentaClienteNewHotel2(ByVal vTacoCodi As String) As String


        Dim Cuenta As String = " "
        Dim Control As Integer = 0
        Dim ControlCuenta As String = " "
        Dim Primerregistro As Boolean = True
        Dim Texto As String = " "

        Dim Avisa As Boolean = False
        Dim Entidades As String = ""




        Try


            ' Localizar la Cuenta Cobtable de la Pago Anticipado entidad
            ' Ojo esta qwery trata de buscar la cuenta contable de la entidad de la central y puede devolver varios registros
            ' si hay varias entidades con el mismo codigo de newconta y distinta cuenta contable


            SQL = "SELECT DISTINCT OPER_NECO,TNHT_ENTI.ENTI_CODI, NVL (ENTI_NCON_AF, '0') AS ENTI_NCON_AF"
            SQL += " FROM TNHT_OPER, TNHT_ENTI "
            SQL += " WHERE TNHT_OPER.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
            SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
            SQL += " GROUP BY TNHT_ENTI.ENTI_CODI,OPER_NECO,ENTI_NCON_AF "

            Me.DbLeeNewHotel2.TraerLector(SQL)
            While Me.DbLeeNewHotel2.mDbLector.Read
                If Primerregistro = True Then
                    Primerregistro = False
                    ControlCuenta = CType(Me.DbLeeNewHotel2.mDbLector("ENTI_NCON_AF"), String)
                End If

                Entidades += " " & CType(Me.DbLeeNewHotel2.mDbLector("ENTI_CODI"), String) & "(" & CType(Me.DbLeeNewHotel2.mDbLector("ENTI_NCON_AF"), String) & ")"

                If CType(Me.DbLeeNewHotel2.mDbLector("ENTI_NCON_AF"), String) <> ControlCuenta Then

                    Texto = "Más de Un ttoo de NewCentral usa la misma Cuenta : " & vTacoCodi & " para la Gestión de Cobros" & vbCrLf
                    Texto += " Sin embargo alguno de ellos NO tiene o Difiere en la Cuenta de Cliente "
                    Texto += vbCrLf & vbCrLf
                    Texto += " Revise También que la Cuenta dentro de Un Mismo TTOO sea la misma para TODOS los Hoteles "
                    '     MsgBox(Texto & vbCrLf & "Entidades = " & Entidades, MsgBoxStyle.Information, "Atención")
                    Cuenta = "0"
                    Avisa = True
                End If

            End While
            Me.DbLeeNewHotel2.mDbLector.Close()


            If Avisa = False Then
                SQL = "SELECT DISTINCT NVL(ENTI_NCON_AF,'0') FROM TNHT_OPER,TNHT_ENTI WHERE "
                SQL += "  TNHT_OPER.ENTI_CODI = TNHT_ENTI.ENTI_CODI"
                SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
                SQL += " AND ENTI_NCON_AF IS NOT NULL"
                Cuenta = Me.DbLeeNewHotel2.EjecutaSqlScalar(SQL)
            Else
                MsgBox("Entidades a revisar Cuenta Cliente NewConta = " & vTacoCodi & vbCrLf & vbCrLf & "NewHotel = " & Entidades)
            End If


            ' compone 5 y 6 digito cuenta de cliente 
            Cuenta = Mid(Cuenta, 1, 4) & Me.mCta56DigitoCuentaClientes & Mid(Cuenta, 5, 6)

            Return Cuenta

        Catch ex As Exception
            MsgBox(ex.Message)
            Return "0"
        End Try

    End Function
    Private Function BuscaCifClienteCentral(ByVal vTacoCodi As String) As String


        Dim Cif As String = " "
        Dim Control As Integer = 0
        Dim ControlCuenta As String = " "
        Dim Primerregistro As Boolean = True
        Dim Texto As String = " "

        Dim Avisa As Boolean = False
        Dim Entidades As String = ""



        Try



            ' Localizar la Cuenta Cobtable de la Pago Anticipado entidad
            ' Ojo esta qwery trata de buscar la cuenta contable de la entidad de la central y puede devolver varios registros
            ' si hay varias entidades con el mismo codigo de newconta y distinta cuenta contable


            SQL = "SELECT DISTINCT OPER_NECO,TNCC_ENHO.ENTI_CODI, NVL (ENTI_NUCO, '?') AS ENTI_NCON_AF"
            SQL += " FROM NCC.TNCC_OPER, NCC.TNCC_ENHO "
            SQL += " WHERE TNCC_OPER.ENTI_CODI = TNCC_ENHO.ENTI_CODI "
            SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
            ' SQL += " AND TNCC_ENHO.HOTE_CODI <> 1 "
            SQL += " AND TNCC_ENHO.HOTE_CODI = " & Me.mHoteCodiNewCentral
            SQL += " GROUP BY TNCC_ENHO.ENTI_CODI,OPER_NECO,ENTI_NUCO "

            Me.DbNewContaAux.TraerLector(SQL)
            While Me.DbNewContaAux.mDbLector.Read
                If Primerregistro = True Then
                    Primerregistro = False
                    ControlCuenta = CType(Me.DbNewContaAux.mDbLector("ENTI_NCON_AF"), String)
                End If

                Entidades += " " & CType(Me.DbNewContaAux.mDbLector("ENTI_CODI"), String) & "(" & CType(Me.DbNewContaAux.mDbLector("ENTI_NCON_AF"), String) & ")"

                If CType(Me.DbNewContaAux.mDbLector("ENTI_NCON_AF"), String) <> ControlCuenta Then
                    Texto = "Más de Un ttoo de NewCentral usa la misma Cuenta : " & vTacoCodi & " para la Gestión de Cobros" & vbCrLf
                    Texto += " Sin embargo alguno de ellos NO tiene o Difiere en la Cuenta de Cliente "
                    Texto += vbCrLf & vbCrLf
                    Texto += " Revise También que la Cuenta dentro de Un Mismo TTOO sea la misma para TODOS los Hoteles "
                    '       MsgBox(Texto & vbCrLf & "Entidades = " & Entidades, MsgBoxStyle.Information, "Atención")
                    Cif = "0"
                    Avisa = True
                End If

            End While
            Me.DbNewContaAux.mDbLector.Close()



            If Avisa = False Then
                SQL = "SELECT DISTINCT NVL(ENTI_NUCO,'?') FROM NCC.TNCC_OPER,NCC.TNCC_ENHO WHERE "
                SQL += "  TNCC_OPER.ENTI_CODI = TNCC_ENHO.ENTI_CODI"
                SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
                '    SQL += " AND TNCC_ENHO.HOTE_CODI <> 1 "
                SQL += " AND TNCC_ENHO.HOTE_CODI = " & Me.mHoteCodiNewCentral
                SQL += " AND ENTI_NCON_AF IS NOT NULL"
                Cif = Me.DbNewContaAux.EjecutaSqlScalar(SQL)

            Else
                MsgBox("Entidades a revisar CIF Cliente NewConta = " & vTacoCodi & vbCrLf & vbCrLf & "NewCentral  = " & Entidades)
            End If




            Return Cif

        Catch ex As Exception
            MsgBox(ex.Message)
            Return "0"
        End Try

    End Function
    Private Function BuscaCifClienteNewHotel(ByVal vTacoCodi As String) As String


        Dim Cif As String = " "
        Dim Control As Integer = 0
        Dim ControlCuenta As String = " "
        Dim Primerregistro As Boolean = True
        Dim Texto As String = " "

        Dim Avisa As Boolean = False
        Dim Entidades As String = ""




        Try


            ' CHAPUZA LOPEZ
            ' SI ES EL SAHARA PLAYA Y SI EL CODIGO NEWCONTA ES > 8999 BUSCA LAS CUENTAS EN EL ESQUEMA DE MEDIAS PENSIONES RMC2
            If Me.mEmpCod = "13" And CInt(vTacoCodi) > 8999 Then
                Return Me.BuscaCifClienteNewHotel2(vTacoCodi)
            End If


            ' Localizar la Cuenta Cobtable de la Pago Anticipado entidad
            ' Ojo esta qwery trata de buscar la cuenta contable de la entidad de la central y puede devolver varios registros
            ' si hay varias entidades con el mismo codigo de newconta y distinta cuenta contable


            SQL = "SELECT DISTINCT OPER_NECO,TNHT_ENTI.ENTI_CODI, NVL (ENTI_NUCO, '0') AS ENTI_NCON_AF"
            SQL += " FROM TNHT_OPER, TNHT_ENTI "
            SQL += " WHERE TNHT_OPER.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
            SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
            SQL += " GROUP BY TNHT_ENTI.ENTI_CODI,OPER_NECO,ENTI_NUCO "

            Me.DbLeeNewHotel.TraerLector(SQL)
            While Me.DbLeeNewHotel.mDbLector.Read
                If Primerregistro = True Then
                    Primerregistro = False
                    ControlCuenta = CType(Me.DbLeeNewHotel.mDbLector("ENTI_NCON_AF"), String)
                End If

                Entidades += " " & CType(Me.DbLeeNewHotel.mDbLector("ENTI_CODI"), String)

                If CType(Me.DbLeeNewHotel.mDbLector("ENTI_NCON_AF"), String) <> ControlCuenta Then

                    Texto = "Más de Un ttoo de NewCentral usa la misma Cuenta : " & vTacoCodi & " para la Gestión de Cobros" & vbCrLf
                    Texto += " Sin embargo alguno de ellos NO tiene o Difiere en la Cuenta de Cliente "
                    Texto += vbCrLf & vbCrLf
                    Texto += " Revise También que la Cuenta dentro de Un Mismo TTOO sea la misma para TODOS los Hoteles "
                    '     MsgBox(Texto & vbCrLf & "Entidades = " & Entidades, MsgBoxStyle.Information, "Atención")
                    Cif = "0"
                    Avisa = True
                End If

            End While
            Me.DbLeeNewHotel.mDbLector.Close()


            If Avisa = False Then
                SQL = "SELECT DISTINCT NVL(ENTI_NUCO,'0') FROM TNHT_OPER,TNHT_ENTI WHERE "
                SQL += "  TNHT_OPER.ENTI_CODI = TNHT_ENTI.ENTI_CODI"
                SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
                SQL += " AND ENTI_NCON_AF IS NOT NULL"
                Cif = Me.DbLeeNewHotel.EjecutaSqlScalar(SQL)
            Else
                MsgBox("Entidades a revisar CIF Cliente NewConta = " & vTacoCodi & vbCrLf & vbCrLf & " NewHotel = " & Entidades)
            End If




            Return Cif

        Catch ex As Exception
            MsgBox(ex.Message)
            Return "0"
        End Try

    End Function
    Private Function BuscaCifClienteNewHotel2(ByVal vTacoCodi As String) As String


        Dim Cif As String = " "
        Dim Control As Integer = 0
        Dim ControlCuenta As String = " "
        Dim Primerregistro As Boolean = True
        Dim Texto As String = " "

        Dim Avisa As Boolean = False
        Dim Entidades As String = ""




        Try


            ' Localizar la Cuenta Cobtable de la Pago Anticipado entidad
            ' Ojo esta qwery trata de buscar la cuenta contable de la entidad de la central y puede devolver varios registros
            ' si hay varias entidades con el mismo codigo de newconta y distinta cuenta contable


            SQL = "SELECT DISTINCT OPER_NECO,TNHT_ENTI.ENTI_CODI, NVL (ENTI_NUCO, '0') AS ENTI_NCON_AF"
            SQL += " FROM TNHT_OPER, TNHT_ENTI "
            SQL += " WHERE TNHT_OPER.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
            SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
            SQL += " GROUP BY TNHT_ENTI.ENTI_CODI,OPER_NECO,ENTI_NUCO "

            Me.DbLeeNewHotel2.TraerLector(SQL)
            While Me.DbLeeNewHotel2.mDbLector.Read
                If Primerregistro = True Then
                    Primerregistro = False
                    ControlCuenta = CType(Me.DbLeeNewHotel2.mDbLector("ENTI_NCON_AF"), String)
                End If

                Entidades += " " & CType(Me.DbLeeNewHotel2.mDbLector("ENTI_CODI"), String)

                If CType(Me.DbLeeNewHotel2.mDbLector("ENTI_NCON_AF"), String) <> ControlCuenta Then

                    Texto = "Más de Un ttoo de NewCentral usa la misma Cuenta : " & vTacoCodi & " para la Gestión de Cobros" & vbCrLf
                    Texto += " Sin embargo alguno de ellos NO tiene o Difiere en la Cuenta de Cliente "
                    Texto += vbCrLf & vbCrLf
                    Texto += " Revise También que la Cuenta dentro de Un Mismo TTOO sea la misma para TODOS los Hoteles "
                    '     MsgBox(Texto & vbCrLf & "Entidades = " & Entidades, MsgBoxStyle.Information, "Atención")
                    Cif = "0"
                    Avisa = True
                End If

            End While
            Me.DbLeeNewHotel2.mDbLector.Close()


            If Avisa = False Then
                SQL = "SELECT DISTINCT NVL(ENTI_NUCO,'0') FROM TNHT_OPER,TNHT_ENTI WHERE "
                SQL += "  TNHT_OPER.ENTI_CODI = TNHT_ENTI.ENTI_CODI"
                SQL += " AND OPER_NECO = '" & vTacoCodi & "'"
                SQL += " AND ENTI_NCON_AF IS NOT NULL"
                Cif = Me.DbLeeNewHotel2.EjecutaSqlScalar(SQL)
            Else
                MsgBox("Entidades a revisar CIF Cliente NewConta = " & vTacoCodi & vbCrLf & vbCrLf & "NewHotel = " & Entidades)
            End If




            Return Cif

        Catch ex As Exception
            MsgBox(ex.Message)
            Return "0"
        End Try

    End Function
#End Region
End Class
