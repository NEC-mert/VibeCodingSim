using UnityEngine;

namespace NEC.UIModule.Common
{
    public class UIBehaviour : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
            AddListeners();
            OnShow();
        }

        public void Hide()
        {
            OnHide();
            RemoveListeners();
            gameObject.SetActive(false);
        }
        
        protected virtual void OnShow() { }

        protected virtual void OnHide() { }

        protected virtual void AddListeners() { }

        protected virtual void RemoveListeners() { }
    }
}
