Public Class MyCompleteBox
    Inherits AutoCompleteBox

    Public Overrides Sub OnApplyTemplate()
        MyBase.OnApplyTemplate()

        Dim list As New List(Of Object)
        Dim list2 As New List(Of Object)
        list.Add(SelectedItem)
        Dispatcher.BeginInvoke(New Action(Sub() OnSelectionChanged(New SelectionChangedEventArgs(SelectionChangedEvent, list2, list))))
    End Sub

End Class
