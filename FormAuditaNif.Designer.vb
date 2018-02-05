<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormAuditaNif
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
        Me.TextBoxDocumento = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextBoxTipo = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextBoxConcepto = New System.Windows.Forms.TextBox
        Me.TextBoxRecibo = New System.Windows.Forms.TextBox
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.TextBoxRecibo)
        Me.GroupBox1.Controls.Add(Me.TextBoxConcepto)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.TextBoxDocumento)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.TextBoxTipo)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(584, 235)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'TextBoxDocumento
        '
        Me.TextBoxDocumento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDocumento.Location = New System.Drawing.Point(74, 49)
        Me.TextBoxDocumento.Name = "TextBoxDocumento"
        Me.TextBoxDocumento.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxDocumento.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Documento"
        '
        'TextBoxTipo
        '
        Me.TextBoxTipo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxTipo.Location = New System.Drawing.Point(74, 19)
        Me.TextBoxTipo.Name = "TextBoxTipo"
        Me.TextBoxTipo.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxTipo.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(28, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Tipo"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 88)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Concepto"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 117)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(41, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Recibo"
        '
        'TextBoxConcepto
        '
        Me.TextBoxConcepto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxConcepto.Location = New System.Drawing.Point(74, 81)
        Me.TextBoxConcepto.Name = "TextBoxConcepto"
        Me.TextBoxConcepto.Size = New System.Drawing.Size(483, 20)
        Me.TextBoxConcepto.TabIndex = 6
        '
        'TextBoxRecibo
        '
        Me.TextBoxRecibo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxRecibo.Location = New System.Drawing.Point(74, 115)
        Me.TextBoxRecibo.Name = "TextBoxRecibo"
        Me.TextBoxRecibo.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxRecibo.TabIndex = 7
        '
        'FormAuditaNif
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(609, 254)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "FormAuditaNif"
        Me.Text = "Auditoría de Nif"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBoxTipo As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBoxDocumento As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBoxRecibo As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxConcepto As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
