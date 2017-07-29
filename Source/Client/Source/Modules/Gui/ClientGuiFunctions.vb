Imports System.Drawing
Imports System.Windows.Forms
Imports Orion
Imports SFML.Graphics
Imports SFML.Window

Public Module ClientGuiFunctions

#Region "Interaction"
    Public Sub CheckGuiMove(X As Integer, Y As Integer)
        Dim eqNum As Integer, InvNum As Integer, skillslot As Integer
        Dim bankitem As Integer, shopslot As Integer, TradeNum As Integer

        ShowItemDesc = False
        'Charpanel
        If pnlCharacterVisible Then
            If X > CharWindowX And X < CharWindowX + CharPanelGFXInfo.Width Then
                If Y > CharWindowY And Y < CharWindowY + CharPanelGFXInfo.Height Then
                    eqNum = IsEqItem(X, Y)
                    If eqNum <> 0 Then
                        UpdateDescWindow(GetPlayerEquipment(MyIndex, eqNum), 0, eqNum, 1)
                        LastItemDesc = GetPlayerEquipment(MyIndex, eqNum) ' set it so you don't re-set values
                        ShowItemDesc = True
                        Exit Sub
                    Else
                        ShowItemDesc = False
                        LastItemDesc = 0 ' no item was last loaded
                    End If
                End If
            End If
        End If

        'inventory
        If pnlInventoryVisible Then
            If AboveInvpanel(X, Y) Then
                InvX = X
                InvY = Y

                If DragInvSlotNum > 0 Then
                    If InTrade Then Exit Sub
                    If InBank Or InShop Then Exit Sub
                    DrawInventoryItem(X, Y)
                    ShowItemDesc = False
                    LastItemDesc = 0 ' no item was last loaded
                Else
                    InvNum = IsInvItem(X, Y)

                    If InvNum <> 0 Then
                        ' exit out if we're offering that item
                        For i = 1 To MAX_INV
                            If TradeYourOffer(i).Num = InvNum Then
                                Exit Sub
                            End If
                        Next
                        UpdateDescWindow(GetPlayerInvItemNum(MyIndex, InvNum), GetPlayerInvItemValue(MyIndex, InvNum), InvNum, 0)
                        LastItemDesc = GetPlayerInvItemNum(MyIndex, InvNum) ' set it so you don't re-set values
                        ShowItemDesc = True
                        Exit Sub
                    Else
                        ShowItemDesc = False
                        LastItemDesc = 0 ' no item was last loaded
                    End If
                End If
            End If
        End If

        'skills
        If pnlSkillsVisible = True Then
            If AboveSkillpanel(X, Y) Then
                SkillX = X
                SkillY = Y

                If DragSkillSlotNum > 0 Then
                    If InTrade Then Exit Sub
                    If InBank Or InShop Then Exit Sub
                    DrawSkillItem(X, Y)
                    LastSkillDesc = 0 ' no item was last loaded
                    ShowSkillDesc = False
                Else
                    skillslot = IsPlayerSkill(X, Y)

                    If skillslot <> 0 Then
                        UpdateSkillWindow(PlayerSkills(skillslot))
                        LastSkillDesc = PlayerSkills(skillslot)
                        ShowSkillDesc = True
                        Exit Sub
                    Else
                        LastSkillDesc = 0
                        ShowSkillDesc = False
                    End If
                End If

            End If
        End If

        'bank
        If pnlBankVisible = True Then
            If AboveBankpanel(X, Y) Then
                BankX = X
                BankY = Y

                If DragBankSlotNum > 0 Then
                    DrawBankItem(X, Y)
                Else
                    bankitem = IsBankItem(X, Y)

                    If bankitem <> 0 Then

                        UpdateDescWindow(Bank.Item(bankitem).Num, Bank.Item(bankitem).Value, bankitem, 2)
                        ShowItemDesc = True
                        Exit Sub
                    Else
                        ShowItemDesc = False
                        LastItemDesc = 0 ' no item was last loaded
                    End If
                End If

            End If
        End If

        'shop
        If pnlShopVisible = True Then
            If AboveShoppanel(X, Y) Then
                shopslot = IsShopItem(X, Y)

                If shopslot <> 0 Then

                    UpdateDescWindow(Shop(InShop).TradeItem(shopslot).Item, Shop(InShop).TradeItem(shopslot).ItemValue, shopslot, 3)
                    LastItemDesc = Shop(InShop).TradeItem(shopslot).Item
                    ShowItemDesc = True
                    Exit Sub
                Else
                    ShowItemDesc = False
                    LastItemDesc = 0
                End If

            End If
        End If

        'trade
        If pnlTradeVisible = True Then
            If AboveTradepanel(X, Y) Then
                TradeX = X
                TradeY = Y

                'ours
                TradeNum = IsTradeItem(X, Y, True)

                If TradeNum <> 0 Then
                    UpdateDescWindow(GetPlayerInvItemNum(MyIndex, TradeYourOffer(TradeNum).Num), TradeYourOffer(TradeNum).Value, TradeNum, 4)
                    LastItemDesc = GetPlayerInvItemNum(MyIndex, TradeYourOffer(TradeNum).Num) ' set it so you don't re-set values
                    ShowItemDesc = True
                    Exit Sub
                Else
                    ShowItemDesc = False
                    LastItemDesc = 0
                End If

                'theirs
                TradeNum = IsTradeItem(X, Y, False)

                If TradeNum <> 0 Then
                    UpdateDescWindow(TradeTheirOffer(TradeNum).Num, TradeTheirOffer(TradeNum).Value, TradeNum, 4)
                    LastItemDesc = TradeTheirOffer(TradeNum).Num ' set it so you don't re-set values
                    ShowItemDesc = True
                    Exit Sub
                Else
                    ShowItemDesc = False
                    LastItemDesc = 0
                End If
            End If
        End If

    End Sub

    Public Function CheckGuiClick(X As Integer, Y As Integer, e As MouseEventArgs) As Boolean
        Dim EqNum As Integer, InvNum As Integer
        Dim slotnum As Integer, hotbarslot As Integer
        Dim Buffer As ByteBuffer

        CheckGuiClick = False
        'action panel
        If HUDVisible And HideGui = False Then
            If AboveActionPanel(X, Y) Then
                ' left click
                If e.Button = MouseButtons.Left Then
                    'Inventory
                    If X > ActionPanelX + InvBtnX And X < ActionPanelX + InvBtnX + 48 And Y > ActionPanelY + InvBtnY And Y < ActionPanelY + InvBtnY + 32 Then
                        PlaySound("Click.ogg")
                        pnlInventoryVisible = Not pnlInventoryVisible
                        pnlCharacterVisible = False
                        pnlSkillsVisible = False
                        CheckGuiClick = True
                        'Skills
                    ElseIf X > ActionPanelX + SkillBtnX And X < ActionPanelX + SkillBtnX + 48 And Y > ActionPanelY + SkillBtnY And Y < ActionPanelY + SkillBtnY + 32 Then
                        PlaySound("Click.ogg")
                        Buffer = New ByteBuffer
                        Buffer.WriteInteger(ClientPackets.CSkills)
                        SendData(Buffer.ToArray())
                        Buffer = Nothing
                        pnlSkillsVisible = Not pnlSkillsVisible
                        pnlInventoryVisible = False
                        pnlCharacterVisible = False
                        CheckGuiClick = True
                        'Char
                    ElseIf X > ActionPanelX + CharBtnX And X < ActionPanelX + CharBtnX + 48 And Y > ActionPanelY + CharBtnY And Y < ActionPanelY + CharBtnY + 32 Then
                        PlaySound("Click.ogg")
                        SendRequestPlayerData()
                        pnlCharacterVisible = Not pnlCharacterVisible
                        pnlInventoryVisible = False
                        pnlSkillsVisible = False
                        CheckGuiClick = True
                        'Quest
                    ElseIf X > ActionPanelX + QuestBtnX And X < ActionPanelX + QuestBtnX + 48 And Y > ActionPanelY + QuestBtnY And Y < ActionPanelY + QuestBtnY + 32 Then
                        UpdateQuestLog()
                        ' show the window
                        pnlInventoryVisible = False
                        pnlCharacterVisible = False
                        RefreshQuestLog()
                        pnlQuestLogVisible = Not pnlQuestLogVisible
                        CheckGuiClick = True
                        'Options
                    ElseIf X > ActionPanelX + OptBtnX And X < ActionPanelX + OptBtnX + 48 And Y > ActionPanelY + OptBtnY And Y < ActionPanelY + OptBtnY + 32 Then
                        PlaySound("Click.ogg")
                        pnlCharacterVisible = False
                        pnlInventoryVisible = False
                        pnlSkillsVisible = False

                        OptionsVisible = Not OptionsVisible
                        frmOptions.BringToFront()
                        CheckGuiClick = True
                        'Exit
                    ElseIf X > ActionPanelX + ExitBtnX And X < ActionPanelX + ExitBtnX + 48 And Y > ActionPanelY + ExitBtnY And Y < ActionPanelY + ExitBtnY + 32 Then
                        PlaySound("Click.ogg")
                        frmAdmin.Dispose()
                        SendLeaveGame()
                        'DestroyGame()

                        CheckGuiClick = True
                    End If
                End If
            End If

            'hotbar
            If AboveHotbar(X, Y) Then

                hotbarslot = IsHotBarSlot(e.Location.X, e.Location.Y)

                If e.Button = MouseButtons.Left Then
                    If hotbarslot > 0 Then
                        slotnum = Player(MyIndex).Hotbar(hotbarslot).Slot

                        If slotnum <> 0 Then
                            PlaySound("Click.ogg")
                            SendUseHotbarSlot(hotbarslot)
                        End If

                        CheckGuiClick = True
                    End If
                ElseIf e.Button = MouseButtons.Right Then ' right click
                    If Player(MyIndex).Hotbar(hotbarslot).Slot > 0 Then
                        'forget hotbar skill
                        Dim result1 As DialogResult = MessageBox.Show("Want to Delete this from your hotbar?", GAME_NAME, MessageBoxButtons.YesNo)
                        If result1 = DialogResult.Yes Then
                            SendDeleteHotbar(IsHotBarSlot(e.Location.X, e.Location.Y))
                        End If

                        CheckGuiClick = True
                    Else
                        Buffer = New ByteBuffer
                        Buffer.WriteInteger(ClientPackets.CSkills)
                        SendData(Buffer.ToArray())
                        Buffer = Nothing
                        pnlSkillsVisible = True
                        AddText("Click on the skill you want to place here", QColorType.TellColor)
                        SelSkillSlot = True
                        SelHotbarSlot = IsHotBarSlot(e.Location.X, e.Location.Y)
                    End If
                End If
                CheckGuiClick = True
            End If

            If AbovePetbar(X, Y) Then
                If Player(MyIndex).Pet.Num > 0 Then
                    hotbarslot = IsPetBarSlot(e.Location.X, e.Location.Y)

                    If e.Button = MouseButtons.Left Then
                        If hotbarslot > 0 Then
                            If hotbarslot >= 1 AndAlso hotbarslot <= 3 Then
                                If hotbarslot = 1 Then
                                    'summon
                                    SendSummonPet()
                                ElseIf hotbarslot = 2 Then
                                    SendPetBehaviour(PET_ATTACK_BEHAVIOUR_ATTACKONSIGHT)
                                ElseIf hotbarslot = 3 Then
                                    SendPetBehaviour(PET_ATTACK_BEHAVIOUR_GUARD)
                                End If

                            ElseIf hotbarslot >= 4 AndAlso hotbarslot <= 7 Then
                                slotnum = Player(MyIndex).Pet.Skill(hotbarslot - 3)

                                If slotnum <> 0 Then
                                    PlaySound("Click.ogg")
                                    SendUsePetSkill(slotnum)
                                End If
                            End If

                            CheckGuiClick = True
                        End If
                    End If

                    CheckGuiClick = True
                End If
            End If

        End If

        'Charpanel
        If pnlCharacterVisible Then
            If AboveCharpanel(X, Y) Then
                ' left click
                If e.Button = MouseButtons.Left Then

                    'lets see if they want to upgrade
                    'Strenght
                    If X > CharWindowX + StrengthUpgradeX And X < CharWindowX + StrengthUpgradeX + 10 And Y > CharWindowY + StrengthUpgradeY And Y < CharWindowY + StrengthUpgradeY + 10 Then
                        If Not GetPlayerPOINTS(MyIndex) = 0 Then
                            PlaySound("Click.ogg")
                            SendTrainStat(1)
                        End If
                    End If
                    'Endurance
                    If X > CharWindowX + EnduranceUpgradeX And X < CharWindowX + EnduranceUpgradeX + 10 And Y > CharWindowY + EnduranceUpgradeY And Y < CharWindowY + EnduranceUpgradeY + 10 Then
                        If Not GetPlayerPOINTS(MyIndex) = 0 Then
                            PlaySound("Click.ogg")
                            SendTrainStat(2)
                        End If
                    End If
                    'Vitality
                    If X > CharWindowX + VitalityUpgradeX And X < CharWindowX + VitalityUpgradeX + 10 And Y > CharWindowY + VitalityUpgradeY And Y < CharWindowY + VitalityUpgradeY + 10 Then
                        If Not GetPlayerPOINTS(MyIndex) = 0 Then
                            PlaySound("Click.ogg")
                            SendTrainStat(3)
                        End If
                    End If
                    'WillPower
                    If X > CharWindowX + LuckUpgradeX And X < CharWindowX + LuckUpgradeX + 10 And Y > CharWindowY + LuckUpgradeY And Y < CharWindowY + LuckUpgradeY + 10 Then
                        If Not GetPlayerPOINTS(MyIndex) = 0 Then
                            PlaySound("Click.ogg")
                            SendTrainStat(4)
                        End If
                    End If
                    'Intellect
                    If X > CharWindowX + IntellectUpgradeX And X < CharWindowX + IntellectUpgradeX + 10 And Y > CharWindowY + IntellectUpgradeY And Y < CharWindowY + IntellectUpgradeY + 10 Then
                        If Not GetPlayerPOINTS(MyIndex) = 0 Then
                            PlaySound("Click.ogg")
                            SendTrainStat(5)
                        End If
                    End If
                    'Spirit
                    If X > CharWindowX + SpiritUpgradeX And X < CharWindowX + SpiritUpgradeX + 10 And Y > CharWindowY + SpiritUpgradeY And Y < CharWindowY + SpiritUpgradeY + 10 Then
                        If Not GetPlayerPOINTS(MyIndex) = 0 Then
                            PlaySound("Click.ogg")
                            SendTrainStat(6)
                        End If
                    End If
                    CheckGuiClick = True
                ElseIf e.Button = MouseButtons.Right Then
                    'first check for equip
                    EqNum = IsEqItem(X, Y)

                    If EqNum <> 0 Then
                        PlaySound("Click.ogg")
                        Dim result1 As DialogResult = MessageBox.Show("Want to Unequip this?", GAME_NAME, MessageBoxButtons.YesNo)
                        If result1 = DialogResult.Yes Then
                            SendUnequip(EqNum)
                        End If
                        CheckGuiClick = True
                    End If
                End If
            End If

            'Inventory panel
        ElseIf pnlInventoryVisible Then
            If AboveInvpanel(X, Y) Then
                InvNum = IsInvItem(e.Location.X, e.Location.Y)

                If e.Button = MouseButtons.Left Then
                    If InvNum <> 0 Then
                        If InTrade Then Exit Function
                        If InBank Or InShop Then Exit Function

                        If Item(GetPlayerInvItemNum(MyIndex, InvNum)).Type = ItemType.Furniture Then
                            PlaySound("Click.ogg")
                            FurnitureSelected = InvNum
                            CheckGuiClick = True
                        End If

                    End If
                End If
            End If
        End If

        If DialogPanelVisible Then
            'ok button
            If X > DialogPanelX + OkButtonX And X < DialogPanelX + OkButtonX + ButtonGFXInfo.Width And Y > DialogPanelY + OkButtonY And Y < DialogPanelY + OkButtonY + ButtonGFXInfo.Height Then
                VbKeyDown = False
                VbKeyUp = False
                VbKeyLeft = False
                VbKeyRight = False

                If DialogType = DIALOGUE_TYPE_BUYHOME Then 'house offer
                    SendBuyHouse(1)
                ElseIf DialogType = DIALOGUE_TYPE_VISIT Then
                    SendVisit(1)
                ElseIf DialogType = DIALOGUE_TYPE_PARTY Then
                    SendAcceptParty()
                ElseIf DialogType = DIALOGUE_TYPE_QUEST Then
                    If QuestAcceptTag > 0 Then
                        PlayerHandleQuest(QuestAcceptTag, 1)
                        QuestAcceptTag = 0
                        RefreshQuestLog()
                    End If
                ElseIf DialogType = DIALOGUE_TYPE_TRADE Then
                    SendTradeInviteAccept(1)
                End If

                PlaySound("Click.ogg")
                DialogPanelVisible = False
            End If
            'cancel button
            If X > DialogPanelX + CancelButtonX And X < DialogPanelX + CancelButtonX + ButtonGFXInfo.Width And Y > DialogPanelY + CancelButtonY And Y < DialogPanelY + CancelButtonY + ButtonGFXInfo.Height Then
                VbKeyDown = False
                VbKeyUp = False
                VbKeyLeft = False
                VbKeyRight = False

                If DialogType = DIALOGUE_TYPE_BUYHOME Then 'house offer declined
                    SendBuyHouse(0)
                ElseIf DIALOGUE_TYPE_VISIT Then 'visit declined
                    SendVisit(0)
                ElseIf DIALOGUE_TYPE_PARTY Then 'party declined
                    SendLeaveParty()
                ElseIf DIALOGUE_TYPE_QUEST Then 'quest declined
                    QuestAcceptTag = 0
                ElseIf DialogType = DIALOGUE_TYPE_TRADE Then
                    SendTradeInviteAccept(0)
                End If
                PlaySound("Click.ogg")
                DialogPanelVisible = False
            End If
            CheckGuiClick = True
        End If

        If pnlBankVisible = True Then
            If AboveBankpanel(X, Y) Then
                If X > BankWindowX + 140 And X < BankWindowX + 140 + GetTextWidth("Close Bank", 15) Then
                    If Y > BankWindowY + BankPanelGFXInfo.Height - 15 And Y < BankWindowY + BankPanelGFXInfo.Height Then
                        PlaySound("Click.ogg")
                        CloseBank()
                    End If
                End If
                CheckGuiClick = True
            End If
        End If

        'trade
        If pnlTradeVisible = True Then
            If AboveTradepanel(X, Y) Then
                'accept button
                If X > TradeWindowX + TradeButtonAcceptX And X < TradeWindowX + TradeButtonAcceptX + ButtonGFXInfo.Width Then
                    If Y > TradeWindowY + TradeButtonAcceptY And Y < TradeWindowY + TradeButtonAcceptY + ButtonGFXInfo.Height Then
                        PlaySound("Click.ogg")
                        AcceptTrade()
                    End If
                End If

                'decline button
                If X > TradeWindowX + TradeButtonDeclineX And X < TradeWindowX + TradeButtonDeclineX + ButtonGFXInfo.Width Then
                    If Y > TradeWindowY + TradeButtonDeclineY And Y < TradeWindowY + TradeButtonDeclineY + ButtonGFXInfo.Height Then
                        PlaySound("Click.ogg")
                        DeclineTrade()
                    End If
                End If

                CheckGuiClick = True
            End If

        End If

        'eventchat
        If pnlEventChatVisible = True Then
            If AboveEventChat(X, Y) Then
                'Response1
                If EventChoiceVisible(1) Then
                    If X > EventChatX + 10 And X < EventChatX + 10 + GetTextWidth(EventChoices(1)) Then
                        If Y > EventChatY + 124 And Y < EventChatY + 124 + 13 Then
                            PlaySound("Click.ogg")
                            Buffer = New ByteBuffer
                            Buffer.WriteInteger(ClientPackets.CEventChatReply)
                            Buffer.WriteInteger(EventReplyID)
                            Buffer.WriteInteger(EventReplyPage)
                            Buffer.WriteInteger(1)
                            SendData(Buffer.ToArray)
                            Buffer = Nothing
                            ClearEventChat()
                            InEvent = False
                        End If
                    End If
                End If

                'Response2
                If EventChoiceVisible(2) Then
                    If X > EventChatX + 10 And X < EventChatX + 10 + GetTextWidth(EventChoices(2)) Then
                        If Y > EventChatY + 146 And Y < EventChatY + 146 + 13 Then
                            PlaySound("Click.ogg")
                            Buffer = New ByteBuffer
                            Buffer.WriteInteger(ClientPackets.CEventChatReply)
                            Buffer.WriteInteger(EventReplyID)
                            Buffer.WriteInteger(EventReplyPage)
                            Buffer.WriteInteger(2)
                            SendData(Buffer.ToArray)
                            Buffer = Nothing
                            ClearEventChat()
                            InEvent = False
                        End If
                    End If
                End If

                'Response3
                If EventChoiceVisible(3) Then
                    If X > EventChatX + 226 And X < EventChatX + 226 + GetTextWidth(EventChoices(3)) Then
                        If Y > EventChatY + 124 And Y < EventChatY + 124 + 13 Then
                            PlaySound("Click.ogg")
                            Buffer = New ByteBuffer
                            Buffer.WriteInteger(ClientPackets.CEventChatReply)
                            Buffer.WriteInteger(EventReplyID)
                            Buffer.WriteInteger(EventReplyPage)
                            Buffer.WriteInteger(3)
                            SendData(Buffer.ToArray)
                            Buffer = Nothing
                            ClearEventChat()
                            InEvent = False
                        End If
                    End If
                End If

                'Response4
                If EventChoiceVisible(4) Then
                    If X > EventChatX + 226 And X < EventChatX + 226 + GetTextWidth(EventChoices(4)) Then
                        If Y > EventChatY + 146 And Y < EventChatY + 146 + 13 Then
                            PlaySound("Click.ogg")
                            Buffer = New ByteBuffer
                            Buffer.WriteInteger(ClientPackets.CEventChatReply)
                            Buffer.WriteInteger(EventReplyID)
                            Buffer.WriteInteger(EventReplyPage)
                            Buffer.WriteInteger(4)
                            SendData(Buffer.ToArray)
                            Buffer = Nothing
                            ClearEventChat()
                            InEvent = False
                        End If
                    End If
                End If

                'continue
                If EventChatType <> 1 Then
                    If X > EventChatX + 410 And X < EventChatX + 410 + GetTextWidth("Continue") Then
                        If Y > EventChatY + 156 And Y < EventChatY + 156 + 13 Then
                            PlaySound("Click.ogg")
                            Buffer = New ByteBuffer
                            Buffer.WriteInteger(ClientPackets.CEventChatReply)
                            Buffer.WriteInteger(EventReplyID)
                            Buffer.WriteInteger(EventReplyPage)
                            Buffer.WriteInteger(0)
                            SendData(Buffer.ToArray)
                            Buffer = Nothing
                            ClearEventChat()
                            InEvent = False
                        End If
                    End If
                End If
                CheckGuiClick = True
            End If
        End If

        'right click
        If pnlRClickVisible = True Then
            If AboveRClickPanel(X, Y) Then
                'trade
                If X > RClickX + (RClickGFXInfo.Width \ 2) - (GetTextWidth("Invite to Trade") \ 2) And X < RClickX + (RClickGFXInfo.Width \ 2) - (GetTextWidth("Invite to Trade") \ 2) + GetTextWidth("Invite to Trade") Then
                    If Y > RClickY + 35 And Y < RClickY + 35 + 12 Then
                        If myTarget > 0 Then
                            SendTradeRequest(Player(myTarget).Name)
                        End If
                        pnlRClickVisible = False
                    End If
                End If

                'party
                If X > RClickX + (RClickGFXInfo.Width \ 2) - (GetTextWidth("Invite to Party") \ 2) And X < RClickX + (RClickGFXInfo.Width \ 2) - (GetTextWidth("Invite to Party") \ 2) + GetTextWidth("Invite to Party") Then
                    If Y > RClickY + 60 And Y < RClickY + 60 + 12 Then
                        If myTarget > 0 Then
                            SendPartyRequest(Player(myTarget).Name)
                        End If
                        pnlRClickVisible = False
                    End If
                End If

                'House
                If X > RClickX + (RClickGFXInfo.Width \ 2) - (GetTextWidth("Invite to House") \ 2) And X < RClickX + (RClickGFXInfo.Width \ 2) - (GetTextWidth("Invite to House") \ 2) + GetTextWidth("Invite to House") Then
                    If Y > RClickY + 85 And Y < RClickY + 85 + 12 Then
                        If myTarget > 0 Then
                            SendInvite(Player(myTarget).Name)
                        End If
                        pnlRClickVisible = False
                    End If
                End If

                CheckGuiClick = True
            End If
        End If

        If pnlQuestLogVisible Then
            If AboveQuestPanel(X, Y) Then
                'check if they press the list
                Dim tmpy As Integer = 10
                For i = 1 To MAX_ACTIVEQUESTS
                    If Len(Trim$(QuestNames(i))) > 0 Then
                        If X > (QuestLogX + 7) And X < (QuestLogX + 7) + (GetTextWidth(QuestNames(i))) Then
                            If Y > (QuestLogY + tmpy) And Y < (QuestLogY + tmpy + 13) Then
                                SelectedQuest = i
                                LoadQuestlogBox()
                            End If
                        End If
                        tmpy = tmpy + 20
                    End If
                Next

                'close button
                If X > (QuestLogX + 195) And X < (QuestLogX + 290) Then
                    If Y > (QuestLogY + 358) And Y < (QuestLogY + 375) Then
                        ResetQuestLog()
                    End If

                    CheckGuiClick = True
                End If
            End If
        End If

        If pnlCraftVisible Then
            If AboveCraftPanel(X, Y) Then
                'check if they press the list
                Dim tmpy As Integer = 10
                For i = 1 To MAX_RECIPE
                    If Len(Trim$(RecipeNames(i))) > 0 Then
                        If X > (CraftPanelX + 12) And X < (CraftPanelX + 12) + (GetTextWidth(RecipeNames(i))) Then
                            If Y > (CraftPanelY + tmpy) And Y < (CraftPanelY + tmpy + 13) Then
                                SelectedRecipe = i
                                CraftingInit()
                            End If
                        End If
                        tmpy = tmpy + 20
                    End If
                Next

                'start button
                If X > (CraftPanelX + 256) And X < (CraftPanelX + 330) Then
                    If Y > (CraftPanelY + 415) And Y < (CraftPanelY + 437) Then
                        If SelectedRecipe > 0 Then
                            CraftProgressValue = 0
                            SendCraftIt(RecipeNames(SelectedRecipe), CraftAmountValue)
                        End If
                    End If
                End If

                'close button
                If X > (CraftPanelX + 614) And X < (CraftPanelX + 689) Then
                    If Y > (CraftPanelY + 472) And Y < (CraftPanelY + 494) Then
                        ResetCraftPanel()
                        pnlCraftVisible = False
                        InCraft = False
                        SendCloseCraft()
                    End If
                End If

                'minus
                If X > (CraftPanelX + 340) And X < (CraftPanelX + 340 + 10) Then
                    If Y > (CraftPanelY + 422) And Y < (CraftPanelY + 422 + 10) Then
                        If CraftAmountValue > 1 Then
                            CraftAmountValue = CraftAmountValue - 1
                        End If
                    End If
                End If

                'plus
                If X > (CraftPanelX + 392) And X < (CraftPanelX + 392 + 10) Then
                    If Y > (CraftPanelY + 422) And Y < (CraftPanelY + 422 + 10) Then
                        If CraftAmountValue < 100 Then
                            CraftAmountValue = CraftAmountValue + 1
                        End If
                    End If
                End If

                CheckGuiClick = True
            End If
        End If

    End Function

    Public Function CheckGuiDoubleClick(X As Integer, Y As Integer, e As MouseEventArgs) As Boolean
        Dim InvNum As Integer, skillnum As Integer, BankItem As Integer
        Dim Value As Integer, TradeNum As Integer
        Dim multiplier As Double
        Dim i As Integer

        If pnlInventoryVisible Then
            If AboveInvpanel(X, Y) Then
                DragInvSlotNum = 0
                InvNum = IsInvItem(InvX, InvY)

                If InvNum <> 0 Then

                    ' are we in a shop?
                    If InShop > 0 Then
                        Select Case ShopAction
                            Case 0 ' nothing, give value
                                multiplier = Shop(InShop).BuyRate / 100
                                Value = Item(GetPlayerInvItemNum(MyIndex, InvNum)).Price * multiplier
                                If Value > 0 Then
                                    AddText("You can sell this item for " & Value & " gold.", QColorType.TellColor)
                                Else
                                    AddText("The shop does not want this item.", QColorType.AlertColor)
                                End If
                            Case 2 ' 2 = sell
                                SellItem(InvNum)
                        End Select

                        Exit Function
                    End If

                    ' in bank?
                    If InBank Then
                        If Item(GetPlayerInvItemNum(MyIndex, InvNum)).Type = ItemType.Currency Or Item(GetPlayerInvItemNum(MyIndex, InvNum)).Stackable = 1 Then
                            CurrencyMenu = 2 ' deposit
                            FrmMainGame.lblCurrency.Text = "How many do you want to deposit?"
                            tmpCurrencyItem = InvNum
                            FrmMainGame.txtCurrency.Text = ""
                            FrmMainGame.pnlCurrency.Visible = True
                            FrmMainGame.pnlCurrency.BringToFront()
                            FrmMainGame.txtCurrency.Focus()
                            Exit Function
                        End If
                        DepositItem(InvNum, 0)
                        Exit Function
                    End If

                    ' in trade?
                    If InTrade = True Then
                        ' exit out if we're offering that item
                        For i = 1 To MAX_INV
                            If TradeYourOffer(i).Num = InvNum Then
                                Exit Function
                            End If
                        Next
                        If Item(GetPlayerInvItemNum(MyIndex, InvNum)).Type = ItemType.Currency Or Item(GetPlayerInvItemNum(MyIndex, InvNum)).Stackable = 1 Then
                            CurrencyMenu = 4 ' trade
                            FrmMainGame.lblCurrency.Text = "How many do you want to trade?"
                            tmpCurrencyItem = InvNum
                            FrmMainGame.txtCurrency.Text = ""
                            FrmMainGame.pnlCurrency.Visible = True
                            FrmMainGame.pnlCurrency.BringToFront()
                            FrmMainGame.txtCurrency.Focus()
                            Exit Function
                        End If
                        TradeItem(InvNum, 0)
                        Exit Function
                    End If

                    ' use item if not doing anything else
                    If Item(GetPlayerInvItemNum(MyIndex, InvNum)).Type = ItemType.None Then Exit Function
                    SendUseItem(InvNum)
                    Exit Function
                End If
            End If
        End If

        'Skill panel
        If pnlSkillsVisible = True Then
            If AboveSkillpanel(X, Y) Then

                skillnum = IsPlayerSkill(SkillX, SkillY)

                If skillnum <> 0 Then
                    PlayerCastSkill(skillnum)
                    Exit Function
                End If
            End If
        End If

        'Bank panel
        If pnlBankVisible = True Then
            If AboveBankpanel(X, Y) Then

                DragBankSlotNum = 0

                BankItem = IsBankItem(BankX, BankY)
                If BankItem <> 0 Then
                    If GetBankItemNum(BankItem) = ItemType.None Then Exit Function

                    If Item(GetBankItemNum(BankItem)).Type = ItemType.Currency Or Item(GetBankItemNum(BankItem)).Stackable = 1 Then
                        CurrencyMenu = 3 ' withdraw
                        FrmMainGame.lblCurrency.Text = "How many do you want to withdraw?"
                        tmpCurrencyItem = BankItem
                        FrmMainGame.txtCurrency.Text = ""
                        FrmMainGame.pnlCurrency.Visible = True
                        FrmMainGame.txtCurrency.Focus()
                        Exit Function
                    End If

                    WithdrawItem(BankItem, 0)
                    Exit Function
                End If
            End If
        End If

        'trade panel
        If pnlTradeVisible = True Then
            'ours?
            If AboveTradepanel(X, Y) Then
                TradeNum = IsTradeItem(TradeX, TradeY, True)

                If TradeNum <> 0 Then
                    UntradeItem(TradeNum)
                End If
            End If
        End If

    End Function

    Public Function CheckGuiMouseUp(X As Integer, Y As Integer, e As MouseEventArgs) As Boolean
        Dim i As Integer, rec_pos As Rectangle, buffer As ByteBuffer
        Dim hotbarslot As Integer

        'Inventory
        If pnlInventoryVisible Then
            If AboveInvpanel(X, Y) Then
                If InTrade > 0 Then Exit Function
                If InBank Or InShop Then Exit Function

                If DragInvSlotNum > 0 Then

                    For i = 1 To MAX_INV

                        With rec_pos
                            .Y = InvWindowY + InvTop + ((InvOffsetY + 32) * ((i - 1) \ InvColumns))
                            .Height = PIC_Y
                            .X = InvWindowX + InvLeft + ((InvOffsetX + 32) * (((i - 1) Mod InvColumns)))
                            .Width = PIC_X
                        End With

                        If e.Location.X >= rec_pos.Left And e.Location.X <= rec_pos.Right Then
                            If e.Location.Y >= rec_pos.Top And e.Location.Y <= rec_pos.Bottom Then '
                                If DragInvSlotNum <> i Then
                                    SendChangeInvSlots(DragInvSlotNum, i)
                                    Exit For
                                End If
                            End If
                        End If

                    Next

                End If

                DragInvSlotNum = 0
            ElseIf AboveHotbar(X, Y) Then
                If DragInvSlotNum > 0 Then
                    hotbarslot = IsHotBarSlot(e.Location.X, e.Location.Y)
                    If hotbarslot > 0 Then
                        SendSetHotbarSlot(hotbarslot, DragInvSlotNum, 2)
                    End If
                End If

                DragInvSlotNum = 0
            Else
                If FurnitureSelected > 0 Then
                    If Player(MyIndex).InHouse = MyIndex Then
                        If Item(PlayerInv(FurnitureSelected).Num).Type = ItemType.Furniture Then
                            buffer = New ByteBuffer
                            buffer.WriteInteger(ClientPackets.CPlaceFurniture)
                            i = CurX
                            buffer.WriteInteger(i)
                            i = CurY
                            buffer.WriteInteger(i)
                            buffer.WriteInteger(FurnitureSelected)
                            SendData(buffer.ToArray)
                            buffer = Nothing

                            FurnitureSelected = 0
                        End If
                    End If
                End If
            End If
        End If

        'skills
        If pnlSkillsVisible Then
            If AboveSkillpanel(X, Y) Then
                If InTrade > 0 Then Exit Function
                If InBank Or InShop Then Exit Function

                If DragSkillSlotNum > 0 Then

                    For i = 1 To MAX_PLAYER_SKILLS

                        With rec_pos
                            .Y = SkillWindowY + SkillTop + ((SkillOffsetY + 32) * ((i - 1) \ SkillColumns))
                            .Height = PIC_Y
                            .X = SkillWindowX + SkillLeft + ((SkillOffsetX + 32) * (((i - 1) Mod SkillColumns)))
                            .Width = PIC_X
                        End With

                        If e.Location.X >= rec_pos.Left And e.Location.X <= rec_pos.Right Then
                            If e.Location.Y >= rec_pos.Top And e.Location.Y <= rec_pos.Bottom Then '
                                If DragSkillSlotNum <> i Then
                                    'SendChangeSkillSlots(DragSkillSlotNum, i)
                                    Exit For
                                End If
                            End If
                        End If

                    Next

                End If

                DragSkillSlotNum = 0
            ElseIf AboveHotbar(X, Y) Then
                If DragSkillSlotNum > 0 Then
                    hotbarslot = IsHotBarSlot(e.Location.X, e.Location.Y)
                    If hotbarslot > 0 Then
                        SendSetHotbarSlot(hotbarslot, DragSkillSlotNum, 1)
                    End If
                End If

                DragSkillSlotNum = 0
            End If
        End If

        'bank
        If pnlBankVisible = True Then
            If AboveBankpanel(X, Y) Then
                ' TODO : Add sub to change bankslots client side first so there's no delay in switching
                If DragBankSlotNum > 0 Then
                    For i = 1 To MAX_BANK
                        With rec_pos
                            .Y = BankWindowY + BankTop + ((BankOffsetY + 32) * ((i - 1) \ BankColumns))
                            .Height = PIC_Y
                            .X = BankWindowX + BankLeft + ((BankOffsetX + 32) * (((i - 1) Mod BankColumns)))
                            .Width = PIC_X
                        End With

                        If X >= rec_pos.Left And X <= rec_pos.Right Then
                            If Y >= rec_pos.Top And Y <= rec_pos.Bottom Then
                                If DragBankSlotNum <> i Then
                                    ChangeBankSlots(DragBankSlotNum, i)
                                    Exit For
                                End If
                            End If
                        End If
                    Next
                End If

                DragBankSlotNum = 0
            End If
        End If

    End Function

    Public Function CheckGuiMouseDown(X As Integer, Y As Integer, e As MouseEventArgs) As Boolean
        Dim InvNum As Integer, skillnum As Integer, bankNum As Integer, shopItem As Integer

        'Inventory
        If pnlInventoryVisible Then
            If AboveInvpanel(X, Y) Then
                InvNum = IsInvItem(e.Location.X, e.Location.Y)

                If e.Button = MouseButtons.Left Then
                    If InvNum <> 0 Then
                        If InTrade Then Exit Function
                        If InBank Or InShop Then Exit Function
                        DragInvSlotNum = InvNum
                    End If
                ElseIf e.Button = MouseButtons.Right Then
                    If Not InBank And Not InShop And Not InTrade Then
                        If InvNum <> 0 Then
                            If Item(GetPlayerInvItemNum(MyIndex, InvNum)).Type = ItemType.Currency Or Item(GetPlayerInvItemNum(MyIndex, InvNum)).Stackable = 1 Then
                                If GetPlayerInvItemValue(MyIndex, InvNum) > 0 Then
                                    CurrencyMenu = 1 ' drop
                                    FrmMainGame.lblCurrency.Text = "How many do you want to drop?"
                                    tmpCurrencyItem = InvNum
                                    FrmMainGame.txtCurrency.Text = ""
                                    FrmMainGame.pnlCurrency.Visible = True
                                    FrmMainGame.txtCurrency.Focus()
                                End If
                            Else
                                SendDropItem(InvNum, 0)
                            End If
                        End If
                    End If
                End If
            End If
        End If

        'skills
        If pnlSkillsVisible = True Then
            If AboveSkillpanel(X, Y) Then
                skillnum = IsPlayerSkill(e.Location.X, e.Location.Y)
                If e.Button = MouseButtons.Left Then
                    If skillnum <> 0 Then
                        If InTrade Then Exit Function

                        DragSkillSlotNum = skillnum

                        If SelSkillSlot = True Then
                            SendSetHotbarSlot(SelHotbarSlot, skillnum, 1)
                        End If
                    End If
                ElseIf e.Button = MouseButtons.Right Then ' right click

                    If skillnum <> 0 Then
                        Dim result1 As DialogResult = MessageBox.Show("Want to forget this skill?", GAME_NAME, MessageBoxButtons.YesNo)
                        If result1 = DialogResult.Yes Then
                            ForgetSkill(skillnum)
                            Exit Function
                        End If
                    End If
                End If
            End If
        End If

        'Bank
        If pnlBankVisible = True Then
            If AboveBankpanel(X, Y) Then
                bankNum = IsBankItem(X, Y)

                If bankNum <> 0 Then

                    If e.Button = MouseButtons.Left Then
                        DragBankSlotNum = bankNum
                    End If

                End If
            End If
        End If

        'Shop
        If pnlShopVisible = True Then
            If AboveShoppanel(X, Y) Then
                shopItem = IsShopItem(X, Y)

                If shopItem > 0 Then
                    Select Case ShopAction
                        Case 0 ' no action, give cost
                            With Shop(InShop).TradeItem(shopItem)
                                AddText("You can buy this item for " & .CostValue & " " & Trim$(Item(.CostItem).Name) & ".", ColorType.Yellow)
                            End With
                        Case 1 ' buy item
                            ' buy item code
                            BuyItem(shopItem)
                    End Select
                Else
                    ' check for buy button
                    If X > ShopWindowX + ShopButtonBuyX And X < ShopWindowX + ShopButtonBuyX + ButtonGFXInfo.Width Then
                        If Y > ShopWindowY + ShopButtonBuyY And Y < ShopWindowY + ShopButtonBuyY + ButtonGFXInfo.Height Then
                            If ShopAction = 1 Then Exit Function
                            ShopAction = 1 ' buying an item
                            AddText("Click on the item in the shop you wish to buy.", ColorType.Yellow)
                        End If
                    End If
                    ' check for sell button
                    If X > ShopWindowX + ShopButtonSellX And X < ShopWindowX + ShopButtonSellX + ButtonGFXInfo.Width Then
                        If Y > ShopWindowY + ShopButtonSellY And Y < ShopWindowY + ShopButtonSellY + ButtonGFXInfo.Height Then
                            If ShopAction = 2 Then Exit Function
                            ShopAction = 2 ' selling an item
                            AddText("Double-click on the item in your inventory you wish to sell.", ColorType.Yellow)
                        End If
                    End If
                    ' check for close button
                    If X > ShopWindowX + ShopButtonCloseX And X < ShopWindowX + ShopButtonCloseX + ButtonGFXInfo.Width Then
                        If Y > ShopWindowY + ShopButtonCloseY And Y < ShopWindowY + ShopButtonCloseY + ButtonGFXInfo.Height Then
                            Dim Buffer As ByteBuffer
                            Buffer = New ByteBuffer
                            Buffer.WriteInteger(ClientPackets.CCloseShop)
                            SendData(Buffer.ToArray())
                            Buffer = Nothing
                            pnlShopVisible = False
                            InShop = 0
                            ShopAction = 0
                        End If
                    End If
                End If
            End If
        End If

        If HUDVisible = True Then
            If AboveChatScrollUp(X, Y) Then
                If ScrollMod + FirstLineIndex < MaxChatDisplayLines Then
                    ScrollMod = ScrollMod + 1
                End If
            End If
            If AboveChatScrollDown(X, Y) Then
                If ScrollMod - 1 >= 0 Then
                    ScrollMod = ScrollMod - 1
                End If
            End If
        End If

    End Function
