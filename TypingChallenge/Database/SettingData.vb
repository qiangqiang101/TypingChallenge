Imports System.IO
Imports System.Xml.Serialization

Public Structure SettingData

    Public ReadOnly Property Instance As SettingData
        Get
            Return ReadFromFile()
        End Get
    End Property

    <XmlIgnore>
    Public Property FileName() As String

    Public MusicVolume As Integer
    Public MusicEnabled As Boolean
    Public SoundVolume As Integer
    Public Quality As Integer
    Public KeyboardColorA As Integer
    Public KeyboardColorR As Integer
    Public KeyboardColorG As Integer
    Public KeyboardColorB As Integer
    Public KeyboardRGB As Boolean
    Public FullScreen As Boolean
    Public ShowFPS As Boolean
    Public Difficulty As Integer
    Public Version As Integer

    Public Sub New(_filename As String)
        FileName = _filename
    End Sub

    Public Sub Save()
        Dim ser = New XmlSerializer(GetType(SettingData))
        Dim writer As TextWriter = New StreamWriter(FileName)
        ser.Serialize(writer, Me)
        writer.Close()
    End Sub

    Public Function ReadFromFile() As SettingData
        If Not File.Exists(FileName) Then
            Return New SettingData(FileName) With {.MusicVolume = 80, .MusicEnabled = True, .SoundVolume = 80, .Quality = 2, .FullScreen = True, .ShowFPS = True,
                .KeyboardColorA = 255, .KeyboardColorR = 255, .KeyboardColorG = 255, .KeyboardColorB = 255, .KeyboardRGB = True, .Difficulty = 0, .Version = 0}
        End If

        Try
            Dim ser = New XmlSerializer(GetType(SettingData))
            Dim reader As TextReader = New StreamReader(FileName)
            Dim instance = CType(ser.Deserialize(reader), SettingData)
            reader.Close()
            Return instance
        Catch ex As Exception
            Return New SettingData(FileName) With {.MusicVolume = 80, .MusicEnabled = True, .SoundVolume = 80, .Quality = 2, .FullScreen = True, .ShowFPS = True,
                .KeyboardColorA = 255, .KeyboardColorR = 255, .KeyboardColorG = 255, .KeyboardColorB = 255, .KeyboardRGB = True, .Difficulty = 0, .Version = 0}
        End Try
    End Function

End Structure
