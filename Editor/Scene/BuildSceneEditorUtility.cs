using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace HungNT.Editor
{
    public static class BuildSceneEditorUtility
    {
        public static bool TryGetFirstEnabledBuildScene(out string path, out string sceneName)
        {
            path = null;
            sceneName = null;

            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled)
                    continue;

                path = scene.path;
                sceneName = Path.GetFileNameWithoutExtension(path);
                return true;
            }

            return false;
        }

        public static void OpenScene(string scenePath)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(scenePath);
        }

        public static bool OpenFirstEnabledBuildScene(bool enterPlayMode)
        {
            if (!TryGetFirstEnabledBuildScene(out var path, out _))
                return false;

            OpenScene(path);

            if (enterPlayMode)
                EditorApplication.isPlaying = true;

            return true;
        }
    }
}
