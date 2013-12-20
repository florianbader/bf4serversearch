using System.Reflection;

namespace rio.Battlefield4.ServerSearch.Wpf.ViewModels
{
    internal class AboutDialogViewModel
    {
        /// <summary>
        /// Gets the version information.
        /// </summary>
        /// <value>
        /// The version information.
        /// </value>
        public string VersionInfo
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }
    }
}