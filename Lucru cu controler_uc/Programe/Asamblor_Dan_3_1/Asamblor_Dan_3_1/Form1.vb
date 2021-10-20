Imports System
Imports System.IO
Public Class Form1
    Dim fisier_de_asamblat_schimbat As Boolean = False 'daca am facut o schimbare
    'in RichTextBox1 unde este fisierul de asamblat
    Dim modificare_deschidere_fisier As Boolean = False 'la deschiderea unui fisier
    'continutul RichTextBox1 se schimba dar asta nu trebuie sa duca la o cerere
    'de salvare.....
    Public nume_fisier_memorie As String

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        Dim dialogDeschidFisier1 As New OpenFileDialog()
        Dim MyStream As Stream = Nothing
        Dim FisierCitit As String

        Form5.Close()

        SimulationToolStripMenuItem.Visible = False

        'dialogDeschidFisier1.InitialDirectory = "e:\"
        dialogDeschidFisier1.InitialDirectory = ""
        dialogDeschidFisier1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
        dialogDeschidFisier1.FilterIndex = 1
        dialogDeschidFisier1.RestoreDirectory = True

        If dialogDeschidFisier1.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            Try
                MyStream = dialogDeschidFisier1.OpenFile
                If (MyStream IsNot Nothing) Then
                    nume_fisier = dialogDeschidFisier1.FileName
                    Label1.Text = "File for assemble: " & nume_fisier
                    FisierCitit = My.Computer.FileSystem.ReadAllText(nume_fisier)
                    fisier_de_asamblat_schimbat = False
                    modificare_deschidere_fisier = True
                    RichTextBox1.Text = FisierCitit
                End If
                AssambleToolStripMenuItem.Visible = True
            Catch ex As Exception
                MsgBox("Eroare citire fisier")
            End Try
        End If
        If (MyStream IsNot Nothing) Then
            MyStream.Close()
        End If
        asamblare()
        If asamblare_corecta Then
            SimulationToolStripMenuItem.Visible = True
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        If fisier_de_asamblat_schimbat Then
            MsgBox("Salvati fisierul de asamblat!")
        Else
            Close()
        End If
    End Sub

    Private Sub RunToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RunToolStripMenuItem.Click
        If fisier_de_asamblat_schimbat Then
            MsgBox("Salvati fisierul de asamblat!")
        Else
            asamblare()
            If asamblare_corecta Then
                SimulationToolStripMenuItem.Visible = True
            Else
                SimulationToolStripMenuItem.Visible = False
            End If
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AssambleToolStripMenuItem.Visible = False
        SimulationToolStripMenuItem.Visible = False
    End Sub

    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click
        If fisier_de_asamblat_schimbat Then
            MsgBox("Salvati fisierul de asamblat!")
        Else
            RichTextBox1.Clear()
            RichTextBox2.Clear()
            Label1.Text = "File for assemble: "
            nume_fisier = "" 'nu pot sa-l sterg! duce la eroare!
        End If
        SimulationToolStripMenuItem.Visible = False
        fisier_de_asamblat_schimbat = False
    End Sub

    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox1.TextChanged
        If Not modificare_deschidere_fisier Then
            'MsgBox("S-a schimbat starea fisier modificat!!!!!!!!!!")
            fisier_de_asamblat_schimbat = True
            asamblare_corecta = False
        Else
            modificare_deschidere_fisier = False
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        If My.Computer.FileSystem.FileExists(nume_fisier) Then
            My.Computer.FileSystem.DeleteFile(nume_fisier)
        End If
        If nume_fisier <> "" Then
            My.Computer.FileSystem.WriteAllText(nume_fisier, RichTextBox1.Text, False)
        Else
            salvare_fisier_de_asamblat()
        End If
        fisier_de_asamblat_schimbat = False
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveAsToolStripMenuItem.Click
        salvare_fisier_de_asamblat()
        fisier_de_asamblat_schimbat = False
    End Sub

    Private Sub salvare_fisier_de_asamblat()
        Dim dialog_salvare_fisier As New SaveFileDialog

        'dialog_salvare_fisier.InitialDirectory = "e:\"
        dialog_salvare_fisier.InitialDirectory = ""
        dialog_salvare_fisier.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
        dialog_salvare_fisier.FilterIndex = 1
        dialog_salvare_fisier.RestoreDirectory = True

        If dialog_salvare_fisier.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            Try
                nume_fisier = dialog_salvare_fisier.FileName
                My.Computer.FileSystem.WriteAllText(nume_fisier, RichTextBox1.Text, False)
            Catch ex As Exception
                MsgBox("Eroare la scriere fisier! ")
            End Try
        End If

    End Sub

    Private Sub HELPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HELPToolStripMenuItem.Click
        Form2.Visible = True
    End Sub

    Private Sub WievToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WievToolStripMenuItem.Click
        Dim FisierAsamblat As String
        RichTextBox2.Clear()
        FisierAsamblat = My.Computer.FileSystem.ReadAllText(nume_asamblat)
        RichTextBox2.Text = FisierAsamblat
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        Dim FisierListing As String
        RichTextBox2.Clear()
        FisierListing = My.Computer.FileSystem.ReadAllText(nume_listing)
        RichTextBox2.Text = FisierListing
    End Sub

    Private Sub MemoryConstructToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MemoryConstructToolStripMenuItem.Click
        Form3.Visible = True
        Me.Visible = False
    End Sub

    Private Sub SimulationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SimulationToolStripMenuItem.Click
        If (Not fisier_de_asamblat_schimbat) And (asamblare_corecta) Then
            Form5.Visible = True
        Else
            MsgBox("Salvati si asamblati fisierul sursa !!!!")
        End If
    End Sub
    Private Sub Form1_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If fisier_de_asamblat_schimbat Then
            MsgBox("Salvati fisierul de asamblat!")
            e.Cancel = True
        Else
            e.Cancel = False
        End If
    End Sub

    Private Sub ViewIncludeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewIncludeToolStripMenuItem.Click
        Dim FisierIncludeCreat As String
        RichTextBox2.Clear()
        FisierIncludeCreat = My.Computer.FileSystem.ReadAllText(nume_include_creat)
        RichTextBox2.Text = FisierIncludeCreat
    End Sub
End Class
