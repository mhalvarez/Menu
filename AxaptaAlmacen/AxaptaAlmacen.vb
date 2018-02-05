Option Strict On

Imports System.IO
Public Class AxaptaAlmacen
    Private mDebug As Boolean = False
    Private mStrConexionHotel As String

    Private mStrConexionCentral As String
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


    Private mSoloInventariosIniciales As Boolean = False

    ' Valores de retorno para debug



    Private SQL As String
    Private Linea As Integer
    Private Filegraba As StreamWriter
    Private DbLeeCentral As C_DATOS.C_DatosOledb
    Private DbLeeHotel As C_DATOS.C_DatosOledb
    Private DbLeeNewPaga As C_DATOS.C_DatosOledb
    Private DbLeeHotelAux As C_DATOS.C_DatosOledb
    Private DbLeeHotelAux2 As C_DATOS.C_DatosOledb
    Private DbLeeHotelAux3 As C_DATOS.C_DatosOledb
    Private DbLeeHotelAux4 As C_DATOS.C_DatosOledb
    Private DbGrabaCentral As C_DATOS.C_DatosOledb
    Private DbSpyro As C_DATOS.C_DatosOledb


    ' otros
    Dim FechaInventarioFinal As Date
    Dim FechaInventarioInicial As Date

    Dim Contador As Integer

    Dim AuxStr As String
    Dim AuxInteger As Integer

    Private mResult As String
    Private mTipoGasto As String

    Private mMostrarMensajes As Boolean = True

    Private P_Existenregistros As Boolean = False

#Region "CONSTRUCTOR"
    Public Sub New(ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vStrConexionCentral As String, _
  ByVal vStrConexionHotel As String, ByVal vFecha As Date, ByVal vFile As String, ByVal vDebug As Boolean, _
  ByVal vControlDebug As System.Windows.Forms.TextBox, ByVal vListBox As System.Windows.Forms.ListBox, _
  ByVal vStrConexionSpyro As String, ByVal vInventario As Boolean, ByVal vSoloInventariosIniciales As Boolean, _
  ByVal vEmpNum As Integer, ByVal vControlForm As System.Windows.Forms.Form, ByVal vProgesBar As System.Windows.Forms.ProgressBar, _
  ByVal vConectaNewPaga As Boolean, ByVal vStrConexionNewPaga As String, ByVal vTipoGasto As String, ByVal vMostrarMensajes As Boolean)
        MyBase.New()

        Me.mDebug = vDebug
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


        Me.mMostrarMensajes = vMostrarMensajes

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


        Me.DbLeeHotelAux4 = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
        Me.DbLeeHotelAux4.AbrirConexion()
        Me.DbLeeHotelAux4.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


        If Me.mConectaNewPaga = True Then
            Me.DbLeeNewPaga = New C_DATOS.C_DatosOledb(Me.mStrConexionNewPaga)
            Me.DbLeeNewPaga.AbrirConexion()
            Me.DbLeeNewPaga.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")
        End If




        Me.CargaParametros()
        Me.BorraRegistros()





    End Sub
