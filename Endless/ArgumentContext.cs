
namespace Endless
{
    using System;
    using System.Diagnostics.Contracts;


    public sealed class ArgumentContext
    {
        int serviceOrderJobPersonnelId;


        public int UserId { get; set; }

        public int FormId { get; internal set; }

        // This can be a SO or SOJ ID.
        public int OwnerId { get; internal set; }


        public int ServiceOrderId
        {
            get
            {
                // From BM
                if (FormId == 30)
                    return OwnerId;
                // From Planner
                else if (FormId == default(int))
                    throw new InvalidOperationException("Service Order ID is not available when called from Planner.");
                else
                    throw new InvalidOperationException(String.Format("Please check that Business Manager's Form ID {0} does pass a service order ID.", FormId));
            }
            internal set
            {
                OwnerId = value;
            }
        }


        public int ServiceOrderJobId
        {
            get
            {
                // From BM
                if (FormId == 31)
                    return OwnerId;
                // From Planner
                else if (FormId == default(int))
                    return OwnerId;
                else
                    throw new InvalidOperationException(String.Format("Please check that Business Manager's Form ID {0} does pass a service order job ID.", FormId));
            }
            internal set
            {
                OwnerId = value;
            }
        }


        public int ServiceOrderJobPersonnelId 
        { 
            get
            {
                if (FormId != default(int))
                    throw new InvalidOperationException("Service Order Job Personnel ID is not available when called from BusinessManager.");
                return serviceOrderJobPersonnelId;
            }
            internal set
            {
                serviceOrderJobPersonnelId = value;
            }
        }


    }
}
