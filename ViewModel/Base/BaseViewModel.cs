using System;
using System.ComponentModel;

namespace OrangeVote.ViewModel.Base
{
    /// <summary>
    /// Basic view model functions.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual bool IsNew { get; protected set; }

        public virtual bool IsDirty { get; set; }

        protected virtual void RaisePropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// Basic view model which wraps a model object.
    /// </summary>
    public abstract class BaseViewModel<T> : BaseViewModel
    {
        public BaseViewModel(T model)
            : base()
        {
            Model = model;
        }

        /// <summary>
        /// Gets or sets if the view model houses a new object.
        /// </summary>
        /// <remarks>A view model is designated as new by the model object not previously existing.</remarks>
        public override bool IsNew
        {
            get { return Model == null; }

            protected set { throw new NotImplementedException(); }
        }

        public T Model { get; protected set; }

        /// <summary>
        /// Creates a new model object that can be used for persistence so the underlying model object doesn't get corrupted.
        /// </summary>
        /// <returns>The new model object to persist or pass.</returns>
        /// <remarks>NOTE:  This should start by creating a new model object and not use the underlying model!</remarks>
        public virtual T ToModel()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reloads the view model with the specified model.
        /// </summary>
        /// <param name="model">The updated model object.</param>
        /// <remarks>NOTE:  Separation must be kept between the view model and model.  Changes by the UI should not impact the
        /// model object.  Any updates or saves that are success should come back through services to the view model.</remarks>
        public virtual void Reload(T model)
        {
            // The override implementation should set view model properties to null in order to reload
            // the values in the UI, call this base method, then raise property changed events.
            Model = model;
            IsDirty = false;
        }
    }
}