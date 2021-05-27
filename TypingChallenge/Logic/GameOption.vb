Imports System.Drawing.Drawing2D

Public Class GameOption
    Inherits BaseControl

    'Tracking Mouse
    Private _mousePos As Point = Point.Empty

    'Controls
    Private musicL, musicR, soundL, soundR, graphicL, graphicR, fullScreenCb, fpsCb, saveBtn, cancelBtn As RectangleF
    Private musicLH, musicRH, soundLH, soundRH, graphicLH, graphicRH, fullScreenCbH, fpsCbH, saveBtnH, cancelBtnH As Boolean

    Public Property MusicVolume() As Integer
    Public Property SoundVolume() As Integer
    Public Property GraphicsQuality() As Integer
    Public Property FullScreen() As Boolean
    Public Property ShowFPS() As Boolean

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
        fullScreenCbH = False
        fpsCbH = False
        saveBtnH = False
        cancelBtnH = False
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
        fullScreenCbH = fullScreenCb.Contains(_mousePos)
        fpsCbH = fpsCb.Contains(_mousePos)
        saveBtnH = saveBtn.Contains(_mousePos)
        cancelBtnH = cancelBtn.Contains(_mousePos)
    End Sub

    Protected Overrides Sub OnPaintBackground(pevent As PaintEventArgs)
        MyBase.OnPaintBackground(pevent)

        Dim g As Graphics = pevent.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

        Dim cr = ClientRectangle
        Dim rWidth As Single = (cr.Width / 2) - 150
        Dim cbWidth As Single = (rWidth / 2)
        Dim sz As Integer = 75

        'Music
        Dim mvRect1 As New RectangleF(100, 100, rWidth, sz)
        g.DrawGDIPlusText("Music Volume", Font, mvRect1, Color.White, StringAlignment.Near)
        Dim mvRect2 As New RectangleF(rWidth + 150, 100, rWidth, sz)
        musicL = New RectangleF(mvRect2.Location, New Size(sz, sz))
        If musicLH Then
            g.FillRoundedRectangle(musicL.ToRectangle, 10, Brushes.White, New RoundedRectCorners(True))
            g.DrawGDIPlusText("◀", Font, musicL, Color.Red, StringAlignment.Center)
        Else
            g.DrawGDIPlusText("◀", Font, musicL, Color.White, StringAlignment.Center)
        End If
        g.DrawGDIPlusText(MusicVolume, Font, mvRect2, Color.White, StringAlignment.Center)
        musicR = New RectangleF(mvRect2.X + mvRect2.Width - sz, mvRect2.Y, sz, sz)
        If musicRH Then
            g.FillRoundedRectangle(musicR.ToRectangle, 10, Brushes.White, New RoundedRectCorners(True))
            g.DrawGDIPlusText("▶", Font, musicR, Color.Red, StringAlignment.Center)
        Else
            g.DrawGDIPlusText("▶", Font, musicR, Color.White, StringAlignment.Center)
        End If

        'Sound
        Dim svRect1 As New RectangleF(100, 200, rWidth, sz)
        g.DrawGDIPlusText("Sound Volume", Font, svRect1, Color.White, StringAlignment.Near)
        Dim svRect2 As New RectangleF(rWidth + 150, 200, rWidth, sz)
        soundL = New RectangleF(svRect2.Location, New Size(sz, sz))
        If soundLH Then
            g.FillRoundedRectangle(soundL.ToRectangle, 10, Brushes.White, New RoundedRectCorners(True))
            g.DrawGDIPlusText("◀", Font, soundL, Color.Red, StringAlignment.Center)
        Else
            g.DrawGDIPlusText("◀", Font, soundL, Color.White, StringAlignment.Center)
        End If
        g.DrawGDIPlusText(SoundVolume, Font, svRect2, Color.White, StringAlignment.Center)
        soundR = New RectangleF(svRect2.X + svRect2.Width - sz, svRect2.Y, sz, sz)
        If soundRH Then
            g.FillRoundedRectangle(soundR.ToRectangle, 10, Brushes.White, New RoundedRectCorners(True))
            g.DrawGDIPlusText("▶", Font, soundR, Color.Red, StringAlignment.Center)
        Else
            g.DrawGDIPlusText("▶", Font, soundR, Color.White, StringAlignment.Center)
        End If

        'Graphics
        Dim gqRect1 As New RectangleF(100, 400, rWidth, sz)
        g.DrawGDIPlusText("Graphics Quality", Font, gqRect1, Color.White, StringAlignment.Near)
        Dim gqRect2 As New RectangleF(rWidth + 150, 400, rWidth, sz)
        graphicL = New RectangleF(gqRect2.Location, New Size(sz, sz))
        If graphicLH Then
            g.FillRoundedRectangle(graphicL.ToRectangle, 10, Brushes.White, New RoundedRectCorners(True))
            g.DrawGDIPlusText("◀", Font, graphicL, Color.Red, StringAlignment.Center)
        Else
            g.DrawGDIPlusText("◀", Font, graphicL, Color.White, StringAlignment.Center)
        End If
        g.DrawGDIPlusText(GraphicsQualityText(GraphicsQuality), Font, gqRect2, Color.White, StringAlignment.Center)
        graphicR = New RectangleF(gqRect2.X + gqRect2.Width - sz, gqRect2.Y, sz, sz)
        If graphicRH Then
            g.FillRoundedRectangle(graphicR.ToRectangle, 10, Brushes.White, New RoundedRectCorners(True))
            g.DrawGDIPlusText("▶", Font, graphicR, Color.Red, StringAlignment.Center)
        Else
            g.DrawGDIPlusText("▶", Font, graphicR, Color.White, StringAlignment.Center)
        End If

        'Fullscreen
        Dim cbRect2 As New RectangleF(rWidth + 150, 500, rWidth, sz)
        Dim cbRect3 As New RectangleF(rWidth + 150 + sz + 20, 500, rWidth - sz, sz)
        fullScreenCb = New RectangleF(cbRect2.Location, New Size(sz, sz))
        If fullScreenCbH Then
            Using p As New Pen(Brushes.Red, 5.0F)
                g.DrawRoundedRectangle(fullScreenCb.ToRectangle, 10, p)
            End Using
        Else
            Using p As New Pen(Brushes.White, 5.0F)
                g.DrawRoundedRectangle(fullScreenCb.ToRectangle, 10, p)
            End Using
        End If
        If FullScreen Then g.FillRoundedRectangle(fullScreenCb.ToRectangle, 10, Brushes.White, New RoundedRectCorners(True))
        g.DrawGDIPlusText("Full Screen", Font, cbRect3, Color.White, StringAlignment.Near)

        'FPS
        Dim cbRect5 As New RectangleF(rWidth + 150, 600, rWidth, sz)
        Dim cbRect6 As New RectangleF(rWidth + 150 + sz + 20, 600, rWidth - sz, sz)
        fpsCb = New RectangleF(cbRect5.Location, New Size(sz, sz))
        If fpsCbH Then
            Using p As New Pen(Brushes.Red, 5.0F)
                g.DrawRoundedRectangle(fpsCb.ToRectangle, 10, p)
            End Using
        Else
            Using p As New Pen(Brushes.White, 5.0F)
                g.DrawRoundedRectangle(fpsCb.ToRectangle, 10, p)
            End Using
        End If
        If ShowFPS Then g.FillRoundedRectangle(fpsCb.ToRectangle, 10, Brushes.White, New RoundedRectCorners(True))
        g.DrawGDIPlusText("Display FPS", Font, cbRect6, Color.White, StringAlignment.Near)

        saveBtn = New Rectangle((cr.Width / 2) - 310, cr.Height - 100, 300, 80)
        cancelBtn = New Rectangle((cr.Width / 2) + 0, cr.Height - 100, 300, 80)
        Using br As New SolidBrush(If(saveBtnH, Color.White, Color.Gray))
            g.FillRoundedRectangle(saveBtn.ToRectangle, 10, br, New RoundedRectCorners(True))
            g.DrawGDIPlusText("Save", Font, saveBtn, If(saveBtnH, Color.Red, Color.White), StringAlignment.Center)
        End Using
        Using br As New SolidBrush(If(cancelBtnH, Color.White, Color.Gray))
            g.FillRoundedRectangle(cancelBtn.ToRectangle, 10, br, New RoundedRectCorners(True))
            g.DrawGDIPlusText("Cancel", Font, cancelBtn, If(cancelBtnH, Color.Red, Color.White), StringAlignment.Center)
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

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)

        If musicLH Then If Not MusicVolume <= 0 Then MusicVolume -= 10
        If musicRH Then If Not MusicVolume >= 100 Then MusicVolume += 10
        If soundLH Then If Not SoundVolume <= 0 Then SoundVolume -= 10
        If soundRH Then If Not SoundVolume >= 100 Then SoundVolume += 10
        If graphicLH Then If Not GraphicsQuality <= 0 Then GraphicsQuality -= 1
        If graphicRH Then If Not GraphicsQuality >= 4 Then GraphicsQuality += 1
        If fullScreenCbH Then FullScreen = Not FullScreen
        If fpsCbH Then ShowFPS = Not ShowFPS

        If saveBtnH Then
            Dim newSetting As New SettingData(setXmlPath)
            With newSetting
                .MusicVolume = MusicVolume
                .SoundVolume = SoundVolume
                .Quality = GraphicsQuality
                .FullScreen = FullScreen
                .ShowFPS = ShowFPS
                .Version = setting.Version
            End With
            newSetting.Save()
            setting = newSetting

            frmGame.MainMenu.Show()
            frmGame.Controls.Remove(Me)
            frmGame.MainMenu.RefreshSettings()

            If setting.FullScreen Then
                frmGame.FormBorderStyle = FormBorderStyle.None
                frmGame.WindowState = FormWindowState.Maximized
            Else
                frmGame.FormBorderStyle = FormBorderStyle.Sizable
            End If
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
