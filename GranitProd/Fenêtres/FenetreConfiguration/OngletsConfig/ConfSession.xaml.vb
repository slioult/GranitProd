Public Class ConfSession

#Region "Constructor"
    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        'Remplir la CmbConfSession
        Me.CbxConfSession.Items.Add(New Session("Nouveau", ""))

        Dim sessions As New List(Of Session)
        sessions = Session.GetSessions()

        For Each s In sessions
            Me.CbxConfSession.Items.Add(s)
        Next

        Me.CbxConfSession.SelectedIndex = 0
        Me.CbxConfSession_SelectionChanged(Nothing, Nothing)

    End Sub
#End Region

#Region "Button"

    ''' <summary>
    ''' Bouton de suppresion d'une session
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Me.CbxConfSession.SelectedIndex > 0 Then
            Dim session As Session = Me.CbxConfSession.SelectedItem
            Dim question As MessageBoxResult = MessageBox.Show("Voulez-vous vraiment supprimer la session sélectionnée ?", "Suppression d'une session", MessageBoxButton.YesNo, MessageBoxImage.Question)
            If question = MessageBoxResult.Yes Then
                Me.CbxConfSession.Items.Remove(Me.CbxConfSession.SelectedItem)
                session.Delete()
                Me.CbxConfSession.SelectedIndex = 0

                MessageBox.Show("La session a été supprimée.", "Session supprimée", MessageBoxButton.OK, MessageBoxImage.Information)
            End If
        Else
            MessageBox.Show("Veuillez sélectionner une session à supprimer", "Suppression d'une session", MessageBoxButton.OK, MessageBoxImage.Stop)
        End If
    End Sub

    ''' <summary>
    ''' Bouton de sauvegarde d'une Session
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Me.CbxConfSession.SelectedIndex = 0 And TxtIdSession.Text <> "" Then
            If PwdBxMdpSession.Password <> "" Then
                If Me.PwdBxMdpSession.Password.ToUpper = Me.PwdBxConfirmationMdpSession.Password.ToUpper Then
                    Dim session As New Session(TxtIdSession.Text, PwdBxMdpSession.Password, ChkAddCmd.IsChecked, ChkUpdCmd.IsChecked, ChkDelCmd.IsChecked, ChkDispCA.IsChecked, ChkDispPaneld.IsChecked, ChkUpdConfig.IsChecked, ChkUpdSession.IsChecked)
                    Dim isExists As Boolean = False
                    For Each item In Me.CbxConfSession.Items
                        Dim tempSession As Session = item
                        If tempSession.Login.ToUpper() = session.Login.ToUpper() Then
                            isExists = True
                            Exit For
                        End If
                    Next

                    If Not isExists Then
                        session.Identifier = session.Insert()
                        Me.CbxConfSession.Items.Add(session)
                        Me.CbxConfSession.SelectedItem = session
                        Me.PwdBxMdpSession.Clear()
                        Me.PwdBxConfirmationMdpSession.Clear()
                        MessageBox.Show("La session a été ajoutée.", "Nouvelle session ajoutée", MessageBoxButton.OK, MessageBoxImage.Information)
                    Else
                        MessageBox.Show("La session existe déjà.", "Session existante", MessageBoxButton.OK, MessageBoxImage.Stop)
                    End If
                Else
                    MessageBox.Show("Veuillez saisir deux mots de passe identiques", "Mots de passe non conforme", MessageBoxButton.OK, MessageBoxImage.Stop)
                End If
            Else
                MessageBox.Show("Veuillez saisir un mot de passe", "Mot de passe non saisi", MessageBoxButton.OK, MessageBoxImage.Stop)
            End If

        ElseIf Me.CbxConfSession.SelectedIndex > 0 And TxtIdSession.Text <> "" Then
            If PwdBxMdpSession.Password <> "" Then
                If PwdBxMdpSession.Password = PwdBxConfirmationMdpSession.Password Then
                    Dim index = Me.CbxConfSession.SelectedIndex
                    Dim session As Session = Me.CbxConfSession.SelectedItem

                    Dim isExists As Boolean = False
                    For Each item In Me.CbxConfSession.Items
                        Dim tempSession As Session = item
                        If Not item.Equals(CbxConfSession.SelectedItem) Then
                            If tempSession.Login.ToUpper() = TxtIdSession.Text.ToUpper() Then
                                isExists = True
                                Exit For
                            End If
                        End If
                    Next

                    If Not isExists Then
                        Dim result As MessageBoxResult = MessageBox.Show("Voulez-vous modifier la session « " + session.Login + " » ?", "Modification d'une session",
                                                                         MessageBoxButton.YesNo, MessageBoxImage.Question)

                        If result = MessageBoxResult.Yes Then
                            session.Login = TxtIdSession.Text
                            session.Password = PwdBxMdpSession.Password
                            session.IsAddCmd = ChkAddCmd.IsChecked
                            session.IsUpdCmd = ChkUpdCmd.IsChecked
                            session.IsDelCmd = ChkDelCmd.IsChecked
                            session.IsDispCA = ChkDispCA.IsChecked
                            session.IsDispPanel = ChkDispPaneld.IsChecked
                            session.IsUpdConfig = ChkUpdConfig.IsChecked
                            session.IsUpdSession = ChkUpdSession.IsChecked
                            session.Update()

                            Me.CbxConfSession.Items.RemoveAt(index)
                            Me.CbxConfSession.Items.Insert(index, session)

                            Me.CbxConfSession.SelectedIndex = index
                            Me.PwdBxMdpSession.Clear()
                            Me.PwdBxConfirmationMdpSession.Clear()
                            MessageBox.Show("La session a été modifiée", "Session modifiée", MessageBoxButton.OK, MessageBoxImage.Information)
                        End If
                    Else
                        MessageBox.Show("La session existe déjà.", "Session existante", MessageBoxButton.OK, MessageBoxImage.Stop)
                    End If
                Else
                    MessageBox.Show("Veuillez saisir deux mots de passe identiques", "Mots de passe non conforme", MessageBoxButton.OK, MessageBoxImage.Stop)
                End If
            Else
                MessageBox.Show("Veuillez saisir un mot de passe", "Mot de passe non saisi", MessageBoxButton.OK, MessageBoxImage.Stop)
            End If


        End If
    End Sub

