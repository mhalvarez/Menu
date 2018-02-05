Public Class FormConvertirAxaptaCompras


    ' NOTAS------------------------------------------------------------------
    '  
    '  (1) precio * unidad 
    '  (2) - descuento cabecera
    '  (3) - descuento linea 
    '


    ' Occurs 1
    ' Path y Modificaciones
    '20121105


    '  Occurs 2 
    ' •	Poner el mismo signo a los campos TotalNoVat y Total, el signo de TotalNoVat es el correcto.
    '•	Se están enviando líneas con importe a cero (ejemplo documento 104/2012).
    '•	El log de errores esta mostrando la totalidad del mensaje, recuerda que te preparamos los textos de error entre ## para que mostraras solo estos y el en caso de que se produzca un error y la cadena de error no contenga las  ## mostrar toda la cadena.
    '•	En el informe de errores, la fecha que aparece en la cabecera (fecha valor) no es correcta, por ejemplo para el día 30/07/2012 indica el informe 22/07/2012.
    '•	El informe de errores no tiene la posibilidad de filtrar por fechas.
    '•	El informe de envíos no tiene la posibilidad de filtrar por fechas.
    '•	Hecho en falta el informe de envíos pendientes.
    '•	Seria posible marcar en verde lo ya enviado (sé que parece una tontería pero visualmente ayuda mucho).
    '•	No están parametrizado las retenciones. Te pongo el cuadro de retenciones enviadas por la asesoría fiscal.
    '•	Al revertir un albarán con retenciones, estas no se aplican al albarán revertido, tienes un ejemplo en el entorno de pruebas para el albarán 10/2012 con doc. Referencia 12/2012.
    '•	NewStock permite alterar un albarán ya traspasados.
    '•	Como ya hablamos ¿seria posible que tu recalcularas el campo de control que determina si un descuento se aplica por porcentaje o por valor?, se estaban dando incidencias en el que el valor de campo estaba a 0 (sin descuento) y no a 1 o 2 como debería. Al recalcularlo realizarías el envío de forma correcta evitando este problema.
    '•	En el botón de fechas posible, solo debería mostrar las fechas en las que hay envíos pendiente y esta mostrando 


    Dim mFecha As Date

    Dim mStrConexion As String
    Dim mStrConexionHotel As String
    Private mEmpGrupoCod As String
    Private mEmpCod As String
    Private mEmpNum As String
    Dim SQL As String
    Dim DbLee As C_DATOS.C_DatosOledb
    Dim DbAux As C_DATOS.C_DatosOledb
    Dim DbWrite As C_DATOS.C_DatosOledb

    Dim DbHotel As C_DATOS.C_DatosOledb



    'Parametros Ax

    Private mAXSourceEndPoint As String
    Private mAXDestinationEndPoint As String
    Private mAxDomainName As String
    Private mAxUserName As String
    Private mAxUserPwd As String
    Private mAxOffSetAccount As String

    Private mAxServCodiBonos As String
    Private mAxServCodiBonosAsoc As String
    Private mAxUrl As String
    Private AxError As String
    Private AxErrorLimpio As String

    Dim T1 As String
    Dim T2 As String

    '

    Private Ind As Integer
    Private Dimension As Integer
    Private Elementos As Integer
    Dim TotalErrores As Integer

    Private m_AxError As String
    Dim Linea As String

    Dim AuxErr As String = ""


    Private m_ResponseStr As String
    Private m_ErrorCorto As String

    Private m_Filtro As String = ""

    Private m_TotalBase As Double
    Private m_TotalImpuesto As Double
    Private m_TotalFactura As Double

    Private m_HayRegistros As Boolean = False

    Private m_Recalculos As Integer

    Private mHotelId As Integer

    ' Web Services Albaranes y Objetos Relacionados 
    Private WebAlbaranes As WebReferenceCompras.SAT_NHVendPackingSlipJourService
    Private DocumentoContextoAlbaranes As WebReferenceCompras.DocumentContext






#Region "CONSTRUCTOR"



    Public Sub New(ByVal vfecha As Date, ByVal vStrConexion As String, ByVal vEmpgrupo_Cod As String, ByVal vEmp_Cod As String, ByVal vEmpNum As Integer, ByVal vStrConexionHotel As String)
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()
        Me.mFecha = vfecha

        Me.mEmpGrupoCod = vEmpgrupo_Cod
        Me.mEmpCod = vEmp_Cod
        Me.mEmpNum = vEmpNum
        Me.Text = Me.Text & " " & Me.mFecha & " Grupo : " & vEmpgrupo_Cod & " Empresa : " & vEmp_Cod & " Formato :"
        Me.mStrConexion = vStrConexion
        Me.mStrConexionHotel = vStrConexionHotel
        Me.Update()
    End Sub
