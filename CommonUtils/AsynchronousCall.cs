using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialMetricConversion.CommonUtils
{
    public class AsynchronousCall
    {
        BackgroundWorker _worker = new BackgroundWorker();
        Func<bool> _canExecute;

        /// <summary>  
        /// The constructor  
        /// </summary>  
        /// <param name="action">The action to be executed</param>  
        /// <param name="canExecute">Will be used to determine if the action can be executed</param>  
        /// <param name="completed">Will be invoked when the action is completed</param>  
        /// <param name="error">Will be invoked if the action throws an error</param>  
        public AsynchronousCall(Action action,
                                    Func<bool> canExecute = null,
                                    Action completed = null,
                                    Action<Exception> error = null
                                    )
        {

            _worker.DoWork += (s, e) =>
            {
                action();
            };

            _worker.RunWorkerCompleted += (s, e) =>
            {

                if (e.Error == null)
                    completed();

                if (error != null && e.Error != null)
                    error(e.Error);

            };

            _canExecute = canExecute;
        }


        /// <summary>  
        /// To cancel an ongoing execution  
        /// </summary>  
        public void Cancel()
        {
            if (_worker.IsBusy)
            {
                _worker.WorkerSupportsCancellation = true;
                _worker.CancelAsync();
            }
        }

        /// <summary>  
        /// Note that this will return false if the worker is already busy  
        /// </summary>  
        /// <param name="parameter"></param>  
        /// <returns></returns>  
        public bool CanExecute(object parameter)
        {
            return (_canExecute == null) ?
                    !(_worker.IsBusy) : !(_worker.IsBusy)
                        && _canExecute();
        }

        /// <summary>  
        /// Here we'll invoke the background worker  
        /// </summary>  
        /// <param name="parameter"></param>  
        public void Execute(object parameter)
        {
            _worker.RunWorkerAsync();
        }
    }
}
