using System;

namespace Endless.Messaging.Rabbit.Interfaces
{
    public interface IRendezvousManager
    {
        IRendezvousGroup CreateGroup();

        void RemoveGroup(Guid rendezvousGroupID);

        void StartListening();

        void StopListening();
    }

    public interface IRendezvousManager<TManagerContext> : IRendezvousManager
        where TManagerContext : class, new()
    {
        TManagerContext Context { get; }
    }

    public interface IRendezvousManager<TManagerContext, TGroupContext> : IRendezvousManager
        where TManagerContext : class, new()
        where TGroupContext : class, new()
    {
        TManagerContext Context { get; }

        new IRendezvousGroup<TManagerContext, TGroupContext> CreateGroup();
    }
}