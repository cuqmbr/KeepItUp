using System;
using System.Linq;
using DatabaseModels.DataTransferObjets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;

    public void SpawnScoreboardRecords()
    {
        if (SessionStore.UserData == null)
        {
            // Login to display online scoreboard
            
            return;
        }
        
        // TODO: POST new HighScore to the database
        
        var filteredScoreboardRecords = GetFilteredScoreboardRecords();
        
        _uiManager.DestroyAllScoreboardRecords();
        _uiManager.InstantiateScoreboardRecords(filteredScoreboardRecords);
        
        ScoreboardRecordDto[] GetFilteredScoreboardRecords()
        {
            var localRecords = SessionStore.ScoreboardRecords.ToList();

            var topUserRecord = localRecords.LastOrDefault(r => r.Score >= SessionStore.HighScore);
            var topUserPlace = localRecords.IndexOf(topUserRecord) + 1;

            var currentUserRecord = localRecords.FirstOrDefault(r => r.User.Username == SessionStore.UserData.Username);
            var currentUserPlace = localRecords.IndexOf(currentUserRecord) != -1 ? localRecords.IndexOf(currentUserRecord) + 1 : -1;

            var startIndex = localRecords.Count - topUserPlace < localRecords.Count - 6 ? topUserPlace - 6 : 0;
            int count = localRecords.Count - currentUserPlace < localRecords.Count && localRecords.Count - currentUserPlace > 6 ? currentUserPlace + 6 - startIndex : currentUserPlace - startIndex;

            if (currentUserPlace == -1)
            {
                count = localRecords.Count - startIndex;
            }

            return localRecords.GetRange(startIndex, count).ToArray();
        }
    }
}