<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormParametrosNomina
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormParametrosNomina))
        Me.TextBoxEmpCod = New System.Windows.Forms.TextBox
        Me.ButtonCancelar = New System.Windows.Forms.Button
        Me.Label78 = New System.Windows.Forms.Label
        Me.TextBoxEmpNum = New System.Windows.Forms.TextBox
        Me.Label77 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ComboBoxEmpCod = New System.Windows.Forms.ComboBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPageGenerales = New System.Windows.Forms.TabPage
        Me.ButtonFileDestino = New System.Windows.Forms.Button
        Me.ButtonFileOrigen = New System.Windows.Forms.Button
        Me.TextBoxFileDestino = New System.Windows.Forms.TextBox
        Me.TextBoxFileOrigen = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.TabPageHotelesLopez = New System.Windows.Forms.TabPage
        Me.Label17 = New System.Windows.Forms.Label
        Me.TextBoxCtaBamco = New System.Windows.Forms.TextBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.TextBoxCtaOtrosDtos = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.TextBoxCtaIndemniza = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.TextBoxCtaIrpf = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.TextBoxCtaSocialEmpresa = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextBoxCtaSocialTotal = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextBoxCtaSocialPersonal = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextBoxCtaAnticipos = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextBoxCtaEmbargos = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextBoxCtaNeto = New System.Windows.Forms.TextBox
        Me.TextBoxCtaBruto = New System.Windows.Forms.TextBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.TabPageSpyro = New System.Windows.Forms.TabPage
        Me.Label16 = New System.Windows.Forms.Label
        Me.TextBoxCfatoTipCod = New System.Windows.Forms.TextBox
        Me.TextBoxCfatoDiariCod = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.ComboBoxGrupoCod = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.TabControl1.SuspendLayout()
        Me.TabPageGenerales.SuspendLayout()
        Me.TabPageHotelesLopez.SuspendLayout()
        Me.TabPageSpyro.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBoxEmpCod
        '
        Me.TextBoxEmpCod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxEmpCod.Enabled = False
        Me.TextBoxEmpCod.Location = New System.Drawing.Point(555, 14)
        Me.TextBoxEmpCod.Name = "TextBoxEmpCod"
        Me.TextBoxEmpCod.Size = New System.Drawing.Size(32, 20)
        Me.TextBoxEmpCod.TabIndex = 49
        '
        'ButtonCancelar
        '
        Me.ButtonCancelar.Location = New System.Drawing.Point(21, 59)
        Me.ButtonCancelar.Name = "ButtonCancelar"
        Me.ButtonCancelar.Size = New System.Drawing.Size(80, 23)
        Me.ButtonCancelar.TabIndex = 13
        Me.ButtonCancelar.Text = "&Cancelar"
        '
        'Label78
        '
        Me.Label78.AutoSize = True
        Me.Label78.Location = New System.Drawing.Point(493, 12)
        Me.Label78.Name = "Label78"
        Me.Label78.Size = New System.Drawing.Size(56, 13)
        Me.Label78.TabIndex = 48
        Me.Label78.Text = "Emp. Cod."
        '
        'TextBoxEmpNum
        '
        Me.TextBoxEmpNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxEmpNum.Enabled = False
        Me.TextBoxEmpNum.Location = New System.Drawing.Point(681, 14)
        Me.TextBoxEmpNum.Name = "TextBoxEmpNum"
        Me.TextBoxEmpNum.Size = New System.Drawing.Size(46, 20)
        Me.TextBoxEmpNum.TabIndex = 47
        '
        'Label77
        '
        Me.Label77.AutoSize = True
        Me.Label77.Location = New System.Drawing.Point(603, 12)
        Me.Label77.Name = "Label77"
        Me.Label77.Size = New System.Drawing.Size(72, 13)
        Me.Label77.TabIndex = 46
        Me.Label77.Text = "Hotel Nùmero"
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Image = CType(resources.GetObject("ButtonAceptar.Image"), System.Drawing.Image)
        Me.ButtonAceptar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonAceptar.Location = New System.Drawing.Point(21, 19)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(80, 23)
        Me.ButtonAceptar.TabIndex = 14
        Me.ButtonAceptar.Text = "&Aceptar"
        '
        'ComboBoxEmpCod
        '
        Me.ComboBoxEmpCod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxEmpCod.DropDownWidth = 250
        Me.ComboBoxEmpCod.Location = New System.Drawing.Point(265, 12)
        Me.ComboBoxEmpCod.Name = "ComboBoxEmpCod"
        Me.ComboBoxEmpCod.Size = New System.Drawing.Size(208, 21)
        Me.ComboBoxEmpCod.TabIndex = 45
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(201, 12)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(64, 23)
        Me.Label11.TabIndex = 44
        Me.Label11.Text = "Emp. cod"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPageGenerales)
        Me.TabControl1.Controls.Add(Me.TabPageHotelesLopez)
        Me.TabControl1.Controls.Add(Me.TabPageSpyro)
        Me.TabControl1.Location = New System.Drawing.Point(15, 19)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(670, 435)
        Me.TabControl1.TabIndex = 0
        '
        'TabPageGenerales
        '
        Me.TabPageGenerales.Controls.Add(Me.ButtonFileDestino)
        Me.TabPageGenerales.Controls.Add(Me.ButtonFileOrigen)
        Me.TabPageGenerales.Controls.Add(Me.TextBoxFileDestino)
        Me.TabPageGenerales.Controls.Add(Me.TextBoxFileOrigen)
        Me.TabPageGenerales.Controls.Add(Me.Label8)
        Me.TabPageGenerales.Controls.Add(Me.Label7)
        Me.TabPageGenerales.Location = New System.Drawing.Point(4, 22)
        Me.TabPageGenerales.Name = "TabPageGenerales"
        Me.TabPageGenerales.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageGenerales.Size = New System.Drawing.Size(662, 409)
        Me.TabPageGenerales.TabIndex = 0
        Me.TabPageGenerales.Text = "Generales"
        Me.TabPageGenerales.UseVisualStyleBackColor = True
        '
        'ButtonFileDestino
        '
        Me.ButtonFileDestino.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonFileDestino.Location = New System.Drawing.Point(604, 49)
        Me.ButtonFileDestino.Name = "ButtonFileDestino"
        Me.ButtonFileDestino.Size = New System.Drawing.Size(38, 23)
        Me.ButtonFileDestino.TabIndex = 5
        Me.ButtonFileDestino.Text = ":::"
        Me.ButtonFileDestino.UseVisualStyleBackColor = True
        '
        'ButtonFileOrigen
        '
        Me.ButtonFileOrigen.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonFileOrigen.Location = New System.Drawing.Point(604, 14)
        Me.ButtonFileOrigen.Name = "ButtonFileOrigen"
        Me.ButtonFileOrigen.Size = New System.Drawing.Size(38, 23)
        Me.ButtonFileOrigen.TabIndex = 4
        Me.ButtonFileOrigen.Text = ":::"
        Me.ButtonFileOrigen.UseVisualStyleBackColor = True
        '
        'TextBoxFileDestino
        '
        Me.TextBoxFileDestino.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxFileDestino.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxFileDestino.Location = New System.Drawing.Point(196, 49)
        Me.TextBoxFileDestino.Name = "TextBoxFileDestino"
        Me.TextBoxFileDestino.ReadOnly = True
        Me.TextBoxFileDestino.Size = New System.Drawing.Size(402, 20)
        Me.TextBoxFileDestino.TabIndex = 3
        '
        'TextBoxFileOrigen
        '
        Me.TextBoxFileOrigen.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxFileOrigen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxFileOrigen.Location = New System.Drawing.Point(196, 17)
        Me.TextBoxFileOrigen.Name = "TextBoxFileOrigen"
        Me.TextBoxFileOrigen.ReadOnly = True
        Me.TextBoxFileOrigen.Size = New System.Drawing.Size(402, 20)
        Me.TextBoxFileOrigen.TabIndex = 2
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(7, 51)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(173, 13)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "Ruta Destino Ficheros Contabilidad"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(7, 19)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(134, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Ruta Origen Datos Nómina"
        '
        'TabPageHotelesLopez
        '
        Me.TabPageHotelesLopez.Controls.Add(Me.Label17)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaBamco)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label20)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label15)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label14)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaOtrosDtos)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label13)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaIndemniza)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label12)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaIrpf)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label6)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaSocialEmpresa)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label5)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaSocialTotal)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label4)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaSocialPersonal)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label3)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaAnticipos)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label2)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaEmbargos)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label1)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaNeto)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaBruto)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label19)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label18)
        Me.TabPageHotelesLopez.Location = New System.Drawing.Point(4, 22)
        Me.TabPageHotelesLopez.Name = "TabPageHotelesLopez"
        Me.TabPageHotelesLopez.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageHotelesLopez.Size = New System.Drawing.Size(662, 409)
        Me.TabPageHotelesLopez.TabIndex = 1
        Me.TabPageHotelesLopez.Text = "Hoteles Lopez"
        Me.TabPageHotelesLopez.UseVisualStyleBackColor = True
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(333, 373)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(62, 13)
        Me.Label17.TabIndex = 44
        Me.Label17.Text = "+ Productor"
        '
        'TextBoxCtaBamco
        '
        Me.TextBoxCtaBamco.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaBamco.Location = New System.Drawing.Point(203, 365)
        Me.TextBoxCtaBamco.Name = "TextBoxCtaBamco"
        Me.TextBoxCtaBamco.Size = New System.Drawing.Size(110, 20)
        Me.TextBoxCtaBamco.TabIndex = 43
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label20.Location = New System.Drawing.Point(7, 372)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(164, 13)
        Me.Label20.TabIndex = 42
        Me.Label20.Text = "Prefijo cuenta BANCO (6 Dígitos)"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(333, 47)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(62, 13)
        Me.Label15.TabIndex = 41
        Me.Label15.Text = "+ Productor"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(333, 20)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(62, 13)
        Me.Label14.TabIndex = 40
        Me.Label14.Text = "+ Productor"
        '
        'TextBoxCtaOtrosDtos
        '
        Me.TextBoxCtaOtrosDtos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaOtrosDtos.Location = New System.Drawing.Point(203, 309)
        Me.TextBoxCtaOtrosDtos.Name = "TextBoxCtaOtrosDtos"
        Me.TextBoxCtaOtrosDtos.Size = New System.Drawing.Size(154, 20)
        Me.TextBoxCtaOtrosDtos.TabIndex = 39
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label13.Location = New System.Drawing.Point(7, 311)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(152, 13)
        Me.Label13.TabIndex = 38
        Me.Label13.Text = "Cuenta Otros Dtos (12 Dígitos)"
        '
        'TextBoxCtaIndemniza
        '
        Me.TextBoxCtaIndemniza.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaIndemniza.Location = New System.Drawing.Point(203, 274)
        Me.TextBoxCtaIndemniza.Name = "TextBoxCtaIndemniza"
        Me.TextBoxCtaIndemniza.Size = New System.Drawing.Size(154, 20)
        Me.TextBoxCtaIndemniza.TabIndex = 37
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label12.Location = New System.Drawing.Point(7, 276)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(150, 13)
        Me.Label12.TabIndex = 36
        Me.Label12.Text = "Cuenta Indemniza (12 Dígitos)"
        '
        'TextBoxCtaIrpf
        '
        Me.TextBoxCtaIrpf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaIrpf.Location = New System.Drawing.Point(203, 238)
        Me.TextBoxCtaIrpf.Name = "TextBoxCtaIrpf"
        Me.TextBoxCtaIrpf.Size = New System.Drawing.Size(154, 20)
        Me.TextBoxCtaIrpf.TabIndex = 35
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(7, 240)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(126, 13)
        Me.Label6.TabIndex = 34
        Me.Label6.Text = "Cuenta IRPF (12 Dígitos)"
        '
        'TextBoxCtaSocialEmpresa
        '
        Me.TextBoxCtaSocialEmpresa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaSocialEmpresa.Location = New System.Drawing.Point(203, 207)
        Me.TextBoxCtaSocialEmpresa.Name = "TextBoxCtaSocialEmpresa"
        Me.TextBoxCtaSocialEmpresa.Size = New System.Drawing.Size(154, 20)
        Me.TextBoxCtaSocialEmpresa.TabIndex = 33
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(7, 209)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(185, 13)
        Me.Label5.TabIndex = 32
        Me.Label5.Text = "Cuenta S Social Empresa (12 Dígitos)"
        '
        'TextBoxCtaSocialTotal
        '
        Me.TextBoxCtaSocialTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaSocialTotal.Location = New System.Drawing.Point(203, 174)
        Me.TextBoxCtaSocialTotal.Name = "TextBoxCtaSocialTotal"
        Me.TextBoxCtaSocialTotal.Size = New System.Drawing.Size(154, 20)
        Me.TextBoxCtaSocialTotal.TabIndex = 31
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(7, 176)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(168, 13)
        Me.Label4.TabIndex = 30
        Me.Label4.Text = "Cuenta S Social Total (12 Dígitos)"
        '
        'TextBoxCtaSocialPersonal
        '
        Me.TextBoxCtaSocialPersonal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaSocialPersonal.Location = New System.Drawing.Point(203, 144)
        Me.TextBoxCtaSocialPersonal.Name = "TextBoxCtaSocialPersonal"
        Me.TextBoxCtaSocialPersonal.Size = New System.Drawing.Size(154, 20)
        Me.TextBoxCtaSocialPersonal.TabIndex = 29
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(7, 146)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(185, 13)
        Me.Label3.TabIndex = 28
        Me.Label3.Text = "Cuenta S Social Personal (12 Dígitos)"
        '
        'TextBoxCtaAnticipos
        '
        Me.TextBoxCtaAnticipos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaAnticipos.Location = New System.Drawing.Point(203, 118)
        Me.TextBoxCtaAnticipos.Name = "TextBoxCtaAnticipos"
        Me.TextBoxCtaAnticipos.Size = New System.Drawing.Size(154, 20)
        Me.TextBoxCtaAnticipos.TabIndex = 27
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(7, 120)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(145, 13)
        Me.Label2.TabIndex = 26
        Me.Label2.Text = "Cuenta Anticipos (12 Dígitos)"
        '
        'TextBoxCtaEmbargos
        '
        Me.TextBoxCtaEmbargos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaEmbargos.Location = New System.Drawing.Point(203, 92)
        Me.TextBoxCtaEmbargos.Name = "TextBoxCtaEmbargos"
        Me.TextBoxCtaEmbargos.Size = New System.Drawing.Size(154, 20)
        Me.TextBoxCtaEmbargos.TabIndex = 25
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(7, 94)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(149, 13)
        Me.Label1.TabIndex = 24
        Me.Label1.Text = "Cuenta Embargos (12 Dígitos)"
        '
        'TextBoxCtaNeto
        '
        Me.TextBoxCtaNeto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaNeto.Location = New System.Drawing.Point(203, 47)
        Me.TextBoxCtaNeto.Name = "TextBoxCtaNeto"
        Me.TextBoxCtaNeto.Size = New System.Drawing.Size(110, 20)
        Me.TextBoxCtaNeto.TabIndex = 23
        '
        'TextBoxCtaBruto
        '
        Me.TextBoxCtaBruto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaBruto.Location = New System.Drawing.Point(203, 19)
        Me.TextBoxCtaBruto.Name = "TextBoxCtaBruto"
        Me.TextBoxCtaBruto.Size = New System.Drawing.Size(110, 20)
        Me.TextBoxCtaBruto.TabIndex = 22
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label19.Location = New System.Drawing.Point(7, 49)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(157, 13)
        Me.Label19.TabIndex = 21
        Me.Label19.Text = "Prefijo cuenta NETO (6 Dígitos)"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label18.Location = New System.Drawing.Point(7, 19)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(165, 13)
        Me.Label18.TabIndex = 20
        Me.Label18.Text = "Prefijo cuenta BRUTO (6 Dígitos)"
        '
        'TabPageSpyro
        '
        Me.TabPageSpyro.Controls.Add(Me.Label16)
        Me.TabPageSpyro.Controls.Add(Me.TextBoxCfatoTipCod)
        Me.TabPageSpyro.Controls.Add(Me.TextBoxCfatoDiariCod)
        Me.TabPageSpyro.Controls.Add(Me.Label9)
        Me.TabPageSpyro.Location = New System.Drawing.Point(4, 22)
        Me.TabPageSpyro.Name = "TabPageSpyro"
        Me.TabPageSpyro.Size = New System.Drawing.Size(662, 409)
        Me.TabPageSpyro.TabIndex = 2
        Me.TabPageSpyro.Text = "Spyro"
        Me.TabPageSpyro.UseVisualStyleBackColor = True
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label16.Location = New System.Drawing.Point(231, 19)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(165, 13)
        Me.Label16.TabIndex = 27
        Me.Label16.Text = "Tipo de Asiento CFATOTIP_COD"
        '
        'TextBoxCfatoTipCod
        '
        Me.TextBoxCfatoTipCod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCfatoTipCod.Location = New System.Drawing.Point(417, 19)
        Me.TextBoxCfatoTipCod.Name = "TextBoxCfatoTipCod"
        Me.TextBoxCfatoTipCod.Size = New System.Drawing.Size(110, 20)
        Me.TextBoxCfatoTipCod.TabIndex = 26
        '
        'TextBoxCfatoDiariCod
        '
        Me.TextBoxCfatoDiariCod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCfatoDiariCod.Location = New System.Drawing.Point(53, 19)
        Me.TextBoxCfatoDiariCod.Name = "TextBoxCfatoDiariCod"
        Me.TextBoxCfatoDiariCod.Size = New System.Drawing.Size(152, 20)
        Me.TextBoxCfatoDiariCod.TabIndex = 24
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(7, 19)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(34, 13)
        Me.Label9.TabIndex = 23
        Me.Label9.Text = "Diario"
        '
        'ComboBoxGrupoCod
        '
        Me.ComboBoxGrupoCod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxGrupoCod.Location = New System.Drawing.Point(73, 12)
        Me.ComboBoxGrupoCod.Name = "ComboBoxGrupoCod"
        Me.ComboBoxGrupoCod.Size = New System.Drawing.Size(121, 21)
        Me.ComboBoxGrupoCod.TabIndex = 43
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(9, 12)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(64, 23)
        Me.Label10.TabIndex = 42
        Me.Label10.Text = "Grupo cod"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.ButtonAceptar)
        Me.GroupBox2.Controls.Add(Me.ButtonCancelar)
        Me.GroupBox2.Location = New System.Drawing.Point(709, 51)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(108, 472)
        Me.GroupBox2.TabIndex = 41
        Me.GroupBox2.TabStop = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.TabControl1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 51)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(691, 472)
        Me.GroupBox1.TabIndex = 40
        Me.GroupBox1.TabStop = False
        '
        'FormParametrosNomina
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(836, 565)
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
        Me.Name = "FormParametrosNomina"
        Me.Text = "Parámetros Integración Nómina"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPageGenerales.ResumeLayout(False)
        Me.TabPageGenerales.PerformLayout()
        Me.TabPageHotelesLopez.ResumeLayout(False)
        Me.TabPageHotelesLopez.PerformLayout()
        Me.TabPageSpyro.ResumeLayout(False)
        Me.TabPageSpyro.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBoxEmpCod As System.Windows.Forms.TextBox
    Friend WithEvents ButtonCancelar As System.Windows.Forms.Button
    Friend WithEvents Label78 As System.Windows.Forms.Label
    Friend WithEvents TextBoxEmpNum As System.Windows.Forms.TextBox
    Friend WithEvents Label77 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ComboBoxEmpCod As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPageGenerales As System.Windows.Forms.TabPage
    Friend WithEvents ComboBoxGrupoCod As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TabPageHotelesLopez As System.Windows.Forms.TabPage
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaNeto As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCtaBruto As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCtaIrpf As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaSocialEmpresa As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaSocialTotal As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaSocialPersonal As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaAnticipos As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaEmbargos As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonFileDestino As System.Windows.Forms.Button
    Friend WithEvents ButtonFileOrigen As System.Windows.Forms.Button
    Friend WithEvents TextBoxFileDestino As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxFileOrigen As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TabPageSpyro As System.Windows.Forms.TabPage
    Friend WithEvents TextBoxCfatoDiariCod As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaOtrosDtos As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaIndemniza As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents TextBoxCfatoTipCod As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaBamco As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
End Class
