using System;
using Tween;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CTE
{
    /// <summary>
    /// 展示所有的章节，分页展示左右切换
    /// </summary>
    public class SectionView : MonoBehaviour
    {
        /* field */
        public Section SectionA;
        public Section SectionB;
        public int NowSection;
        private int m_SectionMaxValue;

        [Tooltip("每个Section视觉宽度")]
        public float Width;

        public Button LeftButton;
        public Button RightButton;

        public bool HaveSelectLevel;

        /* inter */
        
        /* ctor */
        public void Reset()
        {
            Section[] sections = GetComponentsInChildren<Section>();
            if (sections.Length != 2)
                Debug.LogWarning($"sections.Length != 2, {sections.Length}");
            else
            {
                SectionA = sections[0];
                SectionB = sections[1];
            }
            Width = 1280;
        }

        private void Awake()
        {
            NowSection = 0;
            m_SectionMaxValue = GameData.SectionConfig.Count;
        }
        private void Start()
        {
            SectionA.gameObject.SetActive(true);
            SectionB.gameObject.SetActive(true);
            SectionB.transform.position = new Vector3(-100, 0, 0);
            SectionA.SectionIndex = NowSection;
            (SectionA.transform as RectTransform).anchoredPosition = Vector2.zero;
            SectionA.Refresh();

            LeftButton.gameObject.SetActive(NowSection > 0);
            RightButton.gameObject.SetActive(NowSection < m_SectionMaxValue - 1);
        }

        /* func */
        public void SelectLevel(int levelIndex)
        {
            if (HaveSelectLevel)
                return;

            HaveSelectLevel = true;
            LeftButton.interactable = false;
            RightButton.interactable = false;
            CTEGame.StartNewGame(levelIndex);
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
            SectionA.SetInteractable(false);
        }

        public void OnClickLeftButton()
        {
            if (NowSection < 0)
                return;

            Section temp = SectionB;
            SectionB = SectionA;
            SectionA = temp;

            NowSection--;
            SectionA.SectionIndex = NowSection;
            SectionA.Refresh();
            LeftButton.gameObject.SetActive(NowSection > 0);
            RightButton.gameObject.SetActive(NowSection < m_SectionMaxValue - 1);
            LeftButton.interactable = false;
            RightButton.interactable = false;
            (SectionA.transform as RectTransform)
                .DoAnchoredPosition(Vector2.zero, 0.5f, new Vector2(-Width, 0f))
                .DoIt()
                .OnComplete_Handle += () => 
                {
                    LeftButton.interactable = true;
                    RightButton.interactable = true;
                };
            (SectionB.transform as RectTransform)
                .DoAnchoredPosition(new Vector2(Width, 0f), 0.5f)
                .DoIt();
        }
        public void OnClickRightButton()
        {
            if (NowSection >= m_SectionMaxValue - 1)
                return;

            Section temp = SectionB;
            SectionB = SectionA;
            SectionA = temp;

            NowSection++;
            SectionA.SectionIndex = NowSection;
            SectionA.Refresh();
            LeftButton.gameObject.SetActive(NowSection > 0);
            RightButton.gameObject.SetActive(NowSection < m_SectionMaxValue - 1);
            LeftButton.interactable = false;
            RightButton.interactable = false;
            (SectionA.transform as RectTransform)
                .DoAnchoredPosition(Vector2.zero, 0.5f, new Vector2(Width, 0f))
                .DoIt()
                .OnComplete_Handle += () =>
                {
                    LeftButton.interactable = true;
                    RightButton.interactable = true;
                };
            (SectionB.transform as RectTransform)
                .DoAnchoredPosition(new Vector2(-Width, 0f), 0.5f)
                .DoIt();
        }
    }
}