#End Region
#Region "FUNCIONES"
    Private Sub MostrarAsiento(ByVal vFiltro As String)
        Try

            SQL = " SELECT DECODE(ASNT_AX_STATUS,0,'No Enviado',1,'Ok Enviado','7','Corregido','8','Omitido','9','Excluido','?') AS ESTADO "
            SQL += ",NVL(ASNT_AUXILIAR_STRING2,'Nulo') AS TIPO "
            SQL += ", ASNT_AX_NIF AS NIF,ASNT_DOCU AS DOCUMENTO,ASNT_CFCTA_COD AS ""CODIGO AX"", "
            SQL += "ASNT_AMPCPTO AS ""PRODUCTO AXAPTA""  ,NVL(ASNT_AUXILIAR_STRING,'?') AS ""PRODUCTO NEWHOTEL"" "
            SQL += ",ASNT_AX_DTO_CABD AS ""Dto Cabe Porce""  "
            SQL += ",ASNT_AX_DTO_CABV AS ""Dto Cabe Valor""  "

            SQL += ",ASNT_AX_TOTAL AS TOTAL   "
            SQL += ",ASNT_AX_TOTAL_BASE AS ""TOTAL BASE""  "

            SQL += ",ASNT_AX_TOTAL_LINEAS AS LINEAS   "
            SQL += ",ASNT_AX_MOVG_CODI || '/' || ASNT_AX_MOVG_ANCI AS REFE  "
            SQL += ",DECODE(ASNT_AX_TIPO,0,'Albaran',1,'Anulado','?') as ""ESTA ANULADO""  "


            SQL += ",ASNT_AX_QNTD AS ""CANTIDAD SERVIDA""   "
            SQL += ",NVL(ASNT_AX_QNTD_BONUS,0) AS BONUS  "
            SQL += ",ASNT_AX_PRUN  AS ""Precio X Unidad""   "

            SQL += ",ASNT_AX_IMPU_LINEA  AS ""Valor Impuesto Linea""   "
            SQL += ",ASNT_AX_DTO_LIND  AS ""Dto Linea Porce""   "
            SQL += ",ASNT_AX_DTO_LINV  AS ""Dto Linea Valor""   "

            SQL += ",ASNT_AX_QNTD_PED  AS ""Cantidad Pedida""   "
            SQL += ",ASNT_AX_NUM_PED  AS ""Número Pedido""   "

            SQL += ",ASNT_AX_MOVG_DORE  AS ""Doc de Referencia""   "

            SQL += ",ASNT_AJUSTE_DTO  AS AJUSTE   "

            SQL += ",ASNT_TIPO_IGIC  AS ""Tipo Impuesto""   "

            SQL += ",'      '  AS NONE   "



            SQL += " FROM TS_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"


            SQL += " AND TS_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum


            If Me.CheckBoxSoloPendintes.Checked Then
                ' SOLO SIN PROCESAR
                SQL += " AND ASNT_AX_STATUS =  0 "
            End If


            If vFiltro.Length > 0 Then
                SQL += " AND TS_ASNT.ASNT_DOCU LIKE  '" & vFiltro & "%'"
            End If

            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"


            '  Me.DataGridAlbaranes.DataSource = DbLee.TraerDataset(SQL, "ASIENTO")
            ' Me.DataGridAlbaranes.DataMember = "ASIENTO"

            Me.DataGridViewAsientos.DataSource = DbLee.TraerDataset(SQL, "ASIENTO")
            Me.DataGridViewAsientos.DataMember = "ASIENTO"

            If Me.DbLee.mDbDataset.Tables("ASIENTO").Rows.Count > 0 Then
                Me.DataGridViewAsientos.ClearSelection()
                Me.m_HayRegistros = True
            Else
                Me.m_HayRegistros = False
            End If


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    
    Private Sub LeeParametros()
        Try
            SQL = "SELECT "
            SQL += "NVL(PARA_SOURCEENDPOINT,'?') AS PARA_SOURCEENDPOINT,"
            SQL += "NVL(PARA_DESTINATIONENDPOINT,'?') AS PARA_DESTINATIONENDPOINT,"
            SQL += "NVL(PARA_DOMAIN_NAME2,'?') AS PARA_DOMAIN_NAME2,"
            SQL += "NVL(PARA_DOMAIN_USER2,'?') AS PARA_DOMAIN_USER2,"
            SQL += "NVL(PARA_DOMAIN_PWD2,'?') AS PARA_DOMAIN_PWD2,"
            SQL += "NVL(PARA_BANCO_AX,'?') AS  PARA_BANCO_AX,"
            SQL += "NVL(PARA_SERV_CODI_BONO,'?') AS PARA_SERV_CODI_BONO,"
            SQL += "NVL(PARA_SERV_CODI_BONOASC,'?') AS PARA_SERV_CODI_BONOASC,"
            SQL += "NVL(PARA_WEBSERVICE_LOCATION2,'?') AS PARA_WEBSERVICE_LOCATION2, "
            SQL += "NVL(PARA_ANTICIPO_AX,'?') AS PARA_ANTICIPO_AX, "
            SQL += "NVL(PARA_CLIENTES_CONTADO_CIF,'?') AS PARA_CLIENTES_CONTADO_CIF, "
            SQL += "PARA_WEBSERVICE_TIMEOUT2  AS PARA_WEBSERVICE_TIMEOUT2 , "

            SQL += "NVL(PARA_CIF_VENTA_NORMAL,'?') AS PARA_CIF_VENTA_NORMAL, "
            SQL += "NVL(PARA_CIF_VENTA_BONOS,'?') AS PARA_CIF_VENTA_BONOS, "
            SQL += "NVL(PARA_CIF_PRODUC_NORMAL,'?') AS PARA_CIF_PRODUC_NORMAL, "
            SQL += "NVL(PARA_CIF_PRODUC_BONOS,'?') AS PARA_CIF_PRODUC_BONOS,"
            SQL += "NVL(PARA_TRANSFERENCIA_AGENCIA,'?') AS PARA_TRANSFERENCIA_AGENCIA,"
            SQL += "NVL(PARA_CONECTA_NEWGOLF,0) AS PARA_CONECTA_NEWGOLF "
            SQL += ",NVL(PARA_HOTEL_AX,0) AS PARA_HOTEL_AX "


            SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum



            Me.DbLee.TraerLector(SQL)
            Me.DbLee.mDbLector.Read()

            If Me.DbLee.mDbLector.HasRows Then
                Me.mAXSourceEndPoint = Me.DbLee.mDbLector.Item("PARA_SOURCEENDPOINT")
                Me.mAXDestinationEndPoint = Me.DbLee.mDbLector.Item("PARA_DESTINATIONENDPOINT")
                Me.mAxDomainName = Me.DbLee.mDbLector.Item("PARA_DOMAIN_NAME2")
                Me.mAxUserName = Me.DbLee.mDbLector.Item("PARA_DOMAIN_USER2")
                Me.mAxUserPwd = Me.DbLee.mDbLector.Item("PARA_DOMAIN_PWD2")

                Me.mAxUrl = Me.DbLee.mDbLector("PARA_WEBSERVICE_LOCATION2")




                PARA_WEBSERVICE_TIMEOUT = Me.DbLee.mDbLector("PARA_WEBSERVICE_TIMEOUT2")
                Me.NumericUpDownWebServiceTimeOut.Value = PARA_WEBSERVICE_TIMEOUT
                Me.TextBoxDebug.Text = " en " & Me.mAxUrl

                Me.mHotelId = Me.DbLee.mDbLector("PARA_HOTEL_AX")

            Else
                Me.DbLee.mDbLector.Close()
            End If
        Catch ex As Exception
            If IsNothing(Me.DbLee.mDbLector) = False Then
                If Me.DbLee.mDbLector.IsClosed = False Then
                    Me.DbLee.mDbLector.Close()
                End If
            End If
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Validar()
        Try

            Me.ListBoxDebug.Items.Clear()
            Me.ListBoxDebug.Update()

            ' familias

            SQL = "SELECT * FROM TNST_FAMI WHERE FAMI_ABRV IS NULL"

            Me.DbHotel.TraerLector(SQL)

            If Me.DbHotel.mDbLector.HasRows Then

                While Me.DbHotel.mDbLector.Read
                    Me.ListBoxDebug.Items.Add("Familia sin Código Axapta = " & Me.DbHotel.mDbLector.Item("FAMI_DESC"))
                End While

            Else

                Me.ListBoxDebug.Items.Add("Familia sin Código Axapta = todas ok ")
            End If
            Me.DbHotel.mDbLector.Close()



            SQL = "SELECT * FROM TNST_FAMI WHERE LENGTH(FAMI_ABRV) <> 3 "

            Me.DbHotel.TraerLector(SQL)

            If Me.DbHotel.mDbLector.HasRows Then
                While Me.DbHotel.mDbLector.Read
                    Me.ListBoxDebug.Items.Add("Familia con Código Axapta Mal Definido (999) = " & Me.DbHotel.mDbLector.Item("FAMI_DESC"))
                End While
               
            Else
                Me.ListBoxDebug.Items.Add("Familia con Código Axapta Mal Definido  = todas ok")

            End If
            Me.DbHotel.mDbLector.Close()

            ' proveedores

            SQL = "SELECT * FROM TNST_FORN WHERE FORN_CNTR IS NULL "

            Me.DbHotel.TraerLector(SQL)

            If Me.DbHotel.mDbLector.HasRows Then
                While Me.DbHotel.mDbLector.Read
                    Me.ListBoxDebug.Items.Add("Proveedor sin Nif  = " & Me.DbHotel.mDbLector.Item("FORN_DESC"))
                End While

            Else
                Me.ListBoxDebug.Items.Add("Proveedor sin Nif  = todos ok ")
            End If
            Me.DbHotel.mDbLector.Close()

        Catch ex As Exception

        End Try
    End Sub
