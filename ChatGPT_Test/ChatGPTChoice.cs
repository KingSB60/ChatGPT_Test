// ChatGPTChoice.cs
//
// Comments : 
// Date     : 2023-03-13
// Author   : Steffen Börner
// <copyright file="ChatGPTChoice.cs">
//     Copyright (c) Steffen Börner. All rights reserved.
// </copyright

using System.Text.Json.Serialization;

namespace ChatGPT_Test;

public class ChatGPTChoice
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("logprobs")]
    public object? LogProbabilities { get; set; }

    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }
}