Public Class DialogError
    Inherits System.Windows.Forms.Form

#Region " Código generado por el Diseñador de Windows Forms "

    Public Sub New()
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()

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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
    'Puede modificarse utilizando el Diseñador de Windows Forms. 
    'No lo modifique con el editor de código.
    Friend WithEvents TextBoxProcedimiento As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxError As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxSql As System.Windows.Forms.TextBox
    Friend WithEvents ButtonTerminar As System.Windows.Forms.Button
    Friend WithEvents ButtonContinuar As System.Windows.Forms.Button
    Friend WithEvents LinkLabelError As System.Windows.Forms.LinkLabel
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.TextBoxProcedimiento = New System.Windows.Forms.TextBox
        Me.TextBoxError = New System.Windows.Forms.TextBox
        Me.TextBoxSql = New System.Windows.Forms.TextBox
        Me.LinkLabelError = New System.Windows.Forms.LinkLabel
        Me.ButtonTerminar = New System.Windows.Forms.Button
        Me.ButtonContinuar = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'TextBoxProcedimiento
        '
        Me.TextBoxProcedimiento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxProcedimiento.Location = New System.Drawing.Point(16, 16)
        Me.TextBoxProcedimiento.Name = "TextBoxProcedimiento"
        Me.TextBoxProcedimiento.Size = New System.Drawing.Size(616, 20)
        Me.TextBoxProcedimiento.TabIndex = 0
        '
        'TextBoxError
        '
        Me.TextBoxError.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxError.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxError.Location = New System.Drawing.Point(16, 48)
        Me.TextBoxError.Multiline = True
        Me.TextBoxError.Name = "TextBoxError"
        Me.TextBoxError.Size = New System.Drawing.Size(616, 112)
        Me.TextBoxError.TabIndex = 1
        '
        'TextBoxSql
        '
        Me.TextBoxSql.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxSql.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxSql.Location = New System.Drawing.Point(16, 168)
        Me.TextBoxSql.Multiline = True
        Me.TextBoxSql.Name = "TextBoxSql"
        Me.TextBoxSql.Size = New System.Drawing.Size(616, 56)
        Me.TextBoxSql.TabIndex = 2
        '
        'LinkLabelError
        '
        Me.LinkLabelError.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LinkLabelError.Location = New System.Drawing.Point(8, 232)
        Me.LinkLabelError.Name = "LinkLabelError"
        Me.LinkLabelError.Size = New System.Drawing.Size(512, 23)
        Me.LinkLabelError.TabIndex = 3
        Me.LinkLabelError.TabStop = True
        Me.LinkLabelError.Text = "LinkLabel1"
        '
        'ButtonTerminar
        '
        Me.ButtonTerminar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonTerminar.Location = New System.Drawing.Point(526, 227)
        Me.ButtonTerminar.Name = "ButtonTerminar"
        Me.ButtonTerminar.Size = New System.Drawing.Size(106, 23)
        Me.ButtonTerminar.TabIndex = 4
        Me.ButtonTerminar.Text = "&Terminar"
        Me.ButtonTerminar.UseVisualStyleBackColor = True
        '
        'ButtonContinuar
        '
        Me.ButtonContinuar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonContinuar.Location = New System.Drawing.Point(429, 228)
        Me.ButtonContinuar.Name = "ButtonContinuar"
        Me.ButtonContinuar.Size = New System.Drawing.Size(91, 23)
        Me.ButtonContinuar.TabIndex = 5
        Me.ButtonContinuar.Text = "Continuar"
        Me.ButtonContinuar.UseVisualStyleBackColor = True
        '
        'DialogError
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(648, 273)
        Me.Controls.Add(Me.ButtonContinuar)
        Me.Controls.Add(Me.ButtonTerminar)
        Me.Controls.Add(Me.LinkLabelError)
        Me.Controls.Add(Me.TextBoxSql)
        Me.Controls.Add(Me.TextBoxError)
        Me.Controls.Add(Me.TextBoxProcedimiento)
        Me.MinimumSize = New System.Drawing.Size(656, 300)
        Me.Name = "DialogError"
        Me.Text = "DialogError"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region


    Private Sub ButtonTerminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTerminar.Click
        Try
            System.Windows.Forms.Application.DoEvents()
            System.Windows.Forms.Application.ExitThread()
            System.Windows.Forms.Application.Exit()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonContinuar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonContinuar.Click
        Try
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub
End Class
