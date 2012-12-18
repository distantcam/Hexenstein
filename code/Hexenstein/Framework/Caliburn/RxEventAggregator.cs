using System;
using Caliburn.Micro;

namespace Hexenstein.Framework.Caliburn
{
    internal class RxEventAggregator : IEventAggregator
    {
        public Action<System.Action> PublicationThreadMarshaller
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Publish(object message, Action<System.Action> marshal)
        {
            throw new NotImplementedException();
        }

        public void Publish(object message)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(object instance)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(object instance)
        {
            throw new NotImplementedException();
        }
    }
}