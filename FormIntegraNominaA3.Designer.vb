<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormIntegraNominaA3
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
   
    'Public Sub New(ByVal vStrConexionCentral As String, ByVal vStrConexionSpyro As String)
    '   MyBase.New()

    'El Diseñador de Windows Forms requiere esta llamada.
    '        InitializeComponent()

    'Agregar cualquier inicialización después de la llamada a InitializeComponent()
    '       Me.m_StrConexionCentral = vStrConexionCentral
    '      Me.m_StrConexionSpyro = vStrConexionSpyro
    ' End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormIntegraNominaA3))
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.TextBoxHojaExcel = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.DataGridHoteles = New System.Windows.Forms.DataGrid
        Me.ButtonRutaExcel = New System.Windows.Forms.Button
        Me.TextBoxRutaLibroExcel = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.TextBoxDebug = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextBoxTotalProductores = New System.Windows.Forms.TextBox
        Me.TextBoxTotalBruto = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.DataGridAsientos = New System.Windows.Forms.DataGrid
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.ListBoxLibros = New System.Windows.Forms.ListBox
        Me.ButtonExcel = New System.Windows.Forms.Button
        Me.ButtonImprimir = New System.Windows.Forms.Button
        Me.TextBoxTotalDebe = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextBoxTotalHaber = New System.Windows.Forms.TextBox
        Me.TextBoxErrores = New System.Windows.Forms.TextBox
        Me.TextBoxCuadre = New System.Windows.Forms.TextBox
        Me.CheckBoxAjustar = New System.Windows.Forms.CheckBox
        Me.ButtonImprimirErrores = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TextBoxHojaNif = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        CType(Me.DataGridHoteles, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridAsientos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonAceptar.Location = New System.Drawing.Point(15, 12)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(75, 23)
        Me.ButtonAceptar.TabIndex = 1
        Me.ButtonAceptar.Text = "&Aceptar"
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ListBox1
        '
        Me.ListBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ListBox1.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.HorizontalExtent = 5000
        Me.ListBox1.HorizontalScrollbar = True
        Me.ListBox1.ItemHeight = 14
        Me.ListBox1.Location = New System.Drawing.Point(17, 296)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(361, 158)
        Me.ListBox1.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.TextBoxHojaNif)
        Me.GroupBox1.Controls.Add(Me.TextBoxHojaExcel)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.DateTimePicker1)
        Me.GroupBox1.Controls.Add(Me.DataGridHoteles)
        Me.GroupBox1.Controls.Add(Me.ButtonRutaExcel)
        Me.GroupBox1.Controls.Add(Me.TextBoxRutaLibroExcel)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(660, 195)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'TextBoxHojaExcel
        '
        Me.TextBoxHojaExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxHojaExcel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxHojaExcel.Location = New System.Drawing.Point(190, 34)
        Me.TextBoxHojaExcel.Name = "TextBoxHojaExcel"
        Me.TextBoxHojaExcel.Size = New System.Drawing.Size(195, 20)
        Me.TextBoxHojaExcel.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(126, 41)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(58, 13)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "Hoja Excel"
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.CustomFormat = "dd/MM/yyyy"
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker1.Location = New System.Drawing.Point(9, 12)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(111, 20)
        Me.DateTimePicker1.TabIndex = 0
        Me.DateTimePicker1.Value = New Date(2006, 4, 10, 0, 0, 0, 0)
        '
        'DataGridHoteles
        '
        Me.DataGridHoteles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridHoteles.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DataGridHoteles.DataMember = ""
        Me.DataGridHoteles.Font = New System.Drawing.Font("Arial", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridHoteles.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGridHoteles.Location = New System.Drawing.Point(9, 60)
        Me.DataGridHoteles.Name = "DataGridHoteles"
        Me.DataGridHoteles.ReadOnly = True
        Me.DataGridHoteles.Size = New System.Drawing.Size(645, 96)
        Me.DataGridHoteles.TabIndex = 15
        '
        'ButtonRutaExcel
        '
        Me.ButtonRutaExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonRutaExcel.Image = CType(resources.GetObject("ButtonRutaExcel.Image"), System.Drawing.Image)
        Me.ButtonRutaExcel.Location = New System.Drawing.Point(598, 9)
        Me.ButtonRutaExcel.Name = "ButtonRutaExcel"
        Me.ButtonRutaExcel.Size = New System.Drawing.Size(44, 23)
        Me.ButtonRutaExcel.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.ButtonRutaExcel, "Seleción de Ruta Libro de Excel")
        Me.ButtonRutaExcel.UseVisualStyleBackColor = True
        '
        'TextBoxRutaLibroExcel
        '
        Me.TextBoxRutaLibroExcel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxRutaLibroExcel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxRutaLibroExcel.Location = New System.Drawing.Point(191, 12)
        Me.TextBoxRutaLibroExcel.Name = "TextBoxRutaLibroExcel"
        Me.TextBoxRutaLibroExcel.Size = New System.Drawing.Size(401, 20)
        Me.TextBoxRutaLibroExcel.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(126, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Libro Excel"
        '
        'TextBoxDebug
        '
        Me.TextBoxDebug.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxDebug.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxDebug.Location = New System.Drawing.Point(384, 294)
        Me.TextBoxDebug.Multiline = True
        Me.TextBoxDebug.Name = "TextBoxDebug"
        Me.TextBoxDebug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxDebug.Size = New System.Drawing.Size(216, 160)
        Me.TextBoxDebug.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 272)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Productores"
        '
        'TextBoxTotalProductores
        '
        Me.TextBoxTotalProductores.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxTotalProductores.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxTotalProductores.Location = New System.Drawing.Point(84, 269)
        Me.TextBoxTotalProductores.Name = "TextBoxTotalProductores"
        Me.TextBoxTotalProductores.Size = New System.Drawing.Size(44, 20)
        Me.TextBoxTotalProductores.TabIndex = 5
        '
        'TextBoxTotalBruto
        '
        Me.TextBoxTotalBruto.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxTotalBruto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxTotalBruto.Location = New System.Drawing.Point(199, 270)
        Me.TextBoxTotalBruto.Name = "TextBoxTotalBruto"
        Me.TextBoxTotalBruto.Size = New System.Drawing.Size(71, 20)
        Me.TextBoxTotalBruto.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(134, 272)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(59, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Total Bruto"
        '
        'DataGridAsientos
        '
        Me.DataGridAsientos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridAsientos.DataMember = ""
        Me.DataGridAsientos.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridAsientos.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGridAsientos.Location = New System.Drawing.Point(17, 198)
        Me.DataGridAsientos.Name = "DataGridAsientos"
        Me.DataGridAsientos.ReadOnly = True
        Me.DataGridAsientos.Size = New System.Drawing.Size(647, 62)
        Me.DataGridAsientos.TabIndex = 16
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.ListBoxLibros)
        Me.GroupBox2.Controls.Add(Me.ButtonExcel)
        Me.GroupBox2.Controls.Add(Me.ButtonImprimir)
        Me.GroupBox2.Controls.Add(Me.ButtonAceptar)
        Me.GroupBox2.Location = New System.Drawing.Point(674, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(99, 454)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        '
        'ListBoxLibros
        '
        Me.ListBoxLibros.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxLibros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListBoxLibros.FormattingEnabled = True
        Me.ListBoxLibros.HorizontalExtent = 500
        Me.ListBoxLibros.HorizontalScrollbar = True
        Me.ListBoxLibros.Location = New System.Drawing.Point(12, 70)
        Me.ListBoxLibros.Name = "ListBoxLibros"
        Me.ListBoxLibros.Size = New System.Drawing.Size(78, 67)
        Me.ListBoxLibros.TabIndex = 24
        Me.ToolTip1.SetToolTip(Me.ListBoxLibros, "Selección de la Hoja de Excel ")
        '
        'ButtonExcel
        '
        Me.ButtonExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonExcel.Image = CType(resources.GetObject("ButtonExcel.Image"), System.Drawing.Image)
        Me.ButtonExcel.Location = New System.Drawing.Point(27, 41)
        Me.ButtonExcel.Name = "ButtonExcel"
        Me.ButtonExcel.Size = New System.Drawing.Size(45, 23)
        Me.ButtonExcel.TabIndex = 23
        Me.ButtonExcel.Text = ":::"
        Me.ToolTip1.SetToolTip(Me.ButtonExcel, "Selección de la Hoja de Excel ")
        Me.ButtonExcel.UseVisualStyleBackColor = True
        '
        'ButtonImprimir
        '
        Me.ButtonImprimir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonImprimir.Location = New System.Drawing.Point(12, 172)
        Me.ButtonImprimir.Name = "ButtonImprimir"
        Me.ButtonImprimir.Size = New System.Drawing.Size(75, 23)
        Me.ButtonImprimir.TabIndex = 22
        Me.ButtonImprimir.Text = "Imprimir"
        Me.ButtonImprimir.UseVisualStyleBackColor = True
        '
        'TextBoxTotalDebe
        '
        Me.TextBoxTotalDebe.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxTotalDebe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxTotalDebe.Location = New System.Drawing.Point(384, 268)
        Me.TextBoxTotalDebe.Name = "TextBoxTotalDebe"
        Me.TextBoxTotalDebe.Size = New System.Drawing.Size(70, 20)
        Me.TextBoxTotalDebe.TabIndex = 19
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(305, 272)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(73, 13)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "Debe / Haber"
        '
        'TextBoxTotalHaber
        '
        Me.TextBoxTotalHaber.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxTotalHaber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxTotalHaber.Location = New System.Drawing.Point(460, 268)
        Me.TextBoxTotalHaber.Name = "TextBoxTotalHaber"
        Me.TextBoxTotalHaber.Size = New System.Drawing.Size(67, 20)
        Me.TextBoxTotalHaber.TabIndex = 20
        '
        'TextBoxErrores
        '
        Me.TextBoxErrores.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxErrores.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxErrores.Location = New System.Drawing.Point(606, 293)
        Me.TextBoxErrores.Name = "TextBoxErrores"
        Me.TextBoxErrores.Size = New System.Drawing.Size(54, 20)
        Me.TextBoxErrores.TabIndex = 22
        '
        'TextBoxCuadre
        '
        Me.TextBoxCuadre.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxCuadre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCuadre.Location = New System.Drawing.Point(533, 268)
        Me.TextBoxCuadre.Name = "TextBoxCuadre"
        Me.TextBoxCuadre.Size = New System.Drawing.Size(67, 20)
        Me.TextBoxCuadre.TabIndex = 23
        '
        'CheckBoxAjustar
        '
        Me.CheckBoxAjustar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxAjustar.AutoSize = True
        Me.CheckBoxAjustar.Location = New System.Drawing.Point(612, 270)
        Me.CheckBoxAjustar.Name = "CheckBoxAjustar"
        Me.CheckBoxAjustar.Size = New System.Drawing.Size(58, 17)
        Me.CheckBoxAjustar.TabIndex = 24
        Me.CheckBoxAjustar.Text = "Ajustar"
        Me.CheckBoxAjustar.UseVisualStyleBackColor = True
        '
        'ButtonImprimirErrores
        '
        Me.ButtonImprimirErrores.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonImprimirErrores.Location = New System.Drawing.Point(606, 319)
        Me.ButtonImprimirErrores.Name = "ButtonImprimirErrores"
        Me.ButtonImprimirErrores.Size = New System.Drawing.Size(55, 23)
        Me.ButtonImprimirErrores.TabIndex = 25
        Me.ButtonImprimirErrores.Text = "Imprimir"
        Me.ButtonImprimirErrores.UseVisualStyleBackColor = True
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 10000
        Me.ToolTip1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.ToolTip1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.ToolTip1.InitialDelay = 500
        Me.ToolTip1.ReshowDelay = 50
        Me.ToolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
        '
        'TextBoxHojaNif
        '
        Me.TextBoxHojaNif.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxHojaNif.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxHojaNif.Location = New System.Drawing.Point(452, 34)
        Me.TextBoxHojaNif.Name = "TextBoxHojaNif"
        Me.TextBoxHojaNif.Size = New System.Drawing.Size(140, 20)
        Me.TextBoxHojaNif.TabIndex = 18
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(391, 41)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(45, 13)
        Me.Label6.TabIndex = 19
        Me.Label6.Text = "Hoja Nif"
        '
        'FormIntegraNominaA3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(776, 468)
        Me.Controls.Add(Me.ButtonImprimirErrores)
        Me.Controls.Add(Me.CheckBoxAjustar)
        Me.Controls.Add(Me.TextBoxCuadre)
        Me.Controls.Add(Me.TextBoxErrores)
        Me.Controls.Add(Me.TextBoxTotalHaber)
        Me.Controls.Add(Me.TextBoxTotalDebe)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.DataGridAsientos)
        Me.Controls.Add(Me.TextBoxTotalBruto)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TextBoxTotalProductores)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBoxDebug)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ListBox1)
        Me.MinimumSize = New System.Drawing.Size(784, 495)
        Me.Name = "FormIntegraNominaA3"
        Me.Text = "Nómina"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.DataGridHoteles, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridAsientos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonRutaExcel As System.Windows.Forms.Button
    Friend WithEvents TextBoxRutaLibroExcel As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents TextBoxDebug As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBoxTotalProductores As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxTotalBruto As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents DataGridHoteles As System.Windows.Forms.DataGrid
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents DataGridAsientos As System.Windows.Forms.DataGrid
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonImprimir As System.Windows.Forms.Button
    Friend WithEvents TextBoxTotalDebe As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextBoxTotalHaber As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxErrores As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCuadre As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxAjustar As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonImprimirErrores As System.Windows.Forms.Button
    Friend WithEvents TextBoxHojaExcel As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ButtonExcel As System.Windows.Forms.Button
    Friend WithEvents ListBoxLibros As System.Windows.Forms.ListBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextBoxHojaNif As System.Windows.Forms.TextBox
End Class
