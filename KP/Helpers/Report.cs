using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP
{
    public static class Report
    {
        
        public static void StudentWrite(ObservableCollection<Student> students, Excel ex)
        {
            List<Floor> floors = new List<Floor>(students.Select(s => s.Room).Distinct().Select(s => s.Floor).Distinct());
            floors = new List<Floor>(floors.OrderBy(f => f.Number));                                                                       
            int row = 1;
            int col = 1;
            Student st;
            bool flag = false;

            ex.WriteToCell(row, col, "Этаж");
            ex.WriteToCell(row, col + 1, "Комн");
            ex.WriteToCell(row, col+2, "№");
            ex.WriteToCell(row, col+3, "Фамилия");
            ex.WriteToCell(row, col + 4, "Имя");
            ex.WriteToCell(row, col + 5, "Отчество");
            ex.WriteToCell(row, col + 6, "Курс");
            ex.WriteToCell(row, col + 7, "Группа");
            ex.WriteToCell(row, col + 8, "Фак-т");
            ex.WriteToCell(row, col + 9, "Информация");

            row++;
            for (int i = 0; i < floors.Count; i++)// floors
            {
                ex.WriteToCell(row, col, floors[i].Number.ToString());  // номер этажа
                row++;

                for (int j = 0; j < floors[i].Rooms.Count; j++) //  rooms
                {
                    flag = false;
                    foreach (var s in students) // живут ли нужные студенты в комнате
                    {
                        if (s.Room.Number.Equals(floors[i].Rooms[j].Number))
                        {
                            flag = false;
                            break;
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    if(flag)
                    {
                        continue;
                    }


                    ex.WriteToCell(row, col + 1, floors[i].Rooms[j].Number.ToString());  // номер комнаты
                    row++;

                    for (int k = 0; k < floors[i].Rooms[j].Students.Count; k++, row++)
                    {
                        st = floors[i].Rooms[j].Students[k];
                        foreach (var s in students)
                        {
                            if (!s.Equals(st))
                            {
                                continue;
                            }

                            ex.WriteToCell(row, col + 2, (k + 1).ToString());


                            ex.WriteToCell(row, col + 3, st.LastName);
                            ex.WriteToCell(row, col + 4, st.FirstName);
                            ex.WriteToCell(row, col + 5, (st.SecondName != null && st.SecondName != "" ? st.SecondName : "[-]"));
                            ex.WriteToCell(row, col + 6, st.Course.ToString());
                            ex.WriteToCell(row, col + 7, st.Group.ToString());
                            ex.WriteToCell(row, col + 8, st.Faculty.FacultyName);
                            ex.WriteToCell(row, col + 9, (st.Note != null && st.Note != "") ? st.Note : "[-]");
                        }

                    }
                }
            }
            row++;
            ex.WriteToCell(row, col, "---");
            ex.WriteToCell(row, col + 1, "---");
            ex.WriteToCell(row, col + 2, "---");
            ex.WriteToCell(row, col + 3, "---");
            ex.WriteToCell(row, col + 4, "---");
            ex.WriteToCell(row, col + 5, "---");
            ex.WriteToCell(row, col + 6, "---");
            ex.WriteToCell(row, col + 7, "---");
            ex.WriteToCell(row, col + 8, "---");
            ex.WriteToCell(row, col + 9, "---");
            row += 2;

            ex.WriteToCell(row, col, "Студентов:");
            ex.WriteToCell(row, col + 1, students.Count.ToString());


        }

        public static void RoomWrite(List<Room> rooms, Excel ex)
        {
            int row = 1;
            int col = 1;

            int beds = 0;
            int chairs = 0;
            int nightstands = 0;
            int students = 0;

            /*шапка*/
            ex.WriteToCell(row, col, "№ комнаты");
            ex.WriteToCell(row, col + 1, "Кровати");
            ex.WriteToCell(row, col + 2, "Стулья");
            ex.WriteToCell(row, col + 3, "Тумбочки");
            ex.WriteToCell(row, col + 4, "Студенты");

            row++;
            /*Осн инф-ция*/
            foreach(var room in rooms)
            {
                ex.WriteToCell(row, col, room.Number.ToString());
                ex.WriteToCell(row, col+1, room.Bed.ToString());
                ex.WriteToCell(row, col + 2, room.Chair.ToString());
                ex.WriteToCell(row, col + 3, room.Nightstand.ToString());
                ex.WriteToCell(row, col + 4, room.Students.Count.ToString());

                beds += (int)room.Bed;
                chairs += (int)room.Chair;
                nightstands += (int)room.Nightstand;
                students += (int)room.Students.Count;

                row++;
            }

            ex.WriteToCell(row, col, "---");
            ex.WriteToCell(row, col + 1, "---");
            ex.WriteToCell(row, col + 2, "---");
            ex.WriteToCell(row, col + 3, "---");
            ex.WriteToCell(row, col + 4, "---");
            row += 2;
            ex.WriteToCell(row, col, "ИТОГО:");
            ex.WriteToCell(row, col + 1, beds.ToString());
            ex.WriteToCell(row, col + 2, chairs.ToString());
            ex.WriteToCell(row, col + 3, nightstands.ToString());
            ex.WriteToCell(row, col + 4, students.ToString());
        }
    }
}


//FirstName = this.FirstName,
//                SecondName = this.SecondName,
//                LastName = this.LastName,
//                Note = this.Note,
//                Course = this.Course,
//                Group = this.Group,
//                Birthday = this.Birthday,
//                DateOfEntry = this.DateOfEntry,
//                DateOfDeparture = this.DateOfDeparture,
//                Room = this.Room,
//                Faculty = this.Faculty,
//                DutyFloorWatches = this.DutyFloorWatches,
//                StudSovietMember = this.StudSovietMember