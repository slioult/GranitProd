Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.Globalization

Public Class TempsFabrication

#Region "Fields"

    Private _Item As String
    Private _TpsFab As Integer
    Private _ListCommandes As ListBox
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

    Public Property TpsFab As Integer
        Get
            Return Me._TpsFab
        End Get
        Set(ByVal value As Integer)
            Me._TpsFab = value
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

        Dim pl As New PlanningControl()
        CbxChoixWeek.SelectedIndex = pl.GetWeekOfDate(Date.Now) - 1
        pl = Nothing

    End Sub

    Public Sub New(ByVal item As String, ByVal tpsFab As Integer, ByVal listCmd As List(Of Commande))
        InitializeComponent()

        Me.Item = item
        Me.TpsFab = tpsFab
        Me.NumCmds = listCmd
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
        'Met à jour l'interface graphique suivant le choix de l'utilisateur
        If (CbxChoix.SelectedIndex = 0) Then
            CbxChoixWeek.Visibility = Windows.Visibility.Visible
            CbxChoixMonth.Visibility = Windows.Visibility.Collapsed
            CbxChoixMonth.SelectedIndex = Date.Now.Month - 1
            CbxChoixYear.SelectedItem = Date.Now.Year
            DgTpsFab.Columns(0).Header = "Jour"
        ElseIf (CbxChoix.SelectedIndex = 1) Then
            CbxChoixMonth.Visibility = Windows.Visibility.Visible
            CbxChoixWeek.Visibility = Windows.Visibility.Collapsed
            Dim pl As New PlanningControl()
            CbxChoixWeek.SelectedIndex = pl.GetWeekOfDate(Date.Now) - 1
            pl = Nothing
            CbxChoixYear.SelectedItem = Date.Now.Year
            DgTpsFab.Columns(0).Header = "Semaine"
        Else
            CbxChoixWeek.Visibility = Windows.Visibility.Collapsed
            CbxChoixMonth.Visibility = Windows.Visibility.Collapsed
            CbxChoixMonth.SelectedIndex = Date.Now.Month - 1
            Dim pl As New PlanningControl()
            CbxChoixWeek.SelectedIndex = pl.GetWeekOfDate(Date.Now) - 1
            pl = Nothing
            CbxChoixYear.SelectedItem = Date.Now.Year
            DgTpsFab.Columns(0).Header = "Mois"
        End If

        'Met à jour les données contenues dans les DataGrid
        Me.CbxParam_SelectionChanged(Nothing, Nothing)
    End Sub

    ''' <summary>
    ''' Paramètre d'affichage du temps de fabrication change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub CbxParam_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Me.DgTpsFab.Items.Clear()
        Dim Objects As New List(Of List(Of Object))
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection
            connection.Open()

            'Choix d'affichage par semaine
            If CbxChoix.SelectedIndex = 0 Then
                Dim pl As New PlanningControl()
                Dim total As Integer = 0

                'Récupère les jours de la semaine
                Dim semaine As List(Of Date) = pl.GetDaysOfWeek(CbxChoixWeek.SelectedItem, CbxChoixYear.SelectedItem)

                'Parcours les jours de la semaine
                For Each d In semaine
                    'Défini les paramètres de la requête
                    Dim parDate As MySqlParameter = connection.Create("@Date", DbType.DateTime, d)
                    parameters.Add(parDate)

                    'Exécute la requête
                    Objects = connection.ExecuteQuery("SELECT NumCmd, TpsDebit + TpsCmdNumerique + TpsFinition + TpsAutres FROM Commande WHERE DelaiPrevu=@Date", parameters)

                    'Vérifie que la requête a retourné au moins 1 résultat
                    If Objects.Count > 0 Then
                        Dim tps As Integer
                        Dim listCmd As New List(Of Commande)

                        'Traite les résultats
                        For Each obj In Objects
                            tps += Integer.Parse(obj(1))
                            listCmd.Add(New Commande(Long.Parse(obj(0))))
                        Next

                        'Ajoute l'item dans le DataGrid et ajoute le temps au temps total et la liste des n° de commande correspondant à chaque commande concernée
                        Dim tpsFabItem As New TempsFabrication(d.ToString("dddd dd MMMM", New CultureInfo("fr-FR")), tps, listCmd)
                        DgTpsFab.Items.Add(tpsFabItem)
                        total += tpsFabItem.TpsFab
                        tps = 0
                        listCmd = New List(Of Commande)

                    End If

                    parameters.Clear()
                Next

                'Affiche le temps total
                If total > 0 Then
                    Dim tf As New TpsFabricationConverter
                    TxtTotal.Text = tf.Convert(total, Nothing, Nothing, Nothing)
                Else
                    TxtTotal.Text = "Rien de programmé"
                End If

                'Choix d'affichage par mois
            ElseIf CbxChoix.SelectedIndex = 1 And CbxChoixYear.SelectedItem IsNot Nothing Then
                Dim cal As New GregorianCalendar
                Dim pl As New PlanningControl
                'Récupère le nombre de jours dans le mois sélectionné
                Dim days As Integer = cal.GetDaysInMonth(CbxChoixYear.SelectedItem, CbxChoixMonth.SelectedIndex + 1)
                Dim month As Integer = CbxChoixMonth.SelectedIndex + 1
                Dim year As Integer = CbxChoixYear.SelectedItem
                Dim tempSem As Integer = 0
                Dim tps As Integer = 0
                Dim total As Integer = 0
                Dim listCmd As New List(Of Commande)

                'Parcours la liste des jours concernés par la requête
                For i = 1 To days
                    Dim d As New DateTime(year, month, i)
                    'Récupère la semaine correspondant à la date afin de séparer les différentes semaines du mois
                    Dim sem As Integer = pl.GetWeekOfDate(d)

                    'Défini les paramètres de la requête
                    Dim parDate As MySqlParameter = connection.Create("@Date", DbType.DateTime, d)
                    parameters.Add(parDate)

                    'Exécute la requête
                    Objects = connection.ExecuteQuery("SELECT NumCmd, TpsDebit + TpsCmdNumerique + TpsFinition + TpsAutres FROM Commande WHERE DelaiPrevu=@Date", parameters)

                    'Ajoute un item par semaine en additionnant les temps de production
                    If i = days Then
                        Dim tpsFabItem As New TempsFabrication(tempSem, tps, listCmd)
                        DgTpsFab.Items.Add(tpsFabItem)
                        total += tps
                        tps = 0
                        listCmd = New List(Of Commande)
                    ElseIf sem = tempSem And tempSem <> 0 Then
                        For Each obj In Objects
                            tps += Integer.Parse(obj(1))
                            listCmd.Add(New Commande(Integer.Parse(obj(0))))
                        Next
                    ElseIf tempSem <> 0 Then
                        Dim tpsFabItem As New TempsFabrication(tempSem, tps, listCmd)
                        DgTpsFab.Items.Add(tpsFabItem)
                        tempSem = sem
                        total += tps
                        tps = 0
                        listCmd = New List(Of Commande)

                        For Each obj In Objects
                            tps = Integer.Parse(obj(1))
                            listCmd.Add(New Commande(Integer.Parse(obj(0))))
                        Next
                    Else
                        For Each obj In Objects
                            tps += Integer.Parse(obj(1))
                            listCmd.Add(New Commande(Integer.Parse(obj(0))))
                        Next
                        tempSem = sem
                    End If

                    parameters.Clear()
                Next

                'Affiche le total de temps de fabrication du mois
                If total > 0 Then
                    Dim tf As New TpsFabricationConverter
                    TxtTotal.Text = tf.Convert(total, Nothing, Nothing, Nothing)
                Else
                    TxtTotal.Text = "Rien de programmé"
                End If

                'Choix d'affichage par année
            ElseIf CbxChoix.SelectedIndex = 2 Then
                Dim year = CbxChoixYear.SelectedItem
                Dim total As Integer = 0
                Dim tps As Integer = 0
                Dim listCmd As New List(Of Commande)

                'Parcours les mois de l'année
                For i = 1 To 12
                    Dim d As New DateTime(year, i, 1)
                    'Défini les paramètres de la requête
                    Dim parDate As MySqlParameter = connection.Create("@Date", DbType.DateTime, d)
                    parameters.Add(parDate)

                    'Exécute la requête
                    Objects = connection.ExecuteQuery("SELECT NumCmd, TpsDebit + TpsCmdNumerique + TpsFinition + TpsAutres FROM Commande WHERE YEAR(DelaiPrevu)=YEAR(@Date) And MONTH(DelaiPrevu)=MONTH(@Date)",
                                                      parameters)

                    'Traite les résultats
                    For Each obj In Objects
                        tps += Integer.Parse(obj(1))
                        listCmd.Add(New Commande(Integer.Parse(obj(0))))
                    Next

                    'Ajoute un item par mois dans le DataGrid avec le temps de production calculé
                    Dim tpsFabItem As New TempsFabrication(MonthName(i), tps, listCmd)
                    Me.DgTpsFab.Items.Add(tpsFabItem)
                    total += tps
                    tps = 0
                    listCmd = New List(Of Commande)

                    parameters.Clear()
                Next

                'Affiche le total de temps de production de l'année
                If total > 0 Then
                    Dim tf As New TpsFabricationConverter
                    TxtTotal.Text = tf.Convert(total, Nothing, Nothing, Nothing)
                Else
                    TxtTotal.Text = "Rien de programmé"
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
    ''' Évènement se produisant lors du double clique sur un item du DgTpsFab
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DgTpsFab_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)

        If DgTpsFab.SelectedItem IsNot Nothing Then
            'Récupère l'item sélectionné dans le DataGrid
            Dim tpsFabItem As TempsFabrication = DgTpsFab.SelectedItem

            Me.ListCommandes.Items.Clear()

            'Ajoute les commandes correspondant à l'item sélectionné dans la liste du tableau de bord
            For Each tpsItem In tpsFabItem.NumCmds
                Me.ListCommandes.Items.Add(tpsItem.GetCommande())
            Next
        End If

    End Sub

#End Region

End Class
