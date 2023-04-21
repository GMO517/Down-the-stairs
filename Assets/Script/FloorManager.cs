
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
  [SerializeField] GameObject[] floorPreFabs;
  public void SpawnFloor()
  {
    int r = Random.Range(0, floorPreFabs.Length);
    GameObject floor = Instantiate(floorPreFabs[r], transform);
    //後面參數用於將物件生成在父物件底下
    floor.transform.position = new Vector3((Random.Range(-3.8f, 3.8f)), -6f, 0f);
  }
}