#End Region

#Region "Drawing"
    Sub DrawChat()
        Dim i As Integer, x As Integer, y As Integer
        Dim text As String

        'first draw back image
        RenderSprite(ChatWindowSprite, GameWindow, ChatWindowX, ChatWindowY - 2, 0, 0, ChatWindowGFXInfo.Width, ChatWindowGFXInfo.Height)

        y = 5
        x = 5

        FirstLineIndex = (Chat.Count - MaxChatDisplayLines) - ScrollMod 'First element is the 5th from the last in the list
        If FirstLineIndex < 0 Then FirstLineIndex = 0 'if the list has less than 5 elements, the first is the 0th index or first element

        LastLineIndex = (FirstLineIndex + MaxChatDisplayLines) ' - ScrollMod
        If (LastLineIndex >= Chat.Count) Then LastLineIndex = Chat.Count - 1  'Based off of index 0, so the last element should be Chat.Count -1

        'only loop tru last entries
        For i = FirstLineIndex To LastLineIndex
            text = Chat(i).Text

            If text <> "" Then ' or not
                DrawText(ChatWindowX + x, ChatWindowY + y, text, GetSFMLColor(Chat(i).Color), SFML.Graphics.Color.Black, GameWindow)
                y = y + ChatLineSpacing + 1
            End If

        Next

        'My Text
        'first draw back image
        RenderSprite(MyChatWindowSprite, GameWindow, MyChatX, MyChatY - 5, 0, 0, MyChatWindowGFXInfo.Width, MyChatWindowGFXInfo.Height)

        If Len(ChatInput.CurrentMessage) > 0 Then
            Dim subText As String = ChatInput.CurrentMessage
            While GetTextWidth(subText) > MyChatWindowGFXInfo.Width - ChatEntryPadding
                subText = subText.Substring(1)
            End While
            DrawText(MyChatX + 5, MyChatY - 3, subText, SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)
        End If
    End Sub

    Public Sub DrawButton(ByVal Text As String, ByVal DestX As Integer, ByVal DestY As Integer, ByVal Hover As Byte)
        If Hover = 0 Then
            RenderSprite(ButtonSprite, GameWindow, DestX, DestY, 0, 0, ButtonGFXInfo.Width, ButtonGFXInfo.Height)

            DrawText(DestX + (ButtonGFXInfo.Width \ 2) - (GetTextWidth(Text) \ 2), DestY + (ButtonGFXInfo.Height \ 2) - (FONT_SIZE \ 2), Text, SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)
        Else
            RenderSprite(ButtonHoverSprite, GameWindow, DestX, DestY, 0, 0, ButtonHoverGFXInfo.Width, ButtonHoverGFXInfo.Height)

            DrawText(DestX + (ButtonHoverGFXInfo.Width \ 2) - (GetTextWidth(Text) \ 2), DestY + (ButtonHoverGFXInfo.Height \ 2) - (FONT_SIZE \ 2), Text, SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)
        End If

    End Sub

    Public Sub DrawBars()
        Dim tmpY As Integer
        Dim tmpX As Integer
        Dim barWidth As Integer
        Dim rec(1) As Rectangle

        If GettingMap Then Exit Sub

        ' check for casting time bar
        If SkillBuffer > 0 Then
            ' lock to player
            tmpX = GetPlayerX(MyIndex) * PIC_X + Player(MyIndex).XOffset
            tmpY = GetPlayerY(MyIndex) * PIC_Y + Player(MyIndex).YOffset + 35
            If Skill(PlayerSkills(SkillBuffer)).CastTime = 0 Then Skill(PlayerSkills(SkillBuffer)).CastTime = 1
            ' calculate the width to fill
            barWidth = ((GetTimeMs() - SkillBufferTimer) / ((GetTimeMs() - SkillBufferTimer) + (Skill(PlayerSkills(SkillBuffer)).CastTime * 1000)) * 64)
            ' draw bars
            rec(1) = New Rectangle(ConvertMapX(tmpX), ConvertMapY(tmpY), barWidth, 4)
            Dim rectShape As New RectangleShape(New Vector2f(barWidth, 4))
            rectShape.Position = New Vector2f(ConvertMapX(tmpX), ConvertMapY(tmpY))
            rectShape.FillColor = SFML.Graphics.Color.Cyan
            GameWindow.Draw(rectShape)
        End If

        If Options.ShowNpcBar = 1 Then
            ' check for hp bar
            For i = 1 To MAX_MAP_NPCS
                If Map.Npc Is Nothing Then Exit Sub
                If Map.Npc(i) > 0 Then
                    If Npc(MapNpc(i).Num).Behaviour = NpcBehavior.AttackOnSight Or Npc(MapNpc(i).Num).Behaviour = NpcBehavior.AttackWhenAttacked Or Npc(MapNpc(i).Num).Behaviour = NpcBehavior.Guard Then
                        ' lock to npc
                        tmpX = MapNpc(i).X * PIC_X + MapNpc(i).XOffset
                        tmpY = MapNpc(i).Y * PIC_Y + MapNpc(i).YOffset + 35
                        If MapNpc(i).Vital(Vitals.HP) > 0 Then
                            ' calculate the width to fill
                            barWidth = ((MapNpc(i).Vital(Vitals.HP) / (Npc(MapNpc(i).Num).Hp) * 32))
                            ' draw bars
                            rec(1) = New Rectangle(ConvertMapX(tmpX), ConvertMapY(tmpY), barWidth, 4)
                            Dim rectShape As New RectangleShape(New Vector2f(barWidth, 4))
                            rectShape.Position = New Vector2f(ConvertMapX(tmpX), ConvertMapY(tmpY - 75))
                            rectShape.FillColor = SFML.Graphics.Color.Red
                            GameWindow.Draw(rectShape)

                            If MapNpc(i).Vital(Vitals.MP) > 0 Then
                                ' calculate the width to fill
                                barWidth = ((MapNpc(i).Vital(Vitals.MP) / (Npc(MapNpc(i).Num).Stat(Stats.Intelligence) * 2) * 32))
                                ' draw bars
                                rec(1) = New Rectangle(ConvertMapX(tmpX), ConvertMapY(tmpY), barWidth, 4)
                                Dim rectShape2 As New RectangleShape(New Vector2f(barWidth, 4))
                                rectShape2.Position = New Vector2f(ConvertMapX(tmpX), ConvertMapY(tmpY - 80))
                                rectShape2.FillColor = SFML.Graphics.Color.Blue
                                GameWindow.Draw(rectShape2)
                            End If
                        End If
                    End If
                End If
            Next
        End If

        If PetAlive(MyIndex) Then
            ' draw own health bar
            If Player(MyIndex).Pet.Health > 0 And Player(MyIndex).Pet.Health <= Player(MyIndex).Pet.MaxHp Then
                ' lock to Player
                tmpX = Player(MyIndex).Pet.X * PIC_X + Player(MyIndex).Pet.XOffset
                tmpY = Player(MyIndex).Pet.Y * PIC_X + Player(MyIndex).Pet.YOffset + 35
                ' calculate the width to fill
                barWidth = ((Player(MyIndex).Pet.Health) / (Player(MyIndex).Pet.MaxHp)) * 32
                ' draw bars
                rec(1) = New Rectangle(ConvertMapX(tmpX), ConvertMapY(tmpY), barWidth, 4)
                Dim rectShape As New RectangleShape(New Vector2f(barWidth, 4))
                rectShape.Position = New Vector2f(ConvertMapX(tmpX), ConvertMapY(tmpY - 75))
                rectShape.FillColor = SFML.Graphics.Color.Red
                GameWindow.Draw(rectShape)
            End If
        End If
        ' check for pet casting time bar
        If PetSkillBuffer > 0 Then
            If Skill(Pet(Player(MyIndex).Pet.Num).Skill(PetSkillBuffer)).CastTime > 0 Then
                ' lock to pet
                tmpX = Player(MyIndex).Pet.X * PIC_X + Player(MyIndex).Pet.XOffset
                tmpY = Player(MyIndex).Pet.Y * PIC_Y + Player(MyIndex).Pet.YOffset + 35

                ' calculate the width to fill
                barWidth = (GetTimeMs() - PetSkillBufferTimer) / ((Skill(Pet(Player(MyIndex).Pet.Num).Skill(PetSkillBuffer)).CastTime * 1000)) * 64
                ' draw bar background
                rec(1) = New Rectangle(ConvertMapX(tmpX), ConvertMapY(tmpY), barWidth, 4)
                Dim rectShape As New RectangleShape(New Vector2f(barWidth, 4))
                rectShape.Position = New Vector2f(ConvertMapX(tmpX), ConvertMapY(tmpY))
                rectShape.FillColor = SFML.Graphics.Color.Cyan
                GameWindow.Draw(rectShape)
            End If
        End If
    End Sub

    Sub DrawMapName()
        DrawText(DrawMapNameX, DrawMapNameY, Strings.Get("gamegui", "mapname") & Map.Name, DrawMapNameColor, SFML.Graphics.Color.Black, GameWindow)
    End Sub

    Sub DrawCursor()
        RenderSprite(CursorSprite, GameWindow, CurMouseX, CurMouseY, 0, 0, CursorInfo.Width, CursorInfo.Height)
    End Sub

    Public Sub DrawTarget(X2 As Integer, Y2 As Integer)
        Dim rec As Rectangle
        Dim X As Integer, y As Integer
        Dim width As Integer, height As Integer

        With rec
            .Y = 0
            .Height = TargetGFXInfo.Height
            .X = 0
            .Width = TargetGFXInfo.Width / 2
        End With

        X = ConvertMapX(X2)
        y = ConvertMapY(Y2)
        width = (rec.Right - rec.Left)
        height = (rec.Bottom - rec.Top)

        RenderSprite(TargetSprite, GameWindow, X, y, rec.X, rec.Y, rec.Width, rec.Height)
    End Sub

    Public Sub DrawHover(X2 As Integer, Y2 As Integer)
        Dim rec As Rectangle
        Dim X As Integer, Y As Integer
        Dim width As Integer, height As Integer

        With rec
            .Y = 0
            .Height = TargetGFXInfo.Height
            .X = TargetGFXInfo.Width / 2
            .Width = TargetGFXInfo.Width / 2 + TargetGFXInfo.Width / 2
        End With

        X = ConvertMapX(X2)
        Y = ConvertMapY(Y2)
        width = (rec.Right - rec.Left)
        height = (rec.Bottom - rec.Top)

        RenderSprite(TargetSprite, GameWindow, X, Y, rec.X, rec.Y, rec.Width, rec.Height)
    End Sub

    Sub DrawHUD()
        Dim rec As Rectangle

        'first render backpanel
        With rec
            .Y = 0
            .Height = HUDPanelGFXInfo.Height
            .X = 0
            .Width = HUDPanelGFXInfo.Width
        End With

        RenderSprite(HUDPanelSprite, GameWindow, HUDWindowX, HUDWindowY, rec.X, rec.Y, rec.Width, rec.Height)

        If Player(MyIndex).Sprite <= NumFaces Then
            Dim tmpSprite As Sprite = New Sprite(FacesGFX(Player(MyIndex).Sprite))

            If FacesGFXInfo(Player(MyIndex).Sprite).IsLoaded = False Then
                LoadTexture(Player(MyIndex).Sprite, 7)
            End If

            'seeying we still use it, lets update timer
            With FacesGFXInfo(Player(MyIndex).Sprite)
                .TextureTimer = GetTimeMs() + 100000
            End With

            'then render face
            With rec
                .Y = 0
                .Height = FacesGFXInfo(Player(MyIndex).Sprite).Height
                .X = 0
                .Width = FacesGFXInfo(Player(MyIndex).Sprite).Width
            End With

            RenderSprite(FacesSprite(Player(MyIndex).Sprite), GameWindow, HUDFaceX, HUDFaceY, rec.X, rec.Y, rec.Width, rec.Height)
        End If

        'Hp Bar etc
        DrawStatBars()

        'Fps etc
        If FPS > 64 Then FPS = 64

        DrawText(HUDWindowX + HUDHPBarX + HPBarGFXInfo.Width + 10, HUDWindowY + HUDHPBarY + 4, Strings.Get("gamegui", "fps") & FPS, SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)
        DrawText(HUDWindowX + HUDMPBarX + MPBarGFXInfo.Width + 10, HUDWindowY + HUDMPBarY + 4, Strings.Get("gamegui", "ping") & PingToDraw, SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)
        DrawText(HUDWindowX + HUDEXPBarX + EXPBarGFXInfo.Width + 10, HUDWindowY + HUDEXPBarY + 4, Strings.Get("gamegui", "clock") & Time.Instance.ToString("h:mm"), SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)

        If BLPS Then
            DrawText(HUDWindowX + HUDEXPBarX + EXPBarGFXInfo.Width + 10, HUDWindowY + HUDEXPBarY + 20, Strings.Get("gamegui", "lps") & LPS, SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)
        End If

        ' Draw map name
        DrawMapName()
    End Sub

    Sub DrawStatBars()
        Dim rec As Rectangle
        Dim CurHP As Integer, CurMP As Integer, CurEXP As Integer

        'HP Bar
        CurHP = (GetPlayerVital(MyIndex, 1) / GetPlayerMaxVital(MyIndex, 1)) * 100

        With rec
            .Y = 0
            .Height = HPBarGFXInfo.Height
            .X = 0
            .Width = CurHP * HPBarGFXInfo.Width / 100
        End With

        'then render full ontop of it
        RenderSprite(HPBarSprite, GameWindow, HUDWindowX + HUDHPBarX, HUDWindowY + HUDHPBarY + 4, rec.X, rec.Y, rec.Width, rec.Height)

        'then draw the text onto that
        DrawText(HUDWindowX + HUDHPBarX + 65, HUDWindowY + HUDHPBarY + 4, GetPlayerVital(MyIndex, 1) & "/" & GetPlayerMaxVital(MyIndex, 1), SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)

        '==============================

        'MP Bar
        CurMP = (GetPlayerVital(MyIndex, 2) / GetPlayerMaxVital(MyIndex, 2)) * 100

        'then render full ontop of it
        With rec
            .Y = 0
            .Height = MPBarGFXInfo.Height
            .X = 0
            .Width = CurMP * MPBarGFXInfo.Width / 100
        End With

        RenderSprite(MPBarSprite, GameWindow, HUDWindowX + HUDMPBarX, HUDWindowY + HUDMPBarY + 4, rec.X, rec.Y, rec.Width, rec.Height)

        'draw text onto that
        DrawText(HUDWindowX + HUDMPBarX + 65, HUDWindowY + HUDMPBarY + 4, GetPlayerVital(MyIndex, 2) & "/" & GetPlayerMaxVital(MyIndex, 2), SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)

        '====================================================
        'EXP Bar
        CurEXP = (GetPlayerExp(MyIndex) / NextlevelExp) * 100

        'then render full ontop of it
        With rec
            .Y = 0
            .Height = EXPBarGFXInfo.Height
            .X = 0
            .Width = CurEXP * EXPBarGFXInfo.Width / 100
        End With

        RenderSprite(EXPBarSprite, GameWindow, HUDWindowX + HUDEXPBarX, HUDWindowY + HUDEXPBarY + 4, rec.X, rec.Y, rec.Width, rec.Height)

        'draw text onto that
        DrawText(HUDWindowX + HUDEXPBarX + 65, HUDWindowY + HUDEXPBarY + 4, GetPlayerExp(MyIndex) & "/" & NextlevelExp, SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)
    End Sub

    Sub DrawActionPanel()
        Dim rec As Rectangle

        'first render backpanel
        With rec
            .Y = 0
            .Height = ActionPanelGFXInfo.Height
            .X = 0
            .Width = ActionPanelGFXInfo.Width
        End With

        RenderSprite(ActionPanelSprite, GameWindow, ActionPanelX, ActionPanelY, rec.X, rec.Y, rec.Width, rec.Height)

    End Sub

    Public Sub DrawDialogPanel()
        'first render panel
        RenderSprite(EventChatSprite, GameWindow, DialogPanelX, DialogPanelY, 0, 0, EventChatGFXInfo.Width, EventChatGFXInfo.Height)

        DrawText(DialogPanelX + 175, DialogPanelY + 10, Trim(DialogMsg1), SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)

        If Len(DialogMsg2) > 0 Then
            DrawText(DialogPanelX + 60, DialogPanelY + 30, Trim(DialogMsg2), SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)
        End If

        If Len(DialogMsg3) > 0 Then
            DrawText(DialogPanelX + 60, DialogPanelY + 50, Trim(DialogMsg3), SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)
        End If

        If DialogType = DIALOGUE_TYPE_QUEST Then
            If QuestAcceptTag > 0 Then
                'render accept button
                DrawButton(DialogButton1Text, DialogPanelX + OkButtonX, DialogPanelY + OkButtonY, 0)
                DrawButton(DialogButton2Text, DialogPanelX + CancelButtonX, DialogPanelY + CancelButtonY, 0)
            Else
                'render cancel button
                DrawButton(DialogButton2Text, DialogPanelX + CancelButtonX - 140, DialogPanelY + CancelButtonY, 0)
            End If
        Else
            'render ok button
            DrawButton(DialogButton1Text, DialogPanelX + OkButtonX, DialogPanelY + OkButtonY, 0)

            'render cancel button
            DrawButton(DialogButton2Text, DialogPanelX + CancelButtonX, DialogPanelY + CancelButtonY, 0)
        End If

    End Sub

    Public Sub DrawRClick()
        'first render panel
        RenderSprite(RClickSprite, GameWindow, RClickX, RClickY, 0, 0, RClickGFXInfo.Width, RClickGFXInfo.Height)

        DrawText(RClickX + (RClickGFXInfo.Width \ 2) - (GetTextWidth(RClickname) \ 2), RClickY + 10, RClickname, SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)

        DrawText(RClickX + (RClickGFXInfo.Width \ 2) - (GetTextWidth("Invite to Trade") \ 2), RClickY + 35, "Invite to Trade", SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)

        DrawText(RClickX + (RClickGFXInfo.Width \ 2) - (GetTextWidth("Invite to Party") \ 2), RClickY + 60, "Invite to Party", SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)

        DrawText(RClickX + (RClickGFXInfo.Width \ 2) - (GetTextWidth("Invite to House") \ 2), RClickY + 85, "Invite to House", SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow)

    End Sub

    Public Sub DrawGUI()
        'hide GUI when mapping...
        If HideGui = True Then Exit Sub

        If HUDVisible = True Then
            DrawHUD()
            DrawActionPanel()
            DrawChat()
            DrawHotbar()
            DrawPetBar()
            DrawPetStats()
        End If

        If pnlCharacterVisible = True Then
            DrawEquipment()
            If ShowItemDesc = True Then DrawItemDesc()
        End If

        If pnlInventoryVisible = True Then
            DrawInventory()
            If ShowItemDesc = True Then DrawItemDesc()
        End If

        If pnlSkillsVisible = True Then
            DrawPlayerSkills()
            If ShowSkillDesc = True Then DrawSkillDesc()
        End If

        If DialogPanelVisible = True Then
            DrawDialogPanel()
        End If

        If pnlBankVisible = True Then
            DrawBank()
        End If

        If pnlShopVisible = True Then
            DrawShop()
        End If

        If pnlTradeVisible = True Then
            DrawTrade()
        End If

        If pnlEventChatVisible = True Then
            DrawEventChat()
        End If

        If pnlRClickVisible = True Then
            DrawRClick()
        End If

        If pnlQuestLogVisible = True Then
            DrawQuestLog()
        End If

        If pnlCraftVisible = True Then
            DrawCraftPanel()
        End If

        If DragInvSlotNum > 0 Then
            DrawInventoryItem(CurMouseX, CurMouseY)
        End If

        If DragBankSlotNum > 0 Then
            DrawBankItem(CurMouseX, CurMouseY)
        End If

        If DragSkillSlotNum > 0 Then
            DrawSkillItem(CurMouseX, CurMouseY)
        End If

        'draw cursor
        'DrawCursor()
    End Sub
