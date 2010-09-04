using System;
using System.IO;

namespace Tie.Model
{
    /// <summary>
    /// Stellt ein abspielbares Video dar.
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Hält die Informationen zur Datei vor.
        /// </summary>
        private FileInfo _info;

        /// <summary>
        /// Standardkonstruktor.
        /// </summary>
        /// <param name="info">Informationen zur Videodatei. Darf nicht <see langword="null"/> sein.</param>
        /// <exception cref="System.ArgumentNullException">Wird ausgelöst, wenn <paramref name="info"/>
        /// <see langword="null"/> ist.</exception>
        internal Movie(FileInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            this._info = info;
        }

        /// <summary>
        /// Gibt Informationen zur VideoDatei zurück.
        /// </summary>
        public FileInfo Info
        {
            get
            {
                return this._info;
            }
        }

        /// <summary>
        /// Gibt den Titel des Videos zurück.
        /// </summary>
        public string Title
        {
            get
            {
                return this._info.Name;
            }
        }

        /// <summary>
        /// Gibt einen <see cref="System.String"/> zurück, der diese Instanz
        /// repräsentiert.
        /// </summary>
        /// <returns>
        /// Ein <see cref="System.String"/> der diese Instanz repräsentiert.
        /// </returns>
        public override string ToString()
        {
            return Title;
        }
    }
}
