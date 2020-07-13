using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UIAddons
{
    [RequireComponent(typeof(Image))][ExecuteInEditMode]
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private TabGroup tabGroup;
        public Image Background;
        public UnityEvent OnSelected = new UnityEvent(), OnDeselected = new UnityEvent();
        public void OnPointerClick(PointerEventData eventData)
        {
            tabGroup.OnTabSelected(this);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            tabGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tabGroup.OnTabExit(this);
        }
        
        public void Create()
        {
            tabGroup = transform.parent.GetComponent<TabGroup>();
            Background = GetComponent<Image>();
            tabGroup.Subscribe(this);
        }
        public void Select()
        {
            OnSelected.Invoke();
        }
        public void Deselect()
        {
            OnDeselected.Invoke();
        }
    }

}
