﻿Imports System.IO
Imports System.Linq
Imports ASFW
Imports ASFW.IO
Imports ASFW.IO.FileIO

Module ServerDatabase

#Region "Classes"
    Public Sub CreateClasses()
        Dim myXml As New XmlClass With {
            .Filename = Path.Combine(Application.StartupPath, "data", "Classes.xml"),
            .Root = "Data"
        }

        myXml.NewXmlDocument()

        Max_Classes = 1

        myXml.LoadXml()

        myXml.WriteString("INIT", "MaxClasses", Max_Classes)
        myXml.WriteString("CLASS1", "Name", "Warrior")
        myXml.WriteString("CLASS1", "Desc", "Warrior Description")
        myXml.WriteString("CLASS1", "MaleSprite", "1")
        myXml.WriteString("CLASS1", "FemaleSprite", "2")
        myXml.WriteString("CLASS1", "Str", "5")
        myXml.WriteString("CLASS1", "End", "5")
        myXml.WriteString("CLASS1", "Vit", "5")
        myXml.WriteString("CLASS1", "Luck", "5")
        myXml.WriteString("CLASS1", "Int", "5")
        myXml.WriteString("CLASS1", "Spir", "5")
        myXml.WriteString("CLASS1", "BaseExp", "25")

        myXml.WriteString("CLASS1", "StartMap", Options.StartMap)
        myXml.WriteString("CLASS1", "StartX", Options.StartX)
        myXml.WriteString("CLASS1", "StartY", Options.StartY)

        myXml.CloseXml(True)
    End Sub

    Sub ClearClasses()
        Dim i As Integer

        ReDim Classes(0 To Max_Classes)

        For i = 1 To Max_Classes
            Classes(i) = Nothing
            Classes(i).Name = ""
            Classes(i).Desc = ""
        Next

        For i = 0 To Max_Classes
            ReDim Classes(i).Stat(0 To Stats.Count - 1)
            ReDim Classes(i).StartItem(0 To 5)
            ReDim Classes(i).StartValue(0 To 5)
        Next

    End Sub

    Sub LoadClasses()
        Dim i As Integer, n As Integer
        Dim tmpSprite As String
        Dim tmpArray() As String
        Dim x As Integer

        If Not FileExist(Path.Combine(Application.StartupPath, "data", "Classes.xml")) Then CreateClasses()

        Dim myXml As New XmlClass With {
            .Filename = Path.Combine(Application.StartupPath, "data", "Classes.xml"),
            .Root = "Data"
        }

        myXml.LoadXml()

        Max_Classes = Val(myXml.ReadString("INIT", "MaxClasses", "1"))

        ClearClasses()

        For i = 1 To Max_Classes
            Classes(i).Name = myXml.ReadString("CLASS" & i, "Name")
            Classes(i).Desc = myXml.ReadString("CLASS" & i, "Desc")

            ' read string of sprites
            tmpSprite = myXml.ReadString("CLASS" & i, "MaleSprite")
            ' split into an array of strings
            tmpArray = Split(tmpSprite, ",")
            ' redim the class sprite array
            ReDim Classes(i).MaleSprite(0 To UBound(tmpArray))
            ' loop through converting strings to values and store in the sprite array
            For n = 0 To UBound(tmpArray)
                Classes(i).MaleSprite(n) = Val(tmpArray(n))
            Next

            ' read string of sprites
            tmpSprite = myXml.ReadString("CLASS" & i, "FemaleSprite")
            ' split into an array of strings
            tmpArray = Split(tmpSprite, ",")
            ' redim the class sprite array
            ReDim Classes(i).FemaleSprite(0 To UBound(tmpArray))
            ' loop through converting strings to values and store in the sprite array
            For n = 0 To UBound(tmpArray)
                Classes(i).FemaleSprite(n) = Val(tmpArray(n))
            Next

            ' continue
            Classes(i).Stat(Stats.Strength) = Val(myXml.ReadString("CLASS" & i, "Str"))
            Classes(i).Stat(Stats.Endurance) = Val(myXml.ReadString("CLASS" & i, "End"))
            Classes(i).Stat(Stats.Vitality) = Val(myXml.ReadString("CLASS" & i, "Vit"))
            Classes(i).Stat(Stats.Luck) = Val(myXml.ReadString("CLASS" & i, "Luck"))
            Classes(i).Stat(Stats.Intelligence) = Val(myXml.ReadString("CLASS" & i, "Int"))
            Classes(i).Stat(Stats.Spirit) = Val(myXml.ReadString("CLASS" & i, "Speed"))

            Classes(i).BaseExp = Val(myXml.ReadString("CLASS" & i, "BaseExp"))

            Classes(i).StartMap = Val(myXml.ReadString("CLASS" & i, "StartMap"))
            Classes(i).StartX = Val(myXml.ReadString("CLASS" & i, "StartX"))
            Classes(i).StartY = Val(myXml.ReadString("CLASS" & i, "StartY"))

            ' loop for items & values
            For x = 1 To 5
                Classes(i).StartItem(x) = Val(myXml.ReadString("CLASS" & i, "StartItem" & x))
                Classes(i).StartValue(x) = Val(myXml.ReadString("CLASS" & i, "StartValue" & x))
            Next
        Next

        myXml.CloseXml(False)

    End Sub

    Sub SaveClasses()
        Dim tmpstring As String = ""
        Dim i As Integer
        Dim x As Integer

        Dim myXml As New XmlClass With {
            .Filename = Path.Combine(Application.StartupPath, "data", "Classes.xml"),
            .Root = "Data"
        }

        myXml.WriteString("INIT", "MaxClasses", Str(Max_Classes))

        For i = 1 To Max_Classes
            myXml.WriteString("CLASS" & i, "Name", Trim$(Classes(i).Name))
            myXml.WriteString("CLASS" & i, "Desc", Trim$(Classes(i).Desc))

            tmpstring = ""

            For x = 0 To UBound(Classes(i).MaleSprite)
                tmpstring = tmpstring & CStr(Classes(i).MaleSprite(x)) & ","
            Next

            myXml.WriteString("CLASS" & i, "MaleSprite", tmpstring.TrimEnd(","))

            tmpstring = ""

            For x = 0 To UBound(Classes(i).FemaleSprite)
                tmpstring = tmpstring & CStr(Classes(i).FemaleSprite(x)) & ","
            Next
            myXml.WriteString("CLASS" & i, "FemaleSprite", tmpstring.TrimEnd(","))

            tmpstring = ""

            myXml.WriteString("CLASS" & i, "Str", Str(Classes(i).Stat(Stats.Strength)))
            myXml.WriteString("CLASS" & i, "End", Str(Classes(i).Stat(Stats.Endurance)))
            myXml.WriteString("CLASS" & i, "Vit", Str(Classes(i).Stat(Stats.Vitality)))
            myXml.WriteString("CLASS" & i, "Luck", Str(Classes(i).Stat(Stats.Luck)))
            myXml.WriteString("CLASS" & i, "Int", Str(Classes(i).Stat(Stats.Intelligence)))
            myXml.WriteString("CLASS" & i, "Speed", Str(Classes(i).Stat(Stats.Spirit)))

            myXml.WriteString("CLASS" & i, "BaseExp", Str(Classes(i).BaseExp))

            myXml.WriteString("CLASS" & i, "StartMap", Str(Classes(i).StartMap))
            myXml.WriteString("CLASS" & i, "StartX", Str(Classes(i).StartX))
            myXml.WriteString("CLASS" & i, "StartY", Str(Classes(i).StartY))

            ' loop for items & values
            For x = 1 To 5
                myXml.WriteString("CLASS" & i, "StartItem" & x, Str(Classes(i).StartItem(x)))
                myXml.WriteString("CLASS" & i, "StartValue" & x, Str(Classes(i).StartValue(x)))
            Next

        Next

        myXml.CloseXml(True)

    End Sub

    Function GetClassMaxVital(ByVal ClassNum As Integer, ByVal Vital As Vitals) As Integer
        GetClassMaxVital = 0

        Select Case Vital
            Case Vitals.HP
                GetClassMaxVital = (1 + (Classes(ClassNum).Stat(Stats.Vitality) \ 2) + Classes(ClassNum).Stat(Stats.Vitality)) * 2
            Case Vitals.MP
                GetClassMaxVital = (1 + (Classes(ClassNum).Stat(Stats.Intelligence) \ 2) + Classes(ClassNum).Stat(Stats.Intelligence)) * 2
            Case Vitals.SP
                GetClassMaxVital = (1 + (Classes(ClassNum).Stat(Stats.Spirit) \ 2) + Classes(ClassNum).Stat(Stats.Spirit)) * 2
        End Select

    End Function

    Function GetClassName(ByVal ClassNum As Integer) As String
        GetClassName = Trim$(Classes(ClassNum).Name)
    End Function
#End Region

