using Assets.Arch.Services;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    private AudioSource _source;
    private IGameLogic _gameLogic;
    private IStaticDataService _staticData;

    private void OnDisable() =>
        _gameLogic.PlaySound -= PlaySound;

    public void Construct(IStaticDataService staticData, IGameLogic gameLogic)
    {
        _source = gameObject.GetComponent<AudioSource>();
        _source.playOnAwake = false;
        _source.loop = false;
        _gameLogic = gameLogic;
        _gameLogic.PlaySound += PlaySound;
        _staticData = staticData;
    }

    private void PlaySound(string clipName) =>
        _source.PlayOneShot(_staticData.GetSound(clipName));
}
