<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormConvertirAxaptaCompras
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
        Me.ListBoxDebug = New System.Windows.Forms.ListBox()
        Me.ButtonAceptar = New System.Windows.Forms.Button()
        Me.NumericUpDownWebServiceTimeOut = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBoxDebug = New System.Windows.Forms.TextBox()
        Me.ButtonImprimir = New System.Windows.Forms.Button()
        Me.ButtonImprimirErrores = New System.Windows.Forms.Button()
        Me.CheckBoxSoloPendintes = New System.Windows.Forms.CheckBox()
        Me.ButtonValidar = New System.Windows.Forms.Button()
        Me.ButtonResetEnvio = New System.Windows.Forms.Button()
        Me.DataGridViewAsientos = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExcluirDocumentoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBoxDocumento = New System.Windows.Forms.TextBox()
        Me.CheckBoxAjustar2 = New System.Windows.Forms.CheckBox()
        Me.TextBoxDebug2 = New System.Windows.Forms.TextBox()
        Me.CheckBoxDtoValor = New System.Windows.Forms.CheckBox()
        CType(Me.NumericUpDownWebServiceTimeOut, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridViewAsientos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListBoxDebug
        '
        Me.ListBoxDebug.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxDebug.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBoxDebug.FormattingEnabled = True
        Me.ListBoxDebug.HorizontalExtent = 5000
        Me.ListBoxDebug.HorizontalScrollbar = True
        Me.ListBoxDebug.ItemHeight = 14
        Me.ListBoxDebug.Location = New System.Drawing.Point(11, 233)
        Me.ListBoxDebug.Name = "ListBoxDebug"
        Me.ListBoxDebug.Size = New System.Drawing.Size(709, 158)
        Me.ListBoxDebug.TabIndex = 1
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonAceptar.Location = New System.Drawing.Point(804, 12)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(85, 23)
        Me.ButtonAceptar.TabIndex = 2
        Me.ButtonAceptar.Text = "&Aceptar"
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'NumericUpDownWebServiceTimeOut
        '
        Me.NumericUpDownWebServiceTimeOut.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.NumericUpDownWebServiceTimeOut.Location = New System.Drawing.Point(66, 401)
        Me.NumericUpDownWebServiceTimeOut.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.NumericUpDownWebServiceTimeOut.Name = "NumericUpDownWebServiceTimeOut"
        Me.NumericUpDownWebServiceTimeOut.Size = New System.Drawing.Size(72, 20)
        Me.NumericUpDownWebServiceTimeOut.TabIndex = 38
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 401)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 13)
        Me.Label2.TabIndex = 37
        Me.Label2.Text = "Time Out"
        '
        'TextBoxDebug
        '
        Me.TextBoxDebug.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxDebug.BackColor = System.Drawing.SystemColors.Info
        Me.TextBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDebug.Location = New System.Drawing.Point(164, 403)
        Me.TextBoxDebug.Name = "TextBoxDebug"
        Me.TextBoxDebug.Size = New System.Drawing.Size(412, 20)
        Me.TextBoxDebug.TabIndex = 39
        '
        'ButtonImprimir
        '
        Me.ButtonImprimir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonImprimir.Location = New System.Drawing.Point(804, 233)
        Me.ButtonImprimir.Name = "ButtonImprimir"
        Me.ButtonImprimir.Size = New System.Drawing.Size(85, 23)
        Me.ButtonImprimir.TabIndex = 40
        Me.ButtonImprimir.Text = "&Imprimir"
        Me.ButtonImprimir.UseVisualStyleBackColor = True
        '
        'ButtonImprimirErrores
        '
        Me.ButtonImprimirErrores.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonImprimirErrores.Location = New System.Drawing.Point(804, 262)
        Me.ButtonImprimirErrores.Name = "ButtonImprimirErrores"
        Me.ButtonImprimirErrores.Size = New System.Drawing.Size(85, 38)
        Me.ButtonImprimirErrores.TabIndex = 42
        Me.ButtonImprimirErrores.Text = "&Imprimir Errores"
        Me.ButtonImprimirErrores.UseVisualStyleBackColor = True
        '
        'CheckBoxSoloPendintes
        '
        Me.CheckBoxSoloPendintes.AutoSize = True
        Me.CheckBoxSoloPendintes.Location = New System.Drawing.Point(11, 10)
        Me.CheckBoxSoloPendintes.Name = "CheckBoxSoloPendintes"
        Me.CheckBoxSoloPendintes.Size = New System.Drawing.Size(103, 17)
        Me.CheckBoxSoloPendintes.TabIndex = 43
        Me.CheckBoxSoloPendintes.Text = "Solo Pendientes"
        Me.CheckBoxSoloPendintes.UseVisualStyleBackColor = True
        '
        'ButtonValidar
        '
        Me.ButtonValidar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonValidar.Location = New System.Drawing.Point(804, 183)
        Me.ButtonValidar.Name = "ButtonValidar"
        Me.ButtonValidar.Size = New System.Drawing.Size(85, 23)
        Me.ButtonValidar.TabIndex = 44
        Me.ButtonValidar.Text = "&Validar"
        Me.ButtonValidar.UseVisualStyleBackColor = True
        '
        'ButtonResetEnvio
        '
        Me.ButtonResetEnvio.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonResetEnvio.Location = New System.Drawing.Point(804, 322)
        Me.ButtonResetEnvio.Name = "ButtonResetEnvio"
        Me.ButtonResetEnvio.Size = New System.Drawing.Size(85, 69)
        Me.ButtonResetEnvio.TabIndex = 45
        Me.ButtonResetEnvio.Text = "Marca el Día Como NO enviado"
        Me.ButtonResetEnvio.UseVisualStyleBackColor = True
        Me.ButtonResetEnvio.Visible = False
        '
        'DataGridViewAsientos
        '
        Me.DataGridViewAsientos.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridViewAsientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewAsientos.ContextMenuStrip = Me.ContextMenuStrip1
        Me.DataGridViewAsientos.Location = New System.Drawing.Point(11, 33)
        Me.DataGridViewAsientos.Name = "DataGridViewAsientos"
        Me.DataGridViewAsientos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridViewAsientos.Size = New System.Drawing.Size(709, 173)
        Me.DataGridViewAsientos.TabIndex = 46
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExcluirDocumentoToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(175, 26)
        '
        'ExcluirDocumentoToolStripMenuItem
        '
        Me.ExcluirDocumentoToolStripMenuItem.Name = "ExcluirDocumentoToolStripMenuItem"
        Me.ExcluirDocumentoToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.ExcluirDocumentoToolStripMenuItem.Text = "Excluir Documento"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(536, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 13)
        Me.Label1.TabIndex = 48
        Me.Label1.Text = "Documento"
        '
        'TextBoxDocumento
        '
        Me.TextBoxDocumento.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxDocumento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDocumento.Location = New System.Drawing.Point(613, 10)
        Me.TextBoxDocumento.Name = "TextBoxDocumento"
        Me.TextBoxDocumento.Size = New System.Drawing.Size(107, 20)
        Me.TextBoxDocumento.TabIndex = 49
        '
        'CheckBoxAjustar2
        '
        Me.CheckBoxAjustar2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxAjustar2.AutoSize = True
        Me.CheckBoxAjustar2.Checked = True
        Me.CheckBoxAjustar2.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxAjustar2.Location = New System.Drawing.Point(740, 56)
        Me.CheckBoxAjustar2.Name = "CheckBoxAjustar2"
        Me.CheckBoxAjustar2.Size = New System.Drawing.Size(165, 17)
        Me.CheckBoxAjustar2.TabIndex = 50
        Me.CheckBoxAjustar2.Text = "Recálculo TotalNovat y Total"
        Me.CheckBoxAjustar2.UseVisualStyleBackColor = True
        '
        'TextBoxDebug2
        '
        Me.TextBoxDebug2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxDebug2.BackColor = System.Drawing.SystemColors.Info
        Me.TextBoxDebug2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDebug2.Location = New System.Drawing.Point(582, 403)
        Me.TextBoxDebug2.Name = "TextBoxDebug2"
        Me.TextBoxDebug2.Size = New System.Drawing.Size(138, 20)
        Me.TextBoxDebug2.TabIndex = 51
        '
        'CheckBoxDtoValor
        '
        Me.CheckBoxDtoValor.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxDtoValor.AutoSize = True
        Me.CheckBoxDtoValor.Location = New System.Drawing.Point(740, 91)
        Me.CheckBoxDtoValor.Name = "CheckBoxDtoValor"
        Me.CheckBoxDtoValor.Size = New System.Drawing.Size(154, 30)
        Me.CheckBoxDtoValor.TabIndex = 52
        Me.CheckBoxDtoValor.Text = "Trata Problema " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Descuento Cabecera Valor"
        Me.CheckBoxDtoValor.UseVisualStyleBackColor = True
        '
        'FormConvertirAxaptaCompras
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(906, 433)
        Me.Controls.Add(Me.CheckBoxDtoValor)
        Me.Controls.Add(Me.TextBoxDebug2)
        Me.Controls.Add(Me.DataGridViewAsientos)
        Me.Controls.Add(Me.CheckBoxAjustar2)
        Me.Controls.Add(Me.TextBoxDocumento)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ButtonResetEnvio)
        Me.Controls.Add(Me.ButtonValidar)
        Me.Controls.Add(Me.CheckBoxSoloPendintes)
        Me.Controls.Add(Me.ButtonImprimirErrores)
        Me.Controls.Add(Me.ButtonImprimir)
        Me.Controls.Add(Me.TextBoxDebug)
        Me.Controls.Add(Me.NumericUpDownWebServiceTimeOut)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ListBoxDebug)
        Me.MinimumSize = New System.Drawing.Size(914, 460)
        Me.Name = "FormConvertirAxaptaCompras"
        Me.Text = "Axapta Compras"
        CType(Me.NumericUpDownWebServiceTimeOut, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridViewAsientos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ListBoxDebug As System.Windows.Forms.ListBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents NumericUpDownWebServiceTimeOut As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBoxDebug As System.Windows.Forms.TextBox
    Friend WithEvents ButtonImprimir As System.Windows.Forms.Button
    Friend WithEvents ButtonImprimirErrores As System.Windows.Forms.Button
    Friend WithEvents CheckBoxSoloPendintes As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonValidar As System.Windows.Forms.Button
    Friend WithEvents ButtonResetEnvio As System.Windows.Forms.Button
    Friend WithEvents DataGridViewAsientos As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBoxDocumento As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxAjustar2 As System.Windows.Forms.CheckBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExcluirDocumentoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TextBoxDebug2 As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxDtoValor As CheckBox
End Class
