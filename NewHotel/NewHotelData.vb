Public Class NewHotelData
    Private DbLeeHotel As C_DATOS.C_DatosOledb
    Private DbLeeHotelAux As C_DATOS.C_DatosOledb
    Private DbLeeIntegracion As C_DATOS.C_DatosOledb

    Private mStrConexionHotel As String
    Private mStrConexionIntegracion As String

    Private mEmpGrupoCod As String
    Private mEmpCod As String

    Private SQL As String
    Private Cursores As Integer


    Enum TipoFactura As Integer
        Entidad = 1
        NoAlojado = 2
        Contado = 3
        Parcial = 4
        Otras = 9

    End Enum


#Region "CONSTRUCTOR"
    Public Sub New(ByVal vStrConexionHotel As String, ByVal vStrConexionIntegracion As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String)
        MyBase.New()

        Try

            Me.mStrConexionHotel = vStrConexionHotel
            Me.mStrConexionIntegracion = vStrConexionIntegracion
            Me.mEmpGrupoCod = vEmpGrupoCod
            Me.mEmpCod = vEmpCod
            Me.AbreConexiones()
           


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try



    End Sub
#End Region
#Region "RUTINAS GENERALES"
    Private Sub AbreConexiones()
        Try
            Me.DbLeeHotel = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
            Me.DbLeeHotel.AbrirConexion()
            Me.DbLeeHotel.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbLeeHotelAux = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
            Me.DbLeeHotelAux.AbrirConexion()
            Me.DbLeeHotelAux.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


            Me.DbLeeIntegracion = New C_DATOS.C_DatosOledb(Me.mStrConexionIntegracion)
            Me.DbLeeIntegracion.AbrirConexion()
            Me.DbLeeIntegracion.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Abrir conexiones")
        End Try
    End Sub
    Public Sub CerrarConexiones()
        Try
            If IsNothing(Me.DbLeeHotel) = False Then
                If Me.DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                    Me.DbLeeHotel.CerrarConexion()
                    Me.DbLeeHotel = Nothing

                End If
            End If
            If IsNothing(Me.DbLeeHotelAux) = False Then
                If Me.DbLeeHotelAux.EstadoConexion = ConnectionState.Open Then
                    Me.DbLeeHotelAux.CerrarConexion()
                    Me.DbLeeHotelAux = Nothing
                End If
            End If

            If IsNothing(Me.DbLeeIntegracion) = False Then
                If Me.DbLeeIntegracion.EstadoConexion = ConnectionState.Open Then
                    Me.DbLeeIntegracion.CerrarConexion()
                    Me.DbLeeIntegracion = Nothing
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Cerrar conexiones")
        End Try
    End Sub
