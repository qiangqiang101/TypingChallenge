Public Class MainMenu
    Inherits BaseControl

    'Tracking Mouse
    Private _mousePos As Point = Point.Empty
    Private btnPlay, btnSetting, btnCredits, btnExit As Rectangle
    Private btnPlayH As Boolean = False, btnSettingH As Boolean = False, btnCreditsH As Boolean = False, btnExitH As Boolean = False

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)

        btnPlayH = False
        btnSettingH = False
        btnCreditsH = False
        btnExitH = False
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)

        _mousePos = New Point(e.X, e.Y)
        btnPlayH = btnPlay.Contains(_mousePos)
        btnSettingH = btnSetting.Contains(_mousePos)
        btnCreditsH = btnCredits.Contains(_mousePos)
        btnExitH = btnExit.Contains(_mousePos)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
        Dim cr = ClientRectangle

        Dim hero As New Rectangle(10, 10, cr.Width - 20, (cr.Height / 2) - 120)
        Dim botLeft As New Rectangle(0, cr.Height - 20, cr.Width / 2, 20)
        Dim botRight As New Rectangle(cr.Width / 2, cr.Height - 20, cr.Width / 2, 20)
        g.DrawGDIText($"Typing Challenge", Font, hero, Color.Gold, TextFormatFlags.Bottom Or TextFormatFlags.HorizontalCenter)

        Using subFont As New Font(Font.FontFamily, 10.0F, FontStyle.Regular)
            g.DrawGDIPlusText("Copyright © 2021 Zettabyte Technology, No Rights Reserved.", subFont, botLeft, Color.White, StringAlignment.Near)
            g.DrawGDIPlusText($"Demo version {My.Application.Info.Version}", subFont, botRight, Color.White, StringAlignment.Far)
        End Using

        btnPlay = New Rectangle((cr.Width / 2) - 150, hero.Y + hero.Height + 100, 300, 80)
        btnSetting = New Rectangle((cr.Width / 2) - 150, btnPlay.Y + btnPlay.Height, 300, 80)
        btnCredits = New Rectangle((cr.Width / 2) - 150, btnSetting.Y + btnSetting.Height, 300, 80)
        btnExit = New Rectangle((cr.Width / 2) - 150, btnCredits.Y + btnCredits.Height, 300, 80)

        Using resFont As New Font(Font.FontFamily, Font.Size / 2, FontStyle.Bold)
            g.DrawGDIPlusText("Play", resFont, btnPlay, If(btnPlayH, Color.Red, Color.White), StringAlignment.Center)
            g.DrawGDIPlusText("Options", resFont, btnSetting, If(btnSettingH, Color.Red, Color.White), StringAlignment.Center)
            g.DrawGDIPlusText("Credits", resFont, btnCredits, If(btnCreditsH, Color.Red, Color.White), StringAlignment.Center)
            g.DrawGDIPlusText("Quit", resFont, btnExit, If(btnExitH, Color.Red, Color.White), StringAlignment.Center)
        End Using

        Using pen As New Pen(Color.White, 0.5F)
            If btnPlayH Then g.DrawRoundedRectangle(btnPlay, 10, pen)
            If btnSettingH Then g.DrawRoundedRectangle(btnSetting, 10, pen)
            If btnCreditsH Then g.DrawRoundedRectangle(btnCredits, 10, pen)
            If btnExitH Then g.DrawRoundedRectangle(btnExit, 10, pen)
        End Using
    End Sub

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)

        If btnPlayH Then
            Dim lvlSel As New LevelSelection() With {.Dock = DockStyle.Fill, .Font = New Font(Font.FontFamily, Font.Size / 2, FontStyle.Bold, Font.Unit)}
            Parent.Controls.Add(lvlSel)
            lvlSel.GotoPage(1)
            lvlSel.Refresh()
            Me.Hide()
        End If
        If btnSettingH Then
            Dim opt As New GameOption() With {.Dock = DockStyle.Fill, .Font = New Font(Font.FontFamily, Font.Size / 2, FontStyle.Bold, Font.Unit),
                .MusicVolume = setting.MusicVolume, .SoundVolume = setting.SoundVolume, .GraphicsQuality = setting.Quality, .FullScreen = setting.FullScreen, .ShowFPS = setting.ShowFPS}
            Parent.Controls.Add(opt)
            opt.Refresh()
            Me.Hide()
        End If
        If btnCreditsH Then
            'todo
        End If
        If btnExitH Then
            frmGame.Close()
        End If
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)

        Invalidate()
    End Sub

End Class
