﻿Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.IO

Public Class RechercheCommande

#Region "DependencyProperty"

    Public Shared ReadOnly SessionProperty As DependencyProperty = DependencyProperty.Register("Session", GetType(Session), GetType(RechercheCommande),
                                                                                               New FrameworkPropertyMetadata(Nothing))
    Public Property Session As Session
        Get
            Return DirectCast(Me.GetValue(SessionProperty), Session)
        End Get
        Set(ByVal value As Session)
            Me.SetValue(SessionProperty, value)
        End Set
    End Property

    Public Shared ReadOnly PlanningProperty As DependencyProperty = DependencyProperty.Register("Planning", GetType(PlanningControl), GetType(RechercheCommande),
                                                                                               New FrameworkPropertyMetadata(Nothing))
    Public Property Planning As PlanningControl
        Get
            Return DirectCast(Me.GetValue(PlanningProperty), PlanningControl)
        End Get
        Set(ByVal value As PlanningControl)
            Me.SetValue(PlanningProperty, value)
        End Set
    End Property

#End Region

#Region "Fields"

    Private _Commandes As List(Of Commande)
    Private IsUpdClient As Boolean = False
    Private IsUpdContremarque As Boolean = True

#End Region

#Region "Properties"

    Public Property ListCommandes As List(Of Commande)
        Get
            Return Me._Commandes
        End Get
        Set(ByVal value As List(Of Commande))
            Me._Commandes = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().

    End Sub

#End Region

