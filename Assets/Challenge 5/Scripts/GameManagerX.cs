using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

struct ListenerData
{
	public MonoBehaviour m;
	public string l;

	public ListenerData(MonoBehaviour listener, string reader) : this()
	{
		this.m = listener;
		this.l = reader;
	}
}

public class GameManagerX : MonoBehaviour
{
	public TextMeshProUGUI scoreText;
	public GameObject gameOverScreen;
	public GameObject titleScreen;
	private List<ListenerData> listenerStart = new List<ListenerData>();
	private List<ListenerData> listenerEnd = new List<ListenerData>();

	internal void listenStart(MonoBehaviour listener, string reader)
	{
		listenerStart.Add(new ListenerData(listener, reader));
	}
	internal void listenEnd(MonoBehaviour listener, string reader)
	{
		listenerEnd.Add(new ListenerData(listener, reader));
	}

	public List<GameObject> targetPrefabs;

	private int score;
	private float spawnRate = 1.5f;
	public bool isGameActive;

	private float spaceBetweenSquares = 2.5f;
	private float minValueX = -3.75f; //  x value of the center of the left-most square
	private float minValueY = -3.75f; //  y value of the center of the bottom-most square

	// Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked
	public void StartGame(int difficulty)
	{
		spawnRate /= difficulty;
		isGameActive = true;
		StartCoroutine(SpawnTarget());
		score = 0;
		UpdateScore(0);
		gameOverScreen.SetActive(false);
		titleScreen.SetActive(false);
		foreach (ListenerData listener in listenerStart)
		{
			listener.m.Invoke(listener.l, 0);
		}
	}

	// While game is active spawn a random target
	IEnumerator SpawnTarget()
	{
		while (isGameActive)
		{
			yield return new WaitForSeconds(spawnRate);
			int index = Random.Range(0, targetPrefabs.Count);

			if (isGameActive)
			{
				Instantiate(targetPrefabs[index], RandomSpawnPosition(), targetPrefabs[index].transform.rotation);
			}

		}
	}

	// Generate a random spawn position based on a random index from 0 to 3
	Vector3 RandomSpawnPosition()
	{
		float spawnPosX = minValueX + (RandomSquareIndex() * spaceBetweenSquares);
		float spawnPosY = minValueY + (RandomSquareIndex() * spaceBetweenSquares);

		Vector3 spawnPosition = new Vector3(spawnPosX, spawnPosY, 0);
		return spawnPosition;

	}

	// Generates random square index from 0 to 3, which determines which square the target will appear in
	int RandomSquareIndex()
	{
		return Random.Range(0, 4);
	}

	// Update score with value from target clicked
	public void UpdateScore(int scoreToAdd)
	{
		score += scoreToAdd;
		scoreText.text = "Score: " + score;
	}

	// Stop game, bring up game over text and restart button
	public void GameOver()
	{
		gameOverScreen.SetActive(true);
		isGameActive = false;

		foreach (ListenerData listener in listenerEnd)
		{
			listener.m.Invoke(listener.l, 0);
		}
	}

	// Restart game by reloading the scene
	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}
