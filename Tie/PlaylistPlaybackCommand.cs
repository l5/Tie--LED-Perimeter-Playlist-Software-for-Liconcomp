using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Line5.Liconcomp;
using Tie.Model;

namespace Tie
{
    /// <summary>
    /// Kommando, welches alle Playlists untereinander anordnet. Die Videos für jede
    /// Playlist werden nacheinander abgespielt.
    /// </summary>
    internal class PlaylistPlaybackCommand : PlayerCommand
    {
        /// <summary>
        /// Hilfsklasse für die Verkettung von Videos.
        /// </summary>
        private class Chain
        {
            /// <summary>
            /// Das Video, für welches diese Hilfsklasse existiert.
            /// </summary>
            public VideoFile For = null;

            /// <summary>
            /// Das Video, welches als nächstes abgespielt werden soll.
            /// </summary>
            public Chain Next = null;

            /// <summary>
            /// Standardkonstruktor.
            /// </summary>
            public Chain()
            {
                this.Next = this;
            }

            /// <summary>
            /// Standardkonstruktor.
            /// </summary>
            /// <param name="f"></param>
            public Chain(VideoFile f)
                : this()
            {
                this.For = f;
            }
        };

        /// <summary>
        /// Beinhaltet jeweils das erste Element jeder Playlist.
        /// </summary>
        private List<Chain> _chains = new List<Chain>();

        /// <summary>
        /// Standardkonstruktor.
        /// </summary>
        public PlaylistPlaybackCommand()
            : base()
        {
        }

        #region Command Members

        /// <summary>
        /// Wird ausgeführt, wenn ein Video einmal abgespielt wurde.
        /// </summary>
        /// <param name="vf"></param>
        /// <param name="Args"></param>
        private void OnVideoElementLoopFinished(VideoFile vf, EventArgs Args)
        {
            Chain Next = null;
            if (vf != null)
            {
                // Suchen, auf welches Element der Playlist das Ereignis zutrifft.
                foreach (Chain C in this._chains)
                    if (((string)C.For.Filename).CompareTo((string)vf.Filename) == 0)
                    {
                        // Gefunden, das nächste Element muss abgespielt werden.
                        Next = C.Next;
                        break;
                    }

                if (Next == null)
                    return;

                if (((string)Next.For.Filename).CompareTo((string)vf.Filename) == 0)
                {
                    // Playlist hat nur 1 Element, stoppen und gleich wieder starten.
                    vf.Stop();
                    vf.Play();
                }
                else
                {
                    // Altes Video stoppen, unsichtbar machen, neues Video starten
                    // und sichtbar machen. Der LoopCount muss auf 2 gesetzt werden!
                    vf.Visible = false;
                    vf.Stop();
                    Next.For.LoopCount = 2;
                    Next.For.Play();
                    Next.For.Visible = true;
                }
            }
        }

        /// <summary>
        /// Wird ausgeführt, wenn ein Video entfernt wird. Wir müssen unsere Event handler
        /// deinstallieren um Memory Leaks zu vermeiden.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="Args"></param>
        private void OnVideoElementRemove(Element element, EventArgs Args)
        {
            VideoFile vf = element as VideoFile;
            if (vf != null)
            {
                vf.OnLoopFinished -= OnVideoElementLoopFinished;
                vf.OnRemove -= OnVideoElementRemove;
            }
        }

        /// <summary>
        /// Stellt fest, ob dieses Kommando ausgeführt werden kann oder nicht.
        /// </summary>
        /// <param name="parameter">Der Parameter für das Kommando. Kann <see langword="null"/> sein.</param>
        /// <returns>
        /// 	<see langword="true"/> wenn das Kommando ausführbar ist, sonst <see langword="false"/>.
        /// </returns>
        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter) && (Context.Instance.Player.Elements.Count() == 0);
        }

        /// <summary>
        /// Führt dieses Kommando aus.
        /// </summary>
        /// <param name="parameter">Der Parameter für das Kommando. Kann <see langword="null"/> sein.</param>
        public override void Execute(object parameter)
        {
            // Instantiate all movies.
            int nPl = 0;
            int offsetY = 0;
            int vspacing = 20;
            bool isFirst = true;
            int maxHeight = 0;

            List<VideoFile> firstVideos = new List<VideoFile>();

            Context.Instance.DestroySync();

            this._chains.Clear();

            foreach (Playlist pl in Context.Instance.Playlists)
            {
                StaticPosition pos = new StaticPosition(0, offsetY);

                isFirst = true;
                maxHeight = 0;

                Chain C = new Chain();
                Chain Start = C;
                this._chains.Add(C);

                foreach (Movie m in pl.Movies)
                {
                    VideoFile video = Context.Instance.Player.CreateVideoFromFile(m.Info.FullName);
                    if (video != null)
                    {
                        if (C.For == null)
                            C.For = video;
                        else
                        {
                            Chain C_copy = new Chain(video);
                            C.Next = C_copy;
                            C = C_copy;
                            C_copy.Next = Start;
                            this._chains.Add(C_copy);
                        }

                        video.Visible = false;
                        video.Pause();
                        Context.Instance.Sync.Add(video);
                        video.LoopCount = 1;
                        video.Movement = pos;

                        video.OnRemove += OnVideoElementRemove;
                        video.OnLoopFinished += OnVideoElementLoopFinished;

                        if (video.Height > maxHeight)
                            maxHeight = (int)video.Height;

                        if (isFirst)
                        {
                            isFirst = false;
                            firstVideos.Add(video);
                        }
                    }
                }
                nPl++;
                offsetY += maxHeight + vspacing;
            }

            Context.Instance.Sync.Play();
            Context.Instance.Sync.Visible = true;

            CommandManager.InvalidateRequerySuggested();
        }

        #endregion
    }
}
