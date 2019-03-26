Imports System.Text
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
    Private mEmpNum As Integer

    Private DLL As Integer

    Private mRestultStr As String
    Private mRestulInt As Integer


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
                MsgBox("No se ha podido leer Grupo de Empresas en Fichero Ini", MsgBoxStyle.Information, "Atención")
            End If



            '---------------------------------------------------------------------------------------------------
            ' Muestra/Pide Hotel a Integrar  
            '----------------------------------------------------------------------------------------------------


            SQL = "SELECT HOTEL_EMPGRUPO_COD,HOTEL_EMP_COD,HOTEL_DESCRIPCION,HOTEL_ODBC,HOTEL_SPYRO,HOTEL_ODBC_NEWGOLF,HOTEL_ODBC_NEWPOS ,HOTEL_ODBC_NEWPAGA , HOTEL_EMP_NUM,HOTEL_ODBC_ALMACEN FROM TH_HOTEL "
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
                Me.mEmpNum = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 9)


                SQL = "SELECT NVL(PARA_FILE_SPYRO_PATH,'?') FROM TS_PARA "
                SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum

                Me.TextBoxRutaFicheros.Text = Me.DbCentral.EjecutaSqlScalar(SQL)

                SQL = "SELECT NVL(HOTEL_ODBC_NEWPAGA,'?') FROM TH_HOTEL "
                SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND HOTEL_EMP_NUM = " & Me.mEmpNum
                Me.mStrConexionNewPaga = Me.DbCentral.EjecutaSqlScalar(SQL)

                Me.FechaUltima()

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
            TextCol8.MappingName = "HOTEL_ODBC_NewPaga"
            TextCol8.HeaderText = "ODBC NewPaga"
            TextCol8.Width = 0
            ts1.GridColumnStyles.Add(TextCol8)

            Dim TextCol9 As New DataGridTextBoxColumn
            TextCol9.MappingName = "HOTEL_ODBC_ALMACEN"
            TextCol9.HeaderText = "Odbc NewStock"
            TextCol9.Width = 0
            ts1.GridColumnStyles.Add(TextCol9)


            Dim TextCol10 As New DataGridTextBoxColumn
            TextCol10.MappingName = "HOTEL_EMP_NUM"
            TextCol10.HeaderText = "Emp Num"
            TextCol10.Width = 20
            ts1.GridColumnStyles.Add(TextCol10)

            Me.DataGridHoteles.TableStyles.Clear()
            Me.DataGridHoteles.TableStyles.Add(ts1)





        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub ConfGrid()

        Try
            Dim ts1 As New DataGridTableStyle

            ts1.MappingName = "ASIENTO"

            Dim TextCol1A As New DataGridTextBoxColumn
            TextCol1A.MappingName = "F_REGISTRO"
            TextCol1A.HeaderText = "F. Registro"
            TextCol1A.Width = 75

            ts1.GridColumnStyles.Add(TextCol1A)

            Dim TextCol1 As New DataGridTextBoxColumn
            TextCol1.MappingName = "F_VALOR"
            TextCol1.HeaderText = "F. Valor"
            TextCol1.Width = 75

            ts1.GridColumnStyles.Add(TextCol1)


            Dim TextCol2 As New DataGridTextBoxColumn
            TextCol2.MappingName = "CUENTA"
            TextCol2.HeaderText = "Cuenta"
            TextCol2.Width = 75
            ts1.GridColumnStyles.Add(TextCol2)




            Dim TextCol2A As New DataGridTextBoxColumn
            TextCol2A.MappingName = "Tipo"
            TextCol2A.HeaderText = "Identificador"
            TextCol2A.Width = 20
            ts1.GridColumnStyles.Add(TextCol2A)

            Dim TextCol3 As New DataGridTextBoxColumn
            TextCol3.MappingName = "CONCEPTO"
            TextCol3.HeaderText = "Concepto"
            TextCol3.Width = 250

            ts1.GridColumnStyles.Add(TextCol3)


            Dim TextCol4 As New DataGridTextBoxColumn
            TextCol4.MappingName = "DEBE"
            TextCol4.HeaderText = "Debe"
            TextCol4.Width = 100
            TextCol4.NullText = "_"
            ts1.GridColumnStyles.Add(TextCol4)


            Dim TextCol5 As New DataGridTextBoxColumn
            TextCol5.MappingName = "HABER"
            TextCol5.HeaderText = "Haber"
            TextCol5.Width = 100
            TextCol5.NullText = "_"
            ts1.GridColumnStyles.Add(TextCol5)


            Dim TextCol6 As New DataGridTextBoxColumn
            TextCol6.MappingName = "OBSERVACION"
            TextCol6.HeaderText = "Observación"
            TextCol6.Width = 300
            TextCol6.NullText = "_"
            ts1.GridColumnStyles.Add(TextCol6)

            Dim TextCol7 As New DataGridTextBoxColumn
            TextCol7.MappingName = "CIF"
            TextCol7.HeaderText = "CIF"
            TextCol7.Width = 100
            TextCol7.NullText = "_"
            ts1.GridColumnStyles.Add(TextCol7)

            Dim TextCol8 As New DataGridTextBoxColumn
            TextCol8.MappingName = "AUX1"
            TextCol8.HeaderText = "Auxiliar Str"
            TextCol8.Width = 100
            TextCol8.NullText = "_"
            ts1.GridColumnStyles.Add(TextCol8)

            Dim TextCol9 As New DataGridTextBoxColumn
            TextCol9.MappingName = "AUX2"
            TextCol9.HeaderText = "Auxiliar Num"
            TextCol9.Width = 100
            TextCol9.NullText = "_"
            ts1.GridColumnStyles.Add(TextCol9)







            '   ts1.AlternatingBackColor = Color.LightGray

            DataGrid2.TableStyles.Clear()
            DataGrid2.TableStyles.Add(ts1)




        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try
            '    Me.MostrardatosTemporal()

            Dim INTEGRA As Object

            Me.ButtonIncidencias.Enabled = False

            INTEGRA = New Integracion_NewPaga.NewPaga(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
            Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
              MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
              Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 7), Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "NewPaGa " &
              Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
              Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
              Me.ProgressBar1, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 8), Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 9))

            Me.Cursor = Cursors.AppStarting
            INTEGRA.Procesar()


            SQL = "SELECT ROUND(SUM(round(ASNT_DEBE,2)),2) FROM TP_ASNT WHERE ASNT_F_ATOCAB = '" & Me.DateTimePicker1.Value & "'"
            SQL += " AND TP_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TP_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            '   SQL += " AND ASNT_IMPRIMIR = 'SI'"
            Me.TextBoxDebug.Text = Me.DbCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT ROUND(SUM(round(ASNT_HABER,2)),2) FROM TP_ASNT WHERE ASNT_F_ATOCAB = '" & Me.DateTimePicker1.Value & "'"
            SQL += " AND TP_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TP_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            '  SQL += " AND ASNT_IMPRIMIR = 'SI'"
            Me.TextBoxDebug.Text = Me.TextBoxDebug.Text & "   " & Me.DbCentral.EjecutaSqlScalar(SQL)




            SQL = "SELECT ASNT_F_ATOCAB AS F_REGISTRO,ASNT_F_VALOR F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS TIPO,"
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,ASNT_NOMBRE AS OBSERVACION"
            SQL += ",ASNT_CIF AS CIF,ASNT_AUXILIAR_STRING AS AUX1,ASNT_AUXILIAR_NUMERICO AS AUX2 FROM TP_ASNT "
            SQL += " WHERE ASNT_F_ATOCAB = '" & Me.DateTimePicker1.Value & "'"
            SQL += " AND TP_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TP_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            '            SQL += " AND ASNT_IMPRIMIR = 'SI'"
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"
            Me.DataGrid2.DataSource = Me.DbCentral.TraerDataset(SQL, "ASIENTO")
            Me.DataGrid2.DataMember = "ASIENTO"
            Me.ConfGrid()

            If Me.ListBoxDebug.Items.Count > 0 Then
                Me.ButtonIncidencias.Enabled = True
            End If


            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
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

            REPORT_SELECTION_FORMULA = "{TP_ASNT.ASNT_F_ATOCAB}=DATETIME(" & Format(Me.DateTimePicker1.Value, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += " AND {TP_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TP_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"


            Dim Form As New FormVisorCrystal("ASIENTO NEWPAGA.rpt", "Pagos  " & Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"), "", False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

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
                Me.mEmpNum = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 9)

                SQL = "SELECT NVL(PARA_FILE_SPYRO_PATH,'?') FROM TS_PARA "
                SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
                Me.TextBoxRutaFicheros.Text = Me.DbCentral.EjecutaSqlScalar(SQL)

            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub DataGrid2_Navigate(sender As Object, ne As NavigateEventArgs) Handles DataGrid2.Navigate

    End Sub

    Private Sub DataGrid2_CurrentCellChanged(sender As Object, e As EventArgs) Handles DataGrid2.CurrentCellChanged

    End Sub

    Private Sub ButtonUpdateFilePAth_Click(sender As Object, e As EventArgs) Handles ButtonUpdateFilePAth.Click
        Try
            Me.FolderBrowserDialog1.ShowDialog()
            If IsNothing(Me.FolderBrowserDialog1.SelectedPath) = False Then
                If Me.FolderBrowserDialog1.SelectedPath.Length > 0 Then
                    Me.TextBoxRutaFicheros.Text = Me.FolderBrowserDialog1.SelectedPath & "\"
                    SQL = "UPDATE TS_PARA SET PARA_FILE_SPYRO_PATH = '" & Me.TextBoxRutaFicheros.Text & "'"
                    SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                    SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                    SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum

                    Me.Cursor = Cursors.WaitCursor
                    Me.DbCentral.EjecutaSqlCommit(SQL)
                    Me.Cursor = Cursors.Default

                End If
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub DataGridHoteles_Navigate(sender As Object, ne As NavigateEventArgs) Handles DataGridHoteles.Navigate

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Me.FechasPosibles()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub FechasPosibles()
        Try
            Dim Texto As String = ""
            If HayRegistros = True Then
                Me.Cursor = Cursors.WaitCursor
                Me.DbLeeNewPaga = New C_DATOS.C_DatosOledb(Me.mStrConexionNewPaga)
                Me.DbLeeNewPaga.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

                SQL = "SELECT DISTINCT (MOVI_DAVA) AS MOVI_DAVA,COUNT(*) AS TOTAL  FROM TNPG_MOVI "
                SQL += " WHERE  TO_CHAR(MOVI_DAVA,'YYYY') = '" & Year(Me.DateTimePicker1.Value) & "'"
                SQL += " AND  TO_CHAR(MOVI_DAVA,'MM') = '" & Format(Me.DateTimePicker1.Value, "MM") & "'"

                SQL += " AND TRUNC(MOVI_DAVA) NOT IN (SELECT TRUNC(PARA_DATA) FROM TNPG_PARA) "
                SQL += " AND   TNPG_MOVI.FORN_INTE = '0'"
                SQL += " GROUP BY MOVI_DAVA "
                SQL += " ORDER BY MOVI_DAVA ASC"
                DbLeeNewPaga.TraerLector(SQL)
                While DbLeeNewPaga.mDbLector.Read
                    Texto += DbLeeNewPaga.mDbLector.Item("MOVI_DAVA") & " (" & DbLeeNewPaga.mDbLector.Item("TOTAL") & ")" & vbCrLf
                End While
                DbLeeNewPaga.mDbLector.Close()

                If Texto.Length > 0 Then
                    MsgBox(Texto, MsgBoxStyle.Information)
                End If

            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub FechaUltima()
        Try
            Dim Texto As String = ""
            If HayRegistros = True Then
                Me.Cursor = Cursors.WaitCursor
                Me.DbLeeNewPaga = New C_DATOS.C_DatosOledb(Me.mStrConexionNewPaga)
                Me.DbLeeNewPaga.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

                SQL = "SELECT  "
                SQL += "   MAX ( MOVI_DAVA ) AS MOVI_DAVA "
                SQL += "    "
                SQL += "FROM "
                SQL += "    TNPG_MOVI "
                SQL += "WHERE "
                SQL += "     "
                SQL += "     "
                SQL += "     TRUNC(MOVI_DAVA) NOT IN (SELECT TRUNC(PARA_DATA) FROM TNPG_PARA) "
                SQL += "ORDER BY "
                SQL += "    MOVI_DAVA ASC "


                Me.mRestultStr = DbLeeNewPaga.EjecutaSqlScalar(SQL)
                If Me.mRestultStr.Length > 0 Then
                    Me.DateTimePicker1.Value = Format(CDate(Me.mRestultStr), "dd/MM/yyyy")
                End If

            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub ButtonIncidencias_Click(sender As Object, e As EventArgs) Handles ButtonIncidencias.Click
        Try
            If Me.ListBoxDebug.Items.Count > 0 Then
                Dim buffer As New StringBuilder
                For i As Integer = 0 To Me.ListBoxDebug.Items.Count - 1
                    buffer.Append(Me.ListBoxDebug.Items(i).ToString)
                    buffer.Append(vbCrLf)
                Next

                My.Computer.Clipboard.SetText(buffer.ToString)
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class