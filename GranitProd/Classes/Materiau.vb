Imports MySql.Data.MySqlClient
Imports System.Data

Public Class Materiau

#Region "Fields"

    Private _Identifier As Long
    Private _Label As String
    Private _Epaisseur As Integer

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

    Public Property Epaisseur As Integer
        Get
            Return Me._Epaisseur
        End Get

        Set(ByVal value As Integer)
            Me._Epaisseur = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()
    End Sub


    Public Sub New(ByVal label As String, Optional ByVal identifier As Long = 0, Optional ByVal epaisseur As Integer = Nothing)
        Me.Label = label
        Me.Epaisseur = epaisseur
        Me.Identifier = identifier
    End Sub


#End Region

#Region "Methods"

    ''' <summary>
    ''' Surcharge de la méthode Equals permettant de comparer deux materiaux
    ''' </summary>
    ''' <param name="obj">Matériau à comparer</param>
    ''' <returns>Retourne un booléen indiquant si les deux matéiaux sont identiques</returns>
    ''' <remarks></remarks>
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (Me IsNot Nothing And obj IsNot Nothing) Then
            If (TypeOf (obj) Is Materiau) Then
                Dim m As Materiau = obj
                If (Me.Identifier = m.Identifier And Me.Label = m.Label And Me.Epaisseur = m.Epaisseur) Then
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
    ''' Permet de récupérer les informations du matériau à partir de son identifier
    ''' </summary>
    ''' <returns>Retourne un objet de la classe Materiau</returns>
    ''' <remarks></remarks>
    Public Function GetMateriau() As Materiau
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
            Objects = connection.ExecuteQuery("SELECT Identifier, Label FROM Materiau WHERE Identifier=@Identifier", parameters)

            'Ferme la connection
            connection.Close()
            parameters = Nothing

            'Traite les résultats
            For Each obj In Objects
                Me.Label = obj(1).ToString()
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
    ''' Permet de récupérer les matériaux dans la base de données
    ''' </summary>
    ''' <returns>Retourne une liste d'objets de la classe Materiau</returns>
    ''' <remarks></remarks>
    Public Shared Function GetMateriaux() As List(Of Materiau)
        Dim materiaux As New List(Of Materiau)
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))

        Try
            'Ouvre la connection
            connection.Open()

            'Exécute la requête
            Objects = connection.ExecuteQuery("SELECT Identifier, Label FROM Materiau Order By Label")

            'Ferme la connection
            connection.Close()

            'Traite les résultats
            For Each obj In Objects
                materiaux.Add(New Materiau(obj(1).ToString(), Long.Parse(obj(0))))
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                'Ferme la conneciton
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        Return materiaux
    End Function

    ''' <summary>
    ''' Permet d'insérer un matériau en base de données
    ''' </summary>
    ''' <returns>Retourne l'identifier du matériau définit par la BDD</returns>
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

            'Requête
            Dim query As String = "INSERT INTO Materiau (Label) VALUES (@Label)"

            'Exécute la requête
            connection.ExecuteNonQuery(query, parameters)

            'Récupère l'identifier du dernier enregistrement
            Objects = connection.ExecuteQuery("SELECT Max(Identifier) FROM Materiau")

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
    ''' Permet de mettre à jour un matériau en base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Update()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parIdMateriau As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdMateriau)

            Dim parLabel As MySqlParameter = connection.Create("@Label", DbType.String, Me.Label)
            parameters.Add(parLabel)

            'Requête
            Dim query As String = "UPDATE Materiau SET Label=@Label WHERE Identifier=@Identifier"

            'Exécution de la requête
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
    ''' Permet de supprimer un matériau de la base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Delete()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parIdMateriau As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdMateriau)

            'Exécute la requête
            connection.ExecuteNonQuery("DELETE FROM Materiau WHERE Identifier=@Identifier", parameters)

            parameters.Clear()

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
    ''' Permet de savoir si un materiau est utilisé dans une commande
    ''' </summary>
    ''' <returns>Retourne un booléen indiquant si une commande fait référence au matériau</returns>
    ''' <remarks></remarks>
    Public Function IsUsed() As Boolean
        Dim bool As Boolean = False
        Dim Objects As New List(Of List(Of Object))
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            connection.Open()

            Dim parIdMateriau As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdMateriau)

            Objects = connection.ExecuteQuery("SELECT COUNT(Identifier_Commande) FROM Commande_materiau WHERE Identifier_Materiau=@Identifier", parameters)

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
