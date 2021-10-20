Imports System
Imports System.IO
Module Module1
    'aici voi pune asamblorul

    Public Const dimensiune_pagina = 256
    Public Const numar_maxim_linii_text = 500 'numarul maxim de linii a fisierului text de asamblat
    'folosit la simulare pentru corespondenta liniei de text cu linia de cod
    Public Const numar_pagini_memorie = 32 'numarul de pagini de 256 octeti ale memoriei
    'acest program este facut pentru o memorie de 32 pagini = 8k si din acest motiv
    'deocamdata nu folosesc aceasta constanta !!!!!!!!!

    Public nume_fisier As String = "" 'numele fisierului de asamblat
    Public nume_asamblat As String 'numele fisierului asamblat
    Public nume_listing As String 'numele fisierului listing
    Public nume_include_creat As String 'numele fisierului include care se creaza aici
    'Exista doua fisiere include aici:
    '- un fisier care se creaza acum cu toate simbolurile din programul respectiv
    '- un fisier include care se citeste pentru a adauga simbolurile din alte programe (pagini) cu care
    'respectivul program lucreaza
    '----------------------------
    'constructie memorie de 8k = 32 pagini
    Public numar_pagina As Byte = 0 'numarul paginii in care se aseaza programul
    Public constructie_memorie As Boolean = False 'semnaleaza actiunea de constructie a memoriei de 8k
    Public memorie_8k(32, dimensiune_pagina) As String 'memoria de 8k
    Public structura_memorie(32) As String 'numele fisierului din pagina
    Public pagini_introduse(32) As Boolean 'paginile folosite din memore
    Public asamblare_corecta As Boolean = True 'nu sunt erori la asamblare
    '--------------------------------
    'simulator
    Public corespondenta_text_cod(numar_maxim_linii_text) As Integer
    'corespondenta dintre liniile din fisierul text cu liniile din numarul codurilor asamblate
    'folosit la simulator !!!!!
    'Public cod_neexecutabil(numar_maxim_linii_text) As Boolean
    'liniile programului scris in limbaj de asamblare au mnemonici care genereaza cod
    'executabil si directive: equ care atribuie valori numerice etichetelor si db care
    'rezerva in memoria program anumite valori sau siruri de valori. Valorile generate de
    'directiva db nu sunt executabile si trebuie semnalate aici pentru ca simulatorul
    'sa nu incerce sa le execute !!!! True = cod neexecutabil (generat de directiva db
    'False cod executabil
    'PINA LA URMA NU MAI FOLOSESC ASTA (COD NEEXECUTABIL) PENTRU CA IN REALITATE MICROPROCESORUL
    'EXECUTA CODUL DIN MEMORIE SUCCESIV FARA EXCEPTIE! dECI PENTRU A PASTRA CORESPONDENTA CU 
    'REALITATEA NU SE FOLOSESTE! O PASREZ DOAR PENTRU EVENTUALE "EFECTE SPECIALE"

    Public numar_port_intrare_simulator_0 As Byte
    Public numar_port_intrare_simulator_1 As Byte
    Public numar_port_intrare_simulator_2 As Byte
    Public numar_port_iesire_simulator_0 As Byte
    Public numar_port_iesire_simulator_1 As Byte
    Public numar_port_iesire_simulator_2 As Byte
    Public continut_port_intrare_simulator_0 As Byte
    Public continut_port_intrare_simulator_1 As Byte
    Public continut_port_intrare_simulator_2 As Byte

    '--------------------------------
    Dim i_contor As Integer 'contor cu scopuri diverse
    Dim linie_citita As String 'o linie citita din fisierul de asamblat
    Dim linie_citita_include As String 'o linie citita din fisierul de inclus
    Dim pozitie As Integer 'pozitia in linie a diverselor caractere
    Dim linia_de_analizat As String 'portiunea din linie cu cod - fara comentarii
    Public contor_program As Integer 'contorul de program
    Public contor_linie As Integer 'contorul de linii citite din fisier
    Public contor_etichete As Integer 'contorul etichetelor gasite
    Dim operand As Boolean = False 'prima este comanda
    'Dim eticheta_dubla As Boolean = False 'detectez etichetele multiple
    Public temp_asamblat(dimensiune_pagina) As String 'fisierul asamblat pastrat temporar
    Public temp_nume_etichete(dimensiune_pagina) As String 'vectorul cu numele etichetelor gasite
    Public temp_valoare_etichete(dimensiune_pagina) As Integer 'valorile etichetelor gasite
    Public temp_etichete_externe(dimensiune_pagina) As Boolean 'vector cu tipul etichetei
    'True - semnaleaza o eticheta externa sau una predefinita
    'False - semnaleaza o eticheta definita in program
    '--------------------------------------
    Dim sir_ascii As String 'sirul ascii gasit pe linia respectiva
    Dim db_sir As Boolean = False 'semnaleaza daca operandul directivei db este
    'un sir db_sir = True sau un numar db_sir = false
    'Pina la urma semnificatia acestei variabile este: True indica faptul ca
    's-a gasit un sir ascii de tipul: 'sir sau 'sir' si acesta poate fi
    'folosit intr-un sir db sau o mnemonica cu operand (de exemplu: movla)
    Dim equ_val As Boolean = False 'semnaleaza ca s-a intalnit o directiva equ
    'si ca operandul care urmeaza este valoarea etichetei:
    'equ_val = True -> operandul este valoarea etichetei
    'equ_val = False -> operandul NU este valoarea etichetei
    Dim eticheta_temp As String 'se memoreaza aici eticheta pina decid daca e
    'eticheta sau directiva EQU
    Dim eticheta_in_asteptare As Boolean = False 'semnaleaza atunci cind este True ca este o eticheta
    'memorata in eticheta_temp si trebuie folosita
    Dim pozitie_eticheta As Integer 'pozitia in care a fost gasita eticheta eticheta_in_asteptare
    'vectorul cu memonicile limbajului DAN
    Public mnemonici() As String = {"nop", "movla", "addla", "jmp", "movaf", "movfa", "jpz",
                     "jpc", "jpnz", "jpnc", "subla", "addfa", "subfa", "incf",
                    "decf", "call", "ret", "callz", "callnz", "callc", "callnc", "andla",
                    "orla", "andfa", "orfa", "xorla", "xorfa", "nega", "rlca",
                    "rrca", "outa", "ina", "page", "movcaf", "movcfa",
                    "ai_0", "ai_1", "ai_2", "ai_3", "ai_4", "ai_5", "ai_6",
                    "reti_0", "reti_1", "reti_2", "reti_3", "reti_4", "reti_5", "reti_6",
                    "savecz", "restcz", "sfirsit"}
    '-------------------------

    'pozitia mnemonicilor de mai sus care au operand (numaratoarea de la zero)
    Dim cu_operand() As Integer = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
                                  17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 30, 31, 32,
                                   33, 34, 49, 50}
    '---------------------------------------------
    'din determin_etichete
    Dim numar_elemente_expresie As Integer 'numarul elementelor dintr-o expresie operand
    Dim element As String 'elementul dintr-o expresie operand
    Dim rezultat As Integer 'rezultatul calculului unei expresii
    Dim contor1 As Integer 'contor cu scopuri diverse
    '---------------------------------------------

    Private Sub eticheta_fara_operatie()
        'Un operand fara expresie matematica = numai eticheta
        'If temp_asamblat(i_contor)(0) > "@" Then
        'MsgBox(temp_asamblat(i_contor) & " = nu are operatie")
        While (temp_asamblat(i_contor) <> temp_nume_etichete(contor1)) And (contor1 < contor_etichete)
            contor1 = contor1 + 1
        End While
        If temp_asamblat(i_contor) = temp_nume_etichete(contor1) Then
            'MsgBox("Am gasit eticheta ! " & temp_asamblat(i_contor) _
            '      & " si " & temp_nume_etichete(contor1))
            'MsgBox("Inocuiesc " & temp_asamblat(i_contor) & " cu " & Str(temp_valoare_etichete(contor1)))
            temp_asamblat(i_contor) = CStr(temp_valoare_etichete(contor1))
        Else
            'MsgBox("NU am gasit eticheta !!!!!! " & temp_asamblat(i_contor) _
            '     & " si " & temp_nume_etichete(contor1))
            Form1.RichTextBox2.Text = Form1.RichTextBox2.Text & "NU am gasit eticheta !!!!!! " _
                & temp_asamblat(i_contor) & vbCrLf
            asamblare_corecta = False
        End If
        'End If
    End Sub
    Private Sub eticheta_cu_operatie_plus()
        'Un operand cu o suma
        'MsgBox(temp_asamblat(i_contor) & " = are operatie")

        Dim contor2 As Byte = 0
        Dim termen(2) As String
        Dim elemente_componente As String() = temp_asamblat(i_contor).Split(New [Char]() {"+"c})
        For Each s As String In elemente_componente
            If s.Trim() <> "" Then
                'MsgBox("Element: " & s)
                termen(contor2) = s
                contor2 = contor2 + 1
            End If
        Next s
        'MsgBox("Termen1= " & termen(0) & vbCrLf & "Termen2= " & termen(1))

        While (termen(0) <> temp_nume_etichete(contor1)) And (contor1 < contor_etichete)
            contor1 = contor1 + 1
        End While
        If termen(0) = temp_nume_etichete(contor1) Then
            rezultat = temp_valoare_etichete(contor1) + Val(termen(1))
            temp_asamblat(i_contor) = CStr(rezultat)
            If rezultat > 255 Then
                rezultat = 255
                Form1.RichTextBox2.Text = Form1.RichTextBox2.Text &
                        "Operatia cu eticheta: " & termen(0) & "+" & termen(1) _
                        & " = Valoare calculata operand peste 255 !" _
                                                    & vbCrLf
                'scrierea in fisier listing nu o fac deocamdata din cauza ca fisierul
                'listing nu este vizibil aici - poate modific mai tirziu daca e nevoie
                'If Not constructie_memorie Then
                '    fisier_listing.Write("Operatia cu eticheta: " & termen(0) & "-" & termen(1) _
                '        & " = Valoare calculata operand peste 255 !" _
                '        & vbCrLf)
                'End If
                asamblare_corecta = False
            End If
            temp_asamblat(i_contor) = CStr(rezultat)
        Else
            Form1.RichTextBox2.Text = Form1.RichTextBox2.Text & "NU am gasit eticheta !!!!!! " _
                & temp_asamblat(i_contor) & vbCrLf
            asamblare_corecta = False
        End If
    End Sub

    Private Sub eticheta_cu_operatie_minus()
        'Un operand cu o diferenta
        'MsgBox(temp_asamblat(i_contor) & " = are operatie")

        Dim contor2 As Byte = 0
        Dim termen(2) As String
        Dim elemente_componente As String() = temp_asamblat(i_contor).Split(New [Char]() {"-"c})
        For Each s As String In elemente_componente
            If s.Trim() <> "" Then
                'MsgBox("Element: " & s)
                termen(contor2) = s
                contor2 = contor2 + 1
            End If
        Next s
        'MsgBox("Termen1= " & termen(0) & vbCrLf & "Termen2= " & termen(1))

        While (termen(0) <> temp_nume_etichete(contor1)) And (contor1 < contor_etichete)
            contor1 = contor1 + 1
        End While
        If termen(0) = temp_nume_etichete(contor1) Then
            rezultat = temp_valoare_etichete(contor1) - Val(termen(1))
            If rezultat < 0 Then
                rezultat = 0
                Form1.RichTextBox2.Text = Form1.RichTextBox2.Text &
                        "Operatia cu eticheta: " & termen(0) & "-" & termen(1) _
                        & " = Valoare calculata operand negativa !" _
                                                    & vbCrLf
                'scrierea in fisier listing nu o fac deocamdata din cauza ca fisierul
                'listing nu este vizibil aici - poate modific mai tirziu daca e nevoie
                'If Not constructie_memorie Then
                '    fisier_listing.Write("Operatia cu eticheta: " & termen(0) & "-" & termen(1) _
                '        & " = Valoare calculata operand negativa !" _
                '        & vbCrLf)
                'End If
                asamblare_corecta = False
            End If
            temp_asamblat(i_contor) = CStr(rezultat)
        Else
            Form1.RichTextBox2.Text = Form1.RichTextBox2.Text & "NU am gasit eticheta !!!!!! " _
                & temp_asamblat(i_contor) & vbCrLf
            asamblare_corecta = False
        End If
    End Sub

    Private Sub determin_etichete()
        'Determin valorile operanzilor sub forma de etichete si care
        'pot avea operatii de adunare si scadere
        'a doua trecere prin fisierul (temporar) asamblat
        'MsgBox("Acum determin valorile etichetelor - a doua trecere")
        For Module1.i_contor = 0 To contor_program - 1 'baleiez programul asamblat sa caut operanzii
            'si sa le atribui valori

            pozitie = temp_asamblat(i_contor).IndexOf("'") 'caractere ascii intre simbolul '
            'If pozitie <> -1 Then
            '    'este caracter ascii'
            '    eticheta_cu_caracter_ascii() 'operand caracter ascii

            If temp_asamblat(i_contor)(0) > "@" Then 'iau in considerare doar textul nu si numerele de cod
                contor1 = 0
                pozitie = temp_asamblat(i_contor).IndexOf("+")
                If pozitie = -1 Then
                    'nu este adunare
                    pozitie = temp_asamblat(i_contor).IndexOf("-")
                    If pozitie = -1 Then
                        'nu este scadere
                        eticheta_fara_operatie() 'operand simplu eticheta
                    Else
                        eticheta_cu_operatie_minus() 'operand cu scadere
                    End If
                Else
                    eticheta_cu_operatie_plus() 'operand cu adunare
                End If
            End If
        Next i_contor
    End Sub

    Private Sub nume_predefinite()
        'Numele predefinite la asamblarea pmogramului
        'iesiri
        temp_nume_etichete(contor_etichete) = "led"
        temp_valoare_etichete(contor_etichete) = 0
        temp_etichete_externe(contor_etichete) = True
        contor_etichete = contor_etichete + 1
        temp_nume_etichete(contor_etichete) = "sseg_m"
        temp_valoare_etichete(contor_etichete) = 1
        temp_etichete_externe(contor_etichete) = True
        contor_etichete = contor_etichete + 1
        temp_nume_etichete(contor_etichete) = "sseg_tm"
        temp_valoare_etichete(contor_etichete) = 2
        temp_etichete_externe(contor_etichete) = True
        contor_etichete = contor_etichete + 1
        temp_nume_etichete(contor_etichete) = "sseg_h"
        temp_valoare_etichete(contor_etichete) = 3
        temp_etichete_externe(contor_etichete) = True
        contor_etichete = contor_etichete + 1
        temp_nume_etichete(contor_etichete) = "sseg_th"
        temp_valoare_etichete(contor_etichete) = 4
        temp_etichete_externe(contor_etichete) = True
        contor_etichete = contor_etichete + 1
        temp_nume_etichete(contor_etichete) = "timer"
        temp_valoare_etichete(contor_etichete) = 5
        temp_etichete_externe(contor_etichete) = True
        contor_etichete = contor_etichete + 1
        temp_nume_etichete(contor_etichete) = "uart"
        temp_valoare_etichete(contor_etichete) = 6
        temp_etichete_externe(contor_etichete) = True
        contor_etichete = contor_etichete + 1
        temp_nume_etichete(contor_etichete) = "page"
        temp_valoare_etichete(contor_etichete) = 7
        temp_etichete_externe(contor_etichete) = True
        contor_etichete = contor_etichete + 1
        temp_nume_etichete(contor_etichete) = "int_act"
        temp_valoare_etichete(contor_etichete) = 255
        temp_etichete_externe(contor_etichete) = True
        contor_etichete = contor_etichete + 1
        'intrari
        temp_nume_etichete(contor_etichete) = "btn"
        temp_valoare_etichete(contor_etichete) = 0
        temp_etichete_externe(contor_etichete) = True
        contor_etichete = contor_etichete + 1
        temp_nume_etichete(contor_etichete) = "sw"
        temp_valoare_etichete(contor_etichete) = 1
        temp_etichete_externe(contor_etichete) = True
        contor_etichete = contor_etichete + 1
    End Sub

    Private Sub initializare()
        'nume_fisier = ""
        contor_program = 0 'contorul de program
        contor_linie = 0 'contorul de linii citite din fisier
        contor_etichete = 0 'contorul etichetelor gasite
        operand = False 'prima este comanda
        'eticheta_dubla = False 'detectez etichetele multiple
        db_sir = False 'semnaleaza daca operandul directivei db este
        'un sir db_sir = True sau un numar db_sir = false
        equ_val = False 'semnaleaza ca s-a intalnit o directiva equ
        'si ca operandul care urmeaza este valoarea etichetei:
        'equ_val = True -> operandul este valoarea etichetei
        'equ_val = False -> operandul NU este valoarea etichetei
        eticheta_in_asteptare = False 'semnaleaza atunci cind este True ca este o eticheta
        'memorata in eticheta_temp si trebuie folosita
        asamblare_corecta = True
    End Sub

    Private Sub verific_eticheta_dubla(eticheta As String, valoare As String,
                            tip_eticheta As Boolean, fisier As StreamWriter)
        'se compara numele eticheta cu continutul vectorului temp_nume_etichete si:
        '- daca eticheta nu exista se scrie in vectorul temp_nume_etichete si temp_valoare_etichete
        '- daca eticheta exista se compara valoare cu temp_valoare_etichete (pentru eticheta gasita)
        '   * daca valoarea difera semnalez ca eticheta e multipla
        '   * daca valorile sunt egale, trec mai departe fara sa fac nimic
        'eticheta_dubla = False
        i_contor = 0
        While ((eticheta <> temp_nume_etichete(i_contor)) And (i_contor < contor_etichete))
            i_contor = i_contor + 1
        End While
        If i_contor < contor_etichete Then 'am gasit o eticheta in vector
            'eticheta_dubla = True
            'testez daca valorile difera
            If temp_valoare_etichete(i_contor) <> Val(valoare) Then
                'valoarea difera pentru aceeasi eticheta si semnalez eroarea
                Form1.RichTextBox2.Text = Form1.RichTextBox2.Text &
                        "Linia: " & contor_linie & " : " & linie_citita _
                        & " eticheta: " & eticheta & " = Eticheta multipla !" _
                                                       & vbCrLf
                If Not constructie_memorie Then
                    fisier.Write("Linia: " & contor_linie & " : " & linie_citita _
                        & " eticheta: " & eticheta & " = Eticheta multipla !" & vbCrLf)
                End If
                asamblare_corecta = False
            End If
        Else 'eticheta nu a fost gasita si deci o scriu in vector
            temp_nume_etichete(contor_etichete) = eticheta
            temp_valoare_etichete(contor_etichete) = Val(valoare)
            temp_etichete_externe(contor_etichete) = tip_eticheta
            contor_etichete = contor_etichete + 1
            If Not constructie_memorie Then
                fisier.Write(linie_citita)
            End If
        End If
    End Sub

    Public Sub asamblare()
        'asamblarea fisierului sursa - prima trecere

        If nume_fisier <> "" Then
            'initializez vectorul de corespondenta linie text cu cod generat
            For Module1.i_contor = 0 To numar_maxim_linii_text - 1
                corespondenta_text_cod(i_contor) = 0
                'cod_neexecutabil(i_contor) = True 'toate pozitiile sunt neexecutabile
                'le fac executabile (False) pe cele care contin cod - NU FOLOSESC DEOCAMDATA...
            Next

            'stabilesc numele fisierului asamblat
            Dim cale_fisier_asamblat As String = Replace(nume_fisier, ".txt", "_ini.coe")
            'stabilesc numele fisierului listing
            Dim cale_fisier_listing As String = Replace(nume_fisier, "txt", "lst")
            'stabilesc numele fisierului include
            Dim cale_fisier_include_creat As String = Replace(nume_fisier, "txt", "inc")

            nume_asamblat = cale_fisier_asamblat 'fac numele global
            nume_listing = cale_fisier_listing 'fac numele global
            nume_include_creat = cale_fisier_include_creat 'fac numele global

            If Not constructie_memorie Then
                'daca fisierul exista, il sterg
                Try
                    My.Computer.FileSystem.DeleteFile(cale_fisier_asamblat)
                Catch ex As Exception
                End Try

                If My.Computer.FileSystem.FileExists(cale_fisier_listing) Then
                    My.Computer.FileSystem.DeleteFile(cale_fisier_listing)
                End If

                If My.Computer.FileSystem.FileExists(cale_fisier_include_creat) Then
                    My.Computer.FileSystem.DeleteFile(cale_fisier_include_creat)
                End If

            End If

            'deschid fisierul asamblat
            Dim fisier_asamblat = My.Computer.FileSystem.OpenTextFileWriter(cale_fisier_asamblat, True)
            Form1.Label2.Text = "File assembled: " & cale_fisier_asamblat

            'deschid fisier listing
            Dim fisier_listing = My.Computer.FileSystem.OpenTextFileWriter(cale_fisier_listing, True)

            'deschid fisier include
            Dim fisier_include = My.Computer.FileSystem.OpenTextFileWriter(cale_fisier_include_creat, True)
            'NOTA: fisierul include creat aici va fi folosit la asamblarea altor fisiere sursa. Acest fisier
            'nu poate fi inclus in fisierul cu care se lucreaza - se obtine un mesaj de eroare pentru ca
            'fisierul include nu poate fi folosit recursiv. Fisierul include creat se poate folosi doar in
            'celelalte fisiere de asamblat - furnizeaza etichetele din alta pagina faca se folosesc mai multe pagini.

            Try
                'incep asamblarea
                Using sr As New StreamReader(nume_fisier)

                    Form1.RichTextBox2.Clear() 'sterg fereastra cu mesaje
                    contor_program = 0 'initializari
                    contor_linie = 0
                    contor_etichete = 0
                    asamblare_corecta = True
                    eticheta_in_asteptare = False 'semnaleaza atunci cind este True ca este o eticheta
                    'memorata in eticheta_temp si trebuie folosita
                    For i_contor As Integer = 0 To dimensiune_pagina
                        temp_nume_etichete(i_contor) = ""
                        temp_valoare_etichete(i_contor) = 0
                    Next i_contor
                    nume_predefinite() 'pun la inceput etichetele predefinite (ma pregatesc pentru include)

                    If Not constructie_memorie Then
                        'Primele linii din fisierul asamblat corespunzatoare
                        'fisierului de initializare memorie Xilinx
                        'MsgBox(nume_fisier)
                        'MsgBox(cale_fisier_asamblat)
                        fisier_asamblat.WriteLine("memory_initialization_radix=10;")
                        fisier_asamblat.WriteLine("memory_initialization_vector=")

                        fisier_listing.WriteLine("Listing fisier:")
                        fisier_listing.WriteLine(nume_fisier)
                        fisier_listing.WriteLine("==============================")
                        fisier_listing.WriteLine()
                    End If

                    While Not sr.EndOfStream 'cit timp fisierul nu este gata

                        'inainte de a citi o linie verific daca sunt etichete nerezolvate
                        If eticheta_in_asteptare Then
                            verific_eticheta_dubla(eticheta_temp, Str(pozitie_eticheta), False, fisier_listing)
                            'eticheta interna - eticheta programului
                            eticheta_in_asteptare = False
                        End If

                        linie_citita = sr.ReadLine() 'citesc cite o linie
                        corespondenta_text_cod(contor_linie) = contor_program 'corespondenta pentru fiecare linie
                        contor_linie = contor_linie + 1
                        'MsgBox(linie_citita)
                        pozitie = linie_citita.IndexOf(";") 'caut comentariul pe linia respectiva

                        'SE ELIMINA COMENTARIILE
                        If pozitie = 0 Then
                            'MsgBox("Linie numai de comentariu")
                            linia_de_analizat = ""
                            If Not constructie_memorie Then
                                fisier_listing.WriteLine(linie_citita)
                            End If
                        ElseIf pozitie = -1 Then
                            'MsgBox("Nu exista comentariu")
                            linia_de_analizat = linie_citita
                            'MsgBox("Linia de analizat:" & linia_de_analizat)
                        ElseIf pozitie > 0 Then
                            'MsgBox("Date si comentariu")
                            'extrag din linie doar partea cu cod si operanzi
                            linia_de_analizat = linie_citita.Substring(0, pozitie - 1)
                            'MsgBox("Linia de analizat:" & linia_de_analizat)
                        End If

                        'SE DETERMINA EXISTENTA SIRURILOR
                        'verific daca este un sir de caractere de genul
                        'acesta 'Acesta este un sir' sau 'Acesta este un sir 
                        db_sir = 0
                        pozitie = linia_de_analizat.IndexOf("'") 'caut inceputul sirului
                        If pozitie <> -1 Then
                            If linia_de_analizat(linia_de_analizat.Length - 1) = "'" Then
                                'este ghilimea la sfirsit
                                sir_ascii = linia_de_analizat.Substring(pozitie + 1, linia_de_analizat.Length - pozitie - 2)
                            Else
                                'nu sete ghilimea la sfirsit
                                sir_ascii = linia_de_analizat.Substring(pozitie + 1, linia_de_analizat.Length - pozitie - 1)
                            End If
                            linia_de_analizat = linia_de_analizat.Substring(0, pozitie - 1)
                            db_sir = True
                        End If

                        '-------------------- analiza liniei -------------
                        If linia_de_analizat <> "" Then
                            'am o linie cu cod si operanzi (am eliminat mai sus comentariile) si separ elementele acesteia
                            'elementele liniei pot fi etichete, mnemonici (coduri) si operanzi
                            'Dim elemente_componente As String() = linia_de_analizat.Split(New [Char]() {" "c, ","c, "."c, ":"c, CChar(vbTab)})
                            Dim elemente_componente As String() = linia_de_analizat.Split(New [Char]() {" "c, ":"c, CChar(vbTab)}) 'separatori folositi la analiza textului de asamblat
                            'analizez fiecare element gasit pe linia respectiva pentru a determina tipul liniei
                            '- linie numai cu eticheta
                            '- linie numai cu cod si operanzi (eventual)
                            '- se pare ca merge si linie cu eticheta, cod si operanzi
                            For Each s As String In elemente_componente
                                If s.Trim() <> "" Then
                                    'MsgBox("Element: " & s)
                                    pozitie = linie_citita.IndexOf(s)
                                    If pozitie = 0 Then
                                        'am gasit ceva pe pozitia zero deci e eticheta sau direvtiva include
                                        'include

                                        'DIRECTIVA INCLUDE
                                        'verific daca este directiva include
                                        If s(0) = "#" Then
                                            'este directiva include si caut fisierul
                                            'MsgBox(s.Substring(0, 8))
                                            If s.Substring(0, 9) = "#include<" Then
                                                'MsgBox("Directiva Include")
                                                pozitie = s.IndexOf("<")
                                                'fisierul include de aici NU este fisierul curent care se creaza ci fisierul
                                                'care se adauga la fisierul de asamblat!
                                                Dim nume_include As String = s.Substring(pozitie + 1, s.Length - pozitie - 2)
                                                Dim nume_include_path As String = Path.GetFullPath(nume_include)

                                                'MsgBox(nume_include_path)
                                                'If My.Computer.FileSystem.FileExists(nume_include_path) Then
                                                'MsgBox("Deschid fisier direct!" & nume_include)
                                                'Else

                                                'caut fisier

                                                '================================
                                                Dim dialogDeschidFisierInc As New OpenFileDialog()
                                                Dim MyStream As Stream = Nothing
                                                'Dim FisierCititInc As String 
                                                Dim nume_fisier_inc As String = ""

                                                dialogDeschidFisierInc.Title = "INCLUDE FILE"
                                                dialogDeschidFisierInc.InitialDirectory = nume_include_path
                                                dialogDeschidFisierInc.FileName = nume_include
                                                dialogDeschidFisierInc.Filter = "include files (*.inc)|*.inc|All files (*.*)|*.*"
                                                dialogDeschidFisierInc.FilterIndex = 1
                                                dialogDeschidFisierInc.RestoreDirectory = True

                                                If dialogDeschidFisierInc.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                                                    Try
                                                        MyStream = dialogDeschidFisierInc.OpenFile
                                                        If (MyStream IsNot Nothing) Then
                                                            nume_fisier_inc = dialogDeschidFisierInc.FileName
                                                            'FisierCititInc = My.Computer.FileSystem.ReadAllText(nume_fisier_inc)
                                                        End If
                                                        'MsgBox(FisierCititInc)
                                                        Using sr_include As New StreamReader(nume_fisier_inc)
                                                            While Not sr_include.EndOfStream 'cit timp fisierul nu este gata
                                                                linie_citita_include = sr_include.ReadLine() 'citesc cite o linie din fisier include
                                                                'MsgBox(linie_citita_include)

                                                                'linie_citita_include este: eticheta = valoare
                                                                'se cauta daca eticheta se repeta, comparind cu vectorul temp_nume_eticheta
                                                                'daca eticheta se repeta se verifica daca are aceeasi valoare cu cea
                                                                'din vectorul temp_valoare_etichete. Daca valoarea esta aceeasi atunci se trece mai departe.
                                                                'Daca valoarea este diferita se semnaleaza acest lucru.
                                                                'Daca eticheta nu mai este gasita se scrie in vectorii:
                                                                'temp_nume_etichete si temp_valoare_etichete
                                                                pozitie = linie_citita_include.IndexOf("=")
                                                                If ((pozitie = -1) Or (pozitie = 0) Or (pozitie = 1)) Then
                                                                    Form1.RichTextBox2.Text = Form1.RichTextBox2.Text &
                                                                    "Linia: " & linie_citita_include & " -> din fisierul de inclus: " & nume_fisier_inc _
                                                                    & " = Este eronata !" _
                                                                    & vbCrLf
                                                                    If Not constructie_memorie Then
                                                                        fisier_listing.Write("Linia: " & linie_citita_include & " -> din fisierul de inclus: " & nume_fisier_inc _
                                                                        & " = Este eronata !" & vbCrLf)
                                                                    End If
                                                                    asamblare_corecta = False
                                                                Else
                                                                    Dim eticheta_include As String = linie_citita_include.Substring(0, pozitie - 1)
                                                                    Dim valoare_eticheta_include As String = linie_citita_include.Substring(pozitie + 1, linie_citita_include.Length - pozitie - 1)
                                                                    'MsgBox(eticheta_include)
                                                                    'MsgBox(valoare_eticheta_include)
                                                                    'verific daca eticheta se gaseste in temp_eticheta_nume
                                                                    verific_eticheta_dubla(eticheta_include, valoare_eticheta_include, True, fisier_listing)
                                                                    'eticheta externa - din fisierul Include

                                                                    'If Not eticheta_dubla Then
                                                                    '    temp_nume_etichete(contor_etichete) = eticheta_include
                                                                    '    temp_valoare_etichete(contor_etichete) = Val(valoare_eticheta_include)
                                                                    '    contor_etichete = contor_etichete + 1
                                                                    'End If
                                                                End If
                                                            End While
                                                        End Using
                                                    Catch ex As Exception
                                                        MsgBox("Eroare citire fisier")
                                                        asamblare_corecta = False
                                                    End Try
                                                End If
                                                If (MyStream IsNot Nothing) Then
                                                    MyStream.Close()
                                                End If
                                                '================================
                                            Else
                                                'este directiva Include eronata!
                                                Form1.RichTextBox2.Text = Form1.RichTextBox2.Text &
                                                    "Linia: " & contor_linie & " : " & linie_citita _
                                                    & " = Directiva Include eronata !" _
                                                    & vbCrLf
                                                If Not constructie_memorie Then
                                                    fisier_listing.Write("Linia: " & contor_linie & " : " & linie_citita _
                                                        & " = Directiva include eronata !" & vbCrLf)
                                                End If
                                                asamblare_corecta = False
                                            End If

                                            'ETICHETA
                                        Else
                                            'este eticheta
                                            'MsgBox("Eticheta!")
                                            'memorez eticheta
                                            eticheta_temp = s
                                            pozitie_eticheta = contor_program
                                            eticheta_in_asteptare = True
                                            'valoarea etichetei va fi data in pasul doi deci sa verific si acolo concordanta valorilor
                                            'in asa fel incit sa tin cont de directiva equ

                                            'verific daca in coloana zero nu este pusa o mnemonica (comanda)
                                            Dim i_contor As UInt16 = 0
                                            While ((eticheta_temp <> mnemonici(i_contor)) And (i_contor < mnemonici.Length - 1))
                                                i_contor = i_contor + 1
                                            End While
                                            If i_contor < mnemonici.Length - 1 Then
                                                Form1.RichTextBox2.Text = Form1.RichTextBox2.Text &
                                                    "Linia: " & contor_linie & " : " & linie_citita _
                                                    & " = Mnemonica in coloana zero !" _
                                                    & vbCrLf
                                                If Not constructie_memorie Then
                                                    fisier_listing.Write("Linia: " & contor_linie & " : " & linie_citita _
                                                        & " = Mnemonica in coloana zero !" & vbCrLf)
                                                End If
                                                asamblare_corecta = False
                                            End If

                                            'verific daca in coloana zero nu este pusa o directiva asamblor
                                            If (s = "db") Or (s = "equ") Then
                                                Form1.RichTextBox2.Text = Form1.RichTextBox2.Text &
                                                    "Linia: " & contor_linie & " : " & linie_citita _
                                                    & " = Directiva asamblor in coloana zero !" _
                                                    & vbCrLf
                                                If Not constructie_memorie Then
                                                    fisier_listing.Write("Linia: " & contor_linie & " : " & linie_citita _
                                                    & " = Directiva asamblor in coloana zero !" & vbCrLf)
                                                End If
                                                asamblare_corecta = False
                                            End If
                                        End If
                                    Else
                                        'elementul nu este pe pozita zero deci este cod sau operand
                                        'initial trebuie sa fie cod deci initial operand = 0
                                        'pe aici mai trec, operand poate deveni = 1 daca o cere codul

                                        'COD - COMANDA - MNEMONICA
                                        If Not operand Then
                                            'MsgBox("Comanda!")
                                            Dim cod As Integer = 0
                                            'caut codul mnemonicii
                                            While ((s <> mnemonici(cod)) And
                                                  (cod < (mnemonici.Length - 1)))
                                                cod = cod + 1
                                            End While
                                            If cod < (mnemonici.Length - 1) Then
                                                'am gasit un cod deci e mnemonica!
                                                'MsgBox("Mnemonica identificata: " & mnemonici(cod) & " cod: " & cod)
                                                'fisier_asamblat.WriteLine(cod)
                                                temp_asamblat(contor_program) = cod
                                                'cod_neexecutabil(contor_program) = False 'NU FOLOSESC DEOCAMDATA....
                                                If Not constructie_memorie Then
                                                    fisier_listing.Write(contor_program & vbTab & cod & vbTab & linie_citita)
                                                End If
                                                contor_program = contor_program + 1

                                                'OPERATII LA CARE URMEAZA OPERAND !!!!!!
                                                operand = False
                                                'determin daca la mnemonica respectiva exista operand
                                                For i_contor As Integer = 0 To cu_operand.Length - 1
                                                    If cod = cu_operand(i_contor) Then
                                                        operand = True 'urmeaza operand
                                                    End If
                                                Next i_contor

                                                'MNEMONICI CARE POT AVEA OPERAND SIR (se ia numai primul caracter din sir)
                                                'semnalez daca e o mnemonica care poate avea operand un sir
                                                If (((s = "movla") Or (s = "addla") Or (s = "subla") _
                                                    Or (s = "andla") Or (s = "orla") Or (s = "xorla")) And db_sir) Then
                                                    temp_asamblat(contor_program) = Asc(sir_ascii(0)) 'salvez operandul in vectorul asamblarii
                                                    contor_program = contor_program + 1
                                                    operand = False 'urmeaza comanda
                                                    db_sir = False 'am folosit sirul
                                                End If

                                                'DIRECTIVE ASAMBLOR 
                                            ElseIf s = "db" Then
                                                'directive asamblor
                                                'am gasit un "db" urmeaza operand la urmatoarea trecere
                                                'operand = True
                                                'aici operandul poate fi un numar sau un sir
                                                operand = Not db_sir
                                                If db_sir Then
                                                    'aici trebuie sa pun toate caracterele din linie
                                                    'in vectorul dezasamblat (codurile ascii)
                                                    For d_contor As Byte = 0 To sir_ascii.Length - 1
                                                        If sir_ascii(d_contor) = "\" Then
                                                            d_contor = d_contor + 1 'trebuie sa urmeze un singur caracter!
                                                            If sir_ascii(d_contor) = "n" Then 'este linie noua
                                                                temp_asamblat(contor_program) = 10 'cod ascii pentru linie noua
                                                                contor_program = contor_program + 1
                                                            ElseIf sir_ascii(d_contor) = "r" Then 'este retur de car
                                                                temp_asamblat(contor_program) = 13 'cod ascii pentru retur de car
                                                                contor_program = contor_program + 1
                                                            Else 'daca nu e nici unul de mai sus scriu caracterul care trebuie sa fie numeric !!!!
                                                                temp_asamblat(contor_program) = sir_ascii(d_contor)
                                                                'MsgBox(temp_asamblat(contor_program))
                                                                contor_program = contor_program + 1
                                                            End If
                                                        Else
                                                            temp_asamblat(contor_program) = Asc(sir_ascii(d_contor))
                                                            'MsgBox(temp_asamblat(contor_program))
                                                            contor_program = contor_program + 1
                                                        End If
                                                    Next d_contor
                                                    db_sir = False
                                                End If
                                            ElseIf s = "equ" Then
                                                If eticheta_in_asteptare Then
                                                    operand = True
                                                    equ_val = True
                                                Else
                                                    'MsgBox("equ fara eticheta !!!!!!!!!!!!!!")
                                                    Form1.RichTextBox2.Text = Form1.RichTextBox2.Text &
                                                    "Linia: " & contor_linie & " : " & linie_citita _
                                                    & " = Directiva equ fara eticheta !" _
                                                    & vbCrLf
                                                    If Not constructie_memorie Then
                                                        fisier_listing.Write("Linia: " & contor_linie & " : " & linie_citita _
                                                    & " = Directiva equ fara eticheta !" & vbCrLf)
                                                    End If
                                                    asamblare_corecta = False
                                                End If
                                            ElseIf eticheta_in_asteptare
                                                'este eticheta si o inregistrez
                                                verific_eticheta_dubla(eticheta_temp, Str(pozitie_eticheta), False, fisier_listing)
                                                'eticheta interna - eticheta programului
                                                eticheta_in_asteptare = False
                                            Else
                                                'nu e nimic din ce ma astept deci semnalez!
                                                'nu e nici mnemonica nici directiva asamblor
                                                'MsgBox("Nu am gasit mnemonica!")
                                                Form1.RichTextBox2.Text = Form1.RichTextBox2.Text &
                                                "Linia: " & contor_linie & " : " & linie_citita _
                                                & " = Nu am gasit mnemonica !" _
                                                & vbCrLf
                                                asamblare_corecta = False
                                                If Not constructie_memorie Then
                                                    fisier_listing.Write("Linia: " & contor_linie & " : " & linie_citita _
                                                        & " = Nu am gasit mnemonica !")
                                                End If
                                            End If

                                            'OPERAND
                                        Else
                                            'acum trebuie sa gasesc un operand !!!!!
                                            'operandul il scriu direct
                                            'aici trebuie sa verific daca operandul numeric este mai mare ca 255
                                            If Val(s) > 255 Then
                                                'MsgBox("Operand peste 255!")
                                                Form1.RichTextBox2.Text = Form1.RichTextBox2.Text &
                                                    "Linia: " & contor_linie & " : " & linie_citita _
                                                    & " = Operand peste 255 !" _
                                                    & vbCrLf
                                                asamblare_corecta = False
                                            End If
                                            If equ_val Then
                                                'este o directiva equ si deci pun valoarea in etichete

                                                'AICI PUN SI NUMELE SI VALOAREA ETICHETEI SI VERIFIC DACA ETICHETA EXISTA !!!!!!
                                                verific_eticheta_dubla(eticheta_temp, s, False, fisier_listing)
                                                'eticheta interna - eticheta programului
                                                operand = False
                                                equ_val = False
                                                eticheta_in_asteptare = False
                                            Else
                                                'este o mnemonica cu operand sir - se rezolva la a doua trecere
                                                'sau un operand valoare numerica
                                                operand = False 'urmeaza comanda
                                                temp_asamblat(contor_program) = s 'salvez operandul in vectorul asamblarii
                                                contor_program = contor_program + 1
                                                If Not constructie_memorie Then
                                                    fisier_listing.Write(linie_citita)
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                                If Not constructie_memorie Then
                                    fisier_listing.WriteLine()
                                End If
                            Next s
                        End If
                    End While
                End Using

                'o ultima verificare a etichetelor nerezolvate......
                If eticheta_in_asteptare Then
                    verific_eticheta_dubla(eticheta_temp, Str(pozitie_eticheta), False, fisier_listing)
                    'eticheta interna - eticheta program
                    eticheta_in_asteptare = False
                End If

                'am terminat de analizat liniile fisierului de asamblat si trec la analiza etichetelor
                'introduc in temp_nume_etichete numele predefinite
                'acestea sunt simboluri in program cu semnificatie predefinita
                determin_etichete() 'subprogramul cu a doua trecere pentru determinarea valorilor etichetelor
                'prima trecere a fost atunci cind s-a facut asamblarea - in vectorul cu etichete
                'pot exista acum texte pentru care se vor calcula valorile
                Form1.RichTextBox2.Text = Form1.RichTextBox2.Text & "Number of bytes of assembled program: " _
                    & contor_program & vbCrLf
                Form1.RichTextBox2.Text = Form1.RichTextBox2.Text & "Number of bytes remaining on page: " _
                    & 256 - contor_program & vbCrLf
                If asamblare_corecta Then
                    Form1.RichTextBox2.Text = Form1.RichTextBox2.Text & "Assembling completed successfully!" & vbCrLf
                Else
                    Form1.RichTextBox2.SelectionColor() = Color.Red
                    Form1.RichTextBox2.SelectedText += "Abort assembly!" & vbCrLf
                End If
                'MsgBox("Sfirsit fisier!")

                'scriu fisierul asamblat (codul)
                If Not constructie_memorie Then
                    For i_contor As Integer = 0 To contor_program - 1
                        fisier_asamblat.WriteLine(temp_asamblat(i_contor) & ",")
                    Next i_contor
                End If

                'scriu fisierul listing
                If Not constructie_memorie Then
                    fisier_listing.WriteLine("==============================")
                    For i_contor As Integer = 0 To contor_etichete - 1
                        fisier_listing.Write(temp_nume_etichete(i_contor) & " = ")
                        fisier_listing.WriteLine(temp_valoare_etichete(i_contor))
                    Next i_contor
                    fisier_listing.WriteLine("==============================")
                    fisier_listing.WriteLine("Number of bytes used for the program: " & contor_program)
                    fisier_listing.WriteLine("Number of bytes remaining on page: " & 256 - contor_program)
                    fisier_listing.WriteLine("==============================")
                End If

                'scriu fisierul de inclus
                If Not constructie_memorie Then
                    For i_contor As Integer = 0 To contor_etichete - 1
                        If Not temp_etichete_externe(i_contor) Then
                            fisier_include.Write(temp_nume_etichete(i_contor) & " = ")
                            fisier_include.WriteLine(temp_valoare_etichete(i_contor))
                        End If
                    Next i_contor
                End If

                'am terminat, inchid fisierele
                fisier_asamblat.Close()
                fisier_listing.Close()
                fisier_include.Close()

                'daca este o eroare...
            Catch ex As Exception
                MsgBox("Error assembly!")
                initializare()
                fisier_asamblat.Close()
                fisier_listing.Close()
                fisier_include.Close()
            End Try
        End If
    End Sub
End Module
