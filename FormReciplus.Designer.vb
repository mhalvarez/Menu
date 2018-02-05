<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormReciplus
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormReciplus))
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextBoxFicheroEntrada = New System.Windows.Forms.TextBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonCancelar = New System.Windows.Forms.Button
        Me.TextBoxFicheroSalida = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ListBoxInput = New System.Windows.Forms.ListBox
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.ButtonFicheroEntrada = New System.Windows.Forms.Button
        Me.ButtonFicheroSalida = New System.Windows.Forms.Button
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.ListBoxOutPut = New System.Windows.Forms.ListBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.TextBoxGrupoEmpresa = New System.Windows.Forms.TextBox
        Me.TextBoxEmpresaParametros = New System.Windows.Forms.TextBox
        Me.TextBoxLibroIva = New System.Windows.Forms.TextBox
        Me.TextBoxClaseIva = New System.Windows.Forms.TextBox
        Me.TextBoxMoneda = New System.Windows.Forms.TextBox
        Me.TextBoxDiario = New System.Windows.Forms.TextBox
        Me.ButtonLeeParametros = New System.Windows.Forms.Button
        Me.TextBoxEmpresa = New System.Windows.Forms.TextBox
        Me.TextBoxSerie = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.ListBoxSpyro = New System.Windows.Forms.ListBox
        Me.TextBoxTipoIva1 = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.TextBoxTipoIva2 = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.TextBoxSerieNotasdeCredito = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.TextBoxDebug = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 13)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "Fichero de Entrada"
        '
        'TextBoxFicheroEntrada
        '
        Me.TextBoxFicheroEntrada.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxFicheroEntrada.Location = New System.Drawing.Point(126, 7)
        Me.TextBoxFicheroEntrada.Name = "TextBoxFicheroEntrada"
        Me.TextBoxFicheroEntrada.Size = New System.Drawing.Size(402, 20)
        Me.TextBoxFicheroEntrada.TabIndex = 0
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Location = New System.Drawing.Point(672, 23)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(75, 23)
        Me.ButtonAceptar.TabIndex = 3
        Me.ButtonAceptar.Text = "&Aceptar"
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ButtonCancelar
        '
        Me.ButtonCancelar.Location = New System.Drawing.Point(672, 62)
        Me.ButtonCancelar.Name = "ButtonCancelar"
        Me.ButtonCancelar.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCancelar.TabIndex = 4
        Me.ButtonCancelar.Text = "&Cancelar"
        Me.ButtonCancelar.UseVisualStyleBackColor = True
        '
        'TextBoxFicheroSalida
        '
        Me.TextBoxFicheroSalida.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxFicheroSalida.Location = New System.Drawing.Point(126, 39)
        Me.TextBoxFicheroSalida.Name = "TextBoxFicheroSalida"
        Me.TextBoxFicheroSalida.Size = New System.Drawing.Size(402, 20)
        Me.TextBoxFicheroSalida.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Fichero de Salida"
        '
        'ListBoxInput
        '
        Me.ListBoxInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListBoxInput.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBoxInput.FormattingEnabled = True
        Me.ListBoxInput.HorizontalExtent = 5000
        Me.ListBoxInput.HorizontalScrollbar = True
        Me.ListBoxInput.ItemHeight = 14
        Me.ListBoxInput.Location = New System.Drawing.Point(12, 117)
        Me.ListBoxInput.Name = "ListBoxInput"
        Me.ListBoxInput.Size = New System.Drawing.Size(512, 114)
        Me.ListBoxInput.TabIndex = 8
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'ButtonFicheroEntrada
        '
        Me.ButtonFicheroEntrada.Image = CType(resources.GetObject("ButtonFicheroEntrada.Image"), System.Drawing.Image)
        Me.ButtonFicheroEntrada.Location = New System.Drawing.Point(537, 7)
        Me.ButtonFicheroEntrada.Name = "ButtonFicheroEntrada"
        Me.ButtonFicheroEntrada.Size = New System.Drawing.Size(44, 23)
        Me.ButtonFicheroEntrada.TabIndex = 1
        Me.ButtonFicheroEntrada.UseVisualStyleBackColor = True
        '
        'ButtonFicheroSalida
        '
        Me.ButtonFicheroSalida.Image = CType(resources.GetObject("ButtonFicheroSalida.Image"), System.Drawing.Image)
        Me.ButtonFicheroSalida.Location = New System.Drawing.Point(537, 36)
        Me.ButtonFicheroSalida.Name = "ButtonFicheroSalida"
        Me.ButtonFicheroSalida.Size = New System.Drawing.Size(44, 23)
        Me.ButtonFicheroSalida.TabIndex = 3
        Me.ButtonFicheroSalida.UseVisualStyleBackColor = True
        '
        'ListBoxOutPut
        '
        Me.ListBoxOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListBoxOutPut.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBoxOutPut.FormattingEnabled = True
        Me.ListBoxOutPut.HorizontalExtent = 5000
        Me.ListBoxOutPut.HorizontalScrollbar = True
        Me.ListBoxOutPut.ItemHeight = 14
        Me.ListBoxOutPut.Location = New System.Drawing.Point(12, 266)
        Me.ListBoxOutPut.Name = "ListBoxOutPut"
        Me.ListBoxOutPut.Size = New System.Drawing.Size(512, 114)
        Me.ListBoxOutPut.TabIndex = 11
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(550, 107)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 13)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "Fecha Asiento"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(550, 133)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(80, 13)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Grupo Empresa"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(550, 159)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(48, 13)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Empresa"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(550, 185)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(48, 13)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "Libro Iva"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(550, 211)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(51, 13)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "Clase Iva"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(552, 312)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 17
        Me.Label8.Text = "Moneda"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(552, 338)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(34, 13)
        Me.Label9.TabIndex = 18
        Me.Label9.Text = "Diario"
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.CustomFormat = "dd/MM/yyyy"
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker1.Location = New System.Drawing.Point(631, 103)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(116, 20)
        Me.DateTimePicker1.TabIndex = 4
        Me.DateTimePicker1.Value = New Date(2006, 4, 10, 0, 0, 0, 0)
        '
        'TextBoxGrupoEmpresa
        '
        Me.TextBoxGrupoEmpresa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxGrupoEmpresa.Location = New System.Drawing.Point(631, 133)
        Me.TextBoxGrupoEmpresa.Name = "TextBoxGrupoEmpresa"
        Me.TextBoxGrupoEmpresa.ReadOnly = True
        Me.TextBoxGrupoEmpresa.Size = New System.Drawing.Size(116, 20)
        Me.TextBoxGrupoEmpresa.TabIndex = 5
        '
        'TextBoxEmpresaParametros
        '
        Me.TextBoxEmpresaParametros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxEmpresaParametros.Location = New System.Drawing.Point(687, 159)
        Me.TextBoxEmpresaParametros.Name = "TextBoxEmpresaParametros"
        Me.TextBoxEmpresaParametros.Size = New System.Drawing.Size(25, 20)
        Me.TextBoxEmpresaParametros.TabIndex = 7
        Me.TextBoxEmpresaParametros.Text = "02"
        '
        'TextBoxLibroIva
        '
        Me.TextBoxLibroIva.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxLibroIva.Location = New System.Drawing.Point(631, 185)
        Me.TextBoxLibroIva.Name = "TextBoxLibroIva"
        Me.TextBoxLibroIva.Size = New System.Drawing.Size(116, 20)
        Me.TextBoxLibroIva.TabIndex = 8
        '
        'TextBoxClaseIva
        '
        Me.TextBoxClaseIva.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxClaseIva.Location = New System.Drawing.Point(631, 211)
        Me.TextBoxClaseIva.Name = "TextBoxClaseIva"
        Me.TextBoxClaseIva.Size = New System.Drawing.Size(116, 20)
        Me.TextBoxClaseIva.TabIndex = 9
        '
        'TextBoxMoneda
        '
        Me.TextBoxMoneda.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxMoneda.Location = New System.Drawing.Point(630, 312)
        Me.TextBoxMoneda.Name = "TextBoxMoneda"
        Me.TextBoxMoneda.Size = New System.Drawing.Size(116, 20)
        Me.TextBoxMoneda.TabIndex = 12
        '
        'TextBoxDiario
        '
        Me.TextBoxDiario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDiario.Location = New System.Drawing.Point(630, 338)
        Me.TextBoxDiario.Name = "TextBoxDiario"
        Me.TextBoxDiario.Size = New System.Drawing.Size(116, 20)
        Me.TextBoxDiario.TabIndex = 13
        '
        'ButtonLeeParametros
        '
        Me.ButtonLeeParametros.Image = CType(resources.GetObject("ButtonLeeParametros.Image"), System.Drawing.Image)
        Me.ButtonLeeParametros.Location = New System.Drawing.Point(718, 159)
        Me.ButtonLeeParametros.Name = "ButtonLeeParametros"
        Me.ButtonLeeParametros.Size = New System.Drawing.Size(29, 23)
        Me.ButtonLeeParametros.TabIndex = 26
        Me.ButtonLeeParametros.Text = ":::"
        Me.ButtonLeeParametros.UseVisualStyleBackColor = True
        '
        'TextBoxEmpresa
        '
        Me.TextBoxEmpresa.BackColor = System.Drawing.Color.IndianRed
        Me.TextBoxEmpresa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxEmpresa.ForeColor = System.Drawing.SystemColors.Window
        Me.TextBoxEmpresa.Location = New System.Drawing.Point(631, 159)
        Me.TextBoxEmpresa.Name = "TextBoxEmpresa"
        Me.TextBoxEmpresa.Size = New System.Drawing.Size(35, 20)
        Me.TextBoxEmpresa.TabIndex = 6
        '
        'TextBoxSerie
        '
        Me.TextBoxSerie.BackColor = System.Drawing.Color.IndianRed
        Me.TextBoxSerie.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxSerie.ForeColor = System.Drawing.SystemColors.Window
        Me.TextBoxSerie.Location = New System.Drawing.Point(630, 375)
        Me.TextBoxSerie.Name = "TextBoxSerie"
        Me.TextBoxSerie.Size = New System.Drawing.Size(116, 20)
        Me.TextBoxSerie.TabIndex = 14
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(552, 375)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(75, 13)
        Me.Label10.TabIndex = 28
        Me.Label10.Text = "Serie Facturas"
        '
        'ListBoxSpyro
        '
        Me.ListBoxSpyro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListBoxSpyro.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBoxSpyro.FormattingEnabled = True
        Me.ListBoxSpyro.HorizontalExtent = 5000
        Me.ListBoxSpyro.HorizontalScrollbar = True
        Me.ListBoxSpyro.ItemHeight = 14
        Me.ListBoxSpyro.Location = New System.Drawing.Point(12, 419)
        Me.ListBoxSpyro.Name = "ListBoxSpyro"
        Me.ListBoxSpyro.Size = New System.Drawing.Size(512, 114)
        Me.ListBoxSpyro.TabIndex = 30
        '
        'TextBoxTipoIva1
        '
        Me.TextBoxTipoIva1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxTipoIva1.Location = New System.Drawing.Point(631, 239)
        Me.TextBoxTipoIva1.Name = "TextBoxTipoIva1"
        Me.TextBoxTipoIva1.Size = New System.Drawing.Size(116, 20)
        Me.TextBoxTipoIva1.TabIndex = 10
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(550, 239)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(46, 13)
        Me.Label11.TabIndex = 32
        Me.Label11.Text = "Tipo Iva"
        '
        'TextBoxTipoIva2
        '
        Me.TextBoxTipoIva2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxTipoIva2.Location = New System.Drawing.Point(630, 266)
        Me.TextBoxTipoIva2.Name = "TextBoxTipoIva2"
        Me.TextBoxTipoIva2.Size = New System.Drawing.Size(116, 20)
        Me.TextBoxTipoIva2.TabIndex = 11
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(552, 266)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(46, 13)
        Me.Label12.TabIndex = 34
        Me.Label12.Text = "Tipo Iva"
        '
        'TextBoxSerieNotasdeCredito
        '
        Me.TextBoxSerieNotasdeCredito.BackColor = System.Drawing.Color.IndianRed
        Me.TextBoxSerieNotasdeCredito.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxSerieNotasdeCredito.ForeColor = System.Drawing.SystemColors.Window
        Me.TextBoxSerieNotasdeCredito.Location = New System.Drawing.Point(630, 404)
        Me.TextBoxSerieNotasdeCredito.Name = "TextBoxSerieNotasdeCredito"
        Me.TextBoxSerieNotasdeCredito.Size = New System.Drawing.Size(116, 20)
        Me.TextBoxSerieNotasdeCredito.TabIndex = 15
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(552, 404)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(70, 13)
        Me.Label13.TabIndex = 36
        Me.Label13.Text = "Serie Abonos"
        '
        'TextBoxDebug
        '
        Me.TextBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDebug.Location = New System.Drawing.Point(127, 65)
        Me.TextBoxDebug.Name = "TextBoxDebug"
        Me.TextBoxDebug.ReadOnly = True
        Me.TextBoxDebug.Size = New System.Drawing.Size(402, 20)
        Me.TextBoxDebug.TabIndex = 38
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.ForeColor = System.Drawing.Color.Maroon
        Me.Label14.Location = New System.Drawing.Point(11, 97)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(97, 13)
        Me.Label14.TabIndex = 39
        Me.Label14.Text = "Fichero de Entrada"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.ForeColor = System.Drawing.Color.Maroon
        Me.Label15.Location = New System.Drawing.Point(12, 241)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(74, 13)
        Me.Label15.TabIndex = 40
        Me.Label15.Text = "Tipo de Datos"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.ForeColor = System.Drawing.Color.Maroon
        Me.Label16.Location = New System.Drawing.Point(12, 394)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(61, 13)
        Me.Label16.TabIndex = 41
        Me.Label16.Text = "Incidencias"
        '
        'FormReciplus
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(766, 545)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.TextBoxDebug)
        Me.Controls.Add(Me.TextBoxSerieNotasdeCredito)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.TextBoxTipoIva2)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.TextBoxTipoIva1)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.ListBoxSpyro)
        Me.Controls.Add(Me.TextBoxSerie)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.TextBoxEmpresa)
        Me.Controls.Add(Me.ButtonLeeParametros)
        Me.Controls.Add(Me.TextBoxDiario)
        Me.Controls.Add(Me.TextBoxMoneda)
        Me.Controls.Add(Me.TextBoxClaseIva)
        Me.Controls.Add(Me.TextBoxLibroIva)
        Me.Controls.Add(Me.TextBoxEmpresaParametros)
        Me.Controls.Add(Me.TextBoxGrupoEmpresa)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ListBoxOutPut)
        Me.Controls.Add(Me.ButtonFicheroSalida)
        Me.Controls.Add(Me.ButtonFicheroEntrada)
        Me.Controls.Add(Me.ListBoxInput)
        Me.Controls.Add(Me.TextBoxFicheroSalida)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ButtonCancelar)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.TextBoxFicheroEntrada)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormReciplus"
        Me.Text = "Reciplus"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBoxFicheroEntrada As System.Windows.Forms.TextBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonCancelar As System.Windows.Forms.Button
    Friend WithEvents TextBoxFicheroSalida As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ListBoxInput As System.Windows.Forms.ListBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ButtonFicheroEntrada As System.Windows.Forms.Button
    Friend WithEvents ButtonFicheroSalida As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents ListBoxOutPut As System.Windows.Forms.ListBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents TextBoxGrupoEmpresa As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxEmpresaParametros As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxLibroIva As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxClaseIva As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxMoneda As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxDiario As System.Windows.Forms.TextBox
    Friend WithEvents ButtonLeeParametros As System.Windows.Forms.Button
    Friend WithEvents TextBoxEmpresa As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxSerie As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents ListBoxSpyro As System.Windows.Forms.ListBox
    Friend WithEvents TextBoxTipoIva1 As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TextBoxTipoIva2 As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TextBoxSerieNotasdeCredito As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TextBoxDebug As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
End Class
