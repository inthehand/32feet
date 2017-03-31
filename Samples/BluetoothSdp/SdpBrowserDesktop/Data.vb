' 32feet.NET - Personal Area Networking for .NET
'
' InTheHand.Net.Bluetooth.BluetoothRadio
' 
' Copyright (c) 2007 In The Hand Ltd, All rights reserved.
' Copyright (c) 2007 Alan J McFarlane.
' This source code is licensed under the In The Hand Community License - see License.txt

Imports InTheHand.Net.Bluetooth
Imports System.Reflection

Public Class BluetoothServiceItem
#If Not FX1_1 Then
    Implements IEquatable(Of BluetoothServiceItem)
#End If
    '
    Private ReadOnly m_uuid As Guid
    Private ReadOnly m_name As String

    '----------------
    Public Sub New(ByVal uuid As Guid, ByVal name As String)
        m_uuid = uuid
        m_name = name
    End Sub

    '----------------
    Public ReadOnly Property Value() As Guid
        Get
            Return m_uuid
        End Get
    End Property

    Public ReadOnly Property Uuid() As Guid
        Get
            Return m_uuid
        End Get
    End Property

    Public ReadOnly Property Name() As String
        Get
            Return m_name & " " & m_uuid.ToString()
        End Get
    End Property

    '----------------
    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        If Not TypeOf obj Is BluetoothServiceItem Then Return False
        Return Equals(DirectCast(obj, BluetoothServiceItem))
    End Function

#If FX1_1 Then
    Public Overloads Function Equals(ByVal obj As BluetoothServiceItem) As Boolean
    ' No IEquatable(Of T) pre FX2
#Else
    Public Overloads Function Equals(ByVal obj As BluetoothServiceItem) As Boolean _
    Implements IEquatable(Of BluetoothServiceItem).Equals
#End If
        Return Me.Uuid.Equals(obj.Uuid)
    End Function

    '----------------
#If FX1_1 Then
    Public Shared Function GetWellKnownServices() As System.Collections.IList
#Else
    Public Shared Function GetWellKnownServices() As System.Collections.Generic.List(Of BluetoothServiceItem)
#End If
        Dim type As Type = GetType(BluetoothService)
        Dim fields() As FieldInfo = type.GetFields( _
          BindingFlags.Static Or BindingFlags.Public Or BindingFlags.DeclaredOnly)
#If FX1_1 Then
        Dim list As New System.Collections.ArrayList
#Else
        Dim list As New System.Collections.Generic.List(Of BluetoothServiceItem)
#End If
        For Each curField As FieldInfo In fields
            'Dim obj As Object = curField.GetRawConstantValue()
            Dim obj As Object = curField.GetValue(Nothing)
            Dim guid As Guid = CType(obj, Guid)
            If NullGuid(guid) Then
                ' Empty or BluetoothBase etc.
            Else
                list.Add(New BluetoothServiceItem(guid, curField.Name))
            End If
        Next
        Return list
    End Function

    Private Shared Function NullGuid(ByVal uuid As Guid) As Boolean
        Dim asBytes() As Byte = uuid.ToByteArray()
        Dim nullFirstChunk As Boolean _
            = asBytes(3) = 0 AndAlso asBytes(2) = 0 AndAlso asBytes(1) = 0 AndAlso asBytes(0) = 0
        If nullFirstChunk Then
            Dim i As Integer = 0
        Else
            Dim j As Integer = 0
        End If
        Return nullFirstChunk
    End Function

End Class
