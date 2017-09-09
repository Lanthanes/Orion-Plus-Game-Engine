Imports ASFW

Public Module EditorQuest
#Region "Global Info"
    'Constants
    Public Const MAX_QUESTS As Byte = 250
    'Public Const MAX_REQUIREMENTS As Byte = 10
    'Public Const MAX_TASKS As Byte = 10
    Public Const EDITOR_TASKS As Byte = 7

    Public Const QUEST_TYPE_GOSLAY As Byte = 1
    Public Const QUEST_TYPE_GOGATHER As Byte = 2
    Public Const QUEST_TYPE_GOTALK As Byte = 3
    Public Const QUEST_TYPE_GOREACH As Byte = 4
    Public Const QUEST_TYPE_GOGIVE As Byte = 5
    Public Const QUEST_TYPE_GOKILL As Byte = 6
    Public Const QUEST_TYPE_GOTRAIN As Byte = 7
    Public Const QUEST_TYPE_GOGET As Byte = 8

    Public Const QUEST_NOT_STARTED As Byte = 0
    Public Const QUEST_STARTED As Byte = 1
    Public Const QUEST_COMPLETED As Byte = 2
    Public Const QUEST_COMPLETED_BUT As Byte = 3

    Public QuestLogPage As Integer
    Public QuestNames(MAX_ACTIVEQUESTS) As String

    Public Quest_Changed(MAX_QUESTS) As Boolean

    Public QuestEditorShow As Boolean

    'questlog variables

    Public Const MAX_ACTIVEQUESTS = 10

    Public QuestLogX As Integer = 150
    Public QuestLogY As Integer = 100

    Public pnlQuestLogVisible As Boolean
    Public SelectedQuest As String
    Public QuestTaskLogText As String = ""
    Public ActualTaskText As String = ""
    Public QuestDialogText As String = ""
    Public QuestStatus2Text As String = ""
    Public AbandonQuestText As String = ""
    Public AbandonQuestVisible As Boolean
    Public QuestRequirementsText As String = ""
    Public QuestRewardsText As String = ""

    'here we store temp info because off UpdateUI >.<
    Public UpdateQuestWindow As Boolean
    Public UpdateQuestChat As Boolean
    Public QuestNum As Integer
    Public QuestNumForStart As Integer
    Public QuestMessage As String
    Public QuestAcceptTag As Integer

    'Types
    Public Quest(0 To MAX_QUESTS) As QuestRec

    Public Structure PlayerQuestRec
        Dim Status As Integer '0=not started, 1=started, 2=completed, 3=completed but repeatable
        Dim ActualTask As Integer
        Dim CurrentCount As Integer 'Used to handle the Amount property
    End Structure

    Public Structure TaskRec
        Dim Order As Integer
        Dim Npc As Integer
        Dim Item As Integer
        Dim Map As Integer
        Dim Resource As Integer
        Dim Amount As Integer
        Dim Speech As String
        Dim TaskLog As String
        Dim QuestEnd As Byte
        Dim TaskType As Integer
    End Structure

    Public Structure QuestRec
        Dim Name As String
        Dim QuestLog As String
        Dim Repeat As Byte
        Dim Cancelable As Byte

        Dim ReqCount As Integer
        Dim Requirement() As Integer '1=item, 2=quest, 3=class
        Dim RequirementIndex() As Integer

        Dim QuestGiveItem As Integer 'Todo: make this dynamic
        Dim QuestGiveItemValue As Integer
        Dim QuestRemoveItem As Integer
        Dim QuestRemoveItemValue As Integer

        Dim Chat() As String

        Dim RewardCount As Integer
        Dim RewardItem() As Integer
        Dim RewardItemAmount() As Integer
        Dim RewardExp As Integer

        Dim TaskCount As Integer
        Dim Task() As TaskRec

    End Structure
#End Region

