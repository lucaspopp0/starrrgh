using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    [SerializeField] private RectTransform textTransform;
    private float _height;
    private float _offset;

    private void Awake() {
        var rt = (RectTransform) transform;
        _offset = rt.rect.height * transform.localScale.y / 12f;
    }

    public void OnPointerDown(PointerEventData eventData) {
        textTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, _offset, textTransform.rect.height);
    }

    public void OnPointerUp(PointerEventData eventData) {
        textTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, textTransform.rect.height);
    }

}
