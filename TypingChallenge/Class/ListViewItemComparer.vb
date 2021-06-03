Imports System.ComponentModel
Imports System.Dynamic
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

Module ListViewExtension

    <StructLayout(LayoutKind.Sequential)>
    Public Structure HDITEM
        Public theMask As Mask
        Public cxy As Integer
        <MarshalAs(UnmanagedType.LPTStr)>
        Public pszText As String
        Public hbm As IntPtr
        Public cchTextMax As Integer
        Public fmt As Format
        Public lParam As IntPtr
        ' _WIN32_IE >= 0x0300 
        Public iImage As Integer
        Public iOrder As Integer
        ' _WIN32_IE >= 0x0500
        Public type As UInteger
        Public pvFilter As IntPtr
        ' _WIN32_WINNT >= 0x0600
        Public state As UInteger

        <Flags()>
        Public Enum Mask
            Format = &H4       ' HDI_FORMAT
        End Enum


        <Flags()>
        Public Enum Format
            SortDown = &H200 ' HDF_SORTDOWN
            SortUp = &H400     ' HDF_SORTUP
        End Enum
    End Structure

    Public Const LVM_FIRST As Integer = &H1000
    Public Const LVM_GETHEADER As Integer = LVM_FIRST + 31

    Public Const HDM_FIRST As Integer = &H1200
    Public Const HDM_GETITEM As Integer = HDM_FIRST + 11
    Public Const HDM_SETITEM As Integer = HDM_FIRST + 12

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Public Function SendMessage(hWnd As IntPtr, msg As UInt32, wParam As IntPtr, lParam As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Public Function SendMessage(hWnd As IntPtr, msg As UInt32, wParam As IntPtr, ByRef lParam As HDITEM) As IntPtr
    End Function

    <Extension()>
    Public Sub SetSortIcon(listViewControl As ListViewX, columnIndex As Integer, order As SortOrder)
        listViewControl.SortColumn = columnIndex
        Dim columnHeader As IntPtr = SendMessage(listViewControl.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero)
        For columnNumber As Integer = 0 To listViewControl.Columns.Count - 1

            Dim columnPtr As New IntPtr(columnNumber)
            Dim item As New HDITEM

            item.theMask = HDITEM.Mask.Format

            If SendMessage(columnHeader, HDM_GETITEM, columnPtr, item) = IntPtr.Zero Then Throw New Win32Exception

            If order <> SortOrder.None AndAlso columnNumber = columnIndex Then
                Select Case order
                    Case SortOrder.Ascending
                        item.fmt = item.fmt And Not HDITEM.Format.SortDown
                        item.fmt = item.fmt Or HDITEM.Format.SortUp
                    Case SortOrder.Descending
                        item.fmt = item.fmt And Not HDITEM.Format.SortUp
                        item.fmt = item.fmt Or HDITEM.Format.SortDown
                End Select
            Else
                item.fmt = item.fmt And Not HDITEM.Format.SortDown And Not HDITEM.Format.SortUp
            End If

            If SendMessage(columnHeader, HDM_SETITEM, columnPtr, item) = IntPtr.Zero Then Throw New Win32Exception
        Next
    End Sub

    <Extension()>
    Public Sub SetSortIcon(listViewControl As ListViewX, order As SortOrder)
        Dim columnIndex As Integer = listViewControl.SortColumn
        Dim columnHeader As IntPtr = SendMessage(listViewControl.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero)
        For columnNumber As Integer = 0 To listViewControl.Columns.Count - 1

            Dim columnPtr As New IntPtr(columnNumber)
            Dim item As New HDITEM

            item.theMask = HDITEM.Mask.Format

            If SendMessage(columnHeader, HDM_GETITEM, columnPtr, item) = IntPtr.Zero Then Throw New Win32Exception

            If order <> SortOrder.None AndAlso columnNumber = columnIndex Then
                Select Case order
                    Case SortOrder.Ascending
                        item.fmt = item.fmt And Not HDITEM.Format.SortDown
                        item.fmt = item.fmt Or HDITEM.Format.SortUp
                    Case SortOrder.Descending
                        item.fmt = item.fmt And Not HDITEM.Format.SortUp
                        item.fmt = item.fmt Or HDITEM.Format.SortDown
                End Select
            Else
                item.fmt = item.fmt And Not HDITEM.Format.SortDown And Not HDITEM.Format.SortUp
            End If

            If SendMessage(columnHeader, HDM_SETITEM, columnPtr, item) = IntPtr.Zero Then Throw New Win32Exception
        Next
    End Sub

    <DllImport("uxtheme.dll", ExactSpelling:=True, CharSet:=CharSet.Unicode), Extension>
    Public Function SetWindowTheme(ByVal hwnd As IntPtr, Optional pszSubAppName As String = "Explorer", Optional pszSubIdList As String = Nothing) As Integer
    End Function

    <Extension>
    Public Sub AutoColumnWidth(listview As ListView, columns() As ColumnHeader, Optional style As ColumnHeaderAutoResizeStyle = ColumnHeaderAutoResizeStyle.ColumnContent)
        For Each col As ColumnHeader In columns
            Dim textSize As Integer = TextRenderer.MeasureText(col.Text, listview.Font).Width
            textSize = CInt(textSize * 1.2)
            listview.AutoResizeColumn(col.Index, ColumnHeaderAutoResizeStyle.ColumnContent)
            If col.Width < textSize Then col.Width = textSize
        Next
    End Sub

    <Extension>
    Public Sub AutoColumnWidth(listview As ListView, Optional style As ColumnHeaderAutoResizeStyle = ColumnHeaderAutoResizeStyle.ColumnContent)
        For Each col As ColumnHeader In listview.Columns
            Dim textSize As Integer = TextRenderer.MeasureText(col.Text, listview.Font).Width
            textSize = CInt(textSize * 1.2)
            listview.AutoResizeColumn(col.Index, ColumnHeaderAutoResizeStyle.ColumnContent)
            If col.Width < textSize Then col.Width = textSize
        Next
    End Sub

    <Extension>
    Public Sub ResizeColumnHeaders(listview As ListView)
        For i As Integer = 0 To listview.Columns.Count - 1
            listview.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent)
            listview.Columns(listview.Columns.Count - 1).Width = -2
        Next
    End Sub

End Module

Class ListViewItemComparer
    Implements IComparer
    Private col As Integer
    Private order As SortOrder

    Public Sub New()
        col = 0
        order = SortOrder.Ascending
    End Sub

    Public Sub New(column As Integer, order As SortOrder)
        col = column
        Me.order = order
    End Sub

    Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
        Dim returnVal As Integer = -1
        Dim xx As ListViewItem = x
        Dim yy As ListViewItem = y

        Dim count = xx.ListView.Items.Count

        If xx.ListView.Columns(col).Tag = Nothing Then xx.ListView.Columns(col).Tag = "Text"
        If xx.ListView.Columns(col).Tag = "Numeric" Then
            Dim fl1 As Single = Single.Parse(xx.SubItems(col).Text)
            Dim fl2 As Single = Single.Parse(yy.SubItems(col).Text)

            If order = SortOrder.Ascending Then
                Return fl1.CompareTo(fl2)
            Else
                Return fl2.CompareTo(fl1)
            End If
        Else
            Dim str1 As String = xx.SubItems(col).Text
            Dim str2 As String = yy.SubItems(col).Text

            If order = SortOrder.Ascending Then
                Return str1.CompareTo(str2)
            Else
                Return str2.CompareTo(str1)
            End If
        End If
    End Function
End Class