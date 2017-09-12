Imports System.IO
Imports System.Windows.Forms
Imports ASFW
Imports SFML.Graphics
Imports SFML.Window

Friend Module modProjectiles
#Region "Defines"
    Friend Const MAX_PROJECTILES As Integer = 255
    Friend Projectiles(MAX_PROJECTILES) As ProjectileRec
    Friend MapProjectiles(MAX_PROJECTILES) As MapProjectileRec
    Friend NumProjectiles As Integer
    Friend InitProjectileEditor As Boolean
    Friend Const EDITOR_PROJECTILE As Byte = 10
    Friend Projectile_Changed(MAX_PROJECTILES) As Boolean
#End Region

#Region "Types"
    Friend Structure ProjectileRec
        Dim Name As String
        Dim Sprite As Integer
        Dim Range As Byte
        Dim Speed As Integer
        Dim Damage As Integer
    End Structure

    Friend Structure MapProjectileRec
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

    Sub SendRequestProjectiles()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CRequestProjectiles)

        Socket.SendData(Buffer.Data, Buffer.Head)
        Buffer.Dispose()

    End Sub

    Sub SendClearProjectile(ProjectileNum As Integer, CollisionIndex As Integer, CollisionType As Byte, CollisionZone As Integer)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CClearProjectile)
        Buffer.WriteInt32(ProjectileNum)
        Buffer.WriteInt32(CollisionIndex)
        Buffer.WriteInt32(CollisionType)
        Buffer.WriteInt32(CollisionZone)

        Socket.SendData(Buffer.Data, Buffer.Head)
        Buffer.Dispose()

    End Sub

#End Region

