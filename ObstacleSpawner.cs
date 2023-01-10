using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    private NoteTimestamp noteTimestamp = new NoteTimestamp();
    private NotePos notePos = new NotePos();
    private float[] timePoint;
    private int[] obstaclePos;
    public GameManager gameManager;
    public SpriteRenderer spriteRenderer;
    public bool gameStateIsOver, initialState;
    public float queueTime;
    public float timeDelay = 0.0F;
    private float time = 0.0F;
    private float scaleMultiplier;
    public GameObject obstacleObject;
    private bool timestampCallOnce;
    //private float[] timePoint = {0F, 1F, 2F, 3F, 4F, 5F, 6F, 7F, 8F, 9F}; //time where the obstacle should spawn, last value is where it ends.
    private float[] obstaclePoint = {2.5F, 1.25F, 0.0F, -1.25F, -2.5F};
    //private int[] obstaclePos = {0, 1, 2, 3, 4, 3, 2, 1, 0}; //absolute requirement : obstaclePos len < timePoint len, diff = 1
    private int currentTimestamp;
    //public float height;
    void Start()
    {   
        timestampCallOnce = false;
        gameStateIsOver = true;
        initialState = true;
        currentTimestamp = 0;
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if(!timestampCallOnce && gameManager.gameIsStarted)
        {
            timestampCallOnce = true;
            switch(gameManager.DropdownIndex)
            {
                case 0:{
                    timePoint = noteTimestamp.one();
                    obstaclePos = notePos.one();
                }break;

                case 1:{
                    timePoint = noteTimestamp.two();
                    obstaclePos = notePos.two();
                }break;

                case 2:{
                    timePoint = noteTimestamp.three();
                    obstaclePos = notePos.three();
                }break;

                default:{
                    timePoint = noteTimestamp.debug();
                    obstaclePos = notePos.debug();
                }break;
            }
            queueTime = timeDelay + Mathf.Abs(timePoint[currentTimestamp + 1] - timePoint[currentTimestamp]);
            scaleMultiplier = Mathf.Abs(timePoint[currentTimestamp + 1] - timePoint[currentTimestamp]);
        }
        
        if(!gameStateIsOver)
        {
            if(time > queueTime)
            {
                if(obstaclePos[currentTimestamp] >= -1){
                    GameObject go = Instantiate(obstacleObject);
                    spriteRenderer = go.GetComponent<SpriteRenderer>();
                        switch(obstaclePos[currentTimestamp])
                        {
                            case 0:{
                                spriteRenderer.color = new Color(0, 1, 0, 1);
                            }break;                        
                            
                            case 1:{
                                spriteRenderer.color = new Color(0, 0, 1, 1);
                            }break;
                                
                            case 2:{
                                spriteRenderer.color = new Color(0.5F, 0.5F, 0.5F, 1);
                            }break; 

                            case 3:{
                                spriteRenderer.color = new Color(1, 1, 0, 1);
                            }break;
                                    
                            case 4:{
                                spriteRenderer.color = new Color(1, 0, 1, 1);
                            }break;

                            default:{
                                spriteRenderer.color = new Color(1, 1, 1, 0.001F);
                            }break;
                        }
                    if(obstaclePos[currentTimestamp] != -1){
                        go.transform.localScale = new Vector3(scaleMultiplier, transform.localScale.y, transform.localScale.z);
                        go.transform.position = new Vector3(transform.localPosition.x, obstaclePoint[obstaclePos[currentTimestamp]], transform.localPosition.z);
                    }
                    else{
                        go.transform.localScale = new Vector3(scaleMultiplier, transform.localScale.y, transform.localScale.z);
                        go.transform.position = new Vector3(transform.localPosition.x, obstaclePoint[1], transform.localPosition.z);
                        go.GetComponent<BoxCollider2D>().enabled = false;
                    }
                    Destroy(go, 15);
                }
                
                time = 0;
                if(currentTimestamp < timePoint.Length - 2){
                    currentTimestamp += 1;
                    if(obstaclePos[currentTimestamp] >= 0){
                        queueTime = Mathf.Abs(timePoint[currentTimestamp] - timePoint[currentTimestamp - 1]);
                    }
                    else{
                        queueTime = timeDelay + Mathf.Abs(timePoint[currentTimestamp] - timePoint[currentTimestamp - 1]);
                    }
                    scaleMultiplier =  Mathf.Abs(timePoint[currentTimestamp] - timePoint[currentTimestamp + 1]);
                }
                else{
                    StartCoroutine(wait(timePoint[timePoint.Length - 2], timePoint[timePoint.Length - 1]));
                    currentTimestamp = 0;
                    queueTime = timeDelay + Mathf.Abs(timePoint[timePoint.Length - 1]);
                    scaleMultiplier = Mathf.Abs(timePoint[currentTimestamp] - timePoint[currentTimestamp + 1]);
                }
            }
            time += Time.deltaTime;
        } 
    }
    IEnumerator wait(float timestampFinalPrev, float timestampFinal)
    {
        yield return new WaitForSeconds(Mathf.Abs(timestampFinalPrev - timestampFinal) + 3);
        gameStateIsOver = true;
    }
}
