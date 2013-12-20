using System;
using System.Windows;

namespace rio.Battlefield4.ServerSearch.Wpf.Services
{
    public class DialogService : IDialogService
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Shows a new dialog based on the specified view model.
        /// </summary>
        /// <typeparam name="T">The type of the dialog view.</typeparam>
        /// <param name="viewModel">The view model.</param>
        /// <returns>
        /// A System.Nullable<T> value of type System.Boolean that specifies whether
        /// the activity was accepted (true) or canceled (false). The return value is
        /// the value of the System.Windows.Window.DialogResult property before a window
        /// closes.
        /// </returns>
        public bool? ShowDialog<T>(object viewModel)
            where T : System.Windows.Window
        {
            return ShowDialog(viewModel, typeof(T));
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Shows a new dialog based on the specified view model and dialog view type.
        /// </summary>
        /// <typeparam name="T">The type of the dialog view.</typeparam>
        /// <param name="viewModel">The view model.</param>
        /// <returns>
        /// A System.Nullable<T> value of type System.Boolean that specifies whether
        /// the activity was accepted (true) or canceled (false). The return value is
        /// the value of the System.Windows.Window.DialogResult property before a window
        /// closes.
        /// </returns>
        private bool? ShowDialog(object viewModel, Type dialogViewType)
        {
            Window dialog = (Window)Activator.CreateInstance(dialogViewType);
            dialog.Owner = Application.Current.MainWindow;
            dialog.DataContext = viewModel;

            return dialog.ShowDialog();
        }

        #endregion Private Methods

        #endregion Methods
    }
}