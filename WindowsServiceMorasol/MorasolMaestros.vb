Imports System.IO
Imports System.Threading
Imports System.Configuration

Public Class MorasolMaestros
    Private Schedular As Timer
    Private DbIntegracion As C_DATOS.C_DatosOledb
    Private SQL As String
    Private mProcesando As Boolean = False
    Private mResultStr As String
    Private mResulCint As Integer

    Private Enum mEnumTipoEnvio
        FrontOffice
        MaestroClientesNewhotel
        MaestroArticulosNewStock
        MaestroAlmacenesNewStock

    End Enum

    Private EnviaTito As HTitoNewHotelEnviar.HTitoNewHotelEnviar

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Agregue el código aquí para iniciar el servicio. Este método debería poner
        ' en movimiento los elementos para que el servicio pueda funcionar.
        Me.WriteToFile("Simple Service started at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt"))

        Me.ScheduleService()
    End Sub

    Protected Overrides Sub OnStop()
        ' Agregue el código aquí para realizar cualquier anulación necesaria para detener el servicio.
        Me.WriteToFile("Simple Service stopped at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt"))

        Me.Schedular.Dispose()
    End Sub
    Public Sub ScheduleService()

        Try

            If Me.mProcesando = True Then Exit Sub

            Schedular = New Timer(New TimerCallback(AddressOf SchedularCallback))

            Dim mode As String = ConfigurationManager.AppSettings("MyMode").ToUpper()

            Me.WriteToFile((Convert.ToString("Simple Service Mode: ") & mode) + " {0}")



            'Set the Default Time.

            Dim scheduledTime As DateTime = DateTime.MinValue



            If mode = "DAILY" Then

                'Get the Scheduled Time from AppSettings.

                scheduledTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings("MyScheduledTime"))

                If DateTime.Now > scheduledTime Then

                    'If Scheduled Time is passed set Schedule for the next day.

                    scheduledTime = scheduledTime.AddDays(1)

                End If

            End If



            If mode.ToUpper() = "INTERVAL" Then

                'Get the Interval in Minutes from AppSettings.

                Dim intervalMinutes As Integer = Convert.ToInt32(ConfigurationManager.AppSettings("MyIntervalMinutes"))



                'Set the Scheduled Time by adding the Interval to Current Time.

                scheduledTime = DateTime.Now.AddMinutes(intervalMinutes)

                If DateTime.Now > scheduledTime Then

                    'If Scheduled Time is passed set Schedule for the next Interval.

                    scheduledTime = scheduledTime.AddMinutes(intervalMinutes)

                End If

            End If



            Dim timeSpan As TimeSpan = scheduledTime.Subtract(DateTime.Now)

            Dim schedule As String = String.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds)



            Me.WriteToFile((Convert.ToString("Simple Service scheduled to run after: ") & schedule) + " {0}")



            'Get the difference in Minutes between the Scheduled and Current Time.

            Dim dueTime As Integer = Convert.ToInt32(timeSpan.TotalMilliseconds)



            'Change the Timer's Due Time.

            Schedular.Change(dueTime, Timeout.Infinite)


            Me.Procesarregistros()

        Catch ex As Exception

            WriteToFile("Simple Service Error on: {0} " + ex.Message + ex.StackTrace)



            'Stop the Windows Service.

            Using serviceController As New System.ServiceProcess.ServiceController("SimpleService")

                serviceController.[Stop]()

            End Using

        End Try

    End Sub



    Private Sub SchedularCallback(e As Object)

        Me.WriteToFile("Simple Service Log: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt"))

        Me.ScheduleService()

    End Sub



    Private Sub WriteToFile(text As String)

        Dim path As String = My.Application.Info.DirectoryPath & "\Logs\ServiceLog" & Format(Now.ToString("yyyy-MM-dd")) & ".txt"

        Using writer As New StreamWriter(path, True)

            writer.WriteLine(String.Format(text, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt")))

            writer.Close()

        End Using

    End Sub

    Private Sub Procesarregistros()
        Try


            Me.WriteToFile("---------------------------------------------------------------------------------------------------------------")
            Me.mProcesando = True

            If IsNothing(Me.DbIntegracion) Then
                Me.DbIntegracion = New C_DATOS.C_DatosOledb(CStr(ConfigurationManager.AppSettings("MyDataBaseCentralConnectionString")), False)
            End If

            If Me.DbIntegracion.EstadoConexion = ConnectionState.Open Then
                Me.WriteToFile("Simple Service Log: " + " DataBase Open Ok " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt"))


                SQL = "SELECT NVL(COUNT(*),0) AS TOTAL FROM TG_TRANS WHERE TRANS_STAT = 0"

                Me.mResulCint = CInt(Me.DbIntegracion.EjecutaSqlScalar2(SQL))

                If IsNothing(Me.mResulCint) = False Then
                    Me.WriteToFile("Simple Service Log: " + " Find to Process " + CStr(Me.mResulCint) + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt"))
                End If

                If Me.mResulCint > 0 Then
                    Me.WriteToFile("Simple Service Log: " + "Call   Dll de Envio " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt"))


                    If IsNothing(Me.EnviaTito) = True Then
                        Me.WriteToFile("Simple Service Log: " + "Instancia dll " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt"))

                        EnviaTito = New HTitoNewHotelEnviar.HTitoNewHotelEnviar(CStr(ConfigurationManager.AppSettings("MyDataBaseCentralConnectionString")), mEnumTipoEnvio.MaestroClientesNewhotel)

                    End If
                    '   Me.WriteToFile("Simple Service Log: " + "Called Dll de Envio " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt"))
                    EnviaTito = Nothing

                End If





                Me.mProcesando = False
                ' fin 


                If IsNothing(Me.DbIntegracion) = False Then
                    Me.DbIntegracion.CerrarConexion()
                    Me.DbIntegracion = Nothing
                    Me.WriteToFile("Simple Service Log: " + " DataBase Close Ok " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt"))
                    Me.WriteToFile("Simple Service Log: " + " Set to Nothing  Database  Object and Renew = " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt"))
                End If

            Else
                Me.mProcesando = False
                Me.DbIntegracion = Nothing
                Me.WriteToFile("Simple Service Log: " + " Fail to Open Database  = " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt"))
                Me.DbIntegracion = Nothing
                Me.WriteToFile("Simple Service Log: " + " Set to Nothing  Database  Object and Renew = " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt"))
            End If


        Catch ex As Exception
            EnviaTito = Nothing
            Me.mProcesando = False

            ' fin 

            If IsNothing(Me.DbIntegracion) = False Then
                Me.DbIntegracion.CerrarConexion()
                Me.DbIntegracion = Nothing
                Me.WriteToFile("Simple Service Log: " + " DataBase Close Ok bY exception " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt"))
            End If


            Me.WriteToFile("Simple Service Log: " + " Error = " + ex.Message + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt"))

            ' PARA SERVICIO WINDOWS ?



        Finally
            Me.WriteToFile("---------------------------------------------------------------------------------------------------------------")

        End Try

    End Sub

    Private Sub Enviar()

    End Sub

    Private Sub MorasolMaestros_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed

    End Sub

    Private Sub EnviaCorreo()
        Try

        Catch ex As Exception

        End Try
    End Sub

End Class
