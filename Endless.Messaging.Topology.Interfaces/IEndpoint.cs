﻿using Next.Search.Messaging.Generic;

namespace Next.Search.Messaging.Topology.Interfaces
{
    public interface IEndpoint<TMessageBody> : IListener<TMessageBody>, ISender<TMessageBody>
        where TMessageBody : class, new()
    {
    }
}
