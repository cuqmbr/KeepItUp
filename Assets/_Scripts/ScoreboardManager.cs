using System.Collections.Generic;
using System.Linq;
using DatabaseModels.DataTransferObjets;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour
{
    [SerializeField] private GameObject _scoreboardRecordPrefab;

    [SerializeField] private Color _color1;
    [SerializeField] private Color _color2;

    [SerializeField] private GameObject _scrollViewContent;

    public void SpawnScoreboardRecords()
    {
        var filteredScoreboardRecords = GetFilteredScoreboardRecords();
        
        var rectTransform = _scrollViewContent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(0, filteredScoreboardRecords.Length * 100);

        for (int i = 0; i < _scrollViewContent.transform.childCount; i++)
        {
            Destroy(_scrollViewContent.transform.GetChild(i));
        }
        
        for (int i = 0; i < filteredScoreboardRecords.Length; i++)
        {
            Instantiate(_scoreboardRecordPrefab, new Vector2(225, -50 - 100 * i), Quaternion.identity, _scrollViewContent.transform);
        }
        
        ScoreboardRecordDto[] GetFilteredScoreboardRecords()
        {
            var localRecords = SessionStore.ScoreboardRecords.ToList();

            var topUserRecord = localRecords.LastOrDefault(r => r.Score >= SessionStore.HighScore);
            var topUserPlace = localRecords.IndexOf(topUserRecord) + 1;

            var currentUserRecord = localRecords.FirstOrDefault(r => r.User.Username == SessionStore.UserData.Username);
            var currentUserPlace = localRecords.IndexOf(currentUserRecord) != -1 ? localRecords.IndexOf(currentUserRecord) + 1 : -1;

            var startIndex = localRecords.Count - topUserPlace < localRecords.Count - 5 ? topUserPlace - 5 : 0;
            int count = localRecords.Count - currentUserPlace < localRecords.Count && localRecords.Count - currentUserPlace > 3 ? currentUserPlace + 3 - startIndex : currentUserPlace - startIndex;

            if (currentUserPlace == -1)
            {
                count = localRecords.Count - startIndex;
            }

            return localRecords.GetRange(startIndex, count).ToArray();
        }
    }
}