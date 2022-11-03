using PlayFab;
using PlayFab.ClientModels;
using PlayFab.ServerModels;
using DeviceId;
using BotAppToTest;

// See https://aka.ms/new-console-template for more
// information
Console.WriteLine("Enter number of bots to create");
PlayFabSettings.staticSettings.DeveloperSecretKey = PlayFabConfig.DeveloperSecretKey;
PlayFabSettings.staticSettings.TitleId = PlayFabConfig.TitleID;


int botCount = Convert.ToInt32(Console.ReadLine());

for(int i = 0; i < botCount; i++)
{
    var bot = new BotTesting();
    bot.RandomLogin().ContinueWith((result) => {
        if (result.Result.Result.NewlyCreated)
        {
            bot.WritePlayFabIDToFile(result.Result.Result.PlayFabId);
        }
        bot.ModifyScore();
    });
}


Console.ReadLine();

public class BotTesting 
{
    public string PlayFabId
    {
        get;
        set;
    }

    private Random Random
    {
        get;
        set;
    }

    public BotTesting()
    {
        PlayFabId = "";
        Random = new Random();
    }
    public async Task<PlayFabResult<LoginResult>> RandomLogin()
    {
        string deviceID = new DeviceIdBuilder().AddMachineName().ToString() + Guid.NewGuid().ToString();
        var request = new LoginWithCustomIDRequest { CustomId = deviceID, CreateAccount = true };
        Task<PlayFabResult<LoginResult>> loginTask = PlayFabClientAPI.LoginWithCustomIDAsync(request);
        return await loginTask;
    }

    public async Task<PlayFabResult<PlayFab.ServerModels.UpdatePlayerStatisticsResult>> ModifyScore()
    {
        PlayFab.ServerModels.UpdatePlayerStatisticsRequest request = new PlayFab.ServerModels.UpdatePlayerStatisticsRequest
        {
            PlayFabId = this.PlayFabId,
            Statistics = new List<PlayFab.ServerModels.StatisticUpdate>
            {
                new PlayFab.ServerModels.StatisticUpdate
                {
                    StatisticName = "LeagueSP",
                    Value = Random.Next(0,500)
                }
            }
        };

        var modStatsTask = PlayFabServerAPI.UpdatePlayerStatisticsAsync(request);
        return await modStatsTask;
    }

    public void WritePlayFabIDToFile(string id)
    {
        PlayFabId = id;
        File.AppendAllTextAsync("RandomlyCreatedIDs.txt", id+"\n");
    }
}