#End Region
#Region "METODOS PUBLICOS"
    Public Function DevuelveCuentaContabledeFactura(ByVal vNFactura As Integer, ByVal vSerie As String) As String
        Dim Tipo As Integer

        Try

            ' CONTROL CURSORES 
            SQL = "SELECT NVL(COUNT(*),0) AS TOTAL  FROM V$OPEN_CURSOR"
            Cursores = CInt(Me.DbLeeHotel.EjecutaSqlScalar(SQL))
            If Cursores > 300 Then
                ' Me.CerrarConexiones()
                ' Me.AbreConexiones()
            End If

            SQL = " SELECT SEFA_CODI,FACT_CODI,ENTI_CODI,CCEX_CODI,TACO_CODI  FROM TNHT_FACT WHERE SEFA_CODI = '" & vSerie & "'"
            SQL += " AND FACT_CODI = " & vNFactura


            Me.DbLeeHotel.TraerLector(SQL)

            Me.DbLeeHotel.mDbLector.Read()
            If Me.DbLeeHotel.mDbLector.HasRows Then
                If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = False Then
                    Tipo = 1
                    ' DEBAJO PARA FACTURAS PARCIALES ( CON PARTE DE LA FACTURA COBRADA Y PARTE TRANSFERIDA A LA CONTABILIDAD)
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("TACO_CODI")) = False Then
                    Tipo = 4
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("CCEX_CODI")) = False Then
                    Tipo = 2
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = True And IsDBNull(Me.DbLeeHotel.mDbLector.Item("CCEX_CODI")) = True Then
                    Tipo = 3
                Else
                    Tipo = 9
                    MsgBox("Tipo de Factura Desconocido , al buscar Cuenta Contable", MsgBoxStyle.Information, "Atención")
                End If
            End If

            If Tipo = TipoFactura.Entidad Then
                SQL = "SELECT NVL(ENTI_NCON_AF,0) CUENTA FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            ElseIf Tipo = TipoFactura.Parcial Then
                SQL = "SELECT NVL(ENTI_NCON_AF,0) CUENTA FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("TACO_CODI"), String) & "'"
                Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)

            ElseIf Tipo = TipoFactura.NoAlojado Then
                SQL = "SELECT NVL(CCEX_NCON,0) CUENTA FROM TNHT_CCEX WHERE CCEX_CODI = '" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "'"
                Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            ElseIf Tipo = TipoFactura.Contado Then
                SQL = "SELECT NVL(PARA_CLIENTES_CONTADO,0) CUENTA "
                SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
                SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                Return Me.DbLeeIntegracion.EjecutaSqlScalar(SQL)
            Else
                Return "0"

            End If



        Catch ex As Exception
            MsgBox(ex.Message & " en DevuelveCuentaContabledeFactura", MsgBoxStyle.Information, "Atención")
            Return "0"
        Finally
            Me.DbLeeHotel.mDbLector.Close()
        End Try
    End Function
    Public Function DevuelveCuentaContabledeFacturaIgicSatocan(ByVal vNFactura As Integer, ByVal vSerie As String, ByVal vTipoCuenta As Integer) As String

        ' El parametro vTipoCuenta es pasa saber si se envia la cuenta Contable del campo ENCI_NCON_AF  o ENTI_PAMA dependiendo de la empresa
        ' SOLO PARA SATOCAN

        'If vNFactura = 450 Then
        ' MsgBox("aqui")
        ' End If


        Dim Tipo As Integer
        Try

            ' CONTROL CURSORES 
            SQL = "SELECT NVL(COUNT(*),0) AS TOTAL  FROM V$OPEN_CURSOR"
            Cursores = CInt(Me.DbLeeHotel.EjecutaSqlScalar(SQL))
            If Cursores > 250 Then
                ' Me.CerrarConexiones()
                ' Me.AbreConexiones()
            End If

            SQL = " SELECT SEFA_CODI,FACT_CODI,ENTI_CODI,CCEX_CODI  FROM TNHT_FACT WHERE SEFA_CODI = '" & vSerie & "'"
            SQL += " AND FACT_CODI = " & vNFactura

            Me.DbLeeHotel.TraerLector(SQL)
            Me.DbLeeHotel.mDbLector.Read()
            If Me.DbLeeHotel.mDbLector.HasRows Then
                If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = False Then
                    Tipo = 1
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("CCEX_CODI")) = False Then
                    Tipo = 2
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = True And IsDBNull(Me.DbLeeHotel.mDbLector.Item("CCEX_CODI")) = True Then
                    Tipo = 3
                Else
                    Tipo = 9
                    MsgBox("Tipo de Factura Desconocido , al buscar Cuenta Contable", MsgBoxStyle.Information, "Atención")
                End If
            End If


            If Tipo = TipoFactura.Entidad Then
                If vTipoCuenta = 0 Then
                    SQL = "SELECT NVL(ENTI_NCON_AF,0) CUENTA FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                Else
                    SQL = "SELECT NVL(ENTI_PAMA_AF,0) CUENTA FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                End If
                Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            ElseIf Tipo = TipoFactura.NoAlojado Then
                If vTipoCuenta = 0 Then
                    SQL = "SELECT NVL(CCEX_NCON,0) CUENTA FROM TNHT_CCEX WHERE CCEX_CODI = '" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "'"
                Else
                    SQL = "SELECT NVL(CCEX_PAMA,0) CUENTA FROM TNHT_CCEX WHERE CCEX_CODI = '" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "'"
                End If

                Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            ElseIf Tipo = TipoFactura.Contado Then
                If vTipoCuenta = 0 Then
                    SQL = "SELECT NVL(PARA_CLIENTES_CONTADO,0) CUENTA "
                    SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
                    SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                Else
                    SQL = "SELECT NVL(PARA_CLIENTES_CONTADO_CODIGO,0) CUENTA "
                    SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
                    SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                End If
                Return Me.DbLeeIntegracion.EjecutaSqlScalar(SQL)
            Else
                Return "0"

            End If



        Catch ex As Exception
            MsgBox(ex.Message & " en DevuelveCuentaContabledeFactura Satocan " & vbCrLf & vNFactura & "/" & vSerie, MsgBoxStyle.Information, "Atención")
            Return "0"
        Finally
            Me.DbLeeHotel.mDbLector.Close()
        End Try
    End Function
    Public Function DevuelveDniCifContabledeFactura(ByVal vNFactura As Integer, ByVal vSerie As String) As String




        Dim Tipo As Integer
        Dim Dni As String

        Try

            ' CONTROL CURSORES 
            SQL = "SELECT NVL(COUNT(*),0) AS TOTAL  FROM V$OPEN_CURSOR"
            Cursores = CInt(Me.DbLeeHotel.EjecutaSqlScalar(SQL))
            If Cursores > 250 Then
                ' Me.CerrarConexiones()
                ' Me.AbreConexiones()
            End If

            SQL = " SELECT SEFA_CODI,FACT_CODI,ENTI_CODI,CCEX_CODI,NVL(CLIE_CODI,'0') AS CLIENTE,NVL(FACT_NUCO,'0') AS CIF  FROM TNHT_FACT WHERE SEFA_CODI = '" & vSerie & "'"
            SQL += " AND FACT_CODI = " & vNFactura

            Me.DbLeeHotel.TraerLector(SQL)
            Me.DbLeeHotel.mDbLector.Read()
            If Me.DbLeeHotel.mDbLector.HasRows Then
                If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = False Then
                    Tipo = 1
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("CCEX_CODI")) = False Then
                    Tipo = 2
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = True And IsDBNull(Me.DbLeeHotel.mDbLector.Item("CCEX_CODI")) = True Then
                    Tipo = 3
                Else
                    Tipo = 9
                    MsgBox("Tipo de Factura Desconocido , al buscar Cuenta Contable", MsgBoxStyle.Information, "Atención")
                End If
            End If

            If Tipo = TipoFactura.Entidad Then
                SQL = "SELECT NVL(ENTI_NUCO,'0')  FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            ElseIf Tipo = TipoFactura.NoAlojado Then
                SQL = "SELECT NVL(CCEX_NUCO,'0')  FROM TNHT_CCEX WHERE CCEX_CODI = '" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "'"
                Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            ElseIf Tipo = TipoFactura.Parcial Then
                Return CType(Me.DbLeeHotel.mDbLector("CIF"), String)
            ElseIf Tipo = TipoFactura.Contado Then

                SQL = "SELECT NVL(PARA_CLIENTES_CONTADO_CIF,0) CIF "
                SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
                SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                Dni = Me.DbLeeIntegracion.EjecutaSqlScalar(SQL)

                'SQL = "SELECT NVL(CLIE_NUID,'0') FROM TNHT_CLIE WHERE CLIE_CODI = " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), Integer)
                'Dni = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                'If Dni = "0" Or IsNothing(Dni) = True Then
                'SQL = "SELECT NVL(PARA_CLIENTES_CONTADO_CIF,0) CIF "
                'SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
                'SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                'Dni = Me.DbLeeIntegracion.EjecutaSqlScalar(SQL)
                'End If
                Return Dni

            Else
                Return "0"
            End If



        Catch ex As Exception
            MsgBox(ex.Message & " en DevuelveNIFContabledeFactura", MsgBoxStyle.Information, "Atención")
            Return "0"
        Finally
            Me.DbLeeHotel.mDbLector.Close()
        End Try
    End Function
    
    Public Function DevuelveDniCifoPasaporteContabledeFactura(ByVal vNFactura As Integer, ByVal vSerie As String) As String



        Dim Tipo As Integer
        Dim Dni As String

        Try

            ' CONTROL CURSORES 
            SQL = "SELECT NVL(COUNT(*),0) AS TOTAL  FROM V$OPEN_CURSOR"
            Cursores = CInt(Me.DbLeeHotel.EjecutaSqlScalar(SQL))
            If Cursores > 250 Then
                '  Me.CerrarConexiones()
                '  Me.AbreConexiones()
            End If

            SQL = " SELECT SEFA_CODI,FACT_CODI,ENTI_CODI,CCEX_CODI,NVL(CLIE_CODI,'0') AS CLIENTE  FROM TNHT_FACT WHERE SEFA_CODI = '" & vSerie & "'"
            SQL += " AND FACT_CODI = " & vNFactura

            Me.DbLeeHotel.TraerLector(SQL)
            Me.DbLeeHotel.mDbLector.Read()
            If Me.DbLeeHotel.mDbLector.HasRows Then
                If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = False Then
                    Tipo = 1
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("CCEX_CODI")) = False Then
                    Tipo = 2
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = True And IsDBNull(Me.DbLeeHotel.mDbLector.Item("CCEX_CODI")) = True Then
                    Tipo = 3
                Else
                    Tipo = 9
                    MsgBox("Tipo de Factura Desconocido , al buscar Cuenta Contable", MsgBoxStyle.Information, "Atención")
                End If
            End If

            If Tipo = TipoFactura.Entidad Then
                SQL = "SELECT NVL(ENTI_NUCO,'0')  FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            ElseIf Tipo = TipoFactura.NoAlojado Then
                SQL = "SELECT NVL(CCEX_NUCO,'0')  FROM TNHT_CCEX WHERE CCEX_CODI = '" & CType(Me.DbLeeHotel.mDbLector("CCEX_CODI"), String) & "'"
                Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            ElseIf Tipo = TipoFactura.Contado Then

                SQL = "SELECT NVL(CLIE_NUID,'0') FROM TNHT_CLIE WHERE CLIE_CODI = " & CType(Me.DbLeeHotel.mDbLector("CLIENTE"), Integer)
                Dni = Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
                If Dni = "0" Or IsNothing(Dni) = True Then
                    SQL = "SELECT NVL(PARA_CLIENTES_CONTADO_CIF,0) CIF "
                    SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
                    SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                    Dni = Me.DbLeeIntegracion.EjecutaSqlScalar(SQL)
                End If
                Return Dni

            Else
                Return "0"
            End If



        Catch ex As Exception
            MsgBox(ex.Message & " en DevuelveNIFContabledeFactura", MsgBoxStyle.Information, "Atención")
            Return "0"
        Finally
            Me.DbLeeHotel.mDbLector.Close()
        End Try
    End Function
    Public Function DevuelveTipodeFactura(ByVal vNFactura As Integer, ByVal vSerie As String) As Integer

        Try

            ' CONTROL CURSORES 
            SQL = "SELECT NVL(COUNT(*),0) AS TOTAL  FROM V$OPEN_CURSOR"
            Cursores = CInt(Me.DbLeeHotel.EjecutaSqlScalar(SQL))
            If Cursores > 250 Then
                ' Me.CerrarConexiones()
                '  Me.AbreConexiones()
            End If

            SQL = " SELECT SEFA_CODI,FACT_CODI,ENTI_CODI,CCEX_CODI,NVL(CLIE_CODI,'0') AS CLIENTE  FROM TNHT_FACT WHERE SEFA_CODI = '" & vSerie & "'"
            SQL += " AND FACT_CODI = " & vNFactura

            Me.DbLeeHotel.TraerLector(SQL)
            Me.DbLeeHotel.mDbLector.Read()
            If Me.DbLeeHotel.mDbLector.HasRows Then
                If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = False Then
                    Return 1
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("CCEX_CODI")) = False Then
                    Return 2
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = True And IsDBNull(Me.DbLeeHotel.mDbLector.Item("CCEX_CODI")) = True Then
                    Return 3
                Else
                    Return 9
                    MsgBox("Tipo de Factura Desconocido , al buscar Cuenta Contable", MsgBoxStyle.Information, "Atención")
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message & " en DevuelveNIFContabledeFactura", MsgBoxStyle.Information, "Atención")
            Return 9
        Finally
            Me.DbLeeHotel.mDbLector.Close()
        End Try
    End Function
    Public Function DevuelveCodigoEntiCcexdeFactura(ByVal vNFactura As Integer, ByVal vSerie As String) As String

        Try

            ' CONTROL CURSORES 
            SQL = "SELECT NVL(COUNT(*),0) AS TOTAL  FROM V$OPEN_CURSOR"
            Cursores = CInt(Me.DbLeeHotel.EjecutaSqlScalar(SQL))
            If Cursores > 250 Then
                '  Me.CerrarConexiones()
                '  Me.AbreConexiones()
            End If

            SQL = " SELECT SEFA_CODI,FACT_CODI,ENTI_CODI,CCEX_CODI,NVL(CLIE_CODI,'0') AS CLIENTE  FROM TNHT_FACT WHERE SEFA_CODI = '" & vSerie & "'"
            SQL += " AND FACT_CODI = " & vNFactura

            Me.DbLeeHotel.TraerLector(SQL)
            Me.DbLeeHotel.mDbLector.Read()
            If Me.DbLeeHotel.mDbLector.HasRows Then
                If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = False Then
                    Return "ENTI" & CType(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI"), String)
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("CCEX_CODI")) = False Then
                    Return "CCEX" & CType(Me.DbLeeHotel.mDbLector.Item("CCEX_CODI"), String)
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = True And IsDBNull(Me.DbLeeHotel.mDbLector.Item("CCEX_CODI")) = True Then
                    Return ""
                Else
                    Return "?"
                    MsgBox("Tipo de Factura Desconocido , al buscar Cuenta Contable", MsgBoxStyle.Information, "Atención")
                End If
            Else
                Return "?"
            End If




        Catch ex As Exception
            MsgBox(ex.Message & " en DevuelveNIFContabledeFactura", MsgBoxStyle.Information, "Atención")
            Return "?"
        Finally
            Me.DbLeeHotel.mDbLector.Close()
        End Try
    End Function

    Public Function GetValorDescotandoFinanciero(ByVal vFactura As Integer, ByVal vSerie As String) As Double
        Try

            Dim SumaDescuentos As Double
            Dim Resultado As Double



            SQL = "SELECT  SUM(TIDE_PORC)  "
            SQL += "             FROM TNHT_DESF,TNHT_TIDE  "
            SQL += "             WHERE "
            SQL += "                  TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI  "
            SQL += "           AND TNHT_DESF.FACT_CODI = " & vFactura
            SQL += "           AND TNHT_DESF.SEFA_CODI = '" & vSerie & "'"
            SQL += "                GROUP BY TNHT_DESF.FACT_CODI,TNHT_DESF.SEFA_CODI  "


            SumaDescuentos = CDbl(Me.DbLeeHotel.EjecutaSqlScalar(SQL))
            If SumaDescuentos = 0 Then
                Return 0
            End If



            SQL = "SELECT  ( SUM(MOVI_VLIQ) * " & SumaDescuentos & ") / 100 AS TOTAL ,TIDE_PORC  "
            SQL += "             FROM TNHT_FAMO, TNHT_MOVI,TNHT_FACT,TNHT_SERV,TNHT_DESF,TNHT_TIDE  "
            SQL += "             WHERE TNHT_FAMO.MOVI_CODI = TNHT_MOVI.MOVI_CODI  "
            SQL += "               AND TNHT_FAMO.MOVI_DARE = TNHT_MOVI.MOVI_DARE  "
            SQL += "             AND TNHT_FAMO.FACT_CODI = TNHT_FACT.FACT_CODI  "
            SQL += "               AND TNHT_FAMO.SEFA_CODI = TNHT_FACT.SEFA_CODI  "
            SQL += "                AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI  "
            SQL += "               "
            SQL += "                AND TNHT_FACT.FACT_CODI = TNHT_DESF.FACT_CODI  "
            SQL += "                AND TNHT_FACT.SEFA_CODI = TNHT_DESF.SEFA_CODI  "
            SQL += "                AND TNHT_DESF.TIDE_CODI = TNHT_TIDE.TIDE_CODI  "
            SQL += "                 "
            SQL += "           AND TNHT_FACT.FACT_CODI = " & vFactura
            SQL += "           AND TNHT_FACT.SEFA_CODI = '" & vSerie & "'"
            SQL += "                AND MOVI_TIMO = 1  "

            SQL += "                   "
            SQL += "                GROUP BY TNHT_FACT.FACT_CODI,TNHT_FACT.SEFA_CODI,TIDE_PORC  "


            Resultado = CDbl(Me.DbLeeHotel.EjecutaSqlScalar(SQL))

            If IsNothing(Resultado) = False Then
                If IsNumeric(Resultado) Then
                    Return Math.Round(Resultado, 2)
                Else
                    Return 0
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "GetValorDescotandoFinanciero")
            Return 0
        End Try
    End Function
    Public Function DevuelveNombreSeccion(ByVal vSeccion As String) As String

        Try

            ' CONTROL CURSORES 
            SQL = "SELECT NVL(COUNT(*),0) AS TOTAL  FROM V$OPEN_CURSOR"
            Cursores = CInt(Me.DbLeeHotel.EjecutaSqlScalar(SQL))
            If Cursores > 250 Then
                '  Me.CerrarConexiones()
                '  Me.AbreConexiones()
            End If

            SQL = " SELECT SECC_DESC FROM TNHT_SECC WHERE SECC_CODI = '" & vSeccion & "'"


            Me.DbLeeHotel.TraerLector(SQL)
            Me.DbLeeHotel.mDbLector.Read()
            If Me.DbLeeHotel.mDbLector.HasRows Then
                Return CStr(Me.DbLeeHotel.mDbLector.Item("SECC_DESC"))
            Else
                Return "?"
            End If




        Catch ex As Exception
            MsgBox(ex.Message & " en DevuelveNombreSeccion", MsgBoxStyle.Information, "Atención")
            Return "?"
        Finally
            Me.DbLeeHotel.mDbLector.Close()
        End Try
    End Function
