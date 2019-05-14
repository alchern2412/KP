using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KP
{
    class RoomWindowViewModel
    {
        public Room Room { get; private set; }
        public RoomWindowView roomWindowView;

        public RoomWindowViewModel(Room r)
        {
            roomWindowView = new RoomWindowView();
            Room = r;
            roomWindowView.DataContext = Room;
        }

        private RelayCommand accept1Command;
        public RelayCommand Accept1Command
        {
            get
            {
                return accept1Command ??
                  (accept1Command = new RelayCommand(obj =>
                  {
                      roomWindowView.DialogResult = true;

                  }));
            }
        }
    }
}