 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace KP
{
    public class WatchWriteViewModel : INotifyPropertyChanged
    {
        public WatchWriteView watchWriteView;

        

        public WatchWriteViewModel(Student student, DutyFloorWatch dutyFloorWatch)
        {
            this.SelectedStudent = student;
            this.SelectedDutyFloorWatch = dutyFloorWatch;

            SelectedDutyFloorWatch.Student = SelectedStudent;
            SelectedDutyFloorWatch.Date = DateTime.Now;
            SelectedDutyFloorWatch.TimeStart = new TimeSpan(0, 0, 0);
            SelectedDutyFloorWatch.TimeFinish = new TimeSpan(0, 0, 0);

            watchWriteView = new WatchWriteView();
            watchWriteView.Language = XmlLanguage.GetLanguage("ru-RU");
            watchWriteView.DataContext = this;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        Student selectedStudent; // факультет
        public Student SelectedStudent
        {
            get { return selectedStudent; }
            set
            {
                selectedStudent = value;
                OnPropertyChanged("SelectedStudent");
            }
        }

        DutyFloorWatch selectedDutyFloorWatch;
        public DutyFloorWatch SelectedDutyFloorWatch
        {
            get { return selectedDutyFloorWatch; }
            set
            {
                selectedDutyFloorWatch = value;
                OnPropertyChanged("SelectedDutyFloorWatch");
            }
        }


    }
}
