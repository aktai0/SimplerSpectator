Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions

Public Class APIHelper
   Private Const API_KEY_FILE As String = "riot_key"
   Private Shared _API_KEY As String = ""
   Private Shared ReadOnly Property API_KEY() As String
      Get
         If _API_KEY = "" Then
            API_INIT()
            Return _API_KEY
         End If
         If LastWriteTime < GetAPIKeyLastWriteTime Then
            API_LOAD_FILE()
         End If
         Return _API_KEY
      End Get
   End Property

   Private Shared LastWriteTime As DateTime
   Private Shared Sub API_LOAD_FILE()
      Try
         Using reader As New IO.StreamReader(APIHelper.API_KEY_FILE)
            APIHelper._API_KEY = reader.ReadLine().Trim
         End Using

         LastWriteTime = GetAPIKeyLastWriteTime()
      Catch ex As Exception
         MsgBox("Error reading riot_key file. Please check README for instructions.")
      End Try
   End Sub

   Private Shared ReadOnly Property GetAPIKeyLastWriteTime() As DateTime
      Get
         Return (New FileInfo(APIHelper.API_KEY_FILE)).LastWriteTime
      End Get
   End Property

   Public Shared Sub API_INIT()
      If _API_KEY = "" Then
         API_LOAD_FILE()
      End If
   End Sub

   Private Const SUMMONER_ENDPOINT_URL As String = "https://na1.api.riotgames.com/lol/summoner/v4/summoners/by-name/%NAME%?api_key=%KEY%"
   Private Const SPECTATOR_ENDPOINT_URL As String = "https://na1.api.riotgames.com/lol/spectator/v4/active-games/by-summoner/%ID%?api_key=%KEY%"

   Private Shared Function GetURLOutput(ByVal inputURL As String) As String
      Dim strOutput As String = ""
      Dim formatted As String = ""
      Try
         Dim strURL As String = inputURL

         Dim wrResponse As HttpWebResponse
         Dim wrRequest As HttpWebRequest = CType(HttpWebRequest.Create(strURL), HttpWebRequest)
         wrRequest.Timeout = 5000

         Try
            wrResponse = CType(wrRequest.GetResponse(), HttpWebResponse)
         Catch ex As WebException
            If ex.Status = WebExceptionStatus.ProtocolError Then
               wrResponse = CType(ex.Response, HttpWebResponse)
               If wrResponse IsNot Nothing Then
                  Return CStr(wrResponse.StatusCode)
               End If
            Else
               Return "Other Error"
            End If
         Catch ex As Exception
            Return "Other Error"
         End Try

         Using sr As New StreamReader(wrResponse.GetResponseStream())
            strOutput = sr.ReadToEnd()
            ' Close and clean up the StreamReader
            sr.Close()
         End Using
      Catch ex As Exception
         Return "Other Error"
      End Try
      Return strOutput
   End Function

   Public Class SummonerInfo
      Public ReadOnly SummonerID As String
      Public ReadOnly SummonerName As String

      Friend Sub New(ByVal sID As String, ByVal sName As String)
         SummonerID = sID
         SummonerName = sName
      End Sub
   End Class

   ' Calls API and returns the summoner ID of the given name. If there's an error, throw the appropriate exception.
   Public Shared Function QuerySummonerIDOnly(ByVal name As String) As SummonerInfo
      Dim url = SUMMONER_ENDPOINT_URL.Replace("%KEY%", API_KEY).Replace("%NAME%", name)
      Dim result = APIHelper.GetURLOutput(url)
      If result = "Other Error" Then
         Throw New OtherWebError
      ElseIf result = "404" Then
         Throw New SummonerNotFoundException
      ElseIf result.Length = 3 Then
         Throw New APIErrorException
      End If
      ' Might need to change parsing the API output later
      Dim sID As String = ""
      Dim sName As String = ""

      Dim re As New Regex("""id"":""([^""]+)"",")
      Dim match = re.Match(result)
      If match.Groups.Count > 1 Then
         sID = match.Groups(1).Value
      End If

      Dim re2 As New Regex("""name"":""([^""]+)"",")
      Dim match2 = re2.Match(result)
      If match2.Groups.Count > 1 Then
         sName = match2.Groups(1).Value
      End If

      If sID.Length = 0 OrElse sName.Length = 0 Then
         MsgBox("Exception wasn't caught by the end: " & result)
         Throw New APIErrorException
      Else
         Return New SummonerInfo(sID, sName)
      End If
   End Function

   ' Calls API and returns the spectator info of the game of the given ID. If there's an error, throw the appropriate exception.
   Public Shared Function QuerySpectator(ByVal summonerID As String) As SpectatorInfo
      Dim url = SPECTATOR_ENDPOINT_URL.Replace("%KEY%", API_KEY).Replace("%ID%", summonerID)
      Dim result = APIHelper.GetURLOutput(url)
      If result = "Other Error" Then
         Throw New OtherWebError
      ElseIf result = "404" Then
         Throw New SummonerNotInGameException
      ElseIf result.Length = 3 Then
         Throw New APIErrorException
      End If

      Dim enc As String = ""
      Dim id As String = ""
      Dim participantName As String = ""

      Dim re As New Regex("""gameId"":(\d+),")
      Dim match = re.Match(result)
      If match.Groups.Count > 1 Then
         id = match.Groups(1).Value
      End If

      re = New Regex("""encryptionKey"":""([^""]+)""")
      match = re.Match(result)
      If match.Groups.Count > 1 Then
         enc = match.Groups(1).Value
      End If

      Dim idIndex = result.IndexOf(summonerID)

      re = New Regex("""summonerName"":""([^""]+)""")
      match = re.Match(result, idIndex - 100, 100)
      If match.Groups.Count > 1 Then
         participantName = match.Groups(1).Value
      End If

      If id <> "" AndAlso enc <> "" Then
         Return New SpectatorInfo(enc, id, participantName)
      End If
      MsgBox("Exception wasn't caught by the end: " & result)
      Throw New APIErrorException
   End Function

   Public Class SpectatorInfo
      Public EncryptionKey As String = ""
      Public GameID As String = ""
      Public ParticipantName As String

      Public Sub New(ByVal encryption As String, ByVal id As String, ByVal part As String)
         EncryptionKey = encryption
         GameID = id
         ParticipantName = part
      End Sub
   End Class

   Public Class APIErrorException
      Inherits Exception
   End Class

   Public Class SummonerNotFoundException
      Inherits Exception
   End Class

   Public Class SummonerNotInGameException
      Inherits Exception
   End Class

   Public Class OtherWebError
      Inherits Exception
   End Class
End Class
