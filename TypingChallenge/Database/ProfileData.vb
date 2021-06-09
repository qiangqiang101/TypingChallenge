Imports System.IO
Imports System.Xml.Serialization

Public Structure ProfileData

    Public ReadOnly Property Instance As ProfileData
        Get
            Return ReadFromFile()
        End Get
    End Property

    <XmlIgnore>
    Public Property FileName() As String

    Public Name As String
    Public Credits As Integer
    Public ClearedLevel As List(Of UserLevel)

    Public Sub New(_filename As String)
        FileName = _filename
    End Sub

    Public Sub Save()
        Dim ser = New XmlSerializer(GetType(ProfileData))
        Dim writer As TextWriter = New StreamWriter(FileName)
        ser.Serialize(writer, Me)
        writer.Close()
    End Sub

    Public Function ReadFromFile() As ProfileData
        If Not File.Exists(FileName) Then
            Return New ProfileData(FileName) With {.Name = "Player", .Credits = 0, .ClearedLevel = New List(Of UserLevel)}
        End If

        Try
            Dim ser = New XmlSerializer(GetType(ProfileData))
            Dim reader As TextReader = New StreamReader(FileName)
            Dim instance = CType(ser.Deserialize(reader), ProfileData)
            reader.Close()
            Return instance
        Catch ex As Exception
            Return New ProfileData(FileName) With {.Name = "Player", .Credits = 0, .ClearedLevel = New List(Of UserLevel)}
        End Try
    End Function

End Structure

Public Structure UserLevel

    Public Title As String
    Public Level As Integer
    Public Score As Integer

    Public Sub New(_title As String, lvl As Integer, _score As Integer)
        Title = _title
        Level = lvl
        Score = _score
    End Sub

End Structure
