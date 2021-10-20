Public Class Form6

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Salvare fisier configuratie simulator
        Dim nume_fisier_cfg As String
        Dim dialog_salvare_fisier As New SaveFileDialog

        dialog_salvare_fisier.InitialDirectory = "e:\"
        dialog_salvare_fisier.Filter = "cfg files (*.cfg)|*.cfg|All files (*.*)|*.*"
        dialog_salvare_fisier.FilterIndex = 1
        dialog_salvare_fisier.RestoreDirectory = True

        If dialog_salvare_fisier.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            Try
                nume_fisier_cfg = dialog_salvare_fisier.FileName
                'aici salvez fisier de configurare simulator
                'daca fisierul exista, il sterg
                If My.Computer.FileSystem.FileExists(nume_fisier_cfg) Then
                    My.Computer.FileSystem.DeleteFile(nume_fisier_cfg)
                End If
                'deschid fisier de configurare
                Dim fisier_cfg = My.Computer.FileSystem.OpenTextFileWriter(nume_fisier_cfg, True)
                fisier_cfg.WriteLine(numar_port_intrare_simulator_0)
                fisier_cfg.WriteLine(continut_port_intrare_simulator_0)
                fisier_cfg.WriteLine(continut_port_intrare_simulator_1)
                fisier_cfg.WriteLine(numar_port_intrare_simulator_1)
                fisier_cfg.WriteLine(continut_port_intrare_simulator_2)
                fisier_cfg.WriteLine(numar_port_intrare_simulator_2)
                fisier_cfg.WriteLine(numar_port_iesire_simulator_0)
                fisier_cfg.WriteLine(numar_port_iesire_simulator_1)
                fisier_cfg.WriteLine(numar_port_iesire_simulator_2)
                fisier_cfg.Close()
            Catch ex As Exception
                MsgBox("Eroare la scriere fisier configuratie!")
            End Try
        End If
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class