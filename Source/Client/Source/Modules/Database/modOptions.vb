Imports Orion

Friend Class modOptions
    Inherits AbstractOptions

    Friend Const KEY_SAVEPASS = "savepass"
    Friend Const KEY_USERNAME = "username"
    Friend Const KEY_PASSWORD = "password"
    Friend Const KEY_IP = "ip"
    Friend Const KEY_PORT = "port"
    Friend Const KEY_MENUMUSIC = "menumusic"
    Friend Const KEY_MUSIC = "music"
    Friend Const KEY_SOUND = "sound"
    Friend Const KEY_VOLUME = "volume"
    Friend Const KEY_SCREENSIZE = "screensize"
    Friend Const KEY_HIGHEND = "highend"
    Friend Const KEY_SHOWNPCBAR = "shownpcbar"

    Friend Property SavePass As Boolean
        Get
            Return Options(KEY_SAVEPASS)
        End Get
        Set(value As Boolean)
            Options(KEY_SAVEPASS) = value
        End Set
    End Property

    Friend Property Password As String
        Get
            Return Options(KEY_PASSWORD)
        End Get
        Set(value As String)
            Options(KEY_PASSWORD) = value
        End Set
    End Property

    Friend Property Username As String
        Get
            Return Options(KEY_USERNAME)
        End Get
        Set(value As String)
            Options(KEY_USERNAME) = value
        End Set
    End Property

    Friend Property IP As String
        Get
            Return Options(KEY_IP)
        End Get
        Set(value As String)
            Options(KEY_IP) = value
        End Set
    End Property

    Friend Property Port As Integer
        Get
            Return Options(KEY_PORT)
        End Get
        Set(value As Integer)
            Options(KEY_PORT) = value
        End Set
    End Property

    Friend Property MenuMusic As String
        Get
            Return Options(KEY_MENUMUSIC)
        End Get
        Set(value As String)
            Options(KEY_MENUMUSIC) = value
        End Set
    End Property

    Friend Property Music As Byte
        Get
            Return Options(KEY_MUSIC)
        End Get
        Set(value As Byte)
            Options(KEY_MUSIC) = value
        End Set
    End Property

    Friend Property Sound As Byte
        Get
            Return Options(KEY_SOUND)
        End Get
        Set(value As Byte)
            Options(KEY_SOUND) = value
        End Set
    End Property

    Friend Property Volume As Single
        Get
            Return Options(KEY_VOLUME)
        End Get
        Set(value As Single)
            Options(KEY_VOLUME) = value
        End Set
    End Property

    Friend Property ScreenSize As Byte
        Get
            Return Options(KEY_SCREENSIZE)
        End Get
        Set(value As Byte)
            Options(KEY_SCREENSIZE) = value
        End Set
    End Property

    Friend Property HighEnd As Byte
        Get
            Return Options(KEY_HIGHEND)
        End Get
        Set(value As Byte)
            Options(KEY_HIGHEND) = value
        End Set
    End Property

    Friend Property ShowNpcBar As Byte
        Get
            Return Options(KEY_SHOWNPCBAR)
        End Get
        Set(value As Byte)
            Options(KEY_SHOWNPCBAR) = value
        End Set
    End Property
End Class
