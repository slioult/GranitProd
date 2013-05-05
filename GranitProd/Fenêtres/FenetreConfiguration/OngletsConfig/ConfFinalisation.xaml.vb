Imports MGranitDALcsharp
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class ConfFinalisation

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
        Me.CbxConfFinalisation.Items.Add(New Finalisation("Nouveau"))

        Dim finalisations As New List(Of Finalisation)
        finalisations = Finalisation.GetFinalisations()

        For Each e In finalisations
            Me.CbxConfFinalisation.Items.Add(e)
        Next

        Me.CbxConfFinalisation.SelectedIndex = 0

    End Sub
#End Region

#Region "Button"

    ''' <summary>
    ''' Bouton de suppresion d'une Finalisation
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Me.CbxConfFinalisation.SelectedIndex > 0 Then
            Dim finalisation As Finalisation = Me.CbxConfFinalisation.SelectedItem
            If Not finalisation.IsUsed Then
                Dim question As MessageBoxResult = MessageBox.Show("Voulez vous vraiment supprimer la Finalisation selectionnée", "Caution", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                If question = MessageBoxResult.Yes Then
                    Me.CbxConfFinalisation.Items.Remove(Me.CbxConfFinalisation.SelectedItem)
                    finalisation.Delete()
                    Dim ft As New FinalisationTemplate(finalisation)
                    Me.NouvelleCommande.LbxFinalisations.Items.Remove(ft)
                    Me.CbxConfFinalisation.SelectedIndex = 0
                End If
            Else
                MessageBox.Show("La prestation est utilisée dans une commande et ne peut pas etre supprimer")
            End If
        Else
            MessageBox.Show("Selectionnez une Prestation a supprimé")
        End If
    End Sub

    ''' <summary>
    ''' Bouton de sauvegarde d'un Finalisation
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim mesure As New Mesure
        If Me.CbxConfFinalisation.SelectedIndex = 0 And TxtNomFinalisation.Text <> "" Then

            Dim finalisation As New Finalisation(TxtNomFinalisation.Text, CPFinalisation.SelectedColor.ToString())
            Dim isExistsLabel As Boolean = False
            Dim isExistsColor As Boolean = False

            For Each item In Me.CbxConfFinalisation.Items
                Dim tempFinalisation As Finalisation = item
                If Not item.Equals(Me.CbxConfFinalisation.SelectedItem) Then
                    If tempFinalisation.Label.ToUpper() = finalisation.Label.ToUpper() Then
                        isExistsLabel = True
                    End If
                    If tempFinalisation.Color = finalisation.Color Then
                        isExistsColor = True
                    End If
                End If
            Next
            For Each c In mesure.GetColorsReleves
                If c = finalisation.Color Then
                    isExistsColor = True
                End If
            Next

            If Not isExistsLabel And Not isExistsColor Then
                finalisation.Identifier = finalisation.Insert()
                Me.CbxConfFinalisation.Items.Add(finalisation)
                Me.CbxConfFinalisation.SelectedItem = finalisation
                Dim ft As New FinalisationTemplate(finalisation)
                Me.NouvelleCommande.LbxFinalisations.Items.Add(ft)
                MessageBox.Show("La Finalisation a été ajouté")
            Else
                If isExistsLabel Then
                    MessageBox.Show("La prestation existe")
                Else
                    MessageBox.Show("La couleur est deja attribué")
                End If
            End If
        ElseIf Me.CbxConfFinalisation.SelectedIndex > 0 And TxtNomFinalisation.Text <> "" Then

            Dim index = Me.CbxConfFinalisation.SelectedIndex

            Dim finalisation As Finalisation = Me.CbxConfFinalisation.SelectedItem
            Dim isExistsLabel As Boolean = False
            Dim isExistsColor As Boolean = False

            For Each item In Me.CbxConfFinalisation.Items
                Dim tempFinalisation As Finalisation = item
                If Not item.Equals(Me.CbxConfFinalisation.SelectedItem) Then
                    If tempFinalisation.Label.ToUpper() = TxtNomFinalisation.Text.ToUpper() Then
                        isExistsLabel = True
                    End If
                    If tempFinalisation.Color = CPFinalisation.SelectedColor.ToString() Then
                        isExistsColor = True
                    End If
                End If
            Next

            For Each c In mesure.GetColorsReleves
                If c = finalisation.Color Then
                    isExistsColor = True
                End If
            Next

            If Not isExistsLabel And Not isExistsColor Then
                finalisation.Label = TxtNomFinalisation.Text
                finalisation.Color = CPFinalisation.SelectedColor.ToString()
                finalisation.Update()

                Me.CbxConfFinalisation.Items.RemoveAt(index)
                Me.CbxConfFinalisation.Items.Insert(index, finalisation)

                Me.CbxConfFinalisation.SelectedIndex = index
                MessageBox.Show("La Finalisation a été modifié")
            Else
                If isExistsLabel Then
                    MessageBox.Show("La prestation existe")
                Else
                    MessageBox.Show("La couleur est deja attribué")
                End If
            End If

        End If
    End Sub

#End Region

    Private Sub CbxConfFinalisation_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If CbxConfFinalisation.SelectedItem IsNot Nothing Then
            Dim f As Finalisation = CbxConfFinalisation.SelectedItem
            If CbxConfFinalisation.SelectedIndex <> 0 Then
                CPFinalisation.SelectedColor = ColorConverter.ConvertFromString(f.Color)
            Else
                CPFinalisation.SelectedColor = ColorConverter.ConvertFromString("#FF000000")
            End If
        End If



    End Sub

End Class
