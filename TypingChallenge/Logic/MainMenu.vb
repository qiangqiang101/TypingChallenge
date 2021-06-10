Public Class MainMenu
    Inherits BaseControl

    'Tracking Mouse
    Private _mousePos As Point = Point.Empty
    Private btnPlay, btnSetting, btnProfile, btnCredits, btnExit As Rectangle
    Private btnPlayH As Boolean = False, btnSettingH As Boolean = False, btnCreditsH As Boolean = False, btnExitH As Boolean = False, btnProfileH As Boolean = False

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)

        btnPlayH = False
        btnSettingH = False
        btnCreditsH = False
        btnExitH = False
        btnProfileH = False
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)

        _mousePos = New Point(e.X, e.Y)
        btnPlayH = btnPlay.Contains(_mousePos)
        btnSettingH = btnSetting.Contains(_mousePos)
        btnCreditsH = btnCredits.Contains(_mousePos)
        btnExitH = btnExit.Contains(_mousePos)
        btnProfileH = btnProfile.Contains(_mousePos)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
        Dim cr = ClientRectangle

        Dim hero As New Rectangle(10, 10, cr.Width - 20, (cr.Height / 2) - 120)
        Dim botLeft As New Rectangle(0, cr.Height - 20, cr.Width / 2, 20)
        Dim botRight As New Rectangle(cr.Width / 2, cr.Height - 20, cr.Width / 2, 20)
        g.DrawGDIText($"Typing Challenge", Font, hero, Color.Gold, TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)

        Using subFont As New Font(Font.FontFamily, 10.0F, FontStyle.Regular)
            g.DrawGDIText("Copyright © 2021 Zettabyte Technology, No Rights Reserved.", subFont, botLeft, Color.White, TextFormatFlags.Left)
            g.DrawGDIText($"Demo version {My.Application.Info.Version}", subFont, botRight, Color.White, TextFormatFlags.Right)
        End Using

        Dim plyNameRect As New Rectangle(0, cr.Height - 100, cr.Width / 2, 80)

        btnPlay = New Rectangle((cr.Width / 2) - 150, hero.Y + hero.Height + 100, 300, 80)
        btnSetting = New Rectangle((cr.Width / 2) - 150, btnPlay.Y + btnPlay.Height, 300, 80)
        btnProfile = New Rectangle((cr.Width / 2) - 150, btnSetting.Y + btnSetting.Height, 300, 80)
        btnCredits = New Rectangle((cr.Width / 2) - 150, btnProfile.Y + btnProfile.Height, 300, 80)
        btnExit = New Rectangle((cr.Width / 2) - 150, btnCredits.Y + btnCredits.Height, 300, 80)

        Using resFont As New Font(Font.FontFamily, Font.Size / 2, FontStyle.Regular)
            g.DrawGDIText($"Hello, {profile.Name}", resFont, plyNameRect, Color.White, TextFormatFlags.Left)

            g.DrawGDIText("Play", resFont, btnPlay, If(btnPlayH, Color.Red, Color.White), TextFormatFlags.HorizontalCenter)
            g.DrawGDIText("Options", resFont, btnSetting, If(btnSettingH, Color.Red, Color.White), TextFormatFlags.HorizontalCenter)
            g.DrawGDIText("Profile", resFont, btnProfile, If(btnProfileH, Color.Red, Color.White), TextFormatFlags.HorizontalCenter)
            g.DrawGDIText("Credits", resFont, btnCredits, If(btnCreditsH, Color.Red, Color.White), TextFormatFlags.HorizontalCenter)
            g.DrawGDIText("Quit", resFont, btnExit, If(btnExitH, Color.Red, Color.White), TextFormatFlags.HorizontalCenter)
        End Using

        Using pen As New Pen(Color.White, 0.5F)
            If btnPlayH Then g.DrawRoundedRectangle(btnPlay, 10, pen)
            If btnSettingH Then g.DrawRoundedRectangle(btnSetting, 10, pen)
            If btnProfileH Then g.DrawRoundedRectangle(btnProfile, 10, pen)
            If btnCreditsH Then g.DrawRoundedRectangle(btnCredits, 10, pen)
            If btnExitH Then g.DrawRoundedRectangle(btnExit, 10, pen)
        End Using
    End Sub

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)

        If btnPlayH Then
            soundBtnClick.PlayWav
            Dim lvlSel As New LevelSelection() With {.Dock = DockStyle.Fill, .Font = New Font(Font.FontFamily, Font.Size / 2, FontStyle.Bold, Font.Unit)}
            Parent.Controls.Add(lvlSel)
            lvlSel.GotoPage(1)
            lvlSel.Refresh()
            Me.Hide()
        End If
        If btnSettingH Then
            soundBtnClick.PlayWav
            Dim opt As New GameOption() With {.Dock = DockStyle.Fill, .Font = New Font(Font.FontFamily, Font.Size / 2, FontStyle.Bold, Font.Unit),
                .MusicVolume = setting.MusicVolume, .SoundVolume = setting.SoundVolume, .GraphicsQuality = setting.Quality, .FullScreen = setting.FullScreen, .ShowFPS = setting.ShowFPS,
                .KbColor = Color.FromArgb(setting.KeyboardColorA, setting.KeyboardColorR, setting.KeyboardColorG, setting.KeyboardColorB), .RGBKeyboard = setting.KeyboardRGB}
            Parent.Controls.Add(opt)
            opt.Refresh()
            Me.Hide()
        End If
        If btnProfileH Then
            'todo
            player.SetPosition(player.Milleseconds - 1000)
            soundBtnClick.PlayWav
        End If
        If btnCreditsH Then
            'todo
            soundBtnClick.PlayWav
        End If
        If btnExitH Then
            soundBtnCancel.PlayWav
            Threading.Thread.Sleep(500)
            frmGame.Close()
        End If
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)

        Invalidate()
    End Sub

End Class
