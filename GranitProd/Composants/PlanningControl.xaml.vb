Imports System.ComponentModel
Imports System.Globalization
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class PlanningControl

#Region "Fields"

    Private ListOfDays As List(Of Date)
    Private _SelectDate As Date

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

#End Region

#Region "Constructor"

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        ListOfDays = New List(Of Date)

        Dim mes As List(Of Mesure) = Mesure.GetMesures()

        For Each m In mes
            LbxLengendeMesure.Items.Add(m)
        Next

        Dim fin As List(Of Finalisation) = Finalisation.GetFinalisations()

        For Each f In fin
            LbxLengendeFinalisation.Items.Add(f)
        Next

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
            Dim grid As Grid = Me.Parent
            Dim main As MainWindow = grid.Parent
            Dim consult As New ConsultCommande(main.Session, commande, main.SearchCommande, Me)
            consult.Show()
        End If
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Permet de remplir le planning
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Fill()
        Dim sem As Integer = GetWeekOfDate(SelectDate)
        Dim days As List(Of Date) = GetDaysOfWeek(sem, SelectDate.Year)
        Me.ListOfDays.Clear()

        For Each d In days
            Me.ListOfDays.Add(d)
        Next

        TxtWeek.Text = sem

        While (days.Count <> 7)
            If (days.ElementAt(0).DayOfWeek <> 1) Then
                days.Insert(0, New Date(1, 1, 1))
            Else
                days.Add(New Date(1, 1, 1))
            End If
        End While

        TxtLundi.Text = IIf(days.ElementAt(0).Equals(New Date(1, 1, 1)) = False, days.ElementAt(0).Day, String.Empty)
        TxtMardi.Text = IIf(days.ElementAt(1).Equals(New Date(1, 1, 1)) = False, days.ElementAt(1).Day, String.Empty)
        TxtMercredi.Text = IIf(days.ElementAt(2).Equals(New Date(1, 1, 1)) = False, days.ElementAt(2).Day, String.Empty)
        TxtJeudi.Text = IIf(days.ElementAt(3).Equals(New Date(1, 1, 1)) = False, days.ElementAt(3).Day, String.Empty)
        TxtVendredi.Text = IIf(days.ElementAt(4).Equals(New Date(1, 1, 1)) = False, days.ElementAt(4).Day, String.Empty)

        LoadCommande()

    End Sub

    ''' <summary>
    ''' Permet le numéro d'une semaine à partir d'une date
    ''' </summary>
    ''' <param name="d">Date contenue dans la semaine</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetWeekOfDate(ByVal d As Date) As Integer
        Dim semaine As Integer = DatePart(DateInterval.WeekOfYear, d)

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
    ''' <returns></returns>
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
            Dim monthDays = gcal.GetDaysInMonth(year, month)
            Dim firstDayOfYear As Integer = gcal.GetDayOfWeek(d)

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

            Dim dayOfWeek As Integer = gcal.GetDayOfWeek(d)
            If (day <> 1) Then
                day = day - dayOfWeek + 1
            End If

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
    ''' <remarks></remarks>
    Public Sub LoadCommande()
        Try
            Dim Dates As New List(Of Date)
            Dim lundi As Integer = IIf(Integer.TryParse(TxtLundi.Text, 0), Integer.Parse(TxtLundi.Text), 0)
            Dim mardi As Integer = IIf(Integer.TryParse(TxtMardi.Text, 0), Integer.Parse(TxtMardi.Text), 0)
            Dim mercredi As Integer = IIf(Integer.TryParse(TxtMercredi.Text, 0), Integer.Parse(TxtMercredi.Text), 0)
            Dim jeudi As Integer = IIf(Integer.TryParse(TxtJeudi.Text, 0), Integer.Parse(TxtJeudi.Text), 0)
            Dim vendredi As Integer = IIf(Integer.TryParse(TxtVendredi.Text, 0), Integer.Parse(TxtVendredi.Text), 0)
            Me.LbxLundi.Items.Clear()
            Me.LbxMardi.Items.Clear()
            Me.LbxMercredi.Items.Clear()
            Me.LbxJeudi.Items.Clear()
            Me.LbxVendredi.Items.Clear()

            For Each d In Me.ListOfDays
                If (d.Day = lundi Or d.Day = mardi Or d.Day = mercredi Or d.Day = jeudi Or d.Day = vendredi) Then
                    Dates.Add(d)
                End If
            Next

            Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
            Dim Objects As New List(Of List(Of Object))
            Dim parameters As New List(Of MySqlParameter)
            Dim query As String

            Try
                connection.Open()

                For Each d In Dates
                    Dim borderLundi As String = "0"
                    Dim borderMardi As String = "0"
                    Dim borderMercredi As String = "0"
                    Dim borderJeudi As String = "0"
                    Dim borderVendredi As String = "0"
                    Dim parDate As MySqlParameter = connection.Create("@parDate", DbType.DateTime, d)
                    parameters.Add(parDate)
                    query = "SELECT NumCmd FROM Commande WHERE DAY(DateFinalisations)=DAY(@parDate) AND MONTH(DateFinalisations)=MONTH(@parDate) AND YEAR(DateFinalisations)=YEAR(@parDate) " +
                        "Or DAY(DateMesure)=DAY(@parDate) AND MONTH(DateMesure)=MONTH(@parDate) AND YEAR(DateMesure)=YEAR(@parDate)"
                    Objects = connection.ExecuteQuery(query, parameters)

                    parameters.Clear()

                    For Each obj In Objects
                        Select Case d.Day
                            Case lundi
                                Dim cmd As Commande = New Commande(Long.Parse(obj(0))).GetCommande()
                                Me.LbxLundi.Items.Add(New CommandeWork(cmd, d, borderLundi))
                                borderLundi = "1, 0, 0, 0"
                            Case mardi
                                Dim cmd As Commande = New Commande(Long.Parse(obj(0))).GetCommande()
                                Me.LbxMardi.Items.Add(New CommandeWork(cmd, d, borderMardi))
                                borderMardi = "1, 0, 0, 0"
                            Case mercredi
                                Dim cmd As Commande = New Commande(Long.Parse(obj(0))).GetCommande()
                                Dim cmdw As CommandeWork = New CommandeWork(cmd, d, borderMercredi)
                                Me.LbxMercredi.Items.Add(cmdw)
                                borderMercredi = "1, 0, 0, 0"
                            Case jeudi
                                Dim cmd As Commande = New Commande(Long.Parse(obj(0))).GetCommande()
                                Me.LbxJeudi.Items.Add(New CommandeWork(cmd, d, borderJeudi))
                                borderJeudi = "1, 0, 0, 0"
                            Case vendredi
                                Dim cmd As Commande = New Commande(Long.Parse(obj(0))).GetCommande()
                                Me.LbxVendredi.Items.Add(New CommandeWork(cmd, d, borderVendredi))
                                borderVendredi = "1, 0, 0, 0"
                        End Select
                    Next
                Next

                connection.Close()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
            Finally
                Try
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