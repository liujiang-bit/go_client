using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Client.Utils
{
    public class BannedWord
    {
        
        private static BoyerMooreStringMatcher[] BMStringArray;
        private static BoyerMooreStringMatcher[] ChatStringArray;


        public static bool HasInited()
        {
            return BMStringArray != null|| ChatStringArray != null;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void InitBadWord(List<string> badwords)
        {
            if (BMStringArray == null)
            {
                BMStringArray = new BoyerMooreStringMatcher[badwords.Count];

                for (int i = 0; i < badwords.Count; i++)
                {
                    var by = new BoyerMooreStringMatcher(badwords[i]);
                    BMStringArray[i] = by;
                }
            }
        }

        /// <summary>
        /// 初始化聊天
        /// </summary>
        public static void InitChatWord(List<string> badwords)
        {
            if (ChatStringArray == null)
            {
                ChatStringArray = new BoyerMooreStringMatcher[badwords.Count];

                for (int i = 0; i < badwords.Count; i++)
                {
                    var by = new BoyerMooreStringMatcher(badwords[i]);
                    ChatStringArray[i] = by;
                }
            }
        }

        /// <summary>
        /// 检查并替换脏字
        /// </summary>
        /// <param name="text"></param>
        public static void CheckBannedWord(char[] text)
        {
            if (BMStringArray != null)
            {
                for (int i = 1; i < BMStringArray.Length; i++)
                {
                    BMStringArray[i].CheckAndRePlace(text, false);
                }
            }
            
        }
        /// <summary>
        /// 检查并替换脏字
        /// </summary>
        /// <param name="text"></param>
        public static void CheckBannedWord(string text)
        {
            if (BMStringArray != null)
            {
                for (int i = 1; i < BMStringArray.Length; i++)
                {
                    BMStringArray[i].CheckAndRePlace(text, false);
                }
            }
        }
        /// <summary>
        /// 检查是否有脏字
        /// </summary>
        /// <param name="text"></param>
        public static bool ChackHasBannedWord(string text)
        {
            if (BMStringArray != null)
            {
                for (int i = 1; i < BMStringArray.Length; i++)
                {
                    if (BMStringArray[i].TryMatch(text, false))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 脏字的*替换为指定字符串
        /// </summary>
        /// <param name="text"></param>
        public static string ReplaceBannedWord(string origin, string replacement)
        {
            return Regex.Replace(origin, @"\*{2,}", replacement);
        }

        public static bool CheckChatHasBannedWord(string text)
        {
            if (ChatStringArray != null)
            {
                for (int i = 1; i < ChatStringArray.Length; i++)
                {
                    if (ChatStringArray[i].TryMatch(text, false))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
        /// <summary>
        /// 检查并替换聊天脏字
        /// </summary>
        /// <param name="text"></param>
        public static void CheckChatBannedWord(string text)
        {
            if (ChatStringArray != null)
            {
                for (int i = 1; i < ChatStringArray.Length; i++)
                {
                    ChatStringArray[i].CheckAndRePlace(text, false);
                }
            }
        }
    }
}