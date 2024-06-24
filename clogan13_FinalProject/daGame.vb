Imports System.Drawing
Imports System.IO
' ###########################################################
' #                                                         #
' # Author: Chase Logan                                     #
' # Email:  clogan13@uncc.edu                               #
' # SID: 801209646                                          #
' #                                                         #
' # Program Name: daGame.vb                                 #                                                        
' #                                                         #
' ###########################################################
Public Class daGame
#Region " Variables"
    Private player As Rectangle
    Private obstacles As List(Of Rectangle)
    Private score As Integer
    Private highScore As Integer
    Private isJumping As Boolean
    Private jumpSpeed As Integer
    Private gravity As Integer
    Private WithEvents GameTimer As Timer
    Public Property PlayerName As String
    Public Property PlayerColor As String
    Private WithEvents pauseButton As Button
    Private WithEvents exitButton As Button
    Private WithEvents mainMenuButton As Button
    Private isPaused As Boolean
#End Region
#Region " Load daForm"
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize game elements
        player = New Rectangle(50, 200, 50, 50)
        obstacles = New List(Of Rectangle)
        score = 0
        highScore = LoadHighScore(PlayerName)
        isJumping = False
        jumpSpeed = 0
        gravity = 4
        isPaused = False

        ' Set up the form
        Me.DoubleBuffered = True
        Me.KeyPreview = True

        ' Initialize and start the game timer
        GameTimer = New Timer()
        GameTimer.Interval = 20 ' Set the interval to 20 ms
        GameTimer.Start()

        ' Add pause button
        pauseButton = New Button()
        pauseButton.Text = "Pause"
        pauseButton.Location = New Point(10, 50)
        pauseButton.TabStop = False ' Prevent the button from receiving focus
        AddHandler pauseButton.Click, AddressOf PauseGame
        Me.Controls.Add(pauseButton)

        ' Add exit button
        exitButton = New Button()
        exitButton.Text = "Exit"
        exitButton.Location = New Point(100, 50)
        exitButton.TabStop = False ' Prevent the button from receiving focus
        AddHandler exitButton.Click, AddressOf ExitGame
        Me.Controls.Add(exitButton)

        ' Add main menu button
        mainMenuButton = New Button()
        mainMenuButton.Text = "Main Menu"
        mainMenuButton.Location = New Point(190, 50)
        mainMenuButton.TabStop = False ' Prevent the button from receiving focus
        AddHandler mainMenuButton.Click, AddressOf ReturnToMainMenu
        Me.Controls.Add(mainMenuButton)
    End Sub
    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        Dim g As Graphics = e.Graphics

        ' Draw player
        Dim playerBrush As Brush = Brushes.Blue
        Select Case PlayerColor
            Case "Red"
                playerBrush = Brushes.Red
            Case "Green"
                playerBrush = Brushes.Green
            Case "Yellow"
                playerBrush = Brushes.Yellow
        End Select
        g.FillRectangle(playerBrush, player)

        ' Draw obstacles
        For Each obstacle As Rectangle In obstacles
            g.FillRectangle(Brushes.Red, obstacle)
        Next

        ' Draw score
        g.DrawString("Score: " & score, Me.Font, Brushes.Black, 10, 10)
        g.DrawString("High Score: " & highScore, Me.Font, Brushes.Black, 10, 30)
    End Sub
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Space AndAlso Not isJumping AndAlso Not isPaused Then
            isJumping = True
            jumpSpeed = -30 ' Increase jump height
        ElseIf e.KeyCode = Keys.P Then
            PauseGame(Nothing, Nothing)
        End If
    End Sub
#End Region
#Region " Timers"
    Private Sub GameTimer_Tick(sender As Object, e As EventArgs) Handles GameTimer.Tick
        If isPaused Then Return

        ' Update player position
        If isJumping Then
            player.Y += jumpSpeed
            jumpSpeed += gravity
            If player.Y >= 200 Then
                player.Y = 200
                isJumping = False
            End If
        End If

        ' Update obstacles
        For i As Integer = 0 To obstacles.Count - 1
            obstacles(i) = New Rectangle(obstacles(i).X - 10, obstacles(i).Y, obstacles(i).Width, obstacles(i).Height)
        Next

        ' Remove off-screen obstacles
        obstacles.RemoveAll(Function(obstacle) obstacle.X + obstacle.Width < 0)

        ' Add new obstacles
        If obstacles.Count = 0 OrElse obstacles.Last().X < Me.Width - 250 Then ' Increase spacing
            Dim rnd As New Random()
            Dim height As Integer = rnd.Next(20, 70) ' Adjust height range
            obstacles.Add(New Rectangle(Me.Width, 250 - height, 20, height))
        End If

        ' Check for collisions
        For Each obstacle As Rectangle In obstacles
            If player.IntersectsWith(obstacle) Then
                ' Game over
                If score > highScore Then
                    highScore = score
                    SaveHighScore(PlayerName, highScore)
                End If
                score = 0
                obstacles.Clear()
                Exit For
            End If
        Next

        ' Update score
        score += 1

        ' Redraw the form
        Me.Invalidate()
    End Sub

#End Region
#Region " Methods"
    Private Sub SaveHighScore(name As String, score As Integer)
        Dim filePath As String = "leaderboard.txt"
        Dim lines As New List(Of String)
        Dim found As Boolean = False

        If File.Exists(filePath) Then
            lines.AddRange(File.ReadAllLines(filePath))
        End If

        Using writer As New StreamWriter(filePath, False)
            For Each line As String In lines
                Dim parts As String() = line.Split(","c)
                If parts.Length = 2 AndAlso parts(0) = name Then
                    writer.WriteLine(name & "," & score)
                    found = True
                Else
                    writer.WriteLine(line)
                End If
            Next

            If Not found Then
                writer.WriteLine(name & "," & score)
            End If
        End Using
    End Sub

    Private Function LoadHighScore(name As String) As Integer
        Dim filePath As String = "leaderboard.txt"
        If File.Exists(filePath) Then
            Dim lines As String() = File.ReadAllLines(filePath)
            For Each line As String In lines
                Dim parts As String() = line.Split(","c)
                If parts.Length = 2 AndAlso parts(0) = name Then
                    Return Integer.Parse(parts(1))
                End If
            Next
        End If
        Return 0
    End Function

    Private Sub PauseGame(sender As Object, e As EventArgs)
        isPaused = Not isPaused
        pauseButton.Text = If(isPaused, "Resume", "Pause")
    End Sub

    Private Sub ExitGame(sender As Object, e As EventArgs)
        Me.Close()
        Application.Exit()
    End Sub

    Private Sub ReturnToMainMenu(sender As Object, e As EventArgs)
        Dim mainMenuForm As New MainMenuForm()
        mainMenuForm.Show()
        Me.Close()
    End Sub

#End Region
End Class