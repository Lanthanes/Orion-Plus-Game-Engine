Imports ASFW
Imports ASFW.IO

Module ServerNetworkSend
    Sub AlertMsg(ByVal Index As Integer, ByVal Msg As String)
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SAlertMsg)
        Buffer.WriteString(Msg)
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SAlertMsg", PACKET_LOG)
        TextAdd("Sent SMSG: SAlertMsg")

        Buffer.Dispose()
    End Sub

    Sub GlobalMsg(ByVal Msg As String)
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SGlobalMsg)
        'Buffer.WriteString(Msg)
        Buffer.WriteBytes(WriteUnicodeString(Msg))
        SendDataToAll(Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SGlobalMsg", PACKET_LOG)
        TextAdd("Sent SMSG: SGlobalMsg")

        Buffer.Dispose()
    End Sub

    Sub PlayerMsg(ByVal Index As Integer, ByVal Msg As String, ByVal Colour As Integer)
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SPlayerMsg)
        'Buffer.WriteString(Msg)
        Buffer.WriteBytes(WriteUnicodeString(Msg))
        Buffer.WriteInt32(Colour)

        Addlog("Sent SMSG: SPlayerMsg", PACKET_LOG)
        TextAdd("Sent SMSG: SPlayerMsg")

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendAnimations(ByVal Index As Integer)
        Dim i As Integer

        For i = 1 To MAX_ANIMATIONS

            If Len(Trim$(Animation(i).Name)) > 0 Then
                SendUpdateAnimationTo(Index, i)
            End If

        Next

    End Sub

    Sub SendNewCharClasses(ByVal Index As Integer)
        Dim i As Integer, n As Integer, q As Integer
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SNewCharClasses)
        Buffer.WriteInt32(Max_Classes)

        Addlog("Sent SMSG: SNewCharClasses", PACKET_LOG)
        TextAdd("Sent SMSG: SNewCharClasses")

        For i = 1 To Max_Classes
            Buffer.WriteString(GetClassName(i))
            Buffer.WriteString(Trim$(Classes(i).Desc))

            Buffer.WriteInt32(GetClassMaxVital(i, Vitals.HP))
            Buffer.WriteInt32(GetClassMaxVital(i, Vitals.MP))
            Buffer.WriteInt32(GetClassMaxVital(i, Vitals.SP))

            ' set sprite array size
            n = UBound(Classes(i).MaleSprite)
            ' send array size
            Buffer.WriteInt32(n)
            ' loop around sending each sprite
            For q = 0 To n
                Buffer.WriteInt32(Classes(i).MaleSprite(q))
            Next

            ' set sprite array size
            n = UBound(Classes(i).FemaleSprite)
            ' send array size
            Buffer.WriteInt32(n)
            ' loop around sending each sprite
            For q = 0 To n
                Buffer.WriteInt32(Classes(i).FemaleSprite(q))
            Next

            Buffer.WriteInt32(Classes(i).Stat(Stats.Strength))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Endurance))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Vitality))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Luck))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Intelligence))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Spirit))

            For q = 1 To 5
                Buffer.WriteInt32(Classes(i).StartItem(q))
                Buffer.WriteInt32(Classes(i).StartValue(q))
            Next

            Buffer.WriteInt32(Classes(i).StartMap)
            Buffer.WriteInt32(Classes(i).StartX)
            Buffer.WriteInt32(Classes(i).StartY)

            Buffer.WriteInt32(Classes(i).BaseExp)
        Next

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendCloseTrade(ByVal Index As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SCloseTrade)
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SCloseTrade", PACKET_LOG)
        TextAdd("Sent SMSG: SCloseTrade")

        Buffer.Dispose()
    End Sub

    Sub SendExp(ByVal Index As Integer)
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SPlayerEXP)
        Buffer.WriteInt32(Index)
        Buffer.WriteInt32(GetPlayerExp(Index))
        Buffer.WriteInt32(GetPlayerNextLevel(Index))

        Addlog("Sent SMSG: SPlayerEXP", PACKET_LOG)
        TextAdd("Sent SMSG: SPlayerEXP")

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendLoadCharOk(ByVal index As Integer)
        Dim Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SLoadCharOk)
        Buffer.WriteInt32(index)
        Socket.SendDataTo(index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SLoadCharOk", PACKET_LOG)
        TextAdd("Sent SMSG: SLoadCharOk")

        Buffer.Dispose()
    End Sub

    Sub SendEditorLoadOk(ByVal index As Integer)
        Dim Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SLoginOk)
        Buffer.WriteInt32(index)
        Socket.SendDataTo(index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SLoginOk", PACKET_LOG)
        TextAdd("Sent SMSG: SLoginOk")

        Buffer.Dispose()
    End Sub

    Sub SendInGame(ByVal Index As Integer)
        Dim Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SInGame)
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SInGame", PACKET_LOG)
        TextAdd("Sent SMSG: SInGame")

        Buffer.Dispose()
    End Sub

    Sub SendClasses(ByVal Index As Integer)
        Dim i As Integer, n As Integer, q As Integer
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SClassesData)
        Buffer.WriteInt32(Max_Classes)

        Addlog("Sent SMSG: SClassesData", PACKET_LOG)
        TextAdd("Sent SMSG: SClassesData")

        For i = 1 To Max_Classes
            Buffer.WriteString(Trim$(GetClassName(i)))
            Buffer.WriteString(Trim$(Classes(i).Desc))

            Buffer.WriteInt32(GetClassMaxVital(i, Vitals.HP))
            Buffer.WriteInt32(GetClassMaxVital(i, Vitals.MP))
            Buffer.WriteInt32(GetClassMaxVital(i, Vitals.SP))

            ' set sprite array size
            n = UBound(Classes(i).MaleSprite)

            ' send array size
            Buffer.WriteInt32(n)

            ' loop around sending each sprite
            For q = 0 To n
                Buffer.WriteInt32(Classes(i).MaleSprite(q))
            Next

            ' set sprite array size
            n = UBound(Classes(i).FemaleSprite)

            ' send array size
            Buffer.WriteInt32(n)

            ' loop around sending each sprite
            For q = 0 To n
                Buffer.WriteInt32(Classes(i).FemaleSprite(q))
            Next

            Buffer.WriteInt32(Classes(i).Stat(Stats.Strength))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Endurance))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Vitality))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Intelligence))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Luck))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Spirit))

            For q = 1 To 5
                Buffer.WriteInt32(Classes(i).StartItem(q))
                Buffer.WriteInt32(Classes(i).StartValue(q))
            Next

            Buffer.WriteInt32(Classes(i).StartMap)
            Buffer.WriteInt32(Classes(i).StartX)
            Buffer.WriteInt32(Classes(i).StartY)

            Buffer.WriteInt32(Classes(i).BaseExp)
        Next

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendClassesToAll()
        Dim i As Integer, n As Integer, q As Integer
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SClassesData)
        Buffer.WriteInt32(Max_Classes)

        Addlog("Sent SMSG: SClassesData To All", PACKET_LOG)
        TextAdd("Sent SMSG: SClassesData To All")

        For i = 1 To Max_Classes
            Buffer.WriteString(GetClassName(i))
            Buffer.WriteString(Trim$(Classes(i).Desc))

            Buffer.WriteInt32(GetClassMaxVital(i, Vitals.HP))
            Buffer.WriteInt32(GetClassMaxVital(i, Vitals.MP))
            Buffer.WriteInt32(GetClassMaxVital(i, Vitals.SP))

            ' set sprite array size
            n = UBound(Classes(i).MaleSprite)

            ' send array size
            Buffer.WriteInt32(n)

            ' loop around sending each sprite
            For q = 0 To n
                Buffer.WriteInt32(Classes(i).MaleSprite(q))
            Next

            ' set sprite array size
            n = UBound(Classes(i).FemaleSprite)

            ' send array size
            Buffer.WriteInt32(n)

            ' loop around sending each sprite
            For q = 0 To n
                Buffer.WriteInt32(Classes(i).FemaleSprite(q))
            Next

            Buffer.WriteInt32(Classes(i).Stat(Stats.Strength))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Endurance))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Vitality))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Intelligence))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Luck))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Spirit))

            For q = 1 To 5
                Buffer.WriteInt32(Classes(i).StartItem(q))
                Buffer.WriteInt32(Classes(i).StartValue(q))
            Next

            Buffer.WriteInt32(Classes(i).StartMap)
            Buffer.WriteInt32(Classes(i).StartX)
            Buffer.WriteInt32(Classes(i).StartY)

            Buffer.WriteInt32(Classes(i).BaseExp)
        Next

        SendDataToAll(Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendInventory(ByVal Index As Integer)
        Dim i As Integer, n As Integer
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SPlayerInv)

        Addlog("Sent SMSG: SPlayerInv", PACKET_LOG)
        TextAdd("Sent SMSG: SPlayerInv")

        For i = 1 To MAX_INV
            Buffer.WriteInt32(GetPlayerInvItemNum(Index, i))
            Buffer.WriteInt32(GetPlayerInvItemValue(Index, i))
            Buffer.WriteString(Player(Index).Character(TempPlayer(Index).CurChar).RandInv(i).Prefix)
            Buffer.WriteString(Player(Index).Character(TempPlayer(Index).CurChar).RandInv(i).Suffix)
            Buffer.WriteInt32(Player(Index).Character(TempPlayer(Index).CurChar).RandInv(i).Rarity)
            For n = 1 To Stats.Count - 1
                Buffer.WriteInt32(Player(Index).Character(TempPlayer(Index).CurChar).RandInv(i).Stat(n))
            Next
            Buffer.WriteInt32(Player(Index).Character(TempPlayer(Index).CurChar).RandInv(i).Damage)
            Buffer.WriteInt32(Player(Index).Character(TempPlayer(Index).CurChar).RandInv(i).Speed)
        Next

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendItems(ByVal Index As Integer)
        Dim i As Integer

        For i = 1 To MAX_ITEMS

            If Len(Trim$(Item(i).Name)) > 0 Then
                SendUpdateItemTo(Index, i)
            End If

        Next

    End Sub

    Sub SendUpdateItemTo(ByVal Index As Integer, ByVal itemNum As Integer)
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SUpdateItem)
        Buffer.WriteInt32(itemNum)
        Buffer.WriteInt32(Item(itemNum).AccessReq)

        Addlog("Sent SMSG: SUpdateItem", PACKET_LOG)
        TextAdd("Sent SMSG: SUpdateItem")

        For i = 0 To Stats.Count - 1
            Buffer.WriteInt32(Item(itemNum).Add_Stat(i))
        Next

        Buffer.WriteInt32(Item(itemNum).Animation)
        Buffer.WriteInt32(Item(itemNum).BindType)
        Buffer.WriteInt32(Item(itemNum).ClassReq)
        Buffer.WriteInt32(Item(itemNum).Data1)
        Buffer.WriteInt32(Item(itemNum).Data2)
        Buffer.WriteInt32(Item(itemNum).Data3)
        Buffer.WriteInt32(Item(itemNum).TwoHanded)
        Buffer.WriteInt32(Item(itemNum).LevelReq)
        Buffer.WriteInt32(Item(itemNum).Mastery)
        Buffer.WriteString(Trim$(Item(itemNum).Name))
        Buffer.WriteInt32(Item(itemNum).Paperdoll)
        Buffer.WriteInt32(Item(itemNum).Pic)
        Buffer.WriteInt32(Item(itemNum).Price)
        Buffer.WriteInt32(Item(itemNum).Rarity)
        Buffer.WriteInt32(Item(itemNum).Speed)

        Buffer.WriteInt32(Item(itemNum).Randomize)
        Buffer.WriteInt32(Item(itemNum).RandomMin)
        Buffer.WriteInt32(Item(itemNum).RandomMax)

        Buffer.WriteInt32(Item(itemNum).Stackable)
        Buffer.WriteString(Trim$(Item(itemNum).Description))

        For i = 0 To Stats.Count - 1
            Buffer.WriteInt32(Item(itemNum).Stat_Req(i))
        Next

        Buffer.WriteInt32(Item(itemNum).Type)
        Buffer.WriteInt32(Item(itemNum).SubType)

        Buffer.WriteInt32(Item(itemNum).ItemLevel)

        'Housing
        Buffer.WriteInt32(Item(itemNum).FurnitureWidth)
        Buffer.WriteInt32(Item(itemNum).FurnitureHeight)

        For i = 1 To 3
            For x = 1 To 3
                Buffer.WriteInt32(Item(itemNum).FurnitureBlocks(i, x))
                Buffer.WriteInt32(Item(itemNum).FurnitureFringe(i, x))
            Next
        Next

        Buffer.WriteInt32(Item(itemNum).KnockBack)
        Buffer.WriteInt32(Item(itemNum).KnockBackTiles)

        Buffer.WriteInt32(Item(itemNum).Projectile)
        Buffer.WriteInt32(Item(itemNum).Ammo)

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendUpdateItemToAll(ByVal itemNum As Integer)
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SUpdateItem)
        Buffer.WriteInt32(itemNum)
        Buffer.WriteInt32(Item(itemNum).AccessReq)

        Addlog("Sent SMSG: SUpdateItem To All", PACKET_LOG)
        TextAdd("Sent SMSG: SUpdateItem To All")

        For i = 0 To Stats.Count - 1
            Buffer.WriteInt32(Item(itemNum).Add_Stat(i))
        Next

        Buffer.WriteInt32(Item(itemNum).Animation)
        Buffer.WriteInt32(Item(itemNum).BindType)
        Buffer.WriteInt32(Item(itemNum).ClassReq)
        Buffer.WriteInt32(Item(itemNum).Data1)
        Buffer.WriteInt32(Item(itemNum).Data2)
        Buffer.WriteInt32(Item(itemNum).Data3)
        Buffer.WriteInt32(Item(itemNum).TwoHanded)
        Buffer.WriteInt32(Item(itemNum).LevelReq)
        Buffer.WriteInt32(Item(itemNum).Mastery)
        Buffer.WriteString(Trim$(Item(itemNum).Name))
        Buffer.WriteInt32(Item(itemNum).Paperdoll)
        Buffer.WriteInt32(Item(itemNum).Pic)
        Buffer.WriteInt32(Item(itemNum).Price)
        Buffer.WriteInt32(Item(itemNum).Rarity)
        Buffer.WriteInt32(Item(itemNum).Speed)

        Buffer.WriteInt32(Item(itemNum).Randomize)
        Buffer.WriteInt32(Item(itemNum).RandomMin)
        Buffer.WriteInt32(Item(itemNum).RandomMax)

        Buffer.WriteInt32(Item(itemNum).Stackable)
        Buffer.WriteString(Trim$(Item(itemNum).Description))

        For i = 0 To Stats.Count - 1
            Buffer.WriteInt32(Item(itemNum).Stat_Req(i))
        Next

        Buffer.WriteInt32(Item(itemNum).Type)
        Buffer.WriteInt32(Item(itemNum).SubType)

        Buffer.WriteInt32(Item(itemNum).ItemLevel)

        'Housing
        Buffer.WriteInt32(Item(itemNum).FurnitureWidth)
        Buffer.WriteInt32(Item(itemNum).FurnitureHeight)

        For i = 1 To 3
            For x = 1 To 3
                Buffer.WriteInt32(Item(itemNum).FurnitureBlocks(i, x))
                Buffer.WriteInt32(Item(itemNum).FurnitureFringe(i, x))
            Next
        Next

        Buffer.WriteInt32(Item(itemNum).KnockBack)
        Buffer.WriteInt32(Item(itemNum).KnockBackTiles)

        Buffer.WriteInt32(Item(itemNum).Projectile)
        Buffer.WriteInt32(Item(itemNum).Ammo)

        SendDataToAll(Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendLeftMap(ByVal Index As Integer)
        Dim Buffer As New ByteStream(4)
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SLeftMap)
        Buffer.WriteInt32(Index)
        SendDataToAllBut(Index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SLeftMap", PACKET_LOG)
        TextAdd("Sent SMSG: SLeftMap")

        Buffer.Dispose()
    End Sub

    Sub SendLeftGame(ByVal Index As Integer)
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SLeftGame)

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendMapEquipment(ByVal Index As Integer)
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SMapWornEq)
        Buffer.WriteInt32(Index)
        Buffer.WriteInt32(GetPlayerEquipment(Index, EquipmentType.Armor))
        Buffer.WriteInt32(GetPlayerEquipment(Index, EquipmentType.Weapon))
        Buffer.WriteInt32(GetPlayerEquipment(Index, EquipmentType.Helmet))
        Buffer.WriteInt32(GetPlayerEquipment(Index, EquipmentType.Shield))
        Buffer.WriteInt32(GetPlayerEquipment(Index, EquipmentType.Shoes))
        Buffer.WriteInt32(GetPlayerEquipment(Index, EquipmentType.Gloves))

        Addlog("Sent SMSG: SMapWornEq", PACKET_LOG)
        TextAdd("Sent SMSG: SMapWornEq")

        SendDataToMap(GetPlayerMap(Index), Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendMapEquipmentTo(ByVal PlayerNum As Integer, ByVal Index As Integer)
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SMapWornEq)
        Buffer.WriteInt32(PlayerNum)
        Buffer.WriteInt32(GetPlayerEquipment(PlayerNum, EquipmentType.Armor))
        Buffer.WriteInt32(GetPlayerEquipment(PlayerNum, EquipmentType.Weapon))
        Buffer.WriteInt32(GetPlayerEquipment(PlayerNum, EquipmentType.Helmet))
        Buffer.WriteInt32(GetPlayerEquipment(PlayerNum, EquipmentType.Shield))
        Buffer.WriteInt32(GetPlayerEquipment(Index, EquipmentType.Shoes))
        Buffer.WriteInt32(GetPlayerEquipment(Index, EquipmentType.Gloves))

        Addlog("Sent SMSG: SMapWornEq To", PACKET_LOG)
        TextAdd("Sent SMSG: SMapWornEq To")

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendNpcs(ByVal Index As Integer)
        Dim i As Integer

        For i = 1 To MAX_NPCS

            If Len(Trim$(Npc(i).Name)) > 0 Then
                SendUpdateNpcTo(Index, i)
            End If

        Next

    End Sub

    Sub SendUpdateNpcTo(ByVal Index As Integer, ByVal NpcNum As Integer)
        Dim Buffer As ByteStream, i As Integer
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SUpdateNpc)

        Addlog("Sent SMSG: SUpdateNpc", PACKET_LOG)
        TextAdd("Sent SMSG: SUpdateNpc")

        Buffer.WriteInt32(NpcNum)
        Buffer.WriteInt32(Npc(NpcNum).Animation)
        Buffer.WriteString(Npc(NpcNum).AttackSay)
        Buffer.WriteInt32(Npc(NpcNum).Behaviour)

        For i = 1 To 5
            Buffer.WriteInt32(Npc(NpcNum).DropChance(i))
            Buffer.WriteInt32(Npc(NpcNum).DropItem(i))
            Buffer.WriteInt32(Npc(NpcNum).DropItemValue(i))
        Next

        Buffer.WriteInt32(Npc(NpcNum).Exp)
        Buffer.WriteInt32(Npc(NpcNum).Faction)
        Buffer.WriteInt32(Npc(NpcNum).Hp)
        Buffer.WriteString(Npc(NpcNum).Name)
        Buffer.WriteInt32(Npc(NpcNum).Range)
        Buffer.WriteInt32(Npc(NpcNum).SpawnTime)
        Buffer.WriteInt32(Npc(NpcNum).SpawnSecs)
        Buffer.WriteInt32(Npc(NpcNum).Sprite)

        For i = 0 To Stats.Count - 1
            Buffer.WriteInt32(Npc(NpcNum).Stat(i))
        Next

        Buffer.WriteInt32(Npc(NpcNum).QuestNum)

        For i = 1 To MAX_NPC_SKILLS
            Buffer.WriteInt32(Npc(NpcNum).Skill(i))
        Next

        Buffer.WriteInt32(Npc(NpcNum).Level)
        Buffer.WriteInt32(Npc(NpcNum).Damage)

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendUpdateNpcToAll(ByVal NpcNum As Integer)
        Dim Buffer As ByteStream, i As Integer
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SUpdateNpc)

        Addlog("Sent SMSG: SUpdateNpc To All", PACKET_LOG)
        TextAdd("Sent SMSG: SUpdateNpc To All")

        Buffer.WriteInt32(NpcNum)
        Buffer.WriteInt32(Npc(NpcNum).Animation)
        Buffer.WriteString(Npc(NpcNum).AttackSay)
        Buffer.WriteInt32(Npc(NpcNum).Behaviour)

        For i = 1 To 5
            Buffer.WriteInt32(Npc(NpcNum).DropChance(i))
            Buffer.WriteInt32(Npc(NpcNum).DropItem(i))
            Buffer.WriteInt32(Npc(NpcNum).DropItemValue(i))
        Next

        Buffer.WriteInt32(Npc(NpcNum).Exp)
        Buffer.WriteInt32(Npc(NpcNum).Faction)
        Buffer.WriteInt32(Npc(NpcNum).Hp)
        Buffer.WriteString(Npc(NpcNum).Name)
        Buffer.WriteInt32(Npc(NpcNum).Range)
        Buffer.WriteInt32(Npc(NpcNum).SpawnTime)
        Buffer.WriteInt32(Npc(NpcNum).SpawnSecs)
        Buffer.WriteInt32(Npc(NpcNum).Sprite)

        For i = 0 To Stats.Count - 1
            Buffer.WriteInt32(Npc(NpcNum).Stat(i))
        Next

        Buffer.WriteInt32(Npc(NpcNum).QuestNum)

        For i = 1 To MAX_NPC_SKILLS
            Buffer.WriteInt32(Npc(NpcNum).Skill(i))
        Next

        Buffer.WriteInt32(Npc(NpcNum).Level)
        Buffer.WriteInt32(Npc(NpcNum).Damage)

        SendDataToAll(Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendResourceCacheTo(ByVal Index As Integer, ByVal Resource_num As Integer)
        Dim Buffer As ByteStream
        Dim i As Integer
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SResourceCache)
        Buffer.WriteInt32(ResourceCache(GetPlayerMap(Index)).Resource_Count)

        Addlog("Sent SMSG: SResourcesCahce", PACKET_LOG)
        TextAdd("Sent SMSG: SResourcesCache")

        If ResourceCache(GetPlayerMap(Index)).Resource_Count > 0 Then

            For i = 0 To ResourceCache(GetPlayerMap(Index)).Resource_Count
                Buffer.WriteInt32(ResourceCache(GetPlayerMap(Index)).ResourceData(i).ResourceState)
                Buffer.WriteInt32(ResourceCache(GetPlayerMap(Index)).ResourceData(i).X)
                Buffer.WriteInt32(ResourceCache(GetPlayerMap(Index)).ResourceData(i).Y)
            Next

        End If

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendResources(ByVal Index As Integer)
        Dim i As Integer

        For i = 1 To MAX_RESOURCES

            If Len(Trim$(Resource(i).Name)) > 0 Then
                SendUpdateResourceTo(Index, i)
            End If

        Next

    End Sub

    Sub SendUpdateResourceTo(ByVal Index As Integer, ByVal ResourceNum As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SUpdateResource)
        Buffer.WriteInt32(ResourceNum)
        Buffer.WriteInt32(Resource(ResourceNum).Animation)
        Buffer.WriteString(Resource(ResourceNum).EmptyMessage)
        Buffer.WriteInt32(Resource(ResourceNum).ExhaustedImage)
        Buffer.WriteInt32(Resource(ResourceNum).Health)
        Buffer.WriteInt32(Resource(ResourceNum).ExpReward)
        Buffer.WriteInt32(Resource(ResourceNum).ItemReward)
        Buffer.WriteString(Resource(ResourceNum).Name)
        Buffer.WriteInt32(Resource(ResourceNum).ResourceImage)
        Buffer.WriteInt32(Resource(ResourceNum).ResourceType)
        Buffer.WriteInt32(Resource(ResourceNum).RespawnTime)
        Buffer.WriteString(Resource(ResourceNum).SuccessMessage)
        Buffer.WriteInt32(Resource(ResourceNum).LvlRequired)
        Buffer.WriteInt32(Resource(ResourceNum).ToolRequired)
        Buffer.WriteInt32(Resource(ResourceNum).Walkthrough)

        Addlog("Sent SMSG: SUpdateResources", PACKET_LOG)
        TextAdd("Sent SMSG: SUpdateResources")

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendShops(ByVal Index As Integer)
        Dim i As Integer

        For i = 1 To MAX_SHOPS

            If Len(Trim$(Shop(i).Name)) > 0 Then
                SendUpdateShopTo(Index, i)
            End If

        Next

    End Sub

    Sub SendUpdateShopTo(ByVal Index As Integer, ByVal shopNum As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SUpdateShop)
        Buffer.WriteInt32(shopNum)
        Buffer.WriteInt32(Shop(shopNum).BuyRate)
        Buffer.WriteString(Trim(Shop(shopNum).Name))
        Buffer.WriteInt32(Shop(shopNum).Face)

        Addlog("Sent SMSG: SUpdateShop", PACKET_LOG)
        TextAdd("Sent SMSG: SUpdateShop")

        For i = 0 To MAX_TRADES
            Buffer.WriteInt32(Shop(shopNum).TradeItem(i).CostItem)
            Buffer.WriteInt32(Shop(shopNum).TradeItem(i).CostValue)
            Buffer.WriteInt32(Shop(shopNum).TradeItem(i).Item)
            Buffer.WriteInt32(Shop(shopNum).TradeItem(i).ItemValue)
        Next

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendUpdateShopToAll(ByVal shopNum As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SUpdateShop)
        Buffer.WriteInt32(shopNum)
        Buffer.WriteInt32(Shop(shopNum).BuyRate)
        Buffer.WriteString(Shop(shopNum).Name)
        Buffer.WriteInt32(Shop(shopNum).Face)

        Addlog("Sent SMSG: SUpdateShop To All", PACKET_LOG)
        TextAdd("Sent SMSG: SUpdateShop To All")

        For i = 0 To MAX_TRADES
            Buffer.WriteInt32(Shop(shopNum).TradeItem(i).CostItem)
            Buffer.WriteInt32(Shop(shopNum).TradeItem(i).CostValue)
            Buffer.WriteInt32(Shop(shopNum).TradeItem(i).Item)
            Buffer.WriteInt32(Shop(shopNum).TradeItem(i).ItemValue)
        Next

        SendDataToAll(Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendSkills(ByVal Index As Integer)
        Dim i As Integer

        For i = 1 To MAX_SKILLS

            If Len(Trim$(Skill(i).Name)) > 0 Then
                SendUpdateSkillTo(Index, i)
            End If

        Next

    End Sub

    Sub SendUpdateSkillTo(ByVal Index As Integer, ByVal skillnum As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SUpdateSkill)
        Buffer.WriteInt32(skillnum)
        Buffer.WriteInt32(Skill(skillnum).AccessReq)
        Buffer.WriteInt32(Skill(skillnum).AoE)
        Buffer.WriteInt32(Skill(skillnum).CastAnim)
        Buffer.WriteInt32(Skill(skillnum).CastTime)
        Buffer.WriteInt32(Skill(skillnum).CdTime)
        Buffer.WriteInt32(Skill(skillnum).ClassReq)
        Buffer.WriteInt32(Skill(skillnum).Dir)
        Buffer.WriteInt32(Skill(skillnum).Duration)
        Buffer.WriteInt32(Skill(skillnum).Icon)
        Buffer.WriteInt32(Skill(skillnum).Interval)
        Buffer.WriteInt32(Skill(skillnum).IsAoE)
        Buffer.WriteInt32(Skill(skillnum).LevelReq)
        Buffer.WriteInt32(Skill(skillnum).Map)
        Buffer.WriteInt32(Skill(skillnum).MpCost)
        Buffer.WriteString(Trim(Skill(skillnum).Name))
        Buffer.WriteInt32(Skill(skillnum).Range)
        Buffer.WriteInt32(Skill(skillnum).SkillAnim)
        Buffer.WriteInt32(Skill(skillnum).StunDuration)
        Buffer.WriteInt32(Skill(skillnum).Type)
        Buffer.WriteInt32(Skill(skillnum).Vital)
        Buffer.WriteInt32(Skill(skillnum).X)
        Buffer.WriteInt32(Skill(skillnum).Y)

        Addlog("Sent SMSG: SUpdateSkill", PACKET_LOG)
        TextAdd("Sent SMSG: SUpdateSkill")

        'projectiles
        Buffer.WriteInt32(Skill(skillnum).IsProjectile)
        Buffer.WriteInt32(Skill(skillnum).Projectile)

        Buffer.WriteInt32(Skill(skillnum).KnockBack)
        Buffer.WriteInt32(Skill(skillnum).KnockBackTiles)

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendUpdateSkillToAll(ByVal skillnum As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SUpdateSkill)
        Buffer.WriteInt32(skillnum)
        Buffer.WriteInt32(Skill(skillnum).AccessReq)
        Buffer.WriteInt32(Skill(skillnum).AoE)
        Buffer.WriteInt32(Skill(skillnum).CastAnim)
        Buffer.WriteInt32(Skill(skillnum).CastTime)
        Buffer.WriteInt32(Skill(skillnum).CdTime)
        Buffer.WriteInt32(Skill(skillnum).ClassReq)
        Buffer.WriteInt32(Skill(skillnum).Dir)
        Buffer.WriteInt32(Skill(skillnum).Duration)
        Buffer.WriteInt32(Skill(skillnum).Icon)
        Buffer.WriteInt32(Skill(skillnum).Interval)
        Buffer.WriteInt32(Skill(skillnum).IsAoE)
        Buffer.WriteInt32(Skill(skillnum).LevelReq)
        Buffer.WriteInt32(Skill(skillnum).Map)
        Buffer.WriteInt32(Skill(skillnum).MpCost)
        Buffer.WriteString(Skill(skillnum).Name)
        Buffer.WriteInt32(Skill(skillnum).Range)
        Buffer.WriteInt32(Skill(skillnum).SkillAnim)
        Buffer.WriteInt32(Skill(skillnum).StunDuration)
        Buffer.WriteInt32(Skill(skillnum).Type)
        Buffer.WriteInt32(Skill(skillnum).Vital)
        Buffer.WriteInt32(Skill(skillnum).X)
        Buffer.WriteInt32(Skill(skillnum).Y)

        Addlog("Sent SMSG: SUpdateSkill To All", PACKET_LOG)
        TextAdd("Sent SMSG: SUpdateSkill To All")

        'projectiles
        Buffer.WriteInt32(Skill(skillnum).IsProjectile)
        Buffer.WriteInt32(Skill(skillnum).Projectile)

        Buffer.WriteInt32(Skill(skillnum).KnockBack)
        Buffer.WriteInt32(Skill(skillnum).KnockBackTiles)

        SendDataToAll(Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendStats(ByVal Index As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SPlayerStats)
        Buffer.WriteInt32(Index)
        Buffer.WriteInt32(GetPlayerStat(Index, Stats.Strength))
        Buffer.WriteInt32(GetPlayerStat(Index, Stats.Endurance))
        Buffer.WriteInt32(GetPlayerStat(Index, Stats.Vitality))
        Buffer.WriteInt32(GetPlayerStat(Index, Stats.Luck))
        Buffer.WriteInt32(GetPlayerStat(Index, Stats.Intelligence))
        Buffer.WriteInt32(GetPlayerStat(Index, Stats.Spirit))
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SPlayerStats", PACKET_LOG)
        TextAdd("Sent SMSG: SPlayerStats")

        Buffer.Dispose()
    End Sub

    Sub SendUpdateAnimationTo(ByVal Index As Integer, ByVal AnimationNum As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SUpdateAnimation)
        Buffer.WriteInt32(AnimationNum)

        Addlog("Sent SMSG: SUpdateAnimation", PACKET_LOG)
        TextAdd("Sent SMSG: SUpdateAnimation")

        For i = 0 To UBound(Animation(AnimationNum).Frames)
            Buffer.WriteInt32(Animation(AnimationNum).Frames(i))
        Next

        For i = 0 To UBound(Animation(AnimationNum).LoopCount)
            Buffer.WriteInt32(Animation(AnimationNum).LoopCount(i))
        Next

        For i = 0 To UBound(Animation(AnimationNum).LoopTime)
            Buffer.WriteInt32(Animation(AnimationNum).LoopTime(i))
        Next

        Buffer.WriteString(Animation(AnimationNum).Name)
        Buffer.WriteString(Animation(AnimationNum).Sound)

        For i = 0 To UBound(Animation(AnimationNum).Sprite)
            Buffer.WriteInt32(Animation(AnimationNum).Sprite(i))
        Next

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendUpdateAnimationToAll(ByVal AnimationNum As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SUpdateAnimation)
        Buffer.WriteInt32(AnimationNum)

        Addlog("Sent SMSG: SUpdateAnimation To All", PACKET_LOG)
        TextAdd("Sent SMSG: SUpdateAnimation To All")

        For i = 0 To UBound(Animation(AnimationNum).Frames)
            Buffer.WriteInt32(Animation(AnimationNum).Frames(i))
        Next

        For i = 0 To UBound(Animation(AnimationNum).LoopCount)
            Buffer.WriteInt32(Animation(AnimationNum).LoopCount(i))
        Next

        For i = 0 To UBound(Animation(AnimationNum).LoopTime)
            Buffer.WriteInt32(Animation(AnimationNum).LoopTime(i))
        Next

        Buffer.WriteString(Animation(AnimationNum).Name)
        Buffer.WriteString(Animation(AnimationNum).Sound)

        For i = 0 To UBound(Animation(AnimationNum).Sprite)
            Buffer.WriteInt32(Animation(AnimationNum).Sprite(i))
        Next

        SendDataToAll(Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendVitals(ByVal Index As Integer)
        For i = 1 To Vitals.Count - 1
            SendVital(Index, i)
        Next
    End Sub

    Sub SendVital(ByVal Index As Integer, ByVal Vital As Vitals)
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)

        ' Get our packet type.
        Select Case Vital
            Case Vitals.HP
                Buffer.WriteInt32(ServerPackets.SPlayerHp)
                Addlog("Sent SMSG: SPlayerHp", PACKET_LOG)
                TextAdd("Sent SMSG: SPlayerHp")
            Case Vitals.MP
                Buffer.WriteInt32(ServerPackets.SPlayerMp)
                Addlog("Sent SMSG: SPlayerMp", PACKET_LOG)
                TextAdd("Sent SMSG: SPlayerMp")
            Case Vitals.SP
                Buffer.WriteInt32(ServerPackets.SPlayerSp)
                Addlog("Sent SMSG: SPlayerSp", PACKET_LOG)
                TextAdd("Sent SMSG: SPlayerSp")
        End Select

        ' Set and send related data.
        Buffer.WriteInt32(GetPlayerMaxVital(Index, Vital))
        Buffer.WriteInt32(GetPlayerVital(Index, Vital))
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendWelcome(ByVal Index As Integer)

        ' Send them MOTD
        If Len(Options.Motd) > 0 Then
            PlayerMsg(Index, Options.Motd, ColorType.BrightCyan)
        End If

        ' Send whos online
        SendWhosOnline(Index)
    End Sub

    Sub SendWhosOnline(ByVal Index As Integer)
        Dim s As String
        Dim n As Integer
        Dim i As Integer
        s = ""
        For i = 1 To GetPlayersOnline()

            If IsPlaying(i) Then
                If i <> Index Then
                    s = s & GetPlayerName(i) & ", "
                    n = n + 1
                End If
            End If

        Next

        If n = 0 Then
            s = "There are no other players online."
        Else
            s = Mid$(s, 1, Len(s) - 2)
            s = "There are " & n & " other players online: " & s & "."
        End If

        PlayerMsg(Index, s, ColorType.White)
    End Sub

    Sub SendWornEquipment(ByVal Index As Integer)
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SPlayerWornEq)

        Addlog("Sent SMSG: SPlayerWornEq", PACKET_LOG)
        TextAdd("Sent SMSG: SPlayerWornEq")

        For i = 1 To EquipmentType.Count - 1
            Buffer.WriteInt32(GetPlayerEquipment(Index, i))
        Next

        For i = 1 To EquipmentType.Count - 1
            Buffer.WriteString(Player(Index).Character(TempPlayer(Index).CurChar).RandEquip(i).Prefix)
            Buffer.WriteString(Player(Index).Character(TempPlayer(Index).CurChar).RandEquip(i).Suffix)
            Buffer.WriteInt32(Player(Index).Character(TempPlayer(Index).CurChar).RandEquip(i).Damage)
            Buffer.WriteInt32(Player(Index).Character(TempPlayer(Index).CurChar).RandEquip(i).Speed)
            Buffer.WriteInt32(Player(Index).Character(TempPlayer(Index).CurChar).RandEquip(i).Rarity)
            For n = 1 To Stats.Count - 1
                Buffer.WriteInt32(Player(Index).Character(TempPlayer(Index).CurChar).RandEquip(i).Stat(n))
            Next
        Next

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendMapData(ByVal Index As Integer, ByVal MapNum As Integer, ByVal SendMap As Boolean)
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)
        Dim data() As Byte

        If SendMap Then
            Buffer.WriteInt32(1)
            Buffer.WriteInt32(MapNum)
            Buffer.WriteString(Map(MapNum).Name)
            Buffer.WriteString(Map(MapNum).Music)
            Buffer.WriteInt32(Map(MapNum).Revision)
            Buffer.WriteInt32(Map(MapNum).Moral)
            Buffer.WriteInt32(Map(MapNum).Tileset)
            Buffer.WriteInt32(Map(MapNum).Up)
            Buffer.WriteInt32(Map(MapNum).Down)
            Buffer.WriteInt32(Map(MapNum).Left)
            Buffer.WriteInt32(Map(MapNum).Right)
            Buffer.WriteInt32(Map(MapNum).BootMap)
            Buffer.WriteInt32(Map(MapNum).BootX)
            Buffer.WriteInt32(Map(MapNum).BootY)
            Buffer.WriteInt32(Map(MapNum).MaxX)
            Buffer.WriteInt32(Map(MapNum).MaxY)
            Buffer.WriteInt32(Map(MapNum).WeatherType)
            Buffer.WriteInt32(Map(MapNum).FogIndex)
            Buffer.WriteInt32(Map(MapNum).WeatherIntensity)
            Buffer.WriteInt32(Map(MapNum).FogAlpha)
            Buffer.WriteInt32(Map(MapNum).FogSpeed)
            Buffer.WriteInt32(Map(MapNum).HasMapTint)
            Buffer.WriteInt32(Map(MapNum).MapTintR)
            Buffer.WriteInt32(Map(MapNum).MapTintG)
            Buffer.WriteInt32(Map(MapNum).MapTintB)
            Buffer.WriteInt32(Map(MapNum).MapTintA)
            Buffer.WriteInt32(Map(MapNum).Instanced)
            Buffer.WriteInt32(Map(MapNum).Panorama)
            Buffer.WriteInt32(Map(MapNum).Parallax)

            For i = 1 To MAX_MAP_NPCS
                Buffer.WriteInt32(Map(MapNum).Npc(i))
            Next

            For x = 0 To Map(MapNum).MaxX
                For y = 0 To Map(MapNum).MaxY
                    Buffer.WriteInt32(Map(MapNum).Tile(x, y).Data1)
                    Buffer.WriteInt32(Map(MapNum).Tile(x, y).Data2)
                    Buffer.WriteInt32(Map(MapNum).Tile(x, y).Data3)
                    Buffer.WriteInt32(Map(MapNum).Tile(x, y).DirBlock)
                    For i = 0 To MapLayer.Count - 1
                        Buffer.WriteInt32(Map(MapNum).Tile(x, y).Layer(i).Tileset)
                        Buffer.WriteInt32(Map(MapNum).Tile(x, y).Layer(i).X)
                        Buffer.WriteInt32(Map(MapNum).Tile(x, y).Layer(i).Y)
                        Buffer.WriteInt32(Map(MapNum).Tile(x, y).Layer(i).AutoTile)
                    Next
                    Buffer.WriteInt32(Map(MapNum).Tile(x, y).Type)

                Next
            Next

            'Event Data
            Buffer.WriteInt32(Map(MapNum).EventCount)
            If Map(MapNum).EventCount > 0 Then
                For i = 1 To Map(MapNum).EventCount
                    With Map(MapNum).Events(i)
                        Buffer.WriteString(Trim$(.Name))
                        Buffer.WriteInt32(.Globals)
                        Buffer.WriteInt32(.X)
                        Buffer.WriteInt32(.Y)
                        Buffer.WriteInt32(.PageCount)
                    End With
                    If Map(MapNum).Events(i).PageCount > 0 Then
                        For X = 1 To Map(MapNum).Events(i).PageCount
                            With Map(MapNum).Events(i).Pages(X)
                                Buffer.WriteInt32(.chkVariable)
                                Buffer.WriteInt32(.VariableIndex)
                                Buffer.WriteInt32(.VariableCondition)
                                Buffer.WriteInt32(.VariableCompare)
                                Buffer.WriteInt32(.chkSwitch)
                                Buffer.WriteInt32(.SwitchIndex)
                                Buffer.WriteInt32(.SwitchCompare)
                                Buffer.WriteInt32(.chkHasItem)
                                Buffer.WriteInt32(.HasItemIndex)
                                Buffer.WriteInt32(.HasItemAmount)
                                Buffer.WriteInt32(.chkSelfSwitch)
                                Buffer.WriteInt32(.SelfSwitchIndex)
                                Buffer.WriteInt32(.SelfSwitchCompare)
                                Buffer.WriteInt32(.GraphicType)
                                Buffer.WriteInt32(.Graphic)
                                Buffer.WriteInt32(.GraphicX)
                                Buffer.WriteInt32(.GraphicY)
                                Buffer.WriteInt32(.GraphicX2)
                                Buffer.WriteInt32(.GraphicY2)
                                Buffer.WriteInt32(.MoveType)
                                Buffer.WriteInt32(.MoveSpeed)
                                Buffer.WriteInt32(.MoveFreq)
                                Buffer.WriteInt32(.MoveRouteCount)
                                Buffer.WriteInt32(.IgnoreMoveRoute)
                                Buffer.WriteInt32(.RepeatMoveRoute)
                                If .MoveRouteCount > 0 Then
                                    For Y = 1 To .MoveRouteCount
                                        Buffer.WriteInt32(.MoveRoute(Y).Index)
                                        Buffer.WriteInt32(.MoveRoute(Y).Data1)
                                        Buffer.WriteInt32(.MoveRoute(Y).Data2)
                                        Buffer.WriteInt32(.MoveRoute(Y).Data3)
                                        Buffer.WriteInt32(.MoveRoute(Y).Data4)
                                        Buffer.WriteInt32(.MoveRoute(Y).Data5)
                                        Buffer.WriteInt32(.MoveRoute(Y).Data6)
                                    Next
                                End If
                                Buffer.WriteInt32(.WalkAnim)
                                Buffer.WriteInt32(.DirFix)
                                Buffer.WriteInt32(.WalkThrough)
                                Buffer.WriteInt32(.ShowName)
                                Buffer.WriteInt32(.Trigger)
                                Buffer.WriteInt32(.CommandListCount)
                                Buffer.WriteInt32(.Position)
                                Buffer.WriteInt32(.QuestNum)

                                Buffer.WriteInt32(.chkPlayerGender)
                            End With
                            If Map(MapNum).Events(i).Pages(X).CommandListCount > 0 Then
                                For Y = 1 To Map(MapNum).Events(i).Pages(X).CommandListCount
                                    Buffer.WriteInt32(Map(MapNum).Events(i).Pages(X).CommandList(Y).CommandCount)
                                    Buffer.WriteInt32(Map(MapNum).Events(i).Pages(X).CommandList(Y).ParentList)
                                    If Map(MapNum).Events(i).Pages(X).CommandList(Y).CommandCount > 0 Then
                                        For z = 1 To Map(MapNum).Events(i).Pages(X).CommandList(Y).CommandCount
                                            With Map(MapNum).Events(i).Pages(X).CommandList(Y).Commands(z)
                                                Buffer.WriteInt32(.Index)
                                                Buffer.WriteString(Trim$(.Text1))
                                                Buffer.WriteString(Trim$(.Text2))
                                                Buffer.WriteString(Trim$(.Text3))
                                                Buffer.WriteString(Trim$(.Text4))
                                                Buffer.WriteString(Trim$(.Text5))
                                                Buffer.WriteInt32(.Data1)
                                                Buffer.WriteInt32(.Data2)
                                                Buffer.WriteInt32(.Data3)
                                                Buffer.WriteInt32(.Data4)
                                                Buffer.WriteInt32(.Data5)
                                                Buffer.WriteInt32(.Data6)
                                                Buffer.WriteInt32(.ConditionalBranch.CommandList)
                                                Buffer.WriteInt32(.ConditionalBranch.Condition)
                                                Buffer.WriteInt32(.ConditionalBranch.Data1)
                                                Buffer.WriteInt32(.ConditionalBranch.Data2)
                                                Buffer.WriteInt32(.ConditionalBranch.Data3)
                                                Buffer.WriteInt32(.ConditionalBranch.ElseCommandList)
                                                Buffer.WriteInt32(.MoveRouteCount)
                                                If .MoveRouteCount > 0 Then
                                                    For w = 1 To .MoveRouteCount
                                                        Buffer.WriteInt32(.MoveRoute(w).Index)
                                                        Buffer.WriteInt32(.MoveRoute(w).Data1)
                                                        Buffer.WriteInt32(.MoveRoute(w).Data2)
                                                        Buffer.WriteInt32(.MoveRoute(w).Data3)
                                                        Buffer.WriteInt32(.MoveRoute(w).Data4)
                                                        Buffer.WriteInt32(.MoveRoute(w).Data5)
                                                        Buffer.WriteInt32(.MoveRoute(w).Data6)
                                                    Next
                                                End If
                                            End With
                                        Next
                                    End If
                                Next
                            End If
                        Next
                    End If
                Next
            End If
            'End Event Data
        Else
            Buffer.WriteInt32(0)
        End If

        For i = 1 To MAX_MAP_ITEMS
            Buffer.WriteInt32(MapItem(MapNum, i).Num)
            Buffer.WriteInt32(MapItem(MapNum, i).Value)
            Buffer.WriteInt32(MapItem(MapNum, i).X)
            Buffer.WriteInt32(MapItem(MapNum, i).Y)
        Next

        For i = 1 To MAX_MAP_NPCS
            Buffer.WriteInt32(MapNpc(MapNum).Npc(i).Num)
            Buffer.WriteInt32(MapNpc(MapNum).Npc(i).X)
            Buffer.WriteInt32(MapNpc(MapNum).Npc(i).Y)
            Buffer.WriteInt32(MapNpc(MapNum).Npc(i).Dir)
            Buffer.WriteInt32(MapNpc(MapNum).Npc(i).Vital(Vitals.HP))
            Buffer.WriteInt32(MapNpc(MapNum).Npc(i).Vital(Vitals.MP))
        Next

        'send Resource cache
        If ResourceCache(GetPlayerMap(Index)).Resource_Count > 0 Then
            Buffer.WriteInt32(1)
            Buffer.WriteInt32(ResourceCache(GetPlayerMap(Index)).Resource_Count)

            For i = 0 To ResourceCache(GetPlayerMap(Index)).Resource_Count
                Buffer.WriteInt32(ResourceCache(GetPlayerMap(Index)).ResourceData(i).ResourceState)
                Buffer.WriteInt32(ResourceCache(GetPlayerMap(Index)).ResourceData(i).X)
                Buffer.WriteInt32(ResourceCache(GetPlayerMap(Index)).ResourceData(i).Y)
            Next
        Else
            Buffer.WriteInt32(0)
        End If

        data = Compression.CompressBytes(Buffer.ToArray)
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SMapData)
        Buffer.WriteBlock(data)
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SMapData", PACKET_LOG)
        TextAdd("Sent SMSG: SMapData")

        Buffer.Dispose()
    End Sub

    Sub SendJoinMap(ByVal Index As Integer)
        Dim i As Integer
        Dim data As Byte()
        ' Send all players on current map to index
        For i = 1 To GetPlayersOnline()
            If IsPlaying(i) Then
                If i <> Index Then
                    If GetPlayerMap(i) = GetPlayerMap(Index) Then
                        data = PlayerData(i)
                        Socket.SendDataTo(Index, data, data.Length)
                    End If
                End If
            End If
        Next

        ' Send index's player data to everyone on the map including himself
        data = PlayerData(Index)
        SendDataToMap(GetPlayerMap(Index), data, data.Length)
    End Sub

    Function PlayerData(ByVal Index As Integer) As Byte()
        Dim Buffer As ByteStream, i As Integer
        PlayerData = Nothing
        If Index > MAX_PLAYERS Then Exit Function
        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SPlayerData)
        Buffer.WriteInt32(Index)
        Buffer.WriteString(GetPlayerName(Index))
        Buffer.WriteInt32(GetPlayerClass(Index))
        Buffer.WriteInt32(GetPlayerLevel(Index))
        Buffer.WriteInt32(GetPlayerPOINTS(Index))
        Buffer.WriteInt32(GetPlayerSprite(Index))
        Buffer.WriteInt32(GetPlayerMap(Index))
        Buffer.WriteInt32(GetPlayerX(Index))
        Buffer.WriteInt32(GetPlayerY(Index))
        Buffer.WriteInt32(GetPlayerDir(Index))
        Buffer.WriteInt32(GetPlayerAccess(Index))
        Buffer.WriteInt32(GetPlayerPK(Index))

        Addlog("Sent SMSG: SPlayerData", PACKET_LOG)
        TextAdd("Sent SMSG: SPlayerData")

        For i = 1 To Stats.Count - 1
            Buffer.WriteInt32(GetPlayerStat(Index, i))
        Next

        Buffer.WriteInt32(Player(Index).Character(TempPlayer(Index).CurChar).InHouse)

        For i = 0 To ResourceSkills.Count - 1
            Buffer.WriteInt32(GetPlayerGatherSkillLvl(Index, i))
            Buffer.WriteInt32(GetPlayerGatherSkillExp(Index, i))
            Buffer.WriteInt32(GetPlayerGatherSkillMaxExp(Index, i))
        Next

        For i = 1 To MAX_RECIPE
            Buffer.WriteInt32(Player(Index).Character(TempPlayer(Index).CurChar).RecipeLearned(i))
        Next

        PlayerData = Buffer.ToArray()

        Buffer.Dispose()
    End Function

    Sub SendMapItemsTo(ByVal Index As Integer, ByVal MapNum As Integer)
        Dim i As Integer
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SMapItemData)

        Addlog("Sent SMSG: SMapItemData", PACKET_LOG)
        TextAdd("Sent SMSG: SMapItemData")

        For i = 1 To MAX_MAP_ITEMS
            Buffer.WriteInt32(MapItem(MapNum, i).Num)
            Buffer.WriteInt32(MapItem(MapNum, i).Value)
            Buffer.WriteInt32(MapItem(MapNum, i).X)
            Buffer.WriteInt32(MapItem(MapNum, i).Y)
        Next

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendMapNpcsTo(ByVal Index As Integer, ByVal MapNum As Integer)
        Dim i As Integer
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SMapNpcData)

        Addlog("Sent SMSG: SMapNpcData", PACKET_LOG)
        TextAdd("Sent SMSG: SMapNpcData")

        For i = 1 To MAX_MAP_NPCS
            Buffer.WriteInt32(MapNpc(MapNum).Npc(i).Num)
            Buffer.WriteInt32(MapNpc(MapNum).Npc(i).X)
            Buffer.WriteInt32(MapNpc(MapNum).Npc(i).Y)
            Buffer.WriteInt32(MapNpc(MapNum).Npc(i).Dir)
            Buffer.WriteInt32(MapNpc(MapNum).Npc(i).Vital(Vitals.HP))
            Buffer.WriteInt32(MapNpc(MapNum).Npc(i).Vital(Vitals.MP))
        Next

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendMapNpcTo(ByVal MapNum As Integer, ByVal MapNpcNum As Integer)
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SMapNpcUpdate)

        Addlog("Sent SMSG: SMapNpcUpdate", PACKET_LOG)
        TextAdd("Sent SMSG: SMapNpcUpdate")

        Buffer.WriteInt32(MapNpcNum)

        With MapNpc(MapNum).Npc(MapNpcNum)
            Buffer.WriteInt32(.Num)
            Buffer.WriteInt32(.X)
            Buffer.WriteInt32(.Y)
            Buffer.WriteInt32(.Dir)
            Buffer.WriteInt32(.Vital(Vitals.HP))
            Buffer.WriteInt32(.Vital(Vitals.MP))
        End With

        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendPlayerXY(ByVal Index As Integer)
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SPlayerXY)
        Buffer.WriteInt32(GetPlayerX(Index))
        Buffer.WriteInt32(GetPlayerY(Index))
        Buffer.WriteInt32(GetPlayerDir(Index))
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SPlayerXY", PACKET_LOG)
        TextAdd("Sent SMSG: SPlayerXY")

        Buffer.Dispose()
    End Sub

    Sub SendPlayerMove(ByVal Index As Integer, ByVal Movement As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SPlayerMove)
        Buffer.WriteInt32(Index)
        Buffer.WriteInt32(GetPlayerX(Index))
        Buffer.WriteInt32(GetPlayerY(Index))
        Buffer.WriteInt32(GetPlayerDir(Index))
        Buffer.WriteInt32(Movement)
        SendDataToMapBut(Index, GetPlayerMap(Index), Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SPlayerMove", PACKET_LOG)
        TextAdd("Sent SMSG: SPlayerMove")

        Buffer.Dispose()
    End Sub

    Sub SendDoorAnimation(ByVal MapNum As Integer, ByVal X As Integer, ByVal Y As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SDoorAnimation)
        Buffer.WriteInt32(X)
        Buffer.WriteInt32(Y)

        Addlog("Sent SMSG: SDoorAnimation", PACKET_LOG)
        TextAdd("Sent SMSG: SDoorAnimation")

        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendMapKey(ByVal Index As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal Value As Byte)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SMapKey)
        Buffer.WriteInt32(X)
        Buffer.WriteInt32(Y)
        Buffer.WriteInt32(Value)

        Addlog("Sent SMSG: SMapKey", PACKET_LOG)
        TextAdd("Sent SMSG: SMapKey")

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub MapMsg(ByVal MapNum As Integer, ByVal Msg As String, ByVal Color As Byte)
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SMapMsg)
        'Buffer.WriteString(Msg)
        Buffer.WriteBytes(WriteUnicodeString(Msg))

        Addlog("Sent SMSG: SMapMsg", PACKET_LOG)
        TextAdd("Sent SMSG: SMapMsg")

        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendActionMsg(ByVal MapNum As Integer, ByVal Message As String, ByVal Color As Integer, ByVal MsgType As Integer, ByVal X As Integer, ByVal Y As Integer, Optional ByVal PlayerOnlyNum As Integer = 0)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SActionMsg)
        'Buffer.WriteString(Message)
        Buffer.WriteBytes(WriteUnicodeString(Message))
        Buffer.WriteInt32(Color)
        Buffer.WriteInt32(MsgType)
        Buffer.WriteInt32(X)
        Buffer.WriteInt32(Y)

        Addlog("Sent SMSG: SActionMsg", PACKET_LOG)
        TextAdd("Sent SMSG: SActionMsg")

        If PlayerOnlyNum > 0 Then
            Socket.SendDataTo(PlayerOnlyNum, Buffer.Data, Buffer.Head)
        Else
            SendDataToMap(MapNum, Buffer.Data, Buffer.Head)
        End If

        Buffer.Dispose()
    End Sub

    Sub SayMsg_Map(ByVal MapNum As Integer, ByVal Index As Integer, ByVal Message As String, ByVal SayColour As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SSayMsg)
        Buffer.WriteString(GetPlayerName(Index))
        Buffer.WriteInt32(GetPlayerAccess(Index))
        Buffer.WriteInt32(GetPlayerPK(Index))
        'Buffer.WriteString(Message)
        Buffer.WriteBytes(WriteUnicodeString(Message))
        Buffer.WriteString("[Map] ")
        Buffer.WriteInt32(SayColour)

        Addlog("Sent SMSG: SSayMsg", PACKET_LOG)
        TextAdd("Sent SMSG: SSayMsg")

        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendPlayerData(ByVal Index As Integer)
        Dim data = PlayerData(Index)
        SendDataToMap(GetPlayerMap(Index), data, data.Length)
    End Sub

    Sub SendUpdateResourceToAll(ByVal ResourceNum As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SUpdateResource)
        Buffer.WriteInt32(ResourceNum)

        Addlog("Sent SMSG: SUpdateResource", PACKET_LOG)
        TextAdd("Sent SMSG: SUpdateResource")

        Buffer.WriteInt32(Resource(ResourceNum).Animation)
        Buffer.WriteString(Resource(ResourceNum).EmptyMessage)
        Buffer.WriteInt32(Resource(ResourceNum).ExhaustedImage)
        Buffer.WriteInt32(Resource(ResourceNum).Health)
        Buffer.WriteInt32(Resource(ResourceNum).ExpReward)
        Buffer.WriteInt32(Resource(ResourceNum).ItemReward)
        Buffer.WriteString(Resource(ResourceNum).Name)
        Buffer.WriteInt32(Resource(ResourceNum).ResourceImage)
        Buffer.WriteInt32(Resource(ResourceNum).ResourceType)
        Buffer.WriteInt32(Resource(ResourceNum).RespawnTime)
        Buffer.WriteString(Resource(ResourceNum).SuccessMessage)
        Buffer.WriteInt32(Resource(ResourceNum).LvlRequired)
        Buffer.WriteInt32(Resource(ResourceNum).ToolRequired)
        Buffer.WriteInt32(Resource(ResourceNum).Walkthrough)

        SendDataToAll(Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendMapNpcVitals(ByVal MapNum As Integer, ByVal MapNpcNum As Byte)
        Dim i As Integer
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SMapNpcVitals)
        Buffer.WriteInt32(MapNpcNum)

        Addlog("Sent SMSG: SMapNpcVitals", PACKET_LOG)
        TextAdd("Sent SMSG: SMapNpcVitals")

        For i = 1 To Vitals.Count - 1
            Buffer.WriteInt32(MapNpc(MapNum).Npc(MapNpcNum).Vital(i))
        Next

        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendMapKeyToMap(ByVal MapNum As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal Value As Byte)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SMapKey)
        Buffer.WriteInt32(X)
        Buffer.WriteInt32(Y)
        Buffer.WriteInt32(Value)
        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SMapKey", PACKET_LOG)
        TextAdd("Sent SMSG: SMapKey")

        Buffer.Dispose()
    End Sub

    Sub SendResourceCacheToMap(ByVal MapNum As Integer, ByVal Resource_num As Integer)
        Dim Buffer As ByteStream
        Dim i As Integer
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SResourceCache)
        Buffer.WriteInt32(ResourceCache(MapNum).Resource_Count)

        Addlog("Sent SMSG: SResourceCache", PACKET_LOG)
        TextAdd("Sent SMSG: SResourceCache")

        If ResourceCache(MapNum).Resource_Count > 0 Then

            For i = 0 To ResourceCache(MapNum).Resource_Count
                Buffer.WriteInt32(ResourceCache(MapNum).ResourceData(i).ResourceState)
                Buffer.WriteInt32(ResourceCache(MapNum).ResourceData(i).X)
                Buffer.WriteInt32(ResourceCache(MapNum).ResourceData(i).Y)
            Next

        End If

        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendGameData(ByVal index As Integer)
        Dim Buffer As ByteStream
        Dim i As Integer
        Dim data() As Byte
        Buffer = New ByteStream(4)

        Buffer.WriteBlock(ClassData)

        i = 0

        For x = 1 To MAX_ITEMS
            If Len(Trim$(Item(x).Name)) > 0 Then
                i = i + 1
            End If
        Next

        'Write Number of Items it is Sending and then The Item Data
        Buffer.WriteInt32(i)
        Buffer.WriteBlock(ItemsData)

        i = 0

        For x = 1 To MAX_ANIMATIONS
            If Len(Trim$(Animation(x).Name)) > 0 Then
                i = i + 1
            End If
        Next

        Buffer.WriteInt32(i)
        Buffer.WriteBlock(AnimationsData)

        i = 0

        For x = 1 To MAX_NPCS
            If Len(Trim$(Npc(x).Name)) > 0 Then
                i = i + 1
            End If
        Next

        Buffer.WriteInt32(i)
        Buffer.WriteBlock(NpcsData)

        i = 0

        For x = 1 To MAX_SHOPS
            If Len(Trim$(Shop(x).Name)) > 0 Then
                i = i + 1
            End If
        Next

        Buffer.WriteInt32(i)
        Buffer.WriteBlock(ShopsData)

        i = 0

        For x = 1 To MAX_SKILLS
            If Len(Trim$(Skill(x).Name)) > 0 Then
                i = i + 1
            End If
        Next

        Buffer.WriteInt32(i)
        Buffer.WriteBlock(SkillsData)

        i = 0

        For x = 1 To MAX_RESOURCES
            If Len(Trim$(Resource(x).Name)) > 0 Then
                i = i + 1
            End If
        Next

        Buffer.WriteInt32(i)
        Buffer.WriteBlock(ResourcesData)

        data = Compression.CompressBytes(Buffer.ToArray)

        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SGameData)

        Addlog("Sent SMSG: SGameData", PACKET_LOG)
        TextAdd("Sent SMSG: SGameData")

        Buffer.WriteBlock(data)

        Socket.SendDataTo(index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SayMsg_Global(ByVal Index As Integer, ByVal Message As String, ByVal SayColour As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SSayMsg)
        Buffer.WriteString(GetPlayerName(Index))
        Buffer.WriteInt32(GetPlayerAccess(Index))
        Buffer.WriteInt32(GetPlayerPK(Index))
        'Buffer.WriteString(Message)
        Buffer.WriteBytes(WriteUnicodeString(Message))
        Buffer.WriteString("[Global] ")
        Buffer.WriteInt32(SayColour)

        Addlog("Sent SMSG: SSayMsg Global", PACKET_LOG)
        TextAdd("Sent SMSG: SSayMsg Global")

        SendDataToAll(Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendInventoryUpdate(ByVal Index As Integer, ByVal InvSlot As Integer)
        Dim Buffer As ByteStream, n As Integer
        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SPlayerInvUpdate)
        Buffer.WriteInt32(InvSlot)
        Buffer.WriteInt32(GetPlayerInvItemNum(Index, InvSlot))
        Buffer.WriteInt32(GetPlayerInvItemValue(Index, InvSlot))

        Addlog("Sent SMSG: SPlayerInvUpdate", PACKET_LOG)
        TextAdd("Sent SMSG: SPlayerInvUpdate")

        Buffer.WriteString(Player(Index).Character(TempPlayer(Index).CurChar).RandInv(InvSlot).Prefix)
        Buffer.WriteString(Player(Index).Character(TempPlayer(Index).CurChar).RandInv(InvSlot).Suffix)
        Buffer.WriteInt32(Player(Index).Character(TempPlayer(Index).CurChar).RandInv(InvSlot).Rarity)
        For n = 1 To Stats.Count - 1
            Buffer.WriteInt32(Player(Index).Character(TempPlayer(Index).CurChar).RandInv(InvSlot).Stat(n))
        Next n
        Buffer.WriteInt32(Player(Index).Character(TempPlayer(Index).CurChar).RandInv(InvSlot).Damage)
        Buffer.WriteInt32(Player(Index).Character(TempPlayer(Index).CurChar).RandInv(InvSlot).Speed)

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendAnimation(ByVal MapNum As Integer, ByVal Anim As Integer, ByVal X As Integer, ByVal Y As Integer, Optional ByVal LockType As Byte = 0, Optional ByVal LockIndex As Integer = 0)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SAnimation)
        Buffer.WriteInt32(Anim)
        Buffer.WriteInt32(X)
        Buffer.WriteInt32(Y)
        Buffer.WriteInt32(LockType)
        Buffer.WriteInt32(LockIndex)

        Addlog("Sent SMSG: SAnimation", PACKET_LOG)
        TextAdd("Sent SMSG: SAnimation")

        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendOpenShop(ByVal Index As Integer, ByVal ShopNum As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SOpenShop)
        Buffer.WriteInt32(ShopNum)
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SOpenShop", PACKET_LOG)
        TextAdd("Sent SMSG: SOpenShop")

        Buffer.Dispose()
    End Sub

    Sub ResetShopAction(ByVal Index As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SResetShopAction)

        Addlog("Sent SMSG: SResetShopAction", PACKET_LOG)
        TextAdd("Sent SMSG: SResetShopAction")

        SendDataToAll(Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendBank(ByVal Index As Integer)
        Dim Buffer As ByteStream
        Dim i As Integer

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SBank)

        Addlog("Sent SMSG: SBank", PACKET_LOG)
        TextAdd("Sent SMSG: SBank")

        For i = 1 To MAX_BANK
            Buffer.WriteInt32(Bank(Index).Item(i).Num)
            Buffer.WriteInt32(Bank(Index).Item(i).Value)

            Buffer.WriteString(Bank(Index).ItemRand(i).Prefix)
            Buffer.WriteString(Bank(Index).ItemRand(i).Suffix)
            Buffer.WriteInt32(Bank(Index).ItemRand(i).Rarity)
            Buffer.WriteInt32(Bank(Index).ItemRand(i).Damage)
            Buffer.WriteInt32(Bank(Index).ItemRand(i).Speed)

            For x = 1 To Stats.Count - 1
                Buffer.WriteInt32(Bank(Index).ItemRand(i).Stat(x))
            Next
        Next

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendClearSkillBuffer(ByVal Index As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SClearSkillBuffer)

        Addlog("Sent SMSG: SClearSkillBuffer", PACKET_LOG)
        TextAdd("Sent SMSG: SClearSkillBuffer")

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendClearTradeTimer(ByVal Index As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SClearTradeTimer)

        Addlog("Sent SMSG: SClearTradeTimer", PACKET_LOG)
        TextAdd("Sent SMSG: SClearTradeTimer")

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendTradeInvite(ByVal Index As Integer, ByVal TradeIndex As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.STradeInvite)

        Addlog("Sent SMSG: STradeInvite", PACKET_LOG)
        TextAdd("Sent SMSG: STradeInvite")

        Buffer.WriteInt32(TradeIndex)

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendTrade(ByVal Index As Integer, ByVal TradeTarget As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.STrade)
        Buffer.WriteInt32(TradeTarget)
        Buffer.WriteString(Trim$(GetPlayerName(TradeTarget)))
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: STrade", PACKET_LOG)
        TextAdd("Sent SMSG: STrade")

        Buffer.Dispose()
    End Sub

    Sub SendTradeUpdate(ByVal Index As Integer, ByVal DataType As Byte)
        Dim Buffer As ByteStream
        Dim i As Integer
        Dim tradeTarget As Integer
        Dim totalWorth As Integer

        tradeTarget = TempPlayer(Index).InTrade

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.STradeUpdate)
        Buffer.WriteInt32(DataType)

        Addlog("Sent SMSG: STradeUpdate", PACKET_LOG)
        TextAdd("Sent SMSG: STradeUpdate")

        If DataType = 0 Then ' own inventory

            For i = 1 To MAX_INV
                Buffer.WriteInt32(TempPlayer(Index).TradeOffer(i).Num)
                Buffer.WriteInt32(TempPlayer(Index).TradeOffer(i).Value)

                ' add total worth
                If TempPlayer(Index).TradeOffer(i).Num > 0 Then
                    ' currency?
                    If Item(TempPlayer(Index).TradeOffer(i).Num).Type = ItemType.Currency Or Item(TempPlayer(Index).TradeOffer(i).Num).Stackable = 1 Then
                        If TempPlayer(Index).TradeOffer(i).Value = 0 Then TempPlayer(Index).TradeOffer(i).Value = 1
                        totalWorth = totalWorth + (Item(GetPlayerInvItemNum(Index, TempPlayer(Index).TradeOffer(i).Num)).Price * TempPlayer(Index).TradeOffer(i).Value)
                    Else
                        totalWorth = totalWorth + Item(GetPlayerInvItemNum(Index, TempPlayer(Index).TradeOffer(i).Num)).Price
                    End If
                End If
            Next
        ElseIf DataType = 1 Then ' other inventory

            For i = 1 To MAX_INV
                Buffer.WriteInt32(GetPlayerInvItemNum(tradeTarget, TempPlayer(tradeTarget).TradeOffer(i).Num))
                Buffer.WriteInt32(TempPlayer(tradeTarget).TradeOffer(i).Value)

                ' add total worth
                If GetPlayerInvItemNum(tradeTarget, TempPlayer(tradeTarget).TradeOffer(i).Num) > 0 Then
                    ' currency?
                    If Item(GetPlayerInvItemNum(tradeTarget, TempPlayer(tradeTarget).TradeOffer(i).Num)).Type = ItemType.Currency Or Item(GetPlayerInvItemNum(tradeTarget, TempPlayer(tradeTarget).TradeOffer(i).Num)).Stackable = 1 Then
                        If TempPlayer(tradeTarget).TradeOffer(i).Value = 0 Then TempPlayer(tradeTarget).TradeOffer(i).Value = 1
                        totalWorth = totalWorth + (Item(GetPlayerInvItemNum(tradeTarget, TempPlayer(tradeTarget).TradeOffer(i).Num)).Price * TempPlayer(tradeTarget).TradeOffer(i).Value)
                    Else
                        totalWorth = totalWorth + Item(GetPlayerInvItemNum(tradeTarget, TempPlayer(tradeTarget).TradeOffer(i).Num)).Price
                    End If
                End If
            Next
        End If

        ' send total worth of trade
        Buffer.WriteInt32(totalWorth)

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendTradeStatus(ByVal Index As Integer, ByVal Status As Byte)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.STradeStatus)
        Buffer.WriteInt32(Status)
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: STradeStatus", PACKET_LOG)
        TextAdd("Sent SMSG: STradeStatus")

        Buffer.Dispose()
    End Sub

    Sub SendMapItemsToAll(ByVal MapNum As Integer)
        Dim i As Integer
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SMapItemData)

        Addlog("Sent SMSG: SMapItemData To All", PACKET_LOG)
        TextAdd("Sent SMSG: SMapItemdata To All")

        For i = 1 To MAX_MAP_ITEMS
            Buffer.WriteInt32(MapItem(MapNum, i).Num)
            Buffer.WriteInt32(MapItem(MapNum, i).Value)
            Buffer.WriteInt32(MapItem(MapNum, i).X)
            Buffer.WriteInt32(MapItem(MapNum, i).Y)
        Next

        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendStunned(ByVal Index As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SStunned)
        Buffer.WriteInt32(TempPlayer(Index).StunDuration)

        Addlog("Sent SMSG: SStunned", PACKET_LOG)
        TextAdd("Sent SMSG: SStunned")

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendBlood(ByVal MapNum As Integer, ByVal X As Integer, ByVal Y As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SBlood)
        Buffer.WriteInt32(X)
        Buffer.WriteInt32(Y)

        Addlog("Sent SMSG: SBlood", PACKET_LOG)
        TextAdd("Sent SMSG: SBlood")

        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendPlayerSkills(ByVal Index As Integer)
        Dim i As Integer
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SSkills)

        Addlog("Sent SMSG: SSkills", PACKET_LOG)
        TextAdd("Sent SMSG: SSkills")

        For i = 1 To MAX_PLAYER_SKILLS
            Buffer.WriteInt32(GetPlayerSkill(Index, i))
        Next

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendCooldown(ByVal Index As Integer, ByVal Slot As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SCooldown)
        Buffer.WriteInt32(Slot)

        Addlog("Sent SMSG: SCooldown", PACKET_LOG)
        TextAdd("Sent SMSG: SCooldown")

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendTarget(ByVal Index As Integer, ByVal Target As Integer, ByVal TargetType As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.STarget)
        Buffer.WriteInt32(Target)
        Buffer.WriteInt32(TargetType)

        Addlog("Sent SMSG: STarget", PACKET_LOG)
        TextAdd("Sent SMSG: STarget")

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    'Mapreport
    Sub SendMapReport(ByVal Index As Integer)
        Dim Buffer As ByteStream, I As Integer

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SMapReport)

        Addlog("Sent SMSG: SMapReport", PACKET_LOG)
        TextAdd("Sent SMSG: SMapReport")

        For I = 1 To MAX_MAPS
            Buffer.WriteString(Trim(Map(I).Name))
        Next

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendAdminPanel(ByVal Index As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SAdmin)

        Addlog("Sent SMSG: SAdmin", PACKET_LOG)
        TextAdd("Sent SMSG: SAdmin")

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendMapNames(ByVal Index As Integer)
        Dim Buffer As ByteStream, I As Integer

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SMapNames)

        Addlog("Sent SMSG: SMapNames", PACKET_LOG)
        TextAdd("Sent SMSG: SMapNames")

        For I = 1 To MAX_MAPS
            Buffer.WriteString(Trim(Map(I).Name))
        Next

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendHotbar(ByVal Index As Integer)
        Dim Buffer As ByteStream, i As Integer

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SHotbar)

        Addlog("Sent SMSG: SHotbar", PACKET_LOG)
        TextAdd("Sent SMSG: SHotbar")

        For i = 1 To MAX_HOTBAR
            Buffer.WriteInt32(Player(Index).Character(TempPlayer(Index).CurChar).Hotbar(i).Slot)
            Buffer.WriteInt32(Player(Index).Character(TempPlayer(Index).CurChar).Hotbar(i).SlotType)
        Next

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendCritical(ByVal Index As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SCritical)

        Addlog("Sent SMSG: SCritical", PACKET_LOG)
        TextAdd("Sent SMSG: SCritical")

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendKeyPair(ByVal index As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SKeyPair)
        Buffer.WriteString(EKeyPair.ExportKeyString(False))
        Socket.SendDataTo(index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendNews(ByVal Index As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SNews)

        Addlog("Sent SMSG: SNews", PACKET_LOG)
        TextAdd("Sent SMSG: SNews")

        Buffer.WriteString(Trim(Options.GameName))
        Buffer.WriteString(Trim(GetFileContents(Application.StartupPath & "\data\news.txt")))

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendRightClick(ByVal Index As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SrClick)

        Addlog("Sent SMSG: SrClick", PACKET_LOG)
        TextAdd("Sent SMSG: SrClick")

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendClassEditor(ByVal Index As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SClassEditor)

        Addlog("Sent SMSG: SClassEditor", PACKET_LOG)
        TextAdd("Sent SMSG: SClassEditor")

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendAutoMapper(ByVal Index As Integer)
        Dim Buffer As ByteStream, Prefab As Integer
        Dim myXml As New XmlClass With {
            .Filename = Application.StartupPath & "\Data\AutoMapper.xml",
            .Root = "Options"
        }
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SAutoMapper)

        Addlog("Sent SMSG: SAutoMapper", PACKET_LOG)
        TextAdd("Sent SMSG: SAutoMapper")

        Buffer.WriteInt32(MapStart)
        Buffer.WriteInt32(MapSize)
        Buffer.WriteInt32(MapX)
        Buffer.WriteInt32(MapY)
        Buffer.WriteInt32(SandBorder)
        Buffer.WriteInt32(DetailFreq)
        Buffer.WriteInt32(ResourceFreq)

        'send ini info
        Buffer.WriteString(myXml.ReadString("Resources", "ResourcesNum"))

        For Prefab = 1 To TilePrefab.Count - 1
            For Layer = 1 To MapLayer.Count - 1
                If Val(myXml.ReadString("Prefab" & Prefab, "Layer" & Layer & "Tileset")) > 0 Then
                    Buffer.WriteInt32(Layer)
                    Buffer.WriteInt32(Val(myXml.ReadString("Prefab" & Prefab, "Layer" & Layer & "Tileset")))
                    Buffer.WriteInt32(Val(myXml.ReadString("Prefab" & Prefab, "Layer" & Layer & "X")))
                    Buffer.WriteInt32(Val(myXml.ReadString("Prefab" & Prefab, "Layer" & Layer & "Y")))
                    Buffer.WriteInt32(Val(myXml.ReadString("Prefab" & Prefab, "Layer" & Layer & "Autotile")))
                End If
            Next
            Buffer.WriteInt32(Val(myXml.ReadString("Prefab" & Prefab, "Type")))
        Next

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendEmote(ByVal Index As Integer, ByVal Emote As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SEmote)

        Addlog("Sent SMSG: SEmote", PACKET_LOG)
        TextAdd("Sent SMSG: SEmote")

        Buffer.WriteInt32(Index)
        Buffer.WriteInt32(Emote)

        SendDataToMap(GetPlayerMap(Index), Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendChatBubble(ByVal MapNum As Integer, ByVal Target As Integer, ByVal TargetType As Integer, ByVal Message As String, ByVal Colour As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SChatBubble)

        Addlog("Sent SMSG: SChatBubble", PACKET_LOG)
        TextAdd("Sent SMSG: SChatBubble")

        Buffer.WriteInt32(Target)
        Buffer.WriteInt32(TargetType)
        'Buffer.WriteString(Message)
        Buffer.WriteBytes(WriteUnicodeString(Message))
        Buffer.WriteInt32(Colour)
        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)

        Buffer.Dispose()

    End Sub

    Sub SendPlayerAttack(ByVal Index As Integer)
        Dim Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SAttack)

        Addlog("Sent SMSG: SPlayerAttack", PACKET_LOG)
        TextAdd("Sent SMSG: SPlayerAttack")

        Buffer.WriteInt32(Index)
        SendDataToMapBut(Index, GetPlayerMap(Index), Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendNpcAttack(ByVal Index As Integer, ByVal NpcNum As Integer)
        Dim Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SAttack)

        Addlog("Sent SMSG: SNpcAttack", PACKET_LOG)
        TextAdd("Sent SMSG: SNpcAttack")

        Buffer.WriteInt32(NpcNum)
        SendDataToMap(GetPlayerMap(Index), Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendNpcDead(ByVal MapNum As Integer, ByVal Index As Integer)
        Dim Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SNpcDead)

        Addlog("Sent SMSG: SNpcDead", PACKET_LOG)
        TextAdd("Sent SMSG: SNpcDead")

        Buffer.WriteInt32(Index)
        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub SendTotalOnlineTo(ByVal Index As Integer)
        Dim Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.STotalOnline)

        Addlog("Sent SMSG: STotalOnline", PACKET_LOG)
        TextAdd("Sent SMSG: STotalOnline")

        Buffer.WriteInt32(GetPlayersOnline)
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub

    Sub SendTotalOnlineToAll()
        Dim Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.STotalOnline)

        Addlog("Sent SMSG: STotalOnline To All", PACKET_LOG)
        TextAdd("Sent SMSG: STotalOnline To All")

        Buffer.WriteInt32(GetPlayersOnline)
        SendDataToAll(Buffer.Data, Buffer.Head)

        Buffer.Dispose()
    End Sub
End Module
