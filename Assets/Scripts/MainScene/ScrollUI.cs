using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollUI : MonoBehaviour
{

    public ScrollRect scrollRect;
    // Start is called before the first frame update
    void Start()
    {
        scrollRect.content.anchoredPosition = new Vector2(372, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
