Option Strict On
Imports System.IO
Public Class ContanetNewConta
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
    Private mTrataAnulacionesNewConta As Integer
    Private mTrataMultiCobro As Boolean = False

    Private mHoteCodiNewCentral As Integer

    Private mParaUserNewHotel As String

    Private mParaFActAnul As Integer
    Private mParaFActAnul2 As Integer

    Private mParaFActNeg As Integer
    Private mParaFActNeg2 As Integer

    Private mParaContaBilizaPagos As Integer
    Private mParaTextosFijos As Integer





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
    Private mCodigoFacturas As String







    ' OTROS 
    Private iASCII(63) As Integer       'Para conversión a MS-DOS
    Private AuxCif As String
    Private AuxCuenta As String

    Private AuxRecibo As String


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
    Private DbNewContaAux2 As C_DATOS.C_DatosOledb
    Private DbGrabaCentral As C_DATOS.C_DatosOledb
    Private DbSpyro As C_DATOS.C_DatosOledb

    Private mEstaEnDerc As Boolean = False
    Private mContarDere As Integer

    Dim mAuxiliar As Double
    Dim mControl As String
    Dim mRepetido As Boolean
    Dim mPrimerRegistro As Boolean

    Dim mtextoApunte As String


#Region "CONSTRUCTOR"
    Public Sub New(ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vStrConexionCentral As String,
    ByVal vStrConexionNewConta As String, ByVal vFecha As Date, ByVal vFileName As String, ByVal vDebug As Boolean,
    ByVal vConrolDebug As System.Windows.Forms.TextBox, ByVal vListBox As System.Windows.Forms.ListBox,
    ByVal vStrConexionSpyro As String, ByVal vProgress As System.Windows.Forms.ProgressBar,
    ByVal vEstableciomientoNewConta As String, ByVal vEmpNum As Integer, ByVal vStrConexionHotel As String,
    ByVal vOtrosCreditos As Boolean, ByVal vOtrosDebitos As Boolean, ByVal vCodigoReclamaciones As String,
    ByVal vCodigoNotasCredito As String, ByVal vForm As System.Windows.Forms.Form, ByVal vHoteCodiNewCentral As Integer, ByVal vCodigoFacturas As String, ByVal vTrataMultiCobro As Boolean)


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
        Me.mCodigoFacturas = vCodigoFacturas

        Me.mFecha = vFecha

        Me.mParaFileName = vFileName

        Me.mEstablecimientoNewConta = vEstableciomientoNewConta

        Me.mHoteCodiNewCentral = vHoteCodiNewCentral

        Me.mTrataMultiCobro = vTrataMultiCobro

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

            Me.DbLeeNewHotel2 = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
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


            Me.DbNewContaAux2 = New C_DATOS.C_DatosOledb(Me.mStrConexionConta)
            Me.DbNewContaAux2.AbrirConexion()
            Me.DbNewContaAux2.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

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

            SQL = "SELECT  NVL(PARA_ORIGENCUENTAS,0) AS PARA_ORIGENCUENTAS,NVL(PARA_CFATODIARI_COD,'?') AS  DIARIO,PARA_TRATA_ANULACIONES,NVL(PARA_USER_NEWHOTEL,'RMC') AS PARA_USER_NEWHOTEL "
            SQL += ",NVL(PARA_USA_FACT_NEG,0) AS PARA_USA_FACT_NEG ,NVL(PARA_USA_FACT_NEG2,0) AS PARA_USA_FACT_NEG2,NVL(PARA_USA_FACT_ANUL,0) AS PARA_USA_FACT_ANUL ,NVL(PARA_USA_FACT_ANUL2,0) AS PARA_USA_FACT_ANUL2 ,PARA_USA_PAGOS "
            SQL += ",NVL(PARA_TEXTOS_FIJOS,0) AS  PARA_TEXTOS_FIJOS "
            SQL += "  FROM TC_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum

            Me.DbLeeCentral.TraerLector(SQL)
            If Me.DbLeeCentral.mDbLector.Read Then
                Me.mOrigenCuentasNewConta = CType(Me.DbLeeCentral.mDbLector.Item("PARA_ORIGENCUENTAS"), Integer)
                Me.mCfatodiari_Cod = CType(Me.DbLeeCentral.mDbLector.Item("DIARIO"), String)
                Me.mTrataAnulacionesNewConta = CType(Me.DbLeeCentral.mDbLector.Item("PARA_TRATA_ANULACIONES"), Integer)
                Me.mParaUserNewHotel = CStr(Me.DbLeeCentral.mDbLector.Item("PARA_USER_NEWHOTEL"))

                Me.mParaFActAnul = CInt(Me.DbLeeCentral.mDbLector.Item("PARA_USA_FACT_ANUL"))
                Me.mParaFActAnul2 = CInt(Me.DbLeeCentral.mDbLector.Item("PARA_USA_FACT_ANUL2"))

                Me.mParaFActNeg = CInt(Me.DbLeeCentral.mDbLector.Item("PARA_USA_FACT_NEG"))
                Me.mParaFActNeg2 = CInt(Me.DbLeeCentral.mDbLector.Item("PARA_USA_FACT_NEG2"))
                Me.mParaContaBilizaPagos = CInt(Me.DbLeeCentral.mDbLector.Item("PARA_USA_PAGOS"))

                Me.mParaTextosFijos = CInt(Me.DbLeeCentral.mDbLector.Item("PARA_TEXTOS_FIJOS"))

            Else
                Me.mOrigenCuentasNewConta = 0
                Me.mCfatodiari_Cod = "?"
                Me.mTrataAnulacionesNewConta = 0

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

    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                      ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                      , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
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
                                        ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliar As String, ByVal vAuxiliar2 As String,
