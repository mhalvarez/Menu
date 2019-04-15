'00-00
'  La clase se construye pasansole una cadena de conexion o no.
'   Si se pasa la cadena de conexion trata de establecer la conexion tambien , si no hay que llamar al metodo
'   AbrirConexion
'13-marzo 2006 
'  Se incorpora la propiedad StrError (para devolver los Errores de ejecucion de Comandos)
'  Las funciones que no puedan devolver valores y revienten un error ahora devuelven "nothing")
'

Option Strict On
Imports System.Windows.Forms
Imports System.Data
Public Class C_DatosOledb
    Private mDbconexion As OleDb.OleDbConnection
    Private mStrConexion As String
    Private mStrError As String
    Private mDbComando As OleDb.OleDbCommand
    Private mDbTrans As OleDb.OleDbTransaction
    Private mCommandParameters As OleDb.OleDbParameter

    Public mDbDataset As System.Data.DataSet
    Public mDbDatable As System.Data.DataTable
    '    Dim P As System.Data.Common
    Public mDbAdaptador As OleDb.OleDbDataAdapter
    Public mDbComandBuilder As OleDb.OleDbCommandBuilder
    Public mDbLector As OleDb.OleDbDataReader

    Private mFormError As New DialogError

    Private mMostrarStrError As Boolean = True

#Region "CONSTRUCTORES"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal vStrConexion As String)
        MyBase.New()
        mStrConexion = vStrConexion
        AbrirConexion()
    End Sub
    Public Sub New(ByVal vStrConexion As String, ByVal vMostrarError As Boolean)
        MyBase.New()
        mStrConexion = vStrConexion
        Me.mMostrarStrError = vMostrarError
        AbrirConexion()
    End Sub

#End Region
#Region "PROPIEDADES"
    Public Property StrConexion() As String
        Get
            Return mStrConexion
        End Get
        Set(ByVal Value As String)
            mStrConexion = Value
        End Set
    End Property
    Public ReadOnly Property StrVersionDLL() As String
        Get
            Return "FramerWork 2.0 13/03/2003   " & Now.TimeOfDay.ToString
        End Get

    End Property
    Public Property StrError() As String
        Get
            Return mStrError
        End Get
        Set(ByVal Value As String)
            mStrError = Value
        End Set
    End Property
