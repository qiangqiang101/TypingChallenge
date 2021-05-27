<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTest
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
        Me.components = New System.ComponentModel.Container()
        Me.GameOption1 = New TypingChallenge.GameOption()
        Me.SuspendLayout()
        '
        'GameOption1
        '
        Me.GameOption1.BackColor = System.Drawing.Color.Black
        Me.GameOption1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GameOption1.Font = New System.Drawing.Font("Segoe UI", 40.0!)
        Me.GameOption1.FullScreen = False
        Me.GameOption1.GraphicsQuality = 0
        Me.GameOption1.Location = New System.Drawing.Point(0, 0)
        Me.GameOption1.MusicVolume = 0
        Me.GameOption1.Name = "GameOption1"
        Me.GameOption1.ShowFPS = False
        Me.GameOption1.Size = New System.Drawing.Size(1344, 729)
        Me.GameOption1.SoundVolume = 0
        Me.GameOption1.TabIndex = 0
        Me.GameOption1.Text = "GameOption1"
        '
        'frmTest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(15.0!, 37.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1344, 729)
        Me.Controls.Add(Me.GameOption1)
        Me.Font = New System.Drawing.Font("Segoe UI", 20.0!)
        Me.Margin = New System.Windows.Forms.Padding(8, 9, 8, 9)
        Me.Name = "frmTest"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmTest"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GameOption1 As GameOption
End Class