ByVal vDocCre As String, ByVal vDocDeb As String, ByVal vDescCre As String, ByVal vDescDeb As String, ByVal vRecibo As String, ByVal vRecoDaem As String, ByVal vRecoDaan As String, ByVal vTotalFactura As Double, ByVal vKeyField As String, ByVal vMulticobro As Integer, vFechaVAlor As Date)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_AUXILIAR_STRING2,ASNT_DOC_CREDITO,ASNT_DOC_DEBITO,ASNT_DESC_CREDITO,ASNT_DESC_DEBITO,ASNT_RECIBO,ASNT_RECODAEM,ASNT_RECODAAN,ASNT_TOTAL_FACTURA,ASNT_KEY_FIELD,ASNT_MULTICOBRO) values ('"
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
            SQL += Format(vFechaVAlor, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliar & "','" & vAuxiliar2 & "','" & vDocCre & "','" & vDocDeb & "','" & vDescCre & "','" & vDescDeb & "','" & vRecibo & "','" & vRecoDaem & "','" & vRecoDaan & "'," & vTotalFactura & ",'" & vKeyField & "'," & vMulticobro & ")"




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
    Private Sub InsertaOracleGustavo(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                      ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                      , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
                                        ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String,
                                        ByVal vAuxiliar As String, ByVal vAuxiliar2 As String, ByVal vFactura As String,
                                        ByVal vSerie As String, ByVal vDocCre As String, ByVal vDocDeb As String, ByVal vDescCre As String,
                                        ByVal vDescDeb As String, ByVal vRecibo As String, ByVal vRecoDaem As String, ByVal vRecoDaan As String, ByVal vTotalFactura As Double, ByVal vAsntKeyField As String, ByVal vMulticobro As Integer, vFechaValor As Date)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_AUXILIAR_STRING2,"
            SQL += "ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_DOC_CREDITO,ASNT_DOC_DEBITO,ASNT_DESC_CREDITO,ASNT_DESC_DEBITO,ASNT_RECIBO,ASNT_RECODAEM,ASNT_RECODAAN,ASNT_TOTAL_FACTURA,ASNT_KEY_FIELD,ASNT_MULTICOBRO) values ('"
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
            SQL += Format(vFechaValor, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" _
            & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliar & "','" & vAuxiliar2 & "','" & vFactura & "','" &
            vSerie & "','" & vDocCre & "','" & vDocDeb & "','" & vDescCre & "','" & vDescDeb & "','" & vRecibo & "','" & vRecoDaem & "','" & vRecoDaan & "'," & vTotalFactura & ",'" & vAsntKeyField & "'," & vMulticobro & ")"




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
    '**
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                      ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                      , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
                                        ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vFechaValor As Date, ByVal vAuxiliar As String, ByVal vDocCre As String, ByVal vDocDeb As String, ByVal vDescCre As String, ByVal vDescDeb As String, vRecibo As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_DOC_CREDITO,ASNT_DOC_DEBITO,ASNT_DESC_CREDITO,ASNT_DESC_DEBITO,ASNT_RECIBOS) values ('"
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
            SQL += Format(vFechaValor, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliar & "','" & vDocCre & "','" & vDocDeb & "','" & vDescCre & "','" & vDescDeb & "','" & vRecibo & "')"




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
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
                                       ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vFechaValor As Date, ByVal vAuxiliar As String)

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
            SQL += Format(vFechaValor, "dd/MM/yyyy") & "','"
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
                    '      Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    Private Sub InsertaOracleLopez(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                      ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                      , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
                                        ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliar As String, ByVal vAuxiliar2 As String, vComprobante As String, vBancosCod2 As String, vFechaVAlor As Date, vRecibo As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_AUXILIAR_STRING2,ASNT_CFBCOCOMP_COMPROB,ASNT_BANCOS_NOT,ASNT_RECIBOS) values ('"
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
            SQL += Format(vFechaVAlor, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliar & "','" & vAuxiliar2 & "','" & vComprobante & "','" & vBancosCod2 & "','" & vRecibo & "')"




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

    Private Sub GeneraFileAC(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
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
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
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
            "N" & FechaAsiento &
            Format(Me.mFecha, "ddMMyyyy") &
            " ".PadRight(40, CChar(" ")) &
            Me.mCfatotip_Cod.PadRight(4, CChar(" ")))

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
        End Try
    End Sub
    Private Sub GeneraFileACconFechaValor(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
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
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
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
            "N" & FechaAsiento &
            Format(vFechaValor, "ddMMyyyy") &
            " ".PadRight(40, CChar(" ")) &
            Me.mCfatotip_Cod.PadRight(4, CChar(" ")))

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
        End Try
    End Sub
    Private Sub GeneraFileAC2(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
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
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
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
            "N" & FechaAsiento &
            Format(Me.mFecha, "ddMMyyyy") &
            " ".PadRight(40, CChar(" ")) &
            Me.mCfatotip_Cod.PadRight(4, CChar(" ")) &
            mCfivaLibro_Cod.PadRight(2, CChar(" ")) &
            vFactuTipo_cod.PadRight(6, CChar(" ")) &
            CType(vNfactura, String).PadRight(8, CChar(" ")))

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
        End Try
    End Sub
    Private Sub GeneraFileAC3(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
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
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
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
            "N" & FechaAsiento &
            Format(Me.mFecha, "ddMMyyyy") &
            " ".PadRight(40, CChar(" ")) &
            Me.mCfatotip_Cod.PadRight(4, CChar(" ")) &
            "*")


        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
        End Try
    End Sub
    Private Sub GeneraFileAA(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                 ByVal vCfatocab_Refer As Integer,
                                  ByVal vCfcta_Cod As String, ByVal vCfcctip_Cod As String, ByVal vCfcCosto_Cod As String,
                                  ByVal vImonep As Double)
        Try

            '-------------------------------------------------------------------------------------------------
            '  Apuntes Contables(CFATOLINCC)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Mid(Format(Me.mFecha, "ddMMyyyy"), 5, 4) &
            Me.mCfatodiari_Cod.PadRight(4, CChar(" ")) &
            " ".PadLeft(8, CChar(" ")) &
            " ".PadLeft(4, CChar(" ")) &
            vCfcta_Cod.PadRight(15, CChar(" ")) &
            vCfcctip_Cod.PadRight(4, CChar(" ")) &
            vCfcCosto_Cod.PadRight(15, CChar(" ")) &
            CType(vImonep, String).PadLeft(16, CChar(" ")) & Format(Me.mFecha, "ddMMyyyy"))

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAA")
        End Try
    End Sub
    Private Sub GeneraFileFV(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String,
    ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String, ByVal vPendiente As Double)

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
            Mid(vSfactura, 1, 15).PadRight(15, CChar("-")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Format(Me.mFecha, "yyyy") &
            Me.mCfatodiari_Cod.PadRight(4, CChar(" ")) &
            " ".PadLeft(8, CChar(" ")) &
            " ".PadLeft(4, CChar(" ")) &
            vCuenta.PadRight(15, CChar(" ")) &
            vCif.PadRight(20, CChar(" ")) &
            " ".PadRight(6, CChar(" ")) &
            " ".PadRight(1, CChar(" ")) &
            " ".PadRight(8, CChar(" ")) &
            " ".PadRight(8, CChar(" ")) &
            Me.mGvagente_Cod.PadRight(8, CChar(" ")) &
            CType(vPendiente, String).PadRight(16, CChar(" ")) &
            CType(vPendiente, String).PadRight(16, CChar(" ")) & "NN")


        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileFV")
        End Try
    End Sub
    Private Sub GeneraFileFV2(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String,
   ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String)

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
            Mid(vSfactura, 1, 15).PadRight(15, CChar("-")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Format(Me.mFecha, "yyyy") &
            Me.mCfatodiari_Cod.PadRight(4, CChar(" ")) &
            " ".PadLeft(8, CChar(" ")) &
            " ".PadLeft(4, CChar(" ")) &
            vCuenta.PadRight(15, CChar(" ")) &
            vCif.PadRight(20, CChar(" ")) &
            " ".PadRight(6, CChar(" ")) &
            " ".PadRight(1, CChar(" ")) &
            " ".PadRight(8, CChar(" ")) &
            " ".PadRight(8, CChar(" ")) &
            Me.mGvagente_Cod.PadRight(8, CChar(" ")) &
            "0".PadRight(16, CChar(" ")) &
            "0".PadRight(16, CChar(" ")) & "NN")


        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileFV")
        End Try
    End Sub
    Private Sub GeneraFileVF(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String,
   ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String)

        Try
            '-------------------------------------------------------------------------------------------------
            '  Facturas(FACTURAS)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Me.mCfivaLibro_Cod.PadRight(2, CChar(" ")) &
            vSerie.PadRight(6, CChar(" ")) &
            CType(vNfactura, String).PadLeft(8, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Format(Me.mFecha, "yyyy") &
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

            If IsNothing(Me.DbNewContaAux2) = False Then
                If Me.DbNewContaAux2.EstadoConexion = ConnectionState.Open Then
                    Me.DbNewContaAux2.CerrarConexion()
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


#End Region

#Region "ASIENTOS NEWCONTA"
#Region "ASIENTO 111 PAGOS RECIBIDOS"
    Private Sub NCPagosRecibidos()
        Try
            Dim Total As Double
            Dim Cuenta As String = " "
            Linea = 0

            Dim DescCred As String
            Dim DocCred As String

            Dim DescDeb As String
            Dim DocDeb As String
            Dim Recibo As String



            SQL = "SELECT  'MOVIMIENTOS DE CREDITO TIPO PAGO' , 'DEBE',KEY_FIELD, VNCO_MOCO.HOTE_CODI,VNCO_MOCO.TACO_CODI,NVL(TNCO_TIMO.TIMO_COCO,'0') AS CUENTA, VNCO_MOCO.TIMO_CODI, "
            SQL += "         VNCO_MOCO.MOCO_DOCU, VNCO_MOCO.MOCO_DOC1, VNCO_MOCO.UNMO_CODI, "
            SQL += "         TRUNC (VNCO_MOCO.MOCO_DAVA) DAVA, VNCO_MOCO.MOCO_VAOR, "
            SQL += "         VNCO_MOCO.MOCO_CAMB, VNCO_MOCO.MOCO_DEBI, VNCO_MOCO.MOCO_CRED AS TOTAL, "
            SQL += "         VNCO_MOCO.MOCO_ANUL, NVL(VNCO_MOCO.TACO_NOME,' ') AS TACO_NOME, VNCO_MOCO.MOCO_RECI, "
            SQL += "         VNCO_MOCO.MOCO_CODI, VNCO_MOCO.MOCO_DESC, VNCO_MOCO.MOCO_EXTE, "
            SQL += "         VNCO_MOCO.MOCO_OBSE, VNCO_MOCO.DIFE_RATE, VNCO_MOCO.MOCO_DIFE, "
            SQL += "         ABS (VNCO_MOCO.MOCO_VADE) MOCO_VADE, VNCO_MOCO.PACO_PAAS, "
            SQL += "         VNCO_MOCO.TACO_ORIG, VNCO_MOCO.MOCO_DATR,VNCO_MOCO.MOCO_DAVA, VNCO_MOCO.HOTE_CODI, "
            SQL += "         HOTE_DESC, NVL(MOLI_DESC,' ') AS MOLI_DESC, VNCO_MOCO.FACT_CODI "


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
            '    SQL += "     AND VNCO_MOCO.MOCO_DAVA = '" & Me.mFecha & "'"
            SQL += "     AND VNCO_MOCO.MOCO_DATR = '" & Me.mFecha & "'"

            ' Excluye Pagos Realizados y Anulados el mismo dia 
            SQL += "  AND (VNCO_MOCO.MOCO_DAAN > VNCO_MOCO.MOCO_DATR  OR VNCO_MOCO.MOCO_DAAN IS NULL) "

            SQL += "   AND VNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            SQL += "    ORDER BY VNCO_MOCO.MOCO_DAVA DESC, VNCO_MOCO.MOCO_ANUL "


            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read



                '20190125
                If CType(Me.DbNewConta.mDbLector("TIMO_CODI"), String) = mCodigoReclamaciones Then
                    ' si es una reclamacion se busca la cuenta del cliente 
                    Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))


                ElseIf CType(Me.DbNewConta.mDbLector("TIMO_CODI"), String) = mCodigoNotasCredito Then
                    ' si es una nota de credifo se busca la cuenta del cliente 
                    Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))
                Else
                    ' Localizar la Cuenta Cobtable de la forma de Cobro
                    SQL = "SELECT NVL(TIMO_COCO,'0') FROM TNCO_TIMO WHERE TIMO_CODI = '" & CType(Me.DbNewConta.mDbLector("TIMO_CODI"), String) & "'"
                    Cuenta = Me.DbNewContaAux.EjecutaSqlScalar(SQL)
                End If


                If IsDBNull(Me.DbNewConta.mDbLector("MOCO_DOCU")) = False Then
                    DocCred = CStr(Me.DbNewConta.mDbLector("MOCO_DOCU"))
                Else
                    DocCred = ""
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("MOCO_DESC")) = False Then
                    DescCred = CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                Else
                    DescCred = ""
                End If

                DocDeb = ""
                DescDeb = ""



                If Me.mParaTextosFijos = 1 Then
                    If CStr(Me.DbNewConta.mDbLector("TIMO_CODI")) = mCodigoNotasCredito Then
                        Me.mtextoApunte = StrConv(CStr(Me.DbNewConta.mDbLector("MOCO_DESC")), VbStrConv.ProperCase)
                    Else
                        Me.mtextoApunte = Mid(StrConv(CStr(Me.DbNewConta.mDbLector("MOLI_DESC")), VbStrConv.ProperCase), 1, 8) & "  " & StrConv(CStr(Me.DbNewConta.mDbLector("TACO_NOME")), VbStrConv.ProperCase)
                    End If

                Else
                    If CStr(Me.DbNewConta.mDbLector("TIMO_CODI")) = mCodigoNotasCredito Then
                        Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                    ElseIf IsDBNull(Me.DbNewConta.mDbLector("MOCO_DESC")) = True Then
                        Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOLI_DESC"))
                    Else
                        Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOLI_DESC")) & " ," & CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                    End If
                End If





                Recibo = BuscaReciboDeCobro(CStr(Me.DbNewConta.mDbLector("TACO_CODI")), CInt(Me.DbNewConta.mDbLector("MOCO_CODI")))


                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 111, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Me.mtextoApunte, Total, "NO", "", CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", CDate(Format(CDate(Me.DbNewConta.mDbLector("MOCO_DAVA")), "dd/MM/yyyy")), CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), DocCred, DocDeb, DescCred, DescDeb, Recibo)
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Me.mtextoApunte, Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()

            '' lo mismo al revez ( deposito anticipado ) 

            SQL = "SELECT  'MOVIMIENTOS DE CREDITO TIPO PAGO' , 'DEBE',KEY_FIELD, VNCO_MOCO.HOTE_CODI,VNCO_MOCO.TACO_CODI AS TACO_CODI ,NVL(TNCO_TACO.TACO_CODA,'0') AS CUENTA, VNCO_MOCO.TIMO_CODI, "
            SQL += "         VNCO_MOCO.MOCO_DOCU, VNCO_MOCO.MOCO_DOC1, VNCO_MOCO.UNMO_CODI, "
            SQL += "         TRUNC (VNCO_MOCO.MOCO_DAVA) DAVA, VNCO_MOCO.MOCO_VAOR, "
            SQL += "         VNCO_MOCO.MOCO_CAMB, VNCO_MOCO.MOCO_DEBI, VNCO_MOCO.MOCO_CRED AS TOTAL, "
            SQL += "         VNCO_MOCO.MOCO_ANUL, NVL(VNCO_MOCO.TACO_NOME,' ') AS TACO_NOME , VNCO_MOCO.MOCO_RECI, "
            SQL += "         VNCO_MOCO.MOCO_CODI, VNCO_MOCO.MOCO_DESC , VNCO_MOCO.MOCO_EXTE, "
            SQL += "         VNCO_MOCO.MOCO_OBSE, VNCO_MOCO.DIFE_RATE, VNCO_MOCO.MOCO_DIFE, "
            SQL += "         ABS (VNCO_MOCO.MOCO_VADE) MOCO_VADE, VNCO_MOCO.PACO_PAAS, "
            SQL += "         VNCO_MOCO.TACO_ORIG, VNCO_MOCO.MOCO_DATR,VNCO_MOCO.MOCO_DAVA ,VNCO_MOCO.HOTE_CODI, "
            SQL += "         HOTE_DESC, NVL(MOLI_DESC,' ') AS MOLI_DESC, VNCO_MOCO.FACT_CODI , NVL(TNCO_TACO.TACO_NUCO,'?') AS NIF"


            SQL += "    FROM VNCO_MOCO, TNCO_HOTE, TNCO_UTIL, TNCO_MOLI,TNCO_TIMO,TNCO_TACO"


            SQL += "   WHERE VNCO_MOCO.HOTE_CODI = TNCO_HOTE.HOTE_CODI "
            SQL += "     AND VNCO_MOCO.UTIL_CODI = TNCO_UTIL.UTIL_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI "
            SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "
            SQL += "     AND VNCO_MOCO.TACO_CODI = TNCO_TACO.TACO_CODI "



            ''
            SQL += "     AND TNCO_MOLI.LICL_CODI = 1 "
            '    -- SOLO MOV DE CREDITO "
            SQL += "     AND VNCO_MOCO.MOCO_DEBI = 0 "
            '    -- SOLO MOV DE TIPO PAGO "
            SQL += "     AND TNCO_TIMO.TIMO_PAGA = 1 "
            '--
            '   SQL += "     AND VNCO_MOCO.MOCO_DAVA = '" & Me.mFecha & "'"
            SQL += "     AND VNCO_MOCO.MOCO_DATR= '" & Me.mFecha & "'"

            ' Excluye Pagos Realizados y Anulados el mismo dia 
            SQL += "  AND (VNCO_MOCO.MOCO_DAAN > VNCO_MOCO.MOCO_DATR  OR VNCO_MOCO.MOCO_DAAN IS NULL) "

            SQL += "     AND VNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"

            SQL += "    ORDER BY VNCO_MOCO.MOCO_DAVA DESC, VNCO_MOCO.MOCO_ANUL "


            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read


                Cuenta = BuscaCuentaPagosAnticipadosNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))


                If IsDBNull(Me.DbNewConta.mDbLector("MOCO_DOCU")) = False Then
                    DocCred = CStr(Me.DbNewConta.mDbLector("MOCO_DOCU"))
                Else
                    DocCred = ""
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("MOCO_DESC")) = False Then
                    DescCred = CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                Else
                    DescCred = ""
                End If

                DocDeb = ""
                DescDeb = ""


                If Me.mParaTextosFijos = 1 Then
                    If CStr(Me.DbNewConta.mDbLector("TIMO_CODI")) = mCodigoNotasCredito Then
                        Me.mtextoApunte = StrConv(CStr(Me.DbNewConta.mDbLector("MOCO_DESC")), VbStrConv.ProperCase)
                    Else
                        Me.mtextoApunte = Mid(StrConv(CStr(Me.DbNewConta.mDbLector("MOLI_DESC")), VbStrConv.ProperCase), 1, 8) & "  " & StrConv(CStr(Me.DbNewConta.mDbLector("TACO_NOME")), VbStrConv.ProperCase)
                    End If

                Else
                    If CStr(Me.DbNewConta.mDbLector("TIMO_CODI")) = mCodigoNotasCredito Then
                        Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                    ElseIf IsDBNull(Me.DbNewConta.mDbLector("MOCO_DESC")) = True Then
                        Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOLI_DESC"))
                    Else
                        Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOLI_DESC")) & " ," & CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                    End If
                End If


                Recibo = BuscaReciboDeCobro(CStr(Me.DbNewConta.mDbLector("TACO_CODI")), CInt(Me.DbNewConta.mDbLector("MOCO_CODI")))


                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)


                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 111, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Me.mtextoApunte, Total, "NO", "", CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", CDate(Format(CDate(Me.DbNewConta.mDbLector("MOCO_DAVA")), "dd/MM/yyyy")), CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), DocCred, DocDeb, DescCred, DescDeb, Recibo)
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Me.mtextoApunte, Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()

        Catch EX As Exception
            MsgBox(EX.Message)
        End Try


    End Sub
    Private Sub NCPagosRecibidosFacturasNegativas()
        Try
            Dim Total As Double
            Dim Cuenta As String = " "
            Linea = 0

            Dim DescCred As String
            Dim DocCred As String

            Dim DescDeb As String
            Dim DocDeb As String
            Dim Recibo As String

            SQL = "SELECT "
            SQL += "    'MOVIMIENTOS DE DEBITO  NEGATIVOS SERIE DE FACTURAS DE CREDITO', "
            SQL += "    'DEBE', "
            SQL += "    TNCO_MOCO.HOTE_CODI, "
            SQL += "    TNCO_MOCO.TACO_CODI, "
            SQL += "    NVL(TNCO_TIMO.TIMO_COCO,'0') AS CUENTA, "
            SQL += "    TNCO_MOCO.TIMO_CODI, "
            SQL += "    TNCO_MOCO.MOCO_DOCU, "
            SQL += "    TNCO_MOCO.MOCO_DOC1, "
            SQL += "    TNCO_MOCO.UNMO_CODI, "
            SQL += "    TRUNC(TNCO_MOCO.MOCO_DAVA) DAVA, "
            SQL += "    TNCO_MOCO.MOCO_VAOR, "
            SQL += "    TNCO_MOCO.MOCO_CAMB, "
            SQL += "    TNCO_MOCO.MOCO_DEBI, "
            SQL += "    TNCO_MOCO.MOCO_DEBI *-1 AS TOTAL, "
            SQL += "    TNCO_MOCO.MOCO_ANUL, "
            SQL += "    NVL(TNCO_TACO.TACO_NOME,' ') AS TACO_NOME, "
            SQL += "    TNCO_MOCO.MOCO_RECI, "
            SQL += "    TNCO_MOCO.MOCO_CODI, "
            SQL += "    NVL(TNCO_MOCO.MOCO_DESC,' ') AS MOCO_DESC, "
            SQL += "    TNCO_MOCO.MOCO_EXTE, "
            SQL += "    TNCO_MOCO.MOCO_OBSE, "
            SQL += "    ABS(TNCO_MOCO.MOCO_VADE) MOCO_VADE, "
            SQL += "    TNCO_MOCO.TACO_ORIG, "
            SQL += "    TNCO_MOCO.MOCO_DATR, "
            SQL += "    TNCO_MOCO.MOCO_DAVA, "
            SQL += "    TNCO_MOCO.HOTE_CODI, "
            SQL += "    HOTE_DESC, "
            SQL += "    NVL(MOLI_DESC,' ') AS MOLI_DESC, "
            SQL += "    TNCO_MOCO.FACT_CODI, NVL(TNCO_TACO.TACO_NUCO,'?') AS NIF "
            SQL += "FROM "
            SQL += "    TNCO_MOCO, "
            SQL += "    TNCO_HOTE, "
            SQL += "    TNCO_UTIL, "
            SQL += "    TNCO_MOLI, "
            SQL += "    TNCO_TIMO, "
            SQL += "    TNCO_TACO "
            SQL += "WHERE "
            SQL += "    TNCO_MOCO.HOTE_CODI = TNCO_HOTE.HOTE_CODI "
            SQL += "    AND   TNCO_MOCO.UTIL_CODI = TNCO_UTIL.UTIL_CODI "
            SQL += "    AND   TNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI "
            SQL += "    AND   TNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "
            SQL += "    AND   TNCO_MOCO.TACO_CODI = TNCO_TACO.TACO_CODI "
            SQL += "    AND   TNCO_MOLI.LICL_CODI = 1 "
            SQL += "    AND   TNCO_MOCO.MOCO_DEBI < 0 "
            SQL += "    AND   TNCO_TIMO.TIMO_PAGA = 0 "
            SQL += "    AND   TNCO_MOCO.MOCO_DATR= '" & Me.mFecha & "'"
            SQL += "    AND   TNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            SQL += "    AND   SEFA_CODI IN ( "
            SQL += "        SELECT "
            SQL += "            SEFA_CODI "
            SQL += "        FROM " & Me.mParaUserNewHotel & ".TNHT_SEFA "

            SQL += "        WHERE "
            SQL += "            SEFA_TIFA = '1' "
            SQL += "    ) "
            SQL += "ORDER BY "
            SQL += "    TNCO_MOCO.MOCO_DAVA DESC, "
            SQL += "    TNCO_MOCO.MOCO_ANUL "



            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read



                '20190125
                If CType(Me.DbNewConta.mDbLector("TIMO_CODI"), String) = mCodigoReclamaciones Then
                    ' si es una reclamacion se busca la cuenta del cliente 
                    Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))


                ElseIf CType(Me.DbNewConta.mDbLector("TIMO_CODI"), String) = mCodigoNotasCredito Then
                    ' si es una nota de credifo se busca la cuenta del cliente 
                    Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))
                ElseIf CType(Me.DbNewConta.mDbLector("TIMO_CODI"), String) = mCodigoFacturas Then
                    ' si es una Factura Negativa se busca la cuenta del cliente 
                    Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))
                Else
                    ' Localizar la Cuenta Cobtable de la forma de Cobro
                    SQL = "SELECT NVL(TIMO_COCO,'0') FROM TNCO_TIMO WHERE TIMO_CODI = '" & CType(Me.DbNewConta.mDbLector("TIMO_CODI"), String) & "'"
                    Cuenta = Me.DbNewContaAux.EjecutaSqlScalar(SQL)
                End If


                If IsDBNull(Me.DbNewConta.mDbLector("MOCO_DOCU")) = False Then
                    DocCred = CStr(Me.DbNewConta.mDbLector("MOCO_DOCU"))
                Else
                    DocCred = ""
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("MOCO_DESC")) = False Then
                    DescCred = CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                Else
                    DescCred = ""
                End If

                DocDeb = ""
                DescDeb = ""

                If CStr(Me.DbNewConta.mDbLector("TIMO_CODI")) = mCodigoNotasCredito Or CStr(Me.DbNewConta.mDbLector("TIMO_CODI")) = mCodigoFacturas Then
                    Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                ElseIf CStr(Me.DbNewConta.mDbLector("MOCO_DESC")).Length = 1 Then
                    Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOLI_DESC"))
                Else
                    Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOLI_DESC")) & "," & CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                End If


                Recibo = ""


                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 112, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Me.mtextoApunte, Total, "NO", "", CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", CDate(Format(CDate(Me.DbNewConta.mDbLector("MOCO_DAVA")), "dd/MM/yyyy")), CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), DocCred, DocDeb, DescCred, DescDeb, Recibo)
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Me.mtextoApunte, Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()

            '' lo mismo al revez ( deposito anticipado ) 

            SQL = "SELECT "
            SQL += "    'MOVIMIENTOS DE DEBITO  NEGATIVOS SERIE DE FACTURAS DE CREDITO', "
            SQL += "    'DEBE', "
            SQL += "    TNCO_MOCO.HOTE_CODI, "
            SQL += "    TNCO_MOCO.TACO_CODI, "
            SQL += "    NVL(TNCO_TIMO.TIMO_COCO,'0') AS CUENTA, "
            SQL += "    TNCO_MOCO.TIMO_CODI, "
            SQL += "    TNCO_MOCO.MOCO_DOCU, "
            SQL += "    TNCO_MOCO.MOCO_DOC1, "
            SQL += "    TNCO_MOCO.UNMO_CODI, "
            SQL += "    TRUNC(TNCO_MOCO.MOCO_DAVA) DAVA, "
            SQL += "    TNCO_MOCO.MOCO_VAOR, "
            SQL += "    TNCO_MOCO.MOCO_CAMB, "
            SQL += "    TNCO_MOCO.MOCO_DEBI, "
            SQL += "    TNCO_MOCO.MOCO_DEBI *-1 AS TOTAL, "
            SQL += "    TNCO_MOCO.MOCO_ANUL, "
            SQL += "    NVL(TNCO_TACO.TACO_NOME,' ') AS TACO_NOME, "
            SQL += "    TNCO_MOCO.MOCO_RECI, "
            SQL += "    TNCO_MOCO.MOCO_CODI, "
            SQL += "    NVL(TNCO_MOCO.MOCO_DESC,' ') AS MOCO_DESC, "
            SQL += "    TNCO_MOCO.MOCO_EXTE, "
            SQL += "    TNCO_MOCO.MOCO_OBSE, "
            SQL += "    ABS(TNCO_MOCO.MOCO_VADE) MOCO_VADE, "
            SQL += "    TNCO_MOCO.TACO_ORIG, "
            SQL += "    TNCO_MOCO.MOCO_DATR, "
            SQL += "    TNCO_MOCO.MOCO_DAVA, "
            SQL += "    TNCO_MOCO.HOTE_CODI, "
            SQL += "    HOTE_DESC, "
            SQL += "    NVL(MOLI_DESC,' ') AS MOLI_DESC, "
            SQL += "    TNCO_MOCO.FACT_CODI, NVL(TNCO_TACO.TACO_NUCO,'?') AS NIF "
            SQL += "FROM "
            SQL += "    TNCO_MOCO, "
            SQL += "    TNCO_HOTE, "
            SQL += "    TNCO_UTIL, "
            SQL += "    TNCO_MOLI, "
            SQL += "    TNCO_TIMO, "
            SQL += "    TNCO_TACO "
            SQL += "WHERE "
            SQL += "    TNCO_MOCO.HOTE_CODI = TNCO_HOTE.HOTE_CODI "
            SQL += "    AND   TNCO_MOCO.UTIL_CODI = TNCO_UTIL.UTIL_CODI "
            SQL += "    AND   TNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI "
            SQL += "    AND   TNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "
            SQL += "    AND   TNCO_MOCO.TACO_CODI = TNCO_TACO.TACO_CODI "
            SQL += "    AND   TNCO_MOLI.LICL_CODI = 1 "
            SQL += "    AND   TNCO_MOCO.MOCO_DEBI < 0 "
            SQL += "    AND   TNCO_TIMO.TIMO_PAGA = 0 "
            SQL += "    AND   TNCO_MOCO.MOCO_DATR= '" & Me.mFecha & "'"
            SQL += "    AND   TNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            SQL += "    AND   SEFA_CODI IN ( "
            SQL += "        SELECT "
            SQL += "            SEFA_CODI "
            SQL += "        FROM " & Me.mParaUserNewHotel & ".TNHT_SEFA "
            SQL += "        WHERE "
            SQL += "            SEFA_TIFA = '1' "
            SQL += "    ) "
            SQL += "ORDER BY "
            SQL += "    TNCO_MOCO.MOCO_DAVA DESC, "
            SQL += "    TNCO_MOCO.MOCO_ANUL "



            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read


                Cuenta = BuscaCuentaPagosAnticipadosNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))


                If IsDBNull(Me.DbNewConta.mDbLector("MOCO_DOCU")) = False Then
                    DocCred = CStr(Me.DbNewConta.mDbLector("MOCO_DOCU"))
                Else
                    DocCred = ""
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("MOCO_DESC")) = False Then
                    DescCred = CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                Else
                    DescCred = ""
                End If

                DocDeb = ""
                DescDeb = ""


                If CStr(Me.DbNewConta.mDbLector("TIMO_CODI")) = mCodigoNotasCredito Or CStr(Me.DbNewConta.mDbLector("TIMO_CODI")) = mCodigoFacturas Then
                    Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                ElseIf CStr(Me.DbNewConta.mDbLector("MOCO_DESC")).Length = 1 Then
                    Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOLI_DESC"))
                Else
                    Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOLI_DESC")) & "," & CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                End If




                Recibo = ""
                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)


                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 112, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Me.mtextoApunte, Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", CDate(Format(CDate(Me.DbNewConta.mDbLector("MOCO_DAVA")), "dd/MM/yyyy")), CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), DocCred, DocDeb, DescCred, DescDeb, Recibo)
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Me.mtextoApunte, Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()

        Catch EX As Exception
            MsgBox(EX.Message)
        End Try


    End Sub
    Private Sub NCPagosRecibidosFacturasSerieDeAnulacion()
        Try
            Dim Total As Double
            Dim Cuenta As String = " "
            Linea = 0

            Dim DescCred As String
            Dim DocCred As String

            Dim DescDeb As String
            Dim DocDeb As String
            Dim Recibo As String

            SQL = "SELECT "
            SQL += "    'MOVIMIENTOS DE DEBITO  NEGATIVOS SERIE DE FACTURAS DE CREDITO', "
            SQL += "    'DEBE', "
            SQL += "    TNCO_MOCO.HOTE_CODI, "
            SQL += "    TNCO_MOCO.TACO_CODI, "
            SQL += "    NVL(TNCO_TIMO.TIMO_COCO,'0') AS CUENTA, "
            SQL += "    TNCO_MOCO.TIMO_CODI, "
            SQL += "    TNCO_MOCO.MOCO_DOCU, "
            SQL += "    TNCO_MOCO.MOCO_DOC1, "
            SQL += "    TNCO_MOCO.UNMO_CODI, "
            SQL += "    TRUNC(TNCO_MOCO.MOCO_DAVA) DAVA, "
            SQL += "    TNCO_MOCO.MOCO_VAOR, "
            SQL += "    TNCO_MOCO.MOCO_CAMB, "
            SQL += "    TNCO_MOCO.MOCO_DEBI, "
            SQL += "    TNCO_MOCO.MOCO_DEBI *-1 AS TOTAL, "
            SQL += "    TNCO_MOCO.MOCO_ANUL, "
            SQL += "    NVL(TNCO_TACO.TACO_NOME,' ') AS TACO_NOME, "
            SQL += "    TNCO_MOCO.MOCO_RECI, "
            SQL += "    TNCO_MOCO.MOCO_CODI, "
            SQL += "    NVL(TNCO_MOCO.MOCO_DESC,' ') AS MOCO_DESC, "
            SQL += "    TNCO_MOCO.MOCO_EXTE, "
            SQL += "    TNCO_MOCO.MOCO_OBSE, "
            SQL += "    ABS(TNCO_MOCO.MOCO_VADE) MOCO_VADE, "
            SQL += "    TNCO_MOCO.TACO_ORIG, "
            SQL += "    TNCO_MOCO.MOCO_DATR, "
            SQL += "    TNCO_MOCO.MOCO_DAVA, "
            SQL += "    TNCO_MOCO.HOTE_CODI, "
            SQL += "    HOTE_DESC, "
            SQL += "    NVL(MOLI_DESC,' ') AS MOLI_DESC, "
            SQL += "    TNCO_MOCO.FACT_CODI, NVL(TNCO_TACO.TACO_NUCO,'?') AS NIF "
            SQL += "FROM "
            SQL += "    TNCO_MOCO, "
            SQL += "    TNCO_HOTE, "
            SQL += "    TNCO_UTIL, "
            SQL += "    TNCO_MOLI, "
            SQL += "    TNCO_TIMO, "
            SQL += "    TNCO_TACO "
            SQL += "WHERE "
            SQL += "    TNCO_MOCO.HOTE_CODI = TNCO_HOTE.HOTE_CODI "
            SQL += "    AND   TNCO_MOCO.UTIL_CODI = TNCO_UTIL.UTIL_CODI "
            SQL += "    AND   TNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI "
            SQL += "    AND   TNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "
            SQL += "    AND   TNCO_MOCO.TACO_CODI = TNCO_TACO.TACO_CODI "
            SQL += "    AND   TNCO_MOLI.LICL_CODI = 1 "
            SQL += "    AND   TNCO_MOCO.MOCO_DEBI < 0 "
            SQL += "    AND   TNCO_TIMO.TIMO_PAGA = 0 "
            SQL += "    AND   TNCO_MOCO.MOCO_DATR= '" & Me.mFecha & "'"
            SQL += "    AND   TNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            SQL += "    AND   SEFA_CODI IN ( "
            SQL += "        SELECT "
            SQL += "            SEFA_CODI "
            SQL += "        FROM " & Me.mParaUserNewHotel & ".TNHT_SEFA "

            SQL += "        WHERE "
            SQL += "            SEFA_TIFA = '6' "
            SQL += "    ) "
            SQL += "ORDER BY "
            SQL += "    TNCO_MOCO.MOCO_DAVA DESC, "
            SQL += "    TNCO_MOCO.MOCO_ANUL "



            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read



                '20190125
                If CType(Me.DbNewConta.mDbLector("TIMO_CODI"), String) = mCodigoReclamaciones Then
                    ' si es una reclamacion se busca la cuenta del cliente 
                    Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))


                ElseIf CType(Me.DbNewConta.mDbLector("TIMO_CODI"), String) = mCodigoNotasCredito Then
                    ' si es una nota de credifo se busca la cuenta del cliente 
                    Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))
                ElseIf CType(Me.DbNewConta.mDbLector("TIMO_CODI"), String) = mCodigoFacturas Then
                    ' si es una Factura Negativa se busca la cuenta del cliente 
                    Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))
                Else
                    ' Localizar la Cuenta Cobtable de la forma de Cobro
                    SQL = "SELECT NVL(TIMO_COCO,'0') FROM TNCO_TIMO WHERE TIMO_CODI = '" & CType(Me.DbNewConta.mDbLector("TIMO_CODI"), String) & "'"
                    Cuenta = Me.DbNewContaAux.EjecutaSqlScalar(SQL)
                End If


                If IsDBNull(Me.DbNewConta.mDbLector("MOCO_DOCU")) = False Then
                    DocCred = CStr(Me.DbNewConta.mDbLector("MOCO_DOCU"))
                Else
                    DocCred = ""
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("MOCO_DESC")) = False Then
                    DescCred = CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                Else
                    DescCred = ""
                End If

                DocDeb = ""
                DescDeb = ""

                If CStr(Me.DbNewConta.mDbLector("TIMO_CODI")) = mCodigoNotasCredito Or CStr(Me.DbNewConta.mDbLector("TIMO_CODI")) = mCodigoFacturas Then
                    Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                ElseIf CStr(Me.DbNewConta.mDbLector("MOCO_DESC")).Length = 1 Then
                    Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOLI_DESC"))
                Else
                    Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOLI_DESC")) & "," & CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                End If


                Recibo = ""


                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 113, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Me.mtextoApunte, Total, "NO", "", CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", CDate(Format(CDate(Me.DbNewConta.mDbLector("MOCO_DAVA")), "dd/MM/yyyy")), CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), DocCred, DocDeb, DescCred, DescDeb, Recibo)
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Me.mtextoApunte, Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()

            '' lo mismo al revez ( deposito anticipado ) 

            SQL = "SELECT "
            SQL += "    'MOVIMIENTOS DE DEBITO  NEGATIVOS SERIE DE FACTURAS DE CREDITO', "
            SQL += "    'DEBE', "
            SQL += "    TNCO_MOCO.HOTE_CODI, "
            SQL += "    TNCO_MOCO.TACO_CODI, "
            SQL += "    NVL(TNCO_TIMO.TIMO_COCO,'0') AS CUENTA, "
            SQL += "    TNCO_MOCO.TIMO_CODI, "
            SQL += "    TNCO_MOCO.MOCO_DOCU, "
            SQL += "    TNCO_MOCO.MOCO_DOC1, "
            SQL += "    TNCO_MOCO.UNMO_CODI, "
            SQL += "    TRUNC(TNCO_MOCO.MOCO_DAVA) DAVA, "
            SQL += "    TNCO_MOCO.MOCO_VAOR, "
            SQL += "    TNCO_MOCO.MOCO_CAMB, "
            SQL += "    TNCO_MOCO.MOCO_DEBI, "
            SQL += "    TNCO_MOCO.MOCO_DEBI *-1 AS TOTAL, "
            SQL += "    TNCO_MOCO.MOCO_ANUL, "
            SQL += "    NVL(TNCO_TACO.TACO_NOME,' ') AS TACO_NOME, "
            SQL += "    TNCO_MOCO.MOCO_RECI, "
            SQL += "    TNCO_MOCO.MOCO_CODI, "
            SQL += "    NVL(TNCO_MOCO.MOCO_DESC,' ') AS MOCO_DESC, "
            SQL += "    TNCO_MOCO.MOCO_EXTE, "
            SQL += "    TNCO_MOCO.MOCO_OBSE, "
            SQL += "    ABS(TNCO_MOCO.MOCO_VADE) MOCO_VADE, "
            SQL += "    TNCO_MOCO.TACO_ORIG, "
            SQL += "    TNCO_MOCO.MOCO_DATR, "
            SQL += "    TNCO_MOCO.MOCO_DAVA, "
            SQL += "    TNCO_MOCO.HOTE_CODI, "
            SQL += "    HOTE_DESC, "
            SQL += "    NVL(MOLI_DESC,' ') AS MOLI_DESC, "
            SQL += "    TNCO_MOCO.FACT_CODI, NVL(TNCO_TACO.TACO_NUCO,'?') AS NIF "
            SQL += "FROM "
            SQL += "    TNCO_MOCO, "
            SQL += "    TNCO_HOTE, "
            SQL += "    TNCO_UTIL, "
            SQL += "    TNCO_MOLI, "
            SQL += "    TNCO_TIMO, "
            SQL += "    TNCO_TACO "
            SQL += "WHERE "
            SQL += "    TNCO_MOCO.HOTE_CODI = TNCO_HOTE.HOTE_CODI "
            SQL += "    AND   TNCO_MOCO.UTIL_CODI = TNCO_UTIL.UTIL_CODI "
            SQL += "    AND   TNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI "
            SQL += "    AND   TNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "
            SQL += "    AND   TNCO_MOCO.TACO_CODI = TNCO_TACO.TACO_CODI "
            SQL += "    AND   TNCO_MOLI.LICL_CODI = 1 "
            SQL += "    AND   TNCO_MOCO.MOCO_DEBI < 0 "
            SQL += "    AND   TNCO_TIMO.TIMO_PAGA = 0 "
            SQL += "    AND   TNCO_MOCO.MOCO_DATR= '" & Me.mFecha & "'"
            SQL += "    AND   TNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            SQL += "    AND   SEFA_CODI IN ( "
            SQL += "        SELECT "
            SQL += "            SEFA_CODI "
            SQL += "        FROM " & Me.mParaUserNewHotel & ".TNHT_SEFA "
            SQL += "        WHERE "
            SQL += "            SEFA_TIFA = '6' "
            SQL += "    ) "
            SQL += "ORDER BY "
            SQL += "    TNCO_MOCO.MOCO_DAVA DESC, "
            SQL += "    TNCO_MOCO.MOCO_ANUL "



            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read


                Cuenta = BuscaCuentaPagosAnticipadosNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))


                If IsDBNull(Me.DbNewConta.mDbLector("MOCO_DOCU")) = False Then
                    DocCred = CStr(Me.DbNewConta.mDbLector("MOCO_DOCU"))
                Else
                    DocCred = ""
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("MOCO_DESC")) = False Then
                    DescCred = CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                Else
                    DescCred = ""
                End If

                DocDeb = ""
                DescDeb = ""


                If CStr(Me.DbNewConta.mDbLector("TIMO_CODI")) = mCodigoNotasCredito Or CStr(Me.DbNewConta.mDbLector("TIMO_CODI")) = mCodigoFacturas Then
                    Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                ElseIf CStr(Me.DbNewConta.mDbLector("MOCO_DESC")).Length = 1 Then
                    Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOLI_DESC"))
                Else
                    Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOLI_DESC")) & "," & CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                End If




                Recibo = ""
                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)


                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 113, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Me.mtextoApunte, Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", CDate(Format(CDate(Me.DbNewConta.mDbLector("MOCO_DAVA")), "dd/MM/yyyy")), CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), DocCred, DocDeb, DescCred, DescDeb, Recibo)
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Me.mtextoApunte, Total)
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
                    Me.InsertaOracle("AC", 222, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, "NO", "", CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", Me.mFecha, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String))
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()

            '' lo mismo al revez ( deposito anticipado ) 

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



                Cuenta = BuscaCuentaPagosAnticipadosNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))



                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 222, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", Me.mFecha, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String))
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()


            ' NUEVO Dunas
            'se genera un asiento por cada pago de mas que se encuentre en un recibo anulado.

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

            SQL += " AND (MOCO_CODI,TACO_CODI) IN("

            SQL += " SELECT "
            SQL += "         TNCO_MOC1.MOCO_CODI, "
            SQL += "         TNCO_MOC1.TACO_CODI  "
            SQL += "    FROM TNCO_RECO TNCO_RECO, "
            SQL += "         TNCO_DERE TNCO_DERE, "
            SQL += "         VNCO_TACO TNCO_TACO, "
            SQL += "         TNCO_MOCO TNCO_MOCO, "
            SQL += "         TNCO_MOLI TNCO_MOLI, "
            SQL += "         TNCO_MOCO TNCO_MOC1 "
            SQL += "   WHERE ( (TNCO_RECO.RECO_CODI = TNCO_DERE.RECO_CODI(+)) "
            SQL += "          AND (TNCO_RECO.RECO_ANCI = TNCO_DERE.RECO_ANCI(+))) "
            SQL += "         AND (TNCO_RECO.TACO_CODI = TNCO_TACO.TACO_CODI) "
            SQL += "         AND ( (TNCO_RECO.TACO_CODI = TNCO_MOCO.TACO_CODI) "
            SQL += "              AND (TNCO_RECO.MOCO_CODI = TNCO_MOCO.MOCO_CODI)) "
            SQL += "         AND (TNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI) "
            SQL += "         AND ( (TNCO_DERE.TACO_CODI = TNCO_MOC1.TACO_CODI(+)) "
            SQL += "              AND (TNCO_DERE.MOCO_CODI = TNCO_MOC1.MOCO_CODI(+))) "
            SQL += "         AND TNCO_RECO.RECO_DAAN = '" & Me.mFecha & "'"

            'If Me.mTrataAnulacionesNewConta = 0 Then
            ' SQL = SQL & "  AND TNCO_RECO.RECO_DAAN > TNCO_RECO.RECO_DAEM "
            'End If

            SQL += "         AND TNCO_MOLI.LICL_CODI = 1 "
            SQL += "         AND TNCO_RECO.RECO_TIPO = 5 "
            SQL += "         AND TNCO_MOC1.TIMO_CODI = 'PDM' "
            SQL += "         AND TNCO_MOCO.HOTE_CODI = '1' )"




            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                ' Localizar la Cuenta Cobtable de la forma de Cobro
                SQL = "SELECT NVL(TIMO_COCO,'0') FROM TNCO_TIMO WHERE TIMO_CODI = '" & CType(Me.DbNewConta.mDbLector("TIMO_CODI"), String) & "'"
                Cuenta = Me.DbNewContaAux.EjecutaSqlScalar(SQL)

                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double) * -1
                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 222, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, "NO", "", CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", Me.mFecha, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String))
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()

            '' lo mismo al revez ( deposito anticipado ) 

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


            SQL += " AND (VNCO_MOCO.MOCO_CODI,VNCO_MOCO.TACO_CODI) IN("

            SQL += " SELECT "
            SQL += "         TNCO_MOC1.MOCO_CODI, "
            SQL += "         TNCO_MOC1.TACO_CODI  "
            SQL += "    FROM TNCO_RECO TNCO_RECO, "
            SQL += "         TNCO_DERE TNCO_DERE, "
            SQL += "         VNCO_TACO TNCO_TACO, "
            SQL += "         TNCO_MOCO TNCO_MOCO, "
            SQL += "         TNCO_MOLI TNCO_MOLI, "
            SQL += "         TNCO_MOCO TNCO_MOC1 "
            SQL += "   WHERE ( (TNCO_RECO.RECO_CODI = TNCO_DERE.RECO_CODI(+)) "
            SQL += "          AND (TNCO_RECO.RECO_ANCI = TNCO_DERE.RECO_ANCI(+))) "
            SQL += "         AND (TNCO_RECO.TACO_CODI = TNCO_TACO.TACO_CODI) "
            SQL += "         AND ( (TNCO_RECO.TACO_CODI = TNCO_MOCO.TACO_CODI) "
            SQL += "              AND (TNCO_RECO.MOCO_CODI = TNCO_MOCO.MOCO_CODI)) "
            SQL += "         AND (TNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI) "
            SQL += "         AND ( (TNCO_DERE.TACO_CODI = TNCO_MOC1.TACO_CODI(+)) "
            SQL += "              AND (TNCO_DERE.MOCO_CODI = TNCO_MOC1.MOCO_CODI(+))) "
            SQL += "         AND TNCO_RECO.RECO_DAAN = '" & Me.mFecha & "'"

            'If Me.mTrataAnulacionesNewConta = 0 Then
            'SQL = SQL & "  AND TNCO_RECO.RECO_DAAN > TNCO_RECO.RECO_DAEM "
            'End If

            SQL += "         AND TNCO_MOLI.LICL_CODI = 1 "
            SQL += "         AND TNCO_RECO.RECO_TIPO = 5 "
            SQL += "         AND TNCO_MOC1.TIMO_CODI = 'PDM' "
            SQL += "         AND TNCO_MOCO.HOTE_CODI = '1' )"




            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read


                Cuenta = BuscaCuentaPagosAnticipadosNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))



                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double) * -1
                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 222, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", Me.mFecha, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String))
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), Total)
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


            Dim DescCred As String
            Dim DocCred As String

            Dim DescDeb As String
            Dim DocDeb As String


            Dim Documento As String
            Dim Serie As String

            Dim DatosdeCredito1 As String
            Dim DatosdeCredito2 As String
            Dim DatosdeCredito4 As String
            Dim DatosdeCredito5 As String
            Dim DatosdeCredito6 As String



            Dim FecAnul As String

            Dim Multicobro As Integer


            SQL = " SELECT ' FACTURAS DE UN RECIBO', "
            SQL += "  TNCO_MOCO.MOCO_CODI, "
            SQL += "  TNCO_DERE.DERE_CODI, "
            SQL += "  TNCO_DERE.DERE_VARE, "
            SQL += "         TNCO_DERE.RECO_CODI, "
            SQL += "         TNCO_DERE.RECO_ANCI, "
            SQL += "         TNCO_DERE.DERE_TIPO, "
            SQL += "         TNCO_MOCO.MOCO_DAVA, "
            SQL += "         TNCO_MOCO.TIMO_CODI, "
            SQL += "         TNCO_MOCO.MOCO_DESC, "
            SQL += "         NVL(TNCO_MOCO.MOCO_DOCU,'?') AS DOCU_DEBI, "
            SQL += "         TNCO_MOCO.MOCO_VAOR, "
            SQL += "         TNCO_MOCO.MOCO_CAMB, "
            SQL += "         TNCO_MOCO.MOCO_VADE, "
            SQL += "         TNCO_DERE.DERE_VADE, "
            SQL += "         TNCO_MOCO.MOCO_CODI, "
            SQL += "         TNCO_TACO.TACO_CODI  "

            SQL += "       ,NVL(TIMO_PAGA,'0') AS TIPOPAGO "

            SQL += "       ,'' AS DOCU_CRED "
            SQL += "       ,'' AS DESC_CRED "
            SQL += "       ,'' AS DOCU_DEBI "
            SQL += "       ,NVL(TNCO_MOCO.MOCO_DESC,'?') DESC_DEBI "

            SQL += "  ,    TNCO_DERE.DERE_VARE AS TOTAL       "

            SQL += "     ,   NVL(TNCO_MOCO.FACT_CODI,'?') AS DOCUMENTO "
            SQL += "    ,    NVL(TNCO_MOCO.SEFA_CODI,'?') AS SERIE"
            SQL += "    ,    TNCO_RECO.RECO_DAEM,TNCO_RECO.RECO_DAAN"

            SQL += "     ,NVL(TNCO_TACO.TACO_NUCO,'?') AS NIF,"
            SQL += "       TNCO_TACO.TACO_CODI AS CODIGO, NVL(TACO_NOME,'?') AS NOMBRE "
            SQL += "    ,    NVL(TNCO_MOLI.MOLI_DESC,' ') AS MOLI_DESC "
            SQL = SQL & "  , TNCO_RECO.RECO_CODI || '/' ||  TNCO_RECO.RECO_ANCI AS RECIBO "
            SQL += "     ,   TNCO_MOCO.MOCO_VAOR AS TOTALF"


            SQL += ",TNCO_TIMO.TIMO_CODI "

            SQL += "    FROM TNCO_RECO, TNCO_DERE , TNCO_MOCO ,TNCO_TACO,TNCO_TIMO,TNCO_MOLI  "

            SQL += "   WHERE ( (TNCO_RECO.RECO_CODI = TNCO_DERE.RECO_CODI) "
            SQL += "          AND (TNCO_RECO.RECO_ANCI = TNCO_DERE.RECO_ANCI)) "

            SQL += "   AND ( (TNCO_DERE.TACO_CODI = TNCO_MOCO.TACO_CODI) "
            SQL += "          AND (TNCO_DERE.MOCO_CODI = TNCO_MOCO.MOCO_CODI)) "

            SQL += "  AND TNCO_MOCO.TACO_CODI = TNCO_TACO.TACO_CODI "
            SQL += "  AND TNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "

            SQL += "   AND TNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI "

            SQL = SQL & "  AND TNCO_RECO.RECO_DAEM = '" & Me.mFecha & "'"


            If Me.mTrataAnulacionesNewConta = 0 Then
                SQL = SQL & "  AND (TNCO_RECO.RECO_DAAN > TNCO_RECO.RECO_DAEM  OR TNCO_RECO.RECO_DAAN IS NULL) "
            End If

            SQL += "   AND TNCO_MOLI.LICL_CODI = 1 "
            SQL = SQL & "         AND TNCO_DERE.DERE_TIPO = 1 "
            ' Solo recibos de tipo regularizacion
            SQL = SQL & "         AND TNCO_RECO.RECO_TIPO = 5 "
            SQL = SQL & "  AND TNCO_RECO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"



            SQL += "ORDER BY TNCO_RECO.RECO_ANCI, TNCO_RECO.RECO_CODI "


            Me.DbNewConta.TraerLector(SQL)

            If Me.DbNewConta.mDbLector.HasRows Then
                '   MsgBox("Existen Documentos que han sido Afectados por la Anulación de Pagos ", MsgBoxStyle.Information, "Atención")
            End If

            While Me.DbNewConta.mDbLector.Read

                Linea = Linea + 1



                Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))


                If CType(Me.DbNewConta.mDbLector("TIPOPAGO"), String) = "0" Then
                    EsFormaDePago = False
                Else
                    EsFormaDePago = True
                End If


                If IsDBNull(Me.DbNewConta.mDbLector("DOCU_CRED")) = False Then
                    DocCred = CStr(Me.DbNewConta.mDbLector("DOCU_CRED"))
                Else
                    DocCred = ""
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("DESC_CRED")) = False Then
                    DescCred = CStr(Me.DbNewConta.mDbLector("DESC_CRED"))
                Else
                    DescCred = ""
                End If


                If IsDBNull(Me.DbNewConta.mDbLector("DOCU_DEBI")) = False Then
                    DocDeb = CStr(Me.DbNewConta.mDbLector("DOCU_DEBI"))
                Else
                    DocDeb = ""
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("DESC_DEBI")) = False Then
                    DescDeb = CStr(Me.DbNewConta.mDbLector("DESC_DEBI"))
                Else
                    DescDeb = ""
                End If



                If CStr(Me.DbNewConta.mDbLector("DOCUMENTO")) = "?" Then
                    ' Documento = ""
                    Documento = CStr(Me.DbNewConta.mDbLector("DOCU_DEBI"))
                    Serie = ""
                Else
                    Documento = CStr(Me.DbNewConta.mDbLector("DOCUMENTO"))
                    Serie = CStr(Me.DbNewConta.mDbLector("SERIE"))
                End If






                ' SALE A BUSCAR DATOS DEL MOVIMIENTO DE COBRO


                DatosdeCredito1 = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 1, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))
                DatosdeCredito2 = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 2, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))
                DatosdeCredito6 = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 6, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))



                '20190125
                If DatosdeCredito6 = mCodigoNotasCredito Then
                    Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double) * -1
                Else
                    Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("RECO_DAAN")) = True Then
                    FecAnul = ""
                Else
                    FecAnul = CStr(Me.DbNewConta.mDbLector("RECO_DAAN"))
                End If


                DocCred = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 3, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))



                ' Determinar el Tipo del Recibo ( Si es de Multiples Cobros y Multipples facturas o no ) 


                SQL = "SELECT COUNT(*) AS TOTAL FROM TNCO_DERC WHERE RECO_CODI = " & CInt(Me.DbNewConta.mDbLector.Item("RECO_CODI")) & " AND RECO_ANCI = " & CInt(Me.DbNewConta.mDbLector.Item("RECO_ANCI"))
                If Me.DbNewContaAux.EjecutaSqlScalar(SQL) = "0" Then
                    Multicobro = 0
                Else
                    Multicobro = 1
                End If










                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracleGustavo("AC", 333, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("DESC_DEBI"), String), Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("CODIGO"), String) & " " & CType(Me.DbNewConta.mDbLector("NOMBRE"), String), "SI", CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String).PadRight(50, CChar(" ")), DatosdeCredito2, Documento, Serie, DocCred, DocDeb, DatosdeCredito1, DescDeb, CStr(Me.DbNewConta.mDbLector("RECIBO")), CStr(Me.DbNewConta.mDbLector("RECO_DAEM")), FecAnul, CType(Me.DbNewConta.mDbLector("TOTALF"), Double), CStr(Me.DbNewConta.mDbLector("TACO_CODI")) & "/" & CStr(Me.DbNewConta.mDbLector("MOCO_CODI")), Multicobro, CDate(Format(CDate(Me.DbNewConta.mDbLector("MOCO_DAVA")), "dd/MM/yyyy")))
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("DESC_DEBI"), String), Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()

            '' lo mismo al revez ( deposito anticipado ) =

            SQL = "SELECT  "
            SQL += "  ' COBROS DE UN RECIBO', "
            SQL += "  TNCO_RECO.RECO_ANCI, "
            SQL += "         TNCO_RECO.RECO_CODI, "
            SQL += "         TNCO_TACO.TACO_NOME, "
            SQL += "         TNCO_TACO.TACO_MORA, "
            SQL += "         TNCO_TACO.TACO_CODP, "
            SQL += "         TNCO_TACO.TACO_LOCA, "
            SQL += "         TNCO_TACO.TACO_NUCO, "
            SQL += "         TNCO_DERE.DERE_VARE, "
            SQL += "  TNCO_DERE.DERE_CODI, "
            SQL += "         TNCO_RECO.RECO_ANUL, "
            SQL += "         TNCO_RECO.RECO_VALO, "
            SQL += "         TNCO_MOLI.LICL_CODI, "
            SQL += "         TNCO_MOLI.MOLI_DESC , "
            SQL += "         TNCO_RECO.UNMO_CODI, "
            SQL += "         TNCO_MOCO.MOCO_CAMB, "
            SQL += "         TNCO_RECO.RECO_TIPO, "
            SQL += "         TNCO_RECO.RECO_CONS, "
            SQL += "         NVL(TNCO_MOCO.MOCO_DESC,'?') AS MOCO_DESC, "
            SQL += "         TNCO_MOCO.MOCO_OBSE, "
            SQL += "         TNCO_DERE.DERE_TIPO, "
            SQL += "         TNCO_RECO.RECO_DAEM, "
            SQL += "         TNCO_MOC1.MOCO_DOC1, "
            SQL += "         NVL(TNCO_MOC1.MOCO_DOCU,'?') AS DOCU_DEBI, "
            SQL += "         TNCO_TACO.TACO_MOR1, "
            SQL += "         TNCO_TACO.TACO_MOR2, "
            SQL += "         TNCO_TACO.TACO_MOR3, "
            SQL += "         TNCO_MOCO.MOCO_VAOR, "
            SQL += "         TNCO_MOCO.MOCO_VADE, "
            SQL += "         TNCO_MOCO.MOCO_DAVA, "
            SQL += "         NVL(TNCO_MOCO.MOCO_DOCU,'?') AS MOCO_DOCU, "
            SQL += "         TNCO_DERE.TACO_CODI, "
            SQL += "         TNCO_DERE.MOCO_CODI "


            SQL += "       ,NVL(TIMO_PAGA,'0') AS TIPOPAGO "

            SQL += "       ,'' AS DOCU_CRED "
            SQL += "       ,'' AS DESC_CRED "
            SQL += "       ,'' AS DOCU_DEBI "
            SQL += "       ,'' AS  DESC_DEBI "

            SQL += "  ,    TNCO_DERE.DERE_VARE AS TOTAL       "


            SQL += "     ,   NVL(TNCO_MOCO.FACT_CODI,'?') AS DOCUMENTO "
            SQL += "    ,    NVL(TNCO_MOCO.SEFA_CODI,'?') AS SERIE"
            SQL += "    ,    TNCO_RECO.RECO_DAEM,TNCO_RECO.RECO_DAAN"

            SQL += "     ,NVL(TNCO_TACO.TACO_NUCO,'?') AS NIF,"
            SQL += "       TNCO_TACO.TACO_CODI AS CODIGO, NVL(TACO_NOME,'?') AS NOMBRE "
            SQL += "    ,    NVL(TNCO_MOLI.MOLI_DESC,' ') AS MOLI_DESC "
            SQL = SQL & "  , TNCO_RECO.RECO_CODI || '/' ||  TNCO_RECO.RECO_ANCI AS RECIBO "

            SQL += "     ,   TNCO_MOCO.MOCO_VAOR AS TOTALF"

            SQL += ",TNCO_TIMO.TIMO_CODI "

            SQL += "    FROM TNCO_RECO TNCO_RECO, "
            ' SQL += "         TNCO_DERC TNCO_DERE, "
            SQL += "         TNCO_DERE TNCO_DERE, "
            SQL += "         TNCO_TACO TNCO_TACO, "
            SQL += "         TNCO_MOCO TNCO_MOC1, "
            SQL += "         TNCO_MOCO TNCO_MOCO, "
            SQL += "         TNCO_MOLI TNCO_MOLI, "
            SQL += "         TNCO_TIMO "
            SQL += "   WHERE ( (TNCO_RECO.RECO_CODI = TNCO_DERE.RECO_CODI(+)) "
            SQL += "          AND (TNCO_RECO.RECO_ANCI = TNCO_DERE.RECO_ANCI(+))) "
            SQL += "         AND (TNCO_RECO.TACO_CODI = TNCO_TACO.TACO_CODI) "

            SQL += "         AND ( (TNCO_DERE.TACO_CODI = TNCO_MOC1.TACO_CODI(+)) "
            SQL += "              AND (TNCO_DERE.MOCO_CODI = TNCO_MOC1.MOCO_CODI(+))) "

            SQL += "         AND ( (TNCO_DERE.TACO_CODI = TNCO_MOCO.TACO_CODI) "


            SQL += "              AND (TNCO_DERE.MOCO_CODI = TNCO_MOCO.MOCO_CODI)) "
            SQL += "         AND (TNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI) "
            SQL += "  AND TNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "


            SQL = SQL & "  AND TNCO_RECO.RECO_DAEM = '" & Me.mFecha & "'"


            If Me.mTrataAnulacionesNewConta = 0 Then
                SQL = SQL & "  AND (TNCO_RECO.RECO_DAAN > TNCO_RECO.RECO_DAEM  OR TNCO_RECO.RECO_DAAN IS NULL) "
            End If

            SQL += "   AND TNCO_MOLI.LICL_CODI = 1 "
            SQL = SQL & "         AND TNCO_DERE.DERE_TIPO = 1 "
            ' Solo recibos de tipo regularizacion
            SQL = SQL & "         AND TNCO_RECO.RECO_TIPO = 5 "
            SQL = SQL & "  AND TNCO_RECO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"



            SQL += "ORDER BY TNCO_RECO.RECO_ANCI, TNCO_RECO.RECO_CODI "






            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                Linea = Linea + 1
                Cuenta = BuscaCuentaPagosAnticipadosNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))


                ' Si el documento o parte del documento se saldo con una nota de credito el importe va negativo

                If CType(Me.DbNewConta.mDbLector("TIPOPAGO"), String) = "0" Then
                    EsFormaDePago = False
                Else
                    EsFormaDePago = True
                End If
                If IsDBNull(Me.DbNewConta.mDbLector("DOCU_CRED")) = False Then
                    DocCred = CStr(Me.DbNewConta.mDbLector("DOCU_CRED"))
                Else
                    DocCred = ""
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("DESC_CRED")) = False Then
                    DescCred = CStr(Me.DbNewConta.mDbLector("DESC_CRED"))
                Else
                    DescCred = ""
                End If


                If IsDBNull(Me.DbNewConta.mDbLector("DOCU_DEBI")) = False Then
                    DocDeb = CStr(Me.DbNewConta.mDbLector("DOCU_DEBI"))
                Else
                    DocDeb = ""
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("DESC_DEBI")) = False Then
                    DescDeb = CStr(Me.DbNewConta.mDbLector("DESC_DEBI"))
                Else
                    DescDeb = ""
                End If






                ' SALE A BUSCAR DATOS DEL MOVIMIENTO DE COBRO

                DatosdeCredito1 = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 1, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))
                DatosdeCredito2 = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 2, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))
                DatosdeCredito4 = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 4, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))
                DatosdeCredito5 = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 5, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))
                DatosdeCredito6 = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 6, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))


                '20190125
                If DatosdeCredito6 = mCodigoNotasCredito Then
                    Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double) * -1
                Else
                    Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("RECO_DAAN")) = True Then
                    FecAnul = ""
                Else
                    FecAnul = CStr(Me.DbNewConta.mDbLector("RECO_DAAN"))
                End If


                If DatosdeCredito1.Length > 0 Then
                    Me.mtextoApunte = DatosdeCredito1
                Else
                    Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                End If


                DocCred = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 3, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))




                ' Determinar el Tipo del Recibo ( Si es de Multiples Cobros y Multipples facturas o no ) 


                SQL = "SELECT COUNT(*) AS TOTAL FROM TNCO_DERC WHERE RECO_CODI = " & CInt(Me.DbNewConta.mDbLector.Item("RECO_CODI")) & " AND RECO_ANCI = " & CInt(Me.DbNewConta.mDbLector.Item("RECO_ANCI"))
                If Me.DbNewContaAux.EjecutaSqlScalar(SQL) = "0" Then
                    Multicobro = 0
                Else
                    Multicobro = 1
                End If


                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 333, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Me.mtextoApunte, Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("CODIGO"), String) & " " & CType(Me.DbNewConta.mDbLector("MOCO_DOCU"), String), "SI", DatosdeCredito4, "", DocCred, DocDeb, DescCred, DescDeb, CStr(Me.DbNewConta.mDbLector("RECIBO")), CStr(Me.DbNewConta.mDbLector("RECO_DAEM")), FecAnul, CType(Me.DbNewConta.mDbLector("TOTALF"), Double), DatosdeCredito5, Multicobro, CDate(Format(CDate(Me.DbNewConta.mDbLector("MOCO_DAVA")), "dd/MM/yyyy")))
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Me.mtextoApunte, Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try

    End Sub

