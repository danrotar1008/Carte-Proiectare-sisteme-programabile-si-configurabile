Imports System.IO
Public Class Form5

    Dim memorie_simulator(dimensiune_pagina) As String
    Dim memorie_comuna_simulator(32, dimensiune_pagina) As String
    Dim cf As Byte = 0
    Dim cft As Byte = 0
    Dim zf As Byte = 0
    Dim acumulator_simulator As Integer
    Dim contor_program_simulator As UInteger
    Dim contor_stiva_simulator As Byte
    Dim port_intrare_simulator(256) As Byte
    Dim port_iesire_simulator(256) As Byte
    Dim numar_port_iesire_simulator As Byte
    Dim numar_port_intrare_simulator As Byte
    Dim numar_pagina_simulator As Byte
    Dim continut_port_iesire_simulator_0 As Byte
    Dim continut_port_iesire_simulator_1 As Byte
    Dim continut_port_iesire_simulator_2 As Byte
    Dim nume_fisier_simulator As String
    Dim initializare_simulare As Boolean = False
    Dim step_selected As Boolean = False
    Dim nume_eticheta_simulator As String
    Dim listbox1_index As Byte = 0 'pastreaza valoare indexului in timpul executiei.
    'Daca utilizatorul se plimba in fereastre ca sa caute ceva, se modifica valoarea
    'contorului de program simulator si la reluarea executiei, valoarea unde s-a ramas
    'cu executia se ia de aici.
    Dim configuratie_schimbata As Boolean = False
    Dim executie_continua As Boolean = False
    '-----------------------------------------
    Dim validare_intreruperi_simulator As Byte 'bitii 0-6 indica validarea intreruperii daca este = 1
    Dim intrerupere_simulator As Byte 'intreruperea aparuta
    '-----------------------------------------
    Dim constanta_timer_simulator As Byte 'constanta inscrisa in timer
    Dim contor_timer_simulator As Byte 'contorul timer
    '-----------------------------------------
    Dim emisie_seriala_simulator As Byte 'octet trimis prin interfata seriala
    Dim receptie_seriala_simulator As Byte 'octet receptionat prin interfata seriala
    Dim buffer_receptie_gol As Byte '1 = buffer gol, 0 buffer contine caracter
    '----------------------------------------
    Dim pagina_selectata_transfer As Byte 'pagina comuna unde se face transferul

    Private Sub QuitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles QuitToolStripMenuItem.Click
        If configuratie_schimbata Then
            Form6.Visible = True
        End If
        Close()
    End Sub

    Sub listbox1_redesenare()
        'in aceasta fereastra afisez codul asamblat, asezarea lui in pagina si etichetele
        'programului simulat (nu apar etichetele predefinite sau cele incluse)
        'procedeul e urmatorul: pentru fiecare linie din pagina (256 de linii) se baleaza
        'vectorul cu valorile etichetelor si daca gasesc o eticheta cu valoare pozitiei
        'respective atunci eticheta se afiseaza (daca sunt mai multe etichete, se afiseaza
        'prima gasita
        ListBox1.Items.Clear()
        Dim spatii As String = " "
        Dim i_contor As UInteger
        For j_contor As UInteger = 0 To dimensiune_pagina - 1 'contorul pozitiilor din pagina afisata
            i_contor = 0 'caut o eticheta cu valoarea j_contor (valoarea pozitiei pe care ma aflu)
            While (((j_contor <> temp_valoare_etichete(i_contor)) Or temp_etichete_externe(i_contor)) And (i_contor < contor_etichete))
                i_contor = i_contor + 1
            End While
            If i_contor < contor_etichete Then
                'am gasit o eticheta si o afisez
                'daca pe pozitia respectiva din pagina gasesc o eticheta cu aceasta valoare
                'si eticheta nu este externa atunci afisez eticheta
                nume_eticheta_simulator = temp_nume_etichete(i_contor) 'numele etichetei afisate
                If Len(nume_eticheta_simulator) > 10 Then 'eticheta este truncheata la 10 caractere
                    nume_eticheta_simulator = LSet(nume_eticheta_simulator, 10)
                End If
                For k_contor = 0 To (10 - Len(nume_eticheta_simulator))
                    spatii = spatii & " " 'adaug spatii pina completez 10 caractere
                Next
                nume_eticheta_simulator = nume_eticheta_simulator & spatii & vbTab
                spatii = " "
                ListBox1.Items.Add(j_contor & " - " & nume_eticheta_simulator & "| " & memorie_simulator(j_contor))
                'adaug eticheta si codul corespunatori pozitiei din pagina afisata
            Else
                'nu am gasit nici o eticheta si afisez doar codul
                ListBox1.Items.Add(j_contor & vbTab & vbTab & "| " & memorie_simulator(j_contor))
                'nu este eticheta deci afisez linia doar cu pozitia si codul respectiv
            End If
        Next j_contor
    End Sub

    Sub actualizare_afisare_port()
        TextBox5.Text = numar_port_intrare_simulator_0
        TextBox6.Text = continut_port_intrare_simulator_0
        TextBox7.Text = continut_port_intrare_simulator_1
        TextBox8.Text = numar_port_intrare_simulator_1
        TextBox9.Text = continut_port_intrare_simulator_2
        TextBox10.Text = numar_port_intrare_simulator_2
        TextBox11.Text = continut_port_iesire_simulator_0
        TextBox12.Text = numar_port_iesire_simulator_0
        TextBox13.Text = continut_port_iesire_simulator_1
        TextBox14.Text = numar_port_iesire_simulator_1
        TextBox15.Text = continut_port_iesire_simulator_2
        TextBox16.Text = numar_port_iesire_simulator_2

    End Sub

    Sub initializare_simulator()
        initializare_simulare = False
        cf = 0
        zf = 0
        acumulator_simulator = 0
        contor_program_simulator = 0
        contor_stiva_simulator = 255
        For i_contor = 0 To dimensiune_pagina - 1
            port_intrare_simulator(i_contor) = 0
            port_iesire_simulator(i_contor) = 0
        Next
        numar_port_iesire_simulator = 0
        numar_port_intrare_simulator = 0
        numar_pagina_simulator = 0
        numar_port_intrare_simulator_0 = 0
        numar_port_intrare_simulator_1 = 1
        numar_port_intrare_simulator_2 = 2
        numar_port_iesire_simulator_0 = 0
        numar_port_iesire_simulator_1 = 1
        numar_port_iesire_simulator_2 = 2
        continut_port_intrare_simulator_0 = 0
        continut_port_intrare_simulator_1 = 0
        continut_port_intrare_simulator_2 = 0
        continut_port_iesire_simulator_0 = 0
        continut_port_iesire_simulator_1 = 0
        continut_port_iesire_simulator_2 = 0
        pagina_selectata_transfer = 0

        For j_contor = 0 To 31
            For i_contor = 0 To dimensiune_pagina - 1
                memorie_simulator(i_contor) = "0"
                memorie_comuna_simulator(j_contor, i_contor) = "0"
            Next
        Next
        For i_contor = 0 To contor_program - 1
            memorie_simulator(i_contor) = temp_asamblat(i_contor)
            memorie_comuna_simulator(0, i_contor) = temp_asamblat(i_contor)
        Next
        listbox1_redesenare()
        ListBox3.Items.Clear()
        For i_contor = 0 To dimensiune_pagina - 1
            ListBox3.Items.Add(i_contor & vbTab & "| " & memorie_comuna_simulator(pagina_selectata_transfer, i_contor))
        Next
        Label13.Text = "Pagina " & pagina_selectata_transfer
        ListBox1.SelectedIndex = 0
        TextBox1.Text = acumulator_simulator
        TextBox2.Text = contor_program_simulator
        TextBox3.Text = contor_stiva_simulator
        TextBox4.Text = numar_pagina_simulator
        actualizare_afisare_port()
        TextBox17.Text = cf
        TextBox18.Text = zf
        Button1.Text = "GO"
        RadioButton1.Checked = True
        nume_fisier_simulator = nume_fisier
        Label16.Text = Label16.Text & " " & nume_fisier_simulator
        Dim fisier_simulat = My.Computer.FileSystem.OpenTextFileReader(nume_fisier_simulator)
        ListBox2.Items.Clear()
        While Not fisier_simulat.EndOfStream
            ListBox2.Items.Add(fisier_simulat.ReadLine)
        End While
        listbox1_index = 0
        cautare_linie_text()
        Timer1.Interval = 100
        '--------------
        intrerupere_simulator = 0
        validare_intreruperi_simulator = 0
        Label22.Text = validare_intreruperi_simulator
        emisie_seriala_simulator = 0
        TextBox19.Text = emisie_seriala_simulator
        receptie_seriala_simulator = 0
        TextBox20.Text = receptie_seriala_simulator
        buffer_receptie_gol = 1
        constanta_timer_simulator = 0
        contor_timer_simulator = 0
        TextBox21.Text = contor_timer_simulator
        initializare_simulare = True
    End Sub

    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles Me.Load
        initializare_simulator()
    End Sub
    Private Sub ListBox2_Click(sender As Object, e As EventArgs) Handles ListBox2.Click
        If initializare_simulare Then
            contor_program_simulator = corespondenta_text_cod(ListBox2.SelectedIndex)
            ListBox1.SelectedIndex = contor_program_simulator
            TextBox2.Text = contor_program_simulator
        End If
    End Sub
    Private Sub ListBox1_Click(sender As Object, e As EventArgs) Handles ListBox1.Click
        If initializare_simulare Then
            Dim pozitia_cautata As Integer
            Dim i_contor = 0
            pozitia_cautata = ListBox1.SelectedIndex
            If pozitia_cautata < contor_program Then
                While (corespondenta_text_cod(i_contor) <> pozitia_cautata) And (i_contor <= contor_linie)
                    i_contor = i_contor + 1
                End While
                While i_contor > contor_linie
                    pozitia_cautata = pozitia_cautata - 1
                    i_contor = 0
                    While (corespondenta_text_cod(i_contor) <> pozitia_cautata) And (i_contor <= contor_linie)
                        i_contor = i_contor + 1
                    End While
                End While
                contor_program_simulator = pozitia_cautata
                TextBox2.Text = contor_program_simulator
                ListBox1.SelectedIndex = contor_program_simulator
                ListBox2.SelectedIndex = i_contor
            End If
        End If

    End Sub

    Sub cautare_linie_text()
        Dim pozitia_cautata As Integer
        Dim i_contor = 0
        pozitia_cautata = contor_program_simulator
        If pozitia_cautata < contor_program Then
            While (corespondenta_text_cod(i_contor) <> pozitia_cautata) And (i_contor <= contor_linie - 2)
                i_contor = i_contor + 1
            End While
            'caut acum daca urmatoarele linii nu au aceeasi valoare ca sa ma duc pe
            'linia cea mai de jos
            While (corespondenta_text_cod(i_contor) = pozitia_cautata) And (i_contor <= contor_linie - 2)
                i_contor = i_contor + 1
            End While
            'acum sunt pe prima linie diferita dupa cea care coincidea deci scad 1
            ListBox2.SelectedIndex = i_contor - 1
        End If
    End Sub

    Sub executie(ByVal contor As Byte)
        Dim instructiune As String
        'intreruperea 0 - daca a aparut intreruperea o execut
        If ((validare_intreruperi_simulator And 1) And (intrerupere_simulator And 1)) Then
            memorie_simulator(contor_stiva_simulator) = contor_program_simulator
            contor_stiva_simulator = contor_stiva_simulator - 1
            TextBox3.Text = contor_stiva_simulator
            listbox1_redesenare()
            contor_program_simulator = 2
            validare_intreruperi_simulator = validare_intreruperi_simulator And 254
            Label22.Text = validare_intreruperi_simulator
            CheckBox1.Checked = False
            'Intreruperea 1
        ElseIf ((validare_intreruperi_simulator And 2) And (intrerupere_simulator And 2)) Then
            memorie_simulator(contor_stiva_simulator) = contor_program_simulator
            contor_stiva_simulator = contor_stiva_simulator - 1
            TextBox3.Text = contor_stiva_simulator
            listbox1_redesenare()
            contor_program_simulator = 4
            validare_intreruperi_simulator = validare_intreruperi_simulator And 253
            Label22.Text = validare_intreruperi_simulator
            CheckBox2.Checked = False
        Else
            If memorie_simulator(contor) <= mnemonici.Length Then 'verific daca e un cod valid
                instructiune = mnemonici(Val(memorie_simulator(contor)))
                Select Case instructiune
                    Case "nop"
                        contor_program_simulator = contor_program_simulator + 1
                    Case "movla"
                        contor_program_simulator = contor_program_simulator + 1
                        acumulator_simulator = memorie_simulator(contor_program_simulator)
                        TextBox1.Text = acumulator_simulator
                        contor_program_simulator = contor_program_simulator + 1
                    Case "addla"
                        contor_program_simulator = contor_program_simulator + 1
                        acumulator_simulator = acumulator_simulator + memorie_simulator(contor_program_simulator)
                        If acumulator_simulator > 255 Then
                            acumulator_simulator = acumulator_simulator - 256
                            cf = 1
                        Else
                            cf = 0
                        End If
                        TextBox17.Text = cf
                        If acumulator_simulator = 0 Then
                            zf = 1
                        Else
                            zf = 0
                        End If
                        TextBox18.Text = zf
                        TextBox1.Text = acumulator_simulator
                        contor_program_simulator = contor_program_simulator + 1
                    Case "jmp"
                        contor_program_simulator = contor_program_simulator + 1
                        contor_program_simulator = memorie_simulator(contor_program_simulator)
                    Case "movaf"
                        contor_program_simulator = contor_program_simulator + 1
                        memorie_simulator(memorie_simulator(contor_program_simulator)) = acumulator_simulator
                        listbox1_redesenare()
                        contor_program_simulator = contor_program_simulator + 1
                    Case "movfa"
                        contor_program_simulator = contor_program_simulator + 1
                        acumulator_simulator = memorie_simulator(memorie_simulator(contor_program_simulator))
                        contor_program_simulator = contor_program_simulator + 1
                        TextBox1.Text = acumulator_simulator
                    Case "jpz"
                        If zf = 1 Then
                            contor_program_simulator = contor_program_simulator + 1
                            contor_program_simulator = memorie_simulator(contor_program_simulator)
                        Else
                            contor_program_simulator = contor_program_simulator + 2
                        End If
                    Case "jpc"
                        If cf = 1 Then
                            contor_program_simulator = contor_program_simulator + 1
                            contor_program_simulator = memorie_simulator(contor_program_simulator)
                        Else
                            contor_program_simulator = contor_program_simulator + 2
                        End If
                    Case "jpnz"
                        If zf = 0 Then
                            contor_program_simulator = contor_program_simulator + 1
                            contor_program_simulator = memorie_simulator(contor_program_simulator)
                        Else
                            contor_program_simulator = contor_program_simulator + 2
                        End If
                    Case "jpnc"
                        If cf = 0 Then
                            contor_program_simulator = contor_program_simulator + 1
                            contor_program_simulator = memorie_simulator(contor_program_simulator)
                        Else
                            contor_program_simulator = contor_program_simulator + 2
                        End If
                    Case "subla"
                        contor_program_simulator = contor_program_simulator + 1
                        acumulator_simulator = acumulator_simulator - memorie_simulator(contor_program_simulator)
                        If acumulator_simulator < 0 Then
                            acumulator_simulator = 256 + acumulator_simulator
                            cf = 1
                        Else : cf = 0
                        End If
                        TextBox17.Text = cf
                        If acumulator_simulator = 0 Then
                            zf = 1
                        Else
                            zf = 0
                        End If
                        TextBox18.Text = zf
                        TextBox1.Text = acumulator_simulator
                        contor_program_simulator = contor_program_simulator + 1
                    Case "addfa"
                        contor_program_simulator = contor_program_simulator + 1
                        acumulator_simulator = acumulator_simulator + memorie_simulator(memorie_simulator(contor_program_simulator))
                        If acumulator_simulator > 255 Then
                            acumulator_simulator = acumulator_simulator - 256
                            cf = 1
                        Else
                            cf = 0
                        End If
                        TextBox17.Text = cf
                        If acumulator_simulator = 0 Then
                            zf = 1
                        Else
                            zf = 0
                        End If
                        TextBox18.Text = zf
                        TextBox1.Text = acumulator_simulator
                        contor_program_simulator = contor_program_simulator + 1
                    Case "subfa"
                        contor_program_simulator = contor_program_simulator + 1
                        acumulator_simulator = acumulator_simulator - memorie_simulator(memorie_simulator(contor_program_simulator))
                        If acumulator_simulator < 0 Then
                            acumulator_simulator = 256 + acumulator_simulator
                            cf = 1
                        Else
                            cf = 0
                        End If
                        TextBox17.Text = cf
                        If acumulator_simulator = 0 Then
                            zf = 1
                        Else
                            zf = 0
                        End If
                        TextBox18.Text = zf
                        TextBox1.Text = acumulator_simulator
                        contor_program_simulator = contor_program_simulator + 1
                    Case "incf"
                        contor_program_simulator = contor_program_simulator + 1
                        memorie_simulator(memorie_simulator(contor_program_simulator)) =
                            memorie_simulator(memorie_simulator(contor_program_simulator)) + 1
                        If memorie_simulator(memorie_simulator(contor_program_simulator)) > 255 Then
                            memorie_simulator(memorie_simulator(contor_program_simulator)) =
                                memorie_simulator(memorie_simulator(contor_program_simulator)) - 256
                            cf = 1
                        Else
                            cf = 0
                        End If
                        TextBox17.Text = cf
                        If memorie_simulator(memorie_simulator(contor_program_simulator)) = 0 Then
                            zf = 1
                        Else
                            zf = 0
                        End If
                        TextBox18.Text = zf
                        listbox1_redesenare()
                        contor_program_simulator = contor_program_simulator + 1
                    Case "decf"
                        contor_program_simulator = contor_program_simulator + 1
                        memorie_simulator(memorie_simulator(contor_program_simulator)) =
                            memorie_simulator(memorie_simulator(contor_program_simulator)) - 1
                        If memorie_simulator(memorie_simulator(contor_program_simulator)) < 0 Then
                            memorie_simulator(memorie_simulator(contor_program_simulator)) =
                                256 + memorie_simulator(memorie_simulator(contor_program_simulator))
                            cf = 1
                        Else
                            cf = 0
                        End If
                        TextBox17.Text = cf
                        If memorie_simulator(memorie_simulator(contor_program_simulator)) = 0 Then
                            zf = 1
                        Else
                            zf = 0
                        End If
                        TextBox18.Text = zf
                        listbox1_redesenare()
                        contor_program_simulator = contor_program_simulator + 1
                    Case "call"
                        memorie_simulator(contor_stiva_simulator) = contor_program_simulator + 2
                        contor_stiva_simulator = contor_stiva_simulator - 1
                        TextBox3.Text = contor_stiva_simulator
                        listbox1_redesenare()
                        contor_program_simulator = contor_program_simulator + 1
                        contor_program_simulator = memorie_simulator(contor_program_simulator)
                    Case "ret"
                        contor_stiva_simulator = contor_stiva_simulator + 1
                        contor_program_simulator = memorie_simulator(contor_stiva_simulator)
                        TextBox3.Text = contor_stiva_simulator
                    Case "callz"
                        If zf = 1 Then
                            memorie_simulator(contor_stiva_simulator) = contor_program_simulator + 2
                            contor_stiva_simulator = contor_stiva_simulator - 1
                            TextBox3.Text = contor_stiva_simulator
                            listbox1_redesenare()
                            contor_program_simulator = contor_program_simulator + 1
                            contor_program_simulator = memorie_simulator(contor_program_simulator)
                        Else
                            contor_program_simulator = contor_program_simulator + 2
                        End If
                    Case "callnz"
                        If zf = 0 Then
                            memorie_simulator(contor_stiva_simulator) = contor_program_simulator + 2
                            contor_stiva_simulator = contor_stiva_simulator - 1
                            TextBox3.Text = contor_stiva_simulator
                            listbox1_redesenare()
                            contor_program_simulator = contor_program_simulator + 1
                            contor_program_simulator = memorie_simulator(contor_program_simulator)
                        Else
                            contor_program_simulator = contor_program_simulator + 2
                        End If
                    Case "callc"
                        If cf = 1 Then
                            memorie_simulator(contor_stiva_simulator) = contor_program_simulator + 2
                            contor_stiva_simulator = contor_stiva_simulator - 1
                            TextBox3.Text = contor_stiva_simulator
                            listbox1_redesenare()
                            contor_program_simulator = contor_program_simulator + 1
                            contor_program_simulator = memorie_simulator(contor_program_simulator)
                        Else
                            contor_program_simulator = contor_program_simulator + 2
                        End If
                    Case "callnc"
                        If cf = 0 Then
                            memorie_simulator(contor_stiva_simulator) = contor_program_simulator + 2
                            contor_stiva_simulator = contor_stiva_simulator - 1
                            TextBox3.Text = contor_stiva_simulator
                            listbox1_redesenare()
                            contor_program_simulator = contor_program_simulator + 1
                            contor_program_simulator = memorie_simulator(contor_program_simulator)
                        Else
                            contor_program_simulator = contor_program_simulator + 2
                        End If
                    Case "andla"
                        contor_program_simulator = contor_program_simulator + 1
                        acumulator_simulator = acumulator_simulator And memorie_simulator(contor_program_simulator)
                        cf = 0
                        TextBox17.Text = cf
                        If acumulator_simulator = 0 Then
                            zf = 1
                        Else
                            zf = 0
                        End If
                        TextBox18.Text = zf
                        TextBox1.Text = acumulator_simulator
                        contor_program_simulator = contor_program_simulator + 1
                    Case "orla"
                        contor_program_simulator = contor_program_simulator + 1
                        acumulator_simulator = acumulator_simulator Or memorie_simulator(contor_program_simulator)
                        cf = 0
                        TextBox17.Text = cf
                        If acumulator_simulator = 0 Then
                            zf = 1
                        Else
                            zf = 0
                        End If
                        TextBox18.Text = zf
                        TextBox1.Text = acumulator_simulator
                        contor_program_simulator = contor_program_simulator + 1
                    Case "andfa"
                        contor_program_simulator = contor_program_simulator + 1
                        acumulator_simulator = acumulator_simulator And memorie_simulator(memorie_simulator(contor_program_simulator))
                        cf = 0
                        TextBox17.Text = cf
                        If acumulator_simulator = 0 Then
                            zf = 1
                        Else
                            zf = 0
                        End If
                        TextBox18.Text = zf
                        TextBox1.Text = acumulator_simulator
                        contor_program_simulator = contor_program_simulator + 1
                    Case "orfa"
                        contor_program_simulator = contor_program_simulator + 1
                        acumulator_simulator = acumulator_simulator Or memorie_simulator(memorie_simulator(contor_program_simulator))
                        cf = 0
                        TextBox17.Text = cf
                        If acumulator_simulator = 0 Then
                            zf = 1
                        Else
                            zf = 0
                        End If
                        TextBox18.Text = zf
                        TextBox1.Text = acumulator_simulator
                        contor_program_simulator = contor_program_simulator + 1
                    Case "xorla"
                        contor_program_simulator = contor_program_simulator + 1
                        acumulator_simulator = acumulator_simulator Xor memorie_simulator(contor_program_simulator)
                        cf = 0
                        TextBox17.Text = cf
                        If acumulator_simulator = 0 Then
                            zf = 1
                        Else
                            zf = 0
                        End If
                        TextBox18.Text = zf
                        TextBox1.Text = acumulator_simulator
                        contor_program_simulator = contor_program_simulator + 1
                    Case "xorfa"
                        contor_program_simulator = contor_program_simulator + 1
                        acumulator_simulator = acumulator_simulator Xor memorie_simulator(memorie_simulator(contor_program_simulator))
                        cf = 0
                        TextBox17.Text = cf
                        If acumulator_simulator = 0 Then
                            zf = 1
                        Else
                            zf = 0
                        End If
                        TextBox18.Text = zf
                        TextBox1.Text = acumulator_simulator
                        contor_program_simulator = contor_program_simulator + 1
                    Case "nega"
                        acumulator_simulator = 255 - acumulator_simulator
                        cf = 0
                        zf = 0
                        TextBox1.Text = acumulator_simulator
                        TextBox17.Text = cf
                        TextBox18.Text = zf
                        contor_program_simulator = contor_program_simulator + 1
                    Case "rlca"
                        acumulator_simulator = acumulator_simulator << 1
                        If acumulator_simulator > 256 Then
                            cft = 1
                            acumulator_simulator = acumulator_simulator Mod 256
                        Else
                            cft = 0
                        End If
                        If cf = 1 Then
                            acumulator_simulator = acumulator_simulator + 1
                        End If
                        cf = cft
                        TextBox17.Text = cf
                        If acumulator_simulator = 0 Then
                            zf = 1
                        Else
                            zf = 0
                        End If
                        TextBox18.Text = zf
                        TextBox1.Text = acumulator_simulator
                        contor_program_simulator = contor_program_simulator + 1
                    Case "rrca"
                        If acumulator_simulator Mod 2 = 1 Then
                            'acumulator_simulator = acumulator_simulator - 256
                            cft = 1
                        Else
                            cft = 0
                        End If
                        acumulator_simulator = (acumulator_simulator >> 1) + (cf * 128)
                        cf = cft
                        TextBox17.Text = cf
                        If acumulator_simulator = 0 Then
                            zf = 1
                        Else
                            zf = 0
                        End If
                        TextBox18.Text = zf
                        TextBox1.Text = acumulator_simulator
                        contor_program_simulator = contor_program_simulator + 1
                    Case "outa"
                        contor_program_simulator = contor_program_simulator + 1
                        numar_port_iesire_simulator = memorie_simulator(contor_program_simulator)
                        If numar_port_iesire_simulator = 255 Then
                            validare_intreruperi_simulator = acumulator_simulator
                            Label22.Text = validare_intreruperi_simulator
                        ElseIf numar_port_iesire_simulator = 5 Then
                            constanta_timer_simulator = acumulator_simulator
                        ElseIf numar_port_iesire_simulator = 6 Then
                            emisie_seriala_simulator = acumulator_simulator
                            TextBox19.Text = emisie_seriala_simulator
                        ElseIf numar_port_iesire_simulator = 7 Then
                            pagina_selectata_transfer = acumulator_simulator
                            Label13.Text = "Pagina: " & pagina_selectata_transfer
                            ListBox3.Items.Clear()
                            For i_contor = 0 To 255
                                ListBox3.Items.Add(i_contor & vbTab & "| " & memorie_comuna_simulator(pagina_selectata_transfer, i_contor))
                            Next
                        End If
                        port_iesire_simulator(numar_port_iesire_simulator) = acumulator_simulator
                        contor_program_simulator = contor_program_simulator + 1
                        sincronizez_porturi_iesire()
                        actualizare_afisare_port()
                    Case "ina"
                        contor_program_simulator = contor_program_simulator + 1
                        numar_port_intrare_simulator = memorie_simulator(contor_program_simulator)
                        'If numar_port_intrare_simulator = 2 Then
                        '    'stare interfata seriala (port receptie) cf <= rx_empty
                        '    port_intrare_simulator(numar_port_intrare_simulator) = _
                        '        buffer_receptie_gol
                        If numar_port_intrare_simulator = 5 Then
                            port_intrare_simulator(numar_port_intrare_simulator) =
                                contor_timer_simulator
                            'timer - valoare contor
                        ElseIf numar_port_intrare_simulator = 6 Then
                            'citire de la interfata seriala
                            port_intrare_simulator(numar_port_intrare_simulator) =
                                receptie_seriala_simulator
                            cf = buffer_receptie_gol
                            TextBox17.Text = cf
                            buffer_receptie_gol = 1
                        ElseIf numar_port_intrare_simulator = 255 Then
                            'validare intrerupere (1 - intrerupere activata, 0 - intrerupere dezactivata)
                            port_intrare_simulator(numar_port_intrare_simulator) =
                                validare_intreruperi_simulator
                        End If
                        acumulator_simulator = port_intrare_simulator(numar_port_intrare_simulator)
                        TextBox1.Text = acumulator_simulator
                        contor_program_simulator = contor_program_simulator + 1
                    Case "page"
                        numar_pagina = memorie_simulator(contor_program_simulator)
                        If numar_pagina <> numar_pagina_simulator Then
                            'aici trebuie sa incarc noua pagina... complicat!
                        End If
                        contor_program_simulator = 0
                    Case "movcaf"
                        contor_program_simulator = contor_program_simulator + 1
                        memorie_comuna_simulator(pagina_selectata_transfer, memorie_simulator(contor_program_simulator)) = acumulator_simulator
                        ListBox3.Items.Clear()
                        For i_contor = 0 To 255
                            ListBox3.Items.Add(i_contor & vbTab & "| " & memorie_comuna_simulator(pagina_selectata_transfer, i_contor))
                        Next
                        contor_program_simulator = contor_program_simulator + 1
                    Case "movcfa"
                        contor_program_simulator = contor_program_simulator + 1
                        acumulator_simulator = memorie_comuna_simulator(pagina_selectata_transfer, memorie_simulator(contor_program_simulator))
                        contor_program_simulator = contor_program_simulator + 1
                        TextBox1.Text = acumulator_simulator
                    Case "reti_0"
                        validare_intreruperi_simulator = validare_intreruperi_simulator Or 1
                        Label22.Text = validare_intreruperi_simulator
                        contor_stiva_simulator = contor_stiva_simulator + 1
                        contor_program_simulator = memorie_simulator(contor_stiva_simulator)
                        TextBox3.Text = contor_stiva_simulator
                    Case "reti_1"
                        validare_intreruperi_simulator = validare_intreruperi_simulator Or 2
                        Label22.Text = validare_intreruperi_simulator
                        contor_stiva_simulator = contor_stiva_simulator + 1
                        contor_program_simulator = memorie_simulator(contor_stiva_simulator)
                        TextBox3.Text = contor_stiva_simulator
                    Case "reti_2"
                        validare_intreruperi_simulator = validare_intreruperi_simulator Or 4
                        Label22.Text = validare_intreruperi_simulator
                        contor_stiva_simulator = contor_stiva_simulator + 1
                        contor_program_simulator = memorie_simulator(contor_stiva_simulator)
                        TextBox3.Text = contor_stiva_simulator
                    Case "reti_3"
                        validare_intreruperi_simulator = validare_intreruperi_simulator Or 8
                        Label22.Text = validare_intreruperi_simulator
                        contor_stiva_simulator = contor_stiva_simulator + 1
                        contor_program_simulator = memorie_simulator(contor_stiva_simulator)
                        TextBox3.Text = contor_stiva_simulator
                    Case "reti_4"
                        validare_intreruperi_simulator = validare_intreruperi_simulator Or 16
                        Label22.Text = validare_intreruperi_simulator
                        contor_stiva_simulator = contor_stiva_simulator + 1
                        contor_program_simulator = memorie_simulator(contor_stiva_simulator)
                        TextBox3.Text = contor_stiva_simulator
                    Case "reti_5"
                        validare_intreruperi_simulator = validare_intreruperi_simulator Or 32
                        Label22.Text = validare_intreruperi_simulator
                        contor_stiva_simulator = contor_stiva_simulator + 1
                        contor_program_simulator = memorie_simulator(contor_stiva_simulator)
                        TextBox3.Text = contor_stiva_simulator
                    Case "reti_6"
                        validare_intreruperi_simulator = validare_intreruperi_simulator Or 64
                        Label22.Text = validare_intreruperi_simulator
                        contor_stiva_simulator = contor_stiva_simulator + 1
                        contor_program_simulator = memorie_simulator(contor_stiva_simulator)
                        TextBox3.Text = contor_stiva_simulator
                    Case "savecz"
                        contor_program_simulator = contor_program_simulator + 1
                        memorie_simulator(memorie_simulator(contor_program_simulator)) = cf * 128 + zf * 64
                        listbox1_redesenare()
                        contor_program_simulator = contor_program_simulator + 1
                    Case "restcz"
                        contor_program_simulator = contor_program_simulator + 1
                        cf = memorie_simulator(memorie_simulator(contor_program_simulator))
                        If cf = 192 Then
                            cf = 1
                            zf = 1
                        ElseIf cf = 128 Then
                            cf = 1
                            zf = 0
                        ElseIf cf = 64 Then
                            cf = 0
                            zf = 1
                        Else
                            cf = 0
                            zf = 0
                        End If
                        TextBox17.Text = cf
                        TextBox18.Text = zf
                        contor_program_simulator = contor_program_simulator + 1
                    Case Else
                End Select
            Else
                If Not executie_continua Then
                    MsgBox("Cod incorect! - Nu poate fi executat!")
                    contor_program_simulator = contor_program_simulator + 1
                End If
            End If
        End If
    End Sub
    Sub executie_simulator()
        contor_program_simulator = listbox1_index
        'Portiunea comentata evita executia codurilor introduse cu directiva db dar
        'solutia nu este normala pentru ca in realitate microprocesorul executa instructiunile
        'in ordine fara exceptie! Deci acest cod nu se va folosi!
        'While (cod_neexecutabil(contor_program_simulator) And (contor_program_simulator < contor_program + 1))
        '    contor_program_simulator = contor_program_simulator + 1
        'End While
        executie(contor_program_simulator)
        If contor_program_simulator = 256 Then contor_program_simulator = 0
        '----------------------------
        'timer
        If ((contor_timer_simulator = 0) And (constanta_timer_simulator <> 0) And
            ((validare_intreruperi_simulator And 1) = 1)) Then
            'la valoarea zero a contorului timer, daca intreruperile sunt activate,
            'se declanseaza intreruperea timer
            CheckBox1.Checked = True
            contor_timer_simulator = constanta_timer_simulator
        End If
        If ((constanta_timer_simulator <> 0) And (contor_timer_simulator > 0)) Then
            'altfel scad constanta timer cite o unitate
            contor_timer_simulator = contor_timer_simulator - 1
        End If
        TextBox21.Text = contor_timer_simulator
        '----------------------------
        'actualizare listbox1 si listbox2 (list cod si lista text)
        'ListBox1 - contine codul program (programul asamblat)
        'ListBox2 - contine listingul programului (fisierul text)
        ListBox1.SelectedIndex = contor_program_simulator 'pozitionez cursorul pe noua pozitie
        TextBox2.Text = contor_program_simulator 'afisez valoarea contorului program
        cautare_linie_text() 'determin pozitia in fereastra listingului
        listbox1_index = contor_program_simulator 'memorez pozitia in fereastra cu codurile programului

    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'selectie tip de executie din butoane radio
        If RadioButton2.Checked Then
            executie_simulator()
        End If
        If RadioButton1.Checked Then
            If Not executie_continua Then
                Button1.Text = "STOP"
                RadioButton2.Visible = False
                RadioButton3.Visible = False
                executie_continua = True
                Timer1.Start()

            Else
                Button1.Text = "GO"
                RadioButton2.Visible = True
                RadioButton3.Visible = True
                executie_continua = False
                Timer1.Stop()
            End If
        End If
    End Sub

    Private Sub ResetToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResetToolStripMenuItem.Click
        initializare_simulator()
    End Sub

    Sub sincronizez_porturi_iesire()
        For i_contor = 0 To 255
            If i_contor = numar_port_iesire_simulator_0 Then
                continut_port_iesire_simulator_0 = port_iesire_simulator(i_contor)
            End If
            If i_contor = numar_port_iesire_simulator_1 Then
                continut_port_iesire_simulator_1 = port_iesire_simulator(i_contor)
            End If
            If i_contor = numar_port_iesire_simulator_2 Then
                continut_port_iesire_simulator_2 = port_iesire_simulator(i_contor)
            End If
        Next
    End Sub

    Sub sincronizez_porturi_intrare()
        For i_contor = 0 To 255
            If i_contor = numar_port_intrare_simulator_0 Then
                continut_port_intrare_simulator_0 = port_intrare_simulator(i_contor)
            End If
            If i_contor = numar_port_intrare_simulator_1 Then
                continut_port_intrare_simulator_1 = port_intrare_simulator(i_contor)
            End If
            If i_contor = numar_port_intrare_simulator_2 Then
                continut_port_intrare_simulator_2 = port_intrare_simulator(i_contor)
            End If
        Next
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        'numar port intrare
        If initializare_simulare Then
            Try
                numar_port_intrare_simulator_0 = TextBox5.Text
                continut_port_intrare_simulator_0 = TextBox6.Text
                port_intrare_simulator(numar_port_intrare_simulator_0) =
                continut_port_intrare_simulator_0
            Catch ex As Exception
                MsgBox("Valoare incorecta!")
                numar_port_intrare_simulator_0 = 0
            End Try
            sincronizez_porturi_intrare()
            actualizare_afisare_port()
            configuratie_schimbata = True
        End If
    End Sub

    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles TextBox6.TextChanged
        'continut port intrare
        If initializare_simulare Then
            Try
                numar_port_intrare_simulator_0 = TextBox5.Text
                continut_port_intrare_simulator_0 = TextBox6.Text
                port_intrare_simulator(numar_port_intrare_simulator_0) =
                continut_port_intrare_simulator_0
            Catch ex As Exception
                MsgBox("Valoare incorecta!")
                continut_port_intrare_simulator_0 = 0
            End Try
            sincronizez_porturi_intrare()
            actualizare_afisare_port()
            configuratie_schimbata = True
        End If
    End Sub

    Private Sub TextBox7_TextChanged(sender As Object, e As EventArgs) Handles TextBox7.TextChanged
        'continut port intrare
        If initializare_simulare Then
            Try
                numar_port_intrare_simulator_1 = TextBox8.Text
                continut_port_intrare_simulator_1 = TextBox7.Text
                port_intrare_simulator(numar_port_intrare_simulator_1) =
                continut_port_intrare_simulator_1
            Catch ex As Exception
                MsgBox("Valoare incorecta!")
                continut_port_intrare_simulator_1 = 0
            End Try
            sincronizez_porturi_intrare()
            actualizare_afisare_port()
            configuratie_schimbata = True
        End If
    End Sub

    Private Sub TextBox8_TextChanged(sender As Object, e As EventArgs) Handles TextBox8.TextChanged
        'numar port intrare
        If initializare_simulare Then
            Try
                numar_port_intrare_simulator_1 = TextBox8.Text
                continut_port_intrare_simulator_1 = TextBox7.Text
                port_intrare_simulator(numar_port_intrare_simulator_1) =
                continut_port_intrare_simulator_1
            Catch ex As Exception
                MsgBox("Valoare incorecta!")
                numar_port_intrare_simulator_1 = 0
            End Try
            sincronizez_porturi_intrare()
            actualizare_afisare_port()
            configuratie_schimbata = True
        End If
    End Sub

    Private Sub TextBox9_TextChanged(sender As Object, e As EventArgs) Handles TextBox9.TextChanged
        'continut port intrare
        If initializare_simulare Then
            Try
                numar_port_intrare_simulator_2 = TextBox10.Text
                continut_port_intrare_simulator_2 = TextBox9.Text
                port_intrare_simulator(numar_port_intrare_simulator_2) =
                continut_port_intrare_simulator_2
            Catch ex As Exception
                MsgBox("Valoare incorecta!")
                continut_port_intrare_simulator_2 = 0
            End Try
            sincronizez_porturi_intrare()
            actualizare_afisare_port()
            configuratie_schimbata = True
        End If
    End Sub

    Private Sub TextBox10_TextChanged(sender As Object, e As EventArgs) Handles TextBox10.TextChanged
        'numar port intrare
        If initializare_simulare Then
            Try
                numar_port_intrare_simulator_2 = TextBox10.Text
                continut_port_intrare_simulator_2 = TextBox9.Text
                port_intrare_simulator(numar_port_intrare_simulator_2) =
                continut_port_intrare_simulator_2
            Catch ex As Exception
                MsgBox("Valoare incorecta!")
                numar_port_intrare_simulator_2 = 0
            End Try
            sincronizez_porturi_intrare()
            actualizare_afisare_port()
            configuratie_schimbata = True
        End If
    End Sub

    Private Sub TextBox12_TextChanged(sender As Object, e As EventArgs) Handles TextBox12.TextChanged
        'continut port iesire
        If initializare_simulare Then
            Try
                numar_port_iesire_simulator_0 = TextBox12.Text
            Catch ex As Exception
                MsgBox("Valoare incorecta!")
                numar_port_iesire_simulator_0 = 0
            End Try
            sincronizez_porturi_iesire()
            actualizare_afisare_port()
            configuratie_schimbata = True
        End If
    End Sub


    Private Sub TextBox14_TextChanged(sender As Object, e As EventArgs) Handles TextBox14.TextChanged
        'continut port iesire
        If initializare_simulare Then
            Try
                numar_port_iesire_simulator_1 = TextBox14.Text
            Catch ex As Exception
                MsgBox("Valoare incorecta!")
                numar_port_iesire_simulator_1 = 0
            End Try
            sincronizez_porturi_iesire()
            actualizare_afisare_port()
            configuratie_schimbata = True
        End If
    End Sub

    Private Sub TextBox16_TextChanged(sender As Object, e As EventArgs) Handles TextBox16.TextChanged
        'continut port iesire
        If initializare_simulare Then
            Try
                numar_port_iesire_simulator_2 = TextBox16.Text
            Catch ex As Exception
                MsgBox("Valoare incorecta!")
                numar_port_iesire_simulator_2 = 0
            End Try
            sincronizez_porturi_iesire()
            actualizare_afisare_port()
            configuratie_schimbata = True
        End If
    End Sub

    Private Sub TextBox11_TextChanged(sender As Object, e As EventArgs) Handles TextBox11.TextChanged
        'output port - portul este stabilit in caseta TextBox12
        If initializare_simulare Then
            sincronizez_porturi_iesire()
            actualizare_afisare_port()
        End If
    End Sub
    Private Sub TextBox13_TextChanged(sender As Object, e As EventArgs) Handles TextBox13.TextChanged
        'output port - portul este stabilit in caseta TextBox14
        If initializare_simulare Then
            sincronizez_porturi_iesire()
            actualizare_afisare_port()
        End If
    End Sub
    Private Sub TextBox15_TextChanged(sender As Object, e As EventArgs) Handles TextBox15.TextChanged
        'output port - portul este stabilit in caseta TextBox16
        If initializare_simulare Then
            sincronizez_porturi_iesire()
            actualizare_afisare_port()
        End If
    End Sub


    Private Sub SaveCFGToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveCFGToolStripMenuItem.Click
        'salvare fisier de configurare simulator
        Dim nume_fisier_cfg As String
        Dim dialog_salvare_fisier As New SaveFileDialog

        'dialog_salvare_fisier.InitialDirectory = "e:\"
        dialog_salvare_fisier.InitialDirectory = ""
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
                configuratie_schimbata = False
            Catch ex As Exception
                MsgBox("Eroare la scriere fisier configuratie!")
            End Try
        End If
    End Sub

    Private Sub LoadCFGToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadCFGToolStripMenuItem.Click
        'incarcare fisier de configurare simulator
        Dim nume_fisier_cfg As String
        Dim dialog_incarcare_fisier As New OpenFileDialog

        initializare_simulare = False
        'dialog_incarcare_fisier.InitialDirectory = "e:\"
        dialog_incarcare_fisier.InitialDirectory = ""
        dialog_incarcare_fisier.Filter = "cfg files (*.cfg)|*.cfg|All files (*.*)|*.*"
        dialog_incarcare_fisier.FilterIndex = 1
        dialog_incarcare_fisier.RestoreDirectory = True

        If dialog_incarcare_fisier.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            Try
                nume_fisier_cfg = dialog_incarcare_fisier.FileName
                'deschid fisier de configurare
                Using sr As New StreamReader(nume_fisier_cfg)
                    While Not sr.EndOfStream
                        numar_port_intrare_simulator_0 = sr.ReadLine
                        continut_port_intrare_simulator_0 = sr.ReadLine
                        continut_port_intrare_simulator_1 = sr.ReadLine
                        numar_port_intrare_simulator_1 = sr.ReadLine
                        continut_port_intrare_simulator_2 = sr.ReadLine
                        numar_port_intrare_simulator_2 = sr.ReadLine
                        numar_port_iesire_simulator_0 = sr.ReadLine
                        numar_port_iesire_simulator_1 = sr.ReadLine
                        numar_port_iesire_simulator_2 = sr.ReadLine
                        ' actualizez si vectorul porturilor de intrare
                        For i_contor = 0 To 255
                            If i_contor = numar_port_intrare_simulator_0 Then
                                port_intrare_simulator(i_contor) = continut_port_intrare_simulator_0
                            End If
                            If i_contor = numar_port_intrare_simulator_1 Then
                                port_intrare_simulator(i_contor) = continut_port_intrare_simulator_1
                            End If
                            If i_contor = numar_port_intrare_simulator_2 Then
                                port_intrare_simulator(i_contor) = continut_port_intrare_simulator_2
                            End If
                        Next
                        actualizare_afisare_port()
                        initializare_simulare = True
                    End While
                End Using
            Catch ex As Exception
                MsgBox("Eroare la citire fisier configuratie!")
            End Try
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If executie_continua Then
            executie_simulator()
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            intrerupere_simulator = intrerupere_simulator Or 1 'activez intreruperea zero
        Else
            intrerupere_simulator = intrerupere_simulator And 254 'sterg intreruperea zero dar le las
            'pe celelelte neschimbate
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked Then
            intrerupere_simulator = intrerupere_simulator Or 2
        Else
            intrerupere_simulator = intrerupere_simulator And 253
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked Then
            intrerupere_simulator = intrerupere_simulator Or 4
        Else
            intrerupere_simulator = intrerupere_simulator And 251
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked Then
            intrerupere_simulator = intrerupere_simulator Or 8
        Else
            intrerupere_simulator = intrerupere_simulator And 247
        End If
    End Sub

    Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        If CheckBox5.Checked Then
            intrerupere_simulator = intrerupere_simulator Or 16
        Else
            intrerupere_simulator = intrerupere_simulator And 239
        End If
    End Sub

    Private Sub CheckBox6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox6.CheckedChanged
        If CheckBox6.Checked Then
            intrerupere_simulator = intrerupere_simulator Or 32
        Else
            intrerupere_simulator = intrerupere_simulator And 223
        End If
    End Sub

    Private Sub CheckBox7_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox7.CheckedChanged
        If CheckBox7.Checked Then
            intrerupere_simulator = intrerupere_simulator Or 64
        Else
            intrerupere_simulator = intrerupere_simulator And 191
        End If
    End Sub

    Private Sub TextBox20_TextChanged(sender As Object, e As EventArgs) Handles TextBox20.TextChanged
        'caseta RXD
        If initializare_simulare Then
            Try
                If (TextBox20.Text > 255) Then
                    MsgBox("Valoare eronata !")
                    TextBox20.Text = 0
                Else
                    receptie_seriala_simulator = TextBox20.Text
                    CheckBox2.Checked = True
                    buffer_receptie_gol = 0
                End If
            Catch ex As Exception
                MsgBox("Valoare eronata !")
                TextBox20.Text = 0
            End Try
        End If
    End Sub
End Class