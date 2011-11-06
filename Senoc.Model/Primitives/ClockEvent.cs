namespace Senoc.Model.Primitives
{
    public class ClockEvent
    {
        public int ClockCycle { get; set; }

        public override string ToString()
        {
            return "Clock Cycle: " + ClockCycle;
        }
    }
}