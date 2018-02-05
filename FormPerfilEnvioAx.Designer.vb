<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormPerfilEnvioAx
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
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.RadioButtonAlmacen = New System.Windows.Forms.RadioButton
        Me.RadioButtonContabilidad = New System.Windows.Forms.RadioButton
        Me.TextBoxDebug = New System.Windows.Forms.TextBox
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RadioButtonAmbos)
        Me.GroupBox1.Controls.Add(Me.ButtonAceptar)
        Me.GroupBox1.Controls.Add(Me.RadioButtonAlmacen)
        Me.GroupBox1.Controls.Add(Me.RadioButtonContabilidad)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(374, 105)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'RadioButtonAmbos
        '
        Me.RadioButtonAmbos.AutoSize = True
        Me.RadioButtonAmbos.Location = New System.Drawing.Point(19, 77)
        Me.RadioButtonAmbos.Name = "RadioButtonAmbos"
        Me.RadioButtonAmbos.Size = New System.Drawing.Size(57, 17)
        Me.RadioButtonAmbos.TabIndex = 4
        Me.RadioButtonAmbos.TabStop = True
        Me.RadioButtonAmbos.Text = "Ambos"
        Me.RadioButtonAmbos.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Location = New System.Drawing.Point(277, 21)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(75, 23)
        Me.ButtonAceptar.TabIndex = 2
        Me.ButtonAceptar.Text = "&Aceptar"
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'RadioButtonAlmacen
        '
        Me.RadioButtonAlmacen.AutoSize = True
        Me.RadioButtonAlmacen.Location = New System.Drawing.Point(19, 44)
        Me.RadioButtonAlmacen.Name = "RadioButtonAlmacen"
        Me.RadioButtonAlmacen.Size = New System.Drawing.Size(66, 17)
        Me.RadioButtonAlmacen.TabIndex = 1
        Me.RadioButtonAlmacen.Text = "Almacén"
        Me.RadioButtonAlmacen.UseVisualStyleBackColor = True
        '
        'RadioButtonContabilidad
        '
        Me.RadioButtonContabilidad.AutoSize = True
        Me.RadioButtonContabilidad.Checked = True
        Me.RadioButtonContabilidad.Location = New System.Drawing.Point(19, 21)
        Me.RadioButtonContabilidad.Name = "RadioButtonContabilidad"
        Me.RadioButtonContabilidad.Size = New System.Drawing.Size(83, 17)
        Me.RadioButtonContabilidad.TabIndex = 0
        Me.RadioButtonContabilidad.TabStop = True
        Me.RadioButtonContabilidad.Text = "Contabilidad"
        Me.RadioButtonContabilidad.UseVisualStyleBackColor = True
        '
        'TextBoxDebug
        '
        Me.TextBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDebug.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBoxDebug.Location = New System.Drawing.Point(9, 120)
        Me.TextBoxDebug.Name = "TextBoxDebug"
        Me.TextBoxDebug.ReadOnly = True
        Me.TextBoxDebug.Size = New System.Drawing.Size(371, 20)
        Me.TextBoxDebug.TabIndex = 1
        '
        'FormPerfilEnvioAx
        '
        Me.AcceptButton = Me.ButtonAceptar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(387, 161)
        Me.Controls.Add(Me.TextBoxDebug)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormPerfilEnvioAx"
        Me.Text = "Perfil de Envío Axapta"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButtonAlmacen As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonContabilidad As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonAmbos As System.Windows.Forms.RadioButton
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents TextBoxDebug As System.Windows.Forms.TextBox
End Class
