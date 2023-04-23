using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KOTF.Core.Services
{
    public class CoroutineService : IService
    {
        private readonly CoroutineRunner _coroutineRunner;
        private readonly Dictionary<Guid, IEnumerator> _hostIdToRoutine = new();

        public CoroutineService()
        {
            _coroutineRunner = new GameObject(nameof(CoroutineRunner)).AddComponent<CoroutineRunner>();
            _coroutineRunner.hideFlags = HideFlags.HideAndDontSave;
        }

        public Coroutine Start(IEnumerator routine)
        {
            return _coroutineRunner.StartCoroutine(routine);
        }

        public Coroutine Start(Guid guid, IEnumerator routine)
        {
            if (_hostIdToRoutine.ContainsKey(guid))
                _hostIdToRoutine[guid] = routine;
            else
                _hostIdToRoutine.Add(guid, routine);

            return Start(routine);
        }

        public void Stop(Guid guid)
        {
            if (!_hostIdToRoutine.TryGetValue(guid, out var routine))
                return;

            _coroutineRunner.StopCoroutine(routine);
        }

        private class CoroutineRunner : MonoBehaviour { }
    }
}
