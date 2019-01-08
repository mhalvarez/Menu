
Imports System.IO
Public Class IntegraAlmacen
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



    Private mContabilizaAlbaranes As Boolean
    Private mTipoFormalizaAlbaran As String
    Private mParaSeriefacturas As String
    Private mTimo_Albaran As Integer
    Private mTimo_Albaran_Dev As Integer
    Private mTimo_Traspaso As Integer
    Private mTimo_Factura_Directa As Integer
    Private mTimo_Factura_Directa_Dev As Integer
    Private mTimo_Factura_Al As Integer
    Private mTimo_Salida_Gastos As Integer
    Private mTimo_Roturas As Integer

    Private mParaFilePath As String







    ' parametros cuentas

    Private mCtaFormalizaAlbaranes As String
    Private mCtaMercaderia As String




    Private mCfivaLibro_Cod As String
    Private mCfivaClase_Cod As String
    Private mMonedas_Cod As String
    Private mCfatodiari_Cod As String
    Private mCfatodiari_Cod_Inv As String

    Private mParaTipoAnalitica As String

    Private mCfivatimpu_Cod As String
    Private mCfivatip_Cod As String
    Private mCfatotip_Cod As String
    Private mGvagente_Cod As String
    Private mCtaClientesContado As String
    Private mCtaRoturas As String


    Private mSoloInventariosIniciales As Boolean = False

    ' Valores de retorno para debug



    Private SQL As String
    Private SQL2 As String
    Private Linea As Integer
    Private Filegraba As StreamWriter
    Private DbLeeCentral As C_DATOS.C_DatosOledb
    Private DbLeeHotel As C_DATOS.C_DatosOledb
    Private DbLeeHotelAux As C_DATOS.C_DatosOledb
    Private DbLeeHotelAux2 As C_DATOS.C_DatosOledb
    Private DbGrabaCentral As C_DATOS.C_DatosOledb
    Private DbSpyro As C_DATOS.C_DatosOledb


    ' otros
    Dim FechaInventarioFinal As Date
    Dim FechaInventarioInicial As Date

    Private mResult As String

    Private Enum mEnumTipoDebeHaber
        Debe
        Haber
    End Enum
    Private Enum mEnumTipoAsiento
        Albaranes = 1
        Traspasos = 2
        Salidasgastos = 20
        FacturasDirectas = 3
        AlbaranesDev = 4
        FacturasDirectasDev = 5
        FacturasFormaliza = 6
        Roturas = 30

    End Enum
    ' OTROS 
    Private iASCII(63) As Integer       'Para conversión a MS-DOS





