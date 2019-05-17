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

        #endregion

        public HostelContainer db;

        #region Collections
        public ObservableCollection<Room> rooms;
        public ObservableCollection<Room> Rooms
        {
            get { return rooms; }
            set
            {
                rooms = value;
                OnPropertyChanged("Rooms");
            }
        }

        public ObservableCollection<Floor> floors;
        public ObservableCollection<Floor> Floors
        {
            get { return floors; }
            set
            {
                floors = value;
                OnPropertyChanged("Floors");
            }
        }

        public ObservableCollection<Student> students;
        public ObservableCollection<Student> Students
        {
            get { return students; }
            set
            {
                students = value;
                OnPropertyChanged("Students");
            }
        }

        public ObservableCollection<Faculty> faculties;
        public ObservableCollection<Faculty> Faculties
        {
            get { return faculties; }
            set
            {
                faculties = value;
                OnPropertyChanged("Faculties");
            }
        }
        #endregion

        ////tmp
        //public Student st1;
        //public Student St1
        //{
        //    get { return st1; }
        //    set
        //    {
        //        st1 = value;
        //        OnPropertyChanged("St1");
        //    }
        //}
        /////////

        public ApplicationViewModel()
        {
            db = new HostelContainer();

            

            db.Rooms.Load();
            db.Floors.Load();
            db.Faculties.Load();
            db.Students.Load();

            db.Rooms.OrderBy(s => s.Number);

            Rooms = new ObservableCollection<Room>(db.Rooms.Local.ToBindingList().OrderBy(s => s.Number));
            Faculties = new ObservableCollection<Faculty>(db.Faculties.Local.ToBindingList());
            Students = new ObservableCollection<Student>(db.Students.Local.ToBindingList());
            Floors = new ObservableCollection<Floor>(db.Floors.Local.ToBindingList().OrderBy(s => s.Number));
            selectedRooms = new ObservableCollection<Room>();

            
            

            //st1 = Students.First();

            FloorSelectedIndex = 0;
            RoomSelectedIndex = 0;
            StudentSelectedIndex = 0;
        }

        #region CommandsFloor


        private int floorSelectedIndex;  // выбранный этаж
        public int FloorSelectedIndex
        {
            get { return floorSelectedIndex; }
            set
            {
                if (value == -1)
                {
                    floorSelectedIndex = 0;
                }
                else
                {
                    floorSelectedIndex = value;
                }
                
                OnPropertyChanged("SelectedRooms");
                OnPropertyChanged("FloorSelectedIndex");
                RoomSelectedIndex = 0;
            }
        }

        private int roomSelectedIndex;  // выбранная комната
        public int RoomSelectedIndex
        {
            get { return roomSelectedIndex; }
            set
            {
                if (value == -1)
                {
                    roomSelectedIndex = 0;
                    
                }
                else
                {
                    roomSelectedIndex = value;
                }
                
                OnPropertyChanged("RoomSelectedIndex");
                StudentSelectedIndex = 0;
            }
        }

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                    searchText = value;
                    OnPropertyChanged("SearchText");
                RoomSelectedIndex = -1;
                FloorSelectedIndex = -1;

                SelectedStudents = new ObservableCollection<Student>(Students.Where(s => s.FirstName.ToUpper().Contains(searchText.ToUpper()) || s.LastName.ToUpper().Contains(searchText.ToUpper())));
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
                                  Floors.Insert(Floors.Count, floor);
                                  db.Floors.Add(floor);
                                  OnPropertyChanged("Floors");
                                  db.SaveChanges();
                              }
                          }
                          catch (Exception ex)
                          {
                              MessageBox.Show(ex.Message);
                          }


                          //Floor floor = new Floor();
                          //Floors.Insert(0, floor);
                          //SelectedFloor = floor;

                          //db.Floors.Add(floor);
                          //db.SaveChanges();
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
                        Floor floor = obj as Floor;
                        if (floor != null)
                        {
                            


                            /*Удалить из observableCollection*/
                            for (int i = 0; i < Rooms.Count; i++)   
                            {
                                if (Rooms[i].Floor == floor)
                                {
                                    /*Удалить из observableCollection студентов*/
                                    for (int j = 0; j < Students.Count; j++)
                                    {
                                        if (Students[j].Room == Rooms[i])
                                        {
                                            //Students.Remove(Students[j]);
                                            SelectedStudents.Remove(Students[j]);
                                        }
                                    }

                                    SelectedRooms.Remove(Rooms[i]);
                                    OnPropertyChanged("SelectedRooms");
                                }
                            }

                            Floors.Remove(floor);
                            
                            /*Удалить из БД комнаты этажа*/
                            foreach(var i in db.Rooms)
                            {
                                if(i.Floor == floor)
                                {
                                    /*Удалить из БД комнаты этажа*/
                                    foreach (var j in db.Students)
                                    {
                                        if (j.Room == i)
                                        {
                                            db.Students.Remove(j);
                                        }
                                    }

                                    db.Rooms.Remove(i);
                                }
                            }

                            /*Удалить из БД этаж*/
                            db.Floors.Remove(floor);

                            OnPropertyChanged("SelectedRooms");
                            OnPropertyChanged("Rooms");
                            FloorSelectedIndex = 0;
                            RoomSelectedIndex = 0;
                            StudentSelectedIndex = 0;
                            OnPropertyChanged("FloorSelectedIndex");

                            db.SaveChanges();
                        }
                    },
                    (obj) => Floors.Count > 0));
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
                if (selectedFloor != null)
                {
                    selectedRooms = new ObservableCollection<Room>(selectedFloor.Rooms.OrderBy(s => s.Number));
                }
                OnPropertyChanged("SelectedRooms");
                OnPropertyChanged("SelectedFloor");
            }
        }

        /*Комнаты выбранного этажа, которые нужно показать*/
        private ObservableCollection<Room> selectedRooms;
        public ObservableCollection<Room> SelectedRooms
        {
            get { return selectedRooms; }
            set
            {
                
                selectedRooms = value;
                OnPropertyChanged("SelectedRooms");
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
                              Rooms.Insert(Rooms.Count, room);
                              SelectedRooms.Insert(SelectedRooms.Count, room);

                              db.Rooms.Add(room);
                              OnPropertyChanged("Rooms");
                              OnPropertyChanged("SelectedRooms");
                              db.SaveChanges();
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
                            /*Удалить из observableCollection студентов*/
                            for (int i = 0; i < Students.Count; i++)
                            {
                                if (Students[i].Room == room)
                                {
                                    SelectedStudents.Remove(Students[i]);
                                    Students.Remove(Students[i]);
                                }
                            }

                            Rooms.Remove(room);

                            /*Удалить из БД комнаты этажа*/
                            foreach (var i in db.Students)
                            {
                                if (i.Room == room)
                                {
                                    db.Students.Remove(i);
                                }
                            }
                            RoomSelectedIndex = 0;                           
                            SelectedRooms.Remove(room);
                            db.Rooms.Remove(room);
                            db.SaveChanges();
                        }
                    },
                    (obj) => Rooms.Count > 0));
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

        private Room selectedRoom;
        public Room SelectedRoomF
        {
            get { return selectedRoom; }
            set
            {
                selectedRoom = value;
                OnPropertyChanged("SelectedRoomF");
                if (selectedRoom != null)
                {
                    selectedStudents = new ObservableCollection<Student>(selectedRoom.Students);

                }
                OnPropertyChanged("SelectedRooms");
                OnPropertyChanged("SelectedFloor");
                OnPropertyChanged("SelectedStudents");
            }
        }

        #endregion

        #region CommandsStudents

        private ObservableCollection<Student> selectedStudents;
        public ObservableCollection<Student> SelectedStudents
        {
            get { return selectedStudents; }
            set
            {
                selectedStudents = value;
                OnPropertyChanged("SelectedStudents");
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
                      try
                      {
                          StudentWindowViewModel studentWindow = new StudentWindowViewModel(new Student(), Faculties, SelectedRoomF);
                          if (studentWindow.studentWindowView.ShowDialog() == true)
                          {
                              Student student = studentWindow.Student;

                              Students.Insert(Students.Count, student);
                              //SelectedRooms.Insert(SelectedRooms.Count, room);

                              db.Students.Add(student);
                              SelectedStudents.Add(student);
                              OnPropertyChanged("Students");
                              OnPropertyChanged("SelectedStudents");
                              db.SaveChanges();
                          }
                      }
                      catch (Exception ex)
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

        private Student selectedStudent;
        public Student SelectedStudent
        {
            get { return selectedStudent; }
            set
            {
                selectedStudent = value;
                OnPropertyChanged("SelectedStudent");
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


                            SelectedStudents.Remove(student);
                            db.Students.Remove(student);
                            StudentSelectedIndex = 0;
                            db.SaveChanges();
                        }
                    },
                    (obj) => Students.Count > 0));
            }
        }


        private RelayCommand editStudentCommand;
        public RelayCommand EditStudentCommand
        {
            get
            {
                return editStudentCommand ??
                    (editStudentCommand = new RelayCommand(obj =>
                    {
                        Student student = obj as Student;
                        if (student != null)
                        {
                            StudentWindowViewModel studentWindow = new StudentWindowViewModel(student, Faculties, SelectedStudent.Room);
                            if (studentWindow.studentWindowView.ShowDialog() == true)
                            {
                                student = studentWindow.Student;
                                Student tmp = (Student)student.Clone();

                                db.Students.Remove(student);
                                selectedStudents.Remove(student);

                                db.Students.Add(tmp);
                                Students.Add(tmp);
                                selectedStudents.Add(tmp);

                                //tmp.LastName = student.LastName;
                                //Students.Insert(Students.Count, student);
                                //SelectedRooms.Insert(SelectedRooms.Count, room);

                                //db.Students.Add(student);
                                //SelectedStudents.Add(student);
                                OnPropertyChanged("Students");
                                OnPropertyChanged("SelectedStudents");
                                db.SaveChanges();
                            }
                        }
                    }));
            }
        }

        #endregion


        #region ImageMethods

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        #endregion
    }
}
