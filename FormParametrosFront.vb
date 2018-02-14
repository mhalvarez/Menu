Public Class FormParametrosFront
    Inherits System.Windows.Forms.Form
    Dim MyIni As New cIniArray
    Dim DbLee As C_DATOS.C_DatosOledb
    Dim DbLeeAux As C_DATOS.C_DatosOledb
    Dim DbNewGolf As C_DATOS.C_DatosOledb

    Dim DbWrite As New C_DATOS.C_DatosOledb

    Private mParaEmpGrupoCod As String
    Private mParaEmpCod As String
    Private mParaEmpNum As Integer

    Private mParaCfIvaLibroCod As String

    Private mParaUsuarioNewGolf As String


    Friend WithEvents TextBoxCodigoClientesContado As System.Windows.Forms.TextBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents CheckBoxIngresosServicioBloque As System.Windows.Forms.CheckBox
    Friend WithEvents TextBoxCentroCostoComisionesVisa As System.Windows.Forms.TextBox
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents GroupBoxBonosSalobre As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBoxBonosAsociacion As System.Windows.Forms.GroupBox
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtba4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCtba3 As System.Windows.Forms.TextBox
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtba2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCtba1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCtba12 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCtba11 As System.Windows.Forms.TextBox
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtba10 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCtba9 As System.Windows.Forms.TextBox
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtba8 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCtba7 As System.Windows.Forms.TextBox
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtba6 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCtba5 As System.Windows.Forms.TextBox
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents TextBoxComisionBonos As System.Windows.Forms.TextBox
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxGrupoArticulosBonos As System.Windows.Forms.ComboBox
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents TextBoxGrupoArticulosBonos As System.Windows.Forms.TextBox
    Friend WithEvents TabControl2 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents TextBoxAxDestinationEndPoint As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxAxSourceEndPoint As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxAxUserName As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxAxDomainName As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxAxWebServiceUrl As System.Windows.Forms.TextBox
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TextBoxAxServicioBonos As System.Windows.Forms.TextBox
    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents TextBoxAxServicioBonosAsociacion As System.Windows.Forms.TextBox
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents ButtonAxAyudaUrl As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents TextBoxHotelOdbcNewPos As System.Windows.Forms.TextBox
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents TextBoxAxAnticipoFormaCobro As System.Windows.Forms.TextBox
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDownWebServiceTimeOut As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents TextBoxAxPalabraDePaso As System.Windows.Forms.TextBox
    Friend WithEvents Label53 As System.Windows.Forms.Label
    Friend WithEvents LabelPalabraPaso As System.Windows.Forms.Label
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents TextBoxAxOffsetAccount As System.Windows.Forms.TextBox
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents TextBoxNewContaEstablecimientoNewConta As System.Windows.Forms.TextBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents CheckBoxNewContaOrigenCuentaNewCentral As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxNewContaOrigenCuentaNewConta As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxDesglosaAlojamientoporTipoRegimen As System.Windows.Forms.CheckBox
    Friend WithEvents TextBoxCodigoServicioAlojamiento As System.Windows.Forms.TextBox
    Friend WithEvents Label60 As System.Windows.Forms.Label
    Friend WithEvents Label62 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaAlv As System.Windows.Forms.TextBox
    Friend WithEvents Label61 As System.Windows.Forms.Label
    Friend WithEvents Label73 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaXc As System.Windows.Forms.TextBox
    Friend WithEvents Label71 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaXv As System.Windows.Forms.TextBox
    Friend WithEvents Label72 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaTic As System.Windows.Forms.TextBox
    Friend WithEvents Label69 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaTiv As System.Windows.Forms.TextBox
    Friend WithEvents Label70 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaPcc As System.Windows.Forms.TextBox
    Friend WithEvents Label67 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaPcv As System.Windows.Forms.TextBox
    Friend WithEvents Label68 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaMpc As System.Windows.Forms.TextBox
    Friend WithEvents Label65 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaMpv As System.Windows.Forms.TextBox
    Friend WithEvents Label66 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaAdc As System.Windows.Forms.TextBox
    Friend WithEvents Label63 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaAdv As System.Windows.Forms.TextBox
    Friend WithEvents Label64 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaAlc As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxSerrucha As System.Windows.Forms.CheckBox
    Friend WithEvents TextBoxCtaSerieNotasCredito As System.Windows.Forms.TextBox
    Friend WithEvents Label76 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaSerieContado As System.Windows.Forms.TextBox
    Friend WithEvents Label75 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaSerieCredito As System.Windows.Forms.TextBox
    Friend WithEvents Label74 As System.Windows.Forms.Label
    Friend WithEvents Label77 As System.Windows.Forms.Label
    Friend WithEvents TextBoxEmpNum As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxEmpCod As System.Windows.Forms.TextBox
    Friend WithEvents Label78 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox56DigitoCuentaClientes As System.Windows.Forms.TextBox
    Friend WithEvents Label79 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonCancelar As System.Windows.Forms.Button
    Friend WithEvents TextBoxHotelOdbcNewPaga As System.Windows.Forms.TextBox
    Friend WithEvents Label80 As System.Windows.Forms.Label
    Friend WithEvents Label81 As System.Windows.Forms.Label
    Friend WithEvents TextBoxNewContaCodigoHotelNewCentral As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxAxAnticipoTransferenciaAgencia As System.Windows.Forms.TextBox
    Friend WithEvents Label82 As System.Windows.Forms.Label
    Friend WithEvents CheckBoxConectaNewPos As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxConectaNewGolf As System.Windows.Forms.CheckBox
    Friend WithEvents TabPage8 As System.Windows.Forms.TabPage
    Friend WithEvents Label83 As System.Windows.Forms.Label
    Friend WithEvents TextBoxArticuloAxAnulacionReserva As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxAxAjustaBase As System.Windows.Forms.TextBox
    Friend WithEvents Label86 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCftatodiariCod2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCftatodiariCod As System.Windows.Forms.TextBox
    Friend WithEvents Label88 As System.Windows.Forms.Label
    Friend WithEvents Label87 As System.Windows.Forms.Label
    Friend WithEvents CheckBoxTrataAnticiposyDevoluciones As System.Windows.Forms.CheckBox
    Friend WithEvents TextBoxAxPrefijoMotasCreditoNewGolf As System.Windows.Forms.TextBox
    Friend WithEvents Label89 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDownTope As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label90 As System.Windows.Forms.Label
    Friend WithEvents ButtonNuevo As System.Windows.Forms.Button
    Friend WithEvents TextBoxParaPaso As System.Windows.Forms.TextBox
    Friend WithEvents Label91 As System.Windows.Forms.Label
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxInterfazNomina As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxInterfazNewStock As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxInterfazNewConta As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxInterfazNewHotel As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxValidaCuentasSpyro As System.Windows.Forms.CheckBox
    Friend WithEvents Label93 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaDepositos As System.Windows.Forms.TextBox
    Friend WithEvents Label92 As System.Windows.Forms.Label
    Friend WithEvents CheckBoxUsaCuentaDepositos As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonSeccionesNewhotel As System.Windows.Forms.Button
    Friend WithEvents TextBoxSeccionDepositosNh As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCtaDepositosEfectivo As System.Windows.Forms.TextBox
    Friend WithEvents Label94 As System.Windows.Forms.Label
    Friend WithEvents Label95 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaDepositosVisa As System.Windows.Forms.TextBox
    Friend WithEvents TabPage9 As System.Windows.Forms.TabPage
    Friend WithEvents LabelPalabraPaso2 As System.Windows.Forms.Label
    Friend WithEvents TextBoxAxPalabraDePaso2 As System.Windows.Forms.TextBox
    Friend WithEvents Label85 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDownWebServiceTimeOut2 As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label96 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBoxAxWebServiceUrl2 As System.Windows.Forms.TextBox
    Friend WithEvents Label97 As System.Windows.Forms.Label
    Friend WithEvents TextBoxAxUserName2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxAxDomainName2 As System.Windows.Forms.TextBox
    Friend WithEvents Label98 As System.Windows.Forms.Label
    Friend WithEvents Label99 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaSerieAnulacion As System.Windows.Forms.TextBox
    Friend WithEvents Label84 As System.Windows.Forms.Label
    Friend WithEvents TextBoxOdbcNewCentral As System.Windows.Forms.TextBox
    Friend WithEvents Label100 As System.Windows.Forms.Label
    Friend WithEvents TextBoxAxHotelId As System.Windows.Forms.TextBox
    Friend WithEvents Label101 As System.Windows.Forms.Label
    Friend WithEvents CheckBoxNewContaTrataAnulados As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxExcluyeDebitoTpv As CheckBox
    Friend WithEvents TabPageContanet As TabPage
    Friend WithEvents ButtonPathEmpresaContanet As Button
    Friend WithEvents TextBoxRutaEmpresaContanet As TextBox
    Friend WithEvents Label29 As Label
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents TextBoxFactuTipoCod As TextBox
    Friend WithEvents Label103 As Label
    Friend WithEvents GroupBox8 As GroupBox
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents ButtonCobMovSpyro As Button
    Friend WithEvents TextBoxNewContaTipoMovBanc As TextBox
    Friend WithEvents Label102 As Label
    Friend WithEvents ButtonFactutipoSpyro As Button
    Friend WithEvents ListBoxRegistrosSpyro As ListBox
    Friend WithEvents ButtonCobMovSpyro2 As Button
    Friend WithEvents TextBoxNewContaTipoMovBanc2 As TextBox
    Friend WithEvents Label105 As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents TextBoxNewContaFPagoBanc2 As TextBox
    Friend WithEvents Label104 As Label
    Friend WithEvents ButtonNewContaBanco As Button
    Friend WithEvents TextBoxNewContaFPagoBanc As TextBox
    Friend WithEvents Label106 As Label
    Friend WithEvents TabPageVital As TabPage
    Friend WithEvents Label110 As Label
    Friend WithEvents Label109 As Label
    Friend WithEvents TextBoxVitalCajaHaber As TextBox
    Friend WithEvents TextBoxVitalCajaDebe As TextBox
    Friend WithEvents TextBoxVitalSuplementoDesayuno As TextBox
    Friend WithEvents Label107 As Label
    Friend WithEvents ButtonServiciosNewHotel As Button
    Friend WithEvents Label111 As Label
    Friend WithEvents TextBoxVitalForeEfectivo As TextBox
    Friend WithEvents ButtonFormasdeCobroNewHotel As Button
    Friend WithEvents GroupBoxVitalGeneral As GroupBox
    Friend WithEvents GroupBoxVitalOperacionesdeCaja As GroupBox
    Friend WithEvents TabPageMorasol As TabPage
    Friend WithEvents TextBoxWebServiceEmpName As TextBox
    Friend WithEvents Label108 As Label
    Friend WithEvents TextBoxTitoPrefijoProduccion As TextBox
    Friend WithEvents Label112 As Label
    Friend WithEvents TextBoxTitoDimensionHotel As TextBox
    Friend WithEvents Label113 As Label
    Friend WithEvents ButtonSeccionesNewhotel2 As Button
    Friend WithEvents TextBoxSeccionAnticiposNh As TextBox
    Friend WithEvents Label114 As Label
    Friend WithEvents TextBoxTitoIgicProducto As TextBox
    Friend WithEvents Label116 As Label
    Friend WithEvents TextBoxTitoIgicNegocio As TextBox
    Friend WithEvents Label115 As Label
    Friend WithEvents Label117 As Label
    Friend WithEvents TextBoxVitalIdentificadorDepartemento As TextBox
    Friend WithEvents TextBoxJournalBatch As TextBox
    Friend WithEvents Label118 As Label
    Friend WithEvents TextBoxJournalTemplate As TextBox
    Friend WithEvents Label119 As Label
    Friend WithEvents TextBoxMoraSufijoDepositos As TextBox
    Friend WithEvents Label120 As Label
    Friend WithEvents TextBoxMoraSufijoAnticipos As TextBox
    Friend WithEvents Label121 As Label
    Friend WithEvents GroupBox7 As GroupBox
    Friend WithEvents TextBoxMoraEquivDimenHot As TextBox
    Friend WithEvents TextBoxMoraEquivDimenDep As TextBox
    Friend WithEvents TextBoxMoraEquivDimenNat As TextBox
    Friend WithEvents Label124 As Label
    Friend WithEvents Label123 As Label
    Friend WithEvents Label122 As Label
    Friend WithEvents CheckBoxTipoComprobantes As CheckBox
    Friend WithEvents TextBoxMoraSourceType As TextBox
    Friend WithEvents Label125 As Label
    Friend WithEvents ButtonTiposdeEfectoSpyro As Button
    Friend WithEvents Label126 As Label
    Friend WithEvents TextBoxTefect_Cod As TextBox
    Friend WithEvents TextBoxNewContaBanco2 As TextBox
    Friend WithEvents TextBoxNewContaBanco As TextBox
    Friend WithEvents Label128 As Label
    Friend WithEvents Label127 As Label
    Friend WithEvents Button As Button
    Friend WithEvents Button3 As Button
    Dim SQL As String

