using Serilog.Parsing;

namespace StorageSpike.Host.Common.Logging;

internal static class LogEventExtensions
{
    public static string RenderMessageWithNoDoubleQuotes(this LogEvent logEvent)
    {
        var output = new StringWriter();

        foreach (var token in logEvent.MessageTemplate.Tokens)
        {
            if (token is not PropertyToken pt)
            {
                token.Render(logEvent.Properties, output);
                continue;
            }

            if (!logEvent.Properties.TryGetValue(pt.PropertyName, out var propertyValue) ||
                (propertyValue as ScalarValue)?.Value is not string)
            {
                token.Render(logEvent.Properties, output);
                continue;
            }

            output.Write(((ScalarValue)propertyValue).Value);
        }

        return output.ToString();
    }
}
