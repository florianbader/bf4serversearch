using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace rio.Battlefield4.ServerSearch.Wpf
{
    /// <summary>
    /// A base implementation for the <see cref="System.ComponentModel.INotifyPropertyChanged"/> interface.
    /// </summary>
    public abstract class NotifyOnPropertyChangedBase : INotifyPropertyChanged
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyOnPropertyChangedBase"/> class.
        /// </summary>
        public NotifyOnPropertyChangedBase()
        {
            IsPropertyChangedNotificationEnabled = true;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether notifcations about property changes are enabled..
        /// </summary>
        /// <value>
        ///   <c>true</c> if notifications are enabled; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsPropertyChangedNotificationEnabled
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Called when a property value has changed.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        protected void OnPropertyChanged<TResult>(Expression<Func<TResult>> propertyExpression)
        {
            OnPropertyChanged(((MemberExpression)propertyExpression.Body).Member.Name);
        }

        /// <summary>
        /// Sets the property/backing field if the value has been modified and notifies any attached listener about the change.
        /// </summary>
        /// <typeparam name="TReturn">The type of the property.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="backingField">The backing field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected Boolean SetAndNotifyIfChanged<TReturn>(
            Expression<Func<TReturn>> propertyExpression,
            ref TReturn backingField,
            TReturn value)
        {
            ParameterException.CheckDefault(() => propertyExpression);

            if (!EqualityComparer<TReturn>.Default.Equals(backingField, value))
            {
                backingField = value;

                string propertyName = GetPropertyName<TReturn>(propertyExpression);
                OnPropertyChanged(propertyName);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the property/backing field if the value has been modified and notifies any attached listener about the change.
        /// </summary>
        /// <typeparam name="TReturn">The type of the property.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="backingField">The backing field.</param>
        /// <param name="value">The value.</param>
        /// <param name="action">The action to be called in case the property value has been changed. The parameter of the action contains the value of the property before it has been changed.</param>
        /// <returns></returns>
        protected Boolean SetAndNotifyIfChanged<TReturn>(
            Expression<Func<TReturn>> propertyExpression,
            ref TReturn backingField,
            TReturn value,
            Action<TReturn> action)
        {
            ParameterException.CheckDefault(() => propertyExpression);
            ParameterException.CheckDefault(() => action);

            if (!EqualityComparer<TReturn>.Default.Equals(backingField, value))
            {
                var previousBackingFieldValue = backingField;
                backingField = value;

                string propertyName = GetPropertyName<TReturn>(propertyExpression);
                OnPropertyChanged(propertyName);

                action.Invoke(previousBackingFieldValue);

                return true;
            }

            return false;
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Gets the name of the property from the specified property expression.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="property">The property expression.</param>
        /// <returns></returns>
        private String GetPropertyName<TReturn>(Expression<Func<TReturn>> property)
        {
            ParameterException.CheckDefault(() => property);

            String propertyName = null;

            try
            {
                var propertyExpression = property.Body as MemberExpression;
                if (propertyExpression.Expression.NodeType != ExpressionType.Constant)
                    throw new ArgumentException("The property expression must be of the form '() => SomeProperty'");

                propertyName = propertyExpression.Member.Name;
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException("The property expression must be of the form '() => SomeProperty'");
            }

            return propertyName;
        }

        #endregion Private Methods

        #endregion Methods

        #region Other

        /// <summary>
        /// Notifies any attached listener about the change of the property.
        /// </summary>
        /// <typeparam name="TReturn">The type of the property.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        protected internal void Notify<TReturn>(Expression<Func<TReturn>> propertyExpression)
        {
            ParameterException.CheckDefault(() => propertyExpression);

            string propertyName = GetPropertyName<TReturn>(propertyExpression);
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Called when a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected internal void OnPropertyChanged(string propertyName)
        {
            if (!IsPropertyChangedNotificationEnabled)
                return;

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected internal void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (!IsPropertyChangedNotificationEnabled)
                return;

            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion Other
    }
}