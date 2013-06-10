Public Class NatureTemplate
    Inherits Nature

#Region "Fields"
    Private _IsChecked As Boolean
#End Region

#Region "Properties"
    Public Property IsChecked As Boolean
        Get
            Return Me._IsChecked
        End Get
        Set(ByVal value As Boolean)
            Me._IsChecked = value
        End Set
    End Property
#End Region

#Region "Constructor"

    Public Sub New(ByVal nature As Nature, Optional ByVal isChecked As Boolean = False)
        Me.IsChecked = isChecked
        Me.Identifier = nature.Identifier
        Me.Label = nature.Label
    End Sub

#End Region

End Class