#Region "Button"

    ''' <summary>
    ''' Bouton permettant de générer le fichier Excel à partir des résultats de la recherche
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnExcel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If LbxSearchCmd.Items.Count > 0 Then
            Dim cmds As New List(Of Commande)
            For Each item In LbxSearchCmd.Items
                Dim cmd As Commande = item
                cmds.Add(cmd)
            Next

            Dim client As String = String.Empty
            Dim cm As String = String.Empty
            Dim numCmd As String = String.Empty
            Dim mat As String = String.Empty

            Dim search As String = String.Empty

            Dim cbxi As ComboBoxItem = CbxTri.SelectedItem
            Dim tri As String = cbxi.Content
            Dim etatCmd As String
            etatCmd = "Commandes " + Me.CbxEtat.SelectedItem.ToString().ToLower() + " triées par " + tri.ToLower()

            If Me.AutoCompNClient.SelectedItem IsNot Nothing Then
                Dim cl As Client = AutoCompNClient.SelectedItem
                client = "Client : " + cl.Nom
            End If

            If Me.AutoCompNContremarque.SelectedItem IsNot Nothing Then
                Dim cmq As Contremarque = AutoCompNContremarque.SelectedItem
                cm = "CM : " + cmq.Nom
            End If

            If Me.AutoCompNumCmd.SelectedItem IsNot Nothing Then
                numCmd = "N° cmd : " + AutoCompNumCmd.SelectedItem.ToString()
            End If

            If Me.AutoCompLMateriau.SelectedItem IsNot Nothing Then
                Dim m As Materiau = AutoCompLMateriau.SelectedItem
                mat = "Matériau : " + m.Label
            End If

            If client <> String.Empty Then
                search = client
            End If

            If cm <> String.Empty Then
                If search <> String.Empty Then
                    search = search + "   /   " + cm
                Else
                    search = cm
                End If
            End If

            If numCmd <> String.Empty Then
                If search <> String.Empty Then
                    search = search + "   /   " + numCmd
                Else
                    search = numCmd
                End If
            End If

            If mat <> String.Empty Then
                If search <> String.Empty Then
                    search = search + "   /   " + mat
                Else
                    search = mat
                End If
            End If

            ExcelExport.ExportCommande(cmds, search, etatCmd, "XLSX")
        Else
            MessageBox.Show("La recherche doit contenir au moins un résultat.", "Génération du fichier EXCEL", MessageBoxButton.OK, MessageBoxImage.Exclamation)
        End If
    End Sub

    ''' <summary>
    ''' Bouton permettant de générer le fichier PDF à partir des résultats de la recherche
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnPdf_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If LbxSearchCmd.Items.Count > 0 Then
                Dim cmds As New List(Of Commande)
                For Each item In LbxSearchCmd.Items
                    Dim cmd As Commande = item
                    cmds.Add(cmd)
                Next

                Dim client As String = String.Empty
                Dim cm As String = String.Empty
                Dim numCmd As String = String.Empty
                Dim mat As String = String.Empty

                Dim search As String = String.Empty

                Dim etatCmd As String
                etatCmd = "Commandes " + Me.CbxEtat.SelectedItem.ToString().ToLower()

                If Me.AutoCompNClient.SelectedItem IsNot Nothing Then
                    Dim cl As Client = AutoCompNClient.SelectedItem
                    client = "Client : " + cl.Nom
                End If

                If Me.AutoCompNContremarque.SelectedItem IsNot Nothing Then
                    Dim cmq As Contremarque = AutoCompNContremarque.SelectedItem
                    cm = "CM : " + cmq.Nom
                End If

                If Me.AutoCompNumCmd.SelectedItem IsNot Nothing Then
                    numCmd = "N° cmd : " + AutoCompNumCmd.SelectedItem.ToString()
                End If

                If Me.AutoCompLMateriau.SelectedItem IsNot Nothing Then
                    Dim m As Materiau = AutoCompLMateriau.SelectedItem
                    mat = "Matériau : " + m.Label
                End If

                If client <> String.Empty Then
                    search = client
                End If

                If cm <> String.Empty Then
                    If search <> String.Empty Then
                        search = search + "   /   " + cm
                    Else
                        search = cm
                    End If
                End If

                If numCmd <> String.Empty Then
                    If search <> String.Empty Then
                        search = search + "   /   " + numCmd
                    Else
                        search = numCmd
                    End If
                End If

                If mat <> String.Empty Then
                    If search <> String.Empty Then
                        search = search + "   /   " + mat
                    Else
                        search = mat
                    End If
                End If

                ExcelExport.ExportCommande(cmds, search, etatCmd, "PDF")
            Else
                MessageBox.Show("La recherche doit contenir au moins un résultat.", "Génération du fichier PDF", MessageBoxButton.OK, MessageBoxImage.Exclamation)
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
            Dim sw As New StreamWriter(My.Settings.ConfigFiles + "\log.txt")

            Dim content As String = "BtnPDF" + vbCrLf + ex.StackTrace.ToString() + vbCrLf + vbCrLf + ex.Source.ToString()
            If ex.InnerException IsNot Nothing Then
                content = content + vbCrLf + vbCrLf + ex.InnerException.ToString()
            End If

            content = content + vbCrLf + System.IO.Path.GetFullPath(My.Settings.Logo) + vbCrLf + vbCrLf + "/BtnPDF"

            sw.Write(content)

            sw.Close()
        End Try
    End Sub

    ''' <summary>
    ''' Bouton permettant d'ouvrir la commande sélectionnée afin de pouvoir la modifier
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnOpenCmd_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If LbxSearchCmd.SelectedItem IsNot Nothing Then

            Dim cmd As Commande = LbxSearchCmd.SelectedItem

            Dim commande As ConsultCommande = New ConsultCommande(Me.Session, cmd, Me, Me.Planning)
            commande.Show()
        Else
            MessageBox.Show("Veuillez sélectionner une commande.", "Aucune commande sélectionnée", MessageBoxButton.OK, MessageBoxImage.Exclamation)
        End If
    End Sub

    ''' <summary>
    ''' Bouton permettant de lancer la recherche
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub BtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.LbxSearchCmd.Items.Clear()
        Dim whereEtat As String = String.Empty
        Dim param As String = String.Empty

        If Me.CbxEtat.SelectedIndex = 0 Then
            whereEtat = " WHERE c.IdentifierEtat = e.Identifier AND e.Label <> 'Terminée' AND e.Label <> 'Rendue' AND "
        ElseIf Me.CbxEtat.SelectedIndex = 1 Then
            whereEtat = " WHERE c.IdentifierEtat = e.Identifier AND e.Label = 'Terminée' AND "
        ElseIf Me.CbxEtat.SelectedIndex = 2 Then
            whereEtat = " WHERE c.IdentifierEtat = e.Identifier AND e.Label = 'Rendue' AND "
        End If

        If CbxTri.SelectedIndex = 0 Then
            param = " c.DelaiPrevu"
        Else
            param = " c.DateCommande"
        End If

        If Me.AutoCompLMateriau.SelectedItem IsNot Nothing Then
            Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
            Dim Objects As List(Of List(Of Object))
            Dim materiau As Materiau = AutoCompLMateriau.SelectedItem
            Dim parameters As New List(Of MySqlParameter)
            Dim query As String = String.Empty

            Try
                connection.Open()

                Dim parIdMateriau As MySqlParameter = connection.Create("@IdMateriau", DbType.Int32, materiau.Identifier)

                If (Me.AutoCompNClient.SelectedItem IsNot Nothing And Me.AutoCompNContremarque.SelectedItem IsNot Nothing) Then
                    Dim client As Client = AutoCompNClient.SelectedItem
                    Dim cm As Contremarque = AutoCompNContremarque.SelectedItem

                    parameters.Add(parIdMateriau)

                    Dim parIdClient As MySqlParameter = connection.Create("@IdClient", DbType.Int32, client.Identifier)
                    parameters.Add(parIdClient)

                    Dim parIdContremarque As MySqlParameter = connection.Create("@IdContremarque", DbType.Int32, cm.Identifier)
                    parameters.Add(parIdContremarque)

                    query = "Select DISTINCT c.NumCmd, c.DelaiPrevu from Commande as c, commande_materiau as cm, materiau as m, Etat as e" + whereEtat + "cm.identifier_commande = c.identifier and " +
                     "cm.identifier_materiau = m.identifier and c.IdentifierClient = @IdClient and c.IdentifierContremarque = @IdContremarque and m.identifier = @IdMateriau Order By" + param

                ElseIf (AutoCompNClient.SelectedItem IsNot Nothing) Then
                    Dim client As Client = AutoCompNClient.SelectedItem

                    parameters.Add(parIdMateriau)

                    Dim parIdClient As MySqlParameter = connection.Create("@IdClient", DbType.Int32, client.Identifier)
                    parameters.Add(parIdClient)

                    query = "Select DISTINCT c.NumCmd, c.DelaiPrevu from Commande as c, commande_materiau as cm, materiau as m, Etat as e" + whereEtat + "cm.identifier_commande = c.identifier and " +
                       "cm.identifier_materiau = m.identifier and c.IdentifierClient = @IdClient and m.identifier = @IdMateriau Order By" + param

                ElseIf (AutoCompNContremarque.SelectedItem IsNot Nothing) Then
                    Dim cm As Contremarque = AutoCompNContremarque.SelectedItem

                    parameters.Add(parIdMateriau)

                    Dim parIdContremarque As MySqlParameter = connection.Create("@IdContremarque", DbType.Int32, cm.Identifier)
                    parameters.Add(parIdContremarque)

                    query = "Select DISTINCT c.NumCmd, c.DelaiPrevu from Commande as c, commande_materiau as cm, materiau as m, Etat as e" + whereEtat + "cm.identifier_commande = c.identifier and " +
                       "cm.identifier_materiau = m.identifier and c.IdentifierContremarque = @IdContremarque and m.identifier = @IdMateriau Order By" + param
                Else
                    parameters.Add(parIdMateriau)

                    query = "Select DISTINCT c.NumCmd, c.DelaiPrevu from Commande as c, commande_materiau as cm, materiau as m, Etat as e" + whereEtat + "cm.identifier_commande = c.identifier and " +
                                           "cm.identifier_materiau = m.identifier and m.identifier = @IdMateriau Order By" + param
                End If

                Objects = connection.ExecuteQuery(query, parameters)

                parameters.Clear()

                connection.Close()

                For Each obj In Objects
                    Dim cmd As New Commande(Integer.Parse(obj(0)))
                    cmd = cmd.GetCommande()

                    If Me.CbxEtat.SelectedIndex = 0 Then
                        If cmd.Etat.Label <> "Terminée" And cmd.Etat.Label <> "Rendue" Then
                            LbxSearchCmd.Items.Add(cmd)
                        End If
                    ElseIf Me.CbxEtat.SelectedIndex = 1 Then
                        If cmd.Etat.Label = "Terminée" Then
                            LbxSearchCmd.Items.Add(cmd)
                        End If
                    ElseIf Me.CbxEtat.SelectedIndex = 2 Then
                        If cmd.Etat.Label = "Rendue" Then
                            LbxSearchCmd.Items.Add(cmd)
                        End If
                    End If
                Next
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                Try
                    connection.Close()
                Catch ex As Exception
                End Try
            End Try
        ElseIf (Me.AutoCompNumCmd.SelectedItem IsNot Nothing) Then
            Dim cmd As New Commande(Integer.Parse(AutoCompNumCmd.SelectedItem))
            cmd = cmd.GetCommande()
            LbxSearchCmd.Items.Add(cmd)
            If cmd.Etat.Label = "Terminée" Then
                Me.CbxEtat.SelectedIndex = 1
            ElseIf cmd.Etat.Label = "Rendue" Then
                Me.CbxEtat.SelectedIndex = 2
            Else
                Me.CbxEtat.SelectedIndex = 0
            End If
        ElseIf Me.AutoCompNClient.SelectedItem IsNot Nothing Or Me.AutoCompNClient.Text <> "" Then
            Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
            Dim Objects As New List(Of List(Of Object))
            Dim parameters As New List(Of MySqlParameter)

            Try
                connection.Open()

                Dim cli As String
                Dim cmq As String

                If Me.AutoCompNClient.SelectedItem IsNot Nothing Then
                    Dim m As Client = Me.AutoCompNClient.SelectedItem
                    cli = "IdentifierClient=" + m.Identifier.ToString() + " "
                Else
                    Dim cl As Client = Nothing

                    For Each item In Me.AutoCompNClient.ItemsSource
                        Dim c As Client = item
                        If c.Nom = Me.AutoCompNClient.Text.ToUpper() Then
                            Me.AutoCompNClient.SelectedItem = item
                            cl = c
                            Exit For
                        End If
                    Next

                    If Me.AutoCompNClient.SelectedItem Is Nothing Then Exit Sub

                    cli = "IdentifierClient=" + cl.Identifier.ToString() + " "
                End If

                If Me.AutoCompNContremarque.SelectedItem IsNot Nothing Then
                    Dim c As Contremarque = Me.AutoCompNContremarque.SelectedItem
                    If cli = String.Empty Then
                        cmq = "IdentifierContremarque=" + c.Identifier.ToString() + " "
                    Else
                        cmq = "AND IdentifierContremarque=" + c.Identifier.ToString() + " "
                    End If
                Else
                    cmq = String.Empty
                End If

                Dim query As String

                query = "SELECT DISTINCT NumCmd, DateFinalisations, DelaiPrevu FROM Commande as c, Etat as e" + whereEtat + cli + cmq + "Order By" + param

                Objects = connection.ExecuteQuery(query, parameters)

                parameters = Nothing

                connection.Close()

                For Each obj In Objects
                    Dim cmd As New Commande(Long.Parse(obj(0)))
                    cmd = cmd.GetCommande()

                    If Me.CbxEtat.SelectedIndex = 0 Then
                        If cmd.Etat.Label <> "Terminée" And cmd.Etat.Label <> "Rendue" Then
                            LbxSearchCmd.Items.Add(cmd)
                        End If
                    ElseIf Me.CbxEtat.SelectedIndex = 1 Then
                        If cmd.Etat.Label = "Terminée" Then
                            LbxSearchCmd.Items.Add(cmd)
                        End If
                    ElseIf Me.CbxEtat.SelectedIndex = 2 Then
                        If cmd.Etat.Label = "Rendue" Then
                            LbxSearchCmd.Items.Add(cmd)
                        End If
                    End If
                Next
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Erreur")
            Finally
                Try
                    connection.Close()
                Catch ex As Exception
                End Try
            End Try
        ElseIf Me.AutoCompNContremarque.SelectedItem IsNot Nothing Or Me.AutoCompNContremarque.Text <> "" Then
            Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
            Dim Objects As New List(Of List(Of Object))
            Dim parameters As New List(Of MySqlParameter)

            Try
                connection.Open()

                Dim cmq As String

                If Me.AutoCompNContremarque.SelectedItem IsNot Nothing Then
                    Dim c As Contremarque = Me.AutoCompNContremarque.SelectedItem
                    cmq = "IdentifierContremarque=" + c.Identifier.ToString() + " "
                Else
                    Dim cm As Contremarque = Nothing

                    For Each item In Me.AutoCompNContremarque.ItemsSource
                        Dim c As Contremarque = item
                        If c.Nom = Me.AutoCompNContremarque.Text.ToUpper() Then
                            Me.AutoCompNContremarque.SelectedItem = item
                            cm = c
                            Exit For
                        End If
                    Next

                    If Me.AutoCompNContremarque.SelectedItem Is Nothing Then Exit Sub

                    cmq = "IdentifierContremarque=" + cm.Identifier.ToString() + " "
                End If

                Dim query As String

                query = "SELECT DISTINCT NumCmd, DateFinalisations, DelaiPrevu FROM Commande as c, Etat as e" + whereEtat + cmq + "Order By" + param

                Objects = connection.ExecuteQuery(query, parameters)

                parameters = Nothing

                connection.Close()

                For Each obj In Objects
                    Dim cmd As New Commande(Long.Parse(obj(0)))
                    cmd = cmd.GetCommande()

                    If Me.CbxEtat.SelectedIndex = 0 Then
                        If cmd.Etat.Label <> "Terminée" And cmd.Etat.Label <> "Rendue" Then
                            LbxSearchCmd.Items.Add(cmd)
                        End If
                    ElseIf Me.CbxEtat.SelectedIndex = 1 Then
                        If cmd.Etat.Label = "Terminée" Then
                            LbxSearchCmd.Items.Add(cmd)
                        End If
                    ElseIf Me.CbxEtat.SelectedIndex = 2 Then
                        If cmd.Etat.Label = "Rendue" Then
                            LbxSearchCmd.Items.Add(cmd)
                        End If
                    End If
                Next
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Erreur")
            Finally
                Try
                    connection.Close()
                Catch ex As Exception
                End Try
            End Try
        ElseIf AutoCompNClient.Text = "" And AutoCompNContremarque.Text = "" And AutoCompNumCmd.Text = "" And AutoCompLMateriau.Text = "" Then
            Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
            Dim Objects As New List(Of List(Of Object))
            Dim parameters As New List(Of MySqlParameter)

            Try
                connection.Open()

                Dim query As String
                query = "SELECT DISTINCT NumCmd, DateFinalisations, DelaiPrevu FROM Commande as c, Etat as e" + whereEtat.Substring(0, whereEtat.Length - 4) + "Order By" + param

                Objects = connection.ExecuteQuery(query, parameters)

                parameters = Nothing

                connection.Close()

                For Each obj In Objects
                    LbxSearchCmd.Items.Add(New Commande(Long.Parse(obj(0))).GetCommande())
                Next
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Erreur")
            Finally
                Try
                    connection.Close()
                Catch ex As Exception
                End Try
            End Try
        End If
    End Sub

