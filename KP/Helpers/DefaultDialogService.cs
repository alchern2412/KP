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
        public bool SaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files(*.xlsx)|*.xlsx|All files(*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                return true;
            }
            return false;
        }
    }
}
