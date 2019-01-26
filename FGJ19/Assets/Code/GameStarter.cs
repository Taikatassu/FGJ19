using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour {

    void Start() {
        EventManager em = EventManager._instance;
        em.BroadcastStartGame();
        em.BroadcastPlacementModeDisabled();
    }
}
