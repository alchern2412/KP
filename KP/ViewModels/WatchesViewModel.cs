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
using System.Windows.Markup;

namespace KP
{
    public class WatchesViewModel : INotifyPropertyChanged
    {
        public WatchesView watchesView;
        public ApplicationViewModel applicationViewModel {get;set;}

        public WatchesViewModel(ApplicationViewModel applicationViewModel)
        {
            this.applicationViewModel = applicationViewModel;

            watchesView = new WatchesView();
            watchesView.Language = XmlLanguage.GetLanguage("ru-RU");
            watchesView.DataContext = this;

            applicationViewModel.db.Rooms.Load();
            applicationViewModel.db.Floors.Load();
            applicationViewModel.db.Faculties.Load();
            applicationViewModel.db.StudSovietMembers.Load();
            applicationViewModel.db.DutyFloorWatches.Load();

            applicationViewModel.db.Students.Load();

            SelectedDutyWatches = new ObservableCollection<DutyFloorWatch>(applicationViewModel.db.DutyFloorWatches.Where(s => s.Type == "Вахта"));
            OnPropertyChanged("SelectedDutyWatches");

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private Floor selectedFloor;
        public Floor SelectedFloor
        {
            get { return selectedFloor; }
            set
            {
                selectedFloor = value;
                SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(applicationViewModel.db.DutyFloorWatches.Where(s => s.Type == "Этаж"));
                SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(SelectedFloorWatches.Where(s => s.Student.Room.Floor.ToString() == SelectedFloor.ToString()));
                OnPropertyChanged("SelectedFloor");
                OnPropertyChanged("SelectedFloorWatches");
            }
        }

        private DutyFloorWatch selectedWatch;
        public DutyFloorWatch SelectedWatch
        {
            get { return selectedWatch; }
            set
            {
                selectedWatch = value;
                OnPropertyChanged("SelectedWatch");
            }
        }


        public ObservableCollection<DutyFloorWatch> SelectedFloorWatches { get; set; }
        public ObservableCollection<DutyFloorWatch> SelectedDutyWatches { get; set; }


        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged("SearchText");

                //SelectedStudents = new ObservableCollection<Student>(db.Students.Local.Where(s => s.FirstName.ToUpper().Contains(searchText.ToUpper()) || s.LastName.ToUpper().Contains(searchText.ToUpper())));
                if (SearchText == null || SearchText == "")
                {
                    SelectedDutyWatches = new ObservableCollection<DutyFloorWatch>(applicationViewModel.db.DutyFloorWatches.Where(s => s.Type == "Вахта"));
                    if (SelectedFloor != null)
                    {
                        SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(applicationViewModel.db.DutyFloorWatches.Where(s => s.Type == "Этаж" /*&& s.Student.Room.Floor.Equals(SelectedFloor)*/));
                        SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(SelectedFloorWatches.Where(s => s.Student.Room.Floor.ToString() == SelectedFloor.ToString()));
                    }

                }
                else
                {
                    SelectedDutyWatches = new ObservableCollection<DutyFloorWatch>(applicationViewModel.db.DutyFloorWatches.Where(s => s.Type == "Вахта" && (s.Student.FirstName.ToUpper().Contains(searchText.ToUpper()) || s.Student.LastName.ToUpper().Contains(searchText.ToUpper()) || s.Date.ToString().Contains(SearchText)  )));

                    if (selectedFloor != null)
                    {
                        SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(applicationViewModel.db.DutyFloorWatches.Where(s => s.Type == "Этаж" /*&& s.Student.Room.Floor.Equals(SelectedFloor)*/));
                        SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(SelectedFloorWatches.Where(s => s.Student.Room.Floor.ToString() == SelectedFloor.ToString() && (s.Student.FirstName.ToUpper().Contains(searchText.ToUpper()) || s.Student.LastName.ToUpper().Contains(searchText.ToUpper()) || s.Date.ToString().Contains(SearchText) )));
                    }
                }
                OnPropertyChanged("SelectedFloorWatches");
                OnPropertyChanged("SelectedDutyWatches");
                //string sss = String.Empty;
                //foreach(var i in applicationViewModel.db.DutyFloorWatches.Where(s => s.Type == "Вахта"))
                //{
                //    sss += i.Date.ToString() + "  ";
                //}
                //MessageBox.Show(sss);
            }
        }

