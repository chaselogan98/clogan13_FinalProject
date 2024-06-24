Imports System.IO
' ###########################################################
' #                                                         #
' # Author: Chase Logan                                     #
' # Email:  clogan13@uncc.edu                               #
' # SID: 801209646                                          #
' #                                                         #
' # Program Name: LeaderboardForm.vb                        #                                                        
' #                                                         #
' ###########################################################
#Region " Load"
Public Class LeaderboardForm
    Private Sub LeaderboardForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set up the form
        Me.Text = "Leaderboard"
        Me.Size = New Size(400, 300)

        ' Add ListView
        Dim leaderboardListView As New ListView()
        leaderboardListView.Location = New Point(10, 10)
        leaderboardListView.Size = New Size(360, 240)
        leaderboardListView.View = View.Details
        leaderboardListView.Columns.Add("Player Name", 180, HorizontalAlignment.Left)
        leaderboardListView.Columns.Add("High Score", 180, HorizontalAlignment.Left)
        Me.Controls.Add(leaderboardListView)

        ' Load leaderboard data
        Dim filePath As String = "leaderboard.txt"
        If File.Exists(filePath) Then
            Dim lines As String() = File.ReadAllLines(filePath)
            For Each line As String In lines
                Dim parts As String() = line.Split(","c)
                If parts.Length = 2 Then
                    Dim item As New ListViewItem(parts(0))
                    item.SubItems.Add(parts(1))
                    leaderboardListView.Items.Add(item)
                End If
            Next
        Else
            MessageBox.Show("No leaderboard data found.", "Leaderboard")
        End If
    End Sub
End Class

#End Region