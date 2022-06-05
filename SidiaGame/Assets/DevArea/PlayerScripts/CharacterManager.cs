using SidiaGame.GM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SidiaGame.PlayerCode
{
    public class CharacterManager : MonoBehaviour
    {
        [System.Serializable]
        public class AdminArea
        {
            [Range(1, 100)]
            public int Health=1;
            [Range(1, 100)]
            public int Atack=1;

            public Vector3 Offset = new Vector3(0,0.5f,0);
        }
        [Tooltip("Character settings control section")]
        public AdminArea GM;


        [SerializeField] Animator _anim;

        GameplayController GameMaster;
        public GameObject Pin;

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
       public int Move;

        RaycastHit _right;
        RaycastHit _left;
        RaycastHit _forward;
        RaycastHit _backward;
        #endregion


        Vector3 _Move;


        #region CanvasHud
        public GameObject ButtonAtack;
        public GameObject EndPhase;

        public int IamPlayer;
        int AtualTurn;
        bool MyTurn;
        #endregion

        private void Start()
        {
            GameMaster = FindObjectOfType<GameplayController>();
        }
        private void Update()
        {
            Move = Mathf.Clamp(Move, 0, 3);
            TurnControll();
            if (MyTurn)
            {
                ControllerMove();
            }
            Pin.SetActive(MyTurn);
            Pin.transform.Rotate(0,0, 60 * Time.deltaTime);
        }

        void PickUpItem()
        {

        }

        void TurnControll()
        {
            AtualTurn = GameMaster.PlayerTurn;
            if (AtualTurn % 2 == 0)
            {
                if (IamPlayer == 1)
                    MyTurn = false;
                else
                    MyTurn = true;
            }
            else
            {
                if (IamPlayer == 1)
                    MyTurn = true;
                else
                    MyTurn = false;
            }
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
            else
            {
                if (transform.position != _Move)
                {
                    transform.LookAt(_Move);
                    transform.position = Vector3.MoveTowards(transform.position, _Move,Time.deltaTime);
                    _anim.SetInteger("Action", 1);
                }
                else
                {
                    inMove = false;
                    VerifyAtack();
                    _anim.SetInteger("Action", 0);
                }
            }
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
                    _Move = transform.position + _move;
                }
                else
                {
                    inMove = false;
                }
                
                Move--;
            }
        }

        /// <summary>
        /// Checks if after the move it is possible to attack
        /// </summary>
        void VerifyAtack()
        {
            EndPhase.SetActive(false);
            AtackOpen = false;
            float EnemyDistance = 0;
            if (IamPlayer == 1)
                EnemyDistance = Vector3.Distance(transform.position, GameMaster.P2.transform.position);
            else
                EnemyDistance = Vector3.Distance(transform.position, GameMaster.P1.transform.position);

            if (EnemyDistance <= 2)
                AtackOpen = true;

            if (Move <= 0 && AtackOpen == false)
                GameMaster.ChangeTurn();
            else if (Move <= 0 && AtackOpen == true)
                EndPhase.SetActive(AtackOpen);

            ButtonAtack.SetActive(AtackOpen);
        }


        public void ConfirmAtack(CharacterManager alvo)
        {
            
            transform.LookAt(alvo.transform.position);
            _anim.SetInteger("Action", 2);
            alvo.SetDamage(GM.Atack);
            if (Move <= 0)
                GameMaster.ChangeTurn();

        }
        public void ResetAtack()
        {
            _anim.SetInteger("Action", 0);
            if (MyTurn)
                if (Move <= 1)
                    GameMaster.ChangeTurn();
                else
                    Move--;
        }
        public void SetDamage(int _damage)
        {
            GM.Health -= _damage;
            if (MyTurn)
                if (Move <= 1)
                    GameMaster.ChangeTurn();
                else
                    Move--;
        }

        public void OnChangeTurn()
        {
            
            Move = 3;
            AtackOpen = false;
            VerifyAtack();
        }
    }






    

    
}