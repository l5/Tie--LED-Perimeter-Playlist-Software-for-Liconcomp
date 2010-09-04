using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Tie
{
    /// <summary>
    /// Eine Basisklasse für ICommand Implementierungen.
    /// </summary>
    internal abstract class Command : ICommand
    {
        /// <summary>
        /// Der Dispatcher der Anwendung.
        /// </summary>
        private readonly Dispatcher _dispatcher;

        /// <summary>
        /// Standardkonstruktor.
        /// </summary>
        protected Command()
        {
            if (Application.Current != null)
            {
                _dispatcher = Application.Current.Dispatcher;
            }
            else
            {
                //this is useful for unit tests where there is no application running
                _dispatcher = Dispatcher.CurrentDispatcher;
            }

            Debug.Assert(_dispatcher != null);
        }

        /// <summary>
        /// Wird ausgelöst, wenn der Zustand der Anwendung sich verändert hat.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Stellt fest, ob dieses Kommando ausgeführt werden kann oder nicht.
        /// </summary>
        /// <param name="parameter">Der Parameter für das Kommando. Kann <see langword="null"/> sein.</param>
        /// <returns>
        /// <see langword="true"/> wenn das Kommando ausführbar ist, sonst <see langword="false"/>.
        /// </returns>
        public abstract bool CanExecute(object parameter);

        /// <summary>
        /// Führt dieses Kommando aus.
        /// </summary>
        /// <param name="parameter">Der Parameter für das Kommando. Kann <see langword="null"/> sein.</param>
        public abstract void Execute(object parameter);

        /// <summary>
        /// Löst das <see cref="CanExecuteChanged"/> Ereignis aus.
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.Invoke((ThreadStart)OnCanExecuteChanged, DispatcherPriority.Normal);
            }
            else
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}