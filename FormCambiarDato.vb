Public Class FormCambiarDato

    Private Sub ButtonCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancelar.Click
        Try
            PASOSTRING = "CANCELAR"
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try

            If EvaluarNumero(Me.TextBoxDato.Text) = False Then
                MsgBox("Revise el  Formato del Dato", MsgBoxStyle.Exclamation, "Atención")
            Else
                PASOSTRING = "ACEPTAR"
                PASOSTRING2 = Me.TextBoxDato.Text
                Me.Close()
            End If


            
        Catch ex As Exception

        End Try
    End Sub

    Private Sub FormCambiarDato_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            Me.Text = "Importe Anterior = " & PASOSTRING
            PASOSTRING = ""
            Me.TextBoxDato.Focus()
            Me.Update()
        Catch ex As Exception

        End Try
    End Sub
End Class