using UnityEditor;

namespace HungNT.Editor
{
    public static class SceneMenuItems
    {
        [MenuItem("HungNT/Scene/Open First Scene", false)]
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

        [MenuItem("HungNT/Scene/Open First Scene", true)]
        private static bool ValidateOpenFirstScene()
        {
            return BuildSceneEditorUtility.TryGetFirstEnabledBuildScene(out _, out _);
        }
    }
}
