using Microsoft.Win32;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace KP
{
    public class DefaultDialogService
    {
        public string FilePath { get; set; }

        public BitmapImage OpenFileDialog()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                return new BitmapImage(new Uri(op.FileName));
            }
            return null;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
