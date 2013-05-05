﻿Imports MGranitDALcsharp
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class ConfNature


    Dim connection As New MGConnection(My.Settings.DBSource)

#Region "Fields"

    Private _NouvelleCommande As NouvelleCommande

#End Region

#Region "Properties"

    Public Property NouvelleCommande As NouvelleCommande
        Get
            Return Me._NouvelleCommande
        End Get
        Set(ByVal value As NouvelleCommande)
            Me._NouvelleCommande = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Me.CbxConfNature.Items.Add(New Nature("Nouveau"))

        Dim natures As New List(Of Nature)
        natures = Nature.GetNatures()

        For Each e In natures
            Me.CbxConfNature.Items.Add(e)
        Next

        Me.CbxConfNature.SelectedIndex = 0
    End Sub

#End Region

#Region "Button"

    ''' <summary>
    ''' Bouton de suppresion d'un nature
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Me.CbxConfNature.SelectedIndex > 0 Then
            Dim nature As Nature = Me.CbxConfNature.SelectedItem
            If Not nature.IsUsed Then
                Dim question As MessageBoxResult = MessageBox.Show("Voulez vous vraiment supprimer la nature selectionnée", "Caution", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                If question = MessageBoxResult.Yes Then
                    Me.CbxConfNature.Items.Remove(Me.CbxConfNature.SelectedItem)
                    nature.Delete()
                    Dim nt As New NatureTemplate(nature, False)
                    Me.NouvelleCommande.LbxNatures.Items.Remove(nt)
                    Me.CbxConfNature.SelectedIndex = 0
                End If
            Else
                MessageBox.Show("La nature est utilisée dans une commande et ne peut pas etre supprimer")
            End If
        Else
            MessageBox.Show("Selectionnez une nature a supprimé")
        End If
    End Sub

    ''' <summary>
    ''' Bouton de sauvegarde d'un nature
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Me.CbxConfNature.SelectedIndex = 0 And TxtNomNature.Text <> "" Then
            Dim nature As New Nature(TxtNomNature.Text)

            Dim isExists As Boolean = False
            For Each item In Me.CbxConfNature.Items
                Dim tempNature As Nature = item
                If tempNature.Label.ToUpper() = nature.Label.ToUpper() Then
                    isExists = True
                    Exit For
                End If
            Next

            If Not isExists Then
                nature.Identifier = nature.Insert()
                Me.CbxConfNature.Items.Add(nature)
                Me.CbxConfNature.SelectedItem = nature
                Dim nt As New NatureTemplate(nature, False)
                Me.NouvelleCommande.LbxNatures.Items.Add(nt)
                MessageBox.Show("La nature a été ajouté")
            Else
                MessageBox.Show("La nature existe")
            End If
        ElseIf Me.CbxConfNature.SelectedIndex > 0 And TxtNomNature.Text <> "" Then
            Dim index = Me.CbxConfNature.SelectedIndex
            Dim nature As Nature = Me.CbxConfNature.SelectedItem

            Dim isExists As Boolean = False
            For Each item In Me.CbxConfNature.Items
                Dim tempNature As Nature = item
                If tempNature.Label.ToUpper() = TxtNomNature.Text.ToUpper() Then
                    isExists = True
                    Exit For
                End If
            Next

            If Not isExists Then
                nature.Label = TxtNomNature.Text
                nature.Update()

                Me.CbxConfNature.Items.RemoveAt(index)
                Me.CbxConfNature.Items.Insert(index, nature)

                Me.CbxConfNature.SelectedIndex = index
                MessageBox.Show("La nature a été modifié")
            Else
                MessageBox.Show("La nature existe")
            End If

        End If
    End Sub

#End Region

End Class
