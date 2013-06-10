Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.IO
Imports System.ComponentModel
Imports System.Threading
Imports System.Threading.Tasks

Public Class RechercheCommande

#Region "DependencyProperty"

    Public Shared ReadOnly SessionProperty As DependencyProperty = DependencyProperty.Register("Session", GetType(Session), GetType(RechercheCommande),
                                                                                               New FrameworkPropertyMetadata(Nothing))
    Public Property Session As Session
        Get
            Return DirectCast(Me.GetValue(SessionProperty), Session)
        End Get
        Set(ByVal value As Session)
            Me.SetValue(SessionProperty, value)
        End Set
    End Property

    Public Shared ReadOnly PlanningProperty As DependencyProperty = DependencyProperty.Register("Planning", GetType(PlanningControl), GetType(RechercheCommande),
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

    Private _Commandes As List(Of Commande)
    Private IsUpdClient As Boolean = False
    Private IsUpdContremarque As Boolean = True
    Private bwk As BackgroundWorker

#End Region

#Region "Properties"

    Public Property ListCommandes As List(Of Commande)
        Get
            Return Me._Commandes
        End Get
        Set(ByVal value As List(Of Commande))
            Me._Commandes = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        bwk = New BackgroundWorker()

        bwk.WorkerReportsProgress = True
        AddHandler bwk.DoWork, AddressOf search
        AddHandler bwk.RunWorkerCompleted, AddressOf endSearch

        For i = 1 To 53
            Me.CbxSemaine.Items.Add(i)
        Next

        For i = 2010 To Date.Now.Year + 2
            Me.CbxAnnee.Items.Add(i)
        Next

        Dim pl As New PlanningControl(True)
        Dim ds As New List(Of Date)
        Dim sem As Integer = pl.GetWeekOfDate(Date.Now)
        ds = pl.GetDaysOfWeek(sem, Date.Now.Year)
        Dim count As Integer = ds.Count
        Me.DpkDateDebut.SelectedDate = ds(0)
        Me.DpkDateFin.SelectedDate = ds(count - 1)

        Me.CbxSemaine.SelectedIndex = sem
        Me.CbxAnnee.SelectedItem = Date.Now.Year
    End Sub

#End Region

#Region "Button"

    ''' <summary>
    ''' Bouton permettant de générer le fichier Excel à partir des résultats de la recherche
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnExcel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If LbxSearchCmd.Items.Count > 0 Then

            'liste de commandes à exporter
            Dim cmds As New List(Of Commande)
            For Each item In LbxSearchCmd.Items
                Dim cmd As cmdItem = item
                cmds.Add(cmd.Commande)
            Next

            'Définit tous les paramètres de l'exportation
            Dim client As String = String.Empty
            Dim cm As String = String.Empty
            Dim numCmd As String = String.Empty
            Dim mat As String = String.Empty

            Dim search As String = String.Empty

            Dim cbxi As ComboBoxItem = Me.CbxTri.SelectedItem
            Dim tri As String = cbxi.Content
            Dim etat As String = Me.CbxEtat.SelectedItem.Content.ToLower()
            Dim etatCmd As String
            etatCmd = "Commandes " & etat & " triées par " + tri.ToLower()

            If Me.AutoCompNClient.SelectedItem IsNot Nothing Then
                Dim cl As Client = AutoCompNClient.SelectedItem
                client = "Client : " + cl.Nom
            End If

            If Me.AutoCompNContremarque.SelectedItem IsNot Nothing Then
                Dim cmq As Contremarque = AutoCompNContremarque.SelectedItem
                cm = "CM : " + cmq.Nom
            End If

            If Me.AutoCompNumCmd.SelectedItem IsNot Nothing Then
                numCmd = "N° cmd : " + AutoCompNumCmd.SelectedItem.ToString()
            End If

            If Me.AutoCompLMateriau.SelectedItem IsNot Nothing Then
                Dim m As Materiau = AutoCompLMateriau.SelectedItem
                mat = "Matériau : " + m.Label
            End If

            If client <> String.Empty Then
                search = client
            End If

            If cm <> String.Empty Then
                If search <> String.Empty Then
                    search = search + "   /   " + cm
                Else
                    search = cm
                End If
            End If

            If numCmd <> String.Empty Then
                If search <> String.Empty Then
                    search = search + "   /   " + numCmd
                Else
                    search = numCmd
                End If
            End If

            If mat <> String.Empty Then
                If search <> String.Empty Then
                    search = search + "   /   " + mat
                Else
                    search = mat
                End If
            End If

            ExcelExport.ExportCommande(cmds, search, etatCmd, "XLSX")
        Else
            MessageBox.Show("La recherche doit contenir au moins un résultat.", "Génération du fichier EXCEL", MessageBoxButton.OK, MessageBoxImage.Exclamation)
        End If
    End Sub

    ''' <summary>
    ''' Bouton permettant de générer le fichier PDF à partir des résultats de la recherche
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnPdf_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If LbxSearchCmd.Items.Count > 0 Then

                'Liste des commandes à exporter
                Dim cmds As New List(Of Commande)
                For Each item In LbxSearchCmd.Items
                    Dim cmd As cmdItem = item
                    cmds.Add(cmd.Commande)
                Next

                'Définit tous les paramètres de l'exportation
                Dim client As String = String.Empty
                Dim cm As String = String.Empty
                Dim numCmd As String = String.Empty
                Dim mat As String = String.Empty

                Dim search As String = String.Empty

                Dim cbxi As ComboBoxItem = Me.CbxTri.SelectedItem
                Dim tri As String = cbxi.Content
                Dim etat As String = Me.CbxEtat.SelectedItem.Content.ToLower()
                Dim etatCmd As String
                etatCmd = "Commandes " & etat & " triées par " + tri.ToLower()

                If Me.AutoCompNClient.SelectedItem IsNot Nothing Then
                    Dim cl As Client = AutoCompNClient.SelectedItem
                    client = "Client : " + cl.Nom
                End If

                If Me.AutoCompNContremarque.SelectedItem IsNot Nothing Then
                    Dim cmq As Contremarque = AutoCompNContremarque.SelectedItem
                    cm = "CM : " + cmq.Nom
                End If

                If Me.AutoCompNumCmd.SelectedItem IsNot Nothing Then
                    numCmd = "N° cmd : " + AutoCompNumCmd.SelectedItem.ToString()
                End If

                If Me.AutoCompLMateriau.SelectedItem IsNot Nothing Then
                    Dim m As Materiau = AutoCompLMateriau.SelectedItem
                    mat = "Matériau : " + m.Label
                End If

                If client <> String.Empty Then
                    search = client
                End If

                If cm <> String.Empty Then
                    If search <> String.Empty Then
                        search = search + "   /   " + cm
                    Else
                        search = cm
                    End If
                End If

                If numCmd <> String.Empty Then
                    If search <> String.Empty Then
                        search = search + "   /   " + numCmd
                    Else
                        search = numCmd
                    End If
                End If

                If mat <> String.Empty Then
                    If search <> String.Empty Then
                        search = search + "   /   " + mat
                    Else
                        search = mat
                    End If
                End If

                ExcelExport.ExportCommande(cmds, search, etatCmd, "PDF")
            Else
                MessageBox.Show("La recherche doit contenir au moins un résultat.", "Génération du fichier PDF", MessageBoxButton.OK, MessageBoxImage.Exclamation)
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
            Dim sw As New StreamWriter(My.Settings.ConfigFiles + "\log.txt")

            Dim content As String = "BtnPDF" + vbCrLf + ex.StackTrace.ToString() + vbCrLf + vbCrLf + ex.Source.ToString()
            If ex.InnerException IsNot Nothing Then
                content = content + vbCrLf + vbCrLf + ex.InnerException.ToString()
            End If

            content = content + vbCrLf + System.IO.Path.GetFullPath(My.Settings.Logo) + vbCrLf + vbCrLf + "/BtnPDF"

            sw.Write(content)

            sw.Close()
        End Try
    End Sub

    ''' <summary>
    ''' Bouton permettant d'ouvrir la commande sélectionnée afin de pouvoir la modifier
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnOpenCmd_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If LbxSearchCmd.SelectedItem IsNot Nothing Then
            'Récupère la commande sélectionnée
            Dim cmdItem As cmdItem = LbxSearchCmd.SelectedItem
            Dim cmd As Commande = cmdItem.Commande

            'Ouvre une consultation de commande
            Dim commande As ConsultCommande = New ConsultCommande(Me.Session, cmd, Me, Me.Planning)
            If commande.ShowType = 0 Then
                commande.Show()
            Else
                commande.Close()
                commande = Nothing
            End If
        Else
            MessageBox.Show("Veuillez sélectionner une commande.", "Aucune commande sélectionnée", MessageBoxButton.OK, MessageBoxImage.Exclamation)
        End If
    End Sub

    ''' <summary>
    ''' Bouton permettant de lancer la recherche
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub BtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Dim img As New Image()
            Dim bmp As New BitmapImage(New Uri(System.IO.Path.GetFullPath(My.Settings.Sablier)))
            img.Source = bmp
            Me.BtnSearch.Content = img
            Me.LbxSearchCmd.Items.Clear()
            bwk.RunWorkerAsync()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

#End Region

#Region "AutoCompletion"

    ''' <summary>
    ''' Delegate de l'auto-complétion du nom client
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub cbxClient()

    ''' <summary>
    ''' Auto-complétion du nom client
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AutoCompClient()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim objects As New List(Of List(Of Object))
        Try
            'Ouvre la connection
            connection.Open()

            'Élargit la recherche
            Dim text As String = Me.AutoCompNClient.Text.Replace("'", "\'")
            text = text.Replace("""", "\""")

            'Exécute la requête
            objects = connection.ExecuteQuery("SELECT Identifier, Nom FROM Client WHERE Nom Like '%" + text.ToUpper() + "%' Order By Nom")

            Dim clients As New List(Of Client)

            'Traite les résultats
            For Each obj In objects
                clients.Add(New Client(obj(1).ToString(), Long.Parse(obj(0))))
            Next

            'Modifie la source de l'autocompletebox en fonction des résultats obtenus
            Me.AutoCompNClient.ItemsSource = clients
            Me.AutoCompNClient.PopulateComplete()
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

    ''' <summary>
    ''' Delegate de l'auto-complétion du n° commande
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub cbxNumCommande()

    ''' <summary>
    ''' Auto-complétion du n° commande
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AutoCompCommand()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim objects As New List(Of List(Of Object))
        Try
            'Ouvre la connection
            connection.Open()

            'Élargit la recherche
            Dim text As String = Me.AutoCompNumCmd.Text.Replace("'", "\'")
            text = text.Replace("""", "\""")

            'Exécute la requête
            objects = connection.ExecuteQuery("SELECT NumCmd, DelaiPrevu FROM Commande WHERE NumCmd Like '%" + text.ToUpper() + "%' Order By DelaiPrevu")

            Dim commandes As New List(Of String)

            'Traite les résultats
            For Each obj In objects
                commandes.Add(obj(0).ToString())
            Next

            'Modifie la source de l'autocompletebox en fonction des résultats obtenus
            Me.AutoCompNumCmd.ItemsSource = commandes
            Me.AutoCompNumCmd.PopulateComplete()
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

    ''' <summary>
    ''' Delegate de l'auto-complétion du nom contremarque
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub cbxContremarque()

    ''' <summary>
    ''' Auto-complétion du nom contremarque
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AutoCompContremarque()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim objects As New List(Of List(Of Object))
        Try
            'Ouvre la connection
            connection.Open()

            'Élargit la recherche
            Dim text As String = Me.AutoCompNContremarque.Text.Replace("'", "\'")
            text = text.Replace("""", "\""")

            'Exécute la requête
            objects = connection.ExecuteQuery("SELECT Identifier, Nom FROM Contremarque WHERE Nom Like '%" + text.ToUpper() + "%' Order By Nom")

            Dim contremarques = New List(Of Contremarque)

            'Traite les résultats
            For Each obj In objects
                contremarques.Add(New Contremarque(obj(1).ToString(), Long.Parse(obj(0))))
            Next

            'Modifie la source de l'autocompletebox en fonction des résultats obtenus
            Me.AutoCompNContremarque.ItemsSource = contremarques
            Me.AutoCompNContremarque.PopulateComplete()
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

    ''' <summary>
    ''' Delegate de l'auto-complétion du label materiau
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub cbxMateriau()

    ''' <summary>
    ''' Auto-complétion du nom contremarque
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AutoCompMateriau()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim objects As New List(Of List(Of Object))
        Try
            'Ouvre la connection
            connection.Open()

            'Élargit la recherche
            Dim text As String = Me.AutoCompLMateriau.Text.Replace("'", "\'")
            text = text.Replace("""", "\""")

            'Exécute la requête
            objects = connection.ExecuteQuery("SELECT Identifier, Label FROM Materiau WHERE Label Like '" + text.ToUpper() + "%' Order By Label")

            Dim materiaux = New List(Of Materiau)

            'Traite les résultats
            For Each obj In objects
                materiaux.Add(New Materiau(obj(1).ToString(), Long.Parse(obj(0))))
            Next

            'Modifie la source de l'autocompletebox en fonction des résultats obtenus
            Me.AutoCompLMateriau.ItemsSource = materiaux
            Me.AutoCompLMateriau.PopulateComplete()
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


    ''' <summary>
    ''' Écriture dans l'AutoCompleteBox du n° de commande
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AutoCompNumCmd_Populating(ByVal sender As System.Object, ByVal e As System.Windows.Controls.PopulatingEventArgs)
        Dim del As cbxNumCommande
        del = AddressOf AutoCompCommand
        del.Invoke()

        Me.AutoCompNClient.SelectedItem = Nothing
        Me.AutoCompNClient.Text = ""
        Me.AutoCompNContremarque.SelectedItem = Nothing
        Me.AutoCompNContremarque.Text = ""
        Me.AutoCompLMateriau.SelectedItem = Nothing
        Me.AutoCompLMateriau.Text = ""
    End Sub

    ''' <summary>
    ''' Écriture dans l'AutoCompleteBox du nom client
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AutoCompNClient_Populating(ByVal sender As System.Object, ByVal e As System.Windows.Controls.PopulatingEventArgs)
        Dim del As cbxClient
        del = AddressOf AutoCompClient
        del.Invoke()
    End Sub

    ''' <summary>
    ''' Écriture dans l'AutoCompleteBox de la contremarque
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AutoCompNContremarque_Populating(ByVal sender As System.Object, ByVal e As System.Windows.Controls.PopulatingEventArgs)
        Dim del As cbxContremarque
        del = AddressOf AutoCompContremarque
        del.Invoke()
    End Sub

    ''' <summary>
    ''' Écriture dans l'AutoCompleteBox du matériau
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AutoCompLMateriau_Populating(ByVal sender As System.Object, ByVal e As System.Windows.Controls.PopulatingEventArgs)
        Dim del As cbxMateriau
        del = AddressOf AutoCompMateriau
        del.Invoke()

        Me.AutoCompNumCmd.SelectedItem = Nothing
        Me.AutoCompNumCmd.Text = ""
    End Sub

#End Region

#Region "Delegates"

    ''' <summary>
    ''' Delegate du selectionChanged de l'AutoCompleteBox client
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub cbxClientChanged()

    ''' <summary>
    ''' Delegate du selectionChanged de l'AutoCompleteBox contremarque
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub cbxCmqChanged()

    ''' <summary>
    ''' Delegate du selectionChanged de l'AutoCompleteBox n° de commande
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub cbxNumCmdChanged()

#End Region

#Region "SelectionChanged"

    ''' <summary>
    ''' Se produit lorque la valeur du ckeckbox ChkEndCmd change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChkEndCmd_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        LbxSearchCmd.Items.Clear()
    End Sub

    ''' <summary>
    ''' Se produit lorsque la date de début change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DpkDateDebut_SelectedDateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If Me.DpkDateFin.SelectedDate IsNot Nothing And sender.SelectedDate IsNot Nothing Then
            Dim dDeb As DateTime = sender.SelectedDate
            Dim dFin As DateTime = Me.DpkDateFin.SelectedDate

            If dDeb > dFin Then Me.DpkDateFin.SelectedDate = sender.SelectedDate
        ElseIf sender.SelectedDate Is Nothing Then
            Me.DpkDateFin.SelectedDate = Nothing
            Me.ChkSemaine.IsChecked = False
        End If

        If sender.selectedDate IsNot Nothing And Me.ChkSemaine.IsChecked Then
            Dim pl As New PlanningControl(True)
            Dim sem As Integer = pl.GetWeekOfDate(sender.SelectedDate)
            Me.CbxSemaine.SelectedIndex = sem
            Me.CbxAnnee.SelectedItem = sender.SelectedDate.Year
        End If
    End Sub

    ''' <summary>
    ''' Se produit lorsque la date de fin change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DpkDateFin_SelectedDateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If Me.DpkDateDebut.SelectedDate IsNot Nothing And sender.selectedDate IsNot Nothing Then
            Dim dFin As DateTime = sender.SelectedDate
            Dim dDeb As DateTime = Me.DpkDateDebut.SelectedDate

            If dDeb > dFin Then Me.DpkDateDebut.SelectedDate = sender.SelectedDate
        ElseIf sender.SelectedDate Is Nothing Then
            Me.DpkDateDebut.SelectedDate = Nothing
            Me.ChkSemaine.IsChecked = False
        End If
    End Sub

    ''' <summary>
    ''' Se produit lorsque la semaine sélectionnée est modifiée
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CbxSemaine_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If Me.CbxSemaine.SelectedIndex > 0 And Me.CbxAnnee.SelectedIndex > 0 Then
            Dim pl As New PlanningControl(True)
            Dim ds As New List(Of Date)
            ds = pl.GetDaysOfWeek(Me.CbxSemaine.SelectedIndex, Me.CbxAnnee.SelectedItem)
            Dim count As Integer = ds.Count
            Me.DpkDateDebut.SelectedDate = ds(0)
            Me.DpkDateFin.SelectedDate = ds(count - 1)
        End If
    End Sub

    ''' <summary>
    ''' Se produit quand le checkbox permettant d'activer la mise à jour de la semaine est coché
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChkSemaine_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Me.CbxSemaine IsNot Nothing And Me.CbxAnnee IsNot Nothing Then
            Me.CbxSemaine.IsEnabled = True
            Me.CbxAnnee.IsEnabled = True
            If Me.DpkDateDebut.SelectedDate IsNot Nothing And Me.DpkDateFin.SelectedDate IsNot Nothing Then
                Me.DpkDateDebut_SelectedDateChanged(Me.DpkDateDebut, Nothing)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Se produit quand le checkbox permettant d'activer la mise à jour de la semaine est décoché
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChkSemaine_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Me.CbxSemaine IsNot Nothing And Me.CbxAnnee IsNot Nothing Then
            Me.CbxSemaine.IsEnabled = False
            Me.CbxAnnee.IsEnabled = False
            Me.CbxSemaine.SelectedIndex = 0
            Me.CbxAnnee.SelectedIndex = 0
        End If
    End Sub

#End Region

#Region "EventControlEnter"

    ''' <summary>
    ''' Évènement se produisant lorsque qu'une touche est enfoncée sur une AutoCompleteBox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AutoComp_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        Dim autoComp As AutoCompleteBox = sender

        'Limite la saisie à 50 caractères
        If e.Key <> Key.Enter And e.Key <> Key.Return And e.Key <> Key.Back And e.Key <> Key.LeftCtrl <> e.Key <> Key.RightAlt Then
            If autoComp.Text.Length >= 50 Then
                e.Handled = True
            End If
        End If
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Réinitialise le UserControl
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Reinitialize()
        Me.AutoCompNClient.SelectedItem = Nothing
        Me.AutoCompNContremarque.SelectedItem = Nothing
        Me.AutoCompNumCmd.SelectedItem = Nothing
        Me.AutoCompLMateriau.SelectedItem = Nothing
        Me.CbxEtat.SelectedIndex = 0
        Me.LbxSearchCmd.Items.Clear()
    End Sub

    ''' <summary>
    ''' Fonction exécutant la recherche
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub search()
        Application.Current.Dispatcher.Invoke(New Action(Sub()
                                                             Dim whereEtat As String = String.Empty
                                                             Dim param As String = String.Empty
                                                             Dim ListCommandes As New List(Of Commande)
                                                             Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)

                                                             Try

                                                                 'Si l'item n'est pas sélectionné mais que le texte est saisi, fait la liaison entre ce qui est écrit manuelle et un item de l'autocompleteBox
                                                                 If Me.AutoCompLMateriau.SelectedItem Is Nothing And Me.AutoCompLMateriau.Text <> "" Then
                                                                     Dim mt As Materiau = Nothing

                                                                     For Each item In Me.AutoCompLMateriau.ItemsSource
                                                                         Dim m As Materiau = item
                                                                         If m.Label = Me.AutoCompLMateriau.Text.ToUpper() Then
                                                                             Me.AutoCompLMateriau.SelectedItem = item
                                                                             mt = m
                                                                             Exit For
                                                                         End If
                                                                     Next

                                                                     If Me.AutoCompLMateriau.SelectedItem Is Nothing Then Exit Sub
                                                                 End If

                                                                 'Si l'item n'est pas sélectionné mais que le texte est saisi, fait la liaison entre ce qui est écrit manuelle et un item de l'autocompleteBox
                                                                 If Me.AutoCompNClient.SelectedItem Is Nothing And Me.AutoCompNClient.Text <> "" Then
                                                                     Dim cl As Client = Nothing

                                                                     For Each item In Me.AutoCompNClient.ItemsSource
                                                                         Dim c As Client = item
                                                                         If c.Nom = Me.AutoCompNClient.Text.ToUpper() Then
                                                                             Me.AutoCompNClient.SelectedItem = item
                                                                             cl = c
                                                                             Exit For
                                                                         End If
                                                                     Next
                                                                 End If

                                                                 'Si l'item n'est pas sélectionné mais que le texte est saisi, fait la liaison entre ce qui est écrit manuelle et un item de l'autocompleteBox
                                                                 If Me.AutoCompNContremarque.SelectedItem Is Nothing And Me.AutoCompNContremarque.Text <> "" Then
                                                                     Dim cm As Contremarque = Nothing

                                                                     For Each item In Me.AutoCompNContremarque.ItemsSource
                                                                         Dim c As Contremarque = item
                                                                         If c.Nom = Me.AutoCompNContremarque.Text.ToUpper() Then
                                                                             Me.AutoCompNContremarque.SelectedItem = item
                                                                             cm = c
                                                                             Exit For
                                                                         End If
                                                                     Next
                                                                 End If

                                                                 'Définit la date correspondant au 1er du mois précédant
                                                                 Dim month As Integer = Date.Now.Month
                                                                 Dim year As Integer = Date.Now.Year
                                                                 If month < 2 Then
                                                                     month = 12
                                                                     year -= 1
                                                                 Else
                                                                     month -= 1
                                                                 End If
                                                                 Dim minDate As New DateTime(year, month, 1)

                                                                 'Définit une partie de la clause WHERE de la requête en fonction de l'état sélectionné
                                                                 If Me.CbxEtat.SelectedIndex = 0 Then
                                                                     whereEtat = " WHERE c.IdentifierEtat = e.Identifier AND e.Label <> 'Terminée' AND e.Label <> 'Rendue' AND "
                                                                 ElseIf Me.CbxEtat.SelectedIndex = 1 Then
                                                                     whereEtat = " WHERE c.IdentifierEtat = e.Identifier AND e.Label = 'Terminée' AND "
                                                                 ElseIf Me.CbxEtat.SelectedIndex = 2 Then
                                                                     whereEtat = " WHERE c.IdentifierEtat = e.Identifier AND e.Label = 'Rendue' AND "
                                                                 ElseIf Me.CbxEtat.SelectedIndex = 3 Then
                                                                     whereEtat = " WHERE c.DelaiPrevu >= '" + year.ToString() + "-" + month.ToString() + "-1' AND "
                                                                 End If

                                                                 If Me.DpkDateDebut.SelectedDate IsNot Nothing And whereEtat <> String.Empty Then
                                                                     Dim dDeb As Date = Me.DpkDateDebut.SelectedDate
                                                                     Dim dDay As Integer = dDeb.Day
                                                                     Dim dMonth As Integer = dDeb.Month
                                                                     Dim dYear As Integer = dDeb.Year
                                                                     Dim dFin As Date = Me.DpkDateFin.SelectedDate
                                                                     Dim fDay As Integer = dFin.Day
                                                                     Dim fMonth As Integer = dFin.Month
                                                                     Dim fYear As Integer = dFin.Year

                                                                     whereEtat = whereEtat + "DAY(c.DelaiPrevu) >= " + dDay.ToString() + " AND MONTH(c.DelaiPrevu) >= " +
                                                                         dMonth.ToString() + " AND YEAR(c.DelaiPrevu) >= " + dYear.ToString() +
                                                                         " AND DAY(c.DelaiPrevu) <= " + fDay.ToString() + " AND MONTH(c.DelaiPrevu) <= " +
                                                                         fMonth.ToString() + " AND YEAR(c.DelaiPrevu) <= " + fYear.ToString() + " AND "
                                                                 ElseIf Me.DpkDateDebut.SelectedDate IsNot Nothing Then
                                                                     Dim dDeb As Date = Me.DpkDateDebut.SelectedDate
                                                                     Dim dDay As Integer = dDeb.Day
                                                                     Dim dMonth As Integer = dDeb.Month
                                                                     Dim dYear As Integer = dDeb.Year
                                                                     Dim dFin As Date = Me.DpkDateFin.SelectedDate
                                                                     Dim fDay As Integer = dFin.Day
                                                                     Dim fMonth As Integer = dFin.Month
                                                                     Dim fYear As Integer = dFin.Year

                                                                     whereEtat = " WHERE DAY(c.DelaiPrevu) >= " + dDay.ToString() + " AND MONTH(c.DelaiPrevu) >= " +
                                                                         dMonth.ToString() + " AND YEAR(c.DelaiPrevu) >= " + dYear.ToString() +
                                                                         " AND DAY(c.DelaiPrevu) <= " + fDay + " AND MONTH(c.DelaiPrevu) <= " +
                                                                         fMonth.ToString() + " AND YEAR(c.DelaiPrevu) <= " + fYear.ToString() + " AND "
                                                                 End If

                                                                 'Définit une partie de la clause WHERE de la requête en fonction du type de tri sélectionné
                                                                 If CbxTri.SelectedIndex = 0 Then
                                                                     param = " c.DelaiPrevu"
                                                                 Else
                                                                     param = " c.DateCommande"
                                                                 End If

                                                                 'Si Algorithme de recherche si un matériau est sélectionné
                                                                 If Me.AutoCompLMateriau.SelectedItem IsNot Nothing Or Me.AutoCompLMateriau.Text <> "" Then
                                                                     Dim Objects As List(Of List(Of Object))
                                                                     Dim parameters As New List(Of MySqlParameter)
                                                                     Dim query As String = String.Empty

                                                                     'Si l'item n'est pas sélectionné mais que le text est saisi, fait la liaison entre ce qui est écrit manuelle et un item de l'autocompleteBox
                                                                     If Me.AutoCompLMateriau.SelectedItem Is Nothing Then
                                                                         Dim mt As Materiau = Nothing

                                                                         For Each item In Me.AutoCompLMateriau.ItemsSource
                                                                             Dim m As Materiau = item
                                                                             If m.Label = Me.AutoCompLMateriau.Text.ToUpper() Then
                                                                                 Me.AutoCompLMateriau.SelectedItem = item
                                                                                 mt = m
                                                                                 Exit For
                                                                             End If
                                                                         Next

                                                                         If Me.AutoCompLMateriau.SelectedItem Is Nothing Then Exit Sub
                                                                     End If


                                                                     Dim materiau As Materiau = AutoCompLMateriau.SelectedItem

                                                                     'Ouvre la connection
                                                                     connection.Open()

                                                                     Dim parIdMateriau As MySqlParameter = connection.Create("@IdMateriau", DbType.Int32, materiau.Identifier)

                                                                     'S'exécute si un client et une contremarque sont sélectionnés
                                                                     If (Me.AutoCompNClient.SelectedItem IsNot Nothing And Me.AutoCompNContremarque.SelectedItem IsNot Nothing) Then
                                                                         Dim client As Client = AutoCompNClient.SelectedItem
                                                                         Dim cm As Contremarque = AutoCompNContremarque.SelectedItem

                                                                         'Défini les paramètres de la requête
                                                                         parameters.Add(parIdMateriau)

                                                                         Dim parIdClient As MySqlParameter = connection.Create("@IdClient", DbType.Int32, client.Identifier)
                                                                         parameters.Add(parIdClient)

                                                                         Dim parIdContremarque As MySqlParameter = connection.Create("@IdContremarque", DbType.Int32, cm.Identifier)
                                                                         parameters.Add(parIdContremarque)

                                                                         'Requête
                                                                         query = "Select DISTINCT c.NumCmd, c.DelaiPrevu from Commande as c, commande_materiau as cm, materiau as m, Etat as e" + whereEtat + "cm.identifier_commande = c.identifier and " +
                                                                          "cm.identifier_materiau = m.identifier and c.IdentifierClient = @IdClient and c.IdentifierContremarque = @IdContremarque and m.identifier = @IdMateriau Order By" + param

                                                                         'S'exécute si un client est sélectionné
                                                                     ElseIf (AutoCompNClient.SelectedItem IsNot Nothing) Then
                                                                         Dim client As Client = AutoCompNClient.SelectedItem

                                                                         'Défini les paramètres de la requête
                                                                         parameters.Add(parIdMateriau)

                                                                         Dim parIdClient As MySqlParameter = connection.Create("@IdClient", DbType.Int32, client.Identifier)
                                                                         parameters.Add(parIdClient)

                                                                         'Requête
                                                                         query = "Select DISTINCT c.NumCmd, c.DelaiPrevu from Commande as c, commande_materiau as cm, materiau as m, Etat as e" + whereEtat + "cm.identifier_commande = c.identifier and " +
                                                                            "cm.identifier_materiau = m.identifier and c.IdentifierClient = @IdClient and m.identifier = @IdMateriau Order By" + param

                                                                         'S'exécute si une contremarque est sélectionnée
                                                                     ElseIf (AutoCompNContremarque.SelectedItem IsNot Nothing) Then
                                                                         Dim cm As Contremarque = AutoCompNContremarque.SelectedItem

                                                                         'Défini les paramètres de la requête
                                                                         parameters.Add(parIdMateriau)

                                                                         Dim parIdContremarque As MySqlParameter = connection.Create("@IdContremarque", DbType.Int32, cm.Identifier)
                                                                         parameters.Add(parIdContremarque)

                                                                         'Requête
                                                                         query = "Select DISTINCT c.NumCmd, c.DelaiPrevu from Commande as c, commande_materiau as cm, materiau as m, Etat as e" + whereEtat + "cm.identifier_commande = c.identifier and " +
                                                                            "cm.identifier_materiau = m.identifier and c.IdentifierContremarque = @IdContremarque and m.identifier = @IdMateriau Order By" + param

                                                                         'S'exécute si seulement le matériau est sélectionné
                                                                     Else
                                                                         'Défini les paramètres de la requête
                                                                         parameters.Add(parIdMateriau)

                                                                         'Requête
                                                                         query = "Select DISTINCT c.NumCmd, c.DelaiPrevu from Commande as c, commande_materiau as cm, materiau as m, Etat as e" + whereEtat + "cm.identifier_commande = c.identifier and " +
                                                                                                "cm.identifier_materiau = m.identifier and m.identifier = @IdMateriau Order By" + param
                                                                     End If

                                                                     'Exécute la requête
                                                                     Objects = connection.ExecuteQuery(query, parameters)

                                                                     parameters.Clear()

                                                                     'Ferme la connexion
                                                                     connection.Close()

                                                                     'Traite les résultats
                                                                     For Each obj In Objects
                                                                         Dim cmd As New Commande(Integer.Parse(obj(0)))
                                                                         cmd = cmd.GetCommande()

                                                                         'Tri les résultats
                                                                         If Me.CbxEtat.SelectedIndex = 0 Then
                                                                             If cmd.Etat.Label <> "Terminée" And cmd.Etat.Label <> "Rendue" Then
                                                                                 ListCommandes.Add(cmd)
                                                                             End If
                                                                         ElseIf Me.CbxEtat.SelectedIndex = 1 Then
                                                                             If cmd.Etat.Label = "Terminée" Then
                                                                                 ListCommandes.Add(cmd)
                                                                             End If
                                                                         ElseIf Me.CbxEtat.SelectedIndex = 2 Then
                                                                             If cmd.Etat.Label = "Rendue" Then
                                                                                 ListCommandes.Add(cmd)
                                                                             End If
                                                                         ElseIf Me.CbxEtat.SelectedIndex = 3 Then
                                                                             If cmd.DelaiPrevu >= minDate Then
                                                                                 ListCommandes.Add(cmd)
                                                                             End If
                                                                         End If
                                                                     Next

                                                                     'S'exécute si un n° de commande est sélectionné
                                                                 ElseIf (Me.AutoCompNumCmd.SelectedItem IsNot Nothing) Then
                                                                     Dim cmd As New Commande(Integer.Parse(AutoCompNumCmd.SelectedItem))
                                                                     'Récupère la commande correspondante au n° de commande
                                                                     cmd = cmd.GetCommande()
                                                                     ListCommandes.Add(cmd)
                                                                     'L'affiche ou non suivant l'état sélectionné par l'utilisateur
                                                                     If cmd.Etat.Label = "Terminée" Then
                                                                         Me.CbxEtat.SelectedIndex = 1
                                                                     ElseIf cmd.Etat.Label = "Rendue" Then
                                                                         Me.CbxEtat.SelectedIndex = 2
                                                                     Else
                                                                         Me.CbxEtat.SelectedIndex = 0
                                                                     End If

                                                                     'S'exécute si un client est sélectionné
                                                                 ElseIf Me.AutoCompNClient.SelectedItem IsNot Nothing Or Me.AutoCompNClient.Text <> "" Then
                                                                     Dim Objects As New List(Of List(Of Object))
                                                                     Dim parameters As New List(Of MySqlParameter)

                                                                     'Ouvre la connection
                                                                     connection.Open()

                                                                     Dim cli As String
                                                                     Dim cmq As String

                                                                     ' Paramètre une partie de la clause WHERE avec l'identifiant de client sélectionné
                                                                     If Me.AutoCompNClient.SelectedItem IsNot Nothing Then
                                                                         Dim m As Client = Me.AutoCompNClient.SelectedItem
                                                                         cli = "IdentifierClient=" + m.Identifier.ToString() + " "
                                                                     Else
                                                                         Dim cl As Client = Nothing

                                                                         For Each item In Me.AutoCompNClient.ItemsSource
                                                                             Dim c As Client = item
                                                                             If c.Nom = Me.AutoCompNClient.Text.ToUpper() Then
                                                                                 Me.AutoCompNClient.SelectedItem = item
                                                                                 cl = c
                                                                                 Exit For
                                                                             End If
                                                                         Next

                                                                         If Me.AutoCompNClient.SelectedItem Is Nothing Then Exit Sub

                                                                         cli = "IdentifierClient=" + cl.Identifier.ToString() + " "
                                                                     End If

                                                                     'Vérifie si une contremarque est sélectionnée
                                                                     If Me.AutoCompNContremarque.SelectedItem IsNot Nothing Then
                                                                         'Paramètre une partie de la clause WHERE avec l'identifier de la contremarque
                                                                         Dim c As Contremarque = Me.AutoCompNContremarque.SelectedItem
                                                                         If cli = String.Empty Then
                                                                             cmq = "IdentifierContremarque=" + c.Identifier.ToString() + " "
                                                                         Else
                                                                             cmq = "AND IdentifierContremarque=" + c.Identifier.ToString() + " "
                                                                         End If
                                                                     Else
                                                                         cmq = String.Empty
                                                                     End If

                                                                     Dim query As String

                                                                     'Requête
                                                                     query = "SELECT DISTINCT NumCmd, DateFinalisations, DelaiPrevu FROM Commande as c, Etat as e" + whereEtat + cli + cmq + "Order By" + param

                                                                     'Exécute la requête
                                                                     Objects = connection.ExecuteQuery(query, parameters)

                                                                     parameters = Nothing

                                                                     'Ferme la connection
                                                                     connection.Close()

                                                                     'Traite les résultats
                                                                     For Each obj In Objects
                                                                         Dim cmd As New Commande(Long.Parse(obj(0)))
                                                                         cmd = cmd.GetCommande()

                                                                         'Tri les résultats suivant l'état choisi par l'utilisateur
                                                                         If Me.CbxEtat.SelectedIndex = 0 Then
                                                                             If cmd.Etat.Label <> "Terminée" And cmd.Etat.Label <> "Rendue" Then
                                                                                 ListCommandes.Add(cmd)
                                                                             End If
                                                                         ElseIf Me.CbxEtat.SelectedIndex = 1 Then
                                                                             If cmd.Etat.Label = "Terminée" Then
                                                                                 ListCommandes.Add(cmd)
                                                                             End If
                                                                         ElseIf Me.CbxEtat.SelectedIndex = 2 Then
                                                                             If cmd.Etat.Label = "Rendue" Then
                                                                                 ListCommandes.Add(cmd)
                                                                             End If
                                                                         ElseIf Me.CbxEtat.SelectedIndex = 3 Then
                                                                             If cmd.DelaiPrevu >= minDate Then
                                                                                 ListCommandes.Add(cmd)
                                                                             End If
                                                                         End If
                                                                     Next

                                                                     'S'exécute si seulement une contremarque est sélectionnée
                                                                 ElseIf Me.AutoCompNContremarque.SelectedItem IsNot Nothing Or Me.AutoCompNContremarque.Text <> "" Then
                                                                     Dim Objects As New List(Of List(Of Object))
                                                                     Dim parameters As New List(Of MySqlParameter)

                                                                     'Ouvre la connection
                                                                     connection.Open()

                                                                     Dim cmq As String

                                                                     'Paramètre une partie de la clause WHERE de la requête avec l'identifier de la contremarque
                                                                     If Me.AutoCompNContremarque.SelectedItem IsNot Nothing Then
                                                                         Dim c As Contremarque = Me.AutoCompNContremarque.SelectedItem
                                                                         cmq = "IdentifierContremarque=" + c.Identifier.ToString() + " "
                                                                     Else
                                                                         Dim cm As Contremarque = Nothing

                                                                         For Each item In Me.AutoCompNContremarque.ItemsSource
                                                                             Dim c As Contremarque = item
                                                                             If c.Nom = Me.AutoCompNContremarque.Text.ToUpper() Then
                                                                                 Me.AutoCompNContremarque.SelectedItem = item
                                                                                 cm = c
                                                                                 Exit For
                                                                             End If
                                                                         Next

                                                                         If Me.AutoCompNContremarque.SelectedItem Is Nothing Then Exit Sub

                                                                         cmq = "IdentifierContremarque=" + cm.Identifier.ToString() + " "
                                                                     End If

                                                                     Dim query As String

                                                                     'Requête
                                                                     query = "SELECT DISTINCT NumCmd, DateFinalisations, DelaiPrevu FROM Commande as c, Etat as e" + whereEtat + cmq + "Order By" + param

                                                                     'Exécute la requête
                                                                     Objects = connection.ExecuteQuery(query, parameters)

                                                                     parameters = Nothing

                                                                     'Ferme la connection
                                                                     connection.Close()

                                                                     'Traite les résultats
                                                                     For Each obj In Objects
                                                                         Dim cmd As New Commande(Long.Parse(obj(0)))
                                                                         cmd = cmd.GetCommande()

                                                                         'Trie les résultats en fonction de l'état choisi par l'utilisateur
                                                                         If Me.CbxEtat.SelectedIndex = 0 Then
                                                                             If cmd.Etat.Label <> "Terminée" And cmd.Etat.Label <> "Rendue" Then
                                                                                 ListCommandes.Add(cmd)
                                                                             End If
                                                                         ElseIf Me.CbxEtat.SelectedIndex = 1 Then
                                                                             If cmd.Etat.Label = "Terminée" Then
                                                                                 ListCommandes.Add(cmd)
                                                                             End If
                                                                         ElseIf Me.CbxEtat.SelectedIndex = 2 Then
                                                                             If cmd.Etat.Label = "Rendue" Then
                                                                                 ListCommandes.Add(cmd)
                                                                             End If
                                                                         ElseIf Me.CbxEtat.SelectedIndex = 3 Then
                                                                             If cmd.DelaiPrevu >= minDate Then
                                                                                 ListCommandes.Add(cmd)
                                                                             End If
                                                                         End If
                                                                     Next

                                                                     'S'exécute si le client, la contremarque, le n° de commande et le matériau ne sont pas sélectionnés
                                                                 ElseIf AutoCompNClient.Text = "" And AutoCompNContremarque.Text = "" And AutoCompNumCmd.Text = "" And AutoCompLMateriau.Text = "" Then
                                                                     Dim Objects As New List(Of List(Of Object))
                                                                     Dim parameters As New List(Of MySqlParameter)

                                                                     'Ouvre la connection
                                                                     connection.Open()

                                                                     'Requête
                                                                     Dim query As String
                                                                     query = "SELECT DISTINCT NumCmd, DateFinalisations, DelaiPrevu FROM Commande as c, Etat as e" + whereEtat.Substring(0, whereEtat.Length - 4) + "Order By" + param

                                                                     'Exécute la requête
                                                                     Objects = connection.ExecuteQuery(query, parameters)

                                                                     parameters = Nothing

                                                                     'Ferme la connection
                                                                     connection.Close()

                                                                     'Traite les résultats
                                                                     For Each obj In Objects
                                                                         Dim cmd As Commande = New Commande(Long.Parse(obj(0))).GetCommande()
                                                                         ListCommandes.Add(cmd)
                                                                     Next
                                                                 End If

                                                                 Dim tempSem As Integer = 0
                                                                 Dim sem As Integer = 0
                                                                 Dim pl As New PlanningControl(True)
                                                                 Dim cmdItem As cmdItem
                                                                 Dim color As String = "Transparent"

                                                                 For Each cmd In ListCommandes
                                                                     If Me.CbxTri.SelectedIndex = 0 Then
                                                                         sem = pl.GetWeekOfDate(cmd.DelaiPrevu)
                                                                     Else
                                                                         sem = pl.GetWeekOfDate(cmd.DateCommande)
                                                                     End If

                                                                     If tempSem = 0 Then
                                                                         cmdItem = New cmdItem(cmd, color)
                                                                         tempSem = sem
                                                                     ElseIf tempSem = sem Then
                                                                         cmdItem = New cmdItem(cmd, color)
                                                                     Else
                                                                         color = IIf(color = "Transparent", "#cdd4d4", "Transparent")
                                                                         cmdItem = New cmdItem(cmd, color)
                                                                         tempSem = sem
                                                                     End If

                                                                     Me.LbxSearchCmd.Items.Add(cmdItem)
                                                                 Next

                                                             Catch ex As Exception
                                                                 MessageBox.Show(ex.Message, "Erreur")
                                                             Finally
                                                                 Try
                                                                     'Assure la fermeture de la connection
                                                                     connection.Close()
                                                                 Catch ex As Exception
                                                                 End Try
                                                             End Try
                                                         End Sub))

    End Sub

    ''' <summary>
    ''' Se produit à la fin de la recherche, modifie l'image du bouton de recherhce
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub endSearch()
        Try
            Dim img As New Image()
            Dim bmp As New BitmapImage(New Uri(System.IO.Path.GetFullPath(My.Settings.Search)))
            img.Source = bmp
            Me.BtnSearch.Content = img
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

#End Region

End Class

Public Class cmdItem

#Region "Fields"

    Private _Commande As Commande
    Private _Color As String

#End Region

#Region "Properties"

    Public Property Commande As Commande
        Get
            Return Me._Commande
        End Get
        Set(ByVal value As Commande)
            Me._Commande = value
        End Set
    End Property

    Public Property Color As String
        Get
            Return Me._Color
        End Get
        Set(ByVal value As String)
            Me._Color = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal cmd As Commande, ByVal color As String)
        Me.Commande = cmd
        Me.Color = color
    End Sub

#End Region

End Class
