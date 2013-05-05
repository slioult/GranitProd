Public Class NaturesConverter
    Implements System.Windows.Data.IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim natures As List(Of Nature) = CType(value, List(Of Nature))
        Dim natString As String = String.Empty

        If natures IsNot Nothing Then
            For Each nat In natures
                If natString = String.Empty Then
                    natString = nat.Label
                Else
                    natString = natString + ", " + nat.Label
                End If
            Next
        End If

        Return natString
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return Nothing
    End Function
End Class
