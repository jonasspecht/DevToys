using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DevToys.Helpers.RandomString
{
    public static class RandomStringGenerator
    {
        public const string LowerCaseLetters = "abcdefghijklmnopqrstuvwxyz";
        public const string UpperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string Numbers = "1234567890";
        public const string SpecialCharacters = @"~!@#$%^&*_-+=`|\(){}[]:;""'<>,.?/";

        private static readonly RNGCryptoServiceProvider Rng = new();

        public static List<string> Generate(
            int length = 16, 
            int amountToGenerate = 1, 
            bool includeLowerCase = true, 
            bool includeUpperCase = true, 
            bool includeNumbers = true,
            bool includeSpecialCharacters = true,
            string customCharacterSet = "")
        {
            if (length < 1)
            {
                throw new InvalidOperationException("Length must be greater than 0");
            }

            string characterSet = !string.IsNullOrWhiteSpace(customCharacterSet)
                ? customCharacterSet
                : BuildCharacterSet(includeLowerCase, includeUpperCase, includeNumbers, includeSpecialCharacters);

            if (string.IsNullOrWhiteSpace(characterSet))
            {
                throw new InvalidOperationException("Invalid character set configuration");
            }

            var randomStrings = new List<string>(amountToGenerate);
            for (int i = 0; i < amountToGenerate; i++)
            {
                var randomString = new StringBuilder(length);
                for (int j = 0; j < length; j++)
                {
                    int randomNumber = GetRandomNumber(characterSet.Length);
                    randomString.Append(characterSet[randomNumber]);
                }
                randomStrings.Add(randomString.ToString());
            }

            return randomStrings;
        }

        private static string BuildCharacterSet(bool includeLowerCase, bool includeUpperCase, bool includeNumbers, bool includeSpecialCharacters)
        {
            var combinedCharacters = new StringBuilder();
            if (includeLowerCase)
            {
                combinedCharacters.Append(LowerCaseLetters);
            }

            if (includeUpperCase)
            {
                combinedCharacters.Append(UpperCaseLetters);
            }

            if (includeNumbers)
            {
                combinedCharacters.Append(Numbers);
            }

            if (includeSpecialCharacters)
            {
                combinedCharacters.Append(SpecialCharacters);
            }

            return combinedCharacters.ToString();
        }

        private static int GetRandomNumber(int exclusiveMax)
        {
            byte[] data = new byte[sizeof(int)];
            int randomNumber;
            do
            {
                Rng.GetBytes(data);
                randomNumber = BitConverter.ToInt32(data, 0) & int.MaxValue;
            }
            while (randomNumber >= exclusiveMax * (int.MaxValue / exclusiveMax));
            return randomNumber % exclusiveMax;
        }
    }
}
