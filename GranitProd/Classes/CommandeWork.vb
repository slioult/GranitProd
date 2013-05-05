Public Class CommandeWork
    Inherits Commande

#Region "Fields"
    Private _D As Date
    Private _Border As String
#End Region

#Region "Properties"

    Public Property D As Date
        Get
            Return Me._D
        End Get
        Set(ByVal value As Date)
            Me._D = value
        End Set
    End Property

    Public Property Border As String
        Get
            Return Me._Border
        End Get
        Set(ByVal value As String)
            Me._Border = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal cmd As Commande, ByVal d As Date, ByVal border As String)
        Me.Identifier = cmd.Identifier
        Me.NoCommande = cmd.NoCommande
        Me.MontantHT = cmd.MontantHT
        Me.Arrhes = cmd.Arrhes
        Me.DateCommande = cmd.DateCommande
        Me.AdresseChantier = cmd.AdresseChantier
        Me.TpsDebit = cmd.TpsDebit
        Me.TpsCommandeNumerique = cmd.TpsCommandeNumerique
        Me.TpsFinition = cmd.TpsFinition
        Me.TpsAutres = cmd.TpsAutres
        Me.DelaiPrevu = cmd.DelaiPrevu
        Me.Etat = cmd.Etat
        Me.Client = cmd.Client
        Me.Contremarque = cmd.Contremarque
        Me.Mesure = cmd.Mesure
        Me.DateMesure = cmd.DateMesure
        Me.Materiaux = cmd.Materiaux
        Me.Natures = cmd.Natures
        Me.Finalisations = cmd.Finalisations
        Me.DateFinalisations = cmd.DateFinalisations
        Me.Remarques = cmd.Remarques
        Me.Qualites = cmd.Qualites
        Me.D = d
        Me.Border = border
    End Sub

#End Region

End Class
