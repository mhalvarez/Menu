Imports Microsoft.VisualBasic.FileIO

Public Class FormIntegraFront
    Inherits System.Windows.Forms.Form
    Dim MyIni As New cIniArray
    Dim DbLee As C_DATOS.C_DatosOledb
    Dim DbLeeNewHotel As C_DATOS.C_DatosOledb
    Dim SQL As String
    Dim HayRegistros As Boolean = False

    Dim EstaLoad As Boolean = False

    Private mStatusBar As StatusBar
    Private mEmpGrupoCod As String
    Private mEmpCod As String
    Private mEmpNum As Integer

    Friend WithEvents DataGridViewBonos As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonManoCorriente As System.Windows.Forms.Button
    Friend WithEvents CheckBoxSoloBonos As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxIncluyeBonos As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxAuditaCobros As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxAuditaCobrosMostrar As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxDebug2 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxDevoluciones As System.Windows.Forms.CheckBox
    Friend WithEvents DataGridViewErrores As System.Windows.Forms.DataGridView

    Private DLL As Integer


    'To create the timer for the threading part  
    Public KeepAliveDelegate As Threading.TimerCallback
    Public KeepAliveTimer As System.Threading.Timer
    Friend WithEvents DateTimePicker2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents ButtonIncidencias As System.Windows.Forms.Button
    Friend WithEvents ButtonResetPerfil As System.Windows.Forms.Button
    Friend WithEvents CheckBoxRefresch As System.Windows.Forms.CheckBox
    Friend WithEvents TextBoxRefresh As System.Windows.Forms.TextBox

    Dim Win As Form = Nothing
    Friend WithEvents ButtonActBookId As System.Windows.Forms.Button

    ' nuevo tratamiento de dlls

    Dim DllHLopezFront As Hlopez2.Hlopez2

    Dim DllDunas2 As Hoteles_Dunas2.Dunas2

    Dim DllContanet As ContanetNewHotel.ContaNetNewHotel
    Dim DllHTito As HTitoNewHotel.HTitoNewHotel
    Dim DllHVital As VitalNewHotel.VitalNewHotel



    Dim DllHC7Front As HC7.HC7

    Dim DllaxaptaFront As Axapta.Axapta

    Dim DllErezaFront As Ereza.Ereza

    Dim DllTropical As Ptropical.Ptropical

    Friend WithEvents TabControlModosdeOperacion As System.Windows.Forms.TabControl
    Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage8 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage9 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage10 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage11 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage12 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage13 As System.Windows.Forms.TabPage
    Friend WithEvents ButtonImprimeErrores As System.Windows.Forms.Button
    Friend WithEvents ButtonImprimeExclusiones As System.Windows.Forms.Button
    Friend WithEvents ButtonImprimeEnviosPendientes As System.Windows.Forms.Button
    Friend WithEvents TabPage14 As System.Windows.Forms.TabPage

    Dim DllSatocan As Axapta.Axapta
    Private mFechaFront As Date

    Private mEstaProcesadando As Boolean = False
    Dim Test As Integer
    Friend WithEvents ButtonFechaFront As System.Windows.Forms.Button
    Private mProcId As String
    Private mTituloForm As String


    Private Enum mEnumTipoEnvio
        FrontOffice
        MaestroClientesNewhotel
        MaestroArticulosNewStock
        MaestroAlmacenesNewStock
        NewStock

    End Enum