#Region "Maps"
    Sub CheckMaps()
        Dim i As Integer

        For i = 1 To MAX_MAPS
            If Not FileExist(Path.Combine(Application.StartupPath, "data", "maps", String.Format("map{0}.dat", i))) Then
                SaveMap(i)
            End If
        Next

    End Sub

    Sub ClearMaps()
        Dim i As Integer

        For i = 1 To MAX_CACHED_MAPS
            ClearMap(i)
            DoEvents()
        Next

    End Sub

    Sub ClearMap(MapNum As Integer)
        Dim x As Integer
        Dim y As Integer
        Map(MapNum) = Nothing
        Map(MapNum).Tileset = 1
        Map(MapNum).Name = ""
        Map(MapNum).MaxX = MAX_MAPX
        Map(MapNum).MaxY = MAX_MAPY
        ReDim Map(MapNum).Npc(0 To MAX_MAP_NPCS)
        ReDim Map(MapNum).Tile(0 To Map(MapNum).MaxX, 0 To Map(MapNum).MaxY)

        For x = 0 To MAX_MAPX
            For y = 0 To MAX_MAPY
                ReDim Map(MapNum).Tile(x, y).Layer(0 To MapLayer.Count - 1)
            Next
        Next

        Map(MapNum).EventCount = 0
        ReDim Map(MapNum).Events(0)

        ' Reset the values for if a player is on the map or not
        PlayersOnMap(MapNum) = False
        Map(MapNum).Tileset = 1
        Map(MapNum).Name = ""
        Map(MapNum).Music = ""
        Map(MapNum).MaxX = MAX_MAPX
        Map(MapNum).MaxY = MAX_MAPY

        ClearTempTile(MapNum)

    End Sub

    Sub SaveMaps()
        Dim i As Integer

        For i = 1 To MAX_MAPS
            SaveMap(i)
            SaveMapEvent(i)
        Next

    End Sub

    Sub SaveMap(MapNum As Integer)
        Dim filename As String
        Dim x As Integer, y As Integer, l As Integer

        filename = Path.Combine(Application.StartupPath, "data", "maps", String.Format("map{0}.dat", MapNum))
        Dim writer As New ByteStream(100)
        writer.WriteString(Map(MapNum).Name)
        writer.WriteString(Map(MapNum).Music)
        writer.WriteInt32(Map(MapNum).Revision)
        writer.WriteByte(Map(MapNum).Moral)
        writer.WriteInt32(Map(MapNum).Tileset)
        writer.WriteInt32(Map(MapNum).Up)
        writer.WriteInt32(Map(MapNum).Down)
        writer.WriteInt32(Map(MapNum).Left)
        writer.WriteInt32(Map(MapNum).Right)
        writer.WriteInt32(Map(MapNum).BootMap)
        writer.WriteByte(Map(MapNum).BootX)
        writer.WriteByte(Map(MapNum).BootY)
        writer.WriteByte(Map(MapNum).MaxX)
        writer.WriteByte(Map(MapNum).MaxY)
        writer.WriteByte(Map(MapNum).WeatherType)
        writer.WriteInt32(Map(MapNum).FogIndex)
        writer.WriteInt32(Map(MapNum).WeatherIntensity)
        writer.WriteByte(Map(MapNum).FogAlpha)
        writer.WriteByte(Map(MapNum).FogSpeed)
        writer.WriteByte(Map(MapNum).HasMapTint)
        writer.WriteByte(Map(MapNum).MapTintR)
        writer.WriteByte(Map(MapNum).MapTintG)
        writer.WriteByte(Map(MapNum).MapTintB)
        writer.WriteByte(Map(MapNum).MapTintA)

        writer.WriteByte(Map(MapNum).Instanced)
        writer.WriteByte(Map(MapNum).Panorama)
        writer.WriteByte(Map(MapNum).Parallax)

        For x = 0 To Map(MapNum).MaxX
            For y = 0 To Map(MapNum).MaxY
                writer.WriteInt32(Map(MapNum).Tile(x, y).Data1)
                writer.WriteInt32(Map(MapNum).Tile(x, y).Data2)
                writer.WriteInt32(Map(MapNum).Tile(x, y).Data3)
                writer.WriteByte(Map(MapNum).Tile(x, y).DirBlock)
                For l = 0 To MapLayer.Count - 1
                    writer.WriteByte(Map(MapNum).Tile(x, y).Layer(l).Tileset)
                    writer.WriteByte(Map(MapNum).Tile(x, y).Layer(l).X)
                    writer.WriteByte(Map(MapNum).Tile(x, y).Layer(l).Y)
                    writer.WriteByte(Map(MapNum).Tile(x, y).Layer(l).AutoTile)
                Next
                writer.WriteByte(Map(MapNum).Tile(x, y).Type)
            Next
        Next

        For x = 1 To MAX_MAP_NPCS
            writer.WriteInt32(Map(MapNum).Npc(x))
        Next

        BinaryFile.Save(filename, writer)

    End Sub

    Sub SaveMapEvent(MapNum As Integer)
        Dim myXml As New XmlClass With {
            .Filename = Application.StartupPath & "\data\maps\map" & MapNum & "_eventdata.xml",
            .Root = "Data"
        }

        If Not FileExist(Application.StartupPath & "\data\maps\map" & MapNum & "_eventdata.xml") Then
            myXml.NewXmlDocument()
        End If

        myXml.LoadXml()

        'This is for event saving, it is in .xml files because there are non-limited values (strings) that cannot easily be loaded/saved in the normal manner.
        myXml.WriteString("Events", "EventCount", Val(Map(MapNum).EventCount))

        If Map(MapNum).EventCount > 0 Then
            For i = 1 To Map(MapNum).EventCount
                With Map(MapNum).Events(i)
                    myXml.WriteString("Event" & i, "Name", .Name)
                    myXml.WriteString("Event" & i, "Global", Val(.Globals))
                    myXml.WriteString("Event" & i, "x", Val(.X))
                    myXml.WriteString("Event" & i, "y", Val(.Y))
                    myXml.WriteString("Event" & i, "PageCount", Val(.PageCount))

                End With
                If Map(MapNum).Events(i).PageCount > 0 Then
                    For x = 1 To Map(MapNum).Events(i).PageCount
                        With Map(MapNum).Events(i).Pages(x)
                            myXml.WriteString("Event" & i & "Page" & x, "chkVariable", Val(.chkVariable))
                            myXml.WriteString("Event" & i & "Page" & x, "VariableIndex", Val(.VariableIndex))
                            myXml.WriteString("Event" & i & "Page" & x, "VariableCondition", Val(.VariableCondition))
                            myXml.WriteString("Event" & i & "Page" & x, "VariableCompare", Val(.VariableCompare))

                            myXml.WriteString("Event" & i & "Page" & x, "chkSwitch", Val(.chkSwitch))
                            myXml.WriteString("Event" & i & "Page" & x, "SwitchIndex", Val(.SwitchIndex))
                            myXml.WriteString("Event" & i & "Page" & x, "SwitchCompare", Val(.SwitchCompare))

                            myXml.WriteString("Event" & i & "Page" & x, "chkHasItem", Val(.chkHasItem))
                            myXml.WriteString("Event" & i & "Page" & x, "HasItemIndex", Val(.HasItemIndex))
                            myXml.WriteString("Event" & i & "Page" & x, "HasItemAmount", Val(.HasItemAmount))

                            myXml.WriteString("Event" & i & "Page" & x, "chkSelfSwitch", Val(.chkSelfSwitch))
                            myXml.WriteString("Event" & i & "Page" & x, "SelfSwitchIndex", Val(.SelfSwitchIndex))
                            myXml.WriteString("Event" & i & "Page" & x, "SelfSwitchCompare", Val(.SelfSwitchCompare))

                            myXml.WriteString("Event" & i & "Page" & x, "GraphicType", Val(.GraphicType))
                            myXml.WriteString("Event" & i & "Page" & x, "Graphic", Val(.Graphic))
                            myXml.WriteString("Event" & i & "Page" & x, "GraphicX", Val(.GraphicX))
                            myXml.WriteString("Event" & i & "Page" & x, "GraphicY", Val(.GraphicY))
                            myXml.WriteString("Event" & i & "Page" & x, "GraphicX2", Val(.GraphicX2))
                            myXml.WriteString("Event" & i & "Page" & x, "GraphicY2", Val(.GraphicY2))

                            myXml.WriteString("Event" & i & "Page" & x, "MoveType", Val(.MoveType))
                            myXml.WriteString("Event" & i & "Page" & x, "MoveSpeed", Val(.MoveSpeed))
                            myXml.WriteString("Event" & i & "Page" & x, "MoveFreq", Val(.MoveFreq))

                            myXml.WriteString("Event" & i & "Page" & x, "IgnoreMoveRoute", Val(.IgnoreMoveRoute))
                            myXml.WriteString("Event" & i & "Page" & x, "RepeatMoveRoute", Val(.RepeatMoveRoute))

                            myXml.WriteString("Event" & i & "Page" & x, "MoveRouteCount", Val(.MoveRouteCount))

                            If .MoveRouteCount > 0 Then
                                For y = 1 To .MoveRouteCount
                                    myXml.WriteString("Event" & i & "Page" & x, "MoveRoute" & y & "Index", Val(.MoveRoute(y).Index))
                                    myXml.WriteString("Event" & i & "Page" & x, "MoveRoute" & y & "Data1", Val(.MoveRoute(y).Data1))
                                    myXml.WriteString("Event" & i & "Page" & x, "MoveRoute" & y & "Data2", Val(.MoveRoute(y).Data2))
                                    myXml.WriteString("Event" & i & "Page" & x, "MoveRoute" & y & "Data3", Val(.MoveRoute(y).Data3))
                                    myXml.WriteString("Event" & i & "Page" & x, "MoveRoute" & y & "Data4", Val(.MoveRoute(y).Data4))
                                    myXml.WriteString("Event" & i & "Page" & x, "MoveRoute" & y & "Data5", Val(.MoveRoute(y).Data5))
                                    myXml.WriteString("Event" & i & "Page" & x, "MoveRoute" & y & "Data6", Val(.MoveRoute(y).Data6))
                                Next
                            End If

                            myXml.WriteString("Event" & i & "Page" & x, "WalkAnim", Val(.WalkAnim))
                            myXml.WriteString("Event" & i & "Page" & x, "DirFix", Val(.DirFix))
                            myXml.WriteString("Event" & i & "Page" & x, "WalkThrough", Val(.WalkThrough))
                            myXml.WriteString("Event" & i & "Page" & x, "ShowName", Val(.ShowName))
                            myXml.WriteString("Event" & i & "Page" & x, "Trigger", Val(.Trigger))
                            myXml.WriteString("Event" & i & "Page" & x, "CommandListCount", Val(.CommandListCount))

                            myXml.WriteString("Event" & i & "Page" & x, "Position", Val(.Position))
                            myXml.WriteString("Event" & i & "Page" & x, "QuestNum", Val(.QuestNum))

                            myXml.WriteString("Event" & i & "Page" & x, "PlayerGender", Val(.chkPlayerGender))

                        End With

                        If Map(MapNum).Events(i).Pages(x).CommandListCount > 0 Then
                            For y = 1 To Map(MapNum).Events(i).Pages(x).CommandListCount
                                myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "CommandCount", Val(Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount))
                                myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "ParentList", Val(Map(MapNum).Events(i).Pages(x).CommandList(y).ParentList))

                                If Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount > 0 Then
                                    For z = 1 To Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount
                                        With Map(MapNum).Events(i).Pages(x).CommandList(y).Commands(z)
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "Index", Val(.Index))
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "Text1", .Text1)
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "Text2", .Text2)
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "Text3", .Text3)
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "Text4", .Text4)
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "Text5", .Text5)
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "Data1", Val(.Data1))
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "Data2", Val(.Data2))
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "Data3", Val(.Data3))
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "Data4", Val(.Data4))
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "Data5", Val(.Data5))
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "Data6", Val(.Data6))
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "ConditionalBranchCommandList", Val(.ConditionalBranch.CommandList))
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "ConditionalBranchCondition", Val(.ConditionalBranch.Condition))
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "ConditionalBranchData1", Val(.ConditionalBranch.Data1))
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "ConditionalBranchData2", Val(.ConditionalBranch.Data2))
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "ConditionalBranchData3", Val(.ConditionalBranch.Data3))
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "ConditionalBranchElseCommandList", Val(.ConditionalBranch.ElseCommandList))
                                            myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "MoveRouteCount", Val(.MoveRouteCount))

                                            If .MoveRouteCount > 0 Then
                                                For w = 1 To .MoveRouteCount
                                                    myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "MoveRoute" & w & "Index", Val(.MoveRoute(w).Index))
                                                    myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "MoveRoute" & w & "Data1", Val(.MoveRoute(w).Data1))
                                                    myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "MoveRoute" & w & "Data2", Val(.MoveRoute(w).Data2))
                                                    myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "MoveRoute" & w & "Data3", Val(.MoveRoute(w).Data3))
                                                    myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "MoveRoute" & w & "Data4", Val(.MoveRoute(w).Data4))
                                                    myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "MoveRoute" & w & "Data5", Val(.MoveRoute(w).Data5))
                                                    myXml.WriteString("Event" & i & "Page" & x, "CommandList" & y & "Command" & z & "MoveRoute" & w & "Data6", Val(.MoveRoute(w).Data6))
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

        myXml.CloseXml(True)
    End Sub

    Sub LoadMapEvent(MapNum As Integer)
        Dim myXml As New XmlClass With {
            .Filename = Application.StartupPath & "\data\maps\map" & MapNum & "_eventdata.xml",
            .Root = "Data"
        }

        myXml.LoadXml()
        Map(MapNum).EventCount = Val(myXml.ReadString("Events", "EventCount"))

        If Map(MapNum).EventCount > 0 Then
            ReDim Map(MapNum).Events(0 To Map(MapNum).EventCount)
            For i = 1 To Map(MapNum).EventCount
                If Val(myXml.ReadString("Event" & i, "PageCount")) > 0 Then

                    With Map(MapNum).Events(i)
                        .Name = myXml.ReadString("Event" & i, "Name")
                        .Globals = Val(myXml.ReadString("Event" & i, "Global"))
                        .X = Val(myXml.ReadString("Event" & i, "x"))
                        .Y = Val(myXml.ReadString("Event" & i, "y"))
                        .PageCount = Val(myXml.ReadString("Event" & i, "PageCount"))
                    End With
                    If Map(MapNum).Events(i).PageCount > 0 Then
                        ReDim Map(MapNum).Events(i).Pages(0 To Map(MapNum).Events(i).PageCount)
                        For x = 1 To Map(MapNum).Events(i).PageCount
                            With Map(MapNum).Events(i).Pages(x)
                                .chkVariable = Val(myXml.ReadString("Event" & i & "Page" & x, "chkVariable"))
                                .VariableIndex = Val(myXml.ReadString("Event" & i & "Page" & x, "VariableIndex"))
                                .VariableCondition = Val(myXml.ReadString("Event" & i & "Page" & x, "VariableCondition"))
                                .VariableCompare = Val(myXml.ReadString("Event" & i & "Page" & x, "VariableCompare"))

                                .chkSwitch = Val(myXml.ReadString("Event" & i & "Page" & x, "chkSwitch"))
                                .SwitchIndex = Val(myXml.ReadString("Event" & i & "Page" & x, "SwitchIndex"))
                                .SwitchCompare = Val(myXml.ReadString("Event" & i & "Page" & x, "SwitchCompare"))

                                .chkHasItem = Val(myXml.ReadString("Event" & i & "Page" & x, "chkHasItem"))
                                .HasItemIndex = Val(myXml.ReadString("Event" & i & "Page" & x, "HasItemIndex"))
                                .HasItemAmount = Val(myXml.ReadString("Event" & i & "Page" & x, "HasItemAmount"))

                                .chkSelfSwitch = Val(myXml.ReadString("Event" & i & "Page" & x, "chkSelfSwitch"))
                                .SelfSwitchIndex = Val(myXml.ReadString("Event" & i & "Page" & x, "SelfSwitchIndex"))
                                .SelfSwitchCompare = Val(myXml.ReadString("Event" & i & "Page" & x, "SelfSwitchCompare"))

                                .GraphicType = Val(myXml.ReadString("Event" & i & "Page" & x, "GraphicType"))
                                .Graphic = Val(myXml.ReadString("Event" & i & "Page" & x, "Graphic"))
                                .GraphicX = Val(myXml.ReadString("Event" & i & "Page" & x, "GraphicX"))
                                .GraphicY = Val(myXml.ReadString("Event" & i & "Page" & x, "GraphicY"))
                                .GraphicX2 = Val(myXml.ReadString("Event" & i & "Page" & x, "GraphicX2"))
                                .GraphicY2 = Val(myXml.ReadString("Event" & i & "Page" & x, "GraphicY2"))

                                .MoveType = Val(myXml.ReadString("Event" & i & "Page" & x, "MoveType"))
                                .MoveSpeed = Val(myXml.ReadString("Event" & i & "Page" & x, "MoveSpeed"))
                                .MoveFreq = Val(myXml.ReadString("Event" & i & "Page" & x, "MoveFreq"))

                                .IgnoreMoveRoute = Val(myXml.ReadString("Event" & i & "Page" & x, "IgnoreMoveRoute"))
                                .RepeatMoveRoute = Val(myXml.ReadString("Event" & i & "Page" & x, "RepeatMoveRoute"))

                                .MoveRouteCount = Val(myXml.ReadString("Event" & i & "Page" & x, "MoveRouteCount"))

                                If .MoveRouteCount > 0 Then
                                    ReDim Map(MapNum).Events(i).Pages(x).MoveRoute(0 To .MoveRouteCount)
                                    For y = 1 To .MoveRouteCount
                                        .MoveRoute(y).Index = Val(myXml.ReadString("Event" & i & "Page" & x, "MoveRoute" & y & "Index"))
                                        .MoveRoute(y).Data1 = Val(myXml.ReadString("Event" & i & "Page" & x, "MoveRoute" & y & "Data1"))
                                        .MoveRoute(y).Data2 = Val(myXml.ReadString("Event" & i & "Page" & x, "MoveRoute" & y & "Data2"))
                                        .MoveRoute(y).Data3 = Val(myXml.ReadString("Event" & i & "Page" & x, "MoveRoute" & y & "Data3"))
                                        .MoveRoute(y).Data4 = Val(myXml.ReadString("Event" & i & "Page" & x, "MoveRoute" & y & "Data4"))
                                        .MoveRoute(y).Data5 = Val(myXml.ReadString("Event" & i & "Page" & x, "MoveRoute" & y & "Data5"))
                                        .MoveRoute(y).Data6 = Val(myXml.ReadString("Event" & i & "Page" & x, "MoveRoute" & y & "Data6"))
                                    Next
                                End If

                                .WalkAnim = Val(myXml.ReadString("Event" & i & "Page" & x, "WalkAnim"))
                                .DirFix = Val(myXml.ReadString("Event" & i & "Page" & x, "DirFix"))
                                .WalkThrough = Val(myXml.ReadString("Event" & i & "Page" & x, "WalkThrough"))
                                .ShowName = Val(myXml.ReadString("Event" & i & "Page" & x, "ShowName"))
                                .Trigger = Val(myXml.ReadString("Event" & i & "Page" & x, "Trigger"))
                                .CommandListCount = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandListCount"))

                                .Position = Val(myXml.ReadString("Event" & i & "Page" & x, "Position"))
                                .QuestNum = Val(myXml.ReadString("Event" & i & "Page" & x, "QuestNum"))

                                .chkPlayerGender = Val(myXml.ReadString("Event" & i & "Page" & x, "PlayerGender"))
                            End With

                            If Map(MapNum).Events(i).Pages(x).CommandListCount > 0 Then
                                ReDim Map(MapNum).Events(i).Pages(x).CommandList(0 To Map(MapNum).Events(i).Pages(x).CommandListCount)
                                For y = 1 To Map(MapNum).Events(i).Pages(x).CommandListCount
                                    Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "CommandCount"))
                                    Map(MapNum).Events(i).Pages(x).CommandList(y).ParentList = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "ParentList"))
                                    If Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount > 0 Then
                                        ReDim Map(MapNum).Events(i).Pages(x).CommandList(y).Commands(Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount)
                                        For p = 1 To Map(MapNum).Events(i).Pages(x).CommandList(y).CommandCount
                                            With Map(MapNum).Events(i).Pages(x).CommandList(y).Commands(p)
                                                .Index = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "Index"))
                                                .Text1 = myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "Text1")
                                                .Text2 = myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "Text2")
                                                .Text3 = myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "Text3")
                                                .Text4 = myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "Text4")
                                                .Text5 = myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "Text5")
                                                .Data1 = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "Data1"))
                                                .Data2 = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "Data2"))
                                                .Data3 = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "Data3"))
                                                .Data4 = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "Data4"))
                                                .Data5 = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "Data5"))
                                                .Data6 = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "Data6"))
                                                .ConditionalBranch.CommandList = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "ConditionalBranchCommandList"))
                                                .ConditionalBranch.Condition = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "ConditionalBranchCondition"))
                                                .ConditionalBranch.Data1 = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "ConditionalBranchData1"))
                                                .ConditionalBranch.Data2 = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "ConditionalBranchData2"))
                                                .ConditionalBranch.Data3 = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "ConditionalBranchData3"))
                                                .ConditionalBranch.ElseCommandList = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "ConditionalBranchElseCommandList"))
                                                .MoveRouteCount = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "MoveRouteCount"))
                                                If .MoveRouteCount > 0 Then
                                                    ReDim .MoveRoute(0 To .MoveRouteCount)
                                                    For w = 1 To .MoveRouteCount
                                                        .MoveRoute(w).Index = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "MoveRoute" & w & "Index"))
                                                        .MoveRoute(w).Data1 = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "MoveRoute" & w & "Data1"))
                                                        .MoveRoute(w).Data2 = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "MoveRoute" & w & "Data2"))
                                                        .MoveRoute(w).Data3 = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "MoveRoute" & w & "Data3"))
                                                        .MoveRoute(w).Data4 = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "MoveRoute" & w & "Data4"))
                                                        .MoveRoute(w).Data5 = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "MoveRoute" & w & "Data5"))
                                                        .MoveRoute(w).Data6 = Val(myXml.ReadString("Event" & i & "Page" & x, "CommandList" & y & "Command" & p & "MoveRoute" & w & "Data6"))
                                                    Next
                                                End If
                                            End With
                                        Next
                                    End If
                                Next
                            End If
                        Next
                    End If
                End If
                DoEvents()
            Next
        End If

        myXml.CloseXml(False)
        If FileExist(Path.Combine(Application.StartupPath, "data", "maps", String.Format("map{0}_eventdata.dat", MapNum))) Then
            File.Delete(Path.Combine(Application.StartupPath, "data", "maps", String.Format("map{0}_eventdata.dat", MapNum)))
        End If
    End Sub

    Sub LoadMaps()
        Dim i As Integer

        CheckMaps()

        For i = 1 To MAX_MAPS
            LoadMap(i)
        Next
    End Sub

    Sub LoadMap(ByVal MapNum As Integer)
        Dim filename As String
        Dim x As Integer
        Dim y As Integer
        Dim l As Integer

        filename = Path.Combine(Application.StartupPath, "data", "maps", String.Format("map{0}.dat", MapNum))
        Dim reader As New ByteStream()
        BinaryFile.Load(filename, reader)

        Map(MapNum).Name = reader.ReadString()
        Map(MapNum).Music = reader.ReadString()
        Map(MapNum).Revision = reader.ReadInt32()
        Map(MapNum).Moral = reader.ReadByte()
        Map(MapNum).Tileset = reader.ReadInt32()
        Map(MapNum).Up = reader.ReadInt32()
        Map(MapNum).Down = reader.ReadInt32()
        Map(MapNum).Left = reader.ReadInt32()
        Map(MapNum).Right = reader.ReadInt32()
        Map(MapNum).BootMap = reader.ReadInt32()
        Map(MapNum).BootX = reader.ReadByte()
        Map(MapNum).BootY = reader.ReadByte()
        Map(MapNum).MaxX = reader.ReadByte()
        Map(MapNum).MaxY = reader.ReadByte()
        Map(MapNum).WeatherType = reader.ReadByte()
        Map(MapNum).FogIndex = reader.ReadInt32()
        Map(MapNum).WeatherIntensity = reader.ReadInt32()
        Map(MapNum).FogAlpha = reader.ReadByte()
        Map(MapNum).FogSpeed = reader.ReadByte()
        Map(MapNum).HasMapTint = reader.ReadByte()
        Map(MapNum).MapTintR = reader.ReadByte()
        Map(MapNum).MapTintG = reader.ReadByte()
        Map(MapNum).MapTintB = reader.ReadByte()
        Map(MapNum).MapTintA = reader.ReadByte()
        Map(MapNum).Instanced = reader.ReadByte()
        Map(MapNum).Panorama = reader.ReadByte()
        Map(MapNum).Parallax = reader.ReadByte()

        ' have to set the tile()
        ReDim Map(MapNum).Tile(0 To Map(MapNum).MaxX, 0 To Map(MapNum).MaxY)

        For x = 0 To Map(MapNum).MaxX
            For y = 0 To Map(MapNum).MaxY
                Map(MapNum).Tile(x, y).Data1 = reader.ReadInt32()
                Map(MapNum).Tile(x, y).Data2 = reader.ReadInt32()
                Map(MapNum).Tile(x, y).Data3 = reader.ReadInt32()
                Map(MapNum).Tile(x, y).DirBlock = reader.ReadByte()
                ReDim Map(MapNum).Tile(x, y).Layer(0 To MapLayer.Count - 1)
                For l = 0 To MapLayer.Count - 1
                    Map(MapNum).Tile(x, y).Layer(l).Tileset = reader.ReadByte()
                    Map(MapNum).Tile(x, y).Layer(l).X = reader.ReadByte()
                    Map(MapNum).Tile(x, y).Layer(l).Y = reader.ReadByte()
                    Map(MapNum).Tile(x, y).Layer(l).AutoTile = reader.ReadByte()
                Next
                Map(MapNum).Tile(x, y).Type = reader.ReadByte()
            Next
        Next

        For x = 1 To MAX_MAP_NPCS
            Map(MapNum).Npc(x) = reader.ReadInt32()
            MapNpc(MapNum).Npc(x).Num = Map(MapNum).Npc(x)
        Next

        ClearTempTile(MapNum)
        CacheResources(MapNum)

        If Map(MapNum).Name Is Nothing Then Map(MapNum).Name = ""
        If Map(MapNum).Music Is Nothing Then Map(MapNum).Music = ""

        If FileExist(Application.StartupPath & "\data\maps\map" & MapNum & "_eventdata.xml") Then
            LoadMapEvent(MapNum)
        End If

    End Sub

    Sub ClearTempTiles()
        Dim i As Integer

        ReDim TempTile(MAX_CACHED_MAPS)

        For i = 1 To MAX_CACHED_MAPS
            ClearTempTile(i)
        Next

    End Sub

    Sub ClearTempTile(ByVal MapNum As Integer)
        Dim y As Integer
        Dim x As Integer
        TempTile(MapNum).DoorTimer = 0
        ReDim TempTile(MapNum).DoorOpen(0 To Map(MapNum).MaxX, 0 To Map(MapNum).MaxY)

        For x = 0 To Map(MapNum).MaxX
            For y = 0 To Map(MapNum).MaxY
                TempTile(MapNum).DoorOpen(x, y) = False
            Next
        Next

    End Sub

    Sub ClearMapItem(ByVal Index As Integer, ByVal MapNum As Integer)
        MapItem(MapNum, Index) = Nothing
        MapItem(MapNum, Index).RandData.Prefix = ""
        MapItem(MapNum, Index).RandData.Suffix = ""
    End Sub

    Sub ClearMapItems()
        Dim x As Integer
        Dim y As Integer

        For y = 1 To MAX_CACHED_MAPS
            For x = 1 To MAX_MAP_ITEMS
                ClearMapItem(x, y)
            Next
        Next

    End Sub

