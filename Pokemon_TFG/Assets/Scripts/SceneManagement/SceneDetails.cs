﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour
{

    [SerializeField] List<SceneDetails> connectedScenes;

    public bool IsLoaded { get; private set; }

    private List<SavableEntity> savableEntities;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log($"{collision.gameObject.name} ha entrado en {gameObject.name}.");

            LoadScene();
            GameController.Instance.SetCurrentScene(this);

            //Load all connected scenes
            foreach (var scene in connectedScenes)
            {
                scene.LoadScene();
            }

            //Unload scenes that are no longer connected
            SceneDetails prevScene = GameController.Instance.PreviousScene;

            if (prevScene != null)
            {
                var prevLoadedScenes = prevScene.connectedScenes;
                foreach (var scene in prevLoadedScenes)
                {
                    if(!connectedScenes.Contains(scene) && scene != this)
                    {
                        scene.UnloadScene();
                    }
                }

                if (!connectedScenes.Contains(prevScene))
                    prevScene.UnloadScene();
            }
        }
    }

    public void LoadScene()
    {
        if (!IsLoaded)
        {
            var loadOperation = SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            IsLoaded = true;

            loadOperation.completed += (AsyncOperation op) =>
            {
                savableEntities = GetSavableEntitiesInScene();
                SavingSystem.i.RestoreEntityStates(savableEntities);
            };

        }
    }

    public void UnloadScene()
    {
        if (IsLoaded)
        {
            SavingSystem.i.CaptureEntityStates(savableEntities);

            SceneManager.UnloadSceneAsync(gameObject.name);
            IsLoaded = false;
        }
    }

    private List<SavableEntity> GetSavableEntitiesInScene()
    {
        Scene currentScene = SceneManager.GetSceneByName(gameObject.name);
        var savableEntities = FindObjectsOfType<SavableEntity>().Where(x => x.gameObject.scene == currentScene).ToList();

        return savableEntities;
    }
}
