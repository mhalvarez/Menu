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
Namespace WebReferenceCobros
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="SAT_NHJournalCustPaymentQueryServiceSoap", [Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHJournalCustPaymentQ"& _ 
        "uery")>  _
    Partial Public Class SAT_NHJournalCustPaymentQueryService
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private createListSAT_NHJournalCustPaymentQueryOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.Menu.My.MySettings.Default.Menu_WebReferenceCobros2_SAT_NHJournalCustPaymentQueryService
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
        Public Event createListSAT_NHJournalCustPaymentQueryCompleted As createListSAT_NHJournalCustPaymentQueryCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHJournalCustPaymentQ"& _ 
            "uery/createListSAT_NHJournalCustPaymentQuery", RequestNamespace:="http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHJournalCustPaymentQ"& _ 
            "uery", ResponseNamespace:="http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHJournalCustPaymentQ"& _ 
            "uery", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function createListSAT_NHJournalCustPaymentQuery(<System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)> ByVal DocumentContext As DocumentContext, ByVal SAT_NHJournalCustPaymentQuery As AxdSAT_NHJournalCustPaymentQuery) As <System.Xml.Serialization.XmlArrayAttribute("EntityKeyList", [Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/EntityKeyList"), System.Xml.Serialization.XmlArrayItemAttribute([Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/EntityKey", IsNullable:=false)> EntityKey()
            Dim results() As Object = Me.Invoke("createListSAT_NHJournalCustPaymentQuery", New Object() {DocumentContext, SAT_NHJournalCustPaymentQuery})
            Return CType(results(0),EntityKey())
        End Function
        
        '''<remarks/>
        Public Overloads Sub createListSAT_NHJournalCustPaymentQueryAsync(ByVal DocumentContext As DocumentContext, ByVal SAT_NHJournalCustPaymentQuery As AxdSAT_NHJournalCustPaymentQuery)
            Me.createListSAT_NHJournalCustPaymentQueryAsync(DocumentContext, SAT_NHJournalCustPaymentQuery, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub createListSAT_NHJournalCustPaymentQueryAsync(ByVal DocumentContext As DocumentContext, ByVal SAT_NHJournalCustPaymentQuery As AxdSAT_NHJournalCustPaymentQuery, ByVal userState As Object)
            If (Me.createListSAT_NHJournalCustPaymentQueryOperationCompleted Is Nothing) Then
                Me.createListSAT_NHJournalCustPaymentQueryOperationCompleted = AddressOf Me.OncreateListSAT_NHJournalCustPaymentQueryOperationCompleted
            End If
            Me.InvokeAsync("createListSAT_NHJournalCustPaymentQuery", New Object() {DocumentContext, SAT_NHJournalCustPaymentQuery}, Me.createListSAT_NHJournalCustPaymentQueryOperationCompleted, userState)
        End Sub
        
        Private Sub OncreateListSAT_NHJournalCustPaymentQueryOperationCompleted(ByVal arg As Object)
            If (Not (Me.createListSAT_NHJournalCustPaymentQueryCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent createListSAT_NHJournalCustPaymentQueryCompleted(Me, New createListSAT_NHJournalCustPaymentQueryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
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
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHJournalCustPaymentQ"& _ 
        "uery")>  _
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
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHJournalCustPaymentQ"& _ 
        "uery")>  _
    Partial Public Class AxdEntity_SAT_NHJournalCustPayment_1
        
        Private amountField As System.Nullable(Of Decimal)
        
        Private amountFieldSpecified As Boolean
        
        Private bookIDField As String
        
        Private departamentField As String
        
        Private hotelIdField As String
        
        Private invoiceIdField As String
        
        Private paymModeField As String
        
        Private prePaymentField As System.Nullable(Of AxdExtType_NoYesId)
        
        Private prePaymentFieldSpecified As Boolean
        
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
        Public Property Amount() As System.Nullable(Of Decimal)
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property AmountSpecified() As Boolean
            Get
                Return Me.amountFieldSpecified
            End Get
            Set
                Me.amountFieldSpecified = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)>  _
        Public Property BookID() As String
            Get
                Return Me.bookIDField
            End Get
            Set
                Me.bookIDField = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)>  _
        Public Property Departament() As String
            Get
                Return Me.departamentField
            End Get
            Set
                Me.departamentField = value
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
        Public Property InvoiceId() As String
            Get
                Return Me.invoiceIdField
            End Get
            Set
                Me.invoiceIdField = value
            End Set
        End Property
        
        '''<comentarios/>
        Public Property PaymMode() As String
            Get
                Return Me.paymModeField
            End Get
            Set
                Me.paymModeField = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)>  _
        Public Property PrePayment() As System.Nullable(Of AxdExtType_NoYesId)
            Get
                Return Me.prePaymentField
            End Get
            Set
                Me.prePaymentField = value
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property PrePaymentSpecified() As Boolean
            Get
                Return Me.prePaymentFieldSpecified
            End Get
            Set
                Me.prePaymentFieldSpecified = value
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
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHJournalCustPaymentQ"& _ 
        "uery")>  _
    Public Enum AxdExtType_NoYesId
        
        '''<comentarios/>
        No
        
        '''<comentarios/>
        Yes
    End Enum
    
    '''<comentarios/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHJournalCustPaymentQ"& _ 
        "uery")>  _
    Partial Public Class AxdSAT_NHJournalCustPaymentQuery
        
        Private docPurposeField As System.Nullable(Of AxdEnum_XMLDocPurpose)
        
        Private docPurposeFieldSpecified As Boolean
        
        Private senderIdField As String
        
        Private sAT_NHJournalCustPayment_1Field() As AxdEntity_SAT_NHJournalCustPayment_1
        
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
        <System.Xml.Serialization.XmlElementAttribute("SAT_NHJournalCustPayment_1")>  _
        Public Property SAT_NHJournalCustPayment_1() As AxdEntity_SAT_NHJournalCustPayment_1()
            Get
                Return Me.sAT_NHJournalCustPayment_1Field
            End Get
            Set
                Me.sAT_NHJournalCustPayment_1Field = value
            End Set
        End Property
    End Class
    
    '''<comentarios/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2"),  _
     System.SerializableAttribute(),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://schemas.microsoft.com/dynamics/2006/02/documents/SAT_NHJournalCustPaymentQ"& _ 
        "uery")>  _
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
    Public Delegate Sub createListSAT_NHJournalCustPaymentQueryCompletedEventHandler(ByVal sender As Object, ByVal e As createListSAT_NHJournalCustPaymentQueryCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class createListSAT_NHJournalCustPaymentQueryCompletedEventArgs
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
