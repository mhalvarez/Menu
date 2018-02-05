Option Strict Off
Imports System.IO
Imports System.Globalization
Public Class HC7
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



    Declare Function CharToOem Lib "user32" Alias "CharToOemA" (ByVal lpszSrc As String, ByVal lpszDst As String) As Long
    Declare Function OemToChar Lib "user32" Alias "OemToCharA" (ByVal lpszSrc As String, ByVal lpszDst As String) As Long


    Private mDebug As Boolean = False
    Private mStrConexionHotel As String

    Private mStrConexionNewGolf As String
    Private mParaConectaNewGolf As Integer
    Private mParaUsuarioNewGolf As String

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

    Private mParaIngresoPorHabitacion As Integer
    Private mParaIngresoPorhabitacionDpto As String



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

    Private mDevolucionAnticipos As Double

    Private mTotalProduccion As Double
    Private mTotalFacturacion As Double

    Private mAnticiposRecibidos As Double
    Private mCancelacionAnticipos As Double




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
#Region "METODOS VARIOS"
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
            SQL += "NVL(PARA_INGRESO_POR_HABITACION,'0') INGRESOHABITACION,"
            SQL += "NVL(PARA_INGRESO_HABITACION_DPTO,'0') INGRESOHABITACIONDPTO,"
            SQL += "NVL(PARA_CONECTA_NEWGOLF,'0') CONECTANEWGOLF,"
            SQL += "NVL(PARA_USUARIO_NEWGOLF,'?') USUARIONEWGOLF"



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

            End If
            Me.DbLeeCentral.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Exclamation, "Carga de Parámetros en Constructor de la Clase")
        End Try
    End Sub
    Private Sub BorraRegistros()
        Try
            SQL = "SELECT COUNT(*) FROM TH_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            If CType(Me.DbLeeCentral.EjecutaSqlScalar(SQL), Integer) > 0 Then
                MsgBox("Ya existen Movimientos de Integración para esta Fecha", MsgBoxStyle.Information, "Atención")
            End If
            SQL = "DELETE TH_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
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
            SQL += Mid(vAmpcpto, 1, 40).Replace("'", "''") & "',"
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

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                  ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                  , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                    ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vAuxiliarS As String, ByVal vAuxiliarN As Double, ByVal vNumeroFactura As String, ByVal vSerieFactura As String, ByVal vTipoImpuesto As Double, ByVal vAuxiliarS2 As String)

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
            SQL += "ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_CIF,ASNT_IMPRIMIR,ASNT_AUXILIAR_STRING,ASNT_AUXILIAR_NUMERICO,ASNT_FACTURA_NUMERO,ASNT_FACTURA_SERIE,ASNT_TIPO_IMPUESTO,ASNT_AUXILIAR_STRING2) values ('"
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
            SQL += vNombre.Replace("'", "''") & "'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vCif & "','" & vImprimir & "','"
            SQL += vAuxiliarS.Replace("'", "''") & "'," & vAuxiliarN & ",'"
            SQL += vNumeroFactura & "','"
            SQL += vSerieFactura & "',"
            SQL += vTipoImpuesto & ",'"
            SQL += vAuxiliarS2.Replace("'", "''") & "')"




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
                'Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double) + Me.FacturacionSalidasServiciosSinIgic + Me.FacturacionSalidaDesembolsos
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

            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Total LIQUIDO Facturas")
        End Try

    End Sub


    Private Sub FacturasSalidaTotaLDescuentos()
        Dim Total As Double
        Try
            SQL = "SELECT SUM(TNHT_DESF.DESF_VALO)TOTAL,TNHT_TIDE.TIDE_DESC TIPO,NVL(TNHT_TIDE.TIDE_CTB1,'0') CUENTA "
            SQL = SQL & "FROM TNHT_FACT,TNHT_DESF,TNHT_TIDE WHERE"
            SQL = SQL & "     TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI"
            SQL = SQL & " AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI"
            SQL = SQL & " AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI"
            SQL = SQL & " AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "'"
            SQL = SQL & " GROUP BY TNHT_TIDE.TIDE_DESC,TNHT_TIDE.TIDE_CTB1"


            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, "Comisiones " & CType(Me.DbLeeHotel.mDbLector("TIPO"), String), Total, "NO", "", "", "SI")


            End While
            Me.DbLeeHotel.mDbLector.Close()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Total Descuentos Facturas")
        End Try
    End Sub
    Private Sub FacturasSalidaTotalFActura()
        Try
            Dim TotalFactura As Double
            Dim Dni As String
            Dim Cuenta As String = "0"
            Dim CuentaAnula As String = "0"
            Dim Titular As String

            SQL = "SELECT  TNHT_FACT.FACT_STAT AS ESTADO, TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,TNHT_FACT.FACT_TOTA TOTAL,TNHT_FACT.FACT_VALO VALOR,"
            SQL += "NVL(ENTI_CODI,'') AS ENTI_CODI,NVL(CCEX_CODI,'?') AS CCEX_CODI,NVL(CLIE_CODI,'0') AS CLIENTE "
            SQL += " , NVL(TNHT_FACT.FACT_TITU,'') TITULAR "
            SQL += " , FAAN_CODI "
            SQL += "FROM TNHT_FACT "
            SQL += "WHERE "
            SQL += "(TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "
            SQL += "ORDER BY TNHT_FACT.SEFA_CODI ASC, TNHT_FACT.FACT_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                '  If CType(DbLeeHotel.mDbLector("NUMERO"), Integer) = 15 Then
                '  MsgBox("AQUI")
                '  End If

                Linea = Linea + 1
                If CType(DbLeeHotel.mDbLector("CCEX_CODI"), String) = "TPV" Then
                    TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)
                Else
                    '   TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)
                    TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)
                End If


                ' DETERMINAR EL TIPO DE FACTURA 
                ' FACTURA DE CONTADO 

                If IsDBNull(Me.DbLeeHotel.mDbLector("ENTI_CODI")) = True And IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = True Then
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

                If IsDBNull(Me.DbLeeHotel.mDbLector("ENTI_CODI")) = False Then
                    'SQL = "SELECT NVL(ENTI_DEAN_AF,0) CUENTA FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                    SQL = "SELECT NVL(ENTI_NCON_AF,0) CUENTA FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                    Cuenta = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    If Cuenta = "0" Or IsNothing(Cuenta) = True Then
                        Cuenta = "0"
                    End If

                    'SQL = "SELECT NVL(ENTI_PAMA_AF,0) CUENTA FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                    SQL = "SELECT NVL(ENTI_NCON_AF,0) CUENTA FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"

                    CuentaAnula = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    If CuentaAnula = "0" Or IsNothing(Cuenta) = True Then
                        CuentaAnula = "0"
                    End If


                    SQL = "SELECT NVL(ENTI_NUCO,'0') FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                    Dni = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    If Dni = "0" Or IsNothing(Dni) = True Then
                        Dni = "0"
                    End If


                End If


                ' FACTURA DE CUENTA NO ALOJADO 

                If CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) <> "?" Then
                    'SQL = "SELECT NVL(CCEX_DEAN,0) CUENTA FROM TNHT_CCEX WHERE CCEX_CODI = '" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "'"
                    SQL = "SELECT NVL(CCEX_NCON,0) CUENTA FROM TNHT_CCEX WHERE CCEX_CODI = '" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "'"

                    Cuenta = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    If Cuenta = "0" Or IsNothing(Cuenta) = True Then
                        Cuenta = "0"
                    End If

                    'SQL = "SELECT NVL(CCEX_PAMA,0) CUENTA FROM TNHT_CCEX WHERE CCEX_CODI = '" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "'"
                    SQL = "SELECT NVL(CCEX_NCON,0) CUENTA FROM TNHT_CCEX WHERE CCEX_CODI = '" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "'"
                    CuentaAnula = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

                    If CuentaAnula = "0" Or IsNothing(Cuenta) = True Then
                        CuentaAnula = "0"
                    End If


                    SQL = "SELECT NVL(CCEX_NUCO,'0') FROM TNHT_CCEX WHERE CCEX_CODI = '" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "'"
                    Dni = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    If Dni = "0" Or IsNothing(Dni) = True Then
                        Dni = "0"
                    End If
                End If


                '   MsgBox(Cuenta)

                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)

                If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                    ' FACTURAS NORMALES
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, "su Factura " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Dni, Titular, "SI", "DOCUMENTO", 0, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), 0, "")

                Else
                    ' FACTURAS RECTIFICATIVAS
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaber, "su Rectificativa " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura * -1, "NO", Dni, Titular, "SI", "DOCUMENTO", 0, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), 0, "")

                End If


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas")
        End Try
    End Sub
    Private Sub FacturasSalidaTotalBase()
        Try
            Dim TotalFactura As Double
            Dim Dni As String
            Dim Cuenta As String = "0"
            Dim Titular As String


            SQL = "SELECT (SUM(FAIV_INCI) - SUM(FAIV_VIMP)) TOTAL ,SUM(FACT_TOTA) TOTAL1,FACT_DAEM, "

            SQL += " TNHT_FACT.FACT_STAT AS ESTADO, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += " TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,"
            SQL += "NVL(ENTI_CODI,'') AS ENTI_CODI,NVL(CCEX_CODI,'?') AS CCEX_CODI,"
            SQL += " NVL(CLIE_CODI,'0') AS CLIENTE, "
            SQL += "  NVL(TNHT_FACT.FACT_TITU,'') TITULAR "
            SQL += " , FAAN_CODI "


            SQL += "FROM TNHT_FAIV, TNHT_FACT "
            SQL += "WHERE TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "AND TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "' "

            SQL += " GROUP BY FACT_DAEM,FACT_STAT,TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI,ENTI_CODI,CCEX_CODI,CLIE_CODI,FACT_TITU,FAAN_CODI"

            SQL += " ORDER BY TNHT_FACT.SEFA_CODI ASC, TNHT_FACT.FACT_CODI ASC"


            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                '  If CType(DbLeeHotel.mDbLector("NUMERO"), Integer) = 15 Then
                '  MsgBox("AQUI")
                '  End If

                Linea = Linea + 1
                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)


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


                '   MsgBox(Cuenta)

                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)

                If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                    ' FACTURAS NORMALES
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorHaber, "Ingresos por Prestación de Servicios " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura, "NO", Dni, Titular, "SI", "DOCUMENTO", 0, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), 0, "")

                Else
                    ' FACTURAS RECTIFICATIVAS
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("FV", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorDebe, "Ingresos por Prestación de Servicios  " & CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalFactura * -1, "NO", Dni, Titular, "SI", "DOCUMENTO", 0, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), 0, "")

                End If


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas")
        End Try
    End Sub

    Private Sub FacturasSalidaIgicDetallado()

        Try

            Dim TotalIva As Double
            Dim TotalBase As Double

            Dim DescripcionAsiento As String

            Dim Dni As String
            Dim CuentaContraPartida As String = "0"

            SQL = ""
            SQL = "SELECT TNHT_FACT.FACT_STAT AS ESTADO,TNHT_FACT.FACT_CODI AS NUMERO,TNHT_FACT.SEFA_CODI AS SERIE ,"
            SQL += "TNHT_FAIV.FAIV_TAXA AS TIPO,(FAIV_INCI-FAIV_VIMP) BASE, TNHT_FAIV.FAIV_VIMP IGIC,NVL(TIVA_CTB1,'0') CUENTA,FAIV_TAXA ,"
            SQL += "NVL(ENTI_CODI,'') AS ENTI_CODI,NVL(CCEX_CODI,'?') AS CCEX_CODI,NVL(CLIE_CODI,'0') AS CLIENTE "
            SQL += " , FAAN_CODI "
            SQL += "FROM TNHT_FAIV, TNHT_FACT,TNHT_TIVA "
            SQL += "WHERE TNHT_FAIV.SEFA_CODI = TNHT_FACT.SEFA_CODI "
            SQL += "AND TNHT_FAIV.FACT_CODI = TNHT_FACT.FACT_CODI "
            SQL += "AND TNHT_FAIV.TIVA_CODI = TNHT_TIVA.TIVA_CODI "
            SQL += "AND (TNHT_FACT.FACT_DAEM = " & "'" & Me.mFecha & "') "

            SQL += " ORDER BY TNHT_FACT.SEFA_CODI ASC, TNHT_FACT.FACT_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                ' DETERMINAR EL TIPO DE FACTURA 
                ' FACTURA DE CONTADO 

                If CType(Me.DbLeeHotel.mDbLector("ESTADO"), String) = "1" Then
                    SQL = "SELECT NVL(CLIE_NUID,'0') FROM TNHT_CLIE WHERE CLIE_CODI = " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), Integer)
                    Dni = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    If Dni = "0" Or IsNothing(Dni) = True Then
                        Dni = Me.mClientesContadoCif
                    End If
                    CuentaContraPartida = Me.mCtaClientesContado
                Else
                    Dni = ""
                End If

                ' FACTURA DE ENTIDAD

                If CType(Me.DbLeeHotel.mDbLector("ESTADO"), String) = "2" And IsDBNull(Me.DbLeeHotel.mDbLector("ENTI_CODI")) = False Then
                    SQL = "SELECT NVL(ENTI_NCON_AF,0) CUENTA FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                    CuentaContraPartida = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    If CuentaContraPartida = "0" Or IsNothing(CuentaContraPartida) = True Then
                        CuentaContraPartida = "0"
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
                    CuentaContraPartida = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    If CuentaContraPartida = "0" Or IsNothing(CuentaContraPartida) = True Then
                        CuentaContraPartida = "0"
                    End If


                    SQL = "SELECT NVL(CCEX_NUCO,'0') FROM TNHT_CCEX WHERE CCEX_CODI = '" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "'"
                    Dni = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    If Dni = "0" Or IsNothing(Dni) = True Then
                        Dni = "0"
                    End If
                End If





                Linea = Linea + 1
                TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double)
                TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)


                TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
                TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

                DescripcionAsiento = "Igic Soportado " & Me.mParaTextoIva & " " & Me.DbLeeHotel.mDbLector("NUMERO") & "/" & Me.DbLeeHotel.mDbLector("SERIE")


                If IsDBNull(Me.DbLeeHotel.mDbLector("FAAN_CODI")) = True Then
                    ' FACTURAS NORMALES
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorHaber, DescripcionAsiento, TotalIva, "NO", Me.mClientesContadoCif, "Contrapartida : " & CuentaContraPartida, "SI", Me.mParaTextoIva, TotalBase, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("FAIV_TAXA"), Double), CuentaContraPartida)

                Else
                    ' FACTURAS RECTIFICATIVAS
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, DescripcionAsiento, TotalIva * -1, "NO", Me.mClientesContadoCif, "Contrapartida : " & CuentaContraPartida, "SI", Me.mParaTextoIva, TotalBase, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("FAIV_TAXA"), Double), CuentaContraPartida)
                End If




            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Detalle de Impuesto")
        End Try
    End Sub


