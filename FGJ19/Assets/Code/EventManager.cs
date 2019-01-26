using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public static EventManager _instance;

    private void Awake() {
        if(_instance == null) {
            _instance = this;
        } else if(_instance != this) {
            Destroy(gameObject);
            return;
        }
    }

    public delegate void EmptyVoid();
    public delegate void GameObjectVoid(GameObject go);

    public event EmptyVoid OnStartGame;
    public void BroadcastStartGame() {
        OnStartGame?.Invoke();
    }
    
    public event GameObjectVoid OnPlacmentModeEnabled;
    public void BroadcastPlacementModeEnabled(GameObject objectToPlace) {
        OnPlacmentModeEnabled?.Invoke(objectToPlace);
    }

    public event EmptyVoid OnPlacementModeDisabled;
    public void BroadcastPlacementModeDisabled() {
        OnPlacementModeDisabled?.Invoke();
    }
}
