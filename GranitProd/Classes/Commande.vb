Imports MySql.Data.MySqlClient
Imports System.Data
Imports MGranitDALcsharp

Public Class Commande

#Region "Fields"

    Private _Identifier As Long
    Private _NoCommande As Integer
    Private _MontantHT As Decimal
    Private _Arrhes As Decimal
    Private _DateCommande As DateTime
    Private _AdresseChantier As String
    Private _TpsDebit As Integer
    Private _TpsCommandeNumerique As Integer
    Private _TpsFinition As Integer
    Private _TpsAutres As Integer
    Private _DelaiPrevu As DateTime
    Private _Etat As Etat
    Private _Client As Client
    Private _Contremarque As Contremarque
    Private _Mesure As Mesure
    Private _DateMesure As DateTime
    Private _Materiaux As List(Of Materiau)
    Private _Natures As List(Of Nature)
    Private _Finalisations As List(Of Finalisation)
    Private _DateFinalisations As DateTime
    Private _Remarques As List(Of Remarque)
    Private _Qualites As List(Of Qualite)
    Private _Flag As Long

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

    Public Property NoCommande As Integer
        Get
            Return _NoCommande
        End Get
        Set(ByVal value As Integer)
            Me._NoCommande = value
        End Set
    End Property

    Public Property MontantHT As Decimal
        Get
            Return FormatNumber(_MontantHT, 2)
        End Get
        Set(ByVal value As Decimal)
            Me._MontantHT = FormatNumber(value, 2)
        End Set
    End Property

    Public Property Arrhes As Decimal
        Get
            Return FormatNumber(_Arrhes, 2)
        End Get
        Set(ByVal value As Decimal)
            Me._Arrhes = FormatNumber(value, 2)
        End Set
    End Property

    Public Property DateCommande As DateTime
        Get
            Return _DateCommande
        End Get
        Set(ByVal value As DateTime)
            Me._DateCommande = value
        End Set
    End Property

    Public Property AdresseChantier As String
        Get
            Return _AdresseChantier
        End Get
        Set(ByVal value As String)
            Me._AdresseChantier = value
        End Set
    End Property

    Public Property TpsDebit As Integer
        Get
            Return Me._TpsDebit
        End Get

        Set(ByVal value As Integer)
            Me._TpsDebit = value
        End Set
    End Property

    Public Property TpsCommandeNumerique As Integer
        Get
            Return Me._TpsCommandeNumerique
        End Get

        Set(ByVal value As Integer)
            Me._TpsCommandeNumerique = value
        End Set
    End Property

    Public Property TpsFinition As Integer
        Get
            Return Me._TpsFinition
        End Get

        Set(ByVal value As Integer)
            Me._TpsFinition = value
        End Set
    End Property

    Public Property TpsAutres As Integer
        Get
            Return Me._TpsAutres
        End Get

        Set(ByVal value As Integer)
            Me._TpsAutres = value
        End Set
    End Property

    Public Property DelaiPrevu As DateTime
        Get
            Return Me._DelaiPrevu
        End Get
        Set(ByVal value As DateTime)
            Me._DelaiPrevu = value
        End Set
    End Property

    Public Property Etat As Etat
        Get
            Return Me._Etat
        End Get

        Set(ByVal value As Etat)
            Me._Etat = value
        End Set
    End Property

    Public Property Client As Client
        Get
            Return Me._Client
        End Get

        Set(ByVal value As Client)
            Me._Client = value
        End Set
    End Property

    Public Property Contremarque As Contremarque
        Get
            Return Me._Contremarque
        End Get

        Set(ByVal value As Contremarque)
            Me._Contremarque = value
        End Set
    End Property

    Public Property Mesure As Mesure
        Get
            Return Me._Mesure
        End Get

        Set(ByVal value As Mesure)
            Me._Mesure = value
        End Set
    End Property

    Public Property DateMesure As DateTime
        Get
            Return _DateMesure
        End Get
        Set(ByVal value As DateTime)
            Me._DateMesure = value
        End Set
    End Property

    Public Property Materiaux As List(Of Materiau)
        Get
            Return Me._Materiaux
        End Get

        Set(ByVal value As List(Of Materiau))
            Me._Materiaux = value
        End Set
    End Property

    Public Property Natures As List(Of Nature)
        Get
            Return Me._Natures
        End Get

        Set(ByVal value As List(Of Nature))
            Me._Natures = value
        End Set
    End Property

    Public Property Finalisations As List(Of Finalisation)
        Get
            Return Me._Finalisations
        End Get

        Set(ByVal value As List(Of Finalisation))
            Me._Finalisations = value
        End Set
    End Property

    Public Property DateFinalisations As DateTime
        Get
            Return _DateFinalisations
        End Get
        Set(ByVal value As DateTime)
            Me._DateFinalisations = value
        End Set
    End Property

    Public Property Remarques As List(Of Remarque)
        Get
            Return Me._Remarques
        End Get

        Set(ByVal value As List(Of Remarque))
            Me._Remarques = value
        End Set
    End Property

    Public Property Qualites As List(Of Qualite)
        Get
            Return Me._Qualites
        End Get

        Set(ByVal value As List(Of Qualite))
            Me._Qualites = value
        End Set
    End Property

    Public Property Flag As Long
        Get
            Return Me._Flag
        End Get
        Set(ByVal value As Long)
            Me._Flag = value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
        Me.Materiaux = New List(Of Materiau)
        Me.Natures = New List(Of Nature)
        Me.Finalisations = New List(Of Finalisation)
    End Sub

    Public Sub New(ByVal numeroCommande As Integer)
        Me.Materiaux = New List(Of Materiau)
        Me.Natures = New List(Of Nature)
        Me.Finalisations = New List(Of Finalisation)
        Me.NoCommande = numeroCommande
    End Sub

    Public Sub New(ByVal numeroCommande As Integer, ByVal flag As Long)
        Me.Materiaux = New List(Of Materiau)
        Me.Natures = New List(Of Nature)
        Me.Finalisations = New List(Of Finalisation)
        Me.NoCommande = numeroCommande
        Me.Flag = flag
    End Sub

    Public Sub New(ByVal noCommande As Integer, ByVal montant As Decimal, ByVal arrhes As Decimal, ByVal dateCommande As DateTime, ByVal adresseChantier As String,
                   ByVal etat As Etat, ByVal client As Client, Optional ByVal tpsDebit As Integer = 0, Optional ByVal tpsCommandeNumerique As Integer = 0,
                   Optional ByVal tpsFinition As Integer = 0, Optional ByVal tpsAutres As Integer = 0, Optional ByVal delaiPrevu As DateTime = Nothing,
                   Optional ByVal mesure As Mesure = Nothing, Optional ByVal dateMesure As DateTime = Nothing, Optional ByVal contremarque As Contremarque = Nothing,
                   Optional ByVal materiaux As List(Of Materiau) = Nothing, Optional ByVal natures As List(Of Nature) = Nothing,
                   Optional ByVal dateFinalisations As DateTime = Nothing, Optional ByVal finalisations As List(Of Finalisation) = Nothing,
                   Optional ByVal remarques As List(Of Remarque) = Nothing, Optional ByVal qualites As List(Of Qualite) = Nothing, Optional ByVal identifier As Long = 0)

        Me.Identifier = identifier
        Me.NoCommande = noCommande
        Me.MontantHT = FormatNumber(montant, 2)
        Me.Arrhes = FormatNumber(arrhes, 2)
        Me.DateCommande = dateCommande
        Me.AdresseChantier = adresseChantier
        Me.TpsDebit = tpsDebit
        Me.TpsCommandeNumerique = tpsCommandeNumerique
        Me.TpsFinition = tpsFinition
        Me.TpsAutres = tpsAutres
        Me.DelaiPrevu = delaiPrevu
        Me.Etat = etat
        Me.Client = client
        Me.Contremarque = contremarque
        Me.Mesure = mesure
        Me.DateMesure = dateMesure
        Me.Materiaux = materiaux
        Me.Natures = natures
        Me.Finalisations = finalisations
        Me.DateFinalisations = dateFinalisations
        Me.Remarques = remarques
        Me.Qualites = qualites

        If Me.Materiaux Is Nothing Then Me.Materiaux = New List(Of Materiau)
        If Me.Natures Is Nothing Then Me.Natures = New List(Of Nature)
        If Me.Finalisations Is Nothing Then Me.Finalisations = New List(Of Finalisation)
    End Sub
