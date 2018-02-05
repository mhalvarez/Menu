<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMantenimiento_TH_IPOSVEN_AX
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
    Public Sub New(ByVal vStrConexion As String, ByVal vEmpGrupoCod As String)
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()
        Try
            If IsDBNull(vStrConexion) Then
                MsgBox("No se recibio Cadena de Conexión", MsgBoxStyle.Information, "Atención")
                Me.Close()
            End If

            If IsDBNull(vEmpGrupoCod) Then
                MsgBox("No se recibio Grupo de Empresas", MsgBoxStyle.Information, "Atención")
                Me.Close()
            End If

            'Agregar cualquier inicialización después de la llamada a InitializeComponent()
            Me.mStrConexion = vStrConexion

            Me.mEmpGrupoCod = vEmpGrupoCod

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormMantenimiento_TH_IPOSVEN_AX))
        Me.ToolStripDatos = New System.Windows.Forms.ToolStrip
        Me.ToolStripButtonPrimero = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButtonAnterior = New System.Windows.Forms.ToolStripButton
        Me.ToolStripTextBoxPosicion = New System.Windows.Forms.ToolStripTextBox
        Me.ToolStripButtonSiguiente = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButtonUltimo = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripButtonNuevo = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButtonEliminar = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButtonGrabar = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButtonActualizar = New System.Windows.Forms.ToolStripButton
        Me.GroupBoxFiltro = New System.Windows.Forms.GroupBox
        Me.ComboBoxGrupoCod = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.CheckBoxActualizaAutomatico = New System.Windows.Forms.CheckBox
        Me.ComboBoxEmpCod = New System.Windows.Forms.ComboBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.GroupBoxDatos = New System.Windows.Forms.GroupBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextBoxTipoPuntoVenta = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextBoxCodigoAlmacenAx = New System.Windows.Forms.TextBox
        Me.TextBoxDescripcion = New System.Windows.Forms.TextBox
        Me.TextBoxIposCodi = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.ToolStripDatos.SuspendLayout()
        Me.GroupBoxFiltro.SuspendLayout()
        Me.GroupBoxDatos.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStripDatos
        '
        Me.ToolStripDatos.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButtonPrimero, Me.ToolStripButtonAnterior, Me.ToolStripTextBoxPosicion, Me.ToolStripButtonSiguiente, Me.ToolStripButtonUltimo, Me.ToolStripSeparator1, Me.ToolStripButtonNuevo, Me.ToolStripButtonEliminar, Me.ToolStripButtonGrabar, Me.ToolStripButtonActualizar})
        Me.ToolStripDatos.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.ToolStripDatos.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripDatos.Name = "ToolStripDatos"
        Me.ToolStripDatos.Size = New System.Drawing.Size(694, 25)
        Me.ToolStripDatos.TabIndex = 1
        Me.ToolStripDatos.Text = "ToolStrip1"
        '
        'ToolStripButtonPrimero
        '
        Me.ToolStripButtonPrimero.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonPrimero.Image = CType(resources.GetObject("ToolStripButtonPrimero.Image"), System.Drawing.Image)
        Me.ToolStripButtonPrimero.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonPrimero.Name = "ToolStripButtonPrimero"
        Me.ToolStripButtonPrimero.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonPrimero.Text = "ToolStripButton1"
        Me.ToolStripButtonPrimero.ToolTipText = "Primer Registro"
        '
        'ToolStripButtonAnterior
        '
        Me.ToolStripButtonAnterior.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonAnterior.Image = CType(resources.GetObject("ToolStripButtonAnterior.Image"), System.Drawing.Image)
        Me.ToolStripButtonAnterior.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonAnterior.Name = "ToolStripButtonAnterior"
        Me.ToolStripButtonAnterior.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonAnterior.Text = "ToolStripButton1"
        Me.ToolStripButtonAnterior.ToolTipText = "Anterior"
        '
        'ToolStripTextBoxPosicion
        '
        Me.ToolStripTextBoxPosicion.Name = "ToolStripTextBoxPosicion"
        Me.ToolStripTextBoxPosicion.Size = New System.Drawing.Size(100, 25)
        '
        'ToolStripButtonSiguiente
        '
        Me.ToolStripButtonSiguiente.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonSiguiente.Image = CType(resources.GetObject("ToolStripButtonSiguiente.Image"), System.Drawing.Image)
        Me.ToolStripButtonSiguiente.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonSiguiente.Name = "ToolStripButtonSiguiente"
        Me.ToolStripButtonSiguiente.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonSiguiente.Text = "ToolStripButton1"
        Me.ToolStripButtonSiguiente.ToolTipText = "Siguiente"
        '
        'ToolStripButtonUltimo
        '
        Me.ToolStripButtonUltimo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonUltimo.Image = CType(resources.GetObject("ToolStripButtonUltimo.Image"), System.Drawing.Image)
        Me.ToolStripButtonUltimo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonUltimo.Name = "ToolStripButtonUltimo"
        Me.ToolStripButtonUltimo.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonUltimo.Text = "ToolStripButton1"
        Me.ToolStripButtonUltimo.ToolTipText = "Último Registro"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripButtonNuevo
        '
        Me.ToolStripButtonNuevo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonNuevo.Image = CType(resources.GetObject("ToolStripButtonNuevo.Image"), System.Drawing.Image)
        Me.ToolStripButtonNuevo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonNuevo.Name = "ToolStripButtonNuevo"
        Me.ToolStripButtonNuevo.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonNuevo.Text = "ToolStripButton1"
        Me.ToolStripButtonNuevo.ToolTipText = "Nuevo"
        '
        'ToolStripButtonEliminar
        '
        Me.ToolStripButtonEliminar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonEliminar.Image = CType(resources.GetObject("ToolStripButtonEliminar.Image"), System.Drawing.Image)
        Me.ToolStripButtonEliminar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonEliminar.Name = "ToolStripButtonEliminar"
        Me.ToolStripButtonEliminar.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonEliminar.Text = "ToolStripButton1"
        Me.ToolStripButtonEliminar.ToolTipText = "Eliminar"
        '
        'ToolStripButtonGrabar
        '
        Me.ToolStripButtonGrabar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonGrabar.Image = CType(resources.GetObject("ToolStripButtonGrabar.Image"), System.Drawing.Image)
        Me.ToolStripButtonGrabar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonGrabar.Name = "ToolStripButtonGrabar"
        Me.ToolStripButtonGrabar.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonGrabar.Text = "ToolStripButton1"
        Me.ToolStripButtonGrabar.ToolTipText = "Grabar"
        '
        'ToolStripButtonActualizar
        '
        Me.ToolStripButtonActualizar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonActualizar.Image = CType(resources.GetObject("ToolStripButtonActualizar.Image"), System.Drawing.Image)
        Me.ToolStripButtonActualizar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonActualizar.Name = "ToolStripButtonActualizar"
        Me.ToolStripButtonActualizar.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonActualizar.Text = "ToolStripButton1"
        Me.ToolStripButtonActualizar.ToolTipText = "Actualizar en Base"
        '
        'GroupBoxFiltro
        '
        Me.GroupBoxFiltro.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxFiltro.Controls.Add(Me.ComboBoxGrupoCod)
        Me.GroupBoxFiltro.Controls.Add(Me.Label10)
        Me.GroupBoxFiltro.Controls.Add(Me.CheckBoxActualizaAutomatico)
        Me.GroupBoxFiltro.Controls.Add(Me.ComboBoxEmpCod)
        Me.GroupBoxFiltro.Controls.Add(Me.Label11)
        Me.GroupBoxFiltro.Location = New System.Drawing.Point(8, 32)
        Me.GroupBoxFiltro.Name = "GroupBoxFiltro"
        Me.GroupBoxFiltro.Size = New System.Drawing.Size(682, 55)
        Me.GroupBoxFiltro.TabIndex = 3
        Me.GroupBoxFiltro.TabStop = False
        Me.GroupBoxFiltro.Text = "Filtro"
        '
        'ComboBoxGrupoCod
        '
        Me.ComboBoxGrupoCod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxGrupoCod.Location = New System.Drawing.Point(48, 16)
        Me.ComboBoxGrupoCod.Name = "ComboBoxGrupoCod"
        Me.ComboBoxGrupoCod.Size = New System.Drawing.Size(96, 21)
        Me.ComboBoxGrupoCod.TabIndex = 12
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(8, 16)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(48, 23)
        Me.Label10.TabIndex = 11
        Me.Label10.Text = "Grupo cod"
        '
        'CheckBoxActualizaAutomatico
        '
        Me.CheckBoxActualizaAutomatico.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxActualizaAutomatico.AutoSize = True
        Me.CheckBoxActualizaAutomatico.Checked = True
        Me.CheckBoxActualizaAutomatico.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxActualizaAutomatico.Location = New System.Drawing.Point(458, 16)
        Me.CheckBoxActualizaAutomatico.Name = "CheckBoxActualizaAutomatico"
        Me.CheckBoxActualizaAutomatico.Size = New System.Drawing.Size(214, 17)
        Me.CheckBoxActualizaAutomatico.TabIndex = 10
        Me.CheckBoxActualizaAutomatico.Text = "Actualiza Automático si se ha Cambiado"
        Me.CheckBoxActualizaAutomatico.UseVisualStyleBackColor = True
        '
        'ComboBoxEmpCod
        '
        Me.ComboBoxEmpCod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxEmpCod.DropDownWidth = 250
        Me.ComboBoxEmpCod.Location = New System.Drawing.Point(192, 16)
        Me.ComboBoxEmpCod.Name = "ComboBoxEmpCod"
        Me.ComboBoxEmpCod.Size = New System.Drawing.Size(232, 21)
        Me.ComboBoxEmpCod.TabIndex = 9
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(152, 16)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(40, 23)
        Me.Label11.TabIndex = 8
        Me.Label11.Text = "Hotel"
        '
        'GroupBoxDatos
        '
        Me.GroupBoxDatos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxDatos.Controls.Add(Me.Label5)
        Me.GroupBoxDatos.Controls.Add(Me.TextBoxTipoPuntoVenta)
        Me.GroupBoxDatos.Controls.Add(Me.Label4)
        Me.GroupBoxDatos.Controls.Add(Me.TextBoxCodigoAlmacenAx)
        Me.GroupBoxDatos.Controls.Add(Me.TextBoxDescripcion)
        Me.GroupBoxDatos.Controls.Add(Me.TextBoxIposCodi)
        Me.GroupBoxDatos.Controls.Add(Me.Label3)
        Me.GroupBoxDatos.Controls.Add(Me.Label2)
        Me.GroupBoxDatos.Controls.Add(Me.Label1)
        Me.GroupBoxDatos.Location = New System.Drawing.Point(8, 88)
        Me.GroupBoxDatos.Name = "GroupBoxDatos"
        Me.GroupBoxDatos.Size = New System.Drawing.Size(682, 282)
        Me.GroupBoxDatos.TabIndex = 4
        Me.GroupBoxDatos.TabStop = False
        Me.GroupBoxDatos.Text = "Datos"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(264, 80)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(121, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "1=NewGolf   2=NewPos"
        '
        'TextBoxTipoPuntoVenta
        '
        Me.TextBoxTipoPuntoVenta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxTipoPuntoVenta.Location = New System.Drawing.Point(152, 72)
        Me.TextBoxTipoPuntoVenta.Name = "TextBoxTipoPuntoVenta"
        Me.TextBoxTipoPuntoVenta.Size = New System.Drawing.Size(104, 20)
        Me.TextBoxTipoPuntoVenta.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(8, 72)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(120, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Tipo de Punto de Venta"
        '
        'TextBoxCodigoAlmacenAx
        '
        Me.TextBoxCodigoAlmacenAx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxCodigoAlmacenAx.Location = New System.Drawing.Point(152, 96)
        Me.TextBoxCodigoAlmacenAx.Name = "TextBoxCodigoAlmacenAx"
        Me.TextBoxCodigoAlmacenAx.Size = New System.Drawing.Size(104, 20)
        Me.TextBoxCodigoAlmacenAx.TabIndex = 3
        '
        'TextBoxDescripcion
        '
        Me.TextBoxDescripcion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDescripcion.Location = New System.Drawing.Point(152, 48)
        Me.TextBoxDescripcion.Name = "TextBoxDescripcion"
        Me.TextBoxDescripcion.Size = New System.Drawing.Size(384, 20)
        Me.TextBoxDescripcion.TabIndex = 1
        '
        'TextBoxIposCodi
        '
        Me.TextBoxIposCodi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxIposCodi.Location = New System.Drawing.Point(152, 24)
        Me.TextBoxIposCodi.Name = "TextBoxIposCodi"
        Me.TextBoxIposCodi.Size = New System.Drawing.Size(104, 20)
        Me.TextBoxIposCodi.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 96)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(131, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Código de Almacén en AX"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Descripción"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(117, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Código Punto de Venta"
        '
        'FormMantenimiento_TH_IPOSVEN_AX
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(692, 366)
        Me.Controls.Add(Me.GroupBoxDatos)
        Me.Controls.Add(Me.GroupBoxFiltro)
        Me.Controls.Add(Me.ToolStripDatos)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(700, 400)
        Me.Name = "FormMantenimiento_TH_IPOSVEN_AX"
        Me.Text = "Puntos de Venta o Tiendas que generan Diario de Perdidas y Ganancias"
        Me.ToolStripDatos.ResumeLayout(False)
        Me.ToolStripDatos.PerformLayout()
        Me.GroupBoxFiltro.ResumeLayout(False)
        Me.GroupBoxFiltro.PerformLayout()
        Me.GroupBoxDatos.ResumeLayout(False)
        Me.GroupBoxDatos.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStripDatos As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButtonPrimero As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButtonAnterior As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripTextBoxPosicion As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStripButtonSiguiente As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButtonUltimo As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButtonNuevo As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButtonEliminar As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButtonGrabar As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButtonActualizar As System.Windows.Forms.ToolStripButton
    Friend WithEvents GroupBoxFiltro As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxActualizaAutomatico As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxEmpCod As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents GroupBoxDatos As System.Windows.Forms.GroupBox
    Friend WithEvents TextBoxCodigoAlmacenAx As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxDescripcion As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxIposCodi As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxGrupoCod As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextBoxTipoPuntoVenta As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
