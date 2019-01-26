using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager _instance;

    private void Awake() {
        if(_instance == null) {
            _instance = this;
        } else if(_instance != this) {
            Destroy(gameObject);
            return;
        }

        //DontDestroyOnLoad(gameObject);
    }

    public delegate void EmptyVoid();

    public event EmptyVoid OnStartGame;
    public void BroadcastStartGame() {
        Debug.Log("Broadcasting game start!");
        OnStartGame?.Invoke();
    }
}
