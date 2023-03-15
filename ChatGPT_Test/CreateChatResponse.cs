// CreateChatResponse.cs
//
// Comments : 
// Date     : 2023-03-13
// Author   : Steffen Börner
// <copyright file="CreateChatResponse.cs">
//     Copyright (c) Steffen Börner. All rights reserved.
// </copyright

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChatGPT_Test;

public class CreateChatResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("object")]
    public string Object { get; set; } = null!;

    [JsonPropertyName("created")]
    public int Created { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; } = null!;

    [JsonPropertyName("choices")]
    public List<ChatCompletionChoice> Choices { get; set; } = new();

    [JsonPropertyName("usage")]
    public ChatCompletionUsage? Usage { get; set; }
}