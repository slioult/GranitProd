Public Class Panel

#Region "Fields"

    Private _Session As Session
    Private _Planning As PlanningControl

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

    Public Property Planning As PlanningControl
        Get
            Return Me._Planning
        End Get
        Set(ByVal value As PlanningControl)
            Me._Planning = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal session As Session, ByVal planning As PlanningControl)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Me.Session = session
        Me.Planning = planning

        Me.Commentaire.Session = Me.Session
        Me.Commentaire.Planning = Me.Planning
        Me.Enlevement.Session = Me.Session
        Me.Enlevement.Planning = Me.Planning
        Me.CAffaire.ListCommandes = Me.LbxDisplayCommandes
        Me.TpsFabrication.ListCommandes = Me.LbxDisplayCommandes
        Me.SqQualite.ListCommandes = Me.LbxDisplayCommandes
    End Sub

#End Region

#Region "EnventControlEnter"

    ''' <summary>
    ''' Évènement se produisant lors d'un double clique sur la listbox LbxDisplayCommandes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub LbxDisplayCommandes_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        If LbxDisplayCommandes.SelectedItem IsNot Nothing Then
            Dim commande As Commande = LbxDisplayCommandes.SelectedItem

            Dim consult As New ConsultCommande(Me.Session, commande, Nothing, Me.Planning)
            consult.ShowDialog()

            Me.BtnRefresh_Click(Nothing, Nothing)
        End If
    End Sub

#End Region

#Region "Button"

    ''' <summary>
    ''' Évènement se produisant lors du clique sur le bouton refresh
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnRefresh_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.CAffaire.CbxParam_SelectionChanged(Nothing, Nothing)
        Me.TpsFabrication.CbxParam_SelectionChanged(Nothing, Nothing)
        Me.SqQualite.CbxParam_SelectionChanged(Nothing, Nothing)
        Me.Commentaire.LoadRemarques()
        Me.Enlevement.CbxDate_SelectionChanged(Nothing, Nothing)
        Me.LbxDisplayCommandes.Items.Clear()
    End Sub

#End Region

End Class
