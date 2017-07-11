Imports RiotSharp
Imports System.IO

Public Class APIHelper
   Private Const API_KEY_FILE As String = "riot_key"
   Private Shared API_KEY As String = ""

   Private Shared LastWriteTime As DateTime
   Private Shared Sub API_LOAD_FILE()
      Try
         Using reader As New IO.StreamReader(APIHelper.API_KEY_FILE)
            APIHelper.API_KEY = reader.ReadLine().Trim
         End Using

         RiotApi.GetInstance(APIHelper.API_KEY)
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
      If API_KEY = "" Then
         API_LOAD_FILE()
      End If
   End Sub

   Public Shared ReadOnly Property GetRiotSharpInstance As RiotApi
      Get
         If API_KEY = "" Then
            API_INIT()
         End If
         If LastWriteTime < GetAPIKeyLastWriteTime Then
            API_LOAD_FILE()
         End If
         ' Check if API key file modified was newer than the previous value
         Return RiotApi.GetInstance(API_KEY)
      End Get
   End Property
End Class
