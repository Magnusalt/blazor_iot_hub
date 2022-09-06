﻿namespace Models;

public class Event
{
    public Event(Guid sourceId, byte[] payload)
    {
        SourceId = sourceId;
        Payload = payload;
    }

    public Guid Id => Guid.NewGuid();
    public Guid SourceId { get; }
    public byte[] Payload { get; }
}