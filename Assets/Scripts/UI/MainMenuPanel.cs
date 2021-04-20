using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel: MonoBehaviour {

    [SerializeField] private Button closeButton;
    
    public bool isOpen = false;
    public Action onOpen;
    public Action onClose;

    private void Awake() {
        closeButton.onClick.AddListener(Close);
    }

    public void Open() {
        gameObject.SetActive(true);
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
