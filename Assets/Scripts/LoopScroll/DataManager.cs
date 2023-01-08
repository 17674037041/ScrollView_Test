using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    #region 字段
    public List<LoopDataItem> allData = new List<LoopDataItem>();//保存所有数据
    public LinkedList<LoopDataItem> currentShowData = new LinkedList<LoopDataItem>();//当前显示的数据

    #endregion


    #region 方法
    //获取头部数据
    public LoopDataItem GetHeadData()
    {
        if(allData.Count==0)
        {
            return null;
        }

        //当前显示的数据暂无任何数据
        if(currentShowData.Count==0)
        {
            LoopDataItem head = allData[0];
            currentShowData.AddFirst(head);
            return head;
        }

        //当前显示数据的上一个数据
        LoopDataItem obj = currentShowData.First.Value;
        //找到当前数据在总数据中的位置
        int index = allData.IndexOf(obj);
        if(index!=0)
        {
            LoopDataItem head = allData[index - 1];
            //加到当前显示的数据里面
            currentShowData.AddFirst(head);
            return head;
        }
        return null;
    }

    //移除头部数据
    public bool RemoveHeadData()
    {
        //当前没有任何数据 不可移除
        if(currentShowData.Count==0)
        {
            return false;
        }
        //移除当前正在显示数据的第一个数据
        currentShowData.RemoveFirst();
        return true;
    }

    //获取尾部数据
    public LoopDataItem GetLastData()
    {
        if (allData.Count == 0)
        {
            return null;
        }

        //当前显示的数据暂无任何数据
        if (currentShowData.Count == 0)
        {
            LoopDataItem last = allData[0];
            currentShowData.AddLast(last);
            return last;
        }
        //获取当前正在显示数据的下一个数据
        LoopDataItem obj = currentShowData.Last.Value;
        int index = allData.IndexOf(obj);
        //判断该数据是否为总的数据的最后一个数据  若是则获取不到数据了 所有数据都被加载了
        if(index!=allData.Count-1)
        {
            LoopDataItem last=allData[index+1];
            currentShowData.AddLast(last);
            return last;
        }
        return null;
    }

    //移除尾部数据
    public bool RemoveLastData()
    {
        //移除当前正在显示数据的最后一个数据
        if(currentShowData.Count==0)
        {
            return false;
        }
        currentShowData.RemoveLast();
        return true;
    }

    public void InitData(LoopDataItem[] obj)
    {
        allData.Clear();
        currentShowData.Clear();
        allData.AddRange(obj);
    }

    public void InitData(List<LoopDataItem> obj)
    {
        InitData(obj.ToArray());
    }

    public void AddData(LoopDataItem[] obj)
    {
        allData.AddRange(obj);
    }

    public void AddData(List<LoopDataItem> obj)
    {
        AddData(obj.ToArray());
    }
    #endregion

    
}
