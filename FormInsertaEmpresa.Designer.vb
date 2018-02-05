<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormInsertaEmpresa
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormInsertaEmpresa))
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonCancelar = New System.Windows.Forms.Button
        Me.ComboBoxGrupoCod = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.TextBoxEmpCod = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextBoxDescripcion = New System.Windows.Forms.TextBox
        Me.CheckBoxValoresPorDefecto = New System.Windows.Forms.CheckBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.CheckBoxTipoHotel = New System.Windows.Forms.CheckBox
        Me.CheckBoxTipoOtroTipo = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Image = CType(resources.GetObject("ButtonAceptar.Image"), System.Drawing.Image)
        Me.ButtonAceptar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonAceptar.Location = New System.Drawing.Point(488, 12)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(80, 23)
        Me.ButtonAceptar.TabIndex = 3
        Me.ButtonAceptar.Text = "&Aceptar"
        '
        'ButtonCancelar
        '
        Me.ButtonCancelar.Location = New System.Drawing.Point(488, 41)
        Me.ButtonCancelar.Name = "ButtonCancelar"
        Me.ButtonCancelar.Size = New System.Drawing.Size(80, 23)
        Me.ButtonCancelar.TabIndex = 4
        Me.ButtonCancelar.Text = "&Cancelar"
        '
        'ComboBoxGrupoCod
        '
        Me.ComboBoxGrupoCod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxGrupoCod.Enabled = False
        Me.ComboBoxGrupoCod.Location = New System.Drawing.Point(76, 12)
        Me.ComboBoxGrupoCod.Name = "ComboBoxGrupoCod"
        Me.ComboBoxGrupoCod.Size = New System.Drawing.Size(86, 21)
        Me.ComboBoxGrupoCod.TabIndex = 16
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(12, 12)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(64, 23)
        Me.Label10.TabIndex = 15
        Me.Label10.Text = "Grupo Cod"
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(12, 41)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(64, 23)
        Me.Label11.TabIndex = 17
        Me.Label11.Text = "Emp. Cod"
        '
        'TextBoxEmpCod
        '
        Me.TextBoxEmpCod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxEmpCod.Location = New System.Drawing.Point(76, 41)
        Me.TextBoxEmpCod.Name = "TextBoxEmpCod"
        Me.TextBoxEmpCod.Size = New System.Drawing.Size(86, 20)
        Me.TextBoxEmpCod.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(12, 71)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 23)
        Me.Label1.TabIndex = 37
        Me.Label1.Text = "Nombre"
        '
        'TextBoxDescripcion
        '
        Me.TextBoxDescripcion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDescripcion.Location = New System.Drawing.Point(76, 74)
        Me.TextBoxDescripcion.Name = "TextBoxDescripcion"
        Me.TextBoxDescripcion.Size = New System.Drawing.Size(353, 20)
        Me.TextBoxDescripcion.TabIndex = 2
        '
        'CheckBoxValoresPorDefecto
        '
        Me.CheckBoxValoresPorDefecto.AutoSize = True
        Me.CheckBoxValoresPorDefecto.Checked = True
        Me.CheckBoxValoresPorDefecto.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxValoresPorDefecto.Location = New System.Drawing.Point(442, 74)
        Me.CheckBoxValoresPorDefecto.Name = "CheckBoxValoresPorDefecto"
        Me.CheckBoxValoresPorDefecto.Size = New System.Drawing.Size(120, 17)
        Me.CheckBoxValoresPorDefecto.TabIndex = 38
        Me.CheckBoxValoresPorDefecto.Text = "Valores por Defecto"
        Me.CheckBoxValoresPorDefecto.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 116)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 13)
        Me.Label2.TabIndex = 39
        Me.Label2.Text = "Tipo de Empresa"
        '
        'CheckBoxTipoHotel
        '
        Me.CheckBoxTipoHotel.AutoSize = True
        Me.CheckBoxTipoHotel.Checked = True
        Me.CheckBoxTipoHotel.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxTipoHotel.Location = New System.Drawing.Point(111, 116)
        Me.CheckBoxTipoHotel.Name = "CheckBoxTipoHotel"
        Me.CheckBoxTipoHotel.Size = New System.Drawing.Size(51, 17)
        Me.CheckBoxTipoHotel.TabIndex = 40
        Me.CheckBoxTipoHotel.Text = "Hotel"
        Me.CheckBoxTipoHotel.UseVisualStyleBackColor = True
        '
        'CheckBoxTipoOtroTipo
        '
        Me.CheckBoxTipoOtroTipo.AutoSize = True
        Me.CheckBoxTipoOtroTipo.Location = New System.Drawing.Point(181, 116)
        Me.CheckBoxTipoOtroTipo.Name = "CheckBoxTipoOtroTipo"
        Me.CheckBoxTipoOtroTipo.Size = New System.Drawing.Size(70, 17)
        Me.CheckBoxTipoOtroTipo.TabIndex = 41
        Me.CheckBoxTipoOtroTipo.Text = "Otro Tipo"
        Me.CheckBoxTipoOtroTipo.UseVisualStyleBackColor = True
        '
        'FormInsertaEmpresa
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(580, 172)
        Me.Controls.Add(Me.CheckBoxTipoOtroTipo)
        Me.Controls.Add(Me.CheckBoxTipoHotel)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.CheckBoxValoresPorDefecto)
        Me.Controls.Add(Me.TextBoxDescripcion)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBoxEmpCod)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.ComboBoxGrupoCod)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ButtonCancelar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormInsertaEmpresa"
        Me.Text = "Empresa"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonCancelar As System.Windows.Forms.Button
    Friend WithEvents ComboBoxGrupoCod As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TextBoxEmpCod As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBoxDescripcion As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxValoresPorDefecto As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents CheckBoxTipoHotel As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxTipoOtroTipo As System.Windows.Forms.CheckBox
End Class
