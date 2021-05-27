Imports System.Drawing.Drawing2D
Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions

Module Helper

    Public lvlXmlPath As String = ".\data\level.xml"
    Public setXmlPath As String = ".\data\setting.xml"
    Public levels As LevelData = New LevelData(lvlXmlPath).Instance
    Public setting As SettingData = New SettingData(setXmlPath).Instance

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
    Public Sub DrawGDIText(graphics As Graphics, text As String, font As Font, bounds As Rectangle, color As Color, Optional flags As TextFormatFlags = TextFormatFlags.VerticalCenter, Optional offset As Point = Nothing)
        TextRenderer.DrawText(graphics, text, font, bounds, color, flags)
    End Sub

    <Extension>
    Public Function WordCount(text As String) As Integer
        Dim collection As MatchCollection = Regex.Matches(text, "\S+")
        Return collection.Count
    End Function

    <Extension>
    Public Function SecondsToTime(sec As Integer) As String
        Dim ts As TimeSpan = TimeSpan.FromSeconds(sec)

        Dim mydate As Date = New Date(ts.Ticks)
        Return mydate.ToString(("mm:ss"))
    End Function

    <Extension>
    Public Sub DrawRoundedRectangle(ByVal g As Graphics, ByVal r As Rectangle, ByVal d As Integer, ByVal p As Pen)
        Dim mode As SmoothingMode = g.SmoothingMode
        Dim iMode As InterpolationMode = g.InterpolationMode
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.InterpolationMode = InterpolationMode.High
        g.DrawArc(p, r.X, r.Y, d, d, 180, 90)
        g.DrawLine(p, CInt(r.X + d / 2), r.Y, CInt(r.X + r.Width - d / 2), r.Y)
        g.DrawArc(p, r.X + r.Width - d, r.Y, d, d, 270, 90)
        g.DrawLine(p, r.X, CInt(r.Y + d / 2), r.X, CInt(r.Y + r.Height - d / 2))
        g.DrawLine(p, CInt(r.X + r.Width), CInt(r.Y + d / 2), CInt(r.X + r.Width), CInt(r.Y + r.Height - d / 2))
        g.DrawLine(p, CInt(r.X + d / 2), CInt(r.Y + r.Height), CInt(r.X + r.Width - d / 2), CInt(r.Y + r.Height))
        g.DrawArc(p, r.X, r.Y + r.Height - d, d, d, 90, 90)
        g.DrawArc(p, r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90)
        g.SmoothingMode = mode
        g.InterpolationMode = iMode
    End Sub

    <Extension>
    Public Sub FillRoundedRectangle(ByVal g As Graphics, ByVal r As Rectangle, ByVal d As Integer, ByVal b As Brush, corner As RoundedRectCorners)
        Dim mode As SmoothingMode = g.SmoothingMode
        Dim iMode As InterpolationMode = g.InterpolationMode
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.InterpolationMode = InterpolationMode.High
        If corner.TopLeft Then g.FillPie(b, r.X, r.Y, d, d, 180, 90)
        If corner.TopRight Then g.FillPie(b, r.X + r.Width - d, r.Y, d, d, 270, 90)
        If corner.BottomLeft Then g.FillPie(b, r.X, r.Y + r.Height - d, d, d, 90, 90)
        If corner.BottomRight Then g.FillPie(b, r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90)
        If Not corner.TopLeft Then g.FillRectangle(b, r.X, r.Y, d, d)
        If Not corner.TopRight Then g.FillRectangle(b, r.X + r.Width - d, r.Y, d, d)
        If Not corner.BottomLeft Then g.FillRectangle(b, r.X, r.Y + r.Height - d, d, d)
        If Not corner.BottomRight Then g.FillRectangle(b, r.X + r.Width - d, r.Y + r.Height - d, d, d)
        g.FillRectangle(b, CInt(r.X - 1 + d / 2), r.Y, r.Width + 2 - d, CInt(d / 2))
        g.FillRectangle(b, r.X, CInt(r.Y - 1 + d / 2), r.Width, CInt(r.Height + 2 - d))
        g.FillRectangle(b, CInt(r.X - 1 + d / 2), CInt(r.Y + r.Height - d / 2), CInt(r.Width + 2 - d), CInt(d / 2))
        g.SmoothingMode = mode
        g.InterpolationMode = iMode
    End Sub

    <Extension>
    Public Function ToRectangle(rectF As RectangleF) As Rectangle
        Return New Rectangle(CInt(rectF.X), CInt(rectF.Y), CInt(rectF.Width), CInt(rectF.Height))
    End Function

End Module
