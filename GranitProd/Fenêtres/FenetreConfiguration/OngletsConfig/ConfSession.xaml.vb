Imports MGranitDALcsharp
Imports MySql.Data.MySqlClient
Imports System.Data

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
            Dim question As MessageBoxResult = MessageBox.Show("Voulez vous vraiment supprimer la session selectionnée ?", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Warning)
            If question = MessageBoxResult.Yes Then
                Me.CbxConfSession.Items.Remove(Me.CbxConfSession.SelectedItem)
                session.Delete()
                Me.CbxConfSession.SelectedIndex = 0
            End If
        Else
            MessageBox.Show("Veuillez sélectionner une session à supprimer.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning)
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
                    Dim session As New Session(TxtIdSession.Text, PwdBxMdpSession.Password, ChkAddCmd.IsChecked, ChkUpdCmd.IsChecked, ChkDelCmd.IsChecked, ChkDispCA.IsChecked, ChkDispPaneld.IsChecked,
                                               ChkUpdConfig.IsChecked, ChkUpdSession.IsChecked)
                    Dim isExists As Boolean = False
                    For Each item In Me.CbxConfSession.Items
                        Dim tempSession As Session = item
                        If Not item.Equals(Me.CbxConfSession.SelectedItem) Then
                            If tempSession.Login.ToUpper() = session.Login.ToUpper() Then
                                isExists = True
                                Exit For
                            End If
                        End If
                    Next

                    If Not isExists And Me.CbxConfSession.SelectedIndex Then
                        session.Identifier = session.Insert()
                        Me.CbxConfSession.Items.Add(session)
                        Me.CbxConfSession.SelectedItem = session
                        Me.PwdBxMdpSession.Clear()
                        Me.PwdBxConfirmationMdpSession.Clear()
                        MessageBox.Show("La session a été ajoutée avec succès.", "Nouvelle session ajoutée", MessageBoxButton.OK, MessageBoxImage.Information)
                    Else
                        MessageBox.Show("La session existe déjà.", "Session existante", MessageBoxButton.OK, MessageBoxImage.Information)
                    End If
                Else
                    MessageBox.Show("Veuillez saisir deux mots de passe identiques.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning)
                End If
            Else
                MessageBox.Show("Veuillez saisir un mot de passe.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning)
            End If

        ElseIf Me.CbxConfSession.SelectedIndex > 0 And TxtIdSession.Text <> "" Then
            If PwdBxMdpSession.Password <> "" Then
                If PwdBxMdpSession.Password = PwdBxConfirmationMdpSession.Password Then
                    Dim index = Me.CbxConfSession.SelectedIndex
                    Dim session As Session = Me.CbxConfSession.SelectedItem

                    Dim isExists As Boolean = False
                    For Each item In Me.CbxConfSession.Items
                        Dim tempSession As Session = item
                        If tempSession.Login.ToUpper() = TxtIdSession.Text.ToUpper() Then
                            isExists = True
                            Exit For
                        End If
                    Next

                    If Not isExists Then
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
                        MessageBox.Show("La session a été modifiée avec succès.", "Session modifiée", MessageBoxButton.OK, MessageBoxImage.Information)
                    Else
                        MessageBox.Show("La session existe déjà.", "Session existante", MessageBoxButton.OK, MessageBoxImage.Information)
                    End If
                Else
                    MessageBox.Show("Veuillez saisir deux mots de passe identiques.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning)
                End If
            Else
                MessageBox.Show("Veuillez saisir un mot de passe.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning)
            End If


        End If
    End Sub

#End Region

End Class
