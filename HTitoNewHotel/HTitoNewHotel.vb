Imports System.IO
Public Class HTitoNewHotel
    'PDTE: 
    '
    ' Quitar las Llamadas a GeneraFile
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


    Private mParaMoraPrefijoProduccion As String
    Private mParaMoraDimenHotel As String
    Private mParaMoraTMovCuenta As String
    Private mParaMoraTMovBanco As String
    Private mParaMoraTMovCliente As String

    Private mParaMoraTDocFactura As String
    Private mParaMoraTDocAbono As String
    Private mParaMoraTDocAnticipo As String
    Private mParaMoraTDocDeposito As String
    Private mParaIgicNegocio As String
    Private mParaIgicProducto As String

    Private mParaSufijoAnticipos As String
    Private mParaSufijoDepositos As String



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

    Private mExistenRegistros As Boolean = False



    ' OTROS 
    Private iASCII(63) As Integer       'Para conversión a MS-DOS
    Private AuxCif As String
    Private AuxCuenta As String


    Private mCtaSerieCredito As String
    Private mCtaSerieContado As String
    Private mCtaSerieAnulacion As String
    Private mCtaSerieNotaCredito As String

    Private mResultStr As String
    Private mResultInteger As Integer


    Private SQL As String
    Private Linea As Integer
    Private mTexto As String
    Private Filegraba As StreamWriter
    Private FileEstaOk As Boolean = False
    Private DbLeeCentral As C_DATOS.C_DatosOledb
    Private DbLeeHotel As C_DATOS.C_DatosOledb
    Private DbLeeHotelAux As C_DATOS.C_DatosOledb
    Private DbLeeHotelAux2 As C_DATOS.C_DatosOledb
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


    Private mParaDistingeAnticiposDeDepositos As Boolean
    Private mParaCta4b As String
    Private mParaCta4b2Efectivo As String
    Private mParaCta4b3Visa As String
    Private mParaSecc_DepNh As String
    Private mParaSecc_AntNh As String

    Private mAuxStr As String
    Private mVisaComprobante As Integer
    Private mVisaFactura As Integer
    Private mVisaFacturaSerie As String
    Private mVisaFacturaCif As String
    Private mVisaCfbcotmov As String

    Private mMovimientoNegativo As String




