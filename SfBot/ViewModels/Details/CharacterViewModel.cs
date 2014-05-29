﻿using System.Threading.Tasks;
using Caliburn.Micro;
using SfBot.Events;
using SfSdk.Contracts;

namespace SfBot.ViewModels.Details
{
    public class CharacterViewModel : SessionScreenBase
    {
        private ICharacter _character;
        private readonly IEventAggregator _events;

        public ICharacter Character
        {
            get { return _character; }
            set
            {
                _character = value;
                NotifyOfPropertyChange(() => Character);
            }
        }

        public CharacterViewModel(IEventAggregator events)
        {
            _events = events;
            base.DisplayName = "Character";
        }

        public override async Task LoadAsync()
        {
            IsBusy = true;
            _events.PublishOnCurrentThread(new LogEvent(Account, "Character request started"));
            Character = await Account.Session.MyCharacterAsync();
            _events.PublishOnCurrentThread(new LogEvent(Account, "Character request finished"));
            IsBusy = false;
        }

        protected override void OnActivate()
        {
 	        base.OnActivate();
            Load();
        }
    }
}