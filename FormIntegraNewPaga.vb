
Public Class FormIntegraNewPaga
    Dim MyIni As New cIniArray
    Dim DbCentral As C_DATOS.C_DatosOledb
    Dim DbLeeNewPaga As C_DATOS.C_DatosOledb
    Dim SQL As String

    Private mStrConexionNewPaga As String
    Private mEstablecimientoNewPaga As String

    Dim HayRegistros As Boolean = False
    Private mStatusBar As StatusBar
    Private mEmpGrupoCod As String
    Private mEmpCod As String

    Private DLL As Integer

    Private Sub FormIntegraNewPaga_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
                MsgBox("No se ha podido leer Grupo de Empresas en Fichero Ini", MsgBoxStyle.Information, "Atenci�n")
            End If



            '---------------------------------------------------------------------------------------------------
            ' Muestra/Pide Hotel a Integrar  
            '----------------------------------------------------------------------------------------------------


            SQL = "SELECT HOTEL_EMPGRUPO_COD,HOTEL_EMP_COD,HOTEL_DESCRIPCION,HOTEL_ODBC,HOTEL_SPYRO,HOTEL_ODBC_NEWGOLF,HOTEL_ODBC_NEWPOS ,HOTEL_ODBC_NEWPAGA FROM TH_HOTEL "
            SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            Me.DataGridHoteles.DataSource = Me.DbCentral.TraerDataset(SQL, "HOTELES")
            Me.DataGridHoteles.DataMember = "HOTELES"

            Me.ConfGriHoteles()

            If Me.DbCentral.mDbDataset.Tables("HOTELES").Rows.Count > 0 Then
                Me.HayRegistros = True
                Me.DataGrid2.CaptionText = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2)
                Me.DataGridHoteles.Select(0)

                Me.mEmpGrupoCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0)
                Me.mEmpCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1)

                SQL = "SELECT NVL(PARA_FILE_SPYRO_PATH,'?') FROM TH_PARA "
                SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                Me.TextBoxRutaFicheros.Text = Me.DbCentral.EjecutaSqlScalar(SQL)

                SQL = "SELECT NVL(HOTEL_ODBC_NEWPAGA,'?') FROM TH_HOTEL "
                SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.mEmpCod & "'"
                Me.mStrConexionNewPaga = Me.DbCentral.EjecutaSqlScalar(SQL)

              


            Else
                Me.HayRegistros = False

            End If

            'Me.TabControlDebug.SelectedTab = Me.TabPage3
            'Me.TabControlDebug.Update()

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
            TextCol2.HeaderText = "C�digo Empresa"
            TextCol2.Width = 75
            ts1.GridColumnStyles.Add(TextCol2)


            Dim TextCol3 As New DataGridTextBoxColumn
            TextCol3.MappingName = "HOTEL_DESCRIPCION"
            TextCol3.HeaderText = "Descripci�n"
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
            TextCol6.Width = 200
            ts1.GridColumnStyles.Add(TextCol6)

            Dim TextCol7 As New DataGridTextBoxColumn
            TextCol7.MappingName = "HOTEL_ODBC_NEWPOS"
            TextCol7.HeaderText = "ODBC NEWPOS"
            TextCol7.Width = 200
            ts1.GridColumnStyles.Add(TextCol7)

            Dim TextCol8 As New DataGridTextBoxColumn
            TextCol8.MappingName = "HOTEL_ODBC_NewPaga"
            TextCol8.HeaderText = "ODBC NewPaga"
            TextCol8.Width = 200
            ts1.GridColumnStyles.Add(TextCol8)

         


            Me.DataGridHoteles.TableStyles.Clear()
            Me.DataGridHoteles.TableStyles.Add(ts1)





        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try
            '    Me.MostrardatosTemporal()

            Dim INTEGRA As Object

            INTEGRA = New Integracion_NewPaga.NewPaga(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0), _
            Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1), _
            MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), _
            Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 7), Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "F" & _
            Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked, _
            Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4), _
            Me.ProgressBar1)
            INTEGRA.Procesar()


            SQL = "SELECT ROUND(SUM(round(ASNT_DEBE,2)),2) FROM TP_ASNT WHERE ASNT_F_VALOR = '" & Me.DateTimePicker1.Value & "'"
            SQL += " AND TP_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TP_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            '   SQL += " AND ASNT_IMPRIMIR = 'SI'"
            Me.TextBoxDebug.Text = Me.DbCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT ROUND(SUM(round(ASNT_HABER,2)),2) FROM TP_ASNT WHERE ASNT_F_VALOR = '" & Me.DateTimePicker1.Value & "'"
            SQL += " AND TP_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TP_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            '  SQL += " AND ASNT_IMPRIMIR = 'SI'"
            Me.TextBoxDebug.Text = Me.TextBoxDebug.Text & "   " & Me.DbCentral.EjecutaSqlScalar(SQL)




            SQL = "SELECT ASNT_F_VALOR F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS TIPO,"
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,ASNT_NOMBRE AS OBSERVACION"
            SQL += ",ASNT_CIF AS CIF,ASNT_AUXILIAR_STRING AS AUX1,ASNT_AUXILIAR_NUMERICO AS AUX2 FROM TP_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Me.DateTimePicker1.Value & "'"
            SQL += " AND TP_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TP_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            '            SQL += " AND ASNT_IMPRIMIR = 'SI'"
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"
            Me.DataGrid2.DataSource = Me.DbCentral.TraerDataset(SQL, "ASIENTO")
            Me.DataGrid2.DataMember = "ASIENTO"
            ' Me.ConfGrid()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Boton Aceptar")
        End Try
    End Sub
    Private Sub MostrardatosTemporal()
        Try

            Me.DbLeeNewPaga = New C_DATOS.C_DatosOledb(Me.mStrConexionNewPaga)
            Me.DbLeeNewPaga.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

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
            SQL += "   AND VNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewPaga & "'"
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
            SQL += "   AND VNCO_MOCO.HOTE_CODI = '" & Me.mEstablecimientoNewPaga & "'"
            '      SQL += "    ORDER BY VNCO_MOCO.MOCO_DAVA DESC, VNCO_MOCO.MOCO_ANUL "
            SQL += " ORDER BY KEY_FIELD "
            Me.DataGrid2.DataSource = Me.DbLeeNewPaga.TraerDataset(SQL, "ASIENTO")
            Me.DataGrid2.DataMember = "ASIENTO"

            Me.DbLeeNewPaga.CerrarConexion()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Mostrar datos")

        End Try
    End Sub

    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            REPORT_SELECTION_FORMULA = "{TP_ASNT.ASNT_F_VALOR}=DATETIME(" & Format(Me.DateTimePicker1.Value, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += " AND {TP_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TP_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"


            Dim Form As New FormVisorCrystal("ASIENTO NEWPAGA.rpt", "Pagos  " & Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"), "", False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atenci�n")

        End Try
    End Sub

    Private Sub ButtonConvertir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonConvertir.Click
        Dim CadenaConexion As String = MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING")

        If Me.CheckBoxDebug.Checked = False Then
            Dim Form As New FormConvertir(Me.DateTimePicker1.Value, CadenaConexion, Me.mEmpGrupoCod, Me.mEmpCod, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), True, "NewPaga")
            Form.ShowDialog()
        Else
            Dim Form As New FormConvertir(Me.DateTimePicker1.Value, CadenaConexion, Me.mEmpGrupoCod, Me.mEmpCod, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), False, "NewPaga")

            Form.ShowDialog()
        End If


    End Sub

    Private Sub DataGridHoteles_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridHoteles.CurrentCellChanged
        Try
            If Me.HayRegistros = True Then
                Me.DataGrid2.CaptionText = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2)
                Me.mEmpGrupoCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0)
                Me.mEmpCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1)

                SQL = "SELECT NVL(PARA_FILE_SPYRO_PATH,'?') FROM TH_PARA "
                SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                Me.TextBoxRutaFicheros.Text = Me.DbCentral.EjecutaSqlScalar(SQL)

            End If

        Catch ex As Exception

        End Try
    End Sub
End Class