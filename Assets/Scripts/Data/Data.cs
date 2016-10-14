using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Data
{
    class Data : MonoBehaviour
    {
        /// <summary> Reference to the prefabs for each character. </summary>
        [SerializeField]
        [Tooltip("Reference to the prefabs for each character.  4 expected")]
        private BaseBard[] characters;

        /// <summary> internal reference fo ensuring singleton. </summary>
        private static Data instance;

        /// <summary> The tunes for each bard. </summary>
        private Tune[][] tunes;
        /// <summary> The sound each bard makes. </summary>
        private AudioClip[] clips;
        /// <summary> The instrument model to spawn for each bard. </summary>
        private GameObject[] instruments;

        /// <summary> The level to async load. </summary>
        internal string level;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                tunes = new Tune[4][];
                for (int i = 0; i < tunes.Length; i++)
                    tunes[i] = new Tune[3];
                clips = new AudioClip[4];
                instruments = new GameObject[4];
                DontDestroyOnLoad(this.gameObject);
            }
            else if (instance != this)
                Destroy(this.gameObject);
        }

        // TEST CODE REMOVE
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.P))
                loadScene("Test");
        }

        /// <summary> Loads a level. </summary>
        /// <param name="name"> The level to go to. </param>
        public void loadScene(string name)
        {
            level = name;
            SceneManager.LoadScene("Load");
        }

        /// <summary> Spawn the bards in the scene. </summary>
        public void Spawn()
        {
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
            for(int i = 0; i < characters.Length; i++)
            {
                BaseBard b = Instantiate(characters[i]);
                b.transform.position = spawnPoints[i % spawnPoints.Length].transform.position;
                b.tunes = tunes[i];
                b.instrumentSound = clips[i];
                //TODO: spawn instrument 
            }
        }

        /// <summary> Clear the saved bard data. </summary>
        public void Reset()
        {
            tunes = new Tune[4][];
            for (int i = 0; i < tunes.Length; i++)
                tunes[i] = new Tune[3];
            clips = new AudioClip[4];
            instruments = new GameObject[4];
        }

        /// <summary> Associate a tune with a specific bard. </summary>
        /// <param name="id"> The bard to add it to. </param>
        /// <param name="tune"> The tune to add. </param>
        /// <param name="index"> The index of this tune. 1-3. Determines order in UI. </param>
        public void AddTuneToPlayer(PlayerID id, Tune tune, int index)
        {
            tunes[(int)id + 1][index] = tune;
        }

        /// <summary> Associate a specific sound with a bard. </summary>
        /// <param name="id"> The bard to add it to. </param>
        /// <param name="instrumentSound"> The sound to add. </param>
        public void AddSoundToPlayer(PlayerID id, AudioClip instrumentSound)
        {
            clips[(int)id + 1] = instrumentSound;
        }

        /// <summary> Associate an instrument model with a bard. </summary>
        /// <param name="id"> The bard to add it to. </param>
        /// <param name="instrument"> The model to add. </param>
        public void AddInstrumentToPlayer(PlayerID id, GameObject instrument)
        {
            instruments[(int)id + 1] = instrument;
        }


    }
}
