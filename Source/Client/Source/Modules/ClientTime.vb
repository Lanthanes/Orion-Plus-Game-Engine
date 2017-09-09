Imports System.Runtime.InteropServices.Marshal
Imports ASFW
Imports Orion

Public Module ClientTime
    Sub Packet_Clock(ByRef Data() As Byte)
        Dim Buffer As New ByteStream(Data)
        Time.Instance.GameSpeed = Buffer.ReadInt32()
        Time.Instance.Time = New Date(BitConverter.ToInt64(Buffer.ReadBytes(), 0))

        Buffer.Dispose()
    End Sub

    Sub Packet_Time(ByRef Data() As Byte)
        Dim Buffer As New ByteStream(Data)

        Time.Instance.TimeOfDay = Buffer.ReadByte

        Select Case Time.Instance.TimeOfDay
            Case TimeOfDay.Dawn
                AddText("A chilling, refreshing, breeze has come with the morning.", ColorType.BrightBlue)
                Exit Select

            Case TimeOfDay.Day
                AddText("Day has dawned in this region.", ColorType.Yellow)
                Exit Select

            Case TimeOfDay.Dusk
                AddText("Dusk has begun darkening the skies...", ColorType.BrightRed)
                Exit Select

            Case Else
                AddText("Night has fallen upon the weary travelers.", ColorType.DarkGray)
                Exit Select
        End Select

        Buffer.Dispose()
    End Sub
End Module
