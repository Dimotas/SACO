using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Camera cam;
    public GameObject[] moedas;
    private float MaxWidth;
    public float timeLeft;
    public Text txtTimeLeft;
    public GameObject txtGameOver;
    public HatController hc;
    public Score sc;
    private bool playing;
    public GameObject splashImage;
    public GameObject StartButton;
    public GameObject RestartButton;
    public GameObject[] ShowVidas;
    static public int Vidas = 3;
    private Scene cena;
    private float[] tempos = { 2.0f, 1.0f, 0.5f };
    void Start()
    {
        splashImage.SetActive(true);
        StartButton.SetActive(true);
        RestartButton.SetActive(false);
        timeLeft = 20.0f;
        playing = false;
        hc.MudaMexer(false);
        txtGameOver.SetActive(false);
        if (cam == null) cam = Camera.main;
        Vector3 UpperC = new Vector3(Screen.width, Screen.height, 0.0f);
        Vector3 dim = cam.ScreenToWorldPoint(UpperC);
        MaxWidth = dim.x - moedas[0].GetComponent<Renderer>().bounds.extents.x;

        cena = SceneManager.GetActiveScene();

        if (cena.buildIndex > 0) StartGame();


    }
    public void RestartGame()
    {
        RestartButton.SetActive(false);
        txtGameOver.SetActive(false);
        timeLeft = 20.0f;
        Vidas = 3;
        sc.ScoreAZero();
        SceneManager.LoadScene("SampleScene");

        StartGame();

    }
    public void StartGame()
    {
        splashImage.SetActive(false);
        StartButton.SetActive(false);
        timeLeft = 20.0f;
        Vidas = 3;
        sc.ScoreAZero();
        hc.MudaMexer(true);
        playing = true;
        UpdateVidas();
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playing)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                txtTimeLeft.text = "Time Left:\n" + Mathf.RoundToInt(timeLeft);
            }
        }
    }

    IEnumerator Spawn() {
        while (timeLeft>0 && Vidas>0)
        {
           
            Vector3 spawPosition = new Vector3(Random.Range(-MaxWidth, MaxWidth),
                transform.position.y, 0.0f);
            bool siga = true;
            GameObject moeda=null;
            while (siga)
            {
                moeda = moedas[Random.Range(0, moedas.Length)];
                if (moeda.CompareTag("Clock") && timeLeft > 20.0f)
                {
                    siga = true;
                }
                else
                {
                    siga = false;
                }
                
            }
            Instantiate(moeda, spawPosition, Quaternion.identity);
            yield return new WaitForSeconds(tempos[cena.buildIndex]);
        }
        if (Vidas > 0)
        {
            int nCenas = SceneManager.sceneCountInBuildSettings;

            SceneManager.LoadScene((cena.buildIndex + 1 < nCenas ? cena.buildIndex + 1 : 0));
           
            /*if (cena.buildIndex+1 < nCenas)
                 SceneManager.LoadScene(cena.buildIndex + 1);
            else SceneManager.LoadScene(0);*/
        }
        else
        {
            playing = false;
            yield return new WaitForSeconds(1.0f);
            hc.MudaMexer(false);
            txtGameOver.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            RestartButton.SetActive(true);
        }
    }
    public void MudaTempo(float tempo)
    {
        timeLeft += tempo;
    }
    private void UpdateVidas()
    {
        for(int i=0; i<3; i++)
        {
            if (i < Vidas) ShowVidas[i].SetActive(true);
            else ShowVidas[i].SetActive(false);
        }
    }
    public void LifeScore(int vidas)
    {
        
        Vidas = Mathf.Clamp(Vidas + vidas, 0, 3);
        UpdateVidas();
    }
}
