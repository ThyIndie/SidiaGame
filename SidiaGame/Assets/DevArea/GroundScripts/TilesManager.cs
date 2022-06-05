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

            [Tooltip("Manually decide how many tiles will be of this color")]
            [Range(1, 256)]
            public float RedRange = 1;
            [Tooltip("Manually decide how many tiles will be of this color")]
            [Range(1, 256)]
            public float GreenRange = 1;
            [Tooltip("Manually decide how many tiles will be of this color")]
            [Range(1, 256)]
            public float YellowRange = 1;
            [Tooltip("Manually decide how many tiles will be of this color")]
            [Range(1, 256)]
            public float BlueRange = 1;

            [Tooltip("Check this box so that the computer decides the colors.")]
            public bool Random;

            [Tooltip("Spawn time between one block and another at the beginning of the game")]
            [Range(0.01f, 1)]
            public float SetSpeed = 0.05f;

            public float ManagerColors() => RedRange + GreenRange + YellowRange + BlueRange;
        }

        public GameOptions GM;
        public GameObject Last;

        public List<int> Colors;
        //1 - RED, 2 - Green, 3 - Yellow, 4- Blue
        public List<Material> MaterialColor;
        void Start()
        {

            GM.GroundTiles = GameObject.FindGameObjectsWithTag("Tile");

            foreach (GameObject tiles in GM.GroundTiles)
                tiles.SetActive(false);

            ApplyRangeColors(true);


        }

        private void Update()
        {


        }

        /// <summary>
        /// Called when the game will start, this void defines the collectibles of each block
        /// </summary>
        /// <param name="IsRandom">True for a random generate tiles</param>
        void ApplyRangeColors(bool IsRandom)
        {

            if (IsRandom)
            {
                for (int i = 0; i < GM.GroundTiles.Length; i++)
                {
                    Colors.Add(Random.Range(0, 4));
                }


                StartCoroutine(PaintGround(GM.SetSpeed, 0));
            }
            else
            {


            }
        }

        IEnumerator PaintGround(float T, int index)
        {
            yield return new WaitForSeconds(T);
            if (index < GM.GroundTiles.Length)
            {
               
                GM.GroundTiles[index].SetActive(true);
                GM.GroundTiles[index].GetComponent<Renderer>().material = MaterialColor[Colors[index]];
                StartCoroutine(PaintGround(GM.SetSpeed, index + 1));
            }
            else
            {

            }
        }


    }
}
