#If False = False Then

'/// <summary>
'/// The Clipboard class is based on code from 
'/// http://www.opennetcf.org/Forums/topic.asp?TOPIC_ID=125
'/// </summary>

Imports System.Runtime.InteropServices

Public Class Clipboard

#Region "Win32 API ------------------------------------------------------------"
    Private Const CF_UNICODETEXT As Integer = 13

    <DllImport("Coredll.dll")> _
    Private Shared Function OpenClipboard(ByVal hWndNewOwner As IntPtr) As Boolean
    End Function

    <DllImport("Coredll.dll")> _
    Private Shared Function CloseClipboard() As Boolean
    End Function

    <DllImport("Coredll.dll")> _
    Private Shared Function EmptyClipboard() As Boolean
    End Function

    <DllImport("Coredll.dll")> _
    Private Shared Function IsClipboardFormatAvailable(ByVal uFormat As Int32) As Boolean
    End Function

    <DllImport("Coredll.dll")> _
    Private Shared Function GetClipboardData(ByVal uFormat As Int32) As IntPtr
    End Function

    <DllImport("Coredll.dll")> _
    Private Shared Function GetClipboardData_String(ByVal uFormat As Int32) As String
    End Function

    <DllImport("Coredll.dll")> _
    Private Shared Function SetClipboardData(ByVal uFormat As Int32, ByVal hMem As IntPtr) As IntPtr
    End Function

    <DllImport("Coredll.dll")> _
    Private Shared Function LocalAlloc(ByVal uFlags As Integer, ByVal uBytes As Integer) As IntPtr
    End Function

#End Region '-------------------------------------------------------------------



#Region "Internal support functions"

    Private Shared hMem As IntPtr = IntPtr.Zero

    '/// <summary>
    '/// Copy data from string to a pointer
    '/// </summary>
    '/// <param name="dest"></param>
    '/// <param name="src"></param>
    Private Shared Sub CopyToPointer(ByVal dest As IntPtr, ByVal src As String)
        Dim buf0 As Byte() = System.Text.Encoding.Unicode.GetBytes(src)
        Dim buf As Byte() = CType(Array.CreateInstance(GetType(Byte), buf0.Length + 2), Byte())
        buf0.CopyTo(buf, 0)
        System.Diagnostics.Debug.Assert(buf(buf.Length - 2) = 0 And buf(buf.Length - 1) = 0, "null-terminated")
        Marshal.Copy(buf, 0, dest, buf.Length)
    End Sub

    ''/// <summary>
    ''/// Convert a pointer to string
    ''/// </summary>
    ''/// <param name="src"></param>
    ''/// <returns></returns>
    'Private Shared Function _ConvertToString(ByVal src As IntPtr) As String
    '    'int x;
    '    'char* pSrc = (char*) src.ToPointer();
    '    'StringBuilder sb = new StringBuilder();
    '    'for (x = 0; pSrc[x] != '\0'; x++)
    '    '{
    '    '      sb.Append(pSrc[x])
    '    '}
    '    'Return sb.ToString()
    'End Function

#End Region


#Region "Public methods"

    '/// <summary>
    ''/// Check if data is available on the clipboard
    ''/// </summary>
    'Public Shared ReadOnly Property IsDataAvailable() As Boolean
    '    Get
    '        Return IsClipboardFormatAvailable(CF_UNICODETEXT)
    '    End Get
    'End Property

    '/// <summary>
    '/// Set data on the clipboard
    '/// </summary>
    '/// <param name="strText"></param>
    '/// <returns></returns>
    Public Shared Function SetData(ByVal strText As String) As Boolean
        If OpenClipboard(IntPtr.Zero) = False Then
            Return False
        End If
        hMem = LocalAlloc(0, (strText.Length + 1) * Marshal.SizeOf(GetType(Char)))
        If (hMem.ToInt32() = 0) Then
            Return False
        End If
        CopyToPointer(hMem, strText)
        EmptyClipboard()
        SetClipboardData(CF_UNICODETEXT, hMem)
        CloseClipboard()
        Return True
    End Function



    '/// <summary>
    ''/// Get data from the clipboard
    ''/// </summary>
    ''/// <returns></returns>
    'Public Shared Function _GetData() As String
    '    Dim hData As IntPtr
    '    Dim strText As String
    '    If (IsDataAvailable = False) Then
    '        Return Nothing
    '    End If
    '    If (OpenClipboard(IntPtr.Zero) = False) Then
    '        Return Nothing
    '    End If
    '    hData = GetClipboardData(CF_UNICODETEXT)
    '    If (hData.ToInt32() = 0) Then
    '        Return Nothing
    '    End If
    '    strText = _ConvertToString(hData)
    '    CloseClipboard()
    '    Return strText
    'End Function

    Public Shared Sub Cut(ByVal txtBox As TextBox)
        '// Copy the data to the clipboard
        SetData(txtBox.SelectedText)
        '// Remove selected text
        txtBox.SelectedText = ""
    End Sub

    Public Shared Sub Copy(ByVal txtBox As TextBox)
        '// Copy the data to the clipboard
        SetData(txtBox.SelectedText)
    End Sub

    'Public Shared Sub _Paste(ByVal txtBox As TextBox)
    '    If (IsDataAvailable) Then
    '        txtBox.SelectedText = _GetData()
    '    End If
    'End Sub

#End Region
End Class

#End If
