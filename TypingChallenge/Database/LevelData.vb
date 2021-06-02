Imports System.IO
Imports System.Xml.Serialization

Public Structure LevelData

    Public ReadOnly Property Instance As LevelData
        Get
            Return ReadFromFile()
        End Get
    End Property

    <XmlIgnore>
    Public Property FileName() As String

    Public LevelList As List(Of Level)
    Public Version As Integer

    Public Sub New(_filename As String)
        FileName = _filename
    End Sub

    Public Sub Save()
        Dim ser = New XmlSerializer(GetType(LevelData))
        Dim writer As TextWriter = New StreamWriter(FileName)
        ser.Serialize(writer, Me)
        writer.Close()
    End Sub

    Public Function ReadFromFile() As LevelData
        If Not File.Exists(FileName) Then
            Return New LevelData(FileName) With {.LevelList = New List(Of Level), .Version = 0}
        End If

        Try
            Dim ser = New XmlSerializer(GetType(LevelData))
            Dim reader As TextReader = New StreamReader(FileName)
            Dim instance = CType(ser.Deserialize(reader), LevelData)
            reader.Close()
            Return instance
        Catch ex As Exception
            Return New LevelData(FileName) With {.LevelList = New List(Of Level), .Version = 0}
        End Try
    End Function

End Structure

Public Structure Level

    Public Title As String
    Public Author As String
    Public Level As Integer
    Public Phrase As String
    Public Life As Integer
    Public TimeLimit As Integer
    Public Image As String
    Public Page As Integer

    Public Sub New(_title As String, _phrase As String, img As String, _page As Integer, Optional _author As String = "Zettabyte Technology", Optional lvl As Integer = 1, Optional _life As Integer = 5, Optional time As Integer = 60)
        Title = _title
        Author = _author
        Level = lvl
        Phrase = _phrase
        Image = img
        Page = _page
        Life = _life
        TimeLimit = time
    End Sub

End Structure