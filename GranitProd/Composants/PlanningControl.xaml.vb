Imports System.ComponentModel
Imports System.Globalization
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class PlanningControl

#Region "Fields"

    Private ListOfDays As List(Of Date)
    Private _SelectDate As Date
    Private _Session As Session
    Private _Search As RechercheCommande

#End Region

#Region "Properties"

    Public Property SelectDate As Date
        Get
            Return Me._SelectDate
        End Get
        Set(ByVal value As Date)
            Me._SelectDate = value
            Me.Fill()
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

    Public Property Search As RechercheCommande
        Get
            Return Me._Search
        End Get
        Set(ByVal value As RechercheCommande)
            Me._Search = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        ListOfDays = New List(Of Date)
        Me.cal.SelectedDate = Date.Now

    End Sub

#End Region

#Region "MouseClick"

    ''' <summary>
    ''' Ouvre la commande sélectionnée dans le planning lors d'un double clique sur celle-ci
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Commande_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Dim lbx As ListBox = sender

        If lbx.SelectedItem IsNot Nothing Then
            Dim commande As Commande = lbx.SelectedItem

            'Ouvre une consultation de commande
            Dim consult As New ConsultCommande(Me.Session, commande, Me.Search, Me)
            If consult.ShowType = 0 Then
                consult.Show()
            Else
                consult.Close()
                consult = Nothing
            End If
        End If
    End Sub

#End Region

#Region "Button"

    ''' <summary>
    ''' Bouton permettant de rafraîchir le planning. Et ainsi mettre à jour des données ayant pu être ajoutées ou modifiées par un autre utilisateur
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnRefresh_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'Rafraîchi les données contenues dans le planning
        Me.Fill()
    End Sub

    ''' <summary>
    ''' Bouton permettant d'agrandir le planning
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnExtend_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim pv As New PlanningView(Me, Me.Session, Me.SelectDate)
        pv.Show()
    End Sub

#End Region

