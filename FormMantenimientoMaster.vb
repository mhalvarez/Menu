Public Class FormMantenimientoMaster
    Private SQL As String
    Private mStrConexion As String
    Private DbAx As C_DATOS.C_DatosOledb


    Private DB_Adaptador As OleDb.OleDbDataAdapter
    Private DB_CommandBuilder As OleDb.OleDbCommandBuilder
    Private DB_DataTable As System.Data.DataTable
    Private DB_DataRow As Data.DataRow

    Dim DB_FilaActual As Integer
    Dim DB_FilaAnterior As Integer
    Dim DB_FilaModificada As Boolean



#Region "Rutinas Privadas"
    Private Sub Conectar()
        Try
            Me.DbAx = New C_DATOS.C_DatosOledb
            Me.DbAx.StrConexion = Me.mStrConexion
            Me.DbAx.AbrirConexion()


        Catch ex As Exception
            MsgBox(ex.Message)

        End Try

    End Sub
    Private Sub CargaCombos()

        Try

            SQL = " SELECT HOTE_CODI,HOTE_DESC FROM AX_HOTE ORDER BY HOTE_CODI ASC"

            Me.DbAx.TraerDataset(SQL, "HOTEL")

            Dim NewRowB(0) As Object
            NewRowB(0) = "<Ninguno>"

            Me.DbAx.mDbDataset.Tables("HOTEL").LoadDataRow(NewRowB, False)
            With Me.ComboBoxEmpCod
                .DataSource = Me.DbAx.mDbDataset.Tables("HOTEL")
                .ValueMember = "HOTE_CODI"
                .DisplayMember = "HOTE_DESC"
                '       .SelectedIndex = .Items.Count - 1
            End With
            Me.DbAx.mDbDataset = Nothing


            ' Escoger el primer elemento en ambos combos

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
            Me.ComprobarActualizar()
            Me.DB_Adaptador.Update(Me.DB_DataTable)
            Me.DB_DataTable.AcceptChanges()
            Me.ToolStripButtonGrabar.Enabled = False
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


            Me.ComprobarActualizar()

            Me.DB_DataRow = Me.DB_DataTable.NewRow
            Me.DB_DataTable.Rows.Add(Me.DB_DataRow)
            Me.ToolStripButtonGrabar.Enabled = True


            Me.HabilitarBotones()
            Me.ToolStripButtonUltimo.PerformClick()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub ToolStripButtonEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonEliminar.Click

        Try
            If MessageBox.Show("Confirma Eliminar el Registro ", "Atención", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
                Me.DB_DataTable.Rows(Me.DB_FilaActual).Delete()
                Me.ToolStripButtonGrabar.Enabled = True
                Me.DB_FilaModificada = False
                Me.ToolStripButtonSiguiente.PerformClick()

            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region
#Region "RUTINAS"


    Private Sub Mantenimiento(ByVal vsql As String)
        Try
            ' CARGAR OBJETOS DATATABLE
            ' SQL = "SELECT HOTE_CODI,HOTE_DESC,HOTE_AX FROM AX_HOTE ORDER BY HOTE_CODI ASC"
            Me.DB_Adaptador = New OleDb.OleDbDataAdapter(vsql, Me.mStrConexion)
            Me.DB_DataTable = New System.Data.DataTable
            Me.DB_Adaptador.Fill(Me.DB_DataTable)

            ' GENERAR COMANDOS INSERT/DELETE/UPDATE
            Me.DB_CommandBuilder = New OleDb.OleDbCommandBuilder(Me.DB_Adaptador)


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub Mostrardatos()
        Try
            If Me.DB_DataTable.Rows.Count < 1 Then
                Exit Sub
            End If

            If Me.DB_FilaActual < 0 Then
                Me.DB_FilaActual = 0
            End If

            If Me.DB_FilaActual >= Me.DB_DataTable.Rows.Count - 1 Then
                Me.DB_FilaActual = Me.DB_DataTable.Rows.Count - 1
            End If


            If Me.DB_DataTable.Rows(Me.DB_FilaActual).RowState = DataRowState.Deleted Then

                If Me.DB_DataTable.Rows(Me.DB_FilaActual).RowState = DataRowState.Deleted Then

                    If Me.DB_FilaActual = 0 Then
                        For i As Integer = Me.DB_FilaActual + 1 To Me.DB_DataTable.Rows.Count - 1
                            If Me.DB_DataTable.Rows(Me.DB_FilaActual).RowState = DataRowState.Deleted Then
                                Me.DB_FilaActual += 1

                            Else
                                Me.DB_FilaActual -= 1
                                Exit Sub

                            End If

                        Next
                        Me.ToolStripButtonSiguiente.PerformClick()
                        Exit Sub

                    Else
                        If Me.DB_FilaActual = Me.DB_DataTable.Rows.Count - 1 Then
                            For i As Integer = Me.DB_FilaActual - 1 To 0
                                If Me.DB_DataTable.Rows(Me.DB_FilaActual).RowState = DataRowState.Deleted Then
                                    Me.DB_FilaActual -= 1
                                Else
                                    Me.DB_FilaActual += 1
                                    Exit Sub

                                End If
                            Next
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
            End If


            Me.ComprobarActualizar()

            Me.DB_FilaAnterior = Me.DB_FilaActual
            Me.TextBoxCodigo.Text = Me.DB_DataTable.Rows(Me.DB_FilaActual).Item("HOTE_CODI").ToString
            Me.TextBoxDescripcion.Text = Me.DB_DataTable.Rows(Me.DB_FilaActual).Item("HOTE_DESC").ToString
            Me.TextBoxCodigoAx.Text = Me.DB_DataTable.Rows(Me.DB_FilaActual).Item("HOTE_AX").ToString

            Me.ToolStripTextBoxPosicion.Text = (Me.DB_FilaActual + 1).ToString

            Me.DB_FilaModificada = False




        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ComprobarActualizar()
        Try
            If Me.CheckBoxActualizaAutomatico.Checked Or Me.DB_FilaModificada = True Or Me.DB_FilaActual <> Me.DB_FilaAnterior Then
                Me.ActualizarDatos(Me.DB_FilaAnterior)

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ActualizarDatos(ByVal vFila As Integer)
        Try


            Me.DB_DataTable.Rows(vFila).Item("HOTE_CODI") = Me.TextBoxCodigo.Text
            Me.DB_DataTable.Rows(vFila).Item("HOTE_DESC") = Me.TextBoxDescripcion.Text
            Me.DB_DataTable.Rows(vFila).Item("HOTE_AX") = Me.TextBoxCodigoAx.Text

            Me.DB_FilaModificada = False

            Me.ToolStripButtonGrabar.Enabled = True


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
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
#End Region
#End Region

    Private Sub FormMantenimientoMaster_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If IsNothing(Me.DbAx) = False Then
                If Me.DbAx.EstadoConexion = ConnectionState.Open Then
                    Me.DbAx.CerrarConexion()
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
                SQL = "SELECT HOTE_CODI,HOTE_DESC,HOTE_AX FROM AX_HOTE "
                SQL += "ORDER BY HOTE_CODI ASC"
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
            ' Me.DB_Adaptador.SelectCommand = ""

        Catch ex As Exception

        End Try
    End Sub
End Class