#Region " Código generado por el Diseñador de Windows Forms "

    Public Sub New()
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()

    End Sub

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
    'Puede modificarse utilizando el Diseñador de Windows Forms. 
    'No lo modifique con el editor de código.
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TabControlOpciones As System.Windows.Forms.TabControl
    Friend WithEvents TabPageGeneral As System.Windows.Forms.TabPage
    Friend WithEvents TabPageSpyro As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextBoxIndicadorDebe As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ButtonPathDestinoFicheros As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxEmpCod As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxGrupoCod As System.Windows.Forms.ComboBox
    Friend WithEvents TextBoxIndicadorDebeFacturas As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxIndicadorHaber As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxIndicadorHaberFacturas As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCifClientesContado As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCuentaClientesContado As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxFileSpyroPath As System.Windows.Forms.TextBox
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents TextBoxCuentaAjusteRedondeo As System.Windows.Forms.TextBox
    Friend WithEvents RadioButtonFechaRegistroAc As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonFechaValorAc As System.Windows.Forms.RadioButton
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TextBoxDenominacionImpuesto As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TextBoxSerieFacturasAnuladas As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents RadioButtonFormatoAnsi As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonFormatoAsci As System.Windows.Forms.RadioButton
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaManoCorriente As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaEfectivo As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaDesembolsos As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCtaPagosaCuenta As System.Windows.Forms.TextBox
    Friend WithEvents TabPageCadenasOdbc As System.Windows.Forms.TabPage
    Friend WithEvents CheckBoxComisionAfectaImpuesto As System.Windows.Forms.CheckBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents TextBoxHotelDescripcion As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxHotelOdbc As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents TextBoxHotelSpyro As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents TextBoxHotelOdbcAlmacen As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxHotelOdbcNewConta As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxHotelOdbcNewGolf As System.Windows.Forms.TextBox
    Friend WithEvents TabPageGeneral2 As System.Windows.Forms.TabPage
    Friend WithEvents TextBoxCentroCostoComisionesAlojamiento As System.Windows.Forms.TextBox
    Friend WithEvents TabPageHotelesLopez As System.Windows.Forms.TabPage
    Friend WithEvents TabPageGrupoSatocan As System.Windows.Forms.TabPage
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents TextBoxDebug As System.Windows.Forms.TextBox
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents RadioButtonAnulacionStandard As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonAnulacionFactura As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonAnulacionNotaCredito As System.Windows.Forms.RadioButton
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormParametrosFront))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TabControlOpciones = New System.Windows.Forms.TabControl()
        Me.TabPageGeneral = New System.Windows.Forms.TabPage()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.CheckBoxInterfazNomina = New System.Windows.Forms.CheckBox()
        Me.CheckBoxInterfazNewStock = New System.Windows.Forms.CheckBox()
        Me.CheckBoxInterfazNewConta = New System.Windows.Forms.CheckBox()
        Me.CheckBoxInterfazNewHotel = New System.Windows.Forms.CheckBox()
        Me.TextBoxParaPaso = New System.Windows.Forms.TextBox()
        Me.Label91 = New System.Windows.Forms.Label()
        Me.NumericUpDownTope = New System.Windows.Forms.NumericUpDown()
        Me.Label90 = New System.Windows.Forms.Label()
        Me.TextBoxCtaDesembolsos = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.TextBoxCtaPagosaCuenta = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.TextBoxCtaEfectivo = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.TextBoxCtaManoCorriente = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.CheckBoxComisionAfectaImpuesto = New System.Windows.Forms.CheckBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.RadioButtonFormatoAsci = New System.Windows.Forms.RadioButton()
        Me.RadioButtonFormatoAnsi = New System.Windows.Forms.RadioButton()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.TextBoxCentroCostoComisionesAlojamiento = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.TextBoxSerieFacturasAnuladas = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.TextBoxDenominacionImpuesto = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.RadioButtonFechaValorAc = New System.Windows.Forms.RadioButton()
        Me.RadioButtonFechaRegistroAc = New System.Windows.Forms.RadioButton()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TextBoxCuentaAjusteRedondeo = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.ButtonPathDestinoFicheros = New System.Windows.Forms.Button()
        Me.TextBoxFileSpyroPath = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TextBoxCifClientesContado = New System.Windows.Forms.TextBox()
        Me.TextBoxCuentaClientesContado = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TextBoxIndicadorHaberFacturas = New System.Windows.Forms.TextBox()
        Me.TextBoxIndicadorDebeFacturas = New System.Windows.Forms.TextBox()
        Me.TextBoxIndicadorHaber = New System.Windows.Forms.TextBox()
        Me.TextBoxIndicadorDebe = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabPageGeneral2 = New System.Windows.Forms.TabPage()
        Me.CheckBoxExcluyeDebitoTpv = New System.Windows.Forms.CheckBox()
        Me.CheckBoxConectaNewPos = New System.Windows.Forms.CheckBox()
        Me.CheckBoxConectaNewGolf = New System.Windows.Forms.CheckBox()
        Me.TextBoxCodigoServicioAlojamiento = New System.Windows.Forms.TextBox()
        Me.Label60 = New System.Windows.Forms.Label()
        Me.CheckBoxDesglosaAlojamientoporTipoRegimen = New System.Windows.Forms.CheckBox()
        Me.CheckBoxIngresosServicioBloque = New System.Windows.Forms.CheckBox()
        Me.RadioButtonAnulacionNotaCredito = New System.Windows.Forms.RadioButton()
        Me.RadioButtonAnulacionFactura = New System.Windows.Forms.RadioButton()
        Me.RadioButtonAnulacionStandard = New System.Windows.Forms.RadioButton()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.TextBoxDebug = New System.Windows.Forms.TextBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.CheckBoxNewContaTrataAnulados = New System.Windows.Forms.CheckBox()
        Me.TextBoxNewContaCodigoHotelNewCentral = New System.Windows.Forms.TextBox()
        Me.Label81 = New System.Windows.Forms.Label()
        Me.CheckBoxNewContaOrigenCuentaNewCentral = New System.Windows.Forms.CheckBox()
        Me.CheckBoxNewContaOrigenCuentaNewConta = New System.Windows.Forms.CheckBox()
        Me.TextBoxNewContaEstablecimientoNewConta = New System.Windows.Forms.TextBox()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.TabPageSpyro = New System.Windows.Forms.TabPage()
        Me.ListBoxRegistrosSpyro = New System.Windows.Forms.ListBox()
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.CheckBoxValidaCuentasSpyro = New System.Windows.Forms.CheckBox()
        Me.Label88 = New System.Windows.Forms.Label()
        Me.TextBoxCftatodiariCod = New System.Windows.Forms.TextBox()
        Me.TextBoxCftatodiariCod2 = New System.Windows.Forms.TextBox()
        Me.Label87 = New System.Windows.Forms.Label()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.TextBoxNewContaBanco2 = New System.Windows.Forms.TextBox()
        Me.TextBoxNewContaBanco = New System.Windows.Forms.TextBox()
        Me.Label128 = New System.Windows.Forms.Label()
        Me.Label127 = New System.Windows.Forms.Label()
        Me.ButtonTiposdeEfectoSpyro = New System.Windows.Forms.Button()
        Me.Label126 = New System.Windows.Forms.Label()
        Me.TextBoxTefect_Cod = New System.Windows.Forms.TextBox()
        Me.CheckBoxTipoComprobantes = New System.Windows.Forms.CheckBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.TextBoxNewContaFPagoBanc2 = New System.Windows.Forms.TextBox()
        Me.Label104 = New System.Windows.Forms.Label()
        Me.ButtonNewContaBanco = New System.Windows.Forms.Button()
        Me.TextBoxNewContaFPagoBanc = New System.Windows.Forms.TextBox()
        Me.Label106 = New System.Windows.Forms.Label()
        Me.ButtonCobMovSpyro2 = New System.Windows.Forms.Button()
        Me.TextBoxNewContaTipoMovBanc2 = New System.Windows.Forms.TextBox()
        Me.Label105 = New System.Windows.Forms.Label()
        Me.ButtonFactutipoSpyro = New System.Windows.Forms.Button()
        Me.ButtonCobMovSpyro = New System.Windows.Forms.Button()
        Me.TextBoxNewContaTipoMovBanc = New System.Windows.Forms.TextBox()
        Me.Label103 = New System.Windows.Forms.Label()
        Me.Label102 = New System.Windows.Forms.Label()
        Me.TextBoxFactuTipoCod = New System.Windows.Forms.TextBox()
        Me.TabPageCadenasOdbc = New System.Windows.Forms.TabPage()
        Me.TextBoxOdbcNewCentral = New System.Windows.Forms.TextBox()
        Me.Label100 = New System.Windows.Forms.Label()
        Me.TextBoxHotelOdbcNewPaga = New System.Windows.Forms.TextBox()
        Me.Label80 = New System.Windows.Forms.Label()
        Me.TextBoxHotelOdbcNewPos = New System.Windows.Forms.TextBox()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.TextBoxHotelOdbcNewGolf = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.TextBoxHotelOdbcNewConta = New System.Windows.Forms.TextBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.TextBoxHotelOdbcAlmacen = New System.Windows.Forms.TextBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.TextBoxHotelSpyro = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.TextBoxHotelOdbc = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.TextBoxHotelDescripcion = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.TabPageHotelesLopez = New System.Windows.Forms.TabPage()
        Me.Label95 = New System.Windows.Forms.Label()
        Me.TextBoxCtaDepositosVisa = New System.Windows.Forms.TextBox()
        Me.Label94 = New System.Windows.Forms.Label()
        Me.TextBoxCtaDepositosEfectivo = New System.Windows.Forms.TextBox()
        Me.ButtonSeccionesNewhotel = New System.Windows.Forms.Button()
        Me.TextBoxSeccionDepositosNh = New System.Windows.Forms.TextBox()
        Me.Label93 = New System.Windows.Forms.Label()
        Me.TextBoxCtaDepositos = New System.Windows.Forms.TextBox()
        Me.Label92 = New System.Windows.Forms.Label()
        Me.CheckBoxUsaCuentaDepositos = New System.Windows.Forms.CheckBox()
        Me.CheckBoxTrataAnticiposyDevoluciones = New System.Windows.Forms.CheckBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.TextBox56DigitoCuentaClientes = New System.Windows.Forms.TextBox()
        Me.Label79 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.TextBoxCtaSerieAnulacion = New System.Windows.Forms.TextBox()
        Me.Label84 = New System.Windows.Forms.Label()
        Me.Label74 = New System.Windows.Forms.Label()
        Me.TextBoxCtaSerieNotasCredito = New System.Windows.Forms.TextBox()
        Me.TextBoxCtaSerieCredito = New System.Windows.Forms.TextBox()
        Me.Label76 = New System.Windows.Forms.Label()
        Me.Label75 = New System.Windows.Forms.Label()
        Me.TextBoxCtaSerieContado = New System.Windows.Forms.TextBox()
        Me.CheckBoxSerrucha = New System.Windows.Forms.CheckBox()
        Me.Label73 = New System.Windows.Forms.Label()
        Me.TextBoxCtaXc = New System.Windows.Forms.TextBox()
        Me.Label71 = New System.Windows.Forms.Label()
        Me.TextBoxCtaXv = New System.Windows.Forms.TextBox()
        Me.Label72 = New System.Windows.Forms.Label()
        Me.TextBoxCtaTic = New System.Windows.Forms.TextBox()
        Me.Label69 = New System.Windows.Forms.Label()
        Me.TextBoxCtaTiv = New System.Windows.Forms.TextBox()
        Me.Label70 = New System.Windows.Forms.Label()
        Me.TextBoxCtaPcc = New System.Windows.Forms.TextBox()
        Me.Label67 = New System.Windows.Forms.Label()
        Me.TextBoxCtaPcv = New System.Windows.Forms.TextBox()
        Me.Label68 = New System.Windows.Forms.Label()
        Me.TextBoxCtaMpc = New System.Windows.Forms.TextBox()
        Me.Label65 = New System.Windows.Forms.Label()
        Me.TextBoxCtaMpv = New System.Windows.Forms.TextBox()
        Me.Label66 = New System.Windows.Forms.Label()
        Me.TextBoxCtaAdc = New System.Windows.Forms.TextBox()
        Me.Label63 = New System.Windows.Forms.Label()
        Me.TextBoxCtaAdv = New System.Windows.Forms.TextBox()
        Me.Label64 = New System.Windows.Forms.Label()
        Me.TextBoxCtaAlc = New System.Windows.Forms.TextBox()
        Me.Label62 = New System.Windows.Forms.Label()
        Me.TextBoxCtaAlv = New System.Windows.Forms.TextBox()
        Me.Label61 = New System.Windows.Forms.Label()
        Me.TabPageGrupoSatocan = New System.Windows.Forms.TabPage()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label59 = New System.Windows.Forms.Label()
        Me.Label58 = New System.Windows.Forms.Label()
        Me.Label57 = New System.Windows.Forms.Label()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.TabControl2 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TextBoxAxHotelId = New System.Windows.Forms.TextBox()
        Me.Label101 = New System.Windows.Forms.Label()
        Me.TextBoxAxDestinationEndPoint = New System.Windows.Forms.TextBox()
        Me.TextBoxAxSourceEndPoint = New System.Windows.Forms.TextBox()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.LabelPalabraPaso = New System.Windows.Forms.Label()
        Me.TextBoxAxPalabraDePaso = New System.Windows.Forms.TextBox()
        Me.Label53 = New System.Windows.Forms.Label()
        Me.NumericUpDownWebServiceTimeOut = New System.Windows.Forms.NumericUpDown()
        Me.Label52 = New System.Windows.Forms.Label()
        Me.ButtonAxAyudaUrl = New System.Windows.Forms.Button()
        Me.TextBoxAxWebServiceUrl = New System.Windows.Forms.TextBox()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.TextBoxAxUserName = New System.Windows.Forms.TextBox()
        Me.TextBoxAxDomainName = New System.Windows.Forms.TextBox()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.TabPage9 = New System.Windows.Forms.TabPage()
        Me.LabelPalabraPaso2 = New System.Windows.Forms.Label()
        Me.TextBoxAxPalabraDePaso2 = New System.Windows.Forms.TextBox()
        Me.Label85 = New System.Windows.Forms.Label()
        Me.NumericUpDownWebServiceTimeOut2 = New System.Windows.Forms.NumericUpDown()
        Me.Label96 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBoxAxWebServiceUrl2 = New System.Windows.Forms.TextBox()
        Me.Label97 = New System.Windows.Forms.Label()
        Me.TextBoxAxUserName2 = New System.Windows.Forms.TextBox()
        Me.TextBoxAxDomainName2 = New System.Windows.Forms.TextBox()
        Me.Label98 = New System.Windows.Forms.Label()
        Me.Label99 = New System.Windows.Forms.Label()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.TextBoxAxServicioBonosAsociacion = New System.Windows.Forms.TextBox()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.TextBoxAxServicioBonos = New System.Windows.Forms.TextBox()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.TextBoxAxPrefijoMotasCreditoNewGolf = New System.Windows.Forms.TextBox()
        Me.Label89 = New System.Windows.Forms.Label()
        Me.TextBoxAxAnticipoTransferenciaAgencia = New System.Windows.Forms.TextBox()
        Me.Label82 = New System.Windows.Forms.Label()
        Me.TextBoxAxOffsetAccount = New System.Windows.Forms.TextBox()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.TextBoxAxAnticipoFormaCobro = New System.Windows.Forms.TextBox()
        Me.Label51 = New System.Windows.Forms.Label()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.TabPage8 = New System.Windows.Forms.TabPage()
        Me.TextBoxArticuloAxAnulacionReserva = New System.Windows.Forms.TextBox()
        Me.TextBoxAxAjustaBase = New System.Windows.Forms.TextBox()
        Me.Label86 = New System.Windows.Forms.Label()
        Me.Label83 = New System.Windows.Forms.Label()
        Me.GroupBoxBonosSalobre = New System.Windows.Forms.GroupBox()
        Me.TextBoxGrupoArticulosBonos = New System.Windows.Forms.TextBox()
        Me.ComboBoxGrupoArticulosBonos = New System.Windows.Forms.ComboBox()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.TextBoxComisionBonos = New System.Windows.Forms.TextBox()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.TextBoxCtba12 = New System.Windows.Forms.TextBox()
        Me.TextBoxCtba11 = New System.Windows.Forms.TextBox()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.TextBoxCtba10 = New System.Windows.Forms.TextBox()
        Me.TextBoxCtba9 = New System.Windows.Forms.TextBox()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.TextBoxCtba8 = New System.Windows.Forms.TextBox()
        Me.TextBoxCtba7 = New System.Windows.Forms.TextBox()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.GroupBoxBonosAsociacion = New System.Windows.Forms.GroupBox()
        Me.TextBoxCtba6 = New System.Windows.Forms.TextBox()
        Me.TextBoxCtba5 = New System.Windows.Forms.TextBox()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.TextBoxCtba4 = New System.Windows.Forms.TextBox()
        Me.TextBoxCtba3 = New System.Windows.Forms.TextBox()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.TextBoxCtba2 = New System.Windows.Forms.TextBox()
        Me.TextBoxCtba1 = New System.Windows.Forms.TextBox()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.TextBoxCentroCostoComisionesVisa = New System.Windows.Forms.TextBox()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.TextBoxCodigoClientesContado = New System.Windows.Forms.TextBox()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.TabPageContanet = New System.Windows.Forms.TabPage()
        Me.ButtonPathEmpresaContanet = New System.Windows.Forms.Button()
        Me.TextBoxRutaEmpresaContanet = New System.Windows.Forms.TextBox()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.TabPageVital = New System.Windows.Forms.TabPage()
        Me.GroupBoxVitalGeneral = New System.Windows.Forms.GroupBox()
        Me.Label117 = New System.Windows.Forms.Label()
        Me.TextBoxVitalIdentificadorDepartemento = New System.Windows.Forms.TextBox()
        Me.Label107 = New System.Windows.Forms.Label()
        Me.TextBoxVitalSuplementoDesayuno = New System.Windows.Forms.TextBox()
        Me.ButtonServiciosNewHotel = New System.Windows.Forms.Button()
        Me.GroupBoxVitalOperacionesdeCaja = New System.Windows.Forms.GroupBox()
        Me.TextBoxVitalCajaDebe = New System.Windows.Forms.TextBox()
        Me.TextBoxVitalCajaHaber = New System.Windows.Forms.TextBox()
        Me.ButtonFormasdeCobroNewHotel = New System.Windows.Forms.Button()
        Me.Label109 = New System.Windows.Forms.Label()
        Me.Label111 = New System.Windows.Forms.Label()
        Me.Label110 = New System.Windows.Forms.Label()
        Me.TextBoxVitalForeEfectivo = New System.Windows.Forms.TextBox()
        Me.TabPageMorasol = New System.Windows.Forms.TabPage()
        Me.TextBoxMoraSourceType = New System.Windows.Forms.TextBox()
        Me.Label125 = New System.Windows.Forms.Label()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.TextBoxMoraEquivDimenHot = New System.Windows.Forms.TextBox()
        Me.TextBoxMoraEquivDimenDep = New System.Windows.Forms.TextBox()
        Me.TextBoxMoraEquivDimenNat = New System.Windows.Forms.TextBox()
        Me.Label124 = New System.Windows.Forms.Label()
        Me.Label123 = New System.Windows.Forms.Label()
        Me.Label122 = New System.Windows.Forms.Label()
        Me.TextBoxMoraSufijoDepositos = New System.Windows.Forms.TextBox()
        Me.Label120 = New System.Windows.Forms.Label()
        Me.TextBoxMoraSufijoAnticipos = New System.Windows.Forms.TextBox()
        Me.Label121 = New System.Windows.Forms.Label()
        Me.TextBoxJournalBatch = New System.Windows.Forms.TextBox()
        Me.Label118 = New System.Windows.Forms.Label()
        Me.TextBoxJournalTemplate = New System.Windows.Forms.TextBox()
        Me.Label119 = New System.Windows.Forms.Label()
        Me.TextBoxTitoIgicProducto = New System.Windows.Forms.TextBox()
        Me.Label116 = New System.Windows.Forms.Label()
        Me.TextBoxTitoIgicNegocio = New System.Windows.Forms.TextBox()
        Me.Label115 = New System.Windows.Forms.Label()
        Me.ButtonSeccionesNewhotel2 = New System.Windows.Forms.Button()
        Me.TextBoxSeccionAnticiposNh = New System.Windows.Forms.TextBox()
        Me.Label114 = New System.Windows.Forms.Label()
        Me.TextBoxTitoDimensionHotel = New System.Windows.Forms.TextBox()
        Me.Label113 = New System.Windows.Forms.Label()
        Me.TextBoxTitoPrefijoProduccion = New System.Windows.Forms.TextBox()
        Me.Label112 = New System.Windows.Forms.Label()
        Me.TextBoxWebServiceEmpName = New System.Windows.Forms.TextBox()
        Me.Label108 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.ButtonAceptar = New System.Windows.Forms.Button()
        Me.ButtonCancelar = New System.Windows.Forms.Button()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.Label10 = New System.Windows.Forms.Label()
        Me.ComboBoxGrupoCod = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.ComboBoxEmpCod = New System.Windows.Forms.ComboBox()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ButtonNuevo = New System.Windows.Forms.Button()
        Me.Label77 = New System.Windows.Forms.Label()
        Me.TextBoxEmpNum = New System.Windows.Forms.TextBox()
        Me.TextBoxEmpCod = New System.Windows.Forms.TextBox()
        Me.Label78 = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Button = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.TabControlOpciones.SuspendLayout()
        Me.TabPageGeneral.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        CType(Me.NumericUpDownTope, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageGeneral2.SuspendLayout()
        Me.TabPage7.SuspendLayout()
        Me.TabPageSpyro.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.TabPageCadenasOdbc.SuspendLayout()
        Me.TabPageHotelesLopez.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.TabPageGrupoSatocan.SuspendLayout()
        Me.TabControl2.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.NumericUpDownWebServiceTimeOut, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage9.SuspendLayout()
        CType(Me.NumericUpDownWebServiceTimeOut2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.TabPage8.SuspendLayout()
        Me.GroupBoxBonosSalobre.SuspendLayout()
        Me.GroupBoxBonosAsociacion.SuspendLayout()
        Me.TabPageContanet.SuspendLayout()
        Me.TabPageVital.SuspendLayout()
        Me.GroupBoxVitalGeneral.SuspendLayout()
        Me.GroupBoxVitalOperacionesdeCaja.SuspendLayout()
        Me.TabPageMorasol.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.TabControlOpciones)
        Me.GroupBox1.Location = New System.Drawing.Point(0, 40)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(738, 521)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'TabControlOpciones
        '
        Me.TabControlOpciones.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControlOpciones.Controls.Add(Me.TabPageGeneral)
        Me.TabControlOpciones.Controls.Add(Me.TabPageGeneral2)
        Me.TabControlOpciones.Controls.Add(Me.TabPage7)
        Me.TabControlOpciones.Controls.Add(Me.TabPageSpyro)
        Me.TabControlOpciones.Controls.Add(Me.TabPageCadenasOdbc)
        Me.TabControlOpciones.Controls.Add(Me.TabPageHotelesLopez)
        Me.TabControlOpciones.Controls.Add(Me.TabPageGrupoSatocan)
        Me.TabControlOpciones.Controls.Add(Me.TabPageContanet)
        Me.TabControlOpciones.Controls.Add(Me.TabPageVital)
        Me.TabControlOpciones.Controls.Add(Me.TabPageMorasol)
        Me.TabControlOpciones.Location = New System.Drawing.Point(8, 16)
        Me.TabControlOpciones.Name = "TabControlOpciones"
        Me.TabControlOpciones.SelectedIndex = 0
        Me.TabControlOpciones.Size = New System.Drawing.Size(722, 497)
        Me.TabControlOpciones.TabIndex = 0
        '
        'TabPageGeneral
        '
        Me.TabPageGeneral.Controls.Add(Me.GroupBox5)
        Me.TabPageGeneral.Controls.Add(Me.TextBoxParaPaso)
        Me.TabPageGeneral.Controls.Add(Me.Label91)
        Me.TabPageGeneral.Controls.Add(Me.NumericUpDownTope)
        Me.TabPageGeneral.Controls.Add(Me.Label90)
        Me.TabPageGeneral.Controls.Add(Me.TextBoxCtaDesembolsos)
        Me.TabPageGeneral.Controls.Add(Me.Label20)
        Me.TabPageGeneral.Controls.Add(Me.TextBoxCtaPagosaCuenta)
        Me.TabPageGeneral.Controls.Add(Me.Label19)
        Me.TabPageGeneral.Controls.Add(Me.TextBoxCtaEfectivo)
        Me.TabPageGeneral.Controls.Add(Me.Label18)
        Me.TabPageGeneral.Controls.Add(Me.TextBoxCtaManoCorriente)
        Me.TabPageGeneral.Controls.Add(Me.Label17)
        Me.TabPageGeneral.Controls.Add(Me.CheckBoxComisionAfectaImpuesto)
        Me.TabPageGeneral.Controls.Add(Me.Label16)
        Me.TabPageGeneral.Controls.Add(Me.RadioButtonFormatoAsci)
        Me.TabPageGeneral.Controls.Add(Me.RadioButtonFormatoAnsi)
        Me.TabPageGeneral.Controls.Add(Me.Label15)
        Me.TabPageGeneral.Controls.Add(Me.TextBoxCentroCostoComisionesAlojamiento)
        Me.TabPageGeneral.Controls.Add(Me.Label14)
        Me.TabPageGeneral.Controls.Add(Me.TextBoxSerieFacturasAnuladas)
        Me.TabPageGeneral.Controls.Add(Me.Label13)
        Me.TabPageGeneral.Controls.Add(Me.TextBoxDenominacionImpuesto)
        Me.TabPageGeneral.Controls.Add(Me.Label12)
        Me.TabPageGeneral.Controls.Add(Me.RadioButtonFechaValorAc)
        Me.TabPageGeneral.Controls.Add(Me.RadioButtonFechaRegistroAc)
        Me.TabPageGeneral.Controls.Add(Me.Label9)
        Me.TabPageGeneral.Controls.Add(Me.TextBoxCuentaAjusteRedondeo)
        Me.TabPageGeneral.Controls.Add(Me.Label8)
        Me.TabPageGeneral.Controls.Add(Me.ButtonPathDestinoFicheros)
        Me.TabPageGeneral.Controls.Add(Me.TextBoxFileSpyroPath)
        Me.TabPageGeneral.Controls.Add(Me.Label7)
        Me.TabPageGeneral.Controls.Add(Me.TextBoxCifClientesContado)
        Me.TabPageGeneral.Controls.Add(Me.TextBoxCuentaClientesContado)
        Me.TabPageGeneral.Controls.Add(Me.Label6)
        Me.TabPageGeneral.Controls.Add(Me.Label5)
        Me.TabPageGeneral.Controls.Add(Me.TextBoxIndicadorHaberFacturas)
        Me.TabPageGeneral.Controls.Add(Me.TextBoxIndicadorDebeFacturas)
        Me.TabPageGeneral.Controls.Add(Me.TextBoxIndicadorHaber)
        Me.TabPageGeneral.Controls.Add(Me.TextBoxIndicadorDebe)
        Me.TabPageGeneral.Controls.Add(Me.Label4)
        Me.TabPageGeneral.Controls.Add(Me.Label3)
        Me.TabPageGeneral.Controls.Add(Me.Label2)
        Me.TabPageGeneral.Controls.Add(Me.Label1)
        Me.TabPageGeneral.Location = New System.Drawing.Point(4, 22)
        Me.TabPageGeneral.Name = "TabPageGeneral"
        Me.TabPageGeneral.Size = New System.Drawing.Size(714, 471)
        Me.TabPageGeneral.TabIndex = 0
        Me.TabPageGeneral.Text = "Generales"
        Me.TabPageGeneral.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.CheckBoxInterfazNomina)
        Me.GroupBox5.Controls.Add(Me.CheckBoxInterfazNewStock)
        Me.GroupBox5.Controls.Add(Me.CheckBoxInterfazNewConta)
        Me.GroupBox5.Controls.Add(Me.CheckBoxInterfazNewHotel)
        Me.GroupBox5.Location = New System.Drawing.Point(320, 232)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(367, 193)
        Me.GroupBox5.TabIndex = 43
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Interfaces "
        '
        'CheckBoxInterfazNomina
        '
        Me.CheckBoxInterfazNomina.AutoSize = True
        Me.CheckBoxInterfazNomina.Location = New System.Drawing.Point(11, 87)
        Me.CheckBoxInterfazNomina.Name = "CheckBoxInterfazNomina"
        Me.CheckBoxInterfazNomina.Size = New System.Drawing.Size(67, 17)
        Me.CheckBoxInterfazNomina.TabIndex = 3
        Me.CheckBoxInterfazNomina.Text = "Nóminas"
        Me.CheckBoxInterfazNomina.UseVisualStyleBackColor = True
        '
        'CheckBoxInterfazNewStock
        '
        Me.CheckBoxInterfazNewStock.AutoSize = True
        Me.CheckBoxInterfazNewStock.Location = New System.Drawing.Point(11, 64)
        Me.CheckBoxInterfazNewStock.Name = "CheckBoxInterfazNewStock"
        Me.CheckBoxInterfazNewStock.Size = New System.Drawing.Size(120, 17)
        Me.CheckBoxInterfazNewStock.TabIndex = 2
        Me.CheckBoxInterfazNewStock.Text = "NewStock Almacen"
        Me.CheckBoxInterfazNewStock.UseVisualStyleBackColor = True
        '
        'CheckBoxInterfazNewConta
        '
        Me.CheckBoxInterfazNewConta.AutoSize = True
        Me.CheckBoxInterfazNewConta.Location = New System.Drawing.Point(11, 41)
        Me.CheckBoxInterfazNewConta.Name = "CheckBoxInterfazNewConta"
        Me.CheckBoxInterfazNewConta.Size = New System.Drawing.Size(112, 17)
        Me.CheckBoxInterfazNewConta.TabIndex = 1
        Me.CheckBoxInterfazNewConta.Text = "NewConta Cobros"
        Me.CheckBoxInterfazNewConta.UseVisualStyleBackColor = True
        '
        'CheckBoxInterfazNewHotel
        '
        Me.CheckBoxInterfazNewHotel.AutoSize = True
        Me.CheckBoxInterfazNewHotel.Location = New System.Drawing.Point(11, 18)
        Me.CheckBoxInterfazNewHotel.Name = "CheckBoxInterfazNewHotel"
        Me.CheckBoxInterfazNewHotel.Size = New System.Drawing.Size(109, 17)
        Me.CheckBoxInterfazNewHotel.TabIndex = 0
        Me.CheckBoxInterfazNewHotel.Text = "NewHotel Ventas"
        Me.CheckBoxInterfazNewHotel.UseVisualStyleBackColor = True
        '
        'TextBoxParaPaso
        '
        Me.TextBoxParaPaso.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxParaPaso.Location = New System.Drawing.Point(192, 405)
        Me.TextBoxParaPaso.Name = "TextBoxParaPaso"
        Me.TextBoxParaPaso.PasswordChar = Global.Microsoft.VisualBasic.ChrW(35)
        Me.TextBoxParaPaso.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxParaPaso.TabIndex = 42
        '
        'Label91
        '
        Me.Label91.Location = New System.Drawing.Point(8, 407)
        Me.Label91.Name = "Label91"
        Me.Label91.Size = New System.Drawing.Size(128, 23)
        Me.Label91.TabIndex = 41
        Me.Label91.Text = "Palabra de Paso"
        '
        'NumericUpDownTope
        '
        Me.NumericUpDownTope.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NumericUpDownTope.DecimalPlaces = 2
        Me.NumericUpDownTope.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.NumericUpDownTope.Location = New System.Drawing.Point(397, 188)
        Me.NumericUpDownTope.Maximum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.NumericUpDownTope.Name = "NumericUpDownTope"
        Me.NumericUpDownTope.Size = New System.Drawing.Size(66, 20)
        Me.NumericUpDownTope.TabIndex = 40
        '
        'Label90
        '
        Me.Label90.AutoSize = True
        Me.Label90.Location = New System.Drawing.Point(301, 191)
        Me.Label90.Name = "Label90"
        Me.Label90.Size = New System.Drawing.Size(71, 13)
        Me.Label90.TabIndex = 39
        Me.Label90.Text = "Tope Máximo"
        '
        'TextBoxCtaDesembolsos
        '
        Me.TextBoxCtaDesembolsos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaDesembolsos.Location = New System.Drawing.Point(128, 32)
        Me.TextBoxCtaDesembolsos.Name = "TextBoxCtaDesembolsos"
        Me.TextBoxCtaDesembolsos.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaDesembolsos.TabIndex = 38
        '
        'Label20
        '
        Me.Label20.Location = New System.Drawing.Point(8, 32)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(120, 23)
        Me.Label20.TabIndex = 37
        Me.Label20.Text = "Cta. Desembolsos"
        '
        'TextBoxCtaPagosaCuenta
        '
        Me.TextBoxCtaPagosaCuenta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaPagosaCuenta.Location = New System.Drawing.Point(520, 8)
        Me.TextBoxCtaPagosaCuenta.Name = "TextBoxCtaPagosaCuenta"
        Me.TextBoxCtaPagosaCuenta.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaPagosaCuenta.TabIndex = 36
        '
        'Label19
        '
        Me.Label19.Location = New System.Drawing.Point(408, 8)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(112, 23)
        Me.Label19.TabIndex = 35
        Me.Label19.Text = "Cta. Pagos a Cuenta"
        '
        'TextBoxCtaEfectivo
        '
        Me.TextBoxCtaEfectivo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaEfectivo.Enabled = False
        Me.TextBoxCtaEfectivo.Location = New System.Drawing.Point(304, 8)
        Me.TextBoxCtaEfectivo.Name = "TextBoxCtaEfectivo"
        Me.TextBoxCtaEfectivo.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaEfectivo.TabIndex = 34
        '
        'Label18
        '
        Me.Label18.Enabled = False
        Me.Label18.Location = New System.Drawing.Point(232, 8)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(72, 23)
        Me.Label18.TabIndex = 33
        Me.Label18.Text = "Cta. Efectivo"
        '
        'TextBoxCtaManoCorriente
        '
        Me.TextBoxCtaManoCorriente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaManoCorriente.Location = New System.Drawing.Point(128, 8)
        Me.TextBoxCtaManoCorriente.Name = "TextBoxCtaManoCorriente"
        Me.TextBoxCtaManoCorriente.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaManoCorriente.TabIndex = 32
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(8, 8)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(120, 23)
        Me.Label17.TabIndex = 31
        Me.Label17.Text = "Cta. Mano Corriente"
        '
        'CheckBoxComisionAfectaImpuesto
        '
        Me.CheckBoxComisionAfectaImpuesto.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBoxComisionAfectaImpuesto.Location = New System.Drawing.Point(192, 304)
        Me.CheckBoxComisionAfectaImpuesto.Name = "CheckBoxComisionAfectaImpuesto"
        Me.CheckBoxComisionAfectaImpuesto.Size = New System.Drawing.Size(104, 24)
        Me.CheckBoxComisionAfectaImpuesto.TabIndex = 30
        Me.CheckBoxComisionAfectaImpuesto.UseVisualStyleBackColor = False
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(8, 304)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(192, 23)
        Me.Label16.TabIndex = 29
        Me.Label16.Text = "Comisiones Afectan Base Imponible"
        '
        'RadioButtonFormatoAsci
        '
        Me.RadioButtonFormatoAsci.Location = New System.Drawing.Point(192, 347)
        Me.RadioButtonFormatoAsci.Name = "RadioButtonFormatoAsci"
        Me.RadioButtonFormatoAsci.Size = New System.Drawing.Size(104, 24)
        Me.RadioButtonFormatoAsci.TabIndex = 28
        Me.RadioButtonFormatoAsci.Text = "Ascii(Dos)"
        '
        'RadioButtonFormatoAnsi
        '
        Me.RadioButtonFormatoAnsi.Location = New System.Drawing.Point(192, 328)
        Me.RadioButtonFormatoAnsi.Name = "RadioButtonFormatoAnsi"
        Me.RadioButtonFormatoAnsi.Size = New System.Drawing.Size(104, 24)
        Me.RadioButtonFormatoAnsi.TabIndex = 27
        Me.RadioButtonFormatoAnsi.Text = "ANSI(Windows)"
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(8, 328)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(128, 23)
        Me.Label15.TabIndex = 26
        Me.Label15.Text = "Formato Fichero"
        '
        'TextBoxCentroCostoComisionesAlojamiento
        '
        Me.TextBoxCentroCostoComisionesAlojamiento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCentroCostoComisionesAlojamiento.Location = New System.Drawing.Point(192, 280)
        Me.TextBoxCentroCostoComisionesAlojamiento.Name = "TextBoxCentroCostoComisionesAlojamiento"
        Me.TextBoxCentroCostoComisionesAlojamiento.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCentroCostoComisionesAlojamiento.TabIndex = 25
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(8, 272)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(176, 32)
        Me.Label14.TabIndex = 24
        Me.Label14.Text = "Cent. Costo Comisiones Alojamiento"
        '
        'TextBoxSerieFacturasAnuladas
        '
        Me.TextBoxSerieFacturasAnuladas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxSerieFacturasAnuladas.Location = New System.Drawing.Point(192, 256)
        Me.TextBoxSerieFacturasAnuladas.Name = "TextBoxSerieFacturasAnuladas"
        Me.TextBoxSerieFacturasAnuladas.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxSerieFacturasAnuladas.TabIndex = 23
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(8, 256)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(128, 23)
        Me.Label13.TabIndex = 22
        Me.Label13.Text = "Serie Facturas Anuladas"
        '
        'TextBoxDenominacionImpuesto
        '
        Me.TextBoxDenominacionImpuesto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDenominacionImpuesto.Location = New System.Drawing.Point(192, 232)
        Me.TextBoxDenominacionImpuesto.Name = "TextBoxDenominacionImpuesto"
        Me.TextBoxDenominacionImpuesto.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxDenominacionImpuesto.TabIndex = 21
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(8, 232)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(128, 23)
        Me.Label12.TabIndex = 20
        Me.Label12.Text = "Denominación Impuesto"
        '
        'RadioButtonFechaValorAc
        '
        Me.RadioButtonFechaValorAc.Location = New System.Drawing.Point(280, 208)
        Me.RadioButtonFechaValorAc.Name = "RadioButtonFechaValorAc"
        Me.RadioButtonFechaValorAc.Size = New System.Drawing.Size(88, 24)
        Me.RadioButtonFechaValorAc.TabIndex = 19
        Me.RadioButtonFechaValorAc.Text = "De Valor"
        '
        'RadioButtonFechaRegistroAc
        '
        Me.RadioButtonFechaRegistroAc.Location = New System.Drawing.Point(192, 208)
        Me.RadioButtonFechaRegistroAc.Name = "RadioButtonFechaRegistroAc"
        Me.RadioButtonFechaRegistroAc.Size = New System.Drawing.Size(88, 24)
        Me.RadioButtonFechaRegistroAc.TabIndex = 18
        Me.RadioButtonFechaRegistroAc.Text = "De Registro"
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(8, 208)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(176, 23)
        Me.Label9.TabIndex = 17
        Me.Label9.Text = "Fecha Asiento AC "
        '
        'TextBoxCuentaAjusteRedondeo
        '
        Me.TextBoxCuentaAjusteRedondeo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCuentaAjusteRedondeo.Location = New System.Drawing.Point(192, 184)
        Me.TextBoxCuentaAjusteRedondeo.Name = "TextBoxCuentaAjusteRedondeo"
        Me.TextBoxCuentaAjusteRedondeo.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCuentaAjusteRedondeo.TabIndex = 16
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(8, 184)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(176, 23)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Cta. Ajuste Redondeos"
        '
        'ButtonPathDestinoFicheros
        '
        Me.ButtonPathDestinoFicheros.Location = New System.Drawing.Point(592, 160)
        Me.ButtonPathDestinoFicheros.Name = "ButtonPathDestinoFicheros"
        Me.ButtonPathDestinoFicheros.Size = New System.Drawing.Size(16, 23)
        Me.ButtonPathDestinoFicheros.TabIndex = 14
        Me.ButtonPathDestinoFicheros.Text = ":::"
        '
        'TextBoxFileSpyroPath
        '
        Me.TextBoxFileSpyroPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxFileSpyroPath.Location = New System.Drawing.Point(192, 160)
        Me.TextBoxFileSpyroPath.Name = "TextBoxFileSpyroPath"
        Me.TextBoxFileSpyroPath.Size = New System.Drawing.Size(400, 20)
        Me.TextBoxFileSpyroPath.TabIndex = 13
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(8, 160)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(176, 23)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Path Destino Ficheros"
        '
        'TextBoxCifClientesContado
        '
        Me.TextBoxCifClientesContado.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCifClientesContado.Location = New System.Drawing.Point(192, 136)
        Me.TextBoxCifClientesContado.Name = "TextBoxCifClientesContado"
        Me.TextBoxCifClientesContado.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCifClientesContado.TabIndex = 11
        '
        'TextBoxCuentaClientesContado
        '
        Me.TextBoxCuentaClientesContado.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCuentaClientesContado.Location = New System.Drawing.Point(192, 112)
        Me.TextBoxCuentaClientesContado.Name = "TextBoxCuentaClientesContado"
        Me.TextBoxCuentaClientesContado.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCuentaClientesContado.TabIndex = 10
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(8, 136)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(176, 23)
        Me.Label6.TabIndex = 9
        Me.Label6.Text = "CIF Genérico Clientes Contado"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 112)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(184, 23)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Cuenta Genérica Clientes Contado"
        '
        'TextBoxIndicadorHaberFacturas
        '
        Me.TextBoxIndicadorHaberFacturas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxIndicadorHaberFacturas.Location = New System.Drawing.Point(376, 80)
        Me.TextBoxIndicadorHaberFacturas.Name = "TextBoxIndicadorHaberFacturas"
        Me.TextBoxIndicadorHaberFacturas.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxIndicadorHaberFacturas.TabIndex = 7
        '
        'TextBoxIndicadorDebeFacturas
        '
        Me.TextBoxIndicadorDebeFacturas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxIndicadorDebeFacturas.Location = New System.Drawing.Point(376, 56)
        Me.TextBoxIndicadorDebeFacturas.Name = "TextBoxIndicadorDebeFacturas"
        Me.TextBoxIndicadorDebeFacturas.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxIndicadorDebeFacturas.TabIndex = 6
        '
        'TextBoxIndicadorHaber
        '
        Me.TextBoxIndicadorHaber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxIndicadorHaber.Location = New System.Drawing.Point(128, 80)
        Me.TextBoxIndicadorHaber.Name = "TextBoxIndicadorHaber"
        Me.TextBoxIndicadorHaber.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxIndicadorHaber.TabIndex = 5
        '
        'TextBoxIndicadorDebe
        '
        Me.TextBoxIndicadorDebe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxIndicadorDebe.Location = New System.Drawing.Point(128, 56)
        Me.TextBoxIndicadorDebe.Name = "TextBoxIndicadorDebe"
        Me.TextBoxIndicadorDebe.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxIndicadorDebe.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(232, 80)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(152, 23)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Indicador de Haber Facturas"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(232, 56)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(144, 23)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Indicador de Debe Facturas"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 23)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Indicador de Haber"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 56)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 23)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Indicador de Debe"
        '
        'TabPageGeneral2
        '
        Me.TabPageGeneral2.Controls.Add(Me.CheckBoxExcluyeDebitoTpv)
        Me.TabPageGeneral2.Controls.Add(Me.CheckBoxConectaNewPos)
        Me.TabPageGeneral2.Controls.Add(Me.CheckBoxConectaNewGolf)
        Me.TabPageGeneral2.Controls.Add(Me.TextBoxCodigoServicioAlojamiento)
        Me.TabPageGeneral2.Controls.Add(Me.Label60)
        Me.TabPageGeneral2.Controls.Add(Me.CheckBoxDesglosaAlojamientoporTipoRegimen)
        Me.TabPageGeneral2.Controls.Add(Me.CheckBoxIngresosServicioBloque)
        Me.TabPageGeneral2.Controls.Add(Me.RadioButtonAnulacionNotaCredito)
        Me.TabPageGeneral2.Controls.Add(Me.RadioButtonAnulacionFactura)
        Me.TabPageGeneral2.Controls.Add(Me.RadioButtonAnulacionStandard)
        Me.TabPageGeneral2.Controls.Add(Me.Label30)
        Me.TabPageGeneral2.Controls.Add(Me.TextBoxDebug)
        Me.TabPageGeneral2.Controls.Add(Me.Label28)
        Me.TabPageGeneral2.Location = New System.Drawing.Point(4, 22)
        Me.TabPageGeneral2.Name = "TabPageGeneral2"
        Me.TabPageGeneral2.Size = New System.Drawing.Size(714, 471)
        Me.TabPageGeneral2.TabIndex = 3
        Me.TabPageGeneral2.Text = "Generales 2"
        Me.TabPageGeneral2.UseVisualStyleBackColor = True
        '
        'CheckBoxExcluyeDebitoTpv
        '
        Me.CheckBoxExcluyeDebitoTpv.AutoSize = True
        Me.CheckBoxExcluyeDebitoTpv.Location = New System.Drawing.Point(15, 282)
        Me.CheckBoxExcluyeDebitoTpv.Name = "CheckBoxExcluyeDebitoTpv"
        Me.CheckBoxExcluyeDebitoTpv.Size = New System.Drawing.Size(195, 17)
        Me.CheckBoxExcluyeDebitoTpv.TabIndex = 14
        Me.CheckBoxExcluyeDebitoTpv.Text = "NO Tratar Débito Tpv no Facturado"
        Me.CheckBoxExcluyeDebitoTpv.UseVisualStyleBackColor = True
        '
        'CheckBoxConectaNewPos
        '
        Me.CheckBoxConectaNewPos.AutoSize = True
        Me.CheckBoxConectaNewPos.Location = New System.Drawing.Point(11, 174)
        Me.CheckBoxConectaNewPos.Name = "CheckBoxConectaNewPos"
        Me.CheckBoxConectaNewPos.Size = New System.Drawing.Size(109, 17)
        Me.CheckBoxConectaNewPos.TabIndex = 13
        Me.CheckBoxConectaNewPos.Text = "Conecta NewPos"
        Me.CheckBoxConectaNewPos.UseVisualStyleBackColor = True
        '
        'CheckBoxConectaNewGolf
        '
        Me.CheckBoxConectaNewGolf.AutoSize = True
        Me.CheckBoxConectaNewGolf.Location = New System.Drawing.Point(11, 151)
        Me.CheckBoxConectaNewGolf.Name = "CheckBoxConectaNewGolf"
        Me.CheckBoxConectaNewGolf.Size = New System.Drawing.Size(110, 17)
        Me.CheckBoxConectaNewGolf.TabIndex = 12
        Me.CheckBoxConectaNewGolf.Text = "Conecta NewGolf"
        Me.CheckBoxConectaNewGolf.UseVisualStyleBackColor = True
        '
        'TextBoxCodigoServicioAlojamiento
        '
        Me.TextBoxCodigoServicioAlojamiento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCodigoServicioAlojamiento.Location = New System.Drawing.Point(616, 42)
        Me.TextBoxCodigoServicioAlojamiento.Name = "TextBoxCodigoServicioAlojamiento"
        Me.TextBoxCodigoServicioAlojamiento.Size = New System.Drawing.Size(73, 20)
        Me.TextBoxCodigoServicioAlojamiento.TabIndex = 11
        '
        'Label60
        '
        Me.Label60.AutoSize = True
        Me.Label60.Location = New System.Drawing.Point(458, 42)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(138, 13)
        Me.Label60.TabIndex = 10
        Me.Label60.Text = "Código Servicio Alojamiento"
        '
        'CheckBoxDesglosaAlojamientoporTipoRegimen
        '
        Me.CheckBoxDesglosaAlojamientoporTipoRegimen.AutoSize = True
        Me.CheckBoxDesglosaAlojamientoporTipoRegimen.Location = New System.Drawing.Point(460, 11)
        Me.CheckBoxDesglosaAlojamientoporTipoRegimen.Name = "CheckBoxDesglosaAlojamientoporTipoRegimen"
        Me.CheckBoxDesglosaAlojamientoporTipoRegimen.Size = New System.Drawing.Size(229, 17)
        Me.CheckBoxDesglosaAlojamientoporTipoRegimen.TabIndex = 9
        Me.CheckBoxDesglosaAlojamientoporTipoRegimen.Text = "Desglosa Alojamiento por Tipo de Régimen"
        Me.CheckBoxDesglosaAlojamientoporTipoRegimen.UseVisualStyleBackColor = True
        '
        'CheckBoxIngresosServicioBloque
        '
        Me.CheckBoxIngresosServicioBloque.AutoSize = True
        Me.CheckBoxIngresosServicioBloque.Location = New System.Drawing.Point(11, 66)
        Me.CheckBoxIngresosServicioBloque.Name = "CheckBoxIngresosServicioBloque"
        Me.CheckBoxIngresosServicioBloque.Size = New System.Drawing.Size(163, 17)
        Me.CheckBoxIngresosServicioBloque.TabIndex = 7
        Me.CheckBoxIngresosServicioBloque.Text = "Ingresos por Servicio/Bloque"
        Me.CheckBoxIngresosServicioBloque.UseVisualStyleBackColor = True
        '
        'RadioButtonAnulacionNotaCredito
        '
        Me.RadioButtonAnulacionNotaCredito.Location = New System.Drawing.Point(264, 32)
        Me.RadioButtonAnulacionNotaCredito.Name = "RadioButtonAnulacionNotaCredito"
        Me.RadioButtonAnulacionNotaCredito.Size = New System.Drawing.Size(104, 24)
        Me.RadioButtonAnulacionNotaCredito.TabIndex = 6
        Me.RadioButtonAnulacionNotaCredito.Text = "Nota de Crédito"
        '
        'RadioButtonAnulacionFactura
        '
        Me.RadioButtonAnulacionFactura.Location = New System.Drawing.Point(200, 32)
        Me.RadioButtonAnulacionFactura.Name = "RadioButtonAnulacionFactura"
        Me.RadioButtonAnulacionFactura.Size = New System.Drawing.Size(64, 24)
        Me.RadioButtonAnulacionFactura.TabIndex = 5
        Me.RadioButtonAnulacionFactura.Text = "Factura"
        '
        'RadioButtonAnulacionStandard
        '
        Me.RadioButtonAnulacionStandard.Location = New System.Drawing.Point(128, 32)
        Me.RadioButtonAnulacionStandard.Name = "RadioButtonAnulacionStandard"
        Me.RadioButtonAnulacionStandard.Size = New System.Drawing.Size(72, 24)
        Me.RadioButtonAnulacionStandard.TabIndex = 4
        Me.RadioButtonAnulacionStandard.Text = "Standard"
        '
        'Label30
        '
        Me.Label30.Location = New System.Drawing.Point(8, 40)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(96, 23)
        Me.Label30.TabIndex = 3
        Me.Label30.Text = "Tipo Anulación"
        '
        'TextBoxDebug
        '
        Me.TextBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDebug.Location = New System.Drawing.Point(128, 8)
        Me.TextBoxDebug.Name = "TextBoxDebug"
        Me.TextBoxDebug.Size = New System.Drawing.Size(56, 20)
        Me.TextBoxDebug.TabIndex = 1
        '
        'Label28
        '
        Me.Label28.Location = New System.Drawing.Point(8, 8)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(72, 23)
        Me.Label28.TabIndex = 0
        Me.Label28.Text = "Debug"
        '
        'TabPage7
        '
        Me.TabPage7.Controls.Add(Me.CheckBoxNewContaTrataAnulados)
        Me.TabPage7.Controls.Add(Me.TextBoxNewContaCodigoHotelNewCentral)
        Me.TabPage7.Controls.Add(Me.Label81)
        Me.TabPage7.Controls.Add(Me.CheckBoxNewContaOrigenCuentaNewCentral)
        Me.TabPage7.Controls.Add(Me.CheckBoxNewContaOrigenCuentaNewConta)
        Me.TabPage7.Controls.Add(Me.TextBoxNewContaEstablecimientoNewConta)
        Me.TabPage7.Controls.Add(Me.Label27)
        Me.TabPage7.Location = New System.Drawing.Point(4, 22)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Size = New System.Drawing.Size(714, 471)
        Me.TabPage7.TabIndex = 9
        Me.TabPage7.Text = "NewConta Cuentas por Cobrar"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'CheckBoxNewContaTrataAnulados
        '
        Me.CheckBoxNewContaTrataAnulados.AutoSize = True
        Me.CheckBoxNewContaTrataAnulados.Location = New System.Drawing.Point(11, 178)
        Me.CheckBoxNewContaTrataAnulados.Name = "CheckBoxNewContaTrataAnulados"
        Me.CheckBoxNewContaTrataAnulados.Size = New System.Drawing.Size(337, 17)
        Me.CheckBoxNewContaTrataAnulados.TabIndex = 33
        Me.CheckBoxNewContaTrataAnulados.Text = "Trata Recibos Emitidos y Anulados en mismo Día (Solo Dll Dunas)"
        Me.CheckBoxNewContaTrataAnulados.UseVisualStyleBackColor = True
        '
        'TextBoxNewContaCodigoHotelNewCentral
        '
        Me.TextBoxNewContaCodigoHotelNewCentral.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxNewContaCodigoHotelNewCentral.Location = New System.Drawing.Point(185, 34)
        Me.TextBoxNewContaCodigoHotelNewCentral.Name = "TextBoxNewContaCodigoHotelNewCentral"
        Me.TextBoxNewContaCodigoHotelNewCentral.Size = New System.Drawing.Size(112, 20)
        Me.TextBoxNewContaCodigoHotelNewCentral.TabIndex = 32
        '
        'Label81
        '
        Me.Label81.Location = New System.Drawing.Point(8, 31)
        Me.Label81.Name = "Label81"
        Me.Label81.Size = New System.Drawing.Size(171, 23)
        Me.Label81.TabIndex = 31
        Me.Label81.Text = "Código de Hotel en NewCentral"
        '
        'CheckBoxNewContaOrigenCuentaNewCentral
        '
        Me.CheckBoxNewContaOrigenCuentaNewCentral.AutoSize = True
        Me.CheckBoxNewContaOrigenCuentaNewCentral.Location = New System.Drawing.Point(11, 108)
        Me.CheckBoxNewContaOrigenCuentaNewCentral.Name = "CheckBoxNewContaOrigenCuentaNewCentral"
        Me.CheckBoxNewContaOrigenCuentaNewCentral.Size = New System.Drawing.Size(230, 17)
        Me.CheckBoxNewContaOrigenCuentaNewCentral.TabIndex = 30
        Me.CheckBoxNewContaOrigenCuentaNewCentral.Text = "Buscar Cuentas Contables  en NewCentral "
        Me.CheckBoxNewContaOrigenCuentaNewCentral.UseVisualStyleBackColor = True
        '
        'CheckBoxNewContaOrigenCuentaNewConta
        '
        Me.CheckBoxNewContaOrigenCuentaNewConta.AutoSize = True
        Me.CheckBoxNewContaOrigenCuentaNewConta.Location = New System.Drawing.Point(11, 84)
        Me.CheckBoxNewContaOrigenCuentaNewConta.Name = "CheckBoxNewContaOrigenCuentaNewConta"
        Me.CheckBoxNewContaOrigenCuentaNewConta.Size = New System.Drawing.Size(219, 17)
        Me.CheckBoxNewContaOrigenCuentaNewConta.TabIndex = 29
        Me.CheckBoxNewContaOrigenCuentaNewConta.Text = "Buscar Cuentas Contables  en NewHotel"
        Me.CheckBoxNewContaOrigenCuentaNewConta.UseVisualStyleBackColor = True
        '
        'TextBoxNewContaEstablecimientoNewConta
        '
        Me.TextBoxNewContaEstablecimientoNewConta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxNewContaEstablecimientoNewConta.Location = New System.Drawing.Point(185, 8)
        Me.TextBoxNewContaEstablecimientoNewConta.Name = "TextBoxNewContaEstablecimientoNewConta"
        Me.TextBoxNewContaEstablecimientoNewConta.Size = New System.Drawing.Size(112, 20)
        Me.TextBoxNewContaEstablecimientoNewConta.TabIndex = 28
        '
        'Label27
        '
        Me.Label27.Location = New System.Drawing.Point(8, 8)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(161, 23)
        Me.Label27.TabIndex = 27
        Me.Label27.Text = "Establecimiento NewConta"
        '
        'TabPageSpyro
        '
        Me.TabPageSpyro.Controls.Add(Me.ListBoxRegistrosSpyro)
        Me.TabPageSpyro.Controls.Add(Me.GroupBox8)
        Me.TabPageSpyro.Controls.Add(Me.GroupBox6)
        Me.TabPageSpyro.Location = New System.Drawing.Point(4, 22)
        Me.TabPageSpyro.Name = "TabPageSpyro"
        Me.TabPageSpyro.Size = New System.Drawing.Size(714, 471)
        Me.TabPageSpyro.TabIndex = 1
        Me.TabPageSpyro.Text = "Spyro(Diarios ...)"
        Me.TabPageSpyro.UseVisualStyleBackColor = True
        '
        'ListBoxRegistrosSpyro
        '
        Me.ListBoxRegistrosSpyro.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxRegistrosSpyro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListBoxRegistrosSpyro.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBoxRegistrosSpyro.FormattingEnabled = True
        Me.ListBoxRegistrosSpyro.ItemHeight = 14
        Me.ListBoxRegistrosSpyro.Location = New System.Drawing.Point(526, 18)
        Me.ListBoxRegistrosSpyro.Name = "ListBoxRegistrosSpyro"
        Me.ListBoxRegistrosSpyro.Size = New System.Drawing.Size(168, 282)
        Me.ListBoxRegistrosSpyro.TabIndex = 42
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.CheckBoxValidaCuentasSpyro)
        Me.GroupBox8.Controls.Add(Me.Label88)
        Me.GroupBox8.Controls.Add(Me.TextBoxCftatodiariCod)
        Me.GroupBox8.Controls.Add(Me.TextBoxCftatodiariCod2)
        Me.GroupBox8.Controls.Add(Me.Label87)
        Me.GroupBox8.Location = New System.Drawing.Point(15, 18)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(505, 57)
        Me.GroupBox8.TabIndex = 41
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "General"
        '
        'CheckBoxValidaCuentasSpyro
        '
        Me.CheckBoxValidaCuentasSpyro.AutoSize = True
        Me.CheckBoxValidaCuentasSpyro.Location = New System.Drawing.Point(355, 19)
        Me.CheckBoxValidaCuentasSpyro.Name = "CheckBoxValidaCuentasSpyro"
        Me.CheckBoxValidaCuentasSpyro.Size = New System.Drawing.Size(127, 17)
        Me.CheckBoxValidaCuentasSpyro.TabIndex = 4
        Me.CheckBoxValidaCuentasSpyro.Text = "Valida Cuentas Spyro"
        Me.CheckBoxValidaCuentasSpyro.UseVisualStyleBackColor = True
        '
        'Label88
        '
        Me.Label88.AutoSize = True
        Me.Label88.Location = New System.Drawing.Point(137, 21)
        Me.Label88.Name = "Label88"
        Me.Label88.Size = New System.Drawing.Size(134, 13)
        Me.Label88.TabIndex = 1
        Me.Label88.Text = "Diario de Cobros/Tesoreria"
        '
        'TextBoxCftatodiariCod
        '
        Me.TextBoxCftatodiariCod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCftatodiariCod.Location = New System.Drawing.Point(97, 19)
        Me.TextBoxCftatodiariCod.Name = "TextBoxCftatodiariCod"
        Me.TextBoxCftatodiariCod.Size = New System.Drawing.Size(34, 20)
        Me.TextBoxCftatodiariCod.TabIndex = 2
        '
        'TextBoxCftatodiariCod2
        '
        Me.TextBoxCftatodiariCod2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCftatodiariCod2.Location = New System.Drawing.Point(281, 19)
        Me.TextBoxCftatodiariCod2.Name = "TextBoxCftatodiariCod2"
        Me.TextBoxCftatodiariCod2.Size = New System.Drawing.Size(45, 20)
        Me.TextBoxCftatodiariCod2.TabIndex = 3
        '
        'Label87
        '
        Me.Label87.AutoSize = True
        Me.Label87.Location = New System.Drawing.Point(6, 19)
        Me.Label87.Name = "Label87"
        Me.Label87.Size = New System.Drawing.Size(85, 13)
        Me.Label87.TabIndex = 0
        Me.Label87.Text = "Diario de Ventas"
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.Button3)
        Me.GroupBox6.Controls.Add(Me.Button)
        Me.GroupBox6.Controls.Add(Me.TextBoxNewContaBanco2)
        Me.GroupBox6.Controls.Add(Me.TextBoxNewContaBanco)
        Me.GroupBox6.Controls.Add(Me.Label128)
        Me.GroupBox6.Controls.Add(Me.Label127)
        Me.GroupBox6.Controls.Add(Me.ButtonTiposdeEfectoSpyro)
        Me.GroupBox6.Controls.Add(Me.Label126)
        Me.GroupBox6.Controls.Add(Me.TextBoxTefect_Cod)
        Me.GroupBox6.Controls.Add(Me.CheckBoxTipoComprobantes)
        Me.GroupBox6.Controls.Add(Me.Button2)
        Me.GroupBox6.Controls.Add(Me.TextBoxNewContaFPagoBanc2)
        Me.GroupBox6.Controls.Add(Me.Label104)
        Me.GroupBox6.Controls.Add(Me.ButtonNewContaBanco)
        Me.GroupBox6.Controls.Add(Me.TextBoxNewContaFPagoBanc)
        Me.GroupBox6.Controls.Add(Me.Label106)
        Me.GroupBox6.Controls.Add(Me.ButtonCobMovSpyro2)
        Me.GroupBox6.Controls.Add(Me.TextBoxNewContaTipoMovBanc2)
        Me.GroupBox6.Controls.Add(Me.Label105)
        Me.GroupBox6.Controls.Add(Me.ButtonFactutipoSpyro)
        Me.GroupBox6.Controls.Add(Me.ButtonCobMovSpyro)
        Me.GroupBox6.Controls.Add(Me.TextBoxNewContaTipoMovBanc)
        Me.GroupBox6.Controls.Add(Me.Label103)
        Me.GroupBox6.Controls.Add(Me.Label102)
        Me.GroupBox6.Controls.Add(Me.TextBoxFactuTipoCod)
        Me.GroupBox6.Location = New System.Drawing.Point(15, 81)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(505, 387)
        Me.GroupBox6.TabIndex = 40
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Gestión de Comprobantes Bancarios"
        '
        'TextBoxNewContaBanco2
        '
        Me.TextBoxNewContaBanco2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxNewContaBanco2.Location = New System.Drawing.Point(304, 239)
        Me.TextBoxNewContaBanco2.Name = "TextBoxNewContaBanco2"
        Me.TextBoxNewContaBanco2.Size = New System.Drawing.Size(128, 20)
        Me.TextBoxNewContaBanco2.TabIndex = 57
        '
        'TextBoxNewContaBanco
        '
        Me.TextBoxNewContaBanco.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxNewContaBanco.Location = New System.Drawing.Point(304, 112)
        Me.TextBoxNewContaBanco.Name = "TextBoxNewContaBanco"
        Me.TextBoxNewContaBanco.Size = New System.Drawing.Size(128, 20)
        Me.TextBoxNewContaBanco.TabIndex = 56
        '
        'Label128
        '
        Me.Label128.AutoSize = True
        Me.Label128.Location = New System.Drawing.Point(6, 247)
        Me.Label128.Name = "Label128"
        Me.Label128.Size = New System.Drawing.Size(234, 13)
        Me.Label128.TabIndex = 55
        Me.Label128.Text = "Banco ""Not""  ?  Para Cobros Con Transferencia"
        '
        'Label127
        '
        Me.Label127.AutoSize = True
        Me.Label127.Location = New System.Drawing.Point(6, 112)
        Me.Label127.Name = "Label127"
        Me.Label127.Size = New System.Drawing.Size(199, 13)
        Me.Label127.TabIndex = 54
        Me.Label127.Text = "Banco ""Not""  ? Para Cobros Con Tarjeta"
        '
        'ButtonTiposdeEfectoSpyro
        '
        Me.ButtonTiposdeEfectoSpyro.Location = New System.Drawing.Point(438, 357)
        Me.ButtonTiposdeEfectoSpyro.Name = "ButtonTiposdeEfectoSpyro"
        Me.ButtonTiposdeEfectoSpyro.Size = New System.Drawing.Size(44, 23)
        Me.ButtonTiposdeEfectoSpyro.TabIndex = 53
        Me.ButtonTiposdeEfectoSpyro.Text = ":::"
        Me.ButtonTiposdeEfectoSpyro.UseVisualStyleBackColor = True
        '
        'Label126
        '
        Me.Label126.AutoSize = True
        Me.Label126.Location = New System.Drawing.Point(63, 362)
        Me.Label126.Name = "Label126"
        Me.Label126.Size = New System.Drawing.Size(191, 13)
        Me.Label126.TabIndex = 51
        Me.Label126.Text = "Tipo de Efecto (Cobros con Visa Hotel)"
        '
        'TextBoxTefect_Cod
        '
        Me.TextBoxTefect_Cod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxTefect_Cod.Location = New System.Drawing.Point(304, 362)
        Me.TextBoxTefect_Cod.Name = "TextBoxTefect_Cod"
        Me.TextBoxTefect_Cod.Size = New System.Drawing.Size(128, 20)
        Me.TextBoxTefect_Cod.TabIndex = 52
        '
        'CheckBoxTipoComprobantes
        '
        Me.CheckBoxTipoComprobantes.AutoSize = True
        Me.CheckBoxTipoComprobantes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBoxTipoComprobantes.Location = New System.Drawing.Point(12, 335)
        Me.CheckBoxTipoComprobantes.Name = "CheckBoxTipoComprobantes"
        Me.CheckBoxTipoComprobantes.Size = New System.Drawing.Size(363, 17)
        Me.CheckBoxTipoComprobantes.TabIndex = 50
        Me.CheckBoxTipoComprobantes.Text = "Usar Gestión de Comprobantes (Segunda Revisión) Reg VV"
        Me.CheckBoxTipoComprobantes.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(438, 201)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(44, 23)
        Me.Button2.TabIndex = 49
        Me.Button2.Text = ":::"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'TextBoxNewContaFPagoBanc2
        '
        Me.TextBoxNewContaFPagoBanc2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxNewContaFPagoBanc2.Location = New System.Drawing.Point(304, 204)
        Me.TextBoxNewContaFPagoBanc2.Name = "TextBoxNewContaFPagoBanc2"
        Me.TextBoxNewContaFPagoBanc2.Size = New System.Drawing.Size(128, 20)
        Me.TextBoxNewContaFPagoBanc2.TabIndex = 48
        '
        'Label104
        '
        Me.Label104.AutoSize = True
        Me.Label104.Location = New System.Drawing.Point(3, 198)
        Me.Label104.Name = "Label104"
        Me.Label104.Size = New System.Drawing.Size(262, 26)
        Me.Label104.TabIndex = 47
        Me.Label104.Text = "Forma de Pago (SPYRO) para Transferencia Bancaria" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " (FPAGO_COD)"
        '
        'ButtonNewContaBanco
        '
        Me.ButtonNewContaBanco.Location = New System.Drawing.Point(438, 161)
        Me.ButtonNewContaBanco.Name = "ButtonNewContaBanco"
        Me.ButtonNewContaBanco.Size = New System.Drawing.Size(44, 23)
        Me.ButtonNewContaBanco.TabIndex = 46
        Me.ButtonNewContaBanco.Text = ":::"
        Me.ButtonNewContaBanco.UseVisualStyleBackColor = True
        '
        'TextBoxNewContaFPagoBanc
        '
        Me.TextBoxNewContaFPagoBanc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxNewContaFPagoBanc.Location = New System.Drawing.Point(304, 161)
        Me.TextBoxNewContaFPagoBanc.Name = "TextBoxNewContaFPagoBanc"
        Me.TextBoxNewContaFPagoBanc.Size = New System.Drawing.Size(128, 20)
        Me.TextBoxNewContaFPagoBanc.TabIndex = 45
        '
        'Label106
        '
        Me.Label106.AutoSize = True
        Me.Label106.Location = New System.Drawing.Point(3, 155)
        Me.Label106.Name = "Label106"
        Me.Label106.Size = New System.Drawing.Size(234, 26)
        Me.Label106.TabIndex = 44
        Me.Label106.Text = "Forma de Pago (SPYRO) para Cobros Con VISA" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " (FPAGO_COD)"
        '
        'ButtonCobMovSpyro2
        '
        Me.ButtonCobMovSpyro2.Location = New System.Drawing.Point(441, 76)
        Me.ButtonCobMovSpyro2.Name = "ButtonCobMovSpyro2"
        Me.ButtonCobMovSpyro2.Size = New System.Drawing.Size(41, 23)
        Me.ButtonCobMovSpyro2.TabIndex = 43
        Me.ButtonCobMovSpyro2.Text = ":::"
        Me.ButtonCobMovSpyro2.UseVisualStyleBackColor = True
        '
        'TextBoxNewContaTipoMovBanc2
        '
        Me.TextBoxNewContaTipoMovBanc2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxNewContaTipoMovBanc2.Location = New System.Drawing.Point(304, 79)
        Me.TextBoxNewContaTipoMovBanc2.Name = "TextBoxNewContaTipoMovBanc2"
        Me.TextBoxNewContaTipoMovBanc2.Size = New System.Drawing.Size(128, 20)
        Me.TextBoxNewContaTipoMovBanc2.TabIndex = 42
        '
        'Label105
        '
        Me.Label105.AutoSize = True
        Me.Label105.Location = New System.Drawing.Point(6, 73)
        Me.Label105.Name = "Label105"
        Me.Label105.Size = New System.Drawing.Size(259, 26)
        Me.Label105.TabIndex = 41
        Me.Label105.Text = "Tipo de Movimiento (SPYRO) Transferencia Bancaria" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Comprobante  Bancario   (CFBCO" &
    "TMOV_COD)"
        '
        'ButtonFactutipoSpyro
        '
        Me.ButtonFactutipoSpyro.Location = New System.Drawing.Point(438, 295)
        Me.ButtonFactutipoSpyro.Name = "ButtonFactutipoSpyro"
        Me.ButtonFactutipoSpyro.Size = New System.Drawing.Size(44, 23)
        Me.ButtonFactutipoSpyro.TabIndex = 40
        Me.ButtonFactutipoSpyro.Text = ":::"
        Me.ButtonFactutipoSpyro.UseVisualStyleBackColor = True
        '
        'ButtonCobMovSpyro
        '
        Me.ButtonCobMovSpyro.Location = New System.Drawing.Point(441, 36)
        Me.ButtonCobMovSpyro.Name = "ButtonCobMovSpyro"
        Me.ButtonCobMovSpyro.Size = New System.Drawing.Size(41, 23)
        Me.ButtonCobMovSpyro.TabIndex = 39
        Me.ButtonCobMovSpyro.Text = ":::"
        Me.ButtonCobMovSpyro.UseVisualStyleBackColor = True
        '
        'TextBoxNewContaTipoMovBanc
        '
        Me.TextBoxNewContaTipoMovBanc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxNewContaTipoMovBanc.Location = New System.Drawing.Point(304, 36)
        Me.TextBoxNewContaTipoMovBanc.Name = "TextBoxNewContaTipoMovBanc"
        Me.TextBoxNewContaTipoMovBanc.Size = New System.Drawing.Size(128, 20)
        Me.TextBoxNewContaTipoMovBanc.TabIndex = 38
        '
        'Label103
        '
        Me.Label103.AutoSize = True
        Me.Label103.Location = New System.Drawing.Point(9, 305)
        Me.Label103.Name = "Label103"
        Me.Label103.Size = New System.Drawing.Size(196, 13)
        Me.Label103.TabIndex = 5
        Me.Label103.Text = "Serie Facturas Comprobantes Bancarios"
        '
        'Label102
        '
        Me.Label102.AutoSize = True
        Me.Label102.Location = New System.Drawing.Point(6, 30)
        Me.Label102.Name = "Label102"
        Me.Label102.Size = New System.Drawing.Size(231, 26)
        Me.Label102.TabIndex = 37
        Me.Label102.Text = "Tipo de Movimiento (SPYRO) Cobros Con VISA" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Comprobante  Bancario   (CFBCOTMOV_CO" &
    "D)"
        '
        'TextBoxFactuTipoCod
        '
        Me.TextBoxFactuTipoCod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxFactuTipoCod.Location = New System.Drawing.Point(304, 298)
        Me.TextBoxFactuTipoCod.Name = "TextBoxFactuTipoCod"
        Me.TextBoxFactuTipoCod.Size = New System.Drawing.Size(128, 20)
        Me.TextBoxFactuTipoCod.TabIndex = 6
        '
        'TabPageCadenasOdbc
        '
        Me.TabPageCadenasOdbc.Controls.Add(Me.TextBoxOdbcNewCentral)
        Me.TabPageCadenasOdbc.Controls.Add(Me.Label100)
        Me.TabPageCadenasOdbc.Controls.Add(Me.TextBoxHotelOdbcNewPaga)
        Me.TabPageCadenasOdbc.Controls.Add(Me.Label80)
        Me.TabPageCadenasOdbc.Controls.Add(Me.TextBoxHotelOdbcNewPos)
        Me.TabPageCadenasOdbc.Controls.Add(Me.Label50)
        Me.TabPageCadenasOdbc.Controls.Add(Me.TextBoxHotelOdbcNewGolf)
        Me.TabPageCadenasOdbc.Controls.Add(Me.Label26)
        Me.TabPageCadenasOdbc.Controls.Add(Me.TextBoxHotelOdbcNewConta)
        Me.TabPageCadenasOdbc.Controls.Add(Me.Label25)
        Me.TabPageCadenasOdbc.Controls.Add(Me.TextBoxHotelOdbcAlmacen)
        Me.TabPageCadenasOdbc.Controls.Add(Me.Label24)
        Me.TabPageCadenasOdbc.Controls.Add(Me.TextBoxHotelSpyro)
        Me.TabPageCadenasOdbc.Controls.Add(Me.Label23)
        Me.TabPageCadenasOdbc.Controls.Add(Me.TextBoxHotelOdbc)
        Me.TabPageCadenasOdbc.Controls.Add(Me.Label22)
        Me.TabPageCadenasOdbc.Controls.Add(Me.TextBoxHotelDescripcion)
        Me.TabPageCadenasOdbc.Controls.Add(Me.Label21)
        Me.TabPageCadenasOdbc.Location = New System.Drawing.Point(4, 22)
        Me.TabPageCadenasOdbc.Name = "TabPageCadenasOdbc"
        Me.TabPageCadenasOdbc.Size = New System.Drawing.Size(714, 471)
        Me.TabPageCadenasOdbc.TabIndex = 2
        Me.TabPageCadenasOdbc.Text = "Valores Odbc"
        Me.TabPageCadenasOdbc.UseVisualStyleBackColor = True
        '
        'TextBoxOdbcNewCentral
        '
        Me.TextBoxOdbcNewCentral.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxOdbcNewCentral.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxOdbcNewCentral.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxOdbcNewCentral.Location = New System.Drawing.Point(104, 222)
        Me.TextBoxOdbcNewCentral.Name = "TextBoxOdbcNewCentral"
        Me.TextBoxOdbcNewCentral.Size = New System.Drawing.Size(578, 20)
        Me.TextBoxOdbcNewCentral.TabIndex = 19
        '
        'Label100
        '
        Me.Label100.Location = New System.Drawing.Point(8, 224)
        Me.Label100.Name = "Label100"
        Me.Label100.Size = New System.Drawing.Size(98, 23)
        Me.Label100.TabIndex = 18
        Me.Label100.Text = "Odbc NewCentral"
        '
        'TextBoxHotelOdbcNewPaga
        '
        Me.TextBoxHotelOdbcNewPaga.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxHotelOdbcNewPaga.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxHotelOdbcNewPaga.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxHotelOdbcNewPaga.Location = New System.Drawing.Point(104, 178)
        Me.TextBoxHotelOdbcNewPaga.Name = "TextBoxHotelOdbcNewPaga"
        Me.TextBoxHotelOdbcNewPaga.Size = New System.Drawing.Size(578, 20)
        Me.TextBoxHotelOdbcNewPaga.TabIndex = 17
        '
        'Label80
        '
        Me.Label80.Location = New System.Drawing.Point(8, 175)
        Me.Label80.Name = "Label80"
        Me.Label80.Size = New System.Drawing.Size(88, 23)
        Me.Label80.TabIndex = 16
        Me.Label80.Text = "Odbc NewPaga"
        '
        'TextBoxHotelOdbcNewPos
        '
        Me.TextBoxHotelOdbcNewPos.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxHotelOdbcNewPos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxHotelOdbcNewPos.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxHotelOdbcNewPos.Location = New System.Drawing.Point(104, 152)
        Me.TextBoxHotelOdbcNewPos.Name = "TextBoxHotelOdbcNewPos"
        Me.TextBoxHotelOdbcNewPos.Size = New System.Drawing.Size(578, 20)
        Me.TextBoxHotelOdbcNewPos.TabIndex = 15
        '
        'Label50
        '
        Me.Label50.Location = New System.Drawing.Point(8, 152)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(88, 23)
        Me.Label50.TabIndex = 14
        Me.Label50.Text = "Odbc NewPos"
        '
        'TextBoxHotelOdbcNewGolf
        '
        Me.TextBoxHotelOdbcNewGolf.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxHotelOdbcNewGolf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxHotelOdbcNewGolf.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxHotelOdbcNewGolf.Location = New System.Drawing.Point(104, 128)
        Me.TextBoxHotelOdbcNewGolf.Name = "TextBoxHotelOdbcNewGolf"
        Me.TextBoxHotelOdbcNewGolf.Size = New System.Drawing.Size(578, 20)
        Me.TextBoxHotelOdbcNewGolf.TabIndex = 11
        '
        'Label26
        '
        Me.Label26.Location = New System.Drawing.Point(8, 128)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(88, 23)
        Me.Label26.TabIndex = 10
        Me.Label26.Text = "Odbc NewGolf"
        '
        'TextBoxHotelOdbcNewConta
        '
        Me.TextBoxHotelOdbcNewConta.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxHotelOdbcNewConta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxHotelOdbcNewConta.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxHotelOdbcNewConta.Location = New System.Drawing.Point(104, 104)
        Me.TextBoxHotelOdbcNewConta.Name = "TextBoxHotelOdbcNewConta"
        Me.TextBoxHotelOdbcNewConta.Size = New System.Drawing.Size(442, 20)
        Me.TextBoxHotelOdbcNewConta.TabIndex = 9
        '
        'Label25
        '
        Me.Label25.Location = New System.Drawing.Point(8, 104)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(88, 23)
        Me.Label25.TabIndex = 8
        Me.Label25.Text = "Odbc NewConta"
        '
        'TextBoxHotelOdbcAlmacen
        '
        Me.TextBoxHotelOdbcAlmacen.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxHotelOdbcAlmacen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxHotelOdbcAlmacen.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxHotelOdbcAlmacen.Location = New System.Drawing.Point(104, 80)
        Me.TextBoxHotelOdbcAlmacen.Name = "TextBoxHotelOdbcAlmacen"
        Me.TextBoxHotelOdbcAlmacen.Size = New System.Drawing.Size(578, 20)
        Me.TextBoxHotelOdbcAlmacen.TabIndex = 7
        '
        'Label24
        '
        Me.Label24.Location = New System.Drawing.Point(8, 80)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(88, 23)
        Me.Label24.TabIndex = 6
        Me.Label24.Text = "Odbc NewStock"
        '
        'TextBoxHotelSpyro
        '
        Me.TextBoxHotelSpyro.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxHotelSpyro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxHotelSpyro.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxHotelSpyro.Location = New System.Drawing.Point(104, 56)
        Me.TextBoxHotelSpyro.Name = "TextBoxHotelSpyro"
        Me.TextBoxHotelSpyro.Size = New System.Drawing.Size(578, 20)
        Me.TextBoxHotelSpyro.TabIndex = 5
        '
        'Label23
        '
        Me.Label23.Location = New System.Drawing.Point(8, 56)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(88, 23)
        Me.Label23.TabIndex = 4
        Me.Label23.Text = "Odbc Spyro"
        '
        'TextBoxHotelOdbc
        '
        Me.TextBoxHotelOdbc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxHotelOdbc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxHotelOdbc.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxHotelOdbc.Location = New System.Drawing.Point(104, 32)
        Me.TextBoxHotelOdbc.Name = "TextBoxHotelOdbc"
        Me.TextBoxHotelOdbc.Size = New System.Drawing.Size(578, 20)
        Me.TextBoxHotelOdbc.TabIndex = 3
        '
        'Label22
        '
        Me.Label22.Location = New System.Drawing.Point(8, 32)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(88, 23)
        Me.Label22.TabIndex = 2
        Me.Label22.Text = "Odbc NewHotel"
        '
        'TextBoxHotelDescripcion
        '
        Me.TextBoxHotelDescripcion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxHotelDescripcion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxHotelDescripcion.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxHotelDescripcion.Location = New System.Drawing.Point(104, 8)
        Me.TextBoxHotelDescripcion.Name = "TextBoxHotelDescripcion"
        Me.TextBoxHotelDescripcion.Size = New System.Drawing.Size(578, 20)
        Me.TextBoxHotelDescripcion.TabIndex = 1
        '
        'Label21
        '
        Me.Label21.Location = New System.Drawing.Point(8, 8)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(88, 23)
        Me.Label21.TabIndex = 0
        Me.Label21.Text = "Descripción"
        '
        'TabPageHotelesLopez
        '
        Me.TabPageHotelesLopez.Controls.Add(Me.Label95)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaDepositosVisa)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label94)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaDepositosEfectivo)
        Me.TabPageHotelesLopez.Controls.Add(Me.ButtonSeccionesNewhotel)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxSeccionDepositosNh)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label93)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaDepositos)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label92)
        Me.TabPageHotelesLopez.Controls.Add(Me.CheckBoxUsaCuentaDepositos)
        Me.TabPageHotelesLopez.Controls.Add(Me.CheckBoxTrataAnticiposyDevoluciones)
        Me.TabPageHotelesLopez.Controls.Add(Me.GroupBox4)
        Me.TabPageHotelesLopez.Controls.Add(Me.GroupBox3)
        Me.TabPageHotelesLopez.Controls.Add(Me.CheckBoxSerrucha)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label73)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaXc)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label71)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaXv)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label72)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaTic)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label69)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaTiv)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label70)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaPcc)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label67)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaPcv)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label68)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaMpc)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label65)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaMpv)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label66)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaAdc)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label63)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaAdv)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label64)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaAlc)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label62)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaAlv)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label61)
        Me.TabPageHotelesLopez.Location = New System.Drawing.Point(4, 22)
        Me.TabPageHotelesLopez.Name = "TabPageHotelesLopez"
        Me.TabPageHotelesLopez.Size = New System.Drawing.Size(714, 471)
        Me.TabPageHotelesLopez.TabIndex = 6
        Me.TabPageHotelesLopez.Text = "Hoteles Lopez"
        Me.TabPageHotelesLopez.UseVisualStyleBackColor = True
        '
        'Label95
        '
        Me.Label95.AutoSize = True
        Me.Label95.Location = New System.Drawing.Point(325, 406)
        Me.Label95.Name = "Label95"
        Me.Label95.Size = New System.Drawing.Size(83, 13)
        Me.Label95.TabIndex = 44
        Me.Label95.Text = "Visa  Depósitos "
        '
        'TextBoxCtaDepositosVisa
        '
        Me.TextBoxCtaDepositosVisa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaDepositosVisa.Location = New System.Drawing.Point(521, 402)
        Me.TextBoxCtaDepositosVisa.Name = "TextBoxCtaDepositosVisa"
        Me.TextBoxCtaDepositosVisa.Size = New System.Drawing.Size(129, 20)
        Me.TextBoxCtaDepositosVisa.TabIndex = 43
        '
        'Label94
        '
        Me.Label94.AutoSize = True
        Me.Label94.Location = New System.Drawing.Point(325, 377)
        Me.Label94.Name = "Label94"
        Me.Label94.Size = New System.Drawing.Size(102, 13)
        Me.Label94.TabIndex = 42
        Me.Label94.Text = "Efectivo  Depósitos "
        '
        'TextBoxCtaDepositosEfectivo
        '
        Me.TextBoxCtaDepositosEfectivo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaDepositosEfectivo.Location = New System.Drawing.Point(521, 370)
        Me.TextBoxCtaDepositosEfectivo.Name = "TextBoxCtaDepositosEfectivo"
        Me.TextBoxCtaDepositosEfectivo.Size = New System.Drawing.Size(129, 20)
        Me.TextBoxCtaDepositosEfectivo.TabIndex = 41
        '
        'ButtonSeccionesNewhotel
        '
        Me.ButtonSeccionesNewhotel.Location = New System.Drawing.Point(656, 338)
        Me.ButtonSeccionesNewhotel.Name = "ButtonSeccionesNewhotel"
        Me.ButtonSeccionesNewhotel.Size = New System.Drawing.Size(26, 23)
        Me.ButtonSeccionesNewhotel.TabIndex = 40
        Me.ButtonSeccionesNewhotel.Text = ":::"
        Me.ButtonSeccionesNewhotel.UseVisualStyleBackColor = True
        '
        'TextBoxSeccionDepositosNh
        '
        Me.TextBoxSeccionDepositosNh.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxSeccionDepositosNh.Location = New System.Drawing.Point(521, 341)
        Me.TextBoxSeccionDepositosNh.Name = "TextBoxSeccionDepositosNh"
        Me.TextBoxSeccionDepositosNh.Size = New System.Drawing.Size(129, 20)
        Me.TextBoxSeccionDepositosNh.TabIndex = 39
        '
        'Label93
        '
        Me.Label93.AutoSize = True
        Me.Label93.Location = New System.Drawing.Point(325, 348)
        Me.Label93.Name = "Label93"
        Me.Label93.Size = New System.Drawing.Size(146, 13)
        Me.Label93.TabIndex = 38
        Me.Label93.Text = "Sección Depósitos NewHotel"
        '
        'TextBoxCtaDepositos
        '
        Me.TextBoxCtaDepositos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaDepositos.Location = New System.Drawing.Point(521, 314)
        Me.TextBoxCtaDepositos.Name = "TextBoxCtaDepositos"
        Me.TextBoxCtaDepositos.Size = New System.Drawing.Size(129, 20)
        Me.TextBoxCtaDepositos.TabIndex = 37
        '
        'Label92
        '
        Me.Label92.AutoSize = True
        Me.Label92.Location = New System.Drawing.Point(325, 321)
        Me.Label92.Name = "Label92"
        Me.Label92.Size = New System.Drawing.Size(181, 13)
        Me.Label92.TabIndex = 36
        Me.Label92.Text = "Cuenta Depósitos Pdtes. de Facturar"
        '
        'CheckBoxUsaCuentaDepositos
        '
        Me.CheckBoxUsaCuentaDepositos.AutoSize = True
        Me.CheckBoxUsaCuentaDepositos.Location = New System.Drawing.Point(295, 293)
        Me.CheckBoxUsaCuentaDepositos.Name = "CheckBoxUsaCuentaDepositos"
        Me.CheckBoxUsaCuentaDepositos.Size = New System.Drawing.Size(281, 17)
        Me.CheckBoxUsaCuentaDepositos.TabIndex = 35
        Me.CheckBoxUsaCuentaDepositos.Text = "Usa Cuenta Independiente para Depósitos y Anticipos"
        Me.CheckBoxUsaCuentaDepositos.UseVisualStyleBackColor = True
        '
        'CheckBoxTrataAnticiposyDevoluciones
        '
        Me.CheckBoxTrataAnticiposyDevoluciones.AutoSize = True
        Me.CheckBoxTrataAnticiposyDevoluciones.Location = New System.Drawing.Point(296, 261)
        Me.CheckBoxTrataAnticiposyDevoluciones.Name = "CheckBoxTrataAnticiposyDevoluciones"
        Me.CheckBoxTrataAnticiposyDevoluciones.Size = New System.Drawing.Size(173, 17)
        Me.CheckBoxTrataAnticiposyDevoluciones.TabIndex = 34
        Me.CheckBoxTrataAnticiposyDevoluciones.Text = "Trata Anticipos y Devoluciones"
        Me.CheckBoxTrataAnticiposyDevoluciones.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.TextBox56DigitoCuentaClientes)
        Me.GroupBox4.Controls.Add(Me.Label79)
        Me.GroupBox4.Location = New System.Drawing.Point(296, 172)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(381, 76)
        Me.GroupBox4.TabIndex = 33
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Composición de Cuentas (Indicador de Centros )"
        '
        'TextBox56DigitoCuentaClientes
        '
        Me.TextBox56DigitoCuentaClientes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox56DigitoCuentaClientes.Location = New System.Drawing.Point(179, 23)
        Me.TextBox56DigitoCuentaClientes.Name = "TextBox56DigitoCuentaClientes"
        Me.TextBox56DigitoCuentaClientes.Size = New System.Drawing.Size(100, 20)
        Me.TextBox56DigitoCuentaClientes.TabIndex = 28
        '
        'Label79
        '
        Me.Label79.AutoSize = True
        Me.Label79.Location = New System.Drawing.Point(6, 30)
        Me.Label79.Name = "Label79"
        Me.Label79.Size = New System.Drawing.Size(139, 13)
        Me.Label79.TabIndex = 0
        Me.Label79.Text = "5 y 6 Dígito Cuenta Clientes"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.TextBoxCtaSerieAnulacion)
        Me.GroupBox3.Controls.Add(Me.Label84)
        Me.GroupBox3.Controls.Add(Me.Label74)
        Me.GroupBox3.Controls.Add(Me.TextBoxCtaSerieNotasCredito)
        Me.GroupBox3.Controls.Add(Me.TextBoxCtaSerieCredito)
        Me.GroupBox3.Controls.Add(Me.Label76)
        Me.GroupBox3.Controls.Add(Me.Label75)
        Me.GroupBox3.Controls.Add(Me.TextBoxCtaSerieContado)
        Me.GroupBox3.Location = New System.Drawing.Point(296, 45)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(381, 121)
        Me.GroupBox3.TabIndex = 32
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Cuentas Acumulados  Facturación por Serie (sin Quinto y Sexto Dígito)"
        '
        'TextBoxCtaSerieAnulacion
        '
        Me.TextBoxCtaSerieAnulacion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaSerieAnulacion.Location = New System.Drawing.Point(179, 96)
        Me.TextBoxCtaSerieAnulacion.Name = "TextBoxCtaSerieAnulacion"
        Me.TextBoxCtaSerieAnulacion.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaSerieAnulacion.TabIndex = 33
        '
        'Label84
        '
        Me.Label84.AutoSize = True
        Me.Label84.Location = New System.Drawing.Point(6, 95)
        Me.Label84.Name = "Label84"
        Me.Label84.Size = New System.Drawing.Size(162, 13)
        Me.Label84.TabIndex = 32
        Me.Label84.Text = "Cuenta Serie Facturas Anulación"
        '
        'Label74
        '
        Me.Label74.AutoSize = True
        Me.Label74.Location = New System.Drawing.Point(6, 24)
        Me.Label74.Name = "Label74"
        Me.Label74.Size = New System.Drawing.Size(148, 13)
        Me.Label74.TabIndex = 26
        Me.Label74.Text = "Cuenta Serie Facturas Crédito"
        '
        'TextBoxCtaSerieNotasCredito
        '
        Me.TextBoxCtaSerieNotasCredito.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaSerieNotasCredito.Location = New System.Drawing.Point(179, 71)
        Me.TextBoxCtaSerieNotasCredito.Name = "TextBoxCtaSerieNotasCredito"
        Me.TextBoxCtaSerieNotasCredito.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaSerieNotasCredito.TabIndex = 31
        '
        'TextBoxCtaSerieCredito
        '
        Me.TextBoxCtaSerieCredito.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaSerieCredito.Location = New System.Drawing.Point(179, 22)
        Me.TextBoxCtaSerieCredito.Name = "TextBoxCtaSerieCredito"
        Me.TextBoxCtaSerieCredito.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaSerieCredito.TabIndex = 27
        '
        'Label76
        '
        Me.Label76.AutoSize = True
        Me.Label76.Location = New System.Drawing.Point(6, 72)
        Me.Label76.Name = "Label76"
        Me.Label76.Size = New System.Drawing.Size(150, 13)
        Me.Label76.TabIndex = 30
        Me.Label76.Text = "Cuenta Serie Notas de Crédito"
        '
        'Label75
        '
        Me.Label75.AutoSize = True
        Me.Label75.Location = New System.Drawing.Point(6, 48)
        Me.Label75.Name = "Label75"
        Me.Label75.Size = New System.Drawing.Size(155, 13)
        Me.Label75.TabIndex = 28
        Me.Label75.Text = "Cuenta Serie Facturas Contado"
        '
        'TextBoxCtaSerieContado
        '
        Me.TextBoxCtaSerieContado.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaSerieContado.Location = New System.Drawing.Point(179, 46)
        Me.TextBoxCtaSerieContado.Name = "TextBoxCtaSerieContado"
        Me.TextBoxCtaSerieContado.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaSerieContado.TabIndex = 29
        '
        'CheckBoxSerrucha
        '
        Me.CheckBoxSerrucha.AutoSize = True
        Me.CheckBoxSerrucha.Location = New System.Drawing.Point(472, 435)
        Me.CheckBoxSerrucha.Name = "CheckBoxSerrucha"
        Me.CheckBoxSerrucha.Size = New System.Drawing.Size(205, 17)
        Me.CheckBoxSerrucha.TabIndex = 25
        Me.CheckBoxSerrucha.Text = "Tener en Cuenta 99 en SERV_PCRM"
        Me.CheckBoxSerrucha.UseVisualStyleBackColor = True
        '
        'Label73
        '
        Me.Label73.AutoSize = True
        Me.Label73.ForeColor = System.Drawing.Color.Maroon
        Me.Label73.Location = New System.Drawing.Point(12, 18)
        Me.Label73.Name = "Label73"
        Me.Label73.Size = New System.Drawing.Size(429, 13)
        Me.Label73.TabIndex = 24
        Me.Label73.Text = "Para Contabilizar Ingresos y Facturación de Alojamiento Desglosado por Tipo de Ré" &
    "gimen"
        '
        'TextBoxCtaXc
        '
        Me.TextBoxCtaXc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaXc.Enabled = False
        Me.TextBoxCtaXc.Location = New System.Drawing.Point(210, 437)
        Me.TextBoxCtaXc.Name = "TextBoxCtaXc"
        Me.TextBoxCtaXc.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaXc.TabIndex = 23
        '
        'Label71
        '
        Me.Label71.AutoSize = True
        Me.Label71.Location = New System.Drawing.Point(12, 437)
        Me.Label71.Name = "Label71"
        Me.Label71.Size = New System.Drawing.Size(174, 13)
        Me.Label71.TabIndex = 22
        Me.Label71.Text = "Cuenta Cliente Alojamiento OTROS"
        '
        'TextBoxCtaXv
        '
        Me.TextBoxCtaXv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaXv.Enabled = False
        Me.TextBoxCtaXv.Location = New System.Drawing.Point(210, 406)
        Me.TextBoxCtaXv.Name = "TextBoxCtaXv"
        Me.TextBoxCtaXv.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaXv.TabIndex = 21
        '
        'Label72
        '
        Me.Label72.AutoSize = True
        Me.Label72.Location = New System.Drawing.Point(12, 409)
        Me.Label72.Name = "Label72"
        Me.Label72.Size = New System.Drawing.Size(175, 13)
        Me.Label72.TabIndex = 20
        Me.Label72.Text = "Cuenta Ventas Alojamiento OTROS"
        '
        'TextBoxCtaTic
        '
        Me.TextBoxCtaTic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaTic.Enabled = False
        Me.TextBoxCtaTic.Location = New System.Drawing.Point(176, 357)
        Me.TextBoxCtaTic.Name = "TextBoxCtaTic"
        Me.TextBoxCtaTic.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaTic.TabIndex = 19
        '
        'Label69
        '
        Me.Label69.AutoSize = True
        Me.Label69.Location = New System.Drawing.Point(11, 357)
        Me.Label69.Name = "Label69"
        Me.Label69.Size = New System.Drawing.Size(146, 13)
        Me.Label69.TabIndex = 18
        Me.Label69.Text = "Cuenta Cliente Alojamiento TI"
        '
        'TextBoxCtaTiv
        '
        Me.TextBoxCtaTiv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaTiv.Enabled = False
        Me.TextBoxCtaTiv.Location = New System.Drawing.Point(176, 326)
        Me.TextBoxCtaTiv.Name = "TextBoxCtaTiv"
        Me.TextBoxCtaTiv.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaTiv.TabIndex = 17
        '
        'Label70
        '
        Me.Label70.AutoSize = True
        Me.Label70.Location = New System.Drawing.Point(11, 329)
        Me.Label70.Name = "Label70"
        Me.Label70.Size = New System.Drawing.Size(147, 13)
        Me.Label70.TabIndex = 16
        Me.Label70.Text = "Cuenta Ventas Alojamiento TI"
        '
        'TextBoxCtaPcc
        '
        Me.TextBoxCtaPcc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaPcc.Enabled = False
        Me.TextBoxCtaPcc.Location = New System.Drawing.Point(177, 289)
        Me.TextBoxCtaPcc.Name = "TextBoxCtaPcc"
        Me.TextBoxCtaPcc.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaPcc.TabIndex = 15
        '
        'Label67
        '
        Me.Label67.AutoSize = True
        Me.Label67.Location = New System.Drawing.Point(12, 289)
        Me.Label67.Name = "Label67"
        Me.Label67.Size = New System.Drawing.Size(150, 13)
        Me.Label67.TabIndex = 14
        Me.Label67.Text = "Cuenta Cliente Alojamiento PC"
        '
        'TextBoxCtaPcv
        '
        Me.TextBoxCtaPcv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaPcv.Enabled = False
        Me.TextBoxCtaPcv.Location = New System.Drawing.Point(177, 258)
        Me.TextBoxCtaPcv.Name = "TextBoxCtaPcv"
        Me.TextBoxCtaPcv.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaPcv.TabIndex = 13
        '
        'Label68
        '
        Me.Label68.AutoSize = True
        Me.Label68.Location = New System.Drawing.Point(12, 261)
        Me.Label68.Name = "Label68"
        Me.Label68.Size = New System.Drawing.Size(151, 13)
        Me.Label68.TabIndex = 12
        Me.Label68.Text = "Cuenta Ventas Alojamiento PC"
        '
        'TextBoxCtaMpc
        '
        Me.TextBoxCtaMpc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaMpc.Enabled = False
        Me.TextBoxCtaMpc.Location = New System.Drawing.Point(177, 216)
        Me.TextBoxCtaMpc.Name = "TextBoxCtaMpc"
        Me.TextBoxCtaMpc.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaMpc.TabIndex = 11
        '
        'Label65
        '
        Me.Label65.AutoSize = True
        Me.Label65.Location = New System.Drawing.Point(12, 216)
        Me.Label65.Name = "Label65"
        Me.Label65.Size = New System.Drawing.Size(152, 13)
        Me.Label65.TabIndex = 10
        Me.Label65.Text = "Cuenta Cliente Alojamiento MP"
        '
        'TextBoxCtaMpv
        '
        Me.TextBoxCtaMpv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaMpv.Enabled = False
        Me.TextBoxCtaMpv.Location = New System.Drawing.Point(177, 185)
        Me.TextBoxCtaMpv.Name = "TextBoxCtaMpv"
        Me.TextBoxCtaMpv.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaMpv.TabIndex = 9
        '
        'Label66
        '
        Me.Label66.AutoSize = True
        Me.Label66.Location = New System.Drawing.Point(12, 188)
        Me.Label66.Name = "Label66"
        Me.Label66.Size = New System.Drawing.Size(153, 13)
        Me.Label66.TabIndex = 8
        Me.Label66.Text = "Cuenta Ventas Alojamiento MP"
        '
        'TextBoxCtaAdc
        '
        Me.TextBoxCtaAdc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaAdc.Enabled = False
        Me.TextBoxCtaAdc.Location = New System.Drawing.Point(177, 146)
        Me.TextBoxCtaAdc.Name = "TextBoxCtaAdc"
        Me.TextBoxCtaAdc.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaAdc.TabIndex = 7
        '
        'Label63
        '
        Me.Label63.AutoSize = True
        Me.Label63.Location = New System.Drawing.Point(12, 146)
        Me.Label63.Name = "Label63"
        Me.Label63.Size = New System.Drawing.Size(151, 13)
        Me.Label63.TabIndex = 6
        Me.Label63.Text = "Cuenta Cliente Alojamiento AD"
        '
        'TextBoxCtaAdv
        '
        Me.TextBoxCtaAdv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaAdv.Enabled = False
        Me.TextBoxCtaAdv.Location = New System.Drawing.Point(177, 115)
        Me.TextBoxCtaAdv.Name = "TextBoxCtaAdv"
        Me.TextBoxCtaAdv.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaAdv.TabIndex = 5
        '
        'Label64
        '
        Me.Label64.AutoSize = True
        Me.Label64.Location = New System.Drawing.Point(12, 118)
        Me.Label64.Name = "Label64"
        Me.Label64.Size = New System.Drawing.Size(152, 13)
        Me.Label64.TabIndex = 4
        Me.Label64.Text = "Cuenta Ventas Alojamiento AD"
        '
        'TextBoxCtaAlc
        '
        Me.TextBoxCtaAlc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaAlc.Enabled = False
        Me.TextBoxCtaAlc.Location = New System.Drawing.Point(177, 76)
        Me.TextBoxCtaAlc.Name = "TextBoxCtaAlc"
        Me.TextBoxCtaAlc.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaAlc.TabIndex = 3
        '
        'Label62
        '
        Me.Label62.AutoSize = True
        Me.Label62.Location = New System.Drawing.Point(12, 76)
        Me.Label62.Name = "Label62"
        Me.Label62.Size = New System.Drawing.Size(149, 13)
        Me.Label62.TabIndex = 2
        Me.Label62.Text = "Cuenta Cliente Alojamiento AL"
        '
        'TextBoxCtaAlv
        '
        Me.TextBoxCtaAlv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaAlv.Enabled = False
        Me.TextBoxCtaAlv.Location = New System.Drawing.Point(177, 45)
        Me.TextBoxCtaAlv.Name = "TextBoxCtaAlv"
        Me.TextBoxCtaAlv.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtaAlv.TabIndex = 1
        '
        'Label61
        '
        Me.Label61.AutoSize = True
        Me.Label61.Location = New System.Drawing.Point(12, 48)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(150, 13)
        Me.Label61.TabIndex = 0
        Me.Label61.Text = "Cuenta Ventas Alojamiento AL"
        '
        'TabPageGrupoSatocan
        '
        Me.TabPageGrupoSatocan.Controls.Add(Me.TextBox5)
        Me.TabPageGrupoSatocan.Controls.Add(Me.TextBox4)
        Me.TabPageGrupoSatocan.Controls.Add(Me.TextBox3)
        Me.TabPageGrupoSatocan.Controls.Add(Me.TextBox2)
        Me.TabPageGrupoSatocan.Controls.Add(Me.Label59)
        Me.TabPageGrupoSatocan.Controls.Add(Me.Label58)
        Me.TabPageGrupoSatocan.Controls.Add(Me.Label57)
        Me.TabPageGrupoSatocan.Controls.Add(Me.Label56)
        Me.TabPageGrupoSatocan.Controls.Add(Me.TabControl2)
        Me.TabPageGrupoSatocan.Controls.Add(Me.GroupBoxBonosSalobre)
        Me.TabPageGrupoSatocan.Controls.Add(Me.GroupBoxBonosAsociacion)
        Me.TabPageGrupoSatocan.Controls.Add(Me.TextBoxCentroCostoComisionesVisa)
        Me.TabPageGrupoSatocan.Controls.Add(Me.Label33)
        Me.TabPageGrupoSatocan.Controls.Add(Me.TextBoxCodigoClientesContado)
        Me.TabPageGrupoSatocan.Controls.Add(Me.Label32)
        Me.TabPageGrupoSatocan.Controls.Add(Me.TextBox1)
        Me.TabPageGrupoSatocan.Controls.Add(Me.Label31)
        Me.TabPageGrupoSatocan.Location = New System.Drawing.Point(4, 22)
        Me.TabPageGrupoSatocan.Name = "TabPageGrupoSatocan"
        Me.TabPageGrupoSatocan.Size = New System.Drawing.Size(714, 471)
        Me.TabPageGrupoSatocan.TabIndex = 7
        Me.TabPageGrupoSatocan.Text = "Grupo Satocan"
        Me.TabPageGrupoSatocan.UseVisualStyleBackColor = True
        '
        'TextBox5
        '
        Me.TextBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox5.Enabled = False
        Me.TextBox5.Location = New System.Drawing.Point(475, 79)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(100, 20)
        Me.TextBox5.TabIndex = 52
        '
        'TextBox4
        '
        Me.TextBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox4.Enabled = False
        Me.TextBox4.Location = New System.Drawing.Point(475, 62)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(100, 20)
        Me.TextBox4.TabIndex = 51
        '
        'TextBox3
        '
        Me.TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox3.Enabled = False
        Me.TextBox3.Location = New System.Drawing.Point(475, 28)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(100, 20)
        Me.TextBox3.TabIndex = 50
        '
        'TextBox2
        '
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox2.Enabled = False
        Me.TextBox2.Location = New System.Drawing.Point(475, 10)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(100, 20)
        Me.TextBox2.TabIndex = 49
        '
        'Label59
        '
        Me.Label59.AutoSize = True
        Me.Label59.Location = New System.Drawing.Point(325, 79)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(129, 13)
        Me.Label59.TabIndex = 48
        Me.Label59.Text = "Cif Genérico Venta Bonos"
        '
        'Label58
        '
        Me.Label58.AutoSize = True
        Me.Label58.Location = New System.Drawing.Point(322, 55)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(132, 13)
        Me.Label58.TabIndex = 47
        Me.Label58.Text = "Cif Genérico Venta Normal"
        '
        'Label57
        '
        Me.Label57.AutoSize = True
        Me.Label57.Location = New System.Drawing.Point(302, 28)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(155, 13)
        Me.Label57.TabIndex = 46
        Me.Label57.Text = "Cif Genérico Producción Bonos"
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Location = New System.Drawing.Point(302, 10)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(158, 13)
        Me.Label56.TabIndex = 45
        Me.Label56.Text = "Cif Genérico Producción Normal"
        '
        'TabControl2
        '
        Me.TabControl2.Controls.Add(Me.TabPage1)
        Me.TabControl2.Controls.Add(Me.TabPage2)
        Me.TabControl2.Controls.Add(Me.TabPage9)
        Me.TabControl2.Controls.Add(Me.TabPage3)
        Me.TabControl2.Controls.Add(Me.TabPage4)
        Me.TabControl2.Controls.Add(Me.TabPage5)
        Me.TabControl2.Controls.Add(Me.TabPage6)
        Me.TabControl2.Controls.Add(Me.TabPage8)
        Me.TabControl2.Location = New System.Drawing.Point(8, 320)
        Me.TabControl2.Name = "TabControl2"
        Me.TabControl2.SelectedIndex = 0
        Me.TabControl2.Size = New System.Drawing.Size(632, 144)
        Me.TabControl2.TabIndex = 44
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.TextBoxAxHotelId)
        Me.TabPage1.Controls.Add(Me.Label101)
        Me.TabPage1.Controls.Add(Me.TextBoxAxDestinationEndPoint)
        Me.TabPage1.Controls.Add(Me.TextBoxAxSourceEndPoint)
        Me.TabPage1.Controls.Add(Me.Label43)
        Me.TabPage1.Controls.Add(Me.Label42)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(624, 118)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Axapta"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TextBoxAxHotelId
        '
        Me.TextBoxAxHotelId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxHotelId.Location = New System.Drawing.Point(232, 58)
        Me.TextBoxAxHotelId.Name = "TextBoxAxHotelId"
        Me.TextBoxAxHotelId.Size = New System.Drawing.Size(104, 20)
        Me.TextBoxAxHotelId.TabIndex = 5
        '
        'Label101
        '
        Me.Label101.AutoSize = True
        Me.Label101.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label101.ForeColor = System.Drawing.Color.Maroon
        Me.Label101.Location = New System.Drawing.Point(8, 62)
        Me.Label101.Name = "Label101"
        Me.Label101.Size = New System.Drawing.Size(52, 13)
        Me.Label101.TabIndex = 4
        Me.Label101.Text = "Hotel Id"
        '
        'TextBoxAxDestinationEndPoint
        '
        Me.TextBoxAxDestinationEndPoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxDestinationEndPoint.Location = New System.Drawing.Point(232, 32)
        Me.TextBoxAxDestinationEndPoint.Name = "TextBoxAxDestinationEndPoint"
        Me.TextBoxAxDestinationEndPoint.Size = New System.Drawing.Size(104, 20)
        Me.TextBoxAxDestinationEndPoint.TabIndex = 3
        '
        'TextBoxAxSourceEndPoint
        '
        Me.TextBoxAxSourceEndPoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxSourceEndPoint.Location = New System.Drawing.Point(232, 8)
        Me.TextBoxAxSourceEndPoint.Name = "TextBoxAxSourceEndPoint"
        Me.TextBoxAxSourceEndPoint.Size = New System.Drawing.Size(104, 20)
        Me.TextBoxAxSourceEndPoint.TabIndex = 2
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Location = New System.Drawing.Point(8, 32)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(215, 13)
        Me.Label43.TabIndex = 1
        Me.Label43.Text = "Destination  End Point (Id de Extremo Local)"
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.Location = New System.Drawing.Point(8, 8)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(164, 13)
        Me.Label42.TabIndex = 0
        Me.Label42.Text = "Source End Point (Id de Extremo)"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.LabelPalabraPaso)
        Me.TabPage2.Controls.Add(Me.TextBoxAxPalabraDePaso)
        Me.TabPage2.Controls.Add(Me.Label53)
        Me.TabPage2.Controls.Add(Me.NumericUpDownWebServiceTimeOut)
        Me.TabPage2.Controls.Add(Me.Label52)
        Me.TabPage2.Controls.Add(Me.ButtonAxAyudaUrl)
        Me.TabPage2.Controls.Add(Me.TextBoxAxWebServiceUrl)
        Me.TabPage2.Controls.Add(Me.Label47)
        Me.TabPage2.Controls.Add(Me.TextBoxAxUserName)
        Me.TabPage2.Controls.Add(Me.TextBoxAxDomainName)
        Me.TabPage2.Controls.Add(Me.Label45)
        Me.TabPage2.Controls.Add(Me.Label44)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(624, 118)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Web Services Ventas"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'LabelPalabraPaso
        '
        Me.LabelPalabraPaso.AutoSize = True
        Me.LabelPalabraPaso.Location = New System.Drawing.Point(296, 80)
        Me.LabelPalabraPaso.Name = "LabelPalabraPaso"
        Me.LabelPalabraPaso.Size = New System.Drawing.Size(45, 13)
        Me.LabelPalabraPaso.TabIndex = 12
        Me.LabelPalabraPaso.Text = "Label54"
        '
        'TextBoxAxPalabraDePaso
        '
        Me.TextBoxAxPalabraDePaso.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxPalabraDePaso.Location = New System.Drawing.Point(143, 80)
        Me.TextBoxAxPalabraDePaso.Name = "TextBoxAxPalabraDePaso"
        Me.TextBoxAxPalabraDePaso.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBoxAxPalabraDePaso.Size = New System.Drawing.Size(152, 20)
        Me.TextBoxAxPalabraDePaso.TabIndex = 11
        '
        'Label53
        '
        Me.Label53.AutoSize = True
        Me.Label53.Location = New System.Drawing.Point(8, 80)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(85, 13)
        Me.Label53.TabIndex = 10
        Me.Label53.Text = "Palabra de Paso"
        '
        'NumericUpDownWebServiceTimeOut
        '
        Me.NumericUpDownWebServiceTimeOut.Location = New System.Drawing.Point(360, 32)
        Me.NumericUpDownWebServiceTimeOut.Maximum = New Decimal(New Integer() {120, 0, 0, 0})
        Me.NumericUpDownWebServiceTimeOut.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDownWebServiceTimeOut.Name = "NumericUpDownWebServiceTimeOut"
        Me.NumericUpDownWebServiceTimeOut.Size = New System.Drawing.Size(56, 20)
        Me.NumericUpDownWebServiceTimeOut.TabIndex = 9
        Me.NumericUpDownWebServiceTimeOut.Value = New Decimal(New Integer() {23, 0, 0, 0})
        '
        'Label52
        '
        Me.Label52.AutoSize = True
        Me.Label52.Location = New System.Drawing.Point(296, 32)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(50, 13)
        Me.Label52.TabIndex = 8
        Me.Label52.Text = "Time Out"
        '
        'ButtonAxAyudaUrl
        '
        Me.ButtonAxAyudaUrl.Location = New System.Drawing.Point(584, 8)
        Me.ButtonAxAyudaUrl.Name = "ButtonAxAyudaUrl"
        Me.ButtonAxAyudaUrl.Size = New System.Drawing.Size(24, 23)
        Me.ButtonAxAyudaUrl.TabIndex = 7
        Me.ButtonAxAyudaUrl.Text = ":::"
        Me.ToolTip1.SetToolTip(Me.ButtonAxAyudaUrl, "Ejemplo = http://172.16.0.10:83/Directorio Virtual/ServicioWeb.asmx")
        Me.ButtonAxAyudaUrl.UseVisualStyleBackColor = True
        '
        'TextBoxAxWebServiceUrl
        '
        Me.TextBoxAxWebServiceUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxWebServiceUrl.Location = New System.Drawing.Point(143, 8)
        Me.TextBoxAxWebServiceUrl.Name = "TextBoxAxWebServiceUrl"
        Me.TextBoxAxWebServiceUrl.Size = New System.Drawing.Size(436, 20)
        Me.TextBoxAxWebServiceUrl.TabIndex = 6
        '
        'Label47
        '
        Me.Label47.AutoSize = True
        Me.Label47.Location = New System.Drawing.Point(9, 8)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(125, 13)
        Me.Label47.TabIndex = 5
        Me.Label47.Text = "Ubicación Web Services"
        '
        'TextBoxAxUserName
        '
        Me.TextBoxAxUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxUserName.Location = New System.Drawing.Point(143, 56)
        Me.TextBoxAxUserName.Name = "TextBoxAxUserName"
        Me.TextBoxAxUserName.Size = New System.Drawing.Size(152, 20)
        Me.TextBoxAxUserName.TabIndex = 4
        '
        'TextBoxAxDomainName
        '
        Me.TextBoxAxDomainName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxDomainName.Location = New System.Drawing.Point(143, 32)
        Me.TextBoxAxDomainName.Name = "TextBoxAxDomainName"
        Me.TextBoxAxDomainName.Size = New System.Drawing.Size(152, 20)
        Me.TextBoxAxDomainName.TabIndex = 3
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.Location = New System.Drawing.Point(8, 56)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(43, 13)
        Me.Label45.TabIndex = 2
        Me.Label45.Text = "Usuario"
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.Location = New System.Drawing.Point(8, 32)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(45, 13)
        Me.Label44.TabIndex = 1
        Me.Label44.Text = "Dominio"
        '
        'TabPage9
        '
        Me.TabPage9.Controls.Add(Me.LabelPalabraPaso2)
        Me.TabPage9.Controls.Add(Me.TextBoxAxPalabraDePaso2)
        Me.TabPage9.Controls.Add(Me.Label85)
        Me.TabPage9.Controls.Add(Me.NumericUpDownWebServiceTimeOut2)
        Me.TabPage9.Controls.Add(Me.Label96)
        Me.TabPage9.Controls.Add(Me.Button1)
        Me.TabPage9.Controls.Add(Me.TextBoxAxWebServiceUrl2)
        Me.TabPage9.Controls.Add(Me.Label97)
        Me.TabPage9.Controls.Add(Me.TextBoxAxUserName2)
        Me.TabPage9.Controls.Add(Me.TextBoxAxDomainName2)
        Me.TabPage9.Controls.Add(Me.Label98)
        Me.TabPage9.Controls.Add(Me.Label99)
        Me.TabPage9.Location = New System.Drawing.Point(4, 22)
        Me.TabPage9.Name = "TabPage9"
        Me.TabPage9.Size = New System.Drawing.Size(624, 118)
        Me.TabPage9.TabIndex = 7
        Me.TabPage9.Text = "Web Services Compras"
        Me.TabPage9.UseVisualStyleBackColor = True
        '
        'LabelPalabraPaso2
        '
        Me.LabelPalabraPaso2.AutoSize = True
        Me.LabelPalabraPaso2.Location = New System.Drawing.Point(300, 85)
        Me.LabelPalabraPaso2.Name = "LabelPalabraPaso2"
        Me.LabelPalabraPaso2.Size = New System.Drawing.Size(45, 13)
        Me.LabelPalabraPaso2.TabIndex = 24
        Me.LabelPalabraPaso2.Text = "Label54"
        '
        'TextBoxAxPalabraDePaso2
        '
        Me.TextBoxAxPalabraDePaso2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxPalabraDePaso2.Location = New System.Drawing.Point(143, 80)
        Me.TextBoxAxPalabraDePaso2.Name = "TextBoxAxPalabraDePaso2"
        Me.TextBoxAxPalabraDePaso2.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBoxAxPalabraDePaso2.Size = New System.Drawing.Size(152, 20)
        Me.TextBoxAxPalabraDePaso2.TabIndex = 23
        '
        'Label85
        '
        Me.Label85.AutoSize = True
        Me.Label85.Location = New System.Drawing.Point(8, 80)
        Me.Label85.Name = "Label85"
        Me.Label85.Size = New System.Drawing.Size(85, 13)
        Me.Label85.TabIndex = 22
        Me.Label85.Text = "Palabra de Paso"
        '
        'NumericUpDownWebServiceTimeOut2
        '
        Me.NumericUpDownWebServiceTimeOut2.Location = New System.Drawing.Point(389, 32)
        Me.NumericUpDownWebServiceTimeOut2.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.NumericUpDownWebServiceTimeOut2.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.NumericUpDownWebServiceTimeOut2.Name = "NumericUpDownWebServiceTimeOut2"
        Me.NumericUpDownWebServiceTimeOut2.Size = New System.Drawing.Size(56, 20)
        Me.NumericUpDownWebServiceTimeOut2.TabIndex = 21
        Me.NumericUpDownWebServiceTimeOut2.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'Label96
        '
        Me.Label96.AutoSize = True
        Me.Label96.Location = New System.Drawing.Point(296, 32)
        Me.Label96.Name = "Label96"
        Me.Label96.Size = New System.Drawing.Size(78, 13)
        Me.Label96.TabIndex = 20
        Me.Label96.Text = "Time Out (Seg)"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(584, 8)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(24, 23)
        Me.Button1.TabIndex = 19
        Me.Button1.Text = ":::"
        Me.ToolTip1.SetToolTip(Me.Button1, "Ejemplo = http://172.16.0.10:83/Directorio Virtual/ServicioWeb.asmx")
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBoxAxWebServiceUrl2
        '
        Me.TextBoxAxWebServiceUrl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxWebServiceUrl2.Location = New System.Drawing.Point(143, 8)
        Me.TextBoxAxWebServiceUrl2.Name = "TextBoxAxWebServiceUrl2"
        Me.TextBoxAxWebServiceUrl2.Size = New System.Drawing.Size(436, 20)
        Me.TextBoxAxWebServiceUrl2.TabIndex = 18
        '
        'Label97
        '
        Me.Label97.AutoSize = True
        Me.Label97.Location = New System.Drawing.Point(9, 8)
        Me.Label97.Name = "Label97"
        Me.Label97.Size = New System.Drawing.Size(125, 13)
        Me.Label97.TabIndex = 17
        Me.Label97.Text = "Ubicación Web Services"
        '
        'TextBoxAxUserName2
        '
        Me.TextBoxAxUserName2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxUserName2.Location = New System.Drawing.Point(143, 56)
        Me.TextBoxAxUserName2.Name = "TextBoxAxUserName2"
        Me.TextBoxAxUserName2.Size = New System.Drawing.Size(152, 20)
        Me.TextBoxAxUserName2.TabIndex = 16
        '
        'TextBoxAxDomainName2
        '
        Me.TextBoxAxDomainName2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxDomainName2.Location = New System.Drawing.Point(143, 32)
        Me.TextBoxAxDomainName2.Name = "TextBoxAxDomainName2"
        Me.TextBoxAxDomainName2.Size = New System.Drawing.Size(152, 20)
        Me.TextBoxAxDomainName2.TabIndex = 15
        '
        'Label98
        '
        Me.Label98.AutoSize = True
        Me.Label98.Location = New System.Drawing.Point(8, 56)
        Me.Label98.Name = "Label98"
        Me.Label98.Size = New System.Drawing.Size(43, 13)
        Me.Label98.TabIndex = 14
        Me.Label98.Text = "Usuario"
        '
        'Label99
        '
        Me.Label99.AutoSize = True
        Me.Label99.Location = New System.Drawing.Point(8, 32)
        Me.Label99.Name = "Label99"
        Me.Label99.Size = New System.Drawing.Size(45, 13)
        Me.Label99.TabIndex = 13
        Me.Label99.Text = "Dominio"
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.TextBoxAxServicioBonosAsociacion)
        Me.TabPage3.Controls.Add(Me.Label49)
        Me.TabPage3.Controls.Add(Me.TextBoxAxServicioBonos)
        Me.TabPage3.Controls.Add(Me.Label48)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(624, 118)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Bonos"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'TextBoxAxServicioBonosAsociacion
        '
        Me.TextBoxAxServicioBonosAsociacion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxServicioBonosAsociacion.Location = New System.Drawing.Point(376, 32)
        Me.TextBoxAxServicioBonosAsociacion.Name = "TextBoxAxServicioBonosAsociacion"
        Me.TextBoxAxServicioBonosAsociacion.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxAxServicioBonosAsociacion.TabIndex = 3
        '
        'Label49
        '
        Me.Label49.AutoEllipsis = True
        Me.Label49.AutoSize = True
        Me.Label49.Location = New System.Drawing.Point(0, 32)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(366, 13)
        Me.Label49.TabIndex = 2
        Me.Label49.Text = "Servicio de NewHotel Producción de Bonos Asociación de Campos de Golf "
        '
        'TextBoxAxServicioBonos
        '
        Me.TextBoxAxServicioBonos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxServicioBonos.Location = New System.Drawing.Point(232, 8)
        Me.TextBoxAxServicioBonos.Name = "TextBoxAxServicioBonos"
        Me.TextBoxAxServicioBonos.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxAxServicioBonos.TabIndex = 1
        '
        'Label48
        '
        Me.Label48.AutoSize = True
        Me.Label48.Location = New System.Drawing.Point(0, 8)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(215, 13)
        Me.Label48.TabIndex = 0
        Me.Label48.Text = "Servicio de NewHotel Producción de Bonos"
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.TextBoxAxPrefijoMotasCreditoNewGolf)
        Me.TabPage4.Controls.Add(Me.Label89)
        Me.TabPage4.Controls.Add(Me.TextBoxAxAnticipoTransferenciaAgencia)
        Me.TabPage4.Controls.Add(Me.Label82)
        Me.TabPage4.Controls.Add(Me.TextBoxAxOffsetAccount)
        Me.TabPage4.Controls.Add(Me.Label46)
        Me.TabPage4.Controls.Add(Me.TextBoxAxAnticipoFormaCobro)
        Me.TabPage4.Controls.Add(Me.Label51)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(624, 118)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Otros ..."
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'TextBoxAxPrefijoMotasCreditoNewGolf
        '
        Me.TextBoxAxPrefijoMotasCreditoNewGolf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxPrefijoMotasCreditoNewGolf.Location = New System.Drawing.Point(338, 86)
        Me.TextBoxAxPrefijoMotasCreditoNewGolf.Name = "TextBoxAxPrefijoMotasCreditoNewGolf"
        Me.TextBoxAxPrefijoMotasCreditoNewGolf.Size = New System.Drawing.Size(104, 20)
        Me.TextBoxAxPrefijoMotasCreditoNewGolf.TabIndex = 11
        '
        'Label89
        '
        Me.Label89.AutoSize = True
        Me.Label89.Location = New System.Drawing.Point(3, 88)
        Me.Label89.Name = "Label89"
        Me.Label89.Size = New System.Drawing.Size(207, 13)
        Me.Label89.TabIndex = 10
        Me.Label89.Text = "Prefijo Notas de Crédito Anuladas Newgolf"
        '
        'TextBoxAxAnticipoTransferenciaAgencia
        '
        Me.TextBoxAxAnticipoTransferenciaAgencia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxAnticipoTransferenciaAgencia.Location = New System.Drawing.Point(338, 54)
        Me.TextBoxAxAnticipoTransferenciaAgencia.Name = "TextBoxAxAnticipoTransferenciaAgencia"
        Me.TextBoxAxAnticipoTransferenciaAgencia.Size = New System.Drawing.Size(104, 20)
        Me.TextBoxAxAnticipoTransferenciaAgencia.TabIndex = 9
        '
        'Label82
        '
        Me.Label82.AutoSize = True
        Me.Label82.Location = New System.Drawing.Point(3, 61)
        Me.Label82.Name = "Label82"
        Me.Label82.Size = New System.Drawing.Size(318, 13)
        Me.Label82.TabIndex = 8
        Me.Label82.Text = "Código de Forma de Cobro ""Transferencia Agencia"" en NewHotel"
        '
        'TextBoxAxOffsetAccount
        '
        Me.TextBoxAxOffsetAccount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxOffsetAccount.Location = New System.Drawing.Point(338, 8)
        Me.TextBoxAxOffsetAccount.Name = "TextBoxAxOffsetAccount"
        Me.TextBoxAxOffsetAccount.Size = New System.Drawing.Size(104, 20)
        Me.TextBoxAxOffsetAccount.TabIndex = 7
        '
        'Label46
        '
        Me.Label46.AutoSize = True
        Me.Label46.Location = New System.Drawing.Point(3, 8)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(260, 13)
        Me.Label46.TabIndex = 6
        Me.Label46.Text = "OffSetAccount (Código Banco para Formas de Cobro)"
        '
        'TextBoxAxAnticipoFormaCobro
        '
        Me.TextBoxAxAnticipoFormaCobro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxAnticipoFormaCobro.Location = New System.Drawing.Point(338, 32)
        Me.TextBoxAxAnticipoFormaCobro.Name = "TextBoxAxAnticipoFormaCobro"
        Me.TextBoxAxAnticipoFormaCobro.Size = New System.Drawing.Size(104, 20)
        Me.TextBoxAxAnticipoFormaCobro.TabIndex = 3
        '
        'Label51
        '
        Me.Label51.AutoSize = True
        Me.Label51.Location = New System.Drawing.Point(3, 32)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(225, 13)
        Me.Label51.TabIndex = 1
        Me.Label51.Text = "Código de Forma de Cobro Anticipo en Axapta"
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.Label55)
        Me.TabPage5.Controls.Add(Me.Label54)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(624, 118)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "Punto de Venta NewPos"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.Location = New System.Drawing.Point(8, 40)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(127, 13)
        Me.Label55.TabIndex = 1
        Me.Label55.Text = "GRUP_CODI por defecto"
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.Location = New System.Drawing.Point(8, 16)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(127, 13)
        Me.Label54.TabIndex = 0
        Me.Label54.Text = "GRUP_CODI por defecto"
        '
        'TabPage6
        '
        Me.TabPage6.Location = New System.Drawing.Point(4, 22)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(624, 118)
        Me.TabPage6.TabIndex = 5
        Me.TabPage6.Text = "Punto de Venta NewGolf"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'TabPage8
        '
        Me.TabPage8.Controls.Add(Me.TextBoxArticuloAxAnulacionReserva)
        Me.TabPage8.Controls.Add(Me.TextBoxAxAjustaBase)
        Me.TabPage8.Controls.Add(Me.Label86)
        Me.TabPage8.Controls.Add(Me.Label83)
        Me.TabPage8.Location = New System.Drawing.Point(4, 22)
        Me.TabPage8.Name = "TabPage8"
        Me.TabPage8.Size = New System.Drawing.Size(624, 118)
        Me.TabPage8.TabIndex = 6
        Me.TabPage8.Text = "Artículos de Ajuste "
        Me.TabPage8.UseVisualStyleBackColor = True
        '
        'TextBoxArticuloAxAnulacionReserva
        '
        Me.TextBoxArticuloAxAnulacionReserva.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxArticuloAxAnulacionReserva.Location = New System.Drawing.Point(500, 18)
        Me.TextBoxArticuloAxAnulacionReserva.Name = "TextBoxArticuloAxAnulacionReserva"
        Me.TextBoxArticuloAxAnulacionReserva.Size = New System.Drawing.Size(104, 20)
        Me.TextBoxArticuloAxAnulacionReserva.TabIndex = 25
        '
        'TextBoxAxAjustaBase
        '
        Me.TextBoxAxAjustaBase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAxAjustaBase.Location = New System.Drawing.Point(122, 11)
        Me.TextBoxAxAjustaBase.Name = "TextBoxAxAjustaBase"
        Me.TextBoxAxAjustaBase.Size = New System.Drawing.Size(126, 20)
        Me.TextBoxAxAjustaBase.TabIndex = 22
        '
        'Label86
        '
        Me.Label86.AutoSize = True
        Me.Label86.Location = New System.Drawing.Point(261, 18)
        Me.Label86.Name = "Label86"
        Me.Label86.Size = New System.Drawing.Size(221, 13)
        Me.Label86.TabIndex = 3
        Me.Label86.Text = "Anulación de Reservas(Facturas sin Servicio)"
        '
        'Label83
        '
        Me.Label83.AutoSize = True
        Me.Label83.Location = New System.Drawing.Point(5, 18)
        Me.Label83.Name = "Label83"
        Me.Label83.Size = New System.Drawing.Size(111, 13)
        Me.Label83.TabIndex = 0
        Me.Label83.Text = "Ajuste Base Imponible"
        '
        'GroupBoxBonosSalobre
        '
        Me.GroupBoxBonosSalobre.Controls.Add(Me.TextBoxGrupoArticulosBonos)
        Me.GroupBoxBonosSalobre.Controls.Add(Me.ComboBoxGrupoArticulosBonos)
        Me.GroupBoxBonosSalobre.Controls.Add(Me.Label41)
        Me.GroupBoxBonosSalobre.Controls.Add(Me.TextBoxComisionBonos)
        Me.GroupBoxBonosSalobre.Controls.Add(Me.Label40)
        Me.GroupBoxBonosSalobre.Controls.Add(Me.TextBoxCtba12)
        Me.GroupBoxBonosSalobre.Controls.Add(Me.TextBoxCtba11)
        Me.GroupBoxBonosSalobre.Controls.Add(Me.Label36)
        Me.GroupBoxBonosSalobre.Controls.Add(Me.TextBoxCtba10)
        Me.GroupBoxBonosSalobre.Controls.Add(Me.TextBoxCtba9)
        Me.GroupBoxBonosSalobre.Controls.Add(Me.Label38)
        Me.GroupBoxBonosSalobre.Controls.Add(Me.TextBoxCtba8)
        Me.GroupBoxBonosSalobre.Controls.Add(Me.TextBoxCtba7)
        Me.GroupBoxBonosSalobre.Controls.Add(Me.Label39)
        Me.GroupBoxBonosSalobre.Location = New System.Drawing.Point(3, 205)
        Me.GroupBoxBonosSalobre.Name = "GroupBoxBonosSalobre"
        Me.GroupBoxBonosSalobre.Size = New System.Drawing.Size(634, 98)
        Me.GroupBoxBonosSalobre.TabIndex = 43
        Me.GroupBoxBonosSalobre.TabStop = False
        Me.GroupBoxBonosSalobre.Text = "Bonos Asociación de Campos"
        '
        'TextBoxGrupoArticulosBonos
        '
        Me.TextBoxGrupoArticulosBonos.Location = New System.Drawing.Point(497, 15)
        Me.TextBoxGrupoArticulosBonos.Name = "TextBoxGrupoArticulosBonos"
        Me.TextBoxGrupoArticulosBonos.Size = New System.Drawing.Size(26, 20)
        Me.TextBoxGrupoArticulosBonos.TabIndex = 42
        '
        'ComboBoxGrupoArticulosBonos
        '
        Me.ComboBoxGrupoArticulosBonos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxGrupoArticulosBonos.DropDownWidth = 250
        Me.ComboBoxGrupoArticulosBonos.FormattingEnabled = True
        Me.ComboBoxGrupoArticulosBonos.Location = New System.Drawing.Point(529, 14)
        Me.ComboBoxGrupoArticulosBonos.Name = "ComboBoxGrupoArticulosBonos"
        Me.ComboBoxGrupoArticulosBonos.Size = New System.Drawing.Size(96, 21)
        Me.ComboBoxGrupoArticulosBonos.TabIndex = 23
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Location = New System.Drawing.Point(322, 16)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(169, 13)
        Me.Label41.TabIndex = 22
        Me.Label41.Text = "Grupo Artículos Bonos Asociación"
        '
        'TextBoxComisionBonos
        '
        Me.TextBoxComisionBonos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxComisionBonos.Location = New System.Drawing.Point(388, 38)
        Me.TextBoxComisionBonos.Name = "TextBoxComisionBonos"
        Me.TextBoxComisionBonos.Size = New System.Drawing.Size(47, 20)
        Me.TextBoxComisionBonos.TabIndex = 21
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.Location = New System.Drawing.Point(322, 38)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(60, 13)
        Me.Label40.TabIndex = 20
        Me.Label40.Text = "% Comisión"
        '
        'TextBoxCtba12
        '
        Me.TextBoxCtba12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtba12.Location = New System.Drawing.Point(216, 64)
        Me.TextBoxCtba12.Name = "TextBoxCtba12"
        Me.TextBoxCtba12.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtba12.TabIndex = 19
        '
        'TextBoxCtba11
        '
        Me.TextBoxCtba11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtba11.Location = New System.Drawing.Point(110, 64)
        Me.TextBoxCtba11.Name = "TextBoxCtba11"
        Me.TextBoxCtba11.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtba11.TabIndex = 18
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Location = New System.Drawing.Point(10, 64)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(81, 13)
        Me.Label36.TabIndex = 17
        Me.Label36.Text = "Por el Consumo"
        '
        'TextBoxCtba10
        '
        Me.TextBoxCtba10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtba10.Location = New System.Drawing.Point(216, 38)
        Me.TextBoxCtba10.Name = "TextBoxCtba10"
        Me.TextBoxCtba10.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtba10.TabIndex = 16
        '
        'TextBoxCtba9
        '
        Me.TextBoxCtba9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtba9.Location = New System.Drawing.Point(110, 38)
        Me.TextBoxCtba9.Name = "TextBoxCtba9"
        Me.TextBoxCtba9.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtba9.TabIndex = 15
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.Location = New System.Drawing.Point(10, 38)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(79, 13)
        Me.Label38.TabIndex = 14
        Me.Label38.Text = "Por la Comisión"
        '
        'TextBoxCtba8
        '
        Me.TextBoxCtba8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtba8.Location = New System.Drawing.Point(216, 16)
        Me.TextBoxCtba8.Name = "TextBoxCtba8"
        Me.TextBoxCtba8.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtba8.TabIndex = 13
        '
        'TextBoxCtba7
        '
        Me.TextBoxCtba7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtba7.Location = New System.Drawing.Point(110, 16)
        Me.TextBoxCtba7.Name = "TextBoxCtba7"
        Me.TextBoxCtba7.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtba7.TabIndex = 12
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Location = New System.Drawing.Point(10, 16)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(69, 13)
        Me.Label39.TabIndex = 11
        Me.Label39.Text = "Por La Venta"
        '
        'GroupBoxBonosAsociacion
        '
        Me.GroupBoxBonosAsociacion.Controls.Add(Me.TextBoxCtba6)
        Me.GroupBoxBonosAsociacion.Controls.Add(Me.TextBoxCtba5)
        Me.GroupBoxBonosAsociacion.Controls.Add(Me.Label35)
        Me.GroupBoxBonosAsociacion.Controls.Add(Me.TextBoxCtba4)
        Me.GroupBoxBonosAsociacion.Controls.Add(Me.TextBoxCtba3)
        Me.GroupBoxBonosAsociacion.Controls.Add(Me.Label37)
        Me.GroupBoxBonosAsociacion.Controls.Add(Me.TextBoxCtba2)
        Me.GroupBoxBonosAsociacion.Controls.Add(Me.TextBoxCtba1)
        Me.GroupBoxBonosAsociacion.Controls.Add(Me.Label34)
        Me.GroupBoxBonosAsociacion.Location = New System.Drawing.Point(3, 98)
        Me.GroupBoxBonosAsociacion.Name = "GroupBoxBonosAsociacion"
        Me.GroupBoxBonosAsociacion.Size = New System.Drawing.Size(634, 101)
        Me.GroupBoxBonosAsociacion.TabIndex = 42
        Me.GroupBoxBonosAsociacion.TabStop = False
        Me.GroupBoxBonosAsociacion.Text = "Bonos Salobre Golf"
        '
        'TextBoxCtba6
        '
        Me.TextBoxCtba6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtba6.Location = New System.Drawing.Point(218, 73)
        Me.TextBoxCtba6.Name = "TextBoxCtba6"
        Me.TextBoxCtba6.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtba6.TabIndex = 10
        '
        'TextBoxCtba5
        '
        Me.TextBoxCtba5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtba5.Location = New System.Drawing.Point(112, 73)
        Me.TextBoxCtba5.Name = "TextBoxCtba5"
        Me.TextBoxCtba5.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtba5.TabIndex = 9
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(12, 73)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(95, 13)
        Me.Label35.TabIndex = 8
        Me.Label35.Text = "Por el Vencimiento"
        '
        'TextBoxCtba4
        '
        Me.TextBoxCtba4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtba4.Location = New System.Drawing.Point(218, 47)
        Me.TextBoxCtba4.Name = "TextBoxCtba4"
        Me.TextBoxCtba4.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtba4.TabIndex = 7
        '
        'TextBoxCtba3
        '
        Me.TextBoxCtba3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtba3.Location = New System.Drawing.Point(112, 47)
        Me.TextBoxCtba3.Name = "TextBoxCtba3"
        Me.TextBoxCtba3.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtba3.TabIndex = 5
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Location = New System.Drawing.Point(12, 51)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(81, 13)
        Me.Label37.TabIndex = 4
        Me.Label37.Text = "Por el Consumo"
        '
        'TextBoxCtba2
        '
        Me.TextBoxCtba2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtba2.Location = New System.Drawing.Point(218, 25)
        Me.TextBoxCtba2.Name = "TextBoxCtba2"
        Me.TextBoxCtba2.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtba2.TabIndex = 3
        '
        'TextBoxCtba1
        '
        Me.TextBoxCtba1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtba1.Location = New System.Drawing.Point(112, 25)
        Me.TextBoxCtba1.Name = "TextBoxCtba1"
        Me.TextBoxCtba1.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCtba1.TabIndex = 1
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Location = New System.Drawing.Point(12, 25)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(69, 13)
        Me.Label34.TabIndex = 0
        Me.Label34.Text = "Por La Venta"
        '
        'TextBoxCentroCostoComisionesVisa
        '
        Me.TextBoxCentroCostoComisionesVisa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCentroCostoComisionesVisa.Location = New System.Drawing.Point(191, 72)
        Me.TextBoxCentroCostoComisionesVisa.Name = "TextBoxCentroCostoComisionesVisa"
        Me.TextBoxCentroCostoComisionesVisa.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCentroCostoComisionesVisa.TabIndex = 41
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Location = New System.Drawing.Point(8, 75)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(162, 13)
        Me.Label33.TabIndex = 40
        Me.Label33.Text = "Centro de Costo Comisiones Visa"
        '
        'TextBoxCodigoClientesContado
        '
        Me.TextBoxCodigoClientesContado.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCodigoClientesContado.Location = New System.Drawing.Point(118, 38)
        Me.TextBoxCodigoClientesContado.Name = "TextBoxCodigoClientesContado"
        Me.TextBoxCodigoClientesContado.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxCodigoClientesContado.TabIndex = 39
        '
        'Label32
        '
        Me.Label32.Location = New System.Drawing.Point(8, 40)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(138, 28)
        Me.Label32.TabIndex = 38
        Me.Label32.Text = "Códido de  Clientes Contado"
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox1.Location = New System.Drawing.Point(118, 8)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(100, 20)
        Me.TextBox1.TabIndex = 37
        '
        'Label31
        '
        Me.Label31.Location = New System.Drawing.Point(8, 8)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(104, 32)
        Me.Label31.TabIndex = 0
        Me.Label31.Text = "Cuenta No Alojado Invitaciones"
        '
        'TabPageContanet
        '
        Me.TabPageContanet.Controls.Add(Me.ButtonPathEmpresaContanet)
        Me.TabPageContanet.Controls.Add(Me.TextBoxRutaEmpresaContanet)
        Me.TabPageContanet.Controls.Add(Me.Label29)
        Me.TabPageContanet.Location = New System.Drawing.Point(4, 22)
        Me.TabPageContanet.Name = "TabPageContanet"
        Me.TabPageContanet.Size = New System.Drawing.Size(714, 471)
        Me.TabPageContanet.TabIndex = 10
        Me.TabPageContanet.Text = "Contanet"
        Me.TabPageContanet.UseVisualStyleBackColor = True
        '
        'ButtonPathEmpresaContanet
        '
        Me.ButtonPathEmpresaContanet.Location = New System.Drawing.Point(526, 8)
        Me.ButtonPathEmpresaContanet.Name = "ButtonPathEmpresaContanet"
        Me.ButtonPathEmpresaContanet.Size = New System.Drawing.Size(48, 23)
        Me.ButtonPathEmpresaContanet.TabIndex = 34
        Me.ButtonPathEmpresaContanet.Text = ":::"
        Me.ButtonPathEmpresaContanet.UseVisualStyleBackColor = True
        '
        'TextBoxRutaEmpresaContanet
        '
        Me.TextBoxRutaEmpresaContanet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxRutaEmpresaContanet.Location = New System.Drawing.Point(88, 8)
        Me.TextBoxRutaEmpresaContanet.Name = "TextBoxRutaEmpresaContanet"
        Me.TextBoxRutaEmpresaContanet.Size = New System.Drawing.Size(432, 20)
        Me.TextBoxRutaEmpresaContanet.TabIndex = 33
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(8, 8)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(74, 13)
        Me.Label29.TabIndex = 0
        Me.Label29.Text = "Ruta Empresa"
        '
        'TabPageVital
        '
        Me.TabPageVital.Controls.Add(Me.GroupBoxVitalGeneral)
        Me.TabPageVital.Controls.Add(Me.GroupBoxVitalOperacionesdeCaja)
        Me.TabPageVital.Location = New System.Drawing.Point(4, 22)
        Me.TabPageVital.Name = "TabPageVital"
        Me.TabPageVital.Size = New System.Drawing.Size(714, 471)
        Me.TabPageVital.TabIndex = 11
        Me.TabPageVital.Text = "Vital Suites"
        Me.TabPageVital.UseVisualStyleBackColor = True
        '
        'GroupBoxVitalGeneral
        '
        Me.GroupBoxVitalGeneral.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxVitalGeneral.Controls.Add(Me.Label117)
        Me.GroupBoxVitalGeneral.Controls.Add(Me.TextBoxVitalIdentificadorDepartemento)
        Me.GroupBoxVitalGeneral.Controls.Add(Me.Label107)
        Me.GroupBoxVitalGeneral.Controls.Add(Me.TextBoxVitalSuplementoDesayuno)
        Me.GroupBoxVitalGeneral.Controls.Add(Me.ButtonServiciosNewHotel)
        Me.GroupBoxVitalGeneral.Location = New System.Drawing.Point(11, 18)
        Me.GroupBoxVitalGeneral.Name = "GroupBoxVitalGeneral"
        Me.GroupBoxVitalGeneral.Size = New System.Drawing.Size(686, 102)
        Me.GroupBoxVitalGeneral.TabIndex = 46
        Me.GroupBoxVitalGeneral.TabStop = False
        Me.GroupBoxVitalGeneral.Text = "General"
        '
        'Label117
        '
        Me.Label117.AutoSize = True
        Me.Label117.Location = New System.Drawing.Point(6, 48)
        Me.Label117.Name = "Label117"
        Me.Label117.Size = New System.Drawing.Size(196, 13)
        Me.Label117.TabIndex = 42
        Me.Label117.Text = "Identificador Genérico de Departamento"
        '
        'TextBoxVitalIdentificadorDepartemento
        '
        Me.TextBoxVitalIdentificadorDepartemento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxVitalIdentificadorDepartemento.Location = New System.Drawing.Point(243, 46)
        Me.TextBoxVitalIdentificadorDepartemento.Name = "TextBoxVitalIdentificadorDepartemento"
        Me.TextBoxVitalIdentificadorDepartemento.Size = New System.Drawing.Size(39, 20)
        Me.TextBoxVitalIdentificadorDepartemento.TabIndex = 43
        '
        'Label107
        '
        Me.Label107.AutoSize = True
        Me.Label107.Location = New System.Drawing.Point(6, 16)
        Me.Label107.Name = "Label107"
        Me.Label107.Size = New System.Drawing.Size(155, 13)
        Me.Label107.TabIndex = 0
        Me.Label107.Text = "Servicio Suplemento Desayuno"
        '
        'TextBoxVitalSuplementoDesayuno
        '
        Me.TextBoxVitalSuplementoDesayuno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxVitalSuplementoDesayuno.Location = New System.Drawing.Point(182, 14)
        Me.TextBoxVitalSuplementoDesayuno.Name = "TextBoxVitalSuplementoDesayuno"
        Me.TextBoxVitalSuplementoDesayuno.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxVitalSuplementoDesayuno.TabIndex = 1
        '
        'ButtonServiciosNewHotel
        '
        Me.ButtonServiciosNewHotel.Location = New System.Drawing.Point(288, 14)
        Me.ButtonServiciosNewHotel.Name = "ButtonServiciosNewHotel"
        Me.ButtonServiciosNewHotel.Size = New System.Drawing.Size(26, 23)
        Me.ButtonServiciosNewHotel.TabIndex = 41
        Me.ButtonServiciosNewHotel.Text = ":::"
        Me.ButtonServiciosNewHotel.UseVisualStyleBackColor = True
        '
        'GroupBoxVitalOperacionesdeCaja
        '
        Me.GroupBoxVitalOperacionesdeCaja.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxVitalOperacionesdeCaja.Controls.Add(Me.TextBoxVitalCajaDebe)
        Me.GroupBoxVitalOperacionesdeCaja.Controls.Add(Me.TextBoxVitalCajaHaber)
        Me.GroupBoxVitalOperacionesdeCaja.Controls.Add(Me.ButtonFormasdeCobroNewHotel)
        Me.GroupBoxVitalOperacionesdeCaja.Controls.Add(Me.Label109)
        Me.GroupBoxVitalOperacionesdeCaja.Controls.Add(Me.Label111)
        Me.GroupBoxVitalOperacionesdeCaja.Controls.Add(Me.Label110)
        Me.GroupBoxVitalOperacionesdeCaja.Controls.Add(Me.TextBoxVitalForeEfectivo)
        Me.GroupBoxVitalOperacionesdeCaja.Location = New System.Drawing.Point(11, 126)
        Me.GroupBoxVitalOperacionesdeCaja.Name = "GroupBoxVitalOperacionesdeCaja"
        Me.GroupBoxVitalOperacionesdeCaja.Size = New System.Drawing.Size(686, 162)
        Me.GroupBoxVitalOperacionesdeCaja.TabIndex = 45
        Me.GroupBoxVitalOperacionesdeCaja.TabStop = False
        Me.GroupBoxVitalOperacionesdeCaja.Text = "Operaciones de Caja"
        '
        'TextBoxVitalCajaDebe
        '
        Me.TextBoxVitalCajaDebe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxVitalCajaDebe.Location = New System.Drawing.Point(105, 28)
        Me.TextBoxVitalCajaDebe.Name = "TextBoxVitalCajaDebe"
        Me.TextBoxVitalCajaDebe.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxVitalCajaDebe.TabIndex = 3
        '
        'TextBoxVitalCajaHaber
        '
        Me.TextBoxVitalCajaHaber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxVitalCajaHaber.Location = New System.Drawing.Point(105, 64)
        Me.TextBoxVitalCajaHaber.Name = "TextBoxVitalCajaHaber"
        Me.TextBoxVitalCajaHaber.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxVitalCajaHaber.TabIndex = 4
        '
        'ButtonFormasdeCobroNewHotel
        '
        Me.ButtonFormasdeCobroNewHotel.Location = New System.Drawing.Point(211, 121)
        Me.ButtonFormasdeCobroNewHotel.Name = "ButtonFormasdeCobroNewHotel"
        Me.ButtonFormasdeCobroNewHotel.Size = New System.Drawing.Size(26, 23)
        Me.ButtonFormasdeCobroNewHotel.TabIndex = 44
        Me.ButtonFormasdeCobroNewHotel.Text = ":::"
        Me.ButtonFormasdeCobroNewHotel.UseVisualStyleBackColor = True
        '
        'Label109
        '
        Me.Label109.AutoSize = True
        Me.Label109.Location = New System.Drawing.Point(6, 35)
        Me.Label109.Name = "Label109"
        Me.Label109.Size = New System.Drawing.Size(70, 13)
        Me.Label109.TabIndex = 5
        Me.Label109.Text = "Cuenta Debe"
        '
        'Label111
        '
        Me.Label111.AutoSize = True
        Me.Label111.Location = New System.Drawing.Point(5, 131)
        Me.Label111.Name = "Label111"
        Me.Label111.Size = New System.Drawing.Size(94, 13)
        Me.Label111.TabIndex = 43
        Me.Label111.Text = "Fore Codi Efectivo"
        '
        'Label110
        '
        Me.Label110.AutoSize = True
        Me.Label110.Location = New System.Drawing.Point(6, 71)
        Me.Label110.Name = "Label110"
        Me.Label110.Size = New System.Drawing.Size(73, 13)
        Me.Label110.TabIndex = 6
        Me.Label110.Text = "Cuenta Haber"
        '
        'TextBoxVitalForeEfectivo
        '
        Me.TextBoxVitalForeEfectivo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxVitalForeEfectivo.Location = New System.Drawing.Point(105, 124)
        Me.TextBoxVitalForeEfectivo.Name = "TextBoxVitalForeEfectivo"
        Me.TextBoxVitalForeEfectivo.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxVitalForeEfectivo.TabIndex = 42
        '
        'TabPageMorasol
        '
        Me.TabPageMorasol.Controls.Add(Me.TextBoxMoraSourceType)
        Me.TabPageMorasol.Controls.Add(Me.Label125)
        Me.TabPageMorasol.Controls.Add(Me.GroupBox7)
        Me.TabPageMorasol.Controls.Add(Me.TextBoxMoraSufijoDepositos)
        Me.TabPageMorasol.Controls.Add(Me.Label120)
        Me.TabPageMorasol.Controls.Add(Me.TextBoxMoraSufijoAnticipos)
        Me.TabPageMorasol.Controls.Add(Me.Label121)
        Me.TabPageMorasol.Controls.Add(Me.TextBoxJournalBatch)
        Me.TabPageMorasol.Controls.Add(Me.Label118)
        Me.TabPageMorasol.Controls.Add(Me.TextBoxJournalTemplate)
        Me.TabPageMorasol.Controls.Add(Me.Label119)
        Me.TabPageMorasol.Controls.Add(Me.TextBoxTitoIgicProducto)
        Me.TabPageMorasol.Controls.Add(Me.Label116)
        Me.TabPageMorasol.Controls.Add(Me.TextBoxTitoIgicNegocio)
        Me.TabPageMorasol.Controls.Add(Me.Label115)
        Me.TabPageMorasol.Controls.Add(Me.ButtonSeccionesNewhotel2)
        Me.TabPageMorasol.Controls.Add(Me.TextBoxSeccionAnticiposNh)
        Me.TabPageMorasol.Controls.Add(Me.Label114)
        Me.TabPageMorasol.Controls.Add(Me.TextBoxTitoDimensionHotel)
        Me.TabPageMorasol.Controls.Add(Me.Label113)
        Me.TabPageMorasol.Controls.Add(Me.TextBoxTitoPrefijoProduccion)
        Me.TabPageMorasol.Controls.Add(Me.Label112)
        Me.TabPageMorasol.Controls.Add(Me.TextBoxWebServiceEmpName)
        Me.TabPageMorasol.Controls.Add(Me.Label108)
        Me.TabPageMorasol.Location = New System.Drawing.Point(4, 22)
        Me.TabPageMorasol.Name = "TabPageMorasol"
        Me.TabPageMorasol.Size = New System.Drawing.Size(714, 471)
        Me.TabPageMorasol.TabIndex = 12
        Me.TabPageMorasol.Text = "Hermanos Tito(Grupo Morasol)"
        Me.TabPageMorasol.UseVisualStyleBackColor = True
        '
        'TextBoxMoraSourceType
        '
        Me.TextBoxMoraSourceType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxMoraSourceType.Location = New System.Drawing.Point(547, 204)
        Me.TextBoxMoraSourceType.Name = "TextBoxMoraSourceType"
        Me.TextBoxMoraSourceType.Size = New System.Drawing.Size(72, 20)
        Me.TextBoxMoraSourceType.TabIndex = 58
        '
        'Label125
        '
        Me.Label125.AutoSize = True
        Me.Label125.Location = New System.Drawing.Point(349, 206)
        Me.Label125.Name = "Label125"
        Me.Label125.Size = New System.Drawing.Size(112, 26)
        Me.Label125.TabIndex = 57
        Me.Label125.Text = "Source Type" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Tipo de Procedencia)"
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.TextBoxMoraEquivDimenHot)
        Me.GroupBox7.Controls.Add(Me.TextBoxMoraEquivDimenDep)
        Me.GroupBox7.Controls.Add(Me.TextBoxMoraEquivDimenNat)
        Me.GroupBox7.Controls.Add(Me.Label124)
        Me.GroupBox7.Controls.Add(Me.Label123)
        Me.GroupBox7.Controls.Add(Me.Label122)
        Me.GroupBox7.Location = New System.Drawing.Point(350, 51)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(325, 133)
        Me.GroupBox7.TabIndex = 56
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Equivalencia de Dimensiones en Navision"
        '
        'TextBoxMoraEquivDimenHot
        '
        Me.TextBoxMoraEquivDimenHot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxMoraEquivDimenHot.Location = New System.Drawing.Point(197, 91)
        Me.TextBoxMoraEquivDimenHot.Name = "TextBoxMoraEquivDimenHot"
        Me.TextBoxMoraEquivDimenHot.Size = New System.Drawing.Size(72, 20)
        Me.TextBoxMoraEquivDimenHot.TabIndex = 54
        '
        'TextBoxMoraEquivDimenDep
        '
        Me.TextBoxMoraEquivDimenDep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxMoraEquivDimenDep.Location = New System.Drawing.Point(197, 58)
        Me.TextBoxMoraEquivDimenDep.Name = "TextBoxMoraEquivDimenDep"
        Me.TextBoxMoraEquivDimenDep.Size = New System.Drawing.Size(72, 20)
        Me.TextBoxMoraEquivDimenDep.TabIndex = 53
        '
        'TextBoxMoraEquivDimenNat
        '
        Me.TextBoxMoraEquivDimenNat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxMoraEquivDimenNat.Location = New System.Drawing.Point(197, 27)
        Me.TextBoxMoraEquivDimenNat.Name = "TextBoxMoraEquivDimenNat"
        Me.TextBoxMoraEquivDimenNat.Size = New System.Drawing.Size(72, 20)
        Me.TextBoxMoraEquivDimenNat.TabIndex = 52
        '
        'Label124
        '
        Me.Label124.AutoSize = True
        Me.Label124.Location = New System.Drawing.Point(16, 93)
        Me.Label124.Name = "Label124"
        Me.Label124.Size = New System.Drawing.Size(160, 13)
        Me.Label124.TabIndex = 2
        Me.Label124.Text = "Dimensión Acceso Directo Hotel"
        '
        'Label123
        '
        Me.Label123.AutoSize = True
        Me.Label123.Location = New System.Drawing.Point(16, 60)
        Me.Label123.Name = "Label123"
        Me.Label123.Size = New System.Drawing.Size(159, 13)
        Me.Label123.TabIndex = 1
        Me.Label123.Text = "Dimensión Global Departamento"
        '
        'Label122
        '
        Me.Label122.AutoSize = True
        Me.Label122.Location = New System.Drawing.Point(16, 30)
        Me.Label122.Name = "Label122"
        Me.Label122.Size = New System.Drawing.Size(143, 13)
        Me.Label122.TabIndex = 0
        Me.Label122.Text = "Dimensión Global Naturaleza"
        '
        'TextBoxMoraSufijoDepositos
        '
        Me.TextBoxMoraSufijoDepositos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxMoraSufijoDepositos.Location = New System.Drawing.Point(183, 181)
        Me.TextBoxMoraSufijoDepositos.Name = "TextBoxMoraSufijoDepositos"
        Me.TextBoxMoraSufijoDepositos.Size = New System.Drawing.Size(129, 20)
        Me.TextBoxMoraSufijoDepositos.TabIndex = 55
        '
        'Label120
        '
        Me.Label120.AutoSize = True
        Me.Label120.Location = New System.Drawing.Point(3, 183)
        Me.Label120.Name = "Label120"
        Me.Label120.Size = New System.Drawing.Size(140, 13)
        Me.Label120.TabIndex = 54
        Me.Label120.Text = "Sufijo Cta. Cliente Depósitos"
        '
        'TextBoxMoraSufijoAnticipos
        '
        Me.TextBoxMoraSufijoAnticipos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxMoraSufijoAnticipos.Location = New System.Drawing.Point(183, 144)
        Me.TextBoxMoraSufijoAnticipos.Name = "TextBoxMoraSufijoAnticipos"
        Me.TextBoxMoraSufijoAnticipos.Size = New System.Drawing.Size(129, 20)
        Me.TextBoxMoraSufijoAnticipos.TabIndex = 53
        '
        'Label121
        '
        Me.Label121.AutoSize = True
        Me.Label121.Location = New System.Drawing.Point(3, 151)
        Me.Label121.Name = "Label121"
        Me.Label121.Size = New System.Drawing.Size(136, 13)
        Me.Label121.TabIndex = 52
        Me.Label121.Text = "Sufijo Cta. Cliente Anticipos"
        '
        'TextBoxJournalBatch
        '
        Me.TextBoxJournalBatch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxJournalBatch.Enabled = False
        Me.TextBoxJournalBatch.Location = New System.Drawing.Point(183, 340)
        Me.TextBoxJournalBatch.Name = "TextBoxJournalBatch"
        Me.TextBoxJournalBatch.Size = New System.Drawing.Size(129, 20)
        Me.TextBoxJournalBatch.TabIndex = 51
        '
        'Label118
        '
        Me.Label118.AutoSize = True
        Me.Label118.Location = New System.Drawing.Point(3, 342)
        Me.Label118.Name = "Label118"
        Me.Label118.Size = New System.Drawing.Size(156, 26)
        Me.Label118.TabIndex = 50
        Me.Label118.Text = "Journal Batch Name (NO Usar)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Es de Solo Lectura en Navision"
        '
        'TextBoxJournalTemplate
        '
        Me.TextBoxJournalTemplate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxJournalTemplate.Location = New System.Drawing.Point(183, 303)
        Me.TextBoxJournalTemplate.Name = "TextBoxJournalTemplate"
        Me.TextBoxJournalTemplate.Size = New System.Drawing.Size(129, 20)
        Me.TextBoxJournalTemplate.TabIndex = 49
        '
        'Label119
        '
        Me.Label119.AutoSize = True
        Me.Label119.Location = New System.Drawing.Point(3, 310)
        Me.Label119.Name = "Label119"
        Me.Label119.Size = New System.Drawing.Size(122, 13)
        Me.Label119.TabIndex = 48
        Me.Label119.Text = "Journal Template Name "
        '
        'TextBoxTitoIgicProducto
        '
        Me.TextBoxTitoIgicProducto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxTitoIgicProducto.Location = New System.Drawing.Point(183, 270)
        Me.TextBoxTitoIgicProducto.Name = "TextBoxTitoIgicProducto"
        Me.TextBoxTitoIgicProducto.Size = New System.Drawing.Size(129, 20)
        Me.TextBoxTitoIgicProducto.TabIndex = 47
        '
        'Label116
        '
        Me.Label116.AutoSize = True
        Me.Label116.Location = New System.Drawing.Point(4, 272)
        Me.Label116.Name = "Label116"
        Me.Label116.Size = New System.Drawing.Size(134, 26)
        Me.Label116.TabIndex = 46
        Me.Label116.Text = "Igic Producto" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(VAT_Bus_Posting_Group)"
        '
        'TextBoxTitoIgicNegocio
        '
        Me.TextBoxTitoIgicNegocio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxTitoIgicNegocio.Location = New System.Drawing.Point(183, 233)
        Me.TextBoxTitoIgicNegocio.Name = "TextBoxTitoIgicNegocio"
        Me.TextBoxTitoIgicNegocio.Size = New System.Drawing.Size(129, 20)
        Me.TextBoxTitoIgicNegocio.TabIndex = 45
        '
        'Label115
        '
        Me.Label115.AutoSize = True
        Me.Label115.Location = New System.Drawing.Point(4, 240)
        Me.Label115.Name = "Label115"
        Me.Label115.Size = New System.Drawing.Size(133, 26)
        Me.Label115.TabIndex = 44
        Me.Label115.Text = "Igic Grupo de Negocio" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Gen_Bus_Posting_Group)"
        '
        'ButtonSeccionesNewhotel2
        '
        Me.ButtonSeccionesNewhotel2.Location = New System.Drawing.Point(318, 106)
        Me.ButtonSeccionesNewhotel2.Name = "ButtonSeccionesNewhotel2"
        Me.ButtonSeccionesNewhotel2.Size = New System.Drawing.Size(26, 23)
        Me.ButtonSeccionesNewhotel2.TabIndex = 43
        Me.ButtonSeccionesNewhotel2.Text = ":::"
        Me.ButtonSeccionesNewhotel2.UseVisualStyleBackColor = True
        '
        'TextBoxSeccionAnticiposNh
        '
        Me.TextBoxSeccionAnticiposNh.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxSeccionAnticiposNh.Location = New System.Drawing.Point(183, 106)
        Me.TextBoxSeccionAnticiposNh.Name = "TextBoxSeccionAnticiposNh"
        Me.TextBoxSeccionAnticiposNh.Size = New System.Drawing.Size(129, 20)
        Me.TextBoxSeccionAnticiposNh.TabIndex = 42
        '
        'Label114
        '
        Me.Label114.AutoSize = True
        Me.Label114.Location = New System.Drawing.Point(4, 113)
        Me.Label114.Name = "Label114"
        Me.Label114.Size = New System.Drawing.Size(142, 13)
        Me.Label114.TabIndex = 41
        Me.Label114.Text = "Sección Anticipos NewHotel"
        '
        'TextBoxTitoDimensionHotel
        '
        Me.TextBoxTitoDimensionHotel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxTitoDimensionHotel.Location = New System.Drawing.Point(183, 54)
        Me.TextBoxTitoDimensionHotel.Name = "TextBoxTitoDimensionHotel"
        Me.TextBoxTitoDimensionHotel.Size = New System.Drawing.Size(129, 20)
        Me.TextBoxTitoDimensionHotel.TabIndex = 6
        '
        'Label113
        '
        Me.Label113.AutoSize = True
        Me.Label113.Location = New System.Drawing.Point(4, 54)
        Me.Label113.Name = "Label113"
        Me.Label113.Size = New System.Drawing.Size(175, 13)
        Me.Label113.TabIndex = 5
        Me.Label113.Text = "Dimensión de Acceso Directo Hotel"
        '
        'TextBoxTitoPrefijoProduccion
        '
        Me.TextBoxTitoPrefijoProduccion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxTitoPrefijoProduccion.Location = New System.Drawing.Point(183, 78)
        Me.TextBoxTitoPrefijoProduccion.Name = "TextBoxTitoPrefijoProduccion"
        Me.TextBoxTitoPrefijoProduccion.Size = New System.Drawing.Size(129, 20)
        Me.TextBoxTitoPrefijoProduccion.TabIndex = 4
        '
        'Label112
        '
        Me.Label112.AutoSize = True
        Me.Label112.Location = New System.Drawing.Point(4, 80)
        Me.Label112.Name = "Label112"
        Me.Label112.Size = New System.Drawing.Size(131, 13)
        Me.Label112.TabIndex = 3
        Me.Label112.Text = "Prefijo Nº Doc Producción"
        '
        'TextBoxWebServiceEmpName
        '
        Me.TextBoxWebServiceEmpName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxWebServiceEmpName.Location = New System.Drawing.Point(183, 16)
        Me.TextBoxWebServiceEmpName.Name = "TextBoxWebServiceEmpName"
        Me.TextBoxWebServiceEmpName.Size = New System.Drawing.Size(492, 20)
        Me.TextBoxWebServiceEmpName.TabIndex = 2
        '
        'Label108
        '
        Me.Label108.AutoSize = True
        Me.Label108.Location = New System.Drawing.Point(14, 18)
        Me.Label108.Name = "Label108"
        Me.Label108.Size = New System.Drawing.Size(155, 13)
        Me.Label108.TabIndex = 0
        Me.Label108.Text = "Nombre Empresa WebServices"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.ButtonAceptar)
        Me.GroupBox2.Controls.Add(Me.ButtonCancelar)
        Me.GroupBox2.Location = New System.Drawing.Point(746, 40)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(104, 521)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Image = CType(resources.GetObject("ButtonAceptar.Image"), System.Drawing.Image)
        Me.ButtonAceptar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonAceptar.Location = New System.Drawing.Point(8, 16)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(80, 23)
        Me.ButtonAceptar.TabIndex = 12
        Me.ButtonAceptar.Text = "&Aceptar"
        '
        'ButtonCancelar
        '
        Me.ButtonCancelar.Location = New System.Drawing.Point(8, 56)
        Me.ButtonCancelar.Name = "ButtonCancelar"
        Me.ButtonCancelar.Size = New System.Drawing.Size(80, 23)
        Me.ButtonCancelar.TabIndex = 1
        Me.ButtonCancelar.Text = "&Cancelar"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(8, 16)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(64, 23)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "Grupo Cod"
        '
        'ComboBoxGrupoCod
        '
        Me.ComboBoxGrupoCod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxGrupoCod.Location = New System.Drawing.Point(72, 16)
        Me.ComboBoxGrupoCod.Name = "ComboBoxGrupoCod"
        Me.ComboBoxGrupoCod.Size = New System.Drawing.Size(86, 21)
        Me.ComboBoxGrupoCod.TabIndex = 3
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(164, 16)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(64, 23)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "Emp. Cod"
        '
        'ComboBoxEmpCod
        '
        Me.ComboBoxEmpCod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxEmpCod.DropDownWidth = 250
        Me.ComboBoxEmpCod.Location = New System.Drawing.Point(228, 16)
        Me.ComboBoxEmpCod.Name = "ComboBoxEmpCod"
        Me.ComboBoxEmpCod.Size = New System.Drawing.Size(208, 21)
        Me.ComboBoxEmpCod.TabIndex = 5
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 10000
        Me.ToolTip1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.ToolTip1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.ToolTip1.InitialDelay = 500
        Me.ToolTip1.IsBalloon = True
        Me.ToolTip1.ReshowDelay = 50
        Me.ToolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
        '
        'ButtonNuevo
        '
        Me.ButtonNuevo.Image = CType(resources.GetObject("ButtonNuevo.Image"), System.Drawing.Image)
        Me.ButtonNuevo.Location = New System.Drawing.Point(678, 18)
        Me.ButtonNuevo.Name = "ButtonNuevo"
        Me.ButtonNuevo.Size = New System.Drawing.Size(62, 23)
        Me.ButtonNuevo.TabIndex = 36
        Me.ToolTip1.SetToolTip(Me.ButtonNuevo, "Nueva Empresa")
        Me.ButtonNuevo.UseVisualStyleBackColor = True
        '
        'Label77
        '
        Me.Label77.AutoSize = True
        Me.Label77.Location = New System.Drawing.Point(548, 16)
        Me.Label77.Name = "Label77"
        Me.Label77.Size = New System.Drawing.Size(72, 13)
        Me.Label77.TabIndex = 6
        Me.Label77.Text = "Hotel Nùmero"
        '
        'TextBoxEmpNum
        '
        Me.TextBoxEmpNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxEmpNum.Enabled = False
        Me.TextBoxEmpNum.Location = New System.Drawing.Point(626, 18)
        Me.TextBoxEmpNum.Name = "TextBoxEmpNum"
        Me.TextBoxEmpNum.Size = New System.Drawing.Size(46, 20)
        Me.TextBoxEmpNum.TabIndex = 33
        '
        'TextBoxEmpCod
        '
        Me.TextBoxEmpCod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxEmpCod.Enabled = False
        Me.TextBoxEmpCod.Location = New System.Drawing.Point(500, 18)
        Me.TextBoxEmpCod.Name = "TextBoxEmpCod"
        Me.TextBoxEmpCod.Size = New System.Drawing.Size(32, 20)
        Me.TextBoxEmpCod.TabIndex = 35
        '
        'Label78
        '
        Me.Label78.AutoSize = True
        Me.Label78.Location = New System.Drawing.Point(438, 16)
        Me.Label78.Name = "Label78"
        Me.Label78.Size = New System.Drawing.Size(56, 13)
        Me.Label78.TabIndex = 34
        Me.Label78.Text = "Emp. Cod."
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Button
        '
        Me.Button.Location = New System.Drawing.Point(441, 112)
        Me.Button.Name = "Button"
        Me.Button.Size = New System.Drawing.Size(41, 23)
        Me.Button.TabIndex = 58
        Me.Button.Text = ":::"
        Me.Button.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(438, 239)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(44, 23)
        Me.Button3.TabIndex = 59
        Me.Button3.Text = ":::"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'FormParametrosFront
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(858, 579)
        Me.Controls.Add(Me.ButtonNuevo)
        Me.Controls.Add(Me.TextBoxEmpCod)
        Me.Controls.Add(Me.Label78)
        Me.Controls.Add(Me.TextBoxEmpNum)
        Me.Controls.Add(Me.Label77)
        Me.Controls.Add(Me.ComboBoxEmpCod)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.ComboBoxGrupoCod)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.MinimumSize = New System.Drawing.Size(866, 606)
        Me.Name = "FormParametrosFront"
        Me.Text = "Parámetros Integración Front"
        Me.GroupBox1.ResumeLayout(False)
        Me.TabControlOpciones.ResumeLayout(False)
        Me.TabPageGeneral.ResumeLayout(False)
        Me.TabPageGeneral.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        CType(Me.NumericUpDownTope, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageGeneral2.ResumeLayout(False)
        Me.TabPageGeneral2.PerformLayout()
        Me.TabPage7.ResumeLayout(False)
        Me.TabPage7.PerformLayout()
        Me.TabPageSpyro.ResumeLayout(False)
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.TabPageCadenasOdbc.ResumeLayout(False)
        Me.TabPageCadenasOdbc.PerformLayout()
        Me.TabPageHotelesLopez.ResumeLayout(False)
        Me.TabPageHotelesLopez.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.TabPageGrupoSatocan.ResumeLayout(False)
        Me.TabPageGrupoSatocan.PerformLayout()
        Me.TabControl2.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        CType(Me.NumericUpDownWebServiceTimeOut, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage9.ResumeLayout(False)
        Me.TabPage9.PerformLayout()
        CType(Me.NumericUpDownWebServiceTimeOut2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage4.PerformLayout()
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage5.PerformLayout()
        Me.TabPage8.ResumeLayout(False)
        Me.TabPage8.PerformLayout()
        Me.GroupBoxBonosSalobre.ResumeLayout(False)
        Me.GroupBoxBonosSalobre.PerformLayout()
        Me.GroupBoxBonosAsociacion.ResumeLayout(False)
        Me.GroupBoxBonosAsociacion.PerformLayout()
        Me.TabPageContanet.ResumeLayout(False)
        Me.TabPageContanet.PerformLayout()
        Me.TabPageVital.ResumeLayout(False)
        Me.GroupBoxVitalGeneral.ResumeLayout(False)
        Me.GroupBoxVitalGeneral.PerformLayout()
        Me.GroupBoxVitalOperacionesdeCaja.ResumeLayout(False)
        Me.GroupBoxVitalOperacionesdeCaja.PerformLayout()
        Me.TabPageMorasol.ResumeLayout(False)
        Me.TabPageMorasol.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub TextBoxIndicadorDebe_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBoxIndicadorDebe.Validating
        If Me.TextBoxIndicadorDebe.Text.Length = 0 Then
            ErrorProvider1.SetError(Me.TextBoxIndicadorDebe, "Debe tener un valor")
        Else
            ErrorProvider1.SetError(Me.TextBoxIndicadorDebe, "")
        End If

    End Sub

    Private Sub FormParametrosFront_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If IsNothing(Me.DbLee) = False Or Me.DbLee.EstadoConexion = ConnectionState.Open Then
                Me.DbLee.CerrarConexion()
            End If
            If IsNothing(Me.DbLeeAux) = False Or Me.DbLeeAux.EstadoConexion = ConnectionState.Open Then
                Me.DbLeeAux.CerrarConexion()
            End If

            If IsNothing(Me.DbWrite) = False Or Me.DbWrite.EstadoConexion = ConnectionState.Open Then
                Me.DbWrite.CerrarConexion()
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub FormParametrosFront_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            Me.mParaEmpGrupoCod = MyIni.IniGet(Application.StartupPath & "\menu.ini", "PARAMETER", "PARA_EMPGRUPO_COD")
            Me.DbLee = New C_DATOS.C_DatosOledb(MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"))
            Me.DbLee.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbLeeAux = New C_DATOS.C_DatosOledb(MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"))
            Me.DbLeeAux.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


            Me.DbWrite = New C_DATOS.C_DatosOledb(MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"))
            Me.DbWrite.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.CargaCombos()

        Catch ex As Exception
            MsgBox(ex.Message)
            Me.Close()
        End Try
    End Sub
#Region "RUTINAS"
    Private Sub CargaCombos()
        Try
            ' Emp grupo cod

            SQL = " SELECT DISTINCT PARA_EMPGRUPO_COD FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mParaEmpGrupoCod & "'"
            Me.DbLee.TraerDataset(SQL, "EMPGRUPOCOD")

            Dim NewRowA(0) As Object
            NewRowA(0) = "<Ninguno>"

            Me.DbLee.mDbDataset.Tables("EMPGRUPOCOD").LoadDataRow(NewRowA, False)
            With Me.ComboBoxGrupoCod
                .DataSource = Me.DbLee.mDbDataset.Tables("EMPGRUPOCOD")
                .ValueMember = "PARA_EMPGRUPO_COD"
                .DisplayMember = "PARA_EMPGRUPO_COD"
                '       .SelectedIndex = .Items.Count - 1
            End With
            Me.DbLee.mDbDataset = Nothing

            ' Emp cod
            'SQL = " SELECT PARA_EMP_COD FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mParaEmpGrupoCod & "'"
            SQL = " SELECT PARA_EMP_COD,HOTEL_DESCRIPCION FROM TH_PARA,TH_HOTEL WHERE "
            SQL += "PARA_EMPGRUPO_COD = HOTEL_EMPGRUPO_COD AND PARA_EMP_COD = HOTEL_EMP_COD"
            SQL += " AND PARA_EMPGRUPO_COD = '" & Me.mParaEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_NUM = HOTEL_EMP_NUM "
            SQL += " ORDER BY HOTEL_DESCRIPCION ASC"

            Me.DbLee.TraerDataset(SQL, "EMPCOD")

            Dim NewRowB(0) As Object
            NewRowB(0) = "<Ninguno>"

            Me.DbLee.mDbDataset.Tables("EMPCOD").LoadDataRow(NewRowB, False)
            With Me.ComboBoxEmpCod
                .DataSource = Me.DbLee.mDbDataset.Tables("EMPCOD")
                .ValueMember = "PARA_EMP_COD"
                .DisplayMember = "HOTEL_DESCRIPCION"
                '       .SelectedIndex = .Items.Count - 1
            End With
            Me.DbLee.mDbDataset = Nothing


            ' Escoger el primer elemento en ambos combos
            If IsNothing(Me.ComboBoxGrupoCod.Items.Count) = False Then
                Me.ComboBoxGrupoCod.SelectedIndex = 0
            End If

            If IsNothing(Me.ComboBoxEmpCod.Items.Count) = False Then
                Me.ComboBoxEmpCod.SelectedIndex = 0


                ' MOSTRAR LOS DATOS 
                Me.Cursor = Cursors.WaitCursor
                Me.LimpiarControles()



                ' Mostrar Emp_cod
                SQL = "SELECT HOTEL_EMP_COD FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
                SQL += " AND HOTEL_DESCRIPCION = '" & Me.ComboBoxEmpCod.Text & "'"
                Me.mParaEmpCod = CStr(Me.DbLeeAux.EjecutaSqlScalar(SQL))
                Me.TextBoxEmpCod.Text = Me.mParaEmpCod


                ' averiguar numero de empresa
                SQL = "SELECT HOTEL_EMP_NUM FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
                SQL += " AND HOTEL_DESCRIPCION = '" & Me.ComboBoxEmpCod.Text & "'"
                Me.mParaEmpNum = CInt(Me.DbLeeAux.EjecutaSqlScalar(SQL))
                Me.TextBoxEmpNum.Text = Me.mParaEmpNum


                Me.MostrarDatosParametros()
                Me.MostrarDatosHoteles()
                Me.MostrarDatosNewConta()
                Me.Cursor = Cursors.Default
            End If



        Catch EX As Exception
            MsgBox(EX.Message)
        End Try
    End Sub
    Private Sub MostrarDatosParametros()
        Try
            SQL = "SELECT NVL(PARA_CTA1,'<Ninguno>') AS PARA_CTA1,NVL(PARA_CTA4,'<Ninguno>') AS PARA_CTA4,NVL(PARA_CTA5,'<Ninguno>') AS PARA_CTA5,"
            SQL += "NVL(PARA_DEBE,'<Ninguno>') AS PARA_DEBE,NVL(PARA_HABER,'<Ninguno>') AS PARA_HABER,"
            SQL += "NVL(PARA_DEBE_FAC,'<Ninguno>') AS PARA_DEBE_FAC,NVL(PARA_HABER_FAC,'V') AS PARA_HABER_FAC,"
            SQL += "NVL(PARA_CLIENTES_CONTADO,'<Ninguno>') AS PARA_CLIENTES_CONTADO,NVL(PARA_CLIENTES_CONTADO_CIF,'<Ninguno>') AS PARA_CLIENTES_CONTADO_CIF,"
            SQL += "NVL(PARA_FILE_SPYRO_PATH,'<Ninguno>') AS PARA_FILE_SPYRO_PATH,NVL(PARA_CTA_REDONDEO,'<Ninguno>') AS PARA_CTA_REDONDEO,NVL(PARA_FECHA_REGISTRO_AC,'<Ninguno>') AS PARA_FECHA_REGISTRO_AC,"
            SQL += "NVL(PARA_TEXTO_IVA,'<Ninguno>') AS PARA_TEXTO_IVA,NVL(PARA_SERIE_ANULACION,'<Ninguno>') AS PARA_SERIE_ANULACION,"
            SQL += "NVL(PARA_CENTRO_COSTO_AL,'<Ninguno>') AS PARA_CENTRO_COSTO_AL,NVL(PARA_COMISIONES,'1') AS PARA_COMISIONES,"
            SQL += "NVL(PARA_CLIENTES_CONTADO_CODIGO,'<Ninguno>') AS PARA_CLIENTES_CONTADO_CODIGO,"
            SQL += "NVL(PARA_INGRESO_POR_HABITACION,'0') AS PARA_INGRESO_POR_HABITACION,"
            SQL += "NVL(PARA_TIPO_ANULACION,'2') AS PARA_TIPO_ANULACION,"
            SQL += "NVL(PARA_BCTA1,'<Ninguno>') AS PARA_BCTA1,NVL(PARA_BCTA2,'<Ninguno>') AS PARA_BCTA2,NVL(PARA_BCTA3,'<Ninguno>') AS PARA_BCTA3,NVL(PARA_BCTA4,'<Ninguno>') AS PARA_BCTA4,NVL(PARA_BCTA5,'<Ninguno>') AS PARA_BCTA5,NVL(PARA_BCTA6,'<Ninguno>') AS PARA_BCTA6,"
            SQL += "NVL(PARA_BCTA7,'<Ninguno>') AS PARA_BCTA7,NVL(PARA_BCTA8,'<Ninguno>') AS PARA_BCTA8,NVL(PARA_BCTA9,'<Ninguno>') AS PARA_BCTA9,NVL(PARA_BCTA10,'<Ninguno>') AS PARA_BCTA10,NVL(PARA_BCTA11,'<Ninguno>') AS PARA_BCTA11,NVL(PARA_BCTA12,'<Ninguno>') AS PARA_BCTA12, "
            SQL += "PARA_COMISION_BONOS_ASOC,"
            SQL += "NVL(PARA_CONECTA_NEWGOLF,'0') AS PARA_CONECTA_NEWGOLF,"
            SQL += "NVL(PARA_CONECTA_NEWPOS,'0') AS PARA_CONECTA_NEWPOS,"
            SQL += "NVL(PARA_CENTRO_COSTO_COMI,'<Ninguno>') AS PARA_CENTRO_COSTO_COMI,"

            SQL += "NVL(PARA_SOURCEENDPOINT,'<Ninguno>') AS PARA_SOURCEENDPOINT,"
            SQL += "NVL(PARA_DESTINATIONENDPOINT,'<Ninguno>') AS PARA_DESTINATIONENDPOINT,"
            SQL += "NVL(PARA_DOMAIN_NAME,'<Ninguno>') AS PARA_DOMAIN_NAME,"
            SQL += "NVL(PARA_DOMAIN_USER,'<Ninguno>') AS PARA_DOMAIN_USER,"
            SQL += "NVL(PARA_BANCO_AX,'<Ninguno>') AS PARA_BANCO_AX, "
            SQL += "NVL(PARA_SERV_CODI_BONO,'<Ninguno>') AS PARA_SERV_CODI_BONO, "
            SQL += "NVL(PARA_SERV_CODI_BONOASC,'<Ninguno>') AS PARA_SERV_CODI_BONOASC, "
            SQL += "NVL(PARA_WEBSERVICE_LOCATION,'<Ninguno>') AS PARA_WEBSERVICE_LOCATION ,"
            SQL += "NVL(PARA_ANTICIPO_AX,'<Ninguno>') AS PARA_ANTICIPO_AX ,"
            SQL += "NVL(PARA_WEBSERVICE_TIMEOUT ,0) AS PARA_WEBSERVICE_TIMEOUT,  "
            SQL += "NVL(PARA_DOMAIN_PWD,'<Ninguno>') AS PARA_DOMAIN_PWD, "
            SQL += "NVL(PARA_FILE_FORMAT,'0') AS PARA_FILE_FORMAT, "

            SQL += "NVL(PARA_INGRESO_HABITACION_DPTO,'<Ninguno>') AS PARA_INGRESO_HABITACION_DPTO, "
            SQL += "NVL(PARA_DESGLO_ALOJA_REGIMEN,'0') AS PARA_DESGLO_ALOJA_REGIMEN, "

            '

            SQL += "NVL(PARA_CTA_CALO_AL,'<Ninguno>') AS PARA_CTA_CALO_AL, "
            SQL += "NVL(PARA_CTA_VALO_AL,'<Ninguno>') AS PARA_CTA_VALO_AL, "
            SQL += "NVL(PARA_CTA_CALO_AD,'<Ninguno>') AS PARA_CTA_CALO_AD, "
            SQL += "NVL(PARA_CTA_VALO_AD,'<Ninguno>') AS PARA_CTA_VALO_AD, "
            SQL += "NVL(PARA_CTA_CALO_MP,'<Ninguno>') AS PARA_CTA_CALO_MP, "
            SQL += "NVL(PARA_CTA_VALO_MP,'<Ninguno>') AS PARA_CTA_VALO_MP, "
            SQL += "NVL(PARA_CTA_CALO_PC,'<Ninguno>') AS PARA_CTA_CALO_PC, "
            SQL += "NVL(PARA_CTA_VALO_PC,'<Ninguno>') AS PARA_CTA_VALO_PC, "
            SQL += "NVL(PARA_CTA_CALO_TI,'<Ninguno>') AS PARA_CTA_CALO_TI, "
            SQL += "NVL(PARA_CTA_VALO_TI,'<Ninguno>') AS PARA_CTA_VALO_TI, "
            SQL += "NVL(PARA_CTA_CALO_X,'<Ninguno>') AS PARA_CTA_CALO_X, "
            SQL += "NVL(PARA_CTA_VALO_X,'<Ninguno>') AS PARA_CTA_VALO_X,  "

            SQL += "NVL(PARA_SERRUCHA_DPTO,'0') AS PARA_SERRUCHA_DPTO, "

            SQL += "NVL(PARA_CTA_SERIE_CRE,'<Ninguno>') AS PARA_CTA_SERIE_CRE,  "
            SQL += "NVL(PARA_CTA_SERIE_CON,'<Ninguno>') AS PARA_CTA_SERIE_CON,  "
            SQL += "NVL(PARA_CTA_SERIE_NCRE,'<Ninguno>') AS PARA_CTA_SERIE_NCRE,  "
            SQL += "NVL(PARA_CTA_SERIE_ANUL,'<Ninguno>') AS PARA_CTA_SERIE_ANUL,  "




            SQL += "NVL(PARA_CTA_56DIGITO,'<Ninguno>') AS PARA_CTA_56DIGITO,  "
            SQL += "NVL(PARA_TRANSFERENCIA_AGENCIA,'<Ninguno>') AS PARA_TRANSFERENCIA_AGENCIA,  "

            SQL += "NVL(PARA_ART_ANULA_RESERVA,'<Ninguno>') AS PARA_ART_ANULA_RESERVA,  "

            SQL += "NVL(PARA_CFATODIARI_COD,'<Ninguno>') AS PARA_CFATODIARI_COD,  "
            SQL += "NVL(PARA_CFATODIARI_COD_2,'<Ninguno>') AS PARA_CFATODIARI_COD_2,  "

            SQL += "NVL(PARA_TRATA_CAJA,0) AS PARA_TRATA_CAJA, "

            SQL += "NVL(PARA_PREF_NCREG,'<Ninguno>') AS PARA_PREF_NCREG , "
            SQL += "NVL(PARA_PASO,'<Ninguno>') AS PARA_PASO,  "

            SQL += "NVL(PARA_VALIDA_SPYRO,'0') AS PARA_VALIDA_SPYRO ,"
            SQL += "NVL(PARA_USA_CTA4B,'0') AS PARA_USA_CTA4B, "
            SQL += "NVL(PARA_CTA4B,'<Ninguno>') AS PARA_CTA4B,  "
            SQL += "NVL(PARA_SECC_DEPNH,'<Ninguno>') AS PARA_SECC_DEPNH, "
            SQL += "NVL(PARA_SECC_ANTNH,'<Ninguno>') AS PARA_SECC_ANTNH, "
            SQL += "NVL(PARA_CTA4B2,'<Ninguno>') AS PARA_CTA4B2,  "
            SQL += "NVL(PARA_CTA4B3,'<Ninguno>') AS PARA_CTA4B3,  "
            SQL += "NVL(PARA_AX_AJUSTABASE,'<Ninguno>') AS PARA_AX_AJUSTABASE,  "



            SQL += "NVL(PARA_WEBSERVICE_LOCATION2,'<Ninguno>') AS PARA_WEBSERVICE_LOCATION2 ,"
            SQL += "NVL(PARA_DOMAIN_NAME2,'<Ninguno>') AS PARA_DOMAIN_NAME2,"
            SQL += "NVL(PARA_DOMAIN_USER2,'<Ninguno>') AS PARA_DOMAIN_USER2,"
            SQL += "NVL(PARA_DOMAIN_PWD2,'<Ninguno>') AS PARA_DOMAIN_PWD2, "
            SQL += "NVL(PARA_WEBSERVICE_TIMEOUT2 ,0) AS PARA_WEBSERVICE_TIMEOUT2,  "

            SQL += "NVL(PARA_HOTEL_AX,0) AS PARA_HOTEL_AX, "
            SQL += "NVL(PARA_TRATA_TPV,0) AS PARA_TRATA_TPV ,"
            SQL += "NVL(PARA_FACTUTIPO_COD,'<Ninguno>') AS PARA_FACTUTIPO_COD , "
            SQL += "PARA_CFIVALIBRO_COD AS PARA_CFIVALIBRO_COD  ,"

            SQL += "NVL(PARA_VITAL_SUPLEDESAYUNO,'<Ninguno>') AS PARA_VITAL_SUPLEDESAYUNO , "
            SQL += "NVL(PARA_VITAL_DEBECAJA,'<Ninguno>') AS PARA_VITAL_DEBECAJA , "
            SQL += "NVL(PARA_VITAL_HABERCAJA,'<Ninguno>') AS PARA_VITAL_HABERCAJA,  "

            SQL += "NVL(PARA_VITAL_FOREEFECTIVO,'<Ninguno>') AS PARA_VITAL_FOREEFECTIVO,  "

            SQL += "NVL(PARA_WEBSERVICE_ENAME,'<Ninguno>') AS PARA_WEBSERVICE_ENAME,  "

            SQL += "PARA_MORA_PREPROD, "
            SQL += "PARA_MORA_DIMENHOTEL, "

            SQL += "NVL(PARA_MORA_GRUPONEGOCIO,'<Ninguno>') AS PARA_MORA_GRUPONEGOCIO,  "
            SQL += "NVL(PARA_MORA_GRUPOPRODUCTO,'<Ninguno>') AS PARA_MORA_GRUPOPRODUCTO,  "


            SQL += "PARA_VITAL_DPTO_GENERICO ,"

            SQL += "PARA_MORA_JOURNAL_TEMPLATE , "

            SQL += "PARA_MORA_JOURNAL_BATCH  "


            SQL += ",PARA_MORA_SUFI_ANTI  "
            SQL += ",PARA_MORA_SUFI_DEPO  "


            SQL += ",PARA_MORA_EQUIV_NAT  "
            SQL += ",PARA_MORA_EQUIV_DEP  "
            SQL += ",PARA_MORA_EQUIV_HOT  "


            SQL += ",NVL(PARA_LOPEZ_TIPO_COMPROBANTES,'0') PARA_LOPEZ_TIPO_COMPROBANTES"

            SQL += ",PARA_MORA_SOURCE_TYPE  "
            SQL += ",PARA_TEFECT_COD  "


            SQL += "  FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mParaEmpNum

            Me.DbLee.TraerLector(SQL)

            Me.DbLee.mDbLector.Read()

            If Me.DbLee.mDbLector.HasRows Then
                Me.TextBoxCtaManoCorriente.Text = Me.DbLee.mDbLector.Item("PARA_CTA1")
                Me.TextBoxCtaPagosaCuenta.Text = Me.DbLee.mDbLector.Item("PARA_CTA4")
                Me.TextBoxCtaDesembolsos.Text = Me.DbLee.mDbLector.Item("PARA_CTA5")


                Me.TextBoxIndicadorDebe.Text = Me.DbLee.mDbLector.Item("PARA_DEBE")
                Me.TextBoxIndicadorHaber.Text = Me.DbLee.mDbLector.Item("PARA_HABER")
                Me.TextBoxIndicadorDebeFacturas.Text = Me.DbLee.mDbLector.Item("PARA_DEBE_FAC")
                Me.TextBoxIndicadorHaberFacturas.Text = Me.DbLee.mDbLector.Item("PARA_HABER_FAC")
                Me.TextBoxCuentaClientesContado.Text = Me.DbLee.mDbLector.Item("PARA_CLIENTES_CONTADO")
                Me.TextBoxCifClientesContado.Text = Me.DbLee.mDbLector.Item("PARA_CLIENTES_CONTADO_CIF")
                Me.TextBoxFileSpyroPath.Text = Me.DbLee.mDbLector.Item("PARA_FILE_SPYRO_PATH")
                Me.TextBoxCuentaAjusteRedondeo.Text = Me.DbLee.mDbLector.Item("PARA_CTA_REDONDEO")

                If Me.DbLee.mDbLector.Item("PARA_FECHA_REGISTRO_AC") = "V" Then
                    Me.RadioButtonFechaRegistroAc.Checked = False
                    Me.RadioButtonFechaValorAc.Checked = True
                End If
                If Me.DbLee.mDbLector.Item("PARA_FECHA_REGISTRO_AC") = "R" Then
                    Me.RadioButtonFechaRegistroAc.Checked = True
                    Me.RadioButtonFechaValorAc.Checked = False
                End If

                Me.TextBoxDenominacionImpuesto.Text = Me.DbLee.mDbLector.Item("PARA_TEXTO_IVA")
                Me.TextBoxSerieFacturasAnuladas.Text = Me.DbLee.mDbLector.Item("PARA_SERIE_ANULACION")
                Me.TextBoxCentroCostoComisionesAlojamiento.Text = Me.DbLee.mDbLector.Item("PARA_CENTRO_COSTO_AL")

                If Me.DbLee.mDbLector.Item("PARA_COMISIONES") = "1" Then
                    Me.CheckBoxComisionAfectaImpuesto.Checked = True
                Else
                    Me.CheckBoxComisionAfectaImpuesto.Checked = False
                End If

                Me.TextBoxCodigoClientesContado.Text = Me.DbLee.mDbLector.Item("PARA_CLIENTES_CONTADO_CODIGO")


                If Me.DbLee.mDbLector.Item("PARA_INGRESO_POR_HABITACION") = "1" Then
                    Me.CheckBoxIngresosServicioBloque.Checked = True
                Else
                    Me.CheckBoxIngresosServicioBloque.Checked = False
                End If

                If Me.DbLee.mDbLector.Item("PARA_TIPO_ANULACION") = "1" Then
                    Me.RadioButtonAnulacionStandard.Checked = True
                    Me.RadioButtonAnulacionFactura.Checked = False
                    Me.RadioButtonAnulacionNotaCredito.Checked = False
                ElseIf Me.DbLee.mDbLector.Item("PARA_TIPO_ANULACION") = "2" Then
                    Me.RadioButtonAnulacionStandard.Checked = False
                    Me.RadioButtonAnulacionFactura.Checked = True
                    Me.RadioButtonAnulacionNotaCredito.Checked = False
                ElseIf Me.DbLee.mDbLector.Item("PARA_TIPO_ANULACION") = "3" Then
                    Me.RadioButtonAnulacionStandard.Checked = False
                    Me.RadioButtonAnulacionFactura.Checked = False
                    Me.RadioButtonAnulacionNotaCredito.Checked = True
                End If


                Me.TextBoxCtba1.Text = Me.DbLee.mDbLector.Item("PARA_BCTA1")
                Me.TextBoxCtba2.Text = Me.DbLee.mDbLector.Item("PARA_BCTA2")
                Me.TextBoxCtba3.Text = Me.DbLee.mDbLector.Item("PARA_BCTA3")
                Me.TextBoxCtba4.Text = Me.DbLee.mDbLector.Item("PARA_BCTA4")
                Me.TextBoxCtba5.Text = Me.DbLee.mDbLector.Item("PARA_BCTA5")
                Me.TextBoxCtba6.Text = Me.DbLee.mDbLector.Item("PARA_BCTA6")
                Me.TextBoxCtba7.Text = Me.DbLee.mDbLector.Item("PARA_BCTA7")
                Me.TextBoxCtba8.Text = Me.DbLee.mDbLector.Item("PARA_BCTA8")
                Me.TextBoxCtba9.Text = Me.DbLee.mDbLector.Item("PARA_BCTA9")
                Me.TextBoxCtba10.Text = Me.DbLee.mDbLector.Item("PARA_BCTA10")
                Me.TextBoxCtba11.Text = Me.DbLee.mDbLector.Item("PARA_BCTA11")
                Me.TextBoxCtba12.Text = Me.DbLee.mDbLector.Item("PARA_BCTA12")

                Me.TextBoxCentroCostoComisionesVisa.Text = Me.DbLee.mDbLector.Item("PARA_CENTRO_COSTO_COMI")


                Me.TextBoxAxSourceEndPoint.Text = Me.DbLee.mDbLector.Item("PARA_SOURCEENDPOINT")
                Me.TextBoxAxDestinationEndPoint.Text = Me.DbLee.mDbLector.Item("PARA_DESTINATIONENDPOINT")
                Me.TextBoxAxDomainName.Text = Me.DbLee.mDbLector.Item("PARA_DOMAIN_NAME")
                Me.TextBoxAxUserName.Text = Me.DbLee.mDbLector.Item("PARA_DOMAIN_USER")

                Me.TextBoxAxOffsetAccount.Text = Me.DbLee.mDbLector.Item("PARA_BANCO_AX")
                Me.TextBoxAxServicioBonos.Text = Me.DbLee.mDbLector.Item("PARA_SERV_CODI_BONO")
                Me.TextBoxAxServicioBonosAsociacion.Text = Me.DbLee.mDbLector.Item("PARA_SERV_CODI_BONOASC")

                Me.TextBoxAxWebServiceUrl.Text = Me.DbLee.mDbLector.Item("PARA_WEBSERVICE_LOCATION")
                Me.TextBoxAxAnticipoFormaCobro.Text = Me.DbLee.mDbLector.Item("PARA_ANTICIPO_AX")

                Me.NumericUpDownWebServiceTimeOut.Value = Me.DbLee.mDbLector.Item("PARA_WEBSERVICE_TIMEOUT")

                Me.TextBoxAxPalabraDePaso.Text = Me.DbLee.mDbLector.Item("PARA_DOMAIN_PWD")

                If Me.DbLee.mDbLector.Item("PARA_FILE_FORMAT") = "DOS" Then
                    Me.RadioButtonFormatoAsci.Checked = True
                    Me.RadioButtonFormatoAnsi.Checked = False
                Else
                    Me.RadioButtonFormatoAnsi.Checked = True
                    Me.RadioButtonFormatoAsci.Checked = False
                End If

                Me.TextBoxCodigoServicioAlojamiento.Text = Me.DbLee.mDbLector.Item("PARA_INGRESO_HABITACION_DPTO")

                If Me.DbLee.mDbLector.Item("PARA_DESGLO_ALOJA_REGIMEN") = "1" Then
                    Me.CheckBoxDesglosaAlojamientoporTipoRegimen.Checked = True
                Else
                    Me.CheckBoxDesglosaAlojamientoporTipoRegimen.Checked = False
                End If


                Me.TextBoxCtaAlc.Text = Me.DbLee.mDbLector.Item("PARA_CTA_CALO_AL")
                Me.TextBoxCtaAlv.Text = Me.DbLee.mDbLector.Item("PARA_CTA_VALO_AL")
                Me.TextBoxCtaAdc.Text = Me.DbLee.mDbLector.Item("PARA_CTA_CALO_AD")
                Me.TextBoxCtaAdv.Text = Me.DbLee.mDbLector.Item("PARA_CTA_VALO_AD")
                Me.TextBoxCtaMpc.Text = Me.DbLee.mDbLector.Item("PARA_CTA_CALO_MP")
                Me.TextBoxCtaMpv.Text = Me.DbLee.mDbLector.Item("PARA_CTA_VALO_MP")
                Me.TextBoxCtaPcc.Text = Me.DbLee.mDbLector.Item("PARA_CTA_CALO_PC")
                Me.TextBoxCtaPcv.Text = Me.DbLee.mDbLector.Item("PARA_CTA_VALO_PC")
                Me.TextBoxCtaTic.Text = Me.DbLee.mDbLector.Item("PARA_CTA_CALO_TI")
                Me.TextBoxCtaTiv.Text = Me.DbLee.mDbLector.Item("PARA_CTA_VALO_TI")
                Me.TextBoxCtaXc.Text = Me.DbLee.mDbLector.Item("PARA_CTA_CALO_X")
                Me.TextBoxCtaXv.Text = Me.DbLee.mDbLector.Item("PARA_CTA_VALO_X")

                Me.TextBox56DigitoCuentaClientes.Text = Me.DbLee.mDbLector.Item("PARA_CTA_56DIGITO")




                If Me.DbLee.mDbLector.Item("PARA_SERRUCHA_DPTO") = "1" Then
                    Me.CheckBoxSerrucha.Checked = True
                Else
                    Me.CheckBoxSerrucha.Checked = False
                End If


                Me.TextBoxCtaSerieCredito.Text = Me.DbLee.mDbLector.Item("PARA_CTA_SERIE_CRE")
                Me.TextBoxCtaSerieContado.Text = Me.DbLee.mDbLector.Item("PARA_CTA_SERIE_CON")
                Me.TextBoxCtaSerieNotasCredito.Text = Me.DbLee.mDbLector.Item("PARA_CTA_SERIE_NCRE")
                Me.TextBoxCtaSerieAnulacion.Text = Me.DbLee.mDbLector.Item("PARA_CTA_SERIE_ANUL")
                Me.TextBoxAxAnticipoTransferenciaAgencia.Text = Me.DbLee.mDbLector.Item("PARA_TRANSFERENCIA_AGENCIA")


                Me.TextBoxCftatodiariCod.Text = Me.DbLee.mDbLector.Item("PARA_CFATODIARI_COD")
                Me.TextBoxCftatodiariCod2.Text = Me.DbLee.mDbLector.Item("PARA_CFATODIARI_COD_2")


                If CInt(Me.DbLee.mDbLector.Item("PARA_TRATA_CAJA")) = 1 Then
                    Me.CheckBoxTrataAnticiposyDevoluciones.Checked = True
                Else
                    Me.CheckBoxTrataAnticiposyDevoluciones.Checked = False
                End If



                If Me.DbLee.mDbLector.Item("PARA_CONECTA_NEWGOLF") = "1" Then
                    Me.CheckBoxConectaNewGolf.Checked = True
                Else
                    Me.CheckBoxConectaNewGolf.Checked = False
                End If

                If Me.DbLee.mDbLector.Item("PARA_CONECTA_NEWPOS") = "1" Then
                    Me.CheckBoxConectaNewPos.Checked = True
                Else
                    Me.CheckBoxConectaNewPos.Checked = False
                End If

                Me.TextBoxArticuloAxAnulacionReserva.Text = Me.DbLee.mDbLector.Item("PARA_ART_ANULA_RESERVA")
                Me.TextBoxAxPrefijoMotasCreditoNewGolf.Text = Me.DbLee.mDbLector.Item("PARA_PREF_NCREG")
                Me.TextBoxParaPaso.Text = Me.DbLee.mDbLector.Item("PARA_PASO")

                If Me.DbLee.mDbLector.Item("PARA_VALIDA_SPYRO") = "1" Then
                    Me.CheckBoxValidaCuentasSpyro.Checked = True
                Else
                    Me.CheckBoxValidaCuentasSpyro.Checked = False
                End If

                If Me.DbLee.mDbLector.Item("PARA_USA_CTA4B") = "1" Then
                    Me.CheckBoxUsaCuentaDepositos.Checked = True
                Else
                    Me.CheckBoxUsaCuentaDepositos.Checked = False
                End If

                Me.TextBoxCtaDepositos.Text = Me.DbLee.mDbLector.Item("PARA_CTA4B")
                Me.TextBoxSeccionDepositosNh.Text = Me.DbLee.mDbLector.Item("PARA_SECC_DEPNH")
                Me.TextBoxSeccionAnticiposNh.Text = Me.DbLee.mDbLector.Item("PARA_SECC_ANTNH")
                Me.TextBoxCtaDepositosEfectivo.Text = Me.DbLee.mDbLector.Item("PARA_CTA4B2")
                Me.TextBoxCtaDepositosVisa.Text = Me.DbLee.mDbLector.Item("PARA_CTA4B3")
                Me.TextBoxAxAjustaBase.Text = Me.DbLee.mDbLector.Item("PARA_AX_AJUSTABASE")



                Me.TextBoxAxWebServiceUrl2.Text = Me.DbLee.mDbLector.Item("PARA_WEBSERVICE_LOCATION2")
                Me.TextBoxAxDomainName2.Text = Me.DbLee.mDbLector.Item("PARA_DOMAIN_NAME2")
                Me.TextBoxAxUserName2.Text = Me.DbLee.mDbLector.Item("PARA_DOMAIN_USER2")
                Me.TextBoxAxPalabraDePaso2.Text = Me.DbLee.mDbLector.Item("PARA_DOMAIN_PWD2")
                Me.NumericUpDownWebServiceTimeOut2.Value = Me.DbLee.mDbLector.Item("PARA_WEBSERVICE_TIMEOUT2")

                Me.TextBoxAxHotelId.Text = Me.DbLee.mDbLector.Item("PARA_HOTEL_AX")


                If Me.DbLee.mDbLector.Item("PARA_TRATA_TPV") = "1" Then
                    Me.CheckBoxExcluyeDebitoTpv.Checked = True
                Else
                    Me.CheckBoxExcluyeDebitoTpv.Checked = False
                End If


                Me.TextBoxFactuTipoCod.Text = Me.DbLee.mDbLector.Item("PARA_FACTUTIPO_COD")



                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_CFIVALIBRO_COD")) = True Then
                    Me.mParaCfIvaLibroCod = ""
                Else
                    Me.mParaCfIvaLibroCod = Me.DbLee.mDbLector.Item("PARA_CFIVALIBRO_COD")
                End If


                Me.TextBoxVitalSuplementoDesayuno.Text = Me.DbLee.mDbLector.Item("PARA_VITAL_SUPLEDESAYUNO")
                Me.TextBoxVitalCajaDebe.Text = Me.DbLee.mDbLector.Item("PARA_VITAL_DEBECAJA")
                Me.TextBoxVitalCajaHaber.Text = Me.DbLee.mDbLector.Item("PARA_VITAL_HABERCAJA")
                Me.TextBoxVitalForeEfectivo.Text = Me.DbLee.mDbLector.Item("PARA_VITAL_FOREEFECTIVO")

                Me.TextBoxWebServiceEmpName.Text = Me.DbLee.mDbLector.Item("PARA_WEBSERVICE_ENAME")

                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_MORA_PREPROD")) = False Then
                    Me.TextBoxTitoPrefijoProduccion.Text = Me.DbLee.mDbLector.Item("PARA_MORA_PREPROD")
                Else
                    Me.TextBoxTitoPrefijoProduccion.Text = ""
                End If


                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_MORA_DIMENHOTEL")) = False Then
                    Me.TextBoxTitoDimensionHotel.Text = Me.DbLee.mDbLector.Item("PARA_MORA_DIMENHOTEL")
                Else
                    Me.TextBoxTitoDimensionHotel.Text = ""
                End If

                Me.TextBoxTitoIgicNegocio.Text = Me.DbLee.mDbLector.Item("PARA_MORA_GRUPONEGOCIO")
                Me.TextBoxTitoIgicProducto.Text = Me.DbLee.mDbLector.Item("PARA_MORA_GRUPOPRODUCTO")


                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_VITAL_DPTO_GENERICO")) Then
                    Me.TextBoxVitalIdentificadorDepartemento.Text = ""
                Else
                    Me.TextBoxVitalIdentificadorDepartemento.Text = CStr(Me.DbLee.mDbLector.Item("PARA_VITAL_DPTO_GENERICO"))
                End If


                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_MORA_JOURNAL_TEMPLATE")) Then
                    Me.TextBoxJournalTemplate.Text = ""
                Else
                    Me.TextBoxJournalTemplate.Text = CStr(Me.DbLee.mDbLector.Item("PARA_MORA_JOURNAL_TEMPLATE"))
                End If


                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_MORA_JOURNAL_BATCH")) Then
                    Me.TextBoxJournalBatch.Text = ""
                Else
                    Me.TextBoxJournalBatch.Text = CStr(Me.DbLee.mDbLector.Item("PARA_MORA_JOURNAL_BATCH"))
                End If




                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_MORA_SUFI_ANTI")) Then
                    Me.TextBoxMoraSufijoAnticipos.Text = ""
                Else
                    Me.TextBoxMoraSufijoAnticipos.Text = CStr(Me.DbLee.mDbLector.Item("PARA_MORA_SUFI_ANTI"))
                End If



                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_MORA_SUFI_DEPO")) Then
                    Me.TextBoxMoraSufijoDepositos.Text = ""
                Else
                    Me.TextBoxMoraSufijoDepositos.Text = CStr(Me.DbLee.mDbLector.Item("PARA_MORA_SUFI_DEPO"))
                End If



                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_MORA_EQUIV_NAT")) Then
                    Me.TextBoxMoraEquivDimenNat.Text = "0"
                Else
                    Me.TextBoxMoraEquivDimenNat.Text = CStr(Me.DbLee.mDbLector.Item("PARA_MORA_EQUIV_NAT"))
                End If

                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_MORA_EQUIV_DEP")) Then
                    Me.TextBoxMoraEquivDimenDep.Text = "0"
                Else
                    Me.TextBoxMoraEquivDimenDep.Text = CStr(Me.DbLee.mDbLector.Item("PARA_MORA_EQUIV_DEP"))
                End If


                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_MORA_EQUIV_HOT")) Then
                    Me.TextBoxMoraEquivDimenHot.Text = "0"
                Else
                    Me.TextBoxMoraEquivDimenHot.Text = CStr(Me.DbLee.mDbLector.Item("PARA_MORA_EQUIV_HOT"))
                End If




                If CStr(Me.DbLee.mDbLector.Item("PARA_LOPEZ_TIPO_COMPROBANTES")) = "1" Then
                    Me.CheckBoxTipoComprobantes.Checked = True
                Else
                    Me.CheckBoxTipoComprobantes.Checked = False
                End If




                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_MORA_SOURCE_TYPE")) = True Then
                    Me.TextBoxMoraSourceType.Text = ""
                Else
                    Me.TextBoxMoraSourceType.Text = Me.DbLee.mDbLector.Item("PARA_MORA_SOURCE_TYPE")
                End If



                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_TEFECT_COD")) = True Then
                    Me.TextBoxTefect_Cod.Text = ""
                Else
                    Me.TextBoxTefect_Cod.Text = Me.DbLee.mDbLector.Item("PARA_TEFECT_COD")
                End If

                'trata de conectar con newgolf' 
                Try
                    If Me.DbLee.mDbLector.Item("PARA_CONECTA_NEWGOLF") = "1" Then
                        Dim StrConexion As String
                        SQL = "SELECT HOTEL_ODBC_NEWGOLF "
                        SQL += "  FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
                        SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
                        SQL += " AND HOTEL_EMP_NUM = " & Me.mParaEmpNum


                        Me.DbLeeAux.TraerLector(SQL)
                        Me.DbLeeAux.mDbLector.Read()
                        If Me.DbLeeAux.mDbLector.HasRows Then
                            If IsDBNull(Me.DbLeeAux.mDbLector.Item("HOTEL_ODBC_NEWGOLF")) = False Then
                                StrConexion = Me.DbLeeAux.mDbLector.Item("HOTEL_ODBC_NEWGOLF")
                                Me.DbNewGolf = New C_DATOS.C_DatosOledb
                                Me.DbNewGolf.StrConexion = StrConexion
                                Me.DbNewGolf.AbrirConexion()

                                SQL = "SELECT TIAD_DESC,TIAD_CODI FROM TNPL_TIAD "
                                Me.DbNewGolf.TraerDataset(SQL, "TIAD")

                                Dim NewRowA(0) As Object
                                NewRowA(0) = "<Ninguno>"

                                Me.DbNewGolf.mDbDataset.Tables("TIAD").LoadDataRow(NewRowA, False)
                                With Me.ComboBoxGrupoArticulosBonos
                                    .DataSource = Me.DbNewGolf.mDbDataset.Tables("TIAD")
                                    .ValueMember = "TIAD_CODI"
                                    .DisplayMember = "TIAD_DESC"
                                    '       .SelectedIndex = .Items.Count - 1
                                End With
                                Me.DbNewGolf.mDbDataset = Nothing
                            End If
                        End If
                        Me.DbLeeAux.mDbLector.Close()
                        Me.DbNewGolf.CerrarConexion()
                        Me.ComboBoxGrupoArticulosBonos.Enabled = True
                    Else
                        Me.ComboBoxGrupoArticulosBonos.DataSource = Nothing
                        Me.ComboBoxGrupoArticulosBonos.Items.Clear()
                        Me.ComboBoxGrupoArticulosBonos.Enabled = False
                    End If
                Catch ex As Exception
                    MsgBox(ex.Message, MsgBoxStyle.Information, "Mostrar Datos NewGolf")
                    MsgBox("No hay Conexión con alguna de las Bases de datos , se cierra el Formulario", MsgBoxStyle.Critical, "Atención")
                    Me.DbLee.mDbLector.Close()
                    Me.Close()
                End Try
            End If

            Me.DbLee.mDbLector.Close()

            ' PONER EN ROJO NULOS
            Dim MyControl As Control
            For Each MyControl In Me.TabPageGeneral.Controls
                If TypeOf MyControl Is TextBox Then
                    If MyControl.Text = "<Ninguno>" Then
                        MyControl.ForeColor = Color.Maroon
                        MyControl.Update()
                    Else
                        MyControl.ForeColor = Color.Black
                        MyControl.Update()
                    End If
                End If
            Next MyControl

        Catch EX As Exception
            Me.DbLee.mDbLector.Close()
            MsgBox(EX.Message, MsgBoxStyle.Information, "Mostrar Datos")
        End Try
    End Sub
    Private Sub MostrarDatosHoteles()
        Try
            SQL = "SELECT  NVL(HOTEL_DESCRIPCION,' ') HOTEL_DESCRIPCION, NVL(HOTEL_ODBC,' ') HOTEL_ODBC, NVL(HOTEL_SPYRO,' ') HOTEL_SPYRO, NVL(HOTEL_ODBC_ALMACEN, ' ') HOTEL_ODBC_ALMACEN, "
            SQL += " NVL(HOTEL_ODBC_NEWCONTA,' ') HOTEL_ODBC_NEWCONTA,  HOTEL_ESTABLECIMIENTO_NEWCONTA,"
            SQL += " NVL(HOTEL_ODBC_NEWGOLF,' ') HOTEL_ODBC_NEWGOLF ,"
            SQL += " NVL(HOTEL_ODBC_NEWPOS,' ') HOTEL_ODBC_NEWPOS,"
            SQL += " NVL(HOTEL_ODBC_NEWPAGA,' ') HOTEL_ODBC_NEWPAGA,"
            SQL += " NVL(HOTEL_ODBC_NEWCENTRAL,' ') HOTEL_ODBC_NEWCENTRAL,"
            SQL += " NVL(HOTEL_HOTE_CODI,0) HOTEL_HOTE_CODI,"

            SQL += " NVL(HOTEL_INT_NEWH,0) HOTEL_INT_NEWH,"
            SQL += " NVL(HOTEL_INT_NEWC,0) HOTEL_INT_NEWC,"
            SQL += " NVL(HOTEL_INT_NEWS,0) HOTEL_INT_NEWS,"
            SQL += " NVL(HOTEL_INT_NOMI,0) HOTEL_INT_NOMI,"
            SQL += " NVL(HOTEL_EMPRESA_PATH,' ') HOTEL_EMPRESA_PATH"




            SQL += "  FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
            SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
            SQL += " AND HOTEL_EMP_NUM = " & Me.mParaEmpNum



            Me.DbLee.TraerLector(SQL)

            While Me.DbLee.mDbLector.Read
                Me.TextBoxHotelDescripcion.Text = Me.DbLee.mDbLector.Item("HOTEL_DESCRIPCION")
                Me.TextBoxHotelOdbc.Text = Me.DbLee.mDbLector.Item("HOTEL_ODBC")
                Me.TextBoxHotelSpyro.Text = Me.DbLee.mDbLector.Item("HOTEL_SPYRO")
                Me.TextBoxHotelOdbcAlmacen.Text = Me.DbLee.mDbLector.Item("HOTEL_ODBC_ALMACEN")
                Me.TextBoxHotelOdbcNewConta.Text = Me.DbLee.mDbLector.Item("HOTEL_ODBC_NEWCONTA")
                Me.TextBoxNewContaEstablecimientoNewConta.Text = Me.DbLee.mDbLector.Item("HOTEL_ESTABLECIMIENTO_NEWCONTA")
                Me.TextBoxHotelOdbcNewGolf.Text = Me.DbLee.mDbLector.Item("HOTEL_ODBC_NEWGOLF")
                Me.TextBoxHotelOdbcNewPos.Text = Me.DbLee.mDbLector.Item("HOTEL_ODBC_NEWPOS")
                Me.TextBoxHotelOdbcNewPaga.Text = Me.DbLee.mDbLector.Item("HOTEL_ODBC_NEWPAGA")
                Me.TextBoxOdbcNewCentral.Text = Me.DbLee.mDbLector.Item("HOTEL_ODBC_NEWCENTRAL")
                Me.TextBoxNewContaCodigoHotelNewCentral.Text = Me.DbLee.mDbLector.Item("HOTEL_HOTE_CODI")


                If CInt(Me.DbLee.mDbLector.Item("HOTEL_INT_NEWH")) = 1 Then
                    Me.CheckBoxInterfazNewHotel.Checked = True
                Else
                    Me.CheckBoxInterfazNewHotel.Checked = False
                End If

                If CInt(Me.DbLee.mDbLector.Item("HOTEL_INT_NEWC")) = 1 Then
                    Me.CheckBoxInterfazNewConta.Checked = True
                Else
                    Me.CheckBoxInterfazNewConta.Checked = False
                End If

                If CInt(Me.DbLee.mDbLector.Item("HOTEL_INT_NEWS")) = 1 Then
                    Me.CheckBoxInterfazNewStock.Checked = True
                Else
                    Me.CheckBoxInterfazNewStock.Checked = False
                End If

                If CInt(Me.DbLee.mDbLector.Item("HOTEL_INT_NOMI")) = 1 Then
                    Me.CheckBoxInterfazNomina.Checked = True
                Else
                    Me.CheckBoxInterfazNomina.Checked = False
                End If


                Me.TextBoxRutaEmpresaContanet.Text = Me.DbLee.mDbLector.Item("HOTEL_EMPRESA_PATH")

            End While
            Me.DbLee.mDbLector.Close()

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Mostrar Datos Hoteles")
        End Try
    End Sub
    Private Sub MostrarDatosNewConta()
        Try
            SQL = "SELECT  PARA_ORIGENCUENTAS,NVL(PARA_ESTABLECIMIENTO,'<Ninguno>') AS PARA_ESTABLECIMIENTO ,PARA_TRATA_ANULACIONES,PARA_CFBCOTMOV_COD,PARA_CFBCOTMOV_COD2,PARA_FPAGO_COD,PARA_FPAGO_COD2  "
            SQL += " ,PARA_BANCOS_COD,PARA_BANCOS_COD2 "
            SQL += "  FROM TC_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mParaEmpNum




            Me.DbLee.TraerLector(SQL)

            While Me.DbLee.mDbLector.Read

                '    Me.TextBoxNewContaEstablecimientoNewConta.Text = Me.DbLee.mDbLector.Item("PARA_ESTABLECIMIENTO")
                ' ESTE CAMPO ESTA EN TH_PARA OJO FALTA DESARROLLO



                If Me.DbLee.mDbLector.Item("PARA_ORIGENCUENTAS") = "0" Then
                    Me.CheckBoxNewContaOrigenCuentaNewConta.Checked = True
                    Me.CheckBoxNewContaOrigenCuentaNewCentral.Checked = False
                Else
                    Me.CheckBoxNewContaOrigenCuentaNewConta.Checked = False
                    Me.CheckBoxNewContaOrigenCuentaNewCentral.Checked = True
                End If

                If Me.DbLee.mDbLector.Item("PARA_TRATA_ANULACIONES") = "0" Then
                    Me.CheckBoxNewContaTrataAnulados.Checked = False

                Else
                    Me.CheckBoxNewContaTrataAnulados.Checked = True

                End If

                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_CFBCOTMOV_COD")) = False Then
                    Me.TextBoxNewContaTipoMovBanc.Text = CStr(Me.DbLee.mDbLector.Item("PARA_CFBCOTMOV_COD"))
                Else
                    Me.TextBoxNewContaTipoMovBanc.Text = ""
                End If


                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_CFBCOTMOV_COD2")) = False Then
                    Me.TextBoxNewContaTipoMovBanc2.Text = CStr(Me.DbLee.mDbLector.Item("PARA_CFBCOTMOV_COD2"))
                Else
                    Me.TextBoxNewContaTipoMovBanc2.Text = ""
                End If



                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_FPAGO_COD")) = False Then
                    Me.TextBoxNewContaFPagoBanc.Text = CStr(Me.DbLee.mDbLector.Item("PARA_FPAGO_COD"))
                Else
                    Me.TextBoxNewContaFPagoBanc.Text = ""
                End If


                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_FPAGO_COD2")) = False Then
                    Me.TextBoxNewContaFPagoBanc2.Text = CStr(Me.DbLee.mDbLector.Item("PARA_FPAGO_COD2"))
                Else
                    Me.TextBoxNewContaFPagoBanc2.Text = ""
                End If

                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_BANCOS_COD2")) = False Then
                    Me.TextBoxNewContaBanco2.Text = CStr(Me.DbLee.mDbLector.Item("PARA_BANCOS_COD2"))
                Else
                    Me.TextBoxNewContaBanco2.Text = ""
                End If

                If IsDBNull(Me.DbLee.mDbLector.Item("PARA_BANCOS_COD")) = False Then
                    Me.TextBoxNewContaBanco.Text = CStr(Me.DbLee.mDbLector.Item("PARA_BANCOS_COD"))
                Else
                    Me.TextBoxNewContaBanco.Text = ""
                End If


            End While
            Me.DbLee.mDbLector.Close()

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Mostrar Datos NewConta")
        End Try
    End Sub
    Private Sub MuestraSeccionesNewhotel()
        Try
            If Me.CheckBoxInterfazNewHotel.Checked = True Then
                Dim StrHotel As String
                Dim Result As String = ""
                SQL = "SELECT HOTEL_ODBC FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
                SQL += " AND HOTEL_EMP_NUM = " & Me.TextBoxEmpNum.Text
                StrHotel = CStr(Me.DbLeeAux.EjecutaSqlScalar(SQL))

                If StrHotel <> "" Then
                    Dim DbNh As New C_DATOS.C_DatosOledb
                    DbNh.StrConexion = StrHotel
                    DbNh.AbrirConexion()
                    SQL = "SELECT SECC_CODI , SECC_DESC FROM TNHT_SECC ORDER BY SECC_DESC"
                    DbNh.TraerLector(SQL)
                    While DbNh.mDbLector.Read
                        Result += DbNh.mDbLector.Item("SECC_CODI") & " = " & DbNh.mDbLector.Item("SECC_DESC") & vbCrLf
                    End While
                    DbNh.mDbLector.Close()
                    DbNh.CerrarConexion()
                    DbNh = Nothing
                    MsgBox(Result)
                End If

            Else
                MsgBox("No existe Interfaz Odbc con el Hotel", MsgBoxStyle.Information, "Atención")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ActualizaTH_PARA()
        Try
            ' VALIDACIONES
            If Me.CheckBoxUsaCuentaDepositos.Checked Then
                If Me.TextBoxCtaDepositos.Text.Length = 0 Or Me.TextBoxSeccionDepositosNh.Text.Length = 0 Or Me.TextBoxCtaDepositos.Text = "<Ninguno>" Or Me.TextBoxSeccionDepositosNh.Text = "<Ninguno>" Then
                    MsgBox("Si escoge  ""Usa Cuenta Independiente para Depósitos y Anticipos""" & vbCrLf & vbCrLf & " Los Campos ""Cuenta Depósitos Pdtes. de Facturar""" & " y ""Sección Depósitos NewHotel""" & vbCrLf & vbCrLf & "NO pueden estar Vacíos", MsgBoxStyle.Information, "Atención")
                    Me.TextBoxSeccionDepositosNh.BackColor = Color.Maroon
                    Me.TextBoxCtaDepositos.BackColor = Color.Maroon
                    Me.TextBoxSeccionDepositosNh.Focus()
                    Me.Update()
                End If
            End If


            SQL = "UPDATE TH_PARA "
            SQL += "SET PARA_CTA1 ='" & Me.TextBoxCtaManoCorriente.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA4 ='" & Me.TextBoxCtaPagosaCuenta.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA5 ='" & Me.TextBoxCtaDesembolsos.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_DEBE ='" & Me.TextBoxIndicadorDebe.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_HABER ='" & Me.TextBoxIndicadorHaber.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_DEBE_FAC ='" & Me.TextBoxIndicadorDebeFacturas.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_HABER_FAC ='" & Me.TextBoxIndicadorHaberFacturas.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CLIENTES_CONTADO ='" & Me.TextBoxCuentaClientesContado.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CLIENTES_CONTADO_CIF ='" & Me.TextBoxCifClientesContado.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_FILE_SPYRO_PATH ='" & Me.TextBoxFileSpyroPath.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_REDONDEO ='" & Me.TextBoxCuentaAjusteRedondeo.Text.Replace("<Ninguno>", "") & "'"
            If Me.RadioButtonFechaRegistroAc.Checked Then
                SQL += ",PARA_FECHA_REGISTRO_AC ='R'"
            Else
                SQL += ",PARA_FECHA_REGISTRO_AC ='V'"
            End If

            SQL += ",PARA_TEXTO_IVA ='" & Me.TextBoxDenominacionImpuesto.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_SERIE_ANULACION ='" & Me.TextBoxSerieFacturasAnuladas.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CENTRO_COSTO_AL ='" & Me.TextBoxCentroCostoComisionesAlojamiento.Text.Replace("<Ninguno>", "") & "'"
            If CheckBoxComisionAfectaImpuesto.Checked Then
                SQL += ",PARA_COMISIONES= 1"
            Else
                SQL += ",PARA_COMISIONES= 0"
            End If
            SQL += ",PARA_CLIENTES_CONTADO_CODIGO ='" & Me.TextBoxCodigoClientesContado.Text.Replace("<Ninguno>", "") & "'"

            If Me.CheckBoxIngresosServicioBloque.Checked Then
                SQL += ",PARA_INGRESO_POR_HABITACION = 1 "
            Else
                SQL += ",PARA_INGRESO_POR_HABITACION = 0"
            End If

            If Me.RadioButtonAnulacionStandard.Checked Then
                SQL += ",PARA_TIPO_ANULACION = 1"
            ElseIf Me.RadioButtonAnulacionFactura.Checked Then
                SQL += ",PARA_TIPO_ANULACION = 2"
            ElseIf Me.RadioButtonAnulacionNotaCredito.Checked Then
                SQL += ",PARA_TIPO_ANULACION = 3"
            End If

            SQL += ",PARA_BCTA1 ='" & Me.TextBoxCtba1.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_BCTA2 ='" & Me.TextBoxCtba2.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_BCTA3 ='" & Me.TextBoxCtba3.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_BCTA4 ='" & Me.TextBoxCtba4.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_BCTA5 ='" & Me.TextBoxCtba5.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_BCTA6 ='" & Me.TextBoxCtba6.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_BCTA7 ='" & Me.TextBoxCtba7.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_BCTA8 ='" & Me.TextBoxCtba8.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_BCTA9 ='" & Me.TextBoxCtba9.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_BCTA10 ='" & Me.TextBoxCtba10.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_BCTA11 ='" & Me.TextBoxCtba11.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_BCTA12 ='" & Me.TextBoxCtba12.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CENTRO_COSTO_COMI ='" & Me.TextBoxCentroCostoComisionesVisa.Text.Replace("<Ninguno>", "") & "'"

            SQL += ",PARA_SOURCEENDPOINT ='" & Me.TextBoxAxSourceEndPoint.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_DESTINATIONENDPOINT ='" & Me.TextBoxAxDestinationEndPoint.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_DOMAIN_NAME ='" & Me.TextBoxAxDomainName.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_DOMAIN_USER ='" & Me.TextBoxAxUserName.Text.Replace("<Ninguno>", "") & "'"


            SQL += ",PARA_BANCO_AX='" & Me.TextBoxAxOffsetAccount.Text.Replace("<Ninguno>", "") & "'"

            SQL += ",PARA_SERV_CODI_BONO='" & Me.TextBoxAxServicioBonos.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_SERV_CODI_BONOASC='" & Me.TextBoxAxServicioBonosAsociacion.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_WEBSERVICE_LOCATION='" & Me.TextBoxAxWebServiceUrl.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_ANTICIPO_AX='" & Me.TextBoxAxAnticipoFormaCobro.Text.Replace("<Ninguno>", "") & "'"

            SQL += ",PARA_WEBSERVICE_TIMEOUT = " & Me.NumericUpDownWebServiceTimeOut.Value

            SQL += ",PARA_DOMAIN_PWD='" & Me.TextBoxAxPalabraDePaso.Text.Replace("<Ninguno>", "") & "'"


            If Me.RadioButtonFormatoAsci.Checked Then
                SQL += ",PARA_FILE_FORMAT= 'DOS'"
            End If

            If Me.RadioButtonFormatoAnsi.Checked Then
                SQL += ",PARA_FILE_FORMAT= 'WINDOWS'"
            End If

            SQL += ",PARA_INGRESO_HABITACION_DPTO='" & Me.TextBoxCodigoServicioAlojamiento.Text.Replace("<Ninguno>", "") & "'"

            If Me.CheckBoxDesglosaAlojamientoporTipoRegimen.Checked Then
                SQL += ",PARA_DESGLO_ALOJA_REGIMEN = 1 "
            Else
                SQL += ",PARA_DESGLO_ALOJA_REGIMEN = 0"
            End If

            SQL += ",PARA_CTA_CALO_AL='" & Me.TextBoxCtaAlc.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_VALO_AL='" & Me.TextBoxCtaAlv.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_CALO_AD='" & Me.TextBoxCtaAdc.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_VALO_AD='" & Me.TextBoxCtaAdv.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_CALO_MP='" & Me.TextBoxCtaMpc.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_VALO_MP='" & Me.TextBoxCtaMpv.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_CALO_PC='" & Me.TextBoxCtaPcc.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_VALO_PC='" & Me.TextBoxCtaPcv.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_CALO_TI='" & Me.TextBoxCtaTic.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_VALO_TI='" & Me.TextBoxCtaTiv.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_CALO_X='" & Me.TextBoxCtaXc.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_VALO_X='" & Me.TextBoxCtaXv.Text.Replace("<Ninguno>", "") & "'"


            If CheckBoxSerrucha.Checked Then
                SQL += ",PARA_SERRUCHA_DPTO = 1"
            Else
                SQL += ",PARA_SERRUCHA_DPTO = 0"
            End If


            SQL += ",PARA_CTA_SERIE_CRE='" & Me.TextBoxCtaSerieCredito.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_SERIE_CON='" & Me.TextBoxCtaSerieContado.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_SERIE_NCRE='" & Me.TextBoxCtaSerieNotasCredito.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA_SERIE_ANUL='" & Me.TextBoxCtaSerieAnulacion.Text.Replace("<Ninguno>", "") & "'"


            SQL += ",PARA_CTA_56DIGITO='" & Me.TextBox56DigitoCuentaClientes.Text.Replace("<Ninguno>", "") & "'"

            SQL += ",PARA_TRANSFERENCIA_AGENCIA='" & Me.TextBoxAxAnticipoTransferenciaAgencia.Text.Replace("<Ninguno>", "") & "'"


            If Me.CheckBoxConectaNewGolf.Checked Then
                SQL += ",PARA_CONECTA_NEWGOLF = 1 "
            Else
                SQL += ",PARA_CONECTA_NEWGOLF = 0 "
            End If

            If Me.CheckBoxConectaNewPos.Checked Then
                SQL += ",PARA_CONECTA_NEWPOS = 1 "
            Else
                SQL += ",PARA_CONECTA_NEWPOS = 0 "
            End If


            SQL += ",PARA_ART_ANULA_RESERVA='" & Me.TextBoxArticuloAxAnulacionReserva.Text.Replace("<Ninguno>", "") & "'"

            SQL += ",PARA_CFATODIARI_COD ='" & Me.TextBoxCftatodiariCod.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CFATODIARI_COD_2 ='" & Me.TextBoxCftatodiariCod2.Text.Replace("<Ninguno>", "") & "'"


            If Me.CheckBoxTrataAnticiposyDevoluciones.Checked Then
                SQL += ",PARA_TRATA_CAJA = 1 "
            Else
                SQL += ",PARA_TRATA_CAJA = 0"
            End If


            SQL += ",PARA_PREF_NCREG ='" & Me.TextBoxAxPrefijoMotasCreditoNewGolf.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_PASO ='" & Me.TextBoxParaPaso.Text.Replace("<Ninguno>", "") & "'"


            If CheckBoxValidaCuentasSpyro.Checked Then
                SQL += ",PARA_VALIDA_SPYRO = 1"
            Else
                SQL += ",PARA_VALIDA_SPYRO = 0"
            End If


            If CheckBoxUsaCuentaDepositos.Checked Then
                SQL += ",PARA_USA_CTA4B = 1"
            Else
                SQL += ",PARA_USA_CTA4B = 0"
            End If

            SQL += ",PARA_CTA4B ='" & Me.TextBoxCtaDepositos.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_SECC_DEPNH ='" & Me.TextBoxSeccionDepositosNh.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_SECC_ANTNH ='" & Me.TextBoxSeccionAnticiposNh.Text.Replace("<Ninguno>", "") & "'"

            SQL += ",PARA_CTA4B2 ='" & Me.TextBoxCtaDepositosEfectivo.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_CTA4B3 ='" & Me.TextBoxCtaDepositosVisa.Text.Replace("<Ninguno>", "") & "'"

            SQL += ",PARA_AX_AJUSTABASE ='" & Me.TextBoxAxAjustaBase.Text.Replace("<Ninguno>", "") & "'"



            SQL += ",PARA_WEBSERVICE_LOCATION2='" & Me.TextBoxAxWebServiceUrl2.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_DOMAIN_NAME2 ='" & Me.TextBoxAxDomainName2.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_DOMAIN_USER2 ='" & Me.TextBoxAxUserName2.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_DOMAIN_PWD2='" & Me.TextBoxAxPalabraDePaso2.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_WEBSERVICE_TIMEOUT2 = " & Me.NumericUpDownWebServiceTimeOut2.Value



            '   SQL += ",PARA_HOTEL_AX ='" & Me.TextBoxAxHotelId.Text.Replace("<Ninguno>", "") & "'"

            If Me.CheckBoxExcluyeDebitoTpv.Checked Then
                SQL += ",PARA_TRATA_TPV= 1 "
            Else
                SQL += ",PARA_TRATA_TPV = 0"
            End If

            SQL += ",PARA_FACTUTIPO_COD ='" & Me.TextBoxFactuTipoCod.Text.Replace("<Ninguno>", "") & "'"


            SQL += ",PARA_VITAL_SUPLEDESAYUNO ='" & Me.TextBoxVitalSuplementoDesayuno.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_VITAL_DEBECAJA ='" & Me.TextBoxVitalCajaDebe.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_VITAL_HABERCAJA ='" & Me.TextBoxVitalCajaHaber.Text.Replace("<Ninguno>", "") & "'"

            SQL += ",PARA_VITAL_FOREEFECTIVO ='" & Me.TextBoxVitalForeEfectivo.Text.Replace("<Ninguno>", "") & "'"

            SQL += ",PARA_WEBSERVICE_ENAME ='" & Me.TextBoxWebServiceEmpName.Text.Replace("<Ninguno>", "") & "'"


            If Me.TextBoxTitoPrefijoProduccion.TextLength > 0 Then
                SQL += ",PARA_MORA_PREPROD ='" & Me.TextBoxTitoPrefijoProduccion.Text & "'"
            Else
                SQL += ",PARA_MORA_PREPROD = NULL"
            End If

            If Me.TextBoxTitoDimensionHotel.TextLength > 0 Then
                SQL += ",PARA_MORA_DIMENHOTEL ='" & Me.TextBoxTitoDimensionHotel.Text & "'"
            Else
                SQL += ",PARA_MORA_DIMENHOTEL = NULL"
            End If



            SQL += ",PARA_MORA_GRUPONEGOCIO ='" & Me.TextBoxTitoIgicNegocio.Text.Replace("<Ninguno>", "") & "'"
            SQL += ",PARA_MORA_GRUPOPRODUCTO ='" & Me.TextBoxTitoIgicProducto.Text.Replace("<Ninguno>", "") & "'"

            If Me.TextBoxVitalIdentificadorDepartemento.TextLength > 0 Then
                SQL += ",PARA_VITAL_DPTO_GENERICO ='" & Me.TextBoxVitalIdentificadorDepartemento.Text & "'"
            Else
                SQL += ",PARA_VITAL_DPTO_GENERICO = NULL"
            End If



            If Me.TextBoxJournalTemplate.TextLength > 0 Then
                SQL += ",PARA_MORA_JOURNAL_TEMPLATE ='" & Me.TextBoxJournalTemplate.Text & "'"
            Else
                SQL += ",PARA_MORA_JOURNAL_TEMPLATE = NULL"
            End If


            If Me.TextBoxJournalBatch.TextLength > 0 Then
                SQL += ",PARA_MORA_JOURNAL_BATCH ='" & Me.TextBoxJournalBatch.Text & "'"
            Else
                SQL += ",PARA_MORA_JOURNAL_BATCH = NULL"
            End If




            If Me.TextBoxMoraSufijoAnticipos.TextLength > 0 Then
                SQL += ",PARA_MORA_SUFI_ANTI ='" & Me.TextBoxMoraSufijoAnticipos.Text & "'"
            Else
                SQL += ",PARA_MORA_SUFI_ANTI = NULL"
            End If


            If Me.TextBoxMoraSufijoDepositos.TextLength > 0 Then
                SQL += ",PARA_MORA_SUFI_DEPO ='" & Me.TextBoxMoraSufijoDepositos.Text & "'"
            Else
                SQL += ",PARA_MORA_SUFI_DEPO = NULL"
            End If



            If Me.TextBoxMoraEquivDimenNat.TextLength > 0 Then
                SQL += ",PARA_MORA_EQUIV_NAT = " & Me.TextBoxMoraEquivDimenNat.Text
            Else
                SQL += ",PARA_MORA_EQUIV_NAT = NULL"
            End If


            If Me.TextBoxMoraEquivDimenDep.TextLength > 0 Then
                SQL += ",PARA_MORA_EQUIV_DEP = " & Me.TextBoxMoraEquivDimenDep.Text
            Else
                SQL += ",PARA_MORA_EQUIV_DEP = NULL"
            End If

            If Me.TextBoxMoraEquivDimenHot.TextLength > 0 Then
                SQL += ",PARA_MORA_EQUIV_HOT = " & Me.TextBoxMoraEquivDimenHot.Text
            Else
                SQL += ",PARA_MORA_EQUIV_HOT = NULL"
            End If


            If Me.CheckBoxTipoComprobantes.Checked Then
                SQL += ",PARA_LOPEZ_TIPO_COMPROBANTES = 1 "
            Else
                SQL += ",PARA_LOPEZ_TIPO_COMPROBANTES = 0 "
            End If


            If Me.TextBoxMoraSourceType.TextLength > 0 Then
                SQL += ",PARA_MORA_SOURCE_TYPE = " & "'" & Me.TextBoxMoraSourceType.Text & "'"
            Else
                SQL += ",PARA_MORA_SOURCE_TYPE = NULL"
            End If


            If Me.TextBoxTefect_Cod.TextLength > 0 Then
                SQL += ",PARA_TEFECT_COD = " & "'" & Me.TextBoxTefect_Cod.Text & "'"
            Else
                SQL += ",PARA_TEFECT_COD = NULL"
            End If

            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mParaEmpNum


            ' UPDATE
            Me.Cursor = Cursors.WaitCursor
            Me.DbWrite.EjecutaSqlCommit(SQL)
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Update TH_PARA")

        End Try
    End Sub
    Private Sub ActualizaTH_HOTEL()
        Try
            SQL = "UPDATE TH_HOTEL "
            SQL += "SET "
            '    SQL += " HOTEL_DESCRIPCION ='" & Me.TextBoxHotelDescripcion.Text & "'"
            SQL += " HOTEL_ODBC ='" & Me.TextBoxHotelOdbc.Text.Trim & "'"
            SQL += ",HOTEL_SPYRO ='" & Me.TextBoxHotelSpyro.Text.Trim & "'"
            SQL += ",HOTEL_ODBC_ALMACEN ='" & Me.TextBoxHotelOdbcAlmacen.Text.Trim & "'"
            SQL += ",HOTEL_ODBC_NEWCONTA='" & Me.TextBoxHotelOdbcNewConta.Text.Trim & "'"
            SQL += ",HOTEL_ESTABLECIMIENTO_NEWCONTA ='" & Me.TextBoxNewContaEstablecimientoNewConta.Text & "'"
            SQL += ",HOTEL_ODBC_NEWGOLF ='" & Me.TextBoxHotelOdbcNewGolf.Text.Trim & "'"
            SQL += ",HOTEL_ODBC_NEWPOS ='" & Me.TextBoxHotelOdbcNewPos.Text.Trim & "'"
            SQL += ",HOTEL_ODBC_NEWPAGA ='" & Me.TextBoxHotelOdbcNewPaga.Text.Trim & "'"
            SQL += ",HOTEL_ODBC_NEWCENTRAL ='" & Me.TextBoxOdbcNewCentral.Text.Trim & "'"
            SQL += ",HOTEL_HOTE_CODI =" & Me.TextBoxNewContaCodigoHotelNewCentral.Text

            SQL += ",HOTEL_INT_NEWH =" & Me.CheckBoxInterfazNewHotel.CheckState
            SQL += ",HOTEL_INT_NEWC =" & Me.CheckBoxInterfazNewConta.CheckState
            SQL += ",HOTEL_INT_NEWS =" & Me.CheckBoxInterfazNewStock.CheckState
            SQL += ",HOTEL_INT_NOMI =" & Me.CheckBoxInterfazNomina.CheckState

            SQL += ", HOTEL_EMPRESA_PATH = '" & Me.TextBoxRutaEmpresaContanet.Text.Trim & "'"



            SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
            SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
            SQL += " AND HOTEL_EMP_NUM = " & Me.mParaEmpNum


            ' UPDATE
            Me.Cursor = Cursors.WaitCursor
            Me.DbWrite.EjecutaSqlCommit(SQL)
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Update TH_HOTEL")
        End Try
    End Sub
    Private Sub ActualizaTC_PARA()
        Try

            Dim Contador As Integer

            SQL = "SELECT NVL(COUNT(*),0) AS TOTAL FROM TC_PARA"
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"

            Contador = Me.DbWrite.EjecutaSqlScalar(SQL)
            If Contador = 0 Then Exit Try


            SQL = "UPDATE TC_PARA "
            SQL += "SET "

            If Me.CheckBoxNewContaOrigenCuentaNewConta.Checked Then
                SQL += "PARA_ORIGENCUENTAS = 0"
            ElseIf Me.CheckBoxNewContaOrigenCuentaNewCentral.Checked Then
                SQL += "PARA_ORIGENCUENTAS = 1"
            End If



            If Me.CheckBoxNewContaTrataAnulados.Checked Then
                SQL += " , PARA_TRATA_ANULACIONES = 1"
            Else
                SQL += ", PARA_TRATA_ANULACIONES = 0"
            End If


            If Me.TextBoxNewContaTipoMovBanc.TextLength > 0 Then
                SQL += " ,PARA_CFBCOTMOV_COD = '" & Me.TextBoxNewContaTipoMovBanc.Text & "'"
            Else
                SQL += ", PARA_CFBCOTMOV_COD = ''"
            End If


            If Me.TextBoxNewContaTipoMovBanc2.TextLength > 0 Then
                SQL += " ,PARA_CFBCOTMOV_COD2 = '" & Me.TextBoxNewContaTipoMovBanc2.Text & "'"
            Else
                SQL += ", PARA_CFBCOTMOV_COD2 = ''"
            End If




            If Me.TextBoxNewContaFPagoBanc.TextLength > 0 Then
                SQL += " ,PARA_FPAGO_COD = '" & Me.TextBoxNewContaFPagoBanc.Text & "'"
            Else
                SQL += ",PARA_FPAGO_COD = ''"
            End If


            If Me.TextBoxNewContaFPagoBanc2.TextLength > 0 Then
                SQL += " ,PARA_FPAGO_COD2 = '" & Me.TextBoxNewContaFPagoBanc2.Text & "'"
            Else
                SQL += ", PARA_FPAGO_COD2 = ''"
            End If



            If Me.TextBoxNewContaBanco.TextLength > 0 Then
                SQL += " ,PARA_BANCOS_COD = '" & Me.TextBoxNewContaBanco.Text & "'"
            Else
                SQL += ",PARA_BANCOS_COD = ''"
            End If

            If Me.TextBoxNewContaBanco2.TextLength > 0 Then
                SQL += " ,PARA_BANCOS_COD2 = '" & Me.TextBoxNewContaBanco2.Text & "'"
            Else
                SQL += ",PARA_BANCOS_COD2 = ''"
            End If


            SQL += " ,PARA_ESTABLECIMIENTO = '" & Me.TextBoxNewContaEstablecimientoNewConta.Text & "'"

            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mParaEmpNum


            ' UPDATE
            Me.Cursor = Cursors.WaitCursor
            Me.DbWrite.EjecutaSqlCommit(SQL)
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Update TC_PARA")
        End Try
    End Sub
#End Region

    Private Sub LimpiarControles()
        Dim C As Control
        For Each C In Me.Controls
            If TypeOf C Is TextBox Then
                C.Text = ""
                C.BackColor = Color.White
                C.Update()
            End If
        Next

        For Each C In Me.TabPageGeneral.Controls
            If TypeOf C Is TextBox Then
                C.Text = ""
                C.BackColor = Color.White
                C.Update()
            End If
        Next

        For Each C In Me.TabPageHotelesLopez.Controls
            If TypeOf C Is TextBox Then
                C.Text = ""
                C.BackColor = Color.White
                C.Update()
            End If
        Next
    End Sub

    Private Sub ButtonPathDestinoFicheros_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPathDestinoFicheros.Click
        Me.FolderBrowserDialog1.ShowDialog()
        If IsNothing(Me.FolderBrowserDialog1.SelectedPath) = False Then
            If Me.FolderBrowserDialog1.SelectedPath.Length > 0 Then
                Me.TextBoxFileSpyroPath.Text = Me.FolderBrowserDialog1.SelectedPath & "\"
            End If
        End If
    End Sub

    Private Sub RadioButtonFechaRegistroAc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonFechaRegistroAc.CheckedChanged
        If Me.RadioButtonFechaRegistroAc.Checked Then
            Me.RadioButtonFechaValorAc.Checked = False
        End If
    End Sub

    Private Sub RadioButtonFechaValorAc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonFechaValorAc.CheckedChanged
        If Me.RadioButtonFechaValorAc.Checked Then
            Me.RadioButtonFechaRegistroAc.Checked = False
        End If
    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ActualizaTH_PARA()
            Me.ActualizaTC_PARA()
            If Me.TextBoxHotelDescripcion.Text.Length > 0 Then
                Me.ActualizaTH_HOTEL()
            Else
                MsgBox("La Descipción del Hotel , en los parámetros Odbc no puede ser nula", MsgBoxStyle.Information, "Atención")
            End If

            Me.Cursor = Cursors.Default
            '   Me.Close()
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub ComboBoxEmpCod_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxEmpCod.SelectionChangeCommitted
        Try



            Me.Cursor = Cursors.WaitCursor
            Me.LimpiarControles()

            ' Mostrar Emp_cod
            SQL = "SELECT HOTEL_EMP_COD FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
            SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
            SQL += " AND HOTEL_DESCRIPCION = '" & Me.ComboBoxEmpCod.Text & "'"
            Me.mParaEmpCod = CStr(Me.DbLeeAux.EjecutaSqlScalar(SQL))
            Me.TextBoxEmpCod.Text = Me.mParaEmpCod


            ' averiguar numero de empresa
            SQL = "SELECT HOTEL_EMP_NUM FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
            SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
            SQL += " AND HOTEL_DESCRIPCION = '" & Me.ComboBoxEmpCod.Text & "'"
            Me.mParaEmpNum = CInt(Me.DbLeeAux.EjecutaSqlScalar(SQL))
            Me.TextBoxEmpNum.Text = Me.mParaEmpNum

            Me.MostrarDatosParametros()
            Me.MostrarDatosHoteles()
            Me.MostrarDatosNewConta()
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancelar.Click
        Try
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub




    Private Sub TextBoxAxWebServiceUrl_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxAxWebServiceUrl.Validated
        Try
            If Me.TextBoxAxWebServiceUrl.Text.Length > 0 Then
                If Mid(Me.TextBoxAxWebServiceUrl.Text, Me.TextBoxAxWebServiceUrl.TextLength, 1) <> "/" Then
                    MsgBox("Atención el Url del Web Service debe de terminar en /", MsgBoxStyle.Exclamation, "Atención")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBoxAxPalabraDePaso_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxAxPalabraDePaso.TextChanged
        Try
            Me.LabelPalabraPaso.Text = Me.TextBoxAxPalabraDePaso.Text
        Catch ex As Exception

        End Try
    End Sub


    Private Sub CheckBoxNewContaOrigenCuentaNewConta_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxNewContaOrigenCuentaNewConta.CheckedChanged
        Try
            If Me.CheckBoxNewContaOrigenCuentaNewConta.Checked = True Then
                Me.CheckBoxNewContaOrigenCuentaNewCentral.Checked = False
            End If

            If Me.CheckBoxNewContaOrigenCuentaNewConta.Checked = False Then
                Me.CheckBoxNewContaOrigenCuentaNewCentral.Checked = True
            End If


        Catch ex As Exception

        End Try
    End Sub

    Private Sub CheckBoxNewContaOrigenCuentaNewCentral_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxNewContaOrigenCuentaNewCentral.CheckedChanged
        Try
            If Me.CheckBoxNewContaOrigenCuentaNewCentral.Checked = True Then
                Me.CheckBoxNewContaOrigenCuentaNewConta.Checked = False
            End If

            If Me.CheckBoxNewContaOrigenCuentaNewCentral.Checked = False Then
                Me.CheckBoxNewContaOrigenCuentaNewConta.Checked = True
            End If
        Catch ex As Exception

        End Try
    End Sub



    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click
        Try
            Dim F As New FormInsertaEmpresa
            F.StartPosition = FormStartPosition.CenterScreen
            F.ShowDialog()
            Me.CargaCombos()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonSeccionesNewhotel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSeccionesNewhotel.Click
        Try
            Me.MuestraSeccionesNewhotel()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ComboBoxEmpCod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEmpCod.SelectedIndexChanged

    End Sub

    Private Sub TextBoxAxPalabraDePaso2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxAxPalabraDePaso2.TextChanged
        Try
            Me.LabelPalabraPaso2.Text = Me.TextBoxAxPalabraDePaso2.Text

        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBoxAxHotelId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxAxHotelId.TextChanged

    End Sub

    Private Sub TextBoxAxHotelId_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxAxHotelId.Validated
        Try
            If Me.TextBoxAxHotelId.TextLength > 0 Then
                If IsNumeric(Me.TextBoxAxHotelId.Text) = False Then
                    Me.TextBoxAxHotelId.Text = ""
                    MsgBox("Hotel Id , debe de ser numérico", MsgBoxStyle.Information, "Atención")
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonPathEmpresaContanet_Click(sender As Object, e As EventArgs) Handles ButtonPathEmpresaContanet.Click
        Try
            OpenFileDialog1.FileName = ""
            OpenFileDialog1.Filter = "All Files (*.*)|*.*|Ficheros de Empresa (*.nst)|*.nst"

            OpenFileDialog1.Title = "Seleccione Empresa"
            OpenFileDialog1.FilterIndex = 1

            If OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                If IsNothing(Me.OpenFileDialog1.FileName) = False Then
                    If Me.OpenFileDialog1.FileName.Length > 0 Then
                        Me.TextBoxRutaEmpresaContanet.Text = Me.OpenFileDialog1.FileName
                    End If
                End If

            End If




        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub



    Private Sub ButtonCobMovSpyro_Click_1(sender As Object, e As EventArgs) Handles ButtonCobMovSpyro.Click, ButtonCobMovSpyro2.Click
        Try

            Me.ListBoxRegistrosSpyro.Items.Clear()
            Me.ListBoxRegistrosSpyro.Update()
            If Me.CheckBoxValidaCuentasSpyro.Checked = True Then
                Dim StrSpyro As String
                Dim Result As String = ""
                SQL = "SELECT HOTEL_SPYRO FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
                SQL += " AND HOTEL_EMP_NUM = " & Me.TextBoxEmpNum.Text
                StrSpyro = CStr(Me.DbLeeAux.EjecutaSqlScalar(SQL))

                If StrSpyro <> "" Then
                    Dim DbNh As New C_DATOS.C_DatosOledb
                    DbNh.StrConexion = StrSpyro
                    DbNh.AbrirConexion()
                    SQL = "SELECT COD , DESCRIP FROM CFBCOTMOV ORDER BY COD ASC"
                    DbNh.TraerLector(SQL)
                    While DbNh.mDbLector.Read
                        Me.ListBoxRegistrosSpyro.Items.Add(CStr(DbNh.mDbLector.Item("COD")).PadRight(5, " ") & " = " & DbNh.mDbLector.Item("DESCRIP"))
                    End While
                    DbNh.mDbLector.Close()
                    DbNh.CerrarConexion()
                    DbNh = Nothing

                End If

            Else
                MsgBox("No existe Interfaz Odbc con el Hotel", MsgBoxStyle.Information, "Atención")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonFactutipoSpyro_Click(sender As Object, e As EventArgs) Handles ButtonFactutipoSpyro.Click
        Try

            Me.ListBoxRegistrosSpyro.Items.Clear()
            Me.ListBoxRegistrosSpyro.Update()
            If Me.CheckBoxValidaCuentasSpyro.Checked = True Then
                Dim StrSpyro As String
                Dim Result As String = ""
                SQL = "SELECT HOTEL_SPYRO FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
                SQL += " AND HOTEL_EMP_NUM = " & Me.TextBoxEmpNum.Text
                StrSpyro = CStr(Me.DbLeeAux.EjecutaSqlScalar(SQL))

                If StrSpyro <> "" Then
                    Dim DbNh As New C_DATOS.C_DatosOledb
                    DbNh.StrConexion = StrSpyro
                    DbNh.AbrirConexion()
                    SQL = "SELECT COD , DESCRIP FROM FACTUTIPO "
                    SQL += "WHERE CFIVALIBRO_COD = '" & Me.mParaCfIvaLibroCod & "'"
                    SQL += " ORDER BY COD ASC"
                    DbNh.TraerLector(SQL)
                    While DbNh.mDbLector.Read
                        Me.ListBoxRegistrosSpyro.Items.Add(CStr(DbNh.mDbLector.Item("COD")).PadRight(5, " ") & " = " & DbNh.mDbLector.Item("DESCRIP"))
                    End While
                    DbNh.mDbLector.Close()
                    DbNh.CerrarConexion()
                    DbNh = Nothing

                End If

            Else
                MsgBox("No existe Interfaz Odbc con el Hotel", MsgBoxStyle.Information, "Atención")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ButtonCobMovSpyro2.Click

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles ButtonNewContaBanco.Click
        Try

            Me.ListBoxRegistrosSpyro.Items.Clear()
            Me.ListBoxRegistrosSpyro.Update()
            If Me.CheckBoxValidaCuentasSpyro.Checked = True Then
                Dim StrSpyro As String
                Dim Result As String = ""
                SQL = "SELECT HOTEL_SPYRO FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
                SQL += " AND HOTEL_EMP_NUM = " & Me.TextBoxEmpNum.Text
                StrSpyro = CStr(Me.DbLeeAux.EjecutaSqlScalar(SQL))

                If StrSpyro <> "" Then
                    Dim DbNh As New C_DATOS.C_DatosOledb
                    DbNh.StrConexion = StrSpyro
                    DbNh.AbrirConexion()
                    SQL = "SELECT COD , DESCRIP FROM FPAGO ORDER BY COD ASC"
                    DbNh.TraerLector(SQL)
                    While DbNh.mDbLector.Read
                        Me.ListBoxRegistrosSpyro.Items.Add(CStr(DbNh.mDbLector.Item("COD")).PadRight(5, " ") & " = " & DbNh.mDbLector.Item("DESCRIP"))
                    End While
                    DbNh.mDbLector.Close()
                    DbNh.CerrarConexion()
                    DbNh = Nothing

                End If

            Else
                MsgBox("No existe Interfaz Odbc con el Hotel", MsgBoxStyle.Information, "Atención")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        Try

            Me.ListBoxRegistrosSpyro.Items.Clear()
            Me.ListBoxRegistrosSpyro.Update()
            If Me.CheckBoxValidaCuentasSpyro.Checked = True Then
                Dim StrSpyro As String
                Dim Result As String = ""
                SQL = "SELECT HOTEL_SPYRO FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
                SQL += " AND HOTEL_EMP_NUM = " & Me.TextBoxEmpNum.Text
                StrSpyro = CStr(Me.DbLeeAux.EjecutaSqlScalar(SQL))

                If StrSpyro <> "" Then
                    Dim DbNh As New C_DATOS.C_DatosOledb
                    DbNh.StrConexion = StrSpyro
                    DbNh.AbrirConexion()
                    SQL = "SELECT COD , DESCRIP FROM FPAGO ORDER BY COD ASC"
                    DbNh.TraerLector(SQL)
                    While DbNh.mDbLector.Read
                        Me.ListBoxRegistrosSpyro.Items.Add(CStr(DbNh.mDbLector.Item("COD")).PadRight(5, " ") & " = " & DbNh.mDbLector.Item("DESCRIP"))
                    End While
                    DbNh.mDbLector.Close()
                    DbNh.CerrarConexion()
                    DbNh = Nothing

                End If

            Else
                MsgBox("No existe Interfaz Odbc con el Hotel", MsgBoxStyle.Information, "Atención")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonServiciosNewHotel_Click(sender As Object, e As EventArgs) Handles ButtonServiciosNewHotel.Click
        Try
            If Me.CheckBoxInterfazNewHotel.Checked = True Then
                Dim StrHotel As String
                Dim Result As String = ""
                SQL = "SELECT HOTEL_ODBC FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
                SQL += " AND HOTEL_EMP_NUM = " & Me.TextBoxEmpNum.Text
                StrHotel = CStr(Me.DbLeeAux.EjecutaSqlScalar(SQL))

                If StrHotel <> "" Then
                    Dim DbNh As New C_DATOS.C_DatosOledb
                    DbNh.StrConexion = StrHotel
                    DbNh.AbrirConexion()
                    SQL = "SELECT SERV_CODI , SERV_DESC FROM TNHT_SERV ORDER BY SERV_DESC"
                    DbNh.TraerLector(SQL)
                    While DbNh.mDbLector.Read
                        Result += DbNh.mDbLector.Item("SERV_CODI") & " = " & DbNh.mDbLector.Item("SERV_DESC") & vbCrLf
                    End While
                    DbNh.mDbLector.Close()
                    DbNh.CerrarConexion()
                    DbNh = Nothing
                    MsgBox(Result)
                End If

            Else
                MsgBox("No existe Interfaz Odbc con el Hotel", MsgBoxStyle.Information, "Atención")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonFormasdeCobroNewHotel_Click(sender As Object, e As EventArgs) Handles ButtonFormasdeCobroNewHotel.Click
        Try
            If Me.CheckBoxInterfazNewHotel.Checked = True Then
                Dim StrHotel As String
                Dim Result As String = ""
                SQL = "SELECT HOTEL_ODBC FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
                SQL += " AND HOTEL_EMP_NUM = " & Me.TextBoxEmpNum.Text
                StrHotel = CStr(Me.DbLeeAux.EjecutaSqlScalar(SQL))

                If StrHotel <> "" Then
                    Dim DbNh As New C_DATOS.C_DatosOledb
                    DbNh.StrConexion = StrHotel
                    DbNh.AbrirConexion()
                    SQL = "SELECT FORE_CODI , FORE_DESC FROM TNHT_FORE WHERE FORE_CACR = '0' ORDER BY FORE_DESC"
                    DbNh.TraerLector(SQL)
                    While DbNh.mDbLector.Read
                        Result += DbNh.mDbLector.Item("FORE_CODI") & " = " & DbNh.mDbLector.Item("FORE_DESC") & vbCrLf
                    End While
                    DbNh.mDbLector.Close()
                    DbNh.CerrarConexion()
                    DbNh = Nothing
                    MsgBox(Result)
                End If

            Else
                MsgBox("No existe Interfaz Odbc con el Hotel", MsgBoxStyle.Information, "Atención")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub NumericUpDownWebServiceTimeOut_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDownWebServiceTimeOut.ValueChanged

    End Sub

    Private Sub ButtonSeccionesNewhotel2_Click(sender As Object, e As EventArgs) Handles ButtonSeccionesNewhotel2.Click
        Try
            Me.MuestraSeccionesNewhotel()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonTiposdeEfectoSpyro_Click(sender As Object, e As EventArgs) Handles ButtonTiposdeEfectoSpyro.Click
        Try

            Me.ListBoxRegistrosSpyro.Items.Clear()
            Me.ListBoxRegistrosSpyro.Update()
            If Me.CheckBoxValidaCuentasSpyro.Checked = True Then
                Dim StrSpyro As String
                Dim Result As String = ""
                SQL = "SELECT HOTEL_SPYRO FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
                SQL += " AND HOTEL_EMP_NUM = " & Me.TextBoxEmpNum.Text
                StrSpyro = CStr(Me.DbLeeAux.EjecutaSqlScalar(SQL))

                If StrSpyro <> "" Then
                    Dim DbNh As New C_DATOS.C_DatosOledb
                    DbNh.StrConexion = StrSpyro
                    DbNh.AbrirConexion()
                    SQL = "SELECT COD , DESCRIP FROM TEFECT "
                    ''      SQL += "WHERE CFIVALIBRO_COD = '" & Me.mParaCfIvaLibroCod & "'"
                    SQL += " ORDER BY DESCRIP ASC"
                    DbNh.TraerLector(SQL)
                    While DbNh.mDbLector.Read
                        Me.ListBoxRegistrosSpyro.Items.Add(CStr(DbNh.mDbLector.Item("COD")).PadRight(5, " ") & " = " & DbNh.mDbLector.Item("DESCRIP"))
                    End While
                    DbNh.mDbLector.Close()
                    DbNh.CerrarConexion()
                    DbNh = Nothing

                End If

            Else
                MsgBox("No existe Interfaz Odbc con el Hotel", MsgBoxStyle.Information, "Atención")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button_Click(sender As Object, e As EventArgs) Handles Button.Click
        Try

            Me.ListBoxRegistrosSpyro.Items.Clear()
            Me.ListBoxRegistrosSpyro.Update()
            If Me.CheckBoxValidaCuentasSpyro.Checked = True Then
                Dim StrSpyro As String
                Dim Result As String = ""
                SQL = "SELECT HOTEL_SPYRO FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
                SQL += " AND HOTEL_EMP_NUM = " & Me.TextBoxEmpNum.Text
                StrSpyro = CStr(Me.DbLeeAux.EjecutaSqlScalar(SQL))

                If StrSpyro <> "" Then
                    Dim DbNh As New C_DATOS.C_DatosOledb
                    DbNh.StrConexion = StrSpyro
                    DbNh.AbrirConexion()
                    SQL = "SELECT COD , DESCRIP FROM BANCOS ORDER BY COD ASC"
                    DbNh.TraerLector(SQL)
                    While DbNh.mDbLector.Read
                        Me.ListBoxRegistrosSpyro.Items.Add(CStr(DbNh.mDbLector.Item("COD")).PadRight(5, " ") & " = " & DbNh.mDbLector.Item("DESCRIP"))
                    End While
                    DbNh.mDbLector.Close()
                    DbNh.CerrarConexion()
                    DbNh = Nothing

                End If

            Else
                MsgBox("No existe Interfaz Odbc con el Hotel", MsgBoxStyle.Information, "Atención")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        Try

            Me.ListBoxRegistrosSpyro.Items.Clear()
            Me.ListBoxRegistrosSpyro.Update()
            If Me.CheckBoxValidaCuentasSpyro.Checked = True Then
                Dim StrSpyro As String
                Dim Result As String = ""
                SQL = "SELECT HOTEL_SPYRO FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.ComboBoxGrupoCod.Text & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.ComboBoxEmpCod.SelectedValue & "'"
                SQL += " AND HOTEL_EMP_NUM = " & Me.TextBoxEmpNum.Text
                StrSpyro = CStr(Me.DbLeeAux.EjecutaSqlScalar(SQL))

                If StrSpyro <> "" Then
                    Dim DbNh As New C_DATOS.C_DatosOledb
                    DbNh.StrConexion = StrSpyro
                    DbNh.AbrirConexion()
                    SQL = "SELECT COD , DESCRIP FROM BANCOS ORDER BY COD ASC"
                    DbNh.TraerLector(SQL)
                    While DbNh.mDbLector.Read
                        Me.ListBoxRegistrosSpyro.Items.Add(CStr(DbNh.mDbLector.Item("COD")).PadRight(5, " ") & " = " & DbNh.mDbLector.Item("DESCRIP"))
                    End While
                    DbNh.mDbLector.Close()
                    DbNh.CerrarConexion()
                    DbNh = Nothing

                End If

            Else
                MsgBox("No existe Interfaz Odbc con el Hotel", MsgBoxStyle.Information, "Atención")
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
