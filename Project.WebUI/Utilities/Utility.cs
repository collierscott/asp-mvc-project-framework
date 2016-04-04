using System;
using System.Collections.Generic;
using System.Text;

namespace Project.WebUI.Utilities {
    
    public static class Utility {

        public static string GetFacility(string value) {

            if (string.IsNullOrEmpty(value)) {
                value = Settings.DefaultFacility;
            }

            return value;

        }

        /// <summary>
        /// Add single quotes to elements of a comma delimited string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AddSingleQuotes(string value)
        {

            if (string.IsNullOrEmpty(value)) return "";

            value = value.Trim();

            var sb = new StringBuilder();

            string[] split = value.Split(',');

            for (int i = 0; i < split.Length; i++)
            {
                if (split[i].Length <= 0) continue;

                sb.Append("'" + split[i] + "'");

                if (i < split.Length - 1)
                {
                    sb.Append(",");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Add single quotes to elements in a list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string AddSingleQuotes(List<string> list)
        {

            if (list == null || list.Count == 0) return "";

            var sb = new StringBuilder();

            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].Length <= 0) continue;

                sb.Append("'" + list[i] + "'");

                if (i < list.Count - 1)
                {
                    sb.Append(",");
                }

            }

            return sb.ToString();

        }

    }

}