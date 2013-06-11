Imports MySql.Data.MySqlClient
Imports System.Data

Public Class Etat

#Region "Fields"

    Private _Identifier As Long
    Private _Label As String
    Private _Position As Integer

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


    Public Property Label As String
        Get
            Return _Label
        End Get
        Set(ByVal value As String)
            Me._Label = value
        End Set
    End Property

    Public Property Position As Integer
        Get
            Return Me._Position
        End Get

        Set(ByVal value As Integer)
            Me._Position = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()
        Me.Label = String.Empty
        Me.Identifier = 0
    End Sub

    Public Sub New(ByVal label As String, Optional ByVal position As Integer = 0, Optional ByVal identifier As Long = 0)
        Me.Label = label
        Me.Identifier = identifier
        Me.Position = position
    End Sub


#End Region

#Region "Methods"

    ''' <summary>
    ''' Surcharge de la méthode Equals permettant de comparer un deux états
    ''' </summary>
    ''' <param name="obj">État à comparer</param>
    ''' <returns>Retourne un booléen indiquant si les deux états sont identiques</returns>
    ''' <remarks></remarks>
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (Me IsNot Nothing And obj IsNot Nothing) Then
            If (TypeOf (obj) Is Etat) Then
                Dim e As Etat = obj
                If (Me.Identifier = e.Identifier And Me.Label = e.Label) Then
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
    ''' Permet de récupérer les informations d'un état dans la base de données à partir de son identifier
    ''' </summary>
    ''' <returns>Retourne un objet de la classe Etat</returns>
    ''' <remarks></remarks>
    Public Function GetEtat() As Etat
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)
        Dim Objects As New List(Of List(Of Object))

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parIdentifierEtat As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdentifierEtat)

            'Exécute la requête
            Objects = connection.ExecuteQuery("SELECT Identifier, Label, Position FROM Etat WHERE Identifier=@Identifier", parameters)

            parameters = Nothing

            'Ferme la connection
            connection.Close()

            'Traite les résultats
            For Each obj In Objects
                Me.Label = obj(1).ToString()
                Me.Position = Integer.Parse(obj(2))
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

        Return Me
    End Function

    ''' <summary>
    ''' Permet de récupérer tous les états dans la base de données
    ''' </summary>
    ''' <returns>Retourne une liste d'objets de la classe Etat</returns>
    ''' <remarks></remarks>
    Public Shared Function GetEtats() As List(Of Etat)
        Dim etats As New List(Of Etat)
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))

        Try
            'Ouvre la connection
            connection.Open()

            'Exécute la requête
            Objects = connection.ExecuteQuery("SELECT Identifier, Label, Position FROM Etat Order By Position")

            'Ferme la conection
            connection.Close()

            'Traite les résultats
            For Each obj In Objects
                etats.Add(New Etat(obj(1).ToString(), Integer.Parse(obj(2)), Long.Parse(obj(0))))
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

        Return etats
    End Function

    ''' <summary>
    ''' Permet d'insérer un état en base de données
    ''' </summary>
    ''' <returns>Retourne son identifier définit par la BDD</returns>
    ''' <remarks></remarks>
    Public Function Insert() As Long
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)
        Dim Objects As New List(Of List(Of Object))

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parLabel As MySqlParameter = connection.Create("@Label", DbType.String, Me.Label)
            parameters.Add(parLabel)

            Dim parPosition As MySqlParameter = connection.Create("@Position", DbType.String, Me.Position)
            parameters.Add(parPosition)

            'Requête
            Dim query As String = "INSERT INTO Etat (Label, Position) VALUES (@Label, @Position)"

            'Exécute la requête
            connection.ExecuteNonQuery(query, parameters)

            'Récupère l'identifier du dernier enregistrement
            Objects = connection.ExecuteQuery("SELECT Max(Identifier) FROM Etat")

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
    ''' Permet de mettre à jour un état en base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Update()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parIdEtat As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdEtat)
            Dim parLabel As MySqlParameter = connection.Create("@Label", DbType.String, Me.Label)
            parameters.Add(parLabel)
            Dim parPosition As MySqlParameter = connection.Create("@Position", DbType.Int64, Me.Position)
            parameters.Add(parPosition)

            'Requête
            Dim query As String = "UPDATE Etat SET Label=@Label, Position=@Position WHERE Identifier=@Identifier"

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
    ''' Permet de supprimer un état de la base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Delete()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parIdEtat As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdEtat)

            'Exécute la requête
            connection.ExecuteNonQuery("DELETE FROM Etat WHERE Identifier=@Identifier", parameters)

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
    ''' Permet de savoir si un état est utilisé dans une commande
    ''' </summary>
    ''' <returns>Retourne un booléen indiquant si l'état est utilisé</returns>
    ''' <remarks></remarks>
    Public Function IsUsed() As Boolean
        Dim bool As Boolean = False
        Dim Objects As New List(Of List(Of Object))
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            connection.Open()

            Dim parIdEtat As MySqlParameter = connection.Create("@IdentifierEtat", DbType.Int32, Me.Identifier)
            parameters.Add(parIdEtat)

            Objects = connection.ExecuteQuery("SELECT COUNT(Identifier) FROM Commande WHERE IdentifierEtat=@IdentifierEtat", parameters)

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
