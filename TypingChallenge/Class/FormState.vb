Imports System.Runtime.InteropServices

Public Class WinApi

    <DllImport("user32.dll", EntryPoint:="GetSystemMetrics")>
    Public Shared Function GetSystemMetrics(ByVal which As Integer) As Integer
    End Function

    <DllImport("user32.dll")>
    Public Shared Sub SetWindowPos(ByVal hwnd As IntPtr, ByVal hwndInsertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal flags As UInteger)
    End Sub

    Private Const SM_CXSCREEN As Integer = 0
    Private Const SM_CYSCREEN As Integer = 1
    Private Shared HWND_TOP As IntPtr = IntPtr.Zero
    Private Const SWP_SHOWWINDOW As Integer = 64

    Public Shared ReadOnly Property ScreenX As Integer
        Get
            Return GetSystemMetrics(SM_CXSCREEN)
        End Get
    End Property

    Public Shared ReadOnly Property ScreenY As Integer
        Get
            Return GetSystemMetrics(SM_CYSCREEN)
        End Get
    End Property

    Public Shared Sub SetWinFullScreen(ByVal hwnd As IntPtr)
        SetWindowPos(hwnd, HWND_TOP, 0, 0, ScreenX, ScreenY, SWP_SHOWWINDOW)
    End Sub

End Class

Public Class FormState

    Private winState As FormWindowState
    Private brdStyle As FormBorderStyle
    Private topMost As Boolean
    Private bounds As Rectangle
    Private IsMaximized As Boolean = False

    Public Sub Maximize(ByVal targetForm As Form)
        If Not IsMaximized Then
            IsMaximized = True
            Save(targetForm)
            targetForm.WindowState = FormWindowState.Maximized
            targetForm.FormBorderStyle = FormBorderStyle.None
            targetForm.TopMost = True
            WinApi.SetWinFullScreen(targetForm.Handle)
        End If
    End Sub

    Public Sub Save(ByVal targetForm As Form)
        winState = targetForm.WindowState
        brdStyle = targetForm.FormBorderStyle
        topMost = targetForm.TopMost
        bounds = targetForm.Bounds
    End Sub

    Public Sub Restore(ByVal targetForm As Form)
        targetForm.WindowState = winState
        targetForm.FormBorderStyle = brdStyle
        targetForm.TopMost = topMost
        targetForm.Bounds = bounds
        IsMaximized = False
    End Sub

End Class
