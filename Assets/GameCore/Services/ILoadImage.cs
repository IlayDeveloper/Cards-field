using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GameCore.Services
{
    public interface ILoadImage
    {
        Task<Texture2D> LoadRandomTexture(int width, int height);
        
        void LoadRandomTexture(int width, int height, Action<Texture2D> onCompleted);
    }
}