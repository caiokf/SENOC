namespace Senoc.Model.Primitives
{
    public interface IClock
    {
        void Cycle();
        void Cycle(int cycles);
        void Subscribe(IClockedElement clockedElement);
    }
}