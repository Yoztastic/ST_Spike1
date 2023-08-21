using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using Refit;

public static class Extensions
{
    public static async Task<ApiResponse<T>> Do<T>(this string message, Func<Task<ApiResponse<T>>> action)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
       // Console.ReadLine();
        var apiResponse = await action();
        if (apiResponse.StatusCode == HttpStatusCode.Created)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"With Location Header [ {apiResponse.Headers.Location}]");
        }

      //  Console.ReadLine();
        return  apiResponse;
    }

    public static async Task<T> Do<T>(this string message, Func<Task<T>> action)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        // Console.ReadLine();
        return await action();
    }

    public static string ToPrettyJson(this string readAsStringAsync)
    {
        return JsonValue.Parse(readAsStringAsync).ToJsonString(new JsonSerializerOptions() { WriteIndented = true });
    }
}
