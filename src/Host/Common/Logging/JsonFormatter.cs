using Serilog.Formatting.Json;
using Serilog.Parsing;

namespace StorageSpike.Host.Common.Logging;

internal class JsonFormatter : ITextFormatter
{
    private static readonly JsonValueFormatter Formatter = new JsonValueFormatter();

    private static readonly HashSet<string> Properties =
        new HashSet<string> { "conversationid", "contextidentity", "clientcomponent", "clientip", "originalip" };
    private readonly HashSet<string> _extraProperties;
    private readonly LogConstantValues _constantValues;
    private readonly string _appNameProperty;
    private readonly bool _skipDoubleQuotesInLogMessage;

    public JsonFormatter(LogConstantValues constantValues, IEnumerable<string> extraProperties, IOsInfoProvider osInfoProvider)
    {
        _constantValues = constantValues;
        _extraProperties = new HashSet<string>(extraProperties);
        var isWindows = osInfoProvider.IsWindows;
        _appNameProperty = isWindows ? "component" : "name";
        _skipDoubleQuotesInLogMessage = !isWindows;
    }

    public void Format(LogEvent logEvent, TextWriter output)
    {
        output.Write("{");

        WriteString(output, "apphost", _constantValues.HostName);
        WriteSeparator(output);
        WriteString(output, "appversion", _constantValues.AppVersion);
        WriteSeparator(output);

        if (logEvent.Properties.TryGetValue("componentscope", out var value)
            && value is ScalarValue scalar
            && scalar.Value is string scopeSuffix)
        {
            WriteString(output, _appNameProperty, _constantValues.AppName, "_", scopeSuffix);
        }
        else
        {
            WriteString(output, _appNameProperty, _constantValues.AppName);
        }

        WriteSeparator(output);
        WriteString(output, "environment", _constantValues.EnvShortName);
        WriteSeparator(output);
        WriteString(output, "eventtype", LogConstantValues.EventType);

        if (_constantValues.Slice != null)
        {
            WriteSeparator(output);
            WriteString(output, "slice", _constantValues.Slice);
        }

        WriteSeparator(output);
        FormatEvent(logEvent, output, _skipDoubleQuotesInLogMessage);

        FormatProperties(logEvent, output);

        output.Write('}');
        output.WriteLine();
    }

    private static void FormatEvent(LogEvent logEvent, TextWriter output, bool skipDoubleQuotesInLogMessage)
    {
        WriteString(output, "eventtime", logEvent.Timestamp.ToString("O"));

        if (logEvent.Exception != null)
        {
            WriteSeparator(output);
            FormatException(logEvent.Exception, "exceptiondata", output);
        }

        WriteSeparator(output);

        var level = GetLevelString(logEvent.Level);
        WriteString(output, "level", level);

        WriteSeparator(output);

        var id = Guid.NewGuid();
        WriteString(output, "logentryid", id.ToString());

        WriteSeparator(output);

        //We currently have an issue in Linux / syslog where double quotes inside the log message are not properly escaped and break logstash json parsing.
        //As Serilog is adding double quotes around every string property rendered, this is currently breaking most of our log events on Linux.
        //This code will remove those automatically added double codes, but if the properties themselves or the message template contain double quotes,
        //it would still fail, so not a proper or a 100 % fix but a "good-enough" fix for our specific case.
        //Should revisit after Linux and Windows logging have converged.
        if (skipDoubleQuotesInLogMessage)
        {
            var message = logEvent.RenderMessageWithNoDoubleQuotes();
            WriteValue(output, "logmessage", message);
        }
        //Behaviour in windows machines should remain as is
        else
        {
            var message = logEvent.MessageTemplate.Render(logEvent.Properties);
            WriteValue(output, "logmessage", message);
        }

        WriteSeparator(output);

        var template = logEvent.MessageTemplate.Text;
        WriteValue(output, "messagetemplate", template);
    }

    private static void FormatException(Exception exception, string name, TextWriter output)
    {
        output.Write('"');
        output.Write(name);
        output.Write("\":{");

        WriteValue(output, "message", exception.Message);
        WriteSeparator(output);
        WriteString(output, "type", exception.GetType().FullName);
        WriteSeparator(output);
        WriteValue(output, "stacktrace", exception.StackTrace);
        WriteSeparator(output);
        WriteString(output, "source", exception.Source);

        if (exception.InnerException != null)
        {
            WriteSeparator(output);
            FormatException(exception.InnerException, "innerexceptiondata", output);
        }

        output.Write("}");
    }

    private void FormatProperties(LogEvent logEvent, TextWriter output)
    {
        var extraWriter = new StringWriter();
        var emptyExtra = true;

        var tokenWriter = new StringWriter();
        var emptyProperties = true;

        var messageTokens = new HashSet<string>(logEvent.MessageTemplate.Tokens.OfType<PropertyToken>()
            .Select(x => x.PropertyName));

        foreach (var property in logEvent.Properties)
        {
            if (property.Key == "SourceContext")
            {
                WriteSeparator(output);
                WriteValue(output, "source", property.Value);
            }
            else if (Properties.Contains(property.Key))
            {
                WriteSeparator(output);
                WriteValue(output, property.Key, property.Value);
            }
            else if (_extraProperties.Contains(property.Key))
            {
                if (emptyExtra)
                {
                    emptyExtra = false;
                }
                else
                {
                    WriteSeparator(extraWriter);
                }

                WriteValue(extraWriter, property.Key, property.Value);
            }
            else if (messageTokens.Contains(property.Key))
            {
                if (emptyProperties)
                {
                    emptyProperties = false;
                }
                else
                {
                    WriteSeparator(tokenWriter);
                }

                WriteValue(tokenWriter, property.Key, property.Value);
            }
        }

        if (!emptyProperties)
        {
            WriteSeparator(output);

            output.Write("\"properties\":{");
            output.Write(tokenWriter);
            output.Write('}');
        }

        if (!emptyExtra)
        {
            WriteSeparator(output);

            output.Write("\"extra\":{");
            output.Write(extraWriter);
            output.Write('}');
        }
    }

    private static void WriteString(TextWriter output, string name, string value)
    {
        output.Write('"');
        output.Write(name);
        output.Write("\":\"");
        output.Write(value);
        output.Write('"');
    }

    private static void WriteString(TextWriter output, string name, params string[] values)
    {
        output.Write('"');
        output.Write(name);
        output.Write("\":\"");

        for (var i = 0; i < values.Length; i++)
        {
            output.Write(values[i]);
        }

        output.Write('"');
    }

    private static void WriteSeparator(TextWriter output)
    {
        output.Write(',');
    }

    private static void WriteValue(TextWriter output, string name, LogEventPropertyValue value)
    {
        output.Write('"');
        output.Write(name);
        output.Write("\":");
        Formatter.Format(value, output);
    }

    private static void WriteValue(TextWriter output, string name, string value)
    {
        output.Write('"');
        output.Write(name);
        output.Write("\":");
        JsonValueFormatter.WriteQuotedJsonString(value ?? string.Empty, output);
    }

    private static string GetLevelString(LogEventLevel logEventLevel)
    {
        switch (logEventLevel)
        {
            case LogEventLevel.Verbose:
                return "TRACE";
            case LogEventLevel.Debug:
                return "DEBUG";
            case LogEventLevel.Information:
                return "INFO";
            case LogEventLevel.Warning:
                return "WARN";
            case LogEventLevel.Error:
                return "ERROR";
            case LogEventLevel.Fatal:
                return "FATAL";
            default:
                throw new ArgumentOutOfRangeException(nameof(logEventLevel), logEventLevel, null);
        }
    }
}
