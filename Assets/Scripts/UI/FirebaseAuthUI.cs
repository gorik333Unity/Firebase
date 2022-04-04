using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.DB;
using System.Net.Mail;
using System;

namespace Game.UI
{
    public class FirebaseAuthUI : MonoBehaviour
    {
        private const int MIN_PASSWORD_LENGTH = 7;

        [SerializeField]
        private TMP_InputField _emailInputField;

        [SerializeField]
        private TMP_InputField _passwordInputField;

        [SerializeField]
        private Database _database;

        /// <summary>
        /// Signs in to firebase on click
        /// </summary>
        public void SignInOnClick()
        {
            var email = GetAndResetInputField(_emailInputField);
            var password = GetAndResetInputField(_passwordInputField);

            if (!CheckEmail(email))
            {
                OnNonValidEmail();

                return;
            }
            if (!CheckPassword(password))
            {
                OnNonValidPasswod();

                return;
            }

            _database.SignIn(email, password);
        }

        /// <summary>
        /// Signs up to firebase on click
        /// </summary>
        public void SignUpOnClick()
        {
            var email = GetAndResetInputField(_emailInputField);
            var password = GetAndResetInputField(_passwordInputField);

            if (!CheckEmail(email))
            {
                OnNonValidEmail();

                return;
            }
            if (!CheckPassword(password))
            {
                OnNonValidPasswod();

                return;
            }

            _database.SignUp(email, password);
        }

        private bool CheckEmail(string email)
        {
            bool isEmailValid = false;
            if (string.IsNullOrEmpty(email))
            {
                OnNullOrEmptyInputField();
                return isEmailValid;
            }

            isEmailValid = IsValidEmail(email);

            return isEmailValid;
        }

        private bool CheckPassword(string password)
        {
            bool isPasswordValid = false;
            if (string.IsNullOrEmpty(password))
            {
                OnNullOrEmptyInputField();
                return isPasswordValid;
            }

            isPasswordValid = password.Length > MIN_PASSWORD_LENGTH;

            return isPasswordValid;
        }

        private bool IsValidEmail(string emailAddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailAddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private string GetAndResetInputField(TMP_InputField inputField)
        {
            var text = inputField.text;
            inputField.text = string.Empty;

            return text;
        }

        private void OnNonValidEmail()
        {
            Debug.Log("Mail is not valid!");
        }

        private void OnNonValidPasswod()
        {
            Debug.Log("Password is not valid");
        }

        private void OnNullOrEmptyInputField()
        {
            Debug.Log("Field is empty");
        }
    }
}
