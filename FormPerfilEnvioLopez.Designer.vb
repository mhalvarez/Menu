<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormPerfilEnvioLopez
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.RadioButtonAmbos = New System.Windows.Forms.RadioButton
        Me.RadioButtonCaja = New System.Windows.Forms.RadioButton
        Me.RadioButtonFacturas = New System.Windows.Forms.RadioButton
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.TextBoxDebug = New System.Windows.Forms.TextBox
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RadioButtonAmbos)
        Me.GroupBox1.Controls.Add(Me.RadioButtonCaja)
        Me.GroupBox1.Controls.Add(Me.RadioButtonFacturas)
        Me.GroupBox1.Controls.Add(Me.ButtonAceptar)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(535, 108)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'RadioButtonAmbos
        '
        Me.RadioButtonAmbos.AutoSize = True
        Me.RadioButtonAmbos.Checked = True
        Me.RadioButtonAmbos.Location = New System.Drawing.Point(17, 65)
        Me.RadioButtonAmbos.Name = "RadioButtonAmbos"
        Me.RadioButtonAmbos.Size = New System.Drawing.Size(57, 17)
        Me.RadioButtonAmbos.TabIndex = 3
        Me.RadioButtonAmbos.TabStop = True
        Me.RadioButtonAmbos.Text = "Ambos"
        Me.RadioButtonAmbos.UseVisualStyleBackColor = True
        '
        'RadioButtonCaja
        '
        Me.RadioButtonCaja.AutoSize = True
        Me.RadioButtonCaja.Location = New System.Drawing.Point(17, 42)
        Me.RadioButtonCaja.Name = "RadioButtonCaja"
        Me.RadioButtonCaja.Size = New System.Drawing.Size(70, 17)
        Me.RadioButtonCaja.TabIndex = 2
        Me.RadioButtonCaja.Text = "Solo Caja"
        Me.RadioButtonCaja.UseVisualStyleBackColor = True
        '
        'RadioButtonFacturas
        '
        Me.RadioButtonFacturas.AutoSize = True
        Me.RadioButtonFacturas.Location = New System.Drawing.Point(17, 19)
        Me.RadioButtonFacturas.Name = "RadioButtonFacturas"
        Me.RadioButtonFacturas.Size = New System.Drawing.Size(90, 17)
        Me.RadioButtonFacturas.TabIndex = 1
        Me.RadioButtonFacturas.Text = "Solo Facturas"
        Me.RadioButtonFacturas.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Location = New System.Drawing.Point(454, 19)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(75, 23)
        Me.ButtonAceptar.TabIndex = 0
        Me.ButtonAceptar.Text = "&Aceptar"
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'TextBoxDebug
        '
        Me.TextBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDebug.Location = New System.Drawing.Point(12, 118)
        Me.TextBoxDebug.Name = "TextBoxDebug"
        Me.TextBoxDebug.Size = New System.Drawing.Size(535, 20)
        Me.TextBoxDebug.TabIndex = 1
        '
        'FormPerfilEnvioLopez
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(559, 149)
        Me.Controls.Add(Me.TextBoxDebug)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormPerfilEnvioLopez"
        Me.Text = "Perfil de Envío"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents RadioButtonAmbos As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonCaja As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonFacturas As System.Windows.Forms.RadioButton
    Friend WithEvents TextBoxDebug As System.Windows.Forms.TextBox
End Class
