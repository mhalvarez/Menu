Option Strict On
Imports System.IO
Imports System.Globalization
Public Class NewPaga
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



    Declare Function CharToOem Lib "user32" Alias "CharToOemA" (ByVal lpszSrc As String, ByVal lpszDst As String) As Long
    Declare Function OemToChar Lib "user32" Alias "OemToCharA" (ByVal lpszSrc As String, ByVal lpszDst As String) As Long


    Private mDebug As Boolean = False
    Private mStrConexionHotel As String
    Private mStrConexionCentral As String
    Private mStrConexionSpyro As String
    Private mStrConexionNewStock As String

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

    Private mParaUsaCtaPuentePagos As Boolean
    Private mParaCtaPuentePagos As String
    Private mParaAgrupaCtaPuentePagos As Boolean



    Private mUsaTnhtMoviAuto As Boolean

    Private mEstablecimientoNewConta As String






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

    Private mParaNewPagaMovTrans As String
    Private mParaNewPagaMovTransAuxiliar As String





    ' OTROS 
    Private iASCII(63) As Integer       'Para conversión a MS-DOS
    Private AuxCif As String
    Private AuxTipoMovimiento As String
    Private AuxDoc As String


    Private SQL As String
    Private Linea As Integer
    Private mTexto As String
    Private Filegraba As StreamWriter
    Private FileEstaOk As Boolean = False
    Private DbLeeCentral As C_DATOS.C_DatosOledb
    Private DbNewPaga As C_DATOS.C_DatosOledb
    Private DbNewPagaAux As C_DATOS.C_DatosOledb
    Private DbGrabaCentral As C_DATOS.C_DatosOledb
    Private DbSpyro As C_DATOS.C_DatosOledb
    Private DbNewStock As C_DATOS.C_DatosOledb

    Private mProcesaAnulados As Boolean
    Private mAuxStr As String
    Private TipoAsiento As Integer

    Private mResult As String

    Private Enum mEnumEstaDo
        NoAnulado
        SiAnulado

    End Enum

