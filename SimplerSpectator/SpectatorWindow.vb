Imports System.Text.RegularExpressions
Imports SpectatorFunctions
Imports EasyCache

Public Class SpectatorWindow

   Private Sub ReloadLastButton_Click(sender As Object, e As EventArgs)
      'If (lastGame IsNot Nothing) Then
      '   SpectateGame(lastGame)
      'Else
      '   ReloadLastButton.Enabled = False
      'End If
   End Sub

   Private Sub SpectatorWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      CacheManager.Init(New EasyCache.EasyCache() {New Spectator})
      CacheManager.LoadAllCaches()

      MySpectator = CacheManager.RetrieveCache(Of Spectator)()
      RevalidateControls()
   End Sub

   Dim MySpectator As Spectator

   'C:\Program Files (x86)\Riot Games\League of Legends\RADS\solutions\lol_game_client_sln\releases\0.0.1.10\deploy

   Private Sub CommandTextBox_Click(sender As Object, e As EventArgs) Handles CommandTextBox.Click
      CommandTextBox.SelectAll()
   End Sub

   'Private Sub CheckButton_Click(sender As Object, e As EventArgs)
   '   If MySpectator.PopupCheckLeagueVersion() Then
   '      RevalidateControls()
   '   End If
   'End Sub

   Private Sub RevalidateControls()
      LoLFolderTextBox.Text = MySpectator.LoLFolder
      VersionTextBox.Text = "Nothing"
      'VersionTextBox.Text = MySpectator.Version
      NamesComboBox.Items.Clear()
      For Each n In MySpectator.CachedNames
         NamesComboBox.Items.Add(n)
      Next
      If MySpectator.LastSummonerName = "" Then
         NamesComboBox.Text = ""
      Else
         NamesComboBox.SelectedItem = MySpectator.LastSummonerName
         If NamesComboBox.Text = "" Then
            NamesComboBox.Text = MySpectator.LastSummonerName
         End If
      End If
      ResultLabel.ForeColor = Color.LightBlue
      ResultLabel.Text = "Standby"

      DisplayID()
   End Sub

   ' Currently unused
   Private Sub DisplayID()
      Return
      Dim ID = MySpectator.GetSummonerID(NamesComboBox.Text)
      Dim IDStr As String = If(ID = 0, "", ID)
      IDLabel.Text = IDStr
   End Sub

   Private Sub SpectatorWindow_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
      CacheManager.StoreAllCaches()
   End Sub

   Private Sub NamesComboBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles NamesComboBox.KeyPress
      'Console.WriteLine(e.KeyChar.ToString & ":" & e.KeyChar & ":" & NamesComboBox.Text)

      If e.KeyChar = ChrW(Keys.Delete) AndAlso NamesComboBox.DroppedDown Then
         MySpectator.RemoveSummonerIfExists(NamesComboBox.SelectedItem)
         RevalidateControls()
         e.Handled = True
         'ElseIf e.KeyChar = Convert.ToChar(1) Then
         '   Console.WriteLine("asdf")
         '   'NamesComboBox.Focus()
         '   e.Handled = True
      Else
      End If
   End Sub

   Private Sub NamesComboBox_KeyUp(sender As Object, e As KeyEventArgs) Handles NamesComboBox.KeyUp
      If e.KeyData = Keys.Enter Then
         If DoingAutoComplete Then
            Return
         End If

         SpectateUserButton_Click(Nothing, Nothing)
         NamesComboBox.SelectAll()
         e.Handled = True
      End If
   End Sub

   Private DoingAutoComplete As Boolean = False
   Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
      DoingAutoComplete = False
      If keyData = Keys.Delete AndAlso NamesComboBox.DroppedDown AndAlso NamesComboBox.Focused Then
         NamesComboBox_KeyPress(Nothing, New KeyPressEventArgs(ChrW(Keys.Delete)))
         Return True
      ElseIf keyData = Keys.Enter AndAlso NamesComboBox.Focused Then
         If NamesComboBox.SelectedItem = "" Then
            DoingAutoComplete = True
            Return False
         End If
      ElseIf keyData = (Keys.Control Or Keys.F) Then
         ' Ctrl F
         NamesComboBox.Focus()
         Return True
      ElseIf keyData = (Keys.Control Or Keys.R) Then
         SpectateUserButton_Click(Nothing, Nothing)
         Return True
      ElseIf keyData = (Keys.Control Or Keys.A) AndAlso NamesComboBox.Focused Then
         NamesComboBox.Focus()
         Return True
      Else
         Return MyBase.ProcessCmdKey(msg, keyData)
      End If
      Return False
   End Function

   'Private Sub CheckVersionButton_Click(sender As Object, e As EventArgs) Handles CheckVersionButton.Click
   '   If MySpectator.PopupCheckLeagueVersion() Then
   '      RevalidateControls()
   '   End If
   '   MySpectator.FormatAllNames()
   '   RevalidateControls()
   'End Sub

   Private Sub SaveSettingsButton_Click(sender As Object, e As EventArgs) Handles SaveSettingsButton.Click
      CacheManager.StoreAllCaches()
   End Sub

   Private Sub OpGGButton_Click(sender As Object, e As EventArgs) Handles OpGGButton.Click
      If NamesComboBox.Text <> "" And Not NamesComboBox.Text Is Nothing Then
         Process.Start("http://na.op.gg/summoner/userName=" & NamesComboBox.Text)
      End If
   End Sub

   Private Sub BlitzGGButton_Click(sender As Object, e As EventArgs) Handles BlitzGGButton.Click
      If NamesComboBox.Text <> "" And Not NamesComboBox.Text Is Nothing Then
         Dim blitzInfo As New ProcessStartInfo
         With blitzInfo
            .FileName = "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
            .Arguments = "--check-for-update-interval=604800 --profile-directory=""Default"" --start-maximized --app=""https://blitz.gg/lol/live/na1/" & NamesComboBox.Text & """"
         End With

         Process.Start(blitzInfo)
      End If
   End Sub

   Private Sub BlitzGGProfileButton_Click(sender As Object, e As EventArgs) Handles BlitzGGProfileButton.Click
      Dim blitzInfo As New ProcessStartInfo
      With blitzInfo
         .FileName = "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
         .Arguments = "--check-for-update-interval=604800 --profile-directory=""Default"" --start-maximized --app=""https://blitz.gg/lol/profile/na1/" & NamesComboBox.Text & """"
      End With

      Process.Start(blitzInfo)
   End Sub

   Private Sub SpectateUserButton_Click(sender As Object, e As EventArgs) Handles SpectateUserButton.Click
      If OpGGCheckBox.Checked Then
         OpGGButton_Click(Nothing, Nothing)
      End If
      If BlitzGGCheckBox.Checked Then
         BlitzGGButton_Click(Nothing, Nothing)
      End If

      Dim result = MySpectator.SpectateGame(NamesComboBox.Text, AddCheckBox.Checked)
      Select Case result
         Case Spectator.SpectateGameResult.OtherWebError
            ResultLabel.ForeColor = Color.Red
            ResultLabel.Text = "Error: Other/Web error!"
         Case Spectator.SpectateGameResult.APIError
            ResultLabel.ForeColor = Color.Red
            ResultLabel.Text = "Error: API error!"
         Case Spectator.SpectateGameResult.NotInGame
            RevalidateControls()
            ResultLabel.ForeColor = Color.Red
            ResultLabel.Text = "Error: Not in a game!"
         Case Spectator.SpectateGameResult.SummonerNotFound
            ResultLabel.ForeColor = Color.Red
            ResultLabel.Text = "Error: Summoner not found!"
         Case Spectator.SpectateGameResult.Success
            RevalidateControls()
            ResultLabel.ForeColor = Color.Green
            ResultLabel.Text = "Loading game!"
      End Select
   End Sub

   Private Sub NamesComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles NamesComboBox.SelectedIndexChanged
      Console.WriteLine("SIC:" & NamesComboBox.SelectedIndex & ":" & NamesComboBox.SelectedItem & ":" & NamesComboBox.SelectedText & ":" & NamesComboBox.SelectedValue)
      NamesComboBox.SelectedValue = NamesComboBox.SelectedItem
      DisplayID()
   End Sub

   Private Sub NamesComboBox_SelectedValueChanged(sender As Object, e As EventArgs) Handles NamesComboBox.SelectedValueChanged
      Console.WriteLine("SVC:" & NamesComboBox.SelectedIndex & ":" & NamesComboBox.SelectedItem & ":" & NamesComboBox.SelectedText & ":" & NamesComboBox.SelectedValue)
      NamesComboBox.SelectedValue = NamesComboBox.SelectedItem
   End Sub

   Private Sub NamesComboBox_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles NamesComboBox.SelectionChangeCommitted
      Console.WriteLine("SCC:" & NamesComboBox.SelectedIndex & ":" & NamesComboBox.SelectedItem & ":" & NamesComboBox.SelectedText & ":" & NamesComboBox.SelectedValue)
      NamesComboBox.SelectedValue = NamesComboBox.SelectedItem
   End Sub
End Class