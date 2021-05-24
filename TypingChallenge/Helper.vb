Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions

Module Helper

    <Extension>
    Public Sub DrawGDIPlusText(graphics As Graphics, text As String, font As Font, bounds As RectangleF, color As Color, Optional alignment As StringAlignment = StringAlignment.Center, Optional offset As Point = Nothing)
        Dim format As New StringFormat()
        format.Alignment = alignment

        Using myBrush As New SolidBrush(color)
            If Not offset = Nothing Then bounds.Offset(offset)
            graphics.DrawString(text, font, myBrush, bounds, format)
        End Using

        graphics.ResetTransform()
    End Sub

    <Extension>
    Public Function WordCount(text As String) As Integer
        Dim collection As MatchCollection = Regex.Matches(text, "\S+")
        Return collection.Count
    End Function

End Module
