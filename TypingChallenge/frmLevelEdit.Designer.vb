<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLevelEdit
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.AddToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.gbPreview = New System.Windows.Forms.GroupBox()
        Me.pbImage = New System.Windows.Forms.PictureBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtImageURL = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.nudPage = New System.Windows.Forms.NumericUpDown()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.tsslChars = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslWordCount = New System.Windows.Forms.ToolStripStatusLabel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtPhrase = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.nudLives = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.nudTime = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtAuthor = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.nudLevel = New System.Windows.Forms.NumericUpDown()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.lvLevels = New TypingChallenge.ListViewX()
        Me.chLevel = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chPage = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chTitle = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chAuthor = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chTimeLimit = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chLife = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chCharCount = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chWordCount = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.gbPreview.SuspendLayout()
        CType(Me.pbImage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.nudLives, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudLevel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lvLevels)
        Me.SplitContainer1.Panel1.Controls.Add(Me.MenuStrip1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.gbPreview)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label8)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtImageURL)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label7)
        Me.SplitContainer1.Panel2.Controls.Add(Me.nudPage)
        Me.SplitContainer1.Panel2.Controls.Add(Me.StatusStrip1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnCancel)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnSave)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label6)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtPhrase)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label5)
        Me.SplitContainer1.Panel2.Controls.Add(Me.nudLives)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label4)
        Me.SplitContainer1.Panel2.Controls.Add(Me.nudTime)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label3)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtAuthor)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.nudLevel)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtTitle)
        Me.SplitContainer1.Panel2.Padding = New System.Windows.Forms.Padding(3)
        Me.SplitContainer1.Size = New System.Drawing.Size(1344, 729)
        Me.SplitContainer1.SplitterDistance = 678
        Me.SplitContainer1.TabIndex = 0
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddToolStripMenuItem, Me.EditToolStripMenuItem, Me.DeleteToolStripMenuItem1, Me.SaveToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(678, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'AddToolStripMenuItem
        '
        Me.AddToolStripMenuItem.Name = "AddToolStripMenuItem"
        Me.AddToolStripMenuItem.Size = New System.Drawing.Size(41, 20)
        Me.AddToolStripMenuItem.Text = "Add"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.EditToolStripMenuItem.Text = "Edit"
        '
        'DeleteToolStripMenuItem1
        '
        Me.DeleteToolStripMenuItem1.Name = "DeleteToolStripMenuItem1"
        Me.DeleteToolStripMenuItem1.Size = New System.Drawing.Size(52, 20)
        Me.DeleteToolStripMenuItem1.Text = "Delete"
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(43, 20)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'gbPreview
        '
        Me.gbPreview.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPreview.Controls.Add(Me.pbImage)
        Me.gbPreview.Location = New System.Drawing.Point(413, 12)
        Me.gbPreview.Name = "gbPreview"
        Me.gbPreview.Size = New System.Drawing.Size(237, 168)
        Me.gbPreview.TabIndex = 17
        Me.gbPreview.TabStop = False
        Me.gbPreview.Text = "Image Preview"
        '
        'pbImage
        '
        Me.pbImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.pbImage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbImage.Location = New System.Drawing.Point(3, 19)
        Me.pbImage.Name = "pbImage"
        Me.pbImage.Size = New System.Drawing.Size(231, 146)
        Me.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbImage.TabIndex = 0
        Me.pbImage.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(8, 189)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(64, 15)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Image URL"
        '
        'txtImageURL
        '
        Me.txtImageURL.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtImageURL.Location = New System.Drawing.Point(130, 186)
        Me.txtImageURL.Name = "txtImageURL"
        Me.txtImageURL.Size = New System.Drawing.Size(520, 23)
        Me.txtImageURL.TabIndex = 6
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 159)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(33, 15)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Page"
        '
        'nudPage
        '
        Me.nudPage.Location = New System.Drawing.Point(130, 157)
        Me.nudPage.Maximum = New Decimal(New Integer() {99999, 0, 0, 0})
        Me.nudPage.Name = "nudPage"
        Me.nudPage.Size = New System.Drawing.Size(122, 23)
        Me.nudPage.TabIndex = 5
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslChars, Me.tsslWordCount})
        Me.StatusStrip1.Location = New System.Drawing.Point(3, 702)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(656, 24)
        Me.StatusStrip1.TabIndex = 12
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'tsslChars
        '
        Me.tsslChars.Name = "tsslChars"
        Me.tsslChars.Size = New System.Drawing.Size(106, 19)
        Me.tsslChars.Text = "Character Count: 0"
        '
        'tsslWordCount
        '
        Me.tsslWordCount.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left
        Me.tsslWordCount.Name = "tsslWordCount"
        Me.tsslWordCount.Size = New System.Drawing.Size(88, 19)
        Me.tsslWordCount.Text = "Word Count: 0"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(575, 678)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(494, 678)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 8
        Me.btnSave.Text = "Add"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 218)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(42, 15)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Phrase"
        '
        'txtPhrase
        '
        Me.txtPhrase.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPhrase.Location = New System.Drawing.Point(130, 215)
        Me.txtPhrase.MaxLength = 65535
        Me.txtPhrase.Multiline = True
        Me.txtPhrase.Name = "txtPhrase"
        Me.txtPhrase.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtPhrase.Size = New System.Drawing.Size(520, 457)
        Me.txtPhrase.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 130)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(33, 15)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Lives"
        '
        'nudLives
        '
        Me.nudLives.Location = New System.Drawing.Point(130, 128)
        Me.nudLives.Name = "nudLives"
        Me.nudLives.Size = New System.Drawing.Size(122, 23)
        Me.nudLives.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 101)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(118, 15)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Time Limit (Seconds)"
        '
        'nudTime
        '
        Me.nudTime.Location = New System.Drawing.Point(130, 99)
        Me.nudTime.Maximum = New Decimal(New Integer() {3600, 0, 0, 0})
        Me.nudTime.Name = "nudTime"
        Me.nudTime.Size = New System.Drawing.Size(122, 23)
        Me.nudTime.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 44)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(44, 15)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Author"
        '
        'txtAuthor
        '
        Me.txtAuthor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAuthor.Location = New System.Drawing.Point(130, 41)
        Me.txtAuthor.Name = "txtAuthor"
        Me.txtAuthor.Size = New System.Drawing.Size(277, 23)
        Me.txtAuthor.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 15)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Title"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 72)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 15)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Level"
        '
        'nudLevel
        '
        Me.nudLevel.Location = New System.Drawing.Point(130, 70)
        Me.nudLevel.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.nudLevel.Name = "nudLevel"
        Me.nudLevel.Size = New System.Drawing.Size(122, 23)
        Me.nudLevel.TabIndex = 0
        Me.nudLevel.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'txtTitle
        '
        Me.txtTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTitle.Location = New System.Drawing.Point(130, 12)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(277, 23)
        Me.txtTitle.TabIndex = 1
        '
        'lvLevels
        '
        Me.lvLevels.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chLevel, Me.chPage, Me.chTitle, Me.chAuthor, Me.chTimeLimit, Me.chLife, Me.chCharCount, Me.chWordCount})
        Me.lvLevels.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvLevels.FullRowSelect = True
        Me.lvLevels.GridLines = True
        Me.lvLevels.Location = New System.Drawing.Point(0, 24)
        Me.lvLevels.MultiSelect = False
        Me.lvLevels.Name = "lvLevels"
        Me.lvLevels.Size = New System.Drawing.Size(678, 705)
        Me.lvLevels.SortColumn = 0
        Me.lvLevels.Sorting = System.Windows.Forms.SortOrder.Descending
        Me.lvLevels.TabIndex = 0
        Me.lvLevels.UseCompatibleStateImageBehavior = False
        Me.lvLevels.View = System.Windows.Forms.View.Details
        '
        'chLevel
        '
        Me.chLevel.Text = "Level"
        Me.chLevel.Width = 40
        '
        'chPage
        '
        Me.chPage.Text = "Page"
        Me.chPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.chPage.Width = 40
        '
        'chTitle
        '
        Me.chTitle.Text = "Title"
        Me.chTitle.Width = 200
        '
        'chAuthor
        '
        Me.chAuthor.Text = "Author"
        Me.chAuthor.Width = 100
        '
        'chTimeLimit
        '
        Me.chTimeLimit.Text = "Time Limit"
        Me.chTimeLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.chTimeLimit.Width = 70
        '
        'chLife
        '
        Me.chLife.Text = "Life"
        Me.chLife.Width = 40
        '
        'chCharCount
        '
        Me.chCharCount.Text = "Char Count"
        Me.chCharCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.chCharCount.Width = 80
        '
        'chWordCount
        '
        Me.chWordCount.Text = "Word Count"
        Me.chWordCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.chWordCount.Width = 80
        '
        'frmLevelEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1344, 729)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmLevelEdit"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Level Editor"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.gbPreview.ResumeLayout(False)
        CType(Me.pbImage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.nudLives, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudLevel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents lvLevels As ListViewX
    Friend WithEvents chLevel As ColumnHeader
    Friend WithEvents chTitle As ColumnHeader
    Friend WithEvents chAuthor As ColumnHeader
    Friend WithEvents chTimeLimit As ColumnHeader
    Friend WithEvents chLife As ColumnHeader
    Friend WithEvents btnSave As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents txtPhrase As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents nudLives As NumericUpDown
    Friend WithEvents Label4 As Label
    Friend WithEvents nudTime As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents txtAuthor As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents nudLevel As NumericUpDown
    Friend WithEvents txtTitle As TextBox
    Friend WithEvents btnCancel As Button
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents AddToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents tsslChars As ToolStripStatusLabel
    Friend WithEvents tsslWordCount As ToolStripStatusLabel
    Friend WithEvents chCharCount As ColumnHeader
    Friend WithEvents chWordCount As ColumnHeader
    Friend WithEvents Label8 As Label
    Friend WithEvents txtImageURL As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents nudPage As NumericUpDown
    Friend WithEvents gbPreview As GroupBox
    Friend WithEvents pbImage As PictureBox
    Friend WithEvents chPage As ColumnHeader
End Class
