using System;
using System.Collections.Generic;

namespace First_non_repeating_character
{
    class Program
    {
        public static string FirstNonRepeatingLetter(string s)
        {
          
            if (s.Length == 1) return s.Substring(0, 1);
           
            Dictionary<string, int> myDict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach(var temp in s)
            {
                if (myDict.ContainsKey(temp.ToString())) myDict[temp.ToString()] +=1;
                else myDict.Add(temp.ToString(), 0);
            }

            foreach (var item in myDict) 
            {
                if (item.Value == 0) return item.Key.ToString();
            }
            

            return String.Empty;
        }
       
        static void Main(string[] args)
        {
            Console.WriteLine(FirstNonRepeatingLetter("stress"));
            Console.Read();
        }
    }

}