#End Region
#Region "WEB SERVICE"
    Private Sub AxProcesarAlbaranes()
        Try



            Dim Primerregistro As Boolean = False

            Dim ControlFacturaBono As Boolean = False
            Dim ControlFacturaOtrosServicios As Boolean = False

            Dim PackingSlip As New WebReferenceCompras.AxdSAT_NHVendPackingSlipJour()

            Dim PrecioUnidad As Double
            Dim TotalLinaSinImpuesos As Double
            Dim ValorDescuentoCabecera As Double
            Dim ValorDescuentoLinea As Double

            Dim Docreferencia As String

            Me.WebAlbaranes = New WebReferenceCompras.SAT_NHVendPackingSlipJourService

            'Actualiza Url

            Me.WebAlbaranes.Url = Me.mAxUrl & "SAT_NHVendPackingSlipJourService.asmx"


            ' Aactualiza TIME OUT

            If Me.NumericUpDownWebServiceTimeOut.Value > 0 Then

                Me.WebAlbaranes.Timeout = Me.NumericUpDownWebServiceTimeOut.Value * 1000
            End If




            Me.Text += Me.WebAlbaranes.Url
            Me.Update()

            Me.DocumentoContextoAlbaranes = New WebReferenceCompras.DocumentContext
            '  Me.QweryOrdersFacturas = New WebReferenceFacturas.AxdSAT_NHCreateSalesOrdersQuery


            '  Me.WebFacturas.Credentials = System.Net.CredentialCache.DefaultCredentials
            Me.WebAlbaranes.Credentials = New System.Net.NetworkCredential(Me.mAxUserName, Me.mAxUserPwd, Me.mAxDomainName)
            '


            Me.DocumentoContextoAlbaranes.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContextoAlbaranes.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContextoAlbaranes.DestinationEndpoint = Me.mAXDestinationEndPoint
            Me.DocumentoContextoAlbaranes.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)


            ' CABECERA DE ALBARAN



            SQL = "SELECT ASNT_AX_NIF AS NIF,ASNT_DOCU AS DOCUMENTO "
            SQL += ",ASNT_AX_DTO_CABD AS ASNT_AX_DTO_CABD  "
            SQL += ",ASNT_AX_DTO_CABV AS ASNT_AX_DTO_CABV  "

            SQL += ",ASNT_AX_TOTAL AS ASNT_AX_TOTAL  "
            SQL += ",ASNT_AX_TOTAL_BASE AS ASNT_AX_TOTAL_BASE  "

            SQL += ",ASNT_AX_TOTAL_LINEAS AS ASNT_AX_TOTAL_LINEAS   "
            SQL += ",ASNT_AX_MOVG_CODI || '/' || ASNT_AX_MOVG_ANCI AS REFE  "
            SQL += ",ASNT_AX_MOVG_CODI AS ASNT_AX_MOVG_CODI  "
            SQL += ",ASNT_AX_MOVG_ANCI AS ASNT_AX_MOVG_ANCI  "
            SQL += ",DECODE(ASNT_AX_TIPO,0,'-',1,'Anulado','?') as X   "
            SQL += ",ASNT_AUXILIAR_STRING2 AS AUX  "
            SQL += ",ASNT_AX_TIPO AS ANULADO  "
            SQL += ",ASNT_AX_TIPOMOV AS TIPO  "


            ' F ALMACEN
            SQL += ",ASNT_F_VALOR   AS ASNT_F_VALOR   "
            ' F DOCUMENTO
            SQL += ",ASNT_F_ATOCAB  AS ASNT_F_ATOCAB   "

            SQL += ",ASNT_AX_MOVG_DORE  AS ASNT_AX_MOVG_DORE   "

            SQL += " FROM TS_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"

            SQL += " AND TS_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum

            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "


            If Me.m_Filtro.Length > 0 Then
                SQL += " AND TS_ASNT.ASNT_DOCU LIKE  '" & Me.m_Filtro & "%'"
            End If


            SQL += " GROUP BY ASNT_AX_NIF, "
            SQL += "         ASNT_DOCU, "
            SQL += "         ASNT_AX_DTO_CABD, "
            SQL += "         ASNT_AX_DTO_CABV, "
            SQL += "         ASNT_AX_TOTAL, "
            SQL += "         ASNT_AX_TOTAL_BASE, "
            SQL += "         ASNT_AX_TOTAL_LINEAS, "
            SQL += "         ASNT_AX_MOVG_CODI, "
            SQL += "         ASNT_AX_MOVG_ANCI, "
            SQL += "         ASNT_AX_TIPO, "
            SQL += "         ASNT_F_VALOR, "
            SQL += "         ASNT_F_ATOCAB,ASNT_AUXILIAR_STRING2,ASNT_AX_TIPOMOV,ASNT_AX_MOVG_DORE "


            SQL += " ORDER BY ASNT_AX_MOVG_ANCI,ASNT_AX_MOVG_CODI "



            Me.DbLee.TraerLector(SQL)




            While Me.DbLee.mDbLector.Read

                ' CREA A TIMBAQUE EL ELEMENTO 0 
                ReDim PackingSlip.SAT_NHVendPackingSlipJour(0)

                PackingSlip.SAT_NHVendPackingSlipJour(0) = New WebReferenceCompras.AxdEntity_SAT_NHVendPackingSlipJour()



                '  PackingSlip.SAT_NHVendPackingSlipJour(0).class = ""
                PackingSlip.SAT_NHVendPackingSlipJour(0).CounterLines = CInt(Me.DbLee.mDbLector.Item("ASNT_AX_TOTAL_LINEAS"))
                PackingSlip.SAT_NHVendPackingSlipJour(0).DtoPercent = CDec(Me.DbLee.mDbLector.Item("ASNT_AX_DTO_CABD"))
                PackingSlip.SAT_NHVendPackingSlipJour(0).DtoPercentSpecified = True
                PackingSlip.SAT_NHVendPackingSlipJour(0).DtoValue = CDec(Me.DbLee.mDbLector.Item("ASNT_AX_DTO_CABV"))
                PackingSlip.SAT_NHVendPackingSlipJour(0).DtoValueSpecified = True


                ' ES ANULADO
                If CInt(Me.DbLee.mDbLector.Item("ANULADO")) = 0 Then
                    ' NO anulado
                    PackingSlip.SAT_NHVendPackingSlipJour(0).EsAnulacion = WebReferenceCompras.AxdExtType_NoYesId.No
                    PackingSlip.SAT_NHVendPackingSlipJour(0).EsAnulacionSpecified = True
                    PackingSlip.SAT_NHVendPackingSlipJour(0).TotalNoVat = CDec(Me.DbLee.mDbLector.Item("ASNT_AX_TOTAL_BASE"))
                    PackingSlip.SAT_NHVendPackingSlipJour(0).Total = CDec(Me.DbLee.mDbLector.Item("ASNT_AX_TOTAL"))

                Else
                    ' SI  anulado
                    PackingSlip.SAT_NHVendPackingSlipJour(0).EsAnulacion = WebReferenceCompras.AxdExtType_NoYesId.Yes
                    PackingSlip.SAT_NHVendPackingSlipJour(0).EsAnulacionSpecified = True
                    PackingSlip.SAT_NHVendPackingSlipJour(0).TotalNoVat = CDec(Me.DbLee.mDbLector.Item("ASNT_AX_TOTAL_BASE")) * -1
                    PackingSlip.SAT_NHVendPackingSlipJour(0).Total = CDec(Me.DbLee.mDbLector.Item("ASNT_AX_TOTAL")) * -1
                End If




                ' ISBACK
                If CStr(Me.DbLee.mDbLector.Item("TIPO")) = "A" Then
                    If CInt(Me.DbLee.mDbLector.Item("ANULADO")) = 0 Then
                        PackingSlip.SAT_NHVendPackingSlipJour(0).IsBack = WebReferenceCompras.AxdExtType_NoYesId.No

                        PackingSlip.SAT_NHVendPackingSlipJour(0).IsBackSpecified = True
                        PackingSlip.SAT_NHVendPackingSlipJour(0).TotalNoVat = CDec(Me.DbLee.mDbLector.Item("ASNT_AX_TOTAL_BASE"))
                        PackingSlip.SAT_NHVendPackingSlipJour(0).Total = CDec(Me.DbLee.mDbLector.Item("ASNT_AX_TOTAL"))
                    Else
                        ' Path y Modificaciones
                        '20121105
                        PackingSlip.SAT_NHVendPackingSlipJour(0).IsBack = WebReferenceCompras.AxdExtType_NoYesId.No
                        PackingSlip.SAT_NHVendPackingSlipJour(0).IsBackSpecified = True
                        PackingSlip.SAT_NHVendPackingSlipJour(0).TotalNoVat = CDec(Me.DbLee.mDbLector.Item("ASNT_AX_TOTAL_BASE")) * -1
                        PackingSlip.SAT_NHVendPackingSlipJour(0).Total = CDec(Me.DbLee.mDbLector.Item("ASNT_AX_TOTAL")) * -1

                    End If
                End If



                If CStr(Me.DbLee.mDbLector.Item("TIPO")) = "D" Then
                    If CInt(Me.DbLee.mDbLector.Item("ANULADO")) = 0 Then
                        PackingSlip.SAT_NHVendPackingSlipJour(0).IsBack = WebReferenceCompras.AxdExtType_NoYesId.Yes
                        PackingSlip.SAT_NHVendPackingSlipJour(0).IsBackSpecified = True
                        PackingSlip.SAT_NHVendPackingSlipJour(0).TotalNoVat = CDec(Me.DbLee.mDbLector.Item("ASNT_AX_TOTAL_BASE")) * -1
                        PackingSlip.SAT_NHVendPackingSlipJour(0).Total = CDec(Me.DbLee.mDbLector.Item("ASNT_AX_TOTAL")) * -1
                    Else
                        PackingSlip.SAT_NHVendPackingSlipJour(0).IsBack = WebReferenceCompras.AxdExtType_NoYesId.Yes
                        PackingSlip.SAT_NHVendPackingSlipJour(0).IsBackSpecified = True
                        PackingSlip.SAT_NHVendPackingSlipJour(0).TotalNoVat = CDec(Me.DbLee.mDbLector.Item("ASNT_AX_TOTAL_BASE"))
                        PackingSlip.SAT_NHVendPackingSlipJour(0).Total = CDec(Me.DbLee.mDbLector.Item("ASNT_AX_TOTAL"))

                    End If
                End If



                PackingSlip.SAT_NHVendPackingSlipJour(0).PackingSlipId = CStr(Me.DbLee.mDbLector.Item("REFE"))
                PackingSlip.SAT_NHVendPackingSlipJour(0).PackingSlipIdNH = CStr(Me.DbLee.mDbLector.Item("DOCUMENTO"))


                PackingSlip.SAT_NHVendPackingSlipJour(0).RecId = 0
                PackingSlip.SAT_NHVendPackingSlipJour(0).RecIdSpecified = False

            





                '
                PackingSlip.SAT_NHVendPackingSlipJour(0).TransDate = Me.mFecha

                PackingSlip.SAT_NHVendPackingSlipJour(0).ValueDate = CDate(Me.DbLee.mDbLector.Item("ASNT_F_VALOR"))
                ' la linea de abajo ya no se reconoce 

                'PackingSlip.SAT_NHVendPackingSlipJour(0).ValueDateSpecified = True
                PackingSlip.SAT_NHVendPackingSlipJour(0).Vendor = CStr(Me.DbLee.mDbLector.Item("NIF"))

                PackingSlip.SAT_NHVendPackingSlipJour(0).DeliveryDate = CDate(Me.DbLee.mDbLector.Item("ASNT_F_VALOR"))
                PackingSlip.SAT_NHVendPackingSlipJour(0).DeliveryDateSpecified = True

                ' Datos para Gregorio 

                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_ANCI = 0
                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_ANCISpecified = True

                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_CODI = 0
                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_CODISpecified = True

                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_DADO = CDate(Me.DbLee.mDbLector.Item("ASNT_F_ATOCAB"))
                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_DADOSpecified = True

                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_DEST = 0
                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_DESTSpecified = True


                If IsDBNull(Me.DbLee.mDbLector.Item("ASNT_AX_MOVG_DORE")) = False Then
                    Docreferencia = Me.DbLee.mDbLector.Item("ASNT_AX_MOVG_DORE")
                Else
                    Docreferencia = ""
                End If

                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_DORE = Docreferencia
                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_IDDO = ""
                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_IDID = ""
                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_ORIG = 0
                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_ORIGSpecified = True
                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_TIDE = 0
                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_TIDESpecified = True
                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_TIOR = 0
                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_TIORSpecified = True

                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_VATO = 0
                PackingSlip.SAT_NHVendPackingSlipJour(0).MOVG_VATOSpecified = True

                PackingSlip.SAT_NHVendPackingSlipJour(0).PEDG_CODI = 0
                PackingSlip.SAT_NHVendPackingSlipJour(0).PEDG_CODISpecified = True


                PackingSlip.SAT_NHVendPackingSlipJour(0).HotelId = Me.mHotelId



                '..
                '..

                Me.ListBoxDebug.Items.Add("")
                Me.ListBoxDebug.Items.Add("----------------------------------------------------------------------------------------------------------------------------------------------------------------")
                Me.ListBoxDebug.Items.Add(" => MessageId            : " & Me.DocumentoContextoAlbaranes.MessageId.ToString)
                Me.ListBoxDebug.Items.Add(" => SourceEndpoint       : " & Me.DocumentoContextoAlbaranes.SourceEndpoint.ToString)
                Me.ListBoxDebug.Items.Add(" => SourceEndpointUser   : " & Me.DocumentoContextoAlbaranes.SourceEndpointUser.ToString)
                Me.ListBoxDebug.Items.Add(" => DestinationEndpoint  : " & Me.DocumentoContextoAlbaranes.DestinationEndpoint.ToString)
                Me.ListBoxDebug.Items.Add(" ==> Tipo                 : " & CStr(Me.DbLee.mDbLector.Item("AUX")))
                Me.ListBoxDebug.Items.Add(" ==> Fecha                : " & CStr(Me.DbLee.mDbLector.Item("ASNT_F_VALOR")))
                Me.ListBoxDebug.Items.Add(" ==> Fecha Docu.          : " & CStr(Me.DbLee.mDbLector.Item("ASNT_F_ATOCAB")))
                Me.ListBoxDebug.Items.Add(" ==> Vendor               : " & PackingSlip.SAT_NHVendPackingSlipJour(0).Vendor.ToString)
                Me.ListBoxDebug.Items.Add(" ==> Documento            : " & PackingSlip.SAT_NHVendPackingSlipJour(0).PackingSlipIdNH.ToString & "    Key = " & PackingSlip.SAT_NHVendPackingSlipJour(0).PackingSlipId.ToString)
                Me.ListBoxDebug.Items.Add(" ==> Total                : " & PackingSlip.SAT_NHVendPackingSlipJour(0).Total.ToString)
                Me.ListBoxDebug.Items.Add(" ==> Total Sin Imp        : " & PackingSlip.SAT_NHVendPackingSlipJour(0).TotalNoVat.ToString)

                Me.ListBoxDebug.Items.Add(" ==> Dto Cab (%)          : " & PackingSlip.SAT_NHVendPackingSlipJour(0).DtoPercent.ToString & " %")
                Me.ListBoxDebug.Items.Add(" ==> Dto Cab Euros        : " & PackingSlip.SAT_NHVendPackingSlipJour(0).DtoValue.ToString)

                Me.ListBoxDebug.Items.Add("")

                Me.ListBoxDebug.Items.Add(" ==> Esta Anulado         : " & PackingSlip.SAT_NHVendPackingSlipJour(0).EsAnulacion.ToString)
                Me.ListBoxDebug.Items.Add(" ==> Es Tipo Isback       : " & PackingSlip.SAT_NHVendPackingSlipJour(0).IsBack.ToString)


                Me.ListBoxDebug.Items.Add("")



                'If PackingSlip.SAT_NHVendPackingSlipJour(0).PackingSlipIdNH.ToString = "11000115" Then
                ' MsgBox(Now)
                ' End If

                ' Lineas del Albaran


                SQL = "SELECT ASNT_AX_NIF AS NIF,ASNT_DOCU AS DOCUMENTO,ASNT_AX_PRODUCT AS ASNT_AX_PRODUCT, "
                SQL += "ASNT_AMPCPTO AS ""PRODUCTO AXAPTA""  ,NVL(ASNT_AUXILIAR_STRING,'?') AS ""PRODUCTO NEWHOTEL"" "
                SQL += ",ASNT_AX_DTO_CABD AS ASNT_AX_DTO_CABD  "
                SQL += ",ASNT_AX_DTO_CABV AS ASNT_AX_DTO_CABV  "

                SQL += ",ASNT_AX_TOTAL AS ASNT_AX_TOTAL  "
                SQL += ",ASNT_AX_TOTAL_BASE AS ASNT_AX_TOTAL_BASE  "

                SQL += ",ASNT_AX_TOTAL_LINEAS AS ASNT_AX_TOTAL_LINEAS   "
                SQL += ",ASNT_AX_MOVG_CODI || '/' || ASNT_AX_MOVG_ANCI AS REFE  "
                SQL += ",DECODE(ASNT_AX_TIPO,0,'Albaran',1,'Anulado','?') as TIPO   "


                SQL += ",ASNT_AX_QNTD AS ASNT_AX_QNTD   "
                SQL += ",NVL(ASNT_AX_QNTD_BONUS,0) AS ASNT_AX_QNTD_BONUS   "
                SQL += ",ASNT_AX_PRUN  AS ASNT_AX_PRUN   "
                SQL += ",ASNT_AX_PRPO  AS ASNT_AX_PRPO   "

                SQL += ",ASNT_AX_IMPU_LINEA  AS ASNT_AX_IMPU_LINEA   "
                SQL += ",ASNT_AX_DTO_LIND  AS ASNT_AX_DTO_LIND   "
                SQL += ",ASNT_AX_DTO_LINV  AS ASNT_AX_DTO_LINV   "

                SQL += ",ASNT_AX_QNTD_PED  AS ASNT_AX_QNTD_PED   "
                SQL += ",ASNT_AX_NUM_PED   AS ASNT_AX_NUM_PED   "

                SQL += ",ASNT_F_VALOR   AS ASNT_F_VALOR   "

                SQL += ",ASNT_AX_MOVG_CODI , ASNT_AX_MOVG_ANCI  "
                SQL += ",ASNT_AX_MOVD_CODI    "
                SQL += ",ASNT_AJUSTE_DTO    "
                SQL += ",ASNT_TIPO_IGIC    "





                SQL += " FROM TS_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"


                SQL += " AND TS_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND TS_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND TS_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum

                ' key documento

                SQL += " AND ASNT_AX_MOVG_ANCI = " & CInt(Me.DbLee.mDbLector.Item("ASNT_AX_MOVG_ANCI"))
                SQL += " AND ASNT_AX_MOVG_CODI = " & CInt(Me.DbLee.mDbLector.Item("ASNT_AX_MOVG_CODI"))

                ' SOLO SIN PROCESAR
                ' SQL += " AND ASNT_AX_STATUS =  0 "



                SQL += " ORDER BY ASNT_AX_MOVG_ANCI,ASNT_AX_MOVG_CODI,ASNT_LINEA "



                Me.m_TotalBase = 0
                Me.m_TotalImpuesto = 0
                Me.m_TotalFactura = 0

                Me.DbAux.TraerLector(SQL)


                Primerregistro = True

                While Me.DbAux.mDbLector.Read



                    If Primerregistro = True Then
                        Primerregistro = False
                        Ind = 0
                        ReDim PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind)
                    Else                ' añade un elemento a array de lineas / Objeto
                        ReDim Preserve PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(UBound(PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans) + 1)
                        Ind = UBound(PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans)
                    End If


                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind) = New WebReferenceCompras.AxdEntity_SAT_NHVendPackingSlipTrans()


                    ' DETALLE

                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).AmountVAT = CDec(Me.DbAux.mDbLector.Item("ASNT_AX_IMPU_LINEA"))
                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).AmountVATSpecified = True
                    '  PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).class = ""
                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).DtoPercent = CDec(Me.DbAux.mDbLector.Item("ASNT_AX_DTO_LIND"))
                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).DtoPercentSpecified = True
                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).DtoValue = CDec(Me.DbAux.mDbLector.Item("ASNT_AX_DTO_LINV"))
                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).DtoValueSpecified = True
                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).PackingSlipIdNH = CStr(Me.DbLee.mDbLector.Item("DOCUMENTO"))

                    ' Calculo del precio por unidad caso de haber bonus , segun Gregorio (ravisar esto !!!)
                    If CDec(Me.DbAux.mDbLector.Item("ASNT_AX_QNTD_BONUS")) > 0 Then
                        PrecioUnidad = (CDec(Me.DbAux.mDbLector.Item("ASNT_AX_QNTD")) * CDec(Me.DbAux.mDbLector.Item("ASNT_AX_PRUN"))) / (CDec(Me.DbAux.mDbLector.Item("ASNT_AX_QNTD")) + CDec(Me.DbAux.mDbLector.Item("ASNT_AX_QNTD_BONUS")))
                    Else
                        PrecioUnidad = CDec(Me.DbAux.mDbLector.Item("ASNT_AX_PRUN"))
                    End If


                    '20121105 se envian los precios sin redondear
                    'PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).PriceUnit = Math.Round(PrecioUnidad, 2, MidpointRounding.AwayFromZero)
                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).PriceUnit = PrecioUnidad
                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).PriceUnitSpecified = True



                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).PurchIdNH = "NUMERO DE PEDIDO"



                    ' CANTIDAD 

                    If CStr(Me.DbLee.mDbLector.Item("TIPO")) = "A" Then
                        If CInt(Me.DbLee.mDbLector.Item("ANULADO")) = 0 Then
                            PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).Qty = CDec(Me.DbAux.mDbLector.Item("ASNT_AX_QNTD"))
                        Else
                            PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).Qty = CDec(Me.DbAux.mDbLector.Item("ASNT_AX_QNTD"))
                        End If
                    End If

                    If CStr(Me.DbLee.mDbLector.Item("TIPO")) = "D" Then
                        If CInt(Me.DbLee.mDbLector.Item("ANULADO")) = 0 Then
                            PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).Qty = CDec(Me.DbAux.mDbLector.Item("ASNT_AX_QNTD") * -1)
                        Else
                            PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).Qty = CDec(Me.DbAux.mDbLector.Item("ASNT_AX_QNTD") * -1)

                        End If
                    End If


                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).QtySpecified = True
                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).QtyOrdered = CDec(Me.DbAux.mDbLector.Item("ASNT_AX_QNTD"))
                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).QtyOrderedSpecified = True

                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).RecId = 0
                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).RecIdSpecified = True
                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).SAT_NHItemId = CStr(Me.DbAux.mDbLector.Item("ASNT_AX_PRODUCT"))




                    '***************************************************************************************************

                    '20121105 calculo para nuevo campo lineamount = importe de la linea = (precio de compra - descuento )* cantidad 





                    ' Si hay descuento por CABECERA  en valor
                    If Me.DbAux.mDbLector.Item("ASNT_AX_DTO_CABV") > 0 Then
                        ValorDescuentoCabecera = CDec(Me.DbAux.mDbLector.Item("ASNT_AX_DTO_CABV"))
                        TotalLinaSinImpuesos = (PrecioUnidad * PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).Qty) - ValorDescuentoCabecera

                        'PROBLEMA 1 
                        If CheckBoxDtoValor.Checked Then
                            ValorDescuentoCabecera = CDec(Me.DbAux.mDbLector.Item("ASNT_AX_DTO_CABV"))
                            TotalLinaSinImpuesos = (CDec(Me.DbAux.mDbLector.Item("ASNT_AX_PRPO")) * PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).Qty)
                        End If
                        '


                        ' Si hay descuento por CABECERA en %
                    ElseIf Me.DbAux.mDbLector.Item("ASNT_AX_DTO_CABD") > 0 Then
                        ValorDescuentoCabecera = (PrecioUnidad * CDec(Me.DbAux.mDbLector.Item("ASNT_AX_DTO_CABD"))) / 100
                        TotalLinaSinImpuesos = (PrecioUnidad - ValorDescuentoCabecera) * PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).Qty
                    Else
                        ' Si NO hay descuento por CABECERA
                        ValorDescuentoCabecera = 0
                        TotalLinaSinImpuesos = PrecioUnidad * PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).Qty
                    End If


                    PrecioUnidad = PrecioUnidad - ValorDescuentoCabecera

                    'PROBLEMA 1 
                    If CheckBoxDtoValor.Checked Then
                        If Me.DbAux.mDbLector.Item("ASNT_AX_DTO_CABV") > 0 Then
                            PrecioUnidad = CDec(Me.DbAux.mDbLector.Item("ASNT_AX_PRPO"))
                        End If
                    End If
                    '



                    ' Si hay descuento por linea en valor
                    If Me.DbAux.mDbLector.Item("ASNT_AX_DTO_LINV") > 0 Then
                        TotalLinaSinImpuesos = (PrecioUnidad * PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).Qty) - CDec(Me.DbAux.mDbLector.Item("ASNT_AX_DTO_LINV"))

                        ' Si hay descuento por linea en %
                    ElseIf Me.DbAux.mDbLector.Item("ASNT_AX_DTO_LIND") > 0 Then
                        ValorDescuentoLinea = (PrecioUnidad * CDec(Me.DbAux.mDbLector.Item("ASNT_AX_DTO_LIND"))) / 100
                        TotalLinaSinImpuesos = (PrecioUnidad - ValorDescuentoLinea) * PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).Qty
                    Else
                        ' Si NO hay descuento por linea 
                        TotalLinaSinImpuesos = PrecioUnidad * PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).Qty
                    End If

                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).LineAmount = Math.Round(TotalLinaSinImpuesos, 2, MidpointRounding.AwayFromZero)
                    PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).LineAmountSpecified = True


                    ' calculo de total base y total factura a partir de lineamount

                    'Me.m_TotalBase = Me.m_TotalBase + TotalLinaSinImpuesos
                    ' Me.m_TotalImpuesto = Me.m_TotalImpuesto + (TotalLinaSinImpuesos * CDec(Me.DbAux.mDbLector.Item("ASNT_TIPO_IGIC"))) / 100


                    ' calculo del total base sumando las bases de cada lineA REDONDEADA
                    Me.m_TotalBase = Me.m_TotalBase + Math.Round(TotalLinaSinImpuesos, 2, MidpointRounding.AwayFromZero)
                    ' Calculo del impuesto sobre la base de la linea ( REDONDEADA)
                    Me.m_TotalImpuesto = Me.m_TotalImpuesto + (Math.Round(TotalLinaSinImpuesos, 2, MidpointRounding.AwayFromZero) * CDec(Me.DbAux.mDbLector.Item("ASNT_TIPO_IGIC"))) / 100


                    '***************************************************************************************************


                    ' Datos Gregorio

                    SQL = "SELECT NVL(TNST_MOVD.ALMA_CODI,0) AS ALMA_CODI, "
                    SQL += "       NVL(TNST_MOVD.CAPR_CODI,0) AS CAPR_CODI, "
                    SQL += "       NVL(TNST_MOVD.IVAS_CODI,0) AS IVAS_CODI, "
                    SQL += "       NVL(TNST_MOVD.MOVD_CODI,0) AS MOVD_CODI, "
                    SQL += "       NVL(TNST_MOVD.MOVD_CONT,0) AS MOVD_CONT, "
                    SQL += "       NVL(TNST_MOVD.MOVD_IVAS,0) AS MOVD_IVAS, "
                    SQL += "       NVL(TNST_MOVG.MOVG_ANCI,0) AS MOVG_ANCI, "
                    SQL += "       NVL(TNST_MOVG.MOVG_CODI,0) AS MOVG_CODI, "
                    SQL += "       NVL(TNST_MOVD.RETE_CODI,0) AS RETE_CODI, "
                    SQL += "       NVL(TNST_MOVD.RETE_TAXA,0) AS RETE_TAXA, "
                    SQL += "       NVL(TNST_MOVD.UNME_CODI,0) AS UNME_CODI, "

                    SQL += "       NVL(TNST_IVAS.IVAS_TAXA,0) AS IVAS_TAXA "

                    SQL += "  FROM TNST_MOVG, TNST_MOVD,TNST_IVAS "
                    SQL += " WHERE (TNST_MOVG.MOVG_CODI = TNST_MOVD.MOVG_CODI) "
                    SQL += "       AND (TNST_MOVG.MOVG_ANCI = TNST_MOVD.MOVG_ANCI) "

                    SQL += "         AND (TNST_MOVD.IVAS_CODI = TNST_IVAS.IVAS_CODI(+)) "


                    SQL += " AND TNST_MOVD.MOVG_ANCI = " & CInt(Me.DbAux.mDbLector.Item("ASNT_AX_MOVG_ANCI"))
                    SQL += " AND TNST_MOVD.MOVG_CODI = " & CInt(Me.DbAux.mDbLector.Item("ASNT_AX_MOVG_CODI"))
                    SQL += " AND TNST_MOVD.MOVD_CODI = " & CInt(Me.DbAux.mDbLector.Item("ASNT_AX_MOVD_CODI"))



                    Me.DbHotel.TraerLector(SQL)

                    Me.DbHotel.mDbLector.Read()

                    If Me.DbHotel.mDbLector.HasRows Then


                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).ALMA_CODI = CDec(Me.DbHotel.mDbLector.Item("ALMA_CODI"))
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).ALMA_CODISpecified = True

                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).CAPR_CODI = CDec(Me.DbHotel.mDbLector.Item("CAPR_CODI"))
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).CAPR_CODISpecified = True

                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).IVAS_CODI = CDec(Me.DbHotel.mDbLector.Item("IVAS_CODI"))
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).IVAS_CODISpecified = True

                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVD_CODI = CDec(Me.DbHotel.mDbLector.Item("MOVD_CODI"))
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVD_CODISpecified = True

                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVD_CONT = CDec(Me.DbHotel.mDbLector.Item("MOVD_CONT"))
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVD_CONTSpecified = True


                        ' Reisar el uso de este campo parece fuera de uso siempre 0 
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVD_IVAS = CDec(Me.DbHotel.mDbLector.Item("MOVD_IVAS"))
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVD_IVASSpecified = True


                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVG_ANCI = CDec(Me.DbHotel.mDbLector.Item("MOVG_ANCI"))
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVG_ANCISpecified = True


                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVG_CODI = CDec(Me.DbHotel.mDbLector.Item("MOVG_CODI"))
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVG_CODISpecified = True


                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).RETE_CODI = CDec(Me.DbHotel.mDbLector.Item("RETE_CODI"))
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).RETE_CODISpecified = True


                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).RETE_TAXA = CDec(Me.DbHotel.mDbLector.Item("RETE_TAXA"))
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).RETE_TAXASpecified = True

                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).UNME_CODI = CDec(Me.DbHotel.mDbLector.Item("UNME_CODI"))
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).UNME_CODISpecified = True


                        ' TAX VALUE Y HOLD 

                        '20121105
                        'PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).TaxValue = CDec(Me.DbHotel.mDbLector.Item("MOVD_IVAS"))
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).TaxValue = CDec(Me.DbHotel.mDbLector.Item("IVAS_TAXA"))
                        '
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).TaxValueSpecified = True

                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).HoldBackValue = 0
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).HoldBackValueSpecified = True

                        Me.DbHotel.mDbLector.Close()

                    Else

                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).ALMA_CODI = 0
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).ALMA_CODISpecified = True

                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).CAPR_CODI = 0
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).CAPR_CODISpecified = True

                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).IVAS_CODI = 0
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).IVAS_CODISpecified = True

                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVD_CODI = 0
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVD_CODISpecified = True

                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVD_CONT = 0
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVD_CONTSpecified = True

                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVD_IVAS = 0
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVD_IVASSpecified = True


                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVG_ANCI = 0
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVG_ANCISpecified = True


                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVG_CODI = 0
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).MOVG_CODISpecified = True


                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).RETE_CODI = 0
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).RETE_CODISpecified = True


                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).RETE_TAXA = 0
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).RETE_TAXASpecified = True

                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).UNME_CODI = 0
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).UNME_CODISpecified = True


                        ' TAX VALUE Y HOLD 


                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).TaxValue = 0
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).TaxValueSpecified = True

                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).HoldBackValue = 0
                        PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).HoldBackValueSpecified = True



                        Me.DbHotel.mDbLector.Close()

                    End If


                    Linea = " Producto = "
                    Linea += Mid(CStr(Me.DbAux.mDbLector.Item("PRODUCTO NEWHOTEL")), 1, 30).PadRight(30, " ")

                    Linea += " Artículo Ax  = "
                    Linea += PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).SAT_NHItemId.ToString.PadRight(10, " ")

                    Linea += " Cant = "
                    Linea += PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).Qty.ToString.PadRight(10, " ")

                    Linea += " Prec Unidad  = "
                    Linea += PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).PriceUnit.ToString.PadRight(10, " ")

                    Linea += " Dto %  = "
                    Linea += PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).DtoPercent.ToString.PadRight(10, " ") & " %    "

                    Linea += " Dto Eur   = "
                    Linea += PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans(Ind).DtoValue.ToString.PadRight(10, " ") & " Euros"


                    Linea += " Total Linea (LineAmount) = "
                    Linea += TotalLinaSinImpuesos.ToString.PadRight(10, " ") & " Euros"




                    Me.ListBoxDebug.Items.Add(Linea)

                    Me.ListBoxDebug.Update()
                    Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1







                End While
                Me.DbAux.mDbLector.Close()

                Me.ListBoxDebug.Items.Add("")
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Llamada al Web Service Albaranes " & Me.WebAlbaranes.Url)
                Me.ListBoxDebug.Update()


                Elementos = PackingSlip.SAT_NHVendPackingSlipJour(0).SAT_NHVendPackingSlipTrans.GetLength(Dimension)





                ' path 40
                ' Los descuento de linea se aplican emn newstok usando un redondeo NO valido 
                ' ejemplo si el descuento a aplicar una linea es de 1,725  newstock aplica 1,72 y debe ser 1,73
                ' esta rituna incorporta la diferencia de calculos






                SQL = " SELECT SUM(ASNT_AJUSTE_DTO) "
                SQL += " FROM TS_ASNT WHERE ASNT_F_VALOR = '" & Me.mFecha & "'"
                SQL += " AND TS_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND TS_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND TS_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
                SQL += " AND ASNT_AX_MOVG_ANCI = " & CInt(Me.DbLee.mDbLector.Item("ASNT_AX_MOVG_ANCI"))
                SQL += " AND ASNT_AX_MOVG_CODI = " & CInt(Me.DbLee.mDbLector.Item("ASNT_AX_MOVG_CODI"))


                Dim TotalAjuste As Double
                TotalAjuste = Me.DbWrite.EjecutaSqlScalar(SQL)





                'If CheckBoxAjustar2.Checked Then
                If CheckBoxAjustar2.Checked And TotalAjuste <> 0 Then
                    Me.m_TotalFactura = Math.Round(m_TotalBase, 2, MidpointRounding.AwayFromZero) + Math.Round(Me.m_TotalImpuesto, 2, MidpointRounding.AwayFromZero)

                    PackingSlip.SAT_NHVendPackingSlipJour(0).TotalNoVat = Math.Round(Me.m_TotalBase, 2, MidpointRounding.AwayFromZero)
                    PackingSlip.SAT_NHVendPackingSlipJour(0).Total = Me.m_TotalFactura

                    Me.ListBoxDebug.Items.Add("")
                    Me.ListBoxDebug.Items.Add("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!")
                    Me.ListBoxDebug.Items.Add("Ajuste al Documento Anterior")
                    Me.ListBoxDebug.Items.Add(" ==> Total(*)                : " & PackingSlip.SAT_NHVendPackingSlipJour(0).Total.ToString)
                    Me.ListBoxDebug.Items.Add(" ==> Total Sin Imp(*)        : " & PackingSlip.SAT_NHVendPackingSlipJour(0).TotalNoVat.ToString)
                    Me.ListBoxDebug.Items.Add("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!")

                    Me.m_Recalculos = Me.m_Recalculos + 1
                    Me.TextBoxDebug2.Text = "Recálculos = " & Me.m_Recalculos
                    Me.TextBoxDebug2.Update()


                End If


                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.SelectedIndex = Me.ListBoxDebug.Items.Count - 1



                ' LLAMADA AL WEB SERVICE
                ' IF CONTROL NO HAY ERRORES 
                If Elementos <> 0 Then


                    If Me.EnviaAlbaran(Me.DocumentoContextoAlbaranes, PackingSlip, Elementos) = True Then
                        Me.ListBoxDebug.Items.Add(" <== OK: ")
                        Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1
                        Me.ListBoxDebug.Update()
                        Me.AuxErr = ""
                        Me.TrataenvioAlbaran(CInt(Me.DbLee.mDbLector.Item("ASNT_AX_MOVG_CODI")), CInt(Me.DbLee.mDbLector.Item("ASNT_AX_MOVG_ANCI")), 1, "OK")
                    Else
                        Me.TotalErrores = Me.TotalErrores + 1
                        Me.AuxErr = ""
                        Me.TrataenvioAlbaran(CInt(Me.DbLee.mDbLector.Item("ASNT_AX_MOVG_CODI")), CInt(Me.DbLee.mDbLector.Item("ASNT_AX_MOVG_ANCI")), 0, Me.AxError)

                        If Me.m_ErrorCorto = "" Then
                            Me.ListBoxDebug.Items.Add(" <== Largo : " & Me.AxError)
                        Else
                            Me.ListBoxDebug.Items.Add(" <== Corto2 : " & Me.m_ErrorCorto)
                        End If



                        Me.ListBoxDebug.SelectedIndex = Me.ListBoxDebug.Items.Count - 1
                        Me.ListBoxDebug.Update()
                        '  Beep()

                    End If

                End If


                Application.DoEvents()




            End While
            Me.DbLee.mDbLector.Close()




        Catch ex As Exception
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service Albaranes " & Me.WebAlbaranes.Url & " : " & ex.Message)

            MsgBox(ex.Message)
            '     MsgBox(ex.ToString)
        End Try
        '   Catch ex As Web.Services.Protocols.SoapException
        '       MsgBox(ex.Message)'

        'End Try

    End Sub

    Private Function EnviaAlbaran(ByVal vDoc As WebReferenceCompras.DocumentContext, ByVal vQuery As WebReferenceCompras.AxdSAT_NHVendPackingSlipJour, ByVal velementos As Integer) As Boolean


        Try

            Me.AxError = ""
            vDoc.MessageId = Guid.NewGuid.ToString


            'X = Me.WebAlbaranes.createListSAT_NHVendPackingSlipJour(vDoc, vQuery)
            Me.WebAlbaranes.createListSAT_NHVendPackingSlipJour(vDoc, vQuery)
            'Me.ListBoxDebug.Items.Add(X)
            Return True

        Catch ex As Exception

            'If IsNothing(X) = False Then
            'Me.ListBoxDebug.Items.Add(X)
            'End If


            ' Catch ex As System.Web.Services.Protocols.SoapException

            Me.AxError = ex.Message
            ' Control de si el registro se proceso bien m pero ,, ax no pudo contestar  por caida de la linea 
            If ex.Message.IndexOf("REPETIDO") > 0 Then
                Me.ListBoxDebug.Items.Add(" <== OK Documento ya estaba enviado se marca como OK : ")
                Me.ListBoxDebug.Items.Add(" <== : " & Me.AxError)
                Return True
            Else
                ' errores con almoadilla
                Dim separador As Char = "#"

                Dim listErrors As String() = ex.Message.Split(separador)
                If UBound(listErrors) > 0 Then

                    For idx As Integer = 1 To listErrors.Length - 1
                        If listErrors(idx).ToString.Length > 1 Then
                            Me.AxError = Me.AxError & listErrors(idx).ToString & vbCrLf
                        End If
                    Next

                    'For idx As Integer = Me.AxError.Length - 1 To 1 Step -1
                    'If Mid(Me.AxError, idx, 1) <> "#" Then
                    ' Me.T2 = Me.T2 & Mid(Me.AxError, idx, 1)
                    'End If
                    'If Mid(Me.AxError, idx, 1) = "#" Then
                    'Exit For
                    'End If
                    'Next

                    'Me.AxErrorLimpio = Me.AxError.Replace(T1, "")
                    'Me.AxErrorLimpio = Me.AxError.Replace(T2, "")
                    'Me.ListBoxDebug.Items.Add(" <== : " & Me.AxErrorLimpio)

                Else
                    ' errores sin almoadilla
                    ' si llega aqui ek error esta cargado Me.AxError
                End If


                Return False
            End If





        End Try
    End Function

    Private Sub TrataenvioAlbaran(ByVal vMovi_codi As Integer, ByVal vMovi_anci As Integer, ByVal vStatus As Integer, ByVal vMessage As String)


        Try
            Me.m_ErrorCorto = ""
            Me.m_ErrorCorto = Me.TrataError(vMessage)
        Catch ex As Exception
            Me.m_ErrorCorto = ""
        End Try



        Try

            SQL = "UPDATE TS_ASNT SET ASNT_AX_STATUS = " & vStatus
            SQL += " ,ASNT_AX_ERR_MESSAGE = '" & Mid(vMessage, 1, 4000).Replace("'", "''").Trim & "'"
            SQL += " ,ASNT_AX_ERR_MESSAGE2 = '" & Mid(Me.m_ErrorCorto, 1, 4000).Replace("'", "''").Trim & "'"

            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TS_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_AX_MOVG_CODI = " & vMovi_codi
            SQL += " AND ASNT_AX_MOVG_ANCI = " & vMovi_anci

            Me.DbWrite.EjecutaSqlCommit(SQL)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Function TrataError(ByVal vAxMensaje As String) As String
        Try

            Me.m_ResponseStr = ""
            Dim TextoCorto() As String


            TextoCorto = Split(vAxMensaje, "##")


            If TextoCorto.Length > -1 Then

                Dim i As Integer
                For i = TextoCorto.GetLowerBound(0) To TextoCorto.GetUpperBound(0)
                    If i <> TextoCorto.GetLowerBound(0) And i <> TextoCorto.GetUpperBound(0) Then
                        Me.m_ResponseStr += TextoCorto(i) & vbCrLf
                    End If
                Next i
            End If

            Return Me.m_ResponseStr

        Catch ex As Exception
            Return Me.m_ResponseStr
            MsgBox(ex.Message)
        End Try

    End Function

    Private Sub MarcaDiaComoNoEnviado()
        Try


            SQL = "UPDATE TS_ASNT SET ASNT_AX_STATUS = 0"
            SQL += " ,ASNT_AX_ERR_MESSAGE = ''"
            SQL += " ,ASNT_AX_ERR_MESSAGE2 = ''"

            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TS_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"


            Me.DbWrite.EjecutaSqlCommit(SQL)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
