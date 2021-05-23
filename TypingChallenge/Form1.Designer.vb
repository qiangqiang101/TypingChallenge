<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.MyGame1 = New TypingChallenge.MyGame()
        Me.SuspendLayout()
        '
        'MyGame1
        '
        Me.MyGame1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MyGame1.Font = New System.Drawing.Font("Segoe UI", 80.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MyGame1.Level = 1
        Me.MyGame1.Life = 5
        Me.MyGame1.Location = New System.Drawing.Point(0, 0)
        Me.MyGame1.Name = "MyGame1"
        Me.MyGame1.Phrase = Nothing
        Me.MyGame1.Size = New System.Drawing.Size(1344, 729)
        Me.MyGame1.TabIndex = 0
        Me.MyGame1.Text = "MyGame1"
        Me.MyGame1.TimeLimit = 300
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(1344, 729)
        Me.Controls.Add(Me.MyGame1)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents MyGame1 As MyGame
End Class
