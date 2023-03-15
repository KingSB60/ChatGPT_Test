﻿// RequestChatGPT.cs
//
// Comments : 
// Date     : 2023-03-13
// Author   : Steffen Börner
// <copyright file="RequestChatGPT.cs">
//     Copyright (c) Steffen Börner. All rights reserved.
// </copyright

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChatGPT_Test;

public class RequestChatGPT
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = null!;

    [JsonPropertyName("messages")]
    public IList<ChatCompletionMessage> Messages { get; set; } = new List<ChatCompletionMessage>();

    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }

    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }

    [JsonPropertyName("stream")]
    public bool Stream { get; set; }

    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }

    [JsonPropertyName("presence_penalty")]
    public float? PresencePenalty { get; set; }

    [JsonPropertyName("frequency_penalty")]
    public float? FrequencyPenalty { get; set; }
}