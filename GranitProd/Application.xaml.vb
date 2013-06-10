Class Application

    ' Les événements de niveau application, par exemple Startup, Exit et DispatcherUnhandledException
    ' peuvent être gérés dans ce fichier.

    ''' <summary>
    ''' Permet renseigner toutes les épaisseurs existantes dans les combobox correspondant à l'épaisseur de chaque matériau d'une commande
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CbxEpaisseur_Initialized(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim cbx As ComboBox = sender
        Dim Objects As New List(Of List(Of Object))
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Try
            connection.Open()
            Objects = connection.ExecuteQuery("Select Value From Epaisseur Order By Value")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        If (Objects.Count <> 0) Then
            For Each obj In Objects
                cbx.Items.Add(Integer.Parse(obj(0)))
            Next
        End If
    End Sub

End Class
