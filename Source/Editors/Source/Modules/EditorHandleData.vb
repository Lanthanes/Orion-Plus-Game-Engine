Imports ASFW
Imports ASFW.IO

Module EditorHandleData
    Public PlayerBuffer As ByteBuffer
    Private Delegate Sub Packet_(ByVal Data() As Byte)
    Private Packets As Dictionary(Of Integer, Packet_)

    Public Sub HandleData(ByVal data() As Byte)
        Dim pLength As Integer

        If PlayerBuffer Is Nothing Then PlayerBuffer = New ByteBuffer()
        PlayerBuffer.WriteBytes(data.Clone)

        If PlayerBuffer.Count = 0 Then
            PlayerBuffer.Clear()
            Exit Sub
        End If

        If PlayerBuffer.Length >= 4 Then
            pLength = PlayerBuffer.ReadInteger(False)

            If pLength <= 0 Then
                PlayerBuffer.Clear()
                Exit Sub
            End If
        End If

        If PlayerBuffer.Length >= 4 Then
            pLength = PlayerBuffer.ReadInteger(False)

            If pLength <= 0 Then
                PlayerBuffer.Clear()
                Exit Sub
            End If
        End If

        Do While pLength > 0 And pLength <= PlayerBuffer.Length - 4

            If pLength <= PlayerBuffer.Length - 4 Then
                PlayerBuffer.ReadInteger()
                HandleDataPackets(PlayerBuffer.ReadBytes(pLength))
            End If

            pLength = 0

            If PlayerBuffer.Length >= 4 Then
                pLength = PlayerBuffer.ReadInteger(False)

                If pLength < 0 Then
                    PlayerBuffer.Clear()
                    Exit Sub
                End If
            End If

        Loop
    End Sub

    Public Sub InitMessages()
        Packets = New Dictionary(Of Integer, Packet_)

        Packets.Add(ServerPackets.SAlertMsg, AddressOf Packet_AlertMSG)
        Packets.Add(ServerPackets.SKeyPair, AddressOf Packet_KeyPair)

        Packets.Add(ServerPackets.SLoginOk, AddressOf Packet_LoginOk)
        Packets.Add(ServerPackets.SClassesData, AddressOf Packet_ClassesData)

        Packets.Add(ServerPackets.SMapData, AddressOf Packet_MapData)

        Packets.Add(ServerPackets.SMapNpcData, AddressOf Packet_MapNPCData)
        Packets.Add(ServerPackets.SMapNpcUpdate, AddressOf Packet_MapNPCUpdate)

        Packets.Add(ServerPackets.SItemEditor, AddressOf Packet_EditItem)
        Packets.Add(ServerPackets.SUpdateItem, AddressOf Packet_UpdateItem)

        Packets.Add(ServerPackets.SREditor, AddressOf Packet_ResourceEditor)

        Packets.Add(ServerPackets.SNpcEditor, AddressOf Packet_NPCEditor)
        Packets.Add(ServerPackets.SUpdateNpc, AddressOf Packet_UpdateNPC)

        Packets.Add(ServerPackets.SEditMap, AddressOf Packet_EditMap)

        Packets.Add(ServerPackets.SShopEditor, AddressOf Packet_EditShop)
        Packets.Add(ServerPackets.SUpdateShop, AddressOf Packet_UpdateShop)

        Packets.Add(ServerPackets.SSkillEditor, AddressOf Packet_EditSkill)
        Packets.Add(ServerPackets.SUpdateSkill, AddressOf Packet_UpdateSkill)

        Packets.Add(ServerPackets.SResourceEditor, AddressOf Packet_ResourceEditor)
        Packets.Add(ServerPackets.SUpdateResource, AddressOf Packet_UpdateResource)

        Packets.Add(ServerPackets.SAnimationEditor, AddressOf Packet_EditAnimation)
        Packets.Add(ServerPackets.SUpdateAnimation, AddressOf Packet_UpdateAnimation)

        Packets.Add(ServerPackets.SGameData, AddressOf Packet_GameData)
        Packets.Add(ServerPackets.SMapReport, AddressOf Packet_Mapreport) 'Mapreport

        Packets.Add(ServerPackets.SMapNames, AddressOf Packet_MapNames)

        'quests
        Packets.Add(ServerPackets.SQuestEditor, AddressOf Packet_QuestEditor)
        Packets.Add(ServerPackets.SUpdateQuest, AddressOf Packet_UpdateQuest)

        'Housing
        'Packets.Add(ServerPackets.SHouseConfigs, AddressOf Packet_HouseConfigurations)
        'Packets.Add(ServerPackets.SFurniture, AddressOf Packet_Furniture)
        Packets.Add(ServerPackets.SHouseEdit, AddressOf Packet_EditHouses)

        'Events
        Packets.Add(ServerPackets.SSpawnEvent, AddressOf Packet_SpawnEvent)
        Packets.Add(ServerPackets.SEventMove, AddressOf Packet_EventMove)
        Packets.Add(ServerPackets.SEventDir, AddressOf Packet_EventDir)
        Packets.Add(ServerPackets.SEventChat, AddressOf Packet_EventChat)
        Packets.Add(ServerPackets.SEventStart, AddressOf Packet_EventStart)
        Packets.Add(ServerPackets.SEventEnd, AddressOf Packet_EventEnd)
        Packets.Add(ServerPackets.SSwitchesAndVariables, AddressOf Packet_SwitchesAndVariables)
        Packets.Add(ServerPackets.SMapEventData, AddressOf Packet_MapEventData)
        Packets.Add(ServerPackets.SHoldPlayer, AddressOf Packet_HoldPlayer)

        Packets.Add(ServerPackets.SProjectileEditor, AddressOf HandleProjectileEditor)
        Packets.Add(ServerPackets.SUpdateProjectile, AddressOf HandleUpdateProjectile)
        Packets.Add(ServerPackets.SMapProjectile, AddressOf HandleMapProjectile)

        'craft
        Packets.Add(ServerPackets.SUpdateRecipe, AddressOf Packet_UpdateRecipe)
        Packets.Add(ServerPackets.SRecipeEditor, AddressOf Packet_RecipeEditor)

        Packets.Add(ServerPackets.SClassEditor, AddressOf Packet_ClassEditor)

        'Auto Mapper
        Packets.Add(ServerPackets.SAutoMapper, AddressOf Packet_AutoMapper)

        'pets
        Packets.Add(ServerPackets.SPetEditor, AddressOf Packet_PetEditor)
        Packets.Add(ServerPackets.SUpdatePet, AddressOf Packet_UpdatePet)
    End Sub

    Sub HandleDataPackets(ByVal data() As Byte)
        Dim packetnum As Integer, Packet As Packet_ = Nothing
        packetnum = BitConverter.ToInt32(data, 0)

        If packetnum = ServerPackets.SNews Then Exit Sub

        If Packets.TryGetValue(packetnum, Packet) Then
            Dim bytes As Byte() : ReDim bytes(data.Length - 5)
            Buffer.BlockCopy(data, 4, bytes, 0, bytes.Length)
            Packet.Invoke(bytes)
        End If
    End Sub

    Sub Packet_AlertMSG(ByVal data() As Byte)
        Dim Msg As String
        Dim Buffer As New ByteStream(data)
        Msg = Buffer.ReadString

        Buffer.Dispose

        MsgBox(Msg, vbOKOnly, "OrionClient+ Editors")

        CloseEditor()
    End Sub

    Sub Packet_KeyPair(ByVal Data() As Byte)
        Dim Buffer As New ByteStream(Data)
        EKeyPair.ImportKeyString(Buffer.ReadString())
        Buffer.Dispose
    End Sub

    Sub Packet_LoginOk(ByVal Data() As Byte)
        InitEditor = True
    End Sub

    Private Sub Packet_ClassesData(ByVal data() As Byte)
        Dim i As Integer
        Dim z As Integer, X As Integer
        Dim Buffer As New ByteStream(data)
        ' Max classes
        Max_Classes = Buffer.ReadInt32
        ReDim Classes(0 To Max_Classes)

        For i = 0 To Max_Classes
            ReDim Classes(i).Stat(0 To Stats.Count - 1)
        Next

        For i = 0 To Max_Classes
            ReDim Classes(i).Vital(0 To Vitals.Count - 1)
        Next

        For i = 1 To Max_Classes

            With Classes(i)
                .Name = Trim$(Buffer.ReadString)
                .Desc = Trim$(Buffer.ReadString)

                .Vital(Vitals.HP) = Buffer.ReadInt32
                .Vital(Vitals.MP) = Buffer.ReadInt32
                .Vital(Vitals.SP) = Buffer.ReadInt32

                ' get array size
                z = Buffer.ReadInt32
                ' redim array
                ReDim .MaleSprite(0 To z)
                ' loop-receive data
                For X = 0 To z
                    .MaleSprite(X) = Buffer.ReadInt32
                Next

                ' get array size
                z = Buffer.ReadInt32
                ' redim array
                ReDim .FemaleSprite(0 To z)
                ' loop-receive data
                For X = 0 To z
                    .FemaleSprite(X) = Buffer.ReadInt32
                Next

                .Stat(Stats.Strength) = Buffer.ReadInt32
                .Stat(Stats.Endurance) = Buffer.ReadInt32
                .Stat(Stats.Vitality) = Buffer.ReadInt32
                .Stat(Stats.Intelligence) = Buffer.ReadInt32
                .Stat(Stats.Luck) = Buffer.ReadInt32
                .Stat(Stats.Spirit) = Buffer.ReadInt32

                ReDim .StartItem(5)
                ReDim .StartValue(5)
                For q = 1 To 5
                    .StartItem(q) = Buffer.ReadInt32
                    .StartValue(q) = Buffer.ReadInt32
                Next

                .StartMap = Buffer.ReadInt32
                .StartX = Buffer.ReadInt32
                .StartY = Buffer.ReadInt32

                .BaseExp = Buffer.ReadInt32
            End With

        Next

        Buffer.Dispose
    End Sub

    Private Sub Packet_MapData(ByVal Data() As Byte)
        Dim X As Integer, Y As Integer, i As Integer
        Dim MusicFile As String
        Dim Buffer As New ByteStream(Compression.DecompressBytes(Data, 4, Data.Length))

        MapData = False

        SyncLock MapLock
            If Buffer.ReadInt32 = 1 Then
                ClearMap()
                Map.MapNum = Buffer.ReadInt32
                Map.Name = Trim(Buffer.ReadString)
                Map.Music = Trim(Buffer.ReadString)
                Map.Revision = Buffer.ReadInt32
                Map.Moral = Buffer.ReadInt32
                Map.tileset = Buffer.ReadInt32
                Map.Up = Buffer.ReadInt32
                Map.Down = Buffer.ReadInt32
                Map.Left = Buffer.ReadInt32
                Map.Right = Buffer.ReadInt32
                Map.BootMap = Buffer.ReadInt32
                Map.BootX = Buffer.ReadInt32
                Map.BootY = Buffer.ReadInt32
                Map.MaxX = Buffer.ReadInt32
                Map.MaxY = Buffer.ReadInt32
                Map.WeatherType = Buffer.ReadInt32
                Map.FogIndex = Buffer.ReadInt32
                Map.WeatherIntensity = Buffer.ReadInt32
                Map.FogAlpha = Buffer.ReadInt32
                Map.FogSpeed = Buffer.ReadInt32
                Map.HasMapTint = Buffer.ReadInt32
                Map.MapTintR = Buffer.ReadInt32
                Map.MapTintG = Buffer.ReadInt32
                Map.MapTintB = Buffer.ReadInt32
                Map.MapTintA = Buffer.ReadInt32

                Map.Instanced = Buffer.ReadInt32
                Map.Panorama = Buffer.ReadInt32
                Map.Parallax = Buffer.ReadInt32

                ReDim Map.Tile(0 To Map.MaxX, 0 To Map.MaxY)

                For X = 1 To MAX_MAP_NPCS
                    Map.Npc(X) = Buffer.ReadInt32
                Next

                For X = 0 To Map.MaxX
                    For Y = 0 To Map.MaxY
                        Map.Tile(X, Y).Data1 = Buffer.ReadInt32
                        Map.Tile(X, Y).Data2 = Buffer.ReadInt32
                        Map.Tile(X, Y).Data3 = Buffer.ReadInt32
                        Map.Tile(X, Y).DirBlock = Buffer.ReadInt32

                        ReDim Map.Tile(X, Y).Layer(0 To MapLayer.Count - 1)

                        For i = 0 To MapLayer.Count - 1
                            Map.Tile(X, Y).Layer(i).Tileset = Buffer.ReadInt32
                            Map.Tile(X, Y).Layer(i).X = Buffer.ReadInt32
                            Map.Tile(X, Y).Layer(i).Y = Buffer.ReadInt32
                            Map.Tile(X, Y).Layer(i).AutoTile = Buffer.ReadInt32
                        Next
                        Map.Tile(X, Y).Type = Buffer.ReadInt32
                    Next
                Next

                'Event Data!
                ResetEventdata()

                Map.EventCount = Buffer.ReadInt32

                If Map.EventCount > 0 Then
                    ReDim Map.Events(0 To Map.EventCount)
                    For i = 1 To Map.EventCount
                        With Map.Events(i)
                            .Name = Trim(Buffer.ReadString)
                            .Globals = Buffer.ReadInt32
                            .X = Buffer.ReadInt32
                            .Y = Buffer.ReadInt32
                            .PageCount = Buffer.ReadInt32
                        End With
                        If Map.Events(i).PageCount > 0 Then
                            ReDim Map.Events(i).Pages(0 To Map.Events(i).PageCount)
                            For X = 1 To Map.Events(i).PageCount
                                With Map.Events(i).Pages(X)
                                    .chkVariable = Buffer.ReadInt32
                                    .VariableIndex = Buffer.ReadInt32
                                    .VariableCondition = Buffer.ReadInt32
                                    .VariableCompare = Buffer.ReadInt32

                                    .chkSwitch = Buffer.ReadInt32
                                    .SwitchIndex = Buffer.ReadInt32
                                    .SwitchCompare = Buffer.ReadInt32

                                    .chkHasItem = Buffer.ReadInt32
                                    .HasItemIndex = Buffer.ReadInt32
                                    .HasItemAmount = Buffer.ReadInt32

                                    .chkSelfSwitch = Buffer.ReadInt32
                                    .SelfSwitchIndex = Buffer.ReadInt32
                                    .SelfSwitchCompare = Buffer.ReadInt32

                                    .GraphicType = Buffer.ReadInt32
                                    .Graphic = Buffer.ReadInt32
                                    .GraphicX = Buffer.ReadInt32
                                    .GraphicY = Buffer.ReadInt32
                                    .GraphicX2 = Buffer.ReadInt32
                                    .GraphicY2 = Buffer.ReadInt32

                                    .MoveType = Buffer.ReadInt32
                                    .MoveSpeed = Buffer.ReadInt32
                                    .MoveFreq = Buffer.ReadInt32

                                    .MoveRouteCount = Buffer.ReadInt32

                                    .IgnoreMoveRoute = Buffer.ReadInt32
                                    .RepeatMoveRoute = Buffer.ReadInt32

                                    If .MoveRouteCount > 0 Then
                                        ReDim Map.Events(i).Pages(X).MoveRoute(0 To .MoveRouteCount)
                                        For Y = 1 To .MoveRouteCount
                                            .MoveRoute(Y).Index = Buffer.ReadInt32
                                            .MoveRoute(Y).Data1 = Buffer.ReadInt32
                                            .MoveRoute(Y).Data2 = Buffer.ReadInt32
                                            .MoveRoute(Y).Data3 = Buffer.ReadInt32
                                            .MoveRoute(Y).Data4 = Buffer.ReadInt32
                                            .MoveRoute(Y).Data5 = Buffer.ReadInt32
                                            .MoveRoute(Y).Data6 = Buffer.ReadInt32
                                        Next
                                    End If

                                    .WalkAnim = Buffer.ReadInt32
                                    .DirFix = Buffer.ReadInt32
                                    .WalkThrough = Buffer.ReadInt32
                                    .ShowName = Buffer.ReadInt32
                                    .Trigger = Buffer.ReadInt32
                                    .CommandListCount = Buffer.ReadInt32

                                    .Position = Buffer.ReadInt32
                                    .Questnum = Buffer.ReadInt32

                                    .chkPlayerGender = Buffer.ReadInt32
                                End With

                                If Map.Events(i).Pages(X).CommandListCount > 0 Then
                                    ReDim Map.Events(i).Pages(X).CommandList(0 To Map.Events(i).Pages(X).CommandListCount)
                                    For Y = 1 To Map.Events(i).Pages(X).CommandListCount
                                        Map.Events(i).Pages(X).CommandList(Y).CommandCount = Buffer.ReadInt32
                                        Map.Events(i).Pages(X).CommandList(Y).ParentList = Buffer.ReadInt32
                                        If Map.Events(i).Pages(X).CommandList(Y).CommandCount > 0 Then
                                            ReDim Map.Events(i).Pages(X).CommandList(Y).Commands(0 To Map.Events(i).Pages(X).CommandList(Y).CommandCount)
                                            For z = 1 To Map.Events(i).Pages(X).CommandList(Y).CommandCount
                                                With Map.Events(i).Pages(X).CommandList(Y).Commands(z)
                                                    .Index = Buffer.ReadInt32
                                                    .Text1 = Trim(Buffer.ReadString)
                                                    .Text2 = Trim(Buffer.ReadString)
                                                    .Text3 = Trim(Buffer.ReadString)
                                                    .Text4 = Trim(Buffer.ReadString)
                                                    .Text5 = Trim(Buffer.ReadString)
                                                    .Data1 = Buffer.ReadInt32
                                                    .Data2 = Buffer.ReadInt32
                                                    .Data3 = Buffer.ReadInt32
                                                    .Data4 = Buffer.ReadInt32
                                                    .Data5 = Buffer.ReadInt32
                                                    .Data6 = Buffer.ReadInt32
                                                    .ConditionalBranch.CommandList = Buffer.ReadInt32
                                                    .ConditionalBranch.Condition = Buffer.ReadInt32
                                                    .ConditionalBranch.Data1 = Buffer.ReadInt32
                                                    .ConditionalBranch.Data2 = Buffer.ReadInt32
                                                    .ConditionalBranch.Data3 = Buffer.ReadInt32
                                                    .ConditionalBranch.ElseCommandList = Buffer.ReadInt32
                                                    .MoveRouteCount = Buffer.ReadInt32
                                                    If .MoveRouteCount > 0 Then
                                                        ReDim Preserve .MoveRoute(.MoveRouteCount)
                                                        For w = 1 To .MoveRouteCount
                                                            .MoveRoute(w).Index = Buffer.ReadInt32
                                                            .MoveRoute(w).Data1 = Buffer.ReadInt32
                                                            .MoveRoute(w).Data2 = Buffer.ReadInt32
                                                            .MoveRoute(w).Data3 = Buffer.ReadInt32
                                                            .MoveRoute(w).Data4 = Buffer.ReadInt32
                                                            .MoveRoute(w).Data5 = Buffer.ReadInt32
                                                            .MoveRoute(w).Data6 = Buffer.ReadInt32
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

            End If

            For i = 1 To MAX_MAP_ITEMS
                MapItem(i).Num = Buffer.ReadInt32
                MapItem(i).Value = Buffer.ReadInt32()
                MapItem(i).X = Buffer.ReadInt32()
                MapItem(i).Y = Buffer.ReadInt32()
            Next

            For i = 1 To MAX_MAP_NPCS
                MapNpc(i).Num = Buffer.ReadInt32()
                MapNpc(i).X = Buffer.ReadInt32()
                MapNpc(i).Y = Buffer.ReadInt32()
                MapNpc(i).Dir = Buffer.ReadInt32()
                MapNpc(i).Vital(Vitals.HP) = Buffer.ReadInt32()
                MapNpc(i).Vital(Vitals.MP) = Buffer.ReadInt32()
            Next

            If Buffer.ReadInt32 = 1 Then
                Resource_Index = Buffer.ReadInt32
                Resources_Init = False

                If Resource_Index > 0 Then
                    ReDim MapResource(0 To Resource_Index)

                    For i = 0 To Resource_Index
                        MapResource(i).ResourceState = Buffer.ReadInt32
                        MapResource(i).X = Buffer.ReadInt32
                        MapResource(i).Y = Buffer.ReadInt32
                    Next

                    Resources_Init = True
                Else
                    ReDim MapResource(0 To 1)
                End If
            End If

            Buffer.Dispose

        End SyncLock

        ClearTempTile()
        InitAutotiles()

        MapData = True

        CurrentWeather = Map.WeatherType
        CurrentWeatherIntensity = Map.WeatherIntensity
        CurrentFog = Map.FogIndex
        CurrentFogSpeed = Map.FogSpeed
        CurrentFogOpacity = Map.FogAlpha
        CurrentTintR = Map.MapTintR
        CurrentTintG = Map.MapTintG
        CurrentTintB = Map.MapTintB
        CurrentTintA = Map.MapTintA

        MusicFile = Trim$(Map.Music)

        InMapEditor = True

        GettingMap = False
    End Sub

    Private Sub Packet_MapNPCData(ByVal Data() As Byte)
        Dim i As Integer
        Dim Buffer As New ByteStream(Data)

        For i = 1 To MAX_MAP_NPCS

            With MapNpc(i)
                .Num = Buffer.ReadInt32
                .X = Buffer.ReadInt32
                .Y = Buffer.ReadInt32
                .Dir = Buffer.ReadInt32
                .Vital(Vitals.HP) = Buffer.ReadInt32
                .Vital(Vitals.MP) = Buffer.ReadInt32
            End With

        Next

        Buffer.Dispose
    End Sub

    Private Sub Packet_MapNPCUpdate(ByVal Data() As Byte)
        Dim NpcNum As Integer
        Dim Buffer as ByteStream
        Buffer = New ByteStream(Data)

        NpcNum = Buffer.ReadInt32

        With MapNpc(NpcNum)
            .Num = Buffer.ReadInt32
            .X = Buffer.ReadInt32
            .Y = Buffer.ReadInt32
            .Dir = Buffer.ReadInt32
            .Vital(Vitals.HP) = Buffer.ReadInt32
            .Vital(Vitals.MP) = Buffer.ReadInt32
        End With

        Buffer.Dispose
    End Sub

    Private Sub Packet_EditItem(ByVal Data() As Byte)
        Dim Buffer as ByteStream
        Buffer = New ByteStream(Data)
        InitItemEditor = True

        Buffer.Dispose
    End Sub

    Sub Packet_UpdateItem(ByVal data() As Byte)
        Dim n As Integer, i As Integer
        Dim Buffer As New ByteStream(data)
        n = Buffer.ReadInt32

        ' Update the item
        Item(n).AccessReq = Buffer.ReadInt32()

        For i = 0 To Stats.Count - 1
            Item(n).Add_Stat(i) = Buffer.ReadInt32()
        Next

        Item(n).Animation = Buffer.ReadInt32()
        Item(n).BindType = Buffer.ReadInt32()
        Item(n).ClassReq = Buffer.ReadInt32()
        Item(n).Data1 = Buffer.ReadInt32()
        Item(n).Data2 = Buffer.ReadInt32()
        Item(n).Data3 = Buffer.ReadInt32()
        Item(n).TwoHanded = Buffer.ReadInt32()
        Item(n).LevelReq = Buffer.ReadInt32()
        Item(n).Mastery = Buffer.ReadInt32()
        Item(n).Name = Trim$(Buffer.ReadString())
        Item(n).Paperdoll = Buffer.ReadInt32()
        Item(n).Pic = Buffer.ReadInt32()
        Item(n).Price = Buffer.ReadInt32()
        Item(n).Rarity = Buffer.ReadInt32()
        Item(n).Speed = Buffer.ReadInt32()

        Item(n).Randomize = Buffer.ReadInt32()
        Item(n).RandomMin = Buffer.ReadInt32()
        Item(n).RandomMax = Buffer.ReadInt32()

        Item(n).Stackable = Buffer.ReadInt32()
        Item(n).Description = Trim$(Buffer.ReadString())

        For i = 0 To Stats.Count - 1
            Item(n).Stat_Req(i) = Buffer.ReadInt32()
        Next

        Item(n).Type = Buffer.ReadInt32()
        Item(n).SubType = Buffer.ReadInt32()

        Item(n).ItemLevel = Buffer.ReadInt32()

        'Housing
        Item(n).FurnitureWidth = Buffer.ReadInt32()
        Item(n).FurnitureHeight = Buffer.ReadInt32()

        For a = 1 To 3
            For b = 1 To 3
                Item(n).FurnitureBlocks(a, b) = Buffer.ReadInt32()
                Item(n).FurnitureFringe(a, b) = Buffer.ReadInt32()
            Next
        Next

        Item(n).KnockBack = Buffer.ReadInt32()
        Item(n).KnockBackTiles = Buffer.ReadInt32()

        Item(n).Projectile = Buffer.ReadInt32()
        Item(n).Ammo = Buffer.ReadInt32()

        Buffer.Dispose

    End Sub

    Sub Packet_NPCEditor(ByVal data() As Byte)
        Dim Buffer As New ByteStream(data)
        InitNPCEditor = True

        Buffer.Dispose
    End Sub

    Sub Packet_UpdateNPC(ByVal data() As Byte)
        Dim i As Integer, x As Integer
        Dim Buffer As New ByteStream(data)

        i = Buffer.ReadInt32
        ' Update the Npc
        Npc(i).Animation = Buffer.ReadInt32()
        Npc(i).AttackSay = Trim(Buffer.ReadString())
        Npc(i).Behaviour = Buffer.ReadInt32()
        ReDim Npc(i).DropChance(5)
        ReDim Npc(i).DropItem(5)
        ReDim Npc(i).DropItemValue(5)
        For x = 1 To 5
            Npc(i).DropChance(x) = Buffer.ReadInt32()
            Npc(i).DropItem(x) = Buffer.ReadInt32()
            Npc(i).DropItemValue(x) = Buffer.ReadInt32()
        Next

        Npc(i).Exp = Buffer.ReadInt32()
        Npc(i).Faction = Buffer.ReadInt32()
        Npc(i).Hp = Buffer.ReadInt32()
        Npc(i).Name = Trim(Buffer.ReadString())
        Npc(i).Range = Buffer.ReadInt32()
        Npc(i).SpawnTime = Buffer.ReadInt32()
        Npc(i).SpawnSecs = Buffer.ReadInt32()
        Npc(i).Sprite = Buffer.ReadInt32()

        For i = 0 To Stats.Count - 1
            Npc(i).Stat(i) = Buffer.ReadInt32()
        Next

        Npc(i).QuestNum = Buffer.ReadInt32()

        For x = 1 To MAX_NPC_SKILLS
            Npc(i).Skill(x) = Buffer.ReadInt32()
        Next

        Npc(i).Level = Buffer.ReadInt32()
        Npc(i).Damage = Buffer.ReadInt32()

        If Npc(i).AttackSay Is Nothing Then Npc(i).AttackSay = ""
        If Npc(i).Name Is Nothing Then Npc(i).Name = ""

        Buffer.Dispose
    End Sub

    Sub Packet_EditMap(ByVal data() As Byte)
        InitMapEditor = True
    End Sub

    Sub Packet_EditShop(ByVal data() As Byte)
        InitShopEditor = True
    End Sub

    Sub Packet_UpdateShop(ByVal data() As Byte)
        Dim shopnum As Integer
        Dim Buffer As New ByteStream(data)
        shopnum = Buffer.ReadInt32

        Shop(shopnum).BuyRate = Buffer.ReadInt32()
        Shop(shopnum).Name = Trim(Buffer.ReadString())
        Shop(shopnum).Face = Buffer.ReadInt32()

        For i = 0 To MAX_TRADES
            Shop(shopnum).TradeItem(i).CostItem = Buffer.ReadInt32()
            Shop(shopnum).TradeItem(i).CostValue = Buffer.ReadInt32()
            Shop(shopnum).TradeItem(i).Item = Buffer.ReadInt32()
            Shop(shopnum).TradeItem(i).ItemValue = Buffer.ReadInt32()
        Next

        If Shop(shopnum).Name Is Nothing Then Shop(shopnum).Name = ""

        Buffer.Dispose
    End Sub

    Sub Packet_EditSkill(ByVal data() As Byte)
        InitSkillEditor = True
    End Sub

    Sub Packet_UpdateSkill(ByVal data() As Byte)
        Dim skillnum As Integer
        Dim Buffer As New ByteStream(data)
        skillnum = Buffer.ReadInt32

        Skill(skillnum).AccessReq = Buffer.ReadInt32()
        Skill(skillnum).AoE = Buffer.ReadInt32()
        Skill(skillnum).CastAnim = Buffer.ReadInt32()
        Skill(skillnum).CastTime = Buffer.ReadInt32()
        Skill(skillnum).CdTime = Buffer.ReadInt32()
        Skill(skillnum).ClassReq = Buffer.ReadInt32()
        Skill(skillnum).Dir = Buffer.ReadInt32()
        Skill(skillnum).Duration = Buffer.ReadInt32()
        Skill(skillnum).Icon = Buffer.ReadInt32()
        Skill(skillnum).Interval = Buffer.ReadInt32()
        Skill(skillnum).IsAoE = Buffer.ReadInt32()
        Skill(skillnum).LevelReq = Buffer.ReadInt32()
        Skill(skillnum).Map = Buffer.ReadInt32()
        Skill(skillnum).MpCost = Buffer.ReadInt32()
        Skill(skillnum).Name = Trim(Buffer.ReadString())
        Skill(skillnum).Range = Buffer.ReadInt32()
        Skill(skillnum).SkillAnim = Buffer.ReadInt32()
        Skill(skillnum).StunDuration = Buffer.ReadInt32()
        Skill(skillnum).Type = Buffer.ReadInt32()
        Skill(skillnum).Vital = Buffer.ReadInt32()
        Skill(skillnum).X = Buffer.ReadInt32()
        Skill(skillnum).Y = Buffer.ReadInt32()

        Skill(skillnum).IsProjectile = Buffer.ReadInt32()
        Skill(skillnum).Projectile = Buffer.ReadInt32()

        Skill(skillnum).KnockBack = Buffer.ReadInt32()
        Skill(skillnum).KnockBackTiles = Buffer.ReadInt32()

        If Skill(skillnum).Name Is Nothing Then Skill(skillnum).Name = ""

        Buffer.Dispose

    End Sub

    Private Sub Packet_ResourceEditor(ByVal Data() As Byte)
        InitResourceEditor = True
    End Sub

    Private Sub Packet_UpdateResource(ByVal Data() As Byte)
        Dim ResourceNum As Integer
        Dim Buffer As New ByteStream(Data)
        ResourceNum = Buffer.ReadInt32

        Resource(ResourceNum).Animation = Buffer.ReadInt32()
        Resource(ResourceNum).EmptyMessage = Trim(Buffer.ReadString())
        Resource(ResourceNum).ExhaustedImage = Buffer.ReadInt32()
        Resource(ResourceNum).Health = Buffer.ReadInt32()
        Resource(ResourceNum).ExpReward = Buffer.ReadInt32()
        Resource(ResourceNum).ItemReward = Buffer.ReadInt32()
        Resource(ResourceNum).Name = Trim(Buffer.ReadString())
        Resource(ResourceNum).ResourceImage = Buffer.ReadInt32()
        Resource(ResourceNum).ResourceType = Buffer.ReadInt32()
        Resource(ResourceNum).RespawnTime = Buffer.ReadInt32()
        Resource(ResourceNum).SuccessMessage = Trim(Buffer.ReadString())
        Resource(ResourceNum).LvlRequired = Buffer.ReadInt32()
        Resource(ResourceNum).ToolRequired = Buffer.ReadInt32()
        Resource(ResourceNum).Walkthrough = Buffer.ReadInt32()

        If Resource(ResourceNum).Name Is Nothing Then Resource(ResourceNum).Name = ""
        If Resource(ResourceNum).EmptyMessage Is Nothing Then Resource(ResourceNum).EmptyMessage = ""
        If Resource(ResourceNum).SuccessMessage Is Nothing Then Resource(ResourceNum).SuccessMessage = ""

        Buffer.Dispose
    End Sub

    Private Sub Packet_EditAnimation(ByVal Data() As Byte)
        InitAnimationEditor = True
    End Sub

    Private Sub Packet_UpdateAnimation(ByVal Data() As Byte)
        Dim n As Integer, i As Integer
        Dim Buffer As New ByteStream(Data)
        n = Buffer.ReadInt32
        ' Update the Animation
        For i = 0 To UBound(Animation(n).Frames)
            Animation(n).Frames(i) = Buffer.ReadInt32()
        Next

        For i = 0 To UBound(Animation(n).LoopCount)
            Animation(n).LoopCount(i) = Buffer.ReadInt32()
        Next

        For i = 0 To UBound(Animation(n).LoopTime)
            Animation(n).LoopTime(i) = Buffer.ReadInt32()
        Next

        Animation(n).Name = Trim$(Buffer.ReadString)
        Animation(n).Sound = Trim$(Buffer.ReadString)

        If Animation(n).Name Is Nothing Then Animation(n).Name = ""
        If Animation(n).Sound Is Nothing Then Animation(n).Sound = ""

        For i = 0 To UBound(Animation(n).Sprite)
            Animation(n).Sprite(i) = Buffer.ReadInt32()
        Next
        Buffer.Dispose
    End Sub

    Private Sub Packet_GameData(ByVal Data() As Byte)
        Dim n As Integer, i As Integer, z As Integer, x As Integer, a As Integer, b As Integer
        Dim Buffer As New ByteStream(Compression.DecompressBytes(Data, 4, Data.Length))

        '\\\Read Class Data\\\

        ' Max classes
        Max_Classes = Buffer.ReadInt32
        ReDim Classes(0 To Max_Classes)

        For i = 0 To Max_Classes
            ReDim Classes(i).Stat(0 To Stats.Count - 1)
        Next

        For i = 0 To Max_Classes
            ReDim Classes(i).Vital(0 To Vitals.Count - 1)
        Next

        For i = 1 To Max_Classes

            With Classes(i)
                .Name = Trim(buffer.ReadString)
                .Desc = Trim$(buffer.ReadString)

                .Vital(Vitals.HP) = Buffer.ReadInt32
                .Vital(Vitals.MP) = Buffer.ReadInt32
                .Vital(Vitals.SP) = Buffer.ReadInt32

                ' get array size
                z = Buffer.ReadInt32
                ' redim array
                ReDim .MaleSprite(0 To z)
                ' loop-receive data
                For x = 0 To z
                    .MaleSprite(x) = Buffer.ReadInt32
                Next

                ' get array size
                z = Buffer.ReadInt32
                ' redim array
                ReDim .FemaleSprite(0 To z)
                ' loop-receive data
                For x = 0 To z
                    .FemaleSprite(x) = Buffer.ReadInt32
                Next

                .Stat(Stats.Strength) = Buffer.ReadInt32
                .Stat(Stats.Endurance) = Buffer.ReadInt32
                .Stat(Stats.Vitality) = Buffer.ReadInt32
                .Stat(Stats.Intelligence) = Buffer.ReadInt32
                .Stat(Stats.Luck) = Buffer.ReadInt32
                .Stat(Stats.Spirit) = Buffer.ReadInt32

                ReDim .StartItem(5)
                ReDim .StartValue(5)
                For q = 1 To 5
                    .StartItem(q) = Buffer.ReadInt32
                    .StartValue(q) = Buffer.ReadInt32
                Next

                .StartMap = Buffer.ReadInt32
                .StartX = Buffer.ReadInt32
                .StartY = Buffer.ReadInt32

                .BaseExp = Buffer.ReadInt32
            End With

        Next

        i = 0
        x = 0
        n = 0
        z = 0

        '\\\End Read Class Data\\\

        '\\\Read Item Data\\\\\\\
        x = Buffer.ReadInt32

        For i = 1 To x
            n = Buffer.ReadInt32

            ' Update the item
            Item(n).AccessReq = Buffer.ReadInt32()

            For z = 0 To Stats.Count - 1
                Item(n).Add_Stat(z) = Buffer.ReadInt32()
            Next

            Item(n).Animation = Buffer.ReadInt32()
            Item(n).BindType = Buffer.ReadInt32()
            Item(n).ClassReq = Buffer.ReadInt32()
            Item(n).Data1 = Buffer.ReadInt32()
            Item(n).Data2 = Buffer.ReadInt32()
            Item(n).Data3 = Buffer.ReadInt32()
            Item(n).TwoHanded = Buffer.ReadInt32()
            Item(n).LevelReq = Buffer.ReadInt32()
            Item(n).Mastery = Buffer.ReadInt32()
            Item(n).Name = Trim$(buffer.ReadString())
            Item(n).Paperdoll = Buffer.ReadInt32()
            Item(n).Pic = Buffer.ReadInt32()
            Item(n).Price = Buffer.ReadInt32()
            Item(n).Rarity = Buffer.ReadInt32()
            Item(n).Speed = Buffer.ReadInt32()

            Item(n).Randomize = Buffer.ReadInt32()
            Item(n).RandomMin = Buffer.ReadInt32()
            Item(n).RandomMax = Buffer.ReadInt32()

            Item(n).Stackable = Buffer.ReadInt32()
            Item(n).Description = Trim$(buffer.ReadString())

            For z = 0 To Stats.Count - 1
                Item(n).Stat_Req(z) = Buffer.ReadInt32()
            Next

            Item(n).Type = Buffer.ReadInt32()
            Item(n).SubType = Buffer.ReadInt32()

            Item(n).ItemLevel = Buffer.ReadInt32()

            'Housing
            Item(n).FurnitureWidth = Buffer.ReadInt32()
            Item(n).FurnitureHeight = Buffer.ReadInt32()

            For a = 1 To 3
                For b = 1 To 3
                    Item(n).FurnitureBlocks(a, b) = Buffer.ReadInt32()
                    Item(n).FurnitureFringe(a, b) = Buffer.ReadInt32()
                Next
            Next

            Item(n).KnockBack = Buffer.ReadInt32()
            Item(n).KnockBackTiles = Buffer.ReadInt32()

            Item(n).Projectile = Buffer.ReadInt32()
            Item(n).Ammo = Buffer.ReadInt32()
        Next

        i = 0
        n = 0
        x = 0
        z = 0

        '\\\End Read Item Data\\\\\\\

        '\\\Read Animation Data\\\\\\\
        x = Buffer.ReadInt32

        For i = 1 To x
            n = Buffer.ReadInt32
            ' Update the Animation
            For z = 0 To UBound(Animation(n).Frames)
                Animation(n).Frames(z) = Buffer.ReadInt32()
            Next

            For z = 0 To UBound(Animation(n).LoopCount)
                Animation(n).LoopCount(z) = Buffer.ReadInt32()
            Next

            For z = 0 To UBound(Animation(n).LoopTime)
                Animation(n).LoopTime(z) = Buffer.ReadInt32()
            Next

            Animation(n).Name = Trim(buffer.ReadString)
            Animation(n).Sound = Trim(buffer.ReadString)

            If Animation(n).Name Is Nothing Then Animation(n).Name = ""
            If Animation(n).Sound Is Nothing Then Animation(n).Sound = ""

            For z = 0 To UBound(Animation(n).Sprite)
                Animation(n).Sprite(z) = Buffer.ReadInt32()
            Next
        Next

        i = 0
        n = 0
        x = 0
        z = 0

        '\\\End Read Animation Data\\\\\\\

        '\\\Read NPC Data\\\\\\\
        x = Buffer.ReadInt32
        For i = 1 To x
            n = Buffer.ReadInt32
            ' Update the Npc
            Npc(n).Animation = Buffer.ReadInt32()
            Npc(n).AttackSay = Trim(buffer.ReadString())
            Npc(n).Behaviour = Buffer.ReadInt32()
            For z = 1 To 5
                Npc(n).DropChance(z) = Buffer.ReadInt32()
                Npc(n).DropItem(z) = Buffer.ReadInt32()
                Npc(n).DropItemValue(z) = Buffer.ReadInt32()
            Next

            Npc(n).Exp = Buffer.ReadInt32()
            Npc(n).Faction = Buffer.ReadInt32()
            Npc(n).Hp = Buffer.ReadInt32()
            Npc(n).Name = Trim(buffer.ReadString())
            Npc(n).Range = Buffer.ReadInt32()
            Npc(n).SpawnTime = Buffer.ReadInt32()
            Npc(n).SpawnSecs = Buffer.ReadInt32()
            Npc(n).Sprite = Buffer.ReadInt32()

            For z = 0 To Stats.Count - 1
                Npc(n).Stat(z) = Buffer.ReadInt32()
            Next

            Npc(n).QuestNum = Buffer.ReadInt32()

            ReDim Npc(n).Skill(MAX_NPC_SKILLS)
            For z = 1 To MAX_NPC_SKILLS
                Npc(n).Skill(z) = Buffer.ReadInt32()
            Next

            Npc(i).Level = Buffer.ReadInt32()
            Npc(i).Damage = Buffer.ReadInt32()

            If Npc(n).AttackSay Is Nothing Then Npc(n).AttackSay = ""
            If Npc(n).Name Is Nothing Then Npc(n).Name = ""
        Next

        i = 0
        n = 0
        x = 0
        z = 0

        '\\\End Read NPC Data\\\\\\\

        '\\\Read Shop Data\\\\\\\
        x = Buffer.ReadInt32

        For i = 1 To x
            n = Buffer.ReadInt32

            Shop(n).BuyRate = Buffer.ReadInt32()
            Shop(n).Name = Trim(buffer.ReadString())
            Shop(n).Face = Buffer.ReadInt32()

            For z = 0 To MAX_TRADES
                Shop(n).TradeItem(z).CostItem = Buffer.ReadInt32()
                Shop(n).TradeItem(z).CostValue = Buffer.ReadInt32()
                Shop(n).TradeItem(z).Item = Buffer.ReadInt32()
                Shop(n).TradeItem(z).ItemValue = Buffer.ReadInt32()
            Next

            If Shop(n).Name Is Nothing Then Shop(n).Name = ""
        Next

        i = 0
        n = 0
        x = 0
        z = 0

        '\\\End Read Shop Data\\\\\\\

        '\\\Read Skills Data\\\\\\\\\\
        x = Buffer.ReadInt32

        For i = 1 To x
            n = Buffer.ReadInt32

            Skill(n).AccessReq = Buffer.ReadInt32()
            Skill(n).AoE = Buffer.ReadInt32()
            Skill(n).CastAnim = Buffer.ReadInt32()
            Skill(n).CastTime = Buffer.ReadInt32()
            Skill(n).CdTime = Buffer.ReadInt32()
            Skill(n).ClassReq = Buffer.ReadInt32()
            Skill(n).Dir = Buffer.ReadInt32()
            Skill(n).Duration = Buffer.ReadInt32()
            Skill(n).Icon = Buffer.ReadInt32()
            Skill(n).Interval = Buffer.ReadInt32()
            Skill(n).IsAoE = Buffer.ReadInt32()
            Skill(n).LevelReq = Buffer.ReadInt32()
            Skill(n).Map = Buffer.ReadInt32()
            Skill(n).MpCost = Buffer.ReadInt32()
            Skill(n).Name = Trim(buffer.ReadString())
            Skill(n).Range = Buffer.ReadInt32()
            Skill(n).SkillAnim = Buffer.ReadInt32()
            Skill(n).StunDuration = Buffer.ReadInt32()
            Skill(n).Type = Buffer.ReadInt32()
            Skill(n).Vital = Buffer.ReadInt32()
            Skill(n).X = Buffer.ReadInt32()
            Skill(n).Y = Buffer.ReadInt32()

            Skill(n).IsProjectile = Buffer.ReadInt32()
            Skill(n).Projectile = Buffer.ReadInt32()

            Skill(n).KnockBack = Buffer.ReadInt32()
            Skill(n).KnockBackTiles = Buffer.ReadInt32()

            If Skill(n).Name Is Nothing Then Skill(n).Name = ""
        Next

        i = 0
        x = 0
        n = 0
        z = 0

        '\\\End Read Skills Data\\\\\\\\\\

        '\\\Read Resource Data\\\\\\\\\\\\
        x = Buffer.ReadInt32

        For i = 1 To x
            n = Buffer.ReadInt32

            Resource(n).Animation = Buffer.ReadInt32()
            Resource(n).EmptyMessage = Trim(buffer.ReadString())
            Resource(n).ExhaustedImage = Buffer.ReadInt32()
            Resource(n).Health = Buffer.ReadInt32()
            Resource(n).ExpReward = Buffer.ReadInt32()
            Resource(n).ItemReward = Buffer.ReadInt32()
            Resource(n).Name = Trim(buffer.ReadString())
            Resource(n).ResourceImage = Buffer.ReadInt32()
            Resource(n).ResourceType = Buffer.ReadInt32()
            Resource(n).RespawnTime = Buffer.ReadInt32()
            Resource(n).SuccessMessage = Trim(buffer.ReadString())
            Resource(n).LvlRequired = Buffer.ReadInt32()
            Resource(n).ToolRequired = Buffer.ReadInt32()
            Resource(n).Walkthrough = Buffer.ReadInt32()

            If Resource(n).Name Is Nothing Then Resource(n).Name = ""
            If Resource(n).EmptyMessage Is Nothing Then Resource(n).EmptyMessage = ""
            If Resource(n).SuccessMessage Is Nothing Then Resource(n).SuccessMessage = ""
        Next

        i = 0
        n = 0
        x = 0
        z = 0

        '\\\End Read Resource Data\\\\\\\\\\\\

        Buffer.Dispose
    End Sub

    Private Sub Packet_Mapreport(ByVal Data() As Byte)
        Dim I As Integer
        Dim Buffer As New ByteStream(Data)
        For I = 1 To MAX_MAPS
            MapNames(I) = Trim(Buffer.ReadString())
        Next

        UpdateMapnames = True

        Buffer.Dispose
    End Sub

    Private Sub Packet_MapNames(ByVal Data() As Byte)
        Dim I As Integer
        Dim Buffer As New ByteStream(Data)
        For I = 1 To MAX_MAPS
            MapNames(I) = Trim(Buffer.ReadString())
        Next

        Buffer.Dispose
    End Sub

    Private Sub Packet_ClassEditor(ByVal Data() As Byte)
        InitClassEditor = True
    End Sub

    Private Sub Packet_AutoMapper(ByVal Data() As Byte)
        Dim Layer As Integer
        Dim Buffer As New ByteStream(Data)
        MapStart = Buffer.ReadInt32
        MapSize = Buffer.ReadInt32
        MapX = Buffer.ReadInt32
        MapY = Buffer.ReadInt32
        SandBorder = Buffer.ReadInt32
        DetailFreq = Buffer.ReadInt32
        ResourceFreq = Buffer.ReadInt32

        Dim myXml As New XmlClass With {
            .Filename = System.IO.Path.Combine(Application.StartupPath, "Data", "AutoMapper.xml"),
            .Root = "Options"
        }

        myXml.WriteString("Resources", "ResourcesNum", Buffer.ReadString())

        For Prefab = 1 To TilePrefab.Count - 1
            ReDim Tile(Prefab).Layer(0 To MapLayer.Count - 1)

            Layer = Buffer.ReadInt32()
            myXml.WriteString("Prefab" & Prefab, "Layer" & Layer & "Tileset", Buffer.ReadInt32)
            myXml.WriteString("Prefab" & Prefab, "Layer" & Layer & "X", Buffer.ReadInt32)
            myXml.WriteString("Prefab" & Prefab, "Layer" & Layer & "Y", Buffer.ReadInt32)
            myXml.WriteString("Prefab" & Prefab, "Layer" & Layer & "Autotile", Buffer.ReadInt32)

            myXml.WriteString("Prefab" & Prefab, "Type", Buffer.ReadInt32)
        Next

        Buffer.Dispose

        InitAutoMapper = True

    End Sub
End Module