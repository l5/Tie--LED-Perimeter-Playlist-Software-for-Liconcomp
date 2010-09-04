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
using System.Collections.Generic;
using System.Drawing;

namespace Line5.Liconcomp
{
    /// <summary>
    /// Kapselt eine Instanz von Liconcomp.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Diese Klasse bildet den Haupteinstiegspunkt für das Liconcomp .Net Framework.
    /// Erstellen Sie eine Instanz dieser Klasse um Liconcomp zu starten und anzuzeigen.
    /// </para>
    /// <para>
    /// Nutzen Sie die Methode <see cref="CreateVideoFromFile(string)"/> um neue Videos
    /// zu laden. Starten Sie die Videos mit <see cref="VideoFile.Play()"/>.
    /// </para>
    /// <para>
    /// Beachten Sie, dass immer nur eine Instanz dieser Klasse aktiv sein kann. Im DEBUG
    /// Modus wird dies überprüft. Das Erstellen einer zweiten Instanz löst dann eine
    /// <see cref="System.InvalidOperationException"/> aus.
    /// </para>
    /// </remarks>
    /// <threadsafety static="true" instance="false"/>
    public sealed class Player : Disposable
    {
#if DEBUG
        private static Player INSTANCE = null;
#endif

        /// <summary>
        /// Das COM Objekt des Players.
        /// </summary>
        global::Liconcomp.Player _player = null;

        /// <summary>
        /// Eine Liste aller Elemente, die diese Instanz erstellt hat. Diese Liste
        /// soll zu einer späteren Version von Liconcomp bereitgestellt werden.
        /// </summary>
        private List<Element> _elements = new List<Element>();

        /// <summary>
        /// Erstellt eine neue Instanz des Players. Beachten Sie immer nur eine
        /// Instanz dieser Klasse zur gleichen Zeit zu erstellen. Im DEBUG Modus
        /// wird eine <see cref="System.InvalidOperationException"/> ausgelöst,
        /// sollten Sie versuchen eine zweite Instanz zu erzeugen, bevor die erste
        /// freigegeben wurde.
        /// </summary>
        public Player()
            : base()
        {
#if DEBUG
            if (Player.INSTANCE != null)
                throw new InvalidOperationException("You should not instantiate the Liconcomp Class more than once!");
            Player.INSTANCE = this;
#endif

            StartPlayer();
        }

        /// <summary>
        /// Wird von <see cref="Element"/> während seiner Freigabe aufgerufen. Wir müssen
        /// das Element aus unserer Liste entfernen.
        /// </summary>
        /// <param name="element">Das zu entfernende Element.</param>
        /// <exception cref="System.ArgumentNullException">Wird ausgelöst, wenn <paramref name="element"/>
        /// <see langword="null"/> ist.</exception>
        internal void RemoveElement(Element element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            this._elements.Remove(element);
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

            while (this._elements.Count > 0)
                this._elements[0].Dispose();

            StopPlayer();

            this._elements.Clear();
            this._elements = null;

#if DEBUG
            Player.INSTANCE = null;
#endif
        }

        /// <summary>
        /// Startet Liconcomp und setzt die Variable <see cref="_player"/> auf die Instanz
        /// von Liconcomp.
        /// </summary>
        private void StartPlayer()
        {
            this._player = (global::Liconcomp.Player)Activator.CreateInstance(typeof(global::Liconcomp.PlayerClass));
        }

        /// <summary>
        /// Schließt die Verbindung zu Liconcomp. Der Player wird dabei nicht beendet.
        /// </summary>
        private void StopPlayer()
        {
            this._player = null;
        }

        #region Properties

        /// <summary>
        /// Gibt einen typisierten Enumerator aller momentan verfügbaren Elemente zurück.
        /// Dieser Enumerator beinhaltet auch unsichtbare Elemente.
        /// </summary>
        public IEnumerable<Element> Elements
        {
            get
            {
                this.CheckDisposed();
                return this._elements;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Entfernt alle sichtbaren und unsichtbaren Elemente vom Liconcomp Bildschirm.
        /// </summary>
        public void RemoveAllElements()
        {
            this.CheckDisposed();
            while (this._elements.Count > 0)
                this._elements[0].Dispose();
            this._elements.Clear();
        }

        #endregion

        #region IPlayer Members

        /// <summary>
        /// Erstellt ein Video aus einer Datei. Die Datei sollte vorhanden sein und ein
        /// gültiges Video enthalten. Anderenfalls ist das Verhalten dieser Methode
        /// undefiniert.
        /// </summary>
        /// <param name="Filename">Vollständiger Pfad zur Video Datei.</param>
        /// <returns>Eine Instanz der Klasse <see cref="VideoFile"/>, welche das Video
        /// beinhaltet.</returns>
        /// <exception cref="Line5.Liconcomp.LiconcompException">Wird ausgelöst, wenn ein
        /// Fehler auftritt.</exception>
        public VideoFile CreateVideoFromFile(string Filename)
        {
            this.CheckDisposed();

            global::Liconcomp.IVideoFile vf_intf = (global::Liconcomp.IVideoFile)this._player.CreateVideoFromFile(Filename);
            if (vf_intf == null)
                throw new LiconcompException("Could not create video from file '" + Filename + "'.");

            VideoFile vf = new VideoFile(this, vf_intf);
            this._elements.Add(vf);

            return vf;
        }

        /// <summary>
        /// Setzt die Hintergrundfarbe für das Liconcomp Fenster.
        /// </summary>
        /// <param name="C">Die neue Hintergrundfarbe.</param>
        public void SetBackgroundColor(Color C)
        {
            this.CheckDisposed();
            this._player.SetBackgroundColor(C.R, C.G, C.B);
        }

        /// <summary>
        /// Setzt die Hintergrundfarbe für das Liconcomp Fenster.
        /// </summary>
        /// <param name="r">Der Rot Anteil der neuen Farbe.</param>
        /// <param name="g">Der Grün Anteil der neuen Farbe.</param>
        /// <param name="b">Der Blau Anteil der neuen Farbe.</param>
        public void SetBackgroundColor(byte r, byte g, byte b)
        {
            this.CheckDisposed();
            this._player.SetBackgroundColor(r, g, b);
        }

        /// <summary>
        /// Gibt die Fensterhöhe des Liconcomp Fensters in Bildpunkten (Pixel) zurück.
        /// </summary>
        public int WindowHeight
        {
            get
            {
                this.CheckDisposed();
                return this._player.WindowHeight;
            }
        }

        /// <summary>
        /// Gibt die Fensterbreite des Liconcomp Fensters in Bildpunkten (Pixel) zurück.
        /// </summary>
        public int WindowWidth
        {
            get
            {
                this.CheckDisposed();
                return this._player.WindowWidth;
            }
        }

        #endregion
    }
}
