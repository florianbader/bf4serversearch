using rio.Battlefield4.ServerSearch.Core.Entities;
using rio.Battlefield4.ServerSearch.Wpf.ViewModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace rio.Battlefield4.ServerSearch.Wpf.Views
{
    public partial class MainView : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainView"/> class.
        /// </summary>
        public MainView()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }

        protected void MouseDoubleClickEventHandler(object sender, EventArgs args)
        {
            var row = sender as DataGridRow;
            if (row == null)
                return;

            var server = row.Item as Server;
            if (server == null)
                return;

            string uri = string.Format("{0}{1}", @"http://battlelog.battlefield.com/bf4/servers/show/pc/", server.Guid);
            Process.Start(new ProcessStartInfo(uri));
        }
    }
}