using System.Collections;
using UnityEngine;

namespace Assets.Arch.Services
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}