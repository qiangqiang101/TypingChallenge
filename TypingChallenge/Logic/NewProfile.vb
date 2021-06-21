Imports System.Drawing.Drawing2D

Public Class NewProfile
    Inherits BaseControl

    'Tracking Mouse
    Private _mousePos As Point = Point.Empty
    Private txtName, btnOK As Rectangle
    Private txtNameH As Boolean = False, btnOKH As Boolean = False

    Public Property Player() As String = "Player"

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)

        _mousePos = New Point(e.X, e.Y)
        txtNameH = txtName.Contains(_mousePos)
        btnOKH = btnOK.Contains(_mousePos)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

        Dim cr = ClientRectangle.GetSafeZone
        Dim rWidth As Single = 750
        Dim rHeight As Single = 350

        Dim rect As New Rectangle(cr.X + (cr.Width / 2) - (rWidth / 2), cr.Y + (cr.Height / 2) - (rHeight / 2), rWidth, rHeight)
        Dim rHeight2 As Single = rect.GetRowSizef(3).Height

        Dim enterName As New Rectangle(rect.Location, New Size(rect.Width, rHeight2))
        txtName = New Rectangle(enterName.X + 20, rect.Y + rHeight2, enterName.Width - 40, 80)
        btnOK = New Rectangle(rect.X + (rect.Width / 2) - 100, rect.Y + (rHeight2 * 2), 200, 80)

        Using pen As New Pen(Color.White, 2.0F)
            g.DrawRoundedRectangle(rect, 30, pen)
            g.DrawGDIText("Please tell me your name.", Font, enterName, Color.White, TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
            g.DrawRectangle(pen, txtName)
            Using br As New SolidBrush(Color.White)
                g.FillRectangle(br, txtName)
            End Using

            Using br As New SolidBrush(If(btnOKH, Color.White, Color.Gray))
                g.FillRoundedRectangle(btnOK, 10, br, New RoundedRectCorners(True))
                If txtNameH Then
                    g.DrawGDIText($"{Player}_", Font, txtName, Color.Black, TextFormatFlags.HorizontalCenter)
                Else
                    g.DrawGDIText(Player, Font, txtName, Color.Black, TextFormatFlags.HorizontalCenter)
                End If
                g.DrawGDIText("OK", Font, btnOK, If(btnOKH, Color.Red, Color.White), TextFormatFlags.HorizontalCenter)
            End Using
        End Using
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        If txtNameH Then
            If e.KeyCode = Keys.Back Then
                If Not Player.Length = 0 Then Player = Player.Substring(0, Player.Length - 1)
            End If
        End If
    End Sub

    Protected Overrides Sub OnKeyPress(e As KeyPressEventArgs)
        MyBase.OnKeyPress(e)

        Dim allowedChar As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-=_+[]{};':<>,.?/\|`~"" "

        If txtNameH Then
            If allowedChar.Contains(e.KeyChar) Then Player = Player & e.KeyChar
        End If
    End Sub

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)

        If btnOKH Then
            soundBtnClick.PlayWav

            Dim newProfile As New ProfileData(prfXmlPath)
            With newProfile
                .Name = Player
                .DateCreated = Now
                .ClearedLevel = New List(Of UserLevel)
                .Credits = 0
            End With
            newProfile.Save()
            profile = newProfile

            frmGame.MainMenu.Show()
            frmGame.Controls.Remove(Me)
        End If
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)

        Invalidate()
    End Sub

End Class