#End Region


#Region "ASIENTO-51"
    Private Sub NotasDeCreditoTotalLiquido()
        Dim Total As Double


        Try



            SQL = "SELECT VH_NIVA.NCRE_CODI,VH_NIVA.SEFA_CODI,(SUM(MOVI_VLIQ)*-1 ) TOTAL ,TNHT_NCRE.NCRE_DAEM ,NVL(NCRE_TITU,'?') AS NOMBRE "
            SQL += "FROM VH_NIVA,TNHT_NCRE "
            SQL += "WHERE "
            SQL += "  VH_NIVA.NCRE_CODI = TNHT_NCRE.NCRE_CODI "
            SQL += " AND VH_NIVA.SEFA_CODI = TNHT_NCRE.SEFA_CODI "
            SQL += "AND TNHT_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
            SQL += " GROUP BY VH_NIVA.NCRE_CODI,VH_NIVA.SEFA_CODI,TNHT_NCRE.NCRE_DAEM,NCRE_TITU "
            SQL += " ORDER BY VH_NIVA.NCRE_CODI  ASC"


            Me.DbLeeHotel.TraerLector(SQL)


            Total = Decimal.Round(CType(Total, Decimal), 2)
            While Me.DbLeeHotel.mDbLector.Read




                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("NCRE_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String), Total, "SI", "", CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI")
                End If

            End While
            Me.DbLeeHotel.mDbLector.Close()


            'ANULADAS
            SQL = "SELECT VH_NIVA.NCRE_CODI,VH_NIVA.SEFA_CODI,(SUM(MOVI_VLIQ) ) TOTAL ,NCRE_DAAN ,NVL(NCRE_TITU,'?') AS NOMBRE "
            SQL += "FROM VH_NIVA,TNHT_NCRE "
            SQL += "WHERE "
            SQL += "  VH_NIVA.NCRE_CODI = TNHT_NCRE.NCRE_CODI "
            SQL += " AND VH_NIVA.SEFA_CODI = TNHT_NCRE.SEFA_CODI "
            SQL += "AND TNHT_NCRE.NCRE_DAAN = " & "'" & Me.mFecha & "' "
            SQL += " GROUP BY VH_NIVA.NCRE_CODI,VH_NIVA.SEFA_CODI,NCRE_DAAN,NCRE_TITU "
            SQL += " ORDER BY VH_NIVA.NCRE_CODI  ASC"



            Me.DbLeeHotel.TraerLector(SQL)


            Total = Decimal.Round(CType(Total, Decimal), 2)
            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaManoCorriente, Me.mIndicadorDebe, "(A) " & CType(Me.DbLeeHotel.mDbLector("NCRE_CODI"), String) & "/" & CType(Me.DbLeeHotel.mDbLector("SEFA_CODI"), String), Total, "SI", "", CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI")
                End If

            End While
            Me.DbLeeHotel.mDbLector.Close()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub NotasDeCreditoCliente()


        Dim Total As Double
        Dim TotalPendiente As Double


        Dim Dni As String
        Dim Cuenta As String = "0"

        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEFA_CODI AS SERIE, TNHT_NCRE.NCRE_CODI AS NUMERO,TNHT_NCRE.NCRE_CODI||'/'||TNHT_NCRE.SEFA_CODI FACTURA,(NCRE_VALO) TOTAL, "
        SQL += " NCRE_TITU, NCRE_DAEM,NVL(NCRE_TITU,'?') AS NOMBRE "
        SQL += " ,ENTI_CODI,CCEX_CODI"
        SQL += " FROM TNHT_NCRE "
        SQL += " WHERE  "
        SQL += " TNHT_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " ORDER BY TNHT_NCRE.NCRE_CODI ASC "


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read



            ' DETERMINAR EL TIPO DE FACTURA 
            ' FACTURA DE CONTADO 

            If IsDBNull(Me.DbLeeHotel.mDbLector("ENTI_CODI")) = True And IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = True Then
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


            End If


            ' FACTURA DE CUENTA NO ALOJADO 


            If IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = False Then
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



            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalPendiente = 0
            Total = Decimal.Round(CType(Total, Decimal), 2)
            TotalPendiente = Decimal.Round(CType(TotalPendiente, Decimal), 2)

            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaberFac, CType(Me.DbLeeHotel.mDbLector("FACTURA"), String), Total, "NO", Dni, CType(Me.DbLeeHotel.mDbLector("NOMBRE"), String), "SI")


        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub NotasDeCreditoCreditoDetalleIgic()
        Dim TotalIva As Double
        Dim TotalBase As Double
        Dim Totalfactura As Double


        Dim Dni As String
        Dim CuentaContraPartida As String = "0"

        Dim DescripcionAsiento As String



        SQL = "SELECT"
        SQL += " TNHT_NCRE.SEFA_CODI AS SERIE , TNHT_NCRE.NCRE_CODI AS NUMERO,(NCRE_VALO * -1)TOTAL,SUM(MOVI_VLIQ) BASE,SUM(VIVA) IGIC, '"
        SQL += Me.mParaTextoIva & " ' || TIVA_PERC ||'%  '|| TNHT_NCRE.NCRE_CODI ||'/'|| TNHT_NCRE.SEFA_CODI DESCRIPCION, TNHT_NCRE.NCRE_DAEM,TIVA_PERC TIPO,NVL(TIVA_CTB1,'0') "
        SQL += " CUENTA ,NVL(TNHT_TIVA.TIVA_CCVL,'?') AS X "
        SQL += " ,TNHT_NCRE.ENTI_CODI AS ENTI_CODI , TNHT_NCRE.CCEX_CODI AS CCEX_CODI "
        SQL += " FROM TNHT_NCRE,TNHT_TIVA,VH_NIVA"


        SQL += " WHERE  "
        SQL += " TNHT_NCRE.NCRE_CODI = VH_NIVA.NCRE_CODI    AND"
        SQL += " TNHT_NCRE.SEFA_CODI = VH_NIVA.SEFA_CODI"
        SQL += " AND VH_NIVA.TIVA = TNHT_TIVA.TIVA_CODI "
        SQL += " AND TNHT_NCRE.NCRE_DAEM = " & "'" & Me.mFecha & "' "
        SQL += " GROUP BY TNHT_NCRE.SEFA_CODI,TNHT_NCRE.NCRE_CODI,NCRE_VALO,TIVA_PERC,TNHT_NCRE.NCRE_DAEM,TIVA_CTB1,"
        SQL += " TNHT_TIVA.TIVA_CCVL,TNHT_NCRE.ENTI_CODI , TNHT_NCRE.CCEX_CODI "
        SQL += " ORDER BY TNHT_NCRE.NCRE_CODI ASC "

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            ' DETERMINAR EL TIPO DE FACTURA 
            ' FACTURA DE CONTADO 

            If IsDBNull(Me.DbLeeHotel.mDbLector("ENTI_CODI")) = True And IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = True Then
                SQL = "SELECT NVL(CLIE_NUID,'0') FROM TNHT_CLIE WHERE CLIE_CODI = " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), Integer)
                Dni = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                If Dni = "0" Or IsNothing(Dni) = True Then
                    Dni = Me.mClientesContadoCif
                End If
                CuentaContraPartida = Me.mCtaClientesContado
            Else
                Dni = ""
            End If

            ' FACTURA DE ENTIDAD

            If IsDBNull(Me.DbLeeHotel.mDbLector("ENTI_CODI")) = False Then
                SQL = "SELECT NVL(ENTI_NCON_AF,0) CUENTA FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                CuentaContraPartida = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                If CuentaContraPartida = "0" Or IsNothing(CuentaContraPartida) = True Then
                    CuentaContraPartida = "0"
                End If

                SQL = "SELECT NVL(ENTI_NUCO,'0') FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                Dni = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                If Dni = "0" Or IsNothing(Dni) = True Then
                    Dni = "0"
                End If


            End If


            ' FACTURA DE CUENTA NO ALOJADO 

            If IsDBNull(Me.DbLeeHotel.mDbLector("CCEX_CODI")) = False Then
                SQL = "SELECT NVL(CCEX_NCON,0) CUENTA FROM TNHT_CCEX WHERE CCEX_CODI = '" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "'"
                CuentaContraPartida = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                If CuentaContraPartida = "0" Or IsNothing(CuentaContraPartida) = True Then
                    CuentaContraPartida = "0"
                End If


                SQL = "SELECT NVL(CCEX_NUCO,'0') FROM TNHT_CCEX WHERE CCEX_CODI = '" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "'"
                Dni = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                If Dni = "0" Or IsNothing(Dni) = True Then
                    Dni = "0"
                End If
            End If



            ''


            Linea = Linea + 1
            TotalIva = CType(Me.DbLeeHotel.mDbLector("IGIC"), Double) * -1
            TotalIva = Decimal.Round(CType(TotalIva, Decimal), 2)

            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            TotalBase = Decimal.Round(CType(TotalBase, Decimal), 2)

            Totalfactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("TOTAL"), Decimal), 2)

            DescripcionAsiento = Me.mParaTextoIva & " " & Me.DbLeeHotel.mDbLector("NUMERO") & "/" & Me.DbLeeHotel.mDbLector("SERIE")


            Me.mTipoAsiento = "DEBE"
            ' Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("DESCRIPCION"), String), TotalIva, "NO", "", "", "SI")

            Me.InsertaOracle("AC", 51, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CType(Me.DbLeeHotel.mDbLector("CUENTA"), String), Me.mIndicadorDebe, DescripcionAsiento, TotalIva, "NO", Me.mClientesContadoCif, "Contrapartida : " & CuentaContraPartida, "SI", Me.mParaTextoIva, TotalBase, CType(Me.DbLeeHotel.mDbLector("NUMERO"), String), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), CType(Me.DbLeeHotel.mDbLector("TIPO"), Double), CuentaContraPartida)



        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub

