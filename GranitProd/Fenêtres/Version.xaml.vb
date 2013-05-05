Public Class Version

#Region "Constructor"

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Me.TxtVersion.Text = My.Application.Info.Version.ToString().Substring(0, My.Application.Info.Version.ToString().Length - 2)

    End Sub

#End Region

End Class
