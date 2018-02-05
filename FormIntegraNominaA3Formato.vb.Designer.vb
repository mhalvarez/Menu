<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormIntegraNominaA3Formato
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormIntegraNominaA3Formato))
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonCancelar = New System.Windows.Forms.Button
        Me.RadioButtonNomina = New System.Windows.Forms.RadioButton
        Me.RadioButtonRemesa = New System.Windows.Forms.RadioButton
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextBoxCtaBanco = New System.Windows.Forms.TextBox
        Me.ButtonRutaExcel = New System.Windows.Forms.Button
        Me.TextBoxRutaLibroExcel = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.GroupBoxRemesa = New System.Windows.Forms.GroupBox
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.ListBoxLibrosNif = New System.Windows.Forms.ListBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.TextBoxRutaNif = New System.Windows.Forms.TextBox
        Me.GroupBox1.SuspendLayout()
        Me.GroupBoxRemesa.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Location = New System.Drawing.Point(491, 12)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(75, 23)
        Me.ButtonAceptar.TabIndex = 0
        Me.ButtonAceptar.Text = "&Aceptar"
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ButtonCancelar
        '
        Me.ButtonCancelar.Location = New System.Drawing.Point(491, 41)
        Me.ButtonCancelar.Name = "ButtonCancelar"
        Me.ButtonCancelar.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCancelar.TabIndex = 1
        Me.ButtonCancelar.Text = "&Cancelar"
        Me.ButtonCancelar.UseVisualStyleBackColor = True
        '
        'RadioButtonNomina
        '
        Me.RadioButtonNomina.AutoSize = True
        Me.RadioButtonNomina.Location = New System.Drawing.Point(9, 15)
        Me.RadioButtonNomina.Name = "RadioButtonNomina"
        Me.RadioButtonNomina.Size = New System.Drawing.Size(61, 17)
        Me.RadioButtonNomina.TabIndex = 0
        Me.RadioButtonNomina.TabStop = True
        Me.RadioButtonNomina.Text = "Nómina"
        Me.RadioButtonNomina.UseVisualStyleBackColor = True
        '
        'RadioButtonRemesa
        '
        Me.RadioButtonRemesa.AutoSize = True
        Me.RadioButtonRemesa.Location = New System.Drawing.Point(120, 15)
        Me.RadioButtonRemesa.Name = "RadioButtonRemesa"
        Me.RadioButtonRemesa.Size = New System.Drawing.Size(109, 17)
        Me.RadioButtonRemesa.TabIndex = 1
        Me.RadioButtonRemesa.TabStop = True
        Me.RadioButtonRemesa.Text = "Remesa Bancaria"
        Me.RadioButtonRemesa.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RadioButtonRemesa)
        Me.GroupBox1.Controls.Add(Me.RadioButtonNomina)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(262, 61)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 122)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(78, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Cuenta Banco "
        '
        'TextBoxCtaBanco
        '
        Me.TextBoxCtaBanco.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCtaBanco.Location = New System.Drawing.Point(122, 123)
        Me.TextBoxCtaBanco.Name = "TextBoxCtaBanco"
        Me.TextBoxCtaBanco.Size = New System.Drawing.Size(149, 20)
        Me.TextBoxCtaBanco.TabIndex = 1
        '
        'ButtonRutaExcel
        '
        Me.ButtonRutaExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonRutaExcel.Image = CType(resources.GetObject("ButtonRutaExcel.Image"), System.Drawing.Image)
        Me.ButtonRutaExcel.Location = New System.Drawing.Point(506, 26)
        Me.ButtonRutaExcel.Name = "ButtonRutaExcel"
        Me.ButtonRutaExcel.Size = New System.Drawing.Size(44, 23)
        Me.ButtonRutaExcel.TabIndex = 9
        Me.ButtonRutaExcel.UseVisualStyleBackColor = True
        '
        'TextBoxRutaLibroExcel
        '
        Me.TextBoxRutaLibroExcel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxRutaLibroExcel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxRutaLibroExcel.Location = New System.Drawing.Point(120, 29)
        Me.TextBoxRutaLibroExcel.Name = "TextBoxRutaLibroExcel"
        Me.TextBoxRutaLibroExcel.Size = New System.Drawing.Size(380, 20)
        Me.TextBoxRutaLibroExcel.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 31)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Libro Excel Nifes"
        '
        'GroupBoxRemesa
        '
        Me.GroupBoxRemesa.Controls.Add(Me.Label6)
        Me.GroupBoxRemesa.Controls.Add(Me.TextBoxRutaNif)
        Me.GroupBoxRemesa.Controls.Add(Me.ListBoxLibrosNif)
        Me.GroupBoxRemesa.Controls.Add(Me.TextBoxRutaLibroExcel)
        Me.GroupBoxRemesa.Controls.Add(Me.TextBoxCtaBanco)
        Me.GroupBoxRemesa.Controls.Add(Me.ButtonRutaExcel)
        Me.GroupBoxRemesa.Controls.Add(Me.Label1)
        Me.GroupBoxRemesa.Controls.Add(Me.Label2)
        Me.GroupBoxRemesa.Location = New System.Drawing.Point(3, 70)
        Me.GroupBoxRemesa.Name = "GroupBoxRemesa"
        Me.GroupBoxRemesa.Size = New System.Drawing.Size(563, 173)
        Me.GroupBoxRemesa.TabIndex = 10
        Me.GroupBoxRemesa.TabStop = False
        Me.GroupBoxRemesa.Text = "Datos para Remesas"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'ListBoxLibrosNif
        '
        Me.ListBoxLibrosNif.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxLibrosNif.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListBoxLibrosNif.FormattingEnabled = True
        Me.ListBoxLibrosNif.HorizontalExtent = 500
        Me.ListBoxLibrosNif.HorizontalScrollbar = True
        Me.ListBoxLibrosNif.Location = New System.Drawing.Point(290, 51)
        Me.ListBoxLibrosNif.Name = "ListBoxLibrosNif"
        Me.ListBoxLibrosNif.Size = New System.Drawing.Size(210, 93)
        Me.ListBoxLibrosNif.TabIndex = 26
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(63, 70)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(45, 13)
        Me.Label6.TabIndex = 28
        Me.Label6.Text = "Hoja Nif"
        '
        'TextBoxRutaNif
        '
        Me.TextBoxRutaNif.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxRutaNif.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxRutaNif.Location = New System.Drawing.Point(120, 63)
        Me.TextBoxRutaNif.Name = "TextBoxRutaNif"
        Me.TextBoxRutaNif.Size = New System.Drawing.Size(151, 20)
        Me.TextBoxRutaNif.TabIndex = 27
        '
        'FormIntegraNominaA3Formato
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(578, 255)
        Me.Controls.Add(Me.GroupBoxRemesa)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ButtonCancelar)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormIntegraNominaA3Formato"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Tipo de Datos a Procesar"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBoxRemesa.ResumeLayout(False)
        Me.GroupBoxRemesa.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonCancelar As System.Windows.Forms.Button
    Friend WithEvents RadioButtonNomina As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonRemesa As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCtaBanco As System.Windows.Forms.TextBox
    Friend WithEvents ButtonRutaExcel As System.Windows.Forms.Button
    Friend WithEvents TextBoxRutaLibroExcel As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBoxRemesa As System.Windows.Forms.GroupBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ListBoxLibrosNif As System.Windows.Forms.ListBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextBoxRutaNif As System.Windows.Forms.TextBox
End Class
