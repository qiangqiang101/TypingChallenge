Imports System.IO

Public Class frmGame

    Private Sub frmGame_Load(sender As Object, e As EventArgs) Handles Me.Load
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
    End Sub
End Class