#End Region

#Region "Items"
    Sub SaveItems()
        Dim i As Integer

        For i = 1 To MAX_ITEMS
            SaveItem(i)
            DoEvents()
        Next

    End Sub

    Sub SaveItem(ByVal itemNum As Integer)
        Dim filename As String
        filename = Path.Combine(Application.StartupPath, "data", "items", String.Format("item{0}.dat", itemNum))

        Dim writer As New ByteStream(100)
        writer.WriteString(Item(itemNum).Name)
        writer.WriteInt32(Item(itemNum).Pic)
        writer.WriteString(Item(itemNum).Description)

        writer.WriteByte(Item(itemNum).Type)
        writer.WriteByte(Item(itemNum).SubType)
        writer.WriteInt32(Item(itemNum).Data1)
        writer.WriteInt32(Item(itemNum).Data2)
        writer.WriteInt32(Item(itemNum).Data3)
        writer.WriteInt32(Item(itemNum).ClassReq)
        writer.WriteInt32(Item(itemNum).AccessReq)
        writer.WriteInt32(Item(itemNum).LevelReq)
        writer.WriteByte(Item(itemNum).Mastery)
        writer.WriteInt32(Item(itemNum).Price)

        For i = 0 To Stats.Count - 1
            writer.WriteByte(Item(itemNum).Add_Stat(i))
        Next

        writer.WriteByte(Item(itemNum).Rarity)
        writer.WriteInt32(Item(itemNum).Speed)
        writer.WriteInt32(Item(itemNum).TwoHanded)
        writer.WriteByte(Item(itemNum).BindType)

        For i = 0 To Stats.Count - 1
            writer.WriteByte(Item(itemNum).Stat_Req(i))
        Next

        writer.WriteInt32(Item(itemNum).Animation)
        writer.WriteInt32(Item(itemNum).Paperdoll)

        'Housing
        writer.WriteInt32(Item(itemNum).FurnitureWidth)
        writer.WriteInt32(Item(itemNum).FurnitureHeight)

        For a = 1 To 3
            For b = 1 To 3
                writer.WriteInt32(Item(itemNum).FurnitureBlocks(a, b))
                writer.WriteInt32(Item(itemNum).FurnitureFringe(a, b))
            Next
        Next

        writer.WriteByte(Item(itemNum).KnockBack)
        writer.WriteByte(Item(itemNum).KnockBackTiles)

        writer.WriteByte(Item(itemNum).Randomize)
        writer.WriteByte(Item(itemNum).RandomMin)
        writer.WriteByte(Item(itemNum).RandomMax)

        writer.WriteByte(Item(itemNum).Stackable)

        writer.WriteByte(Item(itemNum).ItemLevel)

        writer.WriteInt32(Item(itemNum).Projectile)
        writer.WriteInt32(Item(itemNum).Ammo)

        BinaryFile.Save(filename, writer)
    End Sub

    Sub LoadItems()
        Dim i As Integer

        CheckItems()

        For i = 1 To MAX_ITEMS
            LoadItem(i)
            DoEvents()
        Next
        'SaveItems()
    End Sub

    Sub LoadItem(ByVal ItemNum As Integer)
        Dim filename As String
        Dim s As Integer

        filename = Path.Combine(Application.StartupPath, "data", "items", String.Format("item{0}.dat", ItemNum))

        Dim reader As New ByteStream()
        BinaryFile.Load(filename, reader)

        Item(ItemNum).Name = reader.ReadString()
        Item(ItemNum).Pic = reader.ReadInt32()
        Item(ItemNum).Description = reader.ReadString()

        Item(ItemNum).Type = reader.ReadByte()
        Item(ItemNum).SubType = reader.ReadByte()
        Item(ItemNum).Data1 = reader.ReadInt32()
        Item(ItemNum).Data2 = reader.ReadInt32()
        Item(ItemNum).Data3 = reader.ReadInt32()
        Item(ItemNum).ClassReq = reader.ReadInt32()
        Item(ItemNum).AccessReq = reader.ReadInt32()
        Item(ItemNum).LevelReq = reader.ReadInt32()
        Item(ItemNum).Mastery = reader.ReadByte()
        Item(ItemNum).Price = reader.ReadInt32()

        For s = 0 To Stats.Count - 1
            Item(ItemNum).Add_Stat(s) = reader.ReadByte()
        Next

        Item(ItemNum).Rarity = reader.ReadByte()
        Item(ItemNum).Speed = reader.ReadInt32()
        Item(ItemNum).TwoHanded = reader.ReadInt32()
        Item(ItemNum).BindType = reader.ReadByte()

        For s = 0 To Stats.Count - 1
            Item(ItemNum).Stat_Req(s) = reader.ReadByte()
        Next

        Item(ItemNum).Animation = reader.ReadInt32()
        Item(ItemNum).Paperdoll = reader.ReadInt32()

        'Housing
        Item(ItemNum).FurnitureWidth = reader.ReadInt32()
        Item(ItemNum).FurnitureHeight = reader.ReadInt32()

        For a = 1 To 3
            For b = 1 To 3
                Item(ItemNum).FurnitureBlocks(a, b) = reader.ReadInt32()
                Item(ItemNum).FurnitureFringe(a, b) = reader.ReadInt32()
            Next
        Next

        Item(ItemNum).KnockBack = reader.ReadByte()
        Item(ItemNum).KnockBackTiles = reader.ReadByte()

        Item(ItemNum).Randomize = reader.ReadByte()
        Item(ItemNum).RandomMin = reader.ReadByte()
        Item(ItemNum).RandomMax = reader.ReadByte()

        Item(ItemNum).Stackable = reader.ReadByte()

        Item(ItemNum).ItemLevel = reader.ReadByte()

        Item(ItemNum).Projectile = reader.ReadInt32()
        Item(ItemNum).Ammo = reader.ReadInt32()

    End Sub

    Sub CheckItems()
        Dim i As Integer

        For i = 1 To MAX_ITEMS

            If Not FileExist(Path.Combine(Application.StartupPath, "data", "items", String.Format("item{0}.dat", i))) Then
                SaveItem(i)
            End If

        Next

    End Sub

    Sub ClearItem(ByVal Index As Integer)
        Item(Index) = Nothing
        Item(Index).Name = ""
        Item(Index).Description = ""

        For i = 0 To MAX_ITEMS
            ReDim Item(i).Add_Stat(0 To Stats.Count - 1)
            ReDim Item(i).Stat_Req(0 To Stats.Count - 1)
            ReDim Item(i).FurnitureBlocks(0 To 3, 0 To 3)
            ReDim Item(i).FurnitureFringe(0 To 3, 0 To 3)
        Next

    End Sub

    Sub ClearItems()
        Dim i As Integer

        For i = 1 To MAX_ITEMS
            ClearItem(i)
        Next

    End Sub

#End Region

