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
        Dim rHeight As Single = cr.GetRowSizef(7).Height

        Dim optHeight = TextRenderer.MeasureText("PROFILE", Font).Height
        Dim optTitle As New Rectangle(cr.X, cr.Y + rHeight - optHeight, cr.Width, optHeight)
        Using lbrush As New LinearGradientBrush(optTitle, Color.Goldenrod, Color.Transparent, LinearGradientMode.Horizontal)
            g.FillRectangle(lbrush, optTitle)
        End Using
        g.DrawGDIText("PROFILE", Font, optTitle, Color.White, TextFormatFlags.Left Or TextFormatFlags.Bottom)

        Dim nameRect As New RectangleF(cr.X, cr.Y + rHeight, cr.Width, rHeight)
        g.DrawGDIText($"Name: {profile.Name}", Font, nameRect.ToRectangle, Color.White, TextFormatFlags.Left)

        Dim dateRect As New RectangleF(cr.X, cr.Y + rHeight * 2, cr.Width, rHeight)
        g.DrawGDIText($"Date Created: {profile.DateCreated.ToLongDateString}", Font, dateRect.ToRectangle, Color.White, TextFormatFlags.Left)

        Dim credRect As New RectangleF(cr.X, cr.Y + rHeight * 3, cr.Width, rHeight)
        g.DrawGDIText($"Credits: {profile.Credits}", Font, credRect.ToRectangle, Color.White, TextFormatFlags.Left)

        Dim clearRect As New RectangleF(cr.X, cr.Y + rHeight * 4, cr.Width, rHeight)
        g.DrawGDIText($"Cleared Level: {profile.ClearedLevel.Count}", Font, clearRect.ToRectangle, Color.White, TextFormatFlags.Left)

        Dim scoreRect As New RectangleF(cr.X, cr.Y + rHeight * 5, cr.Width, rHeight)
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
