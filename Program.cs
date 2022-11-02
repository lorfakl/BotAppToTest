using PlayFab;
using PlayFab.ClientModels;
using DeviceId;
using PlayFab.Internal;
using PlayFab.CloudScriptModels;

// See https://aka.ms/new-console-template for more
// information
Console.WriteLine("Hello, World!");





public class BotTesting 
{
    public async Task<PlayFabResult<LoginResult>> RandomLogin()
    {
        string deviceID = new DeviceIdBuilder().AddMachineName().ToString();
        var request = new LoginWithCustomIDRequest { CustomId = deviceID, CreateAccount = true };
        Task<PlayFabResult<LoginResult>> loginTask = PlayFabClientAPI.LoginWithCustomIDAsync(request);
        await loginTask;
        return loginTask.Result;
    }

    public async Task<PlayFabResult<ExecuteFunctionResult>> ModifySP()
    {

    }
    public static void CaptureException(Exception ex)
    {
        string err = ex.Message + "Inner Exception:  " + ex.InnerException + " Stack Trace:" + ex.StackTrace + " Source:" + ex.Source;
    }

    public static void CapturePlayFabError(PlayFabError error)
    {
        string fullErrorDetails = "Error in PlayFab API: " + error.RequestId + "\n" +
            "Error: " + error.Error.ToString() + "\n" + "Error Message: " + error.ErrorMessage
            + "\n" + "Error Details: " + error.ErrorDetails.ToString() + "\n" + error.GenerateErrorReport();
    }
}
