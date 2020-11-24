using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour 
{

	[SerializeField] private int sceneIndex;

	private void Start() => StartCoroutine(LoadAsyncronously());

	private IEnumerator LoadAsyncronously()
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
		while (!operation.isDone) yield return null;
	}

}