#Region "CONSTRUCTOR"
    Public Sub New(ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vStrConexionCentral As String,
    ByVal vStrConexionHotel As String, ByVal vFecha As Date, ByVal vFileName As String, ByVal vDebug As Boolean,
    ByVal vConrolDebug As System.Windows.Forms.TextBox, ByVal vListBox As System.Windows.Forms.ListBox,
    ByVal vStrConexionSpyro As String, ByVal vProgress As System.Windows.Forms.ProgressBar, ByVal vTrataDebitoNoFacturadoTpv As Boolean,
    ByVal vUsaTnhtMoviAuto As Boolean, ByVal vEmpNum As Integer, ByVal vForm As System.Windows.Forms.Form, ByVal vMuestraIncidencias As Boolean, ByVal vPerfilContable As String, ByVal vTrataCaja As Boolean)


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




        Dim Texto As String

        Texto = "Jose Juan queda pendiente que campo del web service se corresponde con 'liquidar por tipo de Documento'  caso facturación de Anticipos" & vbCrLf & vbCrLf
        Texto += "-" & vbCrLf & vbCrLf
        Texto += "- " & vbCrLf & vbCrLf
        Texto += vbCrLf & vbCrLf

        Texto += "Pendiente de Revisar yo  " & vbCrLf
        Texto += "Revisar La aulacion de recibos y la Cooreccion / Anulacion de Movimientos que estan contenidos en un recibo (asunto doc a liquidar  "

        Texto += "Revisar la respuesta del web servive caso de notas de abono , NO se hace el Update de Ok o del error OJO !!!!!!!!  "
        Texto += " Substituir el 3 de las instruciones If Me.WebServiceEnviar(3, 0, "") por   CInt(Me.DbLeeCentral.mDbLector.Item('asiento '))"


        MsgBox(Texto, MsgBoxStyle.Critical, "Atención")

        ' auditoria 
        'Me.FacturasSinCuentaContable()

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

            Me.DbLeeHotel = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
            Me.DbLeeHotel.AbrirConexion()
            Me.DbLeeHotel.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")



            Me.DbLeeHotelAux = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
            Me.DbLeeHotelAux.AbrirConexion()
            Me.DbLeeHotelAux.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


            Me.DbLeeHotelAux2 = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
            Me.DbLeeHotelAux2.AbrirConexion()
            Me.DbLeeHotelAux2.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

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
            SQL += "NVL(PARA_SECC_DEPNH,'<Ninguno>') AS PARA_SECC_DEPNH,  "
            SQL += "NVL(PARA_SECC_ANTNH,'<Ninguno>') AS PARA_SECC_ANTNH,  "

            SQL += "NVL(PARA_MORA_PREPROD,'?') AS  PARA_MORA_PREPROD ,"

            SQL += "NVL(PARA_MORA_TMOVCUENTA,'?') AS PARA_MORA_TMOVCUENTA , "
            SQL += "NVL(PARA_MORA_TMOVBANCO,'?') AS PARA_MORA_TMOVBANCO , "
            SQL += "NVL(PARA_MORA_TMOVCLIENTE,'?') AS PARA_MORA_TMOVCLIENTE , "
            SQL += "NVL(PARA_MORA_TDOCFACTURA,'?') AS PARA_MORA_TDOCFACTURA , "
            SQL += "NVL(PARA_MORA_TDOCABONO,'?') AS PARA_MORA_TDOCABONO , "
            SQL += "NVL(PARA_MORA_TDOCANTICIPO,'?') AS  PARA_MORA_TDOCANTICIPO, "

            SQL += "NVL(PARA_MORA_TDOCDEPOSITO,'?') AS  PARA_MORA_TDOCDEPOSITO, "
            SQL += "NVL(PARA_MORA_DIMENHOTEL,'?') AS  PARA_MORA_DIMENHOTEL, "

            SQL += "NVL(PARA_MORA_GRUPONEGOCIO,'?') AS  PARA_MORA_GRUPONEGOCIO , "
            SQL += "NVL(PARA_MORA_GRUPOPRODUCTO,'?') AS  PARA_MORA_GRUPOPRODUCTO "


            SQL += ",PARA_MORA_SUFI_ANTI "
            SQL += ",PARA_MORA_SUFI_DEPO "





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
                    Me.mParaDistingeAnticiposDeDepositos = True
                Else
                    Me.mParaDistingeAnticiposDeDepositos = False
                End If

                Me.mParaCta4b = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA4B"), String)
                Me.mParaCta4b2Efectivo = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA4B2"), String)
                Me.mParaCta4b3Visa = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA4B3"), String)
                Me.mParaSecc_DepNh = CType(Me.DbLeeCentral.mDbLector.Item("PARA_SECC_DEPNH"), String)
                Me.mParaSecc_AntNh = CType(Me.DbLeeCentral.mDbLector.Item("PARA_SECC_ANTNH"), String)




                mParaMoraPrefijoProduccion = CStr(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_PREPROD"))

                mParaMoraDimenHotel = CStr(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_DIMENHOTEL"))
                mParaMoraTMovCuenta = CStr(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_TMOVCUENTA"))
                mParaMoraTMovBanco = CStr(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_TMOVBANCO"))
                mParaMoraTMovCliente = CStr(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_TMOVCLIENTE"))

                mParaMoraTDocFactura = CStr(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_TDOCFACTURA"))
                mParaMoraTDocAbono = CStr(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_TDOCABONO"))
                mParaMoraTDocAnticipo = CStr(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_TDOCANTICIPO"))
                mParaMoraTDocDeposito = CStr(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_TDOCDEPOSITO"))

                mParaIgicNegocio = CStr(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_GRUPONEGOCIO"))
                mParaIgicProducto = CStr(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_GRUPOPRODUCTO"))


                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_SUFI_ANTI")) Then
                    Me.mParaSufijoAnticipos = ""
                Else
                    Me.mParaSufijoAnticipos = CStr(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_SUFI_ANTI"))
                End If


                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_SUFI_DEPO")) Then
                    Me.mParaSufijoDepositos = ""
                Else
                    Me.mParaSufijoDepositos = CStr(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_SUFI_DEPO"))
                End If



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
            SQL += " AND ASNT_AX_STATUS = 1"
            If CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Integer) > 0 Then
                If mMuestraIncidencias = True Then
                    MsgBox("Ya existen Movimientos de Integración Generados para esta Fecha " & Me.mFecha & "  en Navision " & vbCrLf & vbCrLf & "(Se Procede a Reintentar Errores ...)", MsgBoxStyle.Information, "Atención")

                End If
                Me.mExistenRegistros = True
            Else
                Me.mExistenRegistros = False
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
            End If





        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Borra Registros")
        End Try

    End Sub

    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                      ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                      , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
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
    ' copiada de arriba 1
    Private Sub InsertaOracleMorasol(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                      ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                      , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
                                        ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String _
                                     , vMoraNDoc As String, vMoraTipoMov As String, vMoraDimenNaturaleza As String, vMoraDimenDpto As String,
                                     vMoraDimenAcceso As String, vMoraReserva As String, vMoraLiquidar As String, vMoraTipoDoc As String, vWebServiceName As String)

        Try

            'If Me.mTipoAsiento = "DEBE" Then
            'Me.mDebe = vImonep
            'Me.mHbaber = 0
            'Else
            'Me.mDebe = 0
            'Me.mHbaber = vImonep

            'End If

            If Me.mTipoAsiento = "DEBE" Then
                If vImonep < 0 Then
                    vCfcptos_Cod = Me.mIndicadorHaber
                    Me.mDebe = 0
                    Me.mHbaber = vImonep * -1
                    vImonep = vImonep * -1
                    Me.mMovimientoNegativo = "(-) "
                Else
                    Me.mDebe = vImonep
                    Me.mHbaber = 0
                    Me.mMovimientoNegativo = ""
                End If
            End If
            If Me.mTipoAsiento = "HABER" Then
                If vImonep < 0 Then
                    vCfcptos_Cod = Me.mIndicadorDebe
                    Me.mDebe = vImonep * -1
                    vImonep = vImonep * -1
                    Me.mHbaber = 0
                    Me.mMovimientoNegativo = "(-) "
                Else
                    Me.mDebe = 0
                    Me.mHbaber = vImonep
                    Me.mMovimientoNegativo = ""
                End If
            End If




            SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM"
            SQL += ", ASNT_MORA_NDOCUMENTO, ASNT_MORA_TIPOMOV, ASNT_MORA_DIMENNATURALEZA,ASNT_MORA_DIMENDPTO,ASNT_MORA_DIMENACCESO,ASNT_MORA_RESERVA,ASNT_MORA_LIQUIDAR,ASNT_MORA_TIPODOC,ASNT_WEBSERVICE_NAME"
            SQL += ") VALUES ('"
            SQL += vTipo & " ','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(Me.mMovimientoNegativo & vAmpcpto, 1, 40).Replace("'", ",") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'"
            SQL += vMoraNDoc & "','" & vMoraTipoMov & "','" & vMoraDimenNaturaleza & "','" & vMoraDimenDpto & "','" & vMoraDimenAcceso & "','" & vMoraReserva & "','" & vMoraLiquidar & "','" & vMoraTipoDoc & "','" & vWebServiceName & "')"




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
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
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
    ' Copiado de arriba 4 
    Private Sub InsertaOracleMorasol(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
                                       ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vFactura As String, ByVal vSerie As String _
                                         , vMoraNDoc As String, vMoraTipoMov As String, vMoraDimenNaturaleza As String, vMoraDimenDpto As String, vMoraDimenAcceso As String,
                                     vMoraReserva As String, vMoraLiquidar As String, vAuxiliar1Morasol As String, vAuxiliar2Morasol As String, vTipoDoc As String, vWebServiceName As String, vIvaNegocio As String, vIvaProducto As String, vGrupoNegocio As String, vGrupoProducto As String)

        Try

            'If Me.mTipoAsiento = "DEBE" Then
            'Me.mDebe = vImonep
            'Me.mHbaber = 0
            'Else
            'Me.mDebe = 0
            'Me.mHbaber = vImonep
            'End If

            If Me.mTipoAsiento = "DEBE" Then
                If vImonep < 0 Then
                    vCfcptos_Cod = Me.mIndicadorHaber
                    Me.mDebe = 0
                    Me.mHbaber = vImonep * -1
                    vImonep = vImonep * -1
                    Me.mMovimientoNegativo = "(-) "
                Else
                    Me.mDebe = vImonep
                    Me.mHbaber = 0
                    Me.mMovimientoNegativo = ""
                End If
            End If
            If Me.mTipoAsiento = "HABER" Then
                If vImonep < 0 Then
                    vCfcptos_Cod = Me.mIndicadorDebe
                    Me.mDebe = vImonep * -1
                    vImonep = vImonep * -1
                    Me.mHbaber = 0
                    Me.mMovimientoNegativo = "(-) "
                Else
                    Me.mDebe = 0
                    Me.mHbaber = vImonep
                    Me.mMovimientoNegativo = ""
                End If
            End If


            SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE "
            SQL += ", ASNT_MORA_NDOCUMENTO, ASNT_MORA_TIPOMOV, ASNT_MORA_DIMENNATURALEZA,ASNT_MORA_DIMENDPTO,ASNT_MORA_DIMENACCESO,ASNT_MORA_RESERVA,ASNT_MORA_LIQUIDAR,ASNT_AUXILIAR_STRING,ASNT_MORA_TIPODOC,ASNT_WEBSERVICE_NAME,ASNT_MORA_FAC_IVANEGOCIO,ASNT_MORA_FAC_IVAPRODUCTO,ASNT_MORA_FAC_NEGOCIO,ASNT_MORA_FAC_PRODUCTO "
            SQL += ") VALUES ('"

            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(Me.mMovimientoNegativo & vAmpcpto, 1, 40).Replace("'", ",") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vFactura & "','" & vSerie & "','"
            SQL += vMoraNDoc & "','" & vMoraTipoMov & "','" & vMoraDimenNaturaleza & "','" & vMoraDimenDpto & "','" & vMoraDimenAcceso & "','" & vMoraReserva & "','" & vMoraLiquidar & "','" & vAuxiliar1Morasol & "','" & vTipoDoc & "','" & vWebServiceName & "','" & vIvaNegocio & "','" & vIvaProducto & "','" & vGrupoNegocio & "','" & vGrupoProducto & "')"



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
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
                                       ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String,
                                       ByVal vImprimir As String, ByVal vAuxiliarString As String,
                                       ByVal vMultiDiario As Boolean, ByVal vDptoNh As String)

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
    ' copiada de arriba 3
    Private Sub InsertaOracleMorasol(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
                                       ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String,
                                       ByVal vImprimir As String, ByVal vAuxiliarString As String,
                                       ByVal vMultiDiario As Boolean, ByVal vDptoNh As String _
                                    , vMoraNDoc As String, vMoraTipoMov As String, vMoraDimenNaturaleza As String, vMoraDimenDpto As String, vMoraDimenAcceso As String, vMoraReserva As String, vMoraLiquidar As String, vTipoDoc As String, vWebServiceName As String)

        Try

            'If Me.mTipoAsiento = "DEBE" Then
            ' Me.mDebe = vImonep
            'Me.mHbaber = 0
            'Else
            'Me.mDebe = 0
            'Me.mHbaber = vImonep
            'End If

            If Me.mTipoAsiento = "DEBE" Then
                If vImonep < 0 Then
                    vCfcptos_Cod = Me.mIndicadorHaber
                    Me.mDebe = 0
                    Me.mHbaber = vImonep * -1
                    vImonep = vImonep * -1
                    Me.mMovimientoNegativo = "(-) "
                Else
                    Me.mDebe = vImonep
                    Me.mHbaber = 0
                    Me.mMovimientoNegativo = ""
                End If
            End If
            If Me.mTipoAsiento = "HABER" Then
                If vImonep < 0 Then
                    vCfcptos_Cod = Me.mIndicadorDebe
                    Me.mDebe = vImonep * -1
                    vImonep = vImonep * -1
                    Me.mHbaber = 0
                    Me.mMovimientoNegativo = "(-) "
                Else
                    Me.mDebe = 0
                    Me.mHbaber = vImonep
                    Me.mMovimientoNegativo = ""
                End If
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

            SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_DPTO_CODI,ASNT_DPTO_DESC "
            SQL += ", ASNT_MORA_NDOCUMENTO, ASNT_MORA_TIPOMOV, ASNT_MORA_DIMENNATURALEZA,ASNT_MORA_DIMENDPTO,ASNT_MORA_DIMENACCESO,ASNT_MORA_RESERVA,ASNT_MORA_LIQUIDAR, ASNT_MORA_TIPODOC,ASNT_WEBSERVICE_NAME,ASNT_RECIBO"
            SQL += ") VALUES ('"

            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += DiarioVariable & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(Me.mMovimientoNegativo & vAmpcpto, 1, 40).Replace("'", ",") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste
            SQL += "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vDptoNh & "','" & Me.mAuxStr & "','"
            SQL += vMoraNDoc & "','" & vMoraTipoMov & "','" & vMoraDimenNaturaleza & "','" & vMoraDimenDpto & "','" & vMoraDimenAcceso & "','" & vMoraReserva & "','" & vMoraLiquidar & "','" & vTipoDoc & "','" & vWebServiceName & "','" & vMoraNDoc & "')"

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

    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
                                       ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String,
                                       ByVal vImprimir As String, ByVal vAuxiliarString As String,
                                       ByVal vMultiDiario As Boolean, ByVal vDptoNh As String, vTipoMovComp As String, vBancoCod As String, vComprobante As Integer)

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

            SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_DPTO_CODI,ASNT_DPTO_DESC,ASNT_CFBCOTMOV_COD,ASNT_BANCOS_COD,ASNT_CFBCOCOMP_COMPROB ) values ('"
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
            SQL += "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vDptoNh & "','" & Me.mAuxStr & "','" & vTipoMovComp & "','" & vBancoCod & "'," & vComprobante & ")"

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
    ' Copiada de arriba 2 
    Private Sub InsertaOracleMorasol(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
                                       ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String,
                                       ByVal vImprimir As String, ByVal vAuxiliarString As String,
                                       ByVal vMultiDiario As Boolean, ByVal vDptoNh As String, vTipoMovComp As String, vBancoCod As String, vComprobante As Integer _
        , vMoraNDoc As String, vMoraTipoMov As String, vMoraDimenNaturaleza As String, vMoraDimenDpto As String, vMoraDimenAcceso As String, vMoraReserva As String, vMoraLiquidar As String, vTipoDoc As String, vWebServiceName As String)

        Try

            'If Me.mTipoAsiento = "DEBE" Then
            'Me.mDebe = vImonep
            'Me.mHbaber = 0
            'Else
            'Me.mDebe = 0
            'Me.mHbaber = vImonep
            'End If


            If Me.mTipoAsiento = "DEBE" Then
                If vImonep < 0 Then
                    vCfcptos_Cod = Me.mIndicadorHaber
                    Me.mDebe = 0
                    Me.mHbaber = vImonep * -1
                    vImonep = vImonep * -1
                    Me.mMovimientoNegativo = "(-) "
                Else
                    Me.mDebe = vImonep
                    Me.mHbaber = 0
                    Me.mMovimientoNegativo = ""
                End If
            End If
            If Me.mTipoAsiento = "HABER" Then
                If vImonep < 0 Then
                    vCfcptos_Cod = Me.mIndicadorDebe
                    Me.mDebe = vImonep * -1
                    vImonep = vImonep * -1
                    Me.mHbaber = 0
                    Me.mMovimientoNegativo = "(-) "
                Else
                    Me.mDebe = 0
                    Me.mHbaber = vImonep
                    Me.mMovimientoNegativo = ""
                End If
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

            SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_DPTO_CODI,ASNT_DPTO_DESC,ASNT_CFBCOTMOV_COD,ASNT_BANCOS_COD,ASNT_CFBCOCOMP_COMPROB "
            SQL += ", ASNT_MORA_NDOCUMENTO, ASNT_MORA_TIPOMOV, ASNT_MORA_DIMENNATURALEZA,ASNT_MORA_DIMENDPTO,ASNT_MORA_DIMENACCESO,ASNT_MORA_RESERVA,ASNT_MORA_LIQUIDAR,ASNT_MORA_TIPODOC,ASNT_WEBSERVICE_NAME,ASNT_RECIBO"
            SQL += ") VALUES ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += DiarioVariable & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(Me.mMovimientoNegativo & vAmpcpto, 1, 40).Replace("'", ",") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste
            SQL += "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vDptoNh & "','" & Me.mAuxStr & "','" & vTipoMovComp & "','" & vBancoCod & "'," & vComprobante & ",'"
            SQL += vMoraNDoc & "','" & vMoraTipoMov & "','" & vMoraDimenNaturaleza & "','" & vMoraDimenDpto & "','" & vMoraDimenAcceso & "','" & vMoraReserva & "','" & vMoraLiquidar & "','" & vTipoDoc & "','" & vWebServiceName & "','" & vMoraNDoc & "')"
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
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                    ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                    , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
                                      ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String, ByVal vFactura As String, ByVal vSerie As String)

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
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vFactura & "','" & vSerie & "')"




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
    ' Copiado de arriba 5
    Private Sub InsertaOracleMorasol(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                    ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                    , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
                                      ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarString As String, ByVal vFactura As String, ByVal vSerie As String _
                                      , vMoraNDoc As String, vMoraTipoMov As String, vMoraDimenNaturaleza As String, vMoraDimenDpto As String, vMoraDimenAcceso As String, vMoraReserva As String, vMoraLiquidar As String, vTipoDoc As String, vWebServiceName As String)

        Try

            'If Me.mTipoAsiento = "DEBE" Then
            'Me.mDebe = vImonep
            'Me.mHbaber = 0
            'Else
            'Me.mDebe = 0
            'Me.mHbaber = vImonep
            'End If

            If Me.mTipoAsiento = "DEBE" Then
                If vImonep < 0 Then
                    vCfcptos_Cod = Me.mIndicadorHaber
                    Me.mDebe = 0
                    Me.mHbaber = vImonep * -1
                    vImonep = vImonep * -1
                    Me.mMovimientoNegativo = "(-) "
                Else
                    Me.mDebe = vImonep
                    Me.mHbaber = 0
                    Me.mMovimientoNegativo = ""
                End If
            End If
            If Me.mTipoAsiento = "HABER" Then
                If vImonep < 0 Then
                    vCfcptos_Cod = Me.mIndicadorDebe
                    Me.mDebe = vImonep * -1
                    vImonep = vImonep * -1
                    Me.mHbaber = 0
                    Me.mMovimientoNegativo = "(-) "
                Else
                    Me.mDebe = 0
                    Me.mHbaber = vImonep
                    Me.mMovimientoNegativo = ""
                End If
            End If


            SQL = "INSERT INTO TH_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE "
            SQL += ", ASNT_MORA_NDOCUMENTO, ASNT_MORA_TIPOMOV, ASNT_MORA_DIMENNATURALEZA,ASNT_MORA_DIMENDPTO,ASNT_MORA_DIMENACCESO,ASNT_MORA_RESERVA,ASNT_MORA_LIQUIDAR,ASNT_MORA_TIPODOC,ASNT_WEBSERVICE_NAME"
            SQL += ") VALUES ('"

            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(Me.mMovimientoNegativo & vAmpcpto, 1, 40).Replace("'", ",") & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','" & vFactura & "','" & vSerie & "','"
            SQL += vMoraNDoc & "','" & vMoraTipoMov & "','" & vMoraDimenNaturaleza & "','" & vMoraDimenDpto & "','" & vMoraDimenAcceso & "','" & vMoraReserva & "','" & vMoraLiquidar & "','" & vTipoDoc & "','" & vWebServiceName & "')"




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
                Me.SpyroCompruebaCuenta(CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_CFCTA_COD")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_TIPO_REGISTRO")),
                                        CInt(Me.DbLeeCentral.mDbLector.Item("ASNT_CFATOCAB_REFER")),
                                        CInt(Me.DbLeeCentral.mDbLector.Item("ASNT_LINEA")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_CFCPTOS_COD")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_AMPCPTO")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_NOMBRE")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("NUMERO")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("SERIE")))


            End While
            Me.DbLeeCentral.mDbLector.Close()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub SpyroCompruebaBancos()
        Try
            SQL = "SELECT ASNT_CFCTA_COD,ASNT_TIPO_REGISTRO,ASNT_CFATOCAB_REFER,ASNT_LINEA,ASNT_CFCPTOS_COD,"
            SQL += "NVL(ASNT_AMPCPTO,'?') AS ASNT_AMPCPTO,NVL(ASNT_NOMBRE,'?') AS ASNT_NOMBRE, "
            SQL += " NVL(ASNT_FACTURA_NUMERO,'?') AS NUMERO,NVL(ASNT_FACTURA_SERIE,'?') AS SERIE ,"
            SQL += " ASNT_BANCOS_COD,ASNT_CFBCOTMOV_COD,ASNT_CFBCOCOMP_COMPROB"
            SQL += " FROM TH_ASNT WHERE "
            SQL += "     ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum
            SQL += " AND ASNT_F_VALOR = '" & Me.mFecha & "'"
            SQL += " AND ASNT_CFBCOCOMP_COMPROB IS NOT NULL"

            Me.DbLeeCentral.TraerLector(SQL)
            While Me.DbLeeCentral.mDbLector.Read
                Me.SpyroCompruebaBanco(CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_CFCTA_COD")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_TIPO_REGISTRO")),
                                        CInt(Me.DbLeeCentral.mDbLector.Item("ASNT_CFATOCAB_REFER")),
                                        CInt(Me.DbLeeCentral.mDbLector.Item("ASNT_LINEA")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_CFCPTOS_COD")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_AMPCPTO")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_NOMBRE")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("NUMERO")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("SERIE")), CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_BANCOS_COD")))


            End While
            Me.DbLeeCentral.mDbLector.Close()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub SpyroCompruebaBanco(ByVal vCuenta As String, ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vLinea As Integer, ByVal vDebeHaber As String, ByVal vAmpcpto As String, ByVal vNombre As String, ByVal vFactura As String, ByVal vSerie As String, vBanco As String)
        Try

            Me.mTextDebug.Text = "Validando Bancos Spyro " & vCuenta.PadRight(20, CChar(" ")) & " Longitud : " & vCuenta.Length

            Me.mTextDebug.Update()
            '    Me.mForm.Update()


            SQL = "SELECT COD FROM BANCOS WHERE  EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND COD = '" & vBanco & "'"



            If IsNothing(Me.DbSpyro.EjecutaSqlScalar(SQL)) = True Then
                Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & "  No se localiza Banco de Spyro")
                Me.mListBoxDebug.Update()
                Me.mTexto = "SPYRO   : " & vCuenta & "  No se localiza Banco de Spyro"
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

            TotalRegistros = TotalRegistros + 1

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



            Me.mForm.ParentForm.Text = CStr(TotalRegistros)
        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
        End Try
    End Sub
    Private Sub GeneraFileACMultiDiario(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
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
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Mid(FechaAsiento, 5, 4) &
            Me.DiarioVariable.PadRight(4, CChar(" ")) &
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

            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
        End Try
    End Sub
    Private Sub GeneraFileACMultiDiarioComprobantesBancarios(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
     ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, ByVal vDiario As Boolean, vBancosCod As String, vCfbCotMov As String, vComprobante As Integer)
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
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Mid(FechaAsiento, 5, 4) &
            Me.DiarioVariable.PadRight(4, CChar(" ")) &
            " ".PadLeft(8, CChar(" ")) &
            " ".PadLeft(4, CChar(" ")) &
            vCfcta_Cod.PadRight(15, CChar(" ")) &
            vCfcptos_Cod.PadRight(4, CChar(" ")) &
            Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) &
            CType(vImonep, String).PadLeft(16, CChar(" ")) &
            "S" & FechaAsiento &
            Format(Me.mFecha, "ddMMyyyy") &
            " ".PadRight(40, CChar(" ")) &
            Me.mCfatotip_Cod.PadRight(4, CChar(" ")) &
              " ".PadRight(2, CChar(" ")) &
            " ".PadRight(6, CChar(" ")) &
             " ".PadLeft(8, CChar(" ")) &
           vBancosCod.PadRight(4, CChar(" ")) &
           vCfbCotMov.PadRight(4, CChar(" ")) &
           CStr(vComprobante).PadLeft(8, CChar(" ")))

            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

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


            TotalRegistros = TotalRegistros + 1
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

            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

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

            TotalRegistros = TotalRegistros + 1
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

            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

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

            TotalRegistros = TotalRegistros + 1

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

            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAA")
        End Try
    End Sub
    Private Sub GeneraFileFV(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String,
    ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String, ByVal vPendiente As Double)

        Try

            TotalRegistros = TotalRegistros + 1
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

            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileFV")
        End Try
    End Sub
    Private Sub GeneraFileFVDiariodeCobros(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String,
    ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String, ByVal vPendiente As Double)

        Try

            TotalRegistros = TotalRegistros + 1
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
            Format(Me.mFecha, "yyyy") &
            Me.mCfatodiari_Cod_2.PadRight(4, CChar(" ")) &
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

            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileFV")
        End Try
    End Sub
    Private Sub GeneraFileFV2(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String,
   ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String)

        Try

            TotalRegistros = TotalRegistros + 1
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


            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileFV")
        End Try
    End Sub
    Private Sub GeneraFileVF(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String,
   ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String)

        Try

            TotalRegistros = TotalRegistros + 1
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

            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileVF")
        End Try
    End Sub
    Private Sub GeneraFileIV(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vFactutipo_cod As String,
    ByVal vNfactura As Integer, ByVal vI_basmonemp As Double, ByVal vPj_iva As Double, ByVal vI_ivamonemp As Double, ByVal vX As String)


        Try

            TotalRegistros = TotalRegistros + 1
            '-------------------------------------------------------------------------------------------------
            '  Libro de Iva(CFIVALIN)
            '-------------------------------------------------------------------------------------------------
            ' 2016
            ' Me.mCfivatimpu_Cod.PadRight(2, CChar(" ")) &
            ' CAMBIO A 
            'Me.mCfivatimpu_Cod.PadRight(4, CChar(" ")) &

            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
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

            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileIV")
        End Try
    End Sub
    Private Sub GeneraFileCB(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String,
    ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String,
                             ByVal vCuenta As String, ByVal vCif As String, ByVal vPendiente As Double,
                             vBancosCod As String, vCfbcotmovCod As String, vComprobante As Integer)

        Try

            TotalRegistros = TotalRegistros + 1
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
             Me.mCfatodiari_Cod_2.PadRight(4, CChar(" ")) &
            " ".PadLeft(8, CChar(" ")) &
            " ".PadLeft(4, CChar(" ")) &
            "N" &
            Format(Me.mFecha, "ddMMyyyy"))


            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileCB")
        End Try
    End Sub
    Private Sub GeneraFileMG_old(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String,
    ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String, ByVal vPendiente As Double, vCfbcotmovCod As String, vBancosCod As String, vComprobante As String)

        Try

            TotalRegistros = TotalRegistros + 1
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
             Me.mCfatodiari_Cod_2.PadRight(4, CChar(" ")) &
             " ".PadLeft(8, CChar(" ")) &
             " ".PadLeft(4, CChar(" ")) &
             Mid(vBancosCod, 1, 4).PadRight(4, CChar(" ")) &
             vComprobante.PadLeft(8, CChar(" ")) &
             " ".PadRight(40, CChar(" ")) &
             "N")



            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileFV")
        End Try
    End Sub
    Private Sub GeneraFileMG(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String,
    ByVal vSerie As String, ByVal vNfactura As Integer, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String, ByVal vPendiente As Double, vCfbcotmovCod As String, vBancosCod As String, vComprobante As String)

        Try

            TotalRegistros = TotalRegistros + 1
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



            Me.mForm.ParentForm.Text = CStr(TotalRegistros)

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileFV")
        End Try
    End Sub
    Private Sub CerrarFichero()
        Try
            Filegraba.Close()
            FileEstaOk = False
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
            Me.DbLeeHotelAux2.CerrarConexion()
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
            SQL = "SELECT TNHT_MOVI.SERV_CODI SERVICIO,"
            SQL += "ROUND (SUM (MOVI_VLIQ), 2) AS TOTAL"
            SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI ,TNHT_SERV"
            SQL += " WHERE MOVI_DATR= '" & Me.mFecha & "'"
            SQL += " AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "
            SQL += " GROUP BY TNHT_MOVI.SERV_CODI"

            Me.DbLeeHotel.TraerLector(SQL)
            While Me.DbLeeHotel.mDbLector.Read

                Total = Total + CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

            End While
            Me.DbLeeHotel.mDbLector.Close()
            If Total <> 0 Then
                Linea = 1
                Me.mTipoAsiento = "DEBE"
                Me.mTotalProduccion = Total
                Me.InsertaOracleMorasol("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorDebe, "Producción Diaria", Total, "SI", "", "", "SI", "", Me.mParaMoraTMovCuenta, "", "", Me.mParaMoraDimenHotel, "", "", "", "WEBPRODUCCION")
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub VentasDepartamento()

        Dim Total As Double

        SQL = "Select TNHT_SERV.SERV_TICO, TNHT_SERV.SERV_DESC DEPARTAMENTO , NVL(TNHT_SERV.SERV_CTB1,'?') CUENTA,NVL(TNHT_SERV.SERV_CMBD,'?') DIMNATURALEZA ,NVL(TNHT_SERV.SERV_COMS,'?') DIMDEPARTAMENTO,"
        SQL += "ROUND (SUM (MOVI_VLIQ), 2) TOTAL "
        SQL += " ,TNHT_SERV.SERV_CODI"
        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_SERV"
        SQL += " WHERE (TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI) AND MOVI_DATR= '" & Me.mFecha & "'"

        SQL += " GROUP BY TNHT_SERV.SERV_TICO,TNHT_SERV.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CTB1,TNHT_SERV.SERV_CMBD ,TNHT_SERV.SERV_COMS"
        SQL += " ORDER BY TNHT_SERV.SERV_TICO,TNHT_SERV.SERV_DESC"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracleMorasol("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "Producción Diaria " & CStr(Me.DbLeeHotel.mDbLector("DEPARTAMENTO")), Total, "NO", "", "", "SI", "", Me.mParaMoraTMovCuenta, CStr(Me.DbLeeHotel.mDbLector("DIMNATURALEZA")), CStr(Me.DbLeeHotel.mDbLector("DIMDEPARTAMENTO")), Me.mParaMoraDimenHotel, "", "", "", "WEBPRODUCCION")
            End If
        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub


#End Region
#Region "ASIENTO-2 PAGOS A CUENTA"
    Private Sub TotalPagosaCuentaVisas()
        Try
            Dim Total As Double
            Dim Cuenta As String
            Dim Recibo As String

            Dim Reserva As String
            Dim TipoDoc As String
            Dim Texto As String


            SQL = "Select TNHT_MOVI.MOVI_VDEB TOTAL, TNHT_CACR.CACR_DESC TARJETA, NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA,"
            SQL += "NVL(MOVI_NUDO,' ') MOVI_NUDO,NVL(MOVI_DESC,' ') MOVI_DESC,NVL(SECC_CODI,'?') AS SECC_CODI,CACR_CTB3 "
            SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) RECI_COBR "
            SQL += ",TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA"


            ' 20180118
            SQL += " ,NVL(MOVI_CODR,0) AS MOVI_CODR,MOVI_DARE "

            SQL = SQL & " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
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

                If Me.mParaDistingeAnticiposDeDepositos = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                    Cuenta = Me.mParaCta4b3Visa
                    Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
                    '    TipoDoc = Me.mParaMoraTDocDeposito
                    TipoDoc = ""
                    Texto = " Depósito Recibido "
                Else
                    Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
                    '   TipoDoc = Me.mParaMoraTDocAnticipo
                    TipoDoc = ""
                    Texto = " Anticipo Recibido "
                End If

                If IsDBNull(Me.DbLeeHotel.mDbLector("RECI_COBR")) Then
                    ' si esta nulo es que es la contrapartida de un recibo anulado se llama a funcion que busca el numero de recibo anulado
                    Recibo = LocaLizaReciboAnulado(CInt(Me.DbLeeHotel.mDbLector("MOVI_CODR")), CDate(Me.DbLeeHotel.mDbLector("MOVI_DARE")))
                    Me.mAuxStr = ""
                Else
                    Recibo = CStr(Me.DbLeeHotel.mDbLector("RECI_COBR")).Trim
                    Me.mAuxStr = ""
                End If


                If IsDBNull(Me.DbLeeHotel.mDbLector("RESERVA")) Then
                    Reserva = ""
                Else
                    Reserva = CStr(Me.DbLeeHotel.mDbLector("RESERVA"))
                End If

                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Linea = Linea + 1
                    Me.mTipoAsiento = "DEBE"
                    '   Me.InsertaOracleMorasol("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & Texto & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", "Comprobante Bancario Nº: ", "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Me.mVisaCfbcotmov, "", Me.mVisaComprobante, Recibo, Me.mParaMoraTMovBanco, "", "", Me.mParaMoraDimenHotel, Reserva, "", TipoDoc, "WEBANTICIPOS")
                    Me.InsertaOracleMorasol("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & Texto & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", Me.mAuxStr, "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Me.mVisaCfbcotmov, "", Me.mVisaComprobante, Recibo, Me.mParaMoraTMovBanco, "", "", Me.mParaMoraDimenHotel, Reserva, "", "", "WEBANTICIPOS")

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
        Dim Recibo As String
        Dim Reserva As String

        Dim TipoDoc As String
        Dim Texto As String

        SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,"
        SQL += "FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO) RECI_COBR,NVL(MOVI_NUDO,' ') MOVI_NUDO,NVL(MOVI_DESC,' ') MOVI_DESC,NVL(SECC_CODI,'?') AS SECC_CODI "
        SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) RECI_COBR "
        SQL += ",TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA"

        ' 20180118
        SQL += " ,NVL(MOVI_CODR,0) AS MOVI_CODR,MOVI_DARE "

        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
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

            If Me.mParaDistingeAnticiposDeDepositos = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b2Efectivo
                Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
                '    TipoDoc = Me.mParaMoraTDocDeposito
                TipoDoc = ""
                Texto = " Depósito Recibido "
            Else
                Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
                '   TipoDoc = Me.mParaMoraTDocAnticipo
                TipoDoc = ""
                Texto = " Anticipo Recibido "
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RECI_COBR")) Then
                ' si esta nulo es que es la contrapartida de un recibo anulado se llama a funcion que busca el numero de recibo anulado
                Recibo = LocaLizaReciboAnulado(CInt(Me.DbLeeHotel.mDbLector("MOVI_CODR")), CDate(Me.DbLeeHotel.mDbLector("MOVI_DARE")))
                Me.mAuxStr = ""
            Else
                Recibo = CStr(Me.DbLeeHotel.mDbLector("RECI_COBR")).Trim
                Me.mAuxStr = ""
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("RESERVA")) Then
                Reserva = ""
            Else
                Reserva = CStr(Me.DbLeeHotel.mDbLector("RESERVA"))
            End If


            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "DEBE"
                '    Me.InsertaOracleMorasol("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & Texto & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", "", "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Recibo, Me.mParaMoraTMovBanco, "", "", Me.mParaMoraDimenHotel, Reserva, "", TipoDoc, "WEBANTICIPOS")
                Me.InsertaOracleMorasol("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & Texto & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", Me.mAuxStr, "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Recibo, Me.mParaMoraTMovBanco, "", "", Me.mParaMoraDimenHotel, Reserva, "", "", "WEBANTICIPOS")

            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetallePagosaCuentaVisas()
        Dim Total As Double
        Dim Cuenta As String = ""
        Dim Recibo As String
        Dim Reserva As String
        Dim TipoDoc As String
        Dim Texto As String
        Dim DimDepartamento As String

        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,NVL(MOVI_DESC,' ') MOVI_DESC,"
        SQL = SQL & " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA,TNHT_MOVI.SECC_CODI,SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) RECI_COBR,SECC_REFE AS DIMDEPARTAMENTO "

        ' 20180118
        SQL += " ,NVL(MOVI_CODR,0) AS MOVI_CODR,MOVI_DARE "

        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,"
        SQL = SQL & " TNHT_CACR,TNHT_RESE,TNHT_SECC WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL += "  And TNHT_MOVI.SECC_CODI = TNHT_SECC.SECC_CODI"

        SQL = SQL & " And TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " And TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
        SQL = SQL & " And TNHT_MOVI.TIRE_CODI = 1"
        SQL = SQL & " And TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
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

            If Me.mParaDistingeAnticiposDeDepositos = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b & Me.mParaSufijoDepositos
                '   TipoDoc = Me.mParaMoraTDocDeposito
                TipoDoc = ""
                Texto = "Depósito Recibido "
            Else
                Cuenta = Me.mCtaPagosACuenta & Me.mParaSufijoAnticipos
                '    TipoDoc = Me.mParaMoraTDocAnticipo
                TipoDoc = ""
                Texto = "Anticipo Recibido "
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RECI_COBR")) Then
                ' si esta nulo es que es la contrapartida de un recibo anulado se llama a funcion que busca el numero de recibo anulado
                Recibo = LocaLizaReciboAnulado(CInt(Me.DbLeeHotel.mDbLector("MOVI_CODR")), CDate(Me.DbLeeHotel.mDbLector("MOVI_DARE")))
                Me.mAuxStr = ""
            Else
                Recibo = CStr(Me.DbLeeHotel.mDbLector("RECI_COBR")).Trim
                Me.mAuxStr = ""
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RESERVA")) Then
                Reserva = ""
            Else
                Reserva = CStr(Me.DbLeeHotel.mDbLector("RESERVA"))
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("DIMDEPARTAMENTO")) Then
                DimDepartamento = ""
            Else
                DimDepartamento = CStr(Me.DbLeeHotel.mDbLector("DIMDEPARTAMENTO"))
            End If



            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                '   Me.InsertaOracleMorasol("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Texto & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Recibo, Me.mParaMoraTMovCliente, "", DimDepartamento, Me.mParaMoraDimenHotel, Reserva, "", TipoDoc, "WEBANTICIPOS")
                Me.InsertaOracleMorasol("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Texto & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", Me.mAuxStr, "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Recibo, Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, Reserva, "", TipoDoc, "WEBANTICIPOS")

            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetallePagosaCuentaOtrasFormas()
        Dim Total As Double
        Dim Cuenta As String = ""
        Dim Recibo As String
        Dim Reserva As String
        Dim TipoDoc As String
        Dim Texto As String
        Dim DimDepartamento As String

        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE,TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_DAVA,NVL(MOVI_DESC,' ') MOVI_DESC,TNHT_MOVI.SECC_CODI,SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) RECI_COBR,SECC_REFE AS DIMDEPARTAMENTO "

        ' 20180118
        SQL += " ,NVL(MOVI_CODR,0) AS MOVI_CODR,MOVI_DARE "

        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FORE,TNHT_RESE,TNHT_SECC WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "





        SQL += "  And TNHT_MOVI.SECC_CODI = TNHT_SECC.SECC_CODI"
        SQL = SQL & " And TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " And TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
        SQL = SQL & " And TNHT_MOVI.TIRE_CODI = 1"

        SQL = SQL & " And TNHT_MOVI.CACR_CODI Is NULL"
        SQL = SQL & " And TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
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

            If Me.mParaDistingeAnticiposDeDepositos = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b & Me.mParaSufijoDepositos
                '   TipoDoc = Me.mParaMoraTDocDeposito
                TipoDoc = ""
                Texto = "Depósito Recibido "
            Else
                Cuenta = Me.mCtaPagosACuenta & Me.mParaSufijoAnticipos
                '   TipoDoc = Me.mParaMoraTDocAnticipo
                TipoDoc = ""
                Texto = "Anticipo Recibido "
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RECI_COBR")) Then
                ' si esta nulo es que es la contrapartida de un recibo anulado se llama a funcion que busca el numero de recibo anulado
                Recibo = LocaLizaReciboAnulado(CInt(Me.DbLeeHotel.mDbLector("MOVI_CODR")), CDate(Me.DbLeeHotel.mDbLector("MOVI_DARE")))
                Me.mAuxStr = ""
            Else
                Recibo = CStr(Me.DbLeeHotel.mDbLector("RECI_COBR")).Trim
                Me.mAuxStr = ""
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RESERVA")) Then
                Reserva = ""
            Else
                Reserva = CStr(Me.DbLeeHotel.mDbLector("RESERVA"))
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("DIMDEPARTAMENTO")) Then
                DimDepartamento = ""
            Else
                DimDepartamento = CStr(Me.DbLeeHotel.mDbLector("DIMDEPARTAMENTO"))
            End If

            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                'Me.InsertaOracleMorasol("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Texto & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", "F. Valor " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String), "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Recibo, Me.mParaMoraTMovCliente, "", DimDepartamento, Me.mParaMoraDimenHotel, Reserva, "", TipoDoc, "WEBANTICIPOS")
                Me.InsertaOracleMorasol("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Texto & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", Me.mAuxStr, "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Recibo, Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, Reserva, "", TipoDoc, "WEBANTICIPOS")
            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub TotalPagosaCuentaVisasComision()
        Try
            Dim Total As Double
            Dim TotalComision As Double
            '  Dim vCentroCosto As String

            Dim TipoDoc As String
            Dim Recibo As String

            Dim DimDepartamento As String

            ' 20180118
            ' SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA,NVL(TNHT_CACR.CACR_CTB2,'0') CUENTAGASTO,TNHT_CACR.CACR_COMI,NVL(TNHT_CACR.CACR_CTB3,'?') AS NATURALEZA,TNHT_MOVI.SECC_CODI,SECC_REFE AS DIMDEPARTAMENTO "
            SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA,NVL(TNHT_CACR.CACR_CTB2,'0') CUENTAGASTO,TNHT_CACR.CACR_COMI,NVL(TNHT_CACR.CACR_CTB3,'?') AS NATURALEZA,TNHT_MOVI.SECC_CODI,SECC_REFE AS DIMDEPARTAMENTO "

            SQL += " ,SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) AS RECI_COBR "

            ' 20180118
            SQL += " ,NVL(MOVI_CODR,0) AS MOVI_CODR,MOVI_DARE "

            SQL += " FROM TNHT_MOVI,TNHT_CACR,TNHT_RESE,TNHT_SECC WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL += "  AND TNHT_MOVI.SECC_CODI = TNHT_SECC.SECC_CODI"
            SQL += " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
            SQL += " AND TNHT_MOVI.TIRE_CODI = 1"
            SQL += " AND TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
            SQL += " AND TNHT_MOVI.MOVI_VDEB <> 0"

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

            ' 20180118
            '   SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CTBA,TNHT_CACR.CACR_CTB2,TNHT_CACR.CACR_COMI,TNHT_CACR.CACR_CTB3,TNHT_MOVI.SECC_CODI"
            '   SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20),SECC_REFE"




            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read




                If Me.mParaDistingeAnticiposDeDepositos = True And CStr(Me.DbLeeHotel.mDbLector("SECC_CODI")) = Me.mParaSecc_DepNh Then
                    '   TipoDoc = Me.mParaMoraTDocDeposito
                    TipoDoc = ""
                Else
                    '   TipoDoc = Me.mParaMoraTDocAnticipo
                    TipoDoc = ""
                End If


                If IsDBNull(Me.DbLeeHotel.mDbLector("RECI_COBR")) Then
                    ' si esta nulo es que es la contrapartida de un recibo anulado se llama a funcion que busca el numero de recibo anulado
                    Recibo = LocaLizaReciboAnulado(CInt(Me.DbLeeHotel.mDbLector("MOVI_CODR")), CDate(Me.DbLeeHotel.mDbLector("MOVI_DARE")))
                Else
                    Recibo = CStr(Me.DbLeeHotel.mDbLector("RECI_COBR")).Trim
                End If


                If IsDBNull(Me.DbLeeHotel.mDbLector("DIMDEPARTAMENTO")) Then
                    DimDepartamento = ""
                Else
                    DimDepartamento = CStr(Me.DbLeeHotel.mDbLector("DIMDEPARTAMENTO"))
                End If


                'SQL = "SELECT NVL(PARA_CENTRO_COSTO_COMI,'0') FROM TH_PARA "
                'SQL += " WHERE  PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                'SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                'vCentroCosto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)


                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                If Total <> 0 Then

                    TotalComision = (CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * CType(Me.DbLeeHotel.mDbLector("CACR_COMI"), Double)) / 100

                    If TotalComision <> 0 Then
                        Linea = Linea + 1
                        Me.mTipoAsiento = "HABER"
                        '    Me.InsertaOracleMorasol("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "COMISION ANTICIPOS " & CType(Me.DbLeeHotel.mDbLector("CACR_COMI"), String) & " %  " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", "", "SI", "", Me.Multidiario, "", Recibo, Me.mParaMoraTMovBanco, "", "", Me.mParaMoraDimenHotel, "", "", "", "WEBCOBROS")
                        Me.InsertaOracleMorasol("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "COMISION ANTICIPOS " & CType(Me.DbLeeHotel.mDbLector("CACR_COMI"), String) & " %  " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", CStr(Total), "SI", "", Me.Multidiario, "", Recibo, Me.mParaMoraTMovBanco, "", "", Me.mParaMoraDimenHotel, "", "", "", "WEBCOBROS")


                        Linea = Linea + 1
                        Me.mTipoAsiento = "DEBE"
                        '  Me.InsertaOracleMorasol("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTAGASTO"), String), Me.mIndicadorDebe, "COMISION ANTICIPOS " & CType(Me.DbLeeHotel.mDbLector("CACR_COMI"), String) & " %  " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", vCentroCosto, "SI", "", Me.Multidiario, "", Recibo, Me.mParaMoraTMovCuenta, CStr(Me.DbLeeHotel.mDbLector("NATURALEZA")), "", Me.mParaMoraDimenHotel, "", "", TipoDoc, "WEBCOBROS")
                        Me.InsertaOracleMorasol("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTAGASTO"), String), Me.mIndicadorDebe, "COMISION ANTICIPOS " & CType(Me.DbLeeHotel.mDbLector("CACR_COMI"), String) & " %  " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", CStr(Total), "SI", "", Me.Multidiario, "", Recibo, Me.mParaMoraTMovCuenta, CStr(Me.DbLeeHotel.mDbLector("NATURALEZA")), "", Me.mParaMoraDimenHotel, "", "", TipoDoc, "WEBCOBROS")

                    End If


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

            Dim Dni As String
            Dim Cuenta As String = "0"
            Dim Titular As String
            Dim Reserva As String = ""


            ' TOTAL FACTURA DESPUES DEL DESCUENTO FONANCIERO 
            SQL = "SELECT  TNHT_FACT.FACT_STAT AS ESTADO, TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,TNHT_FACT.FACT_TOTA TOTAL,TNHT_FACT.FACT_VALO VALOR,TNHT_FACT.FACT_CONT PENDIENTE,NVL(ENTI_CODI,'') AS ENTI_CODI,NVL(CCEX_CODI,'') AS CCEX_CODI,NVL(CLIE_CODI,'0') AS CLIENTE "
            SQL += " , NVL(TNHT_FACT.FACT_TITU,'') TITULAR ,TNHT_FACT.FAAN_CODI "
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


                Cuenta = GetNewHotel.DevuelveCuentaContabledeFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))
                Dni = GetNewHotel.DevuelveDniCifContabledeFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))



                Reserva = GetReservasdeUnaFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))


                ' Algunos Controles


                If Dni = "0" Then
                    Me.mTexto = "NEWHOTEL: " & "CIF no válido para descripción de Movimiento =  " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String) & " " & CType(Me.DbLeeHotel.mDbLector("TITULAR"), String).Replace("'", "''")
                    Me.mListBoxDebug.Items.Add(Me.mTexto)

                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

                End If


                ' FACTURAS TRANSFERIDAS A CONTABILIDAD SIBN CODIGO DE ENTIDAD NI CUENTA NO ALOJADO
                If CType(Me.DbLeeHotel.mDbLector("ESTADO"), String) = "2" Or CType(Me.DbLeeHotel.mDbLector("ESTADO"), String) = "3" Then
                    If IsDBNull(Me.DbLeeHotel.mDbLector("ENTI_CODI")) = True And IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = True Then
                        Cuenta = InputBox("No se puede Determinar una Cuenta Contable , Factura = " & CStr(Me.DbLeeHotel.mDbLector("DESCRIPCION")) & " Titular = " & CStr(Me.DbLeeHotel.mDbLector("TITULAR")), "Atención Ingrese Cuenta (10 Dígitos)")
                        Dni = InputBox("No se puede Determinar un DNI/CIF , Factura = " & CStr(Me.DbLeeHotel.mDbLector("DESCRIPCION")) & " Titular = " & CStr(Me.DbLeeHotel.mDbLector("TITULAR")), "Atención Ingrese un Nif / Cif")
                        Me.mForm.Update()

                    End If
                End If




                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)



                Me.mTipoAsiento = "DEBE"
                ' ? 
                If TotalFactura > 0 Then
                    Me.InsertaOracleMorasol("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebeFac, "Documento " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Dni, Titular, "SI", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, Reserva, "", "", "", Me.mParaMoraTDocFactura, "WEBFACTURAS", "", "", "", "")
                Else
                    Me.InsertaOracleMorasol("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebeFac, "Documento " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Dni, Titular, "SI", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, Reserva, "", "", "", Me.mParaMoraTDocAbono, "WEBFACTURAS", "", "", "", "")
                End If


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
    Private Sub NFacturasSalidaLineasTito()
        Try
            Dim TotalFactura As Double



            Dim Dni As String
            Dim Cuenta As String = "0"

            Dim Reserva As String = ""


            ' TOTAL FACTURA DESPUES DEL DESCUENTO FONANCIERO 
            SQL = "SELECT  TNHT_FACT.FACT_STAT AS ESTADO, TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,TNHT_FACT.FACT_TOTA TOTAL,TNHT_FACT.FACT_VALO VALOR "
            SQL += "FROM TNHT_FACT "
            SQL += "WHERE "
            SQL += "(TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "
            SQL += " ORDER BY TNHT_FACT.FACT_CODI ASC"


            Dim GetNewHotel As New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)


            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)


                ' DETERMINAR EL TIPO DE FACTURA 

                Cuenta = GetNewHotel.DevuelveCuentaContabledeFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))
                Dni = GetNewHotel.DevuelveDniCifContabledeFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))


                Reserva = GetReservasdeUnaFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))





                ' loop lineas factura


                SQL = "  SELECT "
                SQL += "         TNHT_FACT.FACT_DAEM, "
                SQL += "         TNHT_FACT.FACT_CODI AS NUMERO, "
                SQL += "         NVL (TNHT_FACT.SEFA_CODI, '?') SERIE, "
                SQL += "         TNHT_FACT.FACT_CODI || '/' || TNHT_FACT.SEFA_CODI DESCRIPCION, "
                SQL += "         TNHT_FACT.FACT_TOTA TOTAL, "
                SQL += "         TNHT_FACT.FACT_VALO VALOR, "
                SQL += "        "
                SQL += "         NVL (TNHT_SERV.SERV_DESC, ' ') CONCEPTO, "
                SQL += "         TNHT_SERV.SERV_CODI AS SERVICIO, "
                SQL += "         NVL(TNHT_SERV.SERV_COMS,'?') AS DIMDEP, "
                SQL += "         TNHT_MOVI.MOVI_VCRE AS LINBRUT, "
                SQL += "         TNHT_MOVI.MOVI_VLIQ AS LINBASE, "
                SQL += "         TNHT_MOVI.MOVI_IMP1 AS LINIMPU, "
                SQL += "         NVL (TNHT_TIVA.TIVA_PERC, '0') AS LINPORCEIGIC, "
                SQL += "         FAAN_CODI "
                SQL += "    "
                SQL += "    FROM TNHT_FACT, "
                SQL += "         TNHT_FAMO, "
                SQL += "         VNHT_MOVH TNHT_MOVI, "
                SQL += "         TNHT_TIVA, "
                SQL += "         TNHT_SERV "
                SQL += "   WHERE     TNHT_FACT.FACT_CODI = TNHT_FAMO.FACT_CODI "
                SQL += "         AND TNHT_FACT.SEFA_CODI = TNHT_FAMO.SEFA_CODI "
                SQL += "         AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
                SQL += "         AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
                SQL += "         AND TNHT_MOVI.MOVI_IVA1 = TNHT_TIVA.TIVA_CODI(+) "
                SQL += "         AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "
                SQL += "         AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "
                SQL += "         AND TNHT_FACT.FACT_CODI = " & CInt(Me.DbLeeHotel.mDbLector("NUMERO"))
                SQL += "         AND TNHT_FACT.SEFA_CODI = " & "'" & CStr(Me.DbLeeHotel.mDbLector("SERIE")) & "' "

                SQL += "         AND TNHT_MOVI.MOVI_TIMO = '1' "
                SQL += "ORDER BY TNHT_FACT.SEFA_CODI ASC, "
                SQL += "         TNHT_FACT.FACT_CODI, "
                SQL += "         TNHT_MOVI.MOVI_DATR, "
                SQL += "         TNHT_MOVI.SERV_CODI ASC "

                Me.DbLeeHotelAux.TraerLector(SQL)

                While Me.DbLeeHotelAux.mDbLector.Read
                    Linea = Linea + 1
                    Me.mTipoAsiento = "DEBE"

                    If TotalFactura > 0 Then
                        Me.InsertaOracleMorasol("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebeFac, "Documento " & CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")) & " " & CType(Me.DbLeeHotelAux.mDbLector("CONCEPTO"), String), CDec(Me.DbLeeHotelAux.mDbLector("LINBASE")), "NO", Dni, "", "SI", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCliente, "", CStr(Me.DbLeeHotelAux.mDbLector("DIMDEP")), Me.mParaMoraDimenHotel, Reserva, "", "", "", Me.mParaMoraTDocFactura, "WEBFACTURAS", "", "", "", "")
                    Else
                        Me.InsertaOracleMorasol("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebeFac, "Documento " & CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")) & " " & CType(Me.DbLeeHotelAux.mDbLector("CONCEPTO"), String), CDec(Me.DbLeeHotelAux.mDbLector("LINBASE")), "NO", Dni, "", "SI", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCliente, "", CStr(Me.DbLeeHotelAux.mDbLector("DIMDEP")), Me.mParaMoraDimenHotel, Reserva, "", "", "", Me.mParaMoraTDocAbono, "WEBFACTURAS", "", "", "", "")
                    End If


                End While
                Me.DbLeeHotelAux.mDbLector.Close()




            End While
            GetNewHotel.CerrarConexiones()
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
            Dim Reserva As String

            '   Dim Dni As String
            Dim Cuenta As String = "0"

            Dim IvaNegocio As String = ""
            Dim IvaProducto As String = ""




            SQL = "SELECT   TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "TNHT_FAIV.FAIV_TAXA AS TIPO, TNHT_FAIV.FAIV_INCI,ROUND((FAIV_INCI-FAIV_VIMP),2) BASE, ROUND(TNHT_FAIV.FAIV_VIMP,2) IGIC, "
            'SQL += Me.mParaTextoIva & " ' || FAIV_TAXA ||'%  '|| TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,ROUND(TNHT_FACT.FACT_TOTA,2) TOTAL,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X "
            SQL += "TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,ROUND(TNHT_FACT.FACT_TOTA,2) TOTAL,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X "
            SQL += ",TIVA_CTB1 IVANEGOCIO,TIVA_CCVL IVAPRODUCTO "

            SQL += ",TNHT_FACT.FACT_VALO VALOR "
            SQL += " FROM TNHT_FAIV, TNHT_FACT,TNHT_TIVA "
            SQL += "WHERE TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "AND TNHT_FAIV.TIVA_CODI = TNHT_TIVA.TIVA_CODI "
            SQL += "AND (TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "
            SQL += "ORDER BY TNHT_FACT.SEFA_CODI ASC, TNHT_FACT.FACT_CODI ASC"


            'Dim GetNewHotel As New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
                TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)


                TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
                TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

                '   TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)

                TotalFactura = TotalBase + TotalIva

                Reserva = GetReservasdeUnaFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))


                ' Cuenta = GetNewHotel.DevuelveCuentaContabledeFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))
                ' Dni = GetNewHotel.DevuelveDniCifContabledeFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))



                If IsDBNull(Me.DbLeeHotel.mDbLector("IVANEGOCIO")) = False Then
                    IvaNegocio = Me.DbLeeHotel.mDbLector("IVANEGOCIO")
                Else
                    IvaNegocio = ""
                End If

                If IsDBNull(Me.DbLeeHotel.mDbLector("IVAPRODUCTO")) = False Then
                    IvaProducto = Me.DbLeeHotel.mDbLector("IVAPRODUCTO")
                Else
                    IvaProducto = ""
                End If


                Me.mTipoAsiento = "HABER"
                If TotalIva <> 0 Then
                    Me.InsertaOracleMorasol("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "Documento " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Me.mClientesContadoCif, "", "SI", CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCuenta, "", "", Me.mParaMoraDimenHotel, Reserva, "", "", "", Me.mParaMoraTDocFactura, "WEBFACTURAS", IvaNegocio, IvaProducto, Me.mParaIgicNegocio, Me.mParaIgicProducto)
                Else
                    Me.InsertaOracleMorasol("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "Documento " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Me.mClientesContadoCif, "", "SI", CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCuenta, "", "", Me.mParaMoraDimenHotel, Reserva, "", "", "", Me.mParaMoraTDocAbono, "WEBFACTURAS", IvaNegocio, IvaProducto, Me.mParaIgicNegocio, Me.mParaIgicProducto)
                End If


                Me.mTextDebug.Text = "Detalle Igic FActura " & CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String)
                Me.mTextDebug.Update()

            End While
            'GetNewHotel.CerrarConexiones()
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

            SQL = "Select "
            SQL += "TNHT_FAIV.FAIV_TAXA As TIPO, SUM((FAIV_INCI - FAIV_VIMP)) BASE, ROUND(SUM(TNHT_FAIV.FAIV_VIMP), 2) IGIC, NVL(TIVA_CTB1,'0') CUENTA, '"
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
                Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, DescripcionAsiento, TotalIva, "NO", Me.mClientesContadoCif, "", "SI")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, DescripcionAsiento, TotalIva)

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
                        Cuenta = mCtaSerieCredito
                        TipoSerie = " Crédito "
                    ElseIf Cuenta = "2" Then
                        Cuenta = mCtaSerieContado
                        TipoSerie = " Contado "
                    ElseIf Cuenta = "6" Then
                        Cuenta = mCtaSerieAnulacion
                        TipoSerie = " Anulación "
                    Else
                        Cuenta = Me.mCtaManoCorriente
                    End If
                Else
                    Cuenta = "0"
                End If




                Linea = Linea + 1
                TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
                TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)


                TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
                TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)


                DescripcionAsiento = "Total Serie " & CStr(Me.DbLeeHotel.mDbLector("SERIE")) & TipoSerie

                Me.mTipoAsiento = "HABER"
                'Me.InsertaOracleMorasol("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, DescripcionAsiento, TotalBase, "NO", "", "", "SI", "0", "XMANO" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), "", Me.mParaMoraTMovCuenta, "", "", Me.mParaMoraDimenHotel, "", "", "", "", "", "WEBFACTURAS", "", "", "", "")
                Me.InsertaOracleMorasol("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, DescripcionAsiento, TotalBase, "NO", "", "", "SI", "0", "XMANO" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), "", Me.mParaMoraTMovCuenta, "", "", Me.mParaMoraDimenHotel, "", "", "", "", "", "CONTRAPARTIDA", "", "", "", "")


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Detalle de Impuesto")
        End Try
    End Sub
    Private Sub NFacturasSalidaBaseImponible()

        Try


            Dim TotalBase As Double

            Dim DescripcionAsiento As String = ""


            Dim TipoSerie As String = ""


            Dim Dni As String
            Dim Cuenta As String = "0"
            Dim Titular As String
            Dim Reserva As String = ""

            SQL = "SELECT "
            SQL += "TNHT_FACT.SEFA_CODI AS SERIE,TNHT_FACT.FACT_CODI AS NUMERO ,SUM((FAIV_INCI-FAIV_VIMP)) BASE, ROUND(SUM(TNHT_FAIV.FAIV_VIMP),2) IGIC  "

            SQL += ",TNHT_FACT.FACT_CODI || '/' || TNHT_FACT.SEFA_CODI AS DESCRIPCION,FACT_TITU  AS TITULAR  "
            SQL += "FROM TNHT_FAIV, TNHT_FACT,TNHT_TIVA "
            SQL += "WHERE TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "AND TNHT_FAIV.TIVA_CODI = TNHT_TIVA.TIVA_CODI "
            SQL += "AND (TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "
            SQL += "GROUP BY TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI,FACT_TITU "



            Dim GetNewHotel As New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                ' Se usa cuenta mano corriente ?
                '  Cuenta = GetNewHotel.DevuelveCuentaContabledeFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))
                Dni = GetNewHotel.DevuelveDniCifContabledeFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))

                Reserva = GetReservasdeUnaFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))

                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)


                Linea = Linea + 1


                TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
                TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)




                Me.mTipoAsiento = "HABER"

                If TotalBase > 0 Then
                    Me.InsertaOracleMorasol("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, mCtaManoCorriente, Me.mIndicadorHaber, "Documento " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalBase, "NO", Dni, Titular, "SI", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCuenta, "", "", Me.mParaMoraDimenHotel, Reserva, "", "", "", Me.mParaMoraTDocFactura, "WEBFACTURAS", "", "", "", "")
                Else
                    Me.InsertaOracleMorasol("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, mCtaManoCorriente, Me.mIndicadorHaber, "Documento " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalBase, "NO", Dni, Titular, "SI", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCuenta, "", "", Me.mParaMoraDimenHotel, Reserva, "", "", "", Me.mParaMoraTDocAbono, "WEBFACTURAS", "", "", "", "")
                End If



                'Me.InsertaOracleMorasol("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, DescripcionAsiento, TotalBase, "NO", "", "", "SI", "0", "XMANO" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), "", Me.mParaMoraTMovCuenta, "", "", Me.mParaMoraDimenHotel, "", "", "", "", "", "WEBFACTURAS", "", "", "", "")


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Detalle de Impuesto")
        End Try
    End Sub
#Region "ASIENTO-3B"
    Private Sub DescuentosFinancieros()

        Try

            Dim Cuenta As String = "0"
            Dim Total As Double
            Dim Documento As String
            ' TOTAL FACTURA DESPUES DEL DESCUENTO FONANCIERO 
            SQL = "Select   TNHT_FACT.FACT_CODI As NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE "
            SQL += "FROM TNHT_FACT "
            SQL += "WHERE "
            SQL += "(TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "
            SQL += " ORDER BY TNHT_FACT.FACT_CODI ASC"


            Dim GetNewHotel As New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)


            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read


                Cuenta = Me.mCtaManoCorriente
                Documento = Me.DbLeeHotel.mDbLector("NUMERO") & "/" & Me.DbLeeHotel.mDbLector("SERIE")

                Total = GetNewHotel.GetValorDescotandoFinanciero(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))
                Total = Decimal.Round(CType(Total, Decimal), 2)

                If Total <> 0 Then
                    Linea = Linea + 1
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracleMorasol("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Dto Financiero " & Documento, Total, "NO", "", "", "SI", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), "", "", "", "", "", "", "FIN", "AUX1", "AUX2", "WEBPDTE")

                    ' Contrapartida cuenta de gastos
                    Linea = Linea + 1
                    Cuenta = "600"
                    Me.mTipoAsiento = "DEBE"

                    Me.InsertaOracleMorasol("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, "Dto Financiero " & Documento, Total, "NO", "", "", "SI", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), "", "", "", "", "", "", "FIN", "AUX1", "AUX2", "WEBPDTE")

                End If


            End While
            GetNewHotel.CerrarConexiones()
            '    MsgBox("CONEXIONES CERRADAS")
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Descuentos Financieros")
        End Try
    End Sub
#End Region

#End Region

#Region "ASIENTO-51"
    Private Sub NotasDeCreditoEntidadTotalLiquido()
        Dim Total As Double
        SQL = "Select NCRE_DAEM As FECHA, SUM(TNHT_MOVI.MOVI_VLIQ) As TOTAL "
        SQL += "FROM " & Me.mStrHayHistorico & " TNHT_MOVI , TNHT_SECC, TNHT_SERV, TNHT_FORE, TNHT_TIRE, TNHT_RECI, TNHT_MCRE, TNHT_NCRE "
        SQL += "WHERE TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI(+) And "
        SQL += "TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+) And "
        SQL += "TNHT_MOVI.SECC_CODI = TNHT_SECC.SECC_CODI And "
        SQL += "TNHT_MOVI.TIRE_CODI = TNHT_TIRE.TIRE_CODI(+) And "
        SQL += "TNHT_MOVI.MOVI_CODI = TNHT_RECI.MOVI_CODI(+) And "
        SQL += "TNHT_MOVI.MOVI_DARE = TNHT_RECI.MOVI_DATR(+) And "
        SQL += "TNHT_MOVI.PAPO_CODI = TNHT_RECI.PAPO_CODI(+) And "
        SQL += "TNHT_MOVI.MOVI_CODI = TNHT_MCRE.MOVI_CODI And "
        SQL += "TNHT_MOVI.MOVI_DARE = TNHT_MCRE.MOVI_DARE And "
        SQL += "TNHT_MCRE.NCRE_CODI = TNHT_NCRE.NCRE_CODI And "
        SQL += "TNHT_MCRE.SEFA_CODI = TNHT_NCRE.SEFA_CODI And "
        SQL += "TNHT_MOVI.MOVI_TIMO = 1 And "
        SQL += "TNHT_MOVI.PAPO_CODI = 1 And "
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
        Dim Reserva As String

        Dim Dni As String = ""
        Dim Titular As String = ""

        Dim Texto As String = ""
        Dim FacturaAnulada As String = ""

        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEDO_CODI AS SERIE, TNHT_NCRE.NCRE_DOCU AS NUMERO,TNHT_NCRE.NCRE_DOCU||'/'||TNHT_NCRE.SEDO_CODI DESCRIPCION,NCRE_VALO AS TOTAL, "
        SQL += " NCRE_TITU, NCRE_DAEM,NVL(ENTI_NCON_AF,0) CUENTA ,NVL(ENTI_NUCO,0) CIF,NVL(ENTI_NOME,'?') AS NOMBRE,NCRE_ANUL AS ANULADA"
        SQL += " ,SEFA_CODI AS SERIE2"
        SQL += ",FACT_CODI,FACT_CODI || '/' || FACT_SEFA AS FANULADA"

        SQL += " FROM TNHT_NCRE, TNHT_ENTI"
        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL += " AND TNHT_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " ORDER BY TNHT_NCRE.NCRE_CODI"

        Me.DbLeeHotel.TraerLector(SQL)


        Linea = 0
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalPendiente = 0
            Total = Decimal.Round(CType(Total, Decimal), 2)
            TotalPendiente = Decimal.Round(CType(TotalPendiente, Decimal), 2)



            Cuenta = CStr(Me.DbLeeHotel.mDbLector("CUENTA"))
            Reserva = GetReservasdeUnaNotaDeCredito(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE2")))

            If IsDBNull(Me.DbLeeHotel.mDbLector("FACT_CODI")) = False Then
                FacturaAnulada = Me.DbLeeHotel.mDbLector("FANULADA")
                Texto = "Doc: " & CStr(Me.DbLeeHotel.mDbLector("DESCRIPCION")) & " Sobre Factura " & FacturaAnulada
            Else
                Texto = "Doc: " & CStr(Me.DbLeeHotel.mDbLector("DESCRIPCION"))
            End If



            Me.mTipoAsiento = "HABER"
            ' PDTE: HACER POR LINEAS NO POR TOTAL
            ' desbloquear

            '     Me.InsertaOracleMorasol("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebeFac, "Documento " & CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Total, "NO", "", "", "SI", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCliente, "", "DIM-DEP", Me.mParaMoraDimenHotel, Reserva, "", "", "", Me.mParaMoraTDocAbono, "WEBFACTURAS", "", "", "", "")
            Me.InsertaOracleMorasol("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaberFac, Texto, Total, "NO", Dni, Titular, "SI", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, Reserva, "", "", "", Me.mParaMoraTDocAbono, "WEBFACTURAS", "", "", "", "")






        End While
        Me.DbLeeHotel.mDbLector.Close()


        '' ANULADAS 
        SQL = "Select"
        SQL += " TNHT_NCRE.SEDO_CODI As SERIE, TNHT_NCRE.NCRE_DOCU As NUMERO, TNHT_NCRE.NCRE_DOCU||'/'||TNHT_NCRE.SEDO_CODI DESCRIPCION, NCRE_VALO AS TOTAL, "
        SQL += " NCRE_TITU, NCRE_DAEM,NVL(ENTI_NCON_AF,0) CUENTA ,NVL(ENTI_NUCO,0) CIF,NVL(ENTI_NOME,'?') AS NOMBRE,NCRE_ANUL AS ANULADA"
        SQL += " ,SEFA_CODI AS SERIE2"
        SQL += ",FACT_CODI,FACT_CODI || '/' || FACT_SEFA AS FANULADA"
        SQL += " FROM TNHT_NCRE, TNHT_ENTI"
        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI "
        SQL += " AND TNHT_NCRE.NCRE_DAAN = " & "'" & Me.mFecha & "' "
        SQL += " ORDER BY TNHT_NCRE.NCRE_CODI"

        Me.DbLeeHotel.TraerLector(SQL)



        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalPendiente = 0
            Total = Decimal.Round(CType(Total, Decimal), 2)
            TotalPendiente = Decimal.Round(CType(TotalPendiente, Decimal), 2)


            Cuenta = CStr(Me.DbLeeHotel.mDbLector("CUENTA"))
            Reserva = GetReservasdeUnaNotaDeCredito(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE2")))

            If IsDBNull(Me.DbLeeHotel.mDbLector("FACT_CODI")) = False Then
                FacturaAnulada = Me.DbLeeHotel.mDbLector("FANULADA")
                Texto = "Documento " & CStr(Me.DbLeeHotel.mDbLector("DESCRIPCION")) & " Sobre Factura " & FacturaAnulada
            Else
                Texto = "Documento " & CStr(Me.DbLeeHotel.mDbLector("DESCRIPCION"))
            End If

            Me.mTipoAsiento = "DEBE"
            ' PDTE: HACER POR LINEAS NO POR TOTAL
            ' desbloquear
            ' Me.InsertaOracleMorasol("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaberFac, "Documento " & CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Total, "NO", "", "", "SI", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCliente, "", "DIM-DEP", Me.mParaMoraDimenHotel, Reserva, "", "", "", Me.mParaMoraTDocAbono, "WEBFACTURAS", "", "", "", "")
            Me.InsertaOracleMorasol("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebeFac, Texto, Total, "NO", Dni, Titular, "SI", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, Reserva, "", "", "", Me.mParaMoraTDocAbono, "WEBFACTURAS", "", "", "", "")




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
        Dim Descripcion As String
        Dim Reserva As String

        '  Dim Cuenta As String


        Dim Dni As String = ""


        Dim IvaNegocio As String = ""
        Dim IvaProducto As String = ""

        Dim Texto As String = ""
        Dim FacturaAnulada As String = ""

        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEDO_CODI AS SERIE , TNHT_NCRE.NCRE_DOCU AS NUMERO,(V.NCRE_VALO)TOTAL,((V.MOVI_VLIQ)) BASE,(V.NCRE_VIMP ) IGIC, "
        SQL += "TNHT_NCRE.NCRE_DOCU ||'/'|| TNHT_NCRE.SEDO_CODI DESCRIPCION, TNHT_NCRE.NCRE_DAEM,TIVA_PERC TIPO,NVL(TIVA_CTB1,'0') "
        SQL += " CUENTA ,NVL(ENTI_NCON_AF,0) CUENTACLIENTE ,NVL(ENTI_NUCO,0) CIF ,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X,NCRE_ANUL AS ANULADA "
        SQL += ",TIVA_CTB1 IVANEGOCIO,TIVA_CCVL IVAPRODUCTO "
        SQL += " ,TNHT_NCRE.SEFA_CODI AS SERIE2"
        SQL += ",FACT_CODI,FACT_CODI || '/' || FACT_SEFA AS FANULADA"
        SQL += " FROM TNHT_NCRE,TNHT_ENTI,TNHT_TIVA,QWE_CONT_NCIM V"
        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI AND "
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

            '    Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)
            Totalfactura = TotalBase + TotalIva

            Descripcion = CStr(Me.DbLeeHotel.mDbLector("DESCRIPCION"))

            Reserva = GetReservasdeUnaNotaDeCredito(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE2")))


            If IsDBNull(Me.DbLeeHotel.mDbLector("FACT_CODI")) = False Then
                FacturaAnulada = Me.DbLeeHotel.mDbLector("FANULADA")
                Texto = "Doc: " & CStr(Me.DbLeeHotel.mDbLector("DESCRIPCION")) & " Sobre Factura " & FacturaAnulada
            Else
                Texto = "Doc: " & CStr(Me.DbLeeHotel.mDbLector("DESCRIPCION"))
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("IVANEGOCIO")) = False Then
                IvaNegocio = Me.DbLeeHotel.mDbLector("IVANEGOCIO")
            Else
                IvaNegocio = ""
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("IVAPRODUCTO")) = False Then
                IvaProducto = Me.DbLeeHotel.mDbLector("IVAPRODUCTO")
            Else
                IvaProducto = ""
            End If

            If Totalfactura <> 0 Then
                Me.mTipoAsiento = "DEBE"
                '     Me.InsertaOracleMorasol("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "Documento " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva, "NO", "", "", "SI", "", "", "", "", "", "", "FIN", "WEBPDTE", "WEBFACTURAS")
                Me.InsertaOracleMorasol("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorDebe, Texto, Totalfactura, "NO", "", "", "SI", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCuenta, "", "", Me.mParaMoraDimenHotel, Reserva, "", "", "", Me.mParaMoraTDocAbono, "WEBFACTURAS", IvaNegocio, IvaProducto, Me.mParaIgicNegocio, Me.mParaIgicProducto)

            End If




        End While
        Me.DbLeeHotel.mDbLector.Close()

        '' ANULADAS
        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEDO_CODI AS SERIE , TNHT_NCRE.NCRE_DOCU AS NUMERO,(V.NCRE_VALO )TOTAL,((V.MOVI_VLIQ) ) BASE,(V.NCRE_VIMP ) IGIC, "
        SQL += " TNHT_NCRE.NCRE_DOCU ||'/'|| TNHT_NCRE.SEDO_CODI DESCRIPCION, TNHT_NCRE.NCRE_DAEM,TIVA_PERC TIPO,NVL(TIVA_CTB1,'0') "
        SQL += " CUENTA ,NVL(ENTI_NCON_AF,0) CUENTACLIENTE ,NVL(ENTI_NUCO,0) CIF ,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X,NCRE_ANUL AS ANULADA "
        SQL += ",TIVA_CTB1 IVANEGOCIO,TIVA_CCVL IVAPRODUCTO "
        SQL += " ,TNHT_NCRE.SEFA_CODI AS SERIE2"
        SQL += ",FACT_CODI,FACT_CODI || '/' || FACT_SEFA AS FANULADA"
        SQL += " FROM TNHT_NCRE,TNHT_ENTI,TNHT_TIVA,QWE_CONT_NCIM V"
        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI AND "
        SQL += " TNHT_NCRE.NCRE_CODI = V.NCRE_CODI    AND"
        SQL += " TNHT_NCRE.SEFA_CODI = V.SEFA_CODI"
        SQL += " AND V.TIVA = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_NCRE.NCRE_DAAN = " & "'" & Me.mFecha & "' "
        SQL += " ORDER BY TNHT_NCRE.NCRE_CODI ASC"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
            TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            '    Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)
            Totalfactura = TotalBase + TotalIva


            Descripcion = CStr(Me.DbLeeHotel.mDbLector("DESCRIPCION"))

            Reserva = GetReservasdeUnaNotaDeCredito(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE2")))


            If IsDBNull(Me.DbLeeHotel.mDbLector("FACT_CODI")) = False Then
                FacturaAnulada = Me.DbLeeHotel.mDbLector("FANULADA")
                Texto = "Doc: " & CStr(Me.DbLeeHotel.mDbLector("DESCRIPCION")) & " Sobre Factura " & FacturaAnulada
            Else
                Texto = "Doc: " & CStr(Me.DbLeeHotel.mDbLector("DESCRIPCION"))
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("IVANEGOCIO")) = False Then
                IvaNegocio = Me.DbLeeHotel.mDbLector("IVANEGOCIO")
            Else
                IvaNegocio = ""
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("IVAPRODUCTO")) = False Then
                IvaProducto = Me.DbLeeHotel.mDbLector("IVAPRODUCTO")
            Else
                IvaProducto = ""
            End If

            If Totalfactura <> 0 Then
                Me.mTipoAsiento = "HABER"
                '  Me.InsertaOracleMorasol("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, "Documento " & CStr(Me.DbLeeHotel.mDbLector("DESCRIPCION")), TotalIva, "NO", "", "", "SI", "", "", "", "", "", "", "FIN", "WEBPDTE", "WEBFACTURAS")
                Me.InsertaOracleMorasol("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, Texto, Totalfactura, "NO", "", "", "SI", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCuenta, "", "", Me.mParaMoraDimenHotel, Reserva, "", "", "", Me.mParaMoraTDocAbono, "WEBFACTURAS", IvaNegocio, IvaProducto, Me.mParaIgicNegocio, Me.mParaIgicProducto)

            End If

        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub

    Private Sub NotasDeCreditoEntidadCreditoBaseImponibleAgrupado()
        Dim TotalIva As Double
        Dim TotalBase As Double
        Dim Totalfactura As Double

        Dim Descripcion As String = ""

        Dim Cuenta As String = ""

        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEDO_CODI AS SERIE ,SUM((QWE_CONT_NCIM.NCRE_VALO )) TOTAL,SUM(QWE_CONT_NCIM.MOVI_VLIQ) BASE,SUM(QWE_CONT_NCIM.NCRE_VIMP) IGIC "
        SQL += " FROM  TNHT_NCRE,TNHT_ENTI,TNHT_TIVA,QWE_CONT_NCIM"

        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI AND "
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
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            Descripcion = "Total Serie " & CStr(Me.DbLeeHotel.mDbLector("SERIE"))


            Cuenta = mCtaSerieNotaCredito



            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracleMorasol("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, TotalBase, "NO", "", "", "SI", "", "", "", "", "", "", "FIN", "WEBPDTE", "WEBFACTURAS")
        End While
        Me.DbLeeHotel.mDbLector.Close()

        '' ANULADAS
        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEDO_CODI AS SERIE ,SUM((QWE_CONT_NCIM.NCRE_VALO * -1)) TOTAL,SUM(QWE_CONT_NCIM.MOVI_VLIQ *-1) BASE,SUM(QWE_CONT_NCIM.NCRE_VIMP) IGIC "
        SQL += " FROM  TNHT_NCRE,TNHT_ENTI,TNHT_TIVA,QWE_CONT_NCIM"

        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI AND "
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

            Cuenta = mCtaSerieNotaCredito



            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracleMorasol("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, TotalBase, "NO", "", "", "SI", "", "", "", "", "", "", "", "WEBPDTE", "WEBCONTRAPARTIDA")
        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub

    Private Sub NotasDeCreditoEntidadCreditoBaseImponible()

        Dim TotalBase As Double
        Dim Totalfactura As Double

        Dim Descripcion As String = ""

        '   Dim Cuenta As String = ""


        Dim Dni As String = ""
        Dim Cuenta As String = "0"
        Dim Titular As String = ""
        Dim Reserva As String = ""

        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEDO_CODI AS SERIE ,TNHT_NCRE.NCRE_CODI AS NUMERO ,SUM((QWE_CONT_NCIM.NCRE_VALO )) TOTAL,SUM(QWE_CONT_NCIM.MOVI_VLIQ) BASE,SUM(QWE_CONT_NCIM.NCRE_VIMP) IGIC "
        SQL += ",TNHT_NCRE.NCRE_CODI  || '/' || TNHT_NCRE.SEDO_CODI AS DESCRIPCION,NCRE_TITU "
        SQL += " FROM  TNHT_NCRE,TNHT_ENTI,TNHT_TIVA,QWE_CONT_NCIM"

        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI AND "
        SQL += " TNHT_NCRE.NCRE_CODI = QWE_CONT_NCIM.NCRE_CODI    AND"
        SQL += " TNHT_NCRE.SEFA_CODI = QWE_CONT_NCIM.SEFA_CODI"
        SQL += " AND QWE_CONT_NCIM.TIVA = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " GROUP BY TNHT_NCRE.SEDO_CODI,TNHT_NCRE.NCRE_CODI,NCRE_TITU"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            Descripcion = CStr(Me.DbLeeHotel.mDbLector("DESCRIPCION"))


            Cuenta = mCtaManoCorriente



            Me.mTipoAsiento = "DEBE"
            'Me.InsertaOracleMorasol("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, TotalBase, "NO", "", "", "SI", "", "", "", "", "", "", "FIN", "WEBPDTE", "WEBFACTURAS")
            Me.InsertaOracleMorasol("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, mCtaManoCorriente, Me.mIndicadorHaber, "Documento " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalBase, "NO", Dni, Titular, "SI", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, Reserva, "", "", "", Me.mParaMoraTDocAbono, "WEBFACTURAS", "", "", "", "")

        End While
        Me.DbLeeHotel.mDbLector.Close()

        '' ANULADAS
        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEDO_CODI AS SERIE ,TNHT_NCRE.NCRE_CODI AS NUMERO,SUM((QWE_CONT_NCIM.NCRE_VALO * -1)) TOTAL,SUM(QWE_CONT_NCIM.MOVI_VLIQ *-1) BASE,SUM(QWE_CONT_NCIM.NCRE_VIMP) IGIC "
        SQL += ",TNHT_NCRE.NCRE_CODI  || '/' || TNHT_NCRE.SEDO_CODI AS DESCRIPCION,NCRE_TITU "
        SQL += " FROM  TNHT_NCRE,TNHT_ENTI,TNHT_TIVA,QWE_CONT_NCIM"

        SQL += " WHERE TNHT_NCRE.ENTI_CODI = TNHT_ENTI.ENTI_CODI AND "
        SQL += " TNHT_NCRE.NCRE_CODI = QWE_CONT_NCIM.NCRE_CODI    AND"
        SQL += " TNHT_NCRE.SEFA_CODI = QWE_CONT_NCIM.SEFA_CODI"
        SQL += " AND QWE_CONT_NCIM.TIVA = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_NCRE.NCRE_DAAN = " & "'" & Me.mFecha & "' "
        SQL += " GROUP BY TNHT_NCRE.SEDO_CODI,TNHT_NCRE.NCRE_CODI,NCRE_TITU"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)
            Descripcion = CStr(Me.DbLeeHotel.mDbLector("DESCRIPCION"))

            Cuenta = mCtaManoCorriente



            Me.mTipoAsiento = "DEBE"
            ' Me.InsertaOracleMorasol("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Descripcion, TotalBase, "NO", "", "", "SI", "", "", "", "", "", "", "", "WEBPDTE", "WEBCONTRAPARTIDA")
            Me.InsertaOracleMorasol("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, mCtaManoCorriente, Me.mIndicadorHaber, "Documento " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalBase, "NO", Dni, Titular, "SI", CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, Reserva, "", "", "", Me.mParaMoraTDocAbono, "WEBFACTURAS", "", "", "", "")

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

        Dim Reserva As String



        SQL = "SELECT SUM(MOVI_VDEB) AS TOTAL ,TNHT_FACT.FACT_CODI AS NUMERO,TNHT_FACT.SEFA_CODI AS SERIE FROM " & Me.mStrHayHistorico & " TNHT_MOVI , TNHT_FACT,TNHT_FAMO "
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

        SQL = SQL & " ORDER BY TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI"


        Me.NEWHOTEL = New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)


        Me.DbLeeHotel.TraerLector(SQL)


        Total = 0
        Linea = 0
        While Me.DbLeeHotel.mDbLector.Read
            Total = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            Cuenta = Me.NEWHOTEL.DevuelveCuentaContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))

            Reserva = GetReservasdeUnaFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))


            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Documento " & CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), Total, "NO", "", "", "SI", "", CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, Reserva, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), "", "WEBCOBROS")
            End If

        End While
        Me.DbLeeHotel.mDbLector.Close()


        SQL = "SELECT SUM((MOVI_VDEB * -1)) AS TOTAL,TNHT_FACT.FACT_CODI AS NUMERO,TNHT_FACT.SEFA_CODI AS SERIE   FROM " & Me.mStrHayHistorico & " TNHT_MOVI , TNHT_FACT,TNHT_FAMO "
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

        SQL = SQL & " ORDER BY TNHT_FACT.SEFA_CODI,TNHT_FACT.FACT_CODI"



        Me.DbLeeHotel.TraerLector(SQL)



        While Me.DbLeeHotel.mDbLector.Read


            Total = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)
            Cuenta = Me.NEWHOTEL.DevuelveCuentaContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))

            Reserva = GetReservasdeUnaFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))


            If Total <> 0 Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                '                Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), Total, "NO", "", "", "SI", "", CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), Me.mParaMoraTMovCuenta, "", "", Me.mParaMoraDimenHotel, "", "", "", "WEBCOBROS")
                Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Documento " & CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), Total, "NO", "", "", "SI", "", CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, Reserva, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), "", "WEBCOBROS")




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
            Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaClientesContado, Me.mIndicadorHaber, "COBROS FACTURACION " & Me.mFecha, Total, "SI", "", "", "SI", "", Me.Multidiario, "", "", "", "", "", "", "", "FIN", "WEBPDTE", "WEBPDTE")

        End If

    End Sub


    Private Sub FacturasContadoTotalVisas()
        Dim Total As Double
        Dim Descripcion As String
        SQL = "SELECT MOVI_VDEB TOTAL,CACR_DESC TARJETA,nvl(CACR_CTBA,'0') CUENTA,"
        SQL += " TNHT_FACT.SEFA_CODI AS SERIE,TNHT_FACT.FACT_CODI AS NUMERO,NVL(TNHT_FACT.FACT_TITU,' ') AS TITULAR,NVL(FAAN_CODI,'0') AS FAAN_CODI,CACR_CTB3  "
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

            Descripcion = CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String) & " " & CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)
            Me.mTipoAsiento = "DEBE"



            'FASE 2 2016 COMPROBANTES BANCARIOS 
            ' si la visa tiene codigo de banco se hace la gestion de comprobantes si no se hace asiento tradicional 
            If IsDBNull(Me.DbLeeHotel.mDbLector("CACR_CTB3")) = False Then
                Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion, Total, "NO", "", "", "SI", "", CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), Me.mParaMoraTMovBanco, "", "", Me.mParaMoraDimenHotel, "", "", "", "WEBCOBROS")
            Else
                'OLD
                Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion, Total, "NO", "", "", "SI", "", CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), Me.mParaMoraTMovBanco, "", "", Me.mParaMoraDimenHotel, "", "", "", "WEBCOBROS")

            End If


        End While
        Me.DbLeeHotel.mDbLector.Close()


    End Sub
    Private Sub FacturasContadoTotaLOtrasFormas()
        Dim Total As Double
        Dim SQL As String

        Dim Descripcion As String


        SQL = ""
        SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,TNHT_FACT.SEFA_CODI AS SERIE,TNHT_FACT.FACT_CODI AS NUMERO,NVL(TNHT_FACT.FACT_TITU,' ') AS TITULAR,NVL(FAAN_CODI,'0') AS FAAN_CODI  FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FORE,TNHT_FACT,TNHT_FAMO WHERE"

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

            Descripcion = CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String) & " " & CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)


            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, Descripcion, Total, "NO", "", "", "SI", "", CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String), Me.mParaMoraTMovBanco, "", "", Me.mParaMoraDimenHotel, "", "", "", "WEBCOBROS")

        End While
        Me.DbLeeHotel.mDbLector.Close()


    End Sub

    Private Sub FacturasCancelaciondeAnticiposHTito(vFactura As Integer, vSerie As String, vSeccion As String, vFaanCodi As String)
        Dim Total As Double
        '    Dim TotalCancelados As Double

        Dim SQL As String
        Dim Cuenta As String
        Dim TipoDoc As String
        Dim Recibo As String
        Dim Reserva As String
        Dim Texto As String


        Dim GetNewHotel As New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)

        ' ANTICIPOS EN FACTURA
        SQL = "SELECT       "
        SQL += "         SUM(TNHT_MOVI.MOVI_VDEB)  TOTAL, TNHT_FACT.FACT_CODI AS NUMERO, "
        SQL += "         TNHT_FACT.SEFA_CODI SERIE "
        SQL += "  ,  TNHT_FACT.FACT_CODI  || '/' || TNHT_FACT.SEFA_CODI AS FACTURA"
        SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) RECI_COBR "
        SQL += ",TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA"
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
        SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20)"
        SQL += ",TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI"


        Me.DbLeeHotelAux.TraerLector(SQL)

        While Me.DbLeeHotelAux.mDbLector.Read
            Linea = Linea + 1
            If vFaanCodi.Length = 0 Then
                Total = CType(Me.DbLeeHotelAux.mDbLector("TOTAL"), Double)
            Else
                Total = (CType(Me.DbLeeHotelAux.mDbLector("TOTAL"), Double) * -1)
            End If


            If IsDBNull(Me.DbLeeHotelAux.mDbLector("RECI_COBR")) Then
                Recibo = ""
            Else
                Recibo = CStr(Me.DbLeeHotelAux.mDbLector("RECI_COBR")).Trim
            End If

            If IsDBNull(Me.DbLeeHotelAux.mDbLector("RESERVA")) Then
                Reserva = ""
            Else
                Reserva = CStr(Me.DbLeeHotelAux.mDbLector("RESERVA")).Trim
            End If

            If Total <> 0 Then

                If Me.mParaDistingeAnticiposDeDepositos = True And vSeccion = Me.mParaSecc_DepNh Then
                    Cuenta = Me.mParaCta4b & mParaSufijoDepositos
                    '   TipoDoc = Me.mParaMoraTDocDeposito
                    TipoDoc = ""
                    Texto = "Depósito Facturado "
                Else
                    Cuenta = Me.mCtaPagosACuenta & mParaSufijoAnticipos
                    '   TipoDoc = Me.mParaMoraTDocAnticipo
                    TipoDoc = ""
                    Texto = "Anticipo Facturado "
                End If

                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Texto & CType(Me.DbLeeHotelAux.mDbLector("FACTURA"), String), Total, "NO", "", "", "SI", "", CStr(Me.DbLeeHotelAux.mDbLector("NUMERO")), CStr(Me.DbLeeHotelAux.mDbLector("SERIE")), CStr(Me.DbLeeHotelAux.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotelAux.mDbLector("SERIE")), Me.mParaMoraTMovCuenta, "", "", Me.mParaMoraDimenHotel, Reserva, Recibo, TipoDoc, "WEBANTICIPOS FACTURADOS")



                Linea = Linea + 1

                Cuenta = GetNewHotel.DevuelveCuentaContabledeFactura(CInt(Me.DbLeeHotelAux.mDbLector("NUMERO")), CStr(Me.DbLeeHotelAux.mDbLector("SERIE")))

                Me.mTipoAsiento = "HABER"
                Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Texto & CType(Me.DbLeeHotelAux.mDbLector("FACTURA"), String), Total, "NO", "", "ANTICIPO", "SI", "", CStr(Me.DbLeeHotelAux.mDbLector("NUMERO")), CStr(Me.DbLeeHotelAux.mDbLector("SERIE")), CStr(Me.DbLeeHotelAux.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotelAux.mDbLector("SERIE")), Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, Reserva, Recibo, TipoDoc, "WEBANTICIPOS FACTURADOS")

            End If



        End While
        Me.DbLeeHotelAux.mDbLector.Close()

        ' DEVOLUCIONES CON RECIBO EN FACTURA TIPO 5
        SQL = "SELECT     "
        SQL += "         SUM(TNHT_MOVI.MOVI_VDEB)  TOTAL, TNHT_FACT.FACT_CODI AS NUMERO, "
        SQL += "         TNHT_FACT.SEFA_CODI SERIE "
        SQL += "  ,  TNHT_FACT.FACT_CODI  || '/' || TNHT_FACT.SEFA_CODI AS FACTURA"
        SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) RECI_COBR "
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
        SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20)"


        Me.DbLeeHotelAux.TraerLector(SQL)

        While Me.DbLeeHotelAux.mDbLector.Read
            Linea = Linea + 1
            If vFaanCodi.Length = 0 Then
                Total = CType(Me.DbLeeHotelAux.mDbLector("TOTAL"), Double)
            Else
                Total = (CType(Me.DbLeeHotelAux.mDbLector("TOTAL"), Double) * -1)
            End If

            If IsDBNull(Me.DbLeeHotelAux.mDbLector("RECI_COBR")) Then
                Recibo = ""
            Else
                Recibo = CStr(Me.DbLeeHotelAux.mDbLector("RECI_COBR")).Trim
            End If

            If Total <> 0 Then


                If Me.mParaDistingeAnticiposDeDepositos = True And vSeccion = Me.mParaSecc_DepNh Then
                    Cuenta = Me.mParaCta4b & mParaSufijoDepositos
                    '   TipoDoc = Me.mParaMoraTDocDeposito
                    TipoDoc = ""
                Else
                    Cuenta = Me.mCtaPagosACuenta & mParaSufijoAnticipos
                    '   TipoDoc = Me.mParaMoraTDocAnticipo
                    TipoDoc = ""
                End If

                ' INSERTA ORACLE BLOQUEADO TIENEN ANSIENTO PROPIO !!
                Me.mTipoAsiento = "DEBE"
                '         Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, "Devolución de Anticipo con Recibo  " & CType(Me.DbLeeHotelAux.mDbLector("FACTURA"), String), Total, "NO", "", "", "SI", "", CStr(Me.DbLeeHotelAux.mDbLector("NUMERO")), CStr(Me.DbLeeHotelAux.mDbLector("SERIE")), CStr(Me.DbLeeHotelAux.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotelAux.mDbLector("SERIE")), Me.mParaMoraTMovCuenta, "", "", Me.mParaMoraDimenHotel, "", Recibo, TipoDoc, "WEBANTICIPOS FACTURADOS")



                Linea = Linea + 1

                Cuenta = GetNewHotel.DevuelveCuentaContabledeFactura(CInt(Me.DbLeeHotelAux.mDbLector("NUMERO")), CStr(Me.DbLeeHotelAux.mDbLector("SERIE")))

                Me.mTipoAsiento = "HABER"
                '       Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Devolución de Anticipo con Recibo  " & CType(Me.DbLeeHotelAux.mDbLector("FACTURA"), String), Total, "NO", "", "ANTICIPO", "SI", "", CStr(Me.DbLeeHotelAux.mDbLector("NUMERO")), CStr(Me.DbLeeHotelAux.mDbLector("SERIE")), CStr(Me.DbLeeHotelAux.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotelAux.mDbLector("SERIE")), Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, "", Recibo, TipoDoc, "WEBANTICIPOS FACTURADOS")

            End If



        End While
        Me.DbLeeHotelAux.mDbLector.Close()

        ' DEVOLUCUONES AL FACTURAR COBROS NEGATICOS 
        SQL = "SELECT      "
        SQL += "         SUM(TNHT_MOVI.MOVI_VDEB)  TOTAL, TNHT_FACT.FACT_CODI AS NUMERO, "
        SQL += "         TNHT_FACT.SEFA_CODI SERIE "
        SQL += "  ,  TNHT_FACT.FACT_CODI  || '/' || TNHT_FACT.SEFA_CODI AS FACTURA"
        SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) RECI_COBR "
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
        SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20)"


        Me.DbLeeHotelAux.TraerLector(SQL)

        While Me.DbLeeHotelAux.mDbLector.Read
            Linea = Linea + 1
            If vFaanCodi.Length = 0 Then
                Total = CType(Me.DbLeeHotelAux.mDbLector("TOTAL"), Double)
            Else
                Total = (CType(Me.DbLeeHotelAux.mDbLector("TOTAL"), Double) * -1)
            End If

            If IsDBNull(Me.DbLeeHotelAux.mDbLector("RECI_COBR")) Then
                Recibo = ""
            Else
                Recibo = CStr(Me.DbLeeHotelAux.mDbLector("RECI_COBR")).Trim
            End If


            If Total <> 0 Then


                If Me.mParaDistingeAnticiposDeDepositos = True And vSeccion = Me.mParaSecc_DepNh Then
                    Cuenta = Me.mParaCta4b & mParaSufijoDepositos
                    '  TipoDoc = Me.mParaMoraTDocDeposito
                    TipoDoc = ""
                Else
                    Cuenta = Me.mCtaPagosACuenta & mParaSufijoAnticipos
                    '   TipoDoc = Me.mParaMoraTDocAnticipo
                    TipoDoc = ""
                End If


                ' INSERTA ORACLE BLOQUEADO TIENEN ANSIENTO PROPIO !!
                Me.mTipoAsiento = "DEBE"
                '          Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, "Devolución de Anticipo en Factura " & CType(Me.DbLeeHotelAux.mDbLector("FACTURA"), String), Total, "NO", "", "", "SI", "", CStr(Me.DbLeeHotelAux.mDbLector("NUMERO")), CStr(Me.DbLeeHotelAux.mDbLector("SERIE")), CStr(Me.DbLeeHotelAux.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotelAux.mDbLector("SERIE")), Me.mParaMoraTMovCuenta, "", "", Me.mParaMoraDimenHotel, "", Recibo, TipoDoc, "WEBANTICIPOS FACTURADOS")



                Linea = Linea + 1

                Cuenta = GetNewHotel.DevuelveCuentaContabledeFactura(CInt(Me.DbLeeHotelAux.mDbLector("NUMERO")), CStr(Me.DbLeeHotelAux.mDbLector("SERIE")))

                Me.mTipoAsiento = "HABER"
                '          Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Devolución de Anticipo en Factura " & CType(Me.DbLeeHotelAux.mDbLector("FACTURA"), String), Total, "NO", "", "ANTICIPO", "SI", "", CStr(Me.DbLeeHotelAux.mDbLector("NUMERO")), CStr(Me.DbLeeHotelAux.mDbLector("SERIE")), CStr(Me.DbLeeHotelAux.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotelAux.mDbLector("SERIE")), Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, "", Recibo, TipoDoc, "WEBANTICIPOS FACTURADOS")

            End If



        End While
        Me.DbLeeHotelAux.mDbLector.Close()




        GetNewHotel.CerrarConexiones()

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

            Linea = Linea + 1
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", "Recibido " & CType(Me.DbLeeHotel.mDbLector("MOVI_DAVA"), String) & " Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI", "", Me.Multidiario, "")
            Me.GeneraFileACMultiDiario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, Me.Multidiario)




        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub FacturasCancelaciondeAnticipos()
        Dim Total As Double
        '    Dim TotalCancelados As Double

        Dim SQL As String
        Dim Cuenta As String
        Dim TipoDoc As String
        Dim Recibo As String


        Try






            ' ANTICIPOS EN FACTURA
            SQL = "SELECT "
            SQL += "        TNHT_MOVI.MOVI_VDEB  TOTAL, TNHT_FACT.FACT_CODI AS NUMERO, "
            SQL += "         TNHT_FACT.SEFA_CODI SERIE,TNHT_FACT.FAAN_CODI ,SECC_CODI "
            SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) RECI_COBR "
            SQL += ",  TNHT_FACT.FACT_CODI || '/' ||  TNHT_FACT.SEFA_CODI AS FACTURA  "
            SQL += "    FROM " & Me.mStrHayHistorico & " TNHT_MOVI, TNHT_FACT, TNHT_RESE, TNHT_FAMO "
            SQL += "   WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "     And TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "     And TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += "     And TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += "     And TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += "     And TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
            SQL += "     And TNHT_MOVI.MOVI_TIMO = '2' "
            SQL += "     AND TNHT_MOVI.TIRE_CODI = '1' "
            SQL += "     AND TNHT_MOVI.MOVI_VDEB <> 0 "
            SQL += "     AND TNHT_MOVI.MOVI_AUTO = '0' "
            SQL += "  AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"





            Me.DbLeeHotel.TraerLector(SQL)

            Dim GetNewHotel As New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                    Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Else
                    Total = (CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1)
                End If

                If Total <> 0 Then


                    If Me.mParaDistingeAnticiposDeDepositos = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                        Cuenta = Me.mParaCta4b & Me.mParaSufijoDepositos
                        ' TipoDoc = Me.mParaMoraTDocDeposito
                        TipoDoc = ""
                    Else
                        Cuenta = Me.mCtaPagosACuenta & Me.mParaSufijoAnticipos
                        '  TipoDoc = Me.mParaMoraTDocAnticipo
                        TipoDoc = ""
                    End If


                    If IsDBNull(Me.DbLeeHotel.mDbLector("RECI_COBR")) Then
                        Recibo = ""
                    Else
                        Recibo = CStr(Me.DbLeeHotel.mDbLector("RECI_COBR")).Trim
                    End If


                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, "Anticipo Facturardo Recibo " & Recibo & " " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, "NO", "", "", "SI", "", CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, "", Recibo, TipoDoc, "WEBANTICIPOS FACTURADOS")

                    Linea = Linea + 1

                    Cuenta = GetNewHotel.DevuelveCuentaContabledeFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))

                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Anticipo Facturardo Recibo " & Recibo & " " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, "NO", "", "ANTICIPO", "SI", "", CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, "", Recibo, "", "WEBANTICIPOS FACTURADOS")

                End If



            End While
            Me.DbLeeHotel.mDbLector.Close()
            GetNewHotel.CerrarConexiones()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub SaldoAnticiposFacturas()
        Try
            '    Dim Total As Double
            Dim Cuenta As String

            Dim Saldo As Double
            '    Dim DocLiquidar As String


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
                    Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaPagosACuenta, Me.mIndicadorDebe, "Saldo de Anticipos al Facturar " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Saldo, "NO", "", "Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI", "ANTICIPO FACTURADO", Me.Multidiario, "", "", "", "", "", "", "", "FIN", "WEBPDTE", "WEBANTICIPOS FACTURADOS")


                    Linea = Linea + 1

                    Cuenta = GetNewHotel.DevuelveCuentaContabledeFactura(CInt(Me.DbLeeHotel.mDbLector("FACT_CODI")), CStr(Me.DbLeeHotel.mDbLector("SEFA_CODI")))

                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Saldo de Anticipos al Facturar " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Saldo, "NO", "", "Fac: " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), "SI", "ANTICIPO FACTURADO", Me.Multidiario, "", "", "", "", "", "", "", "FIN", "WEBPDTE", "WEBANTICIPOS FACTURADOS")
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

            Dim TipoDoc As String
            Dim DocLiquidar As String


            SQL = "SELECT FACT_CODI || '/' || SEFA_CODI AS FACTURA ,"
            SQL += " FACT_CODI AS NUMERO,SEFA_CODI AS SERIE,FAAN_CODI,ENTI_CODI  FROM TNHT_FACT WHERE  "
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
                SQL = SQL & "               AND TNHT_FACT.FACT_CODI =  " & CInt(Me.DbLeeHotel.mDbLector("NUMERO"))
                SQL = SQL & "               AND TNHT_FACT.SEFA_CODI = '" & CStr(Me.DbLeeHotel.mDbLector("SERIE")) & "'"

                SQL = SQL & "          GROUP BY TNHT_MOVI.SECC_CODI) "





                Me.DbLeeHotelAux.TraerLector(SQL)
                While Me.DbLeeHotelAux.mDbLector.Read

                    Saldo = SaldoAnticiposalFacturarSeccion(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CStr(Me.DbLeeHotelAux.mDbLector("SECC_CODI")))

                    Linea = Linea + 1

                    If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                        Saldo = Saldo
                    Else
                        Saldo = Saldo * -1
                    End If




                    If Saldo <> 0 Then
                        Me.mCancelacionAnticipos = Me.mCancelacionAnticipos + Saldo

                        If Me.mParaDistingeAnticiposDeDepositos = True And CType(Me.DbLeeHotelAux.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                            Cuenta = Me.mParaCta4b & mParaSufijoDepositos
                            '    TipoDoc = Me.mParaMoraTDocDeposito
                            TipoDoc = ""
                        Else
                            Cuenta = Me.mCtaPagosACuenta & mParaSufijoAnticipos
                            '   TipoDoc = Me.mParaMoraTDocAnticipo
                            TipoDoc = ""
                        End If

                        DocLiquidar = Me.LocaLizareciboMayorAnticipo(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))

                        Me.mTipoAsiento = "DEBE"
                        Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, "Saldo de " & GetNewHotel.DevuelveNombreSeccion(CStr(Me.DbLeeHotelAux.mDbLector("SECC_CODI"))) & " al Facturar " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Saldo, "NO", "", "", "SI", "", CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, "", DocLiquidar, TipoDoc, "WEBANTICIPOS FACTURADOS")



                        Linea = Linea + 1

                        ' cuenta de contrapatrida (El documento a Liquidar es la factura)
                        Cuenta = GetNewHotel.DevuelveCuentaContabledeFactura(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))
                        DocLiquidar = Me.DbLeeHotel.mDbLector("FACTURA")
                        Me.mTipoAsiento = "HABER"
                        Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Saldo de " & GetNewHotel.DevuelveNombreSeccion(CStr(Me.DbLeeHotelAux.mDbLector("SECC_CODI"))) & " al Facturar " & CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Saldo, "NO", "", "ANTICIPO", "SI", "", CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, "", DocLiquidar, TipoDoc, "WEBANTICIPOS FACTURADOS")

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

    Private Sub SaldoAnticiposFacturasPorSeccionHTito()
        Try
            '    Dim Total As Double

            Dim FaanCodi As String


            SQL = "          SELECT  TNHT_FACT.FACT_CODI AS NUMERO,  TNHT_FACT.SEFA_CODI AS SERIE,TNHT_FACT.FAAN_CODI,TNHT_MOVI.SECC_CODI "
            SQL += "              FROM TNHT_MOVI, TNHT_FACT, TNHT_FAMO "
            SQL += "             WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "               AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "               AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += "               AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += "               AND TNHT_MOVI.MOVI_TIMO = '2' "
            SQL += "               AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
            SQL += "          GROUP BY  TNHT_FACT.FACT_CODI,  TNHT_FACT.SEFA_CODI,TNHT_FACT.FAAN_CODI,TNHT_MOVI.SECC_CODI "


            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) Then
                    FaanCodi = ""
                Else
                    FaanCodi = CStr(Me.DbLeeHotel.mDbLector("FAAN_CODI"))
                End If

                Me.FacturasCancelaciondeAnticiposHTito(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CStr(Me.DbLeeHotel.mDbLector("SECC_CODI")), FaanCodi)





            End While
            Me.DbLeeHotel.mDbLector.Close()




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
            SQL = "Select       "
            SQL += "         SUM(TNHT_MOVI.MOVI_VDEB)  TOTAL, TNHT_FACT.FACT_CODI As NUMERO, "
            SQL += "         TNHT_FACT.SEFA_CODI SERIE "
            SQL += "    FROM " & Me.mStrHayHistorico & " TNHT_MOVI, TNHT_FACT, TNHT_RESE, TNHT_FAMO "
            SQL += "   WHERE TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "     And TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "     And TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += "     And TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += "     And TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += "     And TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
            SQL += "     And TNHT_MOVI.MOVI_TIMO = '2' "
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
            Return 0
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
            Return 0

        End Try

    End Function

    Private Sub FacturasContadoTotalVisasComision()
        Try
            Dim Total As Double
            Dim TotalComision As Double
            Dim vCentroCosto As String
            Dim Recibo As String

            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA,NVL(TNHT_CACR.CACR_CTB2,'0') CUENTAGASTO,TNHT_CACR.CACR_COMI,NVL(TNHT_CACR.CACR_CTB3,'?') BANCOS_COD"
            SQL += " ,NVL(FAAN_CODI,'0') AS FAAN_CODI  "
            SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) RECI_COBR "
            SQL += " ,TNHT_FACT.FACT_CODI AS NUMERO , TNHT_FACT.SEFA_CODI AS SERIE"
            SQL += " ,NVL(CACR_CTB3,'0') AS NATURALEZA  "

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

            '
            SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CTBA,TNHT_CACR.CACR_CTB2,TNHT_CACR.CACR_COMI,FAAN_CODI,TNHT_CACR.CACR_CTB3"
            SQL += ",TNHT_FACT.FACT_CODI , TNHT_FACT.SEFA_CODI"
            SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20),TNHT_CACR.CACR_CTB3"



            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read



                If IsDBNull(Me.DbLeeHotel.mDbLector("RECI_COBR")) Then
                    Recibo = ""
                Else
                    Recibo = CStr(Me.DbLeeHotel.mDbLector("RECI_COBR")).Trim
                End If


                SQL = "SELECT NVL(PARA_CENTRO_COSTO_COMI,'0') FROM TH_PARA "
                SQL += " WHERE  PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                vCentroCosto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)


                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

                If Total <> 0 Then


                    If CType(Me.DbLeeHotel.mDbLector("FAAN_CODI"), Integer) = 0 Then
                        Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                    Else
                        Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
                    End If


                    TotalComision = (Total * CType(Me.DbLeeHotel.mDbLector("CACR_COMI"), Double)) / 100

                    If TotalComision <> 0 Then
                        Linea = Linea + 1
                        Me.mTipoAsiento = "HABER"
                        Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, "COMISION FACTURAS " & CType(Me.DbLeeHotel.mDbLector("CACR_COMI"), String) & " %  " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", "", "SI", "", CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovBanco, "", "", Me.mParaMoraDimenHotel, "", Recibo, "", "WEBCOBROS")



                        Linea = Linea + 1
                        Me.mTipoAsiento = "DEBE"
                        Me.InsertaOracleMorasol("AC", 35, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTAGASTO"), String), Me.mIndicadorDebe, "COMISION FACTURAS " & CType(Me.DbLeeHotel.mDbLector("CACR_COMI"), String) & " %  " & CType(Me.DbLeeHotel.mDbLector("TARJETA"), String), TotalComision, "NO", "", "", "SI", "", CStr(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")), CStr(Me.DbLeeHotel.mDbLector("NUMERO")) & "/" & CStr(Me.DbLeeHotel.mDbLector("SERIE")), Me.mParaMoraTMovCuenta, CStr(Me.DbLeeHotel.mDbLector("NATURALEZA")), "", Me.mParaMoraDimenHotel, "", Recibo, "", "WEBCOBROS")

                    End If
                End If



            End While
            Me.DbLeeHotel.mDbLector.Close()






        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
        End Try

    End Sub
