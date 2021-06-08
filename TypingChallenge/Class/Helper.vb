Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Net
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports TypingChallenge

Module Helper

    Public lvlXmlPath As String = ".\data\level.xml"
    Public setXmlPath As String = ".\data\setting.xml"
    Public levels As LevelData = New LevelData(lvlXmlPath).Instance
    Public setting As SettingData = New SettingData(setXmlPath).Instance

    Public gameFormState As FormState = New FormState()

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
    Public Sub DrawGDIPlusText(graphics As Graphics, text As String, font As Font, bounds As RectangleF, brush As Brush, Optional alignment As StringAlignment = StringAlignment.Center, Optional offset As Point = Nothing)
        Dim format As New StringFormat()
        format.Alignment = alignment

        If Not offset = Nothing Then bounds.Offset(offset)
        graphics.DrawString(text, font, brush, bounds, format)

        graphics.ResetTransform()
    End Sub

    <Extension>
    Public Sub DrawGDIText(graphics As Graphics, text As String, font As Font, bounds As Rectangle, color As Color, Optional flags As TextFormatFlags = TextFormatFlags.VerticalCenter, Optional offset As Point = Nothing)
        TextRenderer.DrawText(graphics, text, font, bounds, color, flags)
    End Sub

    <Extension>
    Public Function WordCount(text As String) As Integer
        text = text.Replace("\n\n", vbNewLine)
        Dim collection As MatchCollection = Regex.Matches(text, "\S+")
        Return collection.Count
    End Function

    <Extension>
    Public Function SecondsToTime(sec As Integer) As String
        Dim ts As TimeSpan = TimeSpan.FromSeconds(sec)

        Dim mydate As Date = New Date(ts.Ticks)
        Return mydate.ToString(("mm:ss"))
    End Function

    Public Function GetColor(bmp As Bitmap, x As Integer, y As Integer) As Color
        Dim c As Color = bmp.GetPixel(x, y)
        Return c
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
    Public Function DrawImageWithRoundedCorners(image As Image, d As Integer, backColor As Color) As Image
        Dim roundedImage As New Bitmap(image.Width, image.Height)
        Using g As Graphics = Graphics.FromImage(roundedImage)
            g.Clear(backColor)
            g.SmoothingMode = SmoothingMode.AntiAlias
            Dim brush As New TextureBrush(image)
            Dim gp As New GraphicsPath
            gp.AddArc(0, 0, d, d, 180, 90)
            gp.AddArc(0 + roundedImage.Width - d, 0, d, d, 270, 90)
            gp.AddArc(0 + roundedImage.Width - d, 0 + roundedImage.Height - d, d, d, 0, 90)
            gp.AddArc(0, 0 + roundedImage.Height - d, d, d, 90, 90)
            g.FillPath(brush, gp)
            Return roundedImage
        End Using
    End Function

    <Extension>
    Public Function ToRectangle(rectF As RectangleF) As Rectangle
        Return New Rectangle(CInt(rectF.X), CInt(rectF.Y), CInt(rectF.Width), CInt(rectF.Height))
    End Function

    <Extension>
    Public Sub DrawSliderControl(graphics As Graphics, font As Font, ByRef refRectL As RectangleF, ByRef refRectR As RectangleF, ByRef refBoolL As Boolean, ByRef refBoolR As Boolean,
                                      location As PointF, size As SizeF, title As String, optText As String, Optional color As Color = Nothing, Optional color2 As Color = Nothing, Optional maxBtnSize As Integer = 75)
        If color2 = Nothing Then color2 = Color.Red
        If color = Nothing Then color = Color.White
        Dim btnSize As Integer = size.Height
        If size.Height > maxBtnSize Then btnSize = maxBtnSize

        Using brush As New SolidBrush(color)
            Dim rect1 As New RectangleF(location.X, location.Y, size.Width, btnSize)
            graphics.DrawGDIPlusText(title, font, rect1, color, StringAlignment.Near)
            Dim rect2 As New RectangleF(location.X + size.Width, location.Y, size.Width, btnSize)
            refRectL = New RectangleF(rect2.Location.X, rect2.Location.Y, btnSize, btnSize)
            If refBoolL Then
                graphics.FillRoundedRectangle(refRectL.ToRectangle, 10, brush, New RoundedRectCorners(True))
                graphics.DrawGDIPlusText("◀", font, refRectL, color2, StringAlignment.Center)
            Else
                graphics.DrawGDIPlusText("◀", font, refRectL, color, StringAlignment.Center)
            End If
            graphics.DrawGDIPlusText(optText, font, rect2, color, StringAlignment.Center)
            refRectR = New RectangleF(rect2.X + rect2.Width - btnSize, rect2.Y, btnSize, btnSize)
            If refBoolR Then
                graphics.FillRoundedRectangle(refRectR.ToRectangle, 10, Brushes.White, New RoundedRectCorners(True))
                graphics.DrawGDIPlusText("▶", font, refRectR, color2, StringAlignment.Center)
            Else
                graphics.DrawGDIPlusText("▶", font, refRectR, color, StringAlignment.Center)
            End If
        End Using
    End Sub

    <Extension>
    Public Sub DrawButtonControl(graphics As Graphics, font As Font, ByRef refRect As RectangleF, ByRef refBool As Boolean, location As PointF, size As SizeF, title As String, optText As String, Optional color As Color = Nothing, Optional color2 As Color = Nothing, Optional maxBtnSize As Integer = 75)
        If color2 = Nothing Then color2 = Color.Red
        If color = Nothing Then color = Color.White
        Dim btnSize As Integer = size.Height
        If size.Height > maxBtnSize Then btnSize = maxBtnSize

        Using brush As New SolidBrush(color)
            Dim rect1 As New RectangleF(location.X, location.Y, size.Width, btnSize)
            graphics.DrawGDIPlusText(title, font, rect1, color, StringAlignment.Near)
            Dim rect2 As New RectangleF(location.X + size.Width, location.Y, size.Width, btnSize)
            refRect = New RectangleF(rect2.Location.X, rect2.Location.Y, rect2.Width, btnSize)
            If refBool Then
                graphics.FillRoundedRectangle(refRect.ToRectangle, 10, brush, New RoundedRectCorners(True))
                graphics.DrawGDIPlusText(optText, font, refRect, color2, StringAlignment.Center)
            Else
                graphics.DrawGDIPlusText(optText, font, refRect, color, StringAlignment.Center)
            End If
        End Using
    End Sub

    <Extension>
    Public Sub DrawLevelSelectionControl(graphics As Graphics, font As Font, ByRef refRect As RectangleF, ByRef refBool As Boolean, location As PointF, size As SizeF,
                                         level As Level, Optional color As Color = Nothing, Optional color2 As Color = Nothing)
        If color2 = Nothing Then color2 = Color.Red
        If color = Nothing Then color = Color.White

        Using brush As New SolidBrush(color)
            refRect = New RectangleF(location, size)
            If refBool Then
                graphics.FillRoundedRectangle(refRect.ToRectangle, 10, brush, New RoundedRectCorners(True))
                Using pen As New Pen(color2, 2.0F)
                    graphics.DrawRoundedRectangle(refRect.ToRectangle, 10, pen)
                End Using
            Else
                Using pen As New Pen(color, 2.0F)
                    graphics.DrawRoundedRectangle(refRect.ToRectangle, 10, pen)
                End Using
            End If

            Dim textSize As SizeF = graphics.MeasureString(level.Title, font)
            Dim rect2 As New RectangleF(location.X + 10, location.Y + 10, refRect.Width - 20, refRect.Height - textSize.Height)
            graphics.DrawGDIPlusText(level.Title, font, rect2, If(refBool, color2, color), StringAlignment.Near)
            Dim rect3 As New RectangleF(location.X, location.Y + size.Height - textSize.Height - 20, refRect.Width - 20, textSize.Height + 10)
            graphics.DrawGDIPlusText($"Level {level.Level}", font, rect3, If(refBool, color2, color), StringAlignment.Far)
        End Using
    End Sub

    <Extension>
    Public Sub DrawCheckboxControl(graphics As Graphics, font As Font, ByRef refRect As RectangleF, ByRef refBool As Boolean, ByVal refValue As Boolean,
                                      location As PointF, size As SizeF, title As String, Optional color As Color = Nothing, Optional color2 As Color = Nothing)
        If color2 = Nothing Then color2 = Color.Red
        If color = Nothing Then color = Color.White

        Dim cbRect2 As New RectangleF(location.X, location.Y, size.Width, size.Height)
        Dim cbRect3 As New RectangleF(size.Width + 150 + size.Height + 20, location.Y, size.Width - size.Height, size.Height)
        refRect = New RectangleF(cbRect2.Location, New Size(size.Height, size.Height))
        If refBool Then
            Using p As New Pen(Brushes.Red, 5.0F)
                graphics.DrawRoundedRectangle(refRect.ToRectangle, 10, p)
            End Using
        Else
            Using p As New Pen(Brushes.White, 5.0F)
                graphics.DrawRoundedRectangle(refRect.ToRectangle, 10, p)
            End Using
        End If
        If refValue Then graphics.FillRoundedRectangle(refRect.ToRectangle, 10, Brushes.White, New RoundedRectCorners(True))
        graphics.DrawGDIPlusText(title, font, cbRect3, Color.White, StringAlignment.Near)
    End Sub

    <Extension>
    Public Function GetSafeZone(rect As Rectangle) As Rectangle
        If rect.Width > rect.Height Then
            Dim extraSpace As Integer = (rect.Height - rect.Width) / 2
            Return New Rectangle(rect.X - extraSpace, rect.Y, rect.Height, rect.Height)
        ElseIf rect.Height > rect.Width Then
            Dim extraSpace As Integer = (rect.Width - rect.Height) / 2
            Return New Rectangle(rect.X, rect.Y - extraSpace, rect.Width, rect.Width)
        End If
    End Function

    <Extension>
    Public Function GetColumnSize(rect As Rectangle, columns As Integer) As Size
        Dim newSize As Integer = CInt(rect.Width / columns)
        Return New Size(newSize, rect.Height)
    End Function

    <Extension>
    Public Function GetColumnSizef(rect As Rectangle, columns As Integer) As SizeF
        Dim newSize As Single = rect.Width / columns
        Return New SizeF(newSize, rect.Height)
    End Function

    <Extension>
    Public Function GetRowSizef(rect As Rectangle, rows As Integer) As SizeF
        Dim newSize As Single = rect.Height / rows
        Return New SizeF(rect.Width, newSize)
    End Function

    Public Enum BestFitStyle
        Zoom
        Stretch
    End Enum

    Public Function GetBestFitRect(ByVal rectFrame As RectangleF, ByVal rect As RectangleF, Optional ByVal BestFitStyle As BestFitStyle = BestFitStyle.Zoom) As RectangleF
        Dim origImageRatio = rect.Width / rect.Height
        Dim canvasRatio = rectFrame.Width / rectFrame.Height
        Dim DrawRect As RectangleF
        If (origImageRatio > canvasRatio AndAlso BestFitStyle = BestFitStyle.Stretch) OrElse (origImageRatio < canvasRatio AndAlso BestFitStyle = BestFitStyle.Zoom) Then
            DrawRect = New RectangleF(rectFrame.Left, rectFrame.Top, rectFrame.Width, rectFrame.Width * (1 / origImageRatio))
        Else
            DrawRect = New RectangleF(rectFrame.Left, rectFrame.Top, rectFrame.Height * origImageRatio, rectFrame.Height)
        End If
        DrawRect.X += ((rectFrame.Width - DrawRect.Width) / 2)
        DrawRect.Y += ((rectFrame.Height - DrawRect.Height) / 2)
        Return DrawRect
    End Function

    <Extension>
    Public Sub DrawImageBestFit(ByVal g As Graphics, ByVal rect As RectangleF, ByVal image As Image, Optional ByVal BestFitStyle As BestFitStyle = BestFitStyle.Zoom)
        Dim oldRegion = g.Clip.Clone
        g.SetClip(rect, CombineMode.Intersect)
        Dim DrawRect = GetBestFitRect(rect, New RectangleF(0, 0, image.Width, image.Height))
        g.DrawImage(image.DrawImageWithRoundedCorners(10, Color.White), DrawRect)
        g.Clip = oldRegion
    End Sub

    <Extension>
    Public Function GetPagesFromNum(level As Integer) As Integer
        Return CInt(Math.Ceiling(level / 9))
    End Function

    <Extension>
    Public Sub StartGame(ctrl As Control, level As Level, font As Font)
        Dim newGame As New MyGame(level.Phrase) With {.Title = level.Title, .Author = level.Author, .Level = level.Level, .Life = level.Life, .TimeLimit = level.TimeLimit, .LevelSel = ctrl, .Dock = DockStyle.Fill,
            .Font = New Font(font.FontFamily, font.Size * 2, FontStyle.Bold, font.Unit)}
        frmGame.Controls.Add(newGame)
        newGame.Refresh()
        ctrl.Hide()
    End Sub

    <Extension>
    Public Sub Striped(listview As ListView, Optional color1 As Color = Nothing, Optional color2 As Color = Nothing)
        If color2 = Nothing Then color2 = SystemColors.ButtonFace
        If color1 = Nothing Then color1 = SystemColors.Window

        Dim alternator As Integer = 0
        For Each lvi As ListViewItem In listview.Items
            If lvi.Group Is Nothing Then
                If alternator Mod 2 = 0 Then
                    For i As Integer = 0 To lvi.SubItems.Count - 1
                        If Not lvi.SubItems(i).BackColor = Color.LightSalmon Then lvi.SubItems(i).BackColor = color1
                    Next
                Else
                    For i As Integer = 0 To lvi.SubItems.Count - 1
                        If Not lvi.SubItems(i).BackColor = Color.LightSalmon Then lvi.SubItems(i).BackColor = color2
                    Next
                End If
                alternator += 1
            End If
        Next
        For Each gp As ListViewGroup In listview.Groups
            For Each lvi As ListViewItem In gp.Items
                If alternator Mod 2 = 0 Then
                    For i As Integer = 0 To lvi.SubItems.Count - 1
                        If Not lvi.SubItems(i).BackColor = Color.LightSalmon Then lvi.SubItems(i).BackColor = color1
                    Next
                Else
                    For i As Integer = 0 To lvi.SubItems.Count - 1
                        If Not lvi.SubItems(i).BackColor = Color.LightSalmon Then lvi.SubItems(i).BackColor = color2
                    Next
                End If
                alternator += 1
            Next
        Next
    End Sub

    <Extension>
    Public Sub AddGroupFooter(listviewx As ListViewX)
        For Each lvg As ListViewGroup In listviewx.Groups
            listviewx.SetGroupFooter(lvg, $"Contain {lvg.Items.Count} models.")
        Next
        listviewx.SetGroupState(ListViewX.ListViewGroupState.Collapsible Or ListViewX.ListViewGroupState.Collapsed)
    End Sub

    <Extension>
    Public Function Base64ToImage(Image As String) As Image
        If Image = "error" Then
            Return Nothing
        Else
            Dim b64 As String = Image.Replace(" ", "+")
            Dim bite() As Byte = Convert.FromBase64String(b64)
            Dim stream As New MemoryStream(bite)
            Return Drawing.Image.FromStream(stream)
        End If
    End Function

    <Extension>
    Public Function ImageToBase64(img As Image) As String
        Dim stream As New MemoryStream
        Dim bmp As Bitmap = New Bitmap(img)
        bmp.Save(stream, ImageFormat.Png)
        Return Convert.ToBase64String(stream.ToArray)
    End Function

    <Extension>
    Public Function InternetImageToBase64(url As String) As String
        Try
            ServicePointManager.Expect100Continue = True
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            Dim request = WebRequest.Create(url)
            Using response = request.GetResponse
                Using stream = response.GetResponseStream()
                    Dim img = Bitmap.FromStream(stream)
                    Return img.ImageToBase64
                End Using
            End Using
        Catch ex As Exception
            Return "error"
        End Try
    End Function

End Module
