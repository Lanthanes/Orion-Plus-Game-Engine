Imports System.IO
Imports ASFW
Imports ASFW.IO

Module ServerNetworkReceive
    Public Sub PacketRouter()
        Socket.PacketId(ClientPackets.CCheckPing) = AddressOf Packet_Ping
        Socket.PacketId(ClientPackets.CNewAccount) = AddressOf Packet_NewAccount
        Socket.PacketId(ClientPackets.CDelAccount) = AddressOf Packet_DeleteAccount
        Socket.PacketId(ClientPackets.CLogin) = AddressOf Packet_Login
        Socket.PacketId(ClientPackets.CAddChar) = AddressOf Packet_AddChar
        Socket.PacketId(ClientPackets.CUseChar) = AddressOf Packet_UseChar
        Socket.PacketId(ClientPackets.CDelChar) = AddressOf Packet_DeleteChar
        Socket.PacketId(ClientPackets.CSayMsg) = AddressOf Packet_SayMessage
        Socket.PacketId(ClientPackets.CBroadcastMsg) = AddressOf Packet_BroadCastMsg
        Socket.PacketId(ClientPackets.CPlayerMsg) = AddressOf Packet_PlayerMsg
        Socket.PacketId(ClientPackets.CPlayerMove) = AddressOf Packet_PlayerMove
        Socket.PacketId(ClientPackets.CPlayerDir) = AddressOf Packet_PlayerDirection
        Socket.PacketId(ClientPackets.CUseItem) = AddressOf Packet_UseItem
        Socket.PacketId(ClientPackets.CAttack) = AddressOf Packet_Attack
        Socket.PacketId(ClientPackets.CPlayerInfoRequest) = AddressOf Packet_PlayerInfo
        Socket.PacketId(ClientPackets.CWarpMeTo) = AddressOf Packet_WarpMeTo
        Socket.PacketId(ClientPackets.CWarpToMe) = AddressOf Packet_WarpToMe
        Socket.PacketId(ClientPackets.CWarpTo) = AddressOf Packet_WarpTo
        Socket.PacketId(ClientPackets.CSetSprite) = AddressOf Packet_SetSprite
        Socket.PacketId(ClientPackets.CGetStats) = AddressOf Packet_GetStats
        Socket.PacketId(ClientPackets.CRequestNewMap) = AddressOf Packet_RequestNewMap
        Socket.PacketId(ClientPackets.CSaveMap) = AddressOf Packet_MapData
        Socket.PacketId(ClientPackets.CNeedMap) = AddressOf Packet_NeedMap
        Socket.PacketId(ClientPackets.CMapGetItem) = AddressOf Packet_GetItem
        Socket.PacketId(ClientPackets.CMapDropItem) = AddressOf Packet_DropItem
        Socket.PacketId(ClientPackets.CMapRespawn) = AddressOf Packet_RespawnMap
        Socket.PacketId(ClientPackets.CMapReport) = AddressOf Packet_MapReport 'Mapreport
        Socket.PacketId(ClientPackets.CKickPlayer) = AddressOf Packet_KickPlayer
        Socket.PacketId(ClientPackets.CBanList) = AddressOf Packet_Banlist
        Socket.PacketId(ClientPackets.CBanDestroy) = AddressOf Packet_DestroyBans
        Socket.PacketId(ClientPackets.CBanPlayer) = AddressOf Packet_BanPlayer

        Socket.PacketId(ClientPackets.CRequestEditMap) = AddressOf Packet_EditMapRequest

        Socket.PacketId(ClientPackets.CSetAccess) = AddressOf Packet_SetAccess
        Socket.PacketId(ClientPackets.CWhosOnline) = AddressOf Packet_WhosOnline
        Socket.PacketId(ClientPackets.CSetMotd) = AddressOf Packet_SetMotd
        Socket.PacketId(ClientPackets.CSearch) = AddressOf Packet_PlayerSearch
        Socket.PacketId(ClientPackets.CSkills) = AddressOf Packet_Skills
        Socket.PacketId(ClientPackets.CCast) = AddressOf Packet_Cast
        Socket.PacketId(ClientPackets.CQuit) = AddressOf Packet_QuitGame
        Socket.PacketId(ClientPackets.CSwapInvSlots) = AddressOf Packet_SwapInvSlots

        Socket.PacketId(ClientPackets.CCheckPing) = AddressOf Packet_CheckPing
        Socket.PacketId(ClientPackets.CUnequip) = AddressOf Packet_Unequip
        Socket.PacketId(ClientPackets.CRequestPlayerData) = AddressOf Packet_RequestPlayerData
        Socket.PacketId(ClientPackets.CRequestItems) = AddressOf Packet_RequestItems
        Socket.PacketId(ClientPackets.CRequestNPCS) = AddressOf Packet_RequestNpcs
        Socket.PacketId(ClientPackets.CRequestResources) = AddressOf Packet_RequestResources
        Socket.PacketId(ClientPackets.CSpawnItem) = AddressOf Packet_SpawnItem
        Socket.PacketId(ClientPackets.CTrainStat) = AddressOf Packet_TrainStat

        Socket.PacketId(ClientPackets.CRequestAnimations) = AddressOf Packet_RequestAnimations
        Socket.PacketId(ClientPackets.CRequestSkills) = AddressOf Packet_RequestSkills
        Socket.PacketId(ClientPackets.CRequestShops) = AddressOf Packet_RequestShops
        Socket.PacketId(ClientPackets.CRequestLevelUp) = AddressOf Packet_RequestLevelUp
        Socket.PacketId(ClientPackets.CForgetSkill) = AddressOf Packet_ForgetSkill
        Socket.PacketId(ClientPackets.CCloseShop) = AddressOf Packet_CloseShop
        Socket.PacketId(ClientPackets.CBuyItem) = AddressOf Packet_BuyItem
        Socket.PacketId(ClientPackets.CSellItem) = AddressOf Packet_SellItem
        Socket.PacketId(ClientPackets.CChangeBankSlots) = AddressOf Packet_ChangeBankSlots
        Socket.PacketId(ClientPackets.CDepositItem) = AddressOf Packet_DepositItem
        Socket.PacketId(ClientPackets.CWithdrawItem) = AddressOf Packet_WithdrawItem
        Socket.PacketId(ClientPackets.CCloseBank) = AddressOf Packet_CloseBank
        Socket.PacketId(ClientPackets.CAdminWarp) = AddressOf Packet_AdminWarp

        Socket.PacketId(ClientPackets.CTradeInvite) = AddressOf Packet_TradeInvite
        Socket.PacketId(ClientPackets.CTradeInviteAccept) = AddressOf Packet_TradeInviteAccept
        Socket.PacketId(ClientPackets.CAcceptTrade) = AddressOf Packet_AcceptTrade
        Socket.PacketId(ClientPackets.CDeclineTrade) = AddressOf Packet_DeclineTrade
        Socket.PacketId(ClientPackets.CTradeItem) = AddressOf Packet_TradeItem
        Socket.PacketId(ClientPackets.CUntradeItem) = AddressOf Packet_UntradeItem

        Socket.PacketId(ClientPackets.CAdmin) = AddressOf Packet_Admin

        'quests
        Socket.PacketId(ClientPackets.CRequestQuests) = AddressOf Packet_RequestQuests
        Socket.PacketId(ClientPackets.CQuestLogUpdate) = AddressOf Packet_QuestLogUpdate
        Socket.PacketId(ClientPackets.CPlayerHandleQuest) = AddressOf Packet_PlayerHandleQuest
        Socket.PacketId(ClientPackets.CQuestReset) = AddressOf Packet_QuestReset

        'Housing
        Socket.PacketId(ClientPackets.CBuyHouse) = AddressOf Packet_BuyHouse
        Socket.PacketId(ClientPackets.CVisit) = AddressOf Packet_InviteToHouse
        Socket.PacketId(ClientPackets.CAcceptVisit) = AddressOf Packet_AcceptInvite
        Socket.PacketId(ClientPackets.CPlaceFurniture) = AddressOf Packet_PlaceFurniture

        Socket.PacketId(ClientPackets.CSellHouse) = AddressOf Packet_SellHouse

        'hotbar
        Socket.PacketId(ClientPackets.CSetHotbarSlot) = AddressOf Packet_SetHotBarSlot
        Socket.PacketId(ClientPackets.CDeleteHotbarSlot) = AddressOf Packet_DeleteHotBarSlot
        Socket.PacketId(ClientPackets.CUseHotbarSlot) = AddressOf Packet_UseHotBarSlot

        'Events
        Socket.PacketId(ClientPackets.CEventChatReply) = AddressOf Packet_EventChatReply
        Socket.PacketId(ClientPackets.CEvent) = AddressOf Packet_Event
        Socket.PacketId(ClientPackets.CRequestSwitchesAndVariables) = AddressOf Packet_RequestSwitchesAndVariables
        Socket.PacketId(ClientPackets.CSwitchesAndVariables) = AddressOf Packet_SwitchesAndVariables

        'projectiles

        Socket.PacketId(ClientPackets.CRequestProjectiles) = AddressOf HandleRequestProjectiles
        Socket.PacketId(ClientPackets.CClearProjectile) = AddressOf HandleClearProjectile

        'craft
        Socket.PacketId(ClientPackets.CRequestRecipes) = AddressOf Packet_RequestRecipes

        Socket.PacketId(ClientPackets.CCloseCraft) = AddressOf Packet_CloseCraft
        Socket.PacketId(ClientPackets.CStartCraft) = AddressOf Packet_StartCraft

        Socket.PacketId(ClientPackets.CRequestClasses) = AddressOf Packet_RequestClasses

        'emotes
        Socket.PacketId(ClientPackets.CEmote) = AddressOf Packet_Emote

        'parties
        Socket.PacketId(ClientPackets.CRequestParty) = AddressOf Packet_PartyRquest
        Socket.PacketId(ClientPackets.CAcceptParty) = AddressOf Packet_AcceptParty
        Socket.PacketId(ClientPackets.CDeclineParty) = AddressOf Packet_DeclineParty
        Socket.PacketId(ClientPackets.CLeaveParty) = AddressOf Packet_LeaveParty
        Socket.PacketId(ClientPackets.CPartyChatMsg) = AddressOf Packet_PartyChatMsg

        'pets
        Socket.PacketId(ClientPackets.CRequestPets) = AddressOf Packet_RequestPets
        Socket.PacketId(ClientPackets.CSummonPet) = AddressOf Packet_SummonPet
        Socket.PacketId(ClientPackets.CPetMove) = AddressOf Packet_PetMove
        Socket.PacketId(ClientPackets.CSetBehaviour) = AddressOf Packet_SetPetBehaviour
        Socket.PacketId(ClientPackets.CReleasePet) = AddressOf Packet_ReleasePet
        Socket.PacketId(ClientPackets.CPetSkill) = AddressOf Packet_PetSkill
        Socket.PacketId(ClientPackets.CPetUseStatPoint) = AddressOf Packet_UsePetStatPoint

        'editor login
        Socket.PacketId(EditorPackets.EditorLogin) = AddressOf Packet_EditorLogin
        Socket.PacketId(EditorPackets.EditorRequestMap) = AddressOf Packet_EditorRequestMap
        Socket.PacketId(EditorPackets.EditorSaveMap) = AddressOf Packet_EditorMapData

        'editor
        Socket.PacketId(EditorPackets.RequestEditItem) = AddressOf Packet_EditItem
        Socket.PacketId(EditorPackets.SaveItem) = AddressOf Packet_SaveItem
        Socket.PacketId(EditorPackets.RequestEditNpc) = AddressOf Packet_EditNpc
        Socket.PacketId(EditorPackets.SaveNpc) = AddressOf Packet_SaveNPC
        Socket.PacketId(EditorPackets.RequestEditShop) = AddressOf Packet_EditShop
        Socket.PacketId(EditorPackets.SaveShop) = AddressOf Packet_SaveShop
        Socket.PacketId(EditorPackets.RequestEditSkill) = AddressOf Packet_EditSkill
        Socket.PacketId(EditorPackets.SaveSkill) = AddressOf Packet_SaveSkill
        Socket.PacketId(EditorPackets.RequestEditResource) = AddressOf Packet_EditResource
        Socket.PacketId(EditorPackets.SaveResource) = AddressOf Packet_SaveResource
        Socket.PacketId(EditorPackets.RequestEditAnimation) = AddressOf Packet_EditAnimation
        Socket.PacketId(EditorPackets.SaveAnimation) = AddressOf Packet_SaveAnimation
        Socket.PacketId(EditorPackets.RequestEditQuest) = AddressOf Packet_RequestEditQuest
        Socket.PacketId(EditorPackets.SaveQuest) = AddressOf Packet_SaveQuest
        Socket.PacketId(EditorPackets.RequestEditHouse) = AddressOf Packet_RequestEditHouse
        Socket.PacketId(EditorPackets.SaveHouses) = AddressOf Packet_SaveHouses
        Socket.PacketId(EditorPackets.RequestEditProjectiles) = AddressOf HandleRequestEditProjectiles
        Socket.PacketId(EditorPackets.SaveProjectile) = AddressOf HandleSaveProjectile
        Socket.PacketId(EditorPackets.RequestEditRecipes) = AddressOf Packet_RequestEditRecipes
        Socket.PacketId(EditorPackets.SaveRecipe) = AddressOf Packet_SaveRecipe
        Socket.PacketId(EditorPackets.RequestEditClasses) = AddressOf Packet_RequestEditClasses
        Socket.PacketId(EditorPackets.SaveClasses) = AddressOf Packet_SaveClasses
        Socket.PacketId(EditorPackets.RequestAutoMap) = AddressOf Packet_RequestAutoMap
        Socket.PacketId(EditorPackets.SaveAutoMap) = AddressOf Packet_SaveAutoMap

        'pet
        Socket.PacketId(EditorPackets.CRequestEditPet) = AddressOf Packet_RequestEditPet
        Socket.PacketId(EditorPackets.CSavePet) = AddressOf Packet_SavePet

    End Sub

    Private Sub Packet_Ping(index As Integer, ByRef data() As Byte)
        TempPlayer(index).DataPackets = TempPlayer(index).DataPackets + 1
    End Sub

    Private Sub Packet_NewAccount(index As Integer, ByRef data() As Byte)
        Dim username As String, password As String
        Dim i As Integer, n As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CNewAccount", PACKET_LOG)
        TextAdd("Recieved CMSG: CNewAccount")

        If Not IsPlaying(index) AndAlso Not IsLoggedIn(index) Then
            'Get the Data
            username = EKeyPair.DecryptString(Buffer.ReadString)
            password = EKeyPair.DecryptString(Buffer.ReadString)
            ' Prevent hacking
            If Len(username.Trim) < 3 Or Len(password.Trim) < 3 Then
                AlertMsg(index, "Your username and password must be at least three characters in length")
                Exit Sub
            End If

            ' Prevent hacking
            For i = 1 To Len(username)
                n = AscW(Mid$(username, i, 1))

                If Not IsNameLegal(n) Then
                    AlertMsg(index, "Invalid username, only letters, numbers, spaces, and _ allowed in usernames.")
                    Exit Sub
                End If
            Next

            ' Check to see if account already exists
            If Not AccountExist(username) Then
                AddAccount(index, username, password)

                TextAdd("Account " & username & " has been created.")
                Addlog("Account " & username & " has been created.", PLAYER_LOG)

                ' Load the player
                LoadPlayer(index, username)

                ' Check if character data has been created
                If Len(Trim$(Player(index).Character(TempPlayer(index).CurChar).Name)) > 0 Then
                    ' we have a char!
                    HandleUseChar(index)
                Else
                    ' send new char shit
                    If Not IsPlaying(index) Then
                        SendNewCharClasses(index)
                    End If
                End If

                ' Show the player up on the socket status
                Addlog(GetPlayerLogin(index) & " has logged in from " & Socket.ClientIp(index) & ".", PLAYER_LOG)
                TextAdd(GetPlayerLogin(index) & " has logged in from " & Socket.ClientIp(index) & ".")
            Else
                AlertMsg(index, "Sorry, that account username is already taken!")
            End If

            Buffer.Dispose()
        End If
    End Sub

    Private Sub Packet_DeleteAccount(index As Integer, ByRef data() As Byte)
        Dim Name As String
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CDelChar", PACKET_LOG)
        TextAdd("Recieved CMSG: CDelChar")

        ' Get the data
        Name = Buffer.ReadString

        If GetPlayerLogin(index) = Trim$(Name) Then
            PlayerMsg(index, "You cannot delete your own account while online!", ColorType.BrightRed)
            Exit Sub
        End If

        For i = 1 To GetPlayersOnline()
            If IsPlaying(i) Then
                If Trim$(Player(i).Login) = Trim$(Name) Then
                    AlertMsg(i, "Your account has been removed by an admin!")
                    ClearPlayer(i)
                End If
            End If
        Next
    End Sub

    Private Sub Packet_Login(index As Integer, ByRef data() As Byte)
        Dim Name As String
        Dim Password As String
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CLogin", PACKET_LOG)
        TextAdd("Recieved CMSG: CLogin")

        If Not IsPlaying(index) Then
            If Not IsLoggedIn(index) Then

                ' Get the data
                Name = EKeyPair.DecryptString(Buffer.ReadString)
                Password = EKeyPair.DecryptString(Buffer.ReadString)

                ' Check versions
                If EKeyPair.DecryptString(Buffer.ReadString) <> Application.ProductVersion Then
                    AlertMsg(index, "Version outdated, please visit " & Options.Website)
                    Exit Sub
                End If

                If isShuttingDown Then
                    AlertMsg(index, "Server is either rebooting or being shutdown.")
                    Exit Sub
                End If

                If Len(Trim$(Name)) < 3 Or Len(Trim$(Password)) < 3 Then
                    AlertMsg(index, "Your name and password must be at least three characters in length")
                    Exit Sub
                End If

                If Not AccountExist(Name) Then
                    AlertMsg(index, "That account name does not exist.")
                    Exit Sub
                End If

                If Not PasswordOK(Name, Password) Then
                    AlertMsg(index, "Incorrect password.")
                    Exit Sub
                End If

                If IsMultiAccounts(Name) Then
                    AlertMsg(index, "Multiple account logins is not authorized.")
                    Exit Sub
                End If

                ' Load the player
                LoadPlayer(index, Name)
                ClearBank(index)
                LoadBank(index, Name)

                Buffer.Dispose()
                Buffer = New ByteStream(4)
                Buffer.WriteInt32(ServerPackets.SLoginOk)
                Buffer.WriteInt32(MAX_CHARS)

                Addlog("Sent SMSG: SLoginOk", PACKET_LOG)
                TextAdd("Sent SMSG: SLoginOk")

                For i = 1 To MAX_CHARS
                    If Player(index).Character(i).Classes <= 0 Then
                        Buffer.WriteString(Trim$(Player(index).Character(i).Name))
                        Buffer.WriteInt32(Player(index).Character(i).Sprite)
                        Buffer.WriteInt32(Player(index).Character(i).Level)
                        Buffer.WriteString("")
                        Buffer.WriteInt32(0)
                    Else
                        Buffer.WriteString(Trim$(Player(index).Character(i).Name))
                        Buffer.WriteInt32(Player(index).Character(i).Sprite)
                        Buffer.WriteInt32(Player(index).Character(i).Level)
                        Buffer.WriteString(Trim$(Classes(Player(index).Character(i).Classes).Name))
                        Buffer.WriteInt32(Player(index).Character(i).Sex)
                    End If

                Next

                Socket.SendDataTo(index, Buffer.Data, Buffer.Head)

                ' Show the player up on the socket status
                Addlog(GetPlayerLogin(index) & " has logged in from " & Socket.ClientIp(index) & ".", PLAYER_LOG)
                TextAdd(GetPlayerLogin(index) & " has logged in from " & Socket.ClientIp(index) & ".")

                '' Check if character data has been created
                'If Len(Trim$(Player(index).Character(TempPlayer(index).CurChar).Name)) > 0 Then
                '    ' we have a char!
                '    'HandleUseChar(index)
                'Else
                '    ' send new char shit
                '    If Not IsPlaying(index) Then
                '        SendNewCharClasses(index)
                '    End If
                'End If

                Buffer.Dispose()
            End If
        End If
    End Sub

    Private Sub Packet_UseChar(index As Integer, ByRef data() As Byte)
        Dim slot As Byte
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CUseChar", PACKET_LOG)
        TextAdd("Recieved CMSG: CUseChar")

        If Not IsPlaying(index) Then
            If IsLoggedIn(index) Then

                slot = Buffer.ReadInt32

                ' Check if character data has been created
                If Len(Trim$(Player(index).Character(slot).Name)) > 0 Then
                    ' we have a char!
                    TempPlayer(index).CurChar = slot
                    HandleUseChar(index)
                    ClearBank(index)
                    LoadBank(index, Trim$(Player(index).Login))
                Else
                    ' send new char shit
                    If Not IsPlaying(index) Then
                        SendNewCharClasses(index)
                        TempPlayer(index).CurChar = slot
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub Packet_AddChar(index As Integer, ByRef data() As Byte)
        Dim Name As String, slot As Byte
        Dim Sex As Integer
        Dim Classes As Integer
        Dim Sprite As Integer
        Dim i As Integer
        Dim n As Integer

        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CAddChar", PACKET_LOG)
        TextAdd("Recieved CMSG: CAddChar")

        If Not IsPlaying(index) Then
            slot = Buffer.ReadInt32
            Name = Buffer.ReadString
            Sex = Buffer.ReadInt32
            Classes = Buffer.ReadInt32
            Sprite = Buffer.ReadInt32

            ' Prevent hacking
            If Len(Name.Trim) < 3 Then
                AlertMsg(index, "Character name must be at least three characters in length.")
                Exit Sub
            End If

            For i = 1 To Len(Name)
                n = AscW(Mid$(Name, i, 1))

                If Not IsNameLegal(n) Then
                    AlertMsg(index, "Invalid name, only letters, numbers, spaces, and _ allowed in names.")
                    Exit Sub
                End If

            Next

            If (Sex < Enums.Sex.Male) Or (Sex > Enums.Sex.Female) Then Exit Sub

            If Classes < 1 Or Classes > Max_Classes Then Exit Sub

            ' Check if char already exists in slot
            If CharExist(index, slot) Then
                AlertMsg(index, "Character already exists!")
                Exit Sub
            End If

            ' Check if name is already in use
            If FindChar(Name) Then
                AlertMsg(index, "Sorry, but that name is in use!")
                Exit Sub
            End If

            ' Everything went ok, add the character
            TempPlayer(index).CurChar = slot
            AddChar(index, slot, Name, Sex, Classes, Sprite)
            Addlog("Character " & Name & " added to " & GetPlayerLogin(index) & "'s account.", PLAYER_LOG)

            ' log them in!!
            HandleUseChar(index)

            Buffer.Dispose()
        End If

    End Sub

    Private Sub Packet_DeleteChar(index As Integer, ByRef data() As Byte)
        Dim slot As Byte
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CDelChar", PACKET_LOG)
        TextAdd("Recieved CMSG: CDelChar")

        If Not IsPlaying(index) Then
            If IsLoggedIn(index) Then

                slot = Buffer.ReadInt32

                ' Check if character data has been created
                If Len(Trim$(Player(index).Character(slot).Name)) > 0 Then
                    ' we have a char!
                    DeleteName(Trim$(Player(index).Character(slot).Name))
                    ClearCharacter(index, slot)
                    SaveCharacter(index, slot)

                    Buffer.Dispose()
                    Buffer = New ByteStream(4)
                    Buffer.WriteInt32(ServerPackets.SLoginOk)
                    Buffer.WriteInt32(MAX_CHARS)

                    Addlog("Sent SMSG: SLoginOk", PACKET_LOG)
                    TextAdd("Sent SMSG: SLoginOk")

                    For i = 1 To MAX_CHARS
                        If Player(index).Character(i).Classes <= 0 Then
                            Buffer.WriteString(Trim$(Player(index).Character(i).Name))
                            Buffer.WriteInt32(Player(index).Character(i).Sprite)
                            Buffer.WriteInt32(Player(index).Character(i).Level)
                            Buffer.WriteString("")
                            Buffer.WriteInt32(0)
                        Else
                            Buffer.WriteString(Trim$(Player(index).Character(i).Name))
                            Buffer.WriteInt32(Player(index).Character(i).Sprite)
                            Buffer.WriteInt32(Player(index).Character(i).Level)
                            Buffer.WriteString(Trim$(Classes(Player(index).Character(i).Classes).Name))
                            Buffer.WriteInt32(Player(index).Character(i).Sex)
                        End If

                    Next

                    Socket.SendDataTo(index, Buffer.Data, Buffer.Head)
                End If
            End If
        End If

    End Sub

    Private Sub Packet_SayMessage(index As Integer, ByRef data() As Byte)
        Dim msg As String
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CSayMsg", PACKET_LOG)
        TextAdd("Recieved CMSG: CSayMsg")

        'msg = Buffer.ReadString
        msg = ReadUnicodeString(Buffer.ReadBytes)

        Addlog("Map #" & GetPlayerMap(index) & ": " & GetPlayerName(index) & " says, '" & msg & "'", PLAYER_LOG)

        SayMsg_Map(GetPlayerMap(index), index, msg, ColorType.White)
        SendChatBubble(GetPlayerMap(index), index, TargetType.Player, msg, ColorType.White)

        Buffer.Dispose()
    End Sub

    Private Sub Packet_BroadCastMsg(index As Integer, ByRef data() As Byte)
        Dim msg As String
        Dim s As String
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CBroadcastMsg", PACKET_LOG)
        TextAdd("Recieved CMSG: CBroadcastMsg")

        'msg = Buffer.ReadString
        msg = ReadUnicodeString(Buffer.ReadBytes)

        s = "[Global]" & GetPlayerName(index) & ": " & msg
        SayMsg_Global(index, msg, ColorType.White)
        Addlog(s, PLAYER_LOG)
        TextAdd(s)

        Buffer.Dispose()
    End Sub

    Public Sub Packet_PlayerMsg(index As Integer, ByRef Data() As Byte)
        Dim OtherPlayer As String, Msg As String, OtherPlayerIndex As Integer
        Dim Buffer As New ByteStream(Data)
        Addlog("Recieved CMSG: CPlayerMsg", PACKET_LOG)
        TextAdd("Recieved CMSG: CPlayerMsg")

        OtherPlayer = Buffer.ReadString
        'Msg = buffer.ReadString
        Msg = ReadUnicodeString(Buffer.ReadBytes)
        Buffer.Dispose()

        OtherPlayerIndex = FindPlayer(OtherPlayer)
        If OtherPlayerIndex <> index Then
            If OtherPlayerIndex > 0 Then
                Addlog(GetPlayerName(index) & " tells " & GetPlayerName(index) & ", '" & Msg & "'", PLAYER_LOG)
                PlayerMsg(OtherPlayerIndex, GetPlayerName(index) & " tells you, '" & Msg & "'", ColorType.Pink)
                PlayerMsg(index, "You tell " & GetPlayerName(OtherPlayerIndex) & ", '" & Msg & "'", ColorType.Pink)
            Else
                PlayerMsg(index, "Player is not online.", ColorType.BrightRed)
            End If
        Else
            PlayerMsg(index, "Cannot message your self!", ColorType.BrightRed)
        End If
    End Sub

    Private Sub Packet_PlayerMove(index As Integer, ByRef data() As Byte)
        Dim Dir As Integer
        Dim movement As Integer
        Dim tmpX As Integer, tmpY As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CPlayerMove", PACKET_LOG)
        TextAdd("Recieved CMSG: CPlayerMove")

        If TempPlayer(index).GettingMap = True Then Exit Sub

        Dir = Buffer.ReadInt32
        movement = Buffer.ReadInt32
        tmpX = Buffer.ReadInt32
        tmpY = Buffer.ReadInt32
        Buffer.Dispose()

        ' Prevent hacking
        If Dir < Direction.Up Or Dir > Direction.Right Then Exit Sub

        If movement < 1 Or movement > 2 Then Exit Sub

        ' Prevent player from moving if they have casted a skill
        If TempPlayer(index).SkillBuffer > 0 Then
            SendPlayerXY(index)
            Exit Sub
        End If

        'Cant move if in the bank!
        If TempPlayer(index).InBank Then
            SendPlayerXY(index)
            Exit Sub
        End If

        ' if stunned, stop them moving
        If TempPlayer(index).StunDuration > 0 Then
            SendPlayerXY(index)
            Exit Sub
        End If

        ' Prevent player from moving if in shop
        If TempPlayer(index).InShop > 0 Then
            SendPlayerXY(index)
            Exit Sub
        End If

        ' Desynced
        If GetPlayerX(index) <> tmpX Then
            SendPlayerXY(index)
            Exit Sub
        End If

        If GetPlayerY(index) <> tmpY Then
            SendPlayerXY(index)
            Exit Sub
        End If

        PlayerMove(index, Dir, movement, False)

        Addlog(" Player: " & GetPlayerName(index) & " : " & " X: " & tmpX & " Y: " & tmpY & " Dir: " & Dir & " Movement: " & movement, PLAYER_LOG)
        TextAdd(" Player: " & GetPlayerName(index) & " : " & " X: " & tmpX & " Y: " & tmpY & " Dir: " & Dir & " Movement: " & movement)

        Buffer.Dispose()
    End Sub

    Sub Packet_PlayerDirection(Index As Integer, ByRef Data() As Byte)
        Dim dir As Integer
        Dim Buffer As New ByteStream(Data)
        Addlog("Recieved CMSG: CPlayerDir", PACKET_LOG)
        TextAdd("Recieved CMSG: CPlayerDir")

        If TempPlayer(Index).GettingMap = True Then Exit Sub

        dir = Buffer.ReadInt32
        Buffer.Dispose()

        ' Prevent hacking
        If dir < Direction.Up Or dir > Direction.Right Then Exit Sub

        SetPlayerDir(Index, dir)

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SPlayerDir)
        Buffer.WriteInt32(Index)
        Buffer.WriteInt32(GetPlayerDir(Index))
        SendDataToMapBut(Index, GetPlayerMap(Index), Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SPlayerDir", PACKET_LOG)
        TextAdd("Sent SMSG: SPlayerDir")

        Buffer.Dispose()

    End Sub

    Sub Packet_UseItem(Index As Integer, ByRef Data() As Byte)
        Dim invnum As Integer
        Dim Buffer As New ByteStream(Data)
        Addlog("Recieved CMSG: CUseItem", PACKET_LOG)
        TextAdd("Recieved CMSG: CUseItem")

        invnum = Buffer.ReadInt32
        Buffer.Dispose()

        UseItem(Index, invnum)
    End Sub

    Sub Packet_Attack(Index As Integer, ByRef Data() As Byte)
        Dim i As Integer
        Dim TempIndex As Integer
        Dim x As Integer, y As Integer
        Dim Buffer As New ByteStream(Data)
        Addlog("Recieved CMSG: CAttack", PACKET_LOG)
        TextAdd("Recieved CMSG: CAttack")

        ' can't attack whilst casting
        If TempPlayer(Index).SkillBuffer > 0 Then Exit Sub

        ' can't attack whilst stunned
        If TempPlayer(Index).StunDuration > 0 Then Exit Sub

        ' Send this packet so they can see the person attacking
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SAttack)
        Buffer.WriteInt32(Index)
        SendDataToMap(GetPlayerMap(Index), Buffer.Data, Buffer.Head)
        Buffer.Dispose()

        ' Projectile check
        If GetPlayerEquipment(Index, EquipmentType.Weapon) > 0 Then
            If Item(GetPlayerEquipment(Index, EquipmentType.Weapon)).Projectile > 0 Then 'Item has a projectile
                If Item(GetPlayerEquipment(Index, EquipmentType.Weapon)).Ammo > 0 Then
                    If HasItem(Index, Item(GetPlayerEquipment(Index, EquipmentType.Weapon)).Ammo) Then
                        TakeInvItem(Index, Item(GetPlayerEquipment(Index, EquipmentType.Weapon)).Ammo, 1)
                        PlayerFireProjectile(Index)
                        Exit Sub
                    Else
                        PlayerMsg(Index, "No More " & Item(Item(GetPlayerEquipment(Index, EquipmentType.Weapon)).Ammo).Name & " !", ColorType.BrightRed)
                        Exit Sub
                    End If
                Else
                    PlayerFireProjectile(Index)
                    Exit Sub
                End If
            End If
        End If

        ' Try to attack a player
        For i = 1 To GetPlayersOnline()
            TempIndex = i

            ' Make sure we dont try to attack ourselves
            If TempIndex <> Index Then
                If IsPlaying(TempIndex) Then
                    TryPlayerAttackPlayer(Index, i)
                End If
            End If
        Next

        ' Try to attack a npc
        For i = 1 To MAX_MAP_NPCS
            TryPlayerAttackNpc(Index, i)
        Next

        ' Check tradeskills
        Select Case GetPlayerDir(Index)
            Case Direction.Up

                If GetPlayerY(Index) = 0 Then Exit Sub
                x = GetPlayerX(Index)
                y = GetPlayerY(Index) - 1
            Case Direction.Down

                If GetPlayerY(Index) = Map(GetPlayerMap(Index)).MaxY Then Exit Sub
                x = GetPlayerX(Index)
                y = GetPlayerY(Index) + 1
            Case Direction.Left

                If GetPlayerX(Index) = 0 Then Exit Sub
                x = GetPlayerX(Index) - 1
                y = GetPlayerY(Index)
            Case Direction.Right

                If GetPlayerX(Index) = Map(GetPlayerMap(Index)).MaxX Then Exit Sub
                x = GetPlayerX(Index) + 1
                y = GetPlayerY(Index)
        End Select

        CheckResource(Index, x, y)

        Buffer.Dispose()
    End Sub

    Sub Packet_PlayerInfo(Index As Integer, ByRef Data() As Byte)
        Dim i As Integer, n As Integer
        Dim name As String
        Dim Buffer As New ByteStream(Data)
        Addlog("Recieved CMSG: CPlayerInfoRequest", PACKET_LOG)
        TextAdd("Recieved CMSG: CPlayerInfoRequest")

        name = Buffer.ReadString
        i = FindPlayer(name)

        If i > 0 Then
            PlayerMsg(Index, "Account:  " & Trim$(Player(i).Login) & ", Name: " & GetPlayerName(i), ColorType.Yellow)

            If GetPlayerAccess(Index) > AdminType.Monitor Then
                PlayerMsg(Index, "-=- Stats for " & GetPlayerName(i) & " -=-", ColorType.Yellow)
                PlayerMsg(Index, "Level: " & GetPlayerLevel(i) & "  Exp: " & GetPlayerExp(i) & "/" & GetPlayerNextLevel(i), ColorType.Yellow)
                PlayerMsg(Index, "HP: " & GetPlayerVital(i, Vitals.HP) & "/" & GetPlayerMaxVital(i, Vitals.HP) & "  MP: " & GetPlayerVital(i, Vitals.MP) & "/" & GetPlayerMaxVital(i, Vitals.MP) & "  SP: " & GetPlayerVital(i, Vitals.SP) & "/" & GetPlayerMaxVital(i, Vitals.SP), ColorType.Yellow)
                PlayerMsg(Index, "Strength: " & GetPlayerStat(i, Stats.Strength) & "  Defense: " & GetPlayerStat(i, Stats.Endurance) & "  Magic: " & GetPlayerStat(i, Stats.Intelligence) & "  Speed: " & GetPlayerStat(i, Stats.Spirit), ColorType.Yellow)
                n = (GetPlayerStat(i, Stats.Strength) \ 2) + (GetPlayerLevel(i) \ 2)
                i = (GetPlayerStat(i, Stats.Endurance) \ 2) + (GetPlayerLevel(i) \ 2)

                If n > 100 Then n = 100
                If i > 100 Then i = 100
                PlayerMsg(Index, "Critical Hit Chance: " & n & "%, Block Chance: " & i & "%", ColorType.Yellow)
            End If

        Else
            PlayerMsg(Index, "Player is not online.", ColorType.BrightRed)
        End If

        Buffer.Dispose()
    End Sub

    Sub Packet_WarpMeTo(ByVal Index As Integer, ByRef data() As Byte)
        Dim n As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CWarpMeTo", PACKET_LOG)
        TextAdd("Recieved CMSG: CWarpMeTo")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Mapper Then Exit Sub

        ' The player
        n = FindPlayer(Buffer.ReadString)
        Buffer.Dispose()

        If n <> Index Then
            If n > 0 Then
                PlayerWarp(Index, GetPlayerMap(n), GetPlayerX(n), GetPlayerY(n))
                PlayerMsg(n, GetPlayerName(Index) & " has warped to you.", ColorType.Yellow)
                PlayerMsg(Index, "You have been warped to " & GetPlayerName(n) & ".", ColorType.Yellow)
                Addlog(GetPlayerName(Index) & " has warped to " & GetPlayerName(n) & ", map #" & GetPlayerMap(n) & ".", ADMIN_LOG)
            Else
                PlayerMsg(Index, "Player is not online.", ColorType.BrightRed)
            End If

        Else
            PlayerMsg(Index, "You cannot warp to yourself, dumbass!", ColorType.BrightRed)
        End If

    End Sub

    Sub Packet_WarpToMe(ByVal Index As Integer, ByRef data() As Byte)
        Dim n As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CWarpToMe", PACKET_LOG)
        TextAdd("Recieved CMSG: CWarpToMe")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Mapper Then Exit Sub

        ' The player
        n = FindPlayer(Buffer.ReadString)
        Buffer.Dispose()

        If n <> Index Then
            If n > 0 Then
                PlayerWarp(n, GetPlayerMap(Index), GetPlayerX(Index), GetPlayerY(Index))
                PlayerMsg(n, "You have been summoned by " & GetPlayerName(Index) & ".", ColorType.Yellow)
                PlayerMsg(Index, GetPlayerName(n) & " has been summoned.", ColorType.Yellow)
                Addlog(GetPlayerName(Index) & " has warped " & GetPlayerName(n) & " to self, map #" & GetPlayerMap(Index) & ".", ADMIN_LOG)
            Else
                PlayerMsg(Index, "Player is not online.", ColorType.BrightRed)
            End If

        Else
            PlayerMsg(Index, "You cannot warp yourself to yourself, dumbass!", ColorType.BrightRed)
        End If

    End Sub

    Sub Packet_WarpTo(ByVal Index As Integer, ByRef data() As Byte)
        Dim n As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CWarpTo", PACKET_LOG)
        TextAdd("Recieved CMSG: CWarpTo")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Mapper Then Exit Sub

        ' The map
        n = Buffer.ReadInt32
        Buffer.Dispose()

        ' Prevent hacking
        If n < 0 Or n > MAX_CACHED_MAPS Then Exit Sub

        PlayerWarp(Index, n, GetPlayerX(Index), GetPlayerY(Index))
        PlayerMsg(Index, "You have been warped to map #" & n, ColorType.Yellow)
        Addlog(GetPlayerName(Index) & " warped to map #" & n & ".", ADMIN_LOG)

    End Sub

    Sub Packet_SetSprite(ByVal Index As Integer, ByRef data() As Byte)
        Dim n As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CSetSprite", PACKET_LOG)
        TextAdd("Recieved CMSG: CSetSprite")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Mapper Then Exit Sub

        ' The sprite
        n = Buffer.ReadInt32
        Buffer.Dispose()

        SetPlayerSprite(Index, n)
        SendPlayerData(Index)

    End Sub

    Sub Packet_GetStats(ByVal Index As Integer, ByRef data() As Byte)
        Dim i As Integer
        Dim n As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CGetStats", PACKET_LOG)
        TextAdd("Recieved CMSG: CGetStats")

        PlayerMsg(Index, "-=- Stats for " & GetPlayerName(Index) & " -=-", ColorType.Yellow)
        PlayerMsg(Index, "Level: " & GetPlayerLevel(Index) & "  Exp: " & GetPlayerExp(Index) & "/" & GetPlayerNextLevel(Index), ColorType.Yellow)
        PlayerMsg(Index, "HP: " & GetPlayerVital(Index, Vitals.HP) & "/" & GetPlayerMaxVital(Index, Vitals.HP) & "  MP: " & GetPlayerVital(Index, Vitals.MP) & "/" & GetPlayerMaxVital(Index, Vitals.MP) & "  SP: " & GetPlayerVital(Index, Vitals.SP) & "/" & GetPlayerMaxVital(Index, Vitals.SP), ColorType.Yellow)
        PlayerMsg(Index, "STR: " & GetPlayerStat(Index, Stats.Strength) & "  DEF: " & GetPlayerStat(Index, Stats.Endurance) & "  MAGI: " & GetPlayerStat(Index, Stats.Intelligence) & "  Speed: " & GetPlayerStat(Index, Stats.Spirit), ColorType.Yellow)
        n = (GetPlayerStat(Index, Stats.Strength) \ 2) + (GetPlayerLevel(Index) \ 2)
        i = (GetPlayerStat(Index, Stats.Endurance) \ 2) + (GetPlayerLevel(Index) \ 2)

        If n > 100 Then n = 100
        If i > 100 Then i = 100
        PlayerMsg(Index, "Critical Hit Chance: " & n & "%, Block Chance: " & i & "%", ColorType.Yellow)
        Buffer.Dispose()
    End Sub

    Sub Packet_RequestNewMap(ByVal Index As Integer, ByRef data() As Byte)
        Dim dir As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CRequestNewMap", PACKET_LOG)
        TextAdd("Recieved CMSG: CRequestNewMap")

        dir = Buffer.ReadInt32
        Buffer.Dispose()

        ' Prevent hacking
        If dir < Direction.Up Or dir > Direction.Right Then Exit Sub

        PlayerMove(Index, dir, 1, True)
    End Sub

    Sub Packet_MapData(ByVal Index As Integer, ByRef data() As Byte)
        Dim i As Integer
        Dim MapNum As Integer
        Dim x As Integer
        Dim y As Integer

        Addlog("Recieved CMSG: CSaveMap", PACKET_LOG)
        TextAdd("Recieved CMSG: CSaveMap")

        Dim Buffer As New ByteStream(Compression.DecompressBytes(data))

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Mapper Then Exit Sub

        Gettingmap = True

        MapNum = GetPlayerMap(Index)

        i = Map(MapNum).Revision + 1
        ClearMap(MapNum)

        Map(MapNum).Name = Buffer.ReadString
        Map(MapNum).Music = Buffer.ReadString
        Map(MapNum).Revision = i
        Map(MapNum).Moral = Buffer.ReadInt32
        Map(MapNum).Tileset = Buffer.ReadInt32
        Map(MapNum).Up = Buffer.ReadInt32
        Map(MapNum).Down = Buffer.ReadInt32
        Map(MapNum).Left = Buffer.ReadInt32
        Map(MapNum).Right = Buffer.ReadInt32
        Map(MapNum).BootMap = Buffer.ReadInt32
        Map(MapNum).BootX = Buffer.ReadInt32
        Map(MapNum).BootY = Buffer.ReadInt32
        Map(MapNum).MaxX = Buffer.ReadInt32
        Map(MapNum).MaxY = Buffer.ReadInt32
        Map(MapNum).WeatherType = Buffer.ReadInt32
        Map(MapNum).FogIndex = Buffer.ReadInt32
        Map(MapNum).WeatherIntensity = Buffer.ReadInt32
        Map(MapNum).FogAlpha = Buffer.ReadInt32
        Map(MapNum).FogSpeed = Buffer.ReadInt32
        Map(MapNum).HasMapTint = Buffer.ReadInt32
        Map(MapNum).MapTintR = Buffer.ReadInt32
        Map(MapNum).MapTintG = Buffer.ReadInt32
        Map(MapNum).MapTintB = Buffer.ReadInt32
        Map(MapNum).MapTintA = Buffer.ReadInt32

        Map(MapNum).Instanced = Buffer.ReadInt32
        Map(MapNum).Panorama = Buffer.ReadInt32
        Map(MapNum).Parallax = Buffer.ReadInt32

        ReDim Map(MapNum).Tile(0 To Map(MapNum).MaxX, 0 To Map(MapNum).MaxY)

        For x = 1 To MAX_MAP_NPCS
            ClearMapNpc(x, MapNum)
            Map(MapNum).Npc(x) = Buffer.ReadInt32
        Next

        With Map(MapNum)
            For x = 0 To .MaxX
                For y = 0 To .MaxY
                    .Tile(x, y).Data1 = Buffer.ReadInt32
                    .Tile(x, y).Data2 = Buffer.ReadInt32
                    .Tile(x, y).Data3 = Buffer.ReadInt32
                    .Tile(x, y).DirBlock = Buffer.ReadInt32
                    ReDim .Tile(x, y).Layer(0 To MapLayer.Count - 1)
                    For i = 0 To MapLayer.Count - 1
                        .Tile(x, y).Layer(i).Tileset = Buffer.ReadInt32
                        .Tile(x, y).Layer(i).X = Buffer.ReadInt32
                        .Tile(x, y).Layer(i).Y = Buffer.ReadInt32
                        .Tile(x, y).Layer(i).AutoTile = Buffer.ReadInt32
                    Next
                    .Tile(x, y).Type = Buffer.ReadInt32
                Next
            Next

        End With

        'Event Data!
        Map(MapNum).EventCount = Buffer.ReadInt32

        If Map(MapNum).EventCount > 0 Then
            ReDim Map(MapNum).Events(0 To Map(MapNum).EventCount)
            For i = 1 To Map(MapNum).EventCount
                With Map(MapNum).Events(i)
                    .Name = Buffer.ReadString
                    .Globals = Buffer.ReadInt32
                    .X = Buffer.ReadInt32
                    .Y = Buffer.ReadInt32
                    .PageCount = Buffer.ReadInt32
                End With
                If Map(MapNum).Events(i).PageCount > 0 Then
                    ReDim Map(MapNum).Events(i).Pages(0 To Map(MapNum).Events(i).PageCount)
                    ReDim TempPlayer(i).EventMap.EventPages(0 To Map(MapNum).Events(i).PageCount)
                    For x = 1 To Map(MapNum).Events(i).PageCount
                        With Map(MapNum).Events(i).Pages(x)
                            .chkVariable = Buffer.ReadInt32
                            .VariableIndex = Buffer.ReadInt32
                            .VariableCondition = Buffer.ReadInt32
                            .VariableCompare = Buffer.ReadInt32

                            Map(MapNum).Events(i).Pages(x).chkSwitch = Buffer.ReadInt32
                            Map(MapNum).Events(i).Pages(x).SwitchIndex = Buffer.ReadInt32
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
                                ReDim Map(MapNum).Events(i).Pages(x).MoveRoute(.MoveRouteCount)
                                For y = 1 To .MoveRouteCount
                                    .MoveRoute(y).Index = Buffer.ReadInt32
                                    .MoveRoute(y).Data1 = Buffer.ReadInt32
                                    .MoveRoute(y).Data2 = Buffer.ReadInt32
                                    .MoveRoute(y).Data3 = Buffer.ReadInt32
                                    .MoveRoute(y).Data4 = Buffer.ReadInt32
                                    .MoveRoute(y).Data5 = Buffer.ReadInt32
                                    .MoveRoute(y).Data6 = Buffer.ReadInt32
                                Next
                            End If

                            .WalkAnim = Buffer.ReadInt32
                            .DirFix = Buffer.ReadInt32
                            .WalkThrough = Buffer.ReadInt32
                            .ShowName = Buffer.ReadInt32
                            .Trigger = Buffer.ReadInt32
                            .CommandListCount = Buffer.ReadInt32

                            .Position = Buffer.ReadInt32
                            .QuestNum = Buffer.ReadInt32

                            .chkPlayerGender = Buffer.ReadInt32
                        End With

                        If Map(MapNum).Events(i).Pages(x).CommandListCount > 0 Then
                            ReDim Map(MapNum).Events(i).Pages(x).CommandList(0 To Map(MapNum).Events(i).Pages(x).CommandListCount)
                            For y = 1 To Map(MapNum).Events(i).Pages(x).CommandListCount
                                Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount = Buffer.ReadInt32
                                Map(MapNum).Events(i).Pages(x).CommandList(y).ParentList = Buffer.ReadInt32
                                If Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount > 0 Then
                                    ReDim Map(MapNum).Events(i).Pages(x).CommandList(y).Commands(0 To Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount)
                                    For z = 1 To Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount
                                        With Map(MapNum).Events(i).Pages(x).CommandList(y).Commands(z)
                                            .Index = Buffer.ReadInt32
                                            .Text1 = Buffer.ReadString
                                            .Text2 = Buffer.ReadString
                                            .Text3 = Buffer.ReadString
                                            .Text4 = Buffer.ReadString
                                            .Text5 = Buffer.ReadString
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
                                            Dim tmpcount As Integer = .MoveRouteCount
                                            If tmpcount > 0 Then
                                                ReDim Preserve .MoveRoute(tmpcount)
                                                For w = 1 To tmpcount
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

        ' Save the map
        SaveMap(MapNum)
        SaveMapEvent(MapNum)

        Gettingmap = False

        SendMapNpcsToMap(MapNum)
        SpawnMapNpcs(MapNum)
        SpawnGlobalEvents(MapNum)

        For i = 1 To GetPlayersOnline()
            If IsPlaying(i) Then
                If Player(i).Character(TempPlayer(i).CurChar).Map = MapNum Then
                    SpawnMapEventsFor(i, MapNum)
                End If
            End If
        Next

        ' Clear it all out 
        For i = 1 To MAX_MAP_ITEMS
            SpawnItemSlot(i, 0, 0, GetPlayerMap(Index), MapItem(GetPlayerMap(Index), i).X, MapItem(GetPlayerMap(Index), i).Y)
            ClearMapItem(i, GetPlayerMap(Index))
        Next

        ' Respawn
        SpawnMapItems(GetPlayerMap(Index))

        ClearTempTile(MapNum)
        CacheResources(MapNum)

        ' Refresh map for everyone online
        For i = 1 To GetPlayersOnline()
            If IsPlaying(i) And GetPlayerMap(i) = MapNum Then
                PlayerWarp(i, MapNum, GetPlayerX(i), GetPlayerY(i))
                ' Send map
                SendMapData(i, MapNum, True)
            End If
        Next

        Buffer.Dispose()
    End Sub

    Private Sub Packet_NeedMap(ByVal Index As Integer, ByRef data() As Byte)
        Dim s As String
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CNeedMap", PACKET_LOG)
        TextAdd("Recieved CMSG: CNeedMap")

        ' Get yes/no value
        s = Buffer.ReadInt32
        Buffer.Dispose()

        ' Check if map data is needed to be sent
        If s = 1 Then
            SendMapData(Index, GetPlayerMap(Index), True)
        Else
            SendMapData(Index, GetPlayerMap(Index), False)
        End If

        SpawnMapEventsFor(Index, GetPlayerMap(Index))
        SendJoinMap(Index)
        TempPlayer(Index).GettingMap = False
    End Sub

    Private Sub Packet_GetItem(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CMapGetItem", PACKET_LOG)
        TextAdd("Recieved CMSG: CMapGetItem")

        PlayerMapGetItem(Index)
    End Sub

    Private Sub Packet_DropItem(ByVal Index As Integer, ByRef data() As Byte)
        Dim InvNum As Integer, Amount As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CMapDropItem", PACKET_LOG)
        TextAdd("Recieved CMSG: CMapDropItem")

        InvNum = Buffer.ReadInt32
        Amount = Buffer.ReadInt32
        Buffer.Dispose()

        If TempPlayer(Index).InBank Or TempPlayer(Index).InShop Then Exit Sub

        ' Prevent hacking
        If InvNum < 1 Or InvNum > MAX_INV Then Exit Sub
        If GetPlayerInvItemNum(Index, InvNum) < 1 Or GetPlayerInvItemNum(Index, InvNum) > MAX_ITEMS Then Exit Sub
        If Item(GetPlayerInvItemNum(Index, InvNum)).Type = ItemType.Currency Or Item(GetPlayerInvItemNum(Index, InvNum)).Stackable = 1 Then
            If Amount < 1 Or Amount > GetPlayerInvItemValue(Index, InvNum) Then Exit Sub
        End If

        ' everything worked out fine
        PlayerMapDropItem(Index, InvNum, Amount)
    End Sub

    Sub Packet_RespawnMap(ByVal Index As Integer, ByRef data() As Byte)
        Dim i As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CMapRespawn", PACKET_LOG)
        TextAdd("Recieved CMSG: CMapRespawn")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Mapper Then Exit Sub

        ' Clear out it all
        For i = 1 To MAX_MAP_ITEMS
            SpawnItemSlot(i, 0, 0, GetPlayerMap(Index), MapItem(GetPlayerMap(Index), i).X, MapItem(GetPlayerMap(Index), i).Y)
            ClearMapItem(i, GetPlayerMap(Index))
        Next

        ' Respawn
        SpawnMapItems(GetPlayerMap(Index))

        ' Respawn NPCS
        For i = 1 To MAX_MAP_NPCS
            SpawnNpc(i, GetPlayerMap(Index))
        Next

        CacheResources(GetPlayerMap(Index))
        PlayerMsg(Index, "Map respawned.", ColorType.BrightGreen)
        Addlog(GetPlayerName(Index) & " has respawned map #" & GetPlayerMap(Index), ADMIN_LOG)

        Buffer.Dispose()
    End Sub

    ' ::::::::::::::::::::::::
    ' :: Kick player packet ::
    ' ::::::::::::::::::::::::
    Sub Packet_KickPlayer(ByVal Index As Integer, ByRef data() As Byte)
        Dim n As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CKickPlayer", PACKET_LOG)
        TextAdd("Recieved CMSG: CKickPlayer")

        ' Prevent hacking
        If GetPlayerAccess(Index) <= 0 Then
            Exit Sub
        End If

        ' The player index
        n = FindPlayer(Buffer.ReadString) 'Parse(1))
        Buffer.Dispose()

        If n <> Index Then
            If n > 0 Then
                If GetPlayerAccess(n) < GetPlayerAccess(Index) Then
                    GlobalMsg(GetPlayerName(n) & " has been kicked from " & Options.GameName & " by " & GetPlayerName(Index) & "!")
                    Addlog(GetPlayerName(Index) & " has kicked " & GetPlayerName(n) & ".", ADMIN_LOG)
                    AlertMsg(n, "You have been kicked by " & GetPlayerName(Index) & "!")
                Else
                    PlayerMsg(Index, "That is a higher or same access admin then you!", ColorType.BrightRed)
                End If

            Else
                PlayerMsg(Index, "Player is not online.", ColorType.BrightRed)
            End If

        Else
            PlayerMsg(Index, "You cannot kick yourself!", ColorType.BrightRed)
        End If
    End Sub

    Sub Packet_Banlist(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CBanList", PACKET_LOG)
        TextAdd("Recieved CMSG: CBanList")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Mapper Then
            Exit Sub
        End If

        PlayerMsg(Index, "Command /banlist is not available in Orion+... yet ;)", ColorType.Yellow)
    End Sub

    Sub Packet_DestroyBans(ByVal Index As Integer, ByRef data() As Byte)
        Dim filename As String

        Addlog("Recieved CMSG: CBanDestory", PACKET_LOG)
        TextAdd("Recieved CMSG: CBanDestory")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Creator Then
            Exit Sub
        End If

        filename = Application.StartupPath & "\data\banlist.txt"

        If File.Exists(filename) Then Kill(filename)

        PlayerMsg(Index, "Ban list destroyed.", ColorType.BrightGreen)
    End Sub

    ' :::::::::::::::::::::::
    ' :: Ban player packet ::
    ' :::::::::::::::::::::::
    Sub Packet_BanPlayer(ByVal Index As Integer, ByRef data() As Byte)
        Dim n As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CBanPlayer", PACKET_LOG)
        TextAdd("Recieved CMSG: CBanPlayer")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Mapper Then
            Exit Sub
        End If

        ' The player index
        n = FindPlayer(Buffer.ReadString)
        Buffer.Dispose()

        If n <> Index Then
            If n > 0 Then
                If GetPlayerAccess(n) < GetPlayerAccess(Index) Then
                    BanIndex(n, Index)
                Else
                    PlayerMsg(Index, "That is a higher or same access admin then you!", ColorType.BrightRed)
                End If

            Else
                PlayerMsg(Index, "Player is not online.", ColorType.BrightRed)
            End If

        Else
            PlayerMsg(Index, "You cannot ban yourself, dumbass!", ColorType.BrightRed)
        End If

    End Sub

    Private Sub Packet_EditMapRequest(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CRequestEditMap", PACKET_LOG)
        TextAdd("Recieved CMSG: CRequestEditMap")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Mapper Then Exit Sub

        If GetPlayerMap(Index) > MAX_MAPS Then
            PlayerMsg(Index, "Cant edit instanced maps!", ColorType.BrightRed)
            Exit Sub
        End If
        SendMapEventData(Index)

        Dim Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SEditMap)
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Private Sub Packet_EditItem(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved EMSG: RequestEditItem", PACKET_LOG)
        TextAdd("Recieved EMSG: RequestEditItem")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Mapper Then
            Exit Sub
        End If

        Dim Buffer = New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SItemEditor)
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SItemEditor", PACKET_LOG)
        TextAdd("Sent SMSG: SItemEditor")

        Buffer.Dispose()
    End Sub

    Private Sub Packet_SaveItem(ByVal Index As Integer, ByRef data() As Byte)
        Dim n As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved EMSG: SaveItem", PACKET_LOG)
        TextAdd("Recieved EMSG: SaveItem")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Developer Then Exit Sub

        n = Buffer.ReadInt32

        If n < 0 Or n > MAX_ITEMS Then
            Exit Sub
        End If

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
        Item(n).Name = Trim$(Buffer.ReadString)
        Item(n).Paperdoll = Buffer.ReadInt32()
        Item(n).Pic = Buffer.ReadInt32()
        Item(n).Price = Buffer.ReadInt32()
        Item(n).Rarity = Buffer.ReadInt32()
        Item(n).Speed = Buffer.ReadInt32()

        Item(n).Randomize = Buffer.ReadInt32()
        Item(n).RandomMin = Buffer.ReadInt32()
        Item(n).RandomMax = Buffer.ReadInt32()

        Item(n).Stackable = Buffer.ReadInt32()
        Item(n).Description = Trim$(Buffer.ReadString)

        For i = 0 To Stats.Count - 1
            Item(n).Stat_Req(i) = Buffer.ReadInt32()
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

        ' Save it
        SendUpdateItemToAll(n)
        SaveItem(n)
        Addlog(GetPlayerLogin(Index) & " saved item #" & n & ".", ADMIN_LOG)
        Buffer.Dispose()
    End Sub

    Sub Packet_EditNpc(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved EMSG: RequestEditNpc", PACKET_LOG)
        TextAdd("Recieved EMSG: RequestEditNpc")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Developer Then
            Exit Sub
        End If

        Dim Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SNpcEditor)
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SNpcEditor", PACKET_LOG)
        TextAdd("Sent SMSG: SNpcEditor")

        Buffer.Dispose()
    End Sub

    Sub Packet_SaveNPC(ByVal Index As Integer, ByRef data() As Byte)
        Dim NpcNum As Integer, i As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved EMSG: SaveNpc", PACKET_LOG)
        TextAdd("Recieved EMSG: SaveNpc")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Developer Then
            Exit Sub
        End If

        NpcNum = Buffer.ReadInt32

        ' Update the Npc
        Npc(NpcNum).Animation = Buffer.ReadInt32()
        Npc(NpcNum).AttackSay = Buffer.ReadString()
        Npc(NpcNum).Behaviour = Buffer.ReadInt32()
        For i = 1 To 5
            Npc(NpcNum).DropChance(i) = Buffer.ReadInt32()
            Npc(NpcNum).DropItem(i) = Buffer.ReadInt32()
            Npc(NpcNum).DropItemValue(i) = Buffer.ReadInt32()
        Next

        Npc(NpcNum).Exp = Buffer.ReadInt32()
        Npc(NpcNum).Faction = Buffer.ReadInt32()
        Npc(NpcNum).Hp = Buffer.ReadInt32()
        Npc(NpcNum).Name = Buffer.ReadString()
        Npc(NpcNum).Range = Buffer.ReadInt32()
        Npc(NpcNum).SpawnTime = Buffer.ReadInt32()
        Npc(NpcNum).SpawnSecs = Buffer.ReadInt32()
        Npc(NpcNum).Sprite = Buffer.ReadInt32()

        For i = 0 To Stats.Count - 1
            Npc(NpcNum).Stat(i) = Buffer.ReadInt32()
        Next

        Npc(NpcNum).QuestNum = Buffer.ReadInt32()

        For i = 1 To MAX_NPC_SKILLS
            Npc(NpcNum).Skill(i) = Buffer.ReadInt32()
        Next

        Npc(NpcNum).Level = Buffer.ReadInt32()
        Npc(NpcNum).Damage = Buffer.ReadInt32()

        ' Save it
        SendUpdateNpcToAll(NpcNum)
        SaveNpc(NpcNum)
        Addlog(GetPlayerLogin(Index) & " saved Npc #" & NpcNum & ".", ADMIN_LOG)

        Buffer.Dispose()
    End Sub

    Sub Packet_EditShop(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved EMSG: RequestEditShop", PACKET_LOG)
        TextAdd("Recieved EMSG: RequestEditShop")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Developer Then
            Exit Sub
        End If

        Dim Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SShopEditor)
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SShopEditor", PACKET_LOG)
        TextAdd("Sent SMSG: SShopEditor")

        Buffer.Dispose()
    End Sub

    Sub Packet_SaveShop(ByVal Index As Integer, ByRef data() As Byte)
        Dim ShopNum As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved EMSG: SaveShop", PACKET_LOG)
        TextAdd("Recieved EMSG: SaveShop")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Developer Then
            Exit Sub
        End If

        ShopNum = Buffer.ReadInt32

        ' Prevent hacking
        If ShopNum < 0 Or ShopNum > MAX_SHOPS Then
            Exit Sub
        End If

        Shop(ShopNum).BuyRate = Buffer.ReadInt32()
        Shop(ShopNum).Name = Buffer.ReadString()
        Shop(ShopNum).Face = Buffer.ReadInt32()

        For i = 0 To MAX_TRADES
            Shop(ShopNum).TradeItem(i).CostItem = Buffer.ReadInt32()
            Shop(ShopNum).TradeItem(i).CostValue = Buffer.ReadInt32()
            Shop(ShopNum).TradeItem(i).Item = Buffer.ReadInt32()
            Shop(ShopNum).TradeItem(i).ItemValue = Buffer.ReadInt32()
        Next

        If Shop(ShopNum).Name Is Nothing Then Shop(ShopNum).Name = ""

        Buffer.Dispose()
        ' Save it
        Call SendUpdateShopToAll(ShopNum)
        Call SaveShop(ShopNum)
        Call Addlog(GetPlayerLogin(Index) & " saving shop #" & ShopNum & ".", ADMIN_LOG)
    End Sub

    Sub Packet_EditSkill(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved EMSG: RequestEditSkill", PACKET_LOG)
        TextAdd("Recieved EMSG: RequestEditSkill")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Developer Then
            Exit Sub
        End If

        Dim Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SSkillEditor)
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SSkillEditor", PACKET_LOG)
        TextAdd("Sent SMSG: SSkillEditor")

        Buffer.Dispose()
    End Sub

    Sub Packet_SaveSkill(ByVal Index As Integer, ByRef data() As Byte)
        Dim skillnum As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved EMSG: SaveSkill", PACKET_LOG)
        TextAdd("Recieved EMSG: SaveSkill")

        skillnum = Buffer.ReadInt32

        ' Prevent hacking
        If skillnum < 0 Or skillnum > MAX_SKILLS Then
            Exit Sub
        End If

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
        Skill(skillnum).Name = Buffer.ReadString()
        Skill(skillnum).Range = Buffer.ReadInt32()
        Skill(skillnum).SkillAnim = Buffer.ReadInt32()
        Skill(skillnum).StunDuration = Buffer.ReadInt32()
        Skill(skillnum).Type = Buffer.ReadInt32()
        Skill(skillnum).Vital = Buffer.ReadInt32()
        Skill(skillnum).X = Buffer.ReadInt32()
        Skill(skillnum).Y = Buffer.ReadInt32()

        'projectiles
        Skill(skillnum).IsProjectile = Buffer.ReadInt32()
        Skill(skillnum).Projectile = Buffer.ReadInt32()

        Skill(skillnum).KnockBack = Buffer.ReadInt32()
        Skill(skillnum).KnockBackTiles = Buffer.ReadInt32()

        ' Save it
        SendUpdateSkillToAll(skillnum)
        SaveSkill(skillnum)
        Addlog(GetPlayerLogin(Index) & " saved Skill #" & skillnum & ".", ADMIN_LOG)

        Buffer.Dispose()
    End Sub

    Sub Packet_SetAccess(ByVal Index As Integer, ByRef data() As Byte)
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CSetAccess", PACKET_LOG)
        TextAdd("Recieved CMSG: CSetAccess")

        Dim n As Integer
        Dim i As Integer

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Creator Then Exit Sub

        ' The index
        n = FindPlayer(Buffer.ReadString)
        ' The access
        i = Buffer.ReadInt32

        ' Check for invalid access level
        If i >= 0 Or i <= 3 Then

            ' Check if player is on
            If n > 0 Then

                'check to see if same level access is trying to change another access of the very same level and boot them if they are.
                If GetPlayerAccess(n) = GetPlayerAccess(Index) Then
                    PlayerMsg(Index, "Invalid access level.", ColorType.BrightRed)
                    Exit Sub
                End If

                If GetPlayerAccess(n) <= 0 Then
                    GlobalMsg(GetPlayerName(n) & " has been blessed with administrative access.")
                End If

                SetPlayerAccess(n, i)
                SendPlayerData(n)
                Addlog(GetPlayerName(Index) & " has modified " & GetPlayerName(n) & "'s access.", ADMIN_LOG)
            Else
                PlayerMsg(Index, "Player is not online.", ColorType.BrightRed)
            End If

        Else
            PlayerMsg(Index, "Invalid access level.", ColorType.BrightRed)
        End If

        Buffer.Dispose()
    End Sub

    Sub Packet_WhosOnline(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CWhosOnline", PACKET_LOG)
        TextAdd("Recieved CMSG: CWhosOnline")

        SendWhosOnline(Index)
    End Sub

    Sub Packet_SetMotd(ByVal Index As Integer, ByRef data() As Byte)
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CSetMotd", PACKET_LOG)
        TextAdd("Recieved CMSG: CSetMotd")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Mapper Then
            Exit Sub
        End If

        Options.Motd = Trim$(Buffer.ReadString)
        SaveOptions()

        GlobalMsg("MOTD changed to: " & Options.Motd)
        Addlog(GetPlayerName(Index) & " changed MOTD to: " & Options.Motd, ADMIN_LOG)

        Buffer.Dispose()
    End Sub

    Sub Packet_PlayerSearch(ByVal Index As Integer, ByRef data() As Byte)
        Dim TargetFound As Byte, rclick As Byte
        Dim x As Integer, y As Integer, i As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CSearch", PACKET_LOG)
        TextAdd("Recieved CMSG: CSearch")

        x = Buffer.ReadInt32
        y = Buffer.ReadInt32
        rclick = Buffer.ReadInt32

        ' Prevent subscript out of range
        If x < 0 Or x > Map(GetPlayerMap(Index)).MaxX Or y < 0 Or y > Map(GetPlayerMap(Index)).MaxY Then Exit Sub

        ' Check for a player
        For i = 1 To GetPlayersOnline()

            If IsPlaying(i) Then
                If GetPlayerMap(Index) = GetPlayerMap(i) Then
                    If GetPlayerX(i) = x Then
                        If GetPlayerY(i) = y Then

                            ' Consider the player
                            If i <> Index Then
                                If GetPlayerLevel(i) >= GetPlayerLevel(Index) + 5 Then
                                    PlayerMsg(Index, "You wouldn't stand a chance.", ColorType.BrightRed)
                                Else

                                    If GetPlayerLevel(i) > GetPlayerLevel(Index) Then
                                        PlayerMsg(Index, "This one seems to have an advantage over you.", ColorType.Yellow)
                                    Else

                                        If GetPlayerLevel(i) = GetPlayerLevel(Index) Then
                                            PlayerMsg(Index, "This would be an even fight.", ColorType.White)
                                        Else

                                            If GetPlayerLevel(Index) >= GetPlayerLevel(i) + 5 Then
                                                PlayerMsg(Index, "You could slaughter that player.", ColorType.BrightBlue)
                                            Else

                                                If GetPlayerLevel(Index) > GetPlayerLevel(i) Then
                                                    PlayerMsg(Index, "You would have an advantage over that player.", ColorType.BrightCyan)
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If

                            ' Change target
                            TempPlayer(Index).Target = i
                            TempPlayer(Index).TargetType = TargetType.Player
                            PlayerMsg(Index, "Your target is now " & GetPlayerName(i) & ".", ColorType.Yellow)
                            SendTarget(Index, TempPlayer(Index).Target, TempPlayer(Index).TargetType)
                            TargetFound = 1
                            If rclick = 1 Then SendRightClick(Index)
                            Exit Sub
                        End If
                    End If
                End If
            End If

        Next

        ' Check for an item
        For i = 1 To MAX_MAP_ITEMS

            If MapItem(GetPlayerMap(Index), i).Num > 0 Then
                If MapItem(GetPlayerMap(Index), i).X = x Then
                    If MapItem(GetPlayerMap(Index), i).Y = y Then
                        PlayerMsg(Index, "You see " & CheckGrammar(Trim$(Item(MapItem(GetPlayerMap(Index), i).Num).Name)) & ".", ColorType.White)
                        Exit Sub
                    End If
                End If
            End If

        Next

        ' Check for an npc
        For i = 1 To MAX_MAP_NPCS

            If MapNpc(GetPlayerMap(Index)).Npc(i).Num > 0 Then
                If MapNpc(GetPlayerMap(Index)).Npc(i).X = x Then
                    If MapNpc(GetPlayerMap(Index)).Npc(i).Y = y Then
                        ' Change target
                        TempPlayer(Index).Target = i
                        TempPlayer(Index).TargetType = TargetType.Npc
                        PlayerMsg(Index, "Your target is now " & CheckGrammar(Trim$(Npc(MapNpc(GetPlayerMap(Index)).Npc(i).Num).Name)) & ".", ColorType.Yellow)
                        SendTarget(Index, TempPlayer(Index).Target, TempPlayer(Index).TargetType)
                        TargetFound = 1
                        Exit Sub
                    End If
                End If
            End If

        Next

        'Housing
        If Player(Index).Character(TempPlayer(Index).CurChar).InHouse > 0 Then
            If Player(Index).Character(TempPlayer(Index).CurChar).InHouse = Index Then
                If Player(Index).Character(TempPlayer(Index).CurChar).House.HouseIndex > 0 Then
                    If Player(Index).Character(TempPlayer(Index).CurChar).House.FurnitureCount > 0 Then
                        For i = 1 To Player(Index).Character(TempPlayer(Index).CurChar).House.FurnitureCount
                            If x >= Player(Index).Character(TempPlayer(Index).CurChar).House.Furniture(i).X And x <= Player(Index).Character(TempPlayer(Index).CurChar).House.Furniture(i).X + Item(Player(Index).Character(TempPlayer(Index).CurChar).House.Furniture(i).ItemNum).FurnitureWidth - 1 Then
                                If y <= Player(Index).Character(TempPlayer(Index).CurChar).House.Furniture(i).Y And y >= Player(Index).Character(TempPlayer(Index).CurChar).House.Furniture(i).Y - Item(Player(Index).Character(TempPlayer(Index).CurChar).House.Furniture(i).ItemNum).FurnitureHeight + 1 Then
                                    'Found an Item, get the index and lets pick it up!
                                    x = FindOpenInvSlot(Index, Player(Index).Character(TempPlayer(Index).CurChar).House.Furniture(i).ItemNum)
                                    If x > 0 Then
                                        GiveInvItem(Index, Player(Index).Character(TempPlayer(Index).CurChar).House.Furniture(i).ItemNum, 0, True)
                                        Player(Index).Character(TempPlayer(Index).CurChar).House.FurnitureCount = Player(Index).Character(TempPlayer(Index).CurChar).House.FurnitureCount - 1
                                        For x = i + 1 To Player(Index).Character(TempPlayer(Index).CurChar).House.FurnitureCount + 1
                                            Player(Index).Character(TempPlayer(Index).CurChar).House.Furniture(x - 1) = Player(Index).Character(TempPlayer(Index).CurChar).House.Furniture(x)
                                        Next
                                        ReDim Preserve Player(Index).Character(TempPlayer(Index).CurChar).House.Furniture(Player(Index).Character(TempPlayer(Index).CurChar).House.FurnitureCount)
                                        SendFurnitureToHouse(Index)
                                        Exit Sub
                                    Else
                                        PlayerMsg(Index, "No inventory space available!", ColorType.BrightRed)
                                    End If
                                    Exit Sub
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        End If

        If TargetFound = 0 Then
            SendTarget(Index, 0, 0)
        End If

        Buffer.Dispose()
    End Sub

    Sub Packet_Skills(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CSkills", PACKET_LOG)
        TextAdd("Recieved CMSG: CSkills")

        SendPlayerSkills(Index)
    End Sub

    Sub Packet_Cast(ByVal Index As Integer, ByRef data() As Byte)
        Dim n As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CCast", PACKET_LOG)
        TextAdd("Recieved CMSG: CCast")

        ' Skill slot
        n = Buffer.ReadInt32
        Buffer.Dispose()

        ' set the skill buffer before castin
        BufferSkill(Index, n)

        Buffer.Dispose()
    End Sub

    Sub Packet_QuitGame(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CQuit", PACKET_LOG)
        TextAdd("Recieved CMSG: CQuit")

        SendLeftGame(Index)
        Socket.Disconnect(Index)
    End Sub

    Sub Packet_SwapInvSlots(ByVal Index As Integer, ByRef data() As Byte)
        Dim oldSlot As Integer, newSlot As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CSwapInvSlots", PACKET_LOG)
        TextAdd("Recieved CMSG: CSwapInvSlots")

        If TempPlayer(Index).InTrade > 0 Or TempPlayer(Index).InBank Or TempPlayer(Index).InShop Then Exit Sub

        ' Old Slot
        oldSlot = Buffer.ReadInt32
        newSlot = Buffer.ReadInt32
        Buffer.Dispose()

        PlayerSwitchInvSlots(Index, oldSlot, newSlot)

        Buffer.Dispose()
    End Sub

    Sub Packet_EditResource(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved EMSG: RequestEditResource", PACKET_LOG)
        TextAdd("Recieved EMSG: RequestEditResource")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Developer Then Exit Sub

        Dim Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SResourceEditor)
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SResourceEditor", PACKET_LOG)
        TextAdd("Sent SMSG: SResourceEditor")

        Buffer.Dispose()
    End Sub

    Sub Packet_SaveResource(ByVal Index As Integer, ByRef data() As Byte)
        Dim resourcenum As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved EMSG: SaveResource", PACKET_LOG)
        TextAdd("Recieved EMSG: SaveResource")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Developer Then Exit Sub

        resourcenum = Buffer.ReadInt32

        ' Prevent hacking
        If resourcenum < 0 Or resourcenum > MAX_RESOURCES Then Exit Sub

        Resource(resourcenum).Animation = Buffer.ReadInt32()
        Resource(resourcenum).EmptyMessage = Buffer.ReadString()
        Resource(resourcenum).ExhaustedImage = Buffer.ReadInt32()
        Resource(resourcenum).Health = Buffer.ReadInt32()
        Resource(resourcenum).ExpReward = Buffer.ReadInt32()
        Resource(resourcenum).ItemReward = Buffer.ReadInt32()
        Resource(resourcenum).Name = Buffer.ReadString()
        Resource(resourcenum).ResourceImage = Buffer.ReadInt32()
        Resource(resourcenum).ResourceType = Buffer.ReadInt32()
        Resource(resourcenum).RespawnTime = Buffer.ReadInt32()
        Resource(resourcenum).SuccessMessage = Buffer.ReadString()
        Resource(resourcenum).LvlRequired = Buffer.ReadInt32()
        Resource(resourcenum).ToolRequired = Buffer.ReadInt32()
        Resource(resourcenum).Walkthrough = Buffer.ReadInt32()

        ' Save it
        SendUpdateResourceToAll(resourcenum)
        SaveResource(resourcenum)

        Addlog(GetPlayerLogin(Index) & " saved Resource #" & resourcenum & ".", ADMIN_LOG)

        Buffer.Dispose()
    End Sub

    Sub Packet_CheckPing(ByVal Index As Integer, ByRef data() As Byte)
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SSendPing)
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

        Addlog("Sent SMSG: SSendPing", PACKET_LOG)
        TextAdd("Sent SMSG: SSendPing")

        Buffer.Dispose()
    End Sub

    Sub Packet_Unequip(ByVal Index As Integer, ByRef data() As Byte)
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CUnequip", PACKET_LOG)
        TextAdd("Recieved CMSG: CUnequip")

        PlayerUnequipItem(Index, Buffer.ReadInt32)

        Buffer.Dispose()
    End Sub

    Sub Packet_RequestPlayerData(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CRequestPlayerData", PACKET_LOG)
        TextAdd("Recieved CMSG: CRequestPlayerData")

        SendPlayerData(Index)
    End Sub

    Sub Packet_RequestItems(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CRequestItems", PACKET_LOG)
        TextAdd("Recieved CMSG: CRequestItems")

        SendItems(Index)
    End Sub

    Sub Packet_RequestNpcs(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CRequestNPCS", PACKET_LOG)
        TextAdd("Recieved CMSG: CRequestNPCS")

        SendNpcs(Index)
    End Sub

    Sub Packet_RequestResources(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CRequestResources", PACKET_LOG)
        TextAdd("Recieved CMSG: CRequestResources")

        SendResources(Index)
    End Sub

    Sub Packet_SpawnItem(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CSpawnItem", PACKET_LOG)
        TextAdd("Recieved CMSG: CSpawnItem")

        Dim tmpItem As Integer
        Dim tmpAmount As Integer
        Dim Buffer As New ByteStream(data)

        ' item
        tmpItem = Buffer.ReadInt32
        tmpAmount = Buffer.ReadInt32

        If GetPlayerAccess(Index) < AdminType.Creator Then Exit Sub

        SpawnItem(tmpItem, tmpAmount, GetPlayerMap(Index), GetPlayerX(Index), GetPlayerY(Index))
        Buffer.Dispose()
    End Sub

    Sub Packet_TrainStat(ByVal Index As Integer, ByRef data() As Byte)
        Dim tmpstat As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CTrainStat", PACKET_LOG)
        TextAdd("Recieved CMSG: CTrainStat")

        ' check points
        If GetPlayerPOINTS(Index) = 0 Then Exit Sub

        ' stat
        tmpstat = Buffer.ReadInt32

        ' increment stat
        SetPlayerStat(Index, tmpstat, GetPlayerRawStat(Index, tmpstat) + 1)

        ' decrement points
        SetPlayerPOINTS(Index, GetPlayerPOINTS(Index) - 1)

        ' send player new data
        SendPlayerData(Index)
        Buffer.Dispose()
    End Sub

    Sub Packet_EditAnimation(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved EMSG: RequestEditAnimation", PACKET_LOG)
        TextAdd("Recieved EMSG: RequestEditAnimation")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Developer Then
            Exit Sub
        End If

        Dim Buffer = New ByteStream(4)
        Buffer.WriteInt32(ServerPackets.SAnimationEditor)
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        Buffer.Dispose()
    End Sub

    Sub Packet_SaveAnimation(ByVal Index As Integer, ByRef data() As Byte)
        Dim AnimNum As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved EMSG: SaveAnimation", PACKET_LOG)
        TextAdd("Recieved EMSG: SaveAnimation")

        AnimNum = Buffer.ReadInt32

        ' Update the Animation
        For i = 0 To UBound(Animation(AnimNum).Frames)
            Animation(AnimNum).Frames(i) = Buffer.ReadInt32()
        Next

        For i = 0 To UBound(Animation(AnimNum).LoopCount)
            Animation(AnimNum).LoopCount(i) = Buffer.ReadInt32()
        Next

        For i = 0 To UBound(Animation(AnimNum).LoopTime)
            Animation(AnimNum).LoopTime(i) = Buffer.ReadInt32()
        Next

        Animation(AnimNum).Name = Buffer.ReadString()
        Animation(AnimNum).Sound = Buffer.ReadString()

        If Animation(AnimNum).Name Is Nothing Then Animation(AnimNum).Name = ""
        If Animation(AnimNum).Sound Is Nothing Then Animation(AnimNum).Sound = ""

        For i = 0 To UBound(Animation(AnimNum).Sprite)
            Animation(AnimNum).Sprite(i) = Buffer.ReadInt32()
        Next

        Buffer.Dispose()

        ' Save it
        SaveAnimation(AnimNum)
        SendUpdateAnimationToAll(AnimNum)
        Addlog(GetPlayerLogin(Index) & " saved Animation #" & AnimNum & ".", ADMIN_LOG)

    End Sub

    Sub Packet_RequestAnimations(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CRequestAnimations", PACKET_LOG)
        TextAdd("Recieved CMSG: CRequestAnimations")

        SendAnimations(Index)
    End Sub

    Sub Packet_RequestSkills(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CRequestSkills", PACKET_LOG)
        TextAdd("Recieved CMSG: CRequestSkills")

        SendSkills(Index)
    End Sub

    Sub Packet_RequestShops(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CRequestShops", PACKET_LOG)
        TextAdd("Recieved CMSG: CRequestShops")

        SendShops(Index)
    End Sub

    Sub Packet_RequestLevelUp(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CRequestLevelUp", PACKET_LOG)
        TextAdd("Recieved CMSG: CRequestLevelUp")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Creator Then
            Exit Sub
        End If

        SetPlayerExp(Index, GetPlayerNextLevel(Index))
        CheckPlayerLevelUp(Index)
    End Sub

    Sub Packet_ForgetSkill(ByVal Index As Integer, ByRef data() As Byte)
        Dim skillslot As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CForgetSkill", PACKET_LOG)
        TextAdd("Recieved CMSG: CForgetSkill")

        skillslot = Buffer.ReadInt32

        ' Check for subscript out of range
        If skillslot < 1 Or skillslot > MAX_PLAYER_SKILLS Then
            Exit Sub
        End If

        ' dont let them forget a skill which is in CD
        If TempPlayer(Index).SkillCD(skillslot) > 0 Then
            PlayerMsg(Index, "Cannot forget a skill which is cooling down!", ColorType.BrightRed)
            Exit Sub
        End If

        ' dont let them forget a skill which is buffered
        If TempPlayer(Index).SkillBuffer = skillslot Then
            PlayerMsg(Index, "Cannot forget a skill which you are casting!", ColorType.BrightRed)
            Exit Sub
        End If

        Player(Index).Character(TempPlayer(Index).CurChar).Skill(skillslot) = 0
        SendPlayerSkills(Index)

        Buffer.Dispose()
    End Sub

    Sub Packet_CloseShop(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CCloseShop", PACKET_LOG)
        TextAdd("Recieved CMSG: CCloseShop")

        TempPlayer(Index).InShop = 0
    End Sub

    Sub Packet_BuyItem(ByVal Index As Integer, ByRef data() As Byte)
        Dim shopslot As Integer, shopnum As Integer, itemamount As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CBuyItem", PACKET_LOG)
        TextAdd("Recieved CMSG: CBuyItem")

        shopslot = Buffer.ReadInt32

        ' not in shop, exit out
        shopnum = TempPlayer(Index).InShop
        If shopnum < 1 Or shopnum > MAX_SHOPS Then Exit Sub

        With Shop(shopnum).TradeItem(shopslot)
            ' check trade exists
            If .Item < 1 Then Exit Sub

            ' check has the cost item
            itemamount = HasItem(Index, .CostItem)
            If itemamount = 0 Or itemamount < .CostValue Then
                PlayerMsg(Index, "You do not have enough to buy this item.", ColorType.BrightRed)
                ResetShopAction(Index)
                Exit Sub
            End If

            ' it's fine, let's go ahead
            TakeInvItem(Index, .CostItem, .CostValue)
            GiveInvItem(Index, .Item, .ItemValue)
        End With

        ' send confirmation message & reset their shop action
        PlayerMsg(Index, "Trade successful.", ColorType.BrightGreen)
        ResetShopAction(Index)

        Buffer.Dispose()
    End Sub

    Sub Packet_SellItem(ByVal Index As Integer, ByRef data() As Byte)
        Dim invSlot As Integer
        Dim itemNum As Integer
        Dim price As Integer
        Dim multiplier As Double
        Dim Buffer As New ByteStream(data)

        Addlog("Recieved CMSG: CSellItem", PACKET_LOG)
        TextAdd("Recieved CMSG: CSellItem")

        invSlot = Buffer.ReadInt32

        ' if invalid, exit out
        If invSlot < 1 Or invSlot > MAX_INV Then Exit Sub

        ' has item?
        If GetPlayerInvItemNum(Index, invSlot) < 1 Or GetPlayerInvItemNum(Index, invSlot) > MAX_ITEMS Then Exit Sub

        ' seems to be valid
        itemNum = GetPlayerInvItemNum(Index, invSlot)

        ' work out price
        multiplier = Shop(TempPlayer(Index).InShop).BuyRate / 100
        price = Item(itemNum).Price * multiplier

        ' item has cost?
        If price <= 0 Then
            PlayerMsg(Index, "The shop doesn't want that item.", ColorType.Yellow)
            ResetShopAction(Index)
            Exit Sub
        End If

        ' take item and give gold
        TakeInvItem(Index, itemNum, 1)
        GiveInvItem(Index, 1, price)

        ' send confirmation message & reset their shop action
        PlayerMsg(Index, "Sold the " & Trim(Item(GetPlayerInvItemNum(Index, invSlot)).Name) & " !", ColorType.BrightGreen)
        ResetShopAction(Index)

        Buffer.Dispose()
    End Sub

    Sub Packet_ChangeBankSlots(ByVal Index As Integer, ByRef data() As Byte)
        Dim oldslot As Integer, newslot As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CChangeBankSlots", PACKET_LOG)
        TextAdd("Recieved CMSG: CChangeBankSlots")

        oldslot = Buffer.ReadInt32
        newslot = Buffer.ReadInt32

        PlayerSwitchBankSlots(Index, oldslot, newslot)

        Buffer.Dispose()
    End Sub

    Sub Packet_DepositItem(ByVal Index As Integer, ByRef data() As Byte)
        Dim invslot As Integer, amount As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CDepositItem", PACKET_LOG)
        TextAdd("Recieved CMSG: CDepositItem")

        invslot = Buffer.ReadInt32
        amount = Buffer.ReadInt32

        GiveBankItem(Index, invslot, amount)

        Buffer.Dispose()
    End Sub

    Sub Packet_WithdrawItem(ByVal Index As Integer, ByRef data() As Byte)
        Dim bankslot As Integer, amount As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CWithdrawItem", PACKET_LOG)
        TextAdd("Recieved CMSG: CWithdrawItem")

        bankslot = Buffer.ReadInt32
        amount = Buffer.ReadInt32

        TakeBankItem(Index, bankslot, amount)

        Buffer.Dispose()
    End Sub

    Sub Packet_CloseBank(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CCloseBank", PACKET_LOG)
        TextAdd("Recieved CMSG: CCloseBank")

        SaveBank(Index)
        SavePlayer(Index)

        TempPlayer(Index).InBank = False
    End Sub

    Sub Packet_AdminWarp(ByVal Index As Integer, ByRef data() As Byte)
        Dim x As Integer, y As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CAdminWarp", PACKET_LOG)
        TextAdd("Recieved CMSG: CAdminWarp")

        x = Buffer.ReadInt32
        y = Buffer.ReadInt32

        If GetPlayerAccess(Index) >= AdminType.Mapper Then
            'Set the  Information
            SetPlayerX(Index, x)
            SetPlayerY(Index, y)

            'send the stuff
            SendPlayerXY(Index)
        End If

        Buffer.Dispose()
    End Sub

    Sub Packet_TradeInvite(ByVal Index As Integer, ByRef data() As Byte)
        Dim Name As String, tradetarget As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CTradeInvite", PACKET_LOG)
        TextAdd("Recieved CMSG: CTradeInvite")

        Name = Buffer.ReadString

        Buffer.Dispose()

        ' Check for a player

        tradetarget = FindPlayer(Name)

        ' make sure we don't error
        If tradetarget <= 0 Or tradetarget > MAX_PLAYERS Then Exit Sub

        ' can't trade with yourself..
        If tradetarget = Index Then
            PlayerMsg(Index, "You can't trade with yourself.", ColorType.BrightRed)
            Exit Sub
        End If

        ' send the trade request
        TempPlayer(Index).TradeRequest = tradetarget
        TempPlayer(tradetarget).TradeRequest = Index

        PlayerMsg(tradetarget, Trim$(GetPlayerName(Index)) & " has invited you to trade.", ColorType.Yellow)
        PlayerMsg(Index, "You have invited " & Trim$(GetPlayerName(tradetarget)) & " to trade.", ColorType.BrightGreen)
        SendClearTradeTimer(Index)

        SendTradeInvite(tradetarget, Index)
    End Sub

    Sub Packet_TradeInviteAccept(ByVal Index As Integer, ByRef data() As Byte)
        Dim tradetarget As Integer, status As Byte
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CTradeInviteAccept", PACKET_LOG)
        TextAdd("Recieved CMSG: CTradeInviteAccept")

        status = Buffer.ReadInt32

        Buffer.Dispose()

        If status = 0 Then Exit Sub

        tradetarget = TempPlayer(Index).TradeRequest

        ' Let them trade!
        If TempPlayer(tradetarget).TradeRequest = Index Then
            ' let them know they're trading
            PlayerMsg(Index, "You have accepted " & Trim$(GetPlayerName(tradetarget)) & "'s trade request.", ColorType.Yellow)
            PlayerMsg(tradetarget, Trim$(GetPlayerName(Index)) & " has accepted your trade request.", ColorType.BrightGreen)
            ' clear the trade timeout clientside
            SendClearTradeTimer(Index)

            ' clear the tradeRequest server-side
            TempPlayer(Index).TradeRequest = 0
            TempPlayer(tradetarget).TradeRequest = 0

            ' set that they're trading with each other
            TempPlayer(Index).InTrade = tradetarget
            TempPlayer(tradetarget).InTrade = Index

            ' clear out their trade offers
            ReDim TempPlayer(Index).TradeOffer(MAX_INV)
            ReDim TempPlayer(tradetarget).TradeOffer(MAX_INV)
            For i = 1 To MAX_INV
                TempPlayer(Index).TradeOffer(i).Num = 0
                TempPlayer(Index).TradeOffer(i).Value = 0
                TempPlayer(tradetarget).TradeOffer(i).Num = 0
                TempPlayer(tradetarget).TradeOffer(i).Value = 0
            Next
            ' Used to init the trade window clientside
            SendTrade(Index, tradetarget)
            SendTrade(tradetarget, Index)

            ' Send the offer data - Used to clear their client
            SendTradeUpdate(Index, 0)
            SendTradeUpdate(Index, 1)
            SendTradeUpdate(tradetarget, 0)
            SendTradeUpdate(tradetarget, 1)
            Exit Sub
        End If
    End Sub

    Sub Packet_AcceptTrade(ByVal Index As Integer, ByRef data() As Byte)
        Dim itemNum As Integer
        Dim tradeTarget As Integer, i As Integer
        Dim tmpTradeItem(0 To MAX_INV) As PlayerInvRec
        Dim tmpTradeItem2(0 To MAX_INV) As PlayerInvRec
        Addlog("Recieved CMSG: CAcceptTrade", PACKET_LOG)
        TextAdd("Recieved CMSG: CAcceptTrade")

        TempPlayer(Index).AcceptTrade = True

        tradeTarget = TempPlayer(Index).InTrade

        ' if not both of them accept, then exit
        If Not TempPlayer(tradeTarget).AcceptTrade Then
            SendTradeStatus(Index, 2)
            SendTradeStatus(tradeTarget, 1)
            Exit Sub
        End If

        ' take their items
        For i = 1 To MAX_INV
            ' player
            If TempPlayer(Index).TradeOffer(i).Num > 0 Then
                itemNum = Player(Index).Character(TempPlayer(Index).CurChar).Inv(TempPlayer(Index).TradeOffer(i).Num).Num
                If itemNum > 0 Then
                    ' store temp
                    tmpTradeItem(i).Num = itemNum
                    tmpTradeItem(i).Value = TempPlayer(Index).TradeOffer(i).Value
                    ' take item
                    TakeInvSlot(Index, TempPlayer(Index).TradeOffer(i).Num, tmpTradeItem(i).Value)
                End If
            End If
            ' target
            If TempPlayer(tradeTarget).TradeOffer(i).Num > 0 Then
                itemNum = GetPlayerInvItemNum(tradeTarget, TempPlayer(tradeTarget).TradeOffer(i).Num)
                If itemNum > 0 Then
                    ' store temp
                    tmpTradeItem2(i).Num = itemNum
                    tmpTradeItem2(i).Value = TempPlayer(tradeTarget).TradeOffer(i).Value
                    ' take item
                    TakeInvSlot(tradeTarget, TempPlayer(tradeTarget).TradeOffer(i).Num, tmpTradeItem2(i).Value)
                End If
            End If
        Next

        ' taken all items. now they can't not get items because of no inventory space.
        For i = 1 To MAX_INV
            ' player
            If tmpTradeItem2(i).Num > 0 Then
                ' give away!
                GiveInvItem(Index, tmpTradeItem2(i).Num, tmpTradeItem2(i).Value, False)
            End If
            ' target
            If tmpTradeItem(i).Num > 0 Then
                ' give away!
                GiveInvItem(tradeTarget, tmpTradeItem(i).Num, tmpTradeItem(i).Value, False)
            End If
        Next

        SendInventory(Index)
        SendInventory(tradeTarget)

        ' they now have all the items. Clear out values + let them out of the trade.
        For i = 1 To MAX_INV
            TempPlayer(Index).TradeOffer(i).Num = 0
            TempPlayer(Index).TradeOffer(i).Value = 0
            TempPlayer(tradeTarget).TradeOffer(i).Num = 0
            TempPlayer(tradeTarget).TradeOffer(i).Value = 0
        Next

        TempPlayer(Index).InTrade = 0
        TempPlayer(tradeTarget).InTrade = 0

        PlayerMsg(Index, "Trade completed.", ColorType.BrightGreen)
        PlayerMsg(tradeTarget, "Trade completed.", ColorType.BrightGreen)

        SendCloseTrade(Index)
        SendCloseTrade(tradeTarget)
    End Sub

    Sub Packet_DeclineTrade(ByVal Index As Integer, ByRef data() As Byte)
        Dim tradeTarget As Integer
        Addlog("Recieved CMSG: CDeclineTrade", PACKET_LOG)
        TextAdd("Recieved CMSG: CDeclineTrade")

        tradeTarget = TempPlayer(Index).InTrade

        For i = 1 To MAX_INV
            TempPlayer(Index).TradeOffer(i).Num = 0
            TempPlayer(Index).TradeOffer(i).Value = 0
            TempPlayer(tradeTarget).TradeOffer(i).Num = 0
            TempPlayer(tradeTarget).TradeOffer(i).Value = 0
        Next

        TempPlayer(Index).InTrade = 0
        TempPlayer(tradeTarget).InTrade = 0

        PlayerMsg(Index, "You declined the trade.", ColorType.Yellow)
        PlayerMsg(tradeTarget, GetPlayerName(Index) & " has declined the trade.", ColorType.BrightRed)

        SendCloseTrade(Index)
        SendCloseTrade(tradeTarget)
    End Sub

    Sub Packet_TradeItem(ByVal Index As Integer, ByRef data() As Byte)
        Dim itemnum As Integer
        Dim invslot As Integer, amount As Integer, emptyslot As Integer, i As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CTradeItem", PACKET_LOG)
        TextAdd("Recieved CMSG: CTradeItem")

        invslot = Buffer.ReadInt32
        amount = Buffer.ReadInt32

        Buffer.Dispose()

        If invslot <= 0 Or invslot > MAX_INV Then Exit Sub

        itemnum = GetPlayerInvItemNum(Index, invslot)

        If itemnum <= 0 Or itemnum > MAX_ITEMS Then Exit Sub

        ' make sure they have the amount they offer
        If amount < 0 Or amount > GetPlayerInvItemValue(Index, invslot) Then Exit Sub

        If Item(itemnum).Type = ItemType.Currency Or Item(itemnum).Stackable = 1 Then

            ' check if already offering same currency item
            For i = 1 To MAX_INV

                If TempPlayer(Index).TradeOffer(i).Num = invslot Then
                    ' add amount
                    TempPlayer(Index).TradeOffer(i).Value = TempPlayer(Index).TradeOffer(i).Value + amount

                    ' clamp to limits
                    If TempPlayer(Index).TradeOffer(i).Value > GetPlayerInvItemValue(Index, invslot) Then
                        TempPlayer(Index).TradeOffer(i).Value = GetPlayerInvItemValue(Index, invslot)
                    End If

                    ' cancel any trade agreement
                    TempPlayer(Index).AcceptTrade = False
                    TempPlayer(TempPlayer(Index).InTrade).AcceptTrade = False

                    SendTradeStatus(Index, 0)
                    SendTradeStatus(TempPlayer(Index).InTrade, 0)

                    SendTradeUpdate(Index, 0)
                    SendTradeUpdate(TempPlayer(Index).InTrade, 1)
                    ' exit early
                    Exit Sub
                End If
            Next
        Else
            ' make sure they're not already offering it
            For i = 1 To MAX_INV
                If TempPlayer(Index).TradeOffer(i).Num = invslot Then
                    PlayerMsg(Index, "You've already offered this item.", ColorType.BrightRed)
                    Exit Sub
                End If
            Next
        End If

        ' not already offering - find earliest empty slot
        For i = 1 To MAX_INV
            If TempPlayer(Index).TradeOffer(i).Num = 0 Then
                emptyslot = i
                Exit For
            End If
        Next
        TempPlayer(Index).TradeOffer(emptyslot).Num = invslot
        TempPlayer(Index).TradeOffer(emptyslot).Value = amount

        ' cancel any trade agreement and send new data
        TempPlayer(Index).AcceptTrade = False
        TempPlayer(TempPlayer(Index).InTrade).AcceptTrade = False

        SendTradeStatus(Index, 0)
        SendTradeStatus(TempPlayer(Index).InTrade, 0)

        SendTradeUpdate(Index, 0)
        SendTradeUpdate(TempPlayer(Index).InTrade, 1)
    End Sub

    Sub Packet_UntradeItem(ByVal Index As Integer, ByRef data() As Byte)
        Dim tradeslot As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CUntradeItem", PACKET_LOG)
        TextAdd("Recieved CMSG: CUntradeItem")

        tradeslot = Buffer.ReadInt32

        Buffer.Dispose()

        If tradeslot <= 0 Or tradeslot > MAX_INV Then Exit Sub
        If TempPlayer(Index).TradeOffer(tradeslot).Num <= 0 Then Exit Sub

        TempPlayer(Index).TradeOffer(tradeslot).Num = 0
        TempPlayer(Index).TradeOffer(tradeslot).Value = 0

        If TempPlayer(Index).AcceptTrade Then TempPlayer(Index).AcceptTrade = False
        If TempPlayer(TempPlayer(Index).InTrade).AcceptTrade Then TempPlayer(TempPlayer(Index).InTrade).AcceptTrade = False

        SendTradeStatus(Index, 0)
        SendTradeStatus(TempPlayer(Index).InTrade, 0)

        SendTradeUpdate(Index, 0)
        SendTradeUpdate(TempPlayer(Index).InTrade, 1)
    End Sub

    Sub HackingAttempt(ByVal Index As Integer, ByVal Reason As String)

        If Index > 0 AndAlso IsPlaying(Index) Then
            GlobalMsg(GetPlayerLogin(Index) & "/" & GetPlayerName(Index) & " has been booted for (" & Reason & ")")

            AlertMsg(Index, "You have lost your connection with " & Options.GameName & ".")
        End If

    End Sub

    'Mapreport
    Sub Packet_MapReport(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CMapReport", PACKET_LOG)
        TextAdd("Recieved CMSG: CMapReport")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Mapper Then Exit Sub

        SendMapReport(Index)
    End Sub

    Sub Packet_Admin(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CAdmin", PACKET_LOG)
        TextAdd("Recieved CMSG: CAdmin")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Mapper Then Exit Sub

        SendAdminPanel(Index)
    End Sub

    Sub Packet_SetHotBarSlot(ByVal Index As Integer, ByRef data() As Byte)
        Dim slot As Integer, skill As Integer, type As Byte
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CSetHotbarSlot", PACKET_LOG)
        TextAdd("Recieved CMSG: CSetHotbarSlot")

        slot = Buffer.ReadInt32
        skill = Buffer.ReadInt32
        type = Buffer.ReadInt32

        Player(Index).Character(TempPlayer(Index).CurChar).Hotbar(slot).Slot = skill
        Player(Index).Character(TempPlayer(Index).CurChar).Hotbar(slot).SlotType = type

        SendHotbar(Index)

        Buffer.Dispose()
    End Sub

    Sub Packet_DeleteHotBarSlot(ByVal Index As Integer, ByRef data() As Byte)
        Dim slot As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CDeleteHotbarSlot", PACKET_LOG)
        TextAdd("Recieved CMSG: CDeleteHotbarSlot")

        slot = Buffer.ReadInt32

        Player(Index).Character(TempPlayer(Index).CurChar).Hotbar(slot).Slot = 0
        Player(Index).Character(TempPlayer(Index).CurChar).Hotbar(slot).SlotType = 0

        SendHotbar(Index)

        Buffer.Dispose()
    End Sub

    Sub Packet_UseHotBarSlot(ByVal Index As Integer, ByRef data() As Byte)
        Dim slot As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CUseHotbarSlot", PACKET_LOG)
        TextAdd("Recieved CMSG: CUseHotbarSlot")

        slot = Buffer.ReadInt32
        Buffer.Dispose()

        If Player(Index).Character(TempPlayer(Index).CurChar).Hotbar(slot).Slot > 0 Then
            If Player(Index).Character(TempPlayer(Index).CurChar).Hotbar(slot).SlotType = 1 Then 'skill
                BufferSkill(Index, Player(Index).Character(TempPlayer(Index).CurChar).Hotbar(slot).Slot)
            ElseIf Player(Index).Character(TempPlayer(Index).CurChar).Hotbar(slot).SlotType = 2 Then 'item
                UseItem(Index, Player(Index).Character(TempPlayer(Index).CurChar).Hotbar(slot).Slot)
            End If
        End If

        SendHotbar(Index)

    End Sub

    Sub Packet_RequestClasses(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CRequestClasses", PACKET_LOG)
        TextAdd("Recieved CMSG: CRequestClasses")

        SendClasses(Index)
    End Sub

    Sub Packet_RequestEditClasses(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved EMSG: RequestEditClasses", PACKET_LOG)
        TextAdd("Recieved EMSG: RequestEditClasses")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Developer Then Exit Sub

        SendClasses(Index)

        SendClassEditor(Index)
    End Sub

    Sub Packet_SaveClasses(ByVal Index As Integer, ByRef data() As Byte)
        Dim i As Integer, z As Integer, x As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved EMSG: SaveClasses", PACKET_LOG)
        TextAdd("Recieved EMSG: SaveClasses")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Developer Then Exit Sub

        ' Max classes
        Max_Classes = Buffer.ReadInt32
        ReDim Classes(0 To Max_Classes)

        For i = 0 To Max_Classes
            ReDim Classes(i).Stat(0 To Stats.Count - 1)
        Next

        For i = 1 To Max_Classes

            With Classes(i)
                .Name = Buffer.ReadString
                .Desc = Buffer.ReadString

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

        Buffer.Dispose()

        SaveClasses()

        LoadClasses()

        SendClassesToAll()
    End Sub

    Private Sub Packet_EditorLogin(ByVal Index As Integer, ByRef data() As Byte)
        Dim Name As String, Password As String, Version As String
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved EMSG: EditorLogin", PACKET_LOG)
        TextAdd("Recieved EMSG: EditorLogin")

        If Not IsLoggedIn(Index) Then

            ' Get the data
            Name = EKeyPair.DecryptString(Buffer.ReadString)
            Password = EKeyPair.DecryptString(Buffer.ReadString)
            Version = EKeyPair.DecryptString(Buffer.ReadString)

            ' Check versions
            If Version <> Application.ProductVersion Then
                AlertMsg(Index, "Version outdated, please visit " & Options.Website)
                Exit Sub
            End If

            If isShuttingDown Then
                AlertMsg(Index, "Server is either rebooting or being shutdown.")
                Exit Sub
            End If

            If Len(Trim$(Name)) < 3 Or Len(Trim$(Password)) < 3 Then
                AlertMsg(Index, "Your name and password must be at least three characters in length")
                Exit Sub
            End If

            If Not AccountExist(Name) Then
                AlertMsg(Index, "That account name does not exist.")
                Exit Sub
            End If

            If Not PasswordOK(Name, Password) Then
                AlertMsg(Index, "Incorrect password.")
                Exit Sub
            End If

            If IsMultiAccounts(Name) Then
                AlertMsg(Index, "Multiple account logins is not authorized.")
                Exit Sub
            End If

            ' Load the player
            LoadPlayer(Index, Name)

            If GetPlayerAccess(Index) > AdminType.Player Then
                SendEditorLoadOk(Index)
                'SendMapData(index, 1, True)
                SendGameData(Index)
                SendMapNames(Index)
                SendProjectiles(Index)
                SendQuests(Index)
                SendRecipes(Index)
                SendHouseConfigs(Index)
                SendPets(Index)
            Else
                AlertMsg(Index, "not authorized.")
                Exit Sub
            End If

            ' Show the player up on the socket status
            Addlog(GetPlayerLogin(Index) & " has logged in from " & Socket.ClientIp(Index) & ".", PLAYER_LOG)

            TextAdd(GetPlayerLogin(Index) & " has logged in from " & Socket.ClientIp(Index) & ".")

        End If

        Buffer.Dispose()
    End Sub

    Private Sub Packet_EditorRequestMap(ByVal Index As Integer, ByRef data() As Byte)
        Dim MapNum As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved EMSG: EditorRequestMap", PACKET_LOG)
        TextAdd("Recieved EMSG: EditorRequestMap")

        MapNum = Buffer.ReadInt32

        Buffer.Dispose()

        If GetPlayerAccess(Index) > AdminType.Player Then
            SendMapData(Index, MapNum, True)
            SendMapNames(Index)

            Buffer = New ByteStream(4)
            Buffer.WriteInt32(ServerPackets.SEditMap)
            Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)

            Addlog("Sent SMSG: SEditMap", PACKET_LOG)
            TextAdd("Sent SMSG: SEditMap")

            Buffer.Dispose()
        Else
            AlertMsg(Index, "Not Allowed!")
        End If

    End Sub

    Sub Packet_EditorMapData(ByVal Index As Integer, ByRef data() As Byte)
        Dim i As Integer
        Dim MapNum As Integer
        Dim x As Integer
        Dim y As Integer

        Addlog("Recieved EMSG: EditorSaveMap", PACKET_LOG)
        TextAdd("Recieved EMSG: EditorSaveMap")

        ' Prevent hacking
        If GetPlayerAccess(Index) < AdminType.Mapper Then Exit Sub

        Dim Buffer As New ByteStream(Compression.DecompressBytes(data))


        Gettingmap = True

        MapNum = Buffer.ReadInt32

        i = Map(MapNum).Revision + 1
        ClearMap(MapNum)

        Map(MapNum).Name = Buffer.ReadString
        Map(MapNum).Music = Buffer.ReadString
        Map(MapNum).Revision = i
        Map(MapNum).Moral = Buffer.ReadInt32
        Map(MapNum).Tileset = Buffer.ReadInt32
        Map(MapNum).Up = Buffer.ReadInt32
        Map(MapNum).Down = Buffer.ReadInt32
        Map(MapNum).Left = Buffer.ReadInt32
        Map(MapNum).Right = Buffer.ReadInt32
        Map(MapNum).BootMap = Buffer.ReadInt32
        Map(MapNum).BootX = Buffer.ReadInt32
        Map(MapNum).BootY = Buffer.ReadInt32
        Map(MapNum).MaxX = Buffer.ReadInt32
        Map(MapNum).MaxY = Buffer.ReadInt32
        Map(MapNum).WeatherType = Buffer.ReadInt32
        Map(MapNum).FogIndex = Buffer.ReadInt32
        Map(MapNum).WeatherIntensity = Buffer.ReadInt32
        Map(MapNum).FogAlpha = Buffer.ReadInt32
        Map(MapNum).FogSpeed = Buffer.ReadInt32
        Map(MapNum).HasMapTint = Buffer.ReadInt32
        Map(MapNum).MapTintR = Buffer.ReadInt32
        Map(MapNum).MapTintG = Buffer.ReadInt32
        Map(MapNum).MapTintB = Buffer.ReadInt32
        Map(MapNum).MapTintA = Buffer.ReadInt32

        Map(MapNum).Instanced = Buffer.ReadInt32
        Map(MapNum).Panorama = Buffer.ReadInt32
        Map(MapNum).Parallax = Buffer.ReadInt32

        ReDim Map(MapNum).Tile(0 To Map(MapNum).MaxX, 0 To Map(MapNum).MaxY)

        For x = 1 To MAX_MAP_NPCS
            ClearMapNpc(x, MapNum)
            Map(MapNum).Npc(x) = Buffer.ReadInt32
        Next

        With Map(MapNum)
            For x = 0 To .MaxX
                For y = 0 To .MaxY
                    .Tile(x, y).Data1 = Buffer.ReadInt32
                    .Tile(x, y).Data2 = Buffer.ReadInt32
                    .Tile(x, y).Data3 = Buffer.ReadInt32
                    .Tile(x, y).DirBlock = Buffer.ReadInt32
                    ReDim .Tile(x, y).Layer(0 To MapLayer.Count - 1)
                    For i = 0 To MapLayer.Count - 1
                        .Tile(x, y).Layer(i).Tileset = Buffer.ReadInt32
                        .Tile(x, y).Layer(i).X = Buffer.ReadInt32
                        .Tile(x, y).Layer(i).Y = Buffer.ReadInt32
                        .Tile(x, y).Layer(i).AutoTile = Buffer.ReadInt32
                    Next
                    .Tile(x, y).Type = Buffer.ReadInt32
                Next
            Next

        End With

        'Event Data!
        Map(MapNum).EventCount = Buffer.ReadInt32

        If Map(MapNum).EventCount > 0 Then
            ReDim Map(MapNum).Events(0 To Map(MapNum).EventCount)
            For i = 1 To Map(MapNum).EventCount
                With Map(MapNum).Events(i)
                    .Name = Buffer.ReadString
                    .Globals = Buffer.ReadInt32
                    .X = Buffer.ReadInt32
                    .Y = Buffer.ReadInt32
                    .PageCount = Buffer.ReadInt32
                End With
                If Map(MapNum).Events(i).PageCount > 0 Then
                    ReDim Map(MapNum).Events(i).Pages(0 To Map(MapNum).Events(i).PageCount)
                    ReDim TempPlayer(i).EventMap.EventPages(0 To Map(MapNum).Events(i).PageCount)
                    For x = 1 To Map(MapNum).Events(i).PageCount
                        With Map(MapNum).Events(i).Pages(x)
                            .chkVariable = Buffer.ReadInt32
                            .VariableIndex = Buffer.ReadInt32
                            .VariableCondition = Buffer.ReadInt32
                            .VariableCompare = Buffer.ReadInt32

                            Map(MapNum).Events(i).Pages(x).chkSwitch = Buffer.ReadInt32
                            Map(MapNum).Events(i).Pages(x).SwitchIndex = Buffer.ReadInt32
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
                                ReDim Map(MapNum).Events(i).Pages(x).MoveRoute(.MoveRouteCount)
                                For y = 1 To .MoveRouteCount
                                    .MoveRoute(y).Index = Buffer.ReadInt32
                                    .MoveRoute(y).Data1 = Buffer.ReadInt32
                                    .MoveRoute(y).Data2 = Buffer.ReadInt32
                                    .MoveRoute(y).Data3 = Buffer.ReadInt32
                                    .MoveRoute(y).Data4 = Buffer.ReadInt32
                                    .MoveRoute(y).Data5 = Buffer.ReadInt32
                                    .MoveRoute(y).Data6 = Buffer.ReadInt32
                                Next
                            End If

                            .WalkAnim = Buffer.ReadInt32
                            .DirFix = Buffer.ReadInt32
                            .WalkThrough = Buffer.ReadInt32
                            .ShowName = Buffer.ReadInt32
                            .Trigger = Buffer.ReadInt32
                            .CommandListCount = Buffer.ReadInt32

                            .Position = Buffer.ReadInt32
                            .QuestNum = Buffer.ReadInt32

                            .chkPlayerGender = Buffer.ReadInt32
                        End With

                        If Map(MapNum).Events(i).Pages(x).CommandListCount > 0 Then
                            ReDim Map(MapNum).Events(i).Pages(x).CommandList(0 To Map(MapNum).Events(i).Pages(x).CommandListCount)
                            For y = 1 To Map(MapNum).Events(i).Pages(x).CommandListCount
                                Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount = Buffer.ReadInt32
                                Map(MapNum).Events(i).Pages(x).CommandList(y).ParentList = Buffer.ReadInt32
                                If Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount > 0 Then
                                    ReDim Map(MapNum).Events(i).Pages(x).CommandList(y).Commands(0 To Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount)
                                    For z = 1 To Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount
                                        With Map(MapNum).Events(i).Pages(x).CommandList(y).Commands(z)
                                            .Index = Buffer.ReadInt32
                                            .Text1 = Buffer.ReadString
                                            .Text2 = Buffer.ReadString
                                            .Text3 = Buffer.ReadString
                                            .Text4 = Buffer.ReadString
                                            .Text5 = Buffer.ReadString
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
                                            Dim tmpcount As Integer = .MoveRouteCount
                                            If tmpcount > 0 Then
                                                ReDim Preserve .MoveRoute(tmpcount)
                                                For w = 1 To tmpcount
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

        ' Save the map
        SaveMap(MapNum)

        Gettingmap = False

        SendMapNpcsToMap(MapNum)
        SpawnMapNpcs(MapNum)
        SpawnGlobalEvents(MapNum)

        For i = 1 To GetPlayersOnline()
            If IsPlaying(i) Then
                If Player(i).Character(TempPlayer(i).CurChar).Map = MapNum Then
                    SpawnMapEventsFor(i, MapNum)
                End If
            End If
        Next

        ' Clear out it all
        For i = 1 To MAX_MAP_ITEMS
            SpawnItemSlot(i, 0, 0, GetPlayerMap(Index), MapItem(GetPlayerMap(Index), i).X, MapItem(GetPlayerMap(Index), i).Y)
            ClearMapItem(i, GetPlayerMap(Index))
        Next

        ' Respawn
        SpawnMapItems(MapNum)

        ClearTempTile(MapNum)
        CacheResources(MapNum)

        ' Refresh map for everyone online
        For i = 1 To GetPlayersOnline()
            If IsPlaying(i) And GetPlayerMap(i) = MapNum Then
                PlayerWarp(i, MapNum, GetPlayerX(i), GetPlayerY(i))
                ' Send map
                SendMapData(i, MapNum, True)
            End If
        Next

        Buffer.Dispose()
    End Sub

    Private Sub Packet_RequestAutoMap(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved EMSG: RequestAutoMap", PACKET_LOG)
        TextAdd("Recieved EMSG: RequestAutoMap")

        If GetPlayerAccess(Index) = AdminType.Player Then Exit Sub

        SendAutoMapper(Index)
    End Sub

    Private Sub Packet_SaveAutoMap(ByVal Index As Integer, ByRef data() As Byte)
        Dim Layer As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved EMSG: SaveAutoMap", PACKET_LOG)
        TextAdd("Recieved EMSG: SaveAutoMap")

        If GetPlayerAccess(Index) = AdminType.Player Then Exit Sub

        MapStart = Buffer.ReadInt32
        MapSize = Buffer.ReadInt32
        MapX = Buffer.ReadInt32
        MapY = Buffer.ReadInt32
        SandBorder = Buffer.ReadInt32
        DetailFreq = Buffer.ReadInt32
        ResourceFreq = Buffer.ReadInt32

        Dim myXml As New XmlClass With {
            .Filename = Application.StartupPath & "\Data\AutoMapper.xml",
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

        Buffer.Dispose()

        StartAutomapper(MapStart, MapSize, MapX, MapY)

    End Sub

    Private Sub Packet_Emote(ByVal Index As Integer, ByRef data() As Byte)
        Dim Emote As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CEmote", PACKET_LOG)
        TextAdd("Recieved CMSG: CEmote")

        Emote = Buffer.ReadInt32

        SendEmote(Index, Emote)

        Buffer.Dispose()
    End Sub
End Module
