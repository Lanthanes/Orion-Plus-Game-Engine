﻿Imports System.Windows.Forms

Public Class frmEditor_Item
    Private Sub scrlPic_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlPic.ValueChanged, scrlPic.Scroll
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        lblPic.Text = "Pic: " & scrlPic.Value
        Item(EditorIndex).Pic = scrlPic.Value
        Call EditorItem_DrawItem()
    End Sub

    Private Sub cmbBind_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbBind.SelectedIndexChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        Item(EditorIndex).BindType = cmbBind.SelectedIndex
    End Sub

    Private Sub scrlRarity_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlRarity.ValueChanged, scrlRarity.Scroll
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        lblRarity.Text = "Rarity: " & scrlRarity.Value
        Item(EditorIndex).Rarity = scrlRarity.Value
    End Sub

    Private Sub scrlAnim_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlAnim.ValueChanged, scrlAnim.Scroll
        Dim sString As String
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        If scrlAnim.Value = 0 Then
            sString = "None"
        Else
            sString = Trim$(Animation(scrlAnim.Value).Name)
        End If
        lblAnim.Text = "Anim: " & sString
        Item(EditorIndex).Animation = scrlAnim.Value
    End Sub

    Private Sub cmbType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbType.SelectedIndexChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub

        If (cmbType.SelectedIndex >= ItemType.WEAPON) And (cmbType.SelectedIndex <= ItemType.GLOVES) Then
            fraEquipment.Visible = True
            'scrlDamage_Change
        Else
            fraEquipment.Visible = False
        End If

        If (cmbType.SelectedIndex >= ItemType.PotionHP) And (cmbType.SelectedIndex <= ItemType.PoisonSP) Then
            fraVitals.Visible = True
            'scrlVitalMod_Change
        Else
            fraVitals.Visible = False
        End If

        If (cmbType.SelectedIndex = ItemType.SKILL) Then
            fraSkill.Visible = True
        Else
            fraSkill.Visible = False
        End If

        If cmbType.SelectedIndex = ItemType.FURNITURE Then
            fraFurniture.Visible = True
        Else
            fraFurniture.Visible = False
        End If

        If cmbType.SelectedIndex = ItemType.Recipe Then
            fraRecipe.Visible = True
        Else
            fraRecipe.Visible = False
        End If

        Item(EditorIndex).Type = cmbType.SelectedIndex
    End Sub

    Private Sub cmbClassReq_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbClassReq.SelectedIndexChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        Item(EditorIndex).ClassReq = cmbClassReq.SelectedIndex
    End Sub

    Private Sub scrlAccessReq_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlAccessReq.ValueChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        lblAccessReq.Text = "Access Req: " & scrlAccessReq.Value
        Item(EditorIndex).AccessReq = scrlAccessReq.Value
    End Sub

    Private Sub scrlLevelReq_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlLevelReq.ValueChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        lblLevelReq.Text = "Level req: " & scrlLevelReq.Value
        Item(EditorIndex).LevelReq = scrlLevelReq.Value
    End Sub

    Private Sub scrlStrReq_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlStrReq.ValueChanged
        lblStrReq.Text = "Str: " & scrlStrReq.Value
        Item(EditorIndex).Stat_Req(Stats.strength) = scrlStrReq.Value
    End Sub

    Private Sub scrlEndReq_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlEndReq.ValueChanged
        lblEndReq.Text = "End: " & scrlEndReq.Value
        Item(EditorIndex).Stat_Req(Stats.endurance) = scrlEndReq.Value
    End Sub

    Private Sub scrlVitReq_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlVitReq.ValueChanged
        lblVitReq.Text = "Vit: " & scrlVitReq.Value
        Item(EditorIndex).Stat_Req(Stats.vitality) = scrlVitReq.Value
    End Sub

    Private Sub scrlWillReq_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlLuckReq.ValueChanged
        lblLuckReq.Text = "Will: " & scrlLuckReq.Value
        Item(EditorIndex).Stat_Req(Stats.luck) = scrlLuckReq.Value
    End Sub

    Private Sub scrlIntReq_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlIntReq.ValueChanged
        lblIntReq.Text = "Int: " & scrlStrReq.Value
        Item(EditorIndex).Stat_Req(Stats.intelligence) = scrlIntReq.Value
    End Sub

    Private Sub scrlSprReq_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlSprReq.ValueChanged
        lblSprReq.Text = "Spr: " & scrlSprReq.Value
        Item(EditorIndex).Stat_Req(Stats.spirit) = scrlSprReq.Value
    End Sub

    Private Sub scrlVitalMod_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlVitalMod.ValueChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        lblVitalMod.Text = "Vital Mod: " & scrlVitalMod.Value
        Item(EditorIndex).Data1 = scrlVitalMod.Value
    End Sub

    Private Sub scrlSkill_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlSkill.ValueChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub

        If Len(Trim$(Skill(scrlSkill.Value).Name)) > 0 Then
            lblSkillName.Text = "Name: " & Trim$(Skill(scrlSkill.Value).Name)
        Else
            lblSkillName.Text = "Name: None"
        End If

        lblSkill.Text = "Skill: " & scrlSkill.Value

        Item(EditorIndex).Data1 = scrlSkill.Value
    End Sub

    Private Sub cmbTool_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbTool.SelectedIndexChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        Item(EditorIndex).Data3 = cmbTool.SelectedIndex
    End Sub

    Private Sub scrlDamage_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlDamage.ValueChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        lblDamage.Text = "Damage: " & scrlDamage.Value
        Item(EditorIndex).Data2 = scrlDamage.Value
    End Sub

    Private Sub scrlSpeed_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlSpeed.ValueChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        lblSpeed.Text = "Speed: " & scrlSpeed.Value / 1000 & " sec"
        Item(EditorIndex).Speed = scrlSpeed.Value
    End Sub

    Private Sub scrlPaperdoll_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlPaperdoll.ValueChanged, scrlPaperdoll.Scroll
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        lblPaperDoll.Text = "Paperdoll: " & scrlPaperdoll.Value
        Item(EditorIndex).Paperdoll = scrlPaperdoll.Value
        EditorItem_DrawPaperdoll()
    End Sub

    Private Sub scrlAddStr_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlAddStr.ValueChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        lblAddStr.Text = "+Str: " & scrlAddStr.Value
        Item(EditorIndex).Add_Stat(Stats.strength) = scrlAddStr.Value
    End Sub

    Private Sub scrlAddWill_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlAddLuck.ValueChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        lblAddLuck.Text = "+Will: " & scrlAddLuck.Value
        Item(EditorIndex).Add_Stat(Stats.luck) = scrlAddLuck.Value
    End Sub

    Private Sub scrlAddEnd_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlAddEnd.ValueChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        lblAddEnd.Text = "+End: " & scrlAddEnd.Value
        Item(EditorIndex).Add_Stat(Stats.endurance) = scrlAddEnd.Value
    End Sub

    Private Sub scrlAddInt_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlAddInt.ValueChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        lblAddInt.Text = "+Int: " & scrlAddInt.Value
        Item(EditorIndex).Add_Stat(Stats.intelligence) = scrlAddInt.Value
    End Sub

    Private Sub scrlAddVit_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlAddVit.ValueChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        lblAddVit.Text = "+Vit: " & scrlAddVit.Value
        Item(EditorIndex).Add_Stat(Stats.vitality) = scrlAddVit.Value
    End Sub

    Private Sub scrlAddSpr_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlAddSpr.ValueChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        lblAddSpr.Text = "+Spr: " & scrlAddSpr.Value
        Item(EditorIndex).Add_Stat(Stats.spirit) = scrlAddSpr.Value
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        ItemEditorOk()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        ItemEditorCancel()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Dim tmpIndex As Integer

        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub

        ClearItem(EditorIndex + 1)

        tmpIndex = lstIndex.SelectedIndex
        lstIndex.Items.RemoveAt(EditorIndex - 1)
        lstIndex.Items.Insert(EditorIndex - 1, EditorIndex & ": " & Item(EditorIndex).Name)
        lstIndex.SelectedIndex = tmpIndex

        ItemEditorInit()
    End Sub

    Private Sub txtName_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtName.TextChanged
        Dim tmpIndex As Integer
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        tmpIndex = lstIndex.SelectedIndex
        Item(EditorIndex).Name = Trim$(txtName.Text)
        lstIndex.Items.RemoveAt(EditorIndex - 1)
        lstIndex.Items.Insert(EditorIndex - 1, EditorIndex & ": " & Item(EditorIndex).Name)
        lstIndex.SelectedIndex = tmpIndex
    End Sub

    Private Sub lstIndex_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lstIndex.Click
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        ItemEditorInit()
    End Sub

    Private Sub scrlPrice_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles scrlPrice.ValueChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        lblPrice.Text = "Price: " & scrlPrice.Value
        Item(EditorIndex).Price = scrlPrice.Value
    End Sub

    Private Sub picItem_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles picItem.Paint
        'Dont let it auto paint ;)
    End Sub

    Private Sub picPaperdoll_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles picPaperdoll.Paint
        'Dont let it auto paint :0
    End Sub

    Private Sub tmrDrawPicPaperdoll_Tick(ByVal sender As Object, ByVal e As EventArgs)
        If Me.Visible = True Then
            If Me.WindowState = FormWindowState.Normal Then
                EditorItem_DrawItem()
                EditorItem_DrawPaperdoll()
                EditorItem_DrawFurniture()
            End If
        End If
    End Sub

    Private Sub frmEditor_Item_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        scrlPic.Maximum = NumItems
        scrlPic.LargeChange = 1
        scrlAnim.Maximum = MAX_ANIMATIONS
        scrlPaperdoll.Maximum = NumPaperdolls
        scrlFurniture.Maximum = NumFurniture
        cmbFurnitureType.SelectedIndex = 0
    End Sub

    Private Sub scrlPrice_Scroll(ByVal sender As Object, ByVal e As ScrollEventArgs) Handles scrlPrice.Scroll
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        lblPrice.Text = "Price: " & scrlPrice.Value
        Item(EditorIndex).Price = scrlPrice.Value
    End Sub

    Private Sub cmbFurnitureType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFurnitureType.SelectedIndexChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        Item(EditorIndex).Data1 = cmbFurnitureType.SelectedIndex
    End Sub

    Private Sub optNoFurnitureEditing_CheckedChanged(sender As Object, e As EventArgs) Handles optNoFurnitureEditing.CheckedChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        If optNoFurnitureEditing.Checked = True Then
            lblSetOption.Text = "No Editing: Lets you look at the image without setting any options (blocks/fringes)."
        End If
    End Sub

    Private Sub optSetBlocks_CheckedChanged(sender As Object, e As EventArgs) Handles optSetBlocks.CheckedChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        If optSetBlocks.Checked = True Then
            lblSetOption.Text = "Set Blocks: Os are passable and Xs are not. Simply place Xs where you do not want the player to walk."
        End If
    End Sub

    Private Sub optSetFringe_CheckedChanged(sender As Object, e As EventArgs) Handles optSetFringe.CheckedChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        If optSetFringe.Checked = True Then
            lblSetOption.Text = "Set Fringe: Os are draw on the fringe layer (the player can walk behind)."
        End If
    End Sub

    Private Sub scrlFurniture_Scroll(sender As Object, e As ScrollEventArgs) Handles scrlFurniture.Scroll
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        lblFurniture.Text = "Furniture: " & scrlFurniture.Value
        Item(EditorIndex).Data2 = scrlFurniture.Value

        If scrlFurniture.Value > 0 And scrlFurniture.Value <= NumFurniture Then
            Item(EditorIndex).FurnitureWidth = FurnitureGFXInfo(scrlFurniture.Value).width / 32
            Item(EditorIndex).FurnitureHeight = FurnitureGFXInfo(scrlFurniture.Value).height / 32
            If Item(EditorIndex).FurnitureHeight > 1 Then Item(EditorIndex).FurnitureHeight = Item(EditorIndex).FurnitureHeight - 1
        Else
            Item(EditorIndex).FurnitureWidth = 1
            Item(EditorIndex).FurnitureHeight = 1
        End If

        EditorItem_DrawFurniture()
    End Sub

    Private Sub picFurniture_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles picFurniture.MouseDown
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        Dim X As Integer, Y As Integer
        X = e.X / 32
        Y = e.Y / 32

        If X > 3 Then Exit Sub
        If Y > 3 Then Exit Sub

        If optSetBlocks.Checked = True Then
            If Item(EditorIndex).FurnitureBlocks(X, Y) = 1 Then
                Item(EditorIndex).FurnitureBlocks(X, Y) = 0
            Else
                Item(EditorIndex).FurnitureBlocks(X, Y) = 1
            End If
        ElseIf optSetFringe.Checked = True Then
            If Item(EditorIndex).FurnitureFringe(X, Y) = 1 Then
                Item(EditorIndex).FurnitureFringe(X, Y) = 0
            Else
                Item(EditorIndex).FurnitureFringe(X, Y) = 1
            End If
        End If
        EditorItem_DrawFurniture()
    End Sub

    Private Sub picFurniture_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles picFurniture.Paint
        'Dont let it auto paint ;)
    End Sub

    Private Sub scrlProjectile_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles scrlProjectile.ValueChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        If scrlProjectile.Value = 0 Then
            lblProjectile.Text = "Projectile: 0 None"
        Else
            lblProjectile.Text = "Projectile: " & scrlProjectile.Value & " " & Trim$(Projectiles(scrlProjectile.Value).Name)
        End If
        Item(EditorIndex).Data1 = scrlProjectile.Value
    End Sub

    Private Sub chkKnockBack_CheckedChanged(sender As Object, e As EventArgs) Handles chkKnockBack.CheckedChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub

        If chkKnockBack.Checked = True Then
            Item(EditorIndex).KnockBack = 1
        Else
            Item(EditorIndex).KnockBack = 0
        End If
    End Sub

    Private Sub cmbKnockBackTiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbKnockBackTiles.SelectedIndexChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub

        Item(EditorIndex).KnockBackTiles = cmbKnockBackTiles.SelectedIndex
    End Sub

    Private Sub chkRandomize_CheckedChanged(sender As Object, e As EventArgs) Handles chkRandomize.CheckedChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub

        If chkRandomize.Checked = True Then
            Item(EditorIndex).Randomize = 1
        Else
            Item(EditorIndex).Randomize = 0
        End If
    End Sub

    Private Sub chkStackable_CheckedChanged(sender As Object, e As EventArgs) Handles chkStackable.CheckedChanged
        If chkStackable.Checked = True Then
            Item(EditorIndex).Stackable = 1
        Else
            Item(EditorIndex).Stackable = 0
        End If
    End Sub

    Private Sub scrlRecipe_Scroll(sender As Object, e As ScrollEventArgs) Handles scrlRecipe.Scroll
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub
        If scrlRecipe.Value = 0 Then
            lblRecipename.Text = "Recipe: 0 None"
        Else
            lblRecipename.Text = "Recipe: " & scrlRecipe.Value & " " & Trim$(Recipe(scrlRecipe.Value).Name)
        End If
        Item(EditorIndex).Data1 = scrlRecipe.Value
    End Sub

    Private Sub txtDescription_TextChanged(sender As Object, e As EventArgs) Handles txtDescription.TextChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub

        Item(EditorIndex).Description = Trim$(txtDescription.Text)
    End Sub

    Private Sub numMin_ValueChanged(sender As Object, e As EventArgs) Handles numMin.ValueChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub

        Item(EditorIndex).RandomMin = numMin.Value
    End Sub

    Private Sub numMax_ValueChanged(sender As Object, e As EventArgs) Handles numMax.ValueChanged
        If EditorIndex = 0 Or EditorIndex > MAX_ITEMS Then Exit Sub

        Item(EditorIndex).RandomMax = numMax.Value
    End Sub

