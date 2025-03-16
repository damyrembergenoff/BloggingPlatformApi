using Microsoft.AspNetCore.SignalR.Client;

var hubUrl = "http://localhost:5104/hubs/notifications";

var connection = new HubConnectionBuilder()
    .WithUrl(hubUrl)
    .WithAutomaticReconnect()
    .Build();

connection.On<string>("ReceiveNotification", (message) =>
{
    Console.WriteLine($"Yangi bildirishnoma: {message}");
});

try
{
    await connection.StartAsync();
    Console.WriteLine("SignalR hubga ulandi");
}
catch(Exception ex)
{
    Console.WriteLine($"SignalR hubga ulanishda hato: {ex.Message}");
    Console.ReadLine();
}
await Task.Delay(-1);