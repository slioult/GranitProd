Public Class WorkBackgroundConverter
    Implements System.Windows.Data.IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim cmdw As CommandeWork = CType(value, CommandeWork)
        Dim color As String = "Transparent"

        If DateEquals(cmdw.D, cmdw.DateFinalisations) Then
            For Each F In cmdw.Finalisations
                color = F.Color
            Next
        ElseIf DateEquals(cmdw.D, cmdw.DateMesure) Then
            color = cmdw.Mesure.Color
        End If

        Return color
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