#Region "CONSTRUCTOR"
    Public Sub New(ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vStrConexionCentral As String,
  ByVal vStrConexionHotel As String, ByVal vFecha As Date, ByVal vFile As String, ByVal vDebug As Boolean,
  ByVal vConrolDebug As System.Windows.Forms.TextBox, ByVal vListBox As System.Windows.Forms.ListBox, ByVal vStrConexionSpyro As String, ByVal vInventario As Boolean, ByVal vSoloInventariosIniciales As Boolean, vEmpNum As Integer)
        MyBase.New()

        Me.mDebug = vDebug
        Me.mEmpGrupoCod = vEmpGrupoCod
        Me.mEmpCod = vEmpCod
        Me.mEmpNum = vEmpNum
        Me.mStrConexionHotel = vStrConexionHotel
        Me.mStrConexionCentral = vStrConexionCentral
        Me.mStrConexionSpyro = vStrConexionSpyro
        Me.mFecha = vFecha


        Me.mTextDebug = vConrolDebug
        Me.mListBoxDebug = vListBox

        Me.mListBoxDebug.Items.Clear()
        Me.mListBoxDebug.Update()

        Me.mSoloInventariosIniciales = vSoloInventariosIniciales



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

        Me.DbSpyro = New C_DATOS.C_DatosOledb(Me.mStrConexionSpyro)
        Me.DbSpyro.AbrirConexion()
        'Me.DbSpyro.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


        Me.CargaParametros()
        Me.BorraRegistros()

        If vInventario = False Then
            Me.CrearFichero(Me.mParaFilePath & vFile)
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
            SQL += "NVL(PARA_CLIENTES_CONTADO,'?') CLIENTESCONTADO,"
            SQL += "NVL(PARA_TIMO_FACTURA_AL,'0') FACTURAFORMALI,"
            SQL += "NVL(PARA_TIMO_SALIDAGASTOS,'0') SALIDAGASTOS,"
            SQL += "NVL(PARA_SERIE_FAC_SPYRO,'0') SERIEFAC,"
            SQL += "NVL(PARA_TIMO_ROTURAS,'0') ROTURAS,"
            SQL += "NVL(PARA_FILE_SPYRO_PATH,'?') PATCH,"
            SQL += "NVL(PARA_CFATODIARI_COD_INV,'?') DIARIOINV,"
            SQL += "NVL(PARA_SPYRO_TIPO_ANALITICA,'?') PARA_SPYRO_TIPO_ANALITICA"


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
                Me.mParaFilePath = CType(Me.DbLeeCentral.mDbLector.Item("PATCH"), String)
                Me.mCfatodiari_Cod_Inv = CType(Me.DbLeeCentral.mDbLector.Item("DIARIOINV"), String)
                Me.mParaTipoAnalitica = CType(Me.DbLeeCentral.mDbLector.Item("PARA_SPYRO_TIPO_ANALITICA"), String)



            End If
            Me.DbLeeCentral.mDbLector.Close()
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try
    End Sub
    Private Sub CrearFichero(ByVal vPath As String)
        Try
            '   Filegraba = New StreamWriter(vPath, False, System.Text.Encoding.UTF8)
            Filegraba = New StreamWriter(vPath, False, System.Text.Encoding.ASCII)
            Filegraba.WriteLine("")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
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


        Return sMSD

    End Function
    Private Function ControlCentrosdeCosto() As Integer
        SQL = "SELECT NVL(COUNT(*),'0') AS TOTAL FROM TNST_ALMA WHERE ALMA_CCST IS NULL"
        Return CType(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL), Integer)
    End Function

    Private Sub BorraRegistros()
        SQL = "DELETE TS_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
        Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
        SQL = "DELETE TH_ERRO WHERE ERRO_F_ATOCAB =  '" & Me.mFecha & "'"
        Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
        SQL = "DELETE TH_INCI WHERE INCI_DATR =  '" & Me.mFecha & "'"
        Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
    End Sub
    Private Sub BorraRegistrosInventario(ByVal vFecha As Date)
        SQL = "DELETE TS_ASNT WHERE ASNT_F_VALOR = '" & vFecha & "'"
        Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
        SQL = "DELETE TH_ERRO WHERE ERRO_F_ATOCAB =  '" & vFecha & "'"
        Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
    End Sub
    Private Function CalculaBaseImponible(ByVal vMovi As Integer, ByVal vAnci As Integer) As Double
        SQL = "SELECT SUM(MOVI_NETO)AS BASE "
        SQL += "FROM TNST_MOVI  "
        SQL += "WHERE TNST_MOVI.MOVG_CODI = " & vMovi
        SQL += " AND TNST_MOVI.MOVG_ANCI = " & vAnci
        Return CType(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL), Double)
    End Function
    Public Sub Procesar()
        Try

            '    If Me.ControlCentrosdeCosto > 0 Then
            '   MsgBox("Existen Departamentos sin Centro de Costo en NewStock", MsgBoxStyle.Information, "Atención")
            '  Me.CerrarFichero()
            ' Me.CierraConexiones()
            'Exit Sub
            'End If
            ' ---------------------------------------------------------------
            ' Asiento de Albaranes 1
            '----------------------------------------------------------------
            Me.mTextDebug.Text = "Calculando Pdte. de Formalizar"
            Me.mTextDebug.Update()

            If Me.mContabilizaAlbaranes = True Then
                If Me.mTipoFormalizaAlbaran = "G" Then
                    Me.TotalPendienteFormalizar()
                Else
                    Me.TotalPendienteFormalizarProveedor()
                End If

                Me.mTextDebug.Text = "Calculando Gastos por Departamento Albaranes"
                Me.mTextDebug.Update()

                '    Me.GastosPorCentrodeCostoAlbaranesAlmacen()
                Me.GestionGastosAnalitica(Me.mTimo_Albaran, mEnumTipoAsiento.Albaranes, mEnumTipoDebeHaber.Debe, "Gastos ", "", "")


            End If



            'Me.AjustarDecimales(1)
            ' ---------------------------------------------------------------
            ' Asiento de Traspasos Internos 2
            '----------------------------------------------------------------

            Me.mTextDebug.Text = "Calculando Salidas por Traspaso"
            Me.mTextDebug.Update()
            '   Me.TraspasosSalidasAlmacen()
            Me.GestionGastosAnalitica(Me.mTimo_Traspaso, mEnumTipoAsiento.Traspasos, mEnumTipoDebeHaber.Haber, "Traspasos Salida ", "", "-1")


            Me.mTextDebug.Text = "Calculando Entradas por Traspaso"
            Me.mTextDebug.Update()
            '   Me.TraspasosEntradasAlmacen()
            Me.GestionGastosAnalitica(Me.mTimo_Traspaso, mEnumTipoAsiento.Traspasos, mEnumTipoDebeHaber.Debe, "Traspasos Entrada ", "", "1")

            '---------------------------------------------------------------
            ' Asiento de Salidas a Gasto 20
            '----------------------------------------------------------------

            Me.mTextDebug.Text = "Calculando Salidas a Gasto"
            Me.mTextDebug.Update()
            '    Me.SalidasGastoSalidas()
            ' Me.GestionGastosAnalitica(Me.mTimo_Salida_Gastos, mEnumTipoAsiento.Salidasgastos, mEnumTipoDebeHaber.Haber, "Salida Gastos ", "", "-1")


            Me.mTextDebug.Text = "Calculando Entradas por Salida a Gasto"
            Me.mTextDebug.Update()
            '   Me.SalidasGastoEntradas()
            '  Me.GestionGastosAnalitica(Me.mTimo_Salida_Gastos, mEnumTipoAsiento.Salidasgastos, mEnumTipoDebeHaber.Debe, "Salida Gastos ", "", "1")



            ' ---------------------------------------------------------------
            ' Asiento de Facturas Directas 3
            '----------------------------------------------------------------

            Me.mTextDebug.Text = "Facturas Directas Proveedor"
            Me.mTextDebug.Update()
            Me.TotalFacturasProveedor()



            Me.mTextDebug.Text = "Calculando Gastos por Departamento Facturas"
            Me.mTextDebug.Update()
            '   Me.GastosPorCentrodeCostoFacturasAlmacen()
            Me.GestionGastosAnalitica(Me.mTimo_Factura_Directa, mEnumTipoAsiento.FacturasDirectas, mEnumTipoDebeHaber.Debe, "Gastos ", "", "")



            Me.mTextDebug.Text = "Calculando Impuesto por Facturas Directas"
            Me.mTextDebug.Update()
            Me.TotalFacturasProveedorImpuesto()
            '--------------------------------------------------

            ' ---------------------------------------------------------------
            ' Devolucion de Albaranes 4
            '----------------------------------------------------------------
            Me.mTextDebug.Text = "Calculando Devolucion Pdte. de Formalizar"
            Me.mTextDebug.Update()

            If Me.mContabilizaAlbaranes = True Then
                If Me.mTipoFormalizaAlbaran = "G" Then
                    Me.TotalPendienteFormalizarDevolucionAlbaran()
                Else
                    Me.TotalPendienteFormalizarProveedorDevolucionAlbaran()
                End If

                Me.mTextDebug.Text = "Calculando Gastos por Devolución  Departamento Albaranes"
                Me.mTextDebug.Update()
                '   Me.GastosPorCentrodeCostoAlbaranesDevolucionAlbaranAlmacen()
                Me.GestionGastosAnalitica(Me.mTimo_Albaran_Dev, mEnumTipoAsiento.AlbaranesDev, mEnumTipoDebeHaber.Haber, "Gastos ", "", "")

            End If

            ' ---------------------------------------------------------------
            ' Asiento de DEVOLUCVION Facturas Directas 5
            '----------------------------------------------------------------

            Me.mTextDebug.Text = "Devoluvión Facturas Directas Proveedor"
            Me.mTextDebug.Update()
            Me.TotalFacturasProveedorDevolucion()


            Me.mTextDebug.Text = "Devoluvión  Calculando Gastos por Departamento Facturas"
            Me.mTextDebug.Update()
            ' Me.GastosPorCentrodeCostoFacturasDevolucionAlmacen()
            Me.GestionGastosAnalitica(Me.mTimo_Factura_Directa_Dev, mEnumTipoAsiento.FacturasDirectasDev, mEnumTipoDebeHaber.Haber, "Gastos ", "", "")



            Me.mTextDebug.Text = "Devoluvión Calculando Impuesto por Facturas Directas"
            Me.mTextDebug.Update()
            Me.TotalFacturasProveedorImpuestoDevolucion()


            ' ---------------------------------------------------------------
            ' Asiento de Facturas que formalizan Albaranes Directas 6
            '----------------------------------------------------------------
            Me.mTextDebug.Text = "Facturas Formaliza Albaranes Proveedor"
            Me.mTextDebug.Update()
            Me.TotalFacturasProveedorFormalizadas()



            Me.mTextDebug.Text = "Calculando Impuesto por Facturas Formalizadas"
            Me.mTextDebug.Update()
            Me.TotalFacturasProveedorImpuestoFormalizadas()



            Me.mTextDebug.Text = "Calculando Albaranes Formalizados"
            Me.mTextDebug.Update()
            'Me.TotalAlbaranesProveedorFormalizados()

            ' ---------------------------------------------------------------
            ' Asiento Roturas 30
            '----------------------------------------------------------------
            Me.mTextDebug.Text = "Roturas"
            Me.mTextDebug.Update()
            '  Me.Roturas()
            '   Me.GestionGastosAnalitica(Me.mTimo_Roturas, mEnumTipoAsiento.Roturas, mEnumTipoDebeHaber.Debe, "Gastos Roturas", "")
            '   Me.GestionGastosAnalitica(Me.mTimo_Roturas, mEnumTipoAsiento.Roturas, mEnumTipoDebeHaber.Haber, "Gastos Roturas", Me.mCtaRoturas)



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
            Me.DbSpyro.CerrarConexion()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub SpyroCompruebaCuentas()
        Try
            SQL = "SELECT DISTINCT ASNT_CFCTA_COD,ASNT_TIPO_REGISTRO,ASNT_CFCPTOS_COD FROM TS_ASNT WHERE "
            SQL += "     ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum
            ' SQL += " AND ASNT_F_ATOCAB = '" & Me.mFecha & "'"
            SQL += " AND ASNT_F_VALOR = '" & Me.mFecha & "'"
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

    Private Sub SpyroCompruebaCentrosDeCosto(vCentroCosto As String, vTextoApunte As String)
        Try


            SQL = "SELECT NVL(COUNT(*),0) AS CONTROL   FROM CFCCOSTO WHERE EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "  AND COD = '" & vCentroCosto & "'"



            If CStr(Me.DbSpyro.EjecutaSqlScalar(SQL)) = "0" Then
                Me.mTexto = "SPYRO   : " & vCentroCosto & " No se localiza Centro de Costo de Spyro para  Apunte " & vTextoApunte
                SQL = "INSERT INTO TH_ERRO ( ERRO_F_ATOCAB, ERRO_CBATOCAB_REFER, ERRO_LINEA,"
                SQL += "ERRO_DESCRIPCION ) VALUES ('" & Format(Me.mFecha, "dd/MM/yyyy") & "'," & 99 & "," & 1 & ",'" & Me.mTexto & "')"
                Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)
            End If





        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub SpyroCompruebaFacturas()
        Try
            SQL = "SELECT DISTINCT NVL(ASNT_DOCU,'?') AS ASNT_DOCU ,ASNT_I_MONEMP,ASNT_FACTURA_SERIE FROM TS_ASNT WHERE "
            SQL += "     ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.mEmpNum
            '  SQL += " AND ASNT_F_ATOCAB = '" & Me.mFecha & "'"
            SQL += " AND ASNT_F_VALOR = '" & Me.mFecha & "'"

            SQL += " AND ASNT_CFCPTOS_COD IN('" & Me.mIndicadorDebeFac & "','" & Me.mIndicadorHaberFac & "')"
            SQL += " AND ASNT_FACTURA_SERIE IS NOT NULL"


            Me.DbLeeCentral.TraerLector(SQL)
            While Me.DbLeeCentral.mDbLector.Read


                Me.mTextDebug.Text = "Validando existencia de Factura ya Contabilizada " & CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_DOCU"))
                Me.mTextDebug.Update()
                SQL = "SELECT N_FACTURA FROM FACTURAS WHERE EMP_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += "  AND CFIVALIBRO_COD = '" & Me.mCfivaLibro_Cod & "'"
                ' SQL += "  AND FACTUTIPO_COD IN ('" & CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_FACTURA_SERIE")) & "','" & Me.mParaSeriefacturasNewPaga & Me.mFecha.Year & "')"
                SQL += "  AND FACTUTIPO_COD IN ('" & CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_FACTURA_SERIE")) & "')"

                SQL += "  AND S_FACTURA = '" & CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_DOCU")) & "'"
                SQL += "  AND I_MONEMP  = " & CDbl(Me.DbLeeCentral.mDbLector.Item("ASNT_I_MONEMP"))



                If Me.DbSpyro.EjecutaSqlScalar(SQL) <> "" Then
                    Me.mTexto = "SPYRO   : " & CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_DOCU")) & " Documento Posiblemente Contabilizado ya en Spyro Su Factura + Importe = " & CDbl(Me.DbLeeCentral.mDbLector.Item("ASNT_I_MONEMP"))
                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)
                End If


                ' control de existencia de serie de facturas
                SQL = "SELECT COD FROM FACTUTIPO WHERE COD = '" & CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_FACTURA_SERIE")) & "'"
                SQL += "  AND CFIVALIBRO_COD = '" & Me.mCfivaLibro_Cod & "'"



                If Me.DbSpyro.EjecutaSqlScalar(SQL) = "" Then
                    Me.mTexto = "SPYRO   : " & CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_DOCU")) & " No se Localiza Serie de Facturas en Spyro = " & CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_FACTURA_SERIE"))
                    Me.GestionIncidencia(Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.mTexto)
                End If




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
            SQL += Mid(vAmpcpto, 1, 40) & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Now, "dd/MM/yyyy") & "','"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "',"
            SQL += "'?'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vDocu & "','" & vDore & "'," & Me.mEmpNum & ")"


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40)
            Me.mTextDebug.Update()

            If vCfcta_Cod.Length < 2 Then
                Me.mListBoxDebug.Items.Add("NEWSTOCK: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40))
            End If


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
            SQL += "ASNT_LINEA,ASNT_CFCTA_COD,ASNT_CFCPTOS_COD,ASNT_AMPCPTO,ASNT_I_MONEMP,ASNT_CONCIL_SN,ASNT_F_ATOCAB,ASNT_F_VALOR,ASNT_NOMBRE,ASNT_DEBE,ASNT_HABER,ASNT_AJUSTAR,ASNT_DOCU,ASNT_DORE) values ('"
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
            SQL += "'?'," & Me.mDebe & "," & Me.mHbaber & ",'" & vAjuste & "','" & vDocu & "','" & vDore & "')"



            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mTextDebug.Text = "Grabando Registro  " & Mid(vAmpcpto, 1, 40)
            Me.mTextDebug.Update()

            If vCfcta_Cod.Length < 2 Then
                Me.mListBoxDebug.Items.Add("NEWSTOCK: " & "Cuenta Contable no válida para descripción de Movimiento =  " & Mid(vAmpcpto, 1, 40))
            End If


        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
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
    Private Function BuscaCuentaProveedorCuentasPorPagar(ByVal vProveedor As Integer) As String
        Try
            SQL = "SELECT NVL(FORN_CEX1,'?') "
            SQL += " FROM TNST_FORN "
            SQL += " WHERE FORN_CODI = " & vProveedor

            If IsNothing(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)) = False Then
                Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            Else
                Return "0"
            End If
        Catch ex As Exception
            Return "0"
        End Try
    End Function
    Private Function BuscaCuentaProveedorAlbaranes(ByVal vProveedor As Integer) As String
        Try
            SQL = "SELECT NVL(FORN_CEX2,'?') "
            SQL += " FROM TNST_FORN "
            SQL += " WHERE FORN_CODI = " & vProveedor

            If IsNothing(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)) = False Then
                Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            Else
                Return "0"
            End If
        Catch ex As Exception
            Return "0"
        End Try
    End Function
    Private Function BuscaCuentaCostoLiquido(ByVal vGrupo As Integer) As String
        Try
            SQL = "SELECT NVL(GRUP_CEXT,'?') "
            SQL += " FROM TNST_GRUP "
            SQL += " WHERE GRUP_CODI = " & vGrupo

            If IsNothing(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)) = False Then
                Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            Else
                Return "0"
            End If
        Catch ex As Exception
            Return "0"
        End Try

    End Function
    Private Function BuscaCuentaCostoLiquidoAlmacen(ByVal vAlmacen As Integer) As String
        Try
            SQL = "SELECT NVL(ALMA_COEX,'?') "
            SQL += " FROM TNST_ALMA "
            SQL += " WHERE ALMA_CODI = " & vAlmacen

            If IsNothing(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)) = False Then
                Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            Else
                Return "0"
            End If
        Catch ex As Exception
            Return "0"
        End Try

    End Function
    Private Function BuscaCuentaImpuestoVenta(ByVal vIva As Integer) As String

        Try
            SQL = "SELECT NVL(IVAS_CEX1,'?') "
            SQL += " FROM TNST_IVAS "
            SQL += " WHERE IVAS_CODI = " & vIva

            If IsNothing(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)) = False Then
                Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            Else
                Return "0"
            End If
        Catch ex As Exception
            Return "0"
        End Try
    End Function
    Private Function BuscaCuentaImpuestoDevolucion(ByVal vIva As Integer) As String
        Try
            SQL = "SELECT NVL(IVAS_CEX1,'?') "
            SQL += " FROM TNST_IVAS "
            SQL += " WHERE IVAS_CODI = " & vIva

            If IsNothing(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)) = False Then
                Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            Else
                Return "0"
            End If
        Catch ex As Exception
            Return "0"
        End Try
    End Function
    Private Function BuscaCuentaExistencia(ByVal vAlmacen As Integer, ByVal vGrupo As Integer) As String

        SQL = "SELECT TNST_CNTA.CNTA_CNTA "
        SQL += "FROM TNST_CUEX, TNST_CNTA "
        SQL += "WHERE TNST_CNTA.CNTA_CODI = TNST_CUEX.CUEX_CNTB "
        SQL += " AND TNST_CUEX.ALMA_CODI = " & vAlmacen
        SQL += " AND TNST_CUEX.GRUP_CODI = " & vGrupo

        If IsNothing(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)) = False Then
            Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
        Else
            Return "0"
        End If
    End Function
    Private Function BuscaCuentaExistenciaSantanaCazorla(ByVal vAlmacen As Integer, ByVal vGrupo As Integer) As String
        Dim Cuenta As String
        Dim CuentaAux As String

        SQL = "SELECT TNST_CNTA.CNTA_CNTA "
        SQL += "FROM TNST_CUCL, TNST_CNTA "
        SQL += "WHERE TNST_CUCL.CUCL_CNTB = TNST_CNTA.CNTA_CODI"
        SQL += " AND TNST_CUCL.ALMA_CODI = " & vAlmacen
        SQL += " AND TNST_CUCL.GRUP_CODI = " & vGrupo

        If IsNothing(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)) = False Then
            Cuenta = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
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


    Private Sub GestionGastosAnalitica(vTimoCodi As Integer, vtipoAsiento As Integer, vTipoDebeHaber As Integer, vTexto As String, vCuenta As String, vEnsa As String)
        Dim Total As Double
        Dim vCentroCosto As String
        Dim vCuentaAnalitica As String
        Dim vTipo As String
        Dim Texto As String = ""

        Dim Cuenta As String = ""



        ' MOvimientos del Tipo Recibido Agrupados por FECHA/ALMACEN
        SQL = "SELECT TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI, SUM(TNST_MOVD.MOVD_TOTA) + SUM(NVL(TNST_MOVD.MOVD_VERD,0)) AS TOTAL,ALMA_DESC AS ALMACEN,"
        SQL += " TNST_MOVD.ALMA_CODI AS ALMACODI ,NVL(ALMA_COEX,'?') AS CUENTA,TNST_MOVG.MOVG_ANUL,NVL(ALMA_CCST,'??') AS ALMA_CCST "
        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD,  TNST_TIMO"
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & vTimoCodi
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        If vEnsa.Length > 0 Then
            SQL += " AND TNST_MOVD.MOVD_ENSA = " & CInt(vEnsa)
        End If

        SQL += " GROUP BY TNST_MOVG.TIMO_CODI,TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,"
        SQL += "TNST_ALMA.ALMA_DESC,ALMA_COEX,TNST_MOVG.MOVG_ANUL,ALMA_CCST"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read


            Linea = Linea + 1
            ' Localiza el Almacen del Asiento Anterior 
            SQL = "SELECT ALMA_COEX AS CUENTA "
            SQL += " FROM TNST_ALMA "
            SQL += " WHERE "
            SQL += " ALMA_CODI = " & CInt(Me.DbLeeHotel.mDbLector("ALMACODI"))



            Me.mResult = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)



            ' MOvimientos del Tipo Recibido Agrupados por FECHA/ALMACEN/GRUPO/FAMILIA

            SQL2 = "SELECT TNST_MOVG.MOVG_DAVA,"
            SQL2 += "TNST_MOVD.ALMA_CODI,TNST_GRUP.GRUP_CODI, SUM(TNST_MOVD.MOVD_TOTA) + SUM(NVL(TNST_MOVD.MOVD_VERD,0)) AS TOTAL,ALMA_DESC AS ALMACEN,"
            SQL2 += " GRUP_DESC AS GRUPO,TNST_MOVD.ALMA_CODI AS ALMACODI,TNST_GRUP.GRUP_CODI AS GRUPCODI,TNST_FAMI.FAMI_CODI AS FAMICODI,FAMI_DESC AS FAMILIA,NVL(ALMA_CCST,'??') AS ALMA_CCST,NVL(GRUP_CCST,'??') AS GRUP_CCST,NVL(FAMI_CCST,'??') AS FAMI_CCST "
            SQL2 += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_GRUP, TNST_TIMO,TNST_FAMI "
            SQL2 += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
            SQL2 += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
            SQL2 += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
            SQL2 += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
            SQL2 += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
            SQL2 += " AND (TNST_PROD.GRUP_CODI = TNST_GRUP.GRUP_CODI)"
            SQL2 += " AND (TNST_PROD.FAMI_CODI = TNST_FAMI.FAMI_CODI)"
            SQL2 += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
            SQL2 += " AND TNST_TIMO.TIMO_TIPO = " & vTimoCodi
            SQL2 += " AND TNST_MOVD.ALMA_CODI =  " & CInt(Me.DbLeeHotel.mDbLector("ALMACODI"))
            SQL2 += " AND TNST_MOVG.MOVG_ANUL = 0"
            If vEnsa.Length > 0 Then
                SQL2 += " AND TNST_MOVD.MOVD_ENSA = " & CInt(vEnsa)
            End If
            SQL2 += " GROUP BY TNST_MOVG.TIMO_CODI,TNST_MOVG.MOVG_DAVA,"
            SQL2 += "TNST_MOVD.ALMA_CODI,"
            SQL2 += "TNST_ALMA.ALMA_DESC,"
            SQL2 += "TNST_GRUP.GRUP_CODI,"
            SQL2 += "TNST_GRUP.GRUP_DESC,"
            SQL2 += "TNST_FAMI.FAMI_CODI,"
            SQL2 += "TNST_FAMI.FAMI_DESC,ALMA_CCST,GRUP_CCST,FAMI_CCST "

            ' Si la cuenta esta completa (10 bytes ) se agrupa el ac y se detalla el AA
            If Me.mResult.Length = 10 Then

                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)

                If vTipoDebeHaber = mEnumTipoDebeHaber.Debe Then
                    vTipo = "D"
                    Me.mTipoAsiento = "DEBE"
                Else
                    vTipo = "H"
                    Me.mTipoAsiento = "HABER"
                End If

                If vCuenta = "" Then
                    Cuenta = CStr(Me.DbLeeHotel.mDbLector("CUENTA"))
                Else
                    Cuenta = vCuenta
                End If


                Me.InsertaOracle("AC", vtipoAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, vTipo, vTexto & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total, "NO", "0", "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, vTipo, vTexto & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total)



                Me.DbLeeHotelAux.TraerLector(SQL2)
                While Me.DbLeeHotelAux.mDbLector.Read

                    vCentroCosto = CStr(Me.DbLeeHotelAux.mDbLector("ALMA_CCST")) & CStr(Me.DbLeeHotelAux.mDbLector("GRUP_CCST")).PadLeft(2, "0") & CStr(Me.DbLeeHotelAux.mDbLector("FAMI_CCST")).PadLeft(2, "0")

                    SpyroCompruebaCentrosDeCosto(vCentroCosto, vTexto & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CStr(Me.DbLeeHotelAux.mDbLector("GRUPO")) & "/" & CStr(Me.DbLeeHotelAux.mDbLector("FAMILIA")))

                    Me.GeneraFileAA("AA", vtipoAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Cuenta, Me.mParaTipoAnalitica, vCentroCosto, CDbl(Me.DbLeeHotelAux.mDbLector("TOTAL")))

                End While
                Me.DbLeeHotelAux.mDbLector.Close()






            Else
                '  Si la cuenta No esta completa (< 10 bytes ) se detalla el ac y se detalla el AA
                Me.DbLeeHotelAux.TraerLector(SQL2)
                While Me.DbLeeHotelAux.mDbLector.Read

                    Linea = Linea + 1
                    Total = CDbl(Me.DbLeeHotelAux.mDbLector("TOTAL"))

                    If vTipoDebeHaber = mEnumTipoDebeHaber.Debe Then
                        vTipo = "D"
                        Me.mTipoAsiento = "DEBE"
                    Else
                        vTipo = "H"
                        Me.mTipoAsiento = "HABER"
                    End If



                    Texto = vTexto & CStr(Me.DbLeeHotelAux.mDbLector("GRUPO"))

                    vCentroCosto = CStr(Me.DbLeeHotelAux.mDbLector("ALMA_CCST")) & CStr(Me.DbLeeHotelAux.mDbLector("GRUP_CCST")).PadLeft(2, "0") & CStr(Me.DbLeeHotelAux.mDbLector("FAMI_CCST")).PadLeft(2, "0")
                    SpyroCompruebaCentrosDeCosto(vCentroCosto, vTexto & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CStr(Me.DbLeeHotelAux.mDbLector("GRUPO")) & "/" & CStr(Me.DbLeeHotelAux.mDbLector("FAMILIA")))


                    vCuentaAnalitica = CStr(Me.DbLeeHotel.mDbLector("CUENTA")) & CStr(Me.DbLeeHotelAux.mDbLector("GRUP_CCST")).PadLeft(2, "0") & CStr(Me.DbLeeHotelAux.mDbLector("FAMI_CCST")).PadLeft(2, "0")


                    If vCuenta = "" Then
                        Cuenta = vCuentaAnalitica
                    Else
                        Cuenta = vCuenta
                    End If

                    Me.InsertaOracle("AC", vtipoAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Cuenta, vTipo, Texto & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total, "NO", "0", "")
                    Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Cuenta, vTipo, Texto & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total)


                    Me.GeneraFileAA("AA", vtipoAsiento, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Cuenta, Me.mParaTipoAnalitica, vCentroCosto, CDbl(Me.DbLeeHotelAux.mDbLector("TOTAL")))



                End While
                Me.DbLeeHotelAux.mDbLector.Close()
            End If




        End While
        Me.DbLeeHotel.mDbLector.Close()




    End Sub
