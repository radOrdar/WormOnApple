﻿using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Loading
{
    public class GameLoadingOperation : ILoadingOperation
    {
        public string Description => "Loading Game..";
        
        public async UniTask Load(Action<float> onProgress)
        {
            onProgress(0.1f);
            var unloadSceneOperations = new List<UniTask>();
        
            Scene mainMenuScene = SceneManager.GetSceneByName(Constants.Scenes.MAIN_MENU);
            if (mainMenuScene.IsValid())
            {
                unloadSceneOperations.Add(SceneManager.UnloadSceneAsync(mainMenuScene).ToUniTask());
            }

            Scene gameScene = SceneManager.GetSceneByName(Constants.Scenes.GAME);
            if (gameScene.IsValid())
            {
                unloadSceneOperations.Add(SceneManager.UnloadSceneAsync(gameScene).ToUniTask());
            }

            await UniTask.WhenAll(unloadSceneOperations);
            onProgress(0.7f);
            await SceneManager.LoadSceneAsync(Constants.Scenes.GAME, LoadSceneMode.Additive);
            UnityEngine.Debug.Log(SceneManager.SetActiveScene(SceneManager.GetSceneByName(Constants.Scenes.GAME)));
        }
    }
}
