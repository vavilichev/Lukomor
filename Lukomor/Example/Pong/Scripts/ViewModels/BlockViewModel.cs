﻿using System;
using System.Reactive.Linq;
using Lukomor.MVVM;

namespace Lukomor.Example.Pong
{
    public class BlockViewModel : IViewModel
    {
        public IObservable<bool> IsActive { get; }
        
        public BlockViewModel(GameSessionsService gameSessionService)
        {
            IsActive = gameSessionService.IsPaused.Select(value => !value);
        }
    }
}