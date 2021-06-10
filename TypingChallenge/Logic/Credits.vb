Imports System.Drawing.Drawing2D

Public Class Credits
    Inherits BaseControl

    Private Y As Integer = 0
    Private WithEvents timer As New Timer() With {.Interval = 1}

    Private textRect As Rectangle

    Public Sub New()
        DoubleBuffered = True
    End Sub

    Public Sub Start()
        Y = ClientSize.Height
        timer.Start()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

        Dim textSize = TextRenderer.MeasureText("TEXT", Font)

        Dim creditText As String = "Typing Challenge Credits

Original Game Idea
Bartholomew Ho

Programmers
Bartholomew Ho
Dat Han
Blake Pell
Dejan Vesic
smdnjp

UI Artist
Bartholomew Ho

Games Designer
Bartholomew Ho

QA
Bartholomew Ho

Music
Me Gusta Music
Adi Goldstein
Simon Leng
Brown House Media
AGsoundtrax
Anuch Music
Bransboynd
LightBeats
Orchestralis
PenguinMusic
331
Kornev Music
Twisterium

Sound
lokohighman
GameChest Audio


Zettabyte Technology

Producer
Bartholomew Ho

Designer
Bartholomew Ho

Community Manager
Bartholomew Ho

PR Manager
Bartholomew Ho

Chief Games Officer
Bartholomew Ho

Publishing Director
Bartholomew Ho

Design Director
Bartholomew Ho

Technical Director
Bartholomew Ho

Operations Director
Bartholomew Ho

Special Thanks
To all our partners and players for your ongoing support."

        textRect = New Rectangle(0, Y, ClientSize.Width, textSize.Height * 72)
        g.DrawGDIText(creditText, Font, textRect, Color.White, TextFormatFlags.HorizontalCenter)
    End Sub

    Private Sub timer_Tick(sender As Object, e As EventArgs) Handles timer.Tick
        Y -= 10

        If Y <= -textRect.Height Then
            timer.Stop()
            frmGame.MainMenu.Show()
            frmGame.Controls.Remove(Me)
        End If
    End Sub
End Class
