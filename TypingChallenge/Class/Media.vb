Imports System.Runtime.InteropServices
Imports System.Text

Public Class Media

    Public Const MM_MCINOTIFY As Integer = &H3B9
    Const WS_CHILD As Integer = &H40000000

    Private _fileName As String
    Private isOpen As Boolean = False
    Private notifyForm As Form
    Private _mediaName As String = "media"

    <DllImport("winmm.dll")>
    Private Shared Function mciSendString(ByVal command As String, ByVal returnValue As StringBuilder, ByVal returnLength As Integer, ByVal winHandle As IntPtr) As Long
    End Function

    Public Property MediaName() As String
        Get
            Return _mediaName
        End Get
        Set(value As String)
            _mediaName = value
        End Set
    End Property


    Private Sub ClosePlayer()
        If isOpen Then
            Dim playCommand As String = $"Close {_mediaName}"
            mciSendString(playCommand, Nothing, 0, IntPtr.Zero)
            isOpen = False
        End If
    End Sub

    Private Sub OpenMediaFile()
        ClosePlayer()
        Dim playCommand As String = "Open """ & _fileName & """ type mpegvideo alias " + _mediaName
        mciSendString(playCommand, Nothing, 0, IntPtr.Zero)
        isOpen = True
    End Sub

    Private Sub OpenVideoMediaFile(ByVal target As Control)
        ClosePlayer()
        Dim playCommand As String = "Open """ & _fileName & """ type mpegvideo alias " & _mediaName & " parent " & CStr(target.Handle.ToInt32) & " style child"
        mciSendString(playCommand, Nothing, 0, IntPtr.Zero)
        isOpen = True
    End Sub

    Private Sub PlayVideoMediaFile()
        If isOpen Then
            Dim playCommand As String = $"Play {_mediaName} repeat"
            mciSendString(playCommand, Nothing, 0, IntPtr.Zero)
        End If
    End Sub

    Private Sub PlayMediaFile()
        If isOpen Then
            Dim playCommand As String = $"Play {_mediaName} notify"
            mciSendString(playCommand, Nothing, 0, notifyForm.Handle)
        End If
    End Sub

    Public Sub Play(ByVal fileName As String, ByVal notifyForm As Form)
        _fileName = fileName
        Me.notifyForm = notifyForm
        OpenMediaFile()
        PlayMediaFile()
    End Sub

    Public Sub PlayVideo(ByVal fileName As String, ByVal target As Control)
        _fileName = fileName
        OpenVideoMediaFile(target)
        PlayVideoMediaFile()
    End Sub

    Public Sub [Stop]()
        ClosePlayer()
    End Sub

    Public Sub SetVolume(volume As Integer)
        Dim playCommand = $"setaudio {_mediaName} volume to {(volume * 100).ToString()}"
        mciSendString(playCommand, Nothing, 0, IntPtr.Zero)
    End Sub

End Class
