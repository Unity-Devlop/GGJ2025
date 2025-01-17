using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;

namespace Framework.Editor
{
    public class FrameworkEditor : OdinMenuEditorWindow
    {
        [MenuItem("Framework/Editor")]
        public static void OpenWindow()
        {
            var window = GetWindow<FrameworkEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }


        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Add("FMOD", new FMODEditor(), EditorIcons.Microphone);
            return tree;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}