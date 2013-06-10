Imports System.IO

Class MainWindow

#Region "Fields"
    Private _Session As Session
#End Region

#Region "Properties"

    Public Property Session As Session
        Get
            Return Me._Session
        End Get
        Set(ByVal value As Session)
            Me._Session = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal Session As Session)
        Me.Session = Session
        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().

        ' Renseigne le titre de la fenêtre en fonction de la session active.
        Me.Title = "GranitProd - " + Me.Session.Login

        ' Met en place les droits relatifs à la session active.
        If (Me.Session.IsAddCmd) Then
            TabNewCmd.Visibility = System.Windows.Visibility.Visible
        ElseIf Not Me.Session.IsDispCA Then
            Me.SearchCommande.BtnPdf.Visibility = Windows.Visibility.Collapsed
            Me.SearchCommande.BtnExcel.Visibility = Windows.Visibility.Collapsed
        End If

        If (Me.Session.IsDispCA) Then
            TabCA.Visibility = System.Windows.Visibility.Visible
            If Me.Session.IsDispPanel Then
                Me.BtnPanel.Visibility = Windows.Visibility.Visible
            End If
        Else
            Me.SearchCommande.BtnPdf.Visibility = Windows.Visibility.Collapsed
            Me.SearchCommande.BtnExcel.Visibility = Windows.Visibility.Collapsed
        End If

        If (Me.Session.IsUpdConfig = False) Then
            Me.BtnConfig.Visibility = Windows.Visibility.Collapsed
        End If

        Me.NewCommande.Planning = Me.planning
        Me.planning.Session = Me.Session

        If Not Directory.Exists(My.Settings.ExportFile) Then
            Directory.CreateDirectory(My.Settings.ExportFile)
        End If

        Me.planning.Session = Me.Session
        Me.planning.Search = SearchCommande

    End Sub

#End Region

#Region "Button"

    ''' <summary>
    ''' Bouton servant à lancer la fenêtre de configuration
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnConfig_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim conf As New ConfigurationWindow(Me.Session, Me.NewCommande, Me.planning)
        conf.ShowDialog()
    End Sub

    ''' <summary>
    ''' Bouton permettant d'ouvrir le tableau de bord
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnPanel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim panel As New Panel(Me.Session, Me.planning)
        panel.Show()
    End Sub

    ''' <summary>
    ''' Évènement se produisant lors du clique sur le menu "À propos de..."
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MenuItem_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim v As New Version()
        v.ShowDialog()
    End Sub

    ''' <summary>
    ''' Évènement se produisant lors du clique sur le menu "Aide"
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MenuItemAide_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Process.Start(My.Settings.Help)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

#End Region

End Class
