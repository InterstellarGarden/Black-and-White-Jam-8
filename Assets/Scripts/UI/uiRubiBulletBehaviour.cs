using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiRubiBulletBehaviour : MonoBehaviour
{
    public List<Sprite> bullets;
    public List<Color> testColors;
        
    public void SetNewBullet(int _choice)
    {
        //To be replaced with bullet sprite
        Color _bulletColor = GetComponent<Image>().color;
        _bulletColor = testColors[_choice];
        _bulletColor.a = 1;
        GetComponent<Image>().color = _bulletColor;
    }
    public void RemoveBullet()
    {
        //To be replaced with empty bullet sprite
        Color _bulletColor = GetComponent<Image>().color;
        _bulletColor.a = 0;
        GetComponent<Image>().color = _bulletColor;
    }
}
