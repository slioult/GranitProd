Public Class ChiffreConverter
    Implements System.Windows.Data.IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim prix As String = CType(value, String)
        Dim tempPrix As String = prix.Substring(0, prix.Length - 3)
        Dim tempIndex As Integer = tempPrix.Length

        tempIndex -= 3

        While tempIndex > 0
            tempPrix = tempPrix.Insert(tempIndex, " ")
            tempIndex -= 3
        End While

        prix = tempPrix + prix.Substring(prix.Length - 3)

        Return prix.Replace(".", ",") + " €"
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return Nothing
    End Function
End Class
