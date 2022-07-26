using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseModels.DataTransferObjets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;

    public async void SpawnScoreboardRecords()
    {
        if (SessionStore.UserData == null)
        {
            // Login to display online scoreboard
            
            return;
        }
        
        var filteredScoreboardRecords = await GetFilteredScoreboardRecords();
        
        _uiManager.DestroyAllScoreboardRecords();
        _uiManager.InstantiateScoreboardRecords(filteredScoreboardRecords);
        
        async Task<ScoreboardRecordDto[]> GetFilteredScoreboardRecords()
        {
            var localRecords = await HttpClient.Get<List<ScoreboardRecordDto>>($"{SessionStore.ApiUrl}/scoreboard");
            var currentUserRecord = localRecords.First(r => r.User.Username == SessionStore.UserData.Username);

            var filteredRecords = localRecords
                .SkipWhile(r => Math.Abs(localRecords.IndexOf(r) - localRecords.IndexOf(currentUserRecord)) >= 5)
                .TakeWhile(r => Math.Abs(localRecords.IndexOf(r) - localRecords.IndexOf(currentUserRecord)) <= 5)
                .ToArray();

            return filteredRecords.ToArray();
        }
    }
}