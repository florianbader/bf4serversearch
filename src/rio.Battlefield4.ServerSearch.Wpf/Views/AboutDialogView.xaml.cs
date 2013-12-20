using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace rio.Battlefield4.ServerSearch.Wpf.Views
{
    public partial class AboutDialogView : Window
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutDialogView"/> class.
        /// </summary>
        public AboutDialogView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        #region Private Methods

        /// <summary>
        /// Handles the RequestNavigate event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Navigation.RequestNavigateEventArgs"/> instance containing the event data.</param>
        private void RequestNavigateEventHandler(object sender, RequestNavigateEventArgs e)
        {
            string uri = e.Uri.AbsoluteUri;
            Process.Start(new ProcessStartInfo(uri));

            e.Handled = true;
        }

        #endregion Private Methods

        #endregion Methods
    }
}