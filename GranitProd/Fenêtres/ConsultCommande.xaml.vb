Imports System.ComponentModel

Public Class ConsultCommande

#Region "Fields"
    Private _Session As Session
    Private _Commande As Commande
    Private _IsReadOnly As Boolean
    Private _ShowType
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

    Public Property IsReadOnly As Boolean
        Get
            Return Me._IsReadOnly
        End Get
        Set(ByVal value As Boolean)
            Me._IsReadOnly = value
        End Set
    End Property

    Public Property ShowType As Integer
        Get
            Return Me._ShowType
        End Get
        Set(ByVal value As Integer)
            Me._ShowType = value
        End Set
    End Property

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructeur principal de la fenêtre ConsultCommande
    ''' </summary>
    ''' <param name="session">Session servant à déterminer les droits de lecture et de modification</param>
    ''' <param name="commande">Commande à afficher</param>
    ''' <param name="search">Composant de recherche à mettre à jour lors d'une modification</param>
    ''' <param name="planning">Planning à mettre à jour lors d'une modification</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal session As Session, Optional ByVal commande As Commande = Nothing, Optional ByVal search As RechercheCommande = Nothing,
                   Optional ByVal planning As PlanningControl = Nothing)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Me.Session = session
        Dim isCanOpen As Boolean = False

        If commande IsNot Nothing And Me.Session IsNot Nothing Then
            Me.Commande = commande.GetFlag()
            If Me.Commande.Flag = 0 Then
                Me.Commande = commande.GetCommande()
                Me.Commande.UpdateFlag(Me.Session.Identifier)
                isCanOpen = True
            Else
                Me.IsReadOnly = True
                Dim s As New Session(Me.Commande.Flag)
                s = s.GetSession(True)
                Dim result As MessageBoxResult = MessageBox.Show("La commande n° " + Me.Commande.NoCommande.ToString() + " est verrouillée pour modification par « " + s.Login + " »." + vbCrLf +
                                                                 vbCrLf +
                                "Si vous ouvrez cette commande, vous ne pourrez y apporter aucune modification." + vbCrLf + "Voulez-vous ouvrir cette commande en mode lecture seule ?",
                                "Mode lecture seule", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)

                If result = MessageBoxResult.Yes Then
                    Me.Commande = Me.Commande.GetCommande()
                    isCanOpen = True
                    Me.NewCmd.IsReadOnly = True
                    Me.Title = Me.Title + "   ---   [LECTURE SEULE]"
                Else
                    Me.ShowType = 1
                    Exit Sub
                End If
            End If
        End If

        If isCanOpen Then
            Me.NewCmd.Session = Me.Session
            Me.NewCmd.IsUpdate = True
            Me.NewCmd.UC = search
            Me.NewCmd.Planning = planning
            Me.NewCmd.Commande = Me.Commande
            Me.NewCmd.Window = Me
            Me.ShowType = 0
        End If

    End Sub

#End Region

#Region "Events"

    ''' <summary>
    ''' Évènement se produisant lors de la fermeture de la fenêtre
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Window_Closed(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Me.Commande IsNot Nothing AndAlso Not Me.IsReadOnly Then
            Me.Commande.UpdateFlag(0)
        End If
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
        Dim conf As New ConfigurationWindow(Me.Session, Me.NewCmd, Me.NewCmd.Planning)
        conf.ShowDialog()
    End Sub

    ''' <summary>
    ''' Menu permettant d'afficher les informations du logiciel
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MenuItem_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim v As New Version()
        v.ShowDialog()
    End Sub

#End Region

End Class
