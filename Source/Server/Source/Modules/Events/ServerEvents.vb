Imports System.IO
Imports ASFW

Public Module ServerEvents
#Region "Globals"
    Public TempEventMap() As GlobalEventsStruct
    Public Switches() As String
    Public Variables() As String

    Public Const MAX_SWITCHES As Integer = 500
    Public Const MAX_VARIABLES As Integer = 500

    Public Const PathfindingType As Integer = 1

    'Effect Constants - Used for event options...
    Public Const EFFECT_TYPE_FADEIN As Integer = 2
    Public Const EFFECT_TYPE_FADEOUT As Integer = 1
    Public Const EFFECT_TYPE_FLASH As Integer = 3
    Public Const EFFECT_TYPE_FOG As Integer = 4
    Public Const EFFECT_TYPE_WEATHER As Integer = 5
    Public Const EFFECT_TYPE_TINT As Integer = 6
#End Region

#Region "Structures"
    Structure MoveRouteStruct
        Dim Index As Integer
        Dim Data1 As Integer
        Dim Data2 As Integer
        Dim Data3 As Integer
        Dim Data4 As Integer
        Dim Data5 As Integer
        Dim Data6 As Integer
    End Structure

    Structure GlobalEventStruct
        Dim X As Integer
        Dim Y As Integer
        Dim Dir As Integer
        Dim Active As Integer

        Dim WalkingAnim As Integer
        Dim FixedDir As Integer
        Dim WalkThrough As Integer
        Dim ShowName As Integer

        Dim Position As Integer

        Dim GraphicType As Integer
        Dim GraphicNum As Integer
        Dim GraphicX As Integer
        Dim GraphicX2 As Integer
        Dim GraphicY As Integer
        Dim GraphicY2 As Integer

        'Server Only Options
        Dim MoveType As Integer
        Dim MoveSpeed As Integer
        Dim MoveFreq As Integer
        Dim MoveRouteCount As Integer
        Dim MoveRoute() As MoveRouteStruct
        Dim MoveRouteStep As Integer

        Dim RepeatMoveRoute As Integer
        Dim IgnoreIfCannotMove As Integer

        Dim MoveTimer As Integer
        Dim QuestNum As Integer
        Dim MoveRouteComplete As Integer
    End Structure

    Structure GlobalEventsStruct
        Dim EventCount As Integer
        Dim Events() As GlobalEventStruct
    End Structure

    Public Structure ConditionalBranchStruct
        Dim Condition As Integer
        Dim Data1 As Integer
        Dim Data2 As Integer
        Dim Data3 As Integer
        Dim CommandList As Integer
        Dim ElseCommandList As Integer
    End Structure

    Structure EventCommandStruct
        Dim Index As Byte
        Dim Text1 As String
        Dim Text2 As String
        Dim Text3 As String
        Dim Text4 As String
        Dim Text5 As String
        Dim Data1 As Integer
        Dim Data2 As Integer
        Dim Data3 As Integer
        Dim Data4 As Integer
        Dim Data5 As Integer
        Dim Data6 As Integer
        Dim ConditionalBranch As ConditionalBranchStruct
        Dim MoveRouteCount As Integer
        Dim MoveRoute() As MoveRouteStruct
    End Structure

    Structure CommandListStruct
        Dim CommandCount As Integer
        Dim ParentList As Integer
        Dim Commands() As EventCommandStruct
    End Structure

    Structure EventPageStruct
        'These are condition variables that decide if the event even appears to the player.
        Dim chkVariable As Integer
        Dim VariableIndex As Integer
        Dim VariableCondition As Integer
        Dim VariableCompare As Integer

        Dim chkSwitch As Integer
        Dim SwitchIndex As Integer
        Dim SwitchCompare As Integer

        Dim chkHasItem As Integer
        Dim HasItemIndex As Integer
        Dim HasItemAmount As Integer

        Dim chkSelfSwitch As Integer
        Dim SelfSwitchIndex As Integer
        Dim SelfSwitchCompare As Integer
        Dim chkPlayerGender As Integer
        'End Conditions

        'Handles the Event Sprite
        Dim GraphicType As Byte
        Dim Graphic As Integer
        Dim GraphicX As Integer
        Dim GraphicY As Integer
        Dim GraphicX2 As Integer
        Dim GraphicY2 As Integer

        'Handles Movement - Move Routes to come soon.
        Dim MoveType As Byte
        Dim MoveSpeed As Byte
        Dim MoveFreq As Byte
        Dim MoveRouteCount As Integer
        Dim MoveRoute() As MoveRouteStruct
        Dim IgnoreMoveRoute As Integer
        Dim RepeatMoveRoute As Integer

        'Guidelines for the event
        Dim WalkAnim As Integer
        Dim DirFix As Integer
        Dim WalkThrough As Integer
        Dim ShowName As Integer

        'Trigger for the event
        Dim Trigger As Byte

        'Commands for the event
        Dim CommandListCount As Integer
        Dim CommandList() As CommandListStruct

        Dim Position As Byte

        Dim QuestNum As Integer

        'For EventMap
        Dim X As Integer
        Dim Y As Integer
    End Structure

    Structure EventStruct
        Dim Name As String
        Dim Globals As Byte
        Dim PageCount As Integer
        Dim Pages() As EventPageStruct
        Dim X As Integer
        Dim Y As Integer
        'Self Switches re-set on restart.
        Dim SelfSwitches() As Integer '0 to 4
    End Structure

    Public Structure GlobalMapEventsStruct
        Dim EventID As Integer
        Dim PageID As Integer
        Dim X As Integer
        Dim Y As Integer
    End Structure

    Structure MapEventStruct
        Dim Dir As Integer
        Dim X As Integer
        Dim Y As Integer

        Dim WalkingAnim As Integer
        Dim FixedDir As Integer
        Dim WalkThrough As Integer
        Dim ShowName As Integer

        Dim GraphicType As Integer
        Dim GraphicX As Integer
        Dim GraphicY As Integer
        Dim GraphicX2 As Integer
        Dim GraphicY2 As Integer
        Dim GraphicNum As Integer

        Dim MovementSpeed As Integer
        Dim Position As Integer
        Dim Visible As Integer
        Dim EventID As Integer
        Dim PageID As Integer

        'Server Only Options
        Dim MoveType As Integer
        Dim MoveSpeed As Integer
        Dim MoveFreq As Integer
        Dim MoveRouteCount As Integer
        Dim MoveRoute() As MoveRouteStruct
        Dim MoveRouteStep As Integer

        Dim RepeatMoveRoute As Integer
        Dim IgnoreIfCannotMove As Integer
        Dim QuestNum As Integer

        Dim MoveTimer As Integer
        Dim SelfSwitches() As Integer '0 to 4
        Dim MoveRouteComplete As Integer
    End Structure

    Structure EventMapStruct
        Dim CurrentEvents As Integer
        Dim EventPages() As MapEventStruct
    End Structure

    Structure EventProcessingStruct
        Dim Active As Integer
        Dim CurList As Integer
        Dim CurSlot As Integer
        Dim EventID As Integer
        Dim PageID As Integer
        Dim WaitingForResponse As Integer
        Dim EventMovingID As Integer
        Dim EventMovingType As Integer
        Dim ActionTimer As Integer
        Dim ListLeftOff() As Integer
    End Structure
#End Region

#Region "Enums"
    Public Enum MoveRouteOpts
        MoveUp = 1
        MoveDown
        MoveLeft
        MoveRight
        MoveRandom
        MoveTowardsPlayer
        MoveAwayFromPlayer
        StepForward
        StepBack
        Wait100ms
        Wait500ms
        Wait1000ms
        TurnUp
        TurnDown
        TurnLeft
        TurnRight
        Turn90Right
        Turn90Left
        Turn180
        TurnRandom
        TurnTowardPlayer
        TurnAwayFromPlayer
        SetSpeed8xSlower
        SetSpeed4xSlower
        SetSpeed2xSlower
        SetSpeedNormal
        SetSpeed2xFaster
        SetSpeed4xFaster
        SetFreqLowest
        SetFreqLower
        SetFreqNormal
        SetFreqHigher
        SetFreqHighest
        WalkingAnimOn
        WalkingAnimOff
        DirFixOn
        DirFixOff
        WalkThroughOn
        WalkThroughOff
        PositionBelowPlayer
        PositionWithPlayer
        PositionAbovePlayer
        ChangeGraphic
    End Enum

    ' Event Types
    Public Enum EventType
        ' Message
        evAddText = 1
        evShowText
        evShowChoices
        ' Game Progression
        evPlayerVar
        evPlayerSwitch
        evSelfSwitch
        ' Flow Control
        evCondition
        evExitProcess
        ' Player
        evChangeItems
        evRestoreHP
        evRestoreMP
        evLevelUp
        evChangeLevel
        evChangeSkills
        evChangeClass
        evChangeSprite
        evChangeSex
        evChangePK
        ' Movement
        evWarpPlayer
        evSetMoveRoute
        ' Character
        evPlayAnimation
        ' Music and Sounds
        evPlayBGM
        evFadeoutBGM
        evPlaySound
        evStopSound
        'Etc...
        evCustomScript
        evSetAccess
        'Shop/Bank
        evOpenBank
        evOpenShop
        'New
        evGiveExp
        evShowChatBubble
        evLabel
        evGotoLabel
        evSpawnNpc
        evFadeIn
        evFadeOut
        evFlashWhite
        evSetFog
        evSetWeather
        evSetTint
        evWait
        evOpenMail
        evBeginQuest
        evEndQuest
        evQuestTask
        evShowPicture
        evHidePicture
        evWaitMovement
        evHoldPlayer
        evReleasePlayer
    End Enum
