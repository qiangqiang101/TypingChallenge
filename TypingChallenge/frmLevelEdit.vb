Public Class frmLevelEdit

    Public Mode As eMode = eMode.Add
    Public CurrentLevel As Level = Nothing
    Public LastEditingItem As ListViewItem = Nothing

    Public autoLevel As Integer = 0
    Public autoPage As Integer = 0
    Public autoTime As Integer = 0
    Public autoLives As Integer = 0

    Private Sub frmLevelEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        levels = New LevelData(lvlXmlPath).Instance

        For Each lvl In levels.LevelList
            Dim item As New ListViewItem({lvl.Level, lvl.Page, lvl.Title, lvl.Author, lvl.TimeLimit, lvl.Life, lvl.Phrase.Length, lvl.Phrase.WordCount}) With {.Tag = lvl}
            lvLevels.Items.Add(item)
        Next
        lvLevels.Load()
        lvLevels.Striped

        NewItem()
    End Sub

    Private Sub lvLevels_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lvLevels.MouseDoubleClick
        EditItem()
    End Sub

    Private Sub EditItem()
        If lvLevels.SelectedItems.Count <> 0 Then
            cbAutoLevel.Enabled = False
            cbAutoLives.Enabled = False
            cbAutoPage.Enabled = False
            cbAutoTime.Enabled = False

            CurrentLevel = lvLevels.SelectedItems.Item(0).Tag
            LastEditingItem = lvLevels.SelectedItems.Item(0)
            Mode = eMode.Edit

            nudLevel.Value = CurrentLevel.Level
            txtTitle.Text = CurrentLevel.Title
            txtAuthor.Text = CurrentLevel.Author
            nudTime.Value = CurrentLevel.TimeLimit
            nudLives.Value = CurrentLevel.Life
            txtPhrase.Text = CurrentLevel.Phrase.Replace("\n\n", vbNewLine)
            nudPage.Value = CurrentLevel.Page

            btnSave.Text = "Edit"
        End If
    End Sub

    Private Sub NewItem()
        cbAutoLevel.Enabled = True
        cbAutoLives.Enabled = True
        cbAutoPage.Enabled = True
        cbAutoTime.Enabled = True

        CurrentLevel = New Level()
        Mode = eMode.Add

        If cbAutoLevel.Checked AndAlso cbAutoLevel.Enabled Then nudLevel.Value = lvLevels.Items.Count + 1 Else nudLevel.Value = 0
        txtTitle.Clear()
        txtAuthor.Clear()
        nudTime.Value = 300
        nudLives.Value = 5
        If cbAutoPage.Checked AndAlso cbAutoPage.Enabled Then nudPage.Value = CInt(nudLevel.Value).GetPagesFromNum Else nudPage.Value = 1
        txtPhrase.Clear()

        btnSave.Text = "Add"
    End Sub

    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click
        EditItem()
    End Sub

    Private Sub AddToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddToolStripMenuItem.Click
        NewItem()
    End Sub

    Private Sub DeleteToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem1.Click
        If lvLevels.SelectedItems.Count <> 0 Then
            Dim delLevel As Level = lvLevels.SelectedItems.Item(0).Tag
            Dim result As DialogResult = MessageBox.Show($"Are you sure you want to delete {delLevel.Title} by {delLevel.Author}?", "Delete", MessageBoxButtons.YesNo)
            If result = DialogResult.Yes Then lvLevels.Items.Remove(lvLevels.SelectedItems.Item(0))
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        Dim gotError As Boolean = False
        Try
            Dim newLevel As New LevelData(lvlXmlPath)
            With newLevel
                Dim newlist As New List(Of Level)
                For Each item As ListViewItem In lvLevels.Items
                    Dim lvl As Level = CType(item.Tag, Level)
                    newlist.Add(lvl)
                Next
                .LevelList = newlist
                .Version = levels.Version
            End With
            newLevel.Save()
        Catch ex As Exception
            gotError = True
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        Finally
            If Not gotError Then MsgBox("Level file saved successfully.", MsgBoxStyle.Information, "Level Editor")
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Select Case Mode
            Case eMode.Add
                Dim phr As String = txtPhrase.Text.Replace(vbCr, "\n").Replace(vbLf, "\n")
                CurrentLevel = New Level(txtTitle.Text, phr, nudPage.Value, txtAuthor.Text, nudLevel.Value, nudLives.Value, nudTime.Value)
                Dim item As New ListViewItem({nudLevel.Value, nudPage.Value, txtTitle.Text, txtAuthor.Text, nudTime.Value, nudLives.Value, txtPhrase.Text.Length, txtPhrase.Text.WordCount}) With {.Tag = CurrentLevel}
                lvLevels.Items.Add(item)
            Case eMode.Edit
                Dim phr As String = txtPhrase.Text.Replace(vbCr, "\n").Replace(vbLf, "\n")
                CurrentLevel = New Level(txtTitle.Text, phr, nudPage.Value, txtAuthor.Text, nudLevel.Value, nudLives.Value, nudTime.Value)
                With LastEditingItem
                    .SubItems(0).Text = nudLevel.Value
                    .SubItems(1).Text = nudPage.Value
                    .SubItems(2).Text = txtTitle.Text
                    .SubItems(3).Text = txtAuthor.Text
                    .SubItems(4).Text = nudTime.Value
                    .SubItems(5).Text = nudLives.Value
                    .Tag = CurrentLevel
                End With
        End Select

        NewItem()
    End Sub

    Private Sub txtPhrase_TextChanged(sender As Object, e As EventArgs) Handles txtPhrase.TextChanged
        If cbAutoTime.Checked AndAlso cbAutoTime.Enabled Then nudTime.Value = Math.Ceiling(txtPhrase.Text.WordCount / 40) * 60
        If cbAutoLives.Checked AndAlso cbAutoLives.Enabled Then nudLives.Value = Math.Ceiling(txtPhrase.Text.Length / 600)
        tsslChars.Text = $"Character Count: {txtPhrase.Text.Length}"
        tsslWordCount.Text = $"Word Count: {txtPhrase.Text.WordCount}"
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        NewItem()
    End Sub

    Private Sub RearrangeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RearrangeToolStripMenuItem.Click
        Dim list As New List(Of ListViewItem)
        For Each item As ListViewItem In lvLevels.Items
            list.Add(item)
        Next

        Dim newLvl As Integer = 1
        For Each item As ListViewItem In list.OrderBy(Function(x) CInt(x.SubItems(7).Text)).ThenBy(Function(x) CInt(x.SubItems(6).Text))
            Dim level As Level = CType(item.Tag, Level)
            Dim newLevel As New Level(level.Title, level.Phrase, newLvl.GetPagesFromNum, level.Author, newLvl, level.Life, level.TimeLimit)

            Dim newItem As New ListViewItem({newLvl, newLvl.GetPagesFromNum, level.Title, level.Author, level.TimeLimit, level.Life, level.Phrase.Length, level.Phrase.WordCount}) With {.Tag = newLevel}
            lvLevels.Items.Add(newItem)

            newLvl += 1
        Next

        For Each item As ListViewItem In list
            lvLevels.Items.Remove(item)
        Next

        list.Clear()
    End Sub
End Class

Public Enum eMode
    Add
    Edit
End Enum