using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Data
{
    public class RoundHandler : MonoBehaviour
    {
        /// <summary> Reference to the canvas for the count down timer. </summary>
        [SerializeField]
        [Tooltip("Reference to the canvas for the count down timer.")]
        private GameObject canvas;
        /// <summary> Reference to the textbox for the timer. </summary>
        [SerializeField]
        [Tooltip("Reference to the textbox for the timer.")]
        private Text timerText;
        /// <summary> Spawn Points for this stage. </summary>
        [SerializeField]
        [Tooltip("Spawn Points for this stage.  4 expected.")]
        private Transform[] spawnPoints;

        /// <summary> internal reference for ensuring singleton. </summary>
        private static RoundHandler instance;
        public static RoundHandler Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindObjectOfType<RoundHandler>();
                    if (instance == null)
                        return null;
                    instance.Init();
                }
                return instance;
            }
        }

        /// <summary> Count down timer for round start. </summary>
        private float countDown;
        /// <summary> Reference to all the bards. </summary>
        private PlayerLife[] bards;
        public PlayerLife[] Bards
        {
            get
            {
                if (bards == null)
                    Init();
                return bards;
            }
        }
        /// <summary> Number of players that have died this round. </summary>
        private int deathCount;
        
        void Init()
        {
            bards = new PlayerLife[4];
            for (int i = 0; i < Bards.Length; i++)
            {
                Bards[i] = Data.Instance.Spawn(i, spawnPoints[i]).GetComponent<PlayerLife>();
            }
        }

        void Start()
        {
            instance = this;
            timerText.text = "3";
            canvas.SetActive(true);
            countDown = 3f;
            if (bards == null)
                Init();
            Transform[] targets = new Transform[4];
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i] = Bards[i].transform;
            }
            GameObject.FindObjectOfType<CameraMovement>().targets = targets;
            ResetRound();
        }

        void Update()
        {
            if(deathCount > 2)
            {
                countDown = 3f;
                canvas.SetActive(true);
                ResetRound();
                deathCount = 0;
            }
            if (countDown > -2)
            {
                countDown -= Time.deltaTime;
                if (countDown > 2)
                {
                    timerText.text = "3";
                    //text effects
                }
                else if (countDown > 1)
                {
                    timerText.text = "2";
                    //text effects
                }
                else if (countDown > 0)
                {
                    timerText.text = "1";
                    //text effects
                }
                else if (countDown > -1)
                {
                    timerText.text = "Start!";
                    //text effects
                }
                else
                {
                    StartRound();
                    canvas.SetActive(false);
                    countDown = -5000;
                }
            }
        }

        /// <summary> Increments the death count for this round. </summary>
        public void AddDeath()
        {
            deathCount++;
        }

        /// <summary> Reset all of the bards. </summary>
        private void ResetRound()
        {
            for (int i = 0; i < Bards.Length; i++)
            {
                Bards[i].GetComponent<BaseControl>().enabled = false;
                Bards[i].GetComponent<BaseBard>().enabled = false;
                Bards[i].GetComponent<CharacterController>().enabled = false;
                if(Bards[i].GetComponent<NavMeshAgent>())
                    Bards[i].GetComponent<NavMeshAgent>().enabled = false;
                Bards[i].transform.position = spawnPoints[i].position;
            }
        }

        /// <summary> Spawn all of the bards so they can begin. </summary>
        private void StartRound()
        {
            for (int i = 0; i < Bards.Length; i++)
            {
                Bards[i].GetComponent<BaseControl>().enabled = true;
                Bards[i].GetComponent<BaseBard>().enabled = true;
                Bards[i].GetComponent<CharacterController>().enabled = true;
                if (Bards[i].GetComponent<NavMeshAgent>())
                    Bards[i].GetComponent<NavMeshAgent>().enabled = true;
                Bards[i].Respawn();
                Bards[i].transform.position = spawnPoints[i].position;
            }
        }

        //public GameObject Bards(PlayerID bard)
        //{
        //    return Bards[(int)bard - 1].gameObject;
        //}

        public PlayerControl[] PlayerControl()
        {
            System.Collections.Generic.List<PlayerControl> bc = new System.Collections.Generic.List<PlayerControl>();
            for (int i = 0; i < Bards.Length; i++)
                if (Bards[i].GetComponent<PlayerControl>() != null)
                    bc.Add(Bards[i].GetComponent<PlayerControl>());
            return bc.Count > 0 ? bc.ToArray() : null;
        }

        public BaseControl[] Control()
        {
            BaseControl[] bc = new BaseControl[4];
            for (int i = 0; i < Bards.Length; i++)
                bc[i] = Bards[i].GetComponent<BaseControl>();
            return bc;
        }
    }
}