#End Region

#Region "Database"
    Sub CreateSwitches()
        Dim i As Integer
        Dim myXml As New XmlClass With {
            .Filename = Path.Combine(Application.StartupPath, "Data", "Switches.xml"),
            .Root = "Data"
        }

        myXml.NewXmlDocument()

        For i = 1 To MAX_SWITCHES
            Switches(i) = ""
        Next

        SaveSwitches()
    End Sub

    Sub CreateVariables()
        Dim i As Integer
        Dim myXml As New XmlClass With {
            .Filename = Path.Combine(Application.StartupPath, "Data", "Variables.xml"),
            .Root = "Data"
        }

        myXml.NewXmlDocument()

        For i = 1 To MAX_VARIABLES
            Variables(i) = ""
        Next

        SaveVariables()
    End Sub

    Sub SaveSwitches()
        Dim i As Integer
        Dim myXml As New XmlClass With {
            .Filename = Path.Combine(Application.StartupPath, "Data", "Switches.xml"),
            .Root = "Data"
        }

        myXml.LoadXml()

        For i = 1 To MAX_SWITCHES
            myXml.WriteString("Switches", "Switch" & i & "Name", Switches(i))
        Next

        myXml.CloseXml(True)
    End Sub

    Sub SaveVariables()
        Dim i As Integer
        Dim myXml As New XmlClass With {
            .Filename = Path.Combine(Application.StartupPath, "Data", "Variables.xml"),
            .Root = "Data"
        }

        myXml.LoadXml()

        For i = 1 To MAX_VARIABLES
            myXml.WriteString("Variables", "Variable" & i & "Name", Variables(i))
        Next

        myXml.CloseXml(True)
    End Sub

    Sub LoadSwitches()
        Dim i As Integer
        Dim myXml As New XmlClass With {
            .Filename = Path.Combine(Application.StartupPath, "Data", "Switches.xml"),
            .Root = "Data"
        }

        If Not FileExist(myXml.Filename) Then
            CreateSwitches()
            Exit Sub
        End If

        myXml.LoadXml()

        For i = 1 To MAX_SWITCHES
            Switches(i) = myXml.ReadString("Switches", "Switch" & i & "Name")
        Next

        myXml.CloseXml(False)
    End Sub

    Sub LoadVariables()
        Dim i As Integer
        Dim myXml As New XmlClass With {
            .Filename = Path.Combine(Application.StartupPath, "Data", "Variables.xml"),
            .Root = "Data"
        }

        If Not FileExist(myXml.Filename) Then
            CreateVariables()
            Exit Sub
        End If

        myXml.LoadXml()

        For i = 1 To MAX_VARIABLES
            Variables(i) = myXml.ReadString("Variables", "Variable" & i & "Name")
        Next

        myXml.CloseXml(False)

    End Sub
#End Region