#Region " Código generado por el Diseñador de Windows Forms "

    Public Sub New()
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()

    End Sub
    Public Sub New(ByVal vStatusBar As StatusBar)
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()
        Me.mStatusBar = vStatusBar
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
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents DataGrid2 As System.Windows.Forms.DataGrid
    Friend WithEvents CheckBoxDebug As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TabControlDebug As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBoxDebug As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxAnalitica As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonImprimir As System.Windows.Forms.Button
    Friend WithEvents ListBoxDebug As System.Windows.Forms.ListBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents ButtonVerErrores As System.Windows.Forms.Button
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents CheckBoxTpvNoFacturado As System.Windows.Forms.CheckBox
    Friend WithEvents CheckCuentaRedondeo As System.Windows.Forms.CheckBox
    Friend WithEvents DataGridErrorres As System.Windows.Forms.DataGrid
    Friend WithEvents CheckBoxFiltroDepositosNewHotel As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxComisiones As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonConvertir As System.Windows.Forms.Button
    Friend WithEvents TabPageGrupoEmpresas As System.Windows.Forms.TabPage
    Friend WithEvents DataGridHoteles As System.Windows.Forms.DataGrid
    Friend WithEvents ButtonSatocanLibroIgic As System.Windows.Forms.Button
    Friend WithEvents CheckBoxMinimizar As System.Windows.Forms.CheckBox
    Friend WithEvents TextBoxProduccion As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxFacturacion As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxSaldo As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxDepositosRecibidos As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxDepositosFacturados As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxDepositosSaldo As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TextBoxRutaFicheros As System.Windows.Forms.TextBox
    Friend WithEvents ButtonRutaFicheros As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormIntegraFront))
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.DataGridHoteles = New System.Windows.Forms.DataGrid()
        Me.DataGrid2 = New System.Windows.Forms.DataGrid()
        Me.CheckBoxDebug = New System.Windows.Forms.CheckBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.TabControlDebug = New System.Windows.Forms.TabControl()
        Me.TabPageGrupoEmpresas = New System.Windows.Forms.TabPage()
        Me.ButtonFechaFront = New System.Windows.Forms.Button()
        Me.DataGridViewBonos = New System.Windows.Forms.DataGridView()
        Me.ButtonRutaFicheros = New System.Windows.Forms.Button()
        Me.TextBoxRutaFicheros = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.TextBoxDebug = New System.Windows.Forms.TextBox()
        Me.CheckBoxAnalitica = New System.Windows.Forms.CheckBox()
        Me.ListBoxDebug = New System.Windows.Forms.ListBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.CheckBoxTpvNoFacturado = New System.Windows.Forms.CheckBox()
        Me.CheckCuentaRedondeo = New System.Windows.Forms.CheckBox()
        Me.DataGridErrorres = New System.Windows.Forms.DataGrid()
        Me.CheckBoxFiltroDepositosNewHotel = New System.Windows.Forms.CheckBox()
        Me.CheckBoxComisiones = New System.Windows.Forms.CheckBox()
        Me.ButtonConvertir = New System.Windows.Forms.Button()
        Me.ButtonSatocanLibroIgic = New System.Windows.Forms.Button()
        Me.CheckBoxMinimizar = New System.Windows.Forms.CheckBox()
        Me.TextBoxProduccion = New System.Windows.Forms.TextBox()
        Me.TextBoxFacturacion = New System.Windows.Forms.TextBox()
        Me.TextBoxSaldo = New System.Windows.Forms.TextBox()
        Me.TextBoxDepositosRecibidos = New System.Windows.Forms.TextBox()
        Me.TextBoxDepositosFacturados = New System.Windows.Forms.TextBox()
        Me.TextBoxDepositosSaldo = New System.Windows.Forms.TextBox()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.ButtonManoCorriente = New System.Windows.Forms.Button()
        Me.CheckBoxSoloBonos = New System.Windows.Forms.CheckBox()
        Me.CheckBoxIncluyeBonos = New System.Windows.Forms.CheckBox()
        Me.CheckBoxAuditaCobros = New System.Windows.Forms.CheckBox()
        Me.CheckBoxAuditaCobrosMostrar = New System.Windows.Forms.CheckBox()
        Me.CheckBoxDebug2 = New System.Windows.Forms.CheckBox()
        Me.CheckBoxDevoluciones = New System.Windows.Forms.CheckBox()
        Me.DataGridViewErrores = New System.Windows.Forms.DataGridView()
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker()
        Me.CheckBoxRefresch = New System.Windows.Forms.CheckBox()
        Me.TextBoxRefresh = New System.Windows.Forms.TextBox()
        Me.ButtonActBookId = New System.Windows.Forms.Button()
        Me.TabControlModosdeOperacion = New System.Windows.Forms.TabControl()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.TabPage8 = New System.Windows.Forms.TabPage()
        Me.TabPage9 = New System.Windows.Forms.TabPage()
        Me.TabPage10 = New System.Windows.Forms.TabPage()
        Me.TabPage12 = New System.Windows.Forms.TabPage()
        Me.TabPage13 = New System.Windows.Forms.TabPage()
        Me.ButtonImprimeEnviosPendientes = New System.Windows.Forms.Button()
        Me.ButtonImprimeExclusiones = New System.Windows.Forms.Button()
        Me.ButtonImprimeErrores = New System.Windows.Forms.Button()
        Me.TabPage11 = New System.Windows.Forms.TabPage()
        Me.TabPage14 = New System.Windows.Forms.TabPage()
        Me.ButtonAceptar = New System.Windows.Forms.Button()
        Me.ButtonResetPerfil = New System.Windows.Forms.Button()
        Me.ButtonIncidencias = New System.Windows.Forms.Button()
        Me.ButtonVerErrores = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.ButtonImprimir = New System.Windows.Forms.Button()
        CType(Me.DataGridHoteles, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGrid2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlDebug.SuspendLayout()
        Me.TabPageGrupoEmpresas.SuspendLayout()
        CType(Me.DataGridViewBonos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage1.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        CType(Me.DataGridErrorres, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridViewErrores, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlModosdeOperacion.SuspendLayout()
        Me.TabPage8.SuspendLayout()
        Me.TabPage10.SuspendLayout()
        Me.TabPage12.SuspendLayout()
        Me.TabPage13.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.CustomFormat = "dd/MM/yyyy"
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker1.Location = New System.Drawing.Point(8, 8)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(152, 20)
        Me.DateTimePicker1.TabIndex = 0
        Me.DateTimePicker1.Value = New Date(2006, 4, 10, 0, 0, 0, 0)
        '
        'DataGridHoteles
        '
        Me.DataGridHoteles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridHoteles.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DataGridHoteles.DataMember = ""
        Me.DataGridHoteles.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridHoteles.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGridHoteles.Location = New System.Drawing.Point(8, 8)
        Me.DataGridHoteles.Name = "DataGridHoteles"
        Me.DataGridHoteles.ReadOnly = True
        Me.DataGridHoteles.Size = New System.Drawing.Size(475, 94)
        Me.DataGridHoteles.TabIndex = 2
        '
        'DataGrid2
        '
        Me.DataGrid2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGrid2.DataMember = ""
        Me.DataGrid2.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGrid2.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGrid2.Location = New System.Drawing.Point(8, 225)
        Me.DataGrid2.Name = "DataGrid2"
        Me.DataGrid2.ReadOnly = True
        Me.DataGrid2.Size = New System.Drawing.Size(846, 95)
        Me.DataGrid2.TabIndex = 3
        '
        'CheckBoxDebug
        '
        Me.CheckBoxDebug.Checked = True
        Me.CheckBoxDebug.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxDebug.Location = New System.Drawing.Point(6, 0)
        Me.CheckBoxDebug.Name = "CheckBoxDebug"
        Me.CheckBoxDebug.Size = New System.Drawing.Size(66, 24)
        Me.CheckBoxDebug.TabIndex = 4
        Me.CheckBoxDebug.Text = "Debug"
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox1.Location = New System.Drawing.Point(144, 8)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(120, 20)
        Me.TextBox1.TabIndex = 5
        '
        'TextBox2
        '
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox2.Location = New System.Drawing.Point(144, 32)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(120, 20)
        Me.TextBox2.TabIndex = 6
        '
        'TextBox3
        '
        Me.TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox3.Location = New System.Drawing.Point(144, 56)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(120, 20)
        Me.TextBox3.TabIndex = 7
        '
        'TabControlDebug
        '
        Me.TabControlDebug.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControlDebug.Controls.Add(Me.TabPageGrupoEmpresas)
        Me.TabControlDebug.Controls.Add(Me.TabPage1)
        Me.TabControlDebug.Controls.Add(Me.TabPage2)
        Me.TabControlDebug.Controls.Add(Me.TabPage3)
        Me.TabControlDebug.Controls.Add(Me.TabPage4)
        Me.TabControlDebug.Controls.Add(Me.TabPage5)
        Me.TabControlDebug.Controls.Add(Me.TabPage6)
        Me.TabControlDebug.Location = New System.Drawing.Point(8, 72)
        Me.TabControlDebug.Name = "TabControlDebug"
        Me.TabControlDebug.SelectedIndex = 0
        Me.TabControlDebug.Size = New System.Drawing.Size(831, 131)
        Me.TabControlDebug.TabIndex = 8
        '
        'TabPageGrupoEmpresas
        '
        Me.TabPageGrupoEmpresas.Controls.Add(Me.ButtonFechaFront)
        Me.TabPageGrupoEmpresas.Controls.Add(Me.DataGridViewBonos)
        Me.TabPageGrupoEmpresas.Controls.Add(Me.ButtonRutaFicheros)
        Me.TabPageGrupoEmpresas.Controls.Add(Me.TextBoxRutaFicheros)
        Me.TabPageGrupoEmpresas.Controls.Add(Me.Label8)
        Me.TabPageGrupoEmpresas.Controls.Add(Me.DataGridHoteles)
        Me.TabPageGrupoEmpresas.Location = New System.Drawing.Point(4, 22)
        Me.TabPageGrupoEmpresas.Name = "TabPageGrupoEmpresas"
        Me.TabPageGrupoEmpresas.Size = New System.Drawing.Size(823, 105)
        Me.TabPageGrupoEmpresas.TabIndex = 6
        Me.TabPageGrupoEmpresas.Text = "Grupo de Empresas"
        '
        'ButtonFechaFront
        '
        Me.ButtonFechaFront.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonFechaFront.Image = Global.Menu.My.Resources.Resources.date2
        Me.ButtonFechaFront.Location = New System.Drawing.Point(489, 8)
        Me.ButtonFechaFront.Name = "ButtonFechaFront"
        Me.ButtonFechaFront.Size = New System.Drawing.Size(36, 40)
        Me.ButtonFechaFront.TabIndex = 37
        Me.ButtonFechaFront.UseVisualStyleBackColor = True
        '
        'DataGridViewBonos
        '
        Me.DataGridViewBonos.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewBonos.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridViewBonos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewBonos.DefaultCellStyle = DataGridViewCellStyle5
        Me.DataGridViewBonos.Location = New System.Drawing.Point(643, 42)
        Me.DataGridViewBonos.Name = "DataGridViewBonos"
        Me.DataGridViewBonos.ReadOnly = True
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewBonos.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.DataGridViewBonos.Size = New System.Drawing.Size(164, 46)
        Me.DataGridViewBonos.TabIndex = 36
        '
        'ButtonRutaFicheros
        '
        Me.ButtonRutaFicheros.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonRutaFicheros.Location = New System.Drawing.Point(767, 16)
        Me.ButtonRutaFicheros.Name = "ButtonRutaFicheros"
        Me.ButtonRutaFicheros.Size = New System.Drawing.Size(40, 23)
        Me.ButtonRutaFicheros.TabIndex = 5
        Me.ButtonRutaFicheros.Text = ":::"
        '
        'TextBoxRutaFicheros
        '
        Me.TextBoxRutaFicheros.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxRutaFicheros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxRutaFicheros.Location = New System.Drawing.Point(616, 16)
        Me.TextBoxRutaFicheros.Name = "TextBoxRutaFicheros"
        Me.TextBoxRutaFicheros.ReadOnly = True
        Me.TextBoxRutaFicheros.Size = New System.Drawing.Size(143, 20)
        Me.TextBoxRutaFicheros.TabIndex = 4
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.Location = New System.Drawing.Point(531, 13)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(80, 23)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "Ruta Ficheros"
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(838, 105)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Venta Líquida"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(128, 23)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Ajuste por Redondeos"
        '
        'TabPage2
        '
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(838, 105)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Anticipos Recibidos"
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.TextBox5)
        Me.TabPage3.Controls.Add(Me.TextBox4)
        Me.TabPage3.Controls.Add(Me.Label7)
        Me.TabPage3.Controls.Add(Me.Label6)
        Me.TabPage3.Controls.Add(Me.Label3)
        Me.TabPage3.Controls.Add(Me.Label2)
        Me.TabPage3.Controls.Add(Me.Label1)
        Me.TabPage3.Controls.Add(Me.TextBox3)
        Me.TabPage3.Controls.Add(Me.TextBox1)
        Me.TabPage3.Controls.Add(Me.TextBox2)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(838, 105)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Fact. Contado"
        '
        'TextBox5
        '
        Me.TextBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox5.Location = New System.Drawing.Point(400, 32)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(120, 20)
        Me.TextBox5.TabIndex = 14
        '
        'TextBox4
        '
        Me.TextBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox4.Location = New System.Drawing.Point(400, 8)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(120, 20)
        Me.TextBox4.TabIndex = 13
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(272, 32)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(112, 23)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Anticipos Devueltos"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(272, 8)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(136, 23)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Anticipos Facturados"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 56)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(136, 23)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Desembolsos Facturados"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(128, 23)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Facturado sin Impuesto"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(136, 23)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Facturado Con Impuesto"
        '
        'TabPage4
        '
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(838, 105)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Desembolsos"
        '
        'TabPage5
        '
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(838, 105)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "Facturación de Crédito"
        '
        'TabPage6
        '
        Me.TabPage6.Location = New System.Drawing.Point(4, 22)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(838, 105)
        Me.TabPage6.TabIndex = 5
        Me.TabPage6.Text = "Facturación de Crédito Anulada"
        '
        'TextBoxDebug
        '
        Me.TextBoxDebug.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxDebug.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.TextBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDebug.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxDebug.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.TextBoxDebug.Location = New System.Drawing.Point(8, 326)
        Me.TextBoxDebug.Name = "TextBoxDebug"
        Me.TextBoxDebug.Size = New System.Drawing.Size(846, 20)
        Me.TextBoxDebug.TabIndex = 9
        '
        'CheckBoxAnalitica
        '
        Me.CheckBoxAnalitica.Checked = True
        Me.CheckBoxAnalitica.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxAnalitica.Location = New System.Drawing.Point(77, 3)
        Me.CheckBoxAnalitica.Name = "CheckBoxAnalitica"
        Me.CheckBoxAnalitica.Size = New System.Drawing.Size(72, 20)
        Me.CheckBoxAnalitica.TabIndex = 10
        Me.CheckBoxAnalitica.Text = "Analítica"
        '
        'ListBoxDebug
        '
        Me.ListBoxDebug.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListBoxDebug.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBoxDebug.ForeColor = System.Drawing.Color.Black
        Me.ListBoxDebug.ItemHeight = 14
        Me.ListBoxDebug.Location = New System.Drawing.Point(8, 510)
        Me.ListBoxDebug.Name = "ListBoxDebug"
        Me.ListBoxDebug.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.ListBoxDebug.Size = New System.Drawing.Size(846, 16)
        Me.ListBoxDebug.TabIndex = 13
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Maroon
        Me.Label5.Location = New System.Drawing.Point(8, 350)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(160, 23)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "No Integre si exiten Mensajes "
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(8, 209)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(846, 10)
        Me.ProgressBar1.TabIndex = 17
        '
        'CheckBoxTpvNoFacturado
        '
        Me.CheckBoxTpvNoFacturado.Checked = True
        Me.CheckBoxTpvNoFacturado.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxTpvNoFacturado.Location = New System.Drawing.Point(157, 3)
        Me.CheckBoxTpvNoFacturado.Name = "CheckBoxTpvNoFacturado"
        Me.CheckBoxTpvNoFacturado.Size = New System.Drawing.Size(246, 24)
        Me.CheckBoxTpvNoFacturado.TabIndex = 19
        Me.CheckBoxTpvNoFacturado.Text = "NO Tratar Débito Tpv no Facturado"
        '
        'CheckCuentaRedondeo
        '
        Me.CheckCuentaRedondeo.Checked = True
        Me.CheckCuentaRedondeo.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckCuentaRedondeo.Location = New System.Drawing.Point(277, 26)
        Me.CheckCuentaRedondeo.Name = "CheckCuentaRedondeo"
        Me.CheckCuentaRedondeo.Size = New System.Drawing.Size(225, 16)
        Me.CheckCuentaRedondeo.TabIndex = 23
        Me.CheckCuentaRedondeo.Text = "Usar Cuenta para Ajuste de Redondeos"
        '
        'DataGridErrorres
        '
        Me.DataGridErrorres.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridErrorres.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DataGridErrorres.DataMember = ""
        Me.DataGridErrorres.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridErrorres.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGridErrorres.Location = New System.Drawing.Point(8, 374)
        Me.DataGridErrorres.Name = "DataGridErrorres"
        Me.DataGridErrorres.ReadOnly = True
        Me.DataGridErrorres.Size = New System.Drawing.Size(842, 130)
        Me.DataGridErrorres.TabIndex = 24
        '
        'CheckBoxFiltroDepositosNewHotel
        '
        Me.CheckBoxFiltroDepositosNewHotel.Checked = True
        Me.CheckBoxFiltroDepositosNewHotel.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxFiltroDepositosNewHotel.Enabled = False
        Me.CheckBoxFiltroDepositosNewHotel.ForeColor = System.Drawing.Color.Maroon
        Me.CheckBoxFiltroDepositosNewHotel.Location = New System.Drawing.Point(384, 4)
        Me.CheckBoxFiltroDepositosNewHotel.Name = "CheckBoxFiltroDepositosNewHotel"
        Me.CheckBoxFiltroDepositosNewHotel.Size = New System.Drawing.Size(112, 20)
        Me.CheckBoxFiltroDepositosNewHotel.TabIndex = 25
        Me.CheckBoxFiltroDepositosNewHotel.Text = "Movi_AUTO = 0"
        '
        'CheckBoxComisiones
        '
        Me.CheckBoxComisiones.Checked = True
        Me.CheckBoxComisiones.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxComisiones.ForeColor = System.Drawing.Color.Maroon
        Me.CheckBoxComisiones.Location = New System.Drawing.Point(6, 22)
        Me.CheckBoxComisiones.Name = "CheckBoxComisiones"
        Me.CheckBoxComisiones.Size = New System.Drawing.Size(232, 24)
        Me.CheckBoxComisiones.TabIndex = 26
        Me.CheckBoxComisiones.Text = "Base Imponible Afectada por Comisiones"
        '
        'ButtonConvertir
        '
        Me.ButtonConvertir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonConvertir.BackColor = System.Drawing.Color.Maroon
        Me.ButtonConvertir.Enabled = False
        Me.ButtonConvertir.ForeColor = System.Drawing.Color.White
        Me.ButtonConvertir.Location = New System.Drawing.Point(862, 241)
        Me.ButtonConvertir.Name = "ButtonConvertir"
        Me.ButtonConvertir.Size = New System.Drawing.Size(88, 23)
        Me.ButtonConvertir.TabIndex = 27
        Me.ButtonConvertir.Text = "Convertir"
        Me.ButtonConvertir.UseVisualStyleBackColor = False
        '
        'ButtonSatocanLibroIgic
        '
        Me.ButtonSatocanLibroIgic.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonSatocanLibroIgic.Location = New System.Drawing.Point(125, 5)
        Me.ButtonSatocanLibroIgic.Name = "ButtonSatocanLibroIgic"
        Me.ButtonSatocanLibroIgic.Size = New System.Drawing.Size(116, 25)
        Me.ButtonSatocanLibroIgic.TabIndex = 28
        Me.ButtonSatocanLibroIgic.Text = "Libro de Impuestos"
        '
        'CheckBoxMinimizar
        '
        Me.CheckBoxMinimizar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxMinimizar.ForeColor = System.Drawing.Color.Maroon
        Me.CheckBoxMinimizar.Location = New System.Drawing.Point(846, 94)
        Me.CheckBoxMinimizar.Name = "CheckBoxMinimizar"
        Me.CheckBoxMinimizar.Size = New System.Drawing.Size(69, 17)
        Me.CheckBoxMinimizar.TabIndex = 29
        Me.CheckBoxMinimizar.Text = "Minimizar"
        '
        'TextBoxProduccion
        '
        Me.TextBoxProduccion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxProduccion.Location = New System.Drawing.Point(486, 350)
        Me.TextBoxProduccion.Name = "TextBoxProduccion"
        Me.TextBoxProduccion.ReadOnly = True
        Me.TextBoxProduccion.Size = New System.Drawing.Size(112, 20)
        Me.TextBoxProduccion.TabIndex = 30
        Me.TextBoxProduccion.Text = "0"
        '
        'TextBoxFacturacion
        '
        Me.TextBoxFacturacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxFacturacion.Location = New System.Drawing.Point(606, 350)
        Me.TextBoxFacturacion.Name = "TextBoxFacturacion"
        Me.TextBoxFacturacion.ReadOnly = True
        Me.TextBoxFacturacion.Size = New System.Drawing.Size(120, 20)
        Me.TextBoxFacturacion.TabIndex = 31
        Me.TextBoxFacturacion.Text = "0"
        '
        'TextBoxSaldo
        '
        Me.TextBoxSaldo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxSaldo.Location = New System.Drawing.Point(750, 350)
        Me.TextBoxSaldo.Name = "TextBoxSaldo"
        Me.TextBoxSaldo.ReadOnly = True
        Me.TextBoxSaldo.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxSaldo.TabIndex = 32
        Me.TextBoxSaldo.Text = "0"
        '
        'TextBoxDepositosRecibidos
        '
        Me.TextBoxDepositosRecibidos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxDepositosRecibidos.Location = New System.Drawing.Point(192, 350)
        Me.TextBoxDepositosRecibidos.Name = "TextBoxDepositosRecibidos"
        Me.TextBoxDepositosRecibidos.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxDepositosRecibidos.TabIndex = 33
        Me.TextBoxDepositosRecibidos.Text = "0"
        '
        'TextBoxDepositosFacturados
        '
        Me.TextBoxDepositosFacturados.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxDepositosFacturados.Location = New System.Drawing.Point(288, 350)
        Me.TextBoxDepositosFacturados.Name = "TextBoxDepositosFacturados"
        Me.TextBoxDepositosFacturados.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxDepositosFacturados.TabIndex = 34
        Me.TextBoxDepositosFacturados.Text = "0"
        '
        'TextBoxDepositosSaldo
        '
        Me.TextBoxDepositosSaldo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxDepositosSaldo.Location = New System.Drawing.Point(384, 350)
        Me.TextBoxDepositosSaldo.Name = "TextBoxDepositosSaldo"
        Me.TextBoxDepositosSaldo.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxDepositosSaldo.TabIndex = 35
        Me.TextBoxDepositosSaldo.Text = "0"
        '
        'FolderBrowserDialog1
        '
        '
        'ButtonManoCorriente
        '
        Me.ButtonManoCorriente.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonManoCorriente.Location = New System.Drawing.Point(247, 5)
        Me.ButtonManoCorriente.Name = "ButtonManoCorriente"
        Me.ButtonManoCorriente.Size = New System.Drawing.Size(114, 25)
        Me.ButtonManoCorriente.TabIndex = 36
        Me.ButtonManoCorriente.Text = "Saldo M. Corriente"
        '
        'CheckBoxSoloBonos
        '
        Me.CheckBoxSoloBonos.AutoSize = True
        Me.CheckBoxSoloBonos.Location = New System.Drawing.Point(3, 6)
        Me.CheckBoxSoloBonos.Name = "CheckBoxSoloBonos"
        Me.CheckBoxSoloBonos.Size = New System.Drawing.Size(153, 17)
        Me.CheckBoxSoloBonos.TabIndex = 37
        Me.CheckBoxSoloBonos.Text = "Solo Bonos (Solo Satocan)"
        Me.CheckBoxSoloBonos.UseVisualStyleBackColor = True
        '
        'CheckBoxIncluyeBonos
        '
        Me.CheckBoxIncluyeBonos.AutoSize = True
        Me.CheckBoxIncluyeBonos.Checked = True
        Me.CheckBoxIncluyeBonos.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxIncluyeBonos.Location = New System.Drawing.Point(395, 6)
        Me.CheckBoxIncluyeBonos.Name = "CheckBoxIncluyeBonos"
        Me.CheckBoxIncluyeBonos.Size = New System.Drawing.Size(87, 17)
        Me.CheckBoxIncluyeBonos.TabIndex = 38
        Me.CheckBoxIncluyeBonos.Text = "Incluir Bonos"
        Me.CheckBoxIncluyeBonos.UseVisualStyleBackColor = True
        '
        'CheckBoxAuditaCobros
        '
        Me.CheckBoxAuditaCobros.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxAuditaCobros.Checked = True
        Me.CheckBoxAuditaCobros.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxAuditaCobros.Location = New System.Drawing.Point(846, 118)
        Me.CheckBoxAuditaCobros.Name = "CheckBoxAuditaCobros"
        Me.CheckBoxAuditaCobros.Size = New System.Drawing.Size(92, 17)
        Me.CheckBoxAuditaCobros.TabIndex = 39
        Me.CheckBoxAuditaCobros.Text = "Audita Cobros"
        Me.CheckBoxAuditaCobros.UseVisualStyleBackColor = True
        '
        'CheckBoxAuditaCobrosMostrar
        '
        Me.CheckBoxAuditaCobrosMostrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxAuditaCobrosMostrar.Location = New System.Drawing.Point(846, 141)
        Me.CheckBoxAuditaCobrosMostrar.Name = "CheckBoxAuditaCobrosMostrar"
        Me.CheckBoxAuditaCobrosMostrar.Size = New System.Drawing.Size(94, 17)
        Me.CheckBoxAuditaCobrosMostrar.TabIndex = 40
        Me.CheckBoxAuditaCobrosMostrar.Text = "Audita Mostrar"
        Me.CheckBoxAuditaCobrosMostrar.UseVisualStyleBackColor = True
        '
        'CheckBoxDebug2
        '
        Me.CheckBoxDebug2.Location = New System.Drawing.Point(274, 6)
        Me.CheckBoxDebug2.Name = "CheckBoxDebug2"
        Me.CheckBoxDebug2.Size = New System.Drawing.Size(115, 19)
        Me.CheckBoxDebug2.TabIndex = 41
        Me.CheckBoxDebug2.Text = "Saldos Anticipos"
        '
        'CheckBoxDevoluciones
        '
        Me.CheckBoxDevoluciones.AutoSize = True
        Me.CheckBoxDevoluciones.Checked = True
        Me.CheckBoxDevoluciones.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxDevoluciones.Location = New System.Drawing.Point(162, 6)
        Me.CheckBoxDevoluciones.Name = "CheckBoxDevoluciones"
        Me.CheckBoxDevoluciones.Size = New System.Drawing.Size(106, 17)
        Me.CheckBoxDevoluciones.TabIndex = 42
        Me.CheckBoxDevoluciones.Text = "Devoluciones Ax"
        Me.CheckBoxDevoluciones.UseVisualStyleBackColor = True
        '
        'DataGridViewErrores
        '
        Me.DataGridViewErrores.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridViewErrores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewErrores.Location = New System.Drawing.Point(864, 403)
        Me.DataGridViewErrores.Name = "DataGridViewErrores"
        Me.DataGridViewErrores.Size = New System.Drawing.Size(89, 31)
        Me.DataGridViewErrores.TabIndex = 43
        Me.DataGridViewErrores.Visible = False
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.CustomFormat = "dd/MM/yyyy"
        Me.DateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker2.Location = New System.Drawing.Point(8, 29)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(152, 20)
        Me.DateTimePicker2.TabIndex = 1
        Me.DateTimePicker2.Value = New Date(2006, 4, 10, 0, 0, 0, 0)
        '
        'CheckBoxRefresch
        '
        Me.CheckBoxRefresch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxRefresch.Location = New System.Drawing.Point(846, 165)
        Me.CheckBoxRefresch.Name = "CheckBoxRefresch"
        Me.CheckBoxRefresch.Size = New System.Drawing.Size(63, 17)
        Me.CheckBoxRefresch.TabIndex = 47
        Me.CheckBoxRefresch.Text = "Refresh"
        Me.CheckBoxRefresch.UseVisualStyleBackColor = True
        '
        'TextBoxRefresh
        '
        Me.TextBoxRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxRefresh.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.TextBoxRefresh.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxRefresh.Location = New System.Drawing.Point(863, 509)
        Me.TextBoxRefresh.Name = "TextBoxRefresh"
        Me.TextBoxRefresh.Size = New System.Drawing.Size(83, 20)
        Me.TextBoxRefresh.TabIndex = 48
        '
        'ButtonActBookId
        '
        Me.ButtonActBookId.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonActBookId.Location = New System.Drawing.Point(3, 5)
        Me.ButtonActBookId.Name = "ButtonActBookId"
        Me.ButtonActBookId.Size = New System.Drawing.Size(116, 25)
        Me.ButtonActBookId.TabIndex = 49
        Me.ButtonActBookId.Text = "Actualiza Book Id"
        '
        'TabControlModosdeOperacion
        '
        Me.TabControlModosdeOperacion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControlModosdeOperacion.Controls.Add(Me.TabPage7)
        Me.TabControlModosdeOperacion.Controls.Add(Me.TabPage8)
        Me.TabControlModosdeOperacion.Controls.Add(Me.TabPage9)
        Me.TabControlModosdeOperacion.Controls.Add(Me.TabPage10)
        Me.TabControlModosdeOperacion.Controls.Add(Me.TabPage12)
        Me.TabControlModosdeOperacion.Controls.Add(Me.TabPage13)
        Me.TabControlModosdeOperacion.Controls.Add(Me.TabPage11)
        Me.TabControlModosdeOperacion.Controls.Add(Me.TabPage14)
        Me.TabControlModosdeOperacion.Location = New System.Drawing.Point(265, 2)
        Me.TabControlModosdeOperacion.Name = "TabControlModosdeOperacion"
        Me.TabControlModosdeOperacion.SelectedIndex = 0
        Me.TabControlModosdeOperacion.Size = New System.Drawing.Size(570, 68)
        Me.TabControlModosdeOperacion.TabIndex = 50
        '
        'TabPage7
        '
        Me.TabPage7.Location = New System.Drawing.Point(4, 22)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage7.Size = New System.Drawing.Size(562, 42)
        Me.TabPage7.TabIndex = 0
        Me.TabPage7.Text = "General"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'TabPage8
        '
        Me.TabPage8.Controls.Add(Me.CheckBoxDebug)
        Me.TabPage8.Controls.Add(Me.CheckBoxAnalitica)
        Me.TabPage8.Controls.Add(Me.CheckBoxTpvNoFacturado)
        Me.TabPage8.Controls.Add(Me.CheckBoxFiltroDepositosNewHotel)
        Me.TabPage8.Controls.Add(Me.CheckBoxComisiones)
        Me.TabPage8.Controls.Add(Me.CheckCuentaRedondeo)
        Me.TabPage8.Location = New System.Drawing.Point(4, 22)
        Me.TabPage8.Name = "TabPage8"
        Me.TabPage8.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage8.Size = New System.Drawing.Size(581, 42)
        Me.TabPage8.TabIndex = 1
        Me.TabPage8.Text = "Modos de Operación"
        Me.TabPage8.UseVisualStyleBackColor = True
        '
        'TabPage9
        '
        Me.TabPage9.Location = New System.Drawing.Point(4, 22)
        Me.TabPage9.Name = "TabPage9"
        Me.TabPage9.Size = New System.Drawing.Size(581, 42)
        Me.TabPage9.TabIndex = 2
        Me.TabPage9.Text = "Debug"
        Me.TabPage9.UseVisualStyleBackColor = True
        '
        'TabPage10
        '
        Me.TabPage10.Controls.Add(Me.CheckBoxSoloBonos)
        Me.TabPage10.Controls.Add(Me.CheckBoxDevoluciones)
        Me.TabPage10.Controls.Add(Me.CheckBoxDebug2)
        Me.TabPage10.Controls.Add(Me.CheckBoxIncluyeBonos)
        Me.TabPage10.Location = New System.Drawing.Point(4, 22)
        Me.TabPage10.Name = "TabPage10"
        Me.TabPage10.Size = New System.Drawing.Size(581, 42)
        Me.TabPage10.TabIndex = 3
        Me.TabPage10.Text = "Satocan"
        Me.TabPage10.UseVisualStyleBackColor = True
        '
        'TabPage12
        '
        Me.TabPage12.Controls.Add(Me.ButtonActBookId)
        Me.TabPage12.Controls.Add(Me.ButtonSatocanLibroIgic)
        Me.TabPage12.Controls.Add(Me.ButtonManoCorriente)
        Me.TabPage12.Location = New System.Drawing.Point(4, 22)
        Me.TabPage12.Name = "TabPage12"
        Me.TabPage12.Size = New System.Drawing.Size(581, 42)
        Me.TabPage12.TabIndex = 5
        Me.TabPage12.Text = "Satocan"
        Me.TabPage12.UseVisualStyleBackColor = True
        '
        'TabPage13
        '
        Me.TabPage13.Controls.Add(Me.ButtonImprimeEnviosPendientes)
        Me.TabPage13.Controls.Add(Me.ButtonImprimeExclusiones)
        Me.TabPage13.Controls.Add(Me.ButtonImprimeErrores)
        Me.TabPage13.Location = New System.Drawing.Point(4, 22)
        Me.TabPage13.Name = "TabPage13"
        Me.TabPage13.Size = New System.Drawing.Size(581, 42)
        Me.TabPage13.TabIndex = 6
        Me.TabPage13.Text = "Satocan Incidencias Reports"
        Me.TabPage13.UseVisualStyleBackColor = True
        '
        'ButtonImprimeEnviosPendientes
        '
        Me.ButtonImprimeEnviosPendientes.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprimeEnviosPendientes.Location = New System.Drawing.Point(277, 5)
        Me.ButtonImprimeEnviosPendientes.Name = "ButtonImprimeEnviosPendientes"
        Me.ButtonImprimeEnviosPendientes.Size = New System.Drawing.Size(104, 23)
        Me.ButtonImprimeEnviosPendientes.TabIndex = 45
        Me.ButtonImprimeEnviosPendientes.Text = "Envios Pdtes."
        Me.ButtonImprimeEnviosPendientes.UseVisualStyleBackColor = True
        '
        'ButtonImprimeExclusiones
        '
        Me.ButtonImprimeExclusiones.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprimeExclusiones.Location = New System.Drawing.Point(113, 5)
        Me.ButtonImprimeExclusiones.Name = "ButtonImprimeExclusiones"
        Me.ButtonImprimeExclusiones.Size = New System.Drawing.Size(158, 23)
        Me.ButtonImprimeExclusiones.TabIndex = 44
        Me.ButtonImprimeExclusiones.Text = "Imprimir &Excluidos/Omitidos"
        Me.ButtonImprimeExclusiones.UseVisualStyleBackColor = True
        '
        'ButtonImprimeErrores
        '
        Me.ButtonImprimeErrores.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprimeErrores.Location = New System.Drawing.Point(3, 5)
        Me.ButtonImprimeErrores.Name = "ButtonImprimeErrores"
        Me.ButtonImprimeErrores.Size = New System.Drawing.Size(104, 23)
        Me.ButtonImprimeErrores.TabIndex = 32
        Me.ButtonImprimeErrores.Text = "&Imprimir Errores"
        Me.ButtonImprimeErrores.UseVisualStyleBackColor = True
        '
        'TabPage11
        '
        Me.TabPage11.Location = New System.Drawing.Point(4, 22)
        Me.TabPage11.Name = "TabPage11"
        Me.TabPage11.Size = New System.Drawing.Size(581, 42)
        Me.TabPage11.TabIndex = 4
        Me.TabPage11.Text = "GRHL"
        Me.TabPage11.UseVisualStyleBackColor = True
        '
        'TabPage14
        '
        Me.TabPage14.Location = New System.Drawing.Point(4, 22)
        Me.TabPage14.Name = "TabPage14"
        Me.TabPage14.Size = New System.Drawing.Size(581, 42)
        Me.TabPage14.TabIndex = 7
        Me.TabPage14.Text = "HC7 Hotels"
        Me.TabPage14.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Maroon
        Me.ButtonAceptar.ForeColor = System.Drawing.Color.White
        Me.ButtonAceptar.Image = CType(resources.GetObject("ButtonAceptar.Image"), System.Drawing.Image)
        Me.ButtonAceptar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonAceptar.Location = New System.Drawing.Point(168, 8)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(80, 23)
        Me.ButtonAceptar.TabIndex = 11
        Me.ButtonAceptar.Text = "&Aceptar"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'ButtonResetPerfil
        '
        Me.ButtonResetPerfil.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonResetPerfil.Image = CType(resources.GetObject("ButtonResetPerfil.Image"), System.Drawing.Image)
        Me.ButtonResetPerfil.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonResetPerfil.Location = New System.Drawing.Point(862, 297)
        Me.ButtonResetPerfil.Name = "ButtonResetPerfil"
        Me.ButtonResetPerfil.Size = New System.Drawing.Size(92, 23)
        Me.ButtonResetPerfil.TabIndex = 46
        Me.ButtonResetPerfil.Text = "Reset Perfil"
        '
        'ButtonIncidencias
        '
        Me.ButtonIncidencias.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonIncidencias.Image = CType(resources.GetObject("ButtonIncidencias.Image"), System.Drawing.Image)
        Me.ButtonIncidencias.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonIncidencias.Location = New System.Drawing.Point(862, 374)
        Me.ButtonIncidencias.Name = "ButtonIncidencias"
        Me.ButtonIncidencias.Size = New System.Drawing.Size(92, 23)
        Me.ButtonIncidencias.TabIndex = 45
        Me.ButtonIncidencias.Text = "Inciencias"
        '
        'ButtonVerErrores
        '
        Me.ButtonVerErrores.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonVerErrores.Enabled = False
        Me.ButtonVerErrores.Image = CType(resources.GetObject("ButtonVerErrores.Image"), System.Drawing.Image)
        Me.ButtonVerErrores.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonVerErrores.Location = New System.Drawing.Point(862, 446)
        Me.ButtonVerErrores.Name = "ButtonVerErrores"
        Me.ButtonVerErrores.Size = New System.Drawing.Size(91, 23)
        Me.ButtonVerErrores.TabIndex = 16
        Me.ButtonVerErrores.Text = ":::"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(168, 350)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(24, 24)
        Me.PictureBox1.TabIndex = 15
        Me.PictureBox1.TabStop = False
        '
        'ButtonImprimir
        '
        Me.ButtonImprimir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonImprimir.BackColor = System.Drawing.Color.Maroon
        Me.ButtonImprimir.ForeColor = System.Drawing.Color.White
        Me.ButtonImprimir.Image = CType(resources.GetObject("ButtonImprimir.Image"), System.Drawing.Image)
        Me.ButtonImprimir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonImprimir.Location = New System.Drawing.Point(862, 201)
        Me.ButtonImprimir.Name = "ButtonImprimir"
        Me.ButtonImprimir.Size = New System.Drawing.Size(88, 24)
        Me.ButtonImprimir.TabIndex = 12
        Me.ButtonImprimir.Text = "&Imprimir"
        Me.ButtonImprimir.UseVisualStyleBackColor = False
        '
        'FormIntegraFront
        '
        Me.AcceptButton = Me.ButtonAceptar
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(958, 539)
        Me.Controls.Add(Me.TabControlModosdeOperacion)
        Me.Controls.Add(Me.TextBoxRefresh)
        Me.Controls.Add(Me.CheckBoxRefresch)
        Me.Controls.Add(Me.ButtonResetPerfil)
        Me.Controls.Add(Me.ButtonIncidencias)
        Me.Controls.Add(Me.DateTimePicker2)
        Me.Controls.Add(Me.DataGridViewErrores)
        Me.Controls.Add(Me.CheckBoxAuditaCobrosMostrar)
        Me.Controls.Add(Me.CheckBoxAuditaCobros)
        Me.Controls.Add(Me.TextBoxDepositosSaldo)
        Me.Controls.Add(Me.TextBoxDepositosFacturados)
        Me.Controls.Add(Me.TextBoxDepositosRecibidos)
        Me.Controls.Add(Me.TextBoxSaldo)
        Me.Controls.Add(Me.TextBoxFacturacion)
        Me.Controls.Add(Me.TextBoxProduccion)
        Me.Controls.Add(Me.CheckBoxMinimizar)
        Me.Controls.Add(Me.ButtonConvertir)
        Me.Controls.Add(Me.DataGridErrorres)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.ButtonVerErrores)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.ListBoxDebug)
        Me.Controls.Add(Me.ButtonImprimir)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.TextBoxDebug)
        Me.Controls.Add(Me.TabControlDebug)
        Me.Controls.Add(Me.DataGrid2)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.MinimumSize = New System.Drawing.Size(750, 550)
        Me.Name = "FormIntegraFront"
        Me.Text = "Integración Contable Front Office"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridHoteles, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGrid2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlDebug.ResumeLayout(False)
        Me.TabPageGrupoEmpresas.ResumeLayout(False)
        Me.TabPageGrupoEmpresas.PerformLayout()
        CType(Me.DataGridViewBonos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        CType(Me.DataGridErrorres, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridViewErrores, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlModosdeOperacion.ResumeLayout(False)
        Me.TabPage8.ResumeLayout(False)
        Me.TabPage10.ResumeLayout(False)
        Me.TabPage10.PerformLayout()
        Me.TabPage12.ResumeLayout(False)
        Me.TabPage13.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
#Region "RUTINAS"
    Private Sub EnableDisableCheckBox()
        Try
            Dim C As Control
            For Each C In Me.Controls
                If TypeOf (C) Is CheckBox Then
                    If ENABLE_CHECKBOX = 1 Then
                        C.Enabled = True
                    Else
                        C.Enabled = False
                    End If
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub GeneraFileVitalSuites()
        Try

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

#End Region


#Region "LOAD"

    Private Sub FormIntegraFront_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If IsNothing(Me.DbLeeNewHotel) = False Then
                If Me.DbLeeNewHotel.EstadoConexion = ConnectionState.Open Then
                    Me.DbLeeNewHotel.CerrarConexion()
                End If
            End If

            If IsNothing(Me.DbLee) = False Then
                If Me.DbLee.EstadoConexion = ConnectionState.Open Then
                    Me.DbLee.CerrarConexion()
                End If
            End If



        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub



    Private Sub FormTest_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            Me.mTituloForm = Me.Text

            Me.DateTimePicker1.Value = (DateAdd(DateInterval.Day, -1, CType(Format(Now, "dd/MM/yyyy"), Date)))
            Me.DateTimePicker2.Value = (DateAdd(DateInterval.Day, -1, CType(Format(Now, "dd/MM/yyyy"), Date)))


            '---------------------------------------------------------------------------------------------------
            ' Conecta con la Base de Datos "central"
            '----------------------------------------------------------------------------------------------------


            Me.DbLee = New C_DATOS.C_DatosOledb(MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"))
            Me.DbLee.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")



            '---------------------------------------------------------------------------------------------------
            ' Lee y muestra algunos parametros de Integracion
            '----------------------------------------------------------------------------------------------------
            Me.mEmpGrupoCod = MyIni.IniGet(Application.StartupPath & "\menu.ini", "PARAMETER", "PARA_EMPGRUPO_COD")



            If Me.mEmpGrupoCod = "" Then
                MsgBox("No se ha podido leer Grupo de Empresas en Fichero Ini", MsgBoxStyle.Information, "Atención")
            End If



            If MULTIFECHA = "1" Then
                Me.DateTimePicker2.Enabled = True
            Else
                Me.DateTimePicker2.Enabled = False
            End If



            If MINIMIZA = "1" Then
                Me.CheckBoxMinimizar.Checked = True
                Me.CheckBoxMinimizar.Update()
            End If


            If Me.mEmpGrupoCod = "SATO" Then
                Me.ButtonActBookId.Enabled = True
            Else
                Me.ButtonActBookId.Enabled = False
            End If



            If Me.mEmpGrupoCod = "PTRO" Or Me.mEmpGrupoCod = "GRTI" Or Me.mEmpGrupoCod = "GRVI" Then
                Me.CheckBoxAuditaCobros.Checked = False
            Else
                Me.CheckBoxAuditaCobros.Checked = True
            End If

            ' If Me.mEmpGrupoCod = "SATO" Then
            'Me.ButtonSatocanLibroIgic.Visible = True
            'Else
            '   Me.ButtonSatocanLibroIgic.Visible = False
            'End If

            'SQL = "SELECT PARA_MULTIHOTEL FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            'Me.DbLee.TraerLector(SQL)


            '---------------------------------------------------------------------------------------------------
            ' Muestra/Pide Hotel a Integrar  
            '----------------------------------------------------------------------------------------------------


            If EMP_COD = "0" Then
                SQL = "SELECT HOTEL_EMPGRUPO_COD,HOTEL_EMP_COD,HOTEL_DESCRIPCION,HOTEL_ODBC,HOTEL_SPYRO,HOTEL_ODBC_NEWGOLF,HOTEL_ODBC_NEWPOS,HOTEL_EMP_NUM,DECODE(NVL(PARA_TRATA_CAJA,0),0,'FALSE',1,'TRUE') AS PARA_TRATA_CAJA FROM TH_HOTEL,TH_PARA "

                SQL += " WHERE HOTEL_EMPGRUPO_COD = PARA_EMPGRUPO_COD"
                SQL += " AND   HOTEL_EMP_COD = PARA_EMP_COD"
                SQL += " AND   HOTEL_EMP_NUM = PARA_EMP_NUM"
                SQL += " AND   HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND HOTEL_INT_NEWH = 1 "
                SQL += " ORDER BY HOTEL_DESCRIPCION ASC"
            Else
                SQL = "SELECT HOTEL_EMPGRUPO_COD,HOTEL_EMP_COD,HOTEL_DESCRIPCION,HOTEL_ODBC,HOTEL_SPYRO,HOTEL_ODBC_NEWGOLF,HOTEL_ODBC_NEWPOS,HOTEL_EMP_NUM,DECODE(NVL(PARA_TRATA_CAJA,0),0,'FALSE',1,'TRUE') AS PARA_TRATA_CAJA  FROM TH_HOTEL,TH_PARA "
                SQL += " WHERE HOTEL_EMPGRUPO_COD = PARA_EMPGRUPO_COD"
                SQL += " AND   HOTEL_EMP_COD = PARA_EMP_COD"
                SQL += " AND   HOTEL_EMP_NUM = PARA_EMP_NUM"

                SQL += " AND  HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND  HOTEL_EMP_COD = '" & EMP_COD & "'"
                SQL += " AND HOTEL_INT_NEWH = 1 "
                SQL += " ORDER BY HOTEL_DESCRIPCION ASC"

            End If

            Me.DataGridHoteles.DataSource = Me.DbLee.TraerDataset(SQL, "HOTELES")
            Me.DataGridHoteles.DataMember = "HOTELES"

            Me.ConfGriHoteles()

            If Me.DbLee.mDbDataset.Tables("HOTELES").Rows.Count > 0 Then
                Me.HayRegistros = True
                Me.DataGrid2.CaptionText = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2)
                Me.DataGridHoteles.Select(0)

                Me.mEmpGrupoCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0)
                Me.mEmpCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1)
                Me.mEmpNum = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 7)

                SQL = "SELECT NVL(PARA_FILE_SPYRO_PATH,'?') FROM TH_PARA "
                SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
                Me.TextBoxRutaFicheros.Text = Me.DbLee.EjecutaSqlScalar(SQL)


                SQL = "SELECT NVL(PARA_TRATA_TPV,'1') FROM TH_PARA "
                SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
                If Me.DbLee.EjecutaSqlScalar(SQL) = "1" Then
                    Me.CheckBoxTpvNoFacturado.Checked = True
                Else
                    Me.CheckBoxTpvNoFacturado.Checked = False
                End If



                Me.BuscaFechaFrontdeCierre()

                If IsDate(Me.mFechaFront) Then
                    Me.DateTimePicker1.Value = Format(Me.mFechaFront, "dd/MM/yyyy")
                    Me.DateTimePicker2.Value = Format(Me.mFechaFront, "dd/MM/yyyy")
                End If





            Else
                Me.HayRegistros = False

            End If

            'Me.TabControlDebug.SelectedTab = Me.TabPage3
            'Me.TabControlDebug.Update()

            Me.EstaLoad = True


            Me.EnableDisableCheckBox()
        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Exclamation, "Load")
        End Try

    End Sub
