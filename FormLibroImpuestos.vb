Imports System.IO
Public Class FormLibroImpuestos
    Inherits System.Windows.Forms.Form
    Dim DbLeeHotel As C_DATOS.C_DatosOledb
    Dim DbLeeHotelAux As C_DATOS.C_DatosOledb
    Dim DbCentral As C_DATOS.C_DatosOledb
    Dim mStrConexionHotel As String
    Dim mStrConexionCentral As String
    Dim SQL As String
    Private mClientesContadoCif As String
    Private FileEstaOk As Boolean = False
    Private Filegraba As StreamWriter

    Private mEmpGrupoCod As String
    Private mEmpCod As String
    Private mNombreHotel As String
    Private mFormato As String
    Private mvFile As String
    Private mvFilePath As String
    Private mvFilePrefijo As String
    Private Separador As String = ","
    Private mConectaGolf As String
    Friend WithEvents CheckBoxModificaFormasdeCobro As System.Windows.Forms.CheckBox
    Private mParaUsuarioNewGolf As String
    Private NEWHOTEL As NewHotel.NewHotelData
    Friend WithEvents ButtonImprimirLibro As System.Windows.Forms.Button
    Friend WithEvents ButtonCancelar As System.Windows.Forms.Button
    Friend WithEvents ButtonImprimirFacturas As System.Windows.Forms.Button
    Private NEWGOLF As NewGolf.NewGolfData



