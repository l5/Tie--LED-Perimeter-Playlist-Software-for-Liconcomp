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
    /// Eine Hilfsklasse um die Schnittstelle <seealso cref="IDisposable"/> zu
    /// implementieren.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Diese Klasse wird von vielen Komponenten des Frameworks verwendet, um
    /// Ressourcen wieder freizugeben. Sie implementiert das <see cref="OnDisposing"/>
    /// Ereignis und bietet geschützte Methoden für Unterklassen, um Fehler
    /// zu werfen falls auf Funktionalität uzugegriffen wird, obwohl die Instanz
    /// bereits freigegeben wurde.
    /// </para>
    /// </remarks>
    /// <threadsafety static="true" instance="false" />
    public abstract class Disposable : IDisposable
    {
        /// <summary>
        /// Gibt an, ob diese Instanz bereits freigegeben wurde. Wird von
        /// <see cref="Dispose()"/> auf <see langword="true"/> gesetzt und
        /// von <see cref="CheckDisposed()"/> überprüft.
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// Wird ausgelöst, kurz bevor die Instanz der Klasse freigegeben wird.
        /// </summary>
        public event EventHandler OnDisposing;

        /// <summary>
        /// Überschrieben, um die Freigabe durchzuführen. Wird nur dann aufgerufen,
        /// wenn die Instanz vom Garbage Collector freigegeben wird!
        /// </summary>
        ~Disposable()
        {
            this.Dispose(true);
        }

        #region IDisposable Members

        /// <summary>
        /// Gibt die Instanz dieser Klasse frei. Nach dem Aufruf von <see cref="Dispose()"/>
        /// kann die Instanz nicht mehr benutzt werden. Sollten weiterhin Methoden aufgerufen
        /// oder Eigenschaften abgefragt werden wird eine <seealso cref="ObjectDisposedException"/>
        /// ausgelöst. Der mehrmalige Aufruf von <see cref="Dispose()"/> ist jedoch gestattet.
        /// </summary>
        public void Dispose()
        {
            if (this._disposed)
                return;

            GC.SuppressFinalize(this);
            this.Dispose(false);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Löst das <see cref="OnDisposing"/> Ereignis aus.
        /// </summary>
        private void FireDisposingEvent()
        {
            EventHandler Copy = OnDisposing;
            if (Copy != null)
                Copy(this, new EventArgs());
        }

        /// <summary>
        /// Interne Methode um alle Instanzvariablen freizugeben. Diese Methode sollte von
        /// erbenden Klassen überschrieben werden.
        /// </summary>
        /// <param name="Disposing"><see langword="true"/>, wenn der Aufruf vom Garbage
        /// Collector stammt, sonst <see langword="false"/>.</param>
        protected virtual void Dispose(bool Disposing)
        {
            FireDisposingEvent();
            this._disposed = true;
        }

        /// <summary>
        /// Eine geschützte Methode um sicherzustellen, dass die Instanz der Klasse noch
        /// nicht freigegeben wurde. Wirde sie freigegeben wird diese Methode eine
        /// <seealso cref="ObjectDisposedException"/> auslösen.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">Wird ausgelöst, wenn die Instanz
        /// bereits durch den Aufruf von <see cref="Dispose()"/> freigegeben wurde.</exception>
        protected void CheckDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        #endregion

        #region Publics

        /// <summary>
        /// Gibt an, ob diese Instanz bereits durch den Aufruf von <see cref="Dispose()"/>
        /// freigegeben wurde oder nicht.
        /// </summary>
        public bool Disposed
        {
            get
            {
                return this._disposed;
            }
        }

        #endregion
    }
}
