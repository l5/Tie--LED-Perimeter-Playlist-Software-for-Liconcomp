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

namespace Line5.Liconcomp
{
    /// <summary>
    /// Signalisiert, dass soeben ein Element (Video, Text, ...) vom Liconcomp
    /// Elemente Pool entfernt wurde. Das Element ist danach nicht mehr benutzbar.
    /// </summary>
    /// <param name="Sender">Das Element, welches entfernt wurde.</param>
    /// <param name="Args">Argumente.</param>
    public delegate void ElementRemoveEvent(Element Sender, EventArgs Args);

    /// <summary>
    /// Diese Klasse kapselt ein sichtbares Element im Liconcomp Fenster.
    /// Es kann keine Instanz dieser Klasse erstellt werden. Sie wird von
    /// <see cref="VideoFile"/> genutzt um gemeinsame Eigenschaften und
    /// Methoden bereitzustellen.
    /// </summary>
    /// <threadsafety static="true" instance="false"/>
    public abstract class Element : Disposable
    {
        /// <summary>
        /// Eine Referenz auf den Player, der dieses Element erstellt hat.
        /// </summary>
        private Player _player = null;

        /// <summary>
        /// Das gekapselte COM Objekt. Alle in Liconcomp sichtbaren Objekte
        /// erben von <see cref="global::Liconcomp.IElement"/>.
        /// </summary>
        private global::Liconcomp.IElement _object = null;

        /// <summary>
        /// In Liconcomp hat jedes Element eine Movement. Positionen können
        /// auch animiert werden. Da wir nicht nur die COM Movement brauchen
        /// sondern auch die gewrappte Movement müssen wir hier die .Net 
        /// Wrapper Klasse zwischenspeichern.
        /// </summary>
        private Movement _position = null;

        /// <summary>
        /// Ein Ereignis welches ausgelöst wird, wenn das Objekt mit einem
        /// Aufruf von <see cref="Remove()"/> vom Bildschirm entfernt wird.
        /// Einmal entfernte Objekte können nicht wieder angezeigt werden!
        /// </summary>
        public event ElementRemoveEvent OnRemove;

        /// <summary>
        /// Konstruktor, der eine Referenz des Players und das Element übernimmt,
        /// welches gewrappt werden soll.
        /// </summary>
        /// <param name="Player">Eine Referenz auf den <seealso cref="Player"/>.
        /// Darf nicht <see langword="null"/> sein.</param>
        /// <param name="Element">Das COM Objekt, welches von dieser Instanz
        /// kontrolliert werden soll. Muss <see cref="global::Liconcomp.IElement"/>
        /// implementieren und darf nicht <see langword="null"/> sein.</param>
        internal Element(Player Player, global::Liconcomp.IElement Element)
            : base()
        {
            if (Player == null)
                throw new ArgumentNullException("Player");
            if (Element == null)
                throw new ArgumentNullException("Element");
            this._player = Player;
            this._object = Element;

            // Wenn das Objekt schon ein Movement hat, dann übernehmen wir es.
            if (this._object.Movement != null)
                this._position = new Movement((global::Liconcomp.IPosition)this.COMObject.Movement);

            HookEvents();
        }

        #region Disposable Members

        /// <summary>
        /// Interne Methode um alle Instanzvariablen freizugeben. Diese Methode sollte von
        /// erbenden Klassen überschrieben werden.
        /// </summary>
        /// <param name="Disposing"><see langword="true"/>, wenn der Aufruf vom Garbage
        /// Collector stammt, sonst <see langword="false"/>.</param>
        protected override void Dispose(bool Disposing)
        {
            // Wir müssen allen erbenden Klassen mitteilen, dass sie ihre Ereignis-
            // handler entfernen sollen. Ansonsten wird Liconcomp die Objekte niemals
            // freigeben.
            UnhookEvents();

            this._object = null;
            this._player.RemoveElement(this);
            this._player = null;

            base.Dispose(Disposing);
        }

        #endregion

        /// <summary>
        /// Löst das <see cref="OnRemove"/> Ereignis aus. Diese Methode wird von Liconcomp
        /// aufgerufen.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Die Dispatchinterfaces wie sie Liconcomp definiert werden nicht vererbt. Dadurch
        /// gibt es keine Schnittstelle "global::Liconcomp.IElementEvents_Event", welche das
        /// "OnRemove" Ereignis definiert. Statt dessen definiert jede Eregnisschnittstelle
        /// alle Ereignisse ihrer übergeordneten Klassen. Deshalb wird diese Methode nur in
        /// erbenden Klassen benutzt (<see cref="VideoFile.HookEvents()"/>).
        /// </para>
        /// </remarks>
        /// <param name="element">Das Element, welches entfernt wurde.</param>
        protected void FireOnRemove(object element)
        {
            ElementRemoveEvent Copy = OnRemove;
            if (Copy != null)
                Copy(this, new EventArgs());
        }

        /// <summary>
        /// Wird von erbenden Klassen überschrieben um Ereignisbehandlungsmethoden zu
        /// registrieren.
        /// </summary>
        protected virtual void HookEvents()
        {
        }

        /// <summary>
        /// Wird von erbenden Klassen überschrieben um Ereignisbehandlungsmethoden zu
        /// entfernen.
        /// </summary>
        protected virtual void UnhookEvents()
        {
        }

        /// <summary>
        /// Liefert das gewrappte COM Objekt zurück.
        /// </summary>
        internal protected global::Liconcomp.IElement COMObject
        {
            get
            {
                return this._object;
            }
        }

        #region IElement Members

        /// <summary>
        /// Liefert die Höhe des Elements in Bildpunkten (Pixel) zurück.
        /// </summary>
        public uint Height
        {
            get
            {
                return this.COMObject.Height;
            }
        }

        /// <summary>
        /// Gibt das dem Element zugewiesene <see cref="Movement"/> zurück oder setzt
        /// es. Das Movement gibt an, wo auf dem Bildschirm sich das Element befindet.
        /// </summary>
        public Movement Movement
        {
            get
            {
                return this._position;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                this._position = value;
                this.COMObject.Movement = value.COMObject;
            }
        }

        /// <summary>
        /// Gibt an, ob das Element sichtbar oder unsichtbar ist.
        /// </summary>
        public bool Visible
        {
            get
            {
                return this.COMObject.Visible;
            }
            set
            {
                this.COMObject.Visible = value;
            }
        }

        /// <summary>
        /// Liefert die Breite des Elements in Bildpunkten (Pixel) zurück.
        /// </summary>
        public uint Width
        {
            get
            {
                return this.COMObject.Width;
            }
        }

        /// <summary>
        /// Liefert den zIndex des Elements zurück oder setzt diesen. Der zIndex
        /// bestimmt, in welcher Reihenfolge 2 dimensionale Elemente gezeichnet werden.
        /// </summary>
        public uint zIndex
        {
            get
            {
                return (uint)this.COMObject.zIndex;
            }
            set
            {
                this.COMObject.zIndex = value;
            }
        }

        /// <summary>
        /// Entfernt das Element vom Bildschirm und gibt es frei. Einmal entfernte
        /// Elemente können nicht wieder angezeigt werden. Diese Methode löst das Ereignis
        /// <see cref="OnRemove"/> aus und ruft <see cref="Disposable.Dispose()"/> auf.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Um Element nur kurz vom Bildschirm zu entfernen setzen Sie deren
        /// <see cref="Visible"/> Eigenschaft auf <see langword="false"/>.
        /// </para></remarks>
        public void Remove()
        {
            this._object.Remove();
            Dispose();
        }

        #endregion
    }
}


