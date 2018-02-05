Public Class FormMantenimiento_TH_IPOSVEN_AX
    Private SQL As String
    Private mStrConexion As String
    Private mEmpGrupoCod As String
    Private DbIntegracion As C_DATOS.C_DatosOledb


    Private DB_Adaptador As OleDb.OleDbDataAdapter
    Private DB_CommandBuilder As OleDb.OleDbCommandBuilder
    Private DB_DataTable As System.Data.DataTable
    Private DB_DataRow As Data.DataRow

    Dim DB_FilaActual As Integer
    Dim DB_FilaAnterior As Integer
    Dim DB_FilaModificada As Boolean
    Dim DB_Cambiado As Boolean


#Region "Rutinas Privadas"
    Private Sub Conectar()
        Try
            Me.DbIntegracion = New C_DATOS.C_DatosOledb
            Me.DbIntegracion.StrConexion = Me.mStrConexion
            Me.DbIntegracion.AbrirConexion()


        Catch ex As Exception
            MsgBox(ex.Message)

        End Try

    End Sub
    Private Sub CargaCombos()
        Try
            ' Emp grupo cod

            SQL = " SELECT DISTINCT PARA_EMPGRUPO_COD FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            Me.DbIntegracion.TraerDataset(SQL, "EMPGRUPOCOD")

            Dim NewRowA(0) As Object
            NewRowA(0) = "<Ninguno>"

            Me.DbIntegracion.mDbDataset.Tables("EMPGRUPOCOD").LoadDataRow(NewRowA, False)
            With Me.ComboBoxGrupoCod
                .DataSource = Me.DbIntegracion.mDbDataset.Tables("EMPGRUPOCOD")
                .ValueMember = "PARA_EMPGRUPO_COD"
                .DisplayMember = "PARA_EMPGRUPO_COD"
                '       .SelectedIndex = .Items.Count - 1
            End With
            Me.DbIntegracion.mDbDataset = Nothing

            ' Emp cod
            'SQL = " SELECT PARA_EMP_COD FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mParaEmpGrupoCod & "'"
            SQL = " SELECT PARA_EMP_COD,HOTEL_DESCRIPCION FROM TH_PARA,TH_HOTEL WHERE "
            SQL += "PARA_EMPGRUPO_COD = HOTEL_EMPGRUPO_COD AND PARA_EMP_COD = HOTEL_EMP_COD"
            SQL += " AND PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"

            Me.DbIntegracion.TraerDataset(SQL, "EMPCOD")

            Dim NewRowB(0) As Object
            NewRowB(0) = "<Ninguno>"

            Me.DbIntegracion.mDbDataset.Tables("EMPCOD").LoadDataRow(NewRowB, False)
            With Me.ComboBoxEmpCod
                .DataSource = Me.DbIntegracion.mDbDataset.Tables("EMPCOD")
                .ValueMember = "PARA_EMP_COD"
                .DisplayMember = "HOTEL_DESCRIPCION"
                '       .SelectedIndex = .Items.Count - 1
            End With
            Me.DbIntegracion.mDbDataset = Nothing


            ' Escoger el primer elemento en ambos combos
            If IsNothing(Me.ComboBoxGrupoCod.Items.Count) = False Then
                Me.ComboBoxGrupoCod.SelectedIndex = 0
            End If

            If IsNothing(Me.ComboBoxEmpCod.Items.Count) = False Then
                Me.ComboBoxEmpCod.SelectedIndex = 0
            End If


        Catch EX As Exception
            MsgBox(EX.Message)
        End Try
    End Sub
