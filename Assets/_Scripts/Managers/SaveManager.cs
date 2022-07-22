using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private string ApiUrl;
    
    private void Awake()
    {
        SessionStore.ApiUrl = ApiUrl;
        SessionStore.Load();
    }

    private void OnApplicationQuit()
    {
        SessionStore.Save();
    }
}
