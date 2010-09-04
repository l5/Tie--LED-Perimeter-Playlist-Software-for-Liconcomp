using System.Linq;
using System.Windows.Input;
using Line5.Liconcomp;
using Tie.Model;

namespace Tie
{
    /// <summary>
    /// Ein Kommando, welches alle Playlisten untereinander und alle Videos aus jeder 
    /// Playlist nebeneinander anordnet.
    /// </summary>
    internal class FloatingPlaybackCommand : PlayerCommand
    {
        /// <summary>
        /// Standardkonstruktor.
        /// </summary>
        public FloatingPlaybackCommand()
            : base()
        {
        }

        #region Command Members

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
            int nMov = 0;
            int canvasWidth = Context.Instance.Player.WindowWidth;
            int canvasHeight = Context.Instance.Player.WindowHeight;

            uint lb = 1000;
            int hspacing = 0;
            int vspacing = 10;

            int offsetX = 0;
            int offsetY = 0;

            Context.Instance.DestroySync();

            foreach (Playlist pl in Context.Instance.Playlists)
            {
                offsetX = 0;
                nMov = 0;

                foreach (Movie m in pl.Movies)
                {
                    // Neues Video erstellens
                    VideoFile video = Context.Instance.Player.CreateVideoFromFile(m.Info.FullName);
                    if (video != null)
                    {
                        // Erstmal unsichtbar machen bis die Positionierung abgeschlossen ist.
                        video.Visible = false;
                        // Schonmal den 1. Frame laden
                        video.Pause();

                        // Wir wollen alle Videos gleichzeitig starten!
                        Context.Instance.Sync.Add(video);

                        // Wir wollen jedes Video an einer festen Position.
                        StaticPosition pos = new StaticPosition(0, offsetY);
                        video.Movement = pos;

                        // Positionsbestimmung:
                        video.EndOfLineOverlap = 0;
                        video.OffsetX = offsetX;
                        video.LineBreak = lb;


                        if (offsetX + video.Width > canvasWidth)
                        {
                            offsetX = (int)video.Width - (canvasWidth - offsetX) + hspacing + (canvasWidth - (int)lb);
                            offsetY += (int)video.Height * (int)(canvasWidth / video.Width);
                        }
                        else
                        {
                            offsetX += (int)video.Width + hspacing;
                        }

                        if (nMov == pl.Movies.Count() - 1)
                            offsetY += (int)video.Height;
                    }

                    nMov++;
                }
                nPl++;
                offsetY += vspacing;
            }

            // Jetzt alle Videos starten und sichtbar machen
            Context.Instance.Sync.Play();
            Context.Instance.Sync.Visible = true;

            CommandManager.InvalidateRequerySuggested();
        }

        #endregion
    }
}
