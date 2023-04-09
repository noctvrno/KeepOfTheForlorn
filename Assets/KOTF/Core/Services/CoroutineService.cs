using System.Collections;
using UnityEngine;

namespace KOTF.Core.Services
{
    public class CoroutineService : IService
    {
        private readonly CoroutineRunner _coroutineRunner;

        public CoroutineService()
        {
            _coroutineRunner = new GameObject(nameof(CoroutineRunner)).AddComponent<CoroutineRunner>();
            _coroutineRunner.hideFlags = HideFlags.HideAndDontSave;
        }

        public Coroutine Start(IEnumerator routine)
        {
            return _coroutineRunner.StartCoroutine(routine);
        }

        private class CoroutineRunner : MonoBehaviour { }
    }
}
