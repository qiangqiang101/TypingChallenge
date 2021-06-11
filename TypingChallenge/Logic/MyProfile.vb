Imports System.Drawing.Drawing2D

Public Class MyProfile
    Inherits BaseControl

    'Tracking Mouse
    Private _mousePos As Point = Point.Empty

    'Controls
    Private btnBack As RectangleF
    Private btnBackH As Boolean

    Public Sub New()
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)

        btnBackH = False
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)

        _mousePos = New Point(e.X, e.Y)
        btnBackH = btnBack.Contains(_mousePos)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

        Dim cr = ClientRectangle.GetSafeZone
        Dim rWidth As Single = cr.GetColumnSizef(2).Width
        Dim rHeight As Single = cr.GetRowSizef(6).Height

        Dim txtHeight = TextRenderer.MeasureText("PROFILE", Font).Height
        Dim optTitle As New Rectangle(cr.X, cr.Y + rHeight - txtHeight, cr.Width, txtHeight)
        Using lbrush As New LinearGradientBrush(optTitle, Color.Goldenrod, Color.Transparent, LinearGradientMode.Horizontal)
            g.FillRectangle(lbrush, optTitle)
        End Using
        g.DrawGDIText("PROFILE", Font, optTitle, Color.White, TextFormatFlags.Left Or TextFormatFlags.Bottom)

        Dim nameRect As New RectangleF(cr.X, cr.Y + rHeight, cr.Width, txtHeight)
        g.DrawGDIText($"Name: {profile.Name}", Font, nameRect.ToRectangle, Color.White, TextFormatFlags.Left)

        Dim dateRect As New RectangleF(cr.X, cr.Y + rHeight + txtHeight, cr.Width, txtHeight)
        g.DrawGDIText($"Date Created: {profile.DateCreated.ToShortDateString} {profile.DateCreated.ToShortTimeString}", Font, dateRect.ToRectangle, Color.White, TextFormatFlags.Left)

        Dim credRect As New RectangleF(cr.X, cr.Y + rHeight + txtHeight * 2, cr.Width, txtHeight)
        g.DrawGDIText($"Credits: {profile.Credits}", Font, credRect.ToRectangle, Color.White, TextFormatFlags.Left)

        Dim clearRect As New RectangleF(cr.X, cr.Y + rHeight + txtHeight * 3, cr.Width, txtHeight)
        g.DrawGDIText($"Cleared Level: {profile.ClearedLevel.Count}", Font, clearRect.ToRectangle, Color.White, TextFormatFlags.Left)

        Dim scoreRect As New RectangleF(cr.X, cr.Y + rHeight + txtHeight * 4, cr.Width, txtHeight)
        g.DrawGDIText($"Total Score: {profile.ClearedLevel.Sum(Function(x) x.Score)}", Font, scoreRect.ToRectangle, Color.White, TextFormatFlags.Left)

        btnBack = New Rectangle(cr.X + (cr.Width / 2) - 150, cr.Y + cr.Height - 100, 300, 80)
        Using br As New SolidBrush(If(btnBackH, Color.White, Color.Gray))
            g.FillRoundedRectangle(btnBack.ToRectangle, 10, br, New RoundedRectCorners(True))
            g.DrawGDIText("Back", Font, btnBack.ToRectangle, If(btnBackH, Color.Red, Color.White), TextFormatFlags.HorizontalCenter)
        End Using
    End Sub

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)

        If btnBackH Then
            soundBtnCancel.PlayWav
            frmGame.MainMenu.Show()
            frmGame.Controls.Remove(Me)
        End If

        Invalidate()
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)

        Invalidate()
    End Sub

End Class
