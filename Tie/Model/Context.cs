using System;
using System.Collections.Generic;
using System.IO;
using Line5.Liconcomp;

namespace Tie.Model
{
    /// <summary>
    /// Eine Hilfsklasse, die dem Zweck dieser Anwendung entsprechende Daten
    /// vorhält und Funktionen implementiert.
    /// </summary>
    public class Context : IDisposable
    {
        /// <summary>
        /// Es kann nur eine Instanz dieser Klasse existieren (Singleton).
        /// </summary>
        private static Context INSTANCE = null;

        /// <summary>
        /// Wurde diese Instanz freigegeben?
        /// </summary>
        bool _disposed = false;

        /// <summary>
        /// Unserer Player den wir benutzen, um Filme abzuspielen.
        /// </summary>
        /// <seealso cref="Line5.Liconcomp.Player"/>
        Player _player = null;

        /// <summary>
        /// Das Synchronisationsobjekt welches wir benutzen, um alle Filme
        /// gleichzeitig anzuzeigen bzw. auszublenden.
        /// </summary>
        /// <seealso cref="Line5.Liconcomp.Sync"/>
        Sync _sync = null;

        /// <summary>
        /// Unsere Playlists.
        /// </summary>
        private List<Playlist> _playlists = new List<Playlist>();

        /// <summary>
        /// Privat. Es sollte die <see cref="Instance"/> Eigenschaft genutzt werden
        /// um eine Instanz dieser Klasse zu erzeugen.
        /// </summary>
        private Context()
        {
            this._player = new Player();
        }

        /// <summary>
        /// Gibt den Kontext für diese Anwendung zurück.
        /// </summary>
        public static Context Instance
        {
            get
            {
                if (Context.INSTANCE == null)
                    Context.INSTANCE = new Context();
                return Context.INSTANCE;
            }
        }

        /// <summary>
        /// Hilfsmethode die eine <see cref="System.ObjectDisposedException"/> auslöst
        /// falls diese Instanz bereits freigegeben wurde.
        /// </summary>
        private void CheckDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException("Context");
        }

        /// <summary>
        /// Gibt diese Instanz frei. Der Kontext kann danach nicht mehr benutzt werden.
        /// </summary>
        /// <param name="disposing">Gibt an, ob die Methode vom Garbage Collector aufgerufen
        /// wird oder nicht.</param>
        protected virtual void Dispose(bool disposing)
        {
            this.DestroySync();
            this._playlists.Clear();
            this._playlists = null;

            if (this._player != null)
            {
                this._player.Dispose();
                this._player = null;
            }
        }

        /// <summary>
        /// Gibt diese Instanz frei. Der Kontext kann danach nicht mehr benutzt werden. Wird
        /// er trotzdem benutzt, wird eine <see cref="System.ObjectDisposedException"/> ausgelöst.
        /// </summary>
        public void Dispose()
        {
            if (this._disposed)
                return;

            this._disposed = true;
            Context.INSTANCE = null;
            GC.SuppressFinalize(this);
            Dispose(false);
        }

        #region Properties

        /// <summary>
        /// Gibt eine Auflistung aller Playlisten zurück.
        /// </summary>
        public IEnumerable<Playlist> Playlists
        {
            get
            {
                this.CheckDisposed();
                return this._playlists;
            }
        }

        /// <summary>
        /// Gibt eine Referenz auf den Liconcomp Player zurück. Siehe Framework!
        /// </summary>
        public Player Player
        {
            get
            {
                return this._player;
            }
        }

        /// <summary>
        /// Gibt eine Instanz auf das in dieser Demo Anwendung benutzte Sync Objekt
        /// zurück. Es wird dafür verwendet Videos gleichzeitig anzeigen oder ausblenden
        /// zu lassen. In Umfangreichen Anwendungen können beliebig viele Sync Instanzen
        /// erstellt werden.
        /// </summary>
        public Sync Sync
        {
            get
            {
                if (this._sync == null)
                    this._sync = new Sync(this._player);
                return this._sync;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gibt das <see cref="Sync"/> Objekt frei.
        /// </summary>
        public void DestroySync()
        {
            if (this._sync != null)
            {
                this._sync.Dispose();
                this._sync = null;
            }
        }

        /// <summary>
        /// Lädt die Playlists. In diesem Fall werden dazu alle Verzeichnisse um Verzeichnis
        /// "movies" unterhalb des Anwendungsverzeichnisses durchsucht.
        /// </summary>
        public void RefreshPlaylists()
        {
            this.CheckDisposed();
            this._playlists.Clear();

            FileInfo appPath = new FileInfo(System.Windows.Forms.Application.ExecutablePath);
            string[] playlists = Directory.GetDirectories(appPath.DirectoryName + @"\movies", "*", SearchOption.TopDirectoryOnly);
            foreach (string pl in playlists)
                this._playlists.Add(new Playlist(new DirectoryInfo(pl)));
        }

        #endregion
    }
}
