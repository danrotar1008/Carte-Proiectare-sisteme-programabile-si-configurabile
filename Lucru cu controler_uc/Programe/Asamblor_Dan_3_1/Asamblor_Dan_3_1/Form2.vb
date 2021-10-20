Imports System.IO
Public Class Form2
    Dim help_path As String
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles Me.Load
        help_path = Path.GetFullPath("help.txt")
        'MsgBox(help_path)
        If My.Computer.FileSystem.FileExists(help_path) Then
            RichTextBox1.Text = My.Computer.FileSystem.ReadAllText(help_path)
        Else
            MsgBox("Fisierul HELP nu exista!")
        End If
    End Sub

End Class