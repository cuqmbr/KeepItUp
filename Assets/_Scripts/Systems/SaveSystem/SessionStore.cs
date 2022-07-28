using DatabaseModels.DataTransferObjets;
using DatabaseModels.Requests;
using DatabaseModels.Responses;

public static class SessionStore
{
   public static string ApiUrl { get; set; }
   
   public static string Jwt { get; set; }

   public static UserData UserData { get; set; }
   public static int HighScore { get; set; }

   public static async void Save()
   {
      await SaveSystem.SaveToJsonAsync("userData.json", UserData);
      await SaveSystem.SaveToBinaryAsync("HighScore.bin", HighScore);
   }

   public static async void Load()
   {
      UserData = await SaveSystem.LoadFromJsonAsync<UserData>("userData.json");
      HighScore = await SaveSystem.LoadFromBinaryAsync<int>("HighScore.bin");

      if (UserData == null)
      {
         return;
      }

      var authResponse = await HttpClient.Post<AuthenticationResponse>($"{ApiUrl}/auth/login", new AuthenticationRequest { Username = UserData.Username, Password = UserData.Password } );
      Jwt = authResponse.Token;

      var dbHighScore = await HttpClient.Get<ScoreboardRecordDto>($"{ApiUrl}/scoreboard/{UserData.Username}");
      if (dbHighScore?.Score != null && dbHighScore.Score > HighScore)
      {
         HighScore = dbHighScore.Score;
      }
   }
}