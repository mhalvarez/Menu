Public Class FormCuentasServiciosBloque
    Inherits System.Windows.Forms.Form
    Dim MyIni As New cIniArray
    Dim DbIntegracion As C_DATOS.C_DatosOledb
    Dim DbHotel As C_DATOS.C_DatosOledb
    Dim DbHotelAux As C_DATOS.C_DatosOledb
    Dim SQL As String
    Dim HayRegistros As Boolean = False
    Dim HayCuentas As Boolean = False
    Private mStatusBar As StatusBar
    Private mEmpGrupoCod As String
    Friend WithEvents CheckBoxActualizaCuenta As System.Windows.Forms.CheckBox
    Private mEmpCod As String


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
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents DataGridHoteles As System.Windows.Forms.DataGrid
    Friend WithEvents ButtonCrearTablaServicios As System.Windows.Forms.Button
    Friend WithEvents DataGrid1 As System.Windows.Forms.DataGrid
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonCancelar As System.Windows.Forms.Button
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.ButtonCancelar = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.DataGridHoteles = New System.Windows.Forms.DataGrid
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.DataGrid1 = New System.Windows.Forms.DataGrid
        Me.ButtonCrearTablaServicios = New System.Windows.Forms.Button
        Me.CheckBoxActualizaCuenta = New System.Windows.Forms.CheckBox
        Me.GroupBox1.SuspendLayout()
        CType(Me.DataGridHoteles, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.ButtonCancelar)
        Me.GroupBox1.Controls.Add(Me.ButtonAceptar)
        Me.GroupBox1.Controls.Add(Me.DataGridHoteles)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(879, 120)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Empresas"
        '
        'ButtonCancelar
        '
        Me.ButtonCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonCancelar.Enabled = False
        Me.ButtonCancelar.Location = New System.Drawing.Point(791, 48)
        Me.ButtonCancelar.Name = "ButtonCancelar"
        Me.ButtonCancelar.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCancelar.TabIndex = 5
        Me.ButtonCancelar.Text = "&Cancelar"
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonAceptar.Location = New System.Drawing.Point(791, 16)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(75, 23)
        Me.ButtonAceptar.TabIndex = 4
        Me.ButtonAceptar.Text = "&Aceptar"
        '
        'DataGridHoteles
        '
        Me.DataGridHoteles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridHoteles.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DataGridHoteles.DataMember = ""
        Me.DataGridHoteles.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridHoteles.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGridHoteles.Location = New System.Drawing.Point(8, 16)
        Me.DataGridHoteles.Name = "DataGridHoteles"
        Me.DataGridHoteles.ReadOnly = True
        Me.DataGridHoteles.Size = New System.Drawing.Size(777, 96)
        Me.DataGridHoteles.TabIndex = 3
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.CheckBoxActualizaCuenta)
        Me.GroupBox2.Controls.Add(Me.ProgressBar1)
        Me.GroupBox2.Controls.Add(Me.ListBox1)
        Me.GroupBox2.Controls.Add(Me.DataGrid1)
        Me.GroupBox2.Controls.Add(Me.ButtonCrearTablaServicios)
        Me.GroupBox2.Location = New System.Drawing.Point(8, 136)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(879, 432)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Servicios"
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(8, 408)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(700, 16)
        Me.ProgressBar1.TabIndex = 6
        '
        'ListBox1
        '
        Me.ListBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox1.Location = New System.Drawing.Point(743, 296)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.ScrollAlwaysVisible = True
        Me.ListBox1.Size = New System.Drawing.Size(120, 95)
        Me.ListBox1.TabIndex = 5
        Me.ListBox1.Visible = False
        '
        'DataGrid1
        '
        Me.DataGrid1.AllowNavigation = False
        Me.DataGrid1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGrid1.DataMember = ""
        Me.DataGrid1.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGrid1.Location = New System.Drawing.Point(8, 16)
        Me.DataGrid1.Name = "DataGrid1"
        Me.DataGrid1.Size = New System.Drawing.Size(700, 384)
        Me.DataGrid1.TabIndex = 0
        '
        'ButtonCrearTablaServicios
        '
        Me.ButtonCrearTablaServicios.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonCrearTablaServicios.Location = New System.Drawing.Point(720, 16)
        Me.ButtonCrearTablaServicios.Name = "ButtonCrearTablaServicios"
        Me.ButtonCrearTablaServicios.Size = New System.Drawing.Size(143, 40)
        Me.ButtonCrearTablaServicios.TabIndex = 4
        Me.ButtonCrearTablaServicios.Text = "Crear/Actualizar Tabla Servicios"
        '
        'CheckBoxActualizaCuenta
        '
        Me.CheckBoxActualizaCuenta.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxActualizaCuenta.AutoSize = True
        Me.CheckBoxActualizaCuenta.Location = New System.Drawing.Point(720, 62)
        Me.CheckBoxActualizaCuenta.Name = "CheckBoxActualizaCuenta"
        Me.CheckBoxActualizaCuenta.Size = New System.Drawing.Size(151, 17)
        Me.CheckBoxActualizaCuenta.TabIndex = 7
        Me.CheckBoxActualizaCuenta.Text = "Actualiza Cuenta Contable"
        Me.CheckBoxActualizaCuenta.UseVisualStyleBackColor = True
        '
        'FormCuentasServiciosBloque
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(895, 573)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "FormCuentasServiciosBloque"
        Me.Text = "Cuentas por Servicio/Bloque de Alojamiento"
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.DataGridHoteles, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub FormCuentasServiciosBloque_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            '---------------------------------------------------------------------------------------------------
            ' Conecta con la Base de Datos "central"
            '----------------------------------------------------------------------------------------------------
            Me.DbIntegracion = New C_DATOS.C_DatosOledb(MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"))
            Me.DbIntegracion.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")
            '---------------------------------------------------------------------------------------------------
            ' Lee y muestra algunos parametros de Integracion
            '----------------------------------------------------------------------------------------------------
            Me.mEmpGrupoCod = MyIni.IniGet(Application.StartupPath & "\menu.ini", "PARAMETER", "PARA_EMPGRUPO_COD")


            If Me.mEmpGrupoCod = "" Then
                MsgBox("No se ha podido leer Grupo de Empresas en Fichero Ini", MsgBoxStyle.Information, "Atención")
            End If


            '---------------------------------------------------------------------------------------------------
            ' Muestra/Pide Hotel a Integrar  
            '----------------------------------------------------------------------------------------------------


            SQL = "SELECT HOTEL_EMPGRUPO_COD,HOTEL_EMP_COD,HOTEL_DESCRIPCION,HOTEL_ODBC,HOTEL_SPYRO,HOTEL_ODBC_NEWGOLF FROM TH_HOTEL "
            SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            Me.DataGridHoteles.DataSource = Me.DbIntegracion.TraerDataset(SQL, "HOTELES")
            Me.DataGridHoteles.DataMember = "HOTELES"

            Me.ConfGriHoteles()

            If Me.DbIntegracion.mDbDataset.Tables("HOTELES").Rows.Count > 0 Then
                Me.HayRegistros = True
                Me.DataGrid1.CaptionText = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2)
                Me.DataGridHoteles.Select(0)


                Me.mEmpGrupoCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0)
                Me.mEmpCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1)
                Me.MostrarRegistros()
            Else
                Me.HayRegistros = False

            End If


        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Exclamation, "Load")
        End Try
    End Sub
    Private Sub ConfGriHoteles()

        Try
            Dim ts1 As New DataGridTableStyle

            ts1.MappingName = "HOTELES"

            Dim TextCol1 As New DataGridTextBoxColumn
            TextCol1.MappingName = "HOTEL_EMPGRUPO_COD"
            TextCol1.HeaderText = "Grupo de Empresas"
            TextCol1.Width = 75
            ts1.GridColumnStyles.Add(TextCol1)


            Dim TextCol2 As New DataGridTextBoxColumn
            TextCol2.MappingName = "HOTEL_EMP_COD"
            TextCol2.HeaderText = "Código Empresa"
            TextCol2.Width = 75
            ts1.GridColumnStyles.Add(TextCol2)


            Dim TextCol3 As New DataGridTextBoxColumn
            TextCol3.MappingName = "HOTEL_DESCRIPCION"
            TextCol3.HeaderText = "Descripción"
            TextCol3.Width = 200
            ts1.GridColumnStyles.Add(TextCol3)

            Dim TextCol4 As New DataGridTextBoxColumn
            TextCol4.MappingName = "HOTEL_ODBC"
            TextCol4.HeaderText = "ODBC HOTEL"
            TextCol4.Width = 0
            ts1.GridColumnStyles.Add(TextCol4)

            Dim TextCol5 As New DataGridTextBoxColumn
            TextCol5.MappingName = "HOTEL_SPYRO"
            TextCol5.HeaderText = "ODBC SPYRO"
            TextCol5.Width = 0
            ts1.GridColumnStyles.Add(TextCol5)


            Dim TextCol6 As New DataGridTextBoxColumn
            TextCol6.MappingName = "HOTEL_ODBC_NEWGOLF"
            TextCol6.HeaderText = "ODBC NEWGOLF"
            TextCol6.Width = 200
            ts1.GridColumnStyles.Add(TextCol6)



            Me.DataGridHoteles.TableStyles.Clear()
            Me.DataGridHoteles.TableStyles.Add(ts1)





        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub ConfGridCuentas()

        Try
            Dim ts1 As New DataGridTableStyle

            ts1.MappingName = "CUENTAS"


            Dim TextCol1 As New DataGridTextBoxColumn
            TextCol1.MappingName = "HOTEL_EMPGRUPO_COD"
            TextCol1.HeaderText = ""
            TextCol1.Width = 0
            TextCol1.ReadOnly = True
            ts1.GridColumnStyles.Add(TextCol1)

            Dim TextCol2 As New DataGridTextBoxColumn
            TextCol2.MappingName = "HOTEL_EMP_COD"
            TextCol2.HeaderText = ""
            TextCol2.Width = 0
            TextCol2.ReadOnly = True
            ts1.GridColumnStyles.Add(TextCol2)

            Dim TextCol3 As New DataGridTextBoxColumn
            TextCol3.MappingName = "SERV_CODI"
            TextCol3.HeaderText = ""
            TextCol3.Width = 0
            TextCol3.ReadOnly = True
            ts1.GridColumnStyles.Add(TextCol3)


            Dim TextCol4 As New DataGridTextBoxColumn
            TextCol4.MappingName = "BLAL_CODI"
            TextCol4.HeaderText = ""
            TextCol4.Width = 0
            TextCol4.ReadOnly = True
            ts1.GridColumnStyles.Add(TextCol4)


            Dim TextColA As New DataGridTextBoxColumn
            TextColA.MappingName = "BLAL_DESC"
            TextColA.HeaderText = "Bloque"
            TextColA.Width = 150
            TextColA.ReadOnly = True

            ts1.GridColumnStyles.Add(TextColA)


            Dim TextColB As New DataGridTextBoxColumn
            TextColB.MappingName = "SERV_DESC"
            TextColB.HeaderText = "Servicio"
            TextColB.Width = 150
            TextColB.ReadOnly = True
            ts1.GridColumnStyles.Add(TextColB)


            Dim TextColC As New DataGridTextBoxColumn
            TextColC.MappingName = "SERV_CTB1"
            TextColC.HeaderText = "Cuenta"
            TextColC.Width = 100
            TextColC.ReadOnly = True
            ts1.GridColumnStyles.Add(TextColC)

            Dim TextColD As New DataGridTextBoxColumn
            TextColD.MappingName = "SERV_COMS"
            TextColD.HeaderText = "Centro de Costo"
            TextColD.Width = 100
            '   TextColD.NullText = "?"

            ts1.GridColumnStyles.Add(TextColD)

            Me.DataGrid1.TableStyles.Clear()
            Me.DataGrid1.TableStyles.Add(ts1)


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub


    Private Sub ButtonCrearTablaServicios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCrearTablaServicios.Click
        Try
            Me.CreaTablaServicios()
            Me.MostrarRegistros()

        Catch ex As Exception

        End Try
    End Sub
    Private Sub CreaTablaServicios()
        Try

            Dim TotalRegistros As Integer
            Dim ControlExiste As Integer

            If IsNothing(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3)) = True Then
                Exit Sub
            End If

            Me.DbHotel = New C_DATOS.C_DatosOledb(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3))
            Me.DbHotel.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbHotelAux = New C_DATOS.C_DatosOledb(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3))
            Me.DbHotelAux.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")



            SQL = "SELECT COUNT(*) FROM TH_SERV_BLAL WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND HOTEL_EMP_COD  ='" & Me.mEmpCod & "'"

            ' verificar que no esixtan los registros
            TotalRegistros = Me.DbIntegracion.EjecutaSqlScalar(SQL)
            If TotalRegistros > 0 Then
                MsgBox("Ya esiste la Tabla , se Procede a Actualizarla ", MsgBoxStyle.Information, "Atención")
                '   Me.MostrarRegistros()
                '   Exit Sub
            End If
            ' Crear los Rsgistros del Hotel seleccionado

            SQL = "SELECT BLAL_CODI,BLAL_DESC FROM TNHT_BLAL"
            Me.DbHotel.TraerLector(SQL)

            Me.Cursor = Cursors.WaitCursor

            While Me.DbHotel.mDbLector.Read

                SQL = "SELECT SERV_CODI,SERV_DESC, SERV_CTB1,SERV_COMS FROM TNHT_SERV"
                Me.DbHotelAux.TraerLector(SQL)
                While Me.DbHotelAux.mDbLector.Read

                    ' Control de si existe el registro 

                    SQL = "SELECT NVL(COUNT(*),'0') AS TOTAL FROM TH_SERV_BLAL WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                    SQL += " AND HOTEL_EMP_COD = '" & Me.mEmpCod & "'"
                    SQL += " AND SERV_CODI = '" & Me.DbHotelAux.mDbLector.Item("SERV_CODI") & "'"
                    SQL += " AND BLAL_CODI = '" & Me.DbHotel.mDbLector.Item("BLAL_CODI") & "'"

                    ControlExiste = CType(Me.DbIntegracion.EjecutaSqlScalar(SQL), Integer)
                    If ControlExiste = 0 Then
                        SQL = "INSERT INTO TH_SERV_BLAL (HOTEL_EMPGRUPO_COD, HOTEL_EMP_COD, SERV_CODI,BLAL_CODI, SERV_CTB1, SERV_COMS) "
                        SQL += "VALUES ('"
                        SQL += Me.mEmpGrupoCod & "','"
                        SQL += Me.mEmpCod & "','"
                        SQL += Me.DbHotelAux.mDbLector.Item("SERV_CODI") & "','"
                        SQL += Me.DbHotel.mDbLector.Item("BLAL_CODI") & "','"
                        SQL += Me.DbHotelAux.mDbLector.Item("SERV_CTB1") & "','"
                        SQL += Me.DbHotelAux.mDbLector.Item("SERV_COMS") & "')"
                        Me.DbIntegracion.EjecutaSqlCommit(SQL)
                    End If
                    ' SI EL REGISTRO EXISTE Y ESTA MARCADO EL CHEK DE ACTUALIZA CUENTA CONTABLE SE ACTUALIZA EL REGISTRO CON LA CUENTA CONTABLE DE NEWHOTEL

                    If ControlExiste = 1 And Me.CheckBoxActualizaCuenta.Checked Then
                        SQL = "UPDATE TH_SERV_BLAL SET SERV_CTB1 = '" & Me.DbHotelAux.mDbLector.Item("SERV_CTB1") & "'"
                        SQL += "WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                        SQL += " AND HOTEL_EMP_COD = '" & Me.mEmpCod & "'"
                        SQL += " AND SERV_CODI = '" & Me.DbHotelAux.mDbLector.Item("SERV_CODI") & "'"
                        SQL += " AND BLAL_CODI = '" & Me.DbHotel.mDbLector.Item("BLAL_CODI") & "'"
                        Me.DbIntegracion.EjecutaSqlCommit(SQL)

                    End If

                End While
                Me.DbHotelAux.mDbLector.Close()
            End While

            Me.DbHotel.mDbLector.Close()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub MostrarRegistros()
        Try
            SQL = "SELECT TH_SERV_BLAL.HOTEL_EMPGRUPO_COD,TH_SERV_BLAL.HOTEL_EMP_COD,TH_SERV_BLAL.SERV_CODI,TH_SERV_BLAL.BLAL_CODI,BLAL_DESC,SERV_DESC,TH_SERV_BLAL.SERV_CTB1,TH_SERV_BLAL.SERV_COMS FROM TH_SERV_BLAL," & StrConexionExtraeUsuario(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3)) & ".TNHT_BLAL,"
            SQL += StrConexionExtraeUsuario(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3)) & ".TNHT_SERV"
            SQL += " WHERE "
            SQL += "TH_SERV_BLAL.BLAL_CODI = " & StrConexionExtraeUsuario(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3)) & ".TNHT_BLAL.BLAL_CODI" & "(+) AND "
            SQL += "TH_SERV_BLAL.SERV_CODI = " & StrConexionExtraeUsuario(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3)) & ".TNHT_SERV.SERV_CODI" & "(+) AND "

            SQL += " HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod
            SQL += "' AND HOTEL_EMP_COD  ='" & Me.mEmpCod & "'"
            SQL += " ORDER BY SERV_DESC,BLAL_DESC  ASC "



            Me.DbIntegracion.TraerDataset(SQL, "CUENTAS")

            Me.DataGrid1.DataSource = Me.DbIntegracion.mDbDataset
            Me.DataGrid1.DataMember = "CUENTAS"

            If Me.DbIntegracion.mDbDataset.Tables("CUENTAS").Rows.Count > 0 Then
                Me.HayCuentas = True
            Else
                Me.HayCuentas = False
            End If
            Me.ConfGridCuentas()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DataGridHoteles_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridHoteles.CurrentCellChanged
        Try
            If Me.HayRegistros = True Then
                Me.DataGrid1.CaptionText = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2)
                Me.mEmpGrupoCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0)
                Me.mEmpCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1)
                Me.MostrarRegistros()
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub DataGrid1_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid1.Validated
        If Me.HayCuentas Then
            Me.ActualizarRegistros()
        End If
    End Sub
    Private Sub ActualizarRegistros()
        Try
            Dim Ind As Integer
            Dim Update As Integer
            Dim HasUpdate As Integer

            Me.ProgressBar1.Value = 0
            Me.ProgressBar1.Maximum = Me.DbIntegracion.mDbDataset.Tables("CUENTAS").Rows.Count - 1
            Me.ProgressBar1.Update()
            Me.ListBox1.Items.Clear()
            Me.ListBox1.Update()
            Me.Cursor = Cursors.WaitCursor
            For Ind = 0 To Me.DbIntegracion.mDbDataset.Tables("CUENTAS").Rows.Count - 1
                Me.ProgressBar1.Value = Ind
                Me.ProgressBar1.Update()
                SQL = "SELECT NVL(SERV_COMS,'?') AS SERV_COMS FROM TH_SERV_BLAL WHERE HOTEL_EMPGRUPO_COD = '" & Me.DataGrid1.Item(Ind, 0) & "'"
                SQL += " AND HOTEL_EMP_COD = '" & Me.DataGrid1.Item(Ind, 1) & "'"
                SQL += " AND SERV_CODI = '" & Me.DataGrid1.Item(Ind, 2) & "'"
                SQL += " AND BLAL_CODI = '" & Me.DataGrid1.Item(Ind, 3) & "'"

                Update = 0
                If IsDBNull(Me.DataGrid1.Item(Ind, 7)) = False Then
                    If Me.DbIntegracion.EjecutaSqlScalar(SQL) <> Me.DataGrid1.Item(Ind, 7) Then
                        Update = 1
                    End If
                End If


                HasUpdate = 0
                If Update = 1 Then
                    SQL = "UPDATE TH_SERV_BLAL SET SERV_COMS = '" & Me.DataGrid1.Item(Ind, 7) & "'"
                    SQL += "WHERE HOTEL_EMPGRUPO_COD = '" & Me.DataGrid1.Item(Ind, 0) & "'"
                    SQL += " AND HOTEL_EMP_COD = '" & Me.DataGrid1.Item(Ind, 1) & "'"
                    SQL += " AND SERV_CODI = '" & Me.DataGrid1.Item(Ind, 2) & "'"
                    SQL += " AND BLAL_CODI = '" & Me.DataGrid1.Item(Ind, 3) & "'"
                    Me.DbIntegracion.EjecutaSqlCommit(SQL)

                    HasUpdate = 1
                End If

                Me.ListBox1.Items.Add("Indice = " & Ind & "  " & Me.DataGrid1.Item(Ind, 7) & "  " & Update & "  " & HasUpdate)


            Next Ind
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            MsgBox(ex.Message)
            Me.Cursor = Cursors.Default
        End Try
    End Sub


    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Me.Close()
    End Sub

    Private Sub FormCuentasServiciosBloque_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Try


            If Me.DbIntegracion.EstadoConexion = ConnectionState.Open Then
                Me.DbIntegracion.CerrarConexion()
            End If

            If IsNothing(Me.DbHotel) = False Then
                If IsDBNull(Me.DbHotel.StrConexion) = False Then
                    If Me.DbHotel.EstadoConexion = ConnectionState.Open Then
                        Me.DbHotel.CerrarConexion()
                    End If
                End If
            End If


            If IsNothing(Me.DbHotelAux) = False Then
                If IsDBNull(Me.DbHotelAux.StrConexion) = False Then
                    If Me.DbHotelAux.EstadoConexion = ConnectionState.Open Then
                        Me.DbHotelAux.CerrarConexion()
                    End If
                End If
            End If




        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

  
End Class