#Region "CONSTRUCTOR"
    Public Sub New(ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vStrConexionCentral As String,
    ByVal vStrConexionNewPaga As String, ByVal vFecha As Date, ByVal vFileName As String, ByVal vDebug As Boolean,
    ByVal vConrolDebug As System.Windows.Forms.TextBox, ByVal vListBox As System.Windows.Forms.ListBox,
    ByVal vStrConexionSpyro As String, ByVal vProgress As System.Windows.Forms.ProgressBar, vStrConexionNewStock As String, vEmpNum As Integer)


        MyBase.New()

        Me.mDebug = vDebug
        Me.mEmpGrupoCod = vEmpGrupoCod
        Me.mEmpCod = vEmpCod
        Me.mEmpNum = vEmpNum
        Me.mStrConexionHotel = vStrConexionNewPaga
        Me.mStrConexionCentral = vStrConexionCentral
        Me.mStrConexionSpyro = vStrConexionSpyro
        Me.mStrConexionNewStock = vStrConexionNewStock
        Me.mFecha = vFecha

        Me.mParaFileName = vFileName




        Me.mTextDebug = vConrolDebug

        Me.mProgress = vProgress
        Me.mProgress.Value = 0
        Me.mProgress.Maximum = 100

        Me.mListBoxDebug = vListBox

        Me.mListBoxDebug.Items.Clear()
        Me.mListBoxDebug.Update()




        Me.AbreConexiones()
        Me.CargaParametros()
        Me.CargaParametrosNewStock()

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

            Me.DbNewPaga = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
            Me.DbNewPaga.AbrirConexion()
            Me.DbNewPaga.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbNewPagaAux = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
            Me.DbNewPagaAux.AbrirConexion()
            Me.DbNewPagaAux.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbNewStock = New C_DATOS.C_DatosOledb(Me.mStrConexionNewStock)
            Me.DbNewStock.AbrirConexion()
            Me.DbNewStock.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

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
            SQL += "NVL(PARA_CTA_SERIE_NOTAS,'0') CTANOTAS"



            SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
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
            End If
            Me.DbLeeCentral.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Exclamation, "Carga de Parámetros en Constructor de la Clase")
        End Try
    End Sub
    Private Sub CargaParametrosNewStock()
        Try
            Dim Aux() As String

            Me.mTextDebug.Text = "Cargando Parámetros NewStock"
            Me.mTextDebug.Update()

            SQL = "SELECT  "
            SQL += "PARA_SPYRO_TIPO_MOVTRANS "

            SQL += ",NVL(PARA_USA_CUENTAPUENTE,'0') PARA_USA_CUENTAPUENTE"
            SQL += ",NVL(PARA_CTAPUENTE_PAGOS,'0') PARA_CTAPUENTE_PAGOS"
            SQL += ",NVL(PARA_CUENTAPUENTE_AGRUPA,'0') PARA_CUENTAPUENTE_AGRUPA"


            SQL += " FROM TS_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
            Me.DbLeeCentral.TraerLector(SQL)
            If Me.DbLeeCentral.mDbLector.Read Then

                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("PARA_SPYRO_TIPO_MOVTRANS")) = False Then
                    Me.mParaNewPagaMovTrans = CStr(Me.DbLeeCentral.mDbLector.Item("PARA_SPYRO_TIPO_MOVTRANS"))
                    Me.mParaNewPagaMovTransAuxiliar = Me.mParaNewPagaMovTrans

                    Aux = Split(Me.mParaNewPagaMovTrans, ",")
                    Me.mParaNewPagaMovTrans = ""

                    For I As Integer = 0 To Aux.Length - 1
                        If I < Aux.Length - 1 Then
                            Me.mParaNewPagaMovTrans += "'" & Aux(I) & "',"

                        Else
                            Me.mParaNewPagaMovTrans += "'" & Aux(I) & "'"
                        End If
                    Next

                Else
                    Me.mParaNewPagaMovTrans = ""
                    Me.mParaNewPagaMovTransAuxiliar = ""
                End If


                If CStr(Me.DbLeeCentral.mDbLector.Item("PARA_USA_CUENTAPUENTE")) = "0" Then
                    mParaUsaCtaPuentePagos = False
                Else
                    mParaUsaCtaPuentePagos = True
                End If

                mParaCtaPuentePagos = CStr(Me.DbLeeCentral.mDbLector.Item("PARA_CTAPUENTE_PAGOS"))

                If CStr(Me.DbLeeCentral.mDbLector.Item("PARA_CUENTAPUENTE_AGRUPA")) = "0" Then
                    mParaAgrupaCtaPuentePagos = False
                Else
                    mParaAgrupaCtaPuentePagos = True
                End If


            End If
                Me.DbLeeCentral.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Exclamation, "Carga de Parámetros en Constructor de la Clase")
        End Try
    End Sub
    Private Sub BorraRegistros()
        Try
            SQL = "SELECT COUNT(*) FROM TP_ASNT WHERE ASNT_F_ATOCAB = '" & Me.mFecha & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND (ASNT_EMP_NUM = " & Me.mEmpNum & " OR ASNT_EMP_NUM IS NULL )"
            If CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Integer) > 0 Then
                MsgBox("Ya existen Movimientos de Integración para esta Fecha", MsgBoxStyle.Information, "Atención")
            End If
            SQL = "DELETE TP_ASNT WHERE ASNT_F_ATOCAB = '" & Me.mFecha & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM= " & Me.mEmpNum
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
                                        ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, vFechaValor As Date, vOperacion As String, vDatosAnulacion As String, vPagoCodi As String)

        Try

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TP_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,"
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_AUXILIAR_STRING2,ASNT_FORE_CODI,ASNT_EMP_NUM) values ('"
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
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','" & vOperacion & "','" & vDatosAnulacion & "','" & vPagoCodi & "'," & Me.mEmpNum & ")"




            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  "
            Me.mTextDebug.Update()

            If vCfcta_Cod.Length < 2 And vCfcta_Cod <> "NO TRATAR" Then
                Me.mTexto = "NEWPAGA: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40)
                Me.mListBoxDebug.Items.Add(Me.mTexto)
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & vAsiento & "," & Linea & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)
            End If



        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    Private Sub SpyroCompruebaCuentas()
        Try
            SQL = "SELECT DISTINCT ASNT_CFCTA_COD,ASNT_TIPO_REGISTRO,ASNT_CFCPTOS_COD FROM TP_ASNT WHERE "
            SQL += "     ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum
            SQL += " AND ASNT_F_ATOCAB = '" & Me.mFecha & "'"
            '  SQL += " AND ASNT_F_VALOR = '" & Me.mFecha & "'"
            SQL += " AND ASNT_CFCTA_COD <> 'NO TRATAR'"

            Me.DbLeeCentral.TraerLector(SQL)
            While Me.DbLeeCentral.mDbLector.Read
                Me.SpyroCompruebaCuentaSimple(CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_CFCTA_COD")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_TIPO_REGISTRO")),
                                        CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_CFCPTOS_COD")))


            End While
            Me.DbLeeCentral.mDbLector.Close()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub SpyroCompruebaCuentaSimple(ByVal vCuenta As String, ByVal vTipo As String, ByVal vDebeHaber As String)

        Try

            Me.mTextDebug.Text = "Validando Plan de Cuentas Spyro " & vCuenta.PadRight(20, CChar(" ")) & " Longitud : " & vCuenta.Length
            Me.mTextDebug.Update()


            SQL = "SELECT COD FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND COD = '" & vCuenta & "'"



            Me.mResult = Me.DbSpyro.EjecutaSqlScalar(SQL)

            If Me.mResult = "" Or IsNothing(Me.mResult) = True Then
                Me.mTexto = "SPYRO   : " & vCuenta & " no se localiza en Plan de Cuentas de Spyro"
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & 99 & "," & 1 & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)
                Exit Sub
            End If


            SQL = "SELECT APTESDIR_SN FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND COD = '" & vCuenta & "'"



            If Me.DbSpyro.EjecutaSqlScalar(SQL) <> "S" Then
                Me.mTexto = "SPYRO   : " & vCuenta & " No es una Cuenta de Apuntes Directos en Plan de Cuentas Spyro"
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & 99 & "," & 1 & ",'" & Me.mTexto & "')"
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
                    Me.mTexto = "SPYRO   : " & vCuenta & " No tiene definida Razón Social  en Plan de Cuentas de Spyro"
                    SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & 99 & "," & 1 & ",'" & Me.mTexto & "')"
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
                    Me.mTexto = "SPYRO   : " & vCuenta & " No tiene definido Libro de Iva   en Plan de Cuentas de Spyro"
                    SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & 99 & "," & 1 & ",'" & Me.mTexto & "')"
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
                    Me.mTexto = "SPYRO   : " & vCuenta & " No tiene definido Clase de Iva   en Plan de Cuentas de Spyro"
                    SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & 99 & "," & 1 & ",'" & Me.mTexto & "')"
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
                    Me.mTexto = "SPYRO   : " & vCuenta & " No tiene definido Forma de pago en Plan de Cuentas de Spyro"
                    SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                    SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & 99 & "," & 1 & ",'" & Me.mTexto & "')"
                    Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)
                    Exit Sub
                End If

            End If



        Catch ex As OleDb.OleDbException
            MsgBox(ex.Message, MsgBoxStyle.Information, " Localiza Cuenta Contable SPYRO")
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
    Private Sub GestionIncidencia(ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vEmpNum As Integer, ByVal vDescripcion As String)

        Try

            SQL = "INSERT INTO TH_INCI (INCI_DATR,INCI_EMPGRUPO_COD,INCI_EMP_COD,INCI_EMP_NUM,INCI_ORIGEN,INCI_DESCRIPCION) "
            SQL += " VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "','" & Me.mEmpGrupoCod & "','" & Me.mEmpCod & "'," & Me.mEmpNum & ",'NEWSTOCK COMPRAS','" & vDescripcion & "')"

            Me.DbGrabaCentral.IniciaTransaccion()

            Me.DbGrabaCentral.EjecutaSql(SQL)

            Me.DbGrabaCentral.ConfirmaTransaccion()

            Me.mListBoxDebug.Items.Add(vDescripcion)
            Me.mListBoxDebug.Update()

        Catch ex As Exception
            Me.DbGrabaCentral.CancelaTransaccion()
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
            Me.Filegraba.WriteLine(MyCharToOem(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Mid(FechaAsiento, 5, 4) &
            "P".PadRight(4, CChar(" ")) &
            " ".PadLeft(8, CChar(" ")) &
            " ".PadLeft(4, CChar(" ")) &
            vCfcta_Cod.PadRight(15, CChar(" ")) &
            vCfcptos_Cod.PadRight(4, CChar(" ")) &
            Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) &
            CType(vImonep, String).PadLeft(16, CChar(" ")) &
            "N" & FechaAsiento &
            Format(Me.mFecha, "ddMMyyyy") &
            " ".PadRight(40, CChar(" ")) &
            "NEWP".PadRight(4, CChar(" "))))

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
            Me.DbNewPaga.CerrarConexion()
            Me.DbNewPagaAux.CerrarConexion()
            Me.DbSpyro.CerrarConexion()
            Me.DbNewStock.CerrarConexion()
        Catch ex As Exception

        End Try

    End Sub

    Private Function BuscaCuentaProveedorCuentasPorPagar(ByVal vProveedor As Integer) As String
        Try
            SQL = "SELECT NVL(FORN_CEX2,'?') "
            SQL += " FROM TNST_FORN "
            SQL += " WHERE FORN_CODI = " & vProveedor

            If IsNothing(Me.DbNewStock.EjecutaSqlScalar(SQL)) = False Then
                Return Me.DbNewStock.EjecutaSqlScalar(SQL)
            Else
                Return "?"
            End If
        Catch ex As Exception
            Return "?"
        End Try
    End Function
    Private Function BuscaCuentaProveedorCuenta(ByVal vProveedor As Integer) As String
        Try
            SQL = "SELECT NVL(FORN_CEX1,'?') "
            SQL += " FROM TNST_FORN "
            SQL += " WHERE FORN_CODI = " & vProveedor

            If IsNothing(Me.DbNewStock.EjecutaSqlScalar(SQL)) = False Then
                Return Me.DbNewStock.EjecutaSqlScalar(SQL)
            Else
                Return "?"
            End If
        Catch ex As Exception
            Return "?"
        End Try
    End Function
    Private Function BuscaCuentaBanco(ByVal vTipo As String) As String
        Try
            SQL = "SELECT  TIMO_CEXT  "
            SQL += " FROM TNPG_TIMO "
            SQL += " WHERE TIMO_CODI = '" & vTipo & "'"

            If Me.DbNewPaga.EjecutaSqlScalar(SQL).ToString.Length > 0 Then
                Return Me.DbNewPaga.EjecutaSqlScalar(SQL)
            Else
                Return "?"
            End If


        Catch ex As Exception
            Return "?"
        End Try
    End Function


