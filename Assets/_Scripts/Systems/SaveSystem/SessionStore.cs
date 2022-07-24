using System.Linq;
using DatabaseModels.DataTransferObjets;

public static class SessionStore
{
   public static string ApiUrl { get; set; }
   
   public static UserData UserData { get; set; }
   public static int HighScore { get; set; }
   
   public static ScoreboardRecordDto[] ScoreboardRecords;

   public static async void Save()
   {
      await SaveSystem.SaveToJsonAsync("userData.json", UserData);
      await SaveSystem.SaveToBinaryAsync("HighScore.bin", HighScore);
   }

   public static async void Load()
   {
      UserData = await SaveSystem.LoadFromJsonAsync<UserData>("userData.json");
      HighScore = await SaveSystem.LoadFromBinaryAsync<int>("HighScore.bin");

      ScoreboardRecords = await HttpClient.Get<ScoreboardRecordDto[]>($"{ApiUrl}/scoreboard");

      if (UserData == null)
      {
         return;
      }
      
      int? dbHighScore = ScoreboardRecords?.FirstOrDefault(sbr => sbr.User.Username == UserData.Username)?.Score;
      if (dbHighScore != null && dbHighScore > HighScore)
      {
         HighScore = (int) dbHighScore;
      }
   }
}