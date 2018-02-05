Public Class FormParametrosNomina
    Dim MyIni As New cIniArray
    Dim DbLee As C_DATOS.C_DatosOledb
    Dim DbLeeAux As C_DATOS.C_DatosOledb
    Dim DbWrite As New C_DATOS.C_DatosOledb
    Private mParaEmpGrupoCod As String
    Private mParaEmpCod As String
    Private mParaEmpNum As Integer
    Dim SQL As String
    Private Sub FormParametrosAlmacen_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If IsNothing(Me.DbLee) = False Or Me.DbLee.EstadoConexion = ConnectionState.Open Then
                Me.DbLee.CerrarConexion()
            End If
            If IsNothing(Me.DbLeeAux) = False Or Me.DbLeeAux.EstadoConexion = ConnectionState.Open Then
                Me.DbLeeAux.CerrarConexion()
            End If

            If IsNothing(Me.DbWrite) = False Or Me.DbWrite.EstadoConexion = ConnectionState.Open Then
                Me.DbWrite.CerrarConexion()
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub FormParametrosAlmacen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Me.mParaEmpGrupoCod = MyIni.IniGet(Application.StartupPath & "\menu.ini", "PARAMETER", "PARA_EMPGRUPO_COD")
            Me.DbLee = New C_DATOS.C_DatosOledb(MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"))
            Me.DbLee.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbLeeAux = New C_DATOS.C_DatosOledb(MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"))
            Me.DbLeeAux.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


            Me.DbWrite = New C_DATOS.C_DatosOledb(MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"))
            Me.DbWrite.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.CargaCombos()

        Catch ex As Exception
            MsgBox(ex.Message)
            Me.Close()
        End Try
    End Sub
    Private Sub ComboBoxEmpCod_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxEmpCod.SelectionChangeCommitted
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.LimpiarControles()

            ' Mostrar Emp_cod
            SQL = "SELECT HOTEL_EMP_COD FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
            SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
            SQL += " AND HOTEL_DESCRIPCION = '" & Me.ComboBoxEmpCod.Text & "'"
            Me.mParaEmpCod = CStr(Me.DbLeeAux.EjecutaSqlScalar(SQL))
            Me.TextBoxEmpCod.Text = Me.mParaEmpCod


            ' averiguar numero de empresa
            SQL = "SELECT HOTEL_EMP_NUM FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
            SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
            SQL += " AND HOTEL_DESCRIPCION = '" & Me.ComboBoxEmpCod.Text & "'"
            Me.mParaEmpNum = CInt(Me.DbLeeAux.EjecutaSqlScalar(SQL))
            Me.TextBoxEmpNum.Text = Me.mParaEmpNum


            ' COPIAR ESAS RITUNAS de parametros front
            Me.MostrarDatosParametros()

            Me.Cursor = Cursors.Default
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ActualizaTS_PARA()

        Catch ex As Exception
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
#Region "RUTINAS"
    Private Sub CargaCombos()
        Try
            ' Emp grupo cod

            SQL = " SELECT DISTINCT PARA_EMPGRUPO_COD FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mParaEmpGrupoCod & "'"
            Me.DbLee.TraerDataset(SQL, "EMPGRUPOCOD")

            Dim NewRowA(0) As Object
            NewRowA(0) = "<Ninguno>"

            Me.DbLee.mDbDataset.Tables("EMPGRUPOCOD").LoadDataRow(NewRowA, False)
            With Me.ComboBoxGrupoCod
                .DataSource = Me.DbLee.mDbDataset.Tables("EMPGRUPOCOD")
                .ValueMember = "PARA_EMPGRUPO_COD"
                .DisplayMember = "PARA_EMPGRUPO_COD"
                '       .SelectedIndex = .Items.Count - 1
            End With
            Me.DbLee.mDbDataset = Nothing

            ' Emp cod
            SQL = " SELECT PARA_EMP_COD,HOTEL_DESCRIPCION FROM TH_PARA,TH_HOTEL WHERE "
            SQL += "PARA_EMPGRUPO_COD = HOTEL_EMPGRUPO_COD AND PARA_EMP_COD = HOTEL_EMP_COD"
            SQL += " AND PARA_EMPGRUPO_COD = '" & Me.mParaEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_NUM = HOTEL_EMP_NUM "
            SQL += " ORDER BY HOTEL_DESCRIPCION ASC"

            Me.DbLee.TraerDataset(SQL, "EMPCOD")

            Dim NewRowB(0) As Object
            NewRowB(0) = "<Ninguno>"

            Me.DbLee.mDbDataset.Tables("EMPCOD").LoadDataRow(NewRowB, False)
            With Me.ComboBoxEmpCod
                .DataSource = Me.DbLee.mDbDataset.Tables("EMPCOD")
                .ValueMember = "PARA_EMP_COD"
                .DisplayMember = "HOTEL_DESCRIPCION"
                '       .SelectedIndex = .Items.Count - 1
            End With
            Me.DbLee.mDbDataset = Nothing


            ' Escoger el primer elemento en ambos combos
            If IsNothing(Me.ComboBoxGrupoCod.Items.Count) = False Then
                Me.ComboBoxGrupoCod.SelectedIndex = 0
            End If

            If IsNothing(Me.ComboBoxEmpCod.Items.Count) = False Then
                Me.ComboBoxEmpCod.SelectedIndex = 0

                ' MOSTRAR LOS DATOS 
                Me.Cursor = Cursors.WaitCursor
                Me.LimpiarControles()

                ' Mostrar Emp_cod
                SQL = "SELECT HOTEL_EMP_COD FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
                SQL += " AND HOTEL_DESCRIPCION = '" & Me.ComboBoxEmpCod.Text & "'"
                Me.mParaEmpCod = CStr(Me.DbLeeAux.EjecutaSqlScalar(SQL))
                Me.TextBoxEmpCod.Text = Me.mParaEmpCod


                ' averiguar numero de empresa
                SQL = "SELECT HOTEL_EMP_NUM FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
                SQL += " AND HOTEL_DESCRIPCION = '" & Me.ComboBoxEmpCod.Text & "'"
                Me.mParaEmpNum = CInt(Me.DbLeeAux.EjecutaSqlScalar(SQL))
                Me.TextBoxEmpNum.Text = Me.mParaEmpNum


                ' COPIAR ESAS RITUNAS de parametros front
                Me.MostrarDatosParametros()

                Me.Cursor = Cursors.Default
            End If



        Catch EX As Exception
            MsgBox(EX.Message)
        End Try
    End Sub
    Private Sub LimpiarControles()
        Dim C As Control
        For Each C In Me.Controls
            If TypeOf C Is TextBox Then
                C.Text = ""
                C.Update()
            End If
        Next

        For Each C In Me.TabControl1.Controls
            If TypeOf C Is TextBox Then
                C.Text = ""
                C.Update()
            End If
        Next
        For Each C In Me.TabPageHotelesLopez.Controls
            If TypeOf C Is TextBox Then
                C.Text = ""
                C.Update()
            End If
        Next
    End Sub

    Private Sub MostrarDatosParametros()
        Try
            SQL = "SELECT NVL(PARA_FILE_ORIGEN,'<Ninguno>') AS PARA_FILE_ORIGEN,"
            SQL += "      NVL(PARA_FILE_DESTINO,'<Ninguno>') AS PARA_FILE_DESTINO,"
            SQL += "      NVL(PARA_CFATODIARI_COD,'<?>') AS PARA_CFATODIARI_COD,"
            SQL += "      NVL(PARA_CTA_BRUTO,'<Ninguno>') AS PARA_CTA_BRUTO,"
            SQL += "      NVL(PARA_CTA_INDEM,'<Ninguno>') AS PARA_CTA_INDEM,"
            SQL += "      NVL(PARA_CTA_SSEMPRE,'<Ninguno>') AS PARA_CTA_SSEMPRE,"
            SQL += "      NVL(PARA_CTA_NETO,'<Ninguno>') AS PARA_CTA_NETO,"

            SQL += "      NVL(PARA_CTA_IRPF,'<Ninguno>') AS PARA_CTA_IRPF,"
            SQL += "      NVL(PARA_CTA_SSTOTAL,'<Ninguno>') AS PARA_CTA_SSTOTAL,"
            SQL += "      NVL(PARA_CTA_755PERSO,'<Ninguno>') AS PARA_CTA_755PERSO,"
            SQL += "      NVL(PARA_CTA_EMBCUOTA,'<Ninguno>') AS PARA_CTA_EMBCUOTA,"
            SQL += "      NVL(PARA_CTA_ANTIPREST,'<Ninguno>') AS PARA_CTA_ANTIPREST,"
            SQL += "      NVL(PARA_CTA_OTROS,'<Ninguno>') AS PARA_CTA_OTROS, "
            SQL += "      NVL(PARA_CFATOTIP_COD,'<?>') AS PARA_CFATOTIP_COD"


            SQL += "  FROM TN_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mParaEmpNum

            Me.DbLee.TraerLector(SQL)

            Me.DbLee.mDbLector.Read()

            If Me.DbLee.mDbLector.HasRows Then

                Me.TextBoxFileOrigen.Text = Me.DbLee.mDbLector.Item("PARA_FILE_ORIGEN")
                Me.TextBoxFileDestino.Text = Me.DbLee.mDbLector.Item("PARA_FILE_DESTINO")
                Me.TextBoxCtaBruto.Text = Me.DbLee.mDbLector.Item("PARA_CTA_BRUTO")
                Me.TextBoxCtaIndemniza.Text = Me.DbLee.mDbLector.Item("PARA_CTA_INDEM")

                Me.TextBoxCtaSocialEmpresa.Text = Me.DbLee.mDbLector.Item("PARA_CTA_SSEMPRE")
                Me.TextBoxCtaNeto.Text = Me.DbLee.mDbLector.Item("PARA_CTA_NETO")
                Me.TextBoxCtaIrpf.Text = Me.DbLee.mDbLector.Item("PARA_CTA_IRPF")
                Me.TextBoxCtaSocialTotal.Text = Me.DbLee.mDbLector.Item("PARA_CTA_SSTOTAL")
                Me.TextBoxCtaSocialPersonal.Text = Me.DbLee.mDbLector.Item("PARA_CTA_755PERSO")
                Me.TextBoxCtaEmbargos.Text = Me.DbLee.mDbLector.Item("PARA_CTA_EMBCUOTA")

                Me.TextBoxCtaAnticipos.Text = Me.DbLee.mDbLector.Item("PARA_CTA_ANTIPREST")
                Me.TextBoxCtaOtrosDtos.Text = Me.DbLee.mDbLector.Item("PARA_CTA_OTROS")


                Me.TextBoxCfatoDiariCod.Text = Me.DbLee.mDbLector.Item("PARA_CFATODIARI_COD")
                Me.TextBoxCfatoTipCod.Text = Me.DbLee.mDbLector.Item("PARA_CFATOTIP_COD")


            Else
                MsgBox("Atención NO existen Registros de Parámeros TN_PARA para Mostrar", MsgBoxStyle.Critical, "Atención")
                Me.Close()

            End If


            Me.DbLee.mDbLector.Close()

            ' PONER EN ROJO NULOS
            Dim MyControl As Control
            For Each MyControl In Me.TabControl1.Controls
                If TypeOf MyControl Is TextBox Then
                    If MyControl.Text = "<Ninguno>" Then
                        MyControl.ForeColor = Color.Maroon
                        MyControl.Update()
                    Else
                        MyControl.ForeColor = Color.Black
                        MyControl.Update()
                    End If
                End If
            Next MyControl

        Catch EX As Exception
            Me.DbLee.mDbLector.Close()
            MsgBox(EX.Message, MsgBoxStyle.Information, "Mostrar Datos")
        End Try
    End Sub
    Private Sub ActualizaTS_PARA()
        Try
            SQL = "UPDATE TN_PARA "
            SQL += "SET "

            SQL += " PARA_CFATODIARI_COD='" & Me.TextBoxCfatoDiariCod.Text.Replace("<?>", "") & "'"
            SQL += ",PARA_FILE_ORIGEN='" & Me.TextBoxFileOrigen.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_FILE_DESTINO='" & Me.TextBoxFileDestino.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_BRUTO='" & Me.TextBoxCtaBruto.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_INDEM='" & Me.TextBoxCtaIndemniza.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_SSEMPRE='" & Me.TextBoxCtaSocialEmpresa.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_NETO='" & Me.TextBoxCtaNeto.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_IRPF='" & Me.TextBoxCtaIrpf.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_SSTOTAL='" & Me.TextBoxCtaSocialTotal.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_755PERSO='" & Me.TextBoxCtaSocialPersonal.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_EMBCUOTA='" & Me.TextBoxCtaEmbargos.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_ANTIPREST='" & Me.TextBoxCtaAnticipos.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_OTROS='" & Me.TextBoxCtaOtrosDtos.Text.Replace("<Ninguno>", "") & "'"

            SQL += " ,PARA_CFATOTIP_COD='" & Me.TextBoxCfatoTipCod.Text.Replace("<?>", "") & "'"


            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mParaEmpNum




            ' UPDATE
            Me.Cursor = Cursors.WaitCursor
            Me.DbWrite.EjecutaSqlCommit(SQL)
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Update TS_PARA")

        End Try
    End Sub
#End Region

    Private Sub ButtonFileOrigen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFileOrigen.Click
        Try
            ' Me.FolderBrowserDialog1.RootFolder = Environment.SpecialFolder.Personal

            Me.FolderBrowserDialog1.ShowNewFolderButton = True

            If Me.FolderBrowserDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

                If Me.FolderBrowserDialog1.SelectedPath.Length > 0 Then
                    Me.TextBoxFileOrigen.Text = Me.FolderBrowserDialog1.SelectedPath
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonFileDestino_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFileDestino.Click
        Try
            '      Me.FolderBrowserDialog1.RootFolder = Environment.SpecialFolder.Personal
            Me.FolderBrowserDialog1.ShowNewFolderButton = True

            If Me.FolderBrowserDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

                If Me.FolderBrowserDialog1.SelectedPath.Length > 0 Then
                    Me.TextBoxFileDestino.Text = Me.FolderBrowserDialog1.SelectedPath
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class