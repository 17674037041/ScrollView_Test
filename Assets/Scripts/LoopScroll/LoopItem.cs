using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopItem : MonoBehaviour
{
    #region 字段
    public RectTransform rect;
    public RectTransform viewRect;
    public Vector3[] rectCorners;
    public Vector3[] viewCorners;
    public bool isf;

    #endregion

    #region 委托、事件
    public Action OnAddHead;
    public Action OnRemoveHead;
    public Action OnAddLast;
    public Action OnRemoveLast;

    #endregion

    #region 回调函数
    void Awake()
    {
        viewRect = GameObject.Find("Canvas/Scroll View").GetComponent<RectTransform>();
        rectCorners = new Vector3[4];
        viewCorners = new Vector3[4];
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ListenerCorners();
    }
    #endregion

    #region 方法
    void ListenerCorners()
    {
        rect.GetWorldCorners(rectCorners);
        viewRect.GetWorldCorners(viewCorners);

        //为首部  判断是否增删头部
        if(isFirst())
        {
            //自身超出scroll显示范围  删除头部
            if(rectCorners[0].y>viewCorners[1].y)
            {
                if (OnRemoveHead != null)
                {
                    OnRemoveHead();
                }
            } 
            //自身在scroll显示范围内  添加头部
            if(rectCorners[1].y<viewCorners[1].y)
            {
                if (OnAddHead != null)
                {
                    OnAddHead();                   
                }
            }
        }

        //为尾部  判断是否增删尾部
        if(isLast())
        {
            //自身超出scroll显示范围  删除尾部
            if (rectCorners[1].y < viewCorners[0].y)
            {
                if(OnRemoveLast!=null)
                {
                    OnRemoveLast();
                }
            }
            //自身在scroll显示范围内  添加尾部
            if (rectCorners[0].y > viewCorners[0].y)
            {
                if(OnAddLast!=null)
                {
                    OnAddLast();
                }
            }
        }
    }

    /// <summary>
    /// 判断是否为末尾物体
    /// </summary>
    bool isLast()
    {
        for (int i = transform.parent.childCount-1; i >= 0 ;i-- )
        {
            GameObject item = transform.parent.GetChild(i).gameObject;//下标i所对应的子物体
            //下标i所对应的物体为激活状态
            if (item.activeSelf)
            {
                //第一个状态为激活的物体和自身相等说明自身为尾部  返回true
                if (item == transform.gameObject)
                {
                    return true;
                }
                break;//首个激活状态的物体不是自身  那么自身不可能为尾部  直接退出循环
            }
        }
        return false;
    }

    /// <summary>
    /// 判断是否首部物体
    /// </summary>
    bool isFirst()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            GameObject item = transform.parent.GetChild(i).gameObject;//下标i所对应的子物体
            //下标i所对应的物体为激活状态
            if(item.activeSelf)
            {
                //第一个状态为激活的物体和自身相等说明自身为首部  返回true
                if(item==transform.gameObject)
                {
                    isf = true;
                    return true;
                }
                break;//首个激活状态的物体不是自身  那么自身不可能为首部  直接退出循环
            }
        }
        isf = false;
        return false;
    }
    #endregion


}
