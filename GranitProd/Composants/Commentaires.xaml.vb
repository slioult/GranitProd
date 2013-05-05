Public Class Commentaires

#Region "DependencyProperty"

    Public Shared ReadOnly SessionProperty As DependencyProperty = DependencyProperty.Register("Session", GetType(Session), GetType(Commentaires),
                                                                                               New FrameworkPropertyMetadata(Nothing))
    Public Property Session As Session
        Get
            Return DirectCast(Me.GetValue(SessionProperty), Session)
        End Get
        Set(ByVal value As Session)
            Me.SetValue(SessionProperty, value)
        End Set
    End Property

    Public Shared ReadOnly PlanningProperty As DependencyProperty = DependencyProperty.Register("Planning", GetType(PlanningControl), GetType(Commentaires),
                                                                                               New FrameworkPropertyMetadata(Nothing))
    Public Property Planning As PlanningControl
        Get
            Return DirectCast(Me.GetValue(PlanningProperty), PlanningControl)
        End Get
        Set(ByVal value As PlanningControl)
            Me.SetValue(PlanningProperty, value)
        End Set
    End Property

#End Region

#Region "Fields"

    Private _NumCmd As Integer
    Private _Contremarque As Contremarque
    Private _Source As String
    Private _DateRem As String
    Private _Remarque As String
    Private _IdentifierCmd As Long

#End Region

#Region "Properties"

    Public Property NumCmd As Integer
        Get
            Return Me._NumCmd
        End Get
        Set(ByVal value As Integer)
            Me._NumCmd = value
        End Set
    End Property

    Public Property Contremarque As Contremarque
        Get
            Return Me._Contremarque
        End Get
        Set(ByVal value As Contremarque)
            Me._Contremarque = value
        End Set
    End Property

    Public Property Source As String
        Get
            Return Me._Source
        End Get
        Set(ByVal value As String)
            Me._Source = value
        End Set
    End Property

    Public Property DateRem As String
        Get
            Return Me._DateRem
        End Get
        Set(ByVal value As String)
            Me._DateRem = value
        End Set
    End Property

    Public Property Remarque As String
        Get
            Return Me._Remarque
        End Get
        Set(ByVal value As String)
            Me._Remarque = value
        End Set
    End Property

    Public Property IdentifierCmd As Long
        Get
            Return Me._IdentifierCmd
        End Get
        Set(ByVal value As Long)
            Me._IdentifierCmd = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Me.LoadRemarques()

    End Sub

    Public Sub New(ByVal numCmd As Integer, ByVal source As String, ByVal dateRem As String, ByVal remarque As String, ByVal identifierCmd As Long, Optional ByVal idCmq As Long = 0)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Me.NumCmd = numCmd
        If idCmq <> 0 Then Me.Contremarque = New Contremarque(idCmq).GetContremarque()
        Me.Source = source
        Me.DateRem = dateRem
        Me.Remarque = remarque
        Me.IdentifierCmd = identifierCmd
    End Sub

#End Region

#Region "EventControlEnter"

    ''' <summary>
    ''' Évènement se produisant lors du double clique sur un item du DataGrid répertoriant les enlèvements
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DgCommentaires_PreviewMouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Dim comItem As Commentaires = DgCommentaires.SelectedItem
        If comItem IsNot Nothing Then
            Dim cmd As New Commande(comItem.NumCmd)

            'Ouvre la consultation de commande
            Dim consult As New ConsultCommande(Me.Session, cmd.GetCommande(), Nothing, Me.Planning)
            If consult.ShowType = 0 Then
                consult.ShowDialog()
            Else
                consult.Close()
                consult = Nothing
            End If
        End If
        e.Handled = True
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Permet de charger les 10 dernières remarques
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadRemarques()
        DgCommentaires.Items.Clear()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))

        Try
            'Ouvre la connection
            connection.Open()

            'Exécute la requête
            Objects = connection.ExecuteQuery("SELECT r.Identifier, r.Commentaire, r.Source, r.Date, r.IdentifierCommande, c.NumCmd, c.IdentifierContremarque " +
                                              "FROM Remarque as r, Commande as c " +
                                              "WHERE r.IdentifierCommande = c.Identifier " +
                                              "Order By r.Identifier DESC LIMIT 0, 10;")
            'Ferme la connection
            connection.Close()

            'Traite les résultats
            For Each obj In Objects
                Dim comItem As New Commentaires(Integer.Parse(obj(5)), obj(2).ToString(), obj(3).ToString(), obj(1).ToString(), Long.Parse(obj(0)), Long.Parse(obj(6)))
                Me.DgCommentaires.Items.Add(comItem)
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
        Finally
            Try
                'Ferme la connection
                connection.Close()
            Catch ex As Exception
            End Try
        End Try
    End Sub

#End Region

End Class
