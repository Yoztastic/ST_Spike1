using System.Net;
using Sdk.Consumer;

public class HandlerInspector : DelegatingHandler
{
    private static readonly ConsoleSpinner _spinner = new(){Cycles = 5, Delay = 5};
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"[{request.Method}]");
        if (request.Content != null)
            Console.WriteLine($"{request.RequestUri} - {(await request.Content.ReadAsStringAsync(cancellationToken)).ToPrettyJson()}");
        else
        {
            Console.WriteLine(request.RequestUri);
        }

        //await Task.Delay(1000,cancellationToken);
        await _spinner.Turn(displayMsg: $"{request.Method.ToString().ToLower()}ing ",cancellationToken);

        var sendAsync = await base.SendAsync(request, cancellationToken);
        var readAsStringAsync = await sendAsync.Content.ReadAsStringAsync(cancellationToken);
        sendAsync.Content = new StringContent(readAsStringAsync);
        Console.ForegroundColor = ConsoleColor.Blue;
        var isJson = (int)sendAsync.StatusCode<500 && sendAsync.StatusCode!= HttpStatusCode.NoContent;
        var s = isJson ?  readAsStringAsync.ToPrettyJson() : readAsStringAsync ;
        Console.WriteLine($"[{sendAsync.StatusCode}] {s}");
        return sendAsync;
    }
}
