namespace Puhinsky.GenericStateMachine.Core
{
    public interface IState<out TInitializer>
    {
        public TInitializer Initializer { get; }
    }
}
