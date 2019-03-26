'
'
' cambie las referencias de las librerias de crystal de la version 10 a la 13 por no imprimir
' en el hotel ipanema !!!
'
'


Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class FormVisorCrystal
    Inherits System.Windows.Forms.Form
    Private mReportName As String
    Private mSystitulo As String
    Private mSelectionFormula As String
    Private mvStrConexionCentral As String
    Private mvStrConexionHotel As String
    Private mvConectarNewhotel As Boolean
    Private Reporte As ReportDocument

    Private mFecha As Date

    Dim MyConexion As ConnectionInfo


    Private CrDatabase As CrystalDecisions.CrystalReports.Engine.Database


    Private CrTablas As CrystalDecisions.CrystalReports.Engine.Tables
    Private CrTabla As CrystalDecisions.CrystalReports.Engine.Table
    Private CrTablaLogInfo As CrystalDecisions.Shared.TableLogOnInfo
    Friend WithEvents CrystalReportViewer1 As CrystalDecisions.Windows.Forms.CrystalReportViewer

    ' paso de parametros al report
    Private vParFecha1 As String


#Region " Código generado por el Diseñador de Windows Forms "

    Public Sub New()
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()

    End Sub
    Public Sub New(ByVal vReportName As String, ByVal vSysTitulo As String, ByVal vSelectionFormula As String, ByVal vStrConexionCentral As String, ByVal vStrConexionHotel As String, ByVal vConectarNewHotel As Boolean, ByVal vConectarNewGolf As Boolean)
        MyBase.New()
        InitializeComponent()
        If IsDBNull(vReportName) = True Then

            Throw New ArgumentException(
                        "El nombre del Report no puede ser una cadena vacía")
        Else
            Me.mReportName = vReportName
            Me.mSystitulo = vSysTitulo
            Me.mSelectionFormula = vSelectionFormula
            Me.mvStrConexionCentral = vStrConexionCentral
            Me.mvStrConexionHotel = vStrConexionHotel
            Me.mvConectarNewhotel = vConectarNewHotel
        End If


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

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.CrystalReportViewer1 = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.SuspendLayout()
        '
        'CrystalReportViewer1
        '
        Me.CrystalReportViewer1.ActiveViewIndex = -1
        Me.CrystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CrystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default
        Me.CrystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CrystalReportViewer1.Location = New System.Drawing.Point(0, 0)
        Me.CrystalReportViewer1.Name = "CrystalReportViewer1"
        Me.CrystalReportViewer1.Size = New System.Drawing.Size(840, 301)
        Me.CrystalReportViewer1.TabIndex = 0
        '
        'FormVisorCrystal
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(840, 301)
        Me.Controls.Add(Me.CrystalReportViewer1)
        Me.Name = "FormVisorCrystal"
        Me.Text = "Visor de Informes"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub FormVisorCrystal_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.ConfigureReports()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ConfigureReports()
        Try

            Dim ReportName As String = REPORT_PATH & mReportName

            Me.MyConexion = New ConnectionInfo
            Me.Reporte = New ReportDocument
            Me.Reporte.Load(ReportName)



            ' GESTION DE FORMULAS 
            Dim Formula As CrystalDecisions.CrystalReports.Engine.FormulaFieldDefinition


            For Each Formula In Me.Reporte.DataDefinition.FormulaFields

                If Formula.Name = "SysTitulo" Then
                    Formula.Text = "'" & Me.mSystitulo & "'"
                End If

            Next



            If Me.mvConectarNewhotel = False Then
                With MyConexion
                    .ServerName = StrConexionExtraeDataSource(mvStrConexionCentral)
                    .DatabaseName = ""
                    .UserID = StrConexionExtraeUsuario(mvStrConexionCentral)
                    .Password = StrConexionExtraePassword(mvStrConexionCentral)
                End With
            Else
                With MyConexion
                    .ServerName = StrConexionExtraeDataSource(mvStrConexionHotel)
                    .DatabaseName = ""
                    .UserID = StrConexionExtraeUsuario(mvStrConexionHotel)
                    .Password = StrConexionExtraePassword(mvStrConexionHotel)
                End With
            End If


            'Paso Formula
            Me.CrystalReportViewer1.SelectionFormula = Me.mSelectionFormula




            Me.CrystalReportViewer1.ReportSource = Me.Reporte
            Me.LogOnReport(MyConexion)
            Me.CrystalReportViewer1.Zoom(120)

        Catch EX As Exception
            MsgBox(EX.Message)

        End Try

    End Sub
    Private Sub LogOnReport(ByVal vConnectionInfo As ConnectionInfo)
        Try

            Me.CrDatabase = Me.Reporte.Database

            Me.CrTablas = Me.CrDatabase.Tables



            For Each Me.CrTabla In Me.CrTablas
                Me.CrTablaLogInfo = Me.CrTabla.LogOnInfo
                Me.CrTablaLogInfo.ConnectionInfo = vConnectionInfo
                Me.CrTabla.ApplyLogOnInfo(Me.CrTablaLogInfo)

                ' PARA REPORTS CON TABLAS DE UNO Y OTRO ESQUEMA ( REVISAR Y COGER EL NOMBRE DE USUARIO POR PARAMETRO
                If Mid(Me.CrTabla.Location, 1, 4) = "TNPL" Then
                    Me.CrTabla.Location = "GMS" & "." & Me.CrTabla.Location
                Else
                    Me.CrTabla.Location = vConnectionInfo.UserID & "." & Me.CrTabla.Location
                End If

            Next


            'PARA LOS SUBINFORMES DEL INFORME
            'Loop through each section and find all the report objects
            'Loop through all the report objects to find all subreport objects, then set the
            'logoninfo to the subreport


            Dim subRepDoc As New ReportDocument()

            Dim subcrSections As Sections
            Dim subcrSection As Section
            Dim subcrReportObjects As ReportObjects
            Dim subcrReportObject As ReportObject
            Dim subcrSubreportObject As SubreportObject
            Dim subcrDatabase As Database = Me.Reporte.Database
            Dim subcrTables As Tables = Me.CrDatabase.Tables
            Dim subcrTable As Table
            Dim subcrLogOnInfo As TableLogOnInfo
            Dim subcrConnInfo As New ConnectionInfo()


            subcrSections = Me.Reporte.ReportDefinition.Sections
            For Each subcrSection In subcrSections
                subcrReportObjects = subcrSection.ReportObjects
                For Each subcrReportObject In subcrReportObjects
                    If subcrReportObject.Kind = ReportObjectKind.SubreportObject Then



                        'If you find a subreport, typecast the reportobject to a subreport object
                        subcrSubreportObject = CType(subcrReportObject, SubreportObject)

                        'Open the subreport
                        subRepDoc = subcrSubreportObject.OpenSubreport(subcrSubreportObject.SubreportName)

                        ' paso de la formla de selecion al subreport
                        subRepDoc.RecordSelectionFormula = Me.mSelectionFormula

                        CrDatabase = subRepDoc.Database
                        subcrTables = CrDatabase.Tables

                        'Loop through each table and set the connection info
                        'Pass the connection info to the logoninfo object then apply the
                        'logoninfo to the subreport

                        For Each subcrTable In subcrTables
                            With subcrConnInfo
                                .ServerName = vConnectionInfo.ServerName
                                .DatabaseName = vConnectionInfo.DatabaseName
                                .UserID = vConnectionInfo.UserID
                                .Password = vConnectionInfo.Password
                            End With
                            subcrLogOnInfo = subcrTable.LogOnInfo
                            subcrLogOnInfo.ConnectionInfo = subcrConnInfo
                            subcrTable.ApplyLogOnInfo(subcrLogOnInfo)


                            ' PARA REPORTS CON TABLAS DE UNO Y OTRO ESQUEMA ( REVISAR Y COGER EL NOMBRE DE USUARIO POR PARAMETRO
                            If Mid(subcrTable.Location, 1, 4) = "TNPL" Then
                                subcrTable.Location = "GMS" & "." & subcrTable.Location
                            Else
                                subcrTable.Location = vConnectionInfo.UserID & "." & subcrTable.Location
                            End If
                        Next


                    End If
                Next
            Next

           

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


End Class
