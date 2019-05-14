using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KP
{
    class ApplicationViewModel : INotifyPropertyChanged
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
                      FloorWindowViewModel floorWindow = new FloorWindowViewModel(new Floor());
                      if (floorWindow.floorWindowView.ShowDialog() == true)
                      {
                          Floor floor = floorWindow.Floor;
                          Floors.Insert(Floors.Count, floor);
                          db.Floors.Add(floor);
                          OnPropertyChanged("Floors");
                          db.SaveChanges();
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
                                    Rooms.Remove(Rooms[i]);
                                }
                            }

                            Floors.Remove(floor);
                            
                            /*Удалить из БД комнаты этажа*/
                            foreach(var i in db.Rooms)
                            {
                                if(i.Floor == floor)
                                {
                                    db.Rooms.Remove(i);
                                }
                            }

                            /*Удалить из БД этаж*/
                            db.Floors.Remove(floor);

                            OnPropertyChanged("SelectedRooms");
                            OnPropertyChanged("Rooms");
                            

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
                selectedRooms = new ObservableCollection<Room>(selectedFloor.Rooms);
                OnPropertyChanged("SelectedRooms");
                OnPropertyChanged("SelectedFloor");
            }
        }

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
                            Rooms.Remove(room);
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
            }
        }

        #endregion

        #region CommandsStudents

        RelayCommand addStudentCommand;
        RelayCommand editStudentCommand;
        RelayCommand deleteStudentCommand;

        #endregion
    }
}
