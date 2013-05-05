Imports MGranitDALcsharp
Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.Globalization
Imports System.IO

Public Class NouvelleCommande

#Region "Fields"
    Private _Session As Session
    Private _IsUpdate As Boolean
    Private _Commande As Commande
    Private _ListMateriaux As New List(Of Materiau)
    Private _ListNatures As New List(Of Nature)
    Private _ListEtat As New List(Of Etat)
    Private _ListMesure As New List(Of Mesure)
    Private _ListFinalisations As New List(Of Finalisation)
    Private _ListEpaisseur As New List(Of Epaisseur)
    Private _ListQualites As New List(Of Qualite)
    Private _Window As Window
    Private _UC As UserControl
    Private _Planning As PlanningControl
    Private _IsRestrictUpdate As Boolean
    Private _IsTextChangeMontant As Boolean
    Private _ListNumPadKeys As List(Of Key)
    Private _ListNumKeys As List(Of Key)
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

    Public Property IsUpdate As Boolean
        Get
            Return Me._IsUpdate
        End Get
        Set(ByVal value As Boolean)
            Me._IsUpdate = value
            If (value And Me.Session.IsDelCmd) Then
                BtnDelete.Visibility = System.Windows.Visibility.Visible
            End If
        End Set
    End Property

    Public Property Commande As Commande
        Get
            Return Me._Commande
        End Get
        Set(ByVal value As Commande)
            Me._Commande = value
            DisplayCommande()
        End Set
    End Property

    Public Property ListMateriaux As List(Of Materiau)
        Get
            Return Me._ListMateriaux
        End Get
        Set(ByVal value As List(Of Materiau))
            Me._ListMateriaux = value
        End Set
    End Property

    Public Property ListNatures As List(Of Nature)
        Get
            Return Me._ListNatures
        End Get
        Set(ByVal value As List(Of Nature))
            Me._ListNatures = value
        End Set
    End Property

    Public Property ListEtats As List(Of Etat)
        Get
            Return Me._ListEtat
        End Get
        Set(ByVal value As List(Of Etat))
            Me._ListEtat = value
        End Set
    End Property

    Public Property ListMesures As List(Of Mesure)
        Get
            Return Me._ListMesure
        End Get
        Set(ByVal value As List(Of Mesure))
            Me._ListMesure = value
        End Set
    End Property

    Public Property ListFinalisations As List(Of Finalisation)
        Get
            Return Me._ListFinalisations
        End Get
        Set(ByVal value As List(Of Finalisation))
            Me._ListFinalisations = value
        End Set
    End Property

    Public Property ListEpaisseurs As List(Of Epaisseur)
        Get
            Return Me._ListEpaisseur
        End Get
        Set(ByVal value As List(Of Epaisseur))
            Me._ListEpaisseur = value
        End Set
    End Property

    Public Property ListQualites As List(Of Qualite)
        Get
            Return Me._ListQualites
        End Get
        Set(ByVal value As List(Of Qualite))
            Me._ListQualites = value
        End Set
    End Property

    Public Property Window As Window
        Get
            Return Me._Window
        End Get
        Set(ByVal value As Window)
            Me._Window = value
        End Set
    End Property

    Public Property UC As UserControl
        Get
            Return Me._UC
        End Get
        Set(ByVal value As UserControl)
            Me._UC = value
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

#Region "Constructors"

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().        

        'Initialisation des combobox
        InitDefault()
        Me.Session = New Session()
        Me.IsUpdate = False
        Me.Commande = Nothing
        Me._IsTextChangeMontant = True

        'Remplit la liste de touches du pavé numériques
        _ListNumPadKeys = New List(Of Key)
        _ListNumPadKeys.Add(Key.NumPad0)
        _ListNumPadKeys.Add(Key.NumPad1)
        _ListNumPadKeys.Add(Key.NumPad2)
        _ListNumPadKeys.Add(Key.NumPad3)
        _ListNumPadKeys.Add(Key.NumPad4)
        _ListNumPadKeys.Add(Key.NumPad5)
        _ListNumPadKeys.Add(Key.NumPad6)
        _ListNumPadKeys.Add(Key.NumPad7)
        _ListNumPadKeys.Add(Key.NumPad8)
        _ListNumPadKeys.Add(Key.NumPad9)

        'Remplit la liste des touches numériques "normales"
        _ListNumKeys = New List(Of Key)
        _ListNumKeys.Add(Key.D0)
        _ListNumKeys.Add(Key.D1)
        _ListNumKeys.Add(Key.D2)
        _ListNumKeys.Add(Key.D3)
        _ListNumKeys.Add(Key.D4)
        _ListNumKeys.Add(Key.D5)
        _ListNumKeys.Add(Key.D6)
        _ListNumKeys.Add(Key.D7)
        _ListNumKeys.Add(Key.D8)
        _ListNumKeys.Add(Key.D9)
    End Sub

#End Region

