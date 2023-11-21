using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace Reading_a_File
{
    public partial class FrmRegistration : Window
    {
        StudentInformationClass StudentInformationClass;
        long _StudentNo;
        long _ContactNo;
        string _FullName;
        int _Age;

        public FrmRegistration()
        {
            InitializeComponent();
            StudentInformationClass = new StudentInformationClass();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] ListOfProgram = new string[]
            {
                "BS Information Technology",
                "BS Computer Science",
                "BS Information System",
                "BS in Accountancy",
                "BS in Hospitality Management",
                "BS in Tourism Management"
            };
            for (int i = 0; i < 6; i++)
            {
                cbPrograms.Items.Add(ListOfProgram[i].ToString());
            }

            string[] ListOfGender = new string[]
            {
                "Male",
                "Female",
                "Non-Binary"
            };
            for (int g = 0; g < 3; g++)
            {
                cbGender.Items.Add(ListOfGender[g].ToString());
            }
        }

        public class InvalidStudentNumberException : Exception
        {
            public InvalidStudentNumberException() : base("Invalid student number format.")
            {
            }
        }

        public class InvalidContactNumberException : Exception
        {
            public InvalidContactNumberException() : base("Invalid contact number format.")
            {
            }
        }

        public class InvalidAgeFormatException : Exception
        {
            public InvalidAgeFormatException() : base("Invalid age format.")
            {
            }
        }

        public class InvalidNameFormatException : Exception
        {
            public InvalidNameFormatException() : base("Invalid name format.")
            {
            }
        }

        private long StudentNumber(string studNum)
        {
            try
            {
                _StudentNo = long.Parse(studNum);
                return _StudentNo;
            }
            catch (FormatException)
            {
                throw new InvalidStudentNumberException();
            }
        }

        private long ContactNo(string Contact)
        {
            try
            {
                if (Regex.IsMatch(Contact, @"^[0-9]{10,11}$"))
                {
                    _ContactNo = long.Parse(Contact);
                }
                return _ContactNo;
            }
            catch (FormatException)
            {
                throw new InvalidContactNumberException();
            }
        }

        private string FullName(string LastName, string FirstName, string MiddleInitial)
        {
            if (Regex.IsMatch(LastName, @"^[a-zA-Z]+$") || Regex.IsMatch(FirstName, @"^[a-zA-Z]+$") || Regex.IsMatch(MiddleInitial, @"^[a-zA-Z]+$"))
            {
                _FullName = LastName + ", " + FirstName + ", " + MiddleInitial;
            }

            return _FullName;
        }

        private int Age(string age)
        {
            try
            {
                if (Regex.IsMatch(age, @"^[0-9]{1,3}$"))
                {
                    _Age = int.Parse(age);
                    return _Age;
                }
                throw new InvalidAgeFormatException();
            }
            catch (FormatException)
            {
                throw new InvalidAgeFormatException();
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StudentInformationClass.SetFullName = FullName(txtLastName.Text, txtFirstName.Text, txtMiddleInitial.Text);
                StudentInformationClass.SetStudentNo = StudentNumber(txtStudentNo.Text);
                StudentInformationClass.SetProgram = cbPrograms.Text;
                StudentInformationClass.SetGender = cbGender.Text;
                StudentInformationClass.SetContactNo = ContactNo(TxtContactNo.Text);
                StudentInformationClass.SetAge = Age(txtAge.Text);
                StudentInformationClass.SetBirthday = datePickerBirthday.SelectedDate?.ToString("yyyy-MM-dd");

                string fileName = $"{StudentInformationClass.SetStudentNo}.txt";

                string[] studentInfo = {
                    $"Student No. : {StudentInformationClass.SetStudentNo}",
                    $"Full Name : {StudentInformationClass.SetFullName}",
                    $"Program : {StudentInformationClass.SetProgram}",
                    $"Gender: {StudentInformationClass.SetGender}",
                    $"Age : {StudentInformationClass.SetAge}",
                    $"Birthday : {StudentInformationClass.SetBirthday}",
                    $"Contact No. : {StudentInformationClass.SetContactNo}"
                };

                string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (string line in studentInfo)
                    {
                        writer.WriteLine(line);
                    }
                }

                MessageBox.Show("The Student Is Registered, And The Text File Is Saved In Your Documents Folder", "Save Path", MessageBoxButton.OK, MessageBoxImage.Information);

                txtLastName.Clear();
                txtFirstName.Clear();
                txtMiddleInitial.Clear();
                txtStudentNo.Clear();
                cbPrograms.SelectedIndex = -1;
                cbGender.SelectedIndex = -1;  
                TxtContactNo.Clear();
                txtAge.Clear();
                datePickerBirthday.SelectedDate = null;

            }
            catch (InvalidStudentNumberException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            catch (InvalidContactNumberException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            catch (InvalidAgeFormatException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            catch (InvalidNameFormatException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void BtnBrowse_OnClick(object sender, RoutedEventArgs e)
        {
            FrmStudentRecord frmStudentRecord = new FrmStudentRecord();
            frmStudentRecord.Show();
            this.Close();
        }
    }

    public class StudentInformationClass
    {
        public long SetStudentNo = 0;
        public long SetContactNo = 0;
        public string SetProgram = " ";
        public string SetGender = " ";
        public string SetBirthday = " ";
        public string SetFullName = " ";
        public int SetAge = 0;
    }
}