#Region "Npc's"
    Sub SaveNpcs()
        Dim i As Integer

        For i = 1 To MAX_NPCS
            SaveNpc(i)
            DoEvents()
        Next

    End Sub

    Sub SaveNpc(ByVal NpcNum As Integer)
        Dim filename As String
        Dim i As Integer
        filename = Path.Combine(Application.StartupPath, "data", "npcs", String.Format("npc{0}.dat", NpcNum))

        Dim writer As New ByteStream(100)
        writer.WriteString(Npc(NpcNum).Name)
        writer.WriteString(Npc(NpcNum).AttackSay)
        writer.WriteInt32(Npc(NpcNum).Sprite)
        writer.WriteByte(Npc(NpcNum).SpawnTime)
        writer.WriteInt32(Npc(NpcNum).SpawnSecs)
        writer.WriteByte(Npc(NpcNum).Behaviour)
        writer.WriteByte(Npc(NpcNum).Range)

        For i = 1 To 5
            writer.WriteInt32(Npc(NpcNum).DropChance(i))
            writer.WriteInt32(Npc(NpcNum).DropItem(i))
            writer.WriteInt32(Npc(NpcNum).DropItemValue(i))
        Next

        For i = 0 To Stats.Count - 1
            writer.WriteByte(Npc(NpcNum).Stat(i))
        Next

        writer.WriteByte(Npc(NpcNum).Faction)
        writer.WriteInt32(Npc(NpcNum).Hp)
        writer.WriteInt32(Npc(NpcNum).Exp)
        writer.WriteInt32(Npc(NpcNum).Animation)

        writer.WriteInt32(Npc(NpcNum).QuestNum)

        For i = 1 To MAX_NPC_SKILLS
            writer.WriteByte(Npc(NpcNum).Skill(i))
        Next

        writer.WriteInt32(Npc(NpcNum).Level)
        writer.WriteInt32(Npc(NpcNum).Damage)

        BinaryFile.Save(filename, writer)
    End Sub

    Sub LoadNpcs()
        Dim i As Integer

        CheckNpcs()

        For i = 1 To MAX_NPCS
            LoadNpc(i)
            DoEvents()
        Next
        'SaveNpcs()
    End Sub

    Sub LoadNpc(ByVal NpcNum As Integer)
        Dim filename As String
        Dim n As Integer

        filename = Path.Combine(Application.StartupPath, "data", "npcs", String.Format("npc{0}.dat", NpcNum))
        Dim reader As New ByteStream()
        BinaryFile.Load(filename, reader)

        Npc(NpcNum).Name = reader.ReadString()
        Npc(NpcNum).AttackSay = reader.ReadString()
        Npc(NpcNum).Sprite = reader.ReadInt32()
        Npc(NpcNum).SpawnTime = reader.ReadByte()
        Npc(NpcNum).SpawnSecs = reader.ReadInt32()
        Npc(NpcNum).Behaviour = reader.ReadByte()
        Npc(NpcNum).Range = reader.ReadByte()

        For i = 1 To 5
            Npc(NpcNum).DropChance(i) = reader.ReadInt32()
            Npc(NpcNum).DropItem(i) = reader.ReadInt32()
            Npc(NpcNum).DropItemValue(i) = reader.ReadInt32()
        Next

        For n = 0 To Stats.Count - 1
            Npc(NpcNum).Stat(n) = reader.ReadByte()
        Next

        Npc(NpcNum).Faction = reader.ReadByte()
        Npc(NpcNum).Hp = reader.ReadInt32()
        Npc(NpcNum).Exp = reader.ReadInt32()
        Npc(NpcNum).Animation = reader.ReadInt32()

        Npc(NpcNum).QuestNum = reader.ReadInt32()

        For i = 1 To MAX_NPC_SKILLS
            Npc(NpcNum).Skill(i) = reader.ReadByte()
        Next

        Npc(NpcNum).Level = reader.ReadInt32()
        Npc(NpcNum).Damage = reader.ReadInt32()

        If Npc(NpcNum).Name Is Nothing Then Npc(NpcNum).Name = ""
        If Npc(NpcNum).AttackSay Is Nothing Then Npc(NpcNum).AttackSay = ""
    End Sub

    Sub CheckNpcs()
        Dim i As Integer

        For i = 1 To MAX_NPCS
            If Not FileExist(Path.Combine(Application.StartupPath, "data", "npcs", String.Format("npc{0}.dat", i))) Then
                SaveNpc(i)
                DoEvents()
            End If

        Next

    End Sub

    Sub ClearMapNpc(ByVal Index As Integer, ByVal MapNum As Integer)
        MapNpc(MapNum).Npc(Index) = Nothing

        ReDim MapNpc(MapNum).Npc(Index).Vital(0 To Vitals.Count)
        ReDim MapNpc(MapNum).Npc(Index).SkillCD(MAX_NPC_SKILLS)
    End Sub

    Sub ClearAllMapNpcs()
        Dim y As Integer

        For y = 1 To MAX_CACHED_MAPS
            ClearMapNpcs(y)
            DoEvents()
        Next

    End Sub

    Sub ClearMapNpcs(ByVal MapNum As Integer)
        Dim x As Integer
        Dim y As Integer

        For x = 1 To MAX_MAP_NPCS
            ClearMapNpc(x, y)
            DoEvents()
        Next

    End Sub

    Sub ClearNpc(ByVal Index As Integer)
        Npc(Index) = Nothing
        Npc(Index).Name = ""
        Npc(Index).AttackSay = ""
        ReDim Npc(Index).Stat(0 To Stats.Count - 1)
        For i = 1 To 5
            ReDim Npc(Index).DropChance(5)
            ReDim Npc(Index).DropItem(5)
            ReDim Npc(Index).DropItemValue(5)
            ReDim Npc(Index).Skill(MAX_NPC_SKILLS)
        Next
    End Sub

    Sub ClearNpcs()
        Dim i As Integer

        For i = 1 To MAX_NPCS
            ClearNpc(i)
            DoEvents()
        Next

    End Sub

#End Region

#Region "Resources"
    Sub SaveResources()
        Dim i As Integer

        For i = 1 To MAX_RESOURCES
            SaveResource(i)
            DoEvents()
        Next

    End Sub

    Sub SaveResource(ByVal ResourceNum As Integer)
        Dim filename As String

        filename = Path.Combine(Application.StartupPath, "data", "resources", String.Format("resource{0}.dat", ResourceNum))

        Dim writer As New ByteStream(100)

        writer.WriteString(Resource(ResourceNum).Name)
        writer.WriteString(Resource(ResourceNum).SuccessMessage)
        writer.WriteString(Resource(ResourceNum).EmptyMessage)
        writer.WriteInt32(Resource(ResourceNum).ResourceType)
        writer.WriteInt32(Resource(ResourceNum).ResourceImage)
        writer.WriteInt32(Resource(ResourceNum).ExhaustedImage)
        writer.WriteInt32(Resource(ResourceNum).ExpReward)
        writer.WriteInt32(Resource(ResourceNum).ItemReward)
        writer.WriteInt32(Resource(ResourceNum).LvlRequired)
        writer.WriteInt32(Resource(ResourceNum).ToolRequired)
        writer.WriteInt32(Resource(ResourceNum).Health)
        writer.WriteInt32(Resource(ResourceNum).RespawnTime)
        writer.WriteBoolean(Resource(ResourceNum).Walkthrough)
        writer.WriteInt32(Resource(ResourceNum).Animation)

        BinaryFile.Save(filename, writer)
    End Sub

    Sub LoadResources()
        Dim i As Integer

        Call CheckResources()

        For i = 1 To MAX_RESOURCES
            LoadResource(i)
            DoEvents()
        Next

    End Sub

    Sub LoadResource(ByVal ResourceNum As Integer)
        Dim filename As String

        filename = Path.Combine(Application.StartupPath, "data", "resources", String.Format("resource{0}.dat", ResourceNum))
        Dim reader As New ByteStream()
        BinaryFile.Load(filename, reader)

        Resource(ResourceNum).Name = reader.ReadString()
        Resource(ResourceNum).SuccessMessage = reader.ReadString()
        Resource(ResourceNum).EmptyMessage = reader.ReadString()
        Resource(ResourceNum).ResourceType = reader.ReadInt32()
        Resource(ResourceNum).ResourceImage = reader.ReadInt32()
        Resource(ResourceNum).ExhaustedImage = reader.ReadInt32()
        Resource(ResourceNum).ExpReward = reader.ReadInt32()
        Resource(ResourceNum).ItemReward = reader.ReadInt32()
        Resource(ResourceNum).LvlRequired = reader.ReadInt32()
        Resource(ResourceNum).ToolRequired = reader.ReadInt32()
        Resource(ResourceNum).Health = reader.ReadInt32()
        Resource(ResourceNum).RespawnTime = reader.ReadInt32()
        Resource(ResourceNum).Walkthrough = reader.ReadBoolean()
        Resource(ResourceNum).Animation = reader.ReadInt32()

        If Resource(ResourceNum).Name Is Nothing Then Resource(ResourceNum).Name = ""
        If Resource(ResourceNum).EmptyMessage Is Nothing Then Resource(ResourceNum).EmptyMessage = ""
        If Resource(ResourceNum).SuccessMessage Is Nothing Then Resource(ResourceNum).SuccessMessage = ""

    End Sub

    Sub CheckResources()
        Dim i As Integer

        For i = 1 To MAX_RESOURCES

            If Not FileExist(Path.Combine(Application.StartupPath, "data", "resources", String.Format("resource{0}.dat", i))) Then
                SaveResource(i)
                DoEvents()
            End If

        Next

    End Sub

    Sub ClearResource(ByVal Index As Integer)
        Resource(Index) = Nothing
        Resource(Index).Name = ""
        Resource(Index).EmptyMessage = ""
        Resource(Index).SuccessMessage = ""
    End Sub

    Sub ClearResources()
        Dim i As Integer

        For i = 1 To MAX_RESOURCES
            ClearResource(i)
            DoEvents()
        Next
    End Sub

    Public Sub CacheResources(ByVal MapNum As Integer)
        Dim x As Integer, y As Integer, Resource_Count As Integer
        Resource_Count = 0

        For x = 0 To Map(MapNum).MaxX
            For y = 0 To Map(MapNum).MaxY

                If Map(MapNum).Tile(x, y).Type = TileType.Resource Then
                    Resource_Count = Resource_Count + 1
                    ReDim Preserve ResourceCache(MapNum).ResourceData(0 To Resource_Count)
                    ResourceCache(MapNum).ResourceData(Resource_Count).X = x
                    ResourceCache(MapNum).ResourceData(Resource_Count).Y = y
                    ResourceCache(MapNum).ResourceData(Resource_Count).Cur_Health = Resource(Map(MapNum).Tile(x, y).Data1).Health
                End If

            Next
        Next

        ResourceCache(MapNum).Resource_Count = Resource_Count
    End Sub

#End Region

#Region "Shops"
    Sub SaveShops()
        Dim i As Integer

        For i = 1 To MAX_SHOPS
            SaveShop(i)
            DoEvents()
        Next

    End Sub

    Sub SaveShop(ByVal shopNum As Integer)
        Dim i As Integer
        Dim filename As String

        filename = Path.Combine(Application.StartupPath, "data", "shops", String.Format("shop{0}.dat", shopNum))

        Dim writer As New ByteStream(100)

        writer.WriteString(Shop(shopNum).Name)
        writer.WriteByte(Shop(shopNum).Face)
        writer.WriteInt32(Shop(shopNum).BuyRate)

        For i = 1 To MAX_TRADES
            writer.WriteInt32(Shop(shopNum).TradeItem(i).Item)
            writer.WriteInt32(Shop(shopNum).TradeItem(i).ItemValue)
            writer.WriteInt32(Shop(shopNum).TradeItem(i).CostItem)
            writer.WriteInt32(Shop(shopNum).TradeItem(i).CostValue)
        Next

        BinaryFile.Save(filename, writer)
    End Sub

    Sub LoadShops()

        Dim i As Integer

        CheckShops()

        For i = 1 To MAX_SHOPS
            LoadShop(i)
            DoEvents()
        Next

    End Sub

    Sub LoadShop(ByVal ShopNum As Integer)
        Dim filename As String
        Dim x As Integer

        filename = Path.Combine(Application.StartupPath, "data", "shops", String.Format("shop{0}.dat", ShopNum))
        Dim reader As New ByteStream()
        BinaryFile.Load(filename, reader)

        Shop(ShopNum).Name = reader.ReadString()
        Shop(ShopNum).Face = reader.ReadByte()
        Shop(ShopNum).BuyRate = reader.ReadInt32()

        For x = 1 To MAX_TRADES
            Shop(ShopNum).TradeItem(x).Item = reader.ReadInt32()
            Shop(ShopNum).TradeItem(x).ItemValue = reader.ReadInt32()
            Shop(ShopNum).TradeItem(x).CostItem = reader.ReadInt32()
            Shop(ShopNum).TradeItem(x).CostValue = reader.ReadInt32()
        Next

    End Sub

    Sub CheckShops()
        Dim i As Integer

        For i = 1 To MAX_SHOPS

            If Not FileExist(Path.Combine(Application.StartupPath, "data", "shops", String.Format("shop{0}.dat", i))) Then
                SaveShop(i)
                DoEvents()
            End If

        Next

    End Sub

    Sub ClearShop(ByVal Index As Integer)
        Dim i As Integer

        Shop(Index) = Nothing
        Shop(Index).Name = ""

        ReDim Shop(Index).TradeItem(MAX_TRADES)
        For i = 0 To MAX_SHOPS
            ReDim Shop(i).TradeItem(0 To MAX_TRADES)
        Next

    End Sub

    Sub ClearShops()
        Dim i As Integer

        For i = 1 To MAX_SHOPS
            Call ClearShop(i)
        Next

    End Sub

#End Region

