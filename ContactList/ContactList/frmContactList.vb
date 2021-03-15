Option Strict On
Option Explicit On
'Author: Bruce Brown
'Date: 3/10/2021
'Purpose: Application will save contacts from a user and display them in a list in alphabetical order.

Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class frmContactList

    'Module level Object
    Dim lstContact As New BindingList(Of clsContacts)
    Dim objSelectedContact As New clsContacts()

    Private Sub btnSaveEdit_Click(sender As Object, e As EventArgs) Handles btnSaveEdit.Click
        'Declare Variables
        Dim firstName As String
        Dim lastName As String
        Dim phoneNumber As String
        Dim addressOne As String
        Dim addressTwo As String
        Dim email As String

        'Assign Variables
        firstName = txtFirstName.Text
        lastName = txtLastName.Text
        phoneNumber = txtPhoneNumber.Text
        addressOne = txtAddress1.Text
        addressTwo = txtAddress2.Text
        email = txtEmail.Text

        'Verify required fields are inputted
        If firstName <> String.Empty Then
            If lastName <> String.Empty Then
                If phoneNumber <> String.Empty Then
                    If phoneNumber.Length >= 10 AndAlso phoneNumber.Length <= 15 Then

                        'Makes sure address one is not blank if address two is filled out
                        If addressTwo <> String.Empty AndAlso addressOne = String.Empty Then
                            Call Msg("Address one cannot be left blank when address two is filled out. Please try again.")
                            txtAddress1.Focus()
                        Else

                            'Assign New Contact Properties
                            Dim objNewContact = New clsContacts(firstName, lastName, phoneNumber)

                            If addressOne <> String.Empty Then
                                objNewContact.AddressOne = addressOne

                                If addressTwo <> String.Empty Then
                                    objNewContact.AddressTwo = addressTwo
                                End If

                            End If

                            If email <> String.Empty Then
                                objNewContact.Email = email
                            End If

                            If btnSaveEdit.Text = "Save" Then

                                'Add new contact to list of customers
                                lstContact.Add(objNewContact)
                                'Insert contact in database
                                InsertContact()

                            ElseIf btnSaveEdit.Text = "Edit" Then

                                'Update contact in database
                                UpdateContact()
                            End If

                            'Make the New contact the selected character
                            objSelectedContact = objNewContact

                            'Select Case the current contact In the listbox
                            lbxContacts.SelectedItem = objNewContact

                            'Reset the fields and get ready for a new contact
                            ResetFields()

                        End If

                    Else
                        Call Msg("Invalid phone number. Please try again.")
                        txtPhoneNumber.Clear()
                        txtPhoneNumber.Focus()
                    End If

                Else
                    Call Msg("Phone number cannot be left blank. Please try again.")
                    txtPhoneNumber.Focus()
                End If

            Else
                Call Msg("Last name cannot be left blank. Please try again.")

                txtLastName.Focus()
            End If

        Else
            Call Msg("First name cannot be left blank. Please try again.")
            txtFirstName.Focus()
        End If


    End Sub

    Private Sub lblReset_Click(sender As Object, e As EventArgs) Handles lblReset.Click
        ResetFields()
    End Sub

    Private Sub ResetFields()
        'Resets all fields
        txtFirstName.Text = String.Empty
        txtLastName.Text = String.Empty
        txtPhoneNumber.Text = String.Empty
        txtAddress1.Text = String.Empty
        txtAddress2.Text = String.Empty
        txtEmail.Text = String.Empty
        txtFirstName.Focus()
    End Sub

    Public Sub Msg(ByVal strMsg As String)
        MessageBox.Show(strMsg, "Contacts", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        'Remove the deleted character
        DeleteContact()
        objSelectedContact = Nothing
    End Sub

    Private Sub frmContactList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Set Custom Binding List to Listbox
        lbxContacts.DataSource = lstContact
        lbxContacts.DisplayMember = "FullName"

        Reload_lbxContacts()
    End Sub

    Private Sub lbxContacts_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbxContacts.SelectedIndexChanged
        If lbxContacts.SelectedIndex >= 0 Then
            objSelectedContact = CType(lbxContacts.SelectedItem, clsContacts)

            'Populate Textboxes with Selected Contact
            txtFirstName.Text = objSelectedContact.FirstName
            txtLastName.Text = objSelectedContact.LastName
            txtPhoneNumber.Text = objSelectedContact.PhoneNumber
            txtAddress1.Text = objSelectedContact.AddressOne
            txtAddress2.Text = objSelectedContact.AddressTwo
            txtEmail.Text = objSelectedContact.Email

            'Display contact information
            MessageBox.Show(objSelectedContact.Name_Number, "Contacts")

            'Determine if this is an edit. Allows delete and new submission if not an edit.
            Dim intResult As Integer = MessageBox.Show("Are you updating this record?", "Contacts", MessageBoxButtons.YesNo)
            If intResult = DialogResult.Yes Then
                btnSaveEdit.Text = "Edit"
            ElseIf intResult = DialogResult.No Then
                ResetFields()
            End If

        End If
    End Sub

    Private Function OpenDBConnection() As SqlConnection

        'Create a connection string
        'This give the full path into the bin/debug folder
        Dim strPath As String = Application.StartupPath
        Dim intPathLength As Integer = strPath.Length
        'This strips off the bin/debug folder to point into your project folder.
        strPath = strPath.Substring(0, intPathLength - 9)

        Dim strconnection As String = "Server=(LocalDB)\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=" + strPath + "ContactsList.mdf"

        'Create a Connection object
        Dim dbConnection As New SqlConnection(strconnection)

        Try
            'Open Database
            dbConnection.Open()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Return dbConnection

    End Function

    Public Sub InsertContact()
        'Open Database
        Dim dbConnection As SqlConnection = OpenDBConnection()

        'Create SQL String
        Dim strSQL = "Insert into Contact_Tbl (FirstName,LastName,PhoneNumber,AddressOne,AddressTwo,Email) values (@FirstName,@LastName,@PhoneNumber,@AddressOne,@AddressTwo,@Email)"

        'Create Command Object
        Dim cmdInsert As New SqlCommand(strSQL, dbConnection)

        'Populate Parameters of the Insert using the last record added to the list object
        cmdInsert.Parameters.AddWithValue("FirstName", lstContact.Last.FirstName)
        cmdInsert.Parameters.AddWithValue("LastName", lstContact.Last.LastName)
        cmdInsert.Parameters.AddWithValue("PhoneNumber", lstContact.Last.PhoneNumber)
        cmdInsert.Parameters.AddWithValue("AddressOne", lstContact.Last.AddressOne)
        cmdInsert.Parameters.AddWithValue("AddressTwo", lstContact.Last.AddressTwo)
        cmdInsert.Parameters.AddWithValue("Email", lstContact.Last.Email)

        Try
            Dim intRowsImpacted = cmdInsert.ExecuteNonQuery()
            If intRowsImpacted = 1 Then
                MessageBox.Show(lstContact.Last.Full_Name & " was inserted.")
            Else
                MessageBox.Show("The insert failed")
            End If
        Catch ex As Exception
            MessageBox.Show("DB Insert Failed" & ex.Message)

        End Try

        Reload_lbxContacts()

    End Sub

    Public Sub UpdateContact()
        'Open Database
        Dim dbConnection As SqlConnection = OpenDBConnection()

        'Create SQL String
        Dim strSQL = "Update Contact_Tbl set FirstName = '" & txtFirstName.Text & "',LastName = '" & txtLastName.Text & "',PhoneNumber = '" & txtPhoneNumber.Text & "',AddressOne = '" & txtAddress1.Text & "',AddressTwo = '" & txtAddress2.Text & "',Email = '" & txtEmail.Text & "' where Contact_ID = '" & objSelectedContact.ContactID & "'"

        'Create Command Object
        Dim cmdUpdate As New SqlCommand(strSQL, dbConnection)

        Try
            Dim intRowsImpacted = cmdUpdate.ExecuteNonQuery()
            If intRowsImpacted = 1 Then
                MessageBox.Show(objSelectedContact.FullName & " was updated.")
            Else
                MessageBox.Show("The update failed")
            End If
        Catch ex As Exception
            MessageBox.Show("DB Update Failed" & ex.Message)

        End Try

        Reload_lbxContacts()
        ResetFields()
    End Sub

    Public Sub DeleteContact()

        'Open Database
        Dim dbConnection As SqlConnection = OpenDBConnection()

        'Create SQL String
        Dim strSQL = "Delete from Contact_Tbl where Contact_ID = '" & objSelectedContact.ContactID & "'"

        'Create Command Object
        Dim cmdDelete As New SqlCommand(strSQL, dbConnection)

        Try
            Dim intRowsImpacted = cmdDelete.ExecuteNonQuery()
            If intRowsImpacted = 1 Then
                MessageBox.Show("Record was deleted.")
            Else
                MessageBox.Show("The delete failed")
            End If
        Catch ex As Exception
            MessageBox.Show("DB delete Failed" & ex.Message)

        End Try

        Reload_lbxContacts()
        ResetFields()

    End Sub

    Private Sub Reload_lbxContacts()

        'Make sure the list box is empty by clearing the list box
        lstContact.Clear()

        'Open Database
        Dim dbConnection As SqlConnection = OpenDBConnection()

        'Create a Command Object
        Dim cmdSelect As New SqlCommand("Select * from Contact_Tbl Order by FirstName,LastName;", dbConnection)

        'Execute Command into a DataReader
        Dim rdrContact As SqlDataReader = cmdSelect.ExecuteReader

        If rdrContact.HasRows Then
            While rdrContact.Read
                'Create temporary Contact Object to store from database to list of Contacts
                Dim objStoredContact As New clsContacts(rdrContact.Item("Contact_ID").ToString)
                objStoredContact.FirstName = rdrContact.Item("FirstName").ToString
                objStoredContact.LastName = rdrContact.Item("LastName").ToString
                objStoredContact.PhoneNumber = rdrContact.Item("PhoneNumber").ToString
                objStoredContact.AddressOne = rdrContact.Item("AddressOne").ToString
                objStoredContact.AddressTwo = rdrContact.Item("AddressTwo").ToString
                objStoredContact.Email = rdrContact.Item("Email").ToString


                'Populating list with data from database
                lstContact.Add(objStoredContact)
            End While
        End If

        dbConnection.Close()
        dbConnection.Dispose()
    End Sub

End Class
