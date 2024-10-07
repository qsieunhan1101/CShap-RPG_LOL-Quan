using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_Canvas : UICanvas
{

    [SerializeField] private Button btnPlay;

    private void Start()
    {
        btnPlay.onClick.AddListener(OnPlay);
    }
    private void OnPlay()
    {
        GameManager.Instance.ChangeState(GameState.GamePlay);
    }

}
