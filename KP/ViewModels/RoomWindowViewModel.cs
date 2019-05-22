using ReactiveValidation;
using ReactiveValidation.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;

namespace KP
{
    public class RoomWindowViewModel : ValidatableObject
    {

        private IObjectValidator GetValidator()
        {
            var builder = new ValidationBuilder<RoomWindowViewModel>();

            builder.RuleFor(vm => vm.RoomNumber).Must(BeAValidRoom).WithMessage("Только цифры. Напр: 102 {1 - этаж, 02 - комн.}").LessThan(10000);
            
            return builder.Build(this);
        }

        private bool BeAValidRoom(int number)
        {
            if (Regex.IsMatch(number.ToString(), @"^([1-9]{1}[0-9]{2})$"))
            {
                Room.Number = RoomNumber;
                BtnOk = true;
                return true;
            }
            else
            {
                BtnOk = false;
                return false;
            }
        }

        public Room Room { get; private set; }

        private bool btnOk;
        public bool BtnOk
        {
            get { return btnOk; }
            set
            {
                btnOk = value;
                OnPropertyChanged("BtnOk");
            }
        }

        private int roomNumber;
        public int RoomNumber
        {
            get { return roomNumber; }
            set
            {
                roomNumber = value;
                OnPropertyChanged("RoomNumber");
            }
        }



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
                RoomNumber = Room.Number;

                Flag = false;
            }
            else
            {
                Flag = true;
            }

            Validator = GetValidator();
            SelectedBedCount = (Room.Bed != null) ? (int)Room.Bed : 4;
            SelectedChairCount = (Room.Chair != null) ? (int)Room.Chair : 4;
            SelectedNightstandCount = (Room.Nightstand != null) ? (int)Room.Nightstand : 4;

            roomWindowView = new RoomWindowView();
            roomWindowView.DataContext = this;
        }
    }
}