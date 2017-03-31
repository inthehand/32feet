'
' Copyright (c) 2009 Alan J. McFarlane
' Free for any use.  No rights reserved.
'

Option Explicit On
Option Strict On
Imports System
Imports System.IO
Imports System.Threading


''' <summary>
''' Raises events when data is recevied on a <see cref="T:System.IO.Stream" />
''' </summary>
''' <remarks>
''' <para>Raises three events <see cref="E:DataReceived" />, <see cref="E:ConnectionClosed" />,
''' and <see cref="E:ErrorOccurred" />.
''' </para>
''' <para>The events are called on the correct synchronization context, e.g. on
''' the UI thread in a WinForms app.
''' </para>
''' </remarks>
''' -
''' <example>
''' <code>
'''     ...
'''     Dim strm As Stream = cli.GetStream() ' etc...
'''     Dim efs As New EventsFromStream() ' We pick-up the UI thread when initialised
'''     AddHandler efs.DataReceived, AddressOf HandleDataReceived
'''     AddHandler efs.ErrorOccurred, AddressOf HandleErrorOccurred
'''     AddHandler efs.ConnectionClosed, AddressOf HandleConnectionClosed
'''     efs.Run(strm)
'''     ...
''' End Sub
''' 
''' Sub HandleDataReceived(ByVal sender As Object, ByVal e as DataReceivedEventArgs)
'''     Dim data() As Byte = e.Data
'''     ...
''' End Sub
''' ...
''' </code>
''' </example>
Class EventsFromStream : Inherits System.ComponentModel.Component
    Private m_strm As Stream
    Private m_syncCtx As SynchronizationContext
    Private m_disposed As Integer
    Private m_disposedEvent As New ManualResetEvent(False)

    '----
    Friend Sub New()
    End Sub

    Overloads Sub Dispose(ByVal disposing As Boolean)
        Try
            Thread.VolatileWrite(m_disposed, 1)
            m_disposedEvent.Set()
            ' Should we close the Stream?  It makes life much easier for us if
            ' we do but
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private ReadOnly Property IsDisposed() As Boolean
        Get
            Dim v As Integer = Thread.VolatileRead(m_disposed)
            Dim disposed As Boolean = v <> 0
            Return disposed
        End Get
    End Property

    '----
    Public Event DataReceived As EventHandler(Of DataReceivedEventArgs)

    Public Event ConnectionClosed As EventHandler(Of EventArgs)

    Public Event ErrorOccurred As EventHandler(Of ErrorEventArgs)

    '----
    Public Sub Run(ByVal strm As Stream)
        If m_strm IsNot Nothing Then Throw New ArgumentException("Supports one stream at the moment")
        m_strm = strm
        Debug.Assert(m_syncCtx Is Nothing)
        m_syncCtx = SynchronizationContext.Current
        ThreadPool.QueueUserWorkItem(AddressOf Runner)
    End Sub

    Private Sub Runner(ByVal state As Object)
        Dim buf(1024 - 1) As Byte
        While True
            Dim readLen As Integer
            Try
#If False Then
                readLen = m_strm.Read(buf, 0, buf.Length)
#Else
                ' Use the async version so we can exit when disposed
                Dim ar As IAsyncResult = m_strm.BeginRead(buf, 0, buf.Length, Nothing, Nothing)
                Dim ah() As WaitHandle = {ar.AsyncWaitHandle, m_disposedEvent}
                Dim signalled As Integer = WaitHandle.WaitAny(ah, -1)
                If signalled = 0 Then
                    readLen = m_strm.EndRead(ar)
                Else
                    ' We leave that BeginReceive hanging in this case. :-(  There's
                    ' not much we can do about that (well we could start a thread
                    ' sitting on EndRead...).
                    ' It would be much nicer if we closed the stream when we were
                    ' disposed...
                    Exit While
                End If
#End If
            Catch ex As Exception
                ' Probably due to the stream closing, so ignore.
                If IsDisposed Then Exit While
                DoError(ex)
                Exit While
            End Try
            If readLen = 0 Then
                DoConnectionClosed()
                Exit While
            End If
            DoDataReceived(Clone(buf, 0, readLen))
        End While
    End Sub

    '--
    Private Sub DoError(ByVal ex As Exception)
        m_syncCtx.Send(AddressOf DoErrorS, ex)
    End Sub

    Private Sub DoErrorS(ByVal state As Object)
        Dim ex As Exception = CType(state, Exception)
        RaiseEvent ErrorOccurred(Me, New ErrorEventArgs(ex))
    End Sub

    '--
    Private Sub DoConnectionClosed()
        m_syncCtx.Send(AddressOf DoConnectionClosedS, Nothing)
    End Sub

    Private Sub DoConnectionClosedS(ByVal state As Object)
        RaiseEvent ConnectionClosed(Me, New EventArgs)
    End Sub

    '--
    Private Sub DoDataReceived(ByVal data() As Byte)
        m_syncCtx.Send(AddressOf DoDataReceivedS, data)
    End Sub

    Private Sub DoDataReceivedS(ByVal state As Object)
        Dim data() As Byte = CType(state, Byte())
        RaiseEvent DataReceived(Me, New DataReceivedEventArgs(data))
    End Sub

    '----
    Private Function Clone(ByVal buf As Byte(), ByVal offset As Integer, ByVal length As Integer) As Byte()
        ' Create a new array of the correct size
        Dim buf2() As Byte = CType(Array.CreateInstance(GetType(Byte), length), Byte())
        Array.Copy(buf, offset, buf2, 0, length)
        Return buf2
    End Function
End Class



Class DataReceivedEventArgs : Inherits EventArgs
    Private m_data() As Byte

    Friend Sub New(ByVal data() As Byte)
        m_data = data
    End Sub

    Public ReadOnly Property Data() As Byte()
        Get
            Return m_data
        End Get
    End Property
End Class
