using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameStateController : MonoBehaviour
{
    [Tooltip("Selected state will be applied when the game starts or on the button press")]
    public GameState ChangeToState;

    private void Awake()
    {
        // Change game state to selected in inspector state when the game starts
        GameStateManager.Instance.ChangeState(ChangeToState);

        PlayerEvents.OnBallTouched += () => GameStateManager.Instance.ChangeState(GameState.Game);
    }

    private void OnApplicationQuit()
    {
        // Change game state back to entry state when exiting playing mode
        GameStateManager.Instance.ChangeState(GameState.Enter);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameStateController))]
class GameStateControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var gameStateController = (GameStateController)target;
        if (gameStateController == null) return;
        
        // Custom button to change game state from inspector during runtime
        if (GUILayout.Button("Change State"))
        {
            if (Application.isPlaying) GameStateManager.Instance.ChangeState(gameStateController.ChangeToState);
        }
    }
}
#endif