using OpenAI.Chat;
using OpenAI.Models;
using OpenAI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLogic;
using System.Management;

namespace SecuriKey.Screens
{
    public partial class AiAssistant : Form
    {
        private OpenAIClient openAiClient = new OpenAIClient(new OpenAIAuthentication("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", ""));
        List<OpenAI.Chat.Message> chatMessages = new();
        Font boldFont = null;
        Font regularFont = null;
        Color userColor = Color.FromArgb(41, 128, 185);
        Color aiColor = Color.FromArgb(26, 195, 126);

        #region Constructor
        public AiAssistant()
        {
            InitializeComponent();

            // Load random AI Assistant Avatar Image
            Random random = new Random();
            int randomNumber = random.Next(1, 5);
            aiAssistantImage.Image = Resources.Resources.ResourceManager.GetObject($"assistant{randomNumber:D2}") as Image;

            // Set Title
            this.Text = "How to Mitigate: " + RiskName;

            // Create Bold Font and cache normal font
            boldFont = new Font(this.chatWindow.Font, FontStyle.Bold);
            regularFont = new Font(this.chatWindow.Font, FontStyle.Regular);
        }
        #endregion

        #region Properties
        public string RiskName { get; set; } = string.Empty;
        public string InitialGptPrompt { get; set; }
        private static Dictionary<string, string> CachedAiAnswers = new Dictionary<string, string>();
        #endregion

        #region Event Handlers
        private async void OnAiAssistantShown(object sender, EventArgs e)
        {
            // Prime the AI to assume the role of cyber security specialist
            string prompt = "You are a skilled cybersecurity specialist focused on hardening Windows machines. You are focused on helping to protect SMB from potential attack vectors. You are also highly skilled at conveying step-by-step instructions to the layman. Please provide remediation steps specific to windows " + GetWindowsVersion().ToString();
            chatMessages.Add(new OpenAI.Chat.Message(Role.System, prompt));

            // Add to chat the initial prompt
            chatMessages.Add(new OpenAI.Chat.Message(Role.User, InitialGptPrompt));

            // Display cached results if available; otherwise, query Open AI
            if (CachedAiAnswers.ContainsKey(InitialGptPrompt))
            {
                String contentString = CachedAiAnswers[InitialGptPrompt];
                pleaseWait.Text = "Here's your answer.";

                // update chat display
                UpdateChatDisplay("AI ASSISTANT", contentString);
            }
            else
            {
                var chatRequest = new ChatRequest(chatMessages, Model.GPT4);
                var result = await openAiClient.ChatEndpoint.GetCompletionAsync(chatRequest);

                dynamic dynamicContent = result.FirstChoice.Message.Content;
                if (dynamicContent is System.Text.Json.JsonElement jsonElement)
                {
                    // Get string from JsonElement
                    string contentString = jsonElement.GetString();

                    // save the answer so we dont query for it again in this session
                    CachedAiAnswers.Add(InitialGptPrompt, contentString);

                    // save answer to chat message history so GPT know context for future
                    chatMessages.Add(new OpenAI.Chat.Message(Role.Assistant, contentString));

                    // update chat display
                    UpdateChatDisplay("AI ASSISTANT", contentString);
                }
                else
                {
                    pleaseWait.Text = "Something went wrong. Please try later.";
                    chatWindow.Clear();
                }

            }
        }
        #endregion

        private async void OnSendButtonClick(object sender, EventArgs e)
        {
            SendQueryToOpenAi(inputTextBox.Text);
            inputTextBox.Clear();
        }

        private void OnInputTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            // if user presses enter, send Open AI the query
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                OnSendButtonClick(this, EventArgs.Empty);
            }
        }

        private void AiAssistantKeyDown(object sender, KeyEventArgs e)
        {
            // if user presses enter, send Open AI the query
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                OnSendButtonClick(this, EventArgs.Empty);
            }
        }

        private async void SendQueryToOpenAi(string query)
        {
            UpdateChatDisplay("YOU", query);
            chatMessages.Add(new OpenAI.Chat.Message(Role.User, query));
            var chatRequest = new ChatRequest(chatMessages, Model.GPT4);
            var result = await openAiClient.ChatEndpoint.GetCompletionAsync(chatRequest);

            dynamic dynamicContent = result.FirstChoice.Message.Content;
            if (dynamicContent is System.Text.Json.JsonElement jsonElement)
            {
                // Get string from JsonElement
                string contentString = jsonElement.GetString();
                UpdateChatDisplay("AI ASSISTANT", contentString);

                // scroll text box to end
                chatWindow.SelectionStart = chatWindow.Text.Length;
                // scroll it automatically
                chatWindow.ScrollToCaret();
            }
        }

        #region Private Methods
        private int GetWindowsVersion()
        {
            var osVersion = Environment.OSVersion;
            var versionInfo = osVersion.Version;

            // Use WMI to get the product name, which includes the Windows version
            var searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            foreach (var os in searcher.Get())
            {
                var productName = os["Caption"].ToString();

                if (productName.Contains("Windows 10")) return 10;
                if (productName.Contains("Windows 11")) return 11;
            }

            return 0; // Return 0 if it's neither Windows 10 nor Windows 11
        }

        private void UpdateChatDisplay(string source, string msg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    UpdateChatDisplay(source, msg);
                }));
            }
            else
            {
                // add the source and bold it
                chatWindow.SelectionFont = boldFont;
                chatWindow.SelectionColor = source == "AI ASSISTANT" ? aiColor : userColor;
                chatWindow.AppendText(source.ToUpper());

                // reset color and font
                chatWindow.SelectionFont = regularFont;
                chatWindow.SelectionColor = Color.White;

                // add the source message on the next line
                chatWindow.AppendText($"\r\n{msg}\r\n\r\n\r\n");

                // scroll text box to end
                chatWindow.ScrollToCaret();
            }
        }
        #endregion
    }
}
