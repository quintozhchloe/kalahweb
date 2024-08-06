using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class MatchHub : Hub
{
    private static List<string> waitingPlayers = new List<string>();

    public async Task RequestMatch(string player1Name, string player1Avatar)
    {
        if (waitingPlayers.Count > 0)
        {
            var player2ConnectionId = waitingPlayers.First();
            waitingPlayers.Remove(player2ConnectionId);

            await Clients.Client(player2ConnectionId).SendAsync("matchFound", new { player1Name, player1Avatar });
            await Clients.Client(Context.ConnectionId).SendAsync("matchFound", new { player1Name = "Another Player", player1Avatar = "/assets/2.png" });
        }
        else
        {
            waitingPlayers.Add(Context.ConnectionId);
        }
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        waitingPlayers.Remove(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}
