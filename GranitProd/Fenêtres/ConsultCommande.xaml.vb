Imports System.ComponentModel

Public Class ConsultCommande

#Region "Fields"
    Private _Session As Session
    Private _Commande As Commande
#End Region

#Region "Properties"

    Public Property Session As Session
        Get
            Return Me._Session
        End Get
        Set(ByVal value As Session)
            Me._Session = value
        End Set
    End Property

    Public Property Commande As Commande
        Get
            Return Me._Commande
        End Get
        Set(ByVal value As Commande)
            Me._Commande = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal session As Session, Optional ByVal commande As Commande = Nothing, Optional ByVal search As RechercheCommande = Nothing,
                   Optional ByVal planning As PlanningControl = Nothing)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Me.Session = session
        Me.NewCmd.Session = Me.Session
        Me.NewCmd.IsUpdate = True
        Me.NewCmd.UC = search
        Me.NewCmd.Planning = planning
        Me.NewCmd.Commande = commande
        Me.NewCmd.Window = Me

    End Sub

#End Region

#Region "Button"

    ''' <summary>
    ''' Bouton servant à lancer la fenêtre de configuration
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnConfig_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim conf As New ConfigurationWindow(Me.Session, Me.NewCmd)
        conf.ShowDialog()
    End Sub

#End Region

End Class
