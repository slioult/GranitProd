Public Class WarningRemarquesConverter
    Implements System.Windows.Data.IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value IsNot Nothing Then
            Dim remarques As List(Of Remarque) = CType(value, List(Of Remarque))

            If remarques.Count > 0 Then
                Dim rm As Remarque = remarques.ElementAt(remarques.Count - 1)

                Return "ATTENTION --> Dernière remarque le " + rm.DatePost.Substring(0, 10) + " à " + rm.DatePost.Substring(11, 5) + " par " + rm.Source
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return Nothing
    End Function
End Class
