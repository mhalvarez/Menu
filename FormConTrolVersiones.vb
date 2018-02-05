Public Class FormConTrolVersiones
    Dim MyIni As New cIniArray
    Dim DbLee As New C_DATOS.C_DatosOledb
    Dim SQL As String

    Private Sub FormConTrolVersiones_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If IsNothing(Me.DbLee) = False Then
                If Me.DbLee.EstadoConexion = ConnectionState.Open Then
                    Me.DbLee.CerrarConexion()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub FormConTrolVersiones_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.DbLee = New C_DATOS.C_DatosOledb(MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"))
            Me.DbLee.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")
            Me.MostrarSqls()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
#Region "RUTINAS"
    Private Sub MostrarSqls()
        Try
            SQL = "SELECT VERS_DATE,VERS_LINE,VERS_DESC,VERS_EJEC FROM TH_VERS ORDER BY VERS_DATE,VERS_LINE ASC"
            Me.DbLee.TraerDataset(SQL, "SQL")
            Me.DataGridViewSentencias.DataSource = Me.DbLee.TraerDataset(SQL, "SQL")
            Me.DataGridViewSentencias.DataMember = "SQL"
        Catch ex As Exception

        End Try
    End Sub
#End Region

    Private Sub ButtonEjecutar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEjecutar.Click
        Try

            Dim EJEC As String = " "
            Dim ARRAY As Array
            Dim Ind As Integer

            SQL = "SELECT VERS_DATE,VERS_LINE,VERS_DESC,VERS_EJEC FROM TH_VERS ORDER BY VERS_DATE,VERS_LINE ASC"
            Me.DbLee.TraerLector(SQL)
            While Me.DbLee.mDbLector.Read

                ' ARRAY = CType(Me.DbLee.mDbLector.Item("VERS_DESC"), String).Split(";")
                ARRAY = CType(Me.DbLee.mDbLector.Item("VERS_DESC"), String).Split(vbCrLf)

                For Ind = 0 To ARRAY.Length - 1
                    Me.ListBox1.Items.Add(Format(Now, "hh:mm:ss") & " Ejecutar ->: " & ARRAY(Ind).ToString)
                    EJEC = EJEC + ARRAY(Ind).ToString.Replace(vbCr, " ").Replace(vbLf, "")
                    If ARRAY(Ind).ToString.Contains(";") Then
                        Me.ListBox1.Items.Add("Hay que ejecutar : " & EJEC)
                        ' ejecutar LA SQL
                        EJEC = ""
                    End If
                Next Ind

            End While

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.DbLee.mDbLector.Close()
        End Try
        
    End Sub
End Class