Imports MySql.Data.MySqlClient
Imports System.Data

Public Class Qualite

#Region "Fields"

    Private _Identifier As Long
    Private _Type As String
    Private _Source As String
    Private _DatePost As DateTime
    Private _Remarque As String

#End Region

#Region "Properties"

    Public Property Identifier As Long
        Get
            Return Me._Identifier
        End Get
        Set(ByVal value As Long)
            Me._Identifier = value
        End Set
    End Property

    Public Property Type As String
        Get
            Return Me._Type
        End Get
        Set(ByVal value As String)
            Me._Type = value
        End Set
    End Property

    Public Property Source As String
        Get
            Return Me._Source
        End Get
        Set(ByVal value As String)
            Me._Source = value
        End Set
    End Property

    Public Property DatePost As DateTime
        Get
            Return Me._DatePost
        End Get
        Set(ByVal value As DateTime)
            Me._DatePost = value
        End Set
    End Property

    Public Property Remarque As String
        Get
            Return Me._Remarque
        End Get
        Set(ByVal value As String)
            Me._Remarque = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()
    End Sub

    Public Sub New(ByVal type As String, Optional ByVal identifier As Long = 0, Optional ByVal source As String = "", Optional ByVal datePost As DateTime = Nothing, Optional ByVal remarque As String = "")
        Me.Identifier = identifier
        Me.Type = type
        Me.Source = source
        Me.DatePost = datePost
        Me.Remarque = remarque
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Surcharge de la méthode Equals permettant de comparer deux qualités
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (Me IsNot Nothing And obj IsNot Nothing) Then
            If (TypeOf (obj) Is Qualite) Then
                Dim q As Qualite = obj
                If (Me.Remarque = q.Remarque And Me.Source = q.Source And Me.Type = q.Type And Me.DatePost.Year = q.DatePost.Year And
                                                                                              Me.DatePost.Month = q.DatePost.Month And
                                                                                              Me.DatePost.Day = q.DatePost.Day And
                                                                                              Me.DatePost.Hour = q.DatePost.Hour And
                                                                                              Me.DatePost.Minute = q.DatePost.Minute And
                                                                                              Me.DatePost.Second = q.DatePost.Second) Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

#End Region

