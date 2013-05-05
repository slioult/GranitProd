Public Class BackgroundRemarquesConverter
    Implements System.Windows.Data.IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value IsNot Nothing Then
            Dim remarques As List(Of Remarque) = CType(value, List(Of Remarque))

            If remarques.Count > 0 Then
                Return "Orange"
            Else
                Return "White"
            End If
        Else
            Return "White"
        End If
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return Nothing
    End Function
End Class
