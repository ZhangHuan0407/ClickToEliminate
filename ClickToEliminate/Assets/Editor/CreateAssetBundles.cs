using UnityEditor;
using System.IO;

namespace CTE.Editor
{
    public static class CreateAssetBundles
    {
        /* func */
        [MenuItem("AssetBundle/CreateAB")]
        public static void CreateAB()
        {
            Directory.CreateDirectory("AssetBundles");
            BuildPipeline.BuildAssetBundles("AssetBundles", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
        }
    }
}