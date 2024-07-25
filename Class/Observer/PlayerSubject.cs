using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drifter.Interface;

namespace Drifter.Class.Observer
{
    internal class PlayerSubject : ISubject
    {
        private List<IObserver> observers;


        //need some object subject observes

        public PlayerSubject() {
            this.observers = new List<IObserver>();
        }


        public void Set()
        {
            Notify();
        }

        public void Notify()
        {
            foreach(IObserver observer in observers)
            {
                observer.Update();
            }
        }

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            if (!observers.Contains(observer) && observer != null)
            {
                observers.Add(observer);
            }
        }

    }
}
