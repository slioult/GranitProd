Imports Excel = Microsoft.Office.Interop.Excel
Imports Microsoft.Office.Interop.Excel
Imports System.IO

Public Module ExcelExport

    ''' <summary>
    ''' Réalise l'export des commande grâce à une application Excel
    ''' </summary>
    ''' <param name="cmds">Liste des commandes à exporter</param>
    ''' <param name="search">Différents critères de la recherches ayant abouti à ces résultats</param>
    ''' <param name="etatCmd">État des commandes (Terminée, Rendue ou en cours)</param>
    ''' <param name="format">Format de l'export (PDF ou EXCEL)</param>
    ''' <remarks></remarks>
    Public Sub ExportCommande(ByVal cmds As List(Of Commande), ByVal search As String, ByVal etatCmd As String, ByVal format As String)
        'Instancie une nouvelle application EXCEL
        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet
        Dim misValue As Object = System.Reflection.Missing.Value
        Dim chartRange As Excel.Range
        Dim procId As Integer = 0

        Try
            Dim minute As String
            Dim heure As String
            Dim jour As String
            Dim mois As String

            'Récupère la liste des processus à l'instant
            Dim p() As Process = Process.GetProcesses()
            'Crée un processus Excel
            xlApp = New Excel.Application
            'Récupère la nouvelle liste des processus
            Dim p2() As Process = Process.GetProcesses()

            'En déduit l'id du processus créé
            procId = GetProcId(p, p2)

            'Paramètre la feuille Excel
            xlWorkBook = xlApp.Workbooks.Add(misValue)
            xlWorkSheet = xlWorkBook.Sheets("Feuil1")
            xlWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape
            xlWorkSheet.Cells.WrapText = True

            Dim l As Integer = 3
            Dim c As Integer = 0

            'Parcours la liste de commandes passée en paramètre et rempli une colonne par commande
            For Each cmd In cmds
                l += 1
                xlWorkSheet.Cells(l, 1) = cmd.NoCommande
                If cmd.DateCommande.Day < 10 Then jour = "0" + cmd.DateCommande.Day.ToString() Else jour = cmd.DateCommande.Day.ToString()
                If cmd.DateCommande.Month < 10 Then mois = "0" + cmd.DateCommande.Month.ToString() Else mois = cmd.DateCommande.Month.ToString()
                xlWorkSheet.Cells(l, 2) = jour + "/" + mois + "/" + cmd.DateCommande.Year.ToString() + Environment.NewLine +
                    " sem " + New PlanningControl(True).GetWeekOfDate(cmd.DateCommande).ToString()
                xlWorkSheet.Cells(l, 3) = cmd.Client.Nom
                If cmd.Contremarque IsNot Nothing Then
                    xlWorkSheet.Cells(l, 4) = cmd.Contremarque.Nom
                Else
                    xlWorkSheet.Cells(l, 4) = "Aucune"
                End If
                Dim natures As String = String.Empty
                For Each nat In cmd.Natures
                    If natures = String.Empty Then
                        natures = nat.Label
                    Else
                        natures = natures + " / " + Environment.NewLine + nat.Label
                    End If
                Next
                xlWorkSheet.Cells(l, 5) = natures
                Dim materiaux As String = String.Empty
                For Each mat In cmd.Materiaux
                    If materiaux = String.Empty Then
                        materiaux = mat.Label + " (" + mat.Epaisseur.ToString() + " mm)"
                    Else
                        materiaux = materiaux + " / " + Environment.NewLine + mat.Label + " (" + mat.Epaisseur.ToString() + " mm)"
                    End If
                Next
                xlWorkSheet.Cells(l, 6) = materiaux

                If cmd.DateMesure.Day < 10 Then jour = "0" + cmd.DateMesure.Day.ToString() Else jour = cmd.DateMesure.Day.ToString()
                If cmd.DateMesure.Month < 10 Then mois = "0" + cmd.DateMesure.Month.ToString() Else mois = cmd.DateMesure.Month.ToString()
                If cmd.DateMesure.Hour < 10 Then heure = "0" + cmd.DateMesure.Hour.ToString() Else heure = cmd.DateMesure.Hour.ToString()
                If cmd.DateMesure.Minute < 10 Then minute = "0" + cmd.DateMesure.Minute.ToString() Else minute = cmd.DateMesure.Minute.ToString()

                xlWorkSheet.Cells(l, 7) = cmd.Mesure.Label + Environment.NewLine + jour + "/" + mois + "/" +
                    cmd.DateMesure.Year.ToString() + Environment.NewLine + heure + "h" + minute
                xlWorkSheet.Cells(l, 8) = cmd.MontantHT.ToString() + " €"

                If cmd.DelaiPrevu.Day < 10 Then jour = "0" + cmd.DelaiPrevu.Day.ToString() Else jour = cmd.DelaiPrevu.Day.ToString()
                If cmd.DelaiPrevu.Month < 10 Then mois = "0" + cmd.DelaiPrevu.Month.ToString() Else mois = cmd.DelaiPrevu.Month.ToString()
                If cmd.DelaiPrevu.Hour < 10 Then heure = "0" + cmd.DelaiPrevu.Hour.ToString() Else heure = cmd.DelaiPrevu.Hour.ToString()
                If cmd.DelaiPrevu.Minute < 10 Then minute = "0" + cmd.DelaiPrevu.Minute.ToString() Else minute = cmd.DelaiPrevu.Minute.ToString()

                xlWorkSheet.Cells(l, 9) = jour + "/" + mois + "/" + cmd.DelaiPrevu.Year.ToString() + Environment.NewLine + " sem " + New PlanningControl(True).GetWeekOfDate(cmd.DelaiPrevu).ToString()
                Dim prestations As String = String.Empty
                For Each fin In cmd.Finalisations
                    If prestations = String.Empty Then
                        prestations = fin.Label
                    Else
                        prestations = prestations + " / " + Environment.NewLine + fin.Label
                    End If
                Next
                xlWorkSheet.Cells(l, 10) = prestations

                If cmd.DateFinalisations.Day < 10 Then jour = "0" + cmd.DateFinalisations.Day.ToString() Else jour = cmd.DateFinalisations.Day.ToString()
                If cmd.DateFinalisations.Month < 10 Then mois = "0" + cmd.DateFinalisations.Month.ToString() Else mois = cmd.DateFinalisations.Month.ToString()
                If cmd.DateFinalisations.Hour < 10 Then heure = "0" + cmd.DateFinalisations.Hour.ToString() Else heure = cmd.DateFinalisations.Hour.ToString()
                If cmd.DateFinalisations.Minute < 10 Then minute = "0" + cmd.DateFinalisations.Minute.ToString() Else minute = cmd.DateFinalisations.Minute.ToString()

                xlWorkSheet.Cells(l, 11) = jour + "/" + mois + "/" + cmd.DateFinalisations.Year.ToString() + Environment.NewLine +
                    heure + "h" + minute + Environment.NewLine +
                    " sem " + New PlanningControl(True).GetWeekOfDate(cmd.DateFinalisations).ToString()
            Next

            'Renseigne les noms de colonne
            xlWorkSheet.Cells(3, 1) = "N°"
            xlWorkSheet.Cells(3, 2) = "Date cmd"
            xlWorkSheet.Cells(3, 3) = "Client"
            xlWorkSheet.Cells(3, 4) = "CM"
            xlWorkSheet.Cells(3, 5) = "Natures"
            xlWorkSheet.Cells(3, 6) = "Matériaux"
            xlWorkSheet.Cells(3, 7) = "Relevé"
            xlWorkSheet.Cells(3, 8) = "Montant"
            xlWorkSheet.Cells(3, 9) = "Délai prévu"
            xlWorkSheet.Cells(3, 10) = "Prestations"
            xlWorkSheet.Cells(3, 11) = "Date Prest°"

            ' Formate les cellules
            chartRange = xlWorkSheet.Range("a1", "k" + l.ToString())
            chartRange.HorizontalAlignment = 3
            chartRange.VerticalAlignment = 2
            chartRange.Font.Size = 10

            For i = 4 To l
                chartRange = xlWorkSheet.Range("a" + i.ToString(), "k" + i.ToString())
                chartRange.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThin, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic)
                chartRange.Font.Size = 8
            Next

            chartRange = xlWorkSheet.Range("a4", "k" + l.ToString())
            chartRange.BorderAround(Excel.XlLineStyle.xlDouble, Excel.XlBorderWeight.xlThin, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic)

            chartRange = xlWorkSheet.Range("a1", "k1")
            chartRange.Merge()
            chartRange.FormulaR1C1 = search

            chartRange = xlWorkSheet.Range("d2", "h2")
            chartRange.Merge()
            chartRange.FormulaR1C1 = etatCmd

            'Ajoute le logo de l'entreprise en haut à gauche
            xlWorkSheet.Shapes.AddPicture(System.IO.Path.GetFullPath(My.Settings.Logo), Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, 10, 5, 100, 20)

            chartRange = xlWorkSheet.Range("i2", "k2")
            chartRange.Merge()
            If Date.Now.Day < 10 Then jour = "0" + Date.Now.Day.ToString() Else jour = Date.Now.Day.ToString()
            If Date.Now.Month < 10 Then mois = "0" + Date.Now.Month.ToString() Else mois = Date.Now.Month.ToString()
            If Date.Now.Hour < 10 Then heure = "0" + Date.Now.Hour.ToString() Else heure = Date.Now.Hour.ToString()
            If Date.Now.Minute < 10 Then minute = "0" + Date.Now.Minute.ToString() Else minute = Date.Now.Minute.ToString()
            chartRange.FormulaR1C1 = "Émis le " + jour + "/" + mois + "/" + Date.Now.Year.ToString() + " à " + heure + "h" + minute
            chartRange.HorizontalAlignment = 4
            chartRange.Font.Size = 8

            chartRange = xlWorkSheet.Range("a1", "k3")
            chartRange.Font.Bold = True

            chartRange = xlWorkSheet.Range("a3", "k3")
            chartRange.BorderAround(Excel.XlLineStyle.xlDouble, Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic)

            'Exporte au format XLSX
            If format = "XLSX" Then
                If System.IO.File.Exists(System.IO.Path.GetFullPath(My.Settings.ExportFile + "\m-granit.xlsx")) Then
                    System.IO.File.Delete(System.IO.Path.GetFullPath(My.Settings.ExportFile + "\m-granit.xlsx"))
                End If
                xlWorkSheet.SaveAs(System.IO.Path.GetFullPath(My.Settings.ExportFile + "\m-granit.xlsx"))

                xlApp.Visible = True

                'Exporte au format PDF
            ElseIf format = "PDF" Then
                Dim paramExportFilePath As String = System.IO.Path.GetFullPath(My.Settings.ExportFile + "\m-granit.pdf")
                Dim paramExportFormat As XlFixedFormatType = _
                    XlFixedFormatType.xlTypePDF
                Dim paramExportQuality As XlFixedFormatQuality = _
                    XlFixedFormatQuality.xlQualityStandard
                Dim paramOpenAfterPublish As Boolean = True
                Dim paramIncludeDocProps As Boolean = True
                Dim paramIgnorePrintAreas As Boolean = True
                Dim paramFromPage As Object = Type.Missing
                Dim paramToPage As Object = Type.Missing
                xlWorkSheet.ExportAsFixedFormat(paramExportFormat, _
                paramExportFilePath, paramExportQuality, _
                paramIncludeDocProps, paramIgnorePrintAreas, _
                paramFromPage, paramToPage, paramOpenAfterPublish)
                xlWorkSheet.SaveAs(System.IO.Path.GetFullPath(My.Settings.ExportFile + "\tempPdf.pdf"))

                xlWorkBook.Close()
                xlApp.Quit()

                System.IO.File.Delete(System.IO.Path.GetFullPath(My.Settings.ExportFile + "\tempPdf.pdf"))
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
            Dim sw As New StreamWriter(My.Settings.ConfigFiles + "\log.txt")

            If procId <> 0 Then
                Process.GetProcessById(procId).Kill()
                procId = 0
            End If

            'Catch l'erreur dans un fichier LOG
            Dim content As String = "ExportExcel" + vbCrLf + ex.StackTrace.ToString() + vbCrLf + vbCrLf + ex.Source.ToString()
            If ex.InnerException IsNot Nothing Then
                content = content + vbCrLf + vbCrLf + ex.InnerException.ToString()
            End If

            content = content + vbCrLf + System.IO.Path.GetFullPath(My.Settings.Logo) + vbCrLf + vbCrLf + "/ExportExcel"

            sw.Write(content)

            sw.Close()
        Finally
            'Libère les ressources
            xlWorkBook = Nothing
            xlApp = Nothing
            'Appelle le Garbage Collector afin de libérer plus rapidement les ressources
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Sub

    ''' <summary>
    ''' Permet de récupérer l'id du processus Excel créé
    ''' </summary>
    ''' <param name="Process1">Liste des processus avant la création du processus Excel</param>
    ''' <param name="Process2">Liste des processus après la création du processus Excel</param>
    ''' <returns>Retourne l'id du processus Excel créé</returns>
    ''' <remarks></remarks>
    Public Function GetProcId(ByVal Process1 As Process(), ByVal Process2 As Process()) As Integer
        Dim ProcId% = 0

        Dim i%, j%
        Dim bMonProcessXL As Boolean

        For j = 0 To Process2.GetUpperBound(0)
            If Process2(j).ProcessName = "EXCEL" Then
                bMonProcessXL = True

                'Listing des processus avant la création Excel®
                For i = 0 To Process1.GetUpperBound(0)
                    If Process1(i).ProcessName = "EXCEL" Then
                        If Process2(j).Id = Process1(i).Id Then

                            ' S'il existait avant, ce n'est pas celui recherché
                            bMonProcessXL = False
                            Exit For
                        End If
                    End If
                Next i

                If bMonProcessXL = True Then
                    'Recopie de l’identifiant du processus créé
                    ProcId = Process2(j).Id
                    Exit For
                End If
            End If
        Next j

        Return ProcId
    End Function

End Module