#Region "ASIENTO-1 ALBARANES"
    Private Sub TotalPendienteFormalizar()
        Dim Total As Double

        SQL = "SELECT SUM (TNST_MOVG.MOVG_VATO) "
        SQL += "FROM TNST_MOVG, TNST_TIMO "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += "AND TIMO_TIPO = " & Me.mTimo_Albaran
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"



        If IsNumeric(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = True Then
            Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)

        Else
            Total = 0
        End If
        Linea = 1
        Me.mTipoAsiento = "HABER"
        Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaFormalizaAlbaranes, Me.mIndicadorHaber, "ALBARANES PENDIENTE DE FORMALIZAR", Total, "SI", "", "")
        Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaFormalizaAlbaranes, Me.mIndicadorHaber, "ALBARANES PENDIENTE DE FORMALIZAR", Total)
    End Sub
    Private Sub TotalPendienteFormalizarProveedor()
        Dim Total As Double

        SQL = "SELECT MOVG_DAVA,MOVG_CODI,MOVG_ORIG,SUM(TNST_MOVG.MOVG_VATO)AS TOTAL,FORN_DESC AS PROVEEDOR ,NVL(MOVG_IDDO,'  ')AS DOCU,TNST_FORN.FORN_CODI AS CODI "
        SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_FORN "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += "AND (TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI OR TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI)"
        SQL += "AND TIMO_TIPO = " & Me.mTimo_Albaran
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " GROUP BY MOVG_DAVA,MOVG_CODI,MOVG_ORIG,MOVG_IDDO,TNST_FORN.FORN_CODI,FORN_DESC"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedorAlbaranes(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), Me.mIndicadorHaber, CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaProveedorAlbaranes(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), Me.mIndicadorHaber, "ALBARAN Num:  " & CType(Me.DbLeeHotel.mDbLector("DOCU"), String), Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub GastosPorCentrodeCostoAlbaranes()
        Dim Total As Double
        Dim vCentroCosto As String

        SQL = "SELECT TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,TNST_GRUP.GRUP_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,"
        SQL += " GRUP_DESC AS GRUPO,TNST_MOVD.ALMA_CODI AS ALMACODI,TNST_GRUP.GRUP_CODI AS GRUPCODI"
        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_GRUP, TNST_TIMO"
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " AND (TNST_PROD.GRUP_CODI = TNST_GRUP.GRUP_CODI)"
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Albaran
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " GROUP BY TNST_MOVG.TIMO_CODI,TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,"
        SQL += "TNST_GRUP.GRUP_CODI,"
        SQL += "TNST_ALMA.ALMA_DESC,"
        SQL += "TNST_GRUP.GRUP_DESC"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorDebe, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("GRUPO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total, "NO", "0", "")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorDebe, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("GRUPO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total)
            Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), "0", vCentroCosto, Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub GastosPorCentrodeCostoAlbaranesAlmacen()
        Dim Total As Double
        Dim vCentroCosto As String

        SQL = "SELECT TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,"
        SQL += " TNST_MOVD.ALMA_CODI AS ALMACODI "
        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD,  TNST_TIMO"
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Albaran
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " GROUP BY TNST_MOVG.TIMO_CODI,TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,"
        SQL += "TNST_ALMA.ALMA_DESC"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorDebe, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total, "NO", "0", "")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorDebe, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total)
            Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), "0", vCentroCosto, Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
