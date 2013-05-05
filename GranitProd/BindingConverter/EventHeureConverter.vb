Public Class EventHeureConverter
    Implements System.Windows.Data.IValueConverter


    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim cmdw As CommandeWork = CType(value, CommandeWork)
        Dim heure As String = "Non définie"
        Dim h As String
        Dim m As String

        If DateEquals(cmdw.D, cmdw.DateFinalisations) And cmdw.DateFinalisations.Hour <> 0 Or DateEquals(cmdw.D, cmdw.DateFinalisations) And cmdw.DateFinalisations.Minute <> 0 Then
            h = IIf(cmdw.DateFinalisations.Hour < 10, "0" + cmdw.DateFinalisations.Hour.ToString(), cmdw.DateFinalisations.Hour.ToString())
            m = IIf(cmdw.DateFinalisations.Minute < 10, "0" + cmdw.DateFinalisations.Minute.ToString(), cmdw.DateFinalisations.Minute.ToString())
            heure = h + "h" + m
        ElseIf DateEquals(cmdw.D, cmdw.DateMesure) And cmdw.DateMesure.Hour <> 0 Or DateEquals(cmdw.D, cmdw.DateMesure) And cmdw.DateMesure.Minute <> 0 Then
            h = IIf(cmdw.DateMesure.Hour < 10, "0" + cmdw.DateMesure.Hour.ToString(), cmdw.DateMesure.Hour.ToString())
            m = IIf(cmdw.DateMesure.Minute < 10, "0" + cmdw.DateMesure.Minute.ToString(), cmdw.DateMesure.Minute.ToString())
            heure = h + "h" + m
        End If

        Return heure
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return Nothing
    End Function

    Public Function DateEquals(ByVal d As DateTime, ByVal d2 As DateTime) As Boolean
        If d.Day = d2.Day And d.Month = d2.Month And d.Year = d2.Year Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
