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
    /// Beschreibt eine Klasse, die eine feste Position ausdrückt. Nutzen
    /// Sie diese Klasse um Objekte absolut auf dem Bildschirm zu positionieren.
    /// Diese Klasse unterstützte keine Animationen oder Bewegungen. Es ist
    /// jedoch möglich, die X, Y und Z Koordinate nach Instanziierung der
    /// Klasse zu verändern.
    /// </summary>
    /// <threadsafety static="true" instance="false"/>
    public class StaticPosition : Movement
    {
        /// <summary>
        /// Standardkonstruktor. Die Movement wird mit (0; 0; 0) initialisiert.
        /// </summary>
        public StaticPosition()
            : this(0, 0, 0)
        {
        }

        /// <summary>
        /// Die Movement wird mit den angegebenen Werten und 0 für Z initialisiert.
        /// </summary>
        /// <param name="x">Die X- Koordinate in Bildpunkten (Pixel).</param>
        /// <param name="y">Die Y- Koordinate in Bildpunkten (Pixel).</param>
        public StaticPosition(int x, int y)
            : this(x, y, 0)
        {
        }

        /// <summary>
        /// Die Movement wird mit den angegebenen Koordinaten initialisiert.
        /// </summary>
        /// <param name="x">Die X- Koordinate in Bildpunkten (Pixel).</param>
        /// <param name="y">Die Y- Koordinate in Bildpunkten (Pixel).</param>
        /// <param name="z">Die Z- Koordinate in Bildpunkten (Pixel).</param>
        public StaticPosition(int x, int y, int z)
            : base()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        #region Movement

        /// <summary>
        /// Erbende Klassen wie <see cref="StaticPosition"/> überschreiben diese
        /// Methode um eine Instanz der von ihnen behandelten COM Klasse zu
        /// erzeugen.
        /// </summary>
        /// <returns>
        /// Eine gültige Instanz einer Klasse, die <see cref="global::Liconcomp.IPosition"/>
        /// implementiert.
        /// </returns>
        protected override global::Liconcomp.IPosition CreateMovement()
        {
            return (global::Liconcomp.IStaticPosition)Activator.CreateInstance(typeof(global::Liconcomp.StaticPositionClass));
        }

        #endregion

        #region IStaticPosition Members

        /// <summary>
        /// Gibt die X- Koordinate in Bildpunkten (Pixel) zurück oder setzt diese.
        /// Dieser Wert kann negativ sein.
        /// </summary>
        new public int X
        {
            get
            {
                return base.X;
            }
            set
            {
                ((global::Liconcomp.IStaticPosition)this.COMObject).SetX(value);
            }
        }

        /// <summary>
        /// Gibt die Y- Koordinate in Bildpunkten (Pixel) zurück oder setzt diese.
        /// Dieser Wert kann negativ sein.
        /// </summary>
        new public int Y
        {
            get
            {
                return base.Y;
            }
            set
            {
                ((global::Liconcomp.IStaticPosition)this.COMObject).SetY(value);
            }
        }

        /// <summary>
        /// Gibt die Z- Koordinate in Bildpunkten (Pixel) zurück oder setzt diese.
        /// Dieser Wert kann negativ sein.
        /// </summary>
        new public int Z
        {
            get
            {
                return base.Z;
            }
            set
            {
                ((global::Liconcomp.IStaticPosition)this.COMObject).SetZ(value);
            }
        }

        #endregion
    }
}
