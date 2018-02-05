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
Namespace WebReferenceTitoAlbaranes
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="GestionNewHotel_Binding", [Namespace]:="urn:microsoft-dynamics-schemas/codeunit/GestionNewHotel")>  _
    Partial Public Class GestionNewHotel
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private CrearAlbCompraOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.HTitoNewHotelEnviar.My.MySettings.Default.HTitoNewHotelEnviar_WebReferenceTitoAlbaranes_GestionNewHotel
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
        Public Event CrearAlbCompraCompleted As CrearAlbCompraCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:microsoft-dynamics-schemas/codeunit/GestionNewHotel:CrearAlbCompra", RequestNamespace:="urn:microsoft-dynamics-schemas/codeunit/GestionNewHotel", ResponseElementName:="CrearAlbCompra_Result", ResponseNamespace:="urn:microsoft-dynamics-schemas/codeunit/GestionNewHotel", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Sub CrearAlbCompra( _
                    ByVal documentoNewHotel As String,  _
                    ByVal codProveedor As String,  _
                    ByVal numAlbProveedor As String,  _
                    <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> ByVal fechaRegistro As Date,  _
                    <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> ByVal fechaDoc As Date,  _
                    ByVal numProducto As String,  _
                    ByVal descripcion As String,  _
                    ByVal codAlmacen As String,  _
                    ByVal cantidad As Decimal,  _
                    ByVal codUdMedida As String,  _
                    ByVal costeUnit As Decimal,  _
                    ByVal globalDim1 As String,  _
                    ByVal globalDim2 As String,  _
                    ByVal shortcutDim3 As String,  _
                    ByVal numDocExterno As String,  _
                    ByVal grupoIvaNeg As String,  _
                    ByVal grupoIvaProd As String,  _
                    ByVal grupoContableNeg As String,  _
                    ByVal grupoContableProd As String)
            Me.Invoke("CrearAlbCompra", New Object() {documentoNewHotel, codProveedor, numAlbProveedor, fechaRegistro, fechaDoc, numProducto, descripcion, codAlmacen, cantidad, codUdMedida, costeUnit, globalDim1, globalDim2, shortcutDim3, numDocExterno, grupoIvaNeg, grupoIvaProd, grupoContableNeg, grupoContableProd})
        End Sub
        
        '''<remarks/>
        Public Overloads Sub CrearAlbCompraAsync( _
                    ByVal documentoNewHotel As String,  _
                    ByVal codProveedor As String,  _
                    ByVal numAlbProveedor As String,  _
                    ByVal fechaRegistro As Date,  _
                    ByVal fechaDoc As Date,  _
                    ByVal numProducto As String,  _
                    ByVal descripcion As String,  _
                    ByVal codAlmacen As String,  _
                    ByVal cantidad As Decimal,  _
                    ByVal codUdMedida As String,  _
                    ByVal costeUnit As Decimal,  _
                    ByVal globalDim1 As String,  _
                    ByVal globalDim2 As String,  _
                    ByVal shortcutDim3 As String,  _
                    ByVal numDocExterno As String,  _
                    ByVal grupoIvaNeg As String,  _
                    ByVal grupoIvaProd As String,  _
                    ByVal grupoContableNeg As String,  _
                    ByVal grupoContableProd As String)
            Me.CrearAlbCompraAsync(documentoNewHotel, codProveedor, numAlbProveedor, fechaRegistro, fechaDoc, numProducto, descripcion, codAlmacen, cantidad, codUdMedida, costeUnit, globalDim1, globalDim2, shortcutDim3, numDocExterno, grupoIvaNeg, grupoIvaProd, grupoContableNeg, grupoContableProd, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub CrearAlbCompraAsync( _
                    ByVal documentoNewHotel As String,  _
                    ByVal codProveedor As String,  _
                    ByVal numAlbProveedor As String,  _
                    ByVal fechaRegistro As Date,  _
                    ByVal fechaDoc As Date,  _
                    ByVal numProducto As String,  _
                    ByVal descripcion As String,  _
                    ByVal codAlmacen As String,  _
                    ByVal cantidad As Decimal,  _
                    ByVal codUdMedida As String,  _
                    ByVal costeUnit As Decimal,  _
                    ByVal globalDim1 As String,  _
                    ByVal globalDim2 As String,  _
                    ByVal shortcutDim3 As String,  _
                    ByVal numDocExterno As String,  _
                    ByVal grupoIvaNeg As String,  _
                    ByVal grupoIvaProd As String,  _
                    ByVal grupoContableNeg As String,  _
                    ByVal grupoContableProd As String,  _
                    ByVal userState As Object)
            If (Me.CrearAlbCompraOperationCompleted Is Nothing) Then
                Me.CrearAlbCompraOperationCompleted = AddressOf Me.OnCrearAlbCompraOperationCompleted
            End If
            Me.InvokeAsync("CrearAlbCompra", New Object() {documentoNewHotel, codProveedor, numAlbProveedor, fechaRegistro, fechaDoc, numProducto, descripcion, codAlmacen, cantidad, codUdMedida, costeUnit, globalDim1, globalDim2, shortcutDim3, numDocExterno, grupoIvaNeg, grupoIvaProd, grupoContableNeg, grupoContableProd}, Me.CrearAlbCompraOperationCompleted, userState)
        End Sub
        
        Private Sub OnCrearAlbCompraOperationCompleted(ByVal arg As Object)
            If (Not (Me.CrearAlbCompraCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent CrearAlbCompraCompleted(Me, New System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
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
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")>  _
    Public Delegate Sub CrearAlbCompraCompletedEventHandler(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
End Namespace
