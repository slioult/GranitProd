Imports MGranitDALcsharp
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class ConfQualite

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
        Me.CbxConfQualite.Items.Add(New Qualite("Nouveau"))

        Dim qualites As New List(Of Qualite)
        qualites = Qualite.GetQualites()

        For Each e In qualites
            Me.CbxConfQualite.Items.Add(e)
        Next

        Me.CbxConfQualite.SelectedIndex = 0

    End Sub
#End Region

#Region "Button"

    ''' <summary>
    ''' Bouton de suppresion d'une qualité
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Me.CbxConfQualite.SelectedIndex > 0 Then
            Dim qualite As Qualite = Me.CbxConfQualite.SelectedItem
            If Not qualite.IsUsed Then
                Dim question As MessageBoxResult = MessageBox.Show("Voulez vous vraiment supprimer la qualité selectionnée ?", "Suppression d'une qualité", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                If question = MessageBoxResult.Yes Then
                    Me.CbxConfQualite.Items.Remove(Me.CbxConfQualite.SelectedItem)
                    qualite.Delete()

                    Dim qualites As New List(Of Qualite)
                    For Each q In CbxConfQualite.Items
                        qualites.Add(q)
                    Next
                    Me.NouvelleCommande.CbxQualite.ItemsSource = qualites
                    Me.CbxConfQualite.SelectedIndex = 0

                    MessageBox.Show("La qualité a été supprimée.", "Qualité supprimée", MessageBoxButton.OK, MessageBoxImage.Information)
                End If
            Else
                MessageBox.Show("La qualité est utilisée dans une commande" + vbCrLf + "Vous ne pouvez donc pas la supprimer.", "Suppression impossible", MessageBoxButton.OK, MessageBoxImage.Exclamation)
            End If
        Else
            MessageBox.Show("Veuillez sélectionner une qualité à supprimer.", "Qualité non sélectionnée", MessageBoxButton.OK, MessageBoxImage.Warning)
        End If
    End Sub

    ''' <summary>
    ''' Bouton de sauvegarde d'une qualité
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Me.CbxConfQualite.SelectedIndex = 0 And TxtNomQualite.Text <> "" Then
            Dim qualite As New Qualite(TxtNomQualite.Text)

            Dim isExists As Boolean = False
            For Each item In Me.CbxConfQualite.Items
                Dim tempqualite As Qualite = item
                If tempqualite.Type.ToUpper() = qualite.Type.ToUpper() Then
                    isExists = True
                    Exit For
                End If
            Next

            If Not isExists Then
                qualite.Identifier = qualite.Insert()
                Me.CbxConfQualite.Items.Add(qualite)
                Me.CbxConfQualite.SelectedItem = qualite

                Dim qualites As New List(Of Qualite)
                For Each q In CbxConfQualite.Items
                    qualites.Add(q)
                Next
                Me.NouvelleCommande.CbxQualite.ItemsSource = qualites

                MessageBox.Show("La qualité a été ajoutée avec succès.", "Nouvelle qualité ajoutée", MessageBoxButton.OK, MessageBoxImage.Information)
            Else
                MessageBox.Show("La qualité existe déjà.", "Qualité existante", MessageBoxButton.OK, MessageBoxImage.Information)
            End If
        ElseIf Me.CbxConfQualite.SelectedIndex > 0 And TxtNomQualite.Text <> "" Then
            Dim index = Me.CbxConfQualite.SelectedIndex
            Dim qualite As Qualite = Me.CbxConfQualite.SelectedItem

            Dim isExists As Boolean = False
            For Each item In Me.CbxConfQualite.Items
                Dim tempQualite As Qualite = item
                If tempQualite.Type.ToUpper() = TxtNomQualite.Text.ToUpper() Then
                    isExists = True
                    Exit For
                End If
            Next

            If Not isExists Then
                Dim result As MessageBoxResult = MessageBox.Show("Voulez-vous vraiment modifier la qualité « " + qualite.Type + " » ?", "Modification d'une qualité",
                                                                 MessageBoxButton.OK, MessageBoxImage.Question)

                If result = MessageBoxResult.Yes Then
                    qualite.Type = TxtNomQualite.Text
                    qualite.Update()

                    Me.CbxConfQualite.Items.RemoveAt(index)
                    Me.CbxConfQualite.Items.Insert(index, qualite)

                    Dim selected As Integer = Me.NouvelleCommande.CbxQualite.SelectedIndex
                    Dim qualites As New List(Of Qualite)
                    For Each q In CbxConfQualite.Items
                        qualites.Add(q)
                    Next
                    Me.NouvelleCommande.CbxQualite.ItemsSource = qualites
                    Me.NouvelleCommande.CbxQualite.SelectedIndex = selected

                    Me.CbxConfQualite.SelectedIndex = index
                    MessageBox.Show("La qualité a été modifiée avec succès.", "Qualité modifiée", MessageBoxButton.OK, MessageBoxImage.Information)
                End If
            Else
                MessageBox.Show("La qualité existe déjà.", "Qualité existante", MessageBoxButton.OK, MessageBoxImage.Information)
            End If

            End If
    End Sub

#End Region

End Class
