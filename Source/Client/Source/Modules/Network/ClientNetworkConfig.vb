﻿Imports System.Windows.Forms
Imports ASFW.Network

Friend Module ClientNetworkConfig
    Friend WithEvents Socket As Client

    Friend Sub InitNetwork()
        If Not Socket Is Nothing Then Return
        Socket = New Client(ServerPackets.COUNT)
        PacketRouter()
    End Sub

    Friend Sub Connect()
        Socket.Connect(Options.IP, Options.Port)
    End Sub

    Friend Sub DestroyNetwork()
        ' Calling a disconnect is not necessary when using destroy network as
        ' Dispose already calls it and cleans up the memory internally.
        Socket.Dispose()
    End Sub

#Region " Events "
    Private Sub Socket_ConnectionSuccess() Handles Socket.ConnectionSuccess

    End Sub

    Private Sub Socket_ConnectionFailed() Handles Socket.ConnectionFailed

    End Sub

    Private Sub Socket_ConnectionLost() Handles Socket.ConnectionLost
        MsgBox("Connection was terminated!")
        DestroyNetwork()
        DestroyGame()
    End Sub

    Private Sub Socket_CrashReport(ByVal err As String) Handles Socket.CrashReport
        MsgBox("There was a network error -> Report: " & err)
        DestroyNetwork()
        DestroyGame()
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