#End Region
#Region "PROPIEDADES"
    Public ReadOnly Property PControlFecha() As Boolean
        Get
            Return Me.ControlFecha
        End Get

    End Property

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
            SQL += "NVL(PARA_CLIENTES_CONTADO,'?') CLIENTESCONTADO,"
            SQL += "NVL(PARA_TIMO_FACTURA_AL,'0') FACTURAFORMALI,"
            SQL += "NVL(PARA_TIMO_SALIDAGASTOS,'0') SALIDAGASTOS,"
            SQL += "NVL(PARA_SERIE_FAC_SPYRO,'?') SERIEFAC,"
            SQL += "NVL(PARA_SERIE_FAC2_SPYRO,'?') SERIEFACPAGA,"
            SQL += "NVL(PARA_TIMO_ROTURAS,'0') ROTURAS,"
            SQL += "NVL(PARA_FILE_SPYRO_PATH,'?') PATCH,"
            SQL += "NVL(PARA_CFATODIARI_COD_INV,'?') DIARIOINV,"
            SQL += "NVL(PARA_SOLO_FACTURAS,0) PARA_SOLO_FACTURAS "


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
                Me.mCtaClientesContado = CType(Me.DbLeeCentral.mDbLector.Item("CLIENTESCONTADO"), String)
                Me.mParaSeriefacturas = CType(Me.DbLeeCentral.mDbLector.Item("SERIEFAC"), String)
                Me.mParaSeriefacturasNewPaga = CType(Me.DbLeeCentral.mDbLector.Item("SERIEFACPAGA"), String)
                Me.mParaFilePath = CType(Me.DbLeeCentral.mDbLector.Item("PATCH"), String)
                Me.mCfatodiari_Cod_Inv = CType(Me.DbLeeCentral.mDbLector.Item("DIARIOINV"), String)
                Me.mParaSoloFacturas = CType(Me.DbLeeCentral.mDbLector.Item("PARA_SOLO_FACTURAS"), Integer)
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
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Function ControlCentrosdeCosto() As Integer
        SQL = "SELECT NVL(COUNT(*),'0') AS TOTAL FROM TNST_ALMA WHERE ALMA_CCST IS NULL"
        Return CType(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL), Integer)
    End Function
    Private Function ControlFecha() As Boolean

        Try
            SQL = "SELECT PARA_FETR - 1 FROM TNST_PARA "

            If Me.mFecha > CDate(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) Then
                SQL = "SELECT PARA_FETR   FROM TNST_PARA "
                If mMostrarMensajes = True Then
                    MsgBox("Fecha NO Válida " & vbCrLf & "Fecha Actual de Almacen = " & CDate(Me.DbLeeHotel.EjecutaSqlScalar(SQL)))
                    Return False
                End If

            Else
                Return True
            End If


        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Control Fecha de Almacen")
            End If
            Return False

        End Try




    End Function
    Private Sub BorraRegistros()

        Try
            SQL = "SELECT COUNT(*) FROM TS_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
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
                SQL = "DELETE TS_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
                SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            End If


        Catch ex As Exception
            If mMostrarMensajes = True Then
                MsgBox(ex.Message, MsgBoxStyle.Information, "Borra Registros")
            End If

        End Try




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
    Public Sub Procesar()
        Try



            If Me.P_Existenregistros = False Then

                ' ---------------------------------------------------------------
                ' Asiento de Albaranes 1
                '----------------------------------------------------------------
                Me.mTextDebug.Text = "Calculando Pdte. de Formalizar"
                Me.mTextDebug.Update()


                Me.AxFacturas()



                Me.mProgresBar.Value = 10
                Me.mProgresBar.Update()
                Me.mControlForm.Update()


                Me.mProgresBar.Value = 100
                Me.mProgresBar.Update()
                Me.mControlForm.Update()

            End If
          

            '    Me.CerrarFichero()
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
            Me.DbLeeHotelAux4.CerrarConexion()

            If IsNothing(Me.DbLeeNewPaga) = False Then
                If Me.DbLeeNewPaga.EstadoConexion = ConnectionState.Open Then
                    Me.DbLeeNewPaga.CerrarConexion()
                End If
            End If
            Me.DbSpyro.CerrarConexion()
        Catch ex As Exception

        End Try

    End Sub



