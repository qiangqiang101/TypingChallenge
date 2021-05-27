Public Class RoundedRectCorners

    Public TopLeft As Boolean
    Public TopRight As Boolean
    Public BottomLeft As Boolean
    Public BottomRight As Boolean

    Public Sub New(All As Boolean)
        TopLeft = All
        TopRight = All
        BottomLeft = All
        BottomRight = All
    End Sub

    Public Sub New(tl As Boolean, tr As Boolean, bl As Boolean, br As Boolean)
        TopLeft = tl
        TopRight = tr
        BottomLeft = bl
        BottomRight = br
    End Sub

End Class