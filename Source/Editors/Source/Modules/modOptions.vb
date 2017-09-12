Imports Orion

Friend Class modOptions
    Inherits AbstractOptions

    Friend Const KEY_SAVEPASS As String = "savepass"
    Friend Const KEY_PASSWORD As String = "password"
    Friend Const KEY_USERNAME As String = "username"
    Friend Const KEY_IP As String = "ip"
    Friend Const KEY_PORT As String = "port"
    Friend Const KEY_MENUMUSIC As String = "menumusic"
    Friend Const KEY_MUSIC As String = "music"
    Friend Const KEY_SOUND As String = "sound"
    Friend Const KEY_VOLUME As String = "volume"
    Friend Const KEY_SCREENSIZE As String = "screensize"

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

End Class
