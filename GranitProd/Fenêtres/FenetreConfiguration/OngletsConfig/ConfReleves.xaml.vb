Imports MGranitDALcsharp
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class ConfReleves

#Region "Fields"

    Private _NouvelleCommande As NouvelleCommande
    Private _Planning As PlanningControl

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

    Public Property Planning As PlanningControl
        Get
            Return Me._Planning
        End Get
        Set(ByVal value As PlanningControl)
            Me._Planning = value
        End Set
    End Property

#End Region

#Region "Constructor"
    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        'Remplir la CmbConfMesure
        Me.CbxConfReleves.Items.Add(New Mesure("Nouveau"))

        Dim releves As New List(Of Mesure)
        releves = Mesure.GetMesures()

        For Each e In releves
            Me.CbxConfReleves.Items.Add(e)
        Next

        Me.CbxConfReleves.SelectedIndex = 0

    End Sub
#End Region

#Region "Button"

    ''' <summary>
    ''' Bouton de suppresion d'une Mesure
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Me.CbxConfReleves.SelectedIndex > 0 Then
            Dim mesure As Mesure = Me.CbxConfReleves.SelectedItem
            If Not mesure.IsUsed Then
                Dim question As MessageBoxResult = MessageBox.Show("Voulez vous vraiment supprimer le type de relevé selectionné ?", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                If question = MessageBoxResult.Yes Then
                    Me.CbxConfReleves.Items.Remove(Me.CbxConfReleves.SelectedItem)
                    mesure.Delete()
                    Dim mesures As New List(Of Mesure)
                    For Each mes In CbxConfReleves.Items
                        mesures.Add(mes)
                    Next
                    Me.NouvelleCommande.CbxMesure.ItemsSource = mesures

                    If Me.Planning IsNot Nothing Then Me.Planning.Fill()
                    Me.CbxConfReleves.SelectedIndex = 0
                End If
            Else
                MessageBox.Show("Le type de relevé est utilisé dans une commande." + vbCrLf + "Vous ne pouvez donc pas le supprimer", "Suppression impossible", MessageBoxButton.OK, MessageBoxImage.Exclamation)
            End If
        Else
            MessageBox.Show("Veuillez sélectionner un type de relevé a supprimer", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning)
        End If
    End Sub

    ''' <summary>
    ''' Bouton de sauvegarde d'un Mesure
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim finalisation As New Finalisation

        If Me.CbxConfReleves.SelectedIndex = 0 And TxtNomReleves.Text <> "" Then

            Dim mesure As New Mesure(TxtNomReleves.Text, CPReleves.SelectedColor.ToString())
            Dim isExistsLabel As Boolean = False
            Dim isExistsColor As Boolean = False

            For Each item In Me.CbxConfReleves.Items
                Dim tempMesure As Mesure = item
                If Not item.Equals(Me.CbxConfReleves.SelectedItem) Then
                    If tempMesure.Label.ToUpper() = mesure.Label.ToUpper() Then
                        isExistsLabel = True
                    End If
                    If tempMesure.Color = mesure.Color Then
                        isExistsColor = True
                    End If
                End If
            Next

            For Each c In finalisation.GetColorsFinalisation
                If c = mesure.Color Then
                    isExistsColor = True
                End If
            Next

            If Not isExistsLabel And Not isExistsColor Then
                mesure.Identifier = mesure.Insert()
                Me.CbxConfReleves.Items.Add(mesure)
                Me.CbxConfReleves.SelectedItem = mesure
                Dim mesures As New List(Of Mesure)
                For Each mes In CbxConfReleves.Items
                    mesures.Add(mes)
                Next
                Me.NouvelleCommande.CbxMesure.ItemsSource = mesures
                If Me.Planning IsNot Nothing Then Me.Planning.Fill()
                MessageBox.Show("Le type de relevé a été ajouté avec succès.", "Nouveau type de relevé ajouté", MessageBoxButton.OK, MessageBoxImage.Information)
            Else
                If isExistsLabel Then
                    MessageBox.Show("Le type de relevé existe déjà.", "Type de relevé existant", MessageBoxButton.OK, MessageBoxImage.Information)
                Else
                    MessageBox.Show("La couleur " + finalisation.Color + " est déjà utilisée." + vbCrLf + "Veuillez en choisir une autre.", "Couleur déjà utilisée", MessageBoxButton.OK,
                                    MessageBoxImage.Information)
                End If
            End If
        ElseIf Me.CbxConfReleves.SelectedIndex > 0 And TxtNomReleves.Text <> "" Then

            Dim index = Me.CbxConfReleves.SelectedIndex
            Dim mesure As Mesure = Me.CbxConfReleves.SelectedItem
            Dim isExistsLabel As Boolean = False
            Dim isExistsColor As Boolean = False

            For Each item In Me.CbxConfReleves.Items
                Dim tempMesure As Mesure = item
                If Not item.Equals(Me.CbxConfReleves.SelectedItem) Then
                    If tempMesure.Label.ToUpper() = TxtNomReleves.Text.ToUpper() Then
                        isExistsLabel = True
                    End If
                    If tempMesure.Color = CPReleves.SelectedColor.ToString() Then
                        isExistsColor = True
                    End If
                End If
            Next

            For Each c In finalisation.GetColorsFinalisation
                If c = mesure.Color Then
                    isExistsColor = True
                End If
            Next

            If Not isExistsLabel And Not isExistsColor Then
                mesure.Label = TxtNomReleves.Text
                mesure.Color = CPReleves.SelectedColor.ToString()
                mesure.Update()

                Me.CbxConfReleves.Items.RemoveAt(index)
                Me.CbxConfReleves.Items.Insert(index, mesure)

                Dim selected As Integer = Me.NouvelleCommande.CbxMesure.SelectedIndex
                Dim mesures As New List(Of Mesure)
                For Each mes In CbxConfReleves.Items
                    mesures.Add(mes)
                Next
                Me.NouvelleCommande.CbxMesure.ItemsSource = mesures
                Me.NouvelleCommande.CbxMesure.SelectedIndex = selected
                If Me.Planning IsNot Nothing Then Me.Planning.Fill()

                Me.CbxConfReleves.SelectedIndex = index
                MessageBox.Show("Le type de relevé a été modifié avec succès.", "Type de relevé modifié", MessageBoxButton.OK, MessageBoxImage.Information)
            Else
                If isExistsLabel Then
                    MessageBox.Show("Le type de relevé existe déjà.", "Type de relevé existant", MessageBoxButton.OK, MessageBoxImage.Information)
                Else
                    MessageBox.Show("La couleur " + finalisation.Color + " est déjà utilisée." + vbCrLf + "Veuillez en choisir une autre.", "Couleur déjà utilisée", MessageBoxButton.OK,
                                    MessageBoxImage.Information)
                End If
            End If

        End If
    End Sub

#End Region

#Region "SelectionChanged"

    ''' <summary>
    ''' Évènement se produisant lorsque l'item sélectionné dans CbxConfReleves change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CbxConfReleves_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If CbxConfReleves.SelectedItem IsNot Nothing Then
            Dim m As Mesure = CbxConfReleves.SelectedItem
            If CbxConfReleves.SelectedIndex <> 0 Then
                CPReleves.SelectedColor = ColorConverter.ConvertFromString(m.Color)
            Else
                CPReleves.SelectedColor = ColorConverter.ConvertFromString("#FF000000")
            End If
        End If
    End Sub

#End Region

End Class