#End Region
#Region "INTERFACES"
    Public Function If1_ProduccionTotal(vFecha As Date, vLiquido As Boolean) As System.Data.OleDb.OleDbDataReader
        Try
            ''debe
            SQL = "SELECT "
            If vLiquido Then
                SQL += "ROUND (SUM (ROUND(MOVI_VLIQ,2)), 2)"
            Else
                SQL += "ROUND (SUM (ROUND(MOVI_VCRE,2)), 2)"
            End If

            SQL += " FROM VNHT_MOVH TNHT_MOVI ,TNHT_SERV"
            SQL += " WHERE MOVI_DATR= '" & vFecha & "'"
            SQL += " AND TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI "

            Me.DbLeeHotel.TraerLector(SQL)

            If IsNothing(Me.DbLeeHotel.mDbLector) = False Then
                Return Me.DbLeeHotel.mDbLector
            Else
                Return Nothing

            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "If_DevuelveProduccion")
            Return Nothing
        End Try
    End Function
    Public Function If1_ProduccionPorDepartamento(vFecha As Date, vLiquido As Boolean) As System.Data.OleDb.OleDbDataReader
        Try
            ''haber
            SQL = "SELECT TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI SERVICIO,TNHT_SERV.SERV_DESC DEPARTAMENTO,NVL(TNHT_SERV.SERV_CMBD,'0') CUENTA ,"
            If vLiquido Then
                SQL += "ROUND (SUM (ROUND(MOVI_VLIQ,2)), 2) TOTAL "
            Else
                SQL += "ROUND (SUM (ROUND(MOVI_VCRE,2)), 2) TOTAL "
            End If
            SQL += " FROM VNHT_MOVH TNHT_MOVI,TNHT_SERV"
            SQL += " WHERE (TNHT_MOVI.SERV_CODI = TNHT_SERV.SERV_CODI) AND MOVI_DATR = '" & vFecha & "'"
            SQL += " GROUP BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CMBD,TNHT_SERV.SERV_COMS"
            SQL += " ORDER BY TNHT_MOVI.SECC_CODI,TNHT_MOVI.SERV_CODI,TNHT_SERV.SERV_DESC,TNHT_SERV.SERV_CMBD"

            Me.DbLeeHotel.TraerLector(SQL)

            If IsNothing(Me.DbLeeHotel.mDbLector) = False Then
                Return Me.DbLeeHotel.mDbLector
            Else
                Return Nothing

            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "If_DevuelveProduccionPorDepartamento")
            Return Nothing

        End Try
    End Function
    Public Function If2_RecibosEnVisa(vFecha As Date, vLiquido As Boolean) As System.Data.OleDb.OleDbDataReader
        Try
            ' debe
            SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_CACR.CACR_DESC TARJETA,NVL(TNHT_CACR.CACR_CTBA,'0') CUENTA,"
            SQL += "NVL(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),'?') RECI_COBR,NVL(MOVI_NUDO,' ') MOVI_NUDO,NVL(MOVI_DESC,' ') MOVI_DESC,NVL(SECC_CODI,'?') AS SECC_CODI,CACR_CTB3 "
            SQL += " , TNHT_MOVI.RESE_CODI,TNHT_MOVI.RESE_ANCI,TNHT_MOVI.CCEX_CODI "
            SQL += "  FROM  VNHT_MOVH TNHT_MOVI,TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL += " And TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += " And TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
            SQL += " And TNHT_MOVI.TIRE_CODI = 1"
            SQL += " And TNHT_MOVI.MOVI_DATR = '" & vFecha & "'"
            SQL += " AND TNHT_MOVI.MOVI_VDEB <> 0"
            SQL += " AND TNHT_MOVI.MOVI_AUTO = '0' "
            SQL += " ORDER BY TNHT_MOVI.MOVI_HORE ASC "



            Me.DbLeeHotel.TraerLector(SQL)

            If IsNothing(Me.DbLeeHotel.mDbLector) = False Then
                Return Me.DbLeeHotel.mDbLector
            Else
                Return Nothing

            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "If_DevuelveProduccionPorDepartamento")
            Return Nothing

        End Try
    End Function
    Public Function If2_RecibosEnOtrasFormas(vFecha As Date, vLiquido As Boolean) As System.Data.OleDb.OleDbDataReader
        Try
            ' debe
            SQL = "SELECT TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,"
            SQL += "NVL(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),'?') RECI_COBR,NVL(MOVI_NUDO,' ') MOVI_NUDO,NVL(MOVI_DESC,' ') MOVI_DESC,NVL(SECC_CODI,'?') AS SECC_CODI "
            SQL += ",NVL(SUBSTR(FNHT_MOVI_RECI(TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.MOVI_TIMO),1,20),' ') RECI_COBR "
            SQL += " FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
            SQL += " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
            SQL += " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
            SQL += " AND TNHT_MOVI.TIRE_CODI = 1"
            SQL += " AND TNHT_MOVI.CACR_CODI IS NULL"
            SQL += " AND TNHT_MOVI.MOVI_DATR = '" & vFecha & "'"
            SQL += " AND TNHT_MOVI.MOVI_VDEB <> 0"
            SQL += " AND TNHT_MOVI.MOVI_AUTO = '0' "
            SQL += " ORDER BY TNHT_MOVI.MOVI_HORE ASC "



            Me.DbLeeHotel.TraerLector(SQL)

            If IsNothing(Me.DbLeeHotel.mDbLector) = False Then
                Return Me.DbLeeHotel.mDbLector
            Else
                Return Nothing

            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "If_DevuelveProduccionPorDepartamento")
            Return Nothing

        End Try
    End Function
    Public Function If2_RecibosEnVisa438(vFecha As Date, vLiquido As Boolean) As System.Data.OleDb.OleDbDataReader
        Try
            ' haber


            ' OJO HACER CON DECODE QUE DEVUELVA UNA CUENTA U OTRA SI ES SECCION DEPOSITO 

            'Me.mParaUsaCta4b = SI SE DESEA CUENTAS DIFERENTES 

            'If Me.mParaUsaCta4b = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
            'Cuenta = Me.mParaCta4b
            'Else
            'Cuenta = Me.mCtaPagosACuenta
            'End If

            SQL = "SELECT  TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE ,TNHT_MOVI.MOVI_VDEB TOTAL,NVL(MOVI_DESC,' ') MOVI_DESC,"
            SQL += " TNHT_CACR.CACR_DESC TARJETA,MOVI_DAVA,NVL(SECC_CODI,'?') AS SECC_CODI FROM VNHT_MOVH TNHT_MOVI,"
            SQL += " TNHT_CACR,TNHT_RESE WHERE TNHT_MOVI.CACR_CODI = TNHT_CACR.CACR_CODI"
            SQL += " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
            SQL += " AND TNHT_MOVI.TIRE_CODI = 1"
            SQL += " AND TNHT_MOVI.MOVI_DATR = '" & vFecha & "'"
            SQL += " AND TNHT_MOVI.MOVI_VDEB <> 0"
            SQL += " AND TNHT_MOVI.MOVI_AUTO = '0' "
            SQL += " ORDER BY TNHT_MOVI.MOVI_HORE ASC "


            Me.DbLeeHotel.TraerLector(SQL)

            If IsNothing(Me.DbLeeHotel.mDbLector) = False Then
                Return Me.DbLeeHotel.mDbLector
            Else
                Return Nothing

            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "If_DevuelveProduccionPorDepartamento")
            Return Nothing

        End Try
    End Function
    Public Function If2_RecibosEnOtrasFormas438(vFecha As Date, vLiquido As Boolean) As System.Data.OleDb.OleDbDataReader
        Try
            ' haber

            ' OJO HACER CON DECODE QUE DEVUELVA UNA CUENTA U OTRA SI ES SECCION DEPOSITO 


            ' OJO HACER CON DECODE QUE DEVUELVA UNA CUENTA U OTRA SI ES SECCION DEPOSITO 

            'Me.mParaUsaCta4b = SI SE DESEA CUENTAS DIFERENTES 

            'If Me.mParaUsaCta4b = True And CType(Me.DbLeeHotel.mDbLector("SECC_CODI"), String) = Me.mParaSecc_DepNh Then
            'Cuenta = Me.mParaCta4b
            'Else
            'Cuenta = Me.mCtaPagosACuenta
            'End If

            SQL = "SELECT TNHT_MOVI.RESE_CODI || '/' || TNHT_MOVI.RESE_ANCI RESERVA,NVL(RESE_ANPH,'?') CLIENTE,TNHT_MOVI.MOVI_VDEB TOTAL,TNHT_FORE.FORE_DESC TIPO,NVL(TNHT_FORE.FORE_CTB1,'0') CUENTA,MOVI_DAVA,NVL(MOVI_DESC,' ') MOVI_DESC,NVL(SECC_CODI,'?') AS SECC_CODI FROM VNHT_MOVH TNHT_MOVI,TNHT_FORE,TNHT_RESE WHERE"
            SQL += " TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI "
            SQL += " AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += " AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+)"
            SQL += " AND TNHT_MOVI.TIRE_CODI = 1"

            SQL += " AND TNHT_MOVI.CACR_CODI IS NULL"
            SQL += " AND TNHT_MOVI.MOVI_DATR = '" & vFecha & "'"
            SQL += " AND TNHT_MOVI.MOVI_VDEB <> 0"
            SQL += " AND TNHT_MOVI.MOVI_AUTO = '0' "


            SQL += " ORDER BY TNHT_MOVI.MOVI_HORE ASC "
            Me.DbLeeHotel.TraerLector(SQL)

            If IsNothing(Me.DbLeeHotel.mDbLector) = False Then
                Return Me.DbLeeHotel.mDbLector
            Else
                Return Nothing

            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "If_DevuelveProduccionPorDepartamento")
            Return Nothing

        End Try
    End Function
#End Region


End Class
