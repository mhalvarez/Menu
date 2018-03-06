Public Class FormPassword

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try
            PARA_PASO_TRY = Me.TextBoxPassword.Text
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancelar.Click
        Try
            PARA_PASO_TRY = ""
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBoxPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBoxPassword.KeyDown
        Try
            If e.KeyValue = Keys.F9 Then
                PARA_PASO_TRY = PARA_PASO_OK
                Me.Close()
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


End Class