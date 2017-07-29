Imports System.Drawing

Public Module ClientNpc

#Region "Data"
    Sub ClearNpcs()
        Dim i As Integer

        ReDim Npc(MAX_NPCS)

        For i = 1 To MAX_NPCS
            ClearNpc(i)
        Next

    End Sub

    Sub ClearNpc(ByVal Index As Integer)
        Npc(Index) = Nothing
        Npc(Index) = New NpcRec

        Npc(Index).Name = ""
        Npc(Index).AttackSay = ""
        For x = 0 To Stats.Count - 1
            ReDim Npc(Index).Stat(x)
        Next

        ReDim Npc(Index).DropChance(5)
        ReDim Npc(Index).DropItem(5)
        ReDim Npc(Index).DropItemValue(5)

        ReDim Npc(Index).Skill(6)
    End Sub
#End Region

#Region "Incoming Packets"
    Sub Packet_NpcDead(data() As Byte)
        Dim i As Integer
        Dim Buffer As New ByteBuffer

        Buffer.WriteBytes(data)

        If Buffer.ReadInteger <> ServerPackets.SNpcDead Then Exit Sub

        i = Buffer.ReadInteger
        ClearMapNpc(i)

        Buffer = Nothing
    End Sub

    Sub Packet_SpawnNPC(data() As Byte)
        Dim i As Integer
        Dim Buffer As New ByteBuffer

        Buffer.WriteBytes(data)

        If Buffer.ReadInteger <> ServerPackets.SSpawnNpc Then Exit Sub

        i = Buffer.ReadInteger

        With MapNpc(i)
            .Num = Buffer.ReadInteger
            .X = Buffer.ReadInteger
            .Y = Buffer.ReadInteger
            .Dir = Buffer.ReadInteger

            For i = 1 To Vitals.Count - 1
                .Vital(i) = Buffer.ReadInteger
            Next
            ' Client use only
            .XOffset = 0
            .YOffset = 0
            .Moving = 0
        End With

        Buffer = Nothing
    End Sub

    Sub Packet_NpcMove(Data() As Byte)
        Dim MapNpcNum As Integer, Movement As Integer
        Dim X As Integer, Y As Integer, Dir As Integer
        Dim Buffer As New ByteBuffer

        Buffer.WriteBytes(Data)

        If Buffer.ReadInteger <> ServerPackets.SNpcMove Then Exit Sub

        MapNpcNum = Buffer.ReadInteger
        X = Buffer.ReadInteger
        Y = Buffer.ReadInteger
        Dir = Buffer.ReadInteger
        Movement = Buffer.ReadInteger

        With MapNpc(MapNpcNum)
            .X = X
            .Y = Y
            .Dir = Dir
            .XOffset = 0
            .YOffset = 0
            .Moving = Movement

            Select Case .Dir
                Case Direction.Up
                    .YOffset = PIC_Y
                Case Direction.Down
                    .YOffset = PIC_Y * -1
                Case Direction.Left
                    .XOffset = PIC_X
                Case Direction.Right
                    .XOffset = PIC_X * -1
            End Select
        End With

        Buffer = Nothing
    End Sub

    Sub Packet_NpcDir(Data() As Byte)
        Dim Dir As Integer, i As Integer
        Dim Buffer As New ByteBuffer

        Buffer.WriteBytes(Data)

        If Buffer.ReadInteger <> ServerPackets.SNpcDir Then Exit Sub

        i = Buffer.ReadInteger
        Dir = Buffer.ReadInteger

        With MapNpc(i)
            .Dir = Dir
            .XOffset = 0
            .YOffset = 0
            .Moving = 0
        End With

        Buffer = Nothing
    End Sub

    Sub Packet_NpcAttack(Data() As Byte)
        Dim i As Integer
        Dim Buffer As New ByteBuffer

        Buffer.WriteBytes(Data)

        If Buffer.ReadInteger <> ServerPackets.SNpcAttack Then Exit Sub

        i = Buffer.ReadInteger

        ' Set npc to attacking
        MapNpc(i).Attacking = 1
        MapNpc(i).AttackTimer = GetTimeMs()

        Buffer = Nothing
    End Sub

    Sub Packet_UpdateNPC(data() As Byte)
        Dim i As Integer, x As Integer
        Dim Buffer As New ByteBuffer

        Buffer.WriteBytes(data)

        If Buffer.ReadInteger <> ServerPackets.SUpdateNpc Then Exit Sub

        i = Buffer.ReadInteger

        ' Update the Npc
        Npc(i).Animation = Buffer.ReadInteger()
        Npc(i).AttackSay = Trim(Buffer.ReadString())
        Npc(i).Behaviour = Buffer.ReadInteger()
        ReDim Npc(i).DropChance(5)
        ReDim Npc(i).DropItem(5)
        ReDim Npc(i).DropItemValue(5)
        For x = 1 To 5
            Npc(i).DropChance(x) = Buffer.ReadInteger()
            Npc(i).DropItem(x) = Buffer.ReadInteger()
            Npc(i).DropItemValue(x) = Buffer.ReadInteger()
        Next

        Npc(i).Exp = Buffer.ReadInteger()
        Npc(i).Faction = Buffer.ReadInteger()
        Npc(i).Hp = Buffer.ReadInteger()
        Npc(i).Name = Trim(Buffer.ReadString())
        Npc(i).Range = Buffer.ReadInteger()
        Npc(i).SpawnTime = Buffer.ReadInteger()
        Npc(i).SpawnSecs = Buffer.ReadInteger()
        Npc(i).Sprite = Buffer.ReadInteger()

        For i = 0 To Stats.Count - 1
            Npc(i).Stat(i) = Buffer.ReadInteger()
        Next

        Npc(i).QuestNum = Buffer.ReadInteger()

        For x = 1 To MAX_NPC_SKILLS
            Npc(i).Skill(x) = Buffer.ReadInteger()
        Next

        Npc(i).Level = Buffer.ReadInteger()
        Npc(i).Damage = Buffer.ReadInteger()

        If Npc(i).AttackSay Is Nothing Then Npc(i).AttackSay = ""
        If Npc(i).Name Is Nothing Then Npc(i).Name = ""

        Buffer = Nothing
    End Sub

    Sub Packet_NPCVitals(Data() As Byte)
        Dim MapNpcNum As Integer
        Dim Buffer As New ByteBuffer

        Buffer.WriteBytes(Data)

        If Buffer.ReadInteger <> ServerPackets.SMapNpcVitals Then Exit Sub

        MapNpcNum = Buffer.ReadInteger
        For i = 1 To Vitals.Count - 1
            MapNpc(MapNpcNum).Vital(i) = Buffer.ReadInteger
        Next

        Buffer = Nothing
    End Sub


