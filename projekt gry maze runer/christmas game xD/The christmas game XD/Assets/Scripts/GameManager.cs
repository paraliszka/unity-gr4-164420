using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro; 

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI endGameMessage;
        [SerializeField] private GameObject endGamePanel;

        private float timer;
        private float timeLimit = 120f;
        private bool isTimerRunning;

        private void Start()
        {
            timeLimit = GameSettings.TimeLimit;
            endGamePanel.SetActive(false);
        }

        private void Update()
        {
            if (isTimerRunning)
            {
                timer += Time.deltaTime;

                UpdateTimerText();

                if (timer >= timeLimit)
                {
                    isTimerRunning = false;
                    EndGame(false);
                }
            }
        }

        public void StartTimer()
        {
            timer = 0f;
            isTimerRunning = true;

            if (timerText == null)
            {
                var player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    timerText = player.GetComponentInChildren<TextMeshProUGUI>();
                }
            }
        }

        public void StopTimer()
        {
            isTimerRunning = false;
        }

        private void UpdateTimerText()
        {
            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(timer / 60);
                int seconds = Mathf.FloorToInt(timer % 60);
                timerText.text = $"{minutes:D2}:{seconds:D2}";
            }
        }

        public void EndGame(bool hasWon)
        {
            isTimerRunning = false;
            Time.timeScale = 0;
            endGamePanel.SetActive(true);

            if (hasWon)
            {
                endGameMessage.text = "You Win!";
            }
            else
            {
                endGameMessage.text = "You Lose!";
            }
        }

        public void ReturnToMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MenuScene"); 
        }
    }
}