#Region "Initialisation"

    ''' <summary>
    ''' Initialisation des combobox par défaut
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitDefault()
        CbxSemaineCommande.Items.Add("Sem")
        CbxSemaineCommande.SelectedIndex = 0
        CbxCommandeYear.Items.Add("Année")
        CbxCommandeYear.SelectedIndex = 0

        DpkDateCommande.SelectedDate = Date.Now

        CbxSemainePrevue.Items.Add("Sem")
        CbxSemainePrevue.SelectedIndex = 0
        CbxDelaiPrevuYear.Items.Add("Année")
        CbxDelaiPrevuYear.SelectedIndex = 0

        CbxSemaineFinalisation.Items.Add("Sem")
        CbxSemaineFinalisation.SelectedIndex = 0
        CbxFinalisationYear.Items.Add("Année")
        CbxFinalisationYear.SelectedIndex = 0

        CbxSemaineMesure.Items.Add("Sem")
        CbxSemaineMesure.SelectedIndex = 0
        CbxMesureYear.Items.Add("Année")
        CbxMesureYear.SelectedIndex = 0

        For i = 1 To 53
            CbxSemaineCommande.Items.Add(i)
            CbxSemainePrevue.Items.Add(i)
            CbxSemaineFinalisation.Items.Add(i)
            CbxSemaineMesure.Items.Add(i)
        Next

        For i = 2010 To Date.Now.Year + 2
            CbxCommandeYear.Items.Add(i)
            CbxDelaiPrevuYear.Items.Add(i)
            CbxFinalisationYear.Items.Add(i)
            CbxMesureYear.Items.Add(i)
        Next

        Dim Objects As New List(Of List(Of Object))
        Dim connection As New MGConnection(My.Settings.DBSource)

        Try
            connection.Open()

            ListEtats = Etat.GetEtats()

            ListMesures = Mesure.GetMesures()

            ListMateriaux = Materiau.GetMateriaux()

            ListNatures = Nature.GetNatures()

            ListFinalisations = Finalisation.GetFinalisations()

            ListEpaisseurs = Epaisseur.GetEpaisseurs()

            ListQualites = Qualite.GetQualites()

            connection.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                connection.Close()
            Catch ex As Exception

            End Try
        End Try

        TxtNumCmd.Focus()
    End Sub

    ''' <summary>
    ''' Permet d'afficher les informations d'une commande
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisplayCommande()
        Me.LbxMateriaux.Items.Clear()
        Me.LbxNatures.Items.Clear()
        Me.LbxFinalisations.Items.Clear()

        Dim Objects As New List(Of List(Of Object))
        Dim connection As New MGConnection(My.Settings.DBSource)

        Me.CbxEtat.ItemsSource = ListEtats
        Me.CbxEtat.SelectedIndex = 0

        Me.CbxQualite.ItemsSource = ListQualites

        Me.CbxMesure.ItemsSource = ListMesures

        If (Me.Commande IsNot Nothing AndAlso Me.Commande.Client IsNot Nothing) Then
            Me.TxtNumCmd.Text = Me.Commande.NoCommande

            Dim cli As New List(Of Client)
            cli.Add(Me.Commande.Client)
            Me.AutoCompNClient.ItemsSource = cli
            Me.AutoCompNClient.PopulateComplete()
            Me.AutoCompNClient.Tag = New Client()
            Me.AutoCompNClient.Tag = Me.Commande.Client
            Me.AutoCompNClient.Text = Me.Commande.Client.Nom

            Me.AutoCompNContremarque.Tag = New Contremarque()
            If Me.Commande.Contremarque IsNot Nothing Then
                Me.AutoCompNContremarque.ItemsSource = Nothing
                Dim cmq As New List(Of Contremarque)
                cmq.Add(Me.Commande.Contremarque)
                Me.AutoCompNContremarque.ItemsSource = cmq
                Me.AutoCompNContremarque.PopulateComplete()
                Me.AutoCompNContremarque.Tag = Me.Commande.Contremarque
                Me.AutoCompNContremarque.Text = Me.Commande.Contremarque.Nom
            End If

            Me.DpkDateCommande.SelectedDate = Me.Commande.DateCommande

            For Each m In ListMateriaux
                Dim isExists As Boolean = False

                For Each Mat In Me.Commande.Materiaux
                    If (Mat.Identifier = m.Identifier) Then
                        isExists = True
                        m = Mat
                        Exit For
                    End If
                Next

                Dim mt As New MateriauTemplate(m, isExists)
                Me.LbxMateriaux.Items.Add(mt)
            Next

            For Each n In ListNatures
                Dim isExists As Boolean = False

                For Each Nat In Me.Commande.Natures
                    If (Nat.Identifier = n.Identifier) Then
                        isExists = True
                        Exit For
                    End If
                Next

                Dim nt As New NatureTemplate(n, isExists)
                Me.LbxNatures.Items.Add(nt)
            Next

            For Each item In CbxEtat.Items
                If (Me.Commande.Etat.Equals(item)) Then
                    CbxEtat.SelectedItem = item
                    Exit For
                End If
            Next

            Dim heure As Integer
            Dim minute As Integer

            Dim temp() As String = convertMinuteToHourMinute(Me.Commande.TpsDebit).Split(";")
            heure = temp(0)
            minute = temp(1)
            Me.TxtTpsDebitH.Text = heure
            Me.TxtTpsDebitM.Text = minute

            temp = convertMinuteToHourMinute(Me.Commande.TpsCommandeNumerique).Split(";")
            heure = temp(0)
            minute = temp(1)
            Me.TxtTpsCmdNumH.Text = heure
            Me.TxtTpsCmdNumM.Text = minute

            temp = convertMinuteToHourMinute(Me.Commande.TpsFinition).Split(";")
            heure = temp(0)
            minute = temp(1)
            Me.TxtTpsFinitionH.Text = heure
            Me.TxtTpsFinitionM.Text = minute

            temp = convertMinuteToHourMinute(Me.Commande.TpsAutres).Split(";")
            heure = temp(0)
            minute = temp(1)
            Me.TxtTpsAutresH.Text = heure
            Me.TxtTpsAutresM.Text = minute

            Me.DpkDelaiPrevu.SelectedDate = Me.Commande.DelaiPrevu

            For Each f In ListFinalisations
                Dim isExists As Boolean = False

                For Each Fin In Me.Commande.Finalisations
                    If (Fin.Identifier = f.Identifier) Then
                        isExists = True
                        Exit For
                    End If
                Next

                Dim ft As New FinalisationTemplate(f, isExists)
                Me.LbxFinalisations.Items.Add(ft)
            Next

            Me.DpkFinalisation.SelectedDate = Me.Commande.DateFinalisations
            heure = Me.Commande.DateFinalisations.Hour
            minute = Me.Commande.DateFinalisations.Minute
            Dim hour As String
            Dim min As String
            If heure < 10 Then hour = "0" + heure.ToString() Else hour = heure.ToString()
            If minute < 10 Then min = "0" + minute.ToString() Else min = minute.ToString()

            Me.TxtRdvFinalisation.Text = hour + "h" + min

            Me._IsTextChangeMontant = False
            Dim cc As New ChiffreConverter
            Dim tempMontant As String = cc.Convert(Me.Commande.MontantHT, Nothing, Nothing, Nothing)
            Me.TxtMontant.Text = tempMontant.Substring(0, tempMontant.Length - 2)

            Me.TxtArrhes.Text = Me.Commande.Arrhes

            For Each item In CbxMesure.Items
                If (Me.Commande.Mesure.Equals(item)) Then
                    CbxMesure.SelectedItem = item
                    Exit For
                End If
            Next

            If Not Me.Commande.DateMesure = DateTime.MinValue Then
                Me.DpkMesure.SelectedDate = Me.Commande.DateMesure
            Else
                Me.DpkMesure.SelectedDate = Nothing
            End If

            heure = Me.Commande.DateMesure.Hour
            minute = Me.Commande.DateMesure.Minute
            If heure < 10 Then hour = "0" + heure.ToString() Else hour = heure.ToString()
            If minute < 10 Then min = "0" + minute.ToString() Else min = minute.ToString()

            Me.TxtRdvMesure.Text = hour + "h" + min

            temp = Me.Commande.AdresseChantier.Split(";")
            Me.TxtAdresse.Text = temp(0)
            Me.TxtCodePostal.Text = temp(1)
            Me.TxtVille.Text = temp(2)

            For Each rm In Me.Commande.Remarques
                DgRemarques.Items.Add(rm)
            Next

            For Each q In Me.Commande.Qualites
                DgQualites.Items.Add(q)
            Next

        Else
            For Each m In ListMateriaux
                Dim mt As New MateriauTemplate(m)
                Me.LbxMateriaux.Items.Add(mt)
            Next

            For Each n In ListNatures
                Dim nt As New NatureTemplate(n)
                Me.LbxNatures.Items.Add(nt)
            Next

            For Each f In ListFinalisations
                Dim ft As New FinalisationTemplate(f)
                Me.LbxFinalisations.Items.Add(ft)
            Next
        End If

        If (Me.Session IsNot Nothing AndAlso Not Me.Session.IsAddCmd And Me.Session.IsUpdCmd) Then
            Me.StDateCommande.IsEnabled = False
            Me.TxtNumCmd.IsEnabled = False
            Me.AutoCompNClient.IsEnabled = False
            Me.AutoCompNContremarque.IsEnabled = False
            Me.StDelaiPrevu.IsEnabled = False
            Me.StMontant.Visibility = Windows.Visibility.Collapsed
            Me.StDateFinalisation.IsEnabled = False
            Me.CbxMesure.IsEnabled = False
            Me.StAdresse.IsEnabled = False
            Me.StDateReleves.IsEnabled = False

            For Each ch In StMateriaux.Children
                If (TypeOf (ch) Is Expander) Then
                    Dim exp As Expander = ch
                    exp.IsExpanded = True
                End If
            Next

            Dim index As New List(Of Integer)
            For Each item In Me.LbxMateriaux.Items
                Dim mt As MateriauTemplate = item
                If Not mt.IsChecked Then
                    index.Add(Me.LbxMateriaux.Items.IndexOf(item))
                End If
            Next

            For i = index.Count - 1 To 0 Step -1
                Dim j As Integer = index(i)
                Me.LbxMateriaux.Items.RemoveAt(j)
            Next

            For Each ch In StNatures.Children
                If (TypeOf (ch) Is Expander) Then
                    Dim exp As Expander = ch
                    exp.IsExpanded = True
                End If
            Next

            index = New List(Of Integer)
            For Each item In Me.LbxNatures.Items
                Dim nt As NatureTemplate = item
                If Not nt.IsChecked Then
                    index.Add(Me.LbxNatures.Items.IndexOf(item))
                End If
            Next

            For i = index.Count - 1 To 0 Step -1
                Dim j As Integer = index(i)
                Me.LbxNatures.Items.RemoveAt(j)
            Next

            For Each ch In StFinalisations.Children
                If (TypeOf (ch) Is Expander) Then
                    Dim exp As Expander = ch
                    exp.IsExpanded = True
                End If
            Next

            Me._IsRestrictUpdate = True
        ElseIf Me.Session IsNot Nothing AndAlso Me.Session.IsDelCmd Then
            If Me.Session.IsDelCmd Then
                Me.BtnDeleteRemarque.Visibility = Windows.Visibility.Visible
                Me.BtnDeleteQualite.Visibility = Windows.Visibility.Visible
            End If

            'trie les matériaux sélectionnés (les place en début de liste)
            Dim index As New List(Of Integer)
            For Each item In Me.LbxMateriaux.Items
                Dim mt As MateriauTemplate = item
                If mt.IsChecked Then
                    index.Add(Me.LbxMateriaux.Items.IndexOf(item))
                End If
            Next

            Dim listChecked = New List(Of MateriauTemplate)
            For i = index.Count - 1 To 0 Step -1
                Dim j As Integer = index(i)
                listChecked.Add(Me.LbxMateriaux.Items.GetItemAt(j))
                Me.LbxMateriaux.Items.RemoveAt(j)
            Next

            For Each mt In listChecked
                Me.LbxMateriaux.Items.Insert(0, mt)
            Next

            'trie les natures sélectionnées (les place en début de liste)
            index = New List(Of Integer)
            For Each item In Me.LbxNatures.Items
                Dim nt As NatureTemplate = item
                If nt.IsChecked Then
                    index.Add(Me.LbxNatures.Items.IndexOf(item))
                End If
            Next

            Dim listCheckedNat = New List(Of NatureTemplate)
            For i = index.Count - 1 To 0 Step -1
                Dim j As Integer = index(i)
                listCheckedNat.Add(Me.LbxNatures.Items.GetItemAt(j))
                Me.LbxNatures.Items.RemoveAt(j)
            Next

            For Each nt In listCheckedNat
                Me.LbxNatures.Items.Insert(0, nt)
            Next
        Else
            Me._IsRestrictUpdate = False
        End If

    End Sub