#Region "DataAccess"

    ''' <summary>
    ''' Permet de récupérer les informations de la qualité
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetQualite() As Qualite
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)
        Dim Objects As New List(Of List(Of Object))

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parIdentifier As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdentifier)

            'Exécute la requête
            Objects = connection.ExecuteQuery("SELECT Identifier, Type FROM Qualite WHERE Identifier=@Identifier", parameters)

            'Ferme la connection
            connection.Close()
            parameters = Nothing

            'Traite les résultats
            For Each obj In Objects
                Me.Type = obj(1).ToString()
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                'Assure la fermeture de la connection
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        Return Me
    End Function

    ''' <summary>
    ''' Permet de récupérer les type de qualité dans la base de données
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetQualites() As List(Of Qualite)
        Dim qualites As New List(Of Qualite)
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))

        Try
            'Ouvre la connection
            connection.Open()

            'Exécute la requête
            Objects = connection.ExecuteQuery("SELECT Identifier, Type FROM Qualite Order By Type")

            'Ferme la connection
            connection.Close()

            'Traite les résultats
            For Each obj In Objects
                qualites.Add(New Qualite(obj(1).ToString(), Long.Parse(obj(0))))
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                'Ferme la connection
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        Return qualites
    End Function

    ''' <summary>
    ''' Met à jour les problèmes de qualité d'une commande
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateQualitiesProblems(ByVal idCmd As Long)
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection
            connection.Open()

            Dim isExists As Boolean = False

            'Défini les paramètres de la requête
            Dim parIdentifierQlt As MySqlParameter = connection.Create("@IdentifierQualite", DbType.Int64, Me.Identifier)
            parameters.Add(parIdentifierQlt)

            Dim parIdentifierCmd As MySqlParameter = connection.Create("@IdentifierCommande", DbType.Int64, idCmd)
            parameters.Add(parIdentifierCmd)

            'Requête
            Dim query As String = "SELECT DateProbleme, Source From Commande_Qualite WHERE Identifier_Commande=@IdentifierCommande AND Identifier_Qualite=@IdentifierQualite"

            'Exécute la requête
            Objects = connection.ExecuteQuery(query, parameters)

            'Traite les résultats
            For Each obj In Objects
                Dim d As DateTime = DateTime.Parse(obj(0))
                Dim s As String = obj(1).ToString()

                'Vérifie si le problème de qualité est déjà signalé
                If d = Me.DatePost AndAlso s = Me.Source Then isExists = True
            Next

            'Si le problème de qualité n'a pas déjà été signalé
            If Not isExists Then

                'Défini les paramètres de la requête
                Dim parDate As MySqlParameter = connection.Create("@DateProbleme", DbType.DateTime, Me.DatePost)
                parameters.Add(parDate)

                Dim parSource As MySqlParameter = connection.Create("@Source", DbType.String, Me.Source)
                parameters.Add(parSource)

                Dim parRemarque As MySqlParameter = connection.Create("@Remarque", DbType.String, Me.Remarque)
                parameters.Add(parRemarque)

                'Requête
                query = "INSERT INTO Commande_Qualite (Identifier_Commande, Identifier_Qualite, DateProbleme, Source, Remarque) " +
                        "VALUES (@IdentifierCommande, @IdentifierQualite, @DateProbleme, @Source, @Remarque)"

                'Exécute la requête
                connection.ExecuteNonQuery(query, parameters)
            End If

            parameters = Nothing
            Objects = Nothing

            'Ferme la connection
            connection.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
        Finally
            Try
                'Assure la fermeture de la connection
                connection.Close()
            Catch
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Récupère tous les problèmes de qualité d'une commande
    ''' </summary>
    ''' <param name="idCmd">Identifier de la commande</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCommandeQualites(ByVal idCmd As Long) As List(Of Qualite)
        Dim qualites As New List(Of Qualite)
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parIdentifierCommande As MySqlParameter = connection.Create("@IdentifierCommande", DbType.Int64, idCmd)
            parameters.Add(parIdentifierCommande)

            'Requête
            Dim query As String = "SELECT q.Type, pq.Identifier_Qualite, pq.DateProbleme, pq.Source, pq.Remarque " +
                                  "FROM Qualite as q " +
                                  "INNER JOIN Commande_Qualite as pq ON q.Identifier=pq.Identifier_Qualite AND pq.Identifier_Commande=@IdentifierCommande Order By pq.DateProbleme"

            'Exécute la requête
            Objects = connection.ExecuteQuery(query, parameters)

            'Traite les résultats
            For Each obj In Objects
                Dim quality As New Qualite(obj(0).ToString(), Long.Parse(obj(1)), obj(3).ToString(), DateTime.Parse(obj(2)), obj(4).ToString())
                qualites.Add(quality)
            Next

            Objects = Nothing
            parameters = Nothing

            'Ferme la connection
            connection.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
        Finally
            Try
                'Assure la fermeture de la connection
                connection.Close()
            Catch
            End Try
        End Try

        Return qualites
    End Function

    ''' <summary>
    ''' Permet de savoir si une qualité est utiliser dans une commande
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsUsed() As Boolean
        Dim bool As Boolean = False
        Dim Objects As New List(Of List(Of Object))
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            connection.Open()

            Dim parIdQualite As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdQualite)

            Objects = connection.ExecuteQuery("SELECT COUNT(Identifier_Commande) FROM Commande_qualite WHERE Identifier_Qualite=@Identifier", parameters)

            For Each obj In Objects
                If Integer.Parse(obj(0)) > 0 Then
                    bool = True
                End If
            Next

            parameters.Clear()

            connection.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error")
        Finally
            Try
                connection.Close()
            Catch ex As Exception
            End Try
        End Try
        Return bool
    End Function

    ''' <summary>
    ''' Permet d'insérer une qualité en base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Insert() As Long
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)
        Dim Objects As New List(Of List(Of Object))

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parType As MySqlParameter = connection.Create("@Type", DbType.String, Me.Type)
            parameters.Add(parType)

            'Requête
            Dim query As String = "INSERT INTO Qualite (Type) VALUES (@Type)"

            'Exécute la requête
            connection.ExecuteNonQuery(query, parameters)

            'Récupère l'identifier du dernier enregistrement
            Objects = connection.ExecuteQuery("SELECT Max(Identifier) FROM Qualite")

            'Traite les résultats
            For Each obj In Objects
                Me.Identifier = Long.Parse(obj(0))
            Next

            parameters = Nothing

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                'Ferme la connection
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        Return Me.Identifier
    End Function

    ''' <summary>
    ''' Permet de mettre à jour une qualité en base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Update()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parIdQualite As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdQualite)
            Dim parType As MySqlParameter = connection.Create("@Type", DbType.String, Me.Type)
            parameters.Add(parType)

            'Requête
            Dim query As String = "UPDATE Qualite SET Type=@Type WHERE Identifier=@Identifier"

            'Exécute la requête
            connection.ExecuteNonQuery(query, parameters)

            parameters = Nothing

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                'Ferme la connection
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

    End Sub

    ''' <summary>
    ''' Permet de supprimer une qualité de la base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Delete()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parIdQualite As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdQualite)

            'Exécute la requête
            connection.ExecuteNonQuery("DELETE FROM Qualite WHERE Identifier=@Identifier", parameters)

            parameters.Clear()

            'Ferme la connection
            connection.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error")
        Finally
            Try
                'Assure la fermeture de la connection
                connection.Close()
            Catch ex As Exception
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Permet d'insérer un problème de qualité en base de données
    ''' </summary>
    ''' <param name="idCmd">Identifier de la commande à laquelle se rapporte le problème de qualité</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsertPb(ByVal idCmd As Long) As Long
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)
        Dim Objects As New List(Of List(Of Object))

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parRemarque As MySqlParameter = connection.Create("@Remarque", DbType.String, Me.Remarque)
            parameters.Add(parRemarque)

            Dim parSource As MySqlParameter = connection.Create("@Source", DbType.String, Me.Source)
            parameters.Add(parSource)

            Dim parDate As MySqlParameter = connection.Create("@Date", DbType.DateTime, Me.DatePost)
            parameters.Add(parDate)

            Dim parIdentifierQualite As MySqlParameter = connection.Create("@IdQualite", DbType.Int64, Me.Identifier)
            parameters.Add(parIdentifierQualite)

            Dim parIdentifierCommande As MySqlParameter = connection.Create("@IdCmd", DbType.Int64, idCmd)
            parameters.Add(parIdentifierCommande)

            Dim query As String
            query = "INSERT INTO Commande_Qualite (Identifier_Commande, Identifier_Qualite, DateProbleme, Source, Remarque) VALUES (@IdCmd, @IdQualite, @Date, @Source, @Remarque)"
            connection.ExecuteQuery(query, parameters)

            parameters = Nothing

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                'Ferme la connection
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        Return Me.Identifier
    End Function

    ''' <summary>
    ''' Permet de supprimer un problème de qualité en base de données
    ''' </summary>
    ''' <param name="idCmd">Identifier de la commande à laquelle se rapporte le problème de qualité</param>
    ''' <remarks></remarks>
    Public Sub DeletePb(ByVal idCmd As Long)
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parIdQualite As MySqlParameter = connection.Create("@IdQualite", DbType.Int64, Me.Identifier)
            parameters.Add(parIdQualite)

            Dim parIdCmd As MySqlParameter = connection.Create("@IdCmd", DbType.Int64, idCmd)
            parameters.Add(parIdCmd)

            Dim parDate As MySqlParameter = connection.Create("@DatePost", DbType.DateTime, Me.DatePost)
            parameters.Add(parDate)

            'Exécute la requête
            connection.ExecuteNonQuery("DELETE FROM Commande_Qualite WHERE Identifier_Commande=@IdCmd AND Identifier_Qualite=@IdQualite AND DateProbleme=@DatePost", parameters)

            parameters = Nothing

            'Ferme la connection
            connection.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error")
        Finally
            Try
                'Assure la fermeture de la connection
                connection.Close()
            Catch ex As Exception
            End Try
        End Try
    End Sub

#End Region

End Class
