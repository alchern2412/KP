using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace KP
{
    public class RoomWindowViewModel
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public Room Room { get; private set; }


        int selectedBedCount;
        public int SelectedBedCount
        {
            get { return selectedBedCount; }
            set
            {
                selectedBedCount = value;
                Room.Bed = (int)selectedBedCount;
                OnPropertyChanged("SelectedBedCount");

                Room.Bed = value;
            }
        }

        bool flag;
        public bool Flag
        {
            get { return flag; }
            set
            {
                flag = value;
                OnPropertyChanged("Flag");
            }
        }

        int selectedChairCount;
        public int SelectedChairCount
        {
            get { return selectedChairCount; }
            set
            {
                selectedChairCount = value;
                Room.Chair = (int)selectedChairCount;
                OnPropertyChanged("SelectedChairCount");

                Room.Chair = value;
            }
        }

        int selectedNightstandCount;
        public int SelectedNightstandCount
        {
            get { return selectedNightstandCount; }
            set
            {
                selectedNightstandCount = value;
                Room.Nightstand = (int)selectedNightstandCount;
               
                OnPropertyChanged("SelectedNightstandCount");

                Room.Nightstand = value;
            }
        }

        public RoomWindowView roomWindowView;

        public RoomWindowViewModel(Room r)
        {
            
            Room = r;

            if (Room.Number != 0)
            {
                

                Flag = false;
            }
            else
            {
                Flag = true;
            }

            SelectedBedCount = (Room.Bed != null) ? (int)Room.Bed : 4;
            SelectedChairCount = (Room.Chair != null) ? (int)Room.Chair : 4;
            SelectedNightstandCount = (Room.Nightstand != null) ? (int)Room.Nightstand : 4;

            roomWindowView = new RoomWindowView();
            roomWindowView.DataContext = this;
        }

        //private RelayCommand accept1Command;
        //public RelayCommand Accept1Command
        //{
        //    get
        //    {
        //        return accept1Command ??
        //          (accept1Command = new RelayCommand(obj =>
        //          {
        //              roomWindowView.DialogResult = true;

        //          }));
        //    }
        //}
    }
}