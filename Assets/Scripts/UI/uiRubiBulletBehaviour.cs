using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiRubiBulletBehaviour : MonoBehaviour
{
    public List<Sprite> bullets;
    public Sprite emptyBullet;
    public List<Color> testColors;
        
    public void SetNewBullet(int _choice)
    {
        Sprite _idealBullet = bullets[_choice];
        GetComponent<Image>().sprite = _idealBullet;
    }
    public void RemoveBullet()
    {
        Sprite _idealBullet = emptyBullet;
        GetComponent<Image>().sprite = _idealBullet;
    }
}
