Public Class FormPerfilEnvioLopez

    Private Sub PerfilEnvioLopez_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If Me.RadioButtonFacturas.Checked Then
                PERFILCONTABLE = "FACTURAS"
            End If
            If Me.RadioButtonCaja.Checked Then
                PERFILCONTABLE = "CAJA"
            End If
            If Me.RadioButtonAmbos.Checked Then
                PERFILCONTABLE = "AMBOS"
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub RadioButtonFacturas_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonFacturas.CheckedChanged
        Try
            Me.TextBoxDebug.Text = "Facturas y Notas de Crédito"
            Me.TextBoxDebug.Update()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub RadioButtonCaja_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonCaja.CheckedChanged
        Try
            Me.TextBoxDebug.Text = "Cobros de Contado en Facturas y Recibos  + Devoluciones "
            Me.TextBoxDebug.Update()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub RadioButtonAmbos_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonAmbos.CheckedChanged
        Me.TextBoxDebug.Text = "Facturas y Notas de Crédito + Cobros de Contado en Facturas y Recibos  + Devoluciones "
        Me.TextBoxDebug.Update()
    End Sub

    Private Sub FormPerfilEnvioLopez_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.RadioButtonAmbos.Checked = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub
End Class