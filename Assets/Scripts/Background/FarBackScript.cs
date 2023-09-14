using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum LAYERXORDER
{
    LEFT,
    CENTER,
    RIGHT
}
public class FarBackScript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject CamObj;
   

    float dist = 0;
    float LayerYPos = 5;
    float StartCamx;
    List<ValueTuple<LayerScript, LayerScript, LayerScript>> LayerPairs = new List<ValueTuple<LayerScript, LayerScript, LayerScript>>();
    LayerScript LeftLayer;
    LayerScript CenterLayer;
    LayerScript RightLayer;
    // Start is called before the first frame update
    private void Awake()
    {



    }
    void Start()
    {
        //CamObj = Camera.main.gameObject;
        CamObj = Camera.main.gameObject;
        LayerScript[] Layers = transform.GetComponentsInChildren<LayerScript>();
        for (int i = 0; i < Layers.Length; i = i + 3)
        {
            ValueTuple<LayerScript, LayerScript, LayerScript> temp = (Layers[i], Layers[i + 1], Layers[i + 2]);
            temp.Item1.Order = LAYERXORDER.LEFT;
            temp.Item2.Order = LAYERXORDER.CENTER;
            temp.Item3.Order = LAYERXORDER.RIGHT;
            LayerPairs.Add(temp);
        }
        StartCamx = CamObj.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {//1, 0.7, 0.4
        
        dist = StartCamx - CamObj.transform.position.x;
        transform.position = new Vector3(CamObj.transform.position.x, 0, 0);
        for (int i = 0; i < LayerPairs.Count; i++)
        {

            LayerPairs[i].Item1.transform.position = new Vector3(dist * (1 - (float)i / (float)LayerPairs.Count) + LayerPairs[i].Item1.Pivot, LayerYPos);
            LayerPairs[i].Item2.transform.position = new Vector3(dist * (1 - (float)i / (float)LayerPairs.Count) + LayerPairs[i].Item2.Pivot, LayerYPos);
            LayerPairs[i].Item3.transform.position = new Vector3(dist * (1 - (float)i / (float)LayerPairs.Count) + LayerPairs[i].Item3.Pivot, LayerYPos);


            //1번 Layer 판명
            if (LAYERXORDER.LEFT == LayerPairs[i].Item1.Order)
            {
                LeftLayer = LayerPairs[i].Item1;
            }
            else if (LAYERXORDER.CENTER == LayerPairs[i].Item1.Order)
            {
                CenterLayer = LayerPairs[i].Item1;
            }
            else if (LAYERXORDER.RIGHT == LayerPairs[i].Item1.Order)
            {
                RightLayer = LayerPairs[i].Item1;
            }
            //2번 Layer 판명
            if (LAYERXORDER.LEFT == LayerPairs[i].Item2.Order)
            {
                LeftLayer = LayerPairs[i].Item2;
            }
            else if (LAYERXORDER.CENTER == LayerPairs[i].Item2.Order)
            {
                CenterLayer = LayerPairs[i].Item2;
            }
            else if (LAYERXORDER.RIGHT == LayerPairs[i].Item2.Order)
            {
                RightLayer = LayerPairs[i].Item2;
            }
            //3번 Layer 판명
            if (LAYERXORDER.LEFT == LayerPairs[i].Item3.Order)
            {
                LeftLayer = LayerPairs[i].Item3;
            }
            else if (LAYERXORDER.CENTER == LayerPairs[i].Item3.Order)
            {
                CenterLayer = LayerPairs[i].Item3;
            }
            else if (LAYERXORDER.RIGHT == LayerPairs[i].Item3.Order)
            {
                RightLayer = LayerPairs[i].Item3;
            }
            
      
            if (CamObj.transform.position.x - CenterLayer.transform.position.x > CenterLayer.Width*0.5)//센터 레이어가 왼쪽으로 가면
            {
                
                LeftLayer.Pivot += LeftLayer.Width * 3;
                LeftLayer.Order = LAYERXORDER.RIGHT;
                CenterLayer.Order = LAYERXORDER.LEFT;
                RightLayer.Order = LAYERXORDER.CENTER;
            }

            if (CenterLayer.transform.position.x - CamObj.transform.position.x > CenterLayer.Width * 0.5)//센터 레이어가 왼쪽으로 가면
            {
                RightLayer.Pivot -= RightLayer.Width * 3;
                RightLayer.Order = LAYERXORDER.LEFT;
                LeftLayer.Order = LAYERXORDER.CENTER;
                CenterLayer.Order = LAYERXORDER.RIGHT;
            }

        }


    }
}
