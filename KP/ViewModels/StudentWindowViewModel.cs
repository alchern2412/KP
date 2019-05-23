using Microsoft.Win32;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;

using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace KP
{
    public class StudentWindowViewModel :  ValidatableObject, INotifyPropertyChanged
    {
        //validation

        private IObjectValidator GetValidator()
        {
            var builder = new ValidationBuilder<StudentWindowViewModel>();

            builder.RuleFor(vm => vm.SelectedLastName).NotEmpty().Must(BeAValidLastName).WithMessage("С заглавной буквы. Напр.: Иванов").Length(2,30);
            builder.RuleFor(vm => vm.SelectedFirstName).NotEmpty().Must(BeAValidName).WithMessage("С заглавной буквы. Напр.: Иван").Length(2,30);
            builder.RuleFor(vm => vm.SelectedSecondName).Must(BeAValidSecondName).Length(0,30);

            return builder.Build(this);
        }

        private bool BeAValidName(string name)
        {
            if(name == null)
            {
                return false;
            }
            if (Regex.IsMatch(name, @"^[А-ЯA-Z][А-Яа-яA-Za-z]+"))
            {
                bName = true;
                BtnOk = true;
                return true;

            }
            else
            {
                bName = false;
                BtnOk = false;
                return false;
            }
        }

        private bool BeAValidLastName(string name)
        {
            if(name == null)
            {
                return false;
            }
            if (Regex.IsMatch(name, @"^[А-ЯA-Z][А-Яа-яA-Za-z]+"))
            {
                bLastName = true;
                BtnOk = true;
                return true;

            }
            else
            {
                bLastName = false;
                BtnOk = false;
                return false;
            }
        }

        private bool BeAValidSecondName(string name)
        {
            if (name == null || name == "")
            {
                bSecondName = true;
                BtnOk = true ;
                return true;
            }
            if (Regex.IsMatch(name, @"^[А-ЯA-Z][А-Яа-яA-Za-z]+"))
            {
                bSecondName = true;
                BtnOk = true;
                return true;

            }
            else
            {
                bSecondName = false;
                BtnOk = true;
                return false;
            }
        }


        // properties with realisation

        //public event PropertyChangedEventHandler PropertyChanged;
        //public void OnPropertyChanged([CallerMemberName]string prop = "")
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(prop));
        //}


        public Student Student { get; private set; }
        public StudentWindowView studentWindowView;

        /*Список факультетов*/
        ObservableCollection<Faculty> faculties;
        public ObservableCollection<Faculty> Faculties
        {
            get { return faculties; }
            set
            {
                faculties = value;
                OnPropertyChanged("Faculties");
            }
        }

        /*Выбранная должность*/

        string studSovietPosition;
        public string StudSovietPosition
        {
            get { return studSovietPosition; }
            set
            {
                studSovietPosition = value;
                Student.StudSovietMember.Position = value;
                OnPropertyChanged("StudSovietPosition");
            }
        }


        bool bName = false;
        bool bSecondName = false;
        bool bLastName = false;

        bool btnOk;
        public bool BtnOk
        {
            get { return btnOk; }
            set
            {
                btnOk = (bName && bLastName && bSecondName)? true : false;
                OnPropertyChanged("BtnOk");
            }
        }

        /*Список студсовета*/
        ObservableCollection<StudSovietMember> studsoviteMembers;
        public ObservableCollection<StudSovietMember> StudsoviteMembers
        {
            get { return studsoviteMembers; }
            set
            {
                studsoviteMembers = value;
                OnPropertyChanged("StudsoviteMembers");
            }
        }

        string selectedLastName;    // имя
        public string SelectedLastName
        {
            get { return selectedLastName; }
            set
            {
                selectedLastName = value;
                if (selectedLastName != null)
                {
                    Student.LastName = (string)selectedLastName;
                }
                OnPropertyChanged("SelectedLastName");
            }
        }

        string selectedFirstName;   // фамилия
        public string SelectedFirstName
        {
            get { return selectedFirstName; }
            set
            {
                selectedFirstName = value;
                if (selectedFirstName != null)
                {
                    Student.FirstName = (string)selectedFirstName;
                }
                OnPropertyChanged("SelectedFirstName");
            }
        }

        string selectedSecondName;
        public string SelectedSecondName
        {
            get { return selectedSecondName; }
            set
            {
                selectedSecondName = value;
                if (selectedSecondName != null)
                {
                    Student.SecondName = (string)selectedSecondName;
                }
                OnPropertyChanged("SelectedSecondName");
            }
        }

        int selectedCourse; // курс
        public int SelectedCourse
        {
            get { return selectedCourse; }
            set
            {
                selectedCourse = value;
                Student.Course = (int)selectedCourse;
                OnPropertyChanged("SelectedCourse");
            }
        }

        int selectedGroup; // группа
        public int SelectedGroup
        {
            get { return selectedGroup; }
            set
            {
                selectedGroup = value;
                Student.Group = (int)selectedGroup;
                OnPropertyChanged("SelectedGroup");
            }
        }

        string selectedSex;    // имя
        public string SelectedSex
        {
            get { return selectedSex; }
            set
            {
                selectedSex = value;
                if (selectedSex != null)
                {
                    Student.Sex = (string)selectedSex;
                }
                OnPropertyChanged("SelectedSex");
            }
        }
        
        string selectedNote; // хар-ка
        public string SelectedNote
        {
            get { return selectedNote; }
            set
            {
                selectedNote = value;
                if (selectedNote != null)
                {
                    Student.Note = (string)selectedNote;
                }
                OnPropertyChanged("SelectedNote");
            }
        }

        Faculty selectedFaculty; // факультет
        public Faculty SelectedFaculty
        {
            get { return selectedFaculty; }
            set
            {
                selectedFaculty = value;
                if (selectedFaculty != null)
                {
                    Student.Faculty = selectedFaculty;
                }
                OnPropertyChanged("SelectedFaculty");
            }
        }


        private DateTime selectedBirthday;  // ДР
        public DateTime SelectedBirthday {
            get { return selectedBirthday; }
            set
            {
                selectedBirthday = value;
                Student.Birthday = selectedBirthday;
                OnPropertyChanged("SelectedBirthday");
            }
        }

        private DateTime selectedDateOfEntry;   // дата заселения
        public DateTime SelectedDateOfEntry {
            get { return selectedDateOfEntry; }
            set
            {
                    selectedDateOfEntry = (DateTime)value;
                Student.DateOfEntry = selectedDateOfEntry;
                OnPropertyChanged("SelectedDateOfEntry");
            }
        }

        private DateTime selectedDateOfDeparture;   // дата выселения
        public DateTime SelectedDateOfDeparture {
            get { return selectedDateOfDeparture; }
            set
            {
                    selectedDateOfDeparture = (DateTime)value;
                Student.DateOfDeparture = selectedDateOfDeparture;
                OnPropertyChanged("SelectedDateOfDeparture");
            }
        }


        private DateTime selectedDateOfEntryMember;   // дата выселения
        public DateTime SelectedDateOfEntryMember
        {
            get { return selectedDateOfEntryMember; }
            set
            {
                selectedDateOfEntryMember = (DateTime)value;
                
                Student.StudSovietMember.DateOfEntry = selectedDateOfEntryMember;
                OnPropertyChanged("SelectedDateOfEntryMember");
            }
        }

        public string imagePath;
        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                Student.Photo = value;
                OnPropertyChanged("ImagePath");
            }
        }

        // команда открытия файла
        private RelayCommand openCommand;
        public RelayCommand OpenCommand
        {
            get
            {
                return openCommand ??
                  (openCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          OpenFileDialog dlg = new OpenFileDialog();
                          dlg.InitialDirectory = "";
                          dlg.Filter = "Image files (*.jpg,*.png,*.bmp)|*.jpg;*.png;*.bmp|All Files (*.*)|*.*";
                          if (dlg.ShowDialog() == true)
                          {
                              dlg.Multiselect = false;
                              ImagePath = dlg.FileName;
                          }

                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        public StudentWindowViewModel(Student s, ObservableCollection<Faculty> faculties, Room selectedRoom)
        {
            try
            {

                // for datetime
                Thread.CurrentThread.CurrentCulture = new CultureInfo("RU-RU");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("RU-RU");

                Validator = GetValidator();

                if (selectedRoom == null)
                {
                    throw new Exception("Ошибка! Не выбрана комната!");
                }

                // Existing
                if (s.FirstName != null)
                {
                    Student = s;
                    Student.Room = selectedRoom;
                    Faculties = faculties;
                    SelectedFirstName = Student.FirstName;
                    SelectedLastName = Student.LastName;
                    SelectedSecondName = Student.SecondName;
                    SelectedCourse = (int)Student.Course;
                    SelectedFaculty = Student.Faculty;
                    SelectedGroup = (int)Student.Group;
                    SelectedNote = Student.Note;
                    SelectedSex = Student.Sex;

                    StudSovietPosition = Student.StudSovietMember.Position;

                    BtnOk = true;

                    if (Student.Photo != null)
                    {
                        ImagePath = Student.Photo;  ///!!!
                    }

                    SelectedDateOfDeparture = (DateTime)Student.DateOfDeparture;
                    SelectedDateOfEntry = (DateTime)Student.DateOfEntry;
                    SelectedDateOfEntryMember = (DateTime)Student.StudSovietMember.DateOfEntry;
                    SelectedBirthday = (DateTime)Student.Birthday;

                }
                // new student
                else
                {
                    Student = s;
                    this.faculties = faculties;
                    BtnOk = false;
                    Student.Room = selectedRoom;
                    Student.Photo = ImagePath = @"\images\user-default.png";
                    Student.StudSovietMember = new StudSovietMember();
                    SelectedDateOfDeparture = DateTime.Now;
                    SelectedDateOfEntry = DateTime.Now;
                    SelectedDateOfEntryMember = DateTime.Now;
                    SelectedBirthday = DateTime.Now;
                    SelectedSex = "Мужской";
                    SelectedCourse = 1;
                    SelectedGroup = 1;
                    SelectedFaculty = faculties[0]??new Faculty {FacultyName="Неизв" };
                    SelectedFirstName = "";
                    SelectedLastName = "";
                    SelectedSecondName = "";

                }

                studentWindowView = new StudentWindowView();
                studentWindowView.Language = XmlLanguage.GetLanguage("ru-RU");
                studentWindowView.DataContext = this;
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }
}