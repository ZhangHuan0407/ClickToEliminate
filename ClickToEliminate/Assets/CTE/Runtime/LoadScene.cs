using Database;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Tween;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace CTE
{
    public class LoadScene : MonoBehaviour
    {
        /* field */
        public TweenService TweenService;

        public string[] AssetBundleUri;

        /* ctor */
        private IEnumerator Start()
        {
            TweenService.enabled = true;
            DontDestroyOnLoad(TweenService.gameObject);

#if UseAssetBundle
            foreach (string uri in AssetBundleUri)
            {
                UnityWebRequest unityWebRequest = UnityWebRequest.Get(uri);
                yield return unityWebRequest.SendWebRequest();
                GameData.LoadAssetBundle(unityWebRequest.downloadHandler.data);
                if (!unityWebRequest.isDone)
                    Debug.LogError($"isNetworkError : {unityWebRequest.isNetworkError}");
                unityWebRequest.Dispose();
            }

            AssetBundle assetBundle = GameData.AllAssetBundles["cte/config.assetbundle"];
            TextAsset configTextAsset = assetBundle.LoadAsset<TextAsset>($"{nameof(GameData.SectionConfig)}");
            TextAsset mapTextAsset = assetBundle.LoadAsset<TextAsset>($"{nameof(GameData.MapConfig)}"); 
#else
            TextAsset configTextAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/CTE/Config/{nameof(GameData.SectionConfig)}.json");
            TextAsset mapTextAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/CTE/Config/{nameof(GameData.MapConfig)}.json");
#endif
            GameData.SectionConfig = Table<int, SectionData>.LoadTableFromJson(nameof(GameData.SectionConfig), configTextAsset.text);
            GameData.MapConfig = Table<int, MapData>.LoadTableFromJson(nameof(GameData.MapConfig), mapTextAsset.text);
            GameData.LoadSaving();

            SceneManager.LoadScene("SelectionScene", LoadSceneMode.Single);
            yield break;
        }
    }
}