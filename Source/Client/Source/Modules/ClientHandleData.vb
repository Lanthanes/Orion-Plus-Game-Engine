Imports ASFW
Imports ASFW.IO

Module ClientHandleData
    Public PlayerBuffer As ByteBuffer
    Private Delegate Sub Packet_(ByVal Data() As Byte)
    Private Packets As Dictionary(Of Integer, Packet_)

    Public Sub HandleData(ByVal data() As Byte)
        Dim pLength As Integer

        If PlayerBuffer Is Nothing Then
            PlayerBuffer = New ByteBuffer
        End If
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
        Packets.Add(ServerPackets.SLoadCharOk, AddressOf Packet_LoadCharOk)
        Packets.Add(ServerPackets.SLoginOk, AddressOf Packet_LoginOk)
        Packets.Add(ServerPackets.SNewCharClasses, AddressOf Packet_NewCharClasses)
        Packets.Add(ServerPackets.SClassesData, AddressOf Packet_ClassesData)
        Packets.Add(ServerPackets.SInGame, AddressOf Packet_InGame)
        Packets.Add(ServerPackets.SPlayerInv, AddressOf Packet_PlayerInv)
        Packets.Add(ServerPackets.SPlayerInvUpdate, AddressOf Packet_PlayerInvUpdate)
        Packets.Add(ServerPackets.SPlayerWornEq, AddressOf Packet_PlayerWornEquipment)
        Packets.Add(ServerPackets.SPlayerHp, AddressOf Packet_PlayerHP)
        Packets.Add(ServerPackets.SPlayerMp, AddressOf Packet_PlayerMP)
        Packets.Add(ServerPackets.SPlayerSp, AddressOf Packet_PlayerSP)
        Packets.Add(ServerPackets.SPlayerStats, AddressOf Packet_PlayerStats)
        Packets.Add(ServerPackets.SPlayerData, AddressOf Packet_PlayerData)
        Packets.Add(ServerPackets.SPlayerMove, AddressOf Packet_PlayerMove)
        Packets.Add(ServerPackets.SNpcMove, AddressOf Packet_NpcMove)
        Packets.Add(ServerPackets.SPlayerDir, AddressOf Packet_PlayerDir)
        Packets.Add(ServerPackets.SNpcDir, AddressOf Packet_NpcDir)
        Packets.Add(ServerPackets.SPlayerXY, AddressOf Packet_PlayerXY)
        Packets.Add(ServerPackets.SAttack, AddressOf Packet_Attack)
        Packets.Add(ServerPackets.SNpcAttack, AddressOf Packet_NpcAttack)
        Packets.Add(ServerPackets.SCheckForMap, AddressOf Packet_CheckMap)
        Packets.Add(ServerPackets.SMapData, AddressOf Packet_MapData)
        Packets.Add(ServerPackets.SMapNpcData, AddressOf Packet_MapNPCData)
        Packets.Add(ServerPackets.SMapNpcUpdate, AddressOf Packet_MapNPCUpdate)
        Packets.Add(ServerPackets.SMapDone, AddressOf Packet_MapDone)
        Packets.Add(ServerPackets.SGlobalMsg, AddressOf Packet_GlobalMessage)
        Packets.Add(ServerPackets.SPlayerMsg, AddressOf Packet_PlayerMessage)
        Packets.Add(ServerPackets.SMapMsg, AddressOf Packet_MapMessage)
        Packets.Add(ServerPackets.SSpawnItem, AddressOf Packet_SpawnItem)
        Packets.Add(ServerPackets.SUpdateItem, AddressOf Packet_UpdateItem)
        Packets.Add(ServerPackets.SSpawnNpc, AddressOf Packet_SpawnNPC)
        Packets.Add(ServerPackets.SNpcDead, AddressOf Packet_NpcDead)
        Packets.Add(ServerPackets.SUpdateNpc, AddressOf Packet_UpdateNPC)
        Packets.Add(ServerPackets.SMapKey, AddressOf Packet_MapKey)
        Packets.Add(ServerPackets.SEditMap, AddressOf Packet_EditMap)
        Packets.Add(ServerPackets.SUpdateShop, AddressOf Packet_UpdateShop)
        Packets.Add(ServerPackets.SUpdateSkill, AddressOf Packet_UpdateSkill)
        Packets.Add(ServerPackets.SSkills, AddressOf Packet_Skills)
        Packets.Add(ServerPackets.SLeftMap, AddressOf Packet_LeftMap)
        Packets.Add(ServerPackets.SResourceCache, AddressOf Packet_ResourceCache)
        Packets.Add(ServerPackets.SUpdateResource, AddressOf Packet_UpdateResource)
        Packets.Add(ServerPackets.SSendPing, AddressOf Packet_Ping)
        Packets.Add(ServerPackets.SDoorAnimation, AddressOf Packet_DoorAnimation)
        Packets.Add(ServerPackets.SActionMsg, AddressOf Packet_ActionMessage)
        Packets.Add(ServerPackets.SPlayerEXP, AddressOf Packet_PlayerExp)
        Packets.Add(ServerPackets.SBlood, AddressOf Packet_Blood)
        Packets.Add(ServerPackets.SUpdateAnimation, AddressOf Packet_UpdateAnimation)
        Packets.Add(ServerPackets.SAnimation, AddressOf Packet_Animation)
        Packets.Add(ServerPackets.SMapNpcVitals, AddressOf Packet_NPCVitals)
        Packets.Add(ServerPackets.SCooldown, AddressOf Packet_Cooldown)
        Packets.Add(ServerPackets.SClearSkillBuffer, AddressOf Packet_ClearSkillBuffer)
        Packets.Add(ServerPackets.SSayMsg, AddressOf Packet_SayMessage)
        Packets.Add(ServerPackets.SOpenShop, AddressOf Packet_OpenShop)
        Packets.Add(ServerPackets.SResetShopAction, AddressOf Packet_ResetShopAction)
        Packets.Add(ServerPackets.SStunned, AddressOf Packet_Stunned)
        Packets.Add(ServerPackets.SMapWornEq, AddressOf Packet_MapWornEquipment)
        Packets.Add(ServerPackets.SBank, AddressOf Packet_OpenBank)
        Packets.Add(ServerPackets.SLeftGame, AddressOf Packet_LeftGame)

        Packets.Add(ServerPackets.SClearTradeTimer, AddressOf Packet_ClearTradeTimer)
        Packets.Add(ServerPackets.STradeInvite, AddressOf Packet_TradeInvite)
        Packets.Add(ServerPackets.STrade, AddressOf Packet_Trade)
        Packets.Add(ServerPackets.SCloseTrade, AddressOf Packet_CloseTrade)
        Packets.Add(ServerPackets.STradeUpdate, AddressOf Packet_TradeUpdate)
        Packets.Add(ServerPackets.STradeStatus, AddressOf Packet_TradeStatus)

        Packets.Add(ServerPackets.SGameData, AddressOf Packet_GameData)
        Packets.Add(ServerPackets.SMapReport, AddressOf Packet_Mapreport) 'Mapreport
        Packets.Add(ServerPackets.STarget, AddressOf Packet_Target)

        Packets.Add(ServerPackets.SAdmin, AddressOf Packet_Admin)
        Packets.Add(ServerPackets.SMapNames, AddressOf Packet_MapNames)

        Packets.Add(ServerPackets.SCritical, AddressOf Packet_Critical)
        Packets.Add(ServerPackets.SNews, AddressOf Packet_News)
        Packets.Add(ServerPackets.SrClick, AddressOf Packet_RClick)
        Packets.Add(ServerPackets.STotalOnline, AddressOf Packet_TotalOnline)

        'quests
        Packets.Add(ServerPackets.SUpdateQuest, AddressOf Packet_UpdateQuest)
        Packets.Add(ServerPackets.SPlayerQuest, AddressOf Packet_PlayerQuest)
        Packets.Add(ServerPackets.SPlayerQuests, AddressOf Packet_PlayerQuests)
        Packets.Add(ServerPackets.SQuestMessage, AddressOf Packet_QuestMessage)

        'Housing
        Packets.Add(ServerPackets.SHouseConfigs, AddressOf Packet_HouseConfigurations)
        Packets.Add(ServerPackets.SBuyHouse, AddressOf Packet_HouseOffer)
        Packets.Add(ServerPackets.SVisit, AddressOf Packet_Visit)
        Packets.Add(ServerPackets.SFurniture, AddressOf Packet_Furniture)

        'hotbar
        Packets.Add(ServerPackets.SHotbar, AddressOf Packet_Hotbar)

        'Events
        Packets.Add(ServerPackets.SSpawnEvent, AddressOf Packet_SpawnEvent)
        Packets.Add(ServerPackets.SEventMove, AddressOf Packet_EventMove)
        Packets.Add(ServerPackets.SEventDir, AddressOf Packet_EventDir)
        Packets.Add(ServerPackets.SEventChat, AddressOf Packet_EventChat)
        Packets.Add(ServerPackets.SEventStart, AddressOf Packet_EventStart)
        Packets.Add(ServerPackets.SEventEnd, AddressOf Packet_EventEnd)
        Packets.Add(ServerPackets.SPlayBGM, AddressOf Packet_PlayBGM)
        Packets.Add(ServerPackets.SPlaySound, AddressOf Packet_PlaySound)
        Packets.Add(ServerPackets.SFadeoutBGM, AddressOf Packet_FadeOutBGM)
        Packets.Add(ServerPackets.SStopSound, AddressOf Packet_StopSound)
        Packets.Add(ServerPackets.SSwitchesAndVariables, AddressOf Packet_SwitchesAndVariables)
        Packets.Add(ServerPackets.SMapEventData, AddressOf Packet_MapEventData)
        'SChatBubble
        Packets.Add(ServerPackets.SChatBubble, AddressOf Packet_ChatBubble)
        Packets.Add(ServerPackets.SSpecialEffect, AddressOf Packet_SpecialEffect)
        'SPic
        Packets.Add(ServerPackets.SHoldPlayer, AddressOf Packet_HoldPlayer)

        Packets.Add(ServerPackets.SUpdateProjectile, AddressOf HandleUpdateProjectile)
        Packets.Add(ServerPackets.SMapProjectile, AddressOf HandleMapProjectile)

        'craft
        Packets.Add(ServerPackets.SUpdateRecipe, AddressOf Packet_UpdateRecipe)
        Packets.Add(ServerPackets.SSendPlayerRecipe, AddressOf Packet_SendPlayerRecipe)
        Packets.Add(ServerPackets.SOpenCraft, AddressOf Packet_OpenCraft)
        Packets.Add(ServerPackets.SUpdateCraft, AddressOf Packet_UpdateCraft)

        'emotes
        Packets.Add(ServerPackets.SEmote, AddressOf Packet_Emote)

        'party
        Packets.Add(ServerPackets.SPartyInvite, AddressOf Packet_PartyInvite)
        Packets.Add(ServerPackets.SPartyUpdate, AddressOf Packet_PartyUpdate)
        Packets.Add(ServerPackets.SPartyVitals, AddressOf Packet_PartyVitals)

        'pets
        Packets.Add(ServerPackets.SUpdatePet, AddressOf Packet_UpdatePet)
        Packets.Add(ServerPackets.SUpdatePlayerPet, AddressOf Packet_UpdatePlayerPet)
        Packets.Add(ServerPackets.SPetMove, AddressOf Packet_PetMove)
        Packets.Add(ServerPackets.SPetDir, AddressOf Packet_PetDir)
        Packets.Add(ServerPackets.SPetVital, AddressOf Packet_PetVital)
        Packets.Add(ServerPackets.SClearPetSkillBuffer, AddressOf Packet_ClearPetSkillBuffer)
        Packets.Add(ServerPackets.SPetAttack, AddressOf Packet_PetAttack)
        Packets.Add(ServerPackets.SPetXY, AddressOf Packet_PetXY)
        Packets.Add(ServerPackets.SPetExp, AddressOf Packet_PetExperience)

        Packets.Add(ServerPackets.SClock, AddressOf Packet_Clock)
        Packets.Add(ServerPackets.STime, AddressOf Packet_Time)
    End Sub

    Sub HandleDataPackets(ByVal data() As Byte)
        Dim packetnum As Integer, Packet As Packet_ = Nothing
        packetnum = BitConverter.ToInt32(data, 0)

        Try
            If Packets.TryGetValue(packetnum, Packet) Then
                Dim bytes As Byte() : ReDim bytes(data.Length - 5)
                Buffer.BlockCopy(data, 4, bytes, 0, bytes.Length)
                Packet.Invoke(bytes)
            End If
        Catch
            MsgBox("PacketCrash: " & CType(packetnum, ServerPackets).ToString())
        End Try
    End Sub

    Sub Packet_AlertMSG(ByVal data() As Byte)
        Dim Msg As String
        Dim Buffer As New ByteStream(data)
        pnlloadvisible = False

        If FrmMenu.Visible = False Then
            frmmenuvisible = True
            frmmaingamevisible = False
        End If

        pnlCharCreateVisible = False
        pnlLoginVisible = False
        pnlRegisterVisible = False
        pnlCharSelectVisible = False

        Msg = Trim(Buffer.ReadString)

        Buffer.Dispose()

        MsgBox(Msg, vbOKOnly, GAME_NAME)
        DestroyGame()
    End Sub

    Sub Packet_KeyPair(ByVal Data() As Byte)
        Dim Buffer As New ByteStream(Data)
        EKeyPair.ImportKeyString(Buffer.ReadString())
        Buffer.Dispose()
    End Sub

    Sub Packet_LoadCharOk(ByVal Data() As Byte)
        Dim Buffer As New ByteStream(Data)
        ' Now we can receive game data
        MyIndex = Buffer.ReadInt32

        Buffer.Dispose()

        pnlloadvisible = True
        SetStatus(Strings.Get("gamegui", "datarecieve"))
    End Sub

    Sub Packet_LoginOk(ByVal Data() As Byte)
        Dim CharName As String, Sprite As Integer
        Dim Level As Integer, ClassName As String, Gender As Byte

        Dim Buffer As New ByteStream(Data)
        ' save options
        Options.SavePass = chkSavePassChecked
        Options.Username = Trim$(tempUserName)

        If chkSavePassChecked = False Then
            Options.Password = ""
        Else
            Options.Password = Trim$(tempPassword)
        End If

        SaveOptions()

        ' Request classes.
        SendRequestClasses()

        ' Now we can receive char data
        MaxChars = Buffer.ReadInt32
        ReDim CharSelection(MaxChars)

        SelectedChar = 1

        'reset for deleting chars
        For i = 1 To MaxChars
            CharSelection(i).Name = ""
            CharSelection(i).Sprite = 0
            CharSelection(i).Level = 0
            CharSelection(i).ClassName = ""
            CharSelection(i).Gender = 0
        Next

        For i = 1 To MaxChars
            CharName = Buffer.ReadString
            Sprite = Buffer.ReadInt32
            Level = Buffer.ReadInt32
            ClassName = Buffer.ReadString
            Gender = Buffer.ReadInt32

            CharSelection(i).Name = CharName
            CharSelection(i).Sprite = Sprite
            CharSelection(i).Level = Level
            CharSelection(i).ClassName = ClassName
            CharSelection(i).Gender = Gender
        Next

        Buffer.Dispose()

        ' Used for if the player is creating a new character
        frmmenuvisible = True
        pnlloadvisible = False
        pnlCreditsVisible = False
        pnlRegisterVisible = False
        pnlCharCreateVisible = False
        pnlLoginVisible = False

        pnlCharSelectVisible = True

        FrmMenu.DrawCharacter()

        DrawCharSelect = True

    End Sub

    Sub Packet_NewCharClasses(ByVal data() As Byte)
        Dim i As Integer, z As Integer, X As Integer
        Dim Buffer As New ByteStream(data)
        ' Max classes
        Max_Classes = Buffer.ReadInt32
        ReDim Classes(0 To Max_Classes)

        SelectedChar = 1

        For i = 1 To Max_Classes

            With Classes(i)
                .Name = Trim(Buffer.ReadString)
                .Desc = Trim(Buffer.ReadString)

                ReDim .Vital(0 To Vitals.Count - 1)

                .Vital(Vitals.HP) = Buffer.ReadInt32
                .Vital(Vitals.MP) = Buffer.ReadInt32
                .Vital(Vitals.SP) = Buffer.ReadInt32

                ' get array size
                z = Buffer.ReadInt32
                ' redim array
                ReDim .MaleSprite(0 To z + 1)
                ' loop-receive data
                For X = 1 To z + 1
                    .MaleSprite(X) = Buffer.ReadInt32
                Next

                ' get array size
                z = Buffer.ReadInt32
                ' redim array
                ReDim .FemaleSprite(0 To z + 1)
                ' loop-receive data
                For X = 1 To z + 1
                    .FemaleSprite(X) = Buffer.ReadInt32
                Next

                ReDim .Stat(0 To Stats.Count - 1)

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

        Buffer.Dispose()

        ' Used for if the player is creating a new character
        frmmenuvisible = True
        pnlloadvisible = False
        pnlCreditsVisible = False
        pnlRegisterVisible = False
        pnlCharCreateVisible = True
        pnlLoginVisible = False

        ReDim cmbclass(0 To Max_Classes)

        For i = 1 To Max_Classes
            cmbclass(i) = Classes(i).Name
        Next

        FrmMenu.DrawCharacter()

        newCharSprite = 1
    End Sub

    Private Sub Packet_ClassesData(ByVal data() As Byte)
        Dim i As Integer, z As Integer, X As Integer
        Dim Buffer As New ByteStream(data)
        ' Max classes
        Max_Classes = Buffer.ReadInt32
        ReDim Classes(0 To Max_Classes)

        SelectedChar = 1

        For i = 1 To Max_Classes

            With Classes(i)
                .Name = Trim(Buffer.ReadString)
                .Desc = Trim(Buffer.ReadString)

                ReDim .Vital(0 To Vitals.Count - 1)

                .Vital(Vitals.HP) = Buffer.ReadInt32
                .Vital(Vitals.MP) = Buffer.ReadInt32
                .Vital(Vitals.SP) = Buffer.ReadInt32

                ' get array size
                z = Buffer.ReadInt32
                ' redim array
                ReDim .MaleSprite(0 To z + 1)
                ' loop-receive data
                For X = 1 To z + 1
                    .MaleSprite(X) = Buffer.ReadInt32
                Next

                ' get array size
                z = Buffer.ReadInt32
                ' redim array
                ReDim .FemaleSprite(0 To z + 1)
                ' loop-receive data
                For X = 1 To z + 1
                    .FemaleSprite(X) = Buffer.ReadInt32
                Next

                ReDim .Stat(0 To Stats.Count - 1)

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

        ReDim cmbclass(0 To Max_Classes)
        For i = 1 To Max_Classes
            cmbclass(i) = Classes(i).Name
        Next
        FrmMenu.DrawCharacter()
        newCharSprite = 1

        Buffer.Dispose()
    End Sub

    Private Sub Packet_InGame(ByVal data() As Byte)
        InGame = True
        CanMoveNow = True
        GameInit()
    End Sub

    Private Sub Packet_PlayerInv(ByVal data() As Byte)
        Dim i As Integer, InvNum As Integer, Amount As Integer
        Dim Buffer As New ByteStream(data)
        For i = 1 To MAX_INV
            InvNum = Buffer.ReadInt32
            Amount = Buffer.ReadInt32
            SetPlayerInvItemNum(MyIndex, i, InvNum)
            SetPlayerInvItemValue(MyIndex, i, Amount)

            Player(MyIndex).RandInv(i).Prefix = Buffer.ReadString
            Player(MyIndex).RandInv(i).Suffix = Buffer.ReadString
            Player(MyIndex).RandInv(i).Rarity = Buffer.ReadInt32
            For n = 1 To Stats.Count - 1
                Player(MyIndex).RandInv(i).Stat(n) = Buffer.ReadInt32
            Next
            Player(MyIndex).RandInv(i).Damage = Buffer.ReadInt32
            Player(MyIndex).RandInv(i).Speed = Buffer.ReadInt32
        Next

        ' changes to inventory, need to clear any drop menu
        FrmMainGame.pnlCurrency.Visible = False
        FrmMainGame.txtCurrency.Text = ""
        tmpCurrencyItem = 0
        CurrencyMenu = 0 ' clear

        Buffer.Dispose()
    End Sub

    Private Sub Packet_PlayerInvUpdate(ByVal data() As Byte)
        Dim n As Integer, i As Integer
        Dim Buffer As New ByteStream(data)
        n = Buffer.ReadInt32
        SetPlayerInvItemNum(MyIndex, n, Buffer.ReadInt32)
        SetPlayerInvItemValue(MyIndex, n, Buffer.ReadInt32)

        Player(MyIndex).RandInv(n).Prefix = Buffer.ReadString
        Player(MyIndex).RandInv(n).Suffix = Buffer.ReadString
        Player(MyIndex).RandInv(n).Rarity = Buffer.ReadInt32
        For i = 1 To Stats.Count - 1
            Player(MyIndex).RandInv(n).Stat(i) = Buffer.ReadInt32
        Next
        Player(MyIndex).RandInv(n).Damage = Buffer.ReadInt32
        Player(MyIndex).RandInv(n).Speed = Buffer.ReadInt32

        ' changes, clear drop menu
        FrmMainGame.pnlCurrency.Visible = False
        FrmMainGame.txtCurrency.Text = ""
        tmpCurrencyItem = 0
        CurrencyMenu = 0 ' clear

        Buffer.Dispose()
    End Sub

    Private Sub Packet_PlayerWornEquipment(ByVal data() As Byte)
        Dim i As Integer, n As Integer
        Dim Buffer As New ByteStream(data)
        For i = 1 To EquipmentType.Count - 1
            SetPlayerEquipment(MyIndex, Buffer.ReadInt32, i)
        Next

        For i = 1 To EquipmentType.Count - 1
            Player(MyIndex).RandEquip(i).Prefix = Buffer.ReadString
            Player(MyIndex).RandEquip(i).Suffix = Buffer.ReadString
            Player(MyIndex).RandEquip(i).Damage = Buffer.ReadInt32
            Player(MyIndex).RandEquip(i).Speed = Buffer.ReadInt32
            Player(MyIndex).RandEquip(i).Rarity = Buffer.ReadInt32

            For n = 1 To Stats.Count - 1
                Player(MyIndex).RandEquip(i).Stat(n) = Buffer.ReadInt32
            Next
        Next

        ' changes to inventory, need to clear any drop menu

        FrmMainGame.pnlCurrency.Visible = False
        FrmMainGame.txtCurrency.Text = ""
        tmpCurrencyItem = 0
        CurrencyMenu = 0 ' clear

        Buffer.Dispose()
    End Sub

    Private Sub Packet_PlayerHP(ByVal data() As Byte)
        Dim Buffer As New ByteStream(data)
        Player(MyIndex).MaxHP = Buffer.ReadInt32

        SetPlayerVital(MyIndex, Vitals.HP, Buffer.ReadInt32)

        If GetPlayerMaxVital(MyIndex, Vitals.HP) > 0 Then
            lblHPText = GetPlayerVital(MyIndex, Vitals.HP) & "/" & GetPlayerMaxVital(MyIndex, Vitals.HP)
            ' hp bar
            picHpWidth = Int(((GetPlayerVital(MyIndex, Vitals.HP) / 169) / (GetPlayerMaxVital(MyIndex, Vitals.HP) / 169)) * 169)
        End If

        Buffer.Dispose()
    End Sub

    Private Sub Packet_PlayerMP(ByVal data() As Byte)
        Dim Buffer As New ByteStream(data)
        Player(MyIndex).MaxMP = Buffer.ReadInt32
        SetPlayerVital(MyIndex, Vitals.MP, Buffer.ReadInt32)

        If GetPlayerMaxVital(MyIndex, Vitals.MP) > 0 Then
            lblManaText = GetPlayerVital(MyIndex, Vitals.MP) & "/" & GetPlayerMaxVital(MyIndex, Vitals.MP)
            ' mp bar
            picManaWidth = Int(((GetPlayerVital(MyIndex, Vitals.MP) / 169) / (GetPlayerMaxVital(MyIndex, Vitals.MP) / 169)) * 169)
        End If

        Buffer.Dispose()
    End Sub

    Private Sub Packet_PlayerSP(ByVal data() As Byte)
        Dim Buffer As New ByteStream(data)
        Player(MyIndex).MaxSP = Buffer.ReadInt32
        SetPlayerVital(MyIndex, Vitals.SP, Buffer.ReadInt32)

        Buffer.Dispose()
    End Sub

    Private Sub Packet_PlayerStats(ByVal data() As Byte)
        Dim i As Integer, index As Integer
        Dim Buffer As New ByteStream(data)
        index = Buffer.ReadInt32
        For i = 1 To Stats.Count - 1
            SetPlayerStat(index, i, Buffer.ReadInt32)
        Next
        UpdateCharacterPanel = True

        Buffer.Dispose()
    End Sub

    Private Sub Packet_PlayerData(ByVal Data() As Byte)
        Dim i As Integer, X As Integer
        Dim Buffer As New ByteStream(Data)
        i = Buffer.ReadInt32
        SetPlayerName(i, Buffer.ReadString)
        SetPlayerClass(i, Buffer.ReadInt32)
        SetPlayerLevel(i, Buffer.ReadInt32)
        SetPlayerPOINTS(i, Buffer.ReadInt32)
        SetPlayerSprite(i, Buffer.ReadInt32)
        SetPlayerMap(i, Buffer.ReadInt32)
        SetPlayerX(i, Buffer.ReadInt32)
        SetPlayerY(i, Buffer.ReadInt32)
        SetPlayerDir(i, Buffer.ReadInt32)
        SetPlayerAccess(i, Buffer.ReadInt32)
        SetPlayerPK(i, Buffer.ReadInt32)

        For X = 1 To Stats.Count - 1
            SetPlayerStat(i, X, Buffer.ReadInt32)
        Next

        Player(i).InHouse = Buffer.ReadInt32

        For X = 0 To ResourceSkills.Count - 1
            Player(i).GatherSkills(X).SkillLevel = Buffer.ReadInt32
            Player(i).GatherSkills(X).SkillCurExp = Buffer.ReadInt32
            Player(i).GatherSkills(X).SkillNextLvlExp = Buffer.ReadInt32
        Next

        For X = 1 To MAX_RECIPE
            Player(i).RecipeLearned(X) = Buffer.ReadInt32
        Next

        ' Check if the player is the client player
        If i = MyIndex Then
            ' Reset directions
            DirUp = False
            DirDown = False
            DirLeft = False
            DirRight = False

            UpdateCharacterPanel = True
        End If

        ' Make sure they aren't walking
        Player(i).Moving = 0
        Player(i).XOffset = 0
        Player(i).YOffset = 0

        If i = MyIndex Then PlayerData = True

        Buffer.Dispose()
    End Sub

    Private Sub Packet_PlayerMove(ByVal Data() As Byte)
        Dim i As Integer, X As Integer, Y As Integer
        Dim Dir As Integer, n As Byte
        Dim Buffer As New ByteStream(Data)
        i = Buffer.ReadInt32
        X = Buffer.ReadInt32
        Y = Buffer.ReadInt32
        Dir = Buffer.ReadInt32
        n = Buffer.ReadInt32

        SetPlayerX(i, X)
        SetPlayerY(i, Y)
        SetPlayerDir(i, Dir)
        Player(i).XOffset = 0
        Player(i).YOffset = 0
        Player(i).Moving = n

        Select Case GetPlayerDir(i)
            Case Direction.Up
                Player(i).YOffset = PIC_Y
            Case Direction.Down
                Player(i).YOffset = PIC_Y * -1
            Case Direction.Left
                Player(i).XOffset = PIC_X
            Case Direction.Right
                Player(i).XOffset = PIC_X * -1
        End Select

        Buffer.Dispose()
    End Sub

    Private Sub Packet_NpcMove(ByVal Data() As Byte)
        Dim MapNpcNum As Integer, Movement As Integer
        Dim X As Integer, Y As Integer, Dir As Integer
        Dim Buffer As New ByteStream(Data)
        MapNpcNum = Buffer.ReadInt32
        X = Buffer.ReadInt32
        Y = Buffer.ReadInt32
        Dir = Buffer.ReadInt32
        Movement = Buffer.ReadInt32

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

        Buffer.Dispose()
    End Sub

    Private Sub Packet_PlayerDir(ByVal Data() As Byte)
        Dim Dir As Integer, i As Integer
        Dim Buffer As New ByteStream(Data)
        i = Buffer.ReadInt32
        Dir = Buffer.ReadInt32

        SetPlayerDir(i, Dir)

        With Player(i)
            .XOffset = 0
            .YOffset = 0
            .Moving = 0
        End With

        Buffer.Dispose()
    End Sub

    Private Sub Packet_NpcDir(ByVal Data() As Byte)
        Dim Dir As Integer, i As Integer
        Dim Buffer As New ByteStream(Data)
        i = Buffer.ReadInt32
        Dir = Buffer.ReadInt32

        With MapNpc(i)
            .Dir = Dir
            .XOffset = 0
            .YOffset = 0
            .Moving = 0
        End With

        Buffer.Dispose()
    End Sub

    Private Sub Packet_PlayerXY(ByVal Data() As Byte)
        Dim X As Integer, Y As Integer, Dir As Integer
        Dim Buffer As New ByteStream(Data)
        X = Buffer.ReadInt32
        Y = Buffer.ReadInt32
        Dir = Buffer.ReadInt32

        SetPlayerX(MyIndex, X)
        SetPlayerY(MyIndex, Y)
        SetPlayerDir(MyIndex, Dir)

        ' Make sure they aren't walking
        Player(MyIndex).Moving = 0
        Player(MyIndex).XOffset = 0
        Player(MyIndex).YOffset = 0

        Buffer.Dispose()
    End Sub

    Private Sub Packet_Attack(ByVal Data() As Byte)
        Dim i As Integer
        Dim Buffer As New ByteStream(Data)
        i = Buffer.ReadInt32

        ' Set player to attacking
        Player(i).Attacking = 1
        Player(i).AttackTimer = GetTickCount()

        Buffer.Dispose()
    End Sub

    Private Sub Packet_NpcAttack(ByVal Data() As Byte)
        Dim i As Integer
        Dim Buffer As New ByteStream(Data)
        i = Buffer.ReadInt32

        ' Set npc to attacking
        MapNpc(i).Attacking = 1
        MapNpc(i).AttackTimer = GetTickCount()

        Buffer.Dispose()
    End Sub

    Private Sub Packet_CheckMap(ByVal Data() As Byte)
        Dim X As Integer, Y As Integer, i As Integer
        Dim NeedMap As Byte
        Dim Buffer As New ByteStream(Data)
        GettingMap = True

        ' Erase all players except self
        For i = 1 To TotalOnline 'MAX_PLAYERS
            If i <> MyIndex Then
                SetPlayerMap(i, 0)
            End If
        Next

        ' Erase all temporary tile values
        ClearTempTile()
        ClearMapNpcs()
        ClearMapItems()
        ClearBlood()
        ClearMap()

        ' Get map num
        X = Buffer.ReadInt32
        ' Get revision
        Y = Buffer.ReadInt32

        NeedMap = 1

        ' Either the revisions didn't match or we dont have the map, so we need it
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ClientPackets.CNeedMap)
        Buffer.WriteInt32(NeedMap)
        SendData(Buffer.ToArray())

        Buffer.Dispose()
    End Sub

    Private Sub Packet_MapData(ByVal Data() As Byte)
        Dim X As Integer, Y As Integer, i As Integer
        Dim MapNum As Integer, MusicFile As String
        Dim Buffer As New ByteStream(Compression.DecompressBytes(Data))

        MapData = False

        ClearMap()

        SyncLock MapLock
            If Buffer.ReadInt32 = 1 Then

                MapNum = Buffer.ReadInt32
                Map.Name = Trim(Buffer.ReadString)
                Map.Music = Trim(Buffer.ReadString)
                Map.Revision = Buffer.ReadInt32
                Map.Moral = Buffer.ReadInt32
                Map.Tileset = Buffer.ReadInt32
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

            ClearTempTile()

            Buffer.Dispose()

        End SyncLock

        InitAutotiles()

        MapData = True

        For i = 1 To Byte.MaxValue
            ClearActionMsg(i)
        Next

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
        PlayMusic(MusicFile)

        UpdateDrawMapName()

        GettingMap = False
        CanMoveNow = True

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

        Buffer.Dispose()
    End Sub

    Private Sub Packet_MapNPCUpdate(ByVal Data() As Byte)
        Dim NpcNum As Integer
        Dim Buffer As New ByteStream(Data)
        NpcNum = Buffer.ReadInt32

        With MapNpc(NpcNum)
            .Num = Buffer.ReadInt32
            .X = Buffer.ReadInt32
            .Y = Buffer.ReadInt32
            .Dir = Buffer.ReadInt32
            .Vital(Vitals.HP) = Buffer.ReadInt32
            .Vital(Vitals.MP) = Buffer.ReadInt32
        End With

        Buffer.Dispose()
    End Sub

    Private Sub Packet_MapDone(ByVal data() As Byte)
        Dim i As Integer
        Dim MusicFile As String

        For i = 1 To Byte.MaxValue
            ClearActionMsg(i)
        Next i

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
        PlayMusic(MusicFile)

        UpdateDrawMapName()

        GettingMap = False
        CanMoveNow = True

    End Sub

    Private Sub Packet_GlobalMessage(ByVal Data() As Byte)
        Dim Msg As String
        Dim Buffer As New ByteStream(Data)
        'Msg = Trim(Buffer.ReadString)
        Msg = Trim(ReadUnicodeString(Buffer.ReadBytes()))

        Buffer.Dispose()

        AddText(Msg, QColorType.GlobalColor)
    End Sub

    Private Sub Packet_MapMessage(ByVal Data() As Byte)
        Dim Msg As String
        Dim Buffer As New ByteStream(Data)

        'Msg = Trim(Buffer.ReadString)
        Msg = Trim(ReadUnicodeString(Buffer.ReadBytes))

        Buffer.Dispose()

        AddText(Msg, QColorType.BroadcastColor)

    End Sub

    Private Sub Packet_SpawnItem(ByVal Data() As Byte)
        Dim i As Integer
        Dim Buffer As New ByteStream(Data)

        i = Buffer.ReadInt32

        With MapItem(i)
            .Num = Buffer.ReadInt32
            .Value = Buffer.ReadInt32
            .X = Buffer.ReadInt32
            .Y = Buffer.ReadInt32
        End With

        Buffer.Dispose()
    End Sub

    Private Sub Packet_PlayerMessage(ByVal Data() As Byte)
        Dim Msg As String, colour As Integer
        Dim Buffer As New ByteStream(Data)
        'Msg = Trim(Buffer.ReadString)
        Msg = Trim(ReadUnicodeString(Buffer.ReadBytes))

        colour = Buffer.ReadInt32

        Buffer.Dispose()

        AddText(Msg, colour)
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
        Item(n).SubType = Buffer.ReadInt32

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

        Buffer.Dispose()

        ' changes to inventory, need to clear any drop menu
        FrmMainGame.pnlCurrency.Visible = False
        FrmMainGame.txtCurrency.Text = ""
        tmpCurrencyItem = 0
        CurrencyMenu = 0 ' clear

    End Sub

    Sub Packet_SpawnNPC(ByVal data() As Byte)
        Dim i As Integer
        Dim Buffer As New ByteStream(data)
        i = Buffer.ReadInt32

        With MapNpc(i)
            .Num = Buffer.ReadInt32
            .X = Buffer.ReadInt32
            .Y = Buffer.ReadInt32
            .Dir = Buffer.ReadInt32

            For i = 1 To Vitals.Count - 1
                .Vital(i) = Buffer.ReadInt32
            Next
            ' Client use only
            .XOffset = 0
            .YOffset = 0
            .Moving = 0
        End With

        Buffer.Dispose()
    End Sub

    Sub Packet_NpcDead(ByVal data() As Byte)
        Dim i As Integer
        Dim Buffer As New ByteStream(data)
        i = Buffer.ReadInt32
        ClearMapNpc(i)

        Buffer.Dispose()
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

        Buffer.Dispose()
    End Sub

    Sub Packet_MapKey(ByVal data() As Byte)
        Dim n As Integer, X As Integer, Y As Integer
        Dim Buffer As New ByteStream(data)
        X = Buffer.ReadInt32
        Y = Buffer.ReadInt32
        n = Buffer.ReadInt32
        TempTile(X, Y).DoorOpen = n

        Buffer.Dispose()
    End Sub

    Sub Packet_EditMap(ByVal data() As Byte)
        Dim Buffer As New ByteStream(data)
        InitMapEditor = True

        Buffer.Dispose()
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

        Buffer.Dispose()
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

        Buffer.Dispose()

    End Sub

    Sub Packet_Skills(ByVal data() As Byte)
        Dim i As Integer
        Dim Buffer As New ByteStream(data)
        For i = 1 To MAX_PLAYER_SKILLS
            PlayerSkills(i) = Buffer.ReadInt32
        Next

        Buffer.Dispose()
    End Sub

    Sub Packet_LeftMap(ByVal data() As Byte)
        Dim Buffer As New ByteStream(data)
        ClearPlayer(Buffer.ReadInt32)

        Buffer.Dispose()
    End Sub

    Private Sub Packet_ResourceCache(ByVal Data() As Byte)
        Dim i As Integer
        Dim Buffer As New ByteStream(Data)
        Resource_Index = Buffer.ReadInt32
        Resources_Init = False

        If Resource_Index > 0 Then
            ReDim Preserve MapResource(0 To Resource_Index)

            For i = 0 To Resource_Index
                MapResource(i).ResourceState = Buffer.ReadInt32
                MapResource(i).X = Buffer.ReadInt32
                MapResource(i).Y = Buffer.ReadInt32
            Next

            Resources_Init = True
        Else
            ReDim MapResource(0 To 1)
        End If

        Buffer.Dispose()
    End Sub

    Private Sub Packet_Ping(ByVal data() As Byte)
        PingEnd = GetTickCount()
        Ping = PingEnd - PingStart
    End Sub

    Private Sub Packet_DoorAnimation(ByVal data() As Byte)
        Dim X As Integer, Y As Integer
        Dim Buffer As New ByteStream(data)
        X = Buffer.ReadInt32
        Y = Buffer.ReadInt32
        With TempTile(X, Y)
            .DoorFrame = 1
            .DoorAnimate = 1 ' 0 = nothing| 1 = opening | 2 = closing
            .DoorTimer = GetTickCount()
        End With

        Buffer.Dispose()
    End Sub

    Private Sub Packet_ActionMessage(ByVal data() As Byte)
        Dim X As Integer, Y As Integer, message As String, color As Integer, tmpType As Integer
        Dim Buffer As New ByteStream(data)
        message = Trim(ReadUnicodeString(Buffer.ReadBytes))
        color = Buffer.ReadInt32
        tmpType = Buffer.ReadInt32
        X = Buffer.ReadInt32
        Y = Buffer.ReadInt32

        Buffer.Dispose()

        CreateActionMsg(message, color, tmpType, X, Y)
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

        Buffer.Dispose()
    End Sub

    Private Sub Packet_PlayerExp(ByVal Data() As Byte)
        Dim index As Integer, TNL As Integer
        Dim Buffer As New ByteStream(Data)
        index = Buffer.ReadInt32
        SetPlayerExp(index, Buffer.ReadInt32)
        TNL = Buffer.ReadInt32

        If TNL = 0 Then TNL = 1
        NextlevelExp = TNL

        Buffer.Dispose()
    End Sub

    Private Sub Packet_Blood(ByVal Data() As Byte)
        Dim X As Integer, Y As Integer, Sprite As Integer
        Dim Buffer As New ByteStream(Data)
        X = Buffer.ReadInt32
        Y = Buffer.ReadInt32

        ' randomise sprite
        Sprite = Rand(1, 3)

        BloodIndex = BloodIndex + 1
        If BloodIndex >= Byte.MaxValue Then BloodIndex = 1

        With Blood(BloodIndex)
            .X = X
            .Y = Y
            .Sprite = Sprite
            .Timer = GetTickCount()
        End With

        Buffer.Dispose()
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
        Buffer.Dispose()
    End Sub

    Private Sub Packet_Animation(ByVal Data() As Byte)
        Dim Buffer As New ByteStream(Data)
        AnimationIndex = AnimationIndex + 1
        If AnimationIndex >= Byte.MaxValue Then AnimationIndex = 1

        With AnimInstance(AnimationIndex)
            .Animation = Buffer.ReadInt32
            .X = Buffer.ReadInt32
            .Y = Buffer.ReadInt32
            .LockType = Buffer.ReadInt32
            .lockindex = Buffer.ReadInt32
            .Used(0) = True
            .Used(1) = True
        End With

        Buffer.Dispose()
    End Sub

    Private Sub Packet_NPCVitals(ByVal Data() As Byte)
        Dim MapNpcNum As Integer
        Dim Buffer As New ByteStream(Data)
        MapNpcNum = Buffer.ReadInt32
        For i = 1 To Vitals.Count - 1
            MapNpc(MapNpcNum).Vital(i) = Buffer.ReadInt32
        Next

        Buffer.Dispose()
    End Sub

    Private Sub Packet_Cooldown(ByVal Data() As Byte)
        Dim slot As Integer
        Dim Buffer As New ByteStream(Data)
        slot = Buffer.ReadInt32
        SkillCD(slot) = GetTickCount()

        Buffer.Dispose()
    End Sub

    Private Sub Packet_ClearSkillBuffer(ByVal Data() As Byte)
        Dim Buffer As New ByteStream(Data)
        SkillBuffer = 0
        SkillBufferTimer = 0

        Buffer.Dispose()
    End Sub

    Private Sub Packet_SayMessage(ByVal Data() As Byte)
        Dim Access As Integer, Name As String, message As String
        Dim Header As String, PK As Integer
        Dim Buffer As New ByteStream(Data)
        Name = Trim(Buffer.ReadString)
        Access = Buffer.ReadInt32
        PK = Buffer.ReadInt32
        'message = Trim(Buffer.ReadString)
        message = Trim(ReadUnicodeString(Buffer.ReadBytes))
        Header = Trim(Buffer.ReadString)

        AddText(Header & Name & ": " & message, QColorType.SayColor)

        Buffer.Dispose()
    End Sub

    Private Sub Packet_OpenShop(ByVal Data() As Byte)
        Dim shopnum As Integer
        Dim Buffer As New ByteStream(Data)
        shopnum = Buffer.ReadInt32

        NeedToOpenShop = True
        NeedToOpenShopNum = shopnum

        Buffer.Dispose()
    End Sub

    Private Sub Packet_ResetShopAction(ByVal Data() As Byte)
        ShopAction = 0
    End Sub

    Private Sub Packet_Stunned(ByVal Data() As Byte)
        Dim Buffer As New ByteStream(Data)
        StunDuration = Buffer.ReadInt32

        Buffer.Dispose()
    End Sub

    Private Sub Packet_MapWornEquipment(ByVal Data() As Byte)
        Dim playernum As Integer
        Dim Buffer As New ByteStream(Data)
        playernum = Buffer.ReadInt32
        SetPlayerEquipment(playernum, Buffer.ReadInt32, EquipmentType.Armor)
        SetPlayerEquipment(playernum, Buffer.ReadInt32, EquipmentType.Weapon)
        SetPlayerEquipment(playernum, Buffer.ReadInt32, EquipmentType.Helmet)
        SetPlayerEquipment(playernum, Buffer.ReadInt32, EquipmentType.Shield)
        SetPlayerEquipment(playernum, Buffer.ReadInt32, EquipmentType.Shoes)
        SetPlayerEquipment(playernum, Buffer.ReadInt32, EquipmentType.Gloves)

        Buffer.Dispose()
    End Sub

    Private Sub Packet_OpenBank(ByVal Data() As Byte)
        Dim i As Integer, x As Integer
        Dim Buffer As New ByteStream(Data)
        For i = 1 To MAX_BANK
            Bank.Item(i).Num = Buffer.ReadInt32
            Bank.Item(i).Value = Buffer.ReadInt32

            Bank.ItemRand(i).Prefix = Buffer.ReadString
            Bank.ItemRand(i).Suffix = Buffer.ReadString
            Bank.ItemRand(i).Rarity = Buffer.ReadInt32
            Bank.ItemRand(i).Damage = Buffer.ReadInt32
            Bank.ItemRand(i).Speed = Buffer.ReadInt32

            For x = 1 To Stats.Count - 1
                Bank.ItemRand(i).Stat(x) = Buffer.ReadInt32
            Next
        Next

        NeedToOpenBank = True

        Buffer.Dispose()
    End Sub

    Private Sub Packet_ClearTradeTimer(ByVal Data() As Byte)
        Dim Buffer As New ByteStream(Data)
        TradeRequest = False
        TradeTimer = 0

        Buffer.Dispose()
    End Sub

    Private Sub Packet_TradeInvite(ByVal Data() As Byte)
        Dim requester As Integer
        Dim Buffer As New ByteStream(Data)
        requester = Buffer.ReadInt32

        DialogType = DIALOGUE_TYPE_TRADE

        DialogMsg1 = Strings.Get("trade", "tradeinvite", Trim$((Player(requester).Name)))

        UpdateDialog = True

        Buffer.Dispose()
    End Sub

    Private Sub Packet_Trade(ByVal Data() As Byte)
        Dim Buffer As New ByteStream(Data)
        NeedToOpenTrade = True
        Buffer.ReadInt32()
        Tradername = Trim(Buffer.ReadString)
        pnlTradeVisible = True

        Buffer.Dispose()
    End Sub

    Private Sub Packet_CloseTrade(ByVal Data() As Byte)
        NeedtoCloseTrade = True
    End Sub

    Private Sub Packet_TradeUpdate(ByVal Data() As Byte)
        Dim datatype As Integer
        Dim Buffer As New ByteStream(Data)
        datatype = Buffer.ReadInt32

        If datatype = 0 Then ' ours!
            For i = 1 To MAX_INV
                TradeYourOffer(i).Num = Buffer.ReadInt32
                TradeYourOffer(i).Value = Buffer.ReadInt32
            Next
            YourWorth = Strings.Get("trade", "tradeworth", Buffer.ReadInt32)
        ElseIf datatype = 1 Then 'theirs
            For i = 1 To MAX_INV
                TradeTheirOffer(i).Num = Buffer.ReadInt32
                TradeTheirOffer(i).Value = Buffer.ReadInt32
            Next
            TheirWorth = "Total Worth: " & Buffer.ReadInt32 & "g"
        End If

        NeedtoUpdateTrade = True

        Buffer.Dispose()
    End Sub

    Private Sub Packet_TradeStatus(ByVal Data() As Byte)
        Dim tradestatus As Integer
        Dim Buffer As New ByteStream(Data)
        tradestatus = Buffer.ReadInt32

        Select Case tradestatus
            Case 0 ' clear
                'frmMainGame.lblTradeStatus.Text = ""
            Case 1 ' they've accepted
                AddText(Strings.Get("trade", "tradestatusok"), ColorType.White)
            Case 2 ' you've accepted
                AddText(Strings.Get("trade", "tradestatuswait"), ColorType.White)
        End Select

        Buffer.Dispose()
    End Sub

    Private Sub Packet_GameData(ByVal Data() As Byte)
        Dim n As Integer, i As Integer, z As Integer, x As Integer, a As Integer, b As Integer
        Dim Buffer As New ByteStream(Compression.DecompressBytes(Data))

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
                .Name = Trim(Buffer.ReadString)
                .Desc = Trim$(Buffer.ReadString)

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

            For z = 0 To Stats.Count - 1
                Item(n).Stat_Req(z) = Buffer.ReadInt32()
            Next

            Item(n).Type = Buffer.ReadInt32()
            Item(n).SubType = Buffer.ReadInt32

            Item(n).ItemLevel = Buffer.ReadInt32

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

        ' changes to inventory, need to clear any drop menu

        FrmMainGame.pnlCurrency.Visible = False
        FrmMainGame.txtCurrency.Text = ""
        tmpCurrencyItem = 0
        CurrencyMenu = 0 ' clear

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

            Animation(n).Name = Trim$(Buffer.ReadString)
            Animation(n).Sound = Trim$(Buffer.ReadString)

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
            Npc(n).AttackSay = Trim(Buffer.ReadString())
            Npc(n).Behaviour = Buffer.ReadInt32()
            For z = 1 To 5
                Npc(n).DropChance(z) = Buffer.ReadInt32()
                Npc(n).DropItem(z) = Buffer.ReadInt32()
                Npc(n).DropItemValue(z) = Buffer.ReadInt32()
            Next

            Npc(n).Exp = Buffer.ReadInt32()
            Npc(n).Faction = Buffer.ReadInt32()
            Npc(n).Hp = Buffer.ReadInt32()
            Npc(n).Name = Trim(Buffer.ReadString())
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
            Shop(n).Name = Trim(Buffer.ReadString())
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
            Skill(n).Name = Trim(Buffer.ReadString())
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
            Resource(n).EmptyMessage = Trim(Buffer.ReadString())
            Resource(n).ExhaustedImage = Buffer.ReadInt32()
            Resource(n).Health = Buffer.ReadInt32()
            Resource(n).ExpReward = Buffer.ReadInt32()
            Resource(n).ItemReward = Buffer.ReadInt32()
            Resource(n).Name = Trim(Buffer.ReadString())
            Resource(n).ResourceImage = Buffer.ReadInt32()
            Resource(n).ResourceType = Buffer.ReadInt32()
            Resource(n).RespawnTime = Buffer.ReadInt32()
            Resource(n).SuccessMessage = Trim(Buffer.ReadString())
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

        Buffer.Dispose()
    End Sub

    Private Sub Packet_Target(ByVal Data() As Byte)
        Dim Buffer As New ByteStream(Data)
        myTarget = Buffer.ReadInt32
        myTargetType = Buffer.ReadInt32

        Buffer.Dispose()
    End Sub

    Private Sub Packet_Mapreport(ByVal Data() As Byte)
        Dim I As Integer
        Dim Buffer As New ByteStream(Data)
        For I = 1 To MAX_MAPS
            MapNames(I) = Trim(Buffer.ReadString())
        Next

        UpdateMapnames = True

        Buffer.Dispose()
    End Sub

    Private Sub Packet_Admin(ByVal Data() As Byte)
        Adminvisible = True
    End Sub

    Private Sub Packet_MapNames(ByVal Data() As Byte)
        Dim I As Integer
        Dim Buffer As New ByteStream(Data)
        For I = 1 To MAX_MAPS
            MapNames(I) = Trim(Buffer.ReadString())
        Next

        Buffer.Dispose()
    End Sub

    Private Sub Packet_Hotbar(ByVal Data() As Byte)
        Dim i As Integer
        Dim Buffer As New ByteStream(Data)
        For i = 1 To MAX_HOTBAR
            Player(MyIndex).Hotbar(i).Slot = Buffer.ReadInt32
            Player(MyIndex).Hotbar(i).sType = Buffer.ReadInt32
        Next

        Buffer.Dispose()
    End Sub

    Private Sub Packet_Critical(ByVal Data() As Byte)
        ShakeTimerEnabled = True
        ShakeTimer = GetTickCount()
    End Sub

    Private Sub Packet_News(ByVal Data() As Byte)
        Dim Buffer As New ByteStream(Data)
        GAME_NAME = Buffer.ReadString
        News = Buffer.ReadString

        UpdateNews = True

        Buffer.Dispose()
    End Sub

    Private Sub Packet_RClick(ByVal Data() As Byte)
        ShowRClick = True
    End Sub

    Private Sub Packet_TotalOnline(ByVal Data() As Byte)
        Dim Buffer As New ByteStream(Data)
        TotalOnline = Buffer.ReadInt32

        Buffer.Dispose()
    End Sub

    Private Sub Packet_Emote(ByVal Data() As Byte)
        Dim index As Integer, emote As Integer
        Dim Buffer As New ByteStream(Data)
        index = Buffer.ReadInt32
        emote = Buffer.ReadInt32

        With Player(index)
            .Emote = emote
            .EmoteTimer = GetTickCount() + 5000
        End With

        Buffer.Dispose()

    End Sub

    Private Sub Packet_ChatBubble(ByVal Data() As Byte)
        Dim targetType As Integer, target As Integer, Message As String, colour As Integer
        Dim Buffer As New ByteStream(Data)
        target = Buffer.ReadInt32
        targetType = Buffer.ReadInt32
        'Message = buffer.ReadString
        Message = Trim(ReadUnicodeString(Buffer.ReadBytes))
        colour = Buffer.ReadInt32
        AddChatBubble(target, targetType, Message, colour)

        Buffer.Dispose()

    End Sub

    Private Sub Packet_LeftGame(ByVal Data() As Byte)
        DestroyGame()
    End Sub
End Module