Imports System.IO
Public Class FormIntegraNominaA3
    Dim MyIni As New cIniArray

    Private DbCentral As C_DATOS.C_DatosOledb
    Private DbWriteCentral As C_DATOS.C_DatosOledb
    Private DbSpyro As C_DATOS.C_DatosOledb

    Private m_StrConexionCentral As String
    Private m_StrConexionSpyro As String


    Private m_EmpGrupoCod As String
    Private m_EmpCod As String
    Private m_EmpNum As Integer

    Private m_ParaFileOrigen As String
    Private m_ParaFileDestino As String

    Private m_Cfatodiari_Cod As String
    Private m_CfatoTip_Cod As String
    Private m_CtaAjusteRedondeo As String
    Private Linea As Integer

    Dim HayRegistros As Boolean = False

    Private SQL As String


    Private m_TipoAsiento As String
    Private m_Debe As Double
    Private m_Haber As Double

    Private m_IndicadorDebe As String
    Private m_IndicadorHaber As String
    Private m_IndicadorAsiento As String
    Private m_ConceptoAsiento As String

    Private m_ValidaSpyro As Boolean = False

    Private m_Texto As String

    Private m_Fecha As Date
    Private Filegraba As StreamWriter
    Private FilegrabaStatus As Boolean = False

    Private FileName As String

    Dim TotalBruto As Double
    Dim TotalIndem As Double
    Dim TotalSsEmpre As Double
    Dim TotalNeto As Double
    Dim TotalIrpf As Double
    Dim TotalSstotal As Double
    Dim Total755Perso As Double
    Dim TotalEmbargoCuota As Double
    Dim TotalAntiPrest As Double
    Dim TotalOtrosDtos As Double


    Dim CtaTotalBruto As String
    Dim CtaTotalIndem As String
    Dim CtaTotalSsEmpre As String
    Dim CtaTotalNeto As String
    Dim CtaTotalIrpf As String
    Dim CtaTotalSstotal As String
    Dim CtaTotal755Perso As String
    Dim CtaTotalEmbargoCuota As String
    Dim CtaTotalAntiPrest As String
    Dim CtaTotalOtrosDtos As String
    Dim CtaProductor As String
    Dim CtaBanco As String


    Dim m_Total As Double
    Dim m_Nombre As String
    Dim m_Descripcion As String

    Dim m_TotalDebe As Double
    Dim m_TotalHaber As Double
    Dim m_TotalDiferencia As Double

    Dim m_HayErrores As Boolean


    Private Result As String

    Private RutaExcelNifs As String

    ' Dim mTexto As String
    ' OTROS 
    Private iASCII(63) As Integer       'Para conversión a MS-DOS
    Private mParaToOem As Boolean = False

    ' excel
    Dim WithEvents ExcelApp As Microsoft.Office.Interop.Excel.Application
    Dim ExcelBook As Microsoft.Office.Interop.Excel.Workbook
    Dim ExcelSheet As Microsoft.Office.Interop.Excel.Worksheet
    Dim Rango As Microsoft.Office.Interop.Excel.Range
    Private m_File As FileInfo


#Region "RUTINAS"
    Private Sub LeeExcelNomina(ByVal vFileName As String)
        Try
            Dim DS As System.Data.DataSet
            Dim DT As System.Data.DataTable
            Dim DR As System.Data.DataRow


            Dim MyCommand As System.Data.OleDb.OleDbDataAdapter
            Dim MyConnection As System.Data.OleDb.OleDbConnection
            Dim StrConexion As String

            Dim Ind As Integer = 1
            '  Dim IndProductor As Integer
            '   Dim StrProductor As String

            Dim TotalProductores As Integer
            Dim ControlTotalBruto As Decimal
            Dim Nombre As String



            Me.Linea = 1



            StrConexion = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & vFileName & ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"""

            MyConnection = New System.Data.OleDb.OleDbConnection(StrConexion)
            DS = New System.Data.DataSet()
            MyCommand = New System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" & Me.TextBoxHojaExcel.Text & "$]", MyConnection)
            '    "select * from [MAYO 2010$]", MyConnection)

            MyCommand.Fill(DS)
            DT = DS.Tables(0)

            DT.Locale.NumberFormat.NumberDecimalSeparator = "."
            DT.Locale.NumberFormat.NumberGroupSeparator = ","




            For Each DR In DT.Rows
                Me.ListBox1.Items.Add(Ind.ToString.PadRight(10, " ") & "|" & DR(0).ToString.PadRight(50, " ") & "|" & DR(1).ToString.PadRight(50, " ") & "|" & DR(2).ToString.PadRight(50, " "))
                ' 

                If IsNumeric(DR(1).ToString) = True Then
                    '    Me.TextBoxDebug.Text += "Línea " & Ind.ToString.PadRight(10, " ") & " Excel Si se puede Evaluar como Código de Productor" & vbCrLf
                    TotalProductores = TotalProductores + 1
                    If IsNumeric(EsUnNUmero(DR(3).ToString)) = True = True Then
                        ControlTotalBruto = ControlTotalBruto + ConvierteNumero(DR(3).ToString)
                    End If


                    ' Nombre del Productor

                    If DR(2).ToString.Length > 0 Then
                        Nombre = DR(2).ToString.Replace("'", "´")
                    Else
                        Nombre = "?"
                    End If

                    ' Importes 
                    If IsNumeric(EsUnNUmero(DR(3).ToString)) = True Then
                        Me.TotalBruto = ConvierteNumero(DR(3).ToString)
                    Else
                        Me.TotalBruto = CType("0", Double)

                    End If


                    If IsNumeric(EsUnNUmero(DR(4).ToString)) = True Then
                        Me.TotalIndem = ConvierteNumero(DR(4).ToString)
                    Else
                        Me.TotalIndem = CType("0", Double)
                    End If



                    If IsNumeric(EsUnNUmero(DR(5).ToString)) = True Then
                        Me.TotalSsEmpre = ConvierteNumero(DR(5).ToString)
                    Else
                        Me.TotalSsEmpre = CType("0", Double)
                    End If

                    If IsNumeric(EsUnNUmero(DR(6).ToString)) = True Then
                        Me.TotalNeto = ConvierteNumero(DR(6).ToString)
                    Else
                        Me.TotalNeto = CType("0", Double)
                    End If

                    If IsNumeric(EsUnNUmero(DR(7).ToString)) = True Then
                        Me.TotalIrpf = ConvierteNumero(DR(7).ToString)
                    Else
                        Me.TotalIrpf = CType("0", Double)
                    End If

                    If IsNumeric(EsUnNUmero(DR(8).ToString)) = True Then
                        Me.TotalSstotal = ConvierteNumero(DR(8).ToString)
                    Else
                        Me.TotalSstotal = CType("0", Double)
                    End If


                    If IsNumeric(EsUnNUmero(DR(9).ToString)) = True Then
                        Me.Total755Perso = ConvierteNumero(DR(9).ToString)
                    Else
                        Me.Total755Perso = CType("0", Double)
                    End If

                    If IsNumeric(EsUnNUmero(DR(10).ToString)) = True Then
                        Me.TotalEmbargoCuota = ConvierteNumero(DR(10).ToString)
                    Else
                        Me.TotalEmbargoCuota = CType("0", Double)
                    End If

                    If IsNumeric(EsUnNUmero(DR(11).ToString)) = True Then
                        Me.TotalAntiPrest = ConvierteNumero(DR(11).ToString)
                    Else
                        Me.TotalAntiPrest = CType("0", Double)
                    End If


                    If IsNumeric(EsUnNUmero(DR(12).ToString)) = True Then
                        Me.TotalOtrosDtos = ConvierteNumero(DR(12).ToString)
                    Else
                        Me.TotalOtrosDtos = CType("0", Double)
                    End If



                    Me.InsertaOracle("EXCEL", 1, Me.m_EmpGrupoCod, Me.m_EmpCod, CType(Now.Year, String), 1, Linea, "CUENTA", "?", Me.m_ConceptoAsiento, 0, "NO", "CIF", Nombre, "SI", Format(Me.m_Fecha, "dd/MM/yyyy"), "", DR(1).ToString, Me.TotalBruto, Me.TotalIndem, Me.TotalSsEmpre, Me.TotalNeto, Me.TotalIrpf, Me.TotalSstotal, Me.Total755Perso, Me.TotalEmbargoCuota, Me.TotalAntiPrest, Me.TotalOtrosDtos, 0)
                    Me.Linea = Me.Linea + 1

                Else
                    Me.TextBoxDebug.Text += "Línea " & Ind.ToString.PadRight(10, " ") & " Excel No se puede Evaluar como Código de Productor" & vbCrLf
                End If

                Ind = Ind + 1
            Next
            DS.Dispose()

            Me.TextBoxTotalProductores.Text = TotalProductores
            Me.TextBoxTotalBruto.Text = ControlTotalBruto
            Me.Update()




            MyConnection.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub LeeExcelNominaRemesa(ByVal vFileName As String)
        Try
            Dim DS As System.Data.DataSet
            Dim DT As System.Data.DataTable
            Dim DR As System.Data.DataRow


            Dim MyCommand As System.Data.OleDb.OleDbDataAdapter
            Dim MyConnection As System.Data.OleDb.OleDbConnection
            Dim StrConexion As String

            Dim Ind As Integer = 1
            '  Dim IndProductor As Integer
            '   Dim StrProductor As String

            Dim TotalProductores As Integer
            Dim ControlTotalBruto As Decimal
            Dim Nombre As String
            Dim Nif As String



            Me.Linea = 1



            StrConexion = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & vFileName & ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"""

            MyConnection = New System.Data.OleDb.OleDbConnection(StrConexion)
            DS = New System.Data.DataSet()
            MyCommand = New System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" & Me.TextBoxHojaExcel.Text & "$]", MyConnection)
            '    "select * from [MAYO 2010$]", MyConnection)

            MyCommand.Fill(DS)
            DT = DS.Tables(0)

            DT.Locale.NumberFormat.NumberDecimalSeparator = "."
            DT.Locale.NumberFormat.NumberGroupSeparator = ","




            For Each DR In DT.Rows
                Me.ListBox1.Items.Add(Ind.ToString.PadRight(10, " ") & "|" & DR(0).ToString.PadRight(50, " ") & "|" & DR(1).ToString.PadRight(50, " ") & "|" & DR(2).ToString.PadRight(50, " "))
                ' 

                ' Si hay importe y cuenta 
                'If IsNumeric(DR(6).ToString) = True And DR(5).ToString.Length > 0 Then
                If EsUnNUmero(DR(6).ToString) And DR(5).ToString.Length > 0 Then
                    '    Me.TextBoxDebug.Text += "Línea " & Ind.ToString.PadRight(10, " ") & " Excel Si se puede Evaluar como Código de Productor" & vbCrLf
                    TotalProductores = TotalProductores + 1
                    If IsNumeric(EsUnNUmero(DR(6).ToString)) = True = True Then
                        ControlTotalBruto = ControlTotalBruto + ConvierteNumero(DR(6).ToString)
                    End If


                    ' Nombre del Productor

                    If DR(0).ToString.Length > 0 Then
                        Nombre = DR(0).ToString.Replace("'", "´")
                    Else
                        Nombre = "?"
                    End If

                    ' Importes 
                    If IsNumeric(EsUnNUmero(DR(6).ToString)) = True Then
                        Me.TotalBruto = ConvierteNumero(DR(6).ToString)
                    Else
                        Me.TotalBruto = CType("0", Double)

                    End If

                    ' nif
                    If DR(1).ToString.Length > 0 Then
                        Nif = DR(1).ToString
                    Else
                        Nif = "?"
                    End If

                    Me.TotalIndem = CType("0", Double)
                    Me.TotalSsEmpre = CType("0", Double)
                    Me.TotalNeto = CType("0", Double)
                    Me.TotalIrpf = CType("0", Double)
                    Me.TotalSstotal = CType("0", Double)
                    Me.Total755Perso = CType("0", Double)
                    Me.TotalEmbargoCuota = CType("0", Double)
                    Me.TotalAntiPrest = CType("0", Double)
                    Me.TotalOtrosDtos = CType("0", Double)




                    Me.InsertaOracle("EXCEL", 1, Me.m_EmpGrupoCod, Me.m_EmpCod, CType(Now.Year, String), 1, Linea, "CUENTA", "?", Me.m_ConceptoAsiento, 0, "NO", Nif, Nombre, "SI", Format(Me.m_Fecha, "dd/MM/yyyy"), "", DR(1).ToString, Me.TotalBruto, Me.TotalIndem, Me.TotalSsEmpre, Me.TotalNeto, Me.TotalIrpf, Me.TotalSstotal, Me.Total755Perso, Me.TotalEmbargoCuota, Me.TotalAntiPrest, Me.TotalOtrosDtos, 0)
                    Me.Linea = Me.Linea + 1

                Else
                    Me.TextBoxDebug.Text += "Línea " & Ind.ToString.PadRight(10, " ") & " Excel No se puede Evaluar como Código de Productor" & vbCrLf

                End If

                Ind = Ind + 1
            Next
            DS.Dispose()

            Me.TextBoxTotalProductores.Text = TotalProductores
            Me.TextBoxTotalBruto.Text = ControlTotalBruto
            Me.Update()




            MyConnection.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub LeeExcelNominaNIFS(ByVal vFileName As String)
        Try
            Dim DS As System.Data.DataSet
            Dim DT As System.Data.DataTable
            Dim DR As System.Data.DataRow


            Dim MyCommand As System.Data.OleDb.OleDbDataAdapter
            Dim MyConnection As System.Data.OleDb.OleDbConnection
            Dim StrConexion As String

            Dim Ind As Integer = 1
            '  Dim IndProductor As Integer
            '   Dim StrProductor As String

            Dim TotalProductores As Integer
            Dim CodigoProductor As String
            Dim NifProductor As String


            Me.Linea = 1


            ' BORRA NIFS 
            Try

                ' INSERT
                SQL = "DELETE TN_NIFS "

                Me.DbWriteCentral.EjecutaSqlCommit(SQL)

            Catch ex As Exception

            End Try



            StrConexion = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & vFileName & ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"""

            MyConnection = New System.Data.OleDb.OleDbConnection(StrConexion)
            DS = New System.Data.DataSet()
            ' MyCommand = New System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" & Me.ContarHojasExcelDevuelveHoja & "$]", MyConnection)
            MyCommand = New System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" & Me.TextBoxHojaNif.Text & "$]", MyConnection)
            '    "select * from [MAYO 2010$]", MyConnection)

            MyCommand.Fill(DS)
            DT = DS.Tables(0)

            DT.Locale.NumberFormat.NumberDecimalSeparator = "."
            DT.Locale.NumberFormat.NumberGroupSeparator = ","




            For Each DR In DT.Rows
                Me.ListBox1.Items.Add(Ind.ToString.PadRight(10, " ") & "|" & DR(0).ToString.PadRight(50, " ") & "|" & DR(1).ToString.PadRight(50, " ") & "|" & DR(2).ToString.PadRight(50, " "))
                ' 

                ' Si HAY CODIGO DE PRODUCTOR 
                If IsNumeric(DR(0).ToString) = True Then
                    '    Me.TextBoxDebug.Text += "Línea " & Ind.ToString.PadRight(10, " ") & " Excel Si se puede Evaluar como Código de Productor" & vbCrLf
                    TotalProductores = TotalProductores + 1

                    ' codigo productor

                    If DR(0).ToString.Length > 0 Then
                        CodigoProductor = DR(0).ToString
                    Else
                        CodigoProductor = ""
                    End If

                    ' nif productor

                    If DR(1).ToString.Length > 0 Then
                        NifProductor = DR(1).ToString
                    Else
                        NifProductor = ""
                    End If




                    ' INSERT
                    SQL = "INSERT INTO TN_NIFS (NIFS_KEY, NIFS_PROD, NIFS_NIF) VALUES(  "
                    SQL += "'KEY','"
                    SQL += CodigoProductor & "','"
                    SQL += NifProductor & "')"

                    Me.DbWriteCentral.EjecutaSqlCommit(SQL)



                Else
                    Me.TextBoxDebug.Text += "Línea " & Ind.ToString.PadRight(10, " ") & " Excel No se puede Evaluar como Código de Productor" & vbCrLf

                End If

                Ind = Ind + 1
            Next
            DS.Dispose()


            MyConnection.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Function EsUnNUmero(ByVal valor As String) As Boolean
        Try
            Dim style As Globalization.NumberStyles
            Dim culture As Globalization.CultureInfo
            Dim number As Double


            style = Globalization.NumberStyles.AllowDecimalPoint Or Globalization.NumberStyles.AllowThousands _
            Or Globalization.NumberStyles.AllowLeadingSign Or Globalization.NumberStyles.AllowTrailingSign
            culture = Globalization.CultureInfo.CreateSpecificCulture("es-ES")
            If Double.TryParse(valor, style, culture, number) Then
                '  Console.WriteLine("Converted '{0}' to {1}.", valor, number)
                Return True
            Else
                'Console.WriteLine("Unable to convert '{0}'.", valor)
                Return False
            End If
        Catch ex As Exception

        End Try

    End Function
    Private Function ConvierteNumero(ByVal valor As String) As Double
        Try
            Dim style As Globalization.NumberStyles
            Dim culture As Globalization.CultureInfo
            Dim number As Double


            style = Globalization.NumberStyles.AllowDecimalPoint Or Globalization.NumberStyles.AllowThousands _
              Or Globalization.NumberStyles.AllowLeadingSign Or Globalization.NumberStyles.AllowTrailingSign
            culture = Globalization.CultureInfo.CreateSpecificCulture("es-ES")
            If Double.TryParse(valor, style, culture, number) Then
                '  Console.WriteLine("Converted '{0}' to {1}.", valor, number)
                Return number
            Else
                'Console.WriteLine("Unable to convert '{0}'.", valor)
                Return 0
            End If
        Catch ex As Exception

        End Try

    End Function
    Private Sub GeneraAsientoNomina()
        Try

            ' BRUTO
            SQL = "SELECT *  FROM TN_ASNT WHERE ASNT_F_ATOCAB = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            SQL += " AND ASNT_BRUTO <> 0 "
            SQL += " AND ASNT_TIPO_REGISTRO = 'EXCEL'"
            SQL += " ORDER BY ASNT_PRODUC ASC"

            Me.DbCentral.TraerLector(SQL)

            While Me.DbCentral.mDbLector.Read
                Me.m_Total = CDbl(Me.DbCentral.mDbLector.Item("ASNT_BRUTO"))

                If IsDBNull(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")) = False Then
                    Me.m_Nombre = CStr(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")).Replace("'", "´")
                    Me.m_Descripcion = "Bruto " & CStr(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")).Replace("'", "´")
                End If


                Me.InsertaOracle("AC", 1, Me.m_EmpGrupoCod, Me.m_EmpCod, CType(Now.Year, String), 1, Linea, Me.CtaTotalBruto & CStr(Me.DbCentral.mDbLector.Item("ASNT_PRODUC")), Me.m_IndicadorDebe, Me.m_Descripcion, Me.m_Total, "NO", "CIF", Me.m_Nombre, "SI", Format(Me.m_Fecha, "dd/MM/yyyy"), "BRUTO", CStr(Me.DbCentral.mDbLector.Item("ASNT_PRODUC")), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)
                Me.Linea = Me.Linea + 1

            End While
            Me.DbCentral.mDbLector.Close()

            ' INDEMNIZACION
            SQL = "SELECT *  FROM TN_ASNT WHERE ASNT_F_ATOCAB = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            SQL += " AND ASNT_INDEM <> 0 "
            SQL += " AND ASNT_TIPO_REGISTRO = 'EXCEL'"
            SQL += " ORDER BY ASNT_PRODUC ASC"

            Me.DbCentral.TraerLector(SQL)

            While Me.DbCentral.mDbLector.Read
                Me.m_Total = CDbl(Me.DbCentral.mDbLector.Item("ASNT_INDEM"))

                If IsDBNull(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")) = False Then
                    Me.m_Nombre = CStr(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")).Replace("'", "´")
                    Me.m_Descripcion = "Indemnización " & CStr(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")).Replace("'", "´")
                End If


                Me.InsertaOracle("AC", 1, Me.m_EmpGrupoCod, Me.m_EmpCod, CType(Now.Year, String), 1, Linea, Me.CtaTotalIndem, Me.m_IndicadorDebe, Me.m_Descripcion, Me.m_Total, "NO", "CIF", Me.m_Nombre, "SI", Format(Me.m_Fecha, "dd/MM/yyyy"), "INDEMNIZACION", CStr(Me.DbCentral.mDbLector.Item("ASNT_PRODUC")), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2)
                Me.Linea = Me.Linea + 1

            End While
            Me.DbCentral.mDbLector.Close()

            ' SEGURIDAD SOCIAL EMPRESA
            SQL = "SELECT NVL(SUM(ASNT_SSEMPE),0) AS ASNT_BRUTO,ASNT_CFCPTOS_COD FROM TN_ASNT WHERE ASNT_F_ATOCAB = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            SQL += " AND ASNT_TIPO_REGISTRO = 'EXCEL'"
            SQL += " GROUP BY ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_EMP_NUM,ASNT_CFCPTOS_COD"

            Me.DbCentral.TraerLector(SQL)

            While Me.DbCentral.mDbLector.Read
                Me.m_Total = CDbl(Me.DbCentral.mDbLector.Item("ASNT_BRUTO"))

                Me.m_Nombre = ""
                Me.m_Descripcion = "NOMINAS MES " & Format(Me.m_Fecha, "MMMM") & "  " & Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2)

                Me.InsertaOracle("AC", 1, Me.m_EmpGrupoCod, Me.m_EmpCod, CType(Now.Year, String), 1, Linea, Me.CtaTotalSsEmpre, Me.m_IndicadorDebe, Me.m_Descripcion, Me.m_Total, "NO", "CIF", Me.m_Nombre, "SI", Format(Me.m_Fecha, "dd/MM/yyyy"), "SEGURIDAD SOCIAL EMPRESA", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3)
                Me.Linea = Me.Linea + 1

            End While
            Me.DbCentral.mDbLector.Close()


            ' NETO
            SQL = "SELECT *  FROM TN_ASNT WHERE ASNT_F_ATOCAB = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            SQL += " AND ASNT_NETO <> 0 "
            SQL += " AND ASNT_TIPO_REGISTRO = 'EXCEL'"
            SQL += " ORDER BY ASNT_PRODUC ASC"

            Me.DbCentral.TraerLector(SQL)

            While Me.DbCentral.mDbLector.Read
                Me.m_Total = CDbl(Me.DbCentral.mDbLector.Item("ASNT_NETO"))

                If IsDBNull(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")) = False Then
                    Me.m_Nombre = CStr(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")).Replace("'", "´")
                    Me.m_Descripcion = "Neto " & CStr(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")).Replace("'", "´")
                End If


                Me.InsertaOracle("AC", 1, Me.m_EmpGrupoCod, Me.m_EmpCod, CType(Now.Year, String), 1, Linea, Me.CtaTotalNeto & CStr(Me.DbCentral.mDbLector.Item("ASNT_PRODUC")), Me.m_IndicadorHaber, Me.m_Descripcion, Me.m_Total, "NO", "CIF", Me.m_Nombre, "SI", Format(Me.m_Fecha, "dd/MM/yyyy"), "NETO", CStr(Me.DbCentral.mDbLector.Item("ASNT_PRODUC")), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4)
                Me.Linea = Me.Linea + 1

            End While
            Me.DbCentral.mDbLector.Close()


            ' IRPF
            SQL = "SELECT NVL(SUM(ASNT_IRPF),0) AS ASNT_IRPF,ASNT_CFCPTOS_COD FROM TN_ASNT WHERE ASNT_F_ATOCAB = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            SQL += " AND ASNT_TIPO_REGISTRO = 'EXCEL'"
            SQL += " GROUP BY ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_EMP_NUM,ASNT_CFCPTOS_COD"

            Me.DbCentral.TraerLector(SQL)

            While Me.DbCentral.mDbLector.Read
                Me.m_Total = CDbl(Me.DbCentral.mDbLector.Item("ASNT_IRPF"))

                Me.m_Nombre = ""
                Me.m_Descripcion = "NOMINAS MES " & Format(Me.m_Fecha, "MMMM") & "  " & Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2)



                Me.InsertaOracle("AC", 1, Me.m_EmpGrupoCod, Me.m_EmpCod, CType(Now.Year, String), 1, Linea, Me.CtaTotalIrpf, Me.m_IndicadorHaber, Me.m_Descripcion, Me.m_Total, "NO", "CIF", Me.m_Nombre, "SI", Format(Me.m_Fecha, "dd/MM/yyyy"), "IRPF", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9)
                Me.Linea = Me.Linea + 1

            End While
            Me.DbCentral.mDbLector.Close()

            ' SEGURIDAD SOCIAL TOTAL
            SQL = "SELECT NVL(SUM(ASNT_SSTOTAL),0) AS ASNT_SSTOTAL,ASNT_CFCPTOS_COD FROM TN_ASNT WHERE ASNT_F_ATOCAB = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            SQL += " AND ASNT_TIPO_REGISTRO = 'EXCEL'"
            SQL += " GROUP BY ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_EMP_NUM,ASNT_CFCPTOS_COD"

            Me.DbCentral.TraerLector(SQL)

            While Me.DbCentral.mDbLector.Read
                Me.m_Total = CDbl(Me.DbCentral.mDbLector.Item("ASNT_SSTOTAL"))

                Me.m_Nombre = ""
                Me.m_Descripcion = "NOMINAS MES " & Format(Me.m_Fecha, "MMMM") & "  " & Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2)



                Me.InsertaOracle("AC", 1, Me.m_EmpGrupoCod, Me.m_EmpCod, CType(Now.Year, String), 1, Linea, Me.CtaTotalSstotal, Me.m_IndicadorHaber, Me.m_Descripcion, Me.m_Total, "NO", "CIF", Me.m_Nombre, "SI", Format(Me.m_Fecha, "dd/MM/yyyy"), "SEGURIDAD SOCIAL TOTAL", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10)
                Me.Linea = Me.Linea + 1

            End While
            Me.DbCentral.mDbLector.Close()


            ' 755-PERSO
            SQL = "SELECT *  FROM TN_ASNT WHERE ASNT_F_ATOCAB = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            SQL += " AND ASNT_755PERSO <> 0 "
            SQL += " AND ASNT_TIPO_REGISTRO = 'EXCEL'"
            SQL += " ORDER BY ASNT_PRODUC ASC"

            Me.DbCentral.TraerLector(SQL)

            While Me.DbCentral.mDbLector.Read
                Me.m_Total = CDbl(Me.DbCentral.mDbLector.Item("ASNT_755PERSO"))

                If IsDBNull(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")) = False Then
                    Me.m_Nombre = CStr(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")).Replace("'", "´")
                    Me.m_Descripcion = "755 Personal " & CStr(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")).Replace("'", "´")
                End If


                Me.InsertaOracle("AC", 1, Me.m_EmpGrupoCod, Me.m_EmpCod, CType(Now.Year, String), 1, Linea, Me.CtaTotal755Perso, Me.m_IndicadorHaber, Me.m_Descripcion, Me.m_Total, "NO", "CIF", Me.m_Nombre, "SI", Format(Me.m_Fecha, "dd/MM/yyyy"), "755-PERSO", CStr(Me.DbCentral.mDbLector.Item("ASNT_PRODUC")), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5)
                Me.Linea = Me.Linea + 1

            End While
            Me.DbCentral.mDbLector.Close()

            ' EMBARGO / CUOTA
            SQL = "SELECT *  FROM TN_ASNT WHERE ASNT_F_ATOCAB = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            SQL += " AND ASNT_EMBCUOTA <> 0 "
            SQL += " AND ASNT_TIPO_REGISTRO = 'EXCEL'"
            SQL += " ORDER BY ASNT_PRODUC ASC"

            Me.DbCentral.TraerLector(SQL)

            While Me.DbCentral.mDbLector.Read
                Me.m_Total = CDbl(Me.DbCentral.mDbLector.Item("ASNT_EMBCUOTA"))

                If IsDBNull(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")) = False Then
                    Me.m_Nombre = CStr(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")).Replace("'", "´")
                    Me.m_Descripcion = "Embargo / Couta " & CStr(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")).Replace("'", "´")
                End If


                Me.InsertaOracle("AC", 1, Me.m_EmpGrupoCod, Me.m_EmpCod, CType(Now.Year, String), 1, Linea, Me.CtaTotalEmbargoCuota, Me.m_IndicadorHaber, Me.m_Descripcion, Me.m_Total, "NO", "CIF", Me.m_Nombre, "SI", Format(Me.m_Fecha, "dd/MM/yyyy"), "EMBARGO / CUOTA", CStr(Me.DbCentral.mDbLector.Item("ASNT_PRODUC")), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6)
                Me.Linea = Me.Linea + 1

            End While
            Me.DbCentral.mDbLector.Close()


            ' ANTICIPOS / PRESTAMOS
            SQL = "SELECT *  FROM TN_ASNT WHERE ASNT_F_ATOCAB = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            SQL += " AND ASNT_ANTIPREST <> 0 "
            SQL += " AND ASNT_TIPO_REGISTRO = 'EXCEL'"
            SQL += " ORDER BY ASNT_PRODUC ASC"

            Me.DbCentral.TraerLector(SQL)

            While Me.DbCentral.mDbLector.Read
                Me.m_Total = CDbl(Me.DbCentral.mDbLector.Item("ASNT_ANTIPREST"))

                If IsDBNull(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")) = False Then
                    Me.m_Nombre = CStr(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")).Replace("'", "´")
                    Me.m_Descripcion = "Anticipos / Préstamos " & CStr(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")).Replace("'", "´")
                End If


                Me.InsertaOracle("AC", 1, Me.m_EmpGrupoCod, Me.m_EmpCod, CType(Now.Year, String), 1, Linea, Me.CtaTotalAntiPrest, Me.m_IndicadorHaber, Me.m_Descripcion, Me.m_Total, "NO", "CIF", Me.m_Nombre, "SI", Format(Me.m_Fecha, "dd/MM/yyyy"), "ANTICIPOS / PRESTAMOS", CStr(Me.DbCentral.mDbLector.Item("ASNT_PRODUC")), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7)
                Me.Linea = Me.Linea + 1

            End While
            Me.DbCentral.mDbLector.Close()


            ' OTROS DESCUENTOS
            SQL = "SELECT *  FROM TN_ASNT WHERE ASNT_F_ATOCAB = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            SQL += " AND ASNT_OTROSDTOS <> 0 "
            SQL += " AND ASNT_TIPO_REGISTRO = 'EXCEL'"
            SQL += " ORDER BY ASNT_PRODUC ASC"

            Me.DbCentral.TraerLector(SQL)

            While Me.DbCentral.mDbLector.Read
                Me.m_Total = CDbl(Me.DbCentral.mDbLector.Item("ASNT_OTROSDTOS"))

                If IsDBNull(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")) = False Then
                    Me.m_Nombre = CStr(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")).Replace("'", "´")
                    Me.m_Descripcion = "Otros Descuentos " & CStr(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")).Replace("'", "´")
                End If


                Me.InsertaOracle("AC", 1, Me.m_EmpGrupoCod, Me.m_EmpCod, CType(Now.Year, String), 1, Linea, Me.CtaTotalOtrosDtos, Me.m_IndicadorHaber, Me.m_Descripcion, Me.m_Total, "NO", "CIF", Me.m_Nombre, "SI", Format(Me.m_Fecha, "dd/MM/yyyy"), "OTROS DESCUENTOS", CStr(Me.DbCentral.mDbLector.Item("ASNT_PRODUC")), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8)
                Me.Linea = Me.Linea + 1

            End While
            Me.DbCentral.mDbLector.Close()



        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

        End Try
    End Sub
    Private Sub GeneraAsientoRemesa()
        Try

            ' LIQUIDO A PERCIBIR
            SQL = "SELECT *  FROM TN_ASNT WHERE ASNT_F_ATOCAB = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            SQL += " AND ASNT_BRUTO <> 0 "
            SQL += " AND ASNT_TIPO_REGISTRO = 'EXCEL'"
            SQL += " ORDER BY ASNT_PRODUC ASC"

            Me.DbCentral.TraerLector(SQL)

            While Me.DbCentral.mDbLector.Read
                Me.m_Total = CDbl(Me.DbCentral.mDbLector.Item("ASNT_BRUTO"))

                '  MsgBox("BUSCAR A QUI LA CUENTA")
                SQL = "SELECT NVL(NIFS_PROD,'?') FROM TN_NIFS "
                SQL += " WHERE NIFS_NIF = '" & CStr(Me.DbCentral.mDbLector.Item("ASNT_CIF")) & "'"

                Me.Result = Me.DbCentral.EjecutaSqlScalar2(SQL)

                Me.CtaProductor = Me.CtaTotalNeto & Me.Result

                If IsDBNull(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")) = False Then
                    Me.m_Nombre = CStr(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")).Replace("'", "´")
                    Me.m_Descripcion = "Remesa : " & CStr(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")).Replace("'", "´")
                End If


                Me.InsertaOracle("AC", 1, Me.m_EmpGrupoCod, Me.m_EmpCod, CType(Now.Year, String), 1, Linea, Me.CtaProductor, Me.m_IndicadorDebe, Me.m_Descripcion, Me.m_Total, "NO", CStr(Me.DbCentral.mDbLector.Item("ASNT_PRODUC")), Me.m_Nombre, "SI", Format(Me.m_Fecha, "dd/MM/yyyy"), "LÌQUIDO A PERCIBIR", CStr(Me.DbCentral.mDbLector.Item("ASNT_PRODUC")), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)
                Me.Linea = Me.Linea + 1

            End While
            Me.DbCentral.mDbLector.Close()



            ' TOTAL banco
            SQL = "SELECT NVL(SUM(ASNT_BRUTO),0) AS ASNT_BRUTO,ASNT_CFCPTOS_COD FROM TN_ASNT WHERE ASNT_F_ATOCAB = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            SQL += " AND ASNT_TIPO_REGISTRO = 'EXCEL'"
            SQL += " GROUP BY ASNT_EMPGRUPO_COD,ASNT_EMP_COD,ASNT_EMP_NUM,ASNT_CFCPTOS_COD"

            Me.DbCentral.TraerLector(SQL)

            While Me.DbCentral.mDbLector.Read
                Me.m_Total = CDbl(Me.DbCentral.mDbLector.Item("ASNT_BRUTO"))

                Me.m_Nombre = ""
                Me.m_Descripcion = "REMESA MES " & Format(DateAdd(DateInterval.Month, -1, Me.m_Fecha), "MMMM") & "  " & Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2)
                Me.InsertaOracle("AC", 1, Me.m_EmpGrupoCod, Me.m_EmpCod, CType(Now.Year, String), 1, Linea, Me.CtaBanco, Me.m_IndicadorHaber, Me.m_Descripcion, Me.m_Total, "NO", "CIF", Me.m_Nombre, "SI", Format(Me.m_Fecha, "dd/MM/yyyy"), "BANCO ", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3)
                Me.Linea = Me.Linea + 1

            End While
            Me.DbCentral.mDbLector.Close()





        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

        End Try
    End Sub
    Private Sub MostrarAsientos()
        Try
            SQL = "SELECT *  FROM TN_ASNT WHERE ASNT_F_ATOCAB = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            SQL += " AND ASNT_TIPO_REGISTRO = " & "'AC'"

            SQL += " ORDER BY ASNT_APU_ORDE,ASNT_LINEA ASC"

            Me.DataGridAsientos.DataSource = Me.DbCentral.TraerDataset(SQL, "ASIENTOS")

            Me.DataGridAsientos.DataMember = "ASIENTOS"



        Catch ex As Exception

        End Try
    End Sub
    Private Sub AuditaAsientosCuadre(ByVal vAjustar As Boolean)
        Try


            Me.m_TotalDebe = 0
            Me.m_TotalHaber = 0
            Me.m_TotalDiferencia = 0

            SQL = "SELECT NVL(SUM(ASNT_I_MONEMP),0)  FROM TN_ASNT WHERE ASNT_F_ATOCAB = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            SQL += " AND ASNT_TIPO_REGISTRO = " & "'AC'"
            SQL += " AND ASNT_CFCPTOS_COD = '" & Me.m_IndicadorDebe & "'"

            SQL += " ORDER BY ASNT_APU_ORDE,ASNT_LINEA ASC"

            Me.m_TotalDebe = CDbl(Me.DbCentral.EjecutaSqlScalar(SQL))
            Me.TextBoxTotalDebe.Text = Me.m_TotalDebe


            SQL = "SELECT NVL(SUM(ASNT_I_MONEMP),0)  FROM TN_ASNT WHERE ASNT_F_ATOCAB = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            SQL += " AND ASNT_TIPO_REGISTRO = " & "'AC'"
            SQL += " AND ASNT_CFCPTOS_COD = '" & Me.m_IndicadorHaber & "'"

            SQL += " ORDER BY ASNT_APU_ORDE,ASNT_LINEA ASC"

            Me.m_TotalHaber = CDbl(Me.DbCentral.EjecutaSqlScalar(SQL))
            Me.TextBoxTotalHaber.Text = Me.m_TotalHaber

            Me.m_TotalDiferencia = Me.m_TotalDebe - Me.m_TotalHaber

            Me.TextBoxCuadre.Text = Math.Round(CDec(Me.m_TotalDiferencia), 2)


            If Me.m_TotalDiferencia <> 0 Then
                Me.TextBoxCuadre.ForeColor = Color.White
                Me.TextBoxCuadre.BackColor = Color.Maroon
                Me.TextBoxCuadre.Update()
            Else
                Me.TextBoxCuadre.ForeColor = Color.White
                Me.TextBoxCuadre.BackColor = Color.Green
                Me.TextBoxCuadre.Update()
            End If

            If vAjustar = True Then
                If Me.m_TotalHaber > Me.m_TotalDebe Then
                    Me.m_TotalDiferencia = Me.m_TotalHaber - Me.m_TotalDebe
                    Me.InsertaOracle("AC", 1, Me.m_EmpGrupoCod, Me.m_EmpCod, CType(Now.Year, String), 1, Linea, Me.m_CtaAjusteRedondeo, Me.m_IndicadorDebe, "AJUSTE REDONDEO", Math.Round(CDec(Me.m_TotalDiferencia), 2), "NO", "CIF", "", "SI", Format(Me.m_Fecha, "dd/MM/yyyy"), "AJUSTE REDONDEO", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 99)
                    Me.Linea = Me.Linea + 1
                End If

                If Me.m_TotalHaber < Me.m_TotalDebe Then
                    Me.m_TotalDiferencia = Me.m_TotalDebe - Me.m_TotalHaber
                    Me.InsertaOracle("AC", 1, Me.m_EmpGrupoCod, Me.m_EmpCod, CType(Now.Year, String), 1, Linea, Me.m_CtaAjusteRedondeo, Me.m_IndicadorHaber, "AJUSTE REDONDEO", Math.Round(CDec(Me.m_TotalDiferencia), 2), "NO", "CIF", "", "SI", Format(Me.m_Fecha, "dd/MM/yyyy"), "AJUSTE REDONDEO", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 99)
                    Me.Linea = Me.Linea + 1
                End If
            End If


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub BorrarAsientos()
        Try
            SQL = "DELETE  FROM TN_ASNT WHERE ASNT_F_ATOCAB = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            Me.DbCentral.EjecutaSqlCommit(SQL)

            SQL = "DELETE TH_ERRO WHERE ERRO_F_ATOCAB =  '" & Me.m_Fecha & "'"
            Me.DbCentral.EjecutaSqlCommit(SQL)

            SQL = "DELETE TH_INCI WHERE INCI_DATR =  '" & Me.m_Fecha & "'"
            SQL += " AND INCI_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND INCI_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND INCI_EMP_NUM =  " & Me.m_EmpNum
            SQL += " AND INCI_ORIGEN =  '" & "NOMINA A3" & "'"
            Me.DbCentral.EjecutaSqlCommit(SQL)



        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub AbrirConexiones()
        Try
            '---------------------------------------------------------------------------------------------------
            ' Conecta con la Base de Datos "central"
            '----------------------------------------------------------------------------------------------------
            '      Me.DbCentral = New C_DATOS.C_DatosOledb(MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"))
            '     Me.DbCentral.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")


            Me.DbWriteCentral = New C_DATOS.C_DatosOledb(MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"))
            Me.DbWriteCentral.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")

            Me.DbSpyro = New C_DATOS.C_DatosOledb(Me.m_StrConexionSpyro)
            Me.DbSpyro.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")



        Catch ex As Exception

        End Try
    End Sub
    Private Sub CerrarConexion()
        Try
            '---------------------------------------------------------------------------------------------------
            ' Conecta con la Base de Datos "central"
            '----------------------------------------------------------------------------------------------------
            If Me.DbCentral.EstadoConexion = ConnectionState.Open Then
                Me.DbCentral.CerrarConexion()
            End If
            If Me.DbWriteCentral.EstadoConexion = ConnectionState.Open Then
                Me.DbWriteCentral.CerrarConexion()
            End If
            If Me.DbSpyro.EstadoConexion = ConnectionState.Open Then
                Me.DbSpyro.CerrarConexion()
            End If

        Catch ex As Exception

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

            Dim TextCol10 As New DataGridTextBoxColumn
            TextCol10.MappingName = "HOTEL_EMP_NUM"
            TextCol10.HeaderText = "Número de Empresa"
            TextCol10.Width = 100
            ts1.GridColumnStyles.Add(TextCol10)


            Dim TextCol11 As New DataGridTextBoxColumn
            TextCol11.MappingName = "PARA_FILE_ORIGEN"
            TextCol11.HeaderText = "Origen Nómina"
            TextCol11.Width = 100
            ts1.GridColumnStyles.Add(TextCol11)


            Dim TextCol12 As New DataGridTextBoxColumn
            TextCol12.MappingName = "PARA_FILE_DESTINO"
            TextCol12.HeaderText = "Destino Fichero"
            TextCol12.Width = 100
            ts1.GridColumnStyles.Add(TextCol12)

            Dim TextCol13 As New DataGridTextBoxColumn
            TextCol13.MappingName = "PARA_CFATODIARI_COD"
            TextCol13.HeaderText = "Diario"
            TextCol13.Width = 50
            ts1.GridColumnStyles.Add(TextCol13)


            Me.DataGridHoteles.TableStyles.Clear()
            Me.DataGridHoteles.TableStyles.Add(ts1)



        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub LimpiaGridAsientos()
        Try
            SQL = "SELECT 'NINGUNO' FROM DUAL"
            Me.DataGridAsientos.DataSource = DbCentral.TraerDataset(SQL, "LIMPIA")
            Me.DataGridAsientos.DataMember = "LIMPIA"
            Me.DataGridAsientos.Update()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub InsertaOracle(ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
                                     ByVal vCfatocab_Refer As Integer, ByVal vLinea As Integer _
                                     , ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double, _
                                       ByVal vAjuste As String, ByVal vCif As String, ByVal vNombre As String, ByVal vImprimir As String, ByVal vFechaValor As Date, ByVal vAuxiliar As String, ByVal vProductor As String, _
                                      ByVal vBruto As Double, ByVal vIndem As Double, ByVal vSsempre As Double, ByVal vNeto As Double, _
                                     ByVal vIrpf As Double, ByVal vSstotal As Double, ByVal v755Perso As Double, ByVal vEmbcuota As Double, _
                                     ByVal vAntiPres As Double, ByVal vOtrosDtos As Double, ByVal vApuOrden As Integer)

        Try

            If vCfcptos_Cod = Me.m_IndicadorDebe Then
                Me.m_Debe = vImonep
                Me.m_Haber = 0
            ElseIf vCfcptos_Cod = Me.m_IndicadorHaber Then
                Me.m_Debe = 0
                Me.m_Haber = vImonep
            Else
                Me.m_Debe = 0
                Me.m_Haber = 0
            End If


            SQL = "INSERT INTO TN_ASNT ( "
            SQL += "   ASNT_TIPO_REGISTRO, ASNT_EMPGRUPO_COD, ASNT_EMP_COD,  "
            SQL += "   ASNT_CFEJERC_COD, ASNT_CFATODIARI_COD, ASNT_CFATOCAB_REFER,  "
            SQL += "   ASNT_LINEA, ASNT_CFCTA_COD, ASNT_CFCPTOS_COD,  "
            SQL += "   ASNT_AMPCPTO, ASNT_I_MONEMP, ASNT_CONCIL_SN,  "

            SQL += "   ASNT_F_ATOCAB, ASNT_F_VALOR, ASNT_NOMBRE,  "
            SQL += "   ASNT_DEBE, ASNT_HABER, ASNT_AJUSTAR,  "
            SQL += "   ASNT_AJUSTE, ASNT_CIF, ASNT_IMPRIMIR,  "

            SQL += "   ASNT_AUXILIAR_STRING, ASNT_AUXILIAR_NUMERICO, ASNT_AUXILIAR_STRING2,  "
            SQL += "   ASNT_DPTO_CODI, ASNT_AX_STATUS, ASNT_ERR_MESSAGE,  "
            SQL += "   ASNT_EMP_NUM, ASNT_PRODUC, ASNT_BRUTO,  "

            SQL += "   ASNT_INDEM, ASNT_SSEMPE, ASNT_NETO,  "
            SQL += "   ASNT_IRPF, ASNT_SSTOTAL, ASNT_755PERSO,  "
            SQL += "   ASNT_EMBCUOTA, ASNT_ANTIPREST, ASNT_OTROSDTOS,ASNT_APU_ORDE)  "
            SQL += "VALUES (  '"


            SQL += vTipo & "','"
            SQL += vEmpGrupoCod & "','"
            SQL += vEmpCod & "','"
            SQL += vCefejerc_Cod & "','"
            SQL += Me.m_Cfatodiari_Cod & "',"
            SQL += vAsiento & ","
            SQL += Linea & ",'"
            SQL += vCfcta_Cod & "','"
            SQL += vCfcptos_Cod & "','"
            SQL += Mid(vAmpcpto, 1, 40) & "',"
            SQL += vImonep & ","
            SQL += "'N','"
            SQL += Format(Me.m_Fecha, "dd/MM/yyyy") & "','"
            SQL += Format(vFechaValor, "dd/MM/yyyy") & "','"
            SQL += vNombre.Replace("'", "''") & "'," & Me.m_Debe & "," & Me.m_Haber & ",'" & vAjuste & "',0,'" & vCif & "','" & _
            vImprimir & "','" & vAuxiliar & "',0,'','',0,''," & Me.m_EmpNum & ",'" & vProductor & "'," & _
            vBruto & "," & vIndem & "," & vSsempre & "," & vNeto & "," & vIrpf & "," & vSstotal & "," & v755Perso & "," & vEmbcuota & "," & vAntiPres & "," & vOtrosDtos & "," & vApuOrden & ")"


            Me.DbCentral.EjecutaSqlCommit(SQL)
        Catch EX As Exception

            MsgBox(EX.Message, MsgBoxStyle.Information, "Inserta Asiento Oracle")
        End Try
    End Sub

    Private Sub LeeParametros()
        Try
            SQL = "SELECT NVL(PARA_FILE_ORIGEN,'?') FROM TN_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum
            Me.m_ParaFileOrigen = Me.DbCentral.EjecutaSqlScalar(SQL)


            SQL = "SELECT NVL(PARA_FILE_DESTINO,'?') FROM TN_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum
            Me.m_ParaFileDestino = Me.DbCentral.EjecutaSqlScalar(SQL)


            SQL = "SELECT NVL(PARA_CFATODIARI_COD,'?') FROM TN_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum
            Me.m_Cfatodiari_Cod = Me.DbCentral.EjecutaSqlScalar(SQL)

            If Me.m_Cfatodiari_Cod = "?" Then
                MsgBox("No tiene definido Diario para esta Empresa , Revise Parámetros", MsgBoxStyle.Information, "Atención")
            End If

            SQL = "SELECT NVL(PARA_CFATOTIP_COD,'?') FROM TN_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum
            Me.m_CfatoTip_Cod = Me.DbCentral.EjecutaSqlScalar(SQL)


            SQL = "SELECT NVL(PARA_DEBE,'?') FROM TH_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum
            Me.m_IndicadorDebe = Me.DbCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_HABER,'?') FROM TH_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum
            Me.m_IndicadorHaber = Me.DbCentral.EjecutaSqlScalar(SQL)


            ' Carga de Cueentas Contables
            SQL = "SELECT NVL(PARA_CTA_BRUTO,'?') AS CUENTA FROM TN_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum
            Me.CtaTotalBruto = DbCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_CTA_INDEM,'?') AS CUENTA FROM TN_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum
            Me.CtaTotalIndem = DbCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_CTA_SSEMPRE,'?') AS CUENTA FROM TN_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum
            Me.CtaTotalSsEmpre = DbCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_CTA_NETO,'?') AS CUENTA FROM TN_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum
            Me.CtaTotalNeto = DbCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_CTA_IRPF,'?') AS CUENTA FROM TN_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum
            Me.CtaTotalIrpf = DbCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_CTA_SSTOTAL,'?') AS CUENTA FROM TN_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum
            Me.CtaTotalSstotal = DbCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_CTA_755PERSO,'?') AS CUENTA FROM TN_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum
            Me.CtaTotal755Perso = DbCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_CTA_EMBCUOTA,'?') AS CUENTA FROM TN_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum
            Me.CtaTotalEmbargoCuota = DbCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_CTA_ANTIPREST,'?') AS CUENTA FROM TN_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum
            Me.CtaTotalAntiPrest = DbCentral.EjecutaSqlScalar(SQL)

            SQL = "SELECT NVL(PARA_CTA_OTROS,'?') AS CUENTA FROM TN_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum
            Me.CtaTotalOtrosDtos = DbCentral.EjecutaSqlScalar(SQL)


            ' otras cuentas 

            SQL = "SELECT NVL(PARA_CTA_REDONDEO,'?') FROM TH_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum
            Me.m_CtaAjusteRedondeo = Me.DbCentral.EjecutaSqlScalar(SQL)

            ' otros
            SQL = "SELECT NVL(HOTEL_SPYRO,'?') AS CUENTA FROM TH_HOTEL "
            SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND HOTEL_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND HOTEL_EMP_NUM = " & Me.m_EmpNum
            Me.m_StrConexionSpyro = DbCentral.EjecutaSqlScalar(SQL)

            If Me.m_StrConexionSpyro = "?" Or IsNothing(Me.m_StrConexionSpyro) = True Then
                MsgBox("´No hay Cadena de Conexión a la Base de Datos SPYRO", MsgBoxStyle.Critical, "Atención")
            End If



            SQL = "SELECT NVL(PARA_VALIDA_SPYRO,0) FROM TH_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND PARA_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND PARA_EMP_NUM = " & Me.m_EmpNum

            If Me.DbCentral.EjecutaSqlScalar(SQL) = "1" Then
                Me.m_ValidaSpyro = True
            Else
                Me.m_ValidaSpyro = False
            End If


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub SpyroCompruebaCuentas()
        Try
            SQL = "SELECT ASNT_CFCTA_COD,ASNT_TIPO_REGISTRO,ASNT_CFATOCAB_REFER,ASNT_LINEA,ASNT_CFCPTOS_COD,NVL(ASNT_AMPCPTO,'?') AS ASNT_AMPCPTO,NVL(ASNT_NOMBRE,'?') AS ASNT_NOMBRE FROM TN_ASNT WHERE "
            SQL += "     ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            SQL += " AND ASNT_TIPO_REGISTRO = " & "'AC'"
            SQL += " AND ASNT_F_VALOR = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"

            Me.DbCentral.TraerLector(SQL)
            Me.m_HayErrores = False
            While Me.DbCentral.mDbLector.Read
                Me.SpyroCompruebaCuenta(CStr(Me.DbCentral.mDbLector.Item("ASNT_CFCTA_COD")), _
                                        CStr(Me.DbCentral.mDbLector.Item("ASNT_TIPO_REGISTRO")), _
                                        CInt(Me.DbCentral.mDbLector.Item("ASNT_CFATOCAB_REFER")), _
                                        CInt(Me.DbCentral.mDbLector.Item("ASNT_LINEA")), _
                                        CStr(Me.DbCentral.mDbLector.Item("ASNT_CFCPTOS_COD")), _
                                        CStr(Me.DbCentral.mDbLector.Item("ASNT_AMPCPTO")), _
                                        CStr(Me.DbCentral.mDbLector.Item("ASNT_NOMBRE")))

            End While
            Me.DbCentral.mDbLector.Close()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Function SpyroBuscaCuentadeunNif(ByVal vNif As String) As String
        Try
            SQL = "SELECT CFCTA.COD FROM CFCTA ,RSOCIAL WHERE CFCTA.EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND CFCTA.EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND CFCTA.RSOCIAL_COD = RSOCIAL.COD  "
            SQL += "  AND RSOCIAL.COD  = '" & vNif & "'"

            Me.Result = Me.DbSpyro.EjecutaSqlScalar2(SQL)

            Return Me.Result


        Catch ex As Exception
            Return "?"
        End Try
    End Function

    Private Sub SpyroCompruebaCuenta(ByVal vCuenta As String, ByVal vTipo As String, ByVal vAsiento As Integer, ByVal vLinea As Integer, ByVal vDebeHaber As String, ByVal vAmpcpto As String, ByVal vNombre As String)
        Try
            Me.TextBoxDebug.AppendText("________________________________________________________________________________" & vbCrLf)

            Me.TextBoxDebug.AppendText("Validando Plan de Cuentas Spyro " & vCuenta.PadRight(20, CChar(" ")) & " Longitud : " & vCuenta.Length & vbCrLf)



            Me.TextBoxDebug.Update()
            Me.Update()


            SQL = "SELECT COD FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += "  AND COD = '" & vCuenta & "'"



            If IsNothing(Me.DbSpyro.EjecutaSqlScalar(SQL)) = True Then
                Me.m_HayErrores = True
                Me.TextBoxDebug.AppendText("SPYRO   : " & vCuenta & "  No se localiza en Plan de Cuentas de Spyro" & vbCrLf)
                Me.m_Texto = "SPYRO   : " & vCuenta & "  No se localiza en Plan de Cuentas de Spyro"
                Me.GestionIncidencia(Me.m_EmpGrupoCod, Me.m_EmpCod, Me.m_EmpNum, Me.m_Texto & " + " & vAmpcpto & " + " & vNombre)
                Exit Sub
            End If


            SQL = "SELECT APTESDIR_SN FROM CFCTA WHERE EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += "  AND COD = '" & vCuenta & "'"



            If Me.DbSpyro.EjecutaSqlScalar(SQL) <> "S" Then
                Me.m_HayErrores = True
                Me.TextBoxDebug.AppendText("SPYRO   : " & vCuenta & "  No es una Cuenta de Apuntes Directos en Plan de Cuentas Spyro" & vbCrLf)
                Me.m_Texto = "SPYRO   : " & vCuenta & "  No es una Cuenta de Apuntes Directos en Plan de Cuentas Spyro"
                Me.GestionIncidencia(Me.m_EmpGrupoCod, Me.m_EmpCod, Me.m_EmpNum, Me.m_Texto & " + " & vAmpcpto & " + " & vNombre)


                Exit Sub
            End If


        Catch ex As OleDb.OleDbException
            MsgBox(ex.Message, MsgBoxStyle.Information, " Localiza Cuenta Contable SPYRO")
        End Try
    End Sub
    Private Sub CrearFichero()

        Try
            Dim DInf As New DirectoryInfo(Me.m_ParaFileDestino)
            If DInf.Exists = False Then
                MsgBox("No dispone de acceso a " & Me.m_ParaFileDestino & vbCrLf & "Se cancela el Proceso", MsgBoxStyle.Information, "Atención Fichero NO Creado")
                FilegrabaStatus = False
                Exit Sub
            End If
            FileName = Me.m_ParaFileDestino & "\NOMI-" & Format(Me.m_Fecha, "dd-MM-yyyy") & ".TXT"
            'Filegraba = New StreamWriter(FileName, False, System.Text.Encoding.UTF8)
            Filegraba = New StreamWriter(FileName, False, System.Text.Encoding.ASCII)

            Filegraba.WriteLine("")
            FilegrabaStatus = True
        Catch ex As Exception
            FilegrabaStatus = False
            MsgBox("No dispone de acceso al Fichero " & FileName, MsgBoxStyle.Information, "Atención")
        End Try
    End Sub
    Private Sub GeneraFichero()
        Try
            SQL = "SELECT *  FROM TN_ASNT WHERE ASNT_F_ATOCAB = '" & Format(Me.m_Fecha, "dd/MM/yyyy") & "'"
            SQL += " AND ASNT_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"
            SQL += " AND ASNT_EMP_COD = '" & Me.m_EmpCod & "'"
            SQL += " AND ASNT_EMP_NUM = " & Me.m_EmpNum
            SQL += " AND ASNT_TIPO_REGISTRO = " & "'AC'"

            SQL += " ORDER BY ASNT_APU_ORDE,ASNT_LINEA ASC"

            Me.DbCentral.TraerLector(SQL)
            While Me.DbCentral.mDbLector.Read
                Me.GeneraFileAC("AC", Me.m_EmpGrupoCod, Me.m_EmpCod, CType(Me.m_Fecha.Year, String), CStr(Me.DbCentral.mDbLector.Item("ASNT_CFCTA_COD")), CStr(Me.DbCentral.mDbLector.Item("ASNT_CFCPTOS_COD")), CStr(Me.DbCentral.mDbLector.Item("ASNT_AMPCPTO")), CDbl(Me.DbCentral.mDbLector.Item("ASNT_I_MONEMP")))

            End While
            Me.DbCentral.mDbLector.Close()
            Me.Filegraba.Close()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub GeneraFileAC(ByVal vTipo As String, ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vCefejerc_Cod As String, _
    ByVal vCfcta_Cod As String, ByVal vCfcptos_Cod As String, ByVal vAmpcpto As String, ByVal vImonep As Double)
        Try
            Dim FechaAsiento As String

            FechaAsiento = Format(Me.m_Fecha, "ddMMyyyy")

            '-------------------------------------------------------------------------------------------------
            '  Apuntes Contables(CFATOLIN)
            '-------------------------------------------------------------------------------------------------
            Me.Filegraba.WriteLine(MyCharToOem(vTipo.PadRight(2, CChar(" ")) & _
            vEmpGrupoCod.PadRight(4, CChar(" ")) & _
            vEmpCod.PadRight(4, CChar(" ")) & _
            Mid(FechaAsiento, 5, 4) & _
            Me.m_Cfatodiari_Cod.PadRight(4, CChar(" ")) & _
            " ".PadLeft(8, CChar(" ")) & _
            " ".PadLeft(4, CChar(" ")) & _
            vCfcta_Cod.PadRight(15, CChar(" ")) & _
            vCfcptos_Cod.PadRight(4, CChar(" ")) & _
            Mid(vAmpcpto, 1, 40).PadRight(40, CChar(" ")) & _
            CType(vImonep, String).PadLeft(16, CChar(" ")) & _
            "N" & FechaAsiento & _
            Format(Me.m_Fecha, "ddMMyyyy") & _
            " ".PadRight(40, CChar(" ")) & _
            Me.m_CfatoTip_Cod.PadRight(4, CChar(" "))))

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Information, "Genera FileAc")
        End Try
    End Sub

    Private Sub GestionIncidencia(ByVal vEmpGrupoCod As String, ByVal vEmpCod As String, ByVal vEmpNum As Integer, ByVal vDescripcion As String)

        Try

            SQL = "INSERT INTO TH_INCI (INCI_DATR,INCI_EMPGRUPO_COD,INCI_EMP_COD,INCI_EMP_NUM,INCI_ORIGEN,INCI_DESCRIPCION) "
            SQL += " VALUES ('" & Format(Me.m_Fecha, "dd/MM/yyyy") & "','" & Me.m_EmpGrupoCod & "','" & Me.m_EmpCod & "'," & Me.m_EmpNum & ",'NOMINA A3','" & vDescripcion & "')"

            Me.DbCentral.IniciaTransaccion()

            Me.DbCentral.EjecutaSql(SQL)

            Me.DbCentral.ConfirmaTransaccion()


        Catch ex As Exception
            Me.DbCentral.CancelaTransaccion()
        End Try

    End Sub
#End Region
#Region "OTRAS FUNCIONES"
    Private Sub IniciarFiltroMSDOS()
        'Convertir de ANSI (windows) a ASCII (dos)
        Dim i As Integer
        Dim p As Integer
        '
        p = 0
        For i = 128 To 156          'de Ç a £   29
            p = p + 1
            iASCII(p) = i
        Next
        For i = 160 To 168          'de á a ¿   9
            p = p + 1
            iASCII(p) = i
        Next
        For i = 170 To 175          'de ¬ a »   6
            p = p + 1
            iASCII(p) = i
        Next
        '44 códigos asignados hasta aquí
        iASCII(45) = 225            'ß
        iASCII(46) = 230            'µ
        iASCII(47) = 241            '±
        iASCII(48) = 246            '÷
        iASCII(49) = 253            '²
        iASCII(50) = 65             'Á (A)
        iASCII(51) = 73             'Í (I)
        iASCII(52) = 79             'Ó (O)
        iASCII(53) = 85             'Ú (U)
        iASCII(54) = 73             'Ï (I)
        iASCII(55) = 65             'À (A)
        iASCII(56) = 69             'È (E)
        iASCII(57) = 73             'Ì (I)
        iASCII(58) = 79             'Ò (O)
        iASCII(59) = 85             'Ù (U)
        iASCII(60) = 69             'Ë (E)
        For i = 61 To 63            ''`´ (')
            iASCII(i) = 39
        Next
    End Sub
    Private Function MyCharToOem(ByVal sWIN As String) As String
        'Filtrar la cadena para convertirla en compatible MS-DOS
        Dim sANSI As String = "ÇüéâäàåçêëèïîìÄÅÉæÆôöòûùÿÖÜø£áíóúñÑªº¿¬½¼¡«»ßµ±÷²ÁÍÓÚÏÀÈÌÒÙË'`´"
        '
        Dim i As Integer
        Dim p As Integer
        Dim sC As Integer
        Dim sMSD As String



        'Aquí se puede poner esta comparación para saber
        'si el array está inicializado.
        'De esta forma no será necesario llamar al procedimiento
        'de inicialización antes de usar esta función.
        '(deberás quitar los comentarios)
        If iASCII(1) = 0 Then       'El primer valor debe ser 128
            IniciarFiltroMSDOS()
        End If

        sMSD = ""
        For i = 1 To Len(sWIN)
            sC = Asc(Mid$(sWIN, i, 1))
            p = InStr(sANSI, Chr(sC))
            If p > 0 Then
                sC = iASCII(p)
            End If
            sMSD = sMSD & Chr(sC)
        Next


        If mParaToOem = True Then
            Return sWIN
        Else
            Return sMSD
        End If
    End Function
#End Region
#Region "RUTINAS EXCEL"
    Private Sub MostrarHoja()
        Try
            If Me.TextBoxRutaLibroExcel.Text.Length > 0 Then
                m_File = New FileInfo(Me.TextBoxRutaLibroExcel.Text)
                If m_File.Extension <> ".xls" Or m_File.Extension <> ".xlsx" Then
                    MsgBox("No es un Libro Excel = " & m_File.Extension, MsgBoxStyle.Information, "Atención")
                    Exit Sub
                End If
            End If
            Me.ExcelApp = New Microsoft.Office.Interop.Excel.Application
            Me.ExcelApp.IgnoreRemoteRequests = True
            Me.ExcelBook = Me.ExcelApp.Workbooks.Open(Me.TextBoxRutaLibroExcel.Text, , True, , , , True, )
            Me.ExcelApp.Visible = True
            Me.ExcelApp.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlNormal
        Catch ex As Exception
            MsgBox(ex.Message, , "Mostrar Hoja")
        End Try

    End Sub
    Private Sub ContarLibrosSinUso()
        Try

            Dim P As Integer

            Dim sh As Microsoft.Office.Interop.Excel.Worksheet
            Dim i As Integer
            For Each sh In Me.ExcelApp.Sheets
                Me.ListBoxLibros.Items.Add(sh.Name)
                i = i + 1
                For P = 1 To sh.Name.Length

                    If Mid(sh.Name, P, 1) = "." Then
                        MsgBox("No use punto(.) en el nombre de la Hoja de Excel a Tratar ", MsgBoxStyle.Exclamation, "Atención")
                    End If

                Next P
            Next sh
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Function ContarHojasExcelDevuelveHoja() As String
        Try

            Dim P As Integer

            Dim sh As Microsoft.Office.Interop.Excel.Worksheet
            Dim i As Integer

            ' ojo
            sh = Nothing

            For Each sh In Me.ExcelApp.Sheets
                '  Me.ListBoxLibrosNif.Items.Add(sh.Name)
                i = i + 1
                For P = 1 To sh.Name.Length
                    If Mid(sh.Name, P, 1) = "." Then
                        MsgBox("No use punto(.) en el nombre de la Hoja de Excel a Tratar NIFS", MsgBoxStyle.Exclamation, "Atención")
                    End If

                Next P

            Next sh



            If sh.Name.Length > 0 Then
                Me.TextBoxHojaNif.Text = sh.Name
                Return sh.Name
            Else
                Return "vacio"
            End If
            Return sh.Name
        Catch ex As Exception
            MsgBox(ex.Message)
            Return "vacio"
        End Try

    End Function

    Private Sub ContarHojasExcel(ByVal vLibro As String)
        Try
            Me.ListBoxLibros.Items.Clear()
            Me.ListBoxLibros.Update()

            If IsNothing(Me.ExcelApp) = True Then
                Me.ExcelApp = New Microsoft.Office.Interop.Excel.Application
            End If

            Me.ExcelApp.IgnoreRemoteRequests = True
            '  Me.ExcelBook = Me.ExcelApp.Workbooks.Open(Me.TextBoxRutaLibroExcel.Text, , True, , , , True, )

            Me.ExcelBook = Me.ExcelApp.Workbooks.Add(vLibro)

            '  Me.ExcelApp.Visible = False
            Dim i As Integer
            Dim P As Integer

            For Each Me.ExcelSheet In Me.ExcelApp.Sheets
                Me.ListBoxLibros.Items.Add(Me.ExcelSheet.Name)
                i = i + 1

                For P = 1 To Me.ExcelSheet.Name.Length

                    If Mid(Me.ExcelSheet.Name, P, 1) = "." Then
                        MsgBox("No use punto(.) en el nombre de la Hoja de Excel a Tratar = " & Me.ExcelSheet.Name, MsgBoxStyle.Exclamation, "Atención")
                    End If

                Next P
            Next Me.ExcelSheet

            '   Me.ExcelApp.Workbooks.Close()

        Catch e As System.Runtime.InteropServices.COMException
            MsgBox("Es posible que esta Computadora no tenga instalado Microsoft Excel " & vbCrLf & e.Message, MsgBoxStyle.Information, " Exepción COM Puede Continuar ...")

        Catch ex As Exception
            MsgBox("Es posible que esta Computadora no tenga instalado Microsoft Excel " & vbCrLf & ex.Message, MsgBoxStyle.Information, "Exepción Genérica Puede Continuar ...")
        End Try

    End Sub

#End Region
#Region "EVENTOS"
    Private Sub ButtonRutaExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRutaExcel.Click

        Try
            OpenFileDialog1.CheckPathExists = True
            OpenFileDialog1.InitialDirectory = Me.m_ParaFileOrigen

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Posible Ruta No Accesible")
        End Try


        Try
            OpenFileDialog1.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*"
            OpenFileDialog1.FilterIndex = 1

            '  OpenFileDialog1.RestoreDirectory = True

            If OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

                If Me.OpenFileDialog1.FileName.Length > 0 Then
                    Me.TextBoxRutaLibroExcel.Text = Me.OpenFileDialog1.FileName
                    Me.ContarHojasExcel(Me.TextBoxRutaLibroExcel.Text)
                End If
            End If



        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try
            If Me.TextBoxRutaLibroExcel.TextLength > 0 And Me.TextBoxHojaExcel.TextLength > 0 Then

                ' Pide Formato
                Dim F As New FormIntegraNominaA3Formato
                TIPONOMINA = ""
                PASOSTRING = ""
                PASOSTRING2 = ""
                PASOSTRING3 = ""
                F.ShowDialog()
                If TIPONOMINA = "" Then
                    Exit Sub
                Else
                    Me.DataGridAsientos.CaptionText += " " & TIPONOMINA
                    Me.CtaBanco = PASOSTRING
                    Me.RutaExcelNifs = PASOSTRING2
                    Me.TextBoxHojaNif.Text = PASOSTRING3
                End If



                Me.TextBoxDebug.Clear()
                Me.ListBox1.Items.Clear()
                Me.TextBoxDebug.Update()
                Me.ListBox1.Update()

                Me.LimpiaGridAsientos()


                Me.AbrirConexiones()
                Me.BorrarAsientos()
                Me.Cursor = Cursors.AppStarting


                If TIPONOMINA = "NOMINA" Then
                    Me.LeeExcelNomina(Me.TextBoxRutaLibroExcel.Text)
                    Me.GeneraAsientoNomina()

                Else
                    Me.ContarHojasExcel(Me.RutaExcelNifs)
                    Me.LeeExcelNominaNIFS(Me.RutaExcelNifs)

                    Me.ContarHojasExcel(Me.TextBoxRutaLibroExcel.Text)
                    Me.LeeExcelNominaRemesa(Me.TextBoxRutaLibroExcel.Text)
                    Me.GeneraAsientoRemesa()
                End If



                Me.MostrarAsientos()
                If Me.m_ValidaSpyro = True Then
                    Me.SpyroCompruebaCuentas()
                End If


                If Me.CheckBoxAjustar.Checked Then
                    Me.AuditaAsientosCuadre(True)
                    Me.MostrarAsientos()
                    Me.AuditaAsientosCuadre(False)
                    Me.SpyroCompruebaCuenta(Me.m_CtaAjusteRedondeo, "", 9, 0, "", "Ajuste de Redondeo", "")

                Else
                    Me.AuditaAsientosCuadre(False)
                End If


                If Me.m_HayErrores = True Then
                    Me.TextBoxErrores.ForeColor = Color.White
                    Me.TextBoxErrores.BackColor = Color.Maroon
                    Me.TextBoxErrores.Update()
                Else
                    Me.TextBoxErrores.ForeColor = Color.White
                    Me.TextBoxErrores.BackColor = Color.Green
                    Me.TextBoxErrores.Update()
                End If


                If Me.m_HayErrores = True Or Me.m_TotalDiferencia <> 0 Then
                    MsgBox("Existen Errores ", MsgBoxStyle.Information, "Atención Revisar")
                End If

                Me.CrearFichero()
                If FilegrabaStatus = True Then
                    Me.GeneraFichero()
                End If

            Else
                MsgBox("No hay Libro/Hoja de EXCEL seleccionado", MsgBoxStyle.Information, "Atención")
            End If
        Catch ex As Exception

        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click

    End Sub

    Private Sub FormIntegraNominaA3_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try

            If IsNothing(Me.ExcelApp) = False Then
                GC.Collect()
                GC.WaitForPendingFinalizers()
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(Me.ExcelSheet)
                Me.ExcelBook.Close(False, Type.Missing, Type.Missing)
                'Me.ExcelBook.Close(Microsoft.Office.Interop.Excel.XlSaveAction.xlDoNotSaveChanges, Type.Missing, Type.Missing)

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(Me.ExcelBook)
                Me.ExcelApp.Quit()
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(Me.ExcelApp)

                GC.Collect()

            End If


        Catch ex As Exception

        End Try

        Try
            Me.CerrarConexion()
        Catch ex As Exception

        End Try


    End Sub

    Private Sub FormIntegraNominaA3_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Try
                ' imponer  ultura
                '   System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("es-ES", False)
                ' System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = "."
                '  System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = ","

                '  Me.ToolStripStatusCultura.Text = System.Threading.Thread.CurrentThread.CurrentCulture.DisplayName & " Sep Decimal " & System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator & " [Cultura Impuesta]"


                '  Threading.Thread.CurrentThread.CurrentCulture = New Globalization.CultureInfo("en-US", False)
                ' Dim nfi As Globalization.NumberFormatInfo = Threading.Thread.CurrentThread.CurrentCulture.NumberFormat
                'MsgBox(nfi.NumberDecimalSeparator)


            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            Me.MinimumSize = New System.Drawing.Size(784, 500)
            '---------------------------------------------------------------------------------------------------
            ' Conecta con la Base de Datos "central"
            '----------------------------------------------------------------------------------------------------
            Me.DateTimePicker1.Value = (DateAdd(DateInterval.Day, -1, CType(Format(Now, "dd/MM/yyyy"), Date)))
            Me.DbCentral = New C_DATOS.C_DatosOledb(MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"))
            Me.DbCentral.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT='DD/MM/YYYY'")



            '---------------------------------------------------------------------------------------------------
            ' Lee y muestra algunos parametros de Integracion
            '----------------------------------------------------------------------------------------------------
            Me.m_EmpGrupoCod = MyIni.IniGet(Application.StartupPath & "\menu.ini", "PARAMETER", "PARA_EMPGRUPO_COD")


            If Me.m_EmpGrupoCod = "" Then
                MsgBox("No se ha podido leer Grupo de Empresas en Fichero Ini", MsgBoxStyle.Information, "Atención")
            End If

            '---------------------------------------------------------------------------------------------------
            ' Muestra/Pide Hotel a Integrar  
            '----------------------------------------------------------------------------------------------------


            SQL = "SELECT HOTEL_EMPGRUPO_COD,HOTEL_EMP_COD,HOTEL_DESCRIPCION,HOTEL_EMP_NUM,NVL(PARA_FILE_ORIGEN,'?')AS PARA_FILE_ORIGEN,"
            SQL += " NVL(PARA_FILE_DESTINO,'?') AS PARA_FILE_DESTINO,NVL(PARA_CFATODIARI_COD,'?') AS PARA_CFATODIARI_COD "
            SQL += " FROM TH_HOTEL,TN_PARA "
            SQL += " WHERE HOTEL_EMPGRUPO_COD = '" & Me.m_EmpGrupoCod & "'"

            SQL += " AND TH_HOTEL.HOTEL_EMPGRUPO_COD = TN_PARA.PARA_EMPGRUPO_COD "
            SQL += " AND TH_HOTEL.HOTEL_EMP_COD = TN_PARA.PARA_EMP_COD "
            SQL += " AND TH_HOTEL.HOTEL_EMP_NUM = TN_PARA.PARA_EMP_NUM "

            SQL += " AND TH_HOTEL.HOTEL_INT_NOMI = 1 "

            SQL += " ORDER BY HOTEL_DESCRIPCION ASC"

            Me.DataGridHoteles.DataSource = Me.DbCentral.TraerDataset(SQL, "HOTELES")
            Me.DataGridHoteles.DataMember = "HOTELES"

            Me.ConfGriHoteles()

            If Me.DbCentral.mDbDataset.Tables("HOTELES").Rows.Count > 0 Then
                Me.HayRegistros = True
                Me.DataGridAsientos.CaptionText = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2)
                Me.DataGridHoteles.Select(0)

                Me.m_EmpGrupoCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0)
                Me.m_EmpCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1)
                Me.m_EmpNum = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3)

                Me.LeeParametros()


            Else
                Me.HayRegistros = False

            End If



        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Exclamation, "Load")
        End Try

    End Sub


    Private Sub DataGridHoteles_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridHoteles.CurrentCellChanged
        Try
            If Me.HayRegistros = True Then
                Me.DataGridAsientos.CaptionText = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2)
                Me.m_EmpGrupoCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 0)
                Me.m_EmpCod = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 1)
                Me.m_EmpNum = Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 3)

                Me.LeeParametros()
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click
        Try

            Me.Cursor = Cursors.WaitCursor

            REPORT_SELECTION_FORMULA = "{TN_ASNT.ASNT_F_ATOCAB}=DATETIME(" & Format(Me.m_Fecha, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += " AND {TN_ASNT.ASNT_EMPGRUPO_COD}= '" & Me.m_EmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TN_ASNT.ASNT_EMP_COD}= '" & Me.m_EmpCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TN_ASNT.ASNT_EMP_NUM}= " & Me.m_EmpNum
            REPORT_SELECTION_FORMULA += " AND {TN_ASNT.ASNT_TIPO_REGISTRO}= 'AC'"


            Dim Form As New FormVisorCrystal("ASIENTO NOMINA.rpt", "NOMINA  " & Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING"), "", False, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker1.ValueChanged
        Try
            Me.m_Fecha = Me.DateTimePicker1.Value
        Catch ex As Exception

        End Try
    End Sub

#End Region


    Private Sub ButtonImprimirErrores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimirErrores.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            REPORT_SELECTION_FORMULA = "{TH_INCI.INCI_DATR}=DATETIME(" & Format(Me.DateTimePicker1.Value, REPORT_DATE_FORMAT) & ")"
            REPORT_SELECTION_FORMULA += " AND {TH_INCI.INCI_EMPGRUPO_COD}= '" & Me.m_EmpGrupoCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TH_INCI.INCI_EMP_COD}= '" & Me.m_EmpCod & "'"
            REPORT_SELECTION_FORMULA += " AND {TH_INCI.INCI_EMP_NUM}= " & Me.m_EmpNum

            Dim Form As New FormVisorCrystal("TH_INCI.RPT", CType(Me.DataGridHoteles.Item(Me.DataGridHoteles.CurrentRowIndex, 2), String), REPORT_SELECTION_FORMULA, MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"), "", False, False)

            Form.MdiParent = Me.MdiParent

            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")

        End Try
    End Sub

    Private Sub ButtonExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExcel.Click
        Try
            If Me.TextBoxRutaLibroExcel.Text.Length > 0 Then
                m_File = New FileInfo(Me.TextBoxRutaLibroExcel.Text)
                If m_File.Extension <> ".xls" And m_File.Extension <> ".xlsx" Then
                    MsgBox("No es un Libro Excel = " & m_File.Extension, MsgBoxStyle.Information, "Atención")
                    Exit Sub
                Else
                    Me.ContarHojasExcel(Me.TextBoxRutaLibroExcel.Text)
                End If
            End If


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ListBoxLibros_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBoxLibros.SelectedIndexChanged
        Try
            Me.TextBoxHojaExcel.Text = Me.ListBoxLibros.SelectedItem
            Me.TextBoxHojaExcel.Update()
        Catch ex As Exception

        End Try
    End Sub

End Class