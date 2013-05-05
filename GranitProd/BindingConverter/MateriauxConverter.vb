Public Class MateriauxConverter
    Implements System.Windows.Data.IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim materiaux As List(Of Materiau) = CType(value, List(Of Materiau))
        Dim matString As String = String.Empty

        If materiaux IsNot Nothing Then
            For Each mat In materiaux
                If matString = String.Empty Then
                    matString = mat.Label + " (" + mat.Epaisseur.ToString() + " mm)"
                Else
                    matString = matString + ", " + mat.Label + " (" + mat.Epaisseur.ToString() + " mm)"
                End If
            Next
        End If

        Return matString
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return Nothing
    End Function
End Class
