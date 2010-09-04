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
    /// Genereller Liconcomp Fehler.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Diese Fehlermeldung wird vom Framework verwendet, um Fehlermeldungen
    /// von Liconcomp zu kapseln.
    /// </para>
    /// </remarks>
    /// <threadsafety static="true" instance="true" />
    public sealed class LiconcompException : ApplicationException
    {
        /// <summary>
        /// Standardkonstruktor.
        /// </summary>
        /// <param name="Message">Die Fehlermeldung, die dem Benutzer angezeigt werden soll.</param>
        public LiconcompException(string Message)
            : base(Message)
        {
        }
    }
}
