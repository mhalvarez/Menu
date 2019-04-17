Imports System.IO
Public Class HTitoNewStock
    Private mDebug As Boolean = False
    Private mCondensarAsiento As Boolean = False
    Private mStrConexionHotel As String

    Private mStrConexionCentral As String

    '2013
    Private mStrConexionNewCentral As String
    Private mConectarNewCentral As Integer
    Private mHotelNewCentral As Integer


    Private mStrConexionSpyro As String
    Private mTexto As String


    Private mFecha As Date
    Private mMesInventarios As String
    Private mEmpGrupoCod As String
    Private mEmpCod As String
    Private mEmpNum As Integer

    Private mIndicadorDebe As String
    Private mIndicadorHaber As String

    Private mIndicadorDebeFac As String
    Private mIndicadorHaberFac As String

    Private mIndicadorDebeAbono As String
    Private mIndicadorHaberAbono As String

    Private mTipoAsiento As String

    Private mDebe As Double
    Private mHbaber As Double
    Private mTextDebug As System.Windows.Forms.TextBox
    Private mListBoxDebug As System.Windows.Forms.ListBox

    Private mControlForm As System.Windows.Forms.Form

    Private mProgresBar As System.Windows.Forms.ProgressBar

    Private mConectaNewPaga As Boolean
    Private mStrConexionNewPaga As String



    Private mContabilizaAlbaranes As Boolean
    Private mTipoFormalizaAlbaran As String
    Private mParaSeriefacturas As String
    Private mParaSeriefacturasNewPaga As String
    Private mTimo_Albaran As Integer
    Private mTimo_Albaran_Dev As Integer
    Private mTimo_Traspaso As Integer
    Private mTimo_Factura_Directa As Integer
    Private mTimo_Factura_Directa_Dev As Integer
    Private mTimo_Factura_Al As Integer
    Private mTimo_Salida_Gastos As Integer
    Private mTimo_Roturas As Integer

    Private mParaFilePath As String
    Private mParaSoloFacturas As Integer







    ' parametros cuentas

    Private mCtaFormalizaAlbaranes As String
    Private mCtaMercaderia As String




    Private mCfivaLibro_Cod As String
    Private mCfivaClase_Cod As String
    Private mMonedas_Cod As String
    Private mCfatodiari_Cod As String
    Private mCfatodiari_Cod_Inv As String

    Private mCfivatimpu_Cod As String
    Private mCfivatip_Cod As String
    Private mCfatotip_Cod As String
    Private mGvagente_Cod As String
    Private mCtaClientesContado As String
    Private mCtaRoturas As String
    Private mCtaPuntoVerde As String


    Private mSoloInventariosIniciales As Boolean = False

    ' Valores de retorno para debug



    Private SQL As String
    Private Linea As Integer
    Private Filegraba As StreamWriter
    Private FileEstaOk As Boolean
    Private DbLeeCentral As C_DATOS.C_DatosOledb
    Private DbLeeHotel As C_DATOS.C_DatosOledb
    Private DbLeeNewPaga As C_DATOS.C_DatosOledb
    Private DbLeeHotelAux As C_DATOS.C_DatosOledb
    Private DbLeeHotelAux2 As C_DATOS.C_DatosOledb
    Private DbLeeHotelAux3 As C_DATOS.C_DatosOledb
    Private DbLeeHotelAux4 As C_DATOS.C_DatosOledb

    Private DbLeeHotelAux5 As C_DATOS.C_DatosOledb
    Private DbLeeHotelAux6 As C_DATOS.C_DatosOledb

    Private DbGrabaCentral As C_DATOS.C_DatosOledb
    Private DbSpyro As C_DATOS.C_DatosOledb
    Private DbNewCentral As C_DATOS.C_DatosOledb


    ' otros
    Dim FechaInventarioFinal As Date
    Dim FechaInventarioInicial As Date

    Dim Contador As Integer

    Dim AuxStr As String
    Dim AuxInteger As Integer

    Private mResult As String
    Private mResultInt As Integer
    Private mResultCdbl As Double
    Private mTipoGasto As String

#Region "CONSTRUCTOR"
    Public Sub New(ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vStrConexionCentral As String,
  ByVal vStrConexionHotel As String, ByVal vFecha As Date, ByVal vFile As String, ByVal vDebug As Boolean,
  ByVal vControlDebug As System.Windows.Forms.TextBox, ByVal vListBox As System.Windows.Forms.ListBox,
  ByVal vStrConexionSpyro As String, ByVal vInventario As Boolean, ByVal vSoloInventariosIniciales As Boolean,
  ByVal vEmpNum As Integer, ByVal vControlForm As System.Windows.Forms.Form, ByVal vProgesBar As System.Windows.Forms.ProgressBar,
  ByVal vConectaNewPaga As Boolean, ByVal vStrConexionNewPaga As String, ByVal vTipoGasto As String, ByVal vCondensar As Boolean)
        MyBase.New()

        Me.mDebug = vDebug
        Me.mCondensarAsiento = vCondensar
        Me.mEmpGrupoCod = vEmpGrupoCod
        Me.mEmpCod = vEmpCod
        Me.mEmpNum = vEmpNum
        Me.mStrConexionHotel = vStrConexionHotel
        Me.mStrConexionCentral = vStrConexionCentral
        Me.mStrConexionSpyro = vStrConexionSpyro
        Me.mFecha = vFecha


        Me.mTextDebug = vControlDebug
        Me.mListBoxDebug = vListBox

        Me.mControlForm = vControlForm
        Me.mProgresBar = vProgesBar



        Me.mConectaNewPaga = vConectaNewPaga
        Me.mStrConexionNewPaga = vStrConexionNewPaga


        Me.mListBoxDebug.Items.Clear()
        Me.mListBoxDebug.Update()

        Me.mSoloInventariosIniciales = vSoloInventariosIniciales
        Me.mTipoGasto = vTipoGasto


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

        Me.DbLeeHotelAux3 = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
        Me.DbLeeHotelAux3.AbrirConexion()
        Me.DbLeeHotelAux3.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


        'Me.DbLeeHotelAux4 = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
        'Me.DbLeeHotelAux4.AbrirConexion()
        'Me.DbLeeHotelAux4.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

        'Me.DbLeeHotelAux5 = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
        'Me.DbLeeHotelAux5.AbrirConexion()
        'Me.DbLeeHotelAux5.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


        'Me.DbLeeHotelAux6 = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
        'Me.DbLeeHotelAux6.AbrirConexion()
        'Me.DbLeeHotelAux6.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

        If Me.mConectaNewPaga = True Then
            Me.DbLeeNewPaga = New C_DATOS.C_DatosOledb(Me.mStrConexionNewPaga)
            Me.DbLeeNewPaga.AbrirConexion()
            Me.DbLeeNewPaga.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")
        End If

        'Me.DbSpyro = New C_DATOS.C_DatosOledb(Me.mStrConexionSpyro)
        'Me.DbSpyro.AbrirConexion()
        'Me.DbSpyro.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


        Me.CargaParametros()

        Me.BorraRegistros()

        If vInventario = False Then
            Me.CrearFichero(Me.mParaFilePath & vFile)
        End If

        If Me.mConectarNewCentral = 1 Then
            SQL = "SELECT NVL(HOTEL_ODBC_NEWCENTRAL,'?') FROM TH_HOTEL "
            SQL += "  WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND HOTEL_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND HOTEL_EMP_NUM = " & Me.mEmpNum
            Me.mStrConexionNewCentral = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(HOTEL_HOTE_CODI,0) FROM TH_HOTEL "
            SQL += "  WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND HOTEL_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND HOTEL_EMP_NUM = " & Me.mEmpNum
            Me.mHotelNewCentral = CInt(Me.DbLeeCentral.EjecutaSqlScalar(SQL))


            Me.DbNewCentral = New C_DATOS.C_DatosOledb(Me.mStrConexionNewCentral)
            Me.DbNewCentral.AbrirConexion()
            Me.DbNewCentral.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")
        End If


    End Sub
