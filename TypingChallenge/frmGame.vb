Imports System.ComponentModel
Imports System.IO

Public Class frmGame

    Private Sub frmGame_Load(sender As Object, e As EventArgs) Handles Me.Load
        levels = New LevelData(lvlXmlPath).Instance
        setting = New SettingData(setXmlPath).Instance
        profile = New ProfileData(prfXmlPath).Instance

        If setting.FullScreen Then
            gameFormState.Maximize(Me)
        End If

        Dim intro As New Intro(MainMenu) With {.Dock = DockStyle.Fill, .Image = My.Resources.zintro, .SizeMode = PictureBoxSizeMode.Zoom}
        Controls.Add(intro)
        MainMenu.Hide()
        intro.Refresh()

        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Runtime Then
            PlayNextBGM()
        End If
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = Media.MM_MCINOTIFY Then
            PlayNextBGM()
        End If

        MyBase.WndProc(m)
    End Sub

    Private Sub frmGame_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        player.Stop()
    End Sub
End Class
