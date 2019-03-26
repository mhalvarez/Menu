Imports System.IO
Public Class FormConvertir
    Inherits System.Windows.Forms.Form
    Dim mFecha As Date
    Dim mFecha2 As Date

    Dim mStrConexion As String
    Dim DbLee As C_DATOS.C_DatosOledb
    Dim DbLeeAux As C_DATOS.C_DatosOledb
    Dim SQL As String
    Dim Linea As String

    Dim Importe As Double
    Dim ImporteLiquido As Double
    Dim TipoImpuesto As Double
    Dim CuotaImpuesto As Double
    Dim Indicador As String

    Dim mTipo As String

    Private mEmpGrupoCod As String
    Private mEmpCod As String
    Private mFormato As String
    Private mvFile As String
    Private mNoWait As Boolean
    Private mvFilePath As String
    Private mvFileType As String
    Private mParaDebe As String
    Private mParaHaber As String

    Private mParaDebeFac As String
    Private mParaHaberFac As String

    Private mParaCuentaMano As String

    Private mParaPrefijo As String
    Private mParaTextoIva As String
    Private Filegraba As StreamWriter
    Private FileEstaOk As Boolean = False

    Private PrimerRegistro As Boolean = True
    Private mAsientoSpyro As Integer
    Private mAsiento As Integer
    Private mApunte As Integer


    Dim TotalDebe As Double
    Friend WithEvents CheckBoxNifFinal As System.Windows.Forms.CheckBox
    Dim TotalHaber As Double

    Dim m_TotalRegistros As Integer

    Private m_EmpresaContanet As String
    ' Private m_EXCN As NSOLE5.Enlace
    Private m_EXCN_ANIO As String

    Private m_ContadorLineasIgic As Integer

#Region " Código generado por el Diseñador de Windows Forms "

    Public Sub New()
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()


    End Sub
    Public Sub New(ByVal vfecha As Date, ByVal vStrConexion As String, ByVal vEmpgrupo_Cod As String, ByVal vEmp_Cod As String, ByVal vFormato As String, ByVal vNoWait As Boolean, ByVal vTipo As String)
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()
        Me.mFecha = vfecha

        Me.mEmpGrupoCod = vEmpgrupo_Cod
        Me.mEmpCod = vEmp_Cod
        Me.mFormato = vFormato
        Me.mNoWait = vNoWait
        Me.mTipo = vTipo
        Me.Text = Me.Text & " " & Me.mFecha & " Grupo : " & vEmpgrupo_Cod & " Empresa : " & vEmp_Cod & " Formato : " & vFormato & " Origen : " & vTipo
        Me.mStrConexion = vStrConexion
        Me.Update()

      


    End Sub
   
    Public Sub New(ByVal vfecha As Date, ByVal vfecha2 As Date, ByVal vStrConexion As String, ByVal vEmpgrupo_Cod As String, ByVal vEmp_Cod As String, ByVal vFormato As String, ByVal vNoWait As Boolean, ByVal vTipo As String)
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()
        Me.mFecha = vfecha
        Me.mFecha2 = vfecha2

        Me.mEmpGrupoCod = vEmpgrupo_Cod
        Me.mEmpCod = vEmp_Cod
        Me.mFormato = vFormato
        Me.mNoWait = vNoWait
        Me.mTipo = vTipo
        Me.Text = Me.Text & " " & Me.mFecha & " / " & Me.mFecha2 & " Grupo : " & vEmpgrupo_Cod & " Empresa : " & vEmp_Cod & " Formato : " & vFormato & " Origen : " & vTipo
        Me.mStrConexion = vStrConexion
        Me.Update()

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
    Friend WithEvents ComboBoxFormato As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonProcesar As System.Windows.Forms.Button
    Friend WithEvents TextBoxDebug As System.Windows.Forms.TextBox
    Friend WithEvents ListBoxDebug As System.Windows.Forms.ListBox
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBoxFilePath As System.Windows.Forms.TextBox
    Friend WithEvents DataGrid1 As System.Windows.Forms.DataGrid
    Friend WithEvents TextBoxDebe As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxHaber As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ComboBoxFormato = New System.Windows.Forms.ComboBox()
        Me.ButtonProcesar = New System.Windows.Forms.Button()
        Me.TextBoxDebug = New System.Windows.Forms.TextBox()
        Me.ListBoxDebug = New System.Windows.Forms.ListBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBoxFilePath = New System.Windows.Forms.TextBox()
        Me.DataGrid1 = New System.Windows.Forms.DataGrid()
        Me.TextBoxDebe = New System.Windows.Forms.TextBox()
        Me.TextBoxHaber = New System.Windows.Forms.TextBox()
        Me.CheckBoxNifFinal = New System.Windows.Forms.CheckBox()
        CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 23)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Formato"
        '
        'ComboBoxFormato
        '
        Me.ComboBoxFormato.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxFormato.DropDownWidth = 256
        Me.ComboBoxFormato.Items.AddRange(New Object() {"IslaSoft 2003", "Tahona", "MicroSoft Axapta", "Datisa", "Contaplus H Lopez", "Contaplus Ereza Viajes", "HC7 Sistema Propietario", "A3 Parque Tropical", "ContaPlus 2003", "Contanet (OCX)", "Vital Suites(Gestión)"})
        Me.ComboBoxFormato.Location = New System.Drawing.Point(72, 16)
        Me.ComboBoxFormato.Name = "ComboBoxFormato"
        Me.ComboBoxFormato.Size = New System.Drawing.Size(200, 21)
        Me.ComboBoxFormato.TabIndex = 1
        '
        'ButtonProcesar
        '
        Me.ButtonProcesar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonProcesar.Location = New System.Drawing.Point(874, 16)
        Me.ButtonProcesar.Name = "ButtonProcesar"
        Me.ButtonProcesar.Size = New System.Drawing.Size(75, 23)
        Me.ButtonProcesar.TabIndex = 2
        Me.ButtonProcesar.Text = "Procesar"
        '
        'TextBoxDebug
        '
        Me.TextBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDebug.Location = New System.Drawing.Point(8, 48)
        Me.TextBoxDebug.Name = "TextBoxDebug"
        Me.TextBoxDebug.Size = New System.Drawing.Size(858, 20)
        Me.TextBoxDebug.TabIndex = 3
        '
        'ListBoxDebug
        '
        Me.ListBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListBoxDebug.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBoxDebug.HorizontalExtent = 5000
        Me.ListBoxDebug.HorizontalScrollbar = True
        Me.ListBoxDebug.ItemHeight = 14
        Me.ListBoxDebug.Location = New System.Drawing.Point(8, 80)
        Me.ListBoxDebug.Name = "ListBoxDebug"
        Me.ListBoxDebug.ScrollAlwaysVisible = True
        Me.ListBoxDebug.Size = New System.Drawing.Size(858, 86)
        Me.ListBoxDebug.TabIndex = 4
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(8, 176)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(858, 10)
        Me.ProgressBar1.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 360)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(48, 23)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Fichero"
        '
        'TextBoxFilePath
        '
        Me.TextBoxFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxFilePath.Location = New System.Drawing.Point(56, 360)
        Me.TextBoxFilePath.Name = "TextBoxFilePath"
        Me.TextBoxFilePath.Size = New System.Drawing.Size(216, 20)
        Me.TextBoxFilePath.TabIndex = 8
        '
        'DataGrid1
        '
        Me.DataGrid1.DataMember = ""
        Me.DataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGrid1.Location = New System.Drawing.Point(8, 200)
        Me.DataGrid1.Name = "DataGrid1"
        Me.DataGrid1.Size = New System.Drawing.Size(858, 152)
        Me.DataGrid1.TabIndex = 9
        '
        'TextBoxDebe
        '
        Me.TextBoxDebe.Location = New System.Drawing.Point(660, 359)
        Me.TextBoxDebe.Name = "TextBoxDebe"
        Me.TextBoxDebe.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxDebe.TabIndex = 10
        '
        'TextBoxHaber
        '
        Me.TextBoxHaber.Location = New System.Drawing.Point(764, 359)
        Me.TextBoxHaber.Name = "TextBoxHaber"
        Me.TextBoxHaber.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxHaber.TabIndex = 11
        '
        'CheckBoxNifFinal
        '
        Me.CheckBoxNifFinal.AutoSize = True
        Me.CheckBoxNifFinal.Enabled = False
        Me.CheckBoxNifFinal.ForeColor = System.Drawing.Color.Maroon
        Me.CheckBoxNifFinal.Location = New System.Drawing.Point(292, 18)
        Me.CheckBoxNifFinal.Name = "CheckBoxNifFinal"
        Me.CheckBoxNifFinal.Size = New System.Drawing.Size(88, 17)
        Me.CheckBoxNifFinal.TabIndex = 12
        Me.CheckBoxNifFinal.Text = "+ NIF al Final"
        Me.CheckBoxNifFinal.UseVisualStyleBackColor = True
        '
        'FormConvertir
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(962, 394)
        Me.Controls.Add(Me.CheckBoxNifFinal)
        Me.Controls.Add(Me.TextBoxHaber)
        Me.Controls.Add(Me.TextBoxDebe)
        Me.Controls.Add(Me.DataGrid1)
        Me.Controls.Add(Me.TextBoxFilePath)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.ListBoxDebug)
        Me.Controls.Add(Me.TextBoxDebug)
        Me.Controls.Add(Me.ButtonProcesar)
        Me.Controls.Add(Me.ComboBoxFormato)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormConvertir"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Conversión de Formato"
        CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub ButtonProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonProcesar.Click
        Try
            Me.TotalDebe = 0
            Me.TotalHaber = 0

            If Me.ComboBoxFormato.SelectedIndex > -1 Then
                Me.Cursor = Cursors.WaitCursor
                Me.Procesar()
                Me.Cursor = Cursors.Default
            Else
                MsgBox("Debe de escoger un Formato",, "Atención")
            End If

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención ..")
        End Try
    End Sub
    Private Sub Procesar()
        Try


            Me.DbLee = New C_DATOS.C_DatosOledb(Me.mStrConexion)
            Me.DbLee.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbLeeAux = New C_DATOS.C_DatosOledb(Me.mStrConexion)
            Me.DbLeeAux.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


            ' CARGA PARAMETROS
            SQL = "SELECT NVL(PARA_FILE_FORMAT,'?') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            Me.mvFileType = Me.DbLeeAux.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_FILE_SPYRO_PATH,'?') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            Me.mvFilePath = Me.DbLeeAux.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_FILE_PREFIJO,'-') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            Me.mParaPrefijo = Me.DbLeeAux.EjecutaSqlScalar(SQL)


            SQL = "SELECT NVL(PARA_DEBE,'?') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            Me.mParaDebe = Me.DbLeeAux.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_DEBE_FAC,'?') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            Me.mParaDebeFac = Me.DbLeeAux.EjecutaSqlScalar(SQL)


            SQL = "SELECT NVL(PARA_HABER,'?') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            Me.mParaHaber = Me.DbLeeAux.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_HABER_FAC,'?') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            Me.mParaHaberFac = Me.DbLeeAux.EjecutaSqlScalar(SQL)


            SQL = "SELECT NVL(PARA_TEXTO_IVA,'?') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            Me.mParaTextoIva = Me.DbLeeAux.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_CTA1,'0') FROM TH_PARA WHERE PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"
            Me.mParaCuentaMano = Me.DbLeeAux.EjecutaSqlScalar(SQL)


            SQL = "SELECT HOTEL_EMPRESA_PATH FROM TH_HOTEL WHERE HOTEL_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND HOTEL_EMP_COD = '" & Me.mEmpCod & "'"
            Me.m_EmpresaContanet = Me.DbLeeAux.EjecutaSqlScalar(SQL)


            If mEmpGrupoCod = "MONI" Then
                If Me.m_EmpresaContanet.Length > 0 Then
                    If File.Exists(Me.m_EmpresaContanet) = False Then
                        MsgBox("No se localiza la Ruta ( " & Me.m_EmpresaContanet & ")" & " definida en los Parámetros del Hotel",, "Atención")
                        Exit Sub
                    End If
                Else
                    MsgBox("No está definida la Ruta del Fichero NST de la Empresa en los Parámetros del Hotel",, "Atención")
                    Exit Sub
                End If
            End If



            ' borra fichero standard
            Dim FileToDelete As String = Me.mvFilePath & "F" & Format(Me.mFecha, ("dd-MM-yyyy")) & ".TXT"
            If System.IO.File.Exists(FileToDelete) = True Then
                System.IO.File.Delete(FileToDelete)
            End If


            ' creaFichero convertido 
            If Me.ComboBoxFormato.Text = "Tahona" Then
                Me.mvFile = Me.mvFilePath & "CONTABLE" & ".TXT"
            ElseIf Me.ComboBoxFormato.Text = "Datisa" Then
                Me.mvFile = Me.mvFilePath & Me.mParaPrefijo & Format(Me.mFecha, ("ddMMyy")) & ".TXT"
            ElseIf Me.ComboBoxFormato.Text = "ContaPlus 2003" Then
                Me.mvFile = Me.mvFilePath & Me.mParaPrefijo & Format(Me.mFecha, ("ddMMyy")) & ".TXT"
            ElseIf Me.ComboBoxFormato.Text = "Contaplus H Lopez" Then
                Me.mvFile = Me.mvFilePath & Me.mParaPrefijo & Format(Me.mFecha, ("yyyyMMdd")) & ".TXT"
            ElseIf Me.ComboBoxFormato.Text = "Contaplus Ereza Viajes" Then
                Me.mvFile = Me.mvFilePath & Me.mParaPrefijo & Format(Me.mFecha, ("yyyyMMdd")) & ".TXT"
            ElseIf Me.ComboBoxFormato.Text = "A3 Parque Tropical" Then
                ' Me.mvFile = Me.mvFilePath & "SUENLACE" & ".DAT"
                Me.mvFile = Me.mvFilePath & "Newh" & Format(Me.mFecha, ("yyyyMMdd")) & ".TXT"

            Else
                Me.mvFile = Me.mvFilePath & "CONTABLE" & ".TXT"
            End If
            Me.CrearFichero(Me.mvFile)
            Me.TextBoxFilePath.Text = Me.mvFile

            'PROCESA FORMATO ESCOGIDO
            If Me.FileEstaOk = True Then
                If Me.ComboBoxFormato.Text = "Tahona" Then
                    Me.ProcesaTahona()
                    Me.Filegraba.Close()
                End If
                If Me.ComboBoxFormato.Text = "Datisa" Then
                    Me.ProcesaDatisa()
                    Me.Filegraba.Close()
                End If
                If Me.ComboBoxFormato.Text = "Contaplus H Lopez" Then
                    If Me.mTipo = "NEWCONTA" Then
                        Me.ProcesaContaplusLopezNewConta()
                        Me.Filegraba.Close()
                    End If
                    If Me.mTipo = "NEWSTOCK" Then
                        Me.ProcesaContaplusLopezNewStock()
                        Me.Filegraba.Close()
                    End If

                End If
                If Me.ComboBoxFormato.Text = "ContaPlus 2003" Then
                    Me.ProcesaContaplus()
                    Me.Filegraba.Close()
                End If

                If Me.ComboBoxFormato.Text = "Contaplus Ereza Viajes" Then
                    Me.ProcesaContaplusEreza()
                    Me.Filegraba.Close()
                End If

                ' HC7
                If Me.ComboBoxFormato.SelectedIndex = 6 Then
                    Me.ProcesaCHC7()
                    Me.Filegraba.Close()
                End If

                'Parque Trpical
                If Me.ComboBoxFormato.SelectedIndex = 7 Then
                    ' Me.ProcesaA3PTropicalAsientos()
                    ' Me.ProcesaA3PTropicalAsientosLibrodeIgic()


                    Me.ProcesaA3PTropicalAsientos2()
                    Me.ProcesaA3PTropicalAsientosLibrodeIgic2()

                    Me.Filegraba.Close()
                End If


                ' CONTANET
                If Me.ComboBoxFormato.SelectedIndex = 9 Then
                    '   Me.ProcesaContanet()

                End If

                ' VITAL SUITES
                If Me.ComboBoxFormato.SelectedIndex = 10 Then

                    ' imponer  ultura  coma para cobol
                    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("es-ES", False)
                    System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ","
                    System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = "."

                    ' cierra y borra fichero standard 
                    Me.Filegraba.Close()
                    ' borra fichero standard
                    Dim F As String = Me.mvFile
                    If System.IO.File.Exists(F) = True Then
                        System.IO.File.Delete(F)
                    End If



                    Dim Fecha As Date = Me.mFecha

                    If Me.mFecha2.Year = 1 Then
                        Me.mFecha2 = Me.mFecha
                    End If


                    While Fecha <= Me.mFecha2


                        Me.ListBoxDebug.Items.Clear()
                        Me.ListBoxDebug.Update()

                        Me.CrearFichero(Me.mvFilePath & Format(Fecha, ("yyyyMMdd")) & ".001")

                        Me.mvFile = Me.mvFilePath & Format(Fecha, ("yyyyMMdd")) & ".001"
                        '   Me.ProcesaVital2("AGENCIAS", Fecha)
                        Me.ProcesaVital3("AGENCIAS", Fecha)
                        Me.Filegraba.Close()

                        Me.CrearFichero(Me.mvFilePath & Format(Fecha, ("yyyyMMdd")) & ".002")
                        '   Me.ProcesaVital2("", Fecha)
                        Me.ProcesaVital3("", Fecha)
                        Me.Filegraba.Close()




                        Fecha = DateAdd(DateInterval.Day, 1, Fecha)

                    End While

                    ' imponer  cultura  pone separador   decimal a punto otra vez para oracle 
                    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("es-ES", False)
                    System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = "."
                    System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = ","



                End If
            Else
                MsgBox("Fichero no accesible", MsgBoxStyle.Information, "Atención")
                Exit Sub
            End If



        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub
