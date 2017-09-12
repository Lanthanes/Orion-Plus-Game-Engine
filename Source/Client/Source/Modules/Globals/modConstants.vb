Imports System.Drawing

Module modConstants

    'Chatbubble
    Friend Const ChatBubbleWidth As Integer = 100

    Friend Const EFFECT_TYPE_FADEIN As Integer = 1
    Friend Const EFFECT_TYPE_FADEOUT As Integer = 2
    Friend Const EFFECT_TYPE_FLASH As Integer = 3
    Friend Const EFFECT_TYPE_FOG As Integer = 4
    Friend Const EFFECT_TYPE_WEATHER As Integer = 5
    Friend Const EFFECT_TYPE_TINT As Integer = 6

    ' path constants
    Friend Const SOUND_PATH As String = "\Data\sound\"
    Friend Const MUSIC_PATH As String = "\Data\music\"

    ' Font variables
    Friend Const FONT_NAME As String = "Arial.ttf"
    Friend Const FONT_SIZE As Byte = 13

    ' Log Path and variables
    Friend Const LOG_DEBUG As String = "debug.txt"
    Friend Const LOG_PATH As String = "\Data\logs\"

    ' Gfx Path and variables
    Friend Const GFX_PATH As String = "\Data\graphics\"
    Friend Const GFX_GUI_PATH As String = "\Data\graphics\gui\"
    Friend Const GFX_EXT As String = ".png"

    ' Menu states
    Friend Const MENU_STATE_NEWACCOUNT As Byte = 0
    Friend Const MENU_STATE_DELACCOUNT As Byte = 1
    Friend Const MENU_STATE_LOGIN As Byte = 2
    Friend Const MENU_STATE_GETCHARS As Byte = 3
    Friend Const MENU_STATE_NEWCHAR As Byte = 4
    Friend Const MENU_STATE_ADDCHAR As Byte = 5
    Friend Const MENU_STATE_DELCHAR As Byte = 6
    Friend Const MENU_STATE_USECHAR As Byte = 7
    Friend Const MENU_STATE_INIT As Byte = 8

    ' Number of tiles in width in tilesets
    Friend Const TILESHEET_WIDTH As Integer = 15 ' * PIC_X pixels

    Friend MapGrid As Boolean

    ' Speed moving vars
    Friend Const WALK_SPEED As Byte = 6
    Friend Const RUN_SPEED As Byte = 10

    ' Tile size constants
    Friend Const PIC_X As Integer = 32
    Friend Const PIC_Y As Integer = 32

    ' Sprite, item, skill size constants
    Friend Const SIZE_X As Integer = 32
    Friend Const SIZE_Y As Integer = 32

    ' ********************************************************
    ' * The values below must match with the server's values *
    ' ********************************************************

    ' General constants
    Friend GAME_NAME As String = "Orion+"

    ' Website
    Friend Const GAME_WEBSITE As String = "http://ascensiongamedev.com/index.php"

    ' Map constants
    Friend SCREEN_MAPX As Byte = 35
    Friend SCREEN_MAPY As Byte = 26

    Friend ITEM_RARITY_COLOR_0 = SFML.Graphics.Color.White ' white
    Friend ITEM_RARITY_COLOR_1 = New SFML.Graphics.Color(102, 255, 0) ' green
    Friend ITEM_RARITY_COLOR_2 = New SFML.Graphics.Color(73, 151, 208) ' blue
    Friend ITEM_RARITY_COLOR_3 = New SFML.Graphics.Color(255, 0, 0) ' red
    Friend ITEM_RARITY_COLOR_4 = New SFML.Graphics.Color(159, 0, 197) ' purple
    Friend ITEM_RARITY_COLOR_5 = New SFML.Graphics.Color(255, 215, 0) ' gold

    Friend HalfX As Integer = ((SCREEN_MAPX + 1) \ 2) * PIC_X
    Friend HalfY As Integer = ((SCREEN_MAPY + 1) \ 2) * PIC_Y
    Friend ScreenX As Integer = (SCREEN_MAPX + 1) * PIC_X
    Friend ScreenY As Integer = (SCREEN_MAPY + 1) * PIC_Y

    'dialog types
    Friend Const DIALOGUE_TYPE_BUYHOME As Byte = 1
    Friend Const DIALOGUE_TYPE_VISIT As Byte = 2
    Friend Const DIALOGUE_TYPE_PARTY As Byte = 3
    Friend Const DIALOGUE_TYPE_QUEST As Byte = 4
    Friend Const DIALOGUE_TYPE_TRADE As Byte = 5

End Module
