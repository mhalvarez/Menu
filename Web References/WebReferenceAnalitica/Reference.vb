﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Este código fue generado por una herramienta.
'     Versión de runtime:4.0.30319.42000
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
'
Namespace WebReferenceAnalitica
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="SAT_NHInfoAnalyticalQueryServiceSoap", [Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHInfoAnalyticalQuery"& _ 
        "")>  _
    Partial Public Class SAT_NHInfoAnalyticalQueryService
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private createListSAT_NHInfoAnalyticalQueryOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.Menu.My.MySettings.Default.Menu_WebReferenceAnalitica2_SAT_NHInfoAnalyticalQueryService
            If (Me.IsLocalFileSystemWebService(Me.Url) = true) Then
                Me.UseDefaultCredentials = true
                Me.useDefaultCredentialsSetExplicitly = false
            Else
                Me.useDefaultCredentialsSetExplicitly = true
            End If
        End Sub
        
        Public Shadows Property Url() As String
            Get
                Return MyBase.Url
            End Get
            Set
                If (((Me.IsLocalFileSystemWebService(MyBase.Url) = true)  _
                            AndAlso (Me.useDefaultCredentialsSetExplicitly = false))  _
                            AndAlso (Me.IsLocalFileSystemWebService(value) = false)) Then
                    MyBase.UseDefaultCredentials = false
                End If
                MyBase.Url = value
            End Set
        End Property
        
        Public Shadows Property UseDefaultCredentials() As Boolean
            Get
                Return MyBase.UseDefaultCredentials
            End Get
            Set
                MyBase.UseDefaultCredentials = value
                Me.useDefaultCredentialsSetExplicitly = true
            End Set
        End Property
        
        '''<remarks/>
        Public Event createListSAT_NHInfoAnalyticalQueryCompleted As createListSAT_NHInfoAnalyticalQueryCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHInfoAnalyticalQuery"& _ 
            "/createListSAT_NHInfoAnalyticalQuery", RequestNamespace:="http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHInfoAnalyticalQuery"& _ 
            "", ResponseNamespace:="http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHInfoAnalyticalQuery"& _ 
            "", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function createListSAT_NHInfoAnalyticalQuery(<System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)> ByVal DocumentContext As DocumentContext, ByVal SAT_NHInfoAnalyticalQuery As AxdSAT_NHInfoAnalyticalQuery) As <System.Xml.Serialization.XmlArrayAttribute("EntityKeyList", [Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/EntityKeyList"), System.Xml.Serialization.XmlArrayItemAttribute([Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/EntityKey", IsNullable:=false)> EntityKey()
            Dim results() As Object = Me.Invoke("createListSAT_NHInfoAnalyticalQuery", New Object() {DocumentContext, SAT_NHInfoAnalyticalQuery})
            Return CType(results(0),EntityKey())
        End Function
        
        '''<remarks/>
        Public Overloads Sub createListSAT_NHInfoAnalyticalQueryAsync(ByVal DocumentContext As DocumentContext, ByVal SAT_NHInfoAnalyticalQuery As AxdSAT_NHInfoAnalyticalQuery)
            Me.createListSAT_NHInfoAnalyticalQueryAsync(DocumentContext, SAT_NHInfoAnalyticalQuery, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub createListSAT_NHInfoAnalyticalQueryAsync(ByVal DocumentContext As DocumentContext, ByVal SAT_NHInfoAnalyticalQuery As AxdSAT_NHInfoAnalyticalQuery, ByVal userState As Object)
            If (Me.createListSAT_NHInfoAnalyticalQueryOperationCompleted Is Nothing) Then
                Me.createListSAT_NHInfoAnalyticalQueryOperationCompleted = AddressOf Me.OncreateListSAT_NHInfoAnalyticalQueryOperationCompleted
            End If
            Me.InvokeAsync("createListSAT_NHInfoAnalyticalQuery", New Object() {DocumentContext, SAT_NHInfoAnalyticalQuery}, Me.createListSAT_NHInfoAnalyticalQueryOperationCompleted, userState)
        End Sub
        
        Private Sub OncreateListSAT_NHInfoAnalyticalQueryOperationCompleted(ByVal arg As Object)
            If (Not (Me.createListSAT_NHInfoAnalyticalQueryCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent createListSAT_NHInfoAnalyticalQueryCompleted(Me, New createListSAT_NHInfoAnalyticalQueryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
        
        Private Function IsLocalFileSystemWebService(ByVal url As String) As Boolean
            If ((url Is Nothing)  _
                        OrElse (url Is String.Empty)) Then
                Return false
            End If
            Dim wsUri As System.Uri = New System.Uri(url)
            If ((wsUri.Port >= 1024)  _
                        AndAlso (String.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) = 0)) Then
                Return true
            End If
            Return false
        End Function
    End Class
    
    '''<comentarios/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHInfoAnalyticalQuery"& _ 
        "")>  _
    Partial Public Class DocumentContext
        
        Private messageIdField As String
        
        Private sourceEndpointUserField As String
        
        Private sourceEndpointField As String
        
        Private destinationEndpointField As String
        
        '''<comentarios/>
        Public Property MessageId() As String
            Get
                Return Me.messageIdField
            End Get
            Set
                Me.messageIdField = value
            End Set
        End Property
        
        '''<comentarios/>
        Public Property SourceEndpointUser() As String
            Get
                Return Me.sourceEndpointUserField
            End Get
            Set
                Me.sourceEndpointUserField = value
            End Set
        End Property
        
        '''<comentarios/>
        Public Property SourceEndpoint() As String
            Get
                Return Me.sourceEndpointField
            End Get
            Set
                Me.sourceEndpointField = value
            End Set
        End Property
        
        '''<comentarios/>
        Public Property DestinationEndpoint() As String
            Get
                Return Me.destinationEndpointField
            End Get
            Set
                Me.destinationEndpointField = value
            End Set
        End Property
    End Class
    
    '''<comentarios/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/EntityKey")>  _
    Partial Public Class KeyField
        
        Private fieldField As String
        
        Private valueField As String
        
        '''<comentarios/>
        Public Property Field() As String
            Get
                Return Me.fieldField
            End Get
            Set
                Me.fieldField = value
            End Set
        End Property
        
        '''<comentarios/>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = value
            End Set
        End Property
    End Class
    
    '''<comentarios/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/EntityKey")>  _
    Partial Public Class EntityKey
        
        Private keyDataField() As KeyField
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=false)>  _
        Public Property KeyData() As KeyField()
            Get
                Return Me.keyDataField
            End Get
            Set
                Me.keyDataField = value
            End Set
        End Property
    End Class
    
    '''<comentarios/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHInfoAnalyticalQuery"& _ 
        "")>  _
    Partial Public Class AxdEntity_SAT_NHInfoAnalytical_1
        
        Private analyticalConceptField As System.Nullable(Of AxdEnum_SAT_NHAnalyticalConcept)
        
        Private analyticalConceptFieldSpecified As Boolean
        
        Private hotelIdField As String
        
        Private qtyField As System.Nullable(Of Decimal)
        
        Private qtyFieldSpecified As Boolean
        
        Private qty2Field As System.Nullable(Of Integer)
        
        Private qty2FieldSpecified As Boolean
        
        Private recIdField As System.Nullable(Of Long)
        
        Private recIdFieldSpecified As Boolean
        
        Private recVersionField As System.Nullable(Of Integer)
        
        Private recVersionFieldSpecified As Boolean
        
        Private transDateField As Date
        
        Private classField As String
        
        Public Sub New()
            MyBase.New
            Me.classField = "entity"
        End Sub
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)>  _
        Public Property AnalyticalConcept() As System.Nullable(Of AxdEnum_SAT_NHAnalyticalConcept)
            Get
                Return Me.analyticalConceptField
            End Get
            Set
                Me.analyticalConceptField = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property AnalyticalConceptSpecified() As Boolean
            Get
                Return Me.analyticalConceptFieldSpecified
            End Get
            Set
                Me.analyticalConceptFieldSpecified = value
            End Set
        End Property
        
        '''<comentarios/>
        Public Property HotelId() As String
            Get
                Return Me.hotelIdField
            End Get
            Set
                Me.hotelIdField = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)>  _
        Public Property Qty() As System.Nullable(Of Decimal)
            Get
                Return Me.qtyField
            End Get
            Set
                Me.qtyField = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property QtySpecified() As Boolean
            Get
                Return Me.qtyFieldSpecified
            End Get
            Set
                Me.qtyFieldSpecified = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)>  _
        Public Property Qty2() As System.Nullable(Of Integer)
            Get
                Return Me.qty2Field
            End Get
            Set
                Me.qty2Field = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property Qty2Specified() As Boolean
            Get
                Return Me.qty2FieldSpecified
            End Get
            Set
                Me.qty2FieldSpecified = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)>  _
        Public Property RecId() As System.Nullable(Of Long)
            Get
                Return Me.recIdField
            End Get
            Set
                Me.recIdField = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property RecIdSpecified() As Boolean
            Get
                Return Me.recIdFieldSpecified
            End Get
            Set
                Me.recIdFieldSpecified = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)>  _
        Public Property RecVersion() As System.Nullable(Of Integer)
            Get
                Return Me.recVersionField
            End Get
            Set
                Me.recVersionField = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property RecVersionSpecified() As Boolean
            Get
                Return Me.recVersionFieldSpecified
            End Get
            Set
                Me.recVersionFieldSpecified = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")>  _
        Public Property TransDate() As Date
            Get
                Return Me.transDateField
            End Get
            Set
                Me.transDateField = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property [class]() As String
            Get
                Return Me.classField
            End Get
            Set
                Me.classField = value
            End Set
        End Property
    End Class
    
    '''<comentarios/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2"),  _
     System.SerializableAttribute(),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHInfoAnalyticalQuery"& _ 
        "")>  _
    Public Enum AxdEnum_SAT_NHAnalyticalConcept
        
        '''<comentarios/>
        None
        
        '''<comentarios/>
        NumOverNightStays
        
        '''<comentarios/>
        NumRooms
    End Enum
    
    '''<comentarios/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHInfoAnalyticalQuery"& _ 
        "")>  _
    Partial Public Class AxdSAT_NHInfoAnalyticalQuery
        
        Private docPurposeField As System.Nullable(Of AxdEnum_XMLDocPurpose)
        
        Private docPurposeFieldSpecified As Boolean
        
        Private senderIdField As String
        
        Private sAT_NHInfoAnalytical_1Field() As AxdEntity_SAT_NHInfoAnalytical_1
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)>  _
        Public Property DocPurpose() As System.Nullable(Of AxdEnum_XMLDocPurpose)
            Get
                Return Me.docPurposeField
            End Get
            Set
                Me.docPurposeField = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property DocPurposeSpecified() As Boolean
            Get
                Return Me.docPurposeFieldSpecified
            End Get
            Set
                Me.docPurposeFieldSpecified = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)>  _
        Public Property SenderId() As String
            Get
                Return Me.senderIdField
            End Get
            Set
                Me.senderIdField = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute("SAT_NHInfoAnalytical_1")>  _
        Public Property SAT_NHInfoAnalytical_1() As AxdEntity_SAT_NHInfoAnalytical_1()
            Get
                Return Me.sAT_NHInfoAnalytical_1Field
            End Get
            Set
                Me.sAT_NHInfoAnalytical_1Field = value
            End Set
        End Property
    End Class
    
    '''<comentarios/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2"),  _
     System.SerializableAttribute(),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHInfoAnalyticalQuery"& _ 
        "")>  _
    Public Enum AxdEnum_XMLDocPurpose
        
        '''<comentarios/>
        Original
        
        '''<comentarios/>
        Duplicate
        
        '''<comentarios/>
        Proforma
    End Enum
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")>  _
    Public Delegate Sub createListSAT_NHInfoAnalyticalQueryCompletedEventHandler(ByVal sender As Object, ByVal e As createListSAT_NHInfoAnalyticalQueryCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class createListSAT_NHInfoAnalyticalQueryCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As EntityKey()
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),EntityKey())
            End Get
        End Property
    End Class
End Namespace