#Region " Código generado por el Diseñador de Windows Forms "

    Public Sub New()
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()

    End Sub
    Public Sub New(ByVal vStrConexion As String, ByVal vStrConexionSpyro As String, ByVal vEmpgrupo_Cod As String, ByVal vEmp_Cod As String, ByVal vNombreHotel As String)
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()
        Me.mStrConexionHotel = vStrConexion
        Me.mStrConexionCentral = vStrConexionSpyro
        Me.mEmpGrupoCod = vEmpgrupo_Cod
        Me.mEmpCod = vEmp_Cod
        Me.mNombreHotel = vNombreHotel


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
    Friend WithEvents DateTimePickerDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimePickerHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents ListBoxDebug As System.Windows.Forms.ListBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents TextBoxDebug As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxCambiaSerie As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBoxFormaCredito As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxFormaContado As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButtonCuentaCliente As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonCodigoCliente As System.Windows.Forms.RadioButton
    Friend WithEvents TextBoxIgic1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxIgic2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxIgic3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxIgic4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxIgic5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxIgic6 As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxDebug As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBoxImpuestos As System.Windows.Forms.GroupBox
    Friend WithEvents TextBoxDebugCursor As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormLibroImpuestos))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.ButtonCancelar = New System.Windows.Forms.Button
        Me.CheckBoxDebug = New System.Windows.Forms.CheckBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.DateTimePickerHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimePickerDesde = New System.Windows.Forms.DateTimePicker
        Me.TextBoxDebugCursor = New System.Windows.Forms.TextBox
        Me.GroupBoxImpuestos = New System.Windows.Forms.GroupBox
        Me.TextBoxIgic6 = New System.Windows.Forms.TextBox
        Me.TextBoxIgic5 = New System.Windows.Forms.TextBox
        Me.TextBoxIgic4 = New System.Windows.Forms.TextBox
        Me.TextBoxIgic3 = New System.Windows.Forms.TextBox
        Me.TextBoxIgic2 = New System.Windows.Forms.TextBox
        Me.TextBoxIgic1 = New System.Windows.Forms.TextBox
        Me.ListBoxDebug = New System.Windows.Forms.ListBox
        Me.TextBoxDebug = New System.Windows.Forms.TextBox
        Me.CheckBoxCambiaSerie = New System.Windows.Forms.CheckBox
        Me.TextBoxFormaCredito = New System.Windows.Forms.TextBox
        Me.TextBoxFormaContado = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.CheckBoxModificaFormasdeCobro = New System.Windows.Forms.CheckBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.RadioButtonCodigoCliente = New System.Windows.Forms.RadioButton
        Me.RadioButtonCuentaCliente = New System.Windows.Forms.RadioButton
        Me.ButtonImprimirLibro = New System.Windows.Forms.Button
        Me.ButtonImprimirFacturas = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.GroupBoxImpuestos.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.ButtonCancelar)
        Me.GroupBox1.Controls.Add(Me.CheckBoxDebug)
        Me.GroupBox1.Controls.Add(Me.ButtonAceptar)
        Me.GroupBox1.Controls.Add(Me.DateTimePickerHasta)
        Me.GroupBox1.Controls.Add(Me.DateTimePickerDesde)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(688, 61)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Periodo de Fechas"
        '
        'ButtonCancelar
        '
        Me.ButtonCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonCancelar.Location = New System.Drawing.Point(590, 32)
        Me.ButtonCancelar.Name = "ButtonCancelar"
        Me.ButtonCancelar.Size = New System.Drawing.Size(88, 23)
        Me.ButtonCancelar.TabIndex = 5
        Me.ButtonCancelar.Text = "&Cancelar"
        '
        'CheckBoxDebug
        '
        Me.CheckBoxDebug.Checked = True
        Me.CheckBoxDebug.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxDebug.Location = New System.Drawing.Point(336, 24)
        Me.CheckBoxDebug.Name = "CheckBoxDebug"
        Me.CheckBoxDebug.Size = New System.Drawing.Size(82, 24)
        Me.CheckBoxDebug.TabIndex = 4
        Me.CheckBoxDebug.Text = "Debug"
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonAceptar.Location = New System.Drawing.Point(590, 9)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(88, 23)
        Me.ButtonAceptar.TabIndex = 3
        Me.ButtonAceptar.Text = "&Aceptar"
        '
        'DateTimePickerHasta
        '
        Me.DateTimePickerHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimePickerHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePickerHasta.Location = New System.Drawing.Point(176, 24)
        Me.DateTimePickerHasta.Name = "DateTimePickerHasta"
        Me.DateTimePickerHasta.Size = New System.Drawing.Size(152, 20)
        Me.DateTimePickerHasta.TabIndex = 2
        Me.DateTimePickerHasta.Value = New Date(2006, 4, 10, 0, 0, 0, 0)
        '
        'DateTimePickerDesde
        '
        Me.DateTimePickerDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimePickerDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePickerDesde.Location = New System.Drawing.Point(16, 24)
        Me.DateTimePickerDesde.Name = "DateTimePickerDesde"
        Me.DateTimePickerDesde.Size = New System.Drawing.Size(152, 20)
        Me.DateTimePickerDesde.TabIndex = 1
        Me.DateTimePickerDesde.Value = New Date(2006, 4, 10, 0, 0, 0, 0)
        '
        'TextBoxDebugCursor
        '
        Me.TextBoxDebugCursor.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxDebugCursor.Location = New System.Drawing.Point(638, 319)
        Me.TextBoxDebugCursor.Name = "TextBoxDebugCursor"
        Me.TextBoxDebugCursor.Size = New System.Drawing.Size(48, 20)
        Me.TextBoxDebugCursor.TabIndex = 5
        '
        'GroupBoxImpuestos
        '
        Me.GroupBoxImpuestos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxImpuestos.Controls.Add(Me.TextBoxIgic6)
        Me.GroupBoxImpuestos.Controls.Add(Me.TextBoxIgic5)
        Me.GroupBoxImpuestos.Controls.Add(Me.TextBoxIgic4)
        Me.GroupBoxImpuestos.Controls.Add(Me.TextBoxIgic3)
        Me.GroupBoxImpuestos.Controls.Add(Me.TextBoxIgic2)
        Me.GroupBoxImpuestos.Controls.Add(Me.TextBoxIgic1)
        Me.GroupBoxImpuestos.Controls.Add(Me.ListBoxDebug)
        Me.GroupBoxImpuestos.Location = New System.Drawing.Point(8, 72)
        Me.GroupBoxImpuestos.Name = "GroupBoxImpuestos"
        Me.GroupBoxImpuestos.Size = New System.Drawing.Size(472, 239)
        Me.GroupBoxImpuestos.TabIndex = 1
        Me.GroupBoxImpuestos.TabStop = False
        '
        'TextBoxIgic6
        '
        Me.TextBoxIgic6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxIgic6.Location = New System.Drawing.Point(288, 212)
        Me.TextBoxIgic6.Name = "TextBoxIgic6"
        Me.TextBoxIgic6.ReadOnly = True
        Me.TextBoxIgic6.Size = New System.Drawing.Size(48, 20)
        Me.TextBoxIgic6.TabIndex = 6
        Me.TextBoxIgic6.Tag = "IGIC"
        Me.TextBoxIgic6.Text = "9999"
        '
        'TextBoxIgic5
        '
        Me.TextBoxIgic5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxIgic5.Location = New System.Drawing.Point(232, 212)
        Me.TextBoxIgic5.Name = "TextBoxIgic5"
        Me.TextBoxIgic5.ReadOnly = True
        Me.TextBoxIgic5.Size = New System.Drawing.Size(48, 20)
        Me.TextBoxIgic5.TabIndex = 5
        Me.TextBoxIgic5.Tag = "IGIC"
        Me.TextBoxIgic5.Text = "9999"
        '
        'TextBoxIgic4
        '
        Me.TextBoxIgic4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxIgic4.Location = New System.Drawing.Point(176, 212)
        Me.TextBoxIgic4.Name = "TextBoxIgic4"
        Me.TextBoxIgic4.ReadOnly = True
        Me.TextBoxIgic4.Size = New System.Drawing.Size(48, 20)
        Me.TextBoxIgic4.TabIndex = 4
        Me.TextBoxIgic4.Tag = "IGIC"
        Me.TextBoxIgic4.Text = "9999"
        '
        'TextBoxIgic3
        '
        Me.TextBoxIgic3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxIgic3.Location = New System.Drawing.Point(120, 212)
        Me.TextBoxIgic3.Name = "TextBoxIgic3"
        Me.TextBoxIgic3.ReadOnly = True
        Me.TextBoxIgic3.Size = New System.Drawing.Size(48, 20)
        Me.TextBoxIgic3.TabIndex = 3
        Me.TextBoxIgic3.Tag = "IGIC"
        Me.TextBoxIgic3.Text = "9999"
        '
        'TextBoxIgic2
        '
        Me.TextBoxIgic2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxIgic2.Location = New System.Drawing.Point(64, 212)
        Me.TextBoxIgic2.Name = "TextBoxIgic2"
        Me.TextBoxIgic2.ReadOnly = True
        Me.TextBoxIgic2.Size = New System.Drawing.Size(48, 20)
        Me.TextBoxIgic2.TabIndex = 2
        Me.TextBoxIgic2.Tag = "IGIC"
        Me.TextBoxIgic2.Text = "9999"
        '
        'TextBoxIgic1
        '
        Me.TextBoxIgic1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxIgic1.Location = New System.Drawing.Point(8, 212)
        Me.TextBoxIgic1.Name = "TextBoxIgic1"
        Me.TextBoxIgic1.ReadOnly = True
        Me.TextBoxIgic1.Size = New System.Drawing.Size(48, 20)
        Me.TextBoxIgic1.TabIndex = 1
        Me.TextBoxIgic1.Tag = "IGIC"
        Me.TextBoxIgic1.Text = "9999"
        '
        'ListBoxDebug
        '
        Me.ListBoxDebug.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxDebug.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBoxDebug.HorizontalScrollbar = True
        Me.ListBoxDebug.ItemHeight = 14
        Me.ListBoxDebug.Location = New System.Drawing.Point(8, 10)
        Me.ListBoxDebug.Name = "ListBoxDebug"
        Me.ListBoxDebug.ScrollAlwaysVisible = True
        Me.ListBoxDebug.Size = New System.Drawing.Size(440, 186)
        Me.ListBoxDebug.TabIndex = 0
        '
        'TextBoxDebug
        '
        Me.TextBoxDebug.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxDebug.Location = New System.Drawing.Point(8, 319)
        Me.TextBoxDebug.Name = "TextBoxDebug"
        Me.TextBoxDebug.ReadOnly = True
        Me.TextBoxDebug.Size = New System.Drawing.Size(472, 20)
        Me.TextBoxDebug.TabIndex = 2
        '
        'CheckBoxCambiaSerie
        '
        Me.CheckBoxCambiaSerie.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxCambiaSerie.Checked = True
        Me.CheckBoxCambiaSerie.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxCambiaSerie.Location = New System.Drawing.Point(488, 319)
        Me.CheckBoxCambiaSerie.Name = "CheckBoxCambiaSerie"
        Me.CheckBoxCambiaSerie.Size = New System.Drawing.Size(176, 24)
        Me.CheckBoxCambiaSerie.TabIndex = 3
        Me.CheckBoxCambiaSerie.Text = "Truncar Longitud Serie"
        '
        'TextBoxFormaCredito
        '
        Me.TextBoxFormaCredito.Enabled = False
        Me.TextBoxFormaCredito.Location = New System.Drawing.Point(126, 48)
        Me.TextBoxFormaCredito.Name = "TextBoxFormaCredito"
        Me.TextBoxFormaCredito.Size = New System.Drawing.Size(72, 20)
        Me.TextBoxFormaCredito.TabIndex = 8
        Me.TextBoxFormaCredito.Text = "3"
        '
        'TextBoxFormaContado
        '
        Me.TextBoxFormaContado.Enabled = False
        Me.TextBoxFormaContado.Location = New System.Drawing.Point(126, 24)
        Me.TextBoxFormaContado.Name = "TextBoxFormaContado"
        Me.TextBoxFormaContado.Size = New System.Drawing.Size(72, 20)
        Me.TextBoxFormaContado.TabIndex = 7
        Me.TextBoxFormaContado.Text = "1"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(6, 48)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 23)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Código de Crédito"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(6, 24)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(104, 23)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Código de Contado"
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.CheckBoxModificaFormasdeCobro)
        Me.GroupBox3.Controls.Add(Me.TextBoxFormaCredito)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.TextBoxFormaContado)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Location = New System.Drawing.Point(488, 72)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(208, 99)
        Me.GroupBox3.TabIndex = 9
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Códigos Formas Cobro Datisa"
        '
        'CheckBoxModificaFormasdeCobro
        '
        Me.CheckBoxModificaFormasdeCobro.AutoSize = True
        Me.CheckBoxModificaFormasdeCobro.ForeColor = System.Drawing.Color.Maroon
        Me.CheckBoxModificaFormasdeCobro.Location = New System.Drawing.Point(11, 74)
        Me.CheckBoxModificaFormasdeCobro.Name = "CheckBoxModificaFormasdeCobro"
        Me.CheckBoxModificaFormasdeCobro.Size = New System.Drawing.Size(165, 17)
        Me.CheckBoxModificaFormasdeCobro.TabIndex = 9
        Me.CheckBoxModificaFormasdeCobro.Text = "Modificar Formas por Defecto"
        Me.CheckBoxModificaFormasdeCobro.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox4.Controls.Add(Me.RadioButtonCodigoCliente)
        Me.GroupBox4.Controls.Add(Me.RadioButtonCuentaCliente)
        Me.GroupBox4.Location = New System.Drawing.Point(488, 193)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(208, 66)
        Me.GroupBox4.TabIndex = 10
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Cuenta Contable /Código Cliente"
        '
        'RadioButtonCodigoCliente
        '
        Me.RadioButtonCodigoCliente.Location = New System.Drawing.Point(8, 40)
        Me.RadioButtonCodigoCliente.Name = "RadioButtonCodigoCliente"
        Me.RadioButtonCodigoCliente.Size = New System.Drawing.Size(176, 24)
        Me.RadioButtonCodigoCliente.TabIndex = 1
        Me.RadioButtonCodigoCliente.Text = "Usar Código de Cliente"
        '
        'RadioButtonCuentaCliente
        '
        Me.RadioButtonCuentaCliente.Checked = True
        Me.RadioButtonCuentaCliente.Location = New System.Drawing.Point(8, 16)
        Me.RadioButtonCuentaCliente.Name = "RadioButtonCuentaCliente"
        Me.RadioButtonCuentaCliente.Size = New System.Drawing.Size(184, 24)
        Me.RadioButtonCuentaCliente.TabIndex = 0
        Me.RadioButtonCuentaCliente.TabStop = True
        Me.RadioButtonCuentaCliente.Text = "Usar Cuenta de Contable"
        '
        'ButtonImprimirLibro
        '
        Me.ButtonImprimirLibro.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonImprimirLibro.Enabled = False
        Me.ButtonImprimirLibro.Image = CType(resources.GetObject("ButtonImprimirLibro.Image"), System.Drawing.Image)
        Me.ButtonImprimirLibro.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonImprimirLibro.Location = New System.Drawing.Point(499, 265)
        Me.ButtonImprimirLibro.Name = "ButtonImprimirLibro"
        Me.ButtonImprimirLibro.Size = New System.Drawing.Size(187, 24)
        Me.ButtonImprimirLibro.TabIndex = 13
        Me.ButtonImprimirLibro.Text = "&Imprimir Libro Impuesto"
        '
        'ButtonImprimirFacturas
        '
        Me.ButtonImprimirFacturas.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonImprimirFacturas.Image = CType(resources.GetObject("ButtonImprimirFacturas.Image"), System.Drawing.Image)
        Me.ButtonImprimirFacturas.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonImprimirFacturas.Location = New System.Drawing.Point(499, 289)
        Me.ButtonImprimirFacturas.Name = "ButtonImprimirFacturas"
        Me.ButtonImprimirFacturas.Size = New System.Drawing.Size(187, 24)
        Me.ButtonImprimirFacturas.TabIndex = 14
        Me.ButtonImprimirFacturas.Text = "&Imprimir Relación Documentos"
        '
        'FormLibroImpuestos
        '
        Me.AcceptButton = Me.ButtonAceptar
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(704, 353)
        Me.Controls.Add(Me.ButtonImprimirFacturas)
        Me.Controls.Add(Me.ButtonImprimirLibro)
        Me.Controls.Add(Me.TextBoxDebugCursor)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.CheckBoxCambiaSerie)
        Me.Controls.Add(Me.TextBoxDebug)
        Me.Controls.Add(Me.GroupBoxImpuestos)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimumSize = New System.Drawing.Size(712, 377)
        Me.Name = "FormLibroImpuestos"
        Me.Text = "Libro de Impuestos"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBoxImpuestos.ResumeLayout(False)
        Me.GroupBoxImpuestos.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try

            Me.Update()
            Me.ListBoxDebug.Items.Clear()
            Me.ListBoxDebug.Update()
            Me.DbLeeHotel = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
            Me.DbLeeHotel.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbLeeHotelAux = New C_DATOS.C_DatosOledb(Me.mStrConexionHotel)
            Me.DbLeeHotelAux.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbCentral = New C_DATOS.C_DatosOledb(Me.mStrConexionCentral)
            Me.DbCentral.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


            If Me.DateTimePickerHasta.Value >= Me.DateTimePickerDesde.Value Then
                ' CARGA PARAMETROS
                SQL = "SELECT NVL(PARA_FILE_SPYRO_PATH,'?') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                Me.mvFilePath = Me.DbCentral.EjecutaSqlScalar(SQL)

                SQL = "SELECT NVL(PARA_FILE_PREFIJO_IGIC,'-') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                Me.mvFilePrefijo = Me.DbCentral.EjecutaSqlScalar(SQL)

                SQL = "SELECT NVL(PARA_CONECTA_NEWGOLF,'0') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                Me.mConectaGolf = Me.DbCentral.EjecutaSqlScalar(SQL)

                SQL = "SELECT NVL(PARA_USUARIO_NEWGOLF,'?') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
                Me.mParaUsuarioNewGolf = Me.DbCentral.EjecutaSqlScalar(SQL)



                Me.BuscarTiposDeImpuesto()

                SQL = "DELETE TH_LIIG WHERE LIIG_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND LIIG_EMP_COD = '" & Me.mEmpCod & "'"
                Me.Cursor = Cursors.WaitCursor
                Me.DbCentral.EjecutaSqlCommit(SQL)
                Me.Cursor = Cursors.Default

                SQL = "DELETE TH_LIDO WHERE LIDO_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND LIDO_EMP_COD = '" & Me.mEmpCod & "'"
                Me.Cursor = Cursors.WaitCursor
                Me.DbCentral.EjecutaSqlCommit(SQL)
                Me.Cursor = Cursors.Default

                If Me.CheckBoxDebug.Checked = True Then
                    Me.BuscarFacturasPeriodoTest(Format(Me.DateTimePickerDesde.Value, "dd-MM-yyyy"), Format(Me.DateTimePickerHasta.Value, "dd-MM-yyyy"))
                Else
                    Me.BuscarFacturasPeriodo(Format(Me.DateTimePickerDesde.Value, "dd-MM-yyyy"), Format(Me.DateTimePickerHasta.Value, "dd-MM-yyyy"))
                End If


                Me.ButtonImprimirLibro.Enabled = True

            Else
                MsgBox("Revise las Fechas ", MsgBoxStyle.Information, "Atención")
            End If


        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub BuscarTiposDeImpuesto()
        Try
            Dim Ind As Integer = 1
            SQL = " SELECT TIVA_PERC FROM TNHT_TIVA ORDER BY TIVA_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)
            While Me.DbLeeHotel.mDbLector.Read


                If Ind = 1 Then
                    Me.TextBoxIgic6.Text = Me.DbLeeHotel.mDbLector.Item("TIVA_PERC")
                    Me.TextBoxIgic6.Update()
                End If
                If Ind = 2 Then
                    Me.TextBoxIgic5.Text = Me.DbLeeHotel.mDbLector.Item("TIVA_PERC")
                    Me.TextBoxIgic5.Update()
                End If
                If Ind = 3 Then
                    Me.TextBoxIgic4.Text = Me.DbLeeHotel.mDbLector.Item("TIVA_PERC")
                    Me.TextBoxIgic4.Update()
                End If
                If Ind = 4 Then
                    Me.TextBoxIgic3.Text = Me.DbLeeHotel.mDbLector.Item("TIVA_PERC")
                    Me.TextBoxIgic3.Update()
                End If
                If Ind = 5 Then
                    Me.TextBoxIgic2.Text = Me.DbLeeHotel.mDbLector.Item("TIVA_PERC")
                    Me.TextBoxIgic2.Update()
                End If
                If Ind = 6 Then
                    Me.TextBoxIgic1.Text = Me.DbLeeHotel.mDbLector.Item("TIVA_PERC")
                    Me.TextBoxIgic1.Update()
                End If

                Ind = Ind + 1


            End While

            Me.DbLeeHotel.mDbLector.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub BuscarFacturasPeriodo(ByVal vF1 As Date, ByVal vF2 As Date)
        Try
            Dim TotalFactura As Double
            Dim Dni As String
            Dim Cuenta As String
            Dim Titular As String
            Dim Linea As String
            Dim ClientesContadoCif As String


            Dim PrimeraFactura As Boolean = True
            Dim Controlfactura As String
            Dim Ind As Integer

            Dim Numero As String


            ' creaFichero convertido 

            '     Me.mvFile = Me.mvFilePath & "IGIC " & Format(vF1, ("ddMMyy")) & " " & Format(vF2, ("ddMMyy")) & ".DAT"
            Me.mvFile = Me.mvFilePath & Me.mvFilePrefijo & Format(vF2, ("ddMMyy")) & ".DAT"


            Me.CrearFichero(Me.mvFile)
            If Me.FileEstaOk = False Then Exit Sub


            Me.TextBoxDebug.Text = Me.mvFile



            ClientesContadoCif = Me.DbCentral.EjecutaSqlScalar("SELECT NVL(PARA_CLIENTES_CONTADO_CIF,'0') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'")

            '  If Me.RadioButtonCuentaCliente.Checked Then
            'ClientesContadoCuenta = Me.DbCentral.EjecutaSqlScalar("SELECT NVL(PARA_CLIENTES_CONTADO,'0') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'")
            'Else
            'ClientesContadoCuenta = Me.DbCentral.EjecutaSqlScalar("SELECT NVL(PARA_CLIENTES_CONTADO_CODIGO,'0') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'")
            'End If



            SQL = "SELECT  TNHT_FACT.FACT_STAT AS ESTADO, TNHT_FACT.FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,TNHT_FACT.FACT_TOTA TOTAL,TNHT_FACT.FACT_VALO VALOR,ENTI_CODI,CCEX_CODI,NVL(CLIE_CODI,'0') AS CLIENTE "
            SQL += " , NVL(TNHT_FACT.FACT_TITU,'') TITULAR, "
            SQL += " ROUND((FAIV_INCI - FAIV_VIMP),2)  AS BASE , ROUND(FAIV_VIMP,2) AS IMPUESTO ,FAIV_TAXA AS TIPO "
            SQL += "FROM TNHT_FACT ,TNHT_FAIV "
            SQL += "WHERE "
            SQL += " TNHT_FACT.FACT_CODI = TNHT_FAIV.FACT_CODI  AND TNHT_FACT.SEFA_CODI = TNHT_FAIV.SEFA_CODI AND "
            SQL += "(TNHT_FACT.FACT_DAEM BETWEEN " & "'" & vF1 & "' AND '" & vF2 & "')"
            SQL += "ORDER BY TNHT_FACT.SEFA_CODI ASC, TNHT_FACT.FACT_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)
            Me.Cursor = Cursors.WaitCursor

            Me.NEWHOTEL = New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)


            While Me.DbLeeHotel.mDbLector.Read


                ' troquear numero y serie de factura por maximo de longitud de 2 bytes en datisa

                If Me.CheckBoxCambiaSerie.Checked = True Then

                    If Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) = "R" Then
                        Numero = Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 3, 2) & "" & (CType(DbLeeHotel.mDbLector("NUMERO"), String))
                    Else
                        Numero = Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 2, 2) & "" & (CType(DbLeeHotel.mDbLector("NUMERO"), String))
                    End If

                Else
                    Numero = (CType(DbLeeHotel.mDbLector("NUMERO"), String))

                End If



                If PrimeraFactura = True Then
                    PrimeraFactura = False
                    Controlfactura = CType(DbLeeHotel.mDbLector("NUMERO"), String)
                End If


                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)


                ' DETERMINAR EL TIPO DE FACTURA 
                ' FACTURA DE CONTADO nuevo
                If Me.RadioButtonCuentaCliente.Checked Then
                    Cuenta = Me.NEWHOTEL.DevuelveCuentaContabledeFacturaIgicSatocan(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), 0)
                Else
                    Cuenta = Me.NEWHOTEL.DevuelveCuentaContabledeFacturaIgicSatocan(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), 1)
                End If
                Dni = Me.NEWHOTEL.DevuelveDniCifContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))




                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)

                Linea = """" & Format(CType(DbLeeHotel.mDbLector("FACT_DAEM"), Date), "dd/MM/yyyy") & """" & Separador


                If Me.CheckBoxCambiaSerie.Checked = True Then
                    Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) & """" & Separador
                Else
                    Linea += """" & CType(DbLeeHotel.mDbLector("SERIE"), String) & """" & Separador
                End If




                Linea += Numero & Separador
                Linea += "0" & Separador
                Linea += """" & CType(Cuenta, String) & """" & Separador
                Linea += """" & Format(CType(DbLeeHotel.mDbLector("FACT_DAEM"), Date), "dd/MM/yyyy") & """" & Separador
                Linea += CType(DbLeeHotel.mDbLector("TIPO"), String) & Separador
                Linea += Numero & Separador
                Linea += "1" & Separador
                Linea += CType(TotalFactura, String) & Separador



                Linea += """" & "I" & """" & Separador
                Linea += CType(DbLeeHotel.mDbLector("TIPO"), String) & Separador
                Linea += """" & "G" & """" & Separador
                Linea += CType(DbLeeHotel.mDbLector("BASE"), String) & Separador
                Linea += CType(DbLeeHotel.mDbLector("IMPUESTO"), String) & Separador
                Linea += "100.0" & Separador


                For Ind = 1 To 5
                    Linea += """" & " " & """" & Separador
                    Linea += "0.0" & Separador
                    Linea += """" & "N" & """" & Separador
                    Linea += "0.00" & Separador
                    Linea += "0.00" & Separador
                    Linea += "0.00" & Separador


                Next Ind
                Linea += "0" & Separador
                Linea += "0" & Separador


                ' Indicadores de formas de cobro


                If CType(Me.DbLeeHotel.mDbLector("ESTADO"), String) = "1" Then
                    Linea += """" & Me.TextBoxFormaContado.Text & """" & Separador
                Else
                    Linea += """" & Me.TextBoxFormaCredito.Text & """" & Separador
                End If


                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += """" & "!2" & """" & Separador
                Linea += "1.000000" & Separador
                Linea += """" & "1" & """" & Separador
                Linea += "7" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador

                '      Linea += CType(Dni, String).PadLeft(25, " ") & "|"

                Me.ListBoxDebug.Items.Add(Linea)
                Me.Filegraba.WriteLine(Linea)


            End While
            Me.DbLeeHotel.mDbLector.Close()

            Me.NEWHOTEL.CerrarConexiones()


            Me.Filegraba.Close()
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas")
        End Try
    End Sub
    Private Sub BuscarFacturasPeriodoTest(ByVal vF1 As Date, ByVal vF2 As Date)
        Try
            Dim TotalFactura As Double
            Dim Dni As String
            Dim Cuenta As String
            Dim Titular As String
            Dim Linea As String
            Dim ClientesContadoCif As String


            Dim PrimeraFactura As Boolean = True
            Dim Controlfactura As String


            Dim Numero As String



            ' creaFichero convertido 

            '     Me.mvFile = Me.mvFilePath & "IGIC " & Format(vF1, ("ddMMyy")) & " " & Format(vF2, ("ddMMyy")) & ".DAT"
            Me.mvFile = Me.mvFilePath & Me.mvFilePrefijo & Format(vF2, ("ddMMyy")) & ".DAT"


            Me.CrearFichero(Me.mvFile)
            If Me.FileEstaOk = False Then Exit Sub


            Me.TextBoxDebug.Text = Me.mvFile



            ClientesContadoCif = Me.DbCentral.EjecutaSqlScalar("SELECT NVL(PARA_CLIENTES_CONTADO_CIF,'0') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'")



            SQL = "SELECT  TNHT_FACT.FACT_STAT AS ESTADO, TNHT_FACT.FACT_DAEM AS FACT_DAEM, TNHT_FACT.FACT_CODI AS NUMERO, NVL(TNHT_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNHT_FACT.FACT_CODI ||'/'|| TNHT_FACT.SEFA_CODI DESCRIPCION,TNHT_FACT.FACT_TOTA TOTAL,TNHT_FACT.FACT_VALO VALOR, ENTI_CODI,CCEX_CODI,NVL(CLIE_CODI,'0') AS CLIENTE "
            SQL += " , NVL(TNHT_FACT.FACT_TITU,'') TITULAR "
            SQL += "FROM TNHT_FACT  "
            SQL += "WHERE "
            SQL += "TNHT_FACT.FACT_DAEM BETWEEN " & "'" & vF1 & "' AND '" & vF2 & "' "
            SQL += "ORDER BY TNHT_FACT.SEFA_CODI ASC, TNHT_FACT.FACT_CODI ASC"

            Me.DbLeeHotel.TraerLector(SQL)
            Me.Cursor = Cursors.WaitCursor

            Me.NEWHOTEL = New NewHotel.NewHotelData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)

            While Me.DbLeeHotel.mDbLector.Read


                ' troquear numero y serie de factura por maximo de longitud de 2 bytes en datisa

                If Me.CheckBoxCambiaSerie.Checked = True Then

                    If Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) = "R" Then
                        Numero = Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 3, 2) & "" & (CType(DbLeeHotel.mDbLector("NUMERO"), String))
                    Else
                        Numero = Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 2, 2) & "" & (CType(DbLeeHotel.mDbLector("NUMERO"), String))
                    End If

                Else
                    Numero = (CType(DbLeeHotel.mDbLector("NUMERO"), String))

                End If



                If PrimeraFactura = True Then
                    PrimeraFactura = False
                    Controlfactura = CType(DbLeeHotel.mDbLector("NUMERO"), String)
                End If



                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)



                ' DETERMINAR EL TIPO DE FACTURA 
                ' FACTURA DE CONTADO nuevo
                If Me.RadioButtonCuentaCliente.Checked Then
                    Cuenta = Me.NEWHOTEL.DevuelveCuentaContabledeFacturaIgicSatocan(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), 0)
                Else
                    Cuenta = Me.NEWHOTEL.DevuelveCuentaContabledeFacturaIgicSatocan(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), 1)
                End If

                Dni = Me.NEWHOTEL.DevuelveDniCifContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))

                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)

                Linea = """" & Format(CType(DbLeeHotel.mDbLector("FACT_DAEM"), Date), "dd/MM/yyyy") & """" & Separador


                If Me.CheckBoxCambiaSerie.Checked = True Then
                    If Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) = "R" Then
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 2) & """" & Separador
                    Else
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) & """" & Separador
                    End If

                Else
                    Linea += """" & CType(DbLeeHotel.mDbLector("SERIE"), String) & """" & Separador
                End If





                Linea += Numero & Separador
                Linea += "0" & Separador
                Linea += """" & CType(Cuenta, String) & """" & Separador
                Linea += """" & Format(CType(DbLeeHotel.mDbLector("FACT_DAEM"), Date), "dd/MM/yyyy") & """" & Separador


                If Me.CheckBoxCambiaSerie.Checked = True Then
                    If Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) = "R" Then
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 2) & """" & Separador
                    Else
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) & """" & Separador
                    End If

                Else
                    Linea += """" & CType(DbLeeHotel.mDbLector("SERIE"), String) & """" & Separador
                End If

                'Linea += CType(DbLeeHotel.mDbLector("TIPO"), String) & Separador

                Linea += Numero & Separador
                Linea += "1" & Separador
                Linea += CType(TotalFactura, String) & Separador



                ' Grabar en la tabla de Documentos .
                Me.InsertaOracleLibroDocumentos(CType(DbLeeHotel.mDbLector("NUMERO"), Integer), CType(DbLeeHotel.mDbLector("SERIE"), String), 0, CType(DbLeeHotel.mDbLector("VALOR"), Double), CType(DbLeeHotel.mDbLector("TOTAL"), Double), CType(DbLeeHotel.mDbLector("TITULAR"), String), Cuenta, "Factura Newhotel", Format(DbLeeHotel.mDbLector("FACT_DAEM"), "dd/MM/yyyy"), 0, CType(DbLeeHotel.mDbLector("ESTADO"), String))



                ' BUCLE IGIC


                Dim C As Control

                For Each C In Me.GroupBoxImpuestos.Controls
                    If TypeOf C Is TextBox Then
                        If C.Tag = "IGIC" Then
                            SQL = "SELECT  FACT_CODI AS NUMERO, NVL(SEFA_CODI,'?')  SERIE, "
                            SQL += " ROUND((FAIV_INCI - FAIV_VIMP),2)  AS BASE , ROUND(FAIV_VIMP,2) AS IMPUESTO ,FAIV_TAXA AS TIPO "
                            SQL += "FROM TNHT_FAIV "
                            SQL += "WHERE "
                            SQL += " TNHT_FAIV.FACT_CODI = " & Me.DbLeeHotel.mDbLector.Item("NUMERO")
                            SQL += " AND TNHT_FAIV.SEFA_CODI = '" & Me.DbLeeHotel.mDbLector.Item("SERIE") & "'"
                            SQL += " AND TNHT_FAIV.FAIV_TAXA = " & C.Text

                            ' control cusrores

                            Me.MostrarCursores()
                            If CType(Me.TextBoxDebugCursor.Text, Integer) > 200 Then
                                Me.DbLeeHotelAux.CerrarConexion()
                                Me.DbLeeHotelAux.AbrirConexion()
                            End If

                            Me.DbLeeHotelAux.TraerLector(SQL)
                            Me.DbLeeHotelAux.mDbLector.Read()




                            If Me.DbLeeHotelAux.mDbLector.HasRows Then
                                Linea += """" & "I" & """" & Separador
                                Linea += CType(DbLeeHotelAux.mDbLector("TIPO"), String) & Separador
                                Linea += """" & "G" & """" & Separador
                                Linea += CType(DbLeeHotelAux.mDbLector("BASE"), String) & Separador
                                Linea += CType(DbLeeHotelAux.mDbLector("IMPUESTO"), String) & Separador
                                Linea += "100.0" & Separador

                                ' Grabar en la tabla de Igic aqui 
                                Me.InsertaOracleLibroIgic(CType(DbLeeHotel.mDbLector("NUMERO"), Integer), CType(DbLeeHotel.mDbLector("SERIE"), String), 0, CType(DbLeeHotelAux.mDbLector("BASE"), Double), CType(DbLeeHotelAux.mDbLector("IMPUESTO"), String), CType(DbLeeHotelAux.mDbLector("TIPO"), Double), CType(DbLeeHotel.mDbLector("TITULAR"), String), Cuenta, "Factura Newhotel", Format(DbLeeHotel.mDbLector("FACT_DAEM"), "dd/MM/yyyy"), 0, CType(DbLeeHotel.mDbLector("ESTADO"), String))
                            Else
                                Linea += """" & " " & """" & Separador
                                Linea += "0.0" & Separador
                                Linea += """" & "N" & """" & Separador
                                Linea += "0.00" & Separador
                                Linea += "0.00" & Separador
                                Linea += "0.00" & Separador
                            End If
                            Me.DbLeeHotelAux.mDbLector.Close()





                        End If
                    End If
                Next



                ' DOS CEROS 
                Linea += "0,0" & Separador




                ' Indicadores de formas de cobro


                If CType(Me.DbLeeHotel.mDbLector("ESTADO"), String) = "1" Then
                    Linea += """" & Me.TextBoxFormaContado.Text & """" & Separador
                Else
                    Linea += """" & Me.TextBoxFormaCredito.Text & """" & Separador
                End If



                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += """" & "!2" & """" & Separador
                Linea += "1.000000" & Separador
                Linea += """" & "1" & """" & Separador
                Linea += "7" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador

                '      Linea += CType(Dni, String).PadLeft(25, " ") & "|"

                Me.ListBoxDebug.Items.Add(Linea)
                Me.Filegraba.WriteLine(Linea)


            End While
            Me.DbLeeHotel.mDbLector.Close()

            Me.NEWHOTEL.CerrarConexiones()


            ' Facturas Newgolf 
            If Me.mConectaGolf = "1" Then
                Me.BuscarFacturasPeriodoNewGolf(Format(Me.DateTimePickerDesde.Value, "dd-MM-yyyy"), Format(Me.DateTimePickerHasta.Value, "dd-MM-yyyy"))
                Me.BuscarFacturasPeriodoNewGolfAnuladas(Format(Me.DateTimePickerDesde.Value, "dd-MM-yyyy"), Format(Me.DateTimePickerHasta.Value, "dd-MM-yyyy"))
                Me.BuscarNotasdeCreditoPeriodoNewGolf(Format(Me.DateTimePickerDesde.Value, "dd-MM-yyyy"), Format(Me.DateTimePickerHasta.Value, "dd-MM-yyyy"))
                Me.BuscarNotasdeCreditoPeriodoNewGolfAnuladas(Format(Me.DateTimePickerDesde.Value, "dd-MM-yyyy"), Format(Me.DateTimePickerHasta.Value, "dd-MM-yyyy"))


            End If

            Me.Filegraba.Close()
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas NewHotel")
        End Try
    End Sub
    Private Sub BuscarFacturasPeriodoNewGolf(ByVal vF1 As Date, ByVal vF2 As Date)
        Try
            Dim TotalFactura As Double
            Dim Dni As String
            Dim Cuenta As String = "0"
            Dim Titular As String
            Dim Linea As String
            Dim ClientesContadoCif As String


            Dim PrimeraFactura As Boolean = True
            Dim Controlfactura As String

            Dim Numero As String




            ClientesContadoCif = Me.DbCentral.EjecutaSqlScalar("SELECT NVL(PARA_CLIENTES_CONTADO_CIF,'0') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'")

            'If Me.RadioButtonCuentaCliente.Checked Then
            ' ClientesContadoCuenta = Me.DbCentral.EjecutaSqlScalar("SELECT NVL(PARA_CLIENTES_CONTADO,'0') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'")
            ' Else
            ' ClientesContadoCuenta = Me.DbCentral.EjecutaSqlScalar("SELECT NVL(PARA_CLIENTES_CONTADO_CODIGO,'0') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'")
            ' End If



            SQL = "SELECT  TNPL_FACT.FACT_STAT AS ESTADO, TNPL_FACT.FACT_DAEM AS FACT_DAEM, TNPL_FACT.FACT_CODI AS NUMERO, NVL(TNPL_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNPL_FACT.FACT_CODI ||'/'|| TNPL_FACT.SEFA_CODI DESCRIPCION,TNPL_FACT.FACT_IMPO TOTAL,TNPL_FACT.FACT_VALO VALOR,NVL(ENTI_CODI,'') AS ENTI_CODI"
            SQL += " ,NVL(AGEN_CODI,'0') AS CLIENTE "
            SQL += " , NVL(TNPL_FACT.FACT_TITU,'') TITULAR,NVL(TNPL_FACT.FACT_NUCO,'') NIF "
            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_FACT "
            SQL += "WHERE "
            SQL += "TNPL_FACT.FACT_DAEM BETWEEN " & "'" & vF1 & "' AND '" & vF2 & "' "
            SQL += "ORDER BY TNPL_FACT.SEFA_CODI ASC, TNPL_FACT.FACT_CODI ASC"



            Me.DbLeeHotel.TraerLector(SQL)
            Me.Cursor = Cursors.WaitCursor

            Me.NEWGOLF = New NewGolf.NewGolfData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)

            While Me.DbLeeHotel.mDbLector.Read


                ' troquear numero y serie de factura por maximo de longitud de 2 bytes en datisa

                If Me.CheckBoxCambiaSerie.Checked = True Then

                    If Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) = "R" Then
                        Numero = Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 3, 2) & "" & (CType(DbLeeHotel.mDbLector("NUMERO"), String))
                    Else
                        Numero = Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 2, 2) & "" & (CType(DbLeeHotel.mDbLector("NUMERO"), String))
                    End If

                Else
                    Numero = (CType(DbLeeHotel.mDbLector("NUMERO"), String))

                End If



                If PrimeraFactura = True Then
                    PrimeraFactura = False
                    Controlfactura = CType(DbLeeHotel.mDbLector("NUMERO"), String)
                End If



                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)



                ' DETERMINAR EL TIPO DE FACTURA 
                ' FACTURA DE CONTADO NUEVO

                If Me.RadioButtonCuentaCliente.Checked Then
                    Cuenta = Me.NEWGOLF.DevuelveCuentaContabledeFacturasSoloParaLibroIgicSatocan(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), 0)
                Else
                    Cuenta = Me.NEWGOLF.DevuelveCuentaContabledeFacturasSoloParaLibroIgicSatocan(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), 1)
                End If


                Dni = Me.NEWGOLF.DevuelveDniCifContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))

                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)

                Linea = """" & Format(CType(DbLeeHotel.mDbLector("FACT_DAEM"), Date), "dd/MM/yyyy") & """" & Separador


                If Me.CheckBoxCambiaSerie.Checked = True Then
                    If Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) = "R" Then
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 2) & """" & Separador
                    Else
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) & """" & Separador
                    End If

                Else
                    Linea += """" & CType(DbLeeHotel.mDbLector("SERIE"), String) & """" & Separador
                End If




                Linea += Numero & Separador
                Linea += "0" & Separador
                Linea += """" & CType(Cuenta, String) & """" & Separador
                Linea += """" & Format(CType(DbLeeHotel.mDbLector("FACT_DAEM"), Date), "dd/MM/yyyy") & """" & Separador

                If Me.CheckBoxCambiaSerie.Checked = True Then
                    If Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) = "R" Then
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 2) & """" & Separador
                    Else
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) & """" & Separador
                    End If

                Else
                    Linea += """" & CType(DbLeeHotel.mDbLector("SERIE"), String) & """" & Separador
                End If

                'Linea += CType(DbLeeHotel.mDbLector("TIPO"), String) & Separador

                Linea += Numero & Separador
                Linea += "1" & Separador
                Linea += CType(TotalFactura, String) & Separador



                ' Grabar en la tabla de Documentos .
                Me.InsertaOracleLibroDocumentos(CType(DbLeeHotel.mDbLector("NUMERO"), Integer), CType(DbLeeHotel.mDbLector("SERIE"), String), 0, CType(DbLeeHotel.mDbLector("VALOR"), Double), CType(DbLeeHotel.mDbLector("TOTAL"), Double), CType(DbLeeHotel.mDbLector("TITULAR"), String), Cuenta, "Factura NewGolf", Format(DbLeeHotel.mDbLector("FACT_DAEM"), "dd/MM/yyyy"), 5, CType(DbLeeHotel.mDbLector("ESTADO"), String))


                ' BUCLE IGIC


                Dim C As Control

                For Each C In Me.GroupBoxImpuestos.Controls
                    If TypeOf C Is TextBox Then
                        If C.Tag = "IGIC" Then
                            SQL = "SELECT  FACT_CODI AS NUMERO, NVL(SEFA_CODI,'?')  SERIE, "
                            SQL += " ROUND((FAIV_INCI - FAIV_VIMP),2)  AS BASE , ROUND(FAIV_VIMP,2) AS IMPUESTO ,FAIV_TAXA AS TIPO "
                            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_FAIV "
                            SQL += "WHERE "
                            SQL += " TNPL_FAIV.FACT_CODI = " & Me.DbLeeHotel.mDbLector.Item("NUMERO")
                            SQL += " AND TNPL_FAIV.SEFA_CODI = '" & Me.DbLeeHotel.mDbLector.Item("SERIE") & "'"
                            SQL += " AND TNPL_FAIV.FAIV_TAXA = " & C.Text

                            ' control cusrores

                            Me.MostrarCursores()
                            If CType(Me.TextBoxDebugCursor.Text, Integer) > 200 Then
                                Me.DbLeeHotelAux.CerrarConexion()
                                Me.DbLeeHotelAux.AbrirConexion()
                            End If

                            Me.DbLeeHotelAux.TraerLector(SQL)
                            Me.DbLeeHotelAux.mDbLector.Read()




                            If Me.DbLeeHotelAux.mDbLector.HasRows Then
                                Linea += """" & "I" & """" & Separador
                                Linea += CType(DbLeeHotelAux.mDbLector("TIPO"), String) & Separador
                                Linea += """" & "G" & """" & Separador
                                Linea += CType(DbLeeHotelAux.mDbLector("BASE"), String) & Separador
                                Linea += CType(DbLeeHotelAux.mDbLector("IMPUESTO"), String) & Separador
                                Linea += "100.0" & Separador

                                ' Grabar en la tabla de Igic aqui 
                                Me.InsertaOracleLibroIgic(CType(DbLeeHotel.mDbLector("NUMERO"), Integer), CType(DbLeeHotel.mDbLector("SERIE"), String), 0, CType(DbLeeHotelAux.mDbLector("BASE"), Double), CType(DbLeeHotelAux.mDbLector("IMPUESTO"), String), CType(DbLeeHotelAux.mDbLector("TIPO"), Double), CType(DbLeeHotel.mDbLector("TITULAR"), String), Cuenta, "Factura NewGolf", Format(DbLeeHotel.mDbLector("FACT_DAEM"), "dd/MM/yyyy"), 5, CType(DbLeeHotel.mDbLector("ESTADO"), String))
                            Else
                                Linea += """" & " " & """" & Separador
                                Linea += "0.0" & Separador
                                Linea += """" & "N" & """" & Separador
                                Linea += "0.00" & Separador
                                Linea += "0.00" & Separador
                                Linea += "0.00" & Separador
                            End If
                            Me.DbLeeHotelAux.mDbLector.Close()



                        End If
                    End If
                Next

                ' DOS CEROS 
                Linea += "0,0" & Separador

                ' Indicadores de formas de cobro


                If CType(Me.DbLeeHotel.mDbLector("ESTADO"), String) = "1" Then
                    Linea += """" & Me.TextBoxFormaContado.Text & """" & Separador
                Else
                    Linea += """" & Me.TextBoxFormaCredito.Text & """" & Separador
                End If



                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += """" & "!2" & """" & Separador
                Linea += "1.000000" & Separador
                Linea += """" & "1" & """" & Separador
                Linea += "7" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador

                '      Linea += CType(Dni, String).PadLeft(25, " ") & "|"

                Me.ListBoxDebug.Items.Add(Linea)
                Me.Filegraba.WriteLine(Linea)



            End While
            Me.DbLeeHotel.mDbLector.Close()
            Me.NEWGOLF.CerrarConexiones()



            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Total Facturas NewGolf")
        End Try
    End Sub
    Private Sub BuscarFacturasPeriodoNewGolfAnuladas(ByVal vF1 As Date, ByVal vF2 As Date)
        Try
            Dim TotalFactura As Double
            Dim Dni As String
            Dim Cuenta As String = "0"
            Dim Titular As String
            Dim Linea As String
            Dim ClientesContadoCif As String


            Dim PrimeraFactura As Boolean = True
            Dim Controlfactura As String


            Dim Numero As String




            ClientesContadoCif = Me.DbCentral.EjecutaSqlScalar("SELECT NVL(PARA_CLIENTES_CONTADO_CIF,'0') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'")

            'If Me.RadioButtonCuentaCliente.Checked Then
            ' ClientesContadoCuenta = Me.DbCentral.EjecutaSqlScalar("SELECT NVL(PARA_CLIENTES_CONTADO,'0') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'")
            ' Else
            ' ClientesContadoCuenta = Me.DbCentral.EjecutaSqlScalar("SELECT NVL(PARA_CLIENTES_CONTADO_CODIGO,'0') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'")
            ' End If



            SQL = "SELECT  TNPL_FACT.FACT_STAT AS ESTADO, TNPL_FACT.FACT_DAEM,TNPL_FACT.FACT_DAAN, TNPL_FACT.FACT_CODI AS NUMERO, NVL(TNPL_FACT.SEFA_CODI,'?')  SERIE, "
            SQL += "  TNPL_FACT.FACT_CODI ||'/'|| TNPL_FACT.SEFA_CODI DESCRIPCION,TNPL_FACT.FACT_IMPO TOTAL,TNPL_FACT.FACT_VALO VALOR,NVL(ENTI_CODI,'') AS ENTI_CODI"
            SQL += " ,NVL(AGEN_CODI,'0') AS CLIENTE "
            SQL += " , NVL(TNPL_FACT.FACT_TITU,'') TITULAR,NVL(TNPL_FACT.FACT_NUCO,'') NIF "
            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_FACT "
            SQL += "WHERE "
            SQL += "TNPL_FACT.FACT_DAAN BETWEEN " & "'" & vF1 & "' AND '" & vF2 & "' "
            SQL += "ORDER BY TNPL_FACT.SEFA_CODI ASC, TNPL_FACT.FACT_CODI ASC"



            Me.DbLeeHotel.TraerLector(SQL)
            Me.Cursor = Cursors.WaitCursor

            Me.NEWGOLF = New NewGolf.NewGolfData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)

            While Me.DbLeeHotel.mDbLector.Read


                ' troquear numero y serie de factura por maximo de longitud de 2 bytes en datisa

                If Me.CheckBoxCambiaSerie.Checked = True Then

                    If Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) = "R" Then
                        Numero = Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 3, 2) & "" & (CType(DbLeeHotel.mDbLector("NUMERO"), String))
                    Else
                        Numero = Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 2, 2) & "" & (CType(DbLeeHotel.mDbLector("NUMERO"), String))
                    End If

                Else
                    Numero = (CType(DbLeeHotel.mDbLector("NUMERO"), String))

                End If



                If PrimeraFactura = True Then
                    PrimeraFactura = False
                    Controlfactura = CType(DbLeeHotel.mDbLector("NUMERO"), String)
                End If



                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2) * -1



                ' DETERMINAR EL TIPO DE FACTURA 
                ' FACTURA DE CONTADO NUEVO

                If Me.RadioButtonCuentaCliente.Checked Then
                    Cuenta = Me.NEWGOLF.DevuelveCuentaContabledeFacturasSoloParaLibroIgicSatocan(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), 0)
                Else
                    Cuenta = Me.NEWGOLF.DevuelveCuentaContabledeFacturasSoloParaLibroIgicSatocan(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), 1)
                End If

                Dni = Me.NEWGOLF.DevuelveDniCifContabledeFactura(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))

                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)

                Linea = """" & Format(CType(DbLeeHotel.mDbLector("FACT_DAAN"), Date), "dd/MM/yyyy") & """" & Separador


                If Me.CheckBoxCambiaSerie.Checked = True Then
                    If Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) = "R" Then
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 2) & """" & Separador
                    Else
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) & """" & Separador
                    End If

                Else
                    Linea += """" & CType(DbLeeHotel.mDbLector("SERIE"), String) & """" & Separador
                End If




                Linea += Numero & Separador
                Linea += "0" & Separador
                Linea += """" & CType(Cuenta, String) & """" & Separador
                Linea += """" & Format(CType(DbLeeHotel.mDbLector("FACT_DAAN"), Date), "dd/MM/yyyy") & """" & Separador

                If Me.CheckBoxCambiaSerie.Checked = True Then
                    If Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) = "R" Then
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 2) & """" & Separador
                    Else
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) & """" & Separador
                    End If

                Else
                    Linea += """" & CType(DbLeeHotel.mDbLector("SERIE"), String) & """" & Separador
                End If

                'Linea += CType(DbLeeHotel.mDbLector("TIPO"), String) & Separador

                Linea += Numero & Separador
                Linea += "1" & Separador
                Linea += CType(TotalFactura, String) & Separador


                ' Grabar en la tabla de Documentos .
                Me.InsertaOracleLibroDocumentos(CType(DbLeeHotel.mDbLector("NUMERO"), Integer), CType(DbLeeHotel.mDbLector("SERIE"), String), 1, CType(DbLeeHotel.mDbLector("VALOR"), Double) * -1, CType(DbLeeHotel.mDbLector("TOTAL"), Double) * -1, CType(DbLeeHotel.mDbLector("TITULAR"), String), Cuenta, "Factura NewGolf Anulada", Format(DbLeeHotel.mDbLector("FACT_DAEM"), "dd/MM/yyyy"), 5, CType(DbLeeHotel.mDbLector("ESTADO"), String))


                ' BUCLE IGIC


                Dim C As Control

                For Each C In Me.GroupBoxImpuestos.Controls
                    If TypeOf C Is TextBox Then
                        If C.Tag = "IGIC" Then
                            SQL = "SELECT  FACT_CODI AS NUMERO, NVL(SEFA_CODI,'?')  SERIE, "
                            SQL += " ROUND((FAIV_INCI - FAIV_VIMP),2) * -1  AS BASE , ROUND(FAIV_VIMP,2) * -1 AS IMPUESTO ,FAIV_TAXA AS TIPO "
                            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".TNPL_FAIV "
                            SQL += "WHERE "
                            SQL += " TNPL_FAIV.FACT_CODI = " & Me.DbLeeHotel.mDbLector.Item("NUMERO")
                            SQL += " AND TNPL_FAIV.SEFA_CODI = '" & Me.DbLeeHotel.mDbLector.Item("SERIE") & "'"
                            SQL += " AND TNPL_FAIV.FAIV_TAXA = " & C.Text

                            ' control cusrores

                            Me.MostrarCursores()
                            If CType(Me.TextBoxDebugCursor.Text, Integer) > 200 Then
                                Me.DbLeeHotelAux.CerrarConexion()
                                Me.DbLeeHotelAux.AbrirConexion()
                            End If

                            Me.DbLeeHotelAux.TraerLector(SQL)
                            Me.DbLeeHotelAux.mDbLector.Read()




                            If Me.DbLeeHotelAux.mDbLector.HasRows Then
                                Linea += """" & "I" & """" & Separador
                                Linea += CType(DbLeeHotelAux.mDbLector("TIPO"), String) & Separador
                                Linea += """" & "G" & """" & Separador
                                Linea += CType(DbLeeHotelAux.mDbLector("BASE"), String) & Separador
                                Linea += CType(DbLeeHotelAux.mDbLector("IMPUESTO"), String) & Separador
                                Linea += "100.0" & Separador

                                ' Grabar en la tabla de Igic aqui 
                                Me.InsertaOracleLibroIgic(CType(DbLeeHotel.mDbLector("NUMERO"), Integer), CType(DbLeeHotel.mDbLector("SERIE"), String), 1, CType(DbLeeHotelAux.mDbLector("BASE"), Double), CType(DbLeeHotelAux.mDbLector("IMPUESTO"), String), CType(DbLeeHotelAux.mDbLector("TIPO"), Double), CType(DbLeeHotel.mDbLector("TITULAR"), String), Cuenta, "Factura NewGolf Anulada", Format(DbLeeHotel.mDbLector("FACT_DAAN"), "dd/MM/yyyy"), 5, CType(DbLeeHotel.mDbLector("ESTADO"), String))

                            Else
                                Linea += """" & " " & """" & Separador
                                Linea += "0.0" & Separador
                                Linea += """" & "N" & """" & Separador
                                Linea += "0.00" & Separador
                                Linea += "0.00" & Separador
                                Linea += "0.00" & Separador
                            End If
                            Me.DbLeeHotelAux.mDbLector.Close()



                        End If
                    End If
                Next

                ' DOS CEROS 
                Linea += "0,0" & Separador

                ' Indicadores de formas de cobro


                If CType(Me.DbLeeHotel.mDbLector("ESTADO"), String) = "1" Then
                    Linea += """" & Me.TextBoxFormaContado.Text & """" & Separador
                Else
                    Linea += """" & Me.TextBoxFormaCredito.Text & """" & Separador
                End If



                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += """" & "!2" & """" & Separador
                Linea += "1.000000" & Separador
                Linea += """" & "1" & """" & Separador
                Linea += "7" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador

                '      Linea += CType(Dni, String).PadLeft(25, " ") & "|"

                Me.ListBoxDebug.Items.Add(Linea)
                Me.Filegraba.WriteLine(Linea)



            End While
            Me.DbLeeHotel.mDbLector.Close()

            Me.NEWGOLF.CerrarConexiones()

            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Total NewGolf Anuladas")
        End Try
    End Sub

    Private Sub BuscarNotasdeCreditoPeriodoNewGolf(ByVal vF1 As Date, ByVal vF2 As Date)
        Try
            Dim TotalFactura As Double
            Dim Dni As String
            Dim Cuenta As String = "0"
            Dim Titular As String
            Dim Linea As String
            Dim ClientesContadoCif As String


            Dim PrimeraFactura As Boolean = True
            Dim Controlfactura As String


            Dim Numero As String

            Dim EstadoTemporal As String




            ClientesContadoCif = Me.DbCentral.EjecutaSqlScalar("SELECT NVL(PARA_CLIENTES_CONTADO_CIF,'0') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'")



            SQL = "SELECT  TNPL_NCRE.NCRE_NECO AS ESTADO, TNPL_NCRE.NCRE_DAEM, TNPL_NCRE.NCRE_CODI AS NUMERO, NVL(TNPL_NCRE.SENC_CODI,'?')  SERIE, "
            SQL += "  TNPL_NCRE.NCRE_CODI ||'/'|| TNPL_NCRE.SENC_CODI DESCRIPCION,TNPL_NCRE.NCRE_VALO * -1 VALOR,NVL(TNPL_FACT.ENTI_CODI,'') AS ENTI_CODI, "
            SQL += " NVL(TNPL_NCRE.NCRE_TITU,'?') TITULAR ,TNPL_FACT.FACT_CODI AS FACTURA "
            SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_NCRE, " & Me.mParaUsuarioNewGolf & ".TNPL_NCFA, " & Me.mParaUsuarioNewGolf & ".TNPL_FACT "
            SQL += "WHERE "

            SQL += "     TNPL_NCRE.SENC_CODI = TNPL_NCFA.SENC_CODI(+) "
            SQL += "AND  TNPL_NCRE.NCRE_CODI = TNPL_NCFA.NCRE_CODI(+) "
            SQL += "AND  TNPL_NCFA.FACT_CODI = TNPL_FACT.FACT_CODI(+)"
            SQL += "AND  TNPL_NCFA.SEFA_CODI =  TNPL_FACT.SEFA_CODI(+) "

            SQL += "AND TNPL_NCRE.NCRE_DAEM BETWEEN " & "'" & vF1 & "' AND '" & vF2 & "' "
            SQL += "ORDER BY TNPL_NCRE.SENC_CODI ASC, TNPL_NCRE.NCRE_CODI ASC"


            Me.DbLeeHotel.TraerLector(SQL)
            Me.Cursor = Cursors.WaitCursor

            Me.NEWGOLF = New NewGolf.NewGolfData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)

            While Me.DbLeeHotel.mDbLector.Read


                ' troquear numero y serie de factura por maximo de longitud de 2 bytes en datisa

                If Me.CheckBoxCambiaSerie.Checked = True Then

                    If Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) = "R" Then
                        Numero = Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 3, 2) & "" & (CType(DbLeeHotel.mDbLector("NUMERO"), String))
                    Else
                        Numero = Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 2, 2) & "" & (CType(DbLeeHotel.mDbLector("NUMERO"), String))
                    End If

                Else
                    Numero = (CType(DbLeeHotel.mDbLector("NUMERO"), String))

                End If



                If PrimeraFactura = True Then
                    PrimeraFactura = False
                    Controlfactura = CType(DbLeeHotel.mDbLector("NUMERO"), String)
                End If



                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)



                ' DETERMINAR EL TIPO DE FACTURA 
                ' NOTA DE CREDIT0 DE CONTADO 
                If Me.RadioButtonCuentaCliente.Checked Then
                    Cuenta = Me.NEWGOLF.DevuelveCuentaContabledeNotaCreditoSoloParaLibroIgicSatocan(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), 0)
                Else
                    Cuenta = Me.NEWGOLF.DevuelveCuentaContabledeNotaCreditoSoloParaLibroIgicSatocan(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), 1)
                End If

                Dni = Me.NEWGOLF.DevuelveDniCifContabledeNotaCredito(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))




                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)

                Linea = """" & Format(CType(DbLeeHotel.mDbLector("NCRE_DAEM"), Date), "dd/MM/yyyy") & """" & Separador


                If Me.CheckBoxCambiaSerie.Checked = True Then
                    If Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) = "R" Then
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 2) & """" & Separador
                    Else
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) & """" & Separador
                    End If

                Else
                    Linea += """" & CType(DbLeeHotel.mDbLector("SERIE"), String) & """" & Separador
                End If




                Linea += Numero & Separador
                Linea += "0" & Separador
                Linea += """" & CType(Cuenta, String) & """" & Separador
                Linea += """" & Format(CType(DbLeeHotel.mDbLector("NCRE_DAEM"), Date), "dd/MM/yyyy") & """" & Separador

                If Me.CheckBoxCambiaSerie.Checked = True Then
                    If Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) = "R" Then
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 2) & """" & Separador
                    Else
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) & """" & Separador
                    End If

                Else
                    Linea += """" & CType(DbLeeHotel.mDbLector("SERIE"), String) & """" & Separador
                End If

                'Linea += CType(DbLeeHotel.mDbLector("TIPO"), String) & Separador

                Linea += Numero & Separador
                Linea += "1" & Separador
                Linea += CType(TotalFactura, String) & Separador

                ' Grabar en la tabla de Documentos .
                If IsDBNull(DbLeeHotel.mDbLector("ESTADO")) = True Then
                    EstadoTemporal = " "
                Else
                    EstadoTemporal = CType(DbLeeHotel.mDbLector("ESTADO"), String)
                End If

                Me.InsertaOracleLibroDocumentos(CType(DbLeeHotel.mDbLector("NUMERO"), Integer), CType(DbLeeHotel.mDbLector("SERIE"), String), 0, CType(DbLeeHotel.mDbLector("VALOR"), Double), CType(DbLeeHotel.mDbLector("VALOR"), Double), CType(DbLeeHotel.mDbLector("TITULAR"), String), Cuenta, "Nota de Crédito NewGolf", Format(DbLeeHotel.mDbLector("NCRE_DAEM"), "dd/MM/yyyy"), 5, EstadoTemporal)



                ' BUCLE IGIC


                Dim C As Control

                For Each C In Me.GroupBoxImpuestos.Controls
                    If TypeOf C Is TextBox Then
                        If C.Tag = "IGIC" Then
                            SQL = "SELECT  NCRE_CODI AS NUMERO, NVL(SENC_CODI,'?')  SERIE, "
                            SQL += " SUM(ROUND(VLIQ,2))  AS BASE , SUM(ROUND(VIMP,2)) AS IMPUESTO ,TIVA_PERC AS TIPO "
                            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".VNPL_NCIV ," & Me.mParaUsuarioNewGolf & ".TNPL_TIVA "
                            SQL += "WHERE "
                            SQL += " VNPL_NCIV.TIVA = TNPL_TIVA.TIVA_CODI AND "
                            SQL += " VNPL_NCIV.NCRE_CODI = " & Me.DbLeeHotel.mDbLector.Item("NUMERO")
                            SQL += " AND VNPL_NCIV.SENC_CODI = '" & Me.DbLeeHotel.mDbLector.Item("SERIE") & "'"
                            SQL += " AND TNPL_TIVA.TIVA_PERC = " & C.Text
                            SQL += " GROUP BY NCRE_CODI,SENC_CODI,TIVA_PERC"

                            ' control cusrores

                            Me.MostrarCursores()
                            If CType(Me.TextBoxDebugCursor.Text, Integer) > 200 Then
                                Me.DbLeeHotelAux.CerrarConexion()
                                Me.DbLeeHotelAux.AbrirConexion()
                            End If

                            Me.DbLeeHotelAux.TraerLector(SQL)
                            Me.DbLeeHotelAux.mDbLector.Read()




                            If Me.DbLeeHotelAux.mDbLector.HasRows Then
                                Linea += """" & "I" & """" & Separador
                                Linea += CType(DbLeeHotelAux.mDbLector("TIPO"), String) & Separador
                                Linea += """" & "G" & """" & Separador
                                Linea += CType(DbLeeHotelAux.mDbLector("BASE"), String) & Separador
                                Linea += CType(DbLeeHotelAux.mDbLector("IMPUESTO"), String) & Separador
                                Linea += "100.0" & Separador

                                ' Grabar en la tabla de Igic aqui 

                                ' truco hasta que la tabla d notas de credito tenga el campo fact_stat o ncre_stat

                                If IsDBNull(DbLeeHotel.mDbLector("ESTADO")) = True Then
                                    EstadoTemporal = " "
                                Else
                                    EstadoTemporal = CType(DbLeeHotel.mDbLector("ESTADO"), String)
                                End If
                                Me.InsertaOracleLibroIgic(CType(DbLeeHotel.mDbLector("NUMERO"), Integer), CType(DbLeeHotel.mDbLector("SERIE"), String), 0, CType(DbLeeHotelAux.mDbLector("BASE"), Double), CType(DbLeeHotelAux.mDbLector("IMPUESTO"), String), CType(DbLeeHotelAux.mDbLector("TIPO"), Double), CType(DbLeeHotel.mDbLector("TITULAR"), String), Cuenta, "Nota de Crédito NewGolf", Format(DbLeeHotel.mDbLector("NCRE_DAEM"), "dd/MM/yyyy"), 5, EstadoTemporal)

                            Else
                                Linea += """" & " " & """" & Separador
                                Linea += "0.0" & Separador
                                Linea += """" & "N" & """" & Separador
                                Linea += "0.00" & Separador
                                Linea += "0.00" & Separador
                                Linea += "0.00" & Separador
                            End If
                            Me.DbLeeHotelAux.mDbLector.Close()



                        End If
                    End If
                Next

                ' DOS CEROS 
                Linea += "0,0" & Separador

                ' Indicadores de formas de cobro


                If IsDBNull(Me.DbLeeHotel.mDbLector("ESTADO")) = True Then
                    Linea += """" & Me.TextBoxFormaContado.Text & """" & Separador
                Else
                    Linea += """" & Me.TextBoxFormaCredito.Text & """" & Separador
                End If



                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += """" & "!2" & """" & Separador
                Linea += "1.000000" & Separador
                Linea += """" & "1" & """" & Separador
                Linea += "7" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador

                '      Linea += CType(Dni, String).PadLeft(25, " ") & "|"

                Me.ListBoxDebug.Items.Add(Linea)
                Me.Filegraba.WriteLine(Linea)



            End While
            Me.DbLeeHotel.mDbLector.Close()

            Me.NEWGOLF.CerrarConexiones()

            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Notas de Crédito NewGolf")
        End Try
    End Sub
    Private Sub BuscarNotasdeCreditoPeriodoNewGolfAnuladas(ByVal vF1 As Date, ByVal vF2 As Date)
        Try
            Dim TotalFactura As Double
            Dim Dni As String
            Dim Cuenta As String = "0"
            Dim Titular As String
            Dim Linea As String
            Dim ClientesContadoCif As String

            Dim PrimeraFactura As Boolean = True
            Dim Controlfactura As String


            Dim Numero As String

            Dim EstadoTemporal As String




            ClientesContadoCif = Me.DbCentral.EjecutaSqlScalar("SELECT NVL(PARA_CLIENTES_CONTADO_CIF,'0') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "' AND PARA_EMP_COD = '" & Me.mEmpCod & "'")



            SQL = "SELECT  TNPL_NCRE.NCRE_NECO AS ESTADO, TNPL_NCRE.NCRE_DAAN, TNPL_NCRE.NCRE_CODI AS NUMERO, NVL(TNPL_NCRE.SENC_CODI,'?')  SERIE, "
            SQL += "  TNPL_NCRE.NCRE_CODI ||'/'|| TNPL_NCRE.SENC_CODI DESCRIPCION,TNPL_NCRE.NCRE_VALO  VALOR,NVL(TNPL_FACT.ENTI_CODI,'') AS ENTI_CODI, "
            SQL += " NVL(TNPL_NCRE.NCRE_TITU,'?') TITULAR ,TNPL_FACT.FACT_CODI AS FACTURA "
            SQL += " FROM " & Me.mParaUsuarioNewGolf & ".TNPL_NCRE, " & Me.mParaUsuarioNewGolf & ".TNPL_NCFA, " & Me.mParaUsuarioNewGolf & ".TNPL_FACT "
            SQL += "WHERE "

            SQL += "     TNPL_NCRE.SENC_CODI = TNPL_NCFA.SENC_CODI(+) "
            SQL += "AND  TNPL_NCRE.NCRE_CODI = TNPL_NCFA.NCRE_CODI(+) "
            SQL += "AND  TNPL_NCFA.FACT_CODI = TNPL_FACT.FACT_CODI(+)"
            SQL += "AND  TNPL_NCFA.SEFA_CODI =  TNPL_FACT.SEFA_CODI(+) "

            SQL += "AND TNPL_NCRE.NCRE_DAAN BETWEEN " & "'" & vF1 & "' AND '" & vF2 & "' "
            SQL += "ORDER BY TNPL_NCRE.SENC_CODI ASC, TNPL_NCRE.NCRE_CODI ASC"


            Me.DbLeeHotel.TraerLector(SQL)
            Me.Cursor = Cursors.WaitCursor

            Me.NEWGOLF = New NewGolf.NewGolfData(Me.mStrConexionHotel, Me.mStrConexionCentral, Me.mEmpGrupoCod, Me.mEmpCod)
            While Me.DbLeeHotel.mDbLector.Read


                ' troquear numero y serie de factura por maximo de longitud de 2 bytes en datisa

                If Me.CheckBoxCambiaSerie.Checked = True Then

                    If Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) = "R" Then
                        Numero = Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 3, 2) & "" & (CType(DbLeeHotel.mDbLector("NUMERO"), String))
                    Else
                        Numero = Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 2, 2) & "" & (CType(DbLeeHotel.mDbLector("NUMERO"), String))
                    End If

                Else
                    Numero = (CType(DbLeeHotel.mDbLector("NUMERO"), String))

                End If



                If PrimeraFactura = True Then
                    PrimeraFactura = False
                    Controlfactura = CType(DbLeeHotel.mDbLector("NUMERO"), String)
                End If



                TotalFactura = Decimal.Round(CType(Me.DbLeeHotel.mDbLector("VALOR"), Decimal), 2)



                ' DETERMINAR EL TIPO DE FACTURA 
                ' NOTA DE CREDIT0 DE CONTADO 

                If Me.RadioButtonCuentaCliente.Checked Then
                    Cuenta = Me.NEWGOLF.DevuelveCuentaContabledeNotaCreditoSoloParaLibroIgicSatocan(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), 0)
                Else
                    Cuenta = Me.NEWGOLF.DevuelveCuentaContabledeNotaCreditoSoloParaLibroIgicSatocan(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String), 1)
                End If

                Dni = Me.NEWGOLF.DevuelveDniCifContabledeNotaCredito(CType(Me.DbLeeHotel.mDbLector("NUMERO"), Integer), CType(Me.DbLeeHotel.mDbLector("SERIE"), String))

                Titular = CType(Me.DbLeeHotel.mDbLector("TITULAR"), String)

                Linea = """" & Format(CType(DbLeeHotel.mDbLector("NCRE_DAAN"), Date), "dd/MM/yyyy") & """" & Separador


                If Me.CheckBoxCambiaSerie.Checked = True Then
                    If Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) = "R" Then
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 2) & """" & Separador
                    Else
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) & """" & Separador
                    End If

                Else
                    Linea += """" & CType(DbLeeHotel.mDbLector("SERIE"), String) & """" & Separador
                End If




                Linea += Numero & Separador
                Linea += "0" & Separador
                Linea += """" & CType(Cuenta, String) & """" & Separador
                Linea += """" & Format(CType(DbLeeHotel.mDbLector("NCRE_DAAN"), Date), "dd/MM/yyyy") & """" & Separador

                If Me.CheckBoxCambiaSerie.Checked = True Then
                    If Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) = "R" Then
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 2) & """" & Separador
                    Else
                        Linea += """" & Mid(CType(DbLeeHotel.mDbLector("SERIE"), String), 1, 1) & """" & Separador
                    End If

                Else
                    Linea += """" & CType(DbLeeHotel.mDbLector("SERIE"), String) & """" & Separador
                End If
                'Linea += CType(DbLeeHotel.mDbLector("TIPO"), String) & Separador

                Linea += Numero & Separador
                Linea += "1" & Separador
                Linea += CType(TotalFactura, String) & Separador


                ' Grabar en la tabla de Documentos .
                If IsDBNull(DbLeeHotel.mDbLector("ESTADO")) = True Then
                    EstadoTemporal = " "
                Else
                    EstadoTemporal = CType(DbLeeHotel.mDbLector("ESTADO"), String)
                End If

                Me.InsertaOracleLibroDocumentos(CType(DbLeeHotel.mDbLector("NUMERO"), Integer), CType(DbLeeHotel.mDbLector("SERIE"), String), 1, CType(DbLeeHotel.mDbLector("VALOR"), Double), CType(DbLeeHotel.mDbLector("VALOR"), Double), CType(DbLeeHotel.mDbLector("TITULAR"), String), Cuenta, "Nota de Crédito NewGolf Anulada", Format(DbLeeHotel.mDbLector("NCRE_DAAN"), "dd/MM/yyyy"), 5, EstadoTemporal)

                ' BUCLE IGIC


                Dim C As Control

                For Each C In Me.GroupBoxImpuestos.Controls
                    If TypeOf C Is TextBox Then
                        If C.Tag = "IGIC" Then
                            SQL = "SELECT  NCRE_CODI AS NUMERO, NVL(SENC_CODI,'?')  SERIE, "
                            SQL += " SUM(ROUND(VLIQ,2)) * -1  AS BASE , SUM(ROUND(VIMP,2)) * -1 AS IMPUESTO ,TIVA_PERC AS TIPO "
                            SQL += "FROM " & Me.mParaUsuarioNewGolf & ".VNPL_NCIV ," & Me.mParaUsuarioNewGolf & ".TNPL_TIVA "
                            SQL += "WHERE "
                            SQL += " VNPL_NCIV.TIVA = TNPL_TIVA.TIVA_CODI AND "
                            SQL += " VNPL_NCIV.NCRE_CODI = " & Me.DbLeeHotel.mDbLector.Item("NUMERO")
                            SQL += " AND VNPL_NCIV.SENC_CODI = '" & Me.DbLeeHotel.mDbLector.Item("SERIE") & "'"
                            SQL += " AND TNPL_TIVA.TIVA_PERC = " & C.Text
                            SQL += " GROUP BY NCRE_CODI,SENC_CODI,TIVA_PERC"


                            ' control cusrores

                            Me.MostrarCursores()
                            If CType(Me.TextBoxDebugCursor.Text, Integer) > 200 Then
                                Me.DbLeeHotelAux.CerrarConexion()
                                Me.DbLeeHotelAux.AbrirConexion()
                            End If

                            Me.DbLeeHotelAux.TraerLector(SQL)
                            Me.DbLeeHotelAux.mDbLector.Read()




                            If Me.DbLeeHotelAux.mDbLector.HasRows Then
                                Linea += """" & "I" & """" & Separador
                                Linea += CType(DbLeeHotelAux.mDbLector("TIPO"), String) & Separador
                                Linea += """" & "G" & """" & Separador
                                Linea += CType(DbLeeHotelAux.mDbLector("BASE"), String) & Separador
                                Linea += CType(DbLeeHotelAux.mDbLector("IMPUESTO"), String) & Separador
                                Linea += "100.0" & Separador

                                ' Grabar en la tabla de Igic aqui 

                                ' Grabar en la tabla de Igic aqui 

                                ' truco hasta que la tabla d notas de credito tenga el campo fact_stat o ncre_stat

                                If IsDBNull(DbLeeHotel.mDbLector("ESTADO")) = True Then
                                    EstadoTemporal = " "
                                Else
                                    EstadoTemporal = CType(DbLeeHotel.mDbLector("ESTADO"), String)
                                End If
                                Me.InsertaOracleLibroIgic(CType(DbLeeHotel.mDbLector("NUMERO"), Integer), CType(DbLeeHotel.mDbLector("SERIE"), String), 1, CType(DbLeeHotelAux.mDbLector("BASE"), Double), CType(DbLeeHotelAux.mDbLector("IMPUESTO"), String), CType(DbLeeHotelAux.mDbLector("TIPO"), Double), CType(DbLeeHotel.mDbLector("TITULAR"), String), Cuenta, "Nota de Crédito NewGolf Anulada", Format(DbLeeHotel.mDbLector("NCRE_DAAN"), "dd/MM/yyyy"), 5, EstadoTemporal)

                            Else
                                Linea += """" & " " & """" & Separador
                                Linea += "0.0" & Separador
                                Linea += """" & "N" & """" & Separador
                                Linea += "0.00" & Separador
                                Linea += "0.00" & Separador
                                Linea += "0.00" & Separador
                            End If
                            Me.DbLeeHotelAux.mDbLector.Close()



                        End If
                    End If
                Next


                ' DOS CEROS 
                Linea += "0,0" & Separador


                ' Indicadores de formas de cobro


                If IsDBNull(Me.DbLeeHotel.mDbLector("ESTADO")) = True Then
                    Linea += """" & Me.TextBoxFormaContado.Text & """" & Separador
                Else
                    Linea += """" & Me.TextBoxFormaCredito.Text & """" & Separador
                End If


                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += """" & "!2" & """" & Separador
                Linea += "1.000000" & Separador
                Linea += """" & "1" & """" & Separador
                Linea += "7" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador
                Linea += "0" & Separador

                '      Linea += CType(Dni, String).PadLeft(25, " ") & "|"

                Me.ListBoxDebug.Items.Add(Linea)
                Me.Filegraba.WriteLine(Linea)



            End While
            Me.DbLeeHotel.mDbLector.Close()
            Me.NEWGOLF.CerrarConexiones()

            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Total Notas de Crédito NewGolf Anuladas")
        End Try
    End Sub
    Private Sub CrearFichero(ByVal vFile As String)

        Try
            FileEstaOk = False
            ' Filegraba = New StreamWriter(vFile, False, System.Text.Encoding.UTF8)
            Filegraba = New StreamWriter(vFile, False, System.Text.Encoding.ASCII)


            FileEstaOk = True
        Catch ex As Exception
            FileEstaOk = False
            MsgBox("No dispone de acceso al Fichero " & vFile, MsgBoxStyle.Information, "Atención")
        End Try
    End Sub

    Private Sub FormLibroImpuestos_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Try
            If DbLeeHotel.EstadoConexion = ConnectionState.Open Then
                DbLeeHotel.CerrarConexion()
            End If
            If DbLeeHotelAux.EstadoConexion = ConnectionState.Open Then
                DbLeeHotelAux.CerrarConexion()
            End If
            If DbCentral.EstadoConexion = ConnectionState.Open Then
                DbCentral.CerrarConexion()
            End If
        Catch EX As Exception

        End Try

    End Sub

    Private Sub FormLibroImpuestos_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If IsNothing(Me.DbCentral) = False Then
                If Me.DbCentral.EstadoConexion = ConnectionState.Open Then
                    Me.DbCentral.CerrarConexion()
                End If

            End If
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

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub FormLibroImpuestos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            Me.Text = Me.Text & " " & Me.mNombreHotel
            Me.DateTimePickerDesde.Value = CType(Format(Now, "dd/MM/yyyy"), Date)
            Me.DateTimePickerHasta.Value = CType(Format(Now, "dd/MM/yyyy"), Date)

            If Me.mEmpGrupoCod = "SATO" And Me.mEmpCod = "3" Then
                Me.RadioButtonCodigoCliente.Checked = True
            ElseIf Me.mEmpGrupoCod = "SATO" And Me.mEmpCod = "2" Then
                Me.RadioButtonCodigoCliente.Checked = True
            Else
                Me.RadioButtonCuentaCliente.Checked = True
            End If


            If Me.mEmpGrupoCod = "SATO" And Me.mEmpCod = "2" Then
                Me.TextBoxFormaContado.Text = "0001"
                Me.TextBoxFormaCredito.Text = "3000"
            End If


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub MostrarCursores()
        Try
            Dim SQL As String
            SQL = "SELECT NVL(COUNT(*),'0') AS TOTAL FROM V$OPEN_CURSOR WHERE USER_NAME = '" & StrConexionExtraeUsuario(Me.mStrConexionHotel) & "'"
            Me.TextBoxDebugCursor.Text = Me.DbCentral.EjecutaSqlScalar(SQL)
            Me.TextBoxDebugCursor.Update()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub CheckBoxModificaFormasdeCobro_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxModificaFormasdeCobro.CheckedChanged
        Try
            If Me.CheckBoxModificaFormasdeCobro.Checked Then
                Me.TextBoxFormaContado.Enabled = True
                Me.TextBoxFormaCredito.Enabled = True
            Else
                Me.TextBoxFormaContado.Enabled = False
                Me.TextBoxFormaCredito.Enabled = False

            End If
        Catch ex As Exception

        End Try
    End Sub



    Private Sub TextBoxFormaContado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBoxFormaContado.Validating
        Try
            If Me.TextBoxFormaContado.Text.Length = 0 Then
                Me.TextBoxFormaContado.Text = "0"
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub TextBoxFormaCredito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBoxFormaCredito.Validating
        Try
            If Me.TextBoxFormaCredito.Text.Length = 0 Then
                Me.TextBoxFormaCredito.Text = "0"
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub InsertaOracleLibroIgic(ByVal vNFactura As Integer, ByVal vSerie As String, ByVal vAnulada As Integer, ByVal vBase As Double, ByVal vVimp As Double, ByVal vTiva As Double, ByVal vTitu As String, ByVal vCuenta As String, ByVal vTipo As String, ByVal vFecha As String, ByVal vOrig As Integer, ByVal vStat As String)
        Try
            SQL = "INSERT INTO TH_LIIG (LIIG_EMPGRUPO_COD, LIIG_EMP_COD, FACT_CODI,SEFA_CODI, LIIG_ANUL, LIIG_BASE,LIIG_VIMP, LIIG_TIVA, LIIG_TITU,LIIG_CTB1, LIIG_TIPO,LIIG_DATE,LIIG_ORIG,LIIG_STAT) "
            SQL += " VALUES ('" & Me.mEmpGrupoCod & "','" & Me.mEmpCod & "',"
            SQL += vNFactura & ",'"
            SQL += vSerie & "',"
            SQL += vAnulada & ","
            SQL += vBase & ","
            SQL += vVimp & ","
            SQL += vTiva & ",'"
            SQL += vTitu.Replace("'", "''") & "','"
            SQL += vCuenta & "','"
            SQL += vTipo & "','"
            SQL += vFecha & "',"
            SQL += vOrig & ",'"
            SQL += vStat & "')"

            Me.DbCentral.EjecutaSqlCommit(SQL)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub InsertaOracleLibroDocumentos(ByVal vNFactura As Integer, ByVal vSerie As String, ByVal vAnulada As Integer, ByVal vValo As Double, ByVal vTota As Double, ByVal vTitu As String, ByVal vCuenta As String, ByVal vTipo As String, ByVal vFecha As String, ByVal vOrig As Integer, ByVal vStat As String)
        Try
            SQL = "INSERT INTO TH_LIDO (LIDO_EMPGRUPO_COD, LIDO_EMP_COD, FACT_CODI,SEFA_CODI, LIDO_ANUL, LIDO_VALO,LIDO_TOTA,LIDO_TITU,LIDO_CTB1, LIDO_TIPO,LIDO_DATE,LIDO_ORIG,LIDO_STAT) "
            SQL += " VALUES ('" & Me.mEmpGrupoCod & "','" & Me.mEmpCod & "',"
            SQL += vNFactura & ",'"
            SQL += vSerie & "',"
            SQL += vAnulada & ","
            SQL += vValo & ","
            SQL += vTota & ",'"
            SQL += vTitu.Replace("'", "''") & "','"
            SQL += vCuenta & "','"
            SQL += vTipo & "','"
            SQL += vFecha & "',"
            SQL += vOrig & ",'"
            SQL += vStat & "')"

            Me.DbCentral.EjecutaSqlCommit(SQL)


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimirLibro.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            REPORT_SELECTION_FORMULA = "{TH_LIIG.LIIG_DATE}>=DATETIME(" & Format(Me.DateTimePickerDesde.Value, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += " AND {TH_LIIG.LIIG_DATE}<=DATETIME(" & Format(Me.DateTimePickerHasta.Value, REPORT_DATE_FORMAT) & ")"

            REPORT_SELECTION_FORMULA += " AND {TH_LIIG.LIIG_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TH_LIIG.LIIG_EMP_COD}= '" & Me.mEmpCod & "'"
            Dim Form As New FormVisorCrystal("Libro de Impuestos.RPT", Me.mNombreHotel, REPORT_SELECTION_FORMULA, Me.mStrConexionCentral, Me.mStrConexionHotel, False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub ButtonCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancelar.Click
        Try
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonImprimirFacturas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimirFacturas.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            REPORT_SELECTION_FORMULA = "{TH_LIDO.LIDO_DATE}>=DATETIME(" & Format(Me.DateTimePickerDesde.Value, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += " AND {TH_LIDO.LIDO_DATE}<=DATETIME(" & Format(Me.DateTimePickerHasta.Value, REPORT_DATE_FORMAT) & ")"

            REPORT_SELECTION_FORMULA += " AND {TH_LIDO.LIDO_EMPGRUPO_COD}= '" & Me.mEmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TH_LIDO.LIDO_EMP_COD}= '" & Me.mEmpCod & "'"
            Dim Form As New FormVisorCrystal("Libro de Documentos.RPT", Me.mNombreHotel & " " & Format(Me.DateTimePickerDesde.Value, REPORT_DATE_FORMAT) & " " & Format(Me.DateTimePickerHasta.Value, REPORT_DATE_FORMAT), REPORT_SELECTION_FORMULA, Me.mStrConexionCentral, Me.mStrConexionHotel, False, False)

            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub TextBoxIgic6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxIgic6.TextChanged

    End Sub
End Class