#Region "Skills"
    Sub SaveSkills()
        Dim i As Integer
        Console.WriteLine("Saving skills... ")

        For i = 1 To MAX_SKILLS
            SaveSkill(i)
            DoEvents()
        Next

    End Sub

    Sub SaveSkill(ByVal skillnum As Integer)
        Dim filename As String
        filename = Path.Combine(Application.StartupPath, "data", "skills", String.Format("skills{0}.dat", skillnum))

        Dim writer As New ByteStream(100)

        writer.WriteString(Skill(skillnum).Name)
        writer.WriteByte(Skill(skillnum).Type)
        writer.WriteInt32(Skill(skillnum).MpCost)
        writer.WriteInt32(Skill(skillnum).LevelReq)
        writer.WriteInt32(Skill(skillnum).AccessReq)
        writer.WriteInt32(Skill(skillnum).ClassReq)
        writer.WriteInt32(Skill(skillnum).CastTime)
        writer.WriteInt32(Skill(skillnum).CdTime)
        writer.WriteInt32(Skill(skillnum).Icon)
        writer.WriteInt32(Skill(skillnum).Map)
        writer.WriteInt32(Skill(skillnum).X)
        writer.WriteInt32(Skill(skillnum).Y)
        writer.WriteByte(Skill(skillnum).Dir)
        writer.WriteInt32(Skill(skillnum).Vital)
        writer.WriteInt32(Skill(skillnum).Duration)
        writer.WriteInt32(Skill(skillnum).Interval)
        writer.WriteInt32(Skill(skillnum).Range)
        writer.WriteBoolean(Skill(skillnum).IsAoE)
        writer.WriteInt32(Skill(skillnum).AoE)
        writer.WriteInt32(Skill(skillnum).CastAnim)
        writer.WriteInt32(Skill(skillnum).SkillAnim)
        writer.WriteInt32(Skill(skillnum).StunDuration)

        writer.WriteInt32(Skill(skillnum).IsProjectile)
        writer.WriteInt32(Skill(skillnum).Projectile)

        writer.WriteByte(Skill(skillnum).KnockBack)
        writer.WriteByte(Skill(skillnum).KnockBackTiles)

        BinaryFile.Save(filename, writer)
    End Sub

    Sub LoadSkills()
        Dim i As Integer

        CheckSkills()

        For i = 1 To MAX_SKILLS
            LoadSkill(i)
            DoEvents()
        Next

    End Sub

    Sub LoadSkill(ByVal SkillNum As Integer)
        Dim filename As String

        filename = Path.Combine(Application.StartupPath, "data", "skills", String.Format("skills{0}.dat", SkillNum))
        Dim reader As New ByteStream()
        BinaryFile.Load(filename, reader)

        Skill(SkillNum).Name = reader.ReadString()
        Skill(SkillNum).Type = reader.ReadByte()
        Skill(SkillNum).MpCost = reader.ReadInt32()
        Skill(SkillNum).LevelReq = reader.ReadInt32()
        Skill(SkillNum).AccessReq = reader.ReadInt32()
        Skill(SkillNum).ClassReq = reader.ReadInt32()
        Skill(SkillNum).CastTime = reader.ReadInt32()
        Skill(SkillNum).CdTime = reader.ReadInt32()
        Skill(SkillNum).Icon = reader.ReadInt32()
        Skill(SkillNum).Map = reader.ReadInt32()
        Skill(SkillNum).X = reader.ReadInt32()
        Skill(SkillNum).Y = reader.ReadInt32()
        Skill(SkillNum).Dir = reader.ReadByte()
        Skill(SkillNum).Vital = reader.ReadInt32()
        Skill(SkillNum).Duration = reader.ReadInt32()
        Skill(SkillNum).Interval = reader.ReadInt32()
        Skill(SkillNum).Range = reader.ReadInt32()
        Skill(SkillNum).IsAoE = reader.ReadBoolean()
        Skill(SkillNum).AoE = reader.ReadInt32()
        Skill(SkillNum).CastAnim = reader.ReadInt32()
        Skill(SkillNum).SkillAnim = reader.ReadInt32()
        Skill(SkillNum).StunDuration = reader.ReadInt32()

        Skill(SkillNum).IsProjectile = reader.ReadInt32()
        Skill(SkillNum).Projectile = reader.ReadInt32()

        Skill(SkillNum).KnockBack = reader.ReadByte()
        Skill(SkillNum).KnockBackTiles = reader.ReadByte()

    End Sub

    Sub CheckSkills()
        Dim i As Integer

        For i = 1 To MAX_SKILLS

            If Not FileExist(Path.Combine(Application.StartupPath, "data", "skills", String.Format("skills{0}.dat", i))) Then
                SaveSkill(i)
                DoEvents()
            End If

        Next

    End Sub

    Sub ClearSkill(ByVal Index As Integer)
        Skill(Index) = Nothing
        Skill(Index).Name = ""
        Skill(Index).LevelReq = 1 'Needs to be 1 for the skill editor
    End Sub

    Sub ClearSkills()
        Dim i As Integer

        For i = 1 To MAX_SKILLS
            ClearSkill(i)
            DoEvents()
        Next

    End Sub

#End Region

#Region "Animations"
    Sub SaveAnimations()
        Dim i As Integer

        For i = 1 To MAX_ANIMATIONS
            SaveAnimation(i)
            DoEvents()
        Next

    End Sub

    Sub SaveAnimation(ByVal AnimationNum As Integer)
        Dim filename As String
        Dim x As Integer

        filename = Path.Combine(Application.StartupPath, "data", "animations", String.Format("animation{0}.dat", AnimationNum))

        Dim writer As New ByteStream(100)

        writer.WriteString(Animation(AnimationNum).Name)
        writer.WriteString(Animation(AnimationNum).Sound)

        For x = 0 To UBound(Animation(AnimationNum).Sprite)
            writer.WriteInt32(Animation(AnimationNum).Sprite(x))
        Next

        For x = 0 To UBound(Animation(AnimationNum).Frames)
            writer.WriteInt32(Animation(AnimationNum).Frames(x))
        Next

        For x = 0 To UBound(Animation(AnimationNum).LoopCount)
            writer.WriteInt32(Animation(AnimationNum).LoopCount(x))
        Next

        For x = 0 To UBound(Animation(AnimationNum).LoopTime)
            writer.WriteInt32(Animation(AnimationNum).LoopTime(x))
        Next

        BinaryFile.Save(filename, writer)
    End Sub

    Sub LoadAnimations()
        Dim i As Integer

        CheckAnimations()

        For i = 1 To MAX_ANIMATIONS
            LoadAnimation(i)
            DoEvents()
        Next

    End Sub

    Sub LoadAnimation(ByVal AnimationNum As Integer)
        Dim filename As String

        filename = Path.Combine(Application.StartupPath, "data", "animations", String.Format("animation{0}.dat", AnimationNum))
        Dim reader As New ByteStream()
        BinaryFile.Load(filename, reader)

        Animation(AnimationNum).Name = reader.ReadString()
        Animation(AnimationNum).Sound = reader.ReadString()

        For x = 0 To UBound(Animation(AnimationNum).Sprite)
            Animation(AnimationNum).Sprite(x) = reader.ReadInt32()
        Next

        For x = 0 To UBound(Animation(AnimationNum).Frames)
            Animation(AnimationNum).Frames(x) = reader.ReadInt32()
        Next

        For x = 0 To UBound(Animation(AnimationNum).LoopCount)
            Animation(AnimationNum).LoopCount(x) = reader.ReadInt32()
        Next

        For x = 0 To UBound(Animation(AnimationNum).LoopTime)
            Animation(AnimationNum).LoopTime(x) = reader.ReadInt32()
        Next

        If Animation(AnimationNum).Name Is Nothing Then Animation(AnimationNum).Name = ""
    End Sub

    Sub CheckAnimations()
        Dim i As Integer

        For i = 1 To MAX_ANIMATIONS

            If Not FileExist(Path.Combine(Application.StartupPath, "data", "animations", String.Format("animation{0}.dat", i))) Then
                SaveAnimation(i)
                DoEvents()
            End If

        Next
    End Sub

    Sub ClearAnimation(ByVal Index As Integer)
        Animation(Index) = Nothing
        Animation(Index).Name = ""
        Animation(Index).Sound = ""
        ReDim Animation(Index).Sprite(0 To 1)
        ReDim Animation(Index).Frames(0 To 1)
        ReDim Animation(Index).LoopCount(0 To 1)
        ReDim Animation(Index).LoopTime(0 To 1)
    End Sub

    Sub ClearAnimations()
        Dim i As Integer

        For i = 1 To MAX_ANIMATIONS
            ClearAnimation(i)
            DoEvents()
        Next
    End Sub

#End Region

#Region "Accounts"
    Function AccountExist(ByVal Name As String) As Boolean
        Dim filename As String
        filename = Path.Combine(Application.StartupPath, "data", "accounts", Trim$(Name), String.Format("{0}.bin", Trim$(Name)))

        If FileExist(filename) Then
            AccountExist = True
        Else
            AccountExist = False
        End If
    End Function

    Function PasswordOK(ByVal Name As String, ByVal Password As String) As Boolean
        Dim filename As String
        Dim RightPassword As String
        Dim namecheck As String

        RightPassword = ""
        namecheck = ""
        PasswordOK = False

        If AccountExist(Name) Then
            filename = Path.Combine(Application.StartupPath, "data", "accounts", Trim$(Name), String.Format("{0}.bin", Trim$(Name)))
            Dim reader As New ByteStream()
            BinaryFile.Load(filename, reader)
            namecheck = reader.ReadString()
            RightPassword = reader.ReadString()

            If Trim(namecheck) <> Trim(Name) Then
                Exit Function
            End If

            If UCase$(Trim$(Password)) = UCase$(Trim$(RightPassword)) Then
                PasswordOK = True
            Else
                PasswordOK = False
            End If
        End If
    End Function

    Sub AddAccount(ByVal Index As Integer, ByVal Name As String, ByVal Password As String)
        ClearPlayer(Index)

        Player(Index).Login = Name
        Player(Index).Password = Password

        SavePlayer(Index)
    End Sub

    Sub DeleteName(ByVal Name As String)

        Dim fileName As String = Path.Combine(Application.StartupPath, "data", "accounts", "charlist.txt")

        ' Read the file line by line
        Dim fileContents = File.ReadAllLines(fileName).ToList

        ' Remove unwanted stuff
        For i = 0 To fileContents.Count - 1
            If fileContents(i).Contains(Trim$(LCase$(Name))) Then
                fileContents.RemoveAt(i)
            End If
        Next

        ' Write the file to disk
        File.WriteAllLines(fileName, fileContents.ToArray)
    End Sub

#End Region

#Region "Players"
    Sub SaveAllPlayersOnline()
        Dim i As Integer

        For i = 1 To GetPlayersOnline()
            If IsPlaying(i) Then
                SavePlayer(i)
                SaveBank(i)
                DoEvents()
            End If
        Next

    End Sub

    Sub SavePlayer(ByVal Index As Integer)
        Dim filename As String
        Dim playername As String

        playername = Trim$(Player(Index).Login)
        CheckDir(Path.Combine(Application.StartupPath, "data", "accounts", playername))

        filename = Path.Combine(Application.StartupPath, "data", "accounts", playername, String.Format("{0}.bin", playername))

        Dim writer As New ByteStream(100)

        writer.WriteString(Player(Index).Login)
        writer.WriteString(Player(Index).Password)
        writer.WriteByte(Player(Index).Access)

        BinaryFile.Save(filename, writer)

        For i = 1 To MAX_CHARS
            SaveCharacter(Index, i)
        Next

    End Sub

    Sub LoadPlayer(ByVal Index As Integer, ByVal Name As String)
        Dim filename As String

        ClearPlayer(Index)

        filename = Path.Combine(Application.StartupPath, "data", "accounts", Name.Trim(), String.Format("{0}.bin", Name.Trim()))
        Dim reader As New ByteStream()
        BinaryFile.Load(filename, reader)

        Player(Index).Login = reader.ReadString()
        Player(Index).Password = reader.ReadString()
        Player(Index).Access = reader.ReadByte()

        For i = 1 To MAX_CHARS
            LoadCharacter(Index, i)
        Next

    End Sub

    Sub ClearPlayer(ByVal Index As Integer)
        Dim i As Integer

        ReDim TempPlayer(Index).SkillCD(MAX_PLAYER_SKILLS)

        Player(Index).Login = ""
        Player(Index).Password = ""

        Player(Index).Access = 0

        For i = 1 To MAX_CHARS
            ClearCharacter(Index, i)
        Next

    End Sub

#End Region

#Region "Bank"
    Public Sub LoadBank(ByVal Index As Integer, ByVal Name As String)
        Dim filename As String

        ClearBank(Index)

        filename = Path.Combine(Application.StartupPath, "data", "banks", String.Format("{0}.bin", Name.Trim()))

        If Not FileExist(filename) Then
            SaveBank(Index)
            Exit Sub
        End If

        Dim reader As New ByteStream()
        BinaryFile.Load(filename, reader)

        For i = 1 To MAX_BANK
            Bank(Index).Item(i).Num = reader.ReadByte()
            Bank(Index).Item(i).Value = reader.ReadInt32()

            Bank(Index).ItemRand(i).Prefix = reader.ReadString()
            Bank(Index).ItemRand(i).Suffix = reader.ReadString()
            Bank(Index).ItemRand(i).Rarity = reader.ReadInt32()
            Bank(Index).ItemRand(i).Damage = reader.ReadInt32()
            Bank(Index).ItemRand(i).Speed = reader.ReadInt32()

            For x = 1 To Stats.Count - 1
                Bank(Index).ItemRand(i).Stat(x) = reader.ReadInt32()
            Next
        Next
    End Sub

    Sub SaveBank(ByVal Index As Integer)
        Dim filename = Path.Combine(Application.StartupPath, "data", "banks", String.Format("{0}.bin", Player(Index).Login.Trim()))

        Dim writer As New ByteStream(100)

        For i = 1 To MAX_BANK
            writer.WriteByte(Bank(Index).Item(i).Num)
            writer.WriteInt32(Bank(Index).Item(i).Value)

            If Bank(Index).ItemRand(i).Prefix = Nothing Then Bank(Index).ItemRand(i).Prefix = ""
            If Bank(Index).ItemRand(i).Suffix = Nothing Then Bank(Index).ItemRand(i).Suffix = ""

            writer.WriteString(Bank(Index).ItemRand(i).Prefix)
            writer.WriteString(Bank(Index).ItemRand(i).Suffix)
            writer.WriteInt32(Bank(Index).ItemRand(i).Rarity)
            writer.WriteInt32(Bank(Index).ItemRand(i).Damage)
            writer.WriteInt32(Bank(Index).ItemRand(i).Speed)

            For x = 1 To Stats.Count - 1
                writer.WriteInt32(Bank(Index).ItemRand(i).Stat(x))
            Next

            'writer.WriteInt32(Bank(Index).ItemRand(i).Stat(Stats.Strength))
            'writer.WriteInt32(Bank(Index).ItemRand(i).Stat(Stats.Endurance))
            'writer.WriteInt32(Bank(Index).ItemRand(i).Stat(Stats.Vitality))
            'writer.WriteInt32(Bank(Index).ItemRand(i).Stat(Stats.Luck))
            'writer.WriteInt32(Bank(Index).ItemRand(i).Stat(Stats.Intelligence))
            'writer.WriteInt32(Bank(Index).ItemRand(i).Stat(Stats.Spirit))
        Next

        BinaryFile.Save(filename, writer)
    End Sub

    Sub ClearBank(ByVal Index As Integer)
        ReDim Bank(Index).Item(MAX_BANK)
        ReDim Bank(Index).ItemRand(MAX_BANK)

        For i = 1 To MAX_BANK

            Bank(Index).Item(i).Num = 0
            Bank(Index).Item(i).Value = 0
            Bank(Index).ItemRand(i).Prefix = ""
            Bank(Index).ItemRand(i).Suffix = ""
            Bank(Index).ItemRand(i).Rarity = 0
            Bank(Index).ItemRand(i).Damage = 0
            Bank(Index).ItemRand(i).Speed = 0

            ReDim Bank(Index).ItemRand(i).Stat(Stats.Count - 1)
            For x = 1 To Stats.Count - 1
                Bank(Index).ItemRand(i).Stat(x) = 0
            Next
        Next
    End Sub

#End Region

