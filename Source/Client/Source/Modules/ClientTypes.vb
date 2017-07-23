Module ClientTypes
    ' options
    Public Options As New ClientOptions()

    ' Public data structures
    Public Map As MapRec
    Public MapLock As New Object()
    Public Bank As BankRec
    Public MapItem(MAX_MAP_ITEMS) As MapItemRec
    Public MapNpc(MAX_MAP_NPCS) As MapNpcRec
    Public TempTile(,) As TempTileRec


    ' client-side stuff
    Public ActionMsg(Byte.MaxValue) As ActionMsgRec
    Public Blood(Byte.MaxValue) As BloodRec
    Public AnimInstance(Byte.MaxValue) As AnimInstanceRec


    'Mapreport
    Public MapNames(MAX_MAPS) As String

    Public CharSelection() As CharSelRec
    Public Structure CharSelRec
        Dim Name As String
        Dim Sprite As Integer
        Dim Gender As Integer
        Dim ClassName As String
        Dim Level As Integer
    End Structure

    Public Structure SkillAnim
        Dim skillnum As Integer
        Dim Timer As Integer
        Dim FramePointer As Integer
    End Structure

    Public Structure MapRec
        Dim Name As String
        Dim Music As String

        Dim Revision As Integer
        Dim Moral As Byte
        Dim Tileset As Integer

        Dim Up As Integer
        Dim Down As Integer
        Dim Left As Integer
        Dim Right As Integer

        Dim BootMap As Integer
        Dim BootX As Byte
        Dim BootY As Byte

        Dim MaxX As Byte
        Dim MaxY As Byte

        Dim Tile(,) As TileRec
        Dim Npc() As Integer
        Dim EventCount As Integer
        Dim Events() As EventStruct

        Dim WeatherType As Byte
        Dim FogIndex As Integer
        Dim WeatherIntensity As Integer
        Dim FogAlpha As Byte
        Dim FogSpeed As Byte

        Dim HasMapTint As Byte
        Dim MapTintR As Byte
        Dim MapTintG As Byte
        Dim MapTintB As Byte
        Dim MapTintA As Byte

        Dim Instanced As Byte

        Dim Panorama As Byte
        Dim Parallax As Byte

        'Client Side Only -- Temporary
        Dim CurrentEvents As Integer
        Dim MapEvents() As MapEventStruct
    End Structure

    Public Structure ClassRec
        Dim Name As String
        Dim Desc As String
        Dim Stat() As Byte
        Dim MaleSprite() As Integer
        Dim FemaleSprite() As Integer
        Dim StartItem() As Integer
        Dim StartValue() As Integer
        Dim StartMap As Integer
        Dim StartX As Byte
        Dim StartY As Byte
        Dim BaseExp As Integer
        ' For client use
        Dim Vital() As Integer
    End Structure

    Public Structure MapItemRec
        Dim Num As Byte
        Dim Value As Integer
        Dim Frame As Byte
        Dim X As Byte
        Dim Y As Byte

        Dim RandData As RandInvRec
    End Structure

    Public Structure MapNpcRec
        Dim Num As Byte
        Dim Target As Byte
        Dim TargetType As Byte
        Dim Vital() As Integer
        Dim Map As Integer
        Dim X As Byte
        Dim Y As Byte
        Dim Dir As Byte
        ' Client use only
        Dim XOffset As Integer
        Dim YOffset As Integer
        Dim Moving As Byte
        Dim Attacking As Byte
        Dim AttackTimer As Integer
        Dim Steps As Integer
    End Structure

    Public Structure TempTileRec
        Dim DoorOpen As Byte
        Dim DoorFrame As Byte
        Dim DoorTimer As Integer
        Dim DoorAnimate As Byte ' 0 = nothing| 1 = opening | 2 = closing
    End Structure

    Public Structure MapResourceRec
        Dim X As Integer
        Dim Y As Integer
        Dim ResourceState As Byte
    End Structure



    Public Structure BloodRec
        Dim Sprite As Integer
        Dim Timer As Integer
        Dim X As Integer
        Dim Y As Integer
    End Structure

End Module