﻿Imports System.Drawing.Drawing2D

Public Class LevelSelection
    Inherits BaseControl

    'Tracking Mouse
    Private _mousePos As Point = Point.Empty

    'Controls
    Private lvlA1, lvlB1, lvlC1, lvlA2, lvlB2, lvlC2, lvlA3, lvlB3, lvlC3, btnPrev, btnNext, btnBack As RectangleF
    Private lvlA1H, lvlB1H, lvlC1H, lvlA2H, lvlB2H, lvlC2H, lvlA3H, lvlB3H, lvlC3H, btnPrevH, btnNextH, btnBackH As Boolean

    Public Property A1() As Level
    Public Property B1() As Level
    Public Property C1() As Level
    Public Property A2() As Level
    Public Property B2() As Level
    Public Property C2() As Level
    Public Property A3() As Level
    Public Property B3() As Level
    Public Property C3() As Level
    Public Property Page() As Integer

    Public Sub New()
        DoubleBuffered = True

        GotoPage(1)
    End Sub

    Public Sub GotoPage(pg As Integer)
        Dim first As Integer = levels.LevelList(pg).Level - 1
        A1 = levels.LevelList.Find(Function(x) x.Level = first)
        B1 = levels.LevelList.Find(Function(x) x.Level = first + 1)
        C1 = levels.LevelList.Find(Function(x) x.Level = first + 2)
        A2 = levels.LevelList.Find(Function(x) x.Level = first + 3)
        B2 = levels.LevelList.Find(Function(x) x.Level = first + 4)
        C2 = levels.LevelList.Find(Function(x) x.Level = first + 5)
        A3 = levels.LevelList.Find(Function(x) x.Level = first + 6)
        B3 = levels.LevelList.Find(Function(x) x.Level = first + 7)
        C3 = levels.LevelList.Find(Function(x) x.Level = first + 8)
        Page = pg

        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)

        lvlA1H = False
        lvlA2H = False
        lvlA3H = False
        lvlB1H = False
        lvlB2H = False
        lvlB3H = False
        lvlC1H = False
        lvlC2H = False
        lvlC3H = False
        btnPrevH = False
        btnNextH = False
        btnBackH = False
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)

        _mousePos = New Point(e.X, e.Y)
        lvlA1H = lvlA1.Contains(_mousePos)
        lvlA2H = lvlA2.Contains(_mousePos)
        lvlA3H = lvlA3.Contains(_mousePos)
        lvlB1H = lvlB1.Contains(_mousePos)
        lvlB2H = lvlB2.Contains(_mousePos)
        lvlB3H = lvlB3.Contains(_mousePos)
        lvlC1H = lvlC1.Contains(_mousePos)
        lvlC2H = lvlC2.Contains(_mousePos)
        lvlC3H = lvlC3.Contains(_mousePos)
        btnPrevH = btnPrev.Contains(_mousePos)
        btnNextH = btnNext.Contains(_mousePos)
        btnBackH = btnBack.Contains(_mousePos)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

        Dim cr = ClientRectangle.GetSafeZone
        Dim rWidth As Single = cr.GetColumnSizef(3).Width
        Dim rHeight As Single = cr.GetRowSizef(5).Height

        Dim lvlTitle As New Rectangle(cr.X, cr.Y, cr.Width, rHeight)
        g.DrawGDIText("LEVEL SELECTION", Font, lvlTitle, Color.White, TextFormatFlags.Left Or TextFormatFlags.Bottom)
        g.DrawGDIText($"Page {Page} of {levels.LevelList.Count.GetPagesFromNum}", Font, lvlTitle, Color.White, TextFormatFlags.Right Or TextFormatFlags.Bottom)

        'Row1
        g.DrawLevelSelectionControl(Font, lvlA1, lvlA1H, New PointF(cr.X + 5, cr.Y + rHeight), New SizeF(rWidth - 10, rHeight - 10), A1)
        g.DrawLevelSelectionControl(Font, lvlB1, lvlB1H, New PointF(cr.X + 5 + rWidth, cr.Y + rHeight), New SizeF(rWidth - 10, rHeight - 10), B1)
        g.DrawLevelSelectionControl(Font, lvlC1, lvlC1H, New PointF(cr.X + 5 + (rWidth * 2), cr.Y + rHeight), New SizeF(rWidth - 10, rHeight - 10), C1)

        'Row2
        g.DrawLevelSelectionControl(Font, lvlA2, lvlA2H, New PointF(cr.X + 5, cr.Y + (rHeight * 2)), New SizeF(rWidth - 10, rHeight - 10), A2)
        g.DrawLevelSelectionControl(Font, lvlB2, lvlB2H, New PointF(cr.X + 5 + rWidth, cr.Y + (rHeight * 2)), New SizeF(rWidth - 10, rHeight - 10), B2)
        g.DrawLevelSelectionControl(Font, lvlC2, lvlC2H, New PointF(cr.X + 5 + (rWidth * 2), cr.Y + (rHeight * 2)), New SizeF(rWidth - 10, rHeight - 10), C2)

        'Row3
        g.DrawLevelSelectionControl(Font, lvlA3, lvlA3H, New PointF(cr.X + 5, cr.Y + (rHeight * 3)), New SizeF(rWidth - 10, rHeight - 10), A3)
        g.DrawLevelSelectionControl(Font, lvlB3, lvlB3H, New PointF(cr.X + 5 + rWidth, cr.Y + (rHeight * 3)), New SizeF(rWidth - 10, rHeight - 10), B3)
        g.DrawLevelSelectionControl(Font, lvlC3, lvlC3H, New PointF(cr.X + 5 + (rWidth * 2), cr.Y + (rHeight * 3)), New SizeF(rWidth - 10, rHeight - 10), C3)

        btnNext = New Rectangle(cr.X + cr.Width - 200, cr.Height - rHeight, 200, 80)
        btnPrev = New Rectangle(btnNext.X - 205, cr.Height - rHeight, 200, 80)
        btnBack = New RectangleF(cr.X, cr.Height - rHeight, 200, 80)
        Using br As New SolidBrush(If(btnPrevH, Color.White, Color.Gray))
            g.FillRoundedRectangle(btnPrev.ToRectangle, 10, br, New RoundedRectCorners(True))
            g.DrawGDIPlusText("◀ Prev", Font, btnPrev, If(btnPrevH, Color.Red, Color.White), StringAlignment.Center)
        End Using
        Using br As New SolidBrush(If(btnNextH, Color.White, Color.Gray))
            g.FillRoundedRectangle(btnNext.ToRectangle, 10, br, New RoundedRectCorners(True))
            g.DrawGDIPlusText("Next ▶", Font, btnNext, If(btnNextH, Color.Red, Color.White), StringAlignment.Center)
        End Using
        Using br As New SolidBrush(If(btnBackH, Color.White, Color.Gray))
            g.FillRoundedRectangle(btnBack.ToRectangle, 10, br, New RoundedRectCorners(True))
            g.DrawGDIPlusText("Back", Font, btnBack, If(btnBackH, Color.Red, Color.White), StringAlignment.Center)
        End Using
    End Sub

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)

        If lvlA1H Then

        End If
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)

        Invalidate()
    End Sub

End Class