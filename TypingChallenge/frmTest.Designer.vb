<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmTest
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.NewProfile1 = New TypingChallenge.NewProfile()
        Me.SuspendLayout()
        '
        'NewProfile1
        '
        Me.NewProfile1.BackColor = System.Drawing.Color.Black
        Me.NewProfile1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NewProfile1.Effect = TypingChallenge.eEffect.Both
        Me.NewProfile1.Font = New System.Drawing.Font("Segoe UI", 40.0!)
        Me.NewProfile1.KeyboardColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.NewProfile1.Location = New System.Drawing.Point(0, 0)
        Me.NewProfile1.Name = "NewProfile1"
        Me.NewProfile1.Player = "Player"
        Me.NewProfile1.RGB = True
        Me.NewProfile1.Size = New System.Drawing.Size(1344, 729)
        Me.NewProfile1.TabIndex = 0
        Me.NewProfile1.Text = "NewProfile1"
        '
        'frmTest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(15.0!, 37.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1344, 729)
        Me.Controls.Add(Me.NewProfile1)
        Me.Font = New System.Drawing.Font("Segoe UI", 20.0!)
        Me.Margin = New System.Windows.Forms.Padding(8, 9, 8, 9)
        Me.Name = "frmTest"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmTest"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents NewProfile1 As NewProfile
End Class
