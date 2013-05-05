Imports MGranitDALcsharp
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class ConfEpaisseur

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
        Dim epaisseurs As New List(Of Epaisseur)
        epaisseurs = Epaisseur.GetEpaisseurs()

        For Each e In epaisseurs
            Me.CbxConfEpaisseur.Items.Add(e)
        Next

        Me.CbxConfEpaisseur.SelectedIndex = 0
    End Sub

#End Region

#Region "Button"

    ''' <summary>
    ''' Bouton de suppresion d'une épaisseur
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Me.CbxConfEpaisseur.SelectedIndex >= 0 Then
            Dim epaisseur As Epaisseur = Me.CbxConfEpaisseur.SelectedItem
            Dim question As MessageBoxResult = MessageBox.Show("Voulez vous vraiment supprimer l'épaisseur selectionnée ?", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Warning)
            If question = MessageBoxResult.Yes Then
                Me.CbxConfEpaisseur.Items.Remove(Me.CbxConfEpaisseur.SelectedItem)
                epaisseur.Delete()
                Dim listMTT As New List(Of MateriauTemplate)

                For Each item In Me.NouvelleCommande.LbxMateriaux.Items
                    Dim matT As MateriauTemplate = item
                    listMTT.Add(matT)
                Next

                Me.NouvelleCommande.LbxMateriaux.Items.Clear()

                For Each m In listMTT
                    Me.NouvelleCommande.LbxMateriaux.Items.Add(m)
                Next
                Me.CbxConfEpaisseur.SelectedIndex = 0
            End If
        Else
            MessageBox.Show("Veuillez sélectionner une épaisseur a supprimer.", "Épaisseur non sélectionnée", MessageBoxButton.OK, MessageBoxImage.Warning)
        End If
    End Sub

    ''' <summary>
    ''' Bouton de sauvegarde d'une épaisseur
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If TxtNomEpaisseur.Text <> "" Then
            Dim epaisseur As New Epaisseur(Integer.Parse(TxtNomEpaisseur.Text))

            Dim isExists As Boolean = False
            For Each item In Me.CbxConfEpaisseur.Items
                Dim tempEpaisseur As Epaisseur = item
                If tempEpaisseur.Value = epaisseur.Value Then
                    isExists = True
                    Exit For
                End If
            Next

            If Not isExists Then
                epaisseur.Identifier = epaisseur.Insert()
                Me.CbxConfEpaisseur.Items.Add(epaisseur)
                Me.CbxConfEpaisseur.SelectedItem = epaisseur
                TxtNomEpaisseur.Clear()
                Dim listMTT As New List(Of MateriauTemplate)

                For Each item In Me.NouvelleCommande.LbxMateriaux.Items
                    Dim matT As MateriauTemplate = item
                    listMTT.Add(matT)
                Next

                Me.NouvelleCommande.LbxMateriaux.Items.Clear()

                For Each m In listMTT
                    Me.NouvelleCommande.LbxMateriaux.Items.Add(m)
                Next
                MessageBox.Show("L'épaisseur a été ajoutée avec succès.", "Nouvelle épaisseur ajoutée", MessageBoxButton.OK, MessageBoxImage.Information)
            Else
                MessageBox.Show("L'épaisseur existe déjà.", "Épaisseur existante", MessageBoxButton.OK, MessageBoxImage.Information)
            End If
        Else
            MessageBox.Show("Veuillez renseigner tous les champs", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning)
        End If
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Vérifie à la saisie si le caractère entré est un nombre
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub VerifSaisie(ByVal e)

        If Char.IsNumber(e.keychar) Then
            e.handled = False
        Else
            e.handled = True
        End If

    End Sub

#End Region

End Class
