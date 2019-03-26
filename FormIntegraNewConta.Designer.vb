<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormIntegraNewConta
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormIntegraNewConta))
        Me.ButtonAceptar = New System.Windows.Forms.Button()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.DataGridHoteles = New System.Windows.Forms.DataGrid()
        Me.TextBoxRutaFicheros = New System.Windows.Forms.TextBox()
        Me.CheckBoxDebug = New System.Windows.Forms.CheckBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.TextBoxDebug = New System.Windows.Forms.TextBox()
        Me.ListBoxDebug = New System.Windows.Forms.ListBox()
        Me.ButtonImprimir = New System.Windows.Forms.Button()
        Me.ButtonConvertir = New System.Windows.Forms.Button()
        Me.ButtonGrants = New System.Windows.Forms.Button()
        Me.ButtonImprimir2 = New System.Windows.Forms.Button()
        Me.CheckBoxOtrosCreditos = New System.Windows.Forms.CheckBox()
        Me.CheckBoxOtrosDebitos = New System.Windows.Forms.CheckBox()
        Me.DataGrid2 = New System.Windows.Forms.DataGrid()
        Me.ButtonImprimirSaldos = New System.Windows.Forms.Button()
        Me.ButtonFechasPosiblesNewConta = New System.Windows.Forms.Button()
        Me.ListBoxFechasPosibles = New System.Windows.Forms.ListBox()
        Me.CheckBoxTrataAnulacionesdelDia = New System.Windows.Forms.CheckBox()
        Me.ListBoxDebug2 = New System.Windows.Forms.ListBox()
        Me.CheckBoxMultiCobros = New System.Windows.Forms.CheckBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.DataGridHoteles, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGrid2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Teal
        Me.ButtonAceptar.ForeColor = System.Drawing.Color.White
        Me.ButtonAceptar.Image = CType(resources.GetObject("ButtonAceptar.Image"), System.Drawing.Image)
        Me.ButtonAceptar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonAceptar.Location = New System.Drawing.Point(170, 7)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(98, 35)
        Me.ButtonAceptar.TabIndex = 13
        Me.ButtonAceptar.Text = "&Procesar"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.CustomFormat = "dd/MM/yyyy"
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker1.Location = New System.Drawing.Point(12, 8)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(152, 20)
        Me.DateTimePicker1.TabIndex = 12
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
        Me.DataGridHoteles.Location = New System.Drawing.Point(12, 48)
        Me.DataGridHoteles.Name = "DataGridHoteles"
        Me.DataGridHoteles.ReadOnly = True
        Me.DataGridHoteles.Size = New System.Drawing.Size(317, 104)
        Me.DataGridHoteles.TabIndex = 14
        '
        'TextBoxRutaFicheros
        '
        Me.TextBoxRutaFicheros.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxRutaFicheros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxRutaFicheros.Location = New System.Drawing.Point(342, 8)
        Me.TextBoxRutaFicheros.Name = "TextBoxRutaFicheros"
        Me.TextBoxRutaFicheros.ReadOnly = True
        Me.TextBoxRutaFicheros.Size = New System.Drawing.Size(300, 20)
        Me.TextBoxRutaFicheros.TabIndex = 16
        '
        'CheckBoxDebug
        '
        Me.CheckBoxDebug.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxDebug.AutoSize = True
        Me.CheckBoxDebug.Checked = True
        Me.CheckBoxDebug.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxDebug.Location = New System.Drawing.Point(713, 43)
        Me.CheckBoxDebug.Name = "CheckBoxDebug"
        Me.CheckBoxDebug.Size = New System.Drawing.Size(58, 17)
        Me.CheckBoxDebug.TabIndex = 17
        Me.CheckBoxDebug.Text = "Debug"
        Me.CheckBoxDebug.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.ForeColor = System.Drawing.Color.SteelBlue
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 166)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(668, 15)
        Me.ProgressBar1.TabIndex = 18
        '
        'TextBoxDebug
        '
        Me.TextBoxDebug.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDebug.Location = New System.Drawing.Point(8, 416)
        Me.TextBoxDebug.Name = "TextBoxDebug"
        Me.TextBoxDebug.Size = New System.Drawing.Size(672, 20)
        Me.TextBoxDebug.TabIndex = 19
        '
        'ListBoxDebug
        '
        Me.ListBoxDebug.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListBoxDebug.FormattingEnabled = True
        Me.ListBoxDebug.HorizontalExtent = 3000
        Me.ListBoxDebug.HorizontalScrollbar = True
        Me.ListBoxDebug.Location = New System.Drawing.Point(8, 445)
        Me.ListBoxDebug.Name = "ListBoxDebug"
        Me.ListBoxDebug.ScrollAlwaysVisible = True
        Me.ListBoxDebug.Size = New System.Drawing.Size(339, 106)
        Me.ListBoxDebug.TabIndex = 20
        '
        'ButtonImprimir
        '
        Me.ButtonImprimir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonImprimir.Location = New System.Drawing.Point(696, 216)
        Me.ButtonImprimir.Name = "ButtonImprimir"
        Me.ButtonImprimir.Size = New System.Drawing.Size(75, 36)
        Me.ButtonImprimir.TabIndex = 21
        Me.ButtonImprimir.Text = "Imprimir Asiento(1)"
        Me.ButtonImprimir.UseVisualStyleBackColor = True
        '
        'ButtonConvertir
        '
        Me.ButtonConvertir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonConvertir.Location = New System.Drawing.Point(696, 187)
        Me.ButtonConvertir.Name = "ButtonConvertir"
        Me.ButtonConvertir.Size = New System.Drawing.Size(75, 23)
        Me.ButtonConvertir.TabIndex = 22
        Me.ButtonConvertir.Text = "Convertir"
        Me.ButtonConvertir.UseVisualStyleBackColor = True
        '
        'ButtonGrants
        '
        Me.ButtonGrants.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonGrants.Location = New System.Drawing.Point(696, 519)
        Me.ButtonGrants.Name = "ButtonGrants"
        Me.ButtonGrants.Size = New System.Drawing.Size(75, 34)
        Me.ButtonGrants.TabIndex = 23
        Me.ButtonGrants.Text = "Grants Central"
        Me.ButtonGrants.UseVisualStyleBackColor = True
        '
        'ButtonImprimir2
        '
        Me.ButtonImprimir2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonImprimir2.Location = New System.Drawing.Point(696, 251)
        Me.ButtonImprimir2.Name = "ButtonImprimir2"
        Me.ButtonImprimir2.Size = New System.Drawing.Size(75, 36)
        Me.ButtonImprimir2.TabIndex = 24
        Me.ButtonImprimir2.Text = "Imprimir Asiento(2)"
        Me.ButtonImprimir2.UseVisualStyleBackColor = True
        '
        'CheckBoxOtrosCreditos
        '
        Me.CheckBoxOtrosCreditos.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxOtrosCreditos.AutoSize = True
        Me.CheckBoxOtrosCreditos.Checked = True
        Me.CheckBoxOtrosCreditos.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxOtrosCreditos.Location = New System.Drawing.Point(654, 7)
        Me.CheckBoxOtrosCreditos.Name = "CheckBoxOtrosCreditos"
        Me.CheckBoxOtrosCreditos.Size = New System.Drawing.Size(92, 17)
        Me.CheckBoxOtrosCreditos.TabIndex = 25
        Me.CheckBoxOtrosCreditos.Text = "Otros Créditos"
        Me.CheckBoxOtrosCreditos.UseVisualStyleBackColor = True
        '
        'CheckBoxOtrosDebitos
        '
        Me.CheckBoxOtrosDebitos.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxOtrosDebitos.AutoSize = True
        Me.CheckBoxOtrosDebitos.Checked = True
        Me.CheckBoxOtrosDebitos.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxOtrosDebitos.Location = New System.Drawing.Point(654, 25)
        Me.CheckBoxOtrosDebitos.Name = "CheckBoxOtrosDebitos"
        Me.CheckBoxOtrosDebitos.Size = New System.Drawing.Size(90, 17)
        Me.CheckBoxOtrosDebitos.TabIndex = 26
        Me.CheckBoxOtrosDebitos.Text = "Otros Débitos"
        Me.CheckBoxOtrosDebitos.UseVisualStyleBackColor = True
        '
        'DataGrid2
        '
        Me.DataGrid2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGrid2.DataMember = ""
        Me.DataGrid2.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGrid2.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGrid2.Location = New System.Drawing.Point(8, 187)
        Me.DataGrid2.Name = "DataGrid2"
        Me.DataGrid2.ReadOnly = True
        Me.DataGrid2.Size = New System.Drawing.Size(672, 223)
        Me.DataGrid2.TabIndex = 15
        '
        'ButtonImprimirSaldos
        '
        Me.ButtonImprimirSaldos.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonImprimirSaldos.Image = CType(resources.GetObject("ButtonImprimirSaldos.Image"), System.Drawing.Image)
        Me.ButtonImprimirSaldos.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonImprimirSaldos.Location = New System.Drawing.Point(696, 307)
        Me.ButtonImprimirSaldos.Name = "ButtonImprimirSaldos"
        Me.ButtonImprimirSaldos.Size = New System.Drawing.Size(75, 103)
        Me.ButtonImprimirSaldos.TabIndex = 27
        Me.ButtonImprimirSaldos.Text = "Imprimir Sit. Saldos"
        Me.ButtonImprimirSaldos.UseVisualStyleBackColor = True
        '
        'ButtonFechasPosiblesNewConta
        '
        Me.ButtonFechasPosiblesNewConta.Image = Global.Menu.My.Resources.ResourceFondos.clear
        Me.ButtonFechasPosiblesNewConta.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonFechasPosiblesNewConta.Location = New System.Drawing.Point(274, 3)
        Me.ButtonFechasPosiblesNewConta.Name = "ButtonFechasPosiblesNewConta"
        Me.ButtonFechasPosiblesNewConta.Size = New System.Drawing.Size(55, 39)
        Me.ButtonFechasPosiblesNewConta.TabIndex = 28
        Me.ButtonFechasPosiblesNewConta.Text = "    ?"
        '
        'ListBoxFechasPosibles
        '
        Me.ListBoxFechasPosibles.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxFechasPosibles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListBoxFechasPosibles.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBoxFechasPosibles.FormattingEnabled = True
        Me.ListBoxFechasPosibles.HorizontalScrollbar = True
        Me.ListBoxFechasPosibles.ItemHeight = 14
        Me.ListBoxFechasPosibles.Location = New System.Drawing.Point(342, 66)
        Me.ListBoxFechasPosibles.Name = "ListBoxFechasPosibles"
        Me.ListBoxFechasPosibles.Size = New System.Drawing.Size(429, 86)
        Me.ListBoxFechasPosibles.TabIndex = 29
        Me.ListBoxFechasPosibles.Visible = False
        '
        'CheckBoxTrataAnulacionesdelDia
        '
        Me.CheckBoxTrataAnulacionesdelDia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxTrataAnulacionesdelDia.AutoSize = True
        Me.CheckBoxTrataAnulacionesdelDia.ForeColor = System.Drawing.Color.Maroon
        Me.CheckBoxTrataAnulacionesdelDia.Location = New System.Drawing.Point(342, 43)
        Me.CheckBoxTrataAnulacionesdelDia.Name = "CheckBoxTrataAnulacionesdelDia"
        Me.CheckBoxTrataAnulacionesdelDia.Size = New System.Drawing.Size(115, 17)
        Me.CheckBoxTrataAnulacionesdelDia.TabIndex = 30
        Me.CheckBoxTrataAnulacionesdelDia.Text = "Tratar Anulaciones"
        Me.ToolTip1.SetToolTip(Me.CheckBoxTrataAnulacionesdelDia, "Indica si se desea dar tratamiento a los Recibos Emitidos y Anulados en el mismo " &
        "Día")
        Me.CheckBoxTrataAnulacionesdelDia.UseVisualStyleBackColor = True
        '
        'ListBoxDebug2
        '
        Me.ListBoxDebug2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxDebug2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListBoxDebug2.FormattingEnabled = True
        Me.ListBoxDebug2.HorizontalExtent = 3000
        Me.ListBoxDebug2.HorizontalScrollbar = True
        Me.ListBoxDebug2.Location = New System.Drawing.Point(353, 445)
        Me.ListBoxDebug2.Name = "ListBoxDebug2"
        Me.ListBoxDebug2.ScrollAlwaysVisible = True
        Me.ListBoxDebug2.Size = New System.Drawing.Size(328, 106)
        Me.ListBoxDebug2.TabIndex = 31
        '
        'CheckBoxMultiCobros
        '
        Me.CheckBoxMultiCobros.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxMultiCobros.AutoSize = True
        Me.CheckBoxMultiCobros.Checked = True
        Me.CheckBoxMultiCobros.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxMultiCobros.Enabled = False
        Me.CheckBoxMultiCobros.ForeColor = System.Drawing.Color.Maroon
        Me.CheckBoxMultiCobros.Location = New System.Drawing.Point(474, 43)
        Me.CheckBoxMultiCobros.Name = "CheckBoxMultiCobros"
        Me.CheckBoxMultiCobros.Size = New System.Drawing.Size(176, 17)
        Me.CheckBoxMultiCobros.TabIndex = 32
        Me.CheckBoxMultiCobros.Text = "Gestión de Recibos Multicobros"
        Me.ToolTip1.SetToolTip(Me.CheckBoxMultiCobros, "En Caso de recibos con más de un Cobro y más de un Documento " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Se usa la asociaci" &
        "ón Cobro/Documento hecha por el software")
        Me.CheckBoxMultiCobros.UseVisualStyleBackColor = True
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 10000
        Me.ToolTip1.InitialDelay = 500
        Me.ToolTip1.IsBalloon = True
        Me.ToolTip1.ReshowDelay = 100
        '
        'FormIntegraNewConta
        '
        Me.AcceptButton = Me.ButtonAceptar
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.ClientSize = New System.Drawing.Size(792, 573)
        Me.Controls.Add(Me.CheckBoxMultiCobros)
        Me.Controls.Add(Me.ListBoxDebug2)
        Me.Controls.Add(Me.CheckBoxTrataAnulacionesdelDia)
        Me.Controls.Add(Me.ListBoxFechasPosibles)
        Me.Controls.Add(Me.ButtonFechasPosiblesNewConta)
        Me.Controls.Add(Me.ButtonImprimirSaldos)
        Me.Controls.Add(Me.CheckBoxOtrosDebitos)
        Me.Controls.Add(Me.CheckBoxOtrosCreditos)
        Me.Controls.Add(Me.ButtonImprimir2)
        Me.Controls.Add(Me.ButtonGrants)
        Me.Controls.Add(Me.ButtonConvertir)
        Me.Controls.Add(Me.ButtonImprimir)
        Me.Controls.Add(Me.ListBoxDebug)
        Me.Controls.Add(Me.TextBoxDebug)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.CheckBoxDebug)
        Me.Controls.Add(Me.TextBoxRutaFicheros)
        Me.Controls.Add(Me.DataGrid2)
        Me.Controls.Add(Me.DataGridHoteles)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "FormIntegraNewConta"
        Me.Text = "Integración NewConta"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridHoteles, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGrid2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents DataGridHoteles As System.Windows.Forms.DataGrid
    Friend WithEvents TextBoxRutaFicheros As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxDebug As System.Windows.Forms.CheckBox
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents TextBoxDebug As System.Windows.Forms.TextBox
    Friend WithEvents ListBoxDebug As System.Windows.Forms.ListBox
    Friend WithEvents ButtonImprimir As System.Windows.Forms.Button
    Friend WithEvents ButtonConvertir As System.Windows.Forms.Button
    Friend WithEvents ButtonGrants As System.Windows.Forms.Button
    Friend WithEvents ButtonImprimir2 As System.Windows.Forms.Button
    Friend WithEvents CheckBoxOtrosCreditos As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxOtrosDebitos As System.Windows.Forms.CheckBox
    Friend WithEvents DataGrid2 As System.Windows.Forms.DataGrid
    Friend WithEvents ButtonImprimirSaldos As System.Windows.Forms.Button
    Friend WithEvents ButtonFechasPosiblesNewConta As System.Windows.Forms.Button
    Friend WithEvents ListBoxFechasPosibles As System.Windows.Forms.ListBox
    Friend WithEvents CheckBoxTrataAnulacionesdelDia As System.Windows.Forms.CheckBox
    Friend WithEvents ListBoxDebug2 As System.Windows.Forms.ListBox
    Friend WithEvents CheckBoxMultiCobros As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
