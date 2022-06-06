using SidiaGame.GM;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SidiaGame.PlayerCode
{
    public class CharacterManager : MonoBehaviour
    {
        [System.Serializable]
        public class AdminArea
        {
            [Range(0, 150)]
            public int Health=1;
            [Range(1, 150)]
            public int Atack=1;
            [Range(1, 150)]
            public int AtackMultiply = 1;
            public Vector3 Offset = new Vector3(0,0.5f,0);
        }
        [Tooltip("Character settings control section")]
        public AdminArea GM;

        #region VisualFeedback
        /// <summary>
        /// Slider with my health
        /// </summary>
        [SerializeField] Slider Mylife;
        /// <summary>
        /// Exit Particle Shot
        /// </summary>
        [SerializeField] Transform ParticleShotExit;
        /// <summary>
        /// The particle shot
        /// </summary>
        [SerializeField] GameObject particleShot;
        /// <summary>
        /// My Animator
        /// </summary>
        [SerializeField] Animator _anim;
        /// <summary>
        /// GM of game
        /// </summary>
        [SerializeField] GameplayController GameMaster;
        /// <summary>
        /// shift indicator
        /// </summary>
        [SerializeField] GameObject Pin;
        #endregion
        #region AtackArea
        bool AtackOpen;
     
        #endregion


        #region InternalControllers
        float HorizontalMove() => Input.GetAxisRaw("Horizontal");
        float VerticalMove() => Input.GetAxisRaw("Vertical");

        /// <summary>
        /// In movement?
        /// </summary>
       public bool inMove;

        /// <summary>
        /// Remaining moves for this turn
        /// </summary>
       public int Move;

        /// <summary>
        /// RayHits for 4 directions
        /// </summary>
        RaycastHit _right;
        RaycastHit _left;
        RaycastHit _forward;
        RaycastHit _backward;
        #endregion

        /// <summary>
        /// Vector with a direction of move
        /// </summary>
        Vector3 _Move;


        #region CanvasHud
        [SerializeField] GameObject ButtonAtack;
        [SerializeField] GameObject EndPhase;

        /// <summary>
        /// Indicates if I am player one or two
        /// </summary>
        [HideInInspector]
        public int IamPlayer;
        /// <summary>
        /// current match turn
        /// </summary>
        int CurrentTurn;
        /// <summary>
        /// My Turn?
        /// </summary>
        bool MyTurn;
        #endregion
        [HideInInspector]
        public bool Death;

        [SerializeField] TextMeshProUGUI debugScreen;
        [SerializeField] TextMeshProUGUI Moveremaing;

        [SerializeField] GameObject WalkEffect;

        

        private void Start()
        {
            SetConfigurations();
            TurnControll();
        }

        void SetConfigurations()
        {
            //SetLife And atack Limits
            GM.Health = Mathf.Clamp(GM.Health, 0, GM.Health);
            GM.Atack = Mathf.Clamp(GM.Atack, 0, GM.Atack);
            //
            ButtonAtack = GameObject.Find("AtackButton");
            EndPhase = GameObject.Find("EndButton");
            GameMaster = FindObjectOfType<GameplayController>();
            debugScreen = GameObject.Find("DebugInScreen").GetComponent<TextMeshProUGUI>();
            //
            if (IamPlayer == 1)
            {
                Moveremaing = GameObject.Find("moveOne").GetComponent<TextMeshProUGUI>();
                Mylife = GameObject.Find("HPP1").GetComponent<Slider>();
                Mylife.maxValue = GM.Health;
               
            }
            else
            {
                Moveremaing = GameObject.Find("moveTwo").GetComponent<TextMeshProUGUI>();
                Mylife = GameObject.Find("HPP2").GetComponent<Slider>();
                Mylife.maxValue = GM.Health;
            }
            //
          

        }

        private void Update()
        {
            TurnControll();
            LifeandMove();
            if (MyTurn)
            {
                ControllerMove();
            }
           
        }
        void LifeandMove()
        {
            Mylife.value = GM.Health;
            if (GM.Health <= 0)
            {
                _anim.SetTrigger("death");
                GetComponent<CharacterManager>().enabled = false;
                Death = true;
            }
            
               
            

            Moveremaing.text = "remaining moves: " + Move;

            Move = Mathf.Clamp(Move, 0, 3);

            Pin.SetActive(MyTurn);

            Pin.transform.Rotate(0, 0, 60 * Time.deltaTime);

            RaycastHit _ground_;
            if (Physics.Raycast(transform.position + GM.Offset, -transform.up, out _ground_, 1))
                if (_ground_.transform.tag != "busy")
                    _ground_.transform.tag = "busy";
        }
        IEnumerator PickUpItem(string debug)
        {
            debugScreen.text = debug;
            yield return new WaitForSeconds(2);
            debugScreen.text = "";
        }

        void TurnControll()
        {
            CurrentTurn = GameMaster.PlayerTurn;
            if (CurrentTurn % 2 == 0)
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
                    WalkEffect.SetActive(true);
                   
                }
                else
                {
                    VerifyGround();
                    WalkEffect.SetActive(false);
                    inMove = false;
                    
                    _anim.SetInteger("Action", 0);
                 
                }
            }
        }
      
        public void VerifyGround()
        {
            RaycastHit _ground;
            if(Physics.Raycast(transform.position+GM.Offset,-transform.up,out _ground, 3))
            {
                if(_ground.transform.GetComponent<Renderer>().material.color == Color.red)
                {
                    //Atack
                    _ground.transform.GetComponent<Renderer>().material.color = Color.white;
                    GM.Atack = GM.AtackMultiply;
                    StartCoroutine(PickUpItem("Atack X2"));
                }
                if (_ground.transform.GetComponent<Renderer>().material.color == Color.green)
                {
                    //Life
                    _ground.transform.GetComponent<Renderer>().material.color = Color.white;
                    GM.Health += 10;
                    StartCoroutine(PickUpItem("Health + 10"));
                }
                if (_ground.transform.GetComponent<Renderer>().material.color == Color.blue)
                {
                    //Move
                    _ground.transform.GetComponent<Renderer>().material.color = Color.white;
                    Move++;
                    StartCoroutine(PickUpItem("+1 Movement"));
                }
                VerifyAtack();
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
                    Move--;
                }
                else
                {
                    inMove = false;
                }
                
               
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

            if (EnemyDistance <= 1.5f)
                AtackOpen = true;

            if (Move <= 0 && AtackOpen == false)
                GameMaster.ChangeTurn();
            else if (Move <= 0 && AtackOpen == true)
                EndPhase.SetActive(AtackOpen);

            ButtonAtack.SetActive(AtackOpen);
        }

        public void RealizeAction()
        {
            Debug.Log("COnfirmando Mudança: " + IamPlayer);
            if (MyTurn)
                if (Move <= 1)
                    GameMaster.ChangeTurn();
                else
                    Move--;
        }
        public void ConfirmAtack(CharacterManager alvo)
        {
            
            transform.LookAt(alvo.transform.position);

            _anim.SetInteger("Action", 2);

            alvo.SetDamage(GM.Atack);

            if (Move <= 0)
                GameMaster.ChangeTurn();

            GM.Atack = GM.AtackMultiply / 2;
        }
        public void ResetAtack()
        {
            _anim.SetInteger("Action", 0);
        }
        public void SetDamage(int _damage)
        {
            GM.Atack = GM.AtackMultiply / 2;
            GM.Health -= _damage;

        }

        public void OnChangeTurn()
        {
            GM.Atack = GM.AtackMultiply / 2;
            Move = 3;
            AtackOpen = false;
            
            VerifyAtack();
        }

        public void Shot()
        {
            Instantiate(particleShot, ParticleShotExit.transform.position,Quaternion.identity);
        }
    }






    

    
}