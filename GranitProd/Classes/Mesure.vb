Imports MySql.Data.MySqlClient
Imports System.Data

Public Class Mesure

#Region "Fields"

    Private _Identifier As Long
    Private _Label As String
    Private _Color As String
    Private _Display As Boolean

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

    Public Property Color As String
        Get
            Return Me._Color
        End Get
        Set(ByVal value As String)
            Me._Color = value
        End Set
    End Property

    Public Property Display As Boolean
        Get
            Return Me._Display
        End Get
        Set(ByVal value As Boolean)
            Me._Display = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()
        Me.Label = String.Empty
        Me.Identifier = 0
    End Sub

    Public Sub New(ByVal identifier As Long)
        Me.Identifier = identifier
    End Sub

    Public Sub New(ByVal label As String, Optional ByVal color As String = "", Optional ByVal display As Boolean = False, Optional ByVal identifier As Long = 0)
        Me.Label = label
        Me.Color = color
        Me.Display = display
        Me.Identifier = identifier
    End Sub


#End Region

#Region "Methods"

    ''' <summary>
    ''' Surcharge de la méthode Equals permettant de comparer deux Mesures
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (Me IsNot Nothing And obj IsNot Nothing) Then
            If (TypeOf (obj) Is Mesure) Then
                Dim m As Mesure = obj
                If (Me.Identifier = m.Identifier And Me.Label = m.Label And Me.Color = m.Color) Then
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
    ''' Permet de récupérer les informations d'une mesure dans la base de données
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMesure() As Mesure
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)
        Dim Objects As New List(Of List(Of Object))

        Try
            'ouvre la requête
            connection.Open()

            'Défini les paramètres de la requêtes
            Dim parIdentifierMesure As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdentifierMesure)

            'Exécute la requête
            Objects = connection.ExecuteQuery("SELECT Identifier, Label, Couleur, Display FROM Mesure WHERE Identifier=@Identifier", parameters)

            parameters = Nothing

            'Ferme la requête
            connection.Close()

            'Traite les résultats
            For Each obj In Objects
                Me.Label = obj(1).ToString()
                Me.Color = obj(2).ToString()
                Me.Display = Boolean.Parse(obj(3))
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                'Assure la fermeture de la requête
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        Return Me
    End Function

    ''' <summary>
    ''' Permet de récupérer tous les type de mesure dans la base de données
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetMesures() As List(Of Mesure)
        Dim mesures As New List(Of Mesure)
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))

        Try
            'Ouvre la connection
            connection.Open()

            'Exécute la requête
            Objects = connection.ExecuteQuery("SELECT Identifier, Label, Couleur, Display FROM Mesure Order By Label")

            'Ferme la connection
            connection.Close()

            'Traite les résultats
            For Each obj In Objects
                mesures.Add(New Mesure(obj(1).ToString(), obj(2).ToString(), Boolean.Parse(obj(3)), Long.Parse(obj(0))))
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                'Assure la fermeture de la requête
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        Return mesures
    End Function

    ''' <summary>
    ''' Permet de récupérer tous les type de mesure devant être affichés dans le planning dans la base de données
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetLegendMesures() As List(Of Mesure)
        Dim mesures As New List(Of Mesure)
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))

        Try
            'Ouvre la connection
            connection.Open()

            'Exécute la requête
            Objects = connection.ExecuteQuery("SELECT Identifier, Label, Couleur, Display FROM Mesure WHERE Display=1 Order By Label")

            'Ferme la connection
            connection.Close()

            'Traite les résultats
            For Each obj In Objects
                mesures.Add(New Mesure(obj(1).ToString(), obj(2).ToString(), Boolean.Parse(obj(3)), Long.Parse(obj(0))))
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                'Assure la fermeture de la requête
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        Return mesures
    End Function

    ''' <summary>
    ''' Permet d'insérer une Mesure en base de données
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
            Dim parLabel As MySqlParameter = connection.Create("@Label", DbType.String, Me.Label)
            parameters.Add(parLabel)
            Dim parCouleur As MySqlParameter = connection.Create("@Couleur", DbType.String, Me.Color)
            parameters.Add(parCouleur)
            Dim parDisplay As MySqlParameter = connection.Create("@Display", DbType.Boolean, Me.Display)
            parameters.Add(parDisplay)

            'Requête
            Dim query As String = "INSERT INTO Mesure (Label, Couleur, Display) VALUES (@Label, @Couleur, @Display)"

            'Exécute la requête
            connection.ExecuteNonQuery(query, parameters)

            'Récupère l'identifier du dernier enregistrement
            Objects = connection.ExecuteQuery("SELECT Max(Identifier) FROM Mesure")

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
    ''' Permet de mettre à jour une Mesure en base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Update()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parIdMesure As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdMesure)
            Dim parLabel As MySqlParameter = connection.Create("@Label", DbType.String, Me.Label)
            parameters.Add(parLabel)
            Dim parCouleur As MySqlParameter = connection.Create("@Couleur", DbType.String, Me.Color)
            parameters.Add(parCouleur)
            Dim parDisplay As MySqlParameter = connection.Create("@Display", DbType.Boolean, Me.Display)
            parameters.Add(parDisplay)

            'Requête
            Dim query As String = "UPDATE Mesure SET Label=@Label, Couleur=@Couleur, Display=@Display WHERE Identifier=@Identifier"

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
    ''' Permet de supprimer une Mesure de la base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Delete()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la requête
            connection.Open()

            'Défini les paramètres de la requête
            Dim parIdMesure As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdMesure)

            'Exécute la requête
            connection.ExecuteNonQuery("DELETE FROM Mesure WHERE Identifier=@Identifier", parameters)

            parameters = Nothing

            'Ferme la requête
            connection.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error")
        Finally
            Try
                'Ferme la requête
                connection.Close()
            Catch ex As Exception
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Permet de récupérer les couleurs utilisées dans las table Mesure
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetColorsReleves() As List(Of String)
        Dim listColorsReleves As New List(Of String)
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))

        Try
            connection.Open()

            Objects = connection.ExecuteQuery("SELECT Couleur FROM Mesure")

            connection.Close()

            For Each obj In Objects
                listColorsReleves.Add(obj(0))
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                connection.Close()
            Catch ex As Exception
            End Try
        End Try


        Return listColorsReleves
    End Function

    ''' <summary>
    ''' Permet de savoir si une mesure est utiliser dans une commande
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

            Dim parIdMesure As MySqlParameter = connection.Create("@IdentifierMesure", DbType.Int32, Me.Identifier)
            parameters.Add(parIdMesure)

            Objects = connection.ExecuteQuery("SELECT COUNT(Identifier) FROM Commande WHERE IdentifierMesure=@IdentifierMesure", parameters)

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
