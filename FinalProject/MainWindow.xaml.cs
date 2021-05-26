using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace FinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static int tbDiseaseCounter = 0;
        static int idCounter = 1;
        TextBox textBox;
        List<Patient> patientList = new List<Patient>();
        List<Doctor> doctorList = new List<Doctor>();
        List<Appointment> appointmentList = new List<Appointment>();
        string patientName = null;
        string patientId = null;
        string patientAddress = null;
        string patientContactNumber = null;
        int ageInt = 0;
        string patientHeight = null;
        double patientHeightDouble = 0.0;
        string patientWeight = null;
        double patientWeightDouble = 0.0;
        bool sundayFlag, mondayFlag, tuesdayFlag, wednesdayFlag, thursdayFlag, fridayFlag, saturdayFlag = false;
        static int doctorIdCounter = 1;
        string doctorID = null;
        string doctorName = null;
        string doctorDepartmentName = null;
        string doctorSpeciality = null;

        public MainWindow()
        {
            InitializeComponent();
            
            
        }

        public void FilterDoctorList()
        {
            List<Doctor> filteredDoctor = new List<Doctor>();
            if(appointmentDate.SelectedDate.HasValue)
            {
                int selectedDay = (int)appointmentDate.SelectedDate.Value.DayOfWeek;
                foreach (Doctor d in doctorList)
                {
                    foreach (DoctorTiming dt in d.TimingList)
                    {
                        if (dt.Day == selectedDay)
                        {
                            filteredDoctor.Add(d);
                            break;
                        }
                    }
                }

                if (comboAppointmentDepartment.SelectedIndex != 0)
                {
                    
                    List<Doctor> tempList = filteredDoctor.ToList<Doctor>();
                    filteredDoctor.Clear();
                    foreach (Doctor d in tempList)
                    {
                        if (d.Department.Equals(((ComboBoxItem)comboAppointmentDepartment.SelectedItem).Content.ToString()))
                        {
                            filteredDoctor.Add(d);
                        }
                    }

                }

                p3DoctorList.ItemsSource = null;
                p3DoctorList.ItemsSource = filteredDoctor;
                p3DoctorList.DisplayMemberPath = "Name";
                p3DoctorList.SelectedValuePath = "Id";
            }

            else
            {
                MessageBox.Show("Please Select A Date", "Error");
            }
                
            
            
        }


        private void idSearch_Click(object sender, RoutedEventArgs e)
        {
            if(p1PatientId.Text.Equals(""))
            {
                
            }
            else
            {
                var query = from patient in patientList
                            where patient.Id.Equals(p1PatientId.Text.ToString().ToUpper())
                            select patient;
                if(query.Count() == 0)
                {
                    MessageBox.Show("No Patient Found");
                    p1PatientName.Text = "";
                    p1PatientAddress.Text = "";
                    p1PatientNumber.Text = "";
                    p1PatientGender.Text = "";
                    p1PatientAge.Text = "";
                    p1PatientHeight.Text = "";
                    p1PatientWeight.Text = "";
                    p1PatientPastHistory.Text = "";
                    p1PatientExtraInformation.Text = "";
                    patientRecord.Text = "";
                }
                foreach(Patient patient in query)
                {
                    p1PatientName.Text = patient.Name;
                    p1PatientAddress.Text = patient.Address;
                    p1PatientNumber.Text = patient.ContactNumber;
                    p1PatientGender.Text = patient.Gender;
                    p1PatientAge.Text = patient.Age.ToString();
                    p1PatientHeight.Text = patient.Height.ToString() + " CM";
                    p1PatientWeight.Text = patient.Weight.ToString() + " KG";
                    p1PatientPastHistory.Text = "";
                    foreach(String history in patient.DiseasesInPast)
                    {
                        p1PatientPastHistory.Text = p1PatientPastHistory.Text+"\n"+history;
                    }
                    p1PatientExtraInformation.Text = patient.OtherImpInformation;
                    patientRecord.Text = "";
                    foreach (String instruction in patient.InstructionGiven)
                    {
                        patientRecord.Text = patientRecord.Text + instruction + "\n";
                    }
                }
            }
        }

        private void BtnAddNewDieases_Click(object sender, RoutedEventArgs e)
        {
            textBox = new TextBox();
            textBox.Name = "tbPatientDiseases" + tbDiseaseCounter.ToString();
            textBox.Width = 300;
            textBox.Height = 20;
            textBox.Margin = new Thickness(0.0, 0.0, 0.0, 10);
            stackPanelPatientDiseases.Children.Add(textBox);
            tbDiseaseCounter++;



            if (stackPanelPatientDiseases.Children.Count > 0)
            {
                btnRemoveDiesease.Visibility = Visibility.Visible;
            }


        }

        private void BtnRemoveDiesease_Click(object sender, RoutedEventArgs e)
        {
            if (stackPanelPatientDiseases.Children.Count > 0)
            {
                stackPanelPatientDiseases.Children.RemoveAt(stackPanelPatientDiseases.Children.Count - 1);
            }


            if (stackPanelPatientDiseases.Children.Count == 0)
            {
                btnRemoveDiesease.Visibility = Visibility.Collapsed;
            }

        }

        private void BtnAddNewPatient_Click(object sender, RoutedEventArgs e)
        {
            if (ValidatePatientInformation())
            {
                Patient patient = new Patient();


                patient.Id = patientId;
                patient.Name = patientName;
                patient.Address = patientAddress;
                patient.ContactNumber = patientContactNumber;
                ComboBoxItem gender = (ComboBoxItem)cbGender.SelectedItem;
                patient.Gender = gender.Content.ToString();
                patient.Age = ageInt;
                patient.Height = patientHeightDouble;
                patient.Weight = patientWeightDouble;

                if (stackPanelPatientDiseases.Children.Count > 0)
                {
                    List<string> diseasesInPast = new List<string>();

                    foreach (UIElement uIElement in stackPanelPatientDiseases.Children)
                    {
                        if (uIElement is TextBox)
                        {
                            TextBox textBox = uIElement as TextBox;

                            diseasesInPast.Add(textBox.Text);
                        }
                    }
                    patient.DiseasesInPast = diseasesInPast;

                }
                if (tbPatientImportantInstruction.Text != string.Empty)
                {
                    patient.OtherImpInformation = tbPatientImportantInstruction.Text;
                }
                patientList.Add(patient);
                MessageBox.Show("New Patient Added Successfully...\n All details will be send to you by email ");

                tbPatientId.Text = string.Empty;
                tbPatientName.Text = string.Empty;
                tbPatientAddress.Text = string.Empty;
                tbPatientContactNumber.Text = string.Empty;
                tbAge.Text = string.Empty;
                tbPatientHeight.Text = string.Empty;
                tbPatientWeight.Text = string.Empty;
                tbPatientImportantInstruction.Text = string.Empty;

                if (stackPanelPatientDiseases.Children.Count > 0)
                {
                    stackPanelPatientDiseases.Children.Clear();
                    btnRemoveDiesease.Visibility = Visibility.Collapsed;


                }
                idCounter++;
                TabItemAddNewPatient_Loaded(sender, e);
            }
        }

        private bool ValidatePatientInformation()
        {
            patientName = tbPatientName.Text;
            if (patientName == string.Empty)
            {
                MessageBox.Show("Please enter Patient Name");
                return false;
            }

            if (int.TryParse(patientName, out int patientNameInNumber))
            {
                MessageBox.Show("Patient Name Cannot be a Number");
                tbPatientName.Text = string.Empty;
                return false;
            }

            patientAddress = tbPatientAddress.Text;
            if (patientAddress == string.Empty)
            {
                MessageBox.Show("Please enter Patient Address");
                return false;
            }


            patientContactNumber = tbPatientContactNumber.Text.Trim();

            if (patientContactNumber == string.Empty)
            {
                MessageBox.Show("Patient Contact Number should not be empty ");
                return false;
            }

            bool isDigitNumber = patientContactNumber.All(char.IsDigit);

            if (isDigitNumber)
            {
                if (patientContactNumber.Length != 10)
                {
                    MessageBox.Show("Number should be 10 digits only ");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Contact Number should not have any non numeric character");
                return false;
            }
            //Gender to be done

            string age = tbAge.Text;
            if (age == string.Empty)
            {
                MessageBox.Show("Please enter your age");
                return false;
            }

            if (int.TryParse(age, out ageInt))
            {
                if (ageInt <= 0)
                {
                    MessageBox.Show("Age should not be less than  or equal to 0");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Age should be number only");
                return false;
            }

            patientHeight = tbPatientHeight.Text;
            if (patientHeight == string.Empty)
            {
                MessageBox.Show("Please Enter Patient Height");
                return false;
            }

            if (double.TryParse(patientHeight, out patientHeightDouble))
            {
                if (patientHeightDouble <= 0.0)
                {
                    MessageBox.Show("Height should not be equal or less than 0 ");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Height should be number only");
                return false;
            }

            patientWeight = tbPatientWeight.Text;
            if (patientWeight == string.Empty)
            {
                MessageBox.Show("Weight should not be empty");
                return false;
            }

            if (double.TryParse(patientWeight, out patientWeightDouble))
            {
                if (patientWeightDouble <= 0.0)
                {
                    MessageBox.Show("Weight should not be equal or less than 0");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Weight should be number only");
                return false;
            }

            int counter = 0;
            foreach (UIElement uiElement in stackPanelPatientDiseases.Children)
            {
                TextBox textBox = uiElement as TextBox;
                string value = textBox.Text;
                if (value == string.Empty)
                {
                    counter++;
                }

            }
            if (counter != 0)
            {
                MessageBox.Show("Please Enter Empty field/fields");
                return false;
            }


            return true;
        }


        private void Cb_Loaded(object sender, RoutedEventArgs e)
        {

            if (sender is ComboBox)
            {
                ComboBox comboBox = sender as ComboBox;
                comboBox.Items.Clear();
                for (int i = 0; i < 24; i++)
                {
                    comboBox.Items.Add(i.ToString("00"));
                }
                if (comboBox.IsEnabled)
                {
                    comboBox.SelectedIndex = 0;

                }

            }
        }

        private void CbSunday_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                if (cbSunday.IsChecked == true)
                {

                    cbSundayFrom.IsEnabled = true;
                    cbSundayFrom.SelectedIndex = 9;
                    cbSundayTo.IsEnabled = true;
                    cbSundayTo.SelectedIndex = 17;
                    sundayFlag = true;
                }
                else
                {

                    cbSundayFrom.IsEnabled = false;
                    cbSundayFrom.SelectedIndex = -1;
                    cbSundayTo.IsEnabled = false;
                    cbSundayTo.SelectedIndex = -1;
                    sundayFlag = false;

                }
            }
        }

        private void BtnAddNewDoctor_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateDoctorDetails())
            {
                Doctor doctor = new Doctor();



                doctor.Id = doctorID;
                doctor.Name = doctorName;
                doctor.Department = doctorDepartmentName;
                doctor.Specialist = doctorSpeciality;

                List<DoctorTiming> doctorTimingList = new List<DoctorTiming>();
                //DoctorTiming doctorTiming = new DoctorTiming();

                if (sundayFlag == true)
                {
                    DoctorTiming doctorTiming = new DoctorTiming();
                    doctorTiming.Day = 0;

                    doctorTiming.StartTime = cbSundayFrom.SelectedItem.ToString();
                    doctorTiming.EndTme = cbSundayTo.SelectedItem.ToString();


                    if (int.Parse(doctorTiming.StartTime) < int.Parse(doctorTiming.EndTme))
                    {
                        doctorTimingList.Add(doctorTiming);
                    }
                    else
                    {
                        MessageBox.Show("Sunday's From time should be less than To time");
                        return;
                    }


                }


                if (mondayFlag == true)
                {
                    DoctorTiming doctorTiming = new DoctorTiming();
                    doctorTiming.Day = 1;

                    doctorTiming.StartTime = cbMondayFrom.SelectedItem.ToString();
                    doctorTiming.EndTme = cbModayTo.SelectedItem.ToString();

                    if (int.Parse(doctorTiming.StartTime) < int.Parse(doctorTiming.EndTme))
                    {
                        doctorTimingList.Add(doctorTiming);
                    }
                    else
                    {
                        MessageBox.Show("Monday's From time should be less than To time");
                        return;
                    }




                }


                if (tuesdayFlag == true)
                {
                    DoctorTiming doctorTiming = new DoctorTiming();
                    doctorTiming.Day = 2;

                    doctorTiming.StartTime = cbTuesdayFrom.SelectedItem.ToString();
                    doctorTiming.EndTme = cbTuesdayTo.SelectedItem.ToString();

                    if (int.Parse(doctorTiming.StartTime) < int.Parse(doctorTiming.EndTme))
                    {
                        doctorTimingList.Add(doctorTiming);
                    }
                    else
                    {
                        MessageBox.Show("Tuesday's From time should be less than To time");
                        return;
                    }
                }


                if (wednesdayFlag == true)
                {
                    DoctorTiming doctorTiming = new DoctorTiming();
                    doctorTiming.Day = 3;

                    doctorTiming.StartTime = cbWednesdayFrom.SelectedItem.ToString();
                    doctorTiming.EndTme = cbWednesdayTo.SelectedItem.ToString();

                    if (int.Parse(doctorTiming.StartTime) < int.Parse(doctorTiming.EndTme))
                    {
                        doctorTimingList.Add(doctorTiming);
                    }
                    else
                    {
                        MessageBox.Show("Wednesday's From time should be less than To time");
                        return;
                    }
                }

                if (thursdayFlag == true)
                {
                    DoctorTiming doctorTiming = new DoctorTiming();
                    doctorTiming.Day = 4;

                    doctorTiming.StartTime = cbThursdayFrom.SelectedItem.ToString();
                    doctorTiming.EndTme = cbThursdayTo.SelectedItem.ToString();

                    if (int.Parse(doctorTiming.StartTime) < int.Parse(doctorTiming.EndTme))
                    {
                        doctorTimingList.Add(doctorTiming);
                    }
                    else
                    {
                        MessageBox.Show("Thursday's From time should be less than To time");
                        return;
                    }
                }

                if (fridayFlag == true)
                {
                    DoctorTiming doctorTiming = new DoctorTiming();
                    doctorTiming.Day = 5;

                    doctorTiming.StartTime = cbFridayFrom.SelectedItem.ToString();
                    doctorTiming.EndTme = cbFridayTo.SelectedItem.ToString();

                    if (int.Parse(doctorTiming.StartTime) < int.Parse(doctorTiming.EndTme))
                    {
                        doctorTimingList.Add(doctorTiming);
                    }
                    else
                    {
                        MessageBox.Show("Friday's From time should be less than to time");
                        return;
                    }
                }

                if (saturdayFlag == true)
                {
                    DoctorTiming doctorTiming = new DoctorTiming();
                    doctorTiming.Day = 6;

                    doctorTiming.StartTime = cbSaturdayFrom.SelectedItem.ToString();
                    doctorTiming.EndTme = cbSaturdayTo.SelectedItem.ToString();

                    if (int.Parse(doctorTiming.StartTime) < int.Parse(doctorTiming.EndTme))
                    {
                        doctorTimingList.Add(doctorTiming);
                    }
                    else
                    {
                        MessageBox.Show("Saturday's From time should be less than to time");
                        return;
                    }
                }

                doctor.TimingList = doctorTimingList;
                doctorList.Add(doctor);
                doctorIdCounter++;
                MessageBox.Show("New Doctor Added Succesfully....");
                UpdateDashboard();
                tbDoctorId.Text = string.Empty;
                tbDoctorName.Text = string.Empty;
                tbDoctorSpeciality.Text = string.Empty;

                cbSunday.IsChecked = false;
                CbSunday_Checked(sender, e);

                cbMonday.IsChecked = false;
                CbMonday_Checked(sender, e);

                cbTuesday.IsChecked = false;
                CbTuesday_Checked(sender, e);

                cbWednesday.IsChecked = false;
                CbWednesday_Checked(sender, e);

                cbThurday.IsChecked = false;
                CbThurday_Checked(sender, e);

                cbFriday.IsChecked = false;
                CbFriday_Checked(sender, e);

                cbSaturday.IsChecked = false;
                CbSaturday_Checked(sender, e);


                TabItemAddNewDoctor_Loaded(sender, e);

                foreach (Doctor doc in doctorList)
                {
                    Console.WriteLine("ID {0}", doc.Id);
                    Console.WriteLine("Name {0}", doc.Name);
                    Console.WriteLine("Department {0}", doc.Department);

                    foreach (DoctorTiming timing in doctorTimingList)
                    {
                        Console.WriteLine("Day {0}", timing.Day);
                        Console.WriteLine("Start {0}", timing.StartTime);
                        Console.WriteLine("End {0}", timing.EndTme);


                    }

                }





            }
        }

        private bool ValidateDoctorDetails()
        {
            doctorName = tbDoctorName.Text;
            if (doctorName == string.Empty)
            {
                MessageBox.Show("Please Enter Doctor Name");
                return false;
            }

            if (int.TryParse(doctorName, out int docorNameInt))
            {
                MessageBox.Show("Name should not be a number");
                return false;
            }

            ComboBoxItem doctorDepartmentItem = (ComboBoxItem)cbDoctorDepartment.SelectedItem;
            doctorDepartmentName = doctorDepartmentItem.Content.ToString();

            doctorSpeciality = tbDoctorSpeciality.Text;
            if (doctorSpeciality == string.Empty)
            {
                MessageBox.Show("Please enter doctor speciality");
                return false;
            }

            if (sundayFlag == false && mondayFlag == false && tuesdayFlag == false && wednesdayFlag == false && thursdayFlag == false && fridayFlag == false && saturdayFlag == false)
            {
                MessageBox.Show("Please select atleast one day");
                return false;
            }



            return true;
        }

        private void TabItemAddNewPatient_Loaded(object sender, RoutedEventArgs e)
        {

            patientId = "PAT" + idCounter.ToString("D5");
            tbPatientId.Text = patientId;



        }

        private void TabItemAddNewDoctor_Loaded(object sender, RoutedEventArgs e)
        {
            doctorID = "DOC" + doctorIdCounter.ToString("D5");
            tbDoctorId.Text = doctorID;

        }

        private void LbAllPatient_Loaded(object sender, RoutedEventArgs e)
        {
            lbAllPatient.Items.Clear();
            foreach (Patient patient in patientList)
            {
                if (!lbAllPatient.Items.Contains(patient.Id))
                {
                    lbAllPatient.Items.Add(patient.Id);
                }
            }

        }

        private void LbAllPatient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (lbAllPatient.Items.Count != 0)
            {
                string patientId = lbAllPatient.SelectedItem.ToString();

                var query = from patient in patientList
                            where patient.Id.Equals(patientId)
                            select patient;

                foreach (Patient patient in query)
                {
                    tbPatientManageId.Text = patient.Id;
                    tbPatientManageName.Text = patient.Name;
                    tbPatientManageAddress.Text = patient.Address;
                    tbPatientManageContact.Text = patient.ContactNumber;
                    tbPatientManageHeight.Text = patient.Height.ToString();
                    tbPatientManageWeight.Text = patient.Weight.ToString();
                }

            }

        }

        private void BtnPatientManageDete_Click(object sender, RoutedEventArgs e)
        {
            string id = tbPatientManageId.Text;

            var query = from patient in patientList
                        where patient.Id.Equals(id)
                        select patient;


            foreach (Patient patient in query)
            {
                if (patient.Id == id)
                {
                    patientList.Remove(patient);
                    MessageBox.Show("Patient Deleted");
                    break;
                }
            }

            LbAllPatient_Loaded(sender, e);
            tbPatientManageId.Text = string.Empty;
            tbPatientManageName.Text = string.Empty;
            tbPatientManageAddress.Text = string.Empty;
            tbPatientManageContact.Text = string.Empty;
            tbPatientManageHeight.Text = string.Empty;
            tbPatientManageWeight.Text = string.Empty;
        }
        
        

        private void btnSearchAppointmentPatientId_Click(object sender, RoutedEventArgs e)
        {
            if (txtAppointmentPatientId.Text.Equals(""))
            {
                
            }
            else
            {
                var query = from patient in patientList
                            where patient.Id.Equals(txtAppointmentPatientId.Text.ToString().ToUpper())
                            select patient;
                if (query.Count() == 0)
                {
                    MessageBox.Show("No Patient Found");
                    appointmentPatientName.Content = "";
                    appointmentPatientNumber.Content = "";
                    appointmentPatientGender.Content = "";

                }
                foreach (Patient patient in query)
                {
                    appointmentPatientName.Content = patient.Name;
                    appointmentPatientNumber.Content = patient.ContactNumber;
                    appointmentPatientGender.Content = patient.Gender;
                    
                }
            }
        }

        private void btnMenuAddPatient_Click(object sender, RoutedEventArgs e)
        {
            tabController.SelectedIndex = 1;
        }

        private void btnMenuManagePatient_Click(object sender, RoutedEventArgs e)
        {
            tabController.SelectedIndex = 2;
        }

        private void btnMenuAddAppointment_Click(object sender, RoutedEventArgs e)
        {
            tabController.SelectedIndex = 3;
        }

        private void btnMenuPatientDetails_Click(object sender, RoutedEventArgs e)
        {
            tabController.SelectedIndex = 4;
        }

        private void btnMenuAddDoctor_Click(object sender, RoutedEventArgs e)
        {
            tabController.SelectedIndex = 5;
        }

        private void btnMenuTimeTabel_Click(object sender, RoutedEventArgs e)
        {
            tabController.SelectedIndex = 6;
        }

       

        private void CbMonday_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                if (cbMonday.IsChecked == true)
                {
                    cbMondayFrom.IsEnabled = true;
                    cbMondayFrom.SelectedIndex = 9;
                    cbModayTo.IsEnabled = true;
                    cbModayTo.SelectedIndex = 17;
                    mondayFlag = true;
                }
                else
                {
                    cbMondayFrom.IsEnabled = false;
                    cbMondayFrom.SelectedIndex = -1;
                    cbModayTo.IsEnabled = false;
                    cbModayTo.SelectedIndex = -1;
                    mondayFlag = false;
                }
            }
        }

        private void comboAppointmentDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (appointmentDate.SelectedDate.HasValue == true)
            {
                FilterDoctorList();
            }
        }

        private void tbxinstruction_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxinstruction.Text = "";
        }

        private void btnAddInstuction_Click(object sender, RoutedEventArgs e)
        {
            if(p1PatientName.Text.Equals(""))
            {
                MessageBox.Show("Please enter patient Id");
            }
            else
            {
                var query = from patient in patientList
                            where patient.Id.Equals(p1PatientId.Text.ToString().ToUpper())
                            select patient;
                foreach(Patient p in query)
                {
                    p.InstructionGiven.Add(DateTime.Now.ToString("dd/MM/yyyy") + "  " + tbxinstruction.Text);
                }
                idSearch_Click(sender, e);
            }
        }

        private void p1PatientId_LostFocus(object sender, RoutedEventArgs e)
        {
            idSearch_Click(sender, e);
        }

        private void p3DoctorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(p3DoctorList.SelectedIndex!=-1)
            {
                Doctor selectedDoctor = (Doctor)p3DoctorList.SelectedItem;
                appointmentDoctorName.Content = selectedDoctor.Name;
                appointmentDoctorDay.Content = appointmentDate.SelectedDate.Value.DayOfWeek.ToString();
                appointmentOtherTiming.Items.Clear();
                appointmentOtherTiming.Items.Add("Other Days and timing");
                foreach(DoctorTiming dt in selectedDoctor.TimingList)
                {
                    if(dt.Day == (int)appointmentDate.SelectedDate.Value.DayOfWeek)
                    {
                        appointmentDoctorTime.Content = dt.StartTime + ":00 to " + dt.EndTme + ":00";
                    }
                    else
                    {
                        var enumDisplayStatus = (DayOfWeek)dt.Day;
                        string stringValue = enumDisplayStatus.ToString();
                        appointmentOtherTiming.Items.Add(stringValue + "   " + dt.StartTime + ":00 to " + dt.EndTme + ":00");
                        
                    }
                }
            }
            else
            {
                appointmentDoctorName.Content = "";
                appointmentDoctorDay.Content = "";
            }

        }

        private void addAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            if(appointmentPatientName.Content.Equals(""))
            {
                MessageBox.Show("Please select a valid patient details");
            }
            else if(appointmentDoctorName.Content.Equals(""))
            {
                MessageBox.Show("Please select a valid doctor details");
            }
            else
            {
                Appointment appointment = new Appointment();
                foreach(Patient p in patientList)
                {
                    if(p.Id.Equals(txtAppointmentPatientId.Text.ToUpper()))
                    {
                        appointment.patient = p;
                    }
                }
                appointment.doctor = (Doctor)p3DoctorList.SelectedItem;
                appointment.appointmentDate = appointmentDate.SelectedDate.Value;
                appointmentList.Add(appointment);
                UpdateDashboard();
                MessageBox.Show("Appointment Added");
            }
        }

        private void txtAppointmentPatientId_LostFocus(object sender, RoutedEventArgs e)
        {
            btnSearchAppointmentPatientId_Click(sender, e);
        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void tabController_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int tabItem = (sender as TabControl).SelectedIndex;
            Console.WriteLine(tabItem);
        }

        private void homePage_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        public void UpdateDashboard()
        {
            todayAppointment.Items.Clear();
            foreach(Appointment a in appointmentList)
            {
                if(a.appointmentDate.ToString("dd/MM/yyyy").Equals(DateTime.Now.ToString("dd/MM/yyyy")))
                {
                    todayAppointment.Items.Add(a.patient.Name + " for " + a.doctor.Name);
                }
            }
            todayDoctorVisit.Items.Clear();
            int todayDayCount = (int)DateTime.Now.DayOfWeek;
            foreach(Doctor d in doctorList)
            {
                foreach(DoctorTiming dt in d.TimingList)
                {
                    if(dt.Day == todayDayCount)
                    {
                        todayDoctorVisit.Items.Add(d.Name + " from " + dt.StartTime+":00 to "+dt.EndTme+":00");
                        break;
                    }
                }
            }
        }

        private void btnMenuLoadData_Click(object sender, RoutedEventArgs e)
        {
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreWhitespace = true;
            readerSettings.IgnoreComments = true;


            XmlReader reader = XmlReader.Create("Data.xml", readerSettings);

            if (reader.ReadToDescendant("Patient"))
            {
                do
                {
                    Patient patient = new Patient();
                    List<string> diseaseList = new List<string>();
                    List<string> informationGiven = new List<string>();


                    reader.ReadStartElement("Patient");
                    string id = reader.ReadElementContentAsString();
                    string name = reader.ReadElementContentAsString();
                    string address =  reader.ReadElementContentAsString();
                    string contact =  reader.ReadElementContentAsString();
                    string gender = reader.ReadElementContentAsString();
                    int age = int.Parse(reader.ReadElementContentAsString());
                    double height = double.Parse(reader.ReadElementContentAsString());
                    double weight = double.Parse(reader.ReadElementContentAsString());


                    if (reader.ReadToDescendant("DISEASE"))
                    {
                        do
                        {
                            reader.ReadStartElement("DISEASE");

                             diseaseList.Add(reader.ReadContentAsString());

                        } while (reader.ReadToNextSibling("DISEASE"));
                        patient.DiseasesInPast = diseaseList;
                        reader.ReadEndElement();


                    }

                    string importantInfo =  reader.ReadElementContentAsString();

                    if (reader.ReadToDescendant("INSTRUCTION"))
                    {
                        do
                        {
                            reader.ReadStartElement("INSTRUCTION");
                            informationGiven.Add(reader.ReadContentAsString());
                        } while (reader.ReadToNextSibling("INSTRUCTION"));
                        patient.InstructionGiven = informationGiven;
                        reader.ReadEndElement();
                    }

                    patient.Id = id;
                    patient.Name = name;
                    patient.Address = address;
                    patient.ContactNumber = contact;
                    patient.Gender = gender;
                    patient.Age = age;
                    patient.Height = height;
                    patient.Weight = weight;
                    patient.OtherImpInformation = importantInfo;

                    patientList.Add(patient);
                    


                } while (reader.ReadToNextSibling("Patient"));
                reader.ReadEndElement();


                if (reader.ReadToDescendant("Doctor"))
                {
                    do
                    {
                        List<DoctorTiming> doctorTimings = new List<DoctorTiming>();
                        DoctorTiming doctorTiming = new DoctorTiming();
                        Doctor doctor = new Doctor();
                        reader.ReadStartElement("Doctor");
                        string docId =  reader.ReadElementContentAsString();
                        string docName = reader.ReadElementContentAsString();
                        string docdepartment = reader.ReadElementContentAsString();
                        string docspecialist = reader.ReadElementContentAsString();

                        if (reader.ReadToDescendant("TIMING"))
                        {
                            do
                            {
                                reader.ReadStartElement("TIMING");
                                doctorTiming.Day = int.Parse(reader.ReadElementContentAsString());
                                doctorTiming.StartTime = reader.ReadElementContentAsString();
                                doctorTiming.EndTme = reader.ReadElementContentAsString();
                                doctorTimings.Add(doctorTiming);
                            } while (reader.ReadToNextSibling("TIMING"));
                            doctor.TimingList = doctorTimings;
                            reader.ReadEndElement();
                        }

                        doctor.Id = docId;
                        doctor.Name = docName;
                        doctor.Department = docdepartment;
                        doctor.Specialist = doctorSpeciality;

                    } while (reader.ReadToNextSibling("Doctor"));
                }


            }
            UpdateDashboard();

        }

        private void btnMenuSaveData_Click(object sender, RoutedEventArgs e)
        {
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.Indent = true;
            writerSettings.IndentChars = "\t";

            XmlWriter writer = XmlWriter.Create("Data.xml", writerSettings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Data");
            WritePatientData(writer);
            WriteDoctorData(writer);
            writer.WriteEndElement();
            writer.Close();
        }


        private void WritePatientData(XmlWriter writer)
        {
            writer.WriteStartElement("Patients");


            foreach (Patient patient in patientList)
            {
                writer.WriteStartElement("Patient");
                writer.WriteElementString("ID", patient.Id);
                writer.WriteElementString("NAME", patient.Name);
                writer.WriteElementString("ADDRESS", patient.Address);
                writer.WriteElementString("CONTACT", patient.ContactNumber);
                writer.WriteElementString("GENDER", patient.Gender);
                writer.WriteElementString("AGE", patient.Age.ToString());
                writer.WriteElementString("HEIGHT", patient.Height.ToString());
                writer.WriteElementString("WEIGHT", patient.Weight.ToString());


                if (patient.DiseasesInPast.Count != 0)
                {
                    writer.WriteStartElement("DISEASES");
                    foreach (string disease in patient.DiseasesInPast)
                    {
                        writer.WriteElementString("DISEASE", disease);
                    }
                    writer.WriteEndElement();
                }


                if (patient.OtherImpInformation != string.Empty)
                {
                    writer.WriteElementString("IMPORTANTINFORMATION", patient.OtherImpInformation);

                }

                if (patient.InstructionGiven.Count != 0)
                {
                    writer.WriteStartElement("INSTRUCTIONS");
                    foreach (string instruction in patient.InstructionGiven)
                    {
                        writer.WriteElementString("INSTRUCTION", instruction);
                    }
                    writer.WriteEndElement();
                }





                writer.WriteEndElement();
            }
            writer.WriteEndElement();

        }

        private void WriteDoctorData(XmlWriter writer)
        {
            writer.WriteStartElement("DOCTORS");

            foreach (Doctor doctor in doctorList)
            {
                writer.WriteStartElement("Doctor");
                writer.WriteElementString("ID", doctor.Id);
                writer.WriteElementString("NAME", doctor.Name);
                writer.WriteElementString("DEPARTMENT", doctor.Department);
                writer.WriteElementString("SPECIALIST", doctor.Specialist);

                writer.WriteStartElement("TIMINGS");

                foreach (DoctorTiming doctorTiming in doctor.TimingList)
                {
                    writer.WriteStartElement("TIMING");
                    writer.WriteElementString("DAY", doctorTiming.Day.ToString());
                    writer.WriteElementString("START", doctorTiming.StartTime);
                    writer.WriteElementString("END", doctorTiming.EndTme);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

            }
        }

        private void btnPatientManagerUpdate_Click(object sender, RoutedEventArgs e)
        {
            string patientId =  tbPatientManageId.Text;
            foreach (Patient patient in patientList)
            {
                if (patient.Id == patientId)
                {
                    patient.Name = tbPatientManageName.Text;
                    patient.Address = tbPatientManageAddress.Text;
                    patient.ContactNumber = tbPatientManageContact.Text;
                    patient.Height = double.Parse(tbPatientManageHeight.Text);
                    patient.Weight = double.Parse(tbPatientManageWeight.Text);
                    break;    
                }
            }
            MessageBox.Show("Patient Details Updated");
        }

        private void appointmentDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterDoctorList();
        }

        private void CbTuesday_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                if (cbTuesday.IsChecked == true)
                {
                    cbTuesdayFrom.IsEnabled = true;
                    cbTuesdayFrom.SelectedIndex = 9;
                    cbTuesdayTo.IsEnabled = true;
                    cbTuesdayTo.SelectedIndex = 17;
                    tuesdayFlag = true;
                }
                else
                {
                    cbTuesdayFrom.IsEnabled = false;
                    cbTuesdayFrom.SelectedIndex = -1;
                    cbTuesdayTo.IsEnabled = false;
                    cbTuesdayTo.SelectedIndex = -1;
                    tuesdayFlag = false;

                }
            }
        }

        private void CbWednesday_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                if (cbWednesday.IsChecked == true)
                {
                    cbWednesdayFrom.IsEnabled = true;
                    cbWednesdayFrom.SelectedIndex = 9;
                    cbWednesdayTo.IsEnabled = true;
                    cbWednesdayTo.SelectedIndex = 17;
                    wednesdayFlag = true;
                }
                else
                {
                    cbWednesdayFrom.IsEnabled = false;
                    cbWednesdayFrom.SelectedIndex = -1;
                    cbWednesdayTo.IsEnabled = false;
                    cbWednesdayTo.SelectedIndex = -1;
                    wednesdayFlag = false;
                }
            }
        }

        private void CbThurday_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                if (cbThurday.IsChecked == true)
                {
                    cbThursdayFrom.IsEnabled = true;
                    cbThursdayFrom.SelectedIndex = 9;
                    cbThursdayTo.IsEnabled = true;
                    cbThursdayTo.SelectedIndex = 17;
                    thursdayFlag = true;
                }
                else
                {
                    cbThursdayFrom.IsEnabled = false;
                    cbThursdayFrom.SelectedIndex = -1;
                    cbThursdayTo.IsEnabled = false;
                    cbThursdayTo.SelectedIndex = -1;
                    thursdayFlag = false;
                }
            }
        }

        private void CbFriday_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                if (cbFriday.IsChecked == true)
                {
                    cbFridayFrom.IsEnabled = true;
                    cbFridayFrom.SelectedIndex = 9;
                    cbFridayTo.IsEnabled = true;
                    cbFridayTo.SelectedIndex = 17;
                    fridayFlag = true;
                }
                else
                {
                    cbFridayFrom.IsEnabled = false;
                    cbFridayFrom.SelectedIndex = -1;
                    cbFridayTo.IsEnabled = false;
                    cbFridayTo.SelectedIndex = -1;
                    fridayFlag = false;
                }
            }
        }

        private void CbSaturday_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                if (cbSaturday.IsChecked == true)
                {
                    cbSaturdayFrom.IsEnabled = true;
                    cbSaturdayFrom.SelectedIndex = 9;
                    cbSaturdayTo.IsEnabled = true;
                    cbSaturdayTo.SelectedIndex = 17;
                    saturdayFlag = true;
                }
                else
                {
                    cbSaturdayFrom.IsEnabled = false;
                    cbSaturdayFrom.SelectedIndex = -1;
                    cbSaturdayTo.IsEnabled = false;
                    cbSaturdayTo.SelectedIndex = -1;
                    saturdayFlag = false;
                }
            }
        }

        


    }
}
