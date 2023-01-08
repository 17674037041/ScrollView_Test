using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//��ҳ���� ˮƽ�Լ���ֱ
public enum PageType
{
    Horizontal,
    Vertical
}

public class PageScrollView : MonoBehaviour, IEndDragHandler,IBeginDragHandler
{
    #region �ֶ�
    private RectTransform Content_Rect;//Content������ʾ���е�Rect���
    public int Item_num;//Content������ʾ���µ�Item����
    public float[] Page_Pos;//ÿ��Item��Ӧ��ˮƽ����
    public int currentPage = 0;//��ǰ����ҳ
    protected ScrollRect rect;
    private float movetimer = 0;//�ƶ���ʱ��
    private float startMovePos;//��ʼ�ƶ���ˮƽ����
    private float AutoMoveTime = 2;//�������ƶ�һ��
    private float AutoMoveTimer = 0;//�Զ��ƶ�ʱ���ʱ

    private bool isMoving = false;//�Ƿ������ƶ�
    public bool isAutoMove = false;//�Ƿ��Զ��ƶ�
    private bool isDraging = false;//�Ƿ�������ק

    public PageType pageType = PageType.Horizontal;//Ĭ��Ϊˮƽ����
    public float re = 0;
    #endregion

    #region �ص�����
    protected void Awake()
    {
        rect = transform.GetComponent<ScrollRect>();
        Content_Rect = transform.Find("Viewport/Content").GetComponent<RectTransform>();
        Item_num = Content_Rect.childCount;
        Debug.Log(Item_num);
        if(Item_num==1)
        {
            throw new System.Exception("һҳ�����÷�ҳ");
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
                AutoMoveTimer = 0;//��ʱ����
            }
        }
        PageMove();
        re = rect.horizontalNormalizedPosition;
    }

    /// <summary>
    /// ��ʼ��ק
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDraging = true;//������ק
    }

    /// <summary>
    /// ��ק����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        isDraging = false;
        AutoMoveTimer = 0;//��ק�����Զ���ҳʱ���ʱ������
        float ii = (float)1 / (Item_num - 1);//�����ҳ��ƽ��ˮƽ����
        switch(pageType)
        {
            case PageType.Horizontal:
                currentPage = (int)(rect.horizontalNormalizedPosition / ii);//��ק����ҳ��ͣ����Ԥ��λ��
                break;
            case PageType.Vertical:
                currentPage = (int)(Item_num-1-rect.verticalNormalizedPosition / ii);//��ק����ҳ��ͣ����Ԥ��λ��
                break;
            default:
                break;
        }
        
        //ҳ��û��ͣ�������һҳ  �ж�ǰ�����λ�ô�С
        if(currentPage<Item_num-1)
        {
            switch(pageType)
            {
                case PageType.Horizontal:
                    //ǰһ��λ������ڵ�ǰͣ��λ�õ�ˮƽ����֮��ͺ�һ��λ�����Ƚ�  ����ȡСֵ
                    currentPage = Mathf.Abs((Page_Pos[currentPage] - rect.horizontalNormalizedPosition))
                    < Mathf.Abs((Page_Pos[currentPage + 1] - rect.horizontalNormalizedPosition)) ? currentPage : currentPage + 1;
                    break;
                case PageType.Vertical:
                    //ǰһ��λ������ڵ�ǰͣ��λ�õ�ˮƽ����֮��ͺ�һ��λ�����Ƚ�  ����ȡСֵ
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

    #region ����
    /// <summary>
    /// ����ҳ���ƶ�
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
    /// ��ʼ���ƶ�����
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