#End Region
#Region "ASIENTO-2 TRASPASOS INTERNOS"
    Private Sub TraspasosSalidas()
        Dim Total As Double
        Dim vCentroCosto As String

        SQL = "SELECT   TNST_MOVG.MOVG_CODI,TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, NVL(TNST_MOVG.MOVG_IDDO,' ')AS DOCU,"
        SQL += "TNST_MOVD.ALMA_CODI,TNST_GRUP.GRUP_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,GRUP_DESC AS GRUPO,TNST_MOVD.MOVD_ENSA,TNST_MOVD.ALMA_CODI AS ALMACODI,TNST_GRUP.GRUP_CODI AS GRUPCODI"
        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_GRUP, TNST_TIMO "
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " AND (TNST_PROD.GRUP_CODI = TNST_GRUP.GRUP_CODI)"
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Traspaso
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " AND TNST_MOVD.MOVD_ENSA = " & -1
        SQL += " GROUP BY TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,"
        SQL += "TNST_GRUP.GRUP_CODI,"
        SQL += "TNST_MOVG.MOVG_IDDO,"
        SQL += "TNST_TIMO.TIMO_TIPO,"
        SQL += "TNST_ALMA.ALMA_DESC,"
        SQL += "TNST_GRUP.GRUP_DESC,"
        SQL += "TNST_MOVD.MOVD_ENSA"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorHaber, CType("SALIDA TRASPASO ", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("GRUPO"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorHaber, CType("SALIDA TRASPASO ", String), Total)
            Me.GeneraFileAA("AA", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), "0", vCentroCosto, Total)


        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub TraspasosSalidasAlmacen()
        Dim Total As Double
        Dim vCentroCosto As String

        SQL = "SELECT   TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, "
        SQL += "TNST_MOVD.ALMA_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,TNST_MOVD.MOVD_ENSA,TNST_MOVD.ALMA_CODI AS ALMACODI "
        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD,  TNST_TIMO "
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Traspaso
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " AND TNST_MOVD.MOVD_ENSA = " & -1
        SQL += " GROUP BY TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,"
        SQL += "TNST_TIMO.TIMO_TIPO,"
        SQL += "TNST_ALMA.ALMA_DESC,"
        SQL += "TNST_MOVD.MOVD_ENSA"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorHaber, CType("SALIDA TRASPASO ", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total, "NO", "", "")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorHaber, CType("SALIDA TRASPASO ", String), Total)
            Me.GeneraFileAA("AA", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), "0", vCentroCosto, Total)


        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub TraspasosEntradas()

        Try
            Dim Total As Double
            Dim vCentroCosto As String

            SQL = "SELECT   TNST_MOVG.MOVG_CODI,TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, NVL(TNST_MOVG.MOVG_IDDO,' ')AS DOCU,"
            SQL += "TNST_MOVD.ALMA_CODI,TNST_GRUP.GRUP_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,GRUP_DESC AS GRUPO,TNST_MOVD.MOVD_ENSA,TNST_MOVD.ALMA_CODI AS ALMACODI,TNST_GRUP.GRUP_CODI AS GRUPCODI"
            SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_GRUP, TNST_TIMO "
            SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
            SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
            SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
            SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
            SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
            SQL += " AND (TNST_PROD.GRUP_CODI = TNST_GRUP.GRUP_CODI)"
            SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Traspaso
            SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
            SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
            SQL += " AND TNST_MOVD.MOVD_ENSA = " & 1
            SQL += " GROUP BY TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_DAVA,"
            SQL += "TNST_MOVD.ALMA_CODI,"
            SQL += "TNST_GRUP.GRUP_CODI,"
            SQL += "TNST_MOVG.MOVG_IDDO,"
            SQL += "TNST_TIMO.TIMO_TIPO,"
            SQL += "TNST_ALMA.ALMA_DESC,"
            SQL += "TNST_GRUP.GRUP_DESC,"
            SQL += "TNST_MOVD.MOVD_ENSA"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
                vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorDebe, CType("ENTRADA TRASPASO ", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("GRUPO"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorDebe, CType("ENTRADA TRASPASO ", String), Total)
                Me.GeneraFileAA("AA", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), "0", vCentroCosto, Total)


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Traspasos Entradas")
        End Try

    End Sub
    Private Sub TraspasosEntradasAlmacen()

        Try
            Dim Total As Double
            Dim vCentroCosto As String

            SQL = "SELECT   TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, "
            SQL += "TNST_MOVD.ALMA_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,TNST_MOVD.MOVD_ENSA,TNST_MOVD.ALMA_CODI AS ALMACODI"
            SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_TIMO "
            SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
            SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
            SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
            SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
            SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
            SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Traspaso
            SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
            SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
            SQL += " AND TNST_MOVD.MOVD_ENSA = " & 1
            SQL += " GROUP BY TNST_MOVG.MOVG_DAVA,"
            SQL += "TNST_MOVD.ALMA_CODI,"
            SQL += "TNST_TIMO.TIMO_TIPO,"
            SQL += "TNST_ALMA.ALMA_DESC,"
            SQL += "TNST_MOVD.MOVD_ENSA"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read

                SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
                vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorDebe, CType("ENTRADA TRASPASO ", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total, "NO", "", "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorDebe, CType("ENTRADA TRASPASO ", String), Total)
                Me.GeneraFileAA("AA", 2, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), "0", vCentroCosto, Total)


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Traspasos Entradas")
        End Try

    End Sub
