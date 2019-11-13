using System;
using System.Collections.Generic;
using System.Text;

namespace EventCalendar.Entities
{
    public class LimitedEvent : Event
    {
        public int MaxParticipators { get; set; }
        public LimitedEvent(Person invitor, string title, DateTime dateTime, int maxParticipators)
            : base(invitor, title, dateTime)
        {
            MaxParticipators = maxParticipators;
        }

        public bool IsEventNotFull(LimitedEvent ev)
        {
            bool result = false;

            if (ev.MaxParticipators > 0
                && ev.Participants.Count < ev.MaxParticipators)
            {
                result = true;
            }

            return result;
        }
    }
}

