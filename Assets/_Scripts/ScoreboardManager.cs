using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseModels.DataTransferObjets;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;

    public async Task SpawnScoreboardRecords()
    {
        if (SessionStore.UserData == null)
        {
            _uiManager._scoreboardLoginTip.SetActive(true);
            return;
        }
        
        _uiManager._scoreboardLoadingScreen.SetActive(true);
        _uiManager.DestroyAllScoreboardRecords();
        var filteredScoreboard = await GetFilteredScoreboard();
        _uiManager.InstantiateScoreboardRecords(filteredScoreboard.records, filteredScoreboard.firstRecordIndex);
        _uiManager._scoreboardLoadingScreen.SetActive(false);
        
        async Task<(ScoreboardRecordDto[] records, int firstRecordIndex)> GetFilteredScoreboard()
        {
            int spareCount = 5;
            
            var localRecords = await HttpClient.Get<List<ScoreboardRecordDto>>($"{SessionStore.ApiUrl}/scoreboard");
            var currentUserRecord = localRecords.First(r => r.User.Username == SessionStore.UserData.Username);

            var firstRecordNum = Mathf.Clamp(localRecords.IndexOf(currentUserRecord) - spareCount, 0, localRecords.Count);
            
            var filteredRecords = localRecords
                .SkipWhile(r => Math.Abs(localRecords.IndexOf(r) - localRecords.IndexOf(currentUserRecord)) >= spareCount)
                .TakeWhile(r => Math.Abs(localRecords.IndexOf(r) - localRecords.IndexOf(currentUserRecord)) <= spareCount)
                .ToArray();

            return (filteredRecords, firstRecordNum);
        }
    }
}