#End Region

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try




            ' control para no preocesar fechas superiores a la fecha de newhotel
            If Me.ControlFechaProcesar = False Then
                Exit Sub
            End If

            DLL = MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DLL", "FRONT")
            If DLL = 6 Then
                Me.ButtonAceptar.Visible = False
                Me.ButtonAceptar.Update()

            End If





            If Me.EstaLoad = True Then
                Me.LimpiaGrid()
            End If

            Me.Cursor = Cursors.WaitCursor
            Me.Update()

            Dim Pendiente As String

            Pendiente = "Pat= 0001 " & vbCrLf
            Pendiente += "       Revisar pasar Número de Empresa para Leer parametros en todas las dll que lean TH_PARA" & vbCrLf & vbCrLf

            Pendiente += "Pat= 0002 " & vbCrLf
            Pendiente += "       Falta Tratamiento de AC y FV en Notas de Crédito igual que se hace en Facturas " & vbCrLf & vbCrLf

            Pendiente += "Pat= 0003 " & vbCrLf
            Pendiente += "       Detalle de Impuesto de Notas de Credito es Lento !!" & vbCrLf & vbCrLf

            Pendiente += "Pat= 0004 " & vbCrLf
            Pendiente += "       Revisar Rutina BorraRegistros y Ajuste Decimal para que tenga en cuenta TH_ASNT.ASNT_EMP_NUM !!" & vbCrLf & vbCrLf

            Pendiente += "Pat= 0005 " & vbCrLf
            Pendiente += "       Revisar  PREVALECE EL NIF PUESTO A MANO POR EL USUARIO  ojo solo si no es un zero " & vbCrLf & vbCrLf


            Pendiente += " Pat= 0006 " & vbCrLf
            Pendiente += "         Comentarte que revisando la contabilización de septiembre, hemos visto que los bonos de la asociación hay que mandarlo como ""NO BONOS"". Aunque la palabra confunde, para nosotros, los bonos de la asociación contabilizan como una factura normal, por lo que cuando envíes la producción de dichos bonos hay que ponerlos en la de artículos normales, no en la de bonos. " & vbCrLf
            Pendiente += "         Ya no avisas cuando lo cambies, y cualquier cosa nos comentas," & vbCrLf


            Pendiente += "Pat= 0007 " & vbCrLf
            Pendiente += "       Control que Fecha de Enlace siempre menor a fecha de Trabajo de la Aplicación  " & vbCrLf & vbCrLf

            Pendiente += "Pat= 0008 " & vbCrLf
            Pendiente += "       Los Cobros del Sahara Playa No estan Centralizados ( buscar las cuentas en NewHotel , o Newconta " & vbCrLf & vbCrLf




            Pendiente += "Pat= AX0009 " & vbCrLf
            Pendiente += " Explicar a Domingo porque el dia 24 de octubre hay un anticipo de 1180 euros del tipo nrec , ojo ver asientos transferidos  newhotel   " & vbCrLf & vbCrLf

            Pendiente += "Pat= AX0013 " & vbCrLf
            Pendiente += " Haciendo los cuadres tenemos incidencia con los cobros de facturas con anticipos realizados con efectivo. Necesitamos que hagas el siguiente cambio:   " & vbCrLf & vbCrLf
            Pendiente += "Cuando una factura se cobra mediante un anticipo que fue realizado en efectivo o tarjeta, necesitamos que la forma de cobro que se pongan en estas líneas sea la original del anticipo. Actualmente, a todas las facturas que se cobran vía anticipo nos pones como forma de pago NH_ANTI. Lo que necesita es que si el ancipo fue realizado en efectivo (NH_EFCTV) o tarjeta (cualquiera de sus modalidades), en el cobro de la factura lo ponga. Para el resto de cobros vía anticipo (los que son por transferencia agencia) que siga poniendo NH_ANITI."


            Pendiente += "Pat= 0014 " & vbCrLf
            Pendiente += "   Pasar Cobros de Contado a Spyro Hoteles Lopez  Molde Tahona   " & vbCrLf & vbCrLf


            Pendiente += "Pat= 0015 " & vbCrLf
            Pendiente += "   SATOCAN SE AÑASE TRATAMIENTO DE ARTICULO ARV POR PARAMETRO`PARA ENVIO DE FACTURAS QUE SOLO TIENEN MOVIMIENTOS DE DEBITOS  " & vbCrLf & vbCrLf



            If SHOW_FALTA = 1 Then
                MsgBox(Pendiente, MsgBoxStyle.Information, "Falta Desarrollo")
                Me.Refresh()
                Me.Update()
            End If


            '  MsgBox("falta asiento de notas de credito cobros , como se hace en facturacion cobros para caso de notas de abono de facturas que tenian debitos", MsgBoxStyle.Information, "Pendiente yo")
            '  MsgBox("REVISAR CUENTA/ CIF EN LIBRO DE IMPUESTO")


            Me.TextBoxDebug.BackColor = System.Drawing.SystemColors.ActiveCaption
            Me.TextBoxDebug.Update()






            DLL = MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DLL", "FRONT")
            Dim INTEGRA As Object

            ' ACTIVA HILO ASINCRONO QUE MUESTRA QUE ESTA TRABAJANDO 
            ' AUN EN DESARROLLO
            ' KeepAliveDelegate = AddressOf RefrescoMientras
            'Me.RefrescoStart()



            If DLL <> 3 Then
                If Me.CheckBoxMinimizar.Checked = True Then
                    Me.ParentForm.WindowState = FormWindowState.Minimized
                End If
            End If



            If Me.HayRegistros = True Then


                ' Me.mEmpGrupoCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0)
                ' Me.mEmpCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1)

                If DLL = 1 Then
                    INTEGRA = New Integracion.IntegraFront(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                                Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                                MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                                Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "F" &
                                Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                                Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                                Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked)
                    INTEGRA.Procesar()

                End If

                If DLL = 2 Then
                    INTEGRA = New Satocan.Satocan(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                                 Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                                 MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                                 Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "F" &
                                 Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                                 Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                                 Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked, Me.CheckBoxIncluyeBonos.Checked)

                    If Me.CheckBoxSoloBonos.Checked Then
                        INTEGRA.ProcesarSoloBonos()
                    Else
                        INTEGRA.Procesar()
                    End If
                    ' solo para debug
                    Me.TextBoxProduccion.Text = Me.TextBoxProduccion.Text + INTEGRA.TotalProduccion
                    Me.TextBoxFacturacion.Text = Me.TextBoxFacturacion.Text + INTEGRA.TotalFacturado
                    Me.TextBoxSaldo.Text = Me.TextBoxProduccion.Text - Me.TextBoxFacturacion.Text

                    Me.TextBoxDepositosRecibidos.Text = Me.TextBoxDepositosRecibidos.Text + INTEGRA.AnticiposRecibidos
                    Me.TextBoxDepositosFacturados.Text = Me.TextBoxDepositosFacturados.Text + INTEGRA.CancelacionAnticipos
                    Me.TextBoxDepositosSaldo.Text = Me.TextBoxDepositosRecibidos.Text - Me.TextBoxDepositosFacturados.Text

                End If

                If DLL = 3 Then

                    If PERFILCONTABLE = "" Then
                        Dim Formp As New FormPerfilEnvioLopez
                        Formp.StartPosition = FormStartPosition.CenterScreen
                        Formp.ShowDialog()

                        Me.Cursor = Cursors.AppStarting
                        Me.Update()
                    End If

                    If MULTIFECHA = "1" Then
                        Me.ProcesarMultiLopez(Me.DateTimePicker1.Value, Me.DateTimePicker2.Value, 3)
                    Else

                        Me.DllHLopezFront = New Hlopez2.Hlopez2(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                                    Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                                   MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                                  Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "F" &
                                  Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                                  Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                                  Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked, Me.mEmpNum, Me, MUESTRAINCIDENCIAS, PERFILCONTABLE, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 8))

                        If Me.CheckBoxMinimizar.Checked = True Then
                            Me.ParentForm.WindowState = FormWindowState.Minimized
                        End If

                        Me.DllHLopezFront.Procesar()
                        Me.DllHLopezFront.CierraConexiones()

                    End If
                End If

                If DLL = 4 Then

                    If MULTIFECHA = "1" Then
                        Me.ProcesarMultiEreza(Me.DateTimePicker1.Value, Me.DateTimePicker2.Value, 3)
                    Else

                        Me.DllErezaFront = New Ereza.Ereza(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                                     Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                                     MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                                     Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "F" &
                                     Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                                     Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                                     Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked)
                        Me.DllErezaFront.Procesar()
                    End If
                End If


                If DLL = 5 Then
                    INTEGRA = New Tahona.Tahona(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                                 Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                                 MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                                 Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "F" &
                                 Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                                 Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                                 Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked, False)
                    INTEGRA.Procesar()
                End If



                If DLL = 6 Then
                    Me.mProcId = Guid.NewGuid().ToString()
                    DllaxaptaFront = New Axapta.Axapta(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                                 Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                                 MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                                 Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "F" &
                                 Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                                 Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                                 Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked, Me.CheckBoxIncluyeBonos.Checked, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 6), Me.CheckBoxDebug2.Checked, Me.CheckBoxDevoluciones.Checked, True, TIPOREDONDEO, Me.mProcId)

                    If DllaxaptaFront.P_Existenregistros = False Then
                        If Me.CheckBoxSoloBonos.Checked Then
                            DllaxaptaFront.ProcesarSoloBonos()
                        Else
                            Me.mEstaProcesadando = True
                            DllaxaptaFront.Procesar()
                            Me.mEstaProcesadando = False

                            SQL = "DELETE  TG_CONTROL WHERE CONTROL_DATA = '"
                            SQL += Format(Me.DateTimePicker1.Value, "dd/MM/yyyy") & "'"
                            SQL += " AND CONTROL_ID= '" & Me.mProcId & "'"
                            SQL += " AND CONTROL_TIPO = 'NewHotel'"

                            Me.DbLee.EjecutaSqlCommit(SQL)

                        End If
                    End If

                    ' solo para debug
                    Me.TextBoxProduccion.Text = Me.TextBoxProduccion.Text + DllaxaptaFront.TotalProduccion
                    Me.TextBoxFacturacion.Text = Me.TextBoxFacturacion.Text + DllaxaptaFront.TotalFacturado
                    Me.TextBoxSaldo.Text = Me.TextBoxProduccion.Text - Me.TextBoxFacturacion.Text

                    Me.TextBoxDepositosRecibidos.Text = Me.TextBoxDepositosRecibidos.Text + DllaxaptaFront.AnticiposRecibidos
                    Me.TextBoxDepositosFacturados.Text = Me.TextBoxDepositosFacturados.Text + DllaxaptaFront.CancelacionAnticipos
                    Me.TextBoxDepositosSaldo.Text = Me.TextBoxDepositosRecibidos.Text - Me.TextBoxDepositosFacturados.Text

                End If


                If DLL = 7 Then

                    'If PERFILCONTABLE = "" Then
                    'Dim Formp As New FormPerfilEnvioLopez
                    'Formp.StartPosition = FormStartPosition.CenterScreen
                    'Formp.ShowDialog()

                    'Me.Cursor = Cursors.AppStarting
                    'Me.Update()
                    'End If

                    If MULTIFECHA = "1" Then
                        Me.ProcesarHC7(Me.DateTimePicker1.Value, Me.DateTimePicker2.Value, 3)
                    Else

                        Me.DllHC7Front = New HC7.HC7(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                                 Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                                 MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                                 Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "F" &
                                 Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                                 Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                                 Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked)
                        If Me.CheckBoxMinimizar.Checked = True Then
                            Me.ParentForm.WindowState = FormWindowState.Minimized
                        End If

                        Me.DllHC7Front.Procesar()
                        Me.DllHC7Front.CierraConexiones()


                    End If
                End If

                If DLL = 8 Then
                    ' Parque Tropical 

                    If MULTIFECHA = "1" Then
                        Me.ProcesarMultiTropical(Me.DateTimePicker1.Value, Me.DateTimePicker2.Value, 3)
                    Else
                        Me.DllTropical = New Ptropical.Ptropical(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                                     Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                                     MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                                     Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "F" &
                                     Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                                     Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                                     Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked, False)

                        If Me.CheckBoxMinimizar.Checked = True Then
                            Me.ParentForm.WindowState = FormWindowState.Minimized
                        End If

                        Me.DllTropical.Procesar()

                    End If

                End If





                If DLL = 10 Then
                    ' hoteles dunas 2
                    If PERFILCONTABLE = "" Then
                        Dim Formp As New FormPerfilEnvioLopez
                        Formp.StartPosition = FormStartPosition.CenterScreen
                        Formp.ShowDialog()

                        Me.Cursor = Cursors.AppStarting
                        Me.Update()
                    End If

                    If MULTIFECHA = "1" Then
                        Me.ProcesarMultiDunas(Me.DateTimePicker1.Value, Me.DateTimePicker2.Value, 3)
                    Else

                        Me.DllDunas2 = New Hoteles_Dunas2.Dunas2(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                                    Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                                   MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                                  Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "F" &
                                  Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                                  Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                                  Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked, Me.mEmpNum, Me,
                                  MUESTRAINCIDENCIAS, PERFILCONTABLE, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 8), CUENTA_OPE_CAJA)

                        If Me.CheckBoxMinimizar.Checked = True Then
                            Me.ParentForm.WindowState = FormWindowState.Minimized
                        End If

                        Me.DllDunas2.Procesar()
                        Me.DllDunas2.CierraConexiones()

                    End If
                End If



                If DLL = 11 Then
                    PERFILCONTABLE = "AMBOS"
                    ' CONTANET

                    If MULTIFECHA = "1" Then
                        '      Me.ProcesarMultiDunas(Me.DateTimePicker1.Value, Me.DateTimePicker2.Value, 3)
                    Else

                        Me.DllContanet = New ContanetNewHotel.ContaNetNewHotel(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                                    Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                                   MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                                  Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "F" &
                                  Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                                  Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                                  Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked, Me.mEmpNum, Me,
                                  MUESTRAINCIDENCIAS, PERFILCONTABLE, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 8), CUENTA_OPE_CAJA)

                        If Me.CheckBoxMinimizar.Checked = True Then
                            Me.ParentForm.WindowState = FormWindowState.Minimized
                        End If

                        Me.DllContanet.Procesar()
                        Me.DllContanet.CierraConexiones()

                    End If
                End If


                If DLL = 12 Then
                    ' navision 2016
                    MUESTRAINCIDENCIAS = True

                    If MULTIFECHA = "1" Then
                        Me.ProcesarMultiTito(Me.DateTimePicker1.Value, Me.DateTimePicker2.Value, 3)
                    Else

                        Me.DllHTito = New HTitoNewHotel.HTitoNewHotel(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                                    Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                                   MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                                  Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "F" &
                                  Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                                  Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                                  Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked, Me.mEmpNum, Me, MUESTRAINCIDENCIAS, PERFILCONTABLE, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 8))



                        If Me.DllHTito.P_Existenregistros = False Then
                            Me.DllHTito.Procesar()
                            Me.DllHTito.CierraConexiones()
                        End If


                        ' LLAMAR A LOS WEBSERVICES
                        '   PDTE: Ojo LLamar si no hay error de cuentas etc ...

                        If InputBox("Enviar", "Enviar", "0") = "1" Then
                            Me.Cursor = Cursors.WaitCursor
                            Dim EnviaTito As New HTitoNewHotelEnviar.HTitoNewHotelEnviar(Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0), Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1), Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 7), "Navision2016.txt", Me.TextBoxRutaFicheros.Text, mEnumTipoEnvio.FrontOffice)
                            Me.Cursor = Cursors.Default
                        End If
                    End If
                End If



                If DLL = 13 Then
                    ' VITAL SUITES
                    'If PERFILCONTABLE = "" Then
                    'Dim Formp As New FormPerfilEnvioLopez
                    'Formp.StartPosition = FormStartPosition.CenterScreen
                    'Formp.ShowDialog()

                    'Me.Cursor = Cursors.AppStarting
                    'Me.Update()
                    'End If

                    PERFILCONTABLE = "AMBOS"
                    If MULTIFECHA = "1" Then
                        Me.ProcesarMultiVital(Me.DateTimePicker1.Value, Me.DateTimePicker2.Value, 3)
                    Else

                        Me.DllHVital = New VitalNewHotel.VitalNewHotel(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                                    Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                                   MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                                  Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "F" &
                                  Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                                  Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                                  Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked, Me.mEmpNum, Me,
                                  MUESTRAINCIDENCIAS, PERFILCONTABLE, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 8), CUENTA_OPE_CAJA)

                        If Me.CheckBoxMinimizar.Checked = True Then
                            Me.ParentForm.WindowState = FormWindowState.Minimized
                        End If

                        Me.DllHVital.Procesar()
                        Me.DllHVital.CierraConexiones()

                    End If
                End If




                Me.ListBoxDebug.Items.Clear()
                Me.ListBoxDebug.Update()


                Me.mStatusBar.Panels(3).Text = "Procesando ...."
                Me.mStatusBar.Update()


                '     Me.TabControlDebug.SelectedTab = Me.TabPage3


                Me.TabControlDebug.Update()
                Me.DateTimePicker1.Focus()
            End If


            Me.Text = Me.mTituloForm & " [DLL Num : " & DLL & "]"
            Me.Update()

            Me.Cursor = Cursors.Default
            Me.mStatusBar.Panels(3).Text = ""

            Me.DataGrid2.CaptionText = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2) & "  " & Me.DateTimePicker1.Value.ToLongDateString & " Con Perfil Cobtable : " & PERFILCONTABLE

            ' ASIENTOS

            If MULTIFECHA = "0" Then

                SQL = "SELECT ROUND(SUM(round(ASNT_DEBE,2)),2) FROM TH_ASNT WHERE ASNT_F_VALOR = '" & Me.DateTimePicker1.Value & "'"
                SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
                Me.TextBoxDebug.Text = Me.DbLee.EjecutaSqlScalar(SQL)

                SQL = "SELECT ROUND(SUM(round(ASNT_HABER,2)),2) FROM TH_ASNT WHERE ASNT_F_VALOR = '" & Me.DateTimePicker1.Value & "'"
                SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
                Me.TextBoxDebug.Text = Me.TextBoxDebug.Text & "   " & Me.DbLee.EjecutaSqlScalar(SQL)




                SQL = "SELECT ASNT_F_VALOR F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS TIPO,"
                SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,ASNT_NOMBRE AS OBSERVACION"
                SQL += ",ASNT_CIF AS CIF,ASNT_AUXILIAR_STRING AS AUX1,ASNT_AUXILIAR_NUMERICO AS AUX2,"
                SQL += " ASNT_CFATODIARI_COD,ASNT_RECIBO "

                If Me.mEmpGrupoCod = "GRTI" Then
                    SQL += " ,ASNT_MORA_DIMENNATURALEZA  AS ""'Dimensión G Naturaleza'"", "
                    SQL += " ASNT_MORA_DIMENDPTO AS ""'Dimensión G Departamento'"", "
                    SQL += "'" & "" & "' AS ""'Dimensión Acceso HOTEL'"" "
                End If

                SQL += "  FROM TH_ASNT "
                SQL += " WHERE ASNT_F_VALOR = '" & Me.DateTimePicker1.Value & "'"
                SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
                SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"
                Me.DataGrid2.DataSource = DbLee.TraerDataset(SQL, "ASIENTO")
                Me.DataGrid2.DataMember = "ASIENTO"
                Me.ConfGrid()
            Else
                SQL = "SELECT ROUND(SUM(round(ASNT_DEBE,2)),2) FROM TH_ASNT WHERE ASNT_F_VALOR >= '" & Me.DateTimePicker1.Value & "'"
                SQL += " AND ASNT_F_VALOR <= '" & Me.DateTimePicker2.Value & "'"
                SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
                Me.TextBoxDebug.Text = Me.DbLee.EjecutaSqlScalar(SQL)

                SQL = "SELECT ROUND(SUM(round(ASNT_HABER,2)),2) FROM TH_ASNT WHERE ASNT_F_VALOR >= '" & Me.DateTimePicker1.Value & "'"
                SQL += " AND ASNT_F_VALOR <= '" & Me.DateTimePicker2.Value & "'"
                SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
                Me.TextBoxDebug.Text = Me.TextBoxDebug.Text & "   " & Me.DbLee.EjecutaSqlScalar(SQL)




                SQL = "SELECT ASNT_F_VALOR F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS TIPO,"
                SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,ASNT_NOMBRE AS OBSERVACION"
                SQL += ",ASNT_CIF AS CIF,ASNT_AUXILIAR_STRING AS AUX1,ASNT_AUXILIAR_NUMERICO AS AUX2,"
                SQL += " ASNT_CFATODIARI_COD ,ASNT_RECIBO"

                If Me.mEmpGrupoCod = "GRTI" Then
                    SQL += " ,ASNT_MORA_DIMENNATURALEZA  AS ""'Dimensión G Naturaleza'"", "
                    SQL += " ASNT_MORA_DIMENDPTO AS ""'Dimensión G Departamento'"", "
                    SQL += "'" & "" & "' AS ""'Dimensión Acceso HOTEL'"" "
                End If

                SQL += "  FROM TH_ASNT "
                SQL += " WHERE ASNT_F_VALOR >= '" & Me.DateTimePicker1.Value & "'"
                SQL += " AND ASNT_F_VALOR <= '" & Me.DateTimePicker2.Value & "'"

                SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"

                SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum

                SQL += " ORDER BY ASNT_F_VALOR,ASNT_CFATOCAB_REFER,ASNT_LINEA ASC"
                Me.DataGrid2.DataSource = DbLee.TraerDataset(SQL, "ASIENTO")
                Me.DataGrid2.DataMember = "ASIENTO"
                Me.ConfGrid()
            End If





            ' errorres

            If DLL = 3 Or DLL = 4 Or DLL = 7 Or DLL = 13 Then
                ' HOTELES LOPEZ , hc7 ,ereza, vital suite
                If MULTIFECHA = "0" Then
                    SQL = "SELECT INCI_DATR,INCI_ORIGEN,INCI_DESCRIPCION FROM TH_INCI WHERE INCI_DATR = '" & Format(Me.DateTimePicker1.Value, "dd/MM/yyyy") & "'"
                    SQL += " AND  INCI_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                    SQL += " AND  INCI_EMP_COD = '" & Me.mEmpCod & "'"
                    SQL += " AND  INCI_EMP_NUM = " & Me.mEmpNum
                    SQL += " GROUP BY INCI_DATR,INCI_ORIGEN,INCI_DESCRIPCION "
                    SQL += " ORDER BY INCI_DATR ASC"
                Else
                    SQL = "SELECT INCI_DATR,INCI_ORIGEN,INCI_DESCRIPCION FROM TH_INCI WHERE INCI_DATR >= '" & Format(Me.DateTimePicker1.Value, "dd/MM/yyyy") & "'"
                    SQL += " AND INCI_DATR <= '" & Format(Me.DateTimePicker2.Value, "dd/MM/yyyy") & "'"
                    SQL += " AND  INCI_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                    SQL += " AND  INCI_EMP_COD = '" & Me.mEmpCod & "'"
                    SQL += " AND  INCI_EMP_NUM = " & Me.mEmpNum
                    SQL += " GROUP BY INCI_DATR,INCI_ORIGEN,INCI_DESCRIPCION "
                    SQL += " ORDER BY INCI_DATR ASC"

                End If
            Else
                ' RESTO DE CLIENTES
                SQL = "SELECT ERRO_F_ATOCAB,ERRO_CBATOCAB_REFER,ERRO_LINEA,ERRO_DESCRIPCION FROM TH_ERRO WHERE ERRO_F_ATOCAB = '" & Me.DateTimePicker1.Value & "'"
                SQL += " ORDER BY ERRO_CBATOCAB_REFER,ERRO_LINEA ASC"

            End If


            Me.DataGridErrorres.DataSource = DbLee.TraerDataset(SQL, "ERRORES")
            Me.DataGridErrorres.DataMember = "ERRORES"


            If DLL = 3 Or DLL = 4 Or DLL = 7 Or DLL = 13 Then
                ' HOTELES LOPEZ , hc7 ,ereza, vital suite
                Me.ConfGridIncidencias()
            Else
                Me.ConfGridError()
            End If



            If Me.CheckBoxMinimizar.Checked = True Then
                Me.ParentForm.WindowState = FormWindowState.Maximized
            End If

            ' 
            ' Alerta
            If Me.DbLee.mDbDataset.Tables("ERRORES").Rows.Count > 0 Then

                Dim Texto As String
                Texto = "Atención existen Alertas en el Proceso de Validación de Cuentas" & vbCrLf & vbCrLf
                Texto += " No integre el Fichero en la Gestión Contable aún"
                If DLL <> 6 Then
                    MsgBox(Texto, MsgBoxStyle.Information, "Atención")
                    Me.Update()
                    Me.ButtonConvertir.Enabled = False
                Else
                    Me.ButtonConvertir.Enabled = True
                End If

            Else
                Me.ButtonConvertir.Enabled = True
                If DLL = 8 Or DLL = 13 Then
                    Me.Convertir()
                End If
            End If


            If Me.mEmpGrupoCod = "DUNA" Then
                Exit Sub
            End If





            If Me.CheckBoxAuditaCobros.Checked = True Then
                Me.Cursor = Cursors.AppStarting
                Me.DbLeeNewHotel = New C_DATOS.C_DatosOledb(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3))
                Me.DbLeeNewHotel.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")
                Me.AuditaCobros()
                Me.DbLeeNewHotel.CerrarConexion()
                Me.Cursor = Cursors.Default
            End If

            '   MsgBox("Fin del Proceso")

        Catch EX As Exception
            Me.Cursor = Cursors.Default
            MsgBox(EX.Message, MsgBoxStyle.Exclamation, "Aceptar")
        Finally
            Me.Cursor = Cursors.Default
            If DLL = 6 Then
                Me.ButtonAceptar.Visible = True
                Me.ButtonAceptar.Update()
            End If


        End Try
    End Sub

    Private Sub ProcesarMultiLopez(ByVal vFecha1 As Date, ByVal vFecha2 As Date, ByVal vDllNum As Integer)
        Try

            '  Dim INTEGRA As Object

            Dim Fecha As Date = vFecha1
            While Fecha <= vFecha2

                If vFecha2 < vFecha1 Then
                    MsgBox("Fechas No Válidas", MsgBoxStyle.Information, "Atención")
                    Exit Sub
                End If

                Me.DllHLopezFront = New Hlopez2.Hlopez2(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                          Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                         MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                         Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Fecha, "dd-MM-yyyy"), "F" &
                         Format(Fecha, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
             Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
             Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked, Me.mEmpNum, Me, MUESTRAINCIDENCIAS, PERFILCONTABLE, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 8))


                If Me.CheckBoxMinimizar.Checked = True Then
                    If Me.ParentForm.WindowState = FormWindowState.Maximized Then
                        Me.ParentForm.WindowState = FormWindowState.Minimized
                    End If

                End If

                Me.DllHLopezFront.Procesar()
                Fecha = DateAdd(DateInterval.Day, 1, Fecha)

            End While




            ' SI MULTIFILE = 0 AGRUPA TODOS LOS FICHEROS GENERADOS EN UNO SOLO 
            Dim FicheroMultiFecha As System.IO.StreamWriter
            Dim FileNameGraba As String = Me.TextBoxRutaFicheros.Text & "F" & Format(vFecha1, "dd-MM-yyyy") & " - " & Format(vFecha2, "dd-MM-yyyy") & ".TXT"

            Dim FileLee As String
            Dim sr As System.IO.StreamReader
            Dim AuxLine As String


            FicheroMultiFecha = New System.IO.StreamWriter(FileNameGraba, False, System.Text.Encoding.ASCII)
            FicheroMultiFecha.WriteLine("")


            ' BUCLE FICCHEROS

            Fecha = vFecha1

            While Fecha <= vFecha2
                FileLee = Me.TextBoxRutaFicheros.Text & "F" & Format(Fecha, "dd-MM-yyyy") & ".TXT"

                sr = New System.IO.StreamReader(FileLee)

                Do While sr.Peek() >= 0
                    AuxLine = sr.ReadLine
                    If AuxLine.Length > 0 Then
                        FicheroMultiFecha.WriteLine(AuxLine)

                        Me.TextBoxDebug.Text = AuxLine
                        Me.TextBoxDebug.Update()
                    End If
                Loop
                sr.Close()
                Fecha = DateAdd(DateInterval.Day, 1, Fecha)
            End While

            FicheroMultiFecha.Close()



            ' borrar ficheros 

            Fecha = vFecha1

            While Fecha <= vFecha2
                FileLee = Me.TextBoxRutaFicheros.Text & "F" & Format(Fecha, "dd-MM-yyyy") & ".TXT"


                If System.IO.File.Exists(FileLee) = True Then
                    System.IO.File.Delete(FileLee)
                End If


                Fecha = DateAdd(DateInterval.Day, 1, Fecha)
            End While


            If Me.CheckBoxMinimizar.Checked = True Then
                If ParentForm.WindowState = FormWindowState.Minimized Then
                    Me.ParentForm.WindowState = FormWindowState.Maximized
                End If
            End If

            MsgBox("Ficheros desde " & Format(vFecha1, "dd-MM-yyyy") & " hasta " & Format(vFecha2, "dd-MM-yyyy") & vbCrLf & " Agrupados en " & FileNameGraba, MsgBoxStyle.Information, "Atención")


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub ProcesarMultiTito(ByVal vFecha1 As Date, ByVal vFecha2 As Date, ByVal vDllNum As Integer)
        Try

            '  Dim INTEGRA As Object

            Dim Fecha As Date = vFecha1
            While Fecha <= vFecha2

                If vFecha2 < vFecha1 Then
                    MsgBox("Fechas No Válidas", MsgBoxStyle.Information, "Atención")
                    Exit Sub
                End If

                Me.DllHTito = New HTitoNewHotel.HTitoNewHotel(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                          Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                         MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                         Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Fecha, "dd-MM-yyyy"), "F" &
                         Format(Fecha, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
             Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
             Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked, Me.mEmpNum, Me, MUESTRAINCIDENCIAS, PERFILCONTABLE, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 8))


                If Me.CheckBoxMinimizar.Checked = True Then
                    If Me.ParentForm.WindowState = FormWindowState.Maximized Then
                        Me.ParentForm.WindowState = FormWindowState.Minimized
                    End If

                End If

                Me.DllHTito.Procesar()
                Me.DllHTito.CierraConexiones()
                Fecha = DateAdd(DateInterval.Day, 1, Fecha)

            End While




            ' SI MULTIFILE = 0 AGRUPA TODOS LOS FICHEROS GENERADOS EN UNO SOLO 
            Dim FicheroMultiFecha As System.IO.StreamWriter
            Dim FileNameGraba As String = Me.TextBoxRutaFicheros.Text & "F" & Format(vFecha1, "dd-MM-yyyy") & " - " & Format(vFecha2, "dd-MM-yyyy") & ".TXT"

            Dim FileLee As String
            Dim sr As System.IO.StreamReader
            Dim AuxLine As String


            FicheroMultiFecha = New System.IO.StreamWriter(FileNameGraba, False, System.Text.Encoding.ASCII)
            FicheroMultiFecha.WriteLine("")


            ' BUCLE FICCHEROS

            Fecha = vFecha1

            While Fecha <= vFecha2
                FileLee = Me.TextBoxRutaFicheros.Text & "F" & Format(Fecha, "dd-MM-yyyy") & ".TXT"

                sr = New System.IO.StreamReader(FileLee)

                Do While sr.Peek() >= 0
                    AuxLine = sr.ReadLine
                    If AuxLine.Length > 0 Then
                        FicheroMultiFecha.WriteLine(AuxLine)

                        Me.TextBoxDebug.Text = AuxLine
                        Me.TextBoxDebug.Update()
                    End If
                Loop
                sr.Close()
                Fecha = DateAdd(DateInterval.Day, 1, Fecha)
            End While

            FicheroMultiFecha.Close()



            ' borrar ficheros 

            Fecha = vFecha1

            While Fecha <= vFecha2
                FileLee = Me.TextBoxRutaFicheros.Text & "F" & Format(Fecha, "dd-MM-yyyy") & ".TXT"


                If System.IO.File.Exists(FileLee) = True Then
                    System.IO.File.Delete(FileLee)
                End If


                Fecha = DateAdd(DateInterval.Day, 1, Fecha)
            End While


            If Me.CheckBoxMinimizar.Checked = True Then
                If ParentForm.WindowState = FormWindowState.Minimized Then
                    Me.ParentForm.WindowState = FormWindowState.Maximized
                End If
            End If

            MsgBox("Ficheros desde " & Format(vFecha1, "dd-MM-yyyy") & " hasta " & Format(vFecha2, "dd-MM-yyyy") & vbCrLf & " Agrupados en " & FileNameGraba, MsgBoxStyle.Information, "Atención")


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub ProcesarMultiTropical(ByVal vFecha1 As Date, ByVal vFecha2 As Date, ByVal vDllNum As Integer)
        Try

            '  Dim INTEGRA As Object

            Dim Fecha As Date = vFecha1
            While Fecha <= vFecha2

                If vFecha2 < vFecha1 Then
                    MsgBox("Fechas No Válidas", MsgBoxStyle.Information, "Atención")
                    Exit Sub
                End If

                Me.DllTropical = New Ptropical.Ptropical(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                             Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                             MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                             Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Fecha, "dd-MM-yyyy"), "F" &
                             Format(Fecha, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                             Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                             Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked, False)

                If Me.CheckBoxMinimizar.Checked = True Then
                    Me.ParentForm.WindowState = FormWindowState.Minimized
                End If




                If Me.CheckBoxMinimizar.Checked = True Then
                    If Me.ParentForm.WindowState = FormWindowState.Maximized Then
                        Me.ParentForm.WindowState = FormWindowState.Minimized
                    End If

                End If

                Me.DllTropical.Procesar()

                Dim Form As New FormConvertir(Fecha, MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"), Me.mEmpGrupoCod, Me.mEmpCod, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), True, "NEWHOTEL")
                Form.ShowDialog()


                Fecha = DateAdd(DateInterval.Day, 1, Fecha)

            End While



        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub ProcesarMultiDunas(ByVal vFecha1 As Date, ByVal vFecha2 As Date, ByVal vDllNum As Integer)
        Try

            '  Dim INTEGRA As Object

            Dim Fecha As Date = vFecha1
            While Fecha <= vFecha2

                If vFecha2 < vFecha1 Then
                    MsgBox("Fechas No Válidas", MsgBoxStyle.Information, "Atención")
                    Exit Sub
                End If

                Me.DllDunas2 = New Hoteles_Dunas2.Dunas2(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                           Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                          MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                         Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Fecha, "dd-MM-yyyy"), "F" &
                         Format(Fecha, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                         Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                         Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked, Me.mEmpNum, Me,
                         MUESTRAINCIDENCIAS, PERFILCONTABLE, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 8), CUENTA_OPE_CAJA)

                If Me.CheckBoxMinimizar.Checked = True Then
                    Me.ParentForm.WindowState = FormWindowState.Minimized
                End If




                If Me.CheckBoxMinimizar.Checked = True Then
                    If Me.ParentForm.WindowState = FormWindowState.Maximized Then
                        Me.ParentForm.WindowState = FormWindowState.Minimized
                    End If

                End If

                Me.DllDunas2.Procesar()
                Me.DllDunas2.CierraConexiones()
                Fecha = DateAdd(DateInterval.Day, 1, Fecha)

            End While




            ' SI MULTIFILE = 0 AGRUPA TODOS LOS FICHEROS GENERADOS EN UNO SOLO 
            Dim FicheroMultiFecha As System.IO.StreamWriter
            Dim FileNameGraba As String = Me.TextBoxRutaFicheros.Text & "F" & Format(vFecha1, "dd-MM-yyyy") & " - " & Format(vFecha2, "dd-MM-yyyy") & ".TXT"

            Dim FileLee As String
            Dim sr As System.IO.StreamReader
            Dim AuxLine As String


            FicheroMultiFecha = New System.IO.StreamWriter(FileNameGraba, False, System.Text.Encoding.ASCII)
            FicheroMultiFecha.WriteLine("")


            ' BUCLE FICCHEROS

            Fecha = vFecha1

            While Fecha <= vFecha2
                FileLee = Me.TextBoxRutaFicheros.Text & "F" & Format(Fecha, "dd-MM-yyyy") & ".TXT"

                sr = New System.IO.StreamReader(FileLee)

                Do While sr.Peek() >= 0
                    AuxLine = sr.ReadLine
                    If AuxLine.Length > 0 Then
                        FicheroMultiFecha.WriteLine(AuxLine)

                        Me.TextBoxDebug.Text = AuxLine
                        Me.TextBoxDebug.Update()
                    End If
                Loop
                sr.Close()
                Fecha = DateAdd(DateInterval.Day, 1, Fecha)
            End While

            FicheroMultiFecha.Close()



            ' borrar ficheros 

            Fecha = vFecha1

            While Fecha <= vFecha2
                FileLee = Me.TextBoxRutaFicheros.Text & "F" & Format(Fecha, "dd-MM-yyyy") & ".TXT"


                If System.IO.File.Exists(FileLee) = True Then
                    System.IO.File.Delete(FileLee)
                End If


                Fecha = DateAdd(DateInterval.Day, 1, Fecha)
            End While


            If Me.CheckBoxMinimizar.Checked = True Then
                If ParentForm.WindowState = FormWindowState.Minimized Then
                    Me.ParentForm.WindowState = FormWindowState.Maximized
                End If
            End If

            '    MsgBox("Ficheros desde " & Format(vFecha1, "dd-MM-yyyy") & " hasta " & Format(vFecha2, "dd-MM-yyyy") & vbCrLf & " Agrupados en " & FileNameGraba, MsgBoxStyle.Information, "Atención")


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub ProcesarMultiVital(ByVal vFecha1 As Date, ByVal vFecha2 As Date, ByVal vDllNum As Integer)
        Try

            '  Dim INTEGRA As Object

            Dim Fecha As Date = vFecha1
            While Fecha <= vFecha2

                If vFecha2 < vFecha1 Then
                    MsgBox("Fechas No Válidas", MsgBoxStyle.Information, "Atención")
                    Exit Sub
                End If

                Me.DllHVital = New VitalNewHotel.VitalNewHotel(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                           Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                          MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                         Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Fecha, "dd-MM-yyyy"), "F" &
                         Format(Fecha, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                         Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                         Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked, Me.mEmpNum, Me,
                         MUESTRAINCIDENCIAS, PERFILCONTABLE, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 8), CUENTA_OPE_CAJA)

                If Me.CheckBoxMinimizar.Checked = True Then
                    Me.ParentForm.WindowState = FormWindowState.Minimized
                End If




                If Me.CheckBoxMinimizar.Checked = True Then
                    If Me.ParentForm.WindowState = FormWindowState.Maximized Then
                        Me.ParentForm.WindowState = FormWindowState.Minimized
                    End If

                End If

                Me.DllHVital.Procesar()
                Me.DllHVital.CierraConexiones()
                Fecha = DateAdd(DateInterval.Day, 1, Fecha)

            End While







            If Me.CheckBoxMinimizar.Checked = True Then
                If ParentForm.WindowState = FormWindowState.Minimized Then
                    Me.ParentForm.WindowState = FormWindowState.Maximized
                End If
            End If

            '    MsgBox("Ficheros desde " & Format(vFecha1, "dd-MM-yyyy") & " hasta " & Format(vFecha2, "dd-MM-yyyy") & vbCrLf & " Agrupados en " & FileNameGraba, MsgBoxStyle.Information, "Atención")


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub ProcesarMultiEreza(ByVal vFecha1 As Date, ByVal vFecha2 As Date, ByVal vDllNum As Integer)
        Try

            Dim Fecha As Date = vFecha1
            While Fecha <= vFecha2

                If vFecha2 < vFecha1 Then
                    MsgBox("Fechas No Válidas", MsgBoxStyle.Information, "Atención")
                    Exit Sub
                End If

                Me.DllErezaFront = New Ereza.Ereza(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                             Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                             MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                             Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Fecha, "dd-MM-yyyy"), "F" &
                             Format(Fecha, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                             Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                             Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked)

                If Me.CheckBoxMinimizar.Checked = True Then
                    If Me.ParentForm.WindowState = FormWindowState.Maximized Then
                        Me.ParentForm.WindowState = FormWindowState.Minimized
                    End If

                End If

                Me.DllErezaFront.Procesar()
                Fecha = DateAdd(DateInterval.Day, 1, Fecha)

            End While




            ' SI MULTIFILE = 0 AGRUPA TODOS LOS FICHEROS GENERADOS EN UNO SOLO 
            Dim FicheroMultiFecha As System.IO.StreamWriter
            Dim FileNameGraba As String = Me.TextBoxRutaFicheros.Text & "F" & Format(vFecha1, "dd-MM-yyyy") & " - " & Format(vFecha2, "dd-MM-yyyy") & ".TXT"

            Dim FileLee As String
            Dim sr As System.IO.StreamReader
            Dim AuxLine As String


            FicheroMultiFecha = New System.IO.StreamWriter(FileNameGraba, False, System.Text.Encoding.ASCII)
            FicheroMultiFecha.WriteLine("")


            ' BUCLE FICCHEROS

            Fecha = vFecha1

            While Fecha <= vFecha2
                FileLee = Me.TextBoxRutaFicheros.Text & "F" & Format(Fecha, "dd-MM-yyyy") & ".TXT"

                sr = New System.IO.StreamReader(FileLee)

                Do While sr.Peek() >= 0
                    AuxLine = sr.ReadLine
                    If AuxLine.Length > 0 Then
                        FicheroMultiFecha.WriteLine(AuxLine)

                        Me.TextBoxDebug.Text = AuxLine
                        Me.TextBoxDebug.Update()
                    End If
                Loop
                sr.Close()
                Fecha = DateAdd(DateInterval.Day, 1, Fecha)
            End While

            FicheroMultiFecha.Close()



            ' borrar ficheros 

            Fecha = vFecha1

            While Fecha <= vFecha2
                FileLee = Me.TextBoxRutaFicheros.Text & "F" & Format(Fecha, "dd-MM-yyyy") & ".TXT"


                If System.IO.File.Exists(FileLee) = True Then
                    System.IO.File.Delete(FileLee)
                End If


                Fecha = DateAdd(DateInterval.Day, 1, Fecha)
            End While


            If Me.CheckBoxMinimizar.Checked = True Then
                If ParentForm.WindowState = FormWindowState.Minimized Then
                    Me.ParentForm.WindowState = FormWindowState.Maximized
                End If
            End If

            ' se borra tambien este fichero en este cliente NO se usa 
            If System.IO.File.Exists(FileNameGraba) = True Then
                System.IO.File.Delete(FileNameGraba)
            End If

            '   MsgBox("Ficheros desde " & Format(vFecha1, "dd-MM-yyyy") & " hasta " & Format(vFecha2, "dd-MM-yyyy") & vbCrLf & " Agrupados en " & FileNameGraba, MsgBoxStyle.Information, "Atención")


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ProcesarHC7(ByVal vFecha1 As Date, ByVal vFecha2 As Date, ByVal vDllNum As Integer)
        Try

            '  Dim INTEGRA As Object

            Dim Fecha As Date = vFecha1
            While Fecha <= vFecha2

                If vFecha2 < vFecha1 Then
                    MsgBox("Fechas No Válidas", MsgBoxStyle.Information, "Atención")
                    Exit Sub
                End If


                Me.DllHC7Front = New HC7.HC7(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                         Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                         MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                         Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Fecha, "dd-MM-yyyy"), "F" &
                         Format(Fecha, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                         Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                         Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked)

                If Me.CheckBoxMinimizar.Checked = True Then
                    If Me.ParentForm.WindowState = FormWindowState.Maximized Then
                        Me.ParentForm.WindowState = FormWindowState.Minimized
                    End If

                End If

                Me.DllHC7Front.Procesar()
                Fecha = DateAdd(DateInterval.Day, 1, Fecha)
                Me.DllHC7Front = Nothing
            End While




            ' SI MULTIFILE = 0 AGRUPA TODOS LOS FICHEROS GENERADOS EN UNO SOLO 
            Dim FicheroMultiFecha As System.IO.StreamWriter
            Dim FileNameGraba As String = Me.TextBoxRutaFicheros.Text & "F" & Format(vFecha1, "dd-MM-yyyy") & " - " & Format(vFecha2, "dd-MM-yyyy") & ".TXT"

            Dim FileLee As String
            Dim sr As System.IO.StreamReader
            Dim AuxLine As String


            FicheroMultiFecha = New System.IO.StreamWriter(FileNameGraba, False, System.Text.Encoding.ASCII)
            FicheroMultiFecha.WriteLine("")


            ' BUCLE FICCHEROS

            Fecha = vFecha1

            While Fecha <= vFecha2
                FileLee = Me.TextBoxRutaFicheros.Text & "F" & Format(Fecha, "dd-MM-yyyy") & ".TXT"

                sr = New System.IO.StreamReader(FileLee)

                Do While sr.Peek() >= 0
                    AuxLine = sr.ReadLine
                    If AuxLine.Length > 0 Then
                        FicheroMultiFecha.WriteLine(AuxLine)

                        Me.TextBoxDebug.Text = AuxLine
                        Me.TextBoxDebug.Update()
                    End If
                Loop
                sr.Close()
                Fecha = DateAdd(DateInterval.Day, 1, Fecha)
            End While

            FicheroMultiFecha.Close()



            ' borrar ficheros 

            Fecha = vFecha1

            While Fecha <= vFecha2
                FileLee = Me.TextBoxRutaFicheros.Text & "F" & Format(Fecha, "dd-MM-yyyy") & ".TXT"


                If System.IO.File.Exists(FileLee) = True Then
                    System.IO.File.Delete(FileLee)
                End If


                Fecha = DateAdd(DateInterval.Day, 1, Fecha)
            End While


            If Me.CheckBoxMinimizar.Checked = True Then
                If ParentForm.WindowState = FormWindowState.Minimized Then
                    Me.ParentForm.WindowState = FormWindowState.Maximized
                End If
            End If

            MsgBox("Ficheros desde " & Format(vFecha1, "dd-MM-yyyy") & " hasta " & Format(vFecha2, "dd-MM-yyyy") & vbCrLf & " Agrupados en " & FileNameGraba, MsgBoxStyle.Information, "Atención")


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


