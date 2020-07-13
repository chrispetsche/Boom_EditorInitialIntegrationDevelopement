using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UIAddons
{
    [ExecuteInEditMode]
    public class TabGroup : MonoBehaviour
    {
        [SerializeField] private List<TabButton> tabButtons = new List<TabButton>();
        [SerializeField] private List<GameObject> panels = new List<GameObject>();
        public UnityEvent OnTabChanged = new UnityEvent();
        public Sprite tabIdleSprite, tabHoverSprite, tabActiveSprite;
        public Color tabIdle = Color.white, tabHover = Color.blue, tabActive = Color.white;
        public int SelectedTab { get; private set; }
        public int PreviousSeletedTab { get; private set; }
        private void OnValidate()
        {
            if (name != "Tab Group") name = "Tab Group";
            if (transform.parent.name != "Tabbed Page Ui")
            {
                GameObject parent = new GameObject("Tabbed Page Ui");
                parent.AddComponent<RectTransform>();
                parent.AddComponent<VerticalLayoutGroup>();
                parent.transform.parent = transform.parent;
                transform.parent = parent.transform;

            }
            UpdateGroup();
        }
        public void UpdateGroup()
        {
            Transform pageParent = transform.parent.Find("Page Group");
            if (pageParent == null)
            {
                pageParent = new GameObject("Page Group").AddComponent<RectTransform>().transform;

                pageParent.parent = transform.parent;
            }
            List<TabButton> currenttabs = new List<TabButton>();
            foreach (Transform p in transform)
            {
                currenttabs.Add(p.GetComponent<TabButton>());

            }
            tabButtons.Clear();
            tabButtons = currenttabs;
            List<GameObject> currentpanels = new List<GameObject>();
            foreach (Transform p in pageParent)
            {
                currentpanels.Add(p.gameObject);

            }
            for (int i = currentpanels.Count; i < tabButtons.Count + 1; i++)
            {
                GameObject panel = new GameObject(i == 0 ? "Default Panel" : "Tab Panel " + i);
                panel.AddComponent<RectTransform>();
                panel.transform.parent = pageParent;
                panels.Add(panel);
            }
            panels.Clear();
            panels = currentpanels;
        }
        private void Start()
        {
            ActivatePanel();
        }
        public void Subscribe(TabButton button)
        {

            tabButtons.Add(button);
        }
        public void OnTabEnter(TabButton button)
        {

            ResetTabs();
            button.Background.color = tabHover;
            if (tabHoverSprite != null)
            {
                button.Background.sprite = tabHoverSprite;
            }
        }
        public void OnTabExit(TabButton button)
        {

            ResetTabs();

        }
        public bool OnTabSelected(TabButton button)
        {
            int index = tabButtons.IndexOf(button);//+1 because defualt panel
            if (SelectedTab >= 0) tabButtons[SelectedTab].Deselect();
            bool selected = index != SelectedTab;
            PreviousSeletedTab = SelectedTab;
            if (selected)
            {

                SelectedTab = index;
                tabButtons[index].Select();
            }
            else
            {
                SelectedTab = -1;
            }
            ResetTabs();
            button.Background.color = tabActive;
            if (tabActiveSprite != null)
            {
                button.Background.sprite = tabActiveSprite;
            }
            OnTabChanged.Invoke();
            ActivatePanel();

            return selected;
        }
        public void ResetTabs()
        {
            for (int i = 0; i < tabButtons.Count; i++)
            {
                if (i == SelectedTab) continue;
                tabButtons[i].Background.color = tabIdle;
                if (tabIdleSprite != null)
                {
                    tabButtons[i].Background.sprite = tabIdleSprite;
                }
            }
        }
        public void ActivatePanel()
        {
            for (int i = 0; i < panels.Count; i++)
            {
                panels[i].SetActive(i == SelectedTab + 1);
            }
        }
    }

}
