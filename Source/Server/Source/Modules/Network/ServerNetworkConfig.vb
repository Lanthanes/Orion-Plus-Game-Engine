Imports System.Net
Imports ASFW.Network

Friend Module ServerNetworkConfig
    Friend WithEvents Socket As Server

    Friend Sub InitNetwork()
        If Not Socket Is Nothing Then Return
        Socket = New Server(EditorPackets.Count, MAX_PLAYERS)

        ' Establish some Rulez
        Socket.BufferLimit = 2048000 ' <- this is 2mb max data storage
        Socket.PacketAcceptLimit = 100 ' Dunno what is a reasonable cap right now so why now? :P
        Socket.PacketDisconnectCount = 150 ' If the other thing was even remotely reasonable, this is DEFINITELY spam count!
        ' END THE ESTABLISHMENT! WOOH ANARCHY! ~SpiceyWolf

        PacketRouter() ' Need them packet ids boah!
    End Sub

    Friend Sub DestroyNetwork()
        Socket.Dispose()
    End Sub

    Friend Function GetIP() As String
        Dim request = HttpWebRequest.Create(New Uri("http://ascensiongamedev.com/resources/myip.php"))
        request.Method = WebRequestMethods.Http.Get

        Try
            Dim reader As New IO.StreamReader(request.GetResponse().GetResponseStream())
            Return reader.ReadToEnd()
        Catch e As Exception
            Return "127.0.0.1"
        End Try
    End Function

    Function IsLoggedIn(ByVal Index As Integer) As Boolean
        Return Len(Trim$(Player(Index).Login)) > 0
    End Function

    Function IsPlaying(ByVal Index As Integer) As Boolean
        Return TempPlayer(Index).InGame
    End Function

    Function IsMultiAccounts(ByVal Login As String) As Boolean
        ' Lol this was broke before ~ SpiceyWolf
        For i As Integer = 1 To GetPlayersOnline()
            If Player(i).Login.Trim.ToLower() = Login.Trim.ToLower() Then Return True
        Next
        Return False
    End Function

    Friend Sub SendDataToAll(ByRef data() As Byte, ByVal head As Integer)
        For i As Integer = 1 To GetPlayersOnline()
            If IsPlaying(i) Then
                Socket.SendDataTo(i, data, head)
            End If
        Next
    End Sub

    Sub SendDataToAllBut(ByVal Index As Integer, ByRef Data() As Byte, ByVal head As Integer)
        For i As Integer = 1 To GetPlayersOnline()
            If IsPlaying(i) AndAlso i <> Index Then
                Socket.SendDataTo(i, Data, head)
            End If
        Next
    End Sub

    Sub SendDataToMapBut(ByVal Index As Integer, ByVal MapNum As Integer, ByRef Data() As Byte, ByVal head As Integer)
        For i As Integer = 1 To GetPlayersOnline()
            If IsPlaying(i) AndAlso GetPlayerMap(i) = MapNum AndAlso i <> Index Then
                Socket.SendDataTo(i, Data, head)
            End If
        Next
    End Sub

    Sub SendDataToMap(ByVal MapNum As Integer, ByRef Data() As Byte, ByVal head As Integer)
        Dim i As Integer

        For i = 1 To GetPlayersOnline()

            If IsPlaying(i) Then
                If GetPlayerMap(i) = MapNum Then
                    Socket.SendDataTo(i, Data, head)
                End If
            End If

        Next

    End Sub

#Region " Events "
    Friend Sub Socket_ConnectionReceived(ByVal index As Integer) Handles Socket.ConnectionReceived
        Console.WriteLine("Connection received on index[" & index & "] - IP[" & Socket.ClientIp(index) & "]")
        SendKeyPair(index)
        SendNews(index)
    End Sub

    Friend Sub Socket_ConnectionLost(ByVal index As Integer) Handles Socket.ConnectionLost
        Console.WriteLine("Connection lost on index[" & index & "] - IP[" & Socket.ClientIp(index) & "]")
        LeftGame(index)
    End Sub

    Friend Sub Socket_CrashReport(ByVal index As Integer, ByVal err As String) Handles Socket.CrashReport
        Console.WriteLine("There was a network error -> Index[" & index & "]")
        Console.WriteLine("Report: " & err)
        LeftGame(index)
    End Sub


#If DEBUG Then
    Private Sub Socket_TrafficReceived(ByVal size As Integer, ByRef data() As Byte) Handles Socket.TrafficReceived
        Console.WriteLine("Traffic Received : [Size: " & size & "]")
        Dim tmpData = data
        Dim BreakPointDummy As Integer
        'Put breakline on BreakPointDummy to look at what is contained in data at runtime in the VS logger.
    End Sub

    Private Sub Socket_PacketReceived(ByVal size As Integer, ByVal header As Integer, ByRef data() As Byte) Handles Socket.PacketReceived
        Console.WriteLine("Packet Received : [Size: " & size & "| Packet: " & CType(header, ServerPackets).ToString() & "]")
        Dim tmpData = data
        Dim BreakPointDummy As Integer
        'Put breakline on BreakPointDummy to look at what is contained in data at runtime in the VS logger.
    End Sub
#End If
#End Region
End Module
