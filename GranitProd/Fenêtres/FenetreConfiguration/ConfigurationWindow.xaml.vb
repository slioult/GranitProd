Public Class ConfigurationWindow

#Region "Fields"
    Private session As Session
    Private NouvelleCommande As NouvelleCommande
#End Region

#Region "Constructor"

    Public Sub New(ByVal session As Session, Optional ByVal nouvelleCommande As NouvelleCommande = Nothing)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        If (session.IsUpdSession = False) Then
            TabSession.Visibility = Windows.Visibility.Collapsed
        End If

        Me.NouvelleCommande = nouvelleCommande
        Me.confMateriau.NouvelleCommande = Me.NouvelleCommande
        Me.confNature.NouvelleCommande = Me.NouvelleCommande
        Me.confEtat.NouvelleCommande = Me.NouvelleCommande
        Me.confFinalisation.NouvelleCommande = Me.NouvelleCommande
        Me.confQualite.NouvelleCommande = Me.NouvelleCommande
        Me.confReleves.NouvelleCommande = Me.NouvelleCommande
        Me.confEpaisseur.NouvelleCommande = Me.NouvelleCommande

    End Sub

#End Region

End Class
