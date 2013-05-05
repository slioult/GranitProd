Imports MySql.Data.MySqlClient
Imports System.Data

Public Class Client

#Region "Fields"

    Private _Identifier As Long
    Private _Nom As String

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


    Public Property Nom As String
        Get
            Return _Nom
        End Get
        Set(ByVal value As String)
            Me._Nom = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()
    End Sub

    Public Sub New(ByVal nom As String, Optional ByVal identifier As Long = 0)
        Me.Nom = nom
        Me.Identifier = identifier
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Surcharge de la méthode Equals permettant de comparer un client avec un autre
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (Me IsNot Nothing And obj IsNot Nothing) Then
            If (TypeOf (obj) Is Client) Then
                Dim client As Client = obj
                If (Me.Identifier = client.Identifier And Me.Nom = client.Nom) Then
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
    ''' Permet d'insérer un client en base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Insert() As Long
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)
        Dim Objects As New List(Of List(Of Object))

        Try
            connection.Open()

            Dim parNom As MySqlParameter = connection.Create("@Nom", DbType.String, Me.Nom)
            parameters.Add(parNom)

            Dim query As String = "INSERT INTO Client (Nom) VALUES (@Nom)"

            connection.ExecuteNonQuery(query, parameters)

            Objects = connection.ExecuteQuery("SELECT Max(Identifier) FROM Client")

            For Each obj In Objects
                Me.Identifier = Long.Parse(obj(0))
            Next

            parameters = Nothing

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        Return Me.Identifier
    End Function

    ''' <summary>
    ''' Permet de supprimer un client de la base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Delete()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            connection.Open()

            Dim parIdClient As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdClient)

            connection.ExecuteNonQuery("DELETE FROM Client WHERE Identifier=@Identifier", parameters)

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
    End Sub

    ''' <summary>
    ''' Permet de récupérer les informations d'un client dans la base de données
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetClient() As Client
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)
        Dim Objects As New List(Of List(Of Object))

        Try
            connection.Open()

            Dim parIdentifierClient As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdentifierClient)

            Objects = connection.ExecuteQuery("SELECT Identifier, Nom FROM Client WHERE Identifier=@Identifier", parameters)

            parameters = Nothing

            connection.Close()

            For Each obj In Objects
                Me.Nom = obj(1).ToString()
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        Return Me
    End Function

    ''' <summary>
    ''' Permet de récupérer la liste de tous les clients contenus dans la base de données
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetClients() As List(Of Client)
        Dim clients As New List(Of Client)

        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))

        Try
            connection.Open()

            Objects = connection.ExecuteQuery("SELECT Identifier, Nom FROM Client")

            connection.Close()

            For Each obj In Objects
                clients.Add(New Client(Me.Nom = obj(1).ToString(), Long.Parse(obj(0))))
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        Return clients
    End Function

#End Region

End Class
