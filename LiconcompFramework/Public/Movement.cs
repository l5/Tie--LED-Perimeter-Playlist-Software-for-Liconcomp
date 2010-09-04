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
    /// Stellt eine Kapselung für die Liconcomp Schnittstelle <see cref="global::Liconcomp.IPosition"/>
    /// dar. Dies ist nur eine Basisklasse. Nutzen Sie für die Positionsangabe
    /// die Klasse <seealso cref="StaticPosition"/>.
    /// </summary>
    /// <threadsafety static="true" instance="false"/>
    public class Movement : Disposable
    {
        /// <summary>
        /// COM Objekt für das Movement.
        /// </summary>
        private global::Liconcomp.IPosition _COMObject = null;

        /// <summary>
        /// Erstellt eine neue Instanz dieser Klasse. Positionen und Movements
        /// benötigen keine Instanz von Liconcomp und können selbst ohne eine
        /// Instanz von <see cref="Player"/> erstellt werden.
        /// </summary>
        public Movement()
        {
            this._COMObject = CreateMovement();

            if (this._COMObject == null)
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Konstruktor, der kein neues Movement erstellt, sondern um ein
        /// bestehendes Movement wrappt.
        /// </summary>
        /// <param name="COMObject">Das Movement, das gekapselt werden soll.</param>
        internal Movement(global::Liconcomp.IPosition COMObject)
        {
            if (COMObject == null)
                throw new ArgumentNullException("COMObject");
            this._COMObject = COMObject;
        }

        /// <summary>
        /// Gibt das gemanagte COM Objekt zurück.
        /// </summary>
        internal global::Liconcomp.IPosition COMObject
        {
            get
            {
                return this._COMObject;
            }
        }

        /// <summary>
        /// Erbende Klassen wie <see cref="StaticPosition"/> überschreiben diese
        /// Methode um eine Instanz der von ihnen behandelten COM Klasse zu
        /// erzeugen.
        /// </summary>
        /// <returns>Eine gültige Instanz einer Klasse, die <see cref="global::Liconcomp.IPosition"/>
        /// implementiert.</returns>
        protected virtual global::Liconcomp.IPosition CreateMovement()
        {
            return null;
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
            this._COMObject = null;
            base.Dispose(Disposing);
        }

        #endregion

        #region IPosition Members

        /// <summary>
        /// Gibt die X- Koordinate in Bildpunkten (Pixel) zurück. Dieser Wert kann
        /// negativ sein.
        /// </summary>
        public int X
        {
            get
            {
                return (int)this._COMObject.GetX();
            }
        }

        /// <summary>
        /// Gibt die Y- Koordinate in Bildpunkten (Pixel) zurück. Dieser Wert kann
        /// negativ sein.
        /// </summary>
        public int Y
        {
            get
            {
                return (int)this._COMObject.GetY();
            }
        }

        /// <summary>
        /// Gibt die Z- Koordinate in Bildpunkten (Pixel) zurück. Dieser Wert kann
        /// negativ sein.
        /// </summary>
        public int Z
        {
            get
            {
                return (int)this._COMObject.GetZ();
            }
        }

        #endregion
    }
}
