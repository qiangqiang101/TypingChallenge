Imports System.IO

Public Class frmGame



    Private Sub frmGame_Load(sender As Object, e As EventArgs) Handles Me.Load
        levels = New LevelData(lvlXmlPath).Instance
        setting = New SettingData(setXmlPath).Instance

        If setting.FullScreen Then
            gameFormState.Maximize(Me)
        End If
    End Sub
End Class
