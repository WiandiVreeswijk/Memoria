using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Scrollbar;

/// <summary>Adds support for auto-scrolling to selected value</summary>
/// <remarks>Place this after actual Dropdown Component, depends on GetComponents order logic</remarks>
public class AutoscrollDropdown : MonoBehaviour, IPointerClickHandler {
    private TMP_Dropdown target = null!;

    void Start() {
        target = GetComponent<TMP_Dropdown>();
    }
    public void OnPointerClick(PointerEventData _) {
        if (!target.GetComponentInChildren<Scrollbar>() || !target.IsActive() || !target.IsInteractable()) {
            return;
        }

        var scrollbar = GetComponentInChildren<ScrollRect>()?.verticalScrollbar;
        if (target.options.Count > 1 && scrollbar != null) {
            var valuePosition = (float)target.value / (target.options.Count - 1);
            var value = scrollbar.direction == Direction.TopToBottom ? valuePosition : 1f - valuePosition;
            scrollbar.value = Mathf.Max(.001f, value);
        }
    }
}
