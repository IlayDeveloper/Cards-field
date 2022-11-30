using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityHttpWrapper
{
    public class UnityWebRequestAwaiter : INotifyCompletion
    {
        private UnityWebRequestAsyncOperation _asyncOp;
        private Action _continuation;

        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
        {
            _asyncOp = asyncOp;
            asyncOp.completed += OnRequestCompleted;
        }

        public bool IsCompleted => _asyncOp.isDone;

        public void GetResult()
        {
        }

        public void OnCompleted(Action continuation)
        {
            _continuation = continuation;
        }

        private void OnRequestCompleted(AsyncOperation obj)
        {
            _continuation();
        }
    }
}