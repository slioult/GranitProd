Imports MGranitDALcsharp
Imports System.IO
Imports System.Runtime.InteropServices

Public Class Connexion

#Region "Fields"

    Private isOk As Boolean

#End Region

#Region "Constructor"

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        TbVersion.Text = "2013 GranitProd - Version " + My.Application.Info.Version.ToString().Substring(0, My.Application.Info.Version.ToString().Length - 2)

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        TxtLogin.Focus()
        isOk = True

        'Fichier de config (notamment pour le dernier compte connecté sur ce PC
        If Not Directory.Exists(My.Settings.ConfigFiles) Then Directory.CreateDirectory(My.Settings.ConfigFiles)
        If Not File.Exists(My.Settings.ConfigFile) Then
            File.Create(My.Settings.ConfigFile)
        Else
            Dim sr As New StreamReader(My.Settings.ConfigFile)

            Dim login As String = sr.ReadToEnd()

            sr.Close()

            If login <> "" Then
                Me.TxtLogin.Text = login
                Me.PsxPassword.Focus()

            End If
        End If
    End Sub

#End Region

#Region "Button"

    ''' <summary>
    ''' Action de l'évènement de clique sur le bouton Connexion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnConnexion_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

        Try
            If (TxtLogin.Text <> "") Then
                If (PsxPassword.Password <> "") Then
                    Dim login As String = TxtLogin.Text
                    Dim password As String = PsxPassword.Password

                    'Récupère les différentes sessions existantes.
                    Dim Sessions As List(Of Session) = Session.GetSessions(True)

                    Dim isExists As Boolean = False

                    'vérifie le login et le mot de passe
                    For Each s As Session In Sessions
                        If (s.Login = login And Crypt.verifyMd5Hash(password, s.Password)) Then
                            SaveLogin(login)
                            Dim main As New MainWindow(s)
                            main.Show()
                            Me.Close()
                            isExists = True
                            Exit For
                        End If
                    Next

                    If (isExists = False) Then
                        MessageBox.Show("Identifiant ou mot de passe incorrect.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
                        PsxPassword.Password = String.Empty
                        PsxPassword.Focus()
                        isOk = False
                    End If

                Else
                    MessageBox.Show("Veuillez entrer mot de passe.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
                    PsxPassword.Focus()
                    isOk = False
                End If
            Else
                MessageBox.Show("Veuillez entrer un identifiant.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
                isOk = False
                TxtLogin.Focus()
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message)
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

#End Region

#Region "EventControlEnter"

    ''' <summary>
    ''' Permet d'appeler l'évènement de click sur le bouton "Connexion" lorsque la touche Enter est pressée dans le PsxPassword
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PsxPassword_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If (isOk) Then
            If (e.Key = System.Windows.Input.Key.Enter) Then
                BtnConnexion_Click(Nothing, Nothing)
            End If
        Else
            isOk = True
        End If
    End Sub

    ''' <summary>
    ''' Permet d'appeler l'évènement de click sur le bouton "Connexion" lorsque la touche Enter est pressée dans le TxtLogin
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TxtLogin_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If (isOk) Then
            If (e.Key = System.Windows.Input.Key.Enter) Then
                BtnConnexion_Click(Nothing, Nothing)
            End If
        Else
            isOk = True
        End If
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Sauvegarde le login utilisé dans le fichier conf.ini
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SaveLogin(ByVal login As String)

        If File.Exists(My.Settings.ConfigFile) Then
            Dim sw As New StreamWriter(My.Settings.ConfigFile)

            sw.Write(login)

            sw.Close()
        End If

    End Sub

#End Region

End Class