#End Region

#Region "Outgoing Packets"
    Sub SendRequestNPCS()
        Dim Buffer As New ByteBuffer

        Buffer.WriteInteger(ClientPackets.CRequestNPCS)

        SendData(Buffer.ToArray())
        Buffer = Nothing
    End Sub


#End Region

#Region "Movement"
    Sub ProcessNpcMovement(ByVal MapNpcNum As Integer)

        ' Check if NPC is walking, and if so process moving them over
        If MapNpc(MapNpcNum).Moving = MovementType.Walking Then

            Select Case MapNpc(MapNpcNum).Dir
                Case Direction.Up
                    MapNpc(MapNpcNum).YOffset = MapNpc(MapNpcNum).YOffset - ((ElapsedTime / 1000) * (WALK_SPEED * SIZE_X))
                    If MapNpc(MapNpcNum).YOffset < 0 Then MapNpc(MapNpcNum).YOffset = 0

                Case Direction.Down
                    MapNpc(MapNpcNum).YOffset = MapNpc(MapNpcNum).YOffset + ((ElapsedTime / 1000) * (WALK_SPEED * SIZE_X))
                    If MapNpc(MapNpcNum).YOffset > 0 Then MapNpc(MapNpcNum).YOffset = 0

                Case Direction.Left
                    MapNpc(MapNpcNum).XOffset = MapNpc(MapNpcNum).XOffset - ((ElapsedTime / 1000) * (WALK_SPEED * SIZE_X))
                    If MapNpc(MapNpcNum).XOffset < 0 Then MapNpc(MapNpcNum).XOffset = 0

                Case Direction.Right
                    MapNpc(MapNpcNum).XOffset = MapNpc(MapNpcNum).XOffset + ((ElapsedTime / 1000) * (WALK_SPEED * SIZE_X))
                    If MapNpc(MapNpcNum).XOffset > 0 Then MapNpc(MapNpcNum).XOffset = 0

            End Select

            ' Check if completed walking over to the next tile
            If MapNpc(MapNpcNum).Moving > 0 Then
                If MapNpc(MapNpcNum).Dir = Direction.Right Or MapNpc(MapNpcNum).Dir = Direction.Down Then
                    If (MapNpc(MapNpcNum).XOffset >= 0) And (MapNpc(MapNpcNum).YOffset >= 0) Then
                        MapNpc(MapNpcNum).Moving = 0
                        If MapNpc(MapNpcNum).Steps = 1 Then
                            MapNpc(MapNpcNum).Steps = 3
                        Else
                            MapNpc(MapNpcNum).Steps = 1
                        End If
                    End If
                Else
                    If (MapNpc(MapNpcNum).XOffset <= 0) And (MapNpc(MapNpcNum).YOffset <= 0) Then
                        MapNpc(MapNpcNum).Moving = 0
                        If MapNpc(MapNpcNum).Steps = 1 Then
                            MapNpc(MapNpcNum).Steps = 3
                        Else
                            MapNpc(MapNpcNum).Steps = 1
                        End If
                    End If
                End If
            End If
        End If
    End Sub