#End Region
#Region "MANTENIMIENTO"
#Region "TOOLBAR"


    Private Sub ToolStripButtonPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonPrimero.Click
        Try
            Me.DB_FilaActual = 0
            Me.Mostrardatos()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ToolStripButtonAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonAnterior.Click
        Try
            Me.DB_FilaActual = Me.DB_FilaActual - 1
            Me.Mostrardatos()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ToolStripButtonSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonSiguiente.Click
        Try
            Me.DB_FilaActual = Me.DB_FilaActual + 1
            Me.Mostrardatos()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ToolStripButtonUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonUltimo.Click
        Try
            Me.DB_FilaActual = Me.DB_DataTable.Rows.Count - 1
            Me.Mostrardatos()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub ToolStripButtonGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonGrabar.Click
        Try
            ' DEL BOTON ACTIALIZAR 
            Me.ActualizarDatos(Me.DB_FilaActual)
            ' DEL BOTON GUARDAR EN BASE
            Me.ComprobarActualizar()
            Me.DB_Adaptador.Update(Me.DB_DataTable)
            Me.DB_DataTable.AcceptChanges()
            '     Me.ToolStripButtonGrabar.Enabled = False
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub ToolStripButtonActualizar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonActualizar.Click
        Try
            Me.ActualizarDatos(Me.DB_FilaActual)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub ToolStripButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonNuevo.Click
        Try

            If Me.DB_DataTable.Rows.Count > 0 Then
                ' aqui 
                Me.ComprobarActualizar()
            End If



         
            Me.DB_DataRow = Me.DB_DataTable.NewRow
            Me.DB_DataTable.Rows.Add(Me.DB_DataRow)
            Me.ToolStripButtonGrabar.Enabled = True
            Me.ToolStripButtonActualizar.Enabled = True


            Me.HabilitarBotones()

            If Me.DB_DataTable.Rows.Count > 1 Then
                ' ya hay registros 
                Me.DB_FilaActual = Me.DB_DataTable.Rows.Count - 1
            Else
                ' primer registro
                Me.DB_FilaActual = 0
            End If


            Me.LimpiarCajas()
            Me.TextBoxIposCodi.Focus()
            'Me.ToolStripButtonUltimo.PerformClick()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Boton Nuevo")
        End Try
    End Sub
    Private Sub ToolStripButtonEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonEliminar.Click

        Try

            Me.ComprobarActualizar()

            If MessageBox.Show("Confirma Eliminar el Registro ", "Atención", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
                Me.DB_DataTable.Rows(Me.DB_FilaActual).Delete()
                Me.ToolStripButtonGrabar.Enabled = True
                Me.ToolStripButtonActualizar.Enabled = True
                Me.DB_FilaModificada = False
                ' borra en la base de datos
                Me.DB_Adaptador.Update(Me.DB_DataTable)
                Me.DB_DataTable.AcceptChanges()
                ' camina pa lante 
                Me.ToolStripButtonSiguiente.PerformClick()

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
#End Region
#Region "RUTINAS"


    Private Sub Mantenimiento(ByVal vsql As String)
        Try
            ' CARGAR OBJETOS DATATABLE

            Me.DB_Adaptador = New OleDb.OleDbDataAdapter(vsql, Me.mStrConexion)
            Me.DB_DataTable = New System.Data.DataTable
            Me.DB_Adaptador.Fill(Me.DB_DataTable)

            ' GENERAR COMANDOS INSERT/DELETE/UPDATE
            Me.DB_CommandBuilder = New OleDb.OleDbCommandBuilder(Me.DB_Adaptador)


            Me.Mostrardatos()



        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub Mostrardatos()
        Try
            If Me.DB_DataTable.Rows.Count < 1 Then
                Me.LimpiarCajas()
                Exit Sub
            End If

            If Me.DB_FilaActual < 0 Then
                Me.DB_FilaActual = 0
            End If

            If Me.DB_FilaActual >= Me.DB_DataTable.Rows.Count - 1 Then
                Me.DB_FilaActual = Me.DB_DataTable.Rows.Count - 1
            End If


            If Me.DB_DataTable.Rows(Me.DB_FilaActual).RowState = DataRowState.Deleted Then



                If Me.DB_FilaActual = 0 Then
                    For i As Integer = Me.DB_FilaActual + 1 To Me.DB_DataTable.Rows.Count - 1
                        If Me.DB_DataTable.Rows(Me.DB_FilaActual).RowState = DataRowState.Deleted Then
                            Me.DB_FilaActual = Me.DB_FilaActual + 1

                        Else
                            Me.DB_FilaActual = Me.DB_FilaActual - 1
                            Exit Sub

                        End If

                    Next i
                    Me.ToolStripButtonSiguiente.PerformClick()
                    Exit Sub

                Else
                    If Me.DB_FilaActual = Me.DB_DataTable.Rows.Count - 1 Then
                        For i As Integer = Me.DB_FilaActual - 1 To 0 Step -1
                            If Me.DB_DataTable.Rows(Me.DB_FilaActual).RowState = DataRowState.Deleted Then
                                Me.DB_FilaActual = Me.DB_FilaActual - 1
                            Else
                                Me.DB_FilaActual = Me.DB_FilaActual + 1
                                Exit Sub

                            End If
                        Next i
                        Me.ToolStripButtonAnterior.PerformClick()
                        Exit Sub
                    End If
                End If

                If (Me.DB_FilaActual - Me.DB_FilaAnterior) >= 0 Then
                    Me.ToolStripButtonSiguiente.PerformClick()

                Else
                    Me.ToolStripButtonAnterior.PerformClick()
                    Exit Sub
                End If
            End If



            Me.ComprobarActualizar()

            Me.DB_FilaAnterior = Me.DB_FilaActual

            Me.TextBoxIposCodi.Text = Me.DB_DataTable.Rows(Me.DB_FilaActual).Item("IPOS_CODI").ToString
            Me.TextBoxDescripcion.Text = Me.DB_DataTable.Rows(Me.DB_FilaActual).Item("IPOS_NOME").ToString
            Me.TextBoxTipoPuntoVenta.Text = Me.DB_DataTable.Rows(Me.DB_FilaActual).Item("ALMA_TIPO").ToString
            Me.TextBoxCodigoAlmacenAx.Text = Me.DB_DataTable.Rows(Me.DB_FilaActual).Item("ALMA_AX").ToString


            Me.ToolStripTextBoxPosicion.Text = (Me.DB_FilaActual + 1).ToString

            Me.DB_FilaModificada = False




        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Mostrar Datos")
        End Try
    End Sub

    Private Sub ComprobarActualizar()
        Try
            If Me.CheckBoxActualizaAutomatico.Checked And Me.ComprobarsihaCambiado = True And Me.DB_FilaActual <> Me.DB_FilaAnterior Then

                Me.ActualizarDatos(Me.DB_FilaAnterior)

            End If


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Comprobar Actualizar")
        End Try
    End Sub

    Private Sub ActualizarDatos(ByVal vFila As Integer)
        Try



            Me.DB_DataTable.Rows(vFila).Item("IPOSVEN_EMPGRUPO_COD") = Me.mEmpGrupoCod
            Me.DB_DataTable.Rows(vFila).Item("IPOSVEN_EMP_COD") = Me.ComboBoxEmpCod.SelectedValue

            Me.DB_DataTable.Rows(vFila).Item("IPOS_CODI") = Me.TextBoxIposCodi.Text
            Me.DB_DataTable.Rows(vFila).Item("IPOS_NOME") = Me.TextBoxDescripcion.Text
            Me.DB_DataTable.Rows(vFila).Item("ALMA_TIPO") = Me.TextBoxTipoPuntoVenta.Text
            Me.DB_DataTable.Rows(vFila).Item("ALMA_AX") = Me.TextBoxCodigoAlmacenAx.Text

            Me.DB_FilaModificada = False

            Me.ToolStripButtonGrabar.Enabled = True


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Function ComprobarsihaCambiado() As Boolean
        Try
            Me.DB_Cambiado = False

            If Me.TextBoxDescripcion.Text <> Me.DB_DataTable.Rows(Me.DB_FilaActual).Item("IPOS_NOME") Then
                Return True
            End If

            If Me.TextBoxTipoPuntoVenta.Text <> Me.DB_DataTable.Rows(Me.DB_FilaActual).Item("ALMA_TIPO") Then
                Return True
            End If

            If Me.TextBoxCodigoAlmacenAx.Text <> Me.DB_DataTable.Rows(Me.DB_FilaActual).Item("ALMA_AX") Then
                Return True
            End If


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function
    Private Sub HabilitarBotones()
        Try

            For i As Integer = 0 To Me.ToolStripDatos.Items.Count - 1
                Me.ToolStripDatos.Items.Item(i).Enabled = True
                '  Me.ToolStripDatos.Items.Item(i).ForeColor = Color.Cyan

            Next

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub LimpiarCajas()
        Try
            Dim C As Control

            For Each C In Me.GroupBoxDatos.Controls
                If TypeOf C Is TextBox Then
                    If Me.DB_DataTable.Rows.Count = 0 Then
                        '       C.Enabled = False
                    Else
                        C.Enabled = True
                    End If
                    C.Text = ""
                    C.Update()
                End If
            Next C
        Catch ex As Exception

        End Try
    End Sub
#End Region
#End Region

    Private Sub FormMantenimientoMaster_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If IsNothing(Me.DbIntegracion) = False Then
                If Me.DbIntegracion.EstadoConexion = ConnectionState.Open Then
                    Me.DbIntegracion.CerrarConexion()
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub FormMantenimientoMaster_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Conectar()
            Me.CargaCombos()
            If IsNothing(Me.ComboBoxEmpCod.SelectedValue) = False Then
                SQL = "SELECT IPOSVEN_EMPGRUPO_COD, IPOSVEN_EMP_COD, IPOS_CODI,IPOS_NOME,ALMA_TIPO,ALMA_AX FROM TH_IPOSVEN_AX "
                SQL += " WHERE IPOSVEN_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND  IPOSVEN_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"

                Me.Mantenimiento(SQL)
                Me.Mostrardatos()
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub ComboBoxEmpCod_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxEmpCod.SelectionChangeCommitted
        Try
            ' MOSTRAR LOS DATOS 
            SQL = "SELECT IPOSVEN_EMPGRUPO_COD, IPOSVEN_EMP_COD, IPOS_CODI,IPOS_NOME,ALMA_TIPO,ALMA_AX FROM TH_IPOSVEN_AX "
            SQL += " WHERE IPOSVEN_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND  IPOSVEN_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"


            DB_FilaActual = 0
            DB_FilaAnterior = 0
            DB_FilaModificada = False
            DB_Cambiado = False


            Me.Mantenimiento(SQL)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub TextBoxTipoPuntoVenta_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxTipoPuntoVenta.Validated
        Try
            If Me.TextBoxTipoPuntoVenta.Text.Length > 0 Then
                If Me.TextBoxTipoPuntoVenta.Text <> "1" And Me.TextBoxTipoPuntoVenta.Text <> "2" Then
                    MsgBox("Valor No Válido para Tipo de Punto de Venta ", MsgBoxStyle.Information, "Atención")
                    Me.TextBoxTipoPuntoVenta.Focus()
                    Me.TextBoxTipoPuntoVenta.Select()
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

   
End Class