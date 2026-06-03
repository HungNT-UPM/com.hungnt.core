using UnityEditor;

namespace HungNT.Editor
{
    public static class SceneMenuItems
    {
        [MenuItem("HungNT/Scene Tool/Open First Scene", false, 0)]
        private static void OpenFirstScene()
        {
            if (!BuildSceneEditorUtility.OpenFirstEnabledBuildScene(false))
            {
                EditorUtility.DisplayDialog(
                    "Open First Scene",
                    "No enabled scene found in Build Settings.",
                    "OK");
            }
        }

        [MenuItem("HungNT/Scene Tool/Open First Scene", true, 0)]
        private static bool ValidateOpenFirstScene()
        {
            return BuildSceneEditorUtility.TryGetFirstEnabledBuildScene(out _, out _);
        }
    }
}