#End Region


    Private Sub FormConvertirAxaptaCompras_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If IsNothing(Me.DbLee) = False Then
                If DbLee.EstadoConexion = ConnectionState.Open Then
                    DbLee.CerrarConexion()
                End If
            End If

            If IsNothing(Me.DbAux) = False Then
                If DbAux.EstadoConexion = ConnectionState.Open Then
                    DbAux.CerrarConexion()
                End If
            End If

            If IsNothing(Me.DbWrite) = False Then
                If DbWrite.EstadoConexion = ConnectionState.Open Then
                    DbWrite.CerrarConexion()
                End If
            End If

            If IsNothing(Me.DbHotel) = False Then
                If DbHotel.EstadoConexion = ConnectionState.Open Then
                    DbHotel.CerrarConexion()
                End If
            End If

            ESTACARGADOFORMCONVERTIR = False
        Catch ex As Exception

        End Try
    End Sub

    Private Sub FormConvertirAxaptaCompras_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.DbLee = New C_DATOS.C_DatosOledb(Me.mStrConexion)
            Me.DbLee.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbAux = New C_DATOS.C_DatosOledb(Me.mStrConexion)
            Me.DbAux.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbWrite = New C_DATOS.C_DatosOledb(Me.mStrConexion)
            Me.DbWrite.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbHotel = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
            Me.DbHotel.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


            Me.DataGridViewAsientos.ReadOnly = True
            Me.DataGridViewAsientos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            'Me.DataGridViewAsientos.Font = New Font("Footlight MT Light ", 6.75, FontStyle.Regular)
            Me.DataGridViewAsientos.Font = New Font("Arial", 6.75, FontStyle.Regular)
            Me.LeeParametros()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

 



    Private Sub FormConvertirAxaptaCompras_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            Me.MostrarAsiento(Me.m_Filtro)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try

            Me.Cursor = Cursors.AppStarting
            '  Me.LeeParametros()
            'If Me.CheckBoxDebug.Checked Then
            'Me.UpdateAxaptaVAlues()
            ' End If


            ' EVITA PULSAR EL BOTON DOS VECES
            Me.ButtonAceptar.Visible = False
            Me.ButtonAceptar.Update()


            Me.ListBoxDebug.Items.Clear()
            Me.ListBoxDebug.Update()

            Me.m_Recalculos = 0
            Me.AxProcesarAlbaranes()
            Me.MostrarAsiento(Me.m_Filtro)

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
            Me.ButtonAceptar.Visible = True
            Me.ButtonAceptar.Update()
        End Try
    End Sub

