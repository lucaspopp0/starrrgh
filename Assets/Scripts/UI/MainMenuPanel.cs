using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel: MonoBehaviour {

    public Button closeButton;
    
    public bool isOpen = false;
    public Action onOpen;
    public Action onClose;

    private bool _closeHandlerSet = false;

    public void Open() {
        gameObject.SetActive(true);
        
        if (!_closeHandlerSet) {
            _closeHandlerSet = true;
            closeButton.onClick.AddListener(Close);
        }
        
        isOpen = true;
        onOpen?.Invoke();
    }

    public void Close() {
        gameObject.SetActive(false);
        isOpen = false;
        onClose?.Invoke();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Close();
        }
    }

}