#Region "TAHONA"
    Private Sub ProcesaTahona()

        Try

            Dim ImporteString As String


            ' contar
            SQL = "SELECT NVL(COUNT(*),'0')  AS TOTAL"
            SQL += "  FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            ' SQL += " AND ASNT_IMPRIMIR = 'SI'"
            ' SE BLOQUE ESTO PORQUE TAHONA PASA A USAR DLL DE SATOCAN.DLL EN VEZ DE INTEGRACION.DLL 06-JULIO-2009
            'SQL += " AND ASNT_TIPO_REGISTRO = 'AC'"
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"

            Me.ProgressBar1.Maximum = Me.DbLee.EjecutaSqlScalar(SQL)
            Me.ProgressBar1.Value = 0
            Me.ProgressBar1.Update()



            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, "
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,ASNT_NOMBRE AS OBSERVACION FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            '  SQL += " AND ASNT_IMPRIMIR = 'SI'"
            ' SE BLOQUE ESTO PORQUE TAHONA PASA A USAR DLL DE SATOCAN.DLL EN VEZ DE INTEGRACION.DLL 06-JULIO-2009
            'SQL += " AND ASNT_TIPO_REGISTRO = 'AC'"
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"

            Me.DbLee.TraerLector(SQL)
            Me.ListBoxDebug.Items.Clear()
            Me.ListBoxDebug.Update()
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read

                ' renumeracion de apuntes 
                If Me.PrimerRegistro = True Then
                    Me.PrimerRegistro = False
                    Me.mAsientoSpyro = CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer)
                    Me.mAsiento = 1
                    Me.mApunte = 1
                End If

                If CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer) <> Me.mAsientoSpyro Then
                    Me.mAsientoSpyro = CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer)
                    Me.mAsiento = Me.mAsiento + 1
                    Me.mApunte = 1
                End If

                ' Control quitar signo negativo e invertir indicador debe/haber
                Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)
                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)

                ' If Importe < 0 Then
                ' Importe = Importe * -1
                ' If CType(Me.DbLee.mDbLector.Item("INDICADOR"), String) = Me.mParaDebe Or CType(Me.DbLee.mDbLector.Item("INDICADOR"), String) = Me.mParaDebeFac Then
                ' Indicador = Me.mParaHaber
                ' End If
                ' If CType(Me.DbLee.mDbLector.Item("INDICADOR"), String) = Me.mParaHaber Or CType(Me.DbLee.mDbLector.Item("INDICADOR"), String) = Me.mParaHaberFac Then
                ' Indicador = Me.mParaDebe
                ' End If
                ' End If




                ' TRUCO 1 los decimales con 2 bytes , y si el numero es entero decimal = ,00 

                ImporteString = Format(Importe, "0.00")

                ' nuevo control de negativo
                ' TRUCO 2el sgino por la derecha 
                If Importe < 0 Then
                    ImporteString = ImporteString.Replace("-", "")
                    ImporteString = ImporteString & "-"
                Else
                    ImporteString = ImporteString & " "
                End If



                ' debug debe/HABER
                If Indicador = "D" Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = "H" Then
                    TotalHaber = TotalHaber + Importe
                End If

                Me.Linea = ""
                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO")
                Me.TextBoxDebug.Update()
                ' Monta la Linea
                Me.Linea += "1" & "|"
                Me.Linea += CType(Me.mAsiento, String).PadLeft(4, "0") & "|"
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM") & "|"
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy") & "|"
                Me.Linea += CType(Me.mApunte, String).PadLeft(4, "0") & "|"
                Me.Linea += CType(Me.DbLee.mDbLector.Item("CUENTA"), String).PadRight(15, " ") & "|"
                Me.Linea += Mid(CType(Indicador, String), 1, 1) & "|"




                'Me.Linea += CType(Format(Importe, "F"), String).PadLeft(16, "0").Replace(".", ",") & "|"

                Me.Linea += ImporteString.PadLeft(16, "0").Replace(".", ",") & "|"


                Me.Linea += CType(Me.DbLee.mDbLector.Item("CONCEPTO"), String).PadRight(40, " ") & "|"
                ' DOCUMENTO
                Me.Linea += " ".PadRight(12, " ") & "|"
                Me.Linea += Format(Me.mFecha, "dd/MM/yyyy") & "|"
                ' DIVISA
                Me.Linea += "EUR".PadRight(3, " ") & "|"
                ' COTIZACION
                Me.Linea += CType("0", String).PadLeft(16, "0") & "|"
                ' FACTOR DE CONVERSION
                Me.Linea += CType("1", String).PadLeft(3, "0") & "|"
                ' IMPORTE EN DIVISA
                'Me.Linea += CType(Format(Importe, "F"), String).PadLeft(16, "0").Replace(".", ",") & "|"
                Me.Linea += ImporteString.PadLeft(16, "0").Replace(".", ",") & "|"

                ' TIPO DE DIVISA 
                Me.Linea += "1"
                ' Muestra la Linea
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ProgressBar1.Value = Me.ProgressBar1.Value + 1
                ' graba la linea
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()

            ' debug debe/haber
            Me.TextBoxDebe.Text = Me.TotalDebe
            Me.TextBoxHaber.Text = Me.TotalHaber


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Procesa Tahona")
            Exit Sub

        End Try
    End Sub
#End Region
#Region "DATISA"
    Private Sub ProcesaDatisa()

        Try

            Dim TipoAsiento As String = "0"
            Dim IndicadorProduccion As String = "285"
            Dim IndicadorFacturacion As String = "286"
            Dim Concepto As String
            Dim IndicadorDebe As String
            Dim IndicadorHaber As String

            SQL = "SELECT NVL(PARA_DEBE,'?') FROM TH_PARA "
            SQL += " WHERE  PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"

            IndicadorDebe = Me.DbLee.EjecutaSqlScalar(SQL)


            SQL = "SELECT NVL(PARA_HABER,'?') FROM TH_PARA "
            SQL += " WHERE  PARA_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.mEmpCod & "'"

            IndicadorHaber = Me.DbLee.EjecutaSqlScalar(SQL)

            ' salobre
            If Me.mEmpCod = "2" Then
                'IndicadorProduccion = "287"
                'IndicadorFacturacion = "287"
                IndicadorProduccion = "2" & Mid(Format(Me.mFecha, "yy"), 2, 1) & "7"
                IndicadorFacturacion = "2" & Mid(Format(Me.mFecha, "yy"), 2, 1) & "7"

                ' resto de empresas
            Else
                'IndicadorProduccion = "285"
                'IndicadorFacturacion = "286"
                IndicadorProduccion = "2" & Mid(Format(Me.mFecha, "yy"), 2, 1) & "5"
                IndicadorFacturacion = "2" & Mid(Format(Me.mFecha, "yy"), 2, 1) & "6"



            End If

            ' contar
            SQL = "SELECT NVL(COUNT(*),'0')  AS TOTAL"
            SQL += "  FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            ' SQL += " AND ASNT_IMPRIMIR = 'SI'"
            '   SQL += " AND ASNT_TIPO_REGISTRO = 'AC'"
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"

            Me.ProgressBar1.Maximum = Me.DbLee.EjecutaSqlScalar(SQL)
            Me.ProgressBar1.Value = 0
            Me.ProgressBar1.Update()



            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, "
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS OBSERVACION FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            '  SQL += " AND ASNT_IMPRIMIR = 'SI'"
            ' SQL += " AND ASNT_TIPO_REGISTRO = 'AC'"
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"

            Me.DbLee.TraerLector(SQL)
            Me.ListBoxDebug.Items.Clear()
            Me.ListBoxDebug.Update()
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read

                ' renumeracion de apuntes 
                If Me.PrimerRegistro = True Then
                    Me.PrimerRegistro = False
                    Me.mAsientoSpyro = CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer)
                    Me.mAsiento = 1
                    Me.mApunte = 1
                End If

                If CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer) <> Me.mAsientoSpyro Then
                    Me.mAsientoSpyro = CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer)
                    Me.mAsiento = Me.mAsiento + 1
                    Me.mApunte = 1
                End If



                ' debug debe/HABER

                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)
                Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)


                If Indicador = IndicadorDebe Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = IndicadorHaber Then
                    TotalHaber = TotalHaber + Importe
                End If

                Me.Linea = ""
                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO")
                Me.TextBoxDebug.Update()
                ' Monta la Linea



                ' Indicador de Produccion o Factura salobre
                If Me.mAsiento = 1 And Me.mEmpCod = "2" Then
                    TipoAsiento = IndicadorProduccion & "0" & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM") & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")
                End If

                If Me.mAsiento <> 1 And Me.mEmpCod = "2" Then
                    TipoAsiento = IndicadorFacturacion & "1" & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM") & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")
                End If

                ' Indicador de Produccion o Factura resto de empresas
                If Me.mAsiento = 1 And Me.mEmpCod <> "2" Then
                    TipoAsiento = IndicadorProduccion & "1" & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM") & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")
                End If

                If Me.mAsiento <> 1 And Me.mEmpCod <> "2" Then
                    TipoAsiento = IndicadorFacturacion & "1" & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM") & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")
                End If

                Me.Linea += TipoAsiento & ","
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd") & "/"
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM") & "/"
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yy") & ","



                ' ORGRIGINAL
                'If Me.mAsiento > 1 Then
                'Me.Linea += """"
                'Concepto = CType(Me.DbLee.mDbLector.Item("CONCEPTO"), String) & " " & CType(Me.DbLee.mDbLector.Item("OBSERVACION"), String)
                'Me.Linea += Mid(Concepto, 1, 60).PadRight(60, " ")
                'Me.Linea += """" & ","
                'Else
                'Me.Linea += """"
                'Concepto = "PRODUCCION " & CType(Me.DbLee.mDbLector.Item("CONCEPTO"), String)
                'Me.Linea += Mid(Concepto, 1, 60).PadRight(60, " ")
                'Me.Linea += """" & ","
                'End If



                ' CONCEPTO DEL ASIENTO

                If CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer) = 1 Then
                    Me.Linea += """"
                    Concepto = "PRODUCCION " & CType(Me.DbLee.mDbLector.Item("CONCEPTO"), String)
                    Me.Linea += Mid(Concepto, 1, 60).PadRight(60, " ")
                    Me.Linea += """" & ","
                Else
                    Me.Linea += """"
                    Concepto = CType(Me.DbLee.mDbLector.Item("CONCEPTO"), String) & " " & CType(Me.DbLee.mDbLector.Item("OBSERVACION"), String)
                    Me.Linea += Mid(Concepto, 1, 60).PadRight(60, " ")
                    Me.Linea += """" & ","


                End If



                Me.Linea += CType(Me.DbLee.mDbLector.Item("CUENTA"), String).PadRight(15, " ") & ","
                Me.Linea += Mid(CType(Indicador, String), 1, 1) & ","
                'Me.Linea += CType(Format(Importe, "F"), String).PadLeft(16, "0") & ","
                Me.Linea += CType(Format(Importe, "F"), String) & ","


                ' CENTRO DE COSTO PRODUCCION
                If CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer) = 1 Then
                    If CType(Me.DbLee.mDbLector.Item("OBSERVACION"), String) <> " " Then
                        Me.Linea += CType(Me.DbLee.mDbLector.Item("OBSERVACION"), String) & ","
                    End If
                End If

                ' CENTRO DE COSTO CONSUMO DE BONOS   
                If CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer) = 81 Then
                    If Indicador = IndicadorHaber Then
                        Me.Linea += CType(Me.DbLee.mDbLector.Item("OBSERVACION"), String) & ","
                    End If
                End If
                ' CENTRO DE COSTO VENCIMIENTO DE BONOS   
                If CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer) = 82 Then
                    If Indicador = IndicadorHaber Then
                        Me.Linea += CType(Me.DbLee.mDbLector.Item("OBSERVACION"), String) & ","
                    End If
                End If


                ' CENTRO DE COSTO COMISIONES VISAS
                If CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer) = 2 Or CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer) = 25 Or CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer) = 35 Or CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer) = 37 Then
                    If Indicador = IndicadorDebe Then
                        If Mid(CType(Me.DbLee.mDbLector.Item("CONCEPTO"), String), 1, 8) = "COMISION" Then
                            Me.Linea += CType(Me.DbLee.mDbLector.Item("OBSERVACION"), String) & ","
                        End If
                    End If

                End If



                '     Me.Linea += CType(Me.mApunte, String).PadLeft(4, "0") & "|"
                '     Me.Linea += CType(Me.mAsiento, String).PadLeft(4, "0") & "|"




                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()

            ' debug debe/haber
            Me.TextBoxDebe.Text = Me.TotalDebe
            Me.TextBoxHaber.Text = Me.TotalHaber


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Procesa Datisa")
            Exit Sub

        End Try
    End Sub
