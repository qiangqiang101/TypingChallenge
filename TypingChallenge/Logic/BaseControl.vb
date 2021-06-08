Imports System.Drawing.Drawing2D

Public Class BaseControl
    Inherits Control

    Public WithEvents bgTimer As New Timer() With {.Interval = 1, .Enabled = False}

    'Circles
    Private MainRect As Rectangle
    Private Shared rd As New Random
    Private Shared Circles(260) As cCircle
    Private Shared PointDistance As Decimal = 100
    Private Shared ea As MouseEventArgs

    'Audio
    Private player As AudioFile

    'Keyboard
    Private kbPosX As Integer = (ClientSize.Width / 2) - 800, kbPosY As Integer = (ClientSize.Height / 2) - 225
    Private moveStepX As Integer = 2, moveStepY As Integer = 2
    Private pressedKeys As New List(Of Keys)
    Private rgbNum, rgbCount As Integer, rgbAngle As Single = 60.0F
    Private keyboardRect As Rectangle

    Public Property Effect() As eEffect = eEffect.Both
    Public Property KeyboardColor() As Color = Color.White
    Public Property RGB() As Boolean = False

    Public Sub New()
        KeyboardColor = Color.FromArgb(setting.KeyboardColorA, setting.KeyboardColorR, setting.KeyboardColorG, setting.KeyboardColorB)
        RGB = setting.KeyboardRGB

        Select Case Effect
            Case eEffect.Circles, eEffect.Both
                Select Case setting.Quality
                    Case 0
                        Array.Resize(Circles, 30)
                    Case 1
                        Array.Resize(Circles, 60)
                    Case 2
                        Array.Resize(Circles, 120)
                    Case 3
                        Array.Resize(Circles, 240)
                    Case 4
                        Array.Resize(Circles, 360)
                End Select

                PrepareCircles()
            Case Else
                Array.Resize(Circles, 0)
        End Select

        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Runtime Then
            bgTimer.Start()
            player = New AudioFile(".\audio\bgm.mp3")
            player.Play()
            player.SetVolume(setting.MusicVolume)
        End If

        DoubleBuffered = True
    End Sub

    Public Sub RefreshSettings()
        setting = New SettingData(setXmlPath).Instance

        KeyboardColor = Color.FromArgb(setting.KeyboardColorA, setting.KeyboardColorR, setting.KeyboardColorG, setting.KeyboardColorB)
        RGB = setting.KeyboardRGB

        Select Case Effect
            Case eEffect.Circles, eEffect.Both
                Select Case setting.Quality
                    Case 0
                        Array.Resize(Circles, 30)
                    Case 1
                        Array.Resize(Circles, 60)
                    Case 2
                        Array.Resize(Circles, 120)
                    Case 3
                        Array.Resize(Circles, 240)
                    Case 4
                        Array.Resize(Circles, 360)
                End Select

                PrepareCircles()
            Case Else
                Array.Resize(Circles, 0)
        End Select

        player.SetVolume(setting.MusicVolume)

        Invalidate()
    End Sub

    Private Sub PrepareCircles()
        'get form rectangle
        MainRect = DisplayRectangle

        'prepare circles
        For i As Integer = 0 To Circles.Count - 1
            Circles(i) = New cCircle(MainRect)
        Next
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)

        ea = Nothing
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)

        ea = e
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        If Not pressedKeys.Contains(e.KeyCode) Then pressedKeys.Add(e.KeyCode)
    End Sub

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        MyBase.OnKeyUp(e)

        If pressedKeys.Contains(e.KeyCode) Then pressedKeys.Remove(e.KeyCode)
    End Sub

    Private currentFrameRate As Integer
    Private Sub advanceFrameRate()
        Static ptlu As Double ' Time of last framerate update.
        ' Show me hardware that can do 32k F/sec...
        Static callCount As Integer
        ' Increment the callCounter
        callCount += 1
        ' Change 1000 if an alternate time value is desired.
        If (Environment.TickCount - ptlu) >= 1000 Then

            currentFrameRate = callCount
            ' Reset the callCount, AFTER updating the value.
            callCount = 0
            ' Reset the timeUpdated
            ptlu = Environment.TickCount
        Else
        End If
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

        If setting.ShowFPS Then
            Dim fps As New Rectangle(ClientRectangle.Width - 200, 0, 200, 50)
            Using fpsFont As New Font("Verdana", 30, FontStyle.Regular, GraphicsUnit.Pixel)
                Call advanceFrameRate()
                g.DrawGDIPlusText(currentFrameRate, fpsFont, fps, Color.LimeGreen, StringAlignment.Far)
            End Using
        End If

        If Effect = eEffect.Circles Then
            PaintCircles(g)
        ElseIf Effect = eEffect.Keyboard Then
            'g.Clear(BackColor)
            PaintKeyboard(g, New Point(kbPosX, kbPosY))
        ElseIf Effect = eEffect.Both Then
            PaintCircles(g)
            'g.Clear(BackColor)
            PaintKeyboard(g, New Point(kbPosX, kbPosY))
        End If
    End Sub

    Private Sub PaintCircles(g As Graphics)
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
            Circles(i).Show(g)
            Circles(i).Update()
        Next
    End Sub

    Private Sub MoveKeyboard()
        If Me.Visible Then
            kbPosX += moveStepX
            If kbPosX < 0 OrElse kbPosX + 1600 > ClientSize.Width Then moveStepX = -moveStepX
            kbPosY += moveStepY
            If kbPosY < 0 OrElse kbPosY + 450 > ClientSize.Height Then moveStepY = -moveStepY
            Invalidate()
        End If
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)

        If Effect = eEffect.Circles Then PrepareCircles()
        If Effect = eEffect.Keyboard Then
            kbPosX = (ClientSize.Width / 2) - 800
            kbPosY = (ClientSize.Height / 2) - 225
        End If
        If Effect = eEffect.Both Then
            PrepareCircles()
            kbPosX = (ClientSize.Width / 2) - 800
            kbPosY = (ClientSize.Height / 2) - 225
        End If
        Invalidate()
    End Sub

    Private Sub bgTimer_Tick(sender As Object, e As EventArgs) Handles bgTimer.Tick
        Select Case Effect
            Case eEffect.Keyboard, eEffect.Both
                MoveKeyboard()
                rgbCount += 1
                rgbAngle += 1.0F

                Select Case rgbCount
                    Case 0 To 99
                        rgbNum = 0
                    Case 100 To 199
                        rgbNum = 1
                    Case 200 To 299
                        rgbNum = 2
                End Select

                If rgbCount > 299 Then rgbCount = 0
        End Select

        Invalidate()
    End Sub

    Public Sub PaintKeyboard(g As Graphics, offset As Point)
        Dim rect As New Rectangle(offset.X, offset.Y, 1600, 450)
        Dim kbBorder As New Rectangle(rect.X - 10, rect.Y - 10, rect.Width, rect.Height)
        keyboardRect = kbBorder
        Dim ks = 60, lks = 131, bks = 70, tks = 100, sks = ks * 7, eks = 141
        Dim xy = 10

        Dim esc As New Rectangle(rect.X + xy, rect.Y + xy, ks, ks)
        Dim f1 As New Rectangle(xy + esc.X + esc.Width + 47, rect.Y + xy, ks, ks)
        Dim f2 As New Rectangle(xy + f1.X + f1.Width, rect.Y + xy, ks, ks)
        Dim f3 As New Rectangle(xy + f2.X + f2.Width, rect.Y + xy, ks, ks)
        Dim f4 As New Rectangle(xy + f3.X + f3.Width, rect.Y + xy, ks, ks)
        Dim f5 As New Rectangle(xy + f4.X + f4.Width + 47, rect.Y + xy, ks, ks)
        Dim f6 As New Rectangle(xy + f5.X + f5.Width, rect.Y + xy, ks, ks)
        Dim f7 As New Rectangle(xy + f6.X + f6.Width, rect.Y + xy, ks, ks)
        Dim f8 As New Rectangle(xy + f7.X + f7.Width, rect.Y + xy, ks, ks)
        Dim f9 As New Rectangle(xy + f8.X + f8.Width + 47, rect.Y + xy, ks, ks)
        Dim f10 As New Rectangle(xy + f9.X + f9.Width, rect.Y + xy, ks, ks)
        Dim f11 As New Rectangle(xy + f10.X + f10.Width, rect.Y + xy, ks, ks)
        Dim f12 As New Rectangle(xy + f11.X + f11.Width, rect.Y + xy, ks, ks)
        Dim prnscn As New Rectangle(xy + f12.X + f12.Width + 15, rect.Y + xy, ks, ks)
        Dim scroll As New Rectangle(xy + prnscn.X + prnscn.Width, rect.Y + xy, ks, ks)
        Dim pause As New Rectangle(xy + scroll.X + scroll.Width, rect.Y + xy, ks, ks)

        Dim tilde As New Rectangle(esc.X, xy + esc.Y + esc.Height, ks, ks)
        Dim d1 As New Rectangle(xy + tilde.X + tilde.Width, xy + esc.Y + esc.Height, ks, ks)
        Dim d2 As New Rectangle(xy + d1.X + d1.Width, xy + esc.Y + esc.Height, ks, ks)
        Dim d3 As New Rectangle(xy + d2.X + d2.Width, xy + esc.Y + esc.Height, ks, ks)
        Dim d4 As New Rectangle(xy + d3.X + d3.Width, xy + esc.Y + esc.Height, ks, ks)
        Dim d5 As New Rectangle(xy + d4.X + d4.Width, xy + esc.Y + esc.Height, ks, ks)
        Dim d6 As New Rectangle(xy + d5.X + d5.Width, xy + esc.Y + esc.Height, ks, ks)
        Dim d7 As New Rectangle(xy + d6.X + d6.Width, xy + esc.Y + esc.Height, ks, ks)
        Dim d8 As New Rectangle(xy + d7.X + d7.Width, xy + esc.Y + esc.Height, ks, ks)
        Dim d9 As New Rectangle(xy + d8.X + d8.Width, xy + esc.Y + esc.Height, ks, ks)
        Dim d0 As New Rectangle(xy + d9.X + d9.Width, xy + esc.Y + esc.Height, ks, ks)
        Dim minus As New Rectangle(xy + d0.X + d0.Width, xy + esc.Y + esc.Height, ks, ks)
        Dim plus As New Rectangle(xy + minus.X + minus.Width, xy + esc.Y + esc.Height, ks, ks)
        Dim bspace As New Rectangle(xy + plus.X + plus.Width, xy + esc.Y + esc.Height, 131, ks)
        Dim ins As New Rectangle(xy + bspace.X + bspace.Width + 15, xy + esc.Y + esc.Height, ks, ks)
        Dim home As New Rectangle(xy + ins.X + ins.Width, xy + esc.Y + esc.Height, ks, ks)
        Dim pgup As New Rectangle(xy + home.X + home.Width, xy + esc.Y + esc.Height, ks, ks)
        Dim numlock As New Rectangle(xy + pgup.X + pgup.Width + 15, xy + esc.Y + esc.Height, ks, ks)
        Dim numslash As New Rectangle(xy + numlock.X + numlock.Width, xy + esc.Y + esc.Height, ks, ks)
        Dim numstar As New Rectangle(xy + numslash.X + numslash.Width, xy + esc.Y + esc.Height, ks, ks)
        Dim numminus As New Rectangle(xy + numstar.X + numstar.Width, xy + esc.Y + esc.Height, ks, ks)

        Dim tab As New Rectangle(tilde.X, xy + tilde.Y + tilde.Height, tks, ks)
        Dim q As New Rectangle(xy + tab.X + tab.Width, xy + tilde.Y + tilde.Height, ks, ks)
        Dim w As New Rectangle(xy + q.X + q.Width, xy + tilde.Y + tilde.Height, ks, ks)
        Dim e As New Rectangle(xy + w.X + w.Width, xy + tilde.Y + tilde.Height, ks, ks)
        Dim r As New Rectangle(xy + e.X + e.Width, xy + tilde.Y + tilde.Height, ks, ks)
        Dim t As New Rectangle(xy + r.X + r.Width, xy + tilde.Y + tilde.Height, ks, ks)
        Dim yy As New Rectangle(xy + t.X + t.Width, xy + tilde.Y + tilde.Height, ks, ks)
        Dim u As New Rectangle(xy + yy.X + yy.Width, xy + tilde.Y + tilde.Height, ks, ks)
        Dim i As New Rectangle(xy + u.X + u.Width, xy + tilde.Y + tilde.Height, ks, ks)
        Dim o As New Rectangle(xy + i.X + i.Width, xy + tilde.Y + tilde.Height, ks, ks)
        Dim p As New Rectangle(xy + o.X + o.Width, xy + tilde.Y + tilde.Height, ks, ks)
        Dim lbracket As New Rectangle(xy + p.X + p.Width, xy + tilde.Y + tilde.Height, ks, ks)
        Dim rbracket As New Rectangle(xy + lbracket.X + lbracket.Width, xy + tilde.Y + tilde.Height, ks, ks)
        Dim bslash As New Rectangle(xy + rbracket.X + rbracket.Width, xy + tilde.Y + tilde.Height, 91, ks)
        Dim del As New Rectangle(xy + bslash.X + bslash.Width + 15, xy + tilde.Y + tilde.Height, ks, ks)
        Dim [end] As New Rectangle(xy + del.X + del.Width, xy + tilde.Y + tilde.Height, ks, ks)
        Dim pgdown As New Rectangle(xy + [end].X + [end].Width, xy + tilde.Y + tilde.Height, ks, ks)
        Dim num7 As New Rectangle(xy + pgdown.X + pgdown.Width + 15, xy + tilde.Y + tilde.Height, ks, ks)
        Dim num8 As New Rectangle(xy + num7.X + num7.Width, xy + tilde.Y + tilde.Height, ks, ks)
        Dim num9 As New Rectangle(xy + num8.X + num8.Width, xy + tilde.Y + tilde.Height, ks, ks)
        Dim numplus As New Rectangle(xy + num9.X + num9.Width, xy + tilde.Y + tilde.Height, ks, lks)

        Dim capslock As New Rectangle(tab.X, xy + tab.Y + tab.Height, 120, ks)
        Dim a As New Rectangle(xy + capslock.X + capslock.Width, xy + tab.Y + tab.Height, ks, ks)
        Dim s As New Rectangle(xy + a.X + a.Width, xy + tab.Y + tab.Height, ks, ks)
        Dim d As New Rectangle(xy + s.X + s.Width, xy + tab.Y + tab.Height, ks, ks)
        Dim f As New Rectangle(xy + d.X + d.Width, xy + tab.Y + tab.Height, ks, ks)
        Dim gg As New Rectangle(xy + f.X + f.Width, xy + tab.Y + tab.Height, ks, ks)
        Dim h As New Rectangle(xy + gg.X + gg.Width, xy + tab.Y + tab.Height, ks, ks)
        Dim j As New Rectangle(xy + h.X + h.Width, xy + tab.Y + tab.Height, ks, ks)
        Dim k As New Rectangle(xy + j.X + j.Width, xy + tab.Y + tab.Height, ks, ks)
        Dim l As New Rectangle(xy + k.X + k.Width, xy + tab.Y + tab.Height, ks, ks)
        Dim semicolon As New Rectangle(xy + l.X + l.Width, xy + tab.Y + tab.Height, ks, ks)
        Dim quote As New Rectangle(xy + semicolon.X + semicolon.Width, xy + tab.Y + tab.Height, ks, ks)
        Dim enter As New Rectangle(xy + quote.X + quote.Width, xy + tab.Y + tab.Height, eks, ks)
        Dim num4 As New Rectangle(xy + enter.X + enter.Width + (ks * 4), xy + tab.Y + tab.Height, ks, ks)
        Dim num5 As New Rectangle(xy + num4.X + num4.Width, xy + tab.Y + tab.Height, ks, ks)
        Dim num6 As New Rectangle(xy + num5.X + num5.Width, xy + tab.Y + tab.Height, ks, ks)

        Dim lshift As New Rectangle(capslock.X, xy + capslock.Y + capslock.Height, 160, ks)
        Dim z As New Rectangle(xy + lshift.X + lshift.Width, xy + capslock.Y + capslock.Height, ks, ks)
        Dim xx As New Rectangle(xy + z.X + z.Width, xy + capslock.Y + capslock.Height, ks, ks)
        Dim c As New Rectangle(xy + xx.X + xx.Width, xy + capslock.Y + capslock.Height, ks, ks)
        Dim v As New Rectangle(xy + c.X + c.Width, xy + capslock.Y + capslock.Height, ks, ks)
        Dim b As New Rectangle(xy + v.X + v.Width, xy + capslock.Y + capslock.Height, ks, ks)
        Dim n As New Rectangle(xy + b.X + b.Width, xy + capslock.Y + capslock.Height, ks, ks)
        Dim m As New Rectangle(xy + n.X + n.Width, xy + capslock.Y + capslock.Height, ks, ks)
        Dim comma As New Rectangle(xy + m.X + m.Width, xy + capslock.Y + capslock.Height, ks, ks)
        Dim period As New Rectangle(xy + comma.X + comma.Width, xy + capslock.Y + capslock.Height, ks, ks)
        Dim question As New Rectangle(xy + period.X + period.Width, xy + capslock.Y + capslock.Height, ks, ks)
        Dim rshift As New Rectangle(xy + question.X + question.Width, xy + capslock.Y + capslock.Height, 171, ks)
        Dim up As New Rectangle(xy + rshift.X + rshift.Width + 25 + ks, xy + capslock.Y + capslock.Height, ks, ks)
        Dim num1 As New Rectangle(xy + up.X + up.Width + 25 + ks, xy + capslock.Y + capslock.Height, ks, ks)
        Dim num2 As New Rectangle(xy + num1.X + num1.Width, xy + capslock.Y + capslock.Height, ks, ks)
        Dim num3 As New Rectangle(xy + num2.X + num2.Width, xy + capslock.Y + capslock.Height, ks, ks)
        Dim numenter As New Rectangle(xy + num3.X + num3.Width, xy + capslock.Y + capslock.Height, ks, lks)

        Dim lctrl As New Rectangle(lshift.X, xy + lshift.Y + lshift.Height, tks, ks)
        Dim lwin As New Rectangle(xy + lctrl.X + lctrl.Width, xy + lshift.Y + lshift.Height, bks, ks)
        Dim lalt As New Rectangle(xy + lwin.X + lwin.Width, xy + lshift.Y + lshift.Height, bks, ks)
        Dim space As New Rectangle(xy + lalt.X + lalt.Width, xy + lshift.Y + lshift.Height, sks, ks)
        Dim ralt As New Rectangle(xy + space.X + space.Width, xy + lshift.Y + lshift.Height, bks, ks)
        Dim rwin As New Rectangle(xy + ralt.X + ralt.Width, xy + lshift.Y + lshift.Height, bks, ks)
        Dim menu As New Rectangle(xy + rwin.X + rwin.Width, xy + lshift.Y + lshift.Height, bks, ks)
        Dim rctrl As New Rectangle(xy + menu.X + menu.Width, xy + lshift.Y + lshift.Height, 101, ks)
        Dim left As New Rectangle(xy + rctrl.X + rctrl.Width + 15, xy + lshift.Y + lshift.Height, ks, ks)
        Dim down As New Rectangle(xy + left.X + left.Width, xy + lshift.Y + lshift.Height, ks, ks)
        Dim right As New Rectangle(xy + down.X + down.Width, xy + lshift.Y + lshift.Height, ks, ks)
        Dim num0 As New Rectangle(xy + right.X + right.Width + 15, xy + lshift.Y + lshift.Height, lks, ks)
        Dim numperiod As New Rectangle(xy + num0.X + num0.Width, xy + lshift.Y + lshift.Height, ks, ks)

        Using brush As New SolidBrush(KeyboardColor)
            Using pen As New Pen(brush, 1.5)
                g.DrawRoundedRectangle(kbBorder, 10, pen)

                Using kbFont As New Font("Verdana", 15.0F, FontStyle.Bold, GraphicsUnit.Pixel)
                    DrawKeys(g, esc, pen, "ESC", kbFont, Keys.Escape)
                    DrawKeys(g, f1, pen, "F1", kbFont, Keys.F1)
                    DrawKeys(g, f2, pen, "F2", kbFont, Keys.F2)
                    DrawKeys(g, f3, pen, "F3", kbFont, Keys.F3)
                    DrawKeys(g, f4, pen, "F4", kbFont, Keys.F4)
                    DrawKeys(g, f5, pen, "F5", kbFont, Keys.F5)
                    DrawKeys(g, f6, pen, "F6", kbFont, Keys.F6)
                    DrawKeys(g, f7, pen, "F7", kbFont, Keys.F7)
                    DrawKeys(g, f8, pen, "F8", kbFont, Keys.F8)
                    DrawKeys(g, f9, pen, "F9", kbFont, Keys.F9)
                    DrawKeys(g, f10, pen, "F10", kbFont, Keys.F10)
                    DrawKeys(g, f11, pen, "F11", kbFont, Keys.F11)
                    DrawKeys(g, f12, pen, "F12", kbFont, Keys.F12)
                    DrawKeys(g, prnscn, pen, $"Print{vbNewLine}Screen", kbFont, Keys.PrintScreen, TextFormatFlags.HorizontalCenter)
                    DrawKeys(g, scroll, pen, $"Scroll{vbNewLine}Lock", kbFont, Keys.Scroll, TextFormatFlags.HorizontalCenter)
                    DrawKeys(g, pause, pen, $"Pause{vbNewLine}Break", kbFont, Keys.Pause, TextFormatFlags.HorizontalCenter)

                    DrawKeys(g, tilde, pen, $"~{vbNewLine}`", kbFont, Keys.Oemtilde)
                    DrawKeys(g, d1, pen, $"!{vbNewLine}1", kbFont, Keys.D1)
                    DrawKeys(g, d2, pen, $"@{vbNewLine}2", kbFont, Keys.D2)
                    DrawKeys(g, d3, pen, $"#{vbNewLine}3", kbFont, Keys.D3)
                    DrawKeys(g, d4, pen, $"${vbNewLine}4", kbFont, Keys.D4)
                    DrawKeys(g, d5, pen, $"%{vbNewLine}5", kbFont, Keys.D5)
                    DrawKeys(g, d6, pen, $"^{vbNewLine}6", kbFont, Keys.D6)
                    DrawKeys(g, d7, pen, $"&&{vbNewLine}7", kbFont, Keys.D7)
                    DrawKeys(g, d8, pen, $"*{vbNewLine}8", kbFont, Keys.D8)
                    DrawKeys(g, d9, pen, $"({vbNewLine}9", kbFont, Keys.D9)
                    DrawKeys(g, d0, pen, $"){vbNewLine}0", kbFont, Keys.D0)
                    DrawKeys(g, minus, pen, $"_{vbNewLine}-", kbFont, Keys.OemMinus)
                    DrawKeys(g, plus, pen, $"+{vbNewLine}=", kbFont, Keys.Oemplus)
                    DrawKeys(g, bspace, pen, "Backspace", kbFont, Keys.Back)
                    DrawKeys(g, ins, pen, "Insert", kbFont, Keys.Insert, TextFormatFlags.HorizontalCenter)
                    DrawKeys(g, home, pen, $"Home{vbNewLine}⏮", kbFont, Keys.Home, TextFormatFlags.HorizontalCenter)
                    DrawKeys(g, pgup, pen, $"Page{vbNewLine}Up{vbNewLine}▲", kbFont, Keys.PageUp, TextFormatFlags.HorizontalCenter)
                    DrawKeys(g, numlock, pen, $"Num{vbNewLine}Lock", kbFont, Keys.NumLock, TextFormatFlags.HorizontalCenter)
                    DrawKeys(g, numslash, pen, "/", kbFont, Keys.Divide)
                    DrawKeys(g, numstar, pen, "*", kbFont, Keys.Multiply)
                    DrawKeys(g, numminus, pen, "-", kbFont, Keys.Subtract)

                    DrawKeys(g, tab, pen, $"Tab{vbNewLine}⭾", kbFont, Keys.Tab)
                    DrawKeys(g, q, pen, "Q", kbFont, Keys.Q)
                    DrawKeys(g, w, pen, "W", kbFont, Keys.W)
                    DrawKeys(g, e, pen, "E", kbFont, Keys.E)
                    DrawKeys(g, r, pen, "R", kbFont, Keys.R)
                    DrawKeys(g, t, pen, "T", kbFont, Keys.T)
                    DrawKeys(g, yy, pen, "Y", kbFont, Keys.Y)
                    DrawKeys(g, u, pen, "U", kbFont, Keys.U)
                    DrawKeys(g, i, pen, "I", kbFont, Keys.I)
                    DrawKeys(g, o, pen, "O", kbFont, Keys.O)
                    DrawKeys(g, p, pen, "P", kbFont, Keys.P)
                    DrawKeys(g, lbracket, pen, "{" & vbNewLine & "[", kbFont, Keys.Oem4)
                    DrawKeys(g, rbracket, pen, "}" & vbNewLine & "]", kbFont, Keys.Oem6)
                    DrawKeys(g, bslash, pen, $"|{vbNewLine}\", kbFont, Keys.Oem5)
                    DrawKeys(g, del, pen, "Delete", kbFont, Keys.Delete, TextFormatFlags.HorizontalCenter)
                    DrawKeys(g, [end], pen, $"End{vbNewLine}⏭", kbFont, Keys.End, TextFormatFlags.HorizontalCenter)
                    DrawKeys(g, pgdown, pen, $"Page{vbNewLine}Down{vbNewLine}▼", kbFont, Keys.PageDown, TextFormatFlags.HorizontalCenter)
                    DrawKeys(g, num7, pen, $"7{vbNewLine}Home", kbFont, Keys.NumPad7)
                    DrawKeys(g, num8, pen, $"8{vbNewLine}▲", kbFont, Keys.NumPad8)
                    DrawKeys(g, num9, pen, $"9{vbNewLine}Pg Up", kbFont, Keys.NumPad9)
                    DrawKeys(g, numplus, pen, "+", kbFont, Keys.Add)

                    DrawKeys(g, capslock, pen, $"Caps Lock{vbNewLine}🄰", kbFont, Keys.CapsLock)
                    DrawKeys(g, a, pen, "A", kbFont, Keys.A)
                    DrawKeys(g, s, pen, "S", kbFont, Keys.S)
                    DrawKeys(g, d, pen, "D", kbFont, Keys.D)
                    DrawKeys(g, f, pen, "F", kbFont, Keys.F)
                    DrawKeys(g, gg, pen, "G", kbFont, Keys.G)
                    DrawKeys(g, h, pen, "H", kbFont, Keys.H)
                    DrawKeys(g, j, pen, "J", kbFont, Keys.J)
                    DrawKeys(g, k, pen, "K", kbFont, Keys.K)
                    DrawKeys(g, l, pen, "L", kbFont, Keys.L)
                    DrawKeys(g, semicolon, pen, $":{vbNewLine};", kbFont, Keys.OemSemicolon)
                    DrawKeys(g, quote, pen, $"""{vbNewLine}'", kbFont, Keys.OemQuotes)
                    DrawKeys(g, enter, pen, $"Enter{vbNewLine}⏎", kbFont, Keys.Enter)
                    DrawKeys(g, num4, pen, $"4{vbNewLine}◀", kbFont, Keys.NumPad4)
                    DrawKeys(g, num5, pen, "5", kbFont, Keys.NumPad5)
                    DrawKeys(g, num6, pen, $"6{vbNewLine}▶", kbFont, Keys.NumPad6)

                    DrawKeys(g, lshift, pen, "Shift", kbFont, Keys.ShiftKey)
                    DrawKeys(g, z, pen, "Z", kbFont, Keys.Z)
                    DrawKeys(g, xx, pen, "X", kbFont, Keys.X)
                    DrawKeys(g, c, pen, "C", kbFont, Keys.C)
                    DrawKeys(g, v, pen, "V", kbFont, Keys.V)
                    DrawKeys(g, b, pen, "B", kbFont, Keys.B)
                    DrawKeys(g, n, pen, "N", kbFont, Keys.N)
                    DrawKeys(g, m, pen, "M", kbFont, Keys.M)
                    DrawKeys(g, comma, pen, $"<{vbNewLine},", kbFont, Keys.Oemcomma)
                    DrawKeys(g, period, pen, $">{vbNewLine}.", kbFont, Keys.OemPeriod)
                    DrawKeys(g, question, pen, $"?{vbNewLine}/", kbFont, Keys.OemQuestion)
                    DrawKeys(g, rshift, pen, "Shift", kbFont, Keys.ShiftKey)
                    DrawKeys(g, up, pen, "▲", kbFont, Keys.Up)
                    DrawKeys(g, num1, pen, $"1{vbNewLine}End", kbFont, Keys.NumPad1)
                    DrawKeys(g, num2, pen, $"2{vbNewLine}▼", kbFont, Keys.NumPad2)
                    DrawKeys(g, num3, pen, $"3{vbNewLine}Pg Dn", kbFont, Keys.NumPad3)
                    DrawKeys(g, numenter, pen, $"Enter{vbNewLine}⏎", kbFont, Keys.Return)

                    DrawKeys(g, lctrl, pen, "Ctrl", kbFont, Keys.ControlKey)
                    DrawKeys(g, lwin, pen, "⊞", kbFont, Keys.LWin)
                    DrawKeys(g, lalt, pen, "Alt", kbFont, Keys.Menu)
                    DrawKeys(g, space, pen, "", kbFont, Keys.Space)
                    DrawKeys(g, ralt, pen, "Alt", kbFont, Keys.Menu)
                    DrawKeys(g, rwin, pen, "⊞", kbFont, Keys.RWin)
                    DrawKeys(g, menu, pen, "▤", kbFont, Keys.Apps)
                    DrawKeys(g, rctrl, pen, "Ctrl", kbFont, Keys.ControlKey)
                    DrawKeys(g, left, pen, "◀", kbFont, Keys.Left)
                    DrawKeys(g, down, pen, "▼", kbFont, Keys.Down)
                    DrawKeys(g, right, pen, "▶", kbFont, Keys.Right)
                    DrawKeys(g, num0, pen, $"0{vbNewLine}Ins", kbFont, Keys.NumPad0)
                    DrawKeys(g, numperiod, pen, $".{vbNewLine}Del", kbFont, Keys.Decimal)
                    ' DrawKeys(g, , pen, "", kbFont)   $"{vbNewLine}"
                End Using
            End Using
        End Using
    End Sub

    Private Sub DrawKeys(g As Graphics, rect As Rectangle, pen As Pen, text As String, font As Font, key As Keys, Optional tff As TextFormatFlags = TextFormatFlags.Left)
        If RGB Then
            Using lbrush As New LinearGradientBrush(keyboardRect, Color.Red, Color.Blue, rgbAngle)
                Dim cBlend As New ColorBlend(4)
                cBlend.Colors = New Color(3) {Color.Red, Color.Green, Color.Blue, Color.Black}
                cBlend.Positions = New Single(3) {0F, 0.333F, 0.666F, 1.0F}
                lbrush.InterpolationColors = cBlend

                Using newPen As New Pen(lbrush, 1.5)
                    g.DrawRoundedRectangle(keyboardRect, 10, newPen)
                    If pressedKeys.Contains(key) Then
                        g.FillRoundedRectangle(rect, 10, lbrush, New RoundedRectCorners(True))
                        Dim smaller As New Rectangle(rect.X + 3, rect.Y + 3, rect.Width - 6, rect.Height - 6)
                        g.DrawGDIText(text, font, smaller, BackColor, tff Or TextFormatFlags.Top)
                    Else
                        g.DrawRoundedRectangle(rect, 10, newPen)
                        Dim smaller As New Rectangle(rect.X + 3, rect.Y + 3, rect.Width - 6, rect.Height - 6)
                        'g.DrawGDIText(text, font, smaller, cBlend.Colors(rgbNum), tff Or TextFormatFlags.Top)
                        g.DrawString(text, font, lbrush, smaller.X, smaller.Y)
                    End If
                End Using
            End Using
        Else
            Using brush As New SolidBrush(KeyboardColor)
                If pressedKeys.Contains(key) Then
                    g.FillRoundedRectangle(rect, 10, brush, New RoundedRectCorners(True))
                    Dim smaller As New Rectangle(rect.X + 3, rect.Y + 3, rect.Width - 6, rect.Height - 6)
                    g.DrawGDIText(text, font, smaller, BackColor, tff Or TextFormatFlags.Top)
                Else
                    g.DrawRoundedRectangle(rect, 10, pen)
                    Dim smaller As New Rectangle(rect.X + 3, rect.Y + 3, rect.Width - 6, rect.Height - 6)
                    g.DrawGDIText(text, font, smaller, KeyboardColor, tff Or TextFormatFlags.Top)
                End If
            End Using
        End If
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

    Public Enum eEffect
    Circles
    Keyboard
    Both
End Enum