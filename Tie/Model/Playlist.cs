using System.Collections.Generic;
using System.IO;

namespace Tie.Model
{
    /// <summary>
    /// Definiert eine Klasse, die eine Sammlung von Videos enthält.
    /// </summary>
    public class Playlist
    {
        /// <summary>
        /// Informationen zum Verzeichnis, in dem sich die Videos befinden.
        /// </summary>
        private DirectoryInfo _directory = null;

        /// <summary>
        /// Eine Liste aller <see cref="Movie">Videos</see>, die in dieser
        /// Playlist enthalten sind.
        /// </summary>
        private List<Movie> _movies = new List<Movie>();

        /// <summary>
        /// Standardkonstruktor.
        /// </summary>
        /// <param name="directory">Informationen zum Playlist- Verzeichnis. Darf nicht
        /// <see langword="null"/> sein.</param>
        /// <exception cref="System.ArgumentNullException">Wird ausgelöst, wenn <paramref name="directory"/>
        /// <see langword="null"/> ist.</exception>
        public Playlist(DirectoryInfo directory)
        {
            this._directory = directory;
            string[] movies = Directory.GetFiles(directory.FullName, "*", SearchOption.TopDirectoryOnly);
            foreach (string m in movies)
                this._movies.Add(new Movie(new FileInfo(m)));
        }

        /// <summary>
        /// Gibt den Namen der Playlist zurück.
        /// </summary>
        public string Name
        {
            get
            {
                return this._directory.Name;
            }
        }

        /// <summary>
        /// Gibt den absoluten Pfad der Playlist zurück.
        /// </summary>
        public string Path
        {
            get
            {
                return this._directory.FullName;
            }
        }

        /// <summary>
        /// Gibt eine Auflistung der in dieser Playlist vorhandenen Filme
        /// zurück.
        /// </summary>
        public IEnumerable<Movie> Movies
        {
            get
            {
                return this._movies;
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
            return this._directory.Name;
        }
    }
}