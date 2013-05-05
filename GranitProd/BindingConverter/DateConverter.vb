Public Class DateConverter
    Implements System.Windows.Data.IValueConverter


    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim d As DateTime = CType(value, DateTime)
        Dim p As New PlanningControl()
        Dim sem As Integer = p.GetWeekOfDate(d)
        Dim day As String = IIf(d.Day < 10, "0" + d.Day.ToString(), d.Day.ToString())
        Dim month As String = IIf(d.Month < 10, "0" + d.Month.ToString(), d.Month.ToString())

        Return day + "/" + month.ToString() + "/" + d.Year.ToString() + " sem " + sem.ToString()
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return Nothing
    End Function
End Class
