
Public Class FormFechasPendientesdeProcesar
    Private mStrConexionHotel As String
    Private mStrConexionCentral As String
    Private mDbHotel As C_DATOS.C_DatosOledb
    Private mDbCentral As C_DATOS.C_DatosOledb
    Private SQL As String

    Private mEmpGrupoCod As String
    Private mEmpCod As String
    Private mEmpNum As Integer
    Dim ControlUsuario As Boolean = False

    Sub New(ByVal vStrConexionHotel As String, ByVal vStrConexionCentral As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vEmpNum As Integer)

        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        Try
            Me.mStrConexionHotel = vStrConexionHotel
            Me.mStrConexionCentral = vStrConexionCentral

            Me.mEmpGrupoCod = vEmpGrupoCod
            Me.mEmpCod = vEmpCod
            Me.mEmpNum = vEmpNum


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Sub

    Private Sub FormFechasPendientesdeProcesar_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If IsNothing(Me.mDbHotel) = False Then
                Me.mDbHotel.CerrarConexion()
            End If

            If IsNothing(Me.mDbCentral) = False Then
                Me.mDbCentral.CerrarConexion()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub FormFechasPendientesdeProcesar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.mDbHotel = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel, True)
            Me.mDbCentral = New C_DATOS.C_DatosOledb(Me.mStrConexionCentral, True)


            Me.mDbHotel.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")
            Me.mDbCentral.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


            Me.NumericUpDownEjercicio.Value = Year(Now)
            Me.ControlUsuario = True
            Me.MostrarRegistros()


        Catch ex As Exception
            MsgBox("No Dispone de Acceso a la Base de Datos ", MsgBoxStyle.Information, "Atención")
            Me.Close()
        End Try
    End Sub

    Private Sub MostrarRegistros()
        Try

            Dim ds As New DataSet
            Dim dt As DataTable
            Dim dr As DataRow
            Dim idColumn As DataColumn
            Dim nameColumn As DataColumn
            Dim stateColumn As DataColumn


            Dim IntProcesado As Integer
            Dim IntPendientes As Integer

            dt = New DataTable()
            idColumn = New DataColumn("FECHA", Type.GetType("System.String"))
            nameColumn = New DataColumn("PROCESADO", Type.GetType("System.String"))
            stateColumn = New DataColumn("PENDIENTES", Type.GetType("System.String"))

            dt.Columns.Add(idColumn)
            dt.Columns.Add(nameColumn)
            dt.Columns.Add(stateColumn)


            Me.Cursor = Cursors.WaitCursor

         
            SQL = "SELECT DISTINCT (MOVG_DAVA) AS MOVG_DAVA FROM TNST_MOVG,TNST_TIMO "
            SQL += " WHERE TNST_MOVG.TIMO_CODI = TNST_TIMO.TIMO_CODI "
            ' TIPO DE MOVIMIENTO FACTURA
            SQL += " AND TIMO_TIPO = 1 "
            SQL += " AND TO_CHAR(MOVG_DAVA,'YYYY') = '" & Me.NumericUpDownEjercicio.Value & "'"
            SQL += " ORDER BY MOVG_DAVA ASC"
            Me.mDbHotel.TraerLector(SQL)
            While Me.mDbHotel.mDbLector.Read

                ' Verifica si la feca esta en el Buffer del Interfaz (TS_ASNT)
                SQL = "SELECT COUNT(*) AS TOTAL FROM TS_ASNT WHERE ASNT_F_VALOR = '" & Format(Me.mDbHotel.mDbLector.Item("MOVG_DAVA"), "dd/MM/yyyy") & "'"
                IntProcesado = CInt(Me.mDbCentral.EjecutaSqlScalar(SQL))


                ' Verifica si esta procesado pero con registros pendientes de envio 
                SQL = "SELECT count(*) as TOTAL "
            

                SQL += " FROM TS_ASNT WHERE ASNT_F_VALOR = '" & Format(Me.mDbHotel.mDbLector.Item("MOVG_DAVA"), "dd/MM/yyyy") & "'"

                SQL += " AND TS_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND TS_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND TS_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum

                ' SOLO SIN PROCESAR
                SQL += " AND ASNT_AX_STATUS =  0 "

                IntPendientes = CInt(Me.mDbCentral.EjecutaSqlScalar(SQL))



                dr = dt.NewRow()
                dr("FECHA") = Format(Me.mDbHotel.mDbLector.Item("MOVG_DAVA"), "dd/MM/yyyy")

                If IntProcesado > 0 Then
                    dr("PROCESADO") = "SI"
                Else
                    dr("PROCESADO") = "NO"
                End If

                dr("PENDIENTES") = IntPendientes
                dt.Rows.Add(dr)

            End While
            Me.mDbHotel.mDbLector.Close()


            ds.Tables.Add(dt)


            Me.DataGridView1.DataSource = ds
            Me.DataGridView1.DataMember = ds.Tables(0).ToString




        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Cursor = Cursors.Default

        End Try

    End Sub

    
    Private Sub NumericUpDownEjercicio_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDownEjercicio.ValueChanged
        Try
            If Me.ControlUsuario = True Then
                Me.MostrarRegistros()
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class