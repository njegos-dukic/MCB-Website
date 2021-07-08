using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Util
{
    public static class InputValidator
    {
        public const int PasswordMinimumCharsNum = 6;
        private static readonly Regex emailVerificationRegex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", RegexOptions.Compiled);
        private static readonly Regex phoneVerificationRegex = new Regex(@"^([0-9]{9})$|^(\+[0-9]{3}[0-9]{8})$|^(00[0-9]{3}[0-9]{8})$|^([0-9]{3}\/[0-9]{6})$|^([0-9]{3}\/[0-9]{3}\-[0-9]{3})$", RegexOptions.Compiled);
        private static readonly Regex passwordAllowedCharsRegex = new Regex(@"[^\s]*", RegexOptions.Compiled);
        private static readonly Regex postalCodeVerificationRegex = new Regex(@"^[\d]{5}$");
        private static readonly Regex httpUrlVerificationRegex = new Regex(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");
        private static readonly Regex coordinatesVerificationRegex = new Regex(@"^(-?\d+(\.\d+)?),\s*(-?\d+(\.\d+)?)$");


        public static bool IsValidPassword(string password) => password.Length >= PasswordMinimumCharsNum && IsValidFormat(password, passwordAllowedCharsRegex);

        public static bool IsValidEmail(string email) => IsValidFormat(email, emailVerificationRegex);

        public static bool IsValidPhoneNumber(string phone) => IsValidFormat(phone, phoneVerificationRegex);

        public static bool IsValidPostalCode(string postalCode) => IsValidFormat(postalCode, postalCodeVerificationRegex);

        public static bool IsValidHttpUrl(string url) => IsValidFormat(url, httpUrlVerificationRegex);

        public static bool AreValidCoordinates(string coordinates) => IsValidFormat(coordinates, coordinatesVerificationRegex);

        public static bool IsValidString(string value) => !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value);

        private static bool IsValidFormat(string forValidation, Regex regex)
        {
            try
            {
                return regex.IsMatch(forValidation);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }
    }
}