#Region "Movement"
    Function CanEventMove(Index As Integer, MapNum As Integer, x As Integer, y As Integer, eventID As Integer, WalkThrough As Integer, Dir As Byte, Optional globalevent As Boolean = False) As Boolean
        Dim i As Integer
        Dim n As Integer, z As Integer, begineventprocessing As Boolean

        ' Check for subscript out of range

        If MapNum <= 0 Or MapNum > MAX_MAPS Or Dir < Direction.Up Or Dir > Direction.Right Then Exit Function

        If Gettingmap = True Then Exit Function

        CanEventMove = True

        If Index = 0 Then Exit Function

        Select Case Dir
            Case Direction.Up

                ' Check to make sure not outside of boundries
                If y > 0 Then
                    n = Map(MapNum).Tile(x, y - 1).Type

                    If WalkThrough = 1 Then
                        CanEventMove = True
                        Exit Function
                    End If


                    ' Check to make sure that the tile is walkable
                    If n = TileType.Blocked Then
                        CanEventMove = False
                        Exit Function
                    End If

                    If n <> TileType.None And n <> TileType.Item And n <> TileType.NpcSpawn Then
                        CanEventMove = False
                        Exit Function
                    End If

                    ' Check to make sure that there is not a player in the way
                    For i = 0 To Socket.HighIndex
                        If IsPlaying(i) Then
                            If (GetPlayerMap(i) = MapNum) And (GetPlayerX(i) = x) And (GetPlayerY(i) = y - 1) Then
                                CanEventMove = False
                                'There IS a player in the way. But now maybe we can call the event touch thingy!
                                If Map(MapNum).Events(eventID).Pages(TempPlayer(Index).EventMap.EventPages(eventID).PageID).Trigger = 1 Then
                                    begineventprocessing = True
                                    If begineventprocessing = True Then
                                        'Process this event, it is on-touch and everything checks out.
                                        If Map(MapNum).Events(eventID).Pages(TempPlayer(Index).EventMap.EventPages(eventID).PageID).CommandListCount > 0 Then
                                            TempPlayer(Index).EventProcessing(eventID).Active = 1
                                            TempPlayer(Index).EventProcessing(eventID).ActionTimer = GetTimeMs()
                                            TempPlayer(Index).EventProcessing(eventID).CurList = 1
                                            TempPlayer(Index).EventProcessing(eventID).CurSlot = 1
                                            TempPlayer(Index).EventProcessing(eventID).EventID = eventID
                                            TempPlayer(Index).EventProcessing(eventID).PageID = TempPlayer(Index).EventMap.EventPages(eventID).PageID
                                            TempPlayer(Index).EventProcessing(eventID).WaitingForResponse = 0
                                            ReDim TempPlayer(Index).EventProcessing(eventID).ListLeftOff(0 To Map(GetPlayerMap(Index)).Events(TempPlayer(Index).EventMap.EventPages(eventID).EventID).Pages(TempPlayer(Index).EventMap.EventPages(eventID).PageID).CommandListCount)
                                        End If
                                        begineventprocessing = False
                                    End If
                                End If
                            End If
                        End If
                    Next

                    If CanEventMove = False Then Exit Function
                    ' Check to make sure that there is not another npc in the way
                    For i = 1 To MAX_MAP_NPCS
                        If (MapNpc(MapNum).Npc(i).X = x) And (MapNpc(MapNum).Npc(i).Y = y - 1) Then
                            CanEventMove = False
                            Exit Function
                        End If
                    Next

                    If globalevent = True And TempEventMap(MapNum).EventCount > 0 Then
                        For z = 1 To TempEventMap(MapNum).EventCount
                            If (z <> eventID) And (z > 0) And (TempEventMap(MapNum).Events(z).X = x) And (TempEventMap(MapNum).Events(z).Y = y - 1) And (TempEventMap(MapNum).Events(z).WalkThrough = 0) Then
                                CanEventMove = False
                                Exit Function
                            End If
                        Next
                    Else
                        If TempPlayer(Index).EventMap.CurrentEvents > 0 Then
                            For z = 1 To TempPlayer(Index).EventMap.CurrentEvents
                                If (TempPlayer(Index).EventMap.EventPages(z).EventID <> eventID) And (eventID > 0) And (TempPlayer(Index).EventMap.EventPages(z).X = TempPlayer(Index).EventMap.EventPages(eventID).X) And (TempPlayer(Index).EventMap.EventPages(z).Y = TempPlayer(Index).EventMap.EventPages(eventID).Y - 1) And (TempPlayer(Index).EventMap.EventPages(z).WalkThrough = 0) Then
                                    CanEventMove = False
                                    Exit Function
                                End If
                            Next
                        End If
                    End If

                    ' Directional blocking
                    If IsDirBlocked(Map(MapNum).Tile(x, y).DirBlock, Direction.Up + 1) Then
                        CanEventMove = False
                        Exit Function
                    End If
                Else
                    CanEventMove = False
                End If

            Case Direction.Down

                ' Check to make sure not outside of boundries
                If y < Map(MapNum).MaxY Then
                    n = Map(MapNum).Tile(x, y + 1).Type

                    If WalkThrough = 1 Then
                        CanEventMove = True
                        Exit Function
                    End If

                    ' Check to make sure that the tile is walkable
                    If n = TileType.Blocked Then
                        CanEventMove = False
                        Exit Function
                    End If

                    If n <> TileType.None And n <> TileType.Item And n <> TileType.NpcSpawn Then
                        CanEventMove = False
                        Exit Function
                    End If

                    ' Check to make sure that there is not a player in the way
                    For i = 0 To Socket.HighIndex
                        If IsPlaying(i) Then
                            If (GetPlayerMap(i) = MapNum) And (GetPlayerX(i) = x) And (GetPlayerY(i) = y + 1) Then
                                CanEventMove = False
                                'There IS a player in the way. But now maybe we can call the event touch thingy!
                                If Map(MapNum).Events(eventID).Pages(TempPlayer(Index).EventMap.EventPages(eventID).PageID).Trigger = 1 Then
                                    begineventprocessing = True
                                    If begineventprocessing = True Then
                                        'Process this event, it is on-touch and everything checks out.
                                        If Map(MapNum).Events(eventID).Pages(TempPlayer(Index).EventMap.EventPages(eventID).PageID).CommandListCount > 0 Then
                                            TempPlayer(Index).EventProcessing(eventID).Active = 1
                                            TempPlayer(Index).EventProcessing(eventID).ActionTimer = GetTimeMs()
                                            TempPlayer(Index).EventProcessing(eventID).CurList = 1
                                            TempPlayer(Index).EventProcessing(eventID).CurSlot = 1
                                            TempPlayer(Index).EventProcessing(eventID).EventID = eventID
                                            TempPlayer(Index).EventProcessing(eventID).PageID = TempPlayer(Index).EventMap.EventPages(eventID).PageID
                                            TempPlayer(Index).EventProcessing(eventID).WaitingForResponse = 0
                                            ReDim TempPlayer(Index).EventProcessing(eventID).ListLeftOff(0 To Map(GetPlayerMap(Index)).Events(TempPlayer(Index).EventMap.EventPages(eventID).EventID).Pages(TempPlayer(Index).EventMap.EventPages(eventID).PageID).CommandListCount)
                                        End If
                                        begineventprocessing = False
                                    End If
                                End If
                            End If
                        End If
                    Next

                    If CanEventMove = False Then Exit Function

                    ' Check to make sure that there is not another npc in the way
                    For i = 1 To MAX_MAP_NPCS
                        If (MapNpc(MapNum).Npc(i).X = x) And (MapNpc(MapNum).Npc(i).Y = y + 1) Then
                            CanEventMove = False
                            Exit Function
                        End If
                    Next

                    If globalevent = True And TempEventMap(MapNum).EventCount > 0 Then
                        For z = 1 To TempEventMap(MapNum).EventCount
                            If (z <> eventID) And (z > 0) And (TempEventMap(MapNum).Events(z).X = x) And (TempEventMap(MapNum).Events(z).Y = y + 1) And (TempEventMap(MapNum).Events(z).WalkThrough = 0) Then
                                CanEventMove = False
                                Exit Function
                            End If
                        Next
                    Else
                        If TempPlayer(Index).EventMap.CurrentEvents > 0 Then
                            For z = 1 To TempPlayer(Index).EventMap.CurrentEvents
                                If (TempPlayer(Index).EventMap.EventPages(z).EventID <> eventID) And (eventID > 0) And (TempPlayer(Index).EventMap.EventPages(z).X = TempPlayer(Index).EventMap.EventPages(eventID).X) And (TempPlayer(Index).EventMap.EventPages(z).Y = TempPlayer(Index).EventMap.EventPages(eventID).Y + 1) And (TempPlayer(Index).EventMap.EventPages(z).WalkThrough = 0) Then
                                    CanEventMove = False
                                    Exit Function
                                End If
                            Next
                        End If
                    End If

                    ' Directional blocking
                    If IsDirBlocked(Map(MapNum).Tile(x, y).DirBlock, Direction.Down + 1) Then
                        CanEventMove = False
                        Exit Function
                    End If
                Else
                    CanEventMove = False
                End If

            Case Direction.Left

                ' Check to make sure not outside of boundries
                If x > 0 Then
                    n = Map(MapNum).Tile(x - 1, y).Type

                    If WalkThrough = 1 Then
                        CanEventMove = True
                        Exit Function
                    End If

                    ' Check to make sure that the tile is walkable
                    If n = TileType.Blocked Then
                        CanEventMove = False
                        Exit Function
                    End If

                    If n <> TileType.None And n <> TileType.Item And n <> TileType.NpcSpawn Then
                        CanEventMove = False
                        Exit Function
                    End If

                    ' Check to make sure that there is not a player in the way
                    For i = 0 To Socket.HighIndex
                        If IsPlaying(i) Then
                            If (GetPlayerMap(i) = MapNum) And (GetPlayerX(i) = x - 1) And (GetPlayerY(i) = y) Then
                                CanEventMove = False
                                'There IS a player in the way. But now maybe we can call the event touch thingy!
                                If Map(MapNum).Events(eventID).Pages(TempPlayer(Index).EventMap.EventPages(eventID).PageID).Trigger = 1 Then
                                    begineventprocessing = True
                                    If begineventprocessing = True Then
                                        'Process this event, it is on-touch and everything checks out.
                                        If Map(MapNum).Events(eventID).Pages(TempPlayer(Index).EventMap.EventPages(eventID).PageID).CommandListCount > 0 Then
                                            TempPlayer(Index).EventProcessing(eventID).Active = 1
                                            TempPlayer(Index).EventProcessing(eventID).ActionTimer = GetTimeMs()
                                            TempPlayer(Index).EventProcessing(eventID).CurList = 1
                                            TempPlayer(Index).EventProcessing(eventID).CurSlot = 1
                                            TempPlayer(Index).EventProcessing(eventID).EventID = eventID
                                            TempPlayer(Index).EventProcessing(eventID).PageID = TempPlayer(Index).EventMap.EventPages(eventID).PageID
                                            TempPlayer(Index).EventProcessing(eventID).WaitingForResponse = 0
                                            ReDim TempPlayer(Index).EventProcessing(eventID).ListLeftOff(0 To Map(GetPlayerMap(Index)).Events(TempPlayer(Index).EventMap.EventPages(eventID).EventID).Pages(TempPlayer(Index).EventMap.EventPages(eventID).PageID).CommandListCount)
                                        End If
                                        begineventprocessing = False
                                    End If
                                End If
                            End If
                        End If
                    Next

                    If CanEventMove = False Then Exit Function

                    ' Check to make sure that there is not another npc in the way
                    For i = 1 To MAX_MAP_NPCS
                        If (MapNpc(MapNum).Npc(i).X = x - 1) And (MapNpc(MapNum).Npc(i).Y = y) Then
                            CanEventMove = False
                            Exit Function
                        End If
                    Next

                    If globalevent = True And TempEventMap(MapNum).EventCount > 0 Then
                        For z = 1 To TempEventMap(MapNum).EventCount
                            If (z <> eventID) And (z > 0) And (TempEventMap(MapNum).Events(z).X = x - 1) And (TempEventMap(MapNum).Events(z).Y = y) And (TempEventMap(MapNum).Events(z).WalkThrough = 0) Then
                                CanEventMove = False
                                Exit Function
                            End If
                        Next
                    Else
                        If TempPlayer(Index).EventMap.CurrentEvents > 0 Then
                            For z = 1 To TempPlayer(Index).EventMap.CurrentEvents
                                If (TempPlayer(Index).EventMap.EventPages(z).EventID <> eventID) And (eventID > 0) And (TempPlayer(Index).EventMap.EventPages(z).X = TempPlayer(Index).EventMap.EventPages(eventID).X - 1) And (TempPlayer(Index).EventMap.EventPages(z).Y = TempPlayer(Index).EventMap.EventPages(eventID).Y) And (TempPlayer(Index).EventMap.EventPages(z).WalkThrough = 0) Then
                                    CanEventMove = False
                                    Exit Function
                                End If
                            Next
                        End If
                    End If

                    ' Directional blocking
                    If IsDirBlocked(Map(MapNum).Tile(x, y).DirBlock, Direction.Left + 1) Then
                        CanEventMove = False
                        Exit Function
                    End If
                Else
                    CanEventMove = False
                End If

            Case Direction.Right

                ' Check to make sure not outside of boundries
                If x < Map(MapNum).MaxX Then
                    n = Map(MapNum).Tile(x + 1, y).Type

                    If WalkThrough = 1 Then
                        CanEventMove = True
                        Exit Function
                    End If

                    ' Check to make sure that the tile is walkable
                    If n = TileType.Blocked Then
                        CanEventMove = False
                        Exit Function
                    End If

                    If n <> TileType.None And n <> TileType.Item And n <> TileType.NpcSpawn Then
                        CanEventMove = False
                        Exit Function
                    End If

                    ' Check to make sure that there is not a player in the way
                    For i = 0 To Socket.HighIndex
                        If IsPlaying(i) Then
                            If (GetPlayerMap(i) = MapNum) And (GetPlayerX(i) = x + 1) And (GetPlayerY(i) = y) Then
                                CanEventMove = False
                                'There IS a player in the way. But now maybe we can call the event touch thingy!
                                If Map(MapNum).Events(eventID).Pages(TempPlayer(Index).EventMap.EventPages(eventID).PageID).Trigger = 1 Then
                                    begineventprocessing = True
                                    If begineventprocessing = True Then
                                        'Process this event, it is on-touch and everything checks out.
                                        If Map(MapNum).Events(eventID).Pages(TempPlayer(Index).EventMap.EventPages(eventID).PageID).CommandListCount > 0 Then
                                            TempPlayer(Index).EventProcessing(eventID).Active = 1
                                            TempPlayer(Index).EventProcessing(eventID).ActionTimer = GetTimeMs()
                                            TempPlayer(Index).EventProcessing(eventID).CurList = 1
                                            TempPlayer(Index).EventProcessing(eventID).CurSlot = 1
                                            TempPlayer(Index).EventProcessing(eventID).EventID = eventID
                                            TempPlayer(Index).EventProcessing(eventID).PageID = TempPlayer(Index).EventMap.EventPages(eventID).PageID
                                            TempPlayer(Index).EventProcessing(eventID).WaitingForResponse = 0
                                            ReDim TempPlayer(Index).EventProcessing(eventID).ListLeftOff(0 To Map(GetPlayerMap(Index)).Events(TempPlayer(Index).EventMap.EventPages(eventID).EventID).Pages(TempPlayer(Index).EventMap.EventPages(eventID).PageID).CommandListCount)
                                        End If
                                        begineventprocessing = False
                                    End If
                                End If
                            End If
                        End If
                    Next

                    If CanEventMove = False Then Exit Function

                    ' Check to make sure that there is not another npc in the way
                    For i = 1 To MAX_MAP_NPCS
                        If (MapNpc(MapNum).Npc(i).X = x + 1) And (MapNpc(MapNum).Npc(i).Y = y) Then
                            CanEventMove = False
                            Exit Function
                        End If
                    Next

                    If globalevent = True And TempEventMap(MapNum).EventCount > 0 Then
                        For z = 1 To TempEventMap(MapNum).EventCount
                            If (z <> eventID) And (z > 0) And (TempEventMap(MapNum).Events(z).X = x + 1) And (TempEventMap(MapNum).Events(z).Y = y) And (TempEventMap(MapNum).Events(z).WalkThrough = 0) Then
                                CanEventMove = False
                                Exit Function
                            End If
                        Next
                    Else
                        If TempPlayer(Index).EventMap.CurrentEvents > 0 Then
                            For z = 1 To TempPlayer(Index).EventMap.CurrentEvents
                                If (TempPlayer(Index).EventMap.EventPages(z).EventID <> eventID) And (eventID > 0) And (TempPlayer(Index).EventMap.EventPages(z).X = TempPlayer(Index).EventMap.EventPages(eventID).X + 1) And (TempPlayer(Index).EventMap.EventPages(z).Y = TempPlayer(Index).EventMap.EventPages(eventID).Y) And (TempPlayer(Index).EventMap.EventPages(z).WalkThrough = 0) Then
                                    CanEventMove = False
                                    Exit Function
                                End If
                            Next
                        End If
                    End If

                    ' Directional blocking
                    If IsDirBlocked(Map(MapNum).Tile(x, y).DirBlock, Direction.Right + 1) Then
                        CanEventMove = False
                        Exit Function
                    End If
                Else
                    CanEventMove = False
                End If

        End Select

    End Function

    Sub EventDir(PlayerIndex As Integer, MapNum As Integer, eventID As Integer, Dir As Integer, Optional globalevent As Boolean = False)
        Dim Buffer As New ByteStream(4)
        Dim eventIndex As Integer, i As Integer

        ' Check for subscript out of range

        If Gettingmap = True Then Exit Sub

        If MapNum <= 0 Or MapNum > MAX_MAPS Or Dir < Direction.Up Or Dir > Direction.Right Then
            Exit Sub
        End If

        If globalevent = False Then
            If TempPlayer(PlayerIndex).EventMap.CurrentEvents > 0 Then
                For i = 1 To TempPlayer(PlayerIndex).EventMap.CurrentEvents
                    If eventID = i Then
                        eventIndex = eventID
                        eventID = TempPlayer(PlayerIndex).EventMap.EventPages(i).EventID
                        Exit For
                    End If
                Next
            End If

            If eventIndex = 0 Or eventID = 0 Then Exit Sub
        End If

        If globalevent Then
            If Map(MapNum).Events(eventID).Pages(1).DirFix = 0 Then TempEventMap(MapNum).Events(eventID).Dir = Dir
        Else
            If Map(MapNum).Events(eventID).Pages(TempPlayer(PlayerIndex).EventMap.EventPages(eventIndex).PageID).DirFix = 0 Then TempPlayer(PlayerIndex).EventMap.EventPages(eventIndex).Dir = Dir
        End If

        Buffer.WriteInt32(ServerPackets.SEventDir)
        Buffer.WriteInt32(eventID)

        Addlog("Sent SMSG: SEventDir", PACKET_LOG)
        TextAdd("Sent SMSG: SEventDir")

        If globalevent Then
            Buffer.WriteInt32(TempEventMap(MapNum).Events(eventID).Dir)
        Else
            Buffer.WriteInt32(TempPlayer(PlayerIndex).EventMap.EventPages(eventIndex).Dir)
        End If

        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)

        Buffer.Dispose

    End Sub

    Sub EventMove(Index As Integer, MapNum As Integer, eventID As Integer, Dir As Integer, movementspeed As Integer, Optional globalevent As Boolean = False)
        Dim Buffer As New ByteStream(4)
        Dim eventIndex As Integer, i As Integer

        ' Check for subscript out of range
        If Gettingmap = True Then Exit Sub

        If MapNum <= 0 Or MapNum > MAX_MAPS Or Dir < Direction.Up Or Dir > Direction.Right Then Exit Sub

        If globalevent = False Then
            If TempPlayer(Index).EventMap.CurrentEvents > 0 Then
                For i = 1 To TempPlayer(Index).EventMap.CurrentEvents
                    If eventID = i Then
                        eventIndex = eventID
                        eventID = TempPlayer(Index).EventMap.EventPages(i).EventID
                        Exit For
                    End If
                Next
            End If

            If eventIndex = 0 Or eventID = 0 Then Exit Sub
        Else
            eventIndex = eventID
            If eventIndex = 0 Then Exit Sub
        End If

        If globalevent Then
            If Map(MapNum).Events(eventID).Pages(1).DirFix = 0 Then TempEventMap(MapNum).Events(eventID).Dir = Dir
        Else
            If Map(MapNum).Events(eventID).Pages(TempPlayer(Index).EventMap.EventPages(eventIndex).PageID).DirFix = 0 Then TempPlayer(Index).EventMap.EventPages(eventIndex).Dir = Dir
        End If

        Select Case Dir
            Case Direction.Up
                If globalevent Then
                    TempEventMap(MapNum).Events(eventIndex).Y = TempEventMap(MapNum).Events(eventIndex).Y - 1
                    Buffer.WriteInt32(ServerPackets.SEventMove)
                    Buffer.WriteInt32(eventID)
                    Buffer.WriteInt32(TempEventMap(MapNum).Events(eventIndex).X)
                    Buffer.WriteInt32(TempEventMap(MapNum).Events(eventIndex).Y)
                    Buffer.WriteInt32(Dir)
                    Buffer.WriteInt32(TempEventMap(MapNum).Events(eventIndex).Dir)
                    Buffer.WriteInt32(movementspeed)

                    Addlog("Sent SMSG: SEventMove Dir Up GlobalEvent", PACKET_LOG)
                    TextAdd("Sent SMSG: SEventMove Dir Up GlobalEvent")

                    If globalevent Then
                        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)
                    Else
                        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
                    End If
                    Buffer.Dispose()
                Else
                    TempPlayer(Index).EventMap.EventPages(eventIndex).Y = TempPlayer(Index).EventMap.EventPages(eventIndex).Y - 1
                    Buffer.WriteInt32(ServerPackets.SEventMove)
                    Buffer.WriteInt32(eventID)
                    Buffer.WriteInt32(TempPlayer(Index).EventMap.EventPages(eventIndex).X)
                    Buffer.WriteInt32(TempPlayer(Index).EventMap.EventPages(eventIndex).Y)
                    Buffer.WriteInt32(Dir)
                    Buffer.WriteInt32(TempPlayer(Index).EventMap.EventPages(eventIndex).Dir)
                    Buffer.WriteInt32(movementspeed)

                    Addlog("Sent SMSG: SEventMove Dir Up", PACKET_LOG)
                    TextAdd("Sent SMSG: SEventMove Dir Up")

                    If globalevent Then
                        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)
                    Else
                        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
                    End If
                    Buffer.Dispose()
                End If

            Case Direction.Down
                If globalevent Then
                    TempEventMap(MapNum).Events(eventIndex).Y = TempEventMap(MapNum).Events(eventIndex).Y + 1
                    Buffer.WriteInt32(ServerPackets.SEventMove)
                    Buffer.WriteInt32(eventID)
                    Buffer.WriteInt32(TempEventMap(MapNum).Events(eventIndex).X)
                    Buffer.WriteInt32(TempEventMap(MapNum).Events(eventIndex).Y)
                    Buffer.WriteInt32(Dir)
                    Buffer.WriteInt32(TempEventMap(MapNum).Events(eventIndex).Dir)
                    Buffer.WriteInt32(movementspeed)

                    Addlog("Sent SMSG: SEventMove Down GlobalEvent", PACKET_LOG)
                    TextAdd("Sent SMSG: SEventMove Down GlobalEvent")

                    If globalevent Then
                        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)
                    Else
                        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
                    End If
                    Buffer.Dispose()
                Else
                    TempPlayer(Index).EventMap.EventPages(eventIndex).Y = TempPlayer(Index).EventMap.EventPages(eventIndex).Y + 1
                    Buffer.WriteInt32(ServerPackets.SEventMove)
                    Buffer.WriteInt32(eventID)
                    Buffer.WriteInt32(TempPlayer(Index).EventMap.EventPages(eventIndex).X)
                    Buffer.WriteInt32(TempPlayer(Index).EventMap.EventPages(eventIndex).Y)
                    Buffer.WriteInt32(Dir)
                    Buffer.WriteInt32(TempPlayer(Index).EventMap.EventPages(eventIndex).Dir)
                    Buffer.WriteInt32(movementspeed)

                    Addlog("Sent SMSG: SEventMove", PACKET_LOG)
                    TextAdd("Sent SMSG: SEventMove")

                    If globalevent Then
                        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)
                    Else
                        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
                    End If
                    Buffer.Dispose()
                End If
            Case Direction.Left
                If globalevent Then
                    TempEventMap(MapNum).Events(eventIndex).X = TempEventMap(MapNum).Events(eventIndex).X - 1
                    Buffer.WriteInt32(ServerPackets.SEventMove)
                    Buffer.WriteInt32(eventID)
                    Buffer.WriteInt32(TempEventMap(MapNum).Events(eventIndex).X)
                    Buffer.WriteInt32(TempEventMap(MapNum).Events(eventIndex).Y)
                    Buffer.WriteInt32(Dir)
                    Buffer.WriteInt32(TempEventMap(MapNum).Events(eventIndex).Dir)
                    Buffer.WriteInt32(movementspeed)

                    Addlog("Sent SMSG: SEventMove Left GlobalEvent", PACKET_LOG)
                    TextAdd("Sent SMSG: SEventMove Left GlobalEvent")

                    If globalevent Then
                        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)
                    Else
                        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
                    End If
                    Buffer.Dispose()
                Else
                    TempPlayer(Index).EventMap.EventPages(eventIndex).X = TempPlayer(Index).EventMap.EventPages(eventIndex).X - 1
                    Buffer.WriteInt32(ServerPackets.SEventMove)
                    Buffer.WriteInt32(eventID)
                    Buffer.WriteInt32(TempPlayer(Index).EventMap.EventPages(eventIndex).X)
                    Buffer.WriteInt32(TempPlayer(Index).EventMap.EventPages(eventIndex).Y)
                    Buffer.WriteInt32(Dir)
                    Buffer.WriteInt32(TempPlayer(Index).EventMap.EventPages(eventIndex).Dir)
                    Buffer.WriteInt32(movementspeed)

                    Addlog("Sent SMSG: SEventMove", PACKET_LOG)
                    TextAdd("Sent SMSG: SEventMove")

                    If globalevent Then
                        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)
                    Else
                        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
                    End If
                    Buffer.Dispose()
                End If
            Case Direction.Right
                If globalevent Then
                    TempEventMap(MapNum).Events(eventIndex).X = TempEventMap(MapNum).Events(eventIndex).X + 1
                    Buffer.WriteInt32(ServerPackets.SEventMove)
                    Buffer.WriteInt32(eventID)
                    Buffer.WriteInt32(TempEventMap(MapNum).Events(eventIndex).X)
                    Buffer.WriteInt32(TempEventMap(MapNum).Events(eventIndex).Y)
                    Buffer.WriteInt32(Dir)
                    Buffer.WriteInt32(TempEventMap(MapNum).Events(eventIndex).Dir)
                    Buffer.WriteInt32(movementspeed)

                    Addlog("Sent SMSG: SEventMove GlobalEvent", PACKET_LOG)
                    TextAdd("Sent SMSG: SEventMove GlobalEvent")

                    If globalevent Then
                        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)
                    Else
                        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
                    End If
                    Buffer.Dispose()
                Else
                    TempPlayer(Index).EventMap.EventPages(eventIndex).X = TempPlayer(Index).EventMap.EventPages(eventIndex).X + 1
                    Buffer.WriteInt32(ServerPackets.SEventMove)
                    Buffer.WriteInt32(eventID)
                    Buffer.WriteInt32(TempPlayer(Index).EventMap.EventPages(eventIndex).X)
                    Buffer.WriteInt32(TempPlayer(Index).EventMap.EventPages(eventIndex).Y)
                    Buffer.WriteInt32(Dir)
                    Buffer.WriteInt32(TempPlayer(Index).EventMap.EventPages(eventIndex).Dir)
                    Buffer.WriteInt32(movementspeed)

                    Addlog("Sent SMSG: SEventMove", PACKET_LOG)
                    TextAdd("Sent SMSG: SEventMove")

                    If globalevent Then
                        SendDataToMap(MapNum, Buffer.Data, Buffer.Head)
                    Else
                        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
                    End If
                    Buffer.Dispose()
                End If
        End Select

    End Sub

    Function IsOneBlockAway(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer) As Boolean

        If x1 = x2 Then
            If y1 = y2 - 1 Or y1 = y2 + 1 Then
                IsOneBlockAway = True
            Else
                IsOneBlockAway = False
            End If
        ElseIf y1 = y2 Then
            If x1 = x2 - 1 Or x1 = x2 + 1 Then
                IsOneBlockAway = True
            Else
                IsOneBlockAway = False
            End If
        Else
            IsOneBlockAway = False
        End If

    End Function

    Function GetNpcDir(x As Integer, y As Integer, x1 As Integer, y1 As Integer) As Integer
        Dim i As Integer, distance As Integer

        i = Direction.Right

        If x - x1 > 0 Then
            If x - x1 > distance Then
                i = Direction.Right
                distance = x - x1
            End If
        ElseIf x - x1 < 0 Then
            If ((x - x1) * -1) > distance Then
                i = Direction.Left
                distance = ((x - x1) * -1)
            End If
        End If

        If y - y1 > 0 Then
            If y - y1 > distance Then
                i = Direction.Down
                distance = y - y1
            End If
        ElseIf y - y1 < 0 Then
            If ((y - y1) * -1) > distance Then
                i = Direction.Up
                distance = ((y - y1) * -1)
            End If
        End If

        GetNpcDir = i

    End Function

    Function CanEventMoveTowardsPlayer(playerID As Integer, MapNum As Integer, eventID As Integer) As Integer
        Dim i As Integer, x As Integer, y As Integer, x1 As Integer, y1 As Integer, didwalk As Boolean, WalkThrough As Integer
        Dim tim As Integer, sX As Integer, sY As Integer, pos(,) As Integer, reachable As Boolean, j As Integer, LastSum As Integer, Sum As Integer, FX As Integer, FY As Integer
        Dim path() As Point, LastX As Integer, LastY As Integer, did As Boolean
        'This does not work for global events so this MUST be a player one....

        'This Event returns a direction, 4 is not a valid direction so we assume fail unless otherwise told.
        CanEventMoveTowardsPlayer = 4

        If playerID <= 0 Or playerID > MAX_PLAYERS Then Exit Function
        If MapNum <= 0 Or MapNum > MAX_MAPS Then Exit Function
        If eventID <= 0 Or eventID > TempPlayer(playerID).EventMap.CurrentEvents Then Exit Function
        If Gettingmap = True Then Exit Function

        x = GetPlayerX(playerID)
        y = GetPlayerY(playerID)
        x1 = TempPlayer(playerID).EventMap.EventPages(eventID).X
        y1 = TempPlayer(playerID).EventMap.EventPages(eventID).Y
        WalkThrough = Map(MapNum).Events(TempPlayer(playerID).EventMap.EventPages(eventID).EventID).Pages(TempPlayer(playerID).EventMap.EventPages(eventID).PageID).WalkThrough
        'Add option for pathfinding to random guessing option.

        If PathfindingType = 1 Then
            i = Int(Rnd() * 5)
            didwalk = False

            ' Lets move the event
            Select Case i
                Case 0
                    ' Up
                    If y1 > y And Not didwalk Then
                        If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Up, False) Then
                            CanEventMoveTowardsPlayer = Direction.Up
                            Exit Function
                            didwalk = True
                        End If
                    End If

                    ' Down
                    If y1 < y And Not didwalk Then
                        If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Down, False) Then
                            CanEventMoveTowardsPlayer = Direction.Down
                            Exit Function
                            didwalk = True
                        End If
                    End If

                    ' Left
                    If x1 > x And Not didwalk Then
                        If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Left, False) Then
                            CanEventMoveTowardsPlayer = Direction.Left
                            Exit Function
                            didwalk = True
                        End If
                    End If

                    ' Right
                    If x1 < x And Not didwalk Then
                        If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Right, False) Then
                            CanEventMoveTowardsPlayer = Direction.Right
                            Exit Function
                            didwalk = True
                        End If
                    End If

                Case 1
                    ' Right
                    If x1 < x And Not didwalk Then
                        If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Right, False) Then
                            CanEventMoveTowardsPlayer = Direction.Right
                            Exit Function
                            didwalk = True
                        End If
                    End If

                    ' Left
                    If x1 > x And Not didwalk Then
                        If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Left, False) Then
                            CanEventMoveTowardsPlayer = Direction.Left
                            Exit Function
                            didwalk = True
                        End If
                    End If

                    ' Down
                    If y1 < y And Not didwalk Then
                        If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Down, False) Then
                            CanEventMoveTowardsPlayer = Direction.Down
                            Exit Function
                            didwalk = True
                        End If
                    End If

                    ' Up
                    If y1 > y And Not didwalk Then
                        If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Up, False) Then
                            CanEventMoveTowardsPlayer = Direction.Up
                            Exit Function
                            didwalk = True
                        End If
                    End If

                Case 2
                    ' Down
                    If y1 < y And Not didwalk Then
                        If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Down, False) Then
                            CanEventMoveTowardsPlayer = Direction.Down
                            Exit Function
                            didwalk = True
                        End If
                    End If

                    ' Up
                    If y1 > y And Not didwalk Then
                        If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Up, False) Then
                            CanEventMoveTowardsPlayer = Direction.Up
                            Exit Function
                            didwalk = True
                        End If
                    End If

                    ' Right
                    If x1 < x And Not didwalk Then
                        If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Right, False) Then
                            CanEventMoveTowardsPlayer = Direction.Right
                            Exit Function
                            didwalk = True
                        End If
                    End If

                    ' Left
                    If x1 > x And Not didwalk Then
                        If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Left, False) Then
                            CanEventMoveTowardsPlayer = Direction.Left
                            Exit Function
                            didwalk = True
                        End If
                    End If

                Case 3
                    ' Left
                    If x1 > x And Not didwalk Then
                        If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Left, False) Then
                            CanEventMoveTowardsPlayer = Direction.Left
                            Exit Function
                            didwalk = True
                        End If
                    End If

                    ' Right
                    If x1 < x And Not didwalk Then
                        If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Right, False) Then
                            CanEventMoveTowardsPlayer = Direction.Right
                            Exit Function
                            didwalk = True
                        End If
                    End If

                    ' Up
                    If y1 > y And Not didwalk Then
                        If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Up, False) Then
                            CanEventMoveTowardsPlayer = Direction.Up
                            Exit Function
                            didwalk = True
                        End If
                    End If

                    ' Down
                    If y1 < y And Not didwalk Then
                        If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Down, False) Then
                            CanEventMoveTowardsPlayer = Direction.Down
                            Exit Function
                            didwalk = True
                        End If
                    End If
            End Select
            CanEventMoveTowardsPlayer = Random(0, 3)
        ElseIf PathfindingType = 2 Then
            'Initialization phase
            tim = 0
            sX = x1
            sY = y1
            FX = x
            FY = y

            ReDim pos(0 To Map(MapNum).MaxX, 0 To Map(MapNum).MaxY)

            For i = 1 To TempPlayer(playerID).EventMap.CurrentEvents
                If TempPlayer(playerID).EventMap.EventPages(i).Visible Then
                    If TempPlayer(playerID).EventMap.EventPages(i).WalkThrough = 1 Then
                        pos(TempPlayer(playerID).EventMap.EventPages(i).X, TempPlayer(playerID).EventMap.EventPages(i).Y) = 9
                    End If
                End If
            Next

            pos(sX, sY) = 100 + tim
            pos(FX, FY) = 2

            'reset reachable
            reachable = False

            'Do while reachable is false... if its set true in progress, we jump out
            'If the path is decided unreachable in process, we will use exit sub. Not proper,
            'but faster ;-)
            Do While reachable = False
                'we loop through all squares
                For j = 0 To Map(MapNum).MaxY
                    For i = 0 To Map(MapNum).MaxX
                        'If j = 10 And i = 0 Then MsgBox "hi!"
                        'If they are to be extended, the pointer TIM is on them
                        If pos(i, j) = 100 + tim Then
                            'The part is to be extended, so do it
                            'We have to make sure that there is a pos(i+1,j) BEFORE we actually use it,
                            'because then we get error... If the square is on side, we dont test for this one!
                            If i < Map(MapNum).MaxX Then
                                'If there isnt a wall, or any other... thing
                                If pos(i + 1, j) = 0 Then
                                    'Expand it, and make its pos equal to tim+1, so the next time we make this loop,
                                    'It will exapand that square too! This is crucial part of the program
                                    pos(i + 1, j) = 100 + tim + 1
                                ElseIf pos(i + 1, j) = 2 Then
                                    'If the position is no 0 but its 2 (FINISH) then Reachable = true!!! We found end
                                    reachable = True
                                End If
                            End If

                            'This is the same as the last one, as i said a lot of copy paste work and editing that
                            'This is simply another side that we have to test for... so instead of i+1 we have i-1
                            'Its actually pretty same then... I wont comment it therefore, because its only repeating
                            'same thing with minor changes to check sides
                            If i > 0 Then
                                If pos((i - 1), j) = 0 Then
                                    pos(i - 1, j) = 100 + tim + 1
                                ElseIf pos(i - 1, j) = 2 Then
                                    reachable = True
                                End If
                            End If

                            If j < Map(MapNum).MaxY Then
                                If pos(i, j + 1) = 0 Then
                                    pos(i, j + 1) = 100 + tim + 1
                                ElseIf pos(i, j + 1) = 2 Then
                                    reachable = True
                                End If
                            End If

                            If j > 0 Then
                                If pos(i, j - 1) = 0 Then
                                    pos(i, j - 1) = 100 + tim + 1
                                ElseIf pos(i, j - 1) = 2 Then
                                    reachable = True
                                End If
                            End If
                        End If
                        DoEvents()
                    Next i
                Next j

                'If the reachable is STILL false, then
                If reachable = False Then
                    'reset sum
                    Sum = 0
                    For j = 0 To Map(MapNum).MaxY
                        For i = 0 To Map(MapNum).MaxX
                            'we add up ALL the squares
                            Sum = Sum + pos(i, j)
                        Next i
                    Next j

                    'Now if the sum is euqal to the last sum, its not reachable, if it isnt, then we store
                    'sum to lastsum
                    If Sum = LastSum Then
                        CanEventMoveTowardsPlayer = 4
                        Exit Function
                    Else
                        LastSum = Sum
                    End If
                End If

                'we increase the pointer to point to the next squares to be expanded
                tim = tim + 1
            Loop

            'We work backwards to find the way...
            LastX = FX
            LastY = FY

            ReDim path(tim + 1)

            'The following code may be a little bit confusing but ill try my best to explain it.
            'We are working backwards to find ONE of the shortest ways back to Start.
            'So we repeat the loop until the LastX and LastY arent in start. Look in the code to see
            'how LastX and LasY change
            Do While LastX <> sX Or LastY <> sY
                'We decrease tim by one, and then we are finding any adjacent square to the final one, that
                'has that value. So lets say the tim would be 5, because it takes 5 steps to get to the target.
                'Now everytime we decrease that, so we make it 4, and we look for any adjacent square that has
                'that value. When we find it, we just color it yellow as for the solution
                tim = tim - 1
                'reset did to false
                did = False

                'If we arent on edge
                If LastX < Map(MapNum).MaxX Then
                    'check the square on the right of the solution. Is it a tim-1 one? or just a blank one
                    If pos(LastX + 1, LastY) = 100 + tim Then
                        'if it, then make it yellow, and change did to true
                        LastX = LastX + 1
                        did = True
                    End If
                End If

                'This will then only work if the previous part didnt execute, and did is still false. THen
                'we want to check another square, the on left. Is it a tim-1 one ?
                If did = False Then
                    If LastX > 0 Then
                        If pos(LastX - 1, LastY) = 100 + tim Then
                            LastX = LastX - 1
                            did = True
                        End If
                    End If
                End If

                'We check the one below it
                If did = False Then
                    If LastY < Map(MapNum).MaxY Then
                        If pos(LastX, LastY + 1) = 100 + tim Then
                            LastY = LastY + 1
                            did = True
                        End If
                    End If
                End If

                'And above it. One of these have to be it, since we have found the solution, we know that already
                'there is a way back.
                If did = False Then
                    If LastY > 0 Then
                        If pos(LastX, LastY - 1) = 100 + tim Then
                            LastY = LastY - 1
                        End If
                    End If
                End If

                path(tim).X = LastX
                path(tim).Y = LastY

                'Now we loop back and decrease tim, and look for the next square with lower value
                DoEvents()
            Loop

            'Ok we got a path. Now, lets look at the first step and see what direction we should take.
            If path(1).X > LastX Then
                CanEventMoveTowardsPlayer = Direction.Right
            ElseIf path(1).Y > LastY Then
                CanEventMoveTowardsPlayer = Direction.Down
            ElseIf path(1).Y < LastY Then
                CanEventMoveTowardsPlayer = Direction.Up
            ElseIf path(1).X < LastX Then
                CanEventMoveTowardsPlayer = Direction.Left
            End If

        End If

    End Function

    Function CanEventMoveAwayFromPlayer(playerID As Integer, MapNum As Integer, eventID As Integer) As Integer
        Dim i As Integer, x As Integer, y As Integer, x1 As Integer, y1 As Integer, didwalk As Boolean, WalkThrough As Integer
        'This does not work for global events so this MUST be a player one....

        'This Event returns a direction, 5 is not a valid direction so we assume fail unless otherwise told.
        CanEventMoveAwayFromPlayer = 5

        If playerID <= 0 Or playerID > MAX_PLAYERS Then Exit Function
        If MapNum <= 0 Or MapNum > MAX_MAPS Then Exit Function
        If eventID <= 0 Or eventID > TempPlayer(playerID).EventMap.CurrentEvents Then Exit Function
        If Gettingmap = True Then Exit Function

        x = GetPlayerX(playerID)
        y = GetPlayerY(playerID)
        x1 = TempPlayer(playerID).EventMap.EventPages(eventID).X
        y1 = TempPlayer(playerID).EventMap.EventPages(eventID).Y
        WalkThrough = Map(MapNum).Events(TempPlayer(playerID).EventMap.EventPages(eventID).EventID).Pages(TempPlayer(playerID).EventMap.EventPages(eventID).PageID).WalkThrough

        i = Int(Rnd() * 5)
        didwalk = False

        ' Lets move the event
        Select Case i
            Case 0
                ' Up
                If y1 > y And Not didwalk Then
                    If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Down, False) Then
                        CanEventMoveAwayFromPlayer = Direction.Down
                        Exit Function
                        didwalk = True
                    End If
                End If

                ' Down
                If y1 < y And Not didwalk Then
                    If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Up, False) Then
                        CanEventMoveAwayFromPlayer = Direction.Up
                        Exit Function
                        didwalk = True
                    End If
                End If

                ' Left
                If x1 > x And Not didwalk Then
                    If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Right, False) Then
                        CanEventMoveAwayFromPlayer = Direction.Right
                        Exit Function
                        didwalk = True
                    End If
                End If

                ' Right
                If x1 < x And Not didwalk Then
                    If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Left, False) Then
                        CanEventMoveAwayFromPlayer = Direction.Left
                        Exit Function
                        didwalk = True
                    End If
                End If

            Case 1
                ' Right
                If x1 < x And Not didwalk Then
                    If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Left, False) Then
                        CanEventMoveAwayFromPlayer = Direction.Left
                        Exit Function
                        didwalk = True
                    End If
                End If

                ' Left
                If x1 > x And Not didwalk Then
                    If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Right, False) Then
                        CanEventMoveAwayFromPlayer = Direction.Right
                        Exit Function
                        didwalk = True
                    End If
                End If

                ' Down
                If y1 < y And Not didwalk Then
                    If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Up, False) Then
                        CanEventMoveAwayFromPlayer = Direction.Up
                        Exit Function
                        didwalk = True
                    End If
                End If

                ' Up
                If y1 > y And Not didwalk Then
                    If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Down, False) Then
                        CanEventMoveAwayFromPlayer = Direction.Down
                        Exit Function
                        didwalk = True
                    End If
                End If

            Case 2
                ' Down
                If y1 < y And Not didwalk Then
                    If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Up, False) Then
                        CanEventMoveAwayFromPlayer = Direction.Up
                        Exit Function
                        didwalk = True
                    End If
                End If

                ' Up
                If y1 > y And Not didwalk Then
                    If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Down, False) Then
                        CanEventMoveAwayFromPlayer = Direction.Down
                        Exit Function
                        didwalk = True
                    End If
                End If

                ' Right
                If x1 < x And Not didwalk Then
                    If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Left, False) Then
                        CanEventMoveAwayFromPlayer = Direction.Left
                        Exit Function
                        didwalk = True
                    End If
                End If

                ' Left
                If x1 > x And Not didwalk Then
                    If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Right, False) Then
                        CanEventMoveAwayFromPlayer = Direction.Right
                        Exit Function
                        didwalk = True
                    End If
                End If

            Case 3
                ' Left
                If x1 > x And Not didwalk Then
                    If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Right, False) Then
                        CanEventMoveAwayFromPlayer = Direction.Right
                        Exit Function
                        didwalk = True
                    End If
                End If

                ' Right
                If x1 < x And Not didwalk Then
                    If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Left, False) Then
                        CanEventMoveAwayFromPlayer = Direction.Left
                        Exit Function
                        didwalk = True
                    End If
                End If

                ' Up
                If y1 > y And Not didwalk Then
                    If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Down, False) Then
                        CanEventMoveAwayFromPlayer = Direction.Down
                        Exit Function
                        didwalk = True
                    End If
                End If

                ' Down
                If y1 < y And Not didwalk Then
                    If CanEventMove(playerID, MapNum, x1, y1, eventID, WalkThrough, Direction.Up, False) Then
                        CanEventMoveAwayFromPlayer = Direction.Up
                        Exit Function
                        didwalk = True
                    End If
                End If

        End Select

        CanEventMoveAwayFromPlayer = Random(0, 3)

    End Function

    Function GetDirToPlayer(playerID As Integer, MapNum As Integer, eventID As Integer) As Integer
        Dim i As Integer, x As Integer, y As Integer, x1 As Integer, y1 As Integer, distance As Integer
        'This does not work for global events so this MUST be a player one....

        If playerID <= 0 Or playerID > MAX_PLAYERS Then Exit Function
        If MapNum <= 0 Or MapNum > MAX_MAPS Then Exit Function
        If eventID <= 0 Or eventID > TempPlayer(playerID).EventMap.CurrentEvents Then Exit Function

        x = GetPlayerX(playerID)
        y = GetPlayerY(playerID)
        x1 = TempPlayer(playerID).EventMap.EventPages(eventID).X
        y1 = TempPlayer(playerID).EventMap.EventPages(eventID).Y

        i = Direction.Right

        If x - x1 > 0 Then
            If x - x1 > distance Then
                i = Direction.Right
                distance = x - x1
            End If
        ElseIf x - x1 < 0 Then
            If ((x - x1) * -1) > distance Then
                i = Direction.Left
                distance = ((x - x1) * -1)
            End If
        End If

        If y - y1 > 0 Then
            If y - y1 > distance Then
                i = Direction.Down
                distance = y - y1
            End If
        ElseIf y - y1 < 0 Then
            If ((y - y1) * -1) > distance Then
                i = Direction.Up
                distance = ((y - y1) * -1)
            End If
        End If

        GetDirToPlayer = i

    End Function

    Function GetDirAwayFromPlayer(playerID As Integer, MapNum As Integer, eventID As Integer) As Integer
        Dim i As Integer, x As Integer, y As Integer, x1 As Integer, y1 As Integer, distance As Integer
        'This does not work for global events so this MUST be a player one....

        If playerID <= 0 Or playerID > MAX_PLAYERS Then Exit Function
        If MapNum <= 0 Or MapNum > MAX_MAPS Then Exit Function
        If eventID <= 0 Or eventID > TempPlayer(playerID).EventMap.CurrentEvents Then Exit Function

        x = GetPlayerX(playerID)
        y = GetPlayerY(playerID)
        x1 = TempPlayer(playerID).EventMap.EventPages(eventID).X
        y1 = TempPlayer(playerID).EventMap.EventPages(eventID).Y


        i = Direction.Right

        If x - x1 > 0 Then
            If x - x1 > distance Then
                i = Direction.Left
                distance = x - x1
            End If
        ElseIf x - x1 < 0 Then
            If ((x - x1) * -1) > distance Then
                i = Direction.Right
                distance = ((x - x1) * -1)
            End If
        End If

        If y - y1 > 0 Then
            If y - y1 > distance Then
                i = Direction.Up
                distance = y - y1
            End If
        ElseIf y - y1 < 0 Then
            If ((y - y1) * -1) > distance Then
                i = Direction.Down
                distance = ((y - y1) * -1)
            End If
        End If

        GetDirAwayFromPlayer = i

    End Function
