using System;

namespace model;

public record MessageRequest
{
    public string Message {get; init;} = string.Empty;
}
