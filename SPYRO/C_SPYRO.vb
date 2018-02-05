Public Class C_SPYRO
    Private DbCentral As C_DATOS.C_DatosOledb

    Private mvStrConexionCentral As String
    Private mvListBoxDebug As System.Windows.Forms.ListBox
    Private mvForm As System.Windows.Forms.Form

#Region "CONSTRUCTOR"
    Public Sub New(ByVal vStrConexionCentral As String, ByVal vListBoxDebug As System.Windows.Forms.ListBox, ByVal vForm As System.Windows.Forms.Form)

        If IsDBNull(vStrConexionCentral) = True Then
            Throw New Exception("La cedena de conexión a la Base de datos esta vacia")
            Exit Sub
        Else
            Me.mvStrConexionCentral = vStrConexionCentral
            Me.mvListBoxDebug = vListBoxDebug
            Me.mvForm = vForm

        End If
    End Sub
#End Region

    Public Sub ValidaCuentasAlmacen(ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vEmpNum As Integer, ByVal vFecha As Date)

        Try

            Me.DbCentral = New C_DATOS.C_DatosOledb
            DbCentral.StrConexion = Me.mvStrConexionCentral
            DbCentral.AbrirConexion()

            If Me.DbCentral.EstadoConexion = ConnectionState.Open Then
                DbCentral.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")
            Else
                MsgBox("NO ")
            End If




            Dim Param(4) As OleDb.OleDbParameter
            Param(0) = New OleDb.OleDbParameter("@P_EMPGRUPO_COD ", vEmpGrupoCod)
            Param(1) = New OleDb.OleDbParameter("@P_EMP_COD", vEmpCod)
            Param(2) = New OleDb.OleDbParameter("@P_EMP_NUM", vEmpNum)
            Param(3) = New OleDb.OleDbParameter("@P_DATR", vFecha)
            Param(4) = New OleDb.OleDbParameter("@P_STATUS", OleDb.OleDbType.VarChar, 100)
            Param(4).Direction = ParameterDirection.Output


            Me.mvListBoxDebug.Items.Add("En Procedimento de Base de Datos Validando Cuentas")
            Me.mvListBoxDebug.Update()
            DbCentral.EjecutaProcedimiento("VALIDA_SPYRO_ALMACEN", Param)
            Me.mvListBoxDebug.Items.Add("Fin de  Procedimento de Base de Datos Validando Cuentas " & Param(4).Value.ToString)
            Me.mvListBoxDebug.Update()


        Catch ex As Exception

            Me.mvListBoxDebug.Items.Add(ex.Message)
        Finally
            DbCentral.CerrarConexion()

        End Try
    End Sub
    Public Function EJEMPLO(ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vTipVc As String, _
            ByVal vTipoAlbaran As String, ByVal vCodigoCliente As Integer, ByVal vFecha As String, ByVal vSuRef As String, ByVal vAlmacenDestinoTraspaso As String) As String

        Try

            Dim Param(8) As OleDb.OleDbParameter
            Param(0) = New OleDb.OleDbParameter("@Pi_EMPGRUPO_COD ", vEmpGrupoCod)
            Param(1) = New OleDb.OleDbParameter("@Pi_EMP_COD", vEmpCod)
            Param(2) = New OleDb.OleDbParameter("@Pi_TIP_VC", vTipVc)
            Param(3) = New OleDb.OleDbParameter("@Pi_GGALBTIP_COD", vTipoAlbaran)
            Param(4) = New OleDb.OleDbParameter("@Pi_GGCLIPR_COD ", vCodigoCliente)
            Param(5) = New OleDb.OleDbParameter("@Pi_F_ALBARAN", vFecha)
            Param(6) = New OleDb.OleDbParameter("@Pi_SU_REF_ALB", vSuRef)
            Param(7) = New OleDb.OleDbParameter("@Pi_GAALMNOM_TRASP", vAlmacenDestinoTraspaso)

            Param(8) = New OleDb.OleDbParameter("@Po_Albaran", OleDb.OleDbType.VarChar, 100)

            Param(8).Direction = ParameterDirection.Output

            DbCentral.EjecutaProcedimiento("QWE_GENERAL.QWE_GGALBCAB_INSERTAR", Param)

            If IsDBNull(Param(8).Value) = False Then
                Return CType(Param(8).Value, String)
            Else
                Return "0"
            End If


        Catch ex As Exception
            Return "0"
            '    MsgBox("Error Llamada QWE_GENERAL.QWE_GGALBCAB_INSERTAR" & vbCrLf & ex.Message, MsgBoxStyle.Information, "Atención")
        End Try
    End Function
End Class
