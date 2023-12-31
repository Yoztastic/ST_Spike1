﻿using Xunit.Abstractions;

namespace StorageSpike.Host.Tests.InMemory.Acceptance.Common.Logging;

public class XunitLoggerProvider : ILoggerProvider
{
    private readonly ITestOutputHelper _output;
    private readonly LogLevel _minLevel;

    public XunitLoggerProvider(ITestOutputHelper output)
        : this(output, LogLevel.Trace)
    {
    }

    public XunitLoggerProvider(ITestOutputHelper output, LogLevel minLevel)
    {
        _output = output;
        _minLevel = minLevel;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new XunitLogger(_output, categoryName, _minLevel);
    }

    public void Dispose()
    {
    }
}
