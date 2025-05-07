using NEC.Common;
using NEC.UIModule.Common;
using UnityEngine;

namespace NEC.UIModule
{
    public class UIManager : AManager<UIManager>
    {
        [SerializeField] private UIFrame uiFrame;
        
        public UIFrame UIFrame => uiFrame;
    }
}
