Public Class FormPerfilEnvioLopezAlmacen

    Private Sub RadioButtonNewStock_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonNewStock.CheckedChanged
        Try
            Me.TextBoxDebug.Text = "Albaranes, Traspasos , Facturas , Devoluciones en NewStock"
            Me.TextBoxDebug.Update()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub RadioButtonNewPaga_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonNewPaga.CheckedChanged
        Try
            Me.TextBoxDebug.Text = "Solo Facturas Internas NewPaga"
            Me.TextBoxDebug.Update()

        Catch ex As Exception

        End Try
    End Sub

    
    Private Sub FormPerfilEnvioLopezAlmacen_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If Me.RadioButtonNewStock.Checked Then
                PERFILCONTABLE = "NEWSTOCK"
            End If
            If Me.RadioButtonNewPaga.Checked Then
                PERFILCONTABLE = "NEWPAGA"
            End If
            If Me.RadioButtonAmbos.Checked Then
                PERFILCONTABLE = "AMBOS"
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub FormPerfilEnvioLopezAlmacen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.RadioButtonNewStock.Checked = True
        Catch ex As Exception

        End Try
    End Sub
End Class