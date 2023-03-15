// ChatGPTUsage.cs
//
// Comments : 
// Date     : 2023-03-13
// Author   : Steffen Börner
// <copyright file="ChatGPTUsage.cs">
//     Copyright (c) Steffen Börner. All rights reserved.
// </copyright

using System.Text.Json.Serialization;

namespace ChatGPT_Test;

public class ChatGPTUsage
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }

    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }

    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}