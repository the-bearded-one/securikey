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

        // cached for demo purposes
        const string printSpoolerInitialGptPrompt = @"What are the easy steps for a non-technical person to disable the Print Spooler service on Windows 10 or Windows 11 to mitigate risks like 'PrintNightmare'?";
        const string printSpoolerInitialGptPromptReponse = @"Sure! To disable the Print Spooler service on Windows 11 you can follow the steps below, but remember, disabling the Print Spooler service means that your machine won't be able to print documents:

1. Press the Windows key + R at the same time. This will open the 'Run' dialog box.

2. In the 'Run' dialog box, type `services.msc` and press Enter. This will open the 'Services' window.

3. Scroll down in the 'Services' window and look for ""Print Spooler"".

4. Right-click on ""Print Spooler"" and select 'Properties'.

5. In the 'Properties' window, you will see a 'Startup type' dropdown menu. Click on the dropdown menu and set it to 'Disabled'.

6. If the Service status is 'Running', then click on the 'Stop' button below it and then click 'Apply', then 'OK'.

7. Close the 'Services' window.

That's it! You have now disabled the Print Spooler service on Windows 11. If at any time you need to enable printer functionality again, just go back and change the 'Startup type' in the 'Print Spooler' properties back to 'Automatic' then click 'Start' and 'OK'.";
        const string printSpoolerFollowUpPrompt = @"Can you elaborate on step 1? I'm a bit of a novice.";
        const string printSpoolerFollowUpResponse = @"Absolutely! Here's more detail for step 1:
1. Locate the `Windows` key on your keyboard. It's usually located on the left side of your keyboard between the `Ctrl` and `Alt` keys and has a Windows logo on it.
2. The `R` key is along the top of the keyboard and is usually found between the `E` and `T` key.
3. You need to press these two keys at the same time. Hold down the `Windows` key and while still holding it down, press the `R` key.

After you do this, a small window will open in the bottom-left part of your computer screen. This window is called the 'Run' command window and here you can type commands for your computer to run. Let me know if you need help with the next steps!";
        

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

            // preload example response if not already preloaded
            if (!CachedAiAnswers.ContainsKey(printSpoolerInitialGptPrompt)) CachedAiAnswers.Add(printSpoolerInitialGptPrompt, printSpoolerInitialGptPromptReponse);
            if (!CachedAiAnswers.ContainsKey(printSpoolerFollowUpPrompt)) CachedAiAnswers.Add(printSpoolerFollowUpPrompt, printSpoolerFollowUpResponse);
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
                // display the cached answer, but delay 4 seconds for theatrics
                await Task.Run(() =>
                {
                    Thread.Sleep(4000);

                    String contentString = CachedAiAnswers[InitialGptPrompt];

                    // update chat display
                    UpdateChatDisplay("AI ASSISTANT", contentString);
                });
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
            string contentString = string.Empty;

            UpdateChatDisplay("YOU", query);
            chatMessages.Add(new OpenAI.Chat.Message(Role.User, query));
            var chatRequest = new ChatRequest(chatMessages, Model.GPT4);

            // check if we cached first
            double similarity = CompareStringsSimilarity(query, printSpoolerFollowUpPrompt);
            if (similarity > 0.8)
            {
                // its 80% similar to our cached response for print spooler
                contentString = CachedAiAnswers[printSpoolerFollowUpPrompt];

                // delay 4 seconds for theatrics
                Thread.Sleep(4000);
            }
            else
            {
                // query chat gpt
                var result = await openAiClient.ChatEndpoint.GetCompletionAsync(chatRequest);

                dynamic dynamicContent = result.FirstChoice.Message.Content;
                if (dynamicContent is System.Text.Json.JsonElement jsonElement)
                {
                    // Get string from JsonElement
                    contentString = jsonElement.GetString();
                }
            }
            UpdateChatDisplay("AI ASSISTANT", contentString);

            // scroll text box to end
            chatWindow.SelectionStart = chatWindow.Text.Length;
            // scroll it automatically
            chatWindow.ScrollToCaret();
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
                pleaseWait.Text = "Here's your answer.";

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

        static double CompareStringsSimilarity(string str1, string str2)
        {
            str1 = str1.ToLower();
            str2 = str2.ToLower();

            int[,] matrix = new int[str1.Length + 1, str2.Length + 1];

            for (int i = 0; i <= str1.Length; i++)
            {
                for (int j = 0; j <= str2.Length; j++)
                {
                    if (i == 0)
                    {
                        matrix[i, j] = j;
                    }
                    else if (j == 0)
                    {
                        matrix[i, j] = i;
                    }
                    else if (str1[i - 1] == str2[j - 1])
                    {
                        matrix[i, j] = matrix[i - 1, j - 1];
                    }
                    else
                    {
                        matrix[i, j] = 1 + Math.Min(Math.Min(matrix[i - 1, j], matrix[i, j - 1]), matrix[i - 1, j - 1]);
                    }
                }
            }

            int maxLen = Math.Max(str1.Length, str2.Length);
            double similarityPercentage = 1.0 - (double)matrix[str1.Length, str2.Length] / maxLen;

            return similarityPercentage;
        }
    }
    #endregion
}