#Region "Item Editor"
    Public Sub ItemEditorPreInit()
        Dim i As Integer

        With frmEditor_Item
            Editor = EDITOR_ITEM
            .lstIndex.Items.Clear()

            ' Add the names
            For i = 1 To MAX_ITEMS
                .lstIndex.Items.Add(i & ": " & Trim$(Item(i).Name))
            Next

            .Show()
            .lstIndex.SelectedIndex = 0
            ItemEditorInit()
        End With
    End Sub

    Public Sub ItemEditorInit()
        Dim i As Integer

        If frmEditor_Item.Visible = False Then Exit Sub
        EditorIndex = frmEditor_Item.lstIndex.SelectedIndex + 1

        With Item(EditorIndex)
            frmEditor_Item.txtName.Text = Trim$(.Name)
            frmEditor_Item.txtDescription.Text = Trim$(.Description)

            If .Pic > frmEditor_Item.scrlPic.Maximum Then .Pic = 0
            frmEditor_Item.scrlPic.Value = .Pic
            frmEditor_Item.cmbType.SelectedIndex = .Type
            frmEditor_Item.scrlAnim.Value = .Animation

            ' Type specific settings
            If (frmEditor_Item.cmbType.SelectedIndex >= ItemType.Weapon) And (frmEditor_Item.cmbType.SelectedIndex <= ItemType.Gloves) Then
                frmEditor_Item.fraEquipment.Visible = True
                frmEditor_Item.scrlProjectile.Value = .Data1
                frmEditor_Item.scrlDamage.Value = .Data2
                frmEditor_Item.cmbTool.SelectedIndex = .Data3

                If .Speed < 100 Then .Speed = 100
                If .Speed > frmEditor_Item.scrlSpeed.Maximum Then .Speed = frmEditor_Item.scrlSpeed.Maximum
                frmEditor_Item.scrlSpeed.Value = .Speed

                frmEditor_Item.scrlAddStr.Value = .Add_Stat(Stats.strength)
                frmEditor_Item.scrlAddEnd.Value = .Add_Stat(Stats.endurance)
                frmEditor_Item.scrlAddInt.Value = .Add_Stat(Stats.intelligence)
                frmEditor_Item.scrlAddVit.Value = .Add_Stat(Stats.vitality)
                frmEditor_Item.scrlAddLuck.Value = .Add_Stat(Stats.luck)
                frmEditor_Item.scrlAddSpr.Value = .Add_Stat(Stats.spirit)

                If .KnockBack = 1 Then
                    frmEditor_Item.chkKnockBack.Checked = True
                Else
                    frmEditor_Item.chkKnockBack.Checked = False
                End If
                frmEditor_Item.cmbKnockBackTiles.SelectedIndex = .KnockBackTiles

                If .Randomize = 1 Then
                    frmEditor_Item.chkRandomize.Checked = True
                Else
                    frmEditor_Item.chkRandomize.Checked = False
                End If

                If .RandomMin = 0 Then .RandomMin = 1
                frmEditor_Item.numMin.Value = .RandomMin

                If .RandomMax <= 1 Then .RandomMax = 2
                frmEditor_Item.numMax.Value = .RandomMax

                frmEditor_Item.scrlPaperdoll.Value = .Paperdoll
            Else
                frmEditor_Item.fraEquipment.Visible = False
            End If

            If (frmEditor_Item.cmbType.SelectedIndex >= ItemType.PotionHp) And (frmEditor_Item.cmbType.SelectedIndex <= ItemType.PoisonSp) Then
                frmEditor_Item.fraVitals.Visible = True
                frmEditor_Item.scrlVitalMod.Value = .Data1
            Else
                frmEditor_Item.fraVitals.Visible = False
            End If

            If (frmEditor_Item.cmbType.SelectedIndex = ItemType.Skill) Then
                frmEditor_Item.fraSkill.Visible = True
                frmEditor_Item.scrlSkill.Value = .Data1
            Else
                frmEditor_Item.fraSkill.Visible = False
            End If

            If frmEditor_Item.cmbType.SelectedIndex = ItemType.Furniture Then
                frmEditor_Item.fraFurniture.Visible = True
                If Item(EditorIndex).Data2 > 0 And Item(EditorIndex).Data2 <= NumFurniture Then
                    frmEditor_Item.scrlFurniture.Value = Item(EditorIndex).Data2
                Else
                    frmEditor_Item.scrlFurniture.Value = 1
                End If
                frmEditor_Item.cmbFurnitureType.SelectedIndex = Item(EditorIndex).Data1
            Else
                frmEditor_Item.fraFurniture.Visible = False
            End If

            ' Basic requirements
            frmEditor_Item.scrlAccessReq.Value = .AccessReq
            frmEditor_Item.scrlLevelReq.Value = .LevelReq

            frmEditor_Item.scrlStrReq.Value = .Stat_Req(Stats.strength)
            frmEditor_Item.scrlVitReq.Value = .Stat_Req(Stats.vitality)
            frmEditor_Item.scrlLuckReq.Value = .Stat_Req(Stats.luck)
            frmEditor_Item.scrlEndReq.Value = .Stat_Req(Stats.endurance)
            frmEditor_Item.scrlIntReq.Value = .Stat_Req(Stats.intelligence)
            frmEditor_Item.scrlSprReq.Value = .Stat_Req(Stats.spirit)

            ' Build cmbClassReq
            frmEditor_Item.cmbClassReq.Items.Clear()
            frmEditor_Item.cmbClassReq.Items.Add("None")

            For i = 1 To Max_Classes
                frmEditor_Item.cmbClassReq.Items.Add(Classes(i).Name)
            Next

            frmEditor_Item.cmbClassReq.SelectedIndex = .ClassReq
            ' Info
            frmEditor_Item.scrlPrice.Value = .Price
            frmEditor_Item.cmbBind.SelectedIndex = .BindType
            frmEditor_Item.scrlRarity.Value = .Rarity

            If .Stackable = 1 Then
                frmEditor_Item.chkStackable.Checked = True
            Else
                frmEditor_Item.chkStackable.Checked = False
            End If

            EditorIndex = frmEditor_Item.lstIndex.SelectedIndex + 1
        End With

        frmEditor_Item.scrlPic.Maximum = NumItems
        frmEditor_Item.scrlAnim.Maximum = MAX_ANIMATIONS

        If NumPaperdolls > 0 Then
            frmEditor_Item.scrlPaperdoll.Maximum = NumPaperdolls + 1
        End If


        EditorItem_DrawItem()
        EditorItem_DrawPaperdoll()
        EditorItem_DrawFurniture()
        Item_Changed(EditorIndex) = True

    End Sub

    Public Sub ItemEditorCancel()
        Editor = 0
        frmEditor_Item.Visible = False
        ClearChanged_Item()
        ClearItems()
        SendRequestItems()
    End Sub

    Public Sub ItemEditorOk()
        Dim i As Integer

        For i = 1 To MAX_ITEMS
            If Item_Changed(i) Then
                SendSaveItem(i)
            End If
        Next

        frmEditor_Item.Visible = False
        Editor = 0
        ClearChanged_Item()
    End Sub
#End Region
End Class