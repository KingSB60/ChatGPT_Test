// ResponseGPT.cs
//
// Comments : 
// Date     : 2023-03-13
// Author   : Steffen Börner
// <copyright file="ResponseGPT.cs">
//     Copyright (c) Steffen Börner. All rights reserved.
// </copyright

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChatGPT_Test;

public class ResponseGPT
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("object")]
    public string? @Object { get; set; }

    [JsonPropertyName("created")]
    public int Created { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("choices")]
    public List<ChatGPTChoice>? Choices { get; set; }

    [JsonPropertyName("usage")]
    public ChatGPTUsage? Usage { get; set; }
}