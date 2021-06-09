Imports System.Drawing.Drawing2D

Public Class GameOption
    Inherits BaseControl

    'Tracking Mouse
    Private _mousePos As Point = Point.Empty

    'Controls
    Private musicL, musicR, soundL, soundR, graphicL, graphicR, fullscreenL, fullscreenR, fpsL, fpsR, saveBtn, cancelBtn As RectangleF 'fullScreenCb, fpsCb
    Private kbBtn, rgbL, rgbR As RectangleF
    Private musicLH, musicRH, soundLH, soundRH, graphicLH, graphicRH, fullscreenLH, fullscreenRH, fpsLH, fpsRH, saveBtnH, cancelBtnH As Boolean 'fullScreenCbH, fpsCbH
    Private kbBtnH, rgbLH, rgbRH As Boolean

    Public Property MusicVolume() As Integer
    Public Property SoundVolume() As Integer
    Public Property GraphicsQuality() As Integer
    Public Property FullScreen() As Boolean
    Public Property ShowFPS() As Boolean
    Public Property KeyboardColor() As Color
    Public Property RGBKeyboard() As Boolean

    Public Sub New()
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)

        musicLH = False
        musicRH = False
        soundLH = False
        soundRH = False
        graphicLH = False
        graphicRH = False
        fullscreenLH = False
        fullscreenRH = False
        fpsLH = False
        fpsRH = False
        saveBtnH = False
        cancelBtnH = False

        kbBtnH = False
        rgbLH = False
        rgbRH = False
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)

        _mousePos = New Point(e.X, e.Y)
        musicLH = musicL.Contains(_mousePos)
        musicRH = musicR.Contains(_mousePos)
        soundLH = soundL.Contains(_mousePos)
        soundRH = soundR.Contains(_mousePos)
        graphicLH = graphicL.Contains(_mousePos)
        graphicRH = graphicR.Contains(_mousePos)
        fullscreenLH = fullscreenL.Contains(_mousePos)
        fullscreenRH = fullscreenR.Contains(_mousePos)
        fpsLH = fpsL.Contains(_mousePos)
        fpsRH = fpsR.Contains(_mousePos)
        saveBtnH = saveBtn.Contains(_mousePos)
        cancelBtnH = cancelBtn.Contains(_mousePos)

        kbBtnH = kbBtn.Contains(_mousePos)
        rgbLH = rgbL.Contains(_mousePos)
        rgbRH = rgbR.Contains(_mousePos)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

        Dim cr = ClientRectangle.GetSafeZone
        Dim rWidth As Single = cr.GetColumnSizef(2).Width
        Dim rHeight As Single = cr.GetRowSizef(8).Height
        Dim rHeight2 As Single = (rHeight * 4) / 6
        'Dim rWidth As Single = (cr.Width / 2) '- 150
        Dim sz As Integer = 75

        Dim optTitle As New Rectangle(cr.X, cr.Y, cr.Width, rHeight)
        g.DrawGDIText("OPTIONS", Font, optTitle, Color.White, TextFormatFlags.Left Or TextFormatFlags.Bottom)

        'Music
        g.DrawSliderControl(Font, musicL, musicR, musicLH, musicRH, New PointF(cr.X, cr.Y + rHeight), New Size(rWidth, rHeight2), "Music Volume", $"{MusicVolume}%")

        'Sound
        g.DrawSliderControl(Font, soundL, soundR, soundLH, soundRH, New PointF(cr.X, cr.Y + rHeight + rHeight2), New Size(rWidth, rHeight2), "Sound Volume", $"{SoundVolume}%")

        'Graphics
        g.DrawSliderControl(Font, graphicL, graphicR, graphicLH, graphicRH, New PointF(cr.X, cr.Y + rHeight + (rHeight2 * 3)), New SizeF(rWidth, rHeight2), "Graphics Quality", GraphicsQualityText(GraphicsQuality))

        'Keyboard
        g.DrawButtonControl(Font, kbBtn, kbBtnH, New PointF(cr.X, cr.Y + rHeight + (rHeight2 * 4)), New SizeF(rWidth, rHeight2), "Keyboard Color", KeyboardColor.Name)
        g.DrawSliderControl(Font, rgbL, rgbR, rgbLH, rgbRH, New PointF(cr.X, cr.Y + rHeight + (rHeight2 * 5)), New SizeF(rWidth, rHeight2), "RGB Keyboard", BoolToString(RGBKeyboard))

        'Fullscreen
        g.DrawSliderControl(Font, fullscreenL, fullscreenR, fullscreenLH, fullscreenRH, New PointF(cr.X, cr.Y + rHeight + (rHeight2 * 6)), New SizeF(rWidth, rHeight2), "Window Mode", FullscreenToString(FullScreen))

        'FPS
        g.DrawSliderControl(Font, fpsL, fpsR, fpsLH, fpsRH, New PointF(cr.X, cr.Y + rHeight + (rHeight2 * 7)), New SizeF(rWidth, rHeight2), "Show FPS", BoolToString(ShowFPS))

        saveBtn = New Rectangle(cr.X + (cr.Width / 2) - 310, cr.Height - 100, 300, 80)
        cancelBtn = New Rectangle(cr.X + (cr.Width / 2) + 0, cr.Height - 100, 300, 80)
        Using br As New SolidBrush(If(saveBtnH, Color.White, Color.Gray))
            g.FillRoundedRectangle(saveBtn.ToRectangle, 10, br, New RoundedRectCorners(True))
            g.DrawGDIText("Save", Font, saveBtn.ToRectangle, If(saveBtnH, Color.Red, Color.White), TextFormatFlags.HorizontalCenter)
        End Using
        Using br As New SolidBrush(If(cancelBtnH, Color.White, Color.Gray))
            g.FillRoundedRectangle(cancelBtn.ToRectangle, 10, br, New RoundedRectCorners(True))
            g.DrawGDIText("Cancel", Font, cancelBtn.ToRectangle, If(cancelBtnH, Color.Red, Color.White), TextFormatFlags.HorizontalCenter)
        End Using
    End Sub

    Private Function GraphicsQualityText(id As Integer) As String
        Select Case id
            Case 0
                Return "Performance"
            Case 1
                Return "Medium"
            Case 2
                Return "Normal"
            Case 3
                Return "High"
            Case Else
                Return "Fantastic"
        End Select
    End Function

    Private Function BoolToString(bool As Boolean, Optional onoff As Boolean = False) As String
        If onoff Then
            If bool Then Return "On" Else Return "Off"
        Else
            If bool Then Return "Yes" Else Return "No"
        End If
    End Function

    Private Function FullscreenToString(bool As Boolean) As String
        If bool Then Return "Fullscreen" Else Return "Windowed"
    End Function

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)

        If musicLH Then If Not MusicVolume <= 0 Then MusicVolume -= 10
        If musicRH Then If Not MusicVolume >= 100 Then MusicVolume += 10
        If soundLH Then If Not SoundVolume <= 0 Then SoundVolume -= 10
        If soundRH Then If Not SoundVolume >= 100 Then SoundVolume += 10
        If graphicLH Then If Not GraphicsQuality <= 0 Then GraphicsQuality -= 1
        If graphicRH Then If Not GraphicsQuality >= 4 Then GraphicsQuality += 1
        If fullscreenLH Then FullScreen = Not FullScreen
        If fullscreenRH Then FullScreen = Not FullScreen
        If fpsLH Then ShowFPS = Not ShowFPS
        If fpsRH Then ShowFPS = Not ShowFPS
        If kbBtnH Then
            Dim cd As New ColorDialog
            With cd
                .Color = KeyboardColor
                .AllowFullOpen = True
                .FullOpen = True
            End With
            If cd.ShowDialog <> DialogResult.Cancel Then
                KeyboardColor = cd.Color
            End If
        End If
        If rgbLH Then RGBKeyboard = Not RGBKeyboard
        If rgbRH Then RGBKeyboard = Not RGBKeyboard

        If saveBtnH Then
            Dim newSetting As New SettingData(setXmlPath)
            With newSetting
                .MusicVolume = MusicVolume
                .SoundVolume = SoundVolume
                .Quality = GraphicsQuality
                .FullScreen = FullScreen
                .ShowFPS = ShowFPS
                .KeyboardColorA = KeyboardColor.A
                .KeyboardColorR = KeyboardColor.R
                .KeyboardColorG = KeyboardColor.G
                .KeyboardColorB = KeyboardColor.B
                .KeyboardRGB = RGBKeyboard
                .Version = setting.Version
            End With
            newSetting.Save()
            setting = newSetting

            frmGame.MainMenu.Show()
            frmGame.Controls.Remove(Me)
            frmGame.MainMenu.RefreshSettings()
        End If
        If cancelBtnH Then
            frmGame.MainMenu.Show()
            frmGame.Controls.Remove(Me)
        End If

        Invalidate()
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)

        Invalidate()
    End Sub

End Class
