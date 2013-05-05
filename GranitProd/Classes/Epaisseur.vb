Imports MySql.Data.MySqlClient
Imports System.Data

Public Class Epaisseur

#Region "Fields"

    Private _Identifier As Long
    Private _Value As Integer

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


    Public Property Value As Integer
        Get
            Return _Value
        End Get
        Set(ByVal value As Integer)
            Me._Value = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()
    End Sub

    Public Sub New(ByVal value As Integer, Optional ByVal identifier As Long = 0)
        Me.Value = value
        Me.Identifier = identifier
    End Sub


#End Region

#Region "DataAccess"

    ''' <summary>
    ''' Permet de récupérer les épaisseurs dans la base de données
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetEpaisseurs() As List(Of Epaisseur)
        Dim epaisseurs As New List(Of Epaisseur)
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))

        Try
            'Ouvre la connection
            connection.Open()

            'Exécute la requête
            Objects = connection.ExecuteQuery("SELECT Identifier, Value FROM Epaisseur")

            'Ferme la connection
            connection.Close()

            'Traite les résultats
            For Each obj In Objects
                epaisseurs.Add(New Epaisseur(Integer.Parse(obj(1)), Long.Parse(obj(0))))
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

        Return epaisseurs
    End Function

    ''' <summary>
    ''' Permet d'insérer une épaisseur en base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Insert() As Long
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)
        Dim Objects As New List(Of List(Of Object))

        Try
            'ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parValue As MySqlParameter = connection.Create("@Value", DbType.Int32, Me.Value)
            parameters.Add(parValue)

            'Requête
            Dim query As String = "INSERT INTO Epaisseur (Value) VALUES (@Value)"

            'Exécute la requête
            connection.ExecuteNonQuery(query, parameters)

            'Récupre l'identifier du dernier enregistrement
            Objects = connection.ExecuteQuery("SELECT Max(Identifier) FROM Epaisseur")

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
    ''' Permet de supprimer une épaisseur de la base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Delete()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parIdEpaisseur As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdEpaisseur)

            'Exécute la requête
            connection.ExecuteNonQuery("DELETE FROM Epaisseur WHERE Identifier=@Identifier", parameters)

            parameters.Clear()

            'Ferme la connection
            connection.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error")
        Finally
            Try
                'Assure la fermeture de la connction
                connection.Close()
            Catch ex As Exception
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Permet de savoir si une epaisseur est utilisée dans une commande
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

            Dim parValue As MySqlParameter = connection.Create("@Value", DbType.Int32, Me.Value)
            parameters.Add(parValue)

            Objects = connection.ExecuteQuery("SELECT COUNT(Identifier_Commande) FROM Commande_materiau WHERE Epaisseur=@Value", parameters)

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

#End Region

End Class
