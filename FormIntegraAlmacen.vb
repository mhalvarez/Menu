
Public Class FormIntegraAlmacen
    Inherits System.Windows.Forms.Form
    Dim MyIni As New cIniArray
    Dim DbLee As New C_DATOS.C_DatosOledb
    Dim SQL As String
    Dim HayRegistros As Boolean = False
    Private mEmpGrupoCod As String
    Friend WithEvents ButtonConvertir As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Private mEmpCod As String
    Private mEmpNum As Integer
    Friend WithEvents TextBoxRutaFicheros As System.Windows.Forms.TextBox
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar

    Private mConectaNewpaga As Boolean
    Private mStrConexionNewPaga As String
    Friend WithEvents ButtonIncidencias As System.Windows.Forms.Button
    Friend WithEvents CheckBoxMinimiza As System.Windows.Forms.CheckBox
    Private DLL As Integer
    Friend WithEvents ButtonUpdateFilePAth As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents ButtonFechas As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents ButtonResetPerfil As System.Windows.Forms.Button

    Dim Texto As String = ""
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButtonGastosPorFAmilia As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonGastosPorAlbaran As System.Windows.Forms.RadioButton
    Private mParaSoloFacturas As Integer

    Private mTipoGasto As String
    Private mInicio As Date
    Friend WithEvents TextBoxDuracion As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxCondensar As System.Windows.Forms.CheckBox
    Private mDuracion As Double
    Friend WithEvents TabPageSatocanIncidencias As System.Windows.Forms.TabPage
    Friend WithEvents ButtonImprimirErrores As System.Windows.Forms.Button
    Friend WithEvents ButtonImprimirEstadodeEnvios As System.Windows.Forms.Button
    Friend WithEvents ButtonImprimirEnviosPendintes As System.Windows.Forms.Button
    Friend WithEvents TabPagesSatocanUtils As System.Windows.Forms.TabPage
    Friend WithEvents ButtonSatocanObjects As System.Windows.Forms.Button
    Friend WithEvents TextBoxOraclePwd As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxOracleUser As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBoxOracleSid As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents CheckBoxTextoApuntes As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxLostContact As System.Windows.Forms.CheckBox

    Private CampoFecha As String

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
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents CheckBoxDebug As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents CheckBoxAnalitica As System.Windows.Forms.CheckBox
    Friend WithEvents DataGrid1 As System.Windows.Forms.DataGrid
    Friend WithEvents TabControlDebug As System.Windows.Forms.TabControl
    Friend WithEvents DataGrid2 As System.Windows.Forms.DataGrid
    Friend WithEvents TextBoxDebug As System.Windows.Forms.TextBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ListBoxDebug As System.Windows.Forms.ListBox
    Friend WithEvents ButtonImprimir As System.Windows.Forms.Button
    Friend WithEvents TabPageAlbaranes As System.Windows.Forms.TabPage
    Friend WithEvents TabPageTraspasos As System.Windows.Forms.TabPage
    Friend WithEvents TabPageFacturasDirectas As System.Windows.Forms.TabPage
    Friend WithEvents TabPageFacturasAlbaran As System.Windows.Forms.TabPage
    Friend WithEvents TabPageDevoluciones As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButtonFormalizaGenerico As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonFormalizaProveedor As System.Windows.Forms.RadioButton
    Friend WithEvents ButtonInventarios As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormIntegraAlmacen))
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.CheckBoxDebug = New System.Windows.Forms.CheckBox()
        Me.CheckBoxAnalitica = New System.Windows.Forms.CheckBox()
        Me.DataGrid1 = New System.Windows.Forms.DataGrid()
        Me.TabControlDebug = New System.Windows.Forms.TabControl()
        Me.TabPageAlbaranes = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.CheckBoxLostContact = New System.Windows.Forms.CheckBox()
        Me.CheckBoxTextoApuntes = New System.Windows.Forms.CheckBox()
        Me.CheckBoxCondensar = New System.Windows.Forms.CheckBox()
        Me.TextBoxDuracion = New System.Windows.Forms.TextBox()
        Me.RadioButtonGastosPorFAmilia = New System.Windows.Forms.RadioButton()
        Me.RadioButtonGastosPorAlbaran = New System.Windows.Forms.RadioButton()
        Me.TabPageTraspasos = New System.Windows.Forms.TabPage()
        Me.TabPageFacturasDirectas = New System.Windows.Forms.TabPage()
        Me.TabPageFacturasAlbaran = New System.Windows.Forms.TabPage()
        Me.TabPageDevoluciones = New System.Windows.Forms.TabPage()
        Me.TabPageSatocanIncidencias = New System.Windows.Forms.TabPage()
        Me.ButtonImprimirEnviosPendintes = New System.Windows.Forms.Button()
        Me.ButtonImprimirEstadodeEnvios = New System.Windows.Forms.Button()
        Me.ButtonImprimirErrores = New System.Windows.Forms.Button()
        Me.TabPagesSatocanUtils = New System.Windows.Forms.TabPage()
        Me.TextBoxOracleSid = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBoxOraclePwd = New System.Windows.Forms.TextBox()
        Me.TextBoxOracleUser = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ButtonSatocanObjects = New System.Windows.Forms.Button()
        Me.DataGrid2 = New System.Windows.Forms.DataGrid()
        Me.TextBoxDebug = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ListBoxDebug = New System.Windows.Forms.ListBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.RadioButtonFormalizaProveedor = New System.Windows.Forms.RadioButton()
        Me.RadioButtonFormalizaGenerico = New System.Windows.Forms.RadioButton()
        Me.ButtonInventarios = New System.Windows.Forms.Button()
        Me.ButtonConvertir = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBoxRutaFicheros = New System.Windows.Forms.TextBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.CheckBoxMinimiza = New System.Windows.Forms.CheckBox()
        Me.ButtonUpdateFilePAth = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.ButtonResetPerfil = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ButtonFechas = New System.Windows.Forms.Button()
        Me.ButtonIncidencias = New System.Windows.Forms.Button()
        Me.ButtonImprimir = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.ButtonAceptar = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlDebug.SuspendLayout()
        Me.TabPageAlbaranes.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TabPageSatocanIncidencias.SuspendLayout()
        Me.TabPagesSatocanUtils.SuspendLayout()
        CType(Me.DataGrid2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.CustomFormat = "dd/MM/yyyy"
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker1.Location = New System.Drawing.Point(8, 8)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(152, 20)
        Me.DateTimePicker1.TabIndex = 1
        Me.DateTimePicker1.Value = New Date(2006, 4, 10, 0, 0, 0, 0)
        '
        'CheckBoxDebug
        '
        Me.CheckBoxDebug.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxDebug.Location = New System.Drawing.Point(616, 1)
        Me.CheckBoxDebug.Name = "CheckBoxDebug"
        Me.CheckBoxDebug.Size = New System.Drawing.Size(64, 24)
        Me.CheckBoxDebug.TabIndex = 5
        Me.CheckBoxDebug.Text = "Debug"
        '
        'CheckBoxAnalitica
        '
        Me.CheckBoxAnalitica.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxAnalitica.Checked = True
        Me.CheckBoxAnalitica.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxAnalitica.Location = New System.Drawing.Point(687, 2)
        Me.CheckBoxAnalitica.Name = "CheckBoxAnalitica"
        Me.CheckBoxAnalitica.Size = New System.Drawing.Size(72, 24)
        Me.CheckBoxAnalitica.TabIndex = 13
        Me.CheckBoxAnalitica.Text = "Analítica"
        '
        'DataGrid1
        '
        Me.DataGrid1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGrid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DataGrid1.DataMember = ""
        Me.DataGrid1.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGrid1.Location = New System.Drawing.Point(8, 32)
        Me.DataGrid1.Name = "DataGrid1"
        Me.DataGrid1.ReadOnly = True
        Me.DataGrid1.Size = New System.Drawing.Size(583, 88)
        Me.DataGrid1.TabIndex = 14
        '
        'TabControlDebug
        '
        Me.TabControlDebug.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControlDebug.Controls.Add(Me.TabPageAlbaranes)
        Me.TabControlDebug.Controls.Add(Me.TabPageTraspasos)
        Me.TabControlDebug.Controls.Add(Me.TabPageFacturasDirectas)
        Me.TabControlDebug.Controls.Add(Me.TabPageFacturasAlbaran)
        Me.TabControlDebug.Controls.Add(Me.TabPageDevoluciones)
        Me.TabControlDebug.Controls.Add(Me.TabPageSatocanIncidencias)
        Me.TabControlDebug.Controls.Add(Me.TabPagesSatocanUtils)
        Me.TabControlDebug.Location = New System.Drawing.Point(0, 128)
        Me.TabControlDebug.Name = "TabControlDebug"
        Me.TabControlDebug.SelectedIndex = 0
        Me.TabControlDebug.Size = New System.Drawing.Size(856, 88)
        Me.TabControlDebug.TabIndex = 15
        '
        'TabPageAlbaranes
        '
        Me.TabPageAlbaranes.Controls.Add(Me.GroupBox2)
        Me.TabPageAlbaranes.Location = New System.Drawing.Point(4, 22)
        Me.TabPageAlbaranes.Name = "TabPageAlbaranes"
        Me.TabPageAlbaranes.Size = New System.Drawing.Size(848, 62)
        Me.TabPageAlbaranes.TabIndex = 0
        Me.TabPageAlbaranes.Text = "Albaranes"
        Me.TabPageAlbaranes.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.CheckBoxLostContact)
        Me.GroupBox2.Controls.Add(Me.CheckBoxTextoApuntes)
        Me.GroupBox2.Controls.Add(Me.CheckBoxCondensar)
        Me.GroupBox2.Controls.Add(Me.TextBoxDuracion)
        Me.GroupBox2.Controls.Add(Me.RadioButtonGastosPorFAmilia)
        Me.GroupBox2.Controls.Add(Me.RadioButtonGastosPorAlbaran)
        Me.GroupBox2.Location = New System.Drawing.Point(7, 6)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(797, 39)
        Me.GroupBox2.TabIndex = 54
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Formato Gastos"
        '
        'CheckBoxLostContact
        '
        Me.CheckBoxLostContact.AutoSize = True
        Me.CheckBoxLostContact.Location = New System.Drawing.Point(484, 14)
        Me.CheckBoxLostContact.Name = "CheckBoxLostContact"
        Me.CheckBoxLostContact.Size = New System.Drawing.Size(86, 17)
        Me.CheckBoxLostContact.TabIndex = 5
        Me.CheckBoxLostContact.Text = "Lost Contact"
        Me.CheckBoxLostContact.UseVisualStyleBackColor = True
        '
        'CheckBoxTextoApuntes
        '
        Me.CheckBoxTextoApuntes.AutoSize = True
        Me.CheckBoxTextoApuntes.Location = New System.Drawing.Point(310, 14)
        Me.CheckBoxTextoApuntes.Name = "CheckBoxTextoApuntes"
        Me.CheckBoxTextoApuntes.Size = New System.Drawing.Size(171, 17)
        Me.CheckBoxTextoApuntes.TabIndex = 4
        Me.CheckBoxTextoApuntes.Text = "Usar Texto Común en Apuntes"
        Me.CheckBoxTextoApuntes.UseVisualStyleBackColor = True
        '
        'CheckBoxCondensar
        '
        Me.CheckBoxCondensar.AutoSize = True
        Me.CheckBoxCondensar.Location = New System.Drawing.Point(6, 16)
        Me.CheckBoxCondensar.Name = "CheckBoxCondensar"
        Me.CheckBoxCondensar.Size = New System.Drawing.Size(115, 17)
        Me.CheckBoxCondensar.TabIndex = 3
        Me.CheckBoxCondensar.Text = "Condensar Asiento"
        Me.CheckBoxCondensar.UseVisualStyleBackColor = True
        '
        'TextBoxDuracion
        '
        Me.TextBoxDuracion.BackColor = System.Drawing.SystemColors.Info
        Me.TextBoxDuracion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDuracion.Location = New System.Drawing.Point(605, 13)
        Me.TextBoxDuracion.Name = "TextBoxDuracion"
        Me.TextBoxDuracion.Size = New System.Drawing.Size(186, 20)
        Me.TextBoxDuracion.TabIndex = 2
        '
        'RadioButtonGastosPorFAmilia
        '
        Me.RadioButtonGastosPorFAmilia.AutoSize = True
        Me.RadioButtonGastosPorFAmilia.Location = New System.Drawing.Point(228, 15)
        Me.RadioButtonGastosPorFAmilia.Name = "RadioButtonGastosPorFAmilia"
        Me.RadioButtonGastosPorFAmilia.Size = New System.Drawing.Size(76, 17)
        Me.RadioButtonGastosPorFAmilia.TabIndex = 1
        Me.RadioButtonGastosPorFAmilia.TabStop = True
        Me.RadioButtonGastosPorFAmilia.Text = "Por Familia"
        Me.RadioButtonGastosPorFAmilia.UseVisualStyleBackColor = True
        '
        'RadioButtonGastosPorAlbaran
        '
        Me.RadioButtonGastosPorAlbaran.AutoSize = True
        Me.RadioButtonGastosPorAlbaran.Location = New System.Drawing.Point(142, 16)
        Me.RadioButtonGastosPorAlbaran.Name = "RadioButtonGastosPorAlbaran"
        Me.RadioButtonGastosPorAlbaran.Size = New System.Drawing.Size(80, 17)
        Me.RadioButtonGastosPorAlbaran.TabIndex = 0
        Me.RadioButtonGastosPorAlbaran.TabStop = True
        Me.RadioButtonGastosPorAlbaran.Text = "Por Albarán"
        Me.RadioButtonGastosPorAlbaran.UseVisualStyleBackColor = True
        '
        'TabPageTraspasos
        '
        Me.TabPageTraspasos.Location = New System.Drawing.Point(4, 22)
        Me.TabPageTraspasos.Name = "TabPageTraspasos"
        Me.TabPageTraspasos.Size = New System.Drawing.Size(848, 62)
        Me.TabPageTraspasos.TabIndex = 1
        Me.TabPageTraspasos.Text = "Traspasos"
        Me.TabPageTraspasos.UseVisualStyleBackColor = True
        '
        'TabPageFacturasDirectas
        '
        Me.TabPageFacturasDirectas.Location = New System.Drawing.Point(4, 22)
        Me.TabPageFacturasDirectas.Name = "TabPageFacturasDirectas"
        Me.TabPageFacturasDirectas.Size = New System.Drawing.Size(848, 62)
        Me.TabPageFacturasDirectas.TabIndex = 2
        Me.TabPageFacturasDirectas.Text = "Facturas Directas"
        Me.TabPageFacturasDirectas.UseVisualStyleBackColor = True
        '
        'TabPageFacturasAlbaran
        '
        Me.TabPageFacturasAlbaran.Location = New System.Drawing.Point(4, 22)
        Me.TabPageFacturasAlbaran.Name = "TabPageFacturasAlbaran"
        Me.TabPageFacturasAlbaran.Size = New System.Drawing.Size(848, 62)
        Me.TabPageFacturasAlbaran.TabIndex = 3
        Me.TabPageFacturasAlbaran.Text = "Facturas(Albaranes)"
        Me.TabPageFacturasAlbaran.UseVisualStyleBackColor = True
        '
        'TabPageDevoluciones
        '
        Me.TabPageDevoluciones.Location = New System.Drawing.Point(4, 22)
        Me.TabPageDevoluciones.Name = "TabPageDevoluciones"
        Me.TabPageDevoluciones.Size = New System.Drawing.Size(848, 62)
        Me.TabPageDevoluciones.TabIndex = 4
        Me.TabPageDevoluciones.Text = "Devoluciones"
        Me.TabPageDevoluciones.UseVisualStyleBackColor = True
        '
        'TabPageSatocanIncidencias
        '
        Me.TabPageSatocanIncidencias.Controls.Add(Me.ButtonImprimirEnviosPendintes)
        Me.TabPageSatocanIncidencias.Controls.Add(Me.ButtonImprimirEstadodeEnvios)
        Me.TabPageSatocanIncidencias.Controls.Add(Me.ButtonImprimirErrores)
        Me.TabPageSatocanIncidencias.Location = New System.Drawing.Point(4, 22)
        Me.TabPageSatocanIncidencias.Name = "TabPageSatocanIncidencias"
        Me.TabPageSatocanIncidencias.Size = New System.Drawing.Size(848, 62)
        Me.TabPageSatocanIncidencias.TabIndex = 5
        Me.TabPageSatocanIncidencias.Text = "Satocan Incidencias Reports"
        Me.TabPageSatocanIncidencias.UseVisualStyleBackColor = True
        '
        'ButtonImprimirEnviosPendintes
        '
        Me.ButtonImprimirEnviosPendintes.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprimirEnviosPendintes.Location = New System.Drawing.Point(242, 12)
        Me.ButtonImprimirEnviosPendintes.Name = "ButtonImprimirEnviosPendintes"
        Me.ButtonImprimirEnviosPendintes.Size = New System.Drawing.Size(111, 28)
        Me.ButtonImprimirEnviosPendintes.TabIndex = 45
        Me.ButtonImprimirEnviosPendintes.Text = "&Envíos Pendientes"
        Me.ButtonImprimirEnviosPendintes.UseVisualStyleBackColor = True
        '
        'ButtonImprimirEstadodeEnvios
        '
        Me.ButtonImprimirEstadodeEnvios.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprimirEstadodeEnvios.Location = New System.Drawing.Point(125, 12)
        Me.ButtonImprimirEstadodeEnvios.Name = "ButtonImprimirEstadodeEnvios"
        Me.ButtonImprimirEstadodeEnvios.Size = New System.Drawing.Size(111, 28)
        Me.ButtonImprimirEstadodeEnvios.TabIndex = 44
        Me.ButtonImprimirEstadodeEnvios.Text = "&Estado de Envíos"
        Me.ButtonImprimirEstadodeEnvios.UseVisualStyleBackColor = True
        '
        'ButtonImprimirErrores
        '
        Me.ButtonImprimirErrores.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprimirErrores.Location = New System.Drawing.Point(8, 12)
        Me.ButtonImprimirErrores.Name = "ButtonImprimirErrores"
        Me.ButtonImprimirErrores.Size = New System.Drawing.Size(111, 28)
        Me.ButtonImprimirErrores.TabIndex = 43
        Me.ButtonImprimirErrores.Text = "&Imprimir Errores"
        Me.ButtonImprimirErrores.UseVisualStyleBackColor = True
        '
        'TabPagesSatocanUtils
        '
        Me.TabPagesSatocanUtils.Controls.Add(Me.TextBoxOracleSid)
        Me.TabPagesSatocanUtils.Controls.Add(Me.Label3)
        Me.TabPagesSatocanUtils.Controls.Add(Me.TextBoxOraclePwd)
        Me.TabPagesSatocanUtils.Controls.Add(Me.TextBoxOracleUser)
        Me.TabPagesSatocanUtils.Controls.Add(Me.Label2)
        Me.TabPagesSatocanUtils.Controls.Add(Me.Label1)
        Me.TabPagesSatocanUtils.Controls.Add(Me.ButtonSatocanObjects)
        Me.TabPagesSatocanUtils.Location = New System.Drawing.Point(4, 22)
        Me.TabPagesSatocanUtils.Name = "TabPagesSatocanUtils"
        Me.TabPagesSatocanUtils.Size = New System.Drawing.Size(848, 62)
        Me.TabPagesSatocanUtils.TabIndex = 6
        Me.TabPagesSatocanUtils.Text = "Satocan Utils"
        Me.TabPagesSatocanUtils.UseVisualStyleBackColor = True
        '
        'TextBoxOracleSid
        '
        Me.TextBoxOracleSid.Location = New System.Drawing.Point(132, 36)
        Me.TextBoxOracleSid.Name = "TextBoxOracleSid"
        Me.TextBoxOracleSid.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxOracleSid.TabIndex = 52
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(18, 36)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(25, 13)
        Me.Label3.TabIndex = 51
        Me.Label3.Text = "SID"
        '
        'TextBoxOraclePwd
        '
        Me.TextBoxOraclePwd.Location = New System.Drawing.Point(297, 12)
        Me.TextBoxOraclePwd.Name = "TextBoxOraclePwd"
        Me.TextBoxOraclePwd.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxOraclePwd.TabIndex = 50
        '
        'TextBoxOracleUser
        '
        Me.TextBoxOracleUser.Location = New System.Drawing.Point(132, 12)
        Me.TextBoxOracleUser.Name = "TextBoxOracleUser"
        Me.TextBoxOracleUser.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxOracleUser.TabIndex = 49
        Me.TextBoxOracleUser.Text = "SYSTEM"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(238, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 13)
        Me.Label2.TabIndex = 48
        Me.Label2.Text = "Password"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(108, 13)
        Me.Label1.TabIndex = 47
        Me.Label1.Text = "User SYSTEM oracle"
        '
        'ButtonSatocanObjects
        '
        Me.ButtonSatocanObjects.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonSatocanObjects.Location = New System.Drawing.Point(419, 4)
        Me.ButtonSatocanObjects.Name = "ButtonSatocanObjects"
        Me.ButtonSatocanObjects.Size = New System.Drawing.Size(134, 28)
        Me.ButtonSatocanObjects.TabIndex = 46
        Me.ButtonSatocanObjects.Text = "Create/Compile Objets"
        Me.ButtonSatocanObjects.UseVisualStyleBackColor = True
        '
        'DataGrid2
        '
        Me.DataGrid2.AlternatingBackColor = System.Drawing.SystemColors.ScrollBar
        Me.DataGrid2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGrid2.CaptionFont = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGrid2.DataMember = ""
        Me.DataGrid2.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGrid2.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGrid2.Location = New System.Drawing.Point(8, 222)
        Me.DataGrid2.Name = "DataGrid2"
        Me.DataGrid2.ReadOnly = True
        Me.DataGrid2.Size = New System.Drawing.Size(751, 162)
        Me.DataGrid2.TabIndex = 16
        '
        'TextBoxDebug
        '
        Me.TextBoxDebug.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxDebug.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.TextBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDebug.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxDebug.ForeColor = System.Drawing.Color.White
        Me.TextBoxDebug.Location = New System.Drawing.Point(8, 392)
        Me.TextBoxDebug.Name = "TextBoxDebug"
        Me.TextBoxDebug.Size = New System.Drawing.Size(751, 21)
        Me.TextBoxDebug.TabIndex = 17
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Maroon
        Me.Label5.Location = New System.Drawing.Point(8, 416)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(168, 23)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "No Integre si exiten Mensajes "
        '
        'ListBoxDebug
        '
        Me.ListBoxDebug.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListBoxDebug.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBoxDebug.ForeColor = System.Drawing.Color.SteelBlue
        Me.ListBoxDebug.Location = New System.Drawing.Point(8, 440)
        Me.ListBoxDebug.Name = "ListBoxDebug"
        Me.ListBoxDebug.Size = New System.Drawing.Size(751, 80)
        Me.ListBoxDebug.TabIndex = 20
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.RadioButtonFormalizaProveedor)
        Me.GroupBox1.Controls.Add(Me.RadioButtonFormalizaGenerico)
        Me.GroupBox1.Location = New System.Drawing.Point(597, 24)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(251, 48)
        Me.GroupBox1.TabIndex = 23
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Tipo de Formalización de Albaranes"
        '
        'RadioButtonFormalizaProveedor
        '
        Me.RadioButtonFormalizaProveedor.Location = New System.Drawing.Point(128, 16)
        Me.RadioButtonFormalizaProveedor.Name = "RadioButtonFormalizaProveedor"
        Me.RadioButtonFormalizaProveedor.Size = New System.Drawing.Size(117, 24)
        Me.RadioButtonFormalizaProveedor.TabIndex = 1
        Me.RadioButtonFormalizaProveedor.Text = "Cuenta Proveedor"
        '
        'RadioButtonFormalizaGenerico
        '
        Me.RadioButtonFormalizaGenerico.Location = New System.Drawing.Point(8, 16)
        Me.RadioButtonFormalizaGenerico.Name = "RadioButtonFormalizaGenerico"
        Me.RadioButtonFormalizaGenerico.Size = New System.Drawing.Size(112, 24)
        Me.RadioButtonFormalizaGenerico.TabIndex = 0
        Me.RadioButtonFormalizaGenerico.Text = "Cuenta Genérica"
        '
        'ButtonInventarios
        '
        Me.ButtonInventarios.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonInventarios.Location = New System.Drawing.Point(765, 284)
        Me.ButtonInventarios.Name = "ButtonInventarios"
        Me.ButtonInventarios.Size = New System.Drawing.Size(91, 23)
        Me.ButtonInventarios.TabIndex = 24
        Me.ButtonInventarios.Text = "Inventarios"
        '
        'ButtonConvertir
        '
        Me.ButtonConvertir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonConvertir.Enabled = False
        Me.ButtonConvertir.Location = New System.Drawing.Point(765, 313)
        Me.ButtonConvertir.Name = "ButtonConvertir"
        Me.ButtonConvertir.Size = New System.Drawing.Size(91, 23)
        Me.ButtonConvertir.TabIndex = 28
        Me.ButtonConvertir.Text = "Convertir"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(832, 338)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(24, 23)
        Me.Button1.TabIndex = 29
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBoxRutaFicheros
        '
        Me.TextBoxRutaFicheros.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxRutaFicheros.BackColor = System.Drawing.Color.Maroon
        Me.TextBoxRutaFicheros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxRutaFicheros.ForeColor = System.Drawing.Color.White
        Me.TextBoxRutaFicheros.Location = New System.Drawing.Point(597, 102)
        Me.TextBoxRutaFicheros.Name = "TextBoxRutaFicheros"
        Me.TextBoxRutaFicheros.Size = New System.Drawing.Size(211, 20)
        Me.TextBoxRutaFicheros.TabIndex = 30
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(631, 128)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(217, 16)
        Me.ProgressBar1.TabIndex = 0
        '
        'CheckBoxMinimiza
        '
        Me.CheckBoxMinimiza.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxMinimiza.Checked = True
        Me.CheckBoxMinimiza.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxMinimiza.Location = New System.Drawing.Point(776, 2)
        Me.CheckBoxMinimiza.Name = "CheckBoxMinimiza"
        Me.CheckBoxMinimiza.Size = New System.Drawing.Size(72, 24)
        Me.CheckBoxMinimiza.TabIndex = 47
        Me.CheckBoxMinimiza.Text = "Minimizar"
        '
        'ButtonUpdateFilePAth
        '
        Me.ButtonUpdateFilePAth.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonUpdateFilePAth.Location = New System.Drawing.Point(814, 98)
        Me.ButtonUpdateFilePAth.Name = "ButtonUpdateFilePAth"
        Me.ButtonUpdateFilePAth.Size = New System.Drawing.Size(34, 23)
        Me.ButtonUpdateFilePAth.TabIndex = 48
        Me.ButtonUpdateFilePAth.Text = ":::"
        Me.ButtonUpdateFilePAth.UseVisualStyleBackColor = True
        '
        'ButtonResetPerfil
        '
        Me.ButtonResetPerfil.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonResetPerfil.Image = CType(resources.GetObject("ButtonResetPerfil.Image"), System.Drawing.Image)
        Me.ButtonResetPerfil.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonResetPerfil.Location = New System.Drawing.Point(764, 361)
        Me.ButtonResetPerfil.Name = "ButtonResetPerfil"
        Me.ButtonResetPerfil.Size = New System.Drawing.Size(92, 23)
        Me.ButtonResetPerfil.TabIndex = 52
        Me.ButtonResetPerfil.Text = "Reset Perfil"
        '
        'Button3
        '
        Me.Button3.Image = CType(resources.GetObject("Button3.Image"), System.Drawing.Image)
        Me.Button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button3.Location = New System.Drawing.Point(423, 4)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(168, 27)
        Me.Button3.TabIndex = 51
        Me.Button3.Text = "Fechas Posibles NewPaga?"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button2.Location = New System.Drawing.Point(765, 252)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(91, 24)
        Me.Button2.TabIndex = 50
        Me.Button2.Text = "&Imprimir(I)"
        '
        'ButtonFechas
        '
        Me.ButtonFechas.Image = CType(resources.GetObject("ButtonFechas.Image"), System.Drawing.Image)
        Me.ButtonFechas.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonFechas.Location = New System.Drawing.Point(276, 4)
        Me.ButtonFechas.Name = "ButtonFechas"
        Me.ButtonFechas.Size = New System.Drawing.Size(141, 27)
        Me.ButtonFechas.TabIndex = 49
        Me.ButtonFechas.Text = "Fechas Posibles ?"
        Me.ButtonFechas.UseVisualStyleBackColor = True
        '
        'ButtonIncidencias
        '
        Me.ButtonIncidencias.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonIncidencias.Image = CType(resources.GetObject("ButtonIncidencias.Image"), System.Drawing.Image)
        Me.ButtonIncidencias.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonIncidencias.Location = New System.Drawing.Point(765, 440)
        Me.ButtonIncidencias.Name = "ButtonIncidencias"
        Me.ButtonIncidencias.Size = New System.Drawing.Size(91, 23)
        Me.ButtonIncidencias.TabIndex = 46
        Me.ButtonIncidencias.Text = "Inciencias"
        '
        'ButtonImprimir
        '
        Me.ButtonImprimir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonImprimir.Image = CType(resources.GetObject("ButtonImprimir.Image"), System.Drawing.Image)
        Me.ButtonImprimir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonImprimir.Location = New System.Drawing.Point(765, 222)
        Me.ButtonImprimir.Name = "ButtonImprimir"
        Me.ButtonImprimir.Size = New System.Drawing.Size(91, 24)
        Me.ButtonImprimir.TabIndex = 21
        Me.ButtonImprimir.Text = "&Imprimir"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(184, 416)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(24, 24)
        Me.PictureBox1.TabIndex = 19
        Me.PictureBox1.TabStop = False
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Image = CType(resources.GetObject("ButtonAceptar.Image"), System.Drawing.Image)
        Me.ButtonAceptar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonAceptar.Location = New System.Drawing.Point(168, 4)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(102, 27)
        Me.ButtonAceptar.TabIndex = 12
        Me.ButtonAceptar.Text = "&Aceptar"
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(168, 393)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(55, 47)
        Me.PictureBox2.TabIndex = 53
        Me.PictureBox2.TabStop = False
        Me.PictureBox2.Visible = False
        '
        'FormIntegraAlmacen
        '
        Me.AcceptButton = Me.ButtonAceptar
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(864, 604)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.ButtonResetPerfil)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.ButtonFechas)
        Me.Controls.Add(Me.ButtonUpdateFilePAth)
        Me.Controls.Add(Me.CheckBoxMinimiza)
        Me.Controls.Add(Me.ButtonIncidencias)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.TextBoxRutaFicheros)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ButtonConvertir)
        Me.Controls.Add(Me.ButtonInventarios)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ButtonImprimir)
        Me.Controls.Add(Me.ListBoxDebug)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TextBoxDebug)
        Me.Controls.Add(Me.DataGrid2)
        Me.Controls.Add(Me.TabControlDebug)
        Me.Controls.Add(Me.DataGrid1)
        Me.Controls.Add(Me.CheckBoxAnalitica)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.CheckBoxDebug)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.MinimumSize = New System.Drawing.Size(872, 631)
        Me.Name = "FormIntegraAlmacen"
        Me.Text = "Integración Contable Almacen"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlDebug.ResumeLayout(False)
        Me.TabPageAlbaranes.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TabPageSatocanIncidencias.ResumeLayout(False)
        Me.TabPagesSatocanUtils.ResumeLayout(False)
        Me.TabPagesSatocanUtils.PerformLayout()
        CType(Me.DataGrid2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "LOAD"




    Private Sub FormIntegraAlmacen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            Me.DateTimePicker1.Value = (DateAdd(DateInterval.Day, -1, CType(Format(Now, "dd/MM/yyyy"), Date)))
            Me.DbLee = New C_DATOS.C_DatosOledb(MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"))
            Me.DbLee.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


            Me.mEmpGrupoCod = MyIni.IniGet(Application.StartupPath & "\menu.ini", "PARAMETER", "PARA_EMPGRUPO_COD")


            If MINIMIZA = "1" Then
                Me.CheckBoxMinimiza.Checked = True
            Else
                Me.CheckBoxMinimiza.Checked = False
            End If
            Me.CheckBoxMinimiza.Update()


            If EMP_COD = "0" Then
                SQL = "SELECT HOTEL_EMPGRUPO_COD,HOTEL_EMP_COD,HOTEL_DESCRIPCION,HOTEL_ODBC_ALMACEN,HOTEL_SPYRO,HOTEL_EMP_NUM FROM TH_HOTEL "
                SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND HOTEL_INT_NEWS = 1 "
                SQL += " ORDER BY HOTEL_DESCRIPCION ASC"

            Else
                SQL = "SELECT HOTEL_EMPGRUPO_COD,HOTEL_EMP_COD,HOTEL_DESCRIPCION,HOTEL_ODBC_ALMACEN,HOTEL_SPYRO,HOTEL_EMP_NUM FROM TH_HOTEL "
                SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND  HOTEL_EMP_COD = '" & EMP_COD & "'"
                SQL += " AND HOTEL_INT_NEWS = 1 "
                SQL += " ORDER BY HOTEL_DESCRIPCION ASC"

            End If

            Me.DataGrid1.DataSource = Me.DbLee.TraerDataset(SQL, "HOTELES")
            Me.DataGrid1.DataMember = "HOTELES"
            If Me.DbLee.mDbDataset.Tables("HOTELES").Rows.Count > 0 Then
                Me.HayRegistros = True
                Me.ConfGridHoteles()
                Me.DataGrid1.Select(0)
                Me.mEmpGrupoCod = Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 0)
                Me.mEmpCod = Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 1)
                Me.mEmpNum = Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 5)
            Else
                Me.HayRegistros = False
            End If


            ' SOLO LOPEZ
            If Me.mEmpGrupoCod = "GRHL" Then
                '    Me.RadioButtonGastosPorAlbaran.Checked = 
                Me.CheckBoxCondensar.Checked = False
                Me.RadioButtonGastosPorAlbaran.Checked = True
                Me.RadioButtonGastosPorFAmilia.Checked = False
                Me.CheckBoxTextoApuntes.Checked = True


                Me.RadioButtonGastosPorAlbaran.Enabled = False
                Me.RadioButtonGastosPorFAmilia.Enabled = False
                Me.CheckBoxCondensar.Enabled = False
                Me.CheckBoxTextoApuntes.Enabled = False
            End If


            ' SOLO SATOCAN
            If Me.mEmpGrupoCod = "SATO" Then
                Me.ButtonInventarios.Visible = False
                Me.ButtonResetPerfil.Visible = False
                Me.ButtonIncidencias.Visible = False
                Me.Button1.Visible = False

                Me.Button3.Visible = False


                Me.RadioButtonGastosPorAlbaran.Enabled = False
                Me.RadioButtonGastosPorFAmilia.Enabled = False
                Me.CheckBoxCondensar.Enabled = False





                Dim tb As TabControl.TabPageCollection = Me.TabControlDebug.TabPages
                tb.Remove(Me.TabPageTraspasos)
                tb.Remove(Me.TabPageFacturasDirectas)
                tb.Remove(Me.TabPageFacturasAlbaran)
                tb.Remove(Me.TabPageDevoluciones)


                Me.TextBoxOracleSid.Text = StrConexionExtraeDataSource(MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"))




                Me.Update()
            End If


            Me.ButtonImprimir.Enabled = False
            Me.Button2.Enabled = False


            Me.GetUltimaFechaNewStock()

            Me.ActualizaPatametrosHotel()



        Catch EX As Exception
            MsgBox(EX.Message)
        End Try
    End Sub
#End Region

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try



            ' Texto = "Ojo Falta Marcar determinados Almacenes para que no se hagan los traspasos"
            '  MsgBox(Texto, MsgBoxStyle.Information, "Falta Desarrollo")


            DLL = CType(MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DLL", "ALMACEN"), Integer)
            If IsNumeric(DLL) = False Then
                MsgBox("Falta Numero de Dll a usar en Fichero .INI ", MsgBoxStyle.Exclamation, "Atención")
                Me.Close()
            End If

            ' control larga duracion 
            Me.Texto = "Atención este puede ser un Proceso de Larga Duración , se minimiza la Ventana durante el Cálculo"


            If MINIMIZA = "1" Then
                MsgBox(Me.Texto, MsgBoxStyle.Information, "Atención")
                Me.WindowState = FormWindowState.Minimized
                Me.Update()
                Application.DoEvents()

                Me.ParentForm.WindowState = FormWindowState.Minimized
                Me.ParentForm.Text += " Procesando ...."
                Me.ParentForm.Update()
                Application.DoEvents()
            End If

            Me.Update()




            Dim INTEGRA As Object
            If Me.HayRegistros = True Then
                '' dll generica ( tipo spyro)
                If DLL = 1 Then

                    Me.CampoFecha = " ASNT_F_ATOCAB "
                    INTEGRA = New IntegraAlmacen.IntegraAlmacen(CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 0)),
                    CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 1)),
                    MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                    CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 3)), Me.DateTimePicker1.Value, "A" & Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", False, Me.TextBoxDebug, Me.ListBoxDebug, Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 4), False, False)
                    Me.Cursor = Cursors.AppStarting
                    INTEGRA.Procesar()

                    '' Hoteles Lopez
                ElseIf DLL = 2 Then
                    Me.CampoFecha = " ASNT_F_VALOR "
                    If Me.RadioButtonGastosPorAlbaran.Checked Then
                        Me.mTipoGasto = "0"
                    Else
                        Me.mTipoGasto = "1"
                    End If

                    Me.TextBoxDebug.Text = "Preparando Objeto y Abriendo Conexiones ..."
                    Me.TextBoxDebug.Update()
                    Me.Cursor = Cursors.AppStarting
                    INTEGRA = New Integra_NewStock_HLopez.NewStockHLopez(CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 0)),
                    CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 1)),
                    MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                    CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 3)), Me.DateTimePicker1.Value, "A" _
                    & Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxCondensar.Checked, Me.TextBoxDebug,
                    Me.ListBoxDebug, Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 4), False, False,
                    Me.mEmpNum, Me, Me.ProgressBar1, Me.mConectaNewpaga, Me.mStrConexionNewPaga, Me.mTipoGasto, Me.CheckBoxCondensar.Checked)
                    Me.Cursor = Cursors.AppStarting


                    Me.mInicio = Now


                    Me.TextBoxDuracion.Text = Format(Now, "dd/MM/yyyy HH:mm:ss")
                    Me.Timer1.Enabled = False
                    Me.PictureBox2.Visible = False


                    If Me.mParaSoloFacturas = 0 Then
                        Application.DoEvents()
                        INTEGRA.Procesar()
                    Else
                        Application.DoEvents()
                        INTEGRA.PROCESARSOLOFACTURAS()
                    End If


                    Me.mDuracion = DateDiff(DateInterval.Second, Me.mInicio, Now) / (24 * 3600)
                    Me.TextBoxDuracion.Text = (Format(Date.FromOADate(Me.mDuracion), "mm 'minutos ' ss ' segundos'"))

                    If Me.ListBoxDebug.Items.Count > 0 Then
                        Me.PictureBox2.Visible = True
                        Me.Timer1.Enabled = True
                        Dim Texto As String
                        Texto = " (1)  Existen INCIDENCIAS en el Proceso de Validación de Cuentas " & vbCrLf & vbCrLf
                        Texto += " (2) NO Integre el Fichero en esta Situación " & vbCrLf & vbCrLf
                        Texto += " (3) CORRIJA las Incidencias y VUELVA a GENERAR el Fichero hasta que quede Libre de INCIDENCIAS  " & vbCrLf & vbCrLf
                        Texto += " (4) Las Incidencias puede Verlas/Imprimirlas en la Parte Inferior de la Pantalla Anterior"
                        MsgBox(Texto, MsgBoxStyle.Exclamation, "Atención Lea")
                    End If



                ElseIf DLL = 3 Then
                    Me.CampoFecha = " ASNT_F_VALOR "
                    '' axapta
                    Dim AXDLL As AxaptaAlmacen.AxaptaAlmacen

                    If Me.RadioButtonGastosPorAlbaran.Checked Then
                        Me.mTipoGasto = "0"
                    Else
                        Me.mTipoGasto = "1"
                    End If





                    Me.TextBoxDebug.Text = "Preparando Objeto y Abriendo Conexiones ..."
                    Me.TextBoxDebug.Update()
                    Me.Cursor = Cursors.AppStarting
                    AXDLL = New AxaptaAlmacen.AxaptaAlmacen(CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 0)),
                    CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 1)),
                    MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                    CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 3)), Me.DateTimePicker1.Value, "A" _
                    & Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxCondensar.Checked, Me.TextBoxDebug,
                    Me.ListBoxDebug, Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 4), False, False,
                    Me.mEmpNum, Me, Me.ProgressBar1, Me.mConectaNewpaga, Me.mStrConexionNewPaga, Me.mTipoGasto, True)
                    Me.Cursor = Cursors.AppStarting


                    Me.mInicio = Now

                    If AXDLL.PControlFecha = True Then
                        AXDLL.Procesar()
                        Me.ButtonConvertir.Enabled = True
                    End If




                    Me.mDuracion = DateDiff(DateInterval.Second, Me.mInicio, Now) / (24 * 3600)
                    Me.TextBoxDuracion.Text = (Format(Date.FromOADate(Me.mDuracion), "mm 'minutos ' ss ' segundos'"))

                    '' Hoteles TITO
                ElseIf DLL = 4 Then
                    Me.CampoFecha = " ASNT_F_VALOR "
                    If Me.RadioButtonGastosPorAlbaran.Checked Then
                        Me.mTipoGasto = "0"
                    Else
                        Me.mTipoGasto = "1"
                    End If

                    Me.TextBoxDebug.Text = "Preparando Objeto y Abriendo Conexiones ..."
                    Me.TextBoxDebug.Update()
                    Me.Cursor = Cursors.AppStarting
                    INTEGRA = New IntegraNewStockTito.IntegraNewStockTito(CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 0)),
                    CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 1)),
                    MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                    CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 3)), Me.DateTimePicker1.Value, "A" _
                    & Format(Me.DateTimePicker1.Value, "dd-MM-yyyy") & ".TXT", Me.CheckBoxCondensar.Checked, Me.TextBoxDebug,
                    Me.ListBoxDebug, Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 4), False, False,
                    Me.mEmpNum, Me, Me.ProgressBar1, Me.mConectaNewpaga, Me.mStrConexionNewPaga, Me.mTipoGasto, Me.CheckBoxCondensar.Checked)
                    Me.Cursor = Cursors.AppStarting


                    Me.mInicio = Now


                    Me.TextBoxDuracion.Text = Format(Now, "dd/MM/yyyy HH:mm:ss")
                    Me.Timer1.Enabled = False
                    Me.PictureBox2.Visible = False


                    If Me.mParaSoloFacturas = 0 Then
                        Application.DoEvents()
                        INTEGRA.Procesar()
                    Else
                        Application.DoEvents()
                        INTEGRA.PROCESARSOLOFACTURAS()
                    End If


                    Me.mDuracion = DateDiff(DateInterval.Second, Me.mInicio, Now) / (24 * 3600)
                    Me.TextBoxDuracion.Text = (Format(Date.FromOADate(Me.mDuracion), "mm 'minutos ' ss ' segundos'"))

                    If Me.ListBoxDebug.Items.Count > 0 Then
                        Me.PictureBox2.Visible = True
                        Me.Timer1.Enabled = True
                        Dim Texto As String
                        Texto = " (1)  Existen INCIDENCIAS en el Proceso de Validación de Cuentas " & vbCrLf & vbCrLf
                        Texto += " (2) NO Integre el Fichero en esta Situación " & vbCrLf & vbCrLf
                        Texto += " (3) CORRIJA las Incidencias y VUELVA a GENERAR el Fichero hasta que quede Libre de INCIDENCIAS  " & vbCrLf & vbCrLf
                        Texto += " (4) Las Incidencias puede Verlas/Imprimirlas en la Parte Inferior de la Pantalla Anterior"
                        MsgBox(Texto, MsgBoxStyle.Exclamation, "Atención Lea")
                    End If



                Else
                    MsgBox("No hay DLL escogida en Fichero Ini revise ALMACEN= en Fichero INI ", MsgBoxStyle.Information, "Atención")
                    Me.Close()
                End If


                If Me.CheckBoxDebug.Checked = True Then
                    ' Me.TextBox1.Text = INTEGRA.LiquidoServiciosConIgic
                    ' Me.TextBox2.Text = INTEGRA.LiquidoServiciosSinIgic
                    ' Me.TextBox3.Text = INTEGRA.LiquidoDesembolsos
                    Me.Update()
                End If

            End If


            Me.Cursor = Cursors.Default

            Me.DataGrid2.CaptionText = Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 2) & "  " & Me.DateTimePicker1.Value.ToLongDateString & " con Perfil Contable = " & PERFILCONTABLE
            Me.DataGrid2.Update()


            SQL = "SELECT SUM(ASNT_DEBE)FROM TS_ASNT WHERE " & Me.CampoFecha & "  = '" & Me.DateTimePicker1.Value & "'"
            SQL += " AND TS_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum



            Me.TextBoxDebug.Text = Me.DbLee.EjecutaSqlScalar(SQL)

            SQL = "SELECT SUM(ASNT_HABER)FROM TS_ASNT WHERE " & Me.CampoFecha & "  = '" & Me.DateTimePicker1.Value & "'"

            SQL += " AND TS_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum


            Me.TextBoxDebug.Text = Me.TextBoxDebug.Text & "   " & Me.DbLee.EjecutaSqlScalar(SQL)


            SQL = "SELECT ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_EMP_NUM,ASNT_ALMA_CODI,ASNT_F_ATOCAB FECHA,ASNT_F_VALOR AS ""FECHA DE VALOR"", ASNT_CFCTA_COD AS CUENTA, "
            SQL += "ASNT_AMPCPTO AS CONCEPTO,ASNT_DOCU AS DOCUMENTO,NVL(ASNT_AUXILIAR_STRING,'?') AS ORIGEN , "
            SQL += "NVL(ASNT_AUXILIAR_STRING2,'?') AS CIF, NVL(ASNT_AUXILIAR_STRING,'?')AS ORIGEN, ASNT_DEBE AS DEBE, ASNT_HABER AS HABER,ASNT_ALMA_DESC  "
            SQL += "FROM TS_ASNT WHERE " & Me.CampoFecha & "  = '" & Me.DateTimePicker1.Value & "'"

            SQL += " AND TS_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum


            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"
            Me.DataGrid2.DataSource = DbLee.TraerDataset(SQL, "ASIENTO")
            Me.DataGrid2.DataMember = "ASIENTO"


            If Me.DbLee.mDbDataset.Tables(0).Rows.Count > 0 Then

                Me.ButtonImprimir.Enabled = True
                Me.Button2.Enabled = True

            Else
                Me.ButtonImprimir.Enabled = False
                Me.Button2.Enabled = False

            End If


            Me.ConfGrid()


            Me.ParentForm.WindowState = FormWindowState.Maximized
            Me.ParentForm.Text += "Menu"
            Me.ParentForm.Update()
            Application.DoEvents()


            Me.WindowState = FormWindowState.Maximized
            Me.Update()
            Application.DoEvents()


        Catch EX As Exception
            MsgBox(EX.Message)

            Me.ParentForm.WindowState = FormWindowState.Maximized
            Me.ParentForm.Text += "Menu"
            Me.ParentForm.Update()
            Application.DoEvents()


            Me.WindowState = FormWindowState.Maximized
            Me.Update()
            Application.DoEvents()
        End Try

    End Sub

    Private Sub MostrarAsiento()
        Try

            DLL = CType(MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DLL", "ALMACEN"), Integer)
            If DLL = 1 Then
                Me.CampoFecha = " ASNT_F_ATOCAB "
            ElseIf DLL = 2 Then
                Me.CampoFecha = " ASNT_F_VALOR "
            ElseIf DLL = 3 Then
                Me.CampoFecha = " ASNT_F_VALOR "
            Else
                'OJO 
                Me.CampoFecha = " ASNT_F_VALOR "
            End If


            SQL = "SELECT SUM(ASNT_DEBE)FROM TS_ASNT WHERE " & Me.CampoFecha & "  = '" & Me.DateTimePicker1.Value & "'"
            SQL += " AND TS_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum



            Me.TextBoxDebug.Text = Me.DbLee.EjecutaSqlScalar(SQL)

            SQL = "SELECT SUM(ASNT_HABER)FROM TS_ASNT WHERE " & Me.CampoFecha & "  = '" & Me.DateTimePicker1.Value & "'"

            SQL += " AND TS_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum


            Me.TextBoxDebug.Text = Me.TextBoxDebug.Text & "   " & Me.DbLee.EjecutaSqlScalar(SQL)


            SQL = "SELECT ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_EMP_NUM,ASNT_ALMA_CODI,ASNT_F_ATOCAB FECHA,ASNT_F_VALOR AS ""FECHA DE VALOR"", ASNT_CFCTA_COD AS CUENTA, "
            SQL += "ASNT_AMPCPTO AS CONCEPTO,ASNT_DOCU AS DOCUMENTO,NVL(ASNT_AUXILIAR_STRING,'?') AS ORIGEN , "
            SQL += "NVL(ASNT_AUXILIAR_STRING2,'?') AS CIF, NVL(ASNT_AUXILIAR_STRING,'?')AS ORIGEN, ASNT_DEBE AS DEBE, ASNT_HABER AS HABER,ASNT_ALMA_DESC  "
            SQL += "FROM TS_ASNT WHERE " & Me.CampoFecha & "  = '" & Me.DateTimePicker1.Value & "'"

            SQL += " AND TS_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_NUM = " & Me.mEmpNum


            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"
            Me.DataGrid2.DataSource = DbLee.TraerDataset(SQL, "ASIENTO")
            Me.DataGrid2.DataMember = "ASIENTO"
            Me.ConfGrid()

            ' INCIDENCIAS
            
            SQL = "SELECT INCI_DATR,INCI_ORIGEN,NVL(INCI_DESCRIPCION,'?') AS INCI_DESCRIPCION  FROM TH_INCI WHERE INCI_DATR = '" & Format(Me.DateTimePicker1.Value, "dd/MM/yyyy") & "'"
            SQL += " AND  INCI_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND  INCI_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND  INCI_EMP_NUM = " & Me.mEmpNum
            SQL += " GROUP BY INCI_DATR,INCI_ORIGEN,INCI_DESCRIPCION "
            SQL += " ORDER BY INCI_DATR ASC"

            Me.ListBoxDebug.Items.Clear()

            Me.DbLee.TraerLector(SQL)
            While Me.DbLee.mDbLector.Read
                Me.ListBoxDebug.Items.Add(Me.DbLee.mDbLector("INCI_DESCRIPCION"))


            End While
            Me.DbLee.mDbLector.Close()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
#Region "DISEÑO"
    Private Sub ConfGrid()

        Try
            Dim ts1 As New DataGridTableStyle

            ts1.MappingName = "ASIENTO"


            Dim TextCol1A As New DataGridTextBoxColumn
            TextCol1A.MappingName = "ASNT_EMPGRUPO_COD"
            TextCol1A.HeaderText = "ASNT_EMPGRUPO_COD"
            TextCol1A.Width = 150
            ts1.GridColumnStyles.Add(TextCol1A)


            Dim TextColB As New DataGridTextBoxColumn
            TextColB.MappingName = "ASNT_EMP_COD"
            TextColB.HeaderText = "ASNT_EMP_COD"
            TextColB.Width = 100
            ts1.GridColumnStyles.Add(TextColB)


            Dim TextColC As New DataGridTextBoxColumn
            TextColC.MappingName = "ASNT_EMP_NUM"
            TextColC.HeaderText = "ASNT_EMP_NUM"
            TextColC.Width = 100
            ts1.GridColumnStyles.Add(TextColC)

            Dim TextColD As New DataGridTextBoxColumn
            TextColD.MappingName = "ASNT_ALMA_CODI"
            TextColD.HeaderText = "ASNT_ALMA_CODI"
            TextColD.Width = 100
            ts1.GridColumnStyles.Add(TextColD)



            Dim TextColE As New DataGridTextBoxColumn
            TextColE.MappingName = "ASNT_ALMA_DESC"
            TextColE.HeaderText = "Descripción del Origen"
            TextColE.Width = 300
            TextColE.NullText = "_"
            ts1.GridColumnStyles.Add(TextColE)


            Dim TextCol1 As New DataGridTextBoxColumn
            TextCol1.MappingName = "F_VALOR"
            TextCol1.HeaderText = "F. Valor"
            TextCol1.Width = 100
            ts1.GridColumnStyles.Add(TextCol1)


            Dim TextCol2 As New DataGridTextBoxColumn
            TextCol2.MappingName = "CUENTA"
            TextCol2.HeaderText = "Cuenta"
            TextCol2.Width = 100
            ts1.GridColumnStyles.Add(TextCol2)



            Dim TextCol3 As New DataGridTextBoxColumn
            TextCol3.MappingName = "CONCEPTO"
            TextCol3.HeaderText = "Concepto"
            TextCol3.Width = 300

            ts1.GridColumnStyles.Add(TextCol3)

            Dim TextCol3A As New DataGridTextBoxColumn
            TextCol3A.MappingName = "DOCUMENTO"
            TextCol3A.HeaderText = "Documento"
            TextCol3A.Width = 100

            ts1.GridColumnStyles.Add(TextCol3A)


            Dim TextCol4 As New DataGridTextBoxColumn
            TextCol4.MappingName = "DEBE"
            TextCol4.HeaderText = "Debe"
            TextCol4.Width = 150
            TextCol4.NullText = "_"
            ts1.GridColumnStyles.Add(TextCol4)


            Dim TextCol5 As New DataGridTextBoxColumn
            TextCol5.MappingName = "HABER"
            TextCol5.HeaderText = "Haber"
            TextCol5.Width = 150
            TextCol5.NullText = "_"
            ts1.GridColumnStyles.Add(TextCol5)


            Dim TextCol6 As New DataGridTextBoxColumn
            TextCol6.MappingName = "CIF"
            TextCol6.HeaderText = "CIF"
            TextCol6.Width = 100
            TextCol6.NullText = "_"
            ts1.GridColumnStyles.Add(TextCol6)

            Dim TextCol7 As New DataGridTextBoxColumn
            TextCol7.MappingName = "ORIGEN"
            TextCol7.HeaderText = "Tipo de Origen"
            TextCol7.Width = 100
            TextCol7.NullText = "_"
            ts1.GridColumnStyles.Add(TextCol7)



            ' If DataGrid2.TableStyles.Contains(ts1) Then
            ' Me.DataGrid2.TableStyles.Clear()
            ' End If
            DataGrid2.TableStyles.Clear()
            DataGrid2.TableStyles.Add(ts1)


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub ConfGridHoteles()

        Try
            Dim ts2 As New DataGridTableStyle

            ts2.MappingName = "HOTELES"

            Dim TextCol1 As New DataGridTextBoxColumn
            TextCol1.MappingName = "HOTEL_EMPGRUPO_COD"
            TextCol1.HeaderText = "Grupo de Empresas"
            TextCol1.Width = 100

            ts2.GridColumnStyles.Add(TextCol1)


            Dim TextCol2 As New DataGridTextBoxColumn
            TextCol2.MappingName = "HOTEL_EMP_COD"
            TextCol2.HeaderText = "Empresa"
            TextCol2.Width = 100
            ts2.GridColumnStyles.Add(TextCol2)



            Dim TextCol3 As New DataGridTextBoxColumn
            TextCol3.MappingName = "HOTEL_DESCRIPCION"
            TextCol3.HeaderText = "Nombre"
            TextCol3.Width = 300

            ts2.GridColumnStyles.Add(TextCol3)

            Dim TextCol3A As New DataGridTextBoxColumn
            TextCol3A.MappingName = "HOTEL_ODBC_ALMACEN"
            TextCol3A.HeaderText = "Odbc"
            TextCol3A.Width = 0

            ts2.GridColumnStyles.Add(TextCol3A)


            Dim TextCol4 As New DataGridTextBoxColumn
            TextCol4.MappingName = "HOTEL_SPYRO"
            TextCol4.HeaderText = "Odbc2"
            TextCol4.Width = 0
            TextCol4.NullText = "_"

            ts2.GridColumnStyles.Add(TextCol4)



            Dim TextCol5 As New DataGridTextBoxColumn
            TextCol5.MappingName = "HOTEL_EMP_NUM"
            TextCol5.HeaderText = "Emp. Num"
            TextCol5.Width = 150
            TextCol5.NullText = "_"

            ts2.GridColumnStyles.Add(TextCol5)




            DataGrid1.TableStyles.Clear()
            DataGrid1.TableStyles.Add(ts2)


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
#End Region

    Private Sub ButtonInventarios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonInventarios.Click
        Try
            Dim Form As New FormInventarios(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 0), Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 1), Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 3), Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 4))
            Form.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub


    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            REPORT_SELECTION_FORMULA = "{TS_ASNT." & Me.CampoFecha.Trim & "}=DATETIME(" & Format(Me.DateTimePicker1.Value, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += " AND {TS_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TS_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"



            Dim Form As New FormVisorCrystal("ASIENTO_ALMACEN.RPT", Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 2), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), "", False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub DataGrid1_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid1.CurrentCellChanged
        Try
            If Me.HayRegistros = True Then
                Me.DataGrid2.CaptionText = CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 2)) & " con Perfil Contable = " & PERFILCONTABLE
                Me.DataGrid2.Update()
                Me.mEmpGrupoCod = CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 0))
                Me.mEmpCod = CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 1))
                Me.mEmpNum = Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 5)
                Me.ActualizaPatametrosHotel()
                Me.GetUltimaFechaNewStock()

                '13/05/2013
                Me.MostrarAsiento()
            End If


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub ButtonConvertir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonConvertir.Click
        Dim CadenaConexion As String = MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING")


        If Me.CheckBoxDebug.Checked = False Then
            Dim Form As New FormConvertir(Me.DateTimePicker1.Value, CadenaConexion, Me.mEmpGrupoCod, Me.mEmpCod, Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 2), True, "NEWSTOCK")
            Form.ShowDialog()
        Else
            Dim Form As New FormConvertir(Me.DateTimePicker1.Value, CadenaConexion, Me.mEmpGrupoCod, Me.mEmpCod, Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 2), False, "NEWSTOCK")
            Form.ShowDialog()
        End If



        ' AXAPTA WEB SERVICES
        If Me.DLL = 3 Then
            If ESTACARGADOFORMCONVERTIR = False Then
                ESTACARGADOFORMCONVERTIR = True
                Dim Form As New FormConvertirAxaptaCompras(Me.DateTimePicker1.Value, CadenaConexion, Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum, Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 3))
                Form.MdiParent = Me.MdiParent
                Form.Show()
                Exit Sub
            Else
                My.Forms.FormMenu.LayoutMdi(MdiLayout.TileHorizontal)
                Exit Sub
            End If
        End If


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Me.ButtonConvertir.Enabled = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ActualizaPatametrosHotel()
        Try
            SQL = "SELECT NVL(PARA_FILE_SPYRO_PATH,'?') FROM TS_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
            Me.TextBoxRutaFicheros.Text = Me.DbLee.EjecutaSqlScalar(SQL)


            SQL = "SELECT NVL(PARA_TIPO_FORMALIZA,'?') FROM TS_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
            If Me.DbLee.EjecutaSqlScalar(SQL) = "P" Then
                Me.RadioButtonFormalizaProveedor.Checked = True
                Me.RadioButtonFormalizaGenerico.Checked = False
            Else
                Me.RadioButtonFormalizaGenerico.Checked = True
                Me.RadioButtonFormalizaProveedor.Checked = False
            End If

            SQL = "SELECT NVL(PARA_CONECTA_NEWPAGA,0) FROM TS_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
            If CInt(Me.DbLee.EjecutaSqlScalar(SQL)) = 1 Then
                Me.mConectaNewpaga = True
            Else
                Me.mConectaNewpaga = False
            End If


            SQL = "SELECT NVL(PARA_SOLO_FACTURAS,0) FROM TS_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.mEmpNum
            Me.mParaSoloFacturas = CInt(Me.DbLee.EjecutaSqlScalar(SQL))


            SQL = "SELECT NVL(HOTEL_ODBC_NEWPAGA,'?') FROM TH_HOTEL "
            SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND HOTEL_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND HOTEL_EMP_NUM = " & Me.mEmpNum
            Me.mStrConexionNewPaga = Me.DbLee.EjecutaSqlScalar(SQL)


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub DataGrid1_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs) Handles DataGrid1.Navigate
       
    End Sub

    Private Sub ButtonIncidencias_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonIncidencias.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            '  If MULTIFECHA = "0" Then
            REPORT_SELECTION_FORMULA = "{TH_INCI.INCI_DATR}=DATETIME(" & Format(Me.DateTimePicker1.Value, REPORT_DATE_FORMAT) & ")"
            '     Else
            '        REPORT_SELECTION_FORMULA = "{TH_INCI.INCI_DATR} >= DATETIME(" & Format(Me.DateTimePicker1.Value, REPORT_DATE_FORMAT) & ")"
            '       REPORT_SELECTION_FORMULA += " AND {TH_INCI.INCI_DATR} <= DATETIME(" & Format(Me.DateTimePicker2.Value, REPORT_DATE_FORMAT) & ")"
            ' End If

            REPORT_SELECTION_FORMULA += " AND {TH_INCI.INCI_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TH_INCI.INCI_EMP_COD}= '" & Me.mEmpCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TH_INCI.INCI_EMP_NUM}= " & Me.mEmpNum

            Dim Form As New FormVisorCrystal("TH_INCI.RPT", CType(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 2), String), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), CType(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 3), String), False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub ButtonUpdateFilePAth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUpdateFilePAth.Click
        Try
            Me.FolderBrowserDialog1.ShowDialog()
            If IsNothing(Me.FolderBrowserDialog1.SelectedPath) = False Then
                If Me.FolderBrowserDialog1.SelectedPath.Length > 0 Then
                    Me.TextBoxRutaFicheros.Text = Me.FolderBrowserDialog1.SelectedPath & "\"
                    SQL = "UPDATE TS_PARA SET PARA_FILE_SPYRO_PATH = '" & Me.TextBoxRutaFicheros.Text & "'"
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

    Private Sub ButtonFechas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFechas.Click
        Try
            If mEmpGrupoCod = "SATO" Then
                Dim F As New FormFechasPendientesdeProcesar(CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 3)), STRCONEXIONCENTRAL, Me.mEmpGrupoCod, Me.mEmpCod, Me.mEmpNum)
                F.MdiParent = Me.ParentForm
                '  F.StartPosition = FormStartPosition.Manual
                F.Width = 708
                F.Height = 264

                '   F.StartPosition = FormStartPosition.CenterScreen
                '  F.WindowState = FormWindowState.Normal

                F.Show()


                '  Me.FechasPosiblesSatocan()
            Else
                Me.FechasPosibles()
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub FechasPosibles()
        Try
            Dim Texto As String = ""
            If HayRegistros = True Then
                If CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 3)).Length > 0 Then

                    Me.Cursor = Cursors.WaitCursor

                    Dim DbLee As New C_DATOS.C_DatosOledb
                    DbLee.StrConexion = CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 3))
                    DbLee.AbrirConexion()
                    SQL = "SELECT DISTINCT (MOVG_DAVA) AS MOVG_DAVA FROM TNST_MOVG "
                    SQL += " WHERE  TO_CHAR(MOVG_DAVA,'YYYY') = '" & Year(Me.DateTimePicker1.Value) & "'"

                    SQL += " AND  TO_CHAR(MOVG_DAVA,'MM') = '" & Format(Me.DateTimePicker1.Value, "MM") & "'"
                    SQL += " ORDER BY MOVG_DAVA ASC"
                    DbLee.TraerLector(SQL)
                    While DbLee.mDbLector.Read
                        Texto += DbLee.mDbLector.Item("MOVG_DAVA") & vbCrLf
                    End While
                    DbLee.mDbLector.Close()
                    DbLee.CerrarConexion()

                    MsgBox(Texto, MsgBoxStyle.Information, CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 2)))
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub GetUltimaFechaNewStock()
        Try
            Dim Texto As String = ""
            Dim FechaUltimoMovimiento As Date
            Dim FechaTrabajoNewStock As Date


            If HayRegistros = True Then
                If CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 3)).Length > 0 Then

                    Me.Cursor = Cursors.WaitCursor

                    Dim DbLee As New C_DATOS.C_DatosOledb
                    DbLee.StrConexion = CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 3))
                    DbLee.AbrirConexion()

                    SQL = " SELECT PARA_FETR FROM TNST_PARA "
                    FechaTrabajoNewStock = CDate(DbLee.EjecutaSqlScalar(SQL))

                    SQL = " SELECT MAX(MOVG_DAVA) FROM TNST_MOVG "
                    FechaUltimoMovimiento = CDate(DbLee.EjecutaSqlScalar(SQL))


                    If FechaUltimoMovimiento < FechaTrabajoNewStock Then
                        Me.DateTimePicker1.Value = FechaUltimoMovimiento
                    Else
                        SQL = " SELECT MAX(MOVG_DAVA) FROM TNST_MOVG "
                        SQL = SQL & " "
                        SQL = SQL & "WHERE  MOVG_DAVA NOT IN(SELECT MAX(MOVG_DAVA) FROM TNST_MOVG) "
                        FechaUltimoMovimiento = CDate(DbLee.EjecutaSqlScalar(SQL))
                        Me.DateTimePicker1.Value = FechaUltimoMovimiento
                    End If


                    DbLee.CerrarConexion()


                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

            Me.Cursor = Cursors.Default
        End Try

    End Sub
    Private Sub FechasPosiblesSatocan()
        Try
            Dim Texto As String = ""
            If HayRegistros = True Then
                If CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 3)).Length > 0 Then

                    Me.Cursor = Cursors.WaitCursor

                    Dim DbLee As New C_DATOS.C_DatosOledb
                    DbLee.StrConexion = CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 3))
                    DbLee.AbrirConexion()
                    SQL = "SELECT DISTINCT (MOVG_DAVA) AS MOVG_DAVA FROM TNST_MOVG "
                    SQL += " WHERE  TO_CHAR(MOVG_DAVA,'YYYY') = '" & Year(Me.DateTimePicker1.Value) & "'"
                    SQL += " ORDER BY MOVG_DAVA ASC"
                    DbLee.TraerLector(SQL)
                    While DbLee.mDbLector.Read
                        Texto += DbLee.mDbLector.Item("MOVG_DAVA") & vbCrLf
                    End While
                    DbLee.mDbLector.Close()
                    DbLee.CerrarConexion()

                    MsgBox(Texto, MsgBoxStyle.Information, CStr(Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 2)))
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            Dim Rname As String


            If Me.mEmpGrupoCod = "GRTI" Then
                Rname = "ASIENTO_ALMACENTITO.RPT"
            Else
                Rname = "ASIENTO_ALMACEN3.RPT"
            End If

            REPORT_SELECTION_FORMULA = "{TS_ASNT." & Me.CampoFecha.Trim & "}=DATETIME(" & Format(Me.DateTimePicker1.Value, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += " And {TS_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TS_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"



            Dim Form As New FormVisorCrystal(Rname, Me.DataGrid1.Item(Me.DataGrid1.CurrentRowIndex, 2), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), "", False, False)
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


    Private Sub CheckBoxCondensar_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxCondensar.CheckedChanged
        Try
            If Me.CheckBoxCondensar.Checked Then
                Me.RadioButtonGastosPorAlbaran.Enabled = True
                Me.RadioButtonGastosPorFAmilia.Enabled = True
                Me.RadioButtonGastosPorFAmilia.Checked = True
                Exit Sub
            End If
            If Me.CheckBoxCondensar.Checked = False Then
                Me.RadioButtonGastosPorAlbaran.Enabled = False
                Me.RadioButtonGastosPorFAmilia.Enabled = False
                Me.RadioButtonGastosPorAlbaran.Checked = False
                Me.RadioButtonGastosPorFAmilia.Checked = False
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonImprimirErrores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimirErrores.Click
        Try

            Dim F As New FormPideFechas(Now)
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



            Dim Form As New FormVisorCrystal("ASIENTO_ALMACEN Axapta Errores.RPT", "Errores " & Format(FEC1, "dd/MM/yyyy") & "   " & Format(FEC2, "dd/MM/yyyy"), REPORT_SELECTION_FORMULA, STRCONEXIONCENTRAL, "", False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub ButtonImprimirEstadodeEnvios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimirEstadodeEnvios.Click
        Try


            Dim F As New FormPideFechas(Now)
            CONTROLF = False
            F.ShowDialog()

            If CONTROLF = False Then
                ' salio con cancelar del form pide fechas 
                Exit Sub
            End If
            Me.Cursor = Cursors.WaitCursor

            REPORT_SELECTION_FORMULA = "{TS_ASNT.ASNT_F_VALOR}>=DATETIME(" & Format(FEC1, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += "AND {TS_ASNT.ASNT_F_VALOR}<=DATETIME(" & Format(FEC2, REPORT_DATE_FORMAT) & ")"

            REPORT_SELECTION_FORMULA += " AND {TS_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TS_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"



            Dim Form As New FormVisorCrystal("ASIENTO_ALMACEN Axapta.RPT", "Estado de Envíos " & Format(FEC1, "dd/MM/yyyy") & "   " & Format(FEC2, "dd/MM/yyyy"), REPORT_SELECTION_FORMULA, STRCONEXIONCENTRAL, "", False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub ButtonImprimirEnviosPendintes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimirEnviosPendintes.Click
        Try


            Dim F As New FormPideFechas(Now)
            CONTROLF = False
            F.ShowDialog()

            If CONTROLF = False Then
                ' salio con cancelar del form pide fechas 
                Exit Sub
            End If
            Me.Cursor = Cursors.WaitCursor

            REPORT_SELECTION_FORMULA = "{TS_ASNT.ASNT_F_VALOR}>=DATETIME(" & Format(FEC1, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += "AND {TS_ASNT.ASNT_F_VALOR}<=DATETIME(" & Format(FEC2, REPORT_DATE_FORMAT) & ")"

            REPORT_SELECTION_FORMULA += " AND {TS_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TS_ASNT.ASNT_EMP_COD}= '" & Me.mEmpCod & "'"

            REPORT_SELECTION_FORMULA += " AND  {TS_ASNT.ASNT_AX_STATUS} <> 1 "





            Dim Form As New FormVisorCrystal("Asiento_Almacen Axapta Envios Pendientes.rpt", "Envíos Pendientes " & Format(FEC1, "dd/MM/yyyy") & "   " & Format(FEC2, "dd/MM/yyyy"), REPORT_SELECTION_FORMULA, STRCONEXIONCENTRAL, "", False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub ButtonSatocanObjects_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSatocanObjects.Click
        Try
            Dim UserCas As String
            Dim StrConexion As String
            StrConexion = "Provider=MSDAORA.1;User Id=" & Me.TextBoxOracleUser.Text & ";"
            StrConexion += "Password=" & Me.TextBoxOraclePwd.Text & ";"
            StrConexion += "Data Source =" & Me.TextBoxOracleSid.Text


            Dim DB As New C_DATOS.C_DatosOledb(StrConexion, True)


            If DB.EstadoConexion <> ConnectionState.Open Then
                MsgBox("No Dispone de Acceso a la Base de Datos", MsgBoxStyle.Information, "Atención")
                Exit Sub
            End If


            ' Verifica cadena de conexion Newstock para poder dar permisos 

            SQL = "SELECT NVL(HOTEL_ODBC_ALMACEN,'?') FROM TH_HOTEL "
            SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND HOTEL_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND HOTEL_EMP_NUM = " & Me.mEmpNum

            RESULTSTR = DbLee.EjecutaSqlScalar(SQL)

            UserCas = StrConexionExtraeUsuario(RESULTSTR)


            Dim DBCAS As New C_DATOS.C_DatosOledb(RESULTSTR, True)


            ' verifica que existe usuario
            SQL = "SELECT USERNAME FROM ALL_USERS WHERE USERNAME ='SATOCAN'"

            RESULTSTR = DB.EjecutaSqlScalar(SQL)

            ' Crea Usuario
            If IsNothing(RESULTSTR) Then
                SQL = "CREATE USER SATOCAN "
                SQL += "IDENTIFIED BY SATOCAN "
                SQL += "DEFAULT TABLESPACE USERS "
                SQL += "TEMPORARY TABLESPACE TEMP "
                SQL += "PROFILE DEFAULT "
                SQL += "ACCOUNT UNLOCK "

                DB.EjecutaSqlScalarDML(SQL)

                SQL = "GRANT EXP_FULL_DATABASE TO SATOCAN"
                DB.EjecutaSqlScalarDML(SQL)
                SQL = "GRANT CONNECT TO SATOCAN"
                DB.EjecutaSqlScalarDML(SQL)
                SQL = "GRANT IMP_FULL_DATABASE TO SATOCAN"
                DB.EjecutaSqlScalarDML(SQL)
                SQL = "ALTER USER SATOCAN DEFAULT ROLE ALL"
                DB.EjecutaSqlScalarDML(SQL)
                SQL = "GRANT UNLIMITED TABLESPACE TO SATOCAN"
                DB.EjecutaSqlScalarDML(SQL)
                SQL = "GRANT SELECT ANY TABLE TO SATOCAN"
                DB.EjecutaSqlScalarDML(SQL)

            End If


            ' Crea Triggers

            SQL = "GRANT ALL ON TNST_FOPR TO SATOCAN"
            DBCAS.EjecutaSqlScalarDML(SQL)

            SQL = "GRANT ALL ON TNST_SCOD TO SATOCAN"
            DBCAS.EjecutaSqlScalarDML(SQL)

            SQL = "GRANT ALL ON TNST_MOVG TO SATOCAN"
            DBCAS.EjecutaSqlScalarDML(SQL)






            SQL = "CREATE OR REPLACE TRIGGER  SATOCAN.QWE_TNST_MOVG1 BEFORE INSERT ON " & UserCas & ".TNST_MOVG FOR EACH ROW"
            SQL += " BEGIN "
            SQL += " :NEW.MOVG_EXPO :=1; "
            SQL += " END; "

            DB.EjecutaSqlScalarDML(SQL)

            DB.CerrarConexion()
            DBCAS.CerrarConexion()

        Catch ex As Exception
            MsgBox(ex.Message)


        End Try

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try


            If Me.PictureBox2.Visible = True Then
                Me.PictureBox2.Visible = False
                Exit Sub
            End If

            If Me.PictureBox2.Visible = False Then
                Me.PictureBox2.Visible = True
                Exit Sub
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub TextBoxDuracion_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBoxDuracion.KeyDown
        Try
            '  If e.KeyCode = Keys.ControlKey And e.KeyCode = Keys.F8 Then
            If e.KeyCode = Keys.F8 Then

                If Me.CheckBoxCondensar.Enabled = False Then
                    Me.RadioButtonGastosPorAlbaran.Enabled = True
                    Me.RadioButtonGastosPorFAmilia.Enabled = True
                    Me.CheckBoxCondensar.Enabled = True
                    Me.CheckBoxTextoApuntes.Enabled = True
                    Exit Sub
                Else
                    Me.RadioButtonGastosPorAlbaran.Enabled = False
                    Me.RadioButtonGastosPorFAmilia.Enabled = False
                    Me.CheckBoxCondensar.Enabled = False
                    Me.CheckBoxTextoApuntes.Enabled = False
                    Exit Sub
                End If

            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub TextBoxDuracion_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxDuracion.TextChanged

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

    End Sub

    Private Sub FormIntegraAlmacen_Shown(sender As Object, e As EventArgs) Handles Me.Shown

    End Sub
End Class
