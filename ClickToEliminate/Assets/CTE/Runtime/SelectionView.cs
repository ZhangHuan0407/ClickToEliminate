using Tween;
using UnityEngine;
using UnityEngine.UI;

namespace CTE
{
    /// <summary>
    /// 展示所有的章节，分页展示左右切换
    /// </summary>
    public class SelectionView : MonoBehaviour
    {
        /* field */
        public Section SectionA;
        public Section SectionB;
        [Range(1, 100)]
        public int NowSection;
        private int m_SectionMaxValue;

        [Tooltip("每个Section视觉宽度")]
        public float Width;

        public Button LeftButton;
        public Button RightButton;

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
            NowSection = 1;
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

            LeftButton.gameObject.SetActive(NowSection > 1);
            RightButton.gameObject.SetActive(NowSection < m_SectionMaxValue);
        }

        /* func */
        public void OnClickLeftButton()
        {
            if (NowSection <= 1)
                return;

            Section temp = SectionB;
            SectionB = SectionA;
            SectionA = temp;

            NowSection--;
            SectionA.SectionIndex = NowSection;
            LeftButton.gameObject.SetActive(NowSection > 1);
            RightButton.gameObject.SetActive(NowSection < m_SectionMaxValue);
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
        }
        public void OnClickRightButton()
        {
            if (NowSection >= m_SectionMaxValue)
                return;

            Section temp = SectionB;
            SectionB = SectionA;
            SectionA = temp;

            NowSection++;
            SectionA.SectionIndex = NowSection;
            LeftButton.gameObject.SetActive(NowSection > 1);
            RightButton.gameObject.SetActive(NowSection < m_SectionMaxValue);
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
        }
    }
}