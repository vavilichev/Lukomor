using System;
using System.Globalization;
using Lukomor.Presentation;
using UnityEditor;
using UnityEngine;

public class TagsGameExampleWindow  : EditorWindow
{
    private const string SCENE_BOOT = "LukomorExample_Bootstrap";
    private const string SCENE_GAME = "LukomorExample_Gameplay";
    private const string KEY_DATE = "LUKOMOR_EXAMPLE_DATE";
    private const string KEY_NEVER_ASK = "LUKOMOR_EXAMPLE_NEVER_ASK";
    
    [MenuItem("Lukomore/Example Rules")]
    static void Init()
    {
        TagsGameExampleWindow window = (TagsGameExampleWindow)GetWindow(typeof(TagsGameExampleWindow));
        window.minSize = new Vector2(640f, 260);
        window.maxSize = window.minSize;
        window.Show();
    }
    
    [MenuItem("Lukomore/Clear")]
    static void Clear()
    {
        EditorPrefs.DeleteKey(KEY_NEVER_ASK);
        EditorPrefs.DeleteKey(KEY_DATE);
    }

    void OnGUI()
    {
       DrawTitle();
       DrawWarning();
       DrawWarningText();
       DrawNeverAsButton();
    }

    private void DrawTitle()
    {
        GUIStyle styleWarning = new GUIStyle();
        styleWarning.fontSize = 40;
        styleWarning.alignment = TextAnchor.MiddleCenter;
        styleWarning.fontStyle = FontStyle.Bold;
        styleWarning.normal.textColor = new Color(0.4f, 0.58f, 0.61f);
        GUILayout.Label("Lukomor Architecture Example", styleWarning);
    }

    private void DrawWarning()
    {
        GUIStyle styleWarning = new GUIStyle();
        styleWarning.fontSize = 30;
        styleWarning.alignment = TextAnchor.MiddleCenter;
        styleWarning.fontStyle = FontStyle.Bold;
        GUILayout.Label("WARNING!\n", styleWarning);
    }

    private void DrawWarningText()
    {
        GUIStyle styleCommon = new GUIStyle();
        styleCommon.wordWrap = true;
        styleCommon.alignment = TextAnchor.MiddleCenter;

        var text = $"To make example work properly you must add two scenes in the BuildSettings: {SCENE_BOOT} and {SCENE_GAME}.\n\n" +
                   "Also you must copy prefab [INTERFACE] from LukomorArchitecture/Lukomor/Prefabs to Resources folder.\n(create it if needed)\n\n";
        
        GUILayout.TextArea(text, styleCommon);
    }

    private void DrawNeverAsButton()
    {
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        var neverAskAgain = GUILayout.Button("Never Ask Again", buttonStyle); 

        if (neverAskAgain)
        {
            EditorPrefs.SetBool(KEY_NEVER_ASK, true); 
            
            TagsGameExampleWindow window = (TagsGameExampleWindow)GetWindow(typeof(TagsGameExampleWindow));
            window.Close(); 
        }
    }

    [InitializeOnLoadMethod]
    static void ValidateExample()
    {
        var neverAskAgain = EditorPrefs.GetBool(KEY_NEVER_ASK, false);

        if (!neverAskAgain)
        { 
            var lastWindowShowDateStr = EditorPrefs.GetString(KEY_DATE, new DateTime(2000, 1, 1).ToString(CultureInfo.InvariantCulture));
            var lastWindowShowData = DateTime.Parse(lastWindowShowDateStr);
            var hoursPassed = (DateTime.Now - lastWindowShowData).TotalHours;
            var needToShowWindow = false;

            if (hoursPassed > 24)
            {
                var buildScenes = EditorBuildSettings.scenes;
                var hasSceneBoot = false;
                var hasSceneGameplay = false;

                foreach (var buildScene in buildScenes)
                {
                    if (buildScene.path.Contains(SCENE_BOOT))
                    {
                        hasSceneBoot = true;
                        continue;
                    }

                    if (buildScene.path.Contains(SCENE_GAME))
                    {
                        hasSceneGameplay = true;
                    }
                }

                var interfacePrefab = Resources.Load<UserInterface>("[INTERFACE]");

                needToShowWindow = !hasSceneBoot || !hasSceneGameplay || interfacePrefab == null;

                if (needToShowWindow)
                {
                    EditorPrefs.SetString(KEY_DATE, DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    
                    Init();
                }
            }
        }
    }
}