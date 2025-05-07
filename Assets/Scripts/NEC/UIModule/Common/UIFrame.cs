using UnityEngine;

namespace NEC.UIModule.Common
{
    public class UIFrame : UIBehaviour
    {
        [SerializeField] private Camera uiCamera;
        [SerializeField] private Canvas uiCanvas;
        [SerializeField] private RectTransform uiCanvasRT;
        
        public Camera UICamera => uiCamera;
        public Canvas UICanvas => uiCanvas;
        public RectTransform UICanvasRT => uiCanvasRT;

        public Vector3 ViewportToWorldPoint(Vector2 viewportPoint)
        {
            var worldPoint = uiCamera.ViewportToWorldPoint(viewportPoint);
            worldPoint.z = uiCanvasRT.transform.position.z;
            return worldPoint;
        }

        public Vector2 WorldToViewportPoint(Vector3 worldPoint)
        {
            return uiCamera.WorldToViewportPoint(worldPoint);
        }

        public Vector2 ViewportToCanvasPoint(Vector2 viewportPoint)
        {
            return new Vector2(uiCanvasRT.rect.width * viewportPoint.x, uiCanvasRT.rect.height * viewportPoint.y);
        }

        public Vector2 CanvasToViewportPoint(Vector2 canvasPoint)
        {
            return new Vector2(canvasPoint.x / uiCanvasRT.rect.width, canvasPoint.y / uiCanvasRT.rect.height);
        }
    }
}