        #region watches commands

        private RelayCommand sortAscendingWatchesCommand;
        public RelayCommand SortAscendingWatchesCommand
        {
            get
            {

                return sortAscendingWatchesCommand ??
                  (sortAscendingWatchesCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          
                          SelectedDutyWatches = new ObservableCollection<DutyFloorWatch>(applicationViewModel.db.DutyFloorWatches.OrderBy(s => s.TimeStart).OrderBy(s => s.Date));
                          if (SelectedFloor != null)
                          {
                              SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(applicationViewModel.db.DutyFloorWatches.Where(s => s.Type == "Этаж"));
                              SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(SelectedFloorWatches.Where(s => s.Student.Room.Floor.ToString() == SelectedFloor.ToString()));
                              SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(SelectedFloorWatches.OrderBy(s => s.TimeStart).OrderBy(s => s.Date));
                          }
                              OnPropertyChanged("SelectedDutyWatches");
                          OnPropertyChanged("SelectedFloorWatches");
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));

            }
        }

        private RelayCommand sortDescendingWatchesCommand;
        public RelayCommand SortDescendingWatchesCommand
        {
            get
            {

                return sortDescendingWatchesCommand ??
                  (sortDescendingWatchesCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          SelectedDutyWatches = new ObservableCollection<DutyFloorWatch>(applicationViewModel.db.DutyFloorWatches.OrderByDescending(s => s.TimeStart).OrderByDescending(s => s.Date));
                          if (SelectedFloor != null)
                          {
                              SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(applicationViewModel.db.DutyFloorWatches.Where(s => s.Type == "Этаж"));
                              SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(SelectedFloorWatches.Where(s => s.Student.Room.Floor.ToString() == SelectedFloor.ToString()));
                              SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(SelectedFloorWatches.OrderByDescending(s => s.TimeStart).OrderByDescending(s => s.Date));

                          }
                          OnPropertyChanged("SelectedDutyWatches");
                          OnPropertyChanged("SelectedFloorWatches");
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));

            }
        }
        bool flag = true;
        private RelayCommand sortWatchesFromNowCommand;
        public RelayCommand SortWatchesFromNowCommand
        {
            get
            {

                return sortWatchesFromNowCommand ??
                  (sortWatchesFromNowCommand = new RelayCommand(obj =>
                  {
                      try
                      {



                          if (flag)
                          {
                              SelectedDutyWatches = new ObservableCollection<DutyFloorWatch>(applicationViewModel.db.DutyFloorWatches.OrderBy(s => s.TimeStart).OrderBy(s => s.Date).Where(s => s.Date >= DateTime.Now));
                              if (SelectedFloor != null)
                              {
                                  SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(applicationViewModel.db.DutyFloorWatches.Where(s => s.Type == "Этаж"));
                                  SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(SelectedFloorWatches.Where(s => s.Student.Room.Floor.ToString() == SelectedFloor.ToString()));
                                  SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(SelectedFloorWatches.OrderBy(s => s.TimeStart).OrderBy(s => s.Date).Where(s => s.Date >= DateTime.Now));
                              }
                              flag = false;
                          }
                          else
                          {
                              SelectedDutyWatches = new ObservableCollection<DutyFloorWatch>(applicationViewModel.db.DutyFloorWatches.OrderByDescending(s => s.TimeStart).OrderByDescending(s => s.Date).Where(s => s.Date >= DateTime.Now));
                              if (SelectedFloor != null)
                              {
                                  SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(applicationViewModel.db.DutyFloorWatches.Where(s => s.Type == "Этаж"));
                                  SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(SelectedFloorWatches.Where(s => s.Student.Room.Floor.ToString() == SelectedFloor.ToString()));
                                  SelectedFloorWatches = new ObservableCollection<DutyFloorWatch>(SelectedFloorWatches.OrderByDescending(s => s.TimeStart).OrderByDescending(s => s.Date).Where(s => s.Date >= DateTime.Now));

                              }
                                  flag = true;
                          }
                          OnPropertyChanged("SelectedDutyWatches");
                          OnPropertyChanged("SelectedFloorWatches");
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
