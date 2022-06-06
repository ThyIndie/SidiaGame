using SidiaGame.gameMenu;
using SidiaGame.GM;
using SidiaGame.PlayerCode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SidiaGame.GroundScripts
{
    public class TilesManager : MonoBehaviour
    {
        [System.Serializable]
        public class GameOptions
        {
            [Tooltip("All hexes that are part of the floor are on this list")]
            public GameObject[] GroundTiles;

            [Tooltip("Check this box so that the computer decides the colors.")]
            public bool Random;

            [Tooltip("Spawn time between one block and another at the beginning of the game")]
            [Range(0.01f, 1)]
            public float SetSpeed = 0.05f;

          
        }

        public GameOptions GM;

        public List<int> Colors;
        
        public List<Material> MaterialColor;

        public PlayerChoice PC;

        public GameplayController GameMaster;

        public GameObject[] Soldiers;

        [SerializeField] GameObject ButtonAtack;
        [SerializeField] GameObject EndPhase;

        void Start()
        {
            PC = FindObjectOfType<PlayerChoice>();
            GM.GroundTiles = GameObject.FindGameObjectsWithTag("Tile");
            
            foreach (GameObject tiles in GM.GroundTiles)
                tiles.SetActive(false);

            ApplyRangeColors(false);


        }
       

        public void RewindGround()
        {
            foreach (GameObject tile in GM.GroundTiles)
                tile.transform.tag = "Tile";
            int _value = 0;
            for (int i = 0; i < GM.GroundTiles.Length; i++)
            {
                if (GM.GroundTiles[i].GetComponent<Renderer>().material.color == Color.white)
                    _value++;
            }
            if(_value >= 230)
            {
                //Rewind
                ApplyRangeColors(true);
            }
        }

        /// <summary>
        /// Called when the game will start, this void defines the collectibles of each block
        /// </summary>
        /// <param name="IsRandom">True for a random generate tiles</param>
        void ApplyRangeColors(bool rewind)
        {
                Colors.Clear();
                for (int i = 0; i < GM.GroundTiles.Length; i++)
                {
                    Colors.Add(Random.Range(0, MaterialColor.Count));
                }


                StartCoroutine(PaintGround(GM.SetSpeed, 0,rewind));
            
           
        }

        IEnumerator PaintGround(float T, int index,bool rewind)
        {
            yield return new WaitForSeconds(T);
            if (rewind == true)
            {
                if (index < GM.GroundTiles.Length)
                {

                    GM.GroundTiles[index].SetActive(true);
                    if(GM.GroundTiles[index].GetComponent<Renderer>().material.color == Color.white && GM.GroundTiles[index].transform.tag != "busy")
                       GM.GroundTiles[index].GetComponent<Renderer>().material = MaterialColor[Colors[index]];
                    StartCoroutine(PaintGround(GM.SetSpeed, index + 1, true));

                }
            }
            else
            {
                if (index < GM.GroundTiles.Length)
                {

                    GM.GroundTiles[index].SetActive(true);
                    GM.GroundTiles[index].GetComponent<Renderer>().material = MaterialColor[Colors[index]];
                    StartCoroutine(PaintGround(GM.SetSpeed, index + 1,false));

                }
                else
                {
                    SpawnPlayer(0);
                }
            }
        }

        void SpawnPlayer(int Soldier)
        {
            int _random = Random.Range(0, GM.GroundTiles.Length);
            if (GM.GroundTiles[_random].GetComponent<Renderer>().material.color == Color.white)
            {
                if (Soldier == 0)
                {
                    GameObject P1 = Instantiate(Soldiers[PC.PlayerOne], GM.GroundTiles[_random].transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                    GameMaster.P1 = P1.GetComponent<CharacterManager>();
                    GameMaster.P1.IamPlayer = 1;
                    SpawnPlayer(1);
                }
                else
                {
                    GameObject P2 = Instantiate(Soldiers[PC.PlayerTwo], GM.GroundTiles[_random].transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                    GameMaster.P2 = P2.GetComponent<CharacterManager>();
                    GameMaster.P2.IamPlayer = 2;
                    StartCoroutine(Fight());
                }
            }
            else
            {
                SpawnPlayer(Soldier);
            }
        }
        IEnumerator Fight()
        {
            yield return new WaitForSeconds(0.5f);
            ButtonAtack.SetActive(false);
            EndPhase.SetActive(false);
        }
    }
}
