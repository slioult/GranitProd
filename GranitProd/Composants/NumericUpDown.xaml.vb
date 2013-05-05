Public Class NumericUpDown
    Private _Value As Integer = 0

    Public Property Value As Integer
        Get
            Return Me._Value
        End Get

        Set(ByVal value As Integer)
            Me._Value = value
            TxtNum.Text = value.ToString()
        End Set
    End Property

    Private Sub TxtNum_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs)
        If (Not (Integer.TryParse(TxtNum.Text, Value))) Then
            TxtNum.Text = Value.ToString()
        End If
    End Sub

    Private Sub BtnUp_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.Value = Me.Value + 1
    End Sub

    Private Sub BtnDown_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.Value = Me.Value - 1
    End Sub

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().

    End Sub
End Class