#End Region
#Region "ASIENTO-3 FACTURAS DIRECTAS"
    Private Sub TotalFacturasProveedor()
        Dim Total As Double
        Dim TotalBase As Double



        SQL = "SELECT MOVG_DAVA,MOVG_CODI AS NMOVI,MOVG_ANCI,MOVG_ORIG,SUM(TNST_MOVG.MOVG_VATO)AS TOTAL,FORN_DESC AS PROVEEDOR ,TNST_FORN.FORN_CODI AS CODI,"
        SQL += "NVL(TNST_MOVG.MOVG_IDDO,' ')AS DOCU,'0' AS BASE,NVL(FORN_CNTR,'0') AS CIF "
        SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_FORN  "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += "AND (TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI OR TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI) "
        SQL += "AND TIMO_TIPO = " & Me.mTimo_Factura_Directa
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " GROUP BY MOVG_DAVA,MOVG_CODI,MOVG_ANCI,MOVG_ORIG,TNST_FORN.FORN_CODI,FORN_DESC,TNST_MOVG.MOVG_IDDO,FORN_CNTR"




        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read




            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalBase = Me.CalculaBaseImponible(CType(Me.DbLeeHotel.mDbLector("NMOVI"), Integer), CType(Me.DbLeeHotel.mDbLector("MOVG_ANCI"), Integer))
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedorCuentasPorPagar(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), Me.mIndicadorHaberFac, CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "")
            Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaProveedorCuentasPorPagar(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), Me.mIndicadorHaberFac, "FACTURA Num:  " & CType(Me.DbLeeHotel.mDbLector("DOCU"), String), Total, Me.mParaSeriefacturas & Me.mFecha.Year, CType(Me.DbLeeHotel.mDbLector("NMOVI"), String))
            Me.GeneraFileFV("FC", 3, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSeriefacturas & Me.mFecha.Year, CType(Me.DbLeeHotel.mDbLector("NMOVI"), String), Total, CType(Me.DbLeeHotel.mDbLector("DOCU"), String).PadRight(15, CChar(" ")), Me.BuscaCuentaProveedorCuentasPorPagar(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("MOVG_DAVA"), Date))
        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub GastosPorCentrodeCostoFacturas()
        Dim Total As Double
        Dim vCentroCosto As String
        Try

            SQL = "SELECT TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, "
            SQL += "TNST_MOVD.ALMA_CODI,TNST_GRUP.GRUP_CODI, SUM(TNST_MOVD.MOVD_TOTA-TNST_MOVD.MOVD_IVAS)AS TOTAL,ALMA_DESC AS ALMACEN,GRUP_DESC AS GRUPO,TNST_MOVD.ALMA_CODI AS ALMACODI,TNST_GRUP.GRUP_CODI AS GRUPCODI"
            SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_GRUP, TNST_TIMO"
            SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
            SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
            SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
            SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
            SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
            SQL += " AND (TNST_PROD.GRUP_CODI = TNST_GRUP.GRUP_CODI)"
            SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Factura_Directa
            SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
            SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
            SQL += " GROUP BY  TNST_MOVG.TIMO_CODI,TNST_MOVG.MOVG_DAVA,"
            SQL += "TNST_MOVD.ALMA_CODI,"
            SQL += "TNST_GRUP.GRUP_CODI,"
            SQL += "TNST_TIMO.TIMO_TIPO,"
            SQL += "TNST_ALMA.ALMA_DESC,"
            SQL += "TNST_GRUP.GRUP_DESC"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
                vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorDebe, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("GRUPO"), String), Total, "NO", "0", "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorDebe, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("GRUPO"), String), Total)
                Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), "0", vCentroCosto, Total)



            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub GastosPorCentrodeCostoFacturasAlmacen()
        Dim Total As Double
        Dim vCentroCosto As String
        Try

            SQL = "SELECT TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, "
            SQL += "TNST_MOVD.ALMA_CODI, SUM(TNST_MOVD.MOVD_TOTA-TNST_MOVD.MOVD_IVAS)AS TOTAL,ALMA_DESC AS ALMACEN,TNST_MOVD.ALMA_CODI AS ALMACODI "
            SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_TIMO"
            SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
            SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
            SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
            SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
            SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
            SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Factura_Directa
            SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
            SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
            SQL += " GROUP BY  TNST_MOVG.TIMO_CODI,TNST_MOVG.MOVG_DAVA,"
            SQL += "TNST_MOVD.ALMA_CODI,"
            SQL += "TNST_TIMO.TIMO_TIPO,"
            SQL += "TNST_ALMA.ALMA_DESC"

            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
                vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorDebe, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total, "NO", "0", "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorDebe, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total)
                Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), "0", vCentroCosto, Total)



            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub TotalFacturasProveedorImpuesto()
        Dim Total As Double
        Dim TotalBase As Double
        Dim ValorImpuesto As Double
        Dim TipoImpuesto As String
        Dim CodigoImpuesto As String


        SQL = "SELECT TNST_MOVG.MOVG_CODI AS NMOVI,MOVG_ORIG,SUM(TNST_MOVI.MOVI_IMPU)AS TOTAL ,SUM(TNST_MOVI.MOVI_NETO)AS BASE,NVL(MOVG_IDDO,' ')AS DOCU ,MOVI_TAXA AS TIPO "
        SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_MOVI "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Factura_Directa
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " AND TNST_MOVG.MOVG_CODI = TNST_MOVI.MOVG_CODI "
        SQL += " AND TNST_MOVG.MOVG_ANCI = TNST_MOVI.MOVG_ANCI "

        SQL += " GROUP BY TNST_MOVG.MOVG_CODI,MOVG_ORIG,MOVG_IDDO,MOVI_TAXA"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            SQL = "SELECT IVAS_TAXA FROM TNST_IVAS WHERE IVAS_CODI = " & CType(Me.DbLeeHotel.mDbLector("TIPO"), Double)
            ValorImpuesto = CType(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL), Double)




            SQL = "SELECT NVL(IMPU_TIPO,'0') FROM TS_IMPU WHERE IMPU_VALO = " & ValorImpuesto
            CodigoImpuesto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(CFIVATIP_COD,'0') FROM CFIVAPOR WHERE PIMPUESTO = " & ValorImpuesto
            SQL += "  AND CFIVATIMPU_COD ='" & CodigoImpuesto & "'"
            TipoImpuesto = Me.DbSpyro.EjecutaSqlScalar(SQL)



            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            Me.mTipoAsiento = "DEBE"
            If Total <> 0 Then
                Me.InsertaOracle("AC", 3, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaImpuestoVenta(CType(Me.DbLeeHotel.mDbLector("TIPO"), Integer)), Me.mIndicadorDebe, CType("IMPUESTO FACTURA", String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaImpuestoVenta(CType(Me.DbLeeHotel.mDbLector("TIPO"), Integer)), Me.mIndicadorDebe, CType("IMPUESTO FACTURA", String) & " " & CType(Me.DbLeeHotel.mDbLector("DOCU"), String), Total)
                Me.GeneraFileIV("IC", 3, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSeriefacturas & Me.mFecha.Year, CType(Me.DbLeeHotel.mDbLector("NMOVI"), String), TotalBase, ValorImpuesto, Total, TipoImpuesto, CodigoImpuesto)
            End If

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
#End Region
#Region "ASIENTO-4 DEVOLUCION ALBARANES"
    Private Sub TotalPendienteFormalizarDevolucionAlbaran()
        Dim Total As Double

        SQL = "SELECT SUM (TNST_MOVG.MOVG_VATO) "
        SQL += "FROM TNST_MOVG, TNST_TIMO "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += "AND TIMO_TIPO = " & Me.mTimo_Albaran_Dev
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"

        If IsNumeric(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = True Then
            Total = CType(Me.DbLeeHotel.EjecutaSqlScalar(SQL), Double)

        Else
            Total = 0
        End If
        Linea = 1
        Me.mTipoAsiento = "DEBE"
        Me.InsertaOracle("AC", 4, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaFormalizaAlbaranes, Me.mIndicadorDebe, " DEVO. ALBARANES PENDIENTE DE FORMALIZAR", Total, "SI", "", "")
        Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaFormalizaAlbaranes, Me.mIndicadorDebe, "DEVO. ALBARANES PENDIENTE DE FORMALIZAR", Total)
    End Sub
    Private Sub TotalPendienteFormalizarProveedorDevolucionAlbaran()
        Dim Total As Double

        SQL = "SELECT MOVG_CODI,MOVG_ORIG,SUM(TNST_MOVG.MOVG_VATO)AS TOTAL,FORN_DESC AS PROVEEDOR ,NVL(MOVG_IDDO,'  ')AS DOCU,NVL(MOVG_DORE,' ')AS DORE ,TNST_FORN.FORN_CODI AS CODI "
        SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_FORN "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += "AND (TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI OR TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI)"
        SQL += "AND TIMO_TIPO = " & Me.mTimo_Albaran_Dev
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " GROUP BY MOVG_CODI,MOVG_ORIG,MOVG_IDDO,MOVG_DORE,TNST_FORN.FORN_CODI,FORN_DESC"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 4, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedorAlbaranes(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), Me.mIndicadorDebe, CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), CType(Me.DbLeeHotel.mDbLector("DORE"), String))
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaProveedorAlbaranes(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), Me.mIndicadorDebe, "ALBARAN Num:  " & CType(Me.DbLeeHotel.mDbLector("DOCU"), String), Total)

        End While
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

            SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 4, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorHaber, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("GRUPO"), String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total, "NO", "0", "0")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorHaber, CType("GASTOS", String), Total)
            Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 4, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), "0", vCentroCosto, Total)



        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub GastosPorCentrodeCostoAlbaranesDevolucionAlbaranAlmacen()
        Dim Total As Double
        Dim vCentroCosto As String

        SQL = "SELECT TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,TNST_MOVD.ALMA_CODI AS ALMACODI "
        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD,  TNST_TIMO"
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Albaran_Dev
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " GROUP BY  TNST_MOVG.TIMO_CODI,TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,"
        SQL += "TNST_ALMA.ALMA_DESC"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 4, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorHaber, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total, "NO", "0", "0")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorHaber, CType("GASTOS", String), Total)
            Me.GeneraFileAA("AA", 1, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 4, Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), "0", vCentroCosto, Total)



        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
