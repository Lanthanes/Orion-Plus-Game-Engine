Imports System.Drawing

Module ClientBank
#Region "Globals"
    Public Bank As BankRec
#End Region

#Region "Drawing"
    Sub DrawBank()
        Dim i As Integer, X As Integer, Y As Integer, itemnum As Integer
        Dim Amount As String
        Dim sRECT As Rectangle, dRECT As Rectangle
        Dim Sprite As Integer, colour As SFML.Graphics.Color

        'first render panel
        RenderSprite(BankPanelSprite, GameWindow, BankWindowX, BankWindowY, 0, 0, BankPanelGFXInfo.Width, BankPanelGFXInfo.Height)

        'Headertext
        DrawText(BankWindowX + 140, BankWindowY + 6, "Your Bank", SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow, 15)

        'close
        DrawText(BankWindowX + 140, BankWindowY + BankPanelGFXInfo.Height - 20, "Close Bank", SFML.Graphics.Color.White, SFML.Graphics.Color.Black, GameWindow, 15)

        For i = 1 To MAX_BANK
            itemnum = GetBankItemNum(i)
            If itemnum > 0 And itemnum <= MAX_ITEMS Then

                Sprite = Item(itemnum).Pic

                If ItemsGFXInfo(Sprite).IsLoaded = False Then
                    LoadTexture(Sprite, 4)
                End If

                'seeying we still use it, lets update timer
                With ItemsGFXInfo(Sprite)
                    .TextureTimer = GetTimeMs() + 100000
                End With

                With sRECT
                    .Y = 0
                    .Height = PIC_Y
                    .X = 0
                    .Width = PIC_X
                End With

                With dRECT
                    .Y = BankWindowY + BankTop + ((BankOffsetY + 32) * ((i - 1) \ BankColumns))
                    .Height = PIC_Y
                    .X = BankWindowX + BankLeft + ((BankOffsetX + 32) * (((i - 1) Mod BankColumns)))
                    .Width = PIC_X
                End With

                RenderSprite(ItemsSprite(Sprite), GameWindow, dRECT.X, dRECT.Y, sRECT.X, sRECT.Y, sRECT.Width, sRECT.Height)

                ' If item is a stack - draw the amount you have
                If GetBankItemValue(i) > 1 Then
                    Y = dRECT.Top + 22
                    X = dRECT.Left - 4

                    Amount = GetBankItemValue(i)
                    colour = SFML.Graphics.Color.White
                    ' Draw currency but with k, m, b etc. using a convertion function
                    If CLng(Amount) < 1000000 Then
                        colour = SFML.Graphics.Color.White
                    ElseIf CLng(Amount) > 1000000 And CLng(Amount) < 10000000 Then
                        colour = SFML.Graphics.Color.Yellow
                    ElseIf CLng(Amount) > 10000000 Then
                        colour = SFML.Graphics.Color.Green
                    End If

                    DrawText(X, Y, ConvertCurrency(Amount), colour, SFML.Graphics.Color.Black, GameWindow)
                End If
            End If
        Next

    End Sub

    Public Sub DrawBankItem(X As Integer, Y As Integer)
        Dim rec As Rectangle

        Dim itemnum As Integer
        Dim Sprite As Integer

        itemnum = GetBankItemNum(DragBankSlotNum)
        Sprite = Item(GetBankItemNum(DragBankSlotNum)).Pic

        If itemnum > 0 And itemnum <= MAX_ITEMS Then

            If ItemsGFXInfo(Sprite).IsLoaded = False Then
                LoadTexture(Sprite, 4)
            End If

            'seeying we still use it, lets update timer
            With ItemsGFXInfo(Sprite)
                .TextureTimer = GetTimeMs() + 100000
            End With

            With rec
                .Y = 0
                .Height = PIC_Y
                .X = 0
                .Width = PIC_X
            End With
        End If

        RenderSprite(ItemsSprite(Sprite), GameWindow, X + 16, Y + 16, rec.X, rec.Y, rec.Width, rec.Height)

    End Sub
#End Region

#Region "Incoming Packets"
    Sub Packet_OpenBank(Data() As Byte)
        Dim i As Integer, x As Integer
        Dim Buffer As New ByteBuffer

        Buffer.WriteBytes(Data)

        If Buffer.ReadInteger <> ServerPackets.SBank Then Exit Sub

        For i = 1 To MAX_BANK
            Bank.Item(i).Num = Buffer.ReadInteger
            Bank.Item(i).Value = Buffer.ReadInteger

            Bank.ItemRand(i).Prefix = Buffer.ReadString
            Bank.ItemRand(i).Suffix = Buffer.ReadString
            Bank.ItemRand(i).Rarity = Buffer.ReadInteger
            Bank.ItemRand(i).Damage = Buffer.ReadInteger
            Bank.ItemRand(i).Speed = Buffer.ReadInteger

            For x = 1 To Stats.Count - 1
                Bank.ItemRand(i).Stat(x) = Buffer.ReadInteger
            Next
        Next

        NeedToOpenBank = True

        Buffer = Nothing
    End Sub

#End Region

#Region "Outgoing Packets"
    Public Sub DepositItem(Invslot As Integer, Amount As Integer)
        Dim Buffer As New ByteBuffer

        Buffer.WriteInteger(ClientPackets.CDepositItem)
        Buffer.WriteInteger(Invslot)
        Buffer.WriteInteger(Amount)

        SendData(Buffer.ToArray())
        Buffer = Nothing
    End Sub

    Public Sub WithdrawItem(Bankslot As Integer, Amount As Integer)
        Dim Buffer As New ByteBuffer

        Buffer.WriteInteger(ClientPackets.CWithdrawItem)
        Buffer.WriteInteger(Bankslot)
        Buffer.WriteInteger(Amount)

        SendData(Buffer.ToArray())
        Buffer = Nothing
    End Sub

    Public Sub ChangeBankSlots(OldSlot As Integer, NewSlot As Integer)
        Dim Buffer As New ByteBuffer

        Buffer.WriteInteger(ClientPackets.CChangeBankSlots)
        Buffer.WriteInteger(OldSlot)
        Buffer.WriteInteger(NewSlot)

        SendData(Buffer.ToArray())
        Buffer = Nothing
    End Sub

    Public Sub CloseBank()
        Dim Buffer As New ByteBuffer

        Buffer.WriteInteger(ClientPackets.CCloseBank)

        SendData(Buffer.ToArray())
        Buffer = Nothing

        InBank = False
        pnlBankVisible = False
    End Sub
#End Region

End Module
