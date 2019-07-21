using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    int playerLives = 3;
    int score = 0;
    int bookGet = 0;
    int kunaiAmount = 0;

    int bookToGet;

    [SerializeField] Text livesText;
    [SerializeField] Text scoreText;
    [SerializeField] Text bookText;
    [SerializeField] Text kunaiText;

    private void Awake()
    {
        int gameSessionsNumber = FindObjectsOfType<GameSession>().Length;

        if (gameSessionsNumber > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();

        bookToGet = 3;

        bookText.text = bookGet.ToString() + "/" + bookToGet.ToString();

        kunaiText.text = kunaiAmount.ToString();

        print(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator onPlayerDead()
    {
        yield return new WaitForSecondsRealtime(2f);

        if (playerLives > 1)
        {
            playerLives--;
            livesText.text = playerLives.ToString();
            score = 0;
            bookGet = 0;
            kunaiAmount = 0;
            bookToGet = (SceneManager.GetActiveScene().buildIndex == 1)? 3 : 4;
            scoreText.text = score.ToString();
            bookText.text = bookGet.ToString() + "/" + bookToGet.ToString();
            kunaiText.text = kunaiAmount.ToString();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            BackToMainMenu();
        }
    }

    public void AddToScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();
    }

    void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void AddBook()
    {
        bookGet++;
        bookText.text = bookGet.ToString() + "/" + bookToGet.ToString();

        if (bookGet == bookToGet)
        {
            print("Level passed");
        }
    }

    public void AddKunai()
    {
        kunaiAmount++;
        kunaiText.text = kunaiAmount.ToString();
    }

    public int GetKunaiAmount()
    {
        return kunaiAmount;
    }

    public void RemoveKunai()
    {
        kunaiAmount--;
        kunaiText.text = kunaiAmount.ToString();
    }

    public bool CheckBookAmount()
    {
        return (bookGet == bookToGet);
    }

    public void ChangeBookToGetAmount()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            bookToGet = 4;
            bookText.text = bookGet.ToString() + "/" + bookToGet.ToString();
        }
    }
}
