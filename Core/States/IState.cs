namespace Puhinsky.StateMachine
{
    public interface IState<out TInitializer>
    {
        public TInitializer Initializer { get; }
    }
}
