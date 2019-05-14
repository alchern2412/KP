namespace KP
{
    internal class StudentWindowViewModel
    {

        public Student Student { get; private set; }
        public StudentWindowView studentWindowView;

        public StudentWindowViewModel(Student s)
        {
            studentWindowView = new StudentWindowView();
            Student = s;
            studentWindowView.DataContext = Student;
        }

        private RelayCommand accept1Command;
        public RelayCommand Accept1Command
        {
            get
            {
                return accept1Command ??
                  (accept1Command = new RelayCommand(obj =>
                  {
                      studentWindowView.DialogResult = true;

                  }));
            }
        }
    }
}