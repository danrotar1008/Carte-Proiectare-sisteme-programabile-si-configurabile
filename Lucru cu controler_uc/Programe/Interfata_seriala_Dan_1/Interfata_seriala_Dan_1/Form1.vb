Imports System
Imports System.IO
'Imports System.Text
Imports System.IO.Ports
'Imports System.Threading
'Imports System.ComponentModel


Public Class Form1
    Dim port_serial As String
    Dim nume_fisier As String = ""
    Dim linie_citita As String = ""
    Dim numar_pagina As Byte = 0
    Dim tip_executie As Byte = 0
    Dim tip_emisie As Byte = 1 '= 0 emisie ASCII sau = 1 emisie zecimal
    Dim tip_ecou As Byte = 0 '= 0 cu ecou sau = 1 fara ecou
    Delegate Sub SetTextCallback(ByVal [text] As String) 'Added to prevent threading errors during receiveing of data

    Dim nr_caractere As UInteger
    Dim caracter As Char

    'Private SerialPort1 As New SerialPort
    Private comBuffer As Byte()
    Private Delegate Sub UpdateFormDelegate()
    Private UpdateFormDelegate1 As UpdateFormDelegate

    Sub GetSerialPortNames()
        ' Show all available COM ports.
        For Each sp As String In My.Computer.Ports.SerialPortNames
            ListBox1.Items.Add(sp)
        Next
    End Sub
    Private Sub initializare()
        Button1.Visible = False 'inchide port serial
        Label1.Visible = False 'Alegeti port serial
        Label2.Visible = False 'port serial deschis
        Label3.Visible = False 'Numele fisierului care se transmite
        Label4.Visible = False 'Transmitere fisier cu succes sau eroare
        Button2.Visible = False 'stare parametri port serial
        Button3.Visible = False 'alegere fisier de trimis
        Button4.Visible = False 'buton trimitere fisier la UART
        ListBox2.Visible = False 'alegere pagina unde scriu codul
        Label5.Visible = False 'afiseaza: alegeti pagina
        Label6.Visible = False 'afiseaza: alegeti tipul de date
        'Button4.Visible = False 'comanda trimitere date prin interfata seriala la Nexys3
        RadioButton1.Visible = False 'date - nu se executa
        RadioButton2.Visible = False 'program - se executa
        RadioButton3.Visible = False 'ASCII - implicit
        RadioButton4.Visible = False 'zecimal
        RadioButton5.Visible = False 'Cu ecou - implicit
        RadioButton6.Visible = False 'Fara ecou
        Label7.Visible = False 'afiseaza Emisie
        Button5.Visible = False 'trimite la UART
        RichTextBox1.Visible = False 'contine caracterul de trimis la UART
        nume_fisier = "" 'numele fisierului coe de transmis
        linie_citita = "" 'linia citita din fisierul .coe
        numar_pagina = 0 'pagina unde se scriu datele trimise
        ListBox2.ClearSelected()
        'ListBox2.SelectedIndex = -1
        RadioButton1.Checked = True
        RadioButton2.Checked = False
        RadioButton3.Checked = False
        RadioButton4.Checked = True
        TextBox1.Visible = False
        Button6.Visible = False
        RadioButton5.Checked = True
        RadioButton6.Checked = False
        Label8.Visible = False
        RichTextBox2.Visible = False 'Afiseaza codurile ASCII
        GroupBox1.Visible = False
        GroupBox2.Visible = False
        GroupBox3.Visible = False
        Label9.Visible = False
        Button7.Visible = False 'sterge datele trimise
        RichTextBox1.Text = ""
        RichTextBox2.Text = ""
        RichTextBox3.Text = ""
        RichTextBox3.Visible = False

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initializare()
        'initializare()
        Button4.Visible = False
        Label1.Visible = True 'Alegeti port serial
        For icontor As Byte = 1 To 31
            ListBox2.Items.Add(icontor)
        Next
        GetSerialPortNames()
        AddHandler SerialPort1.DataReceived, AddressOf SerialPort1_DataReceived

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        SerialPort1.Close()
        port_serial = ListBox1.SelectedItem
        'MsgBox("Port serial selactat" & port_serial)
        Try
            SerialPort1.PortName = port_serial
            SerialPort1.Open()
            initializare()
            Label2.Visible = True
            Label2.Text = port_serial & " deschis!"
            Button1.Visible = True
            Button2.Visible = True
            Button3.Visible = True
            Button4.Visible = False
            Label7.Visible = True
            Button5.Visible = True
            RichTextBox1.Visible = True
            RadioButton3.Visible = True 'ASCII - implicit
            RadioButton4.Visible = True 'zecimal
            TextBox1.Visible = True
            Label8.Visible = True
            RadioButton5.Visible = True 'ASCII - implicit
            RadioButton6.Visible = True 'zecimal
            RichTextBox2.Visible = True 'Afiseaza codurile ASCII
            GroupBox1.Visible = False
            GroupBox2.Visible = True
            GroupBox3.Visible = True
            Label9.Visible = True
            RichTextBox2.Text = ""
            RichTextBox3.Visible = True
            RichTextBox3.Text = ""

            'Initializare parametrii impliciti port serial
            SerialPort1.BaudRate = 19200
            SerialPort1.DiscardInBuffer()

        Catch
            MsgBox("A aparut o eroare!")
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SerialPort1.Close()
        initializare()
        'initializare()
        ListBox1.Visible = True
        Label1.Visible = True
        Button4.Visible = False 'buton trimitere fisier la UART
        Form2.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Me.Close()
        Form2.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'transmitere fisier prin interfata seriala
        'deschid fisier coe
        Dim dialogDeschidFisier1 As New OpenFileDialog()
        Dim MyStream As Stream = Nothing

        initializare()
        'initializare()
        Label2.Visible = True
        Label2.Text = port_serial & " deschis!"
        Button1.Visible = True
        Button2.Visible = True
        Button3.Visible = True
        Button4.Visible = False
        RichTextBox1.Visible = True
        RadioButton3.Visible = True 'ASCII - implicit
        RadioButton4.Visible = True 'zecimal
        TextBox1.Visible = True
        Label8.Visible = True
        RadioButton5.Visible = True 'ASCII - implicit
        RadioButton6.Visible = True 'zecimal
        RichTextBox2.Visible = True 'Afiseaza codurile ASCII
        GroupBox2.Visible = True
        GroupBox3.Visible = True
        RichTextBox3.Visible = True
        Label9.Visible = True

        dialogDeschidFisier1.InitialDirectory = ""
        dialogDeschidFisier1.Filter = "coe files (*.coe)|*.coe|All files (*.*)|*.*"
        dialogDeschidFisier1.FilterIndex = 1
        dialogDeschidFisier1.RestoreDirectory = True

        If dialogDeschidFisier1.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            Try
                MyStream = dialogDeschidFisier1.OpenFile
                If (MyStream IsNot Nothing) Then
                    nume_fisier = dialogDeschidFisier1.FileName
                    Label3.Visible = True
                    Label3.Text = "Fisier de transmis: " & nume_fisier
                    Label7.Visible = True
                    Button5.Visible = True
                    RichTextBox1.Visible = True
                    'FisierCitit = My.Computer.FileSystem.ReadAllText(nume_fisier)
                End If
            Catch ex As Exception
                MsgBox("Eroare citire fisier")
            End Try
        End If
        If (MyStream IsNot Nothing) Then
            MyStream.Close()
        End If

        If nume_fisier = "" Then 'nu s-a ales fisier
            initializare()
            Label2.Visible = True
            Label2.Text = port_serial & " deschis!"
            Button1.Visible = True
            Button2.Visible = True
            Button3.Visible = True
            Label7.Visible = True
            Button5.Visible = True
            RichTextBox1.Visible = True
        Else 's-a ales fisier
            'se alege pagina unde scriu datele
            ListBox2.Visible = True
            Label5.Visible = True
            'For icontor As Byte = 1 To 31
            '    ListBox2.Items.Add(icontor)
            'Next
        End If
    End Sub

    Private Sub transmitere_seriala()

        Dim caracter As Byte

        'vezi:
        'https://msdn.microsoft.com/en-us/library/vstudio/ms143551(v=vs.100).aspx        
        'https://msdn.microsoft.com/en-us/library/vstudio/system.byte(v=vs.100).aspx
        'https://msdn.microsoft.com/en-us/library/vstudio/system.int32(v=vs.100).aspx

        'se trimite fisierul .coe la Nexys3
        'pentru inceput creez datele de transmis
        Dim date_de_transmis(300) As String 'datele de trimis la Nexis
        Dim icontor As UInteger = 5 'contor local, de la 7 incep octetii de date
        'incep de la 5 ca sa suprascriu cele doua linii
        'de la inceputul fisierului .coe
        Dim suma_brut As UInteger = 0 'suma tuturor octetilor de date
        Dim suma As Byte = 0 'octetul suma tuturor octetilor de date

        Using sr As New StreamReader(nume_fisier)
            While Not sr.EndOfStream 'cit timp fisierul nu a ajuns la sfirsit
                linie_citita = sr.ReadLine() 'citesc cite o linie
                'MsgBox(linie_citita & " <--> " & Str(Val(linie_citita)))
                linie_citita = Str(Val(linie_citita))
                'MsgBox(linie_citita)
                date_de_transmis(icontor) = linie_citita
                suma_brut = suma_brut + Val(linie_citita)
                icontor = icontor + 1
            End While
        End Using
        suma = suma_brut Mod 256
        'MsgBox("Suma = " & suma)
        '0. D - ASCII 68 zecimal (44h)
        '1. A - ASCII 65 zecimal (41h)
        '2. N - ASCII 78 zecimal (4Eh)
        '3. numarul paginii unde se depun datele (1-31)
        '4. numarul de octeti ai programului (max 255)
        '5. cod de executie: 0- nu se executa programul din pagina respectiva acum, 1- se executa codul din pagina 		respectiva la terminarea incarcarii
        '6. suma de control
        '7. octetii de date pina la sfirsit!
        date_de_transmis(0) = "68" 'D
        date_de_transmis(1) = "65" 'A
        date_de_transmis(2) = "78" 'N
        date_de_transmis(3) = Str(numar_pagina) 'numarul paginii unde scriu datele !!!!
        date_de_transmis(4) = Str(icontor - 7) 'numarul de octeti ai programului !!!!
        date_de_transmis(5) = Str(tip_executie) 'cod de executie !!!!
        date_de_transmis(6) = Str(suma) 'suma de control

        ''afisez datele
        'For jcontor As UInteger = 0 To icontor - 1
        '    MsgBox("Data: " & date_de_transmis(jcontor))
        'Next

        ' transmitere date catre Nexys3
        SerialPort1.DiscardOutBuffer() 'golesc buffer de transmisie
        SerialPort1.DiscardInBuffer() 'golesc buffer de receptie
        For jcontor As UInteger = 0 To icontor - 1
            caracter = Convert.ToByte(date_de_transmis(jcontor))
            Dim buffer() As Byte = {caracter}
            'MsgBox("Data: " & buffer(0))
            SerialPort1.Write(buffer, 0, 1)
        Next

    End Sub

    Private Sub verificare_transmitere()
        'aici verific daca Nexys2 raspunde cu 'DAN'
        'daca raspunde asa atunci s-a receptionat corect
        'daca nu, atunci reiau transmisia
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'se va trimite la port serial
        transmitere_seriala()
        'se verifica daca Nexys a receptionat corect
        verificare_transmitere()
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        numar_pagina = ListBox2.SelectedIndex + 1
        'If numar_pagina <> 0 Then
        '    MsgBox("Numar pagina ales = " & numar_pagina)
        'End If
        Label6.Visible = True
        RadioButton1.Visible = True
        RadioButton2.Visible = True
        GroupBox1.Visible = True
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        tip_executie = 0
        Button4.Visible = True
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        tip_executie = 1
        Button4.Visible = True
    End Sub

    Private Sub SerialPort1_DataReceived(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs)
        'Handles serial port data received events
        UpdateFormDelegate1 = New UpdateFormDelegate(AddressOf UpdateDisplay)
        Dim n As Integer = SerialPort1.BytesToRead 'find number of bytes in buf
        nr_caractere = n
        comBuffer = New Byte(n - 1) {} 're dimension storage buffer
        SerialPort1.Read(comBuffer, 0, n) 'read data from the buffer
        Me.Invoke(UpdateFormDelegate1) 'call the delegate
    End Sub

    Private Sub UpdateDisplay()
        'Label10.Text &= comBuffer(0) & " "
        Try
            For jcontor As UInteger = 0 To nr_caractere - 1
                caracter = Convert.ToChar(comBuffer(jcontor))
                RichTextBox1.Text &= caracter
                RichTextBox3.Text &= "  " & caracter & " - " & comBuffer(jcontor) & vbCrLf
                RichTextBox1.SelectionStart = RichTextBox1.Text.Length
                RichTextBox1.ScrollToCaret()
                RichTextBox3.SelectionStart = RichTextBox3.Text.Length
                RichTextBox3.ScrollToCaret()
            Next
        Catch
        End Try
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        RichTextBox1.Clear()
        RichTextBox3.Clear()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text = "" Then
            Button6.Visible = False
        Else
            Button6.Visible = True
        End If
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        tip_emisie = 0
    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton4.CheckedChanged
        tip_emisie = 1
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim caracter As Byte
        Dim afisat As Char

        Try
            If tip_emisie = 0 Then
                'trimit caracter ascii
                caracter = Convert.ToByte(Asc(TextBox1.Text))
                Dim buffer() As Byte = {caracter}
                'MsgBox("Data transmisa: " & buffer(0))
                SerialPort1.Write(buffer, 0, 1)
                afisat = TextBox1.Text
            Else
                'trimit sir convertit in caracter ascii

                caracter = Convert.ToByte(TextBox1.Text)
                Dim buffer() As Byte = {caracter}
                'MsgBox("Data transmisa: " & buffer(0))
                SerialPort1.Write(buffer, 0, 1)
                afisat = Convert.ToChar(buffer(0))
            End If
            TextBox1.Text = ""
            If tip_ecou = 0 Then
                'MsgBox("Afisez cu ecou")
                Button7.Visible = True 'sterge datele trimise
                RichTextBox2.Text &= "  " & afisat & " = " & caracter & vbCrLf
                RichTextBox2.SelectionStart = RichTextBox2.Text.Length
                RichTextBox2.ScrollToCaret()
            Else
                'MsgBox("Afisez fara ecou")
            End If
        Catch
            MsgBox("Data eronata !!!!!")
        End Try

    End Sub

    Private Sub RadioButton5_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton5.CheckedChanged
        tip_ecou = 0
    End Sub

    Private Sub RadioButton6_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton6.CheckedChanged
        tip_ecou = 1
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        RichTextBox2.Text = ""
        Button7.Visible = False 'sterge datele trimise
    End Sub
End Class


