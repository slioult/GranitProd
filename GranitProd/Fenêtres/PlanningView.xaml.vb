Public Class PlanningView

#Region "Constructor"

    Public Sub New(ByVal pl As PlanningControl, ByVal session As Session, ByVal d As DateTime)
        InitializeComponent()

        Me.planning.Session = session
        Me.planning.BtnExtend.Visibility = Windows.Visibility.Collapsed
        Me.planning.SelectDate = d
    End Sub

#End Region

End Class
