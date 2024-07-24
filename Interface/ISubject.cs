using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drifter.Interface;

namespace Drifter.Interface
{
    internal interface ISubject
    {

        public void Subscribe(IObserver observer);

        public void Unsubscribe(IObserver observer);

        public void Notify();
    }
}
