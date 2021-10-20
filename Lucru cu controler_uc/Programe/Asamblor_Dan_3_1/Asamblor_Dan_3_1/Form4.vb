Public Class Form4

    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = "Sigur initializati TOATA memoria ?!?!"
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        For i_contor = 0 To 31
            For j_contor = 0 To dimensiune_pagina - 1
                memorie_8k(i_contor, j_contor) = "0"
            Next j_contor
        Next i_contor
        For i_contor = 0 To 31
            structura_memorie(i_contor) = ""
            If pagini_introduse(i_contor) Then
                Form3.ListBox2.Items.Add(i_contor)
            End If
        Next
        Form3.Label3.Text = "Constructia realizata:"
        Form3.Button1.Visible = False
        Form3.Button2.Visible = False
        Form3.Button3.Visible = False
        Form3.Button4.Visible = False
        Form3.ListBox1.SelectedIndex = 0
        Me.Close()
    End Sub
End Class