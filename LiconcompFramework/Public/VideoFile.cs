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
    /// Ein Ereignis, welches ausgelöst wird wenn ein Video einmal komplett
    /// abgespielt wurde.
    /// </summary>
    /// <param name="videoFile">Die Instanz der Video Klasse, deren Video
    /// einmal komplett abgespielt wurde.</param>
    /// <param name="Args">Argumente.</param>
    public delegate void VideoFileLoopFinishedEvent(VideoFile videoFile, EventArgs Args);

    /// <summary>
    /// Eine Klasse, die Methoden und Eigenschaften zur Verfügung stellt um
    /// Videos abzuspielen.
    /// </summary>
    public sealed class VideoFile : Element
    {
        /// <summary>
        /// Wird ausgelöst, wenn das Video einmal komplett abgespielt wurde.
        /// </summary>
        public event VideoFileLoopFinishedEvent OnLoopFinished;

        /// <summary>
        /// Standardkonstruktor.
        /// </summary>
        /// <param name="Player">Eine Referenz auf das <see cref="Player"/> Objekt.</param>
        /// <param name="VideoFile">Das COM Objekt, welches gewrappt werden soll.</param>
        internal VideoFile(Player Player, global::Liconcomp.IVideoFile VideoFile)
            : base(Player, VideoFile)
        {
        }

        #region Events

        /// <summary>
        /// Löst das <see cref="OnLoopFinished"/> Ereignis aus.
        /// </summary>
        /// <param name="element">Das Element, welches das Ereignis ausgelöst hat.</param>
        private void FireOnLoopFinished(object element)
        {
            VideoFileLoopFinishedEvent Copy = OnLoopFinished;
            if (Copy != null)
                Copy(this, new EventArgs());
        }

        /// <summary>
        /// Wird von erbenden Klassen überschrieben um Ereignisbehandlungsmethoden zu
        /// registrieren.
        /// </summary>
        protected override void HookEvents()
        {
            base.HookEvents();
            global::Liconcomp.IVideoFileEvents_Event Events = this.COMObject as global::Liconcomp.IVideoFileEvents_Event;

            if (Events != null)
            {
                // Wir müssen alle Ereignisse binden!
                Events.OnRemove += this.FireOnRemove;
                Events.OnLoopFinished += this.FireOnLoopFinished;
            }
        }

        /// <summary>
        /// Wird von erbenden Klassen überschrieben um Ereignisbehandlungsmethoden zu
        /// entfernen.
        /// </summary>
        protected override void UnhookEvents()
        {
            base.HookEvents();

            global::Liconcomp.IVideoFileEvents_Event Events = this.COMObject as global::Liconcomp.IVideoFileEvents_Event;

            if (Events != null)
            {
                Events.OnRemove -= this.FireOnRemove;
                Events.OnLoopFinished -= this.FireOnLoopFinished;
            }
        }

        #endregion

        /// <summary>
        /// Eine private hilfseigenschaft um Casts in den restlichen Eigenschaften
        /// und Methoden zu vermeiden.
        /// </summary>
        private global::Liconcomp.VideoFile TypedCOMObject
        {
            get
            {
                return (global::Liconcomp.VideoFile)base.COMObject;
            }
        }

        #region IVideoFile Members

        /// <summary>
        /// Gibt das End-Of-Line-Overlap in Bildpunkten (Pixel) zurück oder setzt dieses.
        /// </summary>
        public uint EndOfLineOverlap
        {
            get
            {
                return this.TypedCOMObject.EndOfLineOverlap;
            }
            set
            {
                this.TypedCOMObject.EndOfLineOverlap = value;
            }
        }

        /// <summary>
        /// Gibt den Dateiname des Videos zurück.
        /// </summary>
        public string Filename
        {
            get
            {
                return (string)this.TypedCOMObject.Filename;
            }
        }

        /// <summary>
        /// Gibt den Zeilenumbruch des Videos in Bildpunkten (Pixel) zurück oder
        /// setzt diesen.
        /// </summary>
        public uint LineBreak
        {
            get
            {
                return (uint)this.TypedCOMObject.LineBreak;
            }
            set
            {
                this.TypedCOMObject.LineBreak = value;
            }
        }

        /// <summary>
        /// Gibt zurück, wie oft das Video abgespielt werden soll oder setzt
        /// die Anzahl der Wiederholungen. 0 bedeutet unendlich oft.
        /// </summary>
        public uint LoopCount
        {
            get
            {
                return (uint)this.TypedCOMObject.LoopCount;
            }
            set
            {
                this.TypedCOMObject.LoopCount = value;
            }
        }

        /// <summary>
        /// Gibt das X-Offset in Bildpunkten (Pixel) zurück oder setzt dieses.
        /// </summary>
        public int OffsetX
        {
            get
            {
                return (int)this.TypedCOMObject.OffsetX;
            }
            set
            {
                this.TypedCOMObject.OffsetX = value;
            }
        }

        /// <summary>
        /// Gibt das Y-Offset in Bildpunkten (Pixel) zurück oder setzt dieses.
        /// </summary>
        public int OffsetY
        {
            get
            {
                return (int)this.TypedCOMObject.OffsetY;
            }
            set
            {
                this.TypedCOMObject.OffsetY = value;
            }
        }

        /// <summary>
        /// Pausiert das Video. Diese Methode kann auch genutzt werden um den Videostart
        /// zu beschleunigen. Erstellen Sie dazu eine Instanz dieser Klasse, machen Sie sie
        /// unsichtbar (<see cref="Element.Visible"/>) und rufen Sie <see cref="Pause()"/> auf.
        /// Ein nun folgender Aufruf von <see cref="Play()"/> wird das Video sofort abspielen.
        /// </summary>
        public void Pause()
        {
            this.TypedCOMObject.Pause();
        }

        /// <summary>
        /// Spielt das Video ab. Hat keine Wirkung wenn das Video bereits läuft.
        /// </summary>
        public void Play()
        {
            this.TypedCOMObject.Play();
        }

        /// <summary>
        /// Stoppt die Wiedergabe des Videos. Hat keine Auswirkungen wenn das wieder bereits
        /// gestoppt wurde.
        /// </summary>
        public void Stop()
        {
            this.TypedCOMObject.Stop();
        }

        #endregion
    }
}
