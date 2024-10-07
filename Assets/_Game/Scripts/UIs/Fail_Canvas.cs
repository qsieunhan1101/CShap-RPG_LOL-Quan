using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fail_Canvas : UICanvas
{
    [SerializeField] private Button btnNext;


    private void Start()
    {
        btnNext.onClick.AddListener(OnNext);
    }

    private void OnNext()
    {
        GameManager.Instance.ChangeState(GameState.MainMenu);
    }
}
