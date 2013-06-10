Public Class PlVisibilityConverter
    Implements System.Windows.Data.IValueConverter


    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim cmdw As CommandeWork = CType(value, CommandeWork)
        Dim visibility As Visibility = visibility.Visible

        If DateEquals(cmdw.D, cmdw.DateMesure) Then
            visibility = Windows.Visibility.Collapsed
        End If

        Return visibility
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