#End Region
#Region "METODOS PUBLICOS"
    Public Sub AbrirConexion()
        mDbconexion = New OleDb.OleDbConnection(mStrConexion)
        Try
            Me.InicializaError()
            mDbconexion.Open()
        Catch ex As OleDb.OleDbException
            If mMostrarStrError Then
                Me.MostrarError("Abrir Conexión", ex.Message, ex.HelpLink, "Ninguno".ToString)
            End If
            Me.StrError = ex.Message
        End Try
    End Sub
    Public Sub CerrarConexion()
        Try
            Me.InicializaError()
            If IsNothing(mDbconexion) = False Then
                If mDbconexion.State = ConnectionState.Open Then
                    mDbconexion.Close()
                    mDbconexion.Dispose()
                End If
            End If

        Catch ex As OleDb.OleDbException
            If mMostrarStrError Then
                Me.MostrarError("Cerrar Conexión", ex.Message, ex.HelpLink, "Ninguno".ToString)
            End If
            Me.StrError = ex.Message
        End Try
    End Sub
    Public Function EstadoConexion() As ConnectionState
        Try
            Me.InicializaError()
            Return mDbconexion.State
        Catch ex As OleDb.OleDbException
            If mMostrarStrError Then
                Me.MostrarError("EstadoConexión", ex.Message, ex.HelpLink, "Ninguno".ToString)
            End If
            Me.StrError = ex.Message

        End Try
    End Function
    Public Function TraerDataset(ByVal vSql As String, ByVal vDataMember As String) As System.Data.DataSet
        Try
            Me.InicializaError()
            mDbDataset = New System.Data.DataSet
            mDbAdaptador = New OleDb.OleDbDataAdapter(vSql, mDbconexion)
            mDbAdaptador.Fill(mDbDataset, vDataMember)
            Return mDbDataset
        Catch ex As OleDb.OleDbException
            If mMostrarStrError Then
                Me.MostrarError("TraerDataset", ex.Message, ex.HelpLink, vSql)
            End If
            Me.StrError = ex.Message
            Return Nothing
        End Try
    End Function
    Public Function TraerDatatable(ByVal vSql As String) As System.Data.DataTable
        Try
            Me.InicializaError()
            mDbDatable = New System.Data.DataTable
            mDbAdaptador = New OleDb.OleDbDataAdapter(vSql, mDbconexion)
            mDbComando = New OleDb.OleDbCommand(vSql, mDbconexion)
            mDbAdaptador.SelectCommand = mDbComando
            mDbAdaptador.Fill(mDbDatable)
            Return mDbDatable
        Catch ex As OleDb.OleDbException
            If mMostrarStrError Then
                Me.MostrarError("TraerDatatable", ex.Message, ex.HelpLink, vSql)
            End If
            Me.StrError = ex.Message
            Return Nothing
        End Try
    End Function
    Public Function TraerLector(ByVal vSql As String) As System.Data.OleDb.OleDbDataReader
        Try
            Me.InicializaError()
            mDbComando = New OleDb.OleDbCommand(vSql, mDbconexion)
            mDbLector = mDbComando.ExecuteReader
            Return mDbLector
        Catch ex As OleDb.OleDbException
            If mMostrarStrError Then
                Me.MostrarError("TraerLector", ex.Message, ex.HelpLink, vSql)
            End If
            Me.StrError = ex.Message
            Return Nothing
        End Try
    End Function
    ' 
    ' Ojo este metodo ha de estar envuelto entre Inicia Transacion y ConfirmaTransaccion
    '
    '
    Public Function EjecutaSql(ByVal vSql As String) As Integer
        Try
            Me.InicializaError()
            mDbComando = New OleDb.OleDbCommand(vSql, mDbconexion)
            mDbComando.Transaction = mDbTrans
            Return mDbComando.ExecuteNonQuery()
        Catch ex As OleDb.OleDbException
            If Me.EstadoConexion = ConnectionState.Open Then
                Me.CancelaTransaccion()
            End If
            If mMostrarStrError Then
                Me.MostrarError("EjecutaSql", ex.Message, ex.HelpLink, vSql)
            End If
            Me.StrError = ex.Message

        End Try
    End Function
    Public Sub EjecutaSqlParametros(ByVal vSql As String, ByVal ParamArray vValorParametros() As OleDb.OleDbParameter)
        mDbComando = New OleDb.OleDbCommand(vSql, mDbconexion)
        mDbComando.CommandType = CommandType.Text
        mDbComando.Transaction = mDbTrans
        Try
            Me.InicializaError()
            Dim Para As OleDb.OleDbParameter
            For Each Para In vValorParametros
                If (Not Para Is Nothing) Then
                    ' Si algun parametro viene sin valor lo pone dbnull
                    If (Para.Direction = ParameterDirection.InputOutput OrElse Para.Direction = ParameterDirection.Input) AndAlso Para.Value Is Nothing Then
                        Para.Value = DBNull.Value
                    End If
                    mDbComando.Parameters.Add(Para)
                End If
            Next Para
            mDbComando.ExecuteScalar()
        Catch ex As OleDb.OleDbException
            If mMostrarStrError Then
                Me.MostrarError("EjecutaSqlParametros", ex.Message, ex.HelpLink, vSql)
            End If
            Me.StrError = ex.Message

        End Try
    End Sub
    Public Sub EjecutaSqlCommit(ByVal vSql As String)
        Try
            ' pruebo a no iniciar transacciones 15/10/2014
            Me.InicializaError()
            mDbComando = New OleDb.OleDbCommand(vSql, mDbconexion)
            '   Me.IniciaTransaccion()
            '   mDbComando.Transaction = mDbTrans
            mDbComando.ExecuteNonQuery()
            '    Me.ConfirmaTransaccion()
        Catch ex As OleDb.OleDbException

            '   Me.CancelaTransaccion()
            If mMostrarStrError Then
                Me.MostrarError("EjecutaSqlCommit", ex.Message, ex.HelpLink, vSql)
            End If
            Me.StrError = ex.Message
        Finally
            mDbComando.Dispose()
        End Try
    End Sub
    Public Function EjecutaSqlScalar(ByVal vSql As String) As String
        Try

            Me.InicializaError()
            mDbComando = New OleDb.OleDbCommand(vSql, mDbconexion)
            mDbComando.CommandType = CommandType.Text
            '      mDbComando.Transaction = mDbTrans


            If IsDBNull(mDbComando.ExecuteScalar) = False Then
                Return CType(mDbComando.ExecuteScalar, String)
            Else
                ' ES NULO
                Return ""
            End If

        Catch ex As OleDb.OleDbException
            If mMostrarStrError Then
                Me.MostrarError("EjecutaSqlScalar", ex.Message, ex.HelpLink, vSql)
            End If

            Me.StrError = ex.Message
            '  ES NOTHING
            Return "0"
        Finally
            mDbComando.Dispose()
        End Try


    End Function
    Public Function EjecutaSqlScalar2(ByVal vSql As String) As String
        Try
            Dim vResult As String
            Me.InicializaError()
            mDbComando = New OleDb.OleDbCommand(vSql, mDbconexion)
            mDbComando.CommandType = CommandType.Text
            mDbComando.Transaction = mDbTrans


            vResult = CType(mDbComando.ExecuteScalar, String)
            ' no es nulo nI nothing
            If IsDBNull(vResult) = False And IsNothing(vResult) = False Then
                Return vResult
            Else
                Return Nothing
            End If

        Catch ex As OleDb.OleDbException
            If mMostrarStrError Then
                Me.MostrarError("EjecutaSqlScalar", ex.Message, ex.HelpLink, vSql)
            End If

            Me.StrError = ex.Message
            Return Nothing
        Finally
            mDbComando.Dispose()
        End Try


    End Function
    Public Function EjecutaSqlScalarDML(ByVal vSql As String) As String
        Try
            Dim vResult As String
            Me.InicializaError()
            mDbComando = New OleDb.OleDbCommand(vSql, mDbconexion)
            mDbComando.CommandType = CommandType.Text
            mDbComando.Transaction = mDbTrans


            vResult = CType(mDbComando.ExecuteScalar, String)
            ' no es nulo nI nothing
            If IsDBNull(vResult) = False And IsNothing(vResult) = False Then
                Return vResult
            Else
                Return "?"
            End If

        Catch ex As OleDb.OleDbException
            If mMostrarStrError Then
                Me.MostrarError("EjecutaSqlScalar", ex.Message, ex.HelpLink, vSql)
            End If

            Me.StrError = ex.Message
            Return "?"
        Finally
            mDbComando.Dispose()
        End Try


    End Function
    Public Function EjecutaSqlScalarNoTrans(ByVal vSql As String) As String
        Try
            Me.InicializaError()
            mDbComando = New OleDb.OleDbCommand(vSql, mDbconexion)
            mDbComando.CommandType = CommandType.Text

            If IsDBNull(mDbComando.ExecuteScalar) = False Then
                Return CType(mDbComando.ExecuteScalar, String)
            Else
                Return ""
            End If

        Catch ex As OleDb.OleDbException
            If mMostrarStrError Then
                Me.MostrarError("EjecutaSqlScalar", ex.Message, ex.HelpLink, vSql)
            End If
            Me.StrError = ex.Message
            Return "0"
        Finally
            mDbComando.Dispose()
        End Try


    End Function
    Public Sub EjecutaProcedimiento(ByVal vProcedurename As String, ByVal ParamArray vValorParametros() As OleDb.OleDbParameter)
        mDbComando = New OleDb.OleDbCommand(vProcedurename, mDbconexion)
        mDbComando.CommandType = CommandType.StoredProcedure
        mDbComando.Transaction = mDbTrans
        Try
            Me.InicializaError()
            ' lo de new es nuevo (probar) 
            Dim Para As OleDb.OleDbParameter
            For Each Para In vValorParametros
                If (Not Para Is Nothing) Then
                    ' Si algun parametro viene sin valor lo pone dbnull
                    If (Para.Direction = ParameterDirection.InputOutput OrElse Para.Direction = ParameterDirection.Input) AndAlso Para.Value Is Nothing Then
                        Para.Value = DBNull.Value
                    End If
                    mDbComando.Parameters.Add(Para)
                End If
            Next Para
            mDbComando.ExecuteScalar()
            ' Abajo nuevo para tratar de liberar cursores en Oracle
            mDbComando.Dispose()

        Catch ex As OleDb.OleDbException
            If mMostrarStrError Then
                Me.MostrarError("EjecutaProcedimiento", ex.Message, ex.HelpLink, "Procedimiento Almacenado = " & vProcedurename)
            End If
            Me.StrError = ex.Message

        End Try
    End Sub
    Public Sub IniciaTransaccion()
        Try
            Me.InicializaError()
            mDbTrans = mDbconexion.BeginTransaction
        Catch ex As OleDb.OleDbException
            If mMostrarStrError Then
                Me.MostrarError("IniciaTransaccion", ex.Message, ex.HelpLink, "Ninguno".ToString)
            End If

            Me.StrError = ex.Message

        End Try
    End Sub
    Public Sub ConfirmaTransaccion()
        Try
            Me.InicializaError()
            mDbTrans.Commit()
        Catch ex As Exception
            If mMostrarStrError Then
                Me.MostrarError("ConfirmaTransaccion", ex.Message, ex.HelpLink, "Ninguno".ToString)
            End If

            Me.StrError = ex.Message

        End Try
    End Sub
    Public Sub CancelaTransaccion()
        Try
            Me.InicializaError()

            mDbTrans.Rollback()
        Catch ex As Exception
            If mMostrarStrError Then
                Me.MostrarError("CancelaTransaccion", ex.Message, ex.HelpLink, "Ninguno".ToString)
            End If

            Me.StrError = ex.Message

        End Try
    End Sub
#End Region
#Region "RUTINAS PRIVADAS"
    Private Sub InicializaError()
        Me.StrError = ""
    End Sub
    Private Sub MostrarError(ByVal vProcedimiento As String, ByVal vError As String, ByVal vLink As String, ByVal vSql As String)
        Me.mFormError.TextBoxProcedimiento.Text = vProcedimiento
        Me.mFormError.TextBoxError.Text = vError
        Me.mFormError.LinkLabelError.Text = vLink
        Me.mFormError.TextBoxSql.Text = vSql & vbCrLf & vbCrLf & " en : " & Me.mStrConexion
        Me.mFormError.ShowDialog()
    End Sub


#End Region

End Class

