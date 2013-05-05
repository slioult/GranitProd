Imports MySql.Data.MySqlClient
Imports System.Data

Public Class Contremarque

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
        Me.Nom = String.Empty
        Me.Identifier = 0
    End Sub

    Public Sub New(ByVal identifier As Long)
        Me.Nom = String.Empty
        Me.Identifier = identifier
    End Sub

    Public Sub New(ByVal nom As String, Optional ByVal identifier As Long = 0)
        Me.Nom = nom
        Me.Identifier = identifier
    End Sub


#End Region

#Region "Methods"

    ''' <summary>
    ''' Surcharge de la méthode Equals permettant de comparer deux contremarques
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (Me IsNot Nothing And obj IsNot Nothing) Then
            If (TypeOf (obj) Is Contremarque) Then
                Dim cm As Contremarque = obj
                If (Me.Identifier = cm.Identifier And Me.Nom = cm.Nom) Then
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
    ''' Permet d'insérer une contremarque en base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Insert() As Long
        If (Not Me.Equals(Nothing)) Then

            Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
            Dim parameters As New List(Of MySqlParameter)
            Dim Objects As New List(Of List(Of Object))

            Try
                connection.Open()

                Dim parNom As MySqlParameter = connection.Create("@Nom", DbType.String, Me.Nom)
                parameters.Add(parNom)

                Dim query As String = "INSERT INTO Contremarque (Nom) VALUES (@Nom)"

                connection.ExecuteNonQuery(query, parameters)

                Objects = connection.ExecuteQuery("SELECT Max(Identifier) From Contremarque")

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
        End If

        Return Me.Identifier
    End Function

    ''' <summary>
    ''' Permet de supprimer une contremarque de la base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Delete()
        
    End Sub

    ''' <summary>
    ''' Permet de récupérer les informations d'une contremarque dans la base de données
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetContremarque() As Contremarque
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)
        Dim Objects As New List(Of List(Of Object))

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parIdentifierContremarque As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdentifierContremarque)

            'Exécute la requête
            Objects = connection.ExecuteQuery("SELECT Identifier, Nom FROM Contremarque WHERE Identifier=@Identifier", parameters)

            parameters = Nothing

            'Ferme la requête
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

#End Region

End Class