#End Region

#Region "Incoming Packets"
    Sub Packet_EventChatReply(ByVal Index As Integer, ByRef data() As Byte)
        Dim eventID As Integer, pageID As Integer, reply As Integer, i As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CEventChatReply", PACKET_LOG)
        TextAdd("Recieved CMSG: CEventChatReply")


        eventID = Buffer.ReadInt32
        pageID = Buffer.ReadInt32
        reply = Buffer.ReadInt32

        If TempPlayer(Index).EventProcessingCount > 0 Then
            For i = 1 To TempPlayer(Index).EventProcessingCount
                If TempPlayer(Index).EventProcessing(i).EventID = eventID And TempPlayer(Index).EventProcessing(i).PageID = pageID Then
                    If TempPlayer(Index).EventProcessing(i).WaitingForResponse = 1 Then
                        If reply = 0 Then
                            If Map(GetPlayerMap(Index)).Events(eventID).Pages(pageID).CommandList(TempPlayer(Index).EventProcessing(i).CurList).Commands(TempPlayer(Index).EventProcessing(i).CurSlot - 1).Index = EventType.evShowText Then
                                TempPlayer(Index).EventProcessing(i).WaitingForResponse = 0
                            End If
                        ElseIf reply > 0 Then
                            If Map(GetPlayerMap(Index)).Events(eventID).Pages(pageID).CommandList(TempPlayer(Index).EventProcessing(i).CurList).Commands(TempPlayer(Index).EventProcessing(i).CurSlot - 1).Index = EventType.evShowChoices Then
                                Select Case reply
                                    Case 1
                                        TempPlayer(Index).EventProcessing(i).ListLeftOff(TempPlayer(Index).EventProcessing(i).CurList) = TempPlayer(Index).EventProcessing(i).CurSlot - 1
                                        TempPlayer(Index).EventProcessing(i).CurList = Map(GetPlayerMap(Index)).Events(eventID).Pages(pageID).CommandList(TempPlayer(Index).EventProcessing(i).CurList).Commands(TempPlayer(Index).EventProcessing(i).CurSlot - 1).Data1
                                        TempPlayer(Index).EventProcessing(i).CurSlot = 1
                                    Case 2
                                        TempPlayer(Index).EventProcessing(i).ListLeftOff(TempPlayer(Index).EventProcessing(i).CurList) = TempPlayer(Index).EventProcessing(i).CurSlot - 1
                                        TempPlayer(Index).EventProcessing(i).CurList = Map(GetPlayerMap(Index)).Events(eventID).Pages(pageID).CommandList(TempPlayer(Index).EventProcessing(i).CurList).Commands(TempPlayer(Index).EventProcessing(i).CurSlot - 1).Data2
                                        TempPlayer(Index).EventProcessing(i).CurSlot = 1
                                    Case 3
                                        TempPlayer(Index).EventProcessing(i).ListLeftOff(TempPlayer(Index).EventProcessing(i).CurList) = TempPlayer(Index).EventProcessing(i).CurSlot - 1
                                        TempPlayer(Index).EventProcessing(i).CurList = Map(GetPlayerMap(Index)).Events(eventID).Pages(pageID).CommandList(TempPlayer(Index).EventProcessing(i).CurList).Commands(TempPlayer(Index).EventProcessing(i).CurSlot - 1).Data3
                                        TempPlayer(Index).EventProcessing(i).CurSlot = 1
                                    Case 4
                                        TempPlayer(Index).EventProcessing(i).ListLeftOff(TempPlayer(Index).EventProcessing(i).CurList) = TempPlayer(Index).EventProcessing(i).CurSlot - 1
                                        TempPlayer(Index).EventProcessing(i).CurList = Map(GetPlayerMap(Index)).Events(eventID).Pages(pageID).CommandList(TempPlayer(Index).EventProcessing(i).CurList).Commands(TempPlayer(Index).EventProcessing(i).CurSlot - 1).Data4
                                        TempPlayer(Index).EventProcessing(i).CurSlot = 1
                                End Select
                            End If
                            TempPlayer(Index).EventProcessing(i).WaitingForResponse = 0
                        End If
                    End If
                End If
            Next
        End If

        Buffer.Dispose()

    End Sub

    Sub Packet_Event(ByVal Index As Integer, ByRef data() As Byte)
        Dim i As Integer, begineventprocessing As Boolean, z As Integer
        Dim x As Integer, y As Integer
        Dim Buffer As New ByteStream(data)
        Addlog("Recieved CMSG: CEvent", PACKET_LOG)
        TextAdd("Recieved CMSG: CEvent")

        i = Buffer.ReadInt32
        Buffer.Dispose()

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

        If TempPlayer(Index).EventMap.CurrentEvents > 0 Then
            For z = 1 To TempPlayer(Index).EventMap.CurrentEvents
                If TempPlayer(Index).EventMap.EventPages(z).EventID = i Then
                    i = z
                    begineventprocessing = True
                    Exit For
                End If
            Next
        End If

        If begineventprocessing = True Then
            If Map(GetPlayerMap(Index)).Events(TempPlayer(Index).EventMap.EventPages(i).EventID).Pages(TempPlayer(Index).EventMap.EventPages(i).PageID).CommandListCount > 0 Then
                'Process this event, it is action button and everything checks out.
                If (TempPlayer(Index).EventProcessing(TempPlayer(Index).EventMap.EventPages(i).EventID).Active = 0) Then
                    TempPlayer(Index).EventProcessing(TempPlayer(Index).EventMap.EventPages(i).EventID).Active = 1
                    With TempPlayer(Index).EventProcessing(TempPlayer(Index).EventMap.EventPages(i).EventID)
                        .ActionTimer = GetTimeMs()
                        .CurList = 1
                        .CurSlot = 1
                        .EventID = TempPlayer(Index).EventMap.EventPages(i).EventID
                        .PageID = TempPlayer(Index).EventMap.EventPages(i).PageID
                        .WaitingForResponse = 0
                        ReDim .ListLeftOff(0 To Map(GetPlayerMap(Index)).Events(TempPlayer(Index).EventMap.EventPages(i).EventID).Pages(TempPlayer(Index).EventMap.EventPages(i).PageID).CommandListCount)
                    End With
                End If
            End If
            begineventprocessing = False
        End If

    End Sub

    Sub Packet_RequestSwitchesAndVariables(ByVal Index As Integer, ByRef data() As Byte)
        Addlog("Recieved CMSG: CRequestSwitchesAndVariables", PACKET_LOG)
        TextAdd("Recieved CMSG: CRequestSwitchesAndVariables")
        SendSwitchesAndVariables(Index)
    End Sub

    Sub Packet_SwitchesAndVariables(ByVal Index As Integer, ByRef data() As Byte)
        Dim i As Integer
        Dim Buffer As New ByteStream(data)

        Addlog("Recieved CMSG: CSwitchesAndVariables", PACKET_LOG)
        TextAdd("Recieved CMSG: CSwitchesAndVariables")

        For i = 1 To MAX_SWITCHES
            Switches(i) = Buffer.ReadString
        Next

        For i = 1 To MAX_VARIABLES
            Variables(i) = Buffer.ReadString
        Next

        SaveSwitches()
        SaveVariables()

        Buffer.Dispose()

        SendSwitchesAndVariables(0, True)

    End Sub

