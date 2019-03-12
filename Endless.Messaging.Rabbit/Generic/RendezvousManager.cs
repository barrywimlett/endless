using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using Endless.Messaging.Rabbit.Interfaces;
using RabbitMQ.Client.Events;

namespace Endless.Messaging.Rabbit.Generic
{
    public class RendezvousManager : IRendezvousManager
    {
        protected readonly ConcurrentDictionary<Guid, IRendezvousGroup> RendezvousGroups = new ConcurrentDictionary<Guid, IRendezvousGroup>();

        private readonly RabbitListener _listener;

        public RendezvousManager(IRabbitMessagingSettings messagingSettings)
        {
            _listener = new RabbitListener(messagingSettings, string.Empty, ProcessBytesRecieved);
        }

        protected string QueueName => _listener.QueueName;

        public virtual IRendezvousGroup CreateGroup()
        {
            var rendezvousGroup = new RendezvousGroup(_listener.QueueName);

            if (!RendezvousGroups.TryAdd(rendezvousGroup.RendezvousGroupID, rendezvousGroup))
            {
                throw new Exception($"Failed to add Rendezous Group {rendezvousGroup.RendezvousGroupID}");
            }

            return rendezvousGroup;
        }

        public virtual void RemoveGroup(Guid rendezvousGroupID)
        {
            IRendezvousGroup rendezvousGroup;

            if (!RendezvousGroups.TryRemove(rendezvousGroupID, out rendezvousGroup))
            {
                throw new Exception($"Failed to remove Rendezous Group {rendezvousGroup.RendezvousGroupID}");
            }
        }

        public void StartListening()
        {
            _listener.StartListening();
        }

        public void StopListening()
        {
            _listener.StopListening();
        }

        protected void ProcessBytesRecieved(BasicDeliverEventArgs args)
        {
            var split = args.BasicProperties.CorrelationId.Split(':');

            var group = Guid.Parse(split[0]);
            var ticket = Guid.Parse(split[1]);

            // var rendezvousGroup = RendezvousGroups.ToList().FirstOrDefault(rg => rg.RendezvousGroupID == group);
            IRendezvousGroup rendezvousGroup;
            if (!RendezvousGroups.TryGetValue(group, out rendezvousGroup))
            {
                throw new Exception($"Failed to get Rendezous Group {rendezvousGroup.RendezvousGroupID}");
            }

            if (rendezvousGroup == null)
            {
                Debug.Assert(false);
            }
            else
            {
                rendezvousGroup.CheckTicket(ticket);
            }
        }
    }

    public class RendezvousManager<TManagerContext, TGroupContext> : RendezvousManager,IRendezvousManager<TManagerContext>, IRendezvousManager<TManagerContext, TGroupContext>
        where TManagerContext : class, new()
        where TGroupContext : class, new()
    {
        public RendezvousManager(IRabbitMessagingSettings messagingSettings)
            : base(messagingSettings)
        {
            this.Context = new TManagerContext();
        }

        public RendezvousManager(IRabbitMessagingSettings messagingSettings,TManagerContext context)
            : base(messagingSettings)
        {
            this.Context = context;
        }

        public TManagerContext Context
        {
            get;
            protected set;
        }

        public new IRendezvousGroup<TManagerContext, TGroupContext> CreateGroup()
        {
            var rendezvousGroup = new RendezvousGroup<TManagerContext, TGroupContext>(QueueName, this.Context);

            if (!RendezvousGroups.TryAdd(rendezvousGroup.RendezvousGroupID, rendezvousGroup))
            {
                throw new Exception($"Failed to add Rendezous Group {rendezvousGroup.RendezvousGroupID}");
            }

            return rendezvousGroup;
        }

        IRendezvousGroup<TManagerContext, TGroupContext> IRendezvousManager<TManagerContext, TGroupContext>.CreateGroup()
        {
            return CreateGroup();
        }
    }
}