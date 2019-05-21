using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KP
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public ObservableCollection<Student> SelectedStudents { get; set; }

        #endregion

        public HostelContainer db { get;  }

        #region Collections

        #endregion


        public ApplicationViewModel()
        {
            db = new HostelContainer();



            //db.Rooms.Load();
            db.Rooms.OrderBy(s => s.Number).Load();
            db.Floors.OrderBy(s => s.Number).Load();
            db.Faculties.Load();
            db.StudSovietMembers.Load();
            db.DutyFloorWatches.Load();

            db.Students.Load();
            SelectedStudents = new ObservableCollection<Student>(db.Students.Local);
            OnPropertyChanged("SelectedStudents");
            //MessageBox.Show(db.Floors.Local.Count.ToString());    

            ////test excel
            //Excel ex = new Excel(@"D:\HelloApp.xlsx",1);
            //Report.StudentWrite(new ObservableCollection<Floor>(db.Floors.OrderBy(s => s.Number)), ex);
            //ex.Save();
            //ex.Close();
        }

        #region CommandsFloor






        // команда добавления нового объекта
        private RelayCommand addFloorCommand;
        public RelayCommand AddFloorCommand
        {
            get
            {
                
                    return addFloorCommand ??
                      (addFloorCommand = new RelayCommand(obj =>
                      {
                          try
                          { 
                              FloorWindowViewModel floorWindow = new FloorWindowViewModel(new Floor());
                              if (floorWindow.floorWindowView.ShowDialog() == true)
                              {
                                  Floor floor = floorWindow.Floor;
                                  foreach(Floor i in db.Floors)
                                  {
                                      if(i.Number == floor.Number)
                                      {
                                          throw new Exception("Дублирование этажей");
                                      }
                                  }
                                  
                                  db.Floors.Add(floor);
                                  db.SaveChanges();
                              }
                          }
                          catch (Exception ex)
                          {
                              MessageBox.Show(ex.Message);
                          }
                      }));
                
            }
        }

        // команда удаления этажа
        private RelayCommand removeFloorCommand;
        public RelayCommand RemoveFloorCommand
        {
            get
            {
                return removeFloorCommand ??
                    (removeFloorCommand = new RelayCommand(obj =>
                    {
                        /*Удалить из БД этаж*/
                        //SelectedStudents = null;
                        foreach(var room in SelectedFloor.Rooms)
                        {
                            foreach (var i in room.Students)
                            {
                                db.StudSovietMembers.Remove(i.StudSovietMember);
                                db.DutyFloorWatches.RemoveRange(i.DutyFloorWatches);
                            }
                            db.Students.RemoveRange(room.Students);
                        }
                        db.Rooms.RemoveRange(SelectedFloor.Rooms);
                       

                        db.Floors.Remove(SelectedFloor);
                        db.SaveChanges();
                        
                    }));
            }
        }

        //private RelayCommand doubleCommand;
        //public RelayCommand DoubleCommand
        //{
        //    get
        //    {
        //        return doubleCommand ??
        //            (doubleCommand = new RelayCommand(obj =>
        //            {
        //                Phone phone = obj as Phone;
        //                if (phone != null)
        //                {
        //                    Phone phoneCopy = new Phone
        //                    {
        //                        Company = phone.Company,
        //                        Price = phone.Price,
        //                        Title = phone.Title
        //                    };
        //                    Phones.Insert(0, phoneCopy);
        //                }
        //            }));
        //    }
        //}

        private Floor selectedFloor;
        public Floor SelectedFloor
        {
            get { return selectedFloor; }
            set
            {
                selectedFloor = value;
               // selectedFloor.Rooms = selectedFloor.Rooms.OrderBy(s => s.Number);
                OnPropertyChanged("SelectedFloor");
            }
        }

        #endregion

        #region CommandsRooms

        // команда добавления нового объекта
        private RelayCommand addRoomCommand;
        public RelayCommand AddRoomCommand
        {
            get
            {
                return addRoomCommand ??
                  (addRoomCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          RoomWindowViewModel roomWindow = new RoomWindowViewModel(new Room());
                          if (roomWindow.roomWindowView.ShowDialog() == true)
                          {
                              Room room = roomWindow.Room;
                              int flNum = Convert.ToInt32(room.Number.ToString().Substring(0, 1));
                              Floor findedfloor = null;

                              foreach (Room i in db.Rooms)
                              {
                                  if (i.Number == room.Number)
                                  {
                                      throw new Exception("Ошибка! Дублирование комнат.");
                                  }
                              }

                              /*Поиск этажа*/
                              foreach (Floor findfl in db.Floors)
                              {
                                  if (findfl.Number == flNum)
                                  {
                                      findedfloor = findfl;
                                  }
                              }
                              
                              if (findedfloor == null)
                              {
                                  throw new Exception("Не создан этаж комнаты");
                              }

                              room.Floor = findedfloor;

                              db.Rooms.Add(room);
                              db.SaveChanges();
                              //db.Rooms.Load();
                          }
                      }
                      catch(Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }



                      //Room room = new Room();
                      //Rooms.Insert(0, room);
                      //SelectedRoomF = room;

                      //db.Rooms.Add(room);
                      //db.SaveChanges();
                  }));
            }
        }

        // команда редактирования комнаты
        private RelayCommand editRoomCommand;
        public RelayCommand EditRoomCommand
        {
            get
            {
                return editRoomCommand ??
                  (editRoomCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          if(SelectedRoomF == null)
                          {
                              throw new Exception("Ошибка! Выберите комнату для редактирования!");
                          }
                          Room parm = SelectedRoomF;
                          RoomWindowViewModel roomWindow = new RoomWindowViewModel(parm);
                          if (roomWindow.roomWindowView.ShowDialog() == true)
                          {
                              //db.Rooms.Where(s => s.Number == parm.Number).ToList().First().Bed = parm.Bed;
                              //db.Rooms.Where(s => s.Number == parm.Number).ToList().First().Chair = parm.Chair;
                              //db.Rooms.Where(s => s.Number == parm.Number).ToList().First().Nightstand = parm.Nightstand;

                              SelectedRoomF.Nightstand = parm.Nightstand;
                              SelectedRoomF.Bed = parm.Bed;
                              SelectedRoomF.Chair = parm.Chair;

                              OnPropertyChanged("SelectedRoomF");

                              db.SaveChanges();
                          }
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        // команда удаления комнаты
        private RelayCommand removeRoomCommand;
        public RelayCommand RemoveRoomCommand
        {
            get
            {
                return removeRoomCommand ??
                    (removeRoomCommand = new RelayCommand(obj =>
                    {
                        Room room = obj as Room;
                        if (room != null)
                        {
                            //db.StudSovietMembers.Remove(student.StudSovietMember);
                            //db.Students.Remove(student);

                            foreach(var i in room.Students)
                            {
                                db.StudSovietMembers.Remove(i.StudSovietMember);
                                db.DutyFloorWatches.RemoveRange(i.DutyFloorWatches);
                            }
                            db.Students.RemoveRange(room.Students);

                            db.Rooms.Remove(room);
                            
                            db.SaveChanges();
                        }
                    }));
            }
        }


        private Room selectedRoom;
        public Room SelectedRoomF
        {
            get { return selectedRoom; }
            set
            {
                SearchText = SearchCourse = SearchFaculty = SearchGroup = SearchRoom = "";
                selectedRoom = value;
                SelectedStudents = new ObservableCollection<Student>(db.Students.Local.Where(s => s.Room == SelectedRoomF));
                OnPropertyChanged("SelectedRoomF");
                OnPropertyChanged("SelectedStudents");
            }
        }

        #endregion

        #region CommandsStudents

        //////////////////////////////////////// ПОИСК

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged("SearchText");

                int roomtmp = 0;
                if (Int32.TryParse(value, out roomtmp))
                {
                    SelectedStudents = new ObservableCollection<Student>(db.Students.Local.Where(s => s.Room.Number.ToString().Contains(roomtmp.ToString())));
                }
                else
                {
                    find();
                }

                OnPropertyChanged("SelectedStudents");
                
            }
        }

        private void find()
        {
            string si = (searchText) ?? "";
            string nroom = (searchRoom) ?? "";
            string fc = (searchFaculty) ?? "";
            string coursef = (searchCourse) ?? "";
            string gr = (searchGroup) ?? "";
            SelectedStudents = new ObservableCollection<Student>(db.Students.Local.Where(s =>
            s.Room.Number.ToString().ToUpper().Contains(nroom.ToUpper())
                    &   s.Faculty.FacultyName.ToUpper().Contains(fc.ToUpper())
                    &   s.Course.ToString().ToUpper().Contains(coursef.ToUpper())
                    &   s.Group.ToString().ToUpper().Contains(gr.ToUpper())))
                    ;
            SelectedStudents = new ObservableCollection<Student>(SelectedStudents.Where(s =>
                    s.Room.Number.ToString().Contains(si)
                    || s.FirstName.ToUpper().Contains(si.ToUpper())
                    || s.LastName.ToUpper().Contains(si.ToUpper())
                    || /**/ (s.LastName + " " + s.FirstName).ToUpper().Contains(si.ToUpper())
                    || (s.FirstName + " " + s.LastName).ToUpper().Contains(si.ToUpper())));
            OnPropertyChanged("SelectedStudents");

        }

        private string searchRoom;
        public string SearchRoom
        {
            get { return searchRoom; }
            set
            {
                searchRoom = value;
                find();
                OnPropertyChanged("SearchRoom");
                OnPropertyChanged("SelectedStudents");
            }
        }
        private string searchFaculty;
        public string SearchFaculty
        {
            get { return searchFaculty; }
            set
            {
                searchFaculty = value;
                find();
                OnPropertyChanged("SearchFaculty");
                OnPropertyChanged("SelectedStudents");
            }
        }
        private string searchCourse;
        public string SearchCourse
        {
            get { return searchCourse; }
            set
            {
                searchCourse = value;
                find();
                OnPropertyChanged("SearchCourse");
                OnPropertyChanged("SelectedStudents");
            }
        }
        private string searchGroup;
        public string SearchGroup
        {
            get { return searchGroup; }
            set
            {
                searchGroup = value;
                find();
                OnPropertyChanged("SearchGroup");
                OnPropertyChanged("SelectedStudents");
            }
        }



        ///////////////// сохранение отчета


        private RelayCommand writeReportCommand;
        public RelayCommand WriteReportCommand
        {
            get
            {
                return writeReportCommand ??
                  (writeReportCommand = new RelayCommand(obj =>
                  {
                      
                      try
                      {
                          ////test excel
                          DefaultDialogService d = new DefaultDialogService();
                          if(d.SaveFileDialog())
                          {
                              Excel ex = new Excel();
                              ex.CreateNewFile();
                              Report.StudentWrite(new ObservableCollection<Student>(db.Students.Local), ex);
                              ex.SaveAs(d.FilePath);
                              ex.Close();
                          }

                          
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        private RelayCommand writeReportSelCommand;
        public RelayCommand WriteReportSelCommand
        {
            get
            {
                return writeReportSelCommand ??
                  (writeReportSelCommand = new RelayCommand(obj =>
                  {

                      try
                      {
                          ////test excel
                          DefaultDialogService d = new DefaultDialogService();
                          if (d.SaveFileDialog())
                          {
                              Excel ex = new Excel();
                              ex.CreateNewFile();
                              Report.StudentWrite(SelectedStudents, ex);
                              ex.SaveAs(d.FilePath);
                              ex.Close();
                          }


                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }


        private RelayCommand writeReportRoomCommand;
        public RelayCommand WriteReportRoomCommand
        {
            get
            {
                return writeReportRoomCommand ??
                  (writeReportRoomCommand = new RelayCommand(obj =>
                  {

                      try
                      {
                          ////test excel
                          DefaultDialogService d = new DefaultDialogService();
                          if (d.SaveFileDialog())
                          {
                              Excel ex = new Excel();
                              ex.CreateNewFile();
                              Report.RoomWrite(new List<Room>(db.Rooms.Local), ex);
                              ex.SaveAs(d.FilePath);
                              ex.Close();
                          }


                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }








        private int studentSelectedIndex;  // выбранный студент
        public int StudentSelectedIndex
        {
            get { return studentSelectedIndex; }
            set
            {
                if (value == -1)
                {
                    studentSelectedIndex = 0;

                }
                else
                {
                    studentSelectedIndex = value;
                }
                OnPropertyChanged("SelectedStudents");
                OnPropertyChanged("StudentSelectedIndex");
                OnPropertyChanged("SelectedStudent");
            }
        }


        // команда добавления нового объекта
        private RelayCommand addStudentCommand;
        public RelayCommand AddStudentCommand
        {
            get
            {
                return addStudentCommand ??
                  (addStudentCommand = new RelayCommand(obj =>
                  {
                      Student student = new Student();
                      try
                      {
                          StudentWindowViewModel studentWindow = new StudentWindowViewModel(student, db.Faculties.Local, SelectedRoomF);
                          if (studentWindow.studentWindowView.ShowDialog() == true)
                          {
                              student = studentWindow.Student;
                              db.Students.Add(student);
                              db.SaveChanges();

                              SelectedStudents = new ObservableCollection<Student>(db.Students.Local.Where(s => s.Room == SelectedRoomF));

                              OnPropertyChanged("SelectedRoomF");
                              OnPropertyChanged("SelectedStudents");
                          }
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        private Student selectedStudent;
        public Student SelectedStudent
        {
            get { return selectedStudent; }
            set
            {
                selectedStudent = value;
                OnPropertyChanged("SelectedStudent");
                //SelectedFloor = value.Room.Floor;
                //OnPropertyChanged("SelectedFloor");
                //SelectedRoomF = value.Room;
                //OnPropertyChanged("SelectedRoomF");
                
            }
        }

        //команда удаления студента
        private RelayCommand removeStudentCommand;
        public RelayCommand RemoveStudentCommand
        {
            get
            {
                return removeStudentCommand ??
                    (removeStudentCommand = new RelayCommand(obj =>
                    {
                    Student student = obj as Student;
                    if (student != null)
                    {
                            //Students.Remove(student);
                            //db.StudSovietMembers.Remove(student.StudSovietMember);
                            //db.StudSovietMembers.Remove(student.StudSovietMember);


                            db.StudSovietMembers.Remove(student.StudSovietMember);
                            db.DutyFloorWatches.RemoveRange(student.DutyFloorWatches);

                            db.Students.Remove(student);
                            

                            db.SaveChanges();
                            if (SearchText != null && SearchText != "")
                            {
                                SelectedStudents = new ObservableCollection<Student>(db.Students.Local.Where(s => s.FirstName.ToUpper().Contains(SearchText.ToUpper()) || s.LastName.ToUpper().Contains(SearchText.ToUpper())));
                            }
                            else if(SelectedRoomF != null)
                            {
                                SelectedStudents = new ObservableCollection<Student>(db.Students.Local.Where(s => s.Room == SelectedStudent.Room));

                            }
                            else
                            {
                                SelectedStudents = new ObservableCollection<Student>(db.Students.Local);
                            }
                            OnPropertyChanged("SelectedRoomF");
                            OnPropertyChanged("SelectedStudents");
                    }
                        else
                        {
                            MessageBox.Show("Выберите студента!");
                        }
                    }));
            }
        }

        //public ICollectionView ICollectionStudents;

        private RelayCommand editStudentCommand;//entity state modified
        public RelayCommand EditStudentCommand
        {
            get
            {
                return editStudentCommand ??
                    (editStudentCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            Student student = obj as Student;
                            if (student != null)
                            {
                                StudentWindowViewModel studentWindow = new StudentWindowViewModel(student, db.Faculties.Local, SelectedStudent.Room);
                                if (studentWindow.studentWindowView.ShowDialog() == true)
                                {
                                    //SelectedStudent = null;
                                    //SelectedStudents = null;
                                    student = studentWindow.Student;
                                    db.SaveChanges();

                                    SelectedStudents = new ObservableCollection<Student>(db.Students.Local);
                                    
                                    OnPropertyChanged("SelectedStudent");
                                    OnPropertyChanged("SelectedStudents");

                                    if ((SearchText == null || SearchText == "") && SelectedRoomF != null)
                                    {
                                        SelectedStudents = new ObservableCollection<Student>(db.Students.Local.Where(s => s.Room == SelectedRoomF));
                                    }
                                    else if(SearchText != null && SearchText !=  "")
                                    {
                                        SelectedStudents = new ObservableCollection<Student>(db.Students.Local.Where(s => s.FirstName.ToUpper().Contains(searchText.ToUpper()) || s.LastName.ToUpper().Contains(searchText.ToUpper())));
                                    }
                                    else
                                    {
                                        SelectedStudents = new ObservableCollection<Student>(db.Students.Local);
                                    }
                                    OnPropertyChanged("SelectedStudent");
                                    OnPropertyChanged("SelectedStudents");
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }));
            }
        }

        #region sorting
        private RelayCommand sortName;
        public RelayCommand SortName
        {
            get
            {
                return sortName ??
                    (sortName = new RelayCommand(obj =>
                    {
                        bool flag = false;

                        ObservableCollection<Student> tmp = new ObservableCollection<Student>(SelectedStudents.OrderBy(s => s.FirstName));
                        for(int i = 0; i < SelectedStudents.Count; i++ )
                        {
                            if(!tmp[i].Equals(SelectedStudents[i]))
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            tmp = new ObservableCollection<Student>(SelectedStudents.OrderByDescending(s => s.FirstName));
                            for (int i = 0; i < SelectedStudents.Count; i++)
                            {
                                if (!tmp[i].Equals(SelectedStudents[i]))
                                {
                                    flag = false;
                                    break;
                                }
                            }
                        }
                        if(flag)
                        {
                            SelectedStudents = new ObservableCollection<Student>(SelectedStudents.OrderBy(s => s.FirstName));
                        }
                        else
                        {
                            SelectedStudents = new ObservableCollection<Student>(SelectedStudents.OrderByDescending(s => s.FirstName));
                        }

                        OnPropertyChanged("SelectedStudents");
                    }));
            }
        }

        private RelayCommand sortLastname;
        public RelayCommand SortLastname
        {
            get
            {
                return sortLastname ??
                    (sortLastname = new RelayCommand(obj =>
                    {
                        bool flag = false;

                        ObservableCollection<Student> tmp = new ObservableCollection<Student>(SelectedStudents.OrderBy(s => s.LastName));
                        for (int i = 0; i < SelectedStudents.Count; i++)
                        {
                            if (!tmp[i].Equals(SelectedStudents[i]))
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            tmp = new ObservableCollection<Student>(SelectedStudents.OrderByDescending(s => s.LastName));
                            for (int i = 0; i < SelectedStudents.Count; i++)
                            {
                                if (!tmp[i].Equals(SelectedStudents[i]))
                                {
                                    flag = false;
                                    break;
                                }
                            }
                        }
                        if (flag)
                        {
                            SelectedStudents = new ObservableCollection<Student>(SelectedStudents.OrderBy(s => s.LastName));
                        }
                        else
                        {
                            SelectedStudents = tmp;
                        }

                        OnPropertyChanged("SelectedStudents");
                    }));
            }
        }

        private RelayCommand sortAge;
        public RelayCommand SortAge
        {
            get
            {
                return sortAge ??
                    (sortAge = new RelayCommand(obj =>
                    {
                        bool flag = false;

                        ObservableCollection<Student> tmp = new ObservableCollection<Student>(SelectedStudents.OrderBy(s => s.Birthday));
                        for (int i = 0; i < SelectedStudents.Count; i++)
                        {
                            if (!tmp[i].Equals(SelectedStudents[i]))
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            tmp = new ObservableCollection<Student>(SelectedStudents.OrderByDescending(s => s.Birthday));
                            for (int i = 0; i < SelectedStudents.Count; i++)
                            {
                                if (!tmp[i].Equals(SelectedStudents[i]))
                                {
                                    flag = false;
                                    break;
                                }
                            }
                        }
                        if (flag)
                        {
                            SelectedStudents = new ObservableCollection<Student>(SelectedStudents.OrderBy(s => s.Birthday));
                        }
                        else
                        {
                            SelectedStudents = tmp;
                        }

                        OnPropertyChanged("SelectedStudents");
                    }));
            }
        }
        #endregion
        #endregion

        #region Watches

        private RelayCommand openWatchesCommand;
        public RelayCommand OpenWatchesCommand
        {
            get
            {
                return openWatchesCommand ??
                  (openWatchesCommand = new RelayCommand(obj =>
                  {
                      
                      try
                      {
                          WatchesViewModel watchesWindow = new WatchesViewModel(this);
                          if (watchesWindow.watchesView.ShowDialog() == true)
                          {
                              
                          }
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        private RelayCommand writeWatchCommand;
        public RelayCommand WriteWatchCommand
        {
            get
            {
                return writeWatchCommand ??
                  (writeWatchCommand = new RelayCommand(obj =>
                  {

                      DutyFloorWatch dutyFloorWatch = new DutyFloorWatch();
                      try
                      {
                          if(SelectedStudent == null)
                          {
                              throw new Exception("Ошибка! Выберите студента!");
                          }
                          WatchWriteViewModel watchWriteViewModel = new WatchWriteViewModel(SelectedStudent, dutyFloorWatch);
                          if (watchWriteViewModel.watchWriteView.ShowDialog() == true)
                          {
                              dutyFloorWatch = watchWriteViewModel.SelectedDutyFloorWatch;
                              db.DutyFloorWatches.Add(dutyFloorWatch);
                              db.SaveChanges();

                              //SelectedStudents = new ObservableCollection<Student>(db.Students.Local.Where(s => s.Room == SelectedRoomF));

                              //OnPropertyChanged("SelectedRoomF");
                              //OnPropertyChanged("SelectedStudents");
                          }
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }


        #endregion

    }
}