#End Region

#Region "Outgoing Packets"
    Sub SendSpecialEffect(Index As Integer, EffectType As Integer, Optional Data1 As Integer = 0, Optional Data2 As Integer = 0, Optional Data3 As Integer = 0, Optional Data4 As Integer = 0)
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(ServerPackets.SSpecialEffect)

        Addlog("Sent SMSG: SSpecialEffect", PACKET_LOG)
        TextAdd("Sent SMSG: SPecialEffect")

        Select Case EffectType
            Case EFFECT_TYPE_FADEIN
                Buffer.WriteInt32(EffectType)
            Case EFFECT_TYPE_FADEOUT
                Buffer.WriteInt32(EffectType)
            Case EFFECT_TYPE_FLASH
                Buffer.WriteInt32(EffectType)
            Case EFFECT_TYPE_FOG
                Buffer.WriteInt32(EffectType)
                Buffer.WriteInt32(Data1) 'fognum
                Buffer.WriteInt32(Data2) 'fog movement speed
                Buffer.WriteInt32(Data3) 'opacity
            Case EFFECT_TYPE_WEATHER
                Buffer.WriteInt32(EffectType)
                Buffer.WriteInt32(Data1) 'weather type
                Buffer.WriteInt32(Data2) 'weather intensity
            Case EFFECT_TYPE_TINT
                Buffer.WriteInt32(EffectType)
                Buffer.WriteInt32(Data1) 'red
                Buffer.WriteInt32(Data2) 'green
                Buffer.WriteInt32(Data3) 'blue
                Buffer.WriteInt32(Data4) 'alpha
        End Select

        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        Buffer.Dispose()

    End Sub

    Sub SendSwitchesAndVariables(Index As Integer, Optional everyone As Boolean = False)
        Dim Buffer As New ByteStream(4), i As Integer

        Buffer.WriteInt32(ServerPackets.SSwitchesAndVariables)

        Addlog("Sent SMSG: SSwitchesAndVariables", PACKET_LOG)
        TextAdd("Sent SMSG: SSwitchesAndVariables")

        For i = 1 To MAX_SWITCHES
            Buffer.WriteString(Trim(Switches(i)))
        Next

        For i = 1 To MAX_VARIABLES
            Buffer.WriteString(Trim(Variables(i)))
        Next

        If everyone Then
            SendDataToAll(Buffer.Data, Buffer.Head)
        Else
            Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        End If

        Buffer.Dispose()

    End Sub

    Sub SendMapEventData(Index As Integer)
        Dim Buffer As New ByteStream(4), i As Integer, x As Integer, y As Integer
        Dim z As Integer, MapNum As Integer, w As Integer

        Buffer.WriteInt32(ServerPackets.SMapEventData)
        MapNum = GetPlayerMap(Index)

        Addlog("Sent SMSG: SMapEventData", PACKET_LOG)
        TextAdd("Sent SMSG: SMapEventData")

        'Event Data
        Buffer.WriteInt32(Map(MapNum).EventCount)

        If Map(MapNum).EventCount > 0 Then
            For i = 1 To Map(MapNum).EventCount
                With Map(MapNum).Events(i)
                    Buffer.WriteString(Trim(.Name))
                    Buffer.WriteInt32(.Globals)
                    Buffer.WriteInt32(.X)
                    Buffer.WriteInt32(.Y)
                    Buffer.WriteInt32(.PageCount)
                End With
                If Map(MapNum).Events(i).PageCount > 0 Then
                    For x = 1 To Map(MapNum).Events(i).PageCount
                        With Map(MapNum).Events(i).Pages(x)
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
                            Buffer.WriteInt32(.MoveRouteCount)

                            Buffer.WriteInt32(.IgnoreMoveRoute)
                            Buffer.WriteInt32(.RepeatMoveRoute)

                            If .MoveRouteCount > 0 Then
                                For y = 1 To .MoveRouteCount
                                    Buffer.WriteInt32(.MoveRoute(y).Index)
                                    Buffer.WriteInt32(.MoveRoute(y).Data1)
                                    Buffer.WriteInt32(.MoveRoute(y).Data2)
                                    Buffer.WriteInt32(.MoveRoute(y).Data3)
                                    Buffer.WriteInt32(.MoveRoute(y).Data4)
                                    Buffer.WriteInt32(.MoveRoute(y).Data5)
                                    Buffer.WriteInt32(.MoveRoute(y).Data6)
                                Next
                            End If

                            Buffer.WriteInt32(.WalkAnim)
                            Buffer.WriteInt32(.DirFix)
                            Buffer.WriteInt32(.WalkThrough)
                            Buffer.WriteInt32(.ShowName)
                            Buffer.WriteInt32(.Trigger)
                            Buffer.WriteInt32(.CommandListCount)

                            Buffer.WriteInt32(.Position)
                            Buffer.WriteInt32(.QuestNum)
                        End With

                        If Map(MapNum).Events(i).Pages(x).CommandListCount > 0 Then
                            For y = 1 To Map(MapNum).Events(i).Pages(x).CommandListCount
                                Buffer.WriteInt32(Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount)
                                Buffer.WriteInt32(Map(MapNum).Events(i).Pages(x).CommandList(y).ParentList)
                                If Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount > 0 Then
                                    For z = 1 To Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount
                                        With Map(MapNum).Events(i).Pages(x).CommandList(y).Commands(z)
                                            Buffer.WriteInt32(.Index)
                                            Buffer.WriteString(.Text1)
                                            Buffer.WriteString(.Text2)
                                            Buffer.WriteString(.Text3)
                                            Buffer.WriteString(.Text4)
                                            Buffer.WriteString(.Text5)
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
        Socket.SendDataTo(Index, Buffer.Data, Buffer.Head)
        Buffer.Dispose
        SendSwitchesAndVariables(Index)

    End Sub
