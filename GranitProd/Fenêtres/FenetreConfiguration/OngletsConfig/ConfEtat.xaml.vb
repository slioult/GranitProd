Imports MGranitDALcsharp
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class ConfEtat

#Region "Fields"

    Private _NouvelleCommande As NouvelleCommande

#End Region

#Region "Properties"

    Public Property NouvelleCommande As NouvelleCommande
        Get
            Return Me._NouvelleCommande
        End Get
        Set(ByVal value As NouvelleCommande)
            Me._NouvelleCommande = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Me.LstEtat.Items.Add(New Etat("Nouveau..."))

        Dim etats As New List(Of Etat)
        etats = Etat.GetEtats()

        For Each e In etats
            Me.LstEtat.Items.Add(e)
        Next
        Me.LstEtat.SelectedIndex = 0
    End Sub

#End Region

#Region "Button"

    ''' <summary>
    ''' Bouton de suppresion d'un état
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim etat As Etat = Me.LstEtat.SelectedItem
        If etat.Label <> "Terminée" And etat.Label <> "Rendue" Then
            If Me.LstEtat.SelectedIndex > 0 Then
                If Not etat.IsUsed Then
                    Dim question As MessageBoxResult = MessageBox.Show("Voulez vous vraiment supprimer l'état selectionné ?", "Supression d'un état", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    If question = MessageBoxResult.Yes Then
                        Dim indexDel As Integer = LstEtat.SelectedIndex


                        etat.Delete()

                        Me.LstEtat.Items.Remove(Me.LstEtat.SelectedItem)

                        Dim etats As New List(Of Etat)
                        For Each eta In LstEtat.Items
                            etats.Add(eta)
                        Next
                        Me.NouvelleCommande.CbxEtat.ItemsSource = etats

                        For i = indexDel To LstEtat.Items.Count
                            Dim etatTemp As Etat = LstEtat.Items.GetItemAt(i - 1)
                            etatTemp.Position = i - 1
                            etatTemp.Update()
                        Next

                        Me.LstEtat.SelectedIndex = 0
                        MessageBox.Show("L'état a été supprimé.", "État supprimé", MessageBoxButton.OK, MessageBoxImage.Information)
                    End If
                Else
                    MessageBox.Show("L'état est utilisé dans une commande et ne peut pas être supprimé.", "Suppression impossible", MessageBoxButton.OK, MessageBoxImage.Error)
                End If
            Else
                MessageBox.Show("Veuillez sélectionner un état à supprimer.", "Suppression d'un état", MessageBoxButton.OK, MessageBoxImage.Exclamation)
            End If
        Else
            MessageBox.Show("Désolé, cet état ne peut être supprimé." + vbCrLf + "Pour plus d'informations, veuillez contacter les créateur du logiciel.",
                                "Suppression impossible", MessageBoxButton.OK, MessageBoxImage.Information)
        End If

    End Sub

    ''' <summary>
    ''' Bouton de sauvegarde d'un état
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Me.LstEtat.SelectedIndex = 0 And TxtNomEtat.Text <> "" Then
            Dim etat As New Etat(TxtNomEtat.Text, Me.LstEtat.Items.Count)
            Dim isExists As Boolean = False
            For Each item In Me.LstEtat.Items
                Dim tempEtat As Etat = item
                If tempEtat.Label.ToUpper() = etat.Label.ToUpper() Then
                    isExists = True
                    Exit For
                End If
            Next

            If Not isExists Then
                etat.Identifier = etat.Insert()
                Me.LstEtat.Items.Add(etat)
                Me.LstEtat.SelectedItem = etat
                Dim etats As New List(Of Etat)
                For Each eta In LstEtat.Items
                    etats.Add(eta)
                Next
                Me.NouvelleCommande.CbxEtat.ItemsSource = etats
                MessageBox.Show("L'état a été ajouté avec succès", "Nouvel état ajouté", MessageBoxButton.OK, MessageBoxImage.Information)
            Else
                MessageBox.Show("L'état existe déjà", "État existant", MessageBoxButton.OK, MessageBoxImage.Information)
            End If
        ElseIf Me.LstEtat.SelectedIndex > 0 And TxtNomEtat.Text <> "" Then
            Dim etat As Etat = Me.LstEtat.SelectedItem
            If etat.Label <> "Terminée" And etat.Label <> "Rendue" Then
                Dim index = Me.LstEtat.SelectedIndex

                Dim isExists As Boolean = False
                For Each item In Me.LstEtat.Items
                    Dim tempEtat As Etat = item
                    If tempEtat.Label.ToUpper() = TxtNomEtat.Text.ToUpper() Then
                        isExists = True
                        Exit For
                    End If
                Next

                If Not isExists Then
                    Dim result As MessageBoxResult = MessageBox.Show("Voulez-vous modifié l'état « " + etat.Label + " » ?", "Modifier un état",
                                                                     MessageBoxButton.YesNo, MessageBoxImage.Question)
                    If result = MessageBoxResult.Yes Then
                        etat.Label = TxtNomEtat.Text
                        etat.Update()

                        Me.LstEtat.Items.RemoveAt(index)
                        Me.LstEtat.Items.Insert(index, etat)

                        Dim selected = Me.NouvelleCommande.CbxEtat.SelectedIndex
                        Dim etats As New List(Of Etat)
                        For Each eta In LstEtat.Items
                            etats.Add(eta)
                        Next
                        Me.NouvelleCommande.CbxEtat.ItemsSource = etats
                        Me.NouvelleCommande.CbxEtat.SelectedIndex = selected

                        Me.LstEtat.SelectedIndex = index
                        MessageBox.Show("L'état a été modifié avec succès", "État modifié", MessageBoxButton.OK, MessageBoxImage.Information)
                    End If
                Else
                    MessageBox.Show("L'état existe déjà", "État existant", MessageBoxButton.OK, MessageBoxImage.Information)
                End If
            Else
                MessageBox.Show("Désolé, cet état ne peut être modifié." + vbCrLf + "Pour plus d'informations, veuillez contacter les créateur du logiciel.",
                                "Modification impossible", MessageBoxButton.OK, MessageBoxImage.Information)
            End If

        End If
    End Sub

    ''' <summary>
    ''' Faire monter d'une position l'état selectioné
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnUP_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

        Dim index As Integer = Me.LstEtat.SelectedIndex

        If index <> 0 And index - 1 <> 0 Then
            Dim upEtat As Etat = Me.LstEtat.Items.GetItemAt(index)
            Dim donwEtat As Etat = Me.LstEtat.Items.GetItemAt(index - 1)

            upEtat.Position -= 1
            donwEtat.Position += 1

            upEtat.Update()
            donwEtat.Update()

            Me.LstEtat.Items.RemoveAt(index)
            Me.LstEtat.Items.Insert(index - 1, upEtat)

            Me.LstEtat.SelectedIndex = index - 1

            Dim etats As New List(Of Etat)
            For Each eta In LstEtat.Items
                etats.Add(eta)
            Next
            Me.NouvelleCommande.CbxEtat.ItemsSource = etats
        End If

    End Sub

    ''' <summary>
    ''' Faire descendre d'une position l'état selectioné
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDown_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

        Dim index As Integer = Me.LstEtat.SelectedIndex

        If index <> 0 And index <> Me.LstEtat.Items.Count - 1 Then
            Dim upEtat As Etat = Me.LstEtat.Items.GetItemAt(index + 1)
            Dim donwEtat As Etat = Me.LstEtat.Items.GetItemAt(index)

            upEtat.Position -= 1
            donwEtat.Position += 1

            upEtat.Update()
            donwEtat.Update()

            Me.LstEtat.Items.RemoveAt(index)
            Me.LstEtat.Items.Insert(index + 1, donwEtat)

            Me.LstEtat.SelectedIndex = index + 1

            Dim etats As New List(Of Etat)
            For Each eta In LstEtat.Items
                etats.Add(eta)
            Next
            Me.NouvelleCommande.CbxEtat.ItemsSource = etats
        End If
    End Sub

#End Region

End Class