#Region "Characters"
    Sub ClearCharacter(ByVal Index As Integer, ByVal CharNum As Integer)
        Player(Index).Character(CharNum).Classes = 0
        Player(Index).Character(CharNum).Dir = 0

        For i = 0 To EquipmentType.Count - 1
            Player(Index).Character(CharNum).Equipment(i) = 0
        Next

        For i = 0 To MAX_INV
            Player(Index).Character(CharNum).Inv(i).Num = 0
            Player(Index).Character(CharNum).Inv(i).Value = 0
        Next

        Player(Index).Character(CharNum).Exp = 0
        Player(Index).Character(CharNum).Level = 0
        Player(Index).Character(CharNum).Map = 0
        Player(Index).Character(CharNum).Name = ""
        Player(Index).Character(CharNum).Pk = 0
        Player(Index).Character(CharNum).Points = 0
        Player(Index).Character(CharNum).Sex = 0

        For i = 0 To MAX_PLAYER_SKILLS
            Player(Index).Character(CharNum).Skill(i) = 0
        Next

        Player(Index).Character(CharNum).Sprite = 0

        For i = 0 To Stats.Count - 1
            Player(Index).Character(CharNum).Stat(i) = 0
        Next

        For i = 0 To Vitals.Count - 1
            Player(Index).Character(CharNum).Vital(i) = 0
        Next

        Player(Index).Character(CharNum).X = 0
        Player(Index).Character(CharNum).Y = 0

        ReDim Player(Index).Character(CharNum).PlayerQuest(MAX_QUESTS)
        For i = 1 To MAX_QUESTS
            Player(Index).Character(CharNum).PlayerQuest(i).Status = 0
            Player(Index).Character(CharNum).PlayerQuest(i).ActualTask = 0
            Player(Index).Character(CharNum).PlayerQuest(i).CurrentCount = 0
        Next

        'Housing
        Player(Index).Character(CharNum).House.HouseIndex = 0
        Player(Index).Character(CharNum).House.FurnitureCount = 0
        ReDim Player(Index).Character(CharNum).House.Furniture(Player(Index).Character(CharNum).House.FurnitureCount)

        For i = 0 To Player(Index).Character(CharNum).House.FurnitureCount
            Player(Index).Character(CharNum).House.Furniture(i).ItemNum = 0
            Player(Index).Character(CharNum).House.Furniture(i).X = 0
            Player(Index).Character(CharNum).House.Furniture(i).Y = 0
        Next

        Player(Index).Character(CharNum).InHouse = 0
        Player(Index).Character(CharNum).LastMap = 0
        Player(Index).Character(CharNum).LastX = 0
        Player(Index).Character(CharNum).LastY = 0

        ReDim Player(Index).Character(CharNum).Hotbar(MAX_HOTBAR)
        For i = 1 To MAX_HOTBAR
            Player(Index).Character(CharNum).Hotbar(i).Slot = 0
            Player(Index).Character(CharNum).Hotbar(i).SlotType = 0
        Next

        ReDim Player(Index).Character(CharNum).Switches(MAX_SWITCHES)
        For i = 1 To MAX_SWITCHES
            Player(Index).Character(CharNum).Switches(i) = 0
        Next
        ReDim Player(Index).Character(CharNum).Variables(MAX_VARIABLES)
        For i = 1 To MAX_VARIABLES
            Player(Index).Character(CharNum).Variables(i) = 0
        Next

        ReDim Player(Index).Character(CharNum).GatherSkills(ResourceSkills.Count - 1)
        For i = 0 To ResourceSkills.Count - 1
            Player(Index).Character(CharNum).GatherSkills(i).SkillLevel = 1
            Player(Index).Character(CharNum).GatherSkills(i).SkillCurExp = 0
            Player(Index).Character(CharNum).GatherSkills(i).SkillNextLvlExp = 100
        Next

        ReDim Player(Index).Character(CharNum).RecipeLearned(MAX_RECIPE)
        For i = 1 To MAX_RECIPE
            Player(Index).Character(CharNum).RecipeLearned(i) = 0
        Next

        'random items
        ReDim Player(Index).Character(CharNum).RandInv(MAX_INV)
        For i = 1 To MAX_INV
            Player(Index).Character(CharNum).RandInv(i).Prefix = ""
            Player(Index).Character(CharNum).RandInv(i).Suffix = ""
            Player(Index).Character(CharNum).RandInv(i).Rarity = 0
            Player(Index).Character(CharNum).RandInv(i).Damage = 0
            Player(Index).Character(CharNum).RandInv(i).Speed = 0

            ReDim Player(Index).Character(CharNum).RandInv(i).Stat(Stats.Count - 1)
            For x = 1 To Stats.Count - 1
                Player(Index).Character(CharNum).RandInv(i).Stat(x) = 0
            Next
        Next

        ReDim Player(Index).Character(CharNum).RandEquip(EquipmentType.Count - 1)
        For i = 1 To EquipmentType.Count - 1
            Player(Index).Character(CharNum).RandEquip(i).Prefix = ""
            Player(Index).Character(CharNum).RandEquip(i).Suffix = ""
            Player(Index).Character(CharNum).RandEquip(i).Rarity = 0
            Player(Index).Character(CharNum).RandEquip(i).Damage = 0
            Player(Index).Character(CharNum).RandEquip(i).Speed = 0

            ReDim Player(Index).Character(CharNum).RandEquip(i).Stat(Stats.Count - 1)
            For x = 1 To Stats.Count - 1
                Player(Index).Character(CharNum).RandEquip(i).Stat(x) = 0
            Next
        Next

        'pets
        Player(Index).Character(CharNum).Pet.Num = 0
        Player(Index).Character(CharNum).Pet.Health = 0
        Player(Index).Character(CharNum).Pet.Mana = 0
        Player(Index).Character(CharNum).Pet.Level = 0

        ReDim Player(Index).Character(CharNum).Pet.Stat(Stats.Count - 1)
        For i = 1 To Stats.Count - 1
            Player(Index).Character(CharNum).Pet.Stat(i) = 0
        Next

        ReDim Player(Index).Character(CharNum).Pet.Skill(4)
        For i = 1 To 4
            Player(Index).Character(CharNum).Pet.Skill(i) = 0
        Next

        Player(Index).Character(CharNum).Pet.X = 0
        Player(Index).Character(CharNum).Pet.Y = 0
        Player(Index).Character(CharNum).Pet.Dir = 0
        Player(Index).Character(CharNum).Pet.Alive = 0
        Player(Index).Character(CharNum).Pet.AttackBehaviour = 0
        Player(Index).Character(CharNum).Pet.AdoptiveStats = 0
        Player(Index).Character(CharNum).Pet.Points = 0
        Player(Index).Character(CharNum).Pet.Exp = 0

    End Sub

    Sub LoadCharacter(ByVal Index As Integer, ByVal CharNum As Integer)
        Dim filename As String

        ClearCharacter(Index, CharNum)

        filename = Path.Combine(Application.StartupPath, "data", "accounts", Trim$(Player(Index).Login), String.Format("{0}.bin", CharNum))

        Dim reader As New ByteStream()
        BinaryFile.Load(filename, reader)

        Player(Index).Character(CharNum).Classes = reader.ReadByte()
        Player(Index).Character(CharNum).Dir = reader.ReadByte()

        For i = 1 To EquipmentType.Count - 1
            Player(Index).Character(CharNum).Equipment(i) = reader.ReadByte()
        Next

        Player(Index).Character(CharNum).Exp = reader.ReadInt32()

        For i = 0 To MAX_INV
            Player(Index).Character(CharNum).Inv(i).Num = reader.ReadByte()
            Player(Index).Character(CharNum).Inv(i).Value = reader.ReadInt32()
        Next

        Player(Index).Character(CharNum).Level = reader.ReadByte()
        Player(Index).Character(CharNum).Map = reader.ReadInt32()
        Player(Index).Character(CharNum).Name = reader.ReadString()
        Player(Index).Character(CharNum).Pk = reader.ReadByte()
        Player(Index).Character(CharNum).Points = reader.ReadByte()
        Player(Index).Character(CharNum).Sex = reader.ReadByte()

        For i = 0 To MAX_PLAYER_SKILLS
            Player(Index).Character(CharNum).Skill(i) = reader.ReadByte()
        Next

        Player(Index).Character(CharNum).Sprite = reader.ReadInt32()

        For i = 0 To Stats.Count - 1
            Player(Index).Character(CharNum).Stat(i) = reader.ReadByte()
        Next

        For i = 0 To Vitals.Count - 1
            Player(Index).Character(CharNum).Vital(i) = reader.ReadInt32()
        Next

        Player(Index).Character(CharNum).X = reader.ReadByte()
        Player(Index).Character(CharNum).Y = reader.ReadByte()

        For i = 1 To MAX_QUESTS
            Player(Index).Character(CharNum).PlayerQuest(i).Status = reader.ReadInt32()
            Player(Index).Character(CharNum).PlayerQuest(i).ActualTask = reader.ReadInt32()
            Player(Index).Character(CharNum).PlayerQuest(i).CurrentCount = reader.ReadInt32()
        Next

        'Housing
        Player(Index).Character(CharNum).House.HouseIndex = reader.ReadInt32()
        Player(Index).Character(CharNum).House.FurnitureCount = reader.ReadInt32()
        ReDim Player(Index).Character(CharNum).House.Furniture(Player(Index).Character(CharNum).House.FurnitureCount)
        For i = 0 To Player(Index).Character(CharNum).House.FurnitureCount
            Player(Index).Character(CharNum).House.Furniture(i).ItemNum = reader.ReadInt32()
            Player(Index).Character(CharNum).House.Furniture(i).X = reader.ReadInt32()
            Player(Index).Character(CharNum).House.Furniture(i).Y = reader.ReadInt32()
        Next
        Player(Index).Character(CharNum).InHouse = reader.ReadInt32()
        Player(Index).Character(CharNum).LastMap = reader.ReadInt32()
        Player(Index).Character(CharNum).LastX = reader.ReadInt32()
        Player(Index).Character(CharNum).LastY = reader.ReadInt32()

        For i = 1 To MAX_HOTBAR
            Player(Index).Character(CharNum).Hotbar(i).Slot = reader.ReadInt32()
            Player(Index).Character(CharNum).Hotbar(i).SlotType = reader.ReadByte()
        Next

        ReDim Player(Index).Character(CharNum).Switches(MAX_SWITCHES)
        For i = 1 To MAX_SWITCHES
            Player(Index).Character(CharNum).Switches(i) = reader.ReadByte()
        Next
        ReDim Player(Index).Character(CharNum).Variables(MAX_VARIABLES)
        For i = 1 To MAX_VARIABLES
            Player(Index).Character(CharNum).Variables(i) = reader.ReadInt32()
        Next

        ReDim Player(Index).Character(CharNum).GatherSkills(ResourceSkills.Count - 1)
        For i = 0 To ResourceSkills.Count - 1
            Player(Index).Character(CharNum).GatherSkills(i).SkillLevel = reader.ReadInt32()
            Player(Index).Character(CharNum).GatherSkills(i).SkillCurExp = reader.ReadInt32()
            Player(Index).Character(CharNum).GatherSkills(i).SkillNextLvlExp = reader.ReadInt32()
            If Player(Index).Character(CharNum).GatherSkills(i).SkillLevel = 0 Then Player(Index).Character(CharNum).GatherSkills(i).SkillLevel = 1
            If Player(Index).Character(CharNum).GatherSkills(i).SkillNextLvlExp = 0 Then Player(Index).Character(CharNum).GatherSkills(i).SkillNextLvlExp = 100
        Next

        ReDim Player(Index).Character(CharNum).RecipeLearned(MAX_RECIPE)
        For i = 1 To MAX_RECIPE
            Player(Index).Character(CharNum).RecipeLearned(i) = reader.ReadByte()
        Next

        'random items
        ReDim Player(Index).Character(CharNum).RandInv(MAX_INV)
        For i = 1 To MAX_INV
            Player(Index).Character(CharNum).RandInv(i).Prefix = reader.ReadString()
            Player(Index).Character(CharNum).RandInv(i).Suffix = reader.ReadString()
            Player(Index).Character(CharNum).RandInv(i).Rarity = reader.ReadInt32()
            Player(Index).Character(CharNum).RandInv(i).Damage = reader.ReadInt32()
            Player(Index).Character(CharNum).RandInv(i).Speed = reader.ReadInt32()

            ReDim Player(Index).Character(CharNum).RandInv(i).Stat(Stats.Count - 1)
            For x = 1 To Stats.Count - 1
                Player(Index).Character(CharNum).RandInv(i).Stat(x) = reader.ReadInt32()
            Next
        Next

        ReDim Player(Index).Character(CharNum).RandEquip(EquipmentType.Count - 1)
        For i = 1 To EquipmentType.Count - 1
            Player(Index).Character(CharNum).RandEquip(i).Prefix = reader.ReadString()
            Player(Index).Character(CharNum).RandEquip(i).Suffix = reader.ReadString()
            Player(Index).Character(CharNum).RandEquip(i).Rarity = reader.ReadInt32()
            Player(Index).Character(CharNum).RandEquip(i).Damage = reader.ReadInt32()
            Player(Index).Character(CharNum).RandEquip(i).Speed = reader.ReadInt32()

            ReDim Player(Index).Character(CharNum).RandEquip(i).Stat(Stats.Count - 1)
            For x = 1 To Stats.Count - 1
                Player(Index).Character(CharNum).RandEquip(i).Stat(x) = reader.ReadInt32()
            Next
        Next

        'pets
        Player(Index).Character(CharNum).Pet.Num = reader.ReadInt32()
        Player(Index).Character(CharNum).Pet.Health = reader.ReadInt32()
        Player(Index).Character(CharNum).Pet.Mana = reader.ReadInt32()
        Player(Index).Character(CharNum).Pet.Level = reader.ReadInt32()

        ReDim Player(Index).Character(CharNum).Pet.Stat(Stats.Count - 1)
        For i = 1 To Stats.Count - 1
            Player(Index).Character(CharNum).Pet.Stat(i) = reader.ReadByte()
        Next

        ReDim Player(Index).Character(CharNum).Pet.Skill(4)
        For i = 1 To 4
            Player(Index).Character(CharNum).Pet.Skill(i) = reader.ReadInt32()
        Next

        Player(Index).Character(CharNum).Pet.X = reader.ReadInt32()
        Player(Index).Character(CharNum).Pet.Y = reader.ReadInt32()
        Player(Index).Character(CharNum).Pet.Dir = reader.ReadInt32()
        Player(Index).Character(CharNum).Pet.Alive = reader.ReadByte()
        Player(Index).Character(CharNum).Pet.AttackBehaviour = reader.ReadInt32()
        Player(Index).Character(CharNum).Pet.AdoptiveStats = reader.ReadByte()
        Player(Index).Character(CharNum).Pet.Points = reader.ReadInt32()
        Player(Index).Character(CharNum).Pet.Exp = reader.ReadInt32()

    End Sub

    Sub SaveCharacter(ByVal Index As Integer, ByVal CharNum As Integer)
        Dim filename As String

        filename = Path.Combine(Application.StartupPath, "data", "accounts", Trim$(Player(Index).Login), String.Format("{0}.bin", CharNum))

        Dim writer As New ByteStream(100)

        writer.WriteByte(Player(Index).Character(CharNum).Classes)
        writer.WriteByte(Player(Index).Character(CharNum).Dir)

        For i = 1 To EquipmentType.Count - 1
            writer.WriteByte(Player(Index).Character(CharNum).Equipment(i))
        Next

        writer.WriteInt32(Player(Index).Character(CharNum).Exp)

        For i = 0 To MAX_INV
            writer.WriteByte(Player(Index).Character(CharNum).Inv(i).Num)
            writer.WriteInt32(Player(Index).Character(CharNum).Inv(i).Value)
        Next

        writer.WriteByte(Player(Index).Character(CharNum).Level)
        writer.WriteInt32(Player(Index).Character(CharNum).Map)
        writer.WriteString(Player(Index).Character(CharNum).Name)
        writer.WriteByte(Player(Index).Character(CharNum).Pk)
        writer.WriteByte(Player(Index).Character(CharNum).Points)
        writer.WriteByte(Player(Index).Character(CharNum).Sex)

        For i = 0 To MAX_PLAYER_SKILLS
            writer.WriteByte(Player(Index).Character(CharNum).Skill(i))
        Next

        writer.WriteInt32(Player(Index).Character(CharNum).Sprite)

        For i = 0 To Stats.Count - 1
            writer.WriteByte(Player(Index).Character(CharNum).Stat(i))
        Next

        For i = 0 To Vitals.Count - 1
            writer.WriteInt32(Player(Index).Character(CharNum).Vital(i))
        Next

        writer.WriteByte(Player(Index).Character(CharNum).X)
        writer.WriteByte(Player(Index).Character(CharNum).Y)

        For i = 1 To MAX_QUESTS
            writer.WriteInt32(Player(Index).Character(CharNum).PlayerQuest(i).Status)
            writer.WriteInt32(Player(Index).Character(CharNum).PlayerQuest(i).ActualTask)
            writer.WriteInt32(Player(Index).Character(CharNum).PlayerQuest(i).CurrentCount)
        Next

        'Housing
        writer.WriteInt32(Player(Index).Character(CharNum).House.HouseIndex)
        writer.WriteInt32(Player(Index).Character(CharNum).House.FurnitureCount)
        For i = 0 To Player(Index).Character(CharNum).House.FurnitureCount
            writer.WriteInt32(Player(Index).Character(CharNum).House.Furniture(i).ItemNum)
            writer.WriteInt32(Player(Index).Character(CharNum).House.Furniture(i).X)
            writer.WriteInt32(Player(Index).Character(CharNum).House.Furniture(i).Y)
        Next
        writer.WriteInt32(Player(Index).Character(CharNum).InHouse)
        writer.WriteInt32(Player(Index).Character(CharNum).LastMap)
        writer.WriteInt32(Player(Index).Character(CharNum).LastX)
        writer.WriteInt32(Player(Index).Character(CharNum).LastY)

        For i = 1 To MAX_HOTBAR
            writer.WriteInt32(Player(Index).Character(CharNum).Hotbar(i).Slot)
            writer.WriteByte(Player(Index).Character(CharNum).Hotbar(i).SlotType)
        Next

        For i = 1 To MAX_SWITCHES
            writer.WriteByte(Player(Index).Character(CharNum).Switches(i))
        Next

        For i = 1 To MAX_VARIABLES
            writer.WriteInt32(Player(Index).Character(CharNum).Variables(i))
        Next

        For i = 0 To ResourceSkills.Count - 1
            writer.WriteInt32(Player(Index).Character(CharNum).GatherSkills(i).SkillLevel)
            writer.WriteInt32(Player(Index).Character(CharNum).GatherSkills(i).SkillCurExp)
            writer.WriteInt32(Player(Index).Character(CharNum).GatherSkills(i).SkillNextLvlExp)
        Next

        For i = 1 To MAX_RECIPE
            writer.WriteByte(Player(Index).Character(CharNum).RecipeLearned(i))
        Next

        'random items
        For i = 1 To MAX_INV
            writer.WriteString(Player(Index).Character(CharNum).RandInv(i).Prefix)
            writer.WriteString(Player(Index).Character(CharNum).RandInv(i).Suffix)
            writer.WriteInt32(Player(Index).Character(CharNum).RandInv(i).Rarity)
            writer.WriteInt32(Player(Index).Character(CharNum).RandInv(i).Damage)
            writer.WriteInt32(Player(Index).Character(CharNum).RandInv(i).Speed)
            For x = 1 To Stats.Count - 1
                writer.WriteInt32(Player(Index).Character(CharNum).RandInv(i).Stat(x))
            Next
        Next

        For i = 1 To EquipmentType.Count - 1
            writer.WriteString(Player(Index).Character(CharNum).RandEquip(i).Prefix)
            writer.WriteString(Player(Index).Character(CharNum).RandEquip(i).Suffix)
            writer.WriteInt32(Player(Index).Character(CharNum).RandEquip(i).Rarity)
            writer.WriteInt32(Player(Index).Character(CharNum).RandEquip(i).Damage)
            writer.WriteInt32(Player(Index).Character(CharNum).RandEquip(i).Speed)
            For x = 1 To Stats.Count - 1
                writer.WriteInt32(Player(Index).Character(CharNum).RandEquip(i).Stat(x))
            Next
        Next

        'pets
        writer.WriteInt32(Player(Index).Character(CharNum).Pet.Num)
        writer.WriteInt32(Player(Index).Character(CharNum).Pet.Health)
        writer.WriteInt32(Player(Index).Character(CharNum).Pet.Mana)
        writer.WriteInt32(Player(Index).Character(CharNum).Pet.Level)

        For i = 1 To Stats.Count - 1
            writer.WriteByte(Player(Index).Character(CharNum).Pet.Stat(i))
        Next

        For i = 1 To 4
            writer.WriteInt32(Player(Index).Character(CharNum).Pet.Skill(i))
        Next

        writer.WriteInt32(Player(Index).Character(CharNum).Pet.X)
        writer.WriteInt32(Player(Index).Character(CharNum).Pet.Y)
        writer.WriteInt32(Player(Index).Character(CharNum).Pet.Dir)
        writer.WriteByte(Player(Index).Character(CharNum).Pet.Alive)
        writer.WriteInt32(Player(Index).Character(CharNum).Pet.AttackBehaviour)
        writer.WriteByte(Player(Index).Character(CharNum).Pet.AdoptiveStats)
        writer.WriteInt32(Player(Index).Character(CharNum).Pet.Points)
        writer.WriteInt32(Player(Index).Character(CharNum).Pet.Exp)

        BinaryFile.Save(filename, writer)
    End Sub

    Function CharExist(ByVal Index As Integer, ByVal CharNum As Integer) As Boolean

        If Len(Trim$(Player(Index).Character(CharNum).Name)) > 0 Then
            CharExist = True
        Else
            CharExist = False
        End If

    End Function

    Sub AddChar(ByVal Index As Integer, ByVal CharNum As Integer, ByVal Name As String, ByVal Sex As Byte, ByVal ClassNum As Byte, ByVal Sprite As Integer)
        Dim n As Integer, i As Integer
        Dim spritecheck As Boolean

        If Len(Trim$(Player(Index).Character(CharNum).Name)) = 0 Then

            spritecheck = False

            Player(Index).Character(CharNum).Name = Name
            Player(Index).Character(CharNum).Sex = Sex
            Player(Index).Character(CharNum).Classes = ClassNum

            If Player(Index).Character(CharNum).Sex = Enums.Sex.Male Then
                Player(Index).Character(CharNum).Sprite = Classes(ClassNum).MaleSprite(Sprite - 1)
            Else
                Player(Index).Character(CharNum).Sprite = Classes(ClassNum).FemaleSprite(Sprite - 1)
            End If

            Player(Index).Character(CharNum).Level = 1

            For n = 1 To Stats.Count - 1
                Player(Index).Character(CharNum).Stat(n) = Classes(ClassNum).Stat(n)
            Next n

            Player(Index).Character(CharNum).Dir = Direction.Down
            Player(Index).Character(CharNum).Map = Classes(ClassNum).StartMap
            Player(Index).Character(CharNum).X = Classes(ClassNum).StartX
            Player(Index).Character(CharNum).Y = Classes(ClassNum).StartY
            Player(Index).Character(CharNum).Dir = Direction.Down
            Player(Index).Character(CharNum).Vital(Vitals.HP) = GetPlayerMaxVital(Index, Vitals.HP)
            Player(Index).Character(CharNum).Vital(Vitals.MP) = GetPlayerMaxVital(Index, Vitals.MP)
            Player(Index).Character(CharNum).Vital(Vitals.SP) = GetPlayerMaxVital(Index, Vitals.SP)

            ' set starter equipment
            For n = 1 To 5
                If Classes(ClassNum).StartItem(n) > 0 Then
                    Player(Index).Character(CharNum).Inv(n).Num = Classes(ClassNum).StartItem(n)
                    Player(Index).Character(CharNum).Inv(n).Value = Classes(ClassNum).StartValue(n)

                    If Item(Classes(ClassNum).StartItem(n)).Randomize Then
                        Player(Index).Character(TempPlayer(Index).CurChar).RandInv(n).Prefix = ""
                        Player(Index).Character(TempPlayer(Index).CurChar).RandInv(n).Suffix = ""
                        Player(Index).Character(TempPlayer(Index).CurChar).RandInv(n).Rarity = RarityType.RARITY_COMMON
                        Player(Index).Character(TempPlayer(Index).CurChar).RandInv(n).Damage = Item(Classes(ClassNum).StartItem(n)).Data2
                        Player(Index).Character(TempPlayer(Index).CurChar).RandInv(n).Speed = Item(Classes(ClassNum).StartItem(n)).Speed
                        For i = 1 To Stats.Count - 1
                            Player(Index).Character(TempPlayer(Index).CurChar).RandInv(n).Stat(i) = Item(Classes(ClassNum).StartItem(n)).Add_Stat(i)
                        Next
                    End If
                End If
            Next

            'set skills
            ReDim Player(Index).Character(CharNum).GatherSkills(ResourceSkills.Count - 1)
            For i = 0 To ResourceSkills.Count - 1
                Player(Index).Character(CharNum).GatherSkills(i).SkillLevel = 1
                Player(Index).Character(CharNum).GatherSkills(i).SkillCurExp = 0
                Player(Index).Character(CharNum).GatherSkills(i).SkillNextLvlExp = 100
            Next

            ' Append name to file
            AddTextToFile(Name, "accounts\charlist.txt")

            SavePlayer(Index)
            Exit Sub
        End If

    End Sub

    Function FindChar(ByVal Name As String) As Boolean
        FindChar = False
        Dim characters() As String
        Dim fullpath As String
        Dim Contents As String
        Dim bAns As Boolean = False

        fullpath = Path.Combine(Application.StartupPath, "data", "accounts", "charlist.txt")

        Contents = GetFileContents(fullpath)
        characters = Split(Contents, vbNewLine)

        For i = 0 To UBound(characters)
            If Trim$(LCase(characters(i)) = Trim$(LCase(Name))) Then
                FindChar = True
            End If
        Next

        Return FindChar
    End Function

