using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public QuestionSpawner questionSpawner;

    public void OnStartButtonClicked()
    {
        questionSpawner.SpawnRandomQuestion();
    }
}

