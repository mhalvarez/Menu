Public Class FormCambiaPrefijo

    Private Sub ButtonCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancelar.Click
        Try
            PASOSTRING = "CANCELAR"
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try
            PASOSTRING = "ACEPTAR"
            PASOSTRING2 = Me.TextBoxPrefijio.Text
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub FormCambiaPrefijo_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            Me.Text = "Prefijo Anterior = " & PASOSTRING
            PASOSTRING = ""
            Me.TextBoxPrefijio.Focus()
            Me.Update()
        Catch ex As Exception

        End Try
    End Sub
End Class