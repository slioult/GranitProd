Imports MySql.Data.MySqlClient
Imports System.Data

Imports System.Globalization

Public Class CA

#Region "Fields"

    Private _ListCommandes As ListBox

#End Region

#Region "Properties"

    Public Property ListCommandes As ListBox
        Get
            Return Me._ListCommandes
        End Get
        Set(ByVal value As ListBox)
            Me._ListCommandes = value
        End Set
    End Property

#End Region

#Region "Contructor"

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        CbxChoix.Items.Add("Semaine")
        CbxChoix.Items.Add("Mois")
        CbxChoix.Items.Add("Année")
        CbxChoix.SelectedIndex = 1

        Dim months As List(Of String) = New List(Of String)(New String() {"Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre",
                                                                                 "Novembre", "Décembre"})
        For Each e In months
            CbxChoixMonth.Items.Add(e)
        Next
        CbxChoixMonth.SelectedIndex = Date.Now.Month - 1

        For i = 2010 To Date.Now.Year + 2
            CbxChoixYear.Items.Add(i)
        Next
        CbxChoixYear.SelectedItem = Date.Now.Year

        For i = 1 To 53
            CbxChoixWeek.Items.Add(i)
        Next

        Dim pl As New PlanningControl(True)
        CbxChoixWeek.SelectedIndex = pl.GetWeekOfDate(Date.Now) - 1
        pl = Nothing
    End Sub

#End Region

