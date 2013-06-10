Public Class MateriauTemplate
    Inherits Materiau

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

    Public Sub New(ByVal mat As Materiau, Optional ByVal isChecked As Boolean = False)
        Me.IsChecked = isChecked
        Me.Identifier = mat.Identifier
        Me.Label = mat.Label
        Me.Epaisseur = mat.Epaisseur
    End Sub

#End Region

End Class
