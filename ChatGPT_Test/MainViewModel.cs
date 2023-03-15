// MainViewModel.cs
//
// Comments : 
// Date     : 2023-03-10
// Author   : Steffen Börner
// <copyright file="MainViewModel.cs">
//     Copyright (c) Steffen Börner. All rights reserved.
// </copyright

using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;

namespace ChatGPT_Test;

public class MainViewModel : ObservableRecipient
{
    #region Fields

    private readonly string _openAI_ApiKey = "Your API-KEY";
    private string _chat;
    private string _messageText;
    private ObservableCollection<ModelViewModel> _models;

    private ICommand? _mainWindowLoadedCommand;
    private ICommand? _sendMessageCommand;
    private ModelViewModel _selectedModel;
    private float _temperature;
    private float _presencePenalty;
    private float _frequencyPenalty;
    private float _topP;
    private bool _isSending;

    #endregion

    #region Constructor(s)

    public MainViewModel()
    {
        _models = new ObservableCollection<ModelViewModel>
        {
            new()
            {
                Name = "text-davinci-02",
                MaxTokens = 2048,
                IsChatGptModel = false
            },
            new()
            {
                Name = "text-davinci-03",
                MaxTokens = 4000,
                IsChatGptModel = false
            },
            new()
            {
                Name = "code-davinci-02",
                MaxTokens = 2048,
                IsChatGptModel = false
            },
            new()
            {
                Name = "gpt-3.5-turbo",
                MaxTokens = 4096,
                IsChatGptModel = true
            },
            new()
            {
                Name = "code-davinci-03",
                MaxTokens = 4000,
                IsChatGptModel = false
            }
        };
        _selectedModel = _models[1];

        _temperature = 0.5f;
        _presencePenalty = 0;
        _frequencyPenalty = 0.5f;
        _topP = 0.5f;
        _isSending = false;

        _chat = string.Empty;
        _messageText = string.Empty;
    }

    #endregion

    #region Properties

    public string Chat
    {
        get => _chat;
        set => SetProperty(ref _chat, value);
    }

    public string MessageText
    {
        get => _messageText;
        set => SetProperty(ref _messageText, value);
    }

    public float Temperature
    {
        get => _temperature;
        set => SetProperty(ref _temperature, value);
    }

    public float PresencePenalty
    {
        get => _presencePenalty;
        set => SetProperty(ref _presencePenalty, value);
    }

    public float FrequencyPenalty
    {
        get => _frequencyPenalty;
        set => SetProperty(ref _frequencyPenalty, value);
    }

    public float TopP
    {
        get => _topP;
        set => SetProperty(ref _topP, value);
    }

    public bool IsSending
    {
        get => _isSending;
        set => SetProperty(ref _isSending, value);
    }

    public ObservableCollection<ModelViewModel> Models => _models;

    public ModelViewModel SelectedModel
    {
        get => _selectedModel;
        set => SetProperty(ref _selectedModel, value);
    }

    [UsedImplicitly]
    public ICommand MainWindowLoadedCommand => _mainWindowLoadedCommand ??= new RelayCommand(() =>
    {
        if (_openAI_ApiKey == string.Empty)
        {
            MessageBox.Show("Bitte OpenAI API Key eintragen!");
        }
    });

    public ICommand SendMessageCommand => _sendMessageCommand ??= new RelayCommand(() =>
    {
        if (!_selectedModel.IsChatGptModel)
        {
            CallGPTRequest();
        }
        else
        {
            CallChatGPTModel();
        }
    });

    #endregion

    #region Methods

    private async void CallGPTRequest()
    {

        RequestGPT completionReqGTP = new RequestGPT
        {
            Model = _selectedModel.Name,
            Temperature = _temperature,
            MaxTokens = _selectedModel.MaxTokens,
            TopP = _topP,
            FrequencyPenalty = _frequencyPenalty,
            PresencePenalty = _presencePenalty
        };

        using (HttpClient httpClient = new HttpClient())
        {
            try
            {
                IsSending = true;
                Chat = $"{Strings.TXT_Request} > {_messageText}";
                completionReqGTP.Prompt = _messageText;
                ResponseGPT? responseGPT = null;

                using (HttpRequestMessage httpReq =
                       new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/completions"))
                {
                    httpReq.Headers.Add("Authorization", $"Bearer {_openAI_ApiKey}");
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
                    IsSending = false;
                    Chat = $"{Strings.TXT_AnswerGPT} > {responseGPT.Choices?[0]?.Text}";
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
            Model = _selectedModel.Name,
            MaxTokens = _selectedModel.MaxTokens,
            Stream = true,
            Messages = new List<ChatCompletionMessage>
            {
                new("user", _messageText)
            }
        };

        HttpClient httpClient = new HttpClient();
        CreateChatResponse? responseChatGPT = null;
        IsSending = true;

        using (HttpRequestMessage httpReq =
               new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions"))
        {
            httpReq.Headers.Add("Authorization", $"Bearer {_openAI_ApiKey}");
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
            IsSending = false;
            Chat = $"{Strings.TXT_AnswerGPT} > {responseChatGPT.Choices?[0]?.Message}";
        }
    }

    #endregion
}