Imports System.Net.Sockets
Imports System.IO
Imports System.Windows.Forms
Imports ASFW
Imports ASFW.IO

Module ClientTCP
    Private PlayerBuffer As New ByteBuffer
    Public PlayerSocket As TcpClient
    Private SckConnecting As Boolean
    Private SckConnected As Boolean
    Private myStream As NetworkStream
    Private myReader As StreamReader
    Private myWriter As StreamWriter
    Private asyncBuff As Byte()
    Private asyncBuffs As New List(Of Byte())
    Public shouldHandleData As Boolean

    Public Sub Connect()
        If Not PlayerSocket Is Nothing Then
            Try
                If PlayerSocket.Connected Or SckConnecting Then Exit Sub
                PlayerSocket.Close()
                PlayerSocket = Nothing
            Catch ex As Exception

            End Try
        End If
        PlayerSocket = New TcpClient()
        PlayerSocket.ReceiveBufferSize = 4096
        PlayerSocket.SendBufferSize = 4096
        PlayerSocket.NoDelay = False
        ReDim asyncBuff(8192)
        PlayerSocket.BeginConnect(Options.IP, Options.Port, New AsyncCallback(AddressOf ConnectCallback), PlayerSocket)
        SckConnecting = True
    End Sub

    Sub ConnectCallback(asyncConnect As IAsyncResult)
        If Not PlayerSocket Is Nothing Then
            Try
                PlayerSocket.EndConnect(asyncConnect)
                If (PlayerSocket.Connected = False) Then
                    SckConnecting = False
                    SckConnected = False
                    Exit Sub
                Else
                    PlayerSocket.NoDelay = True
                    myStream = PlayerSocket.GetStream()
                    myStream.BeginRead(asyncBuff, 0, 8192, AddressOf OnReceive, Nothing)
                    SckConnected = True
                    SckConnecting = False
                End If
            Catch ex As Exception
                SckConnecting = False
                SckConnected = False
            End Try
        End If
    End Sub

    Sub OnReceive(ar As IAsyncResult)
        If Not PlayerSocket Is Nothing Then
            Try
                If PlayerSocket Is Nothing Then Exit Sub
                Dim byteAmt As Integer = myStream.EndRead(ar)
                Dim myBytes() As Byte
                ReDim myBytes(byteAmt - 1)
                Buffer.BlockCopy(asyncBuff, 0, myBytes, 0, byteAmt)
                If byteAmt = 0 Then
                    MsgBox("Disconnected.")
                    DestroyGame()
                    PlayerSocket.Close()
                    Exit Sub
                End If

                HandleData(myBytes)

                    If PlayerSocket Is Nothing Then Exit Sub
                    myStream.BeginRead(asyncBuff, 0, 8192, AddressOf OnReceive, Nothing)
                Catch ex As Exception
                    MsgBox("Disconnected.")
                DestroyGame()
                PlayerSocket.Close()
            End Try
        End If
    End Sub

    Public Function IsConnected() As Boolean
        If PlayerSocket Is Nothing Then Exit Function

        If PlayerSocket.Connected = True Then
            IsConnected = True
        Else
            IsConnected = False
        End If

    End Function

    Public Sub SendData(ByVal bytes() As Byte)
        Try
            If IsConnected() = False Then Exit Sub
            Dim Buffer As New ByteStream(4)
            Dim len = UBound(bytes) - LBound(bytes) + 1
            Buffer.WriteBytes(bytes)
            'Send data in the socket stream to the server
            myStream.Write(Buffer.Data, 0, len + 4)
            Buffer.Dispose()
            'writes the packet size and sends the data.....
        Catch ex As Exception
            MsgBox("Disconnected.")
            Application.Exit()
        End Try
    End Sub

    Public Sub SendNewAccount(ByVal Name As String, ByVal Password As String)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CNewAccount)
        Buffer.WriteString(EKeyPair.EncryptString(Name))
        Buffer.WriteString(EKeyPair.EncryptString(Password))
        SendData(Buffer.ToArray)

        Buffer.Dispose()
    End Sub

    Public Sub SendAddChar(ByVal Slot As Integer, ByVal Name As String, ByVal Sex As Integer, ByVal ClassNum As Integer, ByVal Sprite As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CAddChar)
        Buffer.WriteInt32(Slot)
        Buffer.WriteString(Name)
        Buffer.WriteInt32(Sex)
        Buffer.WriteInt32(ClassNum)
        Buffer.WriteInt32(Sprite)
        SendData(Buffer.ToArray())

        Buffer.Dispose()
    End Sub

    Public Sub SendLogin(ByVal Name As String, ByVal Password As String)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CLogin)
        Buffer.WriteString(EKeyPair.EncryptString(Name))
        Buffer.WriteString(EKeyPair.EncryptString(Password))
        Buffer.WriteString(EKeyPair.EncryptString(Application.ProductVersion))
        SendData(Buffer.ToArray())

        Buffer.Dispose()
    End Sub

    Sub GetPing()
        Dim Buffer As New ByteStream(4)
        PingStart = GetTickCount()

        Buffer.WriteInt32(ClientPackets.CCheckPing)
        SendData(Buffer.ToArray())

        Buffer.Dispose()
    End Sub

    Public Sub SendRequestEditMap()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CRequestEditMap)
        SendData(Buffer.ToArray())

        Buffer.Dispose()
    End Sub

    Public Sub SendMap()
        Dim X As Integer, Y As Integer, i As Integer
        Dim data() As Byte
        Dim Buffer As New ByteStream(4)
        CanMoveNow = False

        Buffer.WriteString(Trim$(Map.Name))
        Buffer.WriteString(Trim$(Map.Music))
        Buffer.WriteInt32(Map.Moral)
        Buffer.WriteInt32(Map.Tileset)
        Buffer.WriteInt32(Map.Up)
        Buffer.WriteInt32(Map.Down)
        Buffer.WriteInt32(Map.Left)
        Buffer.WriteInt32(Map.Right)
        Buffer.WriteInt32(Map.BootMap)
        Buffer.WriteInt32(Map.BootX)
        Buffer.WriteInt32(Map.BootY)
        Buffer.WriteInt32(Map.MaxX)
        Buffer.WriteInt32(Map.MaxY)
        Buffer.WriteInt32(Map.WeatherType)
        Buffer.WriteInt32(Map.FogIndex)
        Buffer.WriteInt32(Map.WeatherIntensity)
        Buffer.WriteInt32(Map.FogAlpha)
        Buffer.WriteInt32(Map.FogSpeed)
        Buffer.WriteInt32(Map.HasMapTint)
        Buffer.WriteInt32(Map.MapTintR)
        Buffer.WriteInt32(Map.MapTintG)
        Buffer.WriteInt32(Map.MapTintB)
        Buffer.WriteInt32(Map.MapTintA)
        Buffer.WriteInt32(Map.Instanced)
        Buffer.WriteInt32(Map.Panorama)
        Buffer.WriteInt32(Map.Parallax)

        For i = 1 To MAX_MAP_NPCS
            Buffer.WriteInt32(Map.Npc(i))
        Next

        For X = 0 To Map.MaxX
            For Y = 0 To Map.MaxY
                Buffer.WriteInt32(Map.Tile(X, Y).Data1)
                Buffer.WriteInt32(Map.Tile(X, Y).Data2)
                Buffer.WriteInt32(Map.Tile(X, Y).Data3)
                Buffer.WriteInt32(Map.Tile(X, Y).DirBlock)
                For i = 0 To MapLayer.Count - 1
                    Buffer.WriteInt32(Map.Tile(X, Y).Layer(i).Tileset)
                    Buffer.WriteInt32(Map.Tile(X, Y).Layer(i).X)
                    Buffer.WriteInt32(Map.Tile(X, Y).Layer(i).Y)
                    Buffer.WriteInt32(Map.Tile(X, Y).Layer(i).AutoTile)
                Next
                Buffer.WriteInt32(Map.Tile(X, Y).Type)
            Next
        Next

        'Event Data
        Buffer.WriteInt32(Map.EventCount)
        If Map.EventCount > 0 Then
            For i = 1 To Map.EventCount
                With Map.Events(i)
                    Buffer.WriteString(Trim$(.Name))
                    Buffer.WriteInt32(.Globals)
                    Buffer.WriteInt32(.X)
                    Buffer.WriteInt32(.Y)
                    Buffer.WriteInt32(.PageCount)
                End With
                If Map.Events(i).PageCount > 0 Then
                    For X = 1 To Map.Events(i).PageCount
                        With Map.Events(i).Pages(X)
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
                            Buffer.WriteInt32(Map.Events(i).Pages(X).MoveRouteCount)
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
                            Buffer.WriteInt32(.Questnum)

                            Buffer.WriteInt32(.chkPlayerGender)
                        End With
                        If Map.Events(i).Pages(X).CommandListCount > 0 Then
                            For Y = 1 To Map.Events(i).Pages(X).CommandListCount
                                Buffer.WriteInt32(Map.Events(i).Pages(X).CommandList(Y).CommandCount)
                                Buffer.WriteInt32(Map.Events(i).Pages(X).CommandList(Y).ParentList)
                                If Map.Events(i).Pages(X).CommandList(Y).CommandCount > 0 Then
                                    For z = 1 To Map.Events(i).Pages(X).CommandList(Y).CommandCount
                                        With Map.Events(i).Pages(X).CommandList(Y).Commands(z)
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

        data = Buffer.ToArray

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ClientPackets.CSaveMap)
        Buffer.WriteBlock(Compression.CompressBytes(data))

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendPlayerMove()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CPlayerMove)
        Buffer.WriteInt32(GetPlayerDir(MyIndex))
        Buffer.WriteInt32(Player(MyIndex).Moving)
        Buffer.WriteInt32(Player(MyIndex).X)
        Buffer.WriteInt32(Player(MyIndex).Y)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SayMsg(ByVal text As String)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CSayMsg)
        'Buffer.WriteString(text)
        Buffer.WriteBytes(WriteUnicodeString(text))

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendKick(ByVal Name As String)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CKickPlayer)
        Buffer.WriteString(Name)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendBan(ByVal Name As String)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CBanPlayer)
        Buffer.WriteString(Name)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub WarpMeTo(ByVal Name As String)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CWarpMeTo)
        Buffer.WriteString(Name)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub WarpToMe(ByVal Name As String)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CWarpToMe)
        Buffer.WriteString(Name)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub WarpTo(ByVal MapNum As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CWarpTo)
        Buffer.WriteInt32(MapNum)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendRequestLevelUp()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CRequestLevelUp)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Sub SendSpawnItem(ByVal tmpItem As Integer, ByVal tmpAmount As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CSpawnItem)
        Buffer.WriteInt32(tmpItem)
        Buffer.WriteInt32(tmpAmount)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendSetSprite(ByVal SpriteNum As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CSetSprite)
        Buffer.WriteInt32(SpriteNum)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendSetAccess(ByVal Name As String, ByVal Access As Byte)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CSetAccess)
        Buffer.WriteString(Name)
        Buffer.WriteInt32(Access)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Sub SendAttack()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CAttack)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Sub SendRequestItems()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CRequestItems)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendPlayerDir()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CPlayerDir)
        Buffer.WriteInt32(GetPlayerDir(MyIndex))

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendPlayerRequestNewMap()
        If GettingMap Then Exit Sub

        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CRequestNewMap)
        Buffer.WriteInt32(GetPlayerDir(MyIndex))

        SendData(Buffer.ToArray())
        Buffer.Dispose()

    End Sub

    Sub SendRequestResources()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CRequestResources)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Sub SendRequestNPCS()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CRequestNPCS)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Sub SendRequestSkills()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CRequestSkills)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Sub SendRequestShops()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CRequestShops)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Sub SendRequestAnimations()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CRequestAnimations)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendMapRespawn()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CMapRespawn)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Sub SendTrainStat(ByVal StatNum As Byte)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CTrainStat)
        Buffer.WriteInt32(StatNum)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Sub SendRequestPlayerData()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CRequestPlayerData)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub BroadcastMsg(ByVal text As String)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CBroadcastMsg)
        'Buffer.WriteString(text)
        Buffer.WriteBytes(WriteUnicodeString(text))

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub PlayerMsg(ByVal text As String, ByVal MsgTo As String)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CPlayerMsg)
        Buffer.WriteString(MsgTo)
        'Buffer.WriteString(text)
        Buffer.WriteBytes(WriteUnicodeString(text))

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendWhosOnline()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CWhosOnline)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendMOTDChange(ByVal MOTD As String)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CSetMotd)
        Buffer.WriteString(MOTD)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendBanList()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CBanList)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Sub SendChangeInvSlots(ByVal OldSlot As Integer, ByVal NewSlot As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CSwapInvSlots)
        Buffer.WriteInt32(OldSlot)
        Buffer.WriteInt32(NewSlot)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendUseItem(ByVal InvNum As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CUseItem)
        Buffer.WriteInt32(InvNum)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendDropItem(ByVal InvNum As Integer, ByVal Amount As Integer)
        Dim Buffer As New ByteStream(4)

        If InBank Or InShop Then Exit Sub

        ' do basic checks
        If InvNum < 1 Or InvNum > MAX_INV Then Exit Sub
        If PlayerInv(InvNum).Num < 1 Or PlayerInv(InvNum).Num > MAX_ITEMS Then Exit Sub
        If Item(GetPlayerInvItemNum(MyIndex, InvNum)).Type = ItemType.Currency Or Item(GetPlayerInvItemNum(MyIndex, InvNum)).Stackable = 1 Then
            If Amount < 1 Or Amount > PlayerInv(InvNum).Value Then Exit Sub
        End If

        Buffer.WriteInt32(ClientPackets.CMapDropItem)
        Buffer.WriteInt32(InvNum)
        Buffer.WriteInt32(Amount)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub BuyItem(ByVal shopslot As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CBuyItem)
        Buffer.WriteInt32(shopslot)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SellItem(ByVal invslot As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CSellItem)
        Buffer.WriteInt32(invslot)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub DepositItem(ByVal invslot As Integer, ByVal Amount As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CDepositItem)
        Buffer.WriteInt32(invslot)
        Buffer.WriteInt32(Amount)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub WithdrawItem(ByVal bankslot As Integer, ByVal Amount As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CWithdrawItem)
        Buffer.WriteInt32(bankslot)
        Buffer.WriteInt32(Amount)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub ChangeBankSlots(ByVal OldSlot As Integer, ByVal NewSlot As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CChangeBankSlots)
        Buffer.WriteInt32(OldSlot)
        Buffer.WriteInt32(NewSlot)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub CloseBank()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CCloseBank)

        SendData(Buffer.ToArray())
        Buffer.Dispose()

        InBank = False
        pnlBankVisible = False
    End Sub

    Sub PlayerSearch(ByVal CurX As Integer, ByVal CurY As Integer, ByVal RClick As Byte)
        Dim Buffer As New ByteStream(4)

        If IsInBounds() Then
            Buffer.WriteInt32(ClientPackets.CSearch)
            Buffer.WriteInt32(CurX)
            Buffer.WriteInt32(CurY)
            Buffer.WriteInt32(RClick)
            SendData(Buffer.ToArray())
        End If

        Buffer.Dispose()
    End Sub

    Public Sub AdminWarp(ByVal X As Integer, ByVal Y As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CAdminWarp)
        Buffer.WriteInt32(X)
        Buffer.WriteInt32(Y)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Sub SendTradeRequest(ByVal Name As String)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CTradeInvite)

        Buffer.WriteString(Name)

        SendData(Buffer.ToArray())
        Buffer.Dispose()

    End Sub

    Sub SendTradeInviteAccept(ByVal Awnser As Byte)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CTradeInviteAccept)

        Buffer.WriteInt32(Awnser)

        SendData(Buffer.ToArray())
        Buffer.Dispose()

    End Sub

    Public Sub TradeItem(ByVal invslot As Integer, ByVal Amount As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CTradeItem)
        Buffer.WriteInt32(invslot)
        Buffer.WriteInt32(Amount)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub UntradeItem(ByVal invslot As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CUntradeItem)
        Buffer.WriteInt32(invslot)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub AcceptTrade()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CAcceptTrade)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub DeclineTrade()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CDeclineTrade)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendLeaveGame()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CQuit)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Sub SendUnequip(ByVal EqNum As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CUnequip)
        Buffer.WriteInt32(EqNum)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub ForgetSkill(ByVal Skillslot As Integer)
        Dim Buffer As New ByteStream(4)

        ' Check for subscript out of range
        If Skillslot < 1 Or Skillslot > MAX_PLAYER_SKILLS Then Exit Sub

        ' dont let them forget a skill which is in CD
        If SkillCD(Skillslot) > 0 Then
            AddText("Cannot forget a skill which is cooling down!", QColorType.AlertColor)
            Exit Sub
        End If

        ' dont let them forget a skill which is buffered
        If SkillBuffer = Skillslot Then
            AddText("Cannot forget a skill which you are casting!", QColorType.AlertColor)
            Exit Sub
        End If

        If PlayerSkills(Skillslot) > 0 Then
            Buffer.WriteInt32(ClientPackets.CForgetSkill)
            Buffer.WriteInt32(Skillslot)
            SendData(Buffer.ToArray())
        Else
            AddText("No skill found.", QColorType.AlertColor)
        End If

        Buffer.Dispose()
    End Sub

    Public Sub SendRequestMapreport()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CMapReport)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendRequestAdmin()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CAdmin)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendRequestClasses()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CRequestClasses)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub

    Public Sub SendUseEmote(ByVal Emote As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CEmote)
        Buffer.WriteInt32(Emote)

        SendData(Buffer.ToArray())
        Buffer.Dispose()
    End Sub
End Module