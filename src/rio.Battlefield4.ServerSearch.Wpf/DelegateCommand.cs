using System;
using System.Windows.Input;

namespace rio.Battlefield4.ServerSearch.Wpf
{
    /// <summary>
    /// A delegate command.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">The underlying action of the command that will be executed.</param>
        public DelegateCommand(Action<object> execute)
            : this(execute, null)
        {
            ParameterException.CheckDefault(() => execute);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">The underlying action of the command that will be executed.</param>
        /// <param name="canExecute">Determines wether the command can be executed or not.</param>
        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">The underlying action of the command that will be executed.</param>
        /// <param name="canExecute">Determines wether the command can be executed or not.</param>
        /// <param name="postExecution">The action the will be executed right after the command has been executed.</param>
        public DelegateCommand(Action<object> execute, Predicate<object> canExecute, Action<object> postExecution)
            : this(execute, canExecute)
        {
            _postExecution = postExecution;
        }

        #endregion Constructors

        #region Fields

        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;
        private readonly Action<object> _postExecution;

        #endregion Fields

        #region Events

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Occurs when the command has been executed.
        /// </summary>
        public event EventHandler Executed;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is a default command.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is a default command; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefault
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Checks whether the CanExecute command can be executed.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        ///   <c>true</c> if the command can be executed; otherwise, <c>false</c>.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            try
            {
                return _canExecute(parameter);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Executes the Execute command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void Execute(object parameter)
        {
            try
            {
                _execute(parameter);

                if (_postExecution != null)
                    _postExecution(parameter);

                OnExecuted(this, EventArgs.Empty);
            }
            catch (Exception)
            {
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Called when the command has been executed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnExecuted(object sender, EventArgs e)
        {
            if (Executed != null)
                Executed(sender, e);
        }

        #endregion Private Methods

        #endregion Methods
    }
}