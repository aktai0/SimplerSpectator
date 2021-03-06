﻿Imports System.Text.RegularExpressions
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

      RefreshNamesList()

      StatusLabel.ForeColor = Color.LightBlue
      StatusLabel.Text = "Standby"

      ' DisplayID()
   End Sub

   Private Sub RefreshNamesList()
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
   End Sub

   '' Currently unused
   'Private Sub DisplayID()
   '   Return
   '   Dim ID = MySpectator.GetSummonerID(NamesComboBox.Text)
   '   Dim IDStr As String = If(ID = 0, "", ID)
   '   IDLabel.Text = IDStr
   'End Sub

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
            DoingAutoComplete = False
            Return
         End If

         If NamesComboBox.Text = "" Then
            Return
         End If

         SpectateUserButton_Click(Nothing, Nothing)
         NamesComboBox.SelectAll()
         e.Handled = True
      End If
   End Sub

   Private Sub NoRepeat()
      Waiting = True
      NoRepeatBackgroundWorker.RunWorkerAsync()
   End Sub

   Private Waiting As Boolean = False
   Private DoingAutoComplete As Boolean = False
   ' Handles pressing Delete or Enter, also is called when the suggestion dropdown is showing 
   '  and the user clicks a suggestion, due to a quirk in ComboBox
   ' Enter/clicking a suggestion should have the following behavior:
   ' - If the user begins typing and presses Enter to fill the append from the selected suggestion,
   '    then fill in the name (ComboBox does this by itself) and don't try to spectate.
   ' - If the user begins typing and clicks on a suggestion,
   '    then fill in the name (ComboBox does this by itself) and don't try to spectate.
   ' - If the user begins typing and presses Enter without a suggestion showing (i.e. a new name),
   '    then try spectating.
   ' - If the user presses Enter while ComboBox has a valid/known name,
   '    then try spectating.
   ' Keys enum: "Do not use the values in this enumeration for combined bitwise operations.
   '  The values in the enumeration are not mutually exclusive."
   Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
      If keyData = Keys.Delete AndAlso NamesComboBox.DroppedDown AndAlso NamesComboBox.Focused Then
         NamesComboBox_KeyPress(Nothing, New KeyPressEventArgs(ChrW(Keys.Delete)))
         Return True
      ElseIf Not Waiting AndAlso keyData.HasFlag(Keys.Enter) AndAlso (keyData.HasFlag(Keys.Shift) OrElse keyData.HasFlag(Keys.Control) OrElse keyData.HasFlag(Keys.Alt)) Then
         If keyData.HasFlag(Keys.Shift) Then
            OpGGButton_Click(Nothing, Nothing)
         End If
         If keyData.HasFlag(Keys.Control) Then
            BlitzGGProfileButton_Click(Nothing, Nothing)
         End If
         If keyData.HasFlag(Keys.Alt) Then
            PorofessorGGButton_Click(Nothing, Nothing)
         End If
         NoRepeat()
      ElseIf keyData = Keys.Enter AndAlso NamesComboBox.Focused Then
         Console.WriteLine("ProcessCmdKey: " & " Sel Start: " & NamesComboBox.SelectionStart & ", Len: " & NamesComboBox.SelectionLength & ", T: " & NamesComboBox.Text)

         If IsKnownSummoner(NamesComboBox.SelectedItem) Then
            ' Current item is in the names list and not suggested, spectate
         ElseIf IsKnownSummoner(NamesComboBox.Text) Then
            ' Current suggested item is being applied, don't spectate
            ' Pressing Enter and clicking a suggestion both count as SelectedItem being "",
            '  so the first conditional is false. After the user chooses a suggestion, pressing
            '  Enter again will cause the first conditional to be true.
            DoingAutoComplete = True
         Else
            ' Current item is an unknown name, spectate
         End If

         Return False
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

   ' Check if the text in ComboBox is completely selected
   Private ReadOnly Property IsNameSelected() As Boolean
      Get
         Return NamesComboBox.SelectionStart = 0 AndAlso NamesComboBox.SelectionLength = NamesComboBox.Text.Length
      End Get
   End Property

   ' Check if the given name is in the ComboBox's list of names, ignoring spacing & capitalization
   Private Function IsKnownSummoner(ByVal name As String) As Boolean
      Return NamesComboBox.Items.Cast(Of String).Where(Function(x) StringComparer.OrdinalIgnoreCase().Compare(x.Replace(" ", ""), name?.Replace(" ", "")) = 0)?.Count > 0
   End Function

   'Private Sub CheckVersionButton_Click(sender As Object, e As EventArgs) Handles CheckVersionButton.Click
   '   If MySpectator.PopupCheckLeagueVersion() Then
   '      RevalidateControls()
   '   End If
   '   MySpectator.FormatAllNames()
   '   RevalidateControls()
   'End Sub

   Private Sub SaveSettingsButton_Click(sender As Object, e As EventArgs) Handles SaveSettingsButton.Click
      MySpectator.LoLFolder = LoLFolderTextBox.Text
      CacheManager.StoreAllCaches()
      StatusLabel.Text = "Settings Saved"
      StatusLabel.ForeColor = Color.Green
   End Sub

   Private Sub OpGGButton_Click(sender As Object, e As EventArgs) Handles OpGGButton.Click
      If NamesComboBox.Text Is Nothing OrElse NamesComboBox.Text = "" Then
         Return
      End If
      Dim opggInfo As New ProcessStartInfo
      With opggInfo
         .FileName = "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
         .Arguments = "--incognito --check-for-update-interval=604800 --profile-directory=""Default"" --start-maximized ""http://na.op.gg/summoner/userName=" & NamesComboBox.Text & """"
      End With

      Process.Start(opggInfo)
   End Sub

   Private Sub PorofessorGGButton_Click(sender As Object, e As EventArgs) Handles PorofessorGGButton.Click
      If NamesComboBox.Text Is Nothing OrElse NamesComboBox.Text = "" Then
         Return
      End If
      Dim PorofessorInfo As New ProcessStartInfo
      With PorofessorInfo
         .FileName = "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
         .Arguments = "--incognito --check-for-update-interval=604800 --profile-directory=""Default"" --start-maximized ""https://porofessor.gg/live/na/" & NamesComboBox.Text & """"
      End With

      Process.Start(PorofessorInfo)
   End Sub

   Private Sub BlitzGGProfileButton_Click(sender As Object, e As EventArgs) Handles BlitzGGProfileButton.Click
      If NamesComboBox.Text Is Nothing OrElse NamesComboBox.Text = "" Then
         Return
      End If
      Dim blitzInfo As New ProcessStartInfo
      With blitzInfo
         .FileName = "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
         .Arguments = "--incognito --check-for-update-interval=604800 --profile-directory=""Default"" --start-maximized ""https://blitz.gg/lol/profile/na1/" & NamesComboBox.Text & """"
      End With

      Process.Start(blitzInfo)
   End Sub

   Private Sub SpectateUserButton_Click(sender As Object, e As EventArgs) Handles SpectateUserButton.Click
      DisableGoButtonBackgroundWorker.RunWorkerAsync()

      If OpGGCheckBox.Checked Then
         OpGGButton_Click(Nothing, Nothing)
      End If
      If PorofessorGGCheckBox.Checked Then
         PorofessorGGButton_Click(Nothing, Nothing)
      End If

      If LoLFolderTextBox.Text <> MySpectator.LoLFolder Then
         MySpectator.LoLFolder = LoLFolderTextBox.Text
         StatusLabel.Text = "Settings Saved"
         StatusLabel.ForeColor = Color.Green
      End If

      If Process.GetProcessesByName("League of Legends").Count > 0 Then
         StatusLabel.ForeColor = Color.Red
         StatusLabel.Text = "Error: LoL is already running!"
         Return
      End If

      Dim result = MySpectator.SpectateGame(NamesComboBox.Text, AddCheckBox.Checked)
      Select Case result
         Case Spectator.SpectateGameResult.OtherWebError
            StatusLabel.ForeColor = Color.Red
            StatusLabel.Text = "Error: Other/Web error!"
         Case Spectator.SpectateGameResult.APIError
            StatusLabel.ForeColor = Color.Red
            StatusLabel.Text = "Error: API error!"
         Case Spectator.SpectateGameResult.NotInGame
            RevalidateControls()
            StatusLabel.ForeColor = Color.Red
            StatusLabel.Text = "Error: Not in a game!"
         Case Spectator.SpectateGameResult.SummonerNotFound
            StatusLabel.ForeColor = Color.Red
            StatusLabel.Text = "Error: Summoner not found!"
         Case Spectator.SpectateGameResult.Success
            RevalidateControls()
            StatusLabel.ForeColor = Color.Green
            StatusLabel.Text = "Loading game!"
      End Select

      ' If the entered name changed, it's been fixed with the proper capitalization/spacing so update it in the text
      If MySpectator.LastSummonerName <> NamesComboBox.Text Then
         NamesComboBox.Text = MySpectator.LastSummonerName
      End If
   End Sub

   Private Sub NamesComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles NamesComboBox.SelectedIndexChanged
      Console.WriteLine("SIC:" & NamesComboBox.SelectedIndex & ":" & NamesComboBox.SelectedItem & ":" & NamesComboBox.SelectedText & ":" & NamesComboBox.SelectedValue)
      NamesComboBox.SelectedValue = NamesComboBox.SelectedItem
      ' DisplayID()
   End Sub

   Private Sub NamesComboBox_SelectedValueChanged(sender As Object, e As EventArgs) Handles NamesComboBox.SelectedValueChanged
      Console.WriteLine("SVC:" & NamesComboBox.SelectedIndex & ":" & NamesComboBox.SelectedItem & ":" & NamesComboBox.SelectedText & ":" & NamesComboBox.SelectedValue)
      NamesComboBox.SelectedValue = NamesComboBox.SelectedItem
   End Sub

   Private Sub NamesComboBox_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles NamesComboBox.SelectionChangeCommitted
      Console.WriteLine("SCC:" & NamesComboBox.SelectedIndex & ":" & NamesComboBox.SelectedItem & ":" & NamesComboBox.SelectedText & ":" & NamesComboBox.SelectedValue)
      NamesComboBox.SelectedValue = NamesComboBox.SelectedItem
   End Sub

   ' Wait 500 ms before allowing repeated inputs
   Private Sub NoRepeatBackgroundWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles NoRepeatBackgroundWorker.DoWork
      Threading.Thread.Sleep(500)
      Waiting = False
   End Sub

   Private Sub CheckVersionButton_Click(sender As Object, e As EventArgs) Handles CheckVersionButton.Click
      'MySpectator.MakeLastUsedNameDirty()
      'RefreshNamesList()
      'Return
      Dim ret = MySpectator.UpdateSummonerName(NamesComboBox.Text)
      If ret = "" Then
         StatusLabel.Text = "Summoner name was not in cache."
      ElseIf ret <> NamesComboBox.Text Then
         StatusLabel.Text = NamesComboBox.Text & " name updated to " & ret
         NamesComboBox.Text = ret
         RefreshNamesList()
      Else
         StatusLabel.Text = "Summoner name was not changed."
      End If
   End Sub

   Private Sub DisableGoButtonBackgroundWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles DisableGoButtonBackgroundWorker.DoWork
      DisableGoButtonBackgroundWorker.ReportProgress(0)
      System.Threading.Thread.Sleep(2000)
      DisableGoButtonBackgroundWorker.ReportProgress(100)
   End Sub

   Private Sub DisableGoButtonBackgroundWorker_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles DisableGoButtonBackgroundWorker.ProgressChanged
      If e.ProgressPercentage = 0 Then
         SpectateUserButton.Enabled = False
      ElseIf e.ProgressPercentage = 100 Then
         SpectateUserButton.Enabled = True
         ' If the op.gg button is focused (switches automatically when Go is disabled),
         '  or if the form isn't the active window, then focus the Go button when re-enabled.
         If OpGGButton.Focused OrElse (Not Me.ContainsFocus) Then
            SpectateUserButton.Focus()
         End If
      End If
   End Sub
End Class