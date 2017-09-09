
Imports ASFW

Public Module EditorProjectiles
#Region "Defines"
    Public Const MAX_PROJECTILES As Integer = 255
    Public Projectiles(0 To MAX_PROJECTILES) As ProjectileRec
    Public MapProjectiles(0 To MAX_PROJECTILES) As MapProjectileRec
    Public NumProjectiles As Integer
    Public InitProjectileEditor As Boolean
    Public Const EDITOR_PROJECTILE As Byte = 10
    Public Projectile_Changed(0 To MAX_PROJECTILES) As Boolean
#End Region

#Region "Types"
    Public Structure ProjectileRec
        Dim Name As String
        Dim Sprite As Integer
        Dim Range As Byte
        Dim Speed As Integer
        Dim Damage As Integer
    End Structure

    Public Structure MapProjectileRec
        Dim ProjectileNum As Integer
        Dim Owner As Integer
        Dim OwnerType As Byte
        Dim X As Integer
        Dim Y As Integer
        Dim dir As Byte
        Dim Range As Integer
        Dim TravelTime As Integer
        Dim Timer As Integer
    End Structure
#End Region

#Region "Sending"
    Sub SendRequestEditProjectiles()
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(EditorPackets.RequestEditProjectiles)
        SendData(Buffer.ToArray())
        Buffer.Dispose()

    End Sub

    Sub SendSaveProjectile(ByVal ProjectileNum As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)

        Buffer.WriteInt32(EditorPackets.SaveProjectile)
        Buffer.WriteInt32(ProjectileNum)

        Buffer.WriteString(Trim(Projectiles(ProjectileNum).Name))
        Buffer.WriteInt32(Projectiles(ProjectileNum).Sprite)
        Buffer.WriteInt32(Projectiles(ProjectileNum).Range)
        Buffer.WriteInt32(Projectiles(ProjectileNum).Speed)
        Buffer.WriteInt32(Projectiles(ProjectileNum).Damage)

        SendData(Buffer.ToArray())
        Buffer.Dispose()

    End Sub

    Sub SendRequestProjectiles()
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ClientPackets.CRequestProjectiles)
        SendData(Buffer.ToArray())
        Buffer.Dispose()

    End Sub

    Sub SendClearProjectile(ByVal ProjectileNum As Integer, ByVal CollisionIndex As Integer, ByVal CollisionType As Byte, ByVal CollisionZone As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ClientPackets.CClearProjectile)
        Buffer.WriteInt32(ProjectileNum)
        Buffer.WriteInt32(CollisionIndex)
        Buffer.WriteInt32(CollisionType)
        Buffer.WriteInt32(CollisionZone)
        SendData(Buffer.ToArray())
        Buffer.Dispose()

    End Sub

#End Region

#Region "Recieving"
    Public Sub HandleProjectileEditor(ByVal data() As Byte)

        InitProjectileEditor = True

    End Sub

    Public Sub HandleUpdateProjectile(ByVal data() As Byte)
        Dim ProjectileNum As Integer
        Dim Buffer As New ByteStream(data)
        ProjectileNum = Buffer.ReadInt32

        Projectiles(ProjectileNum).Name = Buffer.ReadString
        Projectiles(ProjectileNum).Sprite = Buffer.ReadInt32
        Projectiles(ProjectileNum).Range = Buffer.ReadInt32
        Projectiles(ProjectileNum).Speed = Buffer.ReadInt32
        Projectiles(ProjectileNum).Damage = Buffer.ReadInt32

        Buffer.Dispose()

    End Sub

    Public Sub HandleMapProjectile(ByVal data() As Byte)
        Dim i As Integer
        Dim Buffer As New ByteStream(data)
        i = Buffer.ReadInt32

        With MapProjectiles(i)
            .ProjectileNum = Buffer.ReadInt32
            .Owner = Buffer.ReadInt32
            .OwnerType = Buffer.ReadInt32
            .dir = Buffer.ReadInt32
            .X = Buffer.ReadInt32
            .Y = Buffer.ReadInt32
            .Range = 0
            .Timer = GetTickCount() + 60000
        End With

        Buffer.Dispose()

    End Sub

#End Region

#Region "Database"
    Sub ClearProjectiles()
        Dim i As Integer

        For i = 1 To MAX_PROJECTILES
            Call ClearProjectile(i)
        Next

    End Sub

    Sub ClearProjectile(ByVal Index As Integer)

        Projectiles(Index).Name = ""
        Projectiles(Index).Sprite = 0
        Projectiles(Index).Range = 0
        Projectiles(Index).Speed = 0
        Projectiles(Index).Damage = 0

    End Sub

    Sub ClearMapProjectile(ByVal ProjectileNum As Integer)

        MapProjectiles(ProjectileNum).ProjectileNum = 0
        MapProjectiles(ProjectileNum).Owner = 0
        MapProjectiles(ProjectileNum).OwnerType = 0
        MapProjectiles(ProjectileNum).X = 0
        MapProjectiles(ProjectileNum).Y = 0
        MapProjectiles(ProjectileNum).dir = 0
        MapProjectiles(ProjectileNum).Timer = 0

    End Sub

#End Region

#Region "Drawing"

    Public Sub CheckProjectiles()
        Dim i As Integer

        i = 1

        While FileExist(Application.StartupPath & GFX_PATH & "projectiles\" & i & GFX_EXT)

            NumProjectiles = NumProjectiles + 1
            i = i + 1
        End While

        If NumProjectiles = 0 Then Exit Sub

    End Sub

    Public Sub EditorProjectile_DrawProjectile()
        Dim iconnum As Integer

        iconnum = frmEditor_Projectile.nudPic.Value

        If iconnum < 1 Or iconnum > NumProjectiles Then
            frmEditor_Projectile.picProjectile.BackgroundImage = Nothing
            Exit Sub
        End If

        If FileExist(Application.StartupPath & GFX_PATH & "Projectiles\" & iconnum & GFX_EXT) Then
            frmEditor_Projectile.picProjectile.BackgroundImage = Image.FromFile(Application.StartupPath & GFX_PATH & "Projectiles\" & iconnum & GFX_EXT)
        End If

    End Sub

#End Region

#Region "Projectile Editor"

    Public Sub ProjectileEditorInit()

        If frmEditor_Projectile.Visible = False Then Exit Sub
        EditorIndex = frmEditor_Projectile.lstIndex.SelectedIndex + 1

        With Projectiles(EditorIndex)
            frmEditor_Projectile.txtName.Text = Trim$(.Name)
            frmEditor_Projectile.nudPic.Value = .Sprite
            frmEditor_Projectile.nudRange.Value = .Range
            frmEditor_Projectile.nudSpeed.Value = .Speed
            frmEditor_Projectile.nudDamage.Value = .Damage
        End With

        Projectile_Changed(EditorIndex) = True

    End Sub

    Public Sub ProjectileEditorOk()
        Dim i As Integer

        For i = 1 To MAX_PROJECTILES
            If Projectile_Changed(i) Then
                Call SendSaveProjectile(i)
            End If
        Next

        frmEditor_Projectile.Dispose()
        Editor = 0
        ClearChanged_Projectile()

    End Sub

    Public Sub ProjectileEditorCancel()

        Editor = 0
        frmEditor_Projectile.Dispose()
        ClearChanged_Projectile()
        ClearProjectiles()
        SendRequestProjectiles()

    End Sub

    Public Sub ClearChanged_Projectile()
        Dim i As Integer

        For i = 0 To MAX_PROJECTILES
            Projectile_Changed(i) = False
        Next

    End Sub

#End Region

End Module
