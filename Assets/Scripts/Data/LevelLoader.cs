using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Data
{
    class LevelLoader : MonoBehaviour
    {
        /// <summary> The minimum amount of time in seconds to wait before loading the next scene. </summary>
        [SerializeField]
        [Tooltip("The minimum amount of time in seconds to wait before loading the next scene.")]
        private float waitTime;

        /// <summary> Reference to the data class with all of the properties needed. </summary>
        private Data data;
        /// <summary> Reference to the load operation to track it. </summary>
        private AsyncOperation op;
        /// <summary> Simple timer. </summary>
        private float time;

        void Start()
        {
            data = FindObjectOfType<Data>();
            op = SceneManager.LoadSceneAsync(data.level);
            op.allowSceneActivation = false;
            time = waitTime;
        }

        void Update()
        {
            if ((time -= Time.deltaTime) < 0)
                op.allowSceneActivation = true;
        }
    }
}
