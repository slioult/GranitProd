Imports MGranitDALcsharp

Public Class DeverrouilleCommande

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().

    End Sub

    ''' <summary>
    ''' Évènement se produisant lors du click sur le bouton permettant le déverrouillage d'une commande
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDeverrouille_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Integer.TryParse(Me.TxtNumCmd.Text, 0) Then
            Dim num As Integer = Integer.Parse(Me.TxtNumCmd.Text)
            Dim connection As New MGConnection(My.Settings.DBSource)
            Dim parameters As New List(Of MySql.Data.MySqlClient.MySqlParameter)

            Try
                connection.Open()

                Dim parNum As MySql.Data.MySqlClient.MySqlParameter = connection.Create("@Num", System.Data.DbType.Int32, num)
                parameters.Add(parNum)

                connection.ExecuteNonQuery("UPDATE Commande SET Flag=0 WHERE NumCmd=@Num", parameters)

                MessageBox.Show("La commande a été déverrouillée", "Commande déverrouillée", MessageBoxButton.OK, MessageBoxImage.Information)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
            Finally
                connection.Close()
                connection = Nothing
                parameters = Nothing
            End Try
        Else
            MessageBox.Show("Numéro de commande non valide", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
        End If
    End Sub

End Class
