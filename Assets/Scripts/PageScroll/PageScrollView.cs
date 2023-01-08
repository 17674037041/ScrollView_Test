using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//分页类型 水平以及垂直
public enum PageType
{
    Horizontal,
    Vertical
}

public class PageScrollView : MonoBehaviour, IEndDragHandler,IBeginDragHandler
{
    #region 字段
    private RectTransform Content_Rect;//Content内容显示框中的Rect组件
    public int Item_num;//Content内容显示框下的Item数量
    public float[] Page_Pos;//每个Item对应的水平分量
    public int currentPage = 0;//当前所在页
    protected ScrollRect rect;
    private float movetimer = 0;//移动计时器
    private float startMovePos;//开始移动的水平分量
    private float AutoMoveTime = 2;//隔两秒移动一次
    private float AutoMoveTimer = 0;//自动移动时间计时

    private bool isMoving = false;//是否正在移动
    public bool isAutoMove = false;//是否自动移动
    private bool isDraging = false;//是否正在拖拽

    public PageType pageType = PageType.Horizontal;//默认为水平滑动
    public float re = 0;
    #endregion

    #region 回调函数
    protected void Awake()
    {
        rect = transform.GetComponent<ScrollRect>();
        Content_Rect = transform.Find("Viewport/Content").GetComponent<RectTransform>();
        Item_num = Content_Rect.childCount;
        Debug.Log(Item_num);
        if(Item_num==1)
        {
            throw new System.Exception("一页，不用分页");
        }
        Page_Pos = new float[Item_num];
        for(int i=0;i<Item_num;i++)
        {
            switch(pageType)
            {
                case PageType.Horizontal:
                    Page_Pos[i] = i * ((float)1 / (Item_num - 1));
                    break;
                case PageType.Vertical:
                    Page_Pos[i] = 1-i * ((float)1 / (Item_num - 1));
                    break;
                default:
                    break;
            }
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        if (isAutoMove&&!isDraging)
        {
            AutoMoveTimer += Time.deltaTime;
            if (AutoMoveTimer >= AutoMoveTime)
            {
                currentPage = ++currentPage % Item_num;
                ScrollToPage(currentPage);
                AutoMoveTimer = 0;//计时清零
            }
        }
        PageMove();
        re = rect.horizontalNormalizedPosition;
    }

    /// <summary>
    /// 开始拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDraging = true;//正在拖拽
    }

    /// <summary>
    /// 拖拽结束
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        isDraging = false;
        AutoMoveTimer = 0;//拖拽结束自动分页时间计时器归零
        float ii = (float)1 / (Item_num - 1);//计算出页面平均水平分量
        switch(pageType)
        {
            case PageType.Horizontal:
                currentPage = (int)(rect.horizontalNormalizedPosition / ii);//拖拽结束页面停留的预估位置
                break;
            case PageType.Vertical:
                currentPage = (int)(Item_num-1-rect.verticalNormalizedPosition / ii);//拖拽结束页面停留的预估位置
                break;
            default:
                break;
        }
        
        //页面没有停留在最后一页  判断前后相对位置大小
        if(currentPage<Item_num-1)
        {
            switch(pageType)
            {
                case PageType.Horizontal:
                    //前一个位置相对于当前停留位置的水平分量之差和后一个位置做比较  两者取小值
                    currentPage = Mathf.Abs((Page_Pos[currentPage] - rect.horizontalNormalizedPosition))
                    < Mathf.Abs((Page_Pos[currentPage + 1] - rect.horizontalNormalizedPosition)) ? currentPage : currentPage + 1;
                    break;
                case PageType.Vertical:
                    //前一个位置相对于当前停留位置的水平分量之差和后一个位置做比较  两者取小值
                    currentPage = Mathf.Abs((Page_Pos[currentPage] - rect.verticalNormalizedPosition))
                    < Mathf.Abs((Page_Pos[currentPage + 1] - rect.verticalNormalizedPosition)) ? currentPage : currentPage + 1;
                    break;
                default:
                    break;
            }
        }  
        ScrollToPage(currentPage);
    }
    #endregion

    #region 方法
    /// <summary>
    /// 负责页面移动
    /// </summary>
    void PageMove()
    {
        if (isMoving)
        {
            movetimer += Time.deltaTime * 3;
            switch(pageType)
            {
                case PageType.Horizontal:
                    rect.horizontalNormalizedPosition = Mathf.Lerp(startMovePos, Page_Pos[currentPage], movetimer);
                    break;
                case PageType.Vertical:
                    rect.verticalNormalizedPosition = Mathf.Lerp(startMovePos, Page_Pos[currentPage], movetimer);
                    break;
                default:
                    break;
            }    
            if (movetimer >= 1)
            {
                isMoving = false;
            }
        }
    }

    /// <summary>
    /// 初始化移动数据
    /// </summary>
    /// <param name="page"></param>
    void ScrollToPage(int page)
    {

        isMoving = true;
        currentPage = page;
        movetimer = 0;
        switch(pageType)
        {
            case PageType.Horizontal:
                startMovePos = rect.horizontalNormalizedPosition;
                break;
            case PageType.Vertical:
                startMovePos = rect.verticalNormalizedPosition;
                break;
            default:
                break;
        } 
    }
    #endregion



}
