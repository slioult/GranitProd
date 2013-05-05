Public Class Version

#Region "Constructor"

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Me.TxtVersion.Text = My.Application.Info.Version.ToString()

    End Sub

#End Region

End Class
