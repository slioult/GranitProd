Public Class TpsFabricationConverter
    Implements System.Windows.Data.IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value IsNot Nothing Then
            Dim minutes As Integer = 0

            If TypeOf (value) Is Commande Then
                Dim cmd As Commande = CType(value, Commande)
                minutes = cmd.TpsDebit + cmd.TpsCommandeNumerique + cmd.TpsFinition + cmd.TpsAutres
            ElseIf TypeOf (value) Is Integer Then
                minutes = value
            End If

            Dim temps() As String = convertMinuteToHourMinute(minutes).Split(";")

            If (Integer.Parse(temps(0)) > 0 And Integer.Parse(temps(1)) > 0) Then
                Return temps(0) + " heures " + temps(1) + " minutes"
            ElseIf (Integer.Parse(temps(0)) > 0) Then
                Return temps(0) + " heures"
            ElseIf (Integer.Parse(temps(1)) > 0) Then
                Return temps(1) + " minutes"
            Else
                If TypeOf (value) Is Commande Then
                    Return "Non défini"
                Else
                    Return 0
                End If
            End If
        Else
            Return ""
        End If

    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return Nothing
    End Function

    ''' <summary>
    ''' Convertit un nombre de minutes en Heure et Minute.
    ''' </summary>
    ''' <param name="min">Nombre de minutes à convertir</param>
    ''' <returns>Retourne une chaîne de caracère contenant les heures et les minutes séparées par le caractères ';'</returns>
    ''' <remarks></remarks>
    Private Function convertMinuteToHourMinute(ByVal min As Integer) As String
        Dim result As String = String.Empty

        Dim hour As Integer
        Dim minute As Integer

        minute = min Mod 60
        hour = (min - minute) / 60

        result = hour.ToString() + ";" + minute.ToString()
        Return result
    End Function
End Class
