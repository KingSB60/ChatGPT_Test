// ChatCompletionMessage.cs
//
// Comments : 
// Date     : 2023-03-13
// Author   : Steffen Börner
// <copyright file="ChatCompletionMessage.cs">
//     Copyright (c) Steffen Börner. All rights reserved.
// </copyright

using System.Text.Json.Serialization;

namespace ChatGPT_Test;

public class ChatCompletionMessage
{
    public ChatCompletionMessage(string role, string content)
    {
        Role = role;
        Content = content;
    }

    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }
}