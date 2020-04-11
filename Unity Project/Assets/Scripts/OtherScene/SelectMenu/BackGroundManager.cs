using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class BackGroundManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            // スクロールの速度をセット
            var townScrollView = transform.Find("Town/Scroll View").gameObject;
            var backGoundScroll = townScrollView.GetComponent<BackGroundScroll>();
            backGoundScroll.ScrollSpeed = 0.2f;
        }
    }
}