#Region "Quest Editor"
    Public Sub QuestEditorInit()

        If frmEditor_Quest.Visible = False Then Exit Sub
        EditorIndex = frmEditor_Quest.lstIndex.SelectedIndex + 1

        With frmEditor_Quest
            .txtName.Text = Trim$(Quest(EditorIndex).Name)

            If Quest(EditorIndex).Repeat = 1 Then
                .chkRepeat.Checked = True
            Else
                .chkRepeat.Checked = False
            End If

            .txtStartText.Text = Trim$(Quest(EditorIndex).Chat(1))
            .txtProgressText.Text = Trim$(Quest(EditorIndex).Chat(2))
            .txtEndText.Text = Trim$(Quest(EditorIndex).Chat(3))

            .cmbStartItem.Items.Clear()
            .cmbItemReq.Items.Clear()
            .cmbEndItem.Items.Clear()
            .cmbItemReward.Items.Clear()
            .cmbStartItem.Items.Add("None")
            .cmbItemReq.Items.Add("None")
            .cmbEndItem.Items.Add("None")
            .cmbItemReward.Items.Add("None")

            For i = 1 To MAX_ITEMS
                .cmbStartItem.Items.Add(i & ": " & Item(i).Name)
                .cmbItemReq.Items.Add(i & ": " & Item(i).Name)
                .cmbEndItem.Items.Add(i & ": " & Item(i).Name)
                .cmbItemReward.Items.Add(i & ": " & Item(i).Name)
            Next

            .cmbClassReq.Items.Clear()
            .cmbClassReq.Items.Add("None")
            For i = 1 To Max_Classes
                .cmbClassReq.Items.Add(Trim(Classes(i).Name))
            Next

            .cmbStartItem.SelectedIndex = Quest(EditorIndex).QuestGiveItem
            .cmbEndItem.SelectedIndex = Quest(EditorIndex).QuestRemoveItem

            .nudGiveAmount.Value = Quest(EditorIndex).QuestGiveItemValue

            .nudTakeAmount.Value = Quest(EditorIndex).QuestRemoveItemValue

            .lstRewards.Items.Clear()
            For i = 1 To Quest(EditorIndex).RewardCount
                .lstRewards.Items.Add(i & ":" & Quest(EditorIndex).RewardItemAmount(i) & " X " & Trim(Item(Quest(EditorIndex).RewardItem(i)).Name))
            Next

            .nudExpReward.Value = Quest(EditorIndex).RewardExp


            .lstRequirements.Items.Clear()

            For i = 1 To Quest(EditorIndex).ReqCount

                Select Case Quest(EditorIndex).Requirement(i)
                    Case 1
                        .lstRequirements.Items.Add(i & ":" & "Item Requirement: " & Trim(Item(Quest(EditorIndex).RequirementIndex(i)).Name))
                    Case 2
                        .lstRequirements.Items.Add(i & ":" & "Quest Requirement: " & Trim(Quest(Quest(EditorIndex).RequirementIndex(i)).Name))
                    Case 3
                        .lstRequirements.Items.Add(i & ":" & "Class Requirement: " & Trim(Classes(Quest(EditorIndex).RequirementIndex(i)).Name))
                    Case Else
                        .lstRequirements.Items.Add(i & ":")
                End Select
            Next

            .lstTasks.Items.Clear()
            For i = 1 To Quest(EditorIndex).TaskCount
                frmEditor_Quest.lstTasks.Items.Add(i & ":" & Quest(EditorIndex).Task(i).TaskLog)
            Next

            .rdbNoneReq.Checked = True
        End With

        Quest_Changed(EditorIndex) = True

    End Sub

    Public Sub QuestEditorOk()
        Dim I As Integer

        For I = 1 To MAX_QUESTS
            If Quest_Changed(I) Then
                SendSaveQuest(I)
            End If
        Next

        frmEditor_Quest.Dispose()
        Editor = 0
        ClearChanged_Quest()

    End Sub

    Public Sub QuestEditorCancel()
        Editor = 0
        frmEditor_Quest.Dispose()
        ClearChanged_Quest()
        ClearQuests()
        SendRequestQuests()
    End Sub

    Public Sub ClearChanged_Quest()
        Dim I As Integer

        For I = 0 To MAX_QUESTS
            Quest_Changed(I) = False
        Next
    End Sub
#End Region