#End Region
#Region "Contaplus"
    Private Sub ProcesaContaplus()

        Try

            Dim TipoAsiento As String
            Dim IndicadorProduccion As String = "285"
            Dim IndicadorFacturacion As String = "286"


            '   MsgBox("ojo falta implementar la cuenta de cliente caso de apuntes de igic")
            '   MsgBox("EL indicador de debe / haher ha de ser D/H en los parametros en el caso de Contaplus")

            ' contar
            SQL = "SELECT NVL(COUNT(*),'0')  AS TOTAL"
            SQL += "  FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            ' SQL += " AND ASNT_IMPRIMIR = 'SI'"
            '   SQL += " AND ASNT_TIPO_REGISTRO = 'AC'"
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"

            Me.ProgressBar1.Maximum = Me.DbLee.EjecutaSqlScalar(SQL)
            Me.ProgressBar1.Value = 0
            Me.ProgressBar1.Update()



            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, "
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS OBSERVACION, "
            SQL += "NVL(ASNT_FACTURA_NUMERO,' ') AS ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' '),NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2 "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            '  SQL += " AND ASNT_IMPRIMIR = 'SI'"
            ' SQL += " AND ASNT_TIPO_REGISTRO = 'AC'"
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_CFCPTOS_COD ASC "

            Me.DbLee.TraerLector(SQL)
            Me.ListBoxDebug.Items.Clear()
            Me.ListBoxDebug.Update()
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read

                ' renumeracion de apuntes 
                If Me.PrimerRegistro = True Then
                    Me.PrimerRegistro = False
                    Me.mAsientoSpyro = CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer)
                    Me.mAsiento = 1
                    Me.mApunte = 1
                End If

                If CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer) <> Me.mAsientoSpyro Then
                    Me.mAsientoSpyro = CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer)
                    Me.mAsiento = Me.mAsiento + 1
                    Me.mApunte = 1
                End If



                ' debug debe/HABER

                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)
                Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)


                If Indicador = "D" Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = "H" Then
                    TotalHaber = TotalHaber + Importe
                End If

                Me.Linea = ""
                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO")
                Me.TextBoxDebug.Update()
                ' Monta la Linea
                ' Indicador de Produccion o Factura

                If Me.mAsiento = 1 Then
                    TipoAsiento = IndicadorProduccion & "1" & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM") & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")
                Else
                    TipoAsiento = IndicadorFacturacion & "1" & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM") & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                End If

                Me.Linea += CType(Me.mAsiento, String).PadLeft(6, " ")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")
                Me.Linea += CType(Me.DbLee.mDbLector.Item("CUENTA"), String).PadRight(12, " ")


                ' cuenta de contrapartida para apuntes de igic

                If CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = "IGIC" Then
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"), String).PadRight(12, " ")
                Else
                    Me.Linea += " ".PadRight(12)
                End If



                If Indicador = "D" Then
                    Me.Linea += CType(Format(Importe, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += " ".PadLeft(16, " ")
                End If

                Me.Linea += CType(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 25), String).PadRight(25, " ")

                If Indicador = "H" Then
                    Me.Linea += CType(Format(Importe, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += " ".PadLeft(16, " ")
                End If



                ' El registro es de Igic 
                If CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = Me.mParaTextoIva Then
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO"), String).PadLeft(8, " ")
                    Me.Linea += CType(Format(CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_NUMERICO"), Double), "F"), String).PadLeft(16, " ")
                    Me.Linea += CType(Format(CType(Me.DbLee.mDbLector.Item("ASNT_TIPO_IMPUESTO"), Double), "F"), String).PadLeft(5, " ")
                Else
                    Me.Linea += "0".PadLeft(8, " ")
                    Me.Linea += CType(Format(CType("0", Double), "F"), String).PadLeft(16, " ")
                    Me.Linea += CType(Format(CType("0", Double), "F"), String).PadLeft(5, " ")
                End If

                Me.Linea += "0".PadLeft(5, " ")
                Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO"), String).PadLeft(8, " ")

                ' CENTRO DE COSTO 
                If CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = "VENTA" Then
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"), String).PadRight(3, " ")
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"), String).PadRight(6, " ")
                    Me.Linea += " "
                Else
                    Me.Linea += " ".PadRight(3, " ")
                    Me.Linea += " ".PadRight(6, " ")
                    Me.Linea += " "

                End If




                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()

            ' debug debe/haber
            Me.TextBoxDebe.Text = Me.TotalDebe
            Me.TextBoxHaber.Text = Me.TotalHaber


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Procesa Datisa")
            Exit Sub

        End Try
    End Sub



    Private Sub ProcesaContaplusEreza()

        Try

            Dim TipoAsiento As String
            Dim IndicadorProduccion As String = "285"
            Dim IndicadorFacturacion As String = "286"


            ' contar
            SQL = "SELECT NVL(COUNT(*),'0')  AS TOTAL"
            SQL += "  FROM TH_ASNT "
            SQL += " WHERE  (ASNT_F_VALOR BETWEEN '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            'SQL += " AND   '" & Format(Me.mFecha2, "dd/MM/yyyy") & "')"
            SQL += " AND   '" & Format(Me.mFecha, "dd/MM/yyyy") & "')"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            ' SQL += " AND ASNT_IMPRIMIR = 'SI'"
            '   SQL += " AND ASNT_TIPO_REGISTRO = 'AC'"
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"

            Me.ProgressBar1.Maximum = Me.DbLee.EjecutaSqlScalar(SQL)
            Me.ProgressBar1.Value = 0
            Me.ProgressBar1.Update()



            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, "
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS OBSERVACION, "
            SQL += "NVL(ASNT_FACTURA_NUMERO,' ') AS ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' '),NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2 "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE (ASNT_F_VALOR BETWEEN '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            'SQL += " AND   '" & Format(Me.mFecha2, "dd/MM/yyyy") & "')"
            SQL += " AND   '" & Format(Me.mFecha, "dd/MM/yyyy") & "')"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            '  SQL += " AND ASNT_IMPRIMIR = 'SI'"
            ' SQL += " AND ASNT_TIPO_REGISTRO = 'AC'"
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_CFCPTOS_COD ,ASNT_AUXILIAR_STRING,ASNT_FACTURA_NUMERO ASC "

            Me.DbLee.TraerLector(SQL)
            Me.ListBoxDebug.Items.Clear()
            Me.ListBoxDebug.Update()
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read

                ' renumeracion de apuntes 
                If Me.PrimerRegistro = True Then
                    Me.PrimerRegistro = False
                    Me.mAsientoSpyro = CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer)
                    Me.mAsiento = 1
                    Me.mApunte = 1
                End If

                If CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer) <> Me.mAsientoSpyro Then
                    Me.mAsientoSpyro = CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer)
                    Me.mAsiento = Me.mAsiento + 1
                    Me.mApunte = 1
                End If



                ' debug debe/HABER

                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)
                Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)


                If Indicador = "D" Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = "H" Then
                    TotalHaber = TotalHaber + Importe
                End If

                Me.Linea = ""
                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO")
                Me.TextBoxDebug.Update()
                ' Monta la Linea
                ' Indicador de Produccion o Factura

                If Me.mAsiento = 1 Then
                    TipoAsiento = IndicadorProduccion & "1" & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM") & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")
                Else
                    TipoAsiento = IndicadorFacturacion & "1" & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM") & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                End If

                Me.Linea += CType(Me.mAsiento, String).PadLeft(6, " ")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")
                Me.Linea += CType(Me.DbLee.mDbLector.Item("CUENTA"), String).PadRight(12, " ")


                ' cuenta de contrapartida para apuntes de igic

                If CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = "IGIC" Then
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"), String).PadRight(12, " ")
                Else
                    Me.Linea += " ".PadRight(12)
                End If


                ' DEBE PESETAS
                If Indicador = "D" Then
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                End If

                Me.Linea += CType(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 25), String).PadRight(25, " ")

                ' HABER PESETAS
                If Indicador = "H" Then
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                End If


                ' El registro es de Igic 
                If CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = Me.mParaTextoIva Then
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO"), String).PadLeft(8, " ")
                    ' Me.Linea += CType(Format(CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_NUMERICO"), Double), "F"), String).PadLeft(16, " ")
                    Me.Linea += CType(Format(CType("0", Double), "F"), String).PadLeft(16, " ")
                    Me.Linea += CType(Format(CType(Me.DbLee.mDbLector.Item("ASNT_TIPO_IMPUESTO"), Double), "F"), String).PadLeft(5, " ")
                Else
                    Me.Linea += "0".PadLeft(8, " ")
                    Me.Linea += CType(Format(CType("0", Double), "F"), String).PadLeft(16, " ")
                    Me.Linea += CType(Format(CType("0", Double), "F"), String).PadLeft(5, " ")
                End If

                Me.Linea += CType(Format(CType("0", Double), "F"), String).PadLeft(5, " ")
                'NUMERO DE DOCUMENTO
                Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO"), String).PadLeft(10, " ")

                ' CENTRO DE COSTO 
                If CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = "VENTA" Then
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"), String).PadRight(3, " ")
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"), String).PadRight(6, " ")
                    Me.Linea += " "
                Else
                    Me.Linea += " ".PadRight(3, " ")
                    Me.Linea += " ".PadRight(6, " ")
                    Me.Linea += " "

                End If

                ' NUMERO DE CASACION
                Me.Linea += "0".PadLeft(6, " ")

                ' TIPO DE CASADO
                Me.Linea += "0"

                ' NUMERO DE PAGO
                Me.Linea += "0".PadLeft(6, " ")

                ' CAMBIO A APLICAR
                Me.Linea += "0.000000".PadLeft(16, " ")


                'IMPORTE MONEDA EXTRANJERA
                Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")


                ' INTERNO
                Me.Linea += " "
                ' SERIE
                Me.Linea += " "
                Me.Linea += "    "
                'CODIGO DIVISA
                Me.Linea += "     "
                Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                ' MONEDA
                Me.Linea += "2"



                If Indicador = "D" Then
                    Me.Linea += CType(Format(Importe, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                End If


                If Indicador = "H" Then
                    Me.Linea += CType(Format(Importe, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                End If


                ' El registro es de Igic 
                If CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = Me.mParaTextoIva Then
                    ' BASE IMPONIBLE EUROS
                    Me.Linea += CType(Format(CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_NUMERICO"), Double), "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += CType(Format(CType("0", Double), "F"), String).PadLeft(16, " ")
                End If


                Me.Linea += "F"

                ' CIF TEMPORAL PARA HOJA DE EXCEL JOSE LUISÇ

                If Me.CheckBoxNifFinal.Checked Then
                    Me.Linea += CType(Mid(Me.DbLee.mDbLector.Item("ASNT_CIF"), 1, 25), String).PadRight(25, " ")
                End If

                ' 
                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()

            ' debug debe/haber
            Me.TextBoxDebe.Text = Me.TotalDebe
            Me.TextBoxHaber.Text = Me.TotalHaber


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Procesa Datisa")
            Exit Sub

        End Try
    End Sub
#End Region
#Region "HC7"
    Private Sub ProcesaCHC7()

        Try

            Dim TipoAsiento As String
            Dim IndicadorProduccion As String = "285"
            Dim IndicadorFacturacion As String = "286"


            ' contar
            SQL = "SELECT NVL(COUNT(*),'0')  AS TOTAL"
            SQL += "  FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR >= '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_F_VALOR <= '" & Format(Me.mFecha2, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"

            Me.ProgressBar1.Maximum = Me.DbLee.EjecutaSqlScalar(SQL)
            Me.ProgressBar1.Value = 0
            Me.ProgressBar1.Update()



            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, "
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS OBSERVACION, "
            SQL += "NVL(ASNT_FACTURA_NUMERO,' ') AS ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' '),NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2 "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR >= '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_F_VALOR <= '" & Format(Me.mFecha2, "dd/MM/yyyy") & "'"

            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " ORDER BY ASNT_F_VALOR,ASNT_CFATOCAB_REFER,ASNT_CFCPTOS_COD ASC "

            Me.DbLee.TraerLector(SQL)
            Me.ListBoxDebug.Items.Clear()
            Me.ListBoxDebug.Update()
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read

                ' renumeracion de apuntes 
                If Me.PrimerRegistro = True Then
                    Me.PrimerRegistro = False
                    Me.mAsientoSpyro = CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer)
                    Me.mAsiento = 1
                    Me.mApunte = 1
                End If

                If CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer) <> Me.mAsientoSpyro Then
                    Me.mAsientoSpyro = CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer)
                    Me.mAsiento = Me.mAsiento + 1
                    Me.mApunte = 1
                End If



                ' debug debe/HABER

                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)
                Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)


                If Indicador = "D" Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = "H" Then
                    TotalHaber = TotalHaber + Importe
                End If

                Me.Linea = ""
                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO")
                Me.TextBoxDebug.Update()
                ' Monta la Linea
                ' Indicador de Produccion o Factura

                If Me.mAsiento = 1 Then
                    TipoAsiento = IndicadorProduccion & "1" & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM") & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")
                Else
                    TipoAsiento = IndicadorFacturacion & "1" & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM") & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                End If

                Me.Linea += CType(Me.mAsiento, String).PadLeft(6, " ")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")
                Me.Linea += CType(Me.DbLee.mDbLector.Item("CUENTA"), String).PadRight(12, " ")


                ' cuenta de contrapartida para apuntes de igic

                If CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = "IGIC" Then
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"), String).PadRight(12, " ")
                Else
                    Me.Linea += " ".PadRight(12)
                End If


                ' DEBE PESETAS
                If Indicador = "D" Then
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                End If

                Me.Linea += CType(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 25), String).PadRight(25, " ")

                ' HABER PESETAS
                If Indicador = "H" Then
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                End If


                ' El registro es de Igic 
                If CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = Me.mParaTextoIva Then
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO"), String).PadLeft(8, " ")
                    Me.Linea += CType(Format(CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_NUMERICO"), Double), "F"), String).PadLeft(16, " ")
                    Me.Linea += CType(Format(CType(Me.DbLee.mDbLector.Item("ASNT_TIPO_IMPUESTO"), Double), "F"), String).PadLeft(5, " ")
                Else
                    Me.Linea += "0".PadLeft(8, " ")
                    Me.Linea += CType(Format(CType("0", Double), "F"), String).PadLeft(16, " ")
                    Me.Linea += CType(Format(CType("0", Double), "F"), String).PadLeft(5, " ")
                End If

                Me.Linea += CType(Format(CType("0", Double), "F"), String).PadLeft(5, " ")
                'NUMERO DE DOCUMENTO
                Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO"), String).PadLeft(10, " ")

                ' CENTRO DE COSTO 
                If CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = "VENTA" Then
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"), String).PadRight(3, " ")
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"), String).PadRight(6, " ")
                    Me.Linea += " "
                Else
                    Me.Linea += " ".PadRight(3, " ")
                    Me.Linea += " ".PadRight(6, " ")
                    Me.Linea += " "

                End If

                ' NUMERO DE CASACION
                Me.Linea += "0".PadLeft(6, " ")

                ' TIPO DE CASADO
                Me.Linea += "0"

                ' NUMERO DE PAGO
                Me.Linea += "0".PadLeft(6, " ")

                ' CAMBIO A APLICAR
                Me.Linea += "0.000000".PadLeft(16, " ")


                'IMPORTE MONEDA EXTRANJERA
                Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")


                ' INTERNO
                Me.Linea += " "
                Me.Linea += " "
                Me.Linea += "    "
                'CODIGO DIVISA
                Me.Linea += "     "
                Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                ' MONEDA
                Me.Linea += "2"



                If Indicador = "D" Then
                    Me.Linea += CType(Format(Importe, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                End If


                If Indicador = "H" Then
                    Me.Linea += CType(Format(Importe, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                End If


                ' BASE IMPONIBLE
                Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                Me.Linea += "F"

                ' CIF TEMPORAL PARA HOJA DE EXCEL JOSE LUISÇ

                If Me.CheckBoxNifFinal.Checked Then
                    Me.Linea += CType(Mid(Me.DbLee.mDbLector.Item("ASNT_CIF"), 1, 25), String).PadRight(25, " ")
                End If

                ' 
                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()

            ' debug debe/haber
            Me.TextBoxDebe.Text = Me.TotalDebe
            Me.TextBoxHaber.Text = Me.TotalHaber


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Procesa Datisa")
            Exit Sub

        End Try
    End Sub
#End Region
#Region "Contaplus H Lopez"
    Private Sub ProcesaContaplusLopezNewConta()

        Try

            Dim TipoAsiento As String
            Dim IndicadorProduccion As String = "285"
            Dim IndicadorFacturacion As String = "286"


            ' contar
            SQL = "SELECT NVL(COUNT(*),'0')  AS TOTAL"
            SQL += "  FROM TC_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TC_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TC_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            ' SQL += " AND ASNT_IMPRIMIR = 'SI'"
            '   SQL += " AND ASNT_TIPO_REGISTRO = 'AC'"
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"

            Me.ProgressBar1.Maximum = Me.DbLee.EjecutaSqlScalar(SQL)
            Me.ProgressBar1.Value = 0
            Me.ProgressBar1.Update()



            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, "
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS OBSERVACION, "
            SQL += "NVL(ASNT_FACTURA_NUMERO,' ') AS ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' '),NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,"
            SQL += "NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2 "
            SQL += " ,NVL(ASNT_CIF,' ') AS ASNT_CIF"
            SQL += " FROM TC_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TC_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TC_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            '  SQL += " AND ASNT_IMPRIMIR = 'SI'"
            ' SQL += " AND ASNT_TIPO_REGISTRO = 'AC'"
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_CFCPTOS_COD ASC "

            Me.DbLee.TraerLector(SQL)
            Me.ListBoxDebug.Items.Clear()
            Me.ListBoxDebug.Update()
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read

                ' renumeracion de apuntes 
                If Me.PrimerRegistro = True Then
                    Me.PrimerRegistro = False
                    Me.mAsientoSpyro = CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer)
                    Me.mAsiento = 1
                    Me.mApunte = 1
                End If

                If CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer) <> Me.mAsientoSpyro Then
                    Me.mAsientoSpyro = CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer)
                    Me.mAsiento = Me.mAsiento + 1
                    Me.mApunte = 1
                End If



                ' debug debe/HABER

                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)
                Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)


                If Indicador = "D" Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = "H" Then
                    TotalHaber = TotalHaber + Importe
                End If

                Me.Linea = ""
                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO")
                Me.TextBoxDebug.Update()
                ' Monta la Linea
                ' Indicador de Produccion o Factura

                If Me.mAsiento = 1 Then
                    TipoAsiento = IndicadorProduccion & "1" & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM") & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")
                Else
                    TipoAsiento = IndicadorFacturacion & "1" & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM") & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                End If

                Me.Linea += CType(Me.mAsiento, String).PadLeft(6, " ")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")
                Me.Linea += CType(Me.DbLee.mDbLector.Item("CUENTA"), String).PadRight(12, " ")


                ' cuenta de contrapartida para apuntes de igic

                If CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = "IGIC" Then
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"), String).PadRight(12, " ")
                Else
                    Me.Linea += " ".PadRight(12)
                End If


                ' DEBE PESETAS
                If Indicador = "D" Then
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                End If

                Me.Linea += CType(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 25), String).PadRight(25, " ")

                ' HABER PESETAS
                If Indicador = "H" Then
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                End If


                ' El registro es de Igic 
                If CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = Me.mParaTextoIva Then
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO"), String).PadLeft(8, " ")
                    Me.Linea += CType(Format(CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_NUMERICO"), Double), "F"), String).PadLeft(16, " ")
                    Me.Linea += CType(Format(CType(Me.DbLee.mDbLector.Item("ASNT_TIPO_IMPUESTO"), Double), "F"), String).PadLeft(5, " ")
                Else
                    Me.Linea += "0".PadLeft(8, " ")
                    Me.Linea += CType(Format(CType("0", Double), "F"), String).PadLeft(16, " ")
                    Me.Linea += CType(Format(CType("0", Double), "F"), String).PadLeft(5, " ")
                End If

                Me.Linea += CType(Format(CType("0", Double), "F"), String).PadLeft(5, " ")
                'NUMERO DE DOCUMENTO
                Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO"), String).PadLeft(10, " ")

                ' CENTRO DE COSTO 
                If CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = "VENTA" Then
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"), String).PadRight(3, " ")
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"), String).PadRight(6, " ")
                    Me.Linea += " "
                Else
                    Me.Linea += " ".PadRight(3, " ")
                    Me.Linea += " ".PadRight(6, " ")
                    Me.Linea += " "

                End If

                ' NUMERO DE CASACION
                Me.Linea += "0".PadLeft(6, " ")

                ' TIPO DE CASADO
                Me.Linea += "0"

                ' NUMERO DE PAGO
                Me.Linea += "0".PadLeft(6, " ")

                ' CAMBIO A APLICAR
                Me.Linea += "0.000000".PadLeft(16, " ")


                'IMPORTE MONEDA EXTRANJERA
                Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")


                ' INTERNO
                Me.Linea += " "
                Me.Linea += " "
                Me.Linea += "    "
                'CODIGO DIVISA
                Me.Linea += "     "
                Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                ' MONEDA
                Me.Linea += "2"



                If Indicador = "D" Then
                    Me.Linea += CType(Format(Importe, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                End If


                If Indicador = "H" Then
                    Me.Linea += CType(Format(Importe, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                End If


                ' BASE IMPONIBLE
                Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                Me.Linea += "F"

                ' CIF TEMPORAL PARA HOJA DE EXCEL JOSE LUISÇ

                If Me.CheckBoxNifFinal.Checked Then
                    Me.Linea += CType(Mid(Me.DbLee.mDbLector.Item("ASNT_CIF"), 1, 25), String).PadRight(25, " ")
                End If

                ' 
                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()

            ' debug debe/haber
            Me.TextBoxDebe.Text = Me.TotalDebe
            Me.TextBoxHaber.Text = Me.TotalHaber


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Procesa Datisa")
            Exit Sub

        End Try
    End Sub
    Private Sub ProcesaContaplusLopezNewStock()

        Try

            Dim TipoAsiento As String
            Dim IndicadorProduccion As String = "285"
            Dim IndicadorFacturacion As String = "286"


            ' contar
            SQL = "SELECT NVL(COUNT(*),'0')  AS TOTAL"
            SQL += "  FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            ' SQL += " AND ASNT_IMPRIMIR = 'SI'"
            '   SQL += " AND ASNT_TIPO_REGISTRO = 'AC'"
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"

            Me.ProgressBar1.Maximum = Me.DbLee.EjecutaSqlScalar(SQL)
            Me.ProgressBar1.Value = 0
            Me.ProgressBar1.Update()



            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, "
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS OBSERVACION ,"
            SQL += "NVL(ASNT_FACTURA_NUMERO,' ') AS ASNT_FACTURA_NUMERO,NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2 "
            SQL += " FROM TS_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TS_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TS_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            '  SQL += " AND ASNT_IMPRIMIR = 'SI'"
            ' SQL += " AND ASNT_TIPO_REGISTRO = 'AC'"
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_CFCPTOS_COD ASC "

            Me.DbLee.TraerLector(SQL)
            Me.ListBoxDebug.Items.Clear()
            Me.ListBoxDebug.Update()
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read

                ' renumeracion de apuntes 
                If Me.PrimerRegistro = True Then
                    Me.PrimerRegistro = False
                    Me.mAsientoSpyro = CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer)
                    Me.mAsiento = 1
                    Me.mApunte = 1
                End If

                If CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer) <> Me.mAsientoSpyro Then
                    Me.mAsientoSpyro = CType(Me.DbLee.mDbLector.Item("ASIENTO"), Integer)
                    Me.mAsiento = Me.mAsiento + 1
                    Me.mApunte = 1
                End If



                ' debug debe/HABER

                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)
                Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)


                If Indicador = "D" Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = "H" Then
                    TotalHaber = TotalHaber + Importe
                End If

                Me.Linea = ""
                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO")
                Me.TextBoxDebug.Update()
                ' Monta la Linea
                ' Indicador de Produccion o Factura

                If Me.mAsiento = 1 Then
                    TipoAsiento = IndicadorProduccion & "1" & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM") & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")
                Else
                    TipoAsiento = IndicadorFacturacion & "1" & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM") & Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                End If

                Me.Linea += CType(Me.mAsiento, String).PadLeft(6, " ")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")
                Me.Linea += CType(Mid(Me.DbLee.mDbLector.Item("CUENTA"), 1, 12), String).PadRight(12, " ")


                ' cuenta de contrapartida para apuntes de igic

                If CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = "IGIC" Then
                    Me.Linea += CType(Mid(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"), 1, 12), String).PadRight(12, " ")
                Else
                    Me.Linea += " ".PadRight(12)
                End If


                ' DEBE PESETAS
                If Indicador = "D" Then
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                End If

                Me.Linea += CType(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 25), String).PadRight(25, " ")

                ' HABER PESETAS
                If Indicador = "H" Then
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                End If


                ' El registro es de Igic 
                If CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = Me.mParaTextoIva Then
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO"), String).PadLeft(8, " ")
                    Me.Linea += CType(Format(CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_NUMERICO"), Double), "F"), String).PadLeft(16, " ")
                    Me.Linea += CType(Format(CType(Me.DbLee.mDbLector.Item("ASNT_TIPO_IMPUESTO"), Double), "F"), String).PadLeft(5, " ")
                Else
                    Me.Linea += "0".PadLeft(8, " ")
                    Me.Linea += CType(Format(CType("0", Double), "F"), String).PadLeft(16, " ")
                    Me.Linea += CType(Format(CType("0", Double), "F"), String).PadLeft(5, " ")
                End If

                Me.Linea += CType(Format(CType("0", Double), "F"), String).PadLeft(5, " ")
                'NUMERO DE DOCUMENTO
                Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO"), String).PadLeft(10, " ")

                ' CENTRO DE COSTO 
                If CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING"), String) = "VENTA" Then
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"), String).PadRight(3, " ")
                    Me.Linea += CType(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"), String).PadRight(6, " ")
                    Me.Linea += " "
                Else
                    Me.Linea += " ".PadRight(3, " ")
                    Me.Linea += " ".PadRight(6, " ")
                    Me.Linea += " "

                End If

                ' NUMERO DE CASACION
                Me.Linea += "0".PadLeft(6, " ")

                ' TIPO DE CASADO
                Me.Linea += "0"

                ' NUMERO DE PAGO
                Me.Linea += "0".PadLeft(6, " ")

                ' CAMBIO A APLICAR
                Me.Linea += "0.000000".PadLeft(16, " ")


                'IMPORTE MONEDA EXTRANJERA
                Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")


                ' INTERNO
                Me.Linea += " "
                Me.Linea += " "
                Me.Linea += "    "
                'CODIGO DIVISA
                Me.Linea += "     "
                Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                ' MONEDA
                Me.Linea += "2"



                If Indicador = "D" Then
                    Me.Linea += CType(Format(Importe, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                End If


                If Indicador = "H" Then
                    Me.Linea += CType(Format(Importe, "F"), String).PadLeft(16, " ")
                Else
                    Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                End If


                ' BASE IMPONIBLE
                Me.Linea += CType(Format(0, "F"), String).PadLeft(16, " ")
                Me.Linea += "F"

                ' CIF TEMPORAL PARA HOJA DE EXCEL JOSE LUISÇ

                If Me.CheckBoxNifFinal.Checked Then
                    Me.Linea += CType(Mid(Me.DbLee.mDbLector.Item("ASNT_CIF"), 1, 25), String).PadRight(25, " ")
                End If

                ' 
                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()

            ' debug debe/haber
            Me.TextBoxDebe.Text = Me.TotalDebe
            Me.TextBoxHaber.Text = Me.TotalHaber


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Procesa Datisa")
            Exit Sub

        End Try
    End Sub
#End Region
#Region "PARQUE TROPICAL"


    Private Sub ProcesaA3PTropicalAsientos()

        Try



            Dim RegistrosLeidos As Integer

            Dim PrimerRegistrodelTipo As Integer = 0

            '***************************************************************************************************
            ' PRODUCCCION
            '*****************************************************************************************************

            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, ABS(ASNT_I_MONEMP) AS IMPORTE2,"
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS OBSERVACION, "
            SQL += "NVL(ASNT_FACTURA_NUMERO,' ') AS ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' '),NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2 "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_CFATOCAB_REFER = 1 "
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA ASC "

            RegistrosLeidos = 0
            Me.ContarRegistros(SQL)


            Me.DbLee.TraerLector(SQL)
            Me.ListBoxDebug.Items.Clear()
            Me.ListBoxDebug.Update()
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read

                RegistrosLeidos = RegistrosLeidos + 1

                ' debug debe/HABER

                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)
                Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)


                If Indicador = "D" Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = "H" Then
                    TotalHaber = TotalHaber + Importe
                End If

                Me.Linea = ""
                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO")
                Me.TextBoxDebug.Update()
                ' Monta la Linea


                ' CONSTANTE
                Me.Linea = "4"

                ' EMPRESA
                Me.Linea += EMP_COD.PadLeft(5, "0")

                ' FECHA
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                ' CONSTANTE
                Me.Linea += "0"

                ' CUENTA

                Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")

                ' CONCEPTO
                Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                ' INDICADOR
                Me.Linea += CStr(Me.DbLee.mDbLector.Item("INDICADOR"))

                ' REF
                Me.Linea += "".PadRight(10, " ")
                ' linea del apunte   "I"  "M"  "U"  
                If RegistrosLeidos = 1 Then
                    Me.Linea += "I"
                End If
                If RegistrosLeidos < Me.m_TotalRegistros And RegistrosLeidos <> 1 Then
                    Me.Linea += "M"
                End If
                If RegistrosLeidos = Me.m_TotalRegistros Then
                    Me.Linea += "U"
                End If


                ' CONCEPTO
                Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                ' IMPORTE

                If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                    Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                Else
                    Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                End If

                ' REF
                Me.Linea += "".PadRight(138, " ")

                ' TIPO ANALITICO
                Me.Linea += " "


                ' MONEDA
                Me.Linea += "E"

                ' INDICADOR
                Me.Linea += "N"




                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()
            Me.ListBoxDebug.Items.Add("----------------------------------------------------------")

            '***************************************************************************************************
            ' FACTURAS
            '*****************************************************************************************************

            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, ABS(ASNT_I_MONEMP) AS IMPORTE2,"
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS TITULAR, "
            SQL += "NVL(ASNT_FACTURA_NUMERO,' ') AS ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' '),NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2 "
            SQL += " ,NVL(ASNT_CIF,'?') AS ASNT_CIF"

            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_CFATOCAB_REFER = 3 "
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA ASC "

            RegistrosLeidos = 0
            Me.ContarRegistros(SQL)

            Me.DbLee.TraerLector(SQL)
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read


                RegistrosLeidos = RegistrosLeidos + 1
                ' debug debe/HABER

                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)
                Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)


                If Indicador = "D" Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = "H" Then
                    TotalHaber = TotalHaber + Importe
                End If

                Me.Linea = ""
                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO")
                Me.TextBoxDebug.Update()
                ' Monta la Linea

                If RegistrosLeidos = 1 Then
                    ' RESUMEN MANO CORRIENTE 


                    ' CONSTANTE
                    Me.Linea = "4"

                    ' EMPRESA
                    Me.Linea += EMP_COD.PadLeft(5, "0")

                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                    ' CONSTANTE
                    Me.Linea += "0"

                    ' CUENTA

                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")

                    ' CONCEPTO
                    Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                    ' INDICADOR
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("INDICADOR"))

                    ' REF
                    Me.Linea += "".PadRight(10, " ")
                    ' linea del apunte   "I"  "M"  "U"  
                    If RegistrosLeidos = 1 Then
                        Me.Linea += "I"
                    End If
                    If RegistrosLeidos < Me.m_TotalRegistros And RegistrosLeidos <> 1 Then
                        Me.Linea += "M"
                    End If
                    If RegistrosLeidos = Me.m_TotalRegistros Then
                        Me.Linea += "U"
                    End If


                    ' CONCEPTO
                    Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                    ' IMPORTE

                    If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    Else
                        Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    End If

                    ' REF
                    Me.Linea += "".PadRight(138, " ")

                    ' TIPO ANALITICO
                    Me.Linea += " "


                    ' MONEDA
                    Me.Linea += "E"

                    ' INDICADOR
                    Me.Linea += "N"

                End If




                If RegistrosLeidos > 1 And CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING")) <> "IGIC" Then
                    ' CABECERA DE FACTURAS


                    ' CONSTANTE
                    Me.Linea = "4"

                    ' EMPRESA
                    Me.Linea += EMP_COD.PadLeft(5, "0")

                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                    ' CONSTANTE
                    Me.Linea += "0"

                    ' CUENTA

                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")

                    ' CONCEPTO
                    Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                    ' INDICADOR
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("INDICADOR"))

                    ' REF
                    Me.Linea += "".PadRight(10, " ")
                    ' linea del apunte   "I"  "M"  "U"  
                    If RegistrosLeidos = 1 Then
                        Me.Linea += "I"
                    End If
                    If RegistrosLeidos < Me.m_TotalRegistros And RegistrosLeidos <> 1 Then
                        Me.Linea += "M"
                    End If
                    If RegistrosLeidos = Me.m_TotalRegistros Then
                        Me.Linea += "U"
                    End If


                    ' CONCEPTO
                    Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                    ' IMPORTE

                    If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    Else
                        Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    End If

                    ' REF
                    Me.Linea += "".PadRight(138, " ")

                    ' TIPO ANALITICO
                    Me.Linea += " "


                    ' MONEDA
                    Me.Linea += "E"

                    ' INDICADOR
                    Me.Linea += "N"

                End If


                If RegistrosLeidos > 1 And CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING")) = "IGIC" Then
                    ' DETALLE DE IGIC


                    ' CONSTANTE
                    Me.Linea = "4"

                    ' EMPRESA
                    Me.Linea += EMP_COD.PadLeft(5, "0")

                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                    ' CONSTANTE
                    Me.Linea += "0"

                    ' CUENTA

                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")

                    ' CONCEPTO
                    Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                    ' INDICADOR
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("INDICADOR"))

                    ' REF
                    Me.Linea += "".PadRight(10, " ")
                    ' linea del apunte   "I"  "M"  "U"  
                    If RegistrosLeidos = 1 Then
                        Me.Linea += "I"
                    End If
                    If RegistrosLeidos < Me.m_TotalRegistros And RegistrosLeidos <> 1 Then
                        Me.Linea += "M"
                    End If
                    If RegistrosLeidos = Me.m_TotalRegistros Then
                        Me.Linea += "U"
                    End If


                    ' CONCEPTO
                    Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                    ' IMPORTE

                    If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    Else
                        Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    End If

                    ' REF
                    Me.Linea += "".PadRight(138, " ")

                    ' TIPO ANALITICO
                    Me.Linea += " "


                    ' MONEDA
                    Me.Linea += "E"

                    ' INDICADOR
                    Me.Linea += "N"

                End If



                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()
            Me.ListBoxDebug.Items.Add("----------------------------------------------------------")





            '***************************************************************************************************
            ' NOTAS DE CREDITO
            '*****************************************************************************************************

            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, ABS(ASNT_I_MONEMP) AS IMPORTE2,"
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS TITULAR, "
            SQL += "NVL(ASNT_FACTURA_NUMERO,' ') AS ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' '),NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2 "
            SQL += " ,NVL(ASNT_CIF,'?') AS ASNT_CIF"

            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_CFATOCAB_REFER = 51 "
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA ASC "

            RegistrosLeidos = 0
            Me.ContarRegistros(SQL)

            Me.DbLee.TraerLector(SQL)
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read


                RegistrosLeidos = RegistrosLeidos + 1
                ' debug debe/HABER

                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)
                Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)


                If Indicador = "D" Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = "H" Then
                    TotalHaber = TotalHaber + Importe
                End If

                Me.Linea = ""
                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO")
                Me.TextBoxDebug.Update()
                ' Monta la Linea




                ' CONSTANTE
                Me.Linea = "4"

                ' EMPRESA
                Me.Linea += EMP_COD.PadLeft(5, "0")

                ' FECHA
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                ' CONSTANTE
                Me.Linea += "0"

                ' CUENTA

                Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")

                ' CONCEPTO
                Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                ' INDICADOR
                Me.Linea += CStr(Me.DbLee.mDbLector.Item("INDICADOR"))

                ' REF
                Me.Linea += "".PadRight(10, " ")
                ' linea del apunte   "I"  "M"  "U"  
                If RegistrosLeidos = 1 Then
                    Me.Linea += "I"
                End If
                If RegistrosLeidos < Me.m_TotalRegistros And RegistrosLeidos <> 1 Then
                    Me.Linea += "M"
                End If
                If RegistrosLeidos = Me.m_TotalRegistros Then
                    Me.Linea += "U"
                End If


                ' CONCEPTO
                Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                ' IMPORTE

                If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                    Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                Else
                    Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                End If

                ' REF
                Me.Linea += "".PadRight(138, " ")

                ' TIPO ANALITICO
                Me.Linea += " "


                ' MONEDA
                Me.Linea += "E"

                ' INDICADOR
                Me.Linea += "N"






                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()
            Me.ListBoxDebug.Items.Add("----------------------------------------------------------")



            '***************************************************************************************************
            ' COBROS
            '*****************************************************************************************************

            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, ABS(ASNT_I_MONEMP) AS IMPORTE2,"
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS OBSERVACION, "
            SQL += "NVL(ASNT_FACTURA_NUMERO,' ') AS ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' '),NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2 "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_CFATOCAB_REFER = 35 "
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA ASC "

            RegistrosLeidos = 0
            Me.ContarRegistros(SQL)

            Me.DbLee.TraerLector(SQL)
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read

                RegistrosLeidos = RegistrosLeidos + 1


                ' debug debe/HABER

                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)
                Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)


                If Indicador = "D" Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = "H" Then
                    TotalHaber = TotalHaber + Importe
                End If

                Me.Linea = ""
                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO")
                Me.TextBoxDebug.Update()
                ' Monta la Linea
                ' Indicador de Produccion o Factura


                ' CONSTANTE
                Me.Linea += "4"

                ' EMPRESA
                Me.Linea += EMP_COD.PadLeft(5, "0")

                ' FECHA
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                ' CONSTANTE
                Me.Linea += "0"

                ' CUENTA

                Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")

                ' CONCEPTO
                Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                ' INDICADOR
                Me.Linea += CStr(Me.DbLee.mDbLector.Item("INDICADOR"))

                ' REF
                Me.Linea += "".PadRight(10, " ")

                ' linea del apunte   "I"  "M"  "U"  
                If RegistrosLeidos = 1 Then
                    Me.Linea += "I"
                End If
                If RegistrosLeidos < Me.m_TotalRegistros And RegistrosLeidos <> 1 Then
                    Me.Linea += "M"
                End If
                If RegistrosLeidos = Me.m_TotalRegistros Then
                    Me.Linea += "U"
                End If


                ' CONCEPTO
                Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                ' IMPORTE

                If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                    Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                Else
                    Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                End If

                ' REF
                Me.Linea += "".PadRight(138, " ")

                ' TIPO ANALITICO
                Me.Linea += " "


                ' MONEDA
                Me.Linea += "E"

                ' INDICADOR
                Me.Linea += "N"


                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()
            Me.ListBoxDebug.Items.Add("----------------------------------------------------------")




            '***************************************************************************************************
            ' AJUSTE REDONDEO
            '*****************************************************************************************************

            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, ABS(ASNT_I_MONEMP) AS IMPORTE2,"
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS OBSERVACION, "
            SQL += "NVL(ASNT_FACTURA_NUMERO,' ') AS ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' '),NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2 "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_CFATOCAB_REFER = 999 "
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA ASC "

            Me.DbLee.TraerLector(SQL)
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read


                ' debug debe/HABER

                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)
                Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)


                If Indicador = "D" Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = "H" Then
                    TotalHaber = TotalHaber + Importe
                End If

                Me.Linea = ""
                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO")
                Me.TextBoxDebug.Update()


                ' CONSTANTE
                Me.Linea += "4"

                ' EMPRESA
                Me.Linea += EMP_COD.PadLeft(5, "0")

                ' FECHA
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                ' CONSTANTE
                Me.Linea += "0"

                ' CUENTA

                Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")

                ' CONCEPTO
                Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                ' INDICADOR
                Me.Linea += CStr(Me.DbLee.mDbLector.Item("INDICADOR"))

                ' REF
                Me.Linea += "".PadRight(10, " ")

                ' linea del apunte   "I"  "M"  "U"  
                If RegistrosLeidos = 1 Then
                    Me.Linea += "I"
                End If
                If RegistrosLeidos < Me.m_TotalRegistros And RegistrosLeidos <> 1 Then
                    Me.Linea += "M"
                End If
                If RegistrosLeidos = Me.m_TotalRegistros Then
                    Me.Linea += "U"
                End If


                ' CONCEPTO
                Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                ' IMPORTE

                If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                    Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                Else
                    Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                End If

                ' REF
                Me.Linea += "".PadRight(138, " ")

                ' TIPO ANALITICO
                Me.Linea += " "


                ' MONEDA
                Me.Linea += "E"

                ' INDICADOR
                Me.Linea += "N"




                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()
            Me.ListBoxDebug.Items.Add("----------------------------------------------------------")



            ' debug debe/haber
            Me.TextBoxDebe.Text = Me.TotalDebe
            Me.TextBoxHaber.Text = Me.TotalHaber


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Procesa Datisa")
            Exit Sub

        End Try
    End Sub
    Private Sub ProcesaA3PTropicalAsientosLibrodeIgic()

        Try

            ' Dim TipoAsiento As String

            Dim RegistrosLeidos As Integer

            Dim PrimerRegistrodelTipo As Integer = 0


            '***************************************************************************************************
            ' FACTURAS
            '*****************************************************************************************************

            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, ABS(ASNT_I_MONEMP) AS IMPORTE2,"
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS TITULAR, "
            SQL += "NVL(ASNT_FACTURA_NUMERO,' ') AS ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' '),NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2 "
            SQL += " ,NVL(ASNT_CIF,'?') AS ASNT_CIF"
            SQL += " ,ASNT_LIN_DOCU AS DOCUMENTO,ASNT_LIN_VLIQ AS BASE ,ABS(ASNT_LIN_VLIQ) AS BASE2,ASNT_LIN_TIIMP AS TIPOI,NVL(ASNT_AUXILIAR_STRING3,' ') AS ASNT_AUXILIAR_STRING3 "

            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_CFATOCAB_REFER = 3 "

            '' PARA EVITAR EL RESUMEN A LA MANO CORRIENTE 
            SQL += " AND ASNT_AUXILIAR_STRING IS NOT NULL "

            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA ASC "

            RegistrosLeidos = 0
            Me.ContarRegistros(SQL)

            Me.DbLee.TraerLector(SQL)
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read




                If CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING")) <> "IGIC" Then
                    ' CABECERA DE FACTURAS

                    ' CONSTANTE
                    Me.Linea = "4"
                    ' EMPRESA
                    Me.Linea += EMP_COD.PadLeft(5, "0")

                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                    ' CONSTANTE  1 = FACTURAS 2=ABONOS
                    If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                        Me.Linea += "1"
                    Else
                        ' para serie de anulacion ??
                        Me.Linea += "2"
                    End If


                    ' CUENTA

                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")

                    ' CONCEPTO
                    Me.Linea += Mid(CStr(Me.DbLee.mDbLector.Item("TITULAR")), 1, 30).PadRight(30, " ")


                    ' TIPO DE FACTURA 1=VENTAS 2=COMPRAS
                    Me.Linea += "1"


                    ' NUMERO DE FACTURA
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CONCEPTO")).PadRight(10, " ")


                    ' linea del apunte   "I"  "M"  "U"  
                    Me.Linea += "I"


                    ' CONCEPTO
                    Me.Linea += Mid(CStr(Me.DbLee.mDbLector.Item("CONCEPTO")), 1, 30).PadRight(30, " ")

                    ' IMPORTE

                    If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    Else
                        ' Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                        ' se envia el importe positivo !!!! y mas arriba se indica que es un  tipo de registro abono 
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    End If

                    ' RESERVA
                    Me.Linea += "".PadRight(62, " ")

                    ' CIF
                    Me.Linea += Mid(CStr(Me.DbLee.mDbLector.Item("ASNT_CIF")), 1, 14).PadRight(14, " ")

                    ' CLIENTE
                    Me.Linea += Mid(CStr(Me.DbLee.mDbLector.Item("TITULAR")), 1, 40).PadRight(40, " ")


                    ' C POSTAL
                    Me.Linea += "".PadRight(5, " ")

                    ' RESERVA
                    Me.Linea += "".PadRight(2, " ")


                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")


                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")



                    ' MONEDA
                    Me.Linea += "E"

                    ' INDICADOR
                    Me.Linea += "N"
                End If


                If CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING")) = "IGIC" Then
                    ' DETALLE DE IGIC

                    ' CONSTANTE
                    Me.Linea = "4"
                    ' EMPRESA
                    Me.Linea += EMP_COD.PadLeft(5, "0")

                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                    ' CONSTANTE
                    Me.Linea += "9"

                    ' CUENTA

                    ' CUENTA DE IGIC
                    'Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")
                    ' CUENTA DE CLIENTE
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING3")).PadRight(12, " ")

                    ' CONCEPTO
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CONCEPTO")).PadRight(30, " ")

                    If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                        ' FACTURAS = CARGO
                        Me.Linea += "C"
                    Else
                        ' FACTURAS NEGATIVAS = ABONO
                        Me.Linea += "A"
                    End If

                    ' NUMERO DE FACTURA
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("DOCUMENTO")).PadRight(10, " ")

                    ' linea del apunte   "I"  "M"  "U"  
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"))



                    ' CONCEPTO
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CONCEPTO")).PadRight(30, " ")


                    ' FACTURAS EXPEDIDAS
                    Me.Linea += "01"


                    ' BASE IMPONIBLE 
                    If CDec(Me.DbLee.mDbLector.Item("BASE")) >= 0 Then
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("BASE2")), "F")).PadLeft(13, "0")
                    Else
                        Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("BASE2")), "F")).PadLeft(13, "0")
                    End If

                    ' PORCENTAJE DE IVA


                    Me.Linea += CStr(Format(CDec(Me.DbLee.mDbLector.Item("TIPOI")), "F")).PadLeft(5, "0")

                    ' CUOTA DE IVA

                    If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    Else
                        Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    End If

                    ' porcentaje de recargo
                    Me.Linea += "00.00"

                    ' cuota de recargo
                    Me.Linea += "+0000000000.00"

                    ' porcentaje de retencion
                    Me.Linea += "00.00"

                    ' cuota de retencion
                    Me.Linea += "+0000000000.00"


                    ' impreso 01=347 
                    Me.Linea += "01"

                    ' sujeto a iva
                    Me.Linea += "S"

                    ' AFECTRA AL 415
                    Me.Linea += "S"


                    ' RESERVA
                    Me.Linea += "".PadRight(75, " ")

                    ' TIPO DE REGISTRO ANALITOCO  S ????
                    Me.Linea += " "



                    ' MONEDA
                    Me.Linea += "E"

                    ' INDICADOR
                    Me.Linea += "N"

                End If



                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()
            Me.ListBoxDebug.Items.Add("----------------------------------------------------------")




            '***************************************************************************************************
            ' NOTAS DE CREDITO
            '*****************************************************************************************************

            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, ABS(ASNT_I_MONEMP) AS IMPORTE2,"
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS TITULAR, "
            SQL += "NVL(ASNT_FACTURA_NUMERO,' ') AS ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' ') AS ASNT_FACTURA_SERIE,NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2 "
            SQL += " ,NVL(ASNT_CIF,'?') AS ASNT_CIF"

            SQL += " ,ASNT_FACTURA_NUMERO || '/' || ASNT_FACTURA_SERIE AS DOCUMENTO "
            SQL += " ,ASNT_LIN_VLIQ AS BASE ,ASNT_LIN_TIIMP AS TIPOI,NVL(ASNT_AUXILIAR_STRING3,' ') AS ASNT_AUXILIAR_STRING3 "

            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_CFATOCAB_REFER = 51 "

            '' PARA EVITAR EL RESUMEN A LA MANO CORRIENTE 
            SQL += " AND ASNT_AUXILIAR_STRING <> 'RESUMEN' "

            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA ASC "

            RegistrosLeidos = 0
            Me.ContarRegistros(SQL)

            Me.DbLee.TraerLector(SQL)
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read




                If CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING")) <> "IGIC" Then
                    ' CABECERA DE FACTURAS

                    ' CONSTANTE
                    Me.Linea = "4"
                    ' EMPRESA
                    Me.Linea += EMP_COD.PadLeft(5, "0")

                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                    ' CONSTANTE  1 = FACTURAS 2=ABONOS

                    Me.Linea += "2"



                    ' CUENTA
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")


                    ' CONCEPTO
                    Me.Linea += Mid(CStr(Me.DbLee.mDbLector.Item("TITULAR")), 1, 30).PadRight(30, " ")


                    ' TIPO DE FACTURA 1=VENTAS 2=COMPRAS
                    Me.Linea += "1"


                    ' NUMERO DE FACTURA
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CONCEPTO")).PadRight(10, " ")


                    ' linea del apunte   "I"  "M"  "U"  
                    Me.Linea += "I"


                    ' CONCEPTO
                    Me.Linea += Mid(CStr(Me.DbLee.mDbLector.Item("CONCEPTO")), 1, 30).PadRight(30, " ")

                    ' IMPORTE

                    If Me.DbLee.mDbLector.Item("INDICADOR") = "D" Then
                        ' NOTA DE CREDITO
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    Else
                        ' NOTA DE CREDITO ANULADA
                        Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    End If

                    ' RESERVA
                    Me.Linea += "".PadRight(62, " ")

                    ' CIF
                    Me.Linea += Mid(CStr(Me.DbLee.mDbLector.Item("ASNT_CIF")), 1, 14).PadRight(14, " ")

                    ' CLIENTE
                    Me.Linea += Mid(CStr(Me.DbLee.mDbLector.Item("TITULAR")), 1, 40).PadRight(40, " ")


                    ' C POSTAL
                    Me.Linea += "".PadRight(5, " ")

                    ' RESERVA
                    Me.Linea += "".PadRight(2, " ")


                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")


                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")



                    ' MONEDA
                    Me.Linea += "E"

                    ' INDICADOR
                    Me.Linea += "N"
                End If


                If CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING")) = "IGIC" Then
                    ' DETALLE DE IGIC

                    ' CONSTANTE
                    Me.Linea = "4"
                    ' EMPRESA
                    Me.Linea += EMP_COD.PadLeft(5, "0")

                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                    ' CONSTANTE
                    Me.Linea += "9"

                    ' CUENTA
                    ' cuenta de igic
                    'Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")
                    ' CUENTA DE CLIENTE
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING3")).PadRight(12, " ")

                    ' CONCEPTO
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CONCEPTO")).PadRight(30, " ")


                    ' CONSTANTE C= CARGO A = ABONO

                    If Me.DbLee.mDbLector.Item("INDICADOR") = "D" Then
                        ' NOTA DE CREDITO  = ABONO ?
                        Me.Linea += "A"
                    Else
                        ' NOTA DE CREDITO ANULADA = CARGO
                        Me.Linea += "C"

                    End If



                    ' NUMERO DE FACTURA
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("DOCUMENTO")).PadRight(10, " ")

                    ' linea del apunte   "I"  "M"  "U"  
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"))



                    ' CONCEPTO
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CONCEPTO")).PadRight(30, " ")


                    ' FACTURAS EXPEDIDAS
                    Me.Linea += "01"


                    ' BASE IMPONIBLE 
                    If CDec(Me.DbLee.mDbLector.Item("BASE")) >= 0 Then
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("BASE")), "F")).PadLeft(13, "0")
                    Else
                        Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("BASE")), "F")).PadLeft(13, "0")
                    End If

                    ' PORCENTAJE DE IVA


                    Me.Linea += CStr(Format(CDec(Me.DbLee.mDbLector.Item("TIPOI")), "F")).PadLeft(5, "0")

                    ' CUOTA DE IVA

                    If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    Else
                        Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    End If


                    ' porcentaje de recargo
                    Me.Linea += "00.00"

                    ' cuota de recargo
                    Me.Linea += "+0000000000.00"

                    ' porcentaje de retencion
                    Me.Linea += "00.00"

                    ' cuota de retencion
                    Me.Linea += "+0000000000.00"


                    ' impreso 01=347 
                    Me.Linea += "01"

                    ' sujeto a iva
                    Me.Linea += "S"

                    ' AFECTRA AL 415
                    Me.Linea += "S"


                    ' RESERVA
                    Me.Linea += "".PadRight(75, " ")

                    ' TIPO DE REGISTRO ANALITOCO  S ????
                    Me.Linea += " "



                    ' MONEDA
                    Me.Linea += "E"

                    ' INDICADOR
                    Me.Linea += "N"



                End If



                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()
            Me.ListBoxDebug.Items.Add("------------------------------------------------------")







            ' debug debe/haber
            Me.TextBoxDebe.Text = Me.TotalDebe
            Me.TextBoxHaber.Text = Me.TotalHaber


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Procesa Datisa")
            Exit Sub

        End Try
    End Sub
    Private Sub ProcesaA3PTropicalAsientos2()

        Try



            Dim RegistrosLeidos As Integer

            Dim PrimerRegistrodelTipo As Integer = 0

            '***************************************************************************************************
            ' PRODUCCCION
            '*****************************************************************************************************

            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, ABS(ASNT_I_MONEMP) AS IMPORTE2,"
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS OBSERVACION, "
            SQL += "NVL(ASNT_FACTURA_NUMERO,' ') AS ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' '),NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2 "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_CFATOCAB_REFER = 1 "
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA ASC "

            RegistrosLeidos = 0
            Me.ContarRegistros(SQL)


            Me.DbLee.TraerLector(SQL)
            Me.ListBoxDebug.Items.Clear()
            Me.ListBoxDebug.Update()
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read

                RegistrosLeidos = RegistrosLeidos + 1

                ' debug debe/HABER

                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)
                Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)


                If Indicador = "D" Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = "H" Then
                    TotalHaber = TotalHaber + Importe
                End If

                Me.Linea = ""
                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO")
                Me.TextBoxDebug.Update()
                ' Monta la Linea


                ' CONSTANTE
                Me.Linea = "4"

                ' EMPRESA
                Me.Linea += EMP_COD.PadLeft(5, "0")

                ' FECHA
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                ' CONSTANTE
                Me.Linea += "0"

                ' CUENTA

                Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")

                ' CONCEPTO
                Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                ' INDICADOR
                Me.Linea += CStr(Me.DbLee.mDbLector.Item("INDICADOR"))

                ' REF
                Me.Linea += "".PadRight(10, " ")
                ' linea del apunte   "I"  "M"  "U"  
                If RegistrosLeidos = 1 Then
                    Me.Linea += "I"
                End If
                If RegistrosLeidos < Me.m_TotalRegistros And RegistrosLeidos <> 1 Then
                    Me.Linea += "M"
                End If
                If RegistrosLeidos = Me.m_TotalRegistros Then
                    Me.Linea += "U"
                End If


                ' CONCEPTO
                Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                ' IMPORTE

                If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                    Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                Else
                    Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                End If

                ' REF
                Me.Linea += "".PadRight(138, " ")

                ' TIPO ANALITICO
                Me.Linea += " "


                ' MONEDA
                Me.Linea += "E"

                ' INDICADOR
                Me.Linea += "N"




                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()
            Me.ListBoxDebug.Items.Add("----------------------------------------------------------")





            '***************************************************************************************************
            ' COBROS
            '*****************************************************************************************************

            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, ABS(ASNT_I_MONEMP) AS IMPORTE2,"
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS OBSERVACION, "
            SQL += "NVL(ASNT_FACTURA_NUMERO,' ') AS ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' '),NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2 "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_CFATOCAB_REFER = 35 "
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA ASC "

            RegistrosLeidos = 0
            Me.ContarRegistros(SQL)

            Me.DbLee.TraerLector(SQL)
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read

                RegistrosLeidos = RegistrosLeidos + 1


                ' debug debe/HABER

                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)
                Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)


                If Indicador = "D" Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = "H" Then
                    TotalHaber = TotalHaber + Importe
                End If

                Me.Linea = ""
                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO")
                Me.TextBoxDebug.Update()
                ' Monta la Linea
                ' Indicador de Produccion o Factura


                ' CONSTANTE
                Me.Linea += "4"

                ' EMPRESA
                Me.Linea += EMP_COD.PadLeft(5, "0")

                ' FECHA
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                ' CONSTANTE
                Me.Linea += "0"

                ' CUENTA

                Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")

                ' CONCEPTO
                Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                ' INDICADOR
                Me.Linea += CStr(Me.DbLee.mDbLector.Item("INDICADOR"))

                ' REF
                Me.Linea += "".PadRight(10, " ")

                ' linea del apunte   "I"  "M"  "U"  
                If RegistrosLeidos = 1 Then
                    Me.Linea += "I"
                End If
                If RegistrosLeidos < Me.m_TotalRegistros And RegistrosLeidos <> 1 Then
                    Me.Linea += "M"
                End If
                If RegistrosLeidos = Me.m_TotalRegistros Then
                    Me.Linea += "U"
                End If


                ' CONCEPTO
                Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                ' IMPORTE

                If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                    Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                Else
                    Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                End If

                ' REF
                Me.Linea += "".PadRight(138, " ")

                ' TIPO ANALITICO
                Me.Linea += " "


                ' MONEDA
                Me.Linea += "E"

                ' INDICADOR
                Me.Linea += "N"


                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()
            Me.ListBoxDebug.Items.Add("----------------------------------------------------------")




            '***************************************************************************************************
            ' AJUSTE REDONDEO
            '*****************************************************************************************************

            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, ABS(ASNT_I_MONEMP) AS IMPORTE2,"
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS OBSERVACION, "
            SQL += "NVL(ASNT_FACTURA_NUMERO,' ') AS ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' '),NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2 "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_CFATOCAB_REFER = 999 "
            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA ASC "

            Me.DbLee.TraerLector(SQL)
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read


                ' debug debe/HABER

                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)
                Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)


                If Indicador = "D" Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = "H" Then
                    TotalHaber = TotalHaber + Importe
                End If

                Me.Linea = ""
                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO")
                Me.TextBoxDebug.Update()


                ' CONSTANTE
                Me.Linea += "4"

                ' EMPRESA
                Me.Linea += EMP_COD.PadLeft(5, "0")

                ' FECHA
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                ' CONSTANTE
                Me.Linea += "0"

                ' CUENTA

                Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")

                ' CONCEPTO
                Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                ' INDICADOR
                Me.Linea += CStr(Me.DbLee.mDbLector.Item("INDICADOR"))

                ' REF
                Me.Linea += "".PadRight(10, " ")

                ' linea del apunte   "I"  "M"  "U"  
                If RegistrosLeidos = 1 Then
                    Me.Linea += "I"
                End If
                If RegistrosLeidos < Me.m_TotalRegistros And RegistrosLeidos <> 1 Then
                    Me.Linea += "M"
                End If
                If RegistrosLeidos = Me.m_TotalRegistros Then
                    Me.Linea += "U"
                End If


                ' CONCEPTO
                Me.Linea += CStr(Mid(Me.DbLee.mDbLector.Item("CONCEPTO"), 1, 30)).PadRight(30, " ")

                ' IMPORTE

                If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                    Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                Else
                    Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                End If

                ' REF
                Me.Linea += "".PadRight(138, " ")

                ' TIPO ANALITICO
                Me.Linea += " "


                ' MONEDA
                Me.Linea += "E"

                ' INDICADOR
                Me.Linea += "N"




                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()
            Me.ListBoxDebug.Items.Add("----------------------------------------------------------")



            ' debug debe/haber
            Me.TextBoxDebe.Text = Me.TotalDebe
            Me.TextBoxHaber.Text = Me.TotalHaber


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Procesa Datisa")
            Exit Sub

        End Try
    End Sub
    Private Sub ProcesaA3PTropicalAsientosLibrodeIgic2()


        ' DACTURAS ANULADAS                        IMPORTE NEGATIVO   CABECERA= 2 RECTIFICA  LIBRO = A ABONO   SIGNO CAB   +  SIGNO LINE -
        ' NOTAS DE ABONO                DEBE       IMPORTE NEGATIVO   CABECERA= 2 RECTIFICA  LIBRO = A ABONO   SIGNO CAB   +  SIGNO LINE -      DEBE

        ' FACTURAS                                IMPOERTE POSITOVO   CABECERA= 1 FACTURA    LIBRO = C CARGO   SIGNO CAB   +  SIGNO LINE +  
        ' NOTAS DE ABOO ANULADAS       HABER      IMPOERTE POSITOVO   CABECERA= 1 FACTURA    LIBRO = C CARGO   SIGNO CAB   +  SIGNO LINE +      HABER
        Try

            '  Dim TipoAsiento As String

            Dim RegistrosLeidos As Integer

            Dim PrimerRegistrodelTipo As Integer = 0

            Dim ControlDni As String


            '***************************************************************************************************
            ' FACTURAS
            '*****************************************************************************************************

            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, ABS(ASNT_I_MONEMP) AS IMPORTE2,"
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS TITULAR, "
            SQL += "NVL(ASNT_FACTURA_NUMERO,' ') AS ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' '),NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2 "
            SQL += " ,NVL(ASNT_CIF,' ') AS ASNT_CIF"
            SQL += " ,ASNT_LIN_DOCU AS DOCUMENTO,ASNT_LIN_VLIQ AS BASE ,ABS(ASNT_LIN_VLIQ) AS BASE2,ASNT_LIN_TIIMP AS TIPOI,NVL(ASNT_AUXILIAR_STRING3,' ') AS ASNT_AUXILIAR_STRING3 "

            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_CFATOCAB_REFER = 3 "

            '' PARA EVITAR EL RESUMEN A LA MANO CORRIENTE 
            SQL += " AND ASNT_AUXILIAR_STRING IS NOT NULL "

            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA ASC "

            RegistrosLeidos = 0
            Me.ContarRegistros(SQL)

            Me.DbLee.TraerLector(SQL)
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read




                If CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING")) <> "IGIC" Then
                    ' CABECERA DE FACTURAS

                    ' CONSTANTE
                    Me.Linea = "4"
                    ' EMPRESA
                    Me.Linea += EMP_COD.PadLeft(5, "0")

                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                    ' CONSTANTE  1 = FACTURAS 2=ABONOS
                    If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                        Me.Linea += "1"
                    Else
                        ' para serie de anulacion ??
                        Me.Linea += "2"
                    End If


                    ' CUENTA

                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")

                    ' CONCEPTO
                    Me.Linea += Mid(CStr(Me.DbLee.mDbLector.Item("TITULAR")), 1, 30).PadRight(30, " ")


                    ' TIPO DE FACTURA 1=VENTAS 2=COMPRAS
                    Me.Linea += "1"


                    ' NUMERO DE FACTURA
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CONCEPTO")).PadRight(10, " ")


                    ' linea del apunte   "I"  "M"  "U"  
                    Me.Linea += "I"


                    ' CONCEPTO
                    Me.Linea += Mid(CStr(Me.DbLee.mDbLector.Item("CONCEPTO")), 1, 30).PadRight(30, " ")
                    ' Me.Linea += Mid(CStr(Me.DbLee.mDbLector.Item("TITULAR")), 1, 30).PadRight(30, " ")


                    ' IMPORTE

                    If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    Else
                        ' Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                        ' se envia el importe positivo !!!! y mas arriba se indica que es un  tipo de registro abono 
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    End If

                    ' RESERVA
                    Me.Linea += "".PadRight(62, " ")


                    ' CIF

                    ControlDni = CStr(Me.DbLee.mDbLector.Item("ASNT_CIF"))

                    If ControlDni = "0" Then
                        ControlDni = " "
                    End If

                    'Me.Linea += Mid(CStr(Me.DbLee.mDbLector.Item("ASNT_CIF")), 1, 14).PadRight(14, " ")



                    ' 20140318  dsbloquear abajo para enviar el cif 
                    'Me.Linea += Mid(ControlDni, 1, 14).PadRight(14, " ")
                    ' borrar abajo aqui 
                    Me.Linea += Mid("", 1, 14).PadRight(14, " ")



                    ' CLIENTE
                    Me.Linea += Mid(CStr(Me.DbLee.mDbLector.Item("TITULAR")), 1, 40).PadRight(40, " ")


                    ' C POSTAL
                    Me.Linea += "".PadRight(5, " ")

                    ' RESERVA
                    Me.Linea += "".PadRight(2, " ")


                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")


                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")



                    ' MONEDA
                    Me.Linea += "E"

                    ' INDICADOR
                    Me.Linea += "N"
                End If


                If CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING")) = "IGIC" Then
                    ' DETALLE DE IGIC

                    ' CONSTANTE
                    Me.Linea = "4"
                    ' EMPRESA
                    Me.Linea += EMP_COD.PadLeft(5, "0")

                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                    ' CONSTANTE
                    Me.Linea += "9"

                    ' CUENTA

                    ' CUENTA DE IGIC
                    'Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")
                    ' CUENTA DE CLIENTE
                    'Me.Linea += CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING3")).PadRight(12, " ")
                    ' CUENTA MANO CORRIENTE
                    Me.Linea += Me.mParaCuentaMano.PadRight(12, " ")

                    ' CONCEPTO
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CONCEPTO")).PadRight(30, " ")

                    If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                        ' FACTURAS = CARGO
                        Me.Linea += "C"
                    Else
                        ' FACTURAS NEGATIVAS = ABONO
                        Me.Linea += "A"
                    End If

                    ' NUMERO DE FACTURA
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("DOCUMENTO")).PadRight(10, " ")

                    ' linea del apunte   "I"  "M"  "U"  
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"))



                    ' CONCEPTO
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CONCEPTO")).PadRight(30, " ")


                    ' FACTURAS EXPEDIDAS
                    Me.Linea += "01"


                    ' BASE IMPONIBLE 
                    If CDec(Me.DbLee.mDbLector.Item("BASE")) >= 0 Then
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("BASE2")), "F")).PadLeft(13, "0")
                    Else
                        Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("BASE2")), "F")).PadLeft(13, "0")
                    End If

                    ' PORCENTAJE DE IVA


                    Me.Linea += CStr(Format(CDec(Me.DbLee.mDbLector.Item("TIPOI")), "F")).PadLeft(5, "0")

                    ' CUOTA DE IVA

                    If CDec(Me.DbLee.mDbLector.Item("IMPORTE")) >= 0 Then
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    Else
                        Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    End If

                    ' porcentaje de recargo
                    Me.Linea += "00.00"

                    ' cuota de recargo
                    Me.Linea += "+0000000000.00"

                    ' porcentaje de retencion
                    Me.Linea += "00.00"

                    ' cuota de retencion
                    Me.Linea += "+0000000000.00"


                    ' impreso 01=347 
                    Me.Linea += "01"

                    ' sujeto a iva
                    Me.Linea += "S"

                    ' AFECTRA AL 415
                    Me.Linea += "S"


                    ' RESERVA
                    Me.Linea += "".PadRight(75, " ")

                    ' TIPO DE REGISTRO ANALITOCO  S ????
                    Me.Linea += " "



                    ' MONEDA
                    Me.Linea += "E"

                    ' INDICADOR
                    Me.Linea += "N"

                End If



                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()
            Me.ListBoxDebug.Items.Add("----------------------------------------------------------")




            '***************************************************************************************************
            ' NOTAS DE CREDITO
            '*****************************************************************************************************

            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, ABS(ASNT_I_MONEMP) AS IMPORTE2,"
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS TITULAR, "
            SQL += "NVL(ASNT_FACTURA_NUMERO,' ') AS ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' ') AS ASNT_FACTURA_SERIE,NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2 "
            SQL += " ,NVL(ASNT_CIF,' ') AS ASNT_CIF"

            SQL += " ,ASNT_FACTURA_NUMERO || '/' || ASNT_FACTURA_SERIE AS DOCUMENTO "
            SQL += " ,ASNT_LIN_VLIQ AS BASE ,ABS(ASNT_LIN_VLIQ) AS BASE2,ASNT_LIN_TIIMP AS TIPOI,NVL(ASNT_AUXILIAR_STRING3,' ') AS ASNT_AUXILIAR_STRING3 "

            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(Me.mFecha, "dd/MM/yyyy") & "'"
            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
            SQL += " AND TH_ASNT.ASNT_CFATOCAB_REFER = 51 "

            '' PARA EVITAR EL RESUMEN A LA MANO CORRIENTE 
            SQL += " AND ASNT_AUXILIAR_STRING <> 'RESUMEN' "

            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA ASC "

            RegistrosLeidos = 0
            Me.ContarRegistros(SQL)


            Dim NotaAnulada As Boolean = False
            Me.DbLee.TraerLector(SQL)
            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read




                If CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING")) <> "IGIC" Then
                    ' CABECERA DE FACTURAS

                    ' CONSTANTE
                    Me.Linea = "4"
                    ' EMPRESA
                    Me.Linea += EMP_COD.PadLeft(5, "0")

                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                    ' CONSTANTE  1 = FACTURAS 2=ABONOS
                    If Me.DbLee.mDbLector.Item("INDICADOR") = "D" Then
                        ' NOTA DE CREDITO
                        Me.Linea += "2"
                    Else
                        ' NOTA DE CREDITO ANULADA
                        Me.Linea += "1"
                        'Me.Linea += "2"
                    End If



                    ' CUENTA
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")


                    ' CONCEPTO
                    Me.Linea += Mid(CStr(Me.DbLee.mDbLector.Item("TITULAR")), 1, 30).PadRight(30, " ")


                    ' TIPO DE FACTURA 1=VENTAS 2=COMPRAS
                    Me.Linea += "1"


                    ' NUMERO DE FACTURA
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CONCEPTO")).PadRight(10, " ")


                    ' linea del apunte   "I"  "M"  "U"  
                    Me.Linea += "I"


                    ' CONCEPTO
                    Me.Linea += Mid(CStr(Me.DbLee.mDbLector.Item("CONCEPTO")), 1, 30).PadRight(30, " ")

                    ' IMPORTE

                    If Me.DbLee.mDbLector.Item("INDICADOR") = "D" Then
                        ' NOTA DE CREDITO
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                        NotaAnulada = False
                    Else
                        ' NOTA DE CREDITO ANULADA
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                        NotaAnulada = True
                    End If

                    ' RESERVA
                    Me.Linea += "".PadRight(62, " ")


                    ' CIF

                    ControlDni = CStr(Me.DbLee.mDbLector.Item("ASNT_CIF"))

                    If ControlDni = "0" Then
                        ControlDni = " "
                    End If

                    'Me.Linea += Mid(CStr(Me.DbLee.mDbLector.Item("ASNT_CIF")), 1, 14).PadRight(14, " ")
                    Me.Linea += Mid(ControlDni, 1, 14).PadRight(14, " ")

                    ' CLIENTE
                    Me.Linea += Mid(CStr(Me.DbLee.mDbLector.Item("TITULAR")), 1, 40).PadRight(40, " ")


                    ' C POSTAL
                    Me.Linea += "".PadRight(5, " ")

                    ' RESERVA
                    Me.Linea += "".PadRight(2, " ")


                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")


                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")



                    ' MONEDA
                    Me.Linea += "E"

                    ' INDICADOR
                    Me.Linea += "N"
                End If


                If CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING")) = "IGIC" Then
                    ' DETALLE DE IGIC

                    ' CONSTANTE
                    Me.Linea = "4"
                    ' EMPRESA
                    Me.Linea += EMP_COD.PadLeft(5, "0")

                    ' FECHA
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "yyyy")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "MM")
                    Me.Linea += Format(CType(Me.DbLee.mDbLector.Item("F_VALOR"), Date), "dd")

                    ' CONSTANTE
                    Me.Linea += "9"

                    ' CUENTA
                    ' cuenta de igic
                    'Me.Linea += CStr(Me.DbLee.mDbLector.Item("CUENTA")).PadRight(12, " ")
                    ' CUENTA DE CLIENTE
                    'Me.Linea += CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING3")).PadRight(12, " ")
                    ' CUENTA MANO CORRIENTE
                    Me.Linea += Me.mParaCuentaMano.PadRight(12, " ")

                    ' CONCEPTO
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CONCEPTO")).PadRight(30, " ")


                    ' CONSTANTE C= CARGO A = ABONO

                    If Me.DbLee.mDbLector.Item("INDICADOR") = "H" Then
                        ' NOTA DE CREDITO  = ABONO ?
                        Me.Linea += "A"
                    Else
                        ' NOTA DE CREDITO ANULADA = CARGO
                        Me.Linea += "C"

                    End If



                    ' NUMERO DE FACTURA
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("DOCUMENTO")).PadRight(10, " ")

                    ' linea del apunte   "I"  "M"  "U"  
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("ASNT_AUXILIAR_STRING2"))



                    ' CONCEPTO
                    Me.Linea += CStr(Me.DbLee.mDbLector.Item("CONCEPTO")).PadRight(30, " ")


                    ' FACTURAS EXPEDIDAS
                    Me.Linea += "01"



                    ' BASE IMPONIBLE 
                    If Me.DbLee.mDbLector.Item("INDICADOR") = "H" Then
                        ' NOTA DE CREDITO  
                        Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("BASE2")), "F")).PadLeft(13, "0")
                        NotaAnulada = False
                    Else
                        ' NOTA DE CREDITO ANULADA 
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("BASE2")), "F")).PadLeft(13, "0")
                        NotaAnulada = True

                    End If



                    ' PORCENTAJE DE IVA


                    Me.Linea += CStr(Format(CDec(Me.DbLee.mDbLector.Item("TIPOI")), "F")).PadLeft(5, "0")

                    ' CUOTA DE IVA

                    If Me.DbLee.mDbLector.Item("INDICADOR") = "H" Then
                        ' NOTA DE CREDITO  
                        Me.Linea += "-" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    Else
                        ' NOTA DE CREDITO ANULADA 
                        Me.Linea += "+" & CStr(Format(CDec(Me.DbLee.mDbLector.Item("IMPORTE2")), "F")).PadLeft(13, "0")
                    End If


                    ' porcentaje de recargo
                    Me.Linea += "00.00"

                    ' cuota de recargo
                    Me.Linea += "+0000000000.00"

                    ' porcentaje de retencion
                    Me.Linea += "00.00"

                    ' cuota de retencion
                    Me.Linea += "+0000000000.00"


                    ' impreso 01=347 
                    Me.Linea += "01"

                    ' sujeto a iva
                    Me.Linea += "S"

                    ' AFECTRA AL 415
                    Me.Linea += "S"


                    ' RESERVA
                    Me.Linea += "".PadRight(75, " ")

                    ' TIPO DE REGISTRO ANALITOCO  S ????
                    Me.Linea += " "



                    ' MONEDA
                    Me.Linea += "E"

                    ' INDICADOR
                    Me.Linea += "N"



                End If



                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()

                If PROCESA_NOTASCREDITO = 1 Then
                    ' solo se llevan al fichero las notas NO anuladas por ahora
                    If NotaAnulada = False Then
                        Me.Filegraba.WriteLine(Linea)
                    Else
                        '   Me.Filegraba.WriteLine(Linea)
                    End If


                End If

                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()
            Me.ListBoxDebug.Items.Add("------------------------------------------------------")







            ' debug debe/haber
            Me.TextBoxDebe.Text = Me.TotalDebe
            Me.TextBoxHaber.Text = Me.TotalHaber


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Procesa Datisa")
            Exit Sub

        End Try
    End Sub
