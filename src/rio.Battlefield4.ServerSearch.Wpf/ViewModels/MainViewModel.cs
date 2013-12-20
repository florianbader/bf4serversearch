using rio.Battlefield4.ServerSearch.Core;
using rio.Battlefield4.ServerSearch.Core.Entities;
using rio.Battlefield4.ServerSearch.Wpf.Services;
using rio.Battlefield4.ServerSearch.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace rio.Battlefield4.ServerSearch.Wpf.ViewModels
{
    internal class MainViewModel : NotifyOnPropertyChangedBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            _searcher = new Searcher();
            _dialogService = new DialogService();

            RefreshServerListCommand = new DelegateCommand(
                x => RefreshServerList(),
                x => !_isSearching);

            ShowAboutDialogCommand = new DelegateCommand(
                x => ShowAboutDialog(),
                x => true);

            ShowSettingsCommand = new DelegateCommand(
                x => { },
                x => false);
        }

        #endregion Constructors

        #region Fields

        private readonly IDialogService _dialogService;
        private readonly Searcher _searcher;

        private bool _isSearching;
        private IList<Server> _servers = new List<Server>();

        #endregion Fields

        #region Commands

        /// <summary>
        /// Gets the ShowSettings command.
        /// </summary>
        /// <value>
        /// The ShowSettings command.
        /// </value>
        public ICommand ShowSettingsCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the RefreshServerList command.
        /// </summary>
        /// <value>
        /// The RefreshServerList command.
        /// </value>
        public ICommand RefreshServerListCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ShowAboutDialog command.
        /// </summary>
        /// <value>
        /// The ShowAboutDialog command.
        /// </value>
        public ICommand ShowAboutDialogCommand
        {
            get;
            private set;
        }

        #endregion Commands

        #region Properties

        /// <summary>
        /// Gets or sets the available servers.
        /// </summary>
        /// <value>
        /// The servers.
        /// </value>
        public IList<Server> Servers
        {
            get { return _servers; }
            set { SetAndNotifyIfChanged(() => Servers, ref _servers, value); }
        }

        #endregion Properties

        #region Methods

        #region Private Methods

        /// <summary>
        /// Refreshes the server list.
        /// </summary>
        private void RefreshServerList()
        {
            if (_isSearching)
                return;

            _isSearching = true;

            Task.Factory.StartNew(new Action(() =>
                {
                    var searchResults = _searcher.Search();
                    var servers = searchResults.OrderByDescending(x => x.Weight).Select(x => x.Server).ToList();

                    Servers = servers;

                    _isSearching = false;
                }));
        }

        /// <summary>
        /// Shows the about dialog.
        /// </summary>
        private void ShowAboutDialog()
        {
            _dialogService.ShowDialog<AboutDialogView>(new AboutDialogViewModel());
        }

        #endregion Private Methods

        #endregion Methods
    }
}