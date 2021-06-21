Imports System.IO

Public Class Intro
    Inherits PictureBox

    Public WithEvents bgTimer As New Timer() With {.Interval = 1, .Enabled = True}
    Private countdown As Integer = 300

    Public Property MainMenu() As MainMenu

    Public Sub New(mm As MainMenu)
        BackColor = Color.White
        MainMenu = mm
    End Sub

    Private Sub OnFrameChanged(sender As Object, e As EventArgs)
        Invalidate()
    End Sub

    Private Sub bgTimer_Tick(sender As Object, e As EventArgs) Handles bgTimer.Tick
        countdown -= 1

        If countdown <= 0 Then
            bgTimer.Stop()

            If Not File.Exists(prfXmlPath) Then
                Dim newProfile As New NewProfile() With {.Dock = DockStyle.Fill, .Font = MainMenu.Font}
                Parent.Controls.Add(newProfile)
                newProfile.Focus()
                MainMenu.Hide()
            Else
                MainMenu.Show()
                MainMenu.Focus()
            End If

            Parent.Controls.Remove(Me)
        End If
    End Sub
End Class