#Region "SelectionChanged"

    ''' <summary>
    ''' Évènement se produisant lorsque le choix d'affichage change (par semaine, par mois ou par année)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CbxChoix_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        'Met à jour l'interface graphique en fonction du choix d'affichage de l'utilisateur
        If (CbxChoix.SelectedIndex = 0) Then
            CbxChoixWeek.Visibility = Windows.Visibility.Visible
            CbxChoixMonth.Visibility = Windows.Visibility.Collapsed
            CbxChoixMonth.SelectedIndex = Date.Now.Month - 1
            CbxChoixYear.SelectedItem = Date.Now.Year
            DgCa.Columns(0).Header = "Jour"
        ElseIf (CbxChoix.SelectedIndex = 1) Then
            CbxChoixMonth.Visibility = Windows.Visibility.Visible
            CbxChoixWeek.Visibility = Windows.Visibility.Collapsed
            Dim pl As New PlanningControl(True)
            CbxChoixWeek.SelectedIndex = pl.GetWeekOfDate(Date.Now) - 1
            pl = Nothing
            CbxChoixYear.SelectedItem = Date.Now.Year
            DgCa.Columns(0).Header = "Semaine"
        Else
            CbxChoixWeek.Visibility = Windows.Visibility.Collapsed
            CbxChoixMonth.Visibility = Windows.Visibility.Collapsed
            CbxChoixMonth.SelectedIndex = Date.Now.Month - 1
            Dim pl As New PlanningControl(True)
            CbxChoixWeek.SelectedIndex = pl.GetWeekOfDate(Date.Now) - 1
            pl = Nothing
            CbxChoixYear.SelectedItem = Date.Now.Year
            DgCa.Columns(0).Header = "Mois"
        End If

        'Met à jour les données contenues dans le DataGrid
        Me.CbxParam_SelectionChanged(Nothing, Nothing)
    End Sub

    ''' <summary>
    ''' Paramètre d'affichage du CA change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub CbxParam_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Me.DgCa.Items.Clear()
        Dim Objects As New List(Of List(Of Object))
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection
            connection.Open()

            'Choix par semaine
            If CbxChoix.SelectedIndex = 0 Then
                Dim pl As New PlanningControl(True)
                Dim total As Decimal = 0

                'Récupère les jours de la semaine sélectionnée
                Dim semaine As List(Of Date) = pl.GetDaysOfWeek(CbxChoixWeek.SelectedItem, CbxChoixYear.SelectedItem)

                'Parcours les jours de la semaine
                For Each d In semaine
                    'Défini les paramètres de la requête
                    Dim parDate As MySqlParameter = connection.Create("@Date", DbType.DateTime, d)
                    parameters.Add(parDate)

                    'Exécute la requête
                    Objects = connection.ExecuteQuery("SELECT NumCmd, Montant FROM Commande WHERE DelaiPrevu=@Date", parameters)

                    'Vérifie que la requête a retrourné au moins 1 résultat
                    If Objects.Count > 0 Then
                        Dim chiffre As Decimal
                        Dim listCmd As New List(Of Commande)

                        'Traite les résultats puis ajoute le montant de chaque commande au chiffre d'affaire de chaque journée. ainsi que les n° des commandes concernées
                        For Each obj In Objects
                            chiffre += Decimal.Parse(obj(1))
                            listCmd.Add(New Commande(Integer.Parse(obj(0))))
                        Next

                        'Ajoute un item par jour correspondant au délai prévu d'au moins une commande, puis ajoute son montant au CA total de la semaine
                        Dim caItem As New CaItem(d.ToString("dddd dd MMMM", New CultureInfo("fr-FR")), chiffre, listCmd)
                        DgCa.Items.Add(caItem)
                        total += caItem.Chiffre
                        chiffre = 0
                        listCmd = New List(Of Commande)
                    End If

                    parameters.Clear()
                Next

                'Affiche le CA total de la semaine
                If total > 0 Then
                    Dim cc As New ChiffreConverter
                    TxtTotal.Text = cc.Convert(total.ToString(), Nothing, Nothing, Nothing)
                Else
                    TxtTotal.Text = "0,00 €"
                End If

                'Choix d'affichage par mois
            ElseIf CbxChoix.SelectedIndex = 1 And CbxChoixYear.SelectedItem IsNot Nothing Then
                Dim cal As New GregorianCalendar
                Dim pl As New PlanningControl(True)
                'Récupère le nombre du jours dans le mois sélectionné
                Dim days As Integer = cal.GetDaysInMonth(CbxChoixYear.SelectedItem, CbxChoixMonth.SelectedIndex + 1)
                Dim month As Integer = CbxChoixMonth.SelectedIndex + 1
                Dim year As Integer = CbxChoixYear.SelectedItem
                Dim tempSem As Integer = 0
                Dim chiffre As Decimal = 0
                Dim total As Decimal = 0
                Dim listCmd As New List(Of Commande)

                'Parcours tous les jours du mois
                For i = 1 To days
                    Dim d As New DateTime(year, month, i)
                    'Récupère le numéro de semaine d'une date
                    Dim sem As Integer = pl.GetWeekOfDate(d)

                    'Défini les paramètres de la requête
                    Dim parDate As MySqlParameter = connection.Create("@Date", DbType.DateTime, d)
                    parameters.Add(parDate)

                    'Exécute la requête
                    Objects = connection.ExecuteQuery("SELECT NumCmd, Montant FROM Commande WHERE DelaiPrevu=@Date", parameters)

                    'Ajoute un item par semaine en calculant le chiffre d'affaire de chaque semaine et les n° de commandes correspondant à cette semaine. Ajoute également le CA au CA total du mois
                    'If i = days Then
                    '    Dim caItem As New CaItem(tempSem, chiffre, listCmd)
                    '    DgCa.Items.Add(caItem)
                    '    total += chiffre
                    '    chiffre = 0
                    '    listCmd = New List(Of Commande)
                    If sem = tempSem And tempSem <> 0 Then
                        For Each obj In Objects
                            chiffre += Decimal.Parse(obj(1))
                            listCmd.Add(New Commande(Integer.Parse(obj(0))))
                        Next

                        If i = days Then
                            Dim caItem As New CaItem(tempSem, chiffre, listCmd)
                            DgCa.Items.Add(caItem)
                            total += chiffre
                            chiffre = 0
                            listCmd = New List(Of Commande)
                        End If
                    ElseIf tempSem <> 0 Then
                        Dim caItem As New CaItem(tempSem, chiffre, listCmd)
                        DgCa.Items.Add(caItem)
                        tempSem = sem
                        total += chiffre
                        chiffre = 0
                        listCmd = New List(Of Commande)

                        For Each obj In Objects
                            chiffre += Decimal.Parse(obj(1))
                            listCmd.Add(New Commande(Integer.Parse(obj(0))))
                        Next
                    Else
                        For Each obj In Objects
                            chiffre += Decimal.Parse(obj(1))
                            listCmd.Add(New Commande(Integer.Parse(obj(0))))
                        Next
                        tempSem = sem
                    End If

                    parameters.Clear()
                Next

                'Affiche le CA du mois
                If total > 0 Then
                    Dim cc As New ChiffreConverter
                    TxtTotal.Text = cc.Convert(total.ToString(), Nothing, Nothing, Nothing)
                Else
                    TxtTotal.Text = "0,00 €"
                End If

                'Choix par année
            ElseIf CbxChoix.SelectedIndex = 2 Then
                Dim year = CbxChoixYear.SelectedItem
                Dim total As Decimal = 0
                Dim chiffre As Decimal = 0
                Dim listCmd As New List(Of Commande)

                'Parcours la liste des mois de l'année
                For i = 1 To 12
                    Dim d As New DateTime(year, i, 1)

                    'Défini les paramètres de la requête
                    Dim parDate As MySqlParameter = connection.Create("@Date", DbType.DateTime, d)
                    parameters.Add(parDate)

                    'Exécute la requête
                    Objects = connection.ExecuteQuery("SELECT NumCmd, Montant FROM Commande WHERE YEAR(DelaiPrevu)=YEAR(@Date) And MONTH(DelaiPrevu)=MONTH(@Date)", parameters)

                    'Traite les résultats, ajoute un item par mois avec le CA correspondant et les n° des commandes correspondantes. Puis ajoute le CA du mois au CA total de l'année
                    For Each obj In Objects
                        chiffre += Decimal.Parse(obj(1))
                        listCmd.Add(New Commande(Integer.Parse(obj(0))))
                    Next

                    Dim caItem As New CaItem(MonthName(i), chiffre, listCmd)
                    Me.DgCa.Items.Add(caItem)
                    total += chiffre
                    chiffre = 0
                    listCmd = New List(Of Commande)

                    parameters.Clear()
                Next

                'Affiche le CA total de l'année
                If total > 0 Then
                    Dim cc As New ChiffreConverter
                    TxtTotal.Text = cc.Convert(total.ToString(), Nothing, Nothing, Nothing)
                Else
                    TxtTotal.Text = "0,00 €"
                End If
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                'Ferme la connection
                connection.Close()
            Catch ex As Exception
            End Try
        End Try
    End Sub

