// ModelViewModel.cs
//
// Comments : 
// Date     : Freitag, 10. März 2023
// Author   : Steffen Börner
// <copyright file="ModelViewModel.cs">
//     Copyright (c) Steffen Börner. All rights reserved.
// </copyright

using CommunityToolkit.Mvvm.ComponentModel;

namespace ChatGPT_Test;

public class ModelViewModel : ObservableRecipient
{
    #region Fields

    private string? _name;
    private int _maxTokens;
    private bool _isChatGptModel;

    #endregion

    #region Properties

    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public int MaxTokens
    {
        get => _maxTokens;
        set => SetProperty(ref _maxTokens, value);
    }

    public bool IsChatGptModel
    {
        get => _isChatGptModel;
        set => SetProperty(ref _isChatGptModel, value);
    }

    #endregion
}