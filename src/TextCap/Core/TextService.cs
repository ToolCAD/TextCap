using System.Globalization;

namespace TextCap.Core
{
    public static class TextConvert
    {
        public static string ToLowerCase(string input)
        {
            return input.ToLower();
        }
        public static string ToUpperCase(string input)
        {
            return input.ToUpper();
        }
        public static string ToSentenceCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                // Handle empty or null input as needed
                return string.Empty;
            }

            // Convert the input string to lowercase
            input = input.ToLower();

            // Create a TextInfo object to manipulate casing
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            // Capitalize the first letter of the string
            string firstLetter = textInfo.ToTitleCase(input[0].ToString());
            string sentenceCase = firstLetter + input.Substring(1, input.Length - 1);

            return sentenceCase;
        }
        public static string ToTitleCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                // Handle empty or null input as needed
                return string.Empty;
            }

            // Create a TextInfo object to manipulate casing
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            // Convert the input string to title case
            string titleCase = textInfo.ToTitleCase(input.ToLower());

            return titleCase;
        }
    }
}
