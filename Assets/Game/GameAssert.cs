namespace Game
{
    public static class GameAssert
    {
        public static void EditorAssert(bool value, string message = "")
        {
            if (!value)
            {
                UnityEngine.Debug.LogError($"Assert Failed: {message}");
            }
        }
    }
}