#End Region


#Region "RUTINAS PRIVADAS"

    Private Function xFacturacionSalidaDesembolsos() As Double
        Dim Resultado As String
        Dim Total As Double
        Try

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
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try

    End Function
    Private Function FacturacionTotalServiciosSinIgic() As Double

        Dim Total As Double
        Try


            '__________________________________________________________________________________________
            ' CALCULO DEl TOTAL DE LOS SERVICIOS SIN IGIC DE LAs FACTURA
            '__________________________________________________________________________________________
            SQL = "SELECT MOVI_VCRE AS TOTAL  FROM TNHT_MOVI , TNHT_FACT,TNHT_FAMO "
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


            SQL = "SELECT (MOVI_VCRE * -1) AS TOTAL  FROM TNHT_MOVI , TNHT_FACT,TNHT_FAMO "
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
            MsgBox(EX.Message)
        End Try
    End Function
    Private Function FacturacionContadoServiciosSinIgic() As Double

        Dim Total As Double
        Try


            '__________________________________________________________________________________________
            ' CALCULO DEl TOTAL DE LOS SERVICIOS SIN IGIC DE LAs FACTURA
            '__________________________________________________________________________________________
            SQL = "SELECT MOVI_VCRE AS TOTAL  FROM TNHT_MOVI , TNHT_FACT,TNHT_FAMO "
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


            SQL = "SELECT (MOVI_VCRE * -1) AS TOTAL  FROM TNHT_MOVI , TNHT_FACT,TNHT_FAMO "
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
            MsgBox(EX.Message)
        End Try
    End Function
    Private Function FacturacionCreditoDesembolsos() As Double
        Dim Resultado As String
        Dim Total As Double
        Try


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
        Catch EX As Exception
            MsgBox(EX.Message)
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
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try
    End Function


    Private Function xFacturacionNoAlojadoDesembolsos() As Double
        Dim Resultado As String
        Dim Total As Double
        Try
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
        Catch EX As Exception
            MsgBox(EX.Message)
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
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try
    End Function

    Private Function xFacturacionNoAlojadoDesembolsosFactura(ByVal vSerie As String, ByVal vFactura As Integer) As Double
        Dim Resultado As String
        Dim Total As Double
        Try
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
        Catch EX As Exception
            MsgBox(EX.Message)
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
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try
    End Function


