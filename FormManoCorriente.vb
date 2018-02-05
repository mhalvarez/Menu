Public Class FormManoCorriente

    Dim DbLeeHotel As C_DATOS.C_DatosOledb
    Dim DbLeeHotelAux As C_DATOS.C_DatosOledb
    Dim DbCentral As C_DATOS.C_DatosOledb
    Dim mStrConexionHotel As String
    Dim mStrConexionCentral As String
    Dim SQL As String


    Private mEmpGrupoCod As String
    Private mEmpCod As String
    Private mNombreHotel As String
  
    Private mConectaGolf As String

    Private mParaUsuarioNewGolf As String
    Private NEWHOTEL As NewHotel.NewHotelData
    Private NEWGOLF As NewGolf.NewGolfData

    Private Sub FormManoCorriente_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.Text = Me.Text & " " & Me.mNombreHotel
            Me.DateTimePickerDesde.Value = CType(Format(Now, "dd/MM/yyyy"), Date)
            Me.DateTimePickerHasta.Value = CType(Format(Now, "dd/MM/yyyy"), Date)

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try
            Dim FECHA As Date = Me.DateTimePickerDesde.Value
            Me.DbLeeHotel = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
            Me.DbLeeHotel.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbLeeHotelAux = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
            Me.DbLeeHotelAux.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbCentral = New C_DATOS.C_DatosOledb(Me.mStrConexionCentral)
            Me.DbCentral.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")



            SQL = "DELETE TH_SALDO WHERE SALDO_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND SALDO_EMP_COD ='" & Me.mEmpCod & "'"
            Me.Cursor = Cursors.WaitCursor
            Me.DbCentral.EjecutaSqlCommit(SQL)
            Me.Cursor = Cursors.Default


            Me.Cursor = Cursors.WaitCursor
            Do While FECHA <= Me.DateTimePickerHasta.Value
                Me.PROCESAR(FECHA)
                FECHA = DateAdd(DateInterval.Day, 1, FECHA)
            Loop

            Me.MostrarDatos()

            Me.Cursor = Cursors.Default




        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub PROCESAR(ByVal vFecha As Date)

        Dim ProduccionLiquida As Double
        Try
            SQL = "SELECT "
            SQL += "ROUND (SUM (MOVI_VLIQ), 2)"
            SQL += " FROM TNHT_MOVI ,TNHT_SERV"
            SQL += " WHERE MOVI_DATR= '" & Format(vFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TNHT_MOVI.SERV_CODI(+) = TNHT_SERV.SERV_CODI "


            If IsNumeric(Me.DbLeeHotel.EjecutaSqlScalar(SQL)) = True Then
                ProduccionLiquida = Me.DbLeeHotel.EjecutaSqlScalar(SQL)
            Else
                ProduccionLiquida = 0
            End If
            Me.GrabaOracle(vFecha, ProduccionLiquida)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub GrabaOracle(ByVal vFecha As Date, ByVal vLiquido As Double)
        Try
            'SQL = "INSERT INTO TH_SALDO (SALDO_EMPGRUPO_COD, SALDO_EMP_COD, SALDO_DATR,SALDO_PROD_LIQ, SALDO_FACT_IMP, SALDO_FACT_NO_IMP,SALDO_ANTI_REC, SALDO_ANTI_FACT)"
            SQL = "INSERT INTO TH_SALDO (SALDO_EMPGRUPO_COD, SALDO_EMP_COD, SALDO_DATR,SALDO_PROD_LIQ)"

            SQL += "VALUES ('" & Me.mEmpGrupoCod & "','" & Me.mEmpCod & "','" & Format(vFecha, "dd/MM/yyyy") & "',"
            SQL += vLiquido & ")"

            Me.DbCentral.EjecutaSqlCommit(SQL)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub MostrarDatos()
        Try
            SQL = "SELECT SALDO_DATR AS FECHA,SALDO_PROD_LIQ AS ""VENTA LÍQUIDA"" FROM TH_SALDO ORDER BY SALDO_DATR ASC"
            Me.DataGridView1.DataSource = Me.DbCentral.TraerDataset(SQL, "DATOS")
            Me.DataGridView1.DataMember = "DATOS"
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancelar.Click
        Try
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub
End Class