#End Region

#Region "Options"
    Public Sub SaveOptions()
        Dim myXml As New XmlClass With {
            .Filename = Path.Combine(Application.StartupPath, "Data", "Config.xml"),
            .Root = "Options"
        }

        'Check if xml filename is here.
        If Not File.Exists(myXml.Filename) Then
            'Create new blank xml file.
            myXml.NewXmlDocument()
        End If

        myXml.LoadXml()
        myXml.WriteString("Settings", "Game_Name", Options.GameName)
        myXml.WriteString("Settings", "Port", Str(Options.Port))
        myXml.WriteString("Settings", "MoTd", Options.Motd)

        myXml.WriteString("Settings", "Website", Trim$(Options.Website))

        myXml.WriteString("Settings", "StartMap", Options.StartMap)
        myXml.WriteString("Settings", "StartX", Options.StartX)
        myXml.WriteString("Settings", "StartY", Options.StartY)
        myXml.CloseXml(True)
    End Sub

    Public Sub LoadOptions()
        Dim myXml As New XmlClass With {
            .Filename = Path.Combine(Application.StartupPath, "Data", "Config.xml"),
            .Root = "Options"
        }
        myXml.LoadXml()
        Options.GameName = myXml.ReadString("Settings", "Game_Name", "Orion+")
        Options.Port = myXml.ReadString("Settings", "Port", "7001")
        Options.Motd = myXml.ReadString("Settings", "MoTd", "Welcome to the Orion+ Engine")
        Options.Website = myXml.ReadString("Settings", "Website", "http://ascensiongamedev.com/index.php")
        Options.StartMap = myXml.ReadString("Settings", "StartMap", "1")
        Options.StartX = myXml.ReadString("Settings", "StartX", "13+")
        Options.StartY = myXml.ReadString("Settings", "StartY", "7")
        myXml.CloseXml(False)
    End Sub

#End Region

#Region "Logs"

    Public Function GetFileContents(ByVal FullPath As String, Optional ByRef ErrInfo As String = "") As String
        Dim strContents As String
        Dim objReader As StreamReader
        strContents = ""
        If Not File.Exists(FullPath) Then
            Dim fs As FileStream = File.Create(FullPath)
            fs.Close()
            fs.Dispose()
        End If
        Try
            objReader = New StreamReader(FullPath)
            strContents = objReader.ReadToEnd()
            objReader.Close()
        Catch Ex As Exception
            ErrInfo = Ex.Message
        End Try
        Return strContents
    End Function

    Public Function Addlog(ByVal strData As String, ByVal FN As String, Optional ByVal ErrInfo As String = "") As Boolean
        Dim fullpath As String
        Dim Contents As String
        Dim bAns As Boolean = False
        Dim objReader As StreamWriter
        fullpath = Path.Combine(Application.StartupPath, "data", "logs", FN)
        Contents = GetFileContents(fullpath)
        Contents = Contents & vbNewLine & strData
        Try
            objReader = New StreamWriter(fullpath)
            objReader.Write(Contents)
            objReader.Close()
            bAns = True
        Catch Ex As Exception
            ErrInfo = Ex.Message
        End Try
        Return bAns
    End Function

    Public Function AddTextToFile(ByVal strData As String, ByVal FN As String, Optional ByVal ErrInfo As String = "") As Boolean
        Dim fullpath As String
        Dim Contents As String
        Dim bAns As Boolean = False
        Dim objReader As StreamWriter
        fullpath = Path.Combine(Application.StartupPath, "data", FN)
        Contents = GetFileContents(fullpath)
        Contents = Contents & vbNewLine & strData
        Try
            objReader = New StreamWriter(fullpath)
            objReader.Write(Contents)
            objReader.Close()
            bAns = True
        Catch Ex As Exception
            ErrInfo = Ex.Message
        End Try
        Return bAns
    End Function

#End Region