#End Region
#Region "ASIENTO 777-A FACTURAS DES-REGULARIZADAS AL ANULAR COBROS"
    Private Sub NCFacturasRegularizadasAnuladas2()
        Try
            Dim Total As Double
            Dim Cuenta As String
            Linea = 0
            Dim EsFormaDePago As Boolean = False


            Dim DescCred As String
            Dim DocCred As String

            Dim DescDeb As String
            Dim DocDeb As String


            Dim Documento As String
            Dim Serie As String

            Dim DatosdeCredito1 As String
            Dim DatosdeCredito2 As String
            Dim DatosdeCredito4 As String
            Dim DatosdeCredito5 As String
            Dim DatosdeCredito6 As String



            Dim FecAnul As String
            Dim Multicobro As Integer


            SQL = " SELECT ' FACTURAS DE UN RECIBO', "
            SQL += "  TNCO_MOCO.MOCO_CODI, "
            SQL += "  TNCO_DERE.DERE_CODI, "
            SQL += "  TNCO_DERE.DERE_VARE, "
            SQL += "         TNCO_DERE.RECO_CODI, "
            SQL += "         TNCO_DERE.RECO_ANCI, "
            SQL += "         TNCO_DERE.DERE_TIPO, "
            SQL += "         TNCO_MOCO.MOCO_DAVA, "
            SQL += "         TNCO_MOCO.TIMO_CODI, "
            SQL += "         TNCO_MOCO.MOCO_DESC, "
            SQL += "         NVL(TNCO_MOCO.MOCO_DOCU,'?') AS DOCU_DEBI, "
            SQL += "         TNCO_MOCO.MOCO_VAOR, "
            SQL += "         TNCO_MOCO.MOCO_CAMB, "
            SQL += "         TNCO_MOCO.MOCO_VADE, "
            SQL += "         TNCO_DERE.DERE_VADE, "
            SQL += "         TNCO_MOCO.MOCO_CODI, "
            SQL += "         TNCO_TACO.TACO_CODI  "

            SQL += "       ,NVL(TIMO_PAGA,'0') AS TIPOPAGO "

            SQL += "       ,'' AS DOCU_CRED "
            SQL += "       ,'' AS DESC_CRED "
            SQL += "       ,'' AS DOCU_DEBI "
            SQL += "       ,NVL(TNCO_MOCO.MOCO_DESC,'?') DESC_DEBI "

            SQL += "  ,    TNCO_DERE.DERE_VARE AS TOTAL       "

            SQL += "     ,   NVL(TNCO_MOCO.FACT_CODI,'?') AS DOCUMENTO "
            SQL += "    ,    NVL(TNCO_MOCO.SEFA_CODI,'?') AS SERIE"
            SQL += "    ,    TNCO_RECO.RECO_DAEM,TNCO_RECO.RECO_DAAN"

            SQL += "     ,NVL(TNCO_TACO.TACO_NUCO,'?') AS NIF,"
            SQL += "       TNCO_TACO.TACO_CODI AS CODIGO, NVL(TACO_NOME,'?') AS NOMBRE "
            SQL += "    ,    NVL(TNCO_MOLI.MOLI_DESC,' ') AS MOLI_DESC "
            SQL = SQL & "  , TNCO_RECO.RECO_CONS || '/' ||  TNCO_RECO.RECO_ANCI AS RECIBO "
            SQL += "     ,   TNCO_MOCO.MOCO_VAOR AS TOTALF"

            SQL += ",TNCO_TIMO.TIMO_CODI "

            SQL += "    FROM TNCO_RECO, TNCO_DERE , TNCO_MOCO ,TNCO_TACO,TNCO_TIMO,TNCO_MOLI  "

            SQL += "   WHERE ( (TNCO_RECO.RECO_CODI = TNCO_DERE.RECO_CODI) "
            SQL += "          AND (TNCO_RECO.RECO_ANCI = TNCO_DERE.RECO_ANCI)) "

            SQL += "   AND ( (TNCO_DERE.TACO_CODI = TNCO_MOCO.TACO_CODI) "
            SQL += "          AND (TNCO_DERE.MOCO_CODI = TNCO_MOCO.MOCO_CODI)) "

            SQL += "  AND TNCO_MOCO.TACO_CODI = TNCO_TACO.TACO_CODI "
            SQL += "  AND TNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "

            SQL += "   AND TNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI "

            SQL = SQL & "  AND TNCO_RECO.RECO_DAAN = '" & Me.mFecha & "'"


            If Me.mTrataAnulacionesNewConta = 0 Then
                SQL = SQL & "  AND (TNCO_RECO.RECO_DAAN > TNCO_RECO.RECO_DAEM  OR TNCO_RECO.RECO_DAAN IS NULL) "
            End If

            SQL += "   AND TNCO_MOLI.LICL_CODI = 1 "
            SQL = SQL & "         AND TNCO_DERE.DERE_TIPO = 1 "
            ' Solo recibos de tipo regularizacion
            SQL = SQL & "         AND TNCO_RECO.RECO_TIPO = 5 "
            SQL = SQL & "  AND TNCO_RECO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"



            SQL += "ORDER BY TNCO_RECO.RECO_ANCI, TNCO_RECO.RECO_CODI "


            Me.DbNewConta.TraerLector(SQL)

            If Me.DbNewConta.mDbLector.HasRows Then
                '   MsgBox("Existen Documentos que han sido Afectados por la Anulación de Pagos ", MsgBoxStyle.Information, "Atención")
            End If

            While Me.DbNewConta.mDbLector.Read

                Linea = Linea + 1



                Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))


                If CType(Me.DbNewConta.mDbLector("TIPOPAGO"), String) = "0" Then
                    EsFormaDePago = False
                Else
                    EsFormaDePago = True
                End If


                If IsDBNull(Me.DbNewConta.mDbLector("DOCU_CRED")) = False Then
                    DocCred = CStr(Me.DbNewConta.mDbLector("DOCU_CRED"))
                Else
                    DocCred = ""
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("DESC_CRED")) = False Then
                    DescCred = CStr(Me.DbNewConta.mDbLector("DESC_CRED"))
                Else
                    DescCred = ""
                End If


                If IsDBNull(Me.DbNewConta.mDbLector("DOCU_DEBI")) = False Then
                    DocDeb = CStr(Me.DbNewConta.mDbLector("DOCU_DEBI"))
                Else
                    DocDeb = ""
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("DESC_DEBI")) = False Then
                    DescDeb = CStr(Me.DbNewConta.mDbLector("DESC_DEBI"))
                Else
                    DescDeb = ""
                End If



                If CStr(Me.DbNewConta.mDbLector("DOCUMENTO")) = "?" Then
                    ' Documento = ""
                    Documento = CStr(Me.DbNewConta.mDbLector("DOCU_DEBI"))
                    Serie = ""
                Else
                    Documento = CStr(Me.DbNewConta.mDbLector("DOCUMENTO"))
                    Serie = CStr(Me.DbNewConta.mDbLector("SERIE"))
                End If








                ' SALE A BUSCAR DATOS DEL MOVIMIENTO DE COBRO


                DatosdeCredito1 = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 1, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))
                DatosdeCredito2 = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 2, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))
                DatosdeCredito6 = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 6, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))


                '20190125
                If DatosdeCredito6 = mCodigoNotasCredito Then
                    Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double) * -1
                Else
                    Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("RECO_DAAN")) = True Then
                    FecAnul = ""
                Else
                    FecAnul = CStr(Me.DbNewConta.mDbLector("RECO_DAAN"))
                End If


                DocCred = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 3, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))


                ' Determinar el Tipo del Recibo ( Si es de Multiples Cobros y Multipples facturas o no ) 


                SQL = "SELECT COUNT(*) AS TOTAL FROM TNCO_DERC WHERE RECO_CODI = " & CInt(Me.DbNewConta.mDbLector.Item("RECO_CODI")) & " AND RECO_ANCI = " & CInt(Me.DbNewConta.mDbLector.Item("RECO_ANCI"))
                If Me.DbNewContaAux.EjecutaSqlScalar(SQL) = "0" Then
                    Multicobro = 0
                Else
                    Multicobro = 1
                End If



                If Total <> 0 Then

                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracleGustavo("AC", 777, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("DESC_DEBI"), String), Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("CODIGO"), String) & " " & CType(Me.DbNewConta.mDbLector("NOMBRE"), String), "SI", CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String).PadRight(50, CChar(" ")), DatosdeCredito2, Documento, Serie, DocCred, DocDeb, DatosdeCredito1, DescDeb, CStr(Me.DbNewConta.mDbLector("RECIBO")), CStr(Me.DbNewConta.mDbLector("RECO_DAEM")), FecAnul, CType(Me.DbNewConta.mDbLector("TOTALF"), Double), CStr(Me.DbNewConta.mDbLector("TACO_CODI")) & "/" & CStr(Me.DbNewConta.mDbLector("MOCO_CODI")), Multicobro, CDate(Format(CDate(Me.DbNewConta.mDbLector("MOCO_DAVA")), "dd/MM/yyyy")))
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("DESC_DEBI"), String), Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()

            '' lo mismo al revez ( deposito anticipado ) =

            SQL = "SELECT  "
            SQL += "  ' COBROS DE UN RECIBO', "
            SQL += "  TNCO_RECO.RECO_ANCI, "
            SQL += "         TNCO_RECO.RECO_CODI, "
            SQL += "         TNCO_TACO.TACO_NOME, "
            SQL += "         TNCO_TACO.TACO_MORA, "
            SQL += "         TNCO_TACO.TACO_CODP, "
            SQL += "         TNCO_TACO.TACO_LOCA, "
            SQL += "         TNCO_TACO.TACO_NUCO, "
            SQL += "         TNCO_DERE.DERE_VARE, "
            SQL += "  TNCO_DERE.DERE_CODI, "
            SQL += "         TNCO_RECO.RECO_ANUL, "
            SQL += "         TNCO_RECO.RECO_VALO, "
            SQL += "         TNCO_MOLI.LICL_CODI, "
            SQL += "         TNCO_MOLI.MOLI_DESC , "
            SQL += "         TNCO_RECO.UNMO_CODI, "
            SQL += "         TNCO_MOCO.MOCO_CAMB, "
            SQL += "         TNCO_RECO.RECO_TIPO, "
            SQL += "         TNCO_RECO.RECO_CONS, "
            SQL += "         TNCO_MOCO.MOCO_DESC, "
            SQL += "         TNCO_MOCO.MOCO_OBSE, "
            SQL += "         TNCO_DERE.DERE_TIPO, "
            SQL += "         TNCO_RECO.RECO_DAEM, "
            SQL += "         TNCO_MOC1.MOCO_DOC1, "
            SQL += "         NVL(TNCO_MOC1.MOCO_DOCU,'?') AS DOCU_DEBI, "
            SQL += "         TNCO_TACO.TACO_MOR1, "
            SQL += "         TNCO_TACO.TACO_MOR2, "
            SQL += "         TNCO_TACO.TACO_MOR3, "
            SQL += "         TNCO_MOCO.MOCO_VAOR, "
            SQL += "         TNCO_MOCO.MOCO_VADE, "
            SQL += "         TNCO_MOCO.MOCO_DAVA, "
            SQL += "         NVL(TNCO_MOCO.MOCO_DOCU,'?') AS MOCO_DOCU, "
            SQL += "         TNCO_DERE.TACO_CODI, "
            SQL += "         TNCO_DERE.MOCO_CODI "


            SQL += "       ,NVL(TIMO_PAGA,'0') AS TIPOPAGO "

            SQL += "       ,'' AS DOCU_CRED "
            SQL += "       ,'' AS DESC_CRED "
            SQL += "       ,'' AS DOCU_DEBI "
            SQL += "       ,'' AS  DESC_DEBI "

            SQL += "  ,    TNCO_DERE.DERE_VARE AS TOTAL       "


            SQL += "     ,   NVL(TNCO_MOCO.FACT_CODI,'?') AS DOCUMENTO "
            SQL += "    ,    NVL(TNCO_MOCO.SEFA_CODI,'?') AS SERIE"
            SQL += "    ,    TNCO_RECO.RECO_DAEM,TNCO_RECO.RECO_DAAN"

            SQL += "     ,NVL(TNCO_TACO.TACO_NUCO,'?') AS NIF,"
            SQL += "       TNCO_TACO.TACO_CODI AS CODIGO, NVL(TACO_NOME,'?') AS NOMBRE "
            SQL += "    ,    NVL(TNCO_MOLI.MOLI_DESC,' ') AS MOLI_DESC "
            SQL = SQL & "  , TNCO_RECO.RECO_CONS || '/' ||  TNCO_RECO.RECO_ANCI AS RECIBO "

            SQL += "     ,   TNCO_MOCO.MOCO_VAOR AS TOTALF"

            SQL += ",TNCO_TIMO.TIMO_CODI "

            SQL += "    FROM TNCO_RECO TNCO_RECO, "
            ' SQL += "         TNCO_DERC TNCO_DERE, "
            SQL += "         TNCO_DERE TNCO_DERE, "
            SQL += "         TNCO_TACO TNCO_TACO, "
            SQL += "         TNCO_MOCO TNCO_MOC1, "
            SQL += "         TNCO_MOCO TNCO_MOCO, "
            SQL += "         TNCO_MOLI TNCO_MOLI, "
            SQL += "         TNCO_TIMO "
            SQL += "   WHERE ( (TNCO_RECO.RECO_CODI = TNCO_DERE.RECO_CODI(+)) "
            SQL += "          AND (TNCO_RECO.RECO_ANCI = TNCO_DERE.RECO_ANCI(+))) "
            SQL += "         AND (TNCO_RECO.TACO_CODI = TNCO_TACO.TACO_CODI) "

            SQL += "         AND ( (TNCO_DERE.TACO_CODI = TNCO_MOC1.TACO_CODI(+)) "
            SQL += "              AND (TNCO_DERE.MOCO_CODI = TNCO_MOC1.MOCO_CODI(+))) "

            SQL += "         AND ( (TNCO_DERE.TACO_CODI = TNCO_MOCO.TACO_CODI) "


            SQL += "              AND (TNCO_DERE.MOCO_CODI = TNCO_MOCO.MOCO_CODI)) "
            SQL += "         AND (TNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI) "
            SQL += "  AND TNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "


            SQL = SQL & "  AND TNCO_RECO.RECO_DAAN = '" & Me.mFecha & "'"


            If Me.mTrataAnulacionesNewConta = 0 Then
                SQL = SQL & "  AND (TNCO_RECO.RECO_DAAN > TNCO_RECO.RECO_DAEM  OR TNCO_RECO.RECO_DAAN IS NULL) "
            End If

            SQL += "   AND TNCO_MOLI.LICL_CODI = 1 "
            SQL = SQL & "         AND TNCO_DERE.DERE_TIPO = 1 "
            ' Solo recibos de tipo regularizacion
            SQL = SQL & "         AND TNCO_RECO.RECO_TIPO = 5 "
            SQL = SQL & "  AND TNCO_RECO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"



            SQL += "ORDER BY TNCO_RECO.RECO_ANCI, TNCO_RECO.RECO_CODI "






            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                Linea = Linea + 1

                Cuenta = BuscaCuentaPagosAnticipadosNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))

                ' Si el documento o parte del documento se saldo con una nota de credito el importe va negativo

                If CType(Me.DbNewConta.mDbLector("TIPOPAGO"), String) = "0" Then
                    EsFormaDePago = False
                Else
                    EsFormaDePago = True
                End If
                If IsDBNull(Me.DbNewConta.mDbLector("DOCU_CRED")) = False Then
                    DocCred = CStr(Me.DbNewConta.mDbLector("DOCU_CRED"))
                Else
                    DocCred = ""
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("DESC_CRED")) = False Then
                    DescCred = CStr(Me.DbNewConta.mDbLector("DESC_CRED"))
                Else
                    DescCred = ""
                End If


                If IsDBNull(Me.DbNewConta.mDbLector("DOCU_DEBI")) = False Then
                    DocDeb = CStr(Me.DbNewConta.mDbLector("DOCU_DEBI"))
                Else
                    DocDeb = ""
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("DESC_DEBI")) = False Then
                    DescDeb = CStr(Me.DbNewConta.mDbLector("DESC_DEBI"))
                Else
                    DescDeb = ""
                End If



                '20190125
                If CStr(Me.DbNewConta.mDbLector("TIMO_CODI")) = mCodigoNotasCredito Then
                    Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double) * -1
                Else
                    Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                End If


                DatosdeCredito1 = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 1, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))
                DatosdeCredito2 = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 2, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))
                DatosdeCredito4 = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 4, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))
                DatosdeCredito5 = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 5, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))
                DatosdeCredito6 = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 6, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))


                '20190125
                If DatosdeCredito6 = mCodigoNotasCredito Then
                    Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double) * -1
                Else
                    Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                End If

                If IsDBNull(Me.DbNewConta.mDbLector("RECO_DAAN")) = True Then
                    FecAnul = ""
                Else
                    FecAnul = CStr(Me.DbNewConta.mDbLector("RECO_DAAN"))
                End If


                If DatosdeCredito1.Length > 0 Then
                    Me.mtextoApunte = DatosdeCredito1
                Else
                    Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                End If
                ' TEST


                DocCred = BuscaDatosdeCobrodeunaFactura(CInt(Me.DbNewConta.mDbLector("RECO_CODI")), CInt(Me.DbNewConta.mDbLector("RECO_ANCI")), 3, CInt(Me.DbNewConta.mDbLector("DERE_CODI")))


                ' Determinar el Tipo del Recibo ( Si es de Multiples Cobros y Multipples facturas o no ) 


                SQL = "SELECT COUNT(*) AS TOTAL FROM TNCO_DERC WHERE RECO_CODI = " & CInt(Me.DbNewConta.mDbLector.Item("RECO_CODI")) & " AND RECO_ANCI = " & CInt(Me.DbNewConta.mDbLector.Item("RECO_ANCI"))
                If Me.DbNewContaAux.EjecutaSqlScalar(SQL) = "0" Then
                    Multicobro = 0
                Else
                    Multicobro = 1
                End If


                If Total <> 0 Then

                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 777, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Me.mtextoApunte, Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("CODIGO"), String) & " " & CType(Me.DbNewConta.mDbLector("MOCO_DOCU"), String), "SI", DatosdeCredito4, "", DocCred, DocDeb, DescCred, DescDeb, CStr(Me.DbNewConta.mDbLector("RECIBO")), CStr(Me.DbNewConta.mDbLector("RECO_DAEM")), FecAnul, CType(Me.DbNewConta.mDbLector("TOTALF"), Double), DatosdeCredito5, Multicobro, CDate(Format(CDate(Me.DbNewConta.mDbLector("MOCO_DAVA")), "dd/MM/yyyy")))
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Me.mtextoApunte, Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try

    End Sub

    Private Function DameDocumentodeCobro(ByVal vReco_Anci As Integer, ByVal vRecoCodi As Integer, ByVal vTacoCod2 As String, ByVal vMocoCod2 As Integer, ByVal vValor As Double, ByVal vDereCodi As Integer, ByVal vFechaDaem As Date) As String
        Try
            Dim Result As String = ""
            Dim Tipo As String



            Me.mEstaEnDerc = False
            Me.mContarDere = 0

            SQL = "SELECT COUNT(*) AS TOTAL FROM TNCO_DERC WHERE RECO_CODI = " & vRecoCodi & " AND RECO_ANCI = " & vReco_Anci
            If Me.DbNewContaAux.EjecutaSqlScalar(SQL) = "0" Then
                Me.mEstaEnDerc = False
                Me.mContarDere = 0

            Else
                Me.mEstaEnDerc = True
                SQL = "SELECT  COUNT(DISTINCT MOCO_CODI) AS TOTAL FROM TNCO_DERE WHERE RECO_CODI = " & vRecoCodi & " AND RECO_ANCI = " & vReco_Anci
                Me.mContarDere = CInt(Me.DbNewContaAux.EjecutaSqlScalar(SQL))
            End If

            ' tiene mas de un cobro y mas de una factura 
            If Me.mEstaEnDerc = True Then
                ' If Me.mContarDere > 1 Then

                If Me.mTrataMultiCobro = False Then
                    '   Return "multiple"
                    Return ""
                Else
                    Return Me.DevuelveCobroAsociacionInterna(vRecoCodi, vReco_Anci, vDereCodi)
                End If

                'End If
            End If

            SQL = "SELECT MOCO_COD1,TACO_COD1 FROM TNCO_MORE WHERE "
            SQL += " MOCO_COD2 = " & vMocoCod2
            SQL += " AND TACO_COD2 = " & vTacoCod2
            '  SQL += " AND MORE_DARE = '" & Me.mFecha & "'"
            SQL += " AND MORE_DARE = '" & vFechaDaem & "'"
            ' TRATA FILTAR SI UNA FACTURA SE HA COBRADO EN EL MISMO DIA CON VARIOS COBROS 
            SQL += " AND MORE_VAPA = " & vValor

            Me.DbNewContaAux.TraerLector(SQL)

            While Me.DbNewContaAux.mDbLector.Read

                SQL = "SELECT NVL(MOCO_DOCU,'') FROM TNCO_MOCO WHERE MOCO_CODI = " & CInt(Me.DbNewContaAux.mDbLector.Item("MOCO_COD1"))
                SQL += " AND TACO_CODI = '" & CStr(Me.DbNewContaAux.mDbLector.Item("TACO_COD1")) & "'"
                Result = Me.DbNewContaAux2.EjecutaSqlScalar(SQL)

                SQL = "SELECT TIMO_CODI FROM TNCO_MOCO WHERE MOCO_CODI = " & CInt(Me.DbNewContaAux.mDbLector.Item("MOCO_COD1"))
                SQL += " AND TACO_CODI = '" & CStr(Me.DbNewContaAux.mDbLector.Item("TACO_COD1")) & "'"
                Tipo = Me.DbNewContaAux2.EjecutaSqlScalar(SQL)



                ' QUITAR  LAS BARRAS SI SE COBRO CON FACTURAS O NOTAS D ECREDITO
                If Tipo = Me.mCodigoNotasCredito Then
                    If IsDBNull(Result) = False Then
                        Dim ArrayDoc() As String = Split(Result, "/")
                        Result = ArrayDoc(0)
                    Else
                        Result = ""
                    End If

                ElseIf Tipo = Me.mCodigoFacturas Then
                    If IsDBNull(Result) = False Then
                        Dim ArrayDoc() As String = Split(Result, "/")
                        Result = ArrayDoc(0)
                    Else
                        Result = ""
                    End If


                End If




            End While
            Me.DbNewContaAux.mDbLector.Close()

            Return Result


        Catch ex As Exception
            MsgBox(ex.Message)
            Return ""
        End Try


    End Function
    Private Function DevuelveCobroAsociacionInterna(ByVal vRecoCodi As Integer, ByVal vRecoAnci As Integer, ByVal vDereCodi As Integer) As String
        Try

            Dim Result As String = ""
            Dim Tipo As String = ""
            Dim Documento As String = ""
            Dim DocumentoAux As String = ""
            Dim Descripcion As String = ""
            Dim Cuenta As String = ""

            '1 Determinar el Tipo de Cobro 
            SQL = "SELECT TIMO_CODI FROM QWE_MULTICOBROS "
            SQL += " WHERE RECO_CODI = " & vRecoCodi
            SQL += " AND RECO_ANCI = " & vRecoAnci
            SQL += " AND TIPOC = 'CREDITO' "
            SQL += " AND CONTROL  = " & vDereCodi

            Tipo = Me.DbNewContaAux2.EjecutaSqlScalar(SQL)

            '2 Determinar el numero de documento del cobro
            SQL = "SELECT MOCO_DOCU FROM QWE_MULTICOBROS "
            SQL += " WHERE RECO_CODI = " & vRecoCodi
            SQL += " AND RECO_ANCI = " & vRecoAnci
            SQL += " AND TIPOC = 'CREDITO' "
            SQL += " AND CONTROL  = " & vDereCodi

            DocumentoAux = Me.DbNewContaAux2.EjecutaSqlScalar(SQL)


            '1 Determinar LA CUENTA DEl Tipo de Cobro 
            SQL = "SELECT TIMO_COCO FROM QWE_MULTICOBROS "
            SQL += " WHERE RECO_CODI = " & vRecoCodi
            SQL += " AND RECO_ANCI = " & vRecoAnci
            SQL += " AND TIPOC = 'CREDITO' "
            SQL += " AND CONTROL  = " & vDereCodi

            Cuenta = Me.DbNewContaAux2.EjecutaSqlScalar(SQL)




            ' QUITAR  LAS BARRAS SI SE COBRO CON FACTURAS O NOTAS D ECREDITO
            If Tipo = Me.mCodigoNotasCredito Then
                If IsDBNull(DocumentoAux) = False Then
                    Dim ArrayDoc() As String = Split(DocumentoAux, "/")
                    Documento = ArrayDoc(0)
                Else
                    Documento = ""
                End If

            ElseIf Tipo = Me.mCodigoFacturas Then
                If IsDBNull(DocumentoAux) = False Then
                    Dim ArrayDoc() As String = Split(DocumentoAux, "/")
                    Documento = ArrayDoc(0)
                Else
                    Documento = ""
                End If
            Else
                Documento = DocumentoAux
            End If


            '3 Determinar la descripcion del cobro
            SQL = "SELECT MOCO_DESC FROM QWE_MULTICOBROS "
            SQL += " WHERE RECO_CODI = " & vRecoCodi
            SQL += " AND RECO_ANCI = " & vRecoAnci
            SQL += " AND TIPOC = 'CREDITO' "
            SQL += " AND CONTROL  = " & vDereCodi

            Descripcion = Me.DbNewContaAux2.EjecutaSqlScalar(SQL)


            If Tipo = Me.mCodigoNotasCredito Or Tipo = Me.mCodigoFacturas Then
                Return Documento & "|" & Tipo & " + " & Descripcion
            Else
                Return Documento & "|" & Tipo & " " & Cuenta & " + " & Descripcion
            End If



        Catch ex As Exception
            MsgBox(ex.Message)
            Return ""
        End Try
    End Function



    Private Function EstaElCobroenelrecibo(ByVal vMocoCodi As Integer, ByVal vTacoCodi As String, ByVal vRecoCodi As Integer, ByVal vRecoAnci As Integer) As Boolean

        Try


            SQL = "SELECT TO_CHAR (RECO_CONS) || '/' || TO_CHAR (RECO_ANCI) RECIBO, "
            SQL += "       RECO_CODI, "
            SQL += "       RECO_ANCI, "
            SQL += "       TRUNC (RECO_VALO, 2) RECO_VALO, "
            SQL += "       TNCO_RECO.TACO_CODI || '/' || TO_CHAR (MOCO_CODI) MOVIMIENTO, "
            SQL += "       UNMO_CODI, "
            SQL += "       RECO_DAEM, "
            SQL += "       RECO_ANUL, "
            SQL += "       RECO_TIPO TIPO, "
            SQL += "       DECODE (0, "
            SQL += "               0, DOMI_DES0, "
            SQL += "               1, DOMI_DES1, "
            SQL += "               2, DOMI_DES2, "
            SQL += "               3, DOMI_DES3, "
            SQL += "               DOMI_DES0) "
            SQL += "          RECO_TIPO, "
            SQL += "       1 IMPRIMIR, "
            SQL += "       TNCO_NACI.LICL_CODI "
            SQL += "  FROM TNCO_RECO, "
            SQL += "       TNCO_TACO, "
            SQL += "       TNCO_NACI, "
            SQL += "       TNCO_DOMI "
            SQL += " WHERE     TNCO_RECO.TACO_CODI = TNCO_TACO.TACO_CODI "
            SQL += "       AND TNCO_TACO.NACI_CODI = TNCO_NACI.NACI_CODI(+) "
            SQL += "       AND RECO_TIPO = DOMI_ABRE "
            SQL += "       AND DOMI_ENUM = 'TipoRecibo' "
            SQL += "       AND ( ( (RECO_CODI, RECO_ANCI) IN "
            SQL += "                 (SELECT RECO_CODI, RECO_ANCI "
            SQL += "                    FROM TNCO_DERC "
            SQL += "                   WHERE TACO_CODI = '" & vTacoCodi & "'"

            SQL += " AND MOCO_CODI = " & vMocoCodi
            SQL += ")) "
            SQL += ""

            SQL += "            OR ( (MOCO_CODI = " & vMocoCodi

            SQL += " ) AND TNCO_RECO.TACO_CODI = '" & vTacoCodi & "'"
            SQL += "))"

            SQL += "AND RECO_CODI = " & vRecoCodi
            SQL += "AND RECO_ANCI = " & vRecoAnci





            If Me.DbNewContaAux2.EjecutaSqlScalar(SQL) = Nothing Then
                Return False
            Else
                Return True
            End If


        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        End Try

    End Function
    Private Function EstaEnTNHT_DERE2(ByVal vMocoCodi As Integer, ByVal vTacoCodi As String, ByVal vRecoCodi As Integer, ByVal vRecoAnci As Integer) As Boolean

        Try

            SQL = "SELECT TNCO_RECO.RECO_ANCI, "
            SQL += "         TNCO_RECO.RECO_CODI, "
            SQL += "         TNCO_TACO.TACO_NOME, "
            SQL += "       "
            SQL += "       "
            SQL += "         TNCO_DERE.DERE_VARE, "
            SQL += "         TNCO_RECO.RECO_ANUL, "
            SQL += "         TNCO_RECO.RECO_VALO, "
            SQL += "         TNCO_MOLI.LICL_CODI, "
            SQL += "         TNCO_MOLI.MOLI_DESC, "
            SQL += "         TNCO_RECO.UNMO_CODI, "
            SQL += "       "
            SQL += "         TNCO_RECO.RECO_TIPO, "
            SQL += "         TNCO_RECO.RECO_CONS, "
            SQL += "         TNCO_MOCO.MOCO_DESC, "
            SQL += "         TNCO_MOCO.MOCO_OBSE, "
            SQL += "         TNCO_DERE.DERE_TIPO, "
            SQL += "         TNCO_RECO.RECO_DAEM, "
            SQL += "         TNCO_MOC1.MOCO_DOC1, "
            SQL += "         TNCO_MOC1.MOCO_DOCU, "
            SQL += "         TNCO_TACO.TACO_MOR1, "
            SQL += "         TNCO_TACO.TACO_MOR2, "
            SQL += "         TNCO_TACO.TACO_MOR3, "
            SQL += "         TNCO_MOCO.MOCO_VAOR, "
            SQL += "         TNCO_MOCO.MOCO_VADE, "
            SQL += "         TNCO_MOCO.MOCO_DOCU, "
            SQL += "         TNCO_DERE.TACO_CODI, "
            SQL += "         TNCO_DERE.MOCO_CODI "
            SQL += "    FROM TNCO_RECO TNCO_RECO, "
            SQL += "         TNCO_DERC TNCO_DERE, "
            SQL += "         TNCO_TACO TNCO_TACO, "
            SQL += "         TNCO_MOCO TNCO_MOC1, "
            SQL += "         TNCO_MOCO TNCO_MOCO, "
            SQL += "         TNCO_MOLI TNCO_MOLI "
            SQL += "          "
            SQL += "   WHERE ( (TNCO_RECO.RECO_CODI = TNCO_DERE.RECO_CODI(+)) "
            SQL += "          AND (TNCO_RECO.RECO_ANCI = TNCO_DERE.RECO_ANCI(+))) "
            SQL += "           "
            SQL += "           "
            SQL += "         AND (TNCO_RECO.TACO_CODI = TNCO_TACO.TACO_CODI) "
            SQL += "         AND ( (TNCO_DERE.TACO_CODI = TNCO_MOC1.TACO_CODI(+)) "
            SQL += "              AND (TNCO_DERE.MOCO_CODI = TNCO_MOC1.MOCO_CODI(+))) "
            SQL += "         AND ( (TNCO_DERE.TACO_CODI = TNCO_MOCO.TACO_CODI) "
            SQL += "              AND (TNCO_DERE.MOCO_CODI = TNCO_MOCO.MOCO_CODI)) "
            SQL += "               "
            SQL += "               "
            SQL += "               "
            SQL += "               "
            SQL += "               "
            SQL += "               "
            SQL += "         AND (TNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI) "
            SQL += "         AND TNCO_RECO.RECO_CODI =  " & vRecoCodi
            SQL += "         AND TNCO_RECO.RECO_ANCI =  " & vRecoAnci
            SQL += "         AND TNCO_MOLI.LICL_CODI = 1 "
            SQL += "ORDER BY TNCO_RECO.RECO_ANCI, TNCO_RECO.RECO_CODI "




            Me.DbNewContaAux2.TraerLector(SQL)
            While Me.DbNewContaAux2.mDbLector.Read

                If CStr(Me.DbNewContaAux2.mDbLector.Item("TACO_CODI")) = vTacoCodi And CInt(Me.DbNewContaAux2.mDbLector.Item("MOCO_CODI")) = vMocoCodi Then
                    Me.DbNewContaAux2.mDbLector.Close()
                    Return True
                End If
            End While

            Me.DbNewContaAux2.mDbLector.Close()
            Return False





        Catch ex As Exception
            MsgBox(ex.Message)
            Return False 
        End Try

    End Function

    ''' <summary>
    '''   
    ''' </summary>
    ''' <param name="vfecha"></param>
    ''' <param name="vDocumento"></param>
    ''' <param name="vRecibo"></param>
    ''' <param name="vImporte"></param>
    ''' <param name="vCabrefer"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function EstaDublicadoElRecibo(ByVal vfecha As Date, ByVal vDocumento As String, ByVal vRecibo As String, ByVal vImporte As Double, ByVal vCabrefer As Integer) As Boolean

        Try


            Dim Texto As String

            Dim Mostrarmensaje As Boolean = False


            If Me.AuxRecibo = "" Then
                Me.AuxRecibo = vRecibo
            End If



            SQL = "SELECT ASNT_RECIBO ,nvl(ASNT_I_MONEMP,0) as ASNT_I_MONEMP ,nvl(ASNT_TOTAL_FACTURA,0)  as ASNT_TOTAL_FACTURA FROM TC_ASNT WHERE ASNT_F_ATOCAB = '" & vfecha & "'"

            SQL += " AND ASNT_DOC_DEBITO = '" & vDocumento & "'"
            SQL += " AND ASNT_RECIBO  <>  '" & vRecibo & "'"
            SQL += " AND ASNT_CFATOCAB_REFER = " & vCabrefer
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum

            Me.DbLeeCentral.TraerLector(SQL)

            While Me.DbLeeCentral.mDbLector.Read

                ' If CDbl(Me.DbLeeCentral.mDbLector.Item("ASNT_I_MONEMP")) + vImporte > CDbl(Me.DbLeeCentral.mDbLector.Item("ASNT_TOTAL_FACTURA")) Then
                If Math.Round(CDbl(Me.DbLeeCentral.mDbLector.Item("ASNT_I_MONEMP")) + vImporte, 2) > CDbl(Me.DbLeeCentral.mDbLector.Item("ASNT_TOTAL_FACTURA")) Then
                    Texto = "Localizado recibo Nùmero " & CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_RECIBO")) & " por valor de " & CDbl(Me.DbLeeCentral.mDbLector.Item("ASNT_I_MONEMP")) & " Total Factura = " & CDbl(Me.DbLeeCentral.mDbLector.Item("ASNT_TOTAL_FACTURA")) & vbCrLf & vbCrLf
                    Texto += " Se intenta Grabar Recibo Número " & vRecibo & " por importe de " & vImporte
                    If Me.AuxRecibo <> vRecibo Then
                        Me.DbLeeCentral.mDbLector.Close()
                        '    MsgBox(Texto, MsgBoxStyle.Exclamation, "Exceso de Liquidación")
                        ' GRABA INCIDENCIA
                        Me.mListBoxDebug.Items.Add(Texto)
                        SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                        SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vCabrefer & "," & Linea & ",'" & Texto & "')"
                        Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                        Return True
                    End If
                End If


            End While

            Me.AuxRecibo = vRecibo

            Me.DbLeeCentral.mDbLector.Close()
            Return False



        Catch ex As Exception
            MsgBox(ex.Message)
            Return True
        End Try
    End Function
    Private Function EstaContabilizadoElRecibo(ByVal vfechaEmision As Date, ByVal vDocumento As String, ByVal vRecibo As String, ByVal vImporte As Double, ByVal vCabrefer As Integer) As Boolean

        Try


            Dim Texto As String

            Dim Mostrarmensaje As Boolean = False


            If Me.AuxRecibo = "" Then
                Me.AuxRecibo = vRecibo
            End If



            SQL = "SELECT ASNT_RECIBO ,nvl(ASNT_I_MONEMP,0) as ASNT_I_MONEMP ,nvl(ASNT_TOTAL_FACTURA,0)  as ASNT_TOTAL_FACTURA FROM TC_ASNT WHERE ASNT_F_ATOCAB = '" & vfechaEmision & "'"

            SQL += " AND ASNT_DOC_DEBITO = '" & vDocumento & "'"
            SQL += " AND ASNT_RECIBO  =  '" & vRecibo & "'"
            SQL += " AND ASNT_CFATOCAB_REFER = " & vCabrefer
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum

            Me.DbLeeCentral.TraerLector(SQL)

            While Me.DbLeeCentral.mDbLector.Read
                Me.DbLeeCentral.mDbLector.Close()
                Return True
            End While




            If Me.AuxRecibo <> vRecibo Then
                Texto = "No Localizado recibo Nùmero " & vRecibo & " por lo tanto NO SE CONTABILIZA SU ANULACIÓN"
                Me.DbLeeCentral.mDbLector.Close()

                ' GRABA INCIDENCIA
                Me.mListBoxDebug.Items.Add(Texto)
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vCabrefer & "," & Linea & ",'" & Texto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

                Me.AuxRecibo = vRecibo

            End If


            Return False



        Catch ex As Exception
            MsgBox(ex.Message)
            Return True
        End Try
    End Function
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
                    Me.InsertaOracle("AC", 444, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " +  " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, "NO", "", CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", Me.mFecha, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String))
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()

            '' lo mismo al revez ( deposito anticipado ) 

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
                Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))

                '   Cuenta = "Pendiente"


                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 444, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", Me.mFecha, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String))
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total)
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
            Dim TipoMovimientoPagodeMas As String


            SQL = "SELECT PACO_FACT FROM TNCO_PACO"
            TipoMovimientoFacturas = Me.DbNewContaAux.EjecutaSqlScalar(SQL)


            SQL = "SELECT PACO_PAAS FROM TNCO_PACO"
            TipoMovimientoPagodeMas = Me.DbNewContaAux.EjecutaSqlScalar(SQL)

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

            'SQL += " AND VNCO_MOCO.TIMO_CODI <> '" & TipoMovimientoFacturas & "'"
            SQL += " AND VNCO_MOCO.TIMO_CODI NOT IN  ('" & TipoMovimientoFacturas & "','" & TipoMovimientoPagodeMas & "')"

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
                    Me.InsertaOracle("AC", 555, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " +  " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total, "NO", "", CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", Me.mFecha, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String))
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " + " & CType(Me.DbNewConta.mDbLector("MOCO_DESC"), String), Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()

            '' lo mismo al revez (  ) 

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

            ' SQL += " AND VNCO_MOCO.TIMO_CODI <> '" & TipoMovimientoFacturas & "'"
            SQL += " AND VNCO_MOCO.TIMO_CODI NOT IN  ('" & TipoMovimientoFacturas & "','" & TipoMovimientoPagodeMas & "')"

            SQL += "     AND VNCO_MOCO.MOCO_DATR= '" & Me.mFecha & "'"

            SQL += "     AND VNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            SQL += "    ORDER BY VNCO_MOCO.MOCO_DAVA DESC, VNCO_MOCO.MOCO_ANUL "


            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_CODI"), String))


                Linea = Linea + 1
                Total = CType(Me.DbNewConta.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 555, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, "*** " & CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("TACO_CODI"), String) & " " & CType(Me.DbNewConta.mDbLector("TACO_NOME"), String), "SI", Me.mFecha, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String))
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String), Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try


    End Sub
