Imports MySql.Data.MySqlClient
Imports System.Data

Public Class Remarque

#Region "Fields"

    Private _Identifier As Long
    Private _Comment As String
    Private _Source As String
    Private _DatePost As String

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


    Public Property Comment As String
        Get
            Return _Comment
        End Get
        Set(ByVal value As String)
            Me._Comment = value
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

    Public Property DatePost As String
        Get
            Return Me._DatePost
        End Get
        Set(ByVal value As String)
            Me._DatePost = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()
    End Sub

    Public Sub New(ByVal comment As String, ByVal source As String, ByVal datePost As String, Optional ByVal identifier As Long = 0)
        Me.Comment = comment
        Me.Source = source
        Me.DatePost = datePost
        Me.Identifier = identifier
    End Sub


#End Region

#Region "Methods"

    ''' <summary>
    ''' Surcharge de la méthode Equals permettant de comparer deux remarques
    ''' </summary>
    ''' <param name="obj">Remarque à comparer</param>
    ''' <returns>Retourne un booléen indiquant si les deux remarques sont identiques</returns>
    ''' <remarks></remarks>
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (Me IsNot Nothing And obj IsNot Nothing) Then
            If (TypeOf (obj) Is Remarque) Then
                Dim rm As Remarque = obj
                If (Me.Identifier = rm.Identifier And Me.Comment = rm.Comment And Me.Source = rm.Source And Me.DatePost = rm.DatePost) Then
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
    ''' Permet d'insérer une remarque dans la base de données
    ''' </summary>
    ''' <param name="idCmd">Identifier de la commande à laquelle se réfère la remarque</param>
    ''' <remarks></remarks>
    Public Sub Insert(ByVal idCmd As Long)
        If (idCmd <> 0) Then
            Dim parameters As New List(Of MySqlParameter)
            Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)

            Try
                'Ouvre la connection
                connection.Open()

                'Défini les paramètres de la requête
                Dim parIdentifierCommande As MySqlParameter = connection.Create("@IdentifierCommande", DbType.Int32, idCmd)
                parameters.Add(parIdentifierCommande)

                Dim parComment As MySqlParameter = connection.Create("@Comment", DbType.String, Me.Comment)
                parameters.Add(parComment)

                Dim parSource As MySqlParameter = connection.Create("@Source", DbType.String, Me.Source)
                parameters.Add(parSource)

                Dim parDate As MySqlParameter = connection.Create("@Date", DbType.String, Me.DatePost)
                parameters.Add(parDate)

                'Requête
                Dim query As String = "INSERT INTO Remarque (Commentaire, Source, Date, IdentifierCommande)" +
                                        " VALUES (@Comment, @Source, @Date, @IdentifierCommande)"

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

        End If

    End Sub

    ''' <summary>
    ''' Permet de supprimer une remarque de la base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Delete()
            Dim parameters As New List(Of MySqlParameter)
            Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)

            Try
                'Ouvre la connection
                connection.Open()

            'Défini les paramètres de la requête
            Dim parIdentifierRemarque As MySqlParameter = connection.Create("@Identifier", DbType.Int64, Me.Identifier)
            parameters.Add(parIdentifierRemarque)

                'Requête
            Dim query As String = "DELETE FROM Remarque WHERE Identifier=@Identifier"

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
    ''' Permet de récupérer toutes les remarques relatives à une commande
    ''' </summary>
    ''' <param name="idCmd">Identifier de la commande</param>
    ''' <returns>Retourne une liste d'objets de la classe Remarque</returns>
    ''' <remarks></remarks>
    Public Shared Function GetRemarques(ByVal idCmd As Long) As List(Of Remarque)
        Dim parameters As New List(Of MySqlParameter)
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim remarques As New List(Of Remarque)
        Dim Objects As New List(Of List(Of Object))

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parIdentifierCommande As MySqlParameter = connection.Create("@IdentifierCommande", DbType.Int32, idCmd)
            parameters.Add(parIdentifierCommande)

            'Requête
            Dim query As String = "SELECT Identifier, Commentaire, Source, Date, IdentifierCommande FROM Remarque WHERE IdentifierCommande=@IdentifierCommande"

            'Exécute la requête
            Objects = connection.ExecuteQuery(query, parameters)

            'Ferme la connection
            connection.Close()

            parameters = Nothing

            'Traite les résultats
            For Each obj In Objects
                remarques.Add(New Remarque(obj(1).ToString(), obj(2).ToString, obj(3).ToString, Long.Parse(obj(0))))
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

        Return remarques
    End Function

#End Region

End Class
