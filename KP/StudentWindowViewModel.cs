using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace KP
{
    internal class StudentWindowViewModel
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

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

        string selectedLastName;
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

        string selectedFirstName;
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

        int selectedCourse;
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

        int selectedGroup;
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
        
        string selectedNote;
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

        Faculty selectedFaculty;
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

        //int selectedIndexFaculty;
        //public int SelectedIndexFaculty
        //{
        //    get { return selectedIndexFaculty; }
        //    set
        //    {
        //        if (selectedIndexFaculty != null)
        //        {
                    
        //        }
        //        OnPropertyChanged("SelectedIndexFaculty");
        //    }
        //}

        public StudentWindowViewModel()
        {

        }

        public StudentWindowViewModel(Student s, ObservableCollection<Faculty> faculties, Room selectedRoom)
        {
            try
            {
                if(selectedRoom == null)
                {
                    throw new Exception("Ошибка! Не выбрана комната!");
                }
                if (s.FirstName != null)
                {
                    Student = s;
                    Student.Room = selectedRoom;
                    this.faculties = faculties;
                    SelectedFirstName = Student.FirstName;
                    SelectedLastName = Student.LastName;
                    SelectedSecondName = Student.SecondName;
                    SelectedCourse = (int)Student.Course;
                    SelectedFaculty = Student.Faculty;
                    SelectedGroup = (int)Student.Group;
                    SelectedNote = Student.Note;
                }
                else
                {
                    Student = s;
                    this.faculties = faculties;
                    Student.Room = selectedRoom;
                }

                studentWindowView = new StudentWindowView();
                studentWindowView.DataContext = this;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //private RelayCommand accept1Command;
        //public RelayCommand Accept1Command
        //{
        //    get
        //    {
        //        return accept1Command ??
        //          (accept1Command = new RelayCommand(obj =>
        //          {
        //              studentWindowView.DialogResult = true;

        //          }));
        //    }
        //}
    }
}