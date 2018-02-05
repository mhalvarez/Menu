Public Class NewGolfData
    Private DbLeeHotel As C_DATOS.C_DatosOledb
    Private DbLeeHotelAux As C_DATOS.C_DatosOledb
    Private DbLeeIntegracion As C_DATOS.C_DatosOledb

    Private mStrConexionGolf As String
    Private mStrConexionIntegracion As String
    Private mParaUsuarioNewGolf As String

    Private mEmpGrupoCod As String
    Private mEmpCod As String

    Private SQL As String


    Enum TipoFactura As Integer
        Entidad = 1
        NoAlojado = 2
        Contado = 3
        Otras = 9

    End Enum


#Region "CONSTRUCTOR"
    Public Sub New(ByVal vStrConexionGolf As String, ByVal vStrConexionIntegracion As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String)
        MyBase.New()

        Try

            Me.mStrConexionGolf = vStrConexionGolf
            Me.mStrConexionIntegracion = vStrConexionIntegracion
            Me.mEmpGrupoCod = vEmpGrupoCod
            Me.mEmpCod = vEmpCod
            Me.AbreConexiones()

            ' Carga Algunos Parametros

            SQL = "SELECT PARA_USUARIO_NEWGOLF "
            SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"

            Me.mParaUsuarioNewGolf = Me.DbLeeIntegracion.EjecutaSqlScalar(SQL)

            If IsDBNull(Me.mParaUsuarioNewGolf) = True Then
                MsgBox("No se Localiza Usuario Esquema NewGolf en Parámetros Generales", MsgBoxStyle.Information, "Atención")
            End If




        Catch ex As Exception
            MsgBox(ex.Message)
        End Try



    End Sub
#End Region
#Region "RUTINAS GENERALES"
    Private Sub AbreConexiones()
        Try
            Me.DbLeeHotel = New C_DATOS.C_DatosOledb(Me.mStrConexionGolf)
            Me.DbLeeHotel.AbrirConexion()
            Me.DbLeeHotel.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbLeeHotelAux = New C_DATOS.C_DatosOledb(Me.mStrConexionGolf)
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
                End If
            End If
            If IsNothing(Me.DbLeeHotelAux) = False Then
                If Me.DbLeeHotelAux.EstadoConexion = ConnectionState.Open Then
                    Me.DbLeeHotelAux.CerrarConexion()
                End If
            End If

            If IsNothing(Me.DbLeeIntegracion) = False Then
                If Me.DbLeeIntegracion.EstadoConexion = ConnectionState.Open Then
                    Me.DbLeeIntegracion.CerrarConexion()
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
            SQL = " SELECT SEFA_CODI,FACT_CODI,ENTI_CODI FROM " & Me.mParaUsuarioNewGolf & ".TNPL_FACT WHERE SEFA_CODI = '" & vSerie & "'"
            SQL += " AND FACT_CODI = " & vNFactura

            Me.DbLeeHotel.TraerLector(SQL)
            Me.DbLeeHotel.mDbLector.Read()
            If Me.DbLeeHotel.mDbLector.HasRows Then
                If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = False Then
                    Tipo = 1
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = True Then
                    Tipo = 3
                Else
                    Tipo = 9
                    MsgBox("Tipo de Factura Desconocido , al buscar Cuenta Contable", MsgBoxStyle.Information, "Atención")
                End If
            End If

            If Tipo = TipoFactura.Entidad Then
                SQL = "SELECT NVL(ENTI_NCON_AF,0) CUENTA FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
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
            MsgBox(ex.Message & " en DevuelveCuentaContabledeFactura 1", MsgBoxStyle.Information, "Atención")
            Return "0"
        Finally
            Me.DbLeeHotel.mDbLector.Close()
        End Try
    End Function
    Public Function DevuelveCuentaContabledeFacturasSoloParaLibroIgicSatocan(ByVal vNFactura As Integer, ByVal vSerie As String, ByVal vTipoCuenta As Integer) As String

        ' El parametro vTipoCuenta es pasa saber si se envia la cuenta Contable del campo ENCI_NCON_AF  o ENTI_PAMA dependiendo de la empresa
        ' SOLO PARA SATOCAN

        Dim Tipo As Integer
        Try
            SQL = " SELECT SEFA_CODI,FACT_CODI,ENTI_CODI FROM " & Me.mParaUsuarioNewGolf & ".TNPL_FACT WHERE SEFA_CODI = '" & vSerie & "'"
            SQL += " AND FACT_CODI = " & vNFactura

            Me.DbLeeHotel.TraerLector(SQL)
            Me.DbLeeHotel.mDbLector.Read()
            If Me.DbLeeHotel.mDbLector.HasRows Then
                If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = False Then
                    Tipo = 1
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = True Then
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
            MsgBox(ex.Message & " en DevuelveCuentaContabledeFactura 2", MsgBoxStyle.Information, "Atención")
            Return "0"
        Finally
            Me.DbLeeHotel.mDbLector.Close()
        End Try
    End Function
    Public Function DevuelveDniCifContabledeFactura(ByVal vNFactura As Integer, ByVal vSerie As String) As String



        Dim Tipo As Integer
        Dim Dni As String

        Try
            SQL = " SELECT SEFA_CODI,FACT_CODI,ENTI_CODI,NVL(FACT_NUCO,0) AS FACT_NUCO FROM " & Me.mParaUsuarioNewGolf & ".TNPL_FACT WHERE SEFA_CODI = '" & vSerie & "'"
            SQL += " AND FACT_CODI = " & vNFactura

            Me.DbLeeHotel.TraerLector(SQL)
            Me.DbLeeHotel.mDbLector.Read()
            If Me.DbLeeHotel.mDbLector.HasRows Then
                If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = False Then
                    Tipo = 1
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = True Then
                    Tipo = 3
                Else
                    Tipo = 9
                    MsgBox("Tipo de Factura Desconocido , al buscar Cuenta Contable", MsgBoxStyle.Information, "Atención")
                End If
            End If



            If Tipo = TipoFactura.Entidad Then
                SQL = "SELECT NVL(ENTI_NUCO,'0')  FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            ElseIf Tipo = TipoFactura.Contado Then

                SQL = "SELECT NVL(PARA_CLIENTES_CONTADO_CIF,0) CIF "
                SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
                SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                Dni = Me.DbLeeIntegracion.EjecutaSqlScalar(SQL)

                'Dni = CType(Me.DbLeeHotel.mDbLector.Item("FACT_NUCO"), String)
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
    Public Function DevuelveCuentaContabledeNotaCredito(ByVal vNCredito As Integer, ByVal vSerie As String) As String

        Dim Tipo As Integer
        Try
            SQL = "SELECT TNPL_FACT.ENTI_CODI "
            SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_NCRE, " & Me.mParaUsuarioNewGolf & ".TNPL_NCFA, " & Me.mParaUsuarioNewGolf & ".TNPL_FACT "
            SQL += " WHERE "
            SQL += " TNPL_NCRE.SENC_CODI = '" & vSerie & "'"
            SQL += " AND TNPL_NCRE.NCRE_CODI = " & vNCredito

            SQL += " AND TNPL_NCRE.NCRE_CODI = TNPL_NCFA.NCRE_CODI(+) "
            SQL += " AND TNPL_NCRE.SENC_CODI = TNPL_NCFA.SENC_CODI(+) "

            SQL += " AND TNPL_NCFA.FACT_CODI = TNPL_FACT.FACT_CODI(+) "
            SQL += " AND TNPL_NCFA.SEFA_CODI = TNPL_FACT.SEFA_CODI(+) "




            Me.DbLeeHotel.TraerLector(SQL)
            Me.DbLeeHotel.mDbLector.Read()
            If Me.DbLeeHotel.mDbLector.HasRows Then
                If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = False Then
                    Tipo = 1
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = True Then
                    Tipo = 3
                Else
                    Tipo = 9
                    MsgBox("Tipo de Nota de Crédito Desconocido , al buscar Cuenta Contable", MsgBoxStyle.Information, "Atención")
                End If
            End If

            If Tipo = TipoFactura.Entidad Then
                SQL = "SELECT NVL(ENTI_NCON_AF,0) CUENTA FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
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
            MsgBox(ex.Message & " en DevuelveCuentaContabledeFactura 3", MsgBoxStyle.Information, "Atención")
            Return "0"
        Finally
            Me.DbLeeHotel.mDbLector.Close()
        End Try
    End Function
    Public Function DevuelveCuentaContabledeNotaCreditoSoloParaLibroIgicSatocan(ByVal vNCredito As Integer, ByVal vSerie As String, ByVal vTipoCuenta As Integer) As String


        ' El parametro vTipoCuenta es pasa saber si se envia la cuenta Contable del campo ENCI_NCON_AF  o ENTI_PAMA dependiendo de la empresa
        ' SOLO PARA SATOCAN

        Dim Tipo As Integer
        Try
            'SQL = "SELECT TNPL_FACT.ENTI_CODI "
            'SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_NCFA, " & Me.mParaUsuarioNewGolf & ".TNPL_FACT "
            'SQL += " WHERE "
            'SQL += " TNPL_NCFA.FACT_CODI = TNPL_FACT.FACT_CODI AND "
            'SQL += " TNPL_NCFA.SEFA_CODI = TNPL_FACT.SEFA_CODI AND "
            'SQL += " TNPL_NCFA.SENC_CODI = '" & vSerie & "'"
            'SQL += " AND TNPL_NCFA.NCRE_CODI = " & vNCredito

            SQL = "SELECT TNPL_FACT.ENTI_CODI "
            SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_NCRE, " & Me.mParaUsuarioNewGolf & ".TNPL_NCFA, " & Me.mParaUsuarioNewGolf & ".TNPL_FACT "
            SQL += " WHERE "
            SQL += " TNPL_NCRE.SENC_CODI = '" & vSerie & "'"
            SQL += " AND TNPL_NCRE.NCRE_CODI = " & vNCredito

            SQL += " AND TNPL_NCRE.NCRE_CODI = TNPL_NCFA.NCRE_CODI(+) "
            SQL += " AND TNPL_NCRE.SENC_CODI = TNPL_NCFA.SENC_CODI(+) "

            SQL += " AND TNPL_NCFA.FACT_CODI = TNPL_FACT.FACT_CODI(+) "
            SQL += " AND TNPL_NCFA.SEFA_CODI = TNPL_FACT.SEFA_CODI(+) "



            Me.DbLeeHotel.TraerLector(SQL)
            Me.DbLeeHotel.mDbLector.Read()
            If Me.DbLeeHotel.mDbLector.HasRows Then
                If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = False Then
                    Tipo = 1
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = True Then
                    Tipo = 3
                Else
                    Tipo = 9
                    MsgBox("Tipo de Nota de Crédito Desconocido , al buscar Cuenta Contable", MsgBoxStyle.Information, "Atención")
                End If
            End If

            If Tipo = TipoFactura.Entidad Then
                If vTipoCuenta = 0 Then
                    SQL = "SELECT NVL(ENTI_NCON_AF,0) CUENTA FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                Else
                    SQL = "SELECT NVL(ENTI_PAMA_AF,0) CUENTA FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"

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
            MsgBox(ex.Message & " en DevuelveCuentaContabledeFactura 4", MsgBoxStyle.Information, "Atención")
            Return "0"
        Finally
            Me.DbLeeHotel.mDbLector.Close()
        End Try
    End Function
    Public Function DevuelveDniCifContabledeNotaCredito(ByVal vNCredito As Integer, ByVal vSerie As String) As String



        Dim Tipo As Integer
        Dim Dni As String

        Try
            ' SQL = "SELECT NVL(NCRE_NUCO,'0') AS NCRE_NUCO,TNPL_FACT.ENTI_CODI "
            SQL = "SELECT NVL(NCRE_NUCO,'0') AS NCRE_NUCO,TNPL_FACT.ENTI_CODI "
            SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_NCFA, " & Me.mParaUsuarioNewGolf & ".TNPL_FACT," & Me.mParaUsuarioNewGolf & ".TNPL_NCRE "
            SQL += " WHERE "
            SQL += " TNPL_NCFA.NCRE_CODI = TNPL_NCRE.NCRE_CODI AND "
            SQL += " TNPL_NCFA.SENC_CODI = TNPL_NCRE.SENC_CODI AND "
            SQL += " TNPL_NCFA.FACT_CODI = TNPL_FACT.FACT_CODI AND "
            SQL += " TNPL_NCFA.SEFA_CODI = TNPL_FACT.SEFA_CODI AND "
            SQL += " TNPL_NCFA.SENC_CODI = '" & vSerie & "'"
            SQL += " AND TNPL_NCFA.NCRE_CODI = " & vNCredito


            Me.DbLeeHotel.TraerLector(SQL)
            Me.DbLeeHotel.mDbLector.Read()
            If Me.DbLeeHotel.mDbLector.HasRows Then
                If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = False Then
                    Tipo = 1
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = True Then
                    Tipo = 3
                Else
                    Tipo = 9
                    MsgBox("Tipo de Factura Desconocido , al buscar Cuenta Contable", MsgBoxStyle.Information, "Atención")
                End If
            End If



            If Tipo = TipoFactura.Entidad Then
                SQL = "SELECT NVL(ENTI_NUCO,'0')  FROM TNHT_ENTI WHERE ENTI_CODI = '" & CType(Me.DbLeeHotel.mDbLector("ENTI_CODI"), String) & "'"
                Return Me.DbLeeHotelAux.EjecutaSqlScalar(SQL)
            ElseIf Tipo = TipoFactura.Contado Then

                SQL = "SELECT NVL(PARA_CLIENTES_CONTADO_CIF,0) CIF "
                SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
                SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                Dni = Me.DbLeeIntegracion.EjecutaSqlScalar(SQL)

                'Dni = CType(Me.DbLeeHotel.mDbLector.Item("NCRE_NUCO"), String)
                'If Dni = "0" Or IsNothing(Dni) = True Then
                'SQL = "SELECT NVL(PARA_CLIENTES_CONTADO_CIF,0) CIF "
                'SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
                'SQL += "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                'Dni = Me.DbLeeIntegracion.EjecutaSqlScalar(SQL)
                'End If
                Return Dni
            Else
                ' ultimo intento para notas de credito que no vienen de una factura 
                SQL = "SELECT NVL(NCRE_NUCO,'0') AS NCRE_NUCO "
                SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_NCRE  WHERE "
                SQL += " TNPL_NCRE.SENC_CODI = '" & vSerie & "'"
                SQL += " AND TNPL_NCRE.NCRE_CODI = " & vNCredito

                Return Me.DbLeeHotel.EjecutaSqlScalar(SQL)
                ' Return "0"
            End If



        Catch ex As Exception
            MsgBox(ex.Message & " en DevuelveCuentaContabledeFactura 5", MsgBoxStyle.Information, "Atención")
            Return "0"
        Finally
            Me.DbLeeHotel.mDbLector.Close()
        End Try
    End Function
    Public Function DevuelveTipoFactura(ByVal vNFactura As Integer, ByVal vSerie As String) As Integer
        Try
            SQL = " SELECT SEFA_CODI,FACT_CODI,ENTI_CODI,NVL(FACT_NUCO,0) AS FACT_NUCO FROM " & Me.mParaUsuarioNewGolf & ".TNPL_FACT WHERE SEFA_CODI = '" & vSerie & "'"
            SQL += " AND FACT_CODI = " & vNFactura

            Me.DbLeeHotel.TraerLector(SQL)
            Me.DbLeeHotel.mDbLector.Read()
            If Me.DbLeeHotel.mDbLector.HasRows Then
                If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = False Then
                    Return 1
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = True Then
                    Return 3
                Else
                    Return 9
                    MsgBox("Tipo de Factura Desconocido , al buscar Cuenta Contable", MsgBoxStyle.Information, "Atención")
                End If
            End If



        Catch ex As Exception
            MsgBox(ex.Message & " en DevuelveNIFContabledeFactura", MsgBoxStyle.Information, "Atención")
            Return 0
        Finally
            Me.DbLeeHotel.mDbLector.Close()
        End Try
    End Function
    Public Function DevuelveTipoNotaCredito(ByVal vNCredito As Integer, ByVal vSerie As String) As Integer
        Try
            ' SQL = "SELECT NVL(NCRE_NUCO,'0') AS NCRE_NUCO,TNPL_FACT.ENTI_CODI "
            SQL = "SELECT NVL(NCRE_NUCO,'0') AS NCRE_NUCO,NVL(NCRE_NECO,'0') AS NCRE_NECO "
            SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_NCRE "
            SQL += " WHERE "
            SQL += " TNPL_NCRE.SENC_CODI = '" & vSerie & "'"
            SQL += " AND TNPL_NCRE.NCRE_CODI = " & vNCredito


            Me.DbLeeHotel.TraerLector(SQL)
            Me.DbLeeHotel.mDbLector.Read()
            If Me.DbLeeHotel.mDbLector.HasRows Then
                If IsDBNull(Me.DbLeeHotel.mDbLector.Item("NCRE_NECO")) = False Then
                    Return 1
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("NCRE_NECO")) = True Then
                    Return 3
                Else
                    Return 9
                    MsgBox("Tipo de Factura Desconocido , al buscar Cuenta Contable", MsgBoxStyle.Information, "Atención")
                End If
            End If



        Catch ex As Exception
            MsgBox(ex.Message & " en DevuelveCuentaContabledeFactura 5", MsgBoxStyle.Information, "Atención")
            Return 0
        Finally
            Me.DbLeeHotel.mDbLector.Close()
        End Try
    End Function

    Public Function DevuelveCodigoEntiFactura(ByVal vNFactura As Integer, ByVal vSerie As String) As String
        Try
            SQL = " SELECT SEFA_CODI,FACT_CODI,ENTI_CODI,NVL(FACT_NUCO,0) AS FACT_NUCO FROM " & Me.mParaUsuarioNewGolf & ".TNPL_FACT WHERE SEFA_CODI = '" & vSerie & "'"
            SQL += " AND FACT_CODI = " & vNFactura

            Me.DbLeeHotel.TraerLector(SQL)
            Me.DbLeeHotel.mDbLector.Read()
            If Me.DbLeeHotel.mDbLector.HasRows Then
                If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = False Then
                    Return "ENTI" & CType(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI"), String)
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = True Then
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
    Public Function DevuelveCodigoEntiNotaCredito(ByVal vNCredito As Integer, ByVal vSerie As String) As String
        Try
            ' SQL = "SELECT NVL(NCRE_NUCO,'0') AS NCRE_NUCO,TNPL_FACT.ENTI_CODI "
            SQL = "SELECT NVL(NCRE_NUCO,'0') AS NCRE_NUCO,NVL(NCRE_NECO,'0') AS NCRE_NECO "
            SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_NCRE "
            SQL += " WHERE "
            SQL += " TNPL_NCRE.SENC_CODI = '" & vSerie & "'"
            SQL += " AND TNPL_NCRE.NCRE_CODI = " & vNCredito


            Me.DbLeeHotel.TraerLector(SQL)
            Me.DbLeeHotel.mDbLector.Read()
            If Me.DbLeeHotel.mDbLector.HasRows Then
                If IsDBNull(Me.DbLeeHotel.mDbLector.Item("NCRE_NECO")) = False Then
                    Return "ENTI" & CType(Me.DbLeeHotel.mDbLector.Item("NCRE_NECO"), String)
                ElseIf IsDBNull(Me.DbLeeHotel.mDbLector.Item("NCRE_NECO")) = True Then
                    Return ""
                Else
                    Return "?"
                    MsgBox("Tipo de Factura Desconocido , al buscar Cuenta Contable", MsgBoxStyle.Information, "Atención")
                End If
            Else
                Return "?"
            End If



        Catch ex As Exception
            MsgBox(ex.Message & " en DevuelveCuentaContabledeFactura 5", MsgBoxStyle.Information, "Atención")
            Return "?"
        Finally
            Me.DbLeeHotel.mDbLector.Close()
        End Try
    End Function
#End Region

End Class
