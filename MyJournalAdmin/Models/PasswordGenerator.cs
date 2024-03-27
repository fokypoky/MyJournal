using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJournalAdmin.Models
{
    public class PasswordGenerator
    {
        private const int _maxPasswordLength = 20;
        private char[] _chars = 
        {
			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
            'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
            'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
		};
        private char[] _digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public string Generate(int passwordLength = 20)
        {
            if (passwordLength > _maxPasswordLength)
            {
                throw new Exception("Password can't be longer than 20 characters");
            }

            if (passwordLength < 12)
            {
                throw new Exception("Password can't be shorter than 12 characters");
            }

            var passwordStringBuilder = new StringBuilder();
            var availableChars = new List<char>();

            _chars.ToList().ForEach(ch => availableChars.Add(ch));
            _digits.ToList().ForEach(digit => availableChars.Add(digit));

            availableChars = ShuffleArray(availableChars.ToArray()).ToList();

            for(int i = 0; i < passwordLength; i++)
            {
                passwordStringBuilder.Append(availableChars[new Random().Next(0, availableChars.Count - 1)]);
            }

            return passwordStringBuilder.ToString();
        }

        private char[] ShuffleArray(char[] array)
        {
            char[] arrayCopy = new char[array.Length];
            Array.Copy(array, arrayCopy, array.Length);

            for(int i = 0; i < array.Length; i++)
            {
				for(int j = 0; j < array.Length; j++)
                {
					var randomItemIndex = new Random().Next(0, array.Length - 1);
					var bufer = arrayCopy[i];
					arrayCopy[i] = arrayCopy[randomItemIndex];
					arrayCopy[randomItemIndex] = bufer;
				}
			}

            return arrayCopy;
        }
    }
}
