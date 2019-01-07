Public Class FormMenu
    Inherits System.Windows.Forms.Form
    Dim MyIni As New cIniArray


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
    Friend WithEvents StatusBarMenu As System.Windows.Forms.StatusBar
    Friend WithEvents ToolBarMenu As System.Windows.Forms.ToolBar
    Friend WithEvents ImageListMenu As System.Windows.Forms.ImageList
    Friend WithEvents ToolBarButtonSalir As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButtonNewPos As System.Windows.Forms.ToolBarButton
    Friend WithEvents MainMenu As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItemArchivo As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemSalir As System.Windows.Forms.MenuItem
    Friend WithEvents StatusBarPanelBd As System.Windows.Forms.StatusBarPanel
    Friend WithEvents StatusBarPanelOtros As System.Windows.Forms.StatusBarPanel
    Friend WithEvents MenuItemIntegracion As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemIntegracionFront As System.Windows.Forms.MenuItem
    Friend WithEvents ToolBarButtonIntegraFront As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButtonIntegraAlmacen As System.Windows.Forms.ToolBarButton
    Friend WithEvents StatusBarPanelGeneral As System.Windows.Forms.StatusBarPanel
    Friend WithEvents MenuIntegraFront As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemIntegraFrontParametros As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemIntegracionAlmacen As System.Windows.Forms.MenuItem
    Friend WithEvents MenuIntegraAlmacen As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemIntegraAlmacenParametros As System.Windows.Forms.MenuItem
    Friend WithEvents StatusBarPanelDialogo As System.Windows.Forms.StatusBarPanel
    Friend WithEvents StatusBarPanelVersionExe As System.Windows.Forms.StatusBarPanel
    Friend WithEvents ToolBarButton1 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton5 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton6 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton8 As System.Windows.Forms.ToolBarButton
    Friend WithEvents MenuItem7 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem8 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem9 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemReportsNewHotel As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents ToolBarButtonParametrosFront As System.Windows.Forms.ToolBarButton
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem6 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem10 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem11 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem12 As System.Windows.Forms.MenuItem
    Friend WithEvents StatusBarPanelVersionEnsamblado As System.Windows.Forms.StatusBarPanel
    Friend WithEvents MenuItem13 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem14 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem15 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem16 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem17 As System.Windows.Forms.MenuItem
    Friend WithEvents ToolBarButtonNewConta As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton7 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton9 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton10 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton11 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButtonNewpaga As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton12 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton13 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton14 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton15 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButtonParametrosAlmacen As System.Windows.Forms.ToolBarButton
    Friend WithEvents StatusBarPanelPat As System.Windows.Forms.StatusBarPanel
    Friend WithEvents ToolBarButtonNominaA3 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButtonParametrosNomina As System.Windows.Forms.ToolBarButton
    Friend WithEvents MenuItem18 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem19 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem20 As System.Windows.Forms.MenuItem
    Friend WithEvents ToolBarButtonReciplus As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButtonParametrosFrontOfficePersonalizados As System.Windows.Forms.ToolBarButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormMenu))
        Me.StatusBarMenu = New System.Windows.Forms.StatusBar()
        Me.StatusBarPanelGeneral = New System.Windows.Forms.StatusBarPanel()
        Me.StatusBarPanelOtros = New System.Windows.Forms.StatusBarPanel()
        Me.StatusBarPanelBd = New System.Windows.Forms.StatusBarPanel()
        Me.StatusBarPanelDialogo = New System.Windows.Forms.StatusBarPanel()
        Me.StatusBarPanelVersionExe = New System.Windows.Forms.StatusBarPanel()
        Me.StatusBarPanelVersionEnsamblado = New System.Windows.Forms.StatusBarPanel()
        Me.StatusBarPanelPat = New System.Windows.Forms.StatusBarPanel()
        Me.ToolBarMenu = New System.Windows.Forms.ToolBar()
        Me.ToolBarButtonSalir = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButton1 = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButton5 = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButtonIntegraFront = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButton12 = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButton13 = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButton14 = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButton15 = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButtonIntegraAlmacen = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButtonNewConta = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButtonNewpaga = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButtonNewPos = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButtonNominaA3 = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButtonReciplus = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButton6 = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButton8 = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButton7 = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButton9 = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButton10 = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButton11 = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButtonParametrosFront = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButtonParametrosFrontOfficePersonalizados = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButtonParametrosAlmacen = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButtonParametrosNomina = New System.Windows.Forms.ToolBarButton()
        Me.ImageListMenu = New System.Windows.Forms.ImageList(Me.components)
        Me.MainMenu = New System.Windows.Forms.MainMenu(Me.components)
        Me.MenuItemArchivo = New System.Windows.Forms.MenuItem()
        Me.MenuItemSalir = New System.Windows.Forms.MenuItem()
        Me.MenuItemIntegracion = New System.Windows.Forms.MenuItem()
        Me.MenuItemIntegracionFront = New System.Windows.Forms.MenuItem()
        Me.MenuIntegraFront = New System.Windows.Forms.MenuItem()
        Me.MenuItem1 = New System.Windows.Forms.MenuItem()
        Me.MenuItemIntegraFrontParametros = New System.Windows.Forms.MenuItem()
        Me.MenuItem3 = New System.Windows.Forms.MenuItem()
        Me.MenuItem4 = New System.Windows.Forms.MenuItem()
        Me.MenuItem13 = New System.Windows.Forms.MenuItem()
        Me.MenuItem14 = New System.Windows.Forms.MenuItem()
        Me.MenuItem15 = New System.Windows.Forms.MenuItem()
        Me.MenuItem16 = New System.Windows.Forms.MenuItem()
        Me.MenuItem17 = New System.Windows.Forms.MenuItem()
        Me.MenuItemIntegracionAlmacen = New System.Windows.Forms.MenuItem()
        Me.MenuIntegraAlmacen = New System.Windows.Forms.MenuItem()
        Me.MenuItem2 = New System.Windows.Forms.MenuItem()
        Me.MenuItemIntegraAlmacenParametros = New System.Windows.Forms.MenuItem()
        Me.MenuItem9 = New System.Windows.Forms.MenuItem()
        Me.MenuItemReportsNewHotel = New System.Windows.Forms.MenuItem()
        Me.MenuItem7 = New System.Windows.Forms.MenuItem()
        Me.MenuItem8 = New System.Windows.Forms.MenuItem()
        Me.MenuItem18 = New System.Windows.Forms.MenuItem()
        Me.MenuItem19 = New System.Windows.Forms.MenuItem()
        Me.MenuItem20 = New System.Windows.Forms.MenuItem()
        Me.MenuItem5 = New System.Windows.Forms.MenuItem()
        Me.MenuItem6 = New System.Windows.Forms.MenuItem()
        Me.MenuItem10 = New System.Windows.Forms.MenuItem()
        Me.MenuItem11 = New System.Windows.Forms.MenuItem()
        Me.MenuItem12 = New System.Windows.Forms.MenuItem()
        CType(Me.StatusBarPanelGeneral, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StatusBarPanelOtros, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StatusBarPanelBd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StatusBarPanelDialogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StatusBarPanelVersionExe, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StatusBarPanelVersionEnsamblado, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StatusBarPanelPat, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'StatusBarMenu
        '
        Me.StatusBarMenu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusBarMenu.Location = New System.Drawing.Point(0, 551)
        Me.StatusBarMenu.Name = "StatusBarMenu"
        Me.StatusBarMenu.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.StatusBarPanelGeneral, Me.StatusBarPanelOtros, Me.StatusBarPanelBd, Me.StatusBarPanelDialogo, Me.StatusBarPanelVersionExe, Me.StatusBarPanelVersionEnsamblado, Me.StatusBarPanelPat})
        Me.StatusBarMenu.ShowPanels = True
        Me.StatusBarMenu.Size = New System.Drawing.Size(892, 22)
        Me.StatusBarMenu.TabIndex = 1
        '
        'StatusBarPanelGeneral
        '
        Me.StatusBarPanelGeneral.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.StatusBarPanelGeneral.Name = "StatusBarPanelGeneral"
        Me.StatusBarPanelGeneral.Width = 10
        '
        'StatusBarPanelOtros
        '
        Me.StatusBarPanelOtros.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.StatusBarPanelOtros.Name = "StatusBarPanelOtros"
        Me.StatusBarPanelOtros.Width = 10
        '
        'StatusBarPanelBd
        '
        Me.StatusBarPanelBd.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.StatusBarPanelBd.Name = "StatusBarPanelBd"
        Me.StatusBarPanelBd.Width = 10
        '
        'StatusBarPanelDialogo
        '
        Me.StatusBarPanelDialogo.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.StatusBarPanelDialogo.Name = "StatusBarPanelDialogo"
        Me.StatusBarPanelDialogo.Width = 10
        '
        'StatusBarPanelVersionExe
        '
        Me.StatusBarPanelVersionExe.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.StatusBarPanelVersionExe.Name = "StatusBarPanelVersionExe"
        Me.StatusBarPanelVersionExe.Width = 10
        '
        'StatusBarPanelVersionEnsamblado
        '
        Me.StatusBarPanelVersionEnsamblado.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.StatusBarPanelVersionEnsamblado.Name = "StatusBarPanelVersionEnsamblado"
        Me.StatusBarPanelVersionEnsamblado.Width = 10
        '
        'StatusBarPanelPat
        '
        Me.StatusBarPanelPat.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.StatusBarPanelPat.Name = "StatusBarPanelPat"
        Me.StatusBarPanelPat.Width = 10
        '
        'ToolBarMenu
        '
        Me.ToolBarMenu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ToolBarMenu.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.ToolBarButtonSalir, Me.ToolBarButton1, Me.ToolBarButton5, Me.ToolBarButtonIntegraFront, Me.ToolBarButton12, Me.ToolBarButton13, Me.ToolBarButton14, Me.ToolBarButton15, Me.ToolBarButtonIntegraAlmacen, Me.ToolBarButtonNewConta, Me.ToolBarButtonNewpaga, Me.ToolBarButtonNewPos, Me.ToolBarButtonNominaA3, Me.ToolBarButtonReciplus, Me.ToolBarButton6, Me.ToolBarButton8, Me.ToolBarButton7, Me.ToolBarButton9, Me.ToolBarButton10, Me.ToolBarButton11, Me.ToolBarButtonParametrosFront, Me.ToolBarButtonParametrosFrontOfficePersonalizados, Me.ToolBarButtonParametrosAlmacen, Me.ToolBarButtonParametrosNomina})
        Me.ToolBarMenu.DropDownArrows = True
        Me.ToolBarMenu.ImageList = Me.ImageListMenu
        Me.ToolBarMenu.Location = New System.Drawing.Point(0, 0)
        Me.ToolBarMenu.Name = "ToolBarMenu"
        Me.ToolBarMenu.ShowToolTips = True
        Me.ToolBarMenu.Size = New System.Drawing.Size(892, 59)
        Me.ToolBarMenu.TabIndex = 2
        '
        'ToolBarButtonSalir
        '
        Me.ToolBarButtonSalir.ImageIndex = 0
        Me.ToolBarButtonSalir.Name = "ToolBarButtonSalir"
        Me.ToolBarButtonSalir.Tag = "ToolBarButtonSalir"
        Me.ToolBarButtonSalir.Text = "Salir"
        '
        'ToolBarButton1
        '
        Me.ToolBarButton1.Name = "ToolBarButton1"
        Me.ToolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ToolBarButton5
        '
        Me.ToolBarButton5.Name = "ToolBarButton5"
        Me.ToolBarButton5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ToolBarButtonIntegraFront
        '
        Me.ToolBarButtonIntegraFront.ImageIndex = 1
        Me.ToolBarButtonIntegraFront.Name = "ToolBarButtonIntegraFront"
        Me.ToolBarButtonIntegraFront.Tag = "ToolBarButtonIntegraFront"
        Me.ToolBarButtonIntegraFront.Text = "NewHotel"
        Me.ToolBarButtonIntegraFront.ToolTipText = "NewHotel Front Office"
        '
        'ToolBarButton12
        '
        Me.ToolBarButton12.Name = "ToolBarButton12"
        Me.ToolBarButton12.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ToolBarButton13
        '
        Me.ToolBarButton13.Name = "ToolBarButton13"
        Me.ToolBarButton13.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ToolBarButton14
        '
        Me.ToolBarButton14.Name = "ToolBarButton14"
        Me.ToolBarButton14.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ToolBarButton15
        '
        Me.ToolBarButton15.Name = "ToolBarButton15"
        Me.ToolBarButton15.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ToolBarButtonIntegraAlmacen
        '
        Me.ToolBarButtonIntegraAlmacen.ImageIndex = 5
        Me.ToolBarButtonIntegraAlmacen.Name = "ToolBarButtonIntegraAlmacen"
        Me.ToolBarButtonIntegraAlmacen.Tag = "ToolBarButtonIntegraAlmacen"
        Me.ToolBarButtonIntegraAlmacen.Text = "NewStock"
        Me.ToolBarButtonIntegraAlmacen.ToolTipText = "NewStock (Gestión de Almacen)"
        '
        'ToolBarButtonNewConta
        '
        Me.ToolBarButtonNewConta.ImageIndex = 6
        Me.ToolBarButtonNewConta.Name = "ToolBarButtonNewConta"
        Me.ToolBarButtonNewConta.Tag = "ToolBarButtonNewConta"
        Me.ToolBarButtonNewConta.Text = "NewConta"
        Me.ToolBarButtonNewConta.ToolTipText = "NewConta (Gestión de Cobros)"
        '
        'ToolBarButtonNewpaga
        '
        Me.ToolBarButtonNewpaga.Enabled = False
        Me.ToolBarButtonNewpaga.ImageIndex = 8
        Me.ToolBarButtonNewpaga.Name = "ToolBarButtonNewpaga"
        Me.ToolBarButtonNewpaga.Tag = "ToolBarButtonNewPaga"
        Me.ToolBarButtonNewpaga.Text = "NewPaga"
        Me.ToolBarButtonNewpaga.ToolTipText = "NewPaga (Gestión de Pagos)"
        '
        'ToolBarButtonNewPos
        '
        Me.ToolBarButtonNewPos.ImageIndex = 7
        Me.ToolBarButtonNewPos.Name = "ToolBarButtonNewPos"
        Me.ToolBarButtonNewPos.Tag = "ToolBarButtonNewPos"
        Me.ToolBarButtonNewPos.Text = "NewPos"
        Me.ToolBarButtonNewPos.ToolTipText = "Punto de Venta Newpos/NewGolf"
        '
        'ToolBarButtonNominaA3
        '
        Me.ToolBarButtonNominaA3.ImageIndex = 11
        Me.ToolBarButtonNominaA3.Name = "ToolBarButtonNominaA3"
        Me.ToolBarButtonNominaA3.Tag = "ToolBarButtonNominaA3"
        Me.ToolBarButtonNominaA3.Text = "Nómina ()"
        '
        'ToolBarButtonReciplus
        '
        Me.ToolBarButtonReciplus.ImageIndex = 12
        Me.ToolBarButtonReciplus.Name = "ToolBarButtonReciplus"
        Me.ToolBarButtonReciplus.Tag = "ToolBarButtonReciplus"
        Me.ToolBarButtonReciplus.Text = "Reciplus"
        Me.ToolBarButtonReciplus.ToolTipText = "Importación de Facturas (Formato Contaplus)"
        '
        'ToolBarButton6
        '
        Me.ToolBarButton6.Name = "ToolBarButton6"
        Me.ToolBarButton6.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ToolBarButton8
        '
        Me.ToolBarButton8.Name = "ToolBarButton8"
        Me.ToolBarButton8.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ToolBarButton7
        '
        Me.ToolBarButton7.Name = "ToolBarButton7"
        Me.ToolBarButton7.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ToolBarButton9
        '
        Me.ToolBarButton9.Name = "ToolBarButton9"
        Me.ToolBarButton9.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ToolBarButton10
        '
        Me.ToolBarButton10.Name = "ToolBarButton10"
        Me.ToolBarButton10.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ToolBarButton11
        '
        Me.ToolBarButton11.Name = "ToolBarButton11"
        Me.ToolBarButton11.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ToolBarButtonParametrosFront
        '
        Me.ToolBarButtonParametrosFront.ImageIndex = 3
        Me.ToolBarButtonParametrosFront.Name = "ToolBarButtonParametrosFront"
        Me.ToolBarButtonParametrosFront.Tag = "ToolBarButtonParametrosFront"
        Me.ToolBarButtonParametrosFront.Text = "Parámetros"
        Me.ToolBarButtonParametrosFront.ToolTipText = "Parámetros Front Office (NewHotel/NewConta)"
        '
        'ToolBarButtonParametrosFrontOfficePersonalizados
        '
        Me.ToolBarButtonParametrosFrontOfficePersonalizados.ImageIndex = 3
        Me.ToolBarButtonParametrosFrontOfficePersonalizados.Name = "ToolBarButtonParametrosFrontOfficePersonalizados"
        Me.ToolBarButtonParametrosFrontOfficePersonalizados.Tag = "ToolBarButtonParametrosFrontOfficePersonalizados"
        Me.ToolBarButtonParametrosFrontOfficePersonalizados.Text = "Parámetros"
        Me.ToolBarButtonParametrosFrontOfficePersonalizados.ToolTipText = "Parámetros Front Office Personalizados"
        '
        'ToolBarButtonParametrosAlmacen
        '
        Me.ToolBarButtonParametrosAlmacen.ImageIndex = 3
        Me.ToolBarButtonParametrosAlmacen.Name = "ToolBarButtonParametrosAlmacen"
        Me.ToolBarButtonParametrosAlmacen.Tag = "ToolBarButtonParametrosAlmacen"
        Me.ToolBarButtonParametrosAlmacen.Text = "Parámetros"
        Me.ToolBarButtonParametrosAlmacen.ToolTipText = "Parámetros Almacén ( NewStock)"
        '
        'ToolBarButtonParametrosNomina
        '
        Me.ToolBarButtonParametrosNomina.ImageIndex = 3
        Me.ToolBarButtonParametrosNomina.Name = "ToolBarButtonParametrosNomina"
        Me.ToolBarButtonParametrosNomina.Tag = "ToolBarButtonParametrosNomina"
        Me.ToolBarButtonParametrosNomina.Text = "Parámetros"
        Me.ToolBarButtonParametrosNomina.ToolTipText = "Parámetros (Nómina)"
        '
        'ImageListMenu
        '
        Me.ImageListMenu.ImageStream = CType(resources.GetObject("ImageListMenu.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListMenu.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListMenu.Images.SetKeyName(0, "")
        Me.ImageListMenu.Images.SetKeyName(1, "")
        Me.ImageListMenu.Images.SetKeyName(2, "")
        Me.ImageListMenu.Images.SetKeyName(3, "")
        Me.ImageListMenu.Images.SetKeyName(4, "ooo_math.png")
        Me.ImageListMenu.Images.SetKeyName(5, "ark_addfile.png")
        Me.ImageListMenu.Images.SetKeyName(6, "kontact_date.png")
        Me.ImageListMenu.Images.SetKeyName(7, "pda_black.png")
        Me.ImageListMenu.Images.SetKeyName(8, "proxy.png")
        Me.ImageListMenu.Images.SetKeyName(9, "gnumeric.png")
        Me.ImageListMenu.Images.SetKeyName(10, "firefox.png")
        Me.ImageListMenu.Images.SetKeyName(11, "kontact.png")
        Me.ImageListMenu.Images.SetKeyName(12, "access.png")
        '
        'MainMenu
        '
        Me.MainMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemArchivo, Me.MenuItemIntegracion, Me.MenuItem9, Me.MenuItem7, Me.MenuItem18, Me.MenuItem5})
        '
        'MenuItemArchivo
        '
        Me.MenuItemArchivo.Index = 0
        Me.MenuItemArchivo.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemSalir})
        Me.MenuItemArchivo.Text = "Archivo"
        '
        'MenuItemSalir
        '
        Me.MenuItemSalir.Index = 0
        Me.MenuItemSalir.Text = "&Salir"
        '
        'MenuItemIntegracion
        '
        Me.MenuItemIntegracion.Index = 1
        Me.MenuItemIntegracion.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemIntegracionFront, Me.MenuItemIntegracionAlmacen})
        Me.MenuItemIntegracion.Text = "Integración Contable"
        '
        'MenuItemIntegracionFront
        '
        Me.MenuItemIntegracionFront.Index = 0
        Me.MenuItemIntegracionFront.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuIntegraFront, Me.MenuItem1, Me.MenuItemIntegraFrontParametros, Me.MenuItem3, Me.MenuItem13})
        Me.MenuItemIntegracionFront.Text = "Front Office"
        '
        'MenuIntegraFront
        '
        Me.MenuIntegraFront.Index = 0
        Me.MenuIntegraFront.Text = "Integración"
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 1
        Me.MenuItem1.Text = "-"
        '
        'MenuItemIntegraFrontParametros
        '
        Me.MenuItemIntegraFrontParametros.Index = 2
        Me.MenuItemIntegraFrontParametros.Text = "Parámetros Generales"
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 3
        Me.MenuItem3.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem4})
        Me.MenuItem3.Text = "Parámetros Personalizados"
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 0
        Me.MenuItem4.Text = "Cuentas Contables Servicios por Bloques de Alojamientos"
        '
        'MenuItem13
        '
        Me.MenuItem13.Index = 4
        Me.MenuItem13.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem14, Me.MenuItem15, Me.MenuItem16, Me.MenuItem17})
        Me.MenuItem13.Text = "Parámetros Personalizados Axapta"
        '
        'MenuItem14
        '
        Me.MenuItem14.Index = 0
        Me.MenuItem14.Text = "Equivelencias Empresas y Hoteles ( para el Web Service ) "
        '
        'MenuItem15
        '
        Me.MenuItem15.Index = 1
        Me.MenuItem15.Text = "Equivalencias Almacenes  y Puntos de Venta ( para el Web Service ) "
        '
        'MenuItem16
        '
        Me.MenuItem16.Index = 2
        Me.MenuItem16.Text = "Equivalencias Formas de Cobro / Bancos"
        '
        'MenuItem17
        '
        Me.MenuItem17.Index = 3
        Me.MenuItem17.Text = "Enumeración de Puntos de Venta que generan Diario de Pérdidas y Ganancias"
        '
        'MenuItemIntegracionAlmacen
        '
        Me.MenuItemIntegracionAlmacen.Index = 1
        Me.MenuItemIntegracionAlmacen.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuIntegraAlmacen, Me.MenuItem2, Me.MenuItemIntegraAlmacenParametros})
        Me.MenuItemIntegracionAlmacen.Text = "Almacen"
        '
        'MenuIntegraAlmacen
        '
        Me.MenuIntegraAlmacen.Index = 0
        Me.MenuIntegraAlmacen.Text = "Integración"
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 1
        Me.MenuItem2.Text = "-"
        '
        'MenuItemIntegraAlmacenParametros
        '
        Me.MenuItemIntegraAlmacenParametros.Index = 2
        Me.MenuItemIntegraAlmacenParametros.Text = "Parámetros"
        '
        'MenuItem9
        '
        Me.MenuItem9.Index = 2
        Me.MenuItem9.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemReportsNewHotel})
        Me.MenuItem9.Text = "Reports"
        '
        'MenuItemReportsNewHotel
        '
        Me.MenuItemReportsNewHotel.Index = 0
        Me.MenuItemReportsNewHotel.Text = "NewHotel"
        '
        'MenuItem7
        '
        Me.MenuItem7.Index = 3
        Me.MenuItem7.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem8})
        Me.MenuItem7.Text = "Ventana"
        '
        'MenuItem8
        '
        Me.MenuItem8.Index = 0
        Me.MenuItem8.Text = "Organizar"
        '
        'MenuItem18
        '
        Me.MenuItem18.Index = 4
        Me.MenuItem18.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem19})
        Me.MenuItem18.Text = "Utilidades"
        '
        'MenuItem19
        '
        Me.MenuItem19.Index = 0
        Me.MenuItem19.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem20})
        Me.MenuItem19.Text = "Base de Datos"
        '
        'MenuItem20
        '
        Me.MenuItem20.Index = 0
        Me.MenuItem20.Text = "Computar Estadísticas"
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 5
        Me.MenuItem5.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem6, Me.MenuItem10, Me.MenuItem11, Me.MenuItem12})
        Me.MenuItem5.Text = "Ayuda"
        '
        'MenuItem6
        '
        Me.MenuItem6.Index = 0
        Me.MenuItem6.Text = "Acerca de "
        '
        'MenuItem10
        '
        Me.MenuItem10.Index = 1
        Me.MenuItem10.Text = "-"
        '
        'MenuItem11
        '
        Me.MenuItem11.Index = 2
        Me.MenuItem11.Text = "Imagen de Fondo"
        '
        'MenuItem12
        '
        Me.MenuItem12.Index = 3
        Me.MenuItem12.Text = "Control de Versiones"
        '
        'FormMenu
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(892, 573)
        Me.Controls.Add(Me.ToolBarMenu)
        Me.Controls.Add(Me.StatusBarMenu)
        Me.IsMdiContainer = True
        Me.Menu = Me.MainMenu
        Me.MinimumSize = New System.Drawing.Size(900, 600)
        Me.Name = "FormMenu"
        Me.Text = "Menu"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.StatusBarPanelGeneral, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StatusBarPanelOtros, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StatusBarPanelBd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StatusBarPanelDialogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StatusBarPanelVersionExe, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StatusBarPanelVersionEnsamblado, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StatusBarPanelPat, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub MenuItemIntegracionFront_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemIntegracionFront.Click
        Dim Form As New FormIntegraFront
        Form.MdiParent = Me
        Me.Cursor = Cursors.WaitCursor
        Form.Show()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub ToolBarMenu_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ToolBarMenu.ButtonClick
        If e.Button.Tag = "ToolBarButtonIntegraFront" Then




            Dim Form As New FormIntegraFront(Me.StatusBarMenu)

            Dim Result As Boolean = EstaelFormularioAbierto(Form)

            If IsNothing(Result) = False Then
                If Result = False Then
                    Form.MdiParent = Me
                    Me.Cursor = Cursors.WaitCursor
                    Form.Show()
                    Me.Cursor = Cursors.Default
                    Me.Text = " " & VERSION
                End If

            End If


        End If
        If e.Button.Tag = "ToolBarButtonIntegraAlmacen" Then
            Dim Form As New FormIntegraAlmacen
            Form.MdiParent = Me
            Me.Cursor = Cursors.WaitCursor
            Form.Show()
            Me.Cursor = Cursors.Default
            Me.Text = " " & VERSION

        End If
        If e.Button.Tag = "ToolBarButtonNewConta" Then
            Dim Form As New FormIntegraNewConta
            Form.MdiParent = Me
            Me.Cursor = Cursors.WaitCursor
            Form.Show()
            Me.Cursor = Cursors.Default
            Me.Text = " " & VERSION
        End If
        If e.Button.Tag = "ToolBarButtonNewPaga" Then
            Dim Form As New FormIntegraNewPaga
            Form.MdiParent = Me
            Me.Cursor = Cursors.WaitCursor
            Form.Show()
            Me.Cursor = Cursors.Default
            Me.Text = " " & VERSION
        End If

        If e.Button.Tag = "ToolBarButtonParametrosFront" Then
            Dim F As New FormPassword

            F.StartPosition = FormStartPosition.CenterScreen
            F.ShowDialog()
            If PARA_PASO_OK = PARA_PASO_TRY Then
                Me.Cursor = Cursors.WaitCursor
                Dim Form As New FormParametrosFront
                Form.Width = My.Forms.FormMenu.Width - 100
                Form.StartPosition = FormStartPosition.CenterParent
                Form.ShowDialog()
                PARA_PASO_TRY = ""
                Me.Cursor = Cursors.Default
            Else
                MsgBox("No está autorizado a este proceso , o Palabra de Paso incorrecta", MsgBoxStyle.Information, "Atención")
                Exit Sub
            End If
        End If
        If e.Button.Tag = "ToolBarButtonParametrosAlmacen" Then
            Dim F As New FormPassword
            F.StartPosition = FormStartPosition.CenterScreen
            F.ShowDialog()
            If PARA_PASO_OK = PARA_PASO_TRY Then
                Me.Cursor = Cursors.WaitCursor
                Dim Form As New FormParametrosAlmacen
                Form.ShowDialog()
                PARA_PASO_TRY = ""
                Me.Cursor = Cursors.Default
            Else
                MsgBox("No está autorizado a este proceso , o Palabra de Paso incorrecta", MsgBoxStyle.Information, "Atención")
                Exit Sub
            End If
        End If


        If e.Button.Tag = "ToolBarButtonNominaA3" Then
            Dim F As New FormPassword
            F.StartPosition = FormStartPosition.CenterScreen
            F.ShowDialog()
            If PARA_PASO_OK = PARA_PASO_TRY Then
                Me.Cursor = Cursors.WaitCursor
                Dim Form As New FormIntegraNominaA3
                Form.ShowDialog()
                PARA_PASO_TRY = ""
                Me.Cursor = Cursors.Default
                Me.Text = " " & VERSION
            Else
                MsgBox("No está autorizado a este proceso , o Palabra de Paso incorrecta", MsgBoxStyle.Information, "Atención")
                Exit Sub
            End If
        End If

        If e.Button.Tag = "ToolBarButtonParametrosNomina" Then
            Dim F As New FormPassword
            F.StartPosition = FormStartPosition.CenterScreen
            F.ShowDialog()
            If PARA_PASO_OK = PARA_PASO_TRY Then
                Me.Cursor = Cursors.WaitCursor
                Dim Form As New FormParametrosNomina
                Form.ShowDialog()
                PARA_PASO_TRY = ""
                Me.Cursor = Cursors.Default
            Else
                MsgBox("No está autorizado a este proceso , o Palabra de Paso incorrecta", MsgBoxStyle.Information, "Atención")
                Exit Sub
            End If
        End If

        If e.Button.Tag = "ToolBarButtonParametrosFrontOfficePersonalizados" Then
            Dim Form As New FormCuentasServiciosBloque
            Me.Cursor = Cursors.WaitCursor
            Form.ShowDialog()
            Me.Cursor = Cursors.Default
        End If

        If e.Button.Tag = "ToolBarButtonReciplus" Then
            Dim Form As New FormReciplus
            Me.Cursor = Cursors.WaitCursor
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.ShowDialog()
            Me.Cursor = Cursors.Default
            Me.Text = " " & VERSION
        End If
        If e.Button.Tag = "ToolBarButtonSalir" Then
            Me.Close()
        End If
    End Sub



    Private Sub FormMenu_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            If My.Application.CommandLineArgs.Count > 0 Then

                GAUTOMATICO = True
            End If


            Me.StatusBarPanelOtros.Text = System.Environment.UserName & " en " & System.Environment.UserDomainName & " desde " & System.Environment.MachineName

            WORKSTATION = System.Environment.MachineName
            Me.StatusBarPanelVersionEnsamblado.Text = "FrameWork = " & Environment.Version.ToString



            '    Dim ver As Version = Environment.Version

            Me.Text += " " & VERSION


            ' imponer  ultura
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("es-ES", False)
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = "."
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = ","


            '  Me.StatusBarPanelGeneral.Text = "Sep Decimal " & Application.CurrentInputLanguage.Culture.NumberFormat.CurrencyDecimalSeparator
            'Me.ComprobarVersion()
            Me.LeeParametros()
            Me.ComprobarVersion()
            Me.Derechos()
            ' Me.SeleccionaDriverDB()


            If GAUTOMATICO = True Then
                Me.AUTOMATICO(DateAdd(DateInterval.Day, -2, Now))
            End If

            '  CONTROLSUpdateFont(Me.Controls, "TAHOMA", 8)
            '  Me.Update()

            ' saber si el programa ya esta en ejec ucion

            Dim Result As Boolean = EstaElProgramaenEjecucion("MENU")

            If IsNothing(Result) = False Then
                If Result = True Then
                    If MessageBox.Show("Esta Aplicación ya se encuentra en Ejecución en esta Computadora" & vbCrLf & "Desea Continuar", "Atención", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.Cancel Then
                        Me.Close()
                    End If
                End If
            End If


        Catch EX As Exception
            MsgBox(EX.Message)
        End Try
    End Sub

    Private Sub MenuIntegraFront_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuIntegraFront.Click
        Dim Form As New FormIntegraFront(Me.StatusBarMenu)
        Form.MdiParent = Me
        Me.Cursor = Cursors.WaitCursor
        Form.Show()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub ComprobarVersion()
        Try
            Dim SQL As String
            Dim MyIni As New cIniArray

            Dim FechaExe As Date
            Dim FechaBase As Date
            Dim Pat As String

            Me.StatusBarPanelDialogo.Text = "Conectando a la Base de Datos"
            Me.Cursor = Cursors.WaitCursor
            Dim DB As New C_DATOS.C_DatosOledb(MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"))



            DB.EjecutaSqlCommit("ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY'")
            Me.StatusBarPanelDialogo.Text = "Conectado a la Base de Datos " & StrConexionExtraeDataSource(MyIni.IniGet(Application.StartupPath & "\Menu.ini", "DATABASE", "STRING"))
            Me.Cursor = Cursors.Default

            SQL = "SELECT PARA_FECHA_EXE FROM TH_PARA "
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & EMPGRUPO_COD & "'"
            '  SQL += " AND  PARA_EMP_COD = '" & EMP_COD & "'"

            FechaExe = CType(DB.EjecutaSqlScalar(SQL), Date)

            SQL = "SELECT PARA_FECHA_BASE FROM TH_PARA"
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & EMPGRUPO_COD & "'"
            '   SQL += " AND  PARA_EMP_COD = '" & EMP_COD & "'"

            FechaBase = CType(DB.EjecutaSqlScalar(SQL), Date)

            Me.StatusBarPanelVersionExe.Text = "Ejecutable = " & FechaExe & " Base de Datos = " & FechaBase

            SQL = "SELECT NVL(PARA_PAT_NUM,'?') FROM TH_PARA"
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & EMPGRUPO_COD & "'"
            '    SQL += " AND  PARA_EMP_COD = '" & EMP_COD & "'"

            Pat = CStr(DB.EjecutaSqlScalar(SQL))

            SQL = "SELECT NVL(PARA_PASO,'?') FROM TH_PARA"
            SQL += " WHERE PARA_EMPGRUPO_COD = '" & EMPGRUPO_COD & "'"
            '     SQL += " AND  PARA_EMP_COD = '" & EMP_COD & "'"

            PARA_PASO_OK = CStr(DB.EjecutaSqlScalar(SQL))


            Me.StatusBarPanelPat.Text = "Pat Num = " & Pat



            Me.StatusBarMenu.Update()

            If FechaExe > FechaBase Then
                MsgBox("La versión del Ejecutable es superior a la Base de Datos " & vbCrLf & "Desea Actualizar ? ", MsgBoxStyle.Question)
            End If

            If FechaExe < FechaBase Then
                MsgBox("La versión del Ejecutable no se corresponde con la Base de Datos ", MsgBoxStyle.Information)
                Application.Exit()
            End If

            DB.CerrarConexion()

            ' FONDO
            Dim Fondo As String

            Fondo = MyIni.IniGet(Application.StartupPath & "\Menu.ini", "OPERATION", "FONDO")

            If Fondo.Length > 0 Then
                If Fondo = 1 Then
                    Me.BackgroundImage = My.Resources.ResourceFondos.propietarios
                End If
            Else
                MsgBox("Revise Parámetro FONDO en OPERATION del Fichero INI", MsgBoxStyle.Information, "Atención")
            End If


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
            Application.Exit()
        Finally

        End Try
    End Sub

    Private Sub Derechos()
        Try

            If MyIni.IniGet(Application.StartupPath & "\Menu.ini", "BUTTONS", "NEWHOTEL") = "1" Then
                Me.ToolBarButtonIntegraFront.Enabled = True
                Me.MenuItemIntegracionFront.Enabled = True
            Else
                Me.ToolBarButtonIntegraFront.Enabled = False
                Me.MenuItemIntegracionFront.Enabled = False
            End If


            If MyIni.IniGet(Application.StartupPath & "\Menu.ini", "BUTTONS", "NEWSTOCK") = "1" Then
                Me.ToolBarButtonIntegraAlmacen.Enabled = True
                Me.MenuItemIntegracionAlmacen.Enabled = True
            Else
                Me.ToolBarButtonIntegraAlmacen.Enabled = False
                Me.MenuItemIntegracionAlmacen.Enabled = False
            End If



            If MyIni.IniGet(Application.StartupPath & "\Menu.ini", "BUTTONS", "NEWCONTA") = "1" Then
                Me.ToolBarButtonNewConta.Enabled = True
            Else
                Me.ToolBarButtonNewConta.Enabled = False
            End If

            If MyIni.IniGet(Application.StartupPath & "\Menu.ini", "BUTTONS", "NOMINAA3") = "1" Then
                Me.ToolBarButtonNominaA3.Enabled = True
            Else
                Me.ToolBarButtonNominaA3.Enabled = False
            End If



            If MyIni.IniGet(Application.StartupPath & "\Menu.ini", "BUTTONS", "NEWPOS") = "1" Then
                Me.ToolBarButtonNewPos.Enabled = True
            Else
                Me.ToolBarButtonNewPos.Enabled = False
            End If

            If MyIni.IniGet(Application.StartupPath & "\Menu.ini", "BUTTONS", "NEWPAGA") = "1" Then
                Me.ToolBarButtonNewpaga.Enabled = True
            Else
                Me.ToolBarButtonNewpaga.Enabled = False
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub SeleccionaDriverDB()
        Try
            Dim ControlDrv As String
            ControlDrv = MyIni.IniGet(Application.StartupPath & "\Menu.ini", "WORKSTATION", WORKSTATION)

            If ControlDrv = "" Then
                MyIni.IniWrite(Application.StartupPath & "\Menu.ini", "WORKSTATION", WORKSTATION, "MSDAORA.1")
            Else
                WORKSTATIONDBDRIVER = MyIni.IniGet(Application.StartupPath & "\Menu.ini", "WORKSTATION", WORKSTATION)
            End If



        Catch ex As Exception
            WORKSTATIONDBDRIVER = ""
        Finally
            Me.StatusBarPanelBd.Text += " " & WORKSTATIONDBDRIVER
        End Try
    End Sub

    Private Sub MenuItemIntegraFrontParametros_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemIntegraFrontParametros.Click
        Dim Paso As String

        Paso = InputBox("Palabra de Paso")
        If Paso = PARA_PASO_OK Then
            Dim FORM As New FormParametrosFront
            FORM.ShowDialog()
        Else
            Exit Sub
        End If
    End Sub

    Private Sub LeeParametros()
        Try

            STRCONEXIONCENTRAL = MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING")
            REPORT_PATH = Application.StartupPath & MyIni.IniGet(Application.StartupPath & "\menu.ini", "CRYSTAL REPORT", "REPORT-PATH")

            EMPGRUPO_COD = MyIni.IniGet(Application.StartupPath & "\menu.ini", "PARAMETER", "PARA_EMPGRUPO_COD")
            EMP_COD = MyIni.IniGet(Application.StartupPath & "\menu.ini", "OPERATION", "EMP_COD")
            ENABLE_CHECKBOX = MyIni.IniGet(Application.StartupPath & "\menu.ini", "OPERATION", "ENABLE_CHECKBOX")
            SHOW_FALTA = MyIni.IniGet(Application.StartupPath & "\menu.ini", "OPERATION", "SHOW_FALTA")

            SHOW_INCIDENCIAS = MyIni.IniGet(Application.StartupPath & "\menu.ini", "OPERATION", "SHOW_INCIDENCIAS")


            MULTIFECHA = MyIni.IniGet(Application.StartupPath & "\menu.ini", "OPERATION", "MULTIFECHA")
            MULTIFILE = MyIni.IniGet(Application.StartupPath & "\menu.ini", "OPERATION", "MULTIFILE")


            MINIMIZA = MyIni.IniGet(Application.StartupPath & "\menu.ini", "OPERATION", "MINIMIZA")


            CUENTA_OPE_CAJA = MyIni.IniGet(Application.StartupPath & "\menu.ini", "OPERATION", "CUENTA_OPE_CAJA")








            If MyIni.IniGet(Application.StartupPath & "\menu.ini", "OPERATION", "REDONDEO") <> "" Then
                TIPOREDONDEO = CInt(MyIni.IniGet(Application.StartupPath & "\menu.ini", "OPERATION", "REDONDEO"))
            Else
                TIPOREDONDEO = 1
            End If



            If MyIni.IniGet(Application.StartupPath & "\menu.ini", "OPERATION", "PROCESA_NOTASCREDITO") <> "" Then
                PROCESA_NOTASCREDITO = CInt(MyIni.IniGet(Application.StartupPath & "\menu.ini", "OPERATION", "PROCESA_NOTASCREDITO"))
            Else
                PROCESA_NOTASCREDITO = 1
            End If

            If STRCONEXIONCENTRAL = "" Then
                MsgBox("Parámetro Fichero Ini No Válido [DATABASE]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            End If




            If MINIMIZA = "" Then
                MINIMIZA = "0"
            End If

            If EMP_COD = "" Then
                EMP_COD = "0"
            End If

            If ENABLE_CHECKBOX = "" Then
                MsgBox("Parámetro Fichero Ini No Válido [ENABLE_CHECKBOX]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            ElseIf ENABLE_CHECKBOX = "1" Or ENABLE_CHECKBOX = "0" Then
                ' ok 
            Else
                MsgBox("Parámetro Fichero Ini No Válido [ENABLE_CHECKBOX]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            End If


            If MULTIFECHA = "" Then
                MsgBox("Parámetro Fichero Ini No Válido [MULTIFECHA]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            ElseIf MULTIFECHA = "1" Or MULTIFECHA = "0" Then
                ' ok 
            Else
                MsgBox("Parámetro Fichero Ini No Válido [MULTIFECHA]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            End If


            If MULTIFILE = "" Then
                MsgBox("Parámetro Fichero Ini No Válido [MULTIFILE]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            ElseIf MULTIFILE = "1" Or MULTIFILE = "0" Then
                ' ok 
            Else
                MsgBox("Parámetro Fichero Ini No Válido [MULTIFILE]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            End If



            If SHOW_INCIDENCIAS = "" Then
                MsgBox("Parámetro Fichero Ini No Válido [SHOW_INCIDENCIAS]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            ElseIf SHOW_INCIDENCIAS = "1" Or SHOW_INCIDENCIAS = "0" Then
                ' ok 
            Else
                MsgBox("Parámetro Fichero Ini No Válido [SHOW_INCIDENCIAS]", MsgBoxStyle.Information, "Atención")
                Me.Close()
            End If

            If SHOW_INCIDENCIAS = "1" Then
                MUESTRAINCIDENCIAS = True
            Else
                MUESTRAINCIDENCIAS = False
            End If

            ' ALGUNOS CONTROLES 

            If EMPGRUPO_COD = "SATO" Then
                ToolBarButtonParametrosFrontOfficePersonalizados.Enabled = True
                MenuItem4.Enabled = True
                MenuItem13.Enabled = True
            Else
                ToolBarButtonParametrosFrontOfficePersonalizados.Enabled = False
                MenuItem4.Enabled = False
                MenuItem13.Enabled = False
            End If


        Catch ex As Exception

        End Try
    End Sub

    Private Sub MenuItem8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem8.Click
        Try
            Me.LayoutMdi(MdiLayout.Cascade)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub


    Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem4.Click
        Dim Form As New FormCuentasServiciosBloque
        Form.MdiParent = Me
        Me.Cursor = Cursors.WaitCursor
        Form.Show()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub MenuItem11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem11.Click
        Try
            If MessageBox.Show("Cambiar Imagen de Fondo", "Atención", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
                If IsNothing(Me.BackgroundImage) = True Then
                    Me.BackgroundImage = My.Resources.ResourceFondos.propietarios
                    'Me.BackgroundImage = My.Resources.ResourceFondos.Background
                Else
                    Me.BackgroundImage = Nothing
                End If
            End If


        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub


    Private Sub MenuItem12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem12.Click
        Try
            Dim Form As New FormConTrolVersiones
            Form.ShowDialog()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub MenuItem14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem14.Click
        Try
            Dim Cadena As String
            Cadena = MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE-AX", "STRING-AX")
            If Cadena <> "" Then
                Dim Form As New FormMantenimientoMaster(Cadena)
                Form.MdiParent = Me
                Form.StartPosition = FormStartPosition.CenterScreen
                Form.Show()
                Form.Update()
            Else
                MsgBox("No se Localiza DATABASE-AX en Fichero INI", MsgBoxStyle.Information, "Atención")

            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub MenuItem17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem17.Click
        Try
            Dim Cadena As String
            Cadena = MyIni.IniGet(Application.StartupPath & "\menu.ini", "DATABASE", "STRING")
            If Cadena <> "" Then
                Dim Form As New FormMantenimiento_TH_IPOSVEN_AX(Cadena, EMPGRUPO_COD)
                Form.MdiParent = Me
                Form.StartPosition = FormStartPosition.CenterScreen
                Form.Show()
                Form.Update()
            Else
                MsgBox("No se Localiza DATABASE en Fichero INI", MsgBoxStyle.Information, "Atención")
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub AUTOMATICO(ByVal vFecha As Date)
        Dim Form As New FormIntegraFront(Me.StatusBarMenu)
        Form.MdiParent = Me
        Me.Cursor = Cursors.WaitCursor
        Form.Show()
        Me.Cursor = Cursors.Default


    End Sub

   

    Private Sub MenuItem20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem20.Click
        Try
            Dim StrConexion As String
            Dim DbUser As String = "SYSTEM"
            Dim DbPwd As String = ""
            Dim DbAlias As String = StrConexionExtraeDataSource(STRCONEXIONCENTRAL)
            Dim DB As C_DATOS.C_DatosOledb
            Dim SQL As String

            DbPwd = InputBox("Teclee la Password del Usuario SYSTEM de ORACLE", "Atención")

            If DbPwd <> "" Then
                StrConexion = "Provider=MSDAORA.1;User Id=" & DbUser & ";Password=" & DbPwd & ";Data Source = " & DbAlias
                DB = New C_DATOS.C_DatosOledb(StrConexion)
                If DB.EstadoConexion = ConnectionState.Open Then
                    ' FALLA +++
                    ' SQL = "EXECUTE DBMS_UTILITY.ANALYZE_SCHEMA('INTEGRACION','COMPUTE');"
                    SQL = "SELECT SYSDATE FROM DUAL"
                    Me.Cursor = Cursors.WaitCursor
                    DB.EjecutaSqlScalarNoTrans(SQL)
                    DB.CerrarConexion()
                    Me.Cursor = Cursors.Default
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