#Region "DataBase"
    Sub ClearQuest(ByVal QuestNum As Integer)
        Dim I As Integer

        ' clear the Quest
        Quest(QuestNum).Name = ""
        Quest(QuestNum).QuestLog = ""
        Quest(QuestNum).Repeat = 0
        Quest(QuestNum).Cancelable = 0

        Quest(QuestNum).ReqCount = 0
        ReDim Quest(QuestNum).Requirement(Quest(QuestNum).ReqCount)
        ReDim Quest(QuestNum).RequirementIndex(Quest(QuestNum).ReqCount)
        For I = 1 To Quest(QuestNum).ReqCount
            Quest(QuestNum).Requirement(I) = 0
            Quest(QuestNum).RequirementIndex(I) = 0
        Next

        Quest(QuestNum).QuestGiveItem = 0
        Quest(QuestNum).QuestGiveItemValue = 0
        Quest(QuestNum).QuestRemoveItem = 0
        Quest(QuestNum).QuestRemoveItemValue = 0

        ReDim Quest(QuestNum).Chat(3)
        For I = 1 To 3
            Quest(QuestNum).Chat(I) = ""
        Next

        Quest(QuestNum).RewardCount = 0
        ReDim Quest(QuestNum).RewardItem(Quest(QuestNum).RewardCount)
        ReDim Quest(QuestNum).RewardItemAmount(Quest(QuestNum).RewardCount)
        For I = 1 To Quest(QuestNum).RewardCount
            Quest(QuestNum).RewardItem(I) = 0
            Quest(QuestNum).RewardItemAmount(I) = 0
        Next
        Quest(QuestNum).RewardExp = 0

        Quest(QuestNum).TaskCount = 0
        ReDim Quest(QuestNum).Task(Quest(QuestNum).TaskCount)
        For I = 1 To Quest(QuestNum).TaskCount
            Quest(QuestNum).Task(I).Order = 0
            Quest(QuestNum).Task(I).Npc = 0
            Quest(QuestNum).Task(I).Item = 0
            Quest(QuestNum).Task(I).Map = 0
            Quest(QuestNum).Task(I).Resource = 0
            Quest(QuestNum).Task(I).Amount = 0
            Quest(QuestNum).Task(I).Speech = ""
            Quest(QuestNum).Task(I).TaskLog = ""
            Quest(QuestNum).Task(I).QuestEnd = 0
            Quest(QuestNum).Task(I).TaskType = 0
        Next

    End Sub

    Sub ClearQuests()
        Dim I As Integer

        For I = 1 To MAX_QUESTS
            ClearQuest(I)
        Next
    End Sub
#End Region

#Region "Incoming Packets"
    Public Sub Packet_QuestEditor(ByVal data() As Byte)
        QuestEditorShow = True
    End Sub

    Public Sub Packet_UpdateQuest(ByVal data() As Byte)
        Dim QuestNum As Integer
        Dim Buffer As New ByteStream(data)
        QuestNum = Buffer.ReadInt32

        ' Update the Quest
        Quest(QuestNum).Name = Buffer.ReadString
        Quest(QuestNum).QuestLog = Buffer.ReadString
        Quest(QuestNum).Repeat = Buffer.ReadInt32
        Quest(QuestNum).Cancelable = Buffer.ReadInt32

        Quest(QuestNum).ReqCount = Buffer.ReadInt32
        ReDim Quest(QuestNum).Requirement(Quest(QuestNum).ReqCount)
        ReDim Quest(QuestNum).RequirementIndex(Quest(QuestNum).ReqCount)
        For I = 1 To Quest(QuestNum).ReqCount
            Quest(QuestNum).Requirement(I) = Buffer.ReadInt32
            Quest(QuestNum).RequirementIndex(I) = Buffer.ReadInt32
        Next

        Quest(QuestNum).QuestGiveItem = Buffer.ReadInt32
        Quest(QuestNum).QuestGiveItemValue = Buffer.ReadInt32
        Quest(QuestNum).QuestRemoveItem = Buffer.ReadInt32
        Quest(QuestNum).QuestRemoveItemValue = Buffer.ReadInt32

        For I = 1 To 3
            Quest(QuestNum).Chat(I) = Buffer.ReadString
        Next

        Quest(QuestNum).RewardCount = Buffer.ReadInt32
        ReDim Quest(QuestNum).RewardItem(Quest(QuestNum).RewardCount)
        ReDim Quest(QuestNum).RewardItemAmount(Quest(QuestNum).RewardCount)
        For i = 1 To Quest(QuestNum).RewardCount
            Quest(QuestNum).RewardItem(i) = Buffer.ReadInt32
            Quest(QuestNum).RewardItemAmount(i) = Buffer.ReadInt32
        Next

        Quest(QuestNum).RewardExp = Buffer.ReadInt32

        Quest(QuestNum).TaskCount = Buffer.ReadInt32
        ReDim Quest(QuestNum).Task(Quest(QuestNum).TaskCount)
        For I = 1 To Quest(QuestNum).TaskCount
            Quest(QuestNum).Task(I).Order = Buffer.ReadInt32
            Quest(QuestNum).Task(I).Npc = Buffer.ReadInt32
            Quest(QuestNum).Task(I).Item = Buffer.ReadInt32
            Quest(QuestNum).Task(I).Map = Buffer.ReadInt32
            Quest(QuestNum).Task(I).Resource = Buffer.ReadInt32
            Quest(QuestNum).Task(I).Amount = Buffer.ReadInt32
            Quest(QuestNum).Task(I).Speech = Buffer.ReadString
            Quest(QuestNum).Task(I).TaskLog = Buffer.ReadString
            Quest(QuestNum).Task(I).QuestEnd = Buffer.ReadInt32
            Quest(QuestNum).Task(I).TaskType = Buffer.ReadInt32
        Next

        Buffer.Dispose()
    End Sub

