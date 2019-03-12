using System;

namespace Endless.Messaging.Rabbit.Interfaces
{
    public interface IRendezvousGroup
    {
        event EventHandler<RendezvousCompleteEventArgs> RendezvousCompleteEvent;

        Guid RendezvousGroupID { get; }

        bool IsComplete { get; }

        void TakeTicket(IBasicRabbitMessage message);

        void CheckTicket(Guid ticket);
    }

    public interface IRendezvousGroup<TManagerContext, TGroupContext> : IRendezvousGroup
        where TGroupContext : class, new()
        where TManagerContext : class, new()
    {
        new event EventHandler<RendezvousCompleteEventArgs<TManagerContext, TGroupContext>> RendezvousCompleteEvent;

        TGroupContext Context { get; }

        TManagerContext ManagerContext { get; }
    }

    public class RendezvousCompleteEventArgs : EventArgs
    {
    }

    public class RendezvousCompleteEventArgs<TManagerContext, TGroupContext> : EventArgs
        where TGroupContext : class, new()
        where TManagerContext : class, new()
    {
        public RendezvousCompleteEventArgs(TManagerContext managerContext, TGroupContext groupContext)
        {
            this.GroupContext = groupContext;
            this.ManagerContext = managerContext;
        }

        public TGroupContext GroupContext { get; protected set; }

        public TManagerContext ManagerContext { get; protected set; }
    }
}