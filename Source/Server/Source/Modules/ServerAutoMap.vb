Imports System.IO

Module ServerAutoMap
    ' Automapper System
    ' Version: 1.0
    ' Author: Lucas Tardivo (boasfesta)
    ' Map analysis and tips: Richard Johnson, Luan Meireles (Alenzinho)

    Private MapOrientation() As MapOrientationRec

    Friend MapStart As Integer = 1
    Friend MapSize As Integer = 4
    Friend MapX As Integer = 50
    Friend MapY As Integer = 50

    Friend SandBorder As Integer = 4
    Friend DetailFreq As Integer = 10
    Friend ResourceFreq As Integer = 20

    Friend DetailsChecked As Boolean = True
    Friend PathsChecked As Boolean = True
    Friend RiversChecked As Boolean = True
    Friend MountainsChecked As Boolean = True
    Friend OverGrassChecked As Boolean = True
    Friend ResourcesChecked As Boolean = True

    Enum TilePrefab
        Water = 1
        Sand
        Grass
        Passing
        Overgrass
        River
        Mountain
        Count
    End Enum

    'Distance between mountains and the map limit, so the player can walk freely when teleport between maps
    Private Const MountainBorder As Byte = 5

    Friend Tile(TilePrefab.Count - 1) As TileRec
    Friend Detail() As DetailRec
    Friend ResourcesNum As String
    Private Resources() As String
    Private ActualMap As Integer

    Enum MountainTile
        UpLeftBorder = 0
        UpMidBorder
        UpRightBorder
        MidLeftBorder
        Middle
        MidRightBorder
        BottomLeftBorder
        BottomMidBorder
        BottomRightBorder
        LeftBody
        MiddleBody
        RightBody
        LeftFoot
        MiddleFoot
        RightFoot
    End Enum

    Enum MapPrefab
        Undefined = 0
        UpLeftQuarter
        UpBorder
        UpRightQuarter
        RightBorder
        DownRightQuarter
        BottomBorder
        DownLeftQuarter
        LeftBorder
        Common
    End Enum

    Structure DetailRec
        Dim DetailBase As Byte
        Dim Tile As TileRec
    End Structure

    Structure MapOrientationRec
        Dim Prefab As Integer
        Dim TileStartX As Integer
        Dim TileStartY As Integer
        Dim TileEndX As Integer
        Dim TileEndY As Integer
        Dim Tile(,) As TilePrefab
    End Structure

    ''' <summary>
    ''' Loads TilePrefab from the Automapper.ini
    ''' </summary>
    Sub LoadTilePrefab()
        Dim Prefab As Integer, Layer As Integer

        Dim myXml As New XmlClass With {
            .Filename = Path.Combine(Application.StartupPath, "Data", "AutoMapper.xml"),
            .Root = "Options"
        }

        ReDim Tile(TilePrefab.Count - 1)
        For Prefab = 1 To TilePrefab.Count - 1

            ReDim Tile(Prefab).Layer(LayerType.Count - 1)
            For Layer = 1 To LayerType.Count - 1
                Tile(Prefab).Layer(Layer).Tileset = Val(myXml.ReadString("Prefab" & Prefab, "Layer" & Layer & "Tileset"))
                Tile(Prefab).Layer(Layer).X = Val(myXml.ReadString("Prefab" & Prefab, "Layer" & Layer & "X"))
                Tile(Prefab).Layer(Layer).Y = Val(myXml.ReadString("Prefab" & Prefab, "Layer" & Layer & "Y"))
                Tile(Prefab).Layer(Layer).AutoTile = Val(myXml.ReadString("Prefab" & Prefab, "Layer" & Layer & "Autotile"))
            Next Layer
            Tile(Prefab).Type = Val(myXml.ReadString("Prefab" & Prefab, "Type"))
        Next Prefab

        ResourcesNum = myXml.ReadString("Resources", "ResourcesNum")
        Resources = Split(ResourcesNum, ";")

    End Sub

    ''' <summary>
    ''' Load details to the rec
    ''' </summary>
    ''' <param name="Prefab">Which TilePrefab to use.</param>
    ''' <param name="Tileset">Tileset number to use.</param>
    ''' <param name="X">The X coordinate, where the tiles start on the tilesheet.</param>
    ''' <param name="Y">The Y coordinate, where the tiles start on the tilesheet.</param>
    ''' <param name="TileType">Which TileType to use, if any, blocked, None, etc</param>
    ''' <param name="EndX">The X coordinate, where the tiles end on the tilesheet.</param>
    ''' <param name="EndY">The Y coordinate, where the tiles end on the tilesheet.</param>
    Sub LoadDetail(Prefab As TilePrefab, Tileset As Integer, X As Integer, Y As Integer, Optional TileType As Integer = 0, Optional EndX As Integer = 0, Optional EndY As Integer = 0)
        If EndX = 0 Then EndX = X
        If EndY = 0 Then EndY = Y

        Dim pX As Integer, pY As Integer
        For pX = X To EndX
            For pY = Y To EndY
                AddDetail(Prefab, Tileset, pX, pY, TileType)
            Next
        Next

    End Sub

    ''' <summary>
    ''' Load details to memory for mapping.
    ''' </summary>
    ''' <param name="Prefab">Which TilePrefab to use.</param>
    ''' <param name="Tileset">Tileset number to use.</param>
    ''' <param name="X">The X coordinate, where the tiles start on the tilesheet.</param>
    ''' <param name="Y">The Y coordinate, where the tiles start on the tilesheet.</param>
    ''' <param name="TileType">Which TileType to use, if any, blocked, None, etc.</param>
    Sub AddDetail(Prefab As TilePrefab, Tileset As Integer, X As Integer, Y As Integer, TileType As Integer)
        Dim DetailCount As Integer

        DetailCount = UBound(Detail) + 1

        ReDim Preserve Detail(DetailCount)
        ReDim Preserve Detail(DetailCount).Tile.Layer(LayerType.Count - 1)

        Detail(DetailCount).DetailBase = Prefab
        Detail(DetailCount).Tile.Type = TileType
        Detail(DetailCount).Tile.Layer(LayerType.Mask2).Tileset = Tileset
        Detail(DetailCount).Tile.Layer(LayerType.Mask2).x = X
        Detail(DetailCount).Tile.Layer(LayerType.Mask2).y = Y
    End Sub

    ''' <summary>
    ''' Here a user can define which details to add
    ''' </summary>
    Sub LoadDetails()
        ReDim Detail(1)

        'Detail config area
        'Use: LoadDetail TilePrefab, Tileset, StartTilesetX, StartTilesetY, TileType, EndTilesetX, EndTilesetY
        LoadDetail(TilePrefab.Grass, 9, 0, 0, TileType.None, 9, 15)

        LoadDetail(TilePrefab.Sand, 10, 0, 13, TileType.None, 7, 14)
        LoadDetail(TilePrefab.Sand, 11, 0, 0, TileType.None, 1, 1)
    End Sub

    ''' <summary>
    ''' Check for details
    ''' </summary>
    ''' <param name="Prefab"></param>
    ''' <returns></returns>
    Function HaveDetails(Prefab As TilePrefab) As Boolean
        HaveDetails = Not (Prefab = TilePrefab.Water OrElse Prefab = TilePrefab.River)
    End Function

    Sub AddTile(Prefab As TilePrefab, mapNum as Integer, X As Integer, Y As Integer)
        Dim TileDest As TileRec
        Dim CleanNextTiles As Boolean
        Dim i As Integer

        'DetailFreq = Val(frmAutoMapper.txtDetail.Text)

        TileDest = Map(MapNum).Tile(X, Y)
        TileDest.Type = Tile(Prefab).Type

        ReDim Preserve TileDest.Layer(LayerType.Count - 1)

        For i = 1 To LayerType.Count - 1
            If Tile(Prefab).Layer(i).Tileset <> 0 OrElse CleanNextTiles Then
                TileDest.Layer(i) = Tile(Prefab).Layer(i)
                If Not HaveDetails(Prefab) Then CleanNextTiles = True
            End If
        Next i

        If Prefab = TilePrefab.Grass OrElse Prefab = TilePrefab.Sand Then
            If DetailsChecked = True Then
                If Random(1, DetailFreq) = 1 Then
                    Dim DetailNum As Integer
                    Dim Details() As Integer
                    ReDim Details(1)
                    For i = 1 To UBound(Detail)
                        If Detail(i).DetailBase = Prefab Then
                            ReDim Preserve Details(UBound(Details) + 1)
                            Details(UBound(Details)) = i
                        End If
                    Next i
                    If UBound(Details) > 1 Then
                        DetailNum = Details(Random(2, UBound(Details)))
                        If Detail(DetailNum).DetailBase = Prefab Then
                            TileDest.Layer(3) = Detail(DetailNum).Tile.Layer(3)
                            TileDest.Type = Detail(DetailNum).Tile.Type
                        End If
                    End If
                End If
            End If
        End If

        Map(MapNum).Tile(X, Y) = TileDest
        MapOrientation(MapNum).Tile(X, Y) = Prefab
    End Sub

    Function CanPlaceResource(mapNum as Integer, X As Integer, Y As Integer) As Boolean
        If MapOrientation(MapNum).Tile(X, Y) = TilePrefab.Grass OrElse MapOrientation(MapNum).Tile(X, Y) = TilePrefab.Overgrass OrElse (MapOrientation(MapNum).Tile(X, Y) = TilePrefab.Mountain AndAlso Not Map(MapNum).Tile(X, Y).Type = TileType.Blocked) Then
            CanPlaceResource = True
        End If
    End Function

    Function CanPlaceOvergrass(mapNum as Integer, X As Integer, Y As Integer) As Boolean
        If MapOrientation(MapNum).Tile(X, Y) = TilePrefab.Grass OrElse (MapOrientation(MapNum).Tile(X, Y) = TilePrefab.Mountain AndAlso Not Map(MapNum).Tile(X, Y).Type = TileType.Blocked) Then
            CanPlaceOvergrass = True
        End If
    End Function

    Sub MakeResource(mapNum as Integer)
        Dim x As Integer, y As Integer

        For x = 1 To Map(MapNum).MaxX - 1
            For y = 1 To Map(MapNum).MaxY - 1
                If CanPlaceResource(MapNum, x, y) AndAlso CanPlaceResource(MapNum, x - 1, y) AndAlso CanPlaceResource(MapNum, x + 1, y) AndAlso CanPlaceResource(MapNum, x, y - 1) AndAlso CanPlaceResource(MapNum, x, y + 1) Then
                    Dim ResourceNum As Integer

                    If Random(1, ResourceFreq) = 1 Then
                        ResourceNum = Val(Resources(Random(1, UBound(Resources))))
                        Map(MapNum).Tile(x, y).Type = TileType.Resource
                        Map(MapNum).Tile(x, y).Data1 = ResourceNum
                    End If
                End If
            Next y
        Next x
    End Sub

    Sub MakeResources(MapStart As Integer, Size As Integer)
        Dim i As Integer
        Dim TotalMaps As Integer
        Dim tick As Integer

        Console.WriteLine("Working...")
        Application.DoEvents()
        tick = GetTimeMs()
        TotalMaps = Size * Size

        For i = MapStart To MapStart + TotalMaps - 1
            MakeResource(i)
            CacheResources(i)
        Next i

        tick = GetTimeMs() - tick
        Console.WriteLine("Done and cached resources in " & CDbl(tick / 1000) & "s")
        Application.DoEvents()
    End Sub

    Sub MakeOvergrasses(MapStart As Integer, Size As Integer)
        Dim i As Integer
        Dim TotalMaps As Integer
        Dim tick As Integer

        Console.WriteLine("Working...")
        Application.DoEvents()
        tick = GetTimeMs()
        TotalMaps = Size * Size

        For i = MapStart To MapStart + TotalMaps - 1
            MakeOvergrass(i)
        Next i

        tick = GetTimeMs() - tick
        Console.WriteLine("Done overgrasses in " & CDbl(tick / 1000) & "s")
        Application.DoEvents()
    End Sub

    Sub MakeOvergrass(mapNum as Integer)
        Dim StartX As Integer, StartY As Integer
        Dim TotalOvergrass As Integer
        'Dim MapSize As Integer
        Dim x As Integer, y As Integer
        Dim GrassCount As Integer
        Dim OvergrassCount As Integer
        Dim TotalWalk As Integer
        Dim NextDir As Integer
        Dim BrushSize As Integer
        Dim FoundBorder As Boolean

        With Map(MapNum)
            For x = 0 To .MaxX
                For y = 0 To .MaxY
                    If CanPlaceOvergrass(MapNum, x, y) Then
                        GrassCount = GrassCount + 1
                    End If
                Next y
            Next x

            TotalOvergrass = Random(Int(GrassCount / 100), Int(GrassCount / 50))

            Do Until OvergrassCount >= TotalOvergrass
                TotalWalk = 0
                BrushSize = Random(1, 2)
                StartX = Random(0, .MaxX)
                StartY = Random(0, .MaxY)

                If CanPlaceOvergrass(MapNum, StartX, StartY) Then
                    PaintOvergrass(MapNum, StartX, StartY, BrushSize, BrushSize)
                    x = StartX
                    y = StartY
                    TotalWalk = 1
                    NextDir = 0
                    Do While NextDir <> 5 AndAlso TotalWalk < 15
                        If FoundBorder Then
                            NextDir = Random(0, 5)
                        Else
                            NextDir = Random(0, 4)
                        End If
                        Select Case NextDir
                            Case DirectionType.Up : y = y - 1
                            Case DirectionType.Down : y = y + 1
                            Case DirectionType.Left : x = x - 1
                            Case DirectionType.Right : x = x + 1
                        End Select
                        If NextDir < 5 Then
                            If x > 0 AndAlso x < .MaxX Then
                                If y > 0 AndAlso y < .MaxY Then
                                    If CanPlaceOvergrass(MapNum, x, y) Then
                                        BrushSize = Random(0, 2)
                                        PaintOvergrass(MapNum, x, y, BrushSize, BrushSize)
                                        TotalWalk = TotalWalk + 1
                                        FoundBorder = True
                                    Else
                                        If MapOrientation(MapNum).Tile(x, y) = TilePrefab.Overgrass Then
                                            FoundBorder = False
                                        Else
                                            NextDir = 5
                                        End If
                                    End If
                                Else
                                    NextDir = 5
                                End If
                            Else
                                NextDir = 5
                            End If
                        End If
                    Loop
                    OvergrassCount = OvergrassCount + 1
                End If
            Loop

        End With

    End Sub

    Sub PaintOvergrass(mapNum as Integer, X As Integer, Y As Integer, BrushSizeX As Integer, BrushSizeY As Integer)
        Dim pX As Integer, pY As Integer

        For pX = X - BrushSizeX To X + BrushSizeX
            For pY = Y - BrushSizeY To Y + BrushSizeY
                If pX >= 0 AndAlso pX <= Map(MapNum).MaxX Then
                    If pY >= 0 AndAlso pY <= Map(MapNum).MaxY Then
                        If MapOrientation(MapNum).Tile(pX, pY) <> TilePrefab.Overgrass Then
                            If CanPlaceOvergrass(MapNum, pX, pY) Then
                                If Random(1, 100) >= 50 Then
                                    AddTile(TilePrefab.Overgrass, MapNum, pX, pY)
                                End If
                            End If
                        End If
                    End If
                End If
            Next pY
        Next pX
    End Sub

    Sub PaintTile(Prefab As TilePrefab, mapNum as Integer, X As Integer, Y As Integer, BrushSizeX As Integer, BrushSizeY As Integer, Optional HumanMade As Boolean = True, Optional OnlyTo As TilePrefab = 0)
        Dim pX As Integer, pY As Integer
        For pX = X - BrushSizeX To X + BrushSizeX
            For pY = Y - BrushSizeY To Y + BrushSizeY
                If pX >= 0 AndAlso pX <= Map(MapNum).MaxX Then
                    If pY >= 0 AndAlso pY <= Map(MapNum).MaxY Then
                        If MapOrientation(MapNum).Tile(pX, pY) <> Prefab Then
                            Dim CanPaint As Boolean
                            CanPaint = True
                            If OnlyTo <> 0 Then
                                If MapOrientation(MapNum).Tile(pX, pY) <> OnlyTo Then CanPaint = False
                            End If
                            If CanPaint Then
                                If HumanMade Then
                                    AddTile(Prefab, MapNum, pX, pY)
                                Else
                                    If Random(1, 100) >= 50 Then
                                        AddTile(Prefab, MapNum, pX, pY)
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next pY
        Next pX
    End Sub

    Sub PaintRiver(mapNum as Integer, X As Integer, Y As Integer, RiverDir As Byte, RiverWidth As Integer)
        Dim pX As Integer, pY As Integer
        If RiverDir = DirectionType.Down Then
            For pX = X - RiverWidth To X + RiverWidth
                If pX > 0 AndAlso pX < Map(MapNum).MaxX Then
                    AddTile(TilePrefab.River, MapNum, pX, Y)
                End If
            Next pX
        End If
        If RiverDir = DirectionType.Left OrElse RiverDir = DirectionType.Right Then
            For pY = Y - RiverWidth To Y + RiverWidth
                If pY > 0 AndAlso pY < Map(MapNum).MaxY Then
                    AddTile(TilePrefab.River, MapNum, X, pY)
                End If
            Next pY
        End If
    End Sub

    Sub MakeRivers(MapStart As Integer, Size As Integer)
        'Dim RiverType As Integer
        Dim RiverMap As Integer
        Dim TotalRivers As Integer
        Dim TotalMaps As Integer
        Dim MadeRivers As Integer
        Dim RiverX As Integer, RiverY As Integer
        Dim RiverWidth As Integer, RiverHeight As Integer
        Dim RiverDir As Byte
        Dim RiverBorder As Integer
        Dim RiverFlowWidth As Integer
        Dim RiverEnd As Boolean
        Dim RiverSteps As Integer
        Dim tick As Integer

        Console.WriteLine("Working...")
        Application.DoEvents()
        tick = GetTimeMs()
        RiverBorder = 4
        MadeRivers = 0
        TotalMaps = Size * Size
        TotalRivers = Int(TotalMaps / 4)

        Do While MadeRivers <= TotalRivers
            'Start river
SelectMap:
            RiverMap = Random(MapStart, MapStart + TotalMaps - 1)
            If MapOrientation(RiverMap).Prefab <> MapPrefab.Common Then GoTo SelectMap
            RiverX = Random(RiverBorder, Map(RiverMap).MaxX - RiverBorder)
            RiverY = Random(RiverBorder, Map(RiverMap).MaxY - RiverBorder)
            RiverWidth = Random(2, 3)
            RiverHeight = Random(2, 3)
            RiverFlowWidth = 1
            AddTile(TilePrefab.River, RiverMap, RiverX, RiverY)
            PaintTile(TilePrefab.River, RiverMap, RiverX, RiverY, RiverWidth, RiverHeight, False)
            RiverDir = Random(1, 3)
            RiverEnd = False
            RiverSteps = 0
            RiverFlowWidth = Random(1, 3)
            Do Until MapOrientation(RiverMap).Tile(RiverX, RiverY) <> TilePrefab.River
                If RiverDir = DirectionType.Left Then
                    RiverX = RiverX - 1
                    If RiverX < 0 Then
                        RiverX = 0
                        RiverDir = DirectionType.Right
                    End If
                End If
                If RiverDir = DirectionType.Down Then
                    RiverY = RiverY + 1
                    If RiverY > Map(RiverMap).MaxY Then
                        RiverY = Map(RiverMap).MaxY
                        RiverDir = Random(2, 3)
                    End If
                End If
                If RiverDir = DirectionType.Right Then
                    RiverX = RiverX + 1
                    If RiverX > Map(RiverMap).MaxX Then
                        RiverX = Map(RiverMap).MaxX
                        RiverDir = DirectionType.Left
                    End If
                End If
            Loop
            Do While Not RiverEnd
                RiverSteps = RiverSteps + 1
                If RiverX < 0 Then RiverX = 0
                If RiverX > Map(RiverMap).MaxX Then RiverX = Map(RiverMap).MaxX
                If RiverY < 0 Then RiverY = 0
                If RiverY > Map(RiverMap).MaxY Then RiverY = Map(RiverMap).MaxY
                PaintRiver(RiverMap, RiverX, RiverY, RiverDir, RiverFlowWidth)
                Select Case RiverDir
                    Case DirectionType.Left : RiverY = RiverY + Random(-1, 1)
                    Case DirectionType.Down : RiverX = RiverX + Random(-1, 1)
                    Case DirectionType.Right : RiverY = RiverY + Random(-1, 1)
                End Select

                If Random(1, 100) < 5 Then 'Change dir
                    RiverDir = Random(1, 3)
                End If
                Select Case RiverDir
                    Case DirectionType.Left : RiverX = RiverX - 1
                    Case DirectionType.Down : RiverY = RiverY + 1
                    Case DirectionType.Right : RiverX = RiverX + 1
                End Select
                If RiverDir = DirectionType.Down Then
                    If MapOrientation(Map(RiverMap).Down).Prefab = MapPrefab.Common Then
                        If RiverY > Map(RiverMap).MaxY Then
                            RiverMap = Map(RiverMap).Down
                            RiverY = 0
                        End If
                    Else
                        If RiverY > Map(RiverMap).MaxY / 2 Then
                            PaintTile(TilePrefab.River, RiverMap, RiverX, RiverY, Random(2, 3), Random(3, 4), False)
                            RiverEnd = True
                        End If
                    End If
                End If
                If RiverDir = DirectionType.Left Then
                    If MapOrientation(Map(RiverMap).Left).Prefab = MapPrefab.Common Then
                        If RiverX < 0 Then
                            'MapCache_Create RiverMap
                            RiverMap = Map(RiverMap).Left
                            RiverX = Map(RiverMap).MaxX
                        End If
                    Else
                        If RiverX < Map(RiverMap).MaxX / 2 Then
                            PaintTile(TilePrefab.River, RiverMap, RiverX, RiverY, Random(2, 3), Random(3, 4), False)
                            RiverEnd = True
                        End If
                    End If
                End If
                If RiverDir = DirectionType.Right Then
                    If MapOrientation(Map(RiverMap).Right).Prefab = MapPrefab.Common Then
                        If RiverX > Map(RiverMap).MaxX Then
                            'MapCache_Create RiverMap
                            RiverMap = Map(RiverMap).Right
                            RiverX = 0
                        End If
                    Else
                        If RiverX > Map(RiverMap).MaxX / 2 Then
                            PaintTile(TilePrefab.River, RiverMap, RiverX, RiverY, Random(2, 3), Random(3, 4), False)
                            RiverEnd = True
                        End If
                    End If
                End If
            Loop
            MadeRivers = MadeRivers + 1
        Loop

        tick = GetTimeMs() - tick
        Console.WriteLine("Done " & TotalRivers & " rivers in " & CDbl(tick / 1000) & "s")
        Application.DoEvents()
    End Sub

    Sub PlaceMountain(mapNum as Integer, X As Integer, Y As Integer, MountainPrefab As MountainTile)
        Dim OldX As Integer, OldY As Integer

        OldX = Tile(TilePrefab.Mountain).Layer(2).X
        OldY = Tile(TilePrefab.Mountain).Layer(2).Y
        Tile(TilePrefab.Mountain).Layer(2).X = OldX + (MountainPrefab Mod 3)
        Tile(TilePrefab.Mountain).Layer(2).Y = OldY + (Int(MountainPrefab / 3))
        AddTile(TilePrefab.Mountain, MapNum, X, Y)
        Tile(TilePrefab.Mountain).Layer(2).X = OldX
        Tile(TilePrefab.Mountain).Layer(2).Y = OldY
    End Sub


    Sub MarkMountain(mapNum as Integer, X As Integer, Y As Integer, Width As Integer, Height As Integer)
        Dim pX As Integer, pY As Integer
        For pX = X - Int(Width / 2) To X + Int(Width / 2)
            For pY = Y - Int(Height / 2) To Y + Int(Height / 2)
                If pX > MountainBorder AndAlso pX < Map(MapNum).MaxX - MountainBorder Then
                    If pY > MountainBorder AndAlso pY < Map(MapNum).MaxY - MountainBorder Then
                        MapOrientation(MapNum).Tile(pX, pY) = TilePrefab.Mountain
                    End If
                End If
            Next pY
        Next pX
    End Sub

    Sub MakeMapMountains(mapNum as Integer)
        Dim MountainMinAreaWidth As Integer, MountainMinAreaHeight As Integer
        Dim MountainMinSize As Integer, MountainMinArea As Integer
        Dim MountainSize As Integer
        Dim x As Integer, y As Integer
        'Dim ScanX As Integer, ScanY As Integer
        'Dim FoundPlace As Boolean
        Dim TotalGrass As Integer
        Dim TotalMountains As Integer
        Dim i As Integer
        Dim PositionTries As Integer
        Dim MountainSteps As Integer
        MountainMinAreaWidth = 5
        MountainMinAreaHeight = 5
        MountainMinArea = 4
        MountainMinSize = 10

        For x = MountainBorder To Map(MapNum).MaxX - MountainBorder
            For y = MountainBorder To Map(MapNum).MaxY - MountainBorder
                If MapOrientation(MapNum).Tile(x, y) = TilePrefab.Grass Then
                    TotalGrass = TotalGrass + 1
                End If
            Next y
        Next x

        TotalMountains = Random(0, (TotalGrass / (MountainMinAreaWidth * MountainMinAreaHeight)))

        If TotalMountains > 0 Then
            For i = 1 To TotalMountains
                PositionTries = 0
Retry:
                If PositionTries < 5 Then
                    x = Random(MountainMinAreaWidth + MountainBorder, Map(MapNum).MaxX - MountainMinAreaWidth - MountainBorder)
                    y = Random(MountainMinAreaHeight + MountainBorder, Map(MapNum).MaxY - MountainMinAreaHeight - MountainBorder)
                    If Not ValidMountainPosition(MapNum, x, y, MountainMinAreaWidth, MountainMinAreaHeight) Then
                        PositionTries = PositionTries + 1
                        GoTo Retry
                    End If
                    MarkMountain(MapNum, x, y, MountainMinAreaWidth, MountainMinAreaHeight)

                    MountainSteps = 0
                    MountainSize = Random(MountainMinSize, MountainMinSize * (TotalMountains / i))

                    Do While MountainSteps < MountainSize
                        Dim OldX As Integer, OldY As Integer
                        OldX = x
                        OldY = y
                        x = x + (Random(0, 2) - 1)
                        y = y + (Random(0, 2) - 1)
                        If ValidMountainPosition(MapNum, x, y, 3, 5) Then
                            MarkMountain(MapNum, x, y, 3, 5)
                        Else
                            'Return
                            x = OldX
                            y = OldY
                        End If
                        MountainSteps = MountainSteps + 1
                    Loop

                End If
            Next i

            'Fill Mountain
            For x = MountainBorder To Map(MapNum).MaxX - MountainBorder
                For y = MountainBorder To Map(MapNum).MaxY - MountainBorder
                    If MapOrientation(MapNum).Tile(x, y) = TilePrefab.Mountain Then
                        Dim MountainPrefab As MountainTile
                        MountainPrefab = GetMountainPrefab(MapNum, x, y)
                        'Exceptions
                        If Not MapOrientation(MapNum).Tile(x, y - 1) <> TilePrefab.Mountain Then
                            If ((GetMountainPrefab(MapNum, x - 1, y) = MountainTile.MiddleFoot OrElse GetMountainPrefab(MapNum, x - 1, y) = MountainTile.LeftFoot) OrElse (GetMountainPrefab(MapNum, x - 1, y) = MountainTile.LeftBody OrElse GetMountainPrefab(MapNum, x - 1, y) = MountainTile.MiddleBody)) AndAlso Not (MountainPrefab = MountainTile.MiddleBody OrElse MountainPrefab = MountainTile.MiddleFoot OrElse MountainPrefab = MountainTile.RightBody OrElse MountainPrefab = MountainTile.RightFoot) Then
                                MountainPrefab = MountainTile.MidLeftBorder
                            End If
                            If GetMountainPrefab(MapNum, x, y + 1) = MountainTile.LeftFoot Then
                                MountainPrefab = MountainTile.LeftBody
                                GoTo Important
                            End If
                            If GetMountainPrefab(MapNum, x, y + 2) = MountainTile.LeftFoot Then
                                MountainPrefab = MountainTile.BottomLeftBorder
                                GoTo Important
                            End If
                            If ((GetMountainPrefab(MapNum, x + 1, y) = MountainTile.MiddleFoot OrElse GetMountainPrefab(MapNum, x + 1, y) = MountainTile.RightFoot) OrElse (GetMountainPrefab(MapNum, x + 1, y) = MountainTile.RightBody OrElse GetMountainPrefab(MapNum, x + 1, y) = MountainTile.MiddleBody)) AndAlso Not (MountainPrefab = MountainTile.MiddleBody OrElse MountainPrefab = MountainTile.MiddleFoot OrElse MountainPrefab = MountainTile.LeftBody OrElse MountainPrefab = MountainTile.LeftFoot) Then
                                MountainPrefab = MountainTile.MidRightBorder
                            End If
                            If GetMountainPrefab(MapNum, x, y + 1) = MountainTile.RightFoot Then
                                MountainPrefab = MountainTile.RightBody
                                GoTo Important
                            End If
                            If GetMountainPrefab(MapNum, x, y + 2) = MountainTile.RightFoot Then
                                MountainPrefab = MountainTile.BottomRightBorder
                                GoTo Important
                            End If
                        End If

Important:
                        If MountainPrefab >= 0 Then PlaceMountain(MapNum, x, y, MountainPrefab)
                    End If
                Next y
            Next x
        End If
    End Sub

    Function GetMountainPrefab(mapNum as Integer, X As Integer, Y As Integer) As MountainTile
        Dim VerticalPos As Byte
        Dim MountainPrefab As MountainTile
        If MapOrientation(MapNum).Tile(X, Y) = TilePrefab.Mountain Then
            VerticalPos = 1
            If MapOrientation(MapNum).Tile(X - 1, Y) <> TilePrefab.Mountain Then
                VerticalPos = 0
            End If
            If MapOrientation(MapNum).Tile(X + 1, Y) <> TilePrefab.Mountain Then
                VerticalPos = 2
            End If
            MountainPrefab = -1
            If MapOrientation(MapNum).Tile(X, Y - 1) = TilePrefab.Mountain Then
                'Its not the top
                If Y + 3 < Map(MapNum).MaxY Then
                    If MapOrientation(MapNum).Tile(X, Y + 3) <> TilePrefab.Mountain AndAlso MapOrientation(MapNum).Tile(X, Y + 2) = TilePrefab.Mountain Then
                        'Inferior
                        Select Case VerticalPos
                            Case 0 : MountainPrefab = MountainTile.BottomLeftBorder
                            Case 1 : MountainPrefab = MountainTile.BottomMidBorder
                            Case 2 : MountainPrefab = MountainTile.BottomRightBorder
                        End Select
                    Else
                        If MapOrientation(MapNum).Tile(X, Y + 2) <> TilePrefab.Mountain AndAlso MapOrientation(MapNum).Tile(X, Y + 1) = TilePrefab.Mountain Then
                            'Body
                            Select Case VerticalPos
                                Case 0 : MountainPrefab = MountainTile.LeftBody
                                Case 1 : MountainPrefab = MountainTile.MiddleBody
                                Case 2 : MountainPrefab = MountainTile.RightBody
                            End Select
                        Else
                            If MapOrientation(MapNum).Tile(X, Y + 1) <> TilePrefab.Mountain Then
                                'Foots
                                Select Case VerticalPos
                                    Case 0 : MountainPrefab = MountainTile.LeftFoot
                                    Case 1 : MountainPrefab = MountainTile.MiddleFoot
                                    Case 2 : MountainPrefab = MountainTile.RightFoot
                                End Select
                            Else
                                'Mid
                                Select Case VerticalPos
                                    Case 0 : MountainPrefab = MountainTile.MidLeftBorder
                                    Case 2 : MountainPrefab = MountainTile.MidRightBorder
                                End Select
                            End If
                        End If
                    End If
                End If
            Else
                'Its top
                Select Case VerticalPos
                    Case 0 : MountainPrefab = MountainTile.UpLeftBorder
                    Case 1 : MountainPrefab = MountainTile.UpMidBorder
                    Case 2 : MountainPrefab = MountainTile.UpRightBorder
                End Select
            End If
            GetMountainPrefab = MountainPrefab
        Else
            GetMountainPrefab = -1
        End If
    End Function

    Function ValidMountainPosition(mapNum as Integer, X As Integer, Y As Integer, Width As Integer, Height As Integer) As Boolean
        Dim pX As Integer, pY As Integer
        ValidMountainPosition = True
        For pX = X - Int(Width / 2) To X + Int(Width / 2)
            For pY = Y - Int(Height / 2) To Y + Int(Height / 2)
                If pX < 1 OrElse pX > Map(MapNum).MaxX - 1 Then ValidMountainPosition = False
                If pY < 1 OrElse pY >= Map(MapNum).MaxY - 3 Then ValidMountainPosition = False
                If ValidMountainPosition Then
                    If MapOrientation(MapNum).Tile(pX, pY) <> TilePrefab.Grass AndAlso MapOrientation(MapNum).Tile(pX, pY) <> TilePrefab.Overgrass AndAlso MapOrientation(MapNum).Tile(pX, pY) <> TilePrefab.Mountain Then ValidMountainPosition = False
                End If
            Next pY
        Next pX
    End Function

    Sub MakeMountains(MapStart As Integer, Size As Integer)
        Dim i As Integer
        Dim TotalMaps As Integer
        Dim tick As Integer
        Dim MapCount As Integer
        Console.WriteLine("Working...")
        Application.DoEvents()
        tick = GetTimeMs()
        TotalMaps = Size * Size
        MapCount = 0
        For i = MapStart To MapStart + TotalMaps - 1
            If MapOrientation(i).Prefab = MapPrefab.Common Then
                MakeMapMountains(i)
                MapCount = MapCount + 1
            End If
        Next i
        tick = GetTimeMs() - tick
        Console.WriteLine("Done mountains in " & (MapCount) & " maps in " & CDbl(tick / 1000) & "s")
        Application.DoEvents()
    End Sub

    Sub MakeMap(mapNum as Integer, Prefab As MapPrefab)
        Dim x As Integer, y As Integer
        Dim TileX As Integer, TileY As Integer
        Dim TileStartX As Integer, TileStartY As Integer
        Dim TileEndX As Integer, TileEndY As Integer
        Dim Change As Integer
        Dim Changed As Boolean

        MapOrientation(MapNum).Prefab = Prefab

        With Map(MapNum)
            If Prefab <> MapPrefab.Common Then
                For x = 0 To .MaxX
                    For y = 0 To .MaxY
                        AddTile(TilePrefab.Water, MapNum, x, y)
                    Next y
                Next x
            Else
                For x = 0 To .MaxX
                    For y = 0 To .MaxY
                        AddTile(TilePrefab.Grass, MapNum, x, y)
                    Next y
                Next x
            End If
            If Prefab = MapPrefab.UpLeftQuarter Then
                TileStartX = Int(.MaxX / 2) - Random(0, Int(.MaxX / 4))
                TileStartY = .MaxY
                TileEndX = .MaxX
                TileEndY = Int(.MaxY / 2) - Random(0, Int(.MaxY / 4))
                TileX = TileStartX

                For y = TileStartY To TileEndY Step -1
                    If y <> TileStartY Then TileX = TileX + Random(0, 2)
                    If TileX >= TileEndX Then
                        TileEndY = y
                        Exit For
                    Else
                        For x = TileX To TileEndX
                            If x < TileX + SandBorder OrElse y < TileEndY + SandBorder Then
                                AddTile(TilePrefab.Sand, MapNum, x, y)
                            Else
                                AddTile(TilePrefab.Grass, MapNum, x, y)
                            End If
                        Next x
                    End If
                Next y
            End If
            If Prefab = MapPrefab.UpBorder Then
                TileStartX = 0
                TileStartY = MapOrientation(.Left).TileEndY
                TileEndX = .MaxX
                TileY = TileStartY
                Changed = True
                For x = TileStartX To TileEndX
                    If Changed = False Then
                        Change = Random(-1, 1)
                        If Change <> 0 Then
                            Changed = True
                            TileY = TileY + Change
                        End If
                    Else
                        Changed = False
                    End If
                    If TileY < Int(.MaxY / 4) Then TileY = Int(.MaxY / 4)
                    If TileY > Int(.MaxY / 2) + Int(.MaxY / 4) Then TileY = Int(.MaxY / 2) + Int(.MaxY / 4)
                    For y = TileY To .MaxY
                        If y < TileY + SandBorder Then
                            AddTile(TilePrefab.Sand, MapNum, x, y)
                        Else
                            AddTile(TilePrefab.Grass, MapNum, x, y)
                        End If
                    Next y
                Next x

                MapOrientation(MapNum).TileEndY = TileY
            End If
            If Prefab = MapPrefab.UpRightQuarter Then
                TileStartX = Random(4, 8)
                TileStartY = MapOrientation(.Left).TileEndY
                TileEndX = 0
                TileEndY = .MaxY

                TileX = TileStartX
                For y = TileStartY To TileEndY
                    If y <> TileStartY Then TileX = TileX + Random(0, 2)
                    If TileX > .MaxX Then TileX = .MaxX
                    For x = TileX To TileEndX Step -1
                        If x > TileX - SandBorder OrElse y < TileY + SandBorder Then
                            AddTile(TilePrefab.Sand, MapNum, x, y)
                        Else
                            AddTile(TilePrefab.Grass, MapNum, x, y)
                        End If
                    Next x
                Next y

                TileEndX = TileX
            End If
            If Prefab = MapPrefab.LeftBorder Then
                If .Up = MapStart Then
                    TileStartX = MapOrientation(.Up).TileStartX
                Else
                    TileStartX = MapOrientation(.Up).TileEndX
                End If
                TileStartY = 0
                TileEndX = .MaxX
                TileEndY = .MaxY
                TileX = TileStartX
                Changed = True
                For y = TileStartY To TileEndY
                    If Changed = False Then
                        Change = Random(-1, 1)
                        If Change <> 0 Then
                            Changed = True
                            TileX = TileX + Change
                        End If
                    Else
                        Changed = False
                    End If
                    If TileX < Int(.MaxX / 4) Then TileX = Int(.MaxX / 4)
                    If TileX > Int(.MaxX / 2) + Int(.MaxX / 4) Then TileX = Int(.MaxX / 2) + Int(.MaxX / 4)
                    For x = TileX To TileEndX
                        If x < TileX + SandBorder Then
                            AddTile(TilePrefab.Sand, MapNum, x, y)
                        Else
                            AddTile(TilePrefab.Grass, MapNum, x, y)
                        End If
                    Next x
                Next y

                MapOrientation(MapNum).TileEndX = TileX
            End If
            If Prefab = MapPrefab.RightBorder Then
                If .Up = MapStart Then
                    TileStartX = MapOrientation(.Up).TileStartX
                Else
                    TileStartX = MapOrientation(.Up).TileEndX
                End If
                TileStartY = 0
                TileEndX = .MaxX
                TileEndY = .MaxY
                TileX = TileStartX
                Changed = True
                For y = TileStartY To TileEndY
                    If Changed = False Then
                        Change = Random(-1, 1)
                        If Change <> 0 Then
                            Changed = True
                            TileX = TileX + Change
                        End If
                    Else
                        Changed = False
                    End If
                    If TileX < Int(.MaxX / 4) Then TileX = Int(.MaxX / 4)
                    If TileX > Int(.MaxX / 2) + Int(.MaxX / 4) Then TileX = Int(.MaxX / 2) + Int(.MaxX / 4)
                    For x = TileX To 0 Step -1
                        If x > TileX - SandBorder Then
                            AddTile(TilePrefab.Sand, MapNum, x, y)
                        Else
                            AddTile(TilePrefab.Grass, MapNum, x, y)
                        End If
                    Next x
                Next y

                MapOrientation(MapNum).TileEndX = TileX
            End If
            If Prefab = MapPrefab.DownLeftQuarter Then
                TileStartX = MapOrientation(.Up).TileEndX
                TileEndX = .MaxX
                TileStartY = 0
                TileEndY = Int(.MaxY / 2) + Random(0, Int(.MaxY / 4))

                TileX = TileStartX
                For y = TileStartY To TileEndY
                    If y <> TileStartY Then TileX = TileX + Random(0, 2)
                    If TileX >= TileEndX Then
                        TileEndY = y
                        Exit For
                    Else
                        For x = TileX To TileEndX
                            If x < TileX + SandBorder OrElse y > TileEndY - SandBorder Then
                                AddTile(TilePrefab.Sand, MapNum, x, y)
                            Else
                                AddTile(TilePrefab.Grass, MapNum, x, y)
                            End If
                        Next x
                    End If
                Next y
            End If
            If Prefab = MapPrefab.BottomBorder Then
                TileStartX = 0
                TileEndX = .MaxX
                TileStartY = MapOrientation(.Left).TileEndY

                TileY = TileStartY
                Changed = True
                For x = TileStartX To TileEndX
                    If Changed = False Then
                        Change = Random(-1, 1)
                        If Change <> 0 Then
                            Changed = True
                            TileY = TileY + Change
                        End If
                    Else
                        Changed = False
                    End If
                    If TileY < Int(.MaxY / 4) Then TileY = Int(.MaxY / 4)
                    If TileY > Int(.MaxY / 2) + Int(.MaxY / 4) Then TileY = Int(.MaxY / 2) + Int(.MaxY / 4)
                    For y = TileY To 0 Step -1
                        If y > TileY - SandBorder Then
                            AddTile(TilePrefab.Sand, MapNum, x, y)
                        Else
                            AddTile(TilePrefab.Grass, MapNum, x, y)
                        End If
                    Next y
                Next x

                MapOrientation(MapNum).TileEndY = TileY
            End If
            If Prefab = MapPrefab.DownRightQuarter Then
                TileStartY = MapOrientation(.Left).TileEndY
                TileEndY = 0
                TileStartX = 0
                TileEndX = MapOrientation(.Up).TileEndX
                TileY = TileStartY

                For x = TileStartX To TileEndX
                    If x <> TileStartX Then TileY = TileY - Random(0, 1)
                    For y = TileY To TileEndY Step -1
                        If y > TileY - SandBorder OrElse x > TileEndX - SandBorder Then
                            AddTile(TilePrefab.Sand, MapNum, x, y)
                        Else
                            AddTile(TilePrefab.Grass, MapNum, x, y)
                        End If
                    Next y
                Next x
            End If
        End With

        If MapOrientation(MapNum).TileStartX = 0 Then MapOrientation(MapNum).TileStartX = TileStartX
        If MapOrientation(MapNum).TileStartY = 0 Then MapOrientation(MapNum).TileStartY = TileStartY
        If MapOrientation(MapNum).TileEndX = 0 Then MapOrientation(MapNum).TileEndX = TileEndX
        If MapOrientation(MapNum).TileEndY = 0 Then MapOrientation(MapNum).TileEndY = TileEndY

    End Sub

    Sub MakePath(mapNum as Integer, X As Integer, Y As Integer, Dir As Byte, Optional Steps As Integer = 1)
        Dim PathEnd As Boolean
        Dim BrushX As Integer, BrushY As Integer
        Dim i As Byte
        If Not MapOrientation(MapNum).Prefab = MapPrefab.Common Then Exit Sub
        PathEnd = False
        Do While Not PathEnd
            If Steps Mod Map(MapNum).MaxX = 0 Then
                Dim OldDir As Integer
                OldDir = Dir
ChangeDir:
                Dir = Random(0, 3)
                'Avoid invert direction
                If (OldDir = DirectionType.Up AndAlso Dir = DirectionType.Down) OrElse (OldDir = DirectionType.Down AndAlso Dir = DirectionType.Up) OrElse (OldDir = DirectionType.Right AndAlso Dir = DirectionType.Left) OrElse (OldDir = DirectionType.Left AndAlso Dir = DirectionType.Right) Then GoTo ChangeDir
            End If
            Select Case Dir
                Case DirectionType.Up
                    BrushX = 1
                    BrushY = 0
                    Y = Y - 1
                    X = X + Random(0, 2) - 1
                    If X <= 1 Then X = 1
                    If X >= Map(MapNum).MaxX - 1 Then X = Map(MapNum).MaxX - 1
                Case DirectionType.Down
                    BrushX = 1
                    BrushY = 0
                    Y = Y + 1
                    X = X + Random(0, 2) - 1
                    If X <= 1 Then X = 1
                    If X >= Map(MapNum).MaxX - 1 Then X = Map(MapNum).MaxX - 1
                Case DirectionType.Left
                    BrushX = 0
                    BrushY = 1
                    Y = Y + Random(0, 2) - 1
                    X = X - 1
                    If Y <= 1 Then Y = 1
                    If Y >= Map(MapNum).MaxY - 1 Then Y = Map(MapNum).MaxY - 1
                Case DirectionType.Right
                    BrushX = 0
                    BrushY = 1
                    Y = Y + Random(0, 2) - 1
                    X = X + 1
                    If Y <= 1 Then Y = 1
                    If Y >= Map(MapNum).MaxY - 1 Then Y = Map(MapNum).MaxY - 1
            End Select
            If X <= 0 Then
                If Map(MapNum).Left > 0 Then
                    If MapOrientation(Map(MapNum).Left).Prefab = MapPrefab.Common Then
                        PaintTile(TilePrefab.Passing, MapNum, X, Y, BrushX, BrushY, , TilePrefab.Grass)
                        PaintTile(TilePrefab.Passing, Map(MapNum).Left, Val(Map(MapNum).MaxX), Y, BrushX, BrushY, , TilePrefab.Grass)
                        MakePath(Map(MapNum).Left, Map(MapNum).MaxX, Y, Dir, Steps)
                    End If
                End If
                Exit Sub
            End If
            If X >= Map(MapNum).MaxX Then
                If Map(MapNum).Right > 0 Then
                    If MapOrientation(Map(MapNum).Right).Prefab = MapPrefab.Common Then
                        PaintTile(TilePrefab.Passing, MapNum, X, Y, BrushX, BrushY, , TilePrefab.Grass)
                        PaintTile(TilePrefab.Passing, Map(MapNum).Right, 0, Y, BrushX, BrushY, , TilePrefab.Grass)
                        MakePath(Map(MapNum).Right, 0, Y, Dir, Steps)
                    End If
                End If
                Exit Sub
            End If
            If Y <= 0 Then
                If Map(MapNum).Up > 0 Then
                    If MapOrientation(Map(MapNum).Up).Prefab = MapPrefab.Common Then
                        PaintTile(TilePrefab.Passing, MapNum, X, Y, BrushX, BrushY, , TilePrefab.Grass)
                        PaintTile(TilePrefab.Passing, Map(MapNum).Up, X, Val(Map(MapNum).MaxY), BrushX, BrushY, , TilePrefab.Grass)
                        MakePath(Map(MapNum).Up, X, Map(MapNum).MaxY, Dir, Steps)
                    End If
                End If
                Exit Sub
            End If
            If Y >= Map(MapNum).MaxY Then
                If Map(MapNum).Down > 0 Then
                    If MapOrientation(Map(MapNum).Down).Prefab = MapPrefab.Common Then
                        PaintTile(TilePrefab.Passing, MapNum, X, Y, BrushX, BrushY, , TilePrefab.Grass)
                        PaintTile(TilePrefab.Passing, Map(MapNum).Down, X, 0, BrushX, BrushY, , TilePrefab.Grass)
                        MakePath(Map(MapNum).Down, X, 0, Dir, Steps)
                    End If
                End If
                Exit Sub
            End If

            If CheckPath(MapNum, X, Y, Dir) Then
                PaintTile(TilePrefab.Passing, MapNum, X, Y, BrushX, BrushY, , TilePrefab.Grass)
                Steps = Steps + 1
            Else
                For i = 0 To 3
                    If i <> Dir Then
                        If CheckPath(MapNum, X, Y, i) Then
                            Dir = i
                            Exit For
                        End If
                    End If
                Next i
            End If
        Loop
    End Sub

    Function CheckPath(mapNum as Integer, X As Integer, Y As Integer, Dir As Byte) As Boolean
        Dim SizeX As Integer, SizeY As Integer
        Select Case Dir
            Case DirectionType.Up, DirectionType.Down : SizeX = 1
            Case DirectionType.Left, DirectionType.Right : SizeY = 1
        End Select

        CheckPath = True

        Dim pX As Integer, pY As Integer
        For pX = X - SizeX To X + SizeX
            For pY = Y - SizeY To Y + SizeY
                If pX >= 0 AndAlso pX <= Map(MapNum).MaxX Then
                    If pY >= 0 AndAlso pY <= Map(MapNum).MaxY Then
                        If MapOrientation(MapNum).Tile(pX, pY) <> TilePrefab.Grass AndAlso MapOrientation(MapNum).Tile(pX, pY) <> TilePrefab.Passing Then
                            CheckPath = False
                            Exit Function
                        End If
                    End If
                End If
            Next pY
        Next pX
    End Function

    Function SearchForPreviousPaths(mapNum as Integer, X As Integer, Y As Integer) As Boolean
        Dim pX As Integer, pY As Integer
        For pX = X - 10 To X + 10
            For pY = Y - 10 To Y + 10
                If pX >= 0 AndAlso pX <= Map(MapNum).MaxX Then
                    If pY >= 0 AndAlso pY <= Map(MapNum).MaxY Then
                        If MapOrientation(MapNum).Tile(pX, pY) = TilePrefab.Passing Then
                            SearchForPreviousPaths = True
                            Exit Function
                        End If
                    End If
                End If
            Next pY
        Next pX
    End Function

    Sub MakeMapPaths(mapNum as Integer)
        Dim x As Integer, y As Integer
        Dim StartX() As Integer = {0}, StartY() As Integer = {0}
        Dim LocationCount As Integer
        Dim TotalPaths As Integer
        Dim MaxTries As Integer
        Dim Tries As Integer
        Dim tick As Integer

        Console.WriteLine("Working...")
        Application.DoEvents()
        tick = GetTimeMs()

        MaxTries = 30
        TotalPaths = Random(Map(MapNum).MaxX / 20, Map(MapNum).MaxX / 10)

        Do While LocationCount < TotalPaths AndAlso Tries < MaxTries
            x = Random(1, Map(MapNum).MaxX - 1)
            y = Random(1, Map(MapNum).MaxY - 1)
            If MapOrientation(MapNum).Tile(x, y) = TilePrefab.Grass AndAlso MapOrientation(MapNum).Tile(x + 1, y) = TilePrefab.Grass AndAlso MapOrientation(MapNum).Tile(x, y + 1) = TilePrefab.Grass AndAlso MapOrientation(MapNum).Tile(x + 1, y + 1) = TilePrefab.Grass Then
                If Not SearchForPreviousPaths(MapNum, x, y) Then
                    PaintTile(TilePrefab.Passing, MapNum, x, y, 1, 1, , TilePrefab.Grass)
                    LocationCount = LocationCount + 1
                    ReDim Preserve StartX(LocationCount)
                    ReDim Preserve StartY(LocationCount)
                    StartX(LocationCount) = x
                    StartY(LocationCount) = y
                End If
            End If
            Tries = Tries + 1
        Loop

        If LocationCount > 0 Then
            Dim i As Integer
            Dim Dir As Integer
            For i = 1 To LocationCount
                If StartX(i) < Map(MapNum).MaxX / 2 Then
                    If StartY(i) < Map(MapNum).MaxY / 2 Then
                        If Random(1, 2) = 1 Then
                            Dir = DirectionType.Left
                        Else
                            Dir = DirectionType.Up
                        End If
                    Else
                        If Random(1, 2) = 1 Then
                            Dir = DirectionType.Left
                        Else
                            Dir = DirectionType.Down
                        End If
                    End If
                Else
                    If StartY(i) < Map(MapNum).MaxY / 2 Then
                        If Random(1, 2) = 1 Then
                            Dir = DirectionType.Right
                        Else
                            Dir = DirectionType.Up
                        End If
                    Else
                        If Random(1, 2) = 1 Then
                            Dir = DirectionType.Right
                        Else
                            Dir = DirectionType.Down
                        End If
                    End If
                End If
                MakePath(MapNum, StartX(i) + 1, StartY(i), Dir)
            Next i
        End If

        tick = GetTimeMs() - tick
        Console.WriteLine("Done " & TotalPaths & " paths in " & CDbl(tick / 1000) & "s")
        Application.DoEvents()
    End Sub

    Sub MakePaths(MapStart As Integer, Size As Integer)
        Dim TotalMaps As Integer
        TotalMaps = Size * Size

        If TotalMaps Mod 2 = 1 Then
            MakeMapPaths(Int(TotalMaps / 2) + 1)
        Else
            MakeMapPaths(Int(TotalMaps / 2) - (Size / 2))
            MakeMapPaths(Int(TotalMaps / 2) - (Size / 2) + 1)
            MakeMapPaths(Int(TotalMaps / 2) - (Size / 2) + Size)
            MakeMapPaths(Int(TotalMaps / 2) - (Size / 2) + Size + 1)
        End If

    End Sub

    Sub StartAutomapper(MapStart As Integer, Size As Integer, MapX As Integer, MapY As Integer)
        Dim StartTick As Integer
        Dim tick As Integer
        StartTick = GetTimeMs()
        LoadTilePrefab()
        LoadDetails()

        Dim mapNum as Integer = MapStart
        Dim TotalMaps As Integer
        TotalMaps = (Size * Size)

        ReDim MapOrientation(MapStart + TotalMaps)

        tick = GetTimeMs()

        For mapnum = MapStart To MapStart + TotalMaps - 1
            ClearMap(mapnum)

            Map(mapnum).MaxX = MapX
            Map(mapnum).MaxY = MapY
            ReDim Map(mapnum).Tile(Map(mapnum).MaxX,Map(mapnum).MaxY)
            ReDim MapOrientation(mapnum).Tile(Map(mapnum).MaxX,Map(mapnum).MaxY)
            ClearTempTile(mapnum)

            ' // Down teleport \\
            If mapnum <= MapStart - 1 + TotalMaps - Size Then
                Map(mapnum).Down = mapnum + Size
            End If
            ' \\ Down teleport //

            ' // Left teleport \\
            If mapnum - MapStart + 1 Mod Size <> 1 Then
                Map(mapnum).Left = mapnum - 1
            End If
            ' \\ Left teleport //

            ' // Up teleport \\
            If mapnum - MapStart + 1 > Size Then
                Map(mapnum).Up = mapnum - Size
            End If
            ' \\ Up teleport //

            ' // Right teleport \\
            If mapnum - MapStart + 1 Mod Size <> 0 Then
                Map(mapnum).Right = mapnum + 1
            End If
            ' \\ Right teleport //

            Dim Prefab As MapPrefab
            Prefab = MapPrefab.Undefined
            If mapnum = MapStart Then
                Prefab = MapPrefab.UpLeftQuarter
            End If
            If mapnum > MapStart AndAlso mapnum < MapStart - 1 + Size Then
                Prefab = MapPrefab.UpBorder
            End If
            If mapnum = MapStart - 1 + Size Then
                Prefab = MapPrefab.UpRightQuarter
            End If
            If mapnum > MapStart - 1 + Size AndAlso mapnum <= MapStart - 1 + TotalMaps - Size Then
                If (mapnum - MapStart + 1) Mod Size = 1 Then
                    Prefab = MapPrefab.LeftBorder
                Else
                    If (mapnum - MapStart + 1) Mod Size = 0 Then
                        Prefab = MapPrefab.RightBorder
                    Else
                        Prefab = MapPrefab.Common
                    End If
                End If
            End If
            If mapnum > MapStart - 1 + TotalMaps - Size Then
                If (mapnum - MapStart + 1) Mod Size = 1 Then
                    Prefab = MapPrefab.DownLeftQuarter
                Else
                    If (mapnum - MapStart + 1) Mod Size = 0 Then
                        Prefab = MapPrefab.DownRightQuarter
                    Else
                        Prefab = MapPrefab.BottomBorder
                    End If
                End If
            End If

            MakeMap(mapnum, Prefab)
        Next mapnum

        tick = GetTimeMs() - tick
        Console.WriteLine("Done " & TotalMaps & " maps models in " & CDbl(tick / 1000) & "s")
        Application.DoEvents()

        If PathsChecked = True Then MakePaths(MapStart, Size)
        If RiversChecked = True Then MakeRivers(MapStart, Size)
        If MountainsChecked = True Then MakeMountains(MapStart, Size)
        If OverGrassChecked = True Then MakeOvergrasses(MapStart, Size)
        If ResourcesChecked = True Then MakeResources(MapStart, Size)

        tick = GetTimeMs()
        Console.WriteLine("Working...")
        Application.DoEvents()

        For mapnum = MapStart To MapStart + TotalMaps - 1
            SaveMap(mapnum)
            'MapCache_Create mapnum
        Next mapnum

        tick = GetTimeMs() - tick
        StartTick = GetTimeMs() - StartTick

        Console.WriteLine("Cached all maps in " & CDbl(tick / 1000) & "s (" & ((tick / StartTick) * 100) & "%)")
        Console.WriteLine("Done " & TotalMaps & " maps in " & CDbl(StartTick / 1000) & "s")

    End Sub

End Module