#End Region

#Region "Methods"

    ''' <summary>
    ''' Permet d'ajouter une commande
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Add()

        Try
            'Vérifie si le client existe, sinon, l'ajoute dans la DB
            If (Me.Client.Identifier = 0) Then
                Me.Client.Identifier = Me.Client.Insert()
            End If

            'Vérifie si la contremarque existe, sinon, l'ajoute dans la DB
            If (Me.Contremarque IsNot Nothing AndAlso Me.Contremarque.Identifier = 0) Then
                Me.Contremarque.Identifier = Me.Contremarque.Insert()
            ElseIf Me.Contremarque Is Nothing Then
                Me.Contremarque = New Contremarque()
            End If

            'Insert la commande en base de données
            Me.Identifier = Me.Insert()

            'Vérifie si la remarque existe, sinon, l'ajoute dans la DB
            For Each Rm In Me.Remarques
                If (Rm.Identifier = 0) Then
                    Rm.Insert(Me.Identifier)
                End If
            Next

            'Vérifie si le problème de qualite existe, sinon, l'ajoute dans la DB
            For Each q In Me.Qualites
                q.UpdateQualitiesProblems(Me.Identifier)
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try

    End Sub

#End Region

#Region "DataAccess"

    ''' <summary>
    ''' Permet l'insertion d'une commande en base de données
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Insert() As Long
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            ' Ouvre la connexion à la base de données
            connection.Open()

            ' Initialise les paramètres de la requête
            Dim parNumCommande As MySqlParameter = connection.Create("@NumCommande", DbType.Int32, Me.NoCommande)
            parameters.Add(parNumCommande)

            Dim parMontant As MySqlParameter = connection.Create("@Montant", DbType.Decimal, Me.MontantHT)
            parameters.Add(parMontant)

            Dim parArrhes As MySqlParameter = connection.Create("@Arrhes", DbType.Decimal, Me.Arrhes)
            parameters.Add(parArrhes)

            Dim parDateCommande As MySqlParameter = connection.Create("@DateCommande", DbType.DateTime, Me.DateCommande)
            parameters.Add(parDateCommande)

            Dim parAdresse As MySqlParameter = connection.Create("@Adresse", DbType.String, Me.AdresseChantier)
            parameters.Add(parAdresse)

            Dim parTpsDebit As MySqlParameter = connection.Create("@TpsDebit", DbType.Int32, Me.TpsDebit)
            parameters.Add(parTpsDebit)

            Dim parTpsCommandeNumerique As MySqlParameter = connection.Create("@TpsCmdNum", DbType.Int32, Me.TpsCommandeNumerique)
            parameters.Add(parTpsCommandeNumerique)

            Dim parTpsFinition As MySqlParameter = connection.Create("@TpsFinition", DbType.Int32, Me.TpsFinition)
            parameters.Add(parTpsFinition)

            Dim parTpsAutres As MySqlParameter = connection.Create("@TpsAutres", DbType.Int32, Me.TpsAutres)
            parameters.Add(parTpsAutres)

            Dim parDelaiPrevu As MySqlParameter = connection.Create("@DelaiPrevu", DbType.DateTime, Me.DelaiPrevu)
            parameters.Add(parDelaiPrevu)

            Dim parIdentifierEtat As MySqlParameter = connection.Create("@IdentifierEtat", DbType.Int32, Me.Etat.Identifier)
            parameters.Add(parIdentifierEtat)

            Dim parIdentifierClient As MySqlParameter = connection.Create("@IdentifierClient", DbType.Int32, Me.Client.Identifier)
            parameters.Add(parIdentifierClient)

            Dim parIdentifierContremarque As MySqlParameter = connection.Create("@IdentifierContremarque", DbType.Int32, Me.Contremarque.Identifier)
            parameters.Add(parIdentifierContremarque)

            Dim parIdentifierMesure As MySqlParameter = connection.Create("@IdentifierMesure", DbType.Int32, Me.Mesure.Identifier)
            parameters.Add(parIdentifierMesure)

            'Autorise la valeur NULL pour la date de mesure
            Dim parDateMesure As MySqlParameter
            If Me.DateMesure <> DateTime.MinValue Then
                parDateMesure = connection.Create("@DateMesure", DbType.DateTime, Me.DateMesure)
            Else
                parDateMesure = connection.Create("@DateMesure", DbType.DateTime, Nothing)
            End If
            parameters.Add(parDateMesure)

            Dim parDateFinalisations As MySqlParameter = connection.Create("@DateFinalisations", DbType.DateTime, Me.DateFinalisations)
            parameters.Add(parDateFinalisations)

            'requête
            Dim query As String = "INSERT INTO Commande (NumCmd, Montant, Arrhes, DateCommande, AdresseChantier, TpsDebit, TpsCmdNumerique, TpsFinition" + _
                ", TpsAutres, DelaiPrevu, IdentifierEtat, IdentifierClient, IdentifierContremarque, IdentifierMesure, DateMesure, DateFinalisations)" + _
                                    " VALUES (@NumCommande, @Montant, @Arrhes, @DateCommande, @Adresse, @TpsDebit, @TpsCmdNum, @TpsFinition, @TpsAutres" + _
                                    ", @DelaiPrevu, @IdentifierEtat, @IdentifierClient, @IdentifierContremarque, @IdentifierMesure, @DateMesure, @DateFinalisations)"

            'Exécute la requête
            connection.ExecuteNonQuery(query, parameters)

            'Récupère l'identifier du dernier enregistrement
            Dim Objects As New List(Of List(Of Object))
            Objects = connection.ExecuteQuery("SELECT Max(Identifier) FROM Commande")

            For Each obj In Objects
                Me.Identifier = Long.Parse(obj(0))
            Next

            Dim parIdentifierCommande As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdentifierCommande)

            parameters.Clear()

            'Insert les liaisons entre commande et matériaux
            For Each mat In Me.Materiaux
                Dim parIdentifierMateriau As MySqlParameter = connection.Create("@IdMateriau", DbType.Int32, mat.Identifier)
                parameters.Add(parIdentifierMateriau)

                Dim parEpaisseur As MySqlParameter = connection.Create("@Epaisseur", DbType.Int32, mat.Epaisseur)
                parameters.Add(parEpaisseur)

                parameters.Add(parIdentifierCommande)

                query = "INSERT INTO Commande_Materiau (Identifier_Commande, Identifier_Materiau, Epaisseur)" +
                        " VALUES (@Identifier, @IdMateriau, @Epaisseur)"

                connection.ExecuteNonQuery(query, parameters)

                parameters.Clear()
            Next

            'Insert les liaisons entre les natures et les commandes
            For Each nat In Me.Natures
                Dim parIdentifierNature As MySqlParameter = connection.Create("@IdNature", DbType.Int32, nat.Identifier)
                parameters.Add(parIdentifierNature)

                parameters.Add(parIdentifierCommande)

                query = "INSERT INTO Commande_Nature (Identifier_Commande, Identifier_Nature)" +
                        " VALUES (@Identifier, @IdNature)"

                connection.ExecuteNonQuery(query, parameters)

                parameters.Clear()
            Next

            'Insert les liaisons entre les commandes et les prestations
            For Each fin In Me.Finalisations
                Dim parIdentifierFinalisation As MySqlParameter = connection.Create("@IdFinalisation", DbType.Int32, fin.Identifier)
                parameters.Add(parIdentifierFinalisation)

                parameters.Add(parIdentifierCommande)

                query = "INSERT INTO Commande_Finalisation (Identifier_Commande, Identifier_Finalisation)" +
                        " VALUES (@Identifier, @IdFinalisation)"

                connection.ExecuteNonQuery(query, parameters)

                parameters.Clear()
            Next

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
    ''' Permet la suppression d'une commande
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Delete()
        Dim parameters As New List(Of MySqlParameter)
        Dim connection As New MGConnection(My.Settings.DBSource)

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parIdCommande As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdCommande)

            'Requête supprimant tous les liens relatifs à la commande
            Dim query As String = "DELETE FROM Commande_Materiau WHERE Identifier_Commande = @Identifier;" +
                                  "DELETE FROM Commande_Finalisation WHERE Identifier_Commande = @Identifier;" +
                                  "DELETE FROM Commande_Nature WHERE Identifier_Commande = @Identifier;" +
                                  "DELETE FROM Remarque WHERE IdentifierCommande=@Identifier;" +
                                  "DELETE FROM Commande_Qualite WHERE Identifier_Commande=@Identifier;" +
                                  "DELETE FROM Commande WHERE Identifier=@Identifier;" +
                                  "DELETE FROM Client WHERE (SELECT count(c.Identifier) FROM Commande as c WHERE IdentifierClient=Client.Identifier) = 0;" +
                                  "DELETE FROM Contremarque WHERE (SELECT count(c.Identifier) FROM Commande as c WHERE IdentifierContremarque=Contremarque.Identifier) = 0"

            'Exécute la requête
            connection.ExecuteNonQuery(query, parameters)

            parameters.Clear()

            'Ferme la connection
            connection.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error")
        Finally
            Try
                connection.Close()
            Catch
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Permet de récupérer toutes les informations concernant une commande
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCommande() As Commande
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))
        Dim tempObjects As New List(Of List(Of Object))
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection à la base de données
            connection.Open()

            'Initialise les paramètres de la commande
            Dim parNumeroCommande As MySqlParameter = connection.Create("@NumCommande", DbType.Int32, Me.NoCommande)
            parameters.Add(parNumeroCommande)

            'Requête
            Objects = connection.ExecuteQuery("SELECT Identifier, NumCmd, Montant, Arrhes, DateCommande, AdresseChantier, TpsDebit, TpsCmdNumerique, TpsFinition, TpsAutres," +
                                              "DelaiPrevu, IdentifierEtat, IdentifierClient, IdentifierContremarque, IdentifierMesure, DateMesure, DateFinalisations" +
                                              " FROM Commande" +
                                              " WHERE NumCmd=@NumCommande", parameters)

            'Traite les résultats de la requête
            For Each obj In Objects

                Me.Identifier = Long.Parse(obj(0))
                Me.NoCommande = Integer.Parse(obj(1))
                Me.MontantHT = Decimal.Parse(obj(2))
                Me.Arrhes = Decimal.Parse(obj(3))
                Me.DateCommande = DateTime.Parse(obj(4))
                Me.AdresseChantier = obj(5).ToString()
                Me.TpsDebit = Integer.Parse(obj(6))
                Me.TpsCommandeNumerique = Integer.Parse(obj(7))
                Me.TpsFinition = Integer.Parse(obj(8))
                Me.TpsAutres = Integer.Parse(obj(9))
                Me.DelaiPrevu = DateTime.Parse(obj(10))
                Dim ob As Object = obj(15)
                If Not TypeOf (obj(15)) Is System.DBNull Then
                    Me.DateMesure = DateTime.Parse(obj(15))
                Else
                    Me.DateMesure = Nothing
                End If
                Me.DateFinalisations = DateTime.Parse(obj(16))

                Dim parIdentifierEtat As MySqlParameter = connection.Create("@IdentifierEtat", DbType.Int32, Long.Parse(obj(11)))
                parameters.Add(parIdentifierEtat)

                tempObjects = connection.ExecuteQuery("SELECT Identifier, Label, Position FROM Etat WHERE Identifier=@IdentifierEtat", parameters)
                For Each tmpObj In tempObjects
                    Me.Etat = New Etat(tmpObj(1).ToString(), Integer.Parse(tmpObj(2)), Long.Parse(tmpObj(0)))
                Next
                parameters.Clear()
                parIdentifierEtat = Nothing


                Dim parIdentifierClient As MySqlParameter = connection.Create("@IdentifierClient", DbType.Int32, Long.Parse(obj(12)))
                parameters.Add(parIdentifierClient)

                tempObjects = connection.ExecuteQuery("SELECT Identifier, Nom FROM Client WHERE Identifier=@IdentifierClient", parameters)
                For Each tmpObj In tempObjects
                    Me.Client = New Client(tmpObj(1).ToString(), Long.Parse(tmpObj(0)))
                Next
                parameters.Clear()
                parIdentifierClient = Nothing


                Dim parIdentifierContremarque As MySqlParameter = connection.Create("@IdentifierContremarque", DbType.Int32, Long.Parse(obj(13)))
                parameters.Add(parIdentifierContremarque)

                tempObjects = connection.ExecuteQuery("SELECT Identifier, Nom FROM Contremarque WHERE Identifier=@IdentifierContremarque", parameters)
                For Each tmpObj In tempObjects
                    Me.Contremarque = New Contremarque(tmpObj(1).ToString(), Long.Parse(tmpObj(0)))
                Next
                parameters.Clear()
                parIdentifierContremarque = Nothing

                Me.Mesure = New Mesure(Long.Parse(obj(14))).GetMesure()

                Me.Remarques = Remarque.GetRemarques(Me.Identifier)

                Me.Qualites = Qualite.GetCommandeQualites(Me.Identifier)

                Dim parIdentifierCommande As MySqlParameter = connection.Create("@IdentifierCommande", DbType.Int32, Me.Identifier)
                parameters.Add(parIdentifierCommande)

                tempObjects = connection.ExecuteQuery("SELECT Identifier_Materiau, Epaisseur FROM Commande_Materiau WHERE Identifier_Commande=@IdentifierCommande", parameters)

                For Each tmpObj In tempObjects
                    Dim mat As New Materiau("", Long.Parse(tmpObj(0)), Integer.Parse(tmpObj(1)))
                    mat.GetMateriau()
                    Me.Materiaux.Add(mat)
                Next

                tempObjects = connection.ExecuteQuery("SELECT Identifier_Nature FROM Commande_Nature WHERE Identifier_Commande=@IdentifierCommande", parameters)

                For Each tmpObj In tempObjects
                    Me.Natures.Add(New Nature("", Long.Parse(tmpObj(0))).GetNature())
                Next

                tempObjects = connection.ExecuteQuery("SELECT Identifier_Finalisation FROM Commande_Finalisation WHERE Identifier_Commande=@IdentifierCommande", parameters)

                For Each tmpObj In tempObjects
                    Me.Finalisations.Add(New Finalisation(Long.Parse(tmpObj(0))).GetFinalisation())
                Next

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
    ''' Récupère la liste de toutes les commandes en base de données
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCommandes() As List(Of Commande)
        Dim commandes As New List(Of Commande)
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))
        Dim tempObjects As New List(Of List(Of Object))
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection
            connection.Open()

            'Exécute la requête
            Objects = connection.ExecuteQuery("SELECT Identifier, NumCmd, Montant, Arrhes, DateCommande, AdresseChantier, TpsDebit, TpsCmdNumerique, TpsFinition, TpsAutres," +
                                              " DelaiPrevu, IdentifierEtat, IdentifierClient, IdentifierContremarque, IdentifierMesure, DateMesure, DateFinalisations" +
                                              " FROM Commande")

            'Traite les résultats
            For Each obj In Objects
                Dim commande As New Commande()

                commande.Identifier = Long.Parse(obj(0))
                commande.NoCommande = Integer.Parse(obj(1))
                commande.MontantHT = Decimal.Parse(obj(2))
                commande.Arrhes = Decimal.Parse(obj(3))
                commande.DateCommande = DateTime.Parse(obj(4))
                commande.AdresseChantier = obj(5).ToString()
                commande.TpsDebit = Integer.Parse(obj(6))
                commande.TpsCommandeNumerique = Integer.Parse(obj(7))
                commande.TpsFinition = Integer.Parse(obj(8))
                commande.TpsAutres = Integer.Parse(obj(9))
                commande.DelaiPrevu = DateTime.Parse(obj(10))
                commande.DateMesure = DateTime.Parse(obj(15))
                commande.DateFinalisations = DateTime.Parse(obj(16))

                Dim parIdentifierEtat As MySqlParameter = connection.Create("@IdentifierEtat", DbType.Int32, Long.Parse(obj(11)))
                parameters.Add(parIdentifierEtat)

                tempObjects = connection.ExecuteQuery("SELECT Identifier, Label, Position FROM Etat WHERE Identifier=@IdentifierEtat", parameters)
                For Each tmpObj In tempObjects
                    commande.Etat = New Etat(tmpObj(1).ToString(), Integer.Parse(obj(2)), Long.Parse(tmpObj(0)))
                Next
                parameters.Clear()
                parIdentifierEtat = Nothing


                Dim parIdentifierClient As MySqlParameter = connection.Create("@IdentifierClient", DbType.Int32, Long.Parse(obj(12)))
                parameters.Add(parIdentifierClient)

                tempObjects = connection.ExecuteQuery("SELECT Identifier, Nom FROM Client WHERE Identifier=@IdentifierClient", parameters)
                For Each tmpObj In tempObjects
                    commande.Client = New Client(tmpObj(1).ToString(), Long.Parse(tmpObj(0)))
                Next
                parameters.Clear()
                parIdentifierClient = Nothing


                Dim parIdentifierContremarque As MySqlParameter = connection.Create("@IdentifierContremarque", DbType.Int32, Long.Parse(obj(13)))
                parameters.Add(parIdentifierContremarque)

                tempObjects = connection.ExecuteQuery("SELECT Identifier, Nom FROM Contremarque WHERE Identifier=@IdentifierContremarque", parameters)
                For Each tmpObj In tempObjects
                    commande.Contremarque = New Contremarque(tmpObj(1).ToString(), Long.Parse(tmpObj(0)))
                Next
                parameters.Clear()
                parIdentifierContremarque = Nothing

                commande.Mesure = New Mesure(Long.Parse(obj(14))).GetMesure()

                commande.Remarques = Remarque.GetRemarques(commande.Identifier)

                commande.Qualites = Qualite.GetCommandeQualites(commande.Identifier)

                Dim parIdentifierCommande As MySqlParameter = connection.Create("@IdentifierCommande", DbType.Int32, commande.Identifier)
                parameters.Add(parIdentifierCommande)

                tempObjects = connection.ExecuteQuery("SELECT Identifier_Materiau, Epaisseur FROM Commande_Materiau WHERE Identifier_Commande=@IdentifierCommande", parameters)

                For Each tmpObj In tempObjects
                    Dim mat As New Materiau("", Long.Parse(tmpObj(0)), Integer.Parse(tmpObj(1)))
                    mat.GetMateriau()
                    commande.Materiaux.Add(mat)
                Next

                tempObjects = connection.ExecuteQuery("SELECT Identifier_Nature FROM Commande_Nature WHERE Identifier_Commande=@IdentifierCommande", parameters)

                For Each tmpObj In tempObjects
                    commande.Natures.Add(New Nature("", Long.Parse(tmpObj(0))).GetNature())
                Next

                tempObjects = connection.ExecuteQuery("SELECT Identifier_Finalisation FROM Commande_Finalisation WHERE Identifier_Commande=@IdentifierCommande", parameters)

                For Each tmpObj In tempObjects
                    commande.Finalisations.Add(New Finalisation(Long.Parse(tmpObj(0))).GetFinalisation())
                Next

                commandes.Add(commande)

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

        Return commandes
    End Function

    ''' <summary>
    ''' Permet la mise à jour d'une commande
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Update(Optional ByVal IsRestrictUpdate As Boolean = False)
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)
        Dim query As String

        Try
            'Vérifie si le client existe, sinon, l'ajoute dans la DB
            If (Me.Client.Identifier = 0 And Me.Client.Nom <> "") Then
                Me.Client.Identifier = Me.Client.Insert()
            End If

            'Vérifie si la contremarque existe, sinon, l'ajoute dans la DB
            If (Me.Contremarque IsNot Nothing AndAlso Me.Contremarque.Identifier = 0 And Me.Contremarque.Nom <> "") Then
                Me.Contremarque.Identifier = Me.Contremarque.Insert()
            ElseIf Me.Contremarque Is Nothing Then
                Me.Contremarque = New Contremarque()
            End If

            ' Ouvre la connexion à la base de données
            connection.Open()

            ' Initialise les paramètres de la requête
            Dim parIdentifierCommande As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdentifierCommande)

            Dim parTpsDebit As MySqlParameter = connection.Create("@TpsDebit", DbType.Int32, Me.TpsDebit)
            parameters.Add(parTpsDebit)

            Dim parTpsCommandeNumerique As MySqlParameter = connection.Create("@TpsCmdNum", DbType.Int32, Me.TpsCommandeNumerique)
            parameters.Add(parTpsCommandeNumerique)

            Dim parTpsFinition As MySqlParameter = connection.Create("@TpsFinition", DbType.Int32, Me.TpsFinition)
            parameters.Add(parTpsFinition)

            Dim parTpsAutres As MySqlParameter = connection.Create("@TpsAutres", DbType.Int32, Me.TpsAutres)
            parameters.Add(parTpsAutres)

            Dim parIdentifierEtat As MySqlParameter = connection.Create("@IdentifierEtat", DbType.Int32, Me.Etat.Identifier)
            parameters.Add(parIdentifierEtat)

            'Défini la requête d'update à partir d'une session ayant tous les droits
            If Not IsRestrictUpdate Then

                Dim parNumCommande As MySqlParameter = connection.Create("@NumCommande", DbType.Int32, Me.NoCommande)
                parameters.Add(parNumCommande)

                Dim parMontant As MySqlParameter = connection.Create("@Montant", DbType.Decimal, Me.MontantHT)
                parameters.Add(parMontant)

                Dim parArrhes As MySqlParameter = connection.Create("@Arrhes", DbType.Decimal, Me.Arrhes)
                parameters.Add(parArrhes)

                Dim parDateCommande As MySqlParameter = connection.Create("@DateCommande", DbType.DateTime, Me.DateCommande)
                parameters.Add(parDateCommande)

                Dim parAdresse As MySqlParameter = connection.Create("@Adresse", DbType.String, Me.AdresseChantier)
                parameters.Add(parAdresse)

                Dim parDelaiPrevu As MySqlParameter = connection.Create("@DelaiPrevu", DbType.DateTime, Me.DelaiPrevu)
                parameters.Add(parDelaiPrevu)

                Dim parIdentifierClient As MySqlParameter = connection.Create("@IdentifierClient", DbType.Int32, Me.Client.Identifier)
                parameters.Add(parIdentifierClient)

                Dim parIdentifierContremarque As MySqlParameter = connection.Create("@IdentifierContremarque", DbType.Int32, Me.Contremarque.Identifier)
                parameters.Add(parIdentifierContremarque)

                Dim parIdentifierMesure As MySqlParameter = connection.Create("@IdentifierMesure", DbType.Int32, Me.Mesure.Identifier)
                parameters.Add(parIdentifierMesure)

                Dim parDateMesure As MySqlParameter
                If Me.DateMesure <> DateTime.MinValue Then
                    parDateMesure = connection.Create("@DateMesure", DbType.DateTime, Me.DateMesure)
                Else
                    parDateMesure = connection.Create("@DateMesure", DbType.DateTime, Nothing)
                End If
                parameters.Add(parDateMesure)

                Dim parDateFinalisations As MySqlParameter = connection.Create("@DateFinalisations", DbType.DateTime, Me.DateFinalisations)
                parameters.Add(parDateFinalisations)

                query = "UPDATE Commande SET NumCmd=@NumCommande, Montant=@Montant, Arrhes=@Arrhes, DateCommande=@DateCommande, AdresseChantier=@Adresse, " +
                    "TpsDebit=@TpsDebit, TpsCmdNumerique=@TpsCmdNum, TpsFinition=@TpsFinition, TpsAutres=@TpsAutres, DelaiPrevu=@DelaiPrevu, IdentifierEtat=@IdentifierEtat, " +
                    "IdentifierClient=@IdentifierClient, IdentifierContremarque=@IdentifierContremarque, IdentifierMesure=@IdentifierMesure, DateMesure=@DateMesure, DateFinalisations=@DateFinalisations " +
                    "WHERE Identifier=@Identifier"

                'Défini la requête d'update à partir d'une session ayant des droits très restreint
            Else
                query = "UPDATE Commande SET TpsDebit=@TpsDebit, TpsCmdNumerique=@TpsCmdNum, TpsFinition=@TpsFinition, TpsAutres=@TpsAutres, IdentifierEtat=@IdentifierEtat " +
                        "WHERE Identifier=@Identifier"
            End If

            'Exécute la requête
            connection.ExecuteNonQuery(query, parameters)

            parameters.Clear()

            Dim Objects As New List(Of List(Of Object))


            'Met à jour les liaisons entre la commande et les matériaux, natures, prestations pour une session ayant les droits adéquat
            If Not IsRestrictUpdate Then

                'Met à jour les matériaux
                Dim actualMateriaux As New List(Of Materiau)

                parameters.Add(parIdentifierCommande)
                query = "SELECT Identifier_Materiau, Epaisseur FROM Commande_Materiau WHERE Identifier_Commande=@Identifier"

                Objects = connection.ExecuteQuery(query, parameters)

                For Each obj In Objects
                    actualMateriaux.Add(New Materiau("", Long.Parse(obj(0)), Integer.Parse(obj(1))).GetMateriau())
                Next

                For Each mat In Me.Materiaux
                    Dim isUpdated As Boolean = False

                    For Each actMat In actualMateriaux
                        If (mat.Equals(actMat)) Then
                            isUpdated = True

                            Exit For
                        ElseIf (mat.Identifier = actMat.Identifier And mat.Epaisseur <> actMat.Epaisseur) Then
                            Dim parEpaisseur As MySqlParameter = connection.Create("@Epaisseur", DbType.Int32, mat.Epaisseur)
                            parameters.Add(parEpaisseur)

                            query = "UPDATE Commande_Materiau SET Epaisseur=@Epaisseur"
                            connection.ExecuteNonQuery(query, parameters)

                            parameters.Clear()
                            parameters.Add(parIdentifierCommande)

                            isUpdated = True

                            Exit For
                        End If
                    Next

                    parameters.Clear()

                    If (Not isUpdated) Then
                        Dim parIdentifierMateriau As MySqlParameter = connection.Create("@IdMateriau", DbType.Int32, mat.Identifier)
                        parameters.Add(parIdentifierMateriau)
                        Dim parEpaisseur As MySqlParameter = connection.Create("@Epaisseur", DbType.Int32, mat.Epaisseur)
                        parameters.Add(parEpaisseur)

                        parameters.Add(parIdentifierCommande)

                        query = "INSERT INTO Commande_Materiau (Identifier_Commande, Identifier_Materiau, Epaisseur)" +
                                " VALUES (@Identifier, @IdMateriau, @Epaisseur)"

                        connection.ExecuteNonQuery(query, parameters)

                        actualMateriaux.Add(mat)

                        parameters.Clear()
                    End If

                    parameters.Clear()
                Next

                parameters.Clear()

                If actualMateriaux.Count > Me.Materiaux.Count Then
                    For Each m In actualMateriaux
                        Dim isExists As Boolean = False
                        For Each mat In Me.Materiaux
                            If (m.Equals(mat)) Then
                                isExists = True
                            End If
                        Next

                        Dim parIdMateriau As MySqlParameter = connection.Create("@IdMateriau", DbType.Int32, m.Identifier)
                        parameters.Add(parIdMateriau)
                        parameters.Add(parIdentifierCommande)

                        If (Not isExists) Then
                            query = "DELETE FROM Commande_Materiau WHERE Identifier_Commande=@Identifier And Identifier_Materiau=@IdMateriau"
                            connection.ExecuteNonQuery(query, parameters)
                        End If

                        parameters.Clear()
                    Next
                End If

                parameters.Clear()

                'Met à jour les natures
                Dim actualNatures As New List(Of Nature)

                parameters.Add(parIdentifierCommande)
                query = "SELECT Identifier_Nature FROM Commande_Nature WHERE Identifier_Commande=@Identifier"

                Objects = connection.ExecuteQuery(query, parameters)

                For Each obj In Objects
                    actualNatures.Add(New Nature("", Long.Parse(obj(0))).GetNature())
                Next

                For Each nat In Me.Natures
                    Dim isUpdated As Boolean = False

                    For Each actNat In actualNatures
                        If (nat.Equals(actNat)) Then
                            isUpdated = True

                            Exit For
                        End If
                    Next

                    parameters.Clear()

                    If (Not isUpdated) Then
                        Dim parIdentifierNature As MySqlParameter = connection.Create("@IdNature", DbType.Int32, nat.Identifier)
                        parameters.Add(parIdentifierNature)

                        parameters.Add(parIdentifierCommande)

                        query = "INSERT INTO Commande_Nature (Identifier_Commande, Identifier_Nature)" +
                                " VALUES (@Identifier, @IdNature)"

                        connection.ExecuteNonQuery(query, parameters)

                        parameters.Clear()
                    End If

                    parameters.Clear()
                Next

                parameters.Clear()

                If actualNatures.Count > Me.Natures.Count Then
                    For Each n In actualNatures
                        Dim isExists As Boolean = False
                        For Each nat In Me.Natures
                            If (n.Equals(nat)) Then
                                isExists = True
                            End If
                        Next

                        Dim parIdNature As MySqlParameter = connection.Create("@IdNature", DbType.Int32, n.Identifier)
                        parameters.Add(parIdNature)
                        parameters.Add(parIdentifierCommande)

                        If (Not isExists) Then
                            query = "DELETE FROM Commande_Nature WHERE Identifier_Commande=@Identifier And Identifier_Nature=@IdNature"
                            connection.ExecuteNonQuery(query, parameters)
                        End If

                        parameters.Clear()
                    Next
                End If

                parameters.Clear()

                'Met à jour les prestations
                Dim actualFinalisations As New List(Of Finalisation)

                parameters.Add(parIdentifierCommande)
                query = "SELECT Identifier_Finalisation FROM Commande_Finalisation WHERE Identifier_Commande=@Identifier"

                Objects = connection.ExecuteQuery(query, parameters)

                For Each obj In Objects
                    actualFinalisations.Add(New Finalisation(Long.Parse(obj(0))).GetFinalisation())
                Next

                For Each fin In Me.Finalisations
                    Dim isUpdated As Boolean = False

                    For Each actFin In actualFinalisations
                        If (fin.Equals(actFin)) Then
                            isUpdated = True

                            Exit For
                        End If
                    Next

                    parameters.Clear()

                    If (Not isUpdated) Then
                        Dim parIdentifierFinalisation As MySqlParameter = connection.Create("@IdFinalisation", DbType.Int32, fin.Identifier)
                        parameters.Add(parIdentifierFinalisation)

                        parameters.Add(parIdentifierCommande)

                        query = "INSERT INTO Commande_Finalisation (Identifier_Commande, Identifier_Finalisation)" +
                                " VALUES (@Identifier, @IdFinalisation)"

                        connection.ExecuteNonQuery(query, parameters)

                        parameters.Clear()
                    End If

                    parameters.Clear()
                Next

                parameters.Clear()

                If actualFinalisations.Count > Me.Finalisations.Count Then
                    For Each f In actualFinalisations
                        Dim isExists As Boolean = False
                        For Each fin In Me.Finalisations
                            If (f.Equals(fin)) Then
                                isExists = True
                            End If
                        Next

                        Dim parIdFinalisation As MySqlParameter = connection.Create("@IdFinalisation", DbType.Int32, f.Identifier)
                        parameters.Add(parIdFinalisation)
                        parameters.Add(parIdentifierCommande)

                        If (Not isExists) Then
                            query = "DELETE FROM Commande_Finalisation WHERE Identifier_Commande=@Identifier And Identifier_Finalisation=@IdFinalisation"
                            connection.ExecuteNonQuery(query, parameters)
                        End If

                        parameters.Clear()
                    Next
                End If

            End If

            'Met à jour les remarques
            Dim actualRemarques As New List(Of Remarque)

            'Récupère les remarques actuellement en base
            actualRemarques = Remarque.GetRemarques(Me.Identifier)

            'Début suppression des remarques effacées
            Dim rems As New List(Of Remarque)
            For Each r In actualRemarques
                Dim isExists = False
                For Each re In Me.Remarques
                    If r.Equals(re) Then
                        isExists = True
                        Exit For
                    End If
                Next

                If Not isExists Then rems.Add(r)
            Next

            For Each i In rems
                i.Delete()
                actualRemarques.Remove(i)
            Next
            'fin suppression des remarques effacées

            rems.Clear()

            'début ajout des nouvelles remarques
            For Each r In Me.Remarques
                Dim isExists = False
                For Each re In actualRemarques
                    If r.Equals(re) Then
                        isExists = True
                        Exit For
                    End If
                Next

                If Not isExists Then rems.Add(r)
            Next

            For Each i In rems
                i.Insert(Me.Identifier)
                actualRemarques.Add(i)
            Next
            actualRemarques = Nothing
            rems = Nothing
            'fin d'ajout des nouvelles remarques

            Dim actualQualitiesPb As New List(Of Qualite)

            actualQualitiesPb = Qualite.GetCommandeQualites(Me.Identifier)

            'Début suppression des problèmes de qualité effacés
            Dim quals As New List(Of Qualite)
            For Each q In actualQualitiesPb
                Dim isExists = False
                For Each qu In Me.Qualites
                    If q.Equals(qu) Then
                        isExists = True
                        Exit For
                    End If
                Next

                If Not isExists Then quals.Add(q)
            Next

            For Each i In quals
                i.DeletePb(Me.Identifier)
                actualQualitiesPb.Remove(i)
            Next
            'fin suppression des problèmes de qualité effacés

            quals.Clear()

            'début ajout des nouveaux problèmes de qualité
            If Me.Qualites.Count > actualQualitiesPb.Count Then
                For Each q In Me.Qualites
                    Dim isExists = False
                    For Each qu In actualQualitiesPb
                        If q.Equals(qu) Then
                            isExists = True
                            Exit For
                        End If
                    Next

                    If Not isExists Then quals.Add(q)
                Next

                For Each i In quals
                    i.InsertPb(Me.Identifier)
                    actualQualitiesPb.Add(i)
                Next
            End If
            actualQualitiesPb = Nothing
            quals = Nothing
            'fin d'ajout des nouveaux problèmes de qualité

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
    ''' Permet de réserver ou de libérer la commande en écriture
    ''' </summary>
    ''' <param name="flag">Identifier de la session ayant les droits de modification, 0 si la commande est libre en modification</param>
    ''' <remarks></remarks>
    Public Sub UpdateFlag(ByVal flag As Long)
        Dim connection As New MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection
            connection.Open()

            'Défini les paramètres de la requête
            Dim parCommande As MySqlParameter = connection.Create("@IdCmd", DbType.Int64, Me.Identifier)
            parameters.Add(parCommande)

            Dim parSession As MySqlParameter = connection.Create("@Flag", DbType.Int64, flag)
            parameters.Add(parSession)

            'Exécute la requête
            connection.ExecuteNonQuery("UPDATE Commande SET Flag=@Flag WHERE Identifier=@IdCmd", parameters)

            parameters = Nothing
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
        Finally
            Try
                'Ferme la connection
                connection.Close()
                connection = Nothing
            Catch ex As Exception
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Permet de récupérer le flag d'une une commande
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFlag() As Commande
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))
        Dim parameters As New List(Of MySqlParameter)

        Try
            'Ouvre la connection à la base de données
            connection.Open()

            'Initialise les paramètres de la commande
            Dim parNumeroCommande As MySqlParameter = connection.Create("@NumCommande", DbType.Int32, Me.NoCommande)
            parameters.Add(parNumeroCommande)

            'Requête
            Objects = connection.ExecuteQuery("SELECT Flag" +
                                              " FROM Commande" +
                                              " WHERE NumCmd=@NumCommande", parameters)

            'Traite les résultats
            For Each obj In Objects
                Me.Flag = Long.Parse(obj(0))
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

#End Region

End Class