#Region "Recieving"

    Friend Sub HandleUpdateProjectile(ByRef Data() As Byte)
        Dim ProjectileNum As Integer
        Dim Buffer As New ByteStream(Data)
        ProjectileNum = Buffer.ReadInt32

        Projectiles(ProjectileNum).Name = Buffer.ReadString
        Projectiles(ProjectileNum).Sprite = Buffer.ReadInt32
        Projectiles(ProjectileNum).Range = Buffer.ReadInt32
        Projectiles(ProjectileNum).Speed = Buffer.ReadInt32
        Projectiles(ProjectileNum).Damage = Buffer.ReadInt32

        Buffer.Dispose()

    End Sub

    Friend Sub HandleMapProjectile(ByRef Data() As Byte)
        Dim i As Integer
        Dim Buffer As New ByteStream(Data)
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
            ClearProjectile(i)
        Next

    End Sub

    Sub ClearProjectile(Index As Integer)

        Projectiles(Index).Name = ""
        Projectiles(Index).Sprite = 0
        Projectiles(Index).Range = 0
        Projectiles(Index).Speed = 0
        Projectiles(Index).Damage = 0

    End Sub

    Sub ClearMapProjectile(ProjectileNum As Integer)

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

    Friend Sub CheckProjectiles()
        Dim i As Integer

        i = 1

        While File.Exists(Application.StartupPath & GFX_PATH & "projectiles\" & i & GFX_EXT)

            NumProjectiles = NumProjectiles + 1
            i = i + 1
        End While

        If NumProjectiles = 0 Then Exit Sub

    End Sub

    Friend Sub DrawProjectile(ProjectileNum As Integer)
        Dim rec As Rect
        Dim CanClearProjectile As Boolean
        Dim CollisionIndex As Integer
        Dim CollisionType As Byte
        Dim CollisionZone As Integer
        Dim XOffset As Integer, YOffset As Integer
        Dim X As Integer, Y As Integer
        Dim i As Integer
        Dim Sprite As Integer

        ' check to see if it's time to move the Projectile
        If GetTickCount() > MapProjectiles(ProjectileNum).TravelTime Then
            Select Case MapProjectiles(ProjectileNum).dir
                Case DirectionType.Up
                    MapProjectiles(ProjectileNum).Y = MapProjectiles(ProjectileNum).Y - 1
                Case DirectionType.Down
                    MapProjectiles(ProjectileNum).Y = MapProjectiles(ProjectileNum).Y + 1
                Case DirectionType.Left
                    MapProjectiles(ProjectileNum).X = MapProjectiles(ProjectileNum).X - 1
                Case DirectionType.Right
                    MapProjectiles(ProjectileNum).X = MapProjectiles(ProjectileNum).X + 1
            End Select
            MapProjectiles(ProjectileNum).TravelTime = GetTickCount() + Projectiles(MapProjectiles(ProjectileNum).ProjectileNum).Speed
            MapProjectiles(ProjectileNum).Range = MapProjectiles(ProjectileNum).Range + 1
        End If

        X = MapProjectiles(ProjectileNum).X
        Y = MapProjectiles(ProjectileNum).Y

        'Check if its been going for over 1 minute, if so clear.
        If MapProjectiles(ProjectileNum).Timer < GetTickCount() Then CanClearProjectile = True

        If X > Map.MaxX OrElse X < 0 Then CanClearProjectile = True
        If Y > Map.MaxY OrElse Y < 0 Then CanClearProjectile = True

        'Check for blocked wall collision
        If CanClearProjectile = False Then 'Add a check to prevent crashing
            If Map.Tile(X, Y).Type = TileType.Blocked Then CanClearProjectile = True
        End If

        'Check for npc collision
        For i = 1 To MAX_MAP_NPCS
            If MapNpc(i).X = X AndAlso MapNpc(i).Y = Y Then
                CanClearProjectile = True
                CollisionIndex = i
                CollisionType = TargetType.Npc
                CollisionZone = -1
                Exit For
            End If
        Next

        'Check for player collision
        For i = 1 To MAX_PLAYERS
            If IsPlaying(i) AndAlso GetPlayerMap(i) = GetPlayerMap(MyIndex) Then
                If GetPlayerX(i) = X AndAlso GetPlayerY(i) = Y Then
                    CanClearProjectile = True
                    CollisionIndex = i
                    CollisionType = TargetType.Player
                    CollisionZone = -1
                    If MapProjectiles(ProjectileNum).OwnerType = TargetType.Player Then
                        If MapProjectiles(ProjectileNum).Owner = i Then CanClearProjectile = False ' Reset if its the owner of projectile
                    End If
                    Exit For
                End If

            End If
        Next

        'Check if it has hit its maximum range
        If MapProjectiles(ProjectileNum).Range >= Projectiles(MapProjectiles(ProjectileNum).ProjectileNum).Range + 1 Then CanClearProjectile = True

        'Clear the projectile if possible
        If CanClearProjectile = True Then
            'Only send the clear to the server if you're the projectile caster or the one hit (only if owner is not a player)
            If (MapProjectiles(ProjectileNum).OwnerType = TargetType.Player AndAlso MapProjectiles(ProjectileNum).Owner = MyIndex) Then
                SendClearProjectile(ProjectileNum, CollisionIndex, CollisionType, CollisionZone)
            End If

            ClearMapProjectile(ProjectileNum)
            Exit Sub
        End If

        Sprite = Projectiles(MapProjectiles(ProjectileNum).ProjectileNum).Sprite
        If Sprite < 1 OrElse Sprite > NumProjectiles Then Exit Sub

        If ProjectileGFXInfo(Sprite).IsLoaded = False Then
            LoadTexture(Sprite, 11)
        End If

        'seeying we still use it, lets update timer
        With ProjectileGFXInfo(Sprite)
            .TextureTimer = GetTickCount() + 100000
        End With

        ' src rect
        With rec
            .Top = 0
            .Bottom = ProjectileGFXInfo(Sprite).Height
            .Left = MapProjectiles(ProjectileNum).dir * PIC_X
            .Right = .Left + PIC_X
        End With

        'Find the offset
        Select Case MapProjectiles(ProjectileNum).dir
            Case DirectionType.Up
                YOffset = ((MapProjectiles(ProjectileNum).TravelTime - GetTickCount()) / Projectiles(MapProjectiles(ProjectileNum).ProjectileNum).Speed) * PIC_Y
            Case DirectionType.Down
                YOffset = -((MapProjectiles(ProjectileNum).TravelTime - GetTickCount()) / Projectiles(MapProjectiles(ProjectileNum).ProjectileNum).Speed) * PIC_Y
            Case DirectionType.Left
                XOffset = ((MapProjectiles(ProjectileNum).TravelTime - GetTickCount()) / Projectiles(MapProjectiles(ProjectileNum).ProjectileNum).Speed) * PIC_X
            Case DirectionType.Right
                XOffset = -((MapProjectiles(ProjectileNum).TravelTime - GetTickCount()) / Projectiles(MapProjectiles(ProjectileNum).ProjectileNum).Speed) * PIC_X
        End Select

        X = ConvertMapX(X * PIC_X)
        Y = ConvertMapY(Y * PIC_Y)

        Dim tmpSprite As Sprite = New Sprite(ProjectileGFX(Sprite))
        tmpSprite.TextureRect = New IntRect(rec.Left, rec.Top, 32, 32)
        tmpSprite.Position = New Vector2f(X, Y)
        GameWindow.Draw(tmpSprite)

    End Sub

#End Region
End Module