using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Tie.Model;

namespace Tie.Controllers
{
    /// <summary>
    /// Controller für das Hauptfenster.
    /// </summary>
    public class MainWindowViewModel
    {
        /// <summary>
        /// Das Kommando für das Entfernen aller Elemente vom Bildschirm.
        /// </summary>
        private ICommand _blankScreenCommand = null;

        /// <summary>
        /// Das Kommando, um alle Playlists gleichzeitig abzuspielen und die Videos
        /// jeder Playlist nebeneinander anzuordnen.
        /// </summary>
        private ICommand _startFloatingPlaybackCommand = null;

        /// <summary>
        /// Kommando um alle Playlists untereinander anzuordnen und die Videos jeder
        /// Playlist nacheinander abzuspielen.
        /// </summary>
        private ICommand _startPlaylistPlaybackCommand = null;

        /// <summary>
        /// Log Meldungen.
        /// </summary>
        private ObservableCollection<string> _messages = new ObservableCollection<string>();

        /// <summary>
        /// Standardkonstruktor.
        /// </summary>
        public MainWindowViewModel()
        {
            LogMessage("Welcome!");
        }

        /// <summary>
        /// Fügt eine Textnachricht den Log Meldungen hinzu.
        /// </summary>
        /// <param name="Msg">Die hinzuzufügende Nachricht.</param>
        public void LogMessage(string Msg)
        {
            this._messages.Insert(0, Msg);
        }

        /// <summary>
        /// Gibt das Kommando zurück, welches den Playlist- Modus startet.
        /// </summary>
        public ICommand StartPlaylistPlaybackCommand
        {
            get
            {
                if (this._startPlaylistPlaybackCommand == null)
                    this._startPlaylistPlaybackCommand = new PlaylistPlaybackCommand();
                return this._startPlaylistPlaybackCommand;
            }
        }

        /// <summary>
        /// Gibt das Kommando zurück, welches den Banner- Modus startet.
        /// </summary>
        public ICommand StartFloatingPlaybackCommand
        {
            get
            {
                if (this._startFloatingPlaybackCommand == null)
                    this._startFloatingPlaybackCommand = new FloatingPlaybackCommand();
                return this._startFloatingPlaybackCommand;
            }
        }

        /// <summary>
        /// Gibt das Kommando zurück, welches alle Elemente vom Bildschirm entfernt.
        /// </summary>
        public ICommand BlankScreenCommand
        {
            get
            {
                if (this._blankScreenCommand == null)
                    this._blankScreenCommand = new BlankScreenCommand();
                return this._blankScreenCommand;
            }
        }

        /// <summary>
        /// Gibt eine Liste aller Playlists zurück.
        /// </summary>
        public IEnumerable<Playlist> Playlists
        {
            get
            {
                return Context.Instance.Playlists;
            }
        }

        /// <summary>
        /// Gibt eine Sammlung aller Nachrichten zurück.
        /// </summary>
        public ObservableCollection<string> Messages
        {
            get
            {
                return this._messages;
            }
        }
    }
}
