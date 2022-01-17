using Infraestructure.Transversal.Logs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Nucleus.Observers
{
    public class Observable
    {
        //List of observers
        protected Dictionary<Observer, string> _observers = new Dictionary<Observer, string>();

        /// <summary>
        /// Attach an observer
        /// </summary>
        /// <param name="observer"></param>
        /// <param name="eventName"></param>
        public void attach(Observer observer, string eventName = null)
        {
            this._observers.Add(observer, eventName);
        }

        /// <summary>
        /// Detach an observer
        /// </summary>
        /// <param name="observer"></param>
        public void detach(Observer observer)
        {
            this._observers.Remove(observer);
        }

        /// <summary>
        /// Notify an event to registered observers
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="information"></param>
        public void NotifyEvent(string eventName, Information information)
        {
            foreach (Observer key in _observers.Keys)
            {
                if (_observers[key] == eventName)
                {
                    key.NotifyEvent(information);
                }
            }
        }

        /// <summary>
        /// Notify an exception to registered observers
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="errorException"></param>
        public void NotifyException(string eventName, ErrorException errorException)
        {
            foreach (Observer key in _observers.Keys)
            {
                if (_observers[key] == eventName)
                {
                    key.NotifyException(errorException);
                }
            }
        }
    }
}
