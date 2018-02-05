Imports System.IO
Imports System.Globalization
Public Class Axapta
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
    '20080825
    '  hay facturas de liquidacion de cobtado con debitos de dias aNTERIORES NO FACTURADOS


    ' PRUEBAS DEVOLUCIONES 30-03-2011

    ' Base de DAtos "MARINA/MARINA@OPRTATIL"
    ' RESERVA       FECHA      ANTICIPO 
    ' 9275/2010     10/11/2010     100
    ' 9276/2010     10/11/2010     200 
    '
    '
    'PEN
    '


    Declare Function CharToOem Lib "user32" Alias "CharToOemA" (ByVal lpszSrc As String, ByVal lpszDst As String) As Long
    Declare Function OemToChar Lib "user32" Alias "OemToCharA" (ByVal lpszSrc As String, ByVal lpszDst As String) As Long


    Private mDebug As Boolean = False
    Private mDebug2 As Boolean = False
    Private mTratarBonos As Boolean = False

    Private mStrConexionHotel As String

    Private mStrConexionNewGolf As String
    Private mStrConexionNewPos As String
    Private mParaConectaNewGolf As Integer
    Private mParaUsuarioNewGolf As String

    Private mParaConectaNewPos As Integer

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

    Private mParaIngresoPorHabitacion As Integer
    Private mParaIngresoPorhabitacionDpto As String

    Private mParaServCodiBonos As String
    Private mParaServCodiBonosAsoc As String
    Private mParaArticuloAnulareserva As String
    Private mParaPreFijoNotadeCredito As String


    Private mParaAnticiposAxapta As String



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


    Private mClientesContadoCifVentasNormal As String
    Private mClientesContadoCifVentasBonos As String
    Private mClientesContadoCifProduccionNormal As String
    Private mClientesContadoCifProduccionBonos As String


    ' Valores de retorno para debug
    Private mLiquidoServiciosConIgic As Double
    Private mLiquidoServiciosSinIgic As Double
    Private mLiquidoDesembolsos As Double

    Private mDevolucionAnticipos As Double

    Private mTotalProduccion As Double
    Private mTotalFacturacion As Double

    Private mAnticiposRecibidos As Double
    Private mCancelacionAnticipos As Double

    ' otros retornos
    Private mExistenRegistros As Boolean


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

    Private DbNewPos As C_DATOS.C_DatosOledb


    Private NEWHOTEL As NewHotel.NewHotelData

    Private NEWGOLF As NewGolf.NewGolfData



    ' CUENTAS CONTABLES PARA ASIENTOS DE GOLF
    Private mCtaBcta1 As String
    Private mCtaBcta2 As String
    Private mCtaBcta3 As String
    Private mCtaBcta4 As String
    Private mCtaBcta5 As String
    Private mCtaBcta6 As String
    Private mCtaBcta7 As String
    Private mCtaBcta8 As String
    Private mCtaBcta9 As String
    Private mCtaBcta10 As String
    Private mCtaBcta11 As String
    Private mCtaBcta12 As String

    Private MparaTipoBonoAsociacion As Integer

    Private MparaComisionBonoAsociacion As Double
    Private MparaTrataDevoluciones As Boolean

    Private mSufijoBookId As String = ""

    Private mMostrarMensajes As Boolean = True

    Dim mTrabajo As Boolean = True

    Private mTipoRedondeo As Integer
    Enum ETIPOREDONDEO As Integer
        Normal = 1
        AwayFromZero = 2
        ToEven = 3
    End Enum
    Private mProcId As String
    Private mresultStr As String


#Region "CONSTRUCTOR"
    Sub New(ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vStrConexionCentral As String,
    ByVal vStrConexionHotel As String, ByVal vFecha As Date, ByVal vFileName As String, ByVal vDebug As Boolean,
    ByVal vConrolDebug As System.Windows.Forms.TextBox, ByVal vListBox As System.Windows.Forms.ListBox,
    ByVal vStrConexionSpyro As String, ByVal vProgress As System.Windows.Forms.ProgressBar, ByVal vTrataDebitoNoFacturadoTpv As Boolean,
    ByVal vUsaTnhtMoviAuto As Boolean, ByVal vTrataBonos As Boolean, ByVal vStrConexionNewGolf As String, ByVal vDebug2 As Boolean,
    ByVal vTrataDevoluciones As Boolean, ByVal vMostrarMensajes As Boolean, ByVal vTipoRedondeo As Integer, vProcId As String)


        MyBase.New()

        Me.mDebug = vDebug
        Me.mDebug2 = vDebug2

        Me.mTratarBonos = vTrataBonos

        Me.mEmpGrupoCod = vEmpGrupoCod
        Me.mEmpCod = vEmpCod
        '  Me.mEmpNum = vEmpNum
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

        Me.mStrConexionNewPos = vStrConexionNewGolf

        Me.MparaTrataDevoluciones = vTrataDevoluciones

        Me.mMostrarMensajes = vMostrarMensajes

        Me.mTipoRedondeo = vTipoRedondeo

        Me.mProcId = vProcId

        Me.AbreConexiones()
        Me.CargaParametros()

        If Me.mParaValidaSpyro = 1 Then
            Me.DbSpyro = New C_DATOS.C_DatosOledb(Me.mStrConexionSpyro)
            Me.DbSpyro.AbrirConexion()
        Else
            '      MsgBox("No hay proceso de validación de cuentas en Spyro", MsgBoxStyle.Exclamation, "Atención")
        End If

        Me.BorraRegistros()

        If Me.P_Existenregistros = False Then
            Me.CrearFichero(Me.mParaFilePath & vFileName)
        End If




        'Dim Texto As String

        'Texto = "Ojo tratamiento de devoluciones de pagos a cuenta asiento 21 en FASE DE PRUEBAS" & vbCrLf & vbCrLf
        'Texto += "Ojo tratamiento de Notas de credito asiento 51 en FASE DE PRUEBAS(falta asisnto de cancelacion de Notas de credito)" & vbCrLf & vbCrLf
        'Texto += "Ojo falta devoluciones de pagos a cuenta de CUENTAS DE NO ALOJADO"
        'MsgBox(Texto, MsgBoxStyle.Critical, "Atención")

        ' auditoria 
        'Me.FacturasSinCuentaContable()

    End Sub

    Sub New()

    End Sub
#End Region
#Region "PROPIEDADES"
    Public Property P_Existenregistros() As Boolean
        Get
            Return Me.mExistenRegistros
        End Get
        Set(ByVal Value As Boolean)
            Me.mExistenRegistros = Value
        End Set
    End Property
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
    Public Property AnticiposRecibidos() As Double
        Get
            Return mAnticiposRecibidos
        End Get
        Set(ByVal Value As Double)
            mAnticiposRecibidos = Value
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
#Region "RUTINAS VARIAS"
    Private Sub CrearFichero(ByVal vFile As String)

        Try
            'Filegraba = New StreamWriter(vFile, False, System.Text.Encoding.UTF8)
            Filegraba = New StreamWriter(vFile, False, System.Text.Encoding.ASCII)

            Filegraba.WriteLine("")
            FileEstaOk = True

        Catch ex As Exception
            FileEstaOk = False
            If mMostrarMensajes = True Then
                MsgBox("No dispone de acceso al Fichero " & vFile & " " & ex.Message, MsgBoxStyle.Information, "Atención")
            Else
                Me.mListBoxDebug.Items.Add("No dispone de acceso al Fichero " & vFile & " " & ex.Message)
            End If

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
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Char to Oem")
            End If

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

            Me.DbNewPos = New C_DATOS.C_DatosOledb(Me.mStrConexionNewPos)
            Me.DbNewPos.AbrirConexion()
            Me.DbNewPos.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

          

            Me.DbSpyro = New C_DATOS.C_DatosOledb
            ' LA APERTURA se hace mas abajo ahora si existe contabilidad spyro para validar cuentas
            'Me.DbSpyro.AbrirConexion()
            'Me.DbSpyro.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Abrir conexiones")
            End If

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
            SQL += "NVL(PARA_INGRESO_POR_HABITACION,'0') INGRESOHABITACION,"
            SQL += "NVL(PARA_INGRESO_HABITACION_DPTO,'0') INGRESOHABITACIONDPTO,"
            SQL += "NVL(PARA_CONECTA_NEWGOLF,'0') CONECTANEWGOLF,"
            SQL += "NVL(PARA_USUARIO_NEWGOLF,'?') USUARIONEWGOLF,"

            SQL += "NVL(PARA_CONECTA_NEWPOS,'0') CONECTANEWPOS,"

            SQL += "NVL(PARA_BCTA1,'0') BCTA1,"
            SQL += "NVL(PARA_BCTA2,'0') BCTA2,"
            SQL += "NVL(PARA_BCTA3,'0') BCTA3,"
            SQL += "NVL(PARA_BCTA4,'0') BCTA4,"
            SQL += "NVL(PARA_BCTA5,'0') BCTA5,"
            SQL += "NVL(PARA_BCTA6,'0') BCTA6,"
            SQL += "NVL(PARA_BCTA7,'0') BCTA7,"
            SQL += "NVL(PARA_BCTA8,'0') BCTA8,"
            SQL += "NVL(PARA_BCTA9,'0') BCTA9,"
            SQL += "NVL(PARA_BCTA10,'0') BCTA10,"
            SQL += "NVL(PARA_BCTA11,'0') BCTA11,"
            SQL += "NVL(PARA_BCTA12,'0') BCTA12,"
            SQL += "NVL(TIAD_CODI,'0') TIAD_CODI,"
            SQL += "NVL(PARA_COMISION_BONOS_ASOC,'0') PARA_COMISION_BONOS_ASOC,"

            SQL += "NVL(PARA_SERV_CODI_BONO,'?') PARA_SERV_CODI_BONO,"
            SQL += "NVL(PARA_SERV_CODI_BONOASC,'?') PARA_SERV_CODI_BONOASC, "
            SQL += "NVL(PARA_ANTICIPO_AX,'?') PARA_ANTICIPO_AX, "

            SQL += "NVL(PARA_CIF_VENTA_NORMAL,'?') CLIENTESCONTADOCIFVN,"
            SQL += "NVL(PARA_CIF_VENTA_BONOS,'?') CLIENTESCONTADOCIFVB,"
            SQL += "NVL(PARA_CIF_PRODUC_NORMAL,'?') CLIENTESCONTADOCIFPN,"
            SQL += "NVL(PARA_CIF_PRODUC_BONOS,'?') CLIENTESCONTADOCIFPB,"
            SQL += "NVL(PARA_ART_ANULA_RESERVA,'?') PARA_ART_ANULA_RESERVA,"
            SQL += "NVL(PARA_PREF_NCREG,'?') PARA_PREF_NCREG"





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

                Me.mParaIngresoPorHabitacion = CType(Me.DbLeeCentral.mDbLector.Item("INGRESOHABITACION"), Integer)
                Me.mParaIngresoPorhabitacionDpto = CType(Me.DbLeeCentral.mDbLector.Item("INGRESOHABITACIONDPTO"), String)
                Me.mParaConectaNewGolf = CType(Me.DbLeeCentral.mDbLector.Item("CONECTANEWGOLF"), Integer)
                Me.mParaUsuarioNewGolf = CType(Me.DbLeeCentral.mDbLector.Item("USUARIONEWGOLF"), String)

                Me.mParaConectaNewPos = CType(Me.DbLeeCentral.mDbLector.Item("CONECTANEWPOS"), Integer)


                Me.mCtaBcta1 = CType(Me.DbLeeCentral.mDbLector.Item("BCTA1"), String)
                Me.mCtaBcta2 = CType(Me.DbLeeCentral.mDbLector.Item("BCTA2"), String)
                Me.mCtaBcta3 = CType(Me.DbLeeCentral.mDbLector.Item("BCTA3"), String)
                Me.mCtaBcta4 = CType(Me.DbLeeCentral.mDbLector.Item("BCTA4"), String)
                Me.mCtaBcta5 = CType(Me.DbLeeCentral.mDbLector.Item("BCTA5"), String)
                Me.mCtaBcta6 = CType(Me.DbLeeCentral.mDbLector.Item("BCTA6"), String)
                Me.mCtaBcta7 = CType(Me.DbLeeCentral.mDbLector.Item("BCTA7"), String)
                Me.mCtaBcta8 = CType(Me.DbLeeCentral.mDbLector.Item("BCTA8"), String)
                Me.mCtaBcta9 = CType(Me.DbLeeCentral.mDbLector.Item("BCTA9"), String)
                Me.mCtaBcta10 = CType(Me.DbLeeCentral.mDbLector.Item("BCTA10"), String)
                Me.mCtaBcta11 = CType(Me.DbLeeCentral.mDbLector.Item("BCTA11"), String)
                Me.mCtaBcta12 = CType(Me.DbLeeCentral.mDbLector.Item("BCTA12"), String)

                Me.MparaTipoBonoAsociacion = CType(Me.DbLeeCentral.mDbLector.Item("TIAD_CODI"), Integer)
                Me.MparaComisionBonoAsociacion = CType(Me.DbLeeCentral.mDbLector.Item("PARA_COMISION_BONOS_ASOC"), Double)


                Me.mParaServCodiBonos = CType(Me.DbLeeCentral.mDbLector.Item("PARA_SERV_CODI_BONO"), String)
                Me.mParaServCodiBonosAsoc = CType(Me.DbLeeCentral.mDbLector.Item("PARA_SERV_CODI_BONOASC"), String)
                Me.mParaAnticiposAxapta = CType(Me.DbLeeCentral.mDbLector.Item("PARA_ANTICIPO_AX"), String)
               
                Me.mClientesContadoCifVentasNormal = CType(Me.DbLeeCentral.mDbLector.Item("CLIENTESCONTADOCIFVN"), String)
                Me.mClientesContadoCifVentasBonos = CType(Me.DbLeeCentral.mDbLector.Item("CLIENTESCONTADOCIFVB"), String)
                Me.mClientesContadoCifProduccionNormal = CType(Me.DbLeeCentral.mDbLector.Item("CLIENTESCONTADOCIFPN"), String)
                Me.mClientesContadoCifProduccionBonos = CType(Me.DbLeeCentral.mDbLector.Item("CLIENTESCONTADOCIFPB"), String)

                Me.mParaArticuloAnulareserva = CType(Me.DbLeeCentral.mDbLector.Item("PARA_ART_ANULA_RESERVA"), String)
                Me.mParaPreFijoNotadeCredito = CType(Me.DbLeeCentral.mDbLector.Item("PARA_PREF_NCREG"), String)




            End If
            Me.DbLeeCentral.mDbLector.Close()
        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Exclamation, "Carga de Parámetros en Constructor de la Clase")
            End If

        End Try
    End Sub
    Private Sub BorraRegistros()
        Try

            Me.mresultStr = ""

            ' Control de que no se este ENVIANDO la fecha  que se pide generar 
            SQL = "SELECT COUNT(*) FROM TG_CONTROL WHERE CONTROL_DATA = '" & Me.mFecha & "'"
            SQL += " AND CONTROL_TIPO = 'NewHotel' "
            SQL += " AND CONTROL_STATUS = 'Enviando'"

            If CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Integer) > 0 Then

                SQL = "SELECT NVL(CONTROL_OBSE,'?') FROM TG_CONTROL WHERE CONTROL_DATA = '" & Me.mFecha & "'"
                SQL += " AND CONTROL_TIPO = 'NewHotel' "
                SQL += " AND CONTROL_STATUS = 'Enviando'"
                Me.mresultStr = Me.DbLeeCentral.EjecutaSqlScalar(SQL)
                MsgBox("Se está Enviando a Axapta esta Fecha (" & Format(Me.mFecha, "dd/MM/yyyy") & "), en otra Máquina o Sesión" & vbCrLf & vbCrLf & Me.mresultStr, MsgBoxStyle.Information, "Atención")
                Me.P_Existenregistros = True
                Exit Sub
            End If




            ' Control de que no se este GENERANDO fecha  que se pide generar 
            SQL = "SELECT COUNT(*) FROM TG_CONTROL WHERE CONTROL_DATA = '" & Me.mFecha & "'"
            SQL += " AND CONTROL_TIPO = 'NewHotel' "
            SQL += " AND CONTROL_STATUS = 'Generando'"

            If CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Integer) > 0 Then

                SQL = "SELECT NVL(CONTROL_OBSE,'?') FROM TG_CONTROL WHERE CONTROL_DATA = '" & Me.mFecha & "'"
                SQL += " AND CONTROL_TIPO = 'NewHotel' "
                SQL += " AND CONTROL_STATUS = 'Generando'"
                Me.mresultStr = Me.DbLeeCentral.EjecutaSqlScalar(SQL)
                MsgBox("Se está Generando  esta Fecha (" & Format(Me.mFecha, "dd/MM/yyyy") & "),  en otra Máquina o Sesión " & vbCrLf & vbCrLf & Me.mresultStr, MsgBoxStyle.Information, "Atención")
                Me.P_Existenregistros = True
                Exit Sub
            Else
                SQL = "INSERT INTO TG_CONTROL (CONTROL_DATA, CONTROL_ID, CONTROL_TIPO,CONTROL_STATUS, CONTROL_OBSE)VALUES('"
                SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
                SQL += Me.mProcId & "','NewHotel','Generando','"
                SQL += System.Environment.UserName & " de " & System.Environment.UserDomainName & " desde " & System.Environment.MachineName & " " & Now
                SQL += "')"



                Me.DbLeeCentral.EjecutaSqlCommit(SQL)

            End If




            SQL = "SELECT COUNT(*) FROM TH_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_AX_STATUS = 1"
            If CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Integer) > 0 Then
                If mMostrarMensajes = True Then
                    MsgBox("Ya existen Movimientos de Integración Generados para esta Fecha " & Me.mFecha & "  en Axapta " & vbCrLf & vbCrLf & "(Se Procede a Reintentar Errores ...)", MsgBoxStyle.Information, "Atención")
                End If
                Me.P_Existenregistros = True
            Else
                Me.P_Existenregistros = False
                SQL = "DELETE TH_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
                SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

                SQL = "DELETE TH_ERRO WHERE ERRO_F_ATOCAB =  '" & Me.mFecha & "'"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            End If




        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Borra Registros")
            End If

        End Try

    End Sub
    Private Sub CargaBookid()
        Try
            Dim BookId As String
            Dim Cursores As Integer

            ' anticipos 
            SQL = "SELECT  NVL(ASNT_CFCTA_COD,' ') AS CUENTA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,NVL(ASNT_I_MONEMP,'0')  AS IMPORTE"
            SQL += " , ASNT_AUXILIAR_STRING AS TIPO ,NVL(ASNT_AUXILIAR_STRING2,' ') AS RESTO "
            SQL += " , ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI,ASNT_ENTI_CODI,ASNT_TIPO_CLIENTE_ANTICIPO,NVL(ASNT_FORE_CODI_AX,'?') AS FORMA,NVL(ASNT_CIF ,'?') AS CIF "
            SQL += " ,NVL(ASNT_RECIBO,'?') AS RECIBO,ROWID "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHPrePaymentService'"


            Me.DbLeeCentral.TraerLector(SQL)
            While Me.DbLeeCentral.mDbLector.Read
                If CType(Me.DbLeeCentral.mDbLector.Item("ASNT_RESE_CODI"), Integer) = 0 Then
                    BookId = CType(Me.DbLeeCentral.mDbLector.Item("RESTO"), String).Trim & CType(Me.DbLeeCentral.mDbLector.Item("RECIBO"), String)
                Else
                    BookId = CType(Me.DbLeeCentral.mDbLector.Item("RESTO"), String).Trim & CType(Me.DbLeeCentral.mDbLector.Item("ASNT_RESE_CODI"), String) & "/" & CType(Me.DbLeeCentral.mDbLector.Item("ASNT_RESE_ANCI"), String)
                End If
                SQL = "SELECT NVL(COUNT(*),'0') AS TOTAL FROM V$OPEN_CURSOR WHERE USER_NAME = '" & "INTEGRACION" & "'"
                Cursores = Me.DbGrabaCentral.EjecutaSqlScalar(SQL)
                If Cursores > 200 Then
                    Me.DbGrabaCentral.CerrarConexion()
                    Me.DbGrabaCentral.AbrirConexion()
                End If

                ' SE CARGA ESTE DATO PARA REPORT EXTRATO DE BOOKID
                SQL = "UPDATE TH_ASNT SET ASNT_BOOKID = '" & BookId & "'"
                SQL += " WHERE ROWID = '" & Me.DbLeeCentral.mDbLector.Item("ROWID") & "'"

                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)


            End While
            Me.DbLeeCentral.mDbLector.Close()

            ' cobros de anticipos

            SQL = "SELECT ROWID, NVL(ASNT_LIN_DOCU,'?') AS DOCUMENTO,NVL(ASNT_CFCTA_COD,' ') AS FORMA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,NVL(ASNT_I_MONEMP,'0')  AS IMPORTE"
            SQL += " ,NVL(ASNT_AUXILIAR_STRING,'?') AS ASNT_AUXILIAR_STRING, ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI"
            SQL += " ,NVL(ASNT_RECIBO,'?') AS RECIBO ,NVL(ASNT_AUXILIAR_STRING2,' ') AS RESTO "

            SQL += " ,NVL(ASNT_FORE_CODI_AX,'0') AS FORMA2 ,NVL(ASNT_CACR_CODI,'0') AS TARJETA "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService'"
            ' COBROS QUE SI SON ANTICIPOS 
            SQL += " AND ASNT_CFCTA_COD =  '" & Me.mParaAnticiposAxapta & "'"




            Me.DbLeeCentral.TraerLector(SQL)
            While Me.DbLeeCentral.mDbLector.Read


                If CType(Me.DbLeeCentral.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = "ANTICIPO FACTURADO" Then
                    If CType(Me.DbLeeCentral.mDbLector.Item("ASNT_RESE_CODI"), Integer) = 0 Then
                        BookId = CType(Me.DbLeeCentral.mDbLector.Item("RESTO"), String).Trim & CType(Me.DbLeeCentral.mDbLector.Item("RECIBO"), String)
                    Else
                        BookId = CType(Me.DbLeeCentral.mDbLector.Item("RESTO"), String).Trim & CType(Me.DbLeeCentral.mDbLector.Item("ASNT_RESE_CODI"), String) & "/" & CType(Me.DbLeeCentral.mDbLector.Item("ASNT_RESE_ANCI"), String)
                    End If
                Else
                    BookId = ""
                End If


                ' SE CARGA ESTE DATO PARA REPORT EXTRATO DE BOOKID
                SQL = "UPDATE TH_ASNT SET ASNT_BOOKID = '" & BookId & "'"
                SQL += " WHERE ROWID = '" & Me.DbLeeCentral.mDbLector.Item("ROWID") & "'"

                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)


            End While
            Me.DbLeeCentral.mDbLector.Close()




        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "CargaBookid")
            End If

        Finally

        End Try
    End Sub
    Public Sub CargaBookidActualizar()
        Try
            Dim BookId As String
            Dim Cursores As Integer

            ' anticipos 
            SQL = "SELECT  NVL(ASNT_CFCTA_COD,' ') AS CUENTA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,NVL(ASNT_I_MONEMP,'0')  AS IMPORTE"
            SQL += " , ASNT_AUXILIAR_STRING AS TIPO ,NVL(ASNT_AUXILIAR_STRING2,' ') AS RESTO "
            SQL += " , ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI,ASNT_ENTI_CODI,ASNT_TIPO_CLIENTE_ANTICIPO,NVL(ASNT_FORE_CODI_AX,'?') AS FORMA,NVL(ASNT_CIF ,'?') AS CIF "
            SQL += " ,NVL(ASNT_RECIBO,'?') AS RECIBO,ROWID "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE "
            SQL += "  TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHPrePaymentService'"


            Me.DbLeeCentral.TraerLector(SQL)
            While Me.DbLeeCentral.mDbLector.Read
                If CType(Me.DbLeeCentral.mDbLector.Item("ASNT_RESE_CODI"), Integer) = 0 Then
                    BookId = CType(Me.DbLeeCentral.mDbLector.Item("RESTO"), String).Trim & CType(Me.DbLeeCentral.mDbLector.Item("RECIBO"), String)
                Else
                    BookId = CType(Me.DbLeeCentral.mDbLector.Item("RESTO"), String).Trim & CType(Me.DbLeeCentral.mDbLector.Item("ASNT_RESE_CODI"), String) & "/" & CType(Me.DbLeeCentral.mDbLector.Item("ASNT_RESE_ANCI"), String)
                End If


                ' control de cursores
                SQL = "SELECT NVL(COUNT(*),'0') AS TOTAL FROM V$OPEN_CURSOR WHERE USER_NAME = '" & "INTEGRACION" & "'"
                Cursores = Me.DbGrabaCentral.EjecutaSqlScalar(SQL)
                If Cursores > 200 Then
                    Me.DbGrabaCentral.CerrarConexion()
                    Me.DbGrabaCentral.AbrirConexion()
                End If


                ' SE CARGA ESTE DATO PARA REPORT EXTRATO DE BOOKID
                SQL = "UPDATE TH_ASNT SET ASNT_BOOKID = '" & BookId & "'"
                SQL += " WHERE ROWID = '" & Me.DbLeeCentral.mDbLector.Item("ROWID") & "'"
                SQL += " AND ASNT_BOOKID IS NULL "

                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)


            End While
            Me.DbLeeCentral.mDbLector.Close()

            ' cobros de anticipos

            SQL = "SELECT ROWID, NVL(ASNT_LIN_DOCU,'?') AS DOCUMENTO,NVL(ASNT_CFCTA_COD,' ') AS FORMA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,NVL(ASNT_I_MONEMP,'0')  AS IMPORTE"
            SQL += " ,NVL(ASNT_AUXILIAR_STRING,'?') AS ASNT_AUXILIAR_STRING, ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI"
            SQL += " ,NVL(ASNT_RECIBO,'?') AS RECIBO ,NVL(ASNT_AUXILIAR_STRING2,' ') AS RESTO "

            SQL += " ,NVL(ASNT_FORE_CODI_AX,'0') AS FORMA2 ,NVL(ASNT_CACR_CODI,'0') AS TARJETA "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE "
            SQL += "  TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService'"
            ' COBROS QUE SI SON ANTICIPOS 
            SQL += " AND ASNT_CFCTA_COD =  '" & Me.mParaAnticiposAxapta & "'"




            Me.DbLeeCentral.TraerLector(SQL)
            While Me.DbLeeCentral.mDbLector.Read


                If CType(Me.DbLeeCentral.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = "ANTICIPO FACTURADO" Then
                    If CType(Me.DbLeeCentral.mDbLector.Item("ASNT_RESE_CODI"), Integer) = 0 Then
                        BookId = CType(Me.DbLeeCentral.mDbLector.Item("RESTO"), String).Trim & CType(Me.DbLeeCentral.mDbLector.Item("RECIBO"), String)
                    Else
                        BookId = CType(Me.DbLeeCentral.mDbLector.Item("RESTO"), String).Trim & CType(Me.DbLeeCentral.mDbLector.Item("ASNT_RESE_CODI"), String) & "/" & CType(Me.DbLeeCentral.mDbLector.Item("ASNT_RESE_ANCI"), String)
                    End If
                Else
                    BookId = ""
                End If

                ' control de cursores
                SQL = "SELECT NVL(COUNT(*),'0') AS TOTAL FROM V$OPEN_CURSOR WHERE USER_NAME = '" & "INTEGRACION" & "'"
                Cursores = Me.DbGrabaCentral.EjecutaSqlScalar(SQL)
                If Cursores > 200 Then
                    Me.DbGrabaCentral.CerrarConexion()
                    Me.DbGrabaCentral.AbrirConexion()
                End If



                ' SE CARGA ESTE DATO PARA REPORT EXTRATO DE BOOKID
                SQL = "UPDATE TH_ASNT SET ASNT_BOOKID = '" & BookId & "'"
                SQL += " WHERE ROWID = '" & Me.DbLeeCentral.mDbLector.Item("ROWID") & "'"
                SQL += " AND ASNT_BOOKID IS NULL "


                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)


            End While
            Me.DbLeeCentral.mDbLector.Close()


            Me.CerrarFichero()
            Me.CierraConexiones()
            Me.mTextDebug.Text = "Fin de Integración de Actualizacion de Book Id"
            Me.mTextDebug.Update()



        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "CargaBookid")
            End If

        Finally

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
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "')"




            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto
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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub

    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                      ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                      , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                        ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "')"



            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto
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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                       ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String, ByVal vWebServiceName As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "')"



            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                       ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String, ByVal vWebServiceName As String, _
                                       ByVal vRese_Codi As Integer, ByVal vRese_Anci As Integer, ByVal vCcex_Codi As String, ByVal vEnti_Codi As String, ByVal vTipoClienteAnticipo As Integer, ByVal vForeCodi As String, ByVal vRecibo As String, ByVal vNada As String, ByVal vNada2 As String, ByVal vNada3 As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,"
            SQL += "ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI,ASNT_ENTI_CODI,ASNT_TIPO_CLIENTE_ANTICIPO,ASNT_FORE_CODI_AX,ASNT_RECIBO) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += vRese_Codi & ","
            SQL += vRese_Anci & ",'"
            SQL += vCcex_Codi & "','"
            SQL += vEnti_Codi & "',"
            SQL += vTipoClienteAnticipo & ",'"
            SQL += vForeCodi & "','"
            SQL += vRecibo & "')"



            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    'xxxx pat 36
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                       ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String, ByVal vWebServiceName As String, _
                                       ByVal vRese_Codi As Integer, ByVal vRese_Anci As Integer, ByVal vCcex_Codi As String, ByVal vEnti_Codi As String, ByVal vTipoClienteAnticipo As Integer, ByVal vForeCodi As String, ByVal vRecibo As String, ByVal vNada As String, ByVal vNada2 As String, ByVal vNada3 As String, ByVal vMoviCodi As Integer)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,"
            SQL += "ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI,ASNT_ENTI_CODI,ASNT_TIPO_CLIENTE_ANTICIPO,ASNT_FORE_CODI_AX,ASNT_RECIBO,ASNT_MOVI_CODI) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += vRese_Codi & ","
            SQL += vRese_Anci & ",'"
            SQL += vCcex_Codi & "','"
            SQL += vEnti_Codi & "',"
            SQL += vTipoClienteAnticipo & ",'"
            SQL += vForeCodi & "','"
            SQL += vRecibo & "',"
            SQL += vMoviCodi & ")"



            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                  ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                  , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                    ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String, ByVal vWebServiceName As String, _
                                    ByVal vRese_Codi As Integer, ByVal vRese_Anci As Integer, ByVal vCcex_Codi As String, ByVal vEnti_Codi As String, ByVal vTipoClienteAnticipo As Integer, ByVal vForeCodi As String, _
                                    ByVal vRecibo As String, ByVal vNada As String, ByVal vNada2 As String, ByVal vNada3 As String, ByVal vFactura As String, ByVal vSerie As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,"
            SQL += "ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI,ASNT_ENTI_CODI,ASNT_TIPO_CLIENTE_ANTICIPO,ASNT_FORE_CODI_AX,ASNT_RECIBO,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += vRese_Codi & ","
            SQL += vRese_Anci & ",'"
            SQL += vCcex_Codi & "','"
            SQL += vEnti_Codi & "',"
            SQL += vTipoClienteAnticipo & ",'"
            SQL += vForeCodi & "','"
            SQL += vRecibo & "','"
            SQL += vFactura & "','"
            SQL += vSerie & "')"



            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                  ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                  , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                    ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String, ByVal vWebServiceName As String, _
                                    ByVal vRese_Codi As Integer, ByVal vRese_Anci As Integer, ByVal vCcex_Codi As String, ByVal vEnti_Codi As String, ByVal vTipoClienteAnticipo As Integer, ByVal vForeCodi As String, _
                                    ByVal vRecibo As String, ByVal vNada As String, ByVal vNada2 As String, ByVal vNada3 As String, ByVal vFactura As String, ByVal vSerie As String, ByVal vResto As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,"
            SQL += "ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI,ASNT_ENTI_CODI,ASNT_TIPO_CLIENTE_ANTICIPO,ASNT_FORE_CODI_AX,ASNT_RECIBO,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_AUXILIAR_STRING2) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += vRese_Codi & ","
            SQL += vRese_Anci & ",'"
            SQL += vCcex_Codi & "','"
            SQL += vEnti_Codi & "',"
            SQL += vTipoClienteAnticipo & ",'"
            SQL += vForeCodi & "','"
            SQL += vRecibo & "','"
            SQL += vFactura & "','"
            SQL += vSerie & "','"
            SQL += vResto & "')"



            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                  ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                  , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                    ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String, ByVal vWebServiceName As String, _
                                    ByVal vRese_Codi As Integer, ByVal vRese_Anci As Integer, ByVal vCcex_Codi As String, ByVal vEnti_Codi As String, ByVal vTipoClienteAnticipo As Integer, ByVal vForeCodi As String, _
                                    ByVal vRecibo As String, ByVal vNada As String, ByVal vNada2 As String, ByVal vNada3 As String, ByVal vFactura As String, ByVal vSerie As String, ByVal vResto As String, ByVal vDevolucion As String, ByVal vMoviCodi As Integer)

        Try
            '******************************2
            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,"
            SQL += "ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI,ASNT_ENTI_CODI,ASNT_TIPO_CLIENTE_ANTICIPO,ASNT_FORE_CODI_AX,ASNT_RECIBO,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_AUXILIAR_STRING2,ASNT_DEVOLUCION,ASNT_MOVI_CODI) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += vRese_Codi & ","
            SQL += vRese_Anci & ",'"
            SQL += vCcex_Codi & "','"
            SQL += vEnti_Codi & "',"
            SQL += vTipoClienteAnticipo & ",'"
            SQL += vForeCodi & "','"
            SQL += vRecibo & "','"
            SQL += vFactura & "','"
            SQL += vSerie & "','"
            SQL += vResto & "','"
            SQL += vDevolucion & "'," & vMoviCodi & ")"



            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                 ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                 , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                   ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String, ByVal vWebServiceName As String, _
                                   ByVal vRese_Codi As Integer, ByVal vRese_Anci As Integer, ByVal vCcex_Codi As String, ByVal vEnti_Codi As String, ByVal vTipoClienteAnticipo As Integer, ByVal vForeCodi As String, _
                                   ByVal vRecibo As String, ByVal vNada As String, ByVal vNada2 As String, ByVal vNada3 As String, ByVal vFactura As String, ByVal vSerie As String, ByVal vResto As String, ByVal vDevolucion As String, ByVal vMoviCodi As Integer, ByVal vPrefijo As String)

        Try
            '******************************2
            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,"
            SQL += "ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI,ASNT_ENTI_CODI,ASNT_TIPO_CLIENTE_ANTICIPO,ASNT_FORE_CODI_AX,ASNT_RECIBO,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_AUXILIAR_STRING2,ASNT_DEVOLUCION,ASNT_MOVI_CODI) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += vRese_Codi & ","
            SQL += vRese_Anci & ",'"
            SQL += vCcex_Codi & "','"
            SQL += vEnti_Codi & "',"
            SQL += vTipoClienteAnticipo & ",'"
            SQL += vForeCodi & "','"
            SQL += vRecibo & "','"
            SQL += vFactura & "','"
            SQL += vSerie & "','"
            SQL += vResto & vPrefijo & "','"
            SQL += vDevolucion & "'," & vMoviCodi & ")"



            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    '' aqui 
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                  ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                  , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                    ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String, ByVal vWebServiceName As String, _
                                    ByVal vRese_Codi As Integer, ByVal vRese_Anci As Integer, ByVal vCcex_Codi As String, ByVal vEnti_Codi As String, ByVal vTipoClienteAnticipo As Integer, ByVal vForeCodi As String, _
                                    ByVal vRecibo As String, ByVal vNada As String, ByVal vNada2 As String, ByVal vNada3 As String, ByVal vFactura As String, ByVal vSerie As String, ByVal vResto As String, ByVal vStatus1 As Integer)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,"
            SQL += "ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI,ASNT_ENTI_CODI,ASNT_TIPO_CLIENTE_ANTICIPO,ASNT_FORE_CODI_AX,ASNT_RECIBO,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_AUXILIAR_STRING2,ASNT_AX_STATUS) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += vRese_Codi & ","
            SQL += vRese_Anci & ",'"
            SQL += vCcex_Codi & "','"
            SQL += vEnti_Codi & "',"
            SQL += vTipoClienteAnticipo & ",'"
            SQL += vForeCodi & "','"
            SQL += vRecibo & "','"
            SQL += vFactura & "','"
            SQL += vSerie & "','"
            SQL += vResto & "',"
            SQL += vStatus1 & ")"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                       ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String, ByVal vWebServiceName As String, _
                                       ByVal vRese_Codi As Integer, ByVal vRese_Anci As Integer, ByVal vCcex_Codi As String, ByVal vEnti_Codi As String, ByVal vTipoClienteAnticipo As Integer, ByVal vForeCodi As String, ByVal vLinDocu As String, ByVal vRecibo As String)

        Try
            ' xxxxxxxxxxxxxxxxxxxxx
            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,"
            SQL += "ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI,ASNT_ENTI_CODI,ASNT_TIPO_CLIENTE_ANTICIPO,ASNT_FORE_CODI_AX,ASNT_LIN_DOCU,ASNT_RECIBO) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += vRese_Codi & ","
            SQL += vRese_Anci & ",'"
            SQL += vCcex_Codi & "','"
            SQL += vEnti_Codi & "',"
            SQL += vTipoClienteAnticipo & ",'"
            SQL += vForeCodi & "','"
            SQL += vLinDocu & "','"
            SQL += vRecibo & "')"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
           Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto
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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                    ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                    , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                      ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String, ByVal vWebServiceName As String, _
                                      ByVal vRese_Codi As Integer, ByVal vRese_Anci As Integer, ByVal vCcex_Codi As String, ByVal vEnti_Codi As String, ByVal vTipoClienteAnticipo As Integer, ByVal vForeCodi As String, ByVal vLinDocu As String, ByVal vRecibo As String, ByVal vResto As String)

        Try
            ' xxxxxxxxxxxxxxxxxxxxx 35
            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,"
            SQL += "ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI,ASNT_ENTI_CODI,ASNT_TIPO_CLIENTE_ANTICIPO,ASNT_FORE_CODI_AX,ASNT_LIN_DOCU,ASNT_RECIBO,ASNT_AUXILIAR_STRING2) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += vRese_Codi & ","
            SQL += vRese_Anci & ",'"
            SQL += vCcex_Codi & "','"
            SQL += vEnti_Codi & "',"
            SQL += vTipoClienteAnticipo & ",'"
            SQL += vForeCodi & "','"
            SQL += vLinDocu & "','"
            SQL += vRecibo & "','"
            SQL += vResto & "')"



            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle2(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                    ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                    , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                      ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String, ByVal vWebServiceName As String, _
                                      ByVal vRese_Codi As Integer, ByVal vRese_Anci As Integer, ByVal vCcex_Codi As String, ByVal vEnti_Codi As String, ByVal vTipoClienteAnticipo As Integer, ByVal vForeCodi As String, ByVal vLinDocu As String, ByVal vRecibo As String, ByVal vResto As String, ByVal vDepartamento As String)

        Try
            ' xxxxxxxxxxxxxxxxxxxxx 35
            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,"
            SQL += "ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI,ASNT_ENTI_CODI,ASNT_TIPO_CLIENTE_ANTICIPO,ASNT_FORE_CODI_AX,ASNT_LIN_DOCU,ASNT_RECIBO,ASNT_AUXILIAR_STRING2,ASNT_DPTO_DESC) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += vRese_Codi & ","
            SQL += vRese_Anci & ",'"
            SQL += vCcex_Codi & "','"
            SQL += vEnti_Codi & "',"
            SQL += vTipoClienteAnticipo & ",'"
            SQL += vForeCodi & "','"
            SQL += vLinDocu & "','"
            SQL += vRecibo & "','"
            SQL += vResto & "','"
            SQL += vDepartamento & "')"



            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                   ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                   , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                     ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String, ByVal vWebServiceName As String, _
                                     ByVal vRese_Codi As Integer, ByVal vRese_Anci As Integer, ByVal vCcex_Codi As String, ByVal vEnti_Codi As String, _
                                     ByVal vTipoClienteAnticipo As Integer, ByVal vForeCodi As String, ByVal vLinDocu As String, ByVal vRecibo As String, ByVal vResto As String, ByVal vStatus As Integer, ByVal vNada As String)

        Try
            ' xxxxxxxxxxxxxxxxxxxxx 35B
            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,"
            SQL += "ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI,ASNT_ENTI_CODI,ASNT_TIPO_CLIENTE_ANTICIPO,ASNT_FORE_CODI_AX,ASNT_LIN_DOCU,ASNT_RECIBO,ASNT_AUXILIAR_STRING2,ASNT_AX_STATUS) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += vRese_Codi & ","
            SQL += vRese_Anci & ",'"
            SQL += vCcex_Codi & "','"
            SQL += vEnti_Codi & "',"
            SQL += vTipoClienteAnticipo & ",'"
            SQL += vForeCodi & "','"
            SQL += vLinDocu & "','"
            SQL += vRecibo & "','"
            SQL += vResto & "',"
            SQL += vStatus & ")"



            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                   ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                   , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                     ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String, ByVal vWebServiceName As String, _
                                     ByVal vRese_Codi As Integer, ByVal vRese_Anci As Integer, ByVal vCcex_Codi As String, ByVal vEnti_Codi As String, ByVal vTipoClienteAnticipo As Integer, _
                                     ByVal vForeCodi As String, ByVal vLinDocu As String, ByVal vRecibo As String, ByVal vResto As String, ByVal vPrefijo As String, ByVal vNada1 As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,"
            SQL += "ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI,ASNT_ENTI_CODI,ASNT_TIPO_CLIENTE_ANTICIPO,ASNT_FORE_CODI_AX,ASNT_LIN_DOCU,ASNT_RECIBO,ASNT_AUXILIAR_STRING2) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += vRese_Codi & ","
            SQL += vRese_Anci & ",'"
            SQL += vCcex_Codi & "','"
            SQL += vEnti_Codi & "',"
            SQL += vTipoClienteAnticipo & ",'"
            SQL += vForeCodi & "','"
            SQL += vLinDocu & "','"
            SQL += vRecibo & "','"
            SQL += vResto & vPrefijo & "')"



            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    ''
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                    ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                    , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                      ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String, ByVal vWebServiceName As String, _
                                      ByVal vRese_Codi As Integer, ByVal vRese_Anci As Integer, ByVal vCcex_Codi As String, ByVal vEnti_Codi As String, ByVal vTipoClienteAnticipo As Integer, ByVal vForeCodi As String, ByVal vLinDocu As String, ByVal vRecibo As String, ByVal vResto As String, ByVal vStatus2 As Integer)

        Try
            ' xxxxxxxxxxxxxxxxxxxxx
            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,"
            SQL += "ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI,ASNT_ENTI_CODI,ASNT_TIPO_CLIENTE_ANTICIPO,ASNT_FORE_CODI_AX,ASNT_LIN_DOCU,ASNT_RECIBO,ASNT_AUXILIAR_STRING2,ASNT_AX_STATUS) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += vRese_Codi & ","
            SQL += vRese_Anci & ",'"
            SQL += vCcex_Codi & "','"
            SQL += vEnti_Codi & "',"
            SQL += vTipoClienteAnticipo & ",'"
            SQL += vForeCodi & "','"
            SQL += vLinDocu & "','"
            SQL += vRecibo & "','"
            SQL += vResto & "',"
            SQL += vStatus2 & ")"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                    ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                    , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                      ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, _
                                      ByVal vAuxiliarString As String, ByVal vWebServiceName As String, ByVal vProducto As String, _
                                      ByVal vTalla As String, ByVal vColor As String, ByVal vAlmacen As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,"
            SQL += "ASNT_PROD_ID,ASNT_PROD_TALLA,ASNT_PROD_COLOR,ASNT_ALMA_AX) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += "'" & vProducto & "',"
            SQL += "'" & vTalla & "',"
            SQL += "'" & vColor & "',"
            SQL += "'" & vAlmacen & "')"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                    ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                    , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                      ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, _
                                      ByVal vAuxiliarString As String, ByVal vWebServiceName As String, ByVal vForeCodi As String, ByVal vCacrCodi As String, ByVal vForeCodiAx As String, ByVal vLinDocu As String, ByVal vNada As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,ASNT_FORE_CODI,ASNT_CACR_CODI,ASNT_FORE_CODI_AX,ASNT_LIN_DOCU) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += "'" & vForeCodi & "',"
            SQL += "'" & vCacrCodi & "',"
            SQL += "'" & vForeCodiAx & "','" & vLinDocu & "')"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                   ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                   , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                     ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, _
                                     ByVal vAuxiliarString As String, ByVal vWebServiceName As String, ByVal vForeCodi As String, ByVal vCacrCodi As String, ByVal vForeCodiAx As String, ByVal vLinDocu As String, ByVal vNada As String, ByVal vFactura As String, ByVal vSerie As String, ByVal vStatus As Integer)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,ASNT_FORE_CODI,ASNT_CACR_CODI,ASNT_FORE_CODI_AX,ASNT_LIN_DOCU,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_AX_STATUS) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += "'" & vForeCodi & "',"
            SQL += "'" & vCacrCodi & "',"
            SQL += "'" & vForeCodiAx & "','" & vLinDocu & "',"
            SQL += "'" & vFactura & "',"
            SQL += "'" & vSerie & "',"
            SQL += vStatus & ")"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle2(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                  ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                  , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                    ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, _
                                    ByVal vAuxiliarString As String, ByVal vWebServiceName As String, ByVal vForeCodi As String, ByVal vCacrCodi As String, ByVal vForeCodiAx As String, ByVal vLinDocu As String, ByVal vNada As String, ByVal vFactura As String, ByVal vSerie As String, ByVal vStatus As Integer, ByVal vDepartamento As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,ASNT_FORE_CODI,ASNT_CACR_CODI,ASNT_FORE_CODI_AX,ASNT_LIN_DOCU,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_AX_STATUS,ASNT_DPTO_DESC) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += "'" & vForeCodi & "',"
            SQL += "'" & vCacrCodi & "',"
            SQL += "'" & vForeCodiAx & "','" & vLinDocu & "',"
            SQL += "'" & vFactura & "',"
            SQL += "'" & vSerie & "',"
            SQL += vStatus & ",'"
            SQL += vDepartamento & "')"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                       ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, _
                                       ByVal vAuxiliarString As String, ByVal vWebServiceName As String, ByVal vDptoCodi As String, _
                                       ByVal vTipoProduccion As String, ByVal vProdId As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,ASNT_DPTO_CODI,ASNT_TIPO_PROD,ASNT_PROD_ID) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "','" & vDptoCodi & "','" & vTipoProduccion & "','" & vProdId & "')"



            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                    ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                    , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                      ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, _
                                      ByVal vAuxiliarString As String, ByVal vWebServiceName As String, ByVal vLinVcre As Double, ByVal vLinVLiq As Double, ByVal vLinVImp1 As Double, ByVal vLinDocu As String, _
                                      ByVal vDocuValo As Double, ByVal vDptoCodi As String, ByVal vPorceIgic As Double, ByVal vTipoFactura As Integer, ByVal vCodigoClienteNewHotel As String, ByVal vProdId As String, ByVal vNada As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,ASNT_LIN_VCRE,"
            SQL += "ASNT_LIN_VLIQ,ASNT_LIN_IMP1,ASNT_LIN_DOCU,ASNT_DOCU_VALO,ASNT_DPTO_CODI,ASNT_LIN_TIIMP,ASNT_TIPOFACTURA_AX,ASNT_NHCUSTACCOUNT,ASNT_PROD_ID) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += vLinVcre & "," & vLinVLiq & "," & vLinVImp1 & ",'" & vLinDocu & "'," & vDocuValo & ",'" & vDptoCodi & "'," & vPorceIgic & "," & vTipoFactura & ",'" & vCodigoClienteNewHotel & "','" & vProdId & "')"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto & " " & Linea

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    '**** AQUI / RE AQUI AÑADO RECEPCION DE RESERVA Y EJERCICIO PIDE GREGORIO 24 SEPTIEMBRE 2013
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                    ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                    , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                      ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, _
                                      ByVal vAuxiliarString As String, ByVal vWebServiceName As String, ByVal vLinVcre As Double, ByVal vLinVLiq As Double, ByVal vLinVImp1 As Double, ByVal vLinDocu As String, _
                                      ByVal vDocuValo As Double, ByVal vDptoCodi As String, ByVal vPorceIgic As Double, ByVal vTipoFactura As Integer, ByVal vCodigoClienteNewHotel As String, _
                                      ByVal vProdId As String, ByVal vNada As String, ByVal vCcex_Codi As String, ByVal vNada1 As String, ByVal vBookIds As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,ASNT_LIN_VCRE,"
            SQL += "ASNT_LIN_VLIQ,ASNT_LIN_IMP1,ASNT_LIN_DOCU,ASNT_DOCU_VALO,ASNT_DPTO_CODI,ASNT_LIN_TIIMP,ASNT_TIPOFACTURA_AX,ASNT_NHCUSTACCOUNT,ASNT_PROD_ID,ASNT_CCEX_CODI,ASNT_RESE_FACT) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += vLinVcre & "," & vLinVLiq & "," & vLinVImp1 & ",'" & vLinDocu & "'," & vDocuValo & ",'" & vDptoCodi & "'," & vPorceIgic & "," & vTipoFactura & ",'" & vCodigoClienteNewHotel & "','" & vProdId & "','" & vCcex_Codi & "','" & vBookIds & "')"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto & " " & Linea

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                   ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                   , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                     ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, _
                                     ByVal vAuxiliarString As String, ByVal vWebServiceName As String, ByVal vLinVcre As Double, ByVal vLinVLiq As Double, ByVal vLinVImp1 As Double, ByVal vLinDocu As String, _
                                     ByVal vDocuValo As Double, ByVal vDptoCodi As String, ByVal vPorceIgic As Double, ByVal vTipoFactura As Integer, ByVal vCodigoClienteNewHotel As String, ByVal vTipoVenta As String, ByVal vProdId As String, ByVal vNada As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_WEBSERVICE_NAME,ASNT_LIN_VCRE,"
            SQL += "ASNT_LIN_VLIQ,ASNT_LIN_IMP1,ASNT_LIN_DOCU,ASNT_DOCU_VALO,ASNT_DPTO_CODI,ASNT_LIN_TIIMP,ASNT_TIPOFACTURA_AX,ASNT_NHCUSTACCOUNT,ASNT_TIPO_VENTA,ASNT_PROD_ID) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vAuxiliarString & "',"
            SQL += "'" & vWebServiceName & "',"
            SQL += vLinVcre & "," & vLinVLiq & "," & vLinVImp1 & ",'" & vLinDocu & "'," & vDocuValo & ",'" & vDptoCodi & "'," & vPorceIgic & "," & vTipoFactura & ",'" & vCodigoClienteNewHotel & "','" & vTipoVenta & "','" & vProdId & "')"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & vAmpcpto

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
                    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)
                End If
            End If


        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
            End If

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
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, " Localiza Cuenta Contable SPYRO")
            End If

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
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
            End If

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
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
            End If

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
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
            End If

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
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAA")
            End If

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
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileFV")
            End If

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
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileFV")
            End If

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
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileVF")
            End If

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
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileIV")
            End If

        End Try
    End Sub
    Private Sub CerrarFichero()
        Try
            Filegraba.Close()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try
    End Sub
    Private Sub CierraConexiones()
        Try
            Me.DbLeeCentral.CerrarConexion()
            Me.DbGrabaCentral.CerrarConexion()
            Me.DbLeeHotel.CerrarConexion()
            Me.DbLeeHotelAux.CerrarConexion()
            Me.DbSpyro.CerrarConexion()
            Me.DbNewPos.CerrarConexion()
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
            If mMostrarMensajes = True Then
                MsgBox(EX.Message, MsgBoxStyle.Information, "Facturas sin Cuenta Contable")
            End If

        End Try
    End Sub
#End Region
#Region "ASIENTO-1"
    Private Sub PendienteFacturarTotal()

        Try


            Dim Total As Double

            Dim DeduceBonoComoFactura As Boolean = True



            SQL = "SELECT "
            SQL += "ROUND (SUM (MOVI_VLIQ), 2)"
            SQL += " FROM VNHT_MOVH TNHT_MOVI ,TNHT_SERV"
            SQL += " WHERE MOVI_DATR= '" & Me.mFecha & "'"
            SQL += " AND TNHT_MOVI.SERV_CODI(+) = TNHT_SERV.SERV_CODI "


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



            If mParaConectaNewGolf = 0 Or Me.mTratarBonos = False Then Exit Sub
            ' Calcular cuento de la produccion es de venta de bonos  (esther)

            SQL = "SELECT "
            SQL += "  SIGN(MOVI_IMPT) * ABS(MOVI_VLIQ) AS LIQUIDO ,SIGN(MOVI_IMPT) *ABS(MOVI_IMP1) IMPUESTO,NVL(MOVI_OBSV,'?') AS MOVI_OBSV "
            SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_ADIC"
            SQL += " WHERE TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI"
            SQL += " AND MOVI_FECH = '" & Me.mFecha & "'"
            SQL += " AND TNPL_MOVI.ADIC_CODI in (select adic_codi from " & Me.mParaUsuarioNewGolf & ".tnpl_adic where adic_tipo = 3)"

            ' EXCLUIR BONOS ASOCIACION DE CAMPOS 
            SQL += " AND TIAD_CODI <> " & Me.MparaTipoBonoAsociacion

            ' SOLO NO ANULADOS 
            ' OJO REVISAR ESTE ASPECTO CUANDO SE ANULA UN BONO EN NEWGOLF , SE ANULA LA PRODUCCION EN NEWHOTEL EL MISMO DIA 
            SQL += " AND MOVI_ANUL = 0 "
            SQL += " ORDER BY MOVI_CODI ASC"


            Me.DbLeeHotel.TraerLector(SQL)


            If DeduceBonoComoFactura = True Then
                Linea = 8999
            End If



            While Me.DbLeeHotel.mDbLector.Read

                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("LIQUIDO"), Double) * -1

                If Total <> 0 Then
                    Linea = Linea + 1
                    Me.mTipoAsiento = "DEBE"

                    Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorDebe, "(-) Deducción " & CType(Me.DbLeeHotel.mDbLector("MOVI_OBSV"), String), Total, "SI", "", "", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorDebe, "(-) Deducción " & CType(Me.DbLeeHotel.mDbLector("MOVI_OBSV"), String), Total)

                End If


            End While
            Me.DbLeeHotel.mDbLector.Close()

            ' CONTRAPARTIDA POR DEPARTAMENTO  ESTHER (1)  ( PROBAR EL CLIENTE ESTA USANDO OPCION 2 DEBAJO)

            Dim vCentroCosto As String
            Dim TipoProduccion As String
            Dim CodigoCompuesto As String


            SQL = "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI SERVICIO,TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CTB1,'0') CUENTA ,NVL(BLAL_DESC,'OTROS INGRESOS') AS BLOQUE,TNHT_BLAL.BLAL_CODI,"
            SQL += "ROUND(SUM(TNHT_MOVI.MOVI_VLIQ), 2) LIQUIDO "
            SQL += ",NVL(TNHT_BLAL.BLAL_CODI,'000') AS CODIGOBLOQUE "
            SQL += " FROM VNHT_MOVH TNHT_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_ADIC," & Me.mParaUsuarioNewGolf & ".TNPL_MOVI,TNHT_SERV"
            SQL += ",TNHT_ALOJ,TNHT_BLAL "
            SQL += " WHERE TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI"
            SQL += " AND TNHT_MOVI.MOVI_CODI = TNPL_MOVI.NEWH_CODI"
            SQL += " AND TNHT_MOVI.MOVI_DARE = TNPL_MOVI.NEWH_DARE"
            SQL += " AND TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI"


            SQL += " AND TNHT_MOVI.ALOJ_CODI = TNHT_ALOJ.ALOJ_CODI(+) "
            SQL += " AND TNHT_ALOJ.BLAL_CODI = TNHT_BLAL.BLAL_CODI(+) "

            SQL += " AND MOVI_DATR= '" & Me.mFecha & "'"
            SQL += " AND TNPL_MOVI.ADIC_CODI IN"
            SQL += "(SELECT ADIC_CODI FROM GMS.TNPL_ADIC WHERE ADIC_TIPO = 3)"

            ' EXCLUIR BONOS ASOCIACION DE CAMPOS 
            SQL += " AND TIAD_CODI <> " & Me.MparaTipoBonoAsociacion

            SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,TNHT_SERV.SERV_COMS,BLAL_DESC,TNHT_BLAL.BLAL_CODI"

            SQL += " ORDER BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1"

            ' CONTRAPARTIDA POR DEPARTAMENTO  ESTHER (2)  ( BUSCA EL DPTO DE NEWHOTEL EN NEWGOLF POR PROBLEMA EN ENLACE  )

            'SQL = "SELECT SUM(SIGN(MOVI_IMPT) *ABS(MOVI_VLIQ)) AS LIQUIDO, SUM(SIGN(MOVI_IMPT) *ABS(MOVI_IMP1)) AS IMPUESTO,"
            'SQL += "TNHT_SERV.SERV_CODI AS SERVICIO, TNHT_SERV.SERV_DESC DEPARTAMENTO, NVL(TNHT_SERV.SERV_CTB1,   '0') CUENTA, NVL(BLAL_DESC,   'OTROS INGRESOS') AS BLOQUE "
            'SQL += " FROM  " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_ADCO," & Me.mParaUsuarioNewGolf & ".TNPL_ADIC," & "TNHT_SERV, TNHT_ALOJ, TNHT_BLAL"

            'SQL += " WHERE TNPL_MOVI.ADIC_CODI = TNPL_ADCO.ADIC_CODI "
            'SQL += " AND TNPL_ADCO.SERV_CODI = TNHT_SERV.SERV_CODI"
            'SQL += " AND TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI"
            'SQL += " AND TNPL_MOVI.ALOJ_CODI = TNHT_ALOJ.ALOJ_CODI(+)"
            'SQL += " AND TNHT_ALOJ.BLAL_CODI = TNHT_BLAL.BLAL_CODI(+)"

            'SQL += " AND MOVI_FECH = '" & Me.mFecha & "'"
            'SQL += " AND TNPL_MOVI.ADIC_CODI IN (SELECT ADIC_CODI FROM GMS.TNPL_ADIC WHERE ADIC_TIPO = 3)"

            'SQL += " AND MOVI_ANUL = 0"

            ' EXCLUIR BONOS ASOCIACION DE CAMPOS 
            'SQL += " AND TIAD_CODI <> " & Me.MparaTipoBonoAsociacion
            'SQL += " GROUP BY TNHT_SERV.SERV_CODI, TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,BLAL_DESC"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                'If IsDBNull(Me.DbLeeHotel.mDbLector("BLAL_CODI")) = False Then
                'SQL = "SELECT NVL(SERV_COMS,'0') FROM TH_SERV_BLAL WHERE "
                'SQL += " HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND "
                'SQL += " HOTEL_EMP_COD = '" & Me.mEmpCod & "' AND "
                'SQL += "SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "' AND "
                'SQL += "BLAL_CODI = '" & CType(Me.DbLeeHotel.mDbLector("BLAL_CODI"), String) & "'"
                'vCentroCosto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)
                'CodigoCompuesto = Me.DbLeeHotel.mDbLector("SERVICIO") & Me.DbLeeHotel.mDbLector("BLAL_CODI")
                'Else
                ' OTROS INGRESOS ( se coge centro de costo por defecto de la tabla de servicios de newhotel)
                'SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
                'vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                'CodigoCompuesto = vCentroCosto
                'End If


                ' EL DEPARTAMENTO NO EXISTE EN LA TABLA DE CENTROS DE COSTO MIA

                'If IsNothing(vCentroCosto) = True Then
                'MsgBox("Atención  el Departamento " & CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " no existe en la Tabla de Centros de Costo por Bloques de Alojamiento " & vbCrLf & vbCrLf & "Se asume sin Centro de Costo", MsgBoxStyle.Information, "Atención Posible  nuevo Departamento Creado ")
                'vCentroCosto = "0"
                'End If


                vCentroCosto = "0"
                If Me.mParaIngresoPorHabitacion = 1 Then
                    CodigoCompuesto = Me.DbLeeHotel.mDbLector("CODIGOBLOQUE") & "-" & Me.DbLeeHotel.mDbLector("SERVICIO")
                Else
                    CodigoCompuesto = Me.DbLeeHotel.mDbLector("SERVICIO")
                End If


                If Me.DbLeeHotel.mDbLector("SERVICIO") = Me.mParaServCodiBonos Then
                    TipoProduccion = "BONOS"
                ElseIf Me.DbLeeHotel.mDbLector("SERVICIO") = Me.mParaServCodiBonosAsoc Then
                    ' Pat0006
                    ' TipoProduccion = "BONOS ASOC"
                    TipoProduccion = "NORMAL"
                Else
                    TipoProduccion = "NORMAL"
                End If


                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("LIQUIDO"), Double) * -1

                If Total <> 0 Then
                    Linea = Linea + 1

                    Me.mTipoAsiento = "HABER"
                    'DESPEODUCCION DE BONOS 
                    Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "(-) Deducción Dpto. " & CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("BLOQUE"), String), Total, "NO", "", vCentroCosto, "SI", "", "SAT_NHCreateProdOrdersQueryService", CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String), TipoProduccion, CodigoCompuesto)

                    ' ENVIAR LA DESPRODUCCION DE BONOS COMO FACTURA PENDIENTE ( FALTA DESARROLLO) 

                    'Me.InsertaOracle("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, "(-) Deducción Dpto. " & CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("BLOQUE"), String), Total, "NO", "DNI", "CONCEPTO", "SI", "COBRO", "SAT_NHCreateSalesOrdersQueryService", Total, Total, 0, "AX", Total, CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String), 0, 3, 0)
                    '  Me.InsertaOracle("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, "(-) Deducción Dpto. " & CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("BLOQUE"), String), Total, "NO", "DNI", "CONCEPTO", "SI", "COBRO", "SAT_NHCreateSalesOrdersQueryService", Total, Total, 0, "AX", Total, CodigoCompuesto, 0, 3, 0, "PENDIENTE DESARROLLO", "NADA")


                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "(-) Deducción Dpto. " & CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("BLOQUE"), String), Total)
                    If vCentroCosto <> "0" Then
                        Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", vCentroCosto, Total)
                    End If

                End If


            End While
            Me.DbLeeHotel.mDbLector.Close()



        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try
    End Sub
    Private Sub VentasDepartamento()

        Dim Total As Double
        Dim vCentroCosto As String

        Dim TipoProduccion As String

        SQL = "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI SERVICIO,TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CTB1,'0') CUENTA ,"
        SQL += "ROUND (SUM (MOVI_VLIQ), 2) TOTAL "
        SQL += " FROM VNHT_MOVH TNHT_MOVI,TNHT_SERV"
        SQL += " WHERE (TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI) AND MOVI_DATR= '" & Me.mFecha & "'"
        ' SQL += " AND TNHT_SERV.SERV_CTB1 <> '#'"
        SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,TNHT_SERV.SERV_COMS"
        SQL += " ORDER BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            If Me.DbLeeHotel.mDbLector("SERVICIO") = Me.mParaServCodiBonos Then
                TipoProduccion = "BONOS"
            ElseIf Me.DbLeeHotel.mDbLector("SERVICIO") = Me.mParaServCodiBonosAsoc Then
                ' Pat0006
                ' TipoProduccion = "BONOS ASOC"
                TipoProduccion = "NORMAL"

            Else
                TipoProduccion = "NORMAL"
            End If

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)


            If Total <> 0 Then
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total, "NO", "", vCentroCosto, "SI", "", "SAT_NHCreateProdOrdersQueryService", CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String), TipoProduccion, CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String))
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String), Total)
                If vCentroCosto <> "0" Then
                    Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", vCentroCosto, Total)
                End If
            End If
        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub

   
    Private Sub VentasDepartamentoBloqueAxapta()

        Dim Total As Double
        Dim vCentroCosto As String
        Dim TipoProduccion As String

        Dim CodigoCompuesto As String

        Try


            ' INGRESO  POR BLOQUE

            SQL = "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI SERVICIO,TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CTB1,'0') CUENTA ,NVL(BLAL_DESC,'OTROS INGRESOS') AS BLOQUE,"
            SQL += "NVL(TNHT_BLAL.BLAL_CODI,'000') AS CODIGOBLOQUE, "
            SQL += "ROUND(SUM(MOVI_VLIQ), 2) TOTAL "
            SQL += " FROM VNHT_MOVH TNHT_MOVI,TNHT_SERV,TNHT_ALOJ,TNHT_BLAL"
            SQL += " WHERE TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "
            SQL += " AND TNHT_MOVI.ALOJ_CODI = TNHT_ALOJ.ALOJ_CODI(+) "
            SQL += " AND TNHT_ALOJ.BLAL_CODI = TNHT_BLAL.BLAL_CODI(+) "
            SQL += " AND MOVI_DATR= '" & Me.mFecha & "'"
            SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,TNHT_SERV.SERV_COMS,BLAL_DESC,TNHT_BLAL.BLAL_CODI"
            SQL += " ORDER BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1"


            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                vCentroCosto = "0"
                If Me.mParaIngresoPorHabitacion = 1 Then
                    CodigoCompuesto = Me.DbLeeHotel.mDbLector("CODIGOBLOQUE") & "-" & Me.DbLeeHotel.mDbLector("SERVICIO")
                Else
                    CodigoCompuesto = Me.DbLeeHotel.mDbLector("SERVICIO")
                End If

                ' tipo de produccion 
                If Me.DbLeeHotel.mDbLector("SERVICIO") = Me.mParaServCodiBonos Then
                    TipoProduccion = "BONOS"
                ElseIf Me.DbLeeHotel.mDbLector("SERVICIO") = Me.mParaServCodiBonosAsoc Then
                    ' Pat0006
                    ' TipoProduccion = "BONOS ASOC"
                    TipoProduccion = "NORMAL"

                Else
                    TipoProduccion = "NORMAL"
                End If

                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " + " & CType(Me.DbLeeHotel.mDbLector("BLOQUE"), String), Total, "NO", "", vCentroCosto, "SI", "", "SAT_NHCreateProdOrdersQueryService", CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String), TipoProduccion, CodigoCompuesto)
                    ' Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("BLOQUE"), String), Total, "NO", "", vCentroCosto, "SI", "", "SAT_NHCreateProdOrdersQueryService", CodigoCompuesto, TipoProduccion)
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DEPARTAMENTO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("BLOQUE"), String), Total)
                    If vCentroCosto <> "0" Then
                        Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), "0", vCentroCosto, Total)
                    End If
                End If
            End While
            Me.DbLeeHotel.mDbLector.Close()

        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try
    End Sub
#End Region
#Region "ASIENTO-2 PAGOS A CUENTA"
    Private Sub TotalPagosaCuentaVisas()
        Try
            Dim Total As Double
            SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA,"
            SQL += "NVL(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),'?') RECI_COBR,NVL(MOVI_NUDO,' ') MOVI_NUDO"
            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' excluir depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"


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
            '  SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CTBA"


            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read


                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Linea = Linea + 1
                    Me.AnticiposRecibidos = Me.AnticiposRecibidos + Total
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " Docu: " & CType(Me.DbLeeHotel.mDbLector("MOVI_NUDO"), String), Total, "NO", "", "", "SI", "ANTICIPO RECIBIDO")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " Docu: " & CType(Me.DbLeeHotel.mDbLector("MOVI_NUDO"), String), Total)
                End If


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
            End If

        End Try

    End Sub
    Private Sub TotalPagosaCuentaOtrasFormas()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,"
        SQL += "NVL(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),'?') RECI_COBR,NVL(MOVI_NUDO,' ') MOVI_NUDO"
        SQL += " FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"


        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"


        If Me.mTrataDebitoTpvnoFacturado = True Then
            ' EXCLUYE CIERRE DE CONTADO DE TPV
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'POS'"
            ' EXCLUYE CIERRE DE CONTADO DE GOLF
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"
        End If



        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        '   SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"
        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read


            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Linea = Linea + 1
                Me.AnticiposRecibidos = Me.AnticiposRecibidos + Total
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " Docu: " & CType(Me.DbLeeHotel.mDbLector("MOVI_NUDO"), String), Total, "NO", "", "", "SI", "ANTICIPO RECIBIDO")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " Docu: " & CType(Me.DbLeeHotel.mDbLector("MOVI_NUDO"), String), Total)
            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetallePagosaCuentaVisas()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
        SQL = SQL & " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA FROM VNHT_MOVH TNHT_MOVI,"
        SQL = SQL & " TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mTrataDebitoTpvnoFacturado = True Then
            ' EXCLUYE CIERRE DE CONTADO DE TPV
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'POS'"
            ' EXCLUYE CIERRE DE CONTADO DE GOLF
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"
        End If



        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read


            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)
            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetallePagosaCuentaAx()

        '' anticipos ax

        Dim Total As Double
        Dim Descripcion As String = ""
        Dim TipoCliente As Integer
        Dim Nif As String
        Dim FormaCobro As String = ""




        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
        SQL = SQL & " TNHT_CACR.CACR_DESC TARJETANOMBRE,MOVI_DAVA, "

        SQL = SQL & " NVL(TNHT_RESE.RESE_CODI ,0) AS RESE_CODI,NVL(TNHT_RESE.RESE_ANCI ,0) AS RESE_ANCI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_CODI ,'?') AS CCEX_CODI,NVL(TNHT_ENTI.ENTI_CODI ,'?') AS ENTI_CODI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_NUCO ,'?') AS CCEX_NUCO,NVL(TNHT_ENTI.ENTI_NUCO ,'?') AS ENTI_NUCO,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_TITU ,'?') AS CCEX_TITU,NVL(TNHT_ENTI.ENTI_NOME ,'?') AS ENTI_NOME,"
        SQL = SQL & " NVL(TNHT_ENTI.ENTI_FAMA ,'?') AS ENTI_FAMA,"
        SQL = SQL & " NVL(TNHT_CACR.CACR_CTBA,'0') TARJETA,"
        SQL = SQL & " NVL(TNHT_FORE.FORE_CTB1,'0') FORMA,"
        SQL = SQL & " FNHT_ESMOTO(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE) AS TRANSFERIDO"
        ' SQL = SQL & ",NVL(MOVI_NUDO,'?') AS RECIBO"
        SQL = SQL & ",TNHT_MOVI.MOVI_DARE || TNHT_MOVI.MOVI_CODI AS RECIBO "
        SQL = SQL & ",TNHT_MOVI.MOVI_CODI AS MOVI_CODI "




        SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,"
        SQL = SQL & " TNHT_CACR,TNHT_RESE,TNHT_CCEX,TNHT_ENTI,TNHT_FORE "
        SQL = SQL & " WHERE "
        SQL = SQL & "     TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI(+)"
        SQL = SQL & " AND TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+)"

        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"


        SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"

        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"


        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        '  MsgBox("OJO NO SE EXCLUYE DEPOSITOS ANTICIPADOS EN ANTICIPOS EN PRUEBA", MsgBoxStyle.Information, "FALTA DESARROLLO")
        ' excluir depositos anticipados 
        '   SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mTrataDebitoTpvnoFacturado = True Then
            ' EXCLUYE CIERRE DE CONTADO DE TPV
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'POS'"
            ' EXCLUYE CIERRE DE CONTADO DE GOLF
            ''  SQL += " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"
            ' EXCLUYE CIERRE DE CONTADO DE GOLF + ABONA BONO ASOCIACION EN CUENTA TPV 
            ' ESTO SE HACE PARA QUE AL TRANSFERIR DE LA CUENTA DE TPV A LA ASOCICION PASE EL POSITIVO Y EL NEGATIVO DE LA ANULACION DE LA CUENTA DE TPV
            '  If Me.mDebug = True Then
            SQL += "AND (TNHT_MOVI.UTIL_CODI <> 'GMS' OR TNHT_MOVI.UTIL_CODI =  'GMS'  AND FNHT_ESMOTO(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE) > 0 )"
            'Else
            '   SQL += " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"
            'End If
        End If



        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read

            If CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), String) = "0" Then
                ' ES UN NO ALOJADO
                TipoCliente = 2
                Nif = CType(Me.DbLeeHotel.mDbLector("CCEX_NUCO"), String)
                Descripcion = "No Alo : [" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CCEX_TITU"), String)
            Else
                ' NO ES UN NO ALOJADO
                ' ES CLIENTE DIRECTO
                If CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) = "?" Then
                    TipoCliente = 3
                    Nif = Me.mClientesContadoCif

                    Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String)
                Else ' ES CLIENTE DE AGENCIA
                    If CType(Me.DbLeeHotel.mDbLector("ENTI_FAMA"), String) = "1" Then
                        ' TTOO NORMAL
                        TipoCliente = 1
                        Nif = CType(Me.DbLeeHotel.mDbLector("ENTI_NUCO"), String)
                        Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    Else
                        ' TTOO ON LINE ( EL ANTICIPO LE LO CARGAMOS ACLIENTES CONTADO
                        TipoCliente = 3
                        Nif = Me.mClientesContadoCif

                        Descripcion = "[* TTOO WEB ] Rva : " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    End If
                End If
            End If

            If CType(Me.DbLeeHotel.mDbLector("TRANSFERIDO"), String) = "1" Then
                Descripcion = "* " & Descripcion
            End If

            If CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) = "0" Then
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("FORMA"), String)
            Else
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String)
            End If

            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "ANTICIPO", "SAT_NHPrePaymentService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "NADA", "NADA", "NADA", CInt(Me.DbLeeHotel.mDbLector("MOVI_CODI")))
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total)
            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetallePagosaCuentaAxSaldo()

        '' anticipos ax

        Dim Total As Double
        Dim Descripcion As String = ""
        Dim TipoCliente As Integer
        Dim Nif As String
        Dim FormaCobro As String = ""



        If Me.mParaConectaNewGolf = 1 Then

            SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(TNHT_RESE.RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
            SQL = SQL & " TNHT_CACR.CACR_DESC TARJETANOMBRE,MOVI_DAVA, "

            SQL = SQL & " NVL(TNHT_RESE.RESE_CODI ,0) AS RESE_CODI,NVL(TNHT_RESE.RESE_ANCI ,0) AS RESE_ANCI,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_CODI ,'?') AS CCEX_CODI,NVL(TNHT_ENTI.ENTI_CODI ,'?') AS ENTI_CODI,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_NUCO ,'?') AS CCEX_NUCO,NVL(TNHT_ENTI.ENTI_NUCO ,'?') AS ENTI_NUCO,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_TITU ,'?') AS CCEX_TITU,NVL(TNHT_ENTI.ENTI_NOME ,'?') AS ENTI_NOME,"
            SQL = SQL & " NVL(TNHT_ENTI.ENTI_FAMA ,'?') AS ENTI_FAMA,"
            SQL = SQL & " NVL(TNHT_CACR.CACR_CTBA,'0') TARJETA,"
            SQL = SQL & " NVL(TNHT_FORE.FORE_CTB1,'0') FORMA,"
            SQL = SQL & " NVL(MOVI_DESC,'?') AS MOVI_DESC, "
            SQL = SQL & " MOVI_CODI || MOVI_DARE AS MOVI_ID "

            ' SQL = SQL & ",NVL(MOVI_NUDO,'?') AS RECIBO"
            SQL = SQL & ",TNHT_MOVI.MOVI_DARE || TNHT_MOVI.MOVI_CODI AS RECIBO "



            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,"
            SQL = SQL & " TNHT_CACR,TNHT_RESE,TNHT_CCEX,TNHT_ENTI,TNHT_FORE, "
            SQL = SQL & " TNHT_FACT,GMS.TNPL_FACT "
            SQL = SQL & " WHERE "
            SQL = SQL & "     TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+)"

            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"


            SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"


            SQL = SQL & " AND TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI(+)"

            SQL = SQL & " AND TNHT_MOVI.FACT_EXTE = TNPL_FACT.FACT_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.SEFA_EXTE = TNPL_FACT.SEFA_CODI(+)"

            ' FILTRO 
            ' NO FACTURADO
            SQL = SQL & "AND (TNHT_MOVI.FACT_CODI IS NULL AND TNHT_MOVI.FACT_EXTE IS NULL "
            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR < " & "'" & "01/09/2009" & "' "
            ' FACTURADO DESPUES DEL DIA 1 
            SQL = SQL & " OR TNHT_MOVI.MOVI_DATR < " & "'" & "01/09/2009" & "' "
            SQL = SQL & " AND TNHT_FACT.FACT_DAEM >= " & "'" & "01/09/2009" & "'"

            SQL = SQL & " OR TNHT_MOVI.MOVI_DATR < " & "'" & "01/09/2009" & "' "
            SQL = SQL & " AND TNPL_FACT.FACT_DAEM >= " & "'" & "01/09/2009" & "')"

            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"

            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"
            SQL = SQL & " AND TNHT_MOVI.MOVI_ANUL = 0 AND TNHT_MOVI.MOVI_CORR = 0 "


            If Me.mTrataDebitoTpvnoFacturado = True Then
                ' EXCLUYE CIERRE DE CONTADO DE TPV
                SQL += " AND TNHT_MOVI.UTIL_CODI <> 'POS'"
                ' EXCLUYE CIERRE DE CONTADO DE GOLF
                SQL += " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"
            End If


            If Me.mUsaTnhtMoviAuto = True Then
                SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0'"

            End If
            SQL += " ORDER BY MOVI_DATR ASC"
        Else

            SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(TNHT_RESE.RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
            SQL = SQL & " TNHT_CACR.CACR_DESC TARJETANOMBRE,MOVI_DAVA, "

            SQL = SQL & " NVL(TNHT_RESE.RESE_CODI ,0) AS RESE_CODI,NVL(TNHT_RESE.RESE_ANCI ,0) AS RESE_ANCI,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_CODI ,'?') AS CCEX_CODI,NVL(TNHT_ENTI.ENTI_CODI ,'?') AS ENTI_CODI,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_NUCO ,'?') AS CCEX_NUCO,NVL(TNHT_ENTI.ENTI_NUCO ,'?') AS ENTI_NUCO,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_TITU ,'?') AS CCEX_TITU,NVL(TNHT_ENTI.ENTI_NOME ,'?') AS ENTI_NOME,"
            SQL = SQL & " NVL(TNHT_ENTI.ENTI_FAMA ,'?') AS ENTI_FAMA,"
            SQL = SQL & " NVL(TNHT_CACR.CACR_CTBA,'0') TARJETA,"
            SQL = SQL & " NVL(TNHT_FORE.FORE_CTB1,'0') FORMA,"
            SQL = SQL & " NVL(MOVI_DESC,'?') AS MOVI_DESC, "
            SQL = SQL & " MOVI_CODI || MOVI_DARE AS MOVI_ID "

            ' SQL = SQL & ",NVL(MOVI_NUDO,'?') AS RECIBO"
            SQL = SQL & ",TNHT_MOVI.MOVI_DARE || TNHT_MOVI.MOVI_CODI AS RECIBO "



            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,"
            SQL = SQL & " TNHT_CACR,TNHT_RESE,TNHT_CCEX,TNHT_ENTI,TNHT_FORE, "
            SQL = SQL & " TNHT_FACT "
            SQL = SQL & " WHERE "
            SQL = SQL & "     TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+)"

            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"


            SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"


            SQL = SQL & " AND TNHT_MOVI.SEFA_CODI = TNHT_FACT.SEFA_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.FACT_CODI = TNHT_FACT.FACT_CODI(+)"


            ' FILTRO 
            ' NO FACTURADO
            SQL = SQL & "AND (TNHT_MOVI.FACT_CODI IS NULL AND TNHT_MOVI.FACT_EXTE IS NULL "
            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR < " & "'" & "01/09/2009" & "' "
            ' FACTURADO DESPUES DEL DIA 1 
            SQL = SQL & " OR TNHT_MOVI.MOVI_DATR < " & "'" & "01/09/2009" & "' "
            SQL = SQL & " AND TNHT_FACT.FACT_DAEM >= " & "'" & "01/09/2009" & "')"


            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"

            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"
            SQL = SQL & " AND TNHT_MOVI.MOVI_ANUL = 0 AND TNHT_MOVI.MOVI_CORR = 0 "


            If Me.mTrataDebitoTpvnoFacturado = True Then
                ' EXCLUYE CIERRE DE CONTADO DE TPV
                SQL += " AND TNHT_MOVI.UTIL_CODI <> 'POS'"
                ' EXCLUYE CIERRE DE CONTADO DE GOLF
                SQL += " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"
            End If


            If Me.mUsaTnhtMoviAuto = True Then
                SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0'"

            End If
            SQL += " ORDER BY MOVI_DATR ASC"
        End If


        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read

            If CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), String) = "0" Then
                If CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) <> "?" Then
                    ' ES UN NO ALOJADO
                    TipoCliente = 2
                    Nif = CType(Me.DbLeeHotel.mDbLector("CCEX_NUCO"), String)
                    Descripcion = "No Alo : [" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CCEX_TITU"), String)

                Else
                    ' ES UN movimiento sin cuenta 
                    TipoCliente = 2
                    Nif = Me.mClientesContadoCif
                    Descripcion = "Sin Cta  : [" & CType(Me.DbLeeHotel.mDbLector("MOVI_ID"), String) & "] " & "SIN CUENTA"

                End If
            Else
                ' NO ES UN NO ALOJADO
                ' ES CLIENTE DIRECTO
                If CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) = "?" Then
                    TipoCliente = 3
                    Nif = Me.mClientesContadoCif

                    Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String)
                Else ' ES CLIENTE DE AGENCIA
                    If CType(Me.DbLeeHotel.mDbLector("ENTI_FAMA"), String) = "1" Then
                        ' TTOO NORMAL
                        TipoCliente = 1
                        Nif = CType(Me.DbLeeHotel.mDbLector("ENTI_NUCO"), String)
                        Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    Else
                        ' TTOO ON LINE ( EL ANTICIPO LE LO CARGAMOS ACLIENTES CONTADO
                        TipoCliente = 3
                        Nif = Me.mClientesContadoCif

                        Descripcion = "[* TTOO WEB ] Rva : " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    End If
                End If
            End If



            If CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) = "0" Then
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("FORMA"), String)
            Else
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String)
            End If

            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String) & " + " & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), "SI", "ANTICIPO", "SAT_NHPrePaymentService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "NADA", "NADA", "NADA")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total)
            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetallePagosaCuentaOtrasFormas()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE,TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_DAVA FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"

        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"


        If Me.mTrataDebitoTpvnoFacturado = True Then
            ' EXCLUYE CIERRE DE CONTADO DE TPV
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'POS'"
            ' EXCLUYE CIERRE DE CONTADO DE GOLF
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"
        End If



        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read


            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)
            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetallePagosaCuentaOtrasFormasAx()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE,TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_DAVA FROM "
        SQL = SQL & " VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_RESE,TNHT_CCEX,TNHT_ENTI WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"

        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"
        SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"

        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"

        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"


        If Me.mTrataDebitoTpvnoFacturado = True Then
            ' EXCLUYE CIERRE DE CONTADO DE TPV
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'POS'"
            ' EXCLUYE CIERRE DE CONTADO DE GOLF
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"
        End If



        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read


            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "ANTICIPO", "SAT_NHPrePaymentService")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)
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
            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' SOLO VISAS CON COMISION
            SQL = SQL & " AND TNHT_CACR.CACR_COMI > 0 "
            ' excluir depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"


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
                    Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", "", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision)

                    Linea = Linea + 1
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTAGASTO"), String), Me.mIndicadorDebe, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", vCentroCosto, "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTAGASTO"), String), Me.mIndicadorDebe, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision)
                End If



            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
            End If

        End Try

    End Sub
    Private Sub TotalPagosaCuentaVisasComisionAx()
        Try
            Dim Total As Double
            Dim TotalComision As Double

            Dim vCentroCosto As String

            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA,NVL(TNHT_CACR.CACR_CONT,'0') CUENTAGASTO,TNHT_CACR.CACR_COMI"
            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' SOLO VISAS CON COMISION
            SQL = SQL & " AND TNHT_CACR.CACR_COMI > 0 "
            ' excluir depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"


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
                    Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", "", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision)

                    Linea = Linea + 1
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTAGASTO"), String), Me.mIndicadorDebe, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", vCentroCosto, "SI", "COMISION", "SAT_NHPrePaymentService")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTAGASTO"), String), Me.mIndicadorDebe, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision)
                End If



            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
            End If

        End Try

    End Sub
#End Region
#Region "ASIENTO-2 PAGOS A CUENTA NEWGOLF"
    Private Sub TotalPagosaCuentaVisasGolf()
        Try
            Dim Total As Double
            SQL = "SELECT SUM(MOVI_IMPT) TOTAL,CACR_DESC TARJETA,NVL(CACR_CTBA,'0') CUENTA"
            SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_CACO,TNHT_CACR "
            SQL += " WHERE "

            ' SOLO DEBITO 
            SQL = SQL & "  TNPL_MOVI.MOVI_DECR = '0'"


            SQL = SQL & " AND TNPL_MOVI.CACR_CODI = TNPL_CACO.CACR_COD1"
            ' TARJETAS DE CREDITO NEWHOTEL
            SQL = SQL & " AND TNPL_CACO.CACR_COD2 = TNHT_CACR.CACR_CODI"


            '  ES DEPOSITO ANTICIPADO

            SQL = SQL & "  AND TNPL_MOVI.MOVI_DEAN = '1'"

            SQL = SQL & " AND TNPL_MOVI.MOVI_FECH = " & "'" & Me.mFecha & "'"
            SQL = SQL & " GROUP BY CACR_DESC,CACR_CTBA"


            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read

                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Linea = Linea + 1
                    Me.AnticiposRecibidos = Me.AnticiposRecibidos + Total
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 25, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)
                End If


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
            End If

        End Try

    End Sub
    Private Sub TotalPagosaCuentaOtrasFormasGolf()
        Dim Total As Double
        SQL = ""
        SQL = "SELECT SUM(MOVI_IMPT) TOTAL,"
        SQL += "TNPL_FPAG.FPAG_DESC TIPO,"
        SQL += "NVL(TNPL_FPAG.FPAG_CTB1,'0') AS CUENTANEWG,"
        SQL += "NVL(TNHT_FORE.FORE_CTB1,'0') AS CUENTANEWH "
        SQL += "FROM GMS.TNPL_MOVI,"
        SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
        SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPCO,"
        SQL += "TNHT_FORE "
        SQL += "WHERE "
        SQL += "  TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI "
        SQL += "AND TNPL_FPAG.FPAG_CODI  = TNPL_FPCO.FPAG_CODI(+) "
        SQL += "AND TNPL_FPCO.FORE_CODI  = TNHT_FORE.FORE_CODI "
        SQL += "AND TNPL_MOVI.CACR_CODI IS NULL "
        SQL += "AND TNPL_MOVI.MOVI_DECR = '0' "
        SQL += "AND TNPL_MOVI.MOVI_DEAN = '1' "
        SQL += "AND TNPL_MOVI.MOVI_FECH =" & "'" & Me.mFecha & "' "
        SQL += "GROUP BY TNPL_FPAG.FPAG_DESC,TNPL_FPAG.FPAG_CTB1,TNHT_FORE.FORE_CTB1"


        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read


            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Linea = Linea + 1
                Me.AnticiposRecibidos = Me.AnticiposRecibidos + Total
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 25, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTANEWH"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTANEWH"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)
            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetallePagosaCuentaVisasGolf()
        Dim Total As Double

        SQL = "SELECT MOVI_IMPT TOTAL,CACR_DESC TARJETA,NVL(CACR_CTBA,'0') CUENTA,"
        SQL += "TNPL_MOVI.RESE_CODI || '/' || TNPL_MOVI.RESE_ANCI RESERVA,NVL(RESE_NAME,'?') CLIENTE,MOVI_FECH "

        SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_CACO,TNHT_CACR, " & Me.mParaUsuarioNewGolf & ".TNPL_RESE"
        SQL += " WHERE "
        SQL = SQL & "     TNPL_MOVI.RESE_CODI = TNPL_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNPL_MOVI.RESE_ANCI = TNPL_RESE.RESE_ANCI(+)"

        ' SOLO DEBITO 
        SQL = SQL & "   AND TNPL_MOVI.MOVI_DECR = '0'"

        SQL = SQL & " AND TNPL_MOVI.CACR_CODI = TNPL_CACO.CACR_COD1"
        ' TARJETAS DE CREDITO NEWHOTEL
        SQL = SQL & " AND TNPL_CACO.CACR_COD2 = TNHT_CACR.CACR_CODI"

        ' ES DEPOSITO ANTICIPADO

        SQL = SQL & "  AND TNPL_MOVI.MOVI_DEAN = '1'"

        SQL = SQL & " AND TNPL_MOVI.MOVI_FECH = " & "'" & Me.mFecha & "'"





        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read

            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 25, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_FECH"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)
            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetallePagosaCuentaOtrasFormasGolf()
        Dim Total As Double

        SQL = ""
        SQL = "SELECT MOVI_IMPT TOTAL,"
        SQL += "TNPL_FPAG.FPAG_DESC TIPO,"
        SQL += "NVL(TNPL_FPAG.FPAG_CTB1,'0') AS CUENTANEWG,"
        SQL += "NVL(TNHT_FORE.FORE_CTB1,'0') AS CUENTANEWH, "
        SQL += "TNPL_MOVI.RESE_CODI || '/' || TNPL_MOVI.RESE_ANCI RESERVA,NVL(RESE_NAME,'?') CLIENTE,MOVI_FECH "
        SQL += "FROM GMS.TNPL_MOVI,"
        SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
        SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPCO,"
        SQL += "TNHT_FORE, " & Me.mParaUsuarioNewGolf & ".TNPL_RESE"
        SQL += " WHERE "
        SQL = SQL & "     TNPL_MOVI.RESE_CODI = TNPL_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNPL_MOVI.RESE_ANCI = TNPL_RESE.RESE_ANCI(+)"

        SQL += "AND TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI "
        SQL += "AND TNPL_FPAG.FPAG_CODI  = TNPL_FPCO.FPAG_CODI(+) "
        SQL += "AND TNPL_FPCO.FORE_CODI  = TNHT_FORE.FORE_CODI "
        SQL += "AND TNPL_MOVI.CACR_CODI IS NULL "
        SQL += "AND TNPL_MOVI.MOVI_DECR = '0' "
        SQL += "AND TNPL_MOVI.MOVI_DEAN = '1' "
        SQL += "AND TNPL_MOVI.MOVI_FECH =" & "'" & Me.mFecha & "' "


        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read

            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 25, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_FECH"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)
            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetallePagosaCuentaGolfAx()
        Dim Total As Double

        Dim Descripcion As String = ""
        Dim TipoCliente As Integer
        Dim Nif As String
        Dim FormaCobro As String = ""


      
        SQL = ""
        SQL = "SELECT MOVI_IMPT TOTAL,"
        SQL += "TNPL_FPAG.FPAG_DESC TIPO,"
        SQL += "NVL(TNPL_FPAG.FPAG_CTB1,'0') AS CUENTANEWG,"
        SQL += "NVL(TNHT_FORE.FORE_CTB1,'0') AS CUENTANEWH, "
        SQL += "TNPL_MOVI.RESE_CODI || '/' || TNPL_MOVI.RESE_ANCI RESERVA,NVL(RESE_NAME,'?') CLIENTE,MOVI_FECH "
        SQL += " ,TNHT_CACR.CACR_DESC TARJETANOMBRE,MOVI_FECV "

        SQL = SQL & " ,NVL(TNPL_RESE.RESE_CODI ,0) AS RESE_CODI,NVL(TNPL_RESE.RESE_ANCI ,0) AS RESE_ANCI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_CODI ,'?') AS CCEX_CODI,NVL(TNHT_ENTI.ENTI_CODI ,'?') AS ENTI_CODI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_NUCO ,'?') AS CCEX_NUCO,NVL(TNHT_ENTI.ENTI_NUCO ,'?') AS ENTI_NUCO,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_TITU ,'?') AS CCEX_TITU,NVL(TNHT_ENTI.ENTI_NOME ,'?') AS ENTI_NOME,"


        SQL = SQL & " NVL(TNHT_CACR.CACR_CTBA,'0') TARJETA,"
        SQL = SQL & " NVL(TNHT_FORE.FORE_CTB1,'0') FORMA, "
        SQL = SQL & " NVL(TNHT_ENTI.ENTI_FAMA ,'?') AS ENTI_FAMA "

        '   SQL = SQL & ",NVL(TNPL_MOVI.MOVI_NUDO,'?') AS RECIBO"
        SQL = SQL & ",TNPL_MOVI.MOVI_DATE || TNPL_MOVI.MOVI_CODI AS RECIBO "


        SQL += " FROM "
        SQL += Me.mParaUsuarioNewGolf & ".TNPL_MOVI,"
        SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
        SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPCO,"
        SQL += Me.mParaUsuarioNewGolf & ".TNPL_CACO, "
        SQL += Me.mParaUsuarioNewGolf & ".TNPL_RESE"
        SQL += " ,TNHT_CACR,TNHT_FORE,TNHT_ENTI,TNHT_CCEX"
        SQL += " WHERE "
        SQL = SQL & "     TNPL_MOVI.RESE_CODI = TNPL_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNPL_MOVI.RESE_ANCI = TNPL_RESE.RESE_ANCI(+)"

        SQL += "AND TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI "
        SQL += "AND TNPL_FPAG.FPAG_CODI  = TNPL_FPCO.FPAG_CODI(+) "
        SQL += "AND TNPL_FPCO.FORE_CODI  = TNHT_FORE.FORE_CODI(+) "
        ' TARJETAS DE CREDITO NEWHOTEL
        SQL = SQL & " AND TNPL_MOVI.CACR_CODI = TNPL_CACO.CACR_COD1(+)"
        SQL = SQL & " AND TNPL_CACO.CACR_COD2 = TNHT_CACR.CACR_CODI(+)"

        SQL = SQL & " AND TNPL_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
        SQL = SQL & " AND TNPL_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"

        SQL += " AND TNPL_MOVI.MOVI_DECR = '0' "
        SQL += " AND TNPL_MOVI.MOVI_DEAN = '1' "
        SQL += " AND TNPL_MOVI.MOVI_FECH =" & "'" & Me.mFecha & "' "


        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read



            '' nuevo 

            If CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), String) = "0" Then
                ' ES UN NO ALOJADO
                TipoCliente = 2
                Nif = CType(Me.DbLeeHotel.mDbLector("CCEX_NUCO"), String)
                Descripcion = "No Alo : [" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CCEX_TITU"), String)
            Else
                ' NO ES UN NO ALOJADO
                ' ES CLIENTE DIRECTO
                If CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) = "?" Then
                    TipoCliente = 3
                    Nif = Me.mClientesContadoCif

                    Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String)
                Else ' ES CLIENTE DE AGENCIA
                    If CType(Me.DbLeeHotel.mDbLector("ENTI_FAMA"), String) = "1" Then
                        ' TTOO NORMAL
                        TipoCliente = 1
                        Nif = CType(Me.DbLeeHotel.mDbLector("ENTI_NUCO"), String)
                        Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    Else
                        ' TTOO ON LINE ( EL ANTICIPO LE LO CARGAMOS ACLIENTES CONTADO
                        TipoCliente = 3
                        Nif = Me.mClientesContadoCif

                        Descripcion = "[* TTOO WEB ] Rva : " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    End If
                End If
            End If



            If CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) = "0" Then
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("FORMA"), String)
            Else
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String)
            End If





            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 25, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_FECV"), String), "SI", "ANTICIPO", "SAT_NHPrePaymentService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "NADA", "NADA", "NADA")
                ' Me.InsertaOracle("AC", 25, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_FECH"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)
            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetallePagosaCuentaGolfAxSaldo()
        Dim Total As Double

        Dim Descripcion As String = ""
        Dim TipoCliente As Integer
        Dim Nif As String
        Dim FormaCobro As String = ""



        SQL = ""
        SQL = "SELECT MOVI_IMPT TOTAL,"
        SQL += "TNPL_FPAG.FPAG_DESC TIPO,"
        SQL += "NVL(TNPL_FPAG.FPAG_CTB1,'0') AS CUENTANEWG,"
        SQL += "NVL(TNHT_FORE.FORE_CTB1,'0') AS CUENTANEWH, "
        SQL += "TNPL_MOVI.RESE_CODI || '/' || TNPL_MOVI.RESE_ANCI RESERVA,NVL(RESE_NAME,'?') CLIENTE,MOVI_FECH "
        SQL += " ,TNHT_CACR.CACR_DESC TARJETANOMBRE,MOVI_FECV "

        SQL = SQL & " ,NVL(TNPL_RESE.RESE_CODI ,0) AS RESE_CODI,NVL(TNPL_RESE.RESE_ANCI ,0) AS RESE_ANCI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_CODI ,'?') AS CCEX_CODI,NVL(TNHT_ENTI.ENTI_CODI ,'?') AS ENTI_CODI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_NUCO ,'?') AS CCEX_NUCO,NVL(TNHT_ENTI.ENTI_NUCO ,'?') AS ENTI_NUCO,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_TITU ,'?') AS CCEX_TITU,NVL(TNHT_ENTI.ENTI_NOME ,'?') AS ENTI_NOME,"


        SQL = SQL & " NVL(TNHT_CACR.CACR_CTBA,'0') TARJETA,"
        SQL = SQL & " NVL(TNHT_FORE.FORE_CTB1,'0') FORMA, "
        SQL = SQL & " NVL(TNHT_ENTI.ENTI_FAMA ,'?') AS ENTI_FAMA "
        '  SQL = SQL & ",NVL(TNPL_MOVI.MOVI_NUDO,'?') AS RECIBO"
        SQL = SQL & ",TNPL_MOVI.MOVI_DATE || TNPL_MOVI.MOVI_CODI AS RECIBO "




        SQL += " FROM "
        SQL += Me.mParaUsuarioNewGolf & ".TNPL_MOVI,"
        SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
        SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPCO,"
        SQL += Me.mParaUsuarioNewGolf & ".TNPL_CACO, "
        SQL += Me.mParaUsuarioNewGolf & ".TNPL_RESE"
        SQL += " ,TNHT_CACR,TNHT_FORE,TNHT_ENTI,TNHT_CCEX"
        SQL += " WHERE "
        SQL = SQL & "     TNPL_MOVI.RESE_CODI = TNPL_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNPL_MOVI.RESE_ANCI = TNPL_RESE.RESE_ANCI(+)"

        SQL += "AND TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI "
        SQL += "AND TNPL_FPAG.FPAG_CODI  = TNPL_FPCO.FPAG_CODI(+) "
        SQL += "AND TNPL_FPCO.FORE_CODI  = TNHT_FORE.FORE_CODI(+) "
        ' TARJETAS DE CREDITO NEWHOTEL
        SQL = SQL & " AND TNPL_MOVI.CACR_CODI = TNPL_CACO.CACR_COD1(+)"
        SQL = SQL & " AND TNPL_CACO.CACR_COD2 = TNHT_CACR.CACR_CODI(+)"

        SQL = SQL & " AND TNPL_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
        SQL = SQL & " AND TNPL_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"

        SQL += " AND TNPL_MOVI.MOVI_DECR = '0' "
        SQL += " AND TNPL_MOVI.MOVI_DEAN = '1' "
        SQL += " AND TNPL_MOVI.MOVI_FECH =" & "'" & Me.mFecha & "' "


        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read



            '' nuevo 

            If CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), String) = "0" Then
                ' ES UN NO ALOJADO
                TipoCliente = 2
                Nif = CType(Me.DbLeeHotel.mDbLector("CCEX_NUCO"), String)
                Descripcion = "No Alo : [" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CCEX_TITU"), String)
            Else
                ' NO ES UN NO ALOJADO
                ' ES CLIENTE DIRECTO
                If CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) = "?" Then
                    TipoCliente = 3
                    Nif = Me.mClientesContadoCif

                    Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String)
                Else ' ES CLIENTE DE AGENCIA
                    If CType(Me.DbLeeHotel.mDbLector("ENTI_FAMA"), String) = "1" Then
                        ' TTOO NORMAL
                        TipoCliente = 1
                        Nif = CType(Me.DbLeeHotel.mDbLector("ENTI_NUCO"), String)
                        Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    Else
                        ' TTOO ON LINE ( EL ANTICIPO LE LO CARGAMOS ACLIENTES CONTADO
                        TipoCliente = 3
                        Nif = Me.mClientesContadoCif

                        Descripcion = "[* TTOO WEB ] Rva : " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    End If
                End If
            End If



            If CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) = "0" Then
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("FORMA"), String)
            Else
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String)
            End If





            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 25, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_FECV"), String), "SI", "ANTICIPO", "SAT_NHPrePaymentService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "NADA", "NADA", "NADA")
                ' Me.InsertaOracle("AC", 25, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_FECH"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)
            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub TotalPagosaCuentaVisasComisionGolf()
        Try
            Dim Total As Double
            Dim TotalComision As Double

            Dim vCentroCosto As String

            SQL = "SELECT SUM(MOVI_IMPT) TOTAL,CACR_DESC TARJETA,nvl(CACR_CTBA,'0') CUENTA,NVL(TNHT_CACR.CACR_CONT,'0') CUENTAGASTO,TNHT_CACR.CACR_COMI "
            SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_CACO,TNHT_CACR "
            SQL += " WHERE "

            ' SOLO DEBITO 
            SQL = SQL & "  TNPL_MOVI.MOVI_DECR = '0'"


            SQL = SQL & " AND TNPL_MOVI.CACR_CODI = TNPL_CACO.CACR_COD1"
            ' TARJETAS DE CREDITO NEWHOTEL
            SQL = SQL & " AND TNPL_CACO.CACR_COD2 = TNHT_CACR.CACR_CODI"

            ' SOLO VISAS CON COMISION
            SQL += " AND TNHT_CACR.CACR_COMI > 0 "

            '  ES DEPOSITO ANTICIPADO

            SQL = SQL & "  AND TNPL_MOVI.MOVI_DEAN = '1'"

            SQL = SQL & " AND TNPL_MOVI.MOVI_FECH = " & "'" & Me.mFecha & "'"
            SQL = SQL & " GROUP BY CACR_DESC,CACR_CTBA,CACR_CONT,CACR_COMI"


            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read


                SQL = "SELECT NVL(PARA_CENTRO_COSTO_COMI,'0') FROM TH_PARA "
                SQL += " WHERE  PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                vCentroCosto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)



                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                TotalComision = (CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * CType(Me.DbLeeHotel.mDbLector("CACR_COMI"), Double)) / 100
                If Total <> 0 Then
                    Linea = Linea + 1
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 25, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", "", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision)

                    Linea = Linea + 1
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 25, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTAGASTO"), String), Me.mIndicadorDebe, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", vCentroCosto, "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTAGASTO"), String), Me.mIndicadorDebe, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision)
                End If

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
            End If

        End Try

    End Sub
#End Region
#Region "ASIENTO-21 DEVOLUCIONES "
    Private Sub TotalDevolucionesVisas()
        Try
            Dim Total As Double
            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA"
            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 5"
            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' excluir depositos anticipados 
            'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

            If Me.mUsaTnhtMoviAuto = True Then
                SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
            End If
            '
            SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CTBA"


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
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
            End If

        End Try

    End Sub
    Private Sub TotalDevolucionesOtrasFormas()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 5"


        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

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
        SQL = SQL & " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA FROM VNHT_MOVH TNHT_MOVI,"
        SQL = SQL & " TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 5"
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
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 21, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetalleDevolucionesOtrasFormas()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE,TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_DAVA FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 5"

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
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 21, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub TotalDevolucionesOtrasEnFacturaAx()
        Dim Total As Double
        '    Dim TotalCancelados As Double

        Dim SQL As String

        Dim Descripcion As String = ""
        Dim TipoCliente As Integer
        Dim Nif As String
        Dim FormaCobro As String = ""


        Dim Prefijo As String = ""

        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA, "

        SQL += "TNHT_MOVI.MOVI_VDEB TOTAL,NVL(TNHT_RESE.RESE_ANPH,'?') CLIENTE,"
        SQL += " TNHT_FACT.FACT_CODI AS NUMERO ,TNHT_FACT.SEFA_CODI SERIE,"
        SQL = SQL & "TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA,TNHT_FACT.FACT_DAEM,TNHT_MOVI.MOVI_DATR,TNHT_FACT.FAAN_CODI,TNHT_FACT.ENTI_CODI AS ENTIFACT "

        ' N
        SQL = SQL & " ,NVL(TNHT_RESE.RESE_CODI ,0) AS RESE_CODI,NVL(TNHT_RESE.RESE_ANCI ,0) AS RESE_ANCI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_CODI ,'?') AS CCEX_CODI,NVL(TNHT_ENTI.ENTI_CODI ,'?') AS ENTI_CODI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_NUCO ,'?') AS CCEX_NUCO,NVL(TNHT_ENTI.ENTI_NUCO ,'?') AS ENTI_NUCO,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_TITU ,'?') AS CCEX_TITU,NVL(TNHT_ENTI.ENTI_NOME ,'?') AS ENTI_NOME,"
        SQL = SQL & " NVL(TNHT_ENTI.ENTI_FAMA ,'?') AS ENTI_FAMA,"
        SQL = SQL & " NVL(TNHT_CACR.CACR_CTBA,'0') TARJETA,"
        SQL = SQL & " NVL(TNHT_FORE.FORE_CTB1,'0') FORMA"

        ' SQL = SQL & ",NVL(MOVI_NUDO,'?') AS RECIBO"
        SQL = SQL & ",TNHT_MOVI.MOVI_DARE || TNHT_MOVI.MOVI_CODI AS RECIBO "
        SQL = SQL & " ,TIRE_CODI "


        SQL = SQL & ",TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,FACT_HOEM "


        SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO"
        ' N
        SQL = SQL & " ,TNHT_FORE,TNHT_CACR,TNHT_ENTI,TNHT_CCEX "

        '
        SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        'N
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI(+)"
        SQL = SQL & " AND TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+)"
        SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"


        ' NUEVO POR AJUSTE DE RENDIMIENTO 
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) AND "
        SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) AND"


        SQL = SQL & " TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"


        ' SI NO PONGO TIRE = 1 TAMBIEN INCLYUYE DEVOLUCUONES POR CONTABILIDAD 
        ' PROBAR ESTE TEMA SI 1 SE HACE UNA COSA SI 2 SE HACE OTRA ( HABLAR CON DOMINGO ) 
        '  SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '1'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB < 0 "

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '1' "
        End If


        If Me.mTrataDebitoTpvnoFacturado = True Then
            ' EXCLUYE CIERRE DE CONTADO DE TPV
            SQL += "AND TNHT_MOVI.UTIL_CODI <> 'POS'"
            ' PAT 38  SE EXCLUYEN LAS DEVOLUCIONES DE LA FACTURA DE PUNTO DE VENTA 
            SQL += " AND TNHT_MOVI.CCEX_CODI  NOT IN (SELECT CCEX_CODI FROM TNHT_CCEX WHERE CCEX_CODI = 'TPV')"
            ' EXCLUYE CIERRE DE CONTADO DE GOLF
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"
        End If


        SQL += " ORDER BY TNHT_MOVI.MOVI_DAVA "

        Me.DbLeeHotel.TraerLector(SQL)

        Linea = 9000


        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = (CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1)
            End If

            If CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), String) = "0" Then
                ' ES UN NO ALOJADO
                TipoCliente = 2
                Nif = CType(Me.DbLeeHotel.mDbLector("CCEX_NUCO"), String)
                Descripcion = "No Alo : [" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CCEX_TITU"), String)
            Else
                ' NO ES UN NO ALOJADO
                ' ES CLIENTE DIRECTO
                If CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) = "?" Then
                    TipoCliente = 3
                    Nif = Me.mClientesContadoCif
                    Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String)
                Else ' ES CLIENTE DE AGENCIA
                    If CType(Me.DbLeeHotel.mDbLector("ENTI_FAMA"), String) = "1" Then
                        ' TTOO NORMAL
                        TipoCliente = 1
                        Nif = CType(Me.DbLeeHotel.mDbLector("ENTI_NUCO"), String)
                        Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    Else
                        ' TTOO ON LINE ( EL ANTICIPO LE LO CARGAMOS ACLIENTES CONTADO
                        TipoCliente = 3
                        Nif = Me.mClientesContadoCif
                        Descripcion = "[* TTOO WEB ] Rva : " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    End If
                End If
            End If


            If CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) <> "0" Then
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String)
            ElseIf CType(Me.DbLeeHotel.mDbLector("FORMA"), String) <> "0" Then
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("FORMA"), String)
            Else
                '' 
                ' ENVIAR COMO LA FORMA DE COBRO DEL PRIMER ANTICIPO 
                '  FormaCobro = mParaAnticiposAxapta
                FormaCobro = Me.BuscaAnticipoFacturaAnticiposAx(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
            End If

            'PAT0025
            ' averiguacion de prefijo de anticipo 
            ' AQUI HACER LOS IFS DE DEBAJO 

            ' si la devolucion esta en una factura rectifivatica                prtefijo = REC
            ' si la factura es la primera                                       prefijo = ""
            ' si la factura es la segunda,tercera , ... Y NO ES RECTIFICATIVA   prefijo = nrec

            If Me.mTrabajo = True Then
                ' CREAR FUNCION PROPIA PARA DEVOLUCUONES PASAR RESERVA Y BUSCAR SI IMPORTE DE LA DEVOLUCION ESTA EN OTRA FACTURA ANTERIOR
                ' OJO NO PASAR NUMERO DE MOVIMIENTO POR QUE SE CREA NUEVO CADA VEZ QUE SE DEVUELVE
                ' AQUI 222   
                If SaberSiDevolucionFacturadoEstuvoFacturadoAntes(CInt(Me.DbLeeHotel.mDbLector("RESE_CODI")), CInt(Me.DbLeeHotel.mDbLector("RESE_ANCI")), CStr(Me.DbLeeHotel.mDbLector("CCEX_CODI")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CDate(Me.DbLeeHotel.mDbLector("FACT_HOEM")), Total) = False Then
                    ' Facturacion del DEVOLUCION  en la primera factura 
                    Prefijo = ""
                Else
                    '  si la factura NO es la primera en la que se factura el DEVOLUCION y NO es Anulativa
                    If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                        ' Facturacion del Anticipo en las siguientes facturas
                        ' se envia  NREC
                        Prefijo = "NREC"
                    End If
                End If
                ' si la factura es rectificativa
                If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = False Then
                    ' se envia REC negativo
                    Prefijo = "REC"
                End If
            End If



            ' EL COBRO DEL ANTICIPO

            If Total <> 0 Then
                Me.mTipoAsiento = "DEBE"

                If CType(Me.DbLeeHotel.mDbLector("TIRE_CODI"), String) = "1" Then
                    ' DEVOLUCION DE CONTADO
                    Linea = Linea + 1
                    Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(*) Devolución Aut. ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "", Prefijo, "")
                End If
                If CType(Me.DbLeeHotel.mDbLector("TIRE_CODI"), String) = "2" Then
                    ' DEVOLUCION DE CREDITO
                    Linea = Linea + 1
                    Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(*) Devolución Aut. ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "", Prefijo, "")
                End If

            End If



            ' EL ANTICIPO
            If Total <> 0 Then
                Me.mTipoAsiento = "HABER"
                If CType(Me.DbLeeHotel.mDbLector("TIRE_CODI"), String) = "1" Then
                    ' DEVOLUCION DE CONTADO
                    Linea = Linea + 1
                    Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(*) Devolución Aut.  F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "ANTICIPO", "SAT_NHPrePaymentService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "NADA", "NADA", "NADA", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), "", "DEVOLUCION", CType(Me.DbLeeHotel.mDbLector("MOVI_CODI"), Integer), Prefijo)
                End If
                If CType(Me.DbLeeHotel.mDbLector("TIRE_CODI"), String) = "2" Then
                    ' DEVOLUCION DE CREDITO
                    Linea = Linea + 1
                    Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(*) Devolución Aut.  F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "ANTICIPO", "SAT_NHPrePaymentService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "NADA", "NADA", "NADA", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), "", "DEVOLUCION", CType(Me.DbLeeHotel.mDbLector("MOVI_CODI"), Integer), Prefijo)
                    Linea = Linea + 1
                    Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "Resto " & Descripcion.Replace("'", "''"), Total * -1, "NO", Nif, "(*) Devolución Aut.  F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "ANTICIPO", "SAT_NHPrePaymentService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "NADA", "NADA", "NADA", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), "RESTO", "DEVOLUCION", CType(Me.DbLeeHotel.mDbLector("MOVI_CODI"), Integer), Prefijo)

                End If
            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub TotalDevolucionesOtrasManualesAx()
        Dim Total As Double
        '    Dim TotalCancelados As Double

        Dim SQL As String

        Dim Descripcion As String = ""
        Dim TipoCliente As Integer
        Dim Nif As String
        Dim FormaCobro As String = ""



        SQL = "SELECT TNHT_MOVI.TIRE_CODI AS TIRE_CODI, TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
        SQL = SQL & " TNHT_CACR.CACR_DESC TARJETANOMBRE,MOVI_DAVA, "

        SQL = SQL & " NVL(TNHT_RESE.RESE_CODI ,0) AS RESE_CODI,NVL(TNHT_RESE.RESE_ANCI ,0) AS RESE_ANCI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_CODI ,'?') AS CCEX_CODI,NVL(TNHT_ENTI.ENTI_CODI ,'?') AS ENTI_CODI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_NUCO ,'?') AS CCEX_NUCO,NVL(TNHT_ENTI.ENTI_NUCO ,'?') AS ENTI_NUCO,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_TITU ,'?') AS CCEX_TITU,NVL(TNHT_ENTI.ENTI_NOME ,'?') AS ENTI_NOME,"
        SQL = SQL & " NVL(TNHT_ENTI.ENTI_FAMA ,'?') AS ENTI_FAMA,"
        SQL = SQL & " NVL(TNHT_CACR.CACR_CTBA,'0') TARJETA,"
        SQL = SQL & " NVL(TNHT_FORE.FORE_CTB1,'0') FORMA,"
        SQL = SQL & " FNHT_ESMOTO(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE) AS TRANSFERIDO"
        SQL = SQL & ",NVL(MOVI_NUDO,'?') AS RECIBO "
        SQL = SQL & ",TNHT_MOVI.MOVI_DARE || TNHT_MOVI.MOVI_CODI AS RECIBO "

        SQL = SQL & ",TNHT_MOVI.MOVI_CODI AS MOVI_CODI"


        SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,"
        SQL = SQL & " TNHT_CACR,TNHT_RESE,TNHT_CCEX,TNHT_ENTI,TNHT_FORE "
        SQL = SQL & " WHERE "
        SQL = SQL & "     TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI(+)"
        SQL = SQL & " AND TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+)"

        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"


        SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"


        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 5"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"


        If Me.mTrataDebitoTpvnoFacturado = True Then
            ' EXCLUYE CIERRE DE CONTADO DE TPV
            SQL += " AND TNHT_MOVI.UTIL_CODI <> 'POS'"
            SQL += "AND (TNHT_MOVI.UTIL_CODI <> 'GMS' OR TNHT_MOVI.UTIL_CODI =  'GMS'  AND FNHT_ESMOTO(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE) > 0 )"
        End If

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        SQL += " ORDER BY TNHT_MOVI.MOVI_DAVA "

        Me.DbLeeHotel.TraerLector(SQL)

        Linea = 9500


        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            '   If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            'Else
            'Total = (CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1)
            'End If

            If CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), String) = "0" Then
                ' ES UN NO ALOJADO
                TipoCliente = 2
                Nif = CType(Me.DbLeeHotel.mDbLector("CCEX_NUCO"), String)
                Descripcion = "No Alo : [" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CCEX_TITU"), String)
            Else
                ' NO ES UN NO ALOJADO
                ' ES CLIENTE DIRECTO
                If CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) = "?" Then
                    TipoCliente = 3
                    Nif = Me.mClientesContadoCif
                    Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String)
                Else ' ES CLIENTE DE AGENCIA
                    If CType(Me.DbLeeHotel.mDbLector("ENTI_FAMA"), String) = "1" Then
                        ' TTOO NORMAL
                        TipoCliente = 1
                        Nif = CType(Me.DbLeeHotel.mDbLector("ENTI_NUCO"), String)
                        Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    Else
                        ' TTOO ON LINE ( EL ANTICIPO LE LO CARGAMOS ACLIENTES CONTADO
                        TipoCliente = 3
                        Nif = Me.mClientesContadoCif
                        Descripcion = "[* TTOO WEB ] Rva : " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    End If
                End If
            End If


            If CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) <> "0" Then
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String)
            ElseIf CType(Me.DbLeeHotel.mDbLector("FORMA"), String) <> "0" Then
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("FORMA"), String)
            Else
                '' 
                ' ENVIAR COMO LA FORMA DE COBRO DEL PRIMER ANTICIPO 
                '  FormaCobro = mParaAnticiposAxapta
                FormaCobro = Me.BuscaAnticipoFacturaAnticiposAx(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
            End If

            '   MsgBox("LAS DEVOLUCIONES POR CONTABILIDAD SE HACEN EN LA MISMA FORMA DE COBRO EN LA QUE SE RECIBO EL  PRIMER ANTICIPO " & vbCrLf & " OJO !!!! FALTA REVISAR BIEN Y MISMO DESARROLLO facturas  DE GOLF" & vbCrLf & "  Se   Puede Continuar .......", MsgBoxStyle.Information)

            ' EL COBRO DEL ANTICIPO

            If Total <> 0 Then
                Me.mTipoAsiento = "DEBE"
                ' DEVOLUCION DE CONTADO
                '        Linea = Linea + 1
                '       Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "Devolución Manual ", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "")
            End If


            ' EL ANTICIPO
            If Total <> 0 Then
                Me.mTipoAsiento = "HABER"
                ' DEVOLUCION DE CONTADO
                Linea = Linea + 1
                Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(*)Devolución Manual    F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "ANTICIPO", "SAT_NHPrePaymentService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "NADA", "NADA", "NADA", "", "", "", "DEVOLUCION", CType(Me.DbLeeHotel.mDbLector("MOVI_CODI"), Integer))
            End If



        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
#End Region
#Region "ASIENTO-22 DEVOLUCIONES EN FACTURA "
    Private Sub TotalDevolucionesVisasFacturado()
        Try
            Dim Total As Double
            Dim Factura As String
            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA,MOVI_CORR,MOVI_ANUL,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI"
            SQL += " FROM VNHT_MOVH TNHT_MOVI,TNHT_CACR,TNHT_RESE,TNHT_FAMO,TNHT_FACT WHERE "

            SQL += "     TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
            SQL += " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
            SQL += " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            SQL += " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI AND "

            SQL += " TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL += " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
            SQL += " AND (TNHT_MOVI.TIRE_CODI = 1 AND TNHT_MOVI.MOVI_AUTO = 1  AND TNHT_MOVI.MOVI_VDEB < 0 ) "
            SQL += " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"




            SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CTBA,MOVI_CORR,MOVI_ANUL,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI"


            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1

                If CType(Me.DbLeeHotel.mDbLector("MOVI_CORR"), Integer) = 1 Or CType(Me.DbLeeHotel.mDbLector("MOVI_ANUL"), Integer) = 1 Then
                    Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
                Else
                    Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                End If

                Factura = CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String)

                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & Factura, Total, "NO", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & Factura, Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
            End If

        End Try

    End Sub
    Private Sub TotalDevolucionesOtrasFormasFacturado()
        Dim Total As Double
        Dim Factura As String
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_CORR,MOVI_ANUL,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_RESE,TNHT_FAMO,TNHT_FACT WHERE"
        SQL += "     TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL += " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL += " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL += " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI AND "


        SQL += " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL += " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL += " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
        SQL += " AND (TNHT_MOVI.TIRE_CODI = 1 AND TNHT_MOVI.MOVI_AUTO = 1  AND TNHT_MOVI.MOVI_VDEB < 0 ) "

        SQL += " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL += " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"



        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1,MOVI_CORR,MOVI_ANUL,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI"
        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            If CType(Me.DbLeeHotel.mDbLector("MOVI_CORR"), Integer) = 1 Or CType(Me.DbLeeHotel.mDbLector("MOVI_ANUL"), Integer) = 1 Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            End If

            Factura = CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String)

            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " " & Factura, Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " " & Factura, Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetalleDevolucionesVisasFacturado()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(TNHT_RESE.RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
        SQL += " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA,MOVI_CORR,MOVI_ANUL FROM VNHT_MOVH TNHT_MOVI,"
        SQL += " TNHT_CACR,TNHT_RESE,TNHT_FAMO,TNHT_FACT WHERE "

        SQL += "     TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL += " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL += " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL += " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI AND "

        SQL += " TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL += " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL += " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
        SQL += " AND (TNHT_MOVI.TIRE_CODI = 1 AND TNHT_MOVI.MOVI_AUTO = 1  AND TNHT_MOVI.MOVI_VDEB < 0 ) "
        SQL += " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"




        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1

            If CType(Me.DbLeeHotel.mDbLector("MOVI_CORR"), Integer) = 1 Or CType(Me.DbLeeHotel.mDbLector("MOVI_ANUL"), Integer) = 1 Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            End If

            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetalleDevolucionesOtrasFormasFacturado()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(TNHT_RESE.RESE_ANPH,'?') CLIENTE,TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_DAVA,MOVI_CORR,MOVI_ANUL FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_RESE,TNHT_FAMO,TNHT_FACT WHERE"
        SQL += "     TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL += " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL += " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL += " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI AND "

        SQL += " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL += " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL += " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
        SQL += " AND (TNHT_MOVI.TIRE_CODI = 1 AND TNHT_MOVI.MOVI_AUTO = 1  AND TNHT_MOVI.MOVI_VDEB < 0 ) "

        SQL += " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL += " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"




        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1

            If CType(Me.DbLeeHotel.mDbLector("MOVI_CORR"), Integer) = 1 Or CType(Me.DbLeeHotel.mDbLector("MOVI_ANUL"), Integer) = 1 Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            End If


            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
#End Region

#Region "ASIENTO-99 ANTICIPOS RECIBIDOS CUENTAS NO ALOJADO"
    Private Sub TotalPagosaCuentaVisasNoAlojados()
        Try
            Dim Total As Double
            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA"
            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_CACR WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI  IS NOT NULL"
            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' excluir depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

            If Me.mUsaTnhtMoviAuto = True Then
                SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
            End If
            '
            SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CTBA"


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
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
            End If

        End Try

    End Sub
    Private Sub TotalPagosaCuentaOtrasFormasNoAlojados()
        Dim Total As Double
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE WHERE"
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
        SQL = SQL & " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA FROM VNHT_MOVH TNHT_MOVI,"
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
        SQL = "SELECT TNHT_MOVI.CCEX_CODI CCEX,NVL(CCEX_TITU,'?') CLIENTE,TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_DAVA FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_CCEX WHERE"
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
#Region "ASIENTO-31 NOTAS DE CREDITO NEWGOLF"


    Private Sub NotasCreditoGolfTotalLiquido()
        Dim Total As Double
        Dim SQL As String

        Try

            SQL = "SELECT SUM(NVL(MOVI_VLIQ,0)) TOTAL , NCRE_DAEM "
            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_NCRE," & Me.mParaUsuarioNewGolf & ".TNPL_MCRE, " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI "
            SQL += "WHERE TNPL_NCRE.SENC_CODI = TNPL_MCRE.SENC_CODI "
            SQL += "AND TNPL_NCRE.NCRE_CODI = TNPL_MCRE.NCRE_CODI "
            SQL += "AND TNPL_MCRE.MOVI_CODI = TNPL_MOVI.MOVI_CODI "
            SQL += "AND TNPL_MCRE.MOVI_DATE = TNPL_MOVI.MOVI_DATE "

            SQL += "AND TNPL_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
            SQL += "GROUP BY TNPL_NCRE.NCRE_DAEM"




            If IsDBNull(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = False Then
                Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            Else
                Total = 0
            End If


            Total = Decimal.Round(CType(Total, Decimal), 2)



            If Total <> 0 Then
                Linea = 1
                Me.mTipoAsiento = "HABER"
                Me.mTotalFacturacion = Total

                Me.InsertaOracle("AC", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "NOTAS DE CREDITO " & Me.mFecha, Total, "SI", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, "NOTAS DE CREDITO  " & Me.mFecha, Total)

            End If

            ' NOTAS DE CREDITO ANULADAS 

            SQL = "SELECT ABS(SUM(VLIQ)) TOTAL ,NCRE_DAAN "
            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".VNPL_NCIV," & Me.mParaUsuarioNewGolf & ".TNPL_NCRE "
            SQL += "WHERE VNPL_NCIV.SENC_CODI = TNPL_NCRE.SENC_CODI "
            SQL += "AND VNPL_NCIV.NCRE_CODI = TNPL_NCRE.NCRE_CODI "
            SQL += "AND TNPL_NCRE.NCRE_DAAN = " & "'" & Me.mFecha & "' "
            SQL += "GROUP BY TNPL_NCRE.NCRE_DAAN"



            If IsDBNull(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = False Then
                Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            Else
                Total = 0
            End If


            Total = Decimal.Round(CType(Total, Decimal), 2)



            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.mTotalFacturacion = Total

                Me.InsertaOracle("AC", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "NOTAS DE CREDITO ANULADAS " & Me.mFecha, Total, "SI", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, "NOTAS DE CREDITO  ANULADAS " & Me.mFecha, Total)

            End If



        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Total LIQUIDO NOTAS DE CREDITO")
            End If

        End Try

    End Sub
    Private Sub NotasCreditoGolfTotalLiquidoTRABAJANDO()
        Dim Total As Double
        Dim SQL As String

        Try

            '    SQL = "SELECT SUM(MOVI_VLIQ) TOTAL , DPTO_CODI,NCRE_DAEM "
            'SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_NCRE," & Me.mParaUsuarioNewGolf & ".TNPL_MCRE, " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI "
            'SQL += "WHERE TNPL_NCRE.SENC_CODI = TNPL_MCRE.SENC_CODI "
            'SQL += "AND TNPL_NCRE.NCRE_CODI = TNPL_MCRE.NCRE_CODI "
            'SQL += "AND TNPL_MCRE.MOVI_CODI = TNPL_MOVI.MOVI_CODI "
            'SQL += "AND TNPL_MCRE.MOVI_DATE = TNPL_MOVI.MOVI_DATE "

            'SQL += "AND TNPL_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
            'SQL += "GROUP BY TNPL_NCRE.NCRE_DAEM,DPTO_CODI"


            ' NUEVO TRATAMIENTO DESPRODUCIR 
            SQL = "SELECT TNHT_MOVI.SECC_CODI, TNHT_MOVI.SERV_CODI SERVICIO, TNHT_SERV.SERV_DESC DEPARTAMENTO,"
            SQL += "NVL(TNHT_SERV.SERV_CTB1,   '0') CUENTA, NVL(BLAL_DESC,   'OTROS INGRESOS') AS BLOQUE,"
            SQL += "TNHT_BLAL.BLAL_CODI, ROUND(SUM(TNHT_MOVI.MOVI_VLIQ),   2) TOTAL "

            SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_NCRE,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_MCRE,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_MOVI,"
            SQL += "VNHT_MOVH TNHT_MOVI,"
            SQL += "TNHT_SERV,"
            SQL += "TNHT_ALOJ,"
            SQL += "TNHT_BLAL"

            SQL += " WHERE TNPL_NCRE.SENC_CODI = TNPL_MCRE.SENC_CODI"
            SQL += " AND TNPL_NCRE.NCRE_CODI = TNPL_MCRE.NCRE_CODI"

            SQL += " AND TNPL_MCRE.MOVI_CODI = TNPL_MOVI.MOVI_CODI"
            SQL += " AND TNPL_MCRE.MOVI_DATE = TNPL_MOVI.MOVI_DATE"

            SQL += " AND TNPL_MOVI.NEWH_CODI =  TNHT_MOVI.MOVI_CODI "
            SQL += " AND TNPL_MOVI.NEWH_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += " AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI"
            SQL += " AND TNHT_MOVI.ALOJ_CODI = TNHT_ALOJ.ALOJ_CODI(+)"
            SQL += " AND TNHT_ALOJ.BLAL_CODI = TNHT_BLAL.BLAL_CODI(+)"

            SQL += " AND TNPL_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
            SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,TNHT_SERV.SERV_COMS,BLAL_DESC,TNHT_BLAL.BLAL_CODI"

            Me.DbLeeHotel.TraerLector(SQL)

            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read

                Total = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

                If Total <> 0 Then
                    Linea = Linea + 1
                    Me.mTipoAsiento = "DEBE"
                    Me.mTotalFacturacion = Total

                    Me.InsertaOracle("AC", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorDebe, "NOTAS DE CREDITO " & Me.mFecha, Total, "SI", "", "", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorDebe, "NOTAS DE CREDITO  " & Me.mFecha, Total)

                End If
            End While
            Me.DbLeeHotel.mDbLector.Close()




            ' NOTAS DE CREDITO ANULADAS 

            SQL = "SELECT TNHT_MOVI.SECC_CODI, TNHT_MOVI.SERV_CODI SERVICIO, TNHT_SERV.SERV_DESC DEPARTAMENTO,"
            SQL += "NVL(TNHT_SERV.SERV_CTB1,   '0') CUENTA, NVL(BLAL_DESC,   'OTROS INGRESOS') AS BLOQUE,"
            SQL += "TNHT_BLAL.BLAL_CODI, ROUND(SUM(TNHT_MOVI.MOVI_VLIQ),   2) TOTAL "

            SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_NCRE,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_MCRE,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_MOVI,"
            SQL += "VNHT_MOVH TNHT_MOVI,"
            SQL += "TNHT_SERV,"
            SQL += "TNHT_ALOJ,"
            SQL += "TNHT_BLAL"

            SQL += " WHERE TNPL_NCRE.SENC_CODI = TNPL_MCRE.SENC_CODI"
            SQL += " AND TNPL_NCRE.NCRE_CODI = TNPL_MCRE.NCRE_CODI"

            SQL += " AND TNPL_MCRE.MOVI_CODI = TNPL_MOVI.MOVI_CODI"
            SQL += " AND TNPL_MCRE.MOVI_DATE = TNPL_MOVI.MOVI_DATE"

            SQL += " AND TNPL_MOVI.NEWH_CODI =  TNHT_MOVI.MOVI_CODI "
            SQL += " AND TNPL_MOVI.NEWH_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += " AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI"
            SQL += " AND TNHT_MOVI.ALOJ_CODI = TNHT_ALOJ.ALOJ_CODI(+)"
            SQL += " AND TNHT_ALOJ.BLAL_CODI = TNHT_BLAL.BLAL_CODI(+)"

            SQL += " AND TNPL_NCRE.NCRE_DAAN = " & "'" & Me.mFecha & "' "

            SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,TNHT_SERV.SERV_COMS,BLAL_DESC,TNHT_BLAL.BLAL_CODI"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                Total = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

                If Total <> 0 Then
                    Linea = Linea + 1
                    Me.mTipoAsiento = "HABER"
                    Me.mTotalFacturacion = Total

                    Me.InsertaOracle("AC", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "NOTAS DE CREDITO ANULADAS " & Me.mFecha, Total, "SI", "", "", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, "NOTAS DE CREDITO  ANULADAS " & Me.mFecha, Total)

                End If
            End While
            Me.DbLeeHotel.mDbLector.Close()



        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Total LIQUIDO NOTAS DE CREDITO")
            End If

        End Try

    End Sub


    Private Sub NotasCreditoGolfTotalNota()
        Try
            Dim TotalFactura As Double
            Dim Dni As String
            Dim Cuenta As String = "0"
            Dim Titular As String
            Dim TextoAuxiliar As String
            Dim SQL As String

            SQL = "SELECT  TNPL_NCRE.NCRE_NECO AS ESTADO, TNPL_NCRE.NCRE_DAEM, TNPL_NCRE.NCRE_CODI AS NUMERO, NVL(TNPL_NCRE.SENC_CODI,'?')  SERIE, "
            SQL += "  TNPL_NCRE.NCRE_CODI ||'/'|| TNPL_NCRE.SENC_CODI DESCRIPCION,TNPL_NCRE.NCRE_VALO TOTAL,NVL(TNPL_FACT.ENTI_CODI,'') AS ENTI_CODI, "
            SQL += " NVL(TNPL_NCRE.NCRE_TITU,'') TITULAR ,TNPL_FACT.FACT_CODI AS FACTURA "
            SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_NCRE, " & Me.mParaUsuarioNewGolf & ".TNPL_NCFA, " & Me.mParaUsuarioNewGolf & ".TNPL_FACT "
            SQL += "WHERE "

            SQL += "     TNPL_NCRE.SENC_CODI = TNPL_NCFA.SENC_CODI(+) "
            SQL += "AND  TNPL_NCRE.NCRE_CODI = TNPL_NCFA.NCRE_CODI(+) "
            SQL += "AND  TNPL_NCFA.FACT_CODI = TNPL_FACT.FACT_CODI(+)"
            SQL += "AND  TNPL_NCFA.SEFA_CODI =  TNPL_FACT.SEFA_CODI(+) "

            SQL += "AND TNPL_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
            SQL += "ORDER BY TNPL_NCRE.SENC_CODI ASC, TNPL_NCRE.NCRE_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)


            Me.NEWGOLF = New NewGolf.NewGolfData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)

            While Me.DbLeeHotel.mDbLector.Read


                Linea = Linea + 1
                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

                If IsDBNull(Me.DbLeeHotel.mDbLector("FACTURA")) = False Then
                    TextoAuxiliar = "Factura Nº= " & Me.DbLeeHotel.mDbLector("FACTURA")
                Else
                    TextoAuxiliar = "Sin Factura "
                End If


                ' DETERMINAR EL TIPO DE FACTURA 
                ' NOTA DE CREDIT0 DE CONTADO 


                Cuenta = Me.NEWGOLF.DevuelveCuentaContabledeNotaCredito(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                Dni = Me.NEWGOLF.DevuelveDniCifContabledeNotaCredito(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))



                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)



                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("FV", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Dni, Titular, "SI", "NOTA DE CREDITO")
                Me.GeneraFileFV2("FV", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalFactura, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String).PadRight(15, CChar(" ")), Cuenta, Dni)

            End While
            Me.DbLeeHotel.mDbLector.Close()


            ' ANULADAS 

            SQL = "SELECT  TNPL_NCRE.NCRE_NECO AS ESTADO, TNPL_NCRE.NCRE_DAEM,TNPL_NCRE.NCRE_DAAN, TNPL_NCRE.NCRE_CODI AS NUMERO, NVL(TNPL_NCRE.SENC_CODI,'?')  SERIE, "
            SQL += "  TNPL_NCRE.NCRE_CODI ||'/'|| TNPL_NCRE.SENC_CODI DESCRIPCION,TNPL_NCRE.NCRE_VALO TOTAL,NVL(TNPL_FACT.ENTI_CODI,'') AS ENTI_CODI, "
            SQL += " NVL(TNPL_NCRE.NCRE_TITU,'') TITULAR ,TNPL_FACT.FACT_CODI AS FACTURA "
            SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_NCRE, " & Me.mParaUsuarioNewGolf & ".TNPL_NCFA, " & Me.mParaUsuarioNewGolf & ".TNPL_FACT "
            SQL += "WHERE "

            SQL += "     TNPL_NCRE.SENC_CODI = TNPL_NCFA.SENC_CODI(+) "
            SQL += "AND  TNPL_NCRE.NCRE_CODI = TNPL_NCFA.NCRE_CODI(+) "
            SQL += "AND  TNPL_NCFA.FACT_CODI = TNPL_FACT.FACT_CODI(+)"
            SQL += "AND  TNPL_NCFA.SEFA_CODI =  TNPL_FACT.SEFA_CODI(+) "

            SQL += "AND TNPL_NCRE.NCRE_DAAN = " & "'" & Me.mFecha & "' "
            SQL += "ORDER BY TNPL_NCRE.SENC_CODI ASC, TNPL_NCRE.NCRE_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read


                Linea = Linea + 1
                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

                If IsDBNull(Me.DbLeeHotel.mDbLector("FACTURA")) = False Then
                    TextoAuxiliar = "Factura Nº= " & Me.DbLeeHotel.mDbLector("FACTURA")
                Else
                    TextoAuxiliar = "Sin Factura "
                End If


                ' DETERMINAR EL TIPO DE FACTURA 
                ' NOTA DE CREDIT0 DE CONTADO 

                Cuenta = Me.NEWGOLF.DevuelveCuentaContabledeNotaCredito(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                Dni = Me.NEWGOLF.DevuelveDniCifContabledeNotaCredito(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))




                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)



                Me.mTipoAsiento = "DEBE"

                If DbLeeHotel.mDbLector("NCRE_DAAN") > DbLeeHotel.mDbLector("NCRE_DAEM") Then
                    Me.InsertaOracle("FV", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Dni, Titular, "SI", "NOTA DE CREDITO ANULADA")
                Else
                    Me.InsertaOracle("FV", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Dni, Titular, "SI", "NOTA DE CREDITO ANULADA MISMO DIA")
                End If

                Me.GeneraFileFV2("FV", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalFactura, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String).PadRight(15, CChar(" ")), Cuenta, Dni)

            End While
            Me.DbLeeHotel.mDbLector.Close()
            Me.NEWGOLF.CerrarConexiones()


        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas")
            End If

        End Try
    End Sub
    Private Sub NotasCreditoGolfTotalNotaAx()
        Try
            Dim TotalFactura As Double
            Dim TotalLinea As Double
            Dim Dni As String
            Dim Cuenta As String = "0"
            Dim Titular As String
            Dim TextoAuxiliar As String
            Dim SQL As String
            Dim TipoNota As Integer
            Dim CodigoCliente As String = ""

            Dim CodigoArticuloAx As String = ""

            Dim CodigoCompuesto As String

            Dim TipoVenta As String


            SQL = "SELECT   TNPL_NCRE.NCRE_NECO AS ESTADO, TNPL_NCRE.NCRE_DAEM, "
            SQL += "         TNPL_NCRE.NCRE_CODI AS NUMERO, NVL (TNPL_NCRE.SENC_CODI, '?') SERIE, "
            SQL += "         TNPL_NCRE.NCRE_CODI || '/' || TNPL_NCRE.SENC_CODI DESCRIPCION, "
            SQL += "         TNPL_NCRE.NCRE_VALO * -1 AS TOTAL, NVL (TNPL_FACT.ENTI_CODI, "
            SQL += "                                         '') AS ENTI_CODI, "
            SQL += "         NVL (TNPL_NCRE.NCRE_TITU, '') TITULAR, "
            SQL += "         TNPL_FACT.FACT_CODI AS FACTURA, MOVI_IMPT AS LINBRUT, "
            SQL += "         MOVI_VLIQ  AS LINBASE  ,TNPL_MOVI.MOVI_IMP1  AS LINIMPU,NVL(TNPL_TIVA.TIVA_PERC,'0') AS LINPORCEIGIC, "
            SQL += "         DECODE (TNPL_ADIC.ADIC_CODI, "
            SQL += "                 NULL, NVL (TNPL_RECO.SERV_CODI, '?'), "
            SQL += "                 TNPL_ADIC.ADIC_CODI "
            SQL += "                ) AS SERVICIO, "
            SQL += "         DECODE (TNPL_MOVI.RECU_CODI, "
            SQL += "                 NULL, DECODE (TNPL_ADIC.ADIC_CODI, "
            SQL += "                               NULL, TNPL_SSPA.SSPA_DESC, "
            SQL += "                               TNPL_ADIC.ADIC_DESC "
            SQL += "                              ), "
            SQL += "                 TNPL_RECU.RECU_DESC "
            SQL += "                ) AS CONCEPTO "


            ' NUEVA LINEA ABAJO FALTA DESARROLLO EN TODAS LAS LINEAS DE FACTURA 

            SQL += " ,DECODE(ADIC_TIPO,'3','BONOS','NORMAL') AS TIPOVENTA"

            SQL += ",NVL(TNPL_ADCO.SERV_CODI,'?') AS SERVICIODELARTICULO "

            SQL += " ,NVL(ADIC_CTS1,'?') AS CODIGOARTICULOAX "

            SQL += "    FROM  " & Me.mParaUsuarioNewGolf & ".TNPL_NCRE, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_NCFA, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_FACT, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_MCRE, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_TIVA, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_ADIC, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_RECU, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_RECO, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_SSPA, "

            SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADCO "

            SQL += "   WHERE TNPL_NCRE.SENC_CODI = TNPL_NCFA.SENC_CODI(+) "
            SQL += "     AND TNPL_NCRE.NCRE_CODI = TNPL_NCFA.NCRE_CODI(+) "
            SQL += "     AND TNPL_NCFA.FACT_CODI = TNPL_FACT.FACT_CODI(+) "
            SQL += "     AND TNPL_NCFA.SEFA_CODI = TNPL_FACT.SEFA_CODI(+) "
            SQL += "     AND TNPL_NCRE.SENC_CODI = TNPL_MCRE.SENC_CODI "
            SQL += "     AND TNPL_NCRE.NCRE_CODI = TNPL_MCRE.NCRE_CODI "
            SQL += "     AND TNPL_MCRE.MOVI_CODI = TNPL_MOVI.MOVI_CODI "
            SQL += "     AND TNPL_MCRE.MOVI_DATE = TNPL_MOVI.MOVI_DATE "
            SQL += "     AND TNPL_MOVI.RECU_CODI = TNPL_RECU.RECU_CODI(+) "
            SQL += "     AND TNPL_RECU.RECU_CODI = TNPL_RECO.RECU_CODI(+) "
            SQL += "     AND TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI(+) "
            SQL += "     AND TNPL_MOVI.SSPA_CODI = TNPL_SSPA.SSPA_CODI(+) "
            SQL += "     AND TNPL_MOVI.MOVI_IVA1 = TNPL_TIVA.TIVA_CODI(+)"

            SQL += " AND TNPL_MOVI.ADIC_CODI = TNPL_ADCO.ADIC_CODI(+)"

            ' SOLO MOVIMIENTOS DE CREDITO 
            SQL += " AND MOVI_DECR = 1 "
            SQL += "     AND TNPL_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
            SQL += "ORDER BY TNPL_NCRE.SENC_CODI ASC, TNPL_NCRE.NCRE_CODI ASC "


            Me.DbLeeHotel.TraerLector(SQL)


            Me.NEWGOLF = New NewGolf.NewGolfData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)

            While Me.DbLeeHotel.mDbLector.Read


                Linea = Linea + 1
                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)
                TotalLinea = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Decimal), 2)


                If IsDBNull(Me.DbLeeHotel.mDbLector("FACTURA")) = False Then
                    TextoAuxiliar = "Factura Nº= " & Me.DbLeeHotel.mDbLector("FACTURA")
                Else
                    TextoAuxiliar = "Sin Factura "
                End If


                ' DETERMINAR EL TIPO DE FACTURA 
                ' NOTA DE CREDIT0 DE CONTADO 


                Cuenta = Me.NEWGOLF.DevuelveCuentaContabledeNotaCredito(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                Dni = Me.NEWGOLF.DevuelveDniCifContabledeNotaCredito(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                TipoNota = Me.NEWGOLF.DevuelveTipoNotaCredito(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                CodigoCliente = Me.NEWGOLF.DevuelveCodigoEntiNotaCredito(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))



                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)



                If CType(Me.DbLeeHotel.mDbLector("SERVICIODELARTICULO"), String) = Me.mParaServCodiBonosAsoc Then
                    TipoVenta = "BONOS ASOC"
                Else
                    TipoVenta = CType(Me.DbLeeHotel.mDbLector("TIPOVENTA"), String)
                End If


                If Me.mParaIngresoPorHabitacion = 1 Then
                    If CType(Me.DbLeeHotel.mDbLector("SERVICIODELARTICULO"), String) = "?" Then
                        CodigoCompuesto = "000-" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String)
                    Else
                        CodigoCompuesto = "000-" & CType(Me.DbLeeHotel.mDbLector("SERVICIODELARTICULO"), String)
                    End If
                Else
                    CodigoCompuesto = Me.DbLeeHotel.mDbLector("SERVICIO")
                End If


                CodigoArticuloAx = CType(Me.DbLeeHotel.mDbLector("CODIGOARTICULOAX"), String)

                Me.mTipoAsiento = "HABER"

                Me.InsertaOracle("FV", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalLinea * -1, "NO", Dni, CType(Me.DbLeeHotel.mDbLector("CONCEPTO"), String), "SI", "NOTA DE CREDITO", "SAT_NHCreateSalesOrdersQueryService", CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Double), CType(Me.DbLeeHotel.mDbLector("LINBASE"), Double), CType(Me.DbLeeHotel.mDbLector("LINIMPU"), Double), CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String), CType(Me.DbLeeHotel.mDbLector("LINPORCEIGIC"), Double), TipoNota, CodigoCliente, TipoVenta, CodigoCompuesto, "NADA")
                Me.GeneraFileFV2("FV", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalFactura, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String).PadRight(15, CChar(" ")), Cuenta, Dni)

            End While
            Me.DbLeeHotel.mDbLector.Close()


            ' ANULADAS 

            SQL = "SELECT   TNPL_NCRE.NCRE_NECO AS ESTADO, TNPL_NCRE.NCRE_DAEM, TNPL_NCRE.NCRE_DAAN,"
            SQL += "         TNPL_NCRE.NCRE_CODI AS NUMERO, NVL (TNPL_NCRE.SENC_CODI, '?') SERIE, "
            SQL += "         TNPL_NCRE.NCRE_CODI || '/' || TNPL_NCRE.SENC_CODI DESCRIPCION, "
            SQL += "         TNPL_NCRE.NCRE_VALO  AS TOTAL, NVL (TNPL_FACT.ENTI_CODI, "
            SQL += "                                         '') AS ENTI_CODI, "
            SQL += "         NVL (TNPL_NCRE.NCRE_TITU, '') TITULAR, "
            SQL += "         TNPL_FACT.FACT_CODI AS FACTURA, MOVI_IMPT * -1 AS LINBRUT, "
            SQL += "         MOVI_VLIQ * -1 AS LINBASE,TNPL_MOVI.MOVI_IMP1 * -1 AS LINIMPU,NVL(TNPL_TIVA.TIVA_PERC,'0') AS LINPORCEIGIC, "
            SQL += "         DECODE (TNPL_ADIC.ADIC_CODI, "
            SQL += "                 NULL, NVL (TNPL_RECO.SERV_CODI, '?'), "
            SQL += "                 TNPL_ADIC.ADIC_CODI "
            SQL += "                ) AS SERVICIO, "
            SQL += "         DECODE (TNPL_MOVI.RECU_CODI, "
            SQL += "                 NULL, DECODE (TNPL_ADIC.ADIC_CODI, "
            SQL += "                               NULL, TNPL_SSPA.SSPA_DESC, "
            SQL += "                               TNPL_ADIC.ADIC_DESC "
            SQL += "                              ), "
            SQL += "                 TNPL_RECU.RECU_DESC "
            SQL += "                ) AS CONCEPTO "

            ' NUEVA LINEA ABAJO FALTA DESARROLLO EN TODAS LAS LINEAS DE FACTURA 

            SQL += " ,DECODE(ADIC_TIPO,'3','BONOS','NORMAL') AS TIPOVENTA"

            SQL += ",NVL(TNPL_ADCO.SERV_CODI,'?') AS SERVICIODELARTICULO "

            SQL += " ,NVL(ADIC_CTS1,'?') AS CODIGOARTICULOAX "

            SQL += "    FROM  " & Me.mParaUsuarioNewGolf & ".TNPL_NCRE, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_NCFA, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_FACT, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_MCRE, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_TIVA, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_ADIC, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_RECU, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_RECO, "
            SQL += "          " & Me.mParaUsuarioNewGolf & ".TNPL_SSPA ,"

            SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADCO "

            SQL += "   WHERE TNPL_NCRE.SENC_CODI = TNPL_NCFA.SENC_CODI(+) "
            SQL += "     AND TNPL_NCRE.NCRE_CODI = TNPL_NCFA.NCRE_CODI(+) "
            SQL += "     AND TNPL_NCFA.FACT_CODI = TNPL_FACT.FACT_CODI(+) "
            SQL += "     AND TNPL_NCFA.SEFA_CODI = TNPL_FACT.SEFA_CODI(+) "
            SQL += "     AND TNPL_NCRE.SENC_CODI = TNPL_MCRE.SENC_CODI "
            SQL += "     AND TNPL_NCRE.NCRE_CODI = TNPL_MCRE.NCRE_CODI "
            SQL += "     AND TNPL_MCRE.MOVI_CODI = TNPL_MOVI.MOVI_CODI "
            SQL += "     AND TNPL_MCRE.MOVI_DATE = TNPL_MOVI.MOVI_DATE "
            SQL += "     AND TNPL_MOVI.RECU_CODI = TNPL_RECU.RECU_CODI(+) "
            SQL += "     AND TNPL_RECU.RECU_CODI = TNPL_RECO.RECU_CODI(+) "
            SQL += "     AND TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI(+) "
            SQL += "     AND TNPL_MOVI.SSPA_CODI = TNPL_SSPA.SSPA_CODI(+) "
            SQL += "     AND TNPL_MOVI.MOVI_IVA1 = TNPL_TIVA.TIVA_CODI(+)"

            SQL += " AND TNPL_MOVI.ADIC_CODI = TNPL_ADCO.ADIC_CODI(+)"
            ' SOLO MOVIMIENTOS DE CREDITO 
            SQL += " AND MOVI_DECR = 1 "
            SQL += "     AND TNPL_NCRE.NCRE_DAAN = " & "'" & Me.mFecha & "' "
            SQL += "ORDER BY TNPL_NCRE.SENC_CODI ASC, TNPL_NCRE.NCRE_CODI ASC "


            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read


                Linea = Linea + 1
                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)
                TotalLinea = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Decimal), 2)


                If IsDBNull(Me.DbLeeHotel.mDbLector("FACTURA")) = False Then
                    TextoAuxiliar = "Factura Nº= " & Me.DbLeeHotel.mDbLector("FACTURA")
                Else
                    TextoAuxiliar = "Sin Factura "
                End If


                ' DETERMINAR EL TIPO DE FACTURA 
                ' NOTA DE CREDIT0 DE CONTADO 

                Cuenta = Me.NEWGOLF.DevuelveCuentaContabledeNotaCredito(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                Dni = Me.NEWGOLF.DevuelveDniCifContabledeNotaCredito(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                TipoNota = Me.NEWGOLF.DevuelveTipoNotaCredito(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                CodigoCliente = Me.NEWGOLF.DevuelveCodigoEntiNotaCredito(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))

                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)


                If CType(Me.DbLeeHotel.mDbLector("SERVICIODELARTICULO"), String) = Me.mParaServCodiBonosAsoc Then
                    TipoVenta = "BONOS ASOC"
                Else
                    TipoVenta = CType(Me.DbLeeHotel.mDbLector("TIPOVENTA"), String)
                End If



                If Me.mParaIngresoPorHabitacion = 1 Then
                    If CType(Me.DbLeeHotel.mDbLector("SERVICIODELARTICULO"), String) = "?" Then
                        CodigoCompuesto = "000-" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String)
                    Else
                        CodigoCompuesto = "000-" & CType(Me.DbLeeHotel.mDbLector("SERVICIODELARTICULO"), String)
                    End If
                Else
                    CodigoCompuesto = Me.DbLeeHotel.mDbLector("SERVICIO")
                End If

                CodigoArticuloAx = CType(Me.DbLeeHotel.mDbLector("CODIGOARTICULOAX"), String)

                Me.mTipoAsiento = "DEBE"

                If DbLeeHotel.mDbLector("NCRE_DAAN") > DbLeeHotel.mDbLector("NCRE_DAEM") Then
                    Me.InsertaOracle("FV", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Me.mParaPreFijoNotadeCredito & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalLinea, "NO", Dni, CType(Me.DbLeeHotel.mDbLector("CONCEPTO"), String), "SI", "NOTA DE CREDITO ANULADA", "SAT_NHCreateSalesOrdersQueryService", CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Double), CType(Me.DbLeeHotel.mDbLector("LINBASE"), Double), CType(Me.DbLeeHotel.mDbLector("LINIMPU"), Double), Me.mParaPreFijoNotadeCredito & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String), CType(Me.DbLeeHotel.mDbLector("LINPORCEIGIC"), Double), TipoNota, CodigoCliente, TipoVenta, CodigoCompuesto, "NADA")
                Else
                    Me.InsertaOracle("FV", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Me.mParaPreFijoNotadeCredito & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalLinea, "NO", Dni, CType(Me.DbLeeHotel.mDbLector("CONCEPTO"), String), "SI", "NOTA DE CREDITO ANULADA MISMO DIA", "SAT_NHCreateSalesOrdersQueryService", CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Double), CType(Me.DbLeeHotel.mDbLector("LINBASE"), Double), CType(Me.DbLeeHotel.mDbLector("LINIMPU"), Double), Me.mParaPreFijoNotadeCredito & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String), CType(Me.DbLeeHotel.mDbLector("LINPORCEIGIC"), Double), TipoNota, CodigoCliente, TipoVenta, CodigoCompuesto, "NADA")
                End If

                Me.GeneraFileFV2("FV", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalFactura, Me.mParaPreFijoNotadeCredito & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String).PadRight(15, CChar(" ")), Cuenta, Dni)

            End While
            Me.DbLeeHotel.mDbLector.Close()
            Me.NEWGOLF.CerrarConexiones()


        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas")
            End If

        End Try
    End Sub
    Private Sub NotasCreditoGolfCobrosAx()
        Dim Total As Double
        Dim SQL As String

        Dim Cuenta As String
        Try

            SQL = "SELECT   MOVI_IMPT TOTAL, TNPL_FPAG.FPAG_DESC TIPO, TNPL_NCRE.SENC_CODI, "
            SQL += "         TNPL_NCRE.NCRE_CODI, NVL (TNPL_NCRE.NCRE_TITU, ' ') AS TITULAR, "
            SQL += "         NVL (TNPL_FPAG.FPAG_CTB1, '0') AS CUENTANEWG, "
            SQL += "         NVL (TNHT_FORE.FORE_CTB1, '0') AS CUENTANEWH, "
            SQL += "         NVL(CACR_CTBA,'0') CUENTAVISA, "
            SQL += "         TNHT_FORE.FORE_CODI AS TIPOCOBRO "
            SQL += "    FROM GMS.TNPL_MOVI, "
            SQL += "         GMS.TNPL_NCRE, "
            SQL += "         GMS.TNPL_MCRE, "
            SQL += "         GMS.TNPL_FPAG, "
            SQL += "         GMS.TNPL_FPCO, "
            SQL += "         GMS.TNPL_CACO, "
            SQL += "         TNHT_FORE, "
            SQL += "         TNHT_CACR "
            SQL += "   WHERE TNPL_MCRE.NCRE_CODI = TNPL_NCRE.NCRE_CODI "
            SQL += "     AND TNPL_MCRE.SENC_CODI = TNPL_NCRE.SENC_CODI "
            SQL += "     AND TNPL_MCRE.MOVI_DATE = TNPL_MOVI.MOVI_DATE "
            SQL += "     AND TNPL_MCRE.MOVI_CODI = TNPL_MOVI.MOVI_CODI "
            SQL += "     AND TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI "
            SQL += "     AND TNPL_FPAG.FPAG_CODI = TNPL_FPCO.FPAG_CODI(+) "
            SQL += "     AND TNPL_FPCO.FORE_CODI = TNHT_FORE.FORE_CODI "
            SQL += "       "
            SQL += "     AND TNPL_MOVI.CACR_CODI = TNPL_CACO.CACR_COD1(+) "
            SQL += "     AND TNPL_CACO.CACR_COD2 = TNHT_CACR.CACR_CODI(+) "
            SQL += "      "
            SQL += "     AND TNPL_MOVI.MOVI_DECR = '0' "
            SQL += "     AND TNPL_MOVI.MOVI_DEAN = '0' "
            SQL += "     AND TNPL_NCRE.NCRE_DAEM =" & "'" & Me.mFecha & "' "
            SQL += "ORDER BY TNPL_NCRE.SENC_CODI, TNPL_NCRE.NCRE_CODI "


            Me.DbLeeHotel.TraerLector(SQL)

            ' MsgBox("Control cuenta newgolf , newhotel ")
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)


                If CType(Me.DbLeeHotel.mDbLector("CUENTAVISA"), String) = "0" Then
                    Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTANEWH"), String)
                Else
                    Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTAVISA"), String)
                End If

                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI", "", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("TIPOCOBRO"), String), "", "", CType(Me.DbLeeHotel.mDbLector("NCRE_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SENC_CODI"), String), "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()



            SQL = "SELECT   MOVI_IMPT * -1 TOTAL  ,TNPL_FPAG.FPAG_DESC TIPO, TNPL_NCRE.SENC_CODI, "
            SQL += "         TNPL_NCRE.NCRE_CODI, NVL (TNPL_NCRE.NCRE_TITU, ' ') AS TITULAR, "
            SQL += "         NVL (TNPL_FPAG.FPAG_CTB1, '0') AS CUENTANEWG, "
            SQL += "         NVL (TNHT_FORE.FORE_CTB1, '0') AS CUENTANEWH, "
            SQL += "         NVL(CACR_CTBA,'0') CUENTAVISA, "
            SQL += "         TNHT_FORE.FORE_CODI AS TIPOCOBRO "
            SQL += "    FROM GMS.TNPL_MOVI, "
            SQL += "         GMS.TNPL_NCRE, "
            SQL += "         GMS.TNPL_MCRE, "
            SQL += "         GMS.TNPL_FPAG, "
            SQL += "         GMS.TNPL_FPCO, "
            SQL += "         GMS.TNPL_CACO, "
            SQL += "         TNHT_FORE, "
            SQL += "         TNHT_CACR "
            SQL += "   WHERE TNPL_MCRE.NCRE_CODI = TNPL_NCRE.NCRE_CODI "
            SQL += "     AND TNPL_MCRE.SENC_CODI = TNPL_NCRE.SENC_CODI "
            SQL += "     AND TNPL_MCRE.MOVI_DATE = TNPL_MOVI.MOVI_DATE "
            SQL += "     AND TNPL_MCRE.MOVI_CODI = TNPL_MOVI.MOVI_CODI "
            SQL += "     AND TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI "
            SQL += "     AND TNPL_FPAG.FPAG_CODI = TNPL_FPCO.FPAG_CODI(+) "
            SQL += "     AND TNPL_FPCO.FORE_CODI = TNHT_FORE.FORE_CODI "
            SQL += "       "
            SQL += "     AND TNPL_MOVI.CACR_CODI = TNPL_CACO.CACR_COD1(+) "
            SQL += "     AND TNPL_CACO.CACR_COD2 = TNHT_CACR.CACR_CODI(+) "
            SQL += "      "
            SQL += "     AND TNPL_MOVI.MOVI_DECR = '0' "
            SQL += "     AND TNPL_MOVI.MOVI_DEAN = '0' "
            SQL += "     AND TNPL_NCRE.NCRE_DAAN =" & "'" & Me.mFecha & "' "
            SQL += "ORDER BY TNPL_NCRE.SENC_CODI, TNPL_NCRE.NCRE_CODI "


            Me.DbLeeHotel.TraerLector(SQL)

            ' MsgBox("Control cuenta newgolf , newhotel ")
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)


                If CType(Me.DbLeeHotel.mDbLector("CUENTAVISA"), String) = "0" Then
                    Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTANEWH"), String)
                Else
                    Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTAVISA"), String)
                End If

                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI", "", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("TIPOCOBRO"), String), "", "", Me.mParaPreFijoNotadeCredito & CType(Me.DbLeeHotel.mDbLector("NCRE_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SENC_CODI"), String), "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()


        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try

    End Sub
    Private Sub NotasCreditoGolfIgicAgrupado()

        Try

            Dim TotalIva As Double
            Dim TotalBase As Double

            Dim DescripcionAsiento As String
            Dim Cuenta As String = ""


            SQL = ""
            SQL = "SELECT "
            SQL += "TNPL_TIVA.TIVA_PERC AS TIPO,ABS(SUM(VLIQ)) BASE, ABS(SUM(VIMP)) IGIC,TIVA_DESC "
            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_TIVA," & Me.mParaUsuarioNewGolf & ".VNPL_NCIV," & Me.mParaUsuarioNewGolf & ".TNPL_NCRE "

            SQL += "WHERE "
            SQL += "     TNPL_NCRE.SENC_CODI = VNPL_NCIV.SENC_CODI "
            SQL += "AND  TNPL_NCRE.NCRE_CODI = VNPL_NCIV.NCRE_CODI  "
            SQL += "AND  VNPL_NCIV.TIVA = TNPL_TIVA.TIVA_CODI  "

            SQL += "AND TNPL_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
            SQL += "GROUP BY TIVA_PERC,TIVA_DESC"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
                TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)


                TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
                TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

                DescripcionAsiento = " " & CType(Me.DbLeeHotel.mDbLector("TIVA_DESC"), String) & " " & Me.mFecha

                SQL = "SELECT NVL(TIVA_CTB1,'0') FROM " & StrConexionExtraeUsuario(mStrConexionHotel) & ".TNHT_TIVA WHERE TIVA_PERC = " & CType(Me.DbLeeHotel.mDbLector("TIPO"), Integer)
                Cuenta = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)



                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, DescripcionAsiento, TotalIva, "NO", Me.mClientesContadoCif, "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, DescripcionAsiento, TotalIva)

                Me.GeneraFileIV("IV", 31, Me.mEmpGrupoCod, Me.mEmpCod, "SERIE", 0, TotalBase, CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), TotalIva, "X")


            End While
            Me.DbLeeHotel.mDbLector.Close()

            ' ANULADAS 

            SQL = ""
            SQL = "SELECT "
            SQL += "TNPL_TIVA.TIVA_PERC AS TIPO,ABS(SUM(VLIQ)) BASE, ABS(SUM(VIMP)) IGIC,TIVA_DESC "
            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_TIVA," & Me.mParaUsuarioNewGolf & ".VNPL_NCIV," & Me.mParaUsuarioNewGolf & ".TNPL_NCRE "

            SQL += "WHERE "
            SQL += "     TNPL_NCRE.SENC_CODI = VNPL_NCIV.SENC_CODI "
            SQL += "AND  TNPL_NCRE.NCRE_CODI = VNPL_NCIV.NCRE_CODI  "
            SQL += "AND  VNPL_NCIV.TIVA = TNPL_TIVA.TIVA_CODI  "

            SQL += "AND TNPL_NCRE.NCRE_DAAN = " & "'" & Me.mFecha & "' "
            SQL += "GROUP BY TIVA_PERC,TIVA_DESC"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
                TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)


                TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
                TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

                DescripcionAsiento = " " & CType(Me.DbLeeHotel.mDbLector("TIVA_DESC"), String) & " " & Me.mFecha

                SQL = "SELECT NVL(TIVA_CTB1,'0') FROM " & StrConexionExtraeUsuario(mStrConexionHotel) & ".TNHT_TIVA WHERE TIVA_PERC = " & CType(Me.DbLeeHotel.mDbLector("TIPO"), Integer)
                Cuenta = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)



                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 31, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, DescripcionAsiento, TotalIva, "NO", Me.mClientesContadoCif, "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, DescripcionAsiento, TotalIva)

                Me.GeneraFileIV("IV", 31, Me.mEmpGrupoCod, Me.mEmpCod, "SERIE", 0, TotalBase, CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), TotalIva, "X")


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Detalle de Impuesto")
            End If

        End Try
    End Sub


#End Region
#Region "ASIENTO-3 FACTURACION"
    Private Sub FacturasTotalLiquido()

        Dim Total As Double

        Dim TotalComisiones As Double
        Dim SQL As String

        Try


20080721:

            SQL = "SELECT (SUM(FAIV_INCI) - SUM(FAIV_VIMP)) TOTAL ,SUM(FACT_TOTA) TOTAL1,FACT_DAEM "
            SQL += "FROM TNHT_FAIV, TNHT_FACT "
            SQL += "WHERE TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
            SQL += "GROUP BY TNHT_FACT.FACT_DAEM"

            If IsDBNull(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = False Then
                Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            Else
                Total = 0
            End If

            Total = Total + FacturacionTotalServiciosSinIgic()


            ' tinglado de sumar las comisiones al total liquido

            SQL = "SELECT NVL(SUM(TNHT_DESF.DESF_VALO),'0')TOTAL "
            SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE WHERE"
            SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
            SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
            SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
            SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"


            If Me.mParaComisiones = True Then
                TotalComisiones = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
                Total = Total + TotalComisiones
            End If
            Total = Decimal.Round(CType(Total, Decimal), 2)

            If Total <> 0 Then
                Linea = 1
                Me.mTipoAsiento = "HABER"
                Me.mTotalFacturacion = Total

                Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "FACTURACION " & Me.mFecha, Total, "SI", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, "FACTURACION " & Me.mFecha, Total)

            End If


            If mParaConectaNewGolf = 0 Or Me.mTratarBonos = False Then Exit Sub


            ' EXTRAER EL TOTAL LIQUIDO DE LOS BONOS DE LA CUENTA DE MANO CORRIENTE 485 BONOS  (ESTHER)

            SQL = "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI SERVICIO,TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CTB1,'0') CUENTA ,NVL(BLAL_DESC,'OTROS INGRESOS') AS BLOQUE,TNHT_BLAL.BLAL_CODI,"
            SQL += "ROUND(SUM(TNHT_MOVI.MOVI_VLIQ), 2) TOTAL "
            SQL += " FROM VNHT_MOVH TNHT_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_ADIC," & Me.mParaUsuarioNewGolf & ".TNPL_MOVI,TNHT_SERV"
            SQL += ",TNHT_ALOJ,TNHT_BLAL "
            SQL += " WHERE TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI"
            SQL += " AND TNHT_MOVI.MOVI_CODI = TNPL_MOVI.NEWH_CODI"
            SQL += " AND TNHT_MOVI.MOVI_DARE = TNPL_MOVI.NEWH_DARE"
            SQL += " AND TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI"

            SQL += " AND TNHT_MOVI.ALOJ_CODI = TNHT_ALOJ.ALOJ_CODI(+) "
            SQL += " AND TNHT_ALOJ.BLAL_CODI = TNHT_BLAL.BLAL_CODI(+) "

            SQL += " AND MOVI_DATR= '" & Me.mFecha & "'"
            SQL += " AND TNPL_MOVI.ADIC_CODI IN"
            SQL += "(SELECT ADIC_CODI FROM GMS.TNPL_ADIC WHERE ADIC_TIPO = 3)"
            SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,TNHT_SERV.SERV_COMS,BLAL_DESC,TNHT_BLAL.BLAL_CODI"
            SQL += " ORDER BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1"



            ' CONTRAPARTIDA POR DEPARTAMENTO  ESTHER (2)  ( BUSCA EL DPTO DE NEWHOTEL EN NEWGOLF POR PROBLEMA EN ENLACE  )

            SQL = "SELECT SUM(SIGN(MOVI_IMPT) *ABS(MOVI_VLIQ)) AS TOTAL, SUM(SIGN(MOVI_IMPT) *ABS(MOVI_IMP1)) AS IMPUESTO,"
            SQL += "TNHT_SERV.SERV_CODI AS SERVICIO, TNHT_SERV.SERV_DESC DEPARTAMENTO, NVL(TNHT_SERV.SERV_CTB1,   '0') CUENTA, NVL(BLAL_DESC,   'OTROS INGRESOS') AS BLOQUE "
            SQL += " FROM  " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_ADCO," & Me.mParaUsuarioNewGolf & ".TNPL_ADIC," & "TNHT_SERV, TNHT_ALOJ, TNHT_BLAL"

            SQL += " WHERE TNPL_MOVI.ADIC_CODI = TNPL_ADCO.ADIC_CODI "
            SQL += " AND TNPL_ADCO.SERV_CODI = TNHT_SERV.SERV_CODI"
            SQL += " AND TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI"
            SQL += " AND TNPL_MOVI.ALOJ_CODI = TNHT_ALOJ.ALOJ_CODI(+)"
            SQL += " AND TNHT_ALOJ.BLAL_CODI = TNHT_BLAL.BLAL_CODI(+)"

            SQL += " AND MOVI_FECH = '" & Me.mFecha & "'"
            SQL += " AND TNPL_MOVI.ADIC_CODI IN (SELECT ADIC_CODI FROM GMS.TNPL_ADIC WHERE ADIC_TIPO = 3)"

            SQL += " AND MOVI_ANUL = 0"

            ' EXCLUIR BONOS ASOCIACION DE CAMPOS 
            SQL += " AND TIAD_CODI <> " & Me.MparaTipoBonoAsociacion
            SQL += " GROUP BY TNHT_SERV.SERV_CODI, TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,BLAL_DESC"




            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

                If Total <> 0 Then
                    Linea = Linea + 1
                    Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "PRODUCCIÓN BONOS " & Me.mFecha, Total, "SI", "", "", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, "PRODUCCIÓN BONOS " & Me.mFecha, Total)

                    Linea = Linea + 1
                    Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaBcta2, Me.mIndicadorHaber, "PRODUCCIÓN BONOS " & Me.mFecha, Total, "SI", "", "", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaBcta2, Me.mIndicadorHaber, "PRODUCCIÓN BONOS " & Me.mFecha, Total)

                End If

            End While
            Me.DbLeeHotel.mDbLector.Close()


        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Total LIQUIDO Facturas")
            End If

        End Try

    End Sub

    Private Sub FacturasSalidaTotaLDescuentos()
        Dim Total As Double
        Try
            SQL = "SELECT SUM(TNHT_DESF.DESF_VALO)TOTAL,TNHT_TIDE.TIDE_DESC TIPO,NVL(TNHT_TIDE.TIDE_CTB1,'0') CUENTA ,"
            SQL += "DECODE(FAAN_CODI ,NULL,'NO','SI') AS CANCELADA "
            SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE WHERE"
            SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
            SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
            SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
            SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
            SQL = SQL & " GROUP BY TNHT_TIDE.TIDE_DESC,TNHT_TIDE.TIDE_CTB1,"
            SQL += "DECODE(FAAN_CODI ,NULL,'NO','SI') "


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

        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Total Descuentos Facturas")
            End If

        End Try
    End Sub
    Private Sub FacturasSalidaTotalFActuraObsoleto()
        Try
            Dim TotalFactura As Double
            Dim Dni As String = ""
            Dim Cuenta As String = "0"
            Dim Titular As String

            SQL = "SELECT  TNHT_FACT.FACT_STAT AS ESTADO, TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,TNHT_FACT.FACT_TOTA TOTAL,TNHT_FACT.FACT_VALO VALOR,NVL(ENTI_CODI,'') AS ENTI_CODI,NVL(CCEX_CODI,'?') AS CCEX_CODI,NVL(CLIE_CODI,'0') AS CLIENTE "
            SQL += " , NVL(TNHT_FACT.FACT_TITU,'?') TITULAR "
            SQL += "FROM TNHT_FACT "
            SQL += "WHERE "
            SQL += "(TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "
            SQL += "ORDER BY TNHT_FACT.SEFA_CODI ASC, TNHT_FACT.FACT_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)


            While Me.DbLeeHotel.mDbLector.Read

                Linea = Linea + 1

                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)



                ' DETERMINAR EL TIPO DE FACTURA 
                ' FACTURA DE CONTADO NUEVO

                If CType(Me.DbLeeHotel.mDbLector("ESTADO"), String) = "1" Then

                    If IsDBNull(Me.DbLeeHotel.mDbLector("ENTI_CODI")) = False Then
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

                    Else

                        SQL = "SELECT NVL(CLIE_NUID,'0') FROM TNHT_CLIE WHERE CLIE_CODI = " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), Integer)
                        Dni = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                        If Dni = "0" Or IsNothing(Dni) = True Then
                            Dni = Me.mClientesContadoCif
                        End If
                        Cuenta = Me.mCtaClientesContado
                    End If
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

                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)


                If IsDBNull(Cuenta) = True Or IsNothing(Cuenta) = True Then
                    MsgBox("Atención Factura Sin Regularizar " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String) & " " & Titular, MsgBoxStyle.Exclamation, "Atención")
                    Cuenta = InputBox("Ingrese Cuenta contable para Factura " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), Titular)

                    If IsDBNull(Cuenta) = True Or IsNothing(Cuenta) = True Or Cuenta = "" Then
                        Cuenta = "0"
                    End If
                End If


                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Dni, Titular, "SI")
                Me.GeneraFileFV2("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalFactura, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String).PadRight(15, CChar(" ")), Cuenta, Dni)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas")
            End If

        End Try
    End Sub
    Private Sub FacturasSalidaTotalFActuraNuevo()
        Try
            Dim TotalFactura As Double
            Dim Dni As String = ""
            Dim Cuenta As String
            Dim Titular As String

            ' se quita null de codigo de entidad y ceex_codi de la sql  
            ' se quita debajo de la quiery  ( if   de ccex_codi = 'tpv' paso de valor o producccion 

            SQL = "SELECT  TNHT_FACT.FACT_STAT AS ESTADO, TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,TNHT_FACT.FACT_TOTA TOTAL,TNHT_FACT.FACT_VALO VALOR, ENTI_CODI,CCEX_CODI,NVL(CLIE_CODI,'0') AS CLIENTE "
            SQL += " , NVL(TNHT_FACT.FACT_TITU,'?') TITULAR "
            SQL += "FROM TNHT_FACT "
            SQL += "WHERE "
            SQL += "(TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "
            SQL += "ORDER BY TNHT_FACT.SEFA_CODI ASC, TNHT_FACT.FACT_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)

            Me.NEWHOTEL = New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)

            While Me.DbLeeHotel.mDbLector.Read


                Linea = Linea + 1
                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)


                ' DETERMINAR EL TIPO DE FACTURA 
                ' FACTURA DE CONTADO NUEVO

                Cuenta = Me.NEWHOTEL.DevuelveCuentaContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                Dni = Me.NEWHOTEL.DevuelveDniCifContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))

                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)


                If IsDBNull(Cuenta) = True Or IsNothing(Cuenta) = True Then
                    MsgBox("Atención Factura Sin Regularizar " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String) & " " & Titular, MsgBoxStyle.Exclamation, "Atención")
                    Cuenta = InputBox("Ingrese Cuenta contable para Factura " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), Titular)

                    If IsDBNull(Cuenta) = True Or IsNothing(Cuenta) = True Or Cuenta = "" Then
                        Cuenta = "0"
                    End If
                End If


                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Dni, Titular, "SI", "COBRO")
                Me.GeneraFileFV2("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalFactura, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String).PadRight(15, CChar(" ")), Cuenta, Dni)

            End While
            Me.DbLeeHotel.mDbLector.Close()

            Me.NEWHOTEL.CerrarConexiones()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas")
            End If

        End Try
    End Sub
    Private Sub FacturasSalidaTotalFActuraNuevoAX()
        Try
            Dim TotalFactura As Double
           
            Dim TotalLinea As Double
            Dim Dni As String = ""
            Dim Cuenta As String = ""
            Dim Titular As String
            Dim TipoFactura As Integer
            Dim CodigoCliente As String = ""

            Dim CodigoCompuesto As String

            Dim UltimoAvisoNif As String = " "

            Dim Primerregistro As Boolean = True
            Dim ControlFactura As String = ""


            Dim BookId As String
            


            ' se quita null de codigo de entidad y ceex_codi de la sql  
            ' se quita debajo de la quiery  ( if   de ccex_codi = 'tpv' paso de valor o producccion 


            SQL = "SELECT TNHT_FACT.RESE_CODI,TNHT_FACT.RESE_ANCI ,"
            SQL += "  TNHT_FACT.FACT_STAT AS ESTADO, TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,TNHT_FACT.FACT_TOTA TOTAL,TNHT_FACT.FACT_VALO VALOR, ENTI_CODI,NVL(TNHT_FACT.CCEX_CODI,'?') AS CCEX_CODI,NVL(CLIE_CODI,'0') AS CLIENTE "
            SQL += " , NVL(TNHT_FACT.FACT_TITU,'?') TITULAR,NVL(TNHT_SERV.SERV_DESC,' ') CONCEPTO, TNHT_SERV.SERV_CODI AS SERVICIO,"
            SQL += " TNHT_MOVI.MOVI_VCRE AS LINBRUT,TNHT_MOVI.MOVI_VLIQ AS LINBASE , TNHT_MOVI.MOVI_IMP1 AS LINIMPU, NVL(TNHT_TIVA.TIVA_PERC,'0') AS LINPORCEIGIC,FAAN_CODI "
            SQL += " ,NVL(TNHT_BLAL.BLAL_CODI,'000') AS CODIGOBLOQUE  "

            SQL += ",TNHT_FACT.FACT_NUCO AS FACT_NUCO ,NVL(TNHT_FACT.FACT_TITU,'?') AS FACT_TITU "

            SQL += " ,NVL(FACT_CLEN,'0') AS FACT_CLEN "
            'SQL += " FROM TNHT_FACT,TNHT_FAMO,TNHT_MOVI,TNHT_TIVA,TNHT_SERV,TNHT_ALOJ,TNHT_BLAL "
            ' SE INCLUYE MOVIMIENTOS EN HISTORICO FEBRERO 2015
            SQL += " FROM TNHT_FACT,TNHT_FAMO,VNHT_MOVH TNHT_MOVI,TNHT_TIVA,TNHT_SERV,TNHT_ALOJ,TNHT_BLAL "


            SQL += " WHERE TNHT_FACT.FACT_CODI = TNHT_FAMO.FACT_CODI AND "
            SQL += "       TNHT_FACT.SEFA_CODI = TNHT_FAMO.SEFA_CODI AND "

            SQL += "     TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            SQL += " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "

            SQL += " AND TNHT_MOVI.MOVI_IVA1 = TNHT_TIVA.TIVA_CODI(+)"
            SQL += " AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "

            SQL += " AND TNHT_MOVI.ALOJ_CODI = TNHT_ALOJ.ALOJ_CODI(+) "
            SQL += " AND TNHT_ALOJ.BLAL_CODI = TNHT_BLAL.BLAL_CODI(+) "


            SQL += " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "

            ' SOLO MOVIMIENTOS DE CREDITO 
            SQL += " AND TNHT_MOVI.MOVI_TIMO = '1' "
            SQL += " ORDER BY TNHT_FACT.SEFA_CODI ASC, TNHT_FACT.FACT_CODI,TNHT_MOVI.MOVI_DATR,TNHT_MOVI.SERV_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)

            Me.NEWHOTEL = New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)

            While Me.DbLeeHotel.mDbLector.Read

                BookId = Me.BuscaReservasdeunaFactura(Me.DbLeeHotel.mDbLector("NUMERO"), Me.DbLeeHotel.mDbLector("SERIE"), Me.DbLeeHotel.mDbLector("FACT_CLEN"))

                Linea = Linea + 1

                ' si redondeo asi 70,705  = 70,70

                If Me.mTipoRedondeo = ETIPOREDONDEO.AwayFromZero Then
                    TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2, MidpointRounding.AwayFromZero)
                Else
                    TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)
                End If

                '
                ' si  redondeo asi 70,705  = 70,71
                ' AQUI AGOSTO 2011
                'TotalFactura2 = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2, MidpointRounding.AwayFromZero)
                'TotalFactura3 = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2, MidpointRounding.ToEven)

                'If TotalFactura <> TotalFactura2 Then
                '    MsgBox(Me.DbLeeHotel.mDbLector("VALOR") & vbCrLf & "Normal = " & TotalFactura & vbCrLf & " fromzero =  " & TotalFactura2 & vbCrLf & "toeven =  " & TotalFactura3, MsgBoxStyle.Information, CStr(Me.DbLeeHotel.mDbLector("NUMERO")))

                ' End If


                If Me.mParaIngresoPorHabitacion = 1 Then
                    CodigoCompuesto = Me.DbLeeHotel.mDbLector("CODIGOBLOQUE") & "-" & Me.DbLeeHotel.mDbLector("SERVICIO")
                Else
                    CodigoCompuesto = Me.DbLeeHotel.mDbLector("SERVICIO")
                End If



                ' DETERMINAR EL TIPO DE FACTURA 
                ' FACTURA DE CONTADO NUEVO


                If Primerregistro = True Then
                    ControlFactura = CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & CType(Me.DbLeeHotel.mDbLector("SERIE"), String)
                End If


                If Primerregistro = True Or ControlFactura <> CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & CType(Me.DbLeeHotel.mDbLector("SERIE"), String) Then
                    Cuenta = Me.NEWHOTEL.DevuelveCuentaContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                    Dni = Me.NEWHOTEL.DevuelveDniCifContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                    TipoFactura = Me.NEWHOTEL.DevuelveTipodeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                    CodigoCliente = Me.NEWHOTEL.DevuelveCodigoEntiCcexdeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))

                    Primerregistro = False
                    ControlFactura = CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & CType(Me.DbLeeHotel.mDbLector("SERIE"), String)

                End If



                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)


                ' CONTROL CASO DE TENER LA FACTURA CODIGO DE ENTIDAD O NO ALOJADO Y EL USUARIO HABER CAMBIADO 
                ' EL NIF Y EL NOMBRE DEL TITULAR AL EMITIR LA FACTURA

                ' PREVALECE EL NIF PUESTO A MANO POR EL USUARIO 



                If IsDBNull(Me.DbLeeHotel.mDbLector("FACT_NUCO")) = False Then
                    If CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) <> Dni And Dni <> Me.mClientesContadoCif Then
                        ' TRATA DE AVISAR SOLO UNA VZ

                        If CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) <> UltimoAvisoNif Then
                            If mMostrarMensajes = True Then
                                MsgBox("Atención Discrepancia de Nif " & vbCrLf & "Nif que Corresponde " & Dni & vbCrLf & "Nif hallado en Factura " & CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) & vbCrLf & vbCrLf & "Documento " & (CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String)), MsgBoxStyle.Exclamation, "Atención ")
                            Else
                                Me.mListBoxDebug.Items.Add("Atención Discrepancia de Nif " & vbCrLf & "Nif que Corresponde " & Dni & vbCrLf & "Nif hallado en Factura " & CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) & vbCrLf & vbCrLf & "Documento " & (CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String)))
                            End If
                            UltimoAvisoNif = CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String)
                        End If

                        ' quito debajo por peticion de domingo solo avisamos de discrepancia 
                        '      Dni = CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String)

                    End If
                End If

                If IsDBNull(Cuenta) = True Or IsNothing(Cuenta) = True Then
                    MsgBox("Atención Factura Sin Regularizar " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String) & " " & Titular, MsgBoxStyle.Exclamation, "Atención")
                    Cuenta = InputBox("Ingrese Cuenta contable para Factura " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), Titular)

                    If IsDBNull(Cuenta) = True Or IsNothing(Cuenta) = True Or Cuenta = "" Then
                        Cuenta = "0"
                    End If
                End If


                Me.mTipoAsiento = "DEBE"

                If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                    If Me.mTipoRedondeo = ETIPOREDONDEO.AwayFromZero Then
                        TotalLinea = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Decimal), 2, MidpointRounding.AwayFromZero)
                    Else
                        TotalLinea = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Decimal), 2)
                    End If
                    Me.InsertaOracle("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalLinea, "NO", Dni, CType(Me.DbLeeHotel.mDbLector("CONCEPTO"), String), "SI", "COBRO", "SAT_NHCreateSalesOrdersQueryService", CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Double), CType(Me.DbLeeHotel.mDbLector("LINBASE"), Double), CType(Me.DbLeeHotel.mDbLector("LINIMPU"), Double), CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String), CType(Me.DbLeeHotel.mDbLector("LINPORCEIGIC"), Double), TipoFactura, CodigoCliente, CodigoCompuesto, "NADA", CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), "", BookId)
                Else
                    If Me.mTipoRedondeo = ETIPOREDONDEO.AwayFromZero Then
                        TotalLinea = (Decimal.Round(CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Decimal), 2, MidpointRounding.AwayFromZero) * -1)
                    Else
                        TotalLinea = (Decimal.Round(CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Decimal), 2) * -1)
                    End If
                    Me.InsertaOracle("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalLinea, "NO", Dni, CType(Me.DbLeeHotel.mDbLector("CONCEPTO"), String), "SI", "COBRO", "SAT_NHCreateSalesOrdersQueryService", (CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Double) * -1), (CType(Me.DbLeeHotel.mDbLector("LINBASE"), Double) * -1), (CType(Me.DbLeeHotel.mDbLector("LINIMPU"), Double) * -1), CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String), CType(Me.DbLeeHotel.mDbLector("LINPORCEIGIC"), Double), TipoFactura, CodigoCliente, CodigoCompuesto, "NADA", CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), "", BookId)
                End If


            End While
            Me.DbLeeHotel.mDbLector.Close()
            Me.NEWHOTEL.CerrarConexiones()


            ' FACTURAS SIN MOVIMIENTOS DE CREDIT0

            SQL = "SELECT TNHT_FACT.RESE_CODI,TNHT_FACT.RESE_ANCI ,"
            SQL += " TNHT_FACT.FACT_STAT AS ESTADO, TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,TNHT_FACT.FACT_TOTA TOTAL,TNHT_FACT.FACT_VALO VALOR, ENTI_CODI,NVL(TNHT_FACT.CCEX_CODI,'?') AS CCEX_CODI,NVL(CLIE_CODI,'0') AS CLIENTE "
            SQL += " , NVL(TNHT_FACT.FACT_TITU,'?') TITULAR,NVL(TNHT_SERV.SERV_DESC,'(*) Anulación de Reserva Con Anticipos ') CONCEPTO, NVL(TNHT_SERV.SERV_CODI,' ') AS SERVICIO,"
            SQL += " TNHT_MOVI.MOVI_VDEB AS LINBRUT,TNHT_MOVI.MOVI_VLIQ AS LINBASE , TNHT_MOVI.MOVI_IMP1 AS LINIMPU, NVL(TNHT_TIVA.TIVA_PERC,'0') AS LINPORCEIGIC,FAAN_CODI "
            SQL += " ,NVL(TNHT_BLAL.BLAL_CODI,'000') AS CODIGOBLOQUE  "

            SQL += ",TNHT_FACT.FACT_NUCO AS FACT_NUCO ,NVL(TNHT_FACT.FACT_TITU,'?') AS FACT_TITU "

            SQL += " ,NVL(FACT_CLEN,'0') AS FACT_CLEN "

            SQL += " FROM TNHT_FACT,TNHT_FAMO,VNHT_MOVH TNHT_MOVI,TNHT_TIVA,TNHT_SERV,TNHT_ALOJ,TNHT_BLAL "

            SQL += " WHERE TNHT_FACT.FACT_CODI = TNHT_FAMO.FACT_CODI AND "
            SQL += "       TNHT_FACT.SEFA_CODI = TNHT_FAMO.SEFA_CODI AND "

            SQL += "     TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            SQL += " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "

            SQL += " AND TNHT_MOVI.MOVI_IVA1 = TNHT_TIVA.TIVA_CODI(+)"
            SQL += " AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI(+) "

            SQL += " AND TNHT_MOVI.ALOJ_CODI = TNHT_ALOJ.ALOJ_CODI(+) "
            SQL += " AND TNHT_ALOJ.BLAL_CODI = TNHT_BLAL.BLAL_CODI(+) "


            SQL += " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "

            ' SOLO MOVIMIENTOS DE DEBITO 
            SQL += " AND TNHT_MOVI.MOVI_TIMO = '2' "

            ' FACTURAS SIN LINEAS DE CRADITO

            SQL += " AND (TNHT_FACT.FACT_CODI, TNHT_FACT.SEFA_CODI) NOT IN ( "
            SQL += "            SELECT TNHT_FACT.FACT_CODI, TNHT_FACT.SEFA_CODI "
            SQL += "              FROM TNHT_FACT, TNHT_FAMO, VNHT_MOVH TNHT_MOVI "
            SQL += "             WHERE TNHT_FACT.FACT_CODI = TNHT_FAMO.FACT_CODI "
            SQL += "               AND TNHT_FACT.SEFA_CODI = TNHT_FAMO.SEFA_CODI "
            SQL += "               AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += "               AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += "               AND TNHT_MOVI.MOVI_TIMO = '1') "

            SQL += " ORDER BY TNHT_FACT.SEFA_CODI ASC, TNHT_FACT.FACT_CODI,TNHT_MOVI.MOVI_DATR,TNHT_MOVI.SERV_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)

            Me.NEWHOTEL = New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)

            While Me.DbLeeHotel.mDbLector.Read


                BookId = Me.BuscaReservasdeunaFactura(Me.DbLeeHotel.mDbLector("NUMERO"), Me.DbLeeHotel.mDbLector("SERIE"), Me.DbLeeHotel.mDbLector("FACT_CLEN"))


                Linea = Linea + 1
                '  TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)

                If Me.mTipoRedondeo = ETIPOREDONDEO.AwayFromZero Then
                    TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2, MidpointRounding.AwayFromZero)
                Else
                    TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)
                End If



                If Me.mParaIngresoPorHabitacion = 1 Then
                    CodigoCompuesto = Me.DbLeeHotel.mDbLector("CODIGOBLOQUE") & "-" & Me.mParaArticuloAnulareserva
                Else
                    CodigoCompuesto = Me.mParaArticuloAnulareserva
                End If


                ' DETERMINAR EL TIPO DE FACTURA 
                ' FACTURA DE CONTADO NUEVO

                ' PENDIENTE ( CUANDO HAYA TIEMPO NO PREGUNTAR DEBAJO POR CADA LINEA DE FACTURA SINO UNA VEZ POR LA CABECERA ) ESTO DEMORA UN MONTON 

                Cuenta = Me.NEWHOTEL.DevuelveCuentaContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                Dni = Me.NEWHOTEL.DevuelveDniCifContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                TipoFactura = Me.NEWHOTEL.DevuelveTipodeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                CodigoCliente = Me.NEWHOTEL.DevuelveCodigoEntiCcexdeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))

                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)


                ' CONTROL CASO DE TENER LA FACTURA CODIGO DE ENTIDAD O NO ALOJADO Y EL USUARIO HABER CAMBIADO 
                ' EL NIF Y EL NOMBRE DEL TITULAR AL EMITIR LA FACTURA

                ' PREVALECE EL NIF PUESTO A MANO POR EL USUARIO 



                If IsDBNull(Me.DbLeeHotel.mDbLector("FACT_NUCO")) = False Then
                    If CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) <> Dni And Dni <> Me.mClientesContadoCif Then
                        ' TRATA DE AVISAR SOLO UNA VZ

                        If CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) <> UltimoAvisoNif Then
                            If mMostrarMensajes = True Then
                                MsgBox("Atención Discrepancia de Nif " & vbCrLf & "Nif que Corresponde " & Dni & vbCrLf & "Nif hallado en Factura " & CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) & vbCrLf & vbCrLf & "Documento " & (CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String)), MsgBoxStyle.Exclamation, "Atención ")
                            Else
                                Me.mListBoxDebug.Items.Add("Atención Discrepancia de Nif " & vbCrLf & "Nif que Corresponde " & Dni & vbCrLf & "Nif hallado en Factura " & CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) & vbCrLf & vbCrLf & "Documento " & (CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String)))
                            End If
                            UltimoAvisoNif = CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String)
                        End If

                        ' quito debajo por peticion de domingo solo avisamos de discrepancia 
                        '      Dni = CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String)

                    End If
                End If

                If IsDBNull(Cuenta) = True Or IsNothing(Cuenta) = True Then
                    MsgBox("Atención Factura Sin Regularizar " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String) & " " & Titular, MsgBoxStyle.Exclamation, "Atención")
                    Cuenta = InputBox("Ingrese Cuenta contable para Factura " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), Titular)

                    If IsDBNull(Cuenta) = True Or IsNothing(Cuenta) = True Or Cuenta = "" Then
                        Cuenta = "0"
                    End If
                End If


                Me.mTipoAsiento = "DEBE"

                If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                    If Me.mTipoRedondeo = ETIPOREDONDEO.AwayFromZero Then
                        TotalLinea = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Decimal), 2, MidpointRounding.AwayFromZero)
                    Else
                        TotalLinea = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Decimal), 2)

                    End If
                    Me.InsertaOracle("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalLinea, "NO", Dni, CType(Me.DbLeeHotel.mDbLector("CONCEPTO"), String), "SI", "COBRO", "SAT_NHCreateSalesOrdersQueryService", CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Double), CType(Me.DbLeeHotel.mDbLector("LINBASE"), Double), CType(Me.DbLeeHotel.mDbLector("LINIMPU"), Double), CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String), CType(Me.DbLeeHotel.mDbLector("LINPORCEIGIC"), Double), TipoFactura, CodigoCliente, CodigoCompuesto, "NADA", CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), "", BookId)
                    ' LO MISMO AL REVEZ
                    Linea = Linea + 1
                    If Me.mTipoRedondeo = ETIPOREDONDEO.AwayFromZero Then
                        TotalLinea = (Decimal.Round(CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Decimal), 2, MidpointRounding.AwayFromZero) * -1)
                    Else
                        TotalLinea = (Decimal.Round(CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Decimal), 2) * -1)
                    End If
                    Me.InsertaOracle("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalLinea, "NO", Dni, CType(Me.DbLeeHotel.mDbLector("CONCEPTO"), String), "SI", "COBRO", "SAT_NHCreateSalesOrdersQueryService", CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Double), CType(Me.DbLeeHotel.mDbLector("LINBASE"), Double), CType(Me.DbLeeHotel.mDbLector("LINIMPU"), Double), CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String), CType(Me.DbLeeHotel.mDbLector("LINPORCEIGIC"), Double), TipoFactura, CodigoCliente, CodigoCompuesto, "NADA", CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), "", BookId)


                Else
                    If Me.mTipoRedondeo = ETIPOREDONDEO.AwayFromZero Then
                        TotalLinea = (Decimal.Round(CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Decimal), 2, MidpointRounding.AwayFromZero) * -1)
                    Else
                        TotalLinea = (Decimal.Round(CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Decimal), 2) * -1)

                    End If
                    Me.InsertaOracle("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalLinea, "NO", Dni, CType(Me.DbLeeHotel.mDbLector("CONCEPTO"), String), "SI", "COBRO", "SAT_NHCreateSalesOrdersQueryService", (CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Double) * -1), (CType(Me.DbLeeHotel.mDbLector("LINBASE"), Double) * -1), (CType(Me.DbLeeHotel.mDbLector("LINIMPU"), Double) * -1), CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String), CType(Me.DbLeeHotel.mDbLector("LINPORCEIGIC"), Double), TipoFactura, CodigoCliente, CodigoCompuesto, "NADA", CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), "", BookId)
                    ' LO MISMO AL REVEZ
                    Linea = Linea + 1
                    If Me.mTipoRedondeo = ETIPOREDONDEO.AwayFromZero Then
                        TotalLinea = (Decimal.Round(CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Decimal), 2, MidpointRounding.AwayFromZero))
                    Else
                        TotalLinea = (Decimal.Round(CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Decimal), 2))
                    End If

                    Me.InsertaOracle("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalLinea, "NO", Dni, CType(Me.DbLeeHotel.mDbLector("CONCEPTO"), String), "SI", "COBRO", "SAT_NHCreateSalesOrdersQueryService", (CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Double) * -1), (CType(Me.DbLeeHotel.mDbLector("LINBASE"), Double) * -1), (CType(Me.DbLeeHotel.mDbLector("LINIMPU"), Double) * -1), CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String), CType(Me.DbLeeHotel.mDbLector("LINPORCEIGIC"), Double), TipoFactura, CodigoCliente, CodigoCompuesto, "NADA", CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), "", BookId)

                End If


            End While
            Me.DbLeeHotel.mDbLector.Close()
            Me.NEWHOTEL.CerrarConexiones()




        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas")
            End If

        End Try
    End Sub


    Private Sub FacturasSalidaIgicAgrupado()

        Try

            Dim TotalIva As Double
            Dim TotalBase As Double

            Dim DescripcionAsiento As String = "I.G.I.C FACTURACION " & Me.mFecha
            SQL = ""
            SQL = "SELECT "
            SQL += "TNHT_FAIV.FAIV_TAXA AS TIPO,SUM((FAIV_INCI-FAIV_VIMP)) BASE, SUM(TNHT_FAIV.FAIV_VIMP) IGIC,NVL(TIVA_CTB1,'0') CUENTA, '"
            SQL += Me.mParaTextoIva & " ' || FAIV_TAXA ||'%  ' ,"

            SQL += "DECODE(FAAN_CODI ,NULL,'NO','SI') AS CANCELADA "
            SQL += "FROM TNHT_FAIV, TNHT_FACT,TNHT_TIVA "
            SQL += "WHERE TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "AND TNHT_FAIV.TIVA_CODI = TNHT_TIVA.TIVA_CODI "
            SQL += "AND (TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "
            SQL += "GROUP BY TNHT_FAIV.FAIV_TAXA,TIVA_CTB1,"
            SQL += "DECODE(FAAN_CODI ,NULL,'NO','SI')"

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



                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, DescripcionAsiento, TotalIva, "NO", Me.mClientesContadoCif, "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, DescripcionAsiento, TotalIva)

                Me.GeneraFileIV("IV", 3, Me.mEmpGrupoCod, Me.mEmpCod, "SERIE", 0, TotalBase, CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), TotalIva, "X")


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Detalle de Impuesto")
            End If

        End Try
    End Sub


#End Region
#Region "ASIENTO-34 FACTURACION NEWGOLF"
    Private Sub FacturasTotalLiquidoGolf()

        Dim Total As Double

        Dim SQL As String

        SQL = "SELECT (SUM(FAIV_INCI) - SUM(FAIV_VIMP)) TOTAL "
        SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_FAIV," & Me.mParaUsuarioNewGolf & ".TNPL_FACT "
        SQL += "WHERE TNPL_FAIV.SEFA_CODI = TNPL_FACT.SEFA_CODI "
        SQL += "AND TNPL_FAIV.FACT_CODI = TNPL_FACT.FACT_CODI "
        SQL += "AND TNPL_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
        SQL += "GROUP BY TNPL_FACT.FACT_DAEM"

        If IsDBNull(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = False Then
            Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            '+ Me.FacturacionSalidasServiciosSinIgic + Me.FacturacionSalidaDesembolsos
        Else
            Total = 0
        End If

        Total = Total + FacturacionTotalServiciosSinIgicNewgolf()

        Total = Decimal.Round(CType(Total, Decimal), 2)

        If Total <> 0 Then
            Linea = 1
            Me.mTipoAsiento = "HABER"
            Me.mTotalFacturacion = Total

            Me.InsertaOracle("AC", 34, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "FACTURACION NEWGOLF " & Me.mFecha, Total, "SI", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, "FACTURACION NEWGOLF " & Me.mFecha, Total)
        End If

        ' FACTURACION ANULADA 
        SQL = "SELECT (SUM(FAIV_INCI) - SUM(FAIV_VIMP)) TOTAL "
        SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_FAIV," & Me.mParaUsuarioNewGolf & ".TNPL_FACT "
        SQL += "WHERE TNPL_FAIV.SEFA_CODI = TNPL_FACT.SEFA_CODI "
        SQL += "AND TNPL_FAIV.FACT_CODI = TNPL_FACT.FACT_CODI "
        SQL += "AND TNPL_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "
        SQL += "GROUP BY TNPL_FACT.FACT_DAAN"

        If IsDBNull(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = False Then
            Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)
            '+ Me.FacturacionSalidasServiciosSinIgic + Me.FacturacionSalidaDesembolsos
        Else
            Total = 0
        End If

        Total = Total + FacturacionTotalServiciosSinIgicNewgolfAnuladaS()

        Total = Decimal.Round(CType(Total, Decimal), 2) * -1

        If Total <> 0 Then
            Linea = Linea + 1
            Me.mTipoAsiento = "HABER"
            Me.mTotalFacturacion = Total

            Me.InsertaOracle("AC", 34, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "FACTURACION NEWGOLF ANULADA " & Me.mFecha, Total, "SI", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, "FACTURACION NEWGOLF ANULADA " & Me.mFecha, Total)
        End If


    End Sub


    Private Sub FacturasSalidaTotalFActuraGolf()
        Try
            Dim TotalFactura As Double
            Dim Dni As String = "0"
            Dim Cuenta As String = "0"
            Dim Titular As String

            SQL = "SELECT  TNPL_FACT.FACT_STAT AS ESTADO, TNPL_FACT.FACT_DAEM, TNPL_FACT.FACT_CODI AS NUMERO, NVL(TNPL_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNPL_FACT.FACT_CODI ||'/'|| TNPL_FACT.SEFA_CODI DESCRIPCION,TNPL_FACT.FACT_IMPO TOTAL,TNPL_FACT.FACT_VALO VALOR,NVL(ENTI_CODI,'') AS ENTI_CODI"
            'SQL += ",NVL(CCEX_CODI,'?') AS CCEX_CODI"
            SQL += " ,NVL(AGEN_CODI,'0') AS CLIENTE "
            SQL += " , NVL(TNPL_FACT.FACT_TITU,'') TITULAR,NVL(TNPL_FACT.FACT_NUCO,'') NIF "
            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_FACT "
            SQL += "WHERE "
            SQL += "(TNPL_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "
            SQL += "ORDER BY TNPL_FACT.SEFA_CODI ASC, TNPL_FACT.FACT_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)


            Me.NEWGOLF = New NewGolf.NewGolfData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)

                ' DETERMINAR EL TIPO DE FACTURA 
                ' FACTURA DE CONTADO NUEVO

                Cuenta = Me.NEWGOLF.DevuelveCuentaContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                Dni = Me.NEWGOLF.DevuelveDniCifContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))



                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)


                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("FV", 34, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Dni, Titular, "SI", "COBRO")
                Me.GeneraFileFV2("FV", 34, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalFactura, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String).PadRight(15, CChar(" ")), Cuenta, Dni)

            End While
            Me.DbLeeHotel.mDbLector.Close()

            ' FACTURAS ANULADAS 

            SQL = "SELECT  TNPL_FACT.FACT_STAT AS ESTADO, TNPL_FACT.FACT_DAEM, TNPL_FACT.FACT_CODI AS NUMERO, NVL(TNPL_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNPL_FACT.FACT_CODI ||'/'|| TNPL_FACT.SEFA_CODI DESCRIPCION,TNPL_FACT.FACT_IMPO TOTAL,TNPL_FACT.FACT_VALO VALOR,NVL(ENTI_CODI,'') AS ENTI_CODI"
            'SQL += ",NVL(CCEX_CODI,'?') AS CCEX_CODI"
            SQL += " ,NVL(AGEN_CODI,'0') AS CLIENTE "
            SQL += " , NVL(TNPL_FACT.FACT_TITU,'') TITULAR,NVL(TNPL_FACT.FACT_NUCO,'') NIF "
            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_FACT "
            SQL += "WHERE "
            SQL += "(TNPL_FACT.FACT_DAAN = " & "'" & Me.mFecha & "') "
            SQL += "ORDER BY TNPL_FACT.SEFA_CODI ASC, TNPL_FACT.FACT_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)

                ' DETERMINAR EL TIPO DE FACTURA 
                ' FACTURA DE CONTADO NUEVO

                Cuenta = Me.NEWGOLF.DevuelveCuentaContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                Dni = Me.NEWGOLF.DevuelveDniCifContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))


                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)


                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("FV", 34, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String) & " Anulada", TotalFactura, "NO", Dni, Titular, "SI", "COBRO ANULADO")
                Me.GeneraFileFV2("FV", 34, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalFactura, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String).PadRight(15, CChar(" ")), Cuenta, Dni)

            End While
            Me.DbLeeHotel.mDbLector.Close()
            Me.NEWGOLF.CerrarConexiones()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas")
            End If

        End Try
    End Sub
    Private Sub FacturasSalidaTotalFActuraGolfAX()
        Try
            Dim TotalFactura As Double
            Dim TotalLinea As Double
            Dim Dni As String = "0"
            Dim Cuenta As String = "0"
            Dim Titular As String
            Dim TipoFactura As Integer
            Dim CodigoCliente As String = ""

            Dim CodigoArticuloAx As String = ""


            Dim CodigoCompuesto As String

            Dim TipoVenta As String

            SQL = "SELECT  TNPL_FACT.FACT_STAT AS ESTADO, TNPL_FACT.FACT_DAEM, TNPL_FACT.FACT_CODI AS NUMERO, NVL(TNPL_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNPL_FACT.FACT_CODI ||'/'|| TNPL_FACT.SEFA_CODI DESCRIPCION,TNPL_FACT.FACT_IMPO TOTAL,TNPL_FACT.FACT_VALO VALOR,NVL(TNPL_FACT.ENTI_CODI,'') AS ENTI_CODI"
            'SQL += ",NVL(CCEX_CODI,'?') AS CCEX_CODI"
            SQL += " ,NVL(TNPL_FACT.AGEN_CODI,'0') AS CLIENTE "
            SQL += " , NVL(TNPL_FACT.FACT_TITU,'') TITULAR,NVL(TNPL_FACT.FACT_NUCO,'') NIF, "
            SQL += " TNPL_MOVI.MOVI_IMPT AS LINBRUT,TNPL_MOVI.MOVI_VLIQ AS LINBASE , TNPL_MOVI.MOVI_IMP1 AS LINIMPU, NVL(TNPL_TIVA.TIVA_PERC,'0') AS LINPORCEIGIC "
            'SQL += " , DECODE (TNPL_ADIC.ADIC_CODI,NULL,TNPL_RECU.RECU_CODI,TNPL_ADIC.ADIC_CODI) AS SERVICIO ,"
            SQL += " , DECODE (TNPL_ADIC.ADIC_CODI,NULL,NVL(TNPL_RECO.SERV_CODI,'?'),TNPL_ADIC.ADIC_CODI) AS SERVICIO ,"


            SQL += " DECODE (tnpl_movi.recu_codi,NULL, DECODE (tnpl_adic.adic_codi,NULL, tnpl_sspa.sspa_desc,tnpl_adic.adic_desc),tnpl_recu.recu_desc) AS CONCEPTO"

            ' NUEVA LINEA ABAJO FALTA DESARROLLO EN TODAS LAS LINEAS DE FACTURA 

            SQL += " ,DECODE(ADIC_TIPO,'3','BONOS','NORMAL') AS TIPOVENTA"

            SQL += ",NVL(TNPL_ADCO.SERV_CODI,'?') AS SERVICIODELARTICULO "

            SQL += " ,NVL(ADIC_CTS1,'?') AS CODIGOARTICULOAX "

            SQL += ",TNPL_FACT.FACT_NUCO AS FACT_NUCO ,NVL(TNPL_FACT.FACT_TITU,'?') AS FACT_TITU "

            SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_FACT, "
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FAMO, "
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_MOVI, "
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_TIVA, "
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADIC, "
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_SSPA, "
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RECU, "
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RECO, "

            SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADCO "

            SQL += " WHERE TNPL_FACT.FACT_CODI = TNPL_FAMO.FACT_CODI AND "
            SQL += "       TNPL_FACT.SEFA_CODI = TNPL_FAMO.SEFA_CODI AND "

            SQL += "     TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE"
            SQL += " AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI "

            SQL += " AND TNPL_MOVI.MOVI_IVA1 = TNPL_TIVA.TIVA_CODI(+)"


            SQL += " AND TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI(+)"
            SQL += " AND TNPL_MOVI.SSPA_CODI = TNPL_SSPA.SSPA_CODI(+)"
            SQL += " AND TNPL_MOVI.RECU_CODI = TNPL_RECU.RECU_CODI(+)"
            SQL += " AND TNPL_RECU.RECU_CODI = TNPL_RECO.RECU_CODI(+)"

            SQL += " AND TNPL_MOVI.ADIC_CODI = TNPL_ADCO.ADIC_CODI(+)"




            ' SOLO MOVIMIENTOS DE CREDITO 
            SQL += " AND MOVI_DECR = 1 "

            SQL += " AND TNPL_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
            SQL += "ORDER BY TNPL_FACT.SEFA_CODI ASC, TNPL_FACT.FACT_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)


            Me.NEWGOLF = New NewGolf.NewGolfData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)
                TotalLinea = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Decimal), 2)

                ' DETERMINAR EL TIPO DE FACTURA 
                ' FACTURA DE CONTADO NUEVO

                Cuenta = Me.NEWGOLF.DevuelveCuentaContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                Dni = Me.NEWGOLF.DevuelveDniCifContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                TipoFactura = Me.NEWGOLF.DevuelveTipoFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                CodigoCliente = Me.NEWGOLF.DevuelveCodigoEntiFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))

                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)


                If CType(Me.DbLeeHotel.mDbLector("SERVICIODELARTICULO"), String) = Me.mParaServCodiBonosAsoc Then
                    TipoVenta = "BONOS ASOC"
                Else
                    TipoVenta = CType(Me.DbLeeHotel.mDbLector("TIPOVENTA"), String)
                End If



                ' CONTROL CASO DE TENER LA FACTURA CODIGO DE ENTIDAD O NO ALOJADO Y EL USUARIO HABER CAMBIADO 
                ' EL NIF Y EL NOMBRE DEL TITULAR AL EMITIR LA FACTURA

                ' PREVALECE EL NIF PUESTO A MANO POR EL USUARIO 
                If IsDBNull(Me.DbLeeHotel.mDbLector("FACT_NUCO")) = False Then
                    If CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) <> Dni And Dni <> Me.mClientesContadoCif Then
                        If mMostrarMensajes = True Then
                            MsgBox("Atención Discrepancia de Nif " & vbCrLf & "Nif que Corresponde " & Dni & vbCrLf & "Nif hallado en Factura " & CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) & vbCrLf & vbCrLf & "Documento " & (CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String)), MsgBoxStyle.Exclamation, "Atención ")
                        Else
                            Me.mListBoxDebug.Items.Add("Atención Discrepancia de Nif " & vbCrLf & "Nif que Corresponde " & Dni & vbCrLf & "Nif hallado en Factura " & CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) & vbCrLf & vbCrLf & "Documento " & (CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String)))
                        End If
                        ' quito debajo por peticion de domingo solo avisamos de discrepancia 
                        'Dni = CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String)
                    End If
                End If

                ' si es un articulo de la tienda en vez de pasar el codigo del articulo paso su servicio de newhotel ( para saber si es bono o no ) 

                If Me.mParaIngresoPorHabitacion = 1 Then
                    If CType(Me.DbLeeHotel.mDbLector("SERVICIODELARTICULO"), String) = "?" Then
                        CodigoCompuesto = "000-" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String)
                    Else
                        CodigoCompuesto = "000-" & CType(Me.DbLeeHotel.mDbLector("SERVICIODELARTICULO"), String)
                    End If
                Else
                    CodigoCompuesto = Me.DbLeeHotel.mDbLector("SERVICIO")
                End If

                CodigoArticuloAx = CType(Me.DbLeeHotel.mDbLector("CODIGOARTICULOAX"), String)

                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("FV", 34, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalLinea, "NO", Dni, CType(Me.DbLeeHotel.mDbLector("CONCEPTO"), String), "SI", "COBRO", "SAT_NHCreateSalesOrdersQueryService", CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Double), CType(Me.DbLeeHotel.mDbLector("LINBASE"), Double), CType(Me.DbLeeHotel.mDbLector("LINIMPU"), Double), CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String), CType(Me.DbLeeHotel.mDbLector("LINPORCEIGIC"), Double), TipoFactura, CodigoCliente, TipoVenta, CodigoCompuesto, "NADA")
                Me.GeneraFileFV2("FV", 34, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalFactura, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String).PadRight(15, CChar(" ")), Cuenta, Dni)

            End While
            Me.DbLeeHotel.mDbLector.Close()

            ' FACTURAS ANULADAS 

            SQL = "SELECT  TNPL_FACT.FACT_STAT AS ESTADO, TNPL_FACT.FACT_DAEM, TNPL_FACT.FACT_CODI AS NUMERO, NVL(TNPL_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNPL_FACT.FACT_CODI ||'/'|| TNPL_FACT.SEFA_CODI DESCRIPCION,TNPL_FACT.FACT_IMPO TOTAL,TNPL_FACT.FACT_VALO VALOR,NVL(TNPL_FACT.ENTI_CODI,'') AS ENTI_CODI"
            'SQL += ",NVL(CCEX_CODI,'?') AS CCEX_CODI"
            SQL += " ,NVL(TNPL_FACT.AGEN_CODI,'0') AS CLIENTE "
            SQL += " , NVL(TNPL_FACT.FACT_TITU,'') TITULAR,NVL(TNPL_FACT.FACT_NUCO,'') NIF, "
            SQL += " TNPL_MOVI.MOVI_IMPT AS LINBRUT,TNPL_MOVI.MOVI_VLIQ AS LINBASE , TNPL_MOVI.MOVI_IMP1 AS LINIMPU, NVL(TNPL_TIVA.TIVA_PERC,'0') AS LINPORCEIGIC "
            ' SQL += " ,  DECODE (TNPL_ADIC.ADIC_CODI,NULL,TNPL_RECU.RECU_CODI,TNPL_ADIC.ADIC_CODI) AS SERVICIO ,"
            SQL += " , DECODE (TNPL_ADIC.ADIC_CODI,NULL,NVL(TNPL_RECO.SERV_CODI,'?'),TNPL_ADIC.ADIC_CODI) AS SERVICIO ,"


            SQL += " DECODE (tnpl_movi.recu_codi,NULL, DECODE (tnpl_adic.adic_codi,NULL, tnpl_sspa.sspa_desc,tnpl_adic.adic_desc),tnpl_recu.recu_desc) AS CONCEPTO"

            ' NUEVA LINEA ABAJO FALTA DESARROLLO EN TODAS LAS LINEAS DE FACTURA 

            SQL += " ,DECODE(ADIC_TIPO,'3','BONOS','NORMAL') AS TIPOVENTA"

            SQL += ",NVL(TNPL_ADCO.SERV_CODI,'?') AS SERVICIODELARTICULO "

            SQL += " ,NVL(ADIC_CTS1,'?') AS CODIGOARTICULOAX "

            SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_FACT, "
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FAMO, "
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_MOVI, "
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_TIVA, "

            SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADIC, "
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_SSPA, "
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RECU, "
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RECO, "
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADCO "

            SQL += " WHERE TNPL_FACT.FACT_CODI = TNPL_FAMO.FACT_CODI AND "
            SQL += "       TNPL_FACT.SEFA_CODI = TNPL_FAMO.SEFA_CODI AND "

            SQL += "     TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE"
            SQL += " AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI "

            SQL += " AND TNPL_MOVI.MOVI_IVA1 = TNPL_TIVA.TIVA_CODI(+)"

            SQL += " AND TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI(+)"
            SQL += " AND TNPL_MOVI.SSPA_CODI = TNPL_SSPA.SSPA_CODI(+)"
            SQL += " AND TNPL_MOVI.RECU_CODI = TNPL_RECU.RECU_CODI(+)"
            SQL += " AND TNPL_RECU.RECU_CODI = TNPL_RECO.RECU_CODI(+)"

            SQL += " AND TNPL_MOVI.ADIC_CODI = TNPL_ADCO.ADIC_CODI(+)"

            ' SOLO MOVIMIENTOS DE CREDITO 
            SQL += " AND MOVI_DECR = 1 "

            SQL += " AND TNPL_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "
            SQL += "ORDER BY TNPL_FACT.SEFA_CODI ASC, TNPL_FACT.FACT_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)
                TotalLinea = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Decimal), 2)


                ' DETERMINAR EL TIPO DE FACTURA 
                ' FACTURA DE CONTADO NUEVO

                Cuenta = Me.NEWGOLF.DevuelveCuentaContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                Dni = Me.NEWGOLF.DevuelveDniCifContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                TipoFactura = Me.NEWGOLF.DevuelveTipoFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))
                CodigoCliente = Me.NEWGOLF.DevuelveCodigoEntiFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))

                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)


                If CType(Me.DbLeeHotel.mDbLector("SERVICIODELARTICULO"), String) = Me.mParaServCodiBonosAsoc Then
                    TipoVenta = "BONOS ASOC"
                Else
                    TipoVenta = CType(Me.DbLeeHotel.mDbLector("TIPOVENTA"), String)
                End If



                ' si es un articulo de la tienda en vez de pasar el codigo del articulo paso su servicio de newhotel ( para saber si es bono o no ) 

                If Me.mParaIngresoPorHabitacion = 1 Then
                    If CType(Me.DbLeeHotel.mDbLector("SERVICIODELARTICULO"), String) = "?" Then
                        CodigoCompuesto = "000-" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String)
                    Else
                        CodigoCompuesto = "000-" & CType(Me.DbLeeHotel.mDbLector("SERVICIODELARTICULO"), String)
                    End If
                Else
                    CodigoCompuesto = Me.DbLeeHotel.mDbLector("SERVICIO")
                End If

                CodigoArticuloAx = CType(Me.DbLeeHotel.mDbLector("CODIGOARTICULOAX"), String)

                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("FV", 34, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String) & " Anulada", TotalLinea, "NO", Dni, CType(Me.DbLeeHotel.mDbLector("CONCEPTO"), String), "SI", "COBRO ANULADO", "SAT_NHCreateSalesOrdersQueryService", (CType(Me.DbLeeHotel.mDbLector("LINBRUT"), Double) * -1), (CType(Me.DbLeeHotel.mDbLector("LINBASE"), Double) * -1), (CType(Me.DbLeeHotel.mDbLector("LINIMPU"), Double) * -1), CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), (TotalFactura * -1), CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String), CType(Me.DbLeeHotel.mDbLector("LINPORCEIGIC"), Double), TipoFactura, CodigoCliente, TipoVenta, CodigoCompuesto, "NADA")
                Me.GeneraFileFV2("FV", 34, Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), TotalFactura, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String).PadRight(15, CChar(" ")), Cuenta, Dni)

            End While
            Me.DbLeeHotel.mDbLector.Close()
            Me.NEWGOLF.CerrarConexiones()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas")
            End If

        End Try
    End Sub

    Private Sub FacturasSalidaIgicAgrupadoGolf()

        Try

            Dim TotalIva As Double
            Dim TotalBase As Double

            Dim DescripcionAsiento As String = "I.G.I.C FACTURACION " & Me.mFecha
            Dim Cuenta As String

            SQL = "SELECT "
            SQL += "TNPL_FAIV.FAIV_TAXA AS TIPO,SUM((FAIV_INCI-FAIV_VIMP)) BASE, SUM(TNPL_FAIV.FAIV_VIMP) IGIC,'"
            SQL += Me.mParaTextoIva & " ' || FAIV_TAXA ||'%  ' "
            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_FAIV," & Me.mParaUsuarioNewGolf & ".TNPL_FACT  "
            SQL += "WHERE TNPL_FAIV.SEFA_CODI = TNPL_FACT.SEFA_CODI "
            SQL += "AND TNPL_FAIV.FACT_CODI = TNPL_FACT.FACT_CODI "
            SQL += "AND (TNPL_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "
            SQL += "GROUP BY TNPL_FAIV.FAIV_TAXA"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
                TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)


                TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
                TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

                SQL = "SELECT NVL(TIVA_CTB1,'0') FROM " & StrConexionExtraeUsuario(mStrConexionHotel) & ".TNHT_TIVA WHERE TIVA_PERC = " & CType(Me.DbLeeHotel.mDbLector("TIPO"), Integer)
                Cuenta = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 34, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, DescripcionAsiento, TotalIva, "NO", Me.mClientesContadoCif, "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, DescripcionAsiento, TotalIva)

                Me.GeneraFileIV("IV", 34, Me.mEmpGrupoCod, Me.mEmpCod, "SERIE", 0, TotalBase, CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), TotalIva, "X")


            End While
            Me.DbLeeHotel.mDbLector.Close()


            ' FACTURAS ANULADAS

            DescripcionAsiento = "I.G.I.C FACTURACION ANULADO" & Me.mFecha

            SQL = "SELECT "
            SQL += "TNPL_FAIV.FAIV_TAXA AS TIPO,SUM((FAIV_INCI-FAIV_VIMP)) BASE, SUM(TNPL_FAIV.FAIV_VIMP) IGIC,'"
            SQL += Me.mParaTextoIva & " ' || FAIV_TAXA ||'%  ' "
            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_FAIV," & Me.mParaUsuarioNewGolf & ".TNPL_FACT  "
            SQL += "WHERE TNPL_FAIV.SEFA_CODI = TNPL_FACT.SEFA_CODI "
            SQL += "AND TNPL_FAIV.FACT_CODI = TNPL_FACT.FACT_CODI "
            SQL += "AND (TNPL_FACT.FACT_DAAN = " & "'" & Me.mFecha & "') "
            SQL += "GROUP BY TNPL_FAIV.FAIV_TAXA"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
                TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)


                TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
                TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

                SQL = "SELECT NVL(TIVA_CTB1,'0') FROM " & StrConexionExtraeUsuario(mStrConexionHotel) & ".TNHT_TIVA WHERE TIVA_PERC = " & CType(Me.DbLeeHotel.mDbLector("TIPO"), Integer)
                Cuenta = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 34, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, DescripcionAsiento, TotalIva, "NO", Me.mClientesContadoCif, "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, DescripcionAsiento, TotalIva)
                Me.GeneraFileIV("IV", 34, Me.mEmpGrupoCod, Me.mEmpCod, "SERIE", 0, TotalBase, CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), TotalIva, "X")


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Detalle de Impuesto")
            End If

        End Try
    End Sub


#End Region

#Region "ASIENTO-35 FACTURACION DE CONTADO"
    Private Sub FacturasContadoTotal()

        Dim Total As Double

        Dim TotalComisiones As Double
        Dim SQL As String



        SQL = "SELECT MOVI_VDEB AS TOTAL  FROM VNHT_MOVH TNHT_MOVI , TNHT_FACT,TNHT_FAMO "
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
        'SQL += " AND TNHT_MOVI.MOVI_VDEB > 0  "
        ' NUEVO POR AXAPTA 
        SQL += " AND TNHT_MOVI.MOVI_VDEB <> 0  "



        Me.DbLeeHotel.TraerLector(SQL)


        Total = 0
        While Me.DbLeeHotel.mDbLector.Read

            Total = Total + Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

        End While
        Me.DbLeeHotel.mDbLector.Close()


        SQL = "SELECT (MOVI_VDEB * -1) AS TOTAL  FROM VNHT_MOVH TNHT_MOVI , TNHT_FACT,TNHT_FAMO "
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
        'SQL += " AND TNHT_MOVI.MOVI_VDEB > 0  "
        ' NUEVO POR AXAPTA 
        SQL += " AND TNHT_MOVI.MOVI_VDEB <> 0  "



        Me.DbLeeHotel.TraerLector(SQL)



        While Me.DbLeeHotel.mDbLector.Read

            Total = Total + Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

        End While
        Me.DbLeeHotel.mDbLector.Close()



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
            Total = Total + TotalComisiones
        End If
        Total = Decimal.Round(CType(Total, Decimal), 2)


        ' Total = Total + FacturacionContadoServiciosSinIgic()

        'MsgBox(FacturacionContadoServiciosSinIgic)



        If Total <> 0 Then
            Linea = 1
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaClientesContado, Me.mIndicadorHaber, "COBROS FACTURACION " & Me.mFecha, Total, "SI", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaClientesContado, Me.mIndicadorHaber, "COBROS FACTURACION " & Me.mFecha, Total)
        End If

    End Sub


    Private Sub FacturasContadoTotalVisas()
        Dim Total As Double
        Dim Descripcion As String
        SQL = "SELECT MOVI_VDEB TOTAL,CACR_DESC TARJETA,nvl(CACR_CTBA,'0') CUENTA,"
        SQL += " TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI,NVL(TNHT_FACT.FACT_TITU,' ') AS TITULAR,NVL(FAAN_CODI,'0') AS FAAN_CODI  "
        SQL += " FROM VNHT_MOVH TNHT_MOVI,TNHT_CACR,TNHT_FACT,TNHT_FAMO WHERE"

        SQL = SQL & " TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & "  AND (TNHT_MOVI.MOVI_AUTO = '1' OR TNHT_MOVI.MOVI_AUTO = '0' AND TNHT_MOVI.CCEX_CODI = 'TPV')"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"

        ' NUEVO PARA QUE NO TRATE LAS DEVOLUCIONES SI YA SE TRATAN EN UN ASIENTO PROPIO 20090219
        'SQL += " AND TNHT_MOVI.MOVI_VDEB > 0  "
        ' NUEVO POR AXAPTA 
        SQL += " AND TNHT_MOVI.MOVI_VDEB <> 0  "

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
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion, Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion, Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()

        Exit Sub
        ' *************** VISAS anulado de dias anteriores 
        SQL = ""
        SQL = "SELECT MOVI_VDEB TOTAL,CACR_DESC TARJETA,nvl(CACR_CTBA,'0') CUENTA,"
        SQL += " TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI,NVL(TNHT_FACT.FACT_TITU,' ') AS TITULAR,NVL(FAAN_CODI,'0') AS FAAN_CODI  FROM VNHT_MOVH TNHT_MOVI,TNHT_CACR,TNHT_FACT,TNHT_FAMO WHERE"

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
        'SQL += " AND TNHT_MOVI.MOVI_VDEB > 0  "
        ' NUEVO POR AXAPTA 
        SQL += " AND TNHT_MOVI.MOVI_VDEB <> 0  "

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
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion & " Deducido", Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion & " Deducido", Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasContadoTotalVisasAX()
        Dim Total As Double
        Dim Descripcion As String

        Dim Status As Integer


        Dim TipoCliente As Integer
        Dim Nif As String

        Dim FormaCobro As String = ""
        Dim Factura As String

        Dim Nmovi(1) As String

        SQL = "SELECT MOVI_VDEB TOTAL,CACR_DESC TARJETA,nvl(CACR_CTBA,'0') CUENTA,"
        SQL += " TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI,NVL(TNHT_FACT.FACT_TITU,' ') AS TITULAR,NVL(FAAN_CODI,'0') AS FAAN_CODI,TNHT_CACR.CACR_CODI AS TIPOCOBRO,MOVI_DAVA "

        If Me.mDebug = True Then
            SQL += " , TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA "
            SQL = SQL & " ,NVL(TNHT_MOVI.RESE_CODI ,0) AS RESE_CODI,NVL(TNHT_MOVI.RESE_ANCI ,0) AS RESE_ANCI,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_CODI ,'?') AS CCEX_CODI,NVL(TNHT_ENTI.ENTI_CODI ,'?') AS ENTI_CODI,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_NUCO ,'?') AS CCEX_NUCO,NVL(TNHT_ENTI.ENTI_NUCO ,'?') AS ENTI_NUCO,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_TITU ,'?') AS CCEX_TITU,NVL(TNHT_ENTI.ENTI_NOME ,'?') AS ENTI_NOME,"
            SQL = SQL & " NVL(TNHT_ENTI.ENTI_FAMA ,'?') AS ENTI_FAMA "
            SQL = SQL & " ,TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI,TNHT_FACT.FACT_HOEM,TNHT_FACT.FAAN_CODI "
            SQL = SQL & " ,NVL(TNHT_RESE.RESE_ANPH,'?') CLIENTE "

        End If

        SQL += "  ,TNHT_MOVI.MOVI_CODI AS MOVI_CODI "


        SQL += "  , NVL(TNHT_SECC.SECC_DESC,'?')  AS SECCION "

        SQL += " FROM VNHT_MOVH TNHT_MOVI,TNHT_CACR,TNHT_FACT,TNHT_FAMO "

        SQL += " ,TNHT_SECC "

        If Me.mDebug = True Then
            SQL += " ,TNHT_CCEX,TNHT_ENTI,TNHT_RESE "
        End If
        SQL += "  WHERE"

        SQL = SQL & " TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        SQL += " AND TNHT_MOVI.SECC_CODI = TNHT_SECC.SECC_CODI(+)"

        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = '1'"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL = SQL & "  AND (TNHT_MOVI.MOVI_AUTO = '1' OR TNHT_MOVI.MOVI_AUTO = '0' AND TNHT_MOVI.CCEX_CODI = 'TPV')"
        SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"


        If Me.mDebug = True Then
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) AND "
            SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "

            SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"

        End If

        ' NUEVO PARA QUE NO TRATE LAS DEVOLUCIONES SI YA SE TRATAN EN UN ASIENTO PROPIO 20090219
        'SQL += " AND TNHT_MOVI.MOVI_VDEB > 0  "
        ' NUEVO POR AXAPTA 
        SQL += " AND TNHT_MOVI.MOVI_VDEB <> 0  "


        SQL = SQL & " ORDER BY TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read


            If CType(Me.DbLeeHotel.mDbLector("FAAN_CODI"), Integer) = 0 Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            End If

            Factura = CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String)


            ' TRATAMIENTO DE DEVOLUCIONES 
            ' Si el cobro ya esta enviado en el asiento de anticipos como negativo NO se Envia 

            SQL = "SELECT COUNT(*) AS TOTAL FROM TH_ASNT WHERE "
            SQL += " ASNT_WEBSERVICE_NAME = 'SAT_NHPrePaymentService'"
            SQL += " AND ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_FACTURA_NUMERO = '" & DbLeeHotel.mDbLector.Item("FACT_CODI") & "'"
            SQL += " AND ASNT_FACTURA_SERIE = '" & DbLeeHotel.mDbLector.Item("SEFA_CODI") & "'"
            SQL += " AND ASNT_MOVI_CODI = " & DbLeeHotel.mDbLector.Item("MOVI_CODI")
            ' PAT0024  atencion solo si es una devolucion
            SQL += " AND ASNT_DEVOLUCION = 'DEVOLUCION' "

            If CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Integer) > 0 Then
                '   MsgBox("SE ENCONTRO  " & DbLeeHotel.mDbLector.Item("TOTAL"))
                ' poner status a 9 para no enviar este cobro 
                Status = 9

            Else
                Status = 0
            End If


            ' si la factura ademas de cobros contiene anticipos SE ENVIA UN COBRO Y UN ANTICIPO 
            If Me.mDebug = True Then


                If SaberSiFacturaContieneAnticipos(CStr(DbLeeHotel.mDbLector.Item("SEFA_CODI")), CInt(DbLeeHotel.mDbLector.Item("FACT_CODI"))) = True Then
                    ' SE CARGAN ALGUNOS DATOS PARA SIMULAR ANTICIPO
                    FormaCobro = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)

                    If CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), String) = "0" Then
                        ' ES UN NO ALOJADO
                        TipoCliente = 2
                        Nif = CType(Me.DbLeeHotel.mDbLector("CCEX_NUCO"), String)
                        Descripcion = "No Alo : [" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CCEX_TITU"), String)
                    Else
                        ' NO ES UN NO ALOJADO
                        ' ES CLIENTE DIRECTO
                        If CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) = "?" Then
                            TipoCliente = 3
                            Nif = Me.mClientesContadoCif
                            Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String)
                        Else ' ES CLIENTE DE AGENCIA
                            If CType(Me.DbLeeHotel.mDbLector("ENTI_FAMA"), String) = "1" Then
                                ' TTOO NORMAL
                                TipoCliente = 1
                                Nif = CType(Me.DbLeeHotel.mDbLector("ENTI_NUCO"), String)
                                Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                            Else
                                ' TTOO ON LINE ( EL ANTICIPO LE LO CARGAMOS ACLIENTES CONTADO
                                TipoCliente = 3
                                Nif = Me.mClientesContadoCif
                                Descripcion = "[* TTOO WEB ] Rva : " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                            End If
                        End If
                    End If


                    Nmovi = SaberSiFacturaContieneAnticiposDevuelveMovimiento(CStr(DbLeeHotel.mDbLector.Item("SEFA_CODI")), CInt(DbLeeHotel.mDbLector.Item("FACT_CODI")))
                    Me.mSufijoBookId = Me.DamePrefijoBookId(CInt(Nmovi(0)), CDate(Nmovi(1)), CStr(DbLeeHotel.mDbLector.Item("SEFA_CODI")), CInt(DbLeeHotel.mDbLector.Item("FACT_CODI")), CDate(DbLeeHotel.mDbLector.Item("FACT_HOEM")), CStr(DbLeeHotel.mDbLector.Item("FAAN_CODI")))


                    If Status <> 9 Then
                        ' se envia anticipo  si no es una devolucion

                        Linea = Linea + 1
                        Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, Me.mSufijoBookId & " " & Descripcion.Replace("'", "''"), Total, "NO", Nif, "(Cobro como Anticipo) " & Factura & " F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "ANTICIPO", "SAT_NHPrePaymentService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, "0", "NADA", "NADA", "NADA", CType(DbLeeHotel.mDbLector.Item("FACT_CODI"), String), CType(DbLeeHotel.mDbLector.Item("SEFA_CODI"), String), Me.mSufijoBookId)


                        ' se envia el cobro  si no es una devolucion

                        Linea = Linea + 1
                        Me.mTipoAsiento = "DEBE"
                        Me.InsertaOracle2("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(Cobro como Anticipo) " & Factura & " ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "0", Me.mSufijoBookId, CStr(Me.DbLeeHotel.mDbLector("SECCION")))


                        ' 20111111 PARA QUE NO SE  DUPLIQUE EL COBRO "ABAJO" 
                        Status = 9
                    End If

             
                End If
            End If

            If Status <> 9 Then
                Linea = Linea + 1
                ' SI LA FACTURA NO CONTIENE ANTICIPOS EL COBRO SE HACE NORMAL ( NO CONVERTIDO A ANTICIPO ) STATUS = 0
                ' SI ES UNA DEVOLUCION Y SA SE FORZO COMO ANTICIPO STATUS = 9 ( NO SE HACE EL COBRO ) 
                Descripcion = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle2("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion, Total, "NO", "", "", "SI", "", "SAT_NHJournalCustPaymentQueryService", "", CType(Me.DbLeeHotel.mDbLector("TIPOCOBRO"), String), "", CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String), "", CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String), CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String), Status, CStr(Me.DbLeeHotel.mDbLector("SECCION")))

                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion, Total)

            End If

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub FacturasContadoTotaLOtrasFormas()
        Dim Total As Double
        Dim SQL As String


        SQL = ""
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,NVL(FAAN_CODI,'0') AS FAAN_CODI  FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_FACT,TNHT_FAMO WHERE"

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
        'SQL += " AND TNHT_MOVI.MOVI_VDEB > 0  "
        ' NUEVO POR AXAPTA 
        SQL += " AND TNHT_MOVI.MOVI_VDEB <> 0  "


        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1,FAAN_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            If CType(Me.DbLeeHotel.mDbLector("FAAN_CODI"), Integer) = 0 Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            End If



            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()

        Exit Sub

        ' *************** contado anulado de dias anteriores 
        SQL = ""
        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,NVL(FAAN_CODI,'0') AS FAAN_CODI  FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_FACT,TNHT_FAMO WHERE"
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
        'SQL += " AND TNHT_MOVI.MOVI_VDEB > 0  "
        ' NUEVO POR AXAPTA 
        SQL += " AND TNHT_MOVI.MOVI_VDEB <> 0  "


        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1,FAAN_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            If CType(Me.DbLeeHotel.mDbLector("FAAN_CODI"), Integer) = 0 Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            End If



            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " Deducido", Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " Deducido", Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()


    End Sub
    Private Sub FacturasContadoTotaLOtrasFormasAX()
        Dim Total As Double
        Dim SQL As String

        Dim Descripcion As String
        Dim Status As Integer




        Dim TipoCliente As Integer
        Dim Nif As String

        Dim FormaCobro As String = ""
        Dim Factura As String

        Dim Nmovi(1) As String


        SQL = ""
        SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,NVL(FAAN_CODI,'0') AS FAAN_CODI, "
        SQL += " TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI , NVL(TNHT_FACT.FACT_TITU,'?') AS TITULAR,TNHT_FORE.FORE_CODI AS TIPOCOBRO,MOVI_DAVA  "

        If Me.mDebug = True Then
            SQL += " , TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA "
            SQL = SQL & " ,NVL(TNHT_MOVI.RESE_CODI ,0) AS RESE_CODI,NVL(TNHT_MOVI.RESE_ANCI ,0) AS RESE_ANCI,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_CODI ,'?') AS CCEX_CODI,NVL(TNHT_ENTI.ENTI_CODI ,'?') AS ENTI_CODI,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_NUCO ,'?') AS CCEX_NUCO,NVL(TNHT_ENTI.ENTI_NUCO ,'?') AS ENTI_NUCO,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_TITU ,'?') AS CCEX_TITU,NVL(TNHT_ENTI.ENTI_NOME ,'?') AS ENTI_NOME,"
            SQL = SQL & " NVL(TNHT_ENTI.ENTI_FAMA ,'?') AS ENTI_FAMA "
            SQL = SQL & " ,TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI,TNHT_FACT.FACT_HOEM,TNHT_FACT.FAAN_CODI "
            SQL = SQL & " ,NVL(TNHT_RESE.RESE_ANPH,'?') CLIENTE "

        End If

        SQL += "  ,TNHT_MOVI.MOVI_CODI AS MOVI_CODI "

        SQL += "  , NVL(TNHT_SECC.SECC_DESC,'?')  AS SECCION "

        SQL += " FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_FACT,TNHT_FAMO "

        If Me.mDebug = True Then
            SQL += " ,TNHT_CCEX,TNHT_ENTI,TNHT_RESE "
        End If

        SQL += " ,TNHT_SECC "

        SQL += " WHERE "

        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "


        SQL = SQL & " AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"


        SQL += " AND TNHT_MOVI.SECC_CODI = TNHT_SECC.SECC_CODI(+)"

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



        If Me.mDebug = True Then
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) AND "
            SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "

            SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"

        End If

        ' NUEVO PARA QUE NO TRATE LAS DEVOLUCIONES SI YA SE TRATAN EN UN ASIENTO PROPIO 20090219
        'SQL += " AND TNHT_MOVI.MOVI_VDEB > 0  "
        ' NUEVO POR AXAPTA 
        SQL += " AND TNHT_MOVI.MOVI_VDEB <> 0  "


        'SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1,FAAN_CODI"
        SQL = SQL & " ORDER BY TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read


            If CType(Me.DbLeeHotel.mDbLector("FAAN_CODI"), Integer) = 0 Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            End If

            Factura = CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String)




            ' TRATAMIENTO DE DEVOLUCIONES 
            ' Si el cobro ya esta enviado en el asiento de anticipos como negativo NO se Envia 
            SQL = "SELECT COUNT(*) AS TOTAL FROM TH_ASNT WHERE "
            SQL += " ASNT_WEBSERVICE_NAME = 'SAT_NHPrePaymentService'"
            SQL += " AND ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_FACTURA_NUMERO = '" & DbLeeHotel.mDbLector.Item("FACT_CODI") & "'"
            SQL += " AND ASNT_FACTURA_SERIE = '" & DbLeeHotel.mDbLector.Item("SEFA_CODI") & "'"
            SQL += " AND ASNT_MOVI_CODI = " & DbLeeHotel.mDbLector.Item("MOVI_CODI")
            ' PAT0024  atencion solo si es una devolucion
            SQL += " AND ASNT_DEVOLUCION = 'DEVOLUCION' "



            If CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Integer) > 0 Then
                '    MsgBox("SE ENCONTRO  " & DbLeeHotel.mDbLector.Item("TOTAL"))
                ' poner status a 9 para no enviar este cobro 
                Status = 9
            Else
                Status = 0
            End If


            ' si la factura ademas de cobros contiene anticipos se envia un cobro y un anticipo

            ' 20111111
            ' PREGUNTAR SI ES COMO ARRIBA DICE O SE ENVIA EL COBRO Y ADEMASS UN ANTICIPO
            If Me.mDebug = True Then


                If SaberSiFacturaContieneAnticipos(CStr(DbLeeHotel.mDbLector.Item("SEFA_CODI")), CInt(DbLeeHotel.mDbLector.Item("FACT_CODI"))) = True Then
                    ' SE CARGAN ALGUNOS DATOS PARA SIMULAR ANTICIPO
                    FormaCobro = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)

                    If CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), String) = "0" Then
                        ' ES UN NO ALOJADO
                        TipoCliente = 2
                        Nif = CType(Me.DbLeeHotel.mDbLector("CCEX_NUCO"), String)
                        Descripcion = "No Alo : [" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CCEX_TITU"), String)
                    Else
                        ' NO ES UN NO ALOJADO
                        ' ES CLIENTE DIRECTO
                        If CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) = "?" Then
                            TipoCliente = 3
                            Nif = Me.mClientesContadoCif
                            Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String)
                        Else ' ES CLIENTE DE AGENCIA
                            If CType(Me.DbLeeHotel.mDbLector("ENTI_FAMA"), String) = "1" Then
                                ' TTOO NORMAL
                                TipoCliente = 1
                                Nif = CType(Me.DbLeeHotel.mDbLector("ENTI_NUCO"), String)
                                Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                            Else
                                ' TTOO ON LINE ( EL ANTICIPO LE LO CARGAMOS ACLIENTES CONTADO
                                TipoCliente = 3
                                Nif = Me.mClientesContadoCif
                                Descripcion = "[* TTOO WEB ] Rva : " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                            End If
                        End If
                    End If

                    Nmovi = SaberSiFacturaContieneAnticiposDevuelveMovimiento(CStr(DbLeeHotel.mDbLector.Item("SEFA_CODI")), CInt(DbLeeHotel.mDbLector.Item("FACT_CODI")))
                    Me.mSufijoBookId = Me.DamePrefijoBookId(CInt(Nmovi(0)), CDate(Nmovi(1)), CStr(DbLeeHotel.mDbLector.Item("SEFA_CODI")), CInt(DbLeeHotel.mDbLector.Item("FACT_CODI")), CDate(DbLeeHotel.mDbLector.Item("FACT_HOEM")), CStr(DbLeeHotel.mDbLector.Item("FAAN_CODI")))


                    If Status <> 9 Then
                        ' se envia anticipo si no es una devolucion
                        Linea = Linea + 1
                        Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, Me.mSufijoBookId & " " & Descripcion.Replace("'", "''"), Total, "NO", Nif, "(Cobro como Anticipo) " & Factura & " F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "ANTICIPO", "SAT_NHPrePaymentService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, "0", "NADA", "NADA", "NADA", CType(DbLeeHotel.mDbLector.Item("FACT_CODI"), String), CType(DbLeeHotel.mDbLector.Item("SEFA_CODI"), String), Me.mSufijoBookId)


                        ' se envia el cobro  si no es una devolucion
                        ' 20111111
                        Linea = Linea + 1
                        Me.mTipoAsiento = "DEBE"
                        Me.InsertaOracle2("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(Cobro como Anticipo) " & Factura & " ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "0", Me.mSufijoBookId, CStr(Me.DbLeeHotel.mDbLector("SECCION")))


                        ' 20111111 PARA QUE NO SE  DUPLIQUE EL COBRO "ABAJO" 
                        Status = 9

                    End If


                End If
            End If



            If Status <> 9 Then
                Linea = Linea + 1

                ' SI LA FACTURA NO CONTIENE ANTICIPOS EL COBRO SE HACE NORMAL ( NO CONVERTIDO A ANTICIPO ) STATUS = 0
                ' SI ES UNA DEVOLUCION Y SA SE FORZO COMO ANTICIPO STATUS = 9 ( NO SE HACE EL COBRO ) 
                Descripcion = CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle2("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion, Total, "NO", "", "", "SI", "", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("TIPOCOBRO"), String), "", "", CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String), "", CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String), CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String), Status, CStr(Me.DbLeeHotel.mDbLector("SECCION")))
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion, Total)

            End If


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
        SQL = SQL & "TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA,TNHT_FACT.FACT_DAEM,TNHT_MOVI.MOVI_DATR,TNHT_FACT.FAAN_CODI,TNHT_FACT.ENTI_CODI "
        SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO"

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
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String) & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI", "ANTICIPO FACTURADO")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)

            Linea = Linea + 1
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String) & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)




        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub FacturasContadoCancelaciondeAnticiposAx()
        Dim Total As Double
        '    Dim TotalCancelados As Double

        Dim SQL As String

        Dim Descripcion As String = ""
        Dim TipoCliente As Integer
        Dim Nif As String
        Dim FormaCobro As String = ""


        'SQL = "SELECT 'Anticipo RVA= ' ||TNHT_MOVI.RESE_CODI||'/'||TNHT_MOVI.RESE_ANCI RESERVA,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA, "
        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA, "

        SQL += "TNHT_MOVI.MOVI_VDEB TOTAL,NVL(TNHT_RESE.RESE_ANPH,'?') CLIENTE,"
        SQL += " TNHT_FACT.FACT_CODI AS NUMERO ,TNHT_FACT.SEFA_CODI SERIE,"
        SQL = SQL & "TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA,TNHT_FACT.FACT_DAEM,TNHT_MOVI.MOVI_DATR,TNHT_FACT.FAAN_CODI,TNHT_FACT.ENTI_CODI AS ENTIFACT "

        ' N
        SQL = SQL & " ,NVL(TNHT_RESE.RESE_CODI ,0) AS RESE_CODI,NVL(TNHT_RESE.RESE_ANCI ,0) AS RESE_ANCI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_CODI ,'?') AS CCEX_CODI,NVL(TNHT_ENTI.ENTI_CODI ,'?') AS ENTI_CODI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_NUCO ,'?') AS CCEX_NUCO,NVL(TNHT_ENTI.ENTI_NUCO ,'?') AS ENTI_NUCO,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_TITU ,'?') AS CCEX_TITU,NVL(TNHT_ENTI.ENTI_NOME ,'?') AS ENTI_NOME,"
        SQL = SQL & " NVL(TNHT_ENTI.ENTI_FAMA ,'?') AS ENTI_FAMA,"
        SQL = SQL & " NVL(TNHT_CACR.CACR_CTBA,'0') TARJETA,"
        SQL = SQL & " NVL(TNHT_FORE.FORE_CTB1,'0') FORMA"

        ' SQL = SQL & ",NVL(MOVI_NUDO,'?') AS RECIBO"
        SQL = SQL & ",TNHT_MOVI.MOVI_DARE || TNHT_MOVI.MOVI_CODI AS RECIBO "

        SQL = SQL & ",TNHT_FACT.FACT_NUCO AS FACT_NUCO ,NVL(TNHT_FACT.FACT_TITU,'?') AS FACT_TITU "


        SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO"
        ' N
        SQL = SQL & " ,TNHT_FORE,TNHT_CACR,TNHT_ENTI,TNHT_CCEX "

        '
        SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        'N
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI(+)"
        SQL = SQL & " AND TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+)"
        SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"


        ' NUEVO POR AJUSTE DE RENDIMIENTO 
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) AND "
        SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) AND"


        SQL = SQL & " TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '1'"
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

            If CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), String) = "0" Then
                ' ES UN NO ALOJADO
                TipoCliente = 2
                Nif = CType(Me.DbLeeHotel.mDbLector("CCEX_NUCO"), String)
                Descripcion = "No Alo : [" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CCEX_TITU"), String)
            Else
                ' NO ES UN NO ALOJADO
                ' ES CLIENTE DIRECTO
                If CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) = "?" Then
                    TipoCliente = 3
                    Nif = Me.mClientesContadoCif
                    Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String)
                Else ' ES CLIENTE DE AGENCIA
                    If CType(Me.DbLeeHotel.mDbLector("ENTI_FAMA"), String) = "1" Then
                        ' TTOO NORMAL
                        TipoCliente = 1
                        Nif = CType(Me.DbLeeHotel.mDbLector("ENTI_NUCO"), String)
                        Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    Else
                        ' TTOO ON LINE ( EL ANTICIPO LE LO CARGAMOS ACLIENTES CONTADO
                        TipoCliente = 3
                        Nif = Me.mClientesContadoCif
                        Descripcion = "[* TTOO WEB ] Rva : " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    End If
                End If
            End If

            ' CONTROL CASO DE TENER LA FACTURA CODIGO DE ENTIDAD O NO ALOJADO Y EL USUARIO HABER CAMBIADO 
            ' EL NIF Y EL NOMBRE DEL TITULAR AL EMITIR LA FACTURA

            ' PREVALECE EL NIF PUESTO A MANO POR EL USUARIO 
            If IsDBNull(Me.DbLeeHotel.mDbLector("FACT_NUCO")) = False Then
                If CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) <> Nif And Nif <> Me.mClientesContadoCif Then
                    If mMostrarMensajes = True Then
                        MsgBox("Atención Discrepancia de Nif " & vbCrLf & "Nif que Corresponde " & Nif & vbCrLf & "Nif hallado en Factura " & CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) & vbCrLf & vbCrLf & "Documento " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), MsgBoxStyle.Exclamation, "Atención ")
                    Else
                        Me.mListBoxDebug.Items.Add("Atención Discrepancia de Nif " & vbCrLf & "Nif que Corresponde " & Nif & vbCrLf & "Nif hallado en Factura " & CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) & vbCrLf & vbCrLf & "Documento " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String))
                    End If

                    ' quito debajo por peticion de domingo solo avisamos de discrepancia 
                    'Nif = CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String)
                    Descripcion += " (*) " & CType(Me.DbLeeHotel.mDbLector("FACT_TITU"), String)
                End If
            End If



            If CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) = "0" Then
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("FORMA"), String)
            Else
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String)
            End If

            Me.mCancelacionAnticipos = Me.mCancelacionAnticipos + Total
            Me.mTipoAsiento = "DEBE"
            '      Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String) & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", "", "", "", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), "")
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String))


            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)

            Linea = Linea + 1

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub FacturasContadoCancelaciondeAnticiposAxNuevo()
        Dim Total As Double
        '    Dim TotalCancelados As Double

        Dim SQL As String

        Dim Descripcion As String = ""
        Dim TipoCliente As Integer
        Dim Nif As String
        Dim FormaCobro As String = ""

        Dim Factura As String


        Linea = 6000
       

        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA, "

        SQL += "TNHT_MOVI.MOVI_VDEB TOTAL,NVL(TNHT_RESE.RESE_ANPH,'?') CLIENTE,"
        SQL += " TNHT_FACT.FACT_CODI AS NUMERO ,TNHT_FACT.SEFA_CODI SERIE,"
        SQL = SQL & "TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA,TNHT_FACT.FACT_DAEM,TNHT_MOVI.MOVI_DATR,TNHT_FACT.FAAN_CODI,TNHT_FACT.ENTI_CODI AS ENTIFACT "

        ' N
        SQL = SQL & " ,NVL(TNHT_RESE.RESE_CODI ,0) AS RESE_CODI,NVL(TNHT_RESE.RESE_ANCI ,0) AS RESE_ANCI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_CODI ,'?') AS CCEX_CODI,NVL(TNHT_ENTI.ENTI_CODI ,'?') AS ENTI_CODI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_NUCO ,'?') AS CCEX_NUCO,NVL(TNHT_ENTI.ENTI_NUCO ,'?') AS ENTI_NUCO,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_TITU ,'?') AS CCEX_TITU,NVL(TNHT_ENTI.ENTI_NOME ,'?') AS ENTI_NOME,"
        SQL = SQL & " NVL(TNHT_ENTI.ENTI_FAMA ,'?') AS ENTI_FAMA,"
        SQL = SQL & " NVL(TNHT_CACR.CACR_CTBA,'0') TARJETA,"
        SQL = SQL & " NVL(TNHT_FORE.FORE_CTB1,'0') FORMA"

        SQL = SQL & ",TNHT_MOVI.MOVI_DARE || TNHT_MOVI.MOVI_CODI AS RECIBO "

        SQL = SQL & ",TNHT_FACT.FACT_NUCO AS FACT_NUCO ,NVL(TNHT_FACT.FACT_TITU,'?') AS FACT_TITU "

        SQL = SQL & ",TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,FACT_HOEM "


        SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO"
        ' N
        SQL = SQL & " ,TNHT_FORE,TNHT_CACR,TNHT_ENTI,TNHT_CCEX "

        '
        SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        'N
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI(+)"
        SQL = SQL & " AND TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+)"
        SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"


        ' NUEVO POR AJUSTE DE RENDIMIENTO 
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) AND "
        SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) AND"


        SQL = SQL & " TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"

        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"


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

            Factura = CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String)

            Linea = Linea + 1
            If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = (CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1)
            End If

            If CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), String) = "0" Then
                ' ES UN NO ALOJADO
                TipoCliente = 2
                Nif = CType(Me.DbLeeHotel.mDbLector("CCEX_NUCO"), String)
                Descripcion = "No Alo : [" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CCEX_TITU"), String)
            Else
                ' NO ES UN NO ALOJADO
                ' ES CLIENTE DIRECTO
                If CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) = "?" Then
                    TipoCliente = 3
                    Nif = Me.mClientesContadoCif
                    Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String)
                Else ' ES CLIENTE DE AGENCIA
                    If CType(Me.DbLeeHotel.mDbLector("ENTI_FAMA"), String) = "1" Then
                        ' TTOO NORMAL
                        TipoCliente = 1
                        Nif = CType(Me.DbLeeHotel.mDbLector("ENTI_NUCO"), String)
                        Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    Else
                        ' TTOO ON LINE ( EL ANTICIPO LE LO CARGAMOS ACLIENTES CONTADO
                        TipoCliente = 3
                        Nif = Me.mClientesContadoCif
                        Descripcion = "[* TTOO WEB ] Rva : " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    End If
                End If
            End If

            ' CONTROL CASO DE TENER LA FACTURA CODIGO DE ENTIDAD O NO ALOJADO Y EL USUARIO HABER CAMBIADO 
            ' EL NIF Y EL NOMBRE DEL TITULAR AL EMITIR LA FACTURA

            ' PREVALECE EL NIF PUESTO A MANO POR EL USUARIO 
            If IsDBNull(Me.DbLeeHotel.mDbLector("FACT_NUCO")) = False Then
                If CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) <> Nif And Nif <> Me.mClientesContadoCif Then
                    If mMostrarMensajes = True Then
                        MsgBox("Atención Discrepancia de Nif " & vbCrLf & "Nif que Corresponde " & Nif & vbCrLf & "Nif hallado en Factura " & CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) & vbCrLf & vbCrLf & "Documento " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), MsgBoxStyle.Exclamation, "Atención ")
                    Else
                        Me.mListBoxDebug.Items.Add("Atención Discrepancia de Nif " & vbCrLf & "Nif que Corresponde " & Nif & vbCrLf & "Nif hallado en Factura " & CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) & vbCrLf & vbCrLf & "Documento " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String))
                    End If
                    ' quito debajo por peticion de domingo solo avisamos de discrepancia 
                    'Nif = CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String)
                    Descripcion += " (*) " & CType(Me.DbLeeHotel.mDbLector("FACT_TITU"), String)
                End If
            End If



            If CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) = "0" Then
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("FORMA"), String)
            Else
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String)
            End If

            Me.mCancelacionAnticipos = Me.mCancelacionAnticipos + Total

            '-----------------------------------------------------------------------------------------------------
            ' nueva gestion 
            '---------------------------------------------------------------------------------------------------
            ' si la factura es la primera en la que se factura el anticipo 


            If SaberSiAnticipoFacturadoEstuvoFacturadoAntes(CInt(Me.DbLeeHotel.mDbLector("MOVI_CODI")), CDate(Me.DbLeeHotel.mDbLector("MOVI_DARE")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CDate(Me.DbLeeHotel.mDbLector("FACT_HOEM"))) = False Then
                ' Facturacion del Anticipo en la primera factura 
                ' se envia el cobro
                Linea = Linea + 1
                Me.mTipoAsiento = "DEBE"

                ' Si la factura no contiene produccion se excluye el cobro path = 2561
                If Me.SaberSiFacturaContieneProduccion(CStr(Me.DbLeeHotel.mDbLector("SERIE")), CInt(Me.DbLeeHotel.mDbLector("NUMERO"))) = False Then

                    Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(Orden 1 ) " & Factura & " ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "", 9, "NADA")
                Else
                    ' PAT 36 "DEVOLUCION DE ANTICIPO FACTURADO"
                    ' EN SALOBRE HACEN LAS DEVOLUCIONES INSERTANDO ANTICIPOS EN NEGATIVO POR ESO < 0 
                    If CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) < 0 Then
                        Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(Orden 5 ) " & Factura & " ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "NREC")
                        SQL = "UPDATE TH_ASNT SET ASNT_AUXILIAR_STRING2 = 'NREC' WHERE ASNT_MOVI_CODI = " & Me.DbLeeHotel.mDbLector("MOVI_CODI")
                        SQL += " AND ASNT_F_VALOR =  " & "'" & Me.mFecha & "'"
                        SQL += " AND ASNT_AUXILIAR_STRING =  'ANTICIPO'"
                        Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                    Else
                        Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(Orden 1 ) " & Factura & " ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "")

                    End If
                    '  Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(Orden 1 ) " & Factura & " ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "")
                End If


            Else
                '         si la factura NO es la primera en la que se factura el anticipo y NO es Anulativa
                If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                    ' Facturacion del Anticipo en las siguientes facturas
                    ' se envia el cobro NREC
                    Linea = Linea + 1
                    Me.mTipoAsiento = "DEBE"

                    ' Si la factura no contiene produccion se excluye el cobro path = 2561
                    If Me.SaberSiFacturaContieneProduccion(CStr(Me.DbLeeHotel.mDbLector("SERIE")), CInt(Me.DbLeeHotel.mDbLector("NUMERO"))) = False Then
                        Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(Orden 3 NREC) " & Factura & " ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "NREC", 9, "NADA")
                    Else
                        Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(Orden 3 NREC) " & Factura & " ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "NREC")
                    End If

                End If
            End If



            ' si la factura es rectificativa
            If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = False Then
                ' se envia anticipo REC negativo
                Linea = Linea + 1
                Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "REC " & Descripcion.Replace("'", "''"), Total, "NO", Nif, "(Anticipo Rectificativo REC) " & Factura & " F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "ANTICIPO", "SAT_NHPrePaymentService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "NADA", "NADA", "NADA", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), "REC")
                ' se envia anticipo NREC positivo
                Linea = Linea + 1
                Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "NREC " & Descripcion.Replace("'", "''"), Total * -1, "NO", Nif, "(Anticipo Rectificativo NREC) " & Factura & " F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "ANTICIPO", "SAT_NHPrePaymentService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "NADA", "NADA", "NADA", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), "NREC")
                ' se envia el cobro REC negativo
                Linea = Linea + 1

                ' Si la factura no contiene produccion se excluye el cobro path = 2561
                If Me.SaberSiFacturaContieneProduccion(CStr(Me.DbLeeHotel.mDbLector("SERIE")), CInt(Me.DbLeeHotel.mDbLector("NUMERO"))) = False Then
                    Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(Anticipo Rectificativo REC) " & Factura & " ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "REC", 9, "NADA")
                Else
                    Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(Anticipo Rectificativo REC) " & Factura & " ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "REC")
                End If


            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub FacturasContadoCancelaciondeDevolucionesManualesAx()
        Dim Total As Double
        '    Dim TotalCancelados As Double

        Dim SQL As String

        Dim Descripcion As String = ""
        Dim TipoCliente As Integer
        Dim Nif As String
        Dim FormaCobro As String = ""


        'SQL = "SELECT 'Anticipo RVA= ' ||TNHT_MOVI.RESE_CODI||'/'||TNHT_MOVI.RESE_ANCI RESERVA,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA, "
        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA, "

        SQL += "TNHT_MOVI.MOVI_VDEB TOTAL,NVL(TNHT_RESE.RESE_ANPH,'?') CLIENTE,"
        SQL += " TNHT_FACT.FACT_CODI AS NUMERO ,TNHT_FACT.SEFA_CODI SERIE,"
        SQL = SQL & "TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA,TNHT_FACT.FACT_DAEM,TNHT_MOVI.MOVI_DATR,TNHT_FACT.FAAN_CODI,TNHT_FACT.ENTI_CODI AS ENTIFACT "

        ' N
        SQL = SQL & " ,NVL(TNHT_RESE.RESE_CODI ,0) AS RESE_CODI,NVL(TNHT_RESE.RESE_ANCI ,0) AS RESE_ANCI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_CODI ,'?') AS CCEX_CODI,NVL(TNHT_ENTI.ENTI_CODI ,'?') AS ENTI_CODI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_NUCO ,'?') AS CCEX_NUCO,NVL(TNHT_ENTI.ENTI_NUCO ,'?') AS ENTI_NUCO,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_TITU ,'?') AS CCEX_TITU,NVL(TNHT_ENTI.ENTI_NOME ,'?') AS ENTI_NOME,"
        SQL = SQL & " NVL(TNHT_ENTI.ENTI_FAMA ,'?') AS ENTI_FAMA,"
        SQL = SQL & " NVL(TNHT_CACR.CACR_CTBA,'0') TARJETA,"
        SQL = SQL & " NVL(TNHT_FORE.FORE_CTB1,'0') FORMA"

        ' SQL = SQL & ",NVL(MOVI_NUDO,'?') AS RECIBO"
        SQL = SQL & ",TNHT_MOVI.MOVI_DARE || TNHT_MOVI.MOVI_CODI AS RECIBO "

        SQL = SQL & ",TNHT_FACT.FACT_NUCO AS FACT_NUCO ,NVL(TNHT_FACT.FACT_TITU,'?') AS FACT_TITU "


        SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO"
        ' N
        SQL = SQL & " ,TNHT_FORE,TNHT_CACR,TNHT_ENTI,TNHT_CCEX "

        '
        SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        'N
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI(+)"
        SQL = SQL & " AND TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+)"
        SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"


        ' NUEVO POR AJUSTE DE RENDIMIENTO 
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) AND "
        SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) AND"


        SQL = SQL & " TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '5'"
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

            If CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), String) = "0" Then
                ' ES UN NO ALOJADO
                TipoCliente = 2
                Nif = CType(Me.DbLeeHotel.mDbLector("CCEX_NUCO"), String)
                Descripcion = "No Alo : [" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CCEX_TITU"), String)
            Else
                ' NO ES UN NO ALOJADO
                ' ES CLIENTE DIRECTO
                If CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) = "?" Then
                    TipoCliente = 3
                    Nif = Me.mClientesContadoCif
                    Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String)
                Else ' ES CLIENTE DE AGENCIA
                    If CType(Me.DbLeeHotel.mDbLector("ENTI_FAMA"), String) = "1" Then
                        ' TTOO NORMAL
                        TipoCliente = 1
                        Nif = CType(Me.DbLeeHotel.mDbLector("ENTI_NUCO"), String)
                        Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    Else
                        ' TTOO ON LINE ( EL ANTICIPO LE LO CARGAMOS ACLIENTES CONTADO
                        TipoCliente = 3
                        Nif = Me.mClientesContadoCif
                        Descripcion = "[* TTOO WEB ] Rva : " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    End If
                End If
            End If

            ' CONTROL CASO DE TENER LA FACTURA CODIGO DE ENTIDAD O NO ALOJADO Y EL USUARIO HABER CAMBIADO 
            ' EL NIF Y EL NOMBRE DEL TITULAR AL EMITIR LA FACTURA

            ' PREVALECE EL NIF PUESTO A MANO POR EL USUARIO 
            If IsDBNull(Me.DbLeeHotel.mDbLector("FACT_NUCO")) = False Then
                If CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) <> Nif And Nif <> Me.mClientesContadoCif Then
                    If mMostrarMensajes = True Then
                        MsgBox("Atención Discrepancia de Nif " & vbCrLf & "Nif que Corresponde " & Nif & vbCrLf & "Nif hallado en Factura " & CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) & vbCrLf & vbCrLf & "Documento " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), MsgBoxStyle.Exclamation, "Atención ")
                    Else
                        Me.mListBoxDebug.Items.Add("Atención Discrepancia de Nif " & vbCrLf & "Nif que Corresponde " & Nif & vbCrLf & "Nif hallado en Factura " & CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) & vbCrLf & vbCrLf & "Documento " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String))
                    End If
                    ' quito debajo por peticion de domingo solo avisamos de discrepancia 
                    'Nif = CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String)
                    Descripcion += " (*) " & CType(Me.DbLeeHotel.mDbLector("FACT_TITU"), String)
                End If
            End If



            If CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) = "0" Then
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("FORMA"), String)
            Else
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String)
            End If

            Me.mCancelacionAnticipos = Me.mCancelacionAnticipos + Total
            Me.mTipoAsiento = "DEBE"
            '      Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String) & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", "", "", "", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), "")
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(*) Devolución Manual ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String))


            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)

            Linea = Linea + 1

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub FacturasContadoCancelaciondeAnticiposrectificativasAx()
        Dim Total As Double
        '    Dim TotalCancelados As Double

        Dim SQL As String

        Dim Descripcion As String = ""
        Dim TipoCliente As Integer
        Dim Nif As String
        Dim FormaCobro As String = ""

        Dim Factura As String


        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA, "

        SQL += "TNHT_MOVI.MOVI_VDEB TOTAL,NVL(TNHT_RESE.RESE_ANPH,'?') CLIENTE,"
        SQL += " TNHT_FACT.FACT_CODI AS NUMERO ,TNHT_FACT.SEFA_CODI SERIE,"
        SQL = SQL & "TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA,TNHT_FACT.FACT_DAEM,TNHT_MOVI.MOVI_DATR,TNHT_FACT.FAAN_CODI,TNHT_FACT.FAAN_SEFA,TNHT_FACT.ENTI_CODI AS ENTIFACT "

        ' N
        SQL = SQL & " ,NVL(TNHT_RESE.RESE_CODI ,0) AS RESE_CODI,NVL(TNHT_RESE.RESE_ANCI ,0) AS RESE_ANCI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_CODI ,'?') AS CCEX_CODI,NVL(TNHT_ENTI.ENTI_CODI ,'?') AS ENTI_CODI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_NUCO ,'?') AS CCEX_NUCO,NVL(TNHT_ENTI.ENTI_NUCO ,'?') AS ENTI_NUCO,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_TITU ,'?') AS CCEX_TITU,NVL(TNHT_ENTI.ENTI_NOME ,'?') AS ENTI_NOME,"
        SQL = SQL & " NVL(TNHT_ENTI.ENTI_FAMA ,'?') AS ENTI_FAMA,"
        SQL = SQL & " NVL(TNHT_CACR.CACR_CTBA,'0') TARJETA,"
        SQL = SQL & " NVL(TNHT_FORE.FORE_CTB1,'0') FORMA"

        ' SQL = SQL & ",NVL(MOVI_NUDO,'?') AS RECIBO"
        SQL = SQL & ",TNHT_MOVI.MOVI_DARE || TNHT_MOVI.MOVI_CODI AS RECIBO "

        SQL = SQL & ",TNHT_FACT.FACT_NUCO AS FACT_NUCO ,NVL(TNHT_FACT.FACT_TITU,'?') AS FACT_TITU "

        SQL = SQL & ",TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,FACT_HOEM "


        SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO"
        ' N
        SQL = SQL & " ,TNHT_FORE,TNHT_CACR,TNHT_ENTI,TNHT_CCEX "

        '
        SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        'N
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI(+)"
        SQL = SQL & " AND TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+)"
        SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"


        ' NUEVO POR AJUSTE DE RENDIMIENTO 
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) AND "
        SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) AND"


        SQL = SQL & " TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '1'"
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


        Linea = 8000

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1

            If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = (CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1)
            End If


            Factura = CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String)

            If CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), String) = "0" Then
                ' ES UN NO ALOJADO
                TipoCliente = 2
                Nif = CType(Me.DbLeeHotel.mDbLector("CCEX_NUCO"), String)
                Descripcion = "No Alo : [" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CCEX_TITU"), String)
            Else
                ' NO ES UN NO ALOJADO
                ' ES CLIENTE DIRECTO
                If CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) = "?" Then
                    TipoCliente = 3
                    Nif = Me.mClientesContadoCif
                    Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String)
                Else ' ES CLIENTE DE AGENCIA
                    If CType(Me.DbLeeHotel.mDbLector("ENTI_FAMA"), String) = "1" Then
                        ' TTOO NORMAL
                        TipoCliente = 1
                        Nif = CType(Me.DbLeeHotel.mDbLector("ENTI_NUCO"), String)
                        Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    Else
                        ' TTOO ON LINE ( EL ANTICIPO LE LO CARGAMOS ACLIENTES CONTADO
                        TipoCliente = 3
                        Nif = Me.mClientesContadoCif
                        Descripcion = "[* TTOO WEB ] Rva : " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                    End If
                End If
            End If

            ' CONTROL CASO DE TENER LA FACTURA CODIGO DE ENTIDAD O NO ALOJADO Y EL USUARIO HABER CAMBIADO 
            ' EL NIF Y EL NOMBRE DEL TITULAR AL EMITIR LA FACTURA

            ' PREVALECE EL NIF PUESTO A MANO POR EL USUARIO 
            If IsDBNull(Me.DbLeeHotel.mDbLector("FACT_NUCO")) = False Then
                If CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) <> Nif And Nif <> Me.mClientesContadoCif Then
                    If mMostrarMensajes = True Then
                        MsgBox("Atencion Discrepancia de Nif " & vbCrLf & "Nif que Corresponde " & Nif & vbCrLf & "Nif hallado en Factura " & CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) & vbCrLf & vbCrLf & "Documento " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), MsgBoxStyle.Exclamation, "Atención ")
                    Else
                        Me.mListBoxDebug.Items.Add("Atencion Discrepancia de Nif " & vbCrLf & "Nif que Corresponde " & Nif & vbCrLf & "Nif hallado en Factura " & CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String) & vbCrLf & vbCrLf & "Documento " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String))
                    End If
                    ' quito debajo por peticion de domingo solo avisamos de discrepancia 
                    'Nif = CType(Me.DbLeeHotel.mDbLector("FACT_NUCO"), String)
                    Descripcion += " (*) " & CType(Me.DbLeeHotel.mDbLector("FACT_TITU"), String)
                End If
            End If



            If CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) = "0" Then
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("FORMA"), String)
            Else
                FormaCobro = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String)
            End If


            ' GENERAR DOS ANTICIPOS Y DOS COBROS 
            Me.mTipoAsiento = "DEBE"




            ' PAT 0001
            ' EL ANTICIPO

            If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = False Then
                ' Es una factura rectificativa
                Linea = Linea + 1
                Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "REC " & Descripcion.Replace("'", "''"), Total, "NO", Nif, "(*R1) " & Factura & " F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "ANTICIPO", "SAT_NHPrePaymentService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "NADA", "NADA", "NADA", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), "REC")

            Else
                ' NO ES UNA RECTIFICATIVA PERO el anticipo se rectifico alguna vez en otra factura
                If SaberSiAnticipoFacturadoHoyEstuvoenFacturaRectificativa(CInt(Me.DbLeeHotel.mDbLector("MOVI_CODI")), CDate(Me.DbLeeHotel.mDbLector("MOVI_DARE")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CDate(Me.DbLeeHotel.mDbLector("FACT_HOEM"))) = True Then
                    ' anticipo nueva factura
                    Linea = Linea + 1
                    Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "NREC " & Descripcion.Replace("'", "''"), Total, "NO", Nif, "(*R2) " & Factura & " F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "ANTICIPO", "SAT_NHPrePaymentService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "NADA", "NADA", "NADA", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), "NREC")
                End If

            End If

            ' EL COBRO
            If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = False Then

                ' Es una factura rectificativa
                Linea = Linea + 1
                Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(*R1) " & Factura & " ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "REC")

            Else
                ' NO ES UNA RECTIFICATIVA PERO el anticipo se rectifico alguna vez en otra factura
                If SaberSiAnticipoFacturadoHoyEstuvoenFacturaRectificativa(CInt(Me.DbLeeHotel.mDbLector("MOVI_CODI")), CDate(Me.DbLeeHotel.mDbLector("MOVI_DARE")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CDate(Me.DbLeeHotel.mDbLector("FACT_HOEM"))) = True Then

                    Linea = Linea + 1
                    Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "(*R2) " & Factura & " ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "NREC")

                End If

            End If





        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub

    

    Private Function SaberSiAnticipoFacturadoHoyEstuvoenFacturaRectificativa(ByVal vMoviCodi As Integer, ByVal vMoviDare As Date, ByVal vSefaCodi As String, ByVal vFactCodi As Integer, ByVal vFactHore As Date) As Boolean

        Dim SQL As String

        Dim Descripcion As String = ""
        Dim FormaCobro As String = ""

        Dim HayRegistros As Boolean = False


        Try



            SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA, "

            SQL += "TNHT_MOVI.MOVI_VDEB TOTAL,NVL(TNHT_RESE.RESE_ANPH,'?') CLIENTE,"
            SQL += " TNHT_FACT.FACT_CODI AS NUMERO ,TNHT_FACT.SEFA_CODI SERIE,"
            SQL = SQL & "TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA,TNHT_FACT.FACT_DAEM,TNHT_MOVI.MOVI_DATR,TNHT_FACT.FAAN_CODI,TNHT_FACT.ENTI_CODI AS ENTIFACT "

            ' N
            SQL = SQL & " ,NVL(TNHT_RESE.RESE_CODI ,0) AS RESE_CODI,NVL(TNHT_RESE.RESE_ANCI ,0) AS RESE_ANCI,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_CODI ,'?') AS CCEX_CODI,NVL(TNHT_ENTI.ENTI_CODI ,'?') AS ENTI_CODI,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_NUCO ,'?') AS CCEX_NUCO,NVL(TNHT_ENTI.ENTI_NUCO ,'?') AS ENTI_NUCO,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_TITU ,'?') AS CCEX_TITU,NVL(TNHT_ENTI.ENTI_NOME ,'?') AS ENTI_NOME,"
            SQL = SQL & " NVL(TNHT_ENTI.ENTI_FAMA ,'?') AS ENTI_FAMA,"
            SQL = SQL & " NVL(TNHT_CACR.CACR_CTBA,'0') TARJETA,"
            SQL = SQL & " NVL(TNHT_FORE.FORE_CTB1,'0') FORMA"

            ' SQL = SQL & ",NVL(MOVI_NUDO,'?') AS RECIBO"
            SQL = SQL & ",TNHT_MOVI.MOVI_DARE || TNHT_MOVI.MOVI_CODI AS RECIBO "

            SQL = SQL & ",TNHT_FACT.FACT_NUCO AS FACT_NUCO ,NVL(TNHT_FACT.FACT_TITU,'?') AS FACT_TITU "


            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO"
            ' N
            SQL = SQL & " ,TNHT_FORE,TNHT_CACR,TNHT_ENTI,TNHT_CCEX "

            '
            SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
            SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
            SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

            'N
            SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+)"
            SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"


            ' NUEVO POR AJUSTE DE RENDIMIENTO 
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) AND "
            SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "


            ' saber si el anticipo que estamos facturando ha estado en una factura rectificativa antes 
            SQL = SQL & " AND TNHT_MOVI.MOVI_CODI = " & vMoviCodi
            SQL = SQL & " AND TNHT_MOVI.MOVI_DARE =" & "'" & vMoviDare & "'"
            '   SQL = SQL & " AND TNHT_FACT.FACT_DAEM < " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_FACT.FACT_CODI <> " & vFactCodi
            SQL = SQL & " AND TNHT_FACT.SEFA_CODI <> " & "'" & vSefaCodi & "'"
            SQL = SQL & " AND TNHT_FACT.FAAN_CODI IS  NOT NULL "

            ' SOLO SI LA RECTIFICATIVA ES ANTERIOR 20100518

            ' no se si activar este filtro o no hablar con domingo

            If Me.mDebug = True Then
                SQL = SQL & " AND TNHT_FACT.FACT_HOEM  < TO_DATE('" & vFactHore & "','DD/MM/YYYY HH24:MI:SS')"
            End If




            SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"
            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '1'"
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

            Me.DbLeeHotelAux.TraerLector(SQL)

            While Me.DbLeeHotelAux.mDbLector.Read
                HayRegistros = True
            End While
            Me.DbLeeHotelAux.mDbLector.Close()
            Return HayRegistros
        Catch ex As Exception
            Return HayRegistros
        End Try
    End Function

    Private Function SaberSiAnticipoFacturadoEstuvoFacturadoAntes(ByVal vMoviCodi As Integer, ByVal vMoviDare As Date, ByVal vSefaCodi As String, ByVal vFactCodi As Integer, ByVal vFactHore As Date) As Boolean

        Dim SQL As String

        Dim Descripcion As String = ""
        Dim FormaCobro As String = ""

        Dim HayRegistros As Boolean = False


        Try



            SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL "


            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO"
            ' N
            SQL = SQL & " ,TNHT_FORE,TNHT_CACR,TNHT_ENTI,TNHT_CCEX "

            '
            SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
            SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
            SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

            'N
            SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+)"
            SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"


            ' NUEVO POR AJUSTE DE RENDIMIENTO 
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) AND "
            SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "


            SQL = SQL & " AND TNHT_MOVI.MOVI_CODI = " & vMoviCodi
            SQL = SQL & " AND TNHT_MOVI.MOVI_DARE =" & "'" & vMoviDare & "'"
            SQL = SQL & " AND TNHT_FACT.FACT_CODI <> " & vFactCodi
            SQL = SQL & " AND TNHT_FACT.SEFA_CODI <> " & "'" & vSefaCodi & "'"

            SQL = SQL & " AND TNHT_FACT.FACT_HOEM  < TO_DATE('" & vFactHore & "','DD/MM/YYYY HH24:MI:SS')"

            SQL += " ORDER BY TNHT_MOVI.MOVI_DAVA "

            Me.DbLeeHotelAux.TraerLector(SQL)

            While Me.DbLeeHotelAux.mDbLector.Read
                HayRegistros = True
            End While
            Me.DbLeeHotelAux.mDbLector.Close()
            Return HayRegistros
        Catch ex As Exception
            Return HayRegistros
        End Try
    End Function
    Private Function SaberSiDevolucionFacturadoEstuvoFacturadoAntes(ByVal vReseCodi As Integer, ByVal vReseAnci As Integer, ByVal vCcexCodi As String, ByVal vSefaCodi As String, ByVal vFactCodi As Integer, ByVal vFactHore As Date, ByVal vTotal As Double) As Boolean

        Dim SQL As String

        Dim Descripcion As String = ""
        Dim FormaCobro As String = ""

        Dim HayRegistros As Boolean = False


        Try



            SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL "


            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO"
            ' N
            SQL = SQL & " ,TNHT_FORE,TNHT_CACR,TNHT_ENTI,TNHT_CCEX "

            '
            SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
            SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
            SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

            'N
            SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+)"
            SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"


            ' NUEVO POR AJUSTE DE RENDIMIENTO 
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) AND "
            SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "


            SQL = SQL & " AND (TNHT_MOVI.RESE_CODI = " & vReseCodi
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI =" & vReseAnci
            SQL = SQL & " OR  TNHT_MOVI.CCEX_CODI ='" & vCcexCodi & "')"

            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB = " & vTotal
            SQL = SQL & " AND TNHT_FACT.FACT_CODI <> " & vFactCodi
            SQL = SQL & " AND TNHT_FACT.SEFA_CODI <> " & "'" & vSefaCodi & "'"

            SQL = SQL & " AND TNHT_FACT.FACT_HOEM  < TO_DATE('" & vFactHore & "','DD/MM/YYYY HH24:MI:SS')"

            SQL += " ORDER BY TNHT_MOVI.MOVI_DAVA "

            Me.DbLeeHotelAux.TraerLector(SQL)

            While Me.DbLeeHotelAux.mDbLector.Read
                HayRegistros = True
            End While
            Me.DbLeeHotelAux.mDbLector.Close()
            Return HayRegistros
        Catch ex As Exception
            Return HayRegistros
        End Try
    End Function
    Private Function DamePrefijoBookId(ByVal vMoviCodi As Integer, ByVal vMoviDare As Date, ByVal vSerie As String, ByVal vFactura As Integer, ByVal vFactHoem As Date, ByVal vFaanCodi As Integer) As String


        '-----------------------------------------------------------------------------------------------------
        ' nueva gestion 
        '---------------------------------------------------------------------------------------------------
        ' si la factura es la primera en la que se factura el anticipo 

        If SaberSiAnticipoFacturadoEstuvoFacturadoAntes(vMoviCodi, vMoviDare, vSerie, vFactura, vFactHoem) = False Then
            ' Facturacion del Anticipo en la primera factura 
            ' se envia el cobro
            Return ""
        Else
            '         si la factura NO es la primera en la que se factura el anticipo y NO es Anulativa
            If vFaanCodi = 0 Then
                ' Facturacion del Anticipo en las siguientes facturas
                ' se envia el cobro NREC
                Return "NREC"
            Else
                ' si la factura es rectificativa
                Return "REC"
            End If
        End If




    End Function

    Private Function SaberSiFacturaContieneAnticipos(ByVal vSefaCodi As String, ByVal vFactCodi As Integer) As Boolean

        Dim SQL As String

        Dim Descripcion As String = ""
        Dim FormaCobro As String = ""

        Dim HayRegistros As Boolean = False


        Try


            SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL"


            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO"
            ' N
            SQL = SQL & " ,TNHT_FORE,TNHT_CACR,TNHT_ENTI,TNHT_CCEX "

            '
            SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
            SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
            SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

            'N
            SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+)"
            SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"


            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) AND "
            SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "



            SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"
            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '1'"
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


            SQL = SQL & " AND TNHT_FACT.FACT_CODI = " & vFactCodi
            SQL = SQL & " AND TNHT_FACT.SEFA_CODI = " & "'" & vSefaCodi & "'"


            SQL += " ORDER BY TNHT_MOVI.MOVI_DAVA "

            Me.DbLeeHotelAux.TraerLector(SQL)

            While Me.DbLeeHotelAux.mDbLector.Read
                HayRegistros = True
            End While
            Me.DbLeeHotelAux.mDbLector.Close()
            Return HayRegistros
        Catch ex As Exception
            Return HayRegistros
        End Try
    End Function
    Private Function SaberSiFacturaContieneAnticiposDevuelveMovimiento(ByVal vSefaCodi As String, ByVal vFactCodi As Integer) As String()

        Dim SQL As String

        Dim Descripcion As String = ""
        Dim FormaCobro As String = ""

        Dim HayRegistros As Boolean = False

        Dim Retorno(1) As String

        Try


            SQL = "SELECT  TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE"


            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO"
            ' N
            SQL = SQL & " ,TNHT_FORE,TNHT_CACR,TNHT_ENTI,TNHT_CCEX "

            '
            SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
            SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
            SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

            'N
            SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+)"
            SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"


            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) AND "
            SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "



            SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"
            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '1'"
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


            SQL = SQL & " AND TNHT_FACT.FACT_CODI = " & vFactCodi
            SQL = SQL & " AND TNHT_FACT.SEFA_CODI = " & "'" & vSefaCodi & "'"

            SQL = SQL & " AND ROWNUM = 1 "

            SQL += " ORDER BY TNHT_MOVI.MOVI_DAVA "

            Me.DbLeeHotelAux.TraerLector(SQL)

            While Me.DbLeeHotelAux.mDbLector.Read
                Retorno(0) = Me.DbLeeHotelAux.mDbLector("MOVI_CODI")
                Retorno(1) = Me.DbLeeHotelAux.mDbLector("MOVI_DARE")

            End While
            Me.DbLeeHotelAux.mDbLector.Close()
            Return Retorno
        Catch ex As Exception
            Return Retorno
        End Try
    End Function
    Private Function SaberSiFacturaContieneProduccion(ByVal vSefaCodi As String, ByVal vFactCodi As Integer) As Boolean

        Dim SQL As String
        Dim Result As String



        Try


            SQL = " SELECT NVL(COUNT(*),0)  AS TOTAL "
            SQL += " FROM TNHT_FACT, TNHT_FAMO, VNHT_MOVH TNHT_MOVI "
            SQL += " WHERE TNHT_FACT.FACT_CODI = TNHT_FAMO.FACT_CODI "
            SQL += " AND TNHT_FACT.SEFA_CODI = TNHT_FAMO.SEFA_CODI "
            SQL += " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += " AND TNHT_MOVI.MOVI_TIMO = '1' "

            SQL += " AND  TNHT_FACT.FACT_CODI = " & vFactCodi
            SQL += " AND  TNHT_FACT.SEFA_CODI = '" & vSefaCodi & "'"



            Result = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            If CInt(Result) = 0 Then
                Return False
            Else
                Return True
            End If


        Catch ex As Exception
            Return False
        End Try
    End Function




    Private Sub FacturasContadoTotalVisasComision()
        Dim Total As Double

        Dim TotalComision As Double
        Dim vCentroCosto As String

        SQL = "SELECT SUM(MOVI_VDEB) TOTAL,CACR_DESC TARJETA,nvl(CACR_CTBA,'0') CUENTA,NVL(TNHT_CACR.CACR_CONT,'0') CUENTAGASTO,TNHT_CACR.CACR_COMI,"
        SQL += " NVL(FAAN_CODI,'0') AS FAAN_CODI  FROM VNHT_MOVH TNHT_MOVI,TNHT_CACR,TNHT_FACT,TNHT_FAMO WHERE"

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
        SQL = SQL & " GROUP BY TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CTBA,TNHT_CACR.CACR_CONT,TNHT_CACR.CACR_COMI,TNHT_FACT.FAAN_CODI"

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
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision)

            Linea = Linea + 1
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTAGASTO"), String), Me.mIndicadorDebe, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", vCentroCosto, "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTAGASTO"), String), Me.mIndicadorDebe, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision)



        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
#End Region
#Region "ASIENTO-37 FACTURACION DE CONTADO  NEWGOLF"
    Private Sub FacturasContadoTotalGolf()

        Dim Total As Double

        Dim SQL As String



        SQL = "SELECT MOVI_IMPT AS TOTAL  FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_FACT," & Me.mParaUsuarioNewGolf & ".TNPL_FAMO "
        SQL += " WHERE TNPL_FAMO.FACT_CODI = TNPL_FACT.FACT_CODI AND "
        SQL += " TNPL_FAMO.SEFA_CODI = TNPL_FACT.SEFA_CODI AND "

        SQL = SQL & " TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE"
        SQL = SQL & " AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI "

        ' SE SUPONE MOVI_TIPO = 2  SON COBROS 
        SQL += "AND    TNPL_MOVI.MOVI_TIPO = '2'  "
        SQL = SQL & "  AND TNPL_MOVI.MOVI_DEAN = '0'"
        SQL += "AND TNPL_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "


        Me.DbLeeHotel.TraerLector(SQL)


        Total = 0
        While Me.DbLeeHotel.mDbLector.Read

            Total = Total + Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

        End While
        Me.DbLeeHotel.mDbLector.Close()

        If Total <> 0 Then
            Linea = 1
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaClientesContado, Me.mIndicadorHaber, "COBROS FACTURACION " & Me.mFecha, Total, "SI", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaClientesContado, Me.mIndicadorHaber, "COBROS FACTURACION " & Me.mFecha, Total)

        End If




        SQL = "SELECT MOVI_IMPT * -1 AS TOTAL  FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_FACT," & Me.mParaUsuarioNewGolf & ".TNPL_FAMO "
        SQL += " WHERE TNPL_FAMO.FACT_CODI = TNPL_FACT.FACT_CODI AND "
        SQL += " TNPL_FAMO.SEFA_CODI = TNPL_FACT.SEFA_CODI AND "

        SQL = SQL & " TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE"
        SQL = SQL & " AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI "

        ' SE SUPONE MOVI_TIPO = 2  SON COBROS 
        SQL += "AND    TNPL_MOVI.MOVI_TIPO = '2'  "
        SQL = SQL & "  AND TNPL_MOVI.MOVI_DEAN = '0'"
        SQL += "AND TNPL_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "

        Me.DbLeeHotel.TraerLector(SQL)

        Total = 0
        While Me.DbLeeHotel.mDbLector.Read

            Total = Total + Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

        End While
        Me.DbLeeHotel.mDbLector.Close()


        Total = Decimal.Round(CType(Total, Decimal), 2)

        If Total <> 0 Then
            Linea = Linea + 1
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaClientesContado, Me.mIndicadorHaber, "COBROS FACTURACION ANULADO " & Me.mFecha, Total, "SI", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaClientesContado, Me.mIndicadorHaber, "COBROS FACTURACION ANULADO " & Me.mFecha, Total)

        End If

    End Sub

    Private Sub FacturasContadoTotalVisasGolf()
        Dim Total As Double
        Dim Descripcion As String
        SQL = "SELECT MOVI_IMPT TOTAL,CACR_DESC TARJETA,NVL(CACR_CTBA,'0') CUENTA,"
        SQL += " TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI,NVL(TNPL_FACT.FACT_TITU,' ') AS TITULAR "
        SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_CACO," & Me.mParaUsuarioNewGolf & ".TNPL_FACT," & Me.mParaUsuarioNewGolf & ".TNPL_FAMO,TNHT_CACR "
        SQL += " WHERE "

        SQL = SQL & " TNPL_FAMO.FACT_CODI = TNPL_FACT.FACT_CODI"
        SQL = SQL & " AND TNPL_FAMO.SEFA_CODI = TNPL_FACT.SEFA_CODI"
        SQL = SQL & " AND TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE"
        SQL = SQL & " AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI"


        ' SOLO DEBITO 
        SQL = SQL & " AND TNPL_MOVI.MOVI_DECR = '0'"


        SQL = SQL & " AND TNPL_MOVI.CACR_CODI = TNPL_CACO.CACR_COD1"
        ' TARJETAS DE CREDITO NEWHOTEL
        SQL = SQL & " AND TNPL_CACO.CACR_COD2 = TNHT_CACR.CACR_CODI"


        ' NO ES DEPOSITO ANTICIPADO

        SQL = SQL & "  AND TNPL_MOVI.MOVI_DEAN = '0'"

        SQL = SQL & " AND TNPL_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " ORDER BY TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

            Descripcion = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion, Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion, Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()


        ' *************** VISAS anulado de dias anteriores 

        SQL = "SELECT MOVI_IMPT * -1  TOTAL,CACR_DESC TARJETA,NVL(CACR_CTBA,'0') CUENTA,"
        SQL += " TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI,NVL(TNPL_FACT.FACT_TITU,' ') AS TITULAR "
        SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_CACO," & Me.mParaUsuarioNewGolf & ".TNPL_FACT," & Me.mParaUsuarioNewGolf & ".TNPL_FAMO,TNHT_CACR "
        SQL += " WHERE "

        SQL = SQL & " TNPL_FAMO.FACT_CODI = TNPL_FACT.FACT_CODI"
        SQL = SQL & " AND TNPL_FAMO.SEFA_CODI = TNPL_FACT.SEFA_CODI"
        SQL = SQL & " AND TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE"
        SQL = SQL & " AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI"


        ' SOLO DEBITO 
        SQL = SQL & " AND TNPL_MOVI.MOVI_DECR = '0'"


        SQL = SQL & " AND TNPL_MOVI.CACR_CODI = TNPL_CACO.CACR_COD1"
        ' TARJETAS DE CREDITO NEWHOTEL
        SQL = SQL & " AND TNPL_CACO.CACR_COD2 = TNHT_CACR.CACR_CODI"


        ' NO ES DEPOSITO ANTICIPADO

        SQL = SQL & "  AND TNPL_MOVI.MOVI_DEAN = '0'"

        SQL = SQL & " AND TNPL_FACT.FACT_DAAN = " & "'" & Me.mFecha & "'"
        SQL = SQL & " ORDER BY TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI"




        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

            Descripcion = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion & " Deducido", Total, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion & " Deducido", Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasContadoTotalVisasGolfAx()
        Dim Total As Double
        Dim Descripcion As String
        SQL = "SELECT MOVI_IMPT TOTAL,CACR_DESC TARJETA,NVL(CACR_CTBA,'0') CUENTA,"
        SQL += " TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI,NVL(TNPL_FACT.FACT_TITU,' ') AS TITULAR,TNHT_CACR.CACR_CODI AS TIPOCOBRO "
        SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_CACO," & Me.mParaUsuarioNewGolf & ".TNPL_FACT," & Me.mParaUsuarioNewGolf & ".TNPL_FAMO,TNHT_CACR "
        SQL += " WHERE "

        SQL = SQL & " TNPL_FAMO.FACT_CODI = TNPL_FACT.FACT_CODI"
        SQL = SQL & " AND TNPL_FAMO.SEFA_CODI = TNPL_FACT.SEFA_CODI"
        SQL = SQL & " AND TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE"
        SQL = SQL & " AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI"


        ' SOLO DEBITO 
        SQL = SQL & " AND TNPL_MOVI.MOVI_DECR = '0'"


        SQL = SQL & " AND TNPL_MOVI.CACR_CODI = TNPL_CACO.CACR_COD1"
        ' TARJETAS DE CREDITO NEWHOTEL
        SQL = SQL & " AND TNPL_CACO.CACR_COD2 = TNHT_CACR.CACR_CODI"


        ' NO ES DEPOSITO ANTICIPADO

        SQL = SQL & "  AND TNPL_MOVI.MOVI_DEAN = '0'"

        SQL = SQL & " AND TNPL_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " ORDER BY TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

            Descripcion = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion, Total, "NO", "", "", "SI", "", "SAT_NHJournalCustPaymentQueryService", "", CType(Me.DbLeeHotel.mDbLector("TIPOCOBRO"), String), "", CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String), "")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion, Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()


        ' *************** VISAS anulado de dias anteriores 

        SQL = "SELECT MOVI_IMPT * -1  TOTAL,CACR_DESC TARJETA,NVL(CACR_CTBA,'0') CUENTA,"
        SQL += " TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI,NVL(TNPL_FACT.FACT_TITU,' ') AS TITULAR ,TNHT_CACR.CACR_CODI AS TIPOCOBRO"
        SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_CACO," & Me.mParaUsuarioNewGolf & ".TNPL_FACT," & Me.mParaUsuarioNewGolf & ".TNPL_FAMO,TNHT_CACR "
        SQL += " WHERE "

        SQL = SQL & " TNPL_FAMO.FACT_CODI = TNPL_FACT.FACT_CODI"
        SQL = SQL & " AND TNPL_FAMO.SEFA_CODI = TNPL_FACT.SEFA_CODI"
        SQL = SQL & " AND TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE"
        SQL = SQL & " AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI"


        ' SOLO DEBITO 
        SQL = SQL & " AND TNPL_MOVI.MOVI_DECR = '0'"


        SQL = SQL & " AND TNPL_MOVI.CACR_CODI = TNPL_CACO.CACR_COD1"
        ' TARJETAS DE CREDITO NEWHOTEL
        SQL = SQL & " AND TNPL_CACO.CACR_COD2 = TNHT_CACR.CACR_CODI"


        ' NO ES DEPOSITO ANTICIPADO

        SQL = SQL & "  AND TNPL_MOVI.MOVI_DEAN = '0'"

        SQL = SQL & " AND TNPL_FACT.FACT_DAAN = " & "'" & Me.mFecha & "'"
        SQL = SQL & " ORDER BY TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI"




        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

            Descripcion = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion & " Deducido", Total, "NO", "", "", "SI", "", "SAT_NHJournalCustPaymentQueryService", "", CType(Me.DbLeeHotel.mDbLector("TIPOCOBRO"), String), "", CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String), "")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion & " Deducido", Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub FacturasContadoTotaLOtrasFormasGolf()
        Dim Total As Double
        Dim SQL As String
        Try

            SQL = ""
            SQL = "SELECT MOVI_IMPT TOTAL,"
            SQL += "TNPL_FPAG.FPAG_DESC TIPO,"
            SQL += "TNPL_FACT.SEFA_CODI,"
            SQL += "TNPL_FACT.FACT_CODI,"
            SQL += "NVL(TNPL_FACT.FACT_TITU,   ' ') AS TITULAR,"
            SQL += "NVL(TNPL_FPAG.FPAG_CTB1,'0') AS CUENTANEWG,"
            SQL += "NVL(TNHT_FORE.FORE_CTB1,'0') AS CUENTANEWH "
            SQL += "FROM GMS.TNPL_MOVI,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FACT,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FAMO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPCO,"
            SQL += "TNHT_FORE "
            SQL += "WHERE TNPL_FAMO.FACT_CODI = TNPL_FACT.FACT_CODI "
            SQL += "AND TNPL_FAMO.SEFA_CODI = TNPL_FACT.SEFA_CODI "
            SQL += "AND TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE "
            SQL += "AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI "
            SQL += "AND TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI "
            SQL += "AND TNPL_FPAG.FPAG_CODI  = TNPL_FPCO.FPAG_CODI(+) "
            SQL += "AND TNPL_FPCO.FORE_CODI  = TNHT_FORE.FORE_CODI "
            SQL += "AND TNPL_MOVI.CACR_CODI IS NULL "
            SQL += "AND TNPL_MOVI.MOVI_DECR = '0' "
            SQL += "AND TNPL_MOVI.MOVI_DEAN = '0' "
            SQL += "AND TNPL_FACT.FACT_DAEM =" & "'" & Me.mFecha & "' "
            SQL += "ORDER BY TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI"

            Me.DbLeeHotel.TraerLector(SQL)

            ' MsgBox("Control cuenta newgolf , newhotel ")
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTANEWH"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTANEWH"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()


            ' *************** contado anulado de dias anteriores 
            SQL = ""
            SQL = "SELECT MOVI_IMPT * -1 TOTAL,"
            SQL += "TNPL_FPAG.FPAG_DESC TIPO,"
            SQL += "TNPL_FACT.SEFA_CODI,"
            SQL += "TNPL_FACT.FACT_CODI,"
            SQL += "NVL(TNPL_FACT.FACT_TITU,   ' ') AS TITULAR,"
            SQL += "NVL(TNPL_FPAG.FPAG_CTB1,'0') AS CUENTANEWG,"
            SQL += "NVL(TNHT_FORE.FORE_CTB1,'0') AS CUENTANEWH "
            SQL += "FROM GMS.TNPL_MOVI,"
            '  SQL += Me.mParaUsuarioNewGolf & ".TNPL_CACO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FACT,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FAMO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPCO,"
            SQL += "TNHT_FORE "
            SQL += "WHERE TNPL_FAMO.FACT_CODI = TNPL_FACT.FACT_CODI "
            SQL += "AND TNPL_FAMO.SEFA_CODI = TNPL_FACT.SEFA_CODI "
            SQL += "AND TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE "
            SQL += "AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI "
            SQL += "AND TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI "
            SQL += "AND TNPL_FPAG.FPAG_CODI  = TNPL_FPCO.FPAG_CODI(+) "
            SQL += "AND TNPL_FPCO.FORE_CODI  = TNHT_FORE.FORE_CODI "
            SQL += "AND TNPL_MOVI.CACR_CODI IS NULL "
            SQL += "AND TNPL_MOVI.MOVI_DECR = '0' "
            SQL += "AND TNPL_MOVI.MOVI_DEAN = '0' "
            SQL += "AND TNPL_FACT.FACT_DAAN =" & "'" & Me.mFecha & "' "
            SQL += "ORDER BY TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI"
            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTANEWH"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " Deducido", Total, "NO", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTANEWH"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " Deducido", Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()


        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try

    End Sub
    Private Sub FacturasContadoTotaLOtrasFormasGolfAx()
        Dim Total As Double
        Dim SQL As String
        Try

            SQL = ""
            SQL = "SELECT MOVI_IMPT TOTAL,"
            SQL += "TNPL_FPAG.FPAG_DESC TIPO,"
            SQL += "TNPL_FACT.SEFA_CODI,"
            SQL += "TNPL_FACT.FACT_CODI,"
            SQL += "NVL(TNPL_FACT.FACT_TITU,   ' ') AS TITULAR,"
            SQL += "NVL(TNPL_FPAG.FPAG_CTB1,'0') AS CUENTANEWG,"
            SQL += "NVL(TNHT_FORE.FORE_CTB1,'0') AS CUENTANEWH,TNHT_FORE.FORE_CODI AS TIPOCOBRO "
            SQL += "FROM GMS.TNPL_MOVI,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FACT,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FAMO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPCO,"
            SQL += "TNHT_FORE "
            SQL += "WHERE TNPL_FAMO.FACT_CODI = TNPL_FACT.FACT_CODI "
            SQL += "AND TNPL_FAMO.SEFA_CODI = TNPL_FACT.SEFA_CODI "
            SQL += "AND TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE "
            SQL += "AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI "
            SQL += "AND TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI "
            SQL += "AND TNPL_FPAG.FPAG_CODI  = TNPL_FPCO.FPAG_CODI(+) "
            SQL += "AND TNPL_FPCO.FORE_CODI  = TNHT_FORE.FORE_CODI "
            SQL += "AND TNPL_MOVI.CACR_CODI IS NULL "
            SQL += "AND TNPL_MOVI.MOVI_DECR = '0' "
            SQL += "AND TNPL_MOVI.MOVI_DEAN = '0' "
            SQL += "AND TNPL_FACT.FACT_DAEM =" & "'" & Me.mFecha & "' "
            SQL += "ORDER BY TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI"

            Me.DbLeeHotel.TraerLector(SQL)

            ' MsgBox("Control cuenta newgolf , newhotel ")
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTANEWH"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI", "", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("TIPOCOBRO"), String), "", "", CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String), "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTANEWH"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()


            ' *************** contado anulado de dias anteriores 
            SQL = ""
            SQL = "SELECT MOVI_IMPT * -1 TOTAL,"
            SQL += "TNPL_FPAG.FPAG_DESC TIPO,"
            SQL += "TNPL_FACT.SEFA_CODI,"
            SQL += "TNPL_FACT.FACT_CODI,"
            SQL += "NVL(TNPL_FACT.FACT_TITU,   ' ') AS TITULAR,"
            SQL += "NVL(TNPL_FPAG.FPAG_CTB1,'0') AS CUENTANEWG,"
            SQL += "NVL(TNHT_FORE.FORE_CTB1,'0') AS CUENTANEWH,TNHT_FORE.FORE_CODI AS TIPOCOBRO "
            SQL += "FROM GMS.TNPL_MOVI,"
            '  SQL += Me.mParaUsuarioNewGolf & ".TNPL_CACO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FACT,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FAMO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPCO,"
            SQL += "TNHT_FORE "
            SQL += "WHERE TNPL_FAMO.FACT_CODI = TNPL_FACT.FACT_CODI "
            SQL += "AND TNPL_FAMO.SEFA_CODI = TNPL_FACT.SEFA_CODI "
            SQL += "AND TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE "
            SQL += "AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI "
            SQL += "AND TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI "
            SQL += "AND TNPL_FPAG.FPAG_CODI  = TNPL_FPCO.FPAG_CODI(+) "
            SQL += "AND TNPL_FPCO.FORE_CODI  = TNHT_FORE.FORE_CODI "
            SQL += "AND TNPL_MOVI.CACR_CODI IS NULL "
            SQL += "AND TNPL_MOVI.MOVI_DECR = '0' "
            SQL += "AND TNPL_MOVI.MOVI_DEAN = '0' "
            SQL += "AND TNPL_FACT.FACT_DAAN =" & "'" & Me.mFecha & "' "
            SQL += "ORDER BY TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI"
            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTANEWH"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " Deducido", Total, "NO", "", "", "SI", "", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("TIPOCOBRO"), String), "", "", CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String), "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTANEWH"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " Deducido", Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()


        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try

    End Sub
    Private Sub FacturasContadoCancelaciondeAnticiposGolf()
        Dim Total As Double
        '    Dim TotalCancelados As Double

        Dim SQL As String


        Dim Cuenta As String = ""

        Try


            SQL = "SELECT 'Anticipo RVA= ' ||TNPL_MOVI.RESE_CODI||'/'||TNPL_MOVI.RESE_ANCI RESERVA,TNPL_FACT.FACT_CODI||'/'||TNPL_FACT.SEFA_CODI FACTURA, "
            SQL += "TNPL_MOVI.MOVI_IMPT TOTAL,"
            SQL += " TNPL_FACT.FACT_CODI AS NUMERO ,TNPL_FACT.SEFA_CODI SERIE,TNPL_FACT.ENTI_CODI, "
            SQL += "TNPL_MOVI.MOVI_FECV FROM "

            SQL += Me.mParaUsuarioNewGolf & ".TNPL_MOVI,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FACT,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FAMO "

            SQL += "WHERE TNPL_FAMO.FACT_CODI = TNPL_FACT.FACT_CODI "
            SQL += "AND TNPL_FAMO.SEFA_CODI = TNPL_FACT.SEFA_CODI "
            SQL += "AND TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE "
            SQL += "AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI "

            SQL += "AND TNPL_MOVI.MOVI_DECR = '0' "
            SQL += "AND TNPL_MOVI.MOVI_DEAN = '1' "
            SQL += "AND TNPL_FACT.FACT_DAEM =" & "'" & Me.mFecha & "' "
            SQL += "ORDER BY TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)


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
                Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_FECV"), String) & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI", "ANTICIPO FACTURADO")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)

                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_FECV"), String) & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)


            End While
            Me.DbLeeHotel.mDbLector.Close()


            ' PAGOS A CUENTA EN FACTURAS ANULADAS 

            SQL = "SELECT 'Anticipo RVA= ' ||TNPL_MOVI.RESE_CODI||'/'||TNPL_MOVI.RESE_ANCI RESERVA,TNPL_FACT.FACT_CODI||'/'||TNPL_FACT.SEFA_CODI FACTURA, "
            SQL += "TNPL_MOVI.MOVI_IMPT TOTAL,"
            SQL += " TNPL_FACT.FACT_CODI AS NUMERO ,TNPL_FACT.SEFA_CODI SERIE,TNPL_FACT.ENTI_CODI, "
            SQL += "TNPL_MOVI.MOVI_FECV FROM "

            SQL += Me.mParaUsuarioNewGolf & ".TNPL_MOVI,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FACT,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FAMO "

            SQL += "WHERE TNPL_FAMO.FACT_CODI = TNPL_FACT.FACT_CODI "
            SQL += "AND TNPL_FAMO.SEFA_CODI = TNPL_FACT.SEFA_CODI "
            SQL += "AND TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE "
            SQL += "AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI "

            SQL += "AND TNPL_MOVI.MOVI_DECR = '0' "
            SQL += "AND TNPL_MOVI.MOVI_DEAN = '1' "
            SQL += "AND TNPL_FACT.FACT_DAAN =" & "'" & Me.mFecha & "' "
            SQL += "ORDER BY TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1


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
                Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_FECV"), String) & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String) & " Anulada", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)

                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_FECV"), String) & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String) & " Anulada", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)


            End While
            Me.DbLeeHotel.mDbLector.Close()

        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try

    End Sub
    Private Sub FacturasContadoCancelaciondeAnticiposGolfAx()
        Dim Total As Double


        Dim Descripcion As String = ""
        Dim TipoCliente As Integer
        Dim Nif As String
        Dim FormaCobro As String = ""

        '    Dim TotalCancelados As Double

        Dim SQL As String


        Dim Cuenta As String = ""

        Try


            SQL = "SELECT 'Anticipo RVA= ' ||TNPL_MOVI.RESE_CODI||'/'||TNPL_MOVI.RESE_ANCI RESERVA,TNPL_FACT.FACT_CODI||'/'||TNPL_FACT.SEFA_CODI FACTURA, "
            SQL += "TNPL_MOVI.MOVI_IMPT TOTAL,"
            SQL += " TNPL_FACT.FACT_CODI AS NUMERO ,TNPL_FACT.SEFA_CODI SERIE,TNPL_FACT.ENTI_CODI, "


            SQL = SQL & " NVL(TNPL_RESE.RESE_CODI ,0) AS RESE_CODI,NVL(TNPL_RESE.RESE_ANCI ,0) AS RESE_ANCI,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_CODI ,'?') AS CCEX_CODI,NVL(TNHT_ENTI.ENTI_CODI ,'?') AS ENTI_CODI,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_NUCO ,'?') AS CCEX_NUCO,NVL(TNHT_ENTI.ENTI_NUCO ,'?') AS ENTI_NUCO,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_TITU ,'?') AS CCEX_TITU,NVL(TNHT_ENTI.ENTI_NOME ,'?') AS ENTI_NOME,"


            SQL += " NVL(TNHT_CACR.CACR_CTBA,'0') TARJETA,"
            SQL += " NVL(TNHT_FORE.FORE_CTB1,'0') FORMA, "
            SQL += " NVL(TNHT_ENTI.ENTI_FAMA ,'?') AS ENTI_FAMA ,"

            SQL += "TNPL_MOVI.MOVI_FECV "
            '   SQL += ",NVL(TNPL_MOVI.MOVI_NUDO,'?') AS RECIBO"
            SQL += ",TNPL_MOVI.MOVI_DATE || TNPL_MOVI.MOVI_CODI AS RECIBO "


            SQL += " FROM TNHT_ENTI,TNHT_CCEX,TNHT_CACR,TNHT_FORE,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_MOVI,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FACT,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FAMO, "
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RESE, "

            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPCO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_CACO "

            SQL += "WHERE TNPL_FAMO.FACT_CODI = TNPL_FACT.FACT_CODI "
            SQL += "AND TNPL_FAMO.SEFA_CODI = TNPL_FACT.SEFA_CODI "
            SQL += "AND TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE "
            SQL += "AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI "

            SQL += " AND     TNPL_MOVI.RESE_CODI = TNPL_RESE.RESE_CODI(+) "
            SQL += " AND TNPL_MOVI.RESE_ANCI = TNPL_RESE.RESE_ANCI(+)"

            SQL += "AND TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI "
            SQL += "AND TNPL_FPAG.FPAG_CODI  = TNPL_FPCO.FPAG_CODI(+) "
            SQL += "AND TNPL_FPCO.FORE_CODI  = TNHT_FORE.FORE_CODI(+) "
            ' TARJETAS DE CREDITO NEWHOTEL
            SQL += " AND TNPL_MOVI.CACR_CODI = TNPL_CACO.CACR_COD1(+)"
            SQL += " AND TNPL_CACO.CACR_COD2 = TNHT_CACR.CACR_CODI(+)"


            SQL += " AND TNPL_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
            SQL += " AND TNPL_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"


            SQL += "AND TNPL_MOVI.MOVI_DECR = '0' "
            SQL += "AND TNPL_MOVI.MOVI_DEAN = '1' "
            SQL += "AND TNPL_FACT.FACT_DAEM =" & "'" & Me.mFecha & "' "
            SQL += "ORDER BY TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

                If CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), String) = "0" Then
                    ' ES UN NO ALOJADO
                    TipoCliente = 2
                    Nif = CType(Me.DbLeeHotel.mDbLector("CCEX_NUCO"), String)
                    Descripcion = "No Alo : [" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CCEX_TITU"), String)
                Else
                    ' NO ES UN NO ALOJADO
                    ' ES CLIENTE DIRECTO
                    If CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) = "?" Then
                        TipoCliente = 3
                        Nif = Me.mClientesContadoCif
                        Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String)
                    Else ' ES CLIENTE DE AGENCIA
                        If CType(Me.DbLeeHotel.mDbLector("ENTI_FAMA"), String) = "1" Then
                            ' TTOO NORMAL
                            TipoCliente = 1
                            Nif = CType(Me.DbLeeHotel.mDbLector("ENTI_NUCO"), String)
                            Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                        Else
                            ' TTOO ON LINE ( EL ANTICIPO LE LO CARGAMOS ACLIENTES CONTADO
                            TipoCliente = 3
                            Nif = Me.mClientesContadoCif
                            Descripcion = "[* TTOO WEB ] Rva : " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                        End If
                    End If
                End If


                If CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) = "0" Then
                    FormaCobro = CType(Me.DbLeeHotel.mDbLector("FORMA"), String)
                Else
                    FormaCobro = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String)
                End If

                Me.mCancelacionAnticipos = Me.mCancelacionAnticipos + Total
                Me.mTipoAsiento = "DEBE"
                '      Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String) & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", "", "", "", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), "")
                Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String))
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)

                Linea = Linea + 1


            End While
            Me.DbLeeHotel.mDbLector.Close()


            ' PAGOS A CUENTA EN FACTURAS ANULADAS 

            SQL = "SELECT 'Anticipo RVA= ' ||TNPL_MOVI.RESE_CODI||'/'||TNPL_MOVI.RESE_ANCI RESERVA,TNPL_FACT.FACT_CODI||'/'||TNPL_FACT.SEFA_CODI FACTURA, "
            SQL += "TNPL_MOVI.MOVI_IMPT TOTAL,"
            SQL += " TNPL_FACT.FACT_CODI AS NUMERO ,TNPL_FACT.SEFA_CODI SERIE,TNPL_FACT.ENTI_CODI, "


            SQL = SQL & " NVL(TNPL_RESE.RESE_CODI ,0) AS RESE_CODI,NVL(TNPL_RESE.RESE_ANCI ,0) AS RESE_ANCI,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_CODI ,'?') AS CCEX_CODI,NVL(TNHT_ENTI.ENTI_CODI ,'?') AS ENTI_CODI,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_NUCO ,'?') AS CCEX_NUCO,NVL(TNHT_ENTI.ENTI_NUCO ,'?') AS ENTI_NUCO,"
            SQL = SQL & " NVL(TNHT_CCEX.CCEX_TITU ,'?') AS CCEX_TITU,NVL(TNHT_ENTI.ENTI_NOME ,'?') AS ENTI_NOME,"


            SQL += " NVL(TNHT_CACR.CACR_CTBA,'0') TARJETA,"
            SQL += " NVL(TNHT_FORE.FORE_CTB1,'0') FORMA, "
            SQL += " NVL(TNHT_ENTI.ENTI_FAMA ,'?') AS ENTI_FAMA ,"

            SQL += "TNPL_MOVI.MOVI_FECV "

            '  SQL += ",NVL(TNPL_MOVI.MOVI_NUDO,'?') AS RECIBO "
            SQL += ",TNPL_MOVI.MOVI_DATE || TNPL_MOVI.MOVI_CODI AS RECIBO "


            SQL += " FROM TNHT_ENTI,TNHT_CCEX,TNHT_CACR,TNHT_FORE,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_MOVI,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FACT,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FAMO, "
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RESE, "

            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPCO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_CACO "

            SQL += "WHERE TNPL_FAMO.FACT_CODI = TNPL_FACT.FACT_CODI "
            SQL += "AND TNPL_FAMO.SEFA_CODI = TNPL_FACT.SEFA_CODI "
            SQL += "AND TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE "
            SQL += "AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI "

            SQL += " AND     TNPL_MOVI.RESE_CODI = TNPL_RESE.RESE_CODI(+) "
            SQL += " AND TNPL_MOVI.RESE_ANCI = TNPL_RESE.RESE_ANCI(+)"

            SQL += "AND TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI "
            SQL += "AND TNPL_FPAG.FPAG_CODI  = TNPL_FPCO.FPAG_CODI(+) "
            SQL += "AND TNPL_FPCO.FORE_CODI  = TNHT_FORE.FORE_CODI(+) "
            ' TARJETAS DE CREDITO NEWHOTEL
            SQL += " AND TNPL_MOVI.CACR_CODI = TNPL_CACO.CACR_COD1(+)"
            SQL += " AND TNPL_CACO.CACR_COD2 = TNHT_CACR.CACR_CODI(+)"


            SQL += " AND TNPL_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
            SQL += " AND TNPL_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"

            SQL += "AND TNPL_MOVI.MOVI_DECR = '0' "
            SQL += "AND TNPL_MOVI.MOVI_DEAN = '1' "
            SQL += "AND TNPL_FACT.FACT_DAAN =" & "'" & Me.mFecha & "' "
            SQL += "ORDER BY TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1


                If CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), String) = "0" Then
                    ' ES UN NO ALOJADO
                    TipoCliente = 2
                    Nif = CType(Me.DbLeeHotel.mDbLector("CCEX_NUCO"), String)
                    Descripcion = "No Alo : [" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CCEX_TITU"), String)
                Else
                    ' NO ES UN NO ALOJADO
                    ' ES CLIENTE DIRECTO
                    If CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) = "?" Then
                        TipoCliente = 3
                        Nif = Me.mClientesContadoCif
                        Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String)
                    Else ' ES CLIENTE DE AGENCIA
                        If CType(Me.DbLeeHotel.mDbLector("ENTI_FAMA"), String) = "1" Then
                            ' TTOO NORMAL
                            TipoCliente = 1
                            Nif = CType(Me.DbLeeHotel.mDbLector("ENTI_NUCO"), String)
                            Descripcion = "Rva : [" & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & "] " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                        Else
                            ' TTOO ON LINE ( EL ANTICIPO LE LO CARGAMOS ACLIENTES CONTADO
                            TipoCliente = 3
                            Nif = Me.mClientesContadoCif
                            Descripcion = "[* TTOO WEB ] Rva : " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ENTI_NOME"), String)
                        End If
                    End If
                End If


                If CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) = "0" Then
                    FormaCobro = CType(Me.DbLeeHotel.mDbLector("FORMA"), String)
                Else
                    FormaCobro = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String)
                End If

                Me.mCancelacionAnticipos = Me.mCancelacionAnticipos + Total
                Me.mTipoAsiento = "DEBE"
                '      Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String) & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", "", "", "", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), "")
                Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mParaAnticiposAxapta, Me.mIndicadorHaber, Descripcion.Replace("'", "''"), Total, "NO", Nif, "ANTICIPO FACTURADO", "SI", "ANTICIPO FACTURADO", "SAT_NHJournalCustPaymentQueryService", CType(Me.DbLeeHotel.mDbLector("RESE_CODI"), Integer), CType(Me.DbLeeHotel.mDbLector("RESE_ANCI"), Integer), CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String), CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String), TipoCliente, FormaCobro, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String))

                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("RESERVA"), String), Total)

                Linea = Linea + 1


            End While
            Me.DbLeeHotel.mDbLector.Close()

        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try

    End Sub
    Private Sub FacturasContadoTotalVisasComisionGolf()
        Dim Total As Double
        Dim TotalComision As Double
        Dim vCentroCosto As String

        SQL = "SELECT MOVI_IMPT TOTAL,CACR_DESC TARJETA,nvl(CACR_CTBA,'0') CUENTA,NVL(TNHT_CACR.CACR_CONT,'0') CUENTAGASTO,TNHT_CACR.CACR_COMI,"
        SQL += " TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI,NVL(TNPL_FACT.FACT_TITU,' ') AS TITULAR "
        SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_CACO," & Me.mParaUsuarioNewGolf & ".TNPL_FACT," & Me.mParaUsuarioNewGolf & ".TNPL_FAMO,TNHT_CACR "
        SQL += " WHERE "

        SQL = SQL & " TNPL_FAMO.FACT_CODI = TNPL_FACT.FACT_CODI"
        SQL = SQL & " AND TNPL_FAMO.SEFA_CODI = TNPL_FACT.SEFA_CODI"
        SQL = SQL & " AND TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE"
        SQL = SQL & " AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI"


        ' SOLO DEBITO 
        SQL = SQL & " AND TNPL_MOVI.MOVI_DECR = '0'"


        SQL = SQL & " AND TNPL_MOVI.CACR_CODI = TNPL_CACO.CACR_COD1"
        ' TARJETAS DE CREDITO NEWHOTEL
        SQL = SQL & " AND TNPL_CACO.CACR_COD2 = TNHT_CACR.CACR_CODI"


        ' NO ES DEPOSITO ANTICIPADO

        SQL = SQL & "  AND TNPL_MOVI.MOVI_DEAN = '0'"

        SQL = SQL & " AND TNPL_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
        SQL = SQL & " ORDER BY TNPL_FACT.SEFA_CODI,TNPL_FACT.FACT_CODI"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            SQL = "SELECT NVL(PARA_CENTRO_COSTO_COMI,'0') FROM TH_PARA "
            SQL += " WHERE  PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            vCentroCosto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalComision = (CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * CType(Me.DbLeeHotel.mDbLector("CACR_COMI"), Double)) / 100
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", "", "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision)

            Linea = Linea + 1
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 37, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTAGASTO"), String), Me.mIndicadorDebe, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", vCentroCosto, "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTAGASTO"), String), Me.mIndicadorDebe, "COMISION " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision)



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

        Try

            SQL = "SELECT ROUND(SUM(TNHT_MOVI.MOVI_VDEB),2) TOTAL"
            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI WHERE"
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
            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI WHERE"
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
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Desmbolsos")
            End If

        End Try
    End Sub
#End Region

#Region "ASIENTO-20 DEPOSITOS ANTICIPADOS ENTIDAD"
    Private Sub TotalDepositosAnticipadosVisasEntidad()
        Try
            Dim Total As Double
            SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA"
            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_CACR,TNHT_RESE,TNHT_ENTI WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"

            ' SOLO DE AGENCIAS
            SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI"

            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' solo depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='1'"
            '
            '     SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CTBA"


            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI", "ANTICIPO RECIBIDO")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
            End If

        End Try

    End Sub
    Private Sub TotalDepositosAnticipadosOtrasFormasEntidad()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_RESE,TNHT_ENTI WHERE"
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

        '  SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"
        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI", "ANTICIPO RECIBIDO")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetalleDepositosAnticipadosVisasEntidad()
        Dim Total As Double
        Dim Cuenta As String

        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
        SQL = SQL & " TNHT_CACR.CACR_DESC TARJETA,NVL(ENTI_DEAN,'0') AS CUENTA,MOVI_DAVA FROM VNHT_MOVH TNHT_MOVI,"
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

            Me.InsertaOracle("AC", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetalleDepositosAnticipadosOtrasFormasEntidad()
        Dim Total As Double
        Dim Cuenta As String

        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE,"
        SQL = SQL & " TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(ENTI_DEAN,'0') AS CUENTA,MOVI_DAVA FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_RESE,TNHT_ENTI WHERE"
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

            Me.InsertaOracle("AC", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
#End Region
#Region "ASIENTO-30 DEPOSTITOS DE DIRECTOS"
    Private Sub TotalDepositosAnticipadosVisasOtros()
        Try
            Dim Total As Double
            SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA"
            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI"

            ' NO DE AGENCIAS
            SQL = SQL & " AND TNHT_RESE.RESE_TRES = '1'"


            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' solo depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='1'"
            '
            '  SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CTBA"


            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 30, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI", "ANTICIPO RECIBIDO")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
            End If

        End Try

    End Sub
    Private Sub TotalDepositosAnticipadosOtrasFormasOtros()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
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

        '  SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"
        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 30, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI", "ANTICIPO RECIBIDO")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub DetalleDepositosAnticipadosVisasOtros()
        Try
            Dim Total As Double

            SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
            SQL = SQL & " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA FROM VNHT_MOVH TNHT_MOVI,"
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



                Me.InsertaOracle("AC", 30, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try

    End Sub
    Private Sub DetalleDepositosAnticipadosOtrasFormasOtros()
        Try
            Dim Total As Double

            SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE,"
            SQL = SQL & " TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,MOVI_DAVA FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
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



                Me.InsertaOracle("AC", 30, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try

    End Sub
#End Region
#Region "ASIENTO 32 DEPOSITOS ANTICIPADOS NO ALOJADOS"
    Private Sub TotalDepositosAnticipadosVisasOtrosNoAlojados()
        Try
            Dim Total As Double
            SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA"
            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_CACR WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"

            ' CUENTAS NO ALOJADO
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI IS NOT NULL"



            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' solo depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='1'"

            ' EXCLUYE PAGOS A CUENTA DE NEWGOLF YA ESTAN EN UN ASIENTO 
            SQL = SQL & " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"
            '
            '  SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CTBA"


            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 32, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total, "NO", "", "", "SI", "ANTICIPO RECIBIDO")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
            End If

        End Try

    End Sub
    Private Sub TotalDepositosAnticipadosOtrasFormasOtrosNoAlojados()
        Dim Total As Double
        SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "


        ' CUENTAS NO ALOJADO
        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI IS NOT NULL"




        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI IS NULL"
        SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' solo depositos anticipados 
        SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='1'"

        ' EXCLUYE PAGOS A CUENTA DE NEWGOLF YA ESTAN EN UN ASIENTO 
        SQL = SQL & " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"


        ' SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1"
        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 32, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI", "ANTICIPO RECIBIDO")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub DetalleDepositosAnticipadosVisasOtrosNoAlojados()
        Try
            Dim Total As Double

            SQL = "SELECT TNHT_MOVI.CCEX_CODI  CCEX,NVL(CCEX_TITU,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
            SQL = SQL & " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA FROM VNHT_MOVH TNHT_MOVI,"
            SQL = SQL & " TNHT_CACR,TNHT_CCEX WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI "

            ' CUENTAS NO ALOJADO
            SQL = SQL & " AND TNHT_MOVI.CCEX_CODI IS NOT NULL"


            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

            ' solo depositos anticipados 
            SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='1'"

            ' EXCLUYE PAGOS A CUENTA DE NEWGOLF YA ESTAN EN UN ASIENTO 
            SQL = SQL & " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"


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
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try

    End Sub
    Private Sub DetalleDepositosAnticipadosOtrasFormasOtrosNoAlojados()
        Try
            Dim Total As Double

            SQL = "SELECT TNHT_MOVI.CCEX_CODI CCEX,NVL(CCEX_TITU,'?') CLIENTE,"
            SQL = SQL & " TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,MOVI_DAVA FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_CCEX WHERE"
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

            ' EXCLUYE PAGOS A CUENTA DE NEWGOLF YA ESTAN EN UN ASIENTO 
            SQL = SQL & " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"


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
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try

    End Sub
#End Region

#Region "90 VENTA DE TIKETS"
    Private Sub VentaTiketsNewPos()

        Try
            ' SABER QUE PUNTOS DE VENTA HAY QUE ENVIAR A AX
            ' ARMAR FILTRO ( 1,2,3,4, ) Y APASAR A LA QUIERY 

            Dim FiltroTpvs As String = ""

            SQL = "SELECT IPOS_CODI FROM TH_IPOSVEN_AX WHERE IPOSVEN_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND IPOSVEN_EMP_COD ='" & Me.mEmpCod & "' AND ALMA_TIPO = 2"
            SQL += " GROUP BY IPOS_CODI"

            Me.DbLeeCentral.TraerLector(SQL)

            While Me.DbLeeCentral.mDbLector.Read
                If FiltroTpvs.Length = 0 Then
                    FiltroTpvs = Me.DbLeeCentral.mDbLector.Item("IPOS_CODI")
                Else
                    FiltroTpvs = FiltroTpvs & "," & Me.DbLeeCentral.mDbLector.Item("IPOS_CODI")
                End If
            End While
            Me.DbLeeCentral.mDbLector.Close()


            ' --
            Dim Total As Double
            Dim AlmacenAx As String = ""


            ' SQL = " SELECT TNPO_IPOS.IPOS_CODI,TNPO_IPOS.IPOS_NOME, SUM(TNPO_ARVE.ARVE_QTDS) AS TOTAL "
            'SQL = SQL & "  FROM TNPO_ARTG, TNPO_IPOS, TNPO_ARVE, TNPO_VEND "
            'SQL = SQL & " WHERE TNPO_VEND.VEND_CODI = TNPO_ARVE.VEND_CODI "
            'SQL = SQL & "   AND TNPO_VEND.VEND_ANCI = TNPO_ARVE.VEND_ANCI "
            'SQL = SQL & "   AND TNPO_VEND.IPOS_CODI = TNPO_ARVE.IPOS_CODI "
            'SQL = SQL & "   AND TNPO_IPOS.IPOS_CODI = TNPO_VEND.IPOS_CODI "
            'SQL = SQL & "   AND TNPO_VEND.CAJA_CODI = TNPO_VEND.CAJA_CODI "
            'SQL = SQL & "   AND TNPO_ARVE.ARTG_CODI = TNPO_ARTG.ARTG_CODI "
            'SQL = SQL & "   AND TNPO_VEND.VEND_ANUL = 0 "
            'SQL = SQL & "   AND TNPO_ARVE.ARVE_ANUL = 0 "
            'SQL = SQL & "   AND TRUNC(TNPO_VEND.VEND_DAHO) = '" & Me.mFecha & "'"
            'SQL += " AND ( TNPO_IPOS.IPOS_CODI IN "
            'SQL += "(SELECT IPOS_CODI FROM TH_IPOSVEN_AX WHERE IPOSVEN_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND IPOSVEN_EMP_COD ='" & Me.mEmpCod & "' AND ALMA_TIPO = 2))"
            'SQL += " GROUP BY TNPO_IPOS.IPOS_CODI,TNPO_IPOS.IPOS_NOME"

            SQL = " SELECT TNPO_IPOS.IPOS_CODI,TNPO_IPOS.IPOS_NOME, SUM(TNPO_ARVE.ARVE_QTDS) AS TOTAL "
            SQL = SQL & "  FROM TNPO_ARTG, TNPO_IPOS, TNPO_ARVE, TNPO_VEND "
            SQL = SQL & " WHERE TNPO_VEND.VEND_CODI = TNPO_ARVE.VEND_CODI "
            SQL = SQL & "   AND TNPO_VEND.VEND_ANCI = TNPO_ARVE.VEND_ANCI "
            SQL = SQL & "   AND TNPO_VEND.IPOS_CODI = TNPO_ARVE.IPOS_CODI "
            SQL = SQL & "   AND TNPO_IPOS.IPOS_CODI = TNPO_VEND.IPOS_CODI "
            SQL = SQL & "   AND TNPO_VEND.CAJA_CODI = TNPO_VEND.CAJA_CODI "
            SQL = SQL & "   AND TNPO_ARVE.ARTG_CODI = TNPO_ARTG.ARTG_CODI "
            SQL = SQL & "   AND TNPO_VEND.VEND_ANUL = 0 "
            SQL = SQL & "   AND TNPO_ARVE.ARVE_ANUL = 0 "
            SQL = SQL & "   AND TRUNC(TNPO_VEND.VEND_DAHO) = '" & Me.mFecha & "'"
            SQL += " AND  TNPO_IPOS.IPOS_CODI IN (" & FiltroTpvs & ") "
            SQL += " GROUP BY TNPO_IPOS.IPOS_CODI,TNPO_IPOS.IPOS_NOME"

            Me.DbNewPos.TraerLector(SQL)

            Linea = 0
            While Me.DbNewPos.mDbLector.Read

                Total = CType(Me.DbNewPos.mDbLector("TOTAL"), Double)

                If Total <> 0 Then
                    Linea = Linea + 1
                    Me.mTipoAsiento = "DEBE"

                    Me.InsertaOracle("AC", 90, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, "CUENTA", Me.mIndicadorDebe, "VENTA DE TIKETS " & CType(Me.DbNewPos.mDbLector("IPOS_NOME"), String), Total, "SI", "", "", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), "CUENTA", Me.mIndicadorDebe, "VENTA DE TIKETS ", Total)

                End If
            End While
            Me.DbNewPos.mDbLector.Close()


            ' DETALLE


            'SQL = " SELECT TNPO_IPOS.IPOS_CODI,TNPO_ARTG.ARTG_CODI ,TNPO_ARTG.ARTG_DESC, SUM(TNPO_ARVE.ARVE_QTDS) AS TOTAL,NVL(TNPO_ARTG.ARTG_COAX,'?') AS ARTICULO ,TNPO_ARTG.ARTG_CODI AS ARTICULONH"
            'SQL = SQL & "  FROM TNPO_ARTG, TNPO_IPOS, TNPO_ARVE, TNPO_VEND "
            'SQL = SQL & " WHERE TNPO_VEND.VEND_CODI = TNPO_ARVE.VEND_CODI "
            'SQL = SQL & "   AND TNPO_VEND.VEND_ANCI = TNPO_ARVE.VEND_ANCI "
            'SQL = SQL & "   AND TNPO_VEND.IPOS_CODI = TNPO_ARVE.IPOS_CODI "
            'SQL = SQL & "   AND TNPO_IPOS.IPOS_CODI = TNPO_VEND.IPOS_CODI "
            'SQL = SQL & "   AND TNPO_VEND.CAJA_CODI = TNPO_VEND.CAJA_CODI "
            'SQL = SQL & "   AND TNPO_ARVE.ARTG_CODI = TNPO_ARTG.ARTG_CODI "
            'SQL = SQL & "   AND TNPO_VEND.VEND_ANUL = 0 "
            'SQL = SQL & "   AND TNPO_ARVE.ARVE_ANUL = 0 "
            'SQL = SQL & "   AND TRUNC(TNPO_VEND.VEND_DAHO) = '" & Me.mFecha & "'"
            'SQL += " AND ( TNPO_IPOS.IPOS_CODI IN "
            'SQL += "(SELECT IPOS_CODI FROM TH_IPOSVEN_AX WHERE IPOSVEN_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND IPOSVEN_EMP_COD ='" & Me.mEmpCod & "' AND ALMA_TIPO = 2))"
            'SQL += " GROUP BY TNPO_IPOS.IPOS_CODI,TNPO_ARTG.ARTG_CODI,TNPO_ARTG.ARTG_DESC,TNPO_ARTG.ARTG_COAX "
            'SQL += " ORDER BY TNPO_ARTG.ARTG_DESC ASC"



            SQL = " SELECT TNPO_IPOS.IPOS_CODI,TNPO_ARTG.ARTG_CODI ,TNPO_ARTG.ARTG_DESC, SUM(TNPO_ARVE.ARVE_QTDS) AS TOTAL,NVL(TNPO_ARTG.ARTG_COAX,'?') AS ARTICULO ,TNPO_ARTG.ARTG_CODI AS ARTICULONH"
            SQL = SQL & "  FROM TNPO_ARTG, TNPO_IPOS, TNPO_ARVE, TNPO_VEND "
            SQL = SQL & " WHERE TNPO_VEND.VEND_CODI = TNPO_ARVE.VEND_CODI "
            SQL = SQL & "   AND TNPO_VEND.VEND_ANCI = TNPO_ARVE.VEND_ANCI "
            SQL = SQL & "   AND TNPO_VEND.IPOS_CODI = TNPO_ARVE.IPOS_CODI "
            SQL = SQL & "   AND TNPO_IPOS.IPOS_CODI = TNPO_VEND.IPOS_CODI "
            SQL = SQL & "   AND TNPO_VEND.CAJA_CODI = TNPO_VEND.CAJA_CODI "
            SQL = SQL & "   AND TNPO_ARVE.ARTG_CODI = TNPO_ARTG.ARTG_CODI "
            SQL = SQL & "   AND TNPO_VEND.VEND_ANUL = 0 "
            SQL = SQL & "   AND TNPO_ARVE.ARVE_ANUL = 0 "
            SQL = SQL & "   AND TRUNC(TNPO_VEND.VEND_DAHO) = '" & Me.mFecha & "'"

            SQL += " AND  TNPO_IPOS.IPOS_CODI IN (" & FiltroTpvs & ") "

            SQL += " GROUP BY TNPO_IPOS.IPOS_CODI,TNPO_ARTG.ARTG_CODI,TNPO_ARTG.ARTG_DESC,TNPO_ARTG.ARTG_COAX "
            SQL += " ORDER BY TNPO_ARTG.ARTG_DESC ASC"



            Me.DbNewPos.TraerLector(SQL)

            While Me.DbNewPos.mDbLector.Read


                SQL = "SELECT NVL(ALMA_AX,'?') FROM TH_IPOSVEN_AX WHERE IPOSVEN_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND IPOSVEN_EMP_COD ='" & Me.mEmpCod & "' AND IPOS_CODI = " & CType(Me.DbNewPos.mDbLector("IPOS_CODI"), Integer) & " AND ALMA_TIPO = 2"

                AlmacenAx = Me.DbLeeCentral.EjecutaSqlScalar(SQL)


                Total = CType(Me.DbNewPos.mDbLector("TOTAL"), Double)

                If Total <> 0 Then
                    Linea = Linea + 1
                    Me.mTipoAsiento = "HABER"

                    Me.InsertaOracle("AC", 90, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, "CUENTA", Me.mIndicadorHaber, "Venta " & CType(Me.DbNewPos.mDbLector("ARTG_DESC"), String), Total, "SI", "", "", "SI", "", "SAT_JournalLossProfitQueryService", CType(Me.DbNewPos.mDbLector("ARTICULONH"), String), "", "", AlmacenAx)
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), "CUENTA", Me.mIndicadorHaber, "Venta " & CType(Me.DbNewPos.mDbLector("ARTG_DESC"), String), Total)

                End If


            End While
            Me.DbNewPos.mDbLector.Close()


        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Atención en Venta de Tikets NewPos")
            End If

        End Try
    End Sub
    Private Sub VentaTiketsNewgOLF()

        Try


            Dim Total As Double


            Dim Talla As String = ""
            Dim Color As String = ""

            Dim AlmacenAx As String


            SQL = " SELECT "
            SQL = SQL & "       SUM(NVL(TNPL_MOVI.MOVI_PAXS * SIGN(TNPL_MOVI.MOVI_IMPT),0))  AS TOTAL  "
            SQL = SQL & "  FROM " & Me.mParaUsuarioNewGolf & ".VNPL_MOVH TNPL_MOVI, "
            SQL = SQL & "       " & Me.mParaUsuarioNewGolf & ".TNPL_ADIC , "
            SQL = SQL & "       " & Me.mParaUsuarioNewGolf & ".TNPL_TIAD , "
            SQL = SQL & "       " & Me.mParaUsuarioNewGolf & ".TNPL_ADSC   "

            SQL = SQL & " WHERE  "
            SQL = SQL & "       TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI "
            SQL = SQL & "   AND TNPL_ADIC.TIAD_CODI = TNPL_TIAD.TIAD_CODI "
            SQL = SQL & "   AND TNPL_MOVI.ADSC_CODI = TNPL_ADSC.ADSC_CODI(+) "
            SQL = SQL & "   AND TNPL_MOVI.MOVI_DECR = '1' "
            SQL = SQL & "   AND TNPL_MOVI.MOVI_IMPO <> 0 "
            SQL = SQL & "   AND TNPL_MOVI.SSPA_CODI IS NULL "
            ' SOLO ARTICULOS TIPO VENTA 
            SQL = SQL & "   AND TNPL_ADIC.ADIC_TIPO = 1 "

            SQL = SQL & "   AND TNPL_MOVI.MOVI_FECH = '" & Me.mFecha & "'"
            SQL = SQL & " having SUM (NVL(tnpl_movi.movi_paxs,'0') ) <> 0  "



            Me.DbNewPos.TraerLector(SQL)

            Linea = 0
            While Me.DbNewPos.mDbLector.Read



                Total = CType(Me.DbNewPos.mDbLector("TOTAL"), Double)

                If Total <> 0 Then
                    Linea = Linea + 1
                    Me.mTipoAsiento = "DEBE"

                    Me.InsertaOracle("AC", 95, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, "CUENTA", Me.mIndicadorDebe, "VENTA DE TIKETS TIENDA SALOBRE", Total, "SI", "", "", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), "CUENTA", Me.mIndicadorDebe, "VENTA DE TIKETS ", Total)

                End If
            End While
            Me.DbNewPos.mDbLector.Close()


            ' DETALLE

            SQL = " SELECT NVL(TNPL_ADIC.ADIC_DESC,'?') AS ADIC_DESC,NVL(TNPL_ADIC.ADIC_CTS1,'?') AS ARTICULO,NVL(TNPL_ADIC.ADIC_CODI,'0') AS ARTICULONH,TNPL_MOVI.ADSC_CODI,"
            SQL = SQL & "       SUM(NVL(TNPL_MOVI.MOVI_PAXS * SIGN(TNPL_MOVI.MOVI_IMPT),0))  AS TOTAL  "
            SQL = SQL & "  FROM " & Me.mParaUsuarioNewGolf & ".VNPL_MOVH TNPL_MOVI, "
            SQL = SQL & "       " & Me.mParaUsuarioNewGolf & ".TNPL_ADIC , "
            SQL = SQL & "       " & Me.mParaUsuarioNewGolf & ".TNPL_TIAD , "
            SQL = SQL & "       " & Me.mParaUsuarioNewGolf & ".TNPL_ADSC   "

            SQL = SQL & " WHERE  "
            SQL = SQL & "       TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI "
            SQL = SQL & "   AND TNPL_ADIC.TIAD_CODI = TNPL_TIAD.TIAD_CODI "
            SQL = SQL & "   AND TNPL_MOVI.ADSC_CODI = TNPL_ADSC.ADSC_CODI(+) "
            SQL = SQL & "   AND TNPL_MOVI.MOVI_DECR = '1' "
            SQL = SQL & "   AND TNPL_MOVI.MOVI_IMPO <> 0 "
            SQL = SQL & "   AND TNPL_MOVI.SSPA_CODI IS NULL "
            ' SOLO ARTICULOS TIPO VENTA 
            SQL = SQL & "   AND TNPL_ADIC.ADIC_TIPO = 1 "
            ' CONTROL NO NULO 
            SQL = SQL & "   AND TNPL_MOVI.MOVI_PAXS <> 0 "

            SQL = SQL & "   AND TNPL_MOVI.MOVI_FECH = '" & Me.mFecha & "'"
            SQL += " GROUP BY TNPL_ADIC.ADIC_DESC,TNPL_ADIC.ADIC_CTS1,TNPL_MOVI.ADSC_CODI,TNPL_ADIC.ADIC_CODI"
            SQL = SQL & " having SUM (NVL(tnpl_movi.movi_paxs,'0') ) <> 0  "

            SQL += " ORDER BY  TNPL_ADIC.ADIC_DESC ASC"



            Me.DbNewPos.TraerLector(SQL)

            While Me.DbNewPos.mDbLector.Read

                ' control de talla y color
                If IsDBNull(Me.DbNewPos.mDbLector.Item("ADSC_CODI")) = False Then
                    SQL = "SELECT ADIC_SIZE, ADIC_COLO FROM " & Me.mParaUsuarioNewGolf & ".TNPL_ADSC"
                    SQL += " WHERE ADSC_CODI = " & Me.DbNewPos.mDbLector.Item("ADSC_CODI")

                    Me.DbLeeHotel.TraerLector(SQL)
                    Me.DbLeeHotel.mDbLector.Read()

                    If Me.DbLeeHotel.mDbLector.HasRows Then
                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ADIC_SIZE")) = False Then
                            Talla = Me.DbLeeHotel.mDbLector.Item("ADIC_SIZE")
                        Else
                            Talla = ""
                        End If
                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ADIC_COLO")) = False Then
                            Color = Me.DbLeeHotel.mDbLector.Item("ADIC_COLO")
                        Else
                            Color = ""
                        End If
                    Else
                        Talla = ""
                        Color = ""
                    End If

                    Me.DbLeeHotel.mDbLector.Close()

                End If


                ' CONTROL DE ALMACEN AX


                SQL = "SELECT NVL(ALMA_AX,'?') FROM TH_IPOSVEN_AX WHERE IPOSVEN_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND IPOSVEN_EMP_COD ='" & Me.mEmpCod & "' AND IPOS_CODI = 1 AND ALMA_TIPO = 1 "

                AlmacenAx = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

                Total = CType(Me.DbNewPos.mDbLector("TOTAL"), Double)

                If Total <> 0 Then
                    Linea = Linea + 1
                    Me.mTipoAsiento = "HABER"

                    Me.InsertaOracle("AC", 95, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, "CUENTA", Me.mIndicadorHaber, "Venta " & CType(Me.DbNewPos.mDbLector("ADIC_DESC"), String), Total, "SI", "", "", "SI", "", "SAT_JournalLossProfitQueryService", CType(Me.DbNewPos.mDbLector.Item("ARTICULONH"), String), Talla, Color, AlmacenAx)
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), "CUENTA", Me.mIndicadorHaber, "Venta " & CType(Me.DbNewPos.mDbLector("ADIC_DESC"), String), Total)

                End If


            End While
            Me.DbNewPos.mDbLector.Close()


        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Atención en Venta de Tikets NewGolf")
            End If

        End Try
    End Sub
#End Region

#Region "ESTADISTICAS DE OCUPACION"
    Private Sub EstadisticasOcupacion()

        Try


            Dim Total As Double



            SQL = " SELECT NVL(ALOJ_OCUP,0) AS TOTAL FROM VNHT_ESOC "
            SQL = SQL & " WHERE  "
            SQL = SQL & "   EDTA_DATA = '" & Me.mFecha & "'"

            Me.DbLeeHotel.TraerLector(SQL)

            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read

                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Linea = Linea + 1
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 100, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, "CUENTA", Me.mIndicadorDebe, "HABITACIONES", Total, "NO", "", "", "SI", "HABITACIONES", "SAT_NHInfoAnalyticalQueryService")
                    '     Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)
                End If


            End While
            Me.DbLeeHotel.mDbLector.Close()


            SQL = " SELECT NVL(CLIE_PERN,0) AS TOTAL FROM VNHT_ESOC "
            SQL = SQL & " WHERE  "
            SQL = SQL & "   EDTA_DATA = '" & Me.mFecha & "'"

            Me.DbLeeHotel.TraerLector(SQL)


            While Me.DbLeeHotel.mDbLector.Read

                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Linea = Linea + 1
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 100, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, "CUENTA", Me.mIndicadorHaber, "CLIENTES", Total, "NO", "", "CLIENTES", "SI", "CLIENTES", "SAT_NHInfoAnalyticalQueryService")

                    '    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaPagosACuenta, Me.mIndicadorHaber, "RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String), Total)
                End If

            End While
            Me.DbLeeHotel.mDbLector.Close()



        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Atención en Venta de Tikets NewGolf")
            End If

        End Try
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
        SQL += " NCRE_TITU, NCRE_DAEM,NVL(ENTI_NCON_AF,0) CUENTA ,NVL(ENTI_NUCO,0) CIF,NVL(ENTI_NOME,'?') AS NOMBRE"
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
        SQL += " CUENTA ,NVL(ENTI_NCON_AF,0) CUENTACLIENTE ,NVL(ENTI_NUCO,0) CIF ,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X  FROM TNHT_NCRE,TNHT_ENTI,TNHT_TIVA,VH_NIVA"
        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI AND "
        SQL += " TNHT_NCRE.NCRE_CODI = VH_NIVA.NCRE_CODI    AND"
        SQL += " TNHT_NCRE.SEFA_CODI = VH_NIVA.SEFA_CODI"
        SQL += " AND VH_NIVA.TIVA = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " GROUP BY TNHT_NCRE.SEFA_CODI,TNHT_NCRE.NCRE_CODI,NCRE_VALO,TIVA_PERC,TNHT_NCRE.NCRE_DAEM,TIVA_CTB1,ENTI_NCON_AF,"
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
#Region "BONOS VENDIDOS NEWGOLF 80-83"
    Private Sub BonosEmitidos()

        Try

            ' Bonos PVP
            Dim Total As Double
            Dim TotalLiquido As Double
            Dim Descripcion As String
            Dim Cuenta As String

            Dim ComisionPvp As Double
            Dim ComisionLiquido As Double
            Dim ValorImpuesto As Double


            ' EXTRAER EL TOTAL LIQUIDO DE LOS BONOS DE LA CUENTA DE MANO CORRIENTE 485 BONOS  (ESTHER)

            SQL = "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI SERVICIO,TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CTB1,'0') CUENTA ,NVL(BLAL_DESC,'OTROS INGRESOS') AS BLOQUE,TNHT_BLAL.BLAL_CODI,"
            SQL += "ROUND(SUM(TNHT_MOVI.MOVI_VLIQ), 2) TOTAL "
            SQL += " FROM VNHT_MOVH TNHT_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_ADIC," & Me.mParaUsuarioNewGolf & ".TNPL_MOVI,TNHT_SERV"
            SQL += ",TNHT_ALOJ,TNHT_BLAL "
            SQL += " WHERE TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI"
            SQL += " AND TNHT_MOVI.MOVI_CODI = TNPL_MOVI.NEWH_CODI"
            SQL += " AND TNHT_MOVI.MOVI_DARE = TNPL_MOVI.NEWH_DARE"
            SQL += " AND TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI"

            SQL += " AND TNHT_MOVI.ALOJ_CODI = TNHT_ALOJ.ALOJ_CODI(+) "
            SQL += " AND TNHT_ALOJ.BLAL_CODI = TNHT_BLAL.BLAL_CODI(+) "

            SQL += " AND MOVI_DATR= '" & Me.mFecha & "'"
            SQL += " AND TNPL_MOVI.ADIC_CODI IN"
            SQL += "(SELECT ADIC_CODI FROM GMS.TNPL_ADIC WHERE ADIC_TIPO = 3)"
            SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,TNHT_SERV.SERV_COMS,BLAL_DESC,TNHT_BLAL.BLAL_CODI"
            SQL += " ORDER BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1"


            ' CONTRAPARTIDA POR DEPARTAMENTO  ESTHER (2)  ( BUSCA EL DPTO DE NEWHOTEL EN NEWGOLF POR PROBLEMA EN ENLACE  )

            SQL = "SELECT SUM(SIGN(MOVI_IMPT) *ABS(MOVI_VLIQ)) AS TOTAL, SUM(SIGN(MOVI_IMPT) *ABS(MOVI_IMP1)) AS IMPUESTO,"
            SQL += "TNHT_SERV.SERV_CODI AS SERVICIO, TNHT_SERV.SERV_DESC DEPARTAMENTO, NVL(TNHT_SERV.SERV_CTB1,   '0') CUENTA, NVL(BLAL_DESC,   'OTROS INGRESOS') AS BLOQUE "
            SQL += " FROM  " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_ADCO," & Me.mParaUsuarioNewGolf & ".TNPL_ADIC," & "TNHT_SERV, TNHT_ALOJ, TNHT_BLAL"

            SQL += " WHERE TNPL_MOVI.ADIC_CODI = TNPL_ADCO.ADIC_CODI "
            SQL += " AND TNPL_ADCO.SERV_CODI = TNHT_SERV.SERV_CODI"
            SQL += " AND TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI"
            SQL += " AND TNPL_MOVI.ALOJ_CODI = TNHT_ALOJ.ALOJ_CODI(+)"
            SQL += " AND TNHT_ALOJ.BLAL_CODI = TNHT_BLAL.BLAL_CODI(+)"

            SQL += " AND MOVI_FECH = '" & Me.mFecha & "'"
            SQL += " AND TNPL_MOVI.ADIC_CODI IN (SELECT ADIC_CODI FROM GMS.TNPL_ADIC WHERE ADIC_TIPO = 3)"

            SQL += " AND MOVI_ANUL = 0"

            ' EXCLUIR BONOS ASOCIACION DE CAMPOS 
            SQL += " AND TIAD_CODI <> " & Me.MparaTipoBonoAsociacion
            SQL += " GROUP BY TNHT_SERV.SERV_CODI, TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,BLAL_DESC"



            Linea = 0

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

                If Total <> 0 Then
                    Linea = Linea + 1
                    Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "PRODUCCIÓN BONOS " & Me.mFecha, Total, "SI", "", "", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaManoCorriente, Me.mIndicadorHaber, "PRODUCCIÓN BONOS " & Me.mFecha, Total)

                    Linea = Linea + 1
                    Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaBcta2, Me.mIndicadorHaber, "PRODUCCIÓN BONOS " & Me.mFecha, Total, "SI", "", "", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaBcta2, Me.mIndicadorHaber, "PRODUCCIÓN BONOS " & Me.mFecha, Total)

                End If

            End While
            Me.DbLeeHotel.mDbLector.Close()



            ' TOTAL PVP 
            ' SQL = ""
            'SQL += "SELECT TNPL_BONO.BONO_CODI, TNPL_BONO.BONO_ANCI, TNPL_BONO.BONO_DAEM, "
            'SQL += "TNPL_BONO.BONO_CANT, TNPL_BONO.BONO_PREC AS TOTAL, TNPL_BONO.BONO_JUGA, "
            'SQL += "TNPL_BONO.BONO_DAFI, TNPL_ADIC.ADIC_DESC, TNPL_TIVA.TIVA_PERC, "
            'SQL += "TNPL_BONO.TICK_CODI, TNPL_BONO.TICK_ANCI, NVL(TNPL_BONO.BONO_NAME,'?') BONO_NAME, "
            'SQL += "NVL(TNPL_BONO.BONO_APEL,'?') BONO_APEL,NVL(TNPL_ADIC.TIAD_CODI,'0')  AS TIPO  "
            'SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_BONO," & Me.mParaUsuarioNewGolf & ".TNPL_ADIC," & Me.mParaUsuarioNewGolf & ".TNPL_IMSE," & Me.mParaUsuarioNewGolf & ".TNPL_TIVA "
            'SQL += "WHERE ((TNPL_BONO.ADIC_CODI = TNPL_ADIC.ADIC_CODI) "
            'SQL += "AND (TNPL_ADIC.ADIC_CODI = TNPL_IMSE.ADIC_CODI) "
            'SQL += "AND (TNPL_IMSE.TIVA_COD1 = TNPL_TIVA.TIVA_CODI)) "
            'SQL += "AND TNPL_BONO.BONO_DAEM = " & "'" & Me.mFecha & "' "
            'SQL += " ORDER BY TNPL_BONO.BONO_CODI ASC "


            'Me.DbLeeHotel.TraerLector(SQL)



            'While Me.DbLeeHotel.mDbLector.Read

            'If Me.DbLeeHotel.mDbLector.Item("TIPO") <> Me.MparaTipoBonoAsociacion Then
            'Cuenta = Me.mCtaBcta1
            'Descripcion = "Bono    " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ADIC_DESC"), String) & " " & CType(Me.DbLeeHotel.mDbLector("BONO_APEL"), String) & "," & CType(Me.DbLeeHotel.mDbLector("BONO_NAME"), String)
            'Else
            'Cuenta = Me.mCtaBcta7
            'Descripcion = "Bono(*) " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ADIC_DESC"), String) & " " & CType(Me.DbLeeHotel.mDbLector("BONO_APEL"), String) & "," & CType(Me.DbLeeHotel.mDbLector("BONO_NAME"), String)
            'End If


            'Linea = Linea + 1
            'Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

            'Me.mTipoAsiento = "DEBE"
            'Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, Total, "NO", "", "Total PVP", "SI")
            'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Descripcion, Total)

            'End While
            'Me.DbLeeHotel.mDbLector.Close()



            ' Valor Liquido del Bono emitido (1) 

            SQL = ""
            SQL += "SELECT TNPL_BONO.BONO_CODI,TNPL_BONO.BONO_ANCI,MOVI_VLIQ AS LIQUIDO,ADIC_DESC,NVL(TNPL_ADIC.TIAD_CODI,'0')  AS TIPO, "
            SQL += " NVL(MOVI_DESC,0) AS DESCUENTO "

            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_DPTO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RECU,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADIC,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RESE,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_BONO "

            SQL += " WHERE (TNPL_MOVI.DPTO_CODI = TNPL_DPTO.DPTO_CODI(+))"
            SQL += " AND(TNPL_MOVI.RECU_CODI = TNPL_RECU.RECU_CODI(+))"
            SQL += " AND(TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI(+))"
            SQL += " AND(TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI(+))"
            SQL += " AND(TNPL_MOVI.RESE_CODI = TNPL_RESE.RESE_CODI(+))"
            SQL += " AND(TNPL_MOVI.RESE_ANCI = TNPL_RESE.RESE_ANCI(+))"

            ' SOLO MOVIMIENTOS DE BONOS EN EL TIKE
            SQL += " AND TNPL_ADIC.ADIC_TIPO = 3 "

            SQL += " AND TNPL_MOVI.TICK_CODI = TNPL_BONO.TICK_CODI"
            SQL += " AND TNPL_MOVI.TICK_ANCI = TNPL_BONO.TICK_ANCI"
            SQL += " AND TNPL_MOVI.CAJA_CODI = TNPL_BONO.CAJA_CODI"
            SQL += " AND(TNPL_MOVI.TICK_CODI,   TNPL_MOVI.TICK_ANCI,   TNPL_MOVI.CAJA_CODI) IN  (SELECT TICK_CODI,TICK_ANCI,CAJA_CODI FROM " & Me.mParaUsuarioNewGolf & ".TNPL_BONO)"
            SQL += " AND NVL(TNPL_MOVI.MOVI_NEWH,'0') <> '2'"
            SQL += " AND MOVI_FECH = " & "'" & Me.mFecha & "' "
            SQL += " AND MOVI_TIPO = 1 "

            ' EXCLUIR BONOS ASOCIACION DE CAMPOS 
            SQL += " AND TIAD_CODI <> " & Me.MparaTipoBonoAsociacion

            SQL += " ORDER BY TNPL_BONO.BONO_CODI ASC "

            Me.DbLeeHotel.TraerLector(SQL)



            While Me.DbLeeHotel.mDbLector.Read


                If Me.DbLeeHotel.mDbLector.Item("TIPO") <> Me.MparaTipoBonoAsociacion Then
                    Cuenta = Me.mCtaManoCorriente
                    Descripcion = "Bono    " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ADIC_DESC"), String)
                Else
                    Cuenta = "???"
                    Descripcion = "Bono(*) " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ADIC_DESC"), String)
                End If

                Linea = Linea + 1
                TotalLiquido = CType(Me.DbLeeHotel.mDbLector("LIQUIDO"), Double) * -1

                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Descripcion, TotalLiquido, "NO", "", "Total Líquido", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Descripcion, TotalLiquido)

                ' si se aplico descuento 
                'If CType(Me.DbLeeHotel.mDbLector("DESCUENTO"), Double) <> 0 Then
                'Linea = Linea + 1
                'TotalDescuento = CType(Me.DbLeeHotel.mDbLector("DESCUENTO"), Double) * -1

                'Me.mTipoAsiento = "DEBE"
                'Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, TotalDescuento, "NO", "", "Total Descuento", "SI")
                'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Descripcion, TotalDescuento)

                'End If


            End While
            Me.DbLeeHotel.mDbLector.Close()



            ' Valor Liquido del Bono emitido (2 ) 

            SQL = ""
            SQL += "SELECT TNPL_BONO.BONO_CODI,TNPL_BONO.BONO_ANCI,MOVI_VLIQ AS LIQUIDO,ADIC_DESC,NVL(TNPL_ADIC.TIAD_CODI,'0')  AS TIPO, "
            SQL += " NVL(MOVI_DESC,0) AS DESCUENTO "

            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_DPTO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RECU,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADIC,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RESE,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_BONO "

            SQL += " WHERE (TNPL_MOVI.DPTO_CODI = TNPL_DPTO.DPTO_CODI(+))"
            SQL += " AND(TNPL_MOVI.RECU_CODI = TNPL_RECU.RECU_CODI(+))"
            SQL += " AND(TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI(+))"
            SQL += " AND(TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI(+))"
            SQL += " AND(TNPL_MOVI.RESE_CODI = TNPL_RESE.RESE_CODI(+))"
            SQL += " AND(TNPL_MOVI.RESE_ANCI = TNPL_RESE.RESE_ANCI(+))"

            ' SOLO MOVIMIENTOS DE BONOS EN EL TIKE
            SQL += " AND TNPL_ADIC.ADIC_TIPO = 3 "

            SQL += " AND TNPL_MOVI.TICK_CODI = TNPL_BONO.TICK_CODI"
            SQL += " AND TNPL_MOVI.TICK_ANCI = TNPL_BONO.TICK_ANCI"
            SQL += " AND TNPL_MOVI.CAJA_CODI = TNPL_BONO.CAJA_CODI"
            SQL += " AND(TNPL_MOVI.TICK_CODI,   TNPL_MOVI.TICK_ANCI,   TNPL_MOVI.CAJA_CODI) IN  (SELECT TICK_CODI,TICK_ANCI,CAJA_CODI FROM " & Me.mParaUsuarioNewGolf & ".TNPL_BONO)"
            SQL += " AND NVL(TNPL_MOVI.MOVI_NEWH,'0') <> '2'"
            SQL += " AND MOVI_FECH = " & "'" & Me.mFecha & "' "
            SQL += " AND MOVI_TIPO = 1 "
            ' EXCLUIR BONOS ASOCIACION DE CAMPOS 
            SQL += " AND TIAD_CODI <> " & Me.MparaTipoBonoAsociacion

            SQL += " ORDER BY TNPL_BONO.BONO_CODI ASC "

            Me.DbLeeHotel.TraerLector(SQL)



            While Me.DbLeeHotel.mDbLector.Read


                If Me.DbLeeHotel.mDbLector.Item("TIPO") <> Me.MparaTipoBonoAsociacion Then
                    Cuenta = Me.mCtaBcta2
                    Descripcion = "Bono    " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ADIC_DESC"), String)
                Else
                    Cuenta = Me.mCtaBcta8
                    Descripcion = "Bono(*) " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ADIC_DESC"), String)
                End If

                Linea = Linea + 1
                TotalLiquido = CType(Me.DbLeeHotel.mDbLector("LIQUIDO"), Double)

                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Descripcion, TotalLiquido, "NO", "", "Total Líquido", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Descripcion, TotalLiquido)

                ' si se aplico descuento 
                'If CType(Me.DbLeeHotel.mDbLector("DESCUENTO"), Double) <> 0 Then
                'Linea = Linea + 1
                'TotalDescuento = CType(Me.DbLeeHotel.mDbLector("DESCUENTO"), Double) * -1

                'Me.mTipoAsiento = "DEBE"
                'Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, TotalDescuento, "NO", "", "Total Descuento", "SI")
                'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Descripcion, TotalDescuento)

                'End If


            End While
            Me.DbLeeHotel.mDbLector.Close()


            ' Valor del Impuesto del Bono emitido solo bonos salobre 

            SQL = ""
            SQL += "SELECT TNPL_BONO.BONO_CODI,TNPL_BONO.BONO_ANCI,MOVI_VLIQ AS LIQUIDO,NVL(MOVI_IMP1,0) AS IMPUESTO "
            SQL += ",MOVI_IMPT AS TOTAL,NVL(ADIC_DESC,'?') AS ADIC_DESC, "
            SQL += "NVL(TIVA_CTB1,'0') AS TIVA_CTB1,NVL(TNPL_ADIC.TIAD_CODI,'0')  AS TIPO "
            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_DPTO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RECU,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADIC,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RESE,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_BONO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_TIVA"


            SQL += " WHERE (TNPL_MOVI.DPTO_CODI = TNPL_DPTO.DPTO_CODI(+))"
            SQL += " AND(TNPL_MOVI.RECU_CODI = TNPL_RECU.RECU_CODI(+))"
            SQL += " AND(TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI(+))"
            SQL += " AND(TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI(+))"
            SQL += " AND(TNPL_MOVI.RESE_CODI = TNPL_RESE.RESE_CODI(+))"
            SQL += " AND(TNPL_MOVI.RESE_ANCI = TNPL_RESE.RESE_ANCI(+))"
            SQL += " AND(TNPL_MOVI.MOVI_IVA1 = TNPL_TIVA.TIVA_CODI(+))"

            ' SOLO MOVIMIENTOS DE BONOS EN EL TIKE
            SQL += " AND TNPL_ADIC.ADIC_TIPO = 3 "

            SQL += " AND TNPL_MOVI.TICK_CODI = TNPL_BONO.TICK_CODI"
            SQL += " AND TNPL_MOVI.TICK_ANCI = TNPL_BONO.TICK_ANCI"
            SQL += " AND TNPL_MOVI.CAJA_CODI = TNPL_BONO.CAJA_CODI"
            SQL += " AND(TNPL_MOVI.TICK_CODI,   TNPL_MOVI.TICK_ANCI,   TNPL_MOVI.CAJA_CODI) IN  (SELECT TICK_CODI,TICK_ANCI,CAJA_CODI FROM " & Me.mParaUsuarioNewGolf & ".TNPL_BONO)"
            SQL += " AND NVL(TNPL_MOVI.MOVI_NEWH,'0') <> '2'"
            SQL += " AND MOVI_FECH = " & "'" & Me.mFecha & "' "
            SQL += " AND MOVI_TIPO = 1 "

            ' EXCLUIR BONOS ASOCIACION DE CAMPOS 
            SQL += " AND TIAD_CODI <> " & Me.MparaTipoBonoAsociacion

            SQL += " ORDER BY TNPL_BONO.BONO_CODI ASC "


            Me.DbLeeHotel.TraerLector(SQL)



            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("IMPUESTO"), Double)

                Cuenta = CType(Me.DbLeeHotel.mDbLector("TIVA_CTB1"), Double)

                If Me.DbLeeHotel.mDbLector.Item("TIPO") <> Me.MparaTipoBonoAsociacion Then
                    If Total <> 0 Then
                        Descripcion = "Bono    " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ADIC_DESC"), String)
                        Me.mTipoAsiento = "HABER"
                        '              Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Descripcion, Total, "NO", "", "Total Impuesto", "SI")
                        '              Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Descripcion, Total)
                    End If
                End If



            End While
            Me.DbLeeHotel.mDbLector.Close()





            ' Valor comision bonos asociacion
            SQL = ""
            SQL += "SELECT TNPL_BONO.BONO_CODI,TNPL_BONO.BONO_ANCI,MOVI_VLIQ AS LIQUIDO,ADIC_DESC,NVL(TNPL_ADIC.TIAD_CODI,'0')  AS TIPO,"
            SQL += "TNPL_BONO.BONO_PREC AS PVP,NVL(MOVI_IMP1,0) AS IMPUESTO,NVL(TIVA_CTB1,'0') AS TIVA_CTB1 "

            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_DPTO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RECU,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADIC,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RESE,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_BONO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_TIVA "

            SQL += " WHERE (TNPL_MOVI.DPTO_CODI = TNPL_DPTO.DPTO_CODI(+))"
            SQL += " AND(TNPL_MOVI.RECU_CODI = TNPL_RECU.RECU_CODI(+))"
            SQL += " AND(TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI(+))"
            SQL += " AND(TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI(+))"
            SQL += " AND(TNPL_MOVI.RESE_CODI = TNPL_RESE.RESE_CODI(+))"
            SQL += " AND(TNPL_MOVI.RESE_ANCI = TNPL_RESE.RESE_ANCI(+))"
            SQL += " AND(TNPL_MOVI.MOVI_IVA1 = TNPL_TIVA.TIVA_CODI(+))"

            ' SOLO MOVIMIENTOS DE BONOS EN EL TIKE
            SQL += " AND TNPL_ADIC.ADIC_TIPO = 3 "

            SQL += " AND TNPL_MOVI.TICK_CODI = TNPL_BONO.TICK_CODI"
            SQL += " AND TNPL_MOVI.TICK_ANCI = TNPL_BONO.TICK_ANCI"
            SQL += " AND TNPL_MOVI.CAJA_CODI = TNPL_BONO.CAJA_CODI"
            SQL += " AND(TNPL_MOVI.TICK_CODI,   TNPL_MOVI.TICK_ANCI,   TNPL_MOVI.CAJA_CODI) IN  (SELECT TICK_CODI,TICK_ANCI,CAJA_CODI FROM " & Me.mParaUsuarioNewGolf & ".TNPL_BONO)"
            SQL += " AND NVL(TNPL_MOVI.MOVI_NEWH,'0') <> '2'"
            SQL += " AND MOVI_FECH = " & "'" & Me.mFecha & "' "
            SQL += " AND MOVI_TIPO = 1 "
            SQL += " ORDER BY TNPL_BONO.BONO_CODI ASC "

            Me.DbLeeHotel.TraerLector(SQL)



            While Me.DbLeeHotel.mDbLector.Read


                If Me.DbLeeHotel.mDbLector.Item("TIPO") = Me.MparaTipoBonoAsociacion Then

                    Descripcion = "Bono(*) Comisión " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ADIC_DESC"), String)


                    Linea = Linea + 1
                    ComisionPvp = (CType(Me.DbLeeHotel.mDbLector("PVP"), Double) * Me.MparaComisionBonoAsociacion) / 100
                    Cuenta = Me.mCtaBcta9

                    Me.mTipoAsiento = "DEBE"
                    '   Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, ComisionPvp, "NO", "", "Total Comisión PVP", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Descripcion, ComisionPvp)

                    Linea = Linea + 1
                    ComisionLiquido = (CType(Me.DbLeeHotel.mDbLector("LIQUIDO"), Double) * Me.MparaComisionBonoAsociacion) / 100
                    Cuenta = Me.mCtaBcta10


                    Me.mTipoAsiento = "HABER"
                    '  Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Descripcion, ComisionLiquido, "NO", "", "Total Comisión Líquido", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Descripcion, ComisionLiquido)

                    Linea = Linea + 1
                    ValorImpuesto = CType(Me.DbLeeHotel.mDbLector("IMPUESTO"), Double)
                    Cuenta = Me.DbLeeHotel.mDbLector.Item("TIVA_CTB1")

                    If ValorImpuesto <> 0 Then
                        Me.mTipoAsiento = "HABER"
                        '     Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Descripcion, ValorImpuesto, "NO", "", "Total Igic", "SI")
                        Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Descripcion, ValorImpuesto)
                    End If

                End If

            End While
            Me.DbLeeHotel.mDbLector.Close()

        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try
    End Sub
    Private Sub BonosEmitidosAnulados()

        Try

            ' Bonos PVP
            Dim Total As Double
            Dim TotalLiquido As Double
            Dim TotalDescuento As Double
            Dim Descripcion As String
            Dim Cuenta As String

            Dim ComisionPvp As Double
            Dim ComisionLiquido As Double
            Dim ValorImpuesto As Double


            SQL = ""
            SQL += "SELECT TNPL_BONO.BONO_CODI, TNPL_BONO.BONO_ANCI, TNPL_BONO.BONO_DAEM, "
            SQL += "TNPL_BONO.BONO_CANT, TNPL_BONO.BONO_PREC AS TOTAL, TNPL_BONO.BONO_JUGA, "
            SQL += "TNPL_BONO.BONO_DAFI, TNPL_ADIC.ADIC_DESC, TNPL_TIVA.TIVA_PERC, "
            SQL += "TNPL_BONO.TICK_CODI, TNPL_BONO.TICK_ANCI, NVL(TNPL_BONO.BONO_NAME,'?') BONO_NAME, "
            SQL += "NVL(TNPL_BONO.BONO_APEL,'?') BONO_APEL,NVL(TNPL_ADIC.TIAD_CODI,'0')  AS TIPO  "
            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_BONO," & Me.mParaUsuarioNewGolf & ".TNPL_ADIC," & Me.mParaUsuarioNewGolf & ".TNPL_IMSE," & Me.mParaUsuarioNewGolf & ".TNPL_TIVA "
            SQL += "WHERE ((TNPL_BONO.ADIC_CODI = TNPL_ADIC.ADIC_CODI) "
            SQL += "AND (TNPL_ADIC.ADIC_CODI = TNPL_IMSE.ADIC_CODI) "
            SQL += "AND (TNPL_IMSE.TIVA_COD1 = TNPL_TIVA.TIVA_CODI)) "
            SQL += "AND TNPL_BONO.BONO_DAAN = " & "'" & Me.mFecha & "' "
            SQL += " ORDER BY TNPL_BONO.BONO_CODI ASC "


            Me.DbLeeHotel.TraerLector(SQL)

            '  Linea = 0

            While Me.DbLeeHotel.mDbLector.Read

                If Me.DbLeeHotel.mDbLector.Item("TIPO") <> Me.MparaTipoBonoAsociacion Then
                    Cuenta = Me.mCtaBcta1
                    Descripcion = "Bono    Anulado " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ADIC_DESC"), String) & " " & CType(Me.DbLeeHotel.mDbLector("BONO_APEL"), String) & "," & CType(Me.DbLeeHotel.mDbLector("BONO_NAME"), String)
                Else
                    Cuenta = Me.mCtaBcta7
                    Descripcion = "Bono(*) Anulado " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ADIC_DESC"), String) & " " & CType(Me.DbLeeHotel.mDbLector("BONO_APEL"), String) & "," & CType(Me.DbLeeHotel.mDbLector("BONO_NAME"), String)
                End If


                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1

                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, Total, "NO", "", "Total PVP", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Descripcion, Total)

            End While
            Me.DbLeeHotel.mDbLector.Close()


            ' Valor Liquido del Bono emitido

            SQL = ""
            SQL += "SELECT TNPL_BONO.BONO_CODI,TNPL_BONO.BONO_ANCI,MOVI_VLIQ AS LIQUIDO,ADIC_DESC,NVL(TNPL_ADIC.TIAD_CODI,'0')  AS TIPO, "
            SQL += " NVL(MOVI_DESC,0) AS DESCUENTO "

            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_DPTO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RECU,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADIC,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RESE,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_BONO "

            SQL += " WHERE (TNPL_MOVI.DPTO_CODI = TNPL_DPTO.DPTO_CODI(+))"
            SQL += " AND(TNPL_MOVI.RECU_CODI = TNPL_RECU.RECU_CODI(+))"
            SQL += " AND(TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI(+))"
            SQL += " AND(TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI(+))"
            SQL += " AND(TNPL_MOVI.RESE_CODI = TNPL_RESE.RESE_CODI(+))"
            SQL += " AND(TNPL_MOVI.RESE_ANCI = TNPL_RESE.RESE_ANCI(+))"

            ' SOLO MOVIMIENTOS DE BONOS EN EL TIKE
            SQL += " AND TNPL_ADIC.ADIC_TIPO = 3 "

            SQL += " AND TNPL_MOVI.TICK_CODI = TNPL_BONO.TICK_CODI"
            SQL += " AND TNPL_MOVI.TICK_ANCI = TNPL_BONO.TICK_ANCI"
            SQL += " AND TNPL_MOVI.CAJA_CODI = TNPL_BONO.CAJA_CODI"
            SQL += " AND(TNPL_MOVI.TICK_CODI,   TNPL_MOVI.TICK_ANCI,   TNPL_MOVI.CAJA_CODI) IN  (SELECT TICK_CODI,TICK_ANCI,CAJA_CODI FROM " & Me.mParaUsuarioNewGolf & ".TNPL_BONO)"
            SQL += " AND NVL(TNPL_MOVI.MOVI_NEWH,'0') <> '2'"
            '  SQL += " AND MOVI_FECH = " & "'" & Me.mFecha & "' "
            SQL += " AND MOVI_FECH = BONO_DAAN "
            SQL += " AND MOVI_TIPO = 1 "
            SQL += " ORDER BY TNPL_BONO.BONO_CODI ASC "

            Me.DbLeeHotel.TraerLector(SQL)



            While Me.DbLeeHotel.mDbLector.Read


                If Me.DbLeeHotel.mDbLector.Item("TIPO") <> Me.MparaTipoBonoAsociacion Then
                    Cuenta = Me.mCtaBcta2
                    Descripcion = "Bono    " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ADIC_DESC"), String)
                Else
                    Cuenta = Me.mCtaBcta8
                    Descripcion = "Bono(*) " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ADIC_DESC"), String)
                End If

                Linea = Linea + 1
                TotalLiquido = CType(Me.DbLeeHotel.mDbLector("LIQUIDO"), Double)

                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Descripcion, TotalLiquido, "NO", "", "Total Líquido", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Descripcion, TotalLiquido)

                ' si se aplico descuento 
                If CType(Me.DbLeeHotel.mDbLector("DESCUENTO"), Double) <> 0 Then
                    Linea = Linea + 1
                    TotalDescuento = CType(Me.DbLeeHotel.mDbLector("DESCUENTO"), Double) * -1

                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, TotalDescuento, "NO", "", "Total Descuento", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Descripcion, TotalDescuento)

                End If


            End While
            Me.DbLeeHotel.mDbLector.Close()


            ' Valor del Impuesto del Bono emitido solo bonos salobre 

            SQL = ""
            SQL += "SELECT TNPL_BONO.BONO_CODI,TNPL_BONO.BONO_ANCI,MOVI_VLIQ AS LIQUIDO,NVL(MOVI_IMP1,0) AS IMPUESTO "
            SQL += ",MOVI_IMPT AS TOTAL,NVL(ADIC_DESC,'?') AS ADIC_DESC, "
            SQL += "NVL(TIVA_CTB1,'0') AS TIVA_CTB1,NVL(TNPL_ADIC.TIAD_CODI,'0')  AS TIPO "
            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_DPTO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RECU,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADIC,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RESE,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_BONO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_TIVA"


            SQL += " WHERE (TNPL_MOVI.DPTO_CODI = TNPL_DPTO.DPTO_CODI(+))"
            SQL += " AND(TNPL_MOVI.RECU_CODI = TNPL_RECU.RECU_CODI(+))"
            SQL += " AND(TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI(+))"
            SQL += " AND(TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI(+))"
            SQL += " AND(TNPL_MOVI.RESE_CODI = TNPL_RESE.RESE_CODI(+))"
            SQL += " AND(TNPL_MOVI.RESE_ANCI = TNPL_RESE.RESE_ANCI(+))"
            SQL += " AND(TNPL_MOVI.MOVI_IVA1 = TNPL_TIVA.TIVA_CODI(+))"

            ' SOLO MOVIMIENTOS DE BONOS EN EL TIKE
            SQL += " AND TNPL_ADIC.ADIC_TIPO = 3 "

            SQL += " AND TNPL_MOVI.TICK_CODI = TNPL_BONO.TICK_CODI"
            SQL += " AND TNPL_MOVI.TICK_ANCI = TNPL_BONO.TICK_ANCI"
            SQL += " AND TNPL_MOVI.CAJA_CODI = TNPL_BONO.CAJA_CODI"
            SQL += " AND(TNPL_MOVI.TICK_CODI,   TNPL_MOVI.TICK_ANCI,   TNPL_MOVI.CAJA_CODI) IN  (SELECT TICK_CODI,TICK_ANCI,CAJA_CODI FROM " & Me.mParaUsuarioNewGolf & ".TNPL_BONO)"
            SQL += " AND NVL(TNPL_MOVI.MOVI_NEWH,'0') <> '2'"
            'SQL += " AND MOVI_FECH = " & "'" & Me.mFecha & "' "
            SQL += " AND MOVI_FECH = BONO_DAAN "

            SQL += " AND MOVI_TIPO = 1 "
            SQL += " ORDER BY TNPL_BONO.BONO_CODI ASC "


            Me.DbLeeHotel.TraerLector(SQL)



            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("IMPUESTO"), Double)

                Cuenta = CType(Me.DbLeeHotel.mDbLector("TIVA_CTB1"), Double)

                If Me.DbLeeHotel.mDbLector.Item("TIPO") <> Me.MparaTipoBonoAsociacion Then
                    If Total <> 0 Then
                        Descripcion = "Bono    " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ADIC_DESC"), String)
                        Me.mTipoAsiento = "HABER"
                        Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Descripcion, Total, "NO", "", "Total Impuesto", "SI")
                        Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Descripcion, Total)
                    End If
                End If



            End While
            Me.DbLeeHotel.mDbLector.Close()





            ' Valor comision bonos asociacion
            SQL = ""
            SQL += "SELECT TNPL_BONO.BONO_CODI,TNPL_BONO.BONO_ANCI,MOVI_VLIQ AS LIQUIDO,ADIC_DESC,NVL(TNPL_ADIC.TIAD_CODI,'0')  AS TIPO,"
            SQL += "TNPL_BONO.BONO_PREC AS PVP,NVL(MOVI_IMP1,0) AS IMPUESTO "

            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_DPTO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RECU,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADIC,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RESE,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_BONO "

            SQL += " WHERE (TNPL_MOVI.DPTO_CODI = TNPL_DPTO.DPTO_CODI(+))"
            SQL += " AND(TNPL_MOVI.RECU_CODI = TNPL_RECU.RECU_CODI(+))"
            SQL += " AND(TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI(+))"
            SQL += " AND(TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI(+))"
            SQL += " AND(TNPL_MOVI.RESE_CODI = TNPL_RESE.RESE_CODI(+))"
            SQL += " AND(TNPL_MOVI.RESE_ANCI = TNPL_RESE.RESE_ANCI(+))"

            ' SOLO MOVIMIENTOS DE BONOS EN EL TIKE
            SQL += " AND TNPL_ADIC.ADIC_TIPO = 3 "

            SQL += " AND TNPL_MOVI.TICK_CODI = TNPL_BONO.TICK_CODI"
            SQL += " AND TNPL_MOVI.TICK_ANCI = TNPL_BONO.TICK_ANCI"
            SQL += " AND TNPL_MOVI.CAJA_CODI = TNPL_BONO.CAJA_CODI"
            SQL += " AND(TNPL_MOVI.TICK_CODI,   TNPL_MOVI.TICK_ANCI,   TNPL_MOVI.CAJA_CODI) IN  (SELECT TICK_CODI,TICK_ANCI,CAJA_CODI FROM " & Me.mParaUsuarioNewGolf & ".TNPL_BONO)"
            SQL += " AND NVL(TNPL_MOVI.MOVI_NEWH,'0') <> '2'"
            'SQL += " AND MOVI_FECH = " & "'" & Me.mFecha & "' "
            SQL += " AND MOVI_FECH = BONO_DAAN "

            SQL += " AND MOVI_TIPO = 1 "
            SQL += " ORDER BY TNPL_BONO.BONO_CODI ASC "

            Me.DbLeeHotel.TraerLector(SQL)



            While Me.DbLeeHotel.mDbLector.Read


                If Me.DbLeeHotel.mDbLector.Item("TIPO") = Me.MparaTipoBonoAsociacion Then

                    Descripcion = "Bono(*) Comisión " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ADIC_DESC"), String)


                    Linea = Linea + 1
                    ComisionPvp = (CType(Me.DbLeeHotel.mDbLector("PVP"), Double) * Me.MparaComisionBonoAsociacion) / 100
                    Cuenta = Me.mCtaBcta9

                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, ComisionPvp, "NO", "", "Total Comisión PVP", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Descripcion, ComisionPvp)

                    Linea = Linea + 1
                    ComisionLiquido = (CType(Me.DbLeeHotel.mDbLector("LIQUIDO"), Double) * Me.MparaComisionBonoAsociacion) / 100
                    Cuenta = Me.mCtaBcta10


                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Descripcion, ComisionLiquido, "NO", "", "Total Comisión Líquido", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Descripcion, ComisionLiquido)

                    Linea = Linea + 1
                    ValorImpuesto = CType(Me.DbLeeHotel.mDbLector("IMPUESTO"), Double)
                    Cuenta = "?"


                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 80, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Descripcion, ValorImpuesto, "NO", "", "Total Igic", "SI")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Descripcion, ValorImpuesto)

                End If

            End While
            Me.DbLeeHotel.mDbLector.Close()

        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try
    End Sub
    Private Sub BonosConsumoJugada()

        Try

            Dim Total As Double

            Dim TotalJugadaLiquido As Double
            Dim TotalJugadaPvp As Double

            Dim Descripcion As String
            Dim Descripcion2 As String
            Dim ValorImpuesto As Double

            Dim Tiket As String


            Dim Cuenta As String

            Dim vCentroCosto As String

            Dim TipoProduccion As String

            Dim CodigoCompuesto As String

            SQL = ""
            SQL += "SELECT   tnpl_bono.bono_codi,tnpl_bono.bono_anci,NVL(TNPL_ADIC.TIAD_CODI,'0')  AS TIPO,BONO_PREC AS PVP ,"
            SQL += " TNPL_TICK.TICK_CODI,TNPL_TICK.TICK_ANCI,TNPL_TICK.TICK_ANUL,NVL(TNPL_TICK.TICK_DAAN,'01/09/9999')AS TICK_DAAN,TICK_DATE"
            SQL += " FROM " & Me.mParaUsuarioNewGolf & ".tnpl_movi,"
            SQL += Me.mParaUsuarioNewGolf & ".tnpl_dpto,"
            SQL += Me.mParaUsuarioNewGolf & ".tnpl_recu,"
            SQL += Me.mParaUsuarioNewGolf & ".tnpl_adic,"
            SQL += Me.mParaUsuarioNewGolf & ".tnpl_fpag,"
            SQL += Me.mParaUsuarioNewGolf & ".tnpl_rese,"
            SQL += Me.mParaUsuarioNewGolf & ".tnpl_bomo,"
            SQL += Me.mParaUsuarioNewGolf & ".tnpl_bono,"
            SQL += Me.mParaUsuarioNewGolf & ".tnpl_TICK "

            SQL += "WHERE (tnpl_movi.dpto_codi = tnpl_dpto.dpto_codi(+)) "
            SQL += "AND (tnpl_movi.recu_codi = tnpl_recu.recu_codi(+)) "
            ' SQL += "AND (tnpl_movi.adic_codi = tnpl_adic.adic_codi(+)) "
            SQL += "AND (tnpl_bono.adic_codi = tnpl_adic.adic_codi(+)) "
            SQL += "AND (tnpl_movi.fpag_codi = tnpl_fpag.fpag_codi(+)) "
            SQL += "AND (tnpl_movi.rese_codi = tnpl_rese.rese_codi(+)) "
            SQL += "AND (tnpl_movi.rese_anci = tnpl_rese.rese_anci(+)) "



            ' SOLO MOVIMIENTOS DE BONOS EN EL TIKE
            SQL += " AND TNPL_ADIC.ADIC_TIPO = 3 "


            SQL += " AND TNPL_MOVI.TICK_CODI = TNPL_TICK.TICK_CODI "
            SQL += " AND TNPL_MOVI.TICK_ANCI = TNPL_TICK.TICK_ANCI "
            SQL += " AND TNPL_MOVI.CAJA_CODI = TNPL_TICK.CAJA_CODI "

            SQL += "and tnpl_movi.movi_codi = tnpl_bomo.movi_codi "
            SQL += "and tnpl_movi.movi_date = tnpl_bomo.movi_date "
            SQL += "and  tnpl_bomo.bono_codi = tnpl_bono.bono_codi "
            SQL += "and  tnpl_bomo.bono_anci = tnpl_bono.bono_anci "
            SQL += "AND ((tnpl_movi.movi_codi, tnpl_movi.movi_date) IN (SELECT movi_codi, movi_date FROM " & Me.mParaUsuarioNewGolf & ".tnpl_bomo)) "

            SQL += "AND (MOVI_FECH = " & "'" & Me.mFecha & "' OR TICK_DAAN = " & "'" & Me.mFecha & "')"
            SQL += "AND MOVI_TIPO = 2 "
            SQL += "AND NVL (tnpl_movi.movi_newh, '0') <> '2' "
            ' EXCLUIR BONOS ASOCIACION DE CAMPOS 
            SQL += " AND TIAD_CODI <> " & Me.MparaTipoBonoAsociacion

            SQL += " ORDER BY TNPL_BONO.BONO_CODI ASC "

            Me.DbLeeHotel.TraerLector(SQL)




            Linea = 0

            While Me.DbLeeHotel.mDbLector.Read

                Tiket = Me.DbLeeHotel.mDbLector.Item("TICK_CODI") & "/" & Me.DbLeeHotel.mDbLector.Item("TICK_ANCI")



                ' Averiguar el Valor Liquido del valor total del Bono 


                SQL = "SELECT TNPL_BONO.BONO_CODI,TNPL_BONO.BONO_ANCI,MOVI_VLIQ AS LIQUIDO,TNPL_BONO.BONO_PREC AS PRECIO,ADIC_DESC,NVL(TNPL_ADIC.TIAD_CODI,   '0') AS TIPO,"
                SQL += " NVL(MOVI_DESC,   0) AS DESCUENTO,BONO_CANT,NVL(MOVI_IMP1,0) AS IMPUESTO, "
                SQL += " NVL(TNPL_ADCO.SERV_CODI,'?') AS SERVICIO "
                SQL += "FROM "
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_MOVI, "
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_DPTO,"
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_RECU,"
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADIC,"
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_RESE,"
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_BONO,"
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADCO "
                SQL += "WHERE(TNPL_MOVI.DPTO_CODI = TNPL_DPTO.DPTO_CODI(+)) "
                SQL += "AND(TNPL_MOVI.RECU_CODI = TNPL_RECU.RECU_CODI(+)) "
                SQL += "AND(TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI(+)) "
                SQL += "AND(TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI(+)) "
                SQL += "AND(TNPL_MOVI.RESE_CODI = TNPL_RESE.RESE_CODI(+)) "
                SQL += "AND(TNPL_MOVI.RESE_ANCI = TNPL_RESE.RESE_ANCI(+)) "
                SQL += "AND(TNPL_MOVI.ADIC_CODI = TNPL_ADCO.ADIC_CODI(+)) "
                SQL += "AND TNPL_ADIC.ADIC_TIPO = 3 "
                SQL += "AND TNPL_MOVI.TICK_CODI = TNPL_BONO.TICK_CODI "
                SQL += "AND TNPL_MOVI.TICK_ANCI = TNPL_BONO.TICK_ANCI "
                SQL += "AND TNPL_MOVI.CAJA_CODI = TNPL_BONO.CAJA_CODI "
                SQL += "AND(TNPL_MOVI.TICK_CODI,TNPL_MOVI.TICK_ANCI,TNPL_MOVI.CAJA_CODI) IN "
                SQL += "(SELECT TICK_CODI,TICK_ANCI,CAJA_CODI "
                SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_BONO) "
                SQL += "AND NVL(TNPL_MOVI.MOVI_NEWH,   '0') <> '2' "
                SQL += "AND MOVI_TIPO = 1 "
                SQL += " AND TNPL_BONO.BONO_CODI = " & Me.DbLeeHotel.mDbLector.Item("BONO_CODI")
                SQL += " AND TNPL_BONO.BONO_ANCI = " & Me.DbLeeHotel.mDbLector.Item("BONO_ANCI")

                SQL += " ORDER BY TNPL_BONO.BONO_CODI ASC"



                Me.DbLeeHotelAux.TraerLector(SQL)
                Me.DbLeeHotelAux.mDbLector.Read()
                If Me.DbLeeHotelAux.mDbLector.HasRows Then


                    If IsDBNull(Me.DbLeeHotelAux.mDbLector("SERVICIO")) = False Then
                        SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotelAux.mDbLector("SERVICIO"), String) & "'"
                        vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    Else
                        If mMostrarMensajes = True Then
                            MsgBox("Atención Bono sin Centro de Costo Asociado" & vbCrLf & "Revise Sección/Servicio en NewGolf" & vbCrLf & Me.DbLeeHotelAux.mDbLector("ADIC_DESC"), MsgBoxStyle.Exclamation, "Atención")
                        End If
                        vCentroCosto = "0"
                    End If



                    If Me.DbLeeHotelAux.mDbLector("SERVICIO") = Me.mParaServCodiBonos Then
                        TipoProduccion = "BONOS"
                    ElseIf Me.DbLeeHotelAux.mDbLector("SERVICIO") = Me.mParaServCodiBonosAsoc Then
                        ' Pat0006
                        ' TipoProduccion = "BONOS ASOC"
                        TipoProduccion = "NORMAL"

                    Else
                        TipoProduccion = "NORMAL"
                    End If


                    If Me.mParaIngresoPorHabitacion = 1 Then
                        CodigoCompuesto = "000-" & CType(Me.DbLeeHotelAux.mDbLector("SERVICIO"), String)
                    Else
                        CodigoCompuesto = Me.DbLeeHotelAux.mDbLector("SERVICIO")
                    End If


                    If Me.DbLeeHotel.mDbLector.Item("TIPO") <> Me.MparaTipoBonoAsociacion Then

                        ' CONSUMO BONOS NORMALES
                        Total = CType(Me.DbLeeHotelAux.mDbLector.Item("LIQUIDO"), Double) / CType(Me.DbLeeHotelAux.mDbLector.Item("BONO_CANT"), Integer)
                        Descripcion2 = " " & CType(Me.DbLeeHotelAux.mDbLector("ADIC_DESC"), String) & " Jug. Compradas = " & CType(Me.DbLeeHotelAux.mDbLector("BONO_CANT"), Integer) & " Pvp =  " & CType(Me.DbLeeHotelAux.mDbLector("PRECIO"), Double)


                        ' TIKET EMITIDOS
                        If Me.DbLeeHotel.mDbLector.Item("TICK_DATE") = Me.mFecha Then
                            If Total <> 0 Then
                                Descripcion = "Bono Consumido " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " Ticket = " & Tiket


                                Linea = Linea + 1
                                Cuenta = Me.mCtaBcta3

                                Me.mTipoAsiento = "DEBE"
                                Me.InsertaOracle("AC", 81, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, Total, "NO", "", Descripcion2, "SI")
                                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Descripcion, Total)

                                Linea = Linea + 1
                                Cuenta = Me.mCtaBcta4

                                Me.mTipoAsiento = "HABER"
                                Me.InsertaOracle("AC", 81, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Descripcion, Total, "NO", "", vCentroCosto, "SI", "", "SAT_NHCreateProdOrdersQueryService", CType(Me.DbLeeHotelAux.mDbLector("SERVICIO"), String), TipoProduccion, CodigoCompuesto)
                                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Descripcion, Total)
                            End If
                        End If
                        ' TIKET ANULADOS
                        If CType(Me.DbLeeHotel.mDbLector.Item("TICK_DAAN"), Date) = Me.mFecha Then
                            If Total <> 0 Then
                                Descripcion = "Bono Consumido Anulado " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " Ticket = " & Tiket

                                Linea = Linea + 1
                                Cuenta = Me.mCtaBcta3

                                Me.mTipoAsiento = "DEBE"
                                Me.InsertaOracle("AC", 81, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, Total * -1, "NO", "", Descripcion2, "SI")
                                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Descripcion, Total * -1)

                                Linea = Linea + 1
                                Cuenta = Me.mCtaBcta4

                                Me.mTipoAsiento = "HABER"
                                Me.InsertaOracle("AC", 81, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Descripcion, Total * -1, "NO", "", vCentroCosto, "SI", "", "SAT_NHCreateProdOrdersQueryService", CType(Me.DbLeeHotelAux.mDbLector("SERVICIO"), String), TipoProduccion, CodigoCompuesto)
                                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Descripcion, Total * -1)
                            End If

                        End If

                        ' CONSUMO BONOS ASOCIACION
                    Else

                        TotalJugadaPvp = (CType(Me.DbLeeHotelAux.mDbLector.Item("LIQUIDO"), Double) + CType(Me.DbLeeHotelAux.mDbLector.Item("IMPUESTO"), Double)) / CType(Me.DbLeeHotelAux.mDbLector.Item("BONO_CANT"), Integer)

                        Linea = Linea + 1

                        Descripcion = "Bono(*) Consumido PVP " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " Ticket = " & Tiket
                        Descripcion2 = " " & CType(Me.DbLeeHotelAux.mDbLector("ADIC_DESC"), String) & " Jug. Compradas = " & CType(Me.DbLeeHotelAux.mDbLector("BONO_CANT"), Integer) & " Pvp =  " & CType(Me.DbLeeHotelAux.mDbLector("PRECIO"), Double)

                        Cuenta = Me.mCtaBcta11


                        Me.mTipoAsiento = "DEBE"
                        Me.InsertaOracle("AC", 81, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, TotalJugadaPvp, "NO", "", Descripcion2, "SI")
                        Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Descripcion, TotalJugadaPvp)

                        '--------

                        TotalJugadaLiquido = CType(Me.DbLeeHotelAux.mDbLector.Item("LIQUIDO"), Double) / CType(Me.DbLeeHotelAux.mDbLector.Item("BONO_CANT"), Integer)

                        Linea = Linea + 1
                        Descripcion = "Bono(*) Consumido Líquido  " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String)

                        Cuenta = Me.mCtaBcta12

                        Me.mTipoAsiento = "HABER"
                        Me.InsertaOracle("AC", 81, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Descripcion, TotalJugadaLiquido, "NO", "", Descripcion2, "SI")
                        Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Descripcion, TotalJugadaLiquido)

                        '--------


                        ValorImpuesto = CType(Me.DbLeeHotelAux.mDbLector.Item("IMPUESTO"), Double)

                        Linea = Linea + 1
                        Descripcion = "Bono(*) Consumido Igic  " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String)

                        Cuenta = "?"



                        If ValorImpuesto <> 0 Then
                            Me.mTipoAsiento = "HABER"
                            Me.InsertaOracle("AC", 81, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Descripcion, ValorImpuesto, "NO", "", Descripcion2, "SI")
                            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Descripcion, ValorImpuesto)

                        End If

                    End If

                End If
                Me.DbLeeHotelAux.mDbLector.Close()

            End While


            Me.DbLeeHotel.mDbLector.Close()


            '
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try
    End Sub
    Private Sub BonosVencimiento()

        Try

            '  Dim Total As Double
            Dim Descripcion As String
            Dim Descripcion2 As String = " "
            Dim ValorJugada As Double = 0

            Dim ValorPendiente As Double = 0
            Dim Cuenta As String
            Dim Servicio As String = ""


            Dim vCentroCosto As String = "0"

            Dim TipoProduccion As String = ""
            Dim CodigoCompuesto As String = ""

            SQL = ""
            SQL += "SELECT TNPL_BONO.BONO_CODI, TNPL_BONO.BONO_ANCI, TNPL_BONO.BONO_DAEM, "
            SQL += "TNPL_BONO.BONO_CANT, TNPL_BONO.BONO_PREC AS TOTAL, TNPL_BONO.BONO_JUGA, "
            SQL += "TNPL_BONO.BONO_DAFI, TNPL_ADIC.ADIC_DESC, TNPL_TIVA.TIVA_PERC, "
            SQL += "TNPL_BONO.TICK_CODI, TNPL_BONO.TICK_ANCI, NVL(TNPL_BONO.BONO_NAME,'?') BONO_NAME, "
            SQL += "NVL(TNPL_BONO.BONO_APEL,'?') BONO_APEL,NVL(TNPL_ADIC.TIAD_CODI,   '0') AS TIPO "
            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_BONO," & Me.mParaUsuarioNewGolf & ".TNPL_ADIC," & Me.mParaUsuarioNewGolf & ".TNPL_IMSE," & Me.mParaUsuarioNewGolf & ".TNPL_TIVA "
            SQL += "WHERE ((TNPL_BONO.ADIC_CODI = TNPL_ADIC.ADIC_CODI) "
            SQL += "AND (TNPL_ADIC.ADIC_CODI = TNPL_IMSE.ADIC_CODI) "
            SQL += "AND (TNPL_IMSE.TIVA_COD1 = TNPL_TIVA.TIVA_CODI)) "
            SQL += "AND TNPL_BONO.BONO_DAFI = " & "'" & Me.mFecha & "' "
            SQL += "AND TNPL_BONO.BONO_ANUL = 0 "
            ' EXCLUIR BONOS ASOCIACION DE CAMPOS 
            SQL += " AND TIAD_CODI <> " & Me.MparaTipoBonoAsociacion

            SQL += " ORDER BY TNPL_BONO.BONO_CODI ASC "


            Me.DbLeeHotel.TraerLector(SQL)




            Linea = 0

            While Me.DbLeeHotel.mDbLector.Read



                ' Averiguar el Valor Liquido del valor total del Bono 

                SQL = "SELECT TNPL_BONO.BONO_CODI,TNPL_BONO.BONO_ANCI,MOVI_VLIQ AS LIQUIDO,TNPL_BONO.BONO_PREC AS PRECIO,ADIC_DESC,NVL(TNPL_ADIC.TIAD_CODI,   '0') AS TIPO,"
                SQL += " NVL(MOVI_DESC,   0) AS DESCUENTO,BONO_CANT, "
                SQL += " TNPL_ADCO.SERV_CODI AS SERVICIO "
                SQL += "FROM "
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_MOVI, "
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_DPTO,"
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_RECU,"
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADIC,"
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_RESE,"
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_BONO, "
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADCO "
                SQL += "WHERE(TNPL_MOVI.DPTO_CODI = TNPL_DPTO.DPTO_CODI(+)) "
                SQL += "AND(TNPL_MOVI.RECU_CODI = TNPL_RECU.RECU_CODI(+)) "
                SQL += "AND(TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI(+)) "
                SQL += "AND(TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI(+)) "
                SQL += "AND(TNPL_MOVI.RESE_CODI = TNPL_RESE.RESE_CODI(+)) "
                SQL += "AND(TNPL_MOVI.RESE_ANCI = TNPL_RESE.RESE_ANCI(+)) "
                SQL += "AND(TNPL_MOVI.ADIC_CODI = TNPL_ADCO.ADIC_CODI(+)) "
                SQL += "AND TNPL_ADIC.ADIC_TIPO = 3 "
                SQL += "AND TNPL_MOVI.TICK_CODI = TNPL_BONO.TICK_CODI "
                SQL += "AND TNPL_MOVI.TICK_ANCI = TNPL_BONO.TICK_ANCI "
                SQL += "AND TNPL_MOVI.CAJA_CODI = TNPL_BONO.CAJA_CODI "
                SQL += "AND(TNPL_MOVI.TICK_CODI,TNPL_MOVI.TICK_ANCI,TNPL_MOVI.CAJA_CODI) IN "
                SQL += "(SELECT TICK_CODI,TICK_ANCI,CAJA_CODI "
                SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_BONO) "
                SQL += "AND NVL(TNPL_MOVI.MOVI_NEWH,   '0') <> '2' "
                SQL += "AND MOVI_TIPO = 1 "
                SQL += " AND TNPL_BONO.BONO_CODI = " & Me.DbLeeHotel.mDbLector.Item("BONO_CODI")
                SQL += " AND TNPL_BONO.BONO_ANCI = " & Me.DbLeeHotel.mDbLector.Item("BONO_ANCI")

                SQL += " ORDER BY TNPL_BONO.BONO_CODI ASC"


                Me.DbLeeHotelAux.TraerLector(SQL)
                Me.DbLeeHotelAux.mDbLector.Read()
                If Me.DbLeeHotelAux.mDbLector.HasRows Then

                    If IsDBNull(Me.DbLeeHotelAux.mDbLector("SERVICIO")) = False Then
                        SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotelAux.mDbLector("SERVICIO"), String) & "'"
                        vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    Else
                        If mMostrarMensajes = True Then
                            MsgBox("Atención Bono sin Centro de Costo Asociado" & vbCrLf & "Revise Sección/Servicio en NewGolf" & vbCrLf & Me.DbLeeHotelAux.mDbLector("ADIC_DESC"), MsgBoxStyle.Exclamation, "Atención")
                        End If
                        vCentroCosto = "0"
                    End If

                    If Me.DbLeeHotelAux.mDbLector("SERVICIO") = Me.mParaServCodiBonos Then
                        TipoProduccion = "BONOS"
                    ElseIf Me.DbLeeHotelAux.mDbLector("SERVICIO") = Me.mParaServCodiBonosAsoc Then
                        ' Pat0006
                        ' TipoProduccion = "BONOS ASOC"
                        TipoProduccion = "NORMAL"

                    Else
                        TipoProduccion = "NORMAL"
                    End If


                    If Me.mParaIngresoPorHabitacion = 1 Then
                        CodigoCompuesto = "000-" & CType(Me.DbLeeHotelAux.mDbLector("SERVICIO"), String)
                    Else
                        CodigoCompuesto = Me.DbLeeHotelAux.mDbLector("SERVICIO")
                    End If


                    Servicio = Me.DbLeeHotelAux.mDbLector("SERVICIO")
                    ValorJugada = (CType(Me.DbLeeHotelAux.mDbLector.Item("LIQUIDO"), Double) / CType(Me.DbLeeHotel.mDbLector.Item("BONO_CANT"), Integer))
                    ValorPendiente = (CType(Me.DbLeeHotel.mDbLector.Item("BONO_CANT"), Integer) - CType(Me.DbLeeHotel.mDbLector.Item("BONO_JUGA"), Integer)) * ValorJugada
                    Descripcion2 = " " & CType(Me.DbLeeHotelAux.mDbLector("ADIC_DESC"), String) & " Jug. Pdtes = " & CType(Me.DbLeeHotel.mDbLector.Item("BONO_CANT"), Integer) - CType(Me.DbLeeHotel.mDbLector.Item("BONO_JUGA"), Integer) & " Pvp =  " & CType(Me.DbLeeHotelAux.mDbLector("PRECIO"), Double)
                    Me.DbLeeHotelAux.mDbLector.Close()
                End If


                Descripcion = "Bono Vencido " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String)



                If Me.DbLeeHotel.mDbLector.Item("TIPO") <> Me.MparaTipoBonoAsociacion Then
                    If ValorPendiente <> 0 Then
                        Linea = Linea + 1

                        Cuenta = Me.mCtaBcta5

                        Me.mTipoAsiento = "DEBE"
                        Me.InsertaOracle("AC", 82, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, ValorPendiente, "NO", "", Descripcion2, "SI")
                        Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Descripcion, ValorPendiente)


                        Linea = Linea + 1
                        Cuenta = Me.mCtaBcta6


                        Me.mTipoAsiento = "HABER"
                        Me.InsertaOracle("AC", 82, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Descripcion, ValorPendiente, "NO", "", vCentroCosto, "SI", "", "SAT_NHCreateProdOrdersQueryService", Servicio, TipoProduccion, CodigoCompuesto)
                        Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Descripcion, ValorPendiente)
                    End If
                End If



            End While
            Me.DbLeeHotel.mDbLector.Close()


            '
        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try
    End Sub

    Private Sub BonosAsociacionPagosAcuenta()
        Try

            Dim TotalTiket As Double
            Dim TotalBono As Double
            Dim Total As Double
            Dim Cuenta As String
            Dim Descripcion As String



            SQL = ""
            SQL += "SELECT TNPL_BONO.BONO_CODI,TNPL_BONO.BONO_ANCI,MOVI_VLIQ AS LIQUIDO,ADIC_DESC,NVL(TNPL_ADIC.TIAD_CODI,'0')  AS TIPO, "
            SQL += " NVL(MOVI_DESC,0) AS DESCUENTO,MOVI_IMPT AS TOTAL "

            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_DPTO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RECU,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADIC,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_RESE,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_BONO "

            SQL += " WHERE (TNPL_MOVI.DPTO_CODI = TNPL_DPTO.DPTO_CODI(+))"
            SQL += " AND(TNPL_MOVI.RECU_CODI = TNPL_RECU.RECU_CODI(+))"
            SQL += " AND(TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI(+))"
            SQL += " AND(TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI(+))"
            SQL += " AND(TNPL_MOVI.RESE_CODI = TNPL_RESE.RESE_CODI(+))"
            SQL += " AND(TNPL_MOVI.RESE_ANCI = TNPL_RESE.RESE_ANCI(+))"

            ' SOLO MOVIMIENTOS DE BONOS EN EL TIKE
            SQL += " AND TNPL_ADIC.ADIC_TIPO = 3 "

            SQL += " AND TNPL_MOVI.TICK_CODI = TNPL_BONO.TICK_CODI"
            SQL += " AND TNPL_MOVI.TICK_ANCI = TNPL_BONO.TICK_ANCI"
            SQL += " AND TNPL_MOVI.CAJA_CODI = TNPL_BONO.CAJA_CODI"
            SQL += " AND(TNPL_MOVI.TICK_CODI,   TNPL_MOVI.TICK_ANCI,   TNPL_MOVI.CAJA_CODI) IN  (SELECT TICK_CODI,TICK_ANCI,CAJA_CODI FROM " & Me.mParaUsuarioNewGolf & ".TNPL_BONO)"
            SQL += " AND NVL(TNPL_MOVI.MOVI_NEWH,'0') <> '2'"
            SQL += " AND MOVI_FECH = " & "'" & Me.mFecha & "' "
            SQL += " AND MOVI_TIPO = 1 "
            ' SOLO BONOS DE LA ASOCIACION 
            SQL += " AND TNPL_ADIC.TIAD_CODI = " & Me.MparaTipoBonoAsociacion

            SQL += " ORDER BY TNPL_BONO.BONO_CODI ASC "

            Linea = 0

            Me.DbLeeHotel.TraerLector(SQL)



            While Me.DbLeeHotel.mDbLector.Read
                TotalBono = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)


                Cuenta = Me.mCtaBcta8
                Descripcion = "Bono(Asc) " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ADIC_DESC"), String)

                Linea = Linea + 1

                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 83, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Descripcion, TotalBono, "NO", "", "Total Pvp", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Descripcion, TotalBono)


            End While
            Me.DbLeeHotel.mDbLector.Close()



            ' DETECCION DEL MOVIMIENTO DE FORMA DE PAGO EN EL TIKET 

            SQL = "SELECT tnpl_bono.bono_codi, tnpl_bono.bono_anci, tnpl_bono.bono_daem,"
            SQL += "tnpl_bono.bono_daan, tnpl_adic.adic_desc, tnpl_tick.tick_codi,"
            SQL += "tnpl_tick.tick_anci, tnpl_movi.movi_impt AS TOTAL, tnpl_adic.adic_tipo,"
            SQL += "tnpl_adic.tiad_codi, tnpl_fpag.fpag_desc, tnpl_bono.bono_prec,"
            SQL += "NVL(TNPL_FPAG.FPAG_CTB1,'0') AS CUENTANEWG,"
            SQL += "NVL(TNHT_FORE.FORE_CTB1,'0') AS CUENTANEWH, "
            SQL += "NVL(TNHT_CACR.CACR_CTBA,'0') AS CUENTANEWHVISA, "
            SQL += "  DECODE(tnht_fore.fore_ctb1,NULL,tnht_cacr.cacr_ctba,tnht_fore.fore_ctb1) AS FORMA "
            SQL += " FROM "
            SQL += Me.mParaUsuarioNewGolf & ".tnpl_bono,"
            SQL += Me.mParaUsuarioNewGolf & ".tnpl_tick,"
            SQL += Me.mParaUsuarioNewGolf & ".tnpl_adic,"
            SQL += Me.mParaUsuarioNewGolf & ".tnpl_movi,"
            SQL += Me.mParaUsuarioNewGolf & ".tnpl_fpag,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPCO,"
            SQL += Me.mParaUsuarioNewGolf & ".TNPL_CACO,"
            SQL += "TNHT_FORE ,"
            SQL += "TNHT_CACR "
            SQL += " WHERE ((tnpl_tick.tick_codi = tnpl_movi.tick_codi)"
            SQL += " AND (tnpl_tick.tick_anci = tnpl_movi.tick_anci)"
            SQL += " AND (tnpl_tick.caja_codi = tnpl_movi.caja_codi)"
            SQL += " AND (tnpl_bono.tick_codi = tnpl_tick.tick_codi)"
            SQL += " AND (tnpl_bono.tick_anci = tnpl_tick.tick_anci)"
            SQL += " AND (tnpl_bono.caja_codi = tnpl_tick.caja_codi)"
            SQL += " AND (tnpl_bono.adic_codi = tnpl_adic.adic_codi)"
            SQL += " AND (tnpl_movi.fpag_codi = tnpl_fpag.fpag_codi)"
            SQL += " AND TNPL_FPAG.FPAG_CODI  = TNPL_FPCO.FPAG_CODI(+) "
            SQL += " AND TNPL_FPCO.FORE_CODI  = TNHT_FORE.FORE_CODI "
            SQL += " AND (tnpl_movi.cacr_codi = tnpl_caco.cacr_cod1(+))"
            SQL += " AND (tnpl_caco.cacr_cod2 = tnht_cacr.cacr_codi(+))"
            SQL += " AND MOVI_FECH = " & "'" & Me.mFecha & "' "
            ' SOLO BONOS DE LA ASOCIACION 
            SQL += " AND TNPL_ADIC.TIAD_CODI = " & Me.MparaTipoBonoAsociacion
            SQL += ")"
            SQL += " ORDER BY TNPL_BONO.BONO_CODI ASC "


            Me.DbLeeHotel.TraerLector(SQL)
            While Me.DbLeeHotel.mDbLector.Read

                TotalTiket = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)



                ' VERIFICAR QUE COINCIDE CON EL VALOR DEL BONO ( CASO DE HABER EN EL TIKET OTROS ARTICULOS ) 

                SQL = ""
                SQL += "SELECT TNPL_BONO.BONO_CODI,TNPL_BONO.BONO_ANCI,MOVI_VLIQ AS LIQUIDO,ADIC_DESC,NVL(TNPL_ADIC.TIAD_CODI,'0')  AS TIPO, "
                SQL += " NVL(MOVI_DESC,0) AS DESCUENTO,MOVI_IMPT AS TOTAL "
                'SQL += ",NVL(TNPL_MOVI.MOVI_NUDO,'?') AS RECIBO"
                SQL += ",TNPL_MOVI.MOVI_DATE || TNPL_MOVI.MOVI_CODI AS RECIBO"


                SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI,"
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_DPTO,"
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_RECU,"
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_ADIC,"
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_FPAG,"
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_RESE,"
                SQL += Me.mParaUsuarioNewGolf & ".TNPL_BONO "

                SQL += " WHERE (TNPL_MOVI.DPTO_CODI = TNPL_DPTO.DPTO_CODI(+))"
                SQL += " AND(TNPL_MOVI.RECU_CODI = TNPL_RECU.RECU_CODI(+))"
                SQL += " AND(TNPL_MOVI.ADIC_CODI = TNPL_ADIC.ADIC_CODI(+))"
                SQL += " AND(TNPL_MOVI.FPAG_CODI = TNPL_FPAG.FPAG_CODI(+))"
                SQL += " AND(TNPL_MOVI.RESE_CODI = TNPL_RESE.RESE_CODI(+))"
                SQL += " AND(TNPL_MOVI.RESE_ANCI = TNPL_RESE.RESE_ANCI(+))"

                ' SOLO MOVIMIENTOS DE BONOS EN EL TIKE
                SQL += " AND TNPL_ADIC.ADIC_TIPO = 3 "

                SQL += " AND TNPL_MOVI.TICK_CODI = TNPL_BONO.TICK_CODI"
                SQL += " AND TNPL_MOVI.TICK_ANCI = TNPL_BONO.TICK_ANCI"
                SQL += " AND TNPL_MOVI.CAJA_CODI = TNPL_BONO.CAJA_CODI"
                SQL += " AND(TNPL_MOVI.TICK_CODI,   TNPL_MOVI.TICK_ANCI,   TNPL_MOVI.CAJA_CODI) IN  (SELECT TICK_CODI,TICK_ANCI,CAJA_CODI FROM " & Me.mParaUsuarioNewGolf & ".TNPL_BONO)"
                SQL += " AND NVL(TNPL_MOVI.MOVI_NEWH,'0') <> '2'"
                SQL += " AND MOVI_TIPO = 1 "
                SQL += " AND TNPL_BONO.BONO_CODI = " & Me.DbLeeHotel.mDbLector.Item("BONO_CODI")
                SQL += " AND TNPL_BONO.BONO_ANCI = " & Me.DbLeeHotel.mDbLector.Item("BONO_ANCI")



                Me.DbLeeHotelAux.TraerLector(SQL)
                Me.DbLeeHotelAux.mDbLector.Read()
                TotalBono = CType(Me.DbLeeHotelAux.mDbLector("TOTAL"), Double)
                Me.DbLeeHotelAux.mDbLector.Close()



                '
                If TotalTiket <> TotalBono Then
                    Descripcion = "Tiket(*) " & CType(Me.DbLeeHotel.mDbLector("TICK_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("TICK_ANCI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("FPAG_DESC"), String)
                    Total = TotalBono
                Else
                    Descripcion = "Tiket " & CType(Me.DbLeeHotel.mDbLector("TICK_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("TICK_ANCI"), String) & " " & CType(Me.DbLeeHotel.mDbLector("FPAG_DESC"), String)
                    Total = TotalTiket
                End If

                If Total <> 0 Then
                    Linea = Linea + 1
                    Me.mTipoAsiento = "DEBE"
                    ' poner el numero tiket 
                    '    Me.InsertaOracle("AC", 83, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTANEWH"), String), Me.mIndicadorDebe, Descripcion, Total, "NO", "", "Bono = " & CType(Me.DbLeeHotel.mDbLector("BONO_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("BONO_ANCI"), String), "SI", "COBRO BONO ASOCIACION")

                    Me.InsertaOracle("AC", 83, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorDebe, Descripcion.Replace("'", "''"), Total, "NO", "NIF POR PARAMETRO", "ASOCIACION DE CAMPOS DE GOLF ", "SI", "ANTICIPO", "SAT_NHPrePaymentService", 0, 0, "", "", 9, CType(Me.DbLeeHotel.mDbLector("FORMA"), String), CType(Me.DbLeeHotel.mDbLector("RECIBO"), String), "NADA", "NADA", "NADA")

                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTANEWH"), String), Me.mIndicadorDebe, " " & Descripcion, Total)
                End If


            End While
            Me.DbLeeHotel.mDbLector.Close()


        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try
    End Sub
#End Region


#Region "RUTINAS PRIVADAS"

    Private Function xFacturacionSalidaDesembolsos() As Double
        Dim Resultado As String
        Dim Total As Double
        Try

            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB) TOTAL"
            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT"
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
        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message)
            End If

        End Try

    End Function
    Private Function FacturacionTotalServiciosSinIgic() As Double

        Dim Total As Double
        Try


            '__________________________________________________________________________________________
            ' CALCULO DEl TOTAL DE LOS SERVICIOS SIN IGIC DE LAs FACTURA
            '__________________________________________________________________________________________
            SQL = "SELECT MOVI_VCRE AS TOTAL  FROM VNHT_MOVH TNHT_MOVI , TNHT_FACT,TNHT_FAMO "
            SQL += " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI AND "
            SQL += "       TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI AND "

            SQL = SQL & "     TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "

            SQL += "AND    TNHT_MOVI.MOVI_TIMO = '1'                 AND "
            SQL += "      (TNHT_MOVI.MOVI_AUTO = '1' OR TNHT_MOVI.MOVI_AUTO = '0' AND TNHT_MOVI.CCEX_CODI = 'TPV') "
            SQL += "AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
            '          SQL += "AND TNHT_FACT.FACT_STAT = " & "'1'"
            SQL += "AND TNHT_FACT.FAAN_CODI IS  NULL "
            SQL += " AND MOVI_IMP1 = 0"


            Me.DbLeeHotel.TraerLector(SQL)


            Total = 0
            While Me.DbLeeHotel.mDbLector.Read

                Total = Total + Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            End While
            Me.DbLeeHotel.mDbLector.Close()


            SQL = "SELECT (MOVI_VCRE * -1) AS TOTAL  FROM VNHT_MOVH TNHT_MOVI , TNHT_FACT,TNHT_FAMO "
            SQL += " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI AND "
            SQL += "       TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI AND "

            SQL = SQL & "     TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "

            SQL += "AND    TNHT_MOVI.MOVI_TIMO = '1'                 AND "
            SQL += "      (TNHT_MOVI.MOVI_AUTO = '1' OR TNHT_MOVI.MOVI_AUTO = '0' AND TNHT_MOVI.CCEX_CODI = 'TPV') "
            SQL += "AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
            '        SQL += "AND TNHT_FACT.FACT_STAT = " & "'1'"
            SQL += "AND TNHT_FACT.FAAN_CODI IS NOT  NULL "
            SQL += " AND MOVI_IMP1 = 0"



            Me.DbLeeHotel.TraerLector(SQL)



            While Me.DbLeeHotel.mDbLector.Read

                Total = Total + Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            End While
            Me.DbLeeHotel.mDbLector.Close()


            If IsNumeric(Total) = True Then
                Return Total
            Else
                Return 0
            End If
        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message)
            End If

        End Try
    End Function
    Private Function FacturacionTotalServiciosSinIgicNewgolf() As Double

        Dim Total As Double
        Try

            '__________________________________________________________________________________________
            ' CALCULO DEl TOTAL DE LOS SERVICIOS SIN IGIC DE LAs FACTURA
            '__________________________________________________________________________________________
            SQL = "SELECT MOVI_IMPT AS TOTAL  FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_FACT," & Me.mParaUsuarioNewGolf & ".TNPL_FAMO "
            SQL += "WHERE TNPL_FAMO.FACT_CODI = TNPL_FACT.FACT_CODI "
            SQL += " AND TNPL_FAMO.SEFA_CODI = TNPL_FACT.SEFA_CODI"
            SQL += " AND TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE"
            SQL += " AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI"
            SQL += " AND TNPL_MOVI.MOVI_DECR = '1'"
            SQL += " AND TNPL_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
            SQL += " AND MOVI_IMP1 = 0"


            Me.DbLeeHotel.TraerLector(SQL)


            Total = 0
            While Me.DbLeeHotel.mDbLector.Read

                Total = Total + Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            End While
            Me.DbLeeHotel.mDbLector.Close()

            If IsNumeric(Total) = True Then
                Return Total
            Else
                Return 0
            End If
        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message)
            End If

        End Try
    End Function
    Private Function FacturacionTotalServiciosSinIgicNewgolfAnuladaS() As Double

        Dim Total As Double
        Try

            '__________________________________________________________________________________________
            ' CALCULO DEl TOTAL DE LOS SERVICIOS SIN IGIC DE LAs FACTURA
            '__________________________________________________________________________________________
            SQL = "SELECT MOVI_IMPT AS TOTAL  FROM " & Me.mParaUsuarioNewGolf & ".TNPL_MOVI," & Me.mParaUsuarioNewGolf & ".TNPL_FACT," & Me.mParaUsuarioNewGolf & ".TNPL_FAMO "
            SQL += "WHERE TNPL_FAMO.FACT_CODI = TNPL_FACT.FACT_CODI"
            SQL += " AND TNPL_FAMO.SEFA_CODI = TNPL_FACT.SEFA_CODI"
            SQL += " AND TNPL_FAMO.MOVI_DATE = TNPL_MOVI.MOVI_DATE"
            SQL += " AND TNPL_FAMO.MOVI_CODI = TNPL_MOVI.MOVI_CODI"
            SQL += " AND TNPL_MOVI.MOVI_DECR = '1'"
            SQL += " AND TNPL_FACT.FACT_DAAN = " & "'" & Me.mFecha & "' "
            SQL += " AND MOVI_IMP1 = 0"


            Me.DbLeeHotel.TraerLector(SQL)


            Total = 0
            While Me.DbLeeHotel.mDbLector.Read

                Total = Total + Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            End While
            Me.DbLeeHotel.mDbLector.Close()

            If IsNumeric(Total) = True Then
                Return Total
            Else
                Return 0
            End If
        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message)
            End If

        End Try
    End Function
    Private Function FacturacionContadoServiciosSinIgic() As Double

        Dim Total As Double
        Try


            '__________________________________________________________________________________________
            ' CALCULO DEl TOTAL DE LOS SERVICIOS SIN IGIC DE LAs FACTURA
            '__________________________________________________________________________________________
            SQL = "SELECT MOVI_VCRE AS TOTAL  FROM VNHT_MOVH TNHT_MOVI , TNHT_FACT,TNHT_FAMO "
            SQL += " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI AND "
            SQL += "       TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI AND "

            SQL = SQL & "     TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "

            SQL += "AND    TNHT_MOVI.MOVI_TIMO = '1'                 AND "
            SQL += "      (TNHT_MOVI.MOVI_AUTO = '1' OR TNHT_MOVI.MOVI_AUTO = '0' AND TNHT_MOVI.CCEX_CODI = 'TPV') "
            SQL += "AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
            SQL += "AND TNHT_FACT.FACT_STAT = " & "'1'"
            SQL += "AND TNHT_FACT.FAAN_CODI IS  NULL "
            SQL += " AND MOVI_IMP1 = 0"


            Me.DbLeeHotel.TraerLector(SQL)


            Total = 0
            While Me.DbLeeHotel.mDbLector.Read

                Total = Total + Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            End While
            Me.DbLeeHotel.mDbLector.Close()


            SQL = "SELECT (MOVI_VCRE * -1) AS TOTAL  FROM VNHT_MOVH TNHT_MOVI , TNHT_FACT,TNHT_FAMO "
            SQL += " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI AND "
            SQL += "       TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI AND "

            SQL = SQL & "     TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
            SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "

            SQL += "AND    TNHT_MOVI.MOVI_TIMO = '1'                 AND "
            SQL += "      (TNHT_MOVI.MOVI_AUTO = '1' OR TNHT_MOVI.MOVI_AUTO = '0' AND TNHT_MOVI.CCEX_CODI = 'TPV') "
            SQL += "AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
            SQL += "AND TNHT_FACT.FACT_STAT = " & "'1'"
            SQL += "AND TNHT_FACT.FAAN_CODI IS NOT  NULL "
            SQL += " AND MOVI_IMP1 = 0"



            Me.DbLeeHotel.TraerLector(SQL)



            While Me.DbLeeHotel.mDbLector.Read

                Total = Total + Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            End While
            Me.DbLeeHotel.mDbLector.Close()


            If IsNumeric(Total) = True Then
                Return Total
            Else
                Return 0
            End If
        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message)
            End If

        End Try
    End Function
    Private Function FacturacionCreditoDesembolsos() As Double
        Dim Resultado As String
        Dim Total As Double
        Try


            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB) TOTAL"
            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_ENTI"
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
        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message)
            End If

        End Try
    End Function
    Private Function XFacturacionCreditoServiciosSinIgic() As Double
        Dim Resultado As String
        Dim Total As Double
        Try
            '__________________________________________________________________________________________
            ' CALCULO DEl TOTAL DE LOS SERVICIOS SIN IGIC DE LAs FACTURA
            '__________________________________________________________________________________________
            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VCRE) TOTAL"
            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_SERV,TNHT_TIVA,TNHT_ENTI"
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
        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message)
            End If

        End Try
    End Function


    Private Function xFacturacionNoAlojadoDesembolsos() As Double
        Dim Resultado As String
        Dim Total As Double
        Try
            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB) TOTAL"
            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_CCEX"
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
        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message)
            End If

        End Try
    End Function
    Private Function XFacturacionNoAlojadoServiciosSinIgic() As Double
        Dim Resultado As String
        Dim Total As Double
        Try
            '__________________________________________________________________________________________
            ' CALCULO DEl TOTAL DE LOS SERVICIOS SIN IGIC DE LAs FACTURA
            '__________________________________________________________________________________________
            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VCRE) TOTAL"
            SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_SERV,TNHT_TIVA,TNHT_CCEX"
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
        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message)
            End If

        End Try
    End Function

    Private Function xFacturacionNoAlojadoDesembolsosFactura(ByVal vSerie As String, ByVal vFactura As Integer) As Double
        Dim Resultado As String
        Dim Total As Double
        Try
            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB) TOTAL"
            SQL += " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_CCEX"
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
        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message)
            End If

        End Try
    End Function
    Private Function XFacturacionNoAlojadoServiciosSinIgicFactura(ByVal vSerie As String, ByVal vFactura As Integer) As Double
        Dim Resultado As String
        Dim Total As Double
        Try
            '__________________________________________________________________________________________
            ' CALCULO DEl TOTAL DE LOS SERVICIOS SIN IGIC DE UNA FACTURA
            '__________________________________________________________________________________________
            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VCRE) TOTAL"
            SQL += " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_SERV,TNHT_TIVA,TNHT_CCEX"
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
        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message)
            End If

        End Try
    End Function


#End Region
#End Region

#Region "METODOS PUBLICOS"
    Public Sub Procesar()
        Try

            If Me.mDebug2 = True Then
                Me.DetallePagosaCuentaAxSaldo()

                If Me.mParaConectaNewGolf Then
                    ' NO SE PARA QUE ERA ESTA LINEA
                    '  Me.DetallePagosaCuentaGolfAxSaldo()
                End If



                If Me.mFecha = "31/08/2009" Then

                    Me.AjustarDecimales()
                    Me.mProgress.Value = 100
                    Me.mProgress.Update()

                    Me.CerrarFichero()
                    Me.CierraConexiones()
                    Me.mTextDebug.Text = "Fin de Integración"
                    Me.mTextDebug.Update()

                    Exit Sub
                End If


                'Me.AjustarDecimales()
                'Me.mProgress.Value = 100
                'Me.mProgress.Update()

                'Me.CerrarFichero()
                'Me.CierraConexiones()
                'Me.mTextDebug.Text = "Fin de Integración"
                'Me.mTextDebug.Update()

                'Exit Sub
            End If

            ' MsgBox("Ojo revisar  COMISION de visas , en depositos antincipados de agencias si los hubiera ", MsgBoxStyle.Exclamation, "Atención")
            If Me.FileEstaOk = False Then Exit Sub
            ' ---------------------------------------------------------------
            ' Asiento de Ventas 1
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                Me.PendienteFacturarTotal()
                Me.mTextDebug.Text = "Calculando Pdte. Facturar"
                Me.mTextDebug.Update()

                If Me.mParaIngresoPorHabitacion = 0 Then
                    Me.VentasDepartamento()
                Else
                    '   Me.VentasDepartamentoBloque() 
                    'Me.VentasDepartamentoBloqueTest()
                    Me.VentasDepartamentoBloqueAxapta()
                End If

                Me.mTextDebug.Text = "Calculando Ventas por Departamento"
                Me.mTextDebug.Update()


                Me.mProgress.Value = 10
                Me.mProgress.Update()
            End If

            ' ---------------------------------------------------------------
            ' Asiento de Pagos a Cuenta 2
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                ' Me.TotalPagosaCuentaVisas()
                Me.mTextDebug.Text = "Pagos a Cuenta Visas"
                Me.mTextDebug.Update() '
                'Me.TotalPagosaCuentaOtrasFormas()
                Me.mTextDebug.Text = "Pagos a Cuenta Otras Formas de Pago"
                Me.mTextDebug.Update() '

                ' Me.DetallePagosaCuentaVisas()
                '   Me.DetallePagosaCuentaVisasAx()
                Me.DetallePagosaCuentaAx()
                Me.mTextDebug.Text = "Detalle de Pagos a Cuenta Visas"
                Me.mTextDebug.Update()

                'Me.DetallePagosaCuentaOtrasFormas()
                'Me.DetallePagosaCuentaOtrasFormasAx()
                Me.mTextDebug.Text = "Detalle de Pagos a Cuenta Otras Formas"
                Me.mTextDebug.Update()

                ' Me.TotalPagosaCuentaVisasComision()
                '  Me.TotalPagosaCuentaVisasComisionAx()
                Me.mTextDebug.Text = "COMISION Visas  de Pagos a Cuenta "
                Me.mTextDebug.Update()


                Me.mProgress.Value = 20
                Me.mProgress.Update()
            End If
            ' ---------------------------------------------------------------
            ' Asiento de Pagos a Cuenta 25  newgolf
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                If Me.mParaConectaNewGolf = 1 Then
                    'Me.TotalPagosaCuentaVisasGolf()
                    Me.mTextDebug.Text = "Pagos a Cuenta Visas"
                    Me.mTextDebug.Update() '
                    'Me.TotalPagosaCuentaOtrasFormasGolf()
                    Me.mTextDebug.Text = "Pagos a Cuenta Otras Formas de Pago"
                    Me.mTextDebug.Update() '

                    ' Me.DetallePagosaCuentaVisasGolf()
                    Me.DetallePagosaCuentaGolfAx()
                    Me.mTextDebug.Text = "Detalle de Pagos a Cuenta Visas"
                    Me.mTextDebug.Update()

                    'Me.DetallePagosaCuentaOtrasFormasGolf()
                    Me.mTextDebug.Text = "Detalle de Pagos a Cuenta Otras Formas"
                    Me.mTextDebug.Update()

                    'Me.TotalPagosaCuentaVisasComisionGolf()
                    Me.mTextDebug.Text = "COMISION Visas  de Pagos a Cuenta "
                    Me.mTextDebug.Update()


                    Me.mProgress.Value = 20
                    Me.mProgress.Update()
                End If

            End If
            ' ---------------------------------------------------------------
            ' Asiento de DEVOLUCIONES  21
            '----------------------------------------------------------------

            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then

                If Me.MparaTrataDevoluciones = True Then
                    ' recepcion de la devolucion (2)
                    Me.TotalDevolucionesOtrasManualesAx()
                End If

                '  Me.TotalDevolucionesVisas()
                Me.mTextDebug.Text = "Devoluciones Visas"
                Me.mTextDebug.Update()

                ' Me.TotalDevolucionesOtrasFormas()
                Me.mTextDebug.Text = "Devoluciones Otras Formas de Pago"
                Me.mTextDebug.Update()

                'Me.DetalleDevolucionesVisas()
                Me.mTextDebug.Text = "Detalle de Devoluciones Visas"
                Me.mTextDebug.Update()

                'Me.DetalleDevolucionesOtrasFormas()
                Me.mTextDebug.Text = "Detalle de Devoluciones Otras Formas"
                Me.mTextDebug.Update()

                Me.mProgress.Value = 25
                Me.mProgress.Update()
            End If


            ' ---------------------------------------------------------------
            ' Asiento de DEVOLUCIONES HECHAS POR NEWHOTEL AUTOMATICAS AL FACTURAR  22 
            '----------------------------------------------------------------

            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then


                If Me.MparaTrataDevoluciones = True Then
                    ' recepcion de la devolucion (2)
                    Me.TotalDevolucionesOtrasEnFacturaAx()
                End If

                'Me.TotalDevolucionesVisasFacturado()
                Me.mTextDebug.Text = "Devoluciones Visas en Factura"
                Me.mTextDebug.Update()

                'Me.TotalDevolucionesOtrasFormasFacturado()
                Me.mTextDebug.Text = "Devoluciones Otras Formas de Pago en Factura"
                Me.mTextDebug.Update()

                'Me.DetalleDevolucionesVisasFacturado()
                Me.mTextDebug.Text = "Detalle de Devoluciones Visas en Factura"
                Me.mTextDebug.Update()

                'Me.DetalleDevolucionesOtrasFormasFacturado()
                Me.mTextDebug.Text = "Detalle de Devoluciones Otras Formas en Factura"
                Me.mTextDebug.Update()

                Me.mProgress.Value = 28
                Me.mProgress.Update()

            End If

            ' ---------------------------------------------------------------
            ' Asiento Facturacion total del dia                   3
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                Me.FacturasTotalLiquido()
                Me.mTextDebug.Text = "Calculando Total Luíquido Facturas de Salida"
                Me.mTextDebug.Update()


                Me.FacturasSalidaTotaLDescuentos()
                Me.mTextDebug.Text = "Calculando Descuentos Financieros y Comisiones Facturas de Salida"
                Me.mTextDebug.Update()


                Me.FacturasSalidaIgicAgrupado()
                'Me.FacturasSalidaTotalFActuraNuevo()
                Me.FacturasSalidaTotalFActuraNuevoAX()
            End If


            Me.mTextDebug.Text = "Detalle de Impuesto Facturas de Salida"
            Me.mTextDebug.Update()


            Me.mProgress.Value = 30
            Me.mProgress.Update()



            ' ---------------------------------------------------------------
            ' Asiento Facturacion total del dia     NEWGOLF       34
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                If Me.mParaConectaNewGolf = 1 Then
                    Me.FacturasTotalLiquidoGolf()
                    Me.mTextDebug.Text = "Calculando Total Luíquido Facturas de Salida"
                    Me.mTextDebug.Update()

                    Me.FacturasSalidaIgicAgrupadoGolf()
                    'Me.FacturasSalidaTotalFActuraGolf()
                    Me.FacturasSalidaTotalFActuraGolfAX()

                    Me.mTextDebug.Text = "Detalle de Impuesto Facturas de Salida"
                    Me.mTextDebug.Update()


                    Me.mProgress.Value = 33
                    Me.mProgress.Update()
                End If
            End If

            ' ---------------------------------------------------------------
            ' Asiento Notas de Credito total del dia     NEWGOLF       31
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                If Me.mParaConectaNewGolf = 1 Then
                    Me.NotasCreditoGolfTotalLiquido()
                    Me.mTextDebug.Text = "Calculando Total Luíquido Notas de Credito de Salida"
                    Me.mTextDebug.Update()

                    Me.NotasCreditoGolfIgicAgrupado()
                    'Me.NotasCreditoGolfTotalNota()
                    Me.NotasCreditoGolfTotalNotaAx()
                    '  If MsgBox("Ejecuta Debitos Nota de Abono ??", MsgBoxStyle.OkCancel, "Atención...") = MsgBoxResult.Ok Then
                    'Me.NotasCreditoGolfCobrosAx()
                    'End If

                    Me.mTextDebug.Text = "Detalle de Impuesto Notas de Creditode Salida"
                    Me.mTextDebug.Update()


                    Me.mProgress.Value = 34
                    Me.mProgress.Update()
                End If
            End If

            ' ---------------------------------------------------------------
            ' Asiento Facturacion Contado del dia                   35 
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                Me.FacturasContadoTotal()
                Me.mTextDebug.Text = "Calculando Total Luíquido Facturas de Salida (contado)"
                Me.mTextDebug.Update()


                '      Me.FacturasContadoTotaLDescuentos()
                Me.mTextDebug.Text = "Calculando Descuentos Financieros y Comisiones Facturas de Salida"
                Me.mTextDebug.Update()




                ' t1TRATAMIENTO DE COBROS 
                Me.FacturasContadoTotalVisasAX()
                Me.FacturasContadoTotaLOtrasFormasAX()

                ' t2 pdte !!!
                Me.FacturasContadoCancelaciondeDevolucionesManualesAx()




                '-----------------------------------------------------------------
                ' NUEVO TRATAMIENTO DE FACTURACION DE ANTICIPOS 
                '-----------------------------------------------------------
                ' ANTES
                '  Me.FacturasContadoCancelaciondeAnticiposAx()
                ' Me.FacturasContadoCancelaciondeAnticiposrectificativasAx()
                ' AHORA
                ' t3 
                Me.FacturasContadoCancelaciondeAnticiposAxNuevo()

                ' ojo ver si es necesario debejo 
                'Me.FacturasContadoCancelaciondeAnticiposrectificativasAx()






                Me.mTextDebug.Text = "Cancelación de Anticipos  Facturas de Salida"
                Me.mTextDebug.Update()

                Me.FacturasContadoTotalVisasComision()

                Me.mProgress.Value = 40
                Me.mProgress.Update()
            End If


            ' ---------------------------------------------------------------
            ' Asiento Facturacion Contado del dia  newgolf                 37 
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                If Me.mParaConectaNewGolf = 1 Then
                    Me.FacturasContadoTotalGolf()
                    Me.mTextDebug.Text = "Calculando Total Luíquido Facturas NewGolf  (contado)"
                    Me.mTextDebug.Update()


                    'Me.FacturasContadoTotalVisasGolf()
                    'Me.FacturasContadoTotaLOtrasFormasGolf()
                    Me.FacturasContadoTotalVisasGolfAx()
                    Me.FacturasContadoTotaLOtrasFormasGolfAx()

                    ' Me.FacturasContadoCancelaciondeAnticiposGolf()
                    Me.FacturasContadoCancelaciondeAnticiposGolfAx()
                    Me.mTextDebug.Text = "Cancelación de Anticipos  Facturas de Salida"
                    Me.mTextDebug.Update()

                    Me.FacturasContadoTotalVisasComisionGolf()

                    Me.mProgress.Value = 45
                    Me.mProgress.Update()

                End If

            End If



            ' ---------------------------------------------------------------
            ' Asiento de Depositos Anticipados  de entidad 20
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                '   Me.TotalDepositosAnticipadosVisasEntidad()
                '  Me.mTextDebug.Text = "Depósitos Anticipados Visas"
                ' Me.mTextDebug.Update()

                'Me.TotalDepositosAnticipadosOtrasFormasEntidad()
                'Me.mTextDebug.Text = "Depósitos Anticipados Otras Formas de Pago"
                'Me.mTextDebug.Update()

                'Me.DetalleDepositosAnticipadosVisasEntidad()
                'Me.mTextDebug.Text = "Detalle Depósitos Anticipados Visas"
                'Me.mTextDebug.Update()

                'Me.DetalleDepositosAnticipadosOtrasFormasEntidad()
                'Me.mTextDebug.Text = "Detalle Depósitos Anticipados Otras Formas"
                'Me.mTextDebug.Update()

                'Me.mProgress.Value = 60
                'Me.mProgress.Update()
            End If

            ' ---------------------------------------------------------------
            ' Asiento 30 de Depositos Anticipados  de RESERVAS SIN ENTIDAD
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                'Me.TotalDepositosAnticipadosVisasOtros()
                'Me.mTextDebug.Text = "Depósitos Anticpados Visas"
                'Me.mTextDebug.Update()

                'Me.TotalDepositosAnticipadosOtrasFormasOtros()
                'Me.mTextDebug.Text = "Depósitos Anticipados Otras Formas de Pago"
                'Me.mTextDebug.Update()

                'Me.DetalleDepositosAnticipadosVisasOtros()
                'Me.mTextDebug.Text = "Detalle Depósitos Anticipados Visas"
                'Me.mTextDebug.Update()

                'Me.DetalleDepositosAnticipadosOtrasFormasOtros()
                'Me.mTextDebug.Text = "Detalle Depósitos Anticipados Otras Formas"
                'Me.mTextDebug.Update()

                'Me.mProgress.Value = 70
                'Me.mProgress.Update()
            End If

            ' ---------------------------------------------------------------
            ' Asiento de Depositos Anticipados  de NO ALOJADOS
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                ' Me.TotalDepositosAnticipadosVisasOtrosNoAlojados()
                ' Me.mTextDebug.Text = "Depósitos Anticpados Visas"
                ' Me.mTextDebug.Update()

                'Me.TotalDepositosAnticipadosOtrasFormasOtrosNoAlojados()
                'Me.mTextDebug.Text = "Depósitos Anticpados Otras Formas de Pago"
                'Me.mTextDebug.Update()

                'Me.DetalleDepositosAnticipadosVisasOtrosNoAlojados()
                'Me.mTextDebug.Text = "Detalle Depósitos Anticpados Visas"
                'Me.mTextDebug.Update()

                'Me.DetalleDepositosAnticipadosOtrasFormasOtrosNoAlojados()
                'Me.mTextDebug.Text = "Detalle Depósitos Anticpados Otras Formas"
                'Me.mTextDebug.Update()

                'Me.mProgress.Value = 80
                'Me.mProgress.Update()
            End If


            ' ---------------------------------------------------------------
            ' Asiento de Ventas 80 DE BONOS 
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                If Me.mParaConectaNewGolf = 1 Then
                    If Me.mTratarBonos = True Then
                        Me.mTextDebug.Text = "Procesando Facturación de Bonos"
                        Me.mTextDebug.Update()

                        Me.BonosEmitidos()
                        '    Me.BonosEmitidosAnulados()
                        Me.BonosConsumoJugada()
                        Me.BonosVencimiento()

                        ' 20091125  NO SE HACE AHORA ESTE MOVIMIENTO 
                        '     Me.BonosAsociacionPagosAcuenta()

                        Me.mProgress.Value = 90
                        Me.mProgress.Update()
                    End If

                End If
            End If


            ' ---------------------------------------------------------------
            ' Asiento de Ventas 90 DE TIKETS NEWPOS
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                Me.mTextDebug.Text = "Procesando Venta de Tikets"
                Me.mTextDebug.Update()

                If Me.mParaConectaNewPos = 1 Then
                    Me.VentaTiketsNewPos()
                End If

                Me.mProgress.Value = 95
                Me.mProgress.Update()

            End If
            ' ---------------------------------------------------------------
            ' Asiento de Ventas 95 DE TIKETS NEWGOLF
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then

                If Me.mParaConectaNewGolf = 1 Then
                    Me.mTextDebug.Text = "Procesando Venta de Tikets"
                    Me.mTextDebug.Update()

                    Me.VentaTiketsNewgOLF()

                    Me.mProgress.Value = 96
                    Me.mProgress.Update()
                End If
            End If


            ' ---------------------------------------------------------------
            ' Asiento de Ventas 100 ESTADISTICAS DE OCUPACION 
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then

                Me.mTextDebug.Text = "Procesando Venta Estadídticas"
                Me.mTextDebug.Update()

                Me.EstadisticasOcupacion()

                Me.mProgress.Value = 98
                Me.mProgress.Update()

            End If



            Me.AjustarDecimales()

            If Me.P_Existenregistros = False Then
                Me.CargaBookid()
            End If

            Me.mProgress.Value = 100
            Me.mProgress.Update()

            Me.CerrarFichero()
            Me.CierraConexiones()
            Me.mTextDebug.Text = "Fin de Integración"
            Me.mTextDebug.Update()

        Catch EX As Exception
            If mMostrarMensajes = True Then
                MsgBox(EX.Message)
            End If

        End Try

    End Sub
    Public Sub ProcesarSoloBonos()
        Try

            If Me.FileEstaOk = False Then Exit Sub

            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                If Me.mParaConectaNewGolf = 1 Then
                    Me.mTextDebug.Text = "Procesando Facturación de Bonos"
                    Me.mTextDebug.Update()

                    Me.BonosEmitidos()
                    '    Me.BonosEmitidosAnulados()
                    Me.BonosConsumoJugada()
                    Me.BonosVencimiento()

                    Me.BonosAsociacionPagosAcuenta()

                    Me.mProgress.Value = 98
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

        Catch ex As Exception

        End Try
    End Sub
    Private Sub AjustarDecimales()
        Try

            Dim TotalDebe As Decimal
            Dim TotalHaber As Decimal
            Dim TotalDiferencia As Decimal


            SQL = "SELECT ROUND(SUM(round(NVL(ASNT_DEBE,'0'),2)),2) FROM TH_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_IMPRIMIR = 'SI'"



            If IsNumeric(Me.DbLeeCentral.EjecutaSqlScalar(SQL)) Then
                TotalDebe = CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Decimal)
            Else
                TotalDebe = 0
            End If


            SQL = "SELECT ROUND(SUM(round(NVL(ASNT_HABER,'0'),2)),2) FROM TH_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"

            SQL += " AND ASNT_IMPRIMIR = 'SI'"
            If IsNumeric(Me.DbLeeCentral.EjecutaSqlScalar(SQL)) Then
                TotalHaber = CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Decimal)
            Else
                TotalHaber = 0
            End If




            If TotalHaber > TotalDebe Then
                TotalDiferencia = TotalHaber - TotalDebe
                '     MsgBox("Se va ha producir un Ajuste Decimal  de " & TotalDiferencia & "  " & vbCrLf & vbCrLf & "No Integre con valores superiores a 0.05", MsgBoxStyle.Information, "Atención")
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 999, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaRedondeo, Me.mIndicadorDebe, "AJUSTE REDONDEO", TotalDiferencia, "SI", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaRedondeo, Me.mIndicadorDebe, "AJUSTE REDONDEO", TotalDiferencia)
            End If

            If TotalHaber < TotalDebe Then
                TotalDiferencia = TotalDebe - TotalHaber
                '        MsgBox("Se va ha producir un Ajuste Decimal  de " & TotalDiferencia & "  " & vbCrLf & vbCrLf & "No Integre con valores superiores a 0.05", MsgBoxStyle.Information, "Atención")
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 999, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaRedondeo, Me.mIndicadorHaber, "AJUSTE REDONDEO", TotalDiferencia, "SI", "", "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaRedondeo, Me.mIndicadorHaber, "AJUSTE REDONDEO", TotalDiferencia)
            End If

        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Critical)
            End If

        End Try
    End Sub

#End Region
    Public Function StrConexionExtraeUsuario(ByVal vStrConexion As String) As String
        Try
            If vStrConexion.Length > 0 Then
                Dim Elementos As Array
                Dim SubElementos As Array
                Elementos = Split(vStrConexion, ";")
                SubElementos = Split((Elementos(1)), "=")
                Return CType(SubElementos(1), String).Trim
            Else
                Return ""
            End If

        Catch ex As Exception
            Return ""
            If mMostrarMensajes = True Then
                MsgBox(ex.Message)
            End If

        End Try
    End Function
    Private Function BuscaAnticipoFacturaAnticiposAx(ByVal vFactura As Integer, ByVal Vserie As String) As String

        Dim SQL As String

        Dim Descripcion As String = ""
        Dim FormaCobro As String = ""


        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,TNHT_FACT.FACT_CODI||'/'||TNHT_FACT.SEFA_CODI FACTURA, "

        SQL += "TNHT_MOVI.MOVI_VDEB TOTAL,NVL(TNHT_RESE.RESE_ANPH,'?') CLIENTE,"
        SQL += " TNHT_FACT.FACT_CODI AS NUMERO ,TNHT_FACT.SEFA_CODI SERIE,"
        SQL = SQL & "TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DAVA,TNHT_FACT.FACT_DAEM,TNHT_MOVI.MOVI_DATR,TNHT_FACT.FAAN_CODI,TNHT_FACT.ENTI_CODI AS ENTIFACT "

        ' N
        SQL = SQL & " ,NVL(TNHT_RESE.RESE_CODI ,0) AS RESE_CODI,NVL(TNHT_RESE.RESE_ANCI ,0) AS RESE_ANCI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_CODI ,'?') AS CCEX_CODI,NVL(TNHT_ENTI.ENTI_CODI ,'?') AS ENTI_CODI,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_NUCO ,'?') AS CCEX_NUCO,NVL(TNHT_ENTI.ENTI_NUCO ,'?') AS ENTI_NUCO,"
        SQL = SQL & " NVL(TNHT_CCEX.CCEX_TITU ,'?') AS CCEX_TITU,NVL(TNHT_ENTI.ENTI_NOME ,'?') AS ENTI_NOME,"
        SQL = SQL & " NVL(TNHT_ENTI.ENTI_FAMA ,'?') AS ENTI_FAMA,"
        SQL = SQL & " NVL(TNHT_CACR.CACR_CTBA,'0') TARJETA,"
        SQL = SQL & " NVL(TNHT_FORE.FORE_CTB1,'0') FORMA"

        SQL = SQL & ",TNHT_MOVI.MOVI_DARE || TNHT_MOVI.MOVI_CODI AS RECIBO "


        SQL = SQL & " FROM VNHT_MOVH TNHT_MOVI,TNHT_FACT,TNHT_RESE,TNHT_FAMO"
        ' N
        SQL = SQL & " ,TNHT_FORE,TNHT_CACR,TNHT_ENTI,TNHT_CCEX "

        '
        SQL = SQL & " WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL = SQL & " AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL = SQL & " AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL = SQL & " AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI"

        'N
        SQL = SQL & " AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI(+)"
        SQL = SQL & " AND TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+)"
        SQL = SQL & " AND TNHT_RESE.ENTI_CODI = TNHT_ENTI.ENTI_CODI(+)"
        SQL = SQL & " AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+)"


        ' NUEVO POR AJUSTE DE RENDIMIENTO 
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) AND "
        SQL = SQL & " TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) AND"




        SQL = SQL & " TNHT_FACT.FACT_CODI = " & vFactura
        SQL = SQL & " AND TNHT_FACT.SEFA_CODI = " & "'" & Vserie & "'"

        SQL = SQL & " AND TNHT_MOVI.MOVI_TIMO = '2'"
        SQL = SQL & " AND TNHT_MOVI.TIRE_CODI  = '1'"
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

        ' EL PRIMER ANTICIPO 
        SQL += " AND ROWNUM = 1 "

        SQL += " ORDER BY TNHT_MOVI.MOVI_DAVA ASC "

        Me.DbLeeHotelAux.TraerLector(SQL)

        While Me.DbLeeHotelAux.mDbLector.Read

            If CType(Me.DbLeeHotelAux.mDbLector("TARJETA"), String) = "0" Then
                FormaCobro = CType(Me.DbLeeHotelAux.mDbLector("FORMA"), String)
            Else
                FormaCobro = CType(Me.DbLeeHotelAux.mDbLector("TARJETA"), String)
            End If



        End While
        Me.DbLeeHotelAux.mDbLector.Close()

        Return FormaCobro

    End Function


    Private Function BuscaReservasdeunaFactura(ByVal vfactura As Integer, ByVal vSefa As String, ByVal vTipo As Integer) As String
        Try
            ' vtipo  = 1   buscar las reservas de la factura en tnht_defa
            ' vtipo  = 2   buscar las reservas de la factura en tnht_faen

            Dim Result As String = ""

            If vTipo = 1 Then
                SQL = "SELECT DISTINCT RESE_CODI,RESE_ANCI FROM TNHT_DEFA WHERE FACT_CODI = " & vfactura
                SQL += " AND SEFA_CODI = '" & vSefa & "'"
            ElseIf vTipo = 2 Then
                SQL = "SELECT DISTINCT RESE_CODI,RESE_ANCI FROM TNHT_FAEN WHERE FACT_CODI = " & vfactura
                SQL += " AND SEFA_CODI = '" & vSefa & "'"
            End If

            Me.DbLeeHotelAux.TraerLector(SQL)

            While Me.DbLeeHotelAux.mDbLector.Read
                If IsDBNull(Me.DbLeeHotelAux.mDbLector("RESE_CODI")) = False Then
                    Result = Result & Me.DbLeeHotelAux.mDbLector("RESE_CODI") & "/" & Me.DbLeeHotelAux.mDbLector("RESE_ANCI") & ";"
                End If
            End While
            Me.DbLeeHotelAux.mDbLector.Close()

            Return Result

        Catch ex As Exception
            Return ""
        End Try
    End Function




    ' ojo cerrar conexiones a la base de datos 
End Class