#End Region
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String,
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double,
                                     ByVal vAjuste As String, ByVal vDocu As String, ByVal vDore As String,
                                     ByVal vAuxiliarString As String, ByVal vNif As String, ByVal vDtoCabPorce As Double, ByVal vDtoCabValor As Double,
                                     ByVal vTotalValor As Double, ByVal vTotalBase As Double, ByVal vLineas As Integer, ByVal vMovgCodi As Integer,
                                     ByVal vMovgAnci As Integer, ByVal vTipoDoc As Integer _
                                     , ByVal vAxCodigo As String, ByVal vAxCantidad As Double, ByVal vAxPrun As Double,
                                     ByVal vImpuestoLinea As Double, ByVal vDtoLinPorce As Double, ByVal vDtoLinValor As Double, ByVal vFechaDoc As Date,
                                     ByVal vTipoMov As String, ByVal vTipoMov2 As String, ByVal vAxCantidadBonus As Double,
 ByVal vMovdCodi As Integer, ByVal vMovgDore As String, ByVal vAuxDescuento As Double, ByVal vAjusteDto As Double, ByVal vIvasTaxa As Double, vPrpo As Double)

        Try

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TS_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,ASNT_DEBE,"
            SQL += "ASNT_HABER,ASNT_AJUSTAR,ASNT_DOCU,ASNT_DORE,ASNT_EMP_NUM,ASNT_AUXILIAR_STRING,ASNT_AX_NIF "
            SQL += ",ASNT_AX_DTO_CABD,ASNT_AX_DTO_CABV,ASNT_AX_TOTAL,ASNT_AX_TOTAL_BASE "

            SQL += ",ASNT_AX_TOTAL_LINEAS ,ASNT_AX_MOVG_CODI,ASNT_AX_MOVG_ANCI,ASNT_AX_TIPO "
            SQL += ",ASNT_AX_PRODUCT,ASNT_AX_QNTD,ASNT_AX_PRUN,ASNT_AX_IMPU_LINEA,ASNT_AX_DTO_LIND,ASNT_AX_DTO_LINV,"
            SQL += "ASNT_AUXILIAR_STRING2,ASNT_AX_TIPOMOV,ASNT_AX_QNTD_BONUS,ASNT_AX_MOVD_CODI,ASNT_AX_MOVG_DORE,ASNT_AUX_DTO,ASNT_AJUSTE_DTO,ASNT_TIPO_IGIC,ASNT_AX_PRPO) "


            SQL += " values ('"
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
            '   SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(vFechaDoc, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "',"
            SQL += "'?'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & Mid(vDocu, 1, 15) & "','" & Mid(vDore, 1, 15) & "'," & Me.mEmpNum & ",'" & vAuxiliarString & "','"
            SQL += vNif & "'," & vDtoCabPorce & "," & vDtoCabValor & "," & vTotalValor & "," & vTotalBase & ","
            SQL += vLineas & "," & vMovgCodi & "," & vMovgAnci & "," & vTipoDoc & ",'" & vAxCodigo & "',"
            SQL += vAxCantidad & "," & vAxPrun & "," & vImpuestoLinea & "," & vDtoLinPorce & "," & vDtoLinValor & ",'"
            SQL += vTipoMov & "','"
            SQL += vTipoMov2 & "',"
            SQL += vAxCantidadBonus & "," & vMovdCodi & ",'" & vMovgDore & "'," & vAuxDescuento & "," & vAjusteDto & "," & vIvasTaxa & "," & vPrpo & ")"
            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40)
            Me.mTextDebug.Update()


            Me.Contador = Me.Contador + 1
            'Me.mControlForm.ParentForm.Text = "Menu Procesando ...  " & Contador
            'Me.mControlForm.Update()
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
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, ByVal vAjuste As String, ByVal vDocu As String, ByVal vDore As String, ByVal vAlmaCodi As Integer)
        Try

            If Me.mTipoAsiento = "DEBE" Then
                Me.mDebe = vImonep
                Me.mHbaber = 0
            Else
                Me.mDebe = 0
                Me.mHbaber = vImonep

            End If

            SQL = "INSERT INTO TS_ASNT(ASNT_TIPO_REGISTRO,ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_CFEJERC_COD,ASNT_CFATODIARI_COD,ASNT_CFATOCAB_REFER,"
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_DOCU,ASNT_DORE,ASNT_EMP_NUM,ASNT_ALMA_CODI,ASNT_ALMA_DESC) values ('"
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
            '   SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "',"
            SQL += "'?'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & Mid(vDocu, 1, 15) & "','" & Mid(vDore, 1, 15) & "'," & Me.mEmpNum & "," & vAlmaCodi & ",'" & Me.DameNombreAlmacen(vAlmaCodi) & "')"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40)
            Me.mTextDebug.Update()


            Me.Contador = Me.Contador + 1
            'Me.mControlForm.ParentForm.Text = "Menu Procesando ...  " & Contador
            'Me.mControlForm.Update()
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
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
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
            SQL += Mid(vAmpcpto, 1, 40) & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            ' SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(vFechaValor, "dd/MM/yyyy") & "',"
            SQL += "'?'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & Mid(vDocu, 1, 15) & "','" & Mid(vDore, 1, 15) & "'," & Me.mEmpNum & ",'" & vAuxString & "','" & vAuxString2 & "')"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40)
            Me.mTextDebug.Update()


            Me.Contador = Me.Contador + 1
            'Me.mControlForm.ParentForm.Text = "Menu Procesando ...  " & Contador
            'Me.mControlForm.Update()
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
    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
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
            SQL += Mid(vAmpcpto, 1, 40) & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            ' SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Format(vFechaValor, "dd/MM/yyyy") & "',"
            SQL += "'?'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & Mid(vDocu, 1, 15) & "','" & Mid(vDore, 1, 15) & "'," & Me.mEmpNum & ",'" & vAuxString & "','" & vAuxString2 & "'," & vAlmacodi & ",'" & Me.DameNombreAlmacen(vAlmacodi) & "')"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40)
            Me.mTextDebug.Update()


            Me.Contador = Me.Contador + 1
            'Me.mControlForm.ParentForm.Text = "Menu Procesando ...  " & Contador
            'Me.mControlForm.Update()
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
    Private Sub InsertaOracleInventarios(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
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
            '    SQL = "SELECT NVL(ALMA_DESC,'?') AS ALMA_DESC  FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacodi

            SQL = "SELECT NVL(RPAD(ALMA_DESC,30,' ') || '                   Dígitos de Control =  (' || ALMA_CCST  || ')','?') AS ALMA_DESC  FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacodi

            Return Me.DbLeeHotelAux3.EjecutaSqlScalar(SQL)

        Catch ex As Exception
            Return " "
        End Try
    End Function
   

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
 
    Private Function BuscaCuentaProveedorCuentasPorPagar(ByVal vProveedor As Integer) As String

        'SQL = "SELECT TNST_CNTA.CNTA_CNTA "
        'SQL += "FROM TNST_CUFO, TNST_CNTA "
        'SQL += "WHERE TNST_CUFO.CUFO_CNPB = TNST_CNTA.CNTA_CODI"
        'SQL += " AND TNST_CUFO.FORN_CODI = " & vProveedor

        SQL = "SELECT NVL(FORN_VATP,'0') FROM TNST_FORN "
        SQL += " WHERE  TNST_FORN.FORN_CODI = " & vProveedor

        Me.AuxStr = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

        If IsNothing(Me.AuxStr) = False Then
            Return Me.AuxStr
        Else
            Return "0"
        End If
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


        If IsNothing(Me.DbSpyro.EjecutaSqlScalar(SQL)) = False Then
            Return Me.DbSpyro.EjecutaSqlScalar(SQL)
        Else
            Return "0"
        End If
    End Function
    Private Function BuscaCuentaProveedorAlbaranes(ByVal vProveedor As Integer) As String

        SQL = "SELECT NVL(TNST_CNTA.CNTA_CNTA,'?') "
        SQL += "FROM TNST_CUFO, TNST_CNTA "
        SQL += "WHERE TNST_CUFO.CUFO_CNGB = TNST_CNTA.CNTA_CODI"
        SQL += " AND TNST_CUFO.FORN_CODI = " & vProveedor


        If IsNothing(Me.DbLeeHotelAux.EjecutaSqlScalar2(SQL)) = False Then
            Return Me.DbLeeHotelAux.EjecutaSqlScalar2(SQL)
        Else
            Return "0"
        End If
    End Function
    Private Function BuscaCuentaCostoLiquido(ByVal vAlmacen As Integer, ByVal vGrupo As Integer) As String
        'TIPO DE CUENTA  = COSTO LIQUIDOS 
        ' ASIGNAR A COSTOS LIQUIDOS EN NEWSTOK
        SQL = "SELECT NVL(TNST_CNTA.CNTA_CNTA,'?') "
        SQL += "FROM TNST_CUCL, TNST_CNTA "
        SQL += "WHERE TNST_CUCL.CUCL_CNTB = TNST_CNTA.CNTA_CODI"
        SQL += " AND TNST_CUCL.ALMA_CODI = " & vAlmacen
        SQL += " AND TNST_CUCL.GRUP_CODI = " & vGrupo

        If IsNothing(Me.DbLeeHotelAux.EjecutaSqlScalar2(SQL)) = False Then
            Return Me.DbLeeHotelAux.EjecutaSqlScalar2(SQL)
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

        If IsNothing(Me.DbLeeHotelAux.EjecutaSqlScalar2(SQL)) = False Then
            Return Me.DbLeeHotelAux.EjecutaSqlScalar2(SQL)
        Else
            Return "0"
        End If
    End Function

    Private Function BuscaCuentaImpuestoVenta(ByVal vIva As Integer) As String

        SQL = "SELECT NVL(TNST_CNTA.CNTA_CNTA,'?') "
        SQL += "FROM TNST_CUIM, TNST_CNTA "
        SQL += "WHERE TNST_CUIM.CUIM_CNTC = TNST_CNTA.CNTA_CODI"
        SQL += " AND TNST_CUIM.IVAS_CODI = " & vIva

        If IsNothing(Me.DbLeeHotelAux.EjecutaSqlScalar2(SQL)) = False Then
            Return Me.DbLeeHotelAux.EjecutaSqlScalar2(SQL)
        Else
            Return "0"
        End If
    End Function
    Private Function BuscaCuentaImpuestoVenta2(ByVal vIva As Double) As String

        SQL = "SELECT NVL(IMPU_VCTA,'0') "
        SQL += "FROM TS_IMPU "
        SQL += "WHERE IMPU_VALO = " & vIva

        If IsNothing(Me.DbLeeCentral.EjecutaSqlScalar2(SQL)) = False Then
            Return Me.DbLeeCentral.EjecutaSqlScalar2(SQL)
        Else
            Return "0"
        End If
    End Function
    Private Function BuscaCuentaImpuestoDevolucion(ByVal vIva As Integer) As String

        SQL = "SELECT NVL(TNST_CNTA.CNTA_CNTA,'?') "
        SQL += "FROM TNST_CUIM, TNST_CNTA "
        SQL += "WHERE TNST_CUIM.CUIM_CNTD = TNST_CNTA.CNTA_CODI"
        SQL += " AND TNST_CUIM.IVAS_CODI = " & vIva

        If IsNothing(Me.DbLeeHotelAux.EjecutaSqlScalar2(SQL)) = False Then
            Return Me.DbLeeHotelAux.EjecutaSqlScalar2(SQL)
        Else
            Return "0"
        End If
    End Function
    Private Function BuscaCuentaImpuestoDevolucion2(ByVal vIva As Double) As String

        SQL = "SELECT NVL(IMPU_DCTA,'0') "
        SQL += "FROM TS_IMPU "
        SQL += "WHERE IMPU_VALO = " & vIva

        If IsNothing(Me.DbLeeCentral.EjecutaSqlScalar2(SQL)) = False Then
            Return Me.DbLeeCentral.EjecutaSqlScalar2(SQL)
        Else
            Return "0"
        End If
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

            ' Dim Churro As String


            SQL = "SELECT NVL(PARA_CTA_RAIZ9_GASTO,'GGGGG')  "
            SQL += " FROM TS_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND PARA_EMP_NUM = " & Me.mEmpNum
            ' Raiz = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_CTA_RAIZ9_ACTI,'AA')  "
            SQL += " FROM TS_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND PARA_EMP_NUM = " & Me.mEmpNum
            'Actividad = Me.DbLeeCentral.EjecutaSqlScalar(SQL)


            SQL = "SELECT NVL(PARA_CTA_RAIZ9_CENTRO,'CC')  "
            SQL += " FROM TS_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND PARA_EMP_NUM = " & Me.mEmpNum
            'Hotel = Me.DbLeeCentral.EjecutaSqlScalar(SQL)



            '' ARRIBA OBSOLETO


            SQL = "SELECT NVL(SUBSTR(ALMA_CCST,3,2),'HH')  "
            SQL += " FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacen
            Hotel = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            SQL = "SELECT NVL(SUBSTR(ALMA_CCST,1,2),'AA')  "
            SQL += " FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacen
            Departamento = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)




            SQL = "SELECT NVL(FAMI_CCST,'FF')  "
            SQL = "SELECT NVL(FAMI_CCST,'        ')  "
            SQL += " FROM TNST_FAMI WHERE FAMI_CODI = " & vFamilia
            Fami = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            Me.mTextDebug.Text = " Compuesta =  " & Mid(Fami, 1, 5) & Hotel & Departamento & Mid(Fami, 6, 3)


            ' NUEVO PARA MEJORA DE RENDIMIENTO 
            ' SQL = "SELECT NVL(FAMI_CCST,'AAHH FFF')  "
            'SQL += " FROM TNST_FAMI WHERE FAMI_CODI = " & vFamilia
            'Churro = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            'Churro = Mid(Churro, 1, 5) & Mid(Churro, 3, 2) & Mid(Churro, 1, 2) & Mid(Churro, 6, 3)

            Me.mTextDebug.Update()

            'MsgBox(Mid(Fami, 1, 5) & Hotel & Departamento & Mid(Fami, 6, 3) & vbCrLf & Churro)

            Return Mid(Fami, 1, 5) & Hotel & Departamento & Mid(Fami, 6, 3)
        Catch ex As Exception
            Return "0"
        End Try

    End Function

    Private Function DameHotelapartirdeAlmacen(ByVal vAlmacen As Integer) As String
        Try
            SQL = "SELECT NVL(SUBSTR(ALMA_CCST,5,2),'EE')  "
            SQL += " FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacen
            Me.AuxStr = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

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
    

    Private Sub AxFacturas()
        Dim Total As Double
        
        Dim CodigoAxGenerico As String

        Dim DtoCabeceraDto As Decimal
        Dim DtoCabeceraValor As Decimal

        Dim TotalNeto As Decimal

        Dim TotalLineas As Integer

        Dim TotalImpuestoLinea As Decimal

        Dim DtoLineaDto As Decimal
        Dim DtoLineaValor As Decimal

        Dim DtoLineaValorAuxiliar As Decimal

        Dim TipoMovimiento As String = ""
        Dim TipoMovimientoCod As String = ""
        Dim TipoAnulacionInt As Integer = 0

        Dim Docreferencia As String = ""


        Dim PrecioUnidad As Double







        Dim NetoLineaNewStock As Double
        Dim NetoLineaNormal As Double
        Dim ValorDescuentoLinea As Double
        Dim DiferenciaCalculo As Double

        Try


            SQL = "  SELECT 'TRABAJO', "
            SQL += "         TNST_MOVG.MOVG_DAVA, "
            SQL += "         NVL (FORN_CNTR, '?') AS NIF, "
            SQL += "         nvl(TNST_MOVG.MOVG_IDDO,'Sin Número') AS DOCUMENTO, "
            SQL += "         FAMI_DESC AS FAMILIA, "
            SQL += "         ALMA_DESC AS ALMACEN, "
            SQL += "         FAMI_ABRV AS FAMI_ABRV, "
            SQL += "         NVL (TO_CHAR (IVAS_TAXA), '?') AS IVAS_TAXA, "
            SQL += "         ALMA_ABRV AS ALMA_ABRV, "
            SQL += "         FAMI_ABRV || TRIM (TO_CHAR (NVL (IVAS_TAXA, 0), '09')) || ALMA_ABRV "
            SQL += "            AS AXAPTA, "
            SQL += "         PROD_DESC, "
            SQL += "         TNST_MOVD.MOVD_QNTD AS CANTIDAD, "
            SQL += "         NVL(TNST_MOVD.MOVD_BONU,0) AS BONUS, "
            SQL += "         TNST_MOVD.MOVD_PRUN AS ""PVP UNIDAD"", "
            SQL += "         TNST_MOVD.MOVD_PRPO AS ""PVP PRO"", "
            SQL += "         TNST_MOVD.MOVD_TOTA_B AS ""TOTAL LINEA"", "
            SQL += "         TNST_MOVG.MOVG_CODI AS MOVG_CODI, "
            SQL += "         TNST_MOVG.MOVG_ANCI AS MOVG_ANCI, "
            SQL += "         '+', "
            SQL += "         TNST_MOVD.MOVD_IVAS AS MOVD_IVAS, "
            '   SQL += "         NVL(TNST_MOVG.MOVG_IDDO ,'?') AS MOVG_IDDO, "

            ' Path y Modificaciones
            '20121105
            SQL += "         TNST_MOVG.MOVG_DORE AS MOVG_DORE, "
            '

            SQL += "         TNST_MOVG.MOVG_VATO AS TOTALD,  "
            SQL += "         TNST_MOVG.MOVG_TIIM MODOIMPUESTO,  "

            SQL += "         NVL(TNST_MOVD.MOVD_DEPO ,0) AS MOVD_DEPO, "
            SQL += "         NVL(TNST_MOVD.MOVD_DESC ,0) AS MOVD_DESC, "

            SQL += "         TNST_MOVG.MOVG_DADO AS MOVG_DADO, "
            SQL += "         TNST_TIMO.TIMO_TIPO AS TIMO_TIPO,  "
            SQL += "         TNST_TIMO.TIMO_CODI AS TIMO_CODI,  "
            SQL += "         TNST_MOVD.MOVD_CODI AS MOVD_CODI  "


            SQL += "    FROM TNST_MOVG, "
            SQL += "         TNST_MOVD, "
            SQL += "         TNST_ALMA, "
            SQL += "         TNST_PROD, "
            SQL += "         TNST_FAMI, "
            SQL += "         TNST_IVAS, "
            SQL += "         TNST_TIMO, "
            SQL += "         TNST_FORN "
            SQL += "   WHERE     (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI) "
            SQL += "         AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI) "
            SQL += "         AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI) "
            SQL += "         AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI) "
            SQL += "         AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI) "
            SQL += "         AND (TNST_PROD.FAMI_CODI = TNST_FAMI.FAMI_CODI) "

          
            SQL += "         AND (TNST_MOVD.IVAS_CODI = TNST_IVAS.IVAS_CODI) "
            '
            SQL += "         AND (TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI "
            SQL += "              OR TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI) "


            SQL += " AND TNST_MOVD.MOVD_QNTD <> 0 "

            SQL += "         AND TNST_TIMO.TIMO_TIPO IN(" & Me.mTimo_Factura_Directa & "," & Me.mTimo_Factura_Directa_Dev & ")"

            SQL += "         AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"

            ' excluye albaranes marcados como con impuestos incluidos porque dan error
            ' SOLO TRATA TIPO 2 IMPUESTOS NO INCLUIDOS EN LOS PRECIOS
            SQL += "   AND     (TNST_MOVG.MOVG_TIIM = 2  ) "

            SQL += " ORDER BY TNST_MOVG.MOVG_CODI, TNST_MOVG.MOVG_ANCI, MOVD_CODI ASC "




            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                '  If CStr(Me.DbLeeHotel.mDbLector("DOCUMENTO")) = "2988" Then
                'Dim P As String = ""
                'P = "DEBUG"
                'End If

                CodigoAxGenerico = CStr(Me.DbLeeHotel.mDbLector("AXAPTA"))



                ' Tipo de Movimiento 

                If CStr(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = "1" Then
                    ' ALBARAN
                    TipoMovimientoCod = "A"
                    If CInt(Me.DbLeeHotel.mDbLector("TIMO_CODI")) > 0 Then
                        TipoMovimiento = "Albaran"
                        TipoAnulacionInt = 0
                    Else
                        TipoMovimiento = "Albaran Anulado"
                        TipoAnulacionInt = 1
                    End If
                End If

                If CStr(Me.DbLeeHotel.mDbLector("TIMO_TIPO")) = "6" Then
                    ' DEVOLUCION
                    TipoMovimientoCod = "D"
                    If CInt(Me.DbLeeHotel.mDbLector("TIMO_CODI")) > 0 Then
                        TipoMovimiento = "Devolución"
                        TipoAnulacionInt = 0
                    Else
                        TipoMovimiento = "Devolución Anulada"
                        TipoAnulacionInt = 1
                    End If
                End If


                ' DESCUENTOS DE CABECERA 
                SQL = "SELECT NVL(MOVF_DEPO,0)  FROM TNST_MOVF WHERE MOVG_CODI = " & CInt(Me.DbLeeHotel.mDbLector("MOVG_CODI"))
                SQL += " AND MOVG_ANCI = " & CInt(Me.DbLeeHotel.mDbLector("MOVG_ANCI"))


                If Me.DbLeeHotelAux.EjecutaSqlScalar(SQL) = "1" Then
                    SQL = "SELECT NVL(MOVF_DEVA,0)  FROM TNST_MOVF WHERE MOVG_CODI = " & CInt(Me.DbLeeHotel.mDbLector("MOVG_CODI"))
                    SQL += " AND MOVG_ANCI = " & CInt(Me.DbLeeHotel.mDbLector("MOVG_ANCI"))
                    DtoCabeceraDto = CDec(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL))

                    ' El descuento de cabecerasiempre se envia positivo
                    If DtoCabeceraDto < 0 Then
                        DtoCabeceraDto = DtoCabeceraDto * -1
                    End If

                ElseIf Me.DbLeeHotelAux.EjecutaSqlScalar(SQL) = "2" Then
                    SQL = "SELECT NVL(MOVF_DEVA,0)  FROM TNST_MOVF WHERE MOVG_CODI = " & CInt(Me.DbLeeHotel.mDbLector("MOVG_CODI"))
                    SQL += " AND MOVG_ANCI = " & CInt(Me.DbLeeHotel.mDbLector("MOVG_ANCI"))
                    DtoCabeceraValor = CDec(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL))

                    ' El descuento de cabecerasiempre se envia positivo
                    If DtoCabeceraValor < 0 Then
                        DtoCabeceraValor = DtoCabeceraValor * -1
                    End If
                Else
                    DtoCabeceraDto = 0
                    DtoCabeceraValor = 0
                End If


                ' Bases Imponibles 
                SQL = "SELECT SUM(NVL(MOVI_NETO,0))  FROM TNST_MOVI WHERE MOVG_CODI = " & CInt(Me.DbLeeHotel.mDbLector("MOVG_CODI"))
                SQL += " AND MOVG_ANCI = " & CInt(Me.DbLeeHotel.mDbLector("MOVG_ANCI"))
                ' ERROR AQUI VIENE NULO ENERO 2013
                TotalNeto = CDec(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL))

                ' TOTAL LINEAS

                SQL = "SELECT NVL(COUNT(*),0) AS TOTAL FROM TNST_MOVD WHERE MOVG_CODI = " & CInt(Me.DbLeeHotel.mDbLector("MOVG_CODI"))
                SQL += " AND MOVG_ANCI = " & CInt(Me.DbLeeHotel.mDbLector("MOVG_ANCI"))

                TotalLineas = CInt(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL))


                ' VALOR IMPUESTO DE LA LINEA ( FORMULA SACADA DEL REPORT NST019_SM

                If CStr(Me.DbLeeHotel.mDbLector("IVAS_TAXA")) <> "?" Then
                    If IsDBNull(Me.DbLeeHotel.mDbLector("MODOIMPUESTO")) = True Then
                        TotalImpuestoLinea = (CDec(Me.DbLeeHotel.mDbLector("TOTAL LINEA")) * CDec(Me.DbLeeHotel.mDbLector("IVAS_TAXA"))) / 100
                        TotalImpuestoLinea = Math.Round(TotalImpuestoLinea, 2, MidpointRounding.ToEven)

                    ElseIf CStr(Me.DbLeeHotel.mDbLector("MODOIMPUESTO")) = "2" Then
                        TotalImpuestoLinea = (CDec(Me.DbLeeHotel.mDbLector("TOTAL LINEA")) * CDec(Me.DbLeeHotel.mDbLector("IVAS_TAXA"))) / 100
                        TotalImpuestoLinea = Math.Round(TotalImpuestoLinea, 2, MidpointRounding.ToEven)
                    Else
                        TotalImpuestoLinea = CDec(Me.DbLeeHotel.mDbLector("MOVD_IVAS"))
                        TotalImpuestoLinea = Math.Round(TotalImpuestoLinea, 2, MidpointRounding.ToEven)
                    End If
                Else
                    TotalImpuestoLinea = 0
                End If



                ' DESCUENTOS de linea


                ' rutina original 


                If CInt(Me.DbLeeHotel.mDbLector("MOVD_DEPO")) = 1 Then
                    DtoLineaDto = CDec(Me.DbLeeHotel.mDbLector("MOVD_DESC"))
                    DtoLineaValor = 0
                ElseIf CInt(Me.DbLeeHotel.mDbLector("MOVD_DEPO")) = 2 Then
                    DtoLineaDto = 0
                    DtoLineaValor = CDec(Me.DbLeeHotel.mDbLector("MOVD_DESC"))
                Else
                    DtoLineaDto = 0
                    DtoLineaValor = 0
                End If

                '( a veces hay descuento y movd_depo(indicador de tipo de descuento) viene a zero !!!!!!!  ver con Informarca

                If CDec(Me.DbLeeHotel.mDbLector("MOVD_DESC")) <> 0 And CInt(Me.DbLeeHotel.mDbLector("MOVD_DEPO")) = 0 Then
                    DtoLineaValorAuxiliar = CDec(Me.DbLeeHotel.mDbLector("MOVD_DESC"))
                    '   MsgBox("Atención Incongruencia en Base de Datos hay Valores de Descuento pero NO hay Indicador de Tipo de Descuento  Movd_depo", MsgBoxStyle.Critical, "No Contabilize ....")


                    If CDec(Me.DbLeeHotel.mDbLector("PVP UNIDAD")) - CDec(Me.DbLeeHotel.mDbLector("MOVD_DESC")) = CDbl(Me.DbLeeHotel.mDbLector("PVP PRO")) Then
                        DtoLineaDto = 0
                        DtoLineaValor = CDec(Me.DbLeeHotel.mDbLector("MOVD_DESC"))
                    Else
                        DtoLineaDto = CDec(Me.DbLeeHotel.mDbLector("MOVD_DESC"))
                        DtoLineaValor = 0
                    End If

                Else
                    DtoLineaValorAuxiliar = 0
                End If



                ' 2013  audita calculo de descuento de linea 

                ' debug





                If DtoLineaDto > 0 Then
                    NetoLineaNewStock = CDbl(Me.DbLeeHotel.mDbLector("TOTAL LINEA"))
                    ValorDescuentoLinea = (CDbl(Me.DbLeeHotel.mDbLector("PVP UNIDAD")) * DtoLineaDto) / 100
                    NetoLineaNormal = (CDbl(Me.DbLeeHotel.mDbLector("PVP UNIDAD")) - ValorDescuentoLinea) * CDbl(Me.DbLeeHotel.mDbLector("CANTIDAD"))

                    DiferenciaCalculo = Math.Round(NetoLineaNormal, 2, MidpointRounding.AwayFromZero) - NetoLineaNewStock
                    DiferenciaCalculo = Math.Round(DiferenciaCalculo, 2, MidpointRounding.AwayFromZero)
                Else
                    NetoLineaNewStock = 0
                    NetoLineaNormal = 0
                    ValorDescuentoLinea = 0
                    DiferenciaCalculo = 0
                End If





                If IsDBNull(Me.DbLeeHotel.mDbLector("MOVG_DORE")) = False Then
                    Docreferencia = CStr(Me.DbLeeHotel.mDbLector("MOVG_DORE"))
                Else
                    Docreferencia = ""
                End If


                If IsDBNull(CDbl(Me.DbLeeHotel.mDbLector("PVP UNIDAD"))) = True Or CDbl(Me.DbLeeHotel.mDbLector("PVP UNIDAD")) = 0 Then
                    PrecioUnidad = CDbl(Me.DbLeeHotel.mDbLector("PVP PRO"))
                Else
                    PrecioUnidad = CDbl(Me.DbLeeHotel.mDbLector("PVP UNIDAD"))
                End If



                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL LINEA"), Double)
                Me.mTipoAsiento = "DEBE"

                Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, CodigoAxGenerico, Me.mIndicadorDebe,
                                 CType("FAMILIA", String).Replace("'", ",") & " " & CType(Me.DbLeeHotel.mDbLector("FAMILIA"), String).Replace("'", ",") _
                                 & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String).Replace("'", ","), Total, "NO",
                                 CStr(Me.DbLeeHotel.mDbLector("DOCUMENTO")), "", CStr(Me.DbLeeHotel.mDbLector("PROD_DESC")).Replace("'", ","),
                                 CStr(Me.DbLeeHotel.mDbLector("NIF")), DtoCabeceraDto, DtoCabeceraValor,
                                 CDbl(Me.DbLeeHotel.mDbLector("TOTALD")), TotalNeto, TotalLineas,
                                 CInt(Me.DbLeeHotel.mDbLector("MOVG_CODI")), CInt(Me.DbLeeHotel.mDbLector("MOVG_ANCI")), TipoAnulacionInt, CodigoAxGenerico _
                                 , CDbl(Me.DbLeeHotel.mDbLector("CANTIDAD")), PrecioUnidad, TotalImpuestoLinea,
                                 DtoLineaDto, DtoLineaValor, CDate(Me.DbLeeHotel.mDbLector("MOVG_DADO")), TipoMovimiento, TipoMovimientoCod,
                                 CDbl(Me.DbLeeHotel.mDbLector("BONUS")), CInt(Me.DbLeeHotel.mDbLector("MOVD_CODI")), Docreferencia, DtoLineaValorAuxiliar, DiferenciaCalculo, CDbl(Me.DbLeeHotel.mDbLector("IVAS_TAXA")), CDbl(Me.DbLeeHotel.mDbLector("PVP PRO")))

            End While
            Me.DbLeeHotel.mDbLector.Close()

            '----------------------------------------------------------------------------------------------------------
            ' ANULADOS

            

        Catch ex As Exception
            MsgBox("GastosPorCentrodeCostoAlbaranes" & vbCrLf & ex.Message, MsgBoxStyle.Information, "Atención")
        End Try
    End Sub


#End Region


End Class
