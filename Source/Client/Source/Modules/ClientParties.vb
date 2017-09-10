﻿Imports System.Drawing
Imports ASFW
Imports SFML.Graphics
Imports SFML.Window

Module ClientParties

#Region "Types and Globals"
    Public Party As PartyRec

    Public Structure PartyRec
        Dim Leader As Integer
        Dim Member() As Integer
        Dim MemberCount As Integer
    End Structure
#End Region

#Region "Incoming Packets"
    Sub Packet_PartyInvite(ByRef Data() As Byte)
        Dim Name As String
        Dim Buffer As New ByteStream(Data)
        Name = Buffer.ReadString

        DialogType = DIALOGUE_TYPE_PARTY

        DialogMsg1 = "Party Invite"
        DialogMsg2 = Trim$(Name) & " has invited you to a party. Would you like to join?"

        UpdateDialog = True

        Buffer.Dispose()
    End Sub

    Sub Packet_PartyUpdate(ByRef Data() As Byte)
        Dim I As Integer, InParty As Integer
        Dim Buffer As New ByteStream(Data)
        InParty = Buffer.ReadInt32

        ' exit out if we're not in a party
        If InParty = 0 Then
            ClearParty()
            ' exit out early
            Buffer.Dispose()
            Exit Sub
        End If

        ' carry on otherwise
        Party.Leader = Buffer.ReadInt32
        For I = 1 To MAX_PARTY_MEMBERS
            Party.Member(I) = Buffer.ReadInt32
        Next
        Party.MemberCount = Buffer.ReadInt32

        Buffer.Dispose()
    End Sub

    Sub Packet_PartyVitals(ByRef Data() As Byte)
        Dim playerNum As Integer, partyIndex As Integer
        Dim Buffer As New ByteStream(Data)
        ' which player?
        playerNum = Buffer.ReadInt32

        ' find the party number
        For I = 1 To MAX_PARTY_MEMBERS
            If Party.Member(I) = playerNum Then
                partyIndex = I
            End If
        Next

        ' exit out if wrong data
        If partyIndex <= 0 OrElse partyIndex > MAX_PARTY_MEMBERS Then Exit Sub

        ' set vitals
        Player(playerNum).MaxHP = Buffer.ReadInt32
        Player(playerNum).Vital(Vitals.HP) = Buffer.ReadInt32

        Player(playerNum).MaxMP = Buffer.ReadInt32
        Player(playerNum).Vital(Vitals.MP) = Buffer.ReadInt32

        Player(playerNum).MaxSP = Buffer.ReadInt32
        Player(playerNum).Vital(Vitals.SP) = Buffer.ReadInt32

        Buffer.Dispose()
    End Sub
#End Region

#Region "Outgoing Packets"
    Public Sub SendPartyRequest(ByVal Name As String)
        Dim Buffer As New ByteStream(4)
        Buffer.WriteInt32(ClientPackets.CRequestParty)
        Buffer.WriteString(Name)

        Socket.SendData(Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Public Sub SendAcceptParty()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CAcceptParty)

        Socket.SendData(Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Public Sub SendDeclineParty()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CDeclineParty)

        Socket.SendData(Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Public Sub SendLeaveParty()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CLeaveParty)

        Socket.SendData(Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Public Sub SendPartyChatMsg(ByVal Text As String)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CPartyChatMsg)
        Buffer.WriteString(Text)

        Socket.SendData(Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub
#End Region

    Sub ClearParty()
        Party = New PartyRec With {
            .Leader = 0,
            .MemberCount = 0
        }
        ReDim Party.Member(MAX_PARTY_MEMBERS)
    End Sub

    Public Sub DrawParty()
        Dim I As Long, X As Long, Y As Long, barwidth As Long, playerNum As Long, theName As String
        Dim rec(1) As Rectangle

        ' render the window

        ' draw the bars
        If Party.Leader > 0 Then ' make sure we're in a party
            ' draw leader
            playerNum = Party.Leader
            ' name
            theName = Trim$(GetPlayerName(playerNum))
            ' draw name
            Y = 100
            X = 10
            DrawText(X, Y, theName, SFML.Graphics.Color.Yellow, SFML.Graphics.Color.Black, GameWindow)

            ' draw hp
            If Player(playerNum).Vital(Vitals.HP) > 0 Then
                ' calculate the width to fill
                barwidth = ((Player(playerNum).Vital(Vitals.HP) / (GetPlayerMaxVital(playerNum, Vitals.HP)) * 64))
                ' draw bars
                rec(1) = New Rectangle(X, Y, barwidth, 6)
                Dim rectShape As New RectangleShape(New Vector2f(barwidth, 6))
                rectShape.Position = New Vector2f(X, Y + 15)
                rectShape.FillColor = SFML.Graphics.Color.Red
                GameWindow.Draw(rectShape)
            End If
            ' draw mp
            If Player(playerNum).Vital(Vitals.MP) > 0 Then
                ' calculate the width to fill
                barwidth = ((Player(playerNum).Vital(Vitals.MP) / (GetPlayerMaxVital(playerNum, Vitals.MP)) * 64))
                ' draw bars
                rec(1) = New Rectangle(X, Y, barwidth, 6)
                Dim rectShape2 As New RectangleShape(New Vector2f(barwidth, 6))
                rectShape2.Position = New Vector2f(X, Y + 24)
                rectShape2.FillColor = SFML.Graphics.Color.Blue
                GameWindow.Draw(rectShape2)
            End If

            ' draw members
            For I = 1 To MAX_PARTY_MEMBERS
                If Party.Member(I) > 0 Then
                    If Party.Member(I) <> Party.Leader Then
                        ' cache the index
                        playerNum = Party.Member(I)
                        ' name
                        theName = Trim$(GetPlayerName(playerNum))
                        ' draw name
                        Y = 100 + ((I - 1) * 30)

                        DrawText(X, Y, theName, SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)
                        ' draw hp
                        Y = 115 + ((I - 1) * 30)

                        ' make sure we actually have the data before rendering
                        If GetPlayerVital(playerNum, Vitals.HP) > 0 AndAlso GetPlayerMaxVital(playerNum, Vitals.HP) > 0 Then
                            barwidth = ((Player(playerNum).Vital(Vitals.HP) / (GetPlayerMaxVital(playerNum, Vitals.HP)) * 64))
                        End If
                        rec(1) = New Rectangle(X, Y, barwidth, 6)
                        Dim rectShape As New RectangleShape(New Vector2f(barwidth, 6))
                        rectShape.Position = New Vector2f(X, Y)
                        rectShape.FillColor = SFML.Graphics.Color.Red
                        GameWindow.Draw(rectShape)
                        ' draw mp
                        Y = 115 + ((I - 1) * 30)
                        ' make sure we actually have the data before rendering
                        If GetPlayerVital(playerNum, Vitals.MP) > 0 AndAlso GetPlayerMaxVital(playerNum, Vitals.MP) > 0 Then
                            barwidth = ((Player(playerNum).Vital(Vitals.MP) / (GetPlayerMaxVital(playerNum, Vitals.MP)) * 64))
                        End If
                        rec(1) = New Rectangle(X, Y, barwidth, 6)
                        Dim rectShape2 As New RectangleShape(New Vector2f(barwidth, 6))
                        rectShape2.Position = New Vector2f(X, Y + 8)
                        rectShape2.FillColor = SFML.Graphics.Color.Blue
                        GameWindow.Draw(rectShape2)
                    End If
                End If
            Next
        End If
    End Sub
End Module