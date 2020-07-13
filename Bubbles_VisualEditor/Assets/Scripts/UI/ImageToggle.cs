using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace UIAddons
{
    public class ImageToggle : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Image ForeImage, BackImage;
        [SerializeField] private Sprite ForeImageOn, ForeImageHover, ForeImageOff;
        [SerializeField] private Sprite BackImageOn, BackImageHover, BackImageOff;
        [SerializeField] private bool state = false;
        [SerializeField] private ImageToggleGroup group;
        public UnityEvent OnChange = new UnityEvent();
        private void OnEnable()
        {
            if(group!=null)
            {
                group.Callback.AddListener(SetFalse);
            }
        }
        private void OnDisable()
        {
            if (group != null)
            {
                group.Callback.RemoveListener(SetFalse);
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            SetImage();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (ForeImage != null&&ForeImageHover!=null)
            {
                ForeImage.sprite = ForeImageHover;
            }
            if (BackImage != null && BackImageHover != null)
            {
                BackImage.sprite = BackImageHover;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (group != null)
            {
                group.Callback.Invoke();
            }
            ChangeState();

        }
        public void ChangeState()
        {
            state = !state;
            SetImage();
            OnChange.Invoke();

        }
        public void SetFalse()
        {
            state = false;
            SetImage();


        }
        private void SetImage()
        {
            if (state)
            {
                if (ForeImage != null)
                {
                    ForeImage.sprite = ForeImageOn;
                }
                if (BackImage != null)
                {
                    BackImage.sprite = BackImageOn;
                }
            }
            else
            {
                if (ForeImage != null)
                {
                    ForeImage.sprite = ForeImageOff;
                }
                if (BackImage != null)
                {
                    BackImage.sprite = BackImageOff;
                }
            }
        }
    }
}