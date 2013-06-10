Imports MySql.Data.MySqlClient
Imports System.Data

Public Class Finalisation

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
        Me.Color = String.Empty
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
    ''' Surcharge de la méthode Equals permettant de comparer deux finalisations
    ''' </summary>
    ''' <param name="obj">Finalisation à comparer</param>
    ''' <returns>Retourne un booléen indiquant si les deux finalisations sont identiques</returns>
    ''' <remarks></remarks>
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (Me IsNot Nothing And obj IsNot Nothing) Then
            If (TypeOf (obj) Is Finalisation) Then
                Dim f As Finalisation = obj
                If (Me.Identifier = f.Identifier And Me.Label = f.Label And Me.Color = f.Color) Then
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
    ''' Permet de récupérer les informations de la finalisation à partir de son identifier
    ''' </summary>
    ''' <returns>Retourne un objet de la classe Finalisation</returns>
    ''' <remarks></remarks>
    Public Function GetFinalisation() As Finalisation
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
            Objects = connection.ExecuteQuery("SELECT Identifier, Label, Couleur, Display FROM Finalisation WHERE Identifier=@Identifier", parameters)

            'Ferme la connection
            connection.Close()
            parameters = Nothing

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
                'Assure la fermeture de la connection
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        Return Me
    End Function

    ''' <summary>
    ''' Permet de récupérer toutes les finalisations dans la base de données
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFinalisations() As List(Of Finalisation)
        Dim finalisations As New List(Of Finalisation)
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))

        Try
            'Ouvre la connection
            connection.Open()

            'Exécute la requête
            Objects = connection.ExecuteQuery("SELECT Identifier, Label, Couleur, Display FROM Finalisation Order By Label")

            'Ferme la connection
            connection.Close()

            'Traite les résultats
            For Each obj In Objects
                finalisations.Add(New Finalisation(obj(1).ToString(), obj(2).ToString(), Boolean.Parse(obj(3)), Long.Parse(obj(0))))
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                'Ferme la requête
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        Return finalisations
    End Function

    ''' <summary>
    ''' Permet de récupérer toutes les finalisations devant être affichées dans le planning dans la base de données
    ''' </summary>
    ''' <returns>Retourne une liste d'objets de la classe Finalisation</returns>
    ''' <remarks></remarks>
    Public Shared Function GetLegendFinalisations() As List(Of Finalisation)
        Dim finalisations As New List(Of Finalisation)
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))

        Try
            'Ouvre la connection
            connection.Open()

            'Exécute la requête
            Objects = connection.ExecuteQuery("SELECT Identifier, Label, Couleur, Display FROM Finalisation WHERE Display=1 Order By Label")

            'Ferme la connection
            connection.Close()

            'Traite les résultats
            For Each obj In Objects
                finalisations.Add(New Finalisation(obj(1).ToString(), obj(2).ToString(), Boolean.Parse(obj(3)), Long.Parse(obj(0))))
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                'Ferme la requête
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        Return finalisations
    End Function

    ''' <summary>
    ''' Permet d'insérer une finalisation en base de données
    ''' </summary>
    ''' <returns>Retourne l'identifier de la finalisation définit par la BDD</returns>
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
            Dim query As String = "INSERT INTO Finalisation (Label, Couleur, Display) VALUES (@Label, @Couleur, @Display)"

            'Exécute la requête
            connection.ExecuteNonQuery(query, parameters)

            'Récupère l'identifier du dernier enregistrement
            Objects = connection.ExecuteQuery("SELECT Max(Identifier) FROM Finalisation")

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
    ''' Permet de mettre à jour une finalisation en base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Update()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection
            connection.Open()

            'Définit les paramètres de la requête
            Dim parIdFinalisation As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdFinalisation)
            Dim parLabel As MySqlParameter = connection.Create("@Label", DbType.String, Me.Label)
            parameters.Add(parLabel)
            Dim parCouleur As MySqlParameter = connection.Create("@Couleur", DbType.String, Me.Color)
            parameters.Add(parCouleur)
            Dim parDisplay As MySqlParameter = connection.Create("@Display", DbType.Boolean, Me.Display)
            parameters.Add(parDisplay)

            'Requête
            Dim query As String = "UPDATE Finalisation SET Label=@Label, Couleur=@Couleur, Display=@Display WHERE Identifier=@Identifier"

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
    ''' Permet de supprimer une finalisation de la base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Delete()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parIdFinalisation As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdFinalisation)

            'Exécute la requête
            connection.ExecuteNonQuery("DELETE FROM Finalisation WHERE Identifier=@Identifier", parameters)

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
    ''' Récupère les couleurs utilisées dans la table Finalisation
    ''' </summary>
    ''' <returns>Retourne une liste de string correspondantes aux couleurs utilisées</returns>
    ''' <remarks></remarks>
    Public Function GetColorsFinalisation() As List(Of String)
        Dim listColorsFinalisation As New List(Of String)
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))

        Try
            connection.Open()

            Objects = connection.ExecuteQuery("SELECT Couleur FROM Finalisation")

            connection.Close()

            For Each obj In Objects
                listColorsFinalisation.Add(obj(0))
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                connection.Close()
            Catch ex As Exception
            End Try
        End Try


        Return listColorsFinalisation
    End Function

    ''' <summary>
    ''' Permet de savoir si une finalisation est utilisée dans une commande
    ''' </summary>
    ''' <returns>Retourne un booléen indiquant si une commande fait référence à la finalisation</returns>
    ''' <remarks></remarks>
    Public Function IsUsed() As Boolean
        Dim bool As Boolean = False
        Dim Objects As New List(Of List(Of Object))
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            connection.Open()

            Dim parIdFinalisation As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdFinalisation)

            Objects = connection.ExecuteQuery("SELECT COUNT(Identifier_Commande) FROM Commande_finalisation WHERE Identifier_Finalisation=@Identifier", parameters)

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
