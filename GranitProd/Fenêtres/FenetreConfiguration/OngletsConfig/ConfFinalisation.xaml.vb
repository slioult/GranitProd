Public Class ConfFinalisation

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

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Me.CbxConfFinalisation.Items.Add(New Finalisation("Nouvelle", "", True))

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
                Dim question As MessageBoxResult = MessageBox.Show("Voulez-vous vraiment supprimer la prestation sélectionnée ?", "Suppression d'une prestation", MessageBoxButton.YesNo, MessageBoxImage.Question)
                If question = MessageBoxResult.Yes Then
                    Me.CbxConfFinalisation.Items.Remove(Me.CbxConfFinalisation.SelectedItem)
                    finalisation.Delete()
                    Dim ft As New FinalisationTemplate(finalisation)
                    Me.NouvelleCommande.LbxFinalisations.Items.Remove(ft)
                    If Me.Planning IsNot Nothing Then Me.Planning.Fill()
                    Me.CbxConfFinalisation.SelectedIndex = 0

                    MessageBox.Show("La prestation a été supprimée.", "Prestation supprimée", MessageBoxButton.OK, MessageBoxImage.Information)
                End If
            Else
                MessageBox.Show("La prestation est utilisée dans une commande et ne peut pas être supprimée.", "Suppression d'une prestation", MessageBoxButton.OK, MessageBoxImage.Stop)
            End If
        Else
            MessageBox.Show("Veuillez sélectionner une prestation à supprimer", "Suppression d'une prestation", MessageBoxButton.OK, MessageBoxImage.Stop)
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

            Dim finalisation As New Finalisation(TxtNomFinalisation.Text) ''Creation de la nouvelle Finalisation
            Dim isExistsLabel As Boolean = False
            Dim isExistsColor As Boolean = False

            If ChkDisplayColor.IsChecked Then
                finalisation.Color = CPFinalisation.SelectedColorText
                finalisation.Display = ChkDisplayColor.IsChecked
            End If

            For Each item In Me.CbxConfFinalisation.Items
                Dim tempFinalisation As Finalisation = item
                If Not tempFinalisation.Equals(Me.CbxConfFinalisation.SelectedItem) Then
                    If tempFinalisation.Label.ToUpper() = finalisation.Label.ToUpper() Then
                        isExistsLabel = True
                    End If
                    If finalisation.Display Then
                        If tempFinalisation.Color = finalisation.Color Then
                            isExistsColor = True
                        End If
                    End If
                End If
            Next
            If finalisation.Display Then
                For Each c In mesure.GetColorsReleves
                    If c = finalisation.Color Then
                        isExistsColor = True
                    End If
                Next
            End If

            If Not isExistsLabel And Not isExistsColor Then
                finalisation.Identifier = finalisation.Insert()
                Me.CbxConfFinalisation.Items.Add(finalisation)
                Me.CbxConfFinalisation.SelectedItem = finalisation
                Dim ft As New FinalisationTemplate(finalisation)
                Me.NouvelleCommande.LbxFinalisations.Items.Add(ft)
                If Me.Planning IsNot Nothing Then Me.Planning.Fill()

                MessageBox.Show("La prestation a été ajoutée.", "Prestation ajoutée", MessageBoxButton.OK, MessageBoxImage.Information)
            Else
                If isExistsLabel Then
                    MessageBox.Show("La prestation existe déjà.", "Prestation existante", MessageBoxButton.OK, MessageBoxImage.Stop)
                Else
                    MessageBox.Show("La couleur sélectionnée est déjà attribuée", "Couleur attribuée", MessageBoxButton.OK, MessageBoxImage.Stop)
                End If
            End If
        ElseIf Me.CbxConfFinalisation.SelectedIndex > 0 And TxtNomFinalisation.Text <> "" Then

            Dim index = Me.CbxConfFinalisation.SelectedIndex
            Dim finalisation As Finalisation = Me.CbxConfFinalisation.SelectedItem
            Dim isExistsLabel As Boolean = False
            Dim isExistsColor As Boolean = False

            finalisation.Display = ChkDisplayColor.IsChecked

            If Not finalisation.Display Then
                finalisation.Color = ""
            Else
                finalisation.Color = CPFinalisation.SelectedColorText
            End If


            For Each item In Me.CbxConfFinalisation.Items
                Dim tempFinalisation As Finalisation = item
                If Not tempFinalisation.Equals(Me.CbxConfFinalisation.SelectedItem) Then
                    If tempFinalisation.Label.ToUpper() = TxtNomFinalisation.Text.ToUpper() Then
                        isExistsLabel = True
                    End If
                    If finalisation.Display Then
                        If tempFinalisation.Color = CPFinalisation.SelectedColorText Then
                            isExistsColor = True
                        End If
                    End If
                End If
            Next

            If finalisation.Display Then
                Dim listColorsReleves As List(Of String) = mesure.GetColorsReleves

                For Each c In listColorsReleves
                    If c = CPFinalisation.SelectedColorText Then
                        isExistsColor = True
                    End If
                Next
            End If

            If Not isExistsLabel And Not isExistsColor Then
                Dim result As MessageBoxResult = MessageBox.Show("Voulez-vous modifier la prestation « " + finalisation.Label + " » ?", "Modifier une prestation",
                                                                 MessageBoxButton.YesNo, MessageBoxImage.Question)

                If result = MessageBoxResult.Yes Then
                    finalisation.Label = TxtNomFinalisation.Text
                    finalisation.Display = ChkDisplayColor.IsChecked
                    If finalisation.Display Then
                        finalisation.Color = CPFinalisation.SelectedColor.ToString()
                    Else
                        finalisation.Color = ""
                    End If
                    finalisation.Update()

                    Me.CbxConfFinalisation.Items.RemoveAt(index)
                    Me.CbxConfFinalisation.Items.Insert(index, finalisation)

                    Dim finT As New List(Of FinalisationTemplate)
                    For Each item In Me.NouvelleCommande.LbxFinalisations.Items
                        Dim ft As FinalisationTemplate = item
                        If ft.Identifier = finalisation.Identifier Then
                            ft.Label = finalisation.Label
                            ft.Color = finalisation.Color
                        End If

                        finT.Add(ft)
                    Next

                    Me.NouvelleCommande.LbxFinalisations.Items.Clear()

                    For Each f In finT
                        Me.NouvelleCommande.LbxFinalisations.Items.Add(f)
                    Next
                    If Me.Planning IsNot Nothing Then Me.Planning.Fill()

                    Me.CbxConfFinalisation.SelectedIndex = index
                    MessageBox.Show("La prestation a été modifiée.", "Prestation modifiée", MessageBoxButton.OK, MessageBoxImage.Information)
                End If
            Else
                If isExistsLabel Then
                    MessageBox.Show("La prestation existe déjà", "Prestation existante", MessageBoxButton.OK, MessageBoxImage.Stop)
                Else
                    MessageBox.Show("La couleur sélectionnée est déjà attribuée.", "Couleur attribuée", MessageBoxButton.OK, MessageBoxImage.Stop)
                End If
            End If

            End If
    End Sub

#End Region

#Region "Events"

    ''' <summary>
    ''' Permettre de changer la couleur afficher par rapport a la prestation selectionné dans la combobox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CbxConfFinalisation_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If CbxConfFinalisation.SelectedItem IsNot Nothing Then
            Dim f As Finalisation = CbxConfFinalisation.SelectedItem
            If CbxConfFinalisation.SelectedIndex <> 0 Then
                If Not f.Display Then
                    CPFinalisation.SelectedColor = ColorConverter.ConvertFromString("#FF000000")
                    CPFinalisation.Visibility = Windows.Visibility.Hidden
                Else
                    CPFinalisation.SelectedColor = ColorConverter.ConvertFromString(f.Color)
                    CPFinalisation.Visibility = Windows.Visibility.Visible
                End If
            Else
                CPFinalisation.SelectedColor = ColorConverter.ConvertFromString("#FF000000")
                CPFinalisation.Visibility = Windows.Visibility.Hidden
            End If

        End If

    End Sub

    ''' <summary>
    ''' Changement lors que la chkbox est check ou pas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChkDisplayColor_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If ChkDisplayColor.IsChecked Then
            CPFinalisation.Visibility = Windows.Visibility.Visible
        Else
            CPFinalisation.Visibility = Windows.Visibility.Hidden
        End If
    End Sub

#End Region

End Class