#End Region
#Region "ASIENTO-21 DEVOLUCIONES "
    Private Sub TotalDevolucionesVisas()
        Try
            Dim Total As Double
            Dim Cuenta As String

            Dim TipoDoc As String
            Dim Reserva As String
            Dim Recibo As String

            ' SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA,NVL(SECC_CODI,'?') AS SECC_CODI"
            SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA,NVL(SECC_CODI,'?') AS SECC_CODI"

            SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) RECI_COBR "
            SQL += ",TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA"
            SQL += " ,NVL(MOVI_DESC,' ') MOVI_DESC "

            ' 20180118
            SQL += " ,NVL(MOVI_CODR,0) AS MOVI_CODR,MOVI_DARE "

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
            '   SQL = SQL & " GROUP BY TNHT_MOVI.CACR_CODI,TNHT_CACR.CACR_DESC,TNHT_CACR.CACR_CTBA,SECC_CODI"
            '   SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) ,TNHT_MOVI.RESE_CODI , TNHT_MOVI.RESE_ANCI,MOVI_DESC"



            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read

                If Me.mParaDistingeAnticiposDeDepositos = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                    Cuenta = Me.mParaCta4b3Visa
                    Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
                    '   TipoDoc = Me.mParaMoraTDocDeposito
                    TipoDoc = ""
                Else
                    Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
                    '   TipoDoc = Me.mParaMoraTDocAnticipo
                    TipoDoc = ""
                End If

                If IsDBNull(Me.DbLeeHotel.mDbLector("RECI_COBR")) Then
                    ' si esta nulo es que es la contrapartida de un recibo anulado se llama a funcion que busca el numero de recibo anulado
                    Recibo = LocaLizaReciboAnulado(CInt(Me.DbLeeHotel.mDbLector("MOVI_CODR")), CDate(Me.DbLeeHotel.mDbLector("MOVI_DARE")))
                    Me.mAuxStr = ""
                Else
                    Recibo = CStr(Me.DbLeeHotel.mDbLector("RECI_COBR")).Trim
                    Me.mAuxStr = ""
                End If


                If IsDBNull(Me.DbLeeHotel.mDbLector("RESERVA")) Then
                    Reserva = ""
                Else
                    Reserva = CStr(Me.DbLeeHotel.mDbLector("RESERVA"))
                End If

                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracleMorasol("AC", 21, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", Me.mAuxStr, "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Me.mVisaCfbcotmov, "", Me.mVisaComprobante, Recibo, Me.mParaMoraTMovBanco, "", "", Me.mParaMoraDimenHotel, Reserva, "", "", "WEBCOBROS")




            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
        End Try

    End Sub
    Private Sub TotalDevolucionesOtrasFormas()
        Dim Total As Double
        Dim Cuenta As String
        Dim TipoDoc As String

        Dim Reserva As String
        Dim Recibo As String



        '   SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,NVL(SECC_CODI,'?') AS SECC_CODI "
        SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,NVL(SECC_CODI,'?') AS SECC_CODI "

        SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) RECI_COBR "

        SQL += ",TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA"

        SQL += " ,NVL(MOVI_DESC,' ') MOVI_DESC"

        ' 20180118
        SQL += " ,NVL(MOVI_CODR,0) AS MOVI_CODR,MOVI_DARE "

        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"

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

        '  SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1,SECC_CODI"
        '   SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) ,TNHT_MOVI.RESE_CODI , TNHT_MOVI.RESE_ANCI,MOVI_DESC"

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            If Me.mParaDistingeAnticiposDeDepositos = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b2Efectivo
                Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
                'TipoDoc = Me.mParaMoraTDocDeposito
                TipoDoc = ""
            Else
                Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)
                ' TipoDoc = Me.mParaMoraTDocAnticipo
                TipoDoc = ""
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RECI_COBR")) Then
                ' si esta nulo es que es la contrapartida de un recibo anulado se llama a funcion que busca el numero de recibo anulado
                Recibo = LocaLizaReciboAnulado(CInt(Me.DbLeeHotel.mDbLector("MOVI_CODR")), CDate(Me.DbLeeHotel.mDbLector("MOVI_DARE")))
                Me.mAuxStr = ""
            Else
                Recibo = CStr(Me.DbLeeHotel.mDbLector("RECI_COBR")).Trim
                Me.mAuxStr = ""
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("RESERVA")) Then
                Reserva = ""
            Else
                Reserva = CStr(Me.DbLeeHotel.mDbLector("RESERVA"))
            End If





            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracleMorasol("AC", 21, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", Me.mAuxStr, "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Recibo, Me.mParaMoraTMovBanco, "", "", Me.mParaMoraDimenHotel, Reserva, "", "", "WEBCOBROS")



        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetalleDevolucionesVisas()
        Dim Total As Double
        Dim Cuenta As String = ""
        Dim TipoDoc As String

        Dim Reserva As String
        Dim Recibo As String

        Dim DocLiquidar As String

        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
        SQL += " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA,NVL(MOVI_DESC,' ') MOVI_DESC,NVL(SECC_CODI,'?') AS SECC_CODI "
        SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) RECI_COBR "
        SQL += ",TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA"


        ' 20180118
        SQL += ",TNHT_MOVI.RESE_CODI, TNHT_MOVI.RESE_ANCI "

        ' 20180118
        SQL += " ,NVL(MOVI_CODR,0) AS MOVI_CODR,MOVI_DARE "

        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,"
        SQL += " TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL += " And TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL += " And TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
        'SQL = SQL & " And TNHT_MOVI.TIRE_CODI = 5"
        SQL += " And TNHT_MOVI.TIRE_CODI IN(4,5) "

        SQL += " And TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL += " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL += " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            If Me.mParaDistingeAnticiposDeDepositos = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b & Me.mParaSufijoDepositos
                '    TipoDoc = Me.mParaMoraTDocDeposito
                TipoDoc = ""
            Else
                Cuenta = Me.mCtaPagosACuenta & Me.mParaSufijoAnticipos
                '   TipoDoc = Me.mParaMoraTDocAnticipo
                TipoDoc = ""
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RECI_COBR")) Then
                ' si esta nulo es que es la contrapartida de un recibo anulado se llama a funcion que busca el numero de recibo anulado
                Recibo = LocaLizaReciboAnulado(CInt(Me.DbLeeHotel.mDbLector("MOVI_CODR")), CDate(Me.DbLeeHotel.mDbLector("MOVI_DARE")))
                Me.mAuxStr = ""
            Else
                Recibo = CStr(Me.DbLeeHotel.mDbLector("RECI_COBR")).Trim
                Me.mAuxStr = ""
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("RESERVA")) Then
                Reserva = ""
            Else
                Reserva = CStr(Me.DbLeeHotel.mDbLector("RESERVA"))
            End If

            DocLiquidar = Me.LocaLizareciboDeRecepcionDeDeposito(CInt(Me.DbLeeHotel.mDbLector("RESE_CODI")), CInt(Me.DbLeeHotel.mDbLector("RESE_ANCI")), CStr(Me.DbLeeHotel.mDbLector("SECC_CODI")), CDbl(Me.DbLeeHotel.mDbLector("TOTAL")) * -1, Me.mFecha, Recibo)

            ' 20180118
            ' SI NO SE LOCALIZA EL RECIBO DE RECEPCION , ES PORQUE LA DEVOLUCION ESTA ANULADA 

            If DocLiquidar = "NO LOCALIZADO" Then
                DocLiquidar = Recibo
                Me.mAuxStr = "Anulado"
            Else
                Me.mAuxStr = ""
            End If


            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracleMorasol("AC", 21, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "" & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", Me.mAuxStr, "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Recibo, Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, Reserva, DocLiquidar, TipoDoc, "WEBCOBROS")



        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetalleDevolucionesOtrasFormas()
        Dim Total As Double
        Dim Cuenta As String = ""
        Dim TipoDoc As String

        Dim Reserva As String
        Dim Recibo As String

        Dim DocLiquidar As String

        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE,TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_DAVA,NVL(MOVI_DESC,' ') MOVI_DESC,NVL(SECC_CODI,'?') AS SECC_CODI "
        SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) RECI_COBR "
        SQL += ",TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA"
        SQL += " ,NVL(MOVI_DESC,' ') MOVI_DESC"

        ' 20180118
        SQL += ",TNHT_MOVI.RESE_CODI, TNHT_MOVI.RESE_ANCI "

        ' 20180118
        SQL += " ,NVL(MOVI_CODR,0) AS MOVI_CODR,MOVI_DARE "

        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
        SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL = SQL & " And TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL = SQL & " And TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
        'SQL = SQL & " And TNHT_MOVI.TIRE_CODI = 5"
        SQL = SQL & " And TNHT_MOVI.TIRE_CODI IN(4,5) "


        SQL = SQL & " And TNHT_MOVI.CACR_CODI Is NULL"
        SQL = SQL & " And TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB <> 0"

        ' excluir depositos anticipados 
        'SQL = SQL & " AND TNHT_MOVI.MOVI_DEAN ='0'"

        If Me.mUsaTnhtMoviAuto = True Then
            SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
        End If

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            If Me.mParaDistingeAnticiposDeDepositos = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b & Me.mParaSufijoDepositos
                '    TipoDoc = Me.mParaMoraTDocDeposito
                TipoDoc = ""
            Else
                Cuenta = Me.mCtaPagosACuenta & Me.mParaSufijoAnticipos
                '     TipoDoc = Me.mParaMoraTDocAnticipo
                TipoDoc = ""
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RECI_COBR")) Then
                ' si esta nulo es que es la contrapartida de un recibo anulado se llama a funcion que busca el numero de recibo anulado
                Recibo = LocaLizaReciboAnulado(CInt(Me.DbLeeHotel.mDbLector("MOVI_CODR")), CDate(Me.DbLeeHotel.mDbLector("MOVI_DARE")))
                Me.mAuxStr = ""
            Else
                Recibo = CStr(Me.DbLeeHotel.mDbLector("RECI_COBR")).Trim
                Me.mAuxStr = ""
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("RESERVA")) Then
                Reserva = ""
            Else
                Reserva = CStr(Me.DbLeeHotel.mDbLector("RESERVA"))
            End If

            DocLiquidar = Me.LocaLizareciboDeRecepcionDeDeposito(CInt(Me.DbLeeHotel.mDbLector("RESE_CODI")), CInt(Me.DbLeeHotel.mDbLector("RESE_ANCI")), CStr(Me.DbLeeHotel.mDbLector("SECC_CODI")), CDbl(Me.DbLeeHotel.mDbLector("TOTAL")) * -1, Me.mFecha, Recibo)
            ' 20180118
            ' SI NO SE LOCALIZA EL RECIBO DE RECEPCION , ES PORQUE LA DEVOLUCION ESTA ANULADA 

            If DocLiquidar = "NO LOCALIZADO" Then
                DocLiquidar = Recibo
                Me.mAuxStr = "Anulado"
            Else
                Me.mAuxStr = ""
            End If

            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracleMorasol("AC", 21, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "" & CType(Me.DbLeeHotel.mDbLector("MOVI_DESC"), String), Total, "NO", "", Me.mAuxStr, "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Recibo, Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, Reserva, DocLiquidar, TipoDoc, "WEBCOBROS")


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

            Dim Reserva As String
            Dim Recibo As String
            Dim TipoDoc As String

            SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA,MOVI_CORR,MOVI_ANUL,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI,TNHT_FACT.FAAN_CODI,NVL(SECC_CODI,'?') AS SECC_CODI "

            SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) RECI_COBR "
            SQL += ",TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA"
            SQL += " ,NVL(MOVI_DESC,' ') MOVI_DESC "

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
            SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) ,TNHT_MOVI.RESE_CODI , TNHT_MOVI.RESE_ANCI,MOVI_DESC"


            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read

                If Me.mParaDistingeAnticiposDeDepositos = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                    Cuenta = Me.mParaCta4b3Visa & Me.mParaSufijoDepositos
                    '   TipoDoc = Me.mParaMoraTDocDeposito
                    TipoDoc = ""
                Else
                    Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String) & Me.mParaSufijoAnticipos
                    '  TipoDoc = Me.mParaMoraTDocDeposito 
                    TipoDoc = ""
                End If


                Linea = Linea + 1

                If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                    Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Else
                    Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
                End If


                If IsDBNull(Me.DbLeeHotel.mDbLector("RECI_COBR")) Then
                    Recibo = ""
                Else
                    Recibo = CStr(Me.DbLeeHotel.mDbLector("RECI_COBR")).Trim
                End If


                If IsDBNull(Me.DbLeeHotel.mDbLector("RESERVA")) Then
                    Reserva = ""
                Else
                    Reserva = CStr(Me.DbLeeHotel.mDbLector("RESERVA"))
                End If



                Factura = CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String)

                Me.mTipoAsiento = "DEBE"
                '   Me.InsertaOracleMorasol("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & Factura, Total, "NO", "", "", "SI", "COBRO", Me.Multidiario, "", "", "", "", "", "", "", "FIN", "WEBPDTE", "WEBCOBROS")
                Me.InsertaOracleMorasol("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & Factura, Total, "NO", "", "", "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Factura, Me.mParaMoraTMovBanco, "", "", Me.mParaMoraDimenHotel, Reserva, Recibo, "", "WEBCOBROS")


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

        Dim Reserva As String
        Dim Recibo As String
        Dim TipoDoc As String

        SQL = "SELECT SUM(TNHT_MOVI.MOVI_VDEB)TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_CORR,MOVI_ANUL,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI,TNHT_FACT.FAAN_CODI,NVL(SECC_CODI,'?') AS SECC_CODI "

        SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) RECI_COBR "
        SQL += ",TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA"
        SQL += " ,NVL(MOVI_DESC,' ') MOVI_DESC "

        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FORE,TNHT_RESE,TNHT_FAMO,TNHT_FACT WHERE"
        SQL += "     TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL += " And TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL += " And TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL += " And TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI And "


        SQL += " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL += " And TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL += " And TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
        SQL += " And (TNHT_MOVI.TIRE_CODI = 1 And TNHT_MOVI.MOVI_AUTO = 1  And TNHT_MOVI.MOVI_VDEB < 0 ) "

        SQL += " And TNHT_MOVI.CACR_CODI Is NULL"
        '   SQL += " And TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL += " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"



        SQL = SQL & " GROUP BY TNHT_MOVI.TIRE_CODI,TNHT_MOVI.FORE_CODI,TNHT_FORE.FORE_DESC,TNHT_FORE.FORE_CTB1,MOVI_CORR,MOVI_ANUL,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI,TNHT_FACT.FAAN_CODI,SECC_CODI"
        SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) ,TNHT_MOVI.RESE_CODI , TNHT_MOVI.RESE_ANCI,MOVI_DESC"

        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read

            If Me.mParaDistingeAnticiposDeDepositos = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b2Efectivo & Me.mParaSufijoDepositos
                '  TipoDoc = Me.mParaMoraTDocDeposito
                TipoDoc = ""
            Else
                Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String) & Me.mParaSufijoAnticipos
                '   TipoDoc = Me.mParaMoraTDocDeposito
                TipoDoc = ""
            End If

            Linea = Linea + 1

            If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("RECI_COBR")) Then
                Recibo = ""
            Else
                Recibo = CStr(Me.DbLeeHotel.mDbLector("RECI_COBR")).Trim
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("RESERVA")) Then
                Reserva = ""
            Else
                Reserva = CStr(Me.DbLeeHotel.mDbLector("RESERVA"))
            End If

            Factura = CType(Me.DbLeeHotel.mDbLector("FACT_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String)

            Me.mTipoAsiento = "DEBE"
            '   Me.InsertaOracleMorasol("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " " & Factura, Total, "NO", "", "", "SI", "COBRO", Me.Multidiario, "", "", "", "", "", "", "", "FIN", "WEBPDTE", "WEBCOBROS")
            Me.InsertaOracleMorasol("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TIPO"), String) & " " & Factura, Total, "NO", "", "", "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Factura, Me.mParaMoraTMovBanco, "", "", Me.mParaMoraDimenHotel, Reserva, Recibo, "", "WEBCOBROS")


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub DetalleDevolucionesVisasFacturado()

        Dim Total As Double
        Dim Cuenta As String = ""

        Dim Factura As String

        Dim Reserva As String
        Dim Recibo As String
        Dim TipoDoc As String
        Dim DocLiquidar As String


        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(TNHT_RESE.RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,"
        SQL += " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA,MOVI_CORR,MOVI_ANUL,TNHT_FACT.FACT_CODI AS NUMERO ,TNHT_FACT.SEFA_CODI AS SERIE,TNHT_FACT.FAAN_CODI,NVL(SECC_CODI,'?') AS SECC_CODI "

        SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) RECI_COBR "
        SQL += ",TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA"
        SQL += " ,NVL(MOVI_DESC,' ') MOVI_DESC "

        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,"
        SQL += " TNHT_CACR,TNHT_RESE,TNHT_FAMO,TNHT_FACT WHERE "

        SQL += "     TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL += " And TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL += " And TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL += " And TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI And "

        SQL += " TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
        SQL += " And TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL += " And TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
        SQL += " And (TNHT_MOVI.TIRE_CODI = 1 And TNHT_MOVI.MOVI_AUTO = 1  And TNHT_MOVI.MOVI_VDEB < 0 ) "
        '   SQL += " And TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL += " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"




        Me.NEWHOTEL = New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)


        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1

            If Me.mParaDistingeAnticiposDeDepositos = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b & Me.mParaSufijoDepositos
                '    TipoDoc = Me.mParaMoraTDocDeposito
                TipoDoc = ""
            Else
                Cuenta = Me.mCtaPagosACuenta & Me.mParaSufijoAnticipos
                '    TipoDoc = Me.mParaMoraTDocDeposito
                TipoDoc = ""

            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("RECI_COBR")) Then
                Recibo = ""
            Else
                Recibo = CStr(Me.DbLeeHotel.mDbLector("RECI_COBR")).Trim
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("RESERVA")) Then
                Reserva = ""
            Else
                Reserva = CStr(Me.DbLeeHotel.mDbLector("RESERVA"))
            End If


            Factura = CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String)
            DocLiquidar = Me.LocaLizareciboMayorAnticipo(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))

            Me.mTipoAsiento = "HABER"


            '       Me.InsertaOracleMorasol("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Devol. Pag A Cuenta RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "", "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Factura, Me.mParaMoraTMovBanco, "", "", Me.mParaMoraDimenHotel, Reserva, Recibo, TipoDoc, "WEBCOBROS")
            Me.InsertaOracleMorasol("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Devol. Pag A Cuenta RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "", "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Factura, Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, Reserva, DocLiquidar, TipoDoc, "WEBCOBROS")





        End While
        Me.DbLeeHotel.mDbLector.Close()
        Me.NEWHOTEL.CerrarConexiones()
    End Sub
    Private Sub DetalleDevolucionesOtrasFormasFacturado()
        Dim Total As Double
        Dim Cuenta As String = ""

        Dim Reserva As String
        Dim Recibo As String
        Dim TipoDoc As String
        Dim Factura As String

        Dim DocLiquidar As String

        SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(TNHT_RESE.RESE_ANPH,'?') CLIENTE,TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_DAVA,MOVI_CORR,MOVI_ANUL,TNHT_FACT.FACT_CODI AS NUMERO ,TNHT_FACT.SEFA_CODI AS SERIE,TNHT_FACT.FAAN_CODI,NVL(SECC_CODI,'?') AS SECC_CODI "

        SQL += ",SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) RECI_COBR "
        SQL += ",TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA"
        SQL += " ,NVL(MOVI_DESC,' ') MOVI_DESC"

        SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FORE,TNHT_RESE,TNHT_FAMO,TNHT_FACT WHERE"
        SQL += "     TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI"
        SQL += " And TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI"
        SQL += " And TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE"
        SQL += " And TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI And "

        SQL += " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
        SQL += " And TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
        SQL += " And TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
        SQL += " And (TNHT_MOVI.TIRE_CODI = 1 And TNHT_MOVI.MOVI_AUTO = 1  And TNHT_MOVI.MOVI_VDEB < 0 ) "

        SQL += " And TNHT_MOVI.CACR_CODI Is NULL"
        '   SQL += " And TNHT_MOVI.MOVI_DATR = " & "'" & Me.mFecha & "'"
        SQL += " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"




        Me.NEWHOTEL = New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)



        Me.DbLeeHotel.TraerLector(SQL)
        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1

            If Me.mParaDistingeAnticiposDeDepositos = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
                Cuenta = Me.mParaCta4b & Me.mParaSufijoDepositos
                '  TipoDoc = Me.mParaMoraTDocDeposito
                TipoDoc = ""
            Else
                Cuenta = Me.mCtaPagosACuenta & Me.mParaSufijoAnticipos
                '     TipoDoc = Me.mParaMoraTDocDeposito
                TipoDoc = ""
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Else
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double) * -1
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("RECI_COBR")) Then
                Recibo = ""
            Else
                Recibo = CStr(Me.DbLeeHotel.mDbLector("RECI_COBR")).Trim
            End If


            If IsDBNull(Me.DbLeeHotel.mDbLector("RESERVA")) Then
                Reserva = ""
            Else
                Reserva = CStr(Me.DbLeeHotel.mDbLector("RESERVA"))
            End If

            Factura = CType(Me.DbLeeHotel.mDbLector("NUMERO"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SERIE"), String)
            DocLiquidar = Me.LocaLizareciboMayorAnticipo(CInt(Me.DbLeeHotel.mDbLector("NUMERO")), CStr(Me.DbLeeHotel.mDbLector("SERIE")))


            Me.mTipoAsiento = "HABER"
            '      Me.InsertaOracleMorasol("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Devol. Pag A Cuenta RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "", "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Factura, Me.mParaMoraTMovBanco, "", "", Me.mParaMoraDimenHotel, Reserva, Recibo, TipoDoc, "WEBCOBROS")
            Me.InsertaOracleMorasol("AC", 22, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "Devol. Pag A Cuenta RVA= " & CType(Me.DbLeeHotel.mDbLector("RESERVA"), String) & " " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), String).Replace("'", "''"), Total, "NO", "", "", "SI", "", Me.Multidiario, CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String), Factura, Me.mParaMoraTMovCliente, "", "", Me.mParaMoraDimenHotel, Reserva, DocLiquidar, TipoDoc, "WEBCOBROS")







        End While
        Me.DbLeeHotel.mDbLector.Close()
        Me.NEWHOTEL.CerrarConexiones()
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
    Private Sub GeneraComprobanteBancoVisa(vNumAsiento As Integer, vTotal As Double, vDescripcion As String, vBancosCod As String)
        Try
            SQL = "SELECT TH_COMPROBANTES.NEXTVAL FROM DUAL"
            Me.mVisaComprobante = CInt(Me.DbLeeCentral.EjecutaSqlScalar2(SQL))

            SQL = "SELECT TH_FACTURAS.NEXTVAL FROM DUAL"
            Me.mVisaFactura = CInt(Me.DbLeeCentral.EjecutaSqlScalar2(SQL))

            SQL = "SELECT NVL(PARA_FACTUTIPO_COD,'?')  "
            SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
            Me.mVisaFacturaSerie = CStr(Me.DbLeeCentral.EjecutaSqlScalar(SQL))


            SQL = "SELECT NVL(PARA_CFBCOTMOV_COD,'?')  "
            SQL += " FROM TC_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
            Me.mVisaCfbcotmov = CStr(Me.DbLeeCentral.EjecutaSqlScalar(SQL))
            ' GENERA FV , CB, MG,AC

            Dim Cuenta As String = Me.mCtaClientesContado


            '   Me.GeneraFileFVDiariodeCobros("FV", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, Me.mVisaFacturaSerie, Me.mVisaFactura, vTotal, Me.mVisaFactura & "/" & Me.mVisaFacturaSerie, Cuenta, Me.mClientesContadoCif, vTotal)
            Me.GeneraFileFVDiariodeCobros("FV", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, Me.mVisaFacturaSerie, Me.mVisaFactura, vTotal, Me.mVisaFactura & "/" & Me.mVisaFacturaSerie, Cuenta, Me.mClientesContadoCif, vTotal)
            Me.GeneraFileCB("CB", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, Me.mVisaFacturaSerie, Me.mVisaFactura, vTotal, "", "", "", vTotal, vBancosCod, Me.mVisaCfbcotmov, Me.mVisaComprobante)
            Me.GeneraFileMG("MG", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, Me.mVisaFacturaSerie, Me.mVisaFactura, vTotal, "", "", "", 0, Me.mVisaCfbcotmov, vBancosCod, Me.mVisaComprobante)


        Catch ex As Exception

        End Try

    End Sub

    Private Function GetReservasdeUnaFactura(vFactura As Integer, vSerie As String) As String
        Try

            Dim Reserva As String = ""
            Dim TotalReservas As Integer
            Dim EstaenDefa As Boolean = False
            ' Ver si la fctura es de una sola reserva (morasol)

            ' Busca reservas en DEFA
            EstaenDefa = False

            SQL = "SELECT COUNT(DISTINCT RESE_CODI) AS TOTAL  "
            SQL += "FROM TNHT_DEFA   "
            SQL += "WHERE FACT_CODI =  " & vFactura
            SQL += " AND SEFA_CODI = '" & vSerie & "'"
            SQL += " GROUP BY FACT_CODI,SEFA_CODI "

            Me.mResultStr = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            If IsNothing(Me.mResultStr) = True Then
                EstaenDefa = False
                ' Busca reservas en FAEN
                SQL = "SELECT COUNT(DISTINCT RESE_CODI) AS TOTAL  "
                SQL += "FROM TNHT_FAEN   "
                SQL += "WHERE FACT_CODI =  " & vFactura
                SQL += " AND SEFA_CODI = '" & vSerie & "'"
                SQL += " GROUP BY FACT_CODI,SEFA_CODI "

                Me.mResultStr = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            Else
                EstaenDefa = True
            End If



            If IsNothing(Me.mResultStr) = False Then
                TotalReservas = CInt(Me.mResultStr)
                If TotalReservas > 1 Then
                    Reserva = "Múltiples Reservas"
                Else
                    SQL = "SELECT RESE_CODI|| '/' || RESE_ANCI AS RESERVA "
                    If EstaenDefa Then
                        SQL += "FROM TNHT_FACT  "
                    Else
                        SQL += "FROM TNHT_FAEN  "
                    End If

                    SQL += "WHERE FACT_CODI =  " & vFactura
                    SQL += " AND SEFA_CODI = '" & vSerie & "'"
                    Reserva = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                End If

            Else
                TotalReservas = 0
                Reserva = ""

            End If

            Return Reserva
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
    Private Function GetReservasdeUnaNotaDeCredito(vNota As Integer, vSerie As String) As String
        Try

            Dim Reserva As String = ""
            Dim TotalReservas As Integer
            Dim EstaenDefa As Boolean = False
            ' Ver si la fctura es de una sola reserva (morasol)


            SQL = "  SELECT COUNT(DISTINCT RESE_CODI) "
            SQL += "    FROM TNHT_MOVI TNHT_MOVI "
            SQL += "   WHERE (TNHT_MOVI.MOVI_CODI, TNHT_MOVI.MOVI_DARE) IN "
            SQL += "            (SELECT MOVI_CODI, MOVI_DARE "
            SQL += "               FROM TNHT_MCRE "
            SQL += "              WHERE SEFA_CODI = " & "'" & vSerie & "'"
            SQL += " AND NCRE_CODI = " & vNota & ") "

            Me.mResultStr = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)



            If IsNothing(Me.mResultStr) = False Then
                TotalReservas = CInt(Me.mResultStr)
                If TotalReservas > 1 Then
                    Reserva = "Múltiples Reservas"
                Else
                    SQL = "SELECT RESE_CODI|| '/' || RESE_ANCI AS RESERVA "
                    SQL += "    FROM TNHT_MOVI  "
                    SQL += "   WHERE (TNHT_MOVI.MOVI_CODI, TNHT_MOVI.MOVI_DARE) IN "
                    SQL += "            (SELECT MOVI_CODI, MOVI_DARE "
                    SQL += "               FROM TNHT_MCRE "
                    SQL += "              WHERE SEFA_CODI = " & "'" & vSerie & "'"
                    SQL += " AND NCRE_CODI = " & vNota & ") "


                    Reserva = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                End If

            Else
                TotalReservas = 0
                Reserva = ""

            End If

            Return Reserva
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function


    Private Sub GeneraComprobanteBancarioAgrupado()
        Try
            Dim Total As Double
            Dim Cuenta As String


            SQL = "SELECT SUM(TOTAL) TOTAL, "
            SQL += "         TARJETA, "
            SQL += "         BANCO ,CUENTA FROM ( "
            SQL += "  SELECT SUM(TNHT_MOVI.MOVI_VDEB) TOTAL, "
            SQL += "         TNHT_CACR.CACR_DESC TARJETA, "
            SQL += "        NVL (TNHT_CACR.CACR_CTBA, 'X')  CUENTA, "
            SQL += "        NVL (TNHT_CACR.CACR_CTB3, 'X')  BANCO "
            SQL += "         "
            SQL += "    FROM TNHT_MOVI, TNHT_CACR, TNHT_RESE "
            SQL += "   WHERE     TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI "
            SQL += "         AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += "         AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
            SQL += "         AND TNHT_MOVI.TIRE_CODI = 1 "
            SQL += "         AND TNHT_MOVI.MOVI_DATR =  " & "'" & Me.mFecha & "'"
            SQL += "         AND TNHT_MOVI.MOVI_VDEB <> 0 "
            SQL += "         AND TNHT_MOVI.MOVI_AUTO = '0' "
            SQL += "GROUP BY CACR_DESC,CACR_CTB3,CACR_CTBA "
            SQL += "UNION ALL "
            SQL += " SELECT SUM(MOVI_VDEB) TOTAL, "
            SQL += "         CACR_DESC TARJETA, "
            SQL += "        NVL (TNHT_CACR.CACR_CTBA, 'X')  CUENTA, "
            SQL += "       NVL (TNHT_CACR.CACR_CTB3, 'X')  BANCO "
            SQL += "        "
            SQL += "    FROM TNHT_MOVI, "
            SQL += "         TNHT_CACR, "
            SQL += "         TNHT_FACT, "
            SQL += "         TNHT_FAMO "
            SQL += "   WHERE     TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "         AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "         AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
            SQL += "         AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL += "         AND TNHT_MOVI.TIRE_CODI = '1' "
            SQL += "         AND TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI "
            SQL += "         AND (TNHT_MOVI.MOVI_AUTO = '1' "
            SQL += "              OR TNHT_MOVI.MOVI_AUTO = '0' AND TNHT_MOVI.CCEX_CODI = 'TPV') "
            SQL += "         AND TNHT_FACT.FACT_DAEM =  " & "'" & Me.mFecha & "'"
            SQL += "         AND TNHT_MOVI.MOVI_VDEB > 0 "
            SQL += "GROUP BY CACR_DESC,CACR_CTB3,CACR_CTBA) "
            SQL += "GROUP BY TARJETA,BANCO ,CUENTA"




            Me.DbLeeHotel.TraerLector(SQL)
            Linea = 0
            While Me.DbLeeHotel.mDbLector.Read

                Cuenta = CType(Me.DbLeeHotel.mDbLector("CUENTA"), String)

                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                If Total <> 0 Then

                    'FASE 2 2016 COMPROBANTES BANCARIOS  AGRUPADO !

                    Me.GeneraComprobanteBancoVisa(2, Total, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & Format(Me.mFecha, "dd/MM/yyyy"), CType(Me.DbLeeHotel.mDbLector("BANCO"), String))
                    Linea = Linea + 1
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & Format(Me.mFecha, "dd/MM/yyyy"), Total, "NO", "", "Comprobante Bancario Nº: " & Me.mVisaComprobante, "SI", "", Me.Multidiario, "", Me.mVisaCfbcotmov, CType(Me.DbLeeHotel.mDbLector("CACR_CTB3"), String), Me.mVisaComprobante)
                    Me.GeneraFileACMultiDiarioComprobantesBancarios("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("TARJETA"), String) & " " & Format(Me.mFecha, "dd/MM/yyyy"), Total, Me.Multidiario, CStr(Me.DbLeeHotel.mDbLector("CACR_CTB3")), Me.mVisaCfbcotmov, Me.mVisaComprobante)
                End If



            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Information, "Pagos a Cuenta VISAS")
        End Try
    End Sub




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
            ' Asiento produccion  1
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then

                Me.PendienteFacturarTotal()
                Me.mTextDebug.Text = "Venta Total"
                Me.mTextDebug.Update()


                Me.VentasDepartamento()
                Me.mTextDebug.Text = "Venta por Departamento"
                Me.mTextDebug.Update()


                Me.mProgress.Value = 10
                Me.mProgress.Update()


            End If

            System.Windows.Forms.Application.DoEvents()
            Me.mForm.Update()


            ' ---------------------------------------------------------------
            ' Asiento de Pagos a Cuenta 2
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then


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

                'FASE 2 2016 COMISIONES VISAS 
                Me.TotalPagosaCuentaVisasComision()
                Me.mTextDebug.Text = "COMISION Visas  de Pagos a Cuenta "
                Me.mTextDebug.Update()

                'FASE 2 2016 GENERA COMPROBANTE BANCARIO AGRUPADO POR DIA Y BANCO
                ' GeneraComprobanteBancarioAgrupado()

                Me.mProgress.Value = 20
                Me.mProgress.Update()
            End If



            System.Windows.Forms.Application.DoEvents()
            Me.mForm.Update()


            ' ---------------------------------------------------------------
            ' Asiento de DEVOLUCIONES  21
            '----------------------------------------------------------------

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


            System.Windows.Forms.Application.DoEvents()
            Me.mForm.Update()


            ' ---------------------------------------------------------------
            ' Asiento de DEVOLUCIONES HECHAS POR NEWHOTEL AUTOMATICAS AL FACTURAR  22 
            '----------------------------------------------------------------

            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then

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


            System.Windows.Forms.Application.DoEvents()
            Me.mForm.Update()

            ' ---------------------------------------------------------------
            ' Asiento Facturacion total del dia 3
            '----------------------------------------------------------------

            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then


                ' nuevo bloquua a todas las demas llamadas de este tipo de asiento
                '  Me.NFacturasSalidaLineasTito()



                Me.NFacturasSalidaTotalFActuraNuevo()
                Me.mTextDebug.Text = "Calculando Total Pendiente de Facturar"

                Me.mTextDebug.Update()

                Me.mProgress.Value = 10
                Me.mProgress.Update()



                Me.mTextDebug.Text = "Calculando Descuentos Financieros y Comisiones Facturas"
                Me.mTextDebug.Update()
                '2A

                Me.mProgress.Value = 20
                Me.mProgress.Update()




                ' no se envia AGRUPADO POR SERIE / sino factura por factura
                '   Me.NFacturasSalidaBaseImponibleAgrupado()

                ' DEBAJO original morasol ( no hace falta enviar)
                ' Me.NFacturasSalidaBaseImponible()


                Me.mTextDebug.Text = "Detalle de Impuesto Facturas "
                Me.mTextDebug.Update()
                '3

                Me.NFacturasSalidaDetalleIgic()



                ' test 
                '     Me.DescuentosFinancieros()

                Me.mProgress.Value = 30
                Me.mProgress.Update()


            Else
                MsgBox("No dispone de Conexión a la Base de Datos", MsgBoxStyle.Information, "Atención")

            End If

            System.Windows.Forms.Application.DoEvents()
            Me.mForm.Update()


            ' ---------------------------------------------------------------
            ' Asiento Facturacion Contado del dia   CAJA      35 
            '----------------------------------------------------------------


            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then


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

                If Me.mParaDistingeAnticiposDeDepositos = False Then
                    ' original
                    Me.SaldoAnticiposFacturas()
                    ' test
                    '  Me.FacturasContadoCancelaciondeAnticipos()
                Else
                    'original
                    Me.SaldoAnticiposFacturasPorSeccion()
                    ' test

                    ' Me.FacturasCancelaciondeAnticipos()
                End If


                Me.mTextDebug.Text = "Cancelación de Anticipos  Facturas de Salida"
                Me.mTextDebug.Update()

                'FASE 2 2016 COMISIONES VISAS 
                Me.FacturasContadoTotalVisasComision()

                Me.mProgress.Value = 30
                Me.mProgress.Update()
            End If




            System.Windows.Forms.Application.DoEvents()
            Me.mForm.Update()


            ' ---------------------------------------------------------------
            ' Asiento Notas de Credito de Credito Entidades 51
            '----------------------------------------------------------------
            'PDTE: CAMBIAR A INSERTARORACLE DE AQUI HACIA DEBAJO


            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then

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

                    ' debajo otiginal (tito no hace falta enviar la base)
                    'Me.NotasDeCreditoEntidadCreditoBaseImponible()
                    ' Me.NotasDeCreditoEntidadCreditoBaseImponibleAgrupado()

                    Me.mProgress.Value = 70
                    Me.mProgress.Update()



                    Me.mTextDebug.Text = "Líquido Notas de Crédito"
                    Me.mTextDebug.Update()



                    Me.mProgress.Value = 100
                    Me.mProgress.Update()
                End If




            Else
                MsgBox("No dispone de Conexión a la Base de Datos", MsgBoxStyle.Information, "Atención")
            End If


            System.Windows.Forms.Application.DoEvents()
            Me.mForm.Update()




            ' VALIDACION DE CUENTAS EB SPYRO TODAS JUNTAS AL FINAL

            '   MsgBox("SE VALIDAN CUENTAS AL FINAL")

            If Me.mParaValidaSpyro = 1 Then
                Me.SpyroCompruebaCuentas()
                Me.SpyroCompruebaBancos()
            End If




            Me.AjustarDecimales()
            Me.mProgress.Value = 100
            Me.mProgress.Update()

            Me.CerrarFichero()
            '  Me.CierraConexiones()
            Me.mTextDebug.Text = "Fin de Integración"
            Me.mTextDebug.Update()

        Catch EX As Exception
            If FileEstaOk Then
                CerrarFichero()
            End If
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

    Private Function LocaLizareciboMayorAnticipo(vFactura As Integer, vSerie As String) As String
        Try

            Dim Result As String




            SQL = "  "
            SQL = SQL & "SELECT  MAX (TNHT_MOVI.MOVI_VDEB) TOTAL, "
            SQL = SQL & "       SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20) AS RECIBO "
            SQL = SQL & "  FROM TNHT_MOVI, "
            SQL = SQL & "       TNHT_FACT, "
            SQL = SQL & "       TNHT_RESE, "
            SQL = SQL & "       TNHT_FAMO "
            SQL = SQL & " WHERE     TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL = SQL & "       AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL = SQL & "       AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE "
            SQL = SQL & "       AND TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI "
            SQL = SQL & "       AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL = SQL & "       AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
            SQL = SQL & "       AND TNHT_MOVI.MOVI_TIMO = '2' "
            SQL = SQL & "       AND TNHT_MOVI.TIRE_CODI = '1' "
            SQL = SQL & "       AND TNHT_MOVI.MOVI_VDEB <> 0 "
            SQL = SQL & "       AND TNHT_MOVI.MOVI_AUTO = '0' "
            SQL = SQL & "       AND TNHT_MOVI.SECC_CODI = '" & Me.mParaSecc_AntNh & "'"
            SQL = SQL & "       AND TNHT_FACT.FACT_CODI  = " & vFactura
            SQL = SQL & "       AND TNHT_FACT.SEFA_CODI  = '" & vSerie & "'"

            ' ANUL 0 ?
            SQL = SQL & "       AND TNHT_MOVI.MOVI_ANUL = 0 "
            SQL = SQL & " "
            SQL = SQL & "         GROUP BY SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20)  "
            SQL = SQL & "         ORDER BY TOTAL DESC "





            Me.DbLeeHotelAux2.TraerLector(SQL)
            Me.DbLeeHotelAux2.mDbLector.Read()

            If Me.DbLeeHotelAux2.mDbLector.HasRows Then
                Result = CStr(Me.DbLeeHotelAux2.mDbLector("RECIBO"))
            Else
                Result = "?"
            End If

            Me.DbLeeHotelAux2.mDbLector.Close()

            Return Result




        Catch ex As Exception
            MsgBox(ex.Message,, "LocaLizareciboMayorAnticipo")
            Return "?"
        End Try
    End Function
    Private Function LocaLizareciboDeRecepcionDeDeposito(vReseCodi As Integer, vReseAnci As Integer, vSeccCodi As String, vValor As Double, vDatr As Date, vReciboDev As String) As String
        Try

            Dim Result As String = ""



            SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,"
            SQL += "FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO) RECIBO,NVL(MOVI_NUDO,' ') MOVI_NUDO,NVL(MOVI_DESC,' ') MOVI_DESC,NVL(SECC_CODI,'?') AS SECC_CODI "

            SQL += ",TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA"
            SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
            SQL = SQL & " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
            SQL = SQL & " AND TNHT_MOVI.TIRE_CODI = 1"


            SQL = SQL & " AND TNHT_MOVI.RESE_CODI = " & vReseCodi
            SQL = SQL & " AND TNHT_MOVI.RESE_ANCI = " & vReseAnci
            SQL = SQL & " AND TNHT_MOVI.SECC_CODI = '" & vSeccCodi & "'"

            SQL = SQL & " AND TNHT_MOVI.MOVI_VDEB = " & vValor
            SQL = SQL & " AND TNHT_MOVI.MOVI_DATR <= " & "'" & vDatr & "'"
            ' no anulado
            SQL = SQL & " AND TNHT_MOVI.MOVI_ANUL = 0"
            ' no facturado ( PENSAR ESTO ) 
            ' SQL = SQL & " AND TNHT_MOVI.FACT_CODI IS NULL "


            If Me.mTrataDebitoTpvnoFacturado = True Then
                ' EXCLUYE CIERRE DE CONTADO DE TPV
                SQL += " AND TNHT_MOVI.UTIL_CODI <> 'POS'"
                ' EXCLUYE CIERRE DE CONTADO DE GOLF
                SQL += " AND TNHT_MOVI.UTIL_CODI <> 'GMS'"
            End If



            If Me.mUsaTnhtMoviAuto = True Then
                SQL = SQL & " AND TNHT_MOVI.MOVI_AUTO = '0' "
            End If

            ' ORDEN DESCENDENTE PARA TRATAR DE DEVOLVER EL "ULTIMO" RECIBO DE ESE IMPORTE 
            SQL = SQL & " ORDER BY TNHT_MOVI.MOVI_HORE DESC "




            ' OJO SOLO SE BUSCA EL PRIMER RECIBO DEL MISMO IMPORTE QUE APEREZCA ( PUEDE HABER MAS ) 

            Me.DbLeeHotelAux2.TraerLector(SQL)
            Me.DbLeeHotelAux2.mDbLector.Read()

            If Me.DbLeeHotelAux2.mDbLector.HasRows Then
                If IsDBNull(CStr(Me.DbLeeHotelAux2.mDbLector("RECIBO"))) = False Then
                    Result = CStr(Me.DbLeeHotelAux2.mDbLector("RECIBO"))

                Else
                    Result = "NO LOCALIZADO"
                End If


            Else
                Result = "NO LOCALIZADO"
            End If

            Me.DbLeeHotelAux2.mDbLector.Close()

            Return Result




        Catch ex As Exception
            MsgBox(ex.Message,, "LocaLizareciboMayorAnticipo")
            Return "?"
        End Try
    End Function

    Private Function LocaLizaReciboAnulado(vMoviCodi As Integer, vMoviDare As Date) As String
        Try

            Dim Result As String = ""



            SQL = "SELECT "
            SQL += "FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO) RECIBO "
            SQL += " FROM " & Me.mStrHayHistorico & " TNHT_MOVI"
            SQL += " WHERE MOVI_CODI  = " & vMoviCodi
            SQL += " AND MOVI_DARE = " & "'" & vMoviDare & "'"






            Me.DbLeeHotelAux2.TraerLector(SQL)
            Me.DbLeeHotelAux2.mDbLector.Read()

            If Me.DbLeeHotelAux2.mDbLector.HasRows Then
                If IsDBNull(CStr(Me.DbLeeHotelAux2.mDbLector("RECIBO"))) = False Then
                    Result = CStr(Me.DbLeeHotelAux2.mDbLector("RECIBO"))
                Else
                    Result = "NO LOCALIZADO"
                End If


            Else
                Result = "NO LOCALIZADO"
            End If

            Me.DbLeeHotelAux2.mDbLector.Close()

            Return Result




        Catch ex As Exception
            MsgBox(ex.Message,, "LocaLizareciboMayorAnticipo")
            Return "?"
        End Try
    End Function
#End Region
End Class
