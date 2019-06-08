/********************************************************************************************
                                                               *
 * Purpose  :   Create a small application dealing with passwords, to create accounts with user IDs and passwords
 *              This program is to load the values from a file, print the values in a list, *
 *              add an entry, load images from a folder. Creating new employee with option  *
 *              to upload images.                                                           *
 *                                                                                          *
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Assign5
{
    public partial class Form1 : Form
    {
        public static List<Person> personsList = new List<Person>();            //personList of type Person
        public static List<Person> addPersonsList = new List<Person>();         //personList of type Person
        public static List<Logins> loginsList = new List<Logins>();             // List to store values from file
        //
        //To intialize the form
        //
        public Form1()
        {
            InitializeComponent();
        }
        //
        //To load the main form and read the values from the file
        //
        private void Form1_Load(object sender, EventArgs e)
        {
            HidePanels();
            LoginPanel.Visible = true;
            LoginPasswordTextBox.Focus();
            LoadLogins(loginsList);
            LoadDetails(personsList);
        }
        //
        //Login_Click event to log into the application
        //
        private void LoginButton_Click(object sender, EventArgs e)
        {
            int flag = 0;
            string loginEntered = LoginUsernameTextBox.Text;
            string passwordEntered = EncryptDecrypt(LoginPasswordTextBox.Text);         // Encrypt the entered password
            if (loginEntered != "" && passwordEntered != "")                    // Check if the values are entered into the Username and Password fields
            {
                for (int i = 0; i < loginsList.Count; i++)
                {
                    if (loginsList[i].FindUsername(loginEntered) && loginsList[i].FindPassword(passwordEntered))        // Check if the Username and Passwords match
                    {
                        flag = 1;
                    }
                }
                if (flag == 1)                                          // If matched enter into the User's profile
                {
                    HidePanels();
                    MainPanel.Visible = true;
                }
                else                                                    // If not return error message
                {
                    MessageBox.Show("Username and password doesn't match");
                    ClearTextBoxes();
                }
            }
            else
            {
                MessageBox.Show("Complete all the fields");
            }
        }
        //
        //DetailsButton_Click event to open the employee details panel
        //
        private void DetailsButton_Click(object sender, EventArgs e)
        {
            HidePanels();
            DetailsPanel.Visible = true;
            SideListBox.Items.Clear();
            DisplayList(personsList);
        }
        //
        //SideListBox_SelectedIndexChanged event to get the current selected item
        //
        private void SideListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the currently selected item in the ListBox.
            string curItem = SideListBox.SelectedItem.ToString();
            Console.WriteLine(curItem);
            DisplayDetails(curItem, personsList);
        }
        //
        //AddButton_Click event to open the Addemployee panel
        //
        private void AddButton_Click(object sender, EventArgs e)
        {
            HidePanels();
            AddEmployeePanel.Visible = true;
            ClearTextBoxes();
        }
        //
        //UploadButton_Click event to upload the image
        //
        private void UploadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Images Only. |*.jpg; *.png; *.jpeg; *.gif";
            DialogResult dr = dialog.ShowDialog();
            UploadPictureBox.Image = Image.FromFile(dialog.FileName);                           // Upload the file
            if (dialog.FileName != string.Empty)
            {
                if (File.Exists(dialog.FileName))
                {
                    try
                    {
                        string filename = AddFirstTextBox.Text + " " + AddLastTextBox.Text;
                        String path = Directory.GetCurrentDirectory();
                        File.Copy(dialog.FileName, path + "\\pictures\\" + filename + ".jpg");          // Save that file in your local machine
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Exception" + ex);
                    }
                }
            }
        }
        //
        //AddEmployeebutton_Click event to add new employee to list
        //
        private void AddEmployeebutton_Click(object sender, EventArgs e)
        {
            addPersonsList = personsList;
            string firstname = AddFirstTextBox.Text;
            string lastname = AddLastTextBox.Text;
            string dob = AddDOBTextBox.Text;
            string gender = AddGenderTextBox.Text;
            string job = AddJobTextBox.Text;
            string street = AddStreetTextBox.Text;
            string city = AddCityTextBox.Text;
            string state = AddStateTextBox.Text;
            string emailid = AddEmailidTextBox.Text;
            string phone = AddPhoneTextBox.Text;
            if (firstname != "" && lastname != "" && dob != "" && gender != "" && job != "" && street != "" && city != "" && state != "" && emailid != "" && phone != "")                // Check if all values are entered
            {
                WriteDetails(addPersonsList, firstname, lastname, dob, gender, job, street, city, state, emailid, phone);
                personsList = addPersonsList;
                MessageBox.Show("New entry successful");
                ClearTextBoxes();
                HidePanels();
                MainPanel.Visible = true;
            }
            else
            {
                MessageBox.Show("Complete all the fields");
            }
        }
        //
        //ExitButton_Click event to exit the application
        //
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //
        //CancelButton_Click event to cancel the adding new employee
        //
        private void CancelButton_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            HidePanels();
            MainPanel.Visible = true;
        }
        //
        //ClearButton_Click event to clear text boxes
        //
        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
        }
        //
        //HomeButton_Click event to open the Main panel
        //
        private void HomeButton_Click(object sender, EventArgs e)
        {
            HidePanels();
            MainPanel.Visible = true;
        }
        //
        //Method for opening the file and loading the values into the list
        //
        private void LoadLogins(List<Logins> list)
        {
            String path = Directory.GetCurrentDirectory();
            //Reads values from the file
            using (StreamReader SR = new StreamReader(path + "\\logins.txt"))           // StreamReader to read values from file
            {
                int index = 0;
                Char delimiter = ',';
                Logins logins = new Logins();
                String S = SR.ReadLine();
                while (S != null)
                {
                    index++;
                    String[] sub = S.Split(delimiter);
                    if (index == 1)
                    {
                        //logins.Username = sub[index-1]; 
                        logins.Username = "admin"; // Takes first value as Username
                    }
                    else if (index == 2)
                    {
                        //logins.Password = sub[index-1];                                 // Takes second values as Password
                        logins.Password = "123456";
                        list.Add(new Logins(logins.Username, logins.Password));         // Adds the new values to logins list
                        index = 0;
                        logins = new Logins();
                        S = SR.ReadLine();
                    }
                }
            }
        }
        //
        //Method for opening the file and loading the values into the list
        //
        private void LoadDetails(List<Person> list)
        {
            String path = Directory.GetCurrentDirectory();
            //Reads values from the file
            using (StreamReader SR = new StreamReader(path + "\\details.txt"))           // StreamReader to read values from file
            {
                int index = 0;
                Char delimiter = ',';
                Person persons = new Person();
                String S = SR.ReadLine();
                while (S != null)
                {
                    index++;
                    String[] sub = S.Split(delimiter);
                    if (index == 1)
                    {
                        persons.Firstname = sub[index - 1];                             // Takes first value as Firstname
                    }
                    else if (index == 2)
                    {
                        persons.Lastname = sub[index - 1];                              // Takes second value as Lastname
                    }
                    else if (index == 3)
                    {
                        persons.Dob = sub[index - 1];                                   // Takes third value as Dob
                    }
                    else if (index == 4)
                    {
                        persons.Gender = sub[index - 1];                                // Takes fourth value as Gender
                    }
                    else if (index == 5)
                    {
                        persons.Job = sub[index - 1];                                   // Takes fifth value as Job
                    }
                    else if (index == 6)
                    {
                        persons.Street = sub[index - 1];                                // Takes sixth value as Street
                    }
                    else if (index == 7)
                    {
                        persons.City = sub[index - 1];                                  // Takes seventh value as City
                    }
                    else if (index == 8)
                    {
                        persons.State = sub[index - 1];                                 // Takes eigth value as State
                    }
                    else if (index == 9)
                    {
                        persons.Emailid = sub[index - 1];                               // Takes ninth value as Emailid
                    }
                    else if (index == 10)
                    {
                        persons.Telephone = sub[index - 1];                             // Takes tenth values as Telephone
                        list.Add(new Person(persons.Firstname, persons.Lastname, persons.Dob, persons.Gender, persons.Job, persons.Street, persons.City, persons.State, persons.Emailid, persons.Telephone));         // Adds the new values to logins list
                        index = 0;
                        persons = new Person();
                        S = SR.ReadLine();
                    }
                }
            }
        }
        //
        //Method for displaying the output in specified manner
        //
        private void DisplayList(List<Person> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                SideListBox.Items.Add(list[i].Firstname+" "+list[i].Lastname);
            }
        }
        //
        //Method for displaying the employee details in specified manner
        //
        private void DisplayDetails(String name, List<Person> list)
        {
            String path = Directory.GetCurrentDirectory();
            string listName = "";
            for (int i = 0; i < list.Count; i++)
            {
                listName = list[i].Firstname +" "+ list[i].Lastname;
                if(name == listName)
                {
                    Console.WriteLine(name + list[i].Dob, list[i].Gender, list[i].Job);
                    NameTextBox.Text = name;
                    DOBTextBox.Text = list[i].Dob;
                    GenderTextBox.Text = list[i].Gender;
                    JobTextBox.Text = list[i].Job;
                    StreetTextBox.Text = list[i].Street;
                    CityTextBox.Text = list[i].City;
                    StateTextBox.Text = list[i].State;
                    EmailidTextBox.Text = list[i].Emailid;
                    PhoneTextBox.Text = list[i].Telephone;
                    PictureBox.Load(path +"\\pictures\\"+listName+".jpg");
                }
            }
        }
        //
        //Method for writing details into list
        //
        private void WriteDetails(List<Person> list, String firstname, String lastname, String dob, String gender, String job, String street, String city, String state, String emailid, String phone)
        {
            Person persons = new Person();
            persons.Firstname = firstname;
            persons.Lastname = lastname;
            persons.Dob = dob;
            persons.Gender = gender;
            persons.Job = job;
            persons.Street = street;
            persons.City = city;
            persons.State = state;
            persons.Emailid = emailid;
            persons.Telephone = phone;
            if (firstname != "" && lastname != "" && dob != "" && job != "" && street != "" && city != "" && state != "" && emailid != "" && phone != "")
            {
                list.Add(new Person(persons.Firstname, persons.Lastname, persons.Dob, persons.Gender, persons.Job, persons.Street, persons.City, persons.State, persons.Emailid, persons.Telephone));         //the new entry is added to the list
                Writefile(list);
            }
            else
            {
                MessageBox.Show("Complete all the fields");
            }
        }
        //
        //Method for writing the new values into the file
        //
        private void Writefile(List<Person> list)
        {
            String path = Directory.GetCurrentDirectory();
            using (TextWriter TW = new StreamWriter(path + "\\details.txt"))
            {                                                                               //Textwriter writes the values into the data file
                for (int k = 0; k < list.Count; k++)
                {
                    TW.WriteLine(list[k].Firstname + "," + list[k].Lastname + "," + list[k].Dob + "," + list[k].Gender + "," + list[k].Job+"," + list[k].Street+","+ list[k].City+","+ list[k].State+","+ list[k].Emailid+","+ list[k].Telephone);
                }
            }
        }
        //
        //Method to hide panels. This method contains statements which are repeatedly used. So instead of repeating the statements, created a method.
        //
        private void HidePanels()
        {
            LoginPanel.Visible = false;
            MainPanel.Visible = false;
            DetailsPanel.Visible = false;
            AddEmployeePanel.Visible = false;
        }
        //
        //Method to clear text boxes in the form
        //
        private void ClearTextBoxes()
        {
            Action<Control.ControlCollection> func = null;
            func = (controls) =>                                //lambda function
            {
                foreach (Control control in controls)
                    if (control is TextBox)
                        (control as TextBox).Clear();           //clears the text boxes
                    else
                        func(control.Controls);
            };
            func(Controls);
        }
        //
        //XOR encryption to store the passwords in encrypted format and decrypt passwords when necessary
        //
        private static string EncryptDecrypt(string input)
        {
            char[] key = { 'T', 'A', 'R', 'U', 'N' };                         //Any chars will work, in an array of any size
            char[] output = new char[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                output[i] = (char)(input[i] ^ key[i % key.Length]);
            }

            return new string(output);
        }
    }
    //
    //Class person to implement the accesor methods
    //
    public class Person
    {
        String firstname, lastname, dob, gender, job, street, city, state, emailid, telephone;

        //Accessor methods
        public String Firstname
        {
            get { return firstname; }
            set { firstname = value; }
        }

        public String Lastname
        {
            get { return lastname; }
            set { lastname = value; }
        }

        public String Dob
        {
            get { return dob; }
            set { dob = value; }
        }

        public String Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public String Job
        {
            get { return job; }
            set { job = value; }
        }

        public String Street
        {
            get { return street; }
            set { street = value; }
        }

        public String City
        {
            get { return city; }
            set { city = value; }
        }

        public String State
        {
            get { return state; }
            set { state = value; }
        }

        public String Emailid
        {
            get { return emailid; }
            set { emailid = value; }
        }

        public String Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }
        //Default constructor
        public Person()
        {
            firstname = "";
            lastname = "";
            dob = "";
            gender = "";
            job = "";
            street = "";
            city = "";
            state = "";
            emailid = "";
            telephone = "";
        }
        //Constructor to intiliaze the values of firstname, lastname, dob, job, street, city, state, emailid, telephone
        public Person(String Firstname, String Lastname, String Dob, String Gender, String Job, String Street, String City, String State, String Emailid, String Telephone)
        {
            firstname = Firstname;
            lastname = Lastname;
            dob = Dob;
            gender = Gender;
            job = Job;
            street = Street;
            city = City;
            state = State;
            emailid = Emailid;
            telephone = Telephone;
        }

        public bool FindFirstname(string s)                 //It compares the name passed in the arguments with name in the object and returns true if found 
        {
            return Firstname.ToLower().Equals(s.ToLower());
        }
        public bool FindLastname(string s)                 //It compares the name passed in the arguments with name in the object and returns true if found 
        {
            return Lastname.ToLower().Equals(s.ToLower());
        }
        public bool FindDob(string s)                 //It compares the name passed in the arguments with name in the object and returns true if found 
        {
            return Dob.ToLower().Equals(s.ToLower());
        }
        public bool FindJob(string s)                 //It compares the name passed in the arguments with name in the object and returns true if found 
        {
            return Job.ToLower().Equals(s.ToLower());
        }
        public bool FindStreet(string s)                 //It compares the name passed in the arguments with name in the object and returns true if found 
        {
            return Street.ToLower().Equals(s.ToLower());
        }
        public bool FindCity(string s)                 //It compares the name passed in the arguments with name in the object and returns true if found 
        {
            return City.ToLower().Equals(s.ToLower());
        }
        public bool FindState(string s)                 //It compares the name passed in the arguments with name in the object and returns true if found 
        {
            return State.ToLower().Equals(s.ToLower());
        }
        public bool FindEmailid(string s)                 //It compares the name passed in the arguments with name in the object and returns true if found 
        {
            return Emailid.ToLower().Equals(s.ToLower());
        }
        public bool FindTelephone(string s)             //It compares the Phone Number passed in the arguments with Phone Number in the object
        {
            return Telephone.ToLower().Equals(s.ToLower());
        }
    }
    //
    //Class Logins to implement the accesor methods
    //
    public class Logins
    {
        String username, password;
        //Accessor methods
        public String Username
        {
            get { return username; }
            set { username = value; }
        }
        public String Password
        {
            get { return password; }
            set { password = value; }
        }
        //Default constructor
        public Logins()
        {
            username = "";
            password = "";
        }
        //Constructor to intiliaze the values of Username and Password
        public Logins(String Username, String Password)
        {
            username = Username;
            password = Password;
        }
        public bool FindUsername(string s)                 //It compares the Username passed in the arguments with Username in the object and returns username if found 
        {
            return Username.ToLower().Equals(s.ToLower());
        }
        public bool FindPassword(string s)               //It compares the Password passed in the arguments with Password in the object and returns password if found 
        {
            return Password.ToLower().Equals(s.ToLower());
        }
    }
}
