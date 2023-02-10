using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfApp3
{
    public class FieldChecker
    {
        public bool CheckTeacher(string field_text)
        {
            if (!Regex.IsMatch(field_text, "[^А-Яа-я]+$") && char.IsUpper(field_text[0]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckAud(string field_text)
        {
            try
            {
                int.Parse(field_text);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public string CheckPodrMask(string str)
        {
            string str2 = str;

            if (str[2] != '.')
            {
                str2 = str.Insert(2, ".");
            }

            if (str[5] != '.')
            {
                str2 = str2.Insert(5, ".");
            }

            return str2;
        }

        public bool CheckPodrCountOfNumbers(string str)
        {
            if (str.Length != 6)
            {
                return false;
            }

            try
            {
                int.Parse(str);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool DeleteSpaces(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                char ch = Convert.ToChar(text.Substring(i, 1));

                if (ch == ' ')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
