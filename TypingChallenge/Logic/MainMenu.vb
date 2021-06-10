Imports System.Drawing.Drawing2D

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
        Using heroFont As New Font("Verdana", 80.0F, FontStyle.Bold)
            g.DrawGDIText($"Typing Challenge", heroFont, hero, Color.Gold, TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
        End Using

        Using subFont As New Font(Font.FontFamily, 10.0F, FontStyle.Regular)
            g.DrawGDIText("Copyright © 2021 Zettabyte Technology, All Rights Reserved.", subFont, botLeft, Color.White, TextFormatFlags.Left)
            g.DrawGDIText($"Version {My.Application.Info.Version}", subFont, botRight, Color.White, TextFormatFlags.Right)
        End Using

        Dim plyNameRect As New Rectangle(0, cr.Height - 150, cr.Width / 2, 80)

        btnPlay = New Rectangle((cr.Width / 2) - 150, hero.Y + hero.Height + 100, 300, 80)
        btnSetting = New Rectangle((cr.Width / 2) - 150, btnPlay.Y + btnPlay.Height, 300, 80)
        btnProfile = New Rectangle((cr.Width / 2) - 150, btnSetting.Y + btnSetting.Height, 300, 80)
        btnCredits = New Rectangle((cr.Width / 2) - 150, btnProfile.Y + btnProfile.Height, 300, 80)
        btnExit = New Rectangle((cr.Width / 2) - 150, btnCredits.Y + btnCredits.Height, 300, 80)

        Using lbrush As New LinearGradientBrush(plyNameRect, Color.Goldenrod, Color.Transparent, LinearGradientMode.Horizontal)
            g.FillRectangle(lbrush, plyNameRect)
        End Using
        g.DrawGDIText($"Hello, {profile.Name}", Font, plyNameRect, Color.White, TextFormatFlags.Left)

        Using lbrush As New LinearGradientBrush(btnPlay, Color.Goldenrod, Color.Transparent, LinearGradientMode.Horizontal)
            If btnPlayH Then g.FillRectangle(lbrush, btnPlay)
            If btnSettingH Then g.FillRectangle(lbrush, btnSetting)
            If btnProfileH Then g.FillRectangle(lbrush, btnProfile)
            If btnCreditsH Then g.FillRectangle(lbrush, btnCredits)
            If btnExitH Then g.FillRectangle(lbrush, btnExit)
        End Using

        g.DrawGDIText("Play", Font, btnPlay, Color.White, TextFormatFlags.HorizontalCenter)
        g.DrawGDIText("Options", Font, btnSetting, Color.White, TextFormatFlags.HorizontalCenter)
        g.DrawGDIText("Profile", Font, btnProfile, Color.White, TextFormatFlags.HorizontalCenter)
        g.DrawGDIText("Credits", Font, btnCredits, Color.White, TextFormatFlags.HorizontalCenter)
        g.DrawGDIText("Quit", Font, btnExit, Color.White, TextFormatFlags.HorizontalCenter)
    End Sub

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)

        If btnPlayH Then
            soundBtnClick.PlayWav
            Dim lvlSel As New LevelSelection() With {.Dock = DockStyle.Fill, .Font = Font}
            Parent.Controls.Add(lvlSel)
            lvlSel.GotoPage(1)
            lvlSel.Refresh()
            Me.Hide()
        End If
        If btnSettingH Then
            soundBtnClick.PlayWav
            Dim opt As New GameOption() With {.Dock = DockStyle.Fill, .Font = Font, .MusicVolume = setting.MusicVolume, .SoundVolume = setting.SoundVolume, .GraphicsQuality = setting.Quality, .FullScreen = setting.FullScreen, .ShowFPS = setting.ShowFPS,
                .KbColor = Color.FromArgb(setting.KeyboardColorA, setting.KeyboardColorR, setting.KeyboardColorG, setting.KeyboardColorB), .RGBKeyboard = setting.KeyboardRGB}
            Parent.Controls.Add(opt)
            opt.Refresh()
            Me.Hide()
        End If
        If btnProfileH Then
            soundBtnClick.PlayWav

            Dim prof As New MyProfile() With {.Dock = DockStyle.Fill, .Font = Font}
            Parent.Controls.Add(prof)
            prof.Refresh()
            Me.Hide()
        End If
        If btnCreditsH Then
            soundBtnClick.PlayWav
            Dim credits As New Credits() With {.Dock = DockStyle.Fill, .Font = Font}
            Parent.Controls.Add(credits)
            credits.Refresh()
            credits.Start()
            Me.Hide()
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
