using Database;
using Tween;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CTE
{
    public class LoadScene : MonoBehaviour
    {
        /* field */
        public TweenService TweenService;

        /* ctor */
        private void Start()
        {
            TweenService.enabled = true;
            DontDestroyOnLoad(TweenService.gameObject);

            // 只是用作学习的项目，没做热更新配置
            TextAsset textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/CTE/Config/{nameof(GameData.SectionConfig)}.json");
            GameData.SectionConfig = Table<int, SectionData>.LoadTableFromJson(nameof(GameData.SectionConfig), textAsset.text);
            textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/CTE/Config/{nameof(GameData.MapConfig)}.json");
            GameData.MapConfig = Table<int, SectionData>.LoadTableFromJson(nameof(GameData.MapConfig), textAsset.text);

            SceneManager.LoadScene("SelectionScene", LoadSceneMode.Single);
        }
    }
}