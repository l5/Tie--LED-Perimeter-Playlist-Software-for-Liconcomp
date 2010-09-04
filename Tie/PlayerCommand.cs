using System.Windows.Threading;
using Tie.Model;

namespace Tie
{
    /// <summary>
    /// Eine kleine Hilfsklasse, die Funktionalität implementiert, die alle
    /// Player Kommandos benötigen.
    /// </summary>
    internal abstract class PlayerCommand : Command
    {
        public PlayerCommand()
            : base()
        {
        }

        /// <summary>
        /// Gibt den aktuellen Dispatcher für die Applikation zurück.
        /// </summary>
        /// <value>den aktuellen Dispatcher für die Applikation.</value>
        public Dispatcher Dispatcher
        {
            get
            {
                if (App.Current.Dispatcher != null)
                    return App.Current.Dispatcher;
                else
                    return Dispatcher.CurrentDispatcher;
            }
        }

        #region Command Members

        /// <summary>
        /// Stellt fest, ob dieses Kommando ausgeführt werden kann oder nicht.
        /// </summary>
        /// <param name="parameter">Der Parameter für das Kommando. Kann <see langword="null"/> sein.</param>
        /// <returns>
        /// 	<see langword="true"/> wenn das Kommando ausführbar ist, sonst <see langword="false"/>.
        /// </returns>
        public override bool CanExecute(object parameter)
        {
            return (Context.Instance.Player != null);
        }

        #endregion
    }
}
