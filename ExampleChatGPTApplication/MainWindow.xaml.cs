using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExampleChatGPTApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string OpenAI_ApiKey = "Your API-KEY";
        string modelName = string.Empty;
        int maxTokens = 2048;
        string user = "Anfrage > ";
        string textGPT = "Antwort GPT > ";
        bool isChatGPTModel = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (OpenAI_ApiKey == string.Empty)
            {
                MessageBox.Show("Bitte OpenAI API Key eintragen!");
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = comboBox.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    modelName = "text-davinci-002";
                    maxTokens = 2048;
                    isChatGPTModel = false;
                    break;
                case 1:
                    modelName = "text-davinci-003";
                    maxTokens = 4000;
                    isChatGPTModel = false;
                    break;
                case 2:
                    modelName = "code-davinci-002";
                    maxTokens = 2048;
                    isChatGPTModel = false;
                    break;
                case 3:
                    modelName = "gpt-3.5-turbo";
                    maxTokens = 4096;
                    isChatGPTModel = true;
                    break;
                default:
                    modelName = "text-davinci-003";
                    maxTokens = 4000;
                    isChatGPTModel = false;
                    break;
            }
        }

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isChatGPTModel)
            {
                CallGPTRequest();
            }
            else
            {
                CallChatGPTModel();
            }
        }

        private async void CallGPTRequest()
        {

            RequestGPT completionReqGTP = new RequestGPT
            {
                Model = modelName,
                Temperature = float.Parse(textBox.Text, CultureInfo.InvariantCulture.NumberFormat),
                MaxTokens = maxTokens,
                TopP = float.Parse(textBox3.Text, CultureInfo.InvariantCulture.NumberFormat),
                FrequencyPenalty = float.Parse(textBox1.Text, CultureInfo.InvariantCulture.NumberFormat),
                PresencePenalty = float.Parse(textBox2.Text, CultureInfo.InvariantCulture.NumberFormat)
            };

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    string? userResponse = messageText.Text;
                    chatBox.Foreground = new SolidColorBrush(Colors.Red);
                    chatBox.Text = user + userResponse;
                    completionReqGTP.Prompt = userResponse;
                    ResponseGPT? responseGPT = null;

                    using (HttpRequestMessage httpReq = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/completions"))
                    {
                        httpReq.Headers.Add("Authorization", $"Bearer {OpenAI_ApiKey}");
                        string requestString = JsonSerializer.Serialize(completionReqGTP);
                        httpReq.Content = new StringContent(requestString, Encoding.UTF8, "application/json");

                        using (HttpResponseMessage? httpResponse = await httpClient.SendAsync(httpReq))
                        {
                            if (httpResponse is not null)
                            {
                                string responseString = await httpResponse.Content.ReadAsStringAsync();
                                if (httpResponse.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(responseString))
                                {
                                    responseGPT = JsonSerializer.Deserialize<ResponseGPT>(responseString);
                                }
                            }
                        }
                    }

                    if (responseGPT != null)
                    {
                        string? responseText = responseGPT.Choices?[0]?.Text;
                        chatBox.Foreground = new SolidColorBrush(Colors.DarkBlue);
                        chatBox.Text = textGPT + responseText;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async void CallChatGPTModel()
        {
            var request = new RequestChatGPT
            {
                Model = modelName,
                Stream = true,
                MaxTokens = maxTokens,
                Messages = new List<ChatCompletionMessage>
                {
                    new("user", messageText.Text)
                }
            };

            HttpClient httpClient = new HttpClient();
            CreateChatResponse? responseChatGPT = null;

            using (HttpRequestMessage httpReq = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions"))
            {
                httpReq.Headers.Add("Authorization", $"Bearer {OpenAI_ApiKey}");
                string requestString = JsonSerializer.Serialize(request);
                httpReq.Content = new StringContent(requestString, Encoding.UTF8, "application/json");

                using (HttpResponseMessage? httpResponse = await httpClient.SendAsync(httpReq))
                {
                    if (httpResponse is not null)
                    {
                        string responseString = await httpResponse.Content.ReadAsStringAsync();
                        if (httpResponse.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(responseString))
                        {
                            responseChatGPT = JsonSerializer.Deserialize<CreateChatResponse>(responseString);
                        }
                    }
                }
            }

            if (responseChatGPT != null)
            {
                string? responseText = responseChatGPT.Choices?[0]?.Message?.ToString();
                chatBox.Foreground = new SolidColorBrush(Colors.DarkBlue);
                chatBox.Text = textGPT + responseText;
            }
        }
    }
}