#End Region

#Region "Drawing"
    Public Sub DrawNpc(MapNpcNum As Integer)
        Dim anim As Byte
        Dim X As Integer
        Dim Y As Integer
        Dim Sprite As Integer, spriteleft As Integer
        Dim destrec As Rectangle
        Dim srcrec As Rectangle
        Dim attackspeed As Integer

        If MapNpc(MapNpcNum).Num = 0 Then Exit Sub ' no npc set

        If MapNpc(MapNpcNum).X < TileView.Left Or MapNpc(MapNpcNum).X > TileView.Right Then Exit Sub
        If MapNpc(MapNpcNum).Y < TileView.Top Or MapNpc(MapNpcNum).Y > TileView.Bottom Then Exit Sub

        Sprite = Npc(MapNpc(MapNpcNum).Num).Sprite

        If Sprite < 1 Or Sprite > NumCharacters Then Exit Sub

        attackspeed = 1000

        ' Reset frame
        anim = 0

        ' Check for attacking animation
        If MapNpc(MapNpcNum).AttackTimer + (attackspeed / 2) > GetTimeMs() Then
            If MapNpc(MapNpcNum).Attacking = 1 Then
                anim = 3
            End If
        Else
            ' If not attacking, walk normally
            Select Case MapNpc(MapNpcNum).Dir
                Case Direction.Up
                    If (MapNpc(MapNpcNum).YOffset > 8) Then anim = MapNpc(MapNpcNum).Steps
                Case Direction.Down
                    If (MapNpc(MapNpcNum).YOffset < -8) Then anim = MapNpc(MapNpcNum).Steps
                Case Direction.Left
                    If (MapNpc(MapNpcNum).XOffset > 8) Then anim = MapNpc(MapNpcNum).Steps
                Case Direction.Right
                    If (MapNpc(MapNpcNum).XOffset < -8) Then anim = MapNpc(MapNpcNum).Steps
            End Select
        End If

        ' Check to see if we want to stop making him attack
        With MapNpc(MapNpcNum)
            If .AttackTimer + attackspeed < GetTimeMs() Then
                .Attacking = 0
                .AttackTimer = 0
            End If
        End With

        ' Set the left
        Select Case MapNpc(MapNpcNum).Dir
            Case Direction.Up
                spriteleft = 3
            Case Direction.Right
                spriteleft = 2
            Case Direction.Down
                spriteleft = 0
            Case Direction.Left
                spriteleft = 1
        End Select

        srcrec = New Rectangle((anim) * (CharacterGFXInfo(Sprite).Width / 4), spriteleft * (CharacterGFXInfo(Sprite).Height / 4), (CharacterGFXInfo(Sprite).Width / 4), (CharacterGFXInfo(Sprite).Height / 4))

        ' Calculate the X
        X = MapNpc(MapNpcNum).X * PIC_X + MapNpc(MapNpcNum).XOffset - ((CharacterGFXInfo(Sprite).Width / 4 - 32) / 2)

        ' Is the player's height more than 32..?
        If (CharacterGFXInfo(Sprite).Height / 4) > 32 Then
            ' Create a 32 pixel offset for larger sprites
            Y = MapNpc(MapNpcNum).Y * PIC_Y + MapNpc(MapNpcNum).YOffset - ((CharacterGFXInfo(Sprite).Height / 4) - 32)
        Else
            ' Proceed as normal
            Y = MapNpc(MapNpcNum).Y * PIC_Y + MapNpc(MapNpcNum).YOffset
        End If

        destrec = New Rectangle(X, Y, CharacterGFXInfo(Sprite).Width / 4, CharacterGFXInfo(Sprite).Height / 4)

        DrawCharacter(Sprite, X, Y, srcrec)

        If Npc(MapNpc(MapNpcNum).Num).Behaviour = NpcBehavior.Quest Then
            If CanStartQuest(Npc(MapNpc(MapNpcNum).Num).QuestNum) Then
                If Player(MyIndex).PlayerQuest(Npc(MapNpc(MapNpcNum).Num).QuestNum).Status = QuestStatus.NotStarted Then
                    DrawEmotes(X, Y, 5)
                End If
            ElseIf Player(MyIndex).PlayerQuest(Npc(MapNpc(MapNpcNum).Num).QuestNum).Status = QuestStatus.Started Then
                DrawEmotes(X, Y, 9)
            End If
        End If

    End Sub
#End Region
End Module