#End Region

#Region "Outgoing Packets"
    Public Sub SendRequestEditQuest()
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(EditorPackets.RequestEditQuest)
        SendData(Buffer.ToArray)
        Buffer.Dispose()

    End Sub

    Public Sub SendSaveQuest(ByVal QuestNum As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)

        Buffer.WriteInt32(EditorPackets.SaveQuest)
        Buffer.WriteInt32(QuestNum)

        Buffer.WriteString(Trim(Quest(QuestNum).Name))
        Buffer.WriteString(Trim(Quest(QuestNum).QuestLog))
        Buffer.WriteInt32(Quest(QuestNum).Repeat)
        Buffer.WriteInt32(Quest(QuestNum).Cancelable)

        Buffer.WriteInt32(Quest(QuestNum).ReqCount)
        For I = 1 To Quest(QuestNum).ReqCount
            Buffer.WriteInt32(Quest(QuestNum).Requirement(I))
            Buffer.WriteInt32(Quest(QuestNum).RequirementIndex(I))
        Next

        Buffer.WriteInt32(Quest(QuestNum).QuestGiveItem)
        Buffer.WriteInt32(Quest(QuestNum).QuestGiveItemValue)
        Buffer.WriteInt32(Quest(QuestNum).QuestRemoveItem)
        Buffer.WriteInt32(Quest(QuestNum).QuestRemoveItemValue)

        For I = 1 To 3
            Buffer.WriteString(Trim(Quest(QuestNum).Chat(I)))
        Next

        Buffer.WriteInt32(Quest(QuestNum).RewardCount)
        For i = 1 To Quest(QuestNum).RewardCount
            Buffer.WriteInt32(Quest(QuestNum).RewardItem(i))
            Buffer.WriteInt32(Quest(QuestNum).RewardItemAmount(i))
        Next

        Buffer.WriteInt32(Quest(QuestNum).RewardExp)

        Buffer.WriteInt32(Quest(QuestNum).TaskCount)
        For I = 1 To Quest(QuestNum).TaskCount
            Buffer.WriteInt32(Quest(QuestNum).Task(I).Order)
            Buffer.WriteInt32(Quest(QuestNum).Task(I).Npc)
            Buffer.WriteInt32(Quest(QuestNum).Task(I).Item)
            Buffer.WriteInt32(Quest(QuestNum).Task(I).Map)
            Buffer.WriteInt32(Quest(QuestNum).Task(I).Resource)
            Buffer.WriteInt32(Quest(QuestNum).Task(I).Amount)
            Buffer.WriteString(Trim(Quest(QuestNum).Task(I).Speech))
            Buffer.WriteString(Trim(Quest(QuestNum).Task(I).TaskLog))
            Buffer.WriteInt32(Quest(QuestNum).Task(I).QuestEnd)
            Buffer.WriteInt32(Quest(QuestNum).Task(I).TaskType)
        Next

        SendData(Buffer.ToArray)
        Buffer.Dispose()

    End Sub

    Sub SendRequestQuests()
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ClientPackets.CRequestQuests)
        SendData(Buffer.ToArray)
        Buffer.Dispose()

    End Sub

    Public Sub UpdateQuestLog()
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ClientPackets.CQuestLogUpdate)
        SendData(Buffer.ToArray)
        Buffer.Dispose()

    End Sub

    Public Sub PlayerHandleQuest(ByVal QuestNum As Integer, ByVal Order As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)

        Buffer.WriteInt32(ClientPackets.CPlayerHandleQuest)
        Buffer.WriteInt32(QuestNum)
        Buffer.WriteInt32(Order) '1=accept quest, 2=cancel quest
        SendData(Buffer.ToArray)
        Buffer.Dispose()
    End Sub

    Public Sub QuestReset(ByVal QuestNum As Integer)
        Dim Buffer As ByteStream

        Buffer = New ByteStream(4)
        Buffer.WriteInt32(ClientPackets.CQuestReset)
        Buffer.WriteInt32(QuestNum)
        SendData(Buffer.ToArray)
        Buffer.Dispose()

    End Sub
