Public Class FormPerfilEnvioAx

    Private Sub RadioButtonAlmacen_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonAlmacen.CheckedChanged
        Try
          
            If RadioButtonAlmacen.Checked = True Then
                Me.TextBoxDebug.Text = "Venta de Tikets"
                Me.TextBoxDebug.Update()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub FormPerfilEnvioAx_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If Me.RadioButtonContabilidad.Checked Then
                PERFILCONTABLE = "CONTABILIDAD"
            End If
            If Me.RadioButtonAlmacen.Checked Then
                PERFILCONTABLE = "ALMACEN"
            End If
            If Me.RadioButtonAmbos.Checked Then
                PERFILCONTABLE = "AMBOS"
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub RadioButtonContabilidad_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonContabilidad.CheckedChanged
        Try
            If RadioButtonContabilidad.Checked = True Then
                Me.TextBoxDebug.Text = "Producción + Anticipos + Facturas + Cobros + Estadídticas"
                Me.TextBoxDebug.Update()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub RadioButtonAmbos_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonAmbos.CheckedChanged
        Try
            If RadioButtonAmbos.Checked = True Then
                Me.TextBoxDebug.Text = "Producción + Anticipos + Facturas + Cobros + Estadídticas + Venta de Tikets"
                Me.TextBoxDebug.Update()

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class