Imports MySql.Data.MySqlClient
Imports System.Data

Public Class SuiviQualite

#Region "Fields"

    Private _Quality As Qualite
    Private _NbrProbleme As Integer
    Private _ListCommandes As ListBox

#End Region

#Region "Properties"

    Public Property Quality As Qualite
        Get
            Return Me._Quality
        End Get
        Set(ByVal value As Qualite)
            Me._Quality = value
        End Set
    End Property

    Public Property NbrProbleme As Integer
        Get
            Return Me._NbrProbleme
        End Get
        Set(ByVal value As Integer)
            Me._NbrProbleme = value
        End Set
    End Property

    Public Property ListCommandes As ListBox
        Get
            Return Me._ListCommandes
        End Get
        Set(ByVal value As ListBox)
            Me._ListCommandes = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        For i = 2010 To Date.Now.Year
            CbxAnnee.Items.Add(i)
        Next
        CbxAnnee.SelectedItem = Date.Now.Year

        CbxMois.SelectedIndex = Date.Now.Month - 1
    End Sub

    Public Sub New(ByVal q As Qualite, ByVal nbr As Integer)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Me.Quality = q
        Me.NbrProbleme = nbr
    End Sub

#End Region

#Region "SelectionChanged"

    ''' <summary>
    ''' Se produit lorque l'item sélectionné dans la combobox mois ou année change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub CbxParam_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If CbxChoix IsNot Nothing AndAlso CbxMois IsNot Nothing AndAlso CbxAnnee IsNot Nothing Then
            DgQualite.Items.Clear()
            Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
            Dim Objects As New List(Of List(Of Object))
            Dim parameters As New List(Of MySqlParameter)
            Dim qualities As List(Of Qualite) = Qualite.GetQualites()

            Try
                'Ouvre la connection
                connection.Open()

                'Défini les paramètres de la requête
                Dim parAnnee As MySqlParameter = connection.Create("@Annee", DbType.Int32, CbxAnnee.SelectedItem)
                Dim parMois As MySqlParameter = Nothing
                Dim total As Integer = 0
                Dim query As String

                'Si l'affichage doit être trié par mois
                If CbxChoix.SelectedIndex = 0 Then
                    query = "SELECT Count(Identifier_Qualite) FROM Commande_Qualite WHERE Identifier_Qualite=@Identifier AND YEAR(DateProbleme)=@Annee AND MONTH(DateProbleme)=@Mois"
                    parMois = connection.Create("@Mois", DbType.Int32, CbxMois.SelectedIndex + 1)

                    'Si l'affichage doit être trié par année
                Else
                    query = "SELECT Count(Identifier_Qualite) FROM Commande_Qualite WHERE Identifier_Qualite=@Identifier AND YEAR(DateProbleme)=@Annee"
                End If

                'Parcours la liste de toutes les qualités existantes afin de les ajouté dans le DataGrid avec le nombre de fois qu'elles sont recensées
                For Each q In qualities
                    'Défini les paramètres de la requête
                    parameters.Add(parAnnee)
                    If CbxChoix.SelectedIndex = 0 Then parameters.Add(parMois)

                    Dim parIdentifier As MySqlParameter = connection.Create("@Identifier", DbType.Int64, q.Identifier)
                    parameters.Add(parIdentifier)

                    'Exécute la requête
                    Objects = connection.ExecuteQuery(query, parameters)

                    parameters.Clear()

                    'Traite les résultats
                    For Each obj In Objects
                        DgQualite.Items.Add(New SuiviQualite(q, Integer.Parse(obj(0))))
                        total += Integer.Parse(obj(0))
                    Next
                Next

                'Affiches le total de problèmes recensés
                Me.TxtTotal.Text = total.ToString()

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

    ''' <summary>
    ''' Se produit lorsque l'item sélectionné dans la combobox CbxChoix
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CbxChoix_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        'Met à jour l'affichage en fonction du choix effectué par l'utilisateur
        If CbxChoix IsNot Nothing AndAlso CbxMois IsNot Nothing AndAlso CbxAnnee IsNot Nothing Then
            If CbxChoix.SelectedIndex = 0 Then
                CbxMois.Visibility = Windows.Visibility.Visible
                CbxMois.SelectedIndex = Date.Now.Month - 1
            Else
                CbxMois.Visibility = Windows.Visibility.Collapsed
                CbxAnnee.SelectedItem = Date.Now.Year
            End If
        End If

        'Mets à jour les données contenues dans le DataGrid
        Me.CbxParam_SelectionChanged(Nothing, Nothing)
    End Sub

#End Region

#Region "EnventControlEnter"

    ''' <summary>
    ''' Évènement se produisant lors du double clique sur un item du suivi qualité
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DgQualite_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        If DgQualite.SelectedItem IsNot Nothing And Me.ListCommandes IsNot Nothing Then
            ListCommandes.Items.Clear()
            'Récupère l'item sélectionné dans le DataGrid
            Dim sq As SuiviQualite = DgQualite.SelectedItem
            Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
            Dim parameters As New List(Of MySqlParameter)
            Dim Objects As New List(Of List(Of Object))
            Dim query As String

            Try
                'Ouvre la connection
                connection.Open()

                'Défini les paramètres de la requête
                Dim parIdentifierQualite As MySqlParameter = connection.Create("@Identifier", DbType.Int64, sq.Quality.Identifier)
                parameters.Add(parIdentifierQualite)

                Dim parAnnee As MySqlParameter = connection.Create("@Year", DbType.Int32, CbxAnnee.SelectedItem)
                parameters.Add(parAnnee)

                If Me.CbxChoix.SelectedIndex = 0 Then
                    Dim parMois As MySqlParameter = connection.Create("@Mois", DbType.Int32, CbxMois.SelectedIndex + 1)
                    parameters.Add(parMois)

                    'Requête
                    query = "SELECT DISTINCT c.NumCmd " +
                                          "FROM Commande as c, Commande_Qualite as cq " +
                                          "WHERE cq.Identifier_Qualite=@Identifier AND MONTH(cq.DateProbleme)=@Mois AND YEAR(cq.DateProbleme)=@Year AND cq.Identifier_Commande=c.Identifier"
                Else
                    'Requête
                    query = "SELECT DISTINCT c.NumCmd " +
                                          "FROM Commande as c, Commande_Qualite as cq " +
                                          "WHERE cq.Identifier_Qualite=@Identifier AND YEAR(cq.DateProbleme)=@Year AND cq.Identifier_Commande=c.Identifier"

                End If

                'Exécute la requête
                Objects = connection.ExecuteQuery(query, parameters)

                'Ferme la connection
                connection.Close()

                parameters = Nothing

                'Traite les résultats
                For Each obj In Objects
                    ListCommandes.Items.Add(New Commande(Integer.Parse(obj(0))).GetCommande())
                Next
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
            Finally
                'Ferme la connection
                connection.Close()
            End Try
        End If
    End Sub

#End Region

End Class
