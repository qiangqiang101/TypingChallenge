Imports System.Drawing.Drawing2D

Public Class MyGame
    Inherits BaseControl

    'Properties
    Public Property Title() As String
    Public Property Author() As String
    Public Property Level() As Integer = 1
    Public Property Phrase() As String
    Public Property LifeLeft() As Integer
    Public Property TimeLimit() As Integer
    Public Property LevelSel() As Control
    Public Property Difficulty() As Integer

    'Texts
    Private GrayText As String
    Private GoldText As Char
    Private WhiteText As String
    Private _PhraseBackup As String

    'Data
    Private SecondsLeft As Integer
    Private GameStatus As eGameStatus = eGameStatus.Ready
    Private CorrectCount As Integer = 0
    Private CorrectStreak As Integer = 0
    Private WrongCount As Integer = 0
    Private Score As Integer = 0

    Public WithEvents elapsedTimer As New Timer() With {.Interval = 500}
    Private timerStart As Date = Nothing
    Private timerEnded As Date = Nothing
    Private timerPaused As Date = Nothing

    'Tracking Mouse
    Private _mousePos As Point = Point.Empty
    Private _mouseButtonBack, _mouseButtonNext As Rectangle
    Private _mouseButtonBackHovered As Boolean = False, _mouseButtonNextHovered As Boolean = False

    Public Sub New(ph As String, _life As Integer, _time As Integer, _diff As Integer)
        DoubleBuffered = True
        Phrase = ph.Replace(vbCr, "").Replace(vbLf, "").Replace("—", "").Replace("\n\n", " ").Replace("  ", " ")

        'If Phrase = Nothing Then Phrase = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
        _PhraseBackup = ph.Replace(vbCr, "").Replace(vbLf, "").Replace("—", "").Replace("\n\n", " ")
        If Title = Nothing Then Title = "Demo"
        If Author = Nothing Then Author = "I'm Not MentaL"

        GrayText = ""
        GoldText = Phrase.First
        WhiteText = Phrase.Remove(0, 1)

        LifeLeft = _life
        TimeLimit = _time
        Difficulty = _diff
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
            g.DrawGDIText("GAME OVER", Font, textRect.ToRectangle, Color.White, TextFormatFlags.HorizontalCenter)
            Dim res0Rect As New RectangleF(0, textRect.Y + textRect.Height, cr.Width, 65)
            Dim res1Rect As New RectangleF(0, res0Rect.Y + res0Rect.Height, cr.Width, 65)
            Dim res2Rect As New RectangleF(0, res1Rect.Y + res1Rect.Height, cr.Width, 65)
            Dim res3Rect As New RectangleF(0, res2Rect.Y + res2Rect.Height, cr.Width, 65)
            Dim res4Rect As New RectangleF(0, res3Rect.Y + res3Rect.Height, cr.Width, 65)
            Dim res5Rect As New RectangleF(0, res4Rect.Y + res4Rect.Height, cr.Width, 65)
            Dim res6Rect As New RectangleF(0, res5Rect.Y + res5Rect.Height, cr.Width, 65)
            _mouseButtonBack = New Rectangle((cr.Width / 2) - 310, cr.Height - 100, 300, 65)
            _mouseButtonNext = New Rectangle((cr.Width / 2) + 0, cr.Height - 100, 300, 65)
            Using resFont As New Font(Font.FontFamily, CSng(Font.Size / 2.5), FontStyle.Regular)
                g.DrawGDIText($"{Score} Score", resFont, res0Rect.ToRectangle, Color.White, TextFormatFlags.HorizontalCenter)
                g.DrawGDIText($"{CorrectCount} Correct letters", resFont, res1Rect.ToRectangle, Color.White, TextFormatFlags.HorizontalCenter)
                g.DrawGDIText($"{WrongCount} Wrong letters", resFont, res2Rect.ToRectangle, Color.White, TextFormatFlags.HorizontalCenter)
                g.DrawGDIText($"Time Elapsed {timerEnded.Subtract(timerStart.AddSeconds(CInt($"-{TimeLimit}"))).ToString("mm\:ss")}", resFont, res3Rect.ToRectangle, Color.White, TextFormatFlags.HorizontalCenter)
                g.DrawGDIText($"{GrayText.WordCount} words typed", resFont, res4Rect.ToRectangle, Color.White, TextFormatFlags.HorizontalCenter)
                g.DrawGDIText($"{CalculateAccuracy(CorrectCount, Phrase)} Accuracy", resFont, res5Rect.ToRectangle, Color.White, TextFormatFlags.HorizontalCenter)
                g.DrawGDIText($"Typing Speed {WPM(GrayText.WordCount, timerEnded.Second)} WPM", resFont, res6Rect.ToRectangle, Color.White, TextFormatFlags.HorizontalCenter)

                Using br As New SolidBrush(If(_mouseButtonBackHovered, Color.White, Color.Gray))
                    g.FillRoundedRectangle(_mouseButtonBack, 10, br, New RoundedRectCorners(True))
                End Using
                Using br As New SolidBrush(If(_mouseButtonNextHovered, Color.White, Color.Gray))
                    g.FillRoundedRectangle(_mouseButtonNext, 10, br, New RoundedRectCorners(True))
                End Using

                g.DrawGDIText("Back", resFont, _mouseButtonBack, If(_mouseButtonBackHovered, Color.Red, Color.White), TextFormatFlags.HorizontalCenter)
                g.DrawGDIText("Try Again", resFont, _mouseButtonNext, If(_mouseButtonNextHovered, Color.Red, Color.White), TextFormatFlags.HorizontalCenter)
            End Using
        ElseIf GameStatus = eGameStatus.YouWon Then
            Dim textRect As New RectangleF(0, 100, cr.Width, 210)
            g.DrawGDIText("LEVEL COMPLETED", Font, textRect.ToRectangle, Color.Gold, TextFormatFlags.HorizontalCenter)
            Dim res0Rect As New RectangleF(0, textRect.Y + textRect.Height, cr.Width, 65)
            Dim res1Rect As New RectangleF(0, res0Rect.Y + res0Rect.Height, cr.Width, 65)
            Dim res2Rect As New RectangleF(0, res1Rect.Y + res1Rect.Height, cr.Width, 65)
            Dim res3Rect As New RectangleF(0, res2Rect.Y + res2Rect.Height, cr.Width, 65)
            Dim res4Rect As New RectangleF(0, res3Rect.Y + res3Rect.Height, cr.Width, 65)
            Dim res5Rect As New RectangleF(0, res4Rect.Y + res4Rect.Height, cr.Width, 65)
            Dim res6Rect As New RectangleF(0, res5Rect.Y + res5Rect.Height, cr.Width, 65)
            _mouseButtonBack = New Rectangle((cr.Width / 2) - 310, cr.Height - 100, 300, 65)
            _mouseButtonNext = New Rectangle((cr.Width / 2) + 0, cr.Height - 100, 300, 65)
            Using resFont As New Font(Font.FontFamily, CSng(Font.Size / 2.5), FontStyle.Regular)
                g.DrawGDIText($"{Score} Score", resFont, res0Rect.ToRectangle, Color.White, TextFormatFlags.HorizontalCenter)
                g.DrawGDIText($"{CorrectCount} Correct letters", resFont, res1Rect.ToRectangle, Color.White, TextFormatFlags.HorizontalCenter)
                g.DrawGDIText($"{WrongCount} Wrong letters", resFont, res2Rect.ToRectangle, Color.White, TextFormatFlags.HorizontalCenter)
                g.DrawGDIText($"Time Elapsed {timerEnded.Subtract(timerStart.AddSeconds(CInt($"-{TimeLimit}"))).ToString("mm\:ss")}", resFont, res3Rect.ToRectangle, Color.White, TextFormatFlags.HorizontalCenter)
                g.DrawGDIText($"{GrayText.WordCount} words typed", resFont, res4Rect.ToRectangle, Color.White, TextFormatFlags.HorizontalCenter)
                g.DrawGDIText($"{CalculateAccuracy(CorrectCount, Phrase)} Accuracy", resFont, res5Rect.ToRectangle, Color.White, TextFormatFlags.HorizontalCenter)
                g.DrawGDIText($"Typing Speed {WPM(GrayText.WordCount, timerEnded.Second)} WPM", resFont, res6Rect.ToRectangle, Color.White, TextFormatFlags.HorizontalCenter)

                Using br As New SolidBrush(If(_mouseButtonBackHovered, Color.White, Color.Gray))
                    g.FillRoundedRectangle(_mouseButtonBack, 10, br, New RoundedRectCorners(True))
                End Using
                Using br As New SolidBrush(If(_mouseButtonNextHovered, Color.White, Color.Gray))
                    g.FillRoundedRectangle(_mouseButtonNext, 10, br, New RoundedRectCorners(True))
                End Using
                g.DrawGDIText("Back", resFont, _mouseButtonBack, If(_mouseButtonBackHovered, Color.Red, Color.White), TextFormatFlags.HorizontalCenter)
                g.DrawGDIText("Next Level", resFont, _mouseButtonNext, If(_mouseButtonNextHovered, Color.Red, Color.White), TextFormatFlags.HorizontalCenter)
            End Using
        ElseIf GameStatus = eGameStatus.Ready Then
            Dim lvlRect As New RectangleF(0, (cr.Height / 2) - 300, cr.Width, 120)
            Dim titleRect As New RectangleF(0, (cr.Height / 2) - 300 + lvlRect.Height, cr.Width, 80)
            g.DrawGDIText($"LEVEL {Level}", Font, lvlRect.ToRectangle, Color.AliceBlue, TextFormatFlags.HorizontalCenter)
            Using titleFont As New Font(Font.FontFamily, Font.Size / 2, FontStyle.Regular)
                g.DrawGDIText(Title.Replace("&", "&&").Replace("&&&", "&&"), titleFont, titleRect.ToRectangle, Color.AliceBlue, TextFormatFlags.HorizontalCenter)
            End Using
            Dim textRect As New RectangleF(cr.Width / 2, cr.Height / 2, cr.Width, 210)
            Using timeFont As New Font(Font.FontFamily, Font.Size / 2, FontStyle.Regular)
                g.DrawInstructionalButton(textRect.ToRectangle, "Press ~Enter~ to Start.", timeFont, Color.AliceBlue)
            End Using
        ElseIf GameStatus = eGameStatus.Paused Then
            Dim textRect As New RectangleF(0, (cr.Height / 2) - 120, cr.Width, 210)
            Dim textRect2 As New RectangleF(cr.Width / 2, cr.Height / 2, cr.Width, 210)
            g.DrawGDIText($"PAUSED", Font, textRect.ToRectangle, Color.AliceBlue, TextFormatFlags.HorizontalCenter)
            Dim subRect As New Rectangle(textRect.X, textRect.Y + textRect.Height, textRect.Width, textRect.Height)
            Using timeFont As New Font(Font.FontFamily, Font.Size / 2, FontStyle.Regular)
                g.DrawInstructionalButton(textRect2.ToRectangle, "Press ~ESC~ to resume or Press ~Enter~ to Quit.", timeFont, Color.AliceBlue)
            End Using
        Else
            Dim sf = cr.GetSafeZone
            Dim lifeRect As New RectangleF(sf.X, sf.Y, (sf.Width / 2), 100)
            Dim lvlRect As New RectangleF(sf.X + (sf.Width / 2), sf.Y, (sf.Width / 2), 100)
            Using heartFont As New Font(Font.FontFamily, Font.Size / 2, FontStyle.Bold)
                g.DrawGDIText(HeartsLeft(LifeLeft), heartFont, lifeRect.ToRectangle, Color.Red, TextFormatFlags.Left)
                g.DrawGDIText($"Level {Level}", heartFont, lvlRect.ToRectangle, Color.White, TextFormatFlags.Right)
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

            Dim timeElapseRect As New RectangleF(sf.X, sf.Height - 120, (sf.Width / 2), 100)
            Dim progressRect As New RectangleF(sf.X + (sf.Width / 2), sf.Height - 120, (sf.Width / 2), 100)

            Using timeFont As New Font(Font.FontFamily, Font.Size / 2, FontStyle.Bold)
                g.DrawGDIText($"⏲ {SecondsLeft.SecondsToTime}", timeFont, timeElapseRect.ToRectangle, If(SecondsLeft >= -10, Color.Red, Color.White), TextFormatFlags.Left)
                g.DrawGDIText($"⚔ {Progression()}%", timeFont, progressRect.ToRectangle, Color.White, TextFormatFlags.Right)
                Dim scoreWidth As Single = g.MeasureString($"★ {Score}", timeFont).Width * 1.5
                Dim scoreRect As New RectangleF(sf.X + (sf.Width / 2) - (scoreWidth / 2), sf.Height - 120, scoreWidth, 100)
                g.DrawGDIText($"★ {Score}", timeFont, scoreRect.ToRectangle, Color.Gold, TextFormatFlags.HorizontalCenter)
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
        ElseIf GameStatus = eGameStatus.Running Then
            If e.KeyCode = Keys.Escape Then
                GameStatus = eGameStatus.Paused
                timerPaused = Now
                If elapsedTimer.Enabled Then elapsedTimer.Stop()
            End If
        ElseIf GameStatus = eGameStatus.Paused Then
            If e.KeyCode = Keys.Escape Then
                GameStatus = eGameStatus.Running
                timerStart = Now.AddSeconds((timerStart - timerPaused).TotalSeconds)
                If Not elapsedTimer.Enabled Then elapsedTimer.Start()
            End If
            If e.KeyCode = Keys.Enter Then
                LevelSel.Show()
                Parent.Controls.Remove(Me)
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
                        soundLevelComplete.PlayWav
                        GameStatus = eGameStatus.YouWon
                        timerEnded = Now
                        If elapsedTimer.Enabled Then elapsedTimer.Stop()
                    End Try
                    CorrectCount += 1
                    CorrectStreak += 1
                    Score += 1 * (Difficulty + 1)
                    Select Case Difficulty
                        Case 0
                            If CorrectStreak >= 20 Then
                                soundLife.PlayWav
                                LifeLeft += 1
                                CorrectStreak = 0
                                Score += 100 * (Difficulty + 1)
                            End If
                        Case 1
                            If CorrectStreak >= 30 Then
                                soundLife.PlayWav
                                LifeLeft += 1
                                CorrectStreak = 0
                                Score += 200 * (Difficulty + 1)
                            End If
                        Case 2
                            If CorrectStreak >= 50 Then
                                soundLife.PlayWav
                                LifeLeft += 1
                                CorrectStreak = 0
                                Score += 300 * (Difficulty + 1)
                            End If
                    End Select

                    Invalidate()
                Else
                    soundMinusLife.PlayWav
                    LifeLeft -= 1
                    WrongCount += 1
                    CorrectStreak = 0
                    Score -= 1 * (Difficulty + 1)
                    Invalidate()
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)

        If _mouseButtonBackHovered Then
            soundBtnCancel.PlayWav

            If GameStatus = eGameStatus.YouWon Then
                SaveUserProgress()
            End If

            LevelSel.Show()
            Parent.Controls.Remove(Me)
        End If
        If _mouseButtonNextHovered Then
            soundBtnSelect.PlayWav

            If GameStatus = eGameStatus.YouWon Then
                SaveUserProgress()

                Try
                    Dim nextlevel As Level = levels.LevelList.Find(Function(x) x.Level = Level + 1)
                    Dim newGame As MyGame

                    Select Case setting.Difficulty
                        Case 0
                            newGame = New MyGame(nextlevel.Phrase, nextlevel.Life, nextlevel.TimeLimit, 0) With {.Title = nextlevel.Title, .Author = nextlevel.Author, .Level = nextlevel.Level, .LevelSel = LevelSel, .Dock = DockStyle.Fill, .Font = Font}
                        Case 1
                            newGame = New MyGame(nextlevel.Phrase, nextlevel.Life / 2, (nextlevel.TimeLimit / 3) * 2, 1) With {.Title = nextlevel.Title, .Author = nextlevel.Author, .Level = nextlevel.Level, .LevelSel = LevelSel, .Dock = DockStyle.Fill, .Font = Font}
                        Case Else
                            newGame = New MyGame(nextlevel.Phrase, 1, nextlevel.TimeLimit / 2, 2) With {.Title = nextlevel.Title, .Author = nextlevel.Author, .Level = nextlevel.Level, .LevelSel = LevelSel, .Dock = DockStyle.Fill, .Font = Font}
                    End Select

                    Parent.Controls.Add(newGame)
                    newGame.Refresh()
                    Parent.Controls.Remove(Me)
                Catch ex As Exception
                    Dim credits As New Credits() With {.Dock = DockStyle.Fill, .Font = Font}
                    Parent.Controls.Add(credits)
                    credits.Refresh()
                    credits.Start()
                    Parent.Controls.Remove(Me)
                End Try
            Else
                Dim nextlevel As Level = levels.LevelList.Find(Function(x) x.Level = Level)
                Dim newGame As MyGame

                Select Case setting.Difficulty
                    Case 0
                        newGame = New MyGame(nextlevel.Phrase, nextlevel.Life, nextlevel.TimeLimit, 0) With {.Title = nextlevel.Title, .Author = nextlevel.Author, .Level = nextlevel.Level, .LevelSel = LevelSel, .Dock = DockStyle.Fill, .Font = Font}
                    Case 1
                        newGame = New MyGame(nextlevel.Phrase, nextlevel.Life / 2, (nextlevel.TimeLimit / 3) * 2, 1) With {.Title = nextlevel.Title, .Author = nextlevel.Author, .Level = nextlevel.Level, .LevelSel = LevelSel, .Dock = DockStyle.Fill, .Font = Font}
                    Case Else
                        newGame = New MyGame(nextlevel.Phrase, 1, nextlevel.TimeLimit / 2, 2) With {.Title = nextlevel.Title, .Author = nextlevel.Author, .Level = nextlevel.Level, .LevelSel = LevelSel, .Dock = DockStyle.Fill, .Font = Font}
                End Select

                Parent.Controls.Add(newGame)
                newGame.Refresh()
                Parent.Controls.Remove(Me)
            End If
        End If
    End Sub

    Private Sub SaveUserProgress()
        Select Case setting.Difficulty
            Case 0
                Dim existLevels = profile.ClearedLevel.Where(Function(x) x.Level = Level).Count
                If existLevels = 0 Then
                    profile.ClearedLevel.Add(New UserLevel(Title, Level, Score))
                Else
                    Dim existLevel = profile.ClearedLevel.Find(Function(x) x.Level = Level)
                    If Score > existLevel.Score Then
                        profile.ClearedLevel.Remove(existLevel)
                        profile.ClearedLevel.Add(New UserLevel(Title, Level, Score))
                    End If
                End If

                Dim newProfile As New ProfileData(prfXmlPath)
                With newProfile
                    .Name = profile.Name
                    .DateCreated = profile.DateCreated
                    .ClearedLevel = profile.ClearedLevel
                    .ClearedHardLevel = profile.ClearedHardLevel
                    .ClearedVHardLevel = profile.ClearedVHardLevel
                    .Credits = profile.Credits + (Score / 100)
                End With
                newProfile.Save()
                profile = newProfile
            Case 1
                Dim existLevels = profile.ClearedHardLevel.Where(Function(x) x.Level = Level).Count
                If existLevels = 0 Then
                    profile.ClearedHardLevel.Add(New UserLevel(Title, Level, Score))
                Else
                    Dim existLevel = profile.ClearedHardLevel.Find(Function(x) x.Level = Level)
                    If Score > existLevel.Score Then
                        profile.ClearedHardLevel.Remove(existLevel)
                        profile.ClearedHardLevel.Add(New UserLevel(Title, Level, Score))
                    End If
                End If

                Dim newProfile As New ProfileData(prfXmlPath)
                With newProfile
                    .Name = profile.Name
                    .DateCreated = profile.DateCreated
                    .ClearedLevel = profile.ClearedLevel
                    .ClearedHardLevel = profile.ClearedHardLevel
                    .ClearedVHardLevel = profile.ClearedVHardLevel
                    .Credits = profile.Credits + (Score / 100)
                End With
                newProfile.Save()
                profile = newProfile
            Case 2
                Dim existLevels = profile.ClearedVHardLevel.Where(Function(x) x.Level = Level).Count
                If existLevels = 0 Then
                    profile.ClearedVHardLevel.Add(New UserLevel(Title, Level, Score))
                Else
                    Dim existLevel = profile.ClearedVHardLevel.Find(Function(x) x.Level = Level)
                    If Score > existLevel.Score Then
                        profile.ClearedVHardLevel.Remove(existLevel)
                        profile.ClearedVHardLevel.Add(New UserLevel(Title, Level, Score))
                    End If
                End If

                Dim newProfile As New ProfileData(prfXmlPath)
                With newProfile
                    .Name = profile.Name
                    .DateCreated = profile.DateCreated
                    .ClearedLevel = profile.ClearedLevel
                    .ClearedHardLevel = profile.ClearedHardLevel
                    .ClearedVHardLevel = profile.ClearedVHardLevel
                    .Credits = profile.Credits + (Score / 100)
                End With
                newProfile.Save()
                profile = newProfile
        End Select
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)

        Invalidate()
    End Sub

    Private Sub elapsedTimer_Tick(sender As Object, e As EventArgs) Handles elapsedTimer.Tick
        If LifeLeft < 1 Then
            soundLevelFailed.PlayWav
            GameStatus = eGameStatus.GameOver
            timerEnded = Now
            elapsedTimer.Stop()
        ElseIf SecondsLeft >= 1 Then
            soundLevelFailed.PlayWav
            GameStatus = eGameStatus.GameOver
            timerEnded = Now
            elapsedTimer.Stop()
        Else
            SecondsLeft = CInt(Now.Subtract(timerStart).TotalSeconds)
        End If
    End Sub

End Class

Public Enum eGameStatus
    Ready
    Running
    YouWon
    GameOver
    Paused
End Enum