#End Region

#Region "Support Functions"

    Public Function GetQuestNum(ByVal QuestName As String) As Integer
        Dim I As Integer
        GetQuestNum = 0

        For I = 1 To MAX_QUESTS
            If Trim$(Quest(I).Name) = Trim$(QuestName) Then
                GetQuestNum = I
                Exit For
            End If
        Next
    End Function

#End Region

#Region "Misc Functions"
    Public Sub LoadRequirement(ByVal QuestNum As Integer, ByVal ReqNum As Integer)
        Dim i As Integer

        With frmEditor_Quest
            'Populate combo boxes
            .cmbItemReq.Items.Clear()
            .cmbItemReq.Items.Add("None")

            For i = 1 To MAX_ITEMS
                .cmbItemReq.Items.Add(i & ": " & Item(i).Name)
            Next

            .cmbQuestReq.Items.Clear()
            .cmbQuestReq.Items.Add("None")

            For i = 1 To MAX_QUESTS
                .cmbQuestReq.Items.Add(i & ": " & Quest(i).Name)
            Next

            .cmbClassReq.Items.Clear()
            .cmbClassReq.Items.Add("None")

            For i = 1 To Max_Classes
                .cmbClassReq.Items.Add(i & ": " & Classes(i).Name)
            Next

            .cmbItemReq.Enabled = False
            .cmbQuestReq.Enabled = False
            .cmbClassReq.Enabled = False

            Select Case Quest(QuestNum).Requirement(ReqNum)
                Case 0
                    .rdbNoneReq.Checked = True
                Case 1
                    .rdbItemReq.Checked = True
                    .cmbItemReq.Enabled = True
                    .cmbItemReq.SelectedIndex = Quest(QuestNum).RequirementIndex(ReqNum)
                Case 2
                    .rdbQuestReq.Checked = True
                    .cmbQuestReq.Enabled = True
                    .cmbQuestReq.SelectedIndex = Quest(QuestNum).RequirementIndex(ReqNum)
                Case 3
                    .rdbClassReq.Checked = True
                    .cmbClassReq.Enabled = True
                    .cmbClassReq.SelectedIndex = Quest(QuestNum).RequirementIndex(ReqNum)
            End Select

        End With

    End Sub

    'Subroutine that load the desired task in the form
    Public Sub LoadTask(ByVal QuestNum As Integer, ByVal TaskNum As Integer)
        Dim TaskToLoad As TaskRec
        TaskToLoad = Quest(QuestNum).Task(TaskNum)

        With frmEditor_Quest
            'Load the task type
            Select Case TaskToLoad.Order
                Case 0
                    .optTask0.Checked = True
                Case 1
                    .optTask1.Checked = True
                Case 2
                    .optTask2.Checked = True
                Case 3
                    .optTask3.Checked = True
                Case 4
                    .optTask4.Checked = True
                Case 5
                    .optTask5.Checked = True
                Case 6
                    .optTask6.Checked = True
                Case 7
                    .optTask7.Checked = True
            End Select

            'Load textboxes
            .txtTaskLog.Text = "" & Trim$(TaskToLoad.TaskLog)

            'Populate combo boxes
            .cmbNpc.Items.Clear()
            .cmbNpc.Items.Add("None")

            For i = 1 To MAX_NPCS
                .cmbNpc.Items.Add(i & ": " & Npc(i).Name)
            Next

            .cmbItem.Items.Clear()
            .cmbItem.Items.Add("None")

            For i = 1 To MAX_ITEMS
                .cmbItem.Items.Add(i & ": " & Item(i).Name)
            Next

            .cmbMap.Items.Clear()
            .cmbMap.Items.Add("None")

            For i = 1 To MAX_MAPS
                .cmbMap.Items.Add(i)
            Next

            .cmbResource.Items.Clear()
            .cmbResource.Items.Add("None")

            For i = 1 To MAX_RESOURCES
                .cmbResource.Items.Add(i & ": " & Resource(i).Name)
            Next

            'Set combo to 0 and disable them so they can be enabled when needed
            .cmbNpc.SelectedIndex = 0
            .cmbItem.SelectedIndex = 0
            .cmbMap.SelectedIndex = 0
            .cmbResource.SelectedIndex = 0
            .nudAmount.Value = 0

            .cmbNpc.Enabled = False
            .cmbItem.Enabled = False
            .cmbMap.Enabled = False
            .cmbResource.Enabled = False
            .nudAmount.Enabled = False

            If TaskToLoad.QuestEnd = 1 Then
                .chkEnd.Checked = True
            Else
                .chkEnd.Checked = False
            End If

            Select Case TaskToLoad.Order
                Case 0 'Nothing

                Case QUEST_TYPE_GOSLAY '1
                    .cmbNpc.Enabled = True
                    .cmbNpc.SelectedIndex = TaskToLoad.Npc
                    .nudAmount.Enabled = True
                    .nudAmount.Value = TaskToLoad.Amount

                Case QUEST_TYPE_GOGATHER '2
                    .cmbItem.Enabled = True
                    .cmbItem.SelectedIndex = TaskToLoad.Item
                    .nudAmount.Enabled = True
                    .nudAmount.Value = TaskToLoad.Amount

                Case QUEST_TYPE_GOTALK '3
                    .cmbNpc.Enabled = True
                    .cmbNpc.SelectedIndex = TaskToLoad.Npc

                Case QUEST_TYPE_GOREACH '4
                    .cmbMap.Enabled = True
                    .cmbMap.SelectedIndex = TaskToLoad.Map

                Case QUEST_TYPE_GOGIVE '5
                    .cmbItem.Enabled = True
                    .cmbItem.SelectedIndex = TaskToLoad.Item
                    .nudAmount.Enabled = True
                    .nudAmount.Value = TaskToLoad.Amount
                    .cmbNpc.Enabled = True
                    .cmbNpc.SelectedIndex = TaskToLoad.Npc
                    .txtTaskSpeech.Text = "" & Trim$(TaskToLoad.Speech)

                Case QUEST_TYPE_GOTRAIN '6
                    .cmbResource.Enabled = True
                    .cmbResource.SelectedIndex = TaskToLoad.Resource
                    .nudAmount.Enabled = True
                    .nudAmount.Value = TaskToLoad.Amount

                Case QUEST_TYPE_GOGET '7
                    .cmbNpc.Enabled = True
                    .cmbNpc.SelectedIndex = TaskToLoad.Npc
                    .cmbItem.Enabled = True
                    .cmbItem.SelectedIndex = TaskToLoad.Item
                    .nudAmount.Enabled = True
                    .nudAmount.Value = TaskToLoad.Amount
                    .txtTaskSpeech.Text = "" & Trim$(TaskToLoad.Speech)
            End Select

            .lblTaskNum.Text = "Task Number: " & TaskNum
        End With
    End Sub
#End Region

End Module
