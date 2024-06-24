Imports System.IO
' ###########################################################
' #                                                         #
' # Author: Chase Logan                                     #
' # Email:  clogan13@uncc.edu                               #
' # SID: 801209646                                          #
' #                                                         #
' # Program Name: MainMenuForm.vb                           #                                                        
' #                                                         #
' ###########################################################
Public Class MainMenuForm
#Region " Load"
    Private Sub MainMenuForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set up the form
        Me.Text = "Main Menu"
        Me.Size = New Size(400, 300)

        ' Add controls
        Dim nameLabel As New Label()
        nameLabel.Text = "Enter your name:"
        nameLabel.Location = New Point(50, 50)
        nameLabel.Size = New Size(100, 20)
        Me.Controls.Add(nameLabel)

        Dim nameTextBox As New TextBox()
        nameTextBox.Name = "NameTextBox"
        nameTextBox.Location = New Point(160, 50)
        nameTextBox.Size = New Size(180, 20)
        Me.Controls.Add(nameTextBox)

        Dim colorLabel As New Label()
        colorLabel.Text = "Choose your color:"
        colorLabel.Location = New Point(50, 80)
        colorLabel.Size = New Size(100, 20)
        Me.Controls.Add(colorLabel)

        Dim colorComboBox As New ComboBox()
        colorComboBox.Name = "ColorComboBox"
        colorComboBox.Location = New Point(160, 80)
        colorComboBox.Size = New Size(180, 20)
        colorComboBox.Items.AddRange(New String() {"Blue", "Red", "Green", "Yellow"})
        colorComboBox.SelectedIndex = 0 ' Default to Blue
        Me.Controls.Add(colorComboBox)

        Dim startButton As New Button()
        startButton.Text = "Start Game"
        startButton.Location = New Point(50, 120)
        startButton.Size = New Size(100, 30)
        AddHandler startButton.Click, AddressOf StartGame
        Me.Controls.Add(startButton)

        Dim leaderboardButton As New Button()
        leaderboardButton.Text = "Show Leaderboard"
        leaderboardButton.Location = New Point(160, 120)
        leaderboardButton.Size = New Size(180, 30)
        AddHandler leaderboardButton.Click, AddressOf ShowLeaderboard
        Me.Controls.Add(leaderboardButton)

        Dim exitButton As New Button()
        exitButton.Text = "Exit"
        exitButton.Location = New Point(50, 160)
        exitButton.Size = New Size(100, 30)
        AddHandler exitButton.Click, AddressOf ExitApplication
        Me.Controls.Add(exitButton)
    End Sub
#End Region
#Region " Methods"
    ' Method for Starting the Game
    Private Sub StartGame(sender As Object, e As EventArgs)
        Dim nameTextBox As TextBox = Me.Controls("NameTextBox")
        Dim colorComboBox As ComboBox = Me.Controls("ColorComboBox")
        Dim playerName As String = nameTextBox.Text
        Dim playerColor As String = colorComboBox.SelectedItem.ToString()

        If String.IsNullOrWhiteSpace(playerName) Then
            MessageBox.Show("Please enter your name.", "Error")
            Return
        End If

        Dim gameForm As New daGame()
        gameForm.PlayerName = playerName
        gameForm.PlayerColor = playerColor
        gameForm.Show()
        Me.Hide()
    End Sub
    ' Method for showing the Leaderboard
    Private Sub ShowLeaderboard(sender As Object, e As EventArgs)
        Dim leaderboardForm As New LeaderboardForm()
        leaderboardForm.Show()
    End Sub
    ' Exit
    Private Sub ExitApplication(sender As Object, e As EventArgs)
        Application.Exit()
    End Sub
#End Region
End Class