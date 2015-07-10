using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Forum.Domain.Helpers
{
    public class StringHelper
    {
        /// <summary>
        /// Converts a string to it's proper case
        /// </summary>
        public static string ProperCase(string input)
        {
            if (input == null) return null;
            return input.Substring(0, 1).ToUpper() + input.Remove(0, 1).ToLower();
        }

        /// <summary>
        /// Truncates a string by adding ... to the end
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string TruncateString(string input, int length)
        {
            if (input == null)
                return "";
            if (input.Length < length)
                return input;

            input = input.Replace(Environment.NewLine, "");
            return input.Substring(0, length) + "...";
        }

        /// <summary>
        /// Takes a query string and returns a NameValueCollection
        /// </summary>
        public static NameValueCollection BuildNameValueCollection(string qs)
        {
            NameValueCollection nvc = new NameValueCollection();

            qs = qs.IndexOf('?') > 0 ? qs.Remove(0, qs.IndexOf('?') + 1) : qs;
            Array sqarr = qs.Split("&".ToCharArray());
            for (int i = 0; i < sqarr.Length; i++)
            {
                string[] pairs = sqarr.GetValue(i).ToString().Split("=".ToCharArray());
                nvc.Add(pairs[0], pairs[1]);
            }
            return nvc;
        }

        public static string[] SplitFilterString(string filter)
        {
            return filter.Split(new string[] { ",", ";", ".", "/", "|", "\\", " " }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Convert a string to a valid URL by stripping out any non alphanumeric characters
        /// </summary>
        public static string ConvertToURL(string input)
        {
            return Regex.Replace(input, "[^a-z0-9]", "", RegexOptions.IgnoreCase).ToLower();
        }

        /// <summary>
        /// Strips any HTML tags from a string
        /// </summary>
        /// <param name="str">The string to be cleaned from HTML</param>
        /// <returns></returns>
        public static string StripHTML(string str)
        {
            //variable to hold the returned value
            string strippedString;
            try
            {
                //variable to hold our RegularExpression pattern
                string pattern = "<.*?>";
                //replace all HTML tags
                strippedString = Regex.Replace(str, pattern, string.Empty);
            }
            catch
            {
                strippedString = string.Empty;
            }
            return strippedString;
        }

        public static string GenerateURL(string title)
        {
            return GenerateURL(title, 80);
        }

        public static string GenerateURL(string title, int maxLength)
        {
            return GenerateURL(title, maxLength, true);
        }

        public static string GenerateURL(string title, int maxLength, bool removeForwardSlash)
        {
            if (String.IsNullOrEmpty(title)) return "";

            // remove entities
            title = Regex.Replace(title, @"&\w+;", "");

            if (removeForwardSlash)
                // remove anything that is not letters, numbers, dash, or space
                title = Regex.Replace(title, @"[^A-Za-z0-9\-\s]", "");
            else
                // remove anything that is not letters, numbers, dash, space or forward slash
                title = Regex.Replace(title, @"[^A-Za-z0-9\-\s/]", "");

            // remove any leading or trailing spaces left over
            title = title.Trim();
            // replace spaces with single dash
            title = Regex.Replace(title, @"\s+", "-");
            // if we end up with multiple dashes, collapse to single dash            
            title = Regex.Replace(title, @"\-{2,}", "-");
            // make it all lower case
            title = title.ToLower();
            // if it's too long, clip it
            if (title.Length > maxLength)
                title = title.Substring(0, maxLength - 1);
            // remove trailing dash, if there is one
            if (title.EndsWith("-"))
                title = title.Substring(0, title.Length - 1);

            return title;
        }

        public static string GenerateURL(string[] titles, int maxLength)
        {
            StringBuilder sb = new StringBuilder();
            string delimeter = String.Empty;

            foreach (var title in titles)
            {
                sb.AppendFormat("{0}{1}", delimeter, GenerateURL(title, maxLength));

                if (String.IsNullOrEmpty(delimeter))
                    delimeter = "/";
            }

            return sb.ToString();
        }


        public static string GenerateFileName(string filename, string extension)
        {
            // remove entities
            filename = Regex.Replace(filename, @"&\w+;", "");
            // remove anything that is not letters, numbers, dash, or space
            filename = Regex.Replace(filename, @"[^A-Za-z0-9\-\s]", "");
            // remove any leading or trailing spaces left over
            filename = filename.Trim();
            // replace spaces with single dash
            filename = Regex.Replace(filename, @"\s+", "-");
            // if we end up with multiple dashes, collapse to single dash            
            filename = Regex.Replace(filename, @"\-{2,}", "-");
            // remove trailing dash, if there is one
            if (filename.EndsWith("-"))
                filename = filename.Substring(0, filename.Length - 1);

            return String.Format("{0}{1}", filename, extension);
        }

        public static string CleanSEOString(string str)
        {
            str = StripHTML(str);

            if (str.Contains("&"))
                str = str.Replace("&", "and");

            string illegals = ":,',!,$,%,^,&,*,(,),+,=,{,},[,],@,~,;,#,<,>,?,/";
            string[] illegalArray = illegals.Split(',');
            foreach (string illegal in illegalArray)
            {
                str = str.Replace(illegal, string.Empty);
            }

            return str;
        }


        public static string CreateSearchTermFromURL(string str)
        {
            str = StripHTML(str);

            if (str.Contains("&"))
                str = str.Replace("&", "and");

            string illegals = ":,',!,$,%,^,&,*,(,),+,=,{,},[,],@,~,;,#,<,>,?,/";
            string[] illegalArray = illegals.Split(',');
            foreach (string illegal in illegalArray)
            {
                str = str.Replace(illegal, " ");
            }

            return str;
        }

        public static string ToXml(object o)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(o.GetType());

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream))
                {
                    xmlSerializer.Serialize(streamWriter, o);
                    streamWriter.Flush();
                    memoryStream.Flush();
                    memoryStream.Position = 0;

                    using (StreamReader streamReader = new StreamReader(memoryStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}