#End Region
#Region "METODOS PRIVADOS"
    Private Sub CargaParametros()
        Try

            Me.mTextDebug.Text = "Cargando Parámetros"
            Me.mTextDebug.Update()



            SQL = "SELECT "
            SQL += "NVL(PARA_CONTABILIZA_ALBARANES,'0') CONTABILIZAALBARAN,"
            SQL += "NVL(PARA_CTA1,'0') CTA1,"
            SQL += "NVL(PARA_CTA2,'0') CTA2,"
            SQL += "NVL(PARA_CTA5,'0') CTA5,"
            SQL += "NVL(PARA_TIMO_ALBARAN,'0') ALBARAN,"
            SQL += "NVL(PARA_TIMO_ALBARAN_DEV,'0') ALBARANDEV,"
            SQL += "NVL(PARA_TIMO_TRASPASO,'0') TRASPASO,"
            SQL += "NVL(PARA_TIMO_FACTURA_DI,'0') FACTURAD,"
            SQL += "NVL(PARA_TIMO_FACTURA_DI_DEV,'0') FACTURADDEV,"
            SQL += "NVL(PARA_TIPO_FORMALIZA,'G') FORMALIZA,"
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

            SQL += "NVL(PARA_DEBE_ABONO,'?') PARA_DEBE_ABONO,"
            SQL += "NVL(PARA_HABER_ABONO,'?') PARA_HABER_ABONO,"

            SQL += "NVL(PARA_CLIENTES_CONTADO,'?') CLIENTESCONTADO,"
            SQL += "NVL(PARA_TIMO_FACTURA_AL,'0') FACTURAFORMALI,"
            SQL += "NVL(PARA_TIMO_SALIDAGASTOS,'0') SALIDAGASTOS,"
            SQL += "NVL(PARA_SERIE_FAC_SPYRO,'?') SERIEFAC,"
            SQL += "NVL(PARA_SERIE_FAC2_SPYRO,'?') SERIEFACPAGA,"
            SQL += "NVL(PARA_TIMO_ROTURAS,'0') ROTURAS,"
            SQL += "NVL(PARA_FILE_SPYRO_PATH,'?') PATCH,"
            SQL += "NVL(PARA_CFATODIARI_COD_INV,'?') DIARIOINV,"
            SQL += "NVL(PARA_SOLO_FACTURAS,0) PARA_SOLO_FACTURAS, "

            SQL += "NVL(PARA_CUENTAS_NEWCENTRAL,0) PARA_CUENTAS_NEWCENTRAL, "
            SQL += "NVL(PARA_CTA_PUNTOVERDE,'0') PARA_CTA_PUNTOVERDE"



            SQL += " FROM TS_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
            Me.DbLeeCentral.TraerLector(SQL)
            If Me.DbLeeCentral.mDbLector.Read Then

                Me.mContabilizaAlbaranes = CType(Me.DbLeeCentral.mDbLector.Item("CONTABILIZAALBARAN"), Boolean)
                Me.mCtaFormalizaAlbaranes = CType(Me.DbLeeCentral.mDbLector.Item("CTA1"), String)
                Me.mCtaMercaderia = CType(Me.DbLeeCentral.mDbLector.Item("CTA2"), String)
                Me.mCtaRoturas = CType(Me.DbLeeCentral.mDbLector.Item("CTA5"), String)

                Me.mTipoFormalizaAlbaran = CType(Me.DbLeeCentral.mDbLector.Item("FORMALIZA"), String)

                Me.mTimo_Albaran = CType(Me.DbLeeCentral.mDbLector.Item("ALBARAN"), Integer)
                Me.mTimo_Albaran_Dev = CType(Me.DbLeeCentral.mDbLector.Item("ALBARANDEV"), Integer)

                Me.mTimo_Traspaso = CType(Me.DbLeeCentral.mDbLector.Item("TRASPASO"), Integer)

                Me.mTimo_Factura_Directa = CType(Me.DbLeeCentral.mDbLector.Item("FACTURAD"), Integer)
                Me.mTimo_Factura_Directa_Dev = CType(Me.DbLeeCentral.mDbLector.Item("FACTURADDEV"), Integer)

                Me.mTimo_Factura_Al = CType(Me.DbLeeCentral.mDbLector.Item("FACTURAFORMALI"), Integer)
                Me.mTimo_Salida_Gastos = CType(Me.DbLeeCentral.mDbLector.Item("SALIDAGASTOS"), Integer)
                Me.mTimo_Roturas = CType(Me.DbLeeCentral.mDbLector.Item("ROTURAS"), Integer)


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

                Me.mIndicadorDebeAbono = CType(Me.DbLeeCentral.mDbLector.Item("PARA_DEBE_ABONO"), String)
                Me.mIndicadorHaberAbono = CType(Me.DbLeeCentral.mDbLector.Item("PARA_HABER_ABONO"), String)


                Me.mCtaClientesContado = CType(Me.DbLeeCentral.mDbLector.Item("CLIENTESCONTADO"), String)
                Me.mParaSeriefacturas = CType(Me.DbLeeCentral.mDbLector.Item("SERIEFAC"), String)
                Me.mParaSeriefacturasNewPaga = CType(Me.DbLeeCentral.mDbLector.Item("SERIEFACPAGA"), String)
                Me.mParaFilePath = CType(Me.DbLeeCentral.mDbLector.Item("PATCH"), String)
                Me.mCfatodiari_Cod_Inv = CType(Me.DbLeeCentral.mDbLector.Item("DIARIOINV"), String)
                Me.mParaSoloFacturas = CType(Me.DbLeeCentral.mDbLector.Item("PARA_SOLO_FACTURAS"), Integer)

                Me.mConectarNewCentral = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CUENTAS_NEWCENTRAL"), Integer)
                Me.mCtaPuntoVerde = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA_PUNTOVERDE"), String)


            End If
            Me.DbLeeCentral.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try
    End Sub
    Private Sub CrearFichero(ByVal vPath As String)
        Try
            Filegraba = New StreamWriter(vPath, False, System.Text.Encoding.ASCII)
            Filegraba.WriteLine("")
            Me.FileEstaOk = True
        Catch ex As Exception
            Me.FileEstaOk = False
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Function ControlCentrosdeCosto() As Integer
        SQL = "SELECT NVL(COUNT(*),'0') AS TOTAL FROM TNST_ALMA WHERE ALMA_CCST IS NULL"
        Return CType(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL), Integer)
    End Function
    Private Sub BorraRegistros()
        'SQL = "DELETE TS_ASNT WHERE ASNT_F_ATOCAB = '" & Me.mFecha & "'"
        SQL = "DELETE TS_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
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
        SQL += " AND INCI_ORIGEN =  '" & "NEWSTOCK COMPRAS" & "'"
        Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

    End Sub
    Private Sub BorraRegistrosInventario(ByVal vFecha As Date)
        SQL = "DELETE TS_ASNT WHERE ASNT_F_VALOR = '" & vFecha & "'"
        Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
        SQL = "DELETE TH_ERRO WHERE ERRO_F_ATOCAB =  '" & vFecha & "'"
        Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

        SQL = "DELETE TH_INCI WHERE INCI_DATR =  '" & Me.mFecha & "'"
        SQL += " AND INCI_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
        SQL += " AND INCI_EMP_COD = '" & Me.mEmpCod & "'"
        SQL += " AND INCI_EMP_NUM =  " & Me.mEmpNum
        Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

    End Sub
    Private Function CalculaBaseImponible(ByVal vMovi As Integer, ByVal vAnci As Integer) As Double
        SQL = "SELECT SUM(MOVI_NETO)AS BASE "
        SQL += "FROM TNST_MOVI  "
        SQL += "WHERE TNST_MOVI.MOVG_CODI = " & vMovi
        SQL += " AND TNST_MOVI.MOVG_ANCI = " & vAnci
        Return CType(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL), Double)
    End Function
    Private Function CalculaBaseImponiblenewPaga(ByVal vMovi As Integer, ByVal vTiim As Integer) As Double

        Dim Result As String


        If vTiim = 0 Then
            SQL = "SELECT SUM(NVL(MOVS_IMPO,0))AS BASE "
            SQL += "FROM TNPG_MOVS  "
            SQL += "WHERE TNPG_MOVS.MOVI_CODI = " & vMovi

        Else
            SQL = "SELECT SUM(NVL(MOVS_IMPO,0) - NVL(MOVS_IVAS,0))AS BASE "
            SQL += "FROM TNPG_MOVS  "
            SQL += "WHERE TNPG_MOVS.MOVI_CODI = " & vMovi
        End If


        Result = Me.DbLeeNewPaga.EjecutaSqlScalar(SQL)

        If Result <> "" And IsNumeric(Result) = True Then
            Return CDbl(Result)
        Else
            Return 0
        End If

    End Function
    Public Sub ProcesarOld()
        Try

            If Me.ControlCentrosdeCosto > 0 Or FileEstaOk = False Then
                MsgBox("Existen Departamentos sin Centro de Costo en NewStock", MsgBoxStyle.Information, "Atención")
                Me.CerrarFichero()
                Me.CierraConexiones()
                Exit Sub
            End If



            ' ---------------------------------------------------------------
            ' Asiento de Albaranes 1
            '----------------------------------------------------------------
            Me.mTextDebug.Text = "Calculando Pdte. de Formalizar"
            Me.mTextDebug.Update()

            If Me.mContabilizaAlbaranes = True Then

                Me.TotalPendienteFormalizarProveedorDetalladoHTito()

                Me.mTextDebug.Text = "Calculando Gastos por Departamento Albaranes"
                Me.mTextDebug.Update()

                'OK

                If Me.DbLeeHotel.StrError = "" Then
                    Me.GastosPorCentrodeCostoAlbaranes()
                End If


                Me.mProgresBar.Value = 10
                Me.mProgresBar.Update()
                Me.mControlForm.Update()
            End If



            'Me.AjustarDecimales(1)
            ' ---------------------------------------------------------------
            ' Asiento de Traspasos Internos 2
            '----------------------------------------------------------------

            Me.mTextDebug.Text = "Calculando Salidas por Traspaso"
            Me.mTextDebug.Update()
            'OK
            If Me.DbLeeHotel.StrError = "" Then
                Me.TraspasosSalidas()
            End If


            Me.mTextDebug.Text = "Calculando Entradas por Traspaso"
            Me.mTextDebug.Update()
            ' OK
            If Me.DbLeeHotel.StrError = "" Then
                Me.TraspasosEntradas()
            End If

            Me.mProgresBar.Value = 20
            Me.mProgresBar.Update()
            Me.mControlForm.Update()

            '---------------------------------------------------------------
            ' Asiento de Salidas a Gasto 20
            '----------------------------------------------------------------

            Me.mTextDebug.Text = "Calculando Salidas a Gasto"
            Me.mTextDebug.Update()
            'OK
            If Me.DbLeeHotel.StrError = "" Then
                Me.SalidasGastoSalidas()
            End If
            Me.mTextDebug.Text = "Calculando Entradas por Salida a Gasto"
            Me.mTextDebug.Update()
            'OK
            If Me.DbLeeHotel.StrError = "" Then
                Me.SalidasGastoEntradas()
            End If


            Me.mProgresBar.Value = 30
            Me.mProgresBar.Update()
            Me.mControlForm.Update()
            ' ---------------------------------------------------------------
            ' Asiento de Facturas Directas 3
            '----------------------------------------------------------------

            Me.mTextDebug.Text = "Facturas Directas Proveedor"
            Me.mTextDebug.Update()
            ' OK
            If Me.DbLeeHotel.StrError = "" Then
                Me.TotalFacturasProveedor()
            End If


            Me.mProgresBar.Value = 40
            Me.mProgresBar.Update()
            Me.mControlForm.Update()
            '--------------------------------------------------

            ' ---------------------------------------------------------------
            ' Devolucion de Albaranes 4
            '----------------------------------------------------------------
            Me.mTextDebug.Text = "Calculando Devolucion Pdte. de Formalizar"
            Me.mTextDebug.Update()

            'OK
            If Me.mContabilizaAlbaranes = True Then
                If Me.DbLeeHotel.StrError = "" Then
                    Me.TotalPendienteFormalizarProveedorDevolucionAlbaranHTito()
                    Me.mTextDebug.Text = "Calculando Gastos por Devolución  Departamento Albaranes"
                    Me.mTextDebug.Update()
                    'OK
                    Me.GastosPorCentrodeCostoAlbaranesDevolucionAlbaranAlmacen()
                End If
            End If


            Me.mProgresBar.Value = 50
            Me.mProgresBar.Update()
            Me.mControlForm.Update()
            ' ---------------------------------------------------------------
            ' Asiento de DEVOLUCVION Facturas Directas 5
            '----------------------------------------------------------------

            Me.mTextDebug.Text = "Devoluvión Facturas Directas Proveedor"
            Me.mTextDebug.Update()
            'OK
            If Me.DbLeeHotel.StrError = "" Then
                Me.TotalFacturasProveedorDevolucion()
            End If


            Me.mProgresBar.Value = 60
            Me.mProgresBar.Update()
            Me.mControlForm.Update()
            ' ---------------------------------------------------------------
            ' Asiento de Facturas que formalizan Albaranes Directas 6
            '----------------------------------------------------------------
            Me.mTextDebug.Text = "Facturas Formaliza Albaranes Proveedor"
            Me.mTextDebug.Update()

            If Me.DbLeeHotel.StrError = "" Then
                Me.TotalFacturasProveedorFormalizadas()
            End If

            If Me.DbLeeHotel.StrError = "" Then
                Me.TotalFacturasProveedorImpuestoFormalizadas()
            End If



            Me.mProgresBar.Value = 70
            Me.mProgresBar.Update()
            Me.mControlForm.Update()
            ' ---------------------------------------------------------------
            ' Asiento Roturas 30
            '----------------------------------------------------------------
            Me.mTextDebug.Text = "Roturas"
            Me.mTextDebug.Update()
            If Me.DbLeeHotel.StrError = "" Then
                Me.Roturas()
            End If



            Me.mProgresBar.Value = 80
            Me.mProgresBar.Update()
            Me.mControlForm.Update()
            ' ---------------------------------------------------------------
            ' Asiento de Facturas Directas NEWPAGA 40
            '----------------------------------------------------------------
            If Me.DbLeeHotel.StrError = "" Then
                If Me.mConectaNewPaga = True Then
                    Me.mTextDebug.Text = "Facturas Directas Proveedor"
                    Me.mTextDebug.Update()
                    Me.TotalFacturasProveedorNewPaga()


                    Me.mTextDebug.Text = "Calculando Gastos por Departamento Facturas"
                    Me.mTextDebug.Update()
                    '' FALTA DESARROLLO
                    Me.GastosPorCentrodeCostoFacturasNewPaga()


                    Me.mTextDebug.Text = "Calculando Impuesto por Facturas Directas"
                    Me.mTextDebug.Update()
                    '' FALTA DESARROLLO
                    Me.TotalFacturasProveedorImpuestoNewPaga()
                End If

            End If


            Me.mProgresBar.Value = 100
            Me.mProgresBar.Update()
            Me.mControlForm.Update()



            Me.CerrarFichero()
            Me.CierraConexiones()
            Me.mTextDebug.Text = "Fin de Integración"
            Me.mTextDebug.Update()


        Catch EX As Exception
            MsgBox(EX.Message)

            Me.CerrarFichero()
            Me.CierraConexiones()
        End Try

    End Sub
    Public Sub Procesar()
        Try

            If Me.DbLeeHotel.EstadoConexion <> ConnectionState.Open Then
                Me.CerrarFichero()
                Me.CierraConexiones()
                Exit Sub
            End If


            MsgBox("Revisar Debe/Haber y Signos en TODOS los Casos ")
            Me.GestionaCabecerasDeMovimiento()




            Me.CerrarFichero()
            Me.CierraConexiones()
            Me.mTextDebug.Text = "Fin de Integración"
            Me.mTextDebug.Update()


        Catch EX As Exception
            MsgBox(EX.Message)

            Me.CerrarFichero()
            Me.CierraConexiones()
        End Try

    End Sub
    Private Sub SALIR()
        Try

            Me.CerrarFichero()
            Me.CierraConexiones()
            Me.mTextDebug.Text = "Fin de Integración"
            Me.mTextDebug.Update()
        Catch ex As Exception

        End Try
    End Sub
    Public Sub ProcesarSoloFacturas()
        Try

            If Me.ControlCentrosdeCosto > 0 Then
                MsgBox("Existen Departamentos sin Centro de Costo en NewStock", MsgBoxStyle.Information, "Atención")
                Me.CerrarFichero()
                Me.CierraConexiones()
                Exit Sub
            End If

            ' ---------------------------------------------------------------
            ' Asiento de Facturas Directas 3
            '----------------------------------------------------------------

            Me.mTextDebug.Text = "Facturas Directas Proveedor"
            Me.mTextDebug.Update()
            ' OK
            Me.TotalFacturasProveedor()



            Me.mTextDebug.Text = "Calculando Gastos por Departamento Facturas"
            Me.mTextDebug.Update()
            'OK
            'Me.GastosPorCentrodeCostoFacturas()


            Me.mTextDebug.Text = "Calculando Impuesto por Facturas Directas"
            Me.mTextDebug.Update()
            'OK
            'Me.TotalFacturasProveedorImpuesto()

            Me.mProgresBar.Value = 40
            Me.mProgresBar.Update()
            Me.mControlForm.Update()


            ' ---------------------------------------------------------------
            ' Asiento de DEVOLUCVION Facturas Directas 5
            '----------------------------------------------------------------

            Me.mTextDebug.Text = "Devoluvión Facturas Directas Proveedor"
            Me.mTextDebug.Update()
            'OK
            Me.TotalFacturasProveedorDevolucion()


            Me.mTextDebug.Text = "Devoluvión  Calculando Gastos por Departamento Facturas"
            Me.mTextDebug.Update()
            'OK
            'Me.GastosPorCentrodeCostoFacturasDevolucion()


            Me.mTextDebug.Text = "Devoluvión Calculando Impuesto por Facturas Directas"
            Me.mTextDebug.Update()
            'OK
            'Me.TotalFacturasProveedorImpuestoDevolucion()

            Me.mProgresBar.Value = 60
            Me.mProgresBar.Update()
            Me.mControlForm.Update()
            ' ---------------------------------------------------------------
            ' Asiento de Facturas que formalizan Albaranes Directas 6
            '----------------------------------------------------------------
            Me.mTextDebug.Text = "Facturas Formaliza Albaranes Proveedor"
            Me.mTextDebug.Update()


            Me.TotalFacturasProveedorFormalizadas()


            Me.mTextDebug.Text = "Calculando Impuesto por Facturas Formalizadas"
            Me.mTextDebug.Update()
            '''''

            If Me.mCondensarAsiento = True Then
                Me.TotalFacturasProveedorImpuestoFormalizadas()
            End If




            Me.mTextDebug.Text = "Calculando Albaranes Formalizados"
            Me.mTextDebug.Update()
            'Me.TotalAlbaranesProveedorFormalizados()

            Me.mProgresBar.Value = 70
            Me.mProgresBar.Update()
            Me.mControlForm.Update()



            ' ---------------------------------------------------------------
            ' Asiento de Traspasos Internos 2
            '----------------------------------------------------------------

            Me.mTextDebug.Text = "Calculando Salidas por Traspaso"
            Me.mTextDebug.Update()
            'OK
            Me.TraspasosSalidas()

            Me.mTextDebug.Text = "Calculando Entradas por Traspaso"
            Me.mTextDebug.Update()
            ' OK
            Me.TraspasosEntradas()

            Me.mProgresBar.Value = 80
            Me.mProgresBar.Update()
            Me.mControlForm.Update()


            ' ---------------------------------------------------------------
            ' Asiento de Facturas Directas NEWPAGA 40
            '----------------------------------------------------------------
            If Me.mConectaNewPaga = True Then
                Me.mTextDebug.Text = "Facturas Directas Proveedor"
                Me.mTextDebug.Update()
                Me.TotalFacturasProveedorNewPaga()


                Me.mTextDebug.Text = "Calculando Gastos por Departamento Facturas"
                Me.mTextDebug.Update()
                '' FALTA DESARROLLO
                Me.GastosPorCentrodeCostoFacturasNewPaga()


                Me.mTextDebug.Text = "Calculando Impuesto por Facturas Directas"
                Me.mTextDebug.Update()
                '' FALTA DESARROLLO
                Me.TotalFacturasProveedorImpuestoNewPaga()
            End If



            Me.mProgresBar.Value = 100
            Me.mProgresBar.Update()
            Me.mControlForm.Update()



            'Dim spyro As New SPYRO.C_SPYRO(Me.mStrConexionCentral, Me.mListBoxDebug, Me.mControlForm)
            'spyro.ValidaCuentasAlmacen(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, CDate(Format(Me.mFecha, "dd/MM/yyyy")))

            Me.SpyroCompruebaCuentas()

            Me.SpyroCompruebaFacturas()

            Me.CerrarFichero()
            Me.CierraConexiones()
            Me.mTextDebug.Text = "Fin de Integración"
            Me.mTextDebug.Update()


        Catch EX As Exception
            MsgBox(EX.Message)
        End Try

    End Sub
    Public Sub ProcesarInventario()
        Try

            If Me.ControlCentrosdeCosto > 0 Then
                MsgBox("Existen Departamentos sin Centro de Costo", MsgBoxStyle.Information, "Atención")
                Exit Sub
            End If


            ' ---------------------------------------------------------------
            ' Asiento de Inventario Final 7
            '----------------------------------------------------------------
            Me.mTextDebug.Text = "Inventario Final "
            Me.mTextDebug.Update()
            If Me.mSoloInventariosIniciales = False Then
                Me.InventariosFinal()
                Me.CerrarFichero()
            End If

            ' ---------------------------------------------------------------
            ' Asiento de Inventario Inicial 8
            '----------------------------------------------------------------
            Me.mTextDebug.Text = "Inventario Inicial "
            Me.mTextDebug.Update()
            Me.InventariosInicial()
            Me.CerrarFichero()
            ' ---------------------------------------------------------------
            ' FIN DE PROCESO 
            '----------------------------------------------------------------
            Me.CierraConexiones()
            Me.mTextDebug.Text = "Fin de Integración"
            Me.mTextDebug.Update()


        Catch EX As Exception
            MsgBox(EX.Message)
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
            Me.DbLeeHotelAux2.CerrarConexion()
            Me.DbLeeHotelAux3.CerrarConexion()
            'Me.DbLeeHotelAux4.CerrarConexion()
            'Me.DbLeeHotelAux5.CerrarConexion()
            'Me.DbLeeHotelAux6.CerrarConexion()

            If mConectarNewCentral = 1 Then
                Me.DbNewCentral.CerrarConexion()
            End If


            If IsNothing(Me.DbLeeNewPaga) = False Then
                If Me.DbLeeNewPaga.EstadoConexion = ConnectionState.Open Then
                    Me.DbLeeNewPaga.CerrarConexion()
                End If
            End If
            'Me.DbSpyro.CerrarConexion()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub GastosPorCentrodeCostoAlbaranesAlmacenDocumento(ByVal vMovi As Integer, ByVal vAnci As Integer, ByVal vTexto As String)
        Dim Total As Double
        Dim vCentroCosto As String
        Dim Proveedor As Integer
        Dim ProveedorNombre As String
        Try

            '3X'

            SQL = "SELECT TNST_MOVG.MOVG_DAVA,"
            SQL += "TNST_MOVD.ALMA_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,"
            SQL += " TNST_MOVD.ALMA_CODI AS ALMACODI,TNST_TIMO.TIMO_TIPO AS TIMO_TIPO,FAMI_DESC AS FAMILIA,"
            SQL += "TNST_FAMI.FAMI_CODI AS FAMICODI,NVL(TNST_MOVG.MOVG_IDDO,'0') AS DOCUMENTO "
            SQL += ", TNST_MOVG.MOVG_ORIG AS ORIGEN , TNST_MOVG.MOVG_DEST AS DESTINO "
            SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_TIMO,TNST_FAMI "
            SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
            SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
            SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
            SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
            SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"

            SQL += " AND (TNST_PROD.FAMI_CODI = TNST_FAMI.FAMI_CODI)"
            SQL += " AND (TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Albaran
            SQL += " OR  TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Albaran_Dev & ")"

            ' AQUI ESTA LINEA SOBRA LOS ALBARANES PUEDEN TENER OTRA FECHA INFERIOR A LA FACTURA
            'SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"

            SQL += " AND TNST_MOVG.MOVG_CODI = " & vMovi
            SQL += " AND TNST_MOVG.MOVG_ANCI = " & vAnci
            SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
            SQL += " GROUP BY TNST_TIMO.TIMO_TIPO,TNST_MOVG.MOVG_DAVA,"
            SQL += "TNST_MOVD.ALMA_CODI,"
            SQL += "TNST_ALMA.ALMA_DESC,FAMI_DESC,TNST_FAMI.FAMI_CODI,TNST_MOVG.MOVG_IDDO,TNST_MOVG.MOVG_ORIG,TNST_MOVG.MOVG_DEST"

            Me.DbLeeHotelAux3.TraerLector(SQL)

            While Me.DbLeeHotelAux3.mDbLector.Read

                SQL = "SELECT  NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer)

                vCentroCosto = Me.DbLeeHotelAux4.EjecutaSqlScalar(SQL)

                Linea = Linea + 1
                Total = CType(Me.DbLeeHotelAux3.mDbLector("TOTAL"), Double)


                ' SI LAS CUENTAS ESTAN EN TNST_CTA
                'Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaConsumoInterno(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorDebe, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("GRUPO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total, "NO", "0", "")
                'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaConsumoInterno(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorDebe, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("GRUPO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total)
                'Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Me.BuscaCuentaConsumoInterno(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), "0", vCentroCosto, Total)

                ' SI LAS CUENTAS SE COMPONEN 

                If CInt(Me.DbLeeHotelAux3.mDbLector("TIMO_TIPO")) = Me.mTimo_Albaran Then
                    Proveedor = CInt(Me.DbLeeHotelAux3.mDbLector("ORIGEN"))
                    SQL = "SELECT NVL(FORN_DESC,'?') FROM TNST_FORN WHERE FORN_CODI = " & Proveedor
                    ProveedorNombre = Me.DbLeeHotelAux4.EjecutaSqlScalar(SQL)
                    Me.mTipoAsiento = "DEBE"
                    If Me.mCondensarAsiento = True Then
                        Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorDebe, " -- " & CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String) & "," & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & "," & ProveedorNombre, Total, "NO", CType(Me.DbLeeHotelAux3.mDbLector("DOCUMENTO"), String), "", CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI")), "", "", 0, "", 0, "", "", 0)
                        'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI"))), CType(Now.Year, String), Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorDebe, CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String) & "," & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & "," & ProveedorNombre, Total)
                        'Me.GeneraFileAA("AA", 6, Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI"))), CType(Now.Year, String), 1, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), "0", vCentroCosto, Total)
                    Else
                        Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorDebe, " -- " & CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String) & "," & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & "," & ProveedorNombre, Total, "NO", CType(Me.DbLeeHotelAux3.mDbLector("DOCUMENTO"), String), "", CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI")), "", "", 0, "", 0, "", "", 0)
                        'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI"))), CType(Now.Year, String), Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorDebe, vTexto, Total)
                        'Me.GeneraFileAA("AA", 6, Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI"))), CType(Now.Year, String), 1, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), "0", vCentroCosto, Total)

                    End If
                ElseIf CInt(Me.DbLeeHotelAux3.mDbLector("TIMO_TIPO")) = Me.mTimo_Albaran_Dev Then
                    Proveedor = CInt(Me.DbLeeHotelAux3.mDbLector("DESTINO"))
                    SQL = "SELECT NVL(FORN_DESC,'?') FROM TNST_FORN WHERE FORN_CODI = " & Proveedor
                    ProveedorNombre = Me.DbLeeHotelAux4.EjecutaSqlScalar(SQL)
                    Me.mTipoAsiento = "HABER"
                    If Me.mCondensarAsiento = True Then
                        Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorHaber, " -- " & CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String) & "," & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & "," & ProveedorNombre, Total, "NO", CType(Me.DbLeeHotelAux3.mDbLector("DOCUMENTO"), String), "", CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI")), "", "", 0, "", 0, "", "", 0)
                        'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI"))), CType(Now.Year, String), Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorHaber, CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String) & "," & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & "," & ProveedorNombre, Total)
                        'Me.GeneraFileAA("AA", 6, Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI"))), CType(Now.Year, String), 1, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), "0", vCentroCosto, Total)
                    Else
                        Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorHaber, " -- " & CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String) & "," & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & "," & ProveedorNombre, Total, "NO", CType(Me.DbLeeHotelAux3.mDbLector("DOCUMENTO"), String), "", CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI")), "", "", 0, "", 0, "", "", 0)
                        'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI"))), CType(Now.Year, String), Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorHaber, vTexto, Total)
                        'Me.GeneraFileAA("AA", 6, Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI"))), CType(Now.Year, String), 1, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), "0", vCentroCosto, Total)

                    End If
                End If


            End While
            Me.DbLeeHotelAux3.mDbLector.Close()
        Catch ex As Exception
            MsgBox(" GastosPorCentrodeCostoAlbaranesAlmacenDocumento" & vbCrLf & ex.Message, MsgBoxStyle.Information, "Atención")
        End Try
    End Sub

    Private Sub GastosPorCentrodeCostoAlbaranesFamilia(ByVal vDore As String, ByVal vTexto As String)
        Dim Total As Double
        Dim vCentroCosto As String
        Dim Proveedor As Integer
        Dim ProveedorNombre As String
        Try

            '3X'

            SQL = "SELECT "
            SQL += "TNST_MOVD.ALMA_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,"
            SQL += " TNST_MOVD.ALMA_CODI AS ALMACODI,TNST_TIMO.TIMO_TIPO AS TIMO_TIPO "

            SQL += ", TNST_MOVG.MOVG_ORIG AS ORIGEN , TNST_MOVG.MOVG_DEST AS DESTINO,TNST_MOVG.MOVG_IDDO AS DOC "
            SQL += " ,TNST_PROD.PROD_CODI AS PRODUCTO,TNST_PROD.PROD_DESC"
            SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_TIMO "
            SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
            SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
            SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
            SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
            SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"

            SQL += " AND (TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Albaran
            SQL += " OR  TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Albaran_Dev & ")"

            ' AQUI ESTA LINEA SOBRA LOS ALBARANES PUEDEN TENER OTRA FECHA INFERIOR A LA FACTURA
            'SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"

            SQL += " AND TNST_MOVG.MOVG_DORE = '" & vDore & "'"

            SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
            SQL += " GROUP BY TNST_TIMO.TIMO_TIPO,"
            SQL += "TNST_MOVD.ALMA_CODI,"
            SQL += "TNST_ALMA.ALMA_DESC,"
            SQL += "TNST_MOVG.MOVG_ORIG,TNST_MOVG.MOVG_DEST"
            SQL += ",TNST_MOVG.MOVG_IDDO"
            SQL += ",TNST_PROD.PROD_CODI,TNST_PROD.PROD_DESC "

            SQL += " ORDER BY TNST_MOVG.MOVG_IDDO,TNST_PROD.PROD_CODI"

            Me.DbLeeHotelAux3.TraerLector(SQL)

            While Me.DbLeeHotelAux3.mDbLector.Read

                SQL = "SELECT  NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer)

                vCentroCosto = Me.DbLeeHotelAux4.EjecutaSqlScalar(SQL)

                Linea = Linea + 1
                Total = CType(Me.DbLeeHotelAux3.mDbLector("TOTAL"), Double)





                If CInt(Me.DbLeeHotelAux3.mDbLector("TIMO_TIPO")) = Me.mTimo_Albaran Then
                    Proveedor = CInt(Me.DbLeeHotelAux3.mDbLector("ORIGEN"))
                    SQL = "SELECT NVL(FORN_DESC,'?') FROM TNST_FORN WHERE FORN_CODI = " & Proveedor
                    ProveedorNombre = Me.DbLeeHotelAux4.EjecutaSqlScalar(SQL)
                    Me.mTipoAsiento = "DEBE"
                    If Me.mCondensarAsiento = True Then
                        Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaGastosProducto(Me.DbLeeHotelAux3.mDbLector("PRODUCTO")), Me.mIndicadorDebe, " -- " & CType("GASTOS", String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("PROD_DESC"), String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String), Total, "NO", CType(Me.DbLeeHotelAux3.mDbLector("DOC"), String), "", CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI")), "", "", 0, "", 0, "", "", 0)
                    Else
                        Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaGastosProducto(Me.DbLeeHotelAux3.mDbLector("PRODUCTO")), Me.mIndicadorDebe, " -- " & CType("GASTOS", String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("PROD_DESC"), String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String), Total, "NO", CType(Me.DbLeeHotelAux3.mDbLector("DOC"), String), "", CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI")), "", "", 0, "", 0, "", "", 0)

                    End If
                ElseIf CInt(Me.DbLeeHotelAux3.mDbLector("TIMO_TIPO")) = Me.mTimo_Albaran_Dev Then
                    Proveedor = CInt(Me.DbLeeHotelAux3.mDbLector("DESTINO"))
                    SQL = "SELECT NVL(FORN_DESC,'?') FROM TNST_FORN WHERE FORN_CODI = " & Proveedor
                    ProveedorNombre = Me.DbLeeHotelAux4.EjecutaSqlScalar(SQL)
                    Me.mTipoAsiento = "HABER"
                    If Me.mCondensarAsiento = True Then
                        Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaGastosProducto(Me.DbLeeHotelAux3.mDbLector("PRODUCTO")), Me.mIndicadorHaber, " -- " & CType("GASTOS", String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("PROD_DESC"), String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String), Total, "NO", CType(Me.DbLeeHotelAux3.mDbLector("DOC"), String), "", CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI")), "", "", 0, "", 0, "", "", 0)
                    Else
                        Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaGastosProducto(Me.DbLeeHotelAux3.mDbLector("PRODUCTO")), Me.mIndicadorHaber, " -- " & CType("GASTOS", String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("PROD_DESC"), String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String), Total, "NO", CType(Me.DbLeeHotelAux3.mDbLector("DOC"), String), "", CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI")), "", "", 0, "", 0, "", "", 0)

                    End If
                End If


            End While
            Me.DbLeeHotelAux3.mDbLector.Close()
        Catch ex As Exception
            MsgBox("GastosPorCentrodeCostoAlbaranes" & vbCrLf & ex.Message, MsgBoxStyle.Information, "Atención")
        End Try
    End Sub


    Private Sub GastosPorCentrodeCostoAlbaranesFacturaGuia()
        Dim Total As Double
        Dim vCentroCosto As String

        Dim Proveedor As Integer
        Dim ProveedorNombre As String
        Try

            '3A'

            SQL = "SELECT   TNST_MOVG.MOVG_DAVA, TNST_MOVD.ALMA_CODI, "
            SQL += "         SUM (TNST_MOVD.MOVD_TOTA) AS TOTAL, ALMA_DESC AS ALMACEN, "
            SQL += "         TNST_MOVD.ALMA_CODI AS ALMACODI, TNST_TIMO.TIMO_TIPO AS TIMO_TIPO, "
            SQL += "         FAMI_DESC AS FAMILIA, TNST_FAMI.FAMI_CODI AS FAMICODI, "

            If Me.mTipoGasto = "0" Then
                SQL += "         NVL (TNST_MOVG.MOVG_IDDO, '0') AS DOCUMENTO, "
            End If

            SQL += "         NVL (TNST_MOVG.MOVG_DORE, '0') AS DOCUMENTOF, "
            SQL += "         TO_NUMBER (SUBSTR (MOVG_DORE, 1, INSTR (MOVG_DORE, '/') - 1)) DORE_CODI, "
            SQL += "         TO_NUMBER (SUBSTR (MOVG_DORE, INSTR (MOVG_DORE, '/') + 1, 4)) DORE_ANCI "

            SQL += " ,MOVG_ORIG AS ORIGEN"

            SQL += "    FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_TIMO, TNST_FAMI "
            SQL += "   WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI) "
            SQL += "     AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI) "
            SQL += "     AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI) "
            SQL += "     AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI) "
            SQL += "     AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI) "
            SQL += "     AND (TNST_PROD.FAMI_CODI = TNST_FAMI.FAMI_CODI) "
            SQL += "     AND (TNST_TIMO.TIMO_TIPO = 3 OR TNST_TIMO.TIMO_TIPO = 29) "
            SQL += "     AND TNST_MOVG.MOVG_ANUL = 0 "
            SQL += "AND ( "
            SQL += "  TO_NUMBER (SUBSTR (MOVG_DORE, 1, INSTR (MOVG_DORE, '/') - 1)), "
            SQL += "  TO_NUMBER (SUBSTR (MOVG_DORE, INSTR (MOVG_DORE, '/') + 1, 4))) "
            SQL += "  "
            SQL += "IN ( "
            SQL += "SELECT    TNST_MOVG.MOVG_CODI , TNST_MOVG.MOVG_ANCI "
            SQL += "          "
            SQL += "    FROM TNST_MOVG, TNST_TIMO, TNST_FORN, TNST_MOVI "
            SQL += "   WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
            SQL += "     AND (   TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI "
            SQL += "          OR TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI "
            SQL += "         ) "
            SQL += "     AND TIMO_TIPO = 2 "
            SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"

            SQL += "     AND TNST_MOVG.MOVG_ANUL = 0 "
            SQL += "     AND TNST_MOVG.MOVG_CODI = TNST_MOVI.MOVG_CODI "
            SQL += "     AND TNST_MOVG.MOVG_ANCI = TNST_MOVI.MOVG_ANCI "

            ' para que no coga las facturas que contienen solo devoluciones ( )
            SQL += " AND TNST_MOVG.MOVG_ORIG <> TNST_MOVG.MOVG_DEST"

            SQL += "   )           "
            SQL += "GROUP BY TNST_TIMO.TIMO_TIPO, "
            SQL += "         TNST_MOVG.MOVG_DAVA, "
            SQL += "         TNST_MOVD.ALMA_CODI, "
            SQL += "         TNST_ALMA.ALMA_DESC, "
            SQL += "         FAMI_DESC, "
            SQL += "         TNST_FAMI.FAMI_CODI, "

            If Me.mTipoGasto = "0" Then
                SQL += "         TNST_MOVG.MOVG_IDDO, "
            End If

            SQL += "         TNST_MOVG.MOVG_DORE "

            SQL += "        , TNST_MOVG.MOVG_ORIG "




            ' RECORER DATASET EN VEZ DE LECTOR POR VELOCIDAD 
            Me.DbLeeHotelAux3.TraerDataset(SQL, "ALBARANES")

            For Each r As DataRow In Me.DbLeeHotelAux3.mDbDataset.Tables(0).Rows

                If Me.mConectarNewCentral = 0 Then
                    SQL = "SELECT  NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(r("ALMACODI"), Integer)
                    vCentroCosto = Me.DbLeeHotelAux4.EjecutaSqlScalar(SQL)
                Else
                    SQL = "SELECT  NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNCC_ALMA WHERE ALMA_CODI = " & CType(r("ALMACODI"), Integer)
                    SQL += " AND HOTE_CODI = " & Me.mHotelNewCentral
                    vCentroCosto = Me.DbNewCentral.EjecutaSqlScalar(SQL)

                    If IsNothing(vCentroCosto) = True Then
                        vCentroCosto = "?"
                    End If
                End If




                If Me.mConectarNewCentral = 0 Then
                    Proveedor = CInt(r("ORIGEN"))
                    SQL = "SELECT NVL(FORN_DESC,'?') FROM TNST_FORN WHERE FORN_CODI = " & Proveedor
                    ProveedorNombre = Me.DbLeeHotelAux4.EjecutaSqlScalar(SQL)
                Else
                    Proveedor = CInt(r("ORIGEN"))
                    SQL = "SELECT NVL(FORN_DESC,'?') FROM TNCC_FORN WHERE FORN_CODI = " & Proveedor
                    SQL += " AND HOTE_CODI = " & Me.mHotelNewCentral
                    ProveedorNombre = Me.DbNewCentral.EjecutaSqlScalar(SQL)
                End If


                Linea = Linea + 1
                Total = CType(r("TOTAL"), Double)

                ' SI LAS CUENTAS SE COMPONEN 

                If CInt(r("TIMO_TIPO")) = Me.mTimo_Albaran Then
                    Me.mTipoAsiento = "DEBE"
                    If Me.mTipoGasto = "0" Then
                        Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo6(CType(r("ALMACODI"), Integer), CType(r("FAMICODI"), Integer)), Me.mIndicadorDebe, CType(r("ALMACEN"), String) & " " & CType(r("FAMILIA"), String) & " + " & ProveedorNombre, Total, "NO", CType(r("DOCUMENTO"), String), "", CInt(r("ALMACODI")), "", "", 0, "", 0, "", "", 0)
                        'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(r("ALMACODI"))), CType(Now.Year, String), Me.ComponeCuentaGastoGrupo6(CType(r("ALMACODI"), Integer), CType(r("FAMICODI"), Integer)), Me.mIndicadorDebe, CType(r("ALMACEN"), String) & " " & CType(r("FAMILIA"), String) & " " & ProveedorNombre, Total)
                    Else
                        ' POR FAMILIA
                        Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo6(CType(r("ALMACODI"), Integer), CType(r("FAMICODI"), Integer)), Me.mIndicadorDebe, CType(r("ALMACEN"), String) & " " & CType(r("FAMILIA"), String) & " + " & ProveedorNombre, Total, "NO", "*", "", CInt(r("ALMACODI")), "", "", 0, "", 0, "", "", 0)
                        'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(r("ALMACODI"))), CType(Now.Year, String), Me.ComponeCuentaGastoGrupo6(CType(r("ALMACODI"), Integer), CType(r("FAMICODI"), Integer)), Me.mIndicadorDebe, CType(r("ALMACEN"), String) & " " & CType(r("FAMILIA"), String) & " " & ProveedorNombre, Total)
                    End If
                    'Me.GeneraFileAA("AA", 6, Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(r("ALMACODI"))), CType(Now.Year, String), 1, Me.ComponeCuentaGastoGrupo6(CType(r("ALMACODI"), Integer), CType(r("FAMICODI"), Integer)), "0", vCentroCosto, Total)
                ElseIf CInt(r("TIMO_TIPO")) = Me.mTimo_Albaran_Dev Then
                    Me.mTipoAsiento = "HABER"
                    If Me.mTipoGasto = "0" Then
                        Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo6(CType(r("ALMACODI"), Integer), CType(r("FAMICODI"), Integer)), Me.mIndicadorHaber, CType(r("ALMACEN"), String) & " " & CType(r("FAMILIA"), String) & " + " & ProveedorNombre, Total, "NO", CType(r("DOCUMENTO"), String), "", CInt(r("ALMACODI")), "", "", 0, "", 0, "", "", 0)
                        'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(r("ALMACODI"))), CType(Now.Year, String), Me.ComponeCuentaGastoGrupo6(CType(r("ALMACODI"), Integer), CType(r("FAMICODI"), Integer)), Me.mIndicadorHaber, CType(r("ALMACEN"), String) & " " & CType(r("FAMILIA"), String) & " " & ProveedorNombre, Total)
                    Else
                        Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo6(CType(r("ALMACODI"), Integer), CType(r("FAMICODI"), Integer)), Me.mIndicadorHaber, CType(r("ALMACEN"), String) & " " & CType(r("FAMILIA"), String) & " +  " & ProveedorNombre, Total, "NO", "*", "", CInt(r("ALMACODI")), "", "", 0, "", 0, "", "", 0)
                        'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(r("ALMACODI"))), CType(Now.Year, String), Me.ComponeCuentaGastoGrupo6(CType(r("ALMACODI"), Integer), CType(r("FAMICODI"), Integer)), Me.mIndicadorHaber, CType(r("ALMACEN"), String) & " " & CType(r("FAMILIA"), String) & " " & ProveedorNombre, Total)
                    End If
                    'Me.GeneraFileAA("AA", 6, Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(r("ALMACODI"))), CType(Now.Year, String), 1, Me.ComponeCuentaGastoGrupo6(CType(r("ALMACODI"), Integer), CType(r("FAMICODI"), Integer)), "0", vCentroCosto, Total)
                End If




            Next

            Me.DbLeeHotelAux3.mDbDataset.Clear()









            'Me.DbLeeHotelAux3.TraerLector(SQL)

            'While Me.DbLeeHotelAux3.mDbLector.Read

            'SQL = "SELECT  NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer)

            'vCentroCosto = Me.DbLeeHotelAux4.EjecutaSqlScalar(SQL)


            'Proveedor = CInt(Me.DbLeeHotelAux3.mDbLector("ORIGEN"))
            'SQL = "SELECT NVL(FORN_DESC,'?') FROM TNST_FORN WHERE FORN_CODI = " & Proveedor
            'ProveedorNombre = Me.DbLeeHotelAux4.EjecutaSqlScalar(SQL)

            'Linea = Linea + 1
            'Total = CType(Me.DbLeeHotelAux3.mDbLector("TOTAL"), Double)

            ' SI LAS CUENTAS SE COMPONEN 

            'If CInt(Me.DbLeeHotelAux3.mDbLector("TIMO_TIPO")) = Me.mTimo_Albaran Then
            'Me.mTipoAsiento = "DEBE"
            'If Me.mTipoGasto = "0" Then
            'Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorDebe, CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & " + " & ProveedorNombre, Total, "NO", CType(Me.DbLeeHotelAux3.mDbLector("DOCUMENTO"), String), "", CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI")))
            'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI"))), CType(Now.Year, String), Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorDebe, CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & " " & ProveedorNombre, Total)
            'Else
            ' POR FAMILIA
            'Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorDebe, CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & " + " & ProveedorNombre, Total, "NO", "*", "", CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI")))
            'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI"))), CType(Now.Year, String), Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorDebe, CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & " " & ProveedorNombre, Total)
            'End If
            'Me.GeneraFileAA("AA", 6, Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI"))), CType(Now.Year, String), 1, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), "0", vCentroCosto, Total)
            'ElseIf CInt(Me.DbLeeHotelAux3.mDbLector("TIMO_TIPO")) = Me.mTimo_Albaran_Dev Then
            'Me.mTipoAsiento = "HABER"
            'If Me.mTipoGasto = "0" Then
            'Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorHaber, CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & " + " & ProveedorNombre, Total, "NO", CType(Me.DbLeeHotelAux3.mDbLector("DOCUMENTO"), String), "", CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI")))
            'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI"))), CType(Now.Year, String), Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorHaber, CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & " " & ProveedorNombre, Total)
            'Else
            'Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorHaber, CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & " +  " & ProveedorNombre, Total, "NO", "*", "", CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI")))
            'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI"))), CType(Now.Year, String), Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorHaber, CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & " " & ProveedorNombre, Total)
            'End If
            'Me.GeneraFileAA("AA", 6, Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux3.mDbLector("ALMACODI"))), CType(Now.Year, String), 1, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), "0", vCentroCosto, Total)
            'End If


            'End While
            'Me.DbLeeHotelAux3.mDbLector.Close()


        Catch ex As Exception
            MsgBox("GastosPorCentrodeCostoAlbaranesFacturaGuia" & vbCrLf & ex.Message, MsgBoxStyle.Information, "Atención")
        End Try
    End Sub
#End Region
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, ByVal vAjuste As String, ByVal vDocu As String, ByVal vDore As String)
        Try

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TS_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_DOCU,ASNT_DORE,ASNT_EMP_NUM) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto.Replace("'", ","), 1, 40) & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            '   SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "',"
            SQL += "'?'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & Mid(vDocu, 1, 15) & "','" & Mid(vDore, 1, 15) & "'," & Me.mEmpNum & ")"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40)
            Me.mTextDebug.Update()


            Me.Contador = Me.Contador + 1
            Me.mControlForm.ParentForm.Text = "Menu Procesando ...  " & Contador
            Me.mControlForm.Update()
            System.Windows.Forms.Application.DoEvents()

            If vCfcta_Cod.Length < 2 Then
                '       Me.mListBoxDebug.Items.Add("NEWSTOCK: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40))
                '      Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40))

            End If
            '    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)

        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, ByVal vAjuste As String, ByVal vDocu As String, ByVal vDore As String, ByVal vAlmaCodi As Integer _
                                     , vProveedorCext As String, vAlmacenExt As String, vCantidad As Decimal, vUnidadMedidaCext As String, vPrecioUnidad As Decimal, vDocumentoNewStock As String, vProducto As String, vAxStatus As Integer)
        Try


            '     MsgBox("Ojo falta ampliar la INSERT campos nuevos")

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TS_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,ASNT_DEBE,ASNT_HABER,"
            SQL += "ASNT_AJUSTAR,ASNT_DOCU,ASNT_DORE,ASNT_EMP_NUM,ASNT_ALMA_CODI,ASNT_ALMA_DESC,ASNT_MORA_PRODUCTO,ASNT_MORA_CODALMACEN,ASNT_MORA_CANTIDAD,"
            SQL += "ASNT_MORA_UMEDIDA,ASNT_MORA_COSTE,ASNT_MORA_DOCEXTERNO,ASNT_MORA_PROVEEDOR,ASNT_AX_STATUS) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto.Replace("'", ","), 1, 40) & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            '   SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "',"
            SQL += "'?'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & Mid(vDocu, 1, 15) & "','" & Mid(vDore, 1, 15) & "'," & Me.mEmpNum & ","
            SQL += vAlmaCodi & ",'" & Me.DameNombreAlmacen(vAlmaCodi) & "','" & vProducto & "','" & vAlmacenExt & "','" & vCantidad & "','" & vUnidadMedidaCext
            SQL += "'," & vPrecioUnidad & ",'" & vDocumentoNewStock & "','" & vProveedorCext & "'," & vAxStatus & ")"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40)
            Me.mTextDebug.Update()


            Me.Contador = Me.Contador + 1
            Me.mControlForm.ParentForm.Text = "Menu Procesando ...  " & Contador
            Me.mControlForm.Update()
            System.Windows.Forms.Application.DoEvents()

            If vCfcta_Cod.Length < 2 Then
                '       Me.mListBoxDebug.Items.Add("NEWSTOCK: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40))
                '      Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40))

            End If
            '    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)

        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
                                     ByVal vAjuste As String, ByVal vDocu As String, ByVal vDore As String, ByVal vAuxString As String, ByVal vAuxString2 As String, ByVal vFechaValor As Date)
        Try

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TS_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_DOCU,ASNT_DORE,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_AUXILIAR_STRING2) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto.Replace("'", ","), 1, 40) & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(vFechaValor, "dd/MM/yyyy") & "','"
            'SQL += Format(Me.mFecha, "dd/MM/yyyy") & "',"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "',"
            SQL += "'?'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & Mid(vDocu, 1, 15) & "','" & Mid(vDore, 1, 15) & "'," & Me.mEmpNum & ",'" & vAuxString & "','" & vAuxString2 & "')"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40)
            Me.mTextDebug.Update()


            Me.Contador = Me.Contador + 1
            Me.mControlForm.ParentForm.Text = "Menu Procesando ...  " & Contador
            Me.mControlForm.Update()
            System.Windows.Forms.Application.DoEvents()

            If vCfcta_Cod.Length < 2 Then
                '       Me.mListBoxDebug.Items.Add("NEWSTOCK: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40))
                '      Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40))
            End If
            '    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)

        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
                                     ByVal vAjuste As String, ByVal vDocu As String, ByVal vDore As String, ByVal vAuxString As String, ByVal vAuxString2 As String, ByVal vFechaValor As Date, ByVal vAlmacodi As Integer)
        Try

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TS_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_DOCU,ASNT_DORE,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_AUXILIAR_STRING2,ASNT_ALMA_CODI,ASNT_ALMA_DESC) values ('"
            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.mCfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto.Replace("'", ","), 1, 40) & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(vFechaValor, "dd/MM/yyyy") & "','"
            'SQL += Format(Me.mFecha, "dd/MM/yyyy") & "',"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "',"
            SQL += "'?'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & Mid(vDocu, 1, 15) & "','" & Mid(vDore, 1, 15) & "'," & Me.mEmpNum & ",'" & vAuxString & "','" & vAuxString2 & "'," & vAlmacodi & ",'" & Me.DameNombreAlmacen(vAlmacodi) & "')"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40)
            Me.mTextDebug.Update()


            Me.Contador = Me.Contador + 1
            Me.mControlForm.ParentForm.Text = "Menu Procesando ...  " & Contador
            Me.mControlForm.Update()
            System.Windows.Forms.Application.DoEvents()

            If vCfcta_Cod.Length < 2 Then
                '       Me.mListBoxDebug.Items.Add("NEWSTOCK: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40))
                '      Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40))
            End If
            '    Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)

        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub
    Private Sub InsertaOracleInventarios(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, ByVal vAjuste As String, ByVal vDocu As String, ByVal vDore As String, ByVal vFechaInventario As Date)
        Try

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TS_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_DOCU,ASNT_DORE,ASNT_EMP_NUM) values ('"
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
            SQL += Format(vFechaInventario, "dd/MM/yyyy") & "',"
            SQL += "'?'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vDocu & "','" & vDore & "'," & Me.mEmpNum & ")"



            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  "
            Me.mTextDebug.Update()

            If vCfcta_Cod.Length < 2 Then
                Me.mListBoxDebug.Items.Add("NEWSTOCK: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40))
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40))

            End If

            'Me.SpyroCompruebaCuenta(vCfcta_Cod, vTipo, vAsiento, vLinea, vCfcptos_Cod)

        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub

    Private Function DameNombreAlmacen(ByVal vAlmacodi As Integer) As String
        Try
            SQL = "SELECT NVL(ALMA_DESC,'?') AS ALMA_DESC  FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacodi

            '    SQL = "SELECT NVL(RPAD(ALMA_DESC,30,' ') || '                   Dígitos de Control =  (' || ALMA_CCST  || ')','?') AS ALMA_DESC  FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacodi



            Return Me.DbLeeHotelAux3.EjecutaSqlScalar(SQL)

        Catch ex As Exception
            Return " "
        End Try
    End Function
    Private Sub SpyroCompruebaCuentas()
        Try
            SQL = "SELECT DISTINCT ASNT_CFCTA_COD,ASNT_TIPO_REGISTRO,ASNT_CFCPTOS_COD FROM TS_ASNT WHERE "
            SQL += "     ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum
            SQL += " AND ASNT_F_ATOCAB = '" & Me.mFecha & "'"
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

    Private Sub SpyroCompruebaFacturas()
        Try
            SQL = "SELECT DISTINCT NVL(ASNT_DOCU,'?') AS ASNT_DOCU ,ASNT_I_MONEMP FROM TS_ASNT WHERE "
            SQL += "     ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum
            '  SQL += " AND ASNT_F_ATOCAB = '" & Me.mFecha & "'"
            SQL += " AND ASNT_F_VALOR = '" & Me.mFecha & "'"

            SQL += " AND ASNT_CFCPTOS_COD IN('" & Me.mIndicadorDebeFac & "','" & Me.mIndicadorHaberFac & "')"



            Me.DbLeeCentral.TraerLector(SQL)
            While Me.DbLeeCentral.mDbLector.Read


                Me.mTextDebug.Text = "Validando existencia de Factura ya Contabilizada " & CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_DOCU"))
                Me.mTextDebug.Update()
                SQL = "SELECT N_FACTURA FROM FACTURAS WHERE EMP_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += "  AND CFIVALIBRO_COD = '" & Me.mCfivaLibro_Cod & "'"
                SQL += "  AND FACTUTIPO_COD IN ('" & Me.mParaSeriefacturas & Me.mFecha.Year & "','" & Me.mParaSeriefacturasNewPaga & Me.mFecha.Year & "')"
                SQL += "  AND S_FACTURA = '" & CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_DOCU")) & "'"
                SQL += "  AND I_MONEMP  = " & CDbl(Me.DbLeeCentral.mDbLector.Item("ASNT_I_MONEMP"))



                If Me.DbSpyro.EjecutaSqlScalar(SQL) <> "" Then
                    ' Me.mListBoxDebug.Items.Add("SPYRO   : " & CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_DOCU")) & " Documento Posiblemente Contabilizado ya en Spyro Su Factura + Importe = " & CDbl(Me.DbLeeCentral.mDbLector.Item("ASNT_I_MONEMP")))
                    'Me.mListBoxDebug.Update()
                    Me.mTexto = "SPYRO   : " & CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_DOCU")) & " Documento Posiblemente Contabilizado ya en Spyro Su Factura + Importe = " & CDbl(Me.DbLeeCentral.mDbLector.Item("ASNT_I_MONEMP"))
                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)
                End If


            End While
            Me.DbLeeCentral.mDbLector.Close()





        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub SpyroCompruebaCuenta(ByVal vCuenta As String, ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vLinea As Integer, ByVal vDebeHaber As String)

        Try

            Me.mTextDebug.Text = "Validando Plan de Cuentas Spyro " & vCuenta.PadRight(20, CChar(" ")) & " Longitud : " & vCuenta.Length
            Me.mTextDebug.Update()

            'Me.mControlForm.Refresh()
            'Me.mControlForm.Update()

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

                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)

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
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)
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
                    Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & " No tiene definido Libro de Iva   en Plan de Cuentas de Spyro")
                    Me.mListBoxDebug.Update()
                    Me.mTexto = "SPYRO   : " & vCuenta & " No tiene definido Libro de Iva   en Plan de Cuentas de Spyro"
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
                    Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & " No tiene definido Clase de Iva   en Plan de Cuentas de Spyro")
                    Me.mListBoxDebug.Update()
                    Me.mTexto = "SPYRO   : " & vCuenta & " No tiene definido Clase de Iva   en Plan de Cuentas de Spyro"
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
                    Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & " No tiene definido Forma de pago en Plan de Cuentas de Spyro")
                    Me.mListBoxDebug.Update()
                    Me.mTexto = "SPYRO   : " & vCuenta & " No tiene definido Forma de pago en Plan de Cuentas de Spyro"
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
    Private Sub SpyroCompruebaCuentaSimple(ByVal vCuenta As String, ByVal vTipo As String, ByVal vDebeHaber As String)

        Try

            Me.mTextDebug.Text = "Validando Plan de Cuentas Spyro " & vCuenta.PadRight(20, CChar(" ")) & " Longitud : " & vCuenta.Length
            Me.mTextDebug.Update()

            'Me.mControlForm.Refresh()
            'Me.mControlForm.Update()

            SQL = "SELECT COD FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND COD = '" & vCuenta & "'"



            Me.mResult = Me.DbSpyro.EjecutaSqlScalar(SQL)

            ' If IsNothing(Me.DbSpyro.EjecutaSqlScalar(SQL)) = True Then
            If Me.mResult = "" Or IsNothing(Me.mResult) = True Then
                'Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & " no se localiza en Plan de Cuentas de Spyro")
                'Me.mListBoxDebug.Update()
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
                'Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & " No es una Cuenta de Apuntes Directos en Plan de Cuentas Spyro")
                'Me.mListBoxDebug.Update()
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
                    'Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & " No tiene definida Razón Social  en Plan de Cuentas de Spyro")
                    'Me.mListBoxDebug.Update()
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
                    'Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & " No tiene definido Libro de Iva   en Plan de Cuentas de Spyro")
                    'Me.mListBoxDebug.Update()
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
                    'Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & " No tiene definido Clase de Iva   en Plan de Cuentas de Spyro")
                    'Me.mListBoxDebug.Update()
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
                    'Me.mListBoxDebug.Items.Add("SPYRO   : " & vCuenta & " No tiene definido Forma de pago en Plan de Cuentas de Spyro")
                    'Me.mListBoxDebug.Update()
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

    Private Sub GeneraFileAC(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
    ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double)
        Try


            '-------------------------------------------------------------------------------------------------
            '  Apuntes Contables(CFATOLIN)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Mid(Format(Me.mFecha, "ddMMyyyy"), 5, 4) &
            Me.mCfatodiari_Cod.PadRight(4, CChar(" ")) &
            " ".PadLeft(8, CChar(" ")) &
            " ".PadLeft(4, CChar(" ")) &
            vCfcta_Cod.PadRight(15, CChar(" ")) &
            vCfcptos_Cod.PadRight(4, CChar(" ")) &
            Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) &
            CType(vImonep, String).PadLeft(16, CChar(" ")) &
            "N" & Format(Me.mFecha, "ddMMyyyy") &
            Format(Me.mFecha, "ddMMyyyy") &
            " ".PadRight(40, CChar(" ")) &
            Me.mCfatotip_Cod.PadRight(4, CChar(" ")))

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
        End Try
    End Sub
    Private Sub GeneraFileACInventario(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
   ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, ByVal vFechaInventario As Date)
        Try


            '-------------------------------------------------------------------------------------------------
            '  Apuntes Contables(CFATOLIN)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Mid(vCefejerc_Cod, 1, 4) &
            Me.mCfatodiari_Cod.PadRight(4, CChar(" ")) &
            " ".PadLeft(8, CChar(" ")) &
            " ".PadLeft(4, CChar(" ")) &
            vCfcta_Cod.PadRight(15, CChar(" ")) &
            vCfcptos_Cod.PadRight(4, CChar(" ")) &
            Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) &
            CType(vImonep, String).PadLeft(16, CChar(" ")) &
            "N" & Format(vFechaInventario, "ddMMyyyy") &
            Format(Me.mFecha, "ddMMyyyy") &
            " ".PadRight(40, CChar(" ")) &
            Me.mCfatodiari_Cod_Inv.PadRight(4, CChar(" ")))

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
        End Try
    End Sub
    Private Sub GeneraFileAC2(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, ByVal vFactuTipo_cod As String, ByVal vNfactura As String)
        Try
            'Dim FechaAsiento As String
            'If Me.mParaFechaRegistroAc = "V" Then
            'FechaAsiento = Format(Me.mFecha, "ddMMyyyy")
            'ElseIf Me.mParaFechaRegistroAc = "R" Then
            '    FechaAsiento = Format(Now, "ddMMyyyy")
            'Else
            '    FechaAsiento = Format(Me.mFecha, "ddMMyyyy")
            'End If

            '-------------------------------------------------------------------------------------------------
            '  Apuntes Contables(CFATOLIN)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Mid(Format(Me.mFecha, "ddMMyyyy"), 5, 4) &
            Me.mCfatodiari_Cod.PadRight(4, CChar(" ")) &
            " ".PadLeft(8, CChar(" ")) &
            " ".PadLeft(4, CChar(" ")) &
            vCfcta_Cod.PadRight(15, CChar(" ")) &
            vCfcptos_Cod.PadRight(4, CChar(" ")) &
            Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) &
            CType(vImonep, String).PadLeft(16, CChar(" ")) &
            "N" & Format(Me.mFecha, "ddMMyyyy") &
            Format(Me.mFecha, "ddMMyyyy") &
            " ".PadRight(40, CChar(" ")) &
            Me.mCfatotip_Cod.PadRight(4, CChar(" ")) &
            mCfivaLibro_Cod.PadRight(2, CChar(" ")) &
            vFactuTipo_cod.PadRight(6, CChar(" ")) &
            CType(vNfactura, String).PadRight(8, CChar(" ")))

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc2")
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


            SQL = "SELECT 'X' FROM CFCCOSTO WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND COD = '" & vCfcCosto_Cod & "'"

            If Me.DbSpyro.EjecutaSqlScalar(SQL) <> "X" Then
                'Me.mListBoxDebug.Items.Add("SPYRO   : " & vCfcCosto_Cod & " No se Localiza Centro de Costo en Spyro")
                'Me.mListBoxDebug.Update()
                Me.mTexto = "SPYRO   : " & vCfcCosto_Cod & " No se Localiza Centro de Costo en Spyro"
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & 99 & "," & 1 & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)
                Exit Sub
            End If



        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAA")
        End Try
    End Sub
    Private Sub GeneraFileAAInventario(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                ByVal vCfatocab_Refer As Integer,
                                 ByVal vCfcta_Cod As String, ByVal vCfcctip_Cod As String, ByVal vCfcCosto_Cod As String,
                                 ByVal vImonep As Double, ByVal vFechaInventario As Date)
        Try

            '-------------------------------------------------------------------------------------------------
            '  Apuntes Contables(CFATOLINCC)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) &
            vEmpGrupoCod.PadRight(4, CChar(" ")) &
            vEmpCod.PadRight(4, CChar(" ")) &
            Mid(vCefejerc_Cod, 1, 4) &
            Me.mCfatodiari_Cod.PadRight(4, CChar(" ")) &
            " ".PadLeft(8, CChar(" ")) &
            " ".PadLeft(4, CChar(" ")) &
            vCfcta_Cod.PadRight(15, CChar(" ")) &
            vCfcctip_Cod.PadRight(4, CChar(" ")) &
            vCfcCosto_Cod.PadRight(15, CChar(" ")) &
            CType(vImonep, String).PadLeft(16, CChar(" ")) & Format(vFechaInventario, "ddMMyyyy"))

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAA")
        End Try
    End Sub
    Private Sub GeneraFileFV(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String,
    ByVal vSerie As String, ByVal vNfactura As String, ByVal vImonep As Double, ByVal vSfactura As String, ByVal vCuenta As String, ByVal vCif As String, ByVal vFechaDoc As Date)

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
            Mid(vNfactura, 1, 8).PadLeft(8, CChar(" ")) &
            " ".PadRight(8, CChar(" ")) &
            Format(vFechaDoc, "ddMMyyyy") &
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
            CType(vImonep, String).PadRight(16, CChar(" ")) &
            CType(vImonep, String).PadRight(16, CChar(" ")) & "NN")


        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileFV")
        End Try
    End Sub
    'Private Sub GeneraFileIV(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vFactutipo_cod As String, _
    'ByVal vNfactura As String, ByVal vI_basmonemp As Double, ByVal vPj_iva As Double, ByVal vI_ivamonemp As Double)


    '   Try
    '       '-------------------------------------------------------------------------------------------------
    '       '  Libro de Iva(CFIVALIN)
    '       '-------------------------------------------------------------------------------------------------
    '       Me.Filegraba.WriteLine(vTipo.PadRight(2, CChar(" ")) & _
    '       vEmpGrupoCod.PadRight(4, CChar(" ")) & _
    '       vEmpCod.PadRight(4, CChar(" ")) & _
    '       Me.mCfivaLibro_Cod.PadRight(2, CChar(" ")) & _
    '       vFactutipo_cod.PadRight(6, CChar(" ")) & _
    '       Mid(vNfactura, 1, 8).PadLeft(8, CChar(" ")) & _
    '       Me.mCfivatimpu_Cod.PadRight(2, CChar(" ")) & _
    '        Me.mCfivatip_Cod.PadRight(2, CChar(" ")) & _
    '        CType(vI_basmonemp, String).PadLeft(16, CChar(" ")) & _
    '        CType(vPj_iva, String).PadLeft(13, CChar(" ")) & _
    '        CType(vI_ivamonemp, String).PadLeft(16, CChar(" ")))''

    '   Catch EX As Exception
    '       MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileIV")
    '   End Try
    'End Sub
    Private Sub GeneraFileIV(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vFactutipo_cod As String,
   ByVal vNfactura As String, ByVal vI_basmonemp As Double, ByVal vPj_iva As Double, ByVal vI_ivamonemp As Double, ByVal vX As String, ByVal vCodigoImpuesto As String)


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
            vCodigoImpuesto.PadRight(4, CChar(" ")) &
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
    Private Function BuscaCuentaProveedor(ByVal vProveedor As Integer) As String
        Try
            SQL = "SELECT NVL(FORN_CEX1,'0') "
            SQL += "FROM TNST_FORN "
            SQL += " WHERE FORN_CODI = " & vProveedor

            Me.mResult = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            If IsNothing(Me.mResult) = False Then
                Return Me.mResult
            Else
                Return "0"
            End If
        Catch ex As Exception
            Return "0"
        End Try
    End Function

    Private Function BuscaCuentaProveedorCuentasPorPagarSpyro(ByVal vNif As String) As String


        Dim Control As Integer
        ' TRATA DE BUSCAR LA CUENTA DE PROVEEDOR EN SPYRO A TRAVES DEL NIF 
        ' CONTROL
        SQL = "SELECT nvl(COUNT(*),0),EMPGRUPO_COD,EMP_COD,CFCTA.COD,RSOCIAL_COD FROM CFCTA ,RSOCIAL WHERE "
        SQL += " CFCTA.RSOCIAL_COD = RSOCIAL.COD"

        SQL += " AND EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
        SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
        SQL += " AND RSOCIAL_COD = '" & vNif & "'"

        SQL += " AND (CFCTA.COD LIKE '400%'  OR CFCTA.COD LIKE '410%')"



        SQL += " GROUP BY EMPGRUPO_COD,EMP_COD,CFCTA.COD,RSOCIAL_COD "
        SQL += " HAVING COUNT(*)  > 1"


        Control = CInt(Me.DbSpyro.EjecutaSqlScalar(SQL))

        If IsNothing(Control) = False Then
            If Control > 1 Then
                MsgBox("Atención más de una Cuenta para este NIF : " & vNif & " en Spyro", MsgBoxStyle.Information, "Atención")
                MsgBox("Pedir por INputBox", MsgBoxStyle.Exclamation, "Falta Desarrollo")
                Return "0"
                Exit Function
            End If
        End If




        SQL = "SELECT COD FROM CFCTA WHERE "
        SQL += " EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
        SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
        SQL += " AND RSOCIAL_COD = '" & vNif & "'"
        SQL += " AND (CFCTA.COD LIKE '400%'  OR CFCTA.COD LIKE '410%')"


        Me.mResult = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

        If IsNothing(Me.mResult) = False Then
            Return Me.mResult
        Else
            Return "0"
        End If
    End Function
    Private Function BuscaCuentaProveedorAlbaranes(ByVal vProveedor As Integer) As String
        Try
            SQL = "SELECT NVL(FORN_CEX2,'0') "
            SQL += "FROM TNST_FORN "
            SQL += " WHERE FORN_CODI = " & vProveedor

            Me.mResult = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            If IsNothing(Me.mResult) = False Then
                Return Me.mResult
            Else
                Return "0"
            End If
        Catch ex As Exception
            Return "0"
        End Try


    End Function
    Private Function BuscaCuentaCostoLiquido(ByVal vAlmacen As Integer, ByVal vGrupo As Integer) As String
        'TIPO DE CUENTA  = COSTO LIQUIDOS 
        ' ASIGNAR A COSTOS LIQUIDOS EN NEWSTOK
        MsgBox("TRABAJANDO AQUI , ESTAS FUNCIONES DEBEN DE DEJAR TODAS DE LEER DE TNST_CNTA")
        SQL = "SELECT NVL(TNST_CNTA.CNTA_CNTA,'?') "
        SQL += "FROM TNST_CUCL, TNST_CNTA "
        SQL += "WHERE TNST_CUCL.CUCL_CNTB = TNST_CNTA.CNTA_CODI"
        SQL += " AND TNST_CUCL.ALMA_CODI = " & vAlmacen
        SQL += " AND TNST_CUCL.GRUP_CODI = " & vGrupo

        Me.mResult = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

        If IsNothing(Me.mResult) = False Then
            Return Me.mResult
        Else
            Return "0"
        End If

    End Function
    Private Function BuscaCuentaGastosFamilia(ByVal vFamiCodi As Integer) As String
        'TIPO DE CUENTA  = COSTO LIQUIDOS 
        ' ASIGNAR A COSTOS LIQUIDOS EN NEWSTOK
        SQL = "SELECT NVL(FAMI_CEXT,'0') "
        SQL += "FROM TNST_FAMI "
        SQL += "WHERE FAMI_CODI = " & vFamiCodi


        Me.mResult = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

        If IsNothing(Me.mResult) = False Then
            Return Me.mResult
        Else
            Return "0"
        End If
    End Function

    Private Function BuscaCuentaGastosProducto(ByVal vProdCodi As Integer) As String

        SQL = "SELECT FAMI_CODI "
        SQL += "FROM TNST_PROD "
        SQL += "WHERE PROD_CODI = " & vProdCodi

        Me.mResult = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


        SQL = "SELECT NVL(FAMI_CEXT,'0') "
        SQL += "FROM TNST_FAMI "
        SQL += "WHERE FAMI_CODI = " & Me.mResult

        Me.mResult = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

        If IsNothing(Me.mResult) = False Then
            Return Me.mResult
        Else
            Return "0"
        End If


    End Function
    Private Function BuscaCuentaConsumoInterno(ByVal vAlmacen As Integer, ByVal vGrupo As Integer) As String
        'TIPO DE CUENTA  = CONSUMO INTERNO
        ' ASIGNAR A GASTOS 
        SQL = "SELECT NVL(TNST_CNTA.CNTA_CNTA,'?') "
        SQL += "FROM TNST_CUCI, TNST_CNTA "
        SQL += "WHERE TNST_CUCI.CUCI_CNTB = TNST_CNTA.CNTA_CODI"
        SQL += " AND TNST_CUCI.ALMA_CODI = " & vAlmacen
        SQL += " AND TNST_CUCI.GRUP_CODI = " & vGrupo

        Me.mResult = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

        If IsNothing(Me.mResult) = False Then
            Return Me.mResult
        Else
            Return "0"
        End If
    End Function

    Private Function BuscaCuentaImpuestoVenta(ByVal vIva As Integer) As String

        Try
            SQL = "SELECT NVL(IVAS_CEX1,'0') "
            SQL += "FROM TNST_IVAS "
            SQL += "WHERE IVAS_CODI = " & vIva

            Me.mResult = Me.DbLeeHotelAux.EjecutaSqlScalar2(SQL)

            If IsNothing(Me.mResult) = False Then
                Return Me.mResult
            Else
                Return "0"
            End If

        Catch ex As Exception
            Return "0"
        End Try


    End Function


    Private Function BuscaCuentaExistencia(ByVal vAlmacen As Integer, ByVal vGrupo As Integer) As String

        SQL = "SELECT NVL(TNST_CNTA.CNTA_CNTA,'?') "
        SQL += "FROM TNST_CUEX, TNST_CNTA "
        SQL += "WHERE TNST_CNTA.CNTA_CODI = TNST_CUEX.CUEX_CNTB "
        SQL += " AND TNST_CUEX.ALMA_CODI = " & vAlmacen
        SQL += " AND TNST_CUEX.GRUP_CODI = " & vGrupo

        If IsNothing(Me.DbLeeHotelAux.EjecutaSqlScalar2(SQL)) = False Then
            Return Me.DbLeeHotelAux.EjecutaSqlScalar2(SQL)
        Else
            Return "0"
        End If
    End Function
    Private Function BuscaCuentaExistenciaSantanaCazorla(ByVal vAlmacen As Integer, ByVal vGrupo As Integer) As String
        Dim Cuenta As String
        Dim CuentaAux As String

        SQL = "SELECT NVL(TNST_CNTA.CNTA_CNTA,'?') "
        SQL += "FROM TNST_CUCL, TNST_CNTA "
        SQL += "WHERE TNST_CUCL.CUCL_CNTB = TNST_CNTA.CNTA_CODI"
        SQL += " AND TNST_CUCL.ALMA_CODI = " & vAlmacen
        SQL += " AND TNST_CUCL.GRUP_CODI = " & vGrupo

        If IsNothing(Me.DbLeeHotelAux.EjecutaSqlScalar2(SQL)) = False Then
            Cuenta = Me.DbLeeHotelAux.EjecutaSqlScalar2(SQL)
        Else
            Cuenta = "0"
            Return Cuenta
        End If
        ' componer la cuenta 
        If Mid(Cuenta, 3, 3) = "0" Then
            CuentaAux = "610" & Mid(Cuenta, 4, 6)
        Else
            CuentaAux = "610" & Mid(Cuenta, 3, 2) & Mid(Cuenta, 6, 4)
        End If

        Return CuentaAux

    End Function

    Private Function ComponeCuentaGastoGrupo4(ByVal vAlmacen As Integer, ByVal vGrupo As Integer) As String

        ' raiz + actividad + hotel + centro costo departamento + centro gosto grupo de articulos 
        Try


            Dim Raiz As String
            Dim Actividad As String
            Dim Hotel As String

            Dim Departamento As String
            Dim Grupo As String


            SQL = "SELECT NVL(PARA_CTA_RAIZ4_GASTO,'GGGG')  "
            SQL += " FROM TS_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND PARA_EMP_NUM = " & Me.mEmpNum
            Raiz = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_CTA_RAIZ4_ACTI,'AA')  "
            SQL += " FROM TS_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND PARA_EMP_NUM = " & Me.mEmpNum
            Actividad = Me.DbLeeCentral.EjecutaSqlScalar(SQL)


            SQL = "SELECT NVL(PARA_CTA_RAIZ4_CENTRO,'HH')  "
            SQL += " FROM TS_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND PARA_EMP_NUM = " & Me.mEmpNum
            Hotel = Me.DbLeeCentral.EjecutaSqlScalar(SQL)


            SQL = "SELECT NVL(SUBSTR(ALMA_CCST,1,2),'AA')  "
            SQL += " FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacen
            Departamento = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            SQL = "SELECT NVL(GRUP_CCST,'GGG')  "
            SQL += " FROM TNST_GRUP WHERE GRUP_CODI = " & vGrupo
            Grupo = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            ' LOREZO PIDE SEA EL GRUPO FIJO = 000
            Grupo = "000"

            Me.mTextDebug.Text = " Compuesta =  " & Raiz & Hotel & Departamento & Grupo
            Me.mTextDebug.Update()

            Return Raiz & Hotel & Departamento & Grupo
        Catch ex As Exception
            Return "0"
        End Try

    End Function
    Private Function ComponeCuentaGastoGrupo4Departamento(ByVal vAlmacen As Integer) As String

        ' raiz + actividad + hotel + centro costo departamento + centro gosto grupo de articulos 
        Try


            Dim Raiz As String
            Dim Actividad As String
            Dim Hotel As String

            Dim Departamento As String
            Dim Grupo As String


            SQL = "SELECT NVL(PARA_CTA_RAIZ4_GASTO,'RRRRR')  "
            SQL += " FROM TS_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND PARA_EMP_NUM = " & Me.mEmpNum
            Raiz = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_CTA_RAIZ4_ACTI,'AA')  "
            SQL += " FROM TS_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND PARA_EMP_NUM = " & Me.mEmpNum
            Actividad = Me.DbLeeCentral.EjecutaSqlScalar(SQL)


            'SQL = "SELECT NVL(PARA_CTA_RAIZ4_CENTRO,'HH')  "
            'SQL += " FROM TS_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            'SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            'SQL += "  AND PARA_EMP_NUM = " & Me.mEmpNum
            'Hotel = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(SUBSTR(ALMA_CCST,3,2),'HH')  "
            SQL += " FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacen
            Hotel = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)



            SQL = "SELECT NVL(SUBSTR(ALMA_CCST,1,2),'AA')  "
            SQL += " FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacen
            Departamento = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)




            ' LORENZO PIDE SEA EL GRUPO FIJO = 000
            Grupo = "000"

            Me.mTextDebug.Text = " Compuesta =  " & Raiz & Hotel & Departamento & Grupo
            Me.mTextDebug.Update()

            Return Raiz & Hotel & Departamento & Grupo
        Catch ex As Exception
            Return "0"
        End Try

    End Function
    Private Function ComponeCuentaGastoGrupo6(ByVal vAlmacen As Integer, ByVal vFamilia As Integer) As String

        ' raiz + actividad + hotel + centro costo departamento + centro gosto grupo de articulos 
        Try



            Dim Hotel As String

            Dim Departamento As String
            Dim Fami As String




            If Me.mConectarNewCentral = 0 Then

                SQL = "SELECT NVL(SUBSTR(ALMA_CCST,3,2),'HH')  "
                SQL += " FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacen
                Hotel = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


                SQL = "SELECT NVL(SUBSTR(ALMA_CCST,1,2),'AA')  "
                SQL += " FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacen
                Departamento = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


                SQL = "SELECT NVL(FAMI_CCST,'        ')  "
                SQL += " FROM TNST_FAMI WHERE FAMI_CODI = " & vFamilia
                Fami = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                Me.mTextDebug.Text = " Compuesta Remota=  " & Mid(Fami, 1, 5) & Hotel & Departamento & Mid(Fami, 6, 3)
                Me.mTextDebug.Update()
            Else

                SQL = "SELECT NVL(SUBSTR(ALMA_CCST,3,2),'HH')  "
                SQL += " FROM TNCC_ALMA WHERE ALMA_CODI = " & vAlmacen
                SQL += " AND HOTE_CODI = " & Me.mHotelNewCentral
                Hotel = Me.DbNewCentral.EjecutaSqlScalar(SQL)


                SQL = "SELECT NVL(SUBSTR(ALMA_CCST,1,2),'AA')  "
                SQL += " FROM TNCC_ALMA WHERE ALMA_CODI = " & vAlmacen
                SQL += " AND HOTE_CODI = " & Me.mHotelNewCentral
                Departamento = Me.DbNewCentral.EjecutaSqlScalar(SQL)


                SQL = "SELECT NVL(FAMI_CCST,'        ')  "
                SQL += " FROM TNCC_NS_FAMI WHERE FAMI_CODI = " & vFamilia
                Fami = Me.DbNewCentral.EjecutaSqlScalar(SQL)
                Me.mTextDebug.Text = " Compuesta NewCentral=  " & Mid(Fami, 1, 5) & Hotel & Departamento & Mid(Fami, 6, 3)
                Me.mTextDebug.Update()
            End If



            If IsNothing(Hotel) = False And IsNothing(Departamento) = False And IsNothing(Fami) = False Then
                Return Mid(Fami, 1, 5) & Hotel & Departamento & Mid(Fami, 6, 3)
            Else
                Return "?"
            End If





        Catch ex As Exception
            Return "0"
        End Try

    End Function

    Private Function DameHotelapartirdeAlmacen(ByVal vAlmacen As Integer) As String
        Try
            If Me.mConectarNewCentral = 0 Then
                SQL = "SELECT NVL(SUBSTR(ALMA_CCST,5,2),'EE')  "
                SQL += " FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacen
                Me.AuxStr = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            Else
                SQL = "SELECT NVL(SUBSTR(ALMA_CCST,5,2),'EE')  "
                SQL += " FROM TNCC_ALMA WHERE ALMA_CODI = " & vAlmacen
                SQL += " AND HOTE_CODI = " & Me.mHotelNewCentral
                Me.AuxStr = Me.DbNewCentral.EjecutaSqlScalar(SQL)
            End If


            If IsNothing(Me.AuxStr) = False Then
                If AuxStr = "EE" Then
                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, " Grave !!! No se ha sido capaz de Determinar la Empresa Spyro a partir del Centro de Costo del Almacén = " & vAlmacen)
                End If
                Return Me.AuxStr
            Else
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, " Grave !!! No se ha sido capaz de Determinar la Empresa Spyro a partir del Centro de Costo del Almacén = " & vAlmacen)
                Return "EE"
            End If

        Catch ex As Exception
            Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, " Grave !!! No se ha sido capaz de Determinar la Empresa Spyro a partir del Centro de Costo del Almacén = " & vAlmacen)
            Return "EE"
        End Try

    End Function
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
#Region "ASIENTO-1 ALBARANES"

    Private Sub TotalPendienteFormalizarProveedorCuentaUnicaAgrupado()
        Dim Total As Double

        SQL = "SELECT MOVG_DADO,TNST_MOVG.MOVG_CODI,MOVG_ORIG,SUM(TNST_MOVG.MOVG_VATO)AS TOTAL,FORN_DESC AS PROVEEDOR ,NVL(MOVG_IDDO,'  ')AS DOCU,TNST_FORN.FORN_CODI AS CODI "
        SQL += " ,NVL(MOVG_DEST,0) AS MOVG_DEST "
        SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_FORN,TNST_ALMA "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += "AND TNST_MOVG.MOVG_DEST = TNST_ALMA.ALMA_CODI "
        SQL += "AND (TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI OR TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI)"
        SQL += "AND TIMO_TIPO = " & Me.mTimo_Albaran
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " GROUP BY MOVG_DADO,TNST_MOVG.MOVG_CODI,MOVG_ORIG,MOVG_IDDO,TNST_FORN.FORN_CODI,FORN_DESC,MOVG_DEST"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            Me.mTextDebug.Text = CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String)
            Me.mTextDebug.Update()


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaFormalizaAlbaranes, Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "", CInt(Me.DbLeeHotel.mDbLector("MOVG_DEST")), "", "", 0, "", 0, "", "", 0)
            'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotel.mDbLector("MOVG_DEST"))), CType(Now.Year, String), Me.mCtaFormalizaAlbaranes, Me.mIndicadorHaber, "ALBARAN   " & CType(Me.DbLeeHotel.mDbLector("DOCU"), String) & " " & CType(Me.DbLeeHotel.mDbLector("MOVG_DADO"), String), Total)
            'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaFormalizaAlbaranes, Me.mIndicadorHaber, "ALBARAN   " & CType(Me.DbLeeHotel.mDbLector("DOCU"), String) & " " & CType(Me.DbLeeHotel.mDbLector("MOVG_DADO"), String), Total)


        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub TotalPendienteFormalizarProveedor()
        Dim Total As Double

        SQL = "SELECT MOVG_DADO,MOVG_CODI,MOVG_ORIG,SUM(TNST_MOVG.MOVG_VATO)AS TOTAL,FORN_DESC AS PROVEEDOR ,NVL(MOVG_IDDO,'  ')AS DOCU,TNST_FORN.FORN_CODI AS CODI "
        SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_FORN "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += "AND (TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI OR TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI)"
        SQL += "AND TIMO_TIPO = " & Me.mTimo_Albaran
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " GROUP BY MOVG_DADO,MOVG_CODI,MOVG_ORIG,MOVG_IDDO,TNST_FORN.FORN_CODI,FORN_DESC"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            Me.mTextDebug.Text = CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String)
            Me.mTextDebug.Update()

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedorAlbaranes(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "")
            'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaProveedorAlbaranes(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), Me.mIndicadorHaber, "ALBARAN   " & CType(Me.DbLeeHotel.mDbLector("DOCU"), String) & " " & CType(Me.DbLeeHotel.mDbLector("MOVG_DADO"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub TotalPendienteFormalizarProveedorDetalladoHTito()
        Dim Total As Double

        SQL = "SELECT MOVG_DADO,TNST_MOVG.MOVG_CODI || '/' || TNST_MOVG.MOVG_ANCI AS DOCNEWSTOCK,MOVG_ORIG,SUM(TNST_MOVG.MOVG_VATO)AS TOTAL,FORN_DESC AS PROVEEDOR ,NVL(MOVG_IDDO,'  ')AS DOCU,TNST_FORN.FORN_CODI AS CODI "
        SQL += " ,NVL(MOVG_DEST,0) AS MOVG_DEST "

        SQL += ", NVL(FORN_CEX1,'0') AS FORN_CEX1 "
        SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_FORN "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += "AND (TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI OR TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI)"
        SQL += "AND TIMO_TIPO = " & Me.mTimo_Albaran
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " GROUP BY MOVG_DADO,MOVG_CODI,MOVG_ANCI,MOVG_ORIG,MOVG_IDDO,TNST_FORN.FORN_CODI,FORN_DESC,MOVG_DEST"

        SQL += ",FORN_CEX1"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            Me.mTextDebug.Text = CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String)
            Me.mTextDebug.Update()

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedorAlbaranes(CInt(Me.DbLeeHotel.mDbLector("CODI"))), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "", CType(Me.DbLeeHotel.mDbLector("MOVG_DEST"), Integer), CStr(Me.DbLeeHotel.mDbLector("FORN_CEX1")), "", 0, "", 0, CStr(Me.DbLeeHotel.mDbLector("DOCNEWSTOCK")), "", 9)

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub GastosPorCentrodeCostoAlbaranes()
        Dim Total As Double
        Dim vCentroCosto As String
        Try

            Dim ProdCoex As String
            Dim UnmeCoex As String
            Dim AlmaCoex As String
            Dim ProvCoex As String


            SQL = "SELECT TNST_MOVG.MOVG_DAVA,"
            SQL += "TNST_MOVD.ALMA_CODI, TNST_MOVD.MOVD_TOTA AS TOTAL,ALMA_DESC AS ALMACEN"
            SQL += ",NVL(MOVG_IDDO,'?')AS DOCU"
            SQL += ",TNST_PROD.PROD_CODI AS PRODUCTO ,PROD_DESC,TNST_PROD.PROD_COEX AS PROD_COEX"
            SQL += ", MOVG_ORIG AS PROVEEDOR,MOVD_QNTD AS CANTIDAD, UNME_CEXT AS UNIDAD_CEXT,ALMA_COEX AS ALMA_CEXT, MOVD_PRUN AS PCOSTO, TNST_MOVG.MOVG_CODI || '/' || TNST_MOVG.MOVG_ANCI AS DOCNEWSTOCK"

            SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD,  TNST_TIMO,TNST_UNME"
            SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
            SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
            SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
            SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
            SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
            SQL += " AND (TNST_MOVD.UNME_CODI = TNST_UNME.UNME_CODI)"
            SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Albaran
            SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
            SQL += " AND TNST_MOVG.MOVG_ANUL = 0"


            SQL += " ORDER BY TNST_MOVG.MOVG_IDDO,TNST_PROD.PROD_CODI"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                SQL = "SELECT  NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMA_CODI"), Integer)
                vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

                SQL = "SELECT  NVL(FORN_CEX2,'0') AS FORN_CEX2 FROM TNST_FORN WHERE FORN_CODI = " & CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), Integer)
                ProvCoex = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

                If IsDBNull(Me.DbLeeHotel.mDbLector("PROD_COEX")) = False Then
                    ProdCoex = CStr(Me.DbLeeHotel.mDbLector("PROD_COEX"))
                Else
                    ProdCoex = ""
                End If

                If IsDBNull(Me.DbLeeHotel.mDbLector("UNIDAD_CEXT")) = False Then
                    UnmeCoex = CStr(Me.DbLeeHotel.mDbLector("UNIDAD_CEXT"))
                Else
                    UnmeCoex = ""
                End If

                If IsDBNull(Me.DbLeeHotel.mDbLector("ALMA_CEXT")) = False Then
                    AlmaCoex = CStr(Me.DbLeeHotel.mDbLector("ALMA_CEXT"))
                Else
                    AlmaCoex = ""
                End If

                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"


                Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaGastosProducto(Me.DbLeeHotel.mDbLector("PRODUCTO")), Me.mIndicadorDebe, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("PROD_DESC"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total, "NO", CStr(Me.DbLeeHotel.mDbLector("DOCU")), "", CInt(Me.DbLeeHotel.mDbLector("ALMA_CODI")), ProvCoex, AlmaCoex, CDec(Me.DbLeeHotel.mDbLector("CANTIDAD")), UnmeCoex, CDec(Me.DbLeeHotel.mDbLector("PCOSTO")), CStr(Me.DbLeeHotel.mDbLector("DOCNEWSTOCK")), ProdCoex, 0)


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            Me.DbLeeHotel.mDbLector.Close()
            MsgBox("GastosPorCentrodeCostoAlbaranes" & vbCrLf & ex.Message, MsgBoxStyle.Information, "Atención")
        End Try
    End Sub

#End Region
#Region "ASIENTO-2 TRASPASOS INTERNOS"
    Private Sub TraspasosSalidas()
        Dim Total As Double
        Dim vCentroCosto As String

        Dim ProdCoex As String
        Dim UnmeCoex As String
        Dim AlmaCoex As String

        SQL = "SELECT   TNST_MOVG.MOVG_CODI,TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, NVL(TNST_MOVG.MOVG_IDDO,' ')AS DOCU,"
        SQL += "TNST_MOVD.ALMA_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,"
        SQL += " TNST_MOVD.MOVD_ENSA,TNST_MOVD.ALMA_CODI AS ALMACODI"
        SQL += ",TNST_PROD.PROD_CODI AS PRODUCTO ,PROD_DESC,TNST_PROD.PROD_COEX AS PROD_COEX"
        SQL += ", MOVG_ORIG AS PROVEEDOR,MOVD_QNTD AS CANTIDAD, UNME_CEXT AS UNIDAD_CEXT,ALMA_COEX AS ALMA_CEXT, MOVD_PRUN AS PCOSTO, TNST_MOVG.MOVG_CODI || '/' || TNST_MOVG.MOVG_ANCI AS DOCNEWSTOCK"




        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD,  TNST_TIMO,TNST_UNME "


        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " AND (TNST_MOVD.UNME_CODI = TNST_UNME.UNME_CODI)"


        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Traspaso
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " AND TNST_MOVD.MOVD_ENSA = " & -1

        SQL += " GROUP BY "
        SQL += "    TNST_MOVG.MOVG_CODI, "
        SQL += "    TNST_MOVG.MOVG_DAVA, "
        SQL += "    TNST_MOVD.ALMA_CODI, "
        SQL += "    TNST_MOVG.MOVG_IDDO, "
        SQL += "    TNST_MOVG.MOVG_IDDO, "
        SQL += "    TNST_TIMO.TIMO_TIPO, "
        SQL += "    TNST_ALMA.ALMA_DESC, "
        SQL += "    TNST_MOVD.MOVD_ENSA, "
        SQL += "    TNST_PROD.PROD_CODI, "
        SQL += "    PROD_DESC, "
        SQL += "    ALMA_DESC, "
        SQL += "    TNST_PROD.PROD_COEX, "
        SQL += "    MOVG_ORIG, "
        SQL += "    MOVD_QNTD, "
        SQL += "    UNME_CEXT, "
        SQL += "    ALMA_COEX, "
        SQL += "    MOVD_PRUN, "
        SQL += "    TNST_MOVG.MOVG_CODI, "
        SQL += "    TNST_MOVG.MOVG_ANCI "



        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            SQL = "SELECT  NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)

            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)



            If IsDBNull(Me.DbLeeHotel.mDbLector("PROD_COEX")) = False Then
                ProdCoex = CStr(Me.DbLeeHotel.mDbLector("PROD_COEX"))
            Else
                ProdCoex = ""
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("UNIDAD_CEXT")) = False Then
                UnmeCoex = CStr(Me.DbLeeHotel.mDbLector("UNIDAD_CEXT"))
            Else
                UnmeCoex = ""
            End If

            If IsDBNull(Me.DbLeeHotel.mDbLector("ALMA_CEXT")) = False Then
                AlmaCoex = CStr(Me.DbLeeHotel.mDbLector("ALMA_CEXT"))
            Else
                AlmaCoex = ""
            End If

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"


            '   Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, BuscaCuentaGastosProducto(Me.DbLeeHotel.mDbLector("PRODUCTO")), Me.mIndicadorHaber, CType("SALIDA TRASPASO ", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("PROD_DESC"), String), Total, "NO", "0", "", CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer), "", "", 0, "", 0, "", "")
            Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, BuscaCuentaGastosProducto(Me.DbLeeHotel.mDbLector("PRODUCTO")), Me.mIndicadorHaber, CType("SALIDA TRASPASO ", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("PROD_DESC"), String), Total, "NO", CStr(Me.DbLeeHotel.mDbLector("DOCU")), "", CInt(Me.DbLeeHotel.mDbLector("ALMA_CODI")), CStr(Me.DbLeeHotel.mDbLector("PROVEEDOR")), AlmaCoex, CDec(Me.DbLeeHotel.mDbLector("CANTIDAD")), UnmeCoex, CDec(Me.DbLeeHotel.mDbLector("PCOSTO")), CStr(Me.DbLeeHotel.mDbLector("DOCNEWSTOCK")), ProdCoex, 0)



        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub TraspasosEntradas()

        Try
            Dim Total As Double
            Dim vCentroCosto As String

            Dim ProdCoex As String
            Dim UnmeCoex As String
            Dim AlmaCoex As String

            SQL = "SELECT   TNST_MOVG.MOVG_CODI,TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, NVL(TNST_MOVG.MOVG_IDDO,' ')AS DOCU,"
            SQL += "TNST_MOVD.ALMA_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,"
            SQL += " TNST_MOVD.MOVD_ENSA,TNST_MOVD.ALMA_CODI AS ALMACODI"

            SQL += ",TNST_PROD.PROD_CODI AS PRODUCTO ,PROD_DESC,TNST_PROD.PROD_COEX AS PROD_COEX"
            SQL += ", MOVG_ORIG AS PROVEEDOR,MOVD_QNTD AS CANTIDAD, UNME_CEXT AS UNIDAD_CEXT,ALMA_COEX AS ALMA_CEXT, MOVD_PRUN AS PCOSTO, TNST_MOVG.MOVG_CODI || '/' || TNST_MOVG.MOVG_ANCI AS DOCNEWSTOCK"


            SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD,  TNST_TIMO,TNST_UNME "


            SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
            SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
            SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
            SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
            SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
            SQL += " And (TNST_MOVD.UNME_CODI = TNST_UNME.UNME_CODI)"



            SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Traspaso
            SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
            SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
            SQL += " AND TNST_MOVD.MOVD_ENSA = " & 1


            SQL += " GROUP BY "
            SQL += "    TNST_MOVG.MOVG_CODI, "
            SQL += "    TNST_MOVG.MOVG_DAVA, "
            SQL += "    TNST_MOVD.ALMA_CODI, "
            SQL += "    TNST_MOVG.MOVG_IDDO, "
            SQL += "    TNST_MOVG.MOVG_IDDO, "
            SQL += "    TNST_TIMO.TIMO_TIPO, "
            SQL += "    TNST_ALMA.ALMA_DESC, "
            SQL += "    TNST_MOVD.MOVD_ENSA, "
            SQL += "    TNST_PROD.PROD_CODI, "
            SQL += "    PROD_DESC, "
            SQL += "    ALMA_DESC, "
            SQL += "    TNST_PROD.PROD_COEX, "
            SQL += "    MOVG_ORIG, "
            SQL += "    MOVD_QNTD, "
            SQL += "    UNME_CEXT, "
            SQL += "    ALMA_COEX, "
            SQL += "    MOVD_PRUN, "
            SQL += "    TNST_MOVG.MOVG_CODI, "
            SQL += "    TNST_MOVG.MOVG_ANCI "






            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                SQL = "SELECT  NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
                vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


                If IsDBNull(Me.DbLeeHotel.mDbLector("PROD_COEX")) = False Then
                    ProdCoex = CStr(Me.DbLeeHotel.mDbLector("PROD_COEX"))
                Else
                    ProdCoex = ""
                End If

                If IsDBNull(Me.DbLeeHotel.mDbLector("UNIDAD_CEXT")) = False Then
                    UnmeCoex = CStr(Me.DbLeeHotel.mDbLector("UNIDAD_CEXT"))
                Else
                    UnmeCoex = ""
                End If

                If IsDBNull(Me.DbLeeHotel.mDbLector("ALMA_CEXT")) = False Then
                    AlmaCoex = CStr(Me.DbLeeHotel.mDbLector("ALMA_CEXT"))
                Else
                    AlmaCoex = ""
                End If

                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"


                '    Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, BuscaCuentaGastosProducto(Me.DbLeeHotel.mDbLector("PRODUCTO")), Me.mIndicadorDebe, CType("ENTRADA TRASPASO ", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("PROD_DESC"), String), Total, "NO", "0", "", CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer), "", "", 0, "", 0, "", "")
                Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, BuscaCuentaGastosProducto(Me.DbLeeHotel.mDbLector("PRODUCTO")), Me.mIndicadorDebe, CType("ENTRADA TRASPASO ", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("PROD_DESC"), String), Total, "NO", CStr(Me.DbLeeHotel.mDbLector("DOCU")), "", CInt(Me.DbLeeHotel.mDbLector("ALMA_CODI")), CStr(Me.DbLeeHotel.mDbLector("PROVEEDOR")), AlmaCoex, CDec(Me.DbLeeHotel.mDbLector("CANTIDAD")), UnmeCoex, CDec(Me.DbLeeHotel.mDbLector("PCOSTO")), CStr(Me.DbLeeHotel.mDbLector("DOCNEWSTOCK")), ProdCoex, 0)



            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            Me.DbLeeHotel.mDbLector.Close()
            MsgBox(ex.Message, MsgBoxStyle.Information, "Traspasos Entradas")
        End Try

    End Sub
#End Region
#Region "ASIENTO-3 FACTURAS DIRECTAS"
    Private Sub TotalFacturasProveedor()
        Dim Total As Double
        Dim TotalBase As Double

        Dim Texto As String


        Try

            SQL = "SELECT MOVG_DADO,TNST_MOVG.MOVG_CODI AS NMOVI,TNST_MOVG.MOVG_ANCI,MOVG_ORIG,SUM(TNST_MOVG.MOVG_VATO)AS TOTAL,FORN_DESC AS PROVEEDOR ,TNST_FORN.FORN_CODI AS FORN_CODI,"
            SQL += "NVL(TNST_MOVG.MOVG_IDDO,' ')AS DOCU,'0' AS BASE,NVL(FORN_CNTR,'0') AS CIF ,TNST_MOVG.MOVG_DEST AS MOVG_DEST "

            SQL += ",TNST_MOVG.MOVG_CODI || '/' || TNST_MOVG.MOVG_ANCI AS DOCNEWSTOCK "
            SQL += " ,NVL(MOVG_DEST,0) AS MOVG_DEST "

            SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_FORN ,TNST_ALMA "
            SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
            SQL += "AND (TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI OR TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI) "
            SQL += "AND TIMO_TIPO = " & Me.mTimo_Factura_Directa
            SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
            SQL += " AND TNST_MOVG.MOVG_ANUL = 0"

            SQL += " AND TNST_MOVG.MOVG_DEST = TNST_ALMA.ALMA_CODI "
            SQL += " GROUP BY MOVG_DADO,TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_ANCI,MOVG_ORIG,TNST_FORN.FORN_CODI,"
            SQL += "FORN_DESC,TNST_MOVG.MOVG_IDDO,FORN_CNTR,TNST_MOVG.MOVG_DEST"
            SQL += " ,ALMA_CCST "

            SQL += " ORDER BY  FORN_DESC,SUBSTR(ALMA_CCST,3,2),  TNST_MOVG.MOVG_IDDO "



            '6X'

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                TotalBase = Me.CalculaBaseImponible(CType(Me.DbLeeHotel.mDbLector("NMOVI"), Integer), CType(Me.DbLeeHotel.mDbLector("MOVG_ANCI"), Integer))


                Texto = "Fra Dir:" & CType(Me.DbLeeHotel.mDbLector("DOCU"), String) & " " & Format(Me.mFecha, "MMMM/yyyy") & " " & CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String)
                ' mery
                Texto = "Fra Dir:" & CType(Me.DbLeeHotel.mDbLector("DOCU"), String) & " " & CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String)


                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedor(CType(Me.DbLeeHotel.mDbLector("FORN_CODI"), Integer)), Me.mIndicadorHaberFac, Texto, Total, "NO", CStr(Me.DbLeeHotel.mDbLector("DOCU")), "", CInt(Me.DbLeeHotel.mDbLector("MOVG_DEST")), "", "", 0, "", 0, CStr(Me.DbLeeHotel.mDbLector("DOCNEWSTOCK")), "", 9)


                ' sale a hacer los aspuntes de GASTOS , en vez de todos juntos 
                Me.GastosPorCentrodeCostoFacturas(CType(Me.DbLeeHotel.mDbLector("NMOVI"), Integer), CType(Me.DbLeeHotel.mDbLector("MOVG_ANCI"), Integer), Texto)
                ' sale a hacer los aspuntes de IGIC , en vez de todos juntos 
                Me.TotalFacturasProveedorImpuesto(CType(Me.DbLeeHotel.mDbLector("NMOVI"), Integer), CType(Me.DbLeeHotel.mDbLector("MOVG_ANCI"), Integer), Texto)

            End While
            Me.DbLeeHotel.mDbLector.Close()

        Catch ex As Exception
            Me.DbLeeHotel.mDbLector.Close()
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturas Directas")
        End Try


    End Sub
    Private Sub GastosPorCentrodeCostoFacturas(ByVal vCodi As Integer, ByVal vAnci As Integer, ByVal vTexto As String)
        Dim Total As Double
        Dim vCentroCosto As String
        Try

            SQL = "SELECT TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, "
            SQL += "TNST_MOVD.ALMA_CODI, SUM(TNST_MOVD.MOVD_TOTA-TNST_MOVD.MOVD_IVAS)AS TOTAL,ALMA_DESC AS ALMACEN,TNST_MOVD.ALMA_CODI AS ALMACODI"
            SQL += ",TNST_PROD.PROD_CODI AS PRODUCTO,PROD_DESC"
            SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD,  TNST_TIMO"


            SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
            SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
            SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
            SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
            SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
            SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Factura_Directa
            SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
            SQL += " AND TNST_MOVG.MOVG_ANUL = 0"


            SQL += " AND TNST_MOVG.MOVG_CODI =  " & vCodi
            SQL += " AND TNST_MOVG.MOVG_ANCI =  " & vAnci


            SQL += " GROUP BY  TNST_MOVG.TIMO_CODI,TNST_MOVG.MOVG_DAVA,"
            SQL += "TNST_MOVD.ALMA_CODI,"
            SQL += "TNST_TIMO.TIMO_TIPO,"
            SQL += "TNST_ALMA.ALMA_DESC"
            SQL += ",TNST_PROD.PROD_CODI,PROD_DESC"



            Me.DbLeeHotelAux5.TraerLector(SQL)

            While Me.DbLeeHotelAux5.mDbLector.Read


                Linea = Linea + 1
                Total = CType(Me.DbLeeHotelAux5.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, BuscaCuentaGastosProducto(Me.DbLeeHotelAux5.mDbLector("PRODUCTO")), Me.mIndicadorDebe, " -- " & CType("GASTOS", String) & " " & CType(Me.DbLeeHotelAux5.mDbLector("PROD_DESC"), String) & " " & CType(Me.DbLeeHotelAux5.mDbLector("ALMACEN"), String), Total, "NO", "0", "", CType(Me.DbLeeHotelAux5.mDbLector("ALMACODI"), Integer), "", "", 0, "", 0, "", "", 0)



            End While
            Me.DbLeeHotelAux5.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub TotalFacturasProveedorImpuesto(ByVal vCodi As Integer, ByVal vAnci As Integer, ByVal vTexto As String)
        Dim Total As Double
        Dim TotalBase As Double
        Dim ValorImpuesto As Double
        Dim TipoImpuesto As String
        Dim CodigoImpuesto As String

        Try



            SQL = "SELECT TNST_MOVG.MOVG_CODI AS NMOVI,MOVG_ORIG,SUM(TNST_MOVI.MOVI_IMPU)AS TOTAL ,SUM(TNST_MOVI.MOVI_NETO)AS BASE,NVL(MOVG_IDDO,' ')AS DOCU ,MOVI_TAXA AS TIPO,TNST_MOVG.MOVG_DEST AS MOVG_DEST "
            SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_MOVI "
            SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
            SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Factura_Directa
            SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
            SQL += " AND TNST_MOVG.MOVG_ANUL = 0"

            SQL += " AND TNST_MOVG.MOVG_CODI =  " & vCodi
            SQL += " AND TNST_MOVG.MOVG_ANCI =  " & vAnci

            SQL += " AND TNST_MOVG.MOVG_CODI = TNST_MOVI.MOVG_CODI "
            SQL += " AND TNST_MOVG.MOVG_ANCI = TNST_MOVI.MOVG_ANCI "

            SQL += " GROUP BY TNST_MOVG.MOVG_CODI,MOVG_ORIG,MOVG_IDDO,MOVI_TAXA,TNST_MOVG.MOVG_DEST"

            Me.DbLeeHotelAux3.TraerLector(SQL)

            While Me.DbLeeHotelAux3.mDbLector.Read
                SQL = "SELECT IVAS_TAXA FROM TNST_IVAS WHERE IVAS_CODI = " & CInt(Me.DbLeeHotelAux6.mDbLector("TIPO"))
                ValorImpuesto = CType(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL), Double)




                SQL = "SELECT NVL(IMPU_TIPO,'0') FROM TS_IMPU WHERE IMPU_VALO = " & ValorImpuesto
                CodigoImpuesto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)
                If IsDBNull(CodigoImpuesto) = True Then
                    MsgBox("No se Localiza Equivalencia de Impuesto " & vbCrLf & SQL, MsgBoxStyle.Information, "Atención")
                    Me.DbLeeHotelAux3.mDbLector.Close()
                    Exit Sub
                End If

                '' 2017
                'SQL = "SELECT NVL(CFIVATIP_COD,'0') FROM CFIVAPOR WHERE PIMPUESTO = " & ValorImpuesto
                'SQL += "  AND CFIVATIMPU_COD ='" & CodigoImpuesto & "'"
                'TipoImpuesto = Me.DbSpyro.EjecutaSqlScalar(SQL)

                '' 2017
                SQL = "SELECT NVL(IMPU_CFIVATIP,'?') AS  IMPU_CFIVATIP  FROM TS_IMPU  WHERE IMPU_VALO = " & ValorImpuesto
                'TipoImpuesto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)
                TipoImpuesto = "X"

                If IsNothing(TipoImpuesto) = True Then
                    TipoImpuesto = "?"
                    MsgBox("No se Localiza Equivalente de Impuesto en TS_IMPU " & vbCrLf & vbCrLf & SQL, MsgBoxStyle.Information, "Atención")
                End If






                Linea = Linea + 1
                Total = CType(Me.DbLeeHotelAux6.mDbLector("TOTAL"), Double)
                TotalBase = CType(Me.DbLeeHotelAux6.mDbLector("BASE"), Double)
                Me.mTipoAsiento = "DEBE"
                If TotalBase <> 0 Then
                    Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaImpuestoVenta(CInt(Me.DbLeeHotelAux6.mDbLector("TIPO"))), Me.mIndicadorDebe, CType("IMPUESTO FACTURA ", String) & ValorImpuesto & "%", Total, "NO", CType(Me.DbLeeHotelAux6.mDbLector("DOCU"), String), "", CInt(Me.DbLeeHotelAux6.mDbLector("MOVG_DEST")), "", "", 0, "", 0, "", "", 0)
                    'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux6.mDbLector("MOVG_DEST"))), CType(Now.Year, String), Me.BuscaCuentaImpuestoVenta2(ValorImpuesto), Me.mIndicadorDebe, vTexto, Total)
                    'Me.GeneraFileIV("IV", 3, Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux6.mDbLector("MOVG_DEST"))), Me.mParaSeriefacturas & Me.mFecha.Year, CType(Me.DbLeeHotelAux6.mDbLector("NMOVI"), String), TotalBase, ValorImpuesto, Total, TipoImpuesto, CodigoImpuesto)
                End If

            End While
            Me.DbLeeHotelAux3.mDbLector.Close()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
#End Region
#Region "ASIENTO-40 FACTURAS NEWPAGA"
    Private Sub TotalFacturasProveedorNewPaga()
        Dim Total As Double
        Dim TotalBase As Double

        Dim Cuenta As String

        Dim Texto As String

        Try


            SQL = "SELECT   TNPG_MOVI.*, TNPG_TIMO.*, VNPG_FORN.*, "
            SQL += "         DECODE (TNPG_MOVI.MOVI_INTE, "
            SQL += "                 '0', 'EXTERNO', "
            SQL += "                 '1', 'INTERNO' "
            SQL += "                ) TIPO, "
            SQL += "         DECODE (TNPG_MOVI.FORN_INTE, "
            SQL += "                 '0', 'EXTERNO', "
            SQL += "                 '1', 'INTERNO' "
            SQL += "                ) TIPOF, "
            SQL += "         DECODE (MOVI_CORR, "
            SQL += "                 '0', DECODE (MOVI_ANUL, "
            SQL += "                              '0', ('ANULADO 0'), "
            SQL += "                              '1', ('ANULADO 1') "
            SQL += "                             ), "
            SQL += "                 '1', ('ANULADO 1B') "
            SQL += "                ) ESTADO, "
            SQL += "         DECODE (TNPG_TIMO.TIMO_FORM, "
            SQL += "                 '0', DECODE (TNPG_MOVI.MOVI_AUTO, "
            SQL += "                              '0', 'AUTOMATICO 0', "
            SQL += "                              '1', 'AUTOMATICO 1', "
            SQL += "                              'OTRO' "
            SQL += "                             ), "
            SQL += "                 ' ' "
            SQL += "                ) AUTO "
            SQL += " ,NVL(TNPG_MOVI.MOVI_DOCU,'?') AS DOCUMENTO"
            SQL += " ,NVL(VNPG_FORN.FORN_CNTR,'?') AS NIF"
            SQL += " ,NVL(TNPG_MOVI.MOVI_TIIM,0) AS TIPOIMPUESTO"
            SQL += "    FROM TNPG_MOVI, TNPG_TIMO, VNPG_FORN "
            SQL += "   WHERE TNPG_MOVI.TIMO_CODI = TNPG_TIMO.TIMO_CODI "
            SQL += "     AND TNPG_MOVI.FORN_CODI = VNPG_FORN.FORN_CODI "
            SQL += "     AND TNPG_MOVI.MOVI_UNMO = 'EUR' "
            SQL += "     AND TNPG_MOVI.FORN_INTE = '1' "
            SQL += "     AND TNPG_TIMO.TIMO_FORM = '0' "
            ' Solo Facturas NewPAga
            SQL += "    AND TNPG_MOVI.MOVI_INTE = '0'   "


            SQL += " AND TNPG_MOVI.MOVI_DAVA =  " & "'" & Me.mFecha & "'"
            SQL += "ORDER BY TNPG_MOVI.MOVI_CODI "



            Me.DbLeeNewPaga.TraerLector(SQL)

            While Me.DbLeeNewPaga.mDbLector.Read

                If CType(Me.DbLeeNewPaga.mDbLector("TIPOF"), String) = "EXTERNO" Then
                    Cuenta = Me.BuscaCuentaProveedor(CType(Me.DbLeeNewPaga.mDbLector("FORN_CODI"), Integer))
                ElseIf CType(Me.DbLeeNewPaga.mDbLector("TIPOF"), String) = "INTERNO" Then
                    Cuenta = Me.BuscaCuentaProveedor(CType(Me.DbLeeNewPaga.mDbLector("NIF"), String))
                Else
                    Cuenta = "?"
                End If


                Linea = Linea + 1
                Total = CType(Me.DbLeeNewPaga.mDbLector("MOVI_IMPO"), Double)
                TotalBase = Me.CalculaBaseImponiblenewPaga(CType(Me.DbLeeNewPaga.mDbLector("MOVI_CODI"), Integer), CType(Me.DbLeeNewPaga.mDbLector("TIPOIMPUESTO"), Integer))
                If TotalBase <> 0 Then

                    Texto = "Fra NewPag:" & CType(Me.DbLeeNewPaga.mDbLector("DOCUMENTO"), String) & " " & Format(Me.mFecha, "MMMM/yyyy") & " " & CType(Me.DbLeeNewPaga.mDbLector("FORN_DESC"), String)
                    ' mery
                    Texto = "Fra NewPag:" & CType(Me.DbLeeNewPaga.mDbLector("DOCUMENTO"), String) & " " & CType(Me.DbLeeNewPaga.mDbLector("FORN_DESC"), String)



                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 40, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaberFac, Texto, Total, "NO", CType(Me.DbLeeNewPaga.mDbLector("DOCUMENTO"), String), "", "NEWPAGA", CType(Me.DbLeeNewPaga.mDbLector("NIF"), String), CType(Me.DbLeeNewPaga.mDbLector("MOVI_DADO"), Date))
                    'Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaberFac, Texto, Total, Me.mParaSeriefacturasNewPaga & Me.mFecha.Year, CType(Me.DbLeeNewPaga.mDbLector("MOVI_CODI"), String))
                    'Me.GeneraFileFV("FV", 40, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSeriefacturasNewPaga & Me.mFecha.Year, CType(Me.DbLeeNewPaga.mDbLector("MOVI_CODI"), String), Total, CType(Me.DbLeeNewPaga.mDbLector("DOCUMENTO"), String).PadRight(15, CChar(" ")), Cuenta, CType(Me.DbLeeNewPaga.mDbLector("NIF"), String), CType(Me.DbLeeNewPaga.mDbLector("MOVI_DADO"), Date))
                Else
                    MsgBox("Factura sin Líneas Documento =  " & CType(Me.DbLeeNewPaga.mDbLector("DOCUMENTO"), String) & " de " & CType(Me.DbLeeNewPaga.mDbLector("FORN_DESC"), String), MsgBoxStyle.Exclamation, "Atención")
                End If


            End While
            Me.DbLeeNewPaga.mDbLector.Close()




            ' ANULADAS 

            SQL = "SELECT   TNPG_MOVI.*, TNPG_TIMO.*, VNPG_FORN.*, "
            SQL += "         DECODE (TNPG_MOVI.MOVI_INTE, "
            SQL += "                 '0', 'EXTERNO', "
            SQL += "                 '1', 'INTERNO' "
            SQL += "                ) TIPO, "
            SQL += "         DECODE (TNPG_MOVI.FORN_INTE, "
            SQL += "                 '0', 'EXTERNO', "
            SQL += "                 '1', 'INTERNO'"
            SQL += "                ) TIPOF, "
            SQL += "         DECODE (MOVI_CORR, "
            SQL += "                 '0', DECODE (MOVI_ANUL, "
            SQL += "                              '0', ('ANULADO 0'), "
            SQL += "                              '1', ('ANULADO 1') "
            SQL += "                             ), "
            SQL += "                 '1', ('ANULADO 1B') "
            SQL += "                ) ESTADO, "
            SQL += "         DECODE (TNPG_TIMO.TIMO_FORM, "
            SQL += "                 '0', DECODE (TNPG_MOVI.MOVI_AUTO, "
            SQL += "                              '0', 'AUTOMATICO 0', "
            SQL += "                              '1', 'AUTOMATICO 1', "
            SQL += "                              'OTRO' "
            SQL += "                             ), "
            SQL += "                 ' ' "
            SQL += "                ) AUTO "
            SQL += " ,NVL(TNPG_MOVI.MOVI_DOCU,'?') AS DOCUMENTO"
            SQL += " ,NVL(VNPG_FORN.FORN_CNTR,'?') AS NIF"
            SQL += " ,NVL(TNPG_MOVI.MOVI_TIIM,0) AS TIPOIMPUESTO"
            SQL += "    FROM TNPG_MOVI, TNPG_TIMO, VNPG_FORN "
            SQL += "   WHERE TNPG_MOVI.TIMO_CODI = TNPG_TIMO.TIMO_CODI "
            SQL += "     AND TNPG_MOVI.FORN_CODI = VNPG_FORN.FORN_CODI "
            SQL += "     AND TNPG_MOVI.MOVI_UNMO = 'EUR' "
            SQL += "     AND TNPG_MOVI.FORN_INTE = '1' "
            SQL += "     AND TNPG_TIMO.TIMO_FORM = '0' "
            ' Solo Facturas NewPAga
            SQL += "    AND TNPG_MOVI.MOVI_INTE = '0'   "




            SQL += " AND TRUNC(TNPG_MOVI.MOVI_DAAC) =  " & "'" & Me.mFecha & "'"
            SQL += "ORDER BY TNPG_MOVI.MOVI_CODI "



            Me.DbLeeNewPaga.TraerLector(SQL)

            While Me.DbLeeNewPaga.mDbLector.Read


                If CType(Me.DbLeeNewPaga.mDbLector("TIPOF"), String) = "EXTERNO" Then
                    Cuenta = Me.BuscaCuentaProveedor(CType(Me.DbLeeNewPaga.mDbLector("FORN_CODI"), Integer))
                ElseIf CType(Me.DbLeeNewPaga.mDbLector("TIPOF"), String) = "INTERNO" Then
                    Cuenta = Me.BuscaCuentaProveedorCuentasPorPagarSpyro(CType(Me.DbLeeNewPaga.mDbLector("NIF"), String))
                Else
                    Cuenta = "?"
                End If



                Linea = Linea + 1
                Total = CType(Me.DbLeeNewPaga.mDbLector("MOVI_IMPO"), Double) * -1
                TotalBase = Me.CalculaBaseImponiblenewPaga(CType(Me.DbLeeNewPaga.mDbLector("MOVI_CODI"), Integer), CType(Me.DbLeeNewPaga.mDbLector("TIPOIMPUESTO"), Integer)) * -1
                If TotalBase <> 0 Then
                    Me.mTipoAsiento = "HABER"
                    Me.InsertaOracle("AC", 40, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorHaberFac, CType(Me.DbLeeNewPaga.mDbLector("FORN_DESC"), String) & " Anulada", Total, "NO", CType(Me.DbLeeNewPaga.mDbLector("DOCUMENTO"), String), "", "NEWPAGA", CType(Me.DbLeeNewPaga.mDbLector("NIF"), String), CType(Me.DbLeeNewPaga.mDbLector("MOVI_DADO"), Date))
                    'Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorHaberFac, "FACTURA Anulada Num:  " & CType(Me.DbLeeNewPaga.mDbLector("DOCUMENTO"), String), Total, Me.mParaSeriefacturasNewPaga & Me.mFecha.Year, CType(Me.DbLeeNewPaga.mDbLector("MOVI_CODI"), String))
                    ' Me.GeneraFileFV("FV", 40, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSeriefacturasNewPaga & Me.mFecha.Year, CType(Me.DbLeeNewPaga.mDbLector("MOVI_CODI"), String), Total, CType(Me.DbLeeNewPaga.mDbLector("DOCUMENTO"), String).PadRight(15, CChar(" ")), Cuenta, CType(Me.DbLeeNewPaga.mDbLector("NIF"), String), CType(Me.DbLeeNewPaga.mDbLector("MOVI_DADO"), Date))
                End If

            End While
            Me.DbLeeNewPaga.mDbLector.Close()





        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub GastosPorCentrodeCostoFacturasNewPaga()
        Dim Total As Double
        Dim vCentroCosto As String

        Dim Cuenta As String = "Por Parámetro"
        Try

            SQL = "SELECT  SUM(NVL(MOVS_IMPO,0)) AS IMPORTE , SUM(NVL(MOVS_IVAS,0)) AS IMPUESTO, TNPG_SERV.SERV_CODI,NVL(SERV_DESC,'?') AS SERVICIO "
            SQL += " ,NVL(TNPG_MOVI.MOVI_TIIM,0) AS TIPOIMPUESTO"


            SQL += "    FROM TNPG_MOVI, TNPG_MOVS,TNPG_SERV,TNPG_TIMO  "
            SQL += "   WHERE TNPG_MOVI.TIMO_CODI = TNPG_TIMO.TIMO_CODI "
            SQL += "     AND TNPG_MOVI.MOVI_CODI = TNPG_MOVS.MOVI_CODI "
            SQL += "     AND TNPG_MOVS.SERV_CODI = TNPG_SERV.SERV_CODI "

            SQL += "     AND TNPG_MOVI.MOVI_UNMO = 'EUR' "
            SQL += "     AND TNPG_MOVI.FORN_INTE = '1' "
            SQL += "     AND TNPG_TIMO.TIMO_FORM = '0' "

            SQL += " AND TNPG_MOVI.MOVI_DAVA = " & "'" & Me.mFecha & "'"
            SQL += " GROUP BY TNPG_SERV.SERV_CODI,SERV_DESC,MOVI_TIIM"

            '  SQL += "ORDER BY TNPG_MOVI.MOVI_CODI "


            Me.DbLeeNewPaga.TraerLector(SQL)

            While Me.DbLeeNewPaga.mDbLector.Read
                '  SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeNewPaga.mDbLector("ALMACODI"), Integer)
                '   vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                vCentroCosto = "Pendiente"




                Linea = Linea + 1
                If CType(Me.DbLeeNewPaga.mDbLector("TIPOIMPUESTO"), Integer) = 0 Then
                    Total = CType(Me.DbLeeNewPaga.mDbLector("IMPORTE"), Double)
                Else
                    Total = CType(Me.DbLeeNewPaga.mDbLector("IMPORTE"), Double) - CType(Me.DbLeeNewPaga.mDbLector("IMPUESTO"), Double)
                End If


                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 40, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType("GASTOS", String) & " " & CType(Me.DbLeeNewPaga.mDbLector("SERVICIO"), String), Total, "NO", "0", "")
                'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType("GASTOS", String) & " " & CType(Me.DbLeeNewPaga.mDbLector("SERVICIO"), String), Total)
                'Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Cuenta, "0", vCentroCosto, Total)

            End While
            Me.DbLeeNewPaga.mDbLector.Close()

            '' ANULADAS

            SQL = "SELECT  SUM(NVL(MOVS_IMPO,0)) AS IMPORTE , SUM(NVL(MOVS_IVAS,0)) AS IMPUESTO, TNPG_SERV.SERV_CODI,NVL(SERV_DESC,'?') AS SERVICIO "
            SQL += " ,NVL(TNPG_MOVI.MOVI_TIIM,0) AS TIPOIMPUESTO"


            SQL += "    FROM TNPG_MOVI, TNPG_MOVS,TNPG_SERV,TNPG_TIMO  "
            SQL += "   WHERE TNPG_MOVI.TIMO_CODI = TNPG_TIMO.TIMO_CODI "
            SQL += "     AND TNPG_MOVI.MOVI_CODI = TNPG_MOVS.MOVI_CODI "
            SQL += "     AND TNPG_MOVS.SERV_CODI = TNPG_SERV.SERV_CODI "

            SQL += "     AND TNPG_MOVI.MOVI_UNMO = 'EUR' "
            SQL += "     AND TNPG_MOVI.FORN_INTE = '1' "
            SQL += "     AND TNPG_TIMO.TIMO_FORM = '0' "

            SQL += " AND TRUNC(TNPG_MOVI.MOVI_DAAC) = " & "'" & Me.mFecha & "'"
            SQL += " GROUP BY TNPG_SERV.SERV_CODI,SERV_DESC,MOVI_TIIM"

            '  SQL += "ORDER BY TNPG_MOVI.MOVI_CODI "


            Me.DbLeeNewPaga.TraerLector(SQL)

            While Me.DbLeeNewPaga.mDbLector.Read
                '  SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeNewPaga.mDbLector("ALMACODI"), Integer)
                '   vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                vCentroCosto = "Pendiente"


                Linea = Linea + 1
                If CType(Me.DbLeeNewPaga.mDbLector("TIPOIMPUESTO"), Integer) = 0 Then
                    Total = CType(Me.DbLeeNewPaga.mDbLector("IMPORTE"), Double) * -1
                Else
                    Total = (CType(Me.DbLeeNewPaga.mDbLector("IMPORTE"), Double) - CType(Me.DbLeeNewPaga.mDbLector("IMPUESTO"), Double)) * -1
                End If


                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 40, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, Me.mIndicadorDebe, CType("GASTOS", String) & " " & CType(Me.DbLeeNewPaga.mDbLector("SERVICIO"), String) & " Anulados ", Total, "NO", "0", "")
                'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, Me.mIndicadorDebe, CType("GASTOS", String) & " " & CType(Me.DbLeeNewPaga.mDbLector("SERVICIO"), String), Total)
                'Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Cuenta, "0", vCentroCosto, Total)

            End While
            Me.DbLeeNewPaga.mDbLector.Close()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub TotalFacturasProveedorImpuestoNewPaga()
        Dim Total As Double
        Dim TotalBase As Double
        Dim ValorImpuesto As Double
        Dim TipoImpuesto As String
        Dim CodigoImpuesto As String



        SQL = "SELECT   SUM (NVL (MOVS_IMPO, 0)) AS IMPORTE, "
        SQL += "         SUM (NVL (MOVS_IVAS, 0)) AS IMPUESTO, "
        SQL += "         NVL (TNPG_MOVI.MOVI_TIIM, 0) AS TIPOIMPUESTO, "
        SQL += "         NVL(MOVI_DOCU,'?') AS DOCUMENTO, "
        SQL += "         NVL(MOVI_DESC,'?') AS DESCRIPCION, "
        SQL += "         IVAS_TAXA,IVAS_DESC "
        SQL += "         , TNPG_MOVS.MOVI_CODI AS NMOVI "
        SQL += "    FROM TNPG_MOVI, TNPG_MOVS, TNPG_SERV, TNPG_TIMO,TNPG_IVAS "
        SQL += "   WHERE TNPG_MOVI.TIMO_CODI = TNPG_TIMO.TIMO_CODI "
        SQL += "     AND TNPG_MOVI.MOVI_CODI = TNPG_MOVS.MOVI_CODI "
        SQL += "     AND TNPG_MOVS.SERV_CODI = TNPG_SERV.SERV_CODI "
        SQL += "     AND TNPG_SERV.IVAS_CODI = TNPG_IVAS.IVAS_CODI "
        SQL += "      "
        SQL += "     AND TNPG_MOVI.MOVI_UNMO = 'EUR' "
        SQL += "     AND TNPG_MOVI.FORN_INTE = '1' "
        SQL += "     AND TNPG_TIMO.TIMO_FORM = '0' "
        SQL += "     AND TNPG_MOVI.MOVI_DAVA = " & "'" & Me.mFecha & "'"
        SQL += "GROUP BY TNPG_MOVI.MOVI_CODI,MOVI_DOCU,MOVI_DESC, MOVI_TIIM,IVAS_TAXA,IVAS_DESC,TNPG_MOVS.MOVI_CODI "




        Me.DbLeeNewPaga.TraerLector(SQL)

        While Me.DbLeeNewPaga.mDbLector.Read
            ValorImpuesto = CType(Me.DbLeeNewPaga.mDbLector("IVAS_TAXA"), Double)


            SQL = "SELECT NVL(IMPU_TIPO,'0') FROM TS_IMPU WHERE IMPU_VALO = " & ValorImpuesto
            CodigoImpuesto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)
            If IsDBNull(CodigoImpuesto) = True Then
                MsgBox("No se Localiza Equivalencia de Impuesto " & vbCrLf & SQL, MsgBoxStyle.Information, "Atención")
                Me.DbLeeNewPaga.mDbLector.Close()
                Exit Sub
            End If

            '' 2017
            'SQL = "SELECT NVL(CFIVATIP_COD,'0') FROM CFIVAPOR WHERE PIMPUESTO = " & ValorImpuesto
            'SQL += "  AND CFIVATIMPU_COD ='" & CodigoImpuesto & "'"
            'TipoImpuesto = Me.DbSpyro.EjecutaSqlScalar(SQL)


            '' 2017
            SQL = "SELECT NVL(IMPU_CFIVATIP,'?') AS  IMPU_CFIVATIP  FROM TS_IMPU  WHERE IMPU_VALO = " & ValorImpuesto
            'TipoImpuesto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)
            TipoImpuesto = "X"

            If IsNothing(TipoImpuesto) = True Then
                TipoImpuesto = "?"
                MsgBox("No se Localiza Equivalente de Impuesto en TS_IMPU " & vbCrLf & vbCrLf & SQL, MsgBoxStyle.Information, "Atención")
            End If



            Linea = Linea + 1

            If CType(Me.DbLeeNewPaga.mDbLector("TIPOIMPUESTO"), Integer) = 0 Then
                Total = CType(Me.DbLeeNewPaga.mDbLector("IMPUESTO"), Double)
                TotalBase = CType(Me.DbLeeNewPaga.mDbLector("IMPORTE"), Double)
            Else
                Total = CType(Me.DbLeeNewPaga.mDbLector("IMPUESTO"), Double)
                TotalBase = CType(Me.DbLeeNewPaga.mDbLector("IMPORTE"), Double) - CType(Me.DbLeeNewPaga.mDbLector("IMPUESTO"), Double)
            End If


            Me.mTipoAsiento = "DEBE"
            If TotalBase <> 0 Then
                Me.InsertaOracle("AC", 40, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, "Pdte", Me.mIndicadorDebe, CType("IMPUESTO FACTURA ", String) & ValorImpuesto & "%", Total, "NO", CType(Me.DbLeeNewPaga.mDbLector("DOCUMENTO"), String), "")
                'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), "Pdte", Me.mIndicadorDebe, CType("IMPUESTO FACTURA ", String) & ValorImpuesto & "%" & " " & CType(Me.DbLeeNewPaga.mDbLector("DOCUMENTO"), String), Total)
                'Me.GeneraFileIV("IV", 40, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSeriefacturasNewPaga & Me.mFecha.Year, CType(Me.DbLeeNewPaga.mDbLector("NMOVI"), String), TotalBase, ValorImpuesto, Total, TipoImpuesto, CodigoImpuesto)
            End If

        End While
        Me.DbLeeNewPaga.mDbLector.Close()

        '' ANULADAS 

        SQL = "SELECT   SUM (NVL (MOVS_IMPO, 0)) AS IMPORTE, "
        SQL += "         SUM (NVL (MOVS_IVAS, 0)) AS IMPUESTO, "
        SQL += "         NVL (TNPG_MOVI.MOVI_TIIM, 0) AS TIPOIMPUESTO, "
        SQL += "         NVL(MOVI_DOCU,'?') AS DOCUMENTO, "
        SQL += "         NVL(MOVI_DESC,'?') AS DESCRIPCION, "
        SQL += "         IVAS_TAXA,IVAS_DESC "
        SQL += "         , TNPG_MOVS.MOVI_CODI AS NMOVI "
        SQL += "    FROM TNPG_MOVI, TNPG_MOVS, TNPG_SERV, TNPG_TIMO,TNPG_IVAS "
        SQL += "   WHERE TNPG_MOVI.TIMO_CODI = TNPG_TIMO.TIMO_CODI "
        SQL += "     AND TNPG_MOVI.MOVI_CODI = TNPG_MOVS.MOVI_CODI "
        SQL += "     AND TNPG_MOVS.SERV_CODI = TNPG_SERV.SERV_CODI "
        SQL += "     AND TNPG_SERV.IVAS_CODI = TNPG_IVAS.IVAS_CODI "
        SQL += "      "
        SQL += "     AND TNPG_MOVI.MOVI_UNMO = 'EUR' "
        SQL += "     AND TNPG_MOVI.FORN_INTE = '1' "
        SQL += "     AND TNPG_TIMO.TIMO_FORM = '0' "
        SQL += "     AND TRUNC(TNPG_MOVI.MOVI_DAAC) = " & "'" & Me.mFecha & "'"
        SQL += "GROUP BY TNPG_MOVI.MOVI_CODI,MOVI_DOCU,MOVI_DESC, MOVI_TIIM,IVAS_TAXA,IVAS_DESC,TNPG_MOVS.MOVI_CODI "




        Me.DbLeeNewPaga.TraerLector(SQL)

        While Me.DbLeeNewPaga.mDbLector.Read
            ValorImpuesto = CType(Me.DbLeeNewPaga.mDbLector("IVAS_TAXA"), Double)


            SQL = "SELECT NVL(IMPU_TIPO,'0') FROM TS_IMPU WHERE IMPU_VALO = " & ValorImpuesto
            CodigoImpuesto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)
            If IsDBNull(CodigoImpuesto) = True Then
                MsgBox("No se Localiza Equivalencia de Impuesto " & vbCrLf & SQL, MsgBoxStyle.Information, "Atención")
                Me.DbLeeNewPaga.mDbLector.Close()
                Exit Sub
            End If

            '' 2017
            'SQL = "SELECT NVL(CFIVATIP_COD,'0') FROM CFIVAPOR WHERE PIMPUESTO = " & ValorImpuesto
            'SQL += "  AND CFIVATIMPU_COD ='" & CodigoImpuesto & "'"
            'TipoImpuesto = Me.DbSpyro.EjecutaSqlScalar(SQL)


            '' 2017
            SQL = "SELECT NVL(IMPU_CFIVATIP,'?') AS  IMPU_CFIVATIP  FROM TS_IMPU  WHERE IMPU_VALO = " & ValorImpuesto
            'TipoImpuesto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)
            TipoImpuesto = "X"


            If IsNothing(TipoImpuesto) = True Then
                TipoImpuesto = "?"
                MsgBox("No se Localiza Equivalente de Impuesto TS_IMPU " & vbCrLf & vbCrLf & SQL, MsgBoxStyle.Information, "Atención")
            End If



            Linea = Linea + 1

            If CType(Me.DbLeeNewPaga.mDbLector("TIPOIMPUESTO"), Integer) = 0 Then
                Total = CType(Me.DbLeeNewPaga.mDbLector("IMPUESTO"), Double) * -1
                TotalBase = CType(Me.DbLeeNewPaga.mDbLector("IMPORTE"), Double) * -1
            Else
                Total = CType(Me.DbLeeNewPaga.mDbLector("IMPUESTO"), Double) * -1
                TotalBase = (CType(Me.DbLeeNewPaga.mDbLector("IMPORTE"), Double) - CType(Me.DbLeeNewPaga.mDbLector("IMPUESTO"), Double)) * -1

            End If


            Me.mTipoAsiento = "DEBE"
            If TotalBase <> 0 Then
                Me.InsertaOracle("AC", 40, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, "Pdte", Me.mIndicadorDebe, CType("IMPUESTO FACTURA ANULADA ", String) & ValorImpuesto & "%", Total, "NO", CType(Me.DbLeeNewPaga.mDbLector("DOCUMENTO"), String), "")
                'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), "Pdte", Me.mIndicadorDebe, CType("IMPUESTO FACTURA ANULADA ", String) & ValorImpuesto & "%" & " " & CType(Me.DbLeeNewPaga.mDbLector("DOCUMENTO"), String), Total)
                'Me.GeneraFileIV("IV", 40, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSeriefacturasNewPaga & Me.mFecha.Year, CType(Me.DbLeeNewPaga.mDbLector("NMOVI"), String), TotalBase, ValorImpuesto, Total, TipoImpuesto, CodigoImpuesto)
            End If

        End While
        Me.DbLeeNewPaga.mDbLector.Close()


    End Sub
#End Region
#Region "ASIENTO-4 DEVOLUCION ALBARANES"
    Private Sub TotalPendienteFormalizarDevolucionAlbaran()
        Dim Total As Double

        SQL = "SELECT SUM (TNST_MOVG.MOVG_VATO),TNST_MOVG.MOVG_ORIG AS MOVG_ORIG  "
        SQL += "FROM TNST_MOVG, TNST_TIMO "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += "AND TIMO_TIPO = " & Me.mTimo_Albaran_Dev
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " GROUP BY TNST_MOVG.MOVG_ORIG "

        If IsNumeric(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = True Then
            Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)

        Else
            Total = 0
        End If
        Linea = 1
        If Total <> 0 Then
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 4, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaFormalizaAlbaranes, Me.mIndicadorDebe, " DEVO. ALBARANES PENDIENTE DE FORMALIZAR", Total, "SI", "", "", CInt(Me.DbLeeHotel.mDbLector.Item("MOVG_ORIG")), "", "", 0, "", 0, "", "", 0)
            'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaFormalizaAlbaranes, Me.mIndicadorDebe, "DEVO. ALBARANES PENDIENTE DE FORMALIZAR", Total)

        End If
    End Sub
    Private Sub TotalPendienteFormalizarProveedorDevolucionAlbaranHTito()
        Dim Total As Double

        Try

            SQL = "SELECT MOVG_CODI,MOVG_ORIG,SUM(TNST_MOVG.MOVG_VATO)AS TOTAL,FORN_DESC AS PROVEEDOR ,NVL(MOVG_IDDO,'  ')AS DOCU,NVL(MOVG_DORE,' ')AS DORE ,TNST_FORN.FORN_CODI AS CODI,TNST_MOVG.MOVG_ORIG AS MOVG_ORIG "
            SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_FORN "
            SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
            SQL += "AND (TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI OR TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI)"
            SQL += "AND TIMO_TIPO = " & Me.mTimo_Albaran_Dev
            SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
            SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
            SQL += " GROUP BY MOVG_CODI,MOVG_ORIG,MOVG_IDDO,MOVG_DORE,TNST_FORN.FORN_CODI,FORN_DESC,TNST_MOVG.MOVG_ORIG"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                If Total <> 0 Then
                    Me.mTipoAsiento = "DEBE"
                    Me.InsertaOracle("AC", 4, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedorAlbaranes(CInt(Me.DbLeeHotel.mDbLector("CODI"))), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), CType(Me.DbLeeHotel.mDbLector("DORE"), String), CInt(Me.DbLeeHotel.mDbLector("MOVG_ORIG")), "", "", 0, "", 0, "", "", 0)

                End If

            End While
            Me.DbLeeHotel.mDbLector.Close()

        Catch ex As Exception
            MsgBox(ex.Message,, "Pdte Formalizar")
        End Try
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub GastosPorCentrodeCostoAlbaranesDevolucionAlbaran()
        Dim Total As Double
        Dim vCentroCosto As String

        SQL = "SELECT TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,TNST_GRUP.GRUP_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,GRUP_DESC AS GRUPO,TNST_MOVD.ALMA_CODI AS ALMACODI,TNST_GRUP.GRUP_CODI AS GRUPCODI"
        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_GRUP, TNST_TIMO"
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " AND (TNST_PROD.GRUP_CODI = TNST_GRUP.GRUP_CODI)"
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Albaran_Dev
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " GROUP BY  TNST_MOVG.TIMO_CODI,TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,"
        SQL += "TNST_GRUP.GRUP_CODI,"
        SQL += "TNST_ALMA.ALMA_DESC,"
        SQL += "TNST_GRUP.GRUP_DESC"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            SQL = "SELECT  NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 4, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo4Departamento(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorHaber, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("GRUPO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total, "NO", "0", "0")
            'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.ComponeCuentaGastoGrupo4Departamento(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorHaber, CType("GASTOS", String), Total)
            'Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 4, Me.ComponeCuentaGastoGrupo4Departamento(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), "0", vCentroCosto, Total)



        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub GastosPorCentrodeCostoAlbaranesDevolucionAlbaranAlmacen()
        Dim Total As Double
        Dim vCentroCosto As String


        SQL = "SELECT TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN"
        SQL += " ,TNST_MOVD.ALMA_CODI AS ALMACODI "
        SQL += ",NVL(MOVG_IDDO,'  ')AS DOCU"
        SQL += ",TNST_PROD.PROD_CODI AS PRODUCTO ,PROD_DESC"
        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD,  TNST_TIMO"
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " And (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " And (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " And (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " And (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " And TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Albaran_Dev
        SQL += " And TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " GROUP BY TNST_MOVG.TIMO_CODI,TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,"

        SQL += "TNST_ALMA.ALMA_DESC"

        SQL += ",MOVG_IDDO,TNST_PROD.PROD_CODI,PROD_DESC"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            SQL = "SELECT  NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            '   Me.InsertaOracle("AC", 4, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, "", Me.mIndicadorHaber, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total, "NO", "0", "0", CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer))
            Me.InsertaOracle("AC", 4, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaGastosProducto(Me.DbLeeHotel.mDbLector("PRODUCTO")), Me.mIndicadorHaber, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("PROD_DESC"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "", CType(Me.DbLeeHotel.mDbLector("ALMA_CODI"), Integer), "", "", 0, "", 0, "", "", 0)



        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
#End Region
#Region "ASIENTO-5 DEVOLUCION DE FACTURAS"
    Private Sub TotalFacturasProveedorDevolucion()
        Dim Total As Double
        '  Dim Totalbase As Double
        Dim Texto As String

        SQL = "SELECT MOVG_DADO,MOVG_CODI AS NMOVI,MOVG_ANCI,MOVG_DEST,SUM(TNST_MOVG.MOVG_VATO)AS TOTAL,FORN_DESC AS PROVEEDOR,TNST_FORN.FORN_CODI AS CODI, "
        SQL += "NVL(TNST_MOVG.MOVG_IDDO,' ')AS DOCU,'0' AS BASE,NVL(FORN_CNTR,'0') AS CIF,TNST_MOVG.MOVG_ORIG AS MOVG_ORIG "
        SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_FORN "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += "AND (TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI OR TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI)"
        SQL += "AND TIMO_TIPO = " & Me.mTimo_Factura_Directa_Dev
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        ' SQL += " GROUP BY MOVG_CODI,MOVG_ANCI,MOVG_DEST,TNST_FORN.FORN_CODI,FORN_DESC"
        SQL += " GROUP BY  MOVG_DADO,MOVG_CODI,MOVG_ANCI,MOVG_DEST,TNST_FORN.FORN_CODI,FORN_DESC,TNST_MOVG.MOVG_IDDO,FORN_CNTR,TNST_MOVG.MOVG_ORIG"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            '    Totalbase = Me.CalculaBaseImponible(CType(Me.DbLeeHotel.mDbLector("NMOVI"), Integer), CType(Me.DbLeeHotel.mDbLector("MOVG_ANCI"), Integer))


            Texto = "Fra Dir:" & CType(Me.DbLeeHotel.mDbLector("DOCU"), String) & " " & Format(Me.mFecha, "MMMM/yyyy") & " " & CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String)

            ' mery
            Texto = "Fra Dir:" & CType(Me.DbLeeHotel.mDbLector("DOCU"), String) & " " & CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String)


            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedor(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), Me.mIndicadorHaberAbono, Texto, Total * -1, "NO", "", "", "NEWSTOCK", CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("MOVG_DADO"), Date), CInt(Me.DbLeeHotel.mDbLector("MOVG_ORIG")))

            ' sale a hacer los aspuntes de GASTOS , en vez de todos juntos 
            Me.GastosPorCentrodeCostoFacturasDevolucion(CType(Me.DbLeeHotel.mDbLector("NMOVI"), Integer), CType(Me.DbLeeHotel.mDbLector("MOVG_ANCI"), Integer), Texto)
            ' sale a hacer los aspuntes de IGIC , en vez de todos juntos 
            Me.TotalFacturasProveedorImpuestoDevolucion(CType(Me.DbLeeHotel.mDbLector("NMOVI"), Integer), CType(Me.DbLeeHotel.mDbLector("MOVG_ANCI"), Integer), Texto)



        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub GastosPorCentrodeCostoFacturasDevolucion(ByVal vCodi As Integer, ByVal vAnci As Integer, ByVal vTexto As String)
        Dim Total As Double
        Dim vCentroCosto As String






        SQL = "SELECT TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, "
        SQL += "TNST_MOVD.ALMA_CODI, SUM(TNST_MOVD.MOVD_TOTA-TNST_MOVD.MOVD_IVAS)AS TOTAL,ALMA_DESC AS ALMACEN,TNST_MOVD.ALMA_CODI AS ALMACODI"
        SQL += ",TNST_PROD.PROD_CODI AS PRODUCTO,PROD_DESC"
        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD,  TNST_TIMO"


        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Factura_Directa_Dev
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"


        SQL += " AND TNST_MOVG.MOVG_CODI =  " & vCodi
        SQL += " AND TNST_MOVG.MOVG_ANCI =  " & vAnci


        SQL += " GROUP BY  TNST_MOVG.TIMO_CODI,TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,"
        SQL += "TNST_TIMO.TIMO_TIPO,"
        SQL += "TNST_ALMA.ALMA_DESC"
        SQL += ",TNST_PROD.PROD_CODI,PROD_DESC"


        Me.DbLeeHotelAux5.TraerLector(SQL)

        While Me.DbLeeHotelAux5.mDbLector.Read

            SQL = "SELECT  NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotelAux5.mDbLector("ALMACODI"), Integer)
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotelAux5.mDbLector("TOTAL"), Double)


            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, BuscaCuentaGastosProducto(Me.DbLeeHotelAux5.mDbLector("PRODUCTO")), Me.mIndicadorDebe, CType("GASTOS", String) & " " & CType(Me.DbLeeHotelAux5.mDbLector("PROD_DESC"), String) & " " & CType(Me.DbLeeHotelAux5.mDbLector("ALMACEN"), String), Total * -1, "NO", "0", "", CType(Me.DbLeeHotelAux5.mDbLector("ALMACODI"), Integer), "", "", 0, "", 0, "", "", 0)



        End While
        Me.DbLeeHotelAux5.mDbLector.Close()

    End Sub

    Private Sub TotalFacturasProveedorImpuestoDevolucion(ByVal vCodi As Integer, ByVal vAnci As Integer, ByVal vTexto As String)
        Dim Total As Double
        Dim TotalBase As Double
        Dim ValorImpuesto As Double
        Dim TipoImpuesto As String
        Dim CodigoImpuesto As String


        SQL = "SELECT TNST_MOVG.MOVG_CODI AS NMOVI,MOVG_ORIG,SUM(TNST_MOVI.MOVI_IMPU)AS TOTAL ,SUM(TNST_MOVI.MOVI_NETO)AS BASE,NVL(MOVG_IDDO,' ')AS DOCU ,MOVI_TAXA AS TIPO ,TNST_MOVG.MOVG_ORIG AS MOVG_ORIG "
        SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_MOVI "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Factura_Directa_Dev
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"

        SQL += " AND TNST_MOVG.MOVG_CODI =  " & vCodi
        SQL += " AND TNST_MOVG.MOVG_ANCI =  " & vAnci


        SQL += " AND TNST_MOVG.MOVG_CODI = TNST_MOVI.MOVG_CODI "
        SQL += " AND TNST_MOVG.MOVG_ANCI = TNST_MOVI.MOVG_ANCI "
        SQL += " GROUP BY TNST_MOVG.MOVG_CODI,MOVG_ORIG,MOVG_IDDO,MOVI_TAXA,TNST_MOVG.MOVG_ORIG"

        Me.DbLeeHotelAux6.TraerLector(SQL)

        While Me.DbLeeHotelAux6.mDbLector.Read
            SQL = "SELECT IVAS_TAXA FROM TNST_IVAS WHERE IVAS_CODI = " & CType(Me.DbLeeHotelAux6.mDbLector("TIPO"), Double)
            ValorImpuesto = CType(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL), Double)



            SQL = "SELECT NVL(IMPU_TIPO,'0') FROM TS_IMPU WHERE IMPU_VALO = " & ValorImpuesto
            CodigoImpuesto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

            '' 2017
            ' SQL = "SELECT NVL(CFIVATIP_COD,'0') FROM CFIVAPOR WHERE PIMPUESTO = " & ValorImpuesto
            ' SQL += "  AND CFIVATIMPU_COD ='" & CodigoImpuesto & "'"
            ' TipoImpuesto = Me.DbSpyro.EjecutaSqlScalar(SQL)


            '' 2017
            SQL = "SELECT NVL(IMPU_CFIVATIP,'?') AS  IMPU_CFIVATIP  FROM TS_IMPU  WHERE IMPU_VALO = " & ValorImpuesto
            'TipoImpuesto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)
            TipoImpuesto = "X"


            If IsNothing(TipoImpuesto) = True Then
                TipoImpuesto = "?"
                MsgBox("No se Localiza Equivalente de Impuesto en TS_IMPU " & vbCrLf & vbCrLf & SQL, MsgBoxStyle.Information, "Atención")
            End If



            Linea = Linea + 1
            Total = CType(Me.DbLeeHotelAux6.mDbLector("TOTAL"), Double)
            TotalBase = CType(Me.DbLeeHotelAux6.mDbLector("BASE"), Double)


            Me.mTipoAsiento = "DEBE"
            If TotalBase <> 0 Then
                Me.InsertaOracle("AC", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaImpuestoVenta(ValorImpuesto), Me.mIndicadorDebe, CType("IMPUESTO FACTURA ", String) & ValorImpuesto & "%", Total * -1, "NO", CType(Me.DbLeeHotelAux6.mDbLector("DOCU"), String), "", CInt(Me.DbLeeHotelAux6.mDbLector("MOVG_ORIG")), "", "", 0, "", 0, "", "", 0)
                'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux6.mDbLector("MOVG_ORIG"))), CType(Now.Year, String), Me.BuscaCuentaImpuestoDevolucion2(ValorImpuesto), Me.mIndicadorDebe, vTexto, Total * -1)
                'Me.GeneraFileIV("IV", 5, Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotelAux6.mDbLector("MOVG_ORIG"))), Me.mParaSeriefacturas & Me.mFecha.Year, CType(Me.DbLeeHotelAux6.mDbLector("NMOVI"), String), TotalBase * -1, ValorImpuesto, Total * -1, TipoImpuesto, CodigoImpuesto)
            End If


        End While
        Me.DbLeeHotelAux6.mDbLector.Close()

    End Sub
#End Region
#Region "ASIENTO-6 FACTURAS ALBARANES FORMALIZADAS"
    Private Sub TotalFacturasProveedorFormalizadas()
        Dim Total As Double
        Dim TotalBase As Double

        Dim Texto As String

        SQL = "SELECT MOVG_DADO,TNST_MOVG.MOVG_CODI AS NMOVI,TNST_MOVG.MOVG_ANCI,MOVG_ORIG,MAX(TNST_MOVG.MOVG_VATO)AS TOTAL,FORN_DESC AS PROVEEDOR,"
        SQL += "TNST_MOVG.MOVG_IDDO AS DOCU,TNST_FORN.FORN_CODI AS CODI,SUM(MOVI_NETO) AS BASE,NVL(FORN_CNTR,'0') AS CIF ,TNST_MOVG.MOVG_DEST AS MOVG_DEST "
        SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_FORN,TNST_MOVI,TNST_ALMA "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += "AND (TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI OR TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI)"
        SQL += "AND TIMO_TIPO = " & Me.mTimo_Factura_Al
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"

        ' para que no coga las facturas que contienen solo devoluciones ( )
        SQL += " AND TNST_MOVG.MOVG_ORIG <> TNST_MOVG.MOVG_DEST"
        ' Esta FActura NO tiene NINGUN albaran asociado !!!!
        ' hay un programa en vb 2010  facturas lopez que detecta este problema 
        '  SQL += " AND TNST_MOVG.MOVG_IDDO <> '20036 EN12 R'"

        SQL += " AND TNST_MOVG.MOVG_CODI = TNST_MOVI.MOVG_CODI "
        SQL += " AND TNST_MOVG.MOVG_ANCI = TNST_MOVI.MOVG_ANCI "

        SQL += " AND TNST_MOVG.MOVG_DEST = TNST_ALMA.ALMA_CODI"

        SQL += " GROUP BY MOVG_DADO,TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_ANCI,"
        SQL += " MOVG_ORIG,TNST_FORN.FORN_CODI,FORN_DESC,TNST_MOVG.MOVG_IDDO,FORN_CNTR,TNST_MOVG.MOVG_DEST "
        SQL += " ,ALMA_CCST"


        SQL += " ORDER BY  FORN_DESC,SUBSTR(ALMA_CCST,3,2),  TNST_MOVG.MOVG_IDDO "
        '1X'

        ' RECORER DATASET EN VEZ DE LECTOR POR VELOCIDAD 
        Me.DbLeeHotel.TraerDataset(SQL, "FACTURAS")

        For Each r As DataRow In Me.DbLeeHotel.mDbDataset.Tables(0).Rows

            Linea = Linea + 1
            Total = CType(r("TOTAL"), Double)
            TotalBase = CType(r("BASE"), Double)

            Texto = "Fra:" & CType(r("DOCU"), String) & " " & Format(Me.mFecha, "MMMM/yyyy") & " " & CType(r("PROVEEDOR"), String)
            ' mery
            Texto = "Fra:" & CType(r("DOCU"), String) & " " & CType(r("PROVEEDOR"), String)

            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedor(CType(r("CODI"), Integer)), Me.mIndicadorHaberFac, Texto, Total, "NO", CType(r("DOCU"), String), "", "NEWSTOCK", CType(r("CIF"), String), CType(r("MOVG_DADO"), Date), CInt(r("MOVG_DEST")))


            ' si' AGRUPA POR PROVEEDOR / ALBARAN
            Me.TotalAlbaranesProveedorFormalizados(CType(r("NMOVI"), Integer), CType(r("MOVG_ANCI"), Integer), Texto)


        Next

        Me.DbLeeHotel.mDbDataset.Clear()





    End Sub

    Private Sub TotalFacturasProveedorFormalizadasTEST()
        Dim Total As Double
        Dim TotalBase As Double

        Dim Texto As String

        SQL = "SELECT MOVG_DADO,TNST_MOVG.MOVG_CODI AS NMOVI,TNST_MOVG.MOVG_ANCI,MOVG_ORIG,MAX(TNST_MOVG.MOVG_VATO)AS TOTAL,FORN_DESC AS PROVEEDOR,"
        SQL += "TNST_MOVG.MOVG_IDDO AS DOCU,TNST_FORN.FORN_CODI AS CODI,SUM(MOVI_NETO) AS BASE,NVL(FORN_CNTR,'0') AS CIF ,TNST_MOVG.MOVG_DEST AS MOVG_DEST "
        SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_FORN,TNST_MOVI "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += "AND (TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI OR TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI)"
        SQL += "AND TIMO_TIPO = " & Me.mTimo_Factura_Al
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"

        ' para que no coga las facturas que contienen solo devoluciones ( )
        SQL += " AND TNST_MOVG.MOVG_ORIG <> TNST_MOVG.MOVG_DEST"


        SQL += " AND TNST_MOVG.MOVG_CODI = TNST_MOVI.MOVG_CODI "
        SQL += " AND TNST_MOVG.MOVG_ANCI = TNST_MOVI.MOVG_ANCI "

        SQL += " GROUP BY MOVG_DADO,TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_ANCI,MOVG_ORIG,TNST_FORN.FORN_CODI,FORN_DESC,TNST_MOVG.MOVG_IDDO,FORN_CNTR,TNST_MOVG.MOVG_DEST "

        Me.DbLeeHotel.TraerLector(SQL)


        ' RECORER DATASET EN VEZ DE LECTOR POR VELOCIDAD 
        'Me.DbLeeHotel.TraerDataset(SQL, "FACTURAS")

        'For Each r As DataRow In Me.DbLeeHotel.mDbDataset.Tables(0).Rows
        'MsgBox(r("DOCU").ToString())

        'Next



        While Me.DbLeeHotel.mDbLector.Read
            'SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
            'vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)

            Texto = "Fra:" & CType(Me.DbLeeHotel.mDbLector("DOCU"), String) & " " & Format(Me.mFecha, "MMMM/yyyy") & " " & CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String)
            ' mery
            Texto = "Fra:" & CType(Me.DbLeeHotel.mDbLector("DOCU"), String) & " " & CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String)

            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedor(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), Me.mIndicadorHaberFac, Texto, Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "", "NEWSTOCK", CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("MOVG_DADO"), Date), CInt(Me.DbLeeHotel.mDbLector("MOVG_DEST")))
            'Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotel.mDbLector("MOVG_DEST"))), CType(Now.Year, String), Me.BuscaCuentaProveedorCuentasPorPagar(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), Me.mIndicadorHaberFac, Texto, Total, Me.mParaSeriefacturas & Me.mFecha.Year, CType(Me.DbLeeHotel.mDbLector("NMOVI"), String))
            'Me.GeneraFileFV("FV", 6, Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotel.mDbLector("MOVG_DEST"))), Me.mParaSeriefacturas & Me.mFecha.Year, CType(Me.DbLeeHotel.mDbLector("NMOVI"), String), Total, CType(Me.DbLeeHotel.mDbLector("DOCU"), String).PadRight(15, CChar(" ")), Me.BuscaCuentaProveedorCuentasPorPagar(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("MOVG_DADO"), Date))
            '1X'

            Me.TotalAlbaranesProveedorFormalizados(CType(Me.DbLeeHotel.mDbLector("NMOVI"), Integer), CType(Me.DbLeeHotel.mDbLector("MOVG_ANCI"), Integer), "")


        End While
        Me.DbLeeHotel.mDbLector.Close()

        If Me.mDebug = True Then
            '  Me.GastosPorCentrodeCostoAlbaranesFacturaGuia()
        End If

    End Sub
    Private Sub TotalFacturasProveedorImpuestoFormalizadas()
        Dim Total As Double
        Dim TotalBase As Double
        Dim ValorImpuesto As Double
        Dim TipoImpuesto As String
        Dim CodigoImpuesto As String

        SQL = "SELECT TNST_MOVG.MOVG_CODI AS NMOVI,MOVG_ORIG,SUM(TNST_MOVI.MOVI_IMPU)AS TOTAL ,SUM(TNST_MOVI.MOVI_NETO)AS BASE,NVL(MOVG_IDDO,' ')AS DOCU,MOVI_TAXA AS TIPO,TNST_MOVG.MOVG_DEST AS MOVG_DEST "
        SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_MOVI "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += "AND TIMO_TIPO = " & Me.mTimo_Factura_Al
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " AND TNST_MOVG.MOVG_CODI = TNST_MOVI.MOVG_CODI "
        SQL += " AND TNST_MOVG.MOVG_ANCI = TNST_MOVI.MOVG_ANCI "
        '   SQL += " AND TNST_MOVG.MOVG_IDDO <> 'FV6/7409'"


        ' para que no coga las facturas que contienen solo devoluciones ( )
        SQL += " AND TNST_MOVG.MOVG_ORIG <> TNST_MOVG.MOVG_DEST"
        ' Esta FActura NO tiene NINGUN albaran asociado !!!!
        ' hay un programa en vb 2010  facturas lopez que detecta este problema 
        ' SQL += " AND TNST_MOVG.MOVG_IDDO <> '20036 EN12 R'"

        SQL += " GROUP BY TNST_MOVG.MOVG_CODI,MOVG_ORIG,MOVG_IDDO,MOVI_TAXA,TNST_MOVG.MOVG_DEST"


        Try


            ' RECORER DATASET EN VEZ DE LECTOR POR VELOCIDAD 
            Me.DbLeeHotel.TraerDataset(SQL, "IMPUESTOS")

            For Each r As DataRow In Me.DbLeeHotel.mDbDataset.Tables(0).Rows

                If Me.mConectarNewCentral = 0 Then
                    SQL = "SELECT IVAS_TAXA FROM TNST_IVAS WHERE IVAS_CODI = " & CType(r("TIPO"), Double)
                    ValorImpuesto = CType(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL), Double)
                Else
                    SQL = "SELECT IVAS_TAXA FROM TNCC_NS_IVAS WHERE IVAS_CODI = " & CType(r("TIPO"), Double)
                    ValorImpuesto = CType(Me.DbNewCentral.EjecutaSqlScalar(SQL), Double)
                End If




                SQL = "SELECT NVL(IMPU_TIPO,'0') FROM TS_IMPU WHERE IMPU_VALO = " & ValorImpuesto
                CodigoImpuesto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

                '' 2017
                ' SQL = "SELECT NVL(CFIVATIP_COD,'0') FROM CFIVAPOR WHERE PIMPUESTO = " & ValorImpuesto
                'SQL += "  AND CFIVATIMPU_COD ='" & CodigoImpuesto & "'"
                'TipoImpuesto = Me.DbSpyro.EjecutaSqlScalar(SQL)

                '' 2017
                SQL = "SELECT NVL(IMPU_CFIVATIP,'?') AS  IMPU_CFIVATIP  FROM TS_IMPU  WHERE IMPU_VALO = " & ValorImpuesto
                'TipoImpuesto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)
                TipoImpuesto = "X"

                If IsNothing(TipoImpuesto) = True Then
                    TipoImpuesto = "?"
                    MsgBox("No se Localiza Equivalente de Impuesto en TS_IMPU " & vbCrLf & vbCrLf & SQL, MsgBoxStyle.Information, "Atención")
                End If


                Linea = Linea + 1
                Total = CType(r("TOTAL"), Double)
                TotalBase = CType(r("BASE"), Double)
                Me.mTipoAsiento = "DEBE"
                If TotalBase <> 0 Then
                    Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaImpuestoVenta(ValorImpuesto), Me.mIndicadorDebe, CType("IMPUESTO FACTURA ", String) & ValorImpuesto & "%", Total, "NO", CType(r("DOCU"), String), "", CInt(r("MOVG_DEST")), "", "", 0, "", 0, "", "", 0)
                End If

            Next

            Me.DbLeeHotel.mDbDataset.Clear()



            'Me.DbLeeHotel.TraerLector(SQL)

            'While Me.DbLeeHotel.mDbLector.Read
            'SQL = "SELECT IVAS_TAXA FROM TNST_IVAS WHERE IVAS_CODI = " & CType(Me.DbLeeHotel.mDbLector("TIPO"), Double)
            'ValorImpuesto = CType(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL), Double)


            'SQL = "SELECT NVL(IMPU_TIPO,'0') FROM TS_IMPU WHERE IMPU_VALO = " & ValorImpuesto
            'CodigoImpuesto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

            'SQL = "SELECT NVL(CFIVATIP_COD,'0') FROM CFIVAPOR WHERE PIMPUESTO = " & ValorImpuesto
            'SQL += "  AND CFIVATIMPU_COD ='" & CodigoImpuesto & "'"

            'TipoImpuesto = Me.DbSpyro.EjecutaSqlScalar(SQL)

            'If IsNothing(TipoImpuesto) = True Then
            'TipoImpuesto = "?"
            'MsgBox("No se Localiza Equivalente de Impuesto en Spyro CFIVAPOR " & vbCrLf & vbCrLf & SQL, MsgBoxStyle.Information, "Atención")
            'End If


            'Linea = Linea + 1
            'Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            'TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            'Me.mTipoAsiento = "DEBE"
            'If TotalBase <> 0 Then
            'Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaImpuestoVenta2(ValorImpuesto), Me.mIndicadorDebe, CType("IMPUESTO FACTURA ", String) & ValorImpuesto & "%", Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "", CInt(Me.DbLeeHotel.mDbLector("MOVG_DEST")))
            'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotel.mDbLector("MOVG_DEST"))), CType(Now.Year, String), Me.BuscaCuentaImpuestoVenta2(ValorImpuesto), Me.mIndicadorDebe, CType("IMPUESTO FACTURA ", String) & ValorImpuesto & "%" & " " & CType(Me.DbLeeHotel.mDbLector("DOCU"), String), Total)
            'Me.GeneraFileIV("IV", 6, Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotel.mDbLector("MOVG_DEST"))), Me.mParaSeriefacturas & Me.mFecha.Year, CType(Me.DbLeeHotel.mDbLector("NMOVI"), String), TotalBase, ValorImpuesto, Total, TipoImpuesto, CodigoImpuesto)
            'End If

            'End While
            'Me.DbLeeHotel.mDbLector.Close()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Sub

    Private Sub TotalFacturasProveedorImpuestoFormalizadas(ByVal vCodi As Integer, ByVal vAnci As Integer, ByVal vTexto As String)
        Dim Total As Double
        Dim TotalBase As Double
        Dim ValorImpuesto As Double
        Dim TipoImpuesto As String
        Dim CodigoImpuesto As String

        SQL = "SELECT TNST_MOVG.MOVG_CODI AS NMOVI,MOVG_ORIG,SUM(TNST_MOVI.MOVI_IMPU)AS TOTAL ,SUM(TNST_MOVI.MOVI_NETO)AS BASE,NVL(MOVG_IDDO,' ')AS DOCU,MOVI_TAXA AS TIPO,TNST_MOVG.MOVG_DEST AS MOVG_DEST "
        SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_MOVI "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += "AND TIMO_TIPO = " & Me.mTimo_Factura_Al
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " AND TNST_MOVG.MOVG_CODI = TNST_MOVI.MOVG_CODI "
        SQL += " AND TNST_MOVG.MOVG_ANCI = TNST_MOVI.MOVG_ANCI "


        SQL += " AND TNST_MOVG.MOVG_CODI =  " & vCodi
        SQL += " AND TNST_MOVG.MOVG_ANCI =  " & vAnci


        '   SQL += " AND TNST_MOVG.MOVG_IDDO <> 'FV6/7409'"


        ' para que no coga las facturas que contienen solo devoluciones ( )
        SQL += " AND TNST_MOVG.MOVG_ORIG <> TNST_MOVG.MOVG_DEST"
        ' Esta FActura NO tiene NINGUN albaran asociado !!!!
        ' hay un programa en vb 2010  facturas lopez que detecta este problema 
        ' SQL += " AND TNST_MOVG.MOVG_IDDO <> '20036 EN12 R'"

        SQL += " GROUP BY TNST_MOVG.MOVG_CODI,MOVG_ORIG,MOVG_IDDO,MOVI_TAXA,TNST_MOVG.MOVG_DEST"


        Try


            ' RECORER DATASET EN VEZ DE LECTOR POR VELOCIDAD 
            Me.DbLeeHotel.TraerDataset(SQL, "IMPUESTOS")

            For Each r As DataRow In Me.DbLeeHotel.mDbDataset.Tables(0).Rows

                If Me.mConectarNewCentral = 0 Then
                    SQL = "SELECT IVAS_TAXA FROM TNST_IVAS WHERE IVAS_CODI = " & CType(r("TIPO"), Double)
                    ValorImpuesto = CType(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL), Double)
                Else
                    SQL = "SELECT IVAS_TAXA FROM TNCC_NS_IVAS WHERE IVAS_CODI = " & CType(r("TIPO"), Double)
                    ValorImpuesto = CType(Me.DbNewCentral.EjecutaSqlScalar(SQL), Double)
                End If




                SQL = "SELECT NVL(IMPU_TIPO,'0') FROM TS_IMPU WHERE IMPU_VALO = " & ValorImpuesto
                CodigoImpuesto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

                '' 2017
                ' SQL = "SELECT NVL(CFIVATIP_COD,'0') FROM CFIVAPOR WHERE PIMPUESTO = " & ValorImpuesto
                'SQL += "  AND CFIVATIMPU_COD ='" & CodigoImpuesto & "'"
                'TipoImpuesto = Me.DbSpyro.EjecutaSqlScalar(SQL)

                '' 2017
                SQL = "SELECT NVL(IMPU_CFIVATIP,'?') AS  IMPU_CFIVATIP  FROM TS_IMPU  WHERE IMPU_VALO = " & ValorImpuesto
                'TipoImpuesto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)
                TipoImpuesto = "X"

                If IsNothing(TipoImpuesto) = True Then
                    TipoImpuesto = "?"
                    MsgBox("No se Localiza Equivalente de Impuesto en TS_IMPU " & vbCrLf & vbCrLf & SQL, MsgBoxStyle.Information, "Atención")
                End If


                Linea = Linea + 1
                Total = CType(r("TOTAL"), Double)
                TotalBase = CType(r("BASE"), Double)
                Me.mTipoAsiento = "DEBE"
                If TotalBase <> 0 Then
                    If Me.mCondensarAsiento = True Then
                        Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaImpuestoVenta(ValorImpuesto), Me.mIndicadorDebe, CType(" -- IMPUESTO FACTURA ", String) & ValorImpuesto & "%", Total, "NO", CType(r("DOCU"), String), "", CInt(r("MOVG_DEST")), "", "", 0, "", 0, "", "", 0)
                    Else
                        Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaImpuestoVenta(ValorImpuesto), Me.mIndicadorDebe, CType(" -- IMPUESTO FACTURA ", String) & " " & CType(r("DOCU"), String) & " " & ValorImpuesto & "%", Total, "NO", CType(r("DOCU"), String), "", CInt(r("MOVG_DEST")), "", "", 0, "", 0, "", "", 0)

                    End If

                End If

            Next

            Me.DbLeeHotel.mDbDataset.Clear()



            'Me.DbLeeHotel.TraerLector(SQL)

            'While Me.DbLeeHotel.mDbLector.Read
            'SQL = "SELECT IVAS_TAXA FROM TNST_IVAS WHERE IVAS_CODI = " & CType(Me.DbLeeHotel.mDbLector("TIPO"), Double)
            'ValorImpuesto = CType(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL), Double)


            'SQL = "SELECT NVL(IMPU_TIPO,'0') FROM TS_IMPU WHERE IMPU_VALO = " & ValorImpuesto
            'CodigoImpuesto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

            'SQL = "SELECT NVL(CFIVATIP_COD,'0') FROM CFIVAPOR WHERE PIMPUESTO = " & ValorImpuesto
            'SQL += "  AND CFIVATIMPU_COD ='" & CodigoImpuesto & "'"

            'TipoImpuesto = Me.DbSpyro.EjecutaSqlScalar(SQL)

            'If IsNothing(TipoImpuesto) = True Then
            'TipoImpuesto = "?"
            'MsgBox("No se Localiza Equivalente de Impuesto en Spyro CFIVAPOR " & vbCrLf & vbCrLf & SQL, MsgBoxStyle.Information, "Atención")
            'End If


            'Linea = Linea + 1
            'Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            'TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            'Me.mTipoAsiento = "DEBE"
            'If TotalBase <> 0 Then
            'Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaImpuestoVenta2(ValorImpuesto), Me.mIndicadorDebe, CType("IMPUESTO FACTURA ", String) & ValorImpuesto & "%", Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "", CInt(Me.DbLeeHotel.mDbLector("MOVG_DEST")))
            'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotel.mDbLector("MOVG_DEST"))), CType(Now.Year, String), Me.BuscaCuentaImpuestoVenta2(ValorImpuesto), Me.mIndicadorDebe, CType("IMPUESTO FACTURA ", String) & ValorImpuesto & "%" & " " & CType(Me.DbLeeHotel.mDbLector("DOCU"), String), Total)
            'Me.GeneraFileIV("IV", 6, Me.mEmpGrupoCod, Me.DameHotelapartirdeAlmacen(CInt(Me.DbLeeHotel.mDbLector("MOVG_DEST"))), Me.mParaSeriefacturas & Me.mFecha.Year, CType(Me.DbLeeHotel.mDbLector("NMOVI"), String), TotalBase, ValorImpuesto, Total, TipoImpuesto, CodigoImpuesto)
            'End If

            'End While
            'Me.DbLeeHotel.mDbLector.Close()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Sub
    Private Sub TotalAlbaranesProveedorFormalizados(ByVal vCodi As Integer, ByVal vAnci As Integer, ByVal vTexto As String)
        Dim Total As Double

        SQL = "SELECT TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_ANCI,QWE_VNST_GUIA.TIPO ,SUM (QWE_VNST_GUIA.MOVG_VATO)AS TOTAL, QWE_VNST_GUIA.MOVG_IDDO AS ALBARAN, TNST_MOVG.MOVG_ORIG,TNST_FORN.FORN_CODI AS CODI,"
        SQL += " TNST_FORN.FORN_DESC AS PROVEEDOR , QWE_VNST_GUIA.MOVG_CODI AS C1 , QWE_VNST_GUIA.MOVG_ANCI AS C2,TNST_MOVG.MOVG_ORIG as MOVG_ORIG,TNST_MOVG.MOVG_DEST as MOVG_DEST "
        SQL += " FROM TNST_MOVG, QWE_VNST_GUIA, TNST_TIMO, TNST_FORN "
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = QWE_VNST_GUIA.DORE_CODI) "
        SQL += " AND (TNST_MOVG.MOVG_ANCI = QWE_VNST_GUIA.DORE_ANCI) "
        SQL += " AND QWE_VNST_GUIA.DORE_CODI = " & vCodi
        SQL += " AND QWE_VNST_GUIA.DORE_ANCI = " & vAnci

        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI) "
        SQL += " AND (TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI OR TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI) "

        SQL += " AND (QWE_VNST_GUIA.TIPO = " & Me.mTimo_Albaran
        SQL += " OR QWE_VNST_GUIA.TIPO= " & Me.mTimo_Albaran_Dev
        SQL += ") AND TNST_MOVG.MOVG_ANUL = 0 "


        SQL += "GROUP BY  TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_ANCI,QWE_VNST_GUIA.TIPO,QWE_VNST_GUIA.MOVG_IDDO, TNST_MOVG.MOVG_ORIG, TNST_FORN.FORN_CODI,TNST_FORN.FORN_DESC, QWE_VNST_GUIA.MOVG_CODI, QWE_VNST_GUIA.MOVG_ANCI,TNST_MOVG.MOVG_ORIG,TNST_MOVG.MOVG_DEST"

        '2X'

        Me.DbLeeHotelAux2.TraerLector(SQL)

        While Me.DbLeeHotelAux2.mDbLector.Read

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotelAux2.mDbLector("TOTAL"), Double)

            If CType(Me.DbLeeHotelAux2.mDbLector("TIPO"), Integer) = Me.mTimo_Albaran Then
                Linea = Linea + 1
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedorAlbaranes(CInt(Me.DbLeeHotelAux2.mDbLector("CODI"))), Me.mIndicadorDebe, CType(" --FORMALIZADO ", String) & " " & CType(Me.DbLeeHotelAux2.mDbLector("PROVEEDOR"), String).Replace("'", "''"), Total, "NO", CType(Me.DbLeeHotelAux2.mDbLector("ALBARAN"), String), "", CInt(Me.DbLeeHotelAux2.mDbLector("MOVG_DEST")), "", "", 0, "", 0, "", "", 0)
            End If
            If CType(Me.DbLeeHotelAux2.mDbLector("TIPO"), Integer) = Me.mTimo_Albaran_Dev Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedorAlbaranes(CInt(Me.DbLeeHotelAux2.mDbLector("CODI"))), Me.mIndicadorHaber, CType(" --FORMALIZADO ", String) & " " & CType(Me.DbLeeHotelAux2.mDbLector("PROVEEDOR"), String).Replace("'", "''"), Total, "NO", CType(Me.DbLeeHotelAux2.mDbLector("ALBARAN"), String), "", CInt(Me.DbLeeHotelAux2.mDbLector("MOVG_DEST")), "", "", 0, "", 0, "", "", 0)
            End If



        End While
        Me.DbLeeHotelAux2.mDbLector.Close()
    End Sub
    Private Sub TotalAlbaranesProveedorFormalizadosDepartamentoGrupo(ByVal vCodi As Integer, ByVal vAnci As Integer)
        Dim Total As Double

        ' ANTES COSTOLIQUIDO CONTRA CONSUMOS INTERMOS 

        Dim vCentroCosto As String

        SQL = "SELECT TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,TNST_FAMI.FAMI_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,"
        SQL += " FAMI_DESC AS FAMILIA,TNST_MOVD.ALMA_CODI AS ALMACODI,TNST_FAMI.FAMI_CODI AS FAMICODI,"

        SQL += " TNST_TIMO.TIMO_TIPO AS TIPO,NVL(MOVG_IDDO,'?') AS DOCUMENTO,TNST_MOVG.MOVG_DEST AS MOVG_DEST ,TNST_MOVG.MOVG_ORIG AS MOVG_ORIG "
        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_FAMI, TNST_TIMO"
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " AND (TNST_PROD.FAMI_CODI = TNST_FAMI.FAMI_CODI)"

        SQL += " AND TNST_MOVG.MOVG_CODI = " & vCodi
        SQL += " AND  TNST_MOVG.MOVG_ANCI= " & vAnci

        '  SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " GROUP BY TNST_MOVG.TIMO_CODI,TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,"
        SQL += "TNST_FAMI.FAMI_CODI,"
        SQL += "TNST_ALMA.ALMA_DESC,"
        SQL += "TNST_FAMI.FAMI_DESC,"
        SQL += "TNST_MOVG.MOVG_DORE,"
        SQL += " TNST_TIMO.TIMO_TIPO,"
        SQL += " TNST_MOVG.MOVG_IDDO,TNST_MOVG.MOVG_DEST,TNST_MOVG.MOVG_ORIG"
        Me.DbLeeHotelAux3.TraerLector(SQL)

        While Me.DbLeeHotelAux3.mDbLector.Read


            Total = CType(Me.DbLeeHotelAux3.mDbLector("TOTAL"), Double)


            If CType(Me.DbLeeHotelAux3.mDbLector("TIPO"), Integer) = Me.mTimo_Albaran Then

                SQL = "SELECT  NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotelAux3.mDbLector("MOVG_DEST"), Integer)
                vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


                Linea = Linea + 1
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 61, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorDebe, CType("GASTOS FACT.", String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String), Total, "NO", CType(Me.DbLeeHotelAux3.mDbLector("DOCUMENTO"), String), "", CInt(Me.DbLeeHotelAux3.mDbLector("MOVG_DEST")), "", "", 0, "", 0, "", "", 0)

                Me.mTipoAsiento = "HABER"
                Linea = Linea + 1
                Me.InsertaOracle("AC", 61, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo4Departamento(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer)), Me.mIndicadorHaber, CType("GASTOS FACT.", String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String), Total, "NO", CType(Me.DbLeeHotelAux3.mDbLector("DOCUMENTO"), String), "", CInt(Me.DbLeeHotelAux3.mDbLector("MOVG_DEST")), "", "", 0, "", 0, "", "", 0)

            End If
            If CType(Me.DbLeeHotelAux3.mDbLector("TIPO"), Integer) = Me.mTimo_Albaran_Dev Then

                SQL = "SELECT  NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotelAux3.mDbLector("MOVG_ORIG"), Integer)
                vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 61, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo4Departamento(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer)), Me.mIndicadorHaber, CType("DEV. GASTOS FACT.", String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String), Total, "NO", CType(Me.DbLeeHotelAux3.mDbLector("DOCUMENTO"), String), "", CInt(Me.DbLeeHotelAux3.mDbLector("MOVG_ORIG")), "", "", 0, "", 0, "", "", 0)


                Me.mTipoAsiento = "DEBE"
                Linea = Linea + 1
                Me.InsertaOracle("AC", 61, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotelAux3.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotelAux3.mDbLector("FAMICODI"), Integer)), Me.mIndicadorDebe, CType("DEV. GASTOS FACT.", String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("FAMILIA"), String) & " " & CType(Me.DbLeeHotelAux3.mDbLector("ALMACEN"), String), Total, "NO", CType(Me.DbLeeHotelAux3.mDbLector("DOCUMENTO"), String), "", CInt(Me.DbLeeHotelAux3.mDbLector("MOVG_ORIG")), "", "", 0, "", 0, "", "", 0)


            End If

        End While
        Me.DbLeeHotelAux3.mDbLector.Close()

    End Sub
    Private Sub TotalAlbaranesProveedorFormalizadosx(ByVal vCodi As Integer, ByVal vAnci As Integer)
        Dim Total As Double

        Try
            SQL = "SELECT  SUM (MOVG_VATO)AS TOTAL, MOVG_IDDO AS ALBARAN, TNST_MOVG.MOVG_ORIG,TNST_FORN.FORN_CODI AS CODI,"
            SQL += "TNST_FORN.FORN_DESC AS PROVEEDOR "
            SQL += "FROM TNST_MOVG, VNST_FACT_GUIA, TNST_TIMO, TNST_FORN "
            SQL += "WHERE (GUIA_CODI = MOVG_CODI AND GUIA_ANCI = MOVG_ANCI) "
            'SQL += "AND FACT_CODI = " & vCodi
            ' SQL += "AND FACT_ANCI =  " & vAnci
            SQL += "AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI) "
            SQL += "AND (TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI OR TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI) "



            SQL += "AND (VNST_FACT_GUIA.TIMO_TIPO = " & Me.mTimo_Factura_Al
            SQL += "OR VNST_FACT_GUIA.TIMO_TIPO = " & Me.mTimo_Albaran_Dev

            SQL += ") AND TNST_MOVG.MOVG_ANUL = 0"
            SQL += " AND FACT_CODI IS NOT NULL "
            '  SQL += " AND TNST_MOVG.MOVG_IDDO <> 'FV6/7409'"

            SQL += "GROUP BY MOVG_IDDO, TNST_MOVG.MOVG_ORIG, TNST_FORN.FORN_CODI,TNST_FORN.FORN_DESC"



            Me.DbLeeHotelAux2.TraerLector(SQL)

            While Me.DbLeeHotelAux2.mDbLector.Read
                Me.mTextDebug.Text = "Buscando Albaranes " & vCodi & "/" & vAnci
                Me.mTextDebug.Update()

                'SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
                'vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

                Linea = Linea + 1
                Total = CType(Me.DbLeeHotelAux2.mDbLector("TOTAL"), Double)
                MsgBox(Total)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedorAlbaranes(CType(Me.DbLeeHotelAux2.mDbLector("CODI"), Integer)), Me.mIndicadorDebe, CType("FORMALIZADO ", String) & " " & CType(Me.DbLeeHotelAux2.mDbLector("PROVEEDOR"), String).Replace("'", "''"), Total, "NO", CType(Me.DbLeeHotelAux2.mDbLector("ALBARAN"), String), "")
                'Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaProveedorAlbaranes(CType(Me.DbLeeHotelAux2.mDbLector("CODI"), Integer)), Me.mIndicadorDebe, CType("FORMALIZADO ", String) & CType(Me.DbLeeHotelAux2.mDbLector("ALBARAN"), String), Total)

            End While
            Me.DbLeeHotelAux2.mDbLector.Close()
            'MsgBox("SALIO")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
#End Region
#Region "ASIENTO-7 INVENTARIO FINAL"
    Private Sub InventariosFinal()
        Dim Total As Double

        Dim PrimerRegistro As Boolean = True
        Dim N_Inventario As Integer
        Dim Almacen As Integer
        Dim Grupo As Integer
        Dim Filename As String

        Dim FechaInventarioFinalStr As String

        Filename = "INVENTARIO FINAL " & DateTime.DaysInMonth(Me.mFecha.Year, Me.mFecha.Month) & "-" & Me.mFecha.Month & "-" & Me.mFecha.Year & ".txt"
        FechaInventarioFinalStr = DateTime.DaysInMonth(Me.mFecha.Year, Me.mFecha.Month) & "/" & Me.mFecha.Month & "/" & Me.mFecha.Year
        FechaInventarioFinal = CType(FechaInventarioFinalStr, Date)

        Me.BorraRegistrosInventario(FechaInventarioFinal)

        Me.CrearFichero(Me.mParaFilePath & Filename)

        Dim Existencia As Double

        SQL = "SELECT TNST_INVG.INVG_CODI as INVENTARIO,to_char(TNST_INVG.INVG_DAVA,'MM'),TNST_INVD.ALMA_CODI AS ALMACEN,ALMA_DESC AS ALMANOME,"
        SQL += "TNST_INVD.INVD_EXRE AS REAL,TNST_INVD.INVD_DORE AS DOSISREAL,"
        SQL += "TNST_INVD.INVD_PMCB AS PRECIO, TNST_PROD.GRUP_CODI AS GRUPO, TNST_PROD.PROD_DOSE AS DOSIS,GRUP_DESC AS GRUPNOME "
        SQL += "FROM TNST_INVG,TNST_INVD,TNST_PROD,TNST_UNME,TNST_GRUP,TNST_ALMA "
        SQL += "WHERE "
        SQL += "TNST_INVG.INVG_CODI = TNST_INVD.INVG_CODI AND "
        SQL += "TNST_INVG.INVG_ANCI = TNST_INVD.INVG_ANCI AND "
        SQL += "TNST_INVG.ALMA_CODI = TNST_ALMA.ALMA_CODI AND "
        SQL += "TNST_INVD.PROD_CODI = TNST_PROD.PROD_CODI AND "
        SQL += "TNST_PROD.UNME_CODI = TNST_UNME.UNME_CODI AND "
        SQL += "TNST_PROD.GRUP_CODI = TNST_GRUP.GRUP_CODI AND "
        SQL += " TO_CHAR(TNST_INVG.INVG_DAVA,'MM') = '" & Format(Me.mFecha, "MM") & "'"
        SQL += " AND TO_CHAR(TNST_INVG.INVG_DAVA,'YYYY') = '" & Format(Me.mFecha, "yyyy") & "'"

        If Me.mSoloInventariosIniciales = True Then
            SQL += "AND EXISTS(SELECT 'X' FROM VNST_MOIN "
            SQL += "WHERE TNST_INVD.INVG_CODI = VNST_MOIN.INVG_CODI AND "
            SQL += "TNST_INVD.INVG_ANCI = VNST_MOIN.INVG_ANCI AND "
            SQL += " TNST_INVD.PROD_CODI = VNST_MOIN.PROD_CODI AND VNST_MOIN.TIMO_ABRV = 'INI') "
        Else
            SQL += "AND NOT EXISTS(SELECT 'X' FROM VNST_MOIN "
            SQL += "WHERE TNST_INVD.INVG_CODI = VNST_MOIN.INVG_CODI AND "
            SQL += "TNST_INVD.INVG_ANCI = VNST_MOIN.INVG_ANCI AND "
            SQL += " TNST_INVD.PROD_CODI = VNST_MOIN.PROD_CODI AND VNST_MOIN.TIMO_ABRV = 'INI') "
        End If

        SQL += " ORDER BY TNST_INVD.INVG_CODI, TNST_INVD.ALMA_CODI, TNST_GRUP.GRUP_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            If PrimerRegistro = True Then
                PrimerRegistro = False
                N_Inventario = CType(Me.DbLeeHotel.mDbLector("INVENTARIO"), Integer)
                Almacen = CType(Me.DbLeeHotel.mDbLector("ALMACEN"), Integer)
                Grupo = CType(Me.DbLeeHotel.mDbLector("GRUPO"), Integer)
            End If

            '-------------------------------------------------------
            ' No ha cambiado 
            '.------------------------------------------------------
            If CType(Me.DbLeeHotel.mDbLector("INVENTARIO"), Integer) = N_Inventario And
               CType(Me.DbLeeHotel.mDbLector("ALMACEN"), Integer) = Almacen And
               CType(Me.DbLeeHotel.mDbLector("GRUPO"), Integer) = Grupo Then

                If CType(Me.DbLeeHotel.mDbLector("DOSIS"), Double) = 0 Then
                    Existencia = CType(Me.DbLeeHotel.mDbLector("REAL"), Double)
                Else
                    Existencia = CType(Me.DbLeeHotel.mDbLector("REAL"), Double) + (CType(Me.DbLeeHotel.mDbLector("DOSISREAL"), Double) / CType(Me.DbLeeHotel.mDbLector("DOSIS"), Double))
                End If
                Total = Total + Decimal.Round(CType((Existencia * CType(Me.DbLeeHotel.mDbLector("PRECIO"), Double)), Decimal), 2)

            Else
                '-------------------------------------------------------
                ' SI ha cambiado 
                '.------------------------------------------------------
                If Total > 0 Then
                    Me.GrabaInventarioFinal(Total, Almacen, Grupo, FechaInventarioFinal)
                End If

                Total = 0
                N_Inventario = CType(Me.DbLeeHotel.mDbLector("INVENTARIO"), Integer)
                Almacen = CType(Me.DbLeeHotel.mDbLector("ALMACEN"), Integer)
                Grupo = CType(Me.DbLeeHotel.mDbLector("GRUPO"), Integer)

                If CType(Me.DbLeeHotel.mDbLector("DOSIS"), Double) = 0 Then
                    Existencia = CType(Me.DbLeeHotel.mDbLector("REAL"), Double)
                Else
                    Existencia = CType(Me.DbLeeHotel.mDbLector("REAL"), Double) + (CType(Me.DbLeeHotel.mDbLector("DOSISREAL"), Double) / CType(Me.DbLeeHotel.mDbLector("DOSIS"), Double))
                End If
                Total = Total + Decimal.Round(CType((Existencia * CType(Me.DbLeeHotel.mDbLector("PRECIO"), Double)), Decimal), 2)

            End If




        End While

        If Total > 0 Then
            Me.GrabaInventarioFinal(Total, Almacen, Grupo, FechaInventarioFinal)
        End If
        Me.DbLeeHotel.mDbLector.Close()


    End Sub
    Private Sub GrabaInventarioFinal(ByVal vTotal As Double, ByVal vAlmacen As Integer, ByVal vGrupo As Integer, ByVal vFecha As Date)

        Dim UltimoAlmacen As String = Me.DbLeeHotelAux.EjecutaSqlScalar("SELECT ALMA_DESC FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacen)
        Dim UltimoGrupo As String = Me.DbLeeHotelAux.EjecutaSqlScalar("SELECT GRUP_DESC FROM TNST_GRUP WHERE GRUP_CODI = " & vGrupo)

        Dim vCentroCosto As String
        SQL = "SELECT NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacen
        vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)



        Linea = Linea + 1
        Me.mTipoAsiento = "DEBE"
        Me.InsertaOracleInventarios("AC", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), 1, Linea, Me.BuscaCuentaExistencia(vAlmacen, vGrupo), Me.mIndicadorDebe, UltimoAlmacen & " " & UltimoGrupo, vTotal, "NO", "", "", vFecha)
        'Me.GeneraFileACInventario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), Me.BuscaCuentaExistencia(vAlmacen, vGrupo), Me.mIndicadorDebe, CType("INVENTARIO FINAL ", String), vTotal, vFecha)


        Linea = Linea + 1
        Me.mTipoAsiento = "HABER"
        Me.InsertaOracleInventarios("AC", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), 1, Linea, Me.BuscaCuentaExistenciaSantanaCazorla(vAlmacen, vGrupo), Me.mIndicadorHaber, UltimoAlmacen & " " & UltimoGrupo, vTotal, "NO", "", "", vFecha)
        'Me.GeneraFileACInventario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), Me.BuscaCuentaExistenciaSantanaCazorla(vAlmacen, vGrupo), Me.mIndicadorHaber, CType("INVENTARIO FINAL ", String), vTotal, vFecha)
        'Me.GeneraFileAAInventario("AA", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), 1, Me.BuscaCuentaExistenciaSantanaCazorla(vAlmacen, vGrupo), "0", vCentroCosto, vTotal, vFecha)



    End Sub
#End Region
#Region "ASIENTO-8 INVENTARIO INICIAL"
    Private Sub InventariosInicial()
        Dim Total As Double

        Dim PrimerRegistro As Boolean = True
        Dim N_Inventario As Integer
        Dim Almacen As Integer
        Dim Grupo As Integer
        Dim Filename As String

        Dim FechaInventarioInicialStr As String

        If Me.mSoloInventariosIniciales = False Then
            FechaInventarioInicialStr = DateTime.DaysInMonth(Me.mFecha.Year, Me.mFecha.Month) & "/" & Me.mFecha.Month & "/" & Me.mFecha.Year
            FechaInventarioInicial = DateAdd(DateInterval.Day, 1, CType(FechaInventarioInicialStr, Date))
        Else
            FechaInventarioInicialStr = InputBox("Fecha de Inventario Inicial ", "Use Formato dd/mm/yyyy")
            FechaInventarioInicial = CType(FechaInventarioInicialStr, Date)
        End If



        Filename = "INVENTARIO INICIAL " & FechaInventarioInicial.Day & "-" & FechaInventarioInicial.Month & "-" & FechaInventarioInicial.Year & ".txt"

        Me.BorraRegistrosInventario(FechaInventarioInicial)
        Me.CrearFichero(Me.mParaFilePath & Filename)

        Dim Existencia As Double

        SQL = "SELECT TNST_INVG.INVG_CODI as INVENTARIO,to_char(TNST_INVG.INVG_DAVA,'MM'),TNST_INVD.ALMA_CODI AS ALMACEN,ALMA_DESC AS ALMANOME,"
        SQL += "TNST_INVD.INVD_EXRE AS REAL,TNST_INVD.INVD_DORE AS DOSISREAL,"
        SQL += "TNST_INVD.INVD_PMCB AS PRECIO, TNST_PROD.GRUP_CODI AS GRUPO, TNST_PROD.PROD_DOSE AS DOSIS,GRUP_DESC AS GRUPNOME "
        SQL += "FROM TNST_INVG,TNST_INVD,TNST_PROD,TNST_UNME,TNST_GRUP,TNST_ALMA "
        SQL += "WHERE "
        SQL += "TNST_INVG.INVG_CODI = TNST_INVD.INVG_CODI AND "
        SQL += "TNST_INVG.INVG_ANCI = TNST_INVD.INVG_ANCI AND "
        SQL += "TNST_INVG.ALMA_CODI = TNST_ALMA.ALMA_CODI AND "
        SQL += "TNST_INVD.PROD_CODI = TNST_PROD.PROD_CODI AND "
        SQL += "TNST_PROD.UNME_CODI = TNST_UNME.UNME_CODI AND "
        SQL += "TNST_PROD.GRUP_CODI = TNST_GRUP.GRUP_CODI AND "
        SQL += " TO_CHAR(TNST_INVG.INVG_DAVA,'MM') = '" & Format(Me.mFecha, "MM") & "'"
        SQL += " AND TO_CHAR(TNST_INVG.INVG_DAVA,'YYYY') = '" & Format(Me.mFecha, "yyyy") & "'"

        If Me.mSoloInventariosIniciales = True Then
            SQL += "AND EXISTS(SELECT 'X' FROM VNST_MOIN "
            SQL += "WHERE TNST_INVD.INVG_CODI = VNST_MOIN.INVG_CODI AND "
            SQL += "TNST_INVD.INVG_ANCI = VNST_MOIN.INVG_ANCI AND "
            SQL += " TNST_INVD.PROD_CODI = VNST_MOIN.PROD_CODI AND VNST_MOIN.TIMO_ABRV = 'INI') "
        Else
            SQL += "AND NOT EXISTS(SELECT 'X' FROM VNST_MOIN "
            SQL += "WHERE TNST_INVD.INVG_CODI = VNST_MOIN.INVG_CODI AND "
            SQL += "TNST_INVD.INVG_ANCI = VNST_MOIN.INVG_ANCI AND "
            SQL += " TNST_INVD.PROD_CODI = VNST_MOIN.PROD_CODI AND VNST_MOIN.TIMO_ABRV = 'INI') "
        End If

        SQL += " ORDER BY TNST_INVD.INVG_CODI, TNST_INVD.ALMA_CODI, TNST_GRUP.GRUP_CODI"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            If PrimerRegistro = True Then
                PrimerRegistro = False
                N_Inventario = CType(Me.DbLeeHotel.mDbLector("INVENTARIO"), Integer)
                Almacen = CType(Me.DbLeeHotel.mDbLector("ALMACEN"), Integer)
                Grupo = CType(Me.DbLeeHotel.mDbLector("GRUPO"), Integer)
            End If

            '-------------------------------------------------------
            ' No ha cambiado 
            '.------------------------------------------------------
            If CType(Me.DbLeeHotel.mDbLector("INVENTARIO"), Integer) = N_Inventario And
               CType(Me.DbLeeHotel.mDbLector("ALMACEN"), Integer) = Almacen And
               CType(Me.DbLeeHotel.mDbLector("GRUPO"), Integer) = Grupo Then

                If CType(Me.DbLeeHotel.mDbLector("DOSIS"), Double) = 0 Then
                    Existencia = CType(Me.DbLeeHotel.mDbLector("REAL"), Double)
                Else
                    Existencia = CType(Me.DbLeeHotel.mDbLector("REAL"), Double) + (CType(Me.DbLeeHotel.mDbLector("DOSISREAL"), Double) / CType(Me.DbLeeHotel.mDbLector("DOSIS"), Double))
                End If
                Total = Total + Decimal.Round(CType((Existencia * CType(Me.DbLeeHotel.mDbLector("PRECIO"), Double)), Decimal), 2)

            Else
                '-------------------------------------------------------
                ' SI ha cambiado 
                '.------------------------------------------------------
                If Total > 0 Then
                    Me.GrabaInventarioInicial(Total, Almacen, Grupo, FechaInventarioInicial)
                End If

                Total = 0
                N_Inventario = CType(Me.DbLeeHotel.mDbLector("INVENTARIO"), Integer)
                Almacen = CType(Me.DbLeeHotel.mDbLector("ALMACEN"), Integer)
                Grupo = CType(Me.DbLeeHotel.mDbLector("GRUPO"), Integer)

                If CType(Me.DbLeeHotel.mDbLector("DOSIS"), Double) = 0 Then
                    Existencia = CType(Me.DbLeeHotel.mDbLector("REAL"), Double)
                Else
                    Existencia = CType(Me.DbLeeHotel.mDbLector("REAL"), Double) + (CType(Me.DbLeeHotel.mDbLector("DOSISREAL"), Double) / CType(Me.DbLeeHotel.mDbLector("DOSIS"), Double))
                End If
                Total = Total + Decimal.Round(CType((Existencia * CType(Me.DbLeeHotel.mDbLector("PRECIO"), Double)), Decimal), 2)

            End If




        End While

        If Total > 0 Then
            Me.GrabaInventarioInicial(Total, Almacen, Grupo, FechaInventarioInicial)
        End If
        Me.DbLeeHotel.mDbLector.Close()


    End Sub
    Private Sub GrabaInventarioInicial(ByVal vTotal As Double, ByVal vAlmacen As Integer, ByVal vGrupo As Integer, ByVal vFecha As Date)

        Dim UltimoAlmacen As String = Me.DbLeeHotelAux.EjecutaSqlScalar("SELECT ALMA_DESC FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacen)
        Dim UltimoGrupo As String = Me.DbLeeHotelAux.EjecutaSqlScalar("SELECT GRUP_DESC FROM TNST_GRUP WHERE GRUP_CODI = " & vGrupo)



        Dim vCentroCosto As String
        SQL = "SELECT NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacen
        vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

        Linea = Linea + 1
        Me.mTipoAsiento = "HABER"
        Me.InsertaOracleInventarios("AC", 8, Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), 1, Linea, Me.BuscaCuentaExistencia(vAlmacen, vGrupo), Me.mIndicadorHaber, UltimoAlmacen & " " & UltimoGrupo, vTotal, "NO", "", "", vFecha)
        'Me.GeneraFileACInventario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), Me.BuscaCuentaExistencia(vAlmacen, vGrupo), Me.mIndicadorHaber, CType("INVENTARIO INICIAL ", String), vTotal, vFecha)


        Linea = Linea + 1
        Me.mTipoAsiento = "DEBE"
        Me.InsertaOracleInventarios("AC", 8, Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), 1, Linea, Me.BuscaCuentaExistenciaSantanaCazorla(vAlmacen, vGrupo), Me.mIndicadorDebe, UltimoAlmacen & " " & UltimoGrupo, vTotal, "NO", "", "", vFecha)
        'Me.GeneraFileACInventario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), Me.BuscaCuentaExistenciaSantanaCazorla(vAlmacen, vGrupo), Me.mIndicadorDebe, CType("INVENTARIO INICIAL ", String), vTotal, vFecha)
        'Me.GeneraFileAAInventario("AA", 8, Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), 1, Me.BuscaCuentaExistenciaSantanaCazorla(vAlmacen, vGrupo), "0", vCentroCosto, vTotal, vFecha)

    End Sub
#End Region
#Region "ASIENTO-20 SALIDAS A GASTO"
    Private Sub SalidasGastoSalidas()
        Dim Total As Double
        Dim vCentroCosto As String

        SQL = "SELECT   TNST_MOVG.MOVG_CODI,TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, NVL(TNST_MOVG.MOVG_IDDO,' ')AS DOCU,"
        SQL += "TNST_FAMI.FAMI_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,FAMI_DESC AS FAMILIA,TNST_MOVD.ALMA_CODI AS ALMACODI,TNST_FAMI.FAMI_CODI AS FAMICODI"
        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_FAMI, TNST_TIMO "
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " AND (TNST_PROD.FAMI_CODI = TNST_FAMI.FAMI_CODI)"
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Salida_Gastos
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " AND TNST_MOVD.MOVD_ENSA = " & -1
        SQL += " GROUP BY  TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,"
        SQL += "TNST_FAMI.FAMI_CODI,"
        SQL += "TNST_MOVG.MOVG_IDDO,"
        SQL += "TNST_TIMO.TIMO_TIPO,"
        SQL += "TNST_ALMA.ALMA_DESC,"
        SQL += "TNST_FAMI.FAMI_DESC"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            SQL = "SELECT NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotel.mDbLector("FAMICODI"), Integer)), Me.mIndicadorHaber, CType("SALIDA A GASTOS ", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("FAMILIA"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "", CInt(Me.DbLeeHotel.mDbLector("ALMACODI")), "", "", 0, "", 0, "", "", 0)


        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub SalidasGastoEntradas()

        Try
            Dim Total As Double
            Dim vCentroCosto As String

            SQL = "SELECT    TNST_MOVG.MOVG_CODI,TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, NVL(TNST_MOVG.MOVG_IDDO,' ')AS DOCU,"
            SQL += " TNST_FAMI.FAMI_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,"
            SQL += " FAMI_DESC AS FAMILIA,TNST_MOVG.MOVG_DEST AS ALMACODI,TNST_FAMI.FAMI_CODI AS FAMICODI"
            SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_FAMI, TNST_TIMO "
            SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
            SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
            SQL += " AND (TNST_MOVG.MOVG_DEST = TNST_ALMA.ALMA_CODI)"
            SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
            SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
            SQL += " AND (TNST_PROD.FAMI_CODI = TNST_FAMI.FAMI_CODI)"
            SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Salida_Gastos
            SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
            SQL += " AND TNST_MOVG.MOVG_ANUL = 0"

            SQL += " GROUP BY  TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_DAVA,"
            SQL += "TNST_MOVG.MOVG_DEST,"
            SQL += "TNST_FAMI.FAMI_CODI,"
            SQL += "TNST_MOVG.MOVG_IDDO,"
            SQL += "TNST_TIMO.TIMO_TIPO,"
            SQL += "TNST_ALMA.ALMA_DESC,"
            SQL += "TNST_FAMI.FAMI_DESC"


            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                SQL = "SELECT NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
                vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotel.mDbLector("FAMICODI"), Integer)), Me.mIndicadorDebe, CType("RECIBE SAL. GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("FAMILIA"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "", CInt(Me.DbLeeHotel.mDbLector("ALMACODI")), "", "", 0, "", 0, "", "", 0)


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Traspasos Entradas")
        End Try

    End Sub
#End Region
#Region "ASIENTO-30 ROTURAS"
    Private Sub Roturas()
        Dim Total As Double
        Dim vCentroCosto As String

        SQL = "SELECT   TNST_MOVG.MOVG_CODI,TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, NVL(TNST_MOVG.MOVG_IDDO,' ')AS DOCU,"
        SQL += "TNST_MOVD.ALMA_CODI,TNST_FAMI.FAMI_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,FAMI_DESC AS FAMILIA,TNST_MOVD.MOVD_ENSA,TNST_MOVD.ALMA_CODI AS ALMACODI,TNST_FAMI.FAMI_CODI AS FAMICODI,TNST_MOVG.MOVG_ORIG AS MOVG_ORIG"
        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_FAMI, TNST_TIMO "
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " AND (TNST_PROD.FAMI_CODI = TNST_FAMI.FAMI_CODI)"
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Roturas
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " AND TNST_MOVD.MOVD_ENSA = " & -1
        SQL += " GROUP BY TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,"
        SQL += "TNST_FAMI.FAMI_CODI,"
        SQL += "TNST_MOVG.MOVG_IDDO,"
        SQL += "TNST_TIMO.TIMO_TIPO,"
        SQL += "TNST_ALMA.ALMA_DESC,"
        SQL += "TNST_FAMI.FAMI_DESC,"
        SQL += "TNST_MOVD.MOVD_ENSA,TNST_MOVG.MOVG_ORIG "

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            SQL = "SELECT NVL(SUBSTR(ALMA_CCST,1,2),'AA') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("MOVG_ORIG"), Integer)
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 30, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.ComponeCuentaGastoGrupo6(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer), CType(Me.DbLeeHotel.mDbLector("FAMICODI"), Integer)), Me.mIndicadorHaber, CType("SALIDA ROTURAS ", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("FAMILIA"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "", CInt(Me.DbLeeHotel.mDbLector("MOVG_ORIG")), "", "", 0, "", 0, "", "", 0)

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 30, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaRoturas, Me.mIndicadorDebe, CType("ENTRADA ROTURAS ", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("FAMILIA"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "", CInt(Me.DbLeeHotel.mDbLector("MOVG_ORIG")), "", "", 0, "", 0, "", "", 0)

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
#End Region

#Region "Nuega Gestion Tito"
    Private Sub GestionaCabecerasDeMovimiento()
        Dim Total As Double
        Dim Texto As String = ""
        Dim Proveedor As String = ""
        Dim Cuenta As String = ""
        Dim NumeroAsiento As Integer
        Dim IndicadorDebeHaber As String
        Dim AlmacenExt As String = ""
        Dim AlmaCodi As Integer


        Try




            SQL = "SELECT "
            SQL += "    TIMO_TIPO, "
            SQL += "    TIMO_DESC, "
            SQL += "    MOVG_DADO, "
            SQL += "    TNST_MOVG.MOVG_CODI   AS NMOVI, "
            SQL += "    TNST_MOVG.MOVG_ANCI, "
            SQL += "    MOVG_ORIG, "
            SQL += "    MOVG_DEST, "
            SQL += "    TNST_MOVG.MOVG_VATO   TOTAL, "
            SQL += "    NVL(TNST_MOVG.MOVG_IDDO, ' ') AS DOCU, "
            SQL += "    TNST_MOVG.MOVG_DEST   AS MOVG_DEST, "
            SQL += "    TNST_MOVG.MOVG_CODI "
            SQL += "    || '/' "
            SQL += "    || TNST_MOVG.MOVG_ANCI AS DOCNEWSTOCK, "
            SQL += "    NVL(MOVG_DEST, 0) AS MOVG_DEST "
            SQL += "FROM "
            SQL += "    TNST_MOVG, "
            SQL += "    TNST_TIMO "
            SQL += "WHERE "
            SQL += "    TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
            SQL += "    AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
            SQL += "    AND TNST_MOVG.MOVG_ANUL = 0 "
            SQL += "ORDER BY "
            SQL += "    TIMO_TIPO, "
            SQL += "    TNST_MOVG.MOVG_IDDO "


            '6X'

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read


                ' Es un Albaran o Factura

                If CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = Me.mTimo_Albaran Or CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = Me.mTimo_Factura_Directa Or CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = Me.mTimo_Factura_Al Then
                    SQL = "SELECT NVL(FORN_CEX1,'0') AS FORN_CEX1 FROM TNST_FORN WHERE FORN_CODI = " & CStr(Me.DbLeeHotel.mDbLector("MOVG_ORIG"))
                    Proveedor = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

                    Cuenta = Me.BuscaCuentaProveedor(CStr(Me.DbLeeHotel.mDbLector("MOVG_ORIG")))

                    SQL = "SELECT NVL(FORN_DESC,'?') AS FORN_CEX1 FROM TNST_FORN WHERE FORN_CODI = " & CStr(Me.DbLeeHotel.mDbLector("MOVG_ORIG"))
                    Texto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

                    SQL = "SELECT NVL(ALMA_COEX,'0') AS ALMA_COEX FROM TNST_ALMA WHERE ALMA_CODI = " & CStr(Me.DbLeeHotel.mDbLector("MOVG_DEST"))
                    AlmacenExt = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    AlmaCodi = CInt(Me.DbLeeHotel.mDbLector("MOVG_DEST"))

                    Me.mTipoAsiento = "HABER"
                    IndicadorDebeHaber = Me.mIndicadorHaber

                    If CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = Me.mTimo_Albaran Then
                        NumeroAsiento = 1
                    ElseIf CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = Me.mTimo_Factura_Directa Then
                        NumeroAsiento = 3
                    ElseIf CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = Me.mTimo_Factura_Al Then
                        NumeroAsiento = 6
                    End If

                    ' Es un Albaran o Factura Devuelta 
                ElseIf CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = Me.mTimo_Albaran_Dev Or CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = Me.mTimo_Factura_Directa_Dev Then
                    SQL = "SELECT NVL(FORN_CEX1,'0') AS FORN_CEX1 FROM TNST_FORN WHERE FORN_CODI = " & CStr(Me.DbLeeHotel.mDbLector("MOVG_DEST"))
                    Proveedor = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

                    Cuenta = Me.BuscaCuentaProveedor(CStr(Me.DbLeeHotel.mDbLector("MOVG_DEST")))

                    SQL = "SELECT NVL(FORN_DESC,'?') AS FORN_CEX1 FROM TNST_FORN WHERE FORN_CODI = " & CStr(Me.DbLeeHotel.mDbLector("MOVG_DEST"))
                    Texto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

                    SQL = "SELECT NVL(ALMA_COEX,'0') AS ALMA_COEX FROM TNST_ALMA WHERE ALMA_CODI = " & CStr(Me.DbLeeHotel.mDbLector("MOVG_ORIG"))
                    AlmacenExt = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                    AlmaCodi = CInt(Me.DbLeeHotel.mDbLector("MOVG_ORIG"))

                    Me.mTipoAsiento = "DEBE"
                    IndicadorDebeHaber = Me.mIndicadorDebe

                    If CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = Me.mTimo_Albaran_Dev Then
                        NumeroAsiento = 4
                    ElseIf CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = Me.mTimo_Factura_Directa_Dev Then
                        NumeroAsiento = 5
                    End If
                Else
                    ' Es un Movimiento Interno (Traspasos ...) 

                    If CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = Me.mTimo_Traspaso Then
                        NumeroAsiento = 2
                    ElseIf CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = Me.mTimo_Roturas Then
                        NumeroAsiento = 30
                    End If
                End If



                ' Si no es un Movimiento Interno (Traspasos, Roturas , Inventarios )  se graba la Cabecera 
                If CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) <> Me.mTimo_Traspaso And CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) <> Me.mTimo_Roturas Then
                    Linea = Linea + 1
                    Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                    Me.InsertaOracle("AC", NumeroAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, IndicadorDebeHaber, Texto, Total, "NO", CStr(Me.DbLeeHotel.mDbLector("DOCU")), "", AlmaCodi, "", AlmacenExt, 0, "", 0, CStr(Me.DbLeeHotel.mDbLector("DOCNEWSTOCK")), "", 9)

                    ' sale a hacer los aspuntes de GASTOS 
                    If CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) <> Me.mTimo_Factura_Al Then
                        Me.GestionaDetalledeMovimiento(CType(Me.DbLeeHotel.mDbLector("NMOVI"), Integer), CType(Me.DbLeeHotel.mDbLector("MOVG_ANCI"), Integer), Texto, CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")), NumeroAsiento, Me.mTipoAsiento, "")

                    Else
                        ' Sale a Buscar los Albaranes de la FActura
                        Me.GestionaAlbaranedeFactura(CType(Me.DbLeeHotel.mDbLector("NMOVI"), Integer), CType(Me.DbLeeHotel.mDbLector("MOVG_ANCI"), Integer))

                    End If

                    ' sale a hacer los aspuntes de IGIC si es una factura
                    If CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = Me.mTimo_Factura_Directa Or CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = Me.mTimo_Factura_Directa_Dev Or CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = Me.mTimo_Factura_Al Then
                        Me.GestionaFacturasImpuesto(CType(Me.DbLeeHotel.mDbLector("NMOVI"), Integer), CType(Me.DbLeeHotel.mDbLector("MOVG_ANCI"), Integer), NumeroAsiento, CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")))
                    End If

                Else
                    ' sale a hacer los aspuntes de GASTOS Si es un Traspaso o Rotura
                    Me.GestionaDetalledeMovimiento(CType(Me.DbLeeHotel.mDbLector("NMOVI"), Integer), CType(Me.DbLeeHotel.mDbLector("MOVG_ANCI"), Integer), Texto, CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")), NumeroAsiento, Me.mTipoAsiento, "-1")
                    Me.GestionaDetalledeMovimiento(CType(Me.DbLeeHotel.mDbLector("NMOVI"), Integer), CType(Me.DbLeeHotel.mDbLector("MOVG_ANCI"), Integer), Texto, CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")), NumeroAsiento, Me.mTipoAsiento, "1")

                End If



            End While
            Me.DbLeeHotel.mDbLector.Close()

        Catch ex As Exception
            Me.DbLeeHotel.mDbLector.Close()
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturas Directas")
        End Try


    End Sub
    Private Sub GestionaDetalledeMovimiento(ByVal vCodi As Integer, ByVal vAnci As Integer, ByVal vTexto As String, vTimoTipo As Integer, vNumAsiento As Integer, vTipoPunte As String, vEnsa As String)
        Dim Total As Double
        Dim TotalPuntoVerde As Double
        Dim IndicadorDebeHaber As String
        Dim Texto As String
        Try

            SQL = "SELECT TNST_TIMO.TIMO_TIPO,TNST_TIMO.TIMO_CODI "

            SQL += ",SUM(TNST_MOVD.MOVD_TOTA) AS TOTAL, SUM(NVL(TNST_MOVD.MOVD_VERD,0)) AS PVERDE"
            SQL += ",TNST_MOVD.ALMA_CODI  "
            SQL += ",TNST_PROD.PROD_CODI ,NVL(PROD_DESC,'?') AS PROD_DESC "

            SQL += " ,NVL(TNST_MOVG.MOVG_IDDO, ' ') AS DOCU "

            SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD,  TNST_TIMO"


            SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
            SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
            SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
            SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
            SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
            SQL += " AND TNST_TIMO.TIMO_TIPO = " & vTimoTipo
            SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
            SQL += " AND TNST_MOVG.MOVG_ANUL = 0"


            SQL += " AND TNST_MOVG.MOVG_CODI =  " & vCodi
            SQL += " AND TNST_MOVG.MOVG_ANCI =  " & vAnci

            If vEnsa.Length > 0 Then
                SQL += " AND TNST_MOVD.MOVD_ENSA = " & CInt(vEnsa)
            End If


            SQL += " GROUP BY "
            SQL += " TNST_TIMO.TIMO_TIPO,TNST_TIMO.TIMO_CODI,"
            SQL += " TNST_MOVG.MOVG_IDDO,"
            SQL += " TNST_MOVD.ALMA_CODI,"

            SQL += "TNST_PROD.PROD_CODI,PROD_DESC"



            Me.DbLeeHotelAux2.TraerLector(SQL)

            While Me.DbLeeHotelAux2.mDbLector.Read

                Total = CType(Me.DbLeeHotelAux2.mDbLector("TOTAL"), Double)

                If CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) <> Me.mTimo_Traspaso And CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) <> Me.mTimo_Roturas Then

                    Linea = Linea + 1
                    ' Hace la Contrapartida del Apunte llamador 
                    If vTipoPunte = "DEBE" Then
                        Me.mTipoAsiento = "HABER"
                        IndicadorDebeHaber = Me.mIndicadorHaber
                    Else
                        Me.mTipoAsiento = "DEBE"
                        IndicadorDebeHaber = Me.mIndicadorDebe
                    End If
                    Me.InsertaOracle("AC", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, BuscaCuentaGastosProducto(Me.DbLeeHotelAux2.mDbLector("PROD_CODI")), IndicadorDebeHaber, " -- " & CType(Me.DbLeeHotelAux2.mDbLector("PROD_DESC"), String), Total, "NO", CStr(Me.DbLeeHotelAux2.mDbLector("DOCU")), "", CType(Me.DbLeeHotelAux2.mDbLector("ALMA_CODI"), Integer), "", "", 0, "", 0, "", "", 0)

                    TotalPuntoVerde = CDbl(Me.DbLeeHotelAux2.mDbLector("PVERDE"))
                    If TotalPuntoVerde <> 0 Then
                        Linea = Linea + 1
                        Texto = "Punto Verde"
                        Me.InsertaOracle("AC", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, mCtaPuntoVerde, IndicadorDebeHaber, Texto, TotalPuntoVerde, "NO", "0", "", CType(Me.DbLeeHotelAux2.mDbLector("ALMA_CODI"), Integer), "", "", 0, "", 0, "", "", 0)
                    End If
                Else

                    If CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = Me.mTimo_Traspaso Then
                        If vEnsa = "-1" Then
                            Linea = Linea + 1
                            Me.mTipoAsiento = "HABER"
                            IndicadorDebeHaber = Me.mIndicadorHaber
                            Me.InsertaOracle("AC", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, BuscaCuentaGastosProducto(Me.DbLeeHotelAux2.mDbLector("PROD_CODI")), IndicadorDebeHaber, " -- " & CType(Me.DbLeeHotelAux2.mDbLector("PROD_DESC"), String), Total, "NO", CStr(Me.DbLeeHotelAux2.mDbLector("DOCU")), "", CType(Me.DbLeeHotelAux2.mDbLector("ALMA_CODI"), Integer), "", "", 0, "", 0, "", "", 0)
                        Else
                            Linea = Linea + 1
                            Me.mTipoAsiento = "DEBE"
                            IndicadorDebeHaber = Me.mIndicadorDebe
                            Me.InsertaOracle("AC", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, BuscaCuentaGastosProducto(Me.DbLeeHotelAux2.mDbLector("PROD_CODI")), IndicadorDebeHaber, " -- " & CType(Me.DbLeeHotelAux2.mDbLector("PROD_DESC"), String), Total, "NO", CStr(Me.DbLeeHotelAux2.mDbLector("DOCU")), "", CType(Me.DbLeeHotelAux2.mDbLector("ALMA_CODI"), Integer), "", "", 0, "", 0, "", "", 0)
                        End If
                    End If
                    If CInt(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = Me.mTimo_Roturas Then
                        If vEnsa = "-1" Then
                            Linea = Linea + 1
                            Me.mTipoAsiento = "HABER"
                            IndicadorDebeHaber = Me.mIndicadorHaber
                            Me.InsertaOracle("AC", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, BuscaCuentaTipoMovimiento(CInt(Me.DbLeeHotelAux2.mDbLector("TIMO_CODI"))), IndicadorDebeHaber, " -- " & CType(Me.DbLeeHotelAux2.mDbLector("PROD_DESC"), String), Total, "NO", CStr(Me.DbLeeHotelAux2.mDbLector("DOCU")), "", CType(Me.DbLeeHotelAux2.mDbLector("ALMA_CODI"), Integer), "", "", 0, "", 0, "", "", 0)
                        Else
                            Linea = Linea + 1
                            Me.mTipoAsiento = "DEBE"
                            IndicadorDebeHaber = Me.mIndicadorDebe
                            Me.InsertaOracle("AC", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, BuscaCuentaGastosProducto(Me.DbLeeHotelAux2.mDbLector("PROD_CODI")), IndicadorDebeHaber, " -- " & CType(Me.DbLeeHotelAux2.mDbLector("PROD_DESC"), String), Total, "NO", CStr(Me.DbLeeHotelAux2.mDbLector("DOCU")), "", CType(Me.DbLeeHotelAux2.mDbLector("ALMA_CODI"), Integer), "", "", 0, "", 0, "", "", 0)
                        End If
                    End If





                End If



            End While
            Me.DbLeeHotelAux2.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub GestionaFacturasImpuesto(ByVal vCodi As Integer, ByVal vAnci As Integer, vNumAsiento As Integer, vTimoTipo As Integer)
        Dim Total As Double
        Dim TotalBase As Double
        Dim ValorImpuesto As Double
        Dim AlmaCodi As Integer

        Try



            SQL = "SELECT TNST_MOVG.MOVG_CODI AS NMOVI,MOVG_ORIG,SUM(TNST_MOVI.MOVI_IMPU)AS TOTAL ,SUM(TNST_MOVI.MOVI_NETO)AS BASE,NVL(MOVG_IDDO,' ')AS DOCU ,MOVI_TAXA AS TIPO,"
            SQL += "TNST_MOVG.MOVG_DEST ,TNST_MOVG.MOVG_ORIG "
            SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_MOVI "
            SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
            SQL += " And TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
            SQL += " AND TNST_MOVG.MOVG_ANUL = 0"

            SQL += " AND TNST_MOVG.MOVG_CODI =  " & vCodi
            SQL += " AND TNST_MOVG.MOVG_ANCI =  " & vAnci

            SQL += " AND TNST_MOVG.MOVG_CODI = TNST_MOVI.MOVG_CODI "
            SQL += " AND TNST_MOVG.MOVG_ANCI = TNST_MOVI.MOVG_ANCI "

            SQL += " GROUP BY TNST_MOVG.MOVG_CODI,MOVG_ORIG,MOVG_IDDO,MOVI_TAXA,TNST_MOVG.MOVG_DEST,TNST_MOVG.MOVG_ORIG"

            Me.DbLeeHotelAux3.TraerLector(SQL)

            While Me.DbLeeHotelAux3.mDbLector.Read
                SQL = "SELECT IVAS_TAXA FROM TNST_IVAS WHERE IVAS_CODI = " & CInt(Me.DbLeeHotelAux3.mDbLector("TIPO"))
                ValorImpuesto = CType(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL), Double)

                If vTimoTipo = Me.mTimo_Factura_Directa Or vTimoTipo = Me.mTimo_Factura_Al Then
                    AlmaCodi = CInt(Me.DbLeeHotelAux3.mDbLector("MOVG_DEST"))
                Else
                    AlmaCodi = CInt(Me.DbLeeHotelAux3.mDbLector("MOVG_ORIG"))
                End If


                Linea = Linea + 1
                Total = CType(Me.DbLeeHotelAux3.mDbLector("TOTAL"), Double)
                TotalBase = CType(Me.DbLeeHotelAux3.mDbLector("BASE"), Double)
                Me.mTipoAsiento = "DEBE"
                If TotalBase <> 0 Then
                    Me.InsertaOracle("AC", vNumAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaImpuestoVenta(CInt(Me.DbLeeHotelAux3.mDbLector("TIPO"))), Me.mIndicadorDebe, CType("IMPUESTO FACTURA ", String) & ValorImpuesto & "%", Total, "NO", CType(Me.DbLeeHotelAux3.mDbLector("DOCU"), String), "", AlmaCodi, "", "", 0, "", 0, "", "", 0)
                End If

            End While
            Me.DbLeeHotelAux3.mDbLector.Close()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub GestionaAlbaranedeFactura(ByVal vCodi As Integer, ByVal vAnci As Integer)
        Dim Total As Double

        SQL = "SELECT TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_ANCI,QWE_VNST_GUIA.TIPO ,SUM (QWE_VNST_GUIA.MOVG_VATO)AS TOTAL, QWE_VNST_GUIA.MOVG_IDDO AS ALBARAN, TNST_MOVG.MOVG_ORIG,TNST_FORN.FORN_CODI AS CODI,"
        SQL += " TNST_FORN.FORN_DESC AS PROVEEDOR , QWE_VNST_GUIA.MOVG_CODI AS C1 , QWE_VNST_GUIA.MOVG_ANCI AS C2,TNST_MOVG.MOVG_ORIG as MOVG_ORIG,TNST_MOVG.MOVG_DEST as MOVG_DEST "
        SQL += " FROM TNST_MOVG, QWE_VNST_GUIA, TNST_TIMO, TNST_FORN "
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = QWE_VNST_GUIA.DORE_CODI) "
        SQL += " AND (TNST_MOVG.MOVG_ANCI = QWE_VNST_GUIA.DORE_ANCI) "
        SQL += " AND QWE_VNST_GUIA.DORE_CODI = " & vCodi
        SQL += " AND QWE_VNST_GUIA.DORE_ANCI = " & vAnci

        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI) "
        SQL += " AND (TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI OR TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI) "

        SQL += " AND (QWE_VNST_GUIA.TIPO = " & Me.mTimo_Albaran
        SQL += " OR QWE_VNST_GUIA.TIPO= " & Me.mTimo_Albaran_Dev
        SQL += ") AND TNST_MOVG.MOVG_ANUL = 0 "


        SQL += "GROUP BY  TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_ANCI,QWE_VNST_GUIA.TIPO,QWE_VNST_GUIA.MOVG_IDDO, TNST_MOVG.MOVG_ORIG, TNST_FORN.FORN_CODI,TNST_FORN.FORN_DESC, QWE_VNST_GUIA.MOVG_CODI, QWE_VNST_GUIA.MOVG_ANCI,TNST_MOVG.MOVG_ORIG,TNST_MOVG.MOVG_DEST"

        '2X'

        Me.DbLeeHotelAux2.TraerLector(SQL)

        While Me.DbLeeHotelAux2.mDbLector.Read

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotelAux2.mDbLector("TOTAL"), Double)

            If CType(Me.DbLeeHotelAux2.mDbLector("TIPO"), Integer) = Me.mTimo_Albaran Then
                Linea = Linea + 1
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedorAlbaranes(CInt(Me.DbLeeHotelAux2.mDbLector("CODI"))), Me.mIndicadorDebe, CType(" --FORMALIZADO ", String) & " " & CType(Me.DbLeeHotelAux2.mDbLector("ALBARAN"), String), Total, "NO", CType(Me.DbLeeHotelAux2.mDbLector("ALBARAN"), String), "", CInt(Me.DbLeeHotelAux2.mDbLector("MOVG_DEST")), "", "", 0, "", 0, "", "", 9)
            End If
            If CType(Me.DbLeeHotelAux2.mDbLector("TIPO"), Integer) = Me.mTimo_Albaran_Dev Then
                Linea = Linea + 1
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedorAlbaranes(CInt(Me.DbLeeHotelAux2.mDbLector("CODI"))), Me.mIndicadorHaber, CType(" --FORMALIZADO ", String) & " " & CType(Me.DbLeeHotelAux2.mDbLector("ALBARAN"), String), Total, "NO", CType(Me.DbLeeHotelAux2.mDbLector("ALBARAN"), String), "", CInt(Me.DbLeeHotelAux2.mDbLector("MOVG_DEST")), "", "", 0, "", 0, "", "", 9)
            End If



        End While
        Me.DbLeeHotelAux2.mDbLector.Close()
    End Sub
    Private Function BuscaCuentaTipoMovimiento(ByVal vTipoMovimiento As Integer) As String
        Try
            SQL = "SELECT NVL(TIMO_CEXT,'?') "
            SQL += " FROM TNST_TIMO "
            SQL += " WHERE TIMO_CODI = " & vTipoMovimiento

            Me.mResult = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            If IsNothing(Me.mResult) = False Then
                Return Me.mResult
            Else
                Return "0"
            End If
        Catch ex As Exception
            Return "0"
        End Try

    End Function
#End Region
End Class
