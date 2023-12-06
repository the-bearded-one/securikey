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
        public string ID { get; set; }
        public string GptPrompt { get; set; }
        public string RiskName { get; set; } = string.Empty;

        public AiAssistant()
        {
            InitializeComponent();
            this.Shown += new EventHandler(AiAssistant_Load);
            this.Text = "How to Mitigate: " + RiskName;
        }

        private async void AiAssistant_Load(object sender, EventArgs e)
        {
            Random random = new Random();
            int randomNumber = random.Next(1, 5);

            switch(randomNumber)
            {
                case 1:
                    aiAssistantImage.Image = Resources.Resources.assistant01;
                    break;
                case 2:
                    aiAssistantImage.Image = Resources.Resources.assistant02;
                    break;
                case 3:
                    aiAssistantImage.Image = Resources.Resources.assistant03;
                    break;
                case 4:
                    aiAssistantImage.Image = Resources.Resources.assistant04;
                    break;
            }


            if (BL.Instance.GptAnswers.ContainsKey(ID))
            {
                String contentString = BL.Instance.GptAnswers[ID];
                pleaseWait.Text = "Here's your answer.";
                aiResponse.Text = contentString;
                aiResponse.Visible = true;

            }
            else
            {
                var api = new OpenAIClient(new OpenAIAuthentication("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", ""));
                string prompt = "You are a skilled cybersecurity specialist focused on hardening Windows machines. You are focused on helping to protect SMB from potential attack vectors. You are also highly skilled at conveying step-by-step instructions to the layman. Please provide remediation steps specific to windows " + GetWindowsVersion().ToString();
                
                var messages = new List<OpenAI.Chat.Message>
                {
                    new OpenAI.Chat.Message(Role.System, prompt),
                    new OpenAI.Chat.Message(Role.User, GptPrompt)
                };

                var chatRequest = new ChatRequest(messages, Model.GPT4);
                var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
                //Console.WriteLine($"{result.FirstChoice.Message.Role}: {result.FirstChoice.Message.Content}");

                dynamic dynamicContent = result.FirstChoice.Message.Content;
                if (dynamicContent is System.Text.Json.JsonElement jsonElement)
                {
                    // Get string from JsonElement
                    string contentString = jsonElement.GetString();
                    pleaseWait.Text = "Here's your answer.";
                    aiResponse.Text = contentString;
                    aiResponse.Visible = true;
                    // save the answer so we dont query for it again in this session
                    BL.Instance.GptAnswers.Add(ID, contentString);
                }
                else
                {
                    pleaseWait.Text = "Something went wrong. Please try later.";
                    aiResponse.Text = "";
                }

            }





        }

        public static int GetWindowsVersion()
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

        private void aiResponse_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