#End Region
#Region "ASIENTO-5 DEVOLUCION DE FACTURAS"
    Private Sub TotalFacturasProveedorDevolucion()
        Dim Total As Double
        Dim Totalbase As Double

        SQL = "SELECT MOVG_DAVA,MOVG_CODI AS NMOVI,MOVG_ANCI,MOVG_DEST,SUM(TNST_MOVG.MOVG_VATO)AS TOTAL,FORN_DESC AS PROVEEDOR,TNST_FORN.FORN_CODI AS CODI, "
        SQL += "NVL(TNST_MOVG.MOVG_IDDO,' ')AS DOCU,'0' AS BASE,NVL(FORN_CNTR,'0') AS CIF "
        SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_FORN "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += "AND (TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI OR TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI)"
        SQL += "AND TIMO_TIPO = " & Me.mTimo_Factura_Directa_Dev
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        ' SQL += " GROUP BY MOVG_CODI,MOVG_ANCI,MOVG_DEST,TNST_FORN.FORN_CODI,FORN_DESC"
        SQL += " GROUP BY  MOVG_DAVA,MOVG_CODI,MOVG_ANCI,MOVG_DEST,TNST_FORN.FORN_CODI,FORN_DESC,TNST_MOVG.MOVG_IDDO,FORN_CNTR"


        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            'SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
            'vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Totalbase = Me.CalculaBaseImponible(CType(Me.DbLeeHotel.mDbLector("NMOVI"), Integer), CType(Me.DbLeeHotel.mDbLector("MOVG_ANCI"), Integer))

            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedorCuentasPorPagar(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), Me.mIndicadorDebeFac, CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String), Total, "NO", "", "")
            Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaProveedorCuentasPorPagar(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), Me.mIndicadorDebeFac, "FACTURA Num:  " & CType(Me.DbLeeHotel.mDbLector("DOCU"), String), Total, Me.mParaSeriefacturas & Me.mFecha.Year, CType(Me.DbLeeHotel.mDbLector("NMOVI"), String))
            Me.GeneraFileFV("FC", 5, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSeriefacturas & Me.mFecha.Year, CType(Me.DbLeeHotel.mDbLector("NMOVI"), String), Total * -1, CType(Me.DbLeeHotel.mDbLector("DOCU"), String).PadRight(15, CChar(" ")), Me.BuscaCuentaProveedorCuentasPorPagar(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("MOVG_DAVA"), Date))

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub GastosPorCentrodeCostoFacturasDevolucion()
        Dim Total As Double
        Dim vCentroCosto As String

        SQL = "SELECT TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, "
        SQL += "TNST_MOVD.ALMA_CODI,TNST_GRUP.GRUP_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,GRUP_DESC AS GRUPO,TNST_MOVD.ALMA_CODI AS ALMACODI,TNST_GRUP.GRUP_CODI AS GRUPCODI"
        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_GRUP, TNST_TIMO"
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " AND (TNST_PROD.GRUP_CODI = TNST_GRUP.GRUP_CODI)"
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Factura_Directa_Dev
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " GROUP BY  TNST_MOVG.TIMO_CODI,TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,"
        SQL += "TNST_GRUP.GRUP_CODI,"
        SQL += "TNST_TIMO.TIMO_TIPO,"
        SQL += "TNST_ALMA.ALMA_DESC,"
        SQL += "TNST_GRUP.GRUP_DESC"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorHaber, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("GRUPO"), String), Total, "NO", "0", "")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorHaber, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("GRUPO"), String), Total)
            Me.GeneraFileAA("AA", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), "0", vCentroCosto, Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub GastosPorCentrodeCostoFacturasDevolucionAlmacen()
        Dim Total As Double
        Dim vCentroCosto As String

        SQL = "SELECT TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, "
        SQL += "TNST_MOVD.ALMA_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,TNST_MOVD.ALMA_CODI AS ALMACODI "
        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_TIMO"
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Factura_Directa_Dev
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " GROUP BY  TNST_MOVG.TIMO_CODI,TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,"
        SQL += "TNST_TIMO.TIMO_TIPO,"
        SQL += "TNST_ALMA.ALMA_DESC"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorHaber, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total, "NO", "0", "")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorHaber, CType("GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total)
            Me.GeneraFileAA("AA", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), "0", vCentroCosto, Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub

    Private Sub TotalFacturasProveedorImpuestoDevolucion()
        Dim Total As Double
        Dim TotalBase As Double
        Dim ValorImpuesto As Double
        Dim TipoImpuesto As String
        Dim CodigoImpuesto As String


        SQL = "SELECT TNST_MOVG.MOVG_CODI AS NMOVI,MOVG_ORIG,SUM(TNST_MOVI.MOVI_IMPU)AS TOTAL ,SUM(TNST_MOVI.MOVI_NETO)AS BASE,NVL(MOVG_IDDO,' ')AS DOCU ,MOVI_TAXA AS TIPO "
        SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_MOVI "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Factura_Directa_Dev
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " AND TNST_MOVG.MOVG_CODI = TNST_MOVI.MOVG_CODI "
        SQL += " AND TNST_MOVG.MOVG_ANCI = TNST_MOVI.MOVG_ANCI "
        SQL += " GROUP BY TNST_MOVG.MOVG_CODI,MOVG_ORIG,MOVG_IDDO,MOVI_TAXA"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            SQL = "SELECT IVAS_TAXA FROM TNST_IVAS WHERE IVAS_CODI = " & CType(Me.DbLeeHotel.mDbLector("TIPO"), Double)
            ValorImpuesto = CType(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL), Double)



            SQL = "SELECT NVL(IMPU_TIPO,'0') FROM TS_IMPU WHERE IMPU_VALO = " & ValorImpuesto
            CodigoImpuesto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(CFIVATIP_COD,'0') FROM CFIVAPOR WHERE PIMPUESTO = " & ValorImpuesto
            SQL += "  AND CFIVATIMPU_COD ='" & CodigoImpuesto & "'"
            TipoImpuesto = Me.DbSpyro.EjecutaSqlScalar(SQL)




            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            Me.mTipoAsiento = "HABER"
            If Total <> 0 Then
                Me.InsertaOracle("AC", 5, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaImpuestoVenta(CType(Me.DbLeeHotel.mDbLector("TIPO"), Integer)), Me.mIndicadorHaber, CType("IMPUESTO FACTURA", String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaImpuestoVenta(CType(Me.DbLeeHotel.mDbLector("TIPO"), Integer)), Me.mIndicadorHaber, CType("IMPUESTO FACTURA", String) & " " & CType(Me.DbLeeHotel.mDbLector("DOCU"), String), Total)
                Me.GeneraFileIV("IC", 5, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSeriefacturas & Me.mFecha.Year, CType(Me.DbLeeHotel.mDbLector("NMOVI"), String), TotalBase * -1, ValorImpuesto, Total * -1, TipoImpuesto, CodigoImpuesto)
            End If

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
#End Region
#Region "ASIENTO-6 FACTURAS ALBARANES FORMALIZADAS"
    Private Sub TotalFacturasProveedorFormalizadas()
        Dim Total As Double
        Dim TotalBase As Double

        SQL = "SELECT MOVG_DAVA,TNST_MOVG.MOVG_CODI AS NMOVI,TNST_MOVG.MOVG_ANCI,MOVG_ORIG,MAX(TNST_MOVG.MOVG_VATO)AS TOTAL,FORN_DESC AS PROVEEDOR,"
        SQL += "TNST_MOVG.MOVG_IDDO AS DOCU,TNST_FORN.FORN_CODI AS CODI,SUM(MOVI_NETO) AS BASE,NVL(FORN_CNTR,'0') AS CIF "
        SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_FORN,TNST_MOVI "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += "AND (TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI OR TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI)"
        SQL += "AND TIMO_TIPO = " & Me.mTimo_Factura_Al
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " AND TNST_MOVG.MOVG_CODI = TNST_MOVI.MOVG_CODI "
        SQL += " AND TNST_MOVG.MOVG_ANCI = TNST_MOVI.MOVG_ANCI "
        SQL += " AND TNST_MOVG.MOVG_IDDO <> 'FV6/7409'"

        SQL += " GROUP BY MOVG_DAVA,TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_ANCI,MOVG_ORIG,TNST_FORN.FORN_CODI,FORN_DESC,TNST_MOVG.MOVG_IDDO,FORN_CNTR "

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            'SQL = "SELECT NVL(SERV_COMS,'0') FROM TNHT_SERV WHERE SERV_CODI = '" & CType(Me.DbLeeHotel.mDbLector("SERVICIO"), String) & "'"
            'vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)

            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedorCuentasPorPagar(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), Me.mIndicadorHaberFac, CType("FACTURA", String) & " " & CType(Me.DbLeeHotel.mDbLector("PROVEEDOR"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "")
            Me.GeneraFileAC2("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaProveedorCuentasPorPagar(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), Me.mIndicadorHaberFac, "FACTURA Num:  " & CType(Me.DbLeeHotel.mDbLector("DOCU"), String), Total, Me.mParaSeriefacturas & Me.mFecha.Year, CType(Me.DbLeeHotel.mDbLector("NMOVI"), String))
            Me.GeneraFileFV("FC", 6, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSeriefacturas & Me.mFecha.Year, CType(Me.DbLeeHotel.mDbLector("NMOVI"), String), Total, CType(Me.DbLeeHotel.mDbLector("DOCU"), String).PadRight(15, CChar(" ")), Me.BuscaCuentaProveedorCuentasPorPagar(CType(Me.DbLeeHotel.mDbLector("CODI"), Integer)), CType(Me.DbLeeHotel.mDbLector("CIF"), String), CType(Me.DbLeeHotel.mDbLector("MOVG_DAVA"), Date))

            Me.TotalAlbaranesProveedorFormalizados(CType(Me.DbLeeHotel.mDbLector("NMOVI"), Integer), CType(Me.DbLeeHotel.mDbLector("MOVG_ANCI"), Integer))
        End While
        Me.DbLeeHotel.mDbLector.Close()
    End Sub
    Private Sub TotalFacturasProveedorImpuestoFormalizadas()
        Dim Total As Double
        Dim TotalBase As Double
        Dim ValorImpuesto As Double
        Dim TipoImpuesto As String
        Dim CodigoImpuesto As String

        SQL = "SELECT TNST_MOVG.MOVG_CODI AS NMOVI,MOVG_ORIG,SUM(TNST_MOVI.MOVI_IMPU)AS TOTAL ,SUM(TNST_MOVI.MOVI_NETO)AS BASE,NVL(MOVG_IDDO,' ')AS DOCU,MOVI_TAXA AS TIPO "
        SQL += "FROM TNST_MOVG, TNST_TIMO,TNST_MOVI "
        SQL += "WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
        SQL += "AND TIMO_TIPO = " & Me.mTimo_Factura_Al
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " AND TNST_MOVG.MOVG_CODI = TNST_MOVI.MOVG_CODI "
        SQL += " AND TNST_MOVG.MOVG_ANCI = TNST_MOVI.MOVG_ANCI "
        SQL += " AND TNST_MOVG.MOVG_IDDO <> 'FV6/7409'"

        SQL += " GROUP BY TNST_MOVG.MOVG_CODI,MOVG_ORIG,MOVG_IDDO,MOVI_TAXA"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read
            SQL = "SELECT IVAS_TAXA FROM TNST_IVAS WHERE IVAS_CODI = " & CType(Me.DbLeeHotel.mDbLector("TIPO"), Double)
            ValorImpuesto = CType(Me.DbLeeHotelAux.EjecutaSqlScalar(SQL), Double)


            SQL = "SELECT NVL(IMPU_TIPO,'0') FROM TS_IMPU WHERE IMPU_VALO = " & ValorImpuesto
            CodigoImpuesto = Me.DbLeeCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(CFIVATIP_COD,'0') FROM CFIVAPOR WHERE PIMPUESTO = " & ValorImpuesto
            SQL += "  AND CFIVATIMPU_COD ='" & CodigoImpuesto & "'"

            TipoImpuesto = Me.DbSpyro.EjecutaSqlScalar(SQL)

            If IsNothing(TipoImpuesto) = True Then
                TipoImpuesto = "?"
                MsgBox("No se Localiza Equivalente de Impuesto en SpyroCFIVAPOR " & vbCrLf & vbCrLf & SQL, MsgBoxStyle.Information, "Atención")
            End If


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            TotalBase = CType(Me.DbLeeHotel.mDbLector("BASE"), Double)
            Me.mTipoAsiento = "DEBE"
            If Total <> 0 Then
                Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaImpuestoVenta(CType(Me.DbLeeHotel.mDbLector("TIPO"), Integer)), Me.mIndicadorDebe, CType("IMPUESTO FACTURA", String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaImpuestoVenta(CType(Me.DbLeeHotel.mDbLector("TIPO"), Integer)), Me.mIndicadorDebe, CType("IMPUESTO FACTURA", String) & " " & CType(Me.DbLeeHotel.mDbLector("DOCU"), String), Total)
            End If
            Me.GeneraFileIV("IC", 6, Me.mEmpGrupoCod, Me.mEmpCod, Me.mParaSeriefacturas & Me.mFecha.Year, CType(Me.DbLeeHotel.mDbLector("NMOVI"), String), TotalBase, ValorImpuesto, Total, TipoImpuesto, CodigoImpuesto)

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub TotalAlbaranesProveedorFormalizados(ByVal vCodi As Integer, ByVal vAnci As Integer)
        Dim Total As Double

        SQL = "SELECT TNST_MOVG.MOVG_CODI,QWE_VNST_GUIA.TIPO AS TIPO,SUM (QWE_VNST_GUIA.MOVG_VATO)AS TOTAL, QWE_VNST_GUIA.MOVG_IDDO AS ALBARAN, TNST_MOVG.MOVG_ORIG,TNST_FORN.FORN_CODI AS CODI,"
        SQL += "TNST_FORN.FORN_DESC AS PROVEEDOR "
        SQL += "FROM TNST_MOVG, QWE_VNST_GUIA, TNST_TIMO, TNST_FORN "
        SQL += "WHERE (TNST_MOVG.MOVG_CODI = QWE_VNST_GUIA.DORE_CODI) "
        SQL += "AND (TNST_MOVG.MOVG_ANCI = QWE_VNST_GUIA.DORE_ANCI) "
        SQL += "AND QWE_VNST_GUIA.DORE_CODI = " & vCodi
        SQL += "AND QWE_VNST_GUIA.DORE_ANCI = " & vAnci

        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI) "
        SQL += "AND (TNST_MOVG.MOVG_ORIG = TNST_FORN.FORN_CODI OR TNST_MOVG.MOVG_DEST = TNST_FORN.FORN_CODI) "

        SQL += "AND (QWE_VNST_GUIA.TIPO = " & Me.mTimo_Albaran
        SQL += "OR QWE_VNST_GUIA.TIPO= " & Me.mTimo_Albaran_Dev
        SQL += ") AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " AND TNST_MOVG.MOVG_IDDO <> 'FV6/7409'"

        SQL += "GROUP BY  TNST_MOVG.MOVG_CODI,QWE_VNST_GUIA.TIPO,QWE_VNST_GUIA.MOVG_IDDO, TNST_MOVG.MOVG_ORIG, TNST_FORN.FORN_CODI,TNST_FORN.FORN_DESC"



        Me.DbLeeHotelAux2.TraerLector(SQL)

        While Me.DbLeeHotelAux2.mDbLector.Read

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotelAux2.mDbLector("TOTAL"), Double)

            If CType(Me.DbLeeHotelAux2.mDbLector("TIPO"), Integer) = Me.mTimo_Albaran Then
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedorAlbaranes(CType(Me.DbLeeHotelAux2.mDbLector("CODI"), Integer)), Me.mIndicadorDebe, CType(" -FORMALIZADO ", String) & " " & CType(Me.DbLeeHotelAux2.mDbLector("PROVEEDOR"), String).Replace("'", "''"), Total, "NO", CType(Me.DbLeeHotelAux2.mDbLector("ALBARAN"), String), "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaProveedorAlbaranes(CType(Me.DbLeeHotelAux2.mDbLector("CODI"), Integer)), Me.mIndicadorDebe, CType("FORMALIZANDO", String) & " " & CType(Me.DbLeeHotelAux2.mDbLector("ALBARAN"), String), Total)
            End If
            If CType(Me.DbLeeHotelAux2.mDbLector("TIPO"), Integer) = Me.mTimo_Albaran_Dev Then
                Me.mTipoAsiento = "HABER"
                Me.InsertaOracle("AC", 6, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaProveedorAlbaranes(CType(Me.DbLeeHotelAux2.mDbLector("CODI"), Integer)), Me.mIndicadorHaber, CType(" -FORMALIZADO ", String) & " " & CType(Me.DbLeeHotelAux2.mDbLector("PROVEEDOR"), String).Replace("'", "''"), Total, "NO", CType(Me.DbLeeHotelAux2.mDbLector("ALBARAN"), String), "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaProveedorAlbaranes(CType(Me.DbLeeHotelAux2.mDbLector("CODI"), Integer)), Me.mIndicadorHaber, CType("FORMALIZANDO", String) & " " & CType(Me.DbLeeHotelAux2.mDbLector("ALBARAN"), String), Total)
            End If

        End While
        Me.DbLeeHotelAux2.mDbLector.Close()
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
            SQL += " AND TNST_MOVG.MOVG_IDDO <> 'FV6/7409'"

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
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaProveedorAlbaranes(CType(Me.DbLeeHotelAux2.mDbLector("CODI"), Integer)), Me.mIndicadorDebe, CType("FORMALIZADO", String), Total)

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
        SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacen
        vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


        Linea = Linea + 1
        Me.mTipoAsiento = "DEBE"
        Me.InsertaOracleInventarios("AC", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), 1, Linea, Me.BuscaCuentaExistencia(vAlmacen, vGrupo), Me.mIndicadorDebe, UltimoAlmacen & " " & UltimoGrupo, vTotal, "NO", "", "", vFecha)
        Me.GeneraFileACInventario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), Me.BuscaCuentaExistencia(vAlmacen, vGrupo), Me.mIndicadorDebe, CType("INVENTARIO FINAL ", String), vTotal, vFecha)


        Linea = Linea + 1
        Me.mTipoAsiento = "HABER"
        Me.InsertaOracleInventarios("AC", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), 1, Linea, Me.BuscaCuentaExistenciaSantanaCazorla(vAlmacen, vGrupo), Me.mIndicadorHaber, UltimoAlmacen & " " & UltimoGrupo, vTotal, "NO", "", "", vFecha)
        Me.GeneraFileACInventario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), Me.BuscaCuentaExistenciaSantanaCazorla(vAlmacen, vGrupo), Me.mIndicadorHaber, CType("INVENTARIO FINAL ", String), vTotal, vFecha)
        Me.GeneraFileAAInventario("AA", 7, Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), 1, Me.BuscaCuentaExistenciaSantanaCazorla(vAlmacen, vGrupo), "0", vCentroCosto, vTotal, vFecha)



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
        SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & vAlmacen
        vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

        Linea = Linea + 1
        Me.mTipoAsiento = "HABER"
        Me.InsertaOracleInventarios("AC", 8, Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), 1, Linea, Me.BuscaCuentaExistencia(vAlmacen, vGrupo), Me.mIndicadorHaber, UltimoAlmacen & " " & UltimoGrupo, vTotal, "NO", "", "", vFecha)
        Me.GeneraFileACInventario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), Me.BuscaCuentaExistencia(vAlmacen, vGrupo), Me.mIndicadorHaber, CType("INVENTARIO INICIAL ", String), vTotal, vFecha)


        Linea = Linea + 1
        Me.mTipoAsiento = "DEBE"
        Me.InsertaOracleInventarios("AC", 8, Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), 1, Linea, Me.BuscaCuentaExistenciaSantanaCazorla(vAlmacen, vGrupo), Me.mIndicadorDebe, UltimoAlmacen & " " & UltimoGrupo, vTotal, "NO", "", "", vFecha)
        Me.GeneraFileACInventario("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), Me.BuscaCuentaExistenciaSantanaCazorla(vAlmacen, vGrupo), Me.mIndicadorDebe, CType("INVENTARIO INICIAL ", String), vTotal, vFecha)
        Me.GeneraFileAAInventario("AA", 8, Me.mEmpGrupoCod, Me.mEmpCod, CType(vFecha.Year, String), 1, Me.BuscaCuentaExistenciaSantanaCazorla(vAlmacen, vGrupo), "0", vCentroCosto, vTotal, vFecha)

    End Sub
#End Region
#Region "ASIENTO-20 SALIDAS A GASTO"
    Private Sub SalidasGastoSalidas()
        Dim Total As Double
        Dim vCentroCosto As String

        SQL = "SELECT   TNST_MOVG.MOVG_CODI,TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, NVL(TNST_MOVG.MOVG_IDDO,' ')AS DOCU,"
        SQL += "TNST_GRUP.GRUP_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,GRUP_DESC AS GRUPO,TNST_MOVD.ALMA_CODI AS ALMACODI,TNST_GRUP.GRUP_CODI AS GRUPCODI"
        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_GRUP, TNST_TIMO "
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " AND (TNST_PROD.GRUP_CODI = TNST_GRUP.GRUP_CODI)"
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Salida_Gastos
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " AND TNST_MOVD.MOVD_ENSA = " & -1
        SQL += " GROUP BY  TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,"
        SQL += "TNST_GRUP.GRUP_CODI,"
        SQL += "TNST_MOVG.MOVG_IDDO,"
        SQL += "TNST_TIMO.TIMO_TIPO,"
        SQL += "TNST_ALMA.ALMA_DESC,"
        SQL += "TNST_GRUP.GRUP_DESC"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorHaber, CType("SALIDA A GASTOS ", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("GRUPO"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorHaber, CType("SALIDA A GASTOS ", String), Total)
            Me.GeneraFileAA("AA", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), "0", vCentroCosto, Total)


        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub SalidasGastoSalidasAlmacen()
        Dim Total As Double
        Dim vCentroCosto As String

        SQL = "SELECT   TNST_MOVG.MOVG_CODI,TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, NVL(TNST_MOVG.MOVG_IDDO,' ')AS DOCU,"
        SQL += " SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,TNST_MOVD.ALMA_CODI AS ALMACODI"
        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_GRUP, TNST_TIMO "
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Salida_Gastos
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " AND TNST_MOVD.MOVD_ENSA = " & -1
        SQL += " GROUP BY  TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,"
        SQL += "TNST_MOVG.MOVG_IDDO,"
        SQL += "TNST_TIMO.TIMO_TIPO,"
        SQL += "TNST_ALMA.ALMA_DESC"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorHaber, CType("SALIDA A GASTOS ", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorHaber, CType("SALIDA A GASTOS ", String), Total)
            Me.GeneraFileAA("AA", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), "0", vCentroCosto, Total)


        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
    Private Sub SalidasGastoEntradas()

        Try
            Dim Total As Double
            Dim vCentroCosto As String

            SQL = "SELECT    TNST_MOVG.MOVG_CODI,TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, NVL(TNST_MOVG.MOVG_IDDO,' ')AS DOCU,"
            SQL += " TNST_GRUP.GRUP_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,"
            SQL += " GRUP_DESC AS GRUPO,TNST_MOVG.MOVG_DEST AS ALMACODI,TNST_GRUP.GRUP_CODI AS GRUPCODI"
            SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_GRUP, TNST_TIMO "
            SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
            SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
            SQL += " AND (TNST_MOVG.MOVG_DEST = TNST_ALMA.ALMA_CODI)"
            SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
            SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
            SQL += " AND (TNST_PROD.GRUP_CODI = TNST_GRUP.GRUP_CODI)"
            SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Salida_Gastos
            SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
            SQL += " AND TNST_MOVG.MOVG_ANUL = 0"

            SQL += " GROUP BY  TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_DAVA,"
            SQL += "TNST_MOVG.MOVG_DEST,"
            SQL += "TNST_GRUP.GRUP_CODI,"
            SQL += "TNST_MOVG.MOVG_IDDO,"
            SQL += "TNST_TIMO.TIMO_TIPO,"
            SQL += "TNST_ALMA.ALMA_DESC,"
            SQL += "TNST_GRUP.GRUP_DESC"


            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
                vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorDebe, CType("RECIBE SAL. GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("GRUPO"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorDebe, CType("RECIBE SAL. GASTOS ", String), Total)
                Me.GeneraFileAA("AA", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), "0", vCentroCosto, Total)


            End While
            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Traspasos Entradas")
        End Try

    End Sub
    Private Sub SalidasGastoEntradasAlmacen()

        Try
            Dim Total As Double
            Dim vCentroCosto As String

            SQL = "SELECT    TNST_MOVG.MOVG_CODI,TNST_TIMO.TIMO_TIPO, TNST_MOVG.MOVG_DAVA, NVL(TNST_MOVG.MOVG_IDDO,' ')AS DOCU,"
            SQL += "  SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,"
            SQL += " TNST_MOVG.MOVG_DEST AS ALMACODI "
            SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_TIMO "
            SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
            SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
            SQL += " AND (TNST_MOVG.MOVG_DEST = TNST_ALMA.ALMA_CODI)"
            SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
            SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
            SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Salida_Gastos
            SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
            SQL += " AND TNST_MOVG.MOVG_ANUL = 0"

            SQL += " GROUP BY  TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_DAVA,"
            SQL += "TNST_MOVG.MOVG_DEST,"
            SQL += "TNST_MOVG.MOVG_IDDO,"
            SQL += "TNST_TIMO.TIMO_TIPO,"
            SQL += "TNST_ALMA.ALMA_DESC"


            Me.DbLeeHotel.TraerLector(SQL)

            While Me.DbLeeHotel.mDbLector.Read
                SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
                vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


                Linea = Linea + 1
                Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
                Me.mTipoAsiento = "DEBE"
                Me.InsertaOracle("AC", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorDebe, CType("RECIBE SAL. GASTOS", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "")
                Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), Me.mIndicadorDebe, CType("RECIBE SAL. GASTOS ", String), Total)
                Me.GeneraFileAA("AA", 20, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Me.BuscaCuentaCostoLiquidoAlmacen(CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)), "0", vCentroCosto, Total)


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
        SQL += "TNST_MOVD.ALMA_CODI,TNST_GRUP.GRUP_CODI, SUM(TNST_MOVD.MOVD_TOTA)AS TOTAL,ALMA_DESC AS ALMACEN,GRUP_DESC AS GRUPO,TNST_MOVD.MOVD_ENSA,TNST_MOVD.ALMA_CODI AS ALMACODI,TNST_GRUP.GRUP_CODI AS GRUPCODI"
        SQL += " FROM TNST_MOVG, TNST_MOVD, TNST_ALMA, TNST_PROD, TNST_GRUP, TNST_TIMO "
        SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI)"
        SQL += " AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI)"
        SQL += " AND (TNST_MOVD.ALMA_CODI = TNST_ALMA.ALMA_CODI)"
        SQL += " AND (TNST_MOVD.PROD_CODI = TNST_PROD.PROD_CODI)"
        SQL += " AND (TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI)"
        SQL += " AND (TNST_PROD.GRUP_CODI = TNST_GRUP.GRUP_CODI)"
        SQL += " AND TNST_TIMO.TIMO_TIPO = " & Me.mTimo_Roturas
        SQL += " AND TNST_MOVG.MOVG_DAVA =  " & "'" & Me.mFecha & "'"
        SQL += " AND TNST_MOVG.MOVG_ANUL = 0"
        SQL += " AND TNST_MOVD.MOVD_ENSA = " & -1
        SQL += " GROUP BY TNST_MOVG.MOVG_CODI,TNST_MOVG.MOVG_DAVA,"
        SQL += "TNST_MOVD.ALMA_CODI,"
        SQL += "TNST_GRUP.GRUP_CODI,"
        SQL += "TNST_MOVG.MOVG_IDDO,"
        SQL += "TNST_TIMO.TIMO_TIPO,"
        SQL += "TNST_ALMA.ALMA_DESC,"
        SQL += "TNST_GRUP.GRUP_DESC,"
        SQL += "TNST_MOVD.MOVD_ENSA"

        Me.DbLeeHotel.TraerLector(SQL)

        While Me.DbLeeHotel.mDbLector.Read

            SQL = "SELECT NVL(ALMA_CCST,'0') FROM TNST_ALMA WHERE ALMA_CODI = " & CType(Me.DbLeeHotel.mDbLector("ALMACODI"), Integer)
            vCentroCosto = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)


            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "HABER"
            Me.InsertaOracle("AC", 30, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorHaber, CType("SALIDA ROTURAS ", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("GRUPO"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), Me.mIndicadorHaber, CType("SALIDA ROTURAS ", String), Total)
            Me.GeneraFileAA("AA", 30, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), "0", vCentroCosto, Total)

            Linea = Linea + 1
            Total = CType(Me.DbLeeHotel.mDbLector("TOTAL"), Double)
            Me.mTipoAsiento = "DEBE"
            Me.InsertaOracle("AC", 30, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Linea, Me.mCtaRoturas, Me.mIndicadorDebe, CType("ENTRADA ROTURAS ", String) & " " & CType(Me.DbLeeHotel.mDbLector("ALMACEN"), String) & " " & CType(Me.DbLeeHotel.mDbLector("GRUPO"), String), Total, "NO", CType(Me.DbLeeHotel.mDbLector("DOCU"), String), "")
            Me.GeneraFileAC("AC", Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), Me.mCtaRoturas, Me.mIndicadorDebe, CType("ENTRADA ROTURAS ", String), Total)
            Me.GeneraFileAA("AA", 30, Me.mEmpGrupoCod, Me.mEmpCod, CType(Now.Year, String), 1, Me.BuscaCuentaCostoLiquido(CType(Me.DbLeeHotel.mDbLector("GRUPCODI"), Integer)), "0", vCentroCosto, Total)

        End While
        Me.DbLeeHotel.mDbLector.Close()

    End Sub
#End Region

End Class
