Imports MySql.Data.MySqlClient
Imports System.Data

Public Class Enlevement

#Region "Fields"

    Private _NumCmd As Integer
    Private _DatePrest As DateTime
    Private _Heure As String
    Private _Prestations As List(Of Finalisation)
    Private _Session As Session
    Private _Planning As PlanningControl

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

    Public Property DatePrest As DateTime
        Get
            Return Me._DatePrest
        End Get
        Set(ByVal value As DateTime)
            Me._DatePrest = value
        End Set
    End Property

    Public Property Heure As String
        Get
            Return Me._Heure
        End Get
        Set(ByVal value As String)
            Me._Heure = value
        End Set
    End Property

    Public Property Prestations As List(Of Finalisation)
        Get
            Return Me._Prestations
        End Get
        Set(ByVal value As List(Of Finalisation))
            Me._Prestations = value
        End Set
    End Property

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

    Public Sub New(ByVal cmd As Commande)
        InitializeComponent()

        Me.NumCmd = cmd.NoCommande
        Me.DatePrest = cmd.DateFinalisations
        Me.Prestations = cmd.Finalisations
        Dim m As String = IIf(Me.DatePrest.Minute > 9, Me.DatePrest.Minute.ToString(), "0" + Me.DatePrest.Minute.ToString())
        Me.Heure = Me.DatePrest.Hour.ToString() + "h" + m

    End Sub

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        For i = 2010 To Date.Now.Year + 2
            CbxAnnee.Items.Add(i)
        Next
        CbxAnnee.SelectedItem = Date.Now.Year

        For i = 1 To 53
            CbxSemaine.Items.Add(i)
        Next
        Dim pl As New PlanningControl()
        CbxSemaine.SelectedIndex = pl.GetWeekOfDate(Date.Now) - 1

    End Sub

#End Region

#Region "SelectionChanged"

    ''' <summary>
    ''' Se produit lorsque la semaine choisie change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub CbxDate_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Me.DgEnlevement.Items.Clear()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))
        Dim parameters As New List(Of MySqlParameter)

        If Me.CbxAnnee.SelectedItem IsNot Nothing And Me.CbxSemaine.SelectedItem IsNot Nothing Then
            Try
                'Ouvre la connection
                connection.Open()

                Dim pl As New PlanningControl

                'Récupère la liste des jours de la semaine sélectionnée
                Dim dates As List(Of Date) = pl.GetDaysOfWeek(Me.CbxSemaine.SelectedItem, Me.CbxAnnee.SelectedItem)

                'Parcours cette liste
                For Each d In dates
                    'Défini le paramètre date de la requête
                    Dim parDate As MySqlParameter = connection.Create("@Date", DbType.DateTime, d)
                    parameters.Add(parDate)

                    'Requête
                    Dim query As String = "SELECT NumCmd FROM Commande " +
                        "WHERE DAY(DateFinalisations)=DAY(@Date) And MONTH(DateFinalisations)=MONTH(@Date) And YEAR(DateFinalisations)=YEAR(@Date)"

                    'Exécute la requête
                    Objects = connection.ExecuteQuery(query, parameters)

                    parameters.Clear()

                    'Traite les résultats
                    For Each obj In Objects
                        Dim enlv As New Enlevement(New Commande(Integer.Parse(obj(0))).GetCommande())
                        Me.DgEnlevement.Items.Add(enlv)
                    Next
                Next

                'Ferme la connection
                connection.Close()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
            Finally
                Try
                    'Assure la fermeture de la connection
                    connection.Close()
                Catch
                End Try
            End Try
        End If
    End Sub

#End Region

#Region "EnventControlEnter"

    ''' <summary>
    ''' Évènement se produisant lors du double clique sur un item du DataGrid répertoriant les enlèvements
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DgEnlevement_PreviewMouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Dim comEnlevement As Enlevement = DgEnlevement.SelectedItem
        Dim cmd As New Commande(comEnlevement.NumCmd)

        'Ouvre une consultation de commande
        Dim consult As New ConsultCommande(Me.Session, cmd.GetCommande(), Nothing, Me.Planning)
        consult.Show()
        e.Handled = True
    End Sub

#End Region

End Class
