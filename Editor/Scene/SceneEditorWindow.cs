using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace HungNT.Editor
{
    public sealed class SceneEditorWindow : OdinEditorWindow
    {
        [MenuItem("HungNT/Scene Tool/Scene Editor Window", false, 10)]
        private static void OpenWindow()
        {
            var window = GetWindow<SceneEditorWindow>();
            window.titleContent = new GUIContent("Scene Editor");
        }

        protected override void OnBeginDrawEditors()
        {
            base.OnBeginDrawEditors();
            DrawSceneActions();
            DrawBuildSceneList();
        }

        private static void DrawSceneActions()
        {
            GUILayout.Space(5);
            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("Quick Play Game", GUILayout.Height(30)))
            {
                if (!BuildSceneEditorUtility.OpenFirstEnabledBuildScene(true))
                {
                    EditorUtility.DisplayDialog(
                        "Quick Play Game",
                        "No enabled scene found in Build Settings.",
                        "OK");
                }
            }

            GUI.backgroundColor = Color.white;

            if (BuildSceneEditorUtility.TryGetFirstEnabledBuildScene(out _, out var sceneName))
            {
                if (GUILayout.Button($"Open First Scene: {sceneName}", GUILayout.Height(24)))
                    BuildSceneEditorUtility.OpenFirstEnabledBuildScene(false);
            }
            else
            {
                GUI.enabled = false;
                GUILayout.Button("Open First Scene: (none enabled)", GUILayout.Height(24));
                GUI.enabled = true;
            }

            GUILayout.Space(5);
        }

        private static void DrawBuildSceneList()
        {
            var buildScenes = EditorBuildSettings.scenes;
            var pathToBuildIndex = new Dictionary<string, int>();
            var buildIndexCounter = 0;

            for (var i = 0; i < buildScenes.Length; i++)
            {
                if (!buildScenes[i].enabled)
                    continue;

                pathToBuildIndex[buildScenes[i].path] = buildIndexCounter;
                buildIndexCounter++;
            }

            for (var i = 0; i < buildScenes.Length; i++)
            {
                var sceneSetting = buildScenes[i];
                var scenePath = sceneSetting.path;
                var sceneName = Path.GetFileNameWithoutExtension(scenePath);
                var isEnabled = sceneSetting.enabled;
                var displayName = isEnabled
                    ? $"{pathToBuildIndex[scenePath]}. {sceneName}"
                    : $"_ {sceneName}";

                SirenixEditorGUI.BeginHorizontalToolbar();
                var clicked = SirenixEditorGUI.MenuButton(i, displayName, true, null);

                GUILayout.Space(5);

                if (GUILayout.Button("Ping", GUILayout.Width(50)))
                {
                    var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                    if (sceneAsset != null)
                    {
                        EditorGUIUtility.PingObject(sceneAsset);
                        Selection.activeObject = sceneAsset;
                    }
                }

                GUILayout.Space(10);
                SirenixEditorGUI.EndHorizontalToolbar();

                if (clicked)
                    BuildSceneEditorUtility.OpenScene(scenePath);
            }
        }
    }
}
