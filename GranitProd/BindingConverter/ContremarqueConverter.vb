Public Class ContremarqueConverter
    Implements System.Windows.Data.IValueConverter


    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim contremarque As Contremarque = CType(value, Contremarque)

        If (contremarque IsNot Nothing) Then
            Return contremarque.Nom
        Else
            Return "Aucune"
        End If
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return Nothing
    End Function
End Class
