using Assets.Arch.Utilities;
using UnityEngine;

namespace Assets.Arch
{
    [ExecutionOrder(-900)]
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private GameObject _gamePrefab;
        [SerializeField] private LoadingScreen _loadingScreenPrefab;

        private void Awake()
        {
            var game = FindObjectOfType<Game>();

            if (game != null)
            {
                Destroy(gameObject);
                return;
            }

            game = Instantiate(_gamePrefab).GetComponent<Game>();
            var loadingScreen = Instantiate(_loadingScreenPrefab);
            DontDestroyOnLoad(game);
            game.Run(loadingScreen);
            Destroy(gameObject);
        }
    }
}