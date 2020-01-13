Imports InTheHand.Net.Bluetooth

Public Class FormSelectRadioMode
    Private _mode As RadioMode

    '----
    Private Sub FormSelectRadioMode_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim list As New List(Of RadioMode)
        ' No Enum.GetMembers on NETCF
        list.Add(RadioMode.PowerOff)
        list.Add(RadioMode.Connectable)
        list.Add(RadioMode.Discoverable)
        Me.ComboBox1.DataSource = list
        Me.ComboBox1.SelectedItem = _mode
    End Sub

    Private Sub MenuItemCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub MenuItemSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemSelect.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub FormSelectRadioMode_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Me.DialogResult = Windows.Forms.DialogResult.OK Then
            Dim o As Object = Me.ComboBox1.SelectedValue
            Dim o2 As Object = Me.ComboBox1.SelectedItem
            Try
                If o Is Nothing Then Throw New InvalidCastException
                Dim m As RadioMode = CType(o, RadioMode)
                _mode = m
            Catch cex As InvalidCastException
                ' This won't occur as we're DropDownList mode.
                Form1.MessageBox_Show(Me, "Please select or enter a RadioMode")
                e.Cancel = True
            End Try
        End If
    End Sub

    Function Enum_Parse(Of TEnum)(ByVal str As String) As TEnum
        Dim v0 As Object = [Enum].Parse(GetType(TEnum), str, True)
        Dim v As TEnum = CType(v0, TEnum)
        Return v
    End Function

    '----
    Public Property Mode() As RadioMode
        Get
            Return _mode
        End Get
        Set(ByVal value As RadioMode)
            _mode = value
        End Set
    End Property

End Class