'Author: Bruce Brown
'Date: 3/10/2021
'Purpose: This class manages the contacts inputted by the user.

Option Strict On
Option Explicit On


Public Class clsContacts

    'Declare Variables
    Private mstrContactID As String
    Dim mstrFirstName As String
    Dim mstrLastName As String
    Dim mstrPhoneNumber As String
    Dim mstrAddressOne As String
    Dim mstrAddressTwo As String
    Dim mstrEmail As String

    Public Property ContactID As String
        Get
            Return mstrContactID
        End Get
        Set(value As String)
            mstrContactID = value
        End Set
    End Property
    Public Property FirstName As String
        Get
            Return mstrFirstName
        End Get
        Set(value As String)
            mstrFirstName = value
        End Set
    End Property

    Public Property LastName As String
        Get
            Return mstrLastName
        End Get
        Set(value As String)
            mstrLastName = value
        End Set
    End Property
    Public Property PhoneNumber As String
        Get
            Return mstrPhoneNumber
        End Get
        Set(value As String)
            mstrPhoneNumber = value
        End Set
    End Property
    Public Property AddressOne As String
        Get
            Return mstrAddressOne
        End Get
        Set(value As String)
            mstrAddressOne = value
        End Set
    End Property
    Public Property AddressTwo As String
        Get
            Return mstrAddressTwo
        End Get
        Set(value As String)
            mstrAddressTwo = value
        End Set
    End Property
    Public Property Email As String
        Get
            Return mstrEmail
        End Get
        Set(value As String)
            mstrEmail = value
        End Set
    End Property

    Public ReadOnly Property FullName As String
        Get
            Return Full_Name()
        End Get
    End Property

    'Default Constructor
    Public Sub New()
        mstrContactID = String.Empty
        mstrFirstName = String.Empty
        mstrLastName = String.Empty
        mstrPhoneNumber = String.Empty
        mstrAddressOne = String.Empty
        mstrAddressTwo = String.Empty
        mstrEmail = String.Empty
    End Sub

    'Overload Constructors
    Public Sub New(ByVal strContactID As String)
        mstrContactID = strContactID
        mstrFirstName = String.Empty
        mstrLastName = String.Empty
        mstrPhoneNumber = String.Empty
        mstrAddressOne = String.Empty
        mstrAddressTwo = String.Empty
        mstrEmail = String.Empty
    End Sub

    Public Sub New(ByVal strFirstName As String, ByVal strLastName As String, ByVal strPhoneNumber As String)
        mstrContactID = String.Empty
        mstrFirstName = strFirstName
        mstrLastName = strLastName
        mstrPhoneNumber = strPhoneNumber
        mstrAddressOne = String.Empty
        mstrAddressTwo = String.Empty
        mstrEmail = String.Empty
    End Sub

    'Methods
    Public Function Full_Name() As String
        Dim fullName As String = mstrFirstName & " " & mstrLastName
        Return fullName
    End Function

    Public Function Name_Number() As String
        Dim contact As String = Full_Name()
        contact += ": " & mstrPhoneNumber
        Return contact
    End Function

End Class
