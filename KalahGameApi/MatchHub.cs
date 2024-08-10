using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class MatchHub : Hub
{
    private readonly ILogger<MatchHub> _logger;

    public MatchHub(ILogger<MatchHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("A client connected: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("A client disconnected: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task RequestMatch(string playerName, string playerAvatar)
    {
        _logger.LogInformation("RequestMatch called by {PlayerName}", playerName);
        await Clients.All.SendAsync("matchFound", new { player1Name = playerName, player1Avatar = playerAvatar });
    }

    public async Task CancelMatch()
    {
        _logger.LogInformation("CancelMatch called");
        await Clients.All.SendAsync("matchCancelled");
    }
}
