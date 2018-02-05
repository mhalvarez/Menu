<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormIntegraNewPaga
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormIntegraNewPaga))
        Me.ButtonConvertir = New System.Windows.Forms.Button
        Me.ButtonImprimir = New System.Windows.Forms.Button
        Me.ListBoxDebug = New System.Windows.Forms.ListBox
        Me.TextBoxDebug = New System.Windows.Forms.TextBox
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
        Me.CheckBoxDebug = New System.Windows.Forms.CheckBox
        Me.TextBoxRutaFicheros = New System.Windows.Forms.TextBox
        Me.DataGrid2 = New System.Windows.Forms.DataGrid
        Me.DataGridHoteles = New System.Windows.Forms.DataGrid
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.ButtonAceptar = New System.Windows.Forms.Button
        CType(Me.DataGrid2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridHoteles, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonConvertir
        '
        Me.ButtonConvertir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonConvertir.Location = New System.Drawing.Point(703, 197)
        Me.ButtonConvertir.Name = "ButtonConvertir"
        Me.ButtonConvertir.Size = New System.Drawing.Size(75, 23)
        Me.ButtonConvertir.TabIndex = 32
        Me.ButtonConvertir.Text = "Convertir"
        Me.ButtonConvertir.UseVisualStyleBackColor = True
        '
        'ButtonImprimir
        '
        Me.ButtonImprimir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonImprimir.Location = New System.Drawing.Point(703, 165)
        Me.ButtonImprimir.Name = "ButtonImprimir"
        Me.ButtonImprimir.Size = New System.Drawing.Size(75, 23)
        Me.ButtonImprimir.TabIndex = 31
        Me.ButtonImprimir.Text = "Imprimir"
        Me.ButtonImprimir.UseVisualStyleBackColor = True
        '
        'ListBoxDebug
        '
        Me.ListBoxDebug.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ListBoxDebug.FormattingEnabled = True
        Me.ListBoxDebug.Location = New System.Drawing.Point(15, 502)
        Me.ListBoxDebug.Name = "ListBoxDebug"
        Me.ListBoxDebug.Size = New System.Drawing.Size(672, 56)
        Me.ListBoxDebug.TabIndex = 30
        '
        'TextBoxDebug
        '
        Me.TextBoxDebug.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxDebug.Location = New System.Drawing.Point(15, 470)
        Me.TextBoxDebug.Name = "TextBoxDebug"
        Me.TextBoxDebug.Size = New System.Drawing.Size(672, 20)
        Me.TextBoxDebug.TabIndex = 29
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(15, 141)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(672, 15)
        Me.ProgressBar1.TabIndex = 28
        '
        'CheckBoxDebug
        '
        Me.CheckBoxDebug.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxDebug.AutoSize = True
        Me.CheckBoxDebug.Checked = True
        Me.CheckBoxDebug.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxDebug.Location = New System.Drawing.Point(679, 37)
        Me.CheckBoxDebug.Name = "CheckBoxDebug"
        Me.CheckBoxDebug.Size = New System.Drawing.Size(58, 17)
        Me.CheckBoxDebug.TabIndex = 27
        Me.CheckBoxDebug.Text = "Debug"
        Me.CheckBoxDebug.UseVisualStyleBackColor = True
        '
        'TextBoxRutaFicheros
        '
        Me.TextBoxRutaFicheros.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxRutaFicheros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxRutaFicheros.Location = New System.Drawing.Point(431, 13)
        Me.TextBoxRutaFicheros.Name = "TextBoxRutaFicheros"
        Me.TextBoxRutaFicheros.ReadOnly = True
        Me.TextBoxRutaFicheros.Size = New System.Drawing.Size(276, 20)
        Me.TextBoxRutaFicheros.TabIndex = 26
        '
        'DataGrid2
        '
        Me.DataGrid2.AlternatingBackColor = System.Drawing.SystemColors.ScrollBar
        Me.DataGrid2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGrid2.DataMember = ""
        Me.DataGrid2.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGrid2.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGrid2.Location = New System.Drawing.Point(15, 165)
        Me.DataGrid2.Name = "DataGrid2"
        Me.DataGrid2.ReadOnly = True
        Me.DataGrid2.Size = New System.Drawing.Size(672, 297)
        Me.DataGrid2.TabIndex = 25
        '
        'DataGridHoteles
        '
        Me.DataGridHoteles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridHoteles.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DataGridHoteles.DataMember = ""
        Me.DataGridHoteles.Font = New System.Drawing.Font("Arial", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridHoteles.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGridHoteles.Location = New System.Drawing.Point(15, 37)
        Me.DataGridHoteles.Name = "DataGridHoteles"
        Me.DataGridHoteles.ReadOnly = True
        Me.DataGridHoteles.Size = New System.Drawing.Size(392, 96)
        Me.DataGridHoteles.TabIndex = 24
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.CustomFormat = "dd/MM/yyyy"
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker1.Location = New System.Drawing.Point(15, 13)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(152, 20)
        Me.DateTimePicker1.TabIndex = 23
        Me.DateTimePicker1.Value = New Date(2006, 4, 10, 0, 0, 0, 0)
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Image = CType(resources.GetObject("ButtonAceptar.Image"), System.Drawing.Image)
        Me.ButtonAceptar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonAceptar.Location = New System.Drawing.Point(184, 8)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(80, 23)
        Me.ButtonAceptar.TabIndex = 33
        Me.ButtonAceptar.Text = "&Aceptar"
        '
        'FormIntegraNewPaga
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.ClientSize = New System.Drawing.Size(792, 570)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ButtonConvertir)
        Me.Controls.Add(Me.ButtonImprimir)
        Me.Controls.Add(Me.ListBoxDebug)
        Me.Controls.Add(Me.TextBoxDebug)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.CheckBoxDebug)
        Me.Controls.Add(Me.TextBoxRutaFicheros)
        Me.Controls.Add(Me.DataGrid2)
        Me.Controls.Add(Me.DataGridHoteles)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "FormIntegraNewPaga"
        Me.Text = "Integración NewPaga"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGrid2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridHoteles, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonConvertir As System.Windows.Forms.Button
    Friend WithEvents ButtonImprimir As System.Windows.Forms.Button
    Friend WithEvents ListBoxDebug As System.Windows.Forms.ListBox
    Friend WithEvents TextBoxDebug As System.Windows.Forms.TextBox
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents CheckBoxDebug As System.Windows.Forms.CheckBox
    Friend WithEvents TextBoxRutaFicheros As System.Windows.Forms.TextBox
    Friend WithEvents DataGrid2 As System.Windows.Forms.DataGrid
    Friend WithEvents DataGridHoteles As System.Windows.Forms.DataGrid
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
End Class
