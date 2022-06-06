using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SidiaGame.GroundScripts
{
    public class Tiles : MonoBehaviour
    {
        public Renderer Item;
        public Material LifePotion, MoreWalk, MoreAtack;
        public GameObject PickupParticle;

        public void PickupItem()
        {
            Item.gameObject.SetActive(false);
            Instantiate(PickupParticle,transform.position+new Vector3(0,1,0),Quaternion.identity);
        }
        public void Spawnar()
        {
            
            if(GetComponent<Renderer>().material.color != Color.white)
            {
                Item.gameObject.SetActive(true);
                Color _color = GetComponent<Renderer>().material.color;
                if (_color == Color.green)
                    Item.material = LifePotion;
                if(_color == Color.red)
                    Item.material = MoreAtack;
                if (_color == Color.blue)
                    Item.material = MoreWalk;
            }
            else
            {
                Item.gameObject.SetActive(false);
            }
        }
    }
}