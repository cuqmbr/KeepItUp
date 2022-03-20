#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

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