Imports System.Runtime.InteropServices
Imports System.Text

Public Class Media

    Public Const MM_MCINOTIFY As Integer = &H3B9

    Private _fileName As String
    Private isOpen As Boolean = False
    Private notifyForm As Form
    Private mediaName As String = "media"

    <DllImport("winmm.dll")>
    Private Shared Function mciSendString(ByVal command As String, ByVal returnValue As StringBuilder, ByVal returnLength As Integer, ByVal winHandle As IntPtr) As Long
    End Function

    Private Sub ClosePlayer()
        If isOpen Then
            Dim playCommand As String = $"Close {mediaName}"
            mciSendString(playCommand, Nothing, 0, IntPtr.Zero)
            isOpen = False
        End If
    End Sub

    Private Sub OpenMediaFile()
        ClosePlayer()
        Dim playCommand As String = "Open """ & _fileName & """ type mpegvideo alias " + mediaName
        mciSendString(playCommand, Nothing, 0, IntPtr.Zero)
        isOpen = True
    End Sub

    Private Sub PlayMediaFile()
        If isOpen Then
            Dim playCommand As String = $"Play {mediaName} notify"
            mciSendString(playCommand, Nothing, 0, notifyForm.Handle)
        End If
    End Sub

    Public Sub Play(ByVal fileName As String, ByVal notifyForm As Form)
        _fileName = fileName
        Me.notifyForm = notifyForm
        OpenMediaFile()
        PlayMediaFile()
    End Sub

    Public Sub [Stop]()
        ClosePlayer()
    End Sub

    Public Sub SetVolume(volume As Integer)
        Dim playCommand = $"setaudio {mediaName} volume to {(volume * 100).ToString()}"
        mciSendString(playCommand, Nothing, 0, IntPtr.Zero)
    End Sub

End Class
