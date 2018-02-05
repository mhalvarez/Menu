<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormParametrosAlmacen
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormParametrosAlmacen))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPageGenerales = New System.Windows.Forms.TabPage()
        Me.TextBoxHotelId = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.TextBoxHaberAbonos = New System.Windows.Forms.TextBox()
        Me.TextBoxDebeAbonos = New System.Windows.Forms.TextBox()
        Me.TextBoxHaberFac = New System.Windows.Forms.TextBox()
        Me.TextBoxDebeFac = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.CheckBoxSoloFacturas = New System.Windows.Forms.CheckBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.CheckBoxUsaNewPaga = New System.Windows.Forms.CheckBox()
        Me.TabPageHotelesLopez = New System.Windows.Forms.TabPage()
        Me.GroupBoxSeriesdefactura = New System.Windows.Forms.GroupBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.CheckBoxSerieFacturaDigitosAnio = New System.Windows.Forms.CheckBox()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.TextBoxSerieFacturaPorDepartamentos = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.TextBoxSerieFacturaNewPaga = New System.Windows.Forms.TextBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.TextBoxSerieFacturaNewStock = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.CheckBoxSerieFacturaPorDepartamento = New System.Windows.Forms.CheckBox()
        Me.CheckBoxBuscaCuentasNewCentral = New System.Windows.Forms.CheckBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TextBoxCtaCentro4 = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.TextBoxCtaRaiz4 = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.TextBoxCtaActividad4 = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TextBoxCtaCentro6 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TextBoxCtaRaiz6 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBoxCtaActividad6 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.ButtonAceptar = New System.Windows.Forms.Button()
        Me.ButtonCancelar = New System.Windows.Forms.Button()
        Me.ComboBoxEmpCod = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.ComboBoxGrupoCod = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TextBoxEmpCod = New System.Windows.Forms.TextBox()
        Me.Label78 = New System.Windows.Forms.Label()
        Me.TextBoxEmpNum = New System.Windows.Forms.TextBox()
        Me.Label77 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPageGenerales.SuspendLayout()
        Me.TabPageHotelesLopez.SuspendLayout()
        Me.GroupBoxSeriesdefactura.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.TabControl1)
        Me.GroupBox1.Location = New System.Drawing.Point(19, 48)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(754, 474)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPageGenerales)
        Me.TabControl1.Controls.Add(Me.TabPageHotelesLopez)
        Me.TabControl1.Location = New System.Drawing.Point(15, 19)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(733, 437)
        Me.TabControl1.TabIndex = 0
        '
        'TabPageGenerales
        '
        Me.TabPageGenerales.Controls.Add(Me.TextBoxHotelId)
        Me.TabPageGenerales.Controls.Add(Me.Label22)
        Me.TabPageGenerales.Controls.Add(Me.TextBoxHaberAbonos)
        Me.TabPageGenerales.Controls.Add(Me.TextBoxDebeAbonos)
        Me.TabPageGenerales.Controls.Add(Me.TextBoxHaberFac)
        Me.TabPageGenerales.Controls.Add(Me.TextBoxDebeFac)
        Me.TabPageGenerales.Controls.Add(Me.Label20)
        Me.TabPageGenerales.Controls.Add(Me.Label21)
        Me.TabPageGenerales.Controls.Add(Me.Label19)
        Me.TabPageGenerales.Controls.Add(Me.Label18)
        Me.TabPageGenerales.Controls.Add(Me.CheckBoxSoloFacturas)
        Me.TabPageGenerales.Controls.Add(Me.Label17)
        Me.TabPageGenerales.Controls.Add(Me.CheckBoxUsaNewPaga)
        Me.TabPageGenerales.Location = New System.Drawing.Point(4, 22)
        Me.TabPageGenerales.Name = "TabPageGenerales"
        Me.TabPageGenerales.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageGenerales.Size = New System.Drawing.Size(725, 411)
        Me.TabPageGenerales.TabIndex = 0
        Me.TabPageGenerales.Text = "Generales"
        Me.TabPageGenerales.UseVisualStyleBackColor = True
        '
        'TextBoxHotelId
        '
        Me.TextBoxHotelId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxHotelId.Enabled = False
        Me.TextBoxHotelId.Location = New System.Drawing.Point(151, 206)
        Me.TextBoxHotelId.MaxLength = 4
        Me.TextBoxHotelId.Name = "TextBoxHotelId"
        Me.TextBoxHotelId.Size = New System.Drawing.Size(67, 20)
        Me.TextBoxHotelId.TabIndex = 21
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(12, 213)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(89, 13)
        Me.Label22.TabIndex = 20
        Me.Label22.Text = "Hotel Id  (sin uso)"
        '
        'TextBoxHaberAbonos
        '
        Me.TextBoxHaberAbonos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxHaberAbonos.Location = New System.Drawing.Point(390, 160)
        Me.TextBoxHaberAbonos.MaxLength = 4
        Me.TextBoxHaberAbonos.Name = "TextBoxHaberAbonos"
        Me.TextBoxHaberAbonos.Size = New System.Drawing.Size(67, 20)
        Me.TextBoxHaberAbonos.TabIndex = 19
        '
        'TextBoxDebeAbonos
        '
        Me.TextBoxDebeAbonos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDebeAbonos.Location = New System.Drawing.Point(390, 134)
        Me.TextBoxDebeAbonos.MaxLength = 4
        Me.TextBoxDebeAbonos.Name = "TextBoxDebeAbonos"
        Me.TextBoxDebeAbonos.Size = New System.Drawing.Size(67, 20)
        Me.TextBoxDebeAbonos.TabIndex = 18
        '
        'TextBoxHaberFac
        '
        Me.TextBoxHaberFac.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxHaberFac.Location = New System.Drawing.Point(151, 160)
        Me.TextBoxHaberFac.MaxLength = 4
        Me.TextBoxHaberFac.Name = "TextBoxHaberFac"
        Me.TextBoxHaberFac.Size = New System.Drawing.Size(67, 20)
        Me.TextBoxHaberFac.TabIndex = 17
        '
        'TextBoxDebeFac
        '
        Me.TextBoxDebeFac.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDebeFac.Location = New System.Drawing.Point(151, 134)
        Me.TextBoxDebeFac.MaxLength = 4
        Me.TextBoxDebeFac.Name = "TextBoxDebeFac"
        Me.TextBoxDebeFac.Size = New System.Drawing.Size(67, 20)
        Me.TextBoxDebeFac.TabIndex = 16
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(251, 160)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(122, 13)
        Me.Label20.TabIndex = 6
        Me.Label20.Text = "Indicador Haber Abonos"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(251, 134)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(119, 13)
        Me.Label21.TabIndex = 5
        Me.Label21.Text = "Indicador Debe Abonos"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(12, 160)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(127, 13)
        Me.Label19.TabIndex = 4
        Me.Label19.Text = "Indicador Haber Facturas"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(12, 134)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(124, 13)
        Me.Label18.TabIndex = 3
        Me.Label18.Text = "Indicador Debe Facturas"
        '
        'CheckBoxSoloFacturas
        '
        Me.CheckBoxSoloFacturas.AutoSize = True
        Me.CheckBoxSoloFacturas.Location = New System.Drawing.Point(9, 83)
        Me.CheckBoxSoloFacturas.Name = "CheckBoxSoloFacturas"
        Me.CheckBoxSoloFacturas.Size = New System.Drawing.Size(91, 17)
        Me.CheckBoxSoloFacturas.TabIndex = 2
        Me.CheckBoxSoloFacturas.Text = "Solo Facturas"
        Me.CheckBoxSoloFacturas.UseVisualStyleBackColor = True
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(6, 18)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(93, 13)
        Me.Label17.TabIndex = 1
        Me.Label17.Text = "Cuentas Comunes"
        '
        'CheckBoxUsaNewPaga
        '
        Me.CheckBoxUsaNewPaga.AutoSize = True
        Me.CheckBoxUsaNewPaga.Location = New System.Drawing.Point(9, 60)
        Me.CheckBoxUsaNewPaga.Name = "CheckBoxUsaNewPaga"
        Me.CheckBoxUsaNewPaga.Size = New System.Drawing.Size(95, 17)
        Me.CheckBoxUsaNewPaga.TabIndex = 0
        Me.CheckBoxUsaNewPaga.Text = "Usa NewPaga"
        Me.CheckBoxUsaNewPaga.UseVisualStyleBackColor = True
        '
        'TabPageHotelesLopez
        '
        Me.TabPageHotelesLopez.Controls.Add(Me.GroupBoxSeriesdefactura)
        Me.TabPageHotelesLopez.Controls.Add(Me.CheckBoxBuscaCuentasNewCentral)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label9)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaCentro4)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label12)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label13)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaRaiz4)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label14)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label15)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaActividad4)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label16)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label8)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label7)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaCentro6)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label6)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label5)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaRaiz6)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label4)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label3)
        Me.TabPageHotelesLopez.Controls.Add(Me.TextBoxCtaActividad6)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label2)
        Me.TabPageHotelesLopez.Controls.Add(Me.Label1)
        Me.TabPageHotelesLopez.Location = New System.Drawing.Point(4, 22)
        Me.TabPageHotelesLopez.Name = "TabPageHotelesLopez"
        Me.TabPageHotelesLopez.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageHotelesLopez.Size = New System.Drawing.Size(725, 411)
        Me.TabPageHotelesLopez.TabIndex = 1
        Me.TabPageHotelesLopez.Text = "Hoteles Lopez"
        Me.TabPageHotelesLopez.UseVisualStyleBackColor = True
        '
        'GroupBoxSeriesdefactura
        '
        Me.GroupBoxSeriesdefactura.Controls.Add(Me.Label25)
        Me.GroupBoxSeriesdefactura.Controls.Add(Me.CheckBoxSerieFacturaDigitosAnio)
        Me.GroupBoxSeriesdefactura.Controls.Add(Me.Label27)
        Me.GroupBoxSeriesdefactura.Controls.Add(Me.TextBoxSerieFacturaPorDepartamentos)
        Me.GroupBoxSeriesdefactura.Controls.Add(Me.Label26)
        Me.GroupBoxSeriesdefactura.Controls.Add(Me.TextBoxSerieFacturaNewPaga)
        Me.GroupBoxSeriesdefactura.Controls.Add(Me.Label24)
        Me.GroupBoxSeriesdefactura.Controls.Add(Me.TextBoxSerieFacturaNewStock)
        Me.GroupBoxSeriesdefactura.Controls.Add(Me.Label23)
        Me.GroupBoxSeriesdefactura.Controls.Add(Me.CheckBoxSerieFacturaPorDepartamento)
        Me.GroupBoxSeriesdefactura.Location = New System.Drawing.Point(16, 219)
        Me.GroupBoxSeriesdefactura.Name = "GroupBoxSeriesdefactura"
        Me.GroupBoxSeriesdefactura.Size = New System.Drawing.Size(691, 186)
        Me.GroupBoxSeriesdefactura.TabIndex = 21
        Me.GroupBoxSeriesdefactura.TabStop = False
        Me.GroupBoxSeriesdefactura.Text = "Series de Facturas de Compra (Spyro)"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.ForeColor = System.Drawing.Color.Maroon
        Me.Label25.Location = New System.Drawing.Point(485, 36)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(16, 13)
        Me.Label25.TabIndex = 25
        Me.Label25.Text = "+ "
        '
        'CheckBoxSerieFacturaDigitosAnio
        '
        Me.CheckBoxSerieFacturaDigitosAnio.AutoSize = True
        Me.CheckBoxSerieFacturaDigitosAnio.Location = New System.Drawing.Point(517, 37)
        Me.CheckBoxSerieFacturaDigitosAnio.Name = "CheckBoxSerieFacturaDigitosAnio"
        Me.CheckBoxSerieFacturaDigitosAnio.Size = New System.Drawing.Size(152, 17)
        Me.CheckBoxSerieFacturaDigitosAnio.TabIndex = 24
        Me.CheckBoxSerieFacturaDigitosAnio.Text = "Usar Ejercicio de 2 Dígitos"
        Me.CheckBoxSerieFacturaDigitosAnio.UseVisualStyleBackColor = True
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.ForeColor = System.Drawing.Color.Maroon
        Me.Label27.Location = New System.Drawing.Point(131, 38)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(16, 13)
        Me.Label27.TabIndex = 23
        Me.Label27.Text = "+ "
        '
        'TextBoxSerieFacturaPorDepartamentos
        '
        Me.TextBoxSerieFacturaPorDepartamentos.BackColor = System.Drawing.SystemColors.Info
        Me.TextBoxSerieFacturaPorDepartamentos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxSerieFacturaPorDepartamentos.Location = New System.Drawing.Point(183, 65)
        Me.TextBoxSerieFacturaPorDepartamentos.Multiline = True
        Me.TextBoxSerieFacturaPorDepartamentos.Name = "TextBoxSerieFacturaPorDepartamentos"
        Me.TextBoxSerieFacturaPorDepartamentos.ReadOnly = True
        Me.TextBoxSerieFacturaPorDepartamentos.Size = New System.Drawing.Size(296, 102)
        Me.TextBoxSerieFacturaPorDepartamentos.TabIndex = 22
        Me.TextBoxSerieFacturaPorDepartamentos.Text = resources.GetString("TextBoxSerieFacturaPorDepartamentos.Text")
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(120, 130)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(59, 13)
        Me.Label26.TabIndex = 21
        Me.Label26.Text = " + Ejercicio"
        '
        'TextBoxSerieFacturaNewPaga
        '
        Me.TextBoxSerieFacturaNewPaga.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxSerieFacturaNewPaga.Location = New System.Drawing.Point(65, 130)
        Me.TextBoxSerieFacturaNewPaga.Name = "TextBoxSerieFacturaNewPaga"
        Me.TextBoxSerieFacturaNewPaga.Size = New System.Drawing.Size(49, 20)
        Me.TextBoxSerieFacturaNewPaga.TabIndex = 19
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(5, 130)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(54, 13)
        Me.Label24.TabIndex = 18
        Me.Label24.Text = "NewPaga"
        '
        'TextBoxSerieFacturaNewStock
        '
        Me.TextBoxSerieFacturaNewStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxSerieFacturaNewStock.Location = New System.Drawing.Point(65, 34)
        Me.TextBoxSerieFacturaNewStock.Name = "TextBoxSerieFacturaNewStock"
        Me.TextBoxSerieFacturaNewStock.Size = New System.Drawing.Size(49, 20)
        Me.TextBoxSerieFacturaNewStock.TabIndex = 17
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(5, 36)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(57, 13)
        Me.Label23.TabIndex = 16
        Me.Label23.Text = "NewStock"
        '
        'CheckBoxSerieFacturaPorDepartamento
        '
        Me.CheckBoxSerieFacturaPorDepartamento.AutoSize = True
        Me.CheckBoxSerieFacturaPorDepartamento.Location = New System.Drawing.Point(183, 38)
        Me.CheckBoxSerieFacturaPorDepartamento.Name = "CheckBoxSerieFacturaPorDepartamento"
        Me.CheckBoxSerieFacturaPorDepartamento.Size = New System.Drawing.Size(296, 17)
        Me.CheckBoxSerieFacturaPorDepartamento.TabIndex = 1
        Me.CheckBoxSerieFacturaPorDepartamento.Text = "Añadir a  Series Genéricas Código Externo Departamento"
        Me.CheckBoxSerieFacturaPorDepartamento.UseVisualStyleBackColor = True
        '
        'CheckBoxBuscaCuentasNewCentral
        '
        Me.CheckBoxBuscaCuentasNewCentral.AutoSize = True
        Me.CheckBoxBuscaCuentasNewCentral.Location = New System.Drawing.Point(16, 172)
        Me.CheckBoxBuscaCuentasNewCentral.Name = "CheckBoxBuscaCuentasNewCentral"
        Me.CheckBoxBuscaCuentasNewCentral.Size = New System.Drawing.Size(395, 17)
        Me.CheckBoxBuscaCuentasNewCentral.TabIndex = 20
        Me.CheckBoxBuscaCuentasNewCentral.Text = "Cuentas de Gastos en Newcentral( Evita Conexiones al Hotel Cliente Servidor)"
        Me.CheckBoxBuscaCuentasNewCentral.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(400, 43)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(216, 13)
        Me.Label9.TabIndex = 19
        Me.Label9.Text = "+ c.Costo Almacén BD + Grupo Artìculos BD"
        '
        'TextBoxCtaCentro4
        '
        Me.TextBoxCtaCentro4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaCentro4.Location = New System.Drawing.Point(365, 43)
        Me.TextBoxCtaCentro4.Name = "TextBoxCtaCentro4"
        Me.TextBoxCtaCentro4.Size = New System.Drawing.Size(29, 20)
        Me.TextBoxCtaCentro4.TabIndex = 18
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(290, 43)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(69, 13)
        Me.Label12.TabIndex = 17
        Me.Label12.Text = "Centro(Hotel)"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(97, 43)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(16, 13)
        Me.Label13.TabIndex = 16
        Me.Label13.Text = "+ "
        '
        'TextBoxCtaRaiz4
        '
        Me.TextBoxCtaRaiz4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaRaiz4.Location = New System.Drawing.Point(42, 43)
        Me.TextBoxCtaRaiz4.Name = "TextBoxCtaRaiz4"
        Me.TextBoxCtaRaiz4.Size = New System.Drawing.Size(49, 20)
        Me.TextBoxCtaRaiz4.TabIndex = 15
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(10, 43)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(30, 13)
        Me.Label14.TabIndex = 14
        Me.Label14.Text = "Raíz"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(278, 43)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(16, 13)
        Me.Label15.TabIndex = 13
        Me.Label15.Text = "+ "
        '
        'TextBoxCtaActividad4
        '
        Me.TextBoxCtaActividad4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaActividad4.Location = New System.Drawing.Point(234, 43)
        Me.TextBoxCtaActividad4.Name = "TextBoxCtaActividad4"
        Me.TextBoxCtaActividad4.Size = New System.Drawing.Size(38, 20)
        Me.TextBoxCtaActividad4.TabIndex = 12
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(110, 43)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(124, 13)
        Me.Label16.TabIndex = 11
        Me.Label16.Text = "Actividad de Explotación"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Maroon
        Me.Label8.Location = New System.Drawing.Point(10, 18)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(217, 13)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "Composición de Cuentas de Gasto (Grupo 4)"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(400, 110)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(216, 13)
        Me.Label7.TabIndex = 9
        Me.Label7.Text = "+ c.Costo Almacén BD + Grupo Artìculos BD"
        '
        'TextBoxCtaCentro6
        '
        Me.TextBoxCtaCentro6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaCentro6.Location = New System.Drawing.Point(365, 110)
        Me.TextBoxCtaCentro6.Name = "TextBoxCtaCentro6"
        Me.TextBoxCtaCentro6.Size = New System.Drawing.Size(29, 20)
        Me.TextBoxCtaCentro6.TabIndex = 8
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(290, 110)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(69, 13)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "Centro(Hotel)"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(97, 110)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(16, 13)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "+ "
        '
        'TextBoxCtaRaiz6
        '
        Me.TextBoxCtaRaiz6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaRaiz6.Location = New System.Drawing.Point(42, 110)
        Me.TextBoxCtaRaiz6.Name = "TextBoxCtaRaiz6"
        Me.TextBoxCtaRaiz6.Size = New System.Drawing.Size(49, 20)
        Me.TextBoxCtaRaiz6.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(10, 110)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(30, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Raíz"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(278, 110)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(16, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "+ "
        '
        'TextBoxCtaActividad6
        '
        Me.TextBoxCtaActividad6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaActividad6.Location = New System.Drawing.Point(234, 110)
        Me.TextBoxCtaActividad6.Name = "TextBoxCtaActividad6"
        Me.TextBoxCtaActividad6.Size = New System.Drawing.Size(38, 20)
        Me.TextBoxCtaActividad6.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(110, 110)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(124, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Actividad de Explotación"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Maroon
        Me.Label1.Location = New System.Drawing.Point(10, 81)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(217, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Composición de Cuentas de Gasto (Grupo 6)"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.ButtonAceptar)
        Me.GroupBox2.Controls.Add(Me.ButtonCancelar)
        Me.GroupBox2.Location = New System.Drawing.Point(779, 48)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(108, 474)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
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
        'ButtonCancelar
        '
        Me.ButtonCancelar.Location = New System.Drawing.Point(21, 59)
        Me.ButtonCancelar.Name = "ButtonCancelar"
        Me.ButtonCancelar.Size = New System.Drawing.Size(80, 23)
        Me.ButtonCancelar.TabIndex = 13
        Me.ButtonCancelar.Text = "&Cancelar"
        '
        'ComboBoxEmpCod
        '
        Me.ComboBoxEmpCod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxEmpCod.DropDownWidth = 250
        Me.ComboBoxEmpCod.Location = New System.Drawing.Point(272, 9)
        Me.ComboBoxEmpCod.Name = "ComboBoxEmpCod"
        Me.ComboBoxEmpCod.Size = New System.Drawing.Size(208, 21)
        Me.ComboBoxEmpCod.TabIndex = 9
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(208, 9)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(64, 23)
        Me.Label11.TabIndex = 8
        Me.Label11.Text = "Emp. cod"
        '
        'ComboBoxGrupoCod
        '
        Me.ComboBoxGrupoCod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxGrupoCod.Location = New System.Drawing.Point(80, 9)
        Me.ComboBoxGrupoCod.Name = "ComboBoxGrupoCod"
        Me.ComboBoxGrupoCod.Size = New System.Drawing.Size(121, 21)
        Me.ComboBoxGrupoCod.TabIndex = 7
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(16, 9)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(64, 23)
        Me.Label10.TabIndex = 6
        Me.Label10.Text = "Grupo cod"
        '
        'TextBoxEmpCod
        '
        Me.TextBoxEmpCod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxEmpCod.Enabled = False
        Me.TextBoxEmpCod.Location = New System.Drawing.Point(562, 11)
        Me.TextBoxEmpCod.Name = "TextBoxEmpCod"
        Me.TextBoxEmpCod.Size = New System.Drawing.Size(32, 20)
        Me.TextBoxEmpCod.TabIndex = 39
        '
        'Label78
        '
        Me.Label78.AutoSize = True
        Me.Label78.Location = New System.Drawing.Point(500, 9)
        Me.Label78.Name = "Label78"
        Me.Label78.Size = New System.Drawing.Size(56, 13)
        Me.Label78.TabIndex = 38
        Me.Label78.Text = "Emp. Cod."
        '
        'TextBoxEmpNum
        '
        Me.TextBoxEmpNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxEmpNum.Enabled = False
        Me.TextBoxEmpNum.Location = New System.Drawing.Point(688, 11)
        Me.TextBoxEmpNum.Name = "TextBoxEmpNum"
        Me.TextBoxEmpNum.Size = New System.Drawing.Size(46, 20)
        Me.TextBoxEmpNum.TabIndex = 37
        '
        'Label77
        '
        Me.Label77.AutoSize = True
        Me.Label77.Location = New System.Drawing.Point(610, 9)
        Me.Label77.Name = "Label77"
        Me.Label77.Size = New System.Drawing.Size(72, 13)
        Me.Label77.TabIndex = 36
        Me.Label77.Text = "Hotel Nùmero"
        '
        'FormParametrosAlmacen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(899, 573)
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
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "FormParametrosAlmacen"
        Me.Text = "Parámetros Integración Almacén"
        Me.GroupBox1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPageGenerales.ResumeLayout(False)
        Me.TabPageGenerales.PerformLayout()
        Me.TabPageHotelesLopez.ResumeLayout(False)
        Me.TabPageHotelesLopez.PerformLayout()
        Me.GroupBoxSeriesdefactura.ResumeLayout(False)
        Me.GroupBoxSeriesdefactura.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBoxEmpCod As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxGrupoCod As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPageGenerales As System.Windows.Forms.TabPage
    Friend WithEvents TabPageHotelesLopez As System.Windows.Forms.TabPage
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaRaiz6 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaActividad6 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaCentro6 As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TextBoxEmpCod As System.Windows.Forms.TextBox
    Friend WithEvents Label78 As System.Windows.Forms.Label
    Friend WithEvents TextBoxEmpNum As System.Windows.Forms.TextBox
    Friend WithEvents Label77 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonCancelar As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaCentro4 As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaRaiz4 As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaActividad4 As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents CheckBoxUsaNewPaga As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxSoloFacturas As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxBuscaCuentasNewCentral As System.Windows.Forms.CheckBox
    Friend WithEvents TextBoxHaberAbonos As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxDebeAbonos As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxHaberFac As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxDebeFac As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents TextBoxHotelId As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents GroupBoxSeriesdefactura As GroupBox
    Friend WithEvents Label26 As Label
    Friend WithEvents TextBoxSerieFacturaNewPaga As TextBox
    Friend WithEvents Label24 As Label
    Friend WithEvents TextBoxSerieFacturaNewStock As TextBox
    Friend WithEvents Label23 As Label
    Friend WithEvents CheckBoxSerieFacturaPorDepartamento As CheckBox
    Friend WithEvents TextBoxSerieFacturaPorDepartamentos As TextBox
    Friend WithEvents Label27 As Label
    Friend WithEvents CheckBoxSerieFacturaDigitosAnio As CheckBox
    Friend WithEvents Label25 As Label
End Class
