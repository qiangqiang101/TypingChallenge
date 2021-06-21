Imports System.Drawing.Drawing2D

Public Class GameOption
    Inherits BaseControl

    'Tracking Mouse
    Private _mousePos As Point = Point.Empty

    'Controls
    Private musicL, musicR, soundL, soundR, graphicL, graphicR, fullscreenL, fullscreenR, fpsL, fpsR, saveBtn, cancelBtn As RectangleF 'fullScreenCb, fpsCb
    Private kbBtn, rgbL, rgbR, musicEL, musicER, diffL, diffR As RectangleF
    Private musicLH, musicRH, soundLH, soundRH, graphicLH, graphicRH, fullscreenLH, fullscreenRH, fpsLH, fpsRH, saveBtnH, cancelBtnH As Boolean 'fullScreenCbH, fpsCbH
    Private kbBtnH, rgbLH, rgbRH, musicELH, musicERH, diffLH, diffRH As Boolean

    Public Property MusicVolume() As Integer
    Public Property MusicEnabled() As Boolean
    Public Property SoundVolume() As Integer
    Public Property GraphicsQuality() As Integer
    Public Property FullScreen() As Boolean
    Public Property ShowFPS() As Boolean
    Public Property KbColor() As Color
    Public Property RGBKeyboard() As Boolean
    Public Property Difficulty() As Integer

    Public Sub New()
        DoubleBuffered = True
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

        musicELH = musicEL.Contains(_mousePos)
        musicERH = musicER.Contains(_mousePos)
        diffLH = diffL.Contains(_mousePos)
        diffRH = diffR.Contains(_mousePos)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

        Dim cr = ClientRectangle.GetSafeZone
        Dim rWidth As Single = cr.GetColumnSizef(2).Width
        Dim rHeight As Single = cr.GetRowSizef(6).Height

        'Dim rWidth As Single = (cr.Width / 2) '- 150
        Dim sz As Integer = 75

        Dim optHeight = TextRenderer.MeasureText("OPTIONS", Font).Height
        Dim optTitle As New Rectangle(cr.X, cr.Y + rHeight - optHeight, cr.Width, optHeight)
        Using lbrush As New LinearGradientBrush(optTitle, Color.Goldenrod, Color.Transparent, LinearGradientMode.Horizontal)
            g.FillRectangle(lbrush, optTitle)
        End Using
        g.DrawGDIText("OPTIONS", Font, optTitle, Color.White, TextFormatFlags.Left Or TextFormatFlags.Bottom)

        Using smaller As New Font(Font.Name, If(ClientRectangle.Height < 1080, 25.0F, 30.0F), FontStyle.Regular)
            Dim rHeight2 As Single = TextRenderer.MeasureText("TEXT", smaller).Height '(rHeight * 4) / 6

            'Music
            g.DrawSliderControl(smaller, musicL, musicR, musicLH, musicRH, New PointF(cr.X, cr.Y + rHeight), New Size(rWidth, rHeight2), "Music Volume", $"{MusicVolume}%")
            g.DrawSliderControl(smaller, musicEL, musicER, musicELH, musicERH, New PointF(cr.X, cr.Y + rHeight + rHeight2), New Size(rWidth, rHeight2), "Music Enable", BoolToString(MusicEnabled))

            'Sound
            g.DrawSliderControl(smaller, soundL, soundR, soundLH, soundRH, New PointF(cr.X, cr.Y + rHeight + (rHeight2 * 2)), New Size(rWidth, rHeight2), "Sound Volume", $"{SoundVolume}%")

            'Graphics
            g.DrawSliderControl(smaller, graphicL, graphicR, graphicLH, graphicRH, New PointF(cr.X, cr.Y + rHeight + (rHeight2 * 4)), New SizeF(rWidth, rHeight2), "Graphics Quality", GraphicsQualityText(GraphicsQuality))

            'Keyboard
            g.DrawButtonControl(smaller, kbBtn, kbBtnH, New PointF(cr.X, cr.Y + rHeight + (rHeight2 * 5)), New SizeF(rWidth, rHeight2), "Keyboard Color", KbColor.Name)
            g.DrawSliderControl(smaller, rgbL, rgbR, rgbLH, rgbRH, New PointF(cr.X, cr.Y + rHeight + (rHeight2 * 6)), New SizeF(rWidth, rHeight2), "RGB Keyboard", BoolToString(RGBKeyboard))

            'Fullscreen
            g.DrawSliderControl(smaller, fullscreenL, fullscreenR, fullscreenLH, fullscreenRH, New PointF(cr.X, cr.Y + rHeight + (rHeight2 * 7)), New SizeF(rWidth, rHeight2), "Window Mode", FullscreenToString(FullScreen))

            'FPS
            g.DrawSliderControl(smaller, fpsL, fpsR, fpsLH, fpsRH, New PointF(cr.X, cr.Y + rHeight + (rHeight2 * 8)), New SizeF(rWidth, rHeight2), "Show FPS", BoolToString(ShowFPS))

            'difficulty
            g.DrawSliderControl(smaller, diffL, diffR, diffLH, diffRH, New PointF(cr.X, cr.Y + rHeight + (rHeight2 * 10)), New SizeF(rWidth, rHeight2), "Game Difficulty", DifficultyText(Difficulty))
        End Using

        saveBtn = New Rectangle(cr.X + (cr.Width / 2) - 310, cr.Y + cr.Height - rHeight, 300, 80)
        cancelBtn = New Rectangle(cr.X + (cr.Width / 2) + 0, cr.Y + cr.Height - rHeight, 300, 80)
        Using br As New SolidBrush(If(saveBtnH, Color.White, Color.Gray))
            g.FillRoundedRectangle(saveBtn.ToRectangle, 10, br, New RoundedRectCorners(True))
            g.DrawGDIText("Save", Font, saveBtn.ToRectangle, If(saveBtnH, Color.Red, Color.White), TextFormatFlags.HorizontalCenter)
        End Using
        Using br As New SolidBrush(If(cancelBtnH, Color.White, Color.Gray))
            g.FillRoundedRectangle(cancelBtn.ToRectangle, 10, br, New RoundedRectCorners(True))
            g.DrawGDIText("Cancel", Font, cancelBtn.ToRectangle, If(cancelBtnH, Color.Red, Color.White), TextFormatFlags.HorizontalCenter)
        End Using
    End Sub

    Private Function DifficultyText(id As Integer) As String
        Select Case id
            Case 0
                Return "Normal"
            Case 1
                Return "Hard"
            Case Else
                Return "Very Hard"
        End Select
    End Function

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

        If musicLH Then If Not MusicVolume <= 0 Then MusicVolume -= 5
        If musicRH Then If Not MusicVolume >= 100 Then MusicVolume += 5
        If musicELH Then MusicEnabled = Not MusicEnabled
        If musicERH Then MusicEnabled = Not MusicEnabled
        If soundLH Then If Not SoundVolume <= 0 Then SoundVolume -= 5
        If soundRH Then If Not SoundVolume >= 100 Then SoundVolume += 5
        If graphicLH Then If Not GraphicsQuality <= 0 Then GraphicsQuality -= 1
        If graphicRH Then If Not GraphicsQuality >= 4 Then GraphicsQuality += 1
        If fullscreenLH Then FullScreen = Not FullScreen
        If fullscreenRH Then FullScreen = Not FullScreen
        If fpsLH Then ShowFPS = Not ShowFPS
        If fpsRH Then ShowFPS = Not ShowFPS
        If kbBtnH Then
            Dim cd As New ColorDialog
            With cd
                .Color = KbColor
                .AllowFullOpen = True
                .FullOpen = True
            End With
            If cd.ShowDialog <> DialogResult.Cancel Then
                KbColor = cd.Color
            End If
        End If
        If rgbLH Then RGBKeyboard = Not RGBKeyboard
        If rgbRH Then RGBKeyboard = Not RGBKeyboard
        If diffLH Then If Not Difficulty <= 0 Then Difficulty -= 1
        If diffRH Then If Not Difficulty >= 2 Then Difficulty += 1

        If saveBtnH Then
            soundBtnClick.PlayWav
            Dim newSetting As New SettingData(setXmlPath)
            With newSetting
                .MusicVolume = MusicVolume
                .MusicEnabled = MusicEnabled
                .SoundVolume = SoundVolume
                .Quality = GraphicsQuality
                .FullScreen = FullScreen
                .ShowFPS = ShowFPS
                .KeyboardColorA = KbColor.A
                .KeyboardColorR = KbColor.R
                .KeyboardColorG = KbColor.G
                .KeyboardColorB = KbColor.B
                .KeyboardRGB = RGBKeyboard
                .Difficulty = Difficulty
                .Version = setting.Version
            End With
            newSetting.Save()
            setting = newSetting

            frmGame.MainMenu.Show()
            frmGame.Controls.Remove(Me)
            frmGame.MainMenu.RefreshSettings()
        End If
        If cancelBtnH Then
            soundBtnCancel.PlayWav
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
