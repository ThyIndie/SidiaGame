using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SidiaGame.PlayerCode
{
    public class CharacterManager : MonoBehaviour
    {
        [System.Serializable]
        public class AdminArea
        {
            [Range(1, 100)]
            public float Health=1;
            [Range(1, 100)]
            public float Atack=1;

            public Vector3 Offset = new Vector3(0,0.5f,0);
        }
        [Tooltip("Character settings control section")]
        public AdminArea GM;

        #region AtackArea
        bool AtackOpen;

        #endregion


        #region InternalControllers
        float HorizontalMove() => Input.GetAxisRaw("Horizontal");
        float VerticalMove() => Input.GetAxisRaw("Vertical");

        /// <summary>
        /// In movement?
        /// </summary>
        bool inMove;

        /// <summary>
        /// Remaining moves for this turn
        /// </summary>
        int Move;

        RaycastHit _right;
        RaycastHit _left;
        RaycastHit _forward;
        RaycastHit _backward;
        #endregion

        private void Update()
        {
            ControllerMove();  
        }

        void ControllerMove()
        {
            if (!inMove)
            {
                if (HorizontalMove() != 0)
                    StartMove(new Vector3(HorizontalMove(), 0, 0));
                else if (VerticalMove() != 0)
                    StartMove(new Vector3(0, 0, VerticalMove()));
            }
        }

        void ControllerAtack()
        {

        }

        /// <summary>
        /// First step to move player
        /// </summary>
        /// <param name="Move">Direction of movement</param>
        void StartMove(Vector3 _move)
        {
            if (Move > 0)
            {
                inMove = true;
                bool Inuse = Physics.Raycast(transform.position + GM.Offset, _move, 1);
                if (!Inuse)
                {
                    transform.position = transform.position + _move;
                }
                StartCoroutine(InMoviment());
                Move--;
            }
        }
        /// <summary>
        /// Use to reset state of movement
        /// </summary>
        /// <returns></returns>
        IEnumerator InMoviment()
        {
            yield return new WaitForSeconds(0.1f);
            inMove = false;
            VerifyAtack();
        }

        /// <summary>
        /// Checks if after the move it is possible to attack
        /// </summary>
        void VerifyAtack()
        {
            if (Physics.Raycast(transform.position + GM.Offset, transform.forward, out _forward, 1))
                AtackOpen = true;
            if (Physics.Raycast(transform.position + GM.Offset, -transform.forward, out _backward, 1))
                AtackOpen = true;
            if (Physics.Raycast(transform.position + GM.Offset, transform.right, out _right, 1))
                AtackOpen = true;
            if (Physics.Raycast(transform.position + GM.Offset, -transform.right, out _left, 1))
                AtackOpen = true;
        }

        

    }






    

    
}