Imports System.IO
Imports System.Web.Services.Protocols

Public Class HTitoNewHotelEnviar

    Private SQL As String
    '
    Private Ind As Integer
    '
    Private DbLeeCentral As C_DATOS.C_DatosOledb
    Private DbGrabaCentral As C_DATOS.C_DatosOledb

    Private DbLeeHotel As C_DATOS.C_DatosOledb
    '

    Private mFecha As Date
    Private mStrConexionCentral As String
    Private mEmpgrupo_Cod As String
    Private mEmp_Cod As String
    Private mEmp_Num As String
    Private mDebugFileName As String
    Private mDebugFilePath As String
    Private mDebugFile As StreamWriter
    Private mDebugFileEstaOk As Boolean = False
    Private mWebServiceError As String

    Private mUrl As String


    'PARAMETROS

    Private mParaWebServiceLocation As String
    Private mParaWebServiceName As String
    Private mParaPrefijoProduccion As String
    Private mParaDomainName As String
    Private mParaDomainUser As String
    Private mParaDomainPwd As String
    Private mParaTimeOut As Integer
    Private mParaDimensionHotel As String
    Private mParaSecDepositiosNewHotel As String
    Private mParaSecAnticiposNewHotel As String

    Private mCtaPagosACuenta As String


    Private mParaJournalTemplate As String
    Private mParaJournalBatch As String

    Private mParaSufijoAnticipos As String
    Private mParaSufijoDepositos As String

    Private mParaEquivNat As Integer
    Private mParaEquivDep As Integer
    Private mParaEquivHot As Integer

    Private mParaSourceType As String
    Private mParaIvaNegocio As String
    Private mParaIvaProducto As String







    Private mParaTMovCuenta As String
    Private mParaTMovBanco As String
    Private mParaTMovCliente As String

    Private mParaTDocFactura As String
    Private mParaTDocAbono As String
    Private mParaTDocAnticipos As String
    Private mParaTDocDeposito As String

    Private mAuxInteger As Integer
    Private mAuxString As String

    Private Enum mEnumTipoEnvio
        FrontOffice
        MaestroClientesNewhotel
        MaestroArticulosNewStock
        MaestroAlmacenesNewStock
        NewStock

    End Enum

    Private Enum mEnumAccionMaestro As Integer
        Insertar = 1
        Modificar = 2
        Eliminar = 3
    End Enum

    Private mProcesar As Boolean = False


    '



    ' Web Services Produccion por Departamento  (1.1)
    Private WebServiceProduccionBase As WebReferenceTiToProduccion.ProduccionDpto_Service
    Private WebServiceProduccionList As WebReferenceTiToProduccion.ProduccionDpto()
    Private WebServiceProduccionLinea() As WebReferenceTiToProduccion.ProduccionDpto

    ' Web Services Constitucion de Anticipos y Depositos (1.2)
    Private WebServiceAnticiposBase As WebReferenceTiToAnticiposRecibidos.ConstitucionAnticipos_Service
    Private WebServiceAnticiposList As WebReferenceTiToAnticiposRecibidos.ConstitucionAnticipos()
    Private WebServiceAnticiposLinea() As WebReferenceTiToAnticiposRecibidos.ConstitucionAnticipos

    ' Web Services Aplicacion  de Anticipos y Depositos  en Factura (1.3)
    Private WebServiceAnticiposAplicadosBase As WebReferenceTiToAnticiposFacturados.AplicacionAnticiposFactura_Service
    Private WebServiceAnticiposAplicadosList As WebReferenceTiToAnticiposFacturados.AplicacionAnticiposFactura()
    Private WebServiceAnticiposAplicadosLinea() As WebReferenceTiToAnticiposFacturados.AplicacionAnticiposFactura


    ' Web Services Facturas Emitidas (1.4)


    Private WebServiceFacturacionLineasBase As WebReferenceTiToFacturacion.FacturacionEmitidaCargo_Service
    Private WebServiceFacturacionLineasBaseList As WebReferenceTiToFacturacion.FacturacionEmitidaCargo()
    Private WebServiceFacturacionLineas() As WebReferenceTiToFacturacion.FacturacionEmitidaCargo



    ' Web Services Produccion por Departamento  (1.5)
    Private WebServiceCobrosBase As WebReferenceTiToPagosCobros.PagosCobrosCajaBanco_Service
    Private WebServiceCobrosList As WebReferenceTiToPagosCobros.PagosCobrosCajaBanco()
    Private WebServiceCobrosLinea() As WebReferenceTiToPagosCobros.PagosCobrosCajaBanco




    ' Web Services Clientes 
    Private WebServiceClientesBase As WebReferenceClientesNewHotel.Clientes_Service
    Private WebServiceClientesDatos As WebReferenceClientesNewHotel.Clientes
    Private WebServiceClientesDatosControl As WebReferenceClientesNewHotel.Clientes

    'NewStock Albaranes / Facturas

    Private WebServiceNewStockAlbaranesBase As WebReferenceTitoNewStockAlbaranes.GestionNewHotel
    Private WebServiceANewStockAlbaranesLineasBaseList As WebReferenceTitoNewStockAlbaranes.GestionNewHotel()
    Private WebServiceNewStockAlbaranesLineas() As WebReferenceTitoNewStockAlbaranes.GestionNewHotel




    ' NewStock Traspasos , Inventarios
    Private WebServiceNewStockMovimientos As WebReferenceTitoNewStockMovimientos.StockAlmacenes_Service
    Private WebServiceNewStockMovimientosBaseList As WebReferenceTitoNewStockMovimientos.StockAlmacenes()
    Private NewStockMovimientosLineas() As WebReferenceTitoNewStockMovimientos.StockAlmacenes