#End Region

#Region "EventControlEnter"

    ''' <summary>
    ''' Évènement se produisant lors du double clique sur un item du DgCa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DgCa_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        If Me.ListCommandes IsNot Nothing Then
            If DgCa.SelectedItem IsNot Nothing Then
                'Récupère l'item sélectionné
                Dim caItem As CaItem = DgCa.SelectedItem

                Me.ListCommandes.Items.Clear()

                'Ajoute toutes les commandes correspondantes à la liste du tableau de bord
                For Each ca In caItem.NumCmds
                    Me.ListCommandes.Items.Add(ca.GetCommande())
                Next
            End If
        End If

    End Sub

#End Region

End Class


Public Class CaItem

#Region "Fields"

    Private _Item As String
    Private _Chiffre As Decimal
    Private _NumCmds As List(Of Commande)

#End Region

#Region "Properties"

    Public Property Item As String
        Get
            Return Me._Item
        End Get
        Set(ByVal value As String)
            Me._Item = value
        End Set
    End Property

    Public Property Chiffre As Decimal
        Get
            Return Me._Chiffre
        End Get
        Set(ByVal value As Decimal)
            Me._Chiffre = FormatNumber(value, 2)
        End Set
    End Property

    Public Property NumCmds As List(Of Commande)
        Get
            Return Me._NumCmds
        End Get
        Set(ByVal value As List(Of Commande))
            Me._NumCmds = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal item As String, ByVal chiffre As Decimal, ByVal listCmd As List(Of Commande))
        Me.Item = item
        Me.Chiffre = chiffre
        Me.NumCmds = listCmd
    End Sub

#End Region

End Class
