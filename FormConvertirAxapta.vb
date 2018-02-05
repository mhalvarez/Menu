Imports System.Web.Services.Protocols

Public Class FormConvertirAxapta

    ' pendiente 
    'los bonos facturados van :
    '
    '                web service de facturas ( sin nunmero de factura ) y bond = yes  por el importe de deduccion del asiento 1666.67
    '
    '                quitar del web service de produccion -1666.67    ,  758.10   y 908.57
    '
    '                ejemplo tomando del dia 12-01-2009
    '
    '
    '
    '
    '----------------------------------------------------------------------------------------------------
    ' Diciembre 2009
    ' Tratamiento de Devoluciones  Automaticas
    '     
    '   De Contado
    '             Se envia el Importe de la devolucion como un Anticipo recibido negativo
    '             Se envia la Cancelacion del Anticipo Anterior como Facturado
    '             Se Excluye el Cobro Negativo de la Devolucion del Web Service de Cobros 


    '   De Credito
    '             Se envia el Importe de la devolucion como un Anticipo recibido negativo
    '             Se envia el Importe de la devolucion como un Anticipo recibido positivo

    '             Se envia la Cancelacion del Anticipo negativo Anterior como Facturado
    '             Se envia la Cancelacion del Anticipo positivo Anterior como Facturado 
    '       YA NO Se envia la Cancelacion del Anticipo positivo Anterior como Facturado ( Linea de arriba)

    '        Mayo 2010
    '
    '       (*R1) = facturas Rectificativas que contienen Anticipos      
    '       (*R2) = Factura No Rectificativa que contiene un Anticipo que ha estado en una rectificativa anterior    
    '
    '       Se Incopora tratamiento de devoluciones manuales recepcion y facturacion 


    '   pat 020
    ' las facturas que no tienen creditos caso 9973/m09 marina se envian con el articulo ARV por cada cobro o devolucion que
    '  haya en la factura una vez positivo y otra vez negativo 
    '  pregunta domingo  = Se envian tambien los anticipos 


    ' PAT0025  email 04 de abril 2011
    ' De este tema, corrígeme si me equivoco, pero estas facturas no entrarían, por los siguientes motivos:
    '•'	Cuando tienes una factura con la reserva 882, y va a ser rectificada se debe devolver todo lo que se haya cobrado en la factura como anticipos negativos y luego cobrarlos con la nueva rectificativas. Como la forma que tenemos de localizar las facturas y sus anticipos es vía número de reservas, estos anticipos deben ser asociados a reservas diferentes: Por  ello establecimos en su día que 
    '	el anticipo original iba con el número de reserva, p.e. 882
    '	el anticipo de la rectificativa con REC seguido de número de reserva: REC882
    '	el anticipo de la rectificada con NREC seguido del número de reserva: NREC882.
    '•	De esta forma, los anticipos generados por la rectificativa deben identificares con REC882 para la rectificativa, tanto para en anticipo real que “libera” como para la devolución que se envía como anticipo.
    '•	De igual manera ocurre para la rectificada, donde todos los anticipos de ben ser NREC tanto para en anticipo real que “carga” como para la devolución que se envía como anticipo negativo.


    '   28/julio/2011
    '
    '  PAT0027 Excluir los cobros del envio caso de facturas a zero ( facturas que no tienen produccion anticipo - devolucion) 
    '  pat0028  Si se excluye o corrige un cobro excluir todos los cobros del docuemnto ) 
    ''
    '
    '
    '
    '' AGOSTO 2011
    '
    ' CORREGIR AXAPTA DLL PARA QUE EL TOTAL FATURA SE GRABE REDONDEANDO CON AWAYTOZERO
    '   ANST-I-MONEMP   ASNT_LIN_VCRE  ASNT_LIN_VLIQ  ASNT_LIN_IMP1  ASNT_DOCU_VALO

    '
    '
    '
    ' maxima del redondeo 
    ' para redondear BIEN hay que 
    '  usar este formato :
    '  Ojo es importante NO pasar a la rutina MATH.round tipos de datos Double SIEMPRE DECIMAL
    'Math.Round(Valordecimal, 2, MidpointRounding.AwayFromZero)
    ' ejemplo 
    '  TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2, MidpointRounding.AwayFromZero)

    ' pats ocubre 2011
    ' pat0029
    '  Poruqe se excluye el cobro de 20 euros de la factura 1515/s11 del salobre 
    ' pat 0030
    ' en el envio de cobros se divide en dos
    '    (1) por los cobros de facturas que solo tienen 1 cobro
    '    (2) por los cobros de facturas que      tienen > de un cobro


    ' Noviembre 2011
    ' ' PAT 35 ( OJO FALTA IMPLEMENTAR EN LA TAREA PROGRAMADA !!!)
    ' se separa el envio de cobros en :
    '    facturas con un solo cobro O liquidacion de puntos de venta , y otro envio para las facturas con mas de un cobto agrupando formas de cobro y factura
    ' Este path NO esta aun en la tarea programada COPIAR ESTAS RUTINAS A LA TAREA
    '               Me.AxProcesarCobrosParciales()
    '               Me.AxProcesarCobrosParciales2()
    '               AXTrataenvioCobrosNormalesParciales

    ' PAT 38  SE EXCLUYEN LAS DEVOLUCIONES DE LA FACTURA DE PUNTO DE VENTA 

    ' PAT 40
    ' A VECES SE HACEN AJUSTES DE IMPUESTOS SIN SER NECESARIO 
    ' ( OJO FALTA IMPLEMENTAR EN LA TAREA PROGRAMADA !!!)
    ' ejemplo factura 169/m12  SI ACTIVO    EL PAT 40 ESTA FACTURA NO PASA , 
    '                          SI DESACTIVO EL PAT LE CALCULA UN AJUSTE Y PASA
    '
    '
    '---------------------------------------------------------------------------------------
    'septiembre 2014  ( pide gregorio)
    ' añadir las reservas involucradas en una factura al webservice de facturas ( cabecera )   campo bookid , 
    ' enviar las reservas separadas por ; 
    ' trabajar en la rituna FacturasSalidaTotalFActuraNuevoAX()   de axapta.dll ( ahora solo se tratan 
    '  si la factura tiene una sola reserva.
    '   terminar la funcion "Private Function BuscaReservasdeunaFactura()" de axapta dll

    '-----------------------------------------------------------------------------------------------------
    ' Junio 2014
    'Se añade Ideentioficador de Hotel en el Fichero INI           HOTELID=xxx  
    ' para enviar a los webservices en el nuevo campo HotelId
    '
    ' Se añade tratamiento tambien en tarea programada
    'LLEVAR A LA TAREA 

   
    'LLEVAR A LA TAREA TODAS LAS LINEAS DE ESTE FORM DONDE ESTE =        Me.mHotelId 
    ' OJO QUITAR LOS TRY MENOS EL ULTIMO DE LA RUTINA CalculaAjusteRedondeoImpuestoAx(

    Dim mFecha As Date

    Dim mStrConexion As String
    Dim mStrConexionHotel As String
    Dim Db As C_DATOS.C_DatosOledb
    Dim DbAux As C_DATOS.C_DatosOledb
    Dim DbAux2 As C_DATOS.C_DatosOledb
    Dim DbWrite As C_DATOS.C_DatosOledb


    Dim SQL As String
    Dim Linea As String

    Dim Importe As Double
    Dim Indicador As String

    Private mEmpGrupoCod As String
    Private mEmpCod As String
    Private mPerfilContable As String

    'otros 

    Dim HayRegistros As Boolean = False

    Private Dimension As Integer
    Private Elementos As Integer
    Private Ind As Integer

    Private AxError As String

    ' Gestion de Envios

    Dim TransRowid() As String

    Dim TotalErrores As Integer

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
    Private mAxAnticipo As String
    Private mNWFormaCobroTransferenciaAgencia As String
    Private mParaCifContadoGenerico As String = ""

    Private mParaCifContadoVentaNormal As String = ""
    Private mParaCifContadoVentaBono As String = ""
    Private mParaCifContadoProduccionNormal As String = ""
    Private mParaCifContadoProduccionBono As String = ""

    Private mParaConectaGolf As Integer = 0


    ' impuestos y ajustes

    Private mAxAjustaBase As String


    Private mInicio As Date
    Private mDuracion As Double

    Private mHotelId As Integer

    Private NEWHOTEL As NewHotel.NewHotelData
    Private NEWGOLF As NewGolf.NewGolfData

    ' otros



    ' Web Services produccion y Objetos Relacionados 
    Private WebProduccion As WebReferenceProduccion.SAT_NHCreateProdOrdersQueryService
    Private DocumentoContexto As WebReferenceProduccion.DocumentContext
    Private OrdersTabla(0) As WebReferenceProduccion.AxdEntity_SAT_NHCreateProdOrdersTable_1
    Private OrdersLine(0) As WebReferenceProduccion.AxdEntity_SAT_NHCreateProdOrdersLine_1
    Private QweryOrders As WebReferenceProduccion.AxdSAT_NHCreateProdOrdersQuery




    ' Web Services Facturas y Objetos Relacionados 
    Private WebFacturas As WebReferenceFacturas.SAT_NHCreateSalesOrdersQueryService
    Private DocumentoContextoFacturas As WebReferenceFacturas.DocumentContext
    Private OrdersTablaFacturas(0) As WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersTable_1
    Private OrdersLineFacturas(0) As WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersLine_1
    Private OrdersLineTaxFacturas(0) As WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersTaxLine_1
    Private QweryOrdersFacturas As WebReferenceFacturas.AxdSAT_NHCreateSalesOrdersQuery



    ' Web Services Cobros y Objetos Relacionados 
    Private WebCobros As WebReferenceCobros.SAT_NHJournalCustPaymentQueryService
    Private DocumentoContextoCobros As WebReferenceCobros.DocumentContext
    Private OrdersLineCobros(0) As WebReferenceCobros.AxdEntity_SAT_NHJournalCustPayment_1
    Private QweryOrdersCobros As WebReferenceCobros.AxdSAT_NHJournalCustPaymentQuery



    ' Web Services Anticipos y Objetos Relacionados 
    Private WebAnticipos As WebReferenceAnticipos.SAT_NHPrePaymentService
    Private DocumentoContextoAnticipos As WebReferenceAnticipos.DocumentContext
    Private OrdersLineAnticipos(0) As WebReferenceAnticipos.AxdEntity_Tabla
    Private QweryOrdersAnticipos As WebReferenceAnticipos.AxdSAT_NHPrePayment





    ' Web Services Venta de Tikets y Objetos Relacionados 
    Private WebTikets As WebReferenceVentaTikets.SAT_JournalLossProfitQueryService
    Private DocumentoContextoTikets As WebReferenceVentaTikets.DocumentContext
    Private OrdersLineTikets(0) As WebReferenceVentaTikets.AxdEntity_SAT_NHJournalLossProfitTable
    Private QweryOrdersTikets As WebReferenceVentaTikets.AxdSAT_JournalLossProfitQuery



    ' Web Services Analitica y Objetos Relacionados 
    Private WebAnalitica As WebReferenceAnalitica.SAT_NHInfoAnalyticalQueryService
    Private DocumentoContextoAnalitica As WebReferenceAnalitica.DocumentContext
    Private OrdersLineAnalitica(0) As WebReferenceAnalitica.AxdEntity_SAT_NHInfoAnalytical_1
    Private QweryOrdersAnalitica As WebReferenceAnalitica.AxdSAT_NHInfoAnalyticalQuery

    Private mProcid As String
    Private mResultStr As String


#Region "CONSTRUCTOR"



    Public Sub New(ByVal vfecha As Date, ByVal vStrConexion As String, ByVal vEmpgrupo_Cod As String, ByVal vEmp_Cod As String, ByVal vFormato As String, ByVal vNoWait As Boolean, ByVal vPerfilContable As String, ByVal vStrConexionHotel As String)
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()
        Me.mFecha = vfecha

        Me.mEmpGrupoCod = vEmpgrupo_Cod
        Me.mEmpCod = vEmp_Cod
        Me.mPerfilContable = vPerfilContable
        Me.Text = Me.Text & " " & Me.mFecha & " Grupo : " & vEmpgrupo_Cod & " Empresa : " & vEmp_Cod & " Formato :" & vFormato
        Me.mStrConexion = vStrConexion
        Me.mStrConexionHotel = vStrConexionHotel
        Me.Update()
    End Sub
#End Region

    Private Sub FormConvertirAxapta_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If IsNothing(Me.Db) = False Then
                If Me.Db.EstadoConexion = ConnectionState.Open Then

                    SQL = "DELETE  TG_CONTROL WHERE CONTROL_DATA = '"
                    SQL += Format(Me.mFecha, "dd/MM/yyyy") & "'"
                    SQL += " AND CONTROL_ID= '" & Me.mProcid & "'"
                    SQL += " AND CONTROL_TIPO = 'NewHotel'"

                    Me.Db.EjecutaSqlCommit(SQL)

                    Me.Db.CerrarConexion()
                End If
            End If
            If IsNothing(Me.DbAux) = False Then
                If Me.DbAux.EstadoConexion = ConnectionState.Open Then
                    Me.DbAux.CerrarConexion()
                End If
            End If
            If IsNothing(Me.DbAux2) = False Then
                If Me.DbAux2.EstadoConexion = ConnectionState.Open Then
                    Me.DbAux2.CerrarConexion()
                End If
            End If
            If IsNothing(Me.DbWrite) = False Then
                If Me.DbWrite.EstadoConexion = ConnectionState.Open Then
                    Me.DbWrite.CerrarConexion()
                End If
            End If

        Catch ex As Exception

            MsgBox(ex.Message)
        Finally
            ESTACARGADOFORMCONVERTIR = False
        End Try
       
    End Sub

#Region "LOAD"


    Private Sub FormConvertirAxapta_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Db = New C_DATOS.C_DatosOledb(Me.mStrConexion)
            Me.Db.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbAux = New C_DATOS.C_DatosOledb(Me.mStrConexion, False)
            Me.DbAux.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbAux2 = New C_DATOS.C_DatosOledb(Me.mStrConexion)
            Me.DbAux2.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbWrite = New C_DATOS.C_DatosOledb(Me.mStrConexion)
            Me.DbWrite.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


            Me.LeeParametros()

            Me.DataGridViewAsientos.Font = New Font("arial", 8, FontStyle.Regular)

            Me.MostrarAsientos()

            Me.ConfiguraPerfilEnvio(Me.mPerfilContable)

            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Load")
        End Try
    End Sub
#End Region


#Region "RUTINAS PRIVADAS"
    Private Sub MostrarAsientos()
        Try

            SQL = "SELECT DECODE (ASNT_WEBSERVICE_NAME,'SAT_NHCreateProdOrdersQueryService','1- Pedido de Producción Axapta','SAT_NHPrePaymentService','2- Anticipos Recibidos','SAT_NHCreateSalesOrdersQueryService','3- Pedido de Ventas (Facturas) Axapta','SAT_NHJournalCustPaymentQueryService','4- Pedido de Cobros Axapta','SAT_JournalLossProfitQueryService','5- Perdidas y Ganancias Venta de Tikets','SAT_NHInfoAnalyticalQueryService','6- Analítica (Estadísticas)','?') AS TIPO,"
            SQL += " DECODE(ASNT_AX_STATUS,0,'No Enviado',1,'Ok Enviado','7','Corregido','8','Omitido','9','Excluido','?') AS ESTADO,ASNT_F_VALOR AS F_VALOR, "

            SQL += " DECODE (ASNT_WEBSERVICE_NAME,'SAT_NHPrePaymentService', ASNT_FORE_CODI_AX,'SAT_NHJournalCustPaymentQueryService',ASNT_CFCTA_COD,'') AS FORMA "

          
            SQL += ",ASNT_I_MONEMP AS IMPORTE, "
            SQL += " ASNT_AMPCPTO AS CONCEPTO,"
            '  SQL += "  round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,"
            SQL += " NVL(ASNT_NOMBRE,' ') AS OBSERVACION,ASNT_CIF AS ""NIF/CIF"",ASNT_DPTO_CODI AS DEPARTAMENTO ,ASNT_PROD_ID AS AXARTICULO, ASNT_WEBSERVICE_NAME, "
            SQL += " ASNT_LIN_VCRE AS ""VALOR CREDITO"" ,ASNT_LIN_VLIQ AS ""VALOR LIQUIDO"" , ASNT_LIN_IMP1 AS ""VALOR DE IMPUESTO"" ,"
            SQL += " ASNT_LIN_DOCU AS DOCUMENTO , ASNT_DOCU_VALO AS ""TOTAL FACTURA"" ,ASNT_LIN_TIIMP AS TIPOIMPUESTO "
            SQL += ",ASNT_TIPO_PROD AS ""TIPO DE PRODUCCION"" "
            SQL += ",ASNT_PROD_ID AS ARTICULO  "
            SQL += ",ASNT_PROD_TALLA AS TALLA "
            SQL += ",ASNT_PROD_COLOR COLOR "
            SQL += ",ASNT_ALMA_AX ""ALMACEN DESTINO AXAPTA"" , NVL(ASNT_RECIBO,'?') AS RECIBO ,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA "
            SQL += " , ASNT_FECHA_EXE AS VERSION, ASNT_PAT_NUM AS PAT ,ASNT_ERR_MESSAGE AS ""ERR MESSAGE"", ASNT_AX_REC AS RECIBIDO,NVL(ASNT_AUXILIAR_STRING2,' ') AS PREFIJO,ASNT_BOOKID AS BOOKID,ASNT_RECIBO,ROWID,ASNT_AUX_UPDATE AS AUXILIAR"

            SQL += " ,ASNT_AUX_UPDATE2 AS AUXILIAR2,ASNT_AUX_UPDATE3 AS AUXILIAR3,ASNT_LOG AS LOG,'' AS NOTHING,ASNT_RESE_FACT AS RESERVAS "
            SQL += "  ,ASNT_DPTO_DESC AS DEPARTAMENTOS "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME IS NOT NULL"


            ' SOLO SIN PROCESAR Y EXCLUIDOS
            If Me.CheckBoxSoloPendientes.Checked Then
                SQL += " AND ASNT_AX_STATUS <>  1 "
            End If

            ' SOLO  EXCLUIDOS
            If Me.CheckBoxExcluidos.Checked Then
                SQL += " AND (ASNT_AX_STATUS = 7 OR ASNT_AX_STATUS = 8 OR ASNT_AX_STATUS = 9) "
            End If

            ' con Book id
            If Me.CheckBoxBoockId.Checked Then
                SQL += " AND ASNT_BOOKID  IS NOT NULL  "
            End If



            'SQL += " ORDER BY TIPO, ASNT_CFATOCAB_REFER,ASNT_LINEA"
            SQL += " ORDER BY TIPO, ASNT_CFATOCAB_REFER,ASNT_LINEA,ASNT_LIN_DOCU"
            Me.DataGridViewAsientos.DataSource = Me.Db.TraerDataset(SQL, "ASIENTOS")
            Me.DataGridViewAsientos.DataMember = "ASIENTOS"

            If Me.Db.mDbDataset.Tables("ASIENTOS").Rows.Count > 0 Then
                Me.HayRegistros = True
                '  DataGridViewAsientos.Rows(0).Selected = False
                '  DataGridViewAsientos.Update()
            Else
                Me.HayRegistros = False
            End If



        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub ConfiguraPerfilEnvio(ByVal vPerfil As String)
        Try
            If vPerfil = "CONTABILIDAD" Then
                Me.CheckBoxEnviaProduccion.Checked = True
                Me.CheckBoxEnviaAnticipos.Checked = True
                Me.CheckBoxEnviaFacturacion.Checked = True
                Me.CheckBoxEnviaCobros.Checked = True
                Me.CheckBoxEnviaInventario.Checked = False
                Me.CheckBoxEstadistica.Checked = True
            ElseIf vPerfil = "ALMACEN" Then
                Me.CheckBoxEnviaProduccion.Checked = False
                Me.CheckBoxEnviaAnticipos.Checked = False
                Me.CheckBoxEnviaFacturacion.Checked = False
                Me.CheckBoxEnviaCobros.Checked = False
                Me.CheckBoxEnviaInventario.Checked = True
                Me.CheckBoxEstadistica.Checked = False
            Else
                Me.CheckBoxEnviaProduccion.Checked = True
                Me.CheckBoxEnviaAnticipos.Checked = True
                Me.CheckBoxEnviaFacturacion.Checked = True
                Me.CheckBoxEnviaCobros.Checked = True
                Me.CheckBoxEnviaInventario.Checked = True
                Me.CheckBoxEstadistica.Checked = True
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ProcesaJob1()

        Try

            SQL = "SELECT   ' PARTIDAS PENDIENTES', asnt_webservice_name AS tipo, "
            SQL = SQL & "         asnt_rese_codi || '/' || asnt_rese_anci AS reserva, "
            SQL = SQL & "         asnt_rese_codi , asnt_rese_anci , "
            SQL = SQL & "         th_asnt.asnt_lin_docu AS factura, "
            SQL = SQL & "         SUBSTR (th_asnt.asnt_lin_docu, "
            SQL = SQL & "                 1, "
            SQL = SQL & "                 INSTR (th_asnt.asnt_lin_docu, '/') - 1 "
            SQL = SQL & "                ) AS numero, "
            SQL = SQL & "         SUBSTR (th_asnt.asnt_lin_docu, "
            SQL = SQL & "                 INSTR (th_asnt.asnt_lin_docu, '/') + 1, "
            SQL = SQL & "                 5 "
            SQL = SQL & "                ) AS serie, "
            SQL = SQL & "         vh_facturas.asnt_docu_valo AS ""TOTAL FACTURA"", "
            SQL = SQL & "         SUM (asnt_i_monemp) AS cobro, "
            SQL = SQL & "         vh_facturas.asnt_docu_valo - SUM (asnt_i_monemp) AS auditoria, "
            SQL = SQL & "         asnt_ax_status AS estado, asnt_f_valor AS fecha, asnt_nombre, "
            SQL = SQL & "         asnt_auxiliar_string AS ayuda, asnt_fore_codi_ax AS forma, "
            SQL = SQL & "         asnt_cfcta_cod AS formaco, SUBSTR (asnt_err_message, 250, 150),TH_ASNT.ROWID AS R,ASNT_AMPCPTO"
            SQL = SQL & "    FROM th_asnt, vh_facturas "
            SQL = SQL & "   WHERE th_asnt.asnt_lin_docu = vh_facturas.asnt_lin_docu "
            SQL = SQL & "     AND th_asnt.asnt_f_valor > '31/DEC/2010' "
            SQL = SQL & "     AND th_asnt.asnt_webservice_name = 'SAT_NHJournalCustPaymentQueryService' "
            SQL = SQL & "     AND th_asnt.asnt_ax_status = 0 "
            ' '' EVA !!!!!!!!!!!!!! VER POR QUE NO  PAA SIN ESTA LINEA 
            SQL = SQL & "     AND th_asnt.asnt_fore_codi_ax IS NOT NULL "



            SQL = SQL & " AND ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"


            SQL = SQL & "     AND th_asnt.asnt_lin_docu IN (SELECT th_asnt.asnt_lin_docu "
            SQL = SQL & "                                     FROM th_asnt "
            SQL = SQL & "                                    WHERE th_asnt.asnt_ax_status = 1) "
            SQL = SQL & "GROUP BY asnt_webservice_name, "
            SQL = SQL & "         asnt_rese_codi || '/' || asnt_rese_anci, "
            SQL = SQL & "         th_asnt.asnt_lin_docu, "
            SQL = SQL & "         SUBSTR (th_asnt.asnt_lin_docu, "
            SQL = SQL & "                 1, "
            SQL = SQL & "                 INSTR (th_asnt.asnt_lin_docu, '/') - 1 "
            SQL = SQL & "                ), "
            SQL = SQL & "         SUBSTR (th_asnt.asnt_lin_docu, "
            SQL = SQL & "                 INSTR (th_asnt.asnt_lin_docu, '/') + 1, "
            SQL = SQL & "                 5 "
            SQL = SQL & "                ), "
            SQL = SQL & "         vh_facturas.asnt_docu_valo, "
            SQL = SQL & "         asnt_ax_status, "
            SQL = SQL & "         asnt_f_valor, "
            SQL = SQL & "         asnt_nombre, "
            SQL = SQL & "         asnt_auxiliar_string, "
            SQL = SQL & "         asnt_fore_codi_ax, "
            SQL = SQL & "         asnt_cfcta_cod, "
            SQL = SQL & "         SUBSTR (asnt_err_message, 250, 150),ASNT_RESE_CODI,ASNT_RESE_ANCI,TH_ASNT.ROWID ,ASNT_AMPCPTO "
            SQL = SQL & "ORDER BY TO_NUMBER (SUBSTR (th_asnt.asnt_lin_docu, "
            SQL = SQL & "                            1, "
            SQL = SQL & "                            INSTR (th_asnt.asnt_lin_docu, '/') - 1 "
            SQL = SQL & "                           ) "
            SQL = SQL & "                   ) ASC "


            Me.Db.TraerLector(SQL)

            Me.DbWrite.IniciaTransaccion()

            While Me.Db.mDbLector.Read

                '  Me.ListBox1.Items.Add("Reserva = " & Me.Db.mDbLector.Item("RESERVA"))

                SQL = "  SELECT * FROM TH_ASNT WHERE "
                SQL += " ASNT_RESE_CODI = '" & Me.Db.mDbLector.Item("ASNT_RESE_CODI") & "'"
                SQL += " AND  ASNT_RESE_ANCI = " & Me.Db.mDbLector.Item("ASNT_RESE_ANCI")
                ' ES UN ANTICIPO
                SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHPrePaymentService'"
                ' ES UN ANTICIPO
                SQL += " AND ASNT_AUXILIAR_STRING = 'ANTICIPO'"
                ' ES UNA RECEPCION DE ANTICIPO
                SQL += " AND ASNT_FACTURA_NUMERO  IS NULL"
                ' ESTA ENVIADO
                SQL += " AND ASNT_AX_STATUS = 1 "



                Me.DbAux.TraerLector(SQL)
                Me.DbAux.mDbLector.Read()
                If Me.DbAux.mDbLector.HasRows Then
                    MsgBox("Anticipo Candidato para tomar su Forma de Cobro  = " & CStr(Me.DbAux.mDbLector.Item("ASNT_AMPCPTO")).PadLeft(50, " ") & CStr(Me.DbAux.mDbLector.Item("ASNT_NOMBRE")).PadLeft(50, " ") & CStr(Me.DbAux.mDbLector.Item("ASNT_I_MONEMP")).PadLeft(15, " ") & CStr(Me.DbAux.mDbLector.Item("ASNT_FORE_CODI_AX")).PadLeft(15, " "), MsgBoxStyle.Information, "Atención")

                    If Me.Db.mDbLector.Item("FORMA") <> Me.DbAux.mDbLector.Item("ASNT_FORE_CODI_AX") Then
                        SQL = " UPDATE TH_ASNT SET  "
                        ' forma VIEJA SEGURO
                        SQL += " ASNT_AUX_FORE_CODI_AX = '" & Me.Db.mDbLector.Item("FORMA") & "'"
                        ' forma NUEVA UPDATE 
                        SQL += ",ASNT_FORE_CODI_AX = '" & Me.DbAux.mDbLector.Item("ASNT_FORE_CODI_AX") & "'"

                        ' FORMA ORIGINAL = FORMA nueva
                        SQL += ",ASNT_AUX_UPDATE2 = '" & Me.Db.mDbLector.Item("FORMA") & " = " & Me.DbAux.mDbLector.Item("ASNT_FORE_CODI_AX") & "'"
                        SQL += ",ASNT_AUX_UPDATE3 = '" & Me.Db.mDbLector.Item("ASNT_AMPCPTO") & "'"
                        SQL += ",ASNT_AMPCPTO = '" & Me.Db.mDbLector.Item("ASNT_AMPCPTO") & " [FORMA DE PAGO ORIGINAL DE LA DEVOLUCIÓN = " & Me.Db.mDbLector.Item("FORMA") & "]'"

                        SQL += " WHERE ROWID = '" & Me.Db.mDbLector.Item("R") & "'"
                        Me.DbWrite.EjecutaSql(SQL)
                    End If

                Else
                    '  Me.ListBox1.Items.Add("Candidato = " & " No se localiza !!")

                End If



            End While
            Me.Db.mDbLector.Close()

            Me.Update()

            If MessageBox.Show("Confirma Transaccion ?", "Atención", MessageBoxButtons.OKCancel) = Windows.Forms.DialogResult.OK Then
                Me.DbWrite.ConfirmaTransaccion()
            Else
                Me.DbWrite.CancelaTransaccion()
            End If


        Catch ex As Exception
            Me.DbWrite.CancelaTransaccion()
            MsgBox(ex.Message)
        End Try


    End Sub

    Private Sub EnvioProduccion()
        Try

            '   Dim Ind As Integer
            Dim Primerregistro As Boolean = True


            Me.WebProduccion = New WebReferenceProduccion.SAT_NHCreateProdOrdersQueryService





            'Actualiza Url

            Me.WebProduccion.Url = Me.mAxUrl & "SAT_NHCreateProdOrdersQueryService.asmx"



            Me.TextBoxRutaWebServices.Text = Me.WebProduccion.Url
            Me.TextBoxRutaWebServices.Update()

            Me.DocumentoContexto = New WebReferenceProduccion.DocumentContext
            Me.QweryOrders = New WebReferenceProduccion.AxdSAT_NHCreateProdOrdersQuery

            '  Me.ResponProduccion(0) = New WebReferenceProduccion.EntityKey



            Me.WebProduccion.Credentials = System.Net.CredentialCache.DefaultCredentials
            '


            Me.DocumentoContexto.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContexto.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContexto.DestinationEndpoint = Me.mAXDestinationEndPoint
            'Me.DocumentoContexto.SourceEndpointUser = Environment.ExpandEnvironmentVariables("%userdomain%\\%username%")
            Me.DocumentoContexto.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)

            Me.OrdersTabla(0) = New WebReferenceProduccion.AxdEntity_SAT_NHCreateProdOrdersTable_1


            Me.OrdersTabla(0).InvoiceDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
            Me.OrdersTabla(0).InvoiceDateSpecified = True



            Me.ListBoxDebug.Items.Add(" => MessageId            : " & Me.DocumentoContexto.MessageId.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpoint       : " & Me.DocumentoContexto.SourceEndpoint.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpointUser   : " & Me.DocumentoContexto.SourceEndpointUser.ToString)
            Me.ListBoxDebug.Items.Add(" => DestinationEndpoint  : " & Me.DocumentoContexto.DestinationEndpoint.ToString)
            'Me.ListBoxDebug.Items.Add(" => Bond                 : " & Me.OrdersTabla(0).Bond.ToString)
            Me.ListBoxDebug.Items.Add(" => InvoiceDAte          : " & Me.OrdersTabla(0).InvoiceDate.ToString)



            ' Lineas de produccion 

            SQL = "SELECT ROWID,NVL(ASNT_CFCTA_COD,' ') AS CUENTA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,"
            SQL += "NVL(ASNT_DPTO_CODI,'?') AS SERVICIO ,NVL(ASNT_I_MONEMP,'0')  AS IMPORTE"
            SQL += ", NVL(ASNT_TIPO_PROD,'?') AS ASNT_TIPO_PROD "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHCreateProdOrdersQueryService'"

            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"


            Me.Db.TraerLector(SQL)

            While Me.Db.mDbLector.Read

                If Primerregistro = True Then
                    Primerregistro = False
                    Ind = 0
                    ReDim Me.OrdersLine(Ind)
                    ReDim Me.TransRowid(Ind)

                Else                ' añade un elemento a array de lineas / Objeto
                    ReDim Preserve Me.OrdersLine(UBound(Me.OrdersLine) + 1)
                    ReDim Preserve Me.TransRowid(UBound(Me.OrdersLine) + 1)
                    Ind = UBound(Me.OrdersLine)

                End If


                Me.OrdersLine(Ind) = New WebReferenceProduccion.AxdEntity_SAT_NHCreateProdOrdersLine_1

                Me.OrdersLine(Ind).ItemId = Me.Db.mDbLector.Item("SERVICIO")

                If Me.Db.mDbLector.Item("ASNT_TIPO_PROD") = Me.mAxServCodiBonos Then
                    Me.OrdersTabla(0).Bond = WebReferenceProduccion.AxdExtType_NoYesId.Yes
                    Me.OrdersTabla(0).BondSpecified = True
                Else
                    Me.OrdersTabla(0).Bond = WebReferenceProduccion.AxdExtType_NoYesId.No
                    Me.OrdersTabla(0).BondSpecified = True
                End If

                Me.OrdersLine(Ind).SalesPrice = Me.Db.mDbLector.Item("IMPORTE")



                Me.OrdersLine(Ind).Qty = 1
                Me.OrdersLine(Ind).QtySpecified = True

                Me.ListBoxDebug.Items.Add(" ==> ItemIds y Qtys : " & CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(50, " ") & " " & CType(Me.Db.mDbLector.Item("IMPORTE"), String).PadRight(15, " ") & "  Bono : " & CType(Me.Db.mDbLector.Item("ASNT_TIPO_PROD"), String))
                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                Me.TransRowid(Ind) = Me.Db.mDbLector.Item("ROWID")


            End While
            Me.Db.mDbLector.Close()



            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Llamada al Web Service Producción " & Me.WebProduccion.Url)
            Me.ListBoxDebug.Update()


            Elementos = Me.OrdersLine.GetLength(Dimension)
            Me.TextBoxDebug.Text = Elementos



            ' LLAMADA AL WEB SERVICE
            If Elementos > 0 And IsNothing(Me.OrdersLine(0)) = False Then
                Me.OrdersTabla(0).SAT_NHCreateProdOrdersLine_1 = Me.OrdersLine
                Me.QweryOrders.SAT_NHCreateProdOrdersTable_1 = Me.OrdersTabla


                Me.WebProduccion.createListSAT_NHCreateProdOrdersQuery(Me.DocumentoContexto, Me.QweryOrders)

                Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Axapta Ok  " & Me.WebProduccion.Url)
                Me.TrataenvioGenerico(Me.TransRowid(Ind), 0, "OK")
            Else
                Me.ListBoxDebug.Items.Add(Format(Now, "F") & " No hay Elementos que enviar   " & Me.WebProduccion.Url)
            End If


        Catch ex As Exception

            Dim s As New System.Web.Services.Protocols.SoapException
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service Producción " & Me.WebProduccion.Url & " : " & ex.Message & " + " & s.Message)

            MsgBox(ex.Message)
            '     MsgBox(ex.ToString)
        End Try
        '   Catch ex As Web.Services.Protocols.SoapException
        '       MsgBox(ex.Message)'

        'End Try





    End Sub

    Private Sub EnvioFacturacion()
        Try

            '   Dim Ind As Integer

            Dim Primerregistro As Boolean = False

            Dim ControlFacturaBono As Boolean = False
            Dim ControlFacturaOtrosServicios As Boolean = False


            Me.WebFacturas = New WebReferenceFacturas.SAT_NHCreateSalesOrdersQueryService

            'Actualiza Url

            Me.WebFacturas.Url = Me.mAxUrl & "SAT_NHCreateSalesOrdersQueryService.asmx"


            Me.TextBoxRutaWebServices.Text = Me.WebFacturas.Url
            Me.TextBoxRutaWebServices.Update()

            Me.DocumentoContextoFacturas = New WebReferenceFacturas.DocumentContext
            Me.QweryOrdersFacturas = New WebReferenceFacturas.AxdSAT_NHCreateSalesOrdersQuery


            'Me.WebFacturas.Credentials = System.Net.CredentialCache.DefaultCredentials
            Me.WebFacturas.Credentials = New System.Net.NetworkCredential(Me.mAxUserName, Me.mAxUserPwd, Me.mAxDomainName)
            '


            Me.DocumentoContextoFacturas.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContextoFacturas.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContextoFacturas.DestinationEndpoint = Me.mAXDestinationEndPoint
            'Me.DocumentoContexto.SourceEndpointUser = Environment.ExpandEnvironmentVariables("%userdomain%\\%username%")
            Me.DocumentoContextoFacturas.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)


            ' CABECERA DE FACTURA


            'SQL = "SELECT  ASNT_LIN_DOCU AS DOCUMENTO,NVL(ASNT_CFCTA_COD,'?') AS CUENTA,MAX(ASNT_DOCU_VALO) AS TOTAL,"
            'SQL += " NVL(ASNT_CIF,'?') AS CIF "
            'SQL += " FROM TH_ASNT "
            'SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            'SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            'SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            'SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"

            'SQL += "  GROUP BY ASNT_LIN_DOCU,NVL(ASNT_CFCTA_COD,'?'),NVL(ASNT_CIF,'?') "
            'SQL += " ORDER BY ASNT_LIN_DOCU ASC "

            '
            '   MsgBox("OJO CON EL MAX DE ESTA QWERY HACE QUE NO SE MUESTREN LAS NOTAS DE CREDITO ANULADAS", MsgBoxStyle.Exclamation, "revisar !!!!!!!")

            MsgBox("ojo falta tratamiento de campo nhcustaccountinfo segun notas de marta esta en ASNT_NHCUSTACCOUNT", MsgBoxStyle.Information, " falta Desarrollo")

            SQL = "SELECT  DISTINCT ASNT_LIN_DOCU AS DOCUMENTO,NVL(ASNT_CFCTA_COD,'?') AS CUENTA,ASNT_DOCU_VALO AS TOTAL,"
            SQL += " NVL(ASNT_CIF,'?') AS CIF ,ASNT_TIPOFACTURA_AX AS TIPOCLIENTE,NVL(ASNT_NHCUSTACCOUNT,'?') AS ASNT_NHCUSTACCOUNT "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"

            '  SQL += "  GROUP BY ASNT_LIN_DOCU,NVL(ASNT_CFCTA_COD,'?'),NVL(ASNT_CIF,'?') "
            SQL += " ORDER BY ASNT_LIN_DOCU ASC "


            Me.Db.TraerLector(SQL)

            While Me.Db.mDbLector.Read

                ControlFacturaBono = False
                ControlFacturaOtrosServicios = False


                Me.OrdersTablaFacturas(0) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersTable_1

                ' 3 = CLIENTES CONTADO
                If CType(Me.Db.mDbLector.Item("TIPOCLIENTE"), Integer) = 3 Then
                    Me.OrdersTablaFacturas(0).ContClient = WebReferenceFacturas.AxdExtType_NoYesId.Yes
                Else
                    Me.OrdersTablaFacturas(0).ContClient = WebReferenceFacturas.AxdExtType_NoYesId.No
                End If
                Me.OrdersTablaFacturas(0).ContClientSpecified = True



                Me.OrdersTablaFacturas(0).VATNum = CType(Me.Db.mDbLector.Item("CIF"), String)

                Me.OrdersTablaFacturas(0).CustAccount = CType(Me.Db.mDbLector.Item("ASNT_NHCUSTACCOUNT"), String)
                Me.OrdersTablaFacturas(0).NHCustAccount = CType(Me.Db.mDbLector.Item("ASNT_NHCUSTACCOUNT"), String)

                Me.OrdersTablaFacturas(0).DocumentDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
                Me.OrdersTablaFacturas(0).DocumentDateSpecified = True
                Me.OrdersTablaFacturas(0).FixedDueDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
                Me.OrdersTablaFacturas(0).FixedDueDateSpecified = True
                Me.OrdersTablaFacturas(0).InvoiceDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
                'Me.OrdersTablaFacturas(0).InvoiceDateSpecified = True
                Me.OrdersTablaFacturas(0).InvoiceId = CType(Me.Db.mDbLector.Item("DOCUMENTO"), String)
                Me.OrdersTablaFacturas(0).SalesAmount = CType(Me.Db.mDbLector.Item("TOTAL"), Double)
                Me.OrdersTablaFacturas(0).SalesAmountSpecified = True






                Me.ListBoxDebug.Items.Add(" => MessageId            : " & Me.DocumentoContextoFacturas.MessageId.ToString)
                Me.ListBoxDebug.Items.Add(" => SourceEndpoint       : " & Me.DocumentoContextoFacturas.SourceEndpoint.ToString)
                Me.ListBoxDebug.Items.Add(" => SourceEndpointUser   : " & Me.DocumentoContextoFacturas.SourceEndpointUser.ToString)
                Me.ListBoxDebug.Items.Add(" => DestinationEndpoint  : " & Me.DocumentoContextoFacturas.DestinationEndpoint.ToString)
                Me.ListBoxDebug.Items.Add(" => InvoiceDAte          : " & OrdersTablaFacturas(0).InvoiceDate.ToString)
                Me.ListBoxDebug.Items.Add(" => Cliente              : " & Me.OrdersTablaFacturas(0).CustAccount)
                Me.ListBoxDebug.Items.Add(" => Cliente Contado      : " & Me.OrdersTablaFacturas(0).ContClient.ToString)
                Me.ListBoxDebug.Items.Add(" => Total                : " & Me.OrdersTablaFacturas(0).SalesAmount.ToString)




                ' Lineas de FActura

                'SQL = "SELECT  ASNT_LIN_DOCU AS DOCUMENTO, ASNT_DPTO_CODI AS SERVICIO,"
                'SQL += "SUM(ROUND(ASNT_LIN_IMP1,2)) AS VALORIMPUESTO,SUM(ROUND(ASNT_LIN_VLIQ,2)) AS VALORLIQUIDO,ASNT_LIN_TIIMP AS PORCE "
                'SQL += " FROM TH_ASNT "
                'SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
                'SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                'SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                'SQL += " AND ASNT_LIN_DOCU = '" & Me.Db.mDbLector.Item("DOCUMENTO") & "'"
                'SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"
                'SQL += " GROUP BY ASNT_LIN_DOCU,ASNT_DPTO_CODI,ASNT_LIN_TIIMP"
                'SQL += " ORDER BY ASNT_LIN_DOCU ASC "

                SQL = "SELECT  ASNT_LIN_DOCU AS DOCUMENTO, ASNT_DPTO_CODI AS SERVICIO,"
                SQL += "ROUND(ASNT_LIN_IMP1,2) AS VALORIMPUESTO,ROUND(ASNT_LIN_VLIQ,2) AS VALORLIQUIDO,ASNT_LIN_TIIMP AS PORCE "
                SQL += " FROM TH_ASNT "
                SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
                SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND ASNT_LIN_DOCU = '" & Me.Db.mDbLector.Item("DOCUMENTO") & "'"
                ' DEBAJO PARA DISTINGUIR LAS NOTAS DE CREDITO Y FACTURAS DE NEWGOLF ANULADAS POR SER EL MISMO DOCUMENTO POSITIVO Y NEGATIVO
                SQL += " AND TH_ASNT.ASNT_DOCU_VALO = " & Me.Db.mDbLector.Item("TOTAL")
                SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"
                SQL += " ORDER BY ASNT_LIN_DOCU ASC "

                Me.DbAux.TraerLector(SQL)

                Primerregistro = True

                While Me.DbAux.mDbLector.Read

                    If Primerregistro = True Then
                        Primerregistro = False
                        Ind = 0
                        ReDim Me.OrdersLineFacturas(Ind)
                        ReDim Me.OrdersLineTaxFacturas(Ind)
                    Else                ' añade un elemento a array de lineas / Objeto
                        ReDim Preserve Me.OrdersLineFacturas(UBound(Me.OrdersLineFacturas) + 1)
                        ReDim Preserve Me.OrdersLineTaxFacturas(UBound(Me.OrdersLineTaxFacturas) + 1)

                        Ind = UBound(Me.OrdersLineFacturas)

                    End If



                    Me.OrdersLineFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersLine_1
                    Me.OrdersLineTaxFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersTaxLine_1


                    ' SERVICIO

                    Me.OrdersLineFacturas(Ind).ItemId = CType(Me.DbAux.mDbLector.Item("SERVICIO"), String)


                    Me.OrdersLineFacturas(Ind).Qty = 1
                    Me.OrdersLineFacturas(Ind).QtySpecified = True

                    ' IMPUESTO
                    Me.OrdersLineTaxFacturas(Ind).TaxAmount = CType(Me.DbAux.mDbLector.Item("VALORIMPUESTO"), Decimal)
                    Me.OrdersLineTaxFacturas(Ind).TaxAmountSpecified = True

                    Me.OrdersLineTaxFacturas(Ind).TaxBase = CType(Me.DbAux.mDbLector.Item("VALORLIQUIDO"), Decimal)
                    Me.OrdersLineTaxFacturas(Ind).TaxBaseSpecified = True


                    Me.OrdersLineTaxFacturas(Ind).TaxCode = CType(Me.DbAux.mDbLector.Item("PORCE"), String) & "%"
                    Me.OrdersLineTaxFacturas(Ind).TaxPercent = CType(Me.DbAux.mDbLector.Item("PORCE"), Decimal)
                    Me.OrdersLineTaxFacturas(Ind).TaxPercentSpecified = True


                    ' control que de no enviar facturas que contengan Bonos y otros servicios a la vez 

                    If Me.DbAux.mDbLector.Item("SERVICIO") = Me.mAxServCodiBonos Then
                        ControlFacturaBono = True
                        Me.OrdersTablaFacturas(0).Bond = WebReferenceFacturas.AxdExtType_NoYesId.Yes
                        Me.OrdersTablaFacturas(0).BondSpecified = True
                    End If

                    If Me.DbAux.mDbLector.Item("SERVICIO") <> Me.mAxServCodiBonos Then
                        ControlFacturaOtrosServicios = True
                        Me.OrdersTablaFacturas(0).Bond = WebReferenceFacturas.AxdExtType_NoYesId.No
                        Me.OrdersTablaFacturas(0).BondSpecified = True
                    End If



                    Me.ListBoxDebug.Items.Add(" ==> Lineas de Fac  : " & CType(Me.DbAux.mDbLector.Item("DOCUMENTO"), String).PadRight(10, " ") & " Base : " & CType(Me.DbAux.mDbLector.Item("VALORLIQUIDO"), String).PadRight(35, " ") & " Impuesto :" & CType(Me.DbAux.mDbLector.Item("VALORIMPUESTO"), String).PadRight(10, " ") & " Departamento =  " & CType(Me.DbAux.mDbLector.Item("SERVICIO"), String))
                    Me.ListBoxDebug.Update()
                    Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                End While
                Me.DbAux.mDbLector.Close()



                Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Llamada al Web Service Facturas " & Me.WebFacturas.Url)
                Me.ListBoxDebug.Update()


                Elementos = Me.OrdersLineFacturas.GetLength(Dimension)
                Me.TextBoxDebug.Text = Elementos



                ' LLAMADA AL WEB SERVICE
                If (ControlFacturaBono = True And ControlFacturaOtrosServicios = False) Or (ControlFacturaBono = False And ControlFacturaOtrosServicios = True) Then

                    If Elementos > 0 And IsNothing(Me.OrdersLineFacturas(0)) = False Then
                        Me.OrdersTablaFacturas(0).SAT_NHCreateSalesOrdersLine_1 = Me.OrdersLineFacturas
                        Me.QweryOrdersFacturas.SAT_NHCreateSalesOrdersTable_1 = Me.OrdersTablaFacturas
                        Me.WebFacturas.createListSAT_NHCreateSalesOrdersQuery(Me.DocumentoContextoFacturas, Me.QweryOrdersFacturas)

                        Me.ListBoxDebug.Items.Add("---------------------------------------------------------------------------------")

                        Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Axapta Ok  " & Me.WebFacturas.Url)
                    Else
                        Me.ListBoxDebug.Items.Add(Format(Now, "F") & " No Hay Elementos que enviar   " & Me.WebFacturas.Url)
                    End If
                Else
                    '     MsgBox("Atencion la factura " & CType(Me.Db.mDbLector.Item("DOCUMENTO"), String) & " NO se Transmite por Contener Bonos y Otros Servicios", MsgBoxStyle.Exclamation, "Atención")
                End If


                'Application.DoEvents()


            End While
            Me.Db.mDbLector.Close()




        Catch ex As Exception
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service Facturas " & Me.WebFacturas.Url & " : " & ex.Message)

            MsgBox(ex.Message)
            '     MsgBox(ex.ToString)
        End Try
        '   Catch ex As Web.Services.Protocols.SoapException
        '       MsgBox(ex.Message)'

        'End Try





    End Sub
    Private Sub EnvioCobros()
        Try

            '    Dim Ind As Integer


            Dim Primerregistro As Boolean = False


            Me.WebCobros = New WebReferenceCobros.SAT_NHJournalCustPaymentQueryService

            '    Actualiza(Url)

            Me.WebCobros.Url = Me.mAxUrl & "SAT_NHJournalCustPaymentQueryService.asmx"


            Me.TextBoxRutaWebServices.Text = Me.WebCobros.Url
            Me.TextBoxRutaWebServices.Update()

            Me.DocumentoContextoCobros = New WebReferenceCobros.DocumentContext
            Me.QweryOrdersCobros = New WebReferenceCobros.AxdSAT_NHJournalCustPaymentQuery


            Me.WebCobros.Credentials = System.Net.CredentialCache.DefaultCredentials
            '


            Me.DocumentoContextoCobros.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContextoCobros.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContextoCobros.DestinationEndpoint = Me.mAXDestinationEndPoint
            Me.DocumentoContextoCobros.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)




            Me.ListBoxDebug.Items.Add(" => MessageId            : " & Me.DocumentoContextoCobros.MessageId.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpoint       : " & Me.DocumentoContextoCobros.SourceEndpoint.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpointUser   : " & Me.DocumentoContextoCobros.SourceEndpointUser.ToString)
            Me.ListBoxDebug.Items.Add(" => DestinationEndpoint  : " & Me.DocumentoContextoCobros.DestinationEndpoint.ToString)


            ' Cobros en factura


            SQL = "SELECT  NVL(ASNT_LIN_DOCU,'?') AS DOCUMENTO,NVL(ASNT_CFCTA_COD,' ') AS FORMA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,NVL(ASNT_I_MONEMP,'0')  AS IMPORTE"
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService'"

            SQL += " ORDER BY ASNT_LIN_DOCU ASC "

            Me.Db.TraerLector(SQL)


            Primerregistro = True
            While Me.Db.mDbLector.Read

                If Primerregistro = True Then
                    Primerregistro = False
                    Ind = 0
                    ReDim Me.OrdersLineCobros(Ind)

                Else                ' añade un elemento a array de lineas / Objeto
                    ReDim Preserve Me.OrdersLineCobros(UBound(Me.OrdersLineCobros) + 1)
                    Ind = UBound(Me.OrdersLineCobros)
                End If



                Me.OrdersLineCobros(Ind) = New WebReferenceCobros.AxdEntity_SAT_NHJournalCustPayment_1


                Me.OrdersLineCobros(Ind).InvoiceId = CType(Me.Db.mDbLector.Item("DOCUMENTO"), String)
                Me.OrdersLineCobros(Ind).PaymMode = CType(Me.Db.mDbLector.Item("FORMA"), String)

             

                Me.OrdersLineCobros(Ind).Amount = CType(Me.Db.mDbLector.Item("IMPORTE"), Double)
                Me.OrdersLineCobros(Ind).AmountSpecified = True


                Me.OrdersLineCobros(Ind).TransDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
                'Me.OrdersLineCobros(Ind).TransDateSpecified = True




                Me.ListBoxDebug.Items.Add(" ==> Cobro          : " & CType(Me.Db.mDbLector.Item("DOCUMENTO"), String).PadRight(10, " ") & "  " & CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(60, " ") & CType(Me.OrdersLineCobros(Ind).Amount, String).PadRight(15, " ") & " CON " & CType(Me.Db.mDbLector.Item("FORMA"), String))

            End While
            Me.Db.mDbLector.Close()


            Me.ListBoxDebug.Items.Add("")
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Llamada al Web Service Cobros " & Me.WebCobros.Url)
            Me.ListBoxDebug.Items.Add("")
            Me.ListBoxDebug.Update()



            Elementos = Me.OrdersLineCobros.GetLength(Dimension)
            Me.TextBoxDebug.Text = Elementos


            ' LLAMADA AL WEB SERVICE
            If Elementos > 0 And IsNothing(Me.OrdersLineCobros(0)) = False Then
                Me.QweryOrdersCobros.SAT_NHJournalCustPayment_1 = Me.OrdersLineCobros
                Me.WebCobros.createListSAT_NHJournalCustPaymentQuery(Me.DocumentoContextoCobros, Me.QweryOrdersCobros)

                Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Axapta Ok  " & Me.WebCobros.Url)
            Else
                Me.ListBoxDebug.Items.Add(Format(Now, "F") & " No Hay Elenentos que enviar   " & Me.WebCobros.Url)
            End If

            'Application.DoEvents()


        Catch ex As Exception
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service Cobros " & Me.WebCobros.Url & " : " & ex.Message)

            MsgBox(ex.Message)
            '     MsgBox(ex.ToString)
        End Try
        '   Catch ex As Web.Services.Protocols.SoapException
        '       MsgBox(ex.Message)'

        'End Try

    End Sub
   
    Private Sub EnvioVentaTikets()
        Try

            '   Dim Ind As Integer

            Dim Primerregistro As Boolean = False

            Dim ControlFacturaBono As Boolean = False
            Dim ControlFacturaOtrosServicios As Boolean = False


            Me.WebTikets = New WebReferenceVentaTikets.SAT_JournalLossProfitQueryService

            'Actualiza Url

            Me.WebTikets.Url = Me.mAxUrl & "SAT_JournalLossProfitQueryService.asmx"


            Me.TextBoxRutaWebServices.Text = Me.WebTikets.Url
            Me.TextBoxRutaWebServices.Update()

            Me.DocumentoContextoTikets = New WebReferenceVentaTikets.DocumentContext
            Me.QweryOrdersTikets = New WebReferenceVentaTikets.AxdSAT_JournalLossProfitQuery


            Me.WebTikets.Credentials = System.Net.CredentialCache.DefaultCredentials
            '


            Me.DocumentoContextoTikets.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContextoTikets.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContextoTikets.DestinationEndpoint = Me.mAXDestinationEndPoint
            Me.DocumentoContextoTikets.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)


            ' TIKETS

            ' MsgBox("ojo las cantidades se envian negativas  y no se envian las devoluciones ", MsgBoxStyle.Information, "revisar")


            SQL = "SELECT ASNT_PROD_ID AS PRODUCTO,NVL(ASNT_AMPCPTO,'?') AS DESCRIPCION,ASNT_PROD_TALLA AS TALLA,ASNT_PROD_COLOR AS COLOR ,NVL(ASNT_I_MONEMP,'0')  AS CANTIDAD,ASNT_ALMA_AX AS ALMACEN "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_JournalLossProfitQueryService'"
            ' OJO 
            '  SQL += " AND ROWNUM < 10"


            Me.Db.TraerLector(SQL)

            Primerregistro = True

            While Me.Db.mDbLector.Read

                If Primerregistro = True Then
                    Primerregistro = False
                    Ind = 0
                    ReDim Me.OrdersLineTikets(Ind)
                    ReDim Me.OrdersLineTikets(Ind)
                Else                ' añade un elemento a array de lineas / Objeto
                    ReDim Preserve Me.OrdersLineTikets(UBound(Me.OrdersLineTikets) + 1)
                    Ind = UBound(Me.OrdersLineTikets)

                End If


                Me.OrdersLineTikets(Ind) = New WebReferenceVentaTikets.AxdEntity_SAT_NHJournalLossProfitTable


                ' articulo

                'Me.OrdersLineTikets(Ind).ItemId = Me.Db.mDbLector.Item("PRODUCTO")
                Me.OrdersLineTikets(Ind).ItemId = "SART-000035"

                ' talla
                If IsDBNull(Me.Db.mDbLector.Item("TALLA")) = False Then
                    Me.OrdersLineTikets(Ind).InventSizeId = Me.Db.mDbLector.Item("TALLA")
                End If
                Me.OrdersLineTikets(Ind).InventSizeId = "TAM000003"


                ' color
                If IsDBNull(Me.Db.mDbLector.Item("COLOR")) = False Then
                    Me.OrdersLineTikets(Ind).InventColorId = Me.Db.mDbLector.Item("COLOR")
                End If
                Me.OrdersLineTikets(Ind).InventColorId = "COL000004"


                ' cantidad
                Me.OrdersLineTikets(Ind).Qty = Me.Db.mDbLector.Item("CANTIDAD")
                Me.OrdersLineTikets(Ind).QtySpecified = True

                ' almacen
                'Me.OrdersLineTikets(Ind).InventLocationId = Me.Db.mDbLector.Item("ALMACEN")
                Me.OrdersLineTikets(Ind).InventLocationId = "Pru"




                ' fecha
                Me.OrdersLineTikets(Ind).TransDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
                Me.OrdersLineTikets(Ind).TransDateSpecified = True


                Me.ListBoxDebug.Items.Add(" ==> Venta de Tikets  : " & CType(Me.Db.mDbLector.Item("DESCRIPCION"), String).PadRight(40, " "))
                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1


            End While
            Me.Db.mDbLector.Close()


            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Llamada al Web Service Venta de Tikets " & Me.WebTikets.Url)
            Me.ListBoxDebug.Update()


            Elementos = Me.OrdersLineTikets.GetLength(Dimension)
            Me.TextBoxDebug.Text = Elementos



            ' LLAMADA AL WEB SERVICE

            '   MsgBox("Ojo la Query esta remeneada para que solo devuelva 10 Productos para pruebas", MsgBoxStyle.Information, "Aqui")

            If Elementos > 0 And IsNothing(Me.OrdersLineTikets(0)) = False Then
                Me.QweryOrdersTikets.SAT_NHJournalLossProfitTable = Me.OrdersLineTikets
                Me.WebTikets.createListSAT_JournalLossProfitQuery(Me.DocumentoContextoTikets, Me.QweryOrdersTikets)

                Me.ListBoxDebug.Items.Add("---------------------------------------------------------------------------------")

                Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Axapta Ok  " & Me.WebTikets.Url)
            Else
                Me.ListBoxDebug.Items.Add(Format(Now, "F") & " No Hay Elementos que enviar   " & Me.WebTikets.Url)
            End If


            'Application.DoEvents()


        Catch ex As Exception
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service Tikets " & Me.WebTikets.Url & " : " & ex.Message)

            MsgBox(ex.Message)
            '     MsgBox(ex.ToString)
        End Try
        '   Catch ex As Web.Services.Protocols.SoapException
        '       MsgBox(ex.Message)'

        'End Try



    End Sub
    Private Sub LeeParametros()
        Try
            SQL = "SELECT "
            SQL += "NVL(PARA_SOURCEENDPOINT,'?') AS PARA_SOURCEENDPOINT,"
            SQL += "NVL(PARA_DESTINATIONENDPOINT,'?') AS PARA_DESTINATIONENDPOINT,"
            SQL += "NVL(PARA_DOMAIN_NAME,'?') AS PARA_DOMAIN_NAME,"
            SQL += "NVL(PARA_DOMAIN_USER,'?') AS PARA_DOMAIN_USER,"
            SQL += "NVL(PARA_DOMAIN_PWD,'?') AS PARA_DOMAIN_PWD,"
            SQL += "NVL(PARA_BANCO_AX,'?') AS  PARA_BANCO_AX,"
            SQL += "NVL(PARA_SERV_CODI_BONO,'?') AS PARA_SERV_CODI_BONO,"
            SQL += "NVL(PARA_SERV_CODI_BONOASC,'?') AS PARA_SERV_CODI_BONOASC,"
            SQL += "NVL(PARA_WEBSERVICE_LOCATION,'?') AS PARA_WEBSERVICE_LOCATION, "
            SQL += "NVL(PARA_ANTICIPO_AX,'?') AS PARA_ANTICIPO_AX, "
            SQL += "NVL(PARA_CLIENTES_CONTADO_CIF,'?') AS PARA_CLIENTES_CONTADO_CIF, "
            SQL += "PARA_WEBSERVICE_TIMEOUT  AS PARA_WEBSERVICE_TIMEOUT , "

            SQL += "NVL(PARA_CIF_VENTA_NORMAL,'?') AS PARA_CIF_VENTA_NORMAL, "
            SQL += "NVL(PARA_CIF_VENTA_BONOS,'?') AS PARA_CIF_VENTA_BONOS, "
            SQL += "NVL(PARA_CIF_PRODUC_NORMAL,'?') AS PARA_CIF_PRODUC_NORMAL, "
            SQL += "NVL(PARA_CIF_PRODUC_BONOS,'?') AS PARA_CIF_PRODUC_BONOS,"
            SQL += "NVL(PARA_TRANSFERENCIA_AGENCIA,'?') AS PARA_TRANSFERENCIA_AGENCIA,"
            SQL += "NVL(PARA_CONECTA_NEWGOLF,0) AS PARA_CONECTA_NEWGOLF, "
            SQL += "NVL(PARA_AX_AJUSTABASE,0) AS PARA_AX_AJUSTABASE,"

            SQL += "NVL(PARA_HOTEL_AX,0) AS PARA_HOTEL_AX "


            SQL += " FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"



            Me.Db.TraerLector(SQL)
            Me.Db.mDbLector.Read()

            If Me.Db.mDbLector.HasRows Then
                Me.mAXSourceEndPoint = Me.Db.mDbLector.Item("PARA_SOURCEENDPOINT")
                Me.mAXDestinationEndPoint = Me.Db.mDbLector.Item("PARA_DESTINATIONENDPOINT")
                Me.mAxDomainName = Me.Db.mDbLector.Item("PARA_DOMAIN_NAME")
                Me.mAxUserName = Me.Db.mDbLector.Item("PARA_DOMAIN_USER")
                Me.mAxUserPwd = Me.Db.mDbLector.Item("PARA_DOMAIN_PWD")
                Me.mAxOffSetAccount = Me.Db.mDbLector.Item("PARA_BANCO_AX")
                Me.mAxServCodiBonos = Me.Db.mDbLector("PARA_SERV_CODI_BONO")
                Me.mAxServCodiBonosAsoc = Me.Db.mDbLector("PARA_SERV_CODI_BONOASC")
                Me.mAxUrl = Me.Db.mDbLector("PARA_WEBSERVICE_LOCATION")
                Me.mAxAnticipo = Me.Db.mDbLector("PARA_ANTICIPO_AX")
                Me.mParaCifContadoGenerico = Me.Db.mDbLector("PARA_CLIENTES_CONTADO_CIF")


                Me.mParaCifContadoVentaNormal = Me.Db.mDbLector("PARA_CIF_VENTA_NORMAL")
                Me.mParaCifContadoVentaBono = Me.Db.mDbLector("PARA_CIF_VENTA_BONOS")
                Me.mParaCifContadoProduccionNormal = Me.Db.mDbLector("PARA_CIF_PRODUC_NORMAL")
                Me.mParaCifContadoProduccionBono = Me.Db.mDbLector("PARA_CIF_PRODUC_BONOS")

                Me.mParaCifContadoProduccionBono = Me.Db.mDbLector("PARA_CIF_PRODUC_BONOS")

                Me.mParaConectaGolf = Me.Db.mDbLector("PARA_CONECTA_NEWGOLF")

                Me.mNWFormaCobroTransferenciaAgencia = Me.Db.mDbLector("PARA_TRANSFERENCIA_AGENCIA")

                Me.mAxAjustaBase = Me.Db.mDbLector("PARA_AX_AJUSTABASE")
                Me.ListBox1.Items.Add("Ajuste Base Imponible = " & Me.mAxAjustaBase)


                Me.mHotelId = Me.Db.mDbLector("PARA_HOTEL_AX")

                PARA_WEBSERVICE_TIMEOUT = Me.Db.mDbLector("PARA_WEBSERVICE_TIMEOUT")
                Me.NumericUpDownWebServiceTimeOut.Value = PARA_WEBSERVICE_TIMEOUT
                Me.Text += " en " & Me.mAxUrl
            Else
                Me.Db.mDbLector.Close()
            End If
        Catch ex As Exception
            If IsNothing(Me.Db.mDbLector) = False Then
                If Me.Db.mDbLector.IsClosed = False Then
                    Me.Db.mDbLector.Close()
                End If
            End If
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub MenuContextualPintar()
        Try

            ' se esta sobre un cobro
            If Me.DataGridViewAsientos.Item("ASNT_WEBSERVICE_NAME", Me.DataGridViewAsientos.CurrentRow.Index).Value = "SAT_NHJournalCustPaymentQueryService" Then
                ToolStripMenuItemCambiarEstado.Enabled = True
                ToolStripMenuItemCambiarEstado.Text = "Omitido " & Me.DataGridViewAsientos.Item("CONCEPTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & " Importe = " & Me.DataGridViewAsientos.Item("IMPORTE", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString

                ToolStripMenuItemCambiarEstadoEntodalaFactura.Enabled = True
                ToolStripMenuItemCambiarEstadoEntodalaFactura.Text = "Omitir Todos los Cobros del Documento " & Me.DataGridViewAsientos.Item("DOCUMENTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString


                ToolStripMenuItemCambiarEstado2.Enabled = True
                ToolStripMenuItemCambiarEstado2.Text = "Corregido " & Me.DataGridViewAsientos.Item("CONCEPTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & " Importe = " & Me.DataGridViewAsientos.Item("IMPORTE", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString

                ToolStripMenuItemCambiarEstado2EntodalaFactura.Enabled = True
                ToolStripMenuItemCambiarEstado2EntodalaFactura.Text = "Corregir Todos los Cobros del Documento " & Me.DataGridViewAsientos.Item("DOCUMENTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString


                ToolStripMenuItemCambiarEstado3.Enabled = False
                ToolStripMenuItemCambiarEstado3.Text = "Actualizar el Número de Nif "

                ToolStripMenuItemExluir.Enabled = True
                ToolStripMenuItemExluir.Text = "Excluir/Incluir "

                ToolStripMenuItemCambiarPrefijo.Enabled = True
                ToolStripMenuItemCambiarPrefijo.Text = "Cambiar Prefijo REC/NREC " & Me.DataGridViewAsientos.Item("CONCEPTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & " Importe = " & Me.DataGridViewAsientos.Item("IMPORTE", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString

                '


     
                UpdateImporteDelCobto001ToolStripMenuItem.Enabled = True
                UpdateImporteDelCobto001ToolStripMenuItem.Text = "Update Importe del Cobro +-  0.01 Céntimos " & Me.DataGridViewAsientos.Item("CONCEPTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & " Importe = " & Me.DataGridViewAsientos.Item("IMPORTE", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString




                ' se esta sobre una factura 
            ElseIf Me.DataGridViewAsientos.Item("ASNT_WEBSERVICE_NAME", Me.DataGridViewAsientos.CurrentRow.Index).Value = "SAT_NHCreateSalesOrdersQueryService" Then
                ToolStripMenuItemCambiarEstado.Enabled = False
                ToolStripMenuItemCambiarEstado.Text = "Omitido " & Me.DataGridViewAsientos.Item("CONCEPTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & " Importe = " & Me.DataGridViewAsientos.Item("IMPORTE", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString

                ToolStripMenuItemCambiarEstado2.Enabled = False
                ToolStripMenuItemCambiarEstado2.Text = "Corregido " & Me.DataGridViewAsientos.Item("CONCEPTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & " Importe = " & Me.DataGridViewAsientos.Item("IMPORTE", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString

                ToolStripMenuItemCambiarEstado3.Enabled = True
                ToolStripMenuItemCambiarEstado3.Text = "Actualizar el Número de Nif del Documento = " & Me.DataGridViewAsientos.Item("CONCEPTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString

                ToolStripMenuItemCambiarEstado4.Enabled = True
                ToolStripMenuItemCambiarEstado4.Text = "Marcar una Factura como ya Enviada Documento = " & Me.DataGridViewAsientos.Item("CONCEPTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString

                ToolStripMenuItemExluir.Enabled = False
                ToolStripMenuItemExluir.Text = "Excluir/Incluir "

                ToolStripMenuItemCambiarPrefijo.Enabled = False
                ToolStripMenuItemCambiarPrefijo.Text = "Cambiar Prefijo REC/NREC "

                UpdateImporteDelCobto001ToolStripMenuItem.Enabled = False
                UpdateImporteDelCobto001ToolStripMenuItem.Text = "Update Importe del Cobro +-  0.01 Céntimos "


                ' se esta sobre un Anticipo
            ElseIf Me.DataGridViewAsientos.Item("ASNT_WEBSERVICE_NAME", Me.DataGridViewAsientos.CurrentRow.Index).Value = "SAT_NHPrePaymentService" Then
                ToolStripMenuItemCambiarEstado.Enabled = True
                ToolStripMenuItemCambiarEstado.Text = "Omitido " & Me.DataGridViewAsientos.Item("CONCEPTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & " Importe = " & Me.DataGridViewAsientos.Item("IMPORTE", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString

                ToolStripMenuItemCambiarEstadoEntodalaFactura.Enabled = False
                ToolStripMenuItemCambiarEstadoEntodalaFactura.Text = "_"


                ToolStripMenuItemCambiarEstado2.Enabled = True
                ToolStripMenuItemCambiarEstado2.Text = "Corregido " & Me.DataGridViewAsientos.Item("CONCEPTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & " Importe = " & Me.DataGridViewAsientos.Item("IMPORTE", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString

                ToolStripMenuItemCambiarEstado2EntodalaFactura.Enabled = False
                ToolStripMenuItemCambiarEstado2EntodalaFactura.Text = "_"


                ToolStripMenuItemCambiarEstado3.Enabled = False
                ToolStripMenuItemCambiarEstado3.Text = "Actualizar el Número de Nif "

                ToolStripMenuItemExluir.Enabled = False
                ToolStripMenuItemExluir.Text = "Excluir/Incluir "

                ToolStripMenuItemCambiarPrefijo.Enabled = True
                ToolStripMenuItemCambiarPrefijo.Text = "Cambiar Prefijo REC/NREC " & Me.DataGridViewAsientos.Item("CONCEPTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & " Importe = " & Me.DataGridViewAsientos.Item("IMPORTE", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString


  
                UpdateImporteDelCobto001ToolStripMenuItem.Enabled = False
                UpdateImporteDelCobto001ToolStripMenuItem.Text = "Update Importe del Cobro +-  0.01 Céntimos "


            Else
                ToolStripMenuItemCambiarEstado.Text = "Omitido"
                ToolStripMenuItemCambiarEstado.Enabled = False
                ToolStripMenuItemCambiarEstado2.Text = "Corregido"
                ToolStripMenuItemCambiarEstado2.Enabled = False
                ToolStripMenuItemCambiarEstado3.Text = "Actualizar el Número de Nif "
                ToolStripMenuItemCambiarEstado3.Enabled = False

                ToolStripMenuItemCambiarEstado4.Text = "Actualizar el Número de Nif "
                ToolStripMenuItemCambiarEstado4.Enabled = False

                ToolStripMenuItemCambiarEstado4.Text = "Marcar una Factura como ya Enviada "
                ToolStripMenuItemCambiarEstado4.Enabled = False

                ToolStripMenuItemExluir.Enabled = False
                ToolStripMenuItemExluir.Text = "Excluir/Incluir "

                ToolStripMenuItemCambiarPrefijo.Enabled = False
                ToolStripMenuItemCambiarPrefijo.Text = "Cambiar Prefijo REC/NREC "


                UpdateImporteDelCobto001ToolStripMenuItem.Enabled = False
                UpdateImporteDelCobto001ToolStripMenuItem.Text = "Update Importe del Cobro +-  0.01 Céntimos "


            End If


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
#End Region

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try



            If Me.AuditaNif = True Then
                Exit Sub
            End If



            Me.TotalErrores = 0

            ' EVITA PULSAR EL BOTON DOS VECES
            Me.ButtonAceptar.Visible = False
            Me.ButtonAceptar.Update()


            Me.mResultStr = ""

            ' Control de que no se este Generando la fecha  que se pide generar 
            SQL = "SELECT COUNT(*) FROM TG_CONTROL WHERE CONTROL_DATA = '" & Me.mFecha & "'"
            SQL += " AND CONTROL_TIPO = 'NewHotel' "
            SQL += " AND CONTROL_STATUS = 'Generando'"

            If CType(Me.Db.EjecutaSqlScalar(SQL), Integer) > 0 Then
                SQL = "SELECT NVL(CONTROL_OBSE,'?') FROM TG_CONTROL WHERE CONTROL_DATA = '" & Me.mFecha & "'"
                SQL += " AND CONTROL_TIPO = 'NewHotel' "
                SQL += " AND CONTROL_STATUS = 'Generando'"
                Me.mResultStr = Me.Db.EjecutaSqlScalar(SQL)
                MsgBox("Se esta  Generando esta Fecha ,  en otra Máquina o Sesión" & vbCrLf & vbCrLf & Me.mResultStr, MsgBoxStyle.Information, "Atención")
                Exit Sub

            End If

            ' Control de que no se este Enviando la fecha  que se pide generar 
            SQL = "SELECT COUNT(*) FROM TG_CONTROL WHERE CONTROL_DATA = '" & Me.mFecha & "'"
            SQL += " AND CONTROL_TIPO = 'NewHotel' "
            SQL += " AND CONTROL_STATUS = 'Enviando'"

            If CType(Me.Db.EjecutaSqlScalar(SQL), Integer) > 0 Then

                SQL = "SELECT NVL(CONTROL_OBSE,'?') FROM TG_CONTROL WHERE CONTROL_DATA = '" & Me.mFecha & "'"
                SQL += " AND CONTROL_TIPO = 'NewHotel' "
                SQL += " AND CONTROL_STATUS = 'Enviando'"
                Me.mResultStr = Me.Db.EjecutaSqlScalar(SQL)


                MsgBox("Se esta  Enviando esta Fecha ,  en otra Máquina o Sesión" & vbCrLf & vbCrLf & Me.mResultStr, MsgBoxStyle.Information, "Atención")
                Exit Sub

            End If


            Me.mProcid = Guid.NewGuid().ToString()

            ' SE MARCA EL PROCESO COMO SIENDO ENVIADO TABLA TG_CONTROL
            SQL = "INSERT INTO TG_CONTROL (CONTROL_DATA, CONTROL_ID, CONTROL_TIPO,CONTROL_STATUS, CONTROL_OBSE)VALUES('"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "','"
            SQL += Me.mProcid & "','NewHotel','Enviando','"
            SQL += System.Environment.UserName & " de " & System.Environment.UserDomainName & " desde " & System.Environment.MachineName & " " & Now
            SQL += "')"


            Me.Db.EjecutaSqlCommit(SQL)

            If Me.Db.StrError <> "" Then
                MsgBox(Me.Db.StrError)
                Exit Sub
            End If




            If Me.CheckBoxEnviaProduccion.Checked Then
                Me.Cursor = Cursors.AppStarting
                Me.mInicio = Now
                Me.ListBoxDebug.Items.Add("_________________________________________________________________________________")
                Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Inicio de Envio de Producción")
                Me.ListBoxDebug.Items.Add("_________________________________________________________________________________")

                Me.AXProcesarProduccion()

                Me.mDuracion = DateDiff(DateInterval.Second, Me.mInicio, Now) / (24 * 3600)
                Me.ListBoxDebug.Items.Add(Format(Date.FromOADate(Me.mDuracion), "mm 'minutos ' ss ' segundos'"))

            End If





            If Me.CheckBoxEnviaAnticipos.Checked Then
                Me.Cursor = Cursors.AppStarting
                Me.mInicio = Now
                Me.ListBoxDebug.Items.Add("_________________________________________________________________________________")
                Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Inicio de Envio de Anticipos")
                Me.ListBoxDebug.Items.Add("_________________________________________________________________________________")

                Me.AxProcesarAnticipos()

                Me.mDuracion = DateDiff(DateInterval.Second, Me.mInicio, Now) / (24 * 3600)
                Me.ListBoxDebug.Items.Add(Format(Date.FromOADate(Me.mDuracion), "mm 'minutos ' ss ' segundos'"))

            End If




            If Me.CheckBoxEnviaFacturacion.Checked Then
                Me.Cursor = Cursors.AppStarting
                Me.mInicio = Now
                Me.ListBoxDebug.Items.Add("_________________________________________________________________________________")
                Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Inicio de Envio de Facturación")
                Me.ListBoxDebug.Items.Add("_________________________________________________________________________________")

                Me.AxProcesarFacturacionLineaALinea()

                Me.mDuracion = DateDiff(DateInterval.Second, Me.mInicio, Now) / (24 * 3600)
                Me.ListBoxDebug.Items.Add(Format(Date.FromOADate(Me.mDuracion), "mm 'minutos ' ss ' segundos'"))


            End If




            If Me.CheckBoxEnviaCobros.Checked Then
                Me.Cursor = Cursors.AppStarting
                Me.mInicio = Now
                Me.ListBoxDebug.Items.Add("_________________________________________________________________________________")
                Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Inicio de Envio de Cobros")
                Me.ListBoxDebug.Items.Add("_________________________________________________________________________________")

                If Me.CheckBoxDebugCobros.Checked Then
                    ' PAT 35
                    Me.AxProcesarCobrosParciales()
                    Me.AxProcesarCobrosParciales2()
                Else
                    Me.AxProcesarCobros()
                End If

                Me.mDuracion = DateDiff(DateInterval.Second, Me.mInicio, Now) / (24 * 3600)
                Me.ListBoxDebug.Items.Add(Format(Date.FromOADate(Me.mDuracion), "mm 'minutos ' ss ' segundos'"))

            End If






            If Me.CheckBoxEnviaCobros.Checked Then
                Me.Cursor = Cursors.AppStarting
                Me.mInicio = Now
                Me.AxProcesarCobrosAnticipos()
                Me.mDuracion = DateDiff(DateInterval.Second, Me.mInicio, Now) / (24 * 3600)
                Me.ListBoxDebug.Items.Add(Format(Date.FromOADate(Me.mDuracion), "mm 'minutos ' ss ' segundos'"))
            End If






            If Me.CheckBoxEnviaInventario.Checked Then
                Me.mInicio = Now
                Me.Cursor = Cursors.AppStarting
                Me.ListBoxDebug.Items.Add("_________________________________________________________________________________")
                Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Inicio de Envio de Tikets")
                Me.ListBoxDebug.Items.Add("_________________________________________________________________________________")

                Me.AXProcesarVentaTikets()

                Me.mDuracion = DateDiff(DateInterval.Second, Me.mInicio, Now) / (24 * 3600)

                Me.ListBoxDebug.Items.Add(Format(Date.FromOADate(Me.mDuracion), "mm 'minutos ' ss ' segundos'"))

            End If



            If Me.CheckBoxEstadistica.Checked Then
                Me.Cursor = Cursors.AppStarting
                Me.ListBoxDebug.Items.Add("_________________________________________________________________________________")
                Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Inicio de Envio de Estadisticas")
                Me.ListBoxDebug.Items.Add("_________________________________________________________________________________")
                Me.mInicio = Now

                Me.ProcesarEstadisticas()

                Me.mDuracion = DateDiff(DateInterval.Second, Me.mInicio, Now) / (24 * 3600)
                Me.ListBoxDebug.Items.Add(Format(Date.FromOADate(Me.mDuracion), "mm 'minutos ' ss ' segundos'"))

            End If


            Me.MostrarAsientos()

            Me.Cursor = Cursors.Default

            If Me.TotalErrores > 0 Then
                MsgBox("Atención existen Incidencias , Revise las Excepciones en Axapta y Reenvie ", MsgBoxStyle.Exclamation, "Atención")
            End If



            SQL = "DELETE  TG_CONTROL WHERE CONTROL_DATA = '"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND CONTROL_TIPO = 'NewHotel'"

            Me.Db.EjecutaSqlCommit(SQL)


        Catch ex As Exception
            SQL = "DELETE  TG_CONTROL WHERE CONTROL_DATA = '"
            SQL += Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND CONTROL_TIPO = 'NewHotel'"

            Me.Db.EjecutaSqlCommit(SQL)
            Me.Cursor = Cursors.Default
        Finally



            Me.ButtonAceptar.Visible = True
            Me.ButtonAceptar.Update()
        End Try
    End Sub



    Private Sub ButtonImPrimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImPrimir.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            REPORT_SELECTION_FORMULA = "{TH_ASNT.ASNT_F_VALOR}=DATETIME(" & Format(Me.mFecha, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"
            REPORT_SELECTION_FORMULA += " AND  {@MyAxapta}= 'AX'"


            'Dim Form As New FormVisorCrystal("ASIENTO AXAPTA.RPT", "", REPORT_SELECTION_FORMULA, Me.mStrConexion, "", False, False)
            Dim Form As New FormVisorCrystal("ASIENTO AXAPTA3.RPT", "", REPORT_SELECTION_FORMULA, Me.mStrConexion, "", False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim service As New WebReferenceClientes.SAT_NHCreateCustQueryService
            Dim docCon As New WebReferenceClientes.DocumentContext

            docCon.MessageId = Guid.NewGuid().ToString()


            docCon.SourceEndpoint = Me.mAXSourceEndPoint
            docCon.DestinationEndpoint = Me.mAXDestinationEndPoint

            service.Credentials = New System.Net.NetworkCredential(Me.mAxUserName, Me.mAxUserPwd, Me.mAxDomainName)

            service.Url = Me.mAxUrl & "SAT_NHCreateCustQueryService.asmx"

            docCon.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)

            Dim custLine As New WebReferenceClientes.AxdEntity_SAT_NHCreateCustTable_1()

            custLine.CustAccount = "NH_0012"

            custLine.CustName = "PEPE PEREZ"

            custLine.ZipCode = "35002"



            'custLine.TaxCod

            ' OK 
            '   custLine.VATNum = "54076155G"
            ' KO
            custLine.VATNum = "01020304K"


            Dim axdSAT As New WebReferenceClientes.AxdSAT_NHCreateCustQuery

            axdSAT.SAT_NHCreateCustTable_1 = New WebReferenceClientes.AxdEntity_SAT_NHCreateCustTable_1() {custLine}


            Me.ListBoxDebug.Items.Add(" ==>> " & custLine.CustName)

            Me.Cursor = Cursors.WaitCursor
            service.createListSAT_NHCreateCustQuery(docCon, axdSAT)
            Me.Cursor = Cursors.Default

            Me.ListBoxDebug.Items.Add("Cliente Enviado OK")


            'Catch ex As Exception

            '   Me.Cursor = Cursors.Default
            '   Me.ListBoxDebug.Items.Add(ex.Message)
            '   MsgBox(ex.Message)
            'End Try


            'End Try
        Catch ex As Exception
            Cursor = Cursors.Default
            Dim s As New System.Web.Services.Protocols.SoapException

            MsgBox(ex.Message & vbCrLf & s.Message & vbCrLf & s.ToString, MsgBoxStyle.Information, s.TargetSite)
        End Try





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
                        e.CellStyle.ForeColor = Color.White
                    ElseIf e.Value.ToString = "Omitido" Then
                        e.CellStyle.BackColor = Color.Orange
                        e.CellStyle.ForeColor = Color.Black
                    ElseIf e.Value.ToString = "Corregido" Then
                        e.CellStyle.BackColor = Color.LightBlue
                        e.CellStyle.ForeColor = Color.Black
                    End If
                End If
            End If





            '    SQL += " DECODE(ASNT_AX_STATUS,0,'No Enviado',1,'Ok Enviado','8','Excluido por Usuario','9','Excluido','?')"


            ' COLOR DE FONDO DE LA LINEA
            With Me.DataGridViewAsientos.Rows(e.RowIndex)

                If e.Value.ToString = "Omitido" Then
                    '  .DefaultCellStyle.BackColor = Color.LightGray
                    .DefaultCellStyle.BackColor = Color.Orange
                    .DefaultCellStyle.ForeColor = Color.Black
                End If
                If e.Value.ToString = "Corregido" Then
                    '  .DefaultCellStyle.BackColor = Color.LightGray
                    .DefaultCellStyle.BackColor = Color.LightBlue
                    .DefaultCellStyle.ForeColor = Color.Black
                End If

            End With

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try

            ' PRODUCCION NORMAL
            SQL = "UPDATE TH_ASNT SET ASNT_DPTO_CODI = 'P01'"
            SQL += ", ASNT_PROD_ID = 'P01'"
            SQL += " WHERE ASNT_WEBSERVICE_NAME = 'SAT_NHCreateProdOrdersQueryService'"
            SQL += " AND  ASNT_TIPO_PROD = 'NORMAL'"
            '     Me.DbWrite.EjecutaSqlCommit(SQL)

            ' PRODUCCION BONOS
            SQL = "UPDATE TH_ASNT SET ASNT_DPTO_CODI = 'G1C'"
            SQL += ", ASNT_PROD_ID = 'G1C'"
            SQL += " WHERE ASNT_WEBSERVICE_NAME = 'SAT_NHCreateProdOrdersQueryService'"
            SQL += " AND  ASNT_TIPO_PROD = 'BONOS'"
            '    Me.DbWrite.EjecutaSqlCommit(SQL)


            ' TODOS LOS ARTICULOS FACTURADOS QUE NO SON BONOS DEL TIPO IGIC 5% SON A02
            SQL = "UPDATE TH_ASNT SET "
            SQL += "  ASNT_DPTO_CODI = 'A02'"
            SQL += ", ASNT_PROD_ID = 'A02'"
            SQL += " WHERE ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"
            SQL += " AND ASNT_LIN_TIIMP = 5"
            SQL += "AND ASNT_DPTO_CODI NOT IN('G1C','G3A')"
            '   Me.DbWrite.EjecutaSqlCommit(SQL)

            ' TODOS LOS ARTICULOS FACTURADOS QUE NO SON BONOS DEL TIPO IGIC 2% SON A04 PDTE MARTA
            SQL = "UPDATE TH_ASNT SET "
            SQL += "  ASNT_DPTO_CODI = 'A04'"
            SQL += ", ASNT_PROD_ID = 'A04'"
            SQL += " WHERE ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"
            SQL += " AND ASNT_LIN_TIIMP = 2"
            SQL += "AND ASNT_DPTO_CODI NOT IN('G1C','G3A')"
            '  Me.DbWrite.EjecutaSqlCommit(SQL)

            '  FACTURAS CLIENTE NORMAL
            SQL = "UPDATE TH_ASNT SET ASNT_NHCUSTACCOUNT = 'ENTI0064',"
            SQL += "  ASNT_CIF  = 'B38620464' "
            SQL += " WHERE ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"
            SQL += " AND ASNT_TIPOFACTURA_AX < 3 "
            Me.DbWrite.EjecutaSqlCommit(SQL)



            '  FACTURAS CLIENTE CONTADO
            SQL = "UPDATE TH_ASNT SET "
            SQL += "  ASNT_CIF  = '00000000F'"
            SQL += " WHERE ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"
            SQL += " AND ASNT_TIPOFACTURA_AX = 3 "
            '        Me.DbWrite.EjecutaSqlCommit(SQL)


            ' TODOS LOS ARTICULOS FACTURADOS QUE SI SON BONOS DEL TIPO IGIC 0% SON G1C PDTE MARTA
            SQL = "UPDATE TH_ASNT SET "
            SQL += "  ASNT_DPTO_CODI = 'P0%'"
            SQL += ", ASNT_PROD_ID = 'P0%'"
            SQL += " WHERE ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"
            SQL += " AND ASNT_LIN_TIIMP = 0"
            SQL += "AND (ASNT_TIPO_VENTA = 'BONOS' OR  ASNT_NOMBRE = 'BONOS')"
            '        Me.DbWrite.EjecutaSqlCommit(SQL)

            ' TODOS LOS ARTICULOS FACTURADOS QUE SI SON BONOS DEL TIPO IGIC 2% SON G1C PDTE MARTA
            SQL = "UPDATE TH_ASNT SET "
            SQL += "  ASNT_DPTO_CODI = 'P2%'"
            SQL += ", ASNT_PROD_ID = 'P2%'"
            SQL += " WHERE ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"
            SQL += " AND ASNT_LIN_TIIMP = 2"
            SQL += "AND (ASNT_TIPO_VENTA = 'BONOS' OR  ASNT_NOMBRE = 'BONOS')"
            '       Me.DbWrite.EjecutaSqlCommit(SQL)


            ' TODOS LOS ARTICULOS FACTURADOS QUE SI SON BONOS DEL TIPO IGIC 5% SON G1C PDTE MARTA
            SQL = "UPDATE TH_ASNT SET "
            SQL += "  ASNT_DPTO_CODI = 'P5%'"
            SQL += ", ASNT_PROD_ID = 'P5%'"
            SQL += " WHERE ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"
            SQL += " AND ASNT_LIN_TIIMP = 5"
            SQL += "AND (ASNT_TIPO_VENTA = 'BONOS' OR  ASNT_NOMBRE = 'BONOS')"
            '      Me.DbWrite.EjecutaSqlCommit(SQL)



            '  COBROS VISA 
            SQL = "UPDATE TH_ASNT SET "
            SQL += "  ASNT_CFCTA_COD  = 'VISA'"
            SQL += " WHERE ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService'"
            SQL += " AND ASNT_CACR_CODI IS NOT NULL "
            '     Me.DbWrite.EjecutaSqlCommit(SQL)

            '  COBROS CONTADO
            SQL = "UPDATE TH_ASNT SET "
            SQL += "  ASNT_CFCTA_COD  = 'EF'"
            SQL += " WHERE ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService'"
            SQL += " AND ASNT_FORE_CODI IS NOT NULL "
            '    Me.DbWrite.EjecutaSqlCommit(SQL)



            '  TIKETS
            SQL = "UPDATE TH_ASNT SET "
            SQL += "  ASNT_PROD_ID  = 'NH0001',"
            'SQL += "  ASNT_PROD_ID  = 'GEN-ART025144',"
            SQL += "  ASNT_PROD_TALLA  = 'S',"
            SQL += "  ASNT_PROD_COLOR  = 'AZUL',"
            SQL += "  ASNT_ALMA_AX  = 'TIENDA NH' "
            SQL += " WHERE ASNT_WEBSERVICE_NAME = 'SAT_JournalLossProfitQueryService'"
            '   Me.DbWrite.EjecutaSqlCommit(SQL)



            ' ANTICIPOS TODOS AL NIF 42853743M
            SQL = "UPDATE TH_ASNT SET "
            SQL += "  ASNT_CIF = '42853743M'"
            SQL += " WHERE ASNT_WEBSERVICE_NAME = 'SAT_NHPrePaymentService'"
            '  Me.DbWrite.EjecutaSqlCommit(SQL)

            Me.MostrarAsientos()

        Catch ex As Exception

        End Try
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Me.ListBoxDebug.Items.Clear()
            Me.ListBoxDebug.Update()

            Me.ListBoxAjustesFacturas.Items.Clear()
            Me.ListBoxAjustesFacturas.Update()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub CheckBoxSoloPendientes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxSoloPendientes.CheckedChanged
        Try

            If Me.CheckBoxSoloPendientes.Checked = True Then
                Me.CheckBoxExcluidos.Checked = False
                Me.CheckBoxExcluidos.Update()
                Me.CheckBoxVerTodos.Checked = False
                Me.CheckBoxVerTodos.Update()
                Me.CheckBoxBoockId.Checked = False
                Me.CheckBoxBoockId.Update()

            End If

            Me.MostrarAsientos()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Function AuditaNif() As Boolean
        Try

            Dim F As New FormAuditaNif
            Dim HayProblemas As Boolean = False
            Dim YaestaCarcado As Boolean = False

            'SQL = "SELECT DISTINCT DECODE(ASNT_WEBSERVICE_NAME,'SAT_NHPrePaymentService','Pago a Cuenta','SAT_NHCreateSalesOrdersQueryService','Factura','?') AS TIPO ,ASNT_LIN_DOCU, "
            'SQL += " NVL(ASNT_CFCTA_COD,' ') AS CUENTA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,NVL(ASNT_I_MONEMP,'0')  AS IMPORTE"
            'SQL += " , ASNT_AUXILIAR_STRING AS TIPO "
            'SQL += " , ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI,ASNT_ENTI_CODI,ASNT_TIPO_CLIENTE_ANTICIPO,NVL(ASNT_FORE_CODI_AX,'?') AS FORMA,NVL(ASNT_CIF ,'?') AS CIF "
            'SQL += " FROM TH_ASNT "
            'SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            'SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            'SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            'SQL += " AND (ASNT_WEBSERVICE_NAME = 'SAT_NHPrePaymentService'"
            'SQL += " OR  ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService')"
            ' SOLO SIN PROCESAR
            'SQL += " AND ASNT_AX_STATUS =  0 "

            'SQL += " ORDER BY ASNT_RESE_CODI ASC "

            SQL = "SELECT DISTINCT DECODE(ASNT_WEBSERVICE_NAME,'SAT_NHPrePaymentService','Pago a Cuenta','SAT_NHCreateSalesOrdersQueryService','Factura','?') AS TIPO ,NVL(ASNT_LIN_DOCU,'?') AS ASNT_LIN_DOCU, "
            SQL += " NVL(ASNT_CFCTA_COD,' ') AS CUENTA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO"
            SQL += " , ASNT_AUXILIAR_STRING AS TIPO1 "
            SQL += " ,NVL(ASNT_CIF ,'?') AS CIF "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND (ASNT_WEBSERVICE_NAME = 'SAT_NHPrePaymentService'"
            SQL += " OR  ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService')"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "



            Me.Db.TraerLector(SQL)



            While Me.Db.mDbLector.Read
                If Me.Db.mDbLector.Item("CIF") = "?" Or Me.Db.mDbLector.Item("CIF") = "0" Then
                    HayProblemas = True

                    If YaestaCarcado = False Then
                        YaestaCarcado = True
                        '   Me.AddOwnedForm(F)
                        '   F.Show()
                    End If

                    '  F.TextBoxTipo.Text = CStr(Me.Db.mDbLector.Item("TIPO"))
                    '  F.TextBoxDocumento.Text = CStr(Me.Db.mDbLector.Item("ASNT_LIN_DOCU"))
                    '  F.TextBoxConcepto.Text = CStr(Me.Db.mDbLector.Item("CONCEPTO"))


                    If MessageBox.Show(Me.Db.mDbLector.Item("TIPO") & vbCrLf & "Existen Nifs / Dnis Nulos , se cancela Todo el Proceso de Envío" & vbCrLf & Me.Db.mDbLector.Item("CONCEPTO"), "Atención", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Cancel Then
                        '       F.Close()
                        Exit While
                    End If
                End If
            End While
            Me.Db.mDbLector.Close()

            '    F.Close()



            Return HayProblemas


        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        Finally
            Me.Db.mDbLector.Close()
        End Try
    End Function
    Private Sub ActualizaNif()
        Try

            Dim F As New FormAuditaNif
            Dim HayProblemas As Boolean = False
            Dim YaestaCarcado As Boolean = False

            Dim Factura(1) As String

            Dim Dni As String




            SQL = "SELECT DISTINCT DECODE(ASNT_WEBSERVICE_NAME,'SAT_NHPrePaymentService','Pago a Cuenta','SAT_NHCreateSalesOrdersQueryService','Factura','?') AS TIPO ,NVL(ASNT_LIN_DOCU,'?') AS ASNT_LIN_DOCU, "
            SQL += " NVL(ASNT_CFCTA_COD,' ') AS CUENTA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO"
            SQL += " , ASNT_AUXILIAR_STRING AS TIPO1 "
            SQL += " ,NVL(ASNT_CIF ,'?') AS CIF "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            ' SQL += " AND (ASNT_WEBSERVICE_NAME = 'SAT_NHPrePaymentService'"
            'SQL += " OR  ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService')"

            ' SOLO FACTURAS 
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService' "

            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "



            Me.Db.TraerLector(SQL)


            Me.NEWHOTEL = New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexion, Me.mEmpGrupoCod, Me.mEmpCod)

            If Me.mParaConectaGolf = 1 Then
                Me.NEWGOLF = New NewGolf.NewGolfData(Me.mStrConexionHotel, Me.mStrConexion, Me.mEmpGrupoCod, Me.mEmpCod)
            End If

            While Me.Db.mDbLector.Read

                Factura = Split(Me.Db.mDbLector.Item("ASNT_LIN_DOCU"), "/")

                Dni = Me.NEWHOTEL.DevuelveDniCifContabledeFactura(CInt(Factura(0)), CStr(Factura(1)))

                ' SI LA FACTURA ES DE NEWGOLF
                If Dni = "0" And Me.mParaConectaGolf = 1 Then
                    Dni = Me.NEWGOLF.DevuelveDniCifContabledeFactura(CInt(Factura(0)), CStr(Factura(1)))
                End If


                If Dni <> Me.Db.mDbLector.Item("CIF") Then
                    SQL = " UPDATE TH_ASNT SET ASNT_CIF = '" & Dni & "'"
                    SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
                    SQL += " AND ASNT_CIF = '" & Me.Db.mDbLector.Item("CIF") & "'"
                    SQL += " AND ASNT_AX_STATUS =  0 "

                    Me.DbWrite.EjecutaSqlCommit(SQL)

                End If


            End While
            Me.Db.mDbLector.Close()

            Me.MostrarAsientos()





        Catch ex As Exception
            MsgBox(ex.Message)

        Finally
            Me.Db.mDbLector.Close()
            Me.NEWHOTEL.CerrarConexiones()

            If Me.mParaConectaGolf = 1 Then
                Me.NEWGOLF.CerrarConexiones()
            End If


        End Try
    End Sub


#Region "TEST"
    Private Sub EnvioFacturacionTest()
        Try

            '    Dim Ind As Integer

            Dim Primerregistro As Boolean = False


            Me.WebFacturas = New WebReferenceFacturas.SAT_NHCreateSalesOrdersQueryService

            'Actualiza Url

            Me.WebFacturas.Url = Me.mAxUrl & "SAT_NHCreateSalesOrdersQueryService.asmx"



            Me.TextBoxRutaWebServices.Text = Me.WebFacturas.Url
            Me.TextBoxRutaWebServices.Update()

            Me.DocumentoContextoFacturas = New WebReferenceFacturas.DocumentContext
            Me.QweryOrdersFacturas = New WebReferenceFacturas.AxdSAT_NHCreateSalesOrdersQuery


            'Me.WebFacturas.Credentials = System.Net.CredentialCache.DefaultCredentials
            ' Me.WebFacturas.Credentials = New System.Net.NetworkCredential("Qwe1", "Rty5412", "ABCONSULTORESLP")
            '

            Me.WebFacturas.Credentials = New System.Net.NetworkCredential(Me.mAxUserName, Me.mAxUserPwd, Me.mAxDomainName)




            Me.DocumentoContextoFacturas.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContextoFacturas.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContextoFacturas.DestinationEndpoint = Me.mAXDestinationEndPoint
            'Me.DocumentoContexto.SourceEndpointUser = Environment.ExpandEnvironmentVariables("%userdomain%\\%username%")
            Me.DocumentoContextoFacturas.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)


            ' CABECERA DE FACTURA



            Me.OrdersTablaFacturas(0) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersTable_1

            Me.OrdersTablaFacturas(0).DocumentDate = "27/05/2009"
            Me.OrdersTablaFacturas(0).DocumentDateSpecified = True
            Me.OrdersTablaFacturas(0).FixedDueDate = "27/05/2009"
            Me.OrdersTablaFacturas(0).FixedDueDateSpecified = True
            Me.OrdersTablaFacturas(0).InvoiceDate = "27/05/2009"
            ' Me.OrdersTablaFacturas(0).InvoiceDateSpecified = True


            Me.OrdersTablaFacturas(0).InvoiceId = "7/2009"
            Me.OrdersTablaFacturas(0).SalesAmount = 962.22
            Me.OrdersTablaFacturas(0).SalesAmountSpecified = True



            '  ' bond = si es un bono , bond = no es un servicio normal 
            Me.OrdersTablaFacturas(0).Bond = WebReferenceFacturas.AxdExtType_NoYesId.No
            Me.OrdersTablaFacturas(0).BondSpecified = True

            ' tipo cliente 
            Me.OrdersTablaFacturas(0).ContClient = WebReferenceFacturas.AxdExtType_NoYesId.No
            Me.OrdersTablaFacturas(0).ContClientSpecified = True

            'Me.OrdersTablaFacturas(0).CustAccount = "000050"
            Me.OrdersTablaFacturas(0).VATNum = "18593507P"
            Me.OrdersTablaFacturas(0).NHCustAccount = "NH0001"





            Me.ListBoxDebug.Items.Add(" => MessageId            : " & Me.DocumentoContextoFacturas.MessageId.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpoint       : " & Me.DocumentoContextoFacturas.SourceEndpoint.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpointUser   : " & Me.DocumentoContextoFacturas.SourceEndpointUser.ToString)
            Me.ListBoxDebug.Items.Add(" => DestinationEndpoint  : " & Me.DocumentoContextoFacturas.DestinationEndpoint.ToString)
            Me.ListBoxDebug.Items.Add(" => Bond                 : " & OrdersTablaFacturas(0).Bond.ToString)
            Me.ListBoxDebug.Items.Add(" => Cliente Contado      : " & OrdersTablaFacturas(0).ContClient.ToString)
            Me.ListBoxDebug.Items.Add(" => InvoiceDAte          : " & OrdersTablaFacturas(0).InvoiceDate.ToString)
            Me.ListBoxDebug.Items.Add(" => Cliente              : " & Me.OrdersTablaFacturas(0).CustAccount)
            Me.ListBoxDebug.Items.Add(" => Total                : " & Me.OrdersTablaFacturas(0).SalesAmount.ToString)




            ' INICIALIZA EL ARRAY 
            Ind = 0
            ReDim Me.OrdersLineFacturas(Ind)
            ReDim Me.OrdersLineTaxFacturas(Ind)

            Me.OrdersLineFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersLine_1
            Me.OrdersLineTaxFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersTaxLine_1



            ' servicio 

            'Me.OrdersLineFacturas(Ind).ItemId = "SART-001000"
            Me.OrdersLineFacturas(Ind).ItemId = "A02"
            Me.OrdersLineFacturas(Ind).SalesPrice = 458.2
            Me.OrdersLineFacturas(Ind).SalesPriceSpecified = True
            Me.OrdersLineFacturas(Ind).Qty = 1
            Me.OrdersLineFacturas(Ind).QtySpecified = True




            ' IMPUESTO
            'Me.OrdersLineTaxFacturas(Ind).TaxAmount = CType(Me.DbAux.mDbLector.Item("VALORIMPUESTO"), Decimal)
            Me.OrdersLineTaxFacturas(Ind).TaxAmount = 22.91
            Me.OrdersLineTaxFacturas(Ind).TaxAmountSpecified = True

            'Me.OrdersLineTaxFacturas(Ind).TaxBase = CType(Me.DbAux.mDbLector.Item("VALORLIQUIDO"), Decimal)
            Me.OrdersLineTaxFacturas(Ind).TaxBase = 458.2
            Me.OrdersLineTaxFacturas(Ind).TaxBaseSpecified = True


            Me.OrdersLineTaxFacturas(Ind).TaxCode = "5%"
            Me.OrdersLineTaxFacturas(Ind).TaxPercent = 5
            Me.OrdersLineTaxFacturas(Ind).TaxPercentSpecified = True




            '****

            '---  test otra linea 

            ReDim Preserve Me.OrdersLineFacturas(UBound(Me.OrdersLineFacturas) + 1)
            ReDim Preserve Me.OrdersLineTaxFacturas(UBound(Me.OrdersLineTaxFacturas) + 1)

            Ind = UBound(Me.OrdersLineFacturas)


            Me.OrdersLineFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersLine_1
            Me.OrdersLineTaxFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersTaxLine_1

            Me.OrdersLineFacturas(Ind).ItemId = "A02"
            Me.OrdersLineFacturas(Ind).SalesPrice = 458.2
            Me.OrdersLineFacturas(Ind).SalesPriceSpecified = True
            Me.OrdersLineFacturas(Ind).Qty = 1
            Me.OrdersLineFacturas(Ind).QtySpecified = True


            'IMPUESTO()
            Me.OrdersLineTaxFacturas(Ind).TaxAmount = 22.91
            Me.OrdersLineTaxFacturas(Ind).TaxAmountSpecified = True

            Me.OrdersLineTaxFacturas(Ind).TaxBase = 458.2
            Me.OrdersLineTaxFacturas(Ind).TaxBaseSpecified = True


            Me.OrdersLineTaxFacturas(Ind).TaxCode = "5%"
            Me.OrdersLineTaxFacturas(Ind).TaxPercent = 5
            Me.OrdersLineTaxFacturas(Ind).TaxPercentSpecified = True


            Me.ListBoxDebug.Items.Add(" ==> Lineas de Fac  : " & Me.OrdersLineTaxFacturas(Ind).TaxBase.ToString)



            Me.ListBoxDebug.Items.Add("")
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Llamada al Web Service " & Me.WebFacturas.Url)
            Me.ListBoxDebug.Items.Add("")
            Me.ListBoxDebug.Update()




            Elementos = Me.OrdersLineFacturas.GetLength(Dimension)

            ' LLAMADA AL WEB SERVICE

            Me.OrdersTablaFacturas(0).SAT_NHCreateSalesOrdersLine_1 = Me.OrdersLineFacturas
            Me.QweryOrdersFacturas.SAT_NHCreateSalesOrdersTable_1 = Me.OrdersTablaFacturas
            Me.WebFacturas.createListSAT_NHCreateSalesOrdersQuery(Me.DocumentoContextoFacturas, Me.QweryOrdersFacturas)

            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Axapta Ok  " & Me.WebFacturas.Url)



            Me.Db.mDbLector.Close()




        Catch ex As Exception
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service " & Me.WebFacturas.Url & " : " & ex.Message)

            MsgBox(ex.Message)
            '     MsgBox(ex.ToString)
        End Try
        '   Catch ex As Web.Services.Protocols.SoapException
        '       MsgBox(ex.Message)'

        'End Try





    End Sub

    Private Sub EnvioCobrosTest()
        Try

            '    Dim Ind As Integer

            Dim Primerregistro As Boolean = False


            Me.WebCobros = New WebReferenceCobros.SAT_NHJournalCustPaymentQueryService

            '    Actualiza(Url)

            Me.WebCobros.Url = Me.mAxUrl & "SAT_NHJournalCustPaymentQueryService.asmx"



            Me.TextBoxRutaWebServices.Text = Me.WebCobros.Url
            Me.TextBoxRutaWebServices.Update()

            Me.DocumentoContextoCobros = New WebReferenceCobros.DocumentContext
            Me.QweryOrdersCobros = New WebReferenceCobros.AxdSAT_NHJournalCustPaymentQuery


            Me.WebCobros.Credentials = System.Net.CredentialCache.DefaultCredentials
            '


            Me.DocumentoContextoCobros.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContextoCobros.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContextoCobros.DestinationEndpoint = Me.mAXDestinationEndPoint
            Me.DocumentoContextoCobros.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)




            Me.ListBoxDebug.Items.Add(" => MessageId            : " & Me.DocumentoContextoCobros.MessageId.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpoint       : " & Me.DocumentoContextoCobros.SourceEndpoint.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpointUser   : " & Me.DocumentoContextoCobros.SourceEndpointUser.ToString)
            Me.ListBoxDebug.Items.Add(" => DestinationEndpoint  : " & Me.DocumentoContextoCobros.DestinationEndpoint.ToString)





            Ind = 0
            ReDim Me.OrdersLineCobros(Ind)



            Me.OrdersLineCobros(Ind) = New WebReferenceCobros.AxdEntity_SAT_NHJournalCustPayment_1


            Me.OrdersLineCobros(Ind).InvoiceId = "8/2009"
            'Me.OrdersLineCobros(Ind).OffSetAccount = Me.mAxOffSetAccount
            '   Me.OrdersLineCobros(Ind).OffSetAccount = "BBVA"


            Me.OrdersLineCobros(Ind).PaymMode = "ANTHN"
            Me.OrdersLineCobros(Ind).Amount = 55
            Me.OrdersLineCobros(Ind).AmountSpecified = True
            Me.OrdersLineCobros(Ind).TransDate = "23/04/2009"
            ' Me.OrdersLineCobros(Ind).TransDate = CType("01/01/2009", Date)
            'Me.OrdersLineCobros(Ind).TransDateSpecified = True

            Me.ListBoxDebug.Items.Add(" ==> Cobro          : " & Me.OrdersLineCobros(Ind).InvoiceId.PadRight(10, " ") & "  ")




            ' carga de otro elemento 

            ReDim Preserve Me.OrdersLineCobros(UBound(Me.OrdersLineCobros) + 1)
            Ind = UBound(Me.OrdersLineCobros)
            Me.OrdersLineCobros(Ind) = New WebReferenceCobros.AxdEntity_SAT_NHJournalCustPayment_1
            Me.OrdersLineCobros(Ind).InvoiceId = "8/2009"

            Me.OrdersLineCobros(Ind).PaymMode = "PRU"
            Me.OrdersLineCobros(Ind).TransDate = "23/04/2009"
            'Me.OrdersLineCobros(Ind).TransDateSpecified = True

            Me.OrdersLineCobros(Ind).Amount = 50
            Me.OrdersLineCobros(Ind).AmountSpecified = True



            Me.ListBoxDebug.Items.Add(" ==> Cobro          : ")






            Me.ListBoxDebug.Items.Add("")
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Llamada al Web Service Cobros " & Me.WebCobros.Url)
            Me.ListBoxDebug.Items.Add("")
            Me.ListBoxDebug.Update()



            Elementos = Me.OrdersLineCobros.GetLength(Dimension)
            Me.TextBoxDebug.Text = Elementos


            ' LLAMADA AL WEB SERVICE
            If Elementos > 0 And IsNothing(Me.OrdersLineCobros(0)) = False Then
                Me.QweryOrdersCobros.SAT_NHJournalCustPayment_1 = Me.OrdersLineCobros
                Me.WebCobros.createListSAT_NHJournalCustPaymentQuery(Me.DocumentoContextoCobros, Me.QweryOrdersCobros)
                Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Axapta Ok  " & Me.WebCobros.Url)
            Else
                Me.ListBoxDebug.Items.Add(Format(Now, "F") & " No Hay Elenentos que enviar   " & Me.WebCobros.Url)
            End If

            'Application.DoEvents()


        Catch ex As Exception
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service Cobros " & Me.WebCobros.Url & " : " & ex.Message)

            MsgBox(ex.Message)
            '     MsgBox(ex.ToString)
        End Try
        '   Catch ex As Web.Services.Protocols.SoapException
        '       MsgBox(ex.Message)'

        'End Try


    End Sub
#End Region

#Region "Nuevo Tratamiento Linea a Linea"
#Region "PRODUCCION"
    Private Sub ProcesarProduccionLineaALinea()
        Try

            Me.WebProduccion = New WebReferenceProduccion.SAT_NHCreateProdOrdersQueryService
            'Actualiza Url
            Me.WebProduccion.Url = Me.mAxUrl & "SAT_NHCreateProdOrdersQueryService.asmx"

            Me.DocumentoContexto = New WebReferenceProduccion.DocumentContext
            Me.QweryOrders = New WebReferenceProduccion.AxdSAT_NHCreateProdOrdersQuery


            Me.WebProduccion.Credentials = New System.Net.NetworkCredential(Me.mAxUserName, Me.mAxUserPwd, Me.mAxDomainName)

            Me.DocumentoContexto.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContexto.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContexto.DestinationEndpoint = Me.mAXDestinationEndPoint
            Me.DocumentoContexto.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)

            Me.OrdersTabla(0) = New WebReferenceProduccion.AxdEntity_SAT_NHCreateProdOrdersTable_1


            Me.OrdersTabla(0).InvoiceDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
            Me.OrdersTabla(0).InvoiceDateSpecified = True

            ' 

            Me.ListBoxDebug.Items.Add(" => MessageId            : " & Me.DocumentoContexto.MessageId.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpoint       : " & Me.DocumentoContexto.SourceEndpoint.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpointUser   : " & Me.DocumentoContexto.SourceEndpointUser.ToString)
            Me.ListBoxDebug.Items.Add(" => DestinationEndpoint  : " & Me.DocumentoContexto.DestinationEndpoint.ToString)
            Me.ListBoxDebug.Items.Add(" => InvoiceDAte          : " & Me.OrdersTabla(0).InvoiceDate.ToString)


            ' Lineas de produccion 

            SQL = "SELECT ROWID,NVL(ASNT_CFCTA_COD,' ') AS CUENTA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,"
            SQL += "NVL(ASNT_DPTO_CODI,'?') AS SERVICIO ,NVL(ASNT_I_MONEMP,'0')  AS IMPORTE"
            SQL += ", NVL(ASNT_TIPO_PROD,'?') AS ASNT_TIPO_PROD "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHCreateProdOrdersQueryService'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "

            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"


            Me.Db.TraerLector(SQL)

            While Me.Db.mDbLector.Read

                '

                Ind = 0
                ReDim Me.OrdersLine(Ind)
                ReDim Me.TransRowid(Ind)


                Me.OrdersLine(Ind) = New WebReferenceProduccion.AxdEntity_SAT_NHCreateProdOrdersLine_1

                Me.OrdersLine(Ind).ItemId = Me.Db.mDbLector.Item("SERVICIO")


                If Me.Db.mDbLector.Item("SERVICIO") = Me.mAxServCodiBonos Or Me.Db.mDbLector.Item("SERVICIO") = Me.mAxServCodiBonosAsoc Then
                    Me.OrdersTabla(0).Bond = WebReferenceProduccion.AxdExtType_NoYesId.Yes
                    Me.OrdersTabla(0).BondSpecified = True
                Else
                    Me.OrdersTabla(0).Bond = WebReferenceProduccion.AxdExtType_NoYesId.No
                    Me.OrdersTabla(0).BondSpecified = True
                End If

                Me.OrdersLine(Ind).SalesPrice = Me.Db.mDbLector.Item("IMPORTE")
                Me.OrdersLine(Ind).SalesPriceSpecified = True

                If CType(Me.Db.mDbLector.Item("IMPORTE"), Double) < 0 Then
                    Me.OrdersLine(Ind).Qty = -1
                    Me.OrdersLine(Ind).SalesPrice = Me.Db.mDbLector.Item("IMPORTE") * -1
                Else
                    Me.OrdersLine(Ind).Qty = 1
                End If

                Me.OrdersLine(Ind).QtySpecified = True

                Me.ListBoxDebug.Items.Add(" ==> ItemIds y Qtys : " & CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(50, " ") & " " & CType(Me.Db.mDbLector.Item("IMPORTE"), String).PadRight(15, " ") & "  Bono : " & Me.OrdersTabla(0).Bond.ToString & " " & CType(Me.Db.mDbLector.Item("ASNT_TIPO_PROD"), String).PadRight(15, " ") & "  " & CType(Me.OrdersLine(Ind).Qty, String))
                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                Me.TransRowid(Ind) = Me.Db.mDbLector.Item("ROWID")


                Elementos = Me.OrdersLine.GetLength(Dimension)
                Me.TextBoxDebug.Text = Elementos



                ' LLAMADA AL WEB SERVICE

                Me.OrdersTabla(0).SAT_NHCreateProdOrdersLine_1 = Me.OrdersLine
                Me.QweryOrders.SAT_NHCreateProdOrdersTable_1 = Me.OrdersTabla

                If Me.EnviaProduccion(Me.DocumentoContexto, Me.QweryOrders) = True Then
                    Me.ListBoxDebug.Items.Add(" <== OK: ")
                    Me.ListBoxDebug.Update()
                    Me.TrataenvioGenerico(Me.TransRowid(Ind), 1, "OK")
                Else
                    Me.TotalErrores = Me.TotalErrores + 1
                    Me.TrataenvioGenerico(Me.TransRowid(Ind), 0, Me.AxError)
                    Me.ListBoxDebug.Items.Add(" <== : " & Me.AxError)
                    Me.ListBoxDebug.SelectedIndex = Me.ListBoxDebug.Items.Count - 1
                    Me.ListBoxDebug.Update()

                End If

                'Application.DoEvents()

                ' If MsgBox("Continuar ? ", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                ' Exit While
                ' End If

            End While
            Me.Db.mDbLector.Close()

        Catch ex As Exception
            Dim s As New System.Web.Services.Protocols.SoapException
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service Producción " & Me.WebProduccion.Url & " : " & ex.Message & " + " & s.Message)
            MsgBox(ex.Message)
        End Try
        '   Catch ex As Web.Services.Protocols.SoapException
        '       MsgBox(ex.Message)'

        'End Try

    End Sub
    Private Function EnviaProduccion(ByVal vDoc As WebReferenceProduccion.DocumentContext, ByVal vQuery As WebReferenceProduccion.AxdSAT_NHCreateProdOrdersQuery) As Boolean
        Try
            Me.AxError = ""
            vDoc.MessageId = Guid.NewGuid.ToString
            Me.WebProduccion.createListSAT_NHCreateProdOrdersQuery(vDoc, vQuery)
            Return True

        Catch ex As Exception
            Me.AxError = ex.Message
            Return False
        Finally
            ' Destruir los Objetos
            '   Me.WebProduccion.Dispose()

        End Try
    End Function
    Private Sub TrataenvioGenerico(ByVal vRowid As String, ByVal vStatus As Integer, ByVal vMessage As String)
        Try
            SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = " & vStatus
            SQL += " ,ASNT_ERR_MESSAGE = '" & Mid(vMessage, 1, 4000).Replace("'", "''").Trim & "'"
            SQL += " WHERE ROWID = '" & vRowid & "'"
            Me.DbWrite.EjecutaSqlCommit(SQL)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
#End Region
#Region "FACTURACION"
    Private Sub AxProcesarFacturacionLineaALinea()
        Try


            Dim Primerregistro As Boolean = False

            Dim ControlFacturaBono As Boolean = False
            Dim ControlFacturaOtrosServicios As Boolean = False




            Me.WebFacturas = New WebReferenceFacturas.SAT_NHCreateSalesOrdersQueryService

            'Actualiza Url

            Me.WebFacturas.Url = Me.mAxUrl & "SAT_NHCreateSalesOrdersQueryService.asmx"

            ' Aactualiza TIME OUT

            If Me.NumericUpDownWebServiceTimeOut.Value > 0 Then
                If Me.CheckBoxTimeOut.Checked Then
                    Me.WebFacturas.Timeout = Me.NumericUpDownWebServiceTimeOut.Value * 1000
                End If
            End If



            Me.TextBoxRutaWebServices.Text = Me.WebFacturas.Url
            Me.TextBoxRutaWebServices.Update()

            Me.DocumentoContextoFacturas = New WebReferenceFacturas.DocumentContext
            Me.QweryOrdersFacturas = New WebReferenceFacturas.AxdSAT_NHCreateSalesOrdersQuery


            '  Me.WebFacturas.Credentials = System.Net.CredentialCache.DefaultCredentials
            Me.WebFacturas.Credentials = New System.Net.NetworkCredential(Me.mAxUserName, Me.mAxUserPwd, Me.mAxDomainName)
            '


            Me.DocumentoContextoFacturas.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContextoFacturas.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContextoFacturas.DestinationEndpoint = Me.mAXDestinationEndPoint
            Me.DocumentoContextoFacturas.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)


            ' CABECERA DE FACTURA



            '       MsgBox("ojo falta tratamiento de campo nhcustaccountinfo segun notas de marta esta en ASNT_NHCUSTACCOUNT", MsgBoxStyle.Information, " falta Desarrollo")



            SQL = "SELECT  DISTINCT NVL(ASNT_LIN_DOCU,' ') AS DOCUMENTO,NVL(ASNT_CFCTA_COD,'?') AS CUENTA,ASNT_DOCU_VALO AS TOTAL,"
            SQL += " NVL(ASNT_CIF,'?') AS CIF ,ASNT_TIPOFACTURA_AX AS TIPOCLIENTE,NVL(ASNT_NHCUSTACCOUNT,'?') AS ASNT_NHCUSTACCOUNT "

            '20130926
            SQL += ",ASNT_RESE_FACT "
            '

            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "


            '  SQL += " AND ASNT_LIN_DOCU = '1805/S09' "

            SQL += " ORDER BY DOCUMENTO ASC "


            Me.Db.TraerLector(SQL)




            While Me.Db.mDbLector.Read




                Me.TreeViewDebug.Nodes.Add("==>   Factura: " & CType(Me.Db.mDbLector.Item("DOCUMENTO"), String) & " " & CType(Me.Db.mDbLector.Item("TOTAL"), String))

                ControlFacturaBono = False
                ControlFacturaOtrosServicios = False

                Dim ValorLiquido As Double = 0
                Dim ValorImpuesto As Double = 0
                '
                Dim TotalLinea As Double = 0
                Dim TotalLineaRedondeado As Double = 0
                Dim TotalLineaRedondeadoDiferencia As Double = 0

                '
               ' Dim TotalImpuestoAcum As Double = 0
                Dim TotalImpuestoAcum As Decimal = 0
                Dim TotalBaseAcum As Double = 0

                Dim TotalImpuestoRAcum As Double = 0
                Dim TotalBaseRAcum As Double = 0


                Dim TotalImpuestoRLinea As Double = 0
                Dim TotalBaseRLinea As Double = 0

                Dim TotalDiferenciaBase As Double

                'Dim TotalBugNewhotel As Double = 0
                Dim ControlTotal As Decimal = 0
                Dim TotalBugNewhotel As Decimal = 0

                Dim TotalDiferenciaImpuestos As Decimal = 0

                '' NIFES
                Me.OrdersTablaFacturas(0) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersTable_1

                ' 3 = CLIENTES CONTADO  O CUALQUIER FACTURA QUE TENGA EL NIF GENERAL DE CLIENTES CONTADO 
                If CType(Me.Db.mDbLector.Item("TIPOCLIENTE"), Integer) = 3 Or CType(Me.Db.mDbLector.Item("CIF"), String) = Me.mParaCifContadoGenerico Then
                    Me.OrdersTablaFacturas(0).ContClient = WebReferenceFacturas.AxdExtType_NoYesId.Yes
                    Me.OrdersTablaFacturas(0).VATNum = CType(Me.Db.mDbLector.Item("CIF"), String)
                Else
                    Me.OrdersTablaFacturas(0).ContClient = WebReferenceFacturas.AxdExtType_NoYesId.No
                    Me.OrdersTablaFacturas(0).VATNum = CType(Me.Db.mDbLector.Item("CIF"), String)
                    Me.OrdersTablaFacturas(0).NHCustAccount = CType(Me.Db.mDbLector.Item("ASNT_NHCUSTACCOUNT"), String)
                End If

                Me.OrdersTablaFacturas(0).ContClientSpecified = True




                Me.OrdersTablaFacturas(0).DocumentDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
                Me.OrdersTablaFacturas(0).DocumentDateSpecified = True
                Me.OrdersTablaFacturas(0).FixedDueDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
                Me.OrdersTablaFacturas(0).FixedDueDateSpecified = True
                Me.OrdersTablaFacturas(0).InvoiceDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
                'Me.OrdersTablaFacturas(0).InvoiceDateSpecified = True
                Me.OrdersTablaFacturas(0).InvoiceId = CType(Me.Db.mDbLector.Item("DOCUMENTO"), String)
                Me.OrdersTablaFacturas(0).SalesAmount = CType(Me.Db.mDbLector.Item("TOTAL"), Double)
                Me.OrdersTablaFacturas(0).SalesAmountSpecified = True

                '20130926

                If IsDBNull(Me.Db.mDbLector.Item("ASNT_RESE_FACT")) = False Then
                    Me.OrdersTablaFacturas(0).BookId = Mid(CStr(Me.Db.mDbLector.Item("ASNT_RESE_FACT")), 1, 200)
                Else
                    Me.OrdersTablaFacturas(0).BookId = ""
                End If
                '--



                Me.OrdersTablaFacturas(0).HotelId = Me.mHotelId

                Me.ListBoxDebug.Items.Add(" => MessageId            : " & Me.DocumentoContextoFacturas.MessageId.ToString)
                Me.ListBoxDebug.Items.Add(" => SourceEndpoint       : " & Me.DocumentoContextoFacturas.SourceEndpoint.ToString)
                Me.ListBoxDebug.Items.Add(" => SourceEndpointUser   : " & Me.DocumentoContextoFacturas.SourceEndpointUser.ToString)
                Me.ListBoxDebug.Items.Add(" => DestinationEndpoint  : " & Me.DocumentoContextoFacturas.DestinationEndpoint.ToString)
                Me.ListBoxDebug.Items.Add(" => InvoiceDAte          : " & OrdersTablaFacturas(0).InvoiceDate.ToString)
                Me.ListBoxDebug.Items.Add(" => Cliente              : " & Me.OrdersTablaFacturas(0).CustAccount)
                Me.ListBoxDebug.Items.Add(" => Nif/Pasaporte        : " & Me.OrdersTablaFacturas(0).VATNum)
                Me.ListBoxDebug.Items.Add(" => Cliente NhCustAc..   : " & Me.OrdersTablaFacturas(0).NHCustAccount)
                Me.ListBoxDebug.Items.Add(" => Cliente Contado ?    : " & Me.OrdersTablaFacturas(0).ContClient.ToString)
                Me.ListBoxDebug.Items.Add(" => Documento            : " & Me.OrdersTablaFacturas(0).InvoiceId.ToString)
                Me.ListBoxDebug.Items.Add(" => Total                : " & Me.OrdersTablaFacturas(0).SalesAmount.ToString)
                Me.ListBoxDebug.Items.Add(" => Reservas en Factura  : " & Me.OrdersTablaFacturas(0).BookId.ToString)



                ' Lineas de FActura



                SQL = "SELECT  ASNT_LIN_DOCU AS DOCUMENTO, ASNT_DPTO_CODI AS SERVICIO,"
                'SQL += "ROUND(ASNT_LIN_IMP1,2) AS VALORIMPUESTO,ROUND(ASNT_LIN_VLIQ,2) AS VALORLIQUIDO,ASNT_LIN_TIIMP AS PORCE "
                SQL += "ASNT_LIN_IMP1 AS VALORIMPUESTO,ASNT_LIN_VLIQ AS VALORLIQUIDO,ASNT_LIN_VCRE AS TOTAL,NVL(ASNT_LIN_TIIMP,0) AS PORCE "
                SQL += " ,NVL(ASNT_TIPO_VENTA,'?') AS TIPOVENTA,NVL(ASNT_NOMBRE,'?') AS CONCEPTO,NVL(ASNT_PROD_ID,'?') AS ASNT_PROD_ID "

                SQL += " FROM TH_ASNT "
                SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
                SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND ASNT_LIN_DOCU = '" & Me.Db.mDbLector.Item("DOCUMENTO") & "'"
                ' DEBAJO PARA DISTINGUIR LAS NOTAS DE CREDITO Y FACTURAS DE NEWGOLF ANULADAS POR SER EL MISMO DOCUMENTO POSITIVO Y NEGATIVO
                SQL += " AND TH_ASNT.ASNT_DOCU_VALO = " & Me.Db.mDbLector.Item("TOTAL")
                SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"
                ' SOLO SIN PROCESAR
                SQL += " AND ASNT_AX_STATUS =  0 "
                SQL += " ORDER BY ASNT_LIN_DOCU ASC "


                Me.DbAux.TraerLector(SQL)

                Primerregistro = True

                While Me.DbAux.mDbLector.Read



                    If Primerregistro = True Then
                        Primerregistro = False
                        Ind = 0
                        ReDim Me.OrdersLineFacturas(Ind)
                        ReDim Me.OrdersLineTaxFacturas(Ind)
                    Else                ' añade un elemento a array de lineas / Objeto
                        ReDim Preserve Me.OrdersLineFacturas(UBound(Me.OrdersLineFacturas) + 1)
                        ReDim Preserve Me.OrdersLineTaxFacturas(UBound(Me.OrdersLineTaxFacturas) + 1)

                        Ind = UBound(Me.OrdersLineFacturas)

                    End If



                    Me.OrdersLineFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersLine_1
                    Me.OrdersLineTaxFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersTaxLine_1




                    ' ARTICULO / SERVICIO

                    Me.OrdersLineFacturas(Ind).ItemId = CType(Me.DbAux.mDbLector.Item("ASNT_PROD_ID"), String)


                    ' CANTIDAD

                    If CType(Me.DbAux.mDbLector.Item("VALORLIQUIDO"), Double) < 0 Then
                        ValorLiquido = CType(Me.DbAux.mDbLector.Item("VALORLIQUIDO"), Double) * -1
                        ValorImpuesto = CType(Me.DbAux.mDbLector.Item("VALORIMPUESTO"), Double) * -1
                        Me.OrdersLineFacturas(Ind).Qty = -1
                    Else
                        ValorLiquido = CType(Me.DbAux.mDbLector.Item("VALORLIQUIDO"), Double)
                        ValorImpuesto = CType(Me.DbAux.mDbLector.Item("VALORIMPUESTO"), Double)
                        Me.OrdersLineFacturas(Ind).Qty = 1
                    End If

                    Me.OrdersLineFacturas(Ind).QtySpecified = True

                    ' SALES PRICE
                    Me.OrdersLineFacturas(Ind).SalesPrice = Math.Round(ValorLiquido, 2, MidpointRounding.AwayFromZero)
                    Me.OrdersLineFacturas(Ind).SalesPriceSpecified = True



                    ' IMPUESTO
                    Me.OrdersLineTaxFacturas(Ind).TaxAmount = ValorImpuesto
                    Me.OrdersLineTaxFacturas(Ind).TaxAmountSpecified = True

                    ' BASE IMPONIBLE
                    Me.OrdersLineTaxFacturas(Ind).TaxBase = ValorLiquido
                    Me.OrdersLineTaxFacturas(Ind).TaxBaseSpecified = True


                    Me.OrdersLineTaxFacturas(Ind).TaxCode = CType(Me.DbAux.mDbLector.Item("PORCE"), String) & "%"
                    Me.OrdersLineTaxFacturas(Ind).TaxPercent = CType(Me.DbAux.mDbLector.Item("PORCE"), Double)
                    Me.OrdersLineTaxFacturas(Ind).TaxPercentSpecified = True




                    ' P0006
                    If Me.DbAux.mDbLector.Item("SERVICIO") = Me.mAxServCodiBonos Or Me.DbAux.mDbLector.Item("TIPOVENTA") = "BONOS" Then

                        ControlFacturaBono = True
                        Me.OrdersTablaFacturas(0).Bond = WebReferenceFacturas.AxdExtType_NoYesId.Yes
                        Me.OrdersTablaFacturas(0).BondSpecified = True
                    End If

                    If Me.DbAux.mDbLector.Item("SERVICIO") <> Me.mAxServCodiBonos And Me.DbAux.mDbLector.Item("TIPOVENTA") <> "BONOS" Then
                        ControlFacturaOtrosServicios = True
                        Me.OrdersTablaFacturas(0).Bond = WebReferenceFacturas.AxdExtType_NoYesId.No
                        Me.OrdersTablaFacturas(0).BondSpecified = True
                    End If




                    ' TINGLADO DECIMALES SOLO PARA DEBUG

                    TotalBaseAcum = TotalBaseAcum + CType(Me.DbAux.mDbLector.Item("VALORLIQUIDO"), Double)
                    TotalBaseRAcum = TotalBaseRAcum + Math.Round(CType(Me.DbAux.mDbLector.Item("VALORLIQUIDO"), Double), 2, MidpointRounding.AwayFromZero)
                    TotalBaseRLinea = Math.Round(CType(Me.DbAux.mDbLector.Item("VALORLIQUIDO"), Double), 2, MidpointRounding.AwayFromZero)

                    TotalImpuestoAcum = TotalImpuestoAcum + CType(Me.DbAux.mDbLector.Item("VALORIMPUESTO"), Double)

                    ' IMPUESTO CALCULADO COMO AX
                    TotalImpuestoRLinea = Math.Round((Math.Round(CType(Me.DbAux.mDbLector.Item("VALORLIQUIDO"), Double), 2, MidpointRounding.AwayFromZero) * CType(Me.DbAux.mDbLector.Item("PORCE"), Double)) / 100, 2, MidpointRounding.AwayFromZero)
                    TotalImpuestoRAcum = TotalImpuestoRAcum + TotalImpuestoRLinea



                    ' debug
                    TotalLinea = Math.Round(CType(Me.DbAux.mDbLector.Item("VALORLIQUIDO"), Decimal) + CType(Me.DbAux.mDbLector.Item("VALORIMPUESTO"), Decimal), 2, MidpointRounding.AwayFromZero)
                    TotalLineaRedondeado = Math.Round(CType(Me.DbAux.mDbLector.Item("VALORLIQUIDO"), Decimal), 2, MidpointRounding.AwayFromZero) + Math.Round(CType(Me.DbAux.mDbLector.Item("VALORIMPUESTO"), Decimal), 2, MidpointRounding.AwayFromZero)
                    TotalLineaRedondeadoDiferencia = TotalLineaRedondeado - TotalLinea



                    Me.ListBoxDebug.Items.Add(" ==> Lineas de Fac  : " & CType(Me.DbAux.mDbLector.Item("DOCUMENTO"), String).PadRight(10, " ") & " " & CStr(Me.OrdersLineFacturas(Ind).ItemId).PadRight(15, " ") & " " & CType(Me.DbAux.mDbLector.Item("CONCEPTO"), String).PadRight(30, " ") & " Base : " & CType(Me.DbAux.mDbLector.Item("VALORLIQUIDO"), String).PadRight(35, " ") & " Impuesto (" & CType(Me.DbAux.mDbLector.Item("PORCE"), String) & "%)  " & CType(Me.DbAux.mDbLector.Item("VALORIMPUESTO"), String).PadRight(35, " ") & "  Total Linea NewHotel = " & CStr(TotalLinea).PadRight(15, " ") & " Cantidad = " & CType(Me.OrdersLineFacturas(Ind).Qty, String).PadRight(5, " ") & "| Axapta  Base Ax = " & CStr(TotalBaseRLinea).PadRight(15, " ") & "  Axapta Impuesto Ax = " & TotalImpuestoRLinea)
                    Me.ListBoxDebug.Update()
                    Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                    Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1).Nodes.Add(CType(Me.DbAux.mDbLector.Item("CONCEPTO"), String).PadRight(50, " ") & " " & TotalLinea)
                    Me.TreeViewDebug.SelectedNode = Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1)
                    Me.TreeViewDebug.ExpandAll()
                    Me.TreeViewDebug.Update()

                End While
                Me.DbAux.mDbLector.Close()



                Me.ListBoxDebug.Items.Add("NewHotel Redondeado")
                Me.ListBoxDebug.Items.Add("  Total Base = " & Math.Round(TotalBaseAcum, 2, MidpointRounding.AwayFromZero) & " Total Impuestos = " & Math.Round(TotalImpuestoAcum, 2, MidpointRounding.AwayFromZero) & "  Total factura = " & Math.Round(TotalBaseAcum, 2, MidpointRounding.AwayFromZero) + Math.Round(TotalImpuestoAcum, 2, MidpointRounding.AwayFromZero))

                Me.ListBoxDebug.Items.Add("")
                Me.ListBoxDebug.Items.Add("Simulación Axapta")

                ' Me.ListBoxDebug.Items.Add("  Total Base = " & Math.Round(TotalBaseRAcum, 2, MidpointRounding.AwayFromZero) & " Total Impuestos Suma impuestos de cada Línea   = " & Math.Round(TotalImpuestoRAcum, 2, MidpointRounding.AwayFromZero) & "  Total factura = " & Math.Round(TotalBaseRAcum, 2, MidpointRounding.AwayFromZero) + Math.Round(TotalImpuestoRAcum, 2, MidpointRounding.AwayFromZero))
                ' se muestra los impuestos de axapta calculados sobre  los acumulados de las bases asi es como hace ax parece 
                Me.ListBoxDebug.Items.Add("  Total Base = " & Math.Round(TotalBaseRAcum, 2, MidpointRounding.AwayFromZero) & " Total Impuestos = " & Me.CalculaImpuestosAxVisual(Me.Db.mDbLector.Item("DOCUMENTO"), Me.Db.mDbLector.Item("TOTAL")) & "  Total factura = " & Math.Round(TotalBaseRAcum, 2, MidpointRounding.AwayFromZero) + Me.CalculaImpuestosAxVisual(Me.Db.mDbLector.Item("DOCUMENTO"), Me.Db.mDbLector.Item("TOTAL")))



                Me.ListBoxDebug.Items.Add("")



                '****************************************************************************************
                ' listbos debug se la factura 

                Me.ListBoxAjustesFacturas.Items.Add("NewHotel Factura : " & CType(Me.Db.mDbLector.Item("DOCUMENTO"), String))
                Me.ListBoxAjustesFacturas.Items.Add("  Total Base = " & Math.Round(TotalBaseAcum, 2, MidpointRounding.AwayFromZero) & " Total Impuestos = " & Math.Round(TotalImpuestoAcum, 2, MidpointRounding.AwayFromZero) & "  Total Base + Impuesto = " & Math.Round(TotalBaseAcum, 2, MidpointRounding.AwayFromZero) + Math.Round(TotalImpuestoAcum, 2, MidpointRounding.AwayFromZero) & " Total Factura TNHT_FACT " & CType(Me.Db.mDbLector.Item("TOTAL"), String))

                Me.ListBoxAjustesFacturas.Items.Add("")

                Me.ListBoxAjustesFacturas.Items.Add("Axapta")
                Me.ListBoxAjustesFacturas.Items.Add("  Total Base = " & Math.Round(TotalBaseRAcum, 2, MidpointRounding.AwayFromZero) & " Total Impuestos = " & Me.CalculaImpuestosAxVisual(Me.Db.mDbLector.Item("DOCUMENTO"), Me.Db.mDbLector.Item("TOTAL")) & "  Total factura = " & Math.Round(TotalBaseRAcum, 2, MidpointRounding.AwayFromZero) + Me.CalculaImpuestosAxVisual(Me.Db.mDbLector.Item("DOCUMENTO"), Me.Db.mDbLector.Item("TOTAL")))

                '************************************************************************************


                ' DEBAJO AJUSTE DE BASE IMPONIBLE POR REDONDEO AXAPTA
                TotalDiferenciaBase = TotalBaseAcum - TotalBaseRAcum

                If Me.CalculaAjusteRedondeoBaseAx(Me.Db.mDbLector.Item("DOCUMENTO"), Me.Db.mDbLector.Item("TOTAL")) <> 0 Then


                    Me.ListBoxDebug.Items.Add("Inicio de Ajustes por Diferencia en Total Base Imponible  " & Math.Round(TotalDiferenciaBase, 2, MidpointRounding.AwayFromZero))

                    If Me.CheckBoxRedondeaFactura.Checked Or Me.CheckBoxRedondeaFacturaTest.Checked Then

                        ReDim Preserve Me.OrdersLineFacturas(UBound(Me.OrdersLineFacturas) + 1)
                        Ind = UBound(Me.OrdersLineFacturas)
                        Me.OrdersLineFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersLine_1


                        ' SERVICI0
                        Me.OrdersLineFacturas(Ind).ItemId = mAxAjustaBase


                        If Me.CalculaAjusteRedondeoBaseAx(Me.Db.mDbLector.Item("DOCUMENTO"), Me.Db.mDbLector.Item("TOTAL")) < 0 Then
                            TotalDiferenciaBase = Me.CalculaAjusteRedondeoBaseAx(Me.Db.mDbLector.Item("DOCUMENTO"), Me.Db.mDbLector.Item("TOTAL")) * -1
                            Me.OrdersLineFacturas(Ind).Qty = -1
                        Else
                            TotalDiferenciaBase = Me.CalculaAjusteRedondeoBaseAx(Me.Db.mDbLector.Item("DOCUMENTO"), Me.Db.mDbLector.Item("TOTAL"))
                            Me.OrdersLineFacturas(Ind).Qty = 1
                        End If

                        Me.OrdersLineFacturas(Ind).QtySpecified = True

                        ' SALES PRICE
                        ' Me.OrdersLineFacturas(Ind).SalesPrice = CType(Math.Round(TotalDiferenciaBase, 2), Double)
                        Me.OrdersLineFacturas(Ind).SalesPrice = TotalDiferenciaBase
                        Me.OrdersLineFacturas(Ind).SalesPriceSpecified = True


                        Me.ListBoxDebug.Items.Add(" ==> + Ajuste Necesario  por la Base  : " & " Base : " & Math.Round(TotalDiferenciaBase, 2, MidpointRounding.AwayFromZero) & " Impuesto :" & "0".PadRight(10, " ") & " Servicio =  " & mAxAjustaBase & "  Cantidad  " & CType(Me.OrdersLineFacturas(Ind).Qty, String))
                        Me.ListBoxAjustesFacturas.Items.Add(" ==> + Ajuste Necesario  por la Base  : " & " Base : " & Math.Round(TotalDiferenciaBase, 2, MidpointRounding.AwayFromZero) & " Impuesto :" & "0".PadRight(10, " ") & " Servicio =  " & mAxAjustaBase & "  Cantidad  " & CType(Me.OrdersLineFacturas(Ind).Qty, String))

                        Me.ListBoxDebug.Update()
                        Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                    End If

                End If

                ' sale a buscar y a ajustar descuadres en el impuesto
                If TotalImpuestoAcum <> 0 Then



                    ' PAT 40  
                    ' SOLO VA A BUSCAR DIFERENCIAS SI EXISTE 

                    If Me.CheckBoxPat40.Checked = True Then
                        ' ANTES 
                        ' no usa pat 40
                        If Me.CheckBoxRedondeaFactura.Checked = True Then
                            Me.CalculaAjusteRedondeoImpuestoAx(Me.Db.mDbLector.Item("DOCUMENTO"), Me.Db.mDbLector.Item("TOTAL"))
                        Else
                            Me.CalculaAjusteRedondeoImpuestoAxNuevo5(Me.Db.mDbLector.Item("DOCUMENTO"), Me.Db.mDbLector.Item("TOTAL"))
                        End If
                        ' FIN ANTES 
                    Else
                        ' si usa pat 40
                        If Math.Round(TotalImpuestoAcum, 2, MidpointRounding.AwayFromZero) <> Math.Round(TotalImpuestoRAcum, 2, MidpointRounding.AwayFromZero) Then
                            ' ANTES 
                            If Me.CheckBoxRedondeaFactura.Checked = True Then
                                Me.CalculaAjusteRedondeoImpuestoAx(Me.Db.mDbLector.Item("DOCUMENTO"), Me.Db.mDbLector.Item("TOTAL"))
                            Else
                                Me.CalculaAjusteRedondeoImpuestoAxNuevo5(Me.Db.mDbLector.Item("DOCUMENTO"), Me.Db.mDbLector.Item("TOTAL"))
                            End If
                            ' FIN ANTES 
                        End If
                    End If

                End If




                ' Pat 0012
                ' TINGLADO PARA FACTURAS NEWHOTEL CUYA SUMA DE LA BASE IMPONIOBLE MAS EL IMPUESTO ( REDONDEADOS ) NO CONCINDIDEN CON 
                ' EL TOTAL FACTURA   EJEMPLO DE CASO SALOBRE FACTURA 1805/S09

                '      If Me.OrdersTablaFacturas(0).SalesAmount <> Math.Round(TotalBaseAcum, 2) + Math.Round(TotalImpuestoAcum, 2) Then


                ' TRABAJANDO
                If TIPOREDONDEO = ETIPOREDONDEO.AwayFromZero Then
                    ControlTotal = Math.Round(TotalBaseAcum, 2, MidpointRounding.AwayFromZero) + Math.Round(TotalImpuestoAcum, 2, MidpointRounding.AwayFromZero)

                Else
                    ControlTotal = Math.Round(TotalBaseAcum, 2) + Math.Round(TotalImpuestoAcum, 2)

                End If


                If Me.OrdersTablaFacturas(0).SalesAmount <> ControlTotal Then



                    '   TotalBugNewhotel = Me.OrdersTablaFacturas(0).SalesAmount - (Math.Round(TotalBaseAcum, 2) + Math.Round(TotalImpuestoAcum, 2))
                    TotalBugNewhotel = Me.OrdersTablaFacturas(0).SalesAmount - ControlTotal

                    '    MsgBox("Esto es grave Total Amount Difiere de Total Factura NewHotel Redondeada Pat0012", MsgBoxStyle.Information, "Atención")

                    ReDim Preserve Me.OrdersLineFacturas(UBound(Me.OrdersLineFacturas) + 1)
                    Ind = UBound(Me.OrdersLineFacturas)
                    Me.OrdersLineFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersLine_1


                    ' SERVICI0
                    Me.OrdersLineFacturas(Ind).ItemId = mAxAjustaBase


                    If TotalBugNewhotel < 0 Then
                        TotalDiferenciaBase = TotalBugNewhotel * -1
                        Me.OrdersLineFacturas(Ind).Qty = -1
                    Else
                        TotalDiferenciaBase = TotalBugNewhotel
                        Me.OrdersLineFacturas(Ind).Qty = 1
                    End If

                    Me.OrdersLineFacturas(Ind).QtySpecified = True

                    ' SALES PRICE
                    Me.OrdersLineFacturas(Ind).SalesPrice = TotalDiferenciaBase
                    Me.OrdersLineFacturas(Ind).SalesPriceSpecified = True


                    Me.ListBoxDebug.Items.Add(" ==> + Ajuste Extraordinario Necesario  por la Base(Véase Pat 00012 en Código Fuente : " & " Base : " & Math.Round(TotalDiferenciaBase, 2, MidpointRounding.AwayFromZero) & " Impuesto :" & "0".PadRight(10, " ") & " Servicio =  " & mAxAjustaBase & "  Cantidad  " & CType(Me.OrdersLineFacturas(Ind).Qty, String))
                    Me.ListBoxAjustesFacturas.Items.Add(" ==> + Ajuste Extraordinario Necesario  por la Base(Véase Pat 00012 en Código Fuente : " & " Base : " & Math.Round(TotalDiferenciaBase, 2, MidpointRounding.AwayFromZero) & " Impuesto :" & "0".PadRight(10, " ") & " Servicio =  " & mAxAjustaBase & "  Cantidad  " & CType(Me.OrdersLineFacturas(Ind).Qty, String))
                    Me.ListBoxAjustesFacturas.Items.Add("")
                    Me.ListBoxAjustesFacturas.Items.Add("")

                    Me.ListBoxDebug.Update()
                    Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1
                End If




                Me.ListBoxDebug.Items.Add("")
                Me.ListBoxDebug.Items.Add(" ==> Bono            : " & Me.OrdersTablaFacturas(0).Bond.ToString)
                Me.ListBoxDebug.Items.Add("________________________________________________________________________________________________________")
                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1


                Me.ListBoxAjustesFacturas.Items.Add("________________________________________________________________________________________________________")
                Me.ListBoxAjustesFacturas.Items.Add("")



                Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Llamada al Web Service Facturas " & Me.WebFacturas.Url)
                Me.ListBoxDebug.Update()


                Elementos = Me.OrdersLineFacturas.GetLength(Dimension)
                Me.TextBoxDebug.Text = Elementos

                '--------------------------------------------------------------------------------------------------------------------





                ' LLAMADA AL WEB SERVICE

                If (ControlFacturaBono = True And ControlFacturaOtrosServicios = False) Or (ControlFacturaBono = False And ControlFacturaOtrosServicios = True) Then


                    Me.OrdersTablaFacturas(0).SAT_NHCreateSalesOrdersLine_1 = Me.OrdersLineFacturas
                    Me.QweryOrdersFacturas.SAT_NHCreateSalesOrdersTable_1 = Me.OrdersTablaFacturas

                    If Me.EnviaFactura(Me.DocumentoContextoFacturas, Me.QweryOrdersFacturas, Elementos) = True Then
                        Me.ListBoxDebug.Items.Add(" <== OK: ")
                        Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1
                        Me.ListBoxDebug.Update()
                        Me.TrataenvioFactura(Me.Db.mDbLector.Item("DOCUMENTO"), Me.Db.mDbLector.Item("TOTAL"), 1, "OK")
                    Else
                        Me.TotalErrores = Me.TotalErrores + 1
                        Me.TrataenvioFactura(Me.Db.mDbLector.Item("DOCUMENTO"), Me.Db.mDbLector.Item("TOTAL"), 0, Me.AxError)
                        Me.ListBoxDebug.Items.Add(" <== : " & Me.AxError)
                        Me.ListBoxDebug.SelectedIndex = Me.ListBoxDebug.Items.Count - 1
                        Me.ListBoxDebug.Update()
                        Beep()

                    End If

                Else
                    '    MsgBox("Atencion la factura " & CType(Me.Db.mDbLector.Item("DOCUMENTO"), String) & " NO se Transmite por Contener Bonos y Otros Servicios", MsgBoxStyle.Exclamation, "Atención")

                    ' NUEVO SI SE ENVIA PERO CON BOND = NO E( EN PRUEBAS)
                    'MsgBox("Atencion la factura " & CType(Me.Db.mDbLector.Item("DOCUMENTO"), String) & " SI se Transmite aunque contiene Bonos y Otros Servicios", MsgBoxStyle.Exclamation, "Atención")


                    Me.OrdersTablaFacturas(0).Bond = WebReferenceFacturas.AxdExtType_NoYesId.No
                    Me.OrdersTablaFacturas(0).BondSpecified = True

                    Me.OrdersTablaFacturas(0).SAT_NHCreateSalesOrdersLine_1 = Me.OrdersLineFacturas
                    Me.QweryOrdersFacturas.SAT_NHCreateSalesOrdersTable_1 = Me.OrdersTablaFacturas



                    If Me.EnviaFactura(Me.DocumentoContextoFacturas, Me.QweryOrdersFacturas, Elementos) = True Then
                        Me.ListBoxDebug.Items.Add(" <== OK: ")
                        Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1
                        Me.ListBoxDebug.Update()
                        Me.TrataenvioFactura(Me.Db.mDbLector.Item("DOCUMENTO"), Me.Db.mDbLector.Item("TOTAL"), 1, "OK")
                    Else
                        Me.TotalErrores = Me.TotalErrores + 1
                        Me.TrataenvioFactura(Me.Db.mDbLector.Item("DOCUMENTO"), Me.Db.mDbLector.Item("TOTAL"), 0, Me.AxError)
                        Me.ListBoxDebug.Items.Add(" <== : " & Me.AxError)
                        Me.ListBoxDebug.SelectedIndex = Me.ListBoxDebug.Items.Count - 1
                        Me.ListBoxDebug.Update()
                        Beep()

                    End If
                    ' FIN DE NUEVO SI SE ENVIA PERO CON BOND = NO E( EN PRUEBAS)

                End If


                'Application.DoEvents()

                ' If MsgBox("Continuar ? ", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                ' Exit While
                ' End If

            End While
            Me.Db.mDbLector.Close()




        Catch ex As Exception
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service Facturas " & Me.WebFacturas.Url & " : " & ex.Message)

            MsgBox(ex.Message)
            '     MsgBox(ex.ToString)
        End Try
        '   Catch ex As Web.Services.Protocols.SoapException
        '       MsgBox(ex.Message)'

        'End Try

    End Sub

    Private Function EnviaFactura(ByVal vDoc As WebReferenceFacturas.DocumentContext, ByVal vQuery As WebReferenceFacturas.AxdSAT_NHCreateSalesOrdersQuery, ByVal velementos As Integer) As Boolean
        Try
            Me.AxError = ""
            vDoc.MessageId = Guid.NewGuid.ToString

            'AQUI()
            '   If velementos < 10 Then
            Me.mInicio = Now
            Me.WebFacturas.createListSAT_NHCreateSalesOrdersQuery(vDoc, vQuery)
            'End If
            Me.TreeViewDebug.Nodes.Add("<==  Ok Factura " & Me.mFecha)
            mDuracion = DateDiff(DateInterval.Second, Me.mInicio, Now) / (24 * 3600)
            Me.ListBoxDebug.Items.Add(Format(Date.FromOADate(Me.mDuracion), "mm 'minutos ' ss ' segundos'"))
            Return True

        Catch ex As Exception
            Me.AxError = ex.Message
            Me.TreeViewDebug.Nodes.Add("<==   " & ex.Message)
            mDuracion = DateDiff(DateInterval.Second, Me.mInicio, Now) / (24 * 3600)
            Me.ListBoxDebug.Items.Add(Format(Date.FromOADate(Me.mDuracion), "mm 'minutos ' ss ' segundos'"))
            Return False
        End Try
    End Function
    Private Sub TrataenvioFactura(ByVal vDocumento As String, ByVal vTotal As Double, ByVal vStatus As Integer, ByVal vMessage As String)
        Try
            SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = " & vStatus
            SQL += " ,ASNT_ERR_MESSAGE = '" & Mid(vMessage, 1, 4000).Replace("'", "''").Trim & "'"
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_LIN_DOCU = '" & vDocumento & "'"
            SQL += " AND TH_ASNT.ASNT_DOCU_VALO = " & vTotal
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"

            Me.DbWrite.EjecutaSqlCommit(SQL)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Function CalculaAjusteRedondeoBaseAx(ByVal vDocumento As String, ByVal vValor As Double) As Double
        Try

            ' calcula la diferencia entre la base imponible de newhotel = ( suma del liquido sin redondear linea a linea y redondeoado el resultado final ) 
            ' con la simulacion de la base imponible de axapta = ( suma del los valores liquidos redondeados linea a linea )
            ' la funcion devuelve la dirferencia entre ambios calculos


            Dim Response As Double


            SQL = "SELECT  ASNT_LIN_DOCU AS DOCUMENTO,ASNT_LIN_TIIMP,"
            SQL += "ROUND(SUM(ASNT_LIN_IMP1),2) AS VALORIMPUESTO,ROUND(SUM(ASNT_LIN_VLIQ),2) AS VALORLIQUIDO, "
            SQL += "SUM(ROUND(ASNT_LIN_IMP1,2)) AS VALORIMPUESTOR,SUM(ROUND(ASNT_LIN_VLIQ,2)) AS VALORLIQUIDOR, "
            SQL += "   ROUND (SUM (asnt_lin_vliq), 2)  -  SUM (ROUND (asnt_lin_vliq, 2))  AS DIFERENCIALIQUIDO"
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_LIN_DOCU = '" & vDocumento & "'"
            SQL += " AND TH_ASNT.ASNT_DOCU_VALO = " & vValor
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "
            SQL += " GROUP BY ASNT_LIN_DOCU,ASNT_LIN_TIIMP "
            SQL += " ORDER BY ASNT_LIN_DOCU ASC "

            Me.DbAux.TraerLector(SQL)

            While Me.DbAux.mDbLector.Read
                Response = Response + Me.DbAux.mDbLector.Item("DIFERENCIALIQUIDO")
            End While
            Me.DbAux.mDbLector.Close()

            Return Response

        Catch ex As Exception
            Return 0
            MsgBox(ex.Message)
        End Try
    End Function


    Private Sub CalculaAjusteRedondeoImpuestoAx(ByVal vDocumento As String, ByVal vValor As Double)
        Try

            Dim TotalDiferencia As Double
            Dim Ind2 As Integer

            Dim AuxLastQty As Integer

            SQL = "SELECT  ASNT_LIN_DOCU AS DOCUMENTO,nvl(ASNT_LIN_TIIMP,0) as ASNT_LIN_TIIMP ,"
            SQL += "ROUND(SUM(ASNT_LIN_IMP1),2) AS VALORIMPUESTO,ROUND(SUM(ASNT_LIN_VLIQ),2) AS VALORLIQUIDO, "
            SQL += "SUM(ROUND(ASNT_LIN_IMP1,2)) AS VALORIMPUESTOR,SUM(ROUND(ASNT_LIN_VLIQ,2)) AS VALORLIQUIDOR, "
            SQL += "   ROUND (SUM (asnt_lin_vliq), 2)  -  SUM (ROUND (asnt_lin_vliq, 2))  AS DIFERENCIALIQUIDO,"
            SQL += "ROUND((SUM (ROUND (asnt_lin_vliq, 2))  *  asnt_lin_tiimp ) / 100,2) as impuestoax,"
            SQL += "ROUND (SUM (asnt_lin_imp1), 2) -  ROUND((SUM (ROUND (asnt_lin_vliq, 2))  *  asnt_lin_tiimp ) / 100,2) AS diferenciaimpuesto,"
            SQL += "(ROUND (SUM (asnt_lin_imp1), 2) -  ROUND((SUM (ROUND (asnt_lin_vliq, 2))  *  asnt_lin_tiimp ) / 100,2)) * 100 / asnt_lin_tiimp as AJUSTE"

            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_LIN_DOCU = '" & vDocumento & "'"
            SQL += " AND TH_ASNT.ASNT_DOCU_VALO = " & vValor
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"
            ' SOLO con ajuste necesario Y SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "
            '2016/21 JUNIO EVITAR ERROR DIVIDIR POR ZERO SI HAY PROPINA(SERVIVIO CON IMPUESTO ZERO) EN LA FACTURA 
            SQL += " AND asnt_lin_tiimp <>  0 "

            SQL += " GROUP BY ASNT_LIN_DOCU,ASNT_LIN_TIIMP "
            SQL += "HAVING (ROUND (SUM (asnt_lin_imp1), 2) -  ROUND((SUM (ROUND (asnt_lin_vliq, 2))  *  asnt_lin_tiimp ) / 100,2)) * 100 / asnt_lin_tiimp <> 0 "
            SQL += " ORDER BY ASNT_LIN_DOCU ASC "



            Me.DbAux.TraerLector(SQL)

            While Me.DbAux.mDbLector.Read


                Me.ListBoxDebug.Items.Add("")
                Me.ListBoxDebug.Items.Add("Inicio de Ajustes por Diferencia en Impuestos")


                ReDim Preserve Me.OrdersLineFacturas(UBound(Me.OrdersLineFacturas) + 1)
                ReDim Preserve Me.OrdersLineTaxFacturas(UBound(Me.OrdersLineTaxFacturas) + 1)

                Ind = UBound(Me.OrdersLineFacturas)
                Ind2 = UBound(Me.OrdersLineTaxFacturas)


              
                Me.OrdersLineFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersLine_1
                Me.OrdersLineTaxFacturas(Ind2) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersTaxLine_1


           
            

                SQL = "SELECT NVL(SATI_ARAX,'NULO') FROM TH_SATI WHERE SATI_TASA = " & CDbl(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"))
                Me.OrdersLineFacturas(Ind).ItemId = Me.DbAux2.EjecutaSqlScalar(SQL)


                ' ---------------------------------------------------------------------------------------

                If CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double) < 0 Then
                    TotalDiferencia = CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double) * -1
                    Me.OrdersLineFacturas(Ind).Qty = -1
                Else
                    TotalDiferencia = CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double)
                    Me.OrdersLineFacturas(Ind).Qty = 1
                End If


                Me.ListBox1.Items.Add(Me.OrdersLineFacturas(Ind).ItemId & " = " & TotalDiferencia)

                AuxLastQty = Me.OrdersLineFacturas(Ind).Qty

                Me.OrdersLineFacturas(Ind).QtySpecified = True

                ' SALES PRICE
                Me.OrdersLineFacturas(Ind).SalesPrice = TotalDiferencia
                'Me.OrdersLineFacturas(Ind).SalesPrice = Math.Round(TotalDiferencia, 2, MidpointRounding.AwayFromZero)
                Me.OrdersLineFacturas(Ind).SalesPriceSpecified = True



                ' IMPUESTO
                Me.OrdersLineTaxFacturas(Ind2).TaxAmount = CType(Me.DbAux.mDbLector.Item("diferenciaimpuesto"), Double)
                Me.OrdersLineTaxFacturas(Ind2).TaxAmountSpecified = True

                ' BASE IMPONIBLE

                If Me.CheckBoxPideBase.Checked Then
                    Me.OrdersLineTaxFacturas(Ind2).TaxBase = InputBox("Base Imponible " & vDocumento & " suma o resta un céntimo a " & Math.Round(TotalDiferencia, 2), "Ajuste")
                Else
                    Me.OrdersLineTaxFacturas(Ind2).TaxBase = TotalDiferencia
                End If

                Me.OrdersLineTaxFacturas(Ind2).TaxBaseSpecified = True


                Me.OrdersLineTaxFacturas(Ind2).TaxCode = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), String) & "%"
                Me.OrdersLineTaxFacturas(Ind2).TaxPercent = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), Double)
                Me.OrdersLineTaxFacturas(Ind2).TaxPercentSpecified = True
                Me.ListBoxDebug.Items.Add(" ==> + Ajuste Necesario Impuesto  : " & " Base : " & Math.Round(TotalDiferencia, 2) & " Impuesto :" & CType(Me.OrdersLineTaxFacturas(Ind2).TaxAmount, String).PadRight(10, " ") & " Servicio =  " & CType(Me.OrdersLineFacturas(Ind).ItemId, String) & "  Cantidad  " & CType(Me.OrdersLineFacturas(Ind).Qty, String))
                Me.ListBoxAjustesFacturas.Items.Add(" ==> + Ajuste Necesario Impuesto  : " & " Base : " & Math.Round(TotalDiferencia, 2) & " Impuesto :" & CType(Me.OrdersLineTaxFacturas(Ind2).TaxAmount, String).PadRight(10, " ") & " Servicio =  " & CType(Me.OrdersLineFacturas(Ind).ItemId, String) & "  Cantidad  " & CType(Me.OrdersLineFacturas(Ind).Qty, String))


                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                '----------------------------------------------------------------------------------------------------------------
                'enviar otra vez la base del impuesto al articulo cojn el signo contrario y al articulo sin aimpuesto A02"
                '----------------------------------------------------------------------------------------------------------------

                ReDim Preserve Me.OrdersLineFacturas(UBound(Me.OrdersLineFacturas) + 1)
                Ind = UBound(Me.OrdersLineFacturas)


                Me.OrdersLineFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersLine_1


                ' SERVICI0

                Me.OrdersLineFacturas(Ind).ItemId = mAxAjustaBase


                If CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double) < 0 Then
                    TotalDiferencia = CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double) * -1
                Else
                    TotalDiferencia = CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double)
                End If

                ' cantidad invertida al movimiento anterior
                Me.OrdersLineFacturas(Ind).Qty = AuxLastQty * -1
                Me.OrdersLineFacturas(Ind).QtySpecified = True

                ' SALES PRICE


                If Me.CheckBoxPideBase.Checked Then
                    Me.OrdersLineFacturas(Ind).SalesPrice = InputBox("Base Imponible " & vDocumento & "  suma o resta un céntimo a " & Math.Round(TotalDiferencia, 2), "Ajuste")
                Else
                    Me.OrdersLineFacturas(Ind).SalesPrice = TotalDiferencia
                End If


                Me.OrdersLineFacturas(Ind).SalesPriceSpecified = True




                Me.OrdersLineTaxFacturas(Ind2).TaxCode = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), String) & "%"
                Me.OrdersLineTaxFacturas(Ind2).TaxPercent = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), Double)
                Me.OrdersLineTaxFacturas(Ind2).TaxPercentSpecified = True
                Me.ListBoxDebug.Items.Add(" ==> + Ajuste Necesario Base      : " & " Base : " & Math.Round(TotalDiferencia, 2) & " Impuesto :" & "0".PadRight(10, " ") & " Servicio =  " & mAxAjustaBase & "  Cantidad  " & CType(Me.OrdersLineFacturas(Ind).Qty, String))
                Me.ListBoxAjustesFacturas.Items.Add(" ==> + Ajuste Necesario Base      : " & " Base : " & Math.Round(TotalDiferencia, 2) & " Impuesto :" & "0".PadRight(10, " ") & " Servicio =  " & mAxAjustaBase & "  Cantidad  " & CType(Me.OrdersLineFacturas(Ind).Qty, String))

                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1




            End While
            Me.DbAux.mDbLector.Close()


        Catch ex As Exception

            MsgBox(ex.InnerException)
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Function CalculaImpuestosAxVisual(ByVal vDocumento As String, ByVal vValor As Double) As Double
        Try

            Dim result As Double

            SQL = "SELECT   "
          
            SQL += "ROUND((SUM (ROUND (asnt_lin_vliq, 2))  *  asnt_lin_tiimp ) / 100,2) AS RESULT"
    

            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_LIN_DOCU = '" & vDocumento & "'"
            SQL += " AND TH_ASNT.ASNT_DOCU_VALO = " & vValor
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"
            ' SOLO con ajuste necesario
            SQL += " AND ASNT_AX_STATUS =  0 "
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "
            SQL += " GROUP BY ASNT_LIN_DOCU,ASNT_LIN_TIIMP "
            SQL += " ORDER BY ASNT_LIN_DOCU ASC "

            Me.DbAux.TraerLector(SQL)

            While Me.DbAux.mDbLector.Read
                result = result + Me.DbAux.mDbLector.Item("RESULT")
            End While
            Me.DbAux.mDbLector.Close()

            Return result


        Catch ex As Exception

            MsgBox(ex.Message)
        End Try
    End Function


    Private Sub CalculaAjusteRedondeoImpuestoAxNuevo(ByVal vDocumento As String, ByVal vValor As Double)
        Try

            Dim TotalDiferencia As Double
            Dim Ind2 As Integer

            Dim AuxLastQty As Integer

            SQL = "SELECT   ASNT_LIN_TIIMP, ROUND (SUM (VALORIMPUESTO), 2), SUM (IMPUESTOAX), "
            SQL += "         ROUND (SUM (VALORIMPUESTO) - SUM (IMPUESTOAX), 2) AS DIREFRENCIAIMPUESTO, "
            SQL += "           ROUND (SUM (VALORIMPUESTO) - SUM (IMPUESTOAX), 2) "
            SQL += "         * 100 "
            SQL += "         / ASNT_LIN_TIIMP AS AJUSTE "
            SQL += "    FROM (SELECT   ASNT_LINEA, ' AJUSTE DE IMPUESTO DETALLE', "
            SQL += "                   ASNT_LIN_DOCU AS DOCUMENTO, "
            SQL += "                   NVL (ASNT_LIN_TIIMP, 0) AS ASNT_LIN_TIIMP, "
            SQL += "                   SUM (ASNT_LIN_IMP1) AS VALORIMPUESTO, "
            SQL += "                   ROUND (SUM (ASNT_LIN_VLIQ), 2) AS VALORLIQUIDO, "
            SQL += "                   SUM (ROUND (ASNT_LIN_IMP1, 2)) AS VALORIMPUESTOR, "
            SQL += "                   SUM (ROUND (ASNT_LIN_VLIQ, 2)) AS VALORLIQUIDOR, "
            SQL += "                     ROUND (SUM (ASNT_LIN_VLIQ), 2) "
            SQL += "                   - SUM (ROUND (ASNT_LIN_VLIQ, 2)) AS DIFERENCIALIQUIDO, "
            SQL += "                   ROUND (  (SUM (ROUND (ASNT_LIN_VLIQ, 2)) * ASNT_LIN_TIIMP) "
            SQL += "                          / 100, "
            SQL += "                          2 "
            SQL += "                         ) AS IMPUESTOAX, "
            SQL += "                     ROUND (SUM (ASNT_LIN_IMP1), 2) "
            SQL += "                   - ROUND (  (SUM (ROUND (ASNT_LIN_VLIQ, 2)) * ASNT_LIN_TIIMP "
            SQL += "                              ) "
            SQL += "                            / 100, "
            SQL += "                            2 "
            SQL += "                           ) AS DIFERENCIAIMPUESTO, "
            SQL += "                     (  ROUND (SUM (ASNT_LIN_IMP1), 2) "
            SQL += "                      - ROUND (  (  SUM (ROUND (ASNT_LIN_VLIQ, 2)) "
            SQL += "                                  * ASNT_LIN_TIIMP "
            SQL += "                                 ) "
            SQL += "                               / 100, "
            SQL += "                               2 "
            SQL += "                              ) "
            SQL += "                     ) "
            SQL += "                   * 100 "
            SQL += "                   / ASNT_LIN_TIIMP AS AJUSTE, "
            SQL += "                     ROUND (SUM (ASNT_LIN_IMP1), 2) "
            SQL += "                   - ROUND (  (SUM (ROUND (ASNT_LIN_VLIQ, 2)) * ASNT_LIN_TIIMP "
            SQL += "                              ) "
            SQL += "                            / 100, "
            SQL += "                            2 "
            SQL += "                           ) AS DIFERENCIA, "
            SQL += "                   ROUND (SUM (ASNT_LIN_IMP1), 2) AS AAAAA, "
            SQL += "                   ROUND (  (SUM (ROUND (ASNT_LIN_VLIQ, 2)) * ASNT_LIN_TIIMP) "
            SQL += "                          / 100, "
            SQL += "                          2 "
            SQL += "                         ) AS BBBBB "
            SQL += "              FROM TH_ASNT "

            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_LIN_DOCU = '" & vDocumento & "'"
            SQL += " AND TH_ASNT.ASNT_DOCU_VALO = " & vValor

            SQL += "               AND ASNT_WEBSERVICE_NAME = "
            SQL += "                                         'SAT_NHCreateSalesOrdersQueryService' "
            SQL += "               AND ASNT_AX_STATUS = 0 "
            SQL += "          GROUP BY ASNT_LIN_DOCU, ASNT_LIN_TIIMP, ASNT_LINEA) "

            SQL += " HAVING ROUND (SUM (VALORIMPUESTO) - SUM (IMPUESTOAX), 2) * 100  / ASNT_LIN_TIIMP  <> 0"

            SQL += "GROUP BY ASNT_LIN_TIIMP "



            Me.DbAux.TraerLector(SQL)

            While Me.DbAux.mDbLector.Read

                Me.ListBoxDebug.Items.Add("")
                Me.ListBoxDebug.Items.Add("Inicio de Ajustes por Diferencia en Impuestos")

                ReDim Preserve Me.OrdersLineFacturas(UBound(Me.OrdersLineFacturas) + 1)
                ReDim Preserve Me.OrdersLineTaxFacturas(UBound(Me.OrdersLineTaxFacturas) + 1)

                Ind = UBound(Me.OrdersLineFacturas)
                Ind2 = UBound(Me.OrdersLineTaxFacturas)

                Me.OrdersLineFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersLine_1
                Me.OrdersLineTaxFacturas(Ind2) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersTaxLine_1


                ' SERVICI0
                ' 29/06/2012

                'VIEJO

                'If CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), String) = "2" Then
                'Me.OrdersLineFacturas(Ind).ItemId = Me.TextBox1.Text
                'End If

                'If CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), String) = "5" Then
                'Me.OrdersLineFacturas(Ind).ItemId = Me.TextBox2.Text
                'End If

                ' NUEVO 
                SQL = "SELECT NVL(SATI_ARAX,'NULO') FROM TH_SATI WHERE SATI_TASA = " & CDbl(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"))

                Me.OrdersLineFacturas(Ind).ItemId = Me.DbAux2.EjecutaSqlScalar(SQL)




                If CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double) < 0 Then
                    TotalDiferencia = CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double) * -1
                    Me.OrdersLineFacturas(Ind).Qty = -1
                Else
                    TotalDiferencia = CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double)
                    Me.OrdersLineFacturas(Ind).Qty = 1
                End If



                AuxLastQty = Me.OrdersLineFacturas(Ind).Qty

                Me.OrdersLineFacturas(Ind).QtySpecified = True

                ' SALES PRICE
                Me.OrdersLineFacturas(Ind).SalesPrice = TotalDiferencia
                'Me.OrdersLineFacturas(Ind).SalesPrice = Math.Round(TotalDiferencia, 2, MidpointRounding.AwayFromZero)
                Me.OrdersLineFacturas(Ind).SalesPriceSpecified = True



                Me.ListBox1.Items.Add(Me.OrdersLineFacturas(Ind).ItemId & " *= " & TotalDiferencia)

                ' IMPUESTO
                Me.OrdersLineTaxFacturas(Ind2).TaxAmount = CType(Me.DbAux.mDbLector.Item("DIREFRENCIAIMPUESTO"), Double)
                Me.OrdersLineTaxFacturas(Ind2).TaxAmountSpecified = True

                ' BASE IMPONIBLE
                Me.OrdersLineTaxFacturas(Ind2).TaxBase = TotalDiferencia
                Me.OrdersLineTaxFacturas(Ind2).TaxBaseSpecified = True


                Me.OrdersLineTaxFacturas(Ind2).TaxCode = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), String) & "%"
                Me.OrdersLineTaxFacturas(Ind2).TaxPercent = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), Double)
                Me.OrdersLineTaxFacturas(Ind2).TaxPercentSpecified = True
                Me.ListBoxDebug.Items.Add(" ==> + Ajuste Necesario Impuesto  : " & " Base : " & Math.Round(TotalDiferencia, 2) & " Impuesto :" & CType(Me.OrdersLineTaxFacturas(Ind2).TaxAmount, String).PadRight(10, " ") & " Servicio =  " & CType(Me.OrdersLineFacturas(Ind).ItemId, String) & "  Cantidad  " & CType(Me.OrdersLineFacturas(Ind).Qty, String))
                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                '----------------------------------------------------------------------------------------------------------------
                'enviar otra vez la base del impuesto al articulo cojn el signo contrario y al articulo sin aimpuesto A02"
                '----------------------------------------------------------------------------------------------------------------

                ReDim Preserve Me.OrdersLineFacturas(UBound(Me.OrdersLineFacturas) + 1)
                Ind = UBound(Me.OrdersLineFacturas)


                Me.OrdersLineFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersLine_1


                ' SERVICI0

                Me.OrdersLineFacturas(Ind).ItemId = mAxAjustaBase


                If CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double) < 0 Then
                    TotalDiferencia = CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double) * -1
                Else
                    TotalDiferencia = CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double)
                End If

                ' cantidad invertida al movimiento anterior
                Me.OrdersLineFacturas(Ind).Qty = AuxLastQty * -1
                Me.OrdersLineFacturas(Ind).QtySpecified = True

                ' SALES PRICE
                Me.OrdersLineFacturas(Ind).SalesPrice = TotalDiferencia
                Me.OrdersLineFacturas(Ind).SalesPriceSpecified = True




                Me.OrdersLineTaxFacturas(Ind2).TaxCode = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), String) & "%"
                Me.OrdersLineTaxFacturas(Ind2).TaxPercent = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), Double)
                Me.OrdersLineTaxFacturas(Ind2).TaxPercentSpecified = True
                Me.ListBoxDebug.Items.Add(" ==> + Ajuste Necesario Impuesto  : " & " Base : " & Math.Round(TotalDiferencia, 2) & " Impuesto :" & "0".PadRight(10, " ") & " Servicio =  " & mAxAjustaBase & "  Cantidad  " & CType(Me.OrdersLineFacturas(Ind).Qty, String))
                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1



            End While
            Me.DbAux.mDbLector.Close()


        Catch ex As Exception

            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub CalculaAjusteRedondeoImpuestoAxNuevo3(ByVal vDocumento As String, ByVal vValor As Double)
        Try

            Dim TotalDiferencia As Double
            Dim Ind2 As Integer

            Dim AuxLastQty As Integer

            SQL = " SELECT  "
            SQL += "         ASNT_LIN_DOCU AS DOCUMENTO, "
            SQL += "         NVL (ASNT_LIN_TIIMP, 0) AS ASNT_LIN_TIIMP, "
            SQL += "         ROUND (ROUND (SUM (ASNT_LIN_VLIQ), 2) * ASNT_LIN_TIIMP / 100, 2) "
            SQL += "         - ROUND (SUM (ASNT_LIN_IMP1), 2) "
            SQL += "            AS DIFERENCIAIMPUESTO, "
            SQL += "         ROUND ( "
            SQL += "            (ROUND (ROUND (SUM (ASNT_LIN_VLIQ), 2) * ASNT_LIN_TIIMP / 100, 2) "
            SQL += "             - ROUND (SUM (ASNT_LIN_IMP1), 2)) "
            SQL += "            * 100 "
            SQL += "            / ASNT_LIN_TIIMP, "
            SQL += "            2) "
            SQL += "            AS AJUSTE "
            SQL += "    FROM TH_ASNT "



            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_LIN_DOCU = '" & vDocumento & "'"
            SQL += " AND TH_ASNT.ASNT_DOCU_VALO = " & vValor

            SQL += "               AND ASNT_WEBSERVICE_NAME = "
            SQL += "                                         'SAT_NHCreateSalesOrdersQueryService' "
            SQL += "               AND ASNT_AX_STATUS = 0 "


            ' SOLO SI AJUSTE ES MAYOR QUE ZERO
            SQL += " HAVING ROUND ( "
            SQL += "            (ROUND (ROUND (SUM (ASNT_LIN_VLIQ), 2) * ASNT_LIN_TIIMP / 100, 2) "
            SQL += "             - ROUND (SUM (ASNT_LIN_IMP1), 2)) "
            SQL += "            * 100 "
            SQL += "            / ASNT_LIN_TIIMP, "
            SQL += "            2) > 0 "
            SQL += "GROUP BY ASNT_LIN_DOCU, ASNT_LIN_TIIMP "



            Me.DbAux.TraerLector(SQL)

            While Me.DbAux.mDbLector.Read

                Me.ListBoxDebug.Items.Add("")
                Me.ListBoxDebug.Items.Add("Inicio de Ajustes por Diferencia en Impuestos")

                ReDim Preserve Me.OrdersLineFacturas(UBound(Me.OrdersLineFacturas) + 1)
                ReDim Preserve Me.OrdersLineTaxFacturas(UBound(Me.OrdersLineTaxFacturas) + 1)

                Ind = UBound(Me.OrdersLineFacturas)
                Ind2 = UBound(Me.OrdersLineTaxFacturas)

                Me.OrdersLineFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersLine_1
                Me.OrdersLineTaxFacturas(Ind2) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersTaxLine_1


                ' NUEVO 
                SQL = "SELECT NVL(SATI_ARAX,'NULO') FROM TH_SATI WHERE SATI_TASA = " & CDbl(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"))

                Me.OrdersLineFacturas(Ind).ItemId = Me.DbAux2.EjecutaSqlScalar(SQL)
                ' ---------------------------------------------------------------------------------------

                If CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double) < 0 Then
                    TotalDiferencia = CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double) * -1
                    Me.OrdersLineFacturas(Ind).Qty = -1
                Else
                    TotalDiferencia = CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double)
                    Me.OrdersLineFacturas(Ind).Qty = 1
                End If


                Me.ListBox1.Items.Add(Me.OrdersLineFacturas(Ind).ItemId & " = " & TotalDiferencia)

                AuxLastQty = Me.OrdersLineFacturas(Ind).Qty

                Me.OrdersLineFacturas(Ind).QtySpecified = True

                ' SALES PRICE
                Me.OrdersLineFacturas(Ind).SalesPrice = TotalDiferencia
                'Me.OrdersLineFacturas(Ind).SalesPrice = Math.Round(TotalDiferencia, 2, MidpointRounding.AwayFromZero)
                Me.OrdersLineFacturas(Ind).SalesPriceSpecified = True



                ' IMPUESTO
                Me.OrdersLineTaxFacturas(Ind2).TaxAmount = CType(Me.DbAux.mDbLector.Item("DIFERENCIAIMPUESTO"), Double)
                Me.OrdersLineTaxFacturas(Ind2).TaxAmountSpecified = True

                ' BASE IMPONIBLE
                Me.OrdersLineTaxFacturas(Ind2).TaxBase = TotalDiferencia
                Me.OrdersLineTaxFacturas(Ind2).TaxBaseSpecified = True


                Me.OrdersLineTaxFacturas(Ind2).TaxCode = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), String) & "%"
                Me.OrdersLineTaxFacturas(Ind2).TaxPercent = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), Double)
                Me.OrdersLineTaxFacturas(Ind2).TaxPercentSpecified = True
                Me.ListBoxDebug.Items.Add(" ==> + Ajuste Necesario Impuesto  : " & " Base : " & Math.Round(TotalDiferencia, 2) & " Impuesto :" & CType(Me.OrdersLineTaxFacturas(Ind2).TaxAmount, String).PadRight(10, " ") & " Servicio =  " & CType(Me.OrdersLineFacturas(Ind).ItemId, String) & "  Cantidad  " & CType(Me.OrdersLineFacturas(Ind).Qty, String))
                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                '----------------------------------------------------------------------------------------------------------------
                'enviar otra vez la base del impuesto al articulo cojn el signo contrario y al articulo sin aimpuesto A02"
                '----------------------------------------------------------------------------------------------------------------

                ReDim Preserve Me.OrdersLineFacturas(UBound(Me.OrdersLineFacturas) + 1)
                Ind = UBound(Me.OrdersLineFacturas)


                Me.OrdersLineFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersLine_1


                ' SERVICI0

                Me.OrdersLineFacturas(Ind).ItemId = mAxAjustaBase


                If CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double) < 0 Then
                    TotalDiferencia = CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double) * -1
                Else
                    TotalDiferencia = CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double)
                End If

                ' cantidad invertida al movimiento anterior
                Me.OrdersLineFacturas(Ind).Qty = AuxLastQty * -1
                Me.OrdersLineFacturas(Ind).QtySpecified = True

                ' SALES PRICE
                Me.OrdersLineFacturas(Ind).SalesPrice = TotalDiferencia
                Me.OrdersLineFacturas(Ind).SalesPriceSpecified = True




                Me.OrdersLineTaxFacturas(Ind2).TaxCode = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), String) & "%"
                Me.OrdersLineTaxFacturas(Ind2).TaxPercent = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), Double)
                Me.OrdersLineTaxFacturas(Ind2).TaxPercentSpecified = True
                Me.ListBoxDebug.Items.Add(" ==> + Ajuste Necesario Base      : " & " Base : " & Math.Round(TotalDiferencia, 2) & " Impuesto :" & "0".PadRight(10, " ") & " Servicio =  " & mAxAjustaBase & "  Cantidad  " & CType(Me.OrdersLineFacturas(Ind).Qty, String))

                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1




            End While
            Me.DbAux.mDbLector.Close()



        Catch ex As Exception

            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub CalculaAjusteRedondeoImpuestoAxNuevo4(ByVal vDocumento As String, ByVal vValor As Double)
        Try

            Dim TotalDiferencia As Double
            Dim Ind2 As Integer

            Dim AuxLastQty As Integer

            SQL = "  SELECT ASNT_LIN_DOCU AS DOCUMENTO, "
            SQL += "         NVL (ASNT_LIN_TIIMP, 0) AS ASNT_LIN_TIIMP, "
            SQL += "         ROUND (ROUND (SUM (ASNT_LIN_VLIQ), 2) * ASNT_LIN_TIIMP / 100, 2) "
            SQL += "         - ROUND (SUM (ASNT_LIN_IMP1), 2) "
            SQL += "            AS DIFERENCIAIMPUESTO, "
            SQL += "         ROUND ( "
            SQL += "            (ROUND (ROUND (SUM (ASNT_LIN_VLIQ), 2) * ASNT_LIN_TIIMP / 100, 2) "
            SQL += "             - ROUND (SUM (ASNT_LIN_IMP1), 2)) "
            SQL += "            * 100 "
            SQL += "            / ASNT_LIN_TIIMP, "
            SQL += "            2) "
            SQL += "            AS AJUSTE, "
            SQL += "         ROUND (SUM (ROUND ( (ASNT_LIN_VLIQ), 2)), 2) AS BASEAX, "
            SQL += "         ROUND ( "
            SQL += "            ROUND (SUM (ROUND ( (ASNT_LIN_VLIQ), 2)), 2) * ASNT_LIN_TIIMP / 100, "
            SQL += "            2) "
            SQL += "            AS IMPUESTOAX, "
            SQL += "         ROUND (SUM (ROUND ( (ASNT_LIN_VLIQ), 2)), 2) "
            SQL += "         + ROUND ( "
            SQL += "                ROUND (SUM (ROUND ( (ASNT_LIN_VLIQ), 2)), 2) "
            SQL += "              * ASNT_LIN_TIIMP "
            SQL += "              / 100, "
            SQL += "              2) "
            SQL += "            AS TOTALAX, "
            SQL += "         ASNT_DOCU_VALO "
            SQL += "         - (ROUND (SUM (ROUND ( (ASNT_LIN_VLIQ), 2)), 2) "
            SQL += "            + ROUND ( "
            SQL += "                   ROUND (SUM (ROUND ( (ASNT_LIN_VLIQ), 2)), 2) "
            SQL += "                 * ASNT_LIN_TIIMP "
            SQL += "                 / 100, "
            SQL += "                 2)) "
            SQL += "            AS DIFERENCIA, "
            SQL += "         ROUND ( "
            SQL += "            (ASNT_DOCU_VALO "
            SQL += "             - (ROUND (SUM (ROUND ( (ASNT_LIN_VLIQ), 2)), 2) "
            SQL += "                + ROUND ( "
            SQL += "                       ROUND (SUM (ROUND ( (ASNT_LIN_VLIQ), 2)), 2) "
            SQL += "                     * ASNT_LIN_TIIMP "
            SQL += "                     / 100, "
            SQL += "                     2))) "
            SQL += "            * ASNT_LIN_TIIMP "
            SQL += "            / (100 + ASNT_LIN_TIIMP), "
            SQL += "            2) "
            SQL += "            AS IMPUESTODEDIFERENCIA, "
            SQL += "         (ASNT_DOCU_VALO "
            SQL += "          - (ROUND (SUM (ROUND ( (ASNT_LIN_VLIQ), 2)), 2) "
            SQL += "             + ROUND ( "
            SQL += "                    ROUND (SUM (ROUND ( (ASNT_LIN_VLIQ), 2)), 2) "
            SQL += "                  * ASNT_LIN_TIIMP "
            SQL += "                  / 100, "
            SQL += "                  2))) "
            SQL += "         - (ROUND ( "
            SQL += "               (ASNT_DOCU_VALO "
            SQL += "                - (ROUND (SUM (ROUND ( (ASNT_LIN_VLIQ), 2)), 2) "
            SQL += "                   + ROUND ( "
            SQL += "                          ROUND (SUM (ROUND ( (ASNT_LIN_VLIQ), 2)), 2) "
            SQL += "                        * ASNT_LIN_TIIMP "
            SQL += "                        / 100, "
            SQL += "                        2))) "
            SQL += "               * ASNT_LIN_TIIMP "
            SQL += "               / (100 + ASNT_LIN_TIIMP), "
            SQL += "               2)) "
            SQL += "            AS BASEDIFERENCIA "
            SQL += "    FROM TH_ASNT "



            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_LIN_DOCU = '" & vDocumento & "'"
            SQL += " AND TH_ASNT.ASNT_DOCU_VALO = " & vValor

            SQL += "               AND ASNT_WEBSERVICE_NAME = "
            SQL += "                                         'SAT_NHCreateSalesOrdersQueryService' "
            SQL += "               AND ASNT_AX_STATUS = 0 "

            SQL += "GROUP BY ASNT_LIN_DOCU, ASNT_LIN_TIIMP, ASNT_DOCU_VALO "




            Me.DbAux.TraerLector(SQL)

            While Me.DbAux.mDbLector.Read

                Me.ListBoxDebug.Items.Add("")
                Me.ListBoxDebug.Items.Add("Inicio de Ajustes por Diferencia en Impuestos")

                ReDim Preserve Me.OrdersLineFacturas(UBound(Me.OrdersLineFacturas) + 1)
                ReDim Preserve Me.OrdersLineTaxFacturas(UBound(Me.OrdersLineTaxFacturas) + 1)

                Ind = UBound(Me.OrdersLineFacturas)
                Ind2 = UBound(Me.OrdersLineTaxFacturas)

                Me.OrdersLineFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersLine_1
                Me.OrdersLineTaxFacturas(Ind2) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersTaxLine_1


                ' NUEVO 
                SQL = "SELECT NVL(SATI_ARAX,'NULO') FROM TH_SATI WHERE SATI_TASA = " & CDbl(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"))

                Me.OrdersLineFacturas(Ind).ItemId = Me.DbAux2.EjecutaSqlScalar(SQL)
                ' ---------------------------------------------------------------------------------------

                If CType(Me.DbAux.mDbLector.Item("AJUSTE"), Double) < 0 Then
                    TotalDiferencia = CType(Me.DbAux.mDbLector.Item("BASEDIFERENCIA"), Double) * -1
                    Me.OrdersLineFacturas(Ind).Qty = -1
                Else
                    TotalDiferencia = CType(Me.DbAux.mDbLector.Item("BASEDIFERENCIA"), Double)
                    Me.OrdersLineFacturas(Ind).Qty = 1
                End If


                Me.ListBox1.Items.Add(Me.OrdersLineFacturas(Ind).ItemId & " = " & TotalDiferencia)

                AuxLastQty = Me.OrdersLineFacturas(Ind).Qty

                Me.OrdersLineFacturas(Ind).QtySpecified = True

                ' SALES PRICE
                Me.OrdersLineFacturas(Ind).SalesPrice = TotalDiferencia
                'Me.OrdersLineFacturas(Ind).SalesPrice = Math.Round(TotalDiferencia, 2, MidpointRounding.AwayFromZero)
                Me.OrdersLineFacturas(Ind).SalesPriceSpecified = True



                ' IMPUESTO
                Me.OrdersLineTaxFacturas(Ind2).TaxAmount = CType(Me.DbAux.mDbLector.Item("IMPUESTODEDIFERENCIA"), Double)
                Me.OrdersLineTaxFacturas(Ind2).TaxAmountSpecified = True

                ' BASE IMPONIBLE
                Me.OrdersLineTaxFacturas(Ind2).TaxBase = TotalDiferencia
                Me.OrdersLineTaxFacturas(Ind2).TaxBaseSpecified = True


                Me.OrdersLineTaxFacturas(Ind2).TaxCode = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), String) & "%"
                Me.OrdersLineTaxFacturas(Ind2).TaxPercent = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), Double)
                Me.OrdersLineTaxFacturas(Ind2).TaxPercentSpecified = True
                Me.ListBoxDebug.Items.Add(" ==> + Ajuste Necesario Impuesto  : " & " Base : " & Math.Round(TotalDiferencia, 2) & " Impuesto :" & CType(Me.OrdersLineTaxFacturas(Ind2).TaxAmount, String).PadRight(10, " ") & " Servicio =  " & CType(Me.OrdersLineFacturas(Ind).ItemId, String) & "  Cantidad  " & CType(Me.OrdersLineFacturas(Ind).Qty, String))
                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1



            End While
            Me.DbAux.mDbLector.Close()



        Catch ex As Exception

            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub CalculaAjusteRedondeoImpuestoAxNuevo5(ByVal vDocumento As String, ByVal vValor As Double)
        Try

            Dim TotalDiferencia As Double
            Dim Ind2 As Integer

            Dim AuxLastQty As Integer

            Dim Ajustado As Boolean = False


            SQL = "SELECT  "
            SQL += "         ASNT_LIN_DOCU AS DOCUMENTO, "
            SQL += "         NVL (ASNT_LIN_TIIMP, 0) AS ASNT_LIN_TIIMP, "
            SQL += "          "
            SQL += "         SUM (ROUND (ASNT_LIN_VLIQ, 2)) AS VALORLIQUIDORAX, "
            SQL += "         ROUND (SUM (ASNT_LIN_VLIQ), 2) AS VALORLIQUIDONH, "
            SQL += "         ROUND (SUM (ASNT_LIN_VLIQ), 2) -  SUM (ROUND (ASNT_LIN_VLIQ, 2))  AS AJUSTEBASE, "

            SQL += "         ROUND(SUM(ROUND(ASNT_LIN_VLIQ,2)) * ASNT_LIN_TIIMP / 100 ,2) AS IMPUESTOAX, "
            SQL += "         ROUND(SUM(ASNT_LIN_IMP1),2) AS IMPUESTONH, "
            SQL += "         ROUND(SUM(ASNT_LIN_IMP1),2) - ROUND(SUM(ROUND(ASNT_LIN_VLIQ,2)) * ASNT_LIN_TIIMP / 100 ,2) AS AJUSTEIMPUESTO , "

            SQL += "         ROUND( (ROUND(SUM(ASNT_LIN_IMP1),2) - ROUND(SUM(ROUND(ASNT_LIN_VLIQ,2)) * ASNT_LIN_TIIMP / 100 ,2))  * 100 / ASNT_LIN_TIIMP,2) AS BASEAJUSTEIMPUESTO "

            SQL += "    FROM TH_ASNT "

            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_LIN_DOCU = '" & vDocumento & "'"
            SQL += " AND TH_ASNT.ASNT_DOCU_VALO = " & vValor

            SQL += "               AND ASNT_WEBSERVICE_NAME = "
            SQL += "                                         'SAT_NHCreateSalesOrdersQueryService' "
            SQL += "               AND ASNT_AX_STATUS = 0 "

            ' para que solo ajuste un impuesto ( mejor pedir por pantalla que impuesto ajustar en cuentos centimos
            SQL += "    and ASNT_LIN_TIIMP = 7 "

            SQL += "GROUP BY ASNT_LIN_DOCU, ASNT_LIN_TIIMP "



            Me.DbAux.TraerLector(SQL)

            While Me.DbAux.mDbLector.Read

                Me.ListBoxDebug.Items.Add("")
                Me.ListBoxDebug.Items.Add("Inicio de Ajustes por Diferencia en Impuestos")

                ReDim Preserve Me.OrdersLineFacturas(UBound(Me.OrdersLineFacturas) + 1)
                ReDim Preserve Me.OrdersLineTaxFacturas(UBound(Me.OrdersLineTaxFacturas) + 1)

                Ind = UBound(Me.OrdersLineFacturas)
                Ind2 = UBound(Me.OrdersLineTaxFacturas)

                Me.OrdersLineFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersLine_1
                Me.OrdersLineTaxFacturas(Ind2) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersTaxLine_1


                ' NUEVO 
                SQL = "SELECT NVL(SATI_ARAX,'NULO') FROM TH_SATI WHERE SATI_TASA = " & CDbl(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"))

                Me.OrdersLineFacturas(Ind).ItemId = Me.DbAux2.EjecutaSqlScalar(SQL)
                ' ---------------------------------------------------------------------------------------


                If CType(Me.DbAux.mDbLector.Item("AJUSTEIMPUESTO"), Double) < 0 Then
                    Me.OrdersLineFacturas(Ind).Qty = -1
                    If Me.CheckBoxRedondeaFactura.Checked Then

                        TotalDiferencia = CType(Me.DbAux.mDbLector.Item("BASEAJUSTEIMPUESTO"), Double) * -1
                    Else
                        If Ajustado = False Then
                            If CType(Me.DbAux.mDbLector.Item("AJUSTEIMPUESTO"), Double) <> 0 Then
                                TotalDiferencia = CType(Me.DbAux.mDbLector.Item("BASEAJUSTEIMPUESTO"), Double) + NumericUpDownredondeoBaseAjuste.Value * -1

                                ' ajustado es para que solo ajuste una de las bases caso de facturas con mas de un impuesto 
                                Ajustado = True
                            End If

                        End If

                    End If
                Else
                    Me.OrdersLineFacturas(Ind).Qty = 1
                    If Me.CheckBoxRedondeaFactura.Checked Then
                        TotalDiferencia = CType(Me.DbAux.mDbLector.Item("BASEAJUSTEIMPUESTO"), Double)
                    Else
                        If Ajustado = False Then
                            If CType(Me.DbAux.mDbLector.Item("AJUSTEIMPUESTO"), Double) <> 0 Then
                                TotalDiferencia = CType(Me.DbAux.mDbLector.Item("BASEAJUSTEIMPUESTO"), Double) + NumericUpDownredondeoBaseAjuste.Value
                                ' ajustado es para que solo ajuste una de las bases caso de facturas con mas de un impuesto 
                                Ajustado = True
                            End If
                        End If

                    End If
                End If





                Me.ListBox1.Items.Add(Me.OrdersLineFacturas(Ind).ItemId & " = " & TotalDiferencia)

                AuxLastQty = Me.OrdersLineFacturas(Ind).Qty

                Me.OrdersLineFacturas(Ind).QtySpecified = True

                ' SALES PRICE
                Me.OrdersLineFacturas(Ind).SalesPrice = TotalDiferencia
                'Me.OrdersLineFacturas(Ind).SalesPrice = Math.Round(TotalDiferencia, 2, MidpointRounding.AwayFromZero)
                Me.OrdersLineFacturas(Ind).SalesPriceSpecified = True



                ' IMPUESTO
                Me.OrdersLineTaxFacturas(Ind2).TaxAmount = CType(Me.DbAux.mDbLector.Item("AJUSTEIMPUESTO"), Double)
                Me.OrdersLineTaxFacturas(Ind2).TaxAmountSpecified = True

                ' BASE IMPONIBLE
                Me.OrdersLineTaxFacturas(Ind2).TaxBase = TotalDiferencia
                Me.OrdersLineTaxFacturas(Ind2).TaxBaseSpecified = True


                Me.OrdersLineTaxFacturas(Ind2).TaxCode = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), String) & "%"
                Me.OrdersLineTaxFacturas(Ind2).TaxPercent = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), Double)
                Me.OrdersLineTaxFacturas(Ind2).TaxPercentSpecified = True
                Me.ListBoxDebug.Items.Add(" ==> + Ajuste Necesario Impuesto  : " & " Base : " & Math.Round(TotalDiferencia, 2) & " Impuesto :" & CType(Me.OrdersLineTaxFacturas(Ind2).TaxAmount, String).PadRight(10, " ") & " Servicio =  " & CType(Me.OrdersLineFacturas(Ind).ItemId, String) & "  Cantidad  " & CType(Me.OrdersLineFacturas(Ind).Qty, String))
                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                '----------------------------------------------------------------------------------------------------------------
                'enviar otra vez la base del impuesto al articulo cojn el signo contrario y al articulo sin aimpuesto A02"
                '----------------------------------------------------------------------------------------------------------------

                ReDim Preserve Me.OrdersLineFacturas(UBound(Me.OrdersLineFacturas) + 1)
                Ind = UBound(Me.OrdersLineFacturas)


                Me.OrdersLineFacturas(Ind) = New WebReferenceFacturas.AxdEntity_SAT_NHCreateSalesOrdersLine_1


                ' SERVICI0

                Me.OrdersLineFacturas(Ind).ItemId = mAxAjustaBase


                If CType(Me.DbAux.mDbLector.Item("AJUSTEIMPUESTO"), Double) < 0 Then
                    If Me.CheckBoxRedondeaFactura.Checked Then
                        TotalDiferencia = CType(Me.DbAux.mDbLector.Item("BASEAJUSTEIMPUESTO"), Double) * -1
                    Else
                        TotalDiferencia = CType(Me.DbAux.mDbLector.Item("BASEAJUSTEIMPUESTO"), Double) + NumericUpDownredondeoBaseAjuste.Value * -1
                    End If
                Else
                    If Me.CheckBoxRedondeaFactura.Checked Then
                        TotalDiferencia = CType(Me.DbAux.mDbLector.Item("BASEAJUSTEIMPUESTO"), Double)
                    Else
                        TotalDiferencia = CType(Me.DbAux.mDbLector.Item("BASEAJUSTEIMPUESTO"), Double) + NumericUpDownredondeoBaseAjuste.Value
                    End If
                End If

                ' cantidad invertida al movimiento anterior
                Me.OrdersLineFacturas(Ind).Qty = AuxLastQty * -1
                Me.OrdersLineFacturas(Ind).QtySpecified = True

                ' SALES PRICE
                Me.OrdersLineFacturas(Ind).SalesPrice = TotalDiferencia
                Me.OrdersLineFacturas(Ind).SalesPriceSpecified = True




                Me.OrdersLineTaxFacturas(Ind2).TaxCode = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), String) & "%"
                Me.OrdersLineTaxFacturas(Ind2).TaxPercent = CType(Me.DbAux.mDbLector.Item("ASNT_LIN_TIIMP"), Double)
                Me.OrdersLineTaxFacturas(Ind2).TaxPercentSpecified = True
                Me.ListBoxDebug.Items.Add(" ==> + Ajuste Necesario Base      : " & " Base : " & Math.Round(TotalDiferencia, 2) & " Impuesto :" & "0".PadRight(10, " ") & " Servicio =  " & mAxAjustaBase & "  Cantidad  " & CType(Me.OrdersLineFacturas(Ind).Qty, String))

                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1




            End While
            Me.DbAux.mDbLector.Close()



        Catch ex As Exception

            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub AuditaProblemaCentimo(ByVal vDocummento As String)
        Try
            ' TRABAJANDO
            SQL = "SELECT   ASNT_AX_STATUS AS ESTADO, ASNT_F_VALOR AS FECHA, "
            SQL += "         ASNT_LIN_DOCU AS DOCUMENTO, SUM (ASNT_LIN_VLIQ) AS LIQUIDO, "
            SQL += "         SUM (ASNT_LIN_IMP1) AS IMPUESTO, "
            SQL += "         SUM (ASNT_LIN_VLIQ + ASNT_LIN_IMP1) AS TOTAL, "
            SQL += "         ASNT_DOCU_VALO AS ""VALOR  NEWHOTEL"", "
            SQL += "         ROUND (SUM (ASNT_LIN_VLIQ + ASNT_LIN_IMP1), 2) AS AUDITORIA, "
            SQL += "           ASNT_DOCU_VALO "
            SQL += "         - ROUND (SUM (ASNT_LIN_VLIQ + ASNT_LIN_IMP1), 2) AS RESULT "
            SQL += "    FROM TH_ASNT "
            SQL += "   WHERE TH_ASNT.ASNT_WEBSERVICE_NAME = 'SAT_NHCREATESALESORDERSQUERYSERVICE' "
            SQL += "     AND ASNT_AX_STATUS = 0 "
            SQL += "     AND ASNT_LIN_DOCU = '" & vDocummento & "'"
            SQL += "  HAVING ROUND (SUM (ASNT_LIN_VLIQ + ASNT_LIN_IMP1), 2) <> ASNT_DOCU_VALO "
            SQL += "GROUP BY ASNT_AX_STATUS, ASNT_F_VALOR, ASNT_LIN_DOCU, ASNT_DOCU_VALO "
            SQL += "ORDER BY ASNT_LIN_DOCU ASC "


            Me.DbAux.TraerLector(SQL)
            Me.DbWrite.IniciaTransaccion()
            While Me.DbAux.mDbLector.Read



                If Me.Db.mDbLector.Item("RESULT") = 0.01 Or Me.Db.mDbLector.Item("RESULT") = -0.01 Then


                    SQL = "UPDATE TH_ASNT "
                    SQL += " SET ASNT_DOCU_VALO = " & Me.Db.mDbLector.Item("AUDITORIA")
                    SQL += "   WHERE TH_ASNT.ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService' "
                    SQL += "   AND ASNT_AX_STATUS = 0 "
                    SQL += "   AND ASNT_LIN_DOCU = '" & Me.Db.mDbLector.Item("DOCUMENTO") & "'"

                    Me.ListBoxDebug.Items.Add("Problema redondeo")
                    Me.ListBoxDebug.Items.Add("Update =  " & CStr(Me.DbWrite.EjecutaSql(SQL)))
                End If

            End While
            Me.DbWrite.ConfirmaTransaccion()


        Catch ex As Exception
            Me.DbWrite.CancelaTransaccion()

        Finally
            Me.DbAux.mDbLector.Close()
        End Try
    End Sub
#End Region

#Region "COBROS"
    Private Sub AxProcesarCobros()
        Try

            '    Dim Ind As Integer


            Dim Primerregistro As Boolean = False
            Dim ControlFactura As String = ""

            Dim UltimaFactura As String = ""



            Me.WebCobros = New WebReferenceCobros.SAT_NHJournalCustPaymentQueryService

            '    Actualiza(Url)

            Me.WebCobros.Url = Me.mAxUrl & "SAT_NHJournalCustPaymentQueryService.asmx"


            ' Aactualiza TIME OUT

            If Me.NumericUpDownWebServiceTimeOut.Value > 0 Then
                If Me.CheckBoxTimeOut.Checked Then
                    Me.WebCobros.Timeout = Me.NumericUpDownWebServiceTimeOut.Value * 1000
                End If
            End If


            Me.TextBoxRutaWebServices.Text = Me.WebCobros.Url
            Me.TextBoxRutaWebServices.Update()

            Me.DocumentoContextoCobros = New WebReferenceCobros.DocumentContext
            Me.QweryOrdersCobros = New WebReferenceCobros.AxdSAT_NHJournalCustPaymentQuery


            'Me.WebCobros.Credentials = System.Net.CredentialCache.DefaultCredentials
            Me.WebCobros.Credentials = New System.Net.NetworkCredential(Me.mAxUserName, Me.mAxUserPwd, Me.mAxDomainName)

            '


            ' Me.DocumentoContextoCobros.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContextoCobros.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContextoCobros.DestinationEndpoint = Me.mAXDestinationEndPoint
            Me.DocumentoContextoCobros.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)




            '   Me.ListBoxDebug.Items.Add(" => MessageId            : " & Me.DocumentoContextoCobros.MessageId.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpoint       : " & Me.DocumentoContextoCobros.SourceEndpoint.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpointUser   : " & Me.DocumentoContextoCobros.SourceEndpointUser.ToString)
            Me.ListBoxDebug.Items.Add(" => DestinationEndpoint  : " & Me.DocumentoContextoCobros.DestinationEndpoint.ToString)


            ' Cobros en factura


            SQL = "SELECT ROWID, NVL(ASNT_LIN_DOCU,'?') AS DOCUMENTO,NVL(ASNT_CFCTA_COD,' ') AS FORMA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,NVL(ASNT_I_MONEMP,'0')  AS IMPORTE"
            SQL += " ,NVL(ASNT_AUXILIAR_STRING,'?') AS ASNT_AUXILIAR_STRING, ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI"
            SQL += " ,NVL(ASNT_FACTURA_NUMERO,'0') AS  ASNT_FACTURA_NUMERO ,NVL(ASNT_FACTURA_SERIE,'0') AS ASNT_FACTURA_SERIE "
            SQL += ",nvl(ASNT_DPTO_DESC,'NULL') AS DEPARTAMENTO "



            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "
            ' COBROS QUE NO SON ANTICIPOS 
            SQL += " AND ASNT_CFCTA_COD <>  '" & Me.mAxAnticipo & "'"
            ' SOLO SIN PROCESAR
            SQL += " ORDER BY ASNT_LIN_DOCU ASC "

            Me.Db.TraerLector(SQL)


            Primerregistro = True
            Elementos = 0

            Me.TreeViewDebug.Nodes.Add("==>  Cobros: " & Me.mFecha)
            While Me.Db.mDbLector.Read




                Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1).Nodes.Add(CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(50, " ") & CType(Me.Db.mDbLector.Item("IMPORTE"), String))
                Me.TreeViewDebug.SelectedNode = Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1)
                Me.TreeViewDebug.ExpandAll()
                Me.TreeViewDebug.Update()




                If Primerregistro = True Then
                    Primerregistro = False
                    Elementos = 0
                    Ind = 0
                    ReDim Me.OrdersLineCobros(Ind)

                Else                ' añade un elemento a array de lineas / Objeto
                    ReDim Preserve Me.OrdersLineCobros(UBound(Me.OrdersLineCobros) + 1)
                    Ind = UBound(Me.OrdersLineCobros)
                End If



                Me.OrdersLineCobros(Ind) = New WebReferenceCobros.AxdEntity_SAT_NHJournalCustPayment_1


                Me.OrdersLineCobros(Ind).InvoiceId = CType(Me.Db.mDbLector.Item("DOCUMENTO"), String)
                Me.OrdersLineCobros(Ind).PaymMode = CType(Me.Db.mDbLector.Item("FORMA"), String)


                Me.OrdersLineCobros(Ind).PrePayment = WebReferenceCobros.AxdExtType_NoYesId.No
                Me.OrdersLineCobros(Ind).PrePaymentSpecified = True



                Me.OrdersLineCobros(Ind).Amount = CType(Me.Db.mDbLector.Item("IMPORTE"), Double)
                Me.OrdersLineCobros(Ind).AmountSpecified = True


                Me.OrdersLineCobros(Ind).TransDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
                'Me.OrdersLineCobros(Ind).TransDateSpecified = True


                If (CStr(Me.Db.mDbLector.Item("DEPARTAMENTO"))) = "NULL" Then
                    Me.OrdersLineCobros(Ind).Departament = ""
                Else
                    Me.OrdersLineCobros(Ind).Departament = CStr(Me.Db.mDbLector.Item("DEPARTAMENTO"))
                End If


                Me.OrdersLineCobros(Ind).HotelId = Me.mHotelId

                Me.ListBoxDebug.Items.Add(" ==> Cobro          : " & CType(Me.Db.mDbLector.Item("DOCUMENTO"), String).PadRight(10, " ") & "  " & CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(60, " ") & CType(Me.OrdersLineCobros(Ind).Amount, String).PadRight(15, " ") & " CON " & CType(Me.Db.mDbLector.Item("FORMA"), String) & " Departamento = " & Me.OrdersLineCobros(Ind).Departament.ToString)
                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                Elementos = Me.OrdersLineCobros.GetLength(Dimension)
                Me.TextBoxDebug.Text = Elementos

                '       End If
                'Application.DoEvents()



            End While
            Me.Db.mDbLector.Close()





            If Elementos > 0 Then
                ' LLAMADA AL WEB SERVICE
                Me.QweryOrdersCobros.SAT_NHJournalCustPayment_1 = Me.OrdersLineCobros

                If Me.AxEnviaCobros(Me.DocumentoContextoCobros, Me.QweryOrdersCobros) = True Then
                    Me.ListBoxDebug.Items.Add(" <== OK: ")
                    Me.ListBoxDebug.Update()
                    Me.AXTrataenvioCobrosNormales(1, "OK")
                Else
                    Me.TotalErrores = Me.TotalErrores + 1
                    Me.AXTrataenvioCobrosNormales(0, Me.AxError)
                    Me.ListBoxDebug.Items.Add(" <== : " & Me.AxError)
                    'DEBUG
                    ' MsgBox(Me.AxError)
                    Me.ListBoxDebug.SelectedIndex = Me.ListBoxDebug.Items.Count - 1
                    Me.ListBoxDebug.Update()

                End If

            End If



        Catch ex As Exception
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service Cobros " & Me.WebCobros.Url & " : " & ex.Message)

            MsgBox(ex.Message)
            '     MsgBox(ex.ToString)
        End Try


    End Sub
    Private Sub AxProcesarCobrosParciales()
        ' PAT 35
        Try
            ' AQUO SE TRATAN LAS FACTURAS CON  UN SOLO COBRO 
            'las Facturas con mas de un cobro se envian llamando al web service agrupando por forma de cobro y factura

            '    Dim Ind As Integer


            Dim Primerregistro As Boolean = False
            Dim ControlFactura As String = ""

            Dim UltimaFactura As String = ""



            Me.WebCobros = New WebReferenceCobros.SAT_NHJournalCustPaymentQueryService

            '    Actualiza(Url)

            Me.WebCobros.Url = Me.mAxUrl & "SAT_NHJournalCustPaymentQueryService.asmx"


            ' Aactualiza TIME OUT

            If Me.NumericUpDownWebServiceTimeOut.Value > 0 Then
                If Me.CheckBoxTimeOut.Checked Then
                    Me.WebCobros.Timeout = Me.NumericUpDownWebServiceTimeOut.Value * 1000
                End If
            End If


            Me.TextBoxRutaWebServices.Text = Me.WebCobros.Url
            Me.TextBoxRutaWebServices.Update()

            Me.DocumentoContextoCobros = New WebReferenceCobros.DocumentContext
            Me.QweryOrdersCobros = New WebReferenceCobros.AxdSAT_NHJournalCustPaymentQuery


            'Me.WebCobros.Credentials = System.Net.CredentialCache.DefaultCredentials
            Me.WebCobros.Credentials = New System.Net.NetworkCredential(Me.mAxUserName, Me.mAxUserPwd, Me.mAxDomainName)

            '


            ' Me.DocumentoContextoCobros.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContextoCobros.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContextoCobros.DestinationEndpoint = Me.mAXDestinationEndPoint
            Me.DocumentoContextoCobros.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)




            '   Me.ListBoxDebug.Items.Add(" => MessageId            : " & Me.DocumentoContextoCobros.MessageId.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpoint       : " & Me.DocumentoContextoCobros.SourceEndpoint.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpointUser   : " & Me.DocumentoContextoCobros.SourceEndpointUser.ToString)
            Me.ListBoxDebug.Items.Add(" => DestinationEndpoint  : " & Me.DocumentoContextoCobros.DestinationEndpoint.ToString)


            ' Cobros en factura


            SQL = "SELECT ROWID, NVL(ASNT_LIN_DOCU,'?') AS DOCUMENTO,NVL(ASNT_CFCTA_COD,' ') AS FORMA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,NVL(ASNT_I_MONEMP,'0')  AS IMPORTE"
            SQL += " ,NVL(ASNT_AUXILIAR_STRING,'?') AS ASNT_AUXILIAR_STRING, ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI"
            SQL += " ,NVL(ASNT_FACTURA_NUMERO,'0') AS  ASNT_FACTURA_NUMERO ,NVL(ASNT_FACTURA_SERIE,'0') AS ASNT_FACTURA_SERIE "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "
            ' COBROS QUE NO SON ANTICIPOS 
            SQL += " AND ASNT_CFCTA_COD <>  '" & Me.mAxAnticipo & "'"



            SQL += " AND ASNT_LIN_DOCU IN("

            ' FACTURAS CON UN SOLO COBRO


            SQL += "  SELECT  NVL (ASNT_LIN_DOCU, '?') AS DOCUMENTO "
            SQL += "    FROM TH_ASNT "
            SQL += "   WHERE     ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += "         AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += "         AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "         AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService' "
            SQL += "         AND ASNT_AX_STATUS = 0 "
            SQL += " AND ASNT_CFCTA_COD <>  '" & Me.mAxAnticipo & "'"
            SQL += " GROUP BY ASNT_LIN_DOCU "
            SQL += "  HAVING COUNT (*) = 1 )"

            ' O que sea la liquidacion de puntos de venta ( simepre tiene mas de un cobro ) 

            '  SQL += " OR  ASNT_LIN_DOCU NOT IN("

            'SQL += " SELECT  "
            'SQL += "       NVL (ASNT_LIN_DOCU, '?') AS DOCUMENTO "
            'SQL += "      "
            'SQL += "  FROM TH_ASNT "
            'SQL += " WHERE     ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            'SQL += "       AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            'SQL += "       AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            'SQL += "       AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService' "
            'SQL += "       AND ASNT_AX_STATUS = 0 "
            'SQL += "       AND ASNT_CFCTA_COD = '" & Me.mAxAnticipo & "'))"


            SQL += " ORDER BY ASNT_LIN_DOCU ASC "

            Me.Db.TraerLector(SQL)


            Primerregistro = True
            Elementos = 0

            Me.TreeViewDebug.Nodes.Add("==>  Cobros: " & Me.mFecha)
            While Me.Db.mDbLector.Read




                Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1).Nodes.Add(CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(50, " ") & CType(Me.Db.mDbLector.Item("IMPORTE"), String))
                Me.TreeViewDebug.SelectedNode = Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1)
                Me.TreeViewDebug.ExpandAll()
                Me.TreeViewDebug.Update()




                If Primerregistro = True Then
                    Primerregistro = False
                    Elementos = 0
                    Ind = 0
                    ReDim Me.OrdersLineCobros(Ind)

                Else                ' añade un elemento a array de lineas / Objeto
                    ReDim Preserve Me.OrdersLineCobros(UBound(Me.OrdersLineCobros) + 1)
                    Ind = UBound(Me.OrdersLineCobros)
                End If



                Me.OrdersLineCobros(Ind) = New WebReferenceCobros.AxdEntity_SAT_NHJournalCustPayment_1


                Me.OrdersLineCobros(Ind).InvoiceId = CType(Me.Db.mDbLector.Item("DOCUMENTO"), String)
                Me.OrdersLineCobros(Ind).PaymMode = CType(Me.Db.mDbLector.Item("FORMA"), String)


                Me.OrdersLineCobros(Ind).PrePayment = WebReferenceCobros.AxdExtType_NoYesId.No
                Me.OrdersLineCobros(Ind).PrePaymentSpecified = True



                Me.OrdersLineCobros(Ind).Amount = CType(Me.Db.mDbLector.Item("IMPORTE"), Double)
                Me.OrdersLineCobros(Ind).AmountSpecified = True


                Me.OrdersLineCobros(Ind).TransDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
                'Me.OrdersLineCobros(Ind).TransDateSpecified = True


                Me.OrdersLineCobros(Ind).HotelId = Me.mHotelId


                Me.ListBoxDebug.Items.Add(" ==> Cobro          : " & CType(Me.Db.mDbLector.Item("DOCUMENTO"), String).PadRight(10, " ") & "  " & CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(60, " ") & CType(Me.OrdersLineCobros(Ind).Amount, String).PadRight(15, " ") & " CON " & CType(Me.Db.mDbLector.Item("FORMA"), String))
                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                Elementos = Me.OrdersLineCobros.GetLength(Dimension)
                Me.TextBoxDebug.Text = Elementos

                '       End If
                'Application.DoEvents()



            End While
            Me.Db.mDbLector.Close()





            If Elementos > 0 Then
                ' LLAMADA AL WEB SERVICE
                Me.QweryOrdersCobros.SAT_NHJournalCustPayment_1 = Me.OrdersLineCobros

                If Me.AxEnviaCobros(Me.DocumentoContextoCobros, Me.QweryOrdersCobros) = True Then
                    Me.ListBoxDebug.Items.Add(" <== OK: ")
                    Me.ListBoxDebug.Update()
                    Me.AXTrataenvioCobrosNormales(1, "OK")
                Else
                    Me.TotalErrores = Me.TotalErrores + 1
                    Me.AXTrataenvioCobrosNormales(0, Me.AxError)
                    Me.ListBoxDebug.Items.Add(" <== : " & Me.AxError)
                    'DEBUG
                    ' MsgBox(Me.AxError)
                    Me.ListBoxDebug.SelectedIndex = Me.ListBoxDebug.Items.Count - 1
                    Me.ListBoxDebug.Update()

                End If

            End If



        Catch ex As Exception
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service Cobros " & Me.WebCobros.Url & " : " & ex.Message)

            MsgBox(ex.Message)
            '     MsgBox(ex.ToString)
        End Try


    End Sub
    Private Sub AxProcesarCobrosParciales2()
        ' PAT 35
        Try
            ' AQUO SE TRATAB LAS FACTURAS CON MAS DE UN COBRO 
            'las Facturas con mas de un cobro se envian llamando al web service agrupando por forma de cobro y factura
            '    Dim Ind As Integer


            Dim Primerregistro As Boolean = False
            Dim ControlFactura As String = ""

            Dim UltimaFactura As String = ""


            Dim Factura As String = ""
            Dim Forma As String = ""



            Me.WebCobros = New WebReferenceCobros.SAT_NHJournalCustPaymentQueryService

            '    Actualiza(Url)

            Me.WebCobros.Url = Me.mAxUrl & "SAT_NHJournalCustPaymentQueryService.asmx"


            ' Aactualiza TIME OUT

            If Me.NumericUpDownWebServiceTimeOut.Value > 0 Then
                If Me.CheckBoxTimeOut.Checked Then
                    Me.WebCobros.Timeout = Me.NumericUpDownWebServiceTimeOut.Value * 1000
                End If
            End If


            Me.TextBoxRutaWebServices.Text = Me.WebCobros.Url
            Me.TextBoxRutaWebServices.Update()

            Me.DocumentoContextoCobros = New WebReferenceCobros.DocumentContext
            Me.QweryOrdersCobros = New WebReferenceCobros.AxdSAT_NHJournalCustPaymentQuery


            'Me.WebCobros.Credentials = System.Net.CredentialCache.DefaultCredentials
            Me.WebCobros.Credentials = New System.Net.NetworkCredential(Me.mAxUserName, Me.mAxUserPwd, Me.mAxDomainName)

            '


            ' Me.DocumentoContextoCobros.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContextoCobros.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContextoCobros.DestinationEndpoint = Me.mAXDestinationEndPoint
            Me.DocumentoContextoCobros.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)




            '   Me.ListBoxDebug.Items.Add(" => MessageId            : " & Me.DocumentoContextoCobros.MessageId.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpoint       : " & Me.DocumentoContextoCobros.SourceEndpoint.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpointUser   : " & Me.DocumentoContextoCobros.SourceEndpointUser.ToString)
            Me.ListBoxDebug.Items.Add(" => DestinationEndpoint  : " & Me.DocumentoContextoCobros.DestinationEndpoint.ToString)


            ' Cobros en factura


            SQL = "SELECT ROWID, NVL(ASNT_LIN_DOCU,'?') AS DOCUMENTO,NVL(ASNT_CFCTA_COD,' ') AS FORMA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,NVL(ASNT_I_MONEMP,'0')  AS IMPORTE"
            SQL += " ,NVL(ASNT_AUXILIAR_STRING,'?') AS ASNT_AUXILIAR_STRING, ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI"
            SQL += " ,NVL(ASNT_FACTURA_NUMERO,'0') AS  ASNT_FACTURA_NUMERO ,NVL(ASNT_FACTURA_SERIE,'0') AS ASNT_FACTURA_SERIE "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "
            ' COBROS QUE NO SON ANTICIPOS 
            SQL += " AND ASNT_CFCTA_COD <>  '" & Me.mAxAnticipo & "'"



            SQL += " AND  ASNT_LIN_DOCU IN("

            ' FACTURAS CON MAS DE UN SOLO COBRO

            SQL += "  SELECT  NVL (ASNT_LIN_DOCU, '?') AS DOCUMENTO "
            SQL += "    FROM TH_ASNT "
            SQL += "   WHERE     ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += "         AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += "         AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += "         AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService' "
            SQL += "         AND ASNT_AX_STATUS = 0 "
            SQL += " AND ASNT_CFCTA_COD <>  '" & Me.mAxAnticipo & "'"
            SQL += " GROUP BY ASNT_LIN_DOCU "
            SQL += "  HAVING COUNT (*) > 1 )"

            ' Y QUE  TENGAN ANTICIPOS 

            'SQL += " AND  ASNT_LIN_DOCU IN("

            'SQL += " SELECT  "
            'SQL += "       NVL (ASNT_LIN_DOCU, '?') AS DOCUMENTO "
            'SQL += "      "
            'SQL += "  FROM TH_ASNT "
            'SQL += " WHERE     ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            'SQL += "       AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            'SQL += "       AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            'SQL += "       AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService' "
            'SQL += "       AND ASNT_AX_STATUS = 0 "
            'SQL += "       AND ASNT_CFCTA_COD = '" & Me.mAxAnticipo & "'))"



            SQL += " ORDER BY ASNT_LIN_DOCU,ASNT_CFCTA_COD ASC "

            Me.Db.TraerLector(SQL)


            Primerregistro = True
            Elementos = 0

            Me.TreeViewDebug.Nodes.Add("==>  Cobros: " & Me.mFecha)
            While Me.Db.mDbLector.Read




                If Primerregistro = True Then
                    Primerregistro = False
                    Factura = CType(Me.Db.mDbLector.Item("DOCUMENTO"), String)
                    Forma = CType(Me.Db.mDbLector.Item("FORMA"), String)
                    Elementos = 0
                    Ind = 0
                    ReDim Me.OrdersLineCobros(Ind)

                Else                ' añade un elemento a array de lineas / Objeto
                    ReDim Preserve Me.OrdersLineCobros(UBound(Me.OrdersLineCobros) + 1)
                    Ind = UBound(Me.OrdersLineCobros)
                End If


                '' LAMA AL WEB SERVICE SI CAMBIA LA FACTURA O LA FORMA DE COBRO


                If Factura <> CType(Me.Db.mDbLector.Item("DOCUMENTO"), String) Or Forma <> CType(Me.Db.mDbLector.Item("FORMA"), String) Then

                    If Elementos > 0 Then
                        ' LLAMADA AL WEB SERVICE
                        Me.QweryOrdersCobros.SAT_NHJournalCustPayment_1 = Me.OrdersLineCobros

                        If Me.AxEnviaCobros(Me.DocumentoContextoCobros, Me.QweryOrdersCobros) = True Then
                            Me.ListBoxDebug.Items.Add(" <== OK: ")
                            Me.ListBoxDebug.Update()
                            Me.AXTrataenvioCobrosNormalesParciales(1, "OK", Factura, Forma)
                        Else
                            Me.TotalErrores = Me.TotalErrores + 1
                            Me.AXTrataenvioCobrosNormalesParciales(0, Me.AxError, Factura, Forma)
                            Me.ListBoxDebug.Items.Add(" <== : " & Me.AxError)
                            Me.ListBoxDebug.Update()

                        End If
                    End If
                    ' RECARGA AUXILIARES
                    Factura = CType(Me.Db.mDbLector.Item("DOCUMENTO"), String)
                    Forma = CType(Me.Db.mDbLector.Item("FORMA"), String)
                    Elementos = 0
                    Ind = 0
                    ReDim Me.OrdersLineCobros(Ind)

                End If





                Me.OrdersLineCobros(Ind) = New WebReferenceCobros.AxdEntity_SAT_NHJournalCustPayment_1


                Me.OrdersLineCobros(Ind).InvoiceId = CType(Me.Db.mDbLector.Item("DOCUMENTO"), String)
                Me.OrdersLineCobros(Ind).PaymMode = CType(Me.Db.mDbLector.Item("FORMA"), String)


                Me.OrdersLineCobros(Ind).PrePayment = WebReferenceCobros.AxdExtType_NoYesId.No
                Me.OrdersLineCobros(Ind).PrePaymentSpecified = True



                Me.OrdersLineCobros(Ind).Amount = CType(Me.Db.mDbLector.Item("IMPORTE"), Double)
                Me.OrdersLineCobros(Ind).AmountSpecified = True


                Me.OrdersLineCobros(Ind).TransDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
                'Me.OrdersLineCobros(Ind).TransDateSpecified = True

                Me.OrdersLineCobros(Ind).HotelId = Me.mHotelId


                Me.ListBoxDebug.Items.Add(" ==> Cobro          : " & CType(Me.Db.mDbLector.Item("DOCUMENTO"), String).PadRight(10, " ") & "  " & CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(60, " ") & CType(Me.OrdersLineCobros(Ind).Amount, String).PadRight(15, " ") & " CON " & CType(Me.Db.mDbLector.Item("FORMA"), String))
                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                Elementos = Me.OrdersLineCobros.GetLength(Dimension)
                Me.TextBoxDebug.Text = Elementos

                '       End If
                'Application.DoEvents()




            End While
            Me.Db.mDbLector.Close()


            If Elementos > 0 Then
                ' LLAMADA AL WEB SERVICE
                Me.QweryOrdersCobros.SAT_NHJournalCustPayment_1 = Me.OrdersLineCobros

                If Me.AxEnviaCobros(Me.DocumentoContextoCobros, Me.QweryOrdersCobros) = True Then
                    Me.ListBoxDebug.Items.Add(" <== OK: ")
                    Me.ListBoxDebug.Update()
                    Me.AXTrataenvioCobrosNormalesParciales(1, "OK", Factura, Forma)
                Else
                    Me.TotalErrores = Me.TotalErrores + 1
                    Me.AXTrataenvioCobrosNormalesParciales(0, Me.AxError, Factura, Forma)
                    Me.ListBoxDebug.Items.Add(" <== : " & Me.AxError)
                    Me.ListBoxDebug.Update()

                End If
            End If




        Catch ex As Exception
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service Cobros " & Me.WebCobros.Url & " : " & ex.Message)

            MsgBox(ex.Message)
            '     MsgBox(ex.ToString)
        End Try


    End Sub


    Private Sub AxProcesarCobrosAnticipos()
        Try

            '    Dim Ind As Integer


            Dim Primerregistro As Boolean = False
            Dim ControlFactura As String = ""

            Dim UltimaFactura As String = ""
            Dim FormaCobro As String



            Me.WebCobros = New WebReferenceCobros.SAT_NHJournalCustPaymentQueryService

            '    Actualiza(Url)

            Me.WebCobros.Url = Me.mAxUrl & "SAT_NHJournalCustPaymentQueryService.asmx"

            ' Aactualiza TIME OUT

            If Me.NumericUpDownWebServiceTimeOut.Value > 0 Then
                If Me.CheckBoxTimeOut.Checked Then
                    Me.WebCobros.Timeout = Me.NumericUpDownWebServiceTimeOut.Value * 1000
                End If
            End If


            Me.TextBoxRutaWebServices.Text = Me.WebCobros.Url
            Me.TextBoxRutaWebServices.Update()

            Me.DocumentoContextoCobros = New WebReferenceCobros.DocumentContext
            Me.QweryOrdersCobros = New WebReferenceCobros.AxdSAT_NHJournalCustPaymentQuery


            'Me.WebCobros.Credentials = System.Net.CredentialCache.DefaultCredentials
            Me.WebCobros.Credentials = New System.Net.NetworkCredential(Me.mAxUserName, Me.mAxUserPwd, Me.mAxDomainName)

            '

            '  Dim i As System.ComponentModel.ISite = Me.WebCobros.Site

            '  MsgBox(CStr(Me.WebCobros.Site.ToString))
            '



            ' Me.DocumentoContextoCobros.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContextoCobros.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContextoCobros.DestinationEndpoint = Me.mAXDestinationEndPoint
            Me.DocumentoContextoCobros.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)




            '   Me.ListBoxDebug.Items.Add(" => MessageId            : " & Me.DocumentoContextoCobros.MessageId.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpoint       : " & Me.DocumentoContextoCobros.SourceEndpoint.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpointUser   : " & Me.DocumentoContextoCobros.SourceEndpointUser.ToString)
            Me.ListBoxDebug.Items.Add(" => DestinationEndpoint  : " & Me.DocumentoContextoCobros.DestinationEndpoint.ToString)


            ' Cobros en factura


            SQL = "SELECT ROWID, NVL(ASNT_LIN_DOCU,'?') AS DOCUMENTO,NVL(ASNT_CFCTA_COD,' ') AS FORMA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,NVL(ASNT_I_MONEMP,'0')  AS IMPORTE"
            SQL += " ,NVL(ASNT_AUXILIAR_STRING,'?') AS ASNT_AUXILIAR_STRING, ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI"
            SQL += " ,NVL(ASNT_RECIBO,'?') AS RECIBO ,NVL(ASNT_AUXILIAR_STRING2,' ') AS RESTO "

            SQL += " ,NVL(ASNT_FORE_CODI_AX,'0') AS FORMA2 ,NVL(ASNT_CACR_CODI,'0') AS TARJETA "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "
            ' COBROS QUE SI SON ANTICIPOS 
            SQL += " AND ASNT_CFCTA_COD =  '" & Me.mAxAnticipo & "'"
            ' SOLO SIN PROCESAR
            'SQL += " ORDER BY ASNT_LIN_DOCU ASC "

            Me.Db.TraerLector(SQL)


            Primerregistro = True
            Elementos = 0

            Me.TreeViewDebug.Nodes.Add("==>  Cobros: " & Me.mFecha)
            While Me.Db.mDbLector.Read

                Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1).Nodes.Add(CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(50, " ") & CType(Me.Db.mDbLector.Item("IMPORTE"), String))
                Me.TreeViewDebug.SelectedNode = Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1)
                Me.TreeViewDebug.ExpandAll()
                Me.TreeViewDebug.Update()


                If Primerregistro = True Then
                    Primerregistro = False
                    Elementos = 0
                    Ind = 0
                    ReDim Me.OrdersLineCobros(Ind)

                Else                ' añade un elemento a array de lineas / Objeto
                    ReDim Preserve Me.OrdersLineCobros(UBound(Me.OrdersLineCobros) + 1)
                    Ind = UBound(Me.OrdersLineCobros)
                End If



                Me.OrdersLineCobros(Ind) = New WebReferenceCobros.AxdEntity_SAT_NHJournalCustPayment_1


                Me.OrdersLineCobros(Ind).InvoiceId = CType(Me.Db.mDbLector.Item("DOCUMENTO"), String)


                ' pat0013 
                ' al facturar el anticipo NO se envia la forma de cobro genrtica nhantic sino la forma de cobro en la que se recibio en anticipo

                If CType(Me.Db.mDbLector("FORMA2"), String) <> mNWFormaCobroTransferenciaAgencia Then
                    FormaCobro = CType(Me.Db.mDbLector("FORMA2"), String)
                Else
                    FormaCobro = CType(Me.Db.mDbLector("FORMA"), String)
                End If


                ' Me.OrdersLineCobros(Ind).PaymMode = CType(Me.Db.mDbLector.Item("FORMA"), String)
                Me.OrdersLineCobros(Ind).PaymMode = FormaCobro

                ' SI ES UN ANTICIPO SE INDICA

                If CType(Me.Db.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = "ANTICIPO FACTURADO" Then
                    Me.OrdersLineCobros(Ind).PrePayment = WebReferenceCobros.AxdExtType_NoYesId.Yes
                Else
                    Me.OrdersLineCobros(Ind).PrePayment = WebReferenceCobros.AxdExtType_NoYesId.No
                End If
                Me.OrdersLineCobros(Ind).PrePaymentSpecified = True


                ' SI ES UN ANTICIPO SE INDICA SE CARGA BOOK ID
                If CType(Me.Db.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = "ANTICIPO FACTURADO" Then
                    If CType(Me.Db.mDbLector.Item("ASNT_RESE_CODI"), Integer) = 0 Then
                        ' OJO PONER ASNT_RECIBO MEJOR EN VEZ DE LA CUENTA NO ALOJADO SOLO 
                        'Me.OrdersLineCobros(Ind).BookID = CType(Me.Db.mDbLector.Item("ASNT_CCEX_CODI"), String)
                        Me.OrdersLineCobros(Ind).BookID = CType(Me.Db.mDbLector.Item("RESTO"), String).Trim & CType(Me.Db.mDbLector.Item("RECIBO"), String)
                    Else
                        Me.OrdersLineCobros(Ind).BookID = CType(Me.Db.mDbLector.Item("RESTO"), String).Trim & CType(Me.Db.mDbLector.Item("ASNT_RESE_CODI"), String) & "/" & CType(Me.Db.mDbLector.Item("ASNT_RESE_ANCI"), String)
                    End If
                End If



                Me.OrdersLineCobros(Ind).Amount = CType(Me.Db.mDbLector.Item("IMPORTE"), Double)
                Me.OrdersLineCobros(Ind).AmountSpecified = True


                Me.OrdersLineCobros(Ind).TransDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
                'Me.OrdersLineCobros(Ind).TransDateSpecified = True

                Me.OrdersLineCobros(Ind).HotelId = Me.mHotelId


                Me.ListBoxDebug.Items.Add(" ==> Cancelación de Anticipo          : " & " BookId =  " & CStr(Me.OrdersLineCobros(Ind).BookID).PadRight(15, " ") & " Factura =  " & CType(Me.Db.mDbLector.Item("DOCUMENTO"), String).PadRight(10, " ") & " Concepto =  " & CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(40, " ") & " " & CType(Me.OrdersLineCobros(Ind).Amount, String).PadRight(15, " ") & " CON " & FormaCobro)

                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                Elementos = Me.OrdersLineCobros.GetLength(Dimension)
                Me.TextBoxDebug.Text = Elementos


                'Application.DoEvents()


            End While
            Me.Db.mDbLector.Close()


            If Elementos > 0 Then
                ' LLAMADA AL WEB SERVICE
                Me.QweryOrdersCobros.SAT_NHJournalCustPayment_1 = Me.OrdersLineCobros

                If Me.AxEnviaCobros(Me.DocumentoContextoCobros, Me.QweryOrdersCobros) = True Then
                    Me.ListBoxDebug.Items.Add(" <== OK: ")
                    Me.ListBoxDebug.Update()
                    Me.AXTrataenvioCobrosAnticipos(1, "OK")
                Else
                    Me.TotalErrores = Me.TotalErrores + 1
                    Me.AXTrataenvioCobrosAnticipos(0, Me.AxError)
                    Me.ListBoxDebug.Items.Add(" <== : " & Me.AxError)
                    'DEBUG
                    ' MsgBox(Me.AxError)
                    Me.ListBoxDebug.SelectedIndex = Me.ListBoxDebug.Items.Count - 1
                    Me.ListBoxDebug.Update()

                End If

            End If



        Catch ex As Exception
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service Cobros " & Me.WebCobros.Url & " : " & ex.Message)

            MsgBox(ex.Message)
            '     MsgBox(ex.ToString)
        End Try


    End Sub
    Private Sub AxProcesarCobrosAnticiposUnoPorUno()
        Try

            '    Dim Ind As Integer


            Dim Primerregistro As Boolean = False
            Dim ControlFactura As String = ""

            Dim UltimaFactura As String = ""
            Dim FormaCobro As String



            Me.WebCobros = New WebReferenceCobros.SAT_NHJournalCustPaymentQueryService

            '    Actualiza(Url)

            Me.WebCobros.Url = Me.mAxUrl & "SAT_NHJournalCustPaymentQueryService.asmx"

            ' Aactualiza TIME OUT

            If Me.NumericUpDownWebServiceTimeOut.Value > 0 Then
                If Me.CheckBoxTimeOut.Checked Then
                    Me.WebCobros.Timeout = Me.NumericUpDownWebServiceTimeOut.Value * 1000
                End If
            End If


            Me.TextBoxRutaWebServices.Text = Me.WebCobros.Url
            Me.TextBoxRutaWebServices.Update()

            Me.DocumentoContextoCobros = New WebReferenceCobros.DocumentContext
            Me.QweryOrdersCobros = New WebReferenceCobros.AxdSAT_NHJournalCustPaymentQuery


            'Me.WebCobros.Credentials = System.Net.CredentialCache.DefaultCredentials
            Me.WebCobros.Credentials = New System.Net.NetworkCredential(Me.mAxUserName, Me.mAxUserPwd, Me.mAxDomainName)

            '

            '  Dim i As System.ComponentModel.ISite = Me.WebCobros.Site

            '  MsgBox(CStr(Me.WebCobros.Site.ToString))
            '



            ' Me.DocumentoContextoCobros.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContextoCobros.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContextoCobros.DestinationEndpoint = Me.mAXDestinationEndPoint
            Me.DocumentoContextoCobros.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)




            '   Me.ListBoxDebug.Items.Add(" => MessageId            : " & Me.DocumentoContextoCobros.MessageId.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpoint       : " & Me.DocumentoContextoCobros.SourceEndpoint.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpointUser   : " & Me.DocumentoContextoCobros.SourceEndpointUser.ToString)
            Me.ListBoxDebug.Items.Add(" => DestinationEndpoint  : " & Me.DocumentoContextoCobros.DestinationEndpoint.ToString)


            ' Cobros en factura


            SQL = "SELECT ROWID, NVL(ASNT_LIN_DOCU,'?') AS DOCUMENTO,NVL(ASNT_CFCTA_COD,' ') AS FORMA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,NVL(ASNT_I_MONEMP,'0')  AS IMPORTE"
            SQL += " ,NVL(ASNT_AUXILIAR_STRING,'?') AS ASNT_AUXILIAR_STRING, ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI"
            SQL += " ,NVL(ASNT_RECIBO,'?') AS RECIBO ,NVL(ASNT_AUXILIAR_STRING2,' ') AS RESTO "

            SQL += " ,NVL(ASNT_FORE_CODI_AX,'0') AS FORMA2 ,NVL(ASNT_CACR_CODI,'0') AS TARJETA "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "
            ' COBROS QUE SI SON ANTICIPOS 
            SQL += " AND ASNT_CFCTA_COD =  '" & Me.mAxAnticipo & "'"
            ' SOLO SIN PROCESAR
            'SQL += " ORDER BY ASNT_LIN_DOCU ASC "

            Me.Db.TraerLector(SQL)


            Primerregistro = True
            Elementos = 0

            Me.TreeViewDebug.Nodes.Add("==>  Cobros: " & Me.mFecha)
            While Me.Db.mDbLector.Read

                Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1).Nodes.Add(CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(50, " ") & CType(Me.Db.mDbLector.Item("IMPORTE"), String))
                Me.TreeViewDebug.SelectedNode = Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1)
                Me.TreeViewDebug.ExpandAll()
                Me.TreeViewDebug.Update()


                'If Primerregistro = True Then
                'Primerregistro = False
                'Elementos = 0
                'Ind = 0
                'ReDim Me.OrdersLineCobros(Ind)

                'Else                ' añade un elemento a array de lineas / Objeto
                'ReDim Preserve Me.OrdersLineCobros(UBound(Me.OrdersLineCobros) + 1)
                'Ind = UBound(Me.OrdersLineCobros)
                'End If

                Elementos = 0
                Ind = 0
                ReDim Me.OrdersLineCobros(Ind)




                Me.OrdersLineCobros(Ind) = New WebReferenceCobros.AxdEntity_SAT_NHJournalCustPayment_1


                Me.OrdersLineCobros(Ind).InvoiceId = CType(Me.Db.mDbLector.Item("DOCUMENTO"), String)


                ' pat0013 
                ' al facturar el anticipo NO se envia la forma de cobro genrtica nhantic sino la forma de cobro en la que se recibio en anticipo

                If CType(Me.Db.mDbLector("FORMA2"), String) <> mNWFormaCobroTransferenciaAgencia Then
                    FormaCobro = CType(Me.Db.mDbLector("FORMA2"), String)
                Else
                    FormaCobro = CType(Me.Db.mDbLector("FORMA"), String)
                End If


                ' Me.OrdersLineCobros(Ind).PaymMode = CType(Me.Db.mDbLector.Item("FORMA"), String)
                Me.OrdersLineCobros(Ind).PaymMode = FormaCobro

                ' SI ES UN ANTICIPO SE INDICA

                If CType(Me.Db.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = "ANTICIPO FACTURADO" Then
                    Me.OrdersLineCobros(Ind).PrePayment = WebReferenceCobros.AxdExtType_NoYesId.Yes
                Else
                    Me.OrdersLineCobros(Ind).PrePayment = WebReferenceCobros.AxdExtType_NoYesId.No
                End If
                Me.OrdersLineCobros(Ind).PrePaymentSpecified = True


                ' SI ES UN ANTICIPO SE INDICA SE CARGA BOOK ID
                If CType(Me.Db.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = "ANTICIPO FACTURADO" Then
                    If CType(Me.Db.mDbLector.Item("ASNT_RESE_CODI"), Integer) = 0 Then
                        ' OJO PONER ASNT_RECIBO MEJOR EN VEZ DE LA CUENTA NO ALOJADO SOLO 
                        'Me.OrdersLineCobros(Ind).BookID = CType(Me.Db.mDbLector.Item("ASNT_CCEX_CODI"), String)
                        Me.OrdersLineCobros(Ind).BookID = CType(Me.Db.mDbLector.Item("RESTO"), String).Trim & CType(Me.Db.mDbLector.Item("RECIBO"), String)
                    Else
                        Me.OrdersLineCobros(Ind).BookID = CType(Me.Db.mDbLector.Item("RESTO"), String).Trim & CType(Me.Db.mDbLector.Item("ASNT_RESE_CODI"), String) & "/" & CType(Me.Db.mDbLector.Item("ASNT_RESE_ANCI"), String)
                    End If
                End If



                Me.OrdersLineCobros(Ind).Amount = CType(Me.Db.mDbLector.Item("IMPORTE"), Double)
                Me.OrdersLineCobros(Ind).AmountSpecified = True


                Me.OrdersLineCobros(Ind).TransDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
                'Me.OrdersLineCobros(Ind).TransDateSpecified = True

                Me.ListBoxDebug.Items.Add(" ==> Cancelación de Anticipo          : " & " BookId =  " & CStr(Me.OrdersLineCobros(Ind).BookID).PadRight(15, " ") & " Factura =  " & CType(Me.Db.mDbLector.Item("DOCUMENTO"), String).PadRight(10, " ") & " Concepto =  " & CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(40, " ") & " " & CType(Me.OrdersLineCobros(Ind).Amount, String).PadRight(15, " ") & " CON " & FormaCobro)

                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                Elementos = Me.OrdersLineCobros.GetLength(Dimension)
                Me.TextBoxDebug.Text = Elementos


                'Application.DoEvents()


                ' LLAMADA AL WEB SERVICE
                Me.QweryOrdersCobros.SAT_NHJournalCustPayment_1 = Me.OrdersLineCobros

                If Me.AxEnviaCobros(Me.DocumentoContextoCobros, Me.QweryOrdersCobros) = True Then
                    Me.ListBoxDebug.Items.Add(" <== OK: ")
                    Me.ListBoxDebug.Update()
                    Me.AXTrataenvioCobrosAnticiposUnoPorUno(1, "OK", CType(Me.Db.mDbLector.Item("ROWID"), String))
                Else
                    Me.TotalErrores = Me.TotalErrores + 1
                    Me.AXTrataenvioCobrosAnticiposUnoPorUno(0, Me.AxError, CType(Me.Db.mDbLector.Item("ROWID"), String))
                    Me.ListBoxDebug.Items.Add(" <== : " & Me.AxError)
                    'DEBUG
                    ' MsgBox(Me.AxError)
                    Me.ListBoxDebug.SelectedIndex = Me.ListBoxDebug.Items.Count - 1
                    Me.ListBoxDebug.Update()

                End If

            End While
            Me.Db.mDbLector.Close()






        Catch ex As Exception
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service Cobros " & Me.WebCobros.Url & " : " & ex.Message)

            MsgBox(ex.Message)
            '     MsgBox(ex.ToString)
        End Try


    End Sub
    Private Function AxEnviaCobros(ByVal vDoc As WebReferenceCobros.DocumentContext, ByVal vQuery As WebReferenceCobros.AxdSAT_NHJournalCustPaymentQuery) As Boolean
        Try
            Me.AxError = ""
            vDoc.MessageId = Guid.NewGuid.ToString
            Me.WebCobros.createListSAT_NHJournalCustPaymentQuery(vDoc, vQuery)
            Me.TreeViewDebug.Nodes.Add("<==  Ok Cobros: " & Me.mFecha)
            Return True

        Catch ex As Exception
            Me.AxError = ex.Message
            Me.TreeViewDebug.Nodes.Add("<==   " & ex.Message)
            Return False
        End Try
    End Function
    Private Sub AXTrataenvioCobrosNormales(ByVal vStatus As Integer, ByVal vMessage As String)
        Try
            SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = " & vStatus
            SQL += " ,ASNT_ERR_MESSAGE = '" & "Facturas con un Solo Cobro =  " & Mid(vMessage, 1, 4000).Replace("'", "''").Trim & "'"
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService'"
            ' COBROS QUE NO SON ANTICIPOS 
            SQL += " AND ASNT_CFCTA_COD <>  '" & Me.mAxAnticipo & "'"
            ' cobros que no fueron exluidos del envio 
            SQL += " AND ASNT_AX_STATUS <> 9 "
            SQL += " AND ASNT_AX_STATUS <> 8 "
            SQL += " AND ASNT_AX_STATUS <> 7 "
            ' cobros que no estaban ya enviados ok 
            SQL += " AND ASNT_AX_STATUS <> 1 "


            Me.DbWrite.EjecutaSqlCommit(SQL)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub AXTrataenvioCobrosNormalesParciales(ByVal vStatus As Integer, ByVal vMessage As String, ByVal vFactura As String, ByVal vForma As String)
        ' PAT 35
        Try
            SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = " & vStatus
            SQL += " ,ASNT_ERR_MESSAGE = '" & "Factura = " & vFactura & " = " & Mid(vMessage, 1, 4000).Replace("'", "''").Trim & "'"
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService'"
            ' COBROS QUE NO SON ANTICIPOS 
            SQL += " AND ASNT_CFCTA_COD <>  '" & Me.mAxAnticipo & "'"
            ' cobros que no fueron exluidos del envio 
            SQL += " AND ASNT_AX_STATUS <> 9 "
            SQL += " AND ASNT_AX_STATUS <> 8 "
            SQL += " AND ASNT_AX_STATUS <> 7 "
            ' cobros que no estaban ya enviados ok 
            SQL += " AND ASNT_AX_STATUS <> 1 "

            ' KEY
            SQL += " AND ASNT_LIN_DOCU = '" & vFactura & "'"
            SQL += " AND ASNT_CFCTA_COD = '" & vForma & "'"


            Me.DbWrite.EjecutaSqlCommit(SQL)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub AXTrataenvioCobrosNormalesUnoPorUno(ByVal vStatus As Integer, ByVal vMessage As String, ByVal vRowid As String)
        Try
            SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = " & vStatus
            SQL += " ,ASNT_ERR_MESSAGE = '" & Mid(vMessage, 1, 4000).Replace("'", "''").Trim & "'"
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService'"
            ' COBROS QUE NO SON ANTICIPOS 
            SQL += " AND ASNT_CFCTA_COD <>  '" & Me.mAxAnticipo & "'"
            ' cobros que no fueron exluidos del envio 
            SQL += " AND ASNT_AX_STATUS <> 9 "
            SQL += " AND ASNT_AX_STATUS <> 8 "
            SQL += " AND ASNT_AX_STATUS <> 7 "
            ' cobros que no estaban ya enviados ok 
            SQL += " AND ASNT_AX_STATUS <> 1 "

            ' rowid
            SQL += " AND ROWID = '" & vRowid & "'"


            Me.DbWrite.EjecutaSqlCommit(SQL)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub AXTrataenvioCobrosAnticipos(ByVal vStatus As Integer, ByVal vMessage As String)
        Try
            SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = " & vStatus
            SQL += " ,ASNT_ERR_MESSAGE = '" & Mid(vMessage, 1, 4000).Replace("'", "''").Trim & "'"
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService'"
            ' COBROS QUE si SON ANTICIPOS 
            SQL += " AND ASNT_CFCTA_COD =  '" & Me.mAxAnticipo & "'"
            ' cobros que no fueron exluidos del envio 
            SQL += " AND ASNT_AX_STATUS <> 9 "
            SQL += " AND ASNT_AX_STATUS <> 8 "
            SQL += " AND ASNT_AX_STATUS <> 7 "
            ' cobros que no estaban ya enviados ok 
            SQL += " AND ASNT_AX_STATUS <> 1 "

            Me.DbWrite.EjecutaSqlCommit(SQL)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub AXTrataenvioCobrosAnticiposUnoPorUno(ByVal vStatus As Integer, ByVal vMessage As String, ByVal vRowid As String)
        Try
            SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = " & vStatus
            SQL += " ,ASNT_ERR_MESSAGE = '" & Mid(vMessage, 1, 4000).Replace("'", "''").Trim & "'"
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService'"
            ' COBROS QUE si SON ANTICIPOS 
            SQL += " AND ASNT_CFCTA_COD =  '" & Me.mAxAnticipo & "'"
            ' cobros que no fueron exluidos del envio 
            SQL += " AND ASNT_AX_STATUS <> 9 "
            SQL += " AND ASNT_AX_STATUS <> 8 "
            SQL += " AND ASNT_AX_STATUS <> 7 "
            ' cobros que no estaban ya enviados ok 
            SQL += " AND ASNT_AX_STATUS <> 1 "
            ' rowid
            SQL += " AND ROWID ='" & vRowid & "'"

            Me.DbWrite.EjecutaSqlCommit(SQL)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
#End Region
#Region "VENTA DE TIKETS"
    Private Sub ProcesarVentaTiketsLineaALinea()
        Try

            '   Dim Ind As Integer

            Dim Primerregistro As Boolean = False

            Dim ControlFacturaBono As Boolean = False
            Dim ControlFacturaOtrosServicios As Boolean = False


            Me.WebTikets = New WebReferenceVentaTikets.SAT_JournalLossProfitQueryService

            'Actualiza Url

            Me.WebTikets.Url = Me.mAxUrl & "SAT_JournalLossProfitQueryService.asmx"


            Me.TextBoxRutaWebServices.Text = Me.WebTikets.Url
            Me.TextBoxRutaWebServices.Update()

            Me.DocumentoContextoTikets = New WebReferenceVentaTikets.DocumentContext
            Me.QweryOrdersTikets = New WebReferenceVentaTikets.AxdSAT_JournalLossProfitQuery


            '   Me.WebTikets.Credentials = System.Net.CredentialCache.DefaultCredentials
            Me.WebTikets.Credentials = New System.Net.NetworkCredential(Me.mAxUserName, Me.mAxUserPwd, Me.mAxDomainName)
            '


            Me.DocumentoContextoTikets.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContextoTikets.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContextoTikets.DestinationEndpoint = Me.mAXDestinationEndPoint
            Me.DocumentoContextoTikets.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)


            ' TIKETS

            '  MsgBox("ojo las cantidades se envian negativas  y no se envian las devoluciones ", MsgBoxStyle.Information, "revisar")


            SQL = "SELECT ROWID ,ASNT_PROD_ID AS PRODUCTO,NVL(ASNT_AMPCPTO,'?') AS DESCRIPCION,ASNT_PROD_TALLA AS TALLA,ASNT_PROD_COLOR AS COLOR ,NVL(ASNT_I_MONEMP,'0') * -1  AS CANTIDAD,ASNT_ALMA_AX AS ALMACEN "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_JournalLossProfitQueryService'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "
            ' OJO 
            '  SQL += " AND ROWNUM < 10"


            Me.Db.TraerLector(SQL)

            Primerregistro = True

            While Me.Db.mDbLector.Read


                Ind = 0
                ReDim Me.OrdersLineTikets(Ind)
                ReDim Me.TransRowid(Ind)
                Me.OrdersLineTikets(Ind) = New WebReferenceVentaTikets.AxdEntity_SAT_NHJournalLossProfitTable


                ' articulo

                Me.OrdersLineTikets(Ind).ItemId = Me.Db.mDbLector.Item("PRODUCTO")
                'Me.OrdersLineTikets(Ind).ItemId = "SART-000035"

                ' talla
                If IsDBNull(Me.Db.mDbLector.Item("TALLA")) = False Then
                    Me.OrdersLineTikets(Ind).InventSizeId = Me.Db.mDbLector.Item("TALLA")
                End If
                'Me.OrdersLineTikets(Ind).InventSizeId = "TAM000003"


                ' color
                If IsDBNull(Me.Db.mDbLector.Item("COLOR")) = False Then
                    Me.OrdersLineTikets(Ind).InventColorId = Me.Db.mDbLector.Item("COLOR")
                End If
                'Me.OrdersLineTikets(Ind).InventColorId = "COL000004"


                ' cantidad
                Me.OrdersLineTikets(Ind).Qty = Me.Db.mDbLector.Item("CANTIDAD")
                Me.OrdersLineTikets(Ind).QtySpecified = True

                ' almacen
                Me.OrdersLineTikets(Ind).InventLocationId = Me.Db.mDbLector.Item("ALMACEN")
                'Me.OrdersLineTikets(Ind).InventLocationId = "Pru"


                ' fecha
                Me.OrdersLineTikets(Ind).TransDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
                Me.OrdersLineTikets(Ind).TransDateSpecified = True


                Me.ListBoxDebug.Items.Add(" ==> Venta de Tikets  : " & CType(Me.Db.mDbLector.Item("DESCRIPCION"), String).PadRight(40, " ") & CType(Me.OrdersLineTikets(Ind).Qty, String))
                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                Me.TransRowid(Ind) = Me.Db.mDbLector.Item("ROWID")

                ' LLAMADA AL WEB SERVICE

                '   MsgBox("Ojo la Query esta remeneada para que solo devuelva 10 Productos para pruebas", MsgBoxStyle.Information, "Aqui")

                Me.QweryOrdersTikets.SAT_NHJournalLossProfitTable = Me.OrdersLineTikets

                If Me.EnviaVentaTikets(Me.DocumentoContextoTikets, Me.QweryOrdersTikets) = True Then
                    Me.ListBoxDebug.Items.Add(" <== OK: ")
                    Me.ListBoxDebug.Update()
                    Me.TrataenvioGenerico(Me.TransRowid(Ind), 1, "OK")
                Else
                    Me.TotalErrores = Me.TotalErrores + 1
                    Me.TrataenvioGenerico(Me.TransRowid(Ind), 0, Me.AxError)
                    Me.ListBoxDebug.Items.Add(" <== : " & Me.AxError)
                    Me.ListBoxDebug.SelectedIndex = Me.ListBoxDebug.Items.Count - 1
                    Me.ListBoxDebug.Update()

                End If

                'Application.DoEvents()

                If MsgBox("Continuar ? ", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                    Exit While
                End If


            End While
            Me.Db.mDbLector.Close()




        Catch ex As Exception
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service Tikets " & Me.WebTikets.Url & " : " & ex.Message)

            MsgBox(ex.Message)
            '     MsgBox(ex.ToString)
        End Try
        '   Catch ex As Web.Services.Protocols.SoapException
        '       MsgBox(ex.Message)'

        'End Try

    End Sub
    Private Function EnviaVentaTikets(ByVal vDoc As WebReferenceVentaTikets.DocumentContext, ByVal vQuery As WebReferenceVentaTikets.AxdSAT_JournalLossProfitQuery) As Boolean
        Try
            Me.AxError = ""
            vDoc.MessageId = Guid.NewGuid.ToString
            Me.WebTikets.createListSAT_JournalLossProfitQuery(vDoc, vQuery)
            Return True

        Catch ex As Exception
            Me.AxError = ex.Message
            Return False
        End Try
    End Function
#End Region
#Region "ESRADISTICAS"
    Private Sub ProcesarEstadisticas()
        Try

            '   Dim Ind As Integer

            Dim Primerregistro As Boolean = False


            Me.WebAnalitica = New WebReferenceAnalitica.SAT_NHInfoAnalyticalQueryService

            '    Actualiza(Url)

            Me.WebAnalitica.Url = Me.mAxUrl & "SAT_NHInfoAnalyticalQueryService.asmx"


            Me.TextBoxRutaWebServices.Text = Me.WebAnalitica.Url
            Me.TextBoxRutaWebServices.Update()

            Me.DocumentoContextoAnalitica = New WebReferenceAnalitica.DocumentContext
            Me.QweryOrdersAnalitica = New WebReferenceAnalitica.AxdSAT_NHInfoAnalyticalQuery



            Me.WebAnalitica.Credentials = New System.Net.NetworkCredential(Me.mAxUserName, Me.mAxUserPwd, Me.mAxDomainName)

            Me.DocumentoContextoAnalitica.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContextoAnalitica.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContextoAnalitica.DestinationEndpoint = Me.mAXDestinationEndPoint
            Me.DocumentoContextoAnalitica.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)




            Me.ListBoxDebug.Items.Add(" => MessageId            : " & Me.DocumentoContextoAnalitica.MessageId.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpoint       : " & Me.DocumentoContextoAnalitica.SourceEndpoint.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpointUser   : " & Me.DocumentoContextoAnalitica.SourceEndpointUser.ToString)
            Me.ListBoxDebug.Items.Add(" => DestinationEndpoint  : " & Me.DocumentoContextoAnalitica.DestinationEndpoint.ToString)


            ' Anticipos / comisiones 


            SQL = "SELECT   ROWID,NVL(ASNT_CFCTA_COD,' ') AS CUENTA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,NVL(ASNT_I_MONEMP,'0')  AS IMPORTE"
            SQL += " , ASNT_AUXILIAR_STRING AS TIPO "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHInfoAnalyticalQueryService'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "
            ' SOLO ESTADISTICAS
            SQL += " AND ASNT_AUXILIAR_STRING <> 'ANTICIPO' "
            SQL += " ORDER BY ASNT_AUXILIAR_STRING ASC "

            Me.Db.TraerLector(SQL)


            Primerregistro = True
            Elementos = 0

            Me.TreeViewDebug.Nodes.Add("==>  Analítica: " & Me.mFecha)

            While Me.Db.mDbLector.Read

                Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1).Nodes.Add(CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(50, " ") & CType(Me.Db.mDbLector.Item("IMPORTE"), String))
                Me.TreeViewDebug.SelectedNode = Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1)
                Me.TreeViewDebug.ExpandAll()
                Me.TreeViewDebug.Update()

                If Primerregistro = True Then

                    Primerregistro = False
                    Elementos = 0
                    Ind = 0
                    ReDim Me.OrdersLineAnalitica(Ind)
                    ReDim Me.TransRowid(Ind)


                Else                ' añade un elemento a array de lineas / Objeto
                    ReDim Preserve Me.OrdersLineAnalitica(UBound(Me.OrdersLineAnalitica) + 1)
                    Ind = UBound(Me.OrdersLineAnalitica)
                End If



                Me.OrdersLineAnalitica(Ind) = New WebReferenceAnalitica.AxdEntity_SAT_NHInfoAnalytical_1



                Me.OrdersLineAnalitica(Ind).TransDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
                'Me.OrdersLineAnalitica(Ind).TransDateSpecified = True

                If Me.Db.mDbLector.Item("TIPO") = "CLIENTES" Then
                    Me.OrdersLineAnalitica(Ind).AnalyticalConcept = WebReferenceAnalitica.AxdEnum_SAT_NHAnalyticalConcept.NumOverNightStays
                Else
                    Me.OrdersLineAnalitica(Ind).AnalyticalConcept = WebReferenceAnalitica.AxdEnum_SAT_NHAnalyticalConcept.NumRooms
                End If
                Me.OrdersLineAnalitica(Ind).AnalyticalConceptSpecified = True

                ' Me.OrdersLineAnticipos(Ind).Qty = CType(Db.mDbLector.Item("IMPORTE"), Integer)
                Me.OrdersLineAnalitica(Ind).Qty = CType(Db.mDbLector.Item("IMPORTE"), Double)
                Me.OrdersLineAnalitica(Ind).QtySpecified = True
                Me.OrdersLineAnalitica(Ind).HotelId = Me.mHotelId

                Me.ListBoxDebug.Items.Add(" ==> Estadistica         : " & CType(Me.Db.mDbLector.Item("TIPO"), String).PadRight(15, " ") & CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(40, " ") & " " & Me.Db.mDbLector.Item("IMPORTE"))
                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                Elementos = Me.OrdersLineAnalitica.GetLength(Dimension)
                Me.TextBoxDebug.Text = Elementos

                'Application.DoEvents()

                ' If MsgBox("Continuar ? ", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                ' Exit While
                ' End If

            End While
            Me.Db.mDbLector.Close()



            ' LLAMADA AL WEB SERVICE
            If Elementos > 0 Then


                Me.QweryOrdersAnalitica.SAT_NHInfoAnalytical_1 = Me.OrdersLineAnalitica
                If Me.EnviaEstadisticas(Me.DocumentoContextoAnalitica, Me.QweryOrdersAnalitica) = True Then
                    Me.ListBoxDebug.Items.Add(" <== OK: ")
                    Me.ListBoxDebug.Update()
                    Me.TrataenvioEstadisticas(1, "OK")
                Else
                    Me.TotalErrores = Me.TotalErrores + 1
                    Me.TrataenvioEstadisticas(0, Me.AxError)
                    Me.ListBoxDebug.Items.Add(" <== : " & Me.AxError)
                    Me.ListBoxDebug.SelectedIndex = Me.ListBoxDebug.Items.Count - 1
                    Me.ListBoxDebug.Update()

                End If
            End If


        Catch ex As Exception
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service ESTADISTICAS " & Me.WebAnalitica.Url & " : " & ex.Message)

            MsgBox(ex.Message)
            '     MsgBox(ex.ToString)
        End Try
        '   Catch ex As Web.Services.Protocols.SoapException
        '       MsgBox(ex.Message)'

        'End Try

    End Sub
    Private Function EnviaEstadisticas(ByVal vDoc As WebReferenceAnalitica.DocumentContext, ByVal vQuery As WebReferenceAnalitica.AxdSAT_NHInfoAnalyticalQuery) As Boolean
        Try
            Me.AxError = ""
            vDoc.MessageId = Guid.NewGuid.ToString
            Me.WebAnalitica.createListSAT_NHInfoAnalyticalQuery(vDoc, vQuery)
            Return True

        Catch ex As Exception
            Me.AxError = ex.Message
            Return False
        End Try
    End Function
    Private Sub TrataenvioEstadisticas(ByVal vStatus As Integer, ByVal vMessage As String)
        Try
            SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = " & vStatus
            SQL += " ,ASNT_ERR_MESSAGE = '" & Mid(vMessage, 1, 4000).Replace("'", "''").Trim & "'"
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHInfoAnalyticalQueryService'"
            SQL += " AND ASNT_AUXILIAR_STRING <> 'ANTICIPO' "
            Me.DbWrite.EjecutaSqlCommit(SQL)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

#End Region
#Region "Anticipos"
    Private Sub AxProcesarAnticipos()
        Try

            '    Dim Ind As Integer


            Dim Primerregistro As Boolean = False




            Me.WebAnticipos = New WebReferenceAnticipos.SAT_NHPrePaymentService

            '    Actualiza(Url)

            Me.WebAnticipos.Url = Me.mAxUrl & "SAT_NHPrePaymentService.asmx"

            If Me.NumericUpDownWebServiceTimeOut.Value > 0 Then
                If Me.CheckBoxTimeOut.Checked Then
                    Me.WebAnticipos.Timeout = Me.NumericUpDownWebServiceTimeOut.Value * 1000
                End If
            End If


            Me.TextBoxRutaWebServices.Text = Me.WebAnticipos.Url
            Me.TextBoxRutaWebServices.Update()

            Me.DocumentoContextoAnticipos = New WebReferenceAnticipos.DocumentContext
            Me.QweryOrdersAnticipos = New WebReferenceAnticipos.AxdSAT_NHPrePayment


            'Me.WebAnticipos.Credentials = System.Net.CredentialCache.DefaultCredentials
            Me.WebAnticipos.Credentials = New System.Net.NetworkCredential(Me.mAxUserName, Me.mAxUserPwd, Me.mAxDomainName)

            '


            ' Me.DocumentoContextoAnticipos.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContextoAnticipos.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContextoAnticipos.DestinationEndpoint = Me.mAXDestinationEndPoint
            Me.DocumentoContextoAnticipos.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)




            '   Me.ListBoxDebug.Items.Add(" => MessageId            : " & Me.DocumentoContextoAnticipos.MessageId.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpoint       : " & Me.DocumentoContextoAnticipos.SourceEndpoint.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpointUser   : " & Me.DocumentoContextoAnticipos.SourceEndpointUser.ToString)
            Me.ListBoxDebug.Items.Add(" => DestinationEndpoint  : " & Me.DocumentoContextoAnticipos.DestinationEndpoint.ToString)


            ' Anticipos

            SQL = "SELECT  NVL(ASNT_CFCTA_COD,' ') AS CUENTA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,NVL(ASNT_I_MONEMP,'0')  AS IMPORTE"
            SQL += " , ASNT_AUXILIAR_STRING AS TIPO ,NVL(ASNT_AUXILIAR_STRING2,' ') AS RESTO "
            SQL += " , ASNT_RESE_CODI,ASNT_RESE_ANCI,ASNT_CCEX_CODI,ASNT_ENTI_CODI,ASNT_TIPO_CLIENTE_ANTICIPO,NVL(ASNT_FORE_CODI_AX,'?') AS FORMA,NVL(ASNT_CIF ,'?') AS CIF "
            SQL += " ,NVL(ASNT_RECIBO,'?') AS RECIBO,ROWID "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHPrePaymentService'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "

            ' SQL += " ORDER BY ASNT_RESE_CODI ASC "

            SQL += " ORDER BY ASNT_WEBSERVICE_NAME, ASNT_CFATOCAB_REFER,ASNT_LINEA"


            Me.Db.TraerLector(SQL)


            Primerregistro = True
            Elementos = 0

            Me.TreeViewDebug.Nodes.Add("==>  Anticipos : " & Me.mFecha)
            While Me.Db.mDbLector.Read


                Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1).Nodes.Add(CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(50, " ") & CType(Me.Db.mDbLector.Item("IMPORTE"), String))
                Me.TreeViewDebug.SelectedNode = Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1)
                Me.TreeViewDebug.ExpandAll()
                Me.TreeViewDebug.Update()

                If Primerregistro = True Then
                    Primerregistro = False
                    Elementos = 0
                    Ind = 0
                    ReDim Me.OrdersLineAnticipos(Ind)

                Else                ' añade un elemento a array de lineas / Objeto
                    ReDim Preserve Me.OrdersLineAnticipos(UBound(Me.OrdersLineAnticipos) + 1)
                    Ind = UBound(Me.OrdersLineAnticipos)
                End If





                Me.OrdersLineAnticipos(Ind) = New WebReferenceAnticipos.AxdEntity_Tabla


                If CType(Me.Db.mDbLector.Item("ASNT_RESE_CODI"), Integer) = 0 Then
                    ' OJO PONER ASNT_RECIBO MEJOR EN VEZ DE LA CUENTA NO ALOJADO SOLO 
                    ' Me.OrdersLineAnticipos(Ind).BookID = CType(Me.Db.mDbLector.Item("ASNT_CCEX_CODI"), String)
                    Me.OrdersLineAnticipos(Ind).BookID = CType(Me.Db.mDbLector.Item("RESTO"), String).Trim & CType(Me.Db.mDbLector.Item("RECIBO"), String)
                Else
                    Me.OrdersLineAnticipos(Ind).BookID = CType(Me.Db.mDbLector.Item("RESTO"), String).Trim & CType(Me.Db.mDbLector.Item("ASNT_RESE_CODI"), String) & "/" & CType(Me.Db.mDbLector.Item("ASNT_RESE_ANCI"), String)
                End If



                If CType(Me.Db.mDbLector.Item("ASNT_TIPO_CLIENTE_ANTICIPO"), Integer) = 3 Or CType(Me.Db.mDbLector.Item("CIF"), String) = Me.mParaCifContadoGenerico Then
                    Me.OrdersLineAnticipos(Ind).ContClient = WebReferenceAnticipos.AxdExtType_NoYesId.Yes
                    Me.OrdersLineAnticipos(Ind).ContClientSpecified = True
                    ' Me.OrdersLineAnticipos(Ind).VatNum = ""
                Else
                    Me.OrdersLineAnticipos(Ind).ContClient = WebReferenceAnticipos.AxdExtType_NoYesId.No
                    Me.OrdersLineAnticipos(Ind).ContClientSpecified = True
                    Me.OrdersLineAnticipos(Ind).VatNum = CType(Me.Db.mDbLector.Item("CIF"), String)
                End If




                Me.OrdersLineAnticipos(Ind).PaymMode = CType(Me.Db.mDbLector.Item("FORMA"), String)
                Me.OrdersLineAnticipos(Ind).PrePaymentAmount = CType(Me.Db.mDbLector.Item("IMPORTE"), Double)
                Me.OrdersLineAnticipos(Ind).PrePaymentAmountSpecified = True


                Me.OrdersLineAnticipos(Ind).TransDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
                'Me.OrdersLineAnticipos(Ind).TransDateSpecified = True

                Me.OrdersLineAnticipos(Ind).Txt = CType(Me.Db.mDbLector.Item("CONCEPTO"), String)

                Me.OrdersLineAnticipos(Ind).HotelId = Me.mHotelId

                Me.ListBoxDebug.Items.Add(" ==> Anticipo       : " & " BookId = " & CStr(Me.OrdersLineAnticipos(Ind).BookID).PadRight(15, " ") & " Concepto = " & CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(50, " ") & " Nif " & Me.OrdersLineAnticipos(Ind).VatNum & " Cliente Contado ? = " & Me.OrdersLineAnticipos(Ind).ContClient & " PayMode = " & CStr(Me.OrdersLineAnticipos(Ind).PaymMode).PadRight(15, " ") & " " & Me.OrdersLineAnticipos(Ind).PrePaymentAmount)
                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1

                Elementos = Me.OrdersLineAnticipos.GetLength(Dimension)
                Me.TextBoxDebug.Text = Elementos



                'Application.DoEvents()


            End While
            Me.Db.mDbLector.Close()




            If Elementos > 0 Then
                ' LLAMADA AL WEB SERVICE
                Me.QweryOrdersAnticipos.Tabla = Me.OrdersLineAnticipos

                If Me.AxEnviaAnticipos(Me.DocumentoContextoAnticipos, Me.QweryOrdersAnticipos) = True Then
                    Me.ListBoxDebug.Items.Add(" <== OK: ")
                    Me.ListBoxDebug.Update()
                    Me.AXTrataenvioAnticipos(1, "OK")
                Else
                    Me.TotalErrores = Me.TotalErrores + 1
                    Me.AXTrataenvioAnticipos(0, Me.AxError)
                    Me.ListBoxDebug.Items.Add(" <== : " & Me.AxError)
                    Me.ListBoxDebug.SelectedIndex = Me.ListBoxDebug.Items.Count - 1
                    Me.ListBoxDebug.Update()

                End If

            End If





        Catch ex As Exception
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service Anticipos " & Me.WebAnticipos.Url & " : " & ex.Message)

            MsgBox(ex.Message)
            '     MsgBox(ex.ToString)
        End Try
        '   Catch ex As Web.Services.Protocols.SoapException
        '       MsgBox(ex.Message)'

        'End Try

    End Sub
    Private Function AxEnviaAnticipos(ByVal vDoc As WebReferenceAnticipos.DocumentContext, ByVal vQuery As WebReferenceAnticipos.AxdSAT_NHPrePayment) As Boolean
        Try
            Me.AxError = ""
            vDoc.MessageId = Guid.NewGuid.ToString
            Me.WebAnticipos.SendPrePayment(vDoc, vQuery)
            Me.TreeViewDebug.Nodes.Add("<==  Ok Anticipos: " & Me.mFecha)
            Return True

        Catch ex As Exception
            Me.AxError = ex.Message
            Me.TreeViewDebug.Nodes.Add("<==   " & ex.Message)
            Return False
        End Try
    End Function
    Private Sub AXTrataenvioAnticipos(ByVal vStatus As Integer, ByVal vMessage As String)
        Try
            SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = " & vStatus
            SQL += " ,ASNT_ERR_MESSAGE = '" & Mid(vMessage, 1, 4000).Replace("'", "''").Trim & "'"
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHPrePaymentService'"

            ' Anticipos que no fueron exluidos del envio 
            SQL += " AND ASNT_AX_STATUS <> 9 "
            SQL += " AND ASNT_AX_STATUS <> 8 "
            SQL += " AND ASNT_AX_STATUS <> 7 "
            ' Anticipos que no estaban ya enviados ok 
            SQL += " AND ASNT_AX_STATUS <> 1 "
            '    SQL += " AND ASNT_AX_STATUS not in  (1,7,8,9) "



            Me.DbWrite.EjecutaSqlCommit(SQL)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
#End Region
#End Region



#Region "ENVIOS MASIVOS"
#Region "AX PRODUCCION"
    Private Sub AXProcesarProduccion()
        Try


            Dim ControlTipoProduccion As String = ""

            Me.WebProduccion = New WebReferenceProduccion.SAT_NHCreateProdOrdersQueryService
            'Actualiza Url
            Me.WebProduccion.Url = Me.mAxUrl & "SAT_NHCreateProdOrdersQueryService.asmx"


            ' Aactualiza TIME OUT

            If Me.NumericUpDownWebServiceTimeOut.Value > 0 Then
                If Me.CheckBoxTimeOut.Checked Then
                    Me.WebProduccion.Timeout = Me.NumericUpDownWebServiceTimeOut.Value * 1000
                End If
            End If




            Me.DocumentoContexto = New WebReferenceProduccion.DocumentContext
            Me.QweryOrders = New WebReferenceProduccion.AxdSAT_NHCreateProdOrdersQuery


            Me.WebProduccion.Credentials = New System.Net.NetworkCredential(Me.mAxUserName, Me.mAxUserPwd, Me.mAxDomainName)


            '     Me.DocumentoContexto.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContexto.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContexto.DestinationEndpoint = Me.mAXDestinationEndPoint
            Me.DocumentoContexto.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)

            Me.OrdersTabla(0) = New WebReferenceProduccion.AxdEntity_SAT_NHCreateProdOrdersTable_1


            Me.OrdersTabla(0).InvoiceDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
            Me.OrdersTabla(0).InvoiceDateSpecified = True


            Me.OrdersTabla(0).HotelId = Me.mHotelId


            ' 

            '    Me.ListBoxDebug.Items.Add(" => MessageId            : " & Me.DocumentoContexto.MessageId.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpoint       : " & Me.DocumentoContexto.SourceEndpoint.ToString)
            Me.ListBoxDebug.Items.Add(" => SourceEndpointUser   : " & Me.DocumentoContexto.SourceEndpointUser.ToString)
            Me.ListBoxDebug.Items.Add(" => DestinationEndpoint  : " & Me.DocumentoContexto.DestinationEndpoint.ToString)
            Me.ListBoxDebug.Items.Add(" => InvoiceDAte          : " & Me.OrdersTabla(0).InvoiceDate.ToString)


            ' Lineas de produccion 

            SQL = "SELECT ROWID,NVL(ASNT_CFCTA_COD,' ') AS CUENTA,NVL(ASNT_AMPCPTO,' ') AS CONCEPTO,"
            SQL += "NVL(ASNT_DPTO_CODI,'?') AS SERVICIO ,NVL(ASNT_I_MONEMP,'0')  AS IMPORTE"
            SQL += ", NVL(ASNT_TIPO_PROD,'?') AS ASNT_TIPO_PROD "
            SQL += ", NVL(ASNT_PROD_ID,'?') AS ASNT_PROD_ID "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHCreateProdOrdersQueryService'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "

            SQL += " ORDER BY ASNT_TIPO_PROD ASC"


            Me.Db.TraerLector(SQL)

            Dim Primerregistro As Boolean = True
            Elementos = 0


            Me.TreeViewDebug.Nodes.Add("==>  Producción: " & Me.mFecha)

            While Me.Db.mDbLector.Read

                Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1).Nodes.Add(CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(50, " ") & CType(Me.Db.mDbLector.Item("IMPORTE"), String))
                Me.TreeViewDebug.SelectedNode = Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1)
                Me.TreeViewDebug.ExpandAll()
                Me.TreeViewDebug.Update()

                '
                If Primerregistro = True Then
                    ControlTipoProduccion = Me.Db.mDbLector.Item("ASNT_TIPO_PROD")
                    Primerregistro = False
                    Elementos = 0
                    Ind = 0
                    ReDim Me.OrdersLine(Ind)

                Else                ' añade un elemento a array de lineas / Objeto
                    ReDim Preserve Me.OrdersLine(UBound(Me.OrdersLine) + 1)
                    Ind = UBound(Me.OrdersLine)
                End If



                If ControlTipoProduccion <> Me.Db.mDbLector.Item("ASNT_TIPO_PROD") Then
                    ' LLAMADA AL WEB SERVICE

                    If Elementos > 0 Then

                        Me.OrdersTabla(0).SAT_NHCreateProdOrdersLine_1 = Me.OrdersLine
                        Me.QweryOrders.SAT_NHCreateProdOrdersTable_1 = Me.OrdersTabla


                        If Me.AXEnviaProduccion(Me.DocumentoContexto, Me.QweryOrders) = True Then
                            Me.ListBoxDebug.Items.Add(" <== OK: ")
                            Me.ListBoxDebug.Update()
                            Me.AXTrataenvioProduccion(1, "OK", ControlTipoProduccion)
                        Else
                            Me.TotalErrores = Me.TotalErrores + 1
                            Me.AXTrataenvioProduccion(0, Me.AxError, ControlTipoProduccion)
                            Me.ListBoxDebug.Items.Add(" <== : " & Me.AxError)
                            Me.ListBoxDebug.SelectedIndex = Me.ListBoxDebug.Items.Count - 1
                            Me.ListBoxDebug.Update()
                        End If
                    End If

                    ControlTipoProduccion = Me.Db.mDbLector.Item("ASNT_TIPO_PROD")
                    Ind = 0
                    Elementos = 0
                    ReDim Me.OrdersLine(Ind)
                End If

                Me.OrdersLine(Ind) = New WebReferenceProduccion.AxdEntity_SAT_NHCreateProdOrdersLine_1
                Me.OrdersLine(Ind).ItemId = Me.Db.mDbLector.Item("ASNT_PROD_ID")




                If Me.Db.mDbLector.Item("SERVICIO") = Me.mAxServCodiBonos Then
                    'Or Me.Db.mDbLector.Item("SERVICIO") = Me.mAxServCodiBonosAsoc Then
                    Me.OrdersTabla(0).Bond = WebReferenceProduccion.AxdExtType_NoYesId.Yes
                    Me.OrdersTabla(0).BondSpecified = True
                Else
                    Me.OrdersTabla(0).Bond = WebReferenceProduccion.AxdExtType_NoYesId.No
                    Me.OrdersTabla(0).BondSpecified = True
                End If

                Me.OrdersLine(Ind).SalesPrice = Me.Db.mDbLector.Item("IMPORTE")
                Me.OrdersLine(Ind).SalesPriceSpecified = True

                If CType(Me.Db.mDbLector.Item("IMPORTE"), Double) < 0 Then
                    Me.OrdersLine(Ind).Qty = -1
                    Me.OrdersLine(Ind).SalesPrice = Me.Db.mDbLector.Item("IMPORTE") * -1
                Else
                    Me.OrdersLine(Ind).Qty = 1
                End If

                Me.OrdersLine(Ind).QtySpecified = True

                Me.ListBoxDebug.Items.Add(" ==> ItemIds y Qtys : " & CStr(Me.OrdersLine(Ind).ItemId).PadRight(15, " ") & CType(Me.Db.mDbLector.Item("CONCEPTO"), String).PadRight(50, " ") & " " & CType(Me.Db.mDbLector.Item("IMPORTE"), String).PadRight(15, " ") & "  Bono : " & Me.OrdersTabla(0).Bond.ToString & " " & CType(Me.Db.mDbLector.Item("ASNT_TIPO_PROD"), String).PadRight(15, " ") & "  " & CType(Me.OrdersLine(Ind).Qty, String))
                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1


                Elementos = Me.OrdersLine.GetLength(Dimension)
                Me.TextBoxDebug.Text = Elementos


                'Application.DoEvents()



            End While
            Me.Db.mDbLector.Close()


            ' ENVIO ULTIMO TIPO DE PRODUCCION DESPUES DEL AT END DEL RECORDSET 

            If Elementos > 0 Then

                Me.OrdersTabla(0).SAT_NHCreateProdOrdersLine_1 = Me.OrdersLine
                Me.QweryOrders.SAT_NHCreateProdOrdersTable_1 = Me.OrdersTabla


                If Me.AXEnviaProduccion(Me.DocumentoContexto, Me.QweryOrders) = True Then
                    Me.ListBoxDebug.Items.Add(" <== OK: ")
                    Me.ListBoxDebug.Update()
                    Me.AXTrataenvioProduccion(1, "OK", ControlTipoProduccion)
                Else
                    Me.TotalErrores = Me.TotalErrores + 1
                    Me.AXTrataenvioProduccion(0, Me.AxError, ControlTipoProduccion)
                    Me.ListBoxDebug.Items.Add(" <== : " & Me.AxError)
                    Me.ListBoxDebug.SelectedIndex = Me.ListBoxDebug.Items.Count - 1
                    Me.ListBoxDebug.Update()
                End If
            End If


        Catch ex As Exception
            Dim s As New System.Web.Services.Protocols.SoapException
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service Producción " & Me.WebProduccion.Url & " : " & ex.Message & " + " & s.Message)
            MsgBox(ex.Message)
        End Try
        '   Catch ex As Web.Services.Protocols.SoapException
        '       MsgBox(ex.Message)'

        'End Try

    End Sub
    Private Function AXEnviaProduccion(ByVal vDoc As WebReferenceProduccion.DocumentContext, ByVal vQuery As WebReferenceProduccion.AxdSAT_NHCreateProdOrdersQuery) As Boolean
        Try
            Me.AxError = ""
            vDoc.MessageId = Guid.NewGuid.ToString

            '  Me.ListBoxDebug.Items.Add(" = > " & vDoc.MessageId)
            Me.WebProduccion.createListSAT_NHCreateProdOrdersQuery(vDoc, vQuery)
            Me.TreeViewDebug.Nodes.Add("<==  Ok Producción: " & Me.mFecha)
            Return True

        Catch ex As Exception
            Me.AxError = ex.Message
            Me.TreeViewDebug.Nodes.Add("<==   " & ex.Message)
            Return False
        Finally
            ' Destruir los Objetos
            '   Me.WebProduccion.Dispose()

        End Try
    End Function
    Private Sub AXTrataenvioProduccion(ByVal vStatus As Integer, ByVal vMessage As String, ByVal vTipoProduccion As String)
        Try
            SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = " & vStatus
            SQL += " ,ASNT_ERR_MESSAGE = '" & Mid(vMessage, 1, 4000).Replace("'", "''").Trim & "'"
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHCreateProdOrdersQueryService'"
            SQL += " AND ASNT_TIPO_PROD = '" & vTipoProduccion & "'"
            Me.DbWrite.EjecutaSqlCommit(SQL)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

#End Region
#Region "AX INVENTARIO"
    Private Sub AXProcesarVentaTikets()
        Try

            '   Dim Ind As Integer

            Dim Primerregistro As Boolean = True

            Dim ControlAlmacen As String = ""
            Dim UltimoAlmacen As String = ""

            Dim ControlFacturaBono As Boolean = False
            Dim ControlFacturaOtrosServicios As Boolean = False


            Me.WebTikets = New WebReferenceVentaTikets.SAT_JournalLossProfitQueryService

            'Actualiza Url

            Me.WebTikets.Url = Me.mAxUrl & "SAT_JournalLossProfitQueryService.asmx"

            ' Aactualiza TIME OUT

            If Me.NumericUpDownWebServiceTimeOut.Value > 0 Then
                If Me.CheckBoxTimeOut.Checked Then
                    Me.WebTikets.Timeout = Me.NumericUpDownWebServiceTimeOut.Value * 1000
                End If
            End If


            Me.TextBoxRutaWebServices.Text = Me.WebTikets.Url
            Me.TextBoxRutaWebServices.Update()

            Me.DocumentoContextoTikets = New WebReferenceVentaTikets.DocumentContext
            Me.QweryOrdersTikets = New WebReferenceVentaTikets.AxdSAT_JournalLossProfitQuery



            Me.WebTikets.Credentials = New System.Net.NetworkCredential(Me.mAxUserName, Me.mAxUserPwd, Me.mAxDomainName)
            '


            '    Me.DocumentoContextoTikets.MessageId = Guid.NewGuid.ToString
            Me.DocumentoContextoTikets.SourceEndpoint = Me.mAXSourceEndPoint
            Me.DocumentoContextoTikets.DestinationEndpoint = Me.mAXDestinationEndPoint
            Me.DocumentoContextoTikets.SourceEndpointUser = Environment.ExpandEnvironmentVariables(Me.mAxDomainName & "\" & Me.mAxUserName)


            ' TIKETS

            '  MsgBox("ojo las cantidades se envian negativas  y no se envian las devoluciones ", MsgBoxStyle.Information, "revisar")


            SQL = "SELECT ROWID ,ASNT_PROD_ID AS PRODUCTO,NVL(ASNT_AMPCPTO,'?') AS DESCRIPCION,ASNT_PROD_TALLA AS TALLA,ASNT_PROD_COLOR AS COLOR ,NVL(ASNT_I_MONEMP,'0') * -1  AS CANTIDAD,ASNT_ALMA_AX AS ALMACEN "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_JournalLossProfitQueryService'"
            ' SOLO SIN PROCESAR
            SQL += " AND ASNT_AX_STATUS =  0 "
            SQL += " ORDER BY ASNT_ALMA_AX ASC "

            Me.Db.TraerLector(SQL)

            Primerregistro = True
            Elementos = 0


            Me.TreeViewDebug.Nodes.Add("==>  Stock Vendido : " & Me.mFecha)
            While Me.Db.mDbLector.Read


                Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1).Nodes.Add(CType(Me.Db.mDbLector.Item("DESCRIPCION"), String).PadRight(50, " ") & CType(Me.Db.mDbLector.Item("CANTIDAD"), String))
                Me.TreeViewDebug.SelectedNode = Me.TreeViewDebug.Nodes(Me.TreeViewDebug.Nodes.Count - 1)
                Me.TreeViewDebug.ExpandAll()
                Me.TreeViewDebug.Update()


                If Primerregistro = True Then
                    ControlAlmacen = Me.Db.mDbLector.Item("ALMACEN")
                    Primerregistro = False
                    Elementos = 0
                    Ind = 0
                    ReDim Me.OrdersLineTikets(Ind)

                Else                ' añade un elemento a array de lineas / Objeto
                    ReDim Preserve Me.OrdersLineTikets(UBound(Me.OrdersLineTikets) + 1)
                    Ind = UBound(Me.OrdersLineTikets)
                End If


                If ControlAlmacen <> Me.Db.mDbLector.Item("ALMACEN") Then
                    ' LLAMADA AL WEB SERVICE
                    Me.QweryOrdersTikets.SAT_NHJournalLossProfitTable = Me.OrdersLineTikets

                    If Elementos > 0 Then
                        If Me.AXEnviaVentaTikets(Me.DocumentoContextoTikets, Me.QweryOrdersTikets) = True Then
                            Me.ListBoxDebug.Items.Add(" <== OK: ")
                            Me.ListBoxDebug.Update()
                            Me.AXTrataenvioIventario(1, "OK", Me.Db.mDbLector.Item("ALMACEN"))
                        Else
                            Me.TotalErrores = Me.TotalErrores + 1
                            Me.AXTrataenvioIventario(0, Me.AxError, Me.Db.mDbLector.Item("ALMACEN"))
                            Me.ListBoxDebug.Items.Add(" <== : " & Me.AxError)
                            Me.ListBoxDebug.SelectedIndex = Me.ListBoxDebug.Items.Count - 1
                            Me.ListBoxDebug.Update()

                        End If
                    End If

                    ControlAlmacen = Me.Db.mDbLector.Item("ALMACEN")
                    Ind = 0
                    Elementos = 0
                    ReDim Me.OrdersLineTikets(Ind)
                End If


                Me.OrdersLineTikets(Ind) = New WebReferenceVentaTikets.AxdEntity_SAT_NHJournalLossProfitTable


                ' articulo

                Me.OrdersLineTikets(Ind).ItemId = Me.Db.mDbLector.Item("PRODUCTO")

                ' talla
                If IsDBNull(Me.Db.mDbLector.Item("TALLA")) = False Then
                    Me.OrdersLineTikets(Ind).InventSizeId = Me.Db.mDbLector.Item("TALLA")
                End If



                ' color
                If IsDBNull(Me.Db.mDbLector.Item("COLOR")) = False Then
                    Me.OrdersLineTikets(Ind).InventColorId = Me.Db.mDbLector.Item("COLOR")
                End If



                ' cantidad
                Me.OrdersLineTikets(Ind).Qty = Me.Db.mDbLector.Item("CANTIDAD")
                Me.OrdersLineTikets(Ind).QtySpecified = True

                ' almacen
                Me.OrdersLineTikets(Ind).InventLocationId = Me.Db.mDbLector.Item("ALMACEN")



                ' fecha
                Me.OrdersLineTikets(Ind).TransDate = CType(Format(Me.mFecha, "dd/MM/yyyy"), Date)
                Me.OrdersLineTikets(Ind).TransDateSpecified = True



                Me.OrdersLineTikets(Ind).HotelId = Me.mHotelId

                Me.ListBoxDebug.Items.Add(" ==> Venta de Tikets  : " & CType(Me.Db.mDbLector.Item("DESCRIPCION"), String).PadRight(40, " ") & CType(Me.OrdersLineTikets(Ind).Qty, String))
                Me.ListBoxDebug.Update()
                Me.ListBoxDebug.TopIndex = Me.ListBoxDebug.Items.Count - 1


                Elementos = Me.OrdersLine.GetLength(Dimension)
                Me.TextBoxDebug.Text = Elementos

                'Application.DoEvents()
                UltimoAlmacen = Me.Db.mDbLector.Item("ALMACEN")

            End While
            Me.Db.mDbLector.Close()

            ' ENVIO ULTIMO ALMACEN DESPUES DEL AT END DEL RECORDSET 

            If Elementos > 0 Then
                ' LLAMADA AL WEB SERVICE
                Me.QweryOrdersTikets.SAT_NHJournalLossProfitTable = Me.OrdersLineTikets

                If Me.AXEnviaVentaTikets(Me.DocumentoContextoTikets, Me.QweryOrdersTikets) = True Then
                    Me.ListBoxDebug.Items.Add(" <== OK: ")
                    Me.ListBoxDebug.Update()
                    Me.AXTrataenvioIventario(1, "OK", UltimoAlmacen)
                Else
                    Me.TotalErrores = Me.TotalErrores + 1
                    Me.AXTrataenvioIventario(0, Me.AxError, UltimoAlmacen)
                    Me.ListBoxDebug.Items.Add(" <== : " & Me.AxError)
                    Me.ListBoxDebug.SelectedIndex = Me.ListBoxDebug.Items.Count - 1
                    Me.ListBoxDebug.Update()

                End If
            End If



        Catch ex As Exception
            Me.ListBoxDebug.Items.Add(Format(Now, "F") & " Error Llamada al Web Service Tikets " & Me.WebTikets.Url & " : " & ex.Message)

            MsgBox(ex.Message)
            '     MsgBox(ex.ToString)
        End Try
        '   Catch ex As Web.Services.Protocols.SoapException
        '       MsgBox(ex.Message)'

        'End Try

    End Sub
    Private Function AXEnviaVentaTikets(ByVal vDoc As WebReferenceVentaTikets.DocumentContext, ByVal vQuery As WebReferenceVentaTikets.AxdSAT_JournalLossProfitQuery) As Boolean
        Try
            Me.AxError = ""
            vDoc.MessageId = Guid.NewGuid.ToString
            Me.WebTikets.createListSAT_JournalLossProfitQuery(vDoc, vQuery)
            Me.TreeViewDebug.Nodes.Add("<==  Ok Inventario: " & Me.mFecha)
            Me.TreeViewDebug.Nodes("==>  Stock Vendido : " & Me.mFecha).Checked = True
            Return True

        Catch ex As Exception
            Me.AxError = ex.Message
            Me.TreeViewDebug.Nodes.Add("<==   " & ex.Message)

            Return False
        End Try
    End Function
    Private Sub AXTrataenvioIventario(ByVal vStatus As Integer, ByVal vMessage As String, ByVal vAlmacen As String)
        Try
            SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = " & vStatus
            SQL += " ,ASNT_ERR_MESSAGE = '" & Mid(vMessage, 1, 4000).Replace("'", "''").Trim & "'"
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_JournalLossProfitQueryService'"
            SQL += " AND ASNT_ALMA_AX ='" & vAlmacen & "'"
            Me.DbWrite.EjecutaSqlCommit(SQL)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
#End Region
#End Region

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Try
            Dim F As New FormPideFechas(Me.mFecha)
            CONTROLF = False
            F.ShowDialog()

            If CONTROLF = False Then
                ' salio con cancelar del form pide fechas 
                Exit Sub
            End If

            Me.Cursor = Cursors.WaitCursor

            REPORT_SELECTION_FORMULA = "{TH_ASNT.ASNT_F_VALOR}>=DATETIME(" & Format(FEC1, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += "AND {TH_ASNT.ASNT_F_VALOR}<=DATETIME(" & Format(FEC2, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"
            REPORT_SELECTION_FORMULA += " AND  {@MyAxapta}= 'AX'"
            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_AX_STATUS}= 0 "
            REPORT_SELECTION_FORMULA += " AND ISNULL({TH_ASNT.ASNT_ERR_MESSAGE}) = FALSE AND {TH_ASNT.ASNT_ERR_MESSAGE} <> 'OK'"


            'Dim Form As New FormVisorCrystal("ASIENTO AXAPTA.RPT", "", REPORT_SELECTION_FORMULA, Me.mStrConexion, "", False, False)
            Dim Form As New FormVisorCrystal("ASIENTO AXAPTA ERRORES.RPT", "Errores " & Format(FEC1, "dd/MM/yyyy") & "   " & Format(FEC2, "dd/MM/yyyy"), REPORT_SELECTION_FORMULA, Me.mStrConexion, "", False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

#Region "RUTINAS TEMPORALES SOLO PARA PRUEBAS"
    Private Function DEBUGEnviarCobro(ByVal vDocumento As String) As Boolean
        Try


            Dim ControlFacturaBono As Boolean = False
            Dim ControlFacturaOtrosServicios As Boolean = False


            SQL = "SELECT  ASNT_LIN_DOCU AS DOCUMENTO, ASNT_DPTO_CODI AS SERVICIO,"
            SQL += "ASNT_LIN_IMP1 AS VALORIMPUESTO,ASNT_LIN_VLIQ AS VALORLIQUIDO,ASNT_LIN_VCRE AS TOTAL,ASNT_LIN_TIIMP AS PORCE "
            SQL += " ,NVL(ASNT_TIPO_VENTA,'?') AS TIPOVENTA,NVL(ASNT_NOMBRE,'?') AS CONCEPTO,NVL(ASNT_PROD_ID,'?') AS ASNT_PROD_ID "

            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND ASNT_LIN_DOCU = '" & vDocumento & "'"
            SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHCreateSalesOrdersQueryService'"
            SQL += " ORDER BY ASNT_LIN_DOCU ASC "

            Me.DbAux.TraerLector(SQL)

            While Me.DbAux.mDbLector.Read
                ' control que de no enviar facturas que contengan Bonos y otros servicios a la vez 

                If Me.DbAux.mDbLector.Item("SERVICIO") = Me.mAxServCodiBonos Or Me.DbAux.mDbLector.Item("SERVICIO") = Me.mAxServCodiBonosAsoc Or Me.DbAux.mDbLector.Item("TIPOVENTA") = "BONOS" Then
                    ControlFacturaBono = True
                    Me.OrdersTablaFacturas(0).Bond = WebReferenceFacturas.AxdExtType_NoYesId.Yes
                    Me.OrdersTablaFacturas(0).BondSpecified = True
                End If

                If Me.DbAux.mDbLector.Item("SERVICIO") <> Me.mAxServCodiBonos And Me.DbAux.mDbLector.Item("SERVICIO") <> Me.mAxServCodiBonosAsoc And Me.DbAux.mDbLector.Item("TIPOVENTA") <> "BONOS" Then
                    ControlFacturaOtrosServicios = True
                    Me.OrdersTablaFacturas(0).Bond = WebReferenceFacturas.AxdExtType_NoYesId.No
                    Me.OrdersTablaFacturas(0).BondSpecified = True
                End If
            End While

            Me.DbAux.mDbLector.Close()


            If (ControlFacturaBono = True And ControlFacturaOtrosServicios = False) Or (ControlFacturaBono = False And ControlFacturaOtrosServicios = True) Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Return False
            MsgBox(ex.Message)
        End Try

    End Function

#End Region

    Private Sub ButtonAuditaNif_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAuditaNif.Click
        Try
            If Me.AuditaNif = False Then
                MsgBox("Nif Ok  ( Ni Zero ni Vacío)")
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonAyuda_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try

            Dim Texto As String

            Texto = " (1) Cliente Contado o Contclie = yes  = " & vbCrLf
            Texto += "    Es cuando la Factura NO tiene un Codigo de Entidad o Cuenta de No alojado " & vbCrLf
            Texto += "    O Cuando la Factura tiene un Nif que coincide con el Cif Genérico de Contado de los Parámetros Generales " & vbCrLf & vbCrLf
            Texto += " (2)   = " & vbCrLf

            MessageBox.Show(Texto, "Ayuda ...", MessageBoxButtons.OK, MessageBoxIcon.Information)



        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            REPORT_SELECTION_FORMULA = "{TH_ASNT.ASNT_F_VALOR}=DATETIME(" & Format(Me.mFecha, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"
            REPORT_SELECTION_FORMULA += " AND  {@MyAxapta}= 'AX'"


            'Dim Form As New FormVisorCrystal("ASIENTO AXAPTA.RPT", "", REPORT_SELECTION_FORMULA, Me.mStrConexion, "", False, False)
            Dim Form As New FormVisorCrystal("ASIENTO AXAPTA4.RPT", "", REPORT_SELECTION_FORMULA, Me.mStrConexion, "", False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub
    Private Sub DataGridViewAsientos_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridViewAsientos.SelectionChanged
        Try




            If IsNothing(DataGridViewAsientos.CurrentRow) = False Then
                If IsNothing(DataGridViewAsientos.CurrentRow.Index) = False Then
                    If Me.DataGridViewAsientos.CurrentRow.Index > -1 And Me.HayRegistros = True Then

                        Me.MenuContextualPintar()


                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ToolStripMenuItemCambiarEstado_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemCambiarEstado.Click
        Try
            Dim Seleccion As Integer
            Dim PrimeraVisible As Integer
            Seleccion = Me.DataGridViewAsientos.CurrentRow.Index
            PrimeraVisible = DataGridViewAsientos.FirstDisplayedScrollingRowIndex


            ' SI NO HA SIDO ENVIADO 
            If Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "No Enviado" Then
                SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = 8 WHERE ROWID = '" & Me.DataGridViewAsientos.Item("ROWID", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                Me.DbWrite.EjecutaSqlCommit(SQL)
                ' Reenviar
            ElseIf Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "Omitido" Then
                SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = 0 WHERE ROWID = '" & Me.DataGridViewAsientos.Item("ROWID", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                Me.DbWrite.EjecutaSqlCommit(SQL)
            End If



            Me.MostrarAsientos()
            DataGridViewAsientos.FirstDisplayedScrollingRowIndex = PrimeraVisible
            DataGridViewAsientos.Refresh()




        Catch ex As Exception

        End Try
    End Sub



    Private Sub DataGridViewAsientos_RowContextMenuStripNeeded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowContextMenuStripNeededEventArgs) Handles DataGridViewAsientos.RowContextMenuStripNeeded

        Try


            Dim Seleccion As Integer
            Seleccion = e.RowIndex
            '    Seleccion = Me.DataGridViewAsientos.CurrentRow.Index



            Me.TextBoxDebug.Text = e.RowIndex & "  " & Seleccion

            ' SELECIONAR LA MISMA FILA
            DataGridViewAsientos.Rows(Seleccion).Selected = True
            DataGridViewAsientos.CurrentCell = DataGridViewAsientos.Rows(Seleccion).Cells(1)




            ' SIMULA SELECTION CHANGE
            If IsNothing(DataGridViewAsientos.CurrentRow) = False Then
                If IsNothing(DataGridViewAsientos.CurrentRow.Index) = False Then
                    If Me.DataGridViewAsientos.CurrentRow.Index > -1 And Me.HayRegistros = True Then

                        Me.MenuContextualPintar()

                    End If
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub



    Private Sub CheckBoxRedondeaFactura_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxRedondeaFactura.CheckedChanged
        Try
            If Me.CheckBoxRedondeaFactura.CheckState = CheckState.Checked Then
                Me.CheckBoxRedondeaFacturaTest.CheckState = CheckState.Unchecked
                Me.NumericUpDownredondeoBaseAjuste.Enabled = False
                Exit Sub
            End If

            If Me.CheckBoxRedondeaFactura.CheckState = CheckState.Unchecked Then
                Me.CheckBoxRedondeaFacturaTest.CheckState = CheckState.Checked
                Me.NumericUpDownredondeoBaseAjuste.Enabled = True
                Exit Sub
            End If



        Catch ex As Exception

        End Try
    End Sub

    Private Sub CheckBoxRedondeaFacturaTest_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxRedondeaFacturaTest.CheckedChanged
        Try
            If Me.CheckBoxRedondeaFacturaTest.CheckState = CheckState.Checked Then
                Me.CheckBoxRedondeaFactura.CheckState = CheckState.Unchecked
                Exit Sub
            End If

            If Me.CheckBoxRedondeaFacturaTest.CheckState = CheckState.Unchecked Then
                Me.CheckBoxRedondeaFactura.CheckState = CheckState.Checked
                Exit Sub
            End If



        Catch ex As Exception

        End Try
    End Sub

    Private Sub CheckBoxExcluidos_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxExcluidos.CheckedChanged
        Try

            If Me.CheckBoxExcluidos.Checked = True Then
                Me.CheckBoxSoloPendientes.Checked = False
                Me.CheckBoxSoloPendientes.Update()
                Me.CheckBoxVerTodos.Checked = False
                Me.CheckBoxVerTodos.Update()
                Me.CheckBoxBoockId.Checked = False
                Me.CheckBoxBoockId.Update()
            End If
            Me.MostrarAsientos()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub CheckBoxVerTodos_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxVerTodos.CheckedChanged
        Try

            If Me.CheckBoxVerTodos.Checked = True Then
                Me.CheckBoxSoloPendientes.Checked = False
                Me.CheckBoxSoloPendientes.Update()
                Me.CheckBoxExcluidos.Checked = False
                Me.CheckBoxExcluidos.Update()
                Me.CheckBoxBoockId.Checked = False
                Me.CheckBoxBoockId.Update()

            End If
            Me.MostrarAsientos()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub FormConvertirAxapta_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            Me.CheckBoxVerTodos.Checked = True
            Me.CheckBoxVerTodos.Update()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Try

            Me.Cursor = Cursors.WaitCursor
            Me.ActualizaNif()
            Me.Cursor = Cursors.Default

        Catch ex As Exception

        End Try
    End Sub


    Private Sub ToolStripMenuItemCambiarEstado2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemCambiarEstado2.Click
        Try
            Dim Seleccion As Integer
            Dim PrimeraVisible As Integer
            Seleccion = Me.DataGridViewAsientos.CurrentRow.Index
            PrimeraVisible = DataGridViewAsientos.FirstDisplayedScrollingRowIndex


            ' SI NO HA SIDO ENVIADO 
            If Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "No Enviado" Then
                SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = 7 WHERE ROWID = '" & Me.DataGridViewAsientos.Item("ROWID", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                Me.DbWrite.EjecutaSqlCommit(SQL)
                ' Reenviar
            ElseIf Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "Corregido" Then
                SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = 0 WHERE ROWID = '" & Me.DataGridViewAsientos.Item("ROWID", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                Me.DbWrite.EjecutaSqlCommit(SQL)
            End If



            Me.MostrarAsientos()
            DataGridViewAsientos.FirstDisplayedScrollingRowIndex = PrimeraVisible
            DataGridViewAsientos.Refresh()




        Catch ex As Exception

        End Try
    End Sub

    Private Sub ToolStripMenuItemCambiarEstado3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemCambiarEstado3.Click
        Try
            Dim Seleccion As Integer
            Dim PrimeraVisible As Integer
            Dim Cif As String
            Seleccion = Me.DataGridViewAsientos.CurrentRow.Index
            PrimeraVisible = DataGridViewAsientos.FirstDisplayedScrollingRowIndex



            If Me.CheckBoxModificaNif.Checked = False Then
                ' SI NO HA SIDO ENVIADO 
                If Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "No Enviado" Then
                    ' SI ES ZERO O VACIO 
                    If Me.DataGridViewAsientos.Item("NIF/CIF", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "0" Or Me.DataGridViewAsientos.Item("NIF/CIF", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "" Then
                        Cif = InputBox("Ingrese el Cif", "Actualizar el Cif de una Factura")
                        SQL = "UPDATE TH_ASNT SET ASNT_CIF = '" & Cif & "'"
                        SQL += " WHERE ASNT_LIN_DOCU =  '" & Me.DataGridViewAsientos.Item("DOCUMENTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                        SQL += " AND ASNT_WEBSERVICE_NAME = '" & Me.DataGridViewAsientos.Item("ASNT_WEBSERVICE_NAME", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"

                        SQL += " AND  ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
                        SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                        SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"

                        Me.DbWrite.EjecutaSqlCommit(SQL)
                    Else
                        MsgBox("No se puede Actualizar este Registro  Nif No es Nulo", MsgBoxStyle.Information, "Atención")

                    End If

                Else
                    MsgBox("No se puede Actualizar este Registro por su Estado ... ya esta enviado ", MsgBoxStyle.Information, "Atención")
                End If
            End If

            If Me.CheckBoxModificaNif.Checked = True Then
                ' SI NO HA SIDO ENVIADO 
                If Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "No Enviado" Then
                    Cif = InputBox("Ingrese el Cif", "Actualizar el Cif de una Factura")
                    SQL = "UPDATE TH_ASNT SET ASNT_CIF = '" & Cif & "'"
                    SQL += " WHERE ASNT_LIN_DOCU =  '" & Me.DataGridViewAsientos.Item("DOCUMENTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                    SQL += " AND ASNT_WEBSERVICE_NAME = '" & Me.DataGridViewAsientos.Item("ASNT_WEBSERVICE_NAME", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"

                    SQL += " AND  ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
                    SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                    SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"

                    Me.DbWrite.EjecutaSqlCommit(SQL)
                Else
                    MsgBox("No se puede Actualizar este Registro por su Estado ... ya esta enviado ", MsgBoxStyle.Information, "Atención")
                End If
            End If




            Me.MostrarAsientos()
            DataGridViewAsientos.FirstDisplayedScrollingRowIndex = PrimeraVisible
            DataGridViewAsientos.Refresh()




        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimeExclusiones.Click
        Try
            Dim F As New FormPideFechas(Me.mFecha)
            CONTROLF = False
            F.ShowDialog()

            If CONTROLF = False Then
                ' salio con cancelar del form pide fechas 
                Exit Sub
            End If
            Me.Cursor = Cursors.WaitCursor

            REPORT_SELECTION_FORMULA = "{TH_ASNT.ASNT_F_VALOR}>=DATETIME(" & Format(FEC1, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += "AND {TH_ASNT.ASNT_F_VALOR}<=DATETIME(" & Format(FEC2, REPORT_DATE_FORMAT) & ")"

            REPORT_SELECTION_FORMULA += "  AND {TH_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"
            REPORT_SELECTION_FORMULA += " AND  {@MyAxapta}= 'AX'"
            REPORT_SELECTION_FORMULA += " AND ({TH_ASNT.ASNT_AX_STATUS}= 7 OR {TH_ASNT.ASNT_AX_STATUS}= 8 OR {TH_ASNT.ASNT_AX_STATUS}= 9)"



            Dim Form As New FormVisorCrystal("ASIENTO AXAPTA3.RPT", "Omitidos / Corregidos / Exluidos ", REPORT_SELECTION_FORMULA, Me.mStrConexion, "", False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub ButtonImprimeEnviosPendientes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimeEnviosPendientes.Click
        Try
            Dim Titulo As String = ""
            Dim F As New FormPideFechas(Me.mFecha)
            CONTROLF = False
            F.ShowDialog()

            If CONTROLF = False Then
                ' salio con cancelar del form pide fechas 
                Exit Sub
            End If

            Me.Cursor = Cursors.WaitCursor

            REPORT_SELECTION_FORMULA = "{TH_ASNT.ASNT_F_VALOR}>=DATETIME(" & Format(FEC1, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += "AND {TH_ASNT.ASNT_F_VALOR}<=DATETIME(" & Format(FEC2, REPORT_DATE_FORMAT) & ")"

            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"
            REPORT_SELECTION_FORMULA += " AND  {@MyAxapta}= 'AX'"
            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_AX_STATUS}= 0 "
            If RESULTBOOLEAN = True Then
                ' se excluye envios pendientes de tipo inventario
                REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_WEBSERVICE_NAME} <> 'SAT_JournalLossProfitQueryService' "
            End If


            If RESULTBOOLEAN = True Then
                Titulo = "Registros No Enviados y No se muestran Registros del Tipo Pérdidas y Ganancias !!   "
            Else
                Titulo = "Registros No Enviados  "

            End If


            Dim Form As New FormVisorCrystal("ASIENTO AXAPTA3.RPT", Titulo & Format(FEC1, "dd/MM/yyyy") & "   " & Format(FEC2, "dd/MM/yyyy"), REPORT_SELECTION_FORMULA, Me.mStrConexion, "", False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub


    Private Sub ButtonExtractoBookId_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExtractoBookId.Click
        Try
            Dim Titulo As String = "Extracto de Boock Ids "
            Dim F As New FormPideFechas(Me.mFecha)
            CONTROLF = False
            F.ShowDialog()

            If CONTROLF = False Then
                ' salio con cancelar del form pide fechas 
                Exit Sub
            End If

            Me.Cursor = Cursors.WaitCursor

            REPORT_SELECTION_FORMULA = "{TH_ASNT.ASNT_F_VALOR}>=DATETIME(" & Format(FEC1, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += "AND {TH_ASNT.ASNT_F_VALOR}<=DATETIME(" & Format(FEC2, REPORT_DATE_FORMAT) & ")"

            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"
            REPORT_SELECTION_FORMULA += " AND  {@MyAxapta}= 'AX'"
            REPORT_SELECTION_FORMULA += " AND ({TH_ASNT.ASNT_WEBSERVICE_NAME} = 'SAT_NHPrePaymentService'  "
            REPORT_SELECTION_FORMULA += " OR  {TH_ASNT.ASNT_WEBSERVICE_NAME} = 'SAT_NHJournalCustPaymentQueryService')  "




            Dim Form As New FormVisorCrystal("ASIENTO AXAPTA5.RPT", Titulo & Format(FEC1, "dd/MM/yyyy") & "   " & Format(FEC2, "dd/MM/yyyy"), REPORT_SELECTION_FORMULA, Me.mStrConexion, "", False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub ToolStripMenuItemCambiarEstadoEntodalaFactura_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemCambiarEstadoEntodalaFactura.Click
        Try
            Dim Seleccion As Integer
            Dim PrimeraVisible As Integer
            Seleccion = Me.DataGridViewAsientos.CurrentRow.Index
            PrimeraVisible = DataGridViewAsientos.FirstDisplayedScrollingRowIndex


            ' SI NO HA SIDO ENVIADO 
            If Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "No Enviado" Then
                SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = 8 "

                SQL += " WHERE ASNT_LIN_DOCU =  '" & Me.DataGridViewAsientos.Item("DOCUMENTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                SQL += " AND ASNT_WEBSERVICE_NAME = '" & Me.DataGridViewAsientos.Item("ASNT_WEBSERVICE_NAME", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"

                SQL += " AND  ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
                SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                ' no enviados
                SQL += " AND ASNT_AX_STATUS = 0 "


                Me.DbWrite.EjecutaSqlCommit(SQL)

                ' Reenviar
            ElseIf Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "Omitido" Then
                SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = 0 "

                SQL += " WHERE ASNT_LIN_DOCU =  '" & Me.DataGridViewAsientos.Item("DOCUMENTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                SQL += " AND ASNT_WEBSERVICE_NAME = '" & Me.DataGridViewAsientos.Item("ASNT_WEBSERVICE_NAME", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"

                SQL += " AND  ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
                SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                ' no enviados
                SQL += " AND ASNT_AX_STATUS = 8 "

                Me.DbWrite.EjecutaSqlCommit(SQL)

            End If

            Me.MostrarAsientos()
            DataGridViewAsientos.FirstDisplayedScrollingRowIndex = PrimeraVisible
            DataGridViewAsientos.Refresh()


        Catch ex As Exception

        End Try
    End Sub

    Private Sub ToolStripMenuItemCambiarEstado2EntodalaFactura_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemCambiarEstado2EntodalaFactura.Click
        Try
            Dim Seleccion As Integer
            Dim PrimeraVisible As Integer
            Seleccion = Me.DataGridViewAsientos.CurrentRow.Index
            PrimeraVisible = DataGridViewAsientos.FirstDisplayedScrollingRowIndex


            ' SI NO HA SIDO ENVIADO 
            If Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "No Enviado" Then
                SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = 7 "

                SQL += " WHERE ASNT_LIN_DOCU =  '" & Me.DataGridViewAsientos.Item("DOCUMENTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                SQL += " AND ASNT_WEBSERVICE_NAME = '" & Me.DataGridViewAsientos.Item("ASNT_WEBSERVICE_NAME", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"

                SQL += " AND  ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
                SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                ' no enviados
                SQL += " AND ASNT_AX_STATUS = 0 "

                Me.DbWrite.EjecutaSqlCommit(SQL)

            ElseIf Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "Corregido" Then
                SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = 0 "

                SQL += " WHERE ASNT_LIN_DOCU =  '" & Me.DataGridViewAsientos.Item("DOCUMENTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                SQL += " AND ASNT_WEBSERVICE_NAME = '" & Me.DataGridViewAsientos.Item("ASNT_WEBSERVICE_NAME", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"

                SQL += " AND  ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
                SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                ' no enviados
                SQL += " AND ASNT_AX_STATUS = 7 "


                Me.DbWrite.EjecutaSqlCommit(SQL)

            End If



            Me.MostrarAsientos()
            DataGridViewAsientos.FirstDisplayedScrollingRowIndex = PrimeraVisible
            DataGridViewAsientos.Refresh()



        Catch ex As Exception

        End Try
    End Sub

    Private Sub ToolStripMenuItemCambiarEstado4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemCambiarEstado4.Click
        Try
            Dim Seleccion As Integer
            Dim PrimeraVisible As Integer

            Seleccion = Me.DataGridViewAsientos.CurrentRow.Index
            PrimeraVisible = DataGridViewAsientos.FirstDisplayedScrollingRowIndex



            ' SI NO HA SIDO ENVIADO 
            If Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "No Enviado" Then

                SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = 1 "
                SQL += " WHERE ASNT_LIN_DOCU =  '" & Me.DataGridViewAsientos.Item("DOCUMENTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                SQL += " AND ASNT_WEBSERVICE_NAME = '" & Me.DataGridViewAsientos.Item("ASNT_WEBSERVICE_NAME", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"

                SQL += " AND  ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
                SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"

                Me.DbWrite.EjecutaSqlCommit(SQL)


            Else
                If MessageBox.Show("Esta Factura  ya ha sido enviada !!  Desea realmente modificar su estado ", "Atención", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                    ' SI SI HA SIDO ENVIADO 
                    SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = 0 "
                    SQL += " WHERE ASNT_LIN_DOCU =  '" & Me.DataGridViewAsientos.Item("DOCUMENTO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                    SQL += " AND ASNT_WEBSERVICE_NAME = '" & Me.DataGridViewAsientos.Item("ASNT_WEBSERVICE_NAME", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"

                    SQL += " AND  ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
                    SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                    SQL += " AND ASNT_EMP_COD = '" & Me.mEmpCod & "'"

                    Me.DbWrite.EjecutaSqlCommit(SQL)

                End If
            End If





            Me.MostrarAsientos()
            DataGridViewAsientos.FirstDisplayedScrollingRowIndex = PrimeraVisible
            DataGridViewAsientos.Refresh()




        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonGoma_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonGoma.Click
        Try

            Me.CheckBoxVerTodos.Checked = True
            Me.CheckBoxSoloPendientes.Checked = False
            Me.CheckBoxExcluidos.Checked = False
            Me.TextBoxFactura.Text = ""
            Me.CheckBoxBoockId.Checked = False
            Me.Update()
            Me.MostrarAsientos()



        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub ToolStripMenuItemExluir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemExluir.Click
        Try
            Dim Seleccion As Integer
            Dim PrimeraVisible As Integer
            Seleccion = Me.DataGridViewAsientos.CurrentRow.Index
            PrimeraVisible = DataGridViewAsientos.FirstDisplayedScrollingRowIndex


            ' SI NO HA SIDO ENVIADO 
            If Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "No Enviado" Then
                SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = 9 WHERE ROWID = '" & Me.DataGridViewAsientos.Item("ROWID", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                Me.DbWrite.EjecutaSqlCommit(SQL)
                ' Reenviar
            ElseIf Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "Excluido" Then
                SQL = "UPDATE TH_ASNT SET ASNT_AX_STATUS = 0 WHERE ROWID = '" & Me.DataGridViewAsientos.Item("ROWID", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                Me.DbWrite.EjecutaSqlCommit(SQL)
                SQL = "UPDATE TH_ASNT SET ASNT_AUX_UPDATE = 'Estaba Excluido 9'  WHERE ROWID = '" & Me.DataGridViewAsientos.Item("ROWID", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                Me.DbWrite.EjecutaSqlCommit(SQL)

            End If



            Me.MostrarAsientos()
            DataGridViewAsientos.FirstDisplayedScrollingRowIndex = PrimeraVisible
            DataGridViewAsientos.Refresh()




        Catch ex As Exception

        End Try
    End Sub

    Private Sub CheckBoxBoockId_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxBoockId.CheckedChanged
        Try
            If Me.CheckBoxBoockId.Checked = True Then
                Me.CheckBoxSoloPendientes.Checked = False
                Me.CheckBoxSoloPendientes.Update()
                Me.CheckBoxExcluidos.Checked = False
                Me.CheckBoxExcluidos.Update()
                Me.CheckBoxVerTodos.Checked = False
                Me.CheckBoxVerTodos.Update()
            End If
            Me.MostrarAsientos()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ToolStripMenuItemCambiarPrefijo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemCambiarPrefijo.Click
        Try
            Dim Seleccion As Integer
            Dim PrimeraVisible As Integer
            Seleccion = Me.DataGridViewAsientos.CurrentRow.Index
            PrimeraVisible = DataGridViewAsientos.FirstDisplayedScrollingRowIndex
            Dim Result As String

            Dim NuevoPrefijo As String = ""

            PASOSTRING = ""
            PASOSTRING2 = ""


            If Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value = "No Enviado" Then



                PASOSTRING = Me.DataGridViewAsientos.Item("PREFIJO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString
                Dim F As New FormCambiaPrefijo
                F.ShowDialog()

                If PASOSTRING = "ACEPTAR" Then
                    NuevoPrefijo = PASOSTRING2

                    ' SI NO HA SIDO ENVIADO 
                    If Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "No Enviado" Then
                        SQL = "UPDATE TH_ASNT SET ASNT_AUXILIAR_STRING2 = '" & NuevoPrefijo & "'"
                        SQL += " WHERE ROWID = '" & Me.DataGridViewAsientos.Item("ROWID", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                        Me.DbWrite.EjecutaSqlCommit(SQL)

                        SQL = "SELECT ASNT_AUX_UPDATE FROM TH_ASNT "
                        SQL += " WHERE ROWID = '" & Me.DataGridViewAsientos.Item("ROWID", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"

                        Result = Me.DbWrite.EjecutaSqlScalar(SQL)

                        ' Guarda el Prefijo Original en otro campo 
                        If Result = "" Then
                            SQL = "UPDATE TH_ASNT SET ASNT_AUX_UPDATE = 'Prefijo Original =  " & Me.DataGridViewAsientos.Item("PREFIJO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                            SQL += "   WHERE ROWID = '" & Me.DataGridViewAsientos.Item("ROWID", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                            Me.DbWrite.EjecutaSqlCommit(SQL)

                        End If

                    End If



                    Me.MostrarAsientos()
                    DataGridViewAsientos.FirstDisplayedScrollingRowIndex = PrimeraVisible
                    DataGridViewAsientos.Refresh()

                    Try
                        ' TRATA DE SELECIONAR LA MISMA LINEA
                        DataGridViewAsientos.CurrentCell = DataGridViewAsientos.Rows(Seleccion).Cells(1)

                    Catch ex As Exception

                    End Try

                End If
            Else
                MsgBox("No es Candidato !! , el Registro debe de estar ""No Enviado"" y debe de ser Cobro o Anticipo")
            End If



        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonJobCambiaFormadePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonJobCambiaFormadePago.Click
        Try
            Me.ProcesaJob1()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button7_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim Titulo As String = "Extracto de Boock Ids "
            Dim F As New FormPideFechas
            CONTROLF = False
            F.ShowDialog()

            If CONTROLF = False Then
                ' salio con cancelar del form pide fechas 
                Exit Sub
            End If

            Me.Cursor = Cursors.WaitCursor

            REPORT_SELECTION_FORMULA = "{TH_ASNT.ASNT_F_VALOR}>=DATETIME(" & Format(FEC1, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += "AND {TH_ASNT.ASNT_F_VALOR}<=DATETIME(" & Format(FEC2, REPORT_DATE_FORMAT) & ")"

            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"
            REPORT_SELECTION_FORMULA += " AND  {@MyAxapta}= 'AX'"
            REPORT_SELECTION_FORMULA += " AND ({TH_ASNT.ASNT_WEBSERVICE_NAME} = 'SAT_NHPrePaymentService'  "
            REPORT_SELECTION_FORMULA += " OR  {TH_ASNT.ASNT_WEBSERVICE_NAME} = 'SAT_NHJournalCustPaymentQueryService')  "




            Dim Form As New FormVisorCrystal("ASIENTO AXAPTA5.RPT", Titulo & Format(FEC1, "dd/MM/yyyy") & "   " & Format(FEC2, "dd/MM/yyyy"), REPORT_SELECTION_FORMULA, Me.mStrConexion, "", False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub


    Private Sub UpdateImporteDelCobto001ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateImporteDelCobto001ToolStripMenuItem.Click
        Try
            Dim Seleccion As Integer
            Dim PrimeraVisible As Integer
            Seleccion = Me.DataGridViewAsientos.CurrentRow.Index
            PrimeraVisible = DataGridViewAsientos.FirstDisplayedScrollingRowIndex
            Dim Result As String

            Dim NuevoImporte As Double

            PASOSTRING = ""
            PASOSTRING2 = ""


            If Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value = "No Enviado" Then



                PASOSTRING = Me.DataGridViewAsientos.Item("IMPORTE", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString
                Dim F As New FormCambiarDato
                F.StartPosition = FormStartPosition.CenterScreen
                F.ShowDialog()

                If PASOSTRING = "ACEPTAR" Then


                    NuevoImporte = CDbl(PASOSTRING2.Replace(",", "."))

                    ' SI NO HA SIDO ENVIADO 
                    If Me.DataGridViewAsientos.Item("ESTADO", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString = "No Enviado" Then
                        Me.DbWrite.IniciaTransaccion()

                        SQL = "UPDATE TH_ASNT SET ASNT_I_MONEMP = " & NuevoImporte
                        SQL += ", ASNT_DEBE = " & NuevoImporte
                        SQL += " WHERE ROWID = '" & Me.DataGridViewAsientos.Item("ROWID", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                        Me.DbWrite.EjecutaSql(SQL)

                        SQL = "SELECT ASNT_LOG FROM TH_ASNT "
                        SQL += " WHERE ROWID = '" & Me.DataGridViewAsientos.Item("ROWID", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                        SQL += " AND ASNT_LOG LIKE '%" & "Cobro Original" & "%'"

                        Result = Me.DbWrite.EjecutaSqlScalar(SQL)

                        ' Guarda el Prefijo Original en otro campo 
                        If Result = "" Then
                            SQL = "UPDATE TH_ASNT SET ASNT_LOG = 'Cobro Original =  " & Me.DataGridViewAsientos.Item("IMPORTE", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                            SQL += "   WHERE ROWID = '" & Me.DataGridViewAsientos.Item("ROWID", Me.DataGridViewAsientos.CurrentRow.Index).Value.ToString & "'"
                            Me.DbWrite.EjecutaSql(SQL)

                        End If

                        Me.DbWrite.ConfirmaTransaccion()
                    End If



                    Me.MostrarAsientos()
                    DataGridViewAsientos.FirstDisplayedScrollingRowIndex = PrimeraVisible
                    DataGridViewAsientos.Refresh()

                    Try
                        ' TRATA DE SELECIONAR LA MISMA LINEA
                        DataGridViewAsientos.CurrentCell = DataGridViewAsientos.Rows(Seleccion).Cells(1)

                    Catch ex As Exception

                    End Try

                End If
            Else
                MsgBox("No es Candidato !! , el Registro debe de estar ""No Enviado"" y debe de ser Cobro o Anticipo")
            End If



        Catch ex As Exception

        End Try
    End Sub

    Private Sub ContextMenuStripAx_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStripAx.Opening

    End Sub

    Private Sub DataGridViewAsientos_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridViewAsientos.CellContentClick

    End Sub

    Private Sub ButtonCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCopy.Click
        Try

            '   Dim Texto As String = ""
            '   Dim O As String = ""

            '  For I As Integer = 0 To Me.ListBoxAjustesFacturas.Items.Count - 1

            '  Me.ListBoxAjustesFacturas.SetSelected(I, True)
            '  Next I

            '          For Each O In Me.ListBoxAjustesFacturas.SelectedItems
            '               Texto += O.ToString
            'Next

            'Clipboard.SetText(O)

            'MsgBox(Texto)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub TabControlDebug_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControlDebug.SelectedIndexChanged
        Try
            If Me.TabControlDebug.SelectedIndex = 1 Then
                Me.ButtonCopy.Visible = True
            Else
                Me.ButtonCopy.Visible = False

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CheckBoxPat40_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxPat40.CheckedChanged

    End Sub
End Class