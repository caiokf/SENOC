namespace Senoc.Model.Primitives
{
    public interface IClockedElement
    {
        void Clock(ClockEvent clockEvent);
    }
}