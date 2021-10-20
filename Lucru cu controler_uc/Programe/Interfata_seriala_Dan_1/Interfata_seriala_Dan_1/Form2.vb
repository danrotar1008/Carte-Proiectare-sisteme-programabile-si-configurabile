Public Class Form2
    Dim viteza_seriala As Integer 'aici stochez viteza serialei pina la acceptare

    Private Sub afisare_parametrii_seriala()
        Label3.Text = Label3.Text & " " & Form1.SerialPort1.DataBits
        If Form1.SerialPort1.Parity = 0 Then
            Label4.Text = Label4.Text & " fara"
        ElseIf Form1.SerialPort1.Parity = 1 Then
            Label4.Text = Label4.Text & " impar"
        ElseIf Form1.SerialPort1.Parity = 2 Then
            Label4.Text = Label4.Text & " par"
        ElseIf Form1.SerialPort1.Parity = 3 Then
            Label4.Text = Label4.Text & " mark"
        ElseIf Form1.SerialPort1.Parity = 4 Then
            Label4.Text = Label4.Text & " space"
        End If
        Label5.Text = Label5.Text & " " & Form1.SerialPort1.StopBits
        If Form1.SerialPort1.Handshake = 0 Then
            Label6.Text = Label6.Text & " fara"
        ElseIf Form1.SerialPort1.Handshake = 1 Then
            Label6.Text = Label6.Text & " XonXoff"
        ElseIf Form1.SerialPort1.Handshake = 2 Then
            Label6.Text = Label6.Text & " RequestToSend"
        ElseIf Form1.SerialPort1.Handshake = 3 Then
            Label6.Text = Label6.Text & " RequestToSendXonXoff"
        End If
        Label7.Text = Label7.Text & " " & Form1.SerialPort1.BaudRate
        If Form1.SerialPort1.BaudRate = 4800 Then
            RadioButton1.Checked = True
        ElseIf Form1.SerialPort1.BaudRate = 9600 Then
            RadioButton2.Checked = True
        ElseIf Form1.SerialPort1.BaudRate = 19200 Then
            RadioButton3.Checked = True
        ElseIf Form1.SerialPort1.BaudRate = 38400 Then
            RadioButton4.Checked = True
        ElseIf Form1.SerialPort1.BaudRate = 57600 Then
            RadioButton5.Checked = True
        ElseIf Form1.SerialPort1.BaudRate = 115200 Then
            RadioButton6.Checked = True
        End If

    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click 'accepta
        Form1.SerialPort1.BaudRate = viteza_seriala
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click 'anuleaza
        Me.Close()
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        afisare_parametrii_seriala()
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        viteza_seriala = 4800
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        viteza_seriala = 9600
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        viteza_seriala = 19200
    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton4.CheckedChanged
        viteza_seriala = 38400
    End Sub

    Private Sub RadioButton5_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton5.CheckedChanged
        viteza_seriala = 57600
    End Sub

    Private Sub RadioButton6_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton6.CheckedChanged
        viteza_seriala = 115200
    End Sub
End Class