#Region "DISEÑO"
    Private Sub ConfGrid()

        Try
            Dim ts1 As New DataGridTableStyle

            ts1.MappingName = "ASIENTO"

            Dim TextCol1 As New DataGridTextBoxColumn
            TextCol1.MappingName = "F_VALOR"
            TextCol1.HeaderText = "F. Valor"
            TextCol1.Width = 75

            ts1.GridColumnStyles.Add(TextCol1)


            Dim TextCol2 As New DataGridTextBoxColumn
            TextCol2.MappingName = "CUENTA"
            TextCol2.HeaderText = "Cuenta"
            TextCol2.Width = 75
            ts1.GridColumnStyles.Add(TextCol2)


            Dim TextCol2A1 As New DataGridTextBoxColumn
            TextCol2A1.MappingName = "ASNT_CFATODIARI_COD"
            TextCol2A1.HeaderText = "Diario"
            TextCol2A1.Width = 40
            ts1.GridColumnStyles.Add(TextCol2A1)



            Dim TextCol2A As New DataGridTextBoxColumn
            TextCol2A.MappingName = "Tipo"
            TextCol2A.HeaderText = "Identificador"
            TextCol2A.Width = 20
            ts1.GridColumnStyles.Add(TextCol2A)

            Dim TextCol3 As New DataGridTextBoxColumn
            TextCol3.MappingName = "CONCEPTO"
            TextCol3.HeaderText = "Concepto"
            TextCol3.Width = 250

            ts1.GridColumnStyles.Add(TextCol3)


            Dim TextCol4 As New DataGridTextBoxColumn
            TextCol4.MappingName = "DEBE"
            TextCol4.HeaderText = "Debe"
            TextCol4.Width = 100
            TextCol4.NullText = "_"
            ts1.GridColumnStyles.Add(TextCol4)


            Dim TextCol5 As New DataGridTextBoxColumn
            TextCol5.MappingName = "HABER"
            TextCol5.HeaderText = "Haber"
            TextCol5.Width = 100
            TextCol5.NullText = "_"
            ts1.GridColumnStyles.Add(TextCol5)


            Dim TextCol6 As New DataGridTextBoxColumn
            TextCol6.MappingName = "OBSERVACION"
            TextCol6.HeaderText = "Observación"
            TextCol6.Width = 300
            TextCol6.NullText = "_"
            ts1.GridColumnStyles.Add(TextCol6)

            Dim TextCol7 As New DataGridTextBoxColumn
            TextCol7.MappingName = "CIF"
            TextCol7.HeaderText = "CIF"
            TextCol7.Width = 100
            TextCol7.NullText = "_"
            ts1.GridColumnStyles.Add(TextCol7)

            Dim TextCol8 As New DataGridTextBoxColumn
            TextCol8.MappingName = "AUX1"
            TextCol8.HeaderText = "Auxiliar Str"
            TextCol8.Width = 100
            TextCol8.NullText = "_"
            ts1.GridColumnStyles.Add(TextCol8)

            Dim TextCol9 As New DataGridTextBoxColumn
            TextCol9.MappingName = "AUX2"
            TextCol9.HeaderText = "Auxiliar Num"
            TextCol9.Width = 100
            TextCol9.NullText = "_"
            ts1.GridColumnStyles.Add(TextCol9)


            Dim TextCol10 As New DataGridTextBoxColumn
            TextCol10.MappingName = "ASNT_RECIBO"
            TextCol10.HeaderText = "Recibo"
            TextCol10.Width = 100
            TextCol10.NullText = "_"
            ts1.GridColumnStyles.Add(TextCol10)



            If Me.mEmpGrupoCod = "GRTI" Then

                Dim TextCol11 As New DataGridTextBoxColumn
                TextCol11.MappingName = "Dimensión G Naturaleza"
                TextCol11.HeaderText = "Dimensión Naturaleza"
                TextCol11.Width = 100
                TextCol11.NullText = "_"
                ts1.GridColumnStyles.Add(TextCol11)

                Dim TextCol12 As New DataGridTextBoxColumn
                TextCol12.MappingName = "Dimensión G Departamento"
                TextCol12.HeaderText = "Dimensión Departamento"
                TextCol12.Width = 100
                TextCol12.NullText = "_"
                ts1.GridColumnStyles.Add(TextCol12)


                Dim TextCol13 As New DataGridTextBoxColumn
                TextCol13.MappingName = "Dimensión Acceso HOTEL"
                TextCol13.HeaderText = "Dimensión Hotel"
                TextCol13.Width = 100
                TextCol13.NullText = "_"
                ts1.GridColumnStyles.Add(TextCol13)


            End If


            '   ts1.AlternatingBackColor = Color.LightGray

            DataGrid2.TableStyles.Clear()
            DataGrid2.TableStyles.Add(ts1)




        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub ConfGridError()

        Try
            Dim ts1 As New DataGridTableStyle

            ts1.MappingName = "ERRORES"

            Dim TextCol1 As New DataGridTextBoxColumn
            TextCol1.MappingName = "ERRO_F_ATOCAB"
            TextCol1.HeaderText = "Fecha"
            TextCol1.Width = 75

            ts1.GridColumnStyles.Add(TextCol1)


            Dim TextCol2 As New DataGridTextBoxColumn
            TextCol2.MappingName = "ERRO_CBATOCAB_REFER"
            TextCol2.HeaderText = "Asiento"
            TextCol2.Width = 40
            ts1.GridColumnStyles.Add(TextCol2)


            Dim TextCol2A As New DataGridTextBoxColumn
            TextCol2A.MappingName = "ERRO_LINEA"
            TextCol2A.HeaderText = "Apunte"
            TextCol2A.Width = 40
            ts1.GridColumnStyles.Add(TextCol2A)

            Dim TextCol3 As New DataGridTextBoxColumn
            TextCol3.MappingName = "ERRO_DESCRIPCION"
            TextCol3.HeaderText = "Descripción"
            TextCol3.Width = 400

            ts1.GridColumnStyles.Add(TextCol3)




            Me.DataGridErrorres.TableStyles.Clear()
            Me.DataGridErrorres.TableStyles.Add(ts1)


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub ConfGridIncidencias()

        Try
            Dim ts1 As New DataGridTableStyle

            ts1.MappingName = "ERRORES"

            Dim TextCol1 As New DataGridTextBoxColumn
            TextCol1.MappingName = "INCI_DATR"
            TextCol1.HeaderText = "Fecha"
            TextCol1.Width = 75

            ts1.GridColumnStyles.Add(TextCol1)


            Dim TextCol2 As New DataGridTextBoxColumn
            TextCol2.MappingName = "INCI_ORIGEN"
            TextCol2.HeaderText = "Origen"
            TextCol2.Width = 100
            ts1.GridColumnStyles.Add(TextCol2)


            Dim TextCol3 As New DataGridTextBoxColumn
            TextCol3.MappingName = "INCI_DESCRIPCION"
            TextCol3.HeaderText = "Descripción"
            TextCol3.Width = 600
            ts1.GridColumnStyles.Add(TextCol3)


            Me.DataGridErrorres.TableStyles.Clear()

            Me.DataGridErrorres.TableStyles.Add(ts1)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub ConfGriHoteles()

        Try
            Dim ts1 As New DataGridTableStyle

            ts1.MappingName = "HOTELES"

            Dim TextCol1 As New DataGridTextBoxColumn
            TextCol1.MappingName = "HOTEL_EMPGRUPO_COD"
            TextCol1.HeaderText = "Grupo de Empresas"
            TextCol1.Width = 75
            ts1.GridColumnStyles.Add(TextCol1)


            Dim TextCol2 As New DataGridTextBoxColumn
            TextCol2.MappingName = "HOTEL_EMP_COD"
            TextCol2.HeaderText = "Código Empresa"
            TextCol2.Width = 75
            ts1.GridColumnStyles.Add(TextCol2)


            Dim TextCol3 As New DataGridTextBoxColumn
            TextCol3.MappingName = "HOTEL_DESCRIPCION"
            TextCol3.HeaderText = "Descripción"
            TextCol3.Width = 200
            ts1.GridColumnStyles.Add(TextCol3)

            Dim TextCol4 As New DataGridTextBoxColumn
            TextCol4.MappingName = "HOTEL_ODBC"
            TextCol4.HeaderText = "ODBC HOTEL"
            TextCol4.Width = 0
            ts1.GridColumnStyles.Add(TextCol4)

            Dim TextCol5 As New DataGridTextBoxColumn
            TextCol5.MappingName = "HOTEL_SPYRO"
            TextCol5.HeaderText = "ODBC SPYRO"
            TextCol5.Width = 0
            ts1.GridColumnStyles.Add(TextCol5)


            Dim TextCol6 As New DataGridTextBoxColumn
            TextCol6.MappingName = "HOTEL_ODBC_NEWGOLF"
            TextCol6.HeaderText = "ODBC NEWGOLF"
            TextCol6.Width = 0
            ts1.GridColumnStyles.Add(TextCol6)

            Dim TextCol7 As New DataGridTextBoxColumn
            TextCol7.MappingName = "HOTEL_ODBC_NEWPOS"
            TextCol7.HeaderText = "ODBC NEWPOS"
            TextCol7.Width = 0
            ts1.GridColumnStyles.Add(TextCol7)

            Dim TextCol8 As New DataGridTextBoxColumn
            TextCol8.MappingName = "HOTEL_EMP_NUM"
            TextCol8.HeaderText = "ESTABLECIMIENTO"
            TextCol8.Width = 75
            ts1.GridColumnStyles.Add(TextCol8)


            ' Dim TextCol9 As New DataGridTextBoxColumn
            'TextCol9.MappingName = "PARA_TRATA_CAJA"
            'TextCol9.HeaderText = "TRATA CAJA"
            'TextCol9.Width = 75
            'ts1.GridColumnStyles.Add(TextCol9)

            Dim TextCol9 As New DataGridBoolColumn
            TextCol9.MappingName = "PARA_TRATA_CAJA"
            TextCol9.HeaderText = "TRATA CAJA"

            TextCol9.FalseValue = "FALSE"
            TextCol9.TrueValue = "TRUE"

            TextCol9.Width = 75
            ts1.GridColumnStyles.Add(TextCol9)




            Me.DataGridHoteles.TableStyles.Clear()
            Me.DataGridHoteles.TableStyles.Add(ts1)





        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