#Region "TEST"

    Private Sub UpdateAxaptaVAlues()
        Try
            SQL = "UPDATE TS_ASNT SET ASNT_AX_NIF = 'A35006576'"
            DbLee.EjecutaSqlCommit(SQL)

            SQL = "UPDATE TS_ASNT SET ASNT_AX_PRODUCT = 'GEN-ART000209'"
            SQL += " WHERE SUBSTR(ASNT_AX_PRODUCT,4,2)  = '00' "
            DbLee.EjecutaSqlCommit(SQL)

            SQL = "UPDATE TS_ASNT SET ASNT_AX_PRODUCT = 'GEN-ART000210'"
            SQL += " WHERE SUBSTR(ASNT_AX_PRODUCT,4,2)  = '02' "
            DbLee.EjecutaSqlCommit(SQL)

            SQL = "UPDATE TS_ASNT SET ASNT_AX_PRODUCT = 'GEN-ART000211'"
            SQL += " WHERE SUBSTR(ASNT_AX_PRODUCT,4,2)  = '05' "
            DbLee.EjecutaSqlCommit(SQL)

            SQL = "UPDATE TS_ASNT SET ASNT_AX_PRODUCT = 'GEN-ART000212'"
            SQL += " WHERE SUBSTR(ASNT_AX_PRODUCT,4,2)  = '13' "
            DbLee.EjecutaSqlCommit(SQL)

            SQL = "UPDATE TS_ASNT SET ASNT_AX_PRODUCT = 'GEN-ART000213'"
            SQL += " WHERE SUBSTR(ASNT_AX_PRODUCT,4,2)  = '35' "
            DbLee.EjecutaSqlCommit(SQL)




        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

