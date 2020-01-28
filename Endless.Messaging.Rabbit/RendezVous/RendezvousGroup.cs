using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Endless.Messaging.Rabbit.Interfaces;

namespace Endless.Messaging.Rabbit.RendezVous
{
    public class RendezvousGroup : IRendezvousGroup
    {
        protected readonly List<Guid> TicketsRecievedBack = new List<Guid>();
        protected readonly List<Guid> TicketsTaken = new List<Guid>();
        private readonly string _queueName;

        public RendezvousGroup(string queueName)
        {
            _queueName = queueName;
        }

        public event EventHandler<RendezvousCompleteEventArgs> RendezvousCompleteEvent;

        public Guid RendezvousGroupID { get; } = Guid.NewGuid();

        public bool IsComplete { get; protected set; } = false;

        public void TakeTicket(IBasicRabbitMessage message)
        {
            Contract.Assert(message != null);
            Contract.Assert(message.BasicProperties != null);
            Contract.Assert(string.IsNullOrEmpty(message.BasicProperties.ReplyTo));
            Contract.Assert(message.MessageBody != null);
            Contract.Assert(string.IsNullOrEmpty(message.BasicProperties.CorrelationId));

            var ticket = Guid.NewGuid();

            // message.MessageBody.RendezvousTicket = ticket;
            message.BasicProperties.CorrelationId = $"{RendezvousGroupID}:{ticket}";
            TicketsTaken.Add(ticket);
            message.BasicProperties.ReplyTo = _queueName;
        }

        public void CheckTicket(Guid ticket)
        {
            if (!TicketsTaken.Contains(ticket))
            {
                throw new ApplicationException("Martian Ticket");
            }

            if (TicketsRecievedBack.Contains(ticket))
            {
                throw new ApplicationException("Duplicate Ticket Recieved");
            }

            TicketsRecievedBack.Add(ticket);

            if (TicketsRecievedBack.Count == TicketsTaken.Count)
            {
                OnRendezvousComplete();
                IsComplete = true;
            }
        }

        protected virtual void OnRendezvousComplete()
        {
            if (RendezvousCompleteEvent == null)
            {
                // No one was listening
                Trace.TraceWarning("Rendezvous group completed - but non one cared to listen");                
            }
            else
            {
                RendezvousCompleteEvent(this, new RendezvousCompleteEventArgs());
            }
        }
    }

    public class RendezvousGroup<TManagerContext,TGroupContext> : RendezvousGroup, IRendezvousGroup<TManagerContext, TGroupContext>
        where TManagerContext : class, new()
        where TGroupContext : class, new()
    {
        public RendezvousGroup(string queueName)
            : base(queueName)
        {
            this.Context = new TGroupContext();
        }

        public RendezvousGroup(string queueName, TManagerContext managerContext)
            : base(queueName)
        {
            this.ManagerContext = managerContext;
        }

        public new event EventHandler<RendezvousCompleteEventArgs<TManagerContext, TGroupContext>> RendezvousCompleteEvent;

        public TGroupContext Context { get; protected set; }

        public TManagerContext ManagerContext { get; protected set; }

        protected override void OnRendezvousComplete()
        {
            if (RendezvousCompleteEvent == null)
            {
                // No one was listening
                Trace.TraceWarning("Rendezvous group completed - but non one cared to listen");
                Debug.Assert(false, "Rendezvous group completed - but non one cared to listen");
            }
            else
            {
                RendezvousCompleteEvent(this, new RendezvousCompleteEventArgs<TManagerContext, TGroupContext>(this.ManagerContext,this.Context));
            }
        }
    }
}