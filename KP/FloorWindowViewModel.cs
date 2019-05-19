using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KP
{
    class FloorWindowViewModel
    {
        public Floor Floor { get; private set; }
        public FloorWindowView floorWindowView;

        public FloorWindowViewModel(Floor f)
        {
            floorWindowView = new FloorWindowView();
            Floor = f;
            floorWindowView.DataContext = Floor;
        }
    }
}
