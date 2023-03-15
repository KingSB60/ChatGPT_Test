// RequestGPT.cs
//
// Comments : 
// Date     : 2023-03-13
// Author   : Steffen Börner
// <copyright file="RequestGPT.cs">
//     Copyright (c) Steffen Börner. All rights reserved.
// </copyright

using System.Text.Json.Serialization;

namespace ChatGPT_Test;

public class RequestGPT
{
    public RequestGPT() { }

    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("prompt")]
    public string? Prompt { get; set; }

    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; }

    [JsonPropertyName("temperature")]
    public float Temperature { get; set; }

    [JsonPropertyName("top_p")]
    public float TopP { get; set; }

    [JsonPropertyName("presence_penalty")]
    public float PresencePenalty { get; set; }

    [JsonPropertyName("frequency_penalty")]
    public float FrequencyPenalty { get; set; }
}