Imports System.Text

Module BufferUtility
    Public Function ReadUnicodeString(ByVal data As Byte()) As String
        If data Is Nothing OrElse data.Length = 0 Then Return "Null"
        Return Conv_String(Encoding.ASCII.GetString(data, 0, data.Length))
    End Function

    Public Function WriteUnicodeString(ByVal Input As String)
        If Input = vbNullString Then Return New Byte()
        Return Encoding.ASCII.GetBytes(Conv_Uni(Input))
    End Function

    Public Function Conv_String(ByVal message As String) As String
        Conv_String = ""

        Try
            Dim split As String() = message.Split(New [Char]() {" "c, ","c, "."c, ";"c, CChar(vbTab)})
            For Each s As String In split
                If s.Trim() <> "" Then
                    Conv_String = Conv_String & ChrW(s)
                End If
            Next s
        Catch ex As Exception

        End Try

        Return Conv_String

    End Function

    'Convert a Unicode String to Unicode
    Function Conv_Uni(ByVal inx As String) As String
        Dim i As Integer
        Conv_Uni = ""

        If inx = vbNullString OrElse inx = "" Then
            Conv_Uni = "I miss this."
            Return Conv_Uni
            Exit Function
        End If

        For i = 0 To inx.Length - 1
            Conv_Uni += AscW(inx.Chars(i)) & ";"
        Next

    End Function
End Module