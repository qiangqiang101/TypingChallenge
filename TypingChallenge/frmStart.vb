Public Class frmStart
    Private Sub frmStart_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim cmdline As String = Environment.CommandLine()
        Dim iPos = cmdline.IndexOf("""", 2)
        Dim sCmdLineArgs = cmdline.Substring(iPos + 1).Trim()

        If sCmdLineArgs = "-dev" Then
            frmLevelEdit.Show()
        Else
            frmGame.Show()
        End If

        Close()
    End Sub
End Class