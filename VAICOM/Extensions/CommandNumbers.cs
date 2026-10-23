namespace VAICOM
{

    namespace Extensions
    {

        /// <summary>
        /// Reads numeric parameters out of the spoken command.
        ///
        /// Handlers have historically read one digit per {CMDSEGMENT:n}, which assumes the
        /// speech engine returns every spoken digit as its own token. Engines apply inverse
        /// text normalisation to digit runs, so "five eight eight" can arrive as three
        /// segments ("5","8","8"), two ("58","8") or one ("588") depending on the engine,
        /// the speaker's pacing and the phrasing. When it collapses, a per-segment read
        /// either fails to match at all or silently zero-fills the missing segments.
        ///
        /// {TXTNUM} over the whole spoken command sidesteps that: it returns only the
        /// digits, in order, however they happened to be segmented. Literal sections in the
        /// phrase that carry no digits ("decimal", "point", band words such as "am" / "fm"
        /// or "x-ray" / "yankee") contribute nothing and need no special handling.
        ///
        /// This is the approach already used by the F-4E WSO handlers, in
        /// WSOCommandHandler.GetNumberFromCommand().
        /// </summary>
        public static class CommandNumbers
        {

            /// <summary>
            /// Digits of the spoken command, in order. Never null.
            /// </summary>
            public static string Digits()
            {
                string digits = State.Proxy.Utility.ParseTokens("{TXTNUM:\"{CMD}\"}");
                return digits ?? string.Empty;
            }

            /// <summary>
            /// The digits contained in an arbitrary string, in order. Never null.
            /// Used to work out what part of a command's digit run came from a
            /// particular segment.
            /// </summary>
            public static string DigitsIn(string text)
            {
                if (string.IsNullOrEmpty(text))
                {
                    return string.Empty;
                }

                System.Text.StringBuilder digits = new System.Text.StringBuilder();

                foreach (char c in text)
                {
                    if (c >= '0' && c <= '9')
                    {
                        digits.Append(c);
                    }
                }

                return digits.ToString();
            }

            /// <summary>
            /// Digit at the given position as an int, or -1 when it is not present.
            /// </summary>
            public static int At(string digits, int index)
            {
                if (string.IsNullOrEmpty(digits) || index < 0 || index >= digits.Length)
                {
                    return -1;
                }

                return digits[index] - '0';
            }

        }
    }
}