#Region "SelectionChanged"

    ''' <summary>
    ''' Évènement se produisant lors de la modification du paramètre d'affichage du planning
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CbxTri_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If Me.ListOfDays IsNot Nothing Then
            Me.Fill()
        End If
    End Sub

    ''' <summary>
    ''' Action se produisant lorsque la date sélectionnée dans le calendrier change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cal_SelectedDatesChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Me.SelectDate = cal.SelectedDate
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Permet de remplir le planning
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Fill()
        Dim sem As Integer = GetWeekOfDate(SelectDate)

        'Récupère les jours de la semaine sélectionnée
        Dim days As List(Of Date) = GetDaysOfWeek(sem, SelectDate.Year)
        Me.ListOfDays.Clear()

        'Remplit la liste ListOfDay avec les jours correspondant à la semaine sélectionnée
        For Each d In days
            Me.ListOfDays.Add(d)
        Next

        TxtWeek.Text = sem

        'Complète la semaine si elle ne possède pas 7 jours
        While (days.Count <> 7)
            If (days.ElementAt(0).DayOfWeek <> 1) Then
                days.Insert(0, New Date(1, 1, 1))
            Else
                days.Add(New Date(1, 1, 1))
            End If
        End While

        'Remplit les numéros des jours contenus dans le planning
        TxtLundi.Text = IIf(days.ElementAt(0).Equals(New Date(1, 1, 1)) = False, days.ElementAt(0).Day, String.Empty)
        TxtMardi.Text = IIf(days.ElementAt(1).Equals(New Date(1, 1, 1)) = False, days.ElementAt(1).Day, String.Empty)
        TxtMercredi.Text = IIf(days.ElementAt(2).Equals(New Date(1, 1, 1)) = False, days.ElementAt(2).Day, String.Empty)
        TxtJeudi.Text = IIf(days.ElementAt(3).Equals(New Date(1, 1, 1)) = False, days.ElementAt(3).Day, String.Empty)
        TxtVendredi.Text = IIf(days.ElementAt(4).Equals(New Date(1, 1, 1)) = False, days.ElementAt(4).Day, String.Empty)

        'Charge les commandes correspondant à cette semaine
        LoadCommande(Me.CbxTri.SelectedIndex)

        'Charge les types de relevés dans la légende
        Dim mes As List(Of Mesure) = Mesure.GetLegendMesures()

        LbxLengendeMesure.ItemsSource = mes

        'Charge les types de prestation dans la légende
        Dim fin As List(Of Finalisation) = Finalisation.GetLegendFinalisations()

        LbxLengendeFinalisation.ItemsSource = fin

    End Sub

    ''' <summary>
    ''' Permet de récupérer le numéro d'une semaine à partir d'une date
    ''' </summary>
    ''' <param name="d">Date contenue dans la semaine</param>
    ''' <returns>Retourne un numéro de semaine</returns>
    ''' <remarks></remarks>
    Public Function GetWeekOfDate(ByVal d As Date) As Integer
        Dim semaine As Integer = DatePart(DateInterval.WeekOfYear, d)

        'Permet de faire commencer la semaine le Lundi et non le Dimanche
        If (Convert.ToDateTime(d).DayOfWeek = DayOfWeek.Sunday) Then
            semaine -= 1
        End If

        Return semaine
    End Function

    ''' <summary>
    ''' Permet de récupérer les jours d'une semaine
    ''' </summary>
    ''' <param name="semaine">Numéro de la semaine</param>
    ''' <param name="year">Année de la semaine</param>
    ''' <returns>Retourne une liste de dates</returns>
    ''' <remarks></remarks>
    Public Function GetDaysOfWeek(ByVal semaine As Integer, ByVal year As Integer) As List(Of Date)
        GetDaysOfWeek = New List(Of Date)
        Try
            Dim gcal As New GregorianCalendar()
            gcal.CalendarType = GregorianCalendarTypes.TransliteratedFrench
            Dim semTest As Integer = 1
            Dim month As Integer = 1
            Dim day As Integer = 1
            Dim d As New Date(year, month, day)
            'Récupère le nombre de jours dans le mois passé en paramètre
            Dim monthDays = gcal.GetDaysInMonth(year, month)
            Dim firstDayOfYear As Integer = gcal.GetDayOfWeek(d)

            'Récupère un jour de la semaine passée en paramètre (pas nécessairement le premier jour)
            If (semaine <> 1) Then
                While (semTest < semaine)
                    day += 7

                    If (day > monthDays) Then
                        day = day - monthDays
                        month += 1
                        monthDays = gcal.GetDaysInMonth(year, month)
                    End If

                    semTest += 1
                End While
            End If

            'Récupère le premier jour de la semaine
            Dim dayOfWeek As Integer = gcal.GetDayOfWeek(d)
            If (day <> 1) Then
                day = day - dayOfWeek + 1
            End If

            'Récupère les 5 premiers jours de la semaine. (En gérant les problème de semaine incomplète comme la première et la dernière de l'année)
            Try
                d = New Date(year, month, day)

                GetDaysOfWeek.Add(d)

                For i = 0 To 5
                    If (d.Day < (monthDays)) Then
                        d = New Date(year, month, d.Day + 1)
                    ElseIf (d.Month <> 12) Then
                        month += 1
                        monthDays = Date.DaysInMonth(year, month)
                        d = New Date(year, month, 1)
                    Else
                        Exit For
                    End If

                    GetDaysOfWeek.Add(d)

                    If (d.DayOfWeek = 0) Then Exit For
                Next
            Catch ex As Exception
                Throw ex
            End Try

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try

        Return GetDaysOfWeek
    End Function

    ''' <summary>
    ''' Charge les commandes
    ''' </summary>
    ''' <param name="sorted">Type de tri -- 0 > Délai prévu -- 1 > Date de commande</param>
    ''' <remarks></remarks>
    Public Sub LoadCommande(ByVal sorted As Integer)
        Try
            Dim Dates As New List(Of Date)
            ' Récupère les n° de jour de la semaine
            Dim lundi As Integer
            Dim mardi As Integer
            Dim mercredi As Integer
            Dim jeudi As Integer
            Dim vendredi As Integer

            If Integer.TryParse(TxtLundi.Text, 0) Then
                lundi = Integer.Parse(TxtLundi.Text)
            Else
                lundi = 0
            End If

            If Integer.TryParse(TxtMardi.Text, 0) Then
                mardi = Integer.Parse(TxtMardi.Text)
            Else
                mardi = 0
            End If

            If Integer.TryParse(TxtMercredi.Text, 0) Then
                mercredi = Integer.Parse(TxtMercredi.Text)
            Else
                mercredi = 0
            End If

            If Integer.TryParse(TxtJeudi.Text, 0) Then
                jeudi = Integer.Parse(TxtJeudi.Text)
            Else
                jeudi = 0
            End If

            If Integer.TryParse(TxtVendredi.Text, 0) Then
                vendredi = Integer.Parse(TxtVendredi.Text)
            Else
                vendredi = 0
            End If
            Me.LbxLundi.Items.Clear()
            Me.LbxMardi.Items.Clear()
            Me.LbxMercredi.Items.Clear()
            Me.LbxJeudi.Items.Clear()
            Me.LbxVendredi.Items.Clear()

            'Associe les numéros de jour à des dates
            For Each d In Me.ListOfDays
                If (d.Day = lundi Or d.Day = mardi Or d.Day = mercredi Or d.Day = jeudi Or d.Day = vendredi) Then
                    Dates.Add(d)
                End If
            Next

            'Initialise la connection à la base de données
            Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
            Dim Objects As New List(Of List(Of Object))
            Dim parameters As New List(Of MySqlParameter)
            Dim query As String

            Try
                'Ouvre la connection
                connection.Open()

                'Parcours la liste de dates correspondant aux différents jours de la semaine sélectionnée
                For Each d In Dates
                    Dim borderLundi As String = "0"
                    Dim borderMardi As String = "0"
                    Dim borderMercredi As String = "0"
                    Dim borderJeudi As String = "0"
                    Dim borderVendredi As String = "0"

                    'Défini les paramètres de la requête
                    Dim parDate As MySqlParameter = connection.Create("@parDate", DbType.DateTime, d)
                    parameters.Add(parDate)

                    'Requête
                    If sorted <> 1 Then
                        query = "SELECT NumCmd FROM Commande WHERE DAY(DateFinalisations)=DAY(@parDate) AND MONTH(DateFinalisations)=MONTH(@parDate) AND YEAR(DateFinalisations)=YEAR(@parDate) " +
                            "Or DAY(DateMesure)=DAY(@parDate) AND MONTH(DateMesure)=MONTH(@parDate) AND YEAR(DateMesure)=YEAR(@parDate)"
                    Else
                        query = "SELECT NumCmd FROM Commande WHERE DAY(DateCommande)=DAY(@parDate) AND MONTH(DateCommande)=MONTH(@parDate) AND YEAR(DateCommande)=YEAR(@parDate)"
                    End If

                    'Exécute la requête
                    Objects = connection.ExecuteQuery(query, parameters)

                    parameters.Clear()

                    'Traite les résultats
                    For Each obj In Objects
                        Dim isDisplay As Boolean = True
                        Dim cmd As Commande = New Commande(Long.Parse(obj(0))).GetCommande()

                        'Vérifie si la commande obtenue doit être affichée dans le planning
                        If d.Year = cmd.DateMesure.Year And d.Month = cmd.DateMesure.Month And d.Day = cmd.DateMesure.Day Then
                            If Not cmd.Mesure.Display Then isDisplay = False
                        End If

                        If cmd.Etat.Label = "Rendue" Then
                            isDisplay = False
                        End If

                        'S'exécute si la commande doit être affichée dans le planning
                        If isDisplay Then
                            'Insert la commande dans le planning le bon jour en fonction de la date
                            Select Case d.Day
                                Case lundi
                                    Me.LbxLundi.Items.Add(New CommandeWork(cmd, d, borderLundi))
                                    borderLundi = "1, 0, 0, 0"
                                Case mardi
                                    Me.LbxMardi.Items.Add(New CommandeWork(cmd, d, borderMardi))
                                    borderMardi = "1, 0, 0, 0"
                                Case mercredi
                                    Dim cmdw As CommandeWork = New CommandeWork(cmd, d, borderMercredi)
                                    Me.LbxMercredi.Items.Add(cmdw)
                                    borderMercredi = "1, 0, 0, 0"
                                Case jeudi
                                    Me.LbxJeudi.Items.Add(New CommandeWork(cmd, d, borderJeudi))
                                    borderJeudi = "1, 0, 0, 0"
                                Case vendredi
                                    Me.LbxVendredi.Items.Add(New CommandeWork(cmd, d, borderVendredi))
                                    borderVendredi = "1, 0, 0, 0"
                            End Select
                        End If
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


        Catch ex As Exception
            MessageBox.Show("Les commandes n'ont pas pu être chargées", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

#End Region

End Class