#End Region

    Private Sub ButtonConvertir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonConvertir.Click

        Try
            Me.Convertir()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub Convertir()
        Try
            Dim CadenaConexion As String = MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING")


            If Me.mEmpGrupoCod = "TAHO" Then
                Dim Form As New FormConvertir(Me.DateTimePicker1.Value, CadenaConexion, Me.mEmpGrupoCod, Me.mEmpCod, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), False, "NEWHOTEL")
                Form.ShowDialog()
                Exit Sub
            End If


            ' AXAPTA WEB SERVICES
            If Me.DLL = 6 Then
                Dim Formp As New FormPerfilEnvioAx
                Formp.StartPosition = FormStartPosition.CenterScreen
                Formp.ShowDialog()

                If ESTACARGADOFORMCONVERTIR = False Then
                    ESTACARGADOFORMCONVERTIR = True
                    Dim Form As New FormConvertirAxapta(Me.DateTimePicker1.Value, CadenaConexion, Me.mEmpGrupoCod, Me.mEmpCod, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), False, PERFILCONTABLE, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3))
                    Form.MdiParent = Me.MdiParent
                    Form.Show()
                    Exit Sub
                Else
                    My.Forms.FormMenu.LayoutMdi(MdiLayout.TileHorizontal)
                    Exit Sub
                End If
            End If



            If Me.mEmpGrupoCod = "HC7" Or Me.mEmpGrupoCod = "GRVI" Then
                If MULTIFECHA = "1" Then
                    Dim Form As New FormConvertir(Me.DateTimePicker1.Value, Me.DateTimePicker2.Value, CadenaConexion, Me.mEmpGrupoCod, Me.mEmpCod, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), True, "NEWHOTEL")
                    Form.ShowDialog()
                Else
                    Dim Form As New FormConvertir(Me.DateTimePicker1.Value, CadenaConexion, Me.mEmpGrupoCod, Me.mEmpCod, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), True, "NEWHOTEL")
                    Form.ShowDialog()
                End If

                ' EREZA
            ElseIf Me.mEmpGrupoCod = "EREZ" Then
                If MULTIFECHA = "1" Then
                    Dim Form As New FormConvertir(Me.DateTimePicker1.Value, Me.DateTimePicker2.Value, CadenaConexion, Me.mEmpGrupoCod, Me.mEmpCod, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), False, "NEWHOTEL")
                    Form.ShowDialog()
                Else
                    Dim Form As New FormConvertir(Me.DateTimePicker1.Value, Me.DateTimePicker1.Value, CadenaConexion, Me.mEmpGrupoCod, Me.mEmpCod, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), False, "NEWHOTEL")
                    Form.ShowDialog()
                End If

            ElseIf Me.mEmpGrupoCod = "PTRO" Then
                Dim Form As New FormConvertir(Me.DateTimePicker1.Value, CadenaConexion, Me.mEmpGrupoCod, Me.mEmpCod, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), True, "NEWHOTEL")
                Form.ShowDialog()
            Else
                Dim Form As New FormConvertir(Me.DateTimePicker1.Value, CadenaConexion, Me.mEmpGrupoCod, Me.mEmpCod, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), False, "NEWHOTEL")
                Form.ShowDialog()

            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DataGridHoteles_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridHoteles.CurrentCellChanged

        Try
            If Me.HayRegistros = True Then
                Me.DataGrid2.CaptionText = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2)
                Me.mEmpGrupoCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0)
                Me.mEmpCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1)

                Me.mEmpNum = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 7)

                SQL = "SELECT NVL(PARA_FILE_SPYRO_PATH,'?') FROM TH_PARA "
                SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
                Me.TextBoxRutaFicheros.Text = Me.DbLee.EjecutaSqlScalar(SQL)

                SQL = "SELECT NVL(PARA_TRATA_TPV,'1') FROM TH_PARA "
                SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
                If Me.DbLee.EjecutaSqlScalar(SQL) = "1" Then
                    Me.CheckBoxTpvNoFacturado.Checked = True
                Else
                    Me.CheckBoxTpvNoFacturado.Checked = False
                End If
                Me.BuscaFechaFrontdeCierre()

                If IsDate(Me.mFechaFront) Then
                    Me.DateTimePicker1.Value = Format(Me.mFechaFront, "dd/MM/yyyy")
                    Me.DateTimePicker2.Value = Format(Me.mFechaFront, "dd/MM/yyyy")
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonSatocanLibroIgic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSatocanLibroIgic.Click
        Try
            Dim Form As New FormLibroImpuestos(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), Me.mEmpGrupoCod, Me.mEmpCod, CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), String))
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.ShowDialog()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            If Me.mEmpGrupoCod = "TAHO" Then
                REPORT_SELECTION_FORMULA = "{TH_ASNT.ASNT_F_VALOR}=DATETIME(" & Format(Me.DateTimePicker1.Value, REPORT_DATE_FORMAT) & ")"
                REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
                REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"
                REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMP_NUM}= " & Me.mEmpNum

            ElseIf Me.mEmpGrupoCod = "SATO" Then
                REPORT_SELECTION_FORMULA = "{TH_ASNT.ASNT_F_VALOR}=DATETIME(" & Format(Me.DateTimePicker1.Value, REPORT_DATE_FORMAT) & ")"
                REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
                REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"
                REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMP_NUM}= " & Me.mEmpNum
                If DLL = 6 Then
                    REPORT_SELECTION_FORMULA += " AND isnull({TH_ASNT.ASNT_WEBSERVICE_NAME}) = FALSE"
                End If


            Else

                If MULTIFECHA = 0 Then
                    REPORT_SELECTION_FORMULA = "{TH_ASNT.ASNT_F_VALOR}=DATETIME(" & Format(Me.DateTimePicker1.Value, REPORT_DATE_FORMAT) & ")"
                    REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
                    REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"
                    REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMP_NUM}= " & Me.mEmpNum
                Else
                    REPORT_SELECTION_FORMULA = "{TH_ASNT.ASNT_F_VALOR} >= DATETIME(" & Format(Me.DateTimePicker1.Value, REPORT_DATE_FORMAT) & ")"
                    REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_F_VALOR} <= DATETIME(" & Format(Me.DateTimePicker2.Value, REPORT_DATE_FORMAT) & ")"
                    REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
                    REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"
                    REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMP_NUM}= " & Me.mEmpNum

                End If
            End If


            If Me.mEmpGrupoCod = "GRHL" Then
                Dim Form As New FormVisorCrystal("ASIENTO-LOPEZ.RPT", CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), String), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), String), False, False)
                Form.MdiParent = Me.MdiParent
                Form.StartPosition = FormStartPosition.CenterScreen
                Form.Show()
                Me.Cursor = Cursors.Default

            ElseIf Me.mEmpGrupoCod = "GRTI" Then
                '   Dim Form As New FormVisorCrystal("ASIENTO-HTITO.RPT", CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), String), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), String), False, False)
                Dim Form As New FormVisorCrystal("ASIENTO-HTITO-DEBUG.RPT", CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), String), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), String), False, False)
                Form.MdiParent = Me.MdiParent
                Form.StartPosition = FormStartPosition.CenterScreen
                Form.Show()
                Me.Cursor = Cursors.Default

            ElseIf Me.mEmpGrupoCod = "DUNA" Then
                Dim Form As New FormVisorCrystal("ASIENTO-DUNAS.RPT", CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), String), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), String), False, False)
                Form.MdiParent = Me.MdiParent
                Form.StartPosition = FormStartPosition.CenterScreen
                Form.Show()
                Me.Cursor = Cursors.Default

            ElseIf Me.mEmpGrupoCod = "GRVI" Then
                Dim Form As New FormVisorCrystal("ASIENTO-VITAL.RPT", CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), String), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), String), False, False)
                Form.MdiParent = Me.MdiParent
                Form.StartPosition = FormStartPosition.CenterScreen
                Form.Show()
                Me.Cursor = Cursors.Default
            Else
                Dim Form As New FormVisorCrystal("ASIENTO.RPT", CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), String), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), String), False, False)
                Form.MdiParent = Me.MdiParent
                Form.StartPosition = FormStartPosition.CenterScreen
                Form.Show()
                Me.Cursor = Cursors.Default

            End If

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub ButtonRutaFicheros_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRutaFicheros.Click
        Try
            Me.FolderBrowserDialog1.ShowDialog()
            If IsNothing(Me.FolderBrowserDialog1.SelectedPath) = False Then
                If Me.FolderBrowserDialog1.SelectedPath.Length > 0 Then
                    If Mid(Me.FolderBrowserDialog1.SelectedPath, Me.FolderBrowserDialog1.SelectedPath.Length, 1) <> "\" Then
                        Me.TextBoxRutaFicheros.Text = Me.FolderBrowserDialog1.SelectedPath & "\"
                    End If
                    SQL = "UPDATE TH_PARA SET PARA_FILE_SPYRO_PATH = '" & Me.TextBoxRutaFicheros.Text & "'"
                    SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                    SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                    SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
                    Me.Cursor = Cursors.WaitCursor
                    Me.DbLee.EjecutaSqlCommit(SQL)
                    Me.Cursor = Cursors.Default

                End If
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonManoCorriente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonManoCorriente.Click
        Try

            Me.Cursor = Cursors.WaitCursor


            REPORT_SELECTION_FORMULA = ""
            Dim Form As New FormVisorCrystal("ingresos y saldos.rpt", CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), String), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), String), True, True)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default


        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

        End Try
    End Sub
    Private Sub AuditaCobros()
        Try
            Dim CobrosContabilidad As Decimal
            Dim CobrosNewHotel As Decimal
            Dim AnticiposRecibidosContabilidad As Decimal
            Dim AnticiposFacturadosContabilidad As Decimal
            Dim NotasCreditoNewgolf As Decimal
            Dim NotasCreditoNewgolfAnuladas As Decimal
            Dim FacturasAnuladasNewgolf As Decimal
            Dim PagosCuentaBonosAsociacion As Decimal
            Dim DescuentosFinancieros As Decimal


            Dim TotalContaBilidad As Double
            Dim TotalFinal As Double
            Dim Texto As String


            If Me.mEmpGrupoCod <> "SATO" Then
                SQL = "SELECT NVL(SUM(ASNT_DEBE + ASNT_HABER),0) FROM TH_ASNT "
                SQL += " WHERE ASNT_F_VALOR = '" & Me.DateTimePicker1.Value & "'"
                SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
                SQL += " AND TH_ASNT.ASNT_AUXILIAR_STRING = 'COBRO'"
                CobrosContabilidad = Me.DbLee.EjecutaSqlScalar(SQL)
            Else
                ' SATOCAN
                SQL = "SELECT NVL(SUM(ASNT_DEBE + ASNT_HABER),0) FROM TH_ASNT "
                SQL += " WHERE ASNT_F_VALOR = '" & Me.DateTimePicker1.Value & "'"
                SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
                SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHJournalCustPaymentQueryService'"
                ' COBROS QUE NO SON ANTICIPOS 
                '     SQL += " AND ASNT_CFCTA_COD <>  '" & Me.mAxAnticipo & "'"
                SQL += " AND TH_ASNT.ASNT_AUXILIAR_STRING IS NULL"
                ' REGISTROS NO EXCLUIDOS
                SQL += " AND TH_ASNT.ASNT_AX_STATUS NOT IN(7,8,9)"
                CobrosContabilidad = Me.DbLee.EjecutaSqlScalar(SQL)


            End If


            SQL = "SELECT nvl(SUM(ASNT_DEBE + ASNT_HABER),0) FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Me.DateTimePicker1.Value & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
            SQL += " AND (TH_ASNT.ASNT_AUXILIAR_STRING = 'ANTICIPO FACTURADO'"
            SQL += " OR TH_ASNT.ASNT_AUXILIAR_STRING = 'SALDO ANTICIPO FACTURADO')"


            AnticiposFacturadosContabilidad = Me.DbLee.EjecutaSqlScalar(SQL)


            If Me.mEmpGrupoCod <> "SATO" Then
                SQL = "SELECT nvl(SUM(ASNT_DEBE + ASNT_HABER),0) FROM TH_ASNT "
                SQL += " WHERE ASNT_F_VALOR = '" & Me.DateTimePicker1.Value & "'"
                SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
                SQL += " AND TH_ASNT.ASNT_AUXILIAR_STRING = 'ANTICIPO RECIBIDO'"
                ' SOLO POSITIVOS 
                ' SQL += " AND (ASNT_DEBE > 0 OR ASNT_HABER > 0 )"

                AnticiposRecibidosContabilidad = Me.DbLee.EjecutaSqlScalar(SQL)
            Else
                SQL = "SELECT nvl(SUM(ASNT_DEBE + ASNT_HABER),0) FROM TH_ASNT "
                SQL += " WHERE ASNT_F_VALOR = '" & Me.DateTimePicker1.Value & "'"
                SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
                SQL += " AND ASNT_WEBSERVICE_NAME = 'SAT_NHPrePaymentService'"
                AnticiposRecibidosContabilidad = Me.DbLee.EjecutaSqlScalar(SQL)
            End If




            SQL = "SELECT nvl(SUM(ASNT_DEBE + ASNT_HABER),0) FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Me.DateTimePicker1.Value & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
            SQL += " AND TH_ASNT.ASNT_AUXILIAR_STRING = 'NOTA DE CREDITO'"
            NotasCreditoNewgolf = Me.DbLee.EjecutaSqlScalar(SQL)


            SQL = "SELECT nvl(SUM(ASNT_DEBE + ASNT_HABER),0) FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Me.DateTimePicker1.Value & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
            SQL += " AND TH_ASNT.ASNT_AUXILIAR_STRING = 'NOTA DE CREDITO ANULADA'"
            NotasCreditoNewgolfAnuladas = Me.DbLee.EjecutaSqlScalar(SQL)




            SQL = "SELECT nvl(SUM(ASNT_DEBE + ASNT_HABER),0) FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Me.DateTimePicker1.Value & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
            SQL += " AND TH_ASNT.ASNT_AUXILIAR_STRING = 'COBRO ANULADO'"
            FacturasAnuladasNewgolf = Me.DbLee.EjecutaSqlScalar(SQL)



            SQL = "SELECT nvl(SUM(ASNT_DEBE + ASNT_HABER),0) FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Me.DateTimePicker1.Value & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
            SQL += " AND TH_ASNT.ASNT_AUXILIAR_STRING = 'COBRO BONO ASOCIACION'"
            PagosCuentaBonosAsociacion = Me.DbLee.EjecutaSqlScalar(SQL)


            If Me.mEmpGrupoCod = "GRHL" Then
                SQL = "SELECT NVL(SUM(MOVI_VDEB),0) FROM VN_RECAU "
                SQL += " WHERE MOVI_DATR = '" & Me.DateTimePicker1.Value & "'"
                ' NO TRANSFERIDOS A CONTABILIDAD
                SQL += " AND TIRE_CODI <> '2'"
                CobrosNewHotel = Me.DbLeeNewHotel.EjecutaSqlScalar(SQL)
            ElseIf Me.mEmpGrupoCod = "SATO" Then
                SQL = "SELECT NVL(SUM(MOVI_VDEB),0) FROM VN_RECAU "
                SQL += " WHERE MOVI_DATR = '" & Me.DateTimePicker1.Value & "'"
                ' NO TRANSFERIDOS A CONTABILIDAD
                SQL += " AND TIRE_CODI <> '2'"
                CobrosNewHotel = Me.DbLeeNewHotel.EjecutaSqlScalar(SQL)

            Else
                SQL = "SELECT NVL(SUM(MOVI_VDEB),0) FROM VN_RECAU "
                SQL += " WHERE MOVI_DATR = '" & Me.DateTimePicker1.Value & "'"
                ' NO TRANSFERIDOS A CONTABILIDAD
                SQL += " AND TIRE_CODI <> '2'"
                CobrosNewHotel = Me.DbLeeNewHotel.EjecutaSqlScalar(SQL)

            End If


            SQL = "SELECT nvl(SUM(ASNT_DEBE + ASNT_HABER),0) FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Me.DateTimePicker1.Value & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum
            SQL += " AND TH_ASNT.ASNT_AUXILIAR_STRING = 'DESCUENTO FINANCIERO'"
            DescuentosFinancieros = Me.DbLee.EjecutaSqlScalar(SQL)




            If Me.mEmpGrupoCod = "GRHL" Then
                TotalContaBilidad = Decimal.Round(CobrosContabilidad, 2) + Decimal.Round(AnticiposRecibidosContabilidad, 2)
            ElseIf Me.mEmpGrupoCod = "SATO" Then
                TotalContaBilidad = Decimal.Round(CobrosContabilidad, 2) + Decimal.Round(AnticiposRecibidosContabilidad, 2)
            ElseIf Me.mEmpGrupoCod = "TAHO" Then
                TotalContaBilidad = Decimal.Round(CobrosContabilidad, 2) + Decimal.Round(DescuentosFinancieros, 2) + Decimal.Round(AnticiposRecibidosContabilidad, 2) - Decimal.Round(AnticiposFacturadosContabilidad, 2)
            Else
                '   TotalContaBilidad = Decimal.Round(CobrosContabilidad, 2) + Decimal.Round(AnticiposRecibidosContabilidad, 2) - Decimal.Round(AnticiposFacturadosContabilidad, 2) - Decimal.Round(NotasCreditoNewgolf, 2) + Decimal.Round(NotasCreditoNewgolfAnuladas, 2) - Decimal.Round(FacturasAnuladasNewgolf, 2) + Decimal.Round(PagosCuentaBonosAsociacion, 2)
                TotalContaBilidad = Decimal.Round(CobrosContabilidad, 2) + Decimal.Round(AnticiposRecibidosContabilidad, 2)

            End If

            TotalFinal = Decimal.Round(CobrosNewHotel, 2) - TotalContaBilidad

            Texto = "Cobros NewHotel = " & CobrosNewHotel & " "
            Texto += "Cobros Contabilidad  = " & CobrosContabilidad & " Anticipos Facturados = " & AnticiposFacturadosContabilidad & " Anticipos Recibidos = " & AnticiposRecibidosContabilidad & " Dto Comerciales = " & DescuentosFinancieros
            Texto += " Auditoría de Cobro = " & TotalFinal

            Me.TextBoxDebug.Text = Me.TextBoxDebug.Text & "          " & Texto
            If TotalFinal = 0 Then
                Me.TextBoxDebug.BackColor = Color.Green
            Else
                Me.TextBoxDebug.BackColor = Color.Maroon
            End If
            Me.TextBoxDebug.Update()


            If Me.CheckBoxAuditaCobrosMostrar.Checked Then
                Dim Form As New FormAuditaCobros(CobrosNewHotel, CobrosContabilidad, AnticiposFacturadosContabilidad, NotasCreditoNewgolf, FacturasAnuladasNewgolf, PagosCuentaBonosAsociacion, CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), String), MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Me.DateTimePicker1.Value, NotasCreditoNewgolfAnuladas, AnticiposRecibidosContabilidad, DescuentosFinancieros)
                Form.ShowDialog()
                Form.StartPosition = FormStartPosition.CenterScreen
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub



    Private Sub LimpiaGrid()
        Try
            SQL = " SELECT 'Pulse Aceptar para Calcular....' AS OPERACION FROM  DUAL"
            Me.DataGrid2.DataSource = DbLee.TraerDataset(SQL, "LIMPIO")
            Me.DataGrid2.DataMember = "LIMPIO"

            Dim ts1 As New DataGridTableStyle

            ts1.MappingName = "LIMPIO"

            Dim TextCol1 As New DataGridTextBoxColumn
            TextCol1.MappingName = "OPERACION"
            TextCol1.HeaderText = "OPERACION"
            TextCol1.Width = 300

            ts1.GridColumnStyles.Add(TextCol1)

            DataGrid2.TableStyles.Clear()
            DataGrid2.TableStyles.Add(ts1)

            Me.DataGrid2.Update()



            Me.ButtonConvertir.Enabled = False
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker1.ValueChanged
        Try

            If Me.EstaLoad = True Then
                Me.LimpiaGrid()
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonVerErrores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonVerErrores.Click

        Try

            '  SQL = "SELECT ERRO_F_ATOCAB,ERRO_CBATOCAB_REFER,ERRO_LINEA,ERRO_DESCRIPCION FROM TH_ERRO WHERE ERRO_F_ATOCAB = '" & Me.DateTimePicker1.Value & "'"
            '  SQL += " ORDER BY ERRO_CBATOCAB_REFER,ERRO_LINEA ASC"


            SQL = "SELECT  * FROM TH_INCI WHERE INCI_DATR = '" & Me.DateTimePicker1.Value & "'"


            Me.DataGridViewErrores.DataSource = DbLee.TraerDataset(SQL, "ERRORES2")
            Me.DataGridViewErrores.DataMember = "ERRORES2"


            If Me.DbLee.mDbDataset.Tables("ERRORES2").Rows.Count > 0 Then
                Exportar_pdf("C:\TEMP\b.PDF", Me.DataGridViewErrores, True, False, , , , "Errores", "", False)
            End If


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub FolderBrowserDialog1_HelpRequest(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FolderBrowserDialog1.HelpRequest

    End Sub

    Private Sub DataGridHoteles_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs) Handles DataGridHoteles.Navigate

    End Sub

    Private Sub RefrescoStart()
        Try
            KeepAliveTimer = New System.Threading.Timer(KeepAliveDelegate, Nothing, 100, 100)
            KeepAliveTimer.Change(100, 100)

            If IsNothing(Win) = True Then
                Win = New Form
                Win.Name = "Refresco"
                Win.Visible = True
                Win.Location = New System.Drawing.Point(8, 328)
                Win.StartPosition = FormStartPosition.CenterScreen
                Win.Show()

                '     Me.Controls.Add(R)
            End If
            Win.Text = "Procesando " & Now.TimeOfDay.ToString
            Win.Update()
            'Application.DoEvents()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub



    'To start the threading timer.

    Private Sub RefrescoMientras(ByVal state As Object)
        Try

            '   R.Text = "Pooling Databases ...   " & Now.TimeOfDay.ToString
            '  R.Update()


            '    Me.TextBoxDebug.Text = "Pooling Databases ...   " & Now.TimeOfDay.ToString
            '   Me.TextBoxDebug.Update()
            '  Me.Update()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub RefrescoPara(ByVal state As Object)
        Try
            'To stop the thread or to disable the thread 
            KeepAliveTimer.Change(0, System.Threading.Timeout.Infinite)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub RefrescoRearranca(ByVal state As Object)
        Try
            'to restrat the thread again using the below code
            KeepAliveTimer = New System.Threading.Timer(KeepAliveDelegate, Nothing, 0, 100)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonIncidencias.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            If MULTIFECHA = "0" Then
                REPORT_SELECTION_FORMULA = "{TH_INCI.INCI_DATR}=DATETIME(" & Format(Me.DateTimePicker1.Value, REPORT_DATE_FORMAT) & ")"
            Else
                REPORT_SELECTION_FORMULA = "{TH_INCI.INCI_DATR} >= DATETIME(" & Format(Me.DateTimePicker1.Value, REPORT_DATE_FORMAT) & ")"
                REPORT_SELECTION_FORMULA += " AND {TH_INCI.INCI_DATR} <= DATETIME(" & Format(Me.DateTimePicker2.Value, REPORT_DATE_FORMAT) & ")"
            End If

            REPORT_SELECTION_FORMULA += " AND {TH_INCI.INCI_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TH_INCI.INCI_EMP_COD}= '" & Me.mEmpCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TH_INCI.INCI_EMP_NUM}= " & Me.mEmpNum

            Dim Form As New FormVisorCrystal("TH_INCI.RPT", CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), String), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), String), False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub
    Private Sub ButtonResetPerfil_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonResetPerfil.Click
        Try
            PERFILCONTABLE = ""
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonActBookId_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonActBookId.Click
        Try
            If InputBox("Palabra de Paso", "Actualizar Book Id`s") <> "PEPE" Then
                Exit Sub
            End If
            DLL = MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DLL", "FRONT")
            If DLL = 6 Then
                Me.Cursor = Cursors.WaitCursor
                Me.mProcId = Guid.NewGuid().ToString()
                Me.DllSatocan = New Axapta.Axapta(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0),
                             Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1),
                             MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                             Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3), Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "F" &
                             Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxDebug.Checked,
                             Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 4),
                             Me.ProgressBar1, Me.CheckBoxTpvNoFacturado.Checked, Me.CheckBoxFiltroDepositosNewHotel.Checked, Me.CheckBoxIncluyeBonos.Checked, Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 6), Me.CheckBoxDebug2.Checked, Me.CheckBoxDevoluciones.Checked, True, TIPOREDONDEO, Me.mProcId)

                Me.DllSatocan.CargaBookidActualizar()


            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub CheckBoxComisiones_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxComisiones.CheckedChanged

    End Sub


    Private Sub ButtonImprimeErrores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimeErrores.Click
        Try
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
            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_AX_STATUS}= 0 "
            REPORT_SELECTION_FORMULA += " AND ISNULL({TH_ASNT.ASNT_ERR_MESSAGE}) = FALSE AND {TH_ASNT.ASNT_ERR_MESSAGE} <> 'OK'"


            Dim Form As New FormVisorCrystal("ASIENTO AXAPTA ERRORES.RPT", "Errores " & Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2) & " " & Format(FEC1, "dd/MM/yyyy") & "   " & Format(FEC2, "dd/MM/yyyy"), REPORT_SELECTION_FORMULA, STRCONEXIONCENTRAL, "", False, False)


            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub ButtonImprimeExclusiones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimeExclusiones.Click
        Try

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

            REPORT_SELECTION_FORMULA += "  AND {TH_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"
            REPORT_SELECTION_FORMULA += " AND  {@MyAxapta}= 'AX'"
            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_AX_STATUS}= 7 OR {TH_ASNT.ASNT_AX_STATUS}= 8 OR {TH_ASNT.ASNT_AX_STATUS}= 9"


            Dim Form As New FormVisorCrystal("ASIENTO AXAPTA3.RPT", "Omitidos / Corregidos / Exluidos " & Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), REPORT_SELECTION_FORMULA, STRCONEXIONCENTRAL, "", False, False)

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
            REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_AX_STATUS}= 0 "
            If RESULTBOOLEAN = True Then
                ' se excluye envios pendientes de tipo inventario
                REPORT_SELECTION_FORMULA += " AND {TH_ASNT.ASNT_WEBSERVICE_NAME} <> 'SAT_JournalLossProfitQueryService' "
            End If


            'If RESULTBOOLEAN = True Then
            'Titulo = "Registros No Enviados y No se muestran Registros del Tipo Pérdidas y Ganancias !!   "
            'Else
            'Titulo = "Registros No Enviados  "

            'End If

            Titulo = "Registros No Enviados  " & Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2)

            Dim Form As New FormVisorCrystal("ASIENTO AXAPTA3.RPT", Titulo & Format(FEC1, "dd/MM/yyyy") & "   " & Format(FEC2, "dd/MM/yyyy"), REPORT_SELECTION_FORMULA, STRCONEXIONCENTRAL, "", False, False)

            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try


    End Sub
    Private Sub BuscaFechaFrontdeCierre()
        Try

            Me.Cursor = Cursors.WaitCursor
            If IsNothing(Me.DbLeeNewHotel) = True Then
                Me.DbLeeNewHotel = New C_DATOS.C_DatosOledb(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3))
            Else
                Me.DbLeeNewHotel.AbrirConexion()
            End If


            Me.DbLeeNewHotel.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


            SQL = "SELECT  DATR_DATR FROM TNHT_DATR  "

            Me.mFechaFront = Me.DbLeeNewHotel.EjecutaSqlScalar(SQL)
            Me.mFechaFront = DateAdd(DateInterval.Day, -1, Me.mFechaFront)
            Me.DbLeeNewHotel.CerrarConexion()
            Me.Cursor = Cursors.Default
            Me.DbLeeNewHotel = Nothing
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Function ControlFechaProcesar() As Boolean
        Try

            Me.BuscaFechaFrontdeCierre()
            If Me.DateTimePicker1.Value > Me.mFechaFront Then
                MsgBox("Fecha escogida es superior a última fecha de Cierre " & Me.mFechaFront, MsgBoxStyle.Information, "Atención")
                Return False
            Else
                Return True
            End If

        Catch ex As Exception

        End Try
    End Function

    Private Sub ButtonFechaFront_Click(sender As Object, e As EventArgs) Handles ButtonFechaFront.Click
        Try
            Me.Cursor = Cursors.AppStarting
            Me.BuscaFechaFrontdeCierre()

            If IsNothing(Me.mFechaFront) = False Then
                MsgBox(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2) & " = " & Format(Me.mFechaFront, "dd/MM/yyyy"),, "Fecha Front Ofice")
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub CheckBoxMinimizar_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxMinimizar.CheckedChanged

    End Sub
End Class
