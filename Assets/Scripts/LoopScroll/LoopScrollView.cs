using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopScrollView : MonoBehaviour
{

    #region 字段

    private GameObject ChildItemPrefab;//子物体预制体
    private GridLayoutGroup contentLayout;
    private ContentSizeFitter contentSizeFitter;
    private RectTransform content;
    private DataManager dataManager;

    #endregion

    #region 回调函数

    void Awake()
    {
        contentLayout = transform.Find("Viewport/Content").GetComponent<GridLayoutGroup>();
        contentSizeFitter = transform.Find("Viewport/Content").GetComponent<ContentSizeFitter>();
        ChildItemPrefab = Resources.Load<GameObject>("Prefabs/Item/Item1");
        content = transform.Find("Viewport/Content").GetComponent<RectTransform>();
        dataManager = new DataManager();

        //测试数据
        List<LoopDataItem> loopDataItems = new List<LoopDataItem>();

        for(int i=1;i<=40;i++)
        {
            string itemName = "item" + i.ToString();
            loopDataItems.Add(new LoopDataItem(i,itemName));
        }
        dataManager.InitData(loopDataItems);

    }
    // Start is called before the first frame update
    void Start()
    {
        OnAddHead();
        Invoke("enabledContent", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region 方法
    GameObject GetChildItem()
    {
        //查找空闲物体  
        for(int i=0;i<content.childCount;i++)
        {
            GameObject item=content.GetChild(i).gameObject;
            //物体状态没激活  说明其被回收 可以直接拿来用
            if(!item.activeSelf)
            {
                item.SetActive(true);
                return item;
            }
        }
        //暂无可用的物体  创建一个
        GameObject childItem = GameObject.Instantiate(ChildItemPrefab, content.transform);
        //初始化数据
        childItem.transform.localScale = Vector3.one;
        childItem.transform.localPosition = Vector3.zero;
        childItem.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        childItem.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        childItem.GetComponent<RectTransform>().sizeDelta = contentLayout.cellSize;
        LoopItem loopItem=childItem.GetComponent<LoopItem>();
        //绑定事件
        loopItem.OnAddHead += OnAddHead;
        loopItem.OnRemoveHead += OnRemoveHead;
        loopItem.OnAddLast += OnAddLast;
        loopItem.OnRemoveLast += OnRemoveLast;
        return childItem;
    }

    //添加头
    void OnAddHead()
    {
        LoopDataItem loopDataItem = dataManager.GetHeadData();
        if(loopDataItem!=null)
        {
            Transform first = FindFirst();//找到当前头部后在添加物体在当前头部前面

            GameObject obj = GetChildItem();//获取一个物体
            obj.transform.SetAsFirstSibling();//设为首部
            //设置数据
            SetData(obj, loopDataItem);
            //设置位置 
            if (first != null)
            {
                obj.transform.localPosition = first.localPosition
                    + new Vector3(0, contentLayout.cellSize.y + contentLayout.spacing.y);
            }
        }
    }

    //去除头
    void OnRemoveHead()
    {
        if(dataManager.RemoveHeadData())
        {
            Transform first = FindFirst();
            if (first != null)
            {
                first.gameObject.SetActive(false);
            }
        }
    }

    //添加尾
    void OnAddLast()
    {
        LoopDataItem loopDataItem = dataManager.GetLastData();
        if(loopDataItem!=null)
        {
            Transform last = FindLast();//找到当前头部后在添加物体在当前头部前面

            GameObject obj = GetChildItem();//获取一个物体
            obj.transform.SetAsLastSibling();//设为尾部
            //设置数据
            SetData(obj, loopDataItem);
            //设置位置 
            if (last != null)
            {
                obj.transform.localPosition = last.localPosition
                    - new Vector3(0, contentLayout.cellSize.y + contentLayout.spacing.y, 0);
            }
            //判断要不要添加content高度
            if (IsNeedAddContentHeight(obj.transform))
            {
                //增加高度
                content.sizeDelta += new Vector2(0, contentLayout.cellSize.y + contentLayout.spacing.y);
            }
        }
        
    }

    //去除尾
    void OnRemoveLast()
    {
        if(dataManager.RemoveLastData())
        {
            Transform last = FindLast();
            if (last != null)
            {
                last.gameObject.SetActive(false);
            }
        }  
    }

    //找到目前首部物体
    Transform FindFirst()
    {
        for(int i=0;i<content.childCount;i++)
        {
            GameObject item = content.GetChild(i).gameObject;
            if(item.activeSelf)
            {
                return item.transform;
            }
        }
        return null;
    }

    //找到目前尾部物体
    Transform FindLast()
    {
        for (int i =content.childCount-1; i >=0; i--)
        {
            GameObject item = content.GetChild(i).gameObject;
            if (item.activeSelf)
            {
                return item.transform;
            }
        }
        return null;
    }

    //禁用content组件
    void enabledContent()
    {
        contentLayout.enabled = false;
        contentSizeFitter.enabled = false;
    }

    //判断要不要增加content高度
    bool IsNeedAddContentHeight(Transform item)
    {
        Vector3[] itemCorners=new Vector3[4];
        Vector3[] contentCorners=new Vector3[4];
        //获取到尾部物体和content的四个角坐标 比较坐标
        item.GetComponent<RectTransform>().GetWorldCorners(itemCorners);
        content.GetComponent<RectTransform>().GetWorldCorners(contentCorners);
        if(itemCorners[0].y<contentCorners[0].y)
        {
            return true;
        }
        return false;
    }

    //设置数据
    public void SetData(GameObject childItem,LoopDataItem data)
    {
        childItem.transform.Find("Text").GetComponent<Text>().text = "物品"+data.id.ToString();
        childItem.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/" + data.itemName);
    }
    #endregion
}
