using SidiaGame.gameMenu;
using SidiaGame.GroundScripts;
using SidiaGame.PlayerCode;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SidiaGame.GM
{
    public class GameplayController : MonoBehaviour
    {
        /// <summary>
        /// MainCamera
        /// </summary>
        public CamFollow _cam;

        /// <summary>
        /// Camera of dices roll
        /// </summary>
        public Camera _camdices;
        /// <summary>
        /// Manager of tiles
        /// </summary>
        public TilesManager groundTiles;

        /// <summary>
        /// HUD of game
        /// </summary>
        public GameObject HUD;

        /// <summary>
        /// Dices Of players
        /// </summary>
        public GameObject DicesPlayerOne,DicesplayerTwo;

        /// <summary>
        /// Spawns of dices
        /// </summary>
        public Transform[] SpawnsPlayerOne,SpawnsPlayerTwo;
        
        /// <summary>
        /// Atual player turn
        /// </summary>
        [HideInInspector]
        public int PlayerTurn=1;

        /// <summary>
        /// Number of dices with a result after roll
        /// </summary>
        [HideInInspector]
        public int DicesStoped;

        /// <summary>
        /// List with the result of the dice rolls
        /// </summary>
        [HideInInspector]
        public List<DiceCode> PlayerOneResult, PlayerTwoResult;

        /// <summary>
        ///Players!
        /// </summary>
        [HideInInspector]
        public CharacterManager P1, P2;

        /// <summary>
        /// Points of battle dices
        /// </summary>
        int points1, points2;

        /// <summary>
        /// Hud animator effect
        /// </summary>
        public Animator Hudeffect;

        /// <summary>
        /// Winner of roll dices
        /// </summary>
        int atualwinner;

        /// <summary>
        /// Confirm End the battle
        /// </summary>
        bool EndBattle;
        //


        
      
        private void Update()
        {
            #region Camfollow
            if (PlayerTurn%2 == 0 && P1 != null)
            {
                _cam.Alvo = P2.gameObject;
            }
            else if(P1!=null)
            {
                _cam.Alvo = P1.gameObject;
            }
            #endregion
            #region EndBattle
            if (P1 != null)
            {
                if (P1.Death == true && EndBattle == false)
                {
                    EndBattle = true;
                    Hudeffect.SetInteger("end", 2);
                    GetComponent<AudioSource>().enabled = true;
                    Destroy(GetComponent<GameplayController>());
                }
                if (P2.Death == true && EndBattle == false)
                {
                    EndBattle = true;
                    Hudeffect.SetInteger("end", 1);
                    GetComponent<AudioSource>().enabled = true;
                    Destroy(GetComponent<GameplayController>());
                }

            }
            #endregion
            if (DicesStoped > 5)
            {
                DicesStoped = 0;
                CalculteDiceResult();
            }
        }

        void CalculteDiceResult()
        {
            
            //Dices player one
            int[] result_player_one = new int[] { PlayerOneResult[0].Result, PlayerOneResult[1].Result, PlayerOneResult[2].Result };
            int result_complete_one = PlayerOneResult[0].Result + PlayerOneResult[1].Result + PlayerOneResult[2].Result;
            int max_number_one = result_player_one.Max();
            int min_number_one = result_player_one.Min();
            int middle_number_one = result_complete_one - max_number_one - min_number_one;
            //Dices player Two
            int[] result_player_two = new int[] { PlayerTwoResult[0].Result, PlayerTwoResult[1].Result, PlayerTwoResult[2].Result };
            int result_complete_two = PlayerTwoResult[0].Result + PlayerTwoResult[1].Result + PlayerTwoResult[2].Result;
            int max_number_two = result_player_two.Max();
            int min_number_two = result_player_two.Min();
            int middle_number_two = result_complete_two - max_number_two - min_number_two;
            //Decide winner
            if (max_number_one == max_number_two)
            {
                points1++;
                points2++;
            }
            else if (max_number_one > max_number_two)
                points1++;
            else
                points2++;
            //
            if (middle_number_one == middle_number_two)
            {
                points1++;
                points2++;
            }
            else if (middle_number_one > middle_number_two)
                points1++;
            else
                points2++;
            //
            if (min_number_one == min_number_two)
            {
                points1++;
                points2++;
            }
            else if (min_number_one > min_number_two)
                points1++;
            else
                points2++;

            StartCoroutine(SetWinner());
            
        }

        IEnumerator SetWinner()
        {
            if (points1 == points2)
            {
               
                if (PlayerTurn % 2 == 0)
                {
                    Hudeffect.SetInteger("winner", 2);
                    atualwinner = 2;
                }
                else
                {
                    Hudeffect.SetInteger("winner", 1);
                    atualwinner = 1;
                }
            }
            else
            {
                if (points1 > points2)
                {
                    Hudeffect.SetInteger("winner", 1);
                    atualwinner = 1;
                }
                else
                {
                    Hudeffect.SetInteger("winner", 2);
                    atualwinner = 2;
                }
            }

            yield return new WaitForSeconds(2.9f);

            Hudeffect.SetInteger("winner", 0);
            _camdices.gameObject.SetActive(false);
            if (atualwinner == 1)
                P1.ConfirmAtack(P2);
            else
                P2.ConfirmAtack(P1);

            foreach (DiceCode dices in PlayerOneResult)
                Destroy(dices.gameObject);
            foreach (DiceCode dices in PlayerTwoResult)
                Destroy(dices.gameObject);

            P1.RealizeAction();
            P2.RealizeAction();
            points1 = 0;
            points2 = 0;
          
            HUD.SetActive(true);
        }

        public void ChangeTurn()
        {
            PlayerTurn++;
            
            groundTiles.RewindGround();
            Hudeffect.SetTrigger("endphase");
           
            if(PlayerTurn%2 == 0)
                P1.OnChangeTurn();
            else
                P2.OnChangeTurn();


        }

        public void RollDices()
        {
            StartCoroutine(ChangeCamToDices());
          
                
            HUD.SetActive(false);
        }

        IEnumerator ChangeCamToDices()
        {
            Hudeffect.SetBool("battle", true);
            PlayerOneResult.Clear();
            PlayerTwoResult.Clear();
            yield return new WaitForSeconds(1.75f);
            Hudeffect.SetBool("battle", false);
            _camdices.gameObject.SetActive(true);
            for (int i = 0; i < SpawnsPlayerOne.Length; i++)
            {
                PlayerOneResult.Add(Instantiate(DicesPlayerOne, SpawnsPlayerOne[i].position, new Quaternion(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180))).GetComponent<DiceCode>());

            }
            for (int i = 0; i < SpawnsPlayerOne.Length; i++)
            {
                PlayerTwoResult.Add(Instantiate(DicesplayerTwo, SpawnsPlayerTwo[i].position, new Quaternion(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180))).GetComponent<DiceCode>());
            }
          
        }

        public void ConfirmStop()
        {
            DicesStoped++;
        }

    }
}