#Region "ASIENTOS NEWPAGA"
#Region "ASIENTO 1 PAGOS REALIZADOS"
    Private Sub PagosRealizados(vAnulados As Integer)
        Try
            Dim Total As Double
            Dim Texto As String = ""
            Dim Anulado As String = ""

            Dim Cuenta As String

            If vAnulados = 0 Then
                Linea = 0
            End If

            If Me.mParaAgrupaCtaPuentePagos = True Then
                ' DEL BANCO 
                SQL = "SELECT SUM(TOTAL)  AS TOTAL , "
                SQL += "MOVI_DAVA, "
                SQL += "TIMO_CODI, "
                SQL += "TIMO_FORM, "
                SQL += "  MOVI_DAAC, "
                '     SQL += "DECODE(MOVI_DAAC,NULL,NVL(TIMO_DESC,' '),NVL(TIMO_DESC,' ') || ' ANULADOS') AS MOVI_DESC, "
                SQL += " MOVI_DESC, "
                '   SQL += "'0' AS MOVI_DOCU "
                SQL += " MOVI_DOCU "
                SQL += ",NVL(TIMO_DESC,' ') AS TIMO_DESC, "
                SQL += "'0' AS FORN_CODI, "
                '     SQL += "' ' AS FORN_DESC,'0' AS MOVI_CODI "
                SQL += "' ' AS FORN_DESC, MOVI_CODI "
                SQL += "FROM ( "
                SQL += "SELECT "
                SQL += "  MOVI_IMPO  AS TOTAL , "
                SQL += "MOVI_DAVA,TNPG_MOVI.TIMO_CODI, "
                SQL += "TIMO_FORM, "
                SQL += "MOVI_DAAC, "
                '  SQL += "DECODE(MOVI_DAAC,NULL,NVL(TIMO_DESC,' '),NVL(TIMO_DESC,' ') || ' ANULADOS') AS MOVI_DESC, "
                SQL += " MOVI_DESC, "
                '  SQL += "'0' AS MOVI_DOCU "
                SQL += "MOVI_DOCU "
                SQL += ",NVL(TIMO_DESC,' ') AS TIMO_DESC, "
                SQL += "'0' AS FORN_CODI, "
                '   SQL += "' ' AS FORN_DESC,'0' AS MOVI_CODI "
                SQL += "' ' AS FORN_DESC, MOVI_CODI "
                SQL += "FROM "
                SQL += "    TNPG_MOVI, "
                SQL += "    TNPG_TIMO, "
                SQL += "    VNPG_FORN "

                If vAnulados = 0 Then
                    SQL += " WHERE TRUNC (MOVI_DAVA) = '" & Me.mFecha & "'"
                Else
                    SQL += " WHERE TRUNC (MOVI_DAAC) = '" & Me.mFecha & "'"
                End If

                ' Excluye anulados en mismo dia
                SQL += "    AND TO_DATE(MOVI_DAVA, 'DD/MM/YYYY') <> TO_DATE(NVL(MOVI_DAAC, '01/01/1901'), 'DD/MM/YYYY') "
                SQL += "    AND TNPG_MOVI.TIMO_CODI = TNPG_TIMO.TIMO_CODI "
                SQL += "    AND TNPG_MOVI.FORN_CODI = VNPG_FORN.FORN_CODI (+) "
                SQL += "    AND TIMO_EXCO = 1 "
                SQL += "    AND NOT ( ( MOVI_INTE = 1 ) "
                SQL += "              AND ( TIMO_FORM IN ( "
                SQL += "        0, "
                SQL += "        1, "
                SQL += "        2, "
                SQL += "        3 "
                SQL += "    ) ) ) "


                If Me.mParaNewPagaMovTrans.Length > 0 Then
                    SQL += "   AND TNPG_MOVI.TIMO_CODI NOT IN (" & Me.mParaNewPagaMovTrans & ")"
                End If


                SQL += ") "
                SQL += "GROUP BY   "
                SQL += "MOVI_DAVA, "

                ' NUEVO
                SQL += "MOVI_CODI, "

                SQL += "TIMO_CODI, "
                SQL += "TIMO_FORM ,"
                SQL += "MOVI_DAAC, "
                SQL += " MOVI_DESC, "
                SQL += " MOVI_DOCU "
                SQL += ", TIMO_DESC, "
                SQL += "FORN_CODI, "
                SQL += " FORN_DESC "

                SQL += " ORDER BY TIMO_CODI "


            Else
                ' DEL BANCO 
                SQL = "SELECT 'PAGOS',MOVI_CODI, TNPG_MOVI.TIMO_CODI, MOVI_INTE, TNPG_MOVI.FORN_CODI AS FORN_CODI, FORN_INTE, "
                SQL += "       MOVI_DOCU, MOVI_DAVA, MOVI_UNMO, MOVI_EMPR, MOVI_LLAV,  MOVI_IMPO  AS TOTAL, "
                SQL += "       MOVI_SALD, TIMO_FORM, NVL (MOVI_SERV, '') MOVI_SERV, MOVI_DEPT, "
                SQL += "       TIMO_CDIA, TIMO_CDOC, MOVI_TIIM, MOVI_DESC ,TIMO_DESC,MOVI_DAVA,MOVI_DAAC,NVL(TIMO_DESC,'?') AS TIMO_DESC "
                SQL += "      ,NVL(FORN_DESC,'?') AS FORN_DESC,NVL(ENTI_CODI,0) AS ENTI_CODI ,NVL(BACU_CNTA,'?') AS BACU_CNTA "
                SQL += "  FROM TNPG_MOVI, TNPG_TIMO,VNPG_FORN "

                If vAnulados = 0 Then
                    SQL += " WHERE TRUNC (MOVI_DAVA) = '" & Me.mFecha & "'"
                Else
                    SQL += " WHERE TRUNC (MOVI_DAAC) = '" & Me.mFecha & "'"
                End If


                ' Excluye anulados en mismo dia
                SQL += "  AND TO_DATE(MOVI_DAVA,'DD/MM/YYYY') <> TO_DATE(NVL(MOVI_DAAC,'01/01/1901'),'DD/MM/YYYY') "
                SQL += "  AND TNPG_MOVI.TIMO_CODI = TNPG_TIMO.TIMO_CODI "
                SQL += "  AND TNPG_MOVI.FORN_CODI = VNPG_FORN.FORN_CODI(+) "
                SQL += "  AND TIMO_EXCO = 1 "
                SQL += "  AND NOT ((MOVI_INTE = 1) AND (TIMO_FORM IN (0, 1, 2, 3))) "

                If Me.mParaNewPagaMovTrans.Length > 0 Then
                    SQL += "   AND TNPG_MOVI.TIMO_CODI NOT IN (" & Me.mParaNewPagaMovTrans & ")"
                End If

                SQL += " ORDER BY MOVI_CODI "
            End If



            Me.DbNewPaga.TraerLector(SQL)

            While Me.DbNewPaga.mDbLector.Read


                Linea = Linea + 1

                ' ANULADO O DEVOLUCION (TIMO_FORM = 1)
                If vAnulados = 0 Then
                    If CStr(Me.DbNewPaga.mDbLector("TIMO_FORM")) = "1" Then
                        TipoAsiento = 2
                        Total = CType(Me.DbNewPaga.mDbLector("TOTAL"), Double) * -1
                    Else
                        TipoAsiento = 1
                        Total = CType(Me.DbNewPaga.mDbLector("TOTAL"), Double)
                    End If
                Else
                    If CStr(Me.DbNewPaga.mDbLector("TIMO_FORM")) = "1" Then
                        TipoAsiento = 2
                        Total = CType(Me.DbNewPaga.mDbLector("TOTAL"), Double)
                    Else
                        TipoAsiento = 1
                        Total = CType(Me.DbNewPaga.mDbLector("TOTAL"), Double) * -1
                    End If
                End If




                If IsDBNull(Me.DbNewPaga.mDbLector("MOVI_DESC")) = False Then
                    Texto = CStr(Me.DbNewPaga.mDbLector("MOVI_DESC"))
                ElseIf IsDBNull(Me.DbNewPaga.mDbLector("MOVI_DESC")) = True Then
                    If IsDBNull(Me.DbNewPaga.mDbLector("MOVI_DOCU")) = False Then
                        Texto = "Pago Doc : " & CStr(Me.DbNewPaga.mDbLector("MOVI_DOCU"))
                    Else
                        Texto = "Pago S/N : " & CStr(Me.DbNewPaga.mDbLector("TIMO_DESC"))
                    End If
                End If

                If IsDBNull(Me.DbNewPaga.mDbLector("MOVI_DAAC")) = False Then
                    Anulado = Format(CDate((Me.DbNewPaga.mDbLector("MOVI_DAAC"))), "dd/MM/yyyy")
                Else
                    Anulado = ""
                End If

                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", TipoAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, BuscaCuentaBanco(CStr(Me.DbNewPaga.mDbLector("TIMO_CODI"))), Me.mIndicadorHaber, Texto, Total, "NO", "", CType(Me.DbNewPaga.mDbLector("FORN_CODI"), String) & " " & CType(Me.DbNewPaga.mDbLector("FORN_DESC"), String), "SI", CDate(Format(CDate(Me.DbNewPaga.mDbLector("MOVI_DAVA")), "dd/MM/yyyy")), "Comprobante Nº Movimiento : " & CStr(Me.DbNewPaga.mDbLector("TIMO_CODI")), Anulado, CStr(Me.DbNewPaga.mDbLector("TIMO_CODI")))
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), BuscaCuentaBanco(CStr(Me.DbNewPaga.mDbLector("TIMO_CODI"))), Me.mIndicadorHaber, Texto, Total)
                End If
            End While
            Me.DbNewPaga.mDbLector.Close()



            ' A DEPOSITOS ANTICIPADOS DEL PROVEEDOR
            If Me.mParaAgrupaCtaPuentePagos = True Then
                SQL = "SELECT "
                SQL += "    'PAGOS', "
                SQL += "    '0' AS FORN_CODI, "
                SQL += "    MOVI_DOCU, "
                SQL += "    SUM(TOTAL) AS TOTAL, "
                SQL += "    TIMO_FORM, "
                SQL += "    TIMO_DESC AS MOVI_DESC , "
                SQL += "     TIMO_DESC, "
                SQL += "    MOVI_DAAC, "
                SQL += "    ' ' AS FORN_DESC, "
                SQL += "    MOVI_DAVA, "
                SQL += "    TIMO_CODI AS MOVI_CODI, "
                SQL += "     TIMO_CODI "
                SQL += " FROM "
                SQL += "    ( "
                SQL += "        SELECT "
                SQL += "            'PAGOS', "
                SQL += "            '0' AS FORN_CODI, "
                SQL += "            '' AS MOVI_DOCU, "
                SQL += "            MOVI_IMPO   AS TOTAL, "
                SQL += "            TIMO_FORM, "
                SQL += "            '*PAGOS AGRUPADOS INTERFAZ*' AS MOVI_DESC, "
                SQL += "             NVL(TIMO_DESC,'?') AS TIMO_DESC, "
                SQL += "            NULL AS MOVI_DAAC, "
                SQL += "            ' ' AS FORN_DESC, "
                SQL += "            MOVI_DAVA, "
                SQL += "            '0' AS MOVI_CODI, "
                SQL += "            TNPG_MOVI.TIMO_CODI "
                SQL += "        FROM "
                SQL += "            TNPG_MOVI, "
                SQL += "            TNPG_TIMO, "
                SQL += "            VNPG_FORN "

                If vAnulados = 0 Then
                    SQL += " WHERE TRUNC (MOVI_DAVA) = '" & Me.mFecha & "'"
                Else
                    SQL += " WHERE TRUNC (MOVI_DAAC) = '" & Me.mFecha & "'"
                End If

                ' Excluye anulados en mismo dia
                SQL += "    AND TO_DATE(MOVI_DAVA, 'DD/MM/YYYY') <> TO_DATE(NVL(MOVI_DAAC, '01/01/1901'), 'DD/MM/YYYY') "
                SQL += "    AND TNPG_MOVI.TIMO_CODI = TNPG_TIMO.TIMO_CODI "
                SQL += "    AND TNPG_MOVI.FORN_CODI = VNPG_FORN.FORN_CODI (+) "
                SQL += "    AND TIMO_EXCO = 1 "
                SQL += "    AND NOT ( ( MOVI_INTE = 1 ) "
                SQL += "              AND ( TIMO_FORM IN ( "
                SQL += "        0, "
                SQL += "        1, "
                SQL += "        2, "
                SQL += "        3 "
                SQL += "    ) ) ) "


                If Me.mParaNewPagaMovTrans.Length > 0 Then
                    SQL += "   AND TNPG_MOVI.TIMO_CODI NOT IN (" & Me.mParaNewPagaMovTrans & ")"
                End If


                SQL += ") "
                SQL += "GROUP BY "
                SQL += "    MOVI_DAVA, "
                SQL += "    FORN_CODI, "
                SQL += "    MOVI_DOCU, "
                SQL += "    TIMO_FORM, "
                SQL += "    MOVI_DESC, "
                SQL += "    TIMO_DESC, "
                SQL += "    MOVI_DAAC, "
                SQL += "    TIMO_DESC, "
                SQL += "    TIMO_CODI "



            Else

                SQL = "SELECT 'PAGOS',MOVI_CODI, TNPG_MOVI.TIMO_CODI, MOVI_INTE, NVL(TNPG_MOVI.FORN_CODI,'0') AS FORN_CODI, FORN_INTE, "
                SQL += "       MOVI_DOCU, MOVI_DAVA, MOVI_UNMO, MOVI_EMPR, MOVI_LLAV,  MOVI_IMPO AS TOTAL, "
                SQL += "       MOVI_SALD, TIMO_FORM, NVL (MOVI_SERV, '') MOVI_SERV, MOVI_DEPT, "
                SQL += "       TIMO_CDIA, TIMO_CDOC, MOVI_TIIM, MOVI_DESC,TIMO_DESC ,MOVI_DAVA,MOVI_DAAC,NVL(TIMO_DESC,'?') AS TIMO_DESC "
                SQL += "       ,'DEPOSITOS PROV' AS CUENTA ,NVL(FORN_DESC,'?') AS FORN_DESC"
                SQL += "  FROM TNPG_MOVI, TNPG_TIMO,VNPG_FORN "

                If vAnulados = 0 Then
                    SQL += " WHERE TRUNC (MOVI_DAVA) = '" & Me.mFecha & "'"
                Else
                    SQL += " WHERE TRUNC (MOVI_DAAC) = '" & Me.mFecha & "'"
                End If

                SQL += "  AND TO_DATE(MOVI_DAVA,'DD/MM/YYYY') <> TO_DATE(NVL(MOVI_DAAC,'01/01/1901'),'DD/MM/YYYY') "
                SQL += "  AND TNPG_MOVI.TIMO_CODI = TNPG_TIMO.TIMO_CODI "
                SQL += "  AND TNPG_MOVI.FORN_CODI = VNPG_FORN.FORN_CODI(+) "
                SQL += "  AND TIMO_EXCO = 1 "
                SQL += "  AND NOT ((MOVI_INTE = 1) AND (TIMO_FORM IN (0, 1, 2, 3))) "

                If Me.mParaNewPagaMovTrans.Length > 0 Then
                    SQL += "   AND TNPG_MOVI.TIMO_CODI NOT IN (" & Me.mParaNewPagaMovTrans & ")"
                End If
                SQL += " ORDER BY MOVI_CODI "


            End If



            Me.DbNewPaga.TraerLector(SQL)

            While Me.DbNewPaga.mDbLector.Read

                Linea = Linea + 1


                ' ANULADO O DEVOLUCION (TIMO_FORM = 1)
                If vAnulados = 0 Then
                    If CStr(Me.DbNewPaga.mDbLector("TIMO_FORM")) = "1" Then
                        TipoAsiento = 2
                        Total = CType(Me.DbNewPaga.mDbLector("TOTAL"), Double) * -1
                    Else
                        TipoAsiento = 1
                        Total = CType(Me.DbNewPaga.mDbLector("TOTAL"), Double)
                    End If
                Else
                    If CStr(Me.DbNewPaga.mDbLector("TIMO_FORM")) = "1" Then
                        TipoAsiento = 2
                        Total = CType(Me.DbNewPaga.mDbLector("TOTAL"), Double)
                    Else
                        TipoAsiento = 1
                        Total = CType(Me.DbNewPaga.mDbLector("TOTAL"), Double) * -1
                    End If
                End If

                If IsDBNull(Me.DbNewPaga.mDbLector("MOVI_DESC")) = False Then
                    Texto = CStr(Me.DbNewPaga.mDbLector("MOVI_DESC"))
                ElseIf IsDBNull(Me.DbNewPaga.mDbLector("MOVI_DESC")) = True Then
                    If IsDBNull(Me.DbNewPaga.mDbLector("MOVI_DOCU")) = False Then
                        Texto = "Pago Doc : " & CStr(Me.DbNewPaga.mDbLector("MOVI_DOCU"))
                    Else
                        Texto = "Pago S/N : " & CStr(Me.DbNewPaga.mDbLector("TIMO_DESC"))
                    End If
                End If

                If IsDBNull(Me.DbNewPaga.mDbLector("MOVI_DAAC")) = False Then
                    Anulado = Format(CDate((Me.DbNewPaga.mDbLector("MOVI_DAAC"))), "dd/MM/yyyy")
                Else
                    Anulado = ""
                End If

                If mParaUsaCtaPuentePagos = False Then
                    Cuenta = BuscaCuentaProveedorCuentasPorPagar(CInt(Me.DbNewPaga.mDbLector("FORN_CODI")))
                Else
                    Cuenta = Me.mParaCtaPuentePagos
                End If





                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", TipoAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, Texto, Total, "NO", "", CType(Me.DbNewPaga.mDbLector("FORN_CODI"), String) & " " & CType(Me.DbNewPaga.mDbLector("FORN_DESC"), String), "SI", CDate(Format(CDate(Me.DbNewPaga.mDbLector("MOVI_DAVA")), "dd/MM/yyyy")), "Comprobante Nº Movimiento : " & CStr(Me.DbNewPaga.mDbLector("MOVI_CODI")), Anulado, CStr(Me.DbNewPaga.mDbLector("TIMO_CODI")))
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, Texto, Total)
                End If
            End While
            Me.DbNewPaga.mDbLector.Close()



        Catch EX As Exception
            MsgBox(EX.Message)
        End Try


    End Sub

