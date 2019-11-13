using System;
using System.Collections.Generic;

namespace EventCalendar.Entities
{
    public class Event
    {
        public string Title { get; }
        public Person Invitor { get; }
        public DateTime EventTime { get; }
        public List<Person> Participants { get; }

        public Event(Person invitor, string title, DateTime dateTime)
        {
            Invitor = invitor;
            Title = title;
            EventTime = dateTime;
            Participants = new List<Person>();
        }

        public bool AddParticipant(Person newParticipant)
        {
            bool result = false;

            if (!Participants.Contains(newParticipant))
            {
                Participants.Add(newParticipant);
                result = true;
            }

            return result;
        }

        public bool RemoveParticipant(Person participant)
        {
            bool result = false;

            if (Participants.Contains(participant))
            {
                Participants.Remove(participant);
                result = true;
            }

            return result;
        }

        public List<Person> GetParticipants() => Participants;


    }
}
