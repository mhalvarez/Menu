<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormPerfilEnvioLopezAlmacen
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
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.TextBoxDebug = New System.Windows.Forms.TextBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.RadioButtonAmbos = New System.Windows.Forms.RadioButton
        Me.RadioButtonNewPaga = New System.Windows.Forms.RadioButton
        Me.RadioButtonNewStock = New System.Windows.Forms.RadioButton
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Location = New System.Drawing.Point(495, 13)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(75, 23)
        Me.ButtonAceptar.TabIndex = 0
        Me.ButtonAceptar.Text = "&Aceptar"
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'TextBoxDebug
        '
        Me.TextBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDebug.Location = New System.Drawing.Point(12, 123)
        Me.TextBoxDebug.Name = "TextBoxDebug"
        Me.TextBoxDebug.Size = New System.Drawing.Size(576, 20)
        Me.TextBoxDebug.TabIndex = 3
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RadioButtonAmbos)
        Me.GroupBox1.Controls.Add(Me.RadioButtonNewPaga)
        Me.GroupBox1.Controls.Add(Me.RadioButtonNewStock)
        Me.GroupBox1.Controls.Add(Me.ButtonAceptar)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 9)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(576, 108)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        '
        'RadioButtonAmbos
        '
        Me.RadioButtonAmbos.AutoSize = True
        Me.RadioButtonAmbos.Checked = True
        Me.RadioButtonAmbos.Enabled = False
        Me.RadioButtonAmbos.Location = New System.Drawing.Point(17, 65)
        Me.RadioButtonAmbos.Name = "RadioButtonAmbos"
        Me.RadioButtonAmbos.Size = New System.Drawing.Size(57, 17)
        Me.RadioButtonAmbos.TabIndex = 3
        Me.RadioButtonAmbos.TabStop = True
        Me.RadioButtonAmbos.Text = "Ambos"
        Me.RadioButtonAmbos.UseVisualStyleBackColor = True
        '
        'RadioButtonNewPaga
        '
        Me.RadioButtonNewPaga.AutoSize = True
        Me.RadioButtonNewPaga.Location = New System.Drawing.Point(17, 42)
        Me.RadioButtonNewPaga.Name = "RadioButtonNewPaga"
        Me.RadioButtonNewPaga.Size = New System.Drawing.Size(72, 17)
        Me.RadioButtonNewPaga.TabIndex = 2
        Me.RadioButtonNewPaga.Text = "NewPaga"
        Me.RadioButtonNewPaga.UseVisualStyleBackColor = True
        '
        'RadioButtonNewStock
        '
        Me.RadioButtonNewStock.AutoSize = True
        Me.RadioButtonNewStock.Location = New System.Drawing.Point(17, 19)
        Me.RadioButtonNewStock.Name = "RadioButtonNewStock"
        Me.RadioButtonNewStock.Size = New System.Drawing.Size(75, 17)
        Me.RadioButtonNewStock.TabIndex = 1
        Me.RadioButtonNewStock.Text = "NewStock"
        Me.RadioButtonNewStock.UseVisualStyleBackColor = True
        '
        'FormPerfilEnvioLopezAlmacen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(600, 152)
        Me.Controls.Add(Me.TextBoxDebug)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormPerfilEnvioLopezAlmacen"
        Me.Text = "Perfil de Envío"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents TextBoxDebug As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButtonAmbos As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonNewPaga As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonNewStock As System.Windows.Forms.RadioButton
End Class
