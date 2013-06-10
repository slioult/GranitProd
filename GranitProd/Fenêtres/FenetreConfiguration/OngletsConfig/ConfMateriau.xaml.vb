Imports MGranitDALcsharp
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class ConfMateriau

    Dim connection As New MGConnection(My.Settings.DBSource)
    Dim listMateriau As New List(Of String)

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

        'Remplir la CmbConfMateriau
        Me.CbxConfMateriau.Items.Add(New Materiau("Nouveau"))

        Dim materiaux As New List(Of Materiau)
        materiaux = Materiau.GetMateriaux()

        For Each e In materiaux
            Me.CbxConfMateriau.Items.Add(e)
        Next

        Me.CbxConfMateriau.SelectedIndex = 0

    End Sub
#End Region

#Region "Button"

    ''' <summary>
    ''' Bouton de suppresion d'un Materiau
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Me.CbxConfMateriau.SelectedIndex > 0 Then
            Dim materiau As Materiau = Me.CbxConfMateriau.SelectedItem
            If Not materiau.IsUsed() Then
                Dim question As MessageBoxResult = MessageBox.Show("Voulez-vous vraiment supprimer le matériau selectionné ?", "Suppression d'un matériau", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                If question = MessageBoxResult.Yes Then
                    Me.CbxConfMateriau.Items.Remove(Me.CbxConfMateriau.SelectedItem)
                    materiau.Delete()
                    Dim mt As New MateriauTemplate(materiau, False)
                    Me.NouvelleCommande.LbxMateriaux.Items.Remove(mt)
                    Me.CbxConfMateriau.SelectedIndex = 0

                    MessageBox.Show("Le matériau a été supprimé", "Matériau supprimé", MessageBoxButton.OK, MessageBoxImage.Information)
                End If
            Else
                MessageBox.Show("Le materiau est utilisé dans une commande et ne peut pas être supprimé.", "Suppression impossible", MessageBoxButton.OK, MessageBoxImage.Exclamation)
            End If
        Else
            MessageBox.Show("Veuillez sélectionner un matériau à supprimer.", "Suppression d'un matériau", MessageBoxButton.OK, MessageBoxImage.Exclamation)
        End If
    End Sub

    ''' <summary>
    ''' Bouton de sauvegarde d'un Materiau
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Me.CbxConfMateriau.SelectedIndex = 0 And TxtNomMateriau.Text <> "" Then
            Dim materiau As New Materiau(TxtNomMateriau.Text)

            Dim isExists As Boolean = False
            For Each item In Me.CbxConfMateriau.Items
                Dim tempMateriau As Materiau = item
                If tempMateriau.Label.ToUpper() = materiau.Label.ToUpper() Then
                    isExists = True
                    Exit For
                End If
            Next

            If Not isExists Then
                materiau.Identifier = materiau.Insert()
                Me.CbxConfMateriau.Items.Add(materiau)
                Me.CbxConfMateriau.SelectedItem = materiau
                Dim mt As New MateriauTemplate(materiau, False)
                Me.NouvelleCommande.LbxMateriaux.Items.Add(mt)
                MessageBox.Show("Le matériau a été ajouté.", "Nouveau matériau ajouté", MessageBoxButton.OK, MessageBoxImage.Information)
            Else
                MessageBox.Show("Le matériau existe déjà.", "Matériau existant", MessageBoxButton.OK, MessageBoxImage.Exclamation)
            End If
        ElseIf Me.CbxConfMateriau.SelectedIndex > 0 And TxtNomMateriau.Text <> "" Then
            Dim index = Me.CbxConfMateriau.SelectedIndex
            Dim materiau As Materiau = Me.CbxConfMateriau.SelectedItem

            Dim isExists As Boolean = False
            For Each item In Me.CbxConfMateriau.Items
                Dim tempMateriau As Materiau = item
                If tempMateriau.Label.ToUpper() = TxtNomMateriau.Text.ToUpper() Then
                    isExists = True
                    Exit For
                End If
            Next

            If Not isExists Then
                Dim result As MessageBoxResult = MessageBox.Show("Voulez-vous modifier le matériau « " + materiau.Label + " » ?", "Modification d'un matériau",
                                                                 MessageBoxButton.OK, MessageBoxImage.Question)

                If result = MessageBoxResult.Yes Then
                    Dim newMateriau As New Materiau(materiau.Label, materiau.Identifier)
                    materiau.Label = TxtNomMateriau.Text
                    materiau.Update()

                    Me.CbxConfMateriau.Items.RemoveAt(index)
                    Me.CbxConfMateriau.Items.Insert(index, materiau)

                    Dim listMTT As New List(Of MateriauTemplate)

                    For Each item In Me.NouvelleCommande.LbxMateriaux.Items
                        Dim matT As MateriauTemplate = item
                        listMTT.Add(matT)
                    Next

                    Me.NouvelleCommande.LbxMateriaux.Items.Clear()

                    For Each m In listMTT
                        If m.Identifier = materiau.Identifier Then m.Label = materiau.Label
                        Me.NouvelleCommande.LbxMateriaux.Items.Add(m)
                    Next

                    Me.CbxConfMateriau.SelectedIndex = index
                    MessageBox.Show("Le matériau a été modifié.", "Matériau modifié", MessageBoxButton.OK, MessageBoxImage.Information)
                End If
            Else
                MessageBox.Show("Le matériau existe déjà.", "Matériau existant", MessageBoxButton.OK, MessageBoxImage.Exclamation)
            End If

            End If
    End Sub

#End Region

End Class
