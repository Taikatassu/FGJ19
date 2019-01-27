using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public static EventManager _instance;

    private void Awake() {
        if (_instance == null) {
            _instance = this;
        } else if (_instance != this) {
            Destroy(gameObject);
            return;
        }
    }

    public delegate void EmptyVoid();
    public delegate void GameObjectVoid(GameObject go);
    public delegate void IntVoid(int value);

    public event EmptyVoid OnStartGame;
    public void BroadcastStartGame() {
        OnStartGame?.Invoke();
    }

    public event GameObjectVoid OnPlacementModeEnabled;
    public void BroadcastPlacementModeEnabled(GameObject objectToPlace) {
        OnPlacementModeEnabled?.Invoke(objectToPlace);
    }

    public event EmptyVoid OnPlacementModeDisabled;
    public void BroadcastPlacementModeDisabled() {
        OnPlacementModeDisabled?.Invoke();
    }

    public event GameObjectVoid OnHookCreated;
    public void BroadcastHookCreated(GameObject newHook) {
        OnHookCreated?.Invoke(newHook);
    }

    public event IntVoid OnPlacedObjectCountChanged;
    public void BroadcastPlacedObjectCountChanged(int newCount) {
        OnPlacedObjectCountChanged?.Invoke(newCount);
    }

    public event EmptyVoid OnGameOver;
    public void BroadcastGameOver() {
        OnGameOver?.Invoke();
    }

    public event EmptyVoid OnKeepGoing;
    public void BroadcastKeepGoing() {
        OnKeepGoing?.Invoke();
    }

}
