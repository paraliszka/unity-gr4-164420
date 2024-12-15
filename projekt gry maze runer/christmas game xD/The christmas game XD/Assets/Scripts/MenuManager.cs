using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Platformer
{
    public static class GameSettings
    {
        public static float TimeLimit = 120f; 
        public static int MazeSize = 20; 
    }
         public class MenuManager : MonoBehaviour
    {
        [Header("Sliders")]
        [SerializeField] private Slider timeSlider;
        [SerializeField] private Slider mapSizeSlider;

        [Header("Text Fields")]
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text mapSizeText;

        [Header("Default Values")]
        public float defaultTimeLimit = 120f;
        public int defaultMazeSize = 20;

        private float timeLimit;
        private int mazeSize;

        void Start()
        {
            timeSlider.minValue = 60;
            timeSlider.maxValue = 600;
            timeSlider.value = defaultTimeLimit;
            timeLimit = defaultTimeLimit;

            mapSizeSlider.minValue = 5;
            mapSizeSlider.maxValue = 50;
            mapSizeSlider.value = defaultMazeSize;
            mazeSize = defaultMazeSize;

            UpdateTimeText();
            UpdateMapSizeText();

            timeSlider.onValueChanged.AddListener(UpdateTimeLimit);
            mapSizeSlider.onValueChanged.AddListener(UpdateMapSize);
        }

        public void StartGame()
        {
            GameSettings.TimeLimit = timeLimit;
            GameSettings.MazeSize = mazeSize;

            SceneManager.LoadScene("GameScene");
        }

        public void QuitGame()
        {
            Debug.Log("Quit Game");
            Application.Quit();
        }

        void UpdateTimeLimit(float value)
        {
            timeLimit = value;
            UpdateTimeText();
        }

        void UpdateMapSize(float value)
        {
            mazeSize = Mathf.RoundToInt(value);
            UpdateMapSizeText();
        }

        void UpdateTimeText()
        {
            timeText.text = $"Time: {timeLimit / 60f:F1} min";
        }

        void UpdateMapSizeText()
        {
            mapSizeText.text = $"Size: {mazeSize} x {mazeSize}";
        }
    }
}
