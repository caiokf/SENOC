namespace Senoc.Model.Eventing
{
    public interface IDistributor
    {
        void SendMessage<T>(T message) where T : IEvent;
        void AddListener(object listener);
        void RemoveListener(object listener);
        void Distribute(int clockCycle);
        void Distribute(int fromCycle, int toCycle);
    }
}