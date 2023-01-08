using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePageScrollView : PageScrollView
{
    #region 字段
    private Transform[] items;//存储content下的子物体

    private float currentScale = 1f;//当前页大小
    private float otherScale = 0.6f;//其他页大小

    private int lastPage;//上一页
    private int nextPage;//下一页
    private float percent= 0;//百分比
    private float rotation = 30;//旋转度数
    #endregion


    #region 回调函数
    void Awake()
    {
        base.Awake();
        items = new Transform[Item_num];
        for (int i = 0; i < Item_num; i++)
        {
            items[i] = transform.Find("Viewport/Content").GetChild(i);
        }

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        ListenerScale();
        ListenerRotation();
    }
    #endregion

    #region 方法
    /// <summary>
    /// 监听缩放
    /// </summary>
    public void ListenerScale()
    {
        //找到当前位置的上一页和下一页
        for(int i=0;i<Page_Pos.Length;i++)
        {
            if(Page_Pos[i]<=rect.horizontalNormalizedPosition)
            {
                lastPage = i;
            }
        }

        for(int i=0;i<Page_Pos.Length;i++)
        {
            //某一页水平分量比当前位置水平分量大时退出循环
            if(Page_Pos[i]>rect.horizontalNormalizedPosition)
            {
                nextPage = i;
                break;
            }
        }
        //首末不变
        if(lastPage==nextPage)
        {
            return;
        }
        percent = (rect.horizontalNormalizedPosition - Page_Pos[lastPage]) / (Page_Pos[nextPage] - Page_Pos[lastPage]);
        items[lastPage].localScale = Vector3.Lerp(Vector3.one * currentScale, Vector3.one * otherScale, percent);
        items[nextPage].localScale = Vector3.Lerp(Vector3.one * currentScale, Vector3.one * otherScale, 1 - percent);

        //除了当前页大小为1外 其余均为0.6
        for(int i=0;i<items.Length;i++)
        {
            if(i!=lastPage&&i!=nextPage)
            {
                items[i].localScale = Vector3.one * otherScale;
            }
        }
    }

    /// <summary>
    /// 监听旋转
    /// </summary>
    public void ListenerRotation()
    {

        if(lastPage==nextPage)
        {
            return;
        }
        items[lastPage].localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero,new Vector3(0,-rotation,0),percent));
        items[nextPage].localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, new Vector3(0, rotation, 0), 1-percent));
        
        //其他页
        for(int i=0;i<items.Length;i++)
        {
            if(i!=lastPage&&i!=nextPage)
            {
                //当前页左边 逆时针旋转rotation度
                if (i < currentPage)
                {
                    items[i].rotation = Quaternion.Euler(0, -rotation, 0);
                }
                //当前页
                if (i == currentPage)
                {
                    items[i].rotation = Quaternion.Euler(0, 0, 0);
                }
                //当前页右边
                if (i > currentPage)
                {
                    items[i].rotation = Quaternion.Euler(0, rotation, 0);
                }
            }  
        }
    }
    #endregion
}
