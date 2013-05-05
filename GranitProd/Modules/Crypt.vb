Imports System
Imports System.Security.Cryptography
Imports System.Text

Module Crypt

    ''' <summary>
    ''' Convertit une chaîne en MD5
    ''' </summary>
    ''' <param name="input">Chaîne à convertir</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getMd5Hash(ByVal input As String) As String
        ' Create a new instance of the MD5 object.
        Dim md5Hasher As MD5 = MD5.Create()

        ' Convert the input string to a byte array and compute the hash.
        Dim data As Byte() = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input))

        ' Create a new Stringbuilder to collect the bytes
        ' and create a string.
        Dim sBuilder As New StringBuilder()

        ' Loop through each byte of the hashed data 
        ' and format each one as a hexadecimal string.
        Dim i As Integer
        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i

        ' Return the hexadecimal string.
        Return sBuilder.ToString()

    End Function

    ''' <summary>
    ''' Compare une chaîne MD5 à une chaîne simple
    ''' </summary>
    ''' <param name="input">Chaîne simple</param>
    ''' <param name="hash">Chaîne MD5</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function verifyMd5Hash(ByVal input As String, ByVal hash As String) As Boolean
        ' Hash the input.
        Dim hashOfInput As String = getMd5Hash(input)

        ' Create a StringComparer an compare the hashes.
        Dim comparer As StringComparer = StringComparer.OrdinalIgnoreCase

        If 0 = comparer.Compare(hashOfInput, hash) Then
            Return True
        Else
            Return False
        End If

    End Function

End Module