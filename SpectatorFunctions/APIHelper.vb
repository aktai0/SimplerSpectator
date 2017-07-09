Imports RiotSharp

Public Class APIHelper
   Private Const API_KEY_FILE As String = "riot_key"
   Private Shared API_KEY As String = ""

   Private Shared Sub API_LOAD_FILE()
      Try
         Using reader As New IO.StreamReader(APIHelper.API_KEY_FILE)
            APIHelper.API_KEY = reader.ReadLine().Trim
         End Using

         RiotApi.GetInstance(APIHelper.API_KEY)
      Catch ex As Exception
         MsgBox("Error reading riot_key file. Please check README for instructions.")
      End Try
   End Sub

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
         Return RiotApi.GetInstance(API_KEY)
      End Get
   End Property
End Class
