Public Class FormPideFechas



#Region "CONSTRUCTOR"



    Public Sub New(ByVal vfecha As Date)
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()

        Me.DateTimePicker1.Value = vfecha
        Me.DateTimePicker2.Value = vfecha


    End Sub

    Public Sub New()
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()

        Me.DateTimePicker1.Value = Now
        Me.DateTimePicker2.Value = Now


    End Sub
#End Region
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Try
            FEC1 = Me.DateTimePicker1.Value
            FEC2 = Me.DateTimePicker2.Value
            CONTROLF = True
            RESULTBOOLEAN = Me.CheckBoxExcluyeInventario.Checked
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub FormPideFechas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            
            RESULTBOOLEAN = Me.CheckBoxExcluyeInventario.Checked
        Catch ex As Exception

        End Try
    End Sub
End Class