Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions

Public Class MyGame
    Inherits BaseControl

    'Properties
    Public Property Title() As String
    Public Property Author() As String
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
    Private SecondsLeft As Integer
    Private GameStatus As eGameStatus = eGameStatus.Ready
    Private CorrectCount As Integer = 0
    Private CorrectStreak As Integer = 0
    Private WrongCount As Integer = 0
    Private Score As Integer = 0

    Public WithEvents elapsedTimer As New Timer() With {.Interval = 500}
    Private timerStart As Date = Nothing
    Private timerEnded As Date = Nothing

    'Tracking Mouse
    Private _mousePos As Point = Point.Empty
    Private _mouseButtonBack, _mouseButtonNext As Rectangle
    Private _mouseButtonBackHovered As Boolean = False, _mouseButtonNextHovered As Boolean = False

    Public Sub New()
        DoubleBuffered = True

        If Phrase = Nothing Then Phrase = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
        _PhraseBackup = Phrase
        If Title = Nothing Then Title = "Demo"
        If Author = Nothing Then Author = "I'm Not MentaL"

        GrayText = ""
        GoldText = Phrase.First
        WhiteText = Phrase.Remove(0, 1)
        LifeLeft = Life
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

    Private Function Progression() As String
        Try
            Dim result = CSng((WhiteText.Length / _PhraseBackup.Length) * 100) - 100
            Return result.ToString("N").Replace("-", "")
        Catch ex As Exception
            Return "100"
        End Try
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
            If letter = " " Then
                drawAt.X += graphics.MeasureString("_", font).Width / 1.6F
            Else
                drawAt.X += graphics.MeasureString(letter, font).Width / 1.6F
            End If
        Next
        'Draw Current Text
        graphics.DrawString(If(highlightText = " ", "_", highlightText), font, brushH, drawAt)
        If highlightText = " " Then
            drawAt.X += graphics.MeasureString("_", font).Width / 1.6F
        Else
            drawAt.X += graphics.MeasureString(highlightText, font).Width / 1.6F
        End If
        'Draw Future Text
        For Each letter As Char In nextText
            graphics.DrawString(letter, font, brushN, drawAt)
            If letter = " " Then
                drawAt.X += graphics.MeasureString("_", font).Width / 1.6F
            Else
                drawAt.X += graphics.MeasureString(letter, font).Width / 1.6F
            End If
        Next

        graphics.ResetTransform()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)

        _mouseButtonBackHovered = False
        _mouseButtonNextHovered = False
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)

        _mousePos = New Point(e.X, e.Y)
        _mouseButtonBackHovered = _mouseButtonBack.Contains(_mousePos)
        _mouseButtonNextHovered = _mouseButtonNext.Contains(_mousePos)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

        Dim cr = ClientRectangle

        If GameStatus = eGameStatus.GameOver Then
            Dim textRect As New RectangleF(0, 100, cr.Width, 210)
            DrawGDIPlusText(g, "GAME OVER", Font, textRect, Color.White, StringAlignment.Center)
            Dim res0Rect As New RectangleF(0, textRect.Y + textRect.Height, cr.Width, 80)
            Dim res1Rect As New RectangleF(0, res0Rect.Y + res0Rect.Height, cr.Width, 80)
            Dim res2Rect As New RectangleF(0, res1Rect.Y + res1Rect.Height, cr.Width, 80)
            Dim res3Rect As New RectangleF(0, res2Rect.Y + res2Rect.Height, cr.Width, 80)
            Dim res4Rect As New RectangleF(0, res3Rect.Y + res3Rect.Height, cr.Width, 80)
            _mouseButtonBack = New Rectangle((cr.Width / 2) - 310, cr.Height - 100, 300, 80)
            _mouseButtonNext = New Rectangle((cr.Width / 2) + 0, cr.Height - 100, 300, 80)
            Using resFont As New Font(Font.FontFamily, Font.Size / 2, FontStyle.Bold)
                DrawGDIPlusText(g, $"{Score} Score", resFont, res0Rect, Color.White, StringAlignment.Center)
                DrawGDIPlusText(g, $"{CorrectCount} Correct letters", resFont, res1Rect, Color.White, StringAlignment.Center)
                DrawGDIPlusText(g, $"{WrongCount} Wrong letters", resFont, res2Rect, Color.White, StringAlignment.Center)
                DrawGDIPlusText(g, $"Time Elapsed {timerEnded.Subtract(timerStart.AddSeconds(CInt($"-{TimeLimit}"))).ToString("mm\:ss")}", resFont, res3Rect, Color.White, StringAlignment.Center)
                DrawGDIPlusText(g, $"{GrayText.WordCount} words typed", resFont, res4Rect, Color.White, StringAlignment.Center)

                Using br As New SolidBrush(If(_mouseButtonBackHovered, Color.White, Color.Gray))
                    g.FillRoundedRectangle(_mouseButtonBack, 10, br, New RoundedRectCorners(True))
                End Using
                Using br As New SolidBrush(If(_mouseButtonNextHovered, Color.White, Color.Gray))
                    g.FillRoundedRectangle(_mouseButtonNext, 10, br, New RoundedRectCorners(True))
                End Using
                DrawGDIPlusText(g, "Back", resFont, _mouseButtonBack, If(_mouseButtonBackHovered, Color.Red, Color.White), StringAlignment.Center)
                DrawGDIPlusText(g, "Try Again", resFont, _mouseButtonNext, If(_mouseButtonNextHovered, Color.Red, Color.White), StringAlignment.Center)
            End Using
        ElseIf GameStatus = eGameStatus.YouWon Then
            Dim textRect As New RectangleF(0, 100, cr.Width, 210)
            DrawGDIPlusText(g, "LEVEL COMPLETED", Font, textRect, Color.Gold, StringAlignment.Center)
            Dim res0Rect As New RectangleF(0, textRect.Y + textRect.Height, cr.Width, 80)
            Dim res1Rect As New RectangleF(0, res0Rect.Y + res0Rect.Height, cr.Width, 80)
            Dim res2Rect As New RectangleF(0, res1Rect.Y + res1Rect.Height, cr.Width, 80)
            Dim res3Rect As New RectangleF(0, res2Rect.Y + res2Rect.Height, cr.Width, 80)
            Dim res4Rect As New RectangleF(0, res3Rect.Y + res3Rect.Height, cr.Width, 80)
            _mouseButtonBack = New Rectangle((cr.Width / 2) - 310, cr.Height - 100, 300, 80)
            _mouseButtonNext = New Rectangle((cr.Width / 2) + 0, cr.Height - 100, 300, 80)
            Using resFont As New Font(Font.FontFamily, Font.Size / 2, FontStyle.Bold)
                DrawGDIPlusText(g, $"{Score} Score", resFont, res0Rect, Color.White, StringAlignment.Center)
                DrawGDIPlusText(g, $"{CorrectCount} Correct letters", resFont, res1Rect, Color.White, StringAlignment.Center)
                DrawGDIPlusText(g, $"{WrongCount} Wrong letters", resFont, res2Rect, Color.White, StringAlignment.Center)
                DrawGDIPlusText(g, $"Time Elapsed {timerEnded.Subtract(timerStart.AddSeconds(CInt($"-{TimeLimit}"))).ToString("mm\:ss")}", resFont, res3Rect, Color.White, StringAlignment.Center)
                DrawGDIPlusText(g, $"{GrayText.WordCount} words typed", resFont, res4Rect, Color.White, StringAlignment.Center)

                Using br As New SolidBrush(If(_mouseButtonBackHovered, Color.White, Color.Gray))
                    g.FillRoundedRectangle(_mouseButtonBack, 10, br, New RoundedRectCorners(True))
                End Using
                Using br As New SolidBrush(If(_mouseButtonNextHovered, Color.White, Color.Gray))
                    g.FillRoundedRectangle(_mouseButtonNext, 10, br, New RoundedRectCorners(True))
                End Using
                DrawGDIPlusText(g, "Back", resFont, _mouseButtonBack, If(_mouseButtonBackHovered, Color.Red, Color.White), StringAlignment.Center)
                DrawGDIPlusText(g, "Next Level", resFont, _mouseButtonNext, If(_mouseButtonNextHovered, Color.Red, Color.White), StringAlignment.Center)
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
                DrawGDIPlusText(g, $"⏲ {SecondsLeft.SecondsToTime}", timeFont, timeElapseRect, If(SecondsLeft <= 10, Color.Red, Color.White), StringAlignment.Near)
                DrawGDIPlusText(g, $"⚔ {Progression()}%", timeFont, progressRect, Color.White, StringAlignment.Far)
                Dim scoreWidth As Single = g.MeasureString($"★ {Score}", timeFont).Width * 1.5
                Dim scoreRect As New RectangleF((cr.Width / 2) - (scoreWidth / 2), cr.Height - 120, scoreWidth, 100)
                DrawGDIPlusText(g, $"★ {Score}", timeFont, scoreRect, Color.Gold, StringAlignment.Center)
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

        Dim allowedChar As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-=_+[]{};':<>,.?/\|`~"" "

        If GameStatus = eGameStatus.Running Then
            If allowedChar.Contains(e.KeyChar) Then
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
                    Score += 1
                    If CorrectStreak >= 50 Then
                        LifeLeft += 1
                        CorrectStreak = 0
                        Score += 100
                    End If
                    Invalidate()
                Else
                    LifeLeft -= 1
                    WrongCount += 1
                    CorrectStreak = 0
                    Score -= 1
                    Invalidate()
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)

        If _mouseButtonBackHovered Then
            frmGame.MainMenu.Show()
            Parent.Controls.Remove(Me)
        End If
        If _mouseButtonNextHovered Then
            If GameStatus = eGameStatus.YouWon Then
                'todo
            Else
                Dim newGame As New MyGame() With {.Phrase = Phrase, .Level = Level, .Life = Life, .TimeLimit = TimeLimit, .Dock = DockStyle.Fill, .Font = Font}
                Parent.Controls.Add(newGame)
                newGame.Refresh()
                Parent.Controls.Remove(Me)
            End If
        End If
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)

        Invalidate()
    End Sub

    Private Sub elapsedTimer_Tick(sender As Object, e As EventArgs) Handles elapsedTimer.Tick
        If LifeLeft < 1 Then
            GameStatus = eGameStatus.GameOver
            timerEnded = Now
            elapsedTimer.Stop()
        Else
            SecondsLeft = CInt(Now.Subtract(timerStart).TotalSeconds)
            SecondsLeft = CInt(SecondsLeft.ToString.Replace("-", ""))
        End If
    End Sub

End Class

Public Enum eGameStatus
    Ready
    Running
    YouWon
    GameOver
End Enum