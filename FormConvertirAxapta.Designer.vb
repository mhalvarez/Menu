<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormConvertirAxapta
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormConvertirAxapta))
        Me.ButtonAceptar = New System.Windows.Forms.Button()
        Me.ListBoxDebug = New System.Windows.Forms.ListBox()
        Me.DataGridViewAsientos = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStripAx = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemCambiarEstado = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemCambiarEstadoEntodalaFactura = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemCambiarEstado2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemCambiarEstado2EntodalaFactura = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemCambiarEstado3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemCambiarEstado4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemExluir = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemCambiarPrefijo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.UpdateImporteDelCobto001ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.CheckBoxEnviaProduccion = New System.Windows.Forms.CheckBox()
        Me.CheckBoxEnviaFacturacion = New System.Windows.Forms.CheckBox()
        Me.TextBoxDebug = New System.Windows.Forms.TextBox()
        Me.ButtonImPrimir = New System.Windows.Forms.Button()
        Me.CheckBoxRedondeaFactura = New System.Windows.Forms.CheckBox()
        Me.CheckBoxSoloPendientes = New System.Windows.Forms.CheckBox()
        Me.CheckBoxEnviaAnticipos = New System.Windows.Forms.CheckBox()
        Me.CheckBoxEnviaCobros = New System.Windows.Forms.CheckBox()
        Me.CheckBoxEnviaInventario = New System.Windows.Forms.CheckBox()
        Me.CheckBoxEstadistica = New System.Windows.Forms.CheckBox()
        Me.TreeViewDebug = New System.Windows.Forms.TreeView()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.ButtonAuditaNif = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.CheckBoxTimeOut = New System.Windows.Forms.CheckBox()
        Me.NumericUpDownWebServiceTimeOut = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBoxRutaWebServices = New System.Windows.Forms.TextBox()
        Me.CheckBoxRedondeaFacturaTest = New System.Windows.Forms.CheckBox()
        Me.CheckBoxExcluidos = New System.Windows.Forms.CheckBox()
        Me.CheckBoxVerTodos = New System.Windows.Forms.CheckBox()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.CheckBoxModificaNif = New System.Windows.Forms.CheckBox()
        Me.ToolTipAx = New System.Windows.Forms.ToolTip(Me.components)
        Me.ButtonImprimeEnviosPendientes = New System.Windows.Forms.Button()
        Me.ButtonExtractoBookId = New System.Windows.Forms.Button()
        Me.ButtonImprimeExclusiones = New System.Windows.Forms.Button()
        Me.ButtonJobCambiaFormadePago = New System.Windows.Forms.Button()
        Me.CheckBoxPat40 = New System.Windows.Forms.CheckBox()
        Me.ButtonCopy = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBoxFactura = New System.Windows.Forms.TextBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ButtonGoma = New System.Windows.Forms.Button()
        Me.CheckBoxBoockId = New System.Windows.Forms.CheckBox()
        Me.CheckBoxDebugCobros = New System.Windows.Forms.CheckBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.NumericUpDownredondeoBaseAjuste = New System.Windows.Forms.NumericUpDown()
        Me.CheckBoxPideBase = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TabControlDebug = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.ListBoxAjustesFacturas = New System.Windows.Forms.ListBox()
        CType(Me.DataGridViewAsientos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStripAx.SuspendLayout()
        CType(Me.NumericUpDownWebServiceTimeOut, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.NumericUpDownredondeoBaseAjuste, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlDebug.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Maroon
        Me.ButtonAceptar.ForeColor = System.Drawing.Color.White
        Me.ButtonAceptar.Location = New System.Drawing.Point(830, 16)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(92, 23)
        Me.ButtonAceptar.TabIndex = 0
        Me.ButtonAceptar.Text = "&Aceptar"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'ListBoxDebug
        '
        Me.ListBoxDebug.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListBoxDebug.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBoxDebug.FormattingEnabled = True
        Me.ListBoxDebug.HorizontalExtent = 4000
        Me.ListBoxDebug.HorizontalScrollbar = True
        Me.ListBoxDebug.ItemHeight = 14
        Me.ListBoxDebug.Location = New System.Drawing.Point(6, 14)
        Me.ListBoxDebug.Name = "ListBoxDebug"
        Me.ListBoxDebug.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.ListBoxDebug.Size = New System.Drawing.Size(713, 170)
        Me.ListBoxDebug.TabIndex = 1
        '
        'DataGridViewAsientos
        '
        Me.DataGridViewAsientos.AllowUserToAddRows = False
        Me.DataGridViewAsientos.AllowUserToDeleteRows = False
        Me.DataGridViewAsientos.AllowUserToResizeRows = False
        Me.DataGridViewAsientos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridViewAsientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewAsientos.ContextMenuStrip = Me.ContextMenuStripAx
        Me.DataGridViewAsientos.Location = New System.Drawing.Point(16, 32)
        Me.DataGridViewAsientos.MultiSelect = False
        Me.DataGridViewAsientos.Name = "DataGridViewAsientos"
        Me.DataGridViewAsientos.ReadOnly = True
        Me.DataGridViewAsientos.RowTemplate.Height = 15
        Me.DataGridViewAsientos.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewAsientos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridViewAsientos.Size = New System.Drawing.Size(746, 251)
        Me.DataGridViewAsientos.TabIndex = 2
        '
        'ContextMenuStripAx
        '
        Me.ContextMenuStripAx.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemCambiarEstado, Me.ToolStripMenuItemCambiarEstado2, Me.ToolStripMenuItemCambiarEstado3, Me.ToolStripMenuItemCambiarEstado4, Me.ToolStripMenuItemExluir, Me.ToolStripMenuItemCambiarPrefijo, Me.ToolStripMenuItem1, Me.UpdateImporteDelCobto001ToolStripMenuItem})
        Me.ContextMenuStripAx.Name = "ContextMenuStripAx"
        Me.ContextMenuStripAx.Size = New System.Drawing.Size(310, 164)
        '
        'ToolStripMenuItemCambiarEstado
        '
        Me.ToolStripMenuItemCambiarEstado.BackColor = System.Drawing.Color.Orange
        Me.ToolStripMenuItemCambiarEstado.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemCambiarEstadoEntodalaFactura})
        Me.ToolStripMenuItemCambiarEstado.Image = CType(resources.GetObject("ToolStripMenuItemCambiarEstado.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemCambiarEstado.Name = "ToolStripMenuItemCambiarEstado"
        Me.ToolStripMenuItemCambiarEstado.Size = New System.Drawing.Size(309, 22)
        Me.ToolStripMenuItemCambiarEstado.Text = "Omitido"
        '
        'ToolStripMenuItemCambiarEstadoEntodalaFactura
        '
        Me.ToolStripMenuItemCambiarEstadoEntodalaFactura.BackColor = System.Drawing.Color.Orange
        Me.ToolStripMenuItemCambiarEstadoEntodalaFactura.Name = "ToolStripMenuItemCambiarEstadoEntodalaFactura"
        Me.ToolStripMenuItemCambiarEstadoEntodalaFactura.Size = New System.Drawing.Size(287, 22)
        Me.ToolStripMenuItemCambiarEstadoEntodalaFactura.Text = "Omitir Todos los Cobros del Documento"
        '
        'ToolStripMenuItemCambiarEstado2
        '
        Me.ToolStripMenuItemCambiarEstado2.BackColor = System.Drawing.Color.LightBlue
        Me.ToolStripMenuItemCambiarEstado2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemCambiarEstado2EntodalaFactura})
        Me.ToolStripMenuItemCambiarEstado2.Image = CType(resources.GetObject("ToolStripMenuItemCambiarEstado2.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemCambiarEstado2.Name = "ToolStripMenuItemCambiarEstado2"
        Me.ToolStripMenuItemCambiarEstado2.Size = New System.Drawing.Size(309, 22)
        Me.ToolStripMenuItemCambiarEstado2.Text = "Corregido"
        '
        'ToolStripMenuItemCambiarEstado2EntodalaFactura
        '
        Me.ToolStripMenuItemCambiarEstado2EntodalaFactura.BackColor = System.Drawing.Color.LightBlue
        Me.ToolStripMenuItemCambiarEstado2EntodalaFactura.Name = "ToolStripMenuItemCambiarEstado2EntodalaFactura"
        Me.ToolStripMenuItemCambiarEstado2EntodalaFactura.Size = New System.Drawing.Size(296, 22)
        Me.ToolStripMenuItemCambiarEstado2EntodalaFactura.Text = "Corregir Todos los Cobros del Documento"
        '
        'ToolStripMenuItemCambiarEstado3
        '
        Me.ToolStripMenuItemCambiarEstado3.Image = CType(resources.GetObject("ToolStripMenuItemCambiarEstado3.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemCambiarEstado3.Name = "ToolStripMenuItemCambiarEstado3"
        Me.ToolStripMenuItemCambiarEstado3.Size = New System.Drawing.Size(309, 22)
        Me.ToolStripMenuItemCambiarEstado3.Text = "Actualizar el Número de Nif "
        '
        'ToolStripMenuItemCambiarEstado4
        '
        Me.ToolStripMenuItemCambiarEstado4.Image = CType(resources.GetObject("ToolStripMenuItemCambiarEstado4.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemCambiarEstado4.Name = "ToolStripMenuItemCambiarEstado4"
        Me.ToolStripMenuItemCambiarEstado4.Size = New System.Drawing.Size(309, 22)
        Me.ToolStripMenuItemCambiarEstado4.Text = "Marcar una Factura como ya Enviada"
        '
        'ToolStripMenuItemExluir
        '
        Me.ToolStripMenuItemExluir.BackColor = System.Drawing.Color.Salmon
        Me.ToolStripMenuItemExluir.Name = "ToolStripMenuItemExluir"
        Me.ToolStripMenuItemExluir.Size = New System.Drawing.Size(309, 22)
        Me.ToolStripMenuItemExluir.Text = "Excluir/Incluir"
        '
        'ToolStripMenuItemCambiarPrefijo
        '
        Me.ToolStripMenuItemCambiarPrefijo.Name = "ToolStripMenuItemCambiarPrefijo"
        Me.ToolStripMenuItemCambiarPrefijo.Size = New System.Drawing.Size(309, 22)
        Me.ToolStripMenuItemCambiarPrefijo.Text = "Cambiar Prefijo REC/NREC"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(306, 6)
        '
        'UpdateImporteDelCobto001ToolStripMenuItem
        '
        Me.UpdateImporteDelCobto001ToolStripMenuItem.Name = "UpdateImporteDelCobto001ToolStripMenuItem"
        Me.UpdateImporteDelCobto001ToolStripMenuItem.Size = New System.Drawing.Size(309, 22)
        Me.UpdateImporteDelCobto001ToolStripMenuItem.Text = "Update Importe del Cobro +-  0.01 Céntimos"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(786, 240)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(19, 23)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = ":::"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'CheckBoxEnviaProduccion
        '
        Me.CheckBoxEnviaProduccion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxEnviaProduccion.AutoSize = True
        Me.CheckBoxEnviaProduccion.Checked = True
        Me.CheckBoxEnviaProduccion.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxEnviaProduccion.Location = New System.Drawing.Point(830, 69)
        Me.CheckBoxEnviaProduccion.Name = "CheckBoxEnviaProduccion"
        Me.CheckBoxEnviaProduccion.Size = New System.Drawing.Size(80, 17)
        Me.CheckBoxEnviaProduccion.TabIndex = 8
        Me.CheckBoxEnviaProduccion.Text = "Producción"
        Me.CheckBoxEnviaProduccion.UseVisualStyleBackColor = True
        '
        'CheckBoxEnviaFacturacion
        '
        Me.CheckBoxEnviaFacturacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxEnviaFacturacion.AutoSize = True
        Me.CheckBoxEnviaFacturacion.Checked = True
        Me.CheckBoxEnviaFacturacion.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxEnviaFacturacion.Location = New System.Drawing.Point(830, 101)
        Me.CheckBoxEnviaFacturacion.Name = "CheckBoxEnviaFacturacion"
        Me.CheckBoxEnviaFacturacion.Size = New System.Drawing.Size(82, 17)
        Me.CheckBoxEnviaFacturacion.TabIndex = 9
        Me.CheckBoxEnviaFacturacion.Text = "Facturación"
        Me.CheckBoxEnviaFacturacion.UseVisualStyleBackColor = True
        '
        'TextBoxDebug
        '
        Me.TextBoxDebug.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDebug.Location = New System.Drawing.Point(32, 545)
        Me.TextBoxDebug.Name = "TextBoxDebug"
        Me.TextBoxDebug.Size = New System.Drawing.Size(142, 20)
        Me.TextBoxDebug.TabIndex = 10
        '
        'ButtonImPrimir
        '
        Me.ButtonImPrimir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonImPrimir.Location = New System.Drawing.Point(830, 40)
        Me.ButtonImPrimir.Name = "ButtonImPrimir"
        Me.ButtonImPrimir.Size = New System.Drawing.Size(92, 23)
        Me.ButtonImPrimir.TabIndex = 11
        Me.ButtonImPrimir.Text = "&Imprimir"
        Me.ButtonImPrimir.UseVisualStyleBackColor = True
        '
        'CheckBoxRedondeaFactura
        '
        Me.CheckBoxRedondeaFactura.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxRedondeaFactura.Checked = True
        Me.CheckBoxRedondeaFactura.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxRedondeaFactura.Enabled = False
        Me.CheckBoxRedondeaFactura.ForeColor = System.Drawing.Color.Maroon
        Me.CheckBoxRedondeaFactura.Location = New System.Drawing.Point(369, 283)
        Me.CheckBoxRedondeaFactura.Name = "CheckBoxRedondeaFactura"
        Me.CheckBoxRedondeaFactura.Size = New System.Drawing.Size(251, 26)
        Me.CheckBoxRedondeaFactura.TabIndex = 16
        Me.CheckBoxRedondeaFactura.Text = "Fuera de uso"
        Me.CheckBoxRedondeaFactura.UseVisualStyleBackColor = True
        '
        'CheckBoxSoloPendientes
        '
        Me.CheckBoxSoloPendientes.AutoSize = True
        Me.CheckBoxSoloPendientes.Location = New System.Drawing.Point(97, 9)
        Me.CheckBoxSoloPendientes.Name = "CheckBoxSoloPendientes"
        Me.CheckBoxSoloPendientes.Size = New System.Drawing.Size(194, 17)
        Me.CheckBoxSoloPendientes.TabIndex = 18
        Me.CheckBoxSoloPendientes.Text = "Solo Envios Pendientes y Excluidos"
        Me.CheckBoxSoloPendientes.UseVisualStyleBackColor = True
        '
        'CheckBoxEnviaAnticipos
        '
        Me.CheckBoxEnviaAnticipos.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxEnviaAnticipos.AutoSize = True
        Me.CheckBoxEnviaAnticipos.Checked = True
        Me.CheckBoxEnviaAnticipos.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxEnviaAnticipos.Location = New System.Drawing.Point(830, 85)
        Me.CheckBoxEnviaAnticipos.Name = "CheckBoxEnviaAnticipos"
        Me.CheckBoxEnviaAnticipos.Size = New System.Drawing.Size(69, 17)
        Me.CheckBoxEnviaAnticipos.TabIndex = 26
        Me.CheckBoxEnviaAnticipos.Text = "Anticipos"
        Me.CheckBoxEnviaAnticipos.UseVisualStyleBackColor = True
        '
        'CheckBoxEnviaCobros
        '
        Me.CheckBoxEnviaCobros.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxEnviaCobros.AutoSize = True
        Me.CheckBoxEnviaCobros.Checked = True
        Me.CheckBoxEnviaCobros.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxEnviaCobros.Location = New System.Drawing.Point(830, 117)
        Me.CheckBoxEnviaCobros.Name = "CheckBoxEnviaCobros"
        Me.CheckBoxEnviaCobros.Size = New System.Drawing.Size(59, 17)
        Me.CheckBoxEnviaCobros.TabIndex = 27
        Me.CheckBoxEnviaCobros.Text = "Cobros"
        Me.CheckBoxEnviaCobros.UseVisualStyleBackColor = True
        '
        'CheckBoxEnviaInventario
        '
        Me.CheckBoxEnviaInventario.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxEnviaInventario.AutoSize = True
        Me.CheckBoxEnviaInventario.Checked = True
        Me.CheckBoxEnviaInventario.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxEnviaInventario.Location = New System.Drawing.Point(830, 133)
        Me.CheckBoxEnviaInventario.Name = "CheckBoxEnviaInventario"
        Me.CheckBoxEnviaInventario.Size = New System.Drawing.Size(73, 17)
        Me.CheckBoxEnviaInventario.TabIndex = 28
        Me.CheckBoxEnviaInventario.Text = "Inventario"
        Me.CheckBoxEnviaInventario.UseVisualStyleBackColor = True
        '
        'CheckBoxEstadistica
        '
        Me.CheckBoxEstadistica.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxEstadistica.AutoSize = True
        Me.CheckBoxEstadistica.Checked = True
        Me.CheckBoxEstadistica.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxEstadistica.Location = New System.Drawing.Point(830, 149)
        Me.CheckBoxEstadistica.Name = "CheckBoxEstadistica"
        Me.CheckBoxEstadistica.Size = New System.Drawing.Size(84, 17)
        Me.CheckBoxEstadistica.TabIndex = 29
        Me.CheckBoxEstadistica.Text = "Estadísticas"
        Me.CheckBoxEstadistica.UseVisualStyleBackColor = True
        '
        'TreeViewDebug
        '
        Me.TreeViewDebug.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TreeViewDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TreeViewDebug.CheckBoxes = True
        Me.TreeViewDebug.Enabled = False
        Me.TreeViewDebug.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TreeViewDebug.Location = New System.Drawing.Point(12, 330)
        Me.TreeViewDebug.Name = "TreeViewDebug"
        Me.TreeViewDebug.Size = New System.Drawing.Size(14, 214)
        Me.TreeViewDebug.TabIndex = 30
        '
        'Button4
        '
        Me.Button4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button4.Location = New System.Drawing.Point(823, 172)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(104, 23)
        Me.Button4.TabIndex = 31
        Me.Button4.Text = "&Imprimir Errores"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'ButtonAuditaNif
        '
        Me.ButtonAuditaNif.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonAuditaNif.Location = New System.Drawing.Point(9, 14)
        Me.ButtonAuditaNif.Name = "ButtonAuditaNif"
        Me.ButtonAuditaNif.Size = New System.Drawing.Size(117, 23)
        Me.ButtonAuditaNif.TabIndex = 32
        Me.ButtonAuditaNif.Text = "Audita Nifs"
        Me.ToolTipAx.SetToolTip(Me.ButtonAuditaNif, "Audita que no existan Nifs Nulos o en Zero")
        Me.ButtonAuditaNif.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button5.Location = New System.Drawing.Point(806, 508)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(114, 23)
        Me.Button5.TabIndex = 34
        Me.Button5.Text = "&Imprimir(Debug)"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'CheckBoxTimeOut
        '
        Me.CheckBoxTimeOut.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxTimeOut.AutoSize = True
        Me.CheckBoxTimeOut.Checked = True
        Me.CheckBoxTimeOut.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxTimeOut.Location = New System.Drawing.Point(907, 546)
        Me.CheckBoxTimeOut.Name = "CheckBoxTimeOut"
        Me.CheckBoxTimeOut.Size = New System.Drawing.Size(15, 14)
        Me.CheckBoxTimeOut.TabIndex = 37
        Me.CheckBoxTimeOut.UseVisualStyleBackColor = True
        '
        'NumericUpDownWebServiceTimeOut
        '
        Me.NumericUpDownWebServiceTimeOut.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NumericUpDownWebServiceTimeOut.Location = New System.Drawing.Point(806, 546)
        Me.NumericUpDownWebServiceTimeOut.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.NumericUpDownWebServiceTimeOut.Name = "NumericUpDownWebServiceTimeOut"
        Me.NumericUpDownWebServiceTimeOut.Size = New System.Drawing.Size(93, 20)
        Me.NumericUpDownWebServiceTimeOut.TabIndex = 36
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(736, 548)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 13)
        Me.Label2.TabIndex = 35
        Me.Label2.Text = "Time Out"
        '
        'TextBoxRutaWebServices
        '
        Me.TextBoxRutaWebServices.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxRutaWebServices.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxRutaWebServices.Location = New System.Drawing.Point(180, 545)
        Me.TextBoxRutaWebServices.Name = "TextBoxRutaWebServices"
        Me.TextBoxRutaWebServices.Size = New System.Drawing.Size(440, 20)
        Me.TextBoxRutaWebServices.TabIndex = 4
        '
        'CheckBoxRedondeaFacturaTest
        '
        Me.CheckBoxRedondeaFacturaTest.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxRedondeaFacturaTest.AutoSize = True
        Me.CheckBoxRedondeaFacturaTest.ForeColor = System.Drawing.Color.Maroon
        Me.CheckBoxRedondeaFacturaTest.Location = New System.Drawing.Point(768, 403)
        Me.CheckBoxRedondeaFacturaTest.Name = "CheckBoxRedondeaFacturaTest"
        Me.CheckBoxRedondeaFacturaTest.Size = New System.Drawing.Size(147, 17)
        Me.CheckBoxRedondeaFacturaTest.TabIndex = 38
        Me.CheckBoxRedondeaFacturaTest.Text = "Redondea Facturas(Test)"
        Me.CheckBoxRedondeaFacturaTest.UseVisualStyleBackColor = True
        Me.CheckBoxRedondeaFacturaTest.Visible = False
        '
        'CheckBoxExcluidos
        '
        Me.CheckBoxExcluidos.AutoSize = True
        Me.CheckBoxExcluidos.Location = New System.Drawing.Point(297, 9)
        Me.CheckBoxExcluidos.Name = "CheckBoxExcluidos"
        Me.CheckBoxExcluidos.Size = New System.Drawing.Size(95, 17)
        Me.CheckBoxExcluidos.TabIndex = 39
        Me.CheckBoxExcluidos.Text = "Solo Excluidos"
        Me.CheckBoxExcluidos.UseVisualStyleBackColor = True
        '
        'CheckBoxVerTodos
        '
        Me.CheckBoxVerTodos.AutoSize = True
        Me.CheckBoxVerTodos.Location = New System.Drawing.Point(16, 9)
        Me.CheckBoxVerTodos.Name = "CheckBoxVerTodos"
        Me.CheckBoxVerTodos.Size = New System.Drawing.Size(75, 17)
        Me.CheckBoxVerTodos.TabIndex = 40
        Me.CheckBoxVerTodos.Text = "Ver Todos"
        Me.CheckBoxVerTodos.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button6.Location = New System.Drawing.Point(9, 41)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(117, 25)
        Me.Button6.TabIndex = 41
        Me.Button6.Text = "Actualiza Nifs"
        Me.ToolTipAx.SetToolTip(Me.Button6, "Actualiza los Nifs de las Facturas caso de No ser coincidentes con los Valores Ac" &
        "tuales de la Tabla de Entidades")
        Me.Button6.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.CheckBoxModificaNif)
        Me.GroupBox1.Controls.Add(Me.ButtonAuditaNif)
        Me.GroupBox1.Controls.Add(Me.Button6)
        Me.GroupBox1.Location = New System.Drawing.Point(786, 269)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(136, 94)
        Me.GroupBox1.TabIndex = 42
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Nif"
        '
        'CheckBoxModificaNif
        '
        Me.CheckBoxModificaNif.AutoSize = True
        Me.CheckBoxModificaNif.Location = New System.Drawing.Point(9, 73)
        Me.CheckBoxModificaNif.Name = "CheckBoxModificaNif"
        Me.CheckBoxModificaNif.Size = New System.Drawing.Size(123, 17)
        Me.CheckBoxModificaNif.TabIndex = 42
        Me.CheckBoxModificaNif.Text = "Permite Modificar Nif"
        Me.ToolTipAx.SetToolTip(Me.CheckBoxModificaNif, "Permite Modificar Manuamente el Nif de una Factura (Registros No enviados)")
        Me.CheckBoxModificaNif.UseVisualStyleBackColor = True
        '
        'ToolTipAx
        '
        Me.ToolTipAx.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.ToolTipAx.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.ToolTipAx.IsBalloon = True
        Me.ToolTipAx.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
        '
        'ButtonImprimeEnviosPendientes
        '
        Me.ButtonImprimeEnviosPendientes.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonImprimeEnviosPendientes.Location = New System.Drawing.Point(823, 217)
        Me.ButtonImprimeEnviosPendientes.Name = "ButtonImprimeEnviosPendientes"
        Me.ButtonImprimeEnviosPendientes.Size = New System.Drawing.Size(104, 23)
        Me.ButtonImprimeEnviosPendientes.TabIndex = 44
        Me.ButtonImprimeEnviosPendientes.Text = "Envios Pdtes."
        Me.ToolTipAx.SetToolTip(Me.ButtonImprimeEnviosPendientes, "Envios Pendientes No Excluidos")
        Me.ButtonImprimeEnviosPendientes.UseVisualStyleBackColor = True
        '
        'ButtonExtractoBookId
        '
        Me.ButtonExtractoBookId.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonExtractoBookId.Location = New System.Drawing.Point(823, 240)
        Me.ButtonExtractoBookId.Name = "ButtonExtractoBookId"
        Me.ButtonExtractoBookId.Size = New System.Drawing.Size(104, 23)
        Me.ButtonExtractoBookId.TabIndex = 45
        Me.ButtonExtractoBookId.Text = "Extracto Book Ids"
        Me.ToolTipAx.SetToolTip(Me.ButtonExtractoBookId, "Extracto de Recepción / Devolución y Facturación de Anticipos por Reserva")
        Me.ButtonExtractoBookId.UseVisualStyleBackColor = True
        '
        'ButtonImprimeExclusiones
        '
        Me.ButtonImprimeExclusiones.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonImprimeExclusiones.Location = New System.Drawing.Point(823, 194)
        Me.ButtonImprimeExclusiones.Name = "ButtonImprimeExclusiones"
        Me.ButtonImprimeExclusiones.Size = New System.Drawing.Size(104, 23)
        Me.ButtonImprimeExclusiones.TabIndex = 43
        Me.ButtonImprimeExclusiones.Text = "Imprimir &Excluidos"
        Me.ToolTipAx.SetToolTip(Me.ButtonImprimeExclusiones, "Excluidos / Omitidos")
        Me.ButtonImprimeExclusiones.UseVisualStyleBackColor = True
        '
        'ButtonJobCambiaFormadePago
        '
        Me.ButtonJobCambiaFormadePago.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonJobCambiaFormadePago.Location = New System.Drawing.Point(62, 293)
        Me.ButtonJobCambiaFormadePago.Name = "ButtonJobCambiaFormadePago"
        Me.ButtonJobCambiaFormadePago.Size = New System.Drawing.Size(112, 23)
        Me.ButtonJobCambiaFormadePago.TabIndex = 54
        Me.ButtonJobCambiaFormadePago.Text = "Formas de Pago"
        Me.ToolTipAx.SetToolTip(Me.ButtonJobCambiaFormadePago, "Cambia la Forma de Pago del Cobro/Devolución  a la Forma de Pago del Anticipo")
        Me.ButtonJobCambiaFormadePago.UseVisualStyleBackColor = True
        '
        'CheckBoxPat40
        '
        Me.CheckBoxPat40.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxPat40.AutoSize = True
        Me.CheckBoxPat40.Location = New System.Drawing.Point(198, 292)
        Me.CheckBoxPat40.Name = "CheckBoxPat40"
        Me.CheckBoxPat40.Size = New System.Drawing.Size(165, 17)
        Me.CheckBoxPat40.TabIndex = 56
        Me.CheckBoxPat40.Text = "No Use PAT 40 (Redondeos)"
        Me.ToolTipAx.SetToolTip(Me.CheckBoxPat40, "Cambia la Forma de Simular el Cálculo de Impuestos en Axapta")
        Me.CheckBoxPat40.UseVisualStyleBackColor = True
        '
        'ButtonCopy
        '
        Me.ButtonCopy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonCopy.Location = New System.Drawing.Point(774, 373)
        Me.ButtonCopy.Name = "ButtonCopy"
        Me.ButtonCopy.Size = New System.Drawing.Size(50, 23)
        Me.ButtonCopy.TabIndex = 62
        Me.ButtonCopy.Text = ":::"
        Me.ToolTipAx.SetToolTip(Me.ButtonCopy, "Copiar al Portapapeles")
        Me.ButtonCopy.UseVisualStyleBackColor = True
        Me.ButtonCopy.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(398, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(52, 13)
        Me.Label1.TabIndex = 46
        Me.Label1.Text = "Factura ="
        '
        'TextBoxFactura
        '
        Me.TextBoxFactura.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxFactura.Location = New System.Drawing.Point(447, 9)
        Me.TextBoxFactura.Name = "TextBoxFactura"
        Me.TextBoxFactura.Size = New System.Drawing.Size(56, 20)
        Me.TextBoxFactura.TabIndex = 47
        '
        'TextBox4
        '
        Me.TextBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox4.Enabled = False
        Me.TextBox4.Location = New System.Drawing.Point(553, 9)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(56, 20)
        Me.TextBox4.TabIndex = 49
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(504, 11)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 13)
        Me.Label3.TabIndex = 48
        Me.Label3.Text = "Boock ="
        '
        'ButtonGoma
        '
        Me.ButtonGoma.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonGoma.Image = Global.Menu.My.Resources.ResourceFondos.clear
        Me.ButtonGoma.Location = New System.Drawing.Point(715, 5)
        Me.ButtonGoma.Name = "ButtonGoma"
        Me.ButtonGoma.Size = New System.Drawing.Size(47, 23)
        Me.ButtonGoma.TabIndex = 50
        Me.ButtonGoma.UseVisualStyleBackColor = True
        '
        'CheckBoxBoockId
        '
        Me.CheckBoxBoockId.AutoSize = True
        Me.CheckBoxBoockId.Location = New System.Drawing.Point(615, 10)
        Me.CheckBoxBoockId.Name = "CheckBoxBoockId"
        Me.CheckBoxBoockId.Size = New System.Drawing.Size(90, 17)
        Me.CheckBoxBoockId.TabIndex = 51
        Me.CheckBoxBoockId.Text = "con Boock Id"
        Me.CheckBoxBoockId.UseVisualStyleBackColor = True
        '
        'CheckBoxDebugCobros
        '
        Me.CheckBoxDebugCobros.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxDebugCobros.AutoSize = True
        Me.CheckBoxDebugCobros.Enabled = False
        Me.CheckBoxDebugCobros.Location = New System.Drawing.Point(766, 117)
        Me.CheckBoxDebugCobros.Name = "CheckBoxDebugCobros"
        Me.CheckBoxDebugCobros.Size = New System.Drawing.Size(58, 17)
        Me.CheckBoxDebugCobros.TabIndex = 52
        Me.CheckBoxDebugCobros.Text = "Debug"
        Me.CheckBoxDebugCobros.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(13, 293)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(38, 13)
        Me.Label7.TabIndex = 53
        Me.Label7.Text = "Jobs ="
        '
        'ListBox1
        '
        Me.ListBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.HorizontalScrollbar = True
        Me.ListBox1.Location = New System.Drawing.Point(786, 423)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(136, 80)
        Me.ListBox1.TabIndex = 57
        '
        'NumericUpDownredondeoBaseAjuste
        '
        Me.NumericUpDownredondeoBaseAjuste.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.NumericUpDownredondeoBaseAjuste.DecimalPlaces = 2
        Me.NumericUpDownredondeoBaseAjuste.Enabled = False
        Me.NumericUpDownredondeoBaseAjuste.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.NumericUpDownredondeoBaseAjuste.Location = New System.Drawing.Point(650, 296)
        Me.NumericUpDownredondeoBaseAjuste.Maximum = New Decimal(New Integer() {3, 0, 0, 131072})
        Me.NumericUpDownredondeoBaseAjuste.Minimum = New Decimal(New Integer() {3, 0, 0, -2147352576})
        Me.NumericUpDownredondeoBaseAjuste.Name = "NumericUpDownredondeoBaseAjuste"
        Me.NumericUpDownredondeoBaseAjuste.Size = New System.Drawing.Size(78, 20)
        Me.NumericUpDownredondeoBaseAjuste.TabIndex = 58
        '
        'CheckBoxPideBase
        '
        Me.CheckBoxPideBase.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxPideBase.AutoSize = True
        Me.CheckBoxPideBase.Location = New System.Drawing.Point(369, 310)
        Me.CheckBoxPideBase.Name = "CheckBoxPideBase"
        Me.CheckBoxPideBase.Size = New System.Drawing.Size(74, 17)
        Me.CheckBoxPideBase.TabIndex = 59
        Me.CheckBoxPideBase.Text = "Pide Base"
        Me.CheckBoxPideBase.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(870, 377)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(11, 13)
        Me.Label4.TabIndex = 60
        Me.Label4.Text = "*"
        '
        'TabControlDebug
        '
        Me.TabControlDebug.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControlDebug.Controls.Add(Me.TabPage1)
        Me.TabControlDebug.Controls.Add(Me.TabPage2)
        Me.TabControlDebug.Location = New System.Drawing.Point(32, 330)
        Me.TabControlDebug.Name = "TabControlDebug"
        Me.TabControlDebug.SelectedIndex = 0
        Me.TabControlDebug.Size = New System.Drawing.Size(730, 214)
        Me.TabControlDebug.TabIndex = 61
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.ListBoxDebug)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(722, 188)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Log Envios"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.ListBoxAjustesFacturas)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(722, 188)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Log Ajustes Facturas"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'ListBoxAjustesFacturas
        '
        Me.ListBoxAjustesFacturas.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxAjustesFacturas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListBoxAjustesFacturas.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBoxAjustesFacturas.FormattingEnabled = True
        Me.ListBoxAjustesFacturas.HorizontalExtent = 4000
        Me.ListBoxAjustesFacturas.HorizontalScrollbar = True
        Me.ListBoxAjustesFacturas.ItemHeight = 14
        Me.ListBoxAjustesFacturas.Location = New System.Drawing.Point(6, 14)
        Me.ListBoxAjustesFacturas.Name = "ListBoxAjustesFacturas"
        Me.ListBoxAjustesFacturas.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.ListBoxAjustesFacturas.Size = New System.Drawing.Size(713, 170)
        Me.ListBoxAjustesFacturas.TabIndex = 2
        '
        'FormConvertirAxapta
        '
        Me.AcceptButton = Me.ButtonAceptar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(928, 573)
        Me.Controls.Add(Me.ButtonCopy)
        Me.Controls.Add(Me.TabControlDebug)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.CheckBoxPideBase)
        Me.Controls.Add(Me.NumericUpDownredondeoBaseAjuste)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.CheckBoxPat40)
        Me.Controls.Add(Me.ButtonJobCambiaFormadePago)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.CheckBoxDebugCobros)
        Me.Controls.Add(Me.CheckBoxBoockId)
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.ButtonGoma)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TextBoxFactura)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ButtonExtractoBookId)
        Me.Controls.Add(Me.ButtonImprimeEnviosPendientes)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ButtonImprimeExclusiones)
        Me.Controls.Add(Me.CheckBoxVerTodos)
        Me.Controls.Add(Me.CheckBoxExcluidos)
        Me.Controls.Add(Me.CheckBoxTimeOut)
        Me.Controls.Add(Me.NumericUpDownWebServiceTimeOut)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.TreeViewDebug)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.CheckBoxRedondeaFacturaTest)
        Me.Controls.Add(Me.CheckBoxEstadistica)
        Me.Controls.Add(Me.CheckBoxEnviaInventario)
        Me.Controls.Add(Me.CheckBoxEnviaCobros)
        Me.Controls.Add(Me.CheckBoxEnviaAnticipos)
        Me.Controls.Add(Me.CheckBoxSoloPendientes)
        Me.Controls.Add(Me.ButtonImPrimir)
        Me.Controls.Add(Me.TextBoxDebug)
        Me.Controls.Add(Me.CheckBoxEnviaFacturacion)
        Me.Controls.Add(Me.CheckBoxEnviaProduccion)
        Me.Controls.Add(Me.TextBoxRutaWebServices)
        Me.Controls.Add(Me.CheckBoxRedondeaFactura)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.DataGridViewAsientos)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "FormConvertirAxapta"
        Me.Text = "Axapta Web Interface"
        CType(Me.DataGridViewAsientos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStripAx.ResumeLayout(False)
        CType(Me.NumericUpDownWebServiceTimeOut, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.NumericUpDownredondeoBaseAjuste, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlDebug.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ListBoxDebug As System.Windows.Forms.ListBox
    Friend WithEvents DataGridViewAsientos As System.Windows.Forms.DataGridView
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents CheckBoxEnviaProduccion As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxEnviaFacturacion As System.Windows.Forms.CheckBox
    Friend WithEvents TextBoxDebug As System.Windows.Forms.TextBox
    Friend WithEvents ButtonImPrimir As System.Windows.Forms.Button
    Friend WithEvents CheckBoxRedondeaFactura As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxSoloPendientes As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxEnviaAnticipos As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxEnviaCobros As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxEnviaInventario As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxEstadistica As System.Windows.Forms.CheckBox
    Friend WithEvents TreeViewDebug As System.Windows.Forms.TreeView
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents ButtonAuditaNif As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents CheckBoxTimeOut As System.Windows.Forms.CheckBox
    Friend WithEvents NumericUpDownWebServiceTimeOut As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBoxRutaWebServices As System.Windows.Forms.TextBox
    Friend WithEvents ContextMenuStripAx As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItemCambiarEstado As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CheckBoxRedondeaFacturaTest As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxExcluidos As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxVerTodos As System.Windows.Forms.CheckBox
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents ToolStripMenuItemCambiarEstado2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemCambiarEstado3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxModificaNif As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTipAx As System.Windows.Forms.ToolTip
    Friend WithEvents ButtonImprimeExclusiones As System.Windows.Forms.Button
    Friend WithEvents ButtonImprimeEnviosPendientes As System.Windows.Forms.Button
    Friend WithEvents ButtonExtractoBookId As System.Windows.Forms.Button
    Friend WithEvents ToolStripMenuItemCambiarEstadoEntodalaFactura As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemCambiarEstado2EntodalaFactura As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemCambiarEstado4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBoxFactura As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ButtonGoma As System.Windows.Forms.Button
    Friend WithEvents CheckBoxBoockId As System.Windows.Forms.CheckBox
    Friend WithEvents ToolStripMenuItemExluir As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemCambiarPrefijo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CheckBoxDebugCobros As System.Windows.Forms.CheckBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ButtonJobCambiaFormadePago As System.Windows.Forms.Button
    Friend WithEvents CheckBoxPat40 As System.Windows.Forms.CheckBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents NumericUpDownredondeoBaseAjuste As System.Windows.Forms.NumericUpDown
    Friend WithEvents CheckBoxPideBase As System.Windows.Forms.CheckBox
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents UpdateImporteDelCobto001ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TabControlDebug As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents ListBoxAjustesFacturas As System.Windows.Forms.ListBox
    Friend WithEvents ButtonCopy As System.Windows.Forms.Button
End Class
