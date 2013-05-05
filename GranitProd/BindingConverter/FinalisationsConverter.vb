Public Class FinalisationsConverter
    Implements System.Windows.Data.IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim finalisations As List(Of Finalisation) = CType(value, List(Of Finalisation))
        Dim finString As String = String.Empty

        If finalisations IsNot Nothing Then
            For Each fin In finalisations
                If finString = String.Empty Then
                    finString = fin.Label
                Else
                    finString = finString + ", " + fin.Label
                End If
            Next
        End If

        Return finString
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return Nothing
    End Function
End Class