#End Region

#Region "Misc"
    Public Sub GivePlayerEXP(Index As Integer, Exp As Integer)
        ' give the exp

        SetPlayerExp(Index, GetPlayerExp(Index) + Exp)
        SendActionMsg(GetPlayerMap(Index), "+" & Exp & " Exp", ColorType.White, 1, (GetPlayerX(Index) * 32), (GetPlayerY(Index) * 32))
        ' check if we've leveled
        CheckPlayerLevelUp(Index)

        If PetAlive(Index) Then
            If Pet(GetPetNum(Index)).LevelingType = 0 Then
                SetPetExp(Index, GetPetExp(Index) + (Exp * (Pet(GetPetNum(Index)).ExpGain / 100)))
                SendActionMsg(GetPlayerMap(Index), "+" & (Exp * (Pet(GetPetNum(Index)).ExpGain / 100)) & " Exp", ColorType.White, 1, (GetPetX(Index) * 32), (GetPetY(Index) * 32))
                CheckPetLevelUp(Index)
                SendPetExp(Index)
            End If
        End If

        SendExp(Index)
        SendPlayerData(Index)

    End Sub

    Public Sub CustomScript(Index As Integer, caseID As Integer, MapNum As Integer, EventId As Integer)

        Select Case caseID

            Case Else
                PlayerMsg(Index, "You just activated custom script " & caseID & ". This script is not yet programmed.", ColorType.BrightRed)
        End Select

    End Sub
#End Region

End Module
