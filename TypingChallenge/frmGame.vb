Imports System.ComponentModel
Imports System.IO

Public Class frmGame

    'Audio
    Public player As New Media()
    Public mp3s As String() = Directory.GetFiles(bgmPath, "*.mp3")
    Private rand As New Random

    Private fileToPlay As String

    Private Sub frmGame_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim cmdline As String = Environment.CommandLine()
        Dim iPos = cmdline.IndexOf("""", 2)
        Dim sCmdLineArgs = cmdline.Substring(iPos + 1).Trim()

        If sCmdLineArgs = "-dev" Then
            frmLevelEdit.Show()
            Me.Hide()
        End If

        levels = New LevelData(lvlXmlPath).Instance
        setting = New SettingData(setXmlPath).Instance
        profile = New ProfileData(prfXmlPath).Instance

        If setting.FullScreen Then
            gameFormState.Maximize(Me)
        End If

        If Not File.Exists(prfXmlPath) Then
            Dim newProfile As New NewProfile() With {.Dock = DockStyle.Fill, .Font = New Font(MainMenu.Font.FontFamily, MainMenu.Font.Size / 2, FontStyle.Regular, MainMenu.Font.Unit)}
            Controls.Add(newProfile)
            MainMenu.Hide()
        End If

        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Runtime Then
            PlayNextBGM()
        End If
    End Sub

    Private Sub PlayNextBGM()
        fileToPlay = mp3s(rand.Next(mp3s.Length))
        player.Play(fileToPlay, Me)
        player.SetVolume(setting.MusicVolume)
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