#End Region
#Region "ASIENTO 3 FACTURAS REGULARIZADAS"
    Private Sub FacturasRegularizadas(vAnulados As Integer)
        Try
            Dim Total As Double
            Dim Texto As String
            Dim Anulado As String = ""

            If vAnulados = 0 Then
                Linea = 0
            End If


            Dim Cuenta As String = ""

            Dim Aux() As String
            Dim EsUnaTransferencia As Boolean = False


            If Me.mParaAgrupaCtaPuentePagos = False Then
                ' DEL DEPOSITO PROVEEDOR
                SQL = "SELECT "
                SQL += "    'DOCUMENTOS A PAGAR' AS TIPO, ASOC_DAVA,"
                SQL += "    MOVI_DEBI.MOVI_CODI   AS ORDEN, "
                SQL += "    MOVI_DEBI.TIMO_CODI   AS TIMO_DEBI, "
                SQL += "    NVL(MOVI_DEBI.MOVI_DOCU,'S/N')   AS DEBI_DOCU, "
                SQL += "    SUM(ASOC_IMPO)             AS TOTAL, "
                SQL += "    MOVI_CRED.FORN_CODI   AS FORN_CODI, "
                SQL += "    NVL(MOVI_CRED.MOVI_DOCU, '?') AS MOVI_DOCU, "
                '   SQL += "    '0' AS MOVI_DOCU, "
                SQL += "    NVL(FORN_DESC, '?') AS FORN_DESC ,ASOC_CODA,TIMO_DEBI.TIMO_FORM "
                SQL += " FROM "
                SQL += "    TNPG_MOVI MOVI_CRED, "
                SQL += "    TNPG_MOVI MOVI_DEBI, "
                SQL += "    TNPG_TIMO, "
                SQL += "    TNPG_TIMO TIMO_DEBI, "
                SQL += "    VNPG_FORN, "
                SQL += "    TNPG_ASOC "
                SQL += " WHERE "
                SQL += "    MOVI_CRED.TIMO_CODI = TNPG_TIMO.TIMO_CODI "
                SQL += "    AND MOVI_CRED.FORN_CODI = VNPG_FORN.FORN_CODI "
                SQL += "    AND MOVI_CRED.FORN_INTE = VNPG_FORN.INTERNO "
                SQL += "    AND MOVI_CRED.MOVI_CODI = TNPG_ASOC.ASOC_COD1 "
                SQL += "    AND MOVI_DEBI.MOVI_CODI = TNPG_ASOC.ASOC_CODI "
                SQL += "    AND MOVI_DEBI.TIMO_CODI = TIMO_DEBI.TIMO_CODI "
                SQL += "    AND MOVI_CRED.MOVI_UNMO = 'EUR' "
                If vAnulados = 0 Then
                    SQL += " AND  TRUNC (ASOC_DAVA) = '" & Me.mFecha & "'"
                Else
                    SQL += " AND  TRUNC (ASOC_DAAC) = '" & Me.mFecha & "'"
                End If
                SQL += "    AND TO_DATE(ASOC_DAVA, 'DD/MM/YYYY') <> TO_DATE(NVL(ASOC_DAAC, '01/01/1901'), 'DD/MM/YYYY') "
                SQL += "    AND TNPG_TIMO.TIMO_DECR = '1' "
                If Me.mParaNewPagaMovTrans.Length > 0 Then
                    SQL += "   AND MOVI_DEBI.TIMO_CODI  IN (" & Me.mParaNewPagaMovTrans & ")"
                Else
                    SQL += "   AND MOVI_DEBI.TIMO_CODI  IN (" & "'~~~'" & ")"
                End If

                SQL += "  AND TIMO_DEBI.TIMO_EXCO = 1 "
                SQL += "  AND NOT ((MOVI_DEBI.MOVI_INTE = 1) AND (TIMO_DEBI.TIMO_FORM IN (0, 1, 2, 3))) "
                SQL += " GROUP BY  ASOC_DAVA,MOVI_DEBI.MOVI_CODI,MOVI_DEBI.TIMO_CODI,MOVI_CRED.FORN_CODI,MOVI_DEBI.MOVI_DOCU,MOVI_CRED.MOVI_DOCU,FORN_DESC,ASOC_CODA,TIMO_DEBI.TIMO_FORM "

                SQL += " UNION ALL "
                SQL += "SELECT "
                SQL += "     'DOCUMENTOS A PAGAR' AS TIPO, ASOC_DAVA,"
                SQL += "    MOVI_DEBI.MOVI_CODI   AS ORDEN, "
                SQL += "    MOVI_DEBI.TIMO_CODI   AS TIMO_DEBI, "
                SQL += "    NVL(MOVI_DEBI.MOVI_DOCU,'S/N')   AS DEBI_DOCU, "
                SQL += "    SUM(ASOC_IMPO)             AS TOTAL, "
                SQL += "    MOVI_DEBI.FORN_CODI, "
                SQL += "    NVL(MOVI_CRED.MOVI_DOCU, '?') AS MOVI_DOCU, "
                SQL += "     NVL(FORN_DESC,'?') AS FORN_DESC,  ASOC_CODA,TIMO_DEBI.TIMO_FORM "
                SQL += " FROM "
                SQL += "    TNPG_MOVI MOVI_CRED, "
                SQL += "    TNPG_MOVI MOVI_DEBI, "
                SQL += "    TNPG_TIMO, "
                SQL += "    TNPG_TIMO TIMO_DEBI, "
                SQL += "    VNPG_FORN, "
                SQL += "    TNPG_ASOC "
                SQL += " WHERE "
                SQL += "    MOVI_CRED.TIMO_CODI = TNPG_TIMO.TIMO_CODI "
                SQL += "    AND MOVI_CRED.FORN_CODI = VNPG_FORN.FORN_CODI "
                SQL += "    AND MOVI_CRED.FORN_INTE = VNPG_FORN.INTERNO "
                SQL += "    AND MOVI_CRED.MOVI_CODI = TNPG_ASOC.ASOC_COD1 "
                SQL += "    AND MOVI_DEBI.MOVI_CODI = TNPG_ASOC.ASOC_CODI "
                SQL += "    AND MOVI_DEBI.TIMO_CODI = TIMO_DEBI.TIMO_CODI "
                SQL += "    AND MOVI_CRED.MOVI_UNMO = 'EUR' "
                If vAnulados = 0 Then
                    SQL += " AND  TRUNC (ASOC_DAVA) = '" & Me.mFecha & "'"
                Else
                    SQL += " AND  TRUNC (ASOC_DAAC) = '" & Me.mFecha & "'"
                End If
                SQL += "    AND TO_DATE(ASOC_DAVA, 'DD/MM/YYYY') <> TO_DATE(NVL(ASOC_DAAC, '01/01/1901'), 'DD/MM/YYYY') "
                SQL += "    AND TNPG_TIMO.TIMO_DECR = '1' "
                If Me.mParaNewPagaMovTrans.Length > 0 Then
                    SQL += "   AND MOVI_DEBI.TIMO_CODI NOT  IN (" & Me.mParaNewPagaMovTrans & ")"
                End If
                SQL += "  AND TIMO_DEBI.TIMO_EXCO = 1 "
                SQL += "  AND NOT ((MOVI_DEBI.MOVI_INTE = 1) AND (TIMO_DEBI.TIMO_FORM IN (0, 1, 2, 3))) "
                SQL += " GROUP BY ASOC_DAVA,MOVI_DEBI.MOVI_CODI, MOVI_DEBI.TIMO_CODI, MOVI_DEBI.MOVI_DOCU,TIMO_DEBI.TIMO_FORM,ASOC_CODA,MOVI_DEBI.FORN_CODI,FORN_DESC,MOVI_CRED.MOVI_DOCU "
                SQL += " ORDER BY "
                SQL += "    ORDEN "

            Else
                SQL = "SELECT "
                SQL += "    'DOCUMENTOS A PAGAR TRANS ' AS TIPO, "
                SQL += "    ASOC_DAVA, "
                SQL += "    MOVI_DEBI.MOVI_CODI   AS ORDEN, "
                SQL += "    MOVI_DEBI.TIMO_CODI   AS TIMO_DEBI, "
                SQL += "                                            NVL(MOVI_DEBI.MOVI_DOCU, 'S/N') AS DEBI_DOCU, "
                SQL += "    SUM(ASOC_IMPO) AS TOTAL, "
                SQL += "    0 AS FORN_CODI, "
                SQL += "    '0' AS MOVI_DOCU, "
                SQL += "    '* PROVEEDORES AGRUPADOS INTERFAZ *' AS FORN_DESC, "
                SQL += "                    0 AS ASOC_CODA, "
                SQL += "    TIMO_DEBI.TIMO_FORM "
                SQL += "FROM "
                SQL += "    TNPG_MOVI MOVI_CRED, "
                SQL += "    TNPG_MOVI MOVI_DEBI, "
                SQL += "    TNPG_TIMO, "
                SQL += "    TNPG_TIMO TIMO_DEBI, "
                SQL += "    VNPG_FORN, "
                SQL += "    TNPG_ASOC "
                SQL += "WHERE "
                SQL += "    MOVI_CRED.TIMO_CODI = TNPG_TIMO.TIMO_CODI "
                SQL += "    AND MOVI_CRED.FORN_CODI = VNPG_FORN.FORN_CODI "
                SQL += "    AND MOVI_CRED.FORN_INTE = VNPG_FORN.INTERNO "
                SQL += "    AND MOVI_CRED.MOVI_CODI = TNPG_ASOC.ASOC_COD1 "
                SQL += "    AND MOVI_DEBI.MOVI_CODI = TNPG_ASOC.ASOC_CODI "
                SQL += "    AND MOVI_DEBI.TIMO_CODI = TIMO_DEBI.TIMO_CODI "
                SQL += "    AND MOVI_CRED.MOVI_UNMO = 'EUR' "
                If vAnulados = 0 Then
                    SQL += " AND  TRUNC (ASOC_DAVA) = '" & Me.mFecha & "'"
                Else
                    SQL += " AND  TRUNC (ASOC_DAAC) = '" & Me.mFecha & "'"
                End If
                SQL += "    AND TO_DATE(ASOC_DAVA, 'DD/MM/YYYY') <> TO_DATE(NVL(ASOC_DAAC, '01/01/1901'), 'DD/MM/YYYY') "
                SQL += "    AND TNPG_TIMO.TIMO_DECR = '1' "
                If Me.mParaNewPagaMovTrans.Length > 0 Then
                    SQL += "   AND MOVI_DEBI.TIMO_CODI  IN (" & Me.mParaNewPagaMovTrans & ")"
                Else
                    SQL += "   AND MOVI_DEBI.TIMO_CODI  IN (" & "'~~~'" & ")"
                End If
                SQL += "        AND TIMO_DEBI.TIMO_EXCO = 1 "
                SQL += "    AND NOT ( ( MOVI_DEBI.MOVI_INTE = 1 ) "
                SQL += "              AND ( TIMO_DEBI.TIMO_FORM IN ( "
                SQL += "        0, "
                SQL += "        1, "
                SQL += "        2, "
                SQL += "        3 "
                SQL += "        ) ) ) "
                SQL += "    GROUP BY "
                SQL += "        ASOC_DAVA, "
                SQL += "        MOVI_DEBI.MOVI_CODI, "
                SQL += "        MOVI_DEBI.TIMO_CODI, "
                SQL += "        MOVI_DEBI.MOVI_DOCU, "
                SQL += "        TIMO_DEBI.TIMO_FORM "
                SQL += "         "
                SQL += "UNION ALL "
                SQL += "SELECT "
                SQL += "    'DOCUMENTOS A PAGAR RESTO ' AS TIPO, "
                SQL += "    ASOC_DAVA, "
                SQL += "     ORDEN, "
                SQL += "     TIMO_DEBI, "
                SQL += "    DEBI_DOCU, "
                SQL += "    SUM(TOTAL) AS TOTAL, "
                SQL += "     FORN_CODI, "
                SQL += "     MOVI_DOCU, "
                SQL += "    '* PROVEEDORES AGRUPADOS INTERFAZ *' AS FORN_DESC, "
                SQL += "                     ASOC_CODA, "
                SQL += "    TIMO_FORM  FROM ( "
                SQL += "SELECT "
                SQL += "    'DOCUMENTOS A PAGAR RESTO ' AS TIPO, "
                SQL += "    ASOC_DAVA, "
                SQL += "    0   AS ORDEN, "
                SQL += "    MOVI_DEBI.TIMO_CODI   AS TIMO_DEBI, "
                SQL += "   '' AS DEBI_DOCU, "
                SQL += "    SUM(ASOC_IMPO) AS TOTAL, "
                SQL += "    0 AS FORN_CODI, "
                SQL += "    '0' AS MOVI_DOCU, "
                SQL += "    '* PROVEEDORES AGRUPADOS INTERFAZ *' AS FORN_DESC, "
                SQL += "                    0 AS ASOC_CODA, "
                SQL += "    TIMO_DEBI.TIMO_FORM "
                SQL += "FROM "
                SQL += "    TNPG_MOVI MOVI_CRED, "
                SQL += "    TNPG_MOVI MOVI_DEBI, "
                SQL += "    TNPG_TIMO, "
                SQL += "    TNPG_TIMO TIMO_DEBI, "
                SQL += "    VNPG_FORN, "
                SQL += "    TNPG_ASOC "
                SQL += "WHERE "
                SQL += "    MOVI_CRED.TIMO_CODI = TNPG_TIMO.TIMO_CODI "
                SQL += "    AND MOVI_CRED.FORN_CODI = VNPG_FORN.FORN_CODI "
                SQL += "    AND MOVI_CRED.FORN_INTE = VNPG_FORN.INTERNO "
                SQL += "    AND MOVI_CRED.MOVI_CODI = TNPG_ASOC.ASOC_COD1 "
                SQL += "    AND MOVI_DEBI.MOVI_CODI = TNPG_ASOC.ASOC_CODI "
                SQL += "    AND MOVI_DEBI.TIMO_CODI = TIMO_DEBI.TIMO_CODI "
                SQL += "    AND MOVI_CRED.MOVI_UNMO = 'EUR' "
                If vAnulados = 0 Then
                    SQL += " AND  TRUNC (ASOC_DAVA) = '" & Me.mFecha & "'"
                Else
                    SQL += " AND  TRUNC (ASOC_DAAC) = '" & Me.mFecha & "'"
                End If
                SQL += "    AND TO_DATE(ASOC_DAVA, 'DD/MM/YYYY') <> TO_DATE(NVL(ASOC_DAAC, '01/01/1901'), 'DD/MM/YYYY') "
                SQL += "    AND TNPG_TIMO.TIMO_DECR = '1' "

                If Me.mParaNewPagaMovTrans.Length > 0 Then
                    SQL += "   AND MOVI_DEBI.TIMO_CODI NOT IN (" & Me.mParaNewPagaMovTrans & ")"
                End If
                SQL += "        AND TIMO_DEBI.TIMO_EXCO = 1 "
                SQL += "    AND NOT ( ( MOVI_DEBI.MOVI_INTE = 1 ) "
                SQL += "              AND ( TIMO_DEBI.TIMO_FORM IN ( "
                SQL += "        0, "
                SQL += "        1, "
                SQL += "        2, "
                SQL += "        3 "
                SQL += "        ) ) ) "
                SQL += "    GROUP BY "
                SQL += "        ASOC_DAVA, "
                SQL += "        MOVI_DEBI.MOVI_CODI, "
                SQL += "        MOVI_DEBI.TIMO_CODI, "
                SQL += "        MOVI_DEBI.MOVI_DOCU, "
                SQL += "        TIMO_DEBI.TIMO_FORM "
                SQL += "         "
                SQL += "        ) "
                SQL += "         "
                SQL += "        GROUP  BY  "
                SQL += "         "
                SQL += "   ASOC_DAVA, "
                SQL += "        ORDEN, "
                SQL += "        TIMO_DEBI, "
                SQL += "        DEBI_DOCU, "
                SQL += "        FORN_CODI, "
                SQL += "        MOVI_DOCU, "
                SQL += "        ASOC_CODA, "
                SQL += "        TIMO_FORM         "

            End If




            Me.DbNewPaga.TraerLector(SQL)

            While Me.DbNewPaga.mDbLector.Read

                Linea = Linea + 1


                ' ANULADO O DEVOLUCION (TIMO_FORM = 1)
                If vAnulados = 0 Then
                    If CStr(Me.DbNewPaga.mDbLector("TIMO_FORM")) = "1" Then
                        TipoAsiento = 5
                        Total = CType(Me.DbNewPaga.mDbLector("TOTAL"), Double) * -1
                    Else
                        TipoAsiento = 3
                        Total = CType(Me.DbNewPaga.mDbLector("TOTAL"), Double)
                    End If
                Else
                    If CStr(Me.DbNewPaga.mDbLector("TIMO_FORM")) = "1" Then
                        TipoAsiento = 5
                        Total = CType(Me.DbNewPaga.mDbLector("TOTAL"), Double)
                    Else
                        TipoAsiento = 3
                        Total = CType(Me.DbNewPaga.mDbLector("TOTAL"), Double) * -1
                    End If
                End If


                Me.AuxTipoMovimiento = CStr(Me.DbNewPaga.mDbLector("TIMO_DEBI"))

                EsUnaTransferencia = False
                If Me.mParaNewPagaMovTrans.Length > 0 Then
                    Aux = Split(Me.mParaNewPagaMovTransAuxiliar, ",")
                    For I As Integer = 0 To Aux.Length - 1
                        If Aux(I) = Me.AuxTipoMovimiento Then
                            EsUnaTransferencia = True
                        End If
                    Next
                End If



                If EsUnaTransferencia Then
                    Cuenta = BuscaCuentaBanco(Me.AuxTipoMovimiento)
                    Texto = "Just. de Pago : " & CStr(Me.DbNewPaga.mDbLector("DEBI_DOCU"))
                    Me.mAuxStr = CType(Me.DbNewPaga.mDbLector("FORN_DESC"), String)
                Else

                    If mParaUsaCtaPuentePagos = False Then
                        Cuenta = BuscaCuentaProveedorCuentasPorPagar(CInt(Me.DbNewPaga.mDbLector("FORN_CODI")))
                    Else
                        Cuenta = Me.mParaCtaPuentePagos
                    End If

                    If Me.mParaAgrupaCtaPuentePagos = False Then
                        Texto = "Su / Factura : " & CStr(Me.DbNewPaga.mDbLector("MOVI_DOCU"))
                        Me.mAuxStr = CType(Me.DbNewPaga.mDbLector("FORN_CODI"), String) & " " & CType(Me.DbNewPaga.mDbLector("FORN_DESC"), String)
                    Else
                        SQL = "SELECT NVL(TIMO_DESC,'?') AS TIMO_DESC  FROM TNPG_TIMO WHERE TIMO_CODI = " & "'" & CStr(Me.DbNewPaga.mDbLector("TIMO_DEBI")) & "'"
                        Texto = "Pagos  : " & Me.DbNewPagaAux.EjecutaSqlScalar(SQL)
                        Me.mAuxStr = CType(Me.DbNewPaga.mDbLector("FORN_CODI"), String)
                    End If

                End If





                If Total <> 0 Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", TipoAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, Texto, Total, "NO", "", Me.mAuxStr, "SI", CDate(Format(CDate(Me.DbNewPaga.mDbLector("ASOC_DAVA")), "dd/MM/yyyy")), "Comprobante Nº Movimiento : " & CStr(Me.DbNewPaga.mDbLector("ORDEN")), "", CStr(Me.DbNewPaga.mDbLector("TIMO_DEBI")))
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaber, Texto, Total)

                    ' Sale al Asiento de Contrapartida(Dettale de Facturas Pagadas)  de este Pago
                    If EsUnaTransferencia Then
                        Me.FacturasRegularizadasDocumentos(vAnulados, CInt(Me.DbNewPaga.mDbLector("ORDEN")), CInt(Me.DbNewPaga.mDbLector("ASOC_CODA")), "")
                    Else
                        Me.FacturasRegularizadasDocumentos(vAnulados, CInt(Me.DbNewPaga.mDbLector("ORDEN")), CInt(Me.DbNewPaga.mDbLector("ASOC_CODA")), CStr(Me.DbNewPaga.mDbLector("TIMO_DEBI")))
                    End If



                End If
            End While
            Me.DbNewPaga.mDbLector.Close()




        Catch EX As Exception
            MsgBox(EX.Message)
        End Try


    End Sub
    Private Sub FacturasRegularizadasDocumentos(vAnulados As Integer, vMoviDebi As Integer, vAsocCoda As Integer, vTimoCodi As String)
        Try
            Dim Total As Double
            Dim Texto As String

            Dim Anulado As String = ""
            Dim Cuenta As String = ""

            ' A FACTURAS
            SQL = "SELECT 'PAGOS', TNPG_MOVI.FORN_CODI AS FORN_CODI,  "
            SQL += "       NVL(TNPG_MOVI.MOVI_DOCU,'?') AS MOVI_DOCU, ASOC_IMPO AS TOTAL, "
            SQL += "       NVL(FORN_DESC,'?') AS FORN_DESC,ASOC_DAVA,TIMO_DEBI.TIMO_FORM "

            SQL += "    FROM TNPG_MOVI, TNPG_MOVI MOVI_DEBI,TNPG_TIMO,TNPG_TIMO TIMO_DEBI,  VNPG_FORN,TNPG_ASOC "
            SQL += "     WHERE TNPG_MOVI.TIMO_CODI = TNPG_TIMO.TIMO_CODI "
            SQL += "     AND MOVI_DEBI.TIMO_CODI = TIMO_DEBI.TIMO_CODI "
            SQL += "     AND TNPG_MOVI.FORN_CODI = VNPG_FORN.FORN_CODI "
            SQL += "     AND TNPG_MOVI.FORN_INTE = VNPG_FORN.INTERNO "
            SQL += "     AND TNPG_MOVI.MOVI_CODI = TNPG_ASOC.ASOC_COD1 "
            SQL += "     AND MOVI_DEBI.MOVI_CODI = TNPG_ASOC.ASOC_CODI "
            SQL += "     AND TNPG_MOVI.MOVI_UNMO = 'EUR' "
            If vAnulados = 0 Then
                SQL += " AND  TRUNC (ASOC_DAVA) = '" & Me.mFecha & "'"
            Else
                SQL += " AND  TRUNC (ASOC_DAAC) = '" & Me.mFecha & "'"
            End If
            SQL += "   AND TO_DATE(ASOC_DAVA,'DD/MM/YYYY') <> TO_DATE(NVL(ASOC_DAAC,'01/01/1901'),'DD/MM/YYYY') "


            ' TIPO PAGO 
            SQL += "     AND TNPG_TIMO.TIMO_DECR = '1' "
            ' Mismo Criterio que la Rutina de llamada 
            SQL += "        AND TIMO_DEBI.TIMO_EXCO = 1 "
            SQL += "    AND NOT ( ( MOVI_DEBI.MOVI_INTE = 1 ) "
            SQL += "              AND ( TIMO_DEBI.TIMO_FORM IN ( "
            SQL += "        0, "
            SQL += "        1, "
            SQL += "        2, "
            SQL += "        3 "
            SQL += "        ) ) ) "

            If vAsocCoda <> 0 Then
                SQL += " AND  ASOC_CODA  = " & vAsocCoda
            End If

            If vTimoCodi.Length > 0 Then
                ' Se Buscan Todos los Documentos Regularizados Asociados a una  Transferencia Bancaria
                SQL += " AND  TIMO_DEBI.TIMO_CODI  = " & "'" & vTimoCodi & "'"
            Else
                ' Se Buscan Todos los Documentos Regularizados Asociados a una  Forma de Cobro 
                SQL += " AND  MOVI_DEBI.MOVI_CODI = " & vMoviDebi
            End If

            SQL += " ORDER BY MOVI_DEBI.MOVI_CODI "



            Me.DbNewPagaAux.TraerLector(SQL)

            While Me.DbNewPagaAux.mDbLector.Read

                Linea = Linea + 1


                ' ANULADO O DEVOLUCION (TIMO_FORM = 1)
                If vAnulados = 0 Then
                    If CStr(Me.DbNewPaga.mDbLector("TIMO_FORM")) = "1" Then
                        TipoAsiento = 5
                        Total = CType(Me.DbNewPagaAux.mDbLector("TOTAL"), Double) * -1
                    Else
                        TipoAsiento = 3
                        Total = CType(Me.DbNewPagaAux.mDbLector("TOTAL"), Double)
                    End If
                Else
                    If CStr(Me.DbNewPagaAux.mDbLector("TIMO_FORM")) = "1" Then
                        TipoAsiento = 5
                        Total = CType(Me.DbNewPagaAux.mDbLector("TOTAL"), Double)
                    Else
                        TipoAsiento = 3
                        Total = CType(Me.DbNewPagaAux.mDbLector("TOTAL"), Double) * -1
                    End If
                End If

                Texto = "Su / Factura : " & CStr(Me.DbNewPagaAux.mDbLector("MOVI_DOCU"))



                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", TipoAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, BuscaCuentaProveedorCuenta(CInt(Me.DbNewPagaAux.mDbLector("FORN_CODI"))), Me.mIndicadorDebe, Texto, Total, "NO", "", CType(Me.DbNewPagaAux.mDbLector("FORN_CODI"), String) & " " & CType(Me.DbNewPagaAux.mDbLector("FORN_DESC"), String), "SI", CDate(Format(CDate(Me.DbNewPagaAux.mDbLector("ASOC_DAVA")), "dd/MM/yyyy")), "Comprobante Nº Movimiento : " & CStr(vMoviDebi), "", "")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), BuscaCuentaProveedorCuenta(CInt(Me.DbNewPagaAux.mDbLector("FORN_CODI"))), Me.mIndicadorDebe, Texto, Total)
                End If
            End While
            Me.DbNewPagaAux.mDbLector.Close()




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
            If Me.DbNewPaga.EstadoConexion = ConnectionState.Open Then

                Me.mTextDebug.Text = "Calculando Pagos Realizados"
                Me.mTextDebug.Update()

                Me.PagosRealizados(mEnumEstaDo.NoAnulado)
                Me.PagosRealizados(mEnumEstaDo.SiAnulado)

                Me.mProgress.Value = 50
                Me.mProgress.Update()

                Me.mTextDebug.Text = "Calculando Facturas Regularizadas"
                Me.mTextDebug.Update()

                Me.FacturasRegularizadas(mEnumEstaDo.NoAnulado)
                Me.FacturasRegularizadas(mEnumEstaDo.SiAnulado)

                Me.mProgress.Value = 100
                Me.mProgress.Update()

            End If




            Me.AjustarDecimales()
            Me.mProgress.Value = 100
            Me.mProgress.Update()


            Me.CerrarFichero()

            If Me.mParaValidaSpyro = 1 Then
                Me.SpyroCompruebaCuentas()
            End If



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

            'SQL = "SELECT ROUND(SUM(ASNT_DEBE),2) FROM TC_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
            'SQL += " AND ASNT_IMPRIMIR = 'SI'"
            'TotalDebe = CType(Me.DbNewPaga.EjecutaSqlScalar(SQL), Decimal)

            'SQL = "SELECT ROUND(SUM(ASNT_HABER),2) FROM TC_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
            'SQL += " AND ASNT_IMPRIMIR = 'SI'"
            'TotalHaber = CType(Me.DbNewPaga.EjecutaSqlScalar(SQL), Decimal)

            SQL = "SELECT ROUND(SUM(round(NVL(ASNT_DEBE,'0'),2)),2) FROM TP_ASNT WHERE ASNT_F_ATOCAB = '" & Me.mFecha & "'"
            SQL += " AND ASNT_IMPRIMIR = 'SI'"



            If IsNumeric(Me.DbLeeCentral.EjecutaSqlScalar(SQL)) Then
                TotalDebe = CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Decimal)
            Else
                TotalDebe = 0
            End If


            SQL = "SELECT ROUND(SUM(round(NVL(ASNT_HABER,'0'),2)),2) FROM TP_ASNT WHERE ASNT_F_ATOCAB = '" & Me.mFecha & "'"
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
                Me.InsertaOracle("AC", 999, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaRedondeo, Me.mIndicadorDebe, "AJUSTE REDONDEO", TotalDiferencia, "SI", "", "", "SI", Me.mFecha, "", "", "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaRedondeo, Me.mIndicadorDebe, "AJUSTE REDONDEO", TotalDiferencia)
            End If

            If TotalHaber < TotalDebe Then
                TotalDiferencia = TotalDebe - TotalHaber
                MsgBox("Se va ha producir un Ajuste Decimal  de " & TotalDiferencia & "  " & vbCrLf & vbCrLf & "No Integre con valores superiores a 0.05", MsgBoxStyle.Information, "Atención")
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 999, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaRedondeo, Me.mIndicadorHaber, "AJUSTE REDONDEO", TotalDiferencia, "SI", "", "", "SI", Me.mFecha, "", "", "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaRedondeo, Me.mIndicadorHaber, "AJUSTE REDONDEO", TotalDiferencia)
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

#End Region
End Class
