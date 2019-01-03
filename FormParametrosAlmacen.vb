Public Class FormParametrosAlmacen
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

            If Me.CheckBoxTipoFormalizaAlbaranes.Checked Then
                Me.TextBoxCtaAlbaranesPdtesFormalizar.Enabled = True
                Me.TextBoxCtaAlbaranesPdtesFormalizar.Update()
            Else
                Me.TextBoxCtaAlbaranesPdtesFormalizar.Enabled = False
                Me.TextBoxCtaAlbaranesPdtesFormalizar.Update()
            End If
            Me.CargaCombos()

        Catch ex As Exception
            MsgBox(ex.Message)
            Me.Close()
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
            SQL = "SELECT NVL(PARA_CTA_RAIZ4_GASTO,'<Ninguno>') AS PARA_CTA_RAIZ4_GASTO,"
            SQL += "      NVL(PARA_CTA_RAIZ9_GASTO,'<Ninguno>') AS PARA_CTA_RAIZ9_GASTO,"
            SQL += "      NVL(PARA_CTA_RAIZ4_ACTI,'<Ninguno>') AS PARA_CTA_RAIZ4_ACTI,"
            SQL += "      NVL(PARA_CTA_RAIZ9_ACTI,'<Ninguno>') AS PARA_CTA_RAIZ9_ACTI,"
            SQL += "      NVL(PARA_CTA_RAIZ4_CENTRO,'<Ninguno>') AS PARA_CTA_RAIZ4_CENTRO,"
            SQL += "      NVL(PARA_CTA_RAIZ9_CENTRO,'<Ninguno>') AS PARA_CTA_RAIZ9_CENTRO,"

            SQL += "      NVL(PARA_CONECTA_NEWPAGA,0) AS PARA_CONECTA_NEWPAGA,"
            SQL += "      NVL(PARA_CUENTAS_NEWCENTRAL,0) AS PARA_CUENTAS_NEWCENTRAL,"

            SQL += "      NVL(PARA_SOLO_FACTURAS,0) AS PARA_SOLO_FACTURAS,"

            SQL += "      NVL(PARA_DEBE_FAC,'<Ninguno>') AS PARA_DEBE_FAC,"
            SQL += "      NVL(PARA_HABER_FAC,'<Ninguno>') AS PARA_HABER_FAC,"
            SQL += "      NVL(PARA_DEBE_ABONO,'<Ninguno>') AS PARA_DEBE_ABONO,"
            SQL += "      NVL(PARA_HABER_ABONO,'<Ninguno>') AS PARA_HABER_ABONO,"



            SQL += "      NVL(PARA_SERIE_FAC_SPYRO,'C') AS PARA_SERIE_FAC_SPYRO,"
            SQL += "      NVL(PARA_SERIE_FAC2_SPYRO,'C2') AS PARA_SERIE_FAC2_SPYRO,"

            SQL += "      NVL(PARA_SERIE_MAS_DPTO,0) AS PARA_SERIE_MAS_DPTO"
            SQL += "      ,NVL(PARA_SERIE_ANIO_2B,0) AS PARA_SERIE_ANIO_2B"


            SQL += "      ,NVL(PARA_SERIE_ANIO_2B,0) AS PARA_SERIE_ANIO_2B"

            SQL += "      ,NVL(PARA_CTA1,'<Ninguno>')  AS PARA_CTA1"
            SQL += "      ,NVL(PARA_TIPO_FORMALIZA,'G') AS PARA_TIPO_FORMALIZA "

            SQL += "      ,NVL(PARA_SPYRO_TIPO_ANALITICA,'<Ninguno>') AS PARA_SPYRO_TIPO_ANALITICA"



            SQL += "  FROM TS_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mParaEmpNum

            Me.DbLee.TraerLector(SQL)

            Me.DbLee.mDbLector.Read()

            If Me.DbLee.mDbLector.HasRows Then
                Me.TextBoxCtaRaiz4.Text = Me.DbLee.mDbLector.Item("PARA_CTA_RAIZ4_GASTO")
                Me.TextBoxCtaRaiz6.Text = Me.DbLee.mDbLector.Item("PARA_CTA_RAIZ9_GASTO")
                Me.TextBoxCtaActividad4.Text = Me.DbLee.mDbLector.Item("PARA_CTA_RAIZ4_ACTI")
                Me.TextBoxCtaActividad6.Text = Me.DbLee.mDbLector.Item("PARA_CTA_RAIZ9_ACTI")
                Me.TextBoxCtaCentro4.Text = Me.DbLee.mDbLector.Item("PARA_CTA_RAIZ4_CENTRO")
                Me.TextBoxCtaCentro6.Text = Me.DbLee.mDbLector.Item("PARA_CTA_RAIZ9_CENTRO")

                If CInt(Me.DbLee.mDbLector.Item("PARA_CONECTA_NEWPAGA")) = 1 Then
                    Me.CheckBoxUsaNewPaga.Checked = True
                Else
                    Me.CheckBoxUsaNewPaga.Checked = False
                End If

                If CInt(Me.DbLee.mDbLector.Item("PARA_SOLO_FACTURAS")) = 1 Then
                    Me.CheckBoxSoloFacturas.Checked = True
                Else
                    Me.CheckBoxSoloFacturas.Checked = False
                End If

                If CInt(Me.DbLee.mDbLector.Item("PARA_CUENTAS_NEWCENTRAL")) = 1 Then
                    Me.CheckBoxBuscaCuentasNewCentral.Checked = True
                Else
                    Me.CheckBoxBuscaCuentasNewCentral.Checked = False
                End If



                Me.TextBoxDebeFac.Text = Me.DbLee.mDbLector.Item("PARA_DEBE_FAC")
                Me.TextBoxHaberFac.Text = Me.DbLee.mDbLector.Item("PARA_HABER_FAC")
                Me.TextBoxDebeAbonos.Text = Me.DbLee.mDbLector.Item("PARA_DEBE_ABONO")
                Me.TextBoxHaberAbonos.Text = Me.DbLee.mDbLector.Item("PARA_HABER_ABONO")

                '  Me.TextBoxHotelId.Text = Me.DbLee.mDbLector.Item("PARA_HOTEL_AX")


                '' SERIES DE FACTURAS DE COMPRA GENERICAS 

                Me.TextBoxSerieFacturaNewStock.Text = CStr(Me.DbLee.mDbLector.Item("PARA_SERIE_FAC_SPYRO"))
                Me.TextBoxSerieFacturaNewPaga.Text = CStr(Me.DbLee.mDbLector.Item("PARA_SERIE_FAC2_SPYRO"))




                '' SERIES DE FACTURAS DE COMPRA  POR DEPARTAMENTO
                If CInt(Me.DbLee.mDbLector.Item("PARA_SERIE_MAS_DPTO")) = 1 Then
                    Me.CheckBoxSerieFacturaPorDepartamento.Checked = True
                Else
                    Me.CheckBoxSerieFacturaPorDepartamento.Checked = False
                End If

                '' SERIES DE FACTURAS numero de digitos para el anio
                If CInt(Me.DbLee.mDbLector.Item("PARA_SERIE_ANIO_2B")) = 1 Then
                    Me.CheckBoxSerieFacturaDigitosAnio.Checked = True
                Else
                    Me.CheckBoxSerieFacturaDigitosAnio.Checked = False
                End If


                If CStr(Me.DbLee.mDbLector.Item("PARA_TIPO_FORMALIZA")) = "G" Then
                    Me.CheckBoxTipoFormalizaAlbaranes.Checked = True
                Else
                    Me.CheckBoxTipoFormalizaAlbaranes.Checked = False
                End If

                Me.TextBoxCtaAlbaranesPdtesFormalizar.Text = Me.DbLee.mDbLector.Item("PARA_CTA1")

                Me.TextBoxSpyroTipoAnalitica.Text = Me.DbLee.mDbLector.Item("PARA_SPYRO_TIPO_ANALITICA")



            Else
                MsgBox("Atención NO existen Registros de Parámeros TS_PARA para Mostrar", MsgBoxStyle.Critical, "Atención")
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
            SQL = "UPDATE TS_PARA "
            SQL += "SET "

            SQL += " PARA_CTA_RAIZ4_GASTO='" & Me.TextBoxCtaRaiz4.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_RAIZ9_GASTO='" & Me.TextBoxCtaRaiz6.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_RAIZ4_ACTI='" & Me.TextBoxCtaActividad4.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_RAIZ9_ACTI='" & Me.TextBoxCtaActividad6.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_RAIZ4_CENTRO='" & Me.TextBoxCtaCentro4.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_RAIZ9_CENTRO='" & Me.TextBoxCtaCentro6.Text.Replace("<Ninguno>", "") & "'"


            If CheckBoxUsaNewPaga.Checked Then
                SQL += ",PARA_CONECTA_NEWPAGA = 1"
            Else
                SQL += ",PARA_CONECTA_NEWPAGA = 0"
            End If

            If CheckBoxSoloFacturas.Checked Then
                SQL += ",PARA_SOLO_FACTURAS = 1"
            Else
                SQL += ",PARA_SOLO_FACTURAS = 0"
            End If

            If CheckBoxBuscaCuentasNewCentral.Checked Then
                SQL += ",PARA_CUENTAS_NEWCENTRAL = 1"
            Else
                SQL += ",PARA_CUENTAS_NEWCENTRAL = 0"
            End If

            SQL += ",PARA_DEBE_FAC='" & Me.TextBoxDebeFac.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_HABER_FAC='" & Me.TextBoxHaberFac.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_DEBE_ABONO='" & Me.TextBoxDebeAbonos.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_HABER_ABONO='" & Me.TextBoxHaberAbonos.Text.Replace("<Ninguno>", "") & "'"

            '   If Me.TextBoxHotelId.TextLength = 0 Then
            'SQL += ",PARA_HOTEL_AX = 0"
            '  Else
            ' SQL += ",PARA_HOTEL_AX = " & Me.TextBoxHotelId.Text
            ' End If



            SQL += ",PARA_SERIE_FAC_SPYRO = '" & Me.TextBoxSerieFacturaNewStock.Text & "'"
            SQL += ",PARA_SERIE_FAC2_SPYRO = '" & Me.TextBoxSerieFacturaNewPaga.Text & "'"


            If Me.CheckBoxSerieFacturaPorDepartamento.Checked Then
                SQL += ",PARA_SERIE_MAS_DPTO = 1"
            Else
                SQL += ",PARA_SERIE_MAS_DPTO = 0"
            End If



            If Me.CheckBoxSerieFacturaDigitosAnio.Checked Then
                SQL += ",PARA_SERIE_ANIO_2B = 1"
            Else
                SQL += ",PARA_SERIE_ANIO_2B = 0"
            End If


            If CheckBoxTipoFormalizaAlbaranes.Checked Then
                SQL += ",PARA_CTA1 = '" & Me.TextBoxCtaAlbaranesPdtesFormalizar.Text & "'"
                SQL += ",PARA_TIPO_FORMALIZA = '" & "G" & "'"
            Else
                SQL += ",PARA_CTA1 = '" & Me.TextBoxCtaAlbaranesPdtesFormalizar.Text & "'"
                SQL += ",PARA_TIPO_FORMALIZA = '" & "P" & "'"
            End If


            SQL += ",PARA_SPYRO_TIPO_ANALITICA='" & Me.TextBoxSpyroTipoAnalitica.Text.Replace("<Ninguno>", "") & "'"



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

   
    Private Sub ComboBoxEmpCod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEmpCod.SelectedIndexChanged

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


    Private Sub TextBoxHotelId_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxHotelId.Validated
        Try
            If Me.TextBoxHotelId.TextLength > 0 Then
                If IsNumeric(Me.TextBoxHotelId.Text) = False Then
                    Me.TextBoxHotelId.Text = ""
                    MsgBox("Hotel Id , debe de ser numérico", MsgBoxStyle.Information, "Atención")
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonCancelar_Click(sender As Object, e As EventArgs) Handles ButtonCancelar.Click
        Try
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CheckBoxTipoFormalizaAlbaranes_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxTipoFormalizaAlbaranes.CheckedChanged
        Try
            If Me.CheckBoxTipoFormalizaAlbaranes.Checked Then
                Me.TextBoxCtaAlbaranesPdtesFormalizar.Enabled = True
                Me.TextBoxCtaAlbaranesPdtesFormalizar.Update()
                Exit Sub
            Else
                Me.TextBoxCtaAlbaranesPdtesFormalizar.Enabled = False
                Me.TextBoxCtaAlbaranesPdtesFormalizar.Update()
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


End Class