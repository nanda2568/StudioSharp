Public Class frmCrashulator

    Private Sub btnExecute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExecute.Click

        ' We will use this procedure to demonstrate how to catch multiple 
        ' different types of exceptions
        Try
            ' Select case to figure out what operation to actually execute
            Select Case cmbOperation.SelectedItem.ToString()
                Case "Square Root"
                    Dim op1, result As Decimal
                    op1 = Convert.ToDecimal(txtOperand1.Text)
                    result = Math.Sqrt(op1)
                    txtResult.Text = result.ToString()
                Case "Divide"
                    Dim result, op1, op2 As Decimal
                    op1 = Convert.ToDecimal(txtOperand1.Text)
                    op2 = Convert.ToDecimal(txtOperand2.Text)
                    result = op1 / op2
                    txtResult.Text = result.ToString()
                Case "Factorial"
                    Dim result As Long
                    Dim op1 As Long
                    op1 = Convert.ToInt32(txtOperand1.Text)
                    result = Factorial(op1)
                    txtResult.Text = result.ToString()
                Case "Sum Range"
                    Dim result As Integer
                    Dim op1, op2 As Integer
                    op1 = Convert.ToInt16(txtOperand1.Text)
                    op2 = Convert.ToInt16(txtOperand2.Text)
                    result = SumRange(op1, op2)
                    txtResult.Text = result.ToString()
                Case Else
                    Throw New Exception("Unknown operation")
            End Select

        Catch tmdEx As TooManyDigitsException
            MessageBox.Show(tmdEx.NumberOfDigits.ToString() & " digits: " & tmdEx.Message)

        Catch nullEx As NullReferenceException
            MessageBox.Show("You probably forgot to select an operation: " & nullEx.Message)

        Catch divEx As DivideByZeroException
            MessageBox.Show("Egg under the table: " & divEx.Message)

        Catch ofEx As OverflowException
            MessageBox.Show("Result won't fit: " & ofEx.Message)

        Catch ex As Exception
            MessageBox.Show("General Exception: " & ex.Message)

        End Try

    End Sub

    Private Function Factorial(ByVal value As Long) As Long
        Dim result As Long = 1

        ' For each number between the incoming argument and 1 (step = -1)
        ' keep multiplying
        For x As Long = value To 1 Step -1
            result *= x
        Next

        ' Here we'll do something a little different with a new exception
        ' If we have more than 10 digits, it won't look nice in the form's
        ' text box (I know, that's not a realistic exception, but it's an
        ' understandable example of how a custom exception can be used)
        ' ... imagine this is a handheld calculator with a very small LED screen
        Dim resultString As String = result.ToString()
        ' check the length of this result - if it's more than 10 characters
        ' then throw an exceptoin of our custom type
        If resultString.Length >= 10 Then
            Dim tmdEx As New TooManyDigitsException("Factorial calculation result contains too many digits to display.")
            tmdEx.NumberOfDigits = resultString.Length
            Throw tmdEx
        End If

        Return result

    End Function

    Private Function SumRange(ByVal lowVal As Integer, ByVal highVal As Integer) As Integer
        Dim result As Integer = 0

        ' First, check to see if the highVal is actually greater than
        ' the low val.  If it's not, throw an arithmetic exception.
        If lowVal > highVal Then
            Throw New Exception("Low Value must be less than High Value")
        End If


        ' For each number between the incoming arguments, keep adding
        For x As Integer = lowVal To highVal
            result += x
        Next

        Return result

    End Function


End Class

' Define a new exception class to handle a specific type of application issue
' that we want to handle as an exception
Public Class TooManyDigitsException
    Inherits System.ApplicationException

    Private _numDigits As Long

    Property NumberOfDigits() As Long
        Get
            Return _numDigits
        End Get
        Set(ByVal value As Long)
            _numDigits = value
        End Set
    End Property

    Public Sub New()
    End Sub

    ' Creates a Sub New for the exception that allows you to set the
    ' message property when thrown.
    Public Sub New(ByVal Message As String)
        MyBase.New(Message)
    End Sub
    ' Creates a Sub New that can be used when you also want to include
    ' the inner exception.
    Public Sub New(ByVal Message As String, ByVal Inner As Exception)
        MyBase.New(Message)
    End Sub


End Class
