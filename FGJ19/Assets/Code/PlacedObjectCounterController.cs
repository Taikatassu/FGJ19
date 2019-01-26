using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlacedObjectCounterController : MonoBehaviour {

    private EventManager em;
    private TextMeshProUGUI text;

    private void OnEnable() {
        text = GetComponent<TextMeshProUGUI>();
        em = EventManager._instance;
        em.OnStartGame += OnStartGame;
        em.OnPlacedObjectCountChanged += OnPlacedObjectCountChanged;
    }

    private void OnDisable() {
        em.OnStartGame -= OnStartGame;
        em.OnPlacedObjectCountChanged -= OnPlacedObjectCountChanged;
    }

    private void OnStartGame() {
        text.text = "0";
    }

    private void OnPlacedObjectCountChanged(int newCount) {
        text.text = newCount.ToString();
    }
}