#End Region
#End Region
#Region "METODOS PUBLICOS"
    Public Sub Procesar()
        Try

            '   MsgBox("  anulaciones de notas de crédito")

            ' MsgBox("Ojo revisar  COMISION de visas , en depositos antincipados de agencias si los hubiera ", MsgBoxStyle.Exclamation, "Atención")
            If Me.FileEstaOk = False Then Exit Sub



            ' ---------------------------------------------------------------
            ' Asiento Facturacion total del dia                   3
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                Me.mTextDebug.Text = "Calculando Total Luíquido Facturas de Salida"
                Me.mTextDebug.Update()

                Me.FacturasSalidaTotalFActura()

                Me.mTextDebug.Text = "Calculando Base Imponible Facturas de Salida"
                Me.mTextDebug.Update()


                Me.FacturasSalidaTotalBase()



                Me.FacturasSalidaIgicDetallado()
                Me.mTextDebug.Text = "Detalle de Impuesto Facturas de Salida"
                Me.mTextDebug.Update()


                Me.mProgress.Value = 10
                Me.mProgress.Update()
            End If

            ' ---------------------------------------------------------------
            ' Asiento Notas de Credito 51
            '----------------------------------------------------------------
            If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                Me.mTextDebug.Text = "Calculando Total Luíquido Notas de Crédito"
                Me.mTextDebug.Update()
                Me.NotasDeCreditoTotalLiquido()


                Me.mTextDebug.Text = "Calculando Descuentos Financieros y Comisiones Notas de Crédito"
                Me.mTextDebug.Update()


                Me.NotasDeCreditoCliente()




                Me.mTextDebug.Text = "Detalle de Impuesto Notas de Crédito"
                Me.mTextDebug.Update()

                Me.NotasDeCreditoCreditoDetalleIgic()

                Me.mProgress.Value = 20
                Me.mProgress.Update()
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
                MsgBox("Se va ha producir un Ajuste Decimal  de " & TotalDiferencia & "  " & vbCrLf & vbCrLf & "No Integre con valores superiores a 0.05", MsgBoxStyle.Information, "Atención")
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 999, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaRedondeo, Me.mIndicadorDebe, "AJUSTE REDONDEO", TotalDiferencia, "SI", "", "", "SI")
            End If

            If TotalHaber < TotalDebe Then
                TotalDiferencia = TotalDebe - TotalHaber
                MsgBox("Se va ha producir un Ajuste Decimal  de " & TotalDiferencia & "  " & vbCrLf & vbCrLf & "No Integre con valores superiores a 0.05", MsgBoxStyle.Information, "Atención")
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 999, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaRedondeo, Me.mIndicadorHaber, "AJUSTE REDONDEO", TotalDiferencia, "SI", "", "", "SI")
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

#End Region
End Class