#End Region

#Region "Support Functions"
    Function IsEqItem(ByVal X As Single, ByVal Y As Single) As Integer
        Dim tempRec As Rect
        Dim i As Integer
        IsEqItem = 0

        For i = 1 To EquipmentType.Count - 1

            If GetPlayerEquipment(MyIndex, i) > 0 And GetPlayerEquipment(MyIndex, i) <= MAX_ITEMS Then

                With tempRec
                    .Top = CharWindowY + EqTop + ((EqOffsetY + 32) * ((i - 1) \ EqColumns))
                    .Bottom = .Top + PIC_Y
                    .Left = CharWindowX + EqLeft + ((EqOffsetX + 32) * (((i - 1) Mod EqColumns)))
                    .Right = .Left + PIC_X
                End With

                If X >= tempRec.Left And X <= tempRec.Right Then
                    If Y >= tempRec.Top And Y <= tempRec.Bottom Then
                        IsEqItem = i
                        Exit Function
                    End If
                End If
            End If

        Next

    End Function

    Function IsInvItem(ByVal X As Single, ByVal Y As Single) As Integer
        Dim tempRec As Rect
        Dim i As Integer
        IsInvItem = 0

        For i = 1 To MAX_INV

            If GetPlayerInvItemNum(MyIndex, i) > 0 And GetPlayerInvItemNum(MyIndex, i) <= MAX_ITEMS Then

                With tempRec
                    .Top = InvWindowY + InvTop + ((InvOffsetY + 32) * ((i - 1) \ InvColumns))
                    .Bottom = .Top + PIC_Y
                    .Left = InvWindowX + InvLeft + ((InvOffsetX + 32) * (((i - 1) Mod InvColumns)))
                    .Right = .Left + PIC_X
                End With

                If X >= tempRec.Left And X <= tempRec.Right Then
                    If Y >= tempRec.Top And Y <= tempRec.Bottom Then
                        IsInvItem = i
                        Exit Function
                    End If
                End If
            End If

        Next

    End Function

    Function IsPlayerSkill(ByVal X As Single, ByVal Y As Single) As Integer
        Dim tempRec As Rect
        Dim i As Integer

        IsPlayerSkill = 0

        For i = 1 To MAX_PLAYER_SKILLS

            If PlayerSkills(i) > 0 And PlayerSkills(i) <= MAX_PLAYER_SKILLS Then

                With tempRec
                    .Top = SkillWindowY + SkillTop + ((SkillOffsetY + 32) * ((i - 1) \ SkillColumns))
                    .Bottom = .Top + PIC_Y
                    .Left = SkillWindowX + SkillLeft + ((SkillOffsetX + 32) * (((i - 1) Mod SkillColumns)))
                    .Right = .Left + PIC_X
                End With

                If X >= tempRec.Left And X <= tempRec.Right Then
                    If Y >= tempRec.Top And Y <= tempRec.Bottom Then
                        IsPlayerSkill = i
                        Exit Function
                    End If
                End If
            End If

        Next

    End Function

    Function IsBankItem(ByVal X As Single, ByVal Y As Single) As Integer
        Dim tempRec As Rect
        Dim i As Integer

        IsBankItem = 0

        For i = 1 To MAX_BANK
            If GetBankItemNum(i) > 0 And GetBankItemNum(i) <= MAX_ITEMS Then

                With tempRec
                    .Top = BankWindowY + BankTop + ((BankOffsetY + 32) * ((i - 1) \ BankColumns))
                    .Bottom = .Top + PIC_Y
                    .Left = BankWindowX + BankLeft + ((BankOffsetX + 32) * (((i - 1) Mod BankColumns)))
                    .Right = .Left + PIC_X
                End With

                If X >= tempRec.Left And X <= tempRec.Right Then
                    If Y >= tempRec.Top And Y <= tempRec.Bottom Then

                        IsBankItem = i
                        Exit Function
                    End If
                End If
            End If
        Next
    End Function

    Function IsShopItem(ByVal X As Single, ByVal Y As Single) As Integer
        Dim tempRec As Rectangle
        Dim i As Integer
        IsShopItem = 0

        For i = 1 To MAX_TRADES

            If Shop(InShop).TradeItem(i).Item > 0 And Shop(InShop).TradeItem(i).Item <= MAX_ITEMS Then
                With tempRec
                    .Y = ShopWindowY + ShopTop + ((ShopOffsetY + 32) * ((i - 1) \ ShopColumns))
                    .Height = PIC_Y
                    .X = ShopWindowX + ShopLeft + ((ShopOffsetX + 32) * (((i - 1) Mod ShopColumns)))
                    .Width = PIC_X
                End With

                If X >= tempRec.Left And X <= tempRec.Right Then
                    If Y >= tempRec.Top And Y <= tempRec.Bottom Then
                        IsShopItem = i
                        Exit Function
                    End If
                End If
            End If
        Next
    End Function

    Function IsTradeItem(ByVal X As Single, ByVal Y As Single, ByVal Yours As Boolean) As Integer
        Dim tempRec As Rect
        Dim i As Integer
        Dim itemnum As Integer

        IsTradeItem = 0

        For i = 1 To MAX_INV

            If Yours Then
                itemnum = GetPlayerInvItemNum(MyIndex, TradeYourOffer(i).Num)

                With tempRec
                    .Top = TradeWindowY + OurTradeY + InvTop + ((InvOffsetY + 32) * ((i - 1) \ InvColumns))
                    .Bottom = .Top + PIC_Y
                    .Left = TradeWindowX + OurTradeX + InvLeft + ((InvOffsetX + 32) * (((i - 1) Mod InvColumns)))
                    .Right = .Left + PIC_X
                End With
            Else
                itemnum = TradeTheirOffer(i).Num

                With tempRec
                    .Top = TradeWindowY + TheirTradeY + InvTop + ((InvOffsetY + 32) * ((i - 1) \ InvColumns))
                    .Bottom = .Top + PIC_Y
                    .Left = TradeWindowX + TheirTradeX + InvLeft + ((InvOffsetX + 32) * (((i - 1) Mod InvColumns)))
                    .Right = .Left + PIC_X
                End With
            End If

            If itemnum > 0 And itemnum <= MAX_ITEMS Then

                If X >= tempRec.Left And X <= tempRec.Right Then
                    If Y >= tempRec.Top And Y <= tempRec.Bottom Then
                        IsTradeItem = i
                        Exit Function
                    End If
                End If

            End If

        Next

    End Function

    Function AboveActionPanel(ByVal X As Single, ByVal Y As Single) As Boolean
        AboveActionPanel = False

        If X > ActionPanelX And X < ActionPanelX + ActionPanelGFXInfo.Width Then
            If Y > ActionPanelY And Y < ActionPanelY + ActionPanelGFXInfo.Height Then
                AboveActionPanel = True
            End If
        End If
    End Function

    Function AboveHotbar(ByVal X As Single, ByVal Y As Single) As Boolean
        AboveHotbar = False

        If X > HotbarX And X < HotbarX + HotBarGFXInfo.Width Then
            If Y > HotbarY And Y < HotbarY + HotBarGFXInfo.Height Then
                AboveHotbar = True
            End If
        End If
    End Function

    Function AbovePetbar(ByVal X As Single, ByVal Y As Single) As Boolean
        AbovePetbar = False

        If X > PetbarX And X < PetbarX + PetbarGFXInfo.Width Then
            If Y > PetbarY And Y < PetbarY + HotBarGFXInfo.Height Then
                AbovePetbar = True
            End If
        End If
    End Function

    Function AboveInvpanel(ByVal X As Single, ByVal Y As Single) As Boolean
        AboveInvpanel = False

        If X > InvWindowX And X < InvWindowX + InvPanelGFXInfo.Width Then
            If Y > InvWindowY And Y < InvWindowY + InvPanelGFXInfo.Height Then
                AboveInvpanel = True
            End If
        End If
    End Function

    Function AboveCharpanel(ByVal X As Single, ByVal Y As Single) As Boolean
        AboveCharpanel = False

        If X > CharWindowX And X < CharWindowX + CharPanelGFXInfo.Width Then
            If Y > CharWindowY And Y < CharWindowY + CharPanelGFXInfo.Height Then
                AboveCharpanel = True
            End If
        End If
    End Function

    Function AboveSkillpanel(ByVal X As Single, ByVal Y As Single) As Boolean
        AboveSkillpanel = False

        If X > SkillWindowX And X < SkillWindowX + SkillPanelGFXInfo.Width Then
            If Y > SkillWindowY And Y < SkillWindowY + SkillPanelGFXInfo.Height Then
                AboveSkillpanel = True
            End If
        End If
    End Function

    Function AboveBankpanel(ByVal X As Single, ByVal Y As Single) As Boolean
        AboveBankpanel = False

        If X > BankWindowX And X < BankWindowX + BankPanelGFXInfo.Width Then
            If Y > BankWindowY And Y < BankWindowY + BankPanelGFXInfo.Height Then
                AboveBankpanel = True
            End If
        End If
    End Function

    Function AboveShoppanel(ByVal X As Single, ByVal Y As Single) As Boolean
        AboveShoppanel = False

        If X > ShopWindowX And X < ShopWindowX + ShopPanelGFXInfo.Width Then
            If Y > ShopWindowY And Y < ShopWindowY + ShopPanelGFXInfo.Height Then
                AboveShoppanel = True
            End If
        End If
    End Function

    Function AboveTradepanel(ByVal X As Single, ByVal Y As Single) As Boolean
        AboveTradepanel = False

        If X > TradeWindowX And X < TradeWindowX + TradePanelGFXInfo.Width Then
            If Y > TradeWindowY And Y < TradeWindowY + TradePanelGFXInfo.Height Then
                AboveTradepanel = True
            End If
        End If
    End Function

    Function AboveEventChat(ByVal X As Single, ByVal Y As Single) As Boolean
        AboveEventChat = False

        If X > EventChatX And X < EventChatX + EventChatGFXInfo.Width Then
            If Y > EventChatY And Y < EventChatY + EventChatGFXInfo.Height Then
                AboveEventChat = True
            End If
        End If
    End Function

    Function AboveChatScrollUp(ByVal X As Single, ByVal Y As Single) As Boolean
        AboveChatScrollUp = False

        If X > ChatWindowX + ChatWindowGFXInfo.Width - 24 And X < ChatWindowX + ChatWindowGFXInfo.Width Then
            If Y > ChatWindowY And Y < ChatWindowY + 24 Then 'ChatWindowGFXInfo.height Then
                AboveChatScrollUp = True
            End If
        End If
    End Function

    Function AboveChatScrollDown(ByVal X As Single, ByVal Y As Single) As Boolean
        AboveChatScrollDown = False

        If X > ChatWindowX + ChatWindowGFXInfo.Width - 24 And X < ChatWindowX + ChatWindowGFXInfo.Width Then
            If Y > ChatWindowY + ChatWindowGFXInfo.Height - 24 And Y < ChatWindowY + ChatWindowGFXInfo.Height Then
                AboveChatScrollDown = True
            End If
        End If
    End Function

    Function AboveRClickPanel(ByVal X As Single, ByVal Y As Single) As Boolean
        AboveRClickPanel = False

        If X > RClickX And X < RClickX + RClickGFXInfo.Width Then
            If Y > RClickY And Y < RClickY + RClickGFXInfo.Height Then
                AboveRClickPanel = True
            End If
        End If
    End Function

    Function AboveQuestPanel(ByVal X As Single, ByVal Y As Single) As Boolean
        AboveQuestPanel = False

        If X > QuestLogX And X < QuestLogX + QuestGFXInfo.Width Then
            If Y > QuestLogY And Y < QuestLogY + QuestGFXInfo.Height Then
                AboveQuestPanel = True
            End If
        End If
    End Function

    Function AboveCraftPanel(ByVal X As Single, ByVal Y As Single) As Boolean
        AboveCraftPanel = False

        If X > CraftPanelX And X < CraftPanelX + CraftGFXInfo.Width Then
            If Y > CraftPanelY And Y < CraftPanelY + CraftGFXInfo.Height Then
                AboveCraftPanel = True
            End If
        End If
    End Function
#End Region

End Module