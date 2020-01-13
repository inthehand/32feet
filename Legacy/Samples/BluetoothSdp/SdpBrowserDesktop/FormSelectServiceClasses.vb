Imports InTheHand.Net.Bluetooth

Public Class FormSelectServiceClasses


    ' Its a [Flag] attribute, so multiple value can be combined
    Private m_selectedScs As ServiceClass

    '--------
    Public Property SelectedServiceClasses() As ServiceClass
        Set(ByVal value As ServiceClass)
            m_selectedScs = value
        End Set
        Get
            Return m_selectedScs
        End Get
    End Property

    '--------
    Private Sub FormSelectServiceClasses_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles MyBase.Load
        ' Add the members of the ServiceClass enum to the listbox, except the null one.
        Dim classes() As ServiceClass = CType([Enum].GetValues(GetType(ServiceClass)), ServiceClass())
        ' Remove the Unknown/0 value
        classes = Array.FindAll(classes, AddressOf NotMatchZero)
        Me.ListBox1.DataSource = classes
    End Sub

    Function NotMatchZero(ByVal value As ServiceClass) As Boolean
        Return value <> 0
    End Function

    Private Sub FormSelectServiceClasses_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles MyBase.Shown
        ' Pre-select the items selected by the ServiceClass value passed in.
        Dim selectedItems As ListBox.SelectedObjectCollection = Me.ListBox1.SelectedItems
        selectedItems.Clear()
        For i As Integer = 0 To Me.ListBox1.Items.Count - 1
            Dim item As Object = Me.ListBox1.Items(i)
            Dim itemSc As ServiceClass = CType(item, ServiceClass)
            ' Is that flag set, the pre-select the item.
            If (itemSc And m_selectedScs) = itemSc Then selectedItems.Add(itemSc)
        Next
        If m_selectedScs = 0 Then System.Diagnostics.Debug.Assert(Me.ListBox1.SelectedItems.Count = 0, _
            "For Unknown/None/0 no items should be pre-selected")
    End Sub

    Private Sub FormSelectServiceClasses_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) _
    Handles MyBase.FormClosing
        If Me.DialogResult = Windows.Forms.DialogResult.OK Then
            ' Set the Property result to the selected items.
            m_selectedScs = 0 ' Prepare for setting individual Flag values
            Dim sel As ListBox.SelectedObjectCollection = Me.ListBox1.SelectedItems
            For Each cur As Object In sel
                Dim scCur As ServiceClass = CType(cur, ServiceClass)
                m_selectedScs = m_selectedScs Or scCur
            Next
        End If
    End Sub

    Private Sub ButtonOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles ButtonOk.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

End Class