#End Region

#Region "VITAL SUITES"
    Private Sub ProcesaVital(vTipo As String, vFecha As Date)

        Try


            Dim IndicadorProduccion As String = "285"
            Dim IndicadorFacturacion As String = "286"

            ' contar
            SQL = "SELECT NVL(COUNT(*),'0')  AS TOTAL"
            SQL += "  FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(vFecha, "dd/MM/yyyy") & "'"

            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"

            If vTipo = "AGENCIAS" Then
                SQL += " AND ASNT_CFATOCAB_REFER = 101 "
            Else
                SQL += " AND ASNT_CFATOCAB_REFER <> 101 "
            End If

            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"

            Me.ProgressBar1.Maximum = Me.DbLee.EjecutaSqlScalar(SQL)
            Me.ProgressBar1.Value = 0
            Me.ProgressBar1.Update()



            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, "
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS OBSERVACION, "
            SQL += "ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' ') AS ASNT_FACTURA_SERIE,NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,ASNT_LIN_VLIQ,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2,   ASNT_DPTO_CODI "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(vFecha, "dd/MM/yyyy") & "'"


            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"

            If vTipo = "AGENCIAS" Then
                SQL += " AND ASNT_CFATOCAB_REFER = 101 "
            Else
                SQL += " AND ASNT_CFATOCAB_REFER <> 101 "
            End If
            '    SQL += " ORDER BY ASNT_F_VALOR,ASNT_CFATOCAB_REFER,ASNT_CFCPTOS_COD ASC "
            SQL += " ORDER BY ASNT_F_VALOR,ASNT_CFATOCAB_REFER,ASNT_LINEA ASC "

            Me.DbLee.TraerLector(SQL)

            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read



                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)



                If IsDBNull(Me.DbLee.mDbLector.Item("IMPORTE")) = False Then
                    Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)
                Else
                    Importe = 0
                End If

                If Importe < 0 Then
                    Importe = Importe * -1
                End If



                If IsDBNull(Me.DbLee.mDbLector.Item("ASNT_LIN_VLIQ")) = False Then
                    ImporteLiquido = CType(Me.DbLee.mDbLector.Item("ASNT_LIN_VLIQ"), Double)
                Else
                    ImporteLiquido = 0
                End If

                If ImporteLiquido < 0 Then
                    ImporteLiquido = ImporteLiquido * -1
                End If





                If IsDBNull(Me.DbLee.mDbLector.Item("ASNT_TIPO_IMPUESTO")) = False Then
                    TipoImpuesto = CType(Me.DbLee.mDbLector.Item("ASNT_TIPO_IMPUESTO"), Double)
                Else
                    TipoImpuesto = 0
                End If

                If TipoImpuesto < 0 Then
                    TipoImpuesto = TipoImpuesto * -1
                End If


                If Indicador = "D" Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = "H" Then
                    TotalHaber = TotalHaber + Importe
                End If

                Me.Linea = ""
                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO") & " " & Importe
                Me.TextBoxDebug.Update()

                ' Monta la Linea
                ' CUENTA
                Me.Linea += MID(CType(Me.DbLee.mDbLector.Item("CUENTA"), String), 1, 10).PadRight(10, " ")
                'CONCEPTO
                Me.Linea += Mid(CType(Me.DbLee.mDbLector.Item("CONCEPTO"), String), 1, 20).PadRight(20, " ")
                'DOCUMENTO

                If IsDBNull(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO")) = False Then
                    Me.Linea += Mid(CType(CInt(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO")), String), 1, 7).PadLeft(7, "0")
                Else
                    Me.Linea += "".ToString.PadLeft(7, "0")
                End If

                ' FVALOR
                Me.Linea += Format(Me.DbLee.mDbLector.Item("F_VALOR"), "yyyyMMdd")
                ' DEPARTAMENTO 
                If IsDBNull(Me.DbLee.mDbLector.Item("ASNT_DPTO_CODI")) = False Then
                    Me.Linea += Mid(CType(Me.DbLee.mDbLector.Item("ASNT_DPTO_CODI"), String), 1, 3).PadLeft(3, "0")
                Else
                    Me.Linea += "".ToString.PadLeft(3, "0")
                End If


                ' clave
                Me.Linea += Indicador

                ' importe * 100
                Importe = Math.Round(CDec(Importe * 100), 2, MidpointRounding.AwayFromZero)
                Me.Linea += CType(Importe, String).Replace(",", "").PadLeft(9, "0")




                ' base * 100 

                ImporteLiquido = Math.Round(CDec(ImporteLiquido * 100), 2, MidpointRounding.AwayFromZero)
                Me.Linea += CType(ImporteLiquido, String).Replace(",", "").PadLeft(9, "0")



                ' TIPO IMPUESTO
                If IsDBNull(Me.DbLee.mDbLector.Item("ASNT_TIPO_IMPUESTO")) Then
                    Me.Linea += "".ToString.PadLeft(4, "0")
                Else
                    TipoImpuesto = Math.Round(CDec(Me.DbLee.mDbLector.Item("ASNT_TIPO_IMPUESTO") * 100), 2, MidpointRounding.AwayFromZero)
                    Me.Linea += CType(TipoImpuesto, String).Replace(",", "").PadLeft(4, "0")


                End If

                'CUOITA
                If CStr(Me.DbLee.mDbLector.Item("ASNT_TIPO_IMPUESTO")) = "0" Then
                    Me.Linea += CType("", String).PadLeft(7, " 0")
                Else
                    Me.Linea += CType(Importe, String).Replace(",", "").PadLeft(7, "0")
                End If




                If IsNothing(Me.DbLee.mDbLector.Item("IMPORTE")) = False Then
                    If CDec((Me.DbLee.mDbLector.Item("IMPORTE"))) < 0 Then
                        Me.Linea += "N"
                    Else
                        Me.Linea += " "
                    End If
                End If

                If Me.CheckBoxNifFinal.Checked Then
                    Me.Linea += CType(Mid(Me.DbLee.mDbLector.Item("ASNT_CIF"), 1, 25), String).PadRight(25, " ")
                End If

                ' 
                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()

            ' debug debe/haber
            Me.TextBoxDebe.Text = Me.TotalDebe
            Me.TextBoxHaber.Text = Me.TotalHaber


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Procesa Vital Suite")
            Exit Sub

        End Try
    End Sub
    Private Sub ProcesaVital2(vTipo As String, vFecha As Date)

        Try


            Dim IndicadorProduccion As String = "285"
            Dim IndicadorFacturacion As String = "286"

            ' contar
            SQL = "SELECT NVL(COUNT(*),'0')  AS TOTAL"
            SQL += "  FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(vFecha, "dd/MM/yyyy") & "'"

            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"

            If vTipo = "AGENCIAS" Then
                SQL += " AND ASNT_CFATOCAB_REFER = 101 "
            Else
                SQL += " AND ASNT_CFATOCAB_REFER <> 101 "
            End If

            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"

            Me.ProgressBar1.Maximum = Me.DbLee.EjecutaSqlScalar(SQL)
            Me.ProgressBar1.Value = 0
            Me.ProgressBar1.Update()



            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, "
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS OBSERVACION, "
            SQL += "ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' ') AS ASNT_FACTURA_SERIE,NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,ASNT_LIN_VLIQ,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2,   ASNT_DPTO_CODI "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(vFecha, "dd/MM/yyyy") & "'"


            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"

            If vTipo = "AGENCIAS" Then
                SQL += " AND ASNT_CFATOCAB_REFER = 101 "
            Else
                SQL += " AND ASNT_CFATOCAB_REFER <> 101 "
            End If

            ' Evita tratar lineas de igic 
            SQL += "AND ASNT_TIPO_IMPUESTO = 0"



            SQL += " ORDER BY ASNT_F_VALOR,ASNT_CFATOCAB_REFER,ASNT_LINEA ASC "

            Me.DbLee.TraerLector(SQL)

            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read



                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)



                If IsDBNull(Me.DbLee.mDbLector.Item("IMPORTE")) = False Then
                    Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)
                Else
                    Importe = 0
                End If

                If Importe < 0 Then
                    Importe = Importe * -1
                End If




                If Indicador = "D" Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = "H" Then
                    TotalHaber = TotalHaber + Importe
                End If

                Me.Linea = ""
                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO") & " " & Importe
                Me.TextBoxDebug.Update()




                'si es una factura ir a buscar el registro de impuestos 

                If (CStr(Me.DbLee.mDbLector.Item("ASIENTO")) = 3 Or CStr(Me.DbLee.mDbLector.Item("ASIENTO")) = 51) And IsDBNull(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO")) = False And Me.DbLee.mDbLector.Item("ASNT_TIPO_IMPUESTO") = 0 Then



                    SQL = "SELECT "
                    SQL += "ASNT_I_MONEMP AS IMPORTE,ASNT_LIN_VLIQ,ASNT_TIPO_IMPUESTO "

                    SQL += " FROM TH_ASNT "
                    SQL += " WHERE ASNT_F_VALOR = '" & Format(vFecha, "dd/MM/yyyy") & "'"
                    SQL += " AND ASNT_FACTURA_NUMERO = " & Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO")
                    SQL += " AND ASNT_FACTURA_SERIE = '" & Me.DbLee.mDbLector.Item("ASNT_FACTURA_SERIE") & "'"

                    SQL += " AND ASNT_TIPO_IMPUESTO  > 0"


                    SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                    SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"


                    Me.DbLeeAux.TraerLector(SQL)
                    Me.DbLeeAux.mDbLector.Read()


                    If Me.DbLeeAux.mDbLector.HasRows Then



                        If IsDBNull(Me.DbLeeAux.mDbLector.Item("ASNT_LIN_VLIQ")) = False Then
                            ImporteLiquido = CType(Me.DbLeeAux.mDbLector.Item("ASNT_LIN_VLIQ"), Double)
                        Else
                            ImporteLiquido = 0
                        End If

                        If ImporteLiquido < 0 Then
                            ImporteLiquido = ImporteLiquido * -1
                        End If



                        If IsDBNull(Me.DbLeeAux.mDbLector.Item("ASNT_TIPO_IMPUESTO")) = False Then
                            TipoImpuesto = CType(Me.DbLeeAux.mDbLector.Item("ASNT_TIPO_IMPUESTO"), Double)
                        Else
                            TipoImpuesto = 0
                        End If

                        If TipoImpuesto < 0 Then
                            TipoImpuesto = TipoImpuesto * -1
                        End If


                        If IsDBNull(Me.DbLeeAux.mDbLector.Item("IMPORTE")) = False Then
                            CuotaImpuesto = CType(Me.DbLeeAux.mDbLector.Item("IMPORTE"), Double)
                        Else
                            CuotaImpuesto = 0
                        End If

                        If CuotaImpuesto < 0 Then
                            CuotaImpuesto = CuotaImpuesto * -1
                        End If

                    Else
                        ImporteLiquido = 0
                        TipoImpuesto = 0
                        CuotaImpuesto = 0
                    End If

                    Me.DbLeeAux.mDbLector.Close()

                Else
                    ImporteLiquido = 0
                    TipoImpuesto = 0
                    CuotaImpuesto = 0
                End If
                '---------------------------------------------------








                ' Monta la Linea
                ' CUENTA
                Me.Linea += Mid(CType(Me.DbLee.mDbLector.Item("CUENTA"), String), 1, 10).PadRight(10, " ")
                'CONCEPTO
                Me.Linea += Mid(CType(Me.DbLee.mDbLector.Item("CONCEPTO"), String), 1, 20).PadRight(20, " ")
                'DOCUMENTO

                If IsDBNull(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO")) = False Then
                    Me.Linea += Mid(CType(CInt(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO")), String), 1, 7).PadLeft(7, "0")
                Else
                    Me.Linea += "".ToString.PadLeft(7, "0")
                End If

                ' FVALOR
                Me.Linea += Format(Me.DbLee.mDbLector.Item("F_VALOR"), "yyyyMMdd")

                ' DEPARTAMENTO 
                If IsDBNull(Me.DbLee.mDbLector.Item("ASNT_DPTO_CODI")) = False Then
                    Me.Linea += Mid(CType(Me.DbLee.mDbLector.Item("ASNT_DPTO_CODI"), String), 1, 3).PadLeft(3, "0")
                Else
                    Me.Linea += "".ToString.PadLeft(3, "0")
                End If


                ' clave
                Me.Linea += Indicador

                ' importe * 100
                Importe = Math.Round(CDec(Importe * 100), 2, MidpointRounding.AwayFromZero)
                Me.Linea += CType(Importe, String).Replace(",", "").PadLeft(9, "0")

                ' SIGNO

                If IsNothing(Me.DbLee.mDbLector.Item("IMPORTE")) = False Then
                    If CDec((Me.DbLee.mDbLector.Item("IMPORTE"))) < 0 Then
                        Me.Linea += "N"
                    Else
                        Me.Linea += " "
                    End If
                End If

                ' LOOP 

                ' base * 100 

                If ImporteLiquido <> 0 Then
                    ImporteLiquido = Math.Round(CDec(ImporteLiquido * 100), 2, MidpointRounding.AwayFromZero)
                    Me.Linea += CType(ImporteLiquido, String).Replace(",", "").PadLeft(9, "0")
                Else
                    Me.Linea += CType("", String).PadLeft(9, " 0")
                End If


                If TipoImpuesto <> 0 Then
                    TipoImpuesto = Math.Round(CDec(TipoImpuesto * 100), 2, MidpointRounding.AwayFromZero)
                    Me.Linea += CType(TipoImpuesto, String).Replace(",", "").PadLeft(4, "0")
                Else
                    Me.Linea += "".ToString.PadLeft(4, "0")
                End If

                If CuotaImpuesto <> 0 Then
                    CuotaImpuesto = Math.Round(CDec(CuotaImpuesto * 100), 2, MidpointRounding.AwayFromZero)
                    Me.Linea += CType(CuotaImpuesto, String).Replace(",", "").PadLeft(7, "0")
                Else
                    Me.Linea += CType("", String).PadLeft(7, " 0")
                End If








                If Me.CheckBoxNifFinal.Checked Then
                    Me.Linea += CType(Mid(Me.DbLee.mDbLector.Item("ASNT_CIF"), 1, 25), String).PadRight(25, " ")
                End If

                ' 
                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()

            ' debug debe/haber
            Me.TextBoxDebe.Text = Me.TotalDebe
            Me.TextBoxHaber.Text = Me.TotalHaber


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Procesa Vital Suite")
            Exit Sub

        End Try
    End Sub
    Private Sub ProcesaVital3(vTipo As String, vFecha As Date)

        Try


            Dim IndicadorProduccion As String = "285"
            Dim IndicadorFacturacion As String = "286"

            ' contar
            SQL = "SELECT NVL(COUNT(*),'0')  AS TOTAL"
            SQL += "  FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(vFecha, "dd/MM/yyyy") & "'"

            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"

            If vTipo = "AGENCIAS" Then
                SQL += " AND ASNT_CFATOCAB_REFER = 101 "
            Else
                SQL += " AND ASNT_CFATOCAB_REFER <> 101 "
            End If

            SQL += " ORDER BY ASNT_CFATOCAB_REFER,ASNT_LINEA"

            Me.ProgressBar1.Maximum = Me.DbLee.EjecutaSqlScalar(SQL)
            Me.ProgressBar1.Value = 0
            Me.ProgressBar1.Update()



            SQL = "SELECT ASNT_F_VALOR AS F_VALOR, ASNT_CFCTA_COD AS CUENTA,ASNT_CFATOCAB_REFER AS ASIENTO,ASNT_LINEA AS LINEA,"
            SQL += "ASNT_CFCPTOS_COD AS INDICADOR,ASNT_I_MONEMP AS IMPORTE, "
            SQL += " ASNT_AMPCPTO AS CONCEPTO, round(ASNT_DEBE,2) AS DEBE, round(ASNT_HABER,2) AS HABER,NVL(ASNT_NOMBRE,' ') AS OBSERVACION, "
            SQL += "ASNT_FACTURA_NUMERO,NVL(ASNT_FACTURA_SERIE,' ') AS ASNT_FACTURA_SERIE,NVL(ASNT_AUXILIAR_STRING,' ') AS ASNT_AUXILIAR_STRING,NVL(ASNT_AUXILIAR_NUMERICO,'0') AS ASNT_AUXILIAR_NUMERICO,ASNT_TIPO_IMPUESTO,ASNT_LIN_VLIQ,NVL(ASNT_AUXILIAR_STRING2,' ') AS ASNT_AUXILIAR_STRING2,   ASNT_DPTO_CODI "
            SQL += " FROM TH_ASNT "
            SQL += " WHERE ASNT_F_VALOR = '" & Format(vFecha, "dd/MM/yyyy") & "'"


            SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
            SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"

            If vTipo = "AGENCIAS" Then
                SQL += " AND ASNT_CFATOCAB_REFER = 101 "
            Else
                SQL += " AND ASNT_CFATOCAB_REFER <> 101 "
            End If

            ' Evita tratar lineas de igic 
            SQL += "AND ASNT_TIPO_IMPUESTO = 0"



            SQL += " ORDER BY ASNT_F_VALOR,ASNT_CFATOCAB_REFER,ASNT_LINEA ASC "

            Me.DbLee.TraerLector(SQL)

            Me.PrimerRegistro = True
            While Me.DbLee.mDbLector.Read



                Indicador = CType(Me.DbLee.mDbLector.Item("INDICADOR"), String)



                If IsDBNull(Me.DbLee.mDbLector.Item("IMPORTE")) = False Then
                    Importe = CType(Me.DbLee.mDbLector.Item("IMPORTE"), Double)
                Else
                    Importe = 0
                End If

                If Importe < 0 Then
                    Importe = Importe * -1
                End If


                If Indicador = "D" Then
                    TotalDebe = TotalDebe + Importe
                End If

                If Indicador = "H" Then
                    TotalHaber = TotalHaber + Importe
                End If




                Me.TextBoxDebug.Text = Me.DbLee.mDbLector.Item("F_VALOR") & " " & Me.DbLee.mDbLector.Item("CUENTA") & " " & Me.DbLee.mDbLector.Item("CONCEPTO") & " " & Importe
                Me.TextBoxDebug.Update()


                ' Monta la Linea

                Me.Linea = ""
                ' CUENTA
                Me.Linea += Mid(CType(Me.DbLee.mDbLector.Item("CUENTA"), String), 1, 10).PadRight(10, " ")
                'CONCEPTO
                Me.Linea += Mid(CType(Me.DbLee.mDbLector.Item("CONCEPTO"), String), 1, 20).PadRight(20, " ")
                'DOCUMENTO

                If IsDBNull(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO")) = False Then
                    Me.Linea += Mid(CType(CInt(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO")), String), 1, 7).PadLeft(7, "0")
                Else
                    Me.Linea += "".ToString.PadLeft(7, "0")
                End If

                ' FVALOR
                Me.Linea += Format(Me.DbLee.mDbLector.Item("F_VALOR"), "yyyyMMdd")

                ' DEPARTAMENTO 
                If IsDBNull(Me.DbLee.mDbLector.Item("ASNT_DPTO_CODI")) = False Then
                    '       Me.Linea += Mid(CType(Me.DbLee.mDbLector.Item("ASNT_DPTO_CODI"), String), 1, 3).PadLeft(3, "0")
                Else
                    '      Me.Linea += "".ToString.PadLeft(3, "0")
                End If

                ' DEPARTAMENTO FIJO = A 
                Me.Linea += "A".ToString.PadLeft(3, "0")


                ' clave
                Me.Linea += Indicador

                ' importe * 100
                Importe = Math.Round(CDec(Importe * 100), 2, MidpointRounding.AwayFromZero)
                Me.Linea += CType(Importe, String).Replace(",", "").PadLeft(9, "0")

                ' SIGNO

                If IsNothing(Me.DbLee.mDbLector.Item("IMPORTE")) = False Then
                    If CDec((Me.DbLee.mDbLector.Item("IMPORTE"))) < 0 Then
                        Me.Linea += "N"
                    Else
                        Me.Linea += " "
                    End If
                End If





                'si es una factura ir a buscar el registro de impuestos Ç
                Me.m_ContadorLineasIgic = 0

                If (CStr(Me.DbLee.mDbLector.Item("ASIENTO")) = 3 Or CStr(Me.DbLee.mDbLector.Item("ASIENTO")) = 51 Or CStr(Me.DbLee.mDbLector.Item("ASIENTO")) = 101) And IsDBNull(Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO")) = False And Me.DbLee.mDbLector.Item("ASNT_TIPO_IMPUESTO") = 0 Then



                    SQL = "SELECT "
                    SQL += "ASNT_I_MONEMP AS IMPORTE,ASNT_LIN_VLIQ,ASNT_TIPO_IMPUESTO "

                    SQL += " FROM TH_ASNT "
                    SQL += " WHERE ASNT_F_VALOR = '" & Format(vFecha, "dd/MM/yyyy") & "'"
                    SQL += " AND ASNT_FACTURA_NUMERO = " & Me.DbLee.mDbLector.Item("ASNT_FACTURA_NUMERO")
                    SQL += " AND ASNT_FACTURA_SERIE = '" & Me.DbLee.mDbLector.Item("ASNT_FACTURA_SERIE") & "'"

                    SQL += " AND ASNT_TIPO_IMPUESTO  > 0"


                    SQL += " AND TH_ASNT.ASNT_EMPGRUPO_COD = '" & Me.mEmpGrupoCod & "'"
                    SQL += " AND TH_ASNT.ASNT_EMP_COD = '" & Me.mEmpCod & "'"
                    SQL += "  ORDER BY ASNT_TIPO_IMPUESTO ASC "

                    Me.DbLeeAux.TraerLector(SQL)



                    While Me.DbLeeAux.mDbLector.Read()

                        Me.m_ContadorLineasIgic = Me.m_ContadorLineasIgic + 1


                        ' base

                        If IsDBNull(Me.DbLeeAux.mDbLector.Item("ASNT_LIN_VLIQ")) = False Then
                            ImporteLiquido = CType(Me.DbLeeAux.mDbLector.Item("ASNT_LIN_VLIQ"), Double)
                        Else
                            ImporteLiquido = 0
                        End If

                        If ImporteLiquido < 0 Then
                            ImporteLiquido = ImporteLiquido * -1
                        End If


                        If ImporteLiquido <> 0 Then
                            ImporteLiquido = Math.Round(CDec(ImporteLiquido * 100), 2, MidpointRounding.AwayFromZero)
                            Me.Linea += CType(ImporteLiquido, String).Replace(",", "").PadLeft(9, "0")
                        Else
                            Me.Linea += CType("", String).PadLeft(9, "0")
                        End If

                        ' tipo 


                        If IsDBNull(Me.DbLeeAux.mDbLector.Item("ASNT_TIPO_IMPUESTO")) = False Then
                            TipoImpuesto = CType(Me.DbLeeAux.mDbLector.Item("ASNT_TIPO_IMPUESTO"), Double)
                        Else
                            TipoImpuesto = 0
                        End If

                        If TipoImpuesto < 0 Then
                            TipoImpuesto = TipoImpuesto * -1
                        End If


                        If TipoImpuesto <> 0 Then
                            TipoImpuesto = Math.Round(CDec(TipoImpuesto * 100), 2, MidpointRounding.AwayFromZero)
                            Me.Linea += CType(TipoImpuesto, String).Replace(",", "").PadLeft(4, "0")
                        Else
                            Me.Linea += "".ToString.PadLeft(4, "0")
                        End If

                        ' cuota

                        If IsDBNull(Me.DbLeeAux.mDbLector.Item("IMPORTE")) = False Then
                            CuotaImpuesto = CType(Me.DbLeeAux.mDbLector.Item("IMPORTE"), Double)
                        Else
                            CuotaImpuesto = 0
                        End If

                        If CuotaImpuesto < 0 Then
                            CuotaImpuesto = CuotaImpuesto * -1
                        End If

                        If CuotaImpuesto <> 0 Then
                            CuotaImpuesto = Math.Round(CDec(CuotaImpuesto * 100), 2, MidpointRounding.AwayFromZero)
                            Me.Linea += CType(CuotaImpuesto, String).Replace(",", "").PadLeft(7, "0")
                        Else
                            Me.Linea += CType("", String).PadLeft(7, "0")
                        End If

                        '---------------------------------------------------


                    End While
                    Me.DbLeeAux.mDbLector.Close()

                End If


                If Me.m_ContadorLineasIgic < 4 Then

                    While Me.m_ContadorLineasIgic < 4
                        Me.m_ContadorLineasIgic = Me.m_ContadorLineasIgic + 1
                        Me.Linea += CType("", String).PadLeft(9, "0")
                        Me.Linea += "".ToString.PadLeft(4, "0")
                        Me.Linea += CType("", String).PadLeft(7, "0")
                    End While

                End If




                If Me.CheckBoxNifFinal.Checked Then
                    Me.Linea += CType(Mid(Me.DbLee.mDbLector.Item("ASNT_CIF"), 1, 25), String).PadRight(25, " ")
                End If

                ' 
                ' graba la linea 
                Me.ListBoxDebug.Items.Add(Linea)
                Me.ListBoxDebug.Update()
                Me.Filegraba.WriteLine(Linea)
                Me.mApunte = Me.mApunte + 1
            End While
            Me.DbLee.mDbLector.Close()

            ' debug debe/haber
            Me.TextBoxDebe.Text = Me.TotalDebe
            Me.TextBoxHaber.Text = Me.TotalHaber


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Procesa Vital Suite")
            Exit Sub

        End Try
    End Sub
#End Region
#Region "RUTINAS COMUNES"
    Private Sub CrearFichero(ByVal vFile As String)

        Try
            FileEstaOk = False

            If Me.mvFileType = "DOS" Then
                Filegraba = New StreamWriter(vFile, False, System.Text.Encoding.ASCII)

            Else
                Filegraba = New StreamWriter(vFile, False, System.Text.Encoding.UTF8)
            End If

            FileEstaOk = True
        Catch ex As Exception
            FileEstaOk = False
            MsgBox("No dispone de acceso al Fichero " & vFile, MsgBoxStyle.Information, "Atención")
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Function ContarRegistros(ByVal vSql As String) As Integer
        Try

            Dim AuxSql() As String
            Dim SQL As String

            AuxSql = Split(vSql, "WHERE")
            SQL = "SELECT COUNT(*)  FROM TH_ASNT  WHERE " & AuxSql(1)

            Me.m_TotalRegistros = CInt(Me.DbLeeAux.EjecutaSqlScalar(SQL))

        Catch ex As Exception
            MsgBox(ex.Message)
            Return 0

        End Try
    End Function
#End Region

    Private Sub FormConvertir_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            If mEmpGrupoCod = "TAHO" Then
                Me.ComboBoxFormato.SelectedIndex = 1
                Me.ComboBoxFormato.Enabled = False
                Me.ComboBoxFormato.Update()
            End If
            If mEmpGrupoCod = "SATO" Then
                Me.ComboBoxFormato.SelectedIndex = 3
                Me.ComboBoxFormato.Enabled = False
                Me.ComboBoxFormato.Update()
            End If
            If mEmpGrupoCod = "LOPE" Then
                Me.ComboBoxFormato.SelectedIndex = 4
                Me.ComboBoxFormato.Enabled = False
                Me.ComboBoxFormato.Update()
                Me.CheckBoxNifFinal.Enabled = True
            End If

            If mEmpGrupoCod = "EREZ" Then
                Me.ComboBoxFormato.SelectedIndex = 5
                Me.ComboBoxFormato.Enabled = False
                Me.ComboBoxFormato.Update()
            End If

            If mEmpGrupoCod = "HC7" Then
                Me.ComboBoxFormato.SelectedIndex = 6
                Me.ComboBoxFormato.Enabled = False
                Me.ComboBoxFormato.Update()
            End If

            If mEmpGrupoCod = "PTRO" Then
                Me.ComboBoxFormato.SelectedIndex = 7
                Me.ComboBoxFormato.Enabled = False
                Me.ComboBoxFormato.Update()
            End If

            If mEmpGrupoCod = "MONI" Then
                Me.ComboBoxFormato.SelectedIndex = 9
                Me.ComboBoxFormato.Enabled = False
                Me.ComboBoxFormato.Update()
            End If


            If mEmpGrupoCod = "GRVI" Then
                Me.ComboBoxFormato.SelectedIndex = 10
                Me.ComboBoxFormato.Enabled = False
                Me.ComboBoxFormato.Update()
            End If

            'procesa sin detenerse
            If Me.mNoWait = True Then
                Me.Procesar()

                If File.Exists(Me.mvFile) = True Then
                    MsgBox("Fichero Generado " & Me.mvFile, MsgBoxStyle.Information, "Atención")
                Else
                    MsgBox("Fichero NO Generado " & Me.mvFile, MsgBoxStyle.Critical, "Atención")
                End If

                Me.Close()
            End If






        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Sub

    Private Sub FormConvertir_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Try
            If Me.DbLee.EstadoConexion = ConnectionState.Open Then
                Me.DbLee.CerrarConexion()
            End If
            If Me.DbLeeAux.EstadoConexion = ConnectionState.Open Then
                Me.DbLeeAux.CerrarConexion()
            End If

        Catch ex As Exception

        End Try
    End Sub

End Class
