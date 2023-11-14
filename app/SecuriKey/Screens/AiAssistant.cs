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
                var messages = new List<OpenAI.Chat.Message>
                {
                    new OpenAI.Chat.Message(Role.System, "You are a skilled cybersecurity specialist focused on hardening Windows 10 and Windows 11 machines. You are focused on helping to protect SMB from potential attack vectors. You are also highly skilled at conveying step-by-step instructions to the layman."),
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

        private void aiResponse_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
