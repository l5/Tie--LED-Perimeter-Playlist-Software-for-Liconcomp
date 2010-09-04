/**********************************************************************
 *** Line5 Liconcomp .Net Framework Library                         ***
 ***                                                                ***
 *** Diese Bibliothek kapselt den Zugriff auf Liconcomp COM Dienste ***
 ***                                                                ***
 ***                                                                ***
 *** (c) Line5 e.K., 2010                                           ***
 *** Author: Enrico Neidt <delphi32 at gmx dot de>                  ***
 **********************************************************************/

using System;
using Liconcomp;

namespace Line5.Liconcomp
{
    /// <summary>
    /// Bietet Synchronisationsdienste für das gleichzeitige Ausführen
    /// verschiedener Aktionen.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Nutzen Sie <see cref="Sync"/> um mehrere Videos gleichzeitig starten
    /// zu lassen. Erstellen Sie die Videos, erstellen Sie eine Instanz von
    /// <see cref="Sync"/>, fügen Sie alle Videos mit der <see cref="Add"/>
    /// Methode zu <see cref="Sync"/> hinzu und rufen Sie anschließend
    /// <see cref="Sync.Play()"/> auf. Die Videos werden gleichzeitig starten.
    /// </para>
    /// </remarks>
    /// <threadsafety static="true" instance="false"/>
    public sealed class Sync : Disposable
    {
        /// <summary>
        /// Instanz des Players.
        /// </summary>
        private Player _player = null;

        /// <summary>
        /// Das gewrappte COM Objekt.
        /// </summary>
        private global::Liconcomp.Sync _sync = null;

        /// <summary>
        /// Erstellt eine neue Instanz der Klasse.
        /// </summary>
        /// <param name="Player">Der Player, dem die Klasse zugeordnet werden soll. Kann
        /// nicht <see langword="null"/> sein.</param>
        /// <exception cref="System.ArgumentNullException">Wird ausgelöst, wenn <paramref name="Player"/>
        /// <see langword="null"/> ist.</exception>
        public Sync(Player Player)
            : base()
        {
            if (Player == null)
                throw new ArgumentNullException("Player");

            this._player = Player;
            this._sync = (global::Liconcomp.Sync)Activator.CreateInstance(typeof(global::Liconcomp.SyncClass));
        }

        /// <summary>
        /// Interne Methode um alle Instanzvariablen freizugeben. Diese Methode sollte von
        /// erbenden Klassen überschrieben werden.
        /// </summary>
        /// <param name="Disposing"><see langword="true"/>, wenn der Aufruf vom Garbage
        /// Collector stammt, sonst <see langword="false"/>.</param>
        protected override void Dispose(bool Disposing)
        {
            base.Dispose(Disposing);

            this._sync = null;
            this._player = null;
        }

        /// <summary>
        /// Fügt ein Element der Synchronisationsliste hinzu. Alle auf <see cref="Sync"/>
        /// ausgeführten Aktionen werden auch auf das neue Element ausgeführt.
        /// </summary>
        /// <param name="Element">Das Element, dass der Synchronisationsliste hinzugefügt
        /// werden soll. Darf nicht <see langword="null"/> sein.</param>
        /// <exception cref="System.ArgumentNullException">Wird ausgelöst, wenn <paramref name="Element"/>
        /// <see langword="null"/> ist.</exception>
        public void Add(Element Element)
        {
            if (Element == null)
                throw new ArgumentNullException("Element");

            this.CheckDisposed();

#if DEBUG
            System.Diagnostics.Debug.Assert(this._sync != null);
#endif

            this._sync.Add((global::Liconcomp.Element)Element.COMObject);
        }

        /// <summary>
        /// Pausiert alle Videos in der Synchronisationsliste. Dieser Vorgang hat nur
        /// auf Videos Einfluss. Lauftexte werden nicht gestoppt.
        /// </summary>
        public void Pause()
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(this._sync != null);
#endif
            this.CheckDisposed();
            this._sync.Pause();
        }

        /// <summary>
        /// Startet alle Videos in der Synchronisationsliste. Dieser Vorgang hat nur
        /// auf Videos Einfluss. Lauftexte werden nicht gestoppt.
        /// </summary>
        public void Play()
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(this._sync != null);
#endif
            this.CheckDisposed();
            this._sync.Play();
        }

        /// <summary>
        /// Stoppt alle Videos in der Synchronisationsliste. Dieser Vorgang hat nur
        /// auf Videos Einfluss. Lauftexte werden nicht gestoppt.
        /// </summary>
        public void Stop()
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(this._sync != null);
#endif
            this.CheckDisposed();
            this._sync.Stop();
        }

        /// <summary>
        /// Setzt die <see cref="Element.Visible"/> Eigenschaft aller Elemente
        /// in der Synchronisationsliste. Somit können alle Elemente gleichzeitig ein-
        /// oder ausgeblendet werden.
        /// </summary>
        public bool Visible
        {
            set
            {
#if DEBUG
                System.Diagnostics.Debug.Assert(this._sync != null);
#endif
                this.CheckDisposed();
                this._sync.Visible = value;
            }
        }
    }
}
