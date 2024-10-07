using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class GamePlay_Canvas : UICanvas
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int score;


    private void Start()
    {
        score = 0;
        scoreText.text = $"Score: {score.ToString()}";

    }


    private void OnEnable()
    {
        Enemy.emenyDeadEvent += UpdateScore;
    }
    private void OnDisable()
    {
        Enemy.emenyDeadEvent -= UpdateScore;
    }


    private void UpdateScore()
    {
        score++;
        scoreText.text = $"Score: {score.ToString()}";
        if (score == 10)
        {
            StartCoroutine(VictoryEvent());
        }
    }
    private IEnumerator VictoryEvent()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.ChangeState(GameState.Victory);
    }
}
