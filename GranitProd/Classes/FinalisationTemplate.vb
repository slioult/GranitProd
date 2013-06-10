Public Class FinalisationTemplate
    Inherits Finalisation

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

    Public Sub New(ByVal finalisation As Finalisation, Optional ByVal isChecked As Boolean = False)
        Me.IsChecked = isChecked
        Me.Identifier = finalisation.Identifier
        Me.Label = finalisation.Label
        Me.Color = finalisation.Color
        Me.Display = finalisation.Display
    End Sub

#End Region

End Class
