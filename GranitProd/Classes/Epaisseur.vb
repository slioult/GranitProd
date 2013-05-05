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
            connection.Open()

            Objects = connection.ExecuteQuery("SELECT Identifier, Value FROM Epaisseur")

            connection.Close()

            For Each obj In Objects
                epaisseurs.Add(New Epaisseur(Integer.Parse(obj(1)), Long.Parse(obj(0))))
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
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
            connection.Open()

            Dim parValue As MySqlParameter = connection.Create("@Value", DbType.Int32, Me.Value)
            parameters.Add(parValue)

            Dim query As String = "INSERT INTO Epaisseur (Value) VALUES (@Value)"

            connection.ExecuteNonQuery(query, parameters)

            Objects = connection.ExecuteQuery("SELECT Max(Identifier) FROM Epaisseur")

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
    ''' Permet de supprimer une épaisseur de la base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Delete()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            connection.Open()

            Dim parIdEpaisseur As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdEpaisseur)

            connection.ExecuteNonQuery("DELETE FROM Epaisseur WHERE Identifier=@Identifier", parameters)

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

#End Region

End Class
