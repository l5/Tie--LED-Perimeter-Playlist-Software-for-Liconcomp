using System.Linq;
using System.Windows.Input;
using Tie.Model;

namespace Tie
{
    /// <summary>
    /// Kommando um alle Elemente vom Bildschirm zu löschen.
    /// </summary>
    internal class BlankScreenCommand : PlayerCommand
    {
        /// <summary>
        /// Standardkonstruktor.
        /// </summary>
        public BlankScreenCommand()
            : base()
        {
        }

        #region Command Members

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter) && (Context.Instance.Player.Elements.Count() > 0);
        }

        public override void Execute(object parameter)
        {
            Context.Instance.Sync.Visible = false;
            Context.Instance.Player.RemoveAllElements();
            CommandManager.InvalidateRequerySuggested();
        }

        #endregion
    }
}