#End Region

#Region "AutoCompletion"

    ''' <summary>
    ''' Delegate de l'auto-complétion du nom client
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub cbxClient()

    ''' <summary>
    ''' Auto-complétion du nom client
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AutoCompClient()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim objects As New List(Of List(Of Object))
        Try
            connection.Open()

            Dim text As String = Me.AutoCompNClient.Text.Replace("'", "\'")
            text = text.Replace("""", "\""")

            objects = connection.ExecuteQuery("SELECT Identifier, Nom FROM Client WHERE Nom Like '%" + text.ToUpper() + "%' Order By Nom")

            Dim clients As New List(Of Client)

            For Each obj In objects
                clients.Add(New Client(obj(1).ToString(), Long.Parse(obj(0))))
            Next

            Me.AutoCompNClient.ItemsSource = clients
            Me.AutoCompNClient.PopulateComplete()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                connection.Close()
            Catch ex As Exception
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Delegate de l'auto-complétion du n° commande
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub cbxNumCommande()

    ''' <summary>
    ''' Auto-complétion du n° commande
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AutoCompCommand()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim objects As New List(Of List(Of Object))
        Try
            connection.Open()

            Dim text As String = Me.AutoCompNumCmd.Text.Replace("'", "\'")
            text = text.Replace("""", "\""")

            objects = connection.ExecuteQuery("SELECT NumCmd, DelaiPrevu FROM Commande WHERE NumCmd Like '%" + text.ToUpper() + "%' Order By DelaiPrevu")

            Dim commandes As New List(Of String)

            For Each obj In objects
                commandes.Add(obj(0).ToString())
            Next

            Me.AutoCompNumCmd.ItemsSource = commandes
            Me.AutoCompNumCmd.PopulateComplete()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                connection.Close()
            Catch ex As Exception
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Delegate de l'auto-complétion du nom contremarque
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub cbxContremarque()

    ''' <summary>
    ''' Auto-complétion du nom contremarque
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AutoCompContremarque()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim objects As New List(Of List(Of Object))
        Try
            connection.Open()

            Dim text As String = Me.AutoCompNContremarque.Text.Replace("'", "\'")
            text = text.Replace("""", "\""")

            objects = connection.ExecuteQuery("SELECT Identifier, Nom FROM Contremarque WHERE Nom Like '%" + text.ToUpper() + "%' Order By Nom")

            Dim contremarques = New List(Of Contremarque)

            For Each obj In objects
                contremarques.Add(New Contremarque(obj(1).ToString(), Long.Parse(obj(0))))
            Next

            Me.AutoCompNContremarque.ItemsSource = contremarques
            Me.AutoCompNContremarque.PopulateComplete()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                connection.Close()
            Catch ex As Exception
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Delegate de l'auto-complétion du label materiau
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub cbxMateriau()

    ''' <summary>
    ''' Auto-complétion du nom contremarque
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AutoCompMateriau()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim objects As New List(Of List(Of Object))
        Try
            connection.Open()

            Dim text As String = Me.AutoCompLMateriau.Text.Replace("'", "\'")
            text = text.Replace("""", "\""")

            objects = connection.ExecuteQuery("SELECT Identifier, Label FROM Materiau WHERE Label Like '" + text.ToUpper() + "%' Order By Label")

            Dim materiaux = New List(Of Materiau)

            For Each obj In objects
                materiaux.Add(New Materiau(obj(1).ToString(), Long.Parse(obj(0))))
            Next

            Me.AutoCompLMateriau.ItemsSource = materiaux
            Me.AutoCompLMateriau.PopulateComplete()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                connection.Close()
            Catch ex As Exception
            End Try
        End Try
    End Sub


    ''' <summary>
    ''' Écriture dans l'AutoCompleteBox du n° de commande
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AutoCompNumCmd_Populating(ByVal sender As System.Object, ByVal e As System.Windows.Controls.PopulatingEventArgs)
        Dim del As cbxNumCommande
        del = AddressOf AutoCompCommand
        del.Invoke()

        Me.AutoCompNClient.SelectedItem = Nothing
        Me.AutoCompNClient.Text = ""
        Me.AutoCompNContremarque.SelectedItem = Nothing
        Me.AutoCompNContremarque.Text = ""
        Me.AutoCompLMateriau.SelectedItem = Nothing
        Me.AutoCompLMateriau.Text = ""
    End Sub

    ''' <summary>
    ''' Écriture dans l'AutoCompleteBox du nom client
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AutoCompNClient_Populating(ByVal sender As System.Object, ByVal e As System.Windows.Controls.PopulatingEventArgs)
        Dim del As cbxClient
        del = AddressOf AutoCompClient
        del.Invoke()
    End Sub

    ''' <summary>
    ''' Écriture dans l'AutoCompleteBox de la contremarque
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AutoCompNContremarque_Populating(ByVal sender As System.Object, ByVal e As System.Windows.Controls.PopulatingEventArgs)
        Dim del As cbxContremarque
        del = AddressOf AutoCompContremarque
        del.Invoke()
    End Sub

    ''' <summary>
    ''' Écriture dans l'AutoCompleteBox du matériau
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AutoCompLMateriau_Populating(ByVal sender As System.Object, ByVal e As System.Windows.Controls.PopulatingEventArgs)
        Dim del As cbxMateriau
        del = AddressOf AutoCompMateriau
        del.Invoke()

        Me.AutoCompNumCmd.SelectedItem = Nothing
        Me.AutoCompNumCmd.Text = ""
    End Sub

#End Region

#Region "Delegates"

    ''' <summary>
    ''' Delegate du selectionChanged de l'AutoCompleteBox client
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub cbxClientChanged()

    ''' <summary>
    ''' Delegate du selectionChanged de l'AutoCompleteBox contremarque
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub cbxCmqChanged()

    ''' <summary>
    ''' Delegate du selectionChanged de l'AutoCompleteBox n° de commande
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub cbxNumCmdChanged()

#End Region

#Region "SelectionChanged"

    ''' <summary>
    ''' Se produit lorque la valeur du ckeckbox ChkEndCmd change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChkEndCmd_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        LbxSearchCmd.Items.Clear()
    End Sub

#End Region

#Region "EventControlEnter"

    ''' <summary>
    ''' Évènement se produisant lorsque qu'une touche est enfoncée sur une AutoCompleteBox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AutoComp_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        Dim autoComp As AutoCompleteBox = sender

        If e.Key <> Key.Enter And e.Key <> Key.Return And e.Key <> Key.Back And e.Key <> Key.LeftCtrl <> e.Key <> Key.RightAlt Then
            If autoComp.Text.Length >= 50 Then
                e.Handled = True
            End If
        End If
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Réinitialise le UserControl
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Reinitialize()
        Me.AutoCompNClient.SelectedItem = Nothing
        Me.AutoCompNContremarque.SelectedItem = Nothing
        Me.AutoCompNumCmd.SelectedItem = Nothing
        Me.AutoCompLMateriau.SelectedItem = Nothing
        Me.CbxEtat.SelectedIndex = 0
        Me.LbxSearchCmd.Items.Clear()
    End Sub

#End Region

End Class