#Region "Banning"
    Sub ServerBanIndex(ByVal BanPlayerIndex As Integer)
        Dim filename As String
        Dim IP As String
        Dim F As Integer
        Dim i As Integer
        filename = Application.StartupPath & "data\banlist.txt"

        ' Make sure the file exists
        If Not FileExist("data\banlist.txt") Then
            F = FreeFile()
            'COME HERE!!!
        End If

        ' Cut off last portion of ip
        IP = Socket.ClientIp(BanPlayerIndex)

        For i = Len(IP) To 1 Step -1

            If Mid$(IP, i, 1) = "." Then
                Exit For
            End If

        Next

        IP = Mid$(IP, 1, i)
        AddTextToFile(IP & "," & "Server", "banlist.txt")
        GlobalMsg(GetPlayerName(BanPlayerIndex) & " has been banned from " & Options.GameName & " by " & "the Server" & "!")
        Addlog("The Server" & " has banned " & GetPlayerName(BanPlayerIndex) & ".", ADMIN_LOG)
        AlertMsg(BanPlayerIndex, "You have been banned by " & "The Server" & "!")
    End Sub

    Sub BanIndex(ByVal BanPlayerIndex As Integer, ByVal BannedByIndex As Integer)
        Dim filename As String
        Dim IP As String
        Dim F As Integer
        Dim i As Integer
        filename = Path.Combine(Application.StartupPath, "data", "banlist.txt")

        ' Make sure the file exists
        If Not FileExist("data\banlist.txt") Then
            F = FreeFile()
        End If

        ' Cut off last portion of ip
        IP = Socket.ClientIp(BanPlayerIndex)

        For i = Len(IP) To 1 Step -1

            If Mid$(IP, i, 1) = "." Then
                Exit For
            End If

        Next

        IP = Mid$(IP, 1, i)
        AddTextToFile(IP & "," & GetPlayerName(BannedByIndex), "banlist.txt")
        GlobalMsg(GetPlayerName(BanPlayerIndex) & " has been banned from " & Options.GameName & " by " & GetPlayerName(BannedByIndex) & "!")
        Addlog(GetPlayerName(BannedByIndex) & " has banned " & GetPlayerName(BanPlayerIndex) & ".", ADMIN_LOG)
        AlertMsg(BanPlayerIndex, "You have been banned by " & GetPlayerName(BannedByIndex) & "!")
    End Sub
#End Region

#Region "Data Functions"
    Function ClassData() As Byte()
        Dim i As Integer, n As Integer, q As Integer
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(Max_Classes)

        For i = 1 To Max_Classes
            Buffer.WriteString(Trim$(GetClassName(i)))
            Buffer.WriteString(Trim(Classes(i).Desc))
            Buffer.WriteInt32(GetClassMaxVital(i, Vitals.HP))
            Buffer.WriteInt32(GetClassMaxVital(i, Vitals.MP))
            Buffer.WriteInt32(GetClassMaxVital(i, Vitals.SP))

            ' set sprite array size
            n = UBound(Classes(i).MaleSprite)

            ' send array size
            Buffer.WriteInt32(n)

            ' loop around sending each sprite
            For q = 0 To n
                Buffer.WriteInt32(Classes(i).MaleSprite(q))
            Next

            ' set sprite array size
            n = UBound(Classes(i).FemaleSprite)

            ' send array size
            Buffer.WriteInt32(n)

            ' loop around sending each sprite
            For q = 0 To n
                Buffer.WriteInt32(Classes(i).FemaleSprite(q))
            Next

            Buffer.WriteInt32(Classes(i).Stat(Stats.Strength))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Endurance))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Vitality))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Intelligence))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Luck))
            Buffer.WriteInt32(Classes(i).Stat(Stats.Spirit))

            For q = 1 To 5
                Buffer.WriteInt32(Classes(i).StartItem(q))
                Buffer.WriteInt32(Classes(i).StartValue(q))
            Next

            Buffer.WriteInt32(Classes(i).StartMap)
            Buffer.WriteInt32(Classes(i).StartX)
            Buffer.WriteInt32(Classes(i).StartY)

            Buffer.WriteInt32(Classes(i).BaseExp)
        Next

        Return Buffer.ToArray()
        Buffer.Dispose()
    End Function

    Function ItemsData() As Byte()
        Dim Buffer As ByteStream
        Dim i As Integer
        Buffer = New ByteStream(4)

        For i = 1 To MAX_ITEMS
            If Len(Trim$(Item(i).Name)) > 0 Then
                Buffer.WriteBlock(ItemData(i))
            End If
        Next

        Return Buffer.ToArray
        Buffer.Dispose()

    End Function

    Function ItemData(ByVal itemNum As Integer) As Byte()
        Dim Buffer As ByteStream
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(itemNum)
        Buffer.WriteInt32(Item(itemNum).AccessReq)

        For i = 0 To Stats.Count - 1
            Buffer.WriteInt32(Item(itemNum).Add_Stat(i))
        Next

        Buffer.WriteInt32(Item(itemNum).Animation)
        Buffer.WriteInt32(Item(itemNum).BindType)
        Buffer.WriteInt32(Item(itemNum).ClassReq)
        Buffer.WriteInt32(Item(itemNum).Data1)
        Buffer.WriteInt32(Item(itemNum).Data2)
        Buffer.WriteInt32(Item(itemNum).Data3)
        Buffer.WriteInt32(Item(itemNum).TwoHanded)
        Buffer.WriteInt32(Item(itemNum).LevelReq)
        Buffer.WriteInt32(Item(itemNum).Mastery)
        Buffer.WriteString(Trim$(Item(itemNum).Name))
        Buffer.WriteInt32(Item(itemNum).Paperdoll)
        Buffer.WriteInt32(Item(itemNum).Pic)
        Buffer.WriteInt32(Item(itemNum).Price)
        Buffer.WriteInt32(Item(itemNum).Rarity)
        Buffer.WriteInt32(Item(itemNum).Speed)

        Buffer.WriteInt32(Item(itemNum).Randomize)
        Buffer.WriteInt32(Item(itemNum).RandomMin)
        Buffer.WriteInt32(Item(itemNum).RandomMax)

        Buffer.WriteInt32(Item(itemNum).Stackable)
        Buffer.WriteString(Trim$(Item(itemNum).Description))

        For i = 0 To Stats.Count - 1
            Buffer.WriteInt32(Item(itemNum).Stat_Req(i))
        Next

        Buffer.WriteInt32(Item(itemNum).Type)
        Buffer.WriteInt32(Item(itemNum).SubType)

        Buffer.WriteInt32(Item(itemNum).ItemLevel)

        'Housing
        Buffer.WriteInt32(Item(itemNum).FurnitureWidth)
        Buffer.WriteInt32(Item(itemNum).FurnitureHeight)

        For i = 1 To 3
            For x = 1 To 3
                Buffer.WriteInt32(Item(itemNum).FurnitureBlocks(i, x))
                Buffer.WriteInt32(Item(itemNum).FurnitureFringe(i, x))
            Next
        Next

        Buffer.WriteInt32(Item(itemNum).KnockBack)
        Buffer.WriteInt32(Item(itemNum).KnockBackTiles)

        Buffer.WriteInt32(Item(itemNum).Projectile)
        Buffer.WriteInt32(Item(itemNum).Ammo)

        Return Buffer.ToArray
        Buffer.Dispose()
    End Function

    Function AnimationsData() As Byte()
        Dim Buffer As ByteStream
        Dim i As Integer
        Buffer = New ByteStream(4)

        For i = 1 To MAX_ANIMATIONS

            If Len(Trim$(Animation(i).Name)) > 0 Then
                Buffer.WriteBlock(AnimationData(i))
            End If

        Next

        Return Buffer.ToArray
        Buffer.Dispose()
    End Function

    Function AnimationData(ByVal AnimationNum As Integer) As Byte()
        Dim Buffer As New ByteStream(4)

        Buffer.WriteInt32(AnimationNum)

        For i = 0 To UBound(Animation(AnimationNum).Frames)
            Buffer.WriteInt32(Animation(AnimationNum).Frames(i))
        Next

        For i = 0 To UBound(Animation(AnimationNum).LoopCount)
            Buffer.WriteInt32(Animation(AnimationNum).LoopCount(i))
        Next

        For i = 0 To UBound(Animation(AnimationNum).LoopTime)
            Buffer.WriteInt32(Animation(AnimationNum).LoopTime(i))
        Next

        Buffer.WriteString(Animation(AnimationNum).Name)
        Buffer.WriteString(Animation(AnimationNum).Sound)

        For i = 0 To UBound(Animation(AnimationNum).Sprite)
            Buffer.WriteInt32(Animation(AnimationNum).Sprite(i))
        Next

        Return Buffer.ToArray
        Buffer.Dispose()
    End Function

    Function NpcsData() As Byte()
        Dim Buffer As ByteStream
        Dim i As Integer
        Buffer = New ByteStream(4)

        For i = 1 To MAX_NPCS

            If Len(Trim$(Npc(i).Name)) > 0 Then
                Buffer.WriteBlock(NpcData(i))
            End If

        Next

        Return Buffer.ToArray
        Buffer.Dispose()
    End Function

    Function NpcData(ByVal NpcNum As Integer) As Byte()
        Dim Buffer As ByteStream, i As Integer
        Buffer = New ByteStream(4)
        Buffer.WriteInt32(NpcNum)
        Buffer.WriteInt32(Npc(NpcNum).Animation)
        Buffer.WriteString(Npc(NpcNum).AttackSay)
        Buffer.WriteInt32(Npc(NpcNum).Behaviour)

        For i = 1 To 5
            Buffer.WriteInt32(Npc(NpcNum).DropChance(i))
            Buffer.WriteInt32(Npc(NpcNum).DropItem(i))
            Buffer.WriteInt32(Npc(NpcNum).DropItemValue(i))
        Next

        Buffer.WriteInt32(Npc(NpcNum).Exp)
        Buffer.WriteInt32(Npc(NpcNum).Faction)
        Buffer.WriteInt32(Npc(NpcNum).Hp)
        Buffer.WriteString(Npc(NpcNum).Name)
        Buffer.WriteInt32(Npc(NpcNum).Range)
        Buffer.WriteInt32(Npc(NpcNum).SpawnTime)
        Buffer.WriteInt32(Npc(NpcNum).SpawnSecs)
        Buffer.WriteInt32(Npc(NpcNum).Sprite)

        For i = 0 To Stats.Count - 1
            Buffer.WriteInt32(Npc(NpcNum).Stat(i))
        Next

        Buffer.WriteInt32(Npc(NpcNum).QuestNum)

        For i = 1 To MAX_NPC_SKILLS
            Buffer.WriteInt32(Npc(NpcNum).Skill(i))
        Next

        Buffer.WriteInt32(Npc(NpcNum).Level)
        Buffer.WriteInt32(Npc(NpcNum).Damage)

        Return Buffer.ToArray
        Buffer.Dispose()
    End Function

    Function ShopsData() As Byte()
        Dim Buffer As ByteStream
        Dim i As Integer

        Buffer = New ByteStream(4)

        For i = 1 To MAX_SHOPS

            If Len(Trim$(Shop(i).Name)) > 0 Then
                Buffer.WriteBlock(ShopData(i))
            End If

        Next

        Return Buffer.ToArray
        Buffer.Dispose()
    End Function

    Function ShopData(ByVal shopNum As Integer) As Byte()
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)

        Buffer.WriteInt32(shopNum)
        Buffer.WriteInt32(Shop(shopNum).BuyRate)
        Buffer.WriteString(Shop(shopNum).Name)
        Buffer.WriteInt32(Shop(shopNum).Face)

        For i = 0 To MAX_TRADES
            Buffer.WriteInt32(Shop(shopNum).TradeItem(i).CostItem)
            Buffer.WriteInt32(Shop(shopNum).TradeItem(i).CostValue)
            Buffer.WriteInt32(Shop(shopNum).TradeItem(i).Item)
            Buffer.WriteInt32(Shop(shopNum).TradeItem(i).ItemValue)
        Next

        Return Buffer.ToArray
        Buffer.Dispose()
    End Function

    Function SkillsData() As Byte()
        Dim i As Integer
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)

        For i = 1 To MAX_SKILLS

            If Len(Trim$(Skill(i).Name)) > 0 Then
                Buffer.WriteBlock(SkillData(i))
            End If

        Next

        Return Buffer.ToArray
        Buffer.Dispose()
    End Function

    Function SkillData(ByVal skillnum As Integer) As Byte()
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)

        Buffer.WriteInt32(skillnum)
        Buffer.WriteInt32(Skill(skillnum).AccessReq)
        Buffer.WriteInt32(Skill(skillnum).AoE)
        Buffer.WriteInt32(Skill(skillnum).CastAnim)
        Buffer.WriteInt32(Skill(skillnum).CastTime)
        Buffer.WriteInt32(Skill(skillnum).CdTime)
        Buffer.WriteInt32(Skill(skillnum).ClassReq)
        Buffer.WriteInt32(Skill(skillnum).Dir)
        Buffer.WriteInt32(Skill(skillnum).Duration)
        Buffer.WriteInt32(Skill(skillnum).Icon)
        Buffer.WriteInt32(Skill(skillnum).Interval)
        Buffer.WriteInt32(Skill(skillnum).IsAoE)
        Buffer.WriteInt32(Skill(skillnum).LevelReq)
        Buffer.WriteInt32(Skill(skillnum).Map)
        Buffer.WriteInt32(Skill(skillnum).MpCost)
        Buffer.WriteString(Skill(skillnum).Name)
        Buffer.WriteInt32(Skill(skillnum).Range)
        Buffer.WriteInt32(Skill(skillnum).SkillAnim)
        Buffer.WriteInt32(Skill(skillnum).StunDuration)
        Buffer.WriteInt32(Skill(skillnum).Type)
        Buffer.WriteInt32(Skill(skillnum).Vital)
        Buffer.WriteInt32(Skill(skillnum).X)
        Buffer.WriteInt32(Skill(skillnum).Y)

        Buffer.WriteInt32(Skill(skillnum).IsProjectile)
        Buffer.WriteInt32(Skill(skillnum).Projectile)

        Buffer.WriteInt32(Skill(skillnum).KnockBack)
        Buffer.WriteInt32(Skill(skillnum).KnockBackTiles)

        Return Buffer.ToArray
        Buffer.Dispose()
    End Function

    Function ResourcesData() As Byte()
        Dim Buffer As ByteStream
        Dim i As Integer

        Buffer = New ByteStream(4)

        For i = 1 To MAX_RESOURCES

            If Len(Trim$(Resource(i).Name)) > 0 Then
                Buffer.WriteBlock(ResourceData(i))
            End If

        Next

        Return Buffer.ToArray
        Buffer.Dispose()
    End Function

    Function ResourceData(ByVal ResourceNum As Integer) As Byte()
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ResourceNum)
        Buffer.WriteInt32(Resource(ResourceNum).Animation)
        Buffer.WriteString(Resource(ResourceNum).EmptyMessage)
        Buffer.WriteInt32(Resource(ResourceNum).ExhaustedImage)
        Buffer.WriteInt32(Resource(ResourceNum).Health)
        Buffer.WriteInt32(Resource(ResourceNum).ExpReward)
        Buffer.WriteInt32(Resource(ResourceNum).ItemReward)
        Buffer.WriteString(Resource(ResourceNum).Name)
        Buffer.WriteInt32(Resource(ResourceNum).ResourceImage)
        Buffer.WriteInt32(Resource(ResourceNum).ResourceType)
        Buffer.WriteInt32(Resource(ResourceNum).RespawnTime)
        Buffer.WriteString(Resource(ResourceNum).SuccessMessage)
        Buffer.WriteInt32(Resource(ResourceNum).LvlRequired)
        Buffer.WriteInt32(Resource(ResourceNum).ToolRequired)
        Buffer.WriteInt32(Resource(ResourceNum).Walkthrough)

        Return Buffer.ToArray
        Buffer.Dispose
    End Function

#End Region

End Module