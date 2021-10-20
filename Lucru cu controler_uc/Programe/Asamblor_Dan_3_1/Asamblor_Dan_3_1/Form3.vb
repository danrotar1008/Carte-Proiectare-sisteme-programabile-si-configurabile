Imports System
Imports System.IO

Public Class Form3

    Dim numar_pagini_completate As Byte = 0

    Private Sub Form3_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        constructie_memorie = False
        Form1.Visible() = True

    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = "Pentru constructia memoriei de 8ko se introduce" _
            & vbCrLf & "pagina unde va fi asezat codul si apoi fisierul" _
             & vbCrLf & "text de asamblat."
        For i_contor = 0 To 31
            For j_contor = 0 To dimensiune_pagina - 1
                memorie_8k(i_contor, j_contor) = "0"
            Next j_contor
        Next i_contor
        For i_contor = 0 To 31
            ListBox1.Items.Add(i_contor.ToString())
        Next i_contor
        ListBox1.SelectedIndex = 0
        Button1.Visible = False
        Button2.Visible = False
        Button3.Visible = False
        Button4.Visible = False
        Button5.Visible = False
        numar_pagini_completate = 0
        For i_contor = 0 To 31
            structura_memorie(i_contor) = ""
            pagini_introduse(i_contor) = False
        Next i_contor
        ListBox2.Visible = False
        Label2.Text = "Page Insert"
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        'Salvare fisier memorie 8k
        Dim nume_fisier_8k As String
        Dim dialog_salvare_fisier As New SaveFileDialog

        'dialog_salvare_fisier.InitialDirectory = "e:\"
        dialog_salvare_fisier.InitialDirectory = ""
        dialog_salvare_fisier.Filter = "coe files (*.coe)|*.coe|All files (*.*)|*.*"
        dialog_salvare_fisier.FilterIndex = 1
        dialog_salvare_fisier.RestoreDirectory = True

        If dialog_salvare_fisier.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            Try
                nume_fisier_8k = dialog_salvare_fisier.FileName
                'aici salvez matrice memorie_8k(32, dimensiune_pagina)
                'daca fisierul exista, il sterg
                If My.Computer.FileSystem.FileExists(nume_fisier_8k) Then
                    My.Computer.FileSystem.DeleteFile(nume_fisier_8k)
                End If

                'deschid fisier de initializare memorie
                Dim fisier_memorie8k = My.Computer.FileSystem.OpenTextFileWriter(nume_fisier_8k, True)
                fisier_memorie8k.WriteLine("==================================")
                For i_contor = 0 To 31
                    fisier_memorie8k.Write(structura_memorie(i_contor))
                Next
                fisier_memorie8k.WriteLine()
                fisier_memorie8k.WriteLine()
                fisier_memorie8k.WriteLine("==================================")
                For i_contor = 0 To 31
                    For j_contor = 0 To dimensiune_pagina - 1
                        fisier_memorie8k.WriteLine(memorie_8k(i_contor, j_contor) & ",")
                    Next
                Next
                fisier_memorie8k.Close()
            Catch ex As Exception
                MsgBox("Eroare la scriere fisier! ")
            End Try
        End If
        MsgBox("Constructie finalizata")
        Me.Close()
        Form1.Visible() = True
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim dialogDeschidFisier1 As New OpenFileDialog()
        Dim MyStream As Stream = Nothing
        Dim FisierCitit As String

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
                    FisierCitit = My.Computer.FileSystem.ReadAllText(nume_fisier)
                End If
            Catch ex As Exception
                MsgBox("Eroare citire fisier")
            End Try
        End If
        If (MyStream IsNot Nothing) Then
            MyStream.Close()
            numar_pagina = ListBox1.SelectedIndex
            constructie_memorie = True
            asamblare()
            For i_contor = 0 To dimensiune_pagina - 1
                memorie_8k(numar_pagina, i_contor) = "0"
            Next i_contor
            For i_contor = 0 To contor_program - 1
                memorie_8k(numar_pagina, i_contor) = temp_asamblat(i_contor)
            Next i_contor
            constructie_memorie = False
            numar_pagini_completate = numar_pagini_completate + 1
            structura_memorie(numar_pagina) = vbCrLf & nume_fisier & " => pagina:  " & numar_pagina
            pagini_introduse(numar_pagina) = True
            Label3.Text = Label3.Text & structura_memorie(numar_pagina)
            Button1.Visible = True
            Button3.Visible = True
            Button4.Visible = True
            Button2.Visible = False
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form4.Visible = True
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Button4.Visible = False
        ListBox1.Visible = False
        ListBox2.Items.Clear()
        For i_contor = 0 To 31
            If pagini_introduse(i_contor) Then
                ListBox2.Items.Add(i_contor)
            End If
        Next
        ListBox2.Visible = True
        Label2.Text = "Page Delete"
        Button5.Visible = True
    End Sub

    Private Sub ListBox1_Click(sender As Object, e As EventArgs) Handles ListBox1.Click
        Button2.Visible = True
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        numar_pagina = Val(ListBox2.Text())

        structura_memorie(numar_pagina) = ""
        pagini_introduse(numar_pagina) = False
        Label3.Text = "Constructia realizata:"
        For i_contor = 0 To 31
            Label3.Text = Label3.Text & structura_memorie(i_contor)
        Next
        For j_contor = 0 To dimensiune_pagina - 1
            memorie_8k(numar_pagina, j_contor) = "0"
        Next j_contor
        numar_pagini_completate = numar_pagini_completate - 1
        ListBox2.Visible = False
        ListBox1.Visible = True
        Label2.Text = "Page Insert"
        If numar_pagini_completate = 0 Then
            Button1.Visible = False
            Button2.Visible = False
            Button3.Visible = False
            Button4.Visible = False
            Button5.Visible = False
            ListBox1.SelectedIndex = 0
        Else
            Button4.Visible = True
            Button5.Visible = False
        End If

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Label3.Text = "Constructia realizata:"
        For i_contor = 0 To 31
            Label3.Text = Label3.Text & structura_memorie(i_contor)
        Next
        ListBox2.Visible = False
        ListBox1.Visible = True
        Label2.Text = "Page Insert"
        If numar_pagini_completate = 0 Then
            Button1.Visible = False
            Button2.Visible = False
            Button3.Visible = False
            Button4.Visible = False
            Button5.Visible = False
            ListBox1.SelectedIndex = 0
        Else
            Button4.Visible = True
            Button5.Visible = False
        End If

    End Sub
End Class