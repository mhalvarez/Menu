﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Este código fue generado por una herramienta.
'     Versión de runtime:4.0.30319.42000
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace My
    
    <Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0"),  _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Partial Friend NotInheritable Class MySettings
        Inherits Global.System.Configuration.ApplicationSettingsBase
        
        Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()),MySettings)
        
#Region "Funcionalidad para autoguardar de My.Settings"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(ByVal sender As Global.System.Object, ByVal e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region
        
        Public Shared ReadOnly Property [Default]() As MySettings
            Get
                
#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
                Return defaultInstance
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.WebServiceUrl),  _
         Global.System.Configuration.DefaultSettingValueAttribute("http://srvwspruebas/DynamicsWebService/SAT_NHInfoAnalyticalQueryService.asmx")>  _
        Public ReadOnly Property Menu_WebReferenceAnalitica2_SAT_NHInfoAnalyticalQueryService() As String
            Get
                Return CType(Me("Menu_WebReferenceAnalitica2_SAT_NHInfoAnalyticalQueryService"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.WebServiceUrl),  _
         Global.System.Configuration.DefaultSettingValueAttribute("http://srvwspruebas/DynamicsWebService/SAT_NHPrePaymentService.asmx")>  _
        Public ReadOnly Property Menu_WebReferenceAnticipos2_SAT_NHPrePaymentService() As String
            Get
                Return CType(Me("Menu_WebReferenceAnticipos2_SAT_NHPrePaymentService"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.WebServiceUrl),  _
         Global.System.Configuration.DefaultSettingValueAttribute("http://srvwspruebas/DynamicsWebService/SAT_NHCreateCustQueryService.asmx")>  _
        Public ReadOnly Property Menu_WebReferenceClientes2_SAT_NHCreateCustQueryService() As String
            Get
                Return CType(Me("Menu_WebReferenceClientes2_SAT_NHCreateCustQueryService"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.WebServiceUrl),  _
         Global.System.Configuration.DefaultSettingValueAttribute("http://srvwspruebas/DynamicsWebService/SAT_NHJournalCustPaymentQueryService.asmx")>  _
        Public ReadOnly Property Menu_WebReferenceCobros2_SAT_NHJournalCustPaymentQueryService() As String
            Get
                Return CType(Me("Menu_WebReferenceCobros2_SAT_NHJournalCustPaymentQueryService"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.WebServiceUrl),  _
         Global.System.Configuration.DefaultSettingValueAttribute("http://192.168.0.11:8080/DynamicsWebService/SAT_NHVendPackingSlipJourService.asmx"& _ 
            "")>  _
        Public ReadOnly Property Menu_WebReferenceCompras2_SAT_NHVendPackingSlipJourService() As String
            Get
                Return CType(Me("Menu_WebReferenceCompras2_SAT_NHVendPackingSlipJourService"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.WebServiceUrl),  _
         Global.System.Configuration.DefaultSettingValueAttribute("http://srvwspruebas/DynamicsWebService/SAT_NHCreateSalesOrdersQueryService.asmx")>  _
        Public ReadOnly Property Menu_WebReferenceFacturas2_SAT_NHCreateSalesOrdersQueryService() As String
            Get
                Return CType(Me("Menu_WebReferenceFacturas2_SAT_NHCreateSalesOrdersQueryService"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.WebServiceUrl),  _
         Global.System.Configuration.DefaultSettingValueAttribute("http://srvwspruebas/DynamicsWebService/SAT_NHCreateProdOrdersQueryService.asmx")>  _
        Public ReadOnly Property Menu_WebReferenceProduccion2_SAT_NHCreateProdOrdersQueryService() As String
            Get
                Return CType(Me("Menu_WebReferenceProduccion2_SAT_NHCreateProdOrdersQueryService"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.WebServiceUrl),  _
         Global.System.Configuration.DefaultSettingValueAttribute("http://srvwspruebas/DynamicsWebService/SAT_JournalLossProfitQueryService.ASMX")>  _
        Public ReadOnly Property Menu_WebReferenceVentaTikets2_SAT_JournalLossProfitQueryService() As String
            Get
                Return CType(Me("Menu_WebReferenceVentaTikets2_SAT_JournalLossProfitQueryService"),String)
            End Get
        End Property
    End Class
End Namespace

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.Menu.My.MySettings
            Get
                Return Global.Menu.My.MySettings.Default
            End Get
        End Property
    End Module
End Namespace
