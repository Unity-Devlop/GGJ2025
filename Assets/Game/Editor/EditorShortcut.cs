using UnityEditor;
using UnityEditor.SceneManagement;

namespace Game.GameEditor
{
    public class EditorShortcut : UnityEditor.Editor
    {
        // Command + Shift + A 启动游戏 mac
        // Ctrl + Shift + A 启动游戏 windows
        [MenuItem("Framework/Launch GameEntry #_F2")]
        public static void PlayGameEntry()
        {
            EditorSceneManager.OpenScene("Assets/GameResources/Scenes/GameEntry.unity");
            EditorApplication.isPlaying = true;
        }
    }
}