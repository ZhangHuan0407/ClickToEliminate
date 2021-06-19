using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CTE.Editor
{
    /// <summary>
    /// 修改本地存档
    /// </summary>
    public class PlayerPrefsWindow : EditorWindow
    {
        /* const */
        private static readonly GUIContent KeyLabel = new GUIContent("键", "需要完整键名称");

        /* field */
        private string m_Key;
        private int m_IntValue;
        private float m_FloatValue;
        private string m_StringContent;

        /* ctor */
        [MenuItem("Window/" + nameof(PlayerPrefsWindow))]
        public static void GetDeletePrefabPrefsWindow()
        {
            PlayerPrefsWindow window = GetWindow<PlayerPrefsWindow>();
            window.titleContent = new GUIContent(nameof(PlayerPrefsWindow));
            window.minSize = new Vector2(300f, 200f);
        }

        /* func */
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(KeyLabel, GUILayout.Width(50f));
            m_Key = EditorGUILayout.TextField(m_Key);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Delete", GUILayout.Width(75f)))
            {
                EditorApplication.delayCall += () =>
                {
                    PlayerPrefs.DeleteKey(m_Key);
                };
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("  int", GUILayout.Width(50f));
            m_IntValue = EditorGUILayout.IntField(m_IntValue);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Get int", GUILayout.Width(75f)))
            {
                EditorApplication.delayCall += () =>
                {
                    m_IntValue = 0;
                    m_FloatValue = 0f;
                    m_StringContent = string.Empty;
                    m_IntValue = PlayerPrefs.GetInt(m_Key);
                };
            }
            if (GUILayout.Button("Set int", GUILayout.Width(75f)))
            {
                EditorApplication.delayCall += () =>
                {
                    PlayerPrefs.SetInt(m_Key, m_IntValue);
                };
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("  float", GUILayout.Width(50f));
            m_FloatValue = EditorGUILayout.FloatField(m_FloatValue);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Get float", GUILayout.Width(75f)))
            {
                EditorApplication.delayCall += () =>
                {
                    m_IntValue = 0;
                    m_FloatValue = 0f;
                    m_StringContent = string.Empty;
                    m_FloatValue = PlayerPrefs.GetFloat(m_Key);
                };
            }
            if (GUILayout.Button("Set float", GUILayout.Width(75f)))
            {
                EditorApplication.delayCall += () =>
                {
                    PlayerPrefs.SetFloat(m_Key, m_FloatValue);
                };
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("  string", GUILayout.Width(50f));
            m_StringContent = EditorGUILayout.TextArea(m_StringContent, GUILayout.MinWidth(75f));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Get string", GUILayout.Width(75f)))
            {
                EditorApplication.delayCall += () =>
                {
                    m_IntValue = 0;
                    m_FloatValue = 0f;
                    m_StringContent = string.Empty;
                    m_StringContent = PlayerPrefs.GetString(m_StringContent);
                };
            }
            if (GUILayout.Button("Set string", GUILayout.Width(75f)))
            {
                EditorApplication.delayCall += () =>
                {
                    PlayerPrefs.SetString(m_Key, m_StringContent);
                };
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Delete All", GUILayout.Width(75f)))
            {
                EditorApplication.delayCall += () =>
                {
                    m_IntValue = 0;
                    m_FloatValue = 0f;
                    m_StringContent = string.Empty;
                    m_Key = string.Empty;
                    PlayerPrefs.DeleteAll();
                };
            }
            GUILayout.EndHorizontal();
        }
    }
}