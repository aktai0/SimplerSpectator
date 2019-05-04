Imports EasyCache

<Serializable>
Public Class Spectator
   Inherits EasyCache.EasyCache

   Public UseDefaultLocation As Boolean = True
   Public LoLFolder As String = "C:\Program Files (x86)\Riot Games\League of Legends\"
   Private ReleasesExt As String = "RADS\solutions\lol_game_client_sln\releases\"
   Public Version As String = "0.0.1.86"

   Public ReadOnly Property CachedNames As IEnumerable(Of String)
      Get
         Return From n In SummonerIDs.Keys
                Order By n Ascending
                Select n
      End Get
   End Property

   Public Sub RemoveSummonerIfExists(ByVal summonerName As String)
      If SummonerIDs.ContainsKey(summonerName) Then
         SummonerIDs.Remove(summonerName)
         If LastSummonerName = summonerName Then
            LastSummonerName = ""
         End If
         _CacheChanged = True
      End If
   End Sub

   Public Function PopupCheckLeagueVersion() As Boolean
      If CheckLeagueFolder() Then
         Dim newVersion As String = CheckLeagueVersions()
         If Not newVersion.Equals(Version) Then
            Select Case MsgBox("A new release of League of Legends detected: " & newVersion & ". You are currently set to use: " & Version & ". Would you like to use the newer (recommended) release instead)?", MsgBoxStyle.OkCancel, "New Release Version")
               Case MsgBoxResult.Ok
                  Version = newVersion
                  _CacheChanged = True
            End Select
            Return True
         End If
      End If
      Return False
   End Function

   Public ReadOnly Property GetLeagueExePath() As String
      Get
         Return LoLFolder & ReleasesExt & Version & "\deploy\"
      End Get
   End Property

   Public Function CheckLeagueFolder() As Boolean
      If Not IO.Directory.Exists(LoLFolder) Then
         MsgBox("An error occured: could not find your League of Legends folder.")
         Return False
      End If
      Return True
   End Function

   ' 0.0.1.186 vs 0.0.2.32, etc.
   ' 1 if a > b
   ' 0 if a == b
   ' -1 if b < a
   ' Throw exception if inputs bad
   Private Function CompareVersions(a As String, b As String) As Integer
      Dim aSplit = a.Split("."c)
      Dim bSplit = b.Split("."c)

      For i = 0 To 3
         Dim curA = CInt(aSplit(i))
         Dim curB = CInt(bSplit(i))
         If curA = curB Then
            Continue For
         End If

         If curA > curB Then
            Return 1
         ElseIf curA < curB Then
            Return -1
         End If
      Next

      Return 0
   End Function

   Public Function CheckLeagueVersions() As String
      If Not IO.Directory.Exists(LoLFolder & ReleasesExt) Then
         MsgBox("An error occured: the release folder wasn't found. It should be here: " & LoLFolder & ReleasesExt & ".")
      End If

      'Split every dir in the release folder
      Dim dirs As String() = IO.Directory.GetDirectories(LoLFolder & ReleasesExt)
      Dim versionsInDir As New List(Of String)
      For Each s As String In dirs
         Dim t1 = s.Substring(s.LastIndexOf("\"c) + 1, s.Length - s.LastIndexOf("\"c) - 1)
         versionsInDir.Add(t1)
      Next

      Dim highestVersion = Version
      For Each v In versionsInDir
         Select Case CompareVersions(highestVersion, v)
            Case -1
               highestVersion = v
            Case 0
               Continue For
            Case 1
               Continue For
         End Select
      Next

      Return highestVersion
   End Function

   Public Overrides ReadOnly Property CACHE_FILE_NAME As String
      Get
         Return "SpectatorCache.bin"
      End Get
   End Property

   Public Overrides ReadOnly Property CACHE_NAME As String
      Get
         Return "Spectator"
      End Get
   End Property

   <NonSerialized>
   Dim _CacheChanged As Boolean = False
   Public Overrides ReadOnly Property CacheChanged As Boolean
      Get
         Return _CacheChanged
      End Get
   End Property

   'Private Const RIOTS_MAGIC_NUMBER As String = """12345"""
   'Private Const LOLLAUNCHER_EXE As String = " ""LoLLauncher.exe"""
   'Private Const EMPTY_STR As String = " """""
   Private Const NA_URL As String = " ""spectator spectator.na2.lol.riotgames.com:80 "
   Private Const NA_ID As String = " NA1"""

   Public ReadOnly Property GetFullCommand(ByVal encryption As String, ByVal matchID As String) As String
      Get
         Return GetLeaguePathWithQuotes() & " " & GetArguments(encryption, matchID)
      End Get
   End Property

   Public ReadOnly Property GetLeaguePathWithQuotes() As String
      Get
         Return """" & LoLFolder & ReleasesExt & Version & "\deploy\League of Legends.exe"""
      End Get
   End Property

   Public ReadOnly Property GetArguments(ByVal encryption As String, ByVal matchID As String) As String
      Get
         Return NA_URL & encryption & " " & matchID & NA_ID & " ""-UseRads"""
      End Get
   End Property

   Public Enum SpectateGameResult
      Success
      NotInGame
      SummonerNotFound
      APIError
      OtherWebError
   End Enum

   Private SummonerIDs As New Dictionary(Of String, String)

   Public LastSummonerName As String = ""

   Public Function GetSummonerID(ByVal summonerName As String) As Integer
      If SummonerIDs.ContainsKey(summonerName) Then
         Return CInt(SummonerIDs(summonerName))
      Else
         Return 0
      End If
   End Function

   Public Sub FormatAllNames()
      Dim ids As IEnumerable(Of String) = SummonerIDs.Values.AsEnumerable
      SummonerIDs = New Dictionary(Of String, String)

      For Each i As String In ids
         SummonerIDs.Add(GetSummonerName(i), i)
      Next

      _CacheChanged = True
   End Sub

   Public Function GetSummonerName(ByVal id As String) As String
      Throw New NotImplementedException("Needs to be redone w/o RiotSharp")
   End Function

   'Public Function GetSummonerName(ByVal id As String) As String
   '   Dim api = APIHelper.GetRiotSharpInstance()
   '   Dim summoner As SummonerEndpoint.SummonerBase
   '   Try
   '      summoner = api.GetSummonerName(Region.na, CInt(id))
   '   Catch ex As RiotSharpException
   '      If ex.Message.Contains("404") Then
   '         Console.WriteLine("Summoner not found")
   '         Throw New Exception("Summoner not found")
   '      Else
   '         Console.WriteLine("API Error: " & ex.Message)
   '         Throw New Exception("API Error: " & ex.Message)
   '      End If
   '      Return Nothing
   '   End Try

   '   Return summoner.Name
   'End Function

   ' Returns the summoner's ID if already stored, otherwise query the API for it
   Private Function GetSummonerID(ByVal summonerName As String, ByVal addToList As Boolean) As String
      If SummonerIDs.ContainsKey(summonerName) Then
         Return SummonerIDs(summonerName)
      Else
         Dim summID As String = ""
         Try
            summID = APIHelper.QuerySummonerIDOnly(summonerName)
         Catch ex As Exception
            ' Let the caller handle the exception
            Throw ex
         End Try

         If addToList Then
            _CacheChanged = True
            SummonerIDs.Add(summonerName, summID)
         End If
         Return summID
      End If
   End Function

   Public Function SpectateGame(ByVal summonerName As String, Optional ByVal addSummonerToList As Boolean = True) As SpectateGameResult
      PopupCheckLeagueVersion()

      Dim summID As String = ""
      Try
         summID = GetSummonerID(summonerName, addSummonerToList)
      Catch ex As APIHelper.SummonerNotFoundException
         Return SpectateGameResult.SummonerNotFound
      Catch ex As APIHelper.APIErrorException
         Return SpectateGameResult.APIError
      Catch ex As APIHelper.OtherWebError
         Return SpectateGameResult.OtherWebError
      End Try

      Dim gameInfo As APIHelper.SpectatorInfo = Nothing
      Try
         gameInfo = APIHelper.QuerySpectator(summID)
      Catch ex As APIHelper.SummonerNotInGameException
         Return SpectateGameResult.NotInGame
      Catch ex As APIHelper.APIErrorException
         Return SpectateGameResult.APIError
      Catch ex As APIHelper.OtherWebError
         Return SpectateGameResult.OtherWebError
      End Try

      ' Create the League process with the game info
      Dim process As New System.Diagnostics.Process()

      With process.StartInfo
         .FileName = "League of Legends.exe"
         .WorkingDirectory = GetLeagueExePath
         .Arguments = GetArguments(gameInfo.EncryptionKey, gameInfo.GameID)
         .CreateNoWindow = False
      End With

      process.Start()
      LastSummonerName = summonerName

      Console.WriteLine(GetFullCommand(gameInfo.EncryptionKey, gameInfo.GameID))
      Return SpectateGameResult.Success
   End Function
End Class