#Region "Constructor"
    Sub New(ByVal vfecha As Date, ByVal vStrConexion As String, ByVal vEmpgrupo_Cod As String, ByVal vEmp_Cod As String, ByVal vEmpNum As Integer, vDebugFileName As String, vDebugFilePath As String, vTipoDeEnvio As Integer)
        Try
            If IsDBNull(vfecha) Or IsDBNull(vStrConexion) Or IsDBNull(vEmpgrupo_Cod) Or IsDBNull(vEmpgrupo_Cod) Or IsDBNull(vEmpNum) Then

                Exit Sub
            Else
                Me.mFecha = vfecha
                Me.mStrConexionCentral = vStrConexion
                Me.mEmpgrupo_Cod = vEmpgrupo_Cod
                Me.mEmp_Cod = vEmp_Cod
                Me.mDebugFileName = vDebugFileName
                Me.mDebugFilePath = vDebugFilePath
                Me.mEmp_Num = vEmpNum



            End If

            ' crear Fichero de Gestion de Errores 
            If CrearFichero() = False Then
                Exit Sub
            End If




            If Me.mDebugFileEstaOk Then
                Me.AbreConexiones()
                ' Ler Parametros
                Me.LeerParametros()


                If vTipoDeEnvio = mEnumTipoEnvio.FrontOffice Then

                    '*************************************************************************************************
                    '' Enviar Prodccion
                    Me.WebServiceProduccionBase = New WebReferenceTiToProduccion.ProduccionDpto_Service
                    ' URL
                    Me.WebServiceProduccionBase.Url = Me.mParaWebServiceLocation & Me.mParaWebServiceName & "/Page/ProduccionDpto"
                    ' TimeOut
                    Me.WebServiceProduccionBase.Timeout = Me.mParaTimeOut
                    ' Credenciales
                    Me.WebServiceProduccionBase.Credentials = New System.Net.NetworkCredential(Me.mParaDomainUser, Me.mParaDomainPwd, Me.mParaDomainName)
                    Me.ProcesarProduccion()
                    '*************************************************************************************************
                    '' Enviar Anticipos Constituidos
                    Me.WebServiceAnticiposBase = New WebReferenceTiToAnticiposRecibidos.ConstitucionAnticipos_Service
                    ' URL
                    Me.WebServiceAnticiposBase.Url = Me.mParaWebServiceLocation & Me.mParaWebServiceName & "/Page/ConstitucionAnticipos"
                    ' TimeOut
                    Me.WebServiceAnticiposBase.Timeout = Me.mParaTimeOut
                    ' Credenciales
                    Me.WebServiceAnticiposBase.Credentials = New System.Net.NetworkCredential(Me.mParaDomainUser, Me.mParaDomainPwd, Me.mParaDomainName)
                    Me.ProcesarAnticipos()
                    '*************************************************************************************************
                    '' Enviar Facturas
                    Me.WebServiceFacturacionLineasBase = New WebReferenceTiToFacturacion.FacturacionEmitidaCargo_Service
                    ' URL
                    Me.WebServiceFacturacionLineasBase.Url = Me.mParaWebServiceLocation & Me.mParaWebServiceName & "/Page/FacturacionEmitidaCargo"
                    ' TimeOut
                    Me.WebServiceFacturacionLineasBase.Timeout = Me.mParaTimeOut
                    ' Credenciales
                    'Me.WebServiceFacturacionBase.Credentials = New System.Net.NetworkCredential(Me.mParaDomainUser, Me.mParaDomainPwd, Me.mParaDomainName)
                    Me.WebServiceFacturacionLineasBase.Credentials = New System.Net.NetworkCredential(Me.mParaDomainUser, Me.mParaDomainPwd, Me.mParaDomainName)
                    Me.ProcesarFacturas()

                    '*************************************************************************************************
                    '' Enviar Anticipos Facturados
                    Me.WebServiceAnticiposAplicadosBase = New WebReferenceTiToAnticiposFacturados.AplicacionAnticiposFactura_Service
                    ' URL
                    Me.WebServiceAnticiposAplicadosBase.Url = Me.mParaWebServiceLocation & Me.mParaWebServiceName & "/Page/AplicacionAnticiposFactura"
                    ' TimeOut
                    Me.WebServiceAnticiposAplicadosBase.Timeout = Me.mParaTimeOut
                    ' Credenciales
                    Me.WebServiceAnticiposAplicadosBase.Credentials = New System.Net.NetworkCredential(Me.mParaDomainUser, Me.mParaDomainPwd, Me.mParaDomainName)
                    Me.ProcesarAnticiposFacturados()
                    '*************************************************************************************************
                    '*************************************************************************************************
                    '' Enviar Cobros
                    Me.WebServiceCobrosBase = New WebReferenceTiToPagosCobros.PagosCobrosCajaBanco_Service
                    ' URL
                    Me.WebServiceCobrosBase.Url = Me.mParaWebServiceLocation & Me.mParaWebServiceName & "/Page/PagosCobrosCajaBanco"
                    ' TimeOut
                    Me.WebServiceCobrosBase.Timeout = Me.mParaTimeOut
                    ' Credenciales
                    Me.WebServiceCobrosBase.Credentials = New System.Net.NetworkCredential(Me.mParaDomainUser, Me.mParaDomainPwd, Me.mParaDomainName)
                    Me.ProcesarCobros()
                    '*************************************************************************************************
                End If


                If vTipoDeEnvio = mEnumTipoEnvio.NewStock Then
                    '*************************************************************************************************
                    '' Enviar Traspasos y Movimientos Internos
                    Me.WebServiceNewStockMovimientos = New WebReferenceTitoNewStockMovimientos.StockAlmacenes_Service
                    ' URL
                    Me.WebServiceNewStockMovimientos.Url = Me.mParaWebServiceLocation & Me.mParaWebServiceName & "/Page/StockAlmacenes"
                    ' TimeOut
                    Me.WebServiceNewStockMovimientos.Timeout = Me.mParaTimeOut
                    ' Credenciales
                    Me.WebServiceNewStockMovimientos.Credentials = New System.Net.NetworkCredential(Me.mParaDomainUser, Me.mParaDomainPwd, Me.mParaDomainName)
                    Me.ProcesarNewStockAlbaranes()
                    '*************************************************************************************************

                    '*************************************************************************************************
                    '' Enviar Albaranes
                    Me.WebServiceNewStockAlbaranesBase = New WebReferenceTitoNewStockAlbaranes.GestionNewHotel
                    ' URL
                    Me.WebServiceNewStockAlbaranesBase.Url = Me.mParaWebServiceLocation & Me.mParaWebServiceName & "/Codeunit/Gestión_New_Hotel"
                    ' TimeOut
                    Me.WebServiceNewStockAlbaranesBase.Timeout = Me.mParaTimeOut
                    ' Credenciales
                    Me.WebServiceNewStockAlbaranesBase.Credentials = New System.Net.NetworkCredential(Me.mParaDomainUser, Me.mParaDomainPwd, Me.mParaDomainName)
                    Me.ProcesarNewStockAlbaranes()
                    '*************************************************************************************************
                End If


            End If



        Catch ex As Exception
            If Me.mDebugFileEstaOk Then
                Me.mDebugFile.WriteLine(Now & " Constructor de la Clase HtitoEnviar = " & ex.Message)
            End If
        Finally
            Me.CierraConexiones()
            Me.CerrarFichero()

        End Try
    End Sub
    ''' <summary>
    ''' Constructor para el servicio windows que envia los clientes
    ''' </summary>
    ''' <param name="vStrConexion"></param>
    ''' <param name="vTipoDeEnvio"></param>
    Sub New(ByVal vStrConexion As String, vTipoDeEnvio As Integer)

        Try
            If IsDBNull(vStrConexion) Then

                Exit Sub
            Else

                Me.mStrConexionCentral = vStrConexion
            End If


            ' DEBUG 
            'If CrearFichero("C:\TEMPORAL\", "DEBUG.TXT") = False Then
            'Exit Sub
            'End If

            Me.mDebugFileEstaOk = False

            Me.AbreConexionesParaMAestros()


            ' Ler Parametros

            '   Me.LeerParametros()



            If vTipoDeEnvio = mEnumTipoEnvio.MaestroClientesNewhotel Then

                Me.ProcesarClientes()


            End If






        Catch ex As Exception
            If Me.mDebugFileEstaOk Then
                Me.mDebugFile.WriteLine(Now & " Constructor2 de la Clase HtitoEnviar = " & ex.Message)
            End If
        Finally
            Me.CierraConexiones()
            Me.CerrarFichero()

        End Try
    End Sub
#End Region
#Region "RUTINAS PRIVADAS"
    Private Function CrearFichero() As Boolean
        Try
            mDebugFile = New StreamWriter(Me.mDebugFilePath & Me.mDebugFileName, True, System.Text.Encoding.Default)
            mDebugFileEstaOk = True
            Me.mDebugFile.WriteLine(Now & " --------------------------------------------------------------------------------------------------")
            Return True

        Catch ex As Exception
            mDebugFileEstaOk = False
            Return False
        End Try
    End Function
    Private Function CrearFichero(vPath As String, vName As String) As Boolean
        Try
            mDebugFile = New StreamWriter(vPath & vName, True, System.Text.Encoding.Default)
            mDebugFileEstaOk = True
            Me.mDebugFile.WriteLine(Now & " --------------------------------------------------------------------------------------------------")
            Return True

        Catch ex As Exception
            mDebugFileEstaOk = False
            Return False
        End Try
    End Function
    Private Sub CerrarFichero()
        Try
            mDebugFile.Close()
        Catch ex As Exception


        End Try
    End Sub
    Private Sub AbreConexiones()
        Try
            Me.DbLeeCentral = New C_DATOS.C_DatosOledb(Me.mStrConexionCentral)
            Me.DbLeeCentral.AbrirConexion()
            Me.DbLeeCentral.EjecutaSqlCommit("ALTER SESSION Set NLS_DATE_FORMAT='DD/MM/YYYY'")


            Me.DbGrabaCentral = New C_DATOS.C_DatosOledb(Me.mStrConexionCentral)
            Me.DbGrabaCentral.AbrirConexion()
            Me.DbGrabaCentral.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


        Catch ex As Exception
            ' PDTE: Grabar en Fichero de Errores
        End Try
    End Sub
    Private Sub AbreConexionesParaMAestros()
        Try
            Me.DbLeeCentral = New C_DATOS.C_DatosOledb(Me.mStrConexionCentral, False)
            '    Me.DbLeeCentral.AbrirConexion()
            Me.DbLeeCentral.EjecutaSqlCommit("ALTER SESSION Set NLS_DATE_FORMAT='DD/MM/YYYY'")


            Me.DbGrabaCentral = New C_DATOS.C_DatosOledb(Me.mStrConexionCentral, False)
            '   Me.DbGrabaCentral.AbrirConexion()
            Me.DbGrabaCentral.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


        Catch ex As Exception
            ' PDTE: Grabar en Fichero de Errores
        End Try
    End Sub
    Private Sub CierraConexiones()
        Try
            If IsNothing(Me.DbLeeCentral) = False Then
                Me.DbLeeCentral.CerrarConexion()
                Me.DbLeeCentral = Nothing
            End If

            If IsNothing(Me.DbGrabaCentral) = False Then
                Me.DbGrabaCentral.CerrarConexion()
                Me.DbGrabaCentral = Nothing
            End If

            WriteToFile("tito enviar conexiones cerradas ")
        Catch ex As Exception

        End Try

    End Sub
    Private Sub LeerParametros()
        Try
            SQL = " SELECT "
            SQL += "NVL(PARA_WEBSERVICE_LOCATION,'?') AS PARA_WEBSERVICE_LOCATION"
            SQL += ",NVL(PARA_WEBSERVICE_ENAME,'?') AS PARA_WEBSERVICE_ENAME"
            SQL += ",NVL(PARA_MORA_PREPROD,'?') AS PARA_MORA_PREPROD "
            SQL += ",NVL(PARA_DOMAIN_NAME,'?') AS PARA_DOMAIN_NAME"
            SQL += ",NVL(PARA_DOMAIN_USER,'?') AS PARA_DOMAIN_USER"
            SQL += ",NVL(PARA_DOMAIN_PWD,'?') AS PARA_DOMAIN_PWD"

            SQL += ",NVL(PARA_DOMAIN_NAME2,'?') AS PARA_DOMAIN_NAME2"
            SQL += ",NVL(PARA_DOMAIN_USER2,'?') AS PARA_DOMAIN_USER2"
            SQL += ",NVL(PARA_DOMAIN_PWD2,'?') AS PARA_DOMAIN_PWD2"

            SQL += ",NVL(PARA_WEBSERVICE_TIMEOUT,30000) AS PARA_WEBSERVICE_TIMEOUT"

            SQL += " ,NVL(PARA_MORA_TMOVCUENTA,'?') AS PARA_MORA_TMOVCUENTA, NVL(PARA_MORA_TMOVBANCO,'?') AS PARA_MORA_TMOVBANCO, NVL(PARA_MORA_TMOVCLIENTE,'?') AS PARA_MORA_TMOVCLIENTE, NVL(PARA_MORA_TDOCFACTURA,'?') AS PARA_MORA_TDOCFACTURA "
            SQL += " ,NVL(PARA_MORA_TDOCABONO,'?') AS PARA_MORA_TDOCABONO, NVL(PARA_MORA_TDOCANTICIPO,'?') AS PARA_MORA_TDOCANTICIPO, NVL(PARA_MORA_TDOCDEPOSITO,'?') AS PARA_MORA_TDOCDEPOSITO "
            SQL += " ,NVL(PARA_MORA_DIMENHOTEL,'?') AS PARA_MORA_DIMENHOTEL "
            SQL += " ,NVL(PARA_SECC_DEPNH,'?') AS PARA_SECC_DEPNH "
            SQL += " ,NVL(PARA_SECC_ANTNH,'?') AS PARA_SECC_ANTNH "


            SQL += ",PARA_MORA_JOURNAL_TEMPLATE  "
            SQL += ",PARA_MORA_JOURNAL_BATCH  "


            SQL += ",PARA_MORA_SUFI_ANTI  "
            SQL += ",PARA_MORA_SUFI_DEPO  "



            SQL += ",PARA_MORA_EQUIV_NAT  "
            SQL += ",PARA_MORA_EQUIV_DEP  "
            SQL += ",PARA_MORA_EQUIV_HOT  "


            SQL += ",PARA_MORA_GRUPONEGOCIO  "
            SQL += ",PARA_MORA_GRUPOPRODUCTO  "
            SQL += ",PARA_CTA4  "



            SQL += "  FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpgrupo_Cod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmp_Cod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmp_Num
            ' DEBUG
            If Me.mDebugFileEstaOk Then
                Me.mDebugFile.WriteLine(Now & " SQL " & SQL)
            End If

            Me.DbLeeCentral.TraerLector(SQL)

            Me.DbLeeCentral.mDbLector.Read()

            If Me.DbLeeCentral.mDbLector.HasRows Then
                Me.mParaWebServiceLocation = Me.DbLeeCentral.mDbLector.Item("PARA_WEBSERVICE_LOCATION")
                Me.mParaWebServiceName = Me.DbLeeCentral.mDbLector.Item("PARA_WEBSERVICE_ENAME")
                Me.mParaPrefijoProduccion = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_PREPROD")

                Me.mParaDomainName = Me.DbLeeCentral.mDbLector.Item("PARA_DOMAIN_NAME")
                Me.mParaDomainUser = Me.DbLeeCentral.mDbLector.Item("PARA_DOMAIN_USER")
                Me.mParaDomainPwd = Me.DbLeeCentral.mDbLector.Item("PARA_DOMAIN_PWD")
                Me.mParaTimeOut = CInt(Me.DbLeeCentral.mDbLector.Item("PARA_WEBSERVICE_TIMEOUT")) * 1000

                Me.mParaTMovCuenta = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_TMOVCUENTA")
                Me.mParaTMovBanco = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_TMOVBANCO")
                Me.mParaTMovCliente = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_TMOVCLIENTE")

                Me.mParaTDocFactura = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_TDOCFACTURA")
                Me.mParaTDocAbono = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_TDOCABONO")
                Me.mParaTDocAnticipos = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_TDOCANTICIPO")
                Me.mParaTDocDeposito = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_TDOCDEPOSITO")
                Me.mParaDimensionHotel = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_DIMENHOTEL")
                Me.mParaSecDepositiosNewHotel = Me.DbLeeCentral.mDbLector.Item("PARA_SECC_DEPNH")
                Me.mParaSecAnticiposNewHotel = Me.DbLeeCentral.mDbLector.Item("PARA_SECC_ANTNH")
                Me.mCtaPagosACuenta = CType(Me.DbLeeCentral.mDbLector.Item("PARA_CTA4"), String)



                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_JOURNAL_TEMPLATE")) = False Then
                    Me.mParaJournalTemplate = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_JOURNAL_TEMPLATE")
                Else
                    Me.mParaJournalTemplate = ""
                End If


                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_JOURNAL_BATCH")) = False Then
                    Me.mParaJournalBatch = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_JOURNAL_BATCH")
                Else
                    Me.mParaJournalBatch = ""
                End If




                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_SUFI_ANTI")) = False Then
                    Me.mParaSufijoAnticipos = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_SUFI_ANTI")
                Else
                    Me.mParaSufijoAnticipos = ""
                End If


                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_SUFI_DEPO")) = False Then
                    Me.mParaSufijoDepositos = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_SUFI_DEPO")
                Else
                    Me.mParaSufijoDepositos = ""
                End If


                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_EQUIV_NAT")) = False Then
                    Me.mParaEquivNat = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_EQUIV_NAT")
                Else
                    Me.mParaEquivNat = 0
                End If

                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_EQUIV_DEP")) = False Then
                    Me.mParaEquivDep = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_EQUIV_DEP")
                Else
                    Me.mParaEquivDep = 0
                End If

                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_EQUIV_HOT")) = False Then
                    Me.mParaEquivHot = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_EQUIV_HOT")
                Else
                    Me.mParaEquivHot = 0
                End If



                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_GRUPONEGOCIO")) = False Then
                    Me.mParaIvaNegocio = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_GRUPONEGOCIO")
                Else
                    Me.mParaIvaNegocio = ""
                End If

                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("PARA_MORA_GRUPOPRODUCTO")) = False Then
                    Me.mParaIvaProducto = Me.DbLeeCentral.mDbLector.Item("PARA_MORA_GRUPOPRODUCTO")
                Else
                    Me.mParaIvaProducto = ""
                End If

            End If


                Me.DbLeeCentral.mDbLector.Close()
        Catch ex As Exception
            If Me.mDebugFileEstaOk Then
                Me.mDebugFile.WriteLine(Now & " LeerParametros = " & ex.Message)
            End If
        End Try
    End Sub

#End Region
#Region "ENVIOS"
    Private Sub ProcesarProduccion()

        Dim Primerregistro As Boolean = True

        Try

            SQL = "SELECT ASNT_F_ATOCAB As FECHA, NVL(ASNT_CFCTA_COD,' ') AS CUENTA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,"
            SQL += "NVL(ASNT_DEBE,'0')  AS DEBE,NVL(ASNT_HABER,'0')  AS HABER"
            SQL += ", ASNT_MORA_DIMENNATURALEZA, ASNT_MORA_DIMENDPTO, ASNT_MORA_DIMENACCESO "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpgrupo_Cod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmp_Cod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmp_Num
            ''    SQL += " AND  ASNT_CFATOCAB_REFER = 1"
            SQL += " AND  ASNT_WEBSERVICE_NAME  = 'WEBPRODUCCION'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS = 0 "
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"


            Me.DbLeeCentral.TraerLector(SQL)

            While Me.DbLeeCentral.mDbLector.Read

                If Primerregistro = True Then
                    Primerregistro = False
                    Ind = 0
                    ReDim Me.WebServiceProduccionLinea(Ind)
                    Me.WebServiceProduccionLinea(Ind) = New WebReferenceTiToProduccion.ProduccionDpto



                Else                ' añade un elemento a array de lineas / Objeto
                    ReDim Preserve Me.WebServiceProduccionLinea(UBound(Me.WebServiceProduccionLinea) + 1)
                    Ind = UBound(Me.WebServiceProduccionLinea)
                    Me.WebServiceProduccionLinea(Ind) = New WebReferenceTiToProduccion.ProduccionDpto
                End If



                ' Numero de Documento
                Me.WebServiceProduccionLinea(Ind).Document_No = Me.mParaPrefijoProduccion & Format(Me.mFecha, "yyyyMMdd")

                ' Fecha de Registro
                Me.WebServiceProduccionLinea(Ind).Document_Date = CDate(Format(Me.mFecha, "dd/MM/yyyy"))
                Me.WebServiceProduccionLinea(Ind).Document_DateSpecified = True

                ' Fecha de Documento
                Me.WebServiceProduccionLinea(Ind).Posting_Date = CDate(Format(Me.mFecha, "dd/MM/yyyy"))
                Me.WebServiceProduccionLinea(Ind).Posting_DateSpecified = True

                ' Tipo de Movimiento (Cuenta)
                ' PDTE: Falta saber que campo es en el web service
                'Me.mParaTMovCuenta
                ' 

                ' Falta saber cual es el campo tipo de movimiento en este web service

                '  Me.WebServiceProduccionLinea(Ind).Line_No

                ' test usar num linea 
                '  Me.WebServiceProduccionLinea(Ind).Line_No = Ind
                ' Me.WebServiceProduccionLinea(Ind).Line_NoSpecified = True


                ' Cuenta
                Me.WebServiceProduccionLinea(Ind).Account_No = CStr(Me.DbLeeCentral.mDbLector.Item("CUENTA"))
                ' Concepto
                Me.WebServiceProduccionLinea(Ind).Description = CStr(Me.DbLeeCentral.mDbLector.Item("CONCEPTO"))


                ' Importes
                If CStr(Me.DbLeeCentral.mDbLector.Item("DEBE")) <> "0" Then
                    Me.WebServiceProduccionLinea(Ind).Debit_Amount = CDec(Me.DbLeeCentral.mDbLector.Item("DEBE"))
                    Me.WebServiceProduccionLinea(Ind).Debit_AmountSpecified = True

                    Me.WebServiceProduccionLinea(Ind).Credit_Amount = CDec("0")
                    Me.WebServiceProduccionLinea(Ind).Credit_AmountSpecified = True
                Else
                    Me.WebServiceProduccionLinea(Ind).Credit_Amount = CDec(Me.DbLeeCentral.mDbLector.Item("HABER"))
                    Me.WebServiceProduccionLinea(Ind).Credit_AmountSpecified = True

                    Me.WebServiceProduccionLinea(Ind).Debit_Amount = CDec("0")
                    Me.WebServiceProduccionLinea(Ind).Debit_AmountSpecified = True

                End If

                ' Dimension Naturaleza
                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA")) = False Then

                    If Me.mParaEquivNat = 1 Then
                        Me.WebServiceProduccionLinea(Ind).Shortcut_Dimension_1_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA"))
                    ElseIf Me.mParaEquivNat = 2 Then
                        Me.WebServiceProduccionLinea(Ind).Shortcut_Dimension_2_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA"))
                    ElseIf Me.mParaEquivNat = 3 Then
                        Me.WebServiceProduccionLinea(Ind).ShortcutDimCode3 = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA"))
                    End If
                End If

                ' Dimension Departamento
                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO")) = False Then
                    If Me.mParaEquivDep = 1 Then
                        Me.WebServiceProduccionLinea(Ind).Shortcut_Dimension_1_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO"))
                    ElseIf Me.mParaEquivDep = 2 Then
                        Me.WebServiceProduccionLinea(Ind).Shortcut_Dimension_2_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO"))
                    ElseIf Me.mParaEquivDep = 3 Then
                        Me.WebServiceProduccionLinea(Ind).ShortcutDimCode3 = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO"))
                    End If
                End If

                ' Dimension Acceso Hotel

                If Me.mParaEquivHot = 1 Then
                    Me.WebServiceProduccionLinea(Ind).Shortcut_Dimension_1_Code = Me.mParaDimensionHotel
                ElseIf Me.mParaEquivHot = 2 Then
                    Me.WebServiceProduccionLinea(Ind).Shortcut_Dimension_2_Code = Me.mParaDimensionHotel
                ElseIf Me.mParaEquivHot = 3 Then
                    Me.WebServiceProduccionLinea(Ind).ShortcutDimCode3 = Me.mParaDimensionHotel
                End If





                ' Documento Externo 
                Me.WebServiceProduccionLinea(Ind).External_Document_No = ""



            End While
            Me.DbLeeCentral.mDbLector.Close()


            ' LLAMADA AL WEB SERVICE

            If IsNothing(WebServiceProduccionLinea) = False Then

                If Me.WebServiceEnviarNewhotel(1, 0, "", 0) = True Then
                    ' destruye el web wervice
                    Me.WebServiceProduccionBase = Nothing
                    If Me.mDebugFileEstaOk Then
                        Me.mDebugFile.WriteLine(Now & " Produccion = Nothing")
                    End If
                    ' Gestion de Error
                    Me.WebServiveTrataenviosNewHotel(1, "OK", 1, "", "WEBPRODUCCION", 0, "")
                    Me.mDebugFile.WriteLine(Now & " Produccion = " & "Ok")

                Else
                    ' destruye el web wervice
                    Me.WebServiceProduccionBase = Nothing
                    If Me.mDebugFileEstaOk Then
                        Me.mDebugFile.WriteLine(Now & " Produccion = Nothing")
                    End If
                    ' Gestion de Error
                    Me.WebServiveTrataenviosNewHotel(0, Me.mWebServiceError, 1, "", "WEBPRODUCCION", 0, "")
                    Me.mDebugFile.WriteLine(Now & " Produccion = " & Me.mWebServiceError)
                End If

            End If





        Catch ex As Exception
            ' destruye el web wervice
            If IsNothing(Me.WebServiceProduccionBase) = False Then
                Me.WebServiceProduccionBase = Nothing
            End If

            If Me.mDebugFileEstaOk Then
                Me.mDebugFile.WriteLine(Now & " Produccion = Nothing")
            End If
            ' Gestion de Error
            Me.WebServiveTrataenviosNewHotel(0, Me.mWebServiceError, 1, "", "WEBPRODUCCION", 0, "")
            Me.mDebugFile.WriteLine(Now & " Produccion = " & Me.mWebServiceError)
        End Try
    End Sub


    Private Sub ProcesarAnticipos()

        Dim Primerregistro As Boolean = True

        Try

            SQL = "SELECT  ASNT_F_ATOCAB As FECHA, NVL(ASNT_CFCTA_COD,' ') AS CUENTA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,"
            SQL += "NVL(ASNT_DEBE,'0')  AS DEBE,NVL(ASNT_HABER,'0')  AS HABER,ASNT_DPTO_CODI,ASNT_AUXILIAR_STRING"
            SQL += ", ASNT_MORA_DIMENNATURALEZA, ASNT_MORA_DIMENDPTO, ASNT_MORA_DIMENACCESO "
            SQL += ", NVL(ASNT_MORA_RESERVA,'?') AS  ASNT_MORA_RESERVA"
            SQL += ", NVL(ASNT_MORA_NDOCUMENTO,'?') AS  ASNT_MORA_NDOCUMENTO"
            SQL += " ,ASNT_MORA_TIPODOC,ASNT_MORA_TIPOMOV "


            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpgrupo_Cod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmp_Cod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmp_Num
            'SQL += " AND  ASNT_CFATOCAB_REFER = 2"
            SQL += " AND  ASNT_WEBSERVICE_NAME  = 'WEBANTICIPOS'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"


            Me.DbLeeCentral.TraerLector(SQL)

            While Me.DbLeeCentral.mDbLector.Read

                If Primerregistro = True Then
                    Primerregistro = False
                    Ind = 0
                    ReDim Me.WebServiceAnticiposLinea(Ind)
                    Me.WebServiceAnticiposLinea(Ind) = New WebReferenceTiToAnticiposRecibidos.ConstitucionAnticipos



                Else                ' añade un elemento a array de lineas / Objeto
                    ReDim Preserve Me.WebServiceAnticiposLinea(UBound(Me.WebServiceAnticiposLinea) + 1)
                    Ind = UBound(Me.WebServiceAnticiposLinea)
                    Me.WebServiceAnticiposLinea(Ind) = New WebReferenceTiToAnticiposRecibidos.ConstitucionAnticipos
                End If



                ' Numero de Documento
                Me.WebServiceAnticiposLinea(Ind).Document_No = Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_NDOCUMENTO")

                ' Fecha de Registro
                Me.WebServiceAnticiposLinea(Ind).Document_Date = CDate(Format(Me.mFecha, "dd/MM/yyyy"))
                Me.WebServiceAnticiposLinea(Ind).Document_DateSpecified = True

                ' Fecha de Documento
                Me.WebServiceAnticiposLinea(Ind).Posting_Date = CDate(Format(Me.mFecha, "dd/MM/yyyy"))
                Me.WebServiceAnticiposLinea(Ind).Posting_DateSpecified = True





                ' Tipo de Documento (Anticipo/Deposito)

                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPODOC")) Then
                    ' nullo
                    Me.WebServiceAnticiposLinea(Ind).Document_Subtype = WebReferenceTiToAnticiposRecibidos.Document_Subtype._blank_
                    Me.WebServiceAnticiposLinea(Ind).Document_SubtypeSpecified = True
                ElseIf Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPODOC") = Me.mParaTDocDeposito Then
                    ' deposito
                    Me.WebServiceAnticiposLinea(Ind).Document_Subtype = WebReferenceTiToAnticiposRecibidos.Document_Subtype.Deposit
                    Me.WebServiceAnticiposLinea(Ind).Document_SubtypeSpecified = True
                ElseIf Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPODOC") = Me.mParaTDocAnticipos Then
                    ''anticipo
                    Me.WebServiceAnticiposLinea(Ind).Document_Subtype = WebReferenceTiToAnticiposRecibidos.Document_Subtype.Advance
                    Me.WebServiceAnticiposLinea(Ind).Document_SubtypeSpecified = True
                Else
                    Me.WebServiceAnticiposLinea(Ind).Document_Subtype = WebReferenceTiToAnticiposRecibidos.Document_Subtype._blank_
                    Me.WebServiceAnticiposLinea(Ind).Document_SubtypeSpecified = True
                End If




                ' Tipo de Mov (Cuenta / Banco)

                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPOMOV")) Then
                    ' nullo
                    Me.WebServiceAnticiposLinea(Ind).Account_TypeSpecified = False

                ElseIf CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPOMOV")) = Me.mParaTMovCliente Then
                    Me.WebServiceAnticiposLinea(Ind).Account_Type = WebReferenceTiToAnticiposRecibidos.Account_Type.Customer
                    Me.WebServiceAnticiposLinea(Ind).Account_TypeSpecified = True
                ElseIf CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPOMOV")) = Me.mParaTMovBanco Then
                    Me.WebServiceAnticiposLinea(Ind).Account_Type = WebReferenceTiToAnticiposRecibidos.Account_Type.Bank_Account
                    Me.WebServiceAnticiposLinea(Ind).Account_TypeSpecified = True
                Else
                    ' OTRO
                    Me.WebServiceAnticiposLinea(Ind).Account_TypeSpecified = False
                End If

                ' 

                ' 
                ' Cuenta

                Me.WebServiceAnticiposLinea(Ind).Account_No = CStr(Me.DbLeeCentral.mDbLector.Item("CUENTA"))

                ' Concepto
                Me.WebServiceAnticiposLinea(Ind).Description = CStr(Me.DbLeeCentral.mDbLector.Item("CONCEPTO"))


                ' Importes
                If CStr(Me.DbLeeCentral.mDbLector.Item("DEBE")) <> "0" Then
                    Me.WebServiceAnticiposLinea(Ind).Debit_Amount = CDec(Me.DbLeeCentral.mDbLector.Item("DEBE"))
                    Me.WebServiceAnticiposLinea(Ind).Debit_AmountSpecified = True

                    Me.WebServiceAnticiposLinea(Ind).Credit_Amount = CDec("0")
                    Me.WebServiceAnticiposLinea(Ind).Credit_AmountSpecified = True
                Else
                    Me.WebServiceAnticiposLinea(Ind).Credit_Amount = CDec(Me.DbLeeCentral.mDbLector.Item("HABER"))
                    Me.WebServiceAnticiposLinea(Ind).Credit_AmountSpecified = True

                    Me.WebServiceAnticiposLinea(Ind).Debit_Amount = CDec("0")
                    Me.WebServiceAnticiposLinea(Ind).Debit_AmountSpecified = True

                End If



                ' Dimension Naturaleza
                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA")) = False Then

                    If Me.mParaEquivNat = 1 Then
                        Me.WebServiceAnticiposLinea(Ind).Shortcut_Dimension_1_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA"))
                    ElseIf Me.mParaEquivNat = 2 Then
                        Me.WebServiceAnticiposLinea(Ind).Shortcut_Dimension_2_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA"))
                    ElseIf Me.mParaEquivNat = 3 Then
                        Me.WebServiceAnticiposLinea(Ind).ShortcutDimCode3 = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA"))
                    End If
                End If

                ' Dimension Departamento
                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO")) = False Then

                    If Me.mParaEquivDep = 1 Then
                        Me.WebServiceAnticiposLinea(Ind).Shortcut_Dimension_1_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO"))
                    ElseIf Me.mParaEquivDep = 2 Then
                        Me.WebServiceAnticiposLinea(Ind).Shortcut_Dimension_2_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO"))
                    ElseIf Me.mParaEquivDep = 3 Then
                        Me.WebServiceAnticiposLinea(Ind).ShortcutDimCode3 = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO"))
                    End If
                End If

                ' Dimension Acceso Hotel

                If Me.mParaEquivHot = 1 Then
                    Me.WebServiceAnticiposLinea(Ind).Shortcut_Dimension_1_Code = Me.mParaDimensionHotel
                ElseIf Me.mParaEquivHot = 2 Then
                    Me.WebServiceAnticiposLinea(Ind).Shortcut_Dimension_2_Code = Me.mParaDimensionHotel
                ElseIf Me.mParaEquivHot = 3 Then
                    Me.WebServiceAnticiposLinea(Ind).ShortcutDimCode3 = Me.mParaDimensionHotel
                End If



                ' Documento Externo 
                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_RESERVA")) = False Then
                    Me.WebServiceAnticiposLinea(Ind).External_Document_No = Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_RESERVA")
                End If




            End While
            Me.DbLeeCentral.mDbLector.Close()


            ' LLAMADA AL WEB SERVICE

            If IsNothing(Me.WebServiceAnticiposLinea) = False Then

                If Me.WebServiceEnviarNewhotel(2, 0, "", 0) = True Then
                    ' destruye el web wervice
                    Me.WebServiceAnticiposBase = Nothing
                    If Me.mDebugFileEstaOk Then
                        Me.mDebugFile.WriteLine(Now & " Anticipos = Nothing")
                    End If
                    ' Gestion de Error
                    Me.WebServiveTrataenviosNewHotel(1, "OK", 2, "", "WEBANTICIPOS", 0, "")
                    Me.mDebugFile.WriteLine(Now & " Anticipos = " & "Ok")

                Else
                    ' destruye el web wervice
                    Me.WebServiceAnticiposBase = Nothing
                    If Me.mDebugFileEstaOk Then
                        Me.mDebugFile.WriteLine(Now & " Anticipos = Nothing")
                    End If
                    ' Gestion de Error
                    Me.WebServiveTrataenviosNewHotel(0, Me.mWebServiceError, 2, "", "WEBANTICIPOS", 0, "")
                    Me.mDebugFile.WriteLine(Now & " Anticipos = " & Me.mWebServiceError)
                End If


            End If


        Catch ex As Exception
            ' destruye el web wervice
            If IsNothing(Me.WebServiceAnticiposBase) = False Then
                Me.WebServiceAnticiposBase = Nothing
            End If

            If Me.mDebugFileEstaOk Then
                Me.mDebugFile.WriteLine(Now & " Anticipos = Nothing")
            End If
            ' Gestion de Error
            Me.WebServiveTrataenviosNewHotel(0, ex.Message, 2, "", "WEBANTICIPOS", 0, "")
            Me.mDebugFile.WriteLine(Now & " Anticipos = " & ex.Message)
        End Try
    End Sub
    Private Sub ProcesarAnticiposFacturados()

        Dim Primerregistro As Boolean = True

        Try

            SQL = "SELECT  ASNT_F_ATOCAB As FECHA, NVL(ASNT_CFCTA_COD,' ') AS CUENTA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,"
            SQL += "NVL(ASNT_DEBE,'0')  AS DEBE,NVL(ASNT_HABER,'0')  AS HABER,ASNT_DPTO_CODI,ASNT_AUXILIAR_STRING"
            SQL += ", ASNT_MORA_DIMENNATURALEZA, ASNT_MORA_DIMENDPTO, ASNT_MORA_DIMENACCESO "
            SQL += " ,ASNT_MORA_TIPODOC,ASNT_MORA_TIPOMOV "
            SQL += ",ASNT_FACTURA_NUMERO AS NUMERO "
            SQL += ",ASNT_FACTURA_SERIE AS SERIE "
            SQL += ",ASNT_MORA_LIQUIDAR  "
            SQL += ",ASNT_MORA_RESERVA "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpgrupo_Cod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmp_Cod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmp_Num
            'SQL += " AND  ASNT_CFATOCAB_REFER = 2"
            SQL += " AND  ASNT_WEBSERVICE_NAME  = 'WEBANTICIPOS FACTURADOS'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"


            Me.DbLeeCentral.TraerLector(SQL)

            While Me.DbLeeCentral.mDbLector.Read





                If Primerregistro = True Then
                    Primerregistro = False
                    Ind = 0
                    ReDim Me.WebServiceAnticiposAplicadosLinea(Ind)
                    Me.WebServiceAnticiposAplicadosLinea(Ind) = New WebReferenceTiToAnticiposFacturados.AplicacionAnticiposFactura



                Else                ' añade un elemento a array de lineas / Objeto
                    ReDim Preserve Me.WebServiceAnticiposAplicadosLinea(UBound(Me.WebServiceAnticiposAplicadosLinea) + 1)
                    Ind = UBound(Me.WebServiceAnticiposAplicadosLinea)
                    Me.WebServiceAnticiposAplicadosLinea(Ind) = New WebReferenceTiToAnticiposFacturados.AplicacionAnticiposFactura
                End If




                ' Numero de Documento ( El Numero de Factura)
                Me.WebServiceAnticiposAplicadosLinea(Ind).Document_No = CStr(Me.DbLeeCentral.mDbLector.Item("NUMERO")) & "/" & CStr(Me.DbLeeCentral.mDbLector.Item("SERIE"))

                ' Fecha de Registro
                Me.WebServiceAnticiposAplicadosLinea(Ind).Document_Date = CDate(Format(Me.mFecha, "dd/MM/yyyy"))
                Me.WebServiceAnticiposAplicadosLinea(Ind).Document_DateSpecified = True

                ' Fecha de Documento
                Me.WebServiceAnticiposAplicadosLinea(Ind).Posting_Date = CDate(Format(Me.mFecha, "dd/MM/yyyy"))
                Me.WebServiceAnticiposAplicadosLinea(Ind).Posting_DateSpecified = True


                ' Tipo de Documento (Anticipo/Deposito)

                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPODOC")) Then
                    ' nullo
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Document_Subtype = WebReferenceTiToAnticiposFacturados.Document_Subtype._blank_
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Document_SubtypeSpecified = True

                ElseIf Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPODOC") = Me.mParaTDocDeposito Then
                    ' deposito
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Document_Subtype = WebReferenceTiToAnticiposFacturados.Document_Subtype.Deposit
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Document_SubtypeSpecified = True
                ElseIf Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPODOC") = Me.mParaTDocAnticipos Then
                    ''anticipo
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Document_Subtype = WebReferenceTiToAnticiposFacturados.Document_Subtype.Advance
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Document_SubtypeSpecified = True
                Else
                    ' otro

                    Me.WebServiceAnticiposAplicadosLinea(Ind).Document_Subtype = WebReferenceTiToAnticiposFacturados.Document_Subtype._blank_
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Document_SubtypeSpecified = True
                End If




                ' Tipo de Mov (Cuenta / Banco)

                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPOMOV")) Then
                    ' nullo
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Account_TypeSpecified = False

                ElseIf CStr((Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPOMOV"))) = Me.mParaTMovCuenta Then
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Account_Type = WebReferenceTiToAnticiposFacturados.Account_Type.G_L_Account
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Account_TypeSpecified = True
                ElseIf CStr((Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPOMOV"))) = Me.mParaTMovBanco Then
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Account_Type = WebReferenceTiToAnticiposFacturados.Account_Type.Bank_Account
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Account_TypeSpecified = True
                ElseIf CStr((Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPOMOV"))) = Me.mParaTMovCliente Then
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Account_Type = WebReferenceTiToAnticiposFacturados.Account_Type.Customer
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Account_TypeSpecified = True
                Else
                    ' OTRO
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Account_TypeSpecified = False

                End If

                ' 


                ' 
                ' Cuenta
                Me.WebServiceAnticiposAplicadosLinea(Ind).Account_No = CStr(Me.DbLeeCentral.mDbLector.Item("CUENTA"))
                ' Concepto
                Me.WebServiceAnticiposAplicadosLinea(Ind).Description = CStr(Me.DbLeeCentral.mDbLector.Item("CONCEPTO"))


                ' Importes
                If CStr(Me.DbLeeCentral.mDbLector.Item("DEBE")) <> "0" Then
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Debit_Amount = CDec(Me.DbLeeCentral.mDbLector.Item("DEBE"))
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Debit_AmountSpecified = True

                    Me.WebServiceAnticiposAplicadosLinea(Ind).Credit_Amount = CDec("0")
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Credit_AmountSpecified = True
                Else
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Credit_Amount = CDec(Me.DbLeeCentral.mDbLector.Item("HABER"))
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Credit_AmountSpecified = True

                    Me.WebServiceAnticiposAplicadosLinea(Ind).Debit_Amount = CDec("0")
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Debit_AmountSpecified = True

                End If

                ' Dimension Naturaleza
                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA")) = False Then

                    If Me.mParaEquivNat = 1 Then
                        Me.WebServiceAnticiposAplicadosLinea(Ind).Shortcut_Dimension_1_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA"))
                    ElseIf Me.mParaEquivNat = 2 Then
                        Me.WebServiceAnticiposAplicadosLinea(Ind).Shortcut_Dimension_2_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA"))
                    ElseIf Me.mParaEquivNat = 3 Then
                        Me.WebServiceAnticiposAplicadosLinea(Ind).ShortcutDimCode3 = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA"))
                    End If
                End If

                ' Dimension Departamento
                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO")) = False Then

                    If Me.mParaEquivDep = 1 Then
                        Me.WebServiceAnticiposAplicadosLinea(Ind).Shortcut_Dimension_1_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO"))
                    ElseIf Me.mParaEquivDep = 2 Then
                        Me.WebServiceAnticiposAplicadosLinea(Ind).Shortcut_Dimension_2_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO"))
                    ElseIf Me.mParaEquivDep = 3 Then
                        Me.WebServiceAnticiposAplicadosLinea(Ind).ShortcutDimCode3 = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO"))
                    End If
                End If

                ' Dimension Acceso Hotel

                If Me.mParaEquivHot = 1 Then
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Shortcut_Dimension_1_Code = Me.mParaDimensionHotel
                ElseIf Me.mParaEquivHot = 2 Then
                    Me.WebServiceAnticiposAplicadosLinea(Ind).Shortcut_Dimension_2_Code = Me.mParaDimensionHotel
                ElseIf Me.mParaEquivHot = 3 Then
                    Me.WebServiceAnticiposAplicadosLinea(Ind).ShortcutDimCode3 = Me.mParaDimensionHotel
                End If

                ' Liquidar por numero de documentos 

                Me.WebServiceAnticiposAplicadosLinea(Ind).Applies_to_Doc_No = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_LIQUIDAR"))

                ' Documento Externo 
                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_RESERVA")) = False Then
                    Me.WebServiceAnticiposAplicadosLinea(Ind).External_Document_No = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_RESERVA"))
                Else
                    Me.WebServiceAnticiposAplicadosLinea(Ind).External_Document_No = ""
                End If

                'PDTE  probar
                ' Liquidar por tipo de documentos ( si se factura un anticipo applyes doc type es blanco)

                'If CStr(Me.DbLeeCentral.mDbLector.Item("CUENTA")) <> Me.mCtaPagosACuenta Then
                'Me.WebServiceAnticiposAplicadosLinea(Ind).Applies_to_Doc_Type = WebReferenceTiToFacturacion.Document_Type._blank_
                'Me.WebServiceAnticiposAplicadosLinea(Ind).Applies_to_Doc_TypeSpecified = True
                'Else
                'Me.WebServiceAnticiposAplicadosLinea(Ind).Applies_to_Doc_Type = WebReferenceTiToFacturacion.Document_Type._blank_
                ' Me.WebServiceAnticiposAplicadosLinea(Ind).Applies_to_Doc_TypeSpecified = True
                ' End If

                ' ojo bloquear al terminar justo arriba
                Me.WebServiceAnticiposAplicadosLinea(Ind).Applies_to_Doc_Type = WebReferenceTiToFacturacion.Document_Type._blank_





            End While
            Me.DbLeeCentral.mDbLector.Close()


            ' LLAMADA AL WEB SERVICE

            If IsNothing(Me.WebServiceAnticiposAplicadosLinea) = False Then
                If Me.WebServiceEnviarNewhotel(350, 0, "", 0) = True Then
                    ' destruye el web wervice
                    Me.WebServiceAnticiposAplicadosBase = Nothing
                    If Me.mDebugFileEstaOk Then
                        Me.mDebugFile.WriteLine(Now & " Anticipos Aplicados = Nothing")
                    End If
                    ' Gestion de Error
                    Me.WebServiveTrataenviosNewHotel(1, "OK", 350, "", "WEBANTICIPOS FACTURADOS", 0, "")
                    Me.mDebugFile.WriteLine(Now & " Anticipos Aplicados = " & "Ok")

                Else
                    ' destruye el web wervice
                    Me.WebServiceAnticiposAplicadosBase = Nothing
                    If Me.mDebugFileEstaOk Then
                        Me.mDebugFile.WriteLine(Now & " Anticipos Aplicados = Nothing")
                    End If
                    ' Gestion de Error
                    Me.WebServiveTrataenviosNewHotel(0, Me.mWebServiceError, 350, "", "WEBANTICIPOS FACTURADOS", 0, "")
                    Me.mDebugFile.WriteLine(Now & " Anticipos Aplicados = " & Me.mWebServiceError)
                End If
            End If
        Catch ex As Exception
            ' destruye el web wervice
            Me.WebServiceAnticiposAplicadosBase = Nothing
            If Me.mDebugFileEstaOk Then
                Me.mDebugFile.WriteLine(Now & " Anticipos Aplicados = Nothing")
            End If
            ' Gestion de Error
            Me.WebServiveTrataenviosNewHotel(0, ex.Message, 350, "", "WEBANTICIPOS FACTURADOS", 0, "")
            Me.mDebugFile.WriteLine(Now & " Anticipos Aplicados = " & ex.Message)
        End Try
    End Sub
    Private Sub ProcesarFacturas()

        Dim Primerregistro As Boolean = True

        Dim ControlSerie As String = ""
        Dim ControlFactura As Integer
        Dim ControlAsiento As Integer

        Try

            SQL = "SELECT  ASNT_F_ATOCAB As FECHA, NVL(ASNT_CFCTA_COD,' ') AS CUENTA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,"
            SQL += "NVL(ASNT_DEBE,'0')  AS DEBE,NVL(ASNT_HABER,'0')  AS HABER"
            SQL += ", ASNT_MORA_DIMENNATURALEZA, ASNT_MORA_DIMENDPTO, ASNT_MORA_DIMENACCESO "
            SQL += ",ASNT_FACTURA_NUMERO AS NUMERO "
            SQL += ",ASNT_FACTURA_SERIE  AS SERIE "
            SQL += ", NVL(ASNT_MORA_RESERVA,'?') AS  ASNT_MORA_RESERVA"
            'SQL += ", ROWID "
            SQL += ",ASNT_MORA_TIPODOC "
            SQL += ",ASNT_MORA_TIPOMOV "
            SQL += ",ASNT_MORA_FAC_IVANEGOCIO, ASNT_MORA_FAC_IVAPRODUCTO, ASNT_MORA_FAC_NEGOCIO,ASNT_MORA_FAC_PRODUCTO"

            ' 20180118
            SQL += ",ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_MORA_COD_PROCEDENCIA "
            '
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpgrupo_Cod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmp_Cod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmp_Num
            '    SQL += " AND  ASNT_CFATOCAB_REFER = 3"
            SQL += " AND  ASNT_WEBSERVICE_NAME  = 'WEBFACTURAS'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "
            '  SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"
            SQL += " ORDER BY ASNT_CFATOCAB_REFER, ASNT_FACTURA_SERIE,ASNT_FACTURA_NUMERO,ASNT_LINEA"



            Me.DbLeeCentral.TraerLector(SQL)

            While Me.DbLeeCentral.mDbLector.Read

                '
                If Primerregistro = True Then
                    Primerregistro = False
                    Ind = 0
                    ReDim Me.WebServiceFacturacionLineas(Ind)
                    Me.WebServiceFacturacionLineas(Ind) = New WebReferenceTiToFacturacion.FacturacionEmitidaCargo

                    ControlSerie = CStr(Me.DbLeeCentral.mDbLector.Item("SERIE"))
                    ControlFactura = CInt(Me.DbLeeCentral.mDbLector.Item("NUMERO"))
                    ' 20180118
                    ControlAsiento = CInt(Me.DbLeeCentral.mDbLector.Item("ASIENTO"))
                Else

                    If IsDBNull(Me.DbLeeCentral.mDbLector.Item("SERIE")) = False And CStr(Me.DbLeeCentral.mDbLector.Item("SERIE")) = ControlSerie And CInt(Me.DbLeeCentral.mDbLector.Item("NUMERO")) = ControlFactura Then
                        '                ' añade un elemento a array de lineas / Objeto
                        ReDim Preserve Me.WebServiceFacturacionLineas(UBound(Me.WebServiceFacturacionLineas) + 1)
                        Ind = UBound(Me.WebServiceFacturacionLineas)
                        Me.WebServiceFacturacionLineas(Ind) = New WebReferenceTiToFacturacion.FacturacionEmitidaCargo
                    End If

                End If



                ' CAMBIO DE FACTURA


                If CStr(Me.DbLeeCentral.mDbLector.Item("SERIE")) <> ControlSerie Or CInt(Me.DbLeeCentral.mDbLector.Item("NUMERO")) <> ControlFactura Then



                    ' LLAMADA AL WEB SERVICE

                    If Me.WebServiceEnviarNewhotel(ControlAsiento, 0, "", 0) = True Then

                        ' destruye el web wervice
                        '  Me.WebServiceFacturacionBase = Nothing
                        Me.WebServiceFacturacionLineasBaseList = Nothing
                        If Me.mDebugFileEstaOk Then
                            Me.mDebugFile.WriteLine(Now & " Facturación = Nothing")
                        End If

                        ' Gestion de Error

                        Me.WebServiveTrataenviosNewHotel(1, "OK", ControlAsiento, "", "WEBFACTURAS", ControlFactura, ControlSerie)
                        Me.mDebugFile.WriteLine(Now & " Facturación = " & "Ok")

                    Else
                        ' destruye el web wervice
                        ' Me.WebServiceFacturacionBase = Nothing
                        Me.WebServiceFacturacionLineasBaseList = Nothing
                        If Me.mDebugFileEstaOk Then
                            Me.mDebugFile.WriteLine(Now & " Facturación = Nothing")
                        End If
                        ' Gestion de Error
                        Me.WebServiveTrataenviosNewHotel(0, Me.mWebServiceError, ControlAsiento, "", "WEBFACTURAS", ControlFactura, ControlSerie)
                        Me.mDebugFile.WriteLine(Now & " Facturación = " & Me.mWebServiceError)

                    End If


                    Ind = 0
                    ReDim Me.WebServiceFacturacionLineas(Ind)
                    Me.WebServiceFacturacionLineas(Ind) = New WebReferenceTiToFacturacion.FacturacionEmitidaCargo

                    ControlSerie = CStr(Me.DbLeeCentral.mDbLector.Item("SERIE"))
                    ControlFactura = CInt(Me.DbLeeCentral.mDbLector.Item("NUMERO"))
                    ControlAsiento = CInt(Me.DbLeeCentral.mDbLector.Item("ASIENTO"))


                End If


                ' journal template

                If Me.mParaJournalTemplate.Length > 0 Then
                    Me.WebServiceFacturacionLineas(Ind).Journal_Template_Name = Me.mParaJournalTemplate
                End If


                ' journal batch NO USAR ES READONLY

                'If Me.mParaJournalBatch.Length > 0 Then
                'Me.WebServiceFacturacionLineas(Ind).Journal_Batch_Name = Me.mParaJournalBatch
                'End If

                '   Me.WebServiceFacturacionLineas(Ind).Line_No = Ind + 1
                Me.WebServiceFacturacionLineas(Ind).Line_No = Ind 




                ' Numero de Documento
                If Me.DbLeeCentral.mDbLector.Item("NUMERO") > 0 Then
                    Me.WebServiceFacturacionLineas(Ind).Document_No = CStr(Me.DbLeeCentral.mDbLector.Item("NUMERO")) & "/" & CStr(Me.DbLeeCentral.mDbLector.Item("SERIE"))

                Else '
                    ' es el apunte de mano corriente
                    Me.WebServiceFacturacionLineas(Ind).Document_No = ""
                End If


                ' Fecha de documento
                Me.WebServiceFacturacionLineas(Ind).Document_Date = CDate(Format(Me.mFecha, "dd/MM/yyyy"))
                Me.WebServiceFacturacionLineas(Ind).Document_DateSpecified = True

                ' Fecha de envio
                ' Me.WebServiceFacturacionLineas(Ind).Posting_Date = CDate(Format(Now, "dd/MM/yyyy"))
                Me.WebServiceFacturacionLineas(Ind).Posting_Date = CDate(Format(Me.mFecha, "dd/MM/yyyy"))
                Me.WebServiceFacturacionLineas(Ind).Posting_DateSpecified = True



                ' Tipo de Documento (Factura/Abono)

                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPODOC")) = False Then
                    If CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPODOC")) = Me.mParaTDocFactura Then
                        ' factura
                        Me.WebServiceFacturacionLineas(Ind).Document_Type = WebReferenceTiToFacturacion.Document_Type.Invoice
                        Me.WebServiceFacturacionLineas(Ind).Document_TypeSpecified = True
                        ' abono = Factura Negativa y Notas de Abono
                    ElseIf CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPODOC")) = Me.mParaTDocAbono Then
                        Me.WebServiceFacturacionLineas(Ind).Document_Type = WebReferenceTiToFacturacion.Document_Type.Credit_Memo
                        Me.WebServiceFacturacionLineas(Ind).Document_TypeSpecified = True
                    Else
                        Me.WebServiceFacturacionLineas(Ind).Document_Type = WebReferenceTiToFacturacion.Document_Type._blank_
                        Me.WebServiceFacturacionLineas(Ind).Document_TypeSpecified = True
                    End If
                Else
                    Me.WebServiceFacturacionLineas(Ind).Document_Type = WebReferenceTiToFacturacion.Document_Type._blank_
                    Me.WebServiceFacturacionLineas(Ind).Document_TypeSpecified = True

                End If




                ' Tipo de Movimiento(Cuenta / Cliente)
                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPOMOV")) = False Then
                    If CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPOMOV")) = Me.mParaTMovCliente Then
                        ' CLIENTE
                        Me.WebServiceFacturacionLineas(Ind).Account_Type = WebReferenceTiToFacturacion.Account_Type.Customer
                        Me.WebServiceFacturacionLineas(Ind).Account_TypeSpecified = True

                    ElseIf CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPOMOV")) = Me.mParaTMovCuenta Then
                        ' CUENTA
                        Me.WebServiceFacturacionLineas(Ind).Account_Type = WebReferenceTiToFacturacion.Account_Type.G_L_Account
                        Me.WebServiceFacturacionLineas(Ind).Account_TypeSpecified = True
                    Else
                        ' OTRO
                        Me.WebServiceFacturacionLineas(Ind).Account_TypeSpecified = False

                    End If
                Else
                    ' OTRO
                    Me.WebServiceFacturacionLineas(Ind).Account_TypeSpecified = False
                End If



                ' Cuenta


                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("CUENTA")) = False Then
                    Me.WebServiceFacturacionLineas(Ind).Account_No = CStr(Me.DbLeeCentral.mDbLector.Item("CUENTA"))
                Else
                    Me.WebServiceFacturacionLineas(Ind).Account_No = "?"
                End If




                ' Concepto
                Me.WebServiceFacturacionLineas(Ind).Description = CStr(Me.DbLeeCentral.mDbLector.Item("CONCEPTO"))


                ' Importes
                If CStr(Me.DbLeeCentral.mDbLector.Item("DEBE")) <> "0" Then
                    Me.WebServiceFacturacionLineas(Ind).Debit_Amount = CDec(Me.DbLeeCentral.mDbLector.Item("DEBE"))
                    Me.WebServiceFacturacionLineas(Ind).Debit_AmountSpecified = True

                    Me.WebServiceFacturacionLineas(Ind).Credit_Amount = CDec("0")
                    Me.WebServiceFacturacionLineas(Ind).Credit_AmountSpecified = True
                Else
                    Me.WebServiceFacturacionLineas(Ind).Credit_Amount = CDec(Me.DbLeeCentral.mDbLector.Item("HABER"))
                    Me.WebServiceFacturacionLineas(Ind).Credit_AmountSpecified = True

                    Me.WebServiceFacturacionLineas(Ind).Debit_Amount = CDec("0")
                    Me.WebServiceFacturacionLineas(Ind).Debit_AmountSpecified = True

                End If

                ' Dimension Naturaleza
                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA")) = False Then

                    If Me.mParaEquivNat = 1 Then
                        Me.WebServiceFacturacionLineas(Ind).Shortcut_Dimension_1_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA"))
                    ElseIf Me.mParaEquivNat = 2 Then
                        Me.WebServiceFacturacionLineas(Ind).Shortcut_Dimension_2_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA"))
                    ElseIf Me.mParaEquivNat = 3 Then
                        Me.WebServiceFacturacionLineas(Ind).ShortcutDimCode3 = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA"))
                    End If
                End If

                ' Dimension Departamento
                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO")) = False Then

                    If Me.mParaEquivDep = 1 Then
                        Me.WebServiceFacturacionLineas(Ind).Shortcut_Dimension_1_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO"))
                    ElseIf Me.mParaEquivDep = 2 Then
                        Me.WebServiceFacturacionLineas(Ind).Shortcut_Dimension_2_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO"))
                    ElseIf Me.mParaEquivDep = 3 Then
                        Me.WebServiceFacturacionLineas(Ind).ShortcutDimCode3 = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO"))
                    End If
                End If

                ' Dimension Acceso Hotel

                If Me.mParaEquivHot = 1 Then
                    Me.WebServiceFacturacionLineas(Ind).Shortcut_Dimension_1_Code = Me.mParaDimensionHotel
                ElseIf Me.mParaEquivHot = 2 Then
                    Me.WebServiceFacturacionLineas(Ind).Shortcut_Dimension_2_Code = Me.mParaDimensionHotel
                ElseIf Me.mParaEquivHot = 3 Then
                    Me.WebServiceFacturacionLineas(Ind).ShortcutDimCode3 = Me.mParaDimensionHotel
                End If


                ' Documento Externo 

                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_RESERVA")) = False Then
                    Me.WebServiceFacturacionLineas(Ind).External_Document_No = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_RESERVA"))
                End If



                'IMPUESTOS
                ' PDTE: FALTA SABER QUE CAMPO DEL WEB SERVICE ES IVA NEGOCIO = CANARIAS

                '  De los parametros Generales
                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_FAC_IVANEGOCIO")) = False Then
                    Me.WebServiceFacturacionLineas(Ind).VAT_Bus_Posting_Group = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_FAC_IVANEGOCIO"))
                End If


                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_FAC_NEGOCIO")) = False Then
                    Me.WebServiceFacturacionLineas(Ind).Gen_Bus_Posting_Group = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_FAC_NEGOCIO"))
                End If

                ' De Newhotel Tasas

                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_FAC_IVAPRODUCTO")) = False Then
                    Me.WebServiceFacturacionLineas(Ind).VAT_Prod_Posting_Group = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_FAC_IVAPRODUCTO"))
                End If

                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_FAC_PRODUCTO")) = False Then
                    Me.WebServiceFacturacionLineas(Ind).Gen_Prod_Posting_Group = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_FAC_PRODUCTO"))
                End If




                ' source type (Tipo de procedencia) 
                Me.WebServiceFacturacionLineas(Ind).Source_Type = WebReferenceTiToFacturacion.Source_Type.Customer

                ' source type (Codigo  de procedencia)  

                If CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_COD_PROCEDENCIA")) = Me.mParaTMovCuenta Then
                    Me.WebServiceFacturacionLineas(Ind).Source_No = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_COD_PROCEDENCIA"))
                Else
                    Me.WebServiceFacturacionLineas(Ind).Source_No = ""
                End If







            End While
            ' AT END

            ' LLAMADA AL WEB SERVICE

            If IsNothing(Me.WebServiceFacturacionLineas) = False Then

                If Me.WebServiceEnviarNewhotel(ControlAsiento, 0, "", 0) = True Then

                    ' destruye el web wervice
                    '  Me.WebServiceFacturacionBase = Nothing
                    Me.WebServiceFacturacionLineasBaseList = Nothing
                    If Me.mDebugFileEstaOk Then
                        Me.mDebugFile.WriteLine(Now & " Facturación = Nothing")
                    End If

                    ' Gestion de Error

                    Me.WebServiveTrataenviosNewHotel(1, "OK", ControlAsiento, "", "WEBFACTURAS", ControlFactura, ControlSerie)
                    Me.mDebugFile.WriteLine(Now & " Facturación = " & "Ok")

                Else
                    ' destruye el web wervice
                    ' Me.WebServiceFacturacionBase = Nothing
                    Me.WebServiceFacturacionLineasBaseList = Nothing
                    If Me.mDebugFileEstaOk Then
                        Me.mDebugFile.WriteLine(Now & " Facturación = Nothing")
                    End If
                    ' Gestion de Error
                    Me.WebServiveTrataenviosNewHotel(0, Me.mWebServiceError, ControlAsiento, "", "WEBFACTURAS", ControlFactura, ControlSerie)
                    Me.mDebugFile.WriteLine(Now & " Facturación = " & Me.mWebServiceError)

                End If
                Me.DbLeeCentral.mDbLector.Close()

                ' destruye el web wervice BASE 
                Me.WebServiceFacturacionLineasBase = Nothing

            End If


        Catch ex As Exception

            ' destruye el web wervice
            If IsNothing(Me.WebServiceFacturacionLineasBase) = False Then
                Me.WebServiceFacturacionLineasBase = Nothing
            End If

            If Me.mDebugFileEstaOk Then
                Me.mDebugFile.WriteLine(Now & " Facturación = Nothing")
            End If
            ' Gestion de Error
            Me.WebServiveTrataenviosNewHotel(0, ex.Message, ControlAsiento, "", "WEBFACTURAS", CInt(Me.DbLeeCentral.mDbLector.Item("NUMERO")), CStr(Me.DbLeeCentral.mDbLector.Item("SERIE")))
            Me.mDebugFile.WriteLine(Now & " Facturación = " & ex.Message)




        End Try
    End Sub


    Private Sub ProcesarCobros()

        Dim Primerregistro As Boolean = True

        Try

            SQL = "Select  ASNT_F_ATOCAB As FECHA, NVL(ASNT_CFCTA_COD,' ') AS CUENTA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,"
            SQL += "NVL(ASNT_DEBE,'0')  AS DEBE,NVL(ASNT_HABER,'0')  AS HABER"
            SQL += ", ASNT_MORA_DIMENNATURALEZA, ASNT_MORA_DIMENDPTO, ASNT_MORA_DIMENACCESO "
            SQL += ",ASNT_FACTURA_NUMERO AS NUMERO "
            SQL += ",ASNT_FACTURA_SERIE AS SERIE "
            SQL += ",ASNT_MORA_TIPODOC "
            SQL += ",ASNT_MORA_TIPOMOV "
            SQL += ",ASNT_MORA_NDOCUMENTO "
            SQL += ",ASNT_MORA_LIQUIDAR "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpgrupo_Cod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmp_Cod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmp_Num
            ' SQL += " AND  ASNT_CFATOCAB_REFER = 35"
            SQL += " AND  ASNT_WEBSERVICE_NAME  = 'WEBCOBROS'"

            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"


            Me.DbLeeCentral.TraerLector(SQL)

            While Me.DbLeeCentral.mDbLector.Read

                If Primerregistro = True Then
                    Primerregistro = False
                    Ind = 0
                    ReDim Me.WebServiceCobrosLinea(Ind)
                    Me.WebServiceCobrosLinea(Ind) = New WebReferenceTiToPagosCobros.PagosCobrosCajaBanco





                Else                ' añade un elemento a array de lineas / Objeto
                    ReDim Preserve Me.WebServiceCobrosLinea(UBound(Me.WebServiceCobrosLinea) + 1)
                    Ind = UBound(Me.WebServiceCobrosLinea)
                    Me.WebServiceCobrosLinea(Ind) = New WebReferenceTiToPagosCobros.PagosCobrosCajaBanco
                End If



                ' Numero de Documento
                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("NUMERO")) = False Then
                    Me.WebServiceCobrosLinea(Ind).Document_No = CStr(Me.DbLeeCentral.mDbLector.Item("NUMERO")) & "/" & CStr(Me.DbLeeCentral.mDbLector.Item("SERIE"))
                ElseIf IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_NDOCUMENTO")) = False Then
                    Me.WebServiceCobrosLinea(Ind).Document_No = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_NDOCUMENTO"))
                Else
                    ' PDTE NO DE BE DE LLEGAR AQUI O HAY FACTURA O HAY RECIBO
                    Me.WebServiceCobrosLinea(Ind).Document_No = "PDTE"
                End If



                ' Fecha de Registro
                Me.WebServiceCobrosLinea(Ind).Document_Date = CDate(Format(Me.mFecha, "dd/MM/yyyy"))
                Me.WebServiceCobrosLinea(Ind).Document_DateSpecified = True

                ' Fecha de Documento
                Me.WebServiceCobrosLinea(Ind).Posting_Date = CDate(Format(Me.mFecha, "dd/MM/yyyy"))
                Me.WebServiceCobrosLinea(Ind).Posting_DateSpecified = True

                ' Tipo de Documento (Anticipo, Deposito, vacio)

                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPODOC")) = False Then
                    If CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPODOC")) = Me.mParaTDocAnticipos Then
                        ' PDTE: Ojo no esta claro que enumeracion usar aqui en cada caso 
                        Me.WebServiceCobrosLinea(Ind).Document_Subtype = WebReferenceTiToPagosCobros.Document_Subtype.Advance
                        Me.WebServiceCobrosLinea(Ind).Document_SubtypeSpecified = True
                    ElseIf CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPODOC")) = Me.mParaTDocDeposito Then
                        Me.WebServiceCobrosLinea(Ind).Document_Subtype = WebReferenceTiToPagosCobros.Document_Subtype.Deposit
                        Me.WebServiceCobrosLinea(Ind).Document_SubtypeSpecified = True
                    Else
                        Me.WebServiceCobrosLinea(Ind).Document_Subtype = WebReferenceTiToPagosCobros.Document_Subtype._blank_
                        Me.WebServiceCobrosLinea(Ind).Document_SubtypeSpecified = True
                    End If
                Else
                    Me.WebServiceCobrosLinea(Ind).Document_Subtype = WebReferenceTiToPagosCobros.Document_Subtype._blank_
                    Me.WebServiceCobrosLinea(Ind).Document_SubtypeSpecified = True
                End If



                ' Tipo (Banco, Cuenta,Proveedores,Cliente )

                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPOMOV")) = False Then

                    If CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPOMOV")) = Me.mParaTMovBanco Then
                        Me.WebServiceCobrosLinea(Ind).Account_Type = WebReferenceTiToPagosCobros.Account_Type.Bank_Account
                        Me.WebServiceCobrosLinea(Ind).Account_TypeSpecified = True
                    ElseIf CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPOMOV")) = Me.mParaTMovCuenta Then
                        Me.WebServiceCobrosLinea(Ind).Account_Type = WebReferenceTiToPagosCobros.Account_Type.G_L_Account
                        Me.WebServiceCobrosLinea(Ind).Account_TypeSpecified = True

                    ElseIf CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPOMOV")) = Me.mParaTMovCliente Then
                        Me.WebServiceCobrosLinea(Ind).Account_Type = WebReferenceTiToPagosCobros.Account_Type.Customer
                        Me.WebServiceCobrosLinea(Ind).Account_TypeSpecified = True
                    End If
                End If


                ' 

                ' Cuenta
                Me.WebServiceCobrosLinea(Ind).Account_No = CStr(Me.DbLeeCentral.mDbLector.Item("CUENTA"))
                ' Concepto
                Me.WebServiceCobrosLinea(Ind).Description = CStr(Me.DbLeeCentral.mDbLector.Item("CONCEPTO"))


                ' Importes
                If CStr(Me.DbLeeCentral.mDbLector.Item("DEBE")) <> "0" Then
                    Me.WebServiceCobrosLinea(Ind).Debit_Amount = CDec(Me.DbLeeCentral.mDbLector.Item("DEBE"))
                    Me.WebServiceCobrosLinea(Ind).Debit_AmountSpecified = True

                    Me.WebServiceCobrosLinea(Ind).Credit_Amount = CDec("0")
                    Me.WebServiceCobrosLinea(Ind).Credit_AmountSpecified = True
                Else
                    Me.WebServiceCobrosLinea(Ind).Credit_Amount = CDec(Me.DbLeeCentral.mDbLector.Item("HABER"))
                    Me.WebServiceCobrosLinea(Ind).Credit_AmountSpecified = True

                    Me.WebServiceCobrosLinea(Ind).Debit_Amount = CDec("0")
                    Me.WebServiceCobrosLinea(Ind).Debit_AmountSpecified = True

                End If




                ' Dimension Naturaleza
                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA")) = False Then

                    If Me.mParaEquivNat = 1 Then
                        Me.WebServiceCobrosLinea(Ind).Shortcut_Dimension_1_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA"))
                    ElseIf Me.mParaEquivNat = 2 Then
                        Me.WebServiceCobrosLinea(Ind).Shortcut_Dimension_2_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA"))
                    ElseIf Me.mParaEquivNat = 3 Then
                        Me.WebServiceCobrosLinea(Ind).ShortcutDimCode3 = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENNATURALEZA"))
                    End If
                End If

                ' Dimension Departamento
                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO")) = False Then

                    If Me.mParaEquivDep = 1 Then
                        Me.WebServiceCobrosLinea(Ind).Shortcut_Dimension_1_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO"))
                    ElseIf Me.mParaEquivDep = 2 Then
                        Me.WebServiceCobrosLinea(Ind).Shortcut_Dimension_2_Code = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO"))
                    ElseIf Me.mParaEquivDep = 3 Then
                        Me.WebServiceCobrosLinea(Ind).ShortcutDimCode3 = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_DIMENDPTO"))
                    End If
                End If

                ' Dimension Acceso Hotel

                If Me.mParaEquivHot = 1 Then
                    Me.WebServiceCobrosLinea(Ind).Shortcut_Dimension_1_Code = Me.mParaDimensionHotel
                ElseIf Me.mParaEquivHot = 2 Then
                    Me.WebServiceCobrosLinea(Ind).Shortcut_Dimension_2_Code = Me.mParaDimensionHotel
                ElseIf Me.mParaEquivHot = 3 Then
                    Me.WebServiceCobrosLinea(Ind).ShortcutDimCode3 = Me.mParaDimensionHotel
                End If

                ' LIUIDAR POR DOCUMENTO


                ' Numero de Documento
                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_LIQUIDAR")) = False Then
                    Me.WebServiceCobrosLinea(Ind).Applies_to_Doc_No = CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_LIQUIDAR"))
                Else
                    Me.WebServiceCobrosLinea(Ind).Applies_to_Doc_No = ""
                End If

                'PDTE  probar
                ' Liquidar por tipo de documentos 

                If IsDBNull(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPODOC")) = False Then
                    If CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPODOC")) = Me.mParaTDocFactura Then
                        ' factura
                        Me.WebServiceCobrosLinea(Ind).Applies_to_Doc_Type = WebReferenceTiToFacturacion.Document_Type.Invoice
                        Me.WebServiceCobrosLinea(Ind).Applies_to_Doc_TypeSpecified = True
                        ' abono = Factura Negativa y Notas de Abono
                    ElseIf CStr(Me.DbLeeCentral.mDbLector.Item("ASNT_MORA_TIPODOC")) = Me.mParaTDocAbono Then
                        Me.WebServiceCobrosLinea(Ind).Applies_to_Doc_Type = WebReferenceTiToFacturacion.Document_Type.Credit_Memo
                        Me.WebServiceCobrosLinea(Ind).Applies_to_Doc_TypeSpecified = True

                    Else
                        Me.WebServiceCobrosLinea(Ind).Applies_to_Doc_Type = WebReferenceTiToFacturacion.Document_Type._blank_
                        Me.WebServiceCobrosLinea(Ind).Applies_to_Doc_TypeSpecified = True

                    End If
                Else
                    Me.WebServiceCobrosLinea(Ind).Applies_to_Doc_Type = WebReferenceTiToFacturacion.Document_Type._blank_
                    Me.WebServiceCobrosLinea(Ind).Applies_to_Doc_TypeSpecified = True


                End If



            End While
            Me.DbLeeCentral.mDbLector.Close()


            ' LLAMADA AL WEB SERVICE

            If IsNothing(Me.WebServiceCobrosLinea) = False Then
                If Me.WebServiceEnviarNewhotel(35, 0, "", 0) = True Then
                    ' destruye el web wervice
                    Me.WebServiceCobrosBase = Nothing
                    If Me.mDebugFileEstaOk Then
                        Me.mDebugFile.WriteLine(Now & " Cobros = Nothing")
                    End If
                    ' Gestion de Error
                    Me.WebServiveTrataenviosNewHotel(1, "OK", 0, "", "WEBCOBROS", 0, "")
                    Me.mDebugFile.WriteLine(Now & " Cobros = " & "Ok")

                Else
                    ' destruye el web wervice
                    Me.WebServiceCobrosBase = Nothing
                    If Me.mDebugFileEstaOk Then
                        Me.mDebugFile.WriteLine(Now & " Cobros = Nothing")
                    End If
                    ' Gestion de Error
                    Me.WebServiveTrataenviosNewHotel(0, Me.mWebServiceError, 0, "", "WEBCOBROS", 0, "")
                    Me.mDebugFile.WriteLine(Now & " Cobros = " & Me.mWebServiceError)
                End If
            End If



        Catch ex As Exception
            ' destruye el web wervice
            If IsNothing(Me.WebServiceCobrosBase) = False Then
                Me.WebServiceCobrosBase = Nothing
            End If

            If Me.mDebugFileEstaOk Then
                Me.mDebugFile.WriteLine(Now & " Cobros = Nothing")
            End If
            ' Gestion de Error
            Me.WebServiveTrataenviosNewHotel(0, Me.mWebServiceError, 0, "", "WEBCOBROS", 0, "")
            Me.mDebugFile.WriteLine(Now & " Produccion = " & Me.mWebServiceError)
        End Try
    End Sub


    Private Sub ProcesarNewStockAlbaranes()
        Dim Primerregistro As Boolean = True

        Dim ControlSerie As String = ""
        Dim ControlFactura As Integer
        Dim ControlAsiento As Integer

        Try

            SQL = "SELECT  ASNT_F_ATOCAB As FECHA, NVL(ASNT_CFCTA_COD,' ') AS CUENTA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,"
            SQL += "NVL(ASNT_DEBE,'0')  AS DEBE,NVL(ASNT_HABER,'0')  AS HABER"
            SQL += ", ASNT_MORA_DIMENNATURALEZA, ASNT_MORA_DIMENDPTO, ASNT_MORA_DIMENACCESO "
            SQL += ",ASNT_DOCU AS DOCUMENTO "

            SQL += ", NVL(ASNT_MORA_DOCEXTERNO,'?') AS  ASNT_MORA_DOCEXTERNO"
            'SQL += ", ROWID "

            '
            SQL += " FROM TS_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpgrupo_Cod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmp_Cod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmp_Num
            SQL += " AND  ASNT_CFATOCAB_REFER IN(1,3)"
            '    SQL += " AND  ASNT_WEBSERVICE_NAME  = 'WEBFACTURAS'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "
            '  SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"
            SQL += " ORDER BY ASNT_CFATOCAB_REFER, ASNT_MORA_DOCEXTERNO,ASNT_LINEA"



            Me.DbLeeCentral.TraerLector(SQL)

            While Me.DbLeeCentral.mDbLector.Read

                '
                If Primerregistro = True Then
                    Primerregistro = False
                    Ind = 0
                    ReDim Me.WebServiceNewStockAlbaranesLineas(Ind)
                    Me.WebServiceNewStockAlbaranesLineas(Ind) = New WebReferenceTitoNewStockAlbaranes.GestionNewHotel

                    ControlSerie = CStr(Me.DbLeeCentral.mDbLector.Item("SERIE"))
                    ControlFactura = CInt(Me.DbLeeCentral.mDbLector.Item("NUMERO"))
                    ' 20180118
                    ControlAsiento = CInt(Me.DbLeeCentral.mDbLector.Item("ASIENTO"))
                Else

                    If IsDBNull(Me.DbLeeCentral.mDbLector.Item("SERIE")) = False And CStr(Me.DbLeeCentral.mDbLector.Item("SERIE")) = ControlSerie And CInt(Me.DbLeeCentral.mDbLector.Item("NUMERO")) = ControlFactura Then
                        '                ' añade un elemento a array de lineas / Objeto
                        ReDim Preserve Me.WebServiceNewStockAlbaranesLineas(UBound(Me.WebServiceNewStockAlbaranesLineas) + 1)
                        Ind = UBound(Me.WebServiceNewStockAlbaranesLineas)
                        Me.WebServiceNewStockAlbaranesLineas(Ind) = New WebReferenceTitoNewStockAlbaranes.GestionNewHotel
                    End If

                End If



                ' CAMBIO DE FACTURA


                If CStr(Me.DbLeeCentral.mDbLector.Item("SERIE")) <> ControlSerie Or CInt(Me.DbLeeCentral.mDbLector.Item("NUMERO")) <> ControlFactura Then



                    ' LLAMADA AL WEB SERVICE

                    If Me.WebServiceEnviarNewhotel(ControlAsiento, 0, "", 0) = True Then

                        ' destruye el web wervice
                        '  Me.WebServiceFacturacionBase = Nothing
                        Me.WebServiceNewStockMovimientosBaseList = Nothing
                        If Me.mDebugFileEstaOk Then
                            Me.mDebugFile.WriteLine(Now & " Facturación = Nothing")
                        End If

                        ' Gestion de Error

                        Me.WebServiveTrataenviosNewHotel(1, "OK", ControlAsiento, "", "WEBFACTURAS", ControlFactura, ControlSerie)
                        Me.mDebugFile.WriteLine(Now & " Facturación = " & "Ok")

                    Else
                        ' destruye el web wervice
                        ' Me.WebServiceFacturacionBase = Nothing
                        Me.WebServiceNewStockMovimientosBaseList = Nothing
                        If Me.mDebugFileEstaOk Then
                            Me.mDebugFile.WriteLine(Now & " Facturación = Nothing")
                        End If
                        ' Gestion de Error
                        Me.WebServiveTrataenviosNewHotel(0, Me.mWebServiceError, ControlAsiento, "", "WEBFACTURAS", ControlFactura, ControlSerie)
                        Me.mDebugFile.WriteLine(Now & " Facturación = " & Me.mWebServiceError)

                    End If


                    Ind = 0
                    ReDim Me.WebServiceNewStockAlbaranesLineas(Ind)
                    Me.WebServiceNewStockAlbaranesLineas(Ind) = New WebReferenceTitoNewStockAlbaranes.GestionNewHotel

                    ControlSerie = CStr(Me.DbLeeCentral.mDbLector.Item("SERIE"))
                    ControlFactura = CInt(Me.DbLeeCentral.mDbLector.Item("NUMERO"))
                    ControlAsiento = CInt(Me.DbLeeCentral.mDbLector.Item("ASIENTO"))


                End If


                '



                ' Numero de Documento
                If Me.DbLeeCentral.mDbLector.Item("NUMERO") > 0 Then
                    '          Me.WebServiceNewStockAlbaranesLineas(Ind).Document_No = CStr(Me.DbLeeCentral.mDbLector.Item("NUMERO")) & "/" & CStr(Me.DbLeeCentral.mDbLector.Item("SERIE"))

                Else '
                    ' es el apunte de mano corriente
                    '          Me.WebServiceNewStockAlbaranesLineas(Ind).Document_No = ""
                End If

                '






            End While
            ' AT END

            ' LLAMADA AL WEB SERVICE

            If IsNothing(Me.WebServiceNewStockAlbaranesLineas) = False Then

                If Me.WebServiceEnviarNewhotel(ControlAsiento, 0, "", 0) = True Then

                    ' destruye el web wervice
                    '  Me.WebServiceFacturacionBase = Nothing
                    Me.WebServiceNewStockMovimientosBaseList = Nothing
                    If Me.mDebugFileEstaOk Then
                        Me.mDebugFile.WriteLine(Now & " Facturación = Nothing")
                    End If

                    ' Gestion de Error

                    Me.WebServiveTrataenviosNewHotel(1, "OK", ControlAsiento, "", "WEBFACTURAS", ControlFactura, ControlSerie)
                    Me.mDebugFile.WriteLine(Now & " Facturación = " & "Ok")

                Else
                    ' destruye el web wervice
                    ' Me.WebServiceFacturacionBase = Nothing
                    Me.WebServiceNewStockMovimientosBaseList = Nothing
                    If Me.mDebugFileEstaOk Then
                        Me.mDebugFile.WriteLine(Now & " Facturación = Nothing")
                    End If
                    ' Gestion de Error
                    Me.WebServiveTrataenviosNewHotel(0, Me.mWebServiceError, ControlAsiento, "", "WEBFACTURAS", ControlFactura, ControlSerie)
                    Me.mDebugFile.WriteLine(Now & " Facturación = " & Me.mWebServiceError)

                End If
                Me.DbLeeCentral.mDbLector.Close()

                ' destruye el web wervice BASE 
                Me.WebServiceNewStockMovimientosBaseList = Nothing

            End If


        Catch ex As Exception

            ' destruye el web wervice
            If IsNothing(Me.WebServiceNewStockMovimientosBaseList) = False Then
                Me.WebServiceNewStockMovimientosBaseList = Nothing
            End If

            If Me.mDebugFileEstaOk Then
                Me.mDebugFile.WriteLine(Now & " Facturación = Nothing")
            End If
            ' Gestion de Error
            Me.WebServiveTrataenviosNewHotel(0, ex.Message, ControlAsiento, "", "WEBFACTURAS", CInt(Me.DbLeeCentral.mDbLector.Item("NUMERO")), CStr(Me.DbLeeCentral.mDbLector.Item("SERIE")))
            Me.mDebugFile.WriteLine(Now & " Facturación = " & ex.Message)




        End Try

    End Sub


    Private Sub ProcesarClientes()

        Dim Primerregistro As Boolean = True



        Try




            SQL = "SELECT TRANS_CODI,TRANS_EMPGRUPO_COD,TRANS_EMP_COD,TRANS_EMP_NUM,TRANS_TABL,TRANS_PKEY,"
            '     SQL += " DECODE(TRANS_TYPE,1,'INSERT',2,'UPDATE',3,'DELETE') AS TIPO"
            SQL += "NVL(TRANS_TYPE,0) AS TIPO,TRANS_NAV_KEY,TRANS_CEXT"
            SQL += " ,HOTEL_ODBC "


            SQL += ",NVL(PARA_WEBSERVICE_LOCATION,'?') AS PARA_WEBSERVICE_LOCATION"
            SQL += ",NVL(PARA_WEBSERVICE_ENAME,'?') AS PARA_WEBSERVICE_ENAME"
            SQL += ",NVL(PARA_MORA_PREPROD,'?') AS PARA_MORA_PREPROD "
            SQL += ",NVL(PARA_DOMAIN_NAME,'?') AS PARA_DOMAIN_NAME"
            SQL += ",NVL(PARA_DOMAIN_USER,'?') AS PARA_DOMAIN_USER"
            SQL += ",NVL(PARA_DOMAIN_PWD,'?') AS PARA_DOMAIN_PWD"
            SQL += ",NVL(PARA_WEBSERVICE_TIMEOUT,30000) AS PARA_WEBSERVICE_TIMEOUT"

            SQL += " FROM TG_TRANS,TH_PARA,TH_HOTEL"
            SQL += " WHERE TG_TRANS.TRANS_EMPGRUPO_COD = TH_PARA.PARA_EMPGRUPO_COD"
            SQL += " AND TG_TRANS.TRANS_EMP_COD = TH_PARA.PARA_EMP_COD"
            SQL += " AND TG_TRANS.TRANS_EMP_NUM = TH_PARA.PARA_EMP_NUM"

            SQL += " AND TG_TRANS.TRANS_EMPGRUPO_COD = TH_HOTEL.HOTEL_EMPGRUPO_COD"
            SQL += " AND TG_TRANS.TRANS_EMP_COD = TH_HOTEL.HOTEL_EMP_COD"
            SQL += " AND TG_TRANS.TRANS_EMP_NUM = TH_HOTEL.HOTEL_EMP_NUM"
            ' SOLO SIN PROCESAR
            SQL += " AND TRANS_STAT =  0 "

            SQL += " ORDER BY TRANS_CODI ASC"




            Me.DbLeeCentral.TraerLector(SQL)

            ' debug

            WriteToFile(Now & " " & SQL)




            If Me.DbLeeCentral.mDbLector.HasRows Then

                ' debug
                If Me.mDebugFileEstaOk Then
                    Me.mDebugFile.WriteLine(Now & " has rows")
                End If

                '' Enviar Clientes
                Me.WebServiceClientesBase = New WebReferenceClientesNewHotel.Clientes_Service
                Me.WebServiceClientesDatos = New WebReferenceClientesNewHotel.Clientes


                While Me.DbLeeCentral.mDbLector.Read






                    ' URL
                    Me.WebServiceClientesBase.Url = Me.DbLeeCentral.mDbLector.Item("PARA_WEBSERVICE_LOCATION") & Me.DbLeeCentral.mDbLector.Item("PARA_WEBSERVICE_ENAME") & "/Page/Clientes"

                    ' TimeOut
                    Me.WebServiceClientesBase.Timeout = Me.DbLeeCentral.mDbLector.Item("PARA_WEBSERVICE_TIMEOUT")

                    ' Credenciales
                    Me.WebServiceClientesBase.Credentials = New System.Net.NetworkCredential(Me.DbLeeCentral.mDbLector.Item("PARA_DOMAIN_USER"), Me.DbLeeCentral.mDbLector.Item("PARA_DOMAIN_PWD"), Me.DbLeeCentral.mDbLector.Item("PARA_DOMAIN_NAME"))



                    SQL = "Select ENTI_CODI , ENTI_NOME"
                    SQL += ", ENTI_MORA, ENTI_MOR1, ENTI_LOCA  "
                    SQL += ", TNHT_ENTI.NACI_CODI, NACI_CISO  "
                    SQL += ", ENTI_NUCO  "
                    SQL += ", ENTI_TELG, ENTI_FAXG, ENTI_MAIG, ENTI_MCNU, ENTI_CPCL  "
                    SQL += ", ENTI_NCO2_AF,ENTI_BAFP  "
                    SQL += ", ENTI_DEAN_AF,ENTI_PAMA_AF,ENTI_COCO  "
                    SQL += " FROM TNHT_ENTI, TNHT_NACI "
                    SQL += " WHERE TNHT_ENTI.NACI_CODI = TNHT_NACI.NACI_CODI"
                    SQL += " And ENTI_CODI = '" & Me.DbLeeCentral.mDbLector.Item("TRANS_PKEY") & "'"
                    '   SQL += " AND ENTI_CAUX = '" & Me.DbLeeCentral.mDbLector.Item("TRANS_PKEY") & "'"



                    If IsNothing(Me.DbLeeHotel) Then
                        Me.DbLeeHotel = New C_DATOS.C_DatosOledb(Me.DbLeeCentral.mDbLector.Item("HOTEL_ODBC"), False)
                        Me.DbLeeHotel.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")
                    End If



                    Me.DbLeeHotel.TraerLector(SQL)

                    While Me.DbLeeHotel.mDbLector.Read



                        ' debug
                        If Me.mDebugFileEstaOk Then
                            Me.mDebugFile.WriteLine(Now & " " & SQL)
                        End If



                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_MORA")) = False Then
                            Me.WebServiceClientesDatos.Address = Me.DbLeeHotel.mDbLector.Item("ENTI_MORA")
                        Else
                            Me.WebServiceClientesDatos.Address = ""
                        End If


                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_MOR1")) = False Then
                            Me.WebServiceClientesDatos.Address += " " & Me.DbLeeHotel.mDbLector.Item("ENTI_MOR1")
                        End If

                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_LOCA")) = False Then
                            Me.WebServiceClientesDatos.City = Me.DbLeeHotel.mDbLector.Item("ENTI_LOCA")
                        Else
                            Me.WebServiceClientesDatos.City = ""
                        End If





                        Me.WebServiceClientesDatos.Contact = ""




                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("NACI_CISO")) = False Then
                            Me.WebServiceClientesDatos.Country_Region_Code = Me.DbLeeHotel.mDbLector.Item("NACI_CISO")
                        Else
                            Me.WebServiceClientesDatos.Country_Region_Code = ""
                        End If


                        Me.WebServiceClientesDatos.County = Me.DbLeeHotel.mDbLector.Item("NACI_CODI")






                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_MAIG")) = False Then
                            Me.WebServiceClientesDatos.E_Mail = Me.DbLeeHotel.mDbLector.Item("ENTI_MAIG")
                        Else
                            Me.WebServiceClientesDatos.E_Mail = ""
                        End If


                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_FAXG")) = False Then
                            Me.WebServiceClientesDatos.Fax_No = Me.DbLeeHotel.mDbLector.Item("ENTI_FAXG")
                        Else
                            Me.WebServiceClientesDatos.Fax_No = ""
                        End If





                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_DEAN_AF")) = False Then
                            Me.WebServiceClientesDatos.Gen_Bus_Posting_Group = Me.DbLeeHotel.mDbLector.Item("ENTI_DEAN_AF")
                        Else
                            Me.WebServiceClientesDatos.Gen_Bus_Posting_Group = ""
                        End If



                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_PAMA_AF")) = False Then
                            Me.WebServiceClientesDatos.VAT_Bus_Posting_Group = Me.DbLeeHotel.mDbLector.Item("ENTI_PAMA_AF")
                        Else
                            Me.WebServiceClientesDatos.VAT_Bus_Posting_Group = ""
                        End If



                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_COCO")) = False Then
                            Me.WebServiceClientesDatos.Customer_Posting_Group = Me.DbLeeHotel.mDbLector.Item("ENTI_COCO")
                        Else
                            Me.WebServiceClientesDatos.Customer_Posting_Group = ""
                        End If




                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")) = False Then

                            Me.WebServiceClientesDatos.No = Me.DbLeeHotel.mDbLector.Item("ENTI_CODI")
                        Else

                            Me.WebServiceClientesDatos.No = ""
                        End If



                        '  Me.WebServiceClientesDatos.Key = ""


                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_NOME")) = False Then
                            Me.WebServiceClientesDatos.Name = Me.DbLeeHotel.mDbLector.Item("ENTI_NOME")
                        Else
                            Me.WebServiceClientesDatos.Name = ""
                        End If


                        ' Metodos y Terminos de Pago 

                        ' Se usan los campos  de Codigo de Cuenta contable "moneda extranjera " (Tab Contabilidad)

                        ' y "Forma de Pago" (Tab Banco)  de la de la ficha de la entidad 


                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_NCO2_AF")) = False Then
                            Me.WebServiceClientesDatos.Payment_Terms_Code = Me.DbLeeHotel.mDbLector.Item("ENTI_NCO2_AF")
                        Else
                            Me.WebServiceClientesDatos.Payment_Terms_Code = ""
                        End If

                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_BAFP")) = False Then
                            Me.WebServiceClientesDatos.Payment_Method_Code = Me.DbLeeHotel.mDbLector.Item("ENTI_BAFP")
                        Else
                            Me.WebServiceClientesDatos.Payment_Method_Code = ""
                        End If



                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_TELG")) = False Then
                            Me.WebServiceClientesDatos.Phone_No = Me.DbLeeHotel.mDbLector.Item("ENTI_TELG")
                        Else
                            Me.WebServiceClientesDatos.Phone_No = ""
                        End If



                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_CPCL")) = False Then
                            Me.WebServiceClientesDatos.Post_Code = Me.DbLeeHotel.mDbLector.Item("ENTI_CPCL")
                        Else
                            Me.WebServiceClientesDatos.Post_Code = ""
                        End If






                        If IsDBNull(Me.DbLeeHotel.mDbLector.Item("ENTI_NUCO")) = False Then
                            Me.WebServiceClientesDatos.VAT_Registration_No = Me.DbLeeHotel.mDbLector.Item("ENTI_NUCO")
                        Else
                            Me.WebServiceClientesDatos.VAT_Registration_No = ""
                        End If




                        ' LLAMADA AL WEB SERVICE


                        If IsNothing(Me.WebServiceClientesBase) = False Then
                            If Me.WebServiceEnviarNewhotel(9001, CInt(Me.DbLeeCentral.mDbLector.Item("TIPO")), CStr(Me.DbLeeCentral.mDbLector.Item("TRANS_PKEY")), CInt(Me.DbLeeCentral.mDbLector.Item("TRANS_CODI"))) = True Then

                                ' Gestion de Error
                                Me.WebServiveTrataenviosClientes(1, "OK", Me.DbLeeCentral.mDbLector.Item("TRANS_CODI"))


                            Else

                                ' Gestion de Error
                                Me.WebServiveTrataenviosClientes(0, Me.mWebServiceError, Me.DbLeeCentral.mDbLector.Item("TRANS_CODI"))


                                End If
                            End If


                    End While




                    Me.DbLeeHotel.mDbLector.Close()

                End While

                Me.DbLeeCentral.mDbLector.Close()

                Me.DbLeeHotel.CerrarConexion()
                Me.DbLeeHotel = Nothing

            End If




        Catch ex As Exception


            If IsNothing(Me.DbLeeHotel) = False Then

                Me.DbLeeHotel.CerrarConexion()
                Me.DbLeeHotel = Nothing
            End If


            ' destruye el web wervice
            If IsNothing(Me.WebServiceClientesBase) = False Then
                Me.WebServiceClientesBase = Nothing
            End If


            ' Gestion de Error
            Me.WebServiveTrataenviosClientes(0, Me.mWebServiceError & " + " & ex.Message, Me.DbLeeCentral.mDbLector.Item("TRANS_CODI"))




        Finally

            ' debug
            If Me.mDebugFileEstaOk Then
                Me.mDebugFile.WriteLine(Now & " finally")
            End If

            ' destruye el web wervice
            If IsNothing(Me.WebServiceClientesBase) = False Then
                Me.WebServiceClientesBase = Nothing
            End If
            ' DEBUG 
            '   CerrarFichero()

        End Try
    End Sub


#End Region
#Region "GESTION DE ERRORES"
    Private Function WebServiceEnviarNewhotel(vAsiento As Integer, vAccionMaestro As Integer, vKey As String, vTransCodi As Integer) As Boolean
        Try
            Me.mWebServiceError = ""
            Me.mUrl = ""
            If vAsiento = 1 Then
                Me.mUrl = Me.WebServiceProduccionBase.Url
                Me.WebServiceProduccionList = Me.WebServiceProduccionLinea
                Me.WebServiceProduccionBase.CreateMultiple(Me.WebServiceProduccionList)
                Return True
            End If

            If vAsiento = 2 Then
                Me.mUrl = Me.WebServiceAnticiposBase.Url
                Me.WebServiceAnticiposList = Me.WebServiceAnticiposLinea
                Me.WebServiceAnticiposBase.CreateMultiple(Me.WebServiceAnticiposList)
                Return True
            End If

            ' Facturas Resumidas
            If vAsiento = 3 Or vAsiento = 51 Then

                Me.mUrl = Me.WebServiceFacturacionLineasBase.Url
                Me.WebServiceFacturacionLineasBaseList = Me.WebServiceFacturacionLineas
                Me.WebServiceFacturacionLineasBase.CreateMultiple(Me.WebServiceFacturacionLineasBaseList)
                Return True


            End If


            If vAsiento = 35 Then
                Me.mUrl = Me.WebServiceCobrosBase.Url
                Me.WebServiceCobrosList = Me.WebServiceCobrosLinea
                Me.WebServiceCobrosBase.CreateMultiple(Me.WebServiceCobrosList)
                Return True
            End If

            If vAsiento = 350 Then
                Me.mUrl = Me.WebServiceAnticiposAplicadosBase.Url
                Me.WebServiceAnticiposAplicadosList = Me.WebServiceAnticiposAplicadosLinea
                Me.WebServiceAnticiposAplicadosBase.CreateMultiple(Me.WebServiceAnticiposAplicadosList)
                Return True
            End If


            ' CLIENTES (ENTIDADES)
            If vAsiento = 9001 Then

                Me.mUrl = Me.WebServiceClientesBase.Url
                ' DEJAR ESTA LINEA NO SE LEEN PARAMETROS EN EL ENVIO DE CLIENTES 
                Me.WebServiceClientesBase.Timeout = 90000
                If vAccionMaestro = mEnumAccionMaestro.Insertar Then

                    ' Verificar que el cliente no exista antes de crearlo 

                    Me.WebServiceClientesDatosControl = Nothing

                    Me.WebServiceClientesDatosControl = Me.WebServiceClientesBase.Read(Me.WebServiceClientesDatos.No)

                    ' debug
                    If Me.mDebugFileEstaOk Then
                        Me.mDebugFile.WriteLine(Now & " Preguntando si el cliente " & Me.WebServiceClientesDatos.No & " existe para crearlo")
                    End If


                    If IsNothing(Me.WebServiceClientesDatosControl) = True Then

                        ' debug
                        If Me.mDebugFileEstaOk Then
                            Me.mDebugFile.WriteLine(Now & "el cliente No existe  ")
                        End If

                        ' SI NO EXISTE SE  CREA
                        Me.WebServiceClientesBase.Create(Me.WebServiceClientesDatos)

                        ' SE LEE EL REGISTRO CREADO EN NAVISION Y SE HACE UPDATE A TG_TRANS  CON LE KEY CREADA EN NAVISION 
                        Me.WebServiceClientesDatosControl = Me.WebServiceClientesBase.Read(Me.WebServiceClientesDatos.No)
                        Me.mAuxString = Me.WebServiceClientesDatosControl.Key
                        SQL = "UPDATE TG_TRANS SET TRANS_NAV_KEY = " & "'" & Me.mAuxString & "'"
                        SQL += "WHERE TRANS_CODI = " & vTransCodi
                        Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                        Return True
                    Else
                        Me.mWebServiceError = "Ya existe un cliente con este Código " & Me.WebServiceClientesDatos.No
                        Return False
                    End If


                ElseIf vAccionMaestro = mEnumAccionMaestro.Modificar Then

                    ' Verificar que el cliente  exista antes de MODIFICARLO

                    Me.WebServiceClientesDatosControl = Me.WebServiceClientesBase.Read(Me.WebServiceClientesDatos.No)

                    If IsNothing(Me.WebServiceClientesDatosControl) = True Then

                        'Me.mWebServiceError = "No existe un cliente para ser MODIFICADO con este Código " & Me.WebServiceClientesDatos.No
                        'Return False
                        ' si es una modificacion y el cliente NO existe an Navision se crea !!!!
                        ' se hace asi , por si al crear la entidad No se le asigna el codigo de navision , sino que se asigna mas tarde MODIFICANDO 
                        ' la entidad

                        Me.WebServiceClientesBase.Create(Me.WebServiceClientesDatos)

                        ' SE LEE EL REGISTRO CREADO EN NAVISION Y SE HACE UPDATE A TG_TRANS  CON LE KEY CREADA EN NAVISION 
                        Me.WebServiceClientesDatosControl = Me.WebServiceClientesBase.Read(Me.WebServiceClientesDatos.No)
                        Me.mAuxString = Me.WebServiceClientesDatosControl.Key
                        SQL = "UPDATE TG_TRANS SET TRANS_NAV_KEY = " & "'" & Me.mAuxString & "'"
                        SQL += "WHERE TRANS_CODI = " & vTransCodi
                        Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
                        Return True

                    Else

                        ' validad si el campo key lEido de navision es el mismo que el campo del de TG_TRANS de cuando se creo el registro 
                        ' PDTE : Guardar la key de navision en algun lugar de la entidad para luego poder comparar ?

                        ' If Me.WebServiceClientesDatosControl.Key = vTransNavKey Then
                        If 1 = 1 Then
                            Me.WebServiceClientesDatos.Key = Me.WebServiceClientesDatosControl.Key
                            Me.WebServiceClientesBase.Update(Me.WebServiceClientesDatos)
                            Return True
                        Else
                            Me.mWebServiceError = "El Cliente a Modificar no comparte la key de creación Navision = " & Me.WebServiceClientesDatos.Key & " versus Newhotel =  " & "  Cliente = " & Me.WebServiceClientesDatos.No
                            Return False
                        End If

                        'Me.WebServiceClientesDatos.Key = Me.WebServiceClientesDatosControl.Key
                        'Me.WebServiceClientesBase.Update(Me.WebServiceClientesDatos)
                        'Return True


                    End If




                ElseIf vAccionMaestro = mEnumAccionMaestro.Eliminar Then

                    ' Verificar que el cliente exista antes de Eliminarlo

                    Me.WebServiceClientesDatosControl = Me.WebServiceClientesBase.Read(Me.WebServiceClientesDatos.No)
                    If IsNothing(Me.WebServiceClientesDatosControl) = True Then
                        Me.mWebServiceError = "No existe un cliente para ser ELIMINADO con este Código " & Me.WebServiceClientesDatos.No
                        Return False
                    Else
                        ' PDTE :
                        ' validad si el campo key lido de navision es el mismo que el campo del de TG_TRANS de cuando se creo el registro 
                        '    If Me.WebServiceClientesDatosControl.Key = vTransNavKey Then
                        If 1 = 1 Then
                            Me.WebServiceClientesDatos.Key = Me.WebServiceClientesDatosControl.Key
                            Me.WebServiceClientesBase.Delete(vKey)
                            Return True
                        Else
                            Me.mWebServiceError = "El Cliente a Eliminar  no comparte la key de creación Navision = " & Me.WebServiceClientesDatos.Key & " versus Newhotel =  " & "  Cliente = " & Me.WebServiceClientesDatos.No
                            Return False

                        End If



                    End If




                End If

            End If


            ' asiento no reconocido 
            Return False

        Catch ex As Exception
            Me.mWebServiceError = ex.Message
            Return False
        End Try
    End Function

    Private Sub WebServiveTrataenviosNewHotel(ByVal vStatus As Integer, ByVal vMessage As String, vAsiento As Integer, vRowid As String, vWebServiceName As String, vFacturaNumero As Integer, vFacturaSerie As String)
        Try

            Me.mAuxInteger = Me.mAuxInteger + 1

            SQL = "UPDATE TH_ASNT Set ASNT_AX_STATUS = " & vStatus
            SQL += " , ASNT_ERR_MESSAGE = " & "'[" & Me.mAuxInteger & "] ' || "
            SQL += "TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI:SS') || ' '|| '" & Mid(vMessage, 1, 4000).Replace("'", "''").Trim & "'"

            SQL += "  WHERE ASNT_F_ATOCAB = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpgrupo_Cod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmp_Cod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmp_Num

            If vAsiento > 0 And vAsiento < 350 Then
                SQL += " AND  ASNT_CFATOCAB_REFER = " & vAsiento
            End If


            SQL += " AND  ASNT_WEBSERVICE_NAME = '" & vWebServiceName & "'"


            If vRowid.Length > 0 Then
                SQL += " AND  ROWID = '" & vRowid & "'"
            End If

            If vFacturaSerie.Length > 0 Then
                SQL += " AND  ASNT_FACTURA_NUMERO = " & vFacturaNumero
                SQL += " AND  ASNT_FACTURA_SERIE = " & "'" & vFacturaSerie & "'"
            End If

            ' SOLO MARCA LOS NO ENVIADOS
            SQL += " AND ASNT_AX_STATUS = 0 "

            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mDebugFile.WriteLine(Now & " " & SQL)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub WebServiveTrataenviosClientes(ByVal vStatus As Integer, ByVal vMessage As String, vTransCodi As Integer)
        Try

            Me.mAuxInteger = Me.mAuxInteger + 1

            SQL = "UPDATE TG_TRANS SET TRANS_STAT = " & vStatus
            SQL += " ,TRANS_ERRO = " & "'[" & Me.mAuxInteger & "] ' || "
            SQL += "TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI:SS') || ' '|| '" & Mid(vMessage, 1, 4000).Replace("'", "''").Trim & "'"

            SQL += "  WHERE TRANS_CODI= " & vTransCodi


            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)

            If Me.mDebugFileEstaOk Then
                Me.mDebugFile.WriteLine(Now & " " & SQL)
            End If


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Function WebServiceEnviarNewStock(vAsiento As Integer, vAccionMaestro As Integer, vKey As String, vTransCodi As Integer) As Boolean
        Try
            Me.mWebServiceError = ""
            Me.mUrl = ""
            If vAsiento = 1 Then
                ' Albaranes
                Me.mUrl = Me.WebServiceNewStockMovimientos.Url
                Me.WebServiceNewStockMovimientosBaseList = Me.NewStockMovimientosLineas
                Me.WebServiceNewStockMovimientos.CreateMultiple(Me.WebServiceNewStockMovimientosBaseList)
                Return True
            End If



            ' asiento no reconocido 
            Return False

        Catch ex As Exception
            Me.mWebServiceError = ex.Message
            Return False
        End Try
    End Function
    Private Sub WebServiveTrataenviosNewStock(ByVal vStatus As Integer, ByVal vMessage As String, vAsiento As Integer, vRowid As String, vWebServiceName As String, vFacturaNumero As Integer, vFacturaSerie As String)
        Try

            Me.mAuxInteger = Me.mAuxInteger + 1

            SQL = "UPDATE TS_ASNT Set ASNT_AX_STATUS = " & vStatus
            SQL += " , ASNT_ERR_MESSAGE = " & "'[" & Me.mAuxInteger & "] ' || "
            SQL += "TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI:SS') || ' '|| '" & Mid(vMessage, 1, 4000).Replace("'", "''").Trim & "'"

            SQL += "  WHERE ASNT_F_ATOCAB = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TS_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpgrupo_Cod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_COD = '" & Me.mEmp_Cod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_NUM = " & Me.mEmp_Num

            If vAsiento > 0 And vAsiento < 350 Then
                SQL += " AND  ASNT_CFATOCAB_REFER = " & vAsiento
            End If


            SQL += " AND  ASNT_WEBSERVICE_NAME = '" & vWebServiceName & "'"


            If vRowid.Length > 0 Then
                SQL += " AND  ROWID = '" & vRowid & "'"
            End If

            If vFacturaSerie.Length > 0 Then
                SQL += " AND  ASNT_FACTURA_NUMERO = " & vFacturaNumero
                SQL += " AND  ASNT_FACTURA_SERIE = " & "'" & vFacturaSerie & "'"
            End If

            ' SOLO MARCA LOS NO ENVIADOS
            SQL += " AND ASNT_AX_STATUS = 0 "

            Me.DbGrabaCentral.EjecutaSqlCommit(SQL)
            Me.mDebugFile.WriteLine(Now & " " & SQL)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub WriteToFile(text As String)

        Dim path As String = "C:\ServiceLog" & Format(Now.ToString("yyyy-MM-dd")) & ".txt"

        Using writer As New StreamWriter(path, True)

            writer.WriteLine(String.Format(text, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt")))

            writer.Close()

        End Using

    End Sub
#End Region

End Class
