Public Class FormParametrosConexion
    Inherits System.Windows.Forms.Form

#Region " C�digo generado por el Dise�ador de Windows Forms "

    Public Sub New()
        MyBase.New()

        'El Dise�ador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicializaci�n despu�s de la llamada a InitializeComponent()

    End Sub

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Requerido por el Dise�ador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Dise�ador de Windows Forms requiere el siguiente procedimiento
    'Puede modificarse utilizando el Dise�ador de Windows Forms. 
    'No lo modifique con el editor de c�digo.
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonCancelar As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonCancelar = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Location = New System.Drawing.Point(648, 8)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.TabIndex = 0
        Me.ButtonAceptar.Text = "&Aceptar"
        '
        'ButtonCancelar
        '
        Me.ButtonCancelar.Location = New System.Drawing.Point(648, 40)
        Me.ButtonCancelar.Name = "ButtonCancelar"
        Me.ButtonCancelar.TabIndex = 1
        Me.ButtonCancelar.Text = "&Cancelar"
        '
        'FormParametrosConexion
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(744, 273)
        Me.Controls.Add(Me.ButtonCancelar)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Name = "FormParametrosConexion"
        Me.Text = "FormParametrosConexion"
        Me.ResumeLayout(False)

    End Sub

#End Region

End Class
