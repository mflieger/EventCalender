using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using EventCalendar.Entities;
using static System.String;

namespace EventCalendar.Logic
{
    public class Controller
    {
        private readonly ICollection<Event> _events;
        public int EventsCount => _events.Count;

        public Controller()
        {
            _events = new List<Event>();
        }

        /// <summary>
        /// Ein Event mit dem angegebenen Titel und dem Termin wird für den Einlader angelegt.
        /// Der Titel muss innerhalb der Veranstaltungen eindeutig sein und das Datum darf nicht
        /// in der Vergangenheit liegen.
        /// Mit dem optionalen Parameter maxParticipators kann eine Obergrenze für die Teilnehmer festgelegt
        /// werden.
        /// </summary>
        /// <param name="invitor"></param>
        /// <param name="title"></param>
        /// <param name="dateTime"></param>
        /// <param name="maxParticipators"></param>
        /// <returns>Wurde die Veranstaltung angelegt</returns>
        public bool CreateEvent(Person invitor, string title, DateTime dateTime, int maxParticipators = 0)
        {
            bool result = false;

            if(invitor != null && title != null && dateTime != null && title.Length > 0)
            {
                if (DateTime.Now < dateTime && GetEvent(title) == null)
                {
                    if (maxParticipators == 0)
                    {
                        Event newEvent = new Event(invitor, title, dateTime);
                        _events.Add(newEvent);
                        result = true;
                    }
                    else
                    {
                        Event newEvent = new LimitedEvent(invitor, title, dateTime, maxParticipators);
                        _events.Add(newEvent);
                        result = true;
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Liefert die Veranstaltung mit dem Titel
        /// </summary>
        /// <param name="title"></param>
        /// <returns>Event oder null, falls es keine Veranstaltung mit dem Titel gibt</returns>
        public Event GetEvent(string title)
        {
            Event resultEvent = null;

            if(title != null)
            {
                foreach (Event ev in _events)
                {
                    if (title == ev.Title)
                    {
                        resultEvent = ev;
                        break;
                    }
                }
            }

            return resultEvent;
        }

        /// <summary>
        /// Person registriert sich für Veranstaltung.
        /// Eine Person kann sich zu einer Veranstaltung nur einmal registrieren.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="ev">Veranstaltung</param>
        /// <returns>War die Registrierung erfolgreich?</returns>
        public bool RegisterPersonForEvent(Person person, Event ev)
        {
            if (person == null || ev == null)
            {
                return false;
            }

            bool result = false;
            Event currentEvent = GetEvent(ev.Title);

            if (currentEvent != null)
            {
                result = currentEvent.AddParticipant(person);
            }

            return result;
        }

        /// <summary>
        /// Person meldet sich von Veranstaltung ab
        /// </summary>
        /// <param name="person"></param>
        /// <param name="ev">Veranstaltung</param>
        /// <returns>War die Abmeldung erfolgreich?</returns>
        public bool UnregisterPersonForEvent(Person person, Event ev)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            if (ev == null)
                throw new ArgumentNullException(nameof(ev));

            bool result = false;
            Event currentEvent = GetEvent(ev.Title);

            if (currentEvent != null)
            {
                result = currentEvent.RemoveParticipant(person);
            }

            return result;
        }

        /// <summary>
        /// Liefert alle Teilnehmer an der Veranstaltung.
        /// Sortierung absteigend nach der Anzahl der Events der Personen.
        /// Bei gleicher Anzahl nach dem Namen der Person (aufsteigend).
        /// </summary>
        /// <param name="ev"></param>
        /// <returns>Liste der Teilnehmer oder null im Fehlerfall</returns>
        public IList<Person> GetParticipatorsForEvent(Event ev)
        {
            if (ev == null)
            {
                return null;
            }

            IList<Person> resultList = null;
            Event currentEvent = GetEvent(ev.Title);

            if (currentEvent != null)
            {
                resultList = currentEvent.GetParticipants();
            }

            return resultList;
        }

        /// <summary>
        /// Liefert alle Veranstaltungen der Person nach Datum (aufsteigend) sortiert.
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Liste der Veranstaltungen oder null im Fehlerfall</returns>
        public List<Event> GetEventsForPerson(Person person)
        {
            if (person == null)
            {
                return null;
            }

            List<Event> resultList = new List<Event>();

            foreach (Event ev in _events)
            {
                if (ev.Participants.Contains(person))
                {
                    resultList.Add(ev);
                }
            }

            return resultList;
        }

        /// <summary>
        /// Liefert die Anzahl der Veranstaltungen, für die die Person registriert ist.
        /// </summary>
        /// <param name="participator"></param>
        /// <returns>Anzahl oder 0 im Fehlerfall</returns>
        public int CountEventsForPerson(Person participator)
        {
            if (participator == null)
            {
                return 0;
            }

            List<Event> ev = GetEventsForPerson(participator);

            return ev.Count;
        }

    }
}
