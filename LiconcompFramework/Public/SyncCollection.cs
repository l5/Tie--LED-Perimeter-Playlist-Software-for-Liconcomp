/**********************************************************************
 *** Line5 Liconcomp .Net Framework Library                         ***
 ***                                                                ***
 *** Diese Bibliothek kapselt den Zugriff auf Liconcomp COM Dienste ***
 ***                                                                ***
 ***                                                                ***
 *** (c) Line5 e.K., 2010                                           ***
 *** Author: Enrico Neidt <delphi32 at gmx dot de>                  ***
 **********************************************************************/

using System.Collections;
using System.Collections.Generic;
using System;

namespace Line5.Liconcomp
{
    /// <summary>
    /// Bietet eine Sammlung von <see cref="Sync"/> Objekten zur besseren
    /// Verwaltung an. Diese Klasse wird in der aktuellen Version des
    /// Frameworks noch nicht benutzt!
    /// </summary>
    public class SyncCollection : IEnumerable<Sync>, IEnumerable
    {
        /// <summary>
        /// Referenz auf den Player, der die Liste verwaltet.
        /// </summary>
        private Player _player = null;

        /// <summary>
        /// Liste, die alle Instanzen von <see cref="Sync"/> verwaltet.
        /// </summary>
        private IList<Sync> _list = new List<Sync>();

        /// <summary>
        /// Standardkonstruktor.
        /// </summary>
        /// <param name="Player">Eine Referenz auf den Player. Kann nicht <see langword="null"/> sein.</param>
        /// <exception cref="System.ArgumentNullException">Wird ausgelöst, wenn <paramref name="Player"/>
        /// <see langword="null"/> ist.</exception>
        internal SyncCollection(Player Player)
        {
            if (Player == null)
                throw new ArgumentNullException("Player");
            this._player = Player;
        }

        #region IEnumerable<Sync> Members

        /// <summary>
        /// Liefert einen typisierten Enumerator für die Sammlung zurück.
        /// </summary>
        /// <returns>Einen typisierten Enumerator für die Sammlung der <see cref="Sync"/> Instanzen.</returns>
        public IEnumerator<Sync> GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Liefert einen untypisierten Enumerator für die Sammlung zurück.
        /// </summary>
        /// <returns>Einen untypisierten Enumerator für die Sammlung der <see cref="Sync"/> Instanzen.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        #endregion

    }
}