#End Region


    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            REPORT_SELECTION_FORMULA = "{TS_ASNT.ASNT_F_VALOR}=DATETIME(" & Format(Me.mFecha, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += " AND {TS_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TS_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"



            Dim Form As New FormVisorCrystal("ASIENTO_ALMACEN Axapta.RPT", "", REPORT_SELECTION_FORMULA, Me.mStrConexion, "", False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub ButtonImprimirErrores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimirErrores.Click
        Try

            Dim F As New FormPideFechas(Me.mFecha)
            CONTROLF = False
            F.ShowDialog()

            If CONTROLF = False Then
                ' salio con cancelar del form pide fechas 
                Exit Sub
            End If

            Me.Cursor = Cursors.WaitCursor

            ' REPORT_SELECTION_FORMULA = "{TS_ASNT.ASNT_F_VALOR}=DATETIME(" & Format(Me.mFecha, REPORT_DATE_FORMAT) & ")"

            REPORT_SELECTION_FORMULA = "{TS_ASNT.ASNT_F_VALOR}>=DATETIME(" & Format(FEC1, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += "AND {TS_ASNT.ASNT_F_VALOR}<=DATETIME(" & Format(FEC2, REPORT_DATE_FORMAT) & ")"

            REPORT_SELECTION_FORMULA += " AND {TS_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TS_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"
            REPORT_SELECTION_FORMULA += " AND isnull({TS_ASNT.ASNT_AX_ERR_MESSAGE}) = false AND  {TS_ASNT.ASNT_AX_ERR_MESSAGE} <> 'OK'"


            Dim Form As New FormVisorCrystal("ASIENTO_ALMACEN Axapta Errores.RPT", "Errores " & Format(FEC1, "dd/MM/yyyy") & "   " & Format(FEC2, "dd/MM/yyyy"), REPORT_SELECTION_FORMULA, Me.mStrConexion, "", False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub CheckBoxSoloPendintes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxSoloPendintes.CheckedChanged
        Try
            Me.MostrarAsiento(Me.m_Filtro)
        Catch ex As Exception

        End Try
    End Sub



    Private Sub ButtonValidar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonValidar.Click
        Try
            Me.Validar()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonResetEnvio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonResetEnvio.Click
        Try
            If MessageBox.Show("Marcar " & Me.mFecha & " como NO enviado ?", "Atención", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
                Me.MarcaDiaComoNoEnviado()
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

   
    Private Sub DataGridViewAsientos_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridViewAsientos.CellContentClick

    End Sub

    Private Sub DataGridViewAsientos_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles DataGridViewAsientos.CellFormatting
        Try
            ' COLOR DE FONDO DE LA COLUMNA

            If Me.DataGridViewAsientos.Columns(e.ColumnIndex).Name = "ESTADO" Then
                If e.Value IsNot Nothing Then
                    If e.Value.ToString = "Ok Enviado" Then
                        e.CellStyle.BackColor = Color.Green
                        e.CellStyle.ForeColor = Color.White
                    ElseIf e.Value.ToString = "No Enviado" Then
                        e.CellStyle.BackColor = Color.White
                        e.CellStyle.ForeColor = Color.Black
                    ElseIf e.Value.ToString = "Excluido" Then
                        e.CellStyle.BackColor = Color.Salmon
                        e.CellStyle.ForeColor = Color.Black
                    End If
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBoxDocumento_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxDocumento.TextChanged
        Try
            Me.m_Filtro = Me.TextBoxDocumento.Text
            Me.MostrarAsiento(Me.m_Filtro)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CheckBoxAjustar2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxAjustar2.CheckedChanged

    End Sub

    Private Sub ExcluirDocumentoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExcluirDocumentoToolStripMenuItem.Click
        Try
            Dim Seleccion As Integer
            Dim PrimeraVisible As Integer

            Dim Key As String()

            Seleccion = Me.DataGridViewAsientos.CurrentRow.Index
            PrimeraVisible = DataGridViewAsientos.FirstDisplayedScrollingRowIndex


            If Me.m_HayRegistros = True Then
                Key = Me.DataGridViewAsientos.Item("REFE", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString.Split("/")
            Else
                Exit Sub
            End If

            ' SI NO HA SIDO ENVIADO 
            If Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "No Enviado" Then
                SQL = "UPDATE TS_ASNT SET ASNT_AX_STATUS = 9 WHERE "
                SQL += " ASNT_AX_MOVG_CODI = " & Key(0)
                SQL += " AND ASNT_AX_MOVG_ANCI = " & Key(1)



                Me.DbWrite.EjecutaSqlCommit(SQL)
                ' Reenviar
            ElseIf Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "Excluido" Then
                SQL = "UPDATE TS_ASNT SET ASNT_AX_STATUS = 0 WHERE "
                SQL += " ASNT_AX_MOVG_CODI = " & Key(0)
                SQL += " AND ASNT_AX_MOVG_ANCI = " & Key(1)

                Me.DbWrite.EjecutaSqlCommit(SQL)
                SQL = "UPDATE TS_ASNT SET ASNT_AUX_UPDATE = 'Estaba Excluido 9'  WHERE "
                SQL += " ASNT_AX_MOVG_CODI = " & Key(0)
                SQL += " AND ASNT_AX_MOVG_ANCI = " & Key(1)
                Me.DbWrite.EjecutaSqlCommit(SQL)

            End If



            Me.MostrarAsiento(Me.m_Filtro)
            DataGridViewAsientos.FirstDisplayedScrollingRowIndex = PrimeraVisible
            DataGridViewAsientos.Refresh()




        Catch ex As Exception

        End Try
    End Sub


End Class