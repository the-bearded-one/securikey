using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecuriKey
{
    public partial class PopUpPrompt : Form
    {
        const string _passwordChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVQXYZ0123456789~!@#$%^&*()_+-=<>?,.{}|[];':";
        const string _specialChars = "~!@#$%^&*()_+-=<>?,.{}|[];':";
        const string _numChars = "0123456789";
        const string _lowerCaseChars = "abcdefghijklmnopqrstuvwxyz";
        const string _upperCaseChars = "ABCDEFGHIJKLMNOPQRSTUVQXYZ";
        InputValidation inputValidation;
        DialogResult dialogResult = DialogResult.Cancel;

        public PopUpPrompt(string title, string details, InputValidation inputValidation)
        {
            InitializeComponent();

            this.Text = title;
            this.detailsTextBox.Text = details;
            this.inputValidation = inputValidation;

            // validate input in input field
            inputTextBox.KeyUp += OnInputKeyUp;

            // call validation to initialize GUI
            OnInputKeyUp(this, EventArgs.Empty);
        }

        public string Value => inputTextBox.Text;

        public new DialogResult ShowDialog()
        {
            base.ShowDialog();
            return dialogResult;
        }

        private void OnInputKeyUp(object? sender, EventArgs e)
        {
            if (inputValidation == InputValidation.Password) inputTextBox.Text = SanitizeText(_passwordChars, inputTextBox.Text);
            this.errorLabel.Text = CheckIfValidEntry() ? string.Empty : "Please satisfy password requirements to continue";
        }

        private void OnOkButtonClick(object sender, EventArgs e)
        {
            bool isValid = CheckIfValidEntry();

            if (!isValid)
            {
                if (inputValidation == InputValidation.Password)
                {
                    this.errorLabel.Text = "Invalid password. Please check that password meet minimum requirements!";
                }
                else
                {
                    this.errorLabel.Text = "Input is not valid. Please check requirements";
                }
            }
            else
            {
                dialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private string SanitizeText(string validChars, string unsanitizedText)
        {
            // Create a HashSet to store characters from string B
            HashSet<char> validCharsSet = new HashSet<char>(validChars);

            // Create a string builder to construct the result string
            StringBuilder sanitizedText = new StringBuilder();

            // Iterate through each character in string A
            foreach (char c in unsanitizedText)
            {
                // Check if the character exists in string B
                if (validCharsSet.Contains(c))
                {
                    // If it exists in both A and B, add it to the result
                    sanitizedText.Append(c);
                }
            }

            return sanitizedText.ToString();
        }

        private bool CheckIfValidEntry()
        {
            bool isValid = false;

            if (inputValidation == InputValidation.None)
            {
                isValid = true;
            }
            else if (inputValidation == InputValidation.Password)
            {
                // has eight chars or more
                bool hasEnoughChars = inputTextBox.Text.Length >= 8;

                // must containt at least one special character
                bool hasSpecialChar = (_specialChars.Any(c => inputTextBox.Text.Contains(c)));

                // must contain at least one number
                bool hasNum = (_numChars.Any(c => inputTextBox.Text.Contains(c)));

                // must contain at least one lower case letter
                bool hasLowerCase = (_lowerCaseChars.Any(c => inputTextBox.Text.Contains(c)));

                // must contain at least one upper case letter
                bool hasUpperCase = (_upperCaseChars.Any(c => inputTextBox.Text.Contains(c)));

                isValid = hasEnoughChars && hasSpecialChar && hasNum && hasLowerCase && hasUpperCase;
            }

            return isValid;
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            dialogResult = DialogResult.Cancel;
            this.Close();
        }
    }

    public enum InputValidation
    {
        None,
        Password
    }
}
