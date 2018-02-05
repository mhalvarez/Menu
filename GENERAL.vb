Imports System.Windows.Forms.Form
Imports System.Net.Mail

Module GENERAL

    Public GAUTOMATICO As Boolean = False
    Public REPORT_PATH As String
    Public DEFAULT_PRINTER As String
    Public ENABLE_CHECKBOX As String
    Public SHOW_FALTA As String
    Public SHOW_INCIDENCIAS As String
    Public MUESTRAINCIDENCIAS As Boolean
    Public PROCESA_NOTASCREDITO As String

    Public MULTIFECHA As String
    Public MULTIFILE As String

    Public MINIMIZA As String
    Public EMPGRUPO_COD As String
    Public EMP_COD As String

    Public AX_HOTELID As String


    Public STRCONEXIONCENTRAL As String

    Public OTROS_CREDITOS As String
    Public OTROS_DEBITOS As String

    Public CODIGO_RECLAMACIONES As String
    Public CODIGO_NOTACREDITO As String
    Public CODIGO_FACTURAS As String
    Public CUENTA_OPE_CAJA As String


    Public WORKSTATION As String
    Public WORKSTATIONDBDRIVER As String

    Public STRING_SAHARA As String


    Public VERSION As String = System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString
    Public TIPONOMINA As String = ""

    'Public DBALIAS_QWERTY As String
    'Public PWD_QWERTY As String
    'Public PROPIETARIO_QWERTY As String
    'Public PROPIETARIO_HOTEL As String

    Public REPORT_SELECTION_FORMULA As String
    Public REPORT_DATE_FORMAT As String = "yyyy, MM, dd"



    Public PARA_PASO_OK As String
    Public PARA_PASO_TRY As String

    Public PARA_WEBSERVICE_TIMEOUT As Integer = 20

    ' Variables de Interambio entre Formularios 

    Public PERFILCONTABLE As String
    Public ESTACARGADOFORMCONVERTIR As Boolean = False
    Public PASOSTRING As String
    Public PASOSTRING2 As String
    Public PASOSTRING3 As String

    Public FEC1 As Date = Now
    Public FEC2 As Date = Now
    Public PASOFECHA As Date
    Public RESULTBOOLEAN As Boolean
    Public RESULTSTR As String
    Public CONTROLF As Boolean = False

    Enum GTIPOFACTURA As Integer
        ENTIDAD = 1
        NOALOJADO = 2
        CONTADO = 3
        OTRAS = 9

    End Enum
    Public TIPOREDONDEO As Integer
    Enum ETIPOREDONDEO As Integer
        Normal = 1
        AwayFromZero = 2
        ToEven = 3
    End Enum



    Public Function StrConexionExtraeUsuario(ByVal vStrConexion As String) As String
        Try
            If vStrConexion.Length > 0 Then
                Dim Elementos As Array
                Dim SubElementos As Array
                Elementos = Split(vStrConexion, ";")
                SubElementos = Split((Elementos(1)), "=")
                Return CType(SubElementos(1), String).Trim
            Else
                Return ""
            End If

        Catch ex As Exception
            Return ""
            MsgBox(ex.Message)
        End Try
    End Function
    Public Function StrConexionExtraePassword(ByVal vStrConexion As String) As String
        Try
            If vStrConexion.Length > 0 Then
                Dim Elementos As Array
                Dim SubElementos As Array
                Elementos = Split(vStrConexion, ";")
                SubElementos = Split((Elementos(2)), "=")
                Return CType(SubElementos(1), String).Trim
            Else
                Return ""
            End If

        Catch ex As Exception
            Return ""
            MsgBox(ex.Message)
        End Try
    End Function
    Public Function StrConexionExtraeDataSource(ByVal vStrConexion As String) As String
        Try
            If vStrConexion.Length > 0 Then
                Dim Elementos As Array
                Dim SubElementos As Array
                Elementos = Split(vStrConexion, ";")
                SubElementos = Split((Elementos(3)), "=")
                Return CType(SubElementos(1), String).Trim
            Else
                Return ""
            End If

        Catch ex As Exception
            Return ""
            MsgBox(ex.Message)
        End Try
    End Function

    Public Function EvaluarNumero(ByVal valor As String) As Boolean
        Try
            Dim ContadorPunto As Integer
            Dim ContadorComas As Integer


            ContadorPunto = UBound(Split(valor, "."))
            ContadorComas = UBound(Split(valor, ","))

            If ContadorComas > 1 Then
                Return False
            ElseIf ContadorPunto > 1 Then
                Return False
            ElseIf IsNumeric(valor) = False Then
                Return False
            Else
                Return True
            End If



        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' 
    ''' </summary>
    ''' <param name="controls"></param>
    ''' <param name="vColor"></param>
    ''' <remarks></remarks>
    Public Sub CONTROLSUpdateForeColor(ByVal controls As ControlCollection, ByVal vColor As Color)
        Try
            If controls Is Nothing Then Return
            For Each C As Control In controls
                If TypeOf C Is Label Then DirectCast(C, Label).ForeColor = vColor
                If C.HasChildren Then CONTROLSUpdateForeColor(C.Controls, vColor)
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="controls"></param>
    ''' <param name="vFontName"></param>
    ''' <param name="vSize"></param>
    ''' <remarks></remarks>
    Public Sub CONTROLSUpdateFont(ByVal controls As ControlCollection, ByVal vFontName As String, ByVal vSize As Single)
        Try
            If controls Is Nothing Then Return

            Dim myFont As System.Drawing.Font
            myFont = New System.Drawing.Font(vFontName, vSize, FontStyle.Regular)

            For Each C As Control In controls
                ' If TypeOf C Is Label Then DirectCast(C, Label).Font = myFont
                C.Font = myFont
                If C.HasChildren Then CONTROLSUpdateFont(C.Controls, vFontName, vSize)
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Sub SendMail(ByVal vMensaje As String)
        Try
            Dim MyIni As New cIniArray

            Dim correo As New MailMessage()

            Dim smtp As New SmtpClient

            correo.From = New MailAddress("mhalvarez42@gmail.com", "Marcos Hernandez")

            correo.To.Add("mhalvarez@qwerty-sistemas.es")

            correo.Subject = "Error Aplicacion de Propietarios"


            correo.Body = My.User.Name & vbCrLf & vbCrLf & "Mensaje : " & vMensaje
            'correo.IsBodyHtml = True
            correo.IsBodyHtml = False



            '    smtp.Host = CType(DbQwertyGeneral.EjecutaSqlScalar("SELECT NVL(PARA_SMTP_SERVER,'?') FROM QWE_PARA"), String)
            smtp.Port = 587
            smtp.EnableSsl = True
            ' smtp.Credentials = New System.Net.NetworkCredential(CType(DbQwertyGeneral.EjecutaSqlScalar("SELECT NVL(PARA_SMTP_USER,'?') FROM QWE_PARA"), String), CType(DbQwertyGeneral.EjecutaSqlScalar("SELECT NVL(PARA_SMTP_PWD,'?') FROM QWE_PARA"), String))

            smtp.Send(correo)
            MsgBox("Mansaje enviado Correctamente", MsgBoxStyle.Information, "Atención")

        Catch ex As SmtpException
            MsgBox(ex.Message)
        Finally

        End Try
    End Sub
    Public Function EstaElProgramaenEjecucion(ByVal vProgramName As String) As Boolean
        Try
            Dim aplicacioncorriendo As Process() = Process.GetProcessesByName(vProgramName)
            If aplicacioncorriendo.Length > 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function EstaelFormularioAbierto(vForm As Form) As Boolean
        Try
            For Each f As Form In Application.OpenForms
                If f.Name = vForm.Name Then
                    Return True

                End If
            Next
            Return False

        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try




    End Function

End Module


