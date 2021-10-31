using AdminGui.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdminGui.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GridViewColumnHeader listViewSortCol = null;

        public MainWindow()
        {
            
            InitializeComponent();

        }

        private void nameColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();

            SortDescription oldDescription = processListView.Items.SortDescriptions.FirstOrDefault();

            if (listViewSortCol != null)
            {
                processListView.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && oldDescription.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;

            processListView.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((ClientViewModel)this.DataContext).ClientList_SelectionChanged(sender, e);
        }
    }
}
