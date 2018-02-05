Public Class FormInsertaEmpresa
    Dim DbLee As C_DATOS.C_DatosOledb
    Dim DbWrite As C_DATOS.C_DatosOledb
    Dim SQL As String
    Dim Control As Integer
    Dim AuxEscalar As String
    Private Sub FormInsertaEmpresa_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            Me.DbLee = New C_DATOS.C_DatosOledb(STRCONEXIONCENTRAL)
            Me.DbWrite = New C_DATOS.C_DatosOledb(STRCONEXIONCENTRAL)
            If Me.DbLee.EstadoConexion <> ConnectionState.Open Then
                Me.Close()
            End If


            Me.DbLee.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")
            Me.DbWrite.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.CargaCombos()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub FormInsertaEmpresa_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If IsNothing(Me.DbLee) = False Or Me.DbLee.EstadoConexion = ConnectionState.Open Then
                Me.DbLee.CerrarConexion()
            End If
            If IsNothing(Me.DbWrite) = False Or Me.DbWrite.EstadoConexion = ConnectionState.Open Then
                Me.DbWrite.CerrarConexion()
            End If
        Catch ex As Exception

        End Try
    End Sub
#Region "RUTINAS"
    Private Sub CargaCombos()
        Try
            ' Emp grupo cod

            SQL = " SELECT DISTINCT PARA_EMPGRUPO_COD FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & EMPGRUPO_COD & "'"
            Me.DbLee.TraerDataset(SQL, "EMPGRUPOCOD")

            With Me.ComboBoxGrupoCod
                .DataSource = Me.DbLee.mDbDataset.Tables("EMPGRUPOCOD")
                .ValueMember = "PARA_EMPGRUPO_COD"
                .DisplayMember = "PARA_EMPGRUPO_COD"
                .SelectedIndex = .Items.Count - 1
            End With
            Me.DbLee.mDbDataset = Nothing



        Catch EX As Exception
            MsgBox(EX.Message)
        End Try
    End Sub
    Private Sub InsertaTH_PARA()
        Try



            If Me.TextBoxEmpCod.Text.Length = 0 Or Me.TextBoxDescripcion.Text.Length = 0 Then
                MsgBox("Revise Valores Vacios ", MsgBoxStyle.Information, "Atención")
                Exit Sub
            End If


            Me.DbLee.IniciaTransaccion()

            Me.DbLee.EjecutaSqlScalar("LOCK TABLE TH_PARA IN EXCLUSIVE MODE")

            SQL = "SELECT MAX(NVL(PARA_EMP_NUM,0)) + 1 AS CONTROL  FROM TH_PARA"
            SQL += " WHERE  PARA_EMPGRUPO_COD            = '" & Me.ComboBoxGrupoCod.SelectedValue & "'"
            SQL += " AND    PARA_EMP_COD                 = '" & Me.TextBoxEmpCod.Text & "'"

            Me.AuxEscalar = Me.DbLee.EjecutaSqlScalar(SQL)


            If Me.AuxEscalar <> "" Then
                Control = CInt(Me.AuxEscalar)
            Else
                Control = 0
            End If



            SQL = "INSERT INTO TH_PARA(PARA_EMPGRUPO_COD,PARA_EMP_COD,PARA_EMP_NUM) VALUES ('"
            SQL += Me.ComboBoxGrupoCod.Text & "','"
            SQL += Me.TextBoxEmpCod.Text & "',"
            SQL += Control & ")"

            Me.DbLee.EjecutaSql(SQL)


            SQL = "INSERT INTO TC_PARA(PARA_EMPGRUPO_COD,PARA_EMP_COD,PARA_EMP_NUM) VALUES ('"
            SQL += Me.ComboBoxGrupoCod.Text & "','"
            SQL += Me.TextBoxEmpCod.Text & "',"
            SQL += Control & ")"

            Me.DbLee.EjecutaSql(SQL)

            SQL = "INSERT INTO TS_PARA(PARA_EMPGRUPO_COD,PARA_EMP_COD,PARA_EMP_NUM) VALUES ('"
            SQL += Me.ComboBoxGrupoCod.Text & "','"
            SQL += Me.TextBoxEmpCod.Text & "',"
            SQL += Control & ")"

            Me.DbLee.EjecutaSql(SQL)

            SQL = "INSERT INTO TN_PARA(PARA_EMPGRUPO_COD,PARA_EMP_COD,PARA_EMP_NUM) VALUES ('"
            SQL += Me.ComboBoxGrupoCod.Text & "','"
            SQL += Me.TextBoxEmpCod.Text & "',"
            SQL += Control & ")"

            Me.DbLee.EjecutaSql(SQL)


            SQL = "INSERT INTO TH_HOTEL(HOTEL_EMPGRUPO_COD,HOTEL_EMP_COD,HOTEL_EMP_NUM,HOTEL_DESCRIPCION) VALUES ('"
            SQL += Me.ComboBoxGrupoCod.Text & "','"
            SQL += Me.TextBoxEmpCod.Text & "',"
            SQL += Control & ",'"
            SQL += Me.TextBoxDescripcion.Text.ToUpper & "')"

            Me.DbLee.EjecutaSql(SQL)

            If Me.DbLee.StrError = "" Then
                Me.DbLee.ConfirmaTransaccion()
            Else
                Me.DbLee.CancelaTransaccion()
            End If





        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub UpdateTH_PARA()
        Try

            SQL = "SELECT * FROM TH_PARA WHERE PARA_EMPGRUPO_COD ='" & Me.ComboBoxGrupoCod.SelectedValue & "'"
            SQL += "AND ROWNUM = 1 "
            '    SQL += " ORDER BY PARA_EMPGRUPO_COD,PARA_EMP_COD,PARA_EMP_NUM ASC"
            ' La empresa mas vieja 
            SQL += " ORDER BY ROWID ASC"

            Me.DbLee.TraerLector(SQL)
            Me.DbLee.mDbLector.Read()

            If Me.DbLee.mDbLector.HasRows Then
                SQL = "UPDATE TH_PARA "
                SQL += "SET    "
                SQL += "       PARA_CTA1                    =  '" & Me.DbLee.mDbLector.Item("PARA_CTA1") & "',"
                SQL += "       PARA_CTA2                    =  '" & Me.DbLee.mDbLector.Item("PARA_CTA2") & "',"
                SQL += "       PARA_CTA3                    =  '" & Me.DbLee.mDbLector.Item("PARA_CTA3") & "',"
                SQL += "       PARA_CTA4                    =  '" & Me.DbLee.mDbLector.Item("PARA_CTA4") & "',"
                SQL += "       PARA_CTA5                    =  '" & Me.DbLee.mDbLector.Item("PARA_CTA5") & "',"
                SQL += "       PARA_CFIVALIBRO_COD          =  '" & Me.DbLee.mDbLector.Item("PARA_CFIVALIBRO_COD") & "',"
                SQL += "       PARA_CFIVACLASE_COD          =  '" & Me.DbLee.mDbLector.Item("PARA_CFIVACLASE_COD") & "',"
                SQL += "       PARA_MONEDAS_COD             =  '" & Me.DbLee.mDbLector.Item("PARA_MONEDAS_COD") & "',"
                SQL += "       PARA_CFATODIARI_COD          =  '" & Me.DbLee.mDbLector.Item("PARA_CFATODIARI_COD") & "',"
                SQL += "       PARA_CFIVATIMPU_COD          =  '" & Me.DbLee.mDbLector.Item("PARA_CFIVATIMPU_COD") & "',"
                SQL += "       PARA_CFIVATIP_COD            =  '" & Me.DbLee.mDbLector.Item("PARA_CFIVATIP_COD") & "',"
                SQL += "       PARA_CFATOTIP_COD            =  '" & Me.DbLee.mDbLector.Item("PARA_CFATOTIP_COD") & "',"
                SQL += "       PARA_GVAGENTE_COD            =  '" & Me.DbLee.mDbLector.Item("PARA_GVAGENTE_COD") & "',"
                SQL += "       PARA_DEBE                    =  '" & Me.DbLee.mDbLector.Item("PARA_DEBE") & "',"
                SQL += "       PARA_HABER                   =  '" & Me.DbLee.mDbLector.Item("PARA_HABER") & "',"
                SQL += "       PARA_DEBE_FAC                =  '" & Me.DbLee.mDbLector.Item("PARA_DEBE_FAC") & "',"
                SQL += "       PARA_HABER_FAC               =  '" & Me.DbLee.mDbLector.Item("PARA_HABER_FAC") & "',"
                SQL += "       PARA_CLIENTES_CONTADO        =  '" & Me.DbLee.mDbLector.Item("PARA_CLIENTES_CONTADO") & "',"
                SQL += "       PARA_CLIENTES_CONTADO_CIF    =  '" & Me.DbLee.mDbLector.Item("PARA_CLIENTES_CONTADO_CIF") & "',"
                '    SQL += "       PARA_FILE_SPYRO_PATH         =  '" & Me.DbLee.mDbLector.Item("PARA_FILE_SPYRO_PATH") & "',"
                SQL += "       PARA_CTA_REDONDEO            =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_REDONDEO") & "',"
                SQL += "       PARA_FECHA_REGISTRO_AC       =  '" & Me.DbLee.mDbLector.Item("PARA_FECHA_REGISTRO_AC") & "',"
                SQL += "       PARA_FECHA_EXE               =  '" & Me.DbLee.mDbLector.Item("PARA_FECHA_EXE") & "',"
                SQL += "       PARA_FECHA_BASE              =  '" & Me.DbLee.mDbLector.Item("PARA_FECHA_BASE") & "',"
                SQL += "       PARA_TEXTO_IVA               =  '" & Me.DbLee.mDbLector.Item("PARA_TEXTO_IVA") & "',"
                SQL += "       PARA_SERIE_ANULACION         =  '" & Me.DbLee.mDbLector.Item("PARA_SERIE_ANULACION") & "',"
                SQL += "       PARA_CENTRO_COSTO_AL         =  '" & Me.DbLee.mDbLector.Item("PARA_CENTRO_COSTO_AL") & "',"
                SQL += "       PARA_CHARTOOEM               =  '" & Me.DbLee.mDbLector.Item("PARA_CHARTOOEM") & "',"
                SQL += "       PARA_COMISIONES              =  '" & Me.DbLee.mDbLector.Item("PARA_COMISIONES") & "',"
                SQL += "       PARA_DEBUG                   =  '" & Me.DbLee.mDbLector.Item("PARA_DEBUG") & "',"
                SQL += "       PARA_SANTANACAZORLA          =  '" & Me.DbLee.mDbLector.Item("PARA_SANTANACAZORLA") & "',"
                SQL += "       PARA_TIPO_ANULACION          =  '" & Me.DbLee.mDbLector.Item("PARA_TIPO_ANULACION") & "',"
                SQL += "       PARA_MULTIHOTEL              =  '" & Me.DbLee.mDbLector.Item("PARA_MULTIHOTEL") & "',"
                SQL += "       PARA_VALIDA_SPYRO            =  '" & Me.DbLee.mDbLector.Item("PARA_VALIDA_SPYRO") & "',"
                SQL += "       PARA_ENLAZA_NEWCONTA         =  '" & Me.DbLee.mDbLector.Item("PARA_ENLAZA_NEWCONTA") & "',"
                SQL += "       PARA_USA_MANOCORRIENTE       =  '" & Me.DbLee.mDbLector.Item("PARA_USA_MANOCORRIENTE") & "',"
                SQL += "       PARA_USA_CTACOMISION         =  '" & Me.DbLee.mDbLector.Item("PARA_USA_CTACOMISION") & "',"
                SQL += "       PARA_CTA_SERIE_FAC           =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_SERIE_FAC") & "',"
                SQL += "       PARA_CTA_SERIE_ANUL          =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_SERIE_ANUL") & "',"
                SQL += "       PARA_CTA_SERIE_NOTAS         =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_SERIE_NOTAS") & "',"
                SQL += "       PARA_INGRESO_POR_HABITACION  =  '" & Me.DbLee.mDbLector.Item("PARA_INGRESO_POR_HABITACION") & "',"
                SQL += "       PARA_INGRESO_HABITACION_DPTO =  '" & Me.DbLee.mDbLector.Item("PARA_INGRESO_HABITACION_DPTO") & "',"
                SQL += "       PARA_CONECTA_NEWGOLF         =  '" & Me.DbLee.mDbLector.Item("PARA_CONECTA_NEWGOLF") & "',"
                SQL += "       PARA_FILE_PREFIJO            =  '" & Me.DbLee.mDbLector.Item("PARA_FILE_PREFIJO") & "',"
                SQL += "       PARA_FILE_PREFIJO_IGIC       =  '" & Me.DbLee.mDbLector.Item("PARA_FILE_PREFIJO_IGIC") & "',"
                SQL += "       PARA_USUARIO_NEWGOLF         =  '" & Me.DbLee.mDbLector.Item("PARA_USUARIO_NEWGOLF") & "',"
                SQL += "       PARA_CLIENTES_CONTADO_CODIGO =  '" & Me.DbLee.mDbLector.Item("PARA_CLIENTES_CONTADO_CODIGO") & "',"
                SQL += "       PARA_BCTA1                   =  '" & Me.DbLee.mDbLector.Item("PARA_BCTA1") & "',"
                SQL += "       PARA_BCTA2                   =  '" & Me.DbLee.mDbLector.Item("PARA_BCTA2") & "',"
                SQL += "       PARA_BCTA3                   =  '" & Me.DbLee.mDbLector.Item("PARA_BCTA3") & "',"
                SQL += "       PARA_BCTA4                   =  '" & Me.DbLee.mDbLector.Item("PARA_BCTA4") & "',"
                SQL += "       PARA_BCTA5                   =  '" & Me.DbLee.mDbLector.Item("PARA_BCTA5") & "',"
                SQL += "       PARA_BCTA6                   =  '" & Me.DbLee.mDbLector.Item("PARA_BCTA6") & "',"
                SQL += "       PARA_BCTA7                   =  '" & Me.DbLee.mDbLector.Item("PARA_BCTA7") & "',"
                SQL += "       PARA_BCTA8                   =  '" & Me.DbLee.mDbLector.Item("PARA_BCTA8") & "',"
                SQL += "       PARA_BCTA9                   =  '" & Me.DbLee.mDbLector.Item("PARA_BCTA9") & "',"
                SQL += "       PARA_BCTA10                  =  '" & Me.DbLee.mDbLector.Item("PARA_BCTA10") & "',"
                SQL += "       PARA_BCTA11                  =  '" & Me.DbLee.mDbLector.Item("PARA_BCTA11") & "',"
                SQL += "       PARA_BCTA12                  =  '" & Me.DbLee.mDbLector.Item("PARA_BCTA12") & "',"
                SQL += "       PARA_COMISION_BONOS_ASOC     =  '" & Me.DbLee.mDbLector.Item("PARA_COMISION_BONOS_ASOC") & "',"
                SQL += "       TIAD_CODI                    =  '" & Me.DbLee.mDbLector.Item("TIAD_CODI") & "',"
                SQL += "       PARA_CENTRO_COSTO_COMI       =  '" & Me.DbLee.mDbLector.Item("PARA_CENTRO_COSTO_COMI") & "',"
                SQL += "       PARA_SOURCEENDPOINT          =  '" & Me.DbLee.mDbLector.Item("PARA_SOURCEENDPOINT") & "',"
                SQL += "       PARA_DESTINATIONENDPOINT     =  '" & Me.DbLee.mDbLector.Item("PARA_DESTINATIONENDPOINT") & "',"
                SQL += "       PARA_DOMAIN_NAME             =  '" & Me.DbLee.mDbLector.Item("PARA_DOMAIN_NAME") & "',"
                SQL += "       PARA_DOMAIN_USER             =  '" & Me.DbLee.mDbLector.Item("PARA_DOMAIN_USER") & "',"
                SQL += "       PARA_BANCO_AX                =  '" & Me.DbLee.mDbLector.Item("PARA_BANCO_AX") & "',"
                SQL += "       PARA_SERV_CODI_BONO          =  '" & Me.DbLee.mDbLector.Item("PARA_SERV_CODI_BONO") & "',"
                SQL += "       PARA_SERV_CODI_BONOASC       =  '" & Me.DbLee.mDbLector.Item("PARA_SERV_CODI_BONOASC") & "',"
                SQL += "       PARA_WEBSERVICE_LOCATION     =  '" & Me.DbLee.mDbLector.Item("PARA_WEBSERVICE_LOCATION") & "',"
                SQL += "       PARA_ANTICIPO_AX             =  '" & Me.DbLee.mDbLector.Item("PARA_ANTICIPO_AX") & "',"
                SQL += "       PARA_WEBSERVICE_TIMEOUT      =  '" & Me.DbLee.mDbLector.Item("PARA_WEBSERVICE_TIMEOUT") & "',"
                SQL += "       PARA_DOMAIN_PWD              =  '" & Me.DbLee.mDbLector.Item("PARA_DOMAIN_PWD") & "',"
                SQL += "       PARA_FILE_FORMAT             =  '" & Me.DbLee.mDbLector.Item("PARA_FILE_FORMAT") & "',"
                SQL += "       PARA_CIF_VENTA_NORMAL        =  '" & Me.DbLee.mDbLector.Item("PARA_CIF_VENTA_NORMAL") & "',"
                SQL += "       PARA_CIF_VENTA_BONOS         =  '" & Me.DbLee.mDbLector.Item("PARA_CIF_VENTA_BONOS") & "',"
                SQL += "       PARA_CIF_PRODUC_NORMAL       =  '" & Me.DbLee.mDbLector.Item("PARA_CIF_PRODUC_NORMAL") & "',"
                SQL += "       PARA_CIF_PRODUC_BONOS        =  '" & Me.DbLee.mDbLector.Item("PARA_CIF_PRODUC_BONOS") & "',"
                SQL += "       PARA_DESGLO_ALOJA_REGIMEN    =  '" & Me.DbLee.mDbLector.Item("PARA_DESGLO_ALOJA_REGIMEN") & "',"
                SQL += "       PARA_CTA_CALO_AL             =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_CALO_AL") & "',"
                SQL += "       PARA_CTA_VALO_AL             =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_VALO_AL") & "',"
                SQL += "       PARA_CTA_CALO_AD             =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_CALO_AD") & "',"
                SQL += "       PARA_CTA_VALO_AD             =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_VALO_AD") & "',"
                SQL += "       PARA_CTA_CALO_MP             =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_CALO_MP") & "',"
                SQL += "       PARA_CTA_VALO_MP             =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_VALO_MP") & "',"
                SQL += "       PARA_CTA_CALO_PC             =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_CALO_PC") & "',"
                SQL += "       PARA_CTA_VALO_PC             =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_VALO_PC") & "',"
                SQL += "       PARA_CTA_CALO_TI             =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_CALO_TI") & "',"
                SQL += "       PARA_CTA_VALO_TI             =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_VALO_TI") & "',"
                SQL += "       PARA_CTA_CALO_X              =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_CALO_X") & "',"
                SQL += "       PARA_CTA_VALO_X              =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_VALO_X") & "',"
                SQL += "       PARA_SERRUCHA_DPTO           =  '" & Me.DbLee.mDbLector.Item("PARA_SERRUCHA_DPTO") & "',"
                SQL += "       PARA_CTA_SERIE_CRE           =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_SERIE_CRE") & "',"
                SQL += "       PARA_CTA_SERIE_CON           =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_SERIE_CON") & "',"
                SQL += "       PARA_CTA_SERIE_NCRE          =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_SERIE_NCRE") & "',"
                SQL += "       PARA_CTA_56DIGITO            =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_56DIGITO") & "',"
                SQL += "       PARA_TRANSFERENCIA_AGENCIA   =  '" & Me.DbLee.mDbLector.Item("PARA_TRANSFERENCIA_AGENCIA") & "',"
                SQL += "       PARA_CONECTA_NEWPOS          =  '" & Me.DbLee.mDbLector.Item("PARA_CONECTA_NEWPOS") & "',"
                SQL += "       PARA_ART_ANULA_RESERVA       =  '" & Me.DbLee.mDbLector.Item("PARA_ART_ANULA_RESERVA") & "',"
                SQL += "       PARA_CFATODIARI_COD_2        =  '" & Me.DbLee.mDbLector.Item("PARA_CFATODIARI_COD_2") & "',"
                SQL += "       PARA_TRATA_CAJA              =  '" & Me.DbLee.mDbLector.Item("PARA_TRATA_CAJA") & "',"
                SQL += "       PARA_PREF_NCREG              =  '" & Me.DbLee.mDbLector.Item("PARA_PREF_NCREG") & "',"
                SQL += "       PARA_PAT_NUM                 =  '" & Me.DbLee.mDbLector.Item("PARA_PAT_NUM") & "',"
                SQL += "       PARA_CTA_REDONDEO_TOPE       =  '" & Me.DbLee.mDbLector.Item("PARA_CTA_REDONDEO_TOPE") & "',"
                SQL += "       PARA_PASO                    =  '" & Me.DbLee.mDbLector.Item("PARA_PASO") & "'"
                SQL += " WHERE  PARA_EMPGRUPO_COD            = '" & Me.ComboBoxGrupoCod.SelectedValue & "'"
                SQL += " AND    PARA_EMP_COD                 = '" & Me.TextBoxEmpCod.Text & "'"
                SQL += " AND    PARA_EMP_NUM                 =  " & Me.Control


                Me.DbWrite.IniciaTransaccion()
                Me.DbWrite.EjecutaSql(SQL)
                Me.DbLee.mDbLector.Close()


                SQL = "SELECT * FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD ='" & Me.ComboBoxGrupoCod.SelectedValue & "'"
                SQL += "AND ROWNUM = 1 "
                ' SQL += " ORDER BY HOTEL_EMPGRUPO_COD,HOTEL_EMP_COD,HOTEL_EMP_NUM ASC"
                ' La empresa mas vieja 
                SQL += " ORDER BY ROWID ASC"

                Me.DbLee.TraerLector(SQL)
                Me.DbLee.mDbLector.Read()


                SQL = "UPDATE TH_HOTEL "
                SQL += "SET    HOTEL_SPYRO                    =  '" & Me.DbLee.mDbLector.Item("HOTEL_SPYRO") & "'"
                SQL += " WHERE  HOTEL_EMPGRUPO_COD            = '" & Me.ComboBoxGrupoCod.SelectedValue & "'"
                SQL += " AND    HOTEL_EMP_COD                 = '" & Me.TextBoxEmpCod.Text & "'"
                SQL += " AND    HOTEL_EMP_NUM                 =  " & Me.Control


                Me.DbWrite.EjecutaSql(SQL)
                Me.DbLee.mDbLector.Close()


                If Me.DbWrite.StrError = "" Then
                    Me.DbWrite.ConfirmaTransaccion()
                Else
                    Me.DbWrite.CancelaTransaccion()
                    MsgBox(Me.DbWrite.StrError)
                End If

            End If



        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

        End Try
    End Sub
#End Region

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try
            Me.InsertaTH_PARA()
            If Me.CheckBoxValoresPorDefecto.Checked Then
                Me.UpdateTH_PARA()
            End If
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancelar.Click
        Try
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CheckBoxTipoHotel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxTipoHotel.CheckedChanged
        Try
            If Me.CheckBoxTipoHotel.Checked Then
                Me.CheckBoxTipoOtroTipo.Checked = False
                Exit Sub
            End If
            If Me.CheckBoxTipoHotel.Checked = False Then
                Me.CheckBoxTipoOtroTipo.Checked = True
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CheckBoxTipoOtroTipo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxTipoOtroTipo.CheckedChanged
        Try
            If Me.CheckBoxTipoOtroTipo.Checked Then
                Me.CheckBoxTipoHotel.Checked = False
            End If

            If Me.CheckBoxTipoOtroTipo.Checked = False Then
                Me.CheckBoxTipoHotel.Checked = True
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class