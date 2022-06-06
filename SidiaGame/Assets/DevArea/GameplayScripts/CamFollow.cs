using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SidiaGame.GM
{
    public class CamFollow : MonoBehaviour
    {
        public GameObject Alvo,Around;
        public float Speed;
        public Vector3 Offset;
        private void Update()
        {
            if (Alvo != null)
            {
                transform.LookAt(Alvo.transform.position);
                transform.position = Vector3.Lerp(transform.position, Alvo.transform.position + Offset, Speed * Time.deltaTime);

            }else
            {
                transform.LookAt(Around.transform.position);
                transform.RotateAround(Around.transform.position, transform.up, Speed * 25 * Time.deltaTime);
            }
        }
    }
}