using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CodeDesigner.Interface.Utility.File;
using Microsoft.Win32;

namespace CodeDesigner.Interface.Windows
{
    /// <summary>
    /// Interaction logic for ProjectManager.xaml
    /// </summary>
    public partial class ProjectManager : Window
    {
        private bool dragEnabled = true;

        public ProjectManager()
        {
            InitializeComponent();
        }


        private void ProjectManagerWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragEnabled && e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void MouseEntered(object sender, MouseEventArgs e)
        {
            dragEnabled = true;
        }

        private void MouseLeft(object sender, MouseEventArgs e)
        {
            dragEnabled = false;
        }

        private void OpenButtonPressed(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            if (ofd.FileName != string.Empty)
                XMLLoader.loadProject(ofd.FileName);
        }

        private void CreateButtonPressed(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();

            if (sfd.FileName != string.Empty)
                XMLLoader.createProject(sfd.FileName);
        }
    }
}