#End Region

#Region "Events"

    ''' <summary>
    ''' Évènement se produisant lorsque la session sélectionnée change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CbxConfSession_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If CbxConfSession.SelectedItem IsNot Nothing Then
            PwdBxMdpSession.Clear()
            PwdBxConfirmationMdpSession.Clear()
        End If

        If Me.CbxConfSession.SelectedIndex = 0 Then
            Me.ChkAddCmd.IsChecked = False
            Me.ChkDelCmd.IsEnabled = False
            Me.ChkDelCmd.IsChecked = False
            Me.ChkDispCA.IsEnabled = False
            Me.ChkDispCA.IsChecked = False
            Me.ChkDispPaneld.IsEnabled = False
            Me.ChkDispPaneld.IsChecked = False

            Me.ChkDispCA.IsChecked = False
            Me.ChkDispPaneld.IsEnabled = False
            Me.ChkDispPaneld.IsChecked = False

            Me.ChkUpdConfig.IsChecked = False
            Me.ChkUpdSession.IsEnabled = False
            Me.ChkUpdSession.IsChecked = False
        End If
    End Sub

    ''' <summary>
    ''' Évènement se produisant lorsque le droit d'ajout de commande est coché
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChkAddCmd_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.ChkDelCmd.IsEnabled = True
        Me.ChkDispCA.IsEnabled = True
        Me.ChkUpdCmd.IsChecked = True
    End Sub

    ''' <summary>
    ''' Évènement se produisant lorsque le droit d'ajout de commande est décoché
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChkAddCmd_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.ChkDelCmd.IsEnabled = False
        Me.ChkDelCmd.IsChecked = False
        Me.ChkDispCA.IsEnabled = False
        Me.ChkDispCA.IsChecked = False
        Me.ChkDispPaneld.IsEnabled = False
        Me.ChkDispPaneld.IsChecked = False
    End Sub

    ''' <summary>
    ''' Évènement se produisant lorsque le droit d'affichage du chiffre d'affaire est coché
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChkDispCA_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.ChkDispPaneld.IsEnabled = True
    End Sub

    ''' <summary>
    ''' Évènement se produisant lorsque le droit d'affichage du chiffre d'affaire est décoché
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChkDispCA_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.ChkDispPaneld.IsEnabled = False
        Me.ChkDispPaneld.IsChecked = False
    End Sub

    ''' <summary>
    ''' Évènement se produisant lorsque le droit de mise à jour de la configuration est coché
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChkUpdConfig_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.ChkUpdSession.IsEnabled = True
    End Sub

    ''' <summary>
    ''' Évènement se produisant lorsque le droit de mise à jour de la configuration est décoché
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChkUpdConfig_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.ChkUpdSession.IsEnabled = False
        Me.ChkUpdSession.IsChecked = False
    End Sub

#End Region

End Class
