using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SidiaGame.gameMenu
{
    public class PlayerChoice : MonoBehaviour
    {
        public int AtualPlayer = 1;
        public int AtualSoldier;
        public Sprite[] Soldiers;
        public List<string> Health;
        public List<string> Atack;
        public int PlayerOne, PlayerTwo;
        public Image SoldierIMG;
        public TextMeshProUGUI HealthTxt, AtackTxt;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        public void NextPlayer()
        {
            ConfirmAtualPlayer();

        }
        public void NextSoldier()
        {
            if (AtualSoldier <= 1)
                AtualSoldier++;
            else
                AtualSoldier = 0;
            SoldierIMG.sprite = Soldiers[AtualSoldier];
            HealthTxt.text = "Healt: " + Health[AtualSoldier];
            AtackTxt.text = "Atack: " + Atack[AtualSoldier];
        }
        public void BackSoldier()
        {
            if (AtualSoldier >= 1)
                AtualSoldier--;
            else
                AtualSoldier = 2;
            SoldierIMG.sprite = Soldiers[AtualSoldier];
            HealthTxt.text = "Healt: " + Health[AtualSoldier];
            AtackTxt.text = "Atack: " + Atack[AtualSoldier];
        }

        public void ConfirmAtualPlayer()
        {
            if (AtualPlayer == 1)
                PlayerOne = AtualSoldier;
            else
            {
                PlayerTwo = AtualSoldier;
                StartCoroutine(LoadBattle());
            }
            AtualPlayer++;
        }
      

        IEnumerator LoadBattle()
        {
            yield return new WaitForSeconds(1);
            SceneManager.LoadSceneAsync("Battle");
        }
    }
}