using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour {

    void Start() {
        StartCoroutine("StartGameAtFrameEnd");
    }

    IEnumerator StartGameAtFrameEnd() {
        yield return new WaitForEndOfFrame();
        StartGame();
    }

    private void StartGame() {
        Debug.Log("GameStarter.StartGame");
        EventManager em = EventManager._instance;
        em.BroadcastStartGame();
        em.BroadcastPlacementModeDisabled();
    }
}
