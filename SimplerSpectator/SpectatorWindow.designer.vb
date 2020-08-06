<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SpectatorWindow
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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SpectatorWindow))
      Me.CommandTextBox = New System.Windows.Forms.TextBox()
      Me.CommandOnlyCheckbox = New System.Windows.Forms.CheckBox()
      Me.LoLFolderTextBox = New System.Windows.Forms.TextBox()
      Me.FolderLabel = New System.Windows.Forms.Label()
      Me.VersionLabel = New System.Windows.Forms.Label()
      Me.VersionTextBox = New System.Windows.Forms.TextBox()
      Me.NamesComboBox = New System.Windows.Forms.ComboBox()
      Me.SpectateUserButton = New System.Windows.Forms.Button()
      Me.CheckVersionButton = New System.Windows.Forms.Button()
      Me.SaveSettingsButton = New System.Windows.Forms.Button()
      Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
      Me.LineShape1 = New Microsoft.VisualBasic.PowerPacks.LineShape()
      Me.SummonerNameLabel = New System.Windows.Forms.Label()
      Me.StatusLabel = New System.Windows.Forms.Label()
      Me.BlitzGGButton = New System.Windows.Forms.Button()
      Me.OpGGButton = New System.Windows.Forms.Button()
      Me.OpGGCheckBox = New System.Windows.Forms.CheckBox()
      Me.BlitzGGCheckBox = New System.Windows.Forms.CheckBox()
      Me.IDLabel = New System.Windows.Forms.TextBox()
      Me.AddCheckBox = New System.Windows.Forms.CheckBox()
      Me.BlitzGGProfileButton = New System.Windows.Forms.Button()
      Me.NoRepeatBackgroundWorker = New System.ComponentModel.BackgroundWorker()
      Me.MyToolTip = New System.Windows.Forms.ToolTip(Me.components)
      Me.SuspendLayout()
      '
      'CommandTextBox
      '
      Me.CommandTextBox.Location = New System.Drawing.Point(521, 22)
      Me.CommandTextBox.Name = "CommandTextBox"
      Me.CommandTextBox.ReadOnly = True
      Me.CommandTextBox.Size = New System.Drawing.Size(105, 22)
      Me.CommandTextBox.TabIndex = 10
      Me.CommandTextBox.Visible = False
      '
      'CommandOnlyCheckbox
      '
      Me.CommandOnlyCheckbox.AutoSize = True
      Me.CommandOnlyCheckbox.Location = New System.Drawing.Point(430, 24)
      Me.CommandOnlyCheckbox.Name = "CommandOnlyCheckbox"
      Me.CommandOnlyCheckbox.Size = New System.Drawing.Size(91, 21)
      Me.CommandOnlyCheckbox.TabIndex = 11
      Me.CommandOnlyCheckbox.Text = "Cmd Only"
      Me.CommandOnlyCheckbox.UseVisualStyleBackColor = True
      Me.CommandOnlyCheckbox.Visible = False
      '
      'LoLFolderTextBox
      '
      Me.LoLFolderTextBox.Location = New System.Drawing.Point(78, 10)
      Me.LoLFolderTextBox.Name = "LoLFolderTextBox"
      Me.LoLFolderTextBox.Size = New System.Drawing.Size(83, 22)
      Me.LoLFolderTextBox.TabIndex = 6
      Me.LoLFolderTextBox.TabStop = False
      Me.LoLFolderTextBox.Text = "Error?"
      '
      'FolderLabel
      '
      Me.FolderLabel.AutoSize = True
      Me.FolderLabel.Location = New System.Drawing.Point(12, 13)
      Me.FolderLabel.Name = "FolderLabel"
      Me.FolderLabel.Size = New System.Drawing.Size(52, 17)
      Me.FolderLabel.TabIndex = 13
      Me.FolderLabel.Text = "Folder:"
      '
      'VersionLabel
      '
      Me.VersionLabel.AutoSize = True
      Me.VersionLabel.Location = New System.Drawing.Point(12, 42)
      Me.VersionLabel.Name = "VersionLabel"
      Me.VersionLabel.Size = New System.Drawing.Size(60, 17)
      Me.VersionLabel.TabIndex = 15
      Me.VersionLabel.Text = "Version:"
      '
      'VersionTextBox
      '
      Me.VersionTextBox.Enabled = False
      Me.VersionTextBox.Location = New System.Drawing.Point(78, 39)
      Me.VersionTextBox.Name = "VersionTextBox"
      Me.VersionTextBox.Size = New System.Drawing.Size(83, 22)
      Me.VersionTextBox.TabIndex = 8
      Me.VersionTextBox.TabStop = False
      Me.VersionTextBox.Text = "Unused"
      '
      'NamesComboBox
      '
      Me.NamesComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
      Me.NamesComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
      Me.NamesComboBox.FormattingEnabled = True
      Me.NamesComboBox.Location = New System.Drawing.Point(143, 82)
      Me.NamesComboBox.Name = "NamesComboBox"
      Me.NamesComboBox.Size = New System.Drawing.Size(143, 24)
      Me.NamesComboBox.TabIndex = 0
      '
      'SpectateUserButton
      '
      Me.SpectateUserButton.Location = New System.Drawing.Point(15, 112)
      Me.SpectateUserButton.Name = "SpectateUserButton"
      Me.SpectateUserButton.Size = New System.Drawing.Size(271, 23)
      Me.SpectateUserButton.TabIndex = 1
      Me.SpectateUserButton.Text = "Go"
      Me.SpectateUserButton.UseVisualStyleBackColor = True
      '
      'CheckVersionButton
      '
      Me.CheckVersionButton.Location = New System.Drawing.Point(167, 39)
      Me.CheckVersionButton.Name = "CheckVersionButton"
      Me.CheckVersionButton.Size = New System.Drawing.Size(119, 22)
      Me.CheckVersionButton.TabIndex = 7
      Me.CheckVersionButton.TabStop = False
      Me.CheckVersionButton.Text = "Check Name"
      Me.CheckVersionButton.UseVisualStyleBackColor = True
      '
      'SaveSettingsButton
      '
      Me.SaveSettingsButton.Location = New System.Drawing.Point(167, 10)
      Me.SaveSettingsButton.Name = "SaveSettingsButton"
      Me.SaveSettingsButton.Size = New System.Drawing.Size(119, 22)
      Me.SaveSettingsButton.TabIndex = 9
      Me.SaveSettingsButton.TabStop = False
      Me.SaveSettingsButton.Text = "Save Settings"
      Me.SaveSettingsButton.UseVisualStyleBackColor = True
      '
      'ShapeContainer1
      '
      Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
      Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
      Me.ShapeContainer1.Name = "ShapeContainer1"
      Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.LineShape1})
      Me.ShapeContainer1.Size = New System.Drawing.Size(301, 195)
      Me.ShapeContainer1.TabIndex = 22
      Me.ShapeContainer1.TabStop = False
      '
      'LineShape1
      '
      Me.LineShape1.Name = "LineShape1"
      Me.LineShape1.X1 = 12
      Me.LineShape1.X2 = 289
      Me.LineShape1.Y1 = 72
      Me.LineShape1.Y2 = 72
      '
      'SummonerNameLabel
      '
      Me.SummonerNameLabel.AutoSize = True
      Me.SummonerNameLabel.Location = New System.Drawing.Point(12, 85)
      Me.SummonerNameLabel.Name = "SummonerNameLabel"
      Me.SummonerNameLabel.Size = New System.Drawing.Size(125, 17)
      Me.SummonerNameLabel.TabIndex = 23
      Me.SummonerNameLabel.Text = "Summoner Name: "
      '
      'StatusLabel
      '
      Me.StatusLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.StatusLabel.ForeColor = System.Drawing.Color.Red
      Me.StatusLabel.Location = New System.Drawing.Point(15, 167)
      Me.StatusLabel.Name = "StatusLabel"
      Me.StatusLabel.Size = New System.Drawing.Size(271, 24)
      Me.StatusLabel.TabIndex = 24
      Me.StatusLabel.Text = "RESULTS!"
      Me.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
      '
      'BlitzGGButton
      '
      Me.BlitzGGButton.Location = New System.Drawing.Point(154, 141)
      Me.BlitzGGButton.Name = "BlitzGGButton"
      Me.BlitzGGButton.Size = New System.Drawing.Size(108, 23)
      Me.BlitzGGButton.TabIndex = 3
      Me.BlitzGGButton.Text = "     Blitz.GG"
      Me.MyToolTip.SetToolTip(Me.BlitzGGButton, "Alt + Enter")
      Me.BlitzGGButton.UseVisualStyleBackColor = True
      '
      'OpGGButton
      '
      Me.OpGGButton.Location = New System.Drawing.Point(15, 141)
      Me.OpGGButton.Name = "OpGGButton"
      Me.OpGGButton.Size = New System.Drawing.Size(132, 23)
      Me.OpGGButton.TabIndex = 2
      Me.OpGGButton.Text = "    OP.GG"
      Me.MyToolTip.SetToolTip(Me.OpGGButton, "Shift + Enter")
      Me.OpGGButton.UseVisualStyleBackColor = True
      '
      'OpGGCheckBox
      '
      Me.OpGGCheckBox.AutoSize = True
      Me.OpGGCheckBox.Location = New System.Drawing.Point(19, 145)
      Me.OpGGCheckBox.Name = "OpGGCheckBox"
      Me.OpGGCheckBox.Size = New System.Drawing.Size(18, 17)
      Me.OpGGCheckBox.TabIndex = 2
      Me.OpGGCheckBox.TabStop = False
      Me.OpGGCheckBox.UseVisualStyleBackColor = True
      '
      'BlitzGGCheckBox
      '
      Me.BlitzGGCheckBox.AutoSize = True
      Me.BlitzGGCheckBox.Location = New System.Drawing.Point(158, 145)
      Me.BlitzGGCheckBox.Name = "BlitzGGCheckBox"
      Me.BlitzGGCheckBox.Size = New System.Drawing.Size(18, 17)
      Me.BlitzGGCheckBox.TabIndex = 3
      Me.BlitzGGCheckBox.TabStop = False
      Me.BlitzGGCheckBox.UseVisualStyleBackColor = True
      '
      'IDLabel
      '
      Me.IDLabel.BorderStyle = System.Windows.Forms.BorderStyle.None
      Me.IDLabel.Location = New System.Drawing.Point(15, 193)
      Me.IDLabel.Name = "IDLabel"
      Me.IDLabel.ReadOnly = True
      Me.IDLabel.Size = New System.Drawing.Size(271, 15)
      Me.IDLabel.TabIndex = 26
      Me.IDLabel.TabStop = False
      Me.IDLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
      '
      'AddCheckBox
      '
      Me.AddCheckBox.AutoSize = True
      Me.AddCheckBox.Location = New System.Drawing.Point(19, 116)
      Me.AddCheckBox.Name = "AddCheckBox"
      Me.AddCheckBox.Size = New System.Drawing.Size(18, 17)
      Me.AddCheckBox.TabIndex = 27
      Me.AddCheckBox.TabStop = False
      Me.AddCheckBox.UseVisualStyleBackColor = True
      '
      'BlitzGGProfileButton
      '
      Me.BlitzGGProfileButton.Location = New System.Drawing.Point(263, 141)
      Me.BlitzGGProfileButton.Name = "BlitzGGProfileButton"
      Me.BlitzGGProfileButton.Padding = New System.Windows.Forms.Padding(1, 0, 0, 0)
      Me.BlitzGGProfileButton.Size = New System.Drawing.Size(23, 23)
      Me.BlitzGGProfileButton.TabIndex = 4
      Me.BlitzGGProfileButton.Text = "P"
      Me.MyToolTip.SetToolTip(Me.BlitzGGProfileButton, "Control + Enter")
      Me.BlitzGGProfileButton.UseVisualStyleBackColor = True
      '
      'NoRepeatBackgroundWorker
      '
      '
      'SpectatorWindow
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.ClientSize = New System.Drawing.Size(301, 195)
      Me.Controls.Add(Me.BlitzGGProfileButton)
      Me.Controls.Add(Me.AddCheckBox)
      Me.Controls.Add(Me.IDLabel)
      Me.Controls.Add(Me.BlitzGGCheckBox)
      Me.Controls.Add(Me.OpGGCheckBox)
      Me.Controls.Add(Me.OpGGButton)
      Me.Controls.Add(Me.BlitzGGButton)
      Me.Controls.Add(Me.StatusLabel)
      Me.Controls.Add(Me.SummonerNameLabel)
      Me.Controls.Add(Me.SaveSettingsButton)
      Me.Controls.Add(Me.CheckVersionButton)
      Me.Controls.Add(Me.SpectateUserButton)
      Me.Controls.Add(Me.NamesComboBox)
      Me.Controls.Add(Me.VersionLabel)
      Me.Controls.Add(Me.VersionTextBox)
      Me.Controls.Add(Me.FolderLabel)
      Me.Controls.Add(Me.LoLFolderTextBox)
      Me.Controls.Add(Me.CommandOnlyCheckbox)
      Me.Controls.Add(Me.CommandTextBox)
      Me.Controls.Add(Me.ShapeContainer1)
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
      Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
      Me.MaximizeBox = False
      Me.Name = "SpectatorWindow"
      Me.Text = "Spectate"
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub
   Friend WithEvents CommandTextBox As System.Windows.Forms.TextBox
   Friend WithEvents CommandOnlyCheckbox As System.Windows.Forms.CheckBox
   Friend WithEvents LoLFolderTextBox As System.Windows.Forms.TextBox
   Friend WithEvents FolderLabel As System.Windows.Forms.Label
   Friend WithEvents VersionLabel As System.Windows.Forms.Label
   Friend WithEvents VersionTextBox As System.Windows.Forms.TextBox
   Friend WithEvents NamesComboBox As System.Windows.Forms.ComboBox
   Friend WithEvents SpectateUserButton As System.Windows.Forms.Button
   Friend WithEvents CheckVersionButton As System.Windows.Forms.Button
   Friend WithEvents SaveSettingsButton As System.Windows.Forms.Button
   Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
   Friend WithEvents LineShape1 As Microsoft.VisualBasic.PowerPacks.LineShape
   Friend WithEvents SummonerNameLabel As System.Windows.Forms.Label
   Friend WithEvents StatusLabel As System.Windows.Forms.Label
   Friend WithEvents BlitzGGButton As System.Windows.Forms.Button
   Friend WithEvents OpGGButton As System.Windows.Forms.Button
   Friend WithEvents OpGGCheckBox As System.Windows.Forms.CheckBox
   Friend WithEvents BlitzGGCheckBox As System.Windows.Forms.CheckBox
   Friend WithEvents IDLabel As System.Windows.Forms.TextBox
   Friend WithEvents AddCheckBox As System.Windows.Forms.CheckBox
   Friend WithEvents BlitzGGProfileButton As Button
   Friend WithEvents NoRepeatBackgroundWorker As System.ComponentModel.BackgroundWorker
   Friend WithEvents MyToolTip As ToolTip
End Class
