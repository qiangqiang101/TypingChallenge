Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices

Public Class MyGame
    Inherits Control

    'Properties
    Public Property Level() As Integer = 1
    Public Property Phrase() As String
    Public Property Life() As Integer = 5
    Public Property TimeLimit() As Integer = 300

    'Texts
    Private GrayText As String
    Private GoldText As Char
    Private WhiteText As String
    Private _PhraseBackup As String

    'Data
    Private LifeLeft As Integer
    Private ElapsedTime As Integer
    Private GameStatus As eGameStatus = eGameStatus.Ready
    Private CorrectCount As Integer = 0
    Private CorrectStreak As Integer = 0
    Private WrongCount As Integer = 0

    Public WithEvents elapsedTimer As New Timer() With {.Interval = 500}
    Private timerStart As DateTime = Nothing
    Private timerEnded As DateTime = Nothing
    Public WithEvents bgTimer As New Timer() With {.Interval = 100, .Enabled = True}

    'Circles
    Private MainRect As Rectangle
    Private Shared rd As New Random
    Private Shared Circles(130) As cCircle
    Private Shared PointDistance As Decimal = 100
    Private Shared ea As MouseEventArgs

    Public Sub New()
        DoubleBuffered = True

        If Phrase = Nothing Then Phrase = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
        _PhraseBackup = Phrase

        GrayText = ""
        GoldText = Phrase.First
        WhiteText = Phrase.Remove(0, 1)
        LifeLeft = Life

        'Circles
        PrepareCircles()
    End Sub

    Private Sub PrepareCircles()
        'get form rectangle
        MainRect = DisplayRectangle

        'prepare circles
        For i As Integer = 0 To Circles.Count - 1
            Circles(i) = New cCircle(MainRect)
        Next
    End Sub

    Private Sub DrawGDIPlusText(graphics As Graphics, text As String, font As Font, bounds As RectangleF, color As Color, Optional alignment As StringAlignment = StringAlignment.Center, Optional offset As Point = Nothing)
        Dim format As New StringFormat()
        format.Alignment = alignment

        Using myBrush As New SolidBrush(color)
            If Not offset = Nothing Then bounds.Offset(offset)
            graphics.DrawString(text, font, myBrush, bounds, format)
        End Using

        graphics.ResetTransform()
    End Sub

    Private Function HeartsLeft(h As Integer) As String
        Select Case h
            Case < 0
                Return ""
            Case 0
                Return ""
            Case 1
                Return "❤"
            Case 2
                Return "❤❤"
            Case 3
                Return "❤❤❤"
            Case 4
                Return "❤❤❤❤"
            Case 5
                Return "❤❤❤❤❤"
            Case Else
                Return $"❤❤❤❤❤ +{h - 5}"
        End Select
    End Function

    Private Function Progression() As Integer
        Return CInt((WhiteText.Length / _PhraseBackup.Length) * 100)
    End Function

    Private Sub DrawGameText(graphics As Graphics, pastText As String, highlightText As String, nextText As String, font As Font, bounds As RectangleF, brushP As Brush, brushH As Brush, brushN As Brush, Optional offset As Point = Nothing)
        Dim format As New StringFormat()
        format.Alignment = StringAlignment.Center

        If Not offset = Nothing Then bounds.Offset(offset)
        Dim drawAt As New PointF(bounds.X, bounds.Y)
        Dim useBrushes() As SolidBrush = {brushP, brushH, brushN}

        'Draw Past Text
        For Each letter As Char In pastText
            graphics.DrawString(letter, font, brushP, drawAt)
            drawAt.X += graphics.MeasureString(letter, font).Width / 1.5F
        Next
        'Draw Current Text
        graphics.DrawString(If(highlightText = " ", "_", highlightText), font, brushH, drawAt)
        drawAt.X += graphics.MeasureString(highlightText, font).Width / 1.5F
        'Draw Future Text
        For Each letter As Char In nextText
            graphics.DrawString(letter, font, brushN, drawAt)
            drawAt.X += graphics.MeasureString(letter, font).Width / 1.5F
        Next

        graphics.ResetTransform()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)

        ea = Nothing
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)

        ea = e
    End Sub

    Private Sub PaintBackground(graphics As Graphics)
        For i As Integer = 0 To Circles.Count - 1

            If Not IsNothing(ea) Then
                Dim msx As Integer = ea.X
                Dim msy As Integer = ea.Y
                Dim osx As Integer = Circles(i).x
                Dim osy As Integer = Circles(i).y

                'detect the mouse location. if a circle is within the range (100px) of mouse pointer
                'then push back the circle
                If (msx - osx) ^ 2 + (msy - osy) ^ 2 < 100 ^ 2 Then
                    Dim pTarget As Point = New Point(osx, osy)
                    Dim pOrigin As Point = New Point(msx, msy)

                    'get the angle of cricle from mouse pointer location
                    Dim getAngle As Integer
                    getAngle = (((Math.Atan2(osx - msx, msy - osy) * (180 / Math.PI)) + 360.0) Mod 360.0)

                    'get the distance of circle from mouse pointer location
                    Dim getDist As Integer = DistanceBetween(New Point(msx, msy), New Point(osx, osy))

                    'get the new point where the circle should be pushed back 
                    Dim newPoint As Point = New Point(GetX(osx, 100 - getDist, getAngle), GetY(osy, 100 - getDist, getAngle))

                    'set the new point to the circle
                    Circles(i).x = newPoint.X
                    Circles(i).y = newPoint.Y
                End If
            End If

            'update circles
            Circles(i).Show(graphics)
            Circles(i).Update()
        Next
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality

        PaintBackground(g)

        Dim cr = ClientRectangle

        If GameStatus = eGameStatus.GameOver Then
            Dim textRect As New RectangleF(0, (cr.Height / 2) - 120, cr.Width, 210)
            DrawGDIPlusText(g, "GAME OVER", Font, textRect, Color.White, StringAlignment.Center)
            Dim res1Rect As New RectangleF(0, textRect.Y + textRect.Height, cr.Width, 100)
            Dim res2Rect As New RectangleF(0, res1Rect.Y + res1Rect.Height, cr.Width, 100)
            Dim res3Rect As New RectangleF(0, res2Rect.Y + res2Rect.Height, cr.Width, 100)
            Using resFont As New Font(Font.FontFamily, Font.Size / 2, FontStyle.Bold)
                DrawGDIPlusText(g, $"{CorrectCount} Correct letters ", resFont, res1Rect, Color.White, StringAlignment.Center)
                DrawGDIPlusText(g, $"{WrongCount} Wrong letters ", resFont, res2Rect, Color.White, StringAlignment.Center)
                DrawGDIPlusText(g, $"Time Elapsed {timerEnded.Subtract(timerStart.AddSeconds(CInt($"-{TimeLimit}"))).ToString("mm\:ss")}", resFont, res3Rect, Color.White, StringAlignment.Center)
            End Using
        ElseIf GameStatus = eGameStatus.YouWon Then
            Dim textRect As New RectangleF(0, (cr.Height / 2) - 120, cr.Width, 210)
            DrawGDIPlusText(g, "LEVEL COMPLETED", Font, textRect, Color.Gold, StringAlignment.Center)
            Dim res1Rect As New RectangleF(0, textRect.Y + textRect.Height, cr.Width, 100)
            Dim res2Rect As New RectangleF(0, res1Rect.Y + res1Rect.Height, cr.Width, 100)
            Dim res3Rect As New RectangleF(0, res2Rect.Y + res2Rect.Height, cr.Width, 100)
            Using resFont As New Font(Font.FontFamily, Font.Size / 2, FontStyle.Bold)
                DrawGDIPlusText(g, $"{CorrectCount} Correct letters ", resFont, res1Rect, Color.White, StringAlignment.Center)
                DrawGDIPlusText(g, $"{WrongCount} Wrong letters ", resFont, res2Rect, Color.White, StringAlignment.Center)
                DrawGDIPlusText(g, $"Time Elapsed {timerEnded.Subtract(timerStart.AddSeconds(CInt($"-{TimeLimit}"))).ToString("mm\:ss")}", resFont, res3Rect, Color.White, StringAlignment.Center)
            End Using
        ElseIf GameStatus = eGameStatus.Ready Then
            Dim textRect As New RectangleF(0, (cr.Height / 2) - 120, cr.Width, 210)
            DrawGDIPlusText(g, "Press Enter to Start", Font, textRect, Color.AliceBlue, StringAlignment.Center)
        Else
            Dim lifeRect As New RectangleF(10, 10, (cr.Width / 2) - 10, 100)
            Dim lvlRect As New RectangleF((cr.Width / 2) + 10, 10, (cr.Width / 2) - 10, 100)
            Using heartFont As New Font(Font.FontFamily, Font.Size / 2, FontStyle.Bold)
                DrawGDIPlusText(g, HeartsLeft(LifeLeft), heartFont, lifeRect, Color.Red, StringAlignment.Near)
                DrawGDIPlusText(g, $"Level {Level}", heartFont, lvlRect, Color.White, StringAlignment.Far)
            End Using

            Dim textRect As New RectangleF(0, (cr.Height / 2) - 120, cr.Width, 210)
            Select Case GrayText.Length
                Case 0
                    DrawGameText(g, " ", GoldText, WhiteText, Font, textRect, Brushes.Gray, Brushes.Gold, Brushes.White)
                Case 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
                    DrawGameText(g, GrayText, GoldText, WhiteText, Font, textRect, Brushes.Gray, Brushes.Gold, Brushes.White)
                Case Else
                    DrawGameText(g, GrayText.Substring(GrayText.Length - 10), GoldText, WhiteText, Font, textRect, Brushes.Gray, Brushes.Gold, Brushes.White)
            End Select

            Dim timeElapseRect As New RectangleF(10, cr.Height - 120, (cr.Width / 2) - 10, 100)
            Dim progressRect As New RectangleF((cr.Width / 2) + 10, cr.Height - 120, (cr.Width / 2) - 10, 100)
            Using timeFont As New Font(Font.FontFamily, Font.Size / 2, FontStyle.Bold)
                DrawGDIPlusText(g, $"⏲ {ElapsedTime}", timeFont, timeElapseRect, If(ElapsedTime <= 10, Color.Red, Color.White), StringAlignment.Near)
                DrawGDIPlusText(g, $"⚔ {Progression()}%", timeFont, progressRect, Color.White, StringAlignment.Far)
            End Using
        End If
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        If GameStatus = eGameStatus.Ready Then
            If e.KeyCode = Keys.Enter Then
                If Not elapsedTimer.Enabled Then
                    timerStart = Now.AddSeconds(TimeLimit)
                    elapsedTimer.Start()
                    GameStatus = eGameStatus.Running
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub OnKeyPress(e As KeyPressEventArgs)
        MyBase.OnKeyPress(e)

        If GameStatus = eGameStatus.Running Then
            If e.KeyChar = GoldText Then
                GrayText &= GoldText
                GoldText = WhiteText.FirstOrDefault
                Try
                    WhiteText = WhiteText.Remove(0, 1)
                Catch ex As Exception
                    GameStatus = eGameStatus.YouWon
                    timerEnded = Now
                    If elapsedTimer.Enabled Then elapsedTimer.Stop()
                End Try
                CorrectCount += 1
                CorrectStreak += 1
                If CorrectStreak >= 50 Then LifeLeft += 1 : CorrectStreak = 0
                Invalidate()
            Else
                LifeLeft -= 1
                WrongCount += 1
                CorrectStreak = 0
                Invalidate()
            End If
        End If
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)

        PrepareCircles()
        Invalidate()
    End Sub

    Private Sub elapsedTimer_Tick(sender As Object, e As EventArgs) Handles elapsedTimer.Tick
        If LifeLeft < 1 Then
            GameStatus = eGameStatus.GameOver
            timerEnded = Now
            'Invalidate()
            elapsedTimer.Stop()
        Else
            ElapsedTime = CInt(Now.Subtract(timerStart).TotalSeconds)
            ElapsedTime = CInt(ElapsedTime.ToString.Replace("-", ""))
            'Invalidate()
        End If
    End Sub

    Private Sub bgTimer_Tick(sender As Object, e As EventArgs) Handles bgTimer.Tick
        Invalidate()
    End Sub

    Friend Class cCircle
        Public movementAngle As Decimal
        Public speed As Decimal
        Public size As Decimal
        Public x As Decimal
        Public y As Decimal
        Private MainRect As Rectangle

        Sub New(MainRect As Rectangle)
            Me.MainRect = MainRect
            ResetVars()
        End Sub

        Private Sub ResetVars()
            'reset variables
            movementAngle = rd.Next(0, 360)
            speed = rd.Next(2, 7)
            size = rd.Next(2, 10)
            x = rd.Next(0, MainRect.Width)
            y = rd.Next(0, MainRect.Height)
        End Sub

        Public Sub Show(G As Graphics)
            Dim mypoint As Point = New Point(x, y)

            'loop to all circles to identify nearby circles 
            For i As Integer = 0 To Circles.Count - 1
                Dim cpoint As Point = New Point(Circles(i).x, Circles(i).y)

                If Circles(i).x <> x And Circles(i).y <> y Then
                    'get the distance between 2 circles
                    Dim iDis As Integer = DistanceBetween(mypoint, cpoint)
                    If iDis < PointDistance Then
                        'set the alpha of the line based on the distance
                        'fade when far and more visible when near
                        Dim a As Integer = (iDis / PointDistance) * 50
                        G.DrawLine(New Pen(Color.FromArgb(50 - a, 200, 200, 200), 0.5), mypoint, cpoint)
                    End If
                End If
            Next

            G.FillEllipse(New SolidBrush(Color.FromArgb(100, 250, 250, 250)), New Rectangle(x - (size / 2), y - (size / 2), size, size))
        End Sub

        Public Sub Update()
            'move the position of the circle based on the given speed and angle
            x = GetX(x, speed, movementAngle)
            y = GetY(y, speed, movementAngle)

            'reset variables when the circle reaches the edge
            If x < -20 Or y < -20 Or x > MainRect.Width + 20 Or y > MainRect.Height + 20 Then
                ResetVars()
            End If
        End Sub

    End Class

    Public Shared Function DistanceBetween(p1 As Point, p2 As Point) As Single
        Return Math.Sqrt((Math.Abs(p2.X - p1.X) ^ 2) + (Math.Abs(p2.Y - p1.Y) ^ 2))
    End Function

    Private Shared Function GetX(FromX As Decimal, toAdd As Decimal, Angle As Integer) As Decimal
        Return FromX + toAdd * Math.Cos(If(Angle - 90 < 0, 360 + (Angle - 90), Angle - 90) * Math.PI / 180)
    End Function

    Private Shared Function GetY(FromY As Decimal, toAdd As Decimal, Angle As Integer) As Decimal
        Return FromY + toAdd * Math.Sin(If(Angle - 90 < 0, 360 + (Angle - 90), Angle - 90) * Math.PI / 180)
    End Function
End Class

Public Enum eGameStatus
    Ready
    Running
    YouWon
    GameOver
End Enum