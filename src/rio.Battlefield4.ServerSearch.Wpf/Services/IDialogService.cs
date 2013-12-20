namespace rio.Battlefield4.ServerSearch.Wpf.Services
{
    public interface IDialogService
    {
        #region Methods

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
        bool? ShowDialog<T>(object viewModel)
            where T : System.Windows.Window;

        #endregion Methods
    }
}