// ChatCompletionChoice.cs
//
// Comments : 
// Date     : 2023-03-13
// Author   : Steffen Börner
// <copyright file="ChatCompletionChoice.cs">
//     Copyright (c) Steffen Börner. All rights reserved.
// </copyright

using System.Text.Json.Serialization;

namespace ChatGPT_Test;

public class ChatCompletionChoice
{
    [JsonPropertyName("delta")]
    public ChatCompletionMessage? Delta
    {
        get => Message;
        set => Message = value;
    }

    [JsonPropertyName("message")]
    public ChatCompletionMessage? Message { get; set; }

    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; } = null!;
}