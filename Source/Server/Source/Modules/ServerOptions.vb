Imports Orion

Friend Class ServerOptions
    Inherits AbstractOptions

    Friend Const KEY_GAMENAME As String = "gamename"
    Friend Const KEY_MOTD As String = "motd"
    Friend Const KEY_PORT As String = "port"
    Friend Const KEY_WEBSITE As String = "website"
    Friend Const KEY_STARTMAP As String = "startmap"
    Friend Const KEY_STARTX As String = "startx"
    Friend Const KEY_STARTY As String = "starty"

    Friend Property GameName As String
        Get
            Return Options(KEY_GAMENAME)
        End Get
        Set(value As String)
            Options(KEY_GAMENAME) = value
        End Set
    End Property

    Friend Property Motd As String
        Get
            Return Options(KEY_MOTD)
        End Get
        Set(value As String)
            Options(KEY_MOTD) = value
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

    Friend Property Website As String
        Get
            Return Options(KEY_WEBSITE)
        End Get
        Set(value As String)
            Options(KEY_WEBSITE) = value
        End Set
    End Property

    Friend Property StartMap As Integer
        Get
            Return Options(KEY_STARTMAP)
        End Get
        Set(value As Integer)
            Options(KEY_STARTMAP) = value
        End Set
    End Property

    Friend Property StartX As Integer
        Get
            Return Options(KEY_STARTX)
        End Get
        Set(value As Integer)
            Options(KEY_STARTX) = value
        End Set
    End Property

    Friend Property StartY As Integer
        Get
            Return Options(KEY_STARTY)
        End Get
        Set(value As Integer)
            Options(KEY_STARTY) = value
        End Set
    End Property

End Class
