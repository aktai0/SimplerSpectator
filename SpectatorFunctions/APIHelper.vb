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

   Private Const SUMMONER_ENDPOINT_URL As String = "https://na1.api.riotgames.com/lol/summoner/v3/summoners/by-name/%NAME%?api_key=%KEY%"
   Private Const SPECTATOR_ENDPOINT_URL As String = "https://na1.api.riotgames.com/lol/spectator/v3/active-games/by-summoner/%ID%?api_key=%KEY%"

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

   ' Calls API and returns the summoner ID of the given name. If there's an error, throw the appropriate exception.
   Public Shared Function QuerySummonerIDOnly(ByVal name As String) As String
      Dim url = SUMMONER_ENDPOINT_URL.Replace("%KEY%", API_KEY).Replace("%NAME%", name)
      Dim result = APIHelper.GetURLOutput(url)
      If result = "Other Error" Then
         Throw New OtherWebError
      ElseIf result = "404" Then
         Throw New SummonerNotFoundException
      ElseIf result.Length = 3 Then
         Throw New APIErrorException
      End If
      Dim re As New Regex("""id"":(\d+),")
      Dim match = re.Match(result)
      If match.Groups.Count > 1 Then
         Return match.Groups(1).Value
      End If
      MsgBox("Exception wasn't caught by the end: " & result)
      Throw New APIErrorException
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

      If id <> "" AndAlso enc <> "" Then
         Return New SpectatorInfo(enc, id)
      End If
      MsgBox("Exception wasn't caught by the end: " & result)
      Throw New APIErrorException
   End Function

   Public Class SpectatorInfo
      Public EncryptionKey As String = ""
      Public GameID As String = ""

      Public Sub New(ByVal encryption As String, ByVal id As String)
         EncryptionKey = encryption
         GameID = id
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
