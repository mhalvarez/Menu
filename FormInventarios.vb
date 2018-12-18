Public Class FormInventarios
    Inherits System.Windows.Forms.Form
    Private DbAlmacen As C_DATOS.C_DatosOledb
    Private SQL As String
    Dim MyIni As New cIniArray
    Private StrConexion As String
    Private StrConexionSpyro As String
    Private EmpGrupoCod As String
    Private EmpCod As String
    Private EmpNum As Integer

#Region " Código generado por el Diseñador de Windows Forms "

    Public Sub New(ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vStrConexion As String, ByVal vStrConexionSpyro As String, vEmpNum As Integer)
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()
        Me.StrConexion = vStrConexion
        Me.EmpGrupoCod = vEmpGrupoCod
        Me.EmpCod = vEmpCod
        Me.StrConexionSpyro = vStrConexionSpyro

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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents DataGrid1 As System.Windows.Forms.DataGrid
    Friend WithEvents TextBoxEmpGrupo As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxEmp As System.Windows.Forms.TextBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonCancelar As System.Windows.Forms.Button
    Friend WithEvents LabelDebug As System.Windows.Forms.Label
    Friend WithEvents TextBoxDebug As System.Windows.Forms.TextBox
    Friend WithEvents ListBoxDebug As System.Windows.Forms.ListBox
    Friend WithEvents CheckBoxSoloIniciales As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.TextBoxEmp = New System.Windows.Forms.TextBox
        Me.TextBoxEmpGrupo = New System.Windows.Forms.TextBox
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.LabelDebug = New System.Windows.Forms.Label
        Me.ButtonCancelar = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.DataGrid1 = New System.Windows.Forms.DataGrid
        Me.TextBoxDebug = New System.Windows.Forms.TextBox
        Me.ListBoxDebug = New System.Windows.Forms.ListBox
        Me.CheckBoxSoloIniciales = New System.Windows.Forms.CheckBox
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Mes Inventariado"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.CheckBoxSoloIniciales)
        Me.GroupBox1.Controls.Add(Me.TextBoxEmp)
        Me.GroupBox1.Controls.Add(Me.TextBoxEmpGrupo)
        Me.GroupBox1.Controls.Add(Me.DateTimePicker1)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(586, 48)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        '
        'TextBoxEmp
        '
        Me.TextBoxEmp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxEmp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxEmp.Location = New System.Drawing.Point(530, 16)
        Me.TextBoxEmp.Name = "TextBoxEmp"
        Me.TextBoxEmp.ReadOnly = True
        Me.TextBoxEmp.Size = New System.Drawing.Size(48, 20)
        Me.TextBoxEmp.TabIndex = 3
        Me.TextBoxEmp.Text = "1"
        '
        'TextBoxEmpGrupo
        '
        Me.TextBoxEmpGrupo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxEmpGrupo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxEmpGrupo.Location = New System.Drawing.Point(474, 16)
        Me.TextBoxEmpGrupo.Name = "TextBoxEmpGrupo"
        Me.TextBoxEmpGrupo.ReadOnly = True
        Me.TextBoxEmpGrupo.Size = New System.Drawing.Size(56, 20)
        Me.TextBoxEmpGrupo.TabIndex = 2
        Me.TextBoxEmpGrupo.Text = "HINS"
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.CustomFormat = "dd/MM/yyyy"
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker1.Location = New System.Drawing.Point(104, 16)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(120, 20)
        Me.DateTimePicker1.TabIndex = 1
        Me.DateTimePicker1.Value = New Date(2006, 11, 2, 0, 0, 0, 0)
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.LabelDebug)
        Me.GroupBox2.Controls.Add(Me.ButtonCancelar)
        Me.GroupBox2.Controls.Add(Me.ButtonAceptar)
        Me.GroupBox2.Controls.Add(Me.DataGrid1)
        Me.GroupBox2.Location = New System.Drawing.Point(8, 56)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(584, 208)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        '
        'LabelDebug
        '
        Me.LabelDebug.Location = New System.Drawing.Point(8, 176)
        Me.LabelDebug.Name = "LabelDebug"
        Me.LabelDebug.Size = New System.Drawing.Size(464, 23)
        Me.LabelDebug.TabIndex = 3
        '
        'ButtonCancelar
        '
        Me.ButtonCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancelar.Location = New System.Drawing.Point(480, 56)
        Me.ButtonCancelar.Name = "ButtonCancelar"
        Me.ButtonCancelar.Size = New System.Drawing.Size(96, 23)
        Me.ButtonCancelar.TabIndex = 2
        Me.ButtonCancelar.Text = "&Cerrar"
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonAceptar.Location = New System.Drawing.Point(480, 24)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(96, 23)
        Me.ButtonAceptar.TabIndex = 1
        Me.ButtonAceptar.Text = "&Aceptar"
        '
        'DataGrid1
        '
        Me.DataGrid1.CaptionFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGrid1.DataMember = ""
        Me.DataGrid1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGrid1.Location = New System.Drawing.Point(8, 24)
        Me.DataGrid1.Name = "DataGrid1"
        Me.DataGrid1.ReadOnly = True
        Me.DataGrid1.Size = New System.Drawing.Size(464, 152)
        Me.DataGrid1.TabIndex = 0
        '
        'TextBoxDebug
        '
        Me.TextBoxDebug.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDebug.ForeColor = System.Drawing.Color.SteelBlue
        Me.TextBoxDebug.Location = New System.Drawing.Point(8, 272)
        Me.TextBoxDebug.Name = "TextBoxDebug"
        Me.TextBoxDebug.Size = New System.Drawing.Size(584, 20)
        Me.TextBoxDebug.TabIndex = 18
        Me.TextBoxDebug.Text = ""
        '
        'ListBoxDebug
        '
        Me.ListBoxDebug.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListBoxDebug.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBoxDebug.ForeColor = System.Drawing.Color.SteelBlue
        Me.ListBoxDebug.Location = New System.Drawing.Point(8, 296)
        Me.ListBoxDebug.Name = "ListBoxDebug"
        Me.ListBoxDebug.Size = New System.Drawing.Size(584, 80)
        Me.ListBoxDebug.TabIndex = 21
        '
        'CheckBoxSoloIniciales
        '
        Me.CheckBoxSoloIniciales.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBoxSoloIniciales.ForeColor = System.Drawing.Color.Maroon
        Me.CheckBoxSoloIniciales.Location = New System.Drawing.Point(232, 16)
        Me.CheckBoxSoloIniciales.Name = "CheckBoxSoloIniciales"
        Me.CheckBoxSoloIniciales.Size = New System.Drawing.Size(216, 24)
        Me.CheckBoxSoloIniciales.TabIndex = 4
        Me.CheckBoxSoloIniciales.Text = "Solo Inventarios Iniciales"
        '
        'FormInventarios
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.ButtonCancelar
        Me.ClientSize = New System.Drawing.Size(602, 383)
        Me.Controls.Add(Me.ListBoxDebug)
        Me.Controls.Add(Me.TextBoxDebug)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormInventarios"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Inventarios"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Private Sub FormInventarios_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            Me.DbAlmacen = New C_DATOS.C_DatosOledb(Me.StrConexion)
            Me.DbAlmacen.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.BuscaInventarios()

            Me.DateTimePicker1.Value = Now

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub


    Private Sub BuscaInventarios()
        Try
            SQL = "SELECT INVG_CODI,INVG_ANCI,INVG_DAVA,TNST_ALMA.ALMA_DESC FROM TNST_INVG,TNST_ALMA "
            SQL += "WHERE TNST_INVG.ALMA_CODI = TNST_ALMA.ALMA_CODI "
            SQL += "AND TO_CHAR(INVG_DAVA, 'MM') = " & Format(Me.DateTimePicker1.Value, "MM")
            SQL += " AND TO_CHAR(INVG_DAVA, 'YYYY') = " & Format(Me.DateTimePicker1.Value, "yyyy")
            SQL += " ORDER BY INVG_CODI,INVG_ANCI ASC"

            Me.DbAlmacen.TraerDataset(SQL, "INVENTARIOS")
            Me.DataGrid1.DataSource = Me.DbAlmacen.mDbDataset
            Me.DataGrid1.DataMember = "INVENTARIOS"

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Busca Inventarios")

        End Try

    End Sub
    Private Sub CerrarConexiones()
        If Me.DbAlmacen.EstadoConexion = ConnectionState.Open = True Then
            Me.DbAlmacen.CerrarConexion()
        End If
    End Sub

    Private Sub FormInventarios_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Me.CerrarConexiones()
    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try
            Dim INTEGRA As New IntegraAlmacen.IntegraAlmacen(Me.EmpGrupoCod,
                  Me.EmpCod, MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"),
                   Me.StrConexion, Format(Me.DateTimePicker1.Value, "dd-MM-yyyy"), "", False, Me.TextBoxDebug, Me.ListBoxDebug, Me.StrConexionSpyro, True, Me.CheckBoxSoloIniciales.Checked, Me.EmpNum)
            Me.Cursor = Cursors.WaitCursor
            INTEGRA.ProcesarInventario()
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker1.ValueChanged
        If IsNothing(Me.DbAlmacen) = False Then
            Me.BuscaInventarios()
            Me.LabelDebug.Text = DateTime.DaysInMonth(Me.DateTimePicker1.Value.Year, Me.DateTimePicker1.Value.Month)
            Me.LabelDebug.Update()
        End If

    End Sub
End Class
