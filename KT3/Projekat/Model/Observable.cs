using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Notify();
    }

    public class ObjavaObavestenja : ISubject
    {
        private List<IObserver> observers;
        private Obavestenja novoObavestenje;
        public Obavestenja NovoObavestenje
        {
            get { return novoObavestenje; }
            set
            {
                novoObavestenje = value;
                Notify();
            }
        }

        public ObjavaObavestenja()
        {
            observers = new List<IObserver>();
        }

        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Notify()
        {
            foreach (IObserver o in observers)
            {
                o.Update(this);
            }
        }
    }

    public interface IObserver
    {
        void Update(ISubject subject);
    }
}
