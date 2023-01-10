using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float velocity = 2.4f;
    public GameManager gameManager;
    public ObstacleSpawner obstacleSpawner;
    private Rigidbody2D rigidbodyPlayer;
    private Vector2 vec;
    private float peakFreq, playerPos;
    private int i = 0;
    private float[] freqAvg = {0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F};

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(obstacleSpawner.initialState == false)
        {
            peakFreq = AudioInputBehaviour.currentMax * 10;
            freqAvg[i] = peakFreq;
            if(i < freqAvg.Length - 1){
                i++;
            }
            else{
                i = 0;
            }
            //Debug.Log(freqAvg.Average());
            vec = transform.localPosition;
            //vec.y = freqAvg.Average() - 3.6F;
            //vec.y = 3.42F - 3.5F;
            Debug.Log(freqAvg.Average() * 100);
            vec.y = PlayerPosDefiner(freqAvg.Average() * 100);
            transform.localPosition = vec;
        }
        else
        {
            vec = transform.localPosition;
            vec.y = 0.0F;
            transform.localPosition = vec;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        gameManager.ScoreKeeper();
    }

    private float PlayerPosDefiner(float freqAvg)
    {
        if(freqAvg < 210.100F){ playerPos = -3.125F; } //under
        else if(freqAvg >= 210.100F && freqAvg <= 269.180F){ playerPos = -2.500F; }// 4
        else if(freqAvg > 269.180F && freqAvg < 272.130F){ playerPos = -1.875F; } //4-3
        else if(freqAvg >= 272.130F && freqAvg <= 330.870F){ playerPos = -1.250F; } //3
        else if(freqAvg > 330.870F && freqAvg < 333.320F){ playerPos = -0.625F; } //3-2
        else if(freqAvg >= 333.320F && freqAvg <= 339.710F){ playerPos = 0.000F; } //2
        else if(freqAvg > 339.710F && freqAvg < 341.430F){ playerPos = 0.625F; } //2-1
        else if(freqAvg >= 341.430F && freqAvg <= 401.250F){ playerPos = 1.250F; } //1
        else if(freqAvg > 401.250 && freqAvg < 408.700F){ playerPos = 1.875F; } //1-0
        else if(freqAvg >= 408.700F && freqAvg <= 548.720F){ playerPos = 2.500F; } //0
        else if(freqAvg > 548.720F){ playerPos = 3.125F; } //upper
        else{ playerPos = -100F; }
        return playerPos;
    }
}