#End Region

#Region "SelectionChanged"

    ''' <summary>
    ''' Évènement se produisant lorsque l'item sélectionné dans la combobox CbxSemainePrevue ou dans la combobox CbxDelaiPrevuYear est modifié
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ManuelleDatePrevue_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Try
            If (CbxSemainePrevue.IsDropDownOpen Or CbxDelaiPrevuYear.IsDropDownOpen) Then
                If (CbxSemainePrevue.SelectedIndex <> 0 AndAlso CbxDelaiPrevuYear.SelectedIndex <> 0) Then
                    Dim pl As New PlanningControl()
                    DpkDelaiPrevu.SelectedDate = pl.GetDaysOfWeek(CbxSemainePrevue.SelectedItem, CbxDelaiPrevuYear.SelectedItem).ElementAt(0)
                ElseIf (CbxDelaiPrevuYear.SelectedIndex = 0 And CbxSemainePrevue.SelectedIndex <> 0) Then
                    Dim pl As New PlanningControl()
                    DpkDelaiPrevu.SelectedDate = pl.GetDaysOfWeek(CbxSemainePrevue.SelectedItem, Date.Now.Year).ElementAt(0)
                ElseIf (CbxSemainePrevue.SelectedIndex = 0 AndAlso CbxDelaiPrevuYear.SelectedIndex = 0) Then
                    DpkDelaiPrevu.SelectedDate = Nothing
                End If
            End If
        Catch ex As Exception
            If (CbxSemainePrevue.SelectedIndex = 52) Then
                MessageBox.Show("L'année sélectionnée ne contient que 52 semaines", "Erreur")
            Else
                MessageBox.Show(ex.Message, "Erreur")
            End If

            CbxSemainePrevue.SelectedIndex = 0
            CbxDelaiPrevuYear.SelectedIndex = 0
            DpkDelaiPrevu.SelectedDate = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Évènement se produisant lorsque l'item sélectionné dans la combobox CbxSemaineFinalisation ou dans la combobox CbxFinalisationYear est modifié
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ManuelleDateFinalisation_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Try
            If (CbxSemaineFinalisation.IsDropDownOpen Or CbxFinalisationYear.IsDropDownOpen) Then
                If (CbxSemaineFinalisation.SelectedIndex <> 0 AndAlso CbxFinalisationYear.SelectedIndex <> 0) Then
                    Dim pl As New PlanningControl()
                    DpkFinalisation.SelectedDate = pl.GetDaysOfWeek(CbxSemaineFinalisation.SelectedItem, CbxFinalisationYear.SelectedItem).ElementAt(0)
                ElseIf (CbxFinalisationYear.SelectedIndex = 0 And CbxSemaineFinalisation.SelectedIndex <> 0) Then
                    Dim pl As New PlanningControl()
                    DpkFinalisation.SelectedDate = pl.GetDaysOfWeek(CbxSemaineFinalisation.SelectedItem, Date.Now.Year).ElementAt(0)
                ElseIf (CbxSemaineFinalisation.SelectedIndex = 0 AndAlso CbxFinalisationYear.SelectedIndex = 0) Then
                    DpkFinalisation.SelectedDate = Nothing
                End If
            End If
        Catch ex As Exception
            If (CbxSemaineFinalisation.SelectedIndex = 52) Then
                MessageBox.Show("L'année sélectionnée ne contient que 52 semaines", "Erreur")
            Else
                MessageBox.Show(ex.Message, "Erreur")
            End If

            CbxSemaineFinalisation.SelectedIndex = 0
            CbxFinalisationYear.SelectedIndex = 0
            DpkFinalisation.SelectedDate = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Évènement se produisant lorsque l'item sélectionné dans la combobox CbxSemaineMesure ou dans la combobox CbxMesureYear est modifié
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ManuelleDateMesure_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Try
            If (CbxSemaineMesure.IsDropDownOpen Or CbxMesureYear.IsDropDownOpen) Then
                If (CbxSemaineMesure.SelectedIndex <> 0 AndAlso CbxMesureYear.SelectedIndex <> 0) Then
                    Dim pl As New PlanningControl()
                    DpkMesure.SelectedDate = pl.GetDaysOfWeek(CbxSemaineMesure.SelectedItem, CbxMesureYear.SelectedItem).ElementAt(0)
                ElseIf (CbxMesureYear.SelectedIndex = 0 And CbxSemaineMesure.SelectedIndex <> 0) Then
                    Dim pl As New PlanningControl()
                    DpkMesure.SelectedDate = pl.GetDaysOfWeek(CbxSemaineMesure.SelectedItem, Date.Now.Year).ElementAt(0)
                ElseIf (CbxSemaineMesure.SelectedIndex = 0 AndAlso CbxMesureYear.SelectedIndex = 0) Then
                    DpkMesure.SelectedDate = Nothing
                End If
            End If
        Catch ex As Exception
            If (CbxSemaineMesure.SelectedIndex = 53) Then
                MessageBox.Show("L'année sélectionnée ne contient que 52 semaines", "Erreur")
            Else
                MessageBox.Show(ex.Message, "Erreur")
            End If

            Me.CbxSemaineMesure.SelectedIndex = 0
            Me.CbxMesureYear.SelectedIndex = 0
            Me.DpkMesure.SelectedDate = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Évènement se produisant lorsque l'item sélectionné dans la combobox CbxSemaineCommande ou dans la combobox CbxCommandeYear est modifié
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ManuelleDateCommande_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Try
            If (CbxSemaineCommande.IsDropDownOpen Or CbxCommandeYear.IsDropDownOpen) Then
                If (CbxSemaineCommande.SelectedIndex <> 0 AndAlso CbxCommandeYear.SelectedIndex <> 0) Then
                    Dim pl As New PlanningControl()
                    DpkDateCommande.SelectedDate = pl.GetDaysOfWeek(CbxSemaineCommande.SelectedItem, CbxCommandeYear.SelectedItem).ElementAt(0)
                ElseIf (CbxCommandeYear.SelectedIndex = 0 And CbxSemaineCommande.SelectedIndex <> 0) Then
                    Dim pl As New PlanningControl()
                    DpkDateCommande.SelectedDate = pl.GetDaysOfWeek(CbxSemaineCommande.SelectedItem, Date.Now.Year).ElementAt(0)
                ElseIf (CbxSemaineCommande.SelectedIndex = 0 AndAlso CbxCommandeYear.SelectedIndex = 0) Then
                    DpkDateCommande.SelectedDate = Nothing
                End If
            End If
        Catch ex As Exception
            If (CbxSemaineCommande.SelectedIndex = 52) Then
                MessageBox.Show("L'année sélectionnée ne contient que 52 semaines", "Erreur")
            Else
                MessageBox.Show(ex.Message, "Erreur")
            End If

            CbxSemaineCommande.SelectedIndex = 0
            CbxCommandeYear.SelectedIndex = 0
            DpkDateCommande.SelectedDate = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Évènement se produisant lorsque la date sélectionnée du DatePicker DpkDelaiPrevu est modifiée
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DpkDelaiPrevu_SelectedDateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim pl As New PlanningControl()
        If (IsNothing(DpkDelaiPrevu) = False And DpkDelaiPrevu IsNot Nothing AndAlso DpkDelaiPrevu.SelectedDate IsNot Nothing) Then
            Try
                CbxSemainePrevue.SelectedItem = pl.GetWeekOfDate(DpkDelaiPrevu.SelectedDate)
                CbxDelaiPrevuYear.SelectedItem = DpkDelaiPrevu.SelectedDate.Value.Year
                Me.DpkFinalisation.SelectedDate = DpkDelaiPrevu.SelectedDate
            Catch
                MessageBox.Show("Date indisponible.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        Else
            CbxSemainePrevue.SelectedIndex = 0
            CbxDelaiPrevuYear.SelectedIndex = 0
        End If
    End Sub

    ''' <summary>
    ''' Évènement se produisant lorsque la date sélectionnée du DatePicker DpkFinalisation est modifiée
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DpkFinalisation_SelectedDateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim pl As New PlanningControl()
        If (IsNothing(DpkFinalisation) = False And DpkFinalisation IsNot Nothing AndAlso DpkFinalisation.SelectedDate IsNot Nothing) Then
            Try
                CbxSemaineFinalisation.SelectedItem = pl.GetWeekOfDate(DpkFinalisation.SelectedDate)
                CbxFinalisationYear.SelectedItem = DpkFinalisation.SelectedDate.Value.Year
            Catch
                MessageBox.Show("Date indisponible.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        Else
            CbxSemaineFinalisation.SelectedIndex = 0
            CbxFinalisationYear.SelectedIndex = 0
        End If
    End Sub

    ''' <summary>
    ''' Évènement se produisant lorsque la date sélectionnée du DatePicker DpkMesure est modifiée
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DpkMesure_SelectedDateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim pl As New PlanningControl()
        If (IsNothing(DpkMesure) = False And DpkMesure.SelectedDate IsNot Nothing AndAlso DpkMesure.SelectedDate IsNot Nothing) Then
            Try
                CbxSemaineMesure.SelectedItem = pl.GetWeekOfDate(DpkMesure.SelectedDate)
                CbxMesureYear.SelectedItem = DpkMesure.SelectedDate.Value.Year
            Catch
                MessageBox.Show("Date indisponible.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        Else
            CbxSemaineMesure.SelectedIndex = 0
            CbxMesureYear.SelectedIndex = 0
        End If
    End Sub

    ''' <summary>
    ''' Évènement se produisant lorsque la date sélectionnée du DatePicker DpkDateCommande est modifiée
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DpkDateCommande_SelectedDateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim pl As New PlanningControl()
        If (IsNothing(DpkDateCommande) = False And DpkDateCommande.SelectedDate IsNot Nothing AndAlso DpkDateCommande.SelectedDate IsNot Nothing) Then
            Try
                CbxSemaineCommande.SelectedItem = pl.GetWeekOfDate(DpkDateCommande.SelectedDate)
                CbxCommandeYear.SelectedItem = DpkDateCommande.SelectedDate.Value.Year
            Catch
                MessageBox.Show("Date indisponible.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        Else
            CbxSemaineCommande.SelectedIndex = 0
            CbxCommandeYear.SelectedIndex = 0
        End If
    End Sub

    ''' <summary>
    ''' Évènement se produisant lorsque l'item sélectionné de l'AutoCompleteBox matériaux change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AutoCompLMateriaux_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim mat As Integer = 0

        For i = 0 To LbxMateriaux.Items.Count - 1
            Dim mt As MateriauTemplate = LbxMateriaux.Items.GetItemAt(i)
            If mt.Label = Me.AutoCompLMateriaux.Text Then
                mat = i
            End If
        Next

        Me.LbxMateriaux.ScrollIntoView(LbxMateriaux.Items.GetItemAt(mat))
    End Sub

#End Region

#Region "TextChanged"

    ''' <summary>
    ''' Se produit lorsque l'un des paramètres servant à calculer le temps de fabrication est modifié
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TpsFabrication_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs)
        Dim tbx As TextBox = sender
        Dim cursor As Integer = tbx.CaretIndex

        Dim isOk As Boolean = Integer.TryParse(tbx.Text, 0)
        If (Not isOk And tbx.Text <> "") Then
            tbx.Text = tbx.Text.Remove(cursor - 1, 1)
            tbx.CaretIndex = cursor - 1
        Else
            If (Not IsNothing(TxtTpsDebitH) And Not IsNothing(TxtTpsCmdNumH) And Not IsNothing(TxtTpsFinitionH) And Not IsNothing(TxtTpsAutresH) And Not IsNothing(TxtTpsTotH) And _
                Not IsNothing(TxtTpsTotM) And tbx.Text <> "" And Not IsNothing(TxtTpsDebitM) And Not IsNothing(TxtTpsCmdNumM) And Not IsNothing(TxtTpsFinitionM) And _
                Not IsNothing(TxtTpsAutresM)) Then

                If (tbx.Tag = "M") Then If (Integer.Parse(tbx.Text) > 59) Then tbx.Text = 59

                Dim temps() As String = convertMinuteToHourMinute((Integer.Parse(TxtTpsDebitH.Text) * 60) +
                                                                  (Integer.Parse(TxtTpsCmdNumH.Text) * 60) +
                                                                  (Integer.Parse(TxtTpsFinitionH.Text) * 60) +
                                                                  (Integer.Parse(TxtTpsAutresH.Text) * 60) +
                                                                   Integer.Parse(TxtTpsDebitM.Text) +
                                                                   Integer.Parse(TxtTpsCmdNumM.Text) +
                                                                   Integer.Parse(TxtTpsFinitionM.Text) +
                                                                   Integer.Parse(TxtTpsAutresM.Text)).Split(";")

                TxtTpsTotH.Text = temps(0)
                TxtTpsTotM.Text = temps(1)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Se produit lorsque l'une des TextBox servant à calculer le temps de fabrication perd le focus
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TpsFabrication_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim tbx As TextBox = sender

        If (tbx.Text = "") Then tbx.Text = 0
    End Sub

    ''' <summary>
    ''' Se produit lorsqu'un champ contenant un montant est modifié
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Montant_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs)
        If Me.Session IsNot Nothing AndAlso Me.Session.IsAddCmd Then
            If Me._IsTextChangeMontant Then
                Dim tbx As TextBox = sender
                tbx.Text.Replace(" ", "")
                Dim cursor As Integer = tbx.CaretIndex

                Dim isOk = Decimal.TryParse(tbx.Text, 0)

                If (isOk And tbx.Text.Length <> 0 AndAlso tbx.Text.Substring(tbx.Text.Length - 1, 1) <> ",") Then
                    tbx.Text = Math.Round(Decimal.Parse(tbx.Text), 2)
                    tbx.CaretIndex = cursor
                ElseIf ((tbx.Text.Length >= 1 AndAlso tbx.Text.Substring(tbx.Text.Length - 1, 1) <> "," AndAlso tbx.Text.Substring(tbx.Text.Length - 1, 1) <> ".") Or tbx.Text.Length = 1) Then
                    tbx.Text = tbx.Text.Remove(cursor - 1, 1)
                    tbx.CaretIndex = cursor - 1
                ElseIf (tbx.Text.Length > 1 AndAlso tbx.Text.Substring(tbx.Text.Length - 1, 1) = ".") Then
                    If (Not tbx.Text.Remove(tbx.Text.Length - 1, 1).Contains(",")) Then
                        Dim temp As String = tbx.Text + ","
                        temp = temp.Remove(tbx.Text.Length - 1, 1)
                        tbx.Text = temp
                        tbx.CaretIndex = cursor
                    Else
                        tbx.Text = tbx.Text.Remove(tbx.Text.Length - 1, 1)
                        tbx.CaretIndex = cursor - 1
                    End If
                ElseIf (tbx.Text.Length > 1 AndAlso tbx.Text.Substring(tbx.Text.Length - 1, 1) <> ".") Then
                    If (tbx.Text.Remove(tbx.Text.Length - 1, 1).Contains(",")) Then
                        tbx.Text = tbx.Text.Remove(tbx.Text.Length - 1, 1)
                        tbx.CaretIndex = cursor - 1
                    End If
                End If
            End If
        Else
            Me._IsTextChangeMontant = True
        End If
    End Sub

    ''' <summary>
    ''' Se produit lorsqu'un champ contenant un montant perd le focus
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Montant_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim txt As TextBox = sender

        If (txt.Text = "") Then
            txt.Text = Decimal.Parse("0,00")
        Else
            Dim tempMontant As String = FormatNumber(Decimal.Parse(txt.Text), 2).ToString()
            Me._IsTextChangeMontant = False
            txt.Text = tempMontant
        End If
    End Sub

    ''' <summary>
    ''' Se produit lorsqu'un champ contenant un horaire est modifié
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TxtHoraire_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs)
        Dim tbx As TextBox = sender

        Dim cursor As Integer = tbx.CaretIndex

        If (tbx.Text.Length <> 5 And cursor <> 3 And cursor <> 6) Then
            tbx.Text = tbx.Text.Remove(cursor, 1)
            tbx.CaretIndex = cursor
        ElseIf cursor = 3 Then
            Dim tempString As String = tbx.Text.Substring(cursor - 1, 1)
            Dim tempTbx As String = tbx.Text
            tempTbx = tempTbx.Remove(cursor - 1, 1)
            tempTbx = tempTbx.Insert(cursor, tempString)
            cursor += 1
            tempTbx = tempTbx.Remove(cursor, 1)
            tbx.Text = tempTbx

            tbx.CaretIndex = cursor
        ElseIf cursor = 6 Then
            tbx.Text = tbx.Text.Remove(cursor - 1)
            tbx.CaretIndex = cursor - 1
        End If

        If (Integer.Parse(tbx.Text.Substring(0, 2)) > 23) Then
            Dim temp As String = tbx.Text
            temp = temp.Remove(0, 2)
            temp = temp.Insert(0, "23")
            tbx.Text = temp
            tbx.CaretIndex = cursor
        End If

        If (Integer.Parse(tbx.Text.Substring(3, 2)) > 59) Then
            Dim temp As String = tbx.Text
            temp = temp.Remove(3, 2)
            temp = temp.Insert(3, "59")
            tbx.Text = temp
            tbx.CaretIndex = cursor
        End If
    End Sub

    ''' <summary>
    ''' Se produit lorqu'une touche est enfoncée sur un textbox horaire
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TxtHoraire_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        Dim tbx As TextBox = sender

        If (e.Key = Key.Back Or e.Key = Key.Delete Or e.Key = 88 Or tbx.SelectedText.Length > 1) Then
            e.Handled = True
        End If
    End Sub

#End Region

#Region "Button"

    ''' <summary>
    ''' Bouton permettant d'ajouter une remarque
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnAddRemarque_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If (TxtNewRemarque.Text <> "") Then
            If (Me.Session.Login = "") Then
                If (TypeOf (Parent) Is TabItem) Then
                    Dim ti As TabItem = Parent
                    Dim tc As TabControl = ti.Parent
                    Dim gd As Grid = tc.Parent
                    gd = gd.Parent
                    Dim main As MainWindow = gd.Parent
                    Me.Session = main.Session
                End If
            End If

            If (Me.Session.Login <> "") Then

                Dim culture As New CultureInfo("fr-FR")
                Dim dates As DateTime = Date.Now


                Dim remark As New Remarque(TxtNewRemarque.Text, Me.Session.Login, dates.ToString("g", culture.DateTimeFormat))
                DgRemarques.Items.Add(remark)

                If Me.Planning IsNot Nothing Then Me.Planning.Fill()
            End If

            Me.TxtNewRemarque.Text = String.Empty
        End If
    End Sub

    ''' <summary>
    ''' Bouton permettant d'ajouter un problème de qualité
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnAddQualite_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If (CbxQualite.SelectedIndex >= 0) Then
            If (Me.Session.Login = "") Then
                If (TypeOf (Parent) Is TabItem) Then
                    Dim ti As TabItem = Parent
                    Dim tc As TabControl = ti.Parent
                    Dim gd As Grid = tc.Parent
                    gd = gd.Parent
                    Dim main As MainWindow = gd.Parent
                    Me.Session = main.Session
                End If
            End If

            If (Me.Session.Login <> "") Then
                Dim dates As DateTime = Date.Now

                Dim qualite As Qualite = CbxQualite.SelectedItem
                qualite.DatePost = dates
                qualite.Source = Me.Session.Login
                qualite.Remarque = Me.TxtCommentaire.Text

                Me.DgQualites.Items.Add(qualite)

                If Me.Planning IsNot Nothing Then Me.Planning.Fill()
            End If

            Me.CbxQualite.SelectedItem = Nothing
            Me.TxtCommentaire.Text = String.Empty
        End If
    End Sub

    ''' <summary>
    ''' Bouton permettant de valider la commande
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnSauvegarde_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim messageError As String = String.Empty
        Dim numCmd As Integer = 0
        Dim client As Client = Nothing
        Dim dateCommande As Date
        Dim materiaux As New List(Of Materiau)
        Dim natures As New List(Of Nature)
        Dim delaiPrevu As Date
        Dim finalisations As New List(Of Finalisation)
        Dim dateFinalisation As Date
        Dim dateReleve As Date
        Dim releve As New Mesure()

        Try

            'Récupère la contremarque
            Dim contremarque As Contremarque = Nothing
            If TypeOf (Me.AutoCompNContremarque.SelectedItem) Is Contremarque Then
                contremarque = Me.AutoCompNContremarque.SelectedItem
            ElseIf Me.AutoCompNContremarque.Text <> "" Then
                If Not Me.IsUpdate Then
                    contremarque = New Contremarque(Me.AutoCompNContremarque.Text.ToUpper())
                    For Each item In Me.AutoCompNContremarque.ItemsSource
                        Dim cmq As Contremarque = item
                        If contremarque.Nom = cmq.Nom Then
                            contremarque = cmq
                        End If
                    Next
                Else
                    If Me.AutoCompNContremarque.Tag IsNot Nothing Then
                        Dim cm As Contremarque = Me.AutoCompNContremarque.Tag
                        If cm.Nom = Me.AutoCompNContremarque.Text Then
                            contremarque = cm
                        Else
                            contremarque = New Contremarque(Me.AutoCompNContremarque.Text.ToUpper())
                            For Each item In Me.AutoCompNContremarque.ItemsSource
                                Dim cmq As Contremarque = item
                                If contremarque.Nom = cmq.Nom Then
                                    contremarque = cmq
                                End If
                            Next
                        End If
                    End If
                End If
            End If

            'Récupère l'état de la commande
            Dim etat As Etat = Me.CbxEtat.SelectedItem

            'Récupère les paramètres du temps de fabrication
            Dim tpsDebit As Integer = Integer.Parse(Me.TxtTpsDebitH.Text) * 60 + Integer.Parse(Me.TxtTpsDebitM.Text)
            Dim tpsCmdNumerique As Integer = Integer.Parse(Me.TxtTpsCmdNumH.Text) * 60 + Integer.Parse(Me.TxtTpsCmdNumM.Text)
            Dim tpsFinition As Integer = Integer.Parse(Me.TxtTpsFinitionH.Text) * 60 + Integer.Parse(Me.TxtTpsFinitionM.Text)
            Dim tpsAutres As Integer = Integer.Parse(Me.TxtTpsAutresH.Text) * 60 + Integer.Parse(Me.TxtTpsAutresM.Text)

            'Récupère le montant HT et les arrhes versées
            Dim montant As Decimal
            Dim arrhes As Decimal
            If Me.Session.IsAddCmd Then
                montant = Decimal.Parse(Me.TxtMontant.Text)
                arrhes = Decimal.Parse(Me.TxtArrhes.Text)
            Else
                montant = 0
                arrhes = 0
            End If

            'Récupère la date des relevés
            If (Me.DpkMesure.SelectedDate IsNot Nothing) Then
                Dim heure As Integer
                Dim minute As Integer
                heure = Integer.Parse(TxtRdvMesure.Text.Substring(0, 2))
                Minute = Integer.Parse(TxtRdvMesure.Text.Substring(3, 2))

                dateReleve = Me.DpkMesure.SelectedDate
                If (heure <> 0 Or minute <> 0) Then
                    dateReleve = New DateTime(dateReleve.Year, dateReleve.Month, dateReleve.Day, heure, minute, 0)
                End If
            End If

            'Récupère l'adresse du chantier
            Dim adresse As String = Me.TxtAdresse.Text + ";" + Me.TxtCodePostal.Text + ";" + Me.TxtVille.Text

            'Récupère les remarques
            Dim remarques As New List(Of Remarque)
            For Each item In DgRemarques.Items
                Dim rm As Remarque = item
                remarques.Add(rm)
            Next

            'Récupère les problèmes de qualité
            Dim qualites As New List(Of Qualite)
            For Each item In DgQualites.Items
                Dim q As Qualite = item
                qualites.Add(q)
            Next

            'Récupère le numéro de commande
            If (Integer.TryParse(Me.TxtNumCmd.Text, 0) AndAlso Integer.Parse(Me.TxtNumCmd.Text) <> 0) Then
                numCmd = Integer.Parse(Me.TxtNumCmd.Text)

                'Récupère le client
                If Me.AutoCompNClient.SelectedItem IsNot Nothing Or (Me.AutoCompNClient.SelectedItem Is Nothing And Me.AutoCompNClient.Text <> "") Then
                    If Me.AutoCompNClient.SelectedItem Is Nothing And Me.AutoCompNClient.Text <> "" Then
                        If Not Me.IsUpdate Then
                            client = New Client(Me.AutoCompNClient.Text.ToUpper())
                            For Each item In Me.AutoCompNClient.ItemsSource
                                Dim cli As Client = item
                                If client.Nom = cli.Nom Then
                                    client = cli
                                End If
                            Next
                        Else
                            If Me.AutoCompNClient.Tag IsNot Nothing Then
                                Dim c As Client = Me.AutoCompNClient.Tag
                                If c.Nom = Me.AutoCompNClient.Text Then
                                    client = Me.AutoCompNClient.Tag
                                Else
                                    client = New Client(Me.AutoCompNClient.Text.ToUpper())
                                    For Each item In Me.AutoCompNClient.ItemsSource
                                        Dim cli As Client = item
                                        If client.Nom = cli.Nom Then
                                            client = cli
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    ElseIf Me.AutoCompNClient.SelectedItem IsNot Nothing Then
                        client = Me.AutoCompNClient.SelectedItem
                    End If

                    'Récupère la date de commande
                    If (DpkDateCommande.SelectedDate IsNot Nothing) Then
                        dateCommande = Me.DpkDateCommande.SelectedDate

                        'Récupère la liste de matériaux
                        If (Me.LbxMateriaux.Items.Count <> 0) Then
                            For Each item In Me.LbxMateriaux.Items
                                Dim mt As MateriauTemplate = item
                                If (mt.IsChecked) Then materiaux.Add(New Materiau(mt.Label, mt.Identifier, mt.Epaisseur))
                            Next

                            'Récupère la liste de natures
                            If (Me.LbxNatures.Items.Count <> 0) Then
                                For Each item In Me.LbxNatures.Items
                                    Dim nt As NatureTemplate = item
                                    If (nt.IsChecked) Then natures.Add(New Nature(nt.Label, nt.Identifier))
                                Next

                                'Récupère le délai prévu
                                If (DpkDelaiPrevu.SelectedDate IsNot Nothing) Then
                                    delaiPrevu = Me.DpkDelaiPrevu.SelectedDate

                                    'Récupère la liste des finalisations
                                    If (Me.LbxFinalisations.Items.Count <> 0) Then
                                        For Each item In Me.LbxFinalisations.Items
                                            Dim ft As FinalisationTemplate = item
                                            If (ft.IsChecked) Then finalisations.Add(New Finalisation(ft.Label, ft.Color, ft.Display, ft.Identifier))
                                        Next

                                        'Récupère la date de finalisation
                                        If (Me.DpkFinalisation.SelectedDate IsNot Nothing) Then
                                            Dim heure As Integer = Integer.Parse(TxtRdvFinalisation.Text.Substring(0, 2))
                                            Dim minute As Integer = Integer.Parse(TxtRdvFinalisation.Text.Substring(3, 2))

                                            dateFinalisation = Me.DpkFinalisation.SelectedDate
                                            If (heure <> 0 Or minute <> 0) Then
                                                dateFinalisation = New DateTime(dateFinalisation.Year, dateFinalisation.Month, dateFinalisation.Day, heure, minute, 0)
                                            End If

                                            'Récupère le type de relevés
                                            If (Me.CbxMesure.SelectedItem IsNot Nothing) Then
                                                releve = Me.CbxMesure.SelectedItem

                                                'Vérifie l'unicité du numéro de commande
                                                If Not Me.IsUpdate Then
                                                    Dim connection As New MGConnection(My.Settings.DBSource)
                                                    Dim Objects As New List(Of List(Of Object))
                                                    Dim numResult As Integer
                                                    Try
                                                        connection.Open()

                                                        Objects = connection.ExecuteQuery("SELECT count(NumCmd) FROM Commande WHERE NumCmd=" + numCmd.ToString())
                                                        For Each obj In Objects
                                                            numResult = Integer.Parse(obj(0))
                                                        Next

                                                        connection.Close()
                                                    Catch ex As Exception
                                                        MessageBox.Show(ex.Message)
                                                    Finally
                                                        Try
                                                            connection.Close()
                                                        Catch ex As Exception
                                                        End Try
                                                    End Try
                                                    If (numResult <> 0) Then
                                                        messageError = "Le numéro de commande existe déjà."
                                                    End If
                                                End If

                                            Else
                                                messageError = "Veuillez sélectionner le type de relevé."
                                            End If

                                        Else
                                            messageError = "Veuillez saisir la date d'achèvement."
                                        End If

                                    Else
                                        messageError = "Veuillez sélectionner au moins 1 prestation."
                                    End If

                                Else
                                    messageError = "Veuillez saisir le délai prévu."
                                End If

                            Else
                                messageError = "Veuillez sélectionner au moins 1 nature."
                            End If

                        Else
                            messageError = "Veuillez sélectionner au moins 1 matériau."
                        End If

                    Else
                        messageError = "Veuillez saisir la date de la commande."
                    End If

                Else
                    messageError = "Veuillez sélectionner un client."
                End If

            Else
                messageError = "Numéro de commande non valide."
            End If




            If (messageError = String.Empty) Then
                If Not Me.IsUpdate Then
                    'Construit la commande
                    Dim newCommande As New Commande(numCmd, montant, arrhes, dateCommande, adresse, etat, client, tpsDebit, tpsCmdNumerique, tpsFinition, tpsAutres, delaiPrevu,
                                                            releve, dateReleve, contremarque, materiaux, natures, dateFinalisation, finalisations, remarques, qualites)
                    If (newCommande.Identifier = 0) Then
                        newCommande.Add()
                        If Me.Planning IsNot Nothing Then Me.Planning.Fill()
                        Me.Clear()
                        MessageBox.Show("Votre commande a été enregistrée avec succès !", "Nouvelle commande sauvegardée", MessageBoxButton.OK,
                                        MessageBoxImage.Information)
                    End If
                Else
                    Dim result As MessageBoxResult = MessageBox.Show("Voulez-vous sauvegarder les modifications ?", "Mise à jour", MessageBoxButton.YesNo, MessageBoxImage.Exclamation)

                    If (result = MessageBoxResult.Yes) Then
                        'Construit la commande
                        If contremarque Is Nothing Then contremarque = New Contremarque()


                        Dim newCommande As New Commande(numCmd, montant, arrhes, dateCommande, adresse, etat, client, tpsDebit, tpsCmdNumerique, tpsFinition, tpsAutres, delaiPrevu,
                                                                releve, dateReleve, contremarque, materiaux, natures, dateFinalisation, finalisations, remarques, qualites,
                                                                Me.Commande.Identifier)
                        newCommande.Update(_IsRestrictUpdate)

                        MessageBox.Show("Vos modifications ont été enregistrées avec succès !", "Mise à jour effectuée", MessageBoxButton.OK,
                                        MessageBoxImage.Information)

                        If Me.UC IsNot Nothing Then
                            Dim search As RechercheCommande = Me.UC
                            If search.LbxSearchCmd.Items.Count > 0 Then search.BtnSearch_Click(search.BtnSearch, Nothing)
                        End If
                        If Me.Planning IsNot Nothing Then Me.Planning.Fill()
                    End If
                End If
            Else
                MessageBox.Show(messageError, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End If

        Catch ex As Exception
            MessageBox.Show("Un problème est survenu. Votre commande n'a pas pu être sauvegardée." + vbCrLf + "Veuillez réessayer.", "Erreur", MessageBoxButton.OK,
                            MessageBoxImage.Error)
            Dim sw As New StreamWriter(My.Settings.ConfigFiles + "\log.txt")

            Dim content As String = "BTNCONNEXION" + vbCrLf + ex.StackTrace.ToString() + vbCrLf + vbCrLf + ex.Source.ToString()
            If ex.InnerException IsNot Nothing Then
                content = content + vbCrLf + vbCrLf + ex.InnerException.ToString()
            End If

            content = content + vbCrLf + "/BTNCONNEXION"

            sw.Write(content)

            sw.Close()
        End Try
    End Sub

    ''' <summary>
    ''' Bouton permettant de supprimer une commande
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim result As MessageBoxResult = MessageBox.Show("Attention, vous êtes sur le point de supprimer la commande :" + vbCrLf +
                                                         "N° : " + Me.Commande.NoCommande.ToString() + vbCrLf +
                                                         "Client : " + Me.Commande.Client.Nom + vbCrLf +
                                                         "Date commande : " + Me.Commande.DateCommande + vbCrLf +
                                                         "Délai prévu : " + Me.Commande.DelaiPrevu + vbCrLf +
                                                         "État : " + Me.Commande.Etat.Label + vbCrLf + vbCrLf +
                                                         "Voulez-vous vraiment supprimer définitivement cette commande ?",
                                                         "Suppression de la commande n° " + Me.Commande.NoCommande.ToString(),
                                                         MessageBoxButton.YesNo,
                                                         MessageBoxImage.Warning)

        If result = MessageBoxResult.Yes Then
            Dim parameters As New List(Of MySqlParameter)
            Dim connection As New MGConnection(My.Settings.DBSource)

            Try
                connection.Open()

                Dim parIdCommande As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Commande.Identifier)
                parameters.Add(parIdCommande)

                Dim query As String = "DELETE FROM Commande_Materiau WHERE Identifier_Commande = @Identifier;" +
                                      "DELETE FROM Commande_Finalisation WHERE Identifier_Commande = @Identifier;" +
                                      "DELETE FROM Commande_Nature WHERE Identifier_Commande = @Identifier;" +
                                      "DELETE FROM Remarque WHERE IdentifierCommande=@Identifier;" +
                                      "DELETE FROM Commande_Qualite WHERE Identifier_Commande=@Identifier;" +
                                      "DELETE FROM Commande WHERE Identifier=@Identifier;" +
                                      "DELETE FROM Client WHERE (SELECT count(c.Identifier) FROM Commande as c WHERE IdentifierClient=Client.Identifier) = 0;" +
                                      "DELETE FROM Contremarque WHERE (SELECT count(c.Identifier) FROM Commande as c WHERE IdentifierContremarque=Contremarque.Identifier) = 0"

                connection.ExecuteNonQuery(query, parameters)

                parameters.Clear()

                connection.Close()

                MessageBox.Show("La commande n° " + Me.Commande.NoCommande.ToString() + " a été supprimée avec succès.", "Commande supprimée", MessageBoxButton.OK,
                                MessageBoxImage.Information)

                If Me.UC IsNot Nothing Then
                    Dim search As RechercheCommande = Me.UC
                    search.Reinitialize()
                End If
                If Me.Planning IsNot Nothing Then Me.Planning.Fill()
                Me.Window.Close()


            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error")
            Finally
                Try
                    connection.Close()
                Catch
                End Try
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Bouton permettant de supprimer une remarque
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDeleteRemarqueItem_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If DgRemarques.SelectedItem IsNot Nothing Then
            DgRemarques.Items.Remove(DgRemarques.SelectedItem)
        Else
            MessageBox.Show("Veuillez sélectionner une ligne", "Sélection vide", MessageBoxButton.OK, MessageBoxImage.Exclamation)
        End If
    End Sub

    ''' <summary>
    ''' Bouton permettant de supprimer un problème de qualité
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDeleteQualiteItem_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If DgQualites.SelectedItem IsNot Nothing Then
            DgQualites.Items.Remove(DgQualites.SelectedItem)
        Else
            MessageBox.Show("Veuillez sélectionner une ligne", "Sélection vide", MessageBoxButton.OK, MessageBoxImage.Exclamation)
        End If
    End Sub

#End Region

#Region "EventControlEnter"

    ''' <summary>
    ''' Évènement se produisant lorsque une touche est enfoncée dans le TextBox permettant d'ajouter une remarque
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TxtNewRemarque_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)

        If (e.Key = Key.Enter) Then
            BtnAddRemarque_Click(Nothing, Nothing)
        End If

    End Sub

    ''' <summary>
    ''' Évènement se produisant lors du clique sur une listbox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ListBox_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        If _IsRestrictUpdate Then
            e.Handled = True
        End If
    End Sub

    ''' <summary>
    ''' Évènement se produisant lorsqu'une touche est entrée sur l'AutoCompleteBox matériaux
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AutoCompLMateriaux_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If e.Key = Key.Tab Then
            Me.AutoCompLMateriaux.IsDropDownOpen = False
            e.Handled = True
        End If
    End Sub

    ''' <summary>
    ''' Évènement se produisant lorsque une touche est enfoncée dans le TextBox permettant d'ajouter un commentaire au problème de qualité
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TxtCommentaire_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)

        If (e.Key = Key.Enter) Then
            BtnAddQualite_Click(Nothing, Nothing)
        End If

    End Sub

    ''' <summary>
    ''' Saisie du code postal avec test minimal d'existence
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TxtCodePostal_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        Dim lenghtTb As Integer = TxtCodePostal.Text.Length
        Dim isNum As Boolean = False
        Dim isTwo As Boolean = False
        Dim change As Boolean = True
        Dim keyValue As String = [String].Empty
        Dim actualText As String = TxtCodePostal.Text

        'Vérifie si la longueur du code postal a atteint son maximum
        If lenghtTb < 5 Then
            'Test si la touche enfoncée est une touche du pavé numérique
            For Each k In _ListNumPadKeys
                If e.Key = k Then
                    isNum = True
                End If
            Next

            'Sinon test si la touche enfoncée est a une valeur numérique
            If Not isNum Then
                Dim isToogledLeftShift As Boolean = e.KeyboardDevice.IsKeyToggled(Key.Left)
                Dim isToogledRightShift As Boolean = e.KeyboardDevice.IsKeyToggled(Key.RightShift)
                Dim isToogledCapital As Boolean = e.KeyboardDevice.IsKeyToggled(Key.Capital)

                For Each k In _ListNumKeys
                    'Test des touches de majuscule
                    If e.Key = k AndAlso ((isToogledCapital And Not isToogledLeftShift And Not isToogledRightShift) Or ((isToogledLeftShift Or isToogledRightShift) And Not isToogledCapital)) Then
                        isNum = True
                    End If
                Next
            End If

            'Si la touche a une valeur numérique, traitement des tests permettant de vérifier la validité du code postal
            '                  Tests effectués :
            '                                    - Si le premier chiffre est un 0, alors le deuxième ne peut pas être aussi un 0
            '                                    - Si le premier chiffre est un 9, alors le deuxième ne peut pas être un 6 et ne peut pas être strictement supérieur à 7
            '                                    - Si les deux premiers chiffres sont 97, alors le troisième doit être compris entre 1 et 6 inclus

            If isNum Then
                keyValue = e.Key.ToString().Substring(e.Key.ToString().Length - 1)
                Dim intKeyValue As Integer = Convert.ToInt32(keyValue)

                If lenghtTb > 0 Then
                    Dim intActualValue As Integer = Convert.ToInt32(actualText)

                    If lenghtTb = 1 Then
                        If intActualValue = 0 Then
                            isTwo = True
                        ElseIf intActualValue = 9 Then
                            If (intKeyValue = 6) Or intKeyValue > 7 Then
                                change = False
                            End If
                        End If
                    ElseIf lenghtTb = 2 Then
                        If intActualValue = 97 Then
                            If intKeyValue = 0 Or intKeyValue > 6 Then
                                change = False
                            End If
                        End If
                    End If

                    If isTwo Then
                        If intKeyValue = 0 Then
                            change = False
                        End If
                    End If
                End If
            End If

            If change Then
                Dim cursorPosition As Integer = TxtCodePostal.CaretIndex
                TxtCodePostal.Text = TxtCodePostal.Text.Insert(cursorPosition, keyValue)
                TxtCodePostal.CaretIndex = cursorPosition + 1
            End If
        ElseIf e.Key = Key.Tab Then
            TxtVille.Focus()
        End If

        'Indique que l'évènement est géré par le code ci-dessus
        e.Handled = True
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
            connection.Open()

            Dim text As String = Me.AutoCompNClient.Text.Replace("'", "\'")
            text = text.Replace("""", "\""")

            objects = connection.ExecuteQuery("SELECT Identifier, Nom FROM Client WHERE Nom Like '%" + text.ToUpper() + "%' Order By Nom")

            Dim clients As New List(Of Client)

            For Each obj In objects
                clients.Add(New Client(obj(1).ToString(), Long.Parse(obj(0))))
            Next

            Me.AutoCompNClient.ItemsSource = clients
            Me.AutoCompNClient.PopulateComplete()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
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
            connection.Open()

            Dim text As String = Me.AutoCompNClient.Text.Replace("'", "\'")
            text = text.Replace("""", "\""")

            objects = connection.ExecuteQuery("SELECT Identifier, Nom FROM Contremarque WHERE Nom Like '%" + text.ToUpper() + "%' Order By Nom")

            Dim contremarques = New List(Of Contremarque)

            For Each obj In objects
                contremarques.Add(New Contremarque(obj(1).ToString(), Long.Parse(obj(0))))
            Next

            Me.AutoCompNContremarque.ItemsSource = contremarques
            Me.AutoCompNContremarque.PopulateComplete()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                connection.Close()
            Catch ex As Exception
            End Try
        End Try
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
    ''' Écriture dans l'AutoCompleteBox matériau
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AutoCompLMateriaux_Populating(ByVal sender As System.Object, ByVal e As System.Windows.Controls.PopulatingEventArgs)
        Dim list As New List(Of String)

        For Each item In Me.LbxMateriaux.Items
            Dim mat As MateriauTemplate = item
            list.Add(mat.Label)
        Next

        Me.AutoCompLMateriaux.ItemsSource = list
        Me.AutoCompLMateriaux.PopulateComplete()
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Convertit un nombre de minutes en Heure et Minute.
    ''' </summary>
    ''' <param name="min">Nombre de minutes à convertir</param>
    ''' <returns>Retourne une chaîne de caracère contenant les heures et les minutes séparées par le caractères ';'</returns>
    ''' <remarks></remarks>
    Private Function convertMinuteToHourMinute(ByVal min As Integer) As String
        Dim result As String = String.Empty

        Dim hour As Integer
        Dim minute As Integer

        minute = min Mod 60
        hour = (min - minute) / 60

        result = hour.ToString() + ";" + minute.ToString()
        Return result
    End Function

    ''' <summary>
    ''' Réinitialise les paramètres du composant
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Clear()
        Me.DpkDateCommande.SelectedDate = Date.Now
        Me.TxtNumCmd.Text = String.Empty
        Me.AutoCompNClient.SelectedItem = Nothing
        Me.AutoCompNClient.Text = ""
        Me.AutoCompNContremarque.SelectedItem = Nothing
        Me.AutoCompNContremarque.Text = ""
        Me.LbxMateriaux.Items.Clear()
        For Each m In ListMateriaux

            Dim mt As New MateriauTemplate(m)
            Me.LbxMateriaux.Items.Add(mt)
        Next
        Me.LbxNatures.Items.Clear()
        For Each n In Me.ListNatures

            Dim nt As New NatureTemplate(n)
            Me.LbxNatures.Items.Add(nt)
        Next
        Me.DpkDelaiPrevu.Text = Nothing
        Me.LbxFinalisations.Items.Clear()
        For Each f In Me.ListFinalisations

            Dim ft As New FinalisationTemplate(f)
            Me.LbxFinalisations.Items.Add(ft)
        Next
        Me.DpkFinalisation.SelectedDate = Nothing
        Me.TxtRdvFinalisation.Text = "00h00"
        Me.CbxMesure.SelectedItem = Nothing
        Me.DpkMesure.SelectedDate = Nothing
        Me.TxtRdvMesure.Text = "00h00"
        Me.TxtAdresse.Text = String.Empty
        Me.TxtCodePostal.Text = String.Empty
        Me.TxtVille.Text = String.Empty
        Me.TxtMontant.Text = "0,00"
        Me.TxtArrhes.Text = "0,00"
        Me.TxtTpsDebitH.Text = 0
        Me.TxtTpsDebitM.Text = 0
        Me.TxtTpsCmdNumH.Text = 0
        Me.TxtTpsCmdNumM.Text = 0
        Me.TxtTpsFinitionH.Text = 0
        Me.TxtTpsFinitionM.Text = 0
        Me.TxtTpsAutresH.Text = 0
        Me.TxtTpsAutresM.Text = 0
        Me.CbxEtat.SelectedIndex = 0
        Me.DgRemarques.Items.Clear()
    End Sub

#End Region

End Class
