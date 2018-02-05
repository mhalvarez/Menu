
Public Class FormIntegraNewConta
    Dim MyIni As New cIniArray
    Dim DbCentral As C_DATOS.C_DatosOledb
    Dim DbLeeNewConta As C_DATOS.C_DatosOledb
    Dim SQL As String

    Private mStrConexionNewConta As String
    Private mEstablecimientoNewConta As String

    Private mHoteCodiNewCentral As Integer

    Dim HayRegistros As Boolean = False
    Private mStatusBar As StatusBar
    Private mEmpGrupoCod As String
    Private mEmpCod As String
    Private mEmpNum As Integer

    Private DLL As Integer

    Private Sub FormIntegraNewConta_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            '---------------------------------------------------------------------------------------------------
            ' Conecta con la Base de Datos "central"
            '----------------------------------------------------------------------------------------------------
            Me.DateTimePicker1.Value = (DateAdd(DateInterval.Day, -1, CType(Format(Now, "dd/MM/yyyy"), Date)))
            Me.DbCentral = New C_DATOS.C_DatosOledb(MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"))
            Me.DbCentral.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")



            '---------------------------------------------------------------------------------------------------
            ' Lee y muestra algunos parametros de Integracion
            '----------------------------------------------------------------------------------------------------
            Me.mEmpGrupoCod = MyIni.IniGet(Application.StartupPath & "\menu.ini", "PARAMETER", "PARA_EMPGRUPO_COD")


            If Me.mEmpGrupoCod = "" Then
                MsgBox("No se ha podido leer Grupo de Empresas en Fichero Ini", MsgBoxStyle.Information, "Atención")
            End If



            ' If Me.mEmpGrupoCod = "SATO" Then
            'Me.ButtonSatocanLibroIgic.Visible = True
            'Else
            '   Me.ButtonSatocanLibroIgic.Visible = False
            'End If

            'SQL = "SELECT PARA_MULTIHOTEL FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            'Me.DbLee.TraerLector(SQL)


            '---------------------------------------------------------------------------------------------------
            ' Muestra/Pide Hotel a Integrar  
            '----------------------------------------------------------------------------------------------------


            SQL = "SELECT HOTEL_EMPGRUPO_COD,HOTEL_EMP_COD,HOTEL_DESCRIPCION,HOTEL_ODBC,HOTEL_SPYRO,HOTEL_ODBC_NEWGOLF,HOTEL_ODBC_NEWPOS ,HOTEL_ODBC_NEWCONTA,HOTEL_ESTABLECIMIENTO_NEWCONTA,HOTEL_EMP_NUM,HOTEL_HOTE_CODI FROM TH_HOTEL "
            SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_HOTEL.HOTEL_INT_NEWC = 1 "

            SQL += " ORDER BY HOTEL_DESCRIPCION ASC"

            Me.DataGridHoteles.DataSource = Me.DbCentral.TraerDataset(SQL, "HOTELES")
            Me.DataGridHoteles.DataMember = "HOTELES"

            Me.ConfGriHoteles()

            If Me.DbCentral.mDbDataset.Tables("HOTELES").Rows.Count > 0 Then
                Me.HayRegistros = True
                Me.DataGrid2.CaptionText = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2)
                Me.DataGridHoteles.Select(0)

                Me.mEmpGrupoCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0)
                Me.mEmpCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1)
                Me.mEmpNum = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 9)

                SQL = "SELECT NVL(PARA_FILE_SPYRO_PATH,'?') FROM TH_PARA "
                SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
                Me.TextBoxRutaFicheros.Text = Me.DbCentral.EjecutaSqlScalar(SQL)

                SQL = "SELECT NVL(HOTEL_ODBC_NEWCONTA,'?') FROM TH_HOTEL "
                SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND HOTEL_EMP_NUM = " & Me.mEmpNum

                Me.mStrConexionNewConta = Me.DbCentral.EjecutaSqlScalar(SQL)

                SQL = "SELECT NVL(HOTEL_ESTABLECIMIENTO_NEWCONTA,'0') FROM TH_HOTEL "
                SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND HOTEL_EMP_NUM = " & Me.mEmpNum

                Me.mEstablecimientoNewConta = Me.DbCentral.EjecutaSqlScalar(SQL)


                SQL = "SELECT NVL(HOTEL_HOTE_CODI,0) FROM TH_HOTEL "
                SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND HOTEL_EMP_NUM = " & Me.mEmpNum

                Me.mHoteCodiNewCentral = Me.DbCentral.EjecutaSqlScalar(SQL)



            Else
                Me.HayRegistros = False

            End If

            'Me.TabControlDebug.SelectedTab = Me.TabPage3
            'Me.TabControlDebug.Update()

            If Me.mEmpGrupoCod = "DUNA" Then
                SQL = "SELECT  PARA_TRATA_ANULACIONES FROM TC_PARA "
                SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
                Me.CheckBoxTrataAnulacionesdelDia.CheckState = Me.DbCentral.EjecutaSqlScalar(SQL)
                Me.CheckBoxTrataAnulacionesdelDia.Visible = True
            Else
                Me.CheckBoxTrataAnulacionesdelDia.Visible = False
            End If

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Exclamation, "Load")
        End Try

    End Sub
    Private Sub ConfGriHoteles()

        Try
            Dim ts1 As New DataGridTableStyle

            ts1.MappingName = "HOTELES"

            Dim TextCol1 As New DataGridTextBoxColumn
            TextCol1.MappingName = "HOTEL_EMPGRUPO_COD"
            TextCol1.HeaderText = "Grupo de Empresas"
            TextCol1.Width = 75
            ts1.GridColumnStyles.Add(TextCol1)


            Dim TextCol2 As New DataGridTextBoxColumn
            TextCol2.MappingName = "HOTEL_EMP_COD"
            TextCol2.HeaderText = "Código Empresa"
            TextCol2.Width = 75
            ts1.GridColumnStyles.Add(TextCol2)


            Dim TextCol3 As New DataGridTextBoxColumn
            TextCol3.MappingName = "HOTEL_DESCRIPCION"
            TextCol3.HeaderText = "Descripción"
            TextCol3.Width = 200
            ts1.GridColumnStyles.Add(TextCol3)

            Dim TextCol4 As New DataGridTextBoxColumn
            TextCol4.MappingName = "HOTEL_ODBC"
            TextCol4.HeaderText = "ODBC HOTEL"
            TextCol4.Width = 0
            ts1.GridColumnStyles.Add(TextCol4)

            Dim TextCol5 As New DataGridTextBoxColumn
            TextCol5.MappingName = "HOTEL_SPYRO"
            TextCol5.HeaderText = "ODBC SPYRO"
            TextCol5.Width = 0
            ts1.GridColumnStyles.Add(TextCol5)


            Dim TextCol6 As New DataGridTextBoxColumn
            TextCol6.MappingName = "HOTEL_ODBC_NEWGOLF"
            TextCol6.HeaderText = "ODBC NEWGOLF"
            TextCol6.Width = 0
            ts1.GridColumnStyles.Add(TextCol6)

            Dim TextCol7 As New DataGridTextBoxColumn
            TextCol7.MappingName = "HOTEL_ODBC_NEWPOS"
            TextCol7.HeaderText = "ODBC NEWPOS"
            TextCol7.Width = 0
            ts1.GridColumnStyles.Add(TextCol7)

            Dim TextCol8 As New DataGridTextBoxColumn
            TextCol8.MappingName = "HOTEL_ODBC_NEWCONTA"
            TextCol8.HeaderText = "ODBC NEWCONTA"
            TextCol8.Width = 0
            ts1.GridColumnStyles.Add(TextCol8)

            Dim TextCol9 As New DataGridTextBoxColumn
            TextCol9.MappingName = "HOTEL_ESTABLECIMIENTO_NEWCONTA"
            TextCol9.HeaderText = "ESTABLECIMIENTO NEWCONTA"
            TextCol9.Width = 200
            ts1.GridColumnStyles.Add(TextCol9)


            Dim TextCol10 As New DataGridTextBoxColumn
            TextCol10.MappingName = "HOTEL_EMP_NUM"
            TextCol10.HeaderText = "Número de Empresa"
            TextCol10.Width = 100
            ts1.GridColumnStyles.Add(TextCol10)


            Dim TextCol11 As New DataGridTextBoxColumn
            TextCol11.MappingName = "HOTEL_HOTE_CODI"
            TextCol11.HeaderText = "Hotel NewCentral"
            TextCol11.Width = 100
            ts1.GridColumnStyles.Add(TextCol11)


            Me.DataGridHoteles.TableStyles.Clear()
            Me.DataGridHoteles.TableStyles.Add(ts1)





        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try
            '    Me.MostrardatosTemporal()


            Me.Cursor = Cursors.AppStarting


            If Me.mEmpGrupoCod = "DUNA" Then
                Dim INTEGRA As Integracion_NewConta_Dunas.NewContaDunas

                Me.DataGrid2.CaptionText = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2) & " Código de  Hotel en Central = " & Me.mHoteCodiNewCentral & " Nùmero de Establecimiento en NewConta = " & Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 8)
                Me.DataGrid2.Update()

                INTEGRA = New Integracion_NewConta_Dunas.NewContaDunas(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0), _
                Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1), _
                MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), _
                Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 7), Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "NC-" & _
                Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked, _
                Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4), _
                Me.ProgressBar1, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 8), _
                Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 9), _
                Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Me.CheckBoxOtrosCreditos.Checked, _
                Me.CheckBoxOtrosDebitos.Checked, CODIGO_RECLAMACIONES, CODIGO_NOTACREDITO, Me, Me.mHoteCodiNewCentral, CODIGO_FACTURAS, Me.CheckBoxMultiCobros.Checked)
                INTEGRA.Procesar()

            Else
                Dim INTEGRA As Integracion_NewConta.NewConta

                Me.DataGrid2.CaptionText = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2) & " Código de  Hotel en Central = " & Me.mHoteCodiNewCentral & " Nùmero de Establecimiento en NewConta = " & Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 8)
                Me.DataGrid2.Update()

                INTEGRA = New Integracion_NewConta.NewConta(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0), _
                Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1), _
                MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), _
                Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 7), Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "NC-" & _
                Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked, _
                Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4), _
                Me.ProgressBar1, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 8), _
                Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 9), _
                Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Me.CheckBoxOtrosCreditos.Checked, _
                Me.CheckBoxOtrosDebitos.Checked, CODIGO_RECLAMACIONES, CODIGO_NOTACREDITO, Me, Me.mHoteCodiNewCentral, STRING_SAHARA)
                INTEGRA.Procesar()



            End If
           
            SQL = "SELECT ROUND(SUM(round(ASNT_DEBE,2)),2) FROM TC_ASNT WHERE ASNT_F_ATOCAB = '" & Me.DateTimePicker1.Value & "'"
            SQL += " AND TC_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TC_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TC_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
            '   SQL += " AND ASNT_IMPRIMIR = 'SI'"
            Me.TextBoxDebug.Text = Me.DbCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT ROUND(SUM(round(ASNT_HABER,2)),2) FROM TC_ASNT WHERE ASNT_F_ATOCAB = '" & Me.DateTimePicker1.Value & "'"
            SQL += " AND TC_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TC_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TC_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
            '  SQL += " AND ASNT_IMPRIMIR = 'SI'"
            Me.TextBoxDebug.Text = Me.TextBoxDebug.Text & "   " & Me.DbCentral.EjecutaSqlScalar(SQL)


            If Me.mEmpGrupoCod <> "DUNA" Then
                SQL = "SELECT ASNT_F_ATOCAB AS FECHA,ASNT_F_VALOR F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS TIPO,"
                SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,ASNT_NOMBRE AS OBSERVACION"
                SQL += ",ASNT_CIF AS CIF,ASNT_AUXILIAR_STRING AS AUX1,ASNT_AUXILIAR_STRING2 AS AUX2"
                SQL += ",ASNT_DOC_CREDITO AS REFERENCIA,ASNT_DESC_CREDITO AS REFERENCIA2"

                SQL += " FROM TC_ASNT "
                SQL += " WHERE ASNT_F_ATOCAB = '" & Me.DateTimePicker1.Value & "'"
                SQL += " AND TC_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND TC_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND TC_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
                '            SQL += " AND ASNT_IMPRIMIR = 'SI'"
                SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"
            Else
                SQL = "SELECT ASNT_RECIBO AS RECIBO,ASNT_F_ATOCAB AS FECHA,ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS TIPO,"
                SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,ASNT_NOMBRE AS OBSERVACION"
                SQL += ",ASNT_CIF AS CIF,ASNT_AUXILIAR_STRING AS AUX1,ASNT_AUXILIAR_STRING2 AS AUX2"
                SQL += ",ASNT_DOC_CREDITO AS REFERENCIA,ASNT_DESC_CREDITO AS REFERENCIA2"

                SQL += " FROM TC_ASNT "
                SQL += " WHERE ASNT_F_ATOCAB = '" & Me.DateTimePicker1.Value & "'"
                SQL += " AND TC_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND TC_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND TC_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
                '            SQL += " AND ASNT_IMPRIMIR = 'SI'"
                SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_RECIBO ,ASNT_LINEA"
            End If

          
            Me.DataGrid2.DataSource = Me.DbCentral.TraerDataset(SQL, "ASIENTO")
            Me.DataGrid2.DataMember = "ASIENTO"
            ' Me.ConfGrid()


            Me.Cursor = Cursors.Default


           

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Boton Aceptar")
        End Try
    End Sub
    Private Sub MostrardatosTemporal()
        Try

            Me.DbLeeNewConta = New C_DATOS.C_DatosOledb(Me.mStrConexionNewConta)
            Me.DbLeeNewConta.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            SQL = "SELECT  'MOVIMIENTOS DE CREDITO TIPO PAGO' , 'DEBE',KEY_FIELD, VNCO_MOCO.HOTE_CODI,VNCO_MOCO.TACO_CODI,NVL(TNCO_TIMO.TIMO_COCO,'0') AS CUENTA, VNCO_MOCO.TIMO_CODI, "
            SQL += "         VNCO_MOCO.MOCO_DOCU, VNCO_MOCO.MOCO_DOC1, VNCO_MOCO.UNMO_CODI, "
            SQL += "         TRUNC (VNCO_MOCO.MOCO_DAVA) DAVA, VNCO_MOCO.MOCO_VAOR, "
            SQL += "         VNCO_MOCO.MOCO_CAMB, VNCO_MOCO.MOCO_DEBI, VNCO_MOCO.MOCO_CRED, "
            SQL += "         VNCO_MOCO.MOCO_ANUL, VNCO_MOCO.TACO_NOME, VNCO_MOCO.MOCO_RECI, "
            SQL += "         VNCO_MOCO.MOCO_CODI, VNCO_MOCO.MOCO_DESC, VNCO_MOCO.MOCO_EXTE, "
            SQL += "         VNCO_MOCO.MOCO_OBSE, VNCO_MOCO.DIFE_RATE, VNCO_MOCO.MOCO_DIFE, "
            SQL += "         ABS (VNCO_MOCO.MOCO_VADE) MOCO_VADE, VNCO_MOCO.PACO_PAAS, "
            SQL += "         VNCO_MOCO.TACO_ORIG, VNCO_MOCO.MOCO_DATR, VNCO_MOCO.HOTE_CODI, "
            SQL += "         HOTE_DESC, MOLI_DESC, VNCO_MOCO.FACT_CODI "
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
            SQL += "     AND VNCO_MOCO.MOCO_DAVA = '" & Me.DateTimePicker1.Value & "'"
            SQL += "   AND VNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            '       SQL += "    ORDER BY VNCO_MOCO.MOCO_DAVA DESC, VNCO_MOCO.MOCO_ANUL "

            SQL += " UNION "
            SQL += "SELECT  'MOVIMIENTOS DE CREDITO TIPO PAGO DEPOSITO ANTICIPADO' , 'HABER',KEY_FIELD, VNCO_MOCO.HOTE_CODI,VNCO_MOCO.TACO_CODI,NVL(TNCO_TIMO.TIMO_COCO,'0') AS CUENTA, VNCO_MOCO.TIMO_CODI, "
            SQL += "         VNCO_MOCO.MOCO_DOCU, VNCO_MOCO.MOCO_DOC1, VNCO_MOCO.UNMO_CODI, "
            SQL += "         TRUNC (VNCO_MOCO.MOCO_DAVA) DAVA, VNCO_MOCO.MOCO_VAOR, "
            SQL += "         VNCO_MOCO.MOCO_CAMB, VNCO_MOCO.MOCO_DEBI, VNCO_MOCO.MOCO_CRED, "
            SQL += "         VNCO_MOCO.MOCO_ANUL, VNCO_MOCO.TACO_NOME, VNCO_MOCO.MOCO_RECI, "
            SQL += "         VNCO_MOCO.MOCO_CODI, VNCO_MOCO.MOCO_DESC, VNCO_MOCO.MOCO_EXTE, "
            SQL += "         VNCO_MOCO.MOCO_OBSE, VNCO_MOCO.DIFE_RATE, VNCO_MOCO.MOCO_DIFE, "
            SQL += "         ABS (VNCO_MOCO.MOCO_VADE) MOCO_VADE, VNCO_MOCO.PACO_PAAS, "
            SQL += "         VNCO_MOCO.TACO_ORIG, VNCO_MOCO.MOCO_DATR, VNCO_MOCO.HOTE_CODI, "
            SQL += "         HOTE_DESC, MOLI_DESC, VNCO_MOCO.FACT_CODI "
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
            SQL += "     AND VNCO_MOCO.MOCO_DAVA = '" & Me.DateTimePicker1.Value & "'"
            SQL += "   AND VNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewConta & "'"
            '      SQL += "    ORDER BY VNCO_MOCO.MOCO_DAVA DESC, VNCO_MOCO.MOCO_ANUL "
            SQL += " ORDER BY KEY_FIELD "
            Me.DataGrid2.DataSource = Me.DbLeeNewConta.TraerDataset(SQL, "ASIENTO")
            Me.DataGrid2.DataMember = "ASIENTO"

            Me.DbLeeNewConta.CerrarConexion()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Mostrar datos")

        End Try
    End Sub

    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click
        Try


            MsgBox("Cuadrar Los pagos recibidos con Consulta de Recibos de Newconta " & vbCrLf & "Cuadrar los Documentos Regularizados  con Reporte Diario de NewConta ", MsgBoxStyle.Information, "Atención")



            Me.Cursor = Cursors.WaitCursor

            REPORT_SELECTION_FORMULA = "{TC_ASNT.ASNT_F_ATOCAB}=DATETIME(" & Format(Me.DateTimePicker1.Value, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += " AND {TC_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TC_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TC_ASNT.ASNT_EMP_NUM}= " & Me.mEmpNum


            If EMPGRUPO_COD = "DUNA" Then
                Dim Form As New FormVisorCrystal("ASIENTO NEWCONTA-DUNAS.rpt", "COBROS  " & Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"), "", False, False)
                Form.MdiParent = Me.MdiParent
                Form.StartPosition = FormStartPosition.CenterScreen
                Form.Show()
                Me.Cursor = Cursors.Default
            Else
                Dim Form As New FormVisorCrystal("ASIENTO NEWCONTA.rpt", "COBROS  " & Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"), "", False, False)
                Form.MdiParent = Me.MdiParent
                Form.StartPosition = FormStartPosition.CenterScreen
                Form.Show()
                Me.Cursor = Cursors.Default
            End If

          
         


        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub ButtonConvertir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonConvertir.Click
        Dim CadenaConexion As String = MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING")

        If Me.CheckBoxDebug.Checked = False Then
            Dim Form As New FormConvertir(Me.DateTimePicker1.Value, CadenaConexion, Me.mEmpGrupoCod, Me.mEmpCod, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), True, "NEWCONTA")
            Form.ShowDialog()
        Else
            Dim Form As New FormConvertir(Me.DateTimePicker1.Value, CadenaConexion, Me.mEmpGrupoCod, Me.mEmpCod, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), False, "NEWCONTA")

            Form.ShowDialog()
        End If


    End Sub

    Private Sub DataGridHoteles_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridHoteles.CurrentCellChanged
        Try
            If Me.HayRegistros = True Then
                Me.DataGrid2.CaptionText = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2)
                Me.mEmpGrupoCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0)
                Me.mEmpCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1)
                Me.mEmpNum = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 9)


                SQL = "SELECT NVL(PARA_FILE_SPYRO_PATH,'?') FROM TH_PARA "
                SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
                Me.TextBoxRutaFicheros.Text = Me.DbCentral.EjecutaSqlScalar(SQL)

                SQL = "SELECT NVL(HOTEL_ODBC_NEWCONTA,'?') FROM TH_HOTEL "
                SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND HOTEL_EMP_NUM = " & Me.mEmpNum

                Me.mStrConexionNewConta = Me.DbCentral.EjecutaSqlScalar(SQL)

                SQL = "SELECT NVL(HOTEL_ESTABLECIMIENTO_NEWCONTA,'0') FROM TH_HOTEL "
                SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND HOTEL_EMP_NUM = " & Me.mEmpNum

                Me.mEstablecimientoNewConta = Me.DbCentral.EjecutaSqlScalar(SQL)


                SQL = "SELECT NVL(HOTEL_HOTE_CODI,0) FROM TH_HOTEL "
                SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND HOTEL_EMP_NUM = " & Me.mEmpNum

                Me.mHoteCodiNewCentral = Me.DbCentral.EjecutaSqlScalar(SQL)



                If Me.mEmpGrupoCod = "DUNA" Then
                    SQL = "SELECT  PARA_TRATA_ANULACIONES FROM TC_PARA "
                    SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                    SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                    SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
                    Me.CheckBoxTrataAnulacionesdelDia.CheckState = Me.DbCentral.EjecutaSqlScalar(SQL)
                    Me.CheckBoxTrataAnulacionesdelDia.Visible = True
                Else
                    Me.CheckBoxTrataAnulacionesdelDia.Visible = False
                End If




            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub DataGridHoteles_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs) Handles DataGridHoteles.Navigate

    End Sub

    Private Sub ButtonGrants_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonGrants.Click
        Dim AliasDB As String = InputBox("Alias Base de Datos de la Central", , "DBCENTRAL")
        Dim StrConexion As String = "Provider=MSDAORA.1;User Id=NCC;Password=NCC;Data Source = " & AliasDB

        Me.Cursor = Cursors.WaitCursor

        Dim DB As New C_DATOS.C_DatosOledb

        DB.StrConexion = StrConexion
        DB.AbrirConexion()


        Try
            SQL = "CREATE PUBLIC SYNONYM TNCC_OPER FOR NCC.TNCC_OPER"
            DB.EjecutaSqlCommit(SQL)

        Catch ex As Exception

        End Try


        Try
            SQL = "CREATE PUBLIC SYNONYM TNCC_ENHO FOR NCC.TNCC_ENHO"
            DB.EjecutaSqlCommit(SQL)

        Catch ex As Exception

        End Try



        Try
            SQL = "GRANT SELECT ON TNCC_OPER TO NCO"
            DB.EjecutaSqlCommit(SQL)

        Catch ex As Exception

        End Try


        Try
            SQL = "GRANT SELECT ON TNCC_ENHO TO NCO"
            DB.EjecutaSqlCommit(SQL)

        Catch ex As Exception

        End Try

        Me.Cursor = Cursors.Default
        DB.CerrarConexion()



    End Sub



    Private Sub FormIntegraNewConta_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            OTROS_CREDITOS = MyIni.IniGet(Application.StartupPath & "\menu.ini", "NEWCONTA", "OTROS_CREDITOS")
            OTROS_DEBITOS = MyIni.IniGet(Application.StartupPath & "\menu.ini", "NEWCONTA", "OTROS_DEBITOS")
            CODIGO_RECLAMACIONES = MyIni.IniGet(Application.StartupPath & "\menu.ini", "NEWCONTA", "CODIGO_RECLAMACIONES")
            CODIGO_NOTACREDITO = MyIni.IniGet(Application.StartupPath & "\menu.ini", "NEWCONTA", "CODIGO_NOTACREDITO")
            CODIGO_FACTURAS = MyIni.IniGet(Application.StartupPath & "\menu.ini", "NEWCONTA", "CODIGO_FACTURAS")
            STRING_SAHARA = MyIni.IniGet(Application.StartupPath & "\menu.ini", "OTROS", "STRING_SAHARA")



            If OTROS_CREDITOS = "" Then
                MsgBox("Parámetro Fichero Ini No Válido [OTROS_CREDITOS]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            ElseIf OTROS_CREDITOS = "1" Or OTROS_CREDITOS = "0" Then
                ' ok 
            Else
                MsgBox("Parámetro Fichero Ini No Válido [OTROS_CREDITOS]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            End If


            If OTROS_DEBITOS = "" Then
                MsgBox("Parámetro Fichero Ini No Válido [OTROS_DEBITOS]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            ElseIf OTROS_DEBITOS = "1" Or OTROS_DEBITOS = "0" Then
                ' ok 
            Else
                MsgBox("Parámetro Fichero Ini No Válido [OTROS_DEBITOS]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            End If


            If CODIGO_RECLAMACIONES = "" Then
                MsgBox("Parámetro Fichero Ini No Válido [CODIGO_RECLAMACIONES]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            ElseIf CODIGO_RECLAMACIONES <> "" Then
                ' ok 
            Else
                MsgBox("Parámetro Fichero Ini No Válido [CODIGO_RECLAMACIONES]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            End If


            If CODIGO_NOTACREDITO = "" Then
                MsgBox("Parámetro Fichero Ini No Válido [CODIGO_NOTACREDITO]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            ElseIf CODIGO_NOTACREDITO <> "" Then
                ' ok 
            Else
                MsgBox("Parámetro Fichero Ini No Válido [CODIGO_NOTACREDITO]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            End If


            If OTROS_CREDITOS = "1" Then
                Me.CheckBoxOtrosCreditos.Checked = True
            Else
                Me.CheckBoxOtrosCreditos.Checked = False
            End If

            If OTROS_DEBITOS = "1" Then
                Me.CheckBoxOtrosDebitos.Checked = True
            Else
                Me.CheckBoxOtrosDebitos.Checked = False
            End If

            If STRING_SAHARA = "" Then
                MsgBox("Parámetro Fichero Ini No Válido [STRING_SAHARA]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            ElseIf STRING_SAHARA <> "" Then
                ' ok 
            Else
                MsgBox("Parámetro Fichero Ini No Válido [OTROS_CREDITOS]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            End If



        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonImprimirSaldos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimirSaldos.Click
        Try

            Dim ReportName As String
            Dim Titulo As String


            If Me.mEmpGrupoCod = "DUNA" Then
                ReportName = "SALDOS DUNAS.rpt"
            Else
                ReportName = "SALDOS HLOPEZ.rpt"
            End If

            Me.Cursor = Cursors.WaitCursor

            If MessageBox.Show("Desa Imprimir solo el Hotel Actual  ", "Atención", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                REPORT_SELECTION_FORMULA = "{TNCG_MOCO.HOTE_CODI} =  '" & Me.mEstablecimientoNewConta & "' AND {TNCG_MOCO.MOCO_ANUL} = '0'"
                Titulo = ""
            Else
                REPORT_SELECTION_FORMULA = "{TNCG_MOCO.MOCO_ANUL} = '0'"
                Titulo = "Todos los Hoteles"
            End If



            Dim Form As New FormVisorCrystal(ReportName, Titulo, REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"), Me.mStrConexionNewConta, True, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub ButtonFechasPosiblesNewConta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFechasPosiblesNewConta.Click
        Try

            If Me.ListBoxFechasPosibles.Visible = True Then
                Me.ListBoxFechasPosibles.Visible = False
                Exit Sub
            Else
                Me.MostrarFechasPosibles()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub MostrarFechasPosibles()
        Try

            Dim Result As String

            Me.ListBoxFechasPosibles.Visible = True

            Me.ListBoxFechasPosibles.Items.Clear()
            Me.ListBoxFechasPosibles.Update()


            Me.DbLeeNewConta = New C_DATOS.C_DatosOledb(Me.mStrConexionNewConta)


            If Me.DbLeeNewConta.EstadoConexion = ConnectionState.Open Then
                Me.DbLeeNewConta.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

                SQL = "SELECT DATR_DATR FROM TNCO_DATR"
                Result = Me.DbLeeNewConta.EjecutaSqlScalar(SQL)

                If IsNothing(Result) = False Then
                    Me.ListBoxFechasPosibles.Items.Add("Fecha de Trabajo = " & Result)
                    Me.ListBoxFechasPosibles.Items.Add("-------------------------------------")
                    Me.ListBoxFechasPosibles.Items.Add("")
                End If


                SQL = "SELECT MOCO_DATR AS FECHA  ,' Cobros    ' as TIPO ,HOTE_DESC AS HOTEL FROM VNCO_MOCO,TNCO_TIMO,TNCO_HOTE WHERE "
                SQL += " TO_CHAR(MOCO_DATR,'YYYY') = '" & Year(DateTimePicker1.Value) & "'"

                SQL += "     AND VNCO_MOCO.TIMO_CODI = TNCO_TIMO.TIMO_CODI "
                SQL += "     AND VNCO_MOCO.HOTE_CODI = TNCO_HOTE.HOTE_CODI "
                SQL += "     AND VNCO_MOCO.MOCO_DEBI = 0 "
                '    -- SOLO MOV DE TIPO PAGO "
                SQL += "     AND TNCO_TIMO.TIMO_PAGA = 1 "
                SQL += " GROUP BY MOCO_DATR ,HOTE_DESC "


                SQL += " UNION "


                SQL += "SELECT MORE_DARE AS FECHA  ,' Regulariza' as TIPO,HOTE_DESC AS HOTEL FROM TNCO_MORE,TNCO_MOCO,TNCO_HOTE  WHERE "

                SQL += " TNCO_MORE.TACO_COD1 = TNCO_MOCO.TACO_CODI"
                SQL += " AND TNCO_MORE.MOCO_COD1 = TNCO_MOCO.MOCO_CODI"
                SQL += " AND TNCO_MOCO.HOTE_CODI = TNCO_HOTE.HOTE_CODI "

                SQL += " AND TO_CHAR(MORE_DARE,'YYYY') = '" & Year(DateTimePicker1.Value) & "'"
                SQL += " AND TNCO_MOCO.TIMO_CODI <> '" & CODIGO_FACTURAS & "'"


                SQL += " GROUP BY MORE_DARE,HOTE_DESC  "



                Me.DbLeeNewConta.TraerLector(SQL)
                While Me.DbLeeNewConta.mDbLector.Read

                    Result = Me.DbLeeNewConta.mDbLector.Item("FECHA") & Me.DbLeeNewConta.mDbLector.Item("TIPO") & " " & Me.DbLeeNewConta.mDbLector.Item("HOTEL")
                    Me.ListBoxFechasPosibles.Items.Add(Result)

                End While
                Me.DbLeeNewConta.mDbLector.Close()
                Me.DbLeeNewConta.CerrarConexion()

            Else
                MsgBox("No Dispone de Acceso a la Base de Datos")
                Exit Sub
            End If




        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
  

    Private Sub CheckBoxTrataAnulacionesdelDia_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxTrataAnulacionesdelDia.CheckedChanged
        Try
            SQL = "UPDATE TC_PARA "
            SQL += "SET "

            If Me.CheckBoxTrataAnulacionesdelDia.Checked Then
                SQL += " PARA_TRATA_ANULACIONES = 1"
            Else
                SQL += " PARA_TRATA_ANULACIONES = 0"
            End If
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum



            If IsNothing(DbCentral) = False Then
                If Me.DbCentral.EstadoConexion = ConnectionState.Open Then
                    Me.DbCentral.EjecutaSqlCommit(SQL)
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

  
   
    Private Sub ButtonImprimir3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try


            MsgBox("Cuadrar Los pagos recibidos con Consulta de Recibos de Newconta " & vbCrLf & "Cuadrar los Documentos Regularizados  con Reporte Diario de NewConta ", MsgBoxStyle.Information, "Atención")



            Me.Cursor = Cursors.WaitCursor

            REPORT_SELECTION_FORMULA = "{TC_ASNT.ASNT_F_ATOCAB}=DATETIME(" & Format(Me.DateTimePicker1.Value, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += " AND {TC_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TC_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TC_ASNT.ASNT_EMP_NUM}= " & Me.mEmpNum


            Dim Form As New FormVisorCrystal("ASIENTO NEWCONTA4.rpt", "COBROS  " & Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"), "", False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default



        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub ButtonImprimir2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir2.Click
        Try


            MsgBox("Cuadrar Los pagos recibidos con Consulta de Recibos de Newconta " & vbCrLf & "Cuadrar los Documentos Regularizados  con Reporte Diario de NewConta ", MsgBoxStyle.Information, "Atención")

            Me.Cursor = Cursors.WaitCursor

            If EMPGRUPO_COD = "DUNA" Then

                REPORT_SELECTION_FORMULA = "{TC_ASNT.ASNT_F_ATOCAB}=DATETIME(" & Format(Me.DateTimePicker1.Value, REPORT_DATE_FORMAT) & ")"
                REPORT_SELECTION_FORMULA += " AND {TC_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
                REPORT_SELECTION_FORMULA += " AND {TC_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"
                REPORT_SELECTION_FORMULA += " AND {TC_ASNT.ASNT_EMP_NUM}= " & Me.mEmpNum
                REPORT_SELECTION_FORMULA += " AND ({TC_ASNT.ASNT_CFATOCAB_REFER} = 333 OR {TC_ASNT.ASNT_CFATOCAB_REFER} = 777)"
                '   REPORT_SELECTION_FORMULA += " AND {TC_ASNT.ASNT_CFCPTOS_COD} = 'H'"

                Dim Form As New FormVisorCrystal("ASIENTO NEWCONTA3.rpt", "COBROS  " & Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"), "", False, False)
                Form.MdiParent = Me.MdiParent
                Form.StartPosition = FormStartPosition.CenterScreen
                Form.Show()
                Me.Cursor = Cursors.Default
            Else

                REPORT_SELECTION_FORMULA = "{TC_ASNT.ASNT_F_ATOCAB}=DATETIME(" & Format(Me.DateTimePicker1.Value, REPORT_DATE_FORMAT) & ")"
                REPORT_SELECTION_FORMULA += " AND {TC_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
                REPORT_SELECTION_FORMULA += " AND {TC_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"
                REPORT_SELECTION_FORMULA += " AND {TC_ASNT.ASNT_EMP_NUM}= " & Me.mEmpNum


                Dim Form As New FormVisorCrystal("ASIENTO NEWCONTA2.rpt", "COBROS  " & Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"), "", False, False)
                Form.MdiParent = Me.MdiParent
                Form.StartPosition = FormStartPosition.CenterScreen
                Form.Show()
                Me.Cursor = Cursors.Default
            End If






        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub
End Class