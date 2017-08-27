using System;
using System.Linq;
using System.Text;
using Magic.Royale.Files;
using Magic.Royale.Files.CSV_Logic;
using Magic.Royale.Logic;
using Magic.Royale.Logic.Enums;

namespace Magic.Royale.Extensions
{
    internal static class GameUtils
    {
        internal const int SEARCH_TAG_LENGTH = 14;

        internal static readonly char[] SEARCH_TAG_CHARS = "0289PYLQGRJCUV".ToCharArray();

        internal static string GetHashtag(long Identifier)
        {
            if (GetHighID(Identifier) <= 255)
            {
                var Stringer = new StringBuilder();
                var Count = 11;
                Identifier = ((long) GetLowID(Identifier) << 8) + GetHighID(Identifier);
                while (++Count > 0)
                {
                    Stringer.Append(SEARCH_TAG_CHARS[(int) (Identifier % SEARCH_TAG_LENGTH)]);
                    Identifier /= SEARCH_TAG_LENGTH;
                    if (Identifier <= 0)
                        break;
                }

                return new string(Stringer.Append("#").ToString().Reverse().ToArray());
            }

            return string.Empty;
        }

        internal static long GetUserID(string HashTag)
        {
            const string Search_Tag = "0289PYLQGRJCUV";
            long identifier = 0;
            foreach (var character in HashTag.Replace("#", "").ToUpper().ToCharArray())
            {
                var Index = Search_Tag.IndexOf(character);
                if (Index == -1)
                    return -1;

                identifier *= SEARCH_TAG_LENGTH;
                identifier += Index;
            }
            return (identifier % 256) | ((identifier - identifier % 256) >> 8);
        }

        internal static int GetLowID(long Identifier)
        {
            return (int) (Identifier & 0xFFFFFFFF);
        }

        internal static int GetHighID(long Identifier)
        {
            return (int) (Identifier >> 32);
        }


       
       
    }
}