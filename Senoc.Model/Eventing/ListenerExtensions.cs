using System;
using System.Linq;
using System.Reflection;
using Senoc.Model.Events;
using StructureMap.TypeRules;

namespace Senoc.Model.Eventing
{
    public static class ListenerExtensions
    {
        public static void Handle(this IListenTo listener, IEvent message, Type messageType)
        {
            var listenerType = listener.GetType();

            if (IAmListeningJustForMyMessages(listenerType, messageType) && TheMessageWasNotSentToMe(listener, message))
                return;

            if (IFilterMessages(listenerType, messageType) && TheMessageDoNotSatisfyMyFilter(listener, message, messageType))
                return;

            var genericHandleMethod = HandleMethod(listenerType, messageType);
            
            if (genericHandleMethod == null)
                return;

            genericHandleMethod.Invoke(listener, new[] {message});
        }

        private static MethodInfo HandleMethod(Type listenerType, Type messageType)
        {
            return listenerType
                .GetMethods()
                .Where(x => x.Name.Contains("Handle"))
                .Where(x => x.GetParameters()
                    .Where(p => p.ParameterType == messageType)
                    .Count() > 0)
                .FirstOrDefault();
        }

        private static MethodInfo SatisfiedByMethod(Type closedInterface)
        {
            return closedInterface
                .GetMethods()
                .Where(x => x.Name.Contains("SatisfiedBy"))
                .FirstOrDefault();
        }

        private static bool IFilterMessages(Type listenerType, Type messageType)
        {
            var closedInterface = typeof(IListenTo<>.ThatSatisfy).MakeGenericType(messageType);
            return listenerType.CanBeCastTo(closedInterface);
        }

        private static bool TheMessageDoNotSatisfyMyFilter(IListenTo listener, IEvent message, Type messageType)
        {
            var closedInterface = typeof(IListenTo<>.ThatSatisfy).MakeGenericType(messageType);
            var genericMethod = SatisfiedByMethod(closedInterface);
            
            if (genericMethod == null)
                return true;

            return !(bool)genericMethod.Invoke(listener, new[] { message });
        }

        private static bool TheMessageWasNotSentToMe(IListenTo listener, dynamic message)
        {
            var messageIsDirectedEvent = ((Type)message.GetType()).ImplementsInterfaceTemplate(typeof(IDirectedEvent<>));
            if (!messageIsDirectedEvent)
                return true;

            if (message.CurrentReceiver == null)
                return true;

            var messageWasSentToMe = message.CurrentReceiver.Equals(listener);

            return !messageWasSentToMe;
        }

        private static bool IAmListeningJustForMyMessages(Type listenerType, Type messageType)
        {
            var closedInterface = typeof (IListenTo<>.SentToMe).MakeGenericType(messageType);
            return listenerType.CanBeCastTo(closedInterface);
        }
    }
}