#End Region

#Region "ASIENTO 333 FACTURAS REGULARIZADAS LOPEZ"
    Private Sub NCFacturasRegularizadasLopez()
        Try
            Dim Total As Double
            Dim Cuenta As String
            Linea = 0
            Dim EsFormaDePago As Boolean = False

            Dim Recibo As String = ""

            Dim AuxiliarTipoMovimiento As String

            Dim TipoAsiento As Integer


            SQL = "SELECT '1', /*+ INDEX(TNCO_MORE TNCO_MORE_PRIMARY) */ TAC1.TACO_CODA "
            SQL += "                                                                    TACO_DACR, "
            SQL += "       TAC1.TACO_COCO TACO_COCR , TAC2.TACO_CODA TACO_DADB, "
            SQL += "       TAC2.TACO_COCO TACO_CODB, MOCO.MOCO_CODI MOCO_DEBI, "
            SQL += "       MOCO.TIMO_CODI TIMO_DEBI, MOCO.MOCO_DOCU DOCU_DEBI, "
            SQL += "       NVL(MOCO.MOCO_DESC,'?') DESC_DEBI, MOC1.MOCO_CODI MOCO_CRED, "
            SQL += "       MOC1.TIMO_CODI TIMO_CRED, MOC1.MOCO_DOCU DOCU_CRED, "
            SQL += "       NVL(MOC1.MOCO_DESC,NVL(TNCO_MOLI2.MOLI_DESC, ' ')) DESC_CRED, MORE_DARE, MORE_VAPA AS TOTAL, MORE_DEAN, "
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

            SQL += "        , MOCO.MOCO_DAVA AS DAVADEB "
            SQL += "        , MOC1.MOCO_DAVA AS DAVACRE "
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

            If Me.mParaFActAnul2 = 0 Then
                SQL += "    AND (  MOC1.SEFA_CODI NOT  IN ( "
                SQL += "        SELECT "
                SQL += "            SEFA_CODI "
                SQL += "        FROM " & Me.mParaUserNewHotel & ".TNHT_SEFA "

                SQL += "        WHERE "
                SQL += "            SEFA_TIFA = '6' "
                SQL += "    ) OR MOC1.SEFA_CODI IS NULL) "



            End If



            ' segun chany/edelia no se tratan notas de credito 
            '  SQL += "   AND MOC1.TIMO_CODI <> '" & Me.mCodigoNotasCredito & "'"

            SQL += "  AND MORE_DARE = '" & Me.mFecha & "'"

            ' Excluye Operaciones Realizadas y Anuladas el mismo dia 
            SQL += "  AND (TNCO_MORE.MORE_DAAN > TNCO_MORE.MORE_DARE  OR TNCO_MORE.MORE_DAAN IS NULL) "

            SQL += "  AND MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"


            SQL += " ORDER BY SEFA_CODI,FACT_CODI"



            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                Linea = Linea + 1

                Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_DEBI"), String))

                If CType(Me.DbNewConta.mDbLector("TIPOPAGO"), String) = "0" Then
                    '  TipoAsiento = 334
                    TipoAsiento = 333
                    EsFormaDePago = False
                    AuxiliarTipoMovimiento = CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " (-)"
                Else
                    EsFormaDePago = True
                    TipoAsiento = 333
                    AuxiliarTipoMovimiento = CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String)
                End If


                ' Si el documento o parte del documento se saldo con una nota de credito el importe va negativo
                If CStr(Me.DbNewConta.mDbLector("TIMO_CRED")) = mCodigoNotasCredito Then
                    '    Total = Math.Round(CType(Me.DbNewConta.mDbLector("TOTAL"), Double), 2) * -1
                    Total = Math.Round(CType(Me.DbNewConta.mDbLector("TOTAL"), Double), 2)
                Else
                    Total = Math.Round(CType(Me.DbNewConta.mDbLector("TOTAL"), Double), 2)
                End If


                Recibo = BuscaReciboDeRegularizacion(CStr(Me.DbNewConta.mDbLector("TACO_DEBI")), CInt(Me.DbNewConta.mDbLector("MOCO_DEBI")), Total, 0)


                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"


                    Me.InsertaOracleLopez("AC", TipoAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("DESC_DEBI"), String), Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("CODIGO"), String) & " " & CType(Me.DbNewConta.mDbLector("NOMBRE"), String), "SI", AuxiliarTipoMovimiento, "  Pagada con : " & CType(Me.DbNewConta.mDbLector("DESC_CRED"), String), CStr(Me.DbNewConta.mDbLector("MOCO_CRED")), "", CDate(Format(CDate(Me.DbNewConta.mDbLector("MORE_DARE")), "dd/MM/yyyy")), Recibo)
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
            SQL += "        NVL(MOC1.MOCO_DESC,NVL(TNCO_MOLI.MOLI_DESC, ' ')) DESC_CRED, MORE_DARE, MORE_VAPA AS TOTAL, MORE_DEAN, "
            SQL += "       TIMO_DEBI.TIMO_CDIA DEBI_CDIA, TIMO_DEBI.TIMO_CDOC DEBI_CDOC, "
            SQL += "       TIMO_CRED.TIMO_CDIA CRED_CDIA, TIMO_CRED.TIMO_CDOC CRED_CDOC, "
            SQL += "       MOCO.TACO_CODI TACO_DEBI, MOC1.TACO_CODI TACO_CRED ,"
            '  SQL += "       NVL(TIMO_CRED.TIMO_COCO,'0')  AS CUENTA "
            SQL += "        NVL(TAC1.TACO_CODA,'0') AS CUENTA ,"
            SQL += "       TAC1.TACO_CODI AS CODIGO, NVL(TAC1.TACO_NOME,'?') AS NOMBRE ,NVL(TAC1.TACO_NUCO,'?') AS NIF"

            SQL += "       ,NVL(TIMO_CRED.TIMO_PAGA,'0') AS TIPOPAGO , "
            SQL += "        NVL(MOLI_DESC,' ') AS MOLI_DESC"


            SQL += "        ,MOC1.MOCO_CODI"
            SQL += "        ,NVL(MOCO.FACT_CODI,'0') AS FACT_CODI "
            SQL += "        ,MOCO.SEFA_CODI"

            SQL += "        , MOCO.MOCO_DAVA AS DAVADEB "
            SQL += "        , MOC1.MOCO_DAVA AS DAVACRE "
            SQL += "        , MOC1.MOCO_DESC  "
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


            If Me.mParaFActAnul2 = 0 Then
                SQL += "    AND (  MOC1.SEFA_CODI NOT  IN ( "
                SQL += "        SELECT "
                SQL += "            SEFA_CODI "
                SQL += "        FROM " & Me.mParaUserNewHotel & ".TNHT_SEFA "

                SQL += "        WHERE "
                SQL += "            SEFA_TIFA = '6' "
                SQL += "    ) OR MOC1.SEFA_CODI IS NULL) "

            End If


            ' segun chany/edelia no se tratan notas de credito 
            '    SQL += "   AND MOC1.TIMO_CODI <> '" & Me.mCodigoNotasCredito & "'"

            SQL += "  AND MORE_DARE = '" & Me.mFecha & "'"

            ' Excluye Operaciones Realizadas y Anuladas el mismo dia 
            SQL += "  AND (TNCO_MORE.MORE_DAAN > TNCO_MORE.MORE_DARE  OR TNCO_MORE.MORE_DAAN IS NULL) "


            SQL += "  AND MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            SQL += " ORDER BY SEFA_CODI,FACT_CODI"

            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                Linea = Linea + 1
                Cuenta = BuscaCuentaPagosAnticipadosNewHotel(CType(Me.DbNewConta.mDbLector("CODIGO"), String))




                If Me.mParaTextosFijos = 1 Then
                    If CStr(Me.DbNewConta.mDbLector("TIMO_CRED")) = mCodigoNotasCredito Then
                        Me.mtextoApunte = StrConv(CStr(Me.DbNewConta.mDbLector("DESC_CRED")), VbStrConv.ProperCase)
                    Else
                        Me.mtextoApunte = Mid(StrConv(CStr(Me.DbNewConta.mDbLector("MOLI_DESC")), VbStrConv.ProperCase), 1, 8) & "  " & StrConv(CStr(Me.DbNewConta.mDbLector("NOMBRE")), VbStrConv.ProperCase)
                    End If
                Else
                    If CStr(Me.DbNewConta.mDbLector("TIMO_CRED")) = mCodigoNotasCredito Then
                        Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("DESC_CRED"))
                    ElseIf IsDBNull(Me.DbNewConta.mDbLector("MOCO_DESC")) = True Then
                        Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOLI_DESC"))
                    Else
                        Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOLI_DESC")) & " ," & CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                    End If
                End If


                If CType(Me.DbNewConta.mDbLector("TIPOPAGO"), String) = "0" Then
                    '  TipoAsiento = 334
                    TipoAsiento = 333
                    EsFormaDePago = False
                    AuxiliarTipoMovimiento = CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " (-)"
                Else
                    EsFormaDePago = True
                    TipoAsiento = 333
                    AuxiliarTipoMovimiento = CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String)
                End If

                ' Si el documento o parte del documento se saldo con una nota de credito el importe va negativo
                If CStr(Me.DbNewConta.mDbLector("TIMO_CRED")) = mCodigoNotasCredito Then
                    '   Total = Math.Round(CType(Me.DbNewConta.mDbLector("TOTAL"), Double), 2) * -1
                    Total = Math.Round(CType(Me.DbNewConta.mDbLector("TOTAL"), Double), 2)
                Else
                    Total = Math.Round(CType(Me.DbNewConta.mDbLector("TOTAL"), Double), 2)
                End If

                Recibo = BuscaReciboDeRegularizacion(CStr(Me.DbNewConta.mDbLector("TACO_DEBI")), CInt(Me.DbNewConta.mDbLector("MOCO_DEBI")), Total, 0)

                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracleLopez("AC", TipoAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Me.mtextoApunte, Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("CODIGO"), String) & " " & CType(Me.DbNewConta.mDbLector("NOMBRE"), String), "SI", AuxiliarTipoMovimiento, "  Paga a : " & CType(Me.DbNewConta.mDbLector("DESC_DEBI"), String), CStr(Me.DbNewConta.mDbLector("MOCO_CRED")), "", CDate(Format(CDate(Me.DbNewConta.mDbLector("MORE_DARE")), "dd/MM/yyyy")), Recibo)
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Me.mtextoApunte, Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try


    End Sub
    Private Sub NCFacturasRegularizadasAnuladasLopez()
        Try
            Dim Total As Double
            Dim Cuenta As String
            Linea = 0
            Dim EsFormaDePago As Boolean = False

            Dim Recibo As String
            Dim AuxiliarTipoMovimiento As String
            Dim TipoAsiento As Integer


            SQL = "SELECT '1', /*+ INDEX(TNCO_MORE TNCO_MORE_PRIMARY) */ TAC1.TACO_CODA "
            SQL += "                                                                    TACO_DACR, "
            SQL += "       TAC1.TACO_COCO TACO_COCR , TAC2.TACO_CODA TACO_DADB, "
            SQL += "       TAC2.TACO_COCO TACO_CODB, MOCO.MOCO_CODI MOCO_DEBI, "
            SQL += "       MOCO.TIMO_CODI TIMO_DEBI, MOCO.MOCO_DOCU DOCU_DEBI, "
            SQL += "       NVL(MOCO.MOCO_DESC,'?') DESC_DEBI, MOC1.MOCO_CODI MOCO_CRED, "
            SQL += "       MOC1.TIMO_CODI TIMO_CRED, MOC1.MOCO_DOCU DOCU_CRED, "
            SQL += "      NVL(MOC1.MOCO_DESC,NVL(TNCO_MOLI2.MOLI_DESC, ' ')) DESC_CRED, MORE_DARE, MORE_VAPA AS TOTAL, MORE_DEAN, "
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

            SQL += "        , MOCO.MOCO_DAVA AS DAVADEB "
            SQL += "        , MOC1.MOCO_DAVA AS DAVACRE "

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


            If Me.mParaFActAnul2 = 0 Then
                SQL += "    AND (  MOC1.SEFA_CODI NOT  IN ( "
                SQL += "        SELECT "
                SQL += "            SEFA_CODI "
                SQL += "        FROM " & Me.mParaUserNewHotel & ".TNHT_SEFA "

                SQL += "        WHERE "
                SQL += "            SEFA_TIFA = '6' "
                SQL += "    ) OR MOC1.SEFA_CODI IS NULL) "



            End If

            ' segun chany/edelia no se tratan notas de credito 
            '   SQL += "   AND MOC1.TIMO_CODI <> '" & Me.mCodigoNotasCredito & "'"

            SQL += "  AND MORE_DAAN = '" & Me.mFecha & "'"

            ' Excluye Operaciones Realizadas y Anuladas el mismo dia 
            SQL += "  AND (TNCO_MORE.MORE_DAAN > TNCO_MORE.MORE_DARE  OR TNCO_MORE.MORE_DAAN IS NULL) "

            SQL += "  AND MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            SQL += " ORDER BY SEFA_CODI,FACT_CODI"



            Me.DbNewConta.TraerLector(SQL)

            If Me.DbNewConta.mDbLector.HasRows Then
                '     MsgBox("Existen Documentos que han sido Afectados por la Anulación de Pagos ", MsgBoxStyle.Information, "Atención")
            End If

            While Me.DbNewConta.mDbLector.Read

                Linea = Linea + 1


                Cuenta = BuscaCuentaClienteNewHotel(CType(Me.DbNewConta.mDbLector("TACO_DEBI"), String))


                If CType(Me.DbNewConta.mDbLector("TIPOPAGO"), String) = "0" Then
                    '  TipoAsiento = 778
                    TipoAsiento = 777
                    EsFormaDePago = False
                    AuxiliarTipoMovimiento = CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " (-)"
                Else
                    EsFormaDePago = True
                    TipoAsiento = 777
                    AuxiliarTipoMovimiento = CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String)
                End If


                ' Si el documento o parte del documento se saldo con una nota de credito el importe va positivo
                If CStr(Me.DbNewConta.mDbLector("TIMO_CRED")) = mCodigoNotasCredito Then
                    '   Total = Math.Round(CType(Me.DbNewConta.mDbLector("TOTAL"), Double), 2)
                    Total = Math.Round(CType(Me.DbNewConta.mDbLector("TOTAL"), Double), 2) * -1
                Else
                    Total = Math.Round(CType(Me.DbNewConta.mDbLector("TOTAL"), Double), 2) * -1
                End If

                Recibo = BuscaReciboDeRegularizacion(CStr(Me.DbNewConta.mDbLector("TACO_DEBI")), CInt(Me.DbNewConta.mDbLector("MOCO_DEBI")), Total * -1, 1)

                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"

                    Me.InsertaOracleLopez("AC", TipoAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbNewConta.mDbLector("DESC_DEBI"), String), Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("CODIGO"), String) & " " & CType(Me.DbNewConta.mDbLector("NOMBRE"), String), "SI", AuxiliarTipoMovimiento, " Pagada con : " & CType(Me.DbNewConta.mDbLector("DESC_CRED"), String), CStr(Me.DbNewConta.mDbLector("MOCO_CRED")), "", CDate(Format(CDate(Me.DbNewConta.mDbLector("MORE_DARE")), "dd/MM/yyyy")), Recibo)
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
            SQL += "       NVL(MOC1.MOCO_DESC,NVL(TNCO_MOLI.MOLI_DESC, ' ')) DESC_CRED, MORE_DARE, MORE_VAPA AS TOTAL, MORE_DEAN, "
            SQL += "       TIMO_DEBI.TIMO_CDIA DEBI_CDIA, TIMO_DEBI.TIMO_CDOC DEBI_CDOC, "
            SQL += "       TIMO_CRED.TIMO_CDIA CRED_CDIA, TIMO_CRED.TIMO_CDOC CRED_CDOC, "
            SQL += "       MOCO.TACO_CODI TACO_DEBI, MOC1.TACO_CODI TACO_CRED ,"
            '  SQL += "       NVL(TIMO_CRED.TIMO_COCO,'0')  AS CUENTA "
            SQL += "        NVL(TAC1.TACO_CODA,'0') AS CUENTA ,"
            SQL += "       TAC1.TACO_CODI AS CODIGO, NVL(TAC1.TACO_NOME,'?') AS NOMBRE ,NVL(TAC1.TACO_NUCO,'?') AS NIF"

            SQL += "       ,NVL(TIMO_CRED.TIMO_PAGA,'0') AS TIPOPAGO , "
            SQL += "        NVL(MOLI_DESC,' ') AS MOLI_DESC"

            SQL += "        ,MOC1.MOCO_CODI"
            SQL += "        ,MOCO.FACT_CODI"
            SQL += "        ,MOCO.SEFA_CODI"


            SQL += "        , MOCO.MOCO_DAVA AS DAVADEB "
            SQL += "        , MOC1.MOCO_DAVA AS DAVACRE "
            SQL += "        , MOC1.MOCO_DESC  "

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

            If Me.mParaFActAnul2 = 0 Then
                SQL += "    AND (  MOC1.SEFA_CODI NOT  IN ( "
                SQL += "        SELECT "
                SQL += "            SEFA_CODI "
                SQL += "        FROM " & Me.mParaUserNewHotel & ".TNHT_SEFA "

                SQL += "        WHERE "
                SQL += "            SEFA_TIFA = '6' "
                SQL += "    ) OR MOC1.SEFA_CODI IS NULL) "


            End If

            ' segun chany/edelia no se tratan notas de credito 
            '   SQL += "   AND MOC1.TIMO_CODI <> '" & Me.mCodigoNotasCredito & "'"

            SQL += "  AND MORE_DAAN = '" & Me.mFecha & "'"

            ' Excluye Operaciones Realizadas y Anuladas el mismo dia 
            SQL += "  AND (TNCO_MORE.MORE_DAAN > TNCO_MORE.MORE_DARE  OR TNCO_MORE.MORE_DAAN IS NULL) "

            SQL += "  AND MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            SQL += " ORDER BY SEFA_CODI,FACT_CODI"


            Me.DbNewConta.TraerLector(SQL)

            While Me.DbNewConta.mDbLector.Read

                Linea = Linea + 1

                Cuenta = BuscaCuentaPagosAnticipadosNewHotel(CType(Me.DbNewConta.mDbLector("CODIGO"), String))

                If Me.mParaTextosFijos = 1 Then
                    If CStr(Me.DbNewConta.mDbLector("TIMO_CRED")) = mCodigoNotasCredito Then
                        Me.mtextoApunte = StrConv(CStr(Me.DbNewConta.mDbLector("DESC_CRED")), VbStrConv.ProperCase)
                    Else
                        Me.mtextoApunte = Mid(StrConv(CStr(Me.DbNewConta.mDbLector("MOLI_DESC")), VbStrConv.ProperCase), 1, 8) & "  " & StrConv(CStr(Me.DbNewConta.mDbLector("NOMBRE")), VbStrConv.ProperCase)
                    End If
                Else
                    If CStr(Me.DbNewConta.mDbLector("TIMO_CRED")) = mCodigoNotasCredito Then
                        Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("DESC_CRED"))
                    ElseIf IsDBNull(Me.DbNewConta.mDbLector("MOCO_DESC")) = True Then
                        Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOLI_DESC"))
                    Else
                        Me.mtextoApunte = CStr(Me.DbNewConta.mDbLector("MOLI_DESC")) & " ," & CStr(Me.DbNewConta.mDbLector("MOCO_DESC"))
                    End If
                End If

                If CType(Me.DbNewConta.mDbLector("TIPOPAGO"), String) = "0" Then
                    '  TipoAsiento = 778
                    TipoAsiento = 777
                    EsFormaDePago = False
                    AuxiliarTipoMovimiento = CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String) & " (-)"
                Else
                    EsFormaDePago = True
                    TipoAsiento = 777
                    AuxiliarTipoMovimiento = CType(Me.DbNewConta.mDbLector("MOLI_DESC"), String)
                End If


                ' Si el documento o parte del documento se saldo con una nota de credito el importe va positivo
                If CStr(Me.DbNewConta.mDbLector("TIMO_CRED")) = mCodigoNotasCredito Then
                    '   Total = Math.Round(CType(Me.DbNewConta.mDbLector("TOTAL"), Double), 2)
                    Total = Math.Round(CType(Me.DbNewConta.mDbLector("TOTAL"), Double), 2) * -1
                Else
                    Total = Math.Round(CType(Me.DbNewConta.mDbLector("TOTAL"), Double), 2) * -1
                End If

                Recibo = BuscaReciboDeRegularizacion(CStr(Me.DbNewConta.mDbLector("TACO_DEBI")), CInt(Me.DbNewConta.mDbLector("MOCO_DEBI")), Total * -1, 1)

                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracleLopez("AC", TipoAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Me.mtextoApunte, Total, "NO", CType(Me.DbNewConta.mDbLector("NIF"), String), CType(Me.DbNewConta.mDbLector("CODIGO"), String) & " " & CType(Me.DbNewConta.mDbLector("NOMBRE"), String), "SI", AuxiliarTipoMovimiento, " Paga a : " & CType(Me.DbNewConta.mDbLector("DESC_DEBI"), String), CStr(Me.DbNewConta.mDbLector("MOCO_CRED")), "", CDate(Format(CDate(Me.DbNewConta.mDbLector("MORE_DARE")), "dd/MM/yyyy")), Recibo)
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Me.mtextoApunte, Total)
                End If
            End While
            Me.DbNewConta.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try


    End Sub
#End Region

#End Region
#Region "METODOS PUBLICOS"
    Public Sub Procesar()
        Try

            '    MsgBox("Ojo Pendiente de Añadir Facturas Negativas como medios de PAgo ?? y otros ...  al asiento de PAgos 438... ", MsgBoxStyle.Exclamation, "Atención en Pruebas")
            If Me.FileEstaOk = False Then Exit Sub




            ' ---------------------------------------------------------------
            ' Asiento Facturacion total del dia 
            '----------------------------------------------------------------
            If Me.DbNewConta.EstadoConexion = ConnectionState.Open Then
                Me.mTextDebug.Text = "Calculando Pagos Recibidos"
                Me.mTextDebug.Update()


                If Me.mParaContaBilizaPagos = 1 Then
                    Me.NCPagosRecibidos()
                End If


                If Me.mParaFActNeg = 1 Then
                    Me.NCPagosRecibidosFacturasNegativas()
                End If


                If Me.mParaFActAnul = 1 Then
                    Me.NCPagosRecibidosFacturasSerieDeAnulacion()
                End If


                Me.mProgress.Value = 10
                Me.mProgress.Update()

                Me.mTextDebug.Text = "Calculando Facturas Regularizadas"
                Me.mTextDebug.Update()

                '  Me.NCFacturasRegularizadas()

                Me.NCFacturasRegularizadasLopez()

                Me.mProgress.Value = 20
                Me.mProgress.Update()



                Me.mTextDebug.Text = "Calculando Facturas Des-Regularizadas por Pagos anulados"
                Me.mTextDebug.Update()

                '  Me.NCFacturasRegularizadasAnuladas2()
                Me.NCFacturasRegularizadasAnuladasLopez()



                Me.mProgress.Value = 30
                Me.mProgress.Update()




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

            If Me.mParaValidaSpyro = 1 Then
                Me.SpyroCompruebaCuentasCorto()
                '   Me.SpyroCompruebaBancos()
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

    Private Function BuscaCuentaPagosAnticipadosNewHotel(ByVal vTacoCodi As String) As String


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
                MsgBox("Entidades a revisar  Cuenta Pagos Anticipados NewConta = " & vTacoCodi & vbCrLf & vbCrLf & "NewCentral o NewHotel = " & Entidades)
            End If



            If IsNothing(Cuenta) = False Then
                Return Cuenta
            Else
                ' INTENTA BUSCAR EN CUENTAS NO ALOJADO 
                SQL = "SELECT CCEX_DEAN FROM TNHT_CCEX WHERE  "
                SQL += " OPER_NECO = '" & vTacoCodi & "'"
                Cuenta = Me.DbLeeNewHotel.EjecutaSqlScalar(SQL)

                If IsNothing(Cuenta) = False Then
                    If Cuenta.Length > 0 Then
                        Return Cuenta
                    End If
                Else
                    Return "?"
                End If
            End If


        Catch ex As Exception
            MsgBox(ex.Message)
            Return "0"
        End Try

    End Function


    Private Function BuscaMovimientodeCobrodeunaFactura(ByVal vRecoCodi As Integer, ByVal vRecoAnci As Integer, ByVal vTipo As Integer) As String
        Dim Result As String = " "
        Dim TipoMovimiento As String
        Dim Documento() As String

        ' DE AQUI SE PUEDE RECUPERAR DATOS DEL MOVIMIENTO DE CREDITO MOCO Y DEBITO MOC1 A LA VEZ
        Try


            ' PRIMERO AVERIGUAR EL TIPO DE MOVIMIENTO 
            '  UNA VEZ SABIDO EL TIPO DE MOVIMIENTO + EL VALOR DE VTIPO SE DEVUELVE UNA COSA U OTRA 


            SQL = "Select TNCO_TIMO.TIMO_CODI "
            SQL = SQL & "    FROM TNCO_RECO TNCO_RECO, "
            SQL = SQL & "         TNCO_DERE TNCO_DERE, "
            SQL = SQL & "         VNCO_TACO TNCO_TACO, "
            SQL = SQL & "         TNCO_MOCO TNCO_MOCO, "
            SQL = SQL & "         TNCO_MOLI TNCO_MOLI, "
            SQL = SQL & "         TNCO_MOCO TNCO_MOC1, "
            SQL = SQL & "         TNCO_TIMO "
            SQL = SQL & "   WHERE ( (TNCO_RECO.RECO_CODI = TNCO_DERE.RECO_CODI(+)) "
            SQL = SQL & "          And (TNCO_RECO.RECO_ANCI = TNCO_DERE.RECO_ANCI(+))) "
            SQL = SQL & "         And (TNCO_RECO.TACO_CODI = TNCO_TACO.TACO_CODI) "
            SQL = SQL & "         And ( (TNCO_RECO.TACO_CODI = TNCO_MOCO.TACO_CODI) "
            SQL = SQL & "              And (TNCO_RECO.MOCO_CODI = TNCO_MOCO.MOCO_CODI)) "
            SQL = SQL & "         And (TNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI) "
            SQL = SQL & "         And ( (TNCO_DERE.TACO_CODI = TNCO_MOC1.TACO_CODI(+)) "
            SQL = SQL & "              And (TNCO_DERE.MOCO_CODI = TNCO_MOC1.MOCO_CODI(+))) "
            SQL = SQL & "         And TNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "
            SQL = SQL & "  And TNCO_DERE.RECO_CODI = " & vRecoCodi
            SQL = SQL & "  And TNCO_DERE.RECO_ANCI = " & vRecoAnci
            SQL = SQL & "         And TNCO_MOLI.LICL_CODI = 1 "
            SQL = SQL & "ORDER BY TNCO_RECO.RECO_ANCI, TNCO_RECO.RECO_CODI "


            TipoMovimiento = Me.DbNewContaAux.EjecutaSqlScalar(SQL)

            SQL = "Select TNCO_MOCO.MOCO_DOC1 "
            SQL = SQL & "    FROM TNCO_RECO TNCO_RECO, "
            SQL = SQL & "         TNCO_DERE TNCO_DERE, "
            SQL = SQL & "         VNCO_TACO TNCO_TACO, "
            SQL = SQL & "         TNCO_MOCO TNCO_MOCO, "
            SQL = SQL & "         TNCO_MOLI TNCO_MOLI, "
            SQL = SQL & "         TNCO_MOCO TNCO_MOC1, "
            SQL = SQL & "         TNCO_TIMO "
            SQL = SQL & "   WHERE ( (TNCO_RECO.RECO_CODI = TNCO_DERE.RECO_CODI(+)) "
            SQL = SQL & "          And (TNCO_RECO.RECO_ANCI = TNCO_DERE.RECO_ANCI(+))) "
            SQL = SQL & "         And (TNCO_RECO.TACO_CODI = TNCO_TACO.TACO_CODI) "
            SQL = SQL & "         And ( (TNCO_RECO.TACO_CODI = TNCO_MOCO.TACO_CODI) "
            SQL = SQL & "              And (TNCO_RECO.MOCO_CODI = TNCO_MOCO.MOCO_CODI)) "
            SQL = SQL & "         And (TNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI) "
            SQL = SQL & "         And ( (TNCO_DERE.TACO_CODI = TNCO_MOC1.TACO_CODI(+)) "
            SQL = SQL & "              And (TNCO_DERE.MOCO_CODI = TNCO_MOC1.MOCO_CODI(+))) "
            SQL = SQL & "         And TNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "
            SQL = SQL & "  And TNCO_DERE.RECO_CODI = " & vRecoCodi
            SQL = SQL & "  And TNCO_DERE.RECO_ANCI = " & vRecoAnci
            SQL = SQL & "         And TNCO_MOLI.LICL_CODI = 1 "
            SQL = SQL & "ORDER BY TNCO_RECO.RECO_ANCI, TNCO_RECO.RECO_CODI "


            Documento = CStr(Me.DbNewContaAux.EjecutaSqlScalar(SQL)).Split(CChar("/"))



            If vTipo = 1 Then
                SQL = "  Select  NVL (TNCO_MOCO.MOCO_DESC, '?') AS X"
            ElseIf vTipo = 2 And TipoMovimiento <> Me.mCodigoNotasCredito Then
                SQL = "  SELECT TNCO_TIMO.TIMO_CODI || ' ' || NVL (TNCO_TIMO.TIMO_COCO, '?') || ' + ' || NVL (TNCO_MOCO.MOCO_DESC, '?') AS X"
            ElseIf vTipo = 2 And TipoMovimiento = Me.mCodigoNotasCredito Then
                SQL = "  SELECT TNCO_TIMO.TIMO_CODI || ' ' || '" & Documento(0) & "' || ' + ' || " & "TNCO_TIMO.TIMO_CODI || ' " & Documento(0).Trim & "/" & Documento(1).Trim & "'   AS X"
            ElseIf vTipo = 3 Then
                SQL = "  SELECT  NVL (TNCO_MOLI.MOLI_DESC, '?') AS X"
            ElseIf vTipo = 4 Then
                SQL = "  SELECT  TNCO_MOCO.TACO_CODI || '/' || TNCO_MOCO.MOCO_CODI AS X"

            End If

            SQL = SQL & "    FROM TNCO_RECO TNCO_RECO, "
            SQL = SQL & "         TNCO_DERE TNCO_DERE, "
            SQL = SQL & "         VNCO_TACO TNCO_TACO, "
            SQL = SQL & "         TNCO_MOCO TNCO_MOCO, "
            SQL = SQL & "         TNCO_MOLI TNCO_MOLI, "
            SQL = SQL & "         TNCO_MOCO TNCO_MOC1, "
            SQL = SQL & "         TNCO_TIMO "
            SQL = SQL & "   WHERE ( (TNCO_RECO.RECO_CODI = TNCO_DERE.RECO_CODI(+)) "
            SQL = SQL & "          AND (TNCO_RECO.RECO_ANCI = TNCO_DERE.RECO_ANCI(+))) "
            SQL = SQL & "         AND (TNCO_RECO.TACO_CODI = TNCO_TACO.TACO_CODI) "
            SQL = SQL & "         AND ( (TNCO_RECO.TACO_CODI = TNCO_MOCO.TACO_CODI) "
            SQL = SQL & "              AND (TNCO_RECO.MOCO_CODI = TNCO_MOCO.MOCO_CODI)) "
            SQL = SQL & "         AND (TNCO_MOCO.TIMO_CODI = TNCO_MOLI.TIMO_CODI) "
            SQL = SQL & "         AND ( (TNCO_DERE.TACO_CODI = TNCO_MOC1.TACO_CODI(+)) "
            SQL = SQL & "              AND (TNCO_DERE.MOCO_CODI = TNCO_MOC1.MOCO_CODI(+))) "
            SQL = SQL & "         AND TNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "
            SQL = SQL & "  AND TNCO_DERE.RECO_CODI = " & vRecoCodi
            SQL = SQL & "  AND TNCO_DERE.RECO_ANCI = " & vRecoAnci
            SQL = SQL & "         AND TNCO_MOLI.LICL_CODI = 1 "
            SQL = SQL & "ORDER BY TNCO_RECO.RECO_ANCI, TNCO_RECO.RECO_CODI "

            Result = Me.DbNewContaAux.EjecutaSqlScalar(SQL)


            Return Result

        Catch ex As Exception
            MsgBox(ex.Message)
            Return "0"
        End Try

    End Function
    Private Function BuscaDatosdeCobrodeunaFactura(ByVal vRecoCodi As Integer, ByVal vRecoAnci As Integer, ByVal vTipo As Integer, ByVal vDereCodi As Integer) As String
        Dim Result As String = ""
        Dim TipoMovimiento As String
        Dim Documento As String
        Dim DocumentoAux() As String

        ' VTIPO 1  = PIDE DESCRIPCION DEL MOVIMIENTO 
        ' VTIPO 1  = PIDE DESCRIPCION DEL MOVIMIENTO  CONCATENADA CON TIPO DE MOVIMIEMTO Y CUENTA CONTABLE 
        ' VTIPO 3  = PIDE PIDE EL NUMERO DE DOCUMENTOD DE COBRO

        Try

            SQL = "SELECT TIMO_CODI FROM QWE_COBROS_RECIBO"
            SQL += " WHERE RECO_CODI = " & vRecoCodi
            SQL += " AND RECO_ANCI = " & vRecoAnci
            SQL += " AND DERE_CODI = " & vDereCodi

            TipoMovimiento = Me.DbNewContaAux.EjecutaSqlScalar(SQL)

            SQL = "SELECT MOCO_DOCU FROM QWE_COBROS_RECIBO"
            SQL += " WHERE RECO_CODI = " & vRecoCodi
            SQL += " AND RECO_ANCI = " & vRecoAnci
            SQL += " AND DERE_CODI = " & vDereCodi

            Documento = CStr(Me.DbNewContaAux.EjecutaSqlScalar(SQL))
            DocumentoAux = CStr(Me.DbNewContaAux.EjecutaSqlScalar(SQL)).Split(CChar("/"))


            If vTipo = 1 Then
                SQL = "  SELECT  MOCO_DESC "
            ElseIf vTipo = 2 And TipoMovimiento <> Me.mCodigoNotasCredito Then
                SQL = "  SELECT TIMO_CODI || ' ' || NVL (TIMO_COCO, '?') || ' + ' || NVL (MOCO_DESC, '?') AS X"
            ElseIf vTipo = 2 And TipoMovimiento = Me.mCodigoNotasCredito Then
                SQL = "  SELECT TIMO_CODI || ' ' || '" & DocumentoAux(0) & "' || ' + ' || " & "TIMO_CODI || ' " & DocumentoAux(0).Trim & "/" & DocumentoAux(1).Trim & "'   AS X"
            ElseIf vTipo = 4 Then
                SQL = "  SELECT  NVL (MOLI_DESC, '?') AS X"
            ElseIf vTipo = 5 Then
                SQL = "  SELECT  TACO_CODI || '/' || MOCO_CODI AS X"
            ElseIf vTipo = 6 And TipoMovimiento = Me.mCodigoNotasCredito Then
                SQL = "  SELECT TIMO_CODI    AS X"
            End If

            SQL = SQL & "    FROM QWE_COBROS_RECIBO  "
            SQL += " WHERE RECO_CODI = " & vRecoCodi
            SQL += " AND RECO_ANCI = " & vRecoAnci
            SQL += " AND DERE_CODI = " & vDereCodi


            If vTipo = 1 Or vTipo = 2 Or vTipo = 4 Or vTipo = 5 Or (vTipo = 6 And TipoMovimiento = Me.mCodigoNotasCredito) Then
                Return Me.DbNewContaAux.EjecutaSqlScalar(SQL)
            ElseIf vTipo = 3 Then

                ' QUITAR  LAS BARRAS SI SE COBRO CON FACTURAS O NOTAS D ECREDITO
                If TipoMovimiento = Me.mCodigoNotasCredito Then
                    If IsDBNull(Documento) = False Then
                        Dim ArrayDoc() As String = Split(Documento, "/")
                        Result = ArrayDoc(0)
                    Else
                        Result = ""
                    End If
                    Return Result
                ElseIf TipoMovimiento = Me.mCodigoFacturas Then
                    If IsDBNull(Documento) = False Then
                        Dim ArrayDoc() As String = Split(Documento, "/")
                        Result = ArrayDoc(0)
                    Else
                        Result = ""
                    End If
                    Return Result
                Else
                    Return Documento
                End If
            Else
                Return ""
            End If


        Catch ex As Exception
            MsgBox(ex.Message)
            Return ""
        End Try

    End Function

    ''' <summary>
    ''' ESTA FUNCION RETORNA TRUE SI EL MOVIMIENTO SE ENCUENTRA EN UN RECIBO  NO ANULADO O ANULADO EN FECHAS POSTERIORES
    ''' A LA DE EMISION ( O SEA CONTABILIZABLE)
    ''' </summary>
    ''' <param name="vTacoCodi"></param>
    ''' <param name="vMocoCodi"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function BuscaSiMovimientoCreadoyAnuladoHoy(ByVal vTacoCodi As String, ByVal vMocoCodi As Integer) As Boolean

        Dim Result As String = " "
        Dim ReSultArray As String()

        ' DE AQUI SE PUEDE RECUPERAR DATOS DEL MOVIMIENTO DE CREDITO MOCO Y DEBITO MOC1 A LA VEZ
        Try



            SQL = "SELECT RECO_CODI || ';' || RECO_ANCI FROM TNCO_DERE WHERE TACO_CODI = '" & vTacoCodi & "'"
            SQL += " AND MOCO_CODI = " & vMocoCodi

            Result = Me.DbNewContaAux.EjecutaSqlScalar(SQL)

            If IsNothing(Result) = False Then
                ReSultArray = Split(Result, ";")
                SQL = "SELECT RECO_DAEM || ';' || RECO_DAAN FROM TNCO_RECO WHERE RECO_CODI  = " & ReSultArray(0)
                SQL += " AND RECO_ANCI = " & ReSultArray(1)
                Result = Me.DbNewContaAux.EjecutaSqlScalar(SQL)


                ReSultArray = Split(Result, ";")
                ' NO ANULADO 
                If ReSultArray(1).Length = 0 Then Return True
                ' ANULADO OTRO  DIA 
                If ReSultArray(1).Length > 0 And ReSultArray(1) <> ReSultArray(0) Then Return True

                ' ANULADO EL MISMO DIA 
                If ReSultArray(1).Length > 0 And ReSultArray(1) = ReSultArray(0) Then Return False

            Else
                ' NO TIENE RECIBO 
                Return True
            End If



        Catch ex As Exception
            MsgBox(ex.Message)
            Return True
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
                MsgBox("Entidades a revisar Cuenta Cliente NewConta = " & vTacoCodi & vbCrLf & vbCrLf & "NewCentral o NewHotel = " & Entidades)
            End If




            If IsNothing(Cuenta) = False Then
                Return Cuenta
            Else
                ' INTENTA BUSCAR EN CUENTAS NO ALOJADO 
                SQL = "SELECT CCEX_NCON FROM TNHT_CCEX WHERE  "
                SQL += " OPER_NECO = '" & vTacoCodi & "'"
                Cuenta = Me.DbLeeNewHotel.EjecutaSqlScalar(SQL)

                If IsNothing(Cuenta) = False Then
                    If Cuenta.Length > 0 Then
                        Return Cuenta
                    End If
                Else
                    Return "?"
                End If
            End If


        Catch ex As Exception
            MsgBox(ex.Message)
            Return "0"
        End Try

    End Function
    Private Function BuscaReciboDeRegularizacion(ByVal vTacoCodi As String, vMocoCodi As Integer, vDereVare As Double, vAnulado As Integer) As String

        Try
            Dim Result As String = ""

            SQL = "SELECT TNCO_DERE.RECO_CODI || '/' || TNCO_DERE.RECO_ANCI AS RECIBO"
            SQL += " FROM TNCO_DERE,TNCO_RECO "
            SQL += " WHERE "

            SQL += "  TNCO_DERE.RECO_CODI = TNCO_RECO.RECO_CODI"
            SQL += " AND TNCO_DERE.RECO_ANCI = TNCO_RECO.RECO_ANCI"
            SQL += "  AND TNCO_DERE.TACO_CODI = " & "'" & vTacoCodi & "'"
            SQL += " AND TNCO_DERE.MOCO_CODI  = " & vMocoCodi
            SQL += " AND DERE_VARE  = " & vDereVare
            If vAnulado = 0 Then
                SQL += "  AND RECO_DAEM = '" & Me.mFecha & "'"
                SQL += "  AND RECO_ANUL = '" & "0" & "'"

            Else
                SQL += "  AND RECO_DAAN = '" & Me.mFecha & "'"
            End If


            Me.DbNewContaAux.TraerLector(SQL)


            While Me.DbNewContaAux.mDbLector.Read
                If Result = "" Then
                    Result = CStr(Me.DbNewContaAux.mDbLector.Item("RECIBO"))
                Else
                    Result = Result & " , " & CStr(Me.DbNewContaAux.mDbLector.Item("RECIBO"))
                    End If
                End While
            Me.DbNewContaAux.mDbLector.Close()


            Return Result




        Catch ex As Exception
            MsgBox(ex.Message)
            Return "No Localizado"
        End Try

    End Function
    Private Function BuscaReciboDeCobro(ByVal vTacoCodi As String, vMocoCodi As Integer) As String

        Try
            Dim Result As String = ""

            SQL = "SELECT RECO_CODI || '/' || RECO_ANCI AS RECIBO"
            SQL += " FROM TNCO_RECO "
            SQL += " WHERE TACO_CODI = " & "'" & vTacoCodi & "'"
            SQL += " AND MOCO_CODI  = " & vMocoCodi


            Result = Me.DbNewContaAux.EjecutaSqlScalar(SQL)

            If IsNothing(Result) = False Then
                If Result.Length > 0 Then
                    Return Result
                Else
                    Return ""
                End If
            Else
                Return ""
            End If




        Catch ex As Exception
            MsgBox(ex.Message)
            Return "?"
        End Try

    End Function
#End Region
End Class
