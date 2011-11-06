namespace Senoc.Model.Eventing
{
    public interface IListenTo {}

    public class IListenTo<T> where T : IEvent
    {
        public interface All : IListenTo
        {
            void Handle(T message);
        }

        public interface SentToMe : All
        {
        }

        public interface ThatSatisfy : All
        {
            bool SatisfiedBy(T message);
        }
    }
}