using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;


public class DanMuReciver : MonoBehaviour
{
    public static DanMuReciver Instance;

    private void Awake()
    {
        Instance = this;
    }

    //弹幕接收器
    string url = "https://api.live.bilibili.com/xlive/web-room/v1/dM/gethistory?roomid=880235";
    HttpWebRequest request;


    //全局变量
    long lastReadUnix;//可能每次检测之间有多条弹幕，从上一次读取时间以后读取之后的所有弹幕
    int lastReadUid;


    public float tickInterval=5f;//轮询间隔
    //Debug
    public bool debugMode;

    public void SetRequest()
    {
        request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Accept = "application/json, text/javascript, */*; q=0.01";
        request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
    }

    public string Response()
    {
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
        {
            return reader.ReadToEnd();
        }

    }
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ReciveDanMu());
        DontDestroyOnLoad(gameObject);
    }

    public void ParseDanMu(ResponseResult ret)
    {
        //从头读取每条弹幕，直到时间大于上次读取时间，
        for(int i = 0; i < ret.data.room.Count; i++)
        {
            //从头读取每条弹幕，直到时间大于上次读取时间，
            string time= ret.data.room[i].timeline;
            int uid = ret.data.room[i].uid;
            long unix = GetUnixTime(time);
            if (unix > lastReadUnix || unix>=lastReadUnix && uid!=lastReadUid)//时间大于上一次读取弹幕的时间或者uid不同
            {
                string name = ret.data.room[i].nickname;
                string text = ret.data.room[i].text;
                string str = name + " [" + time + "]: " + text;
                Debug.Log(name + " [" + time + "]: " + text);
                EventCenter.Broadcast(EnumEventType.OnDanMuReceived,name,uid,time,text);
                //更新上一次读取的弹幕时间
                lastReadUnix = unix;
                lastReadUid = uid;
                    
            }
        }
    }

    IEnumerator ReciveDanMu()
    {
        while (true) { 
            SetRequest();
            string json = Response();

            if(debugMode)
                Debug.Log(json);

            ResponseResult ret = JsonMapper.ToObject<ResponseResult>(json);

            ParseDanMu(ret);
            
            yield return new WaitForSeconds(tickInterval);
        }
    }

    /// <summary>
    /// 扩展方法, 本地时间转Unix时间; (如 本地时间 "2020-01-01 20:20:10" 转换unix后等于 1577881210)
    /// </summary>
    /// <param name="time">本地时间</param>
    /// <returns>基于秒的10位数</returns>
    public long GetUnixTime(string timeStr)
    {
        DateTime time = Convert.ToDateTime(timeStr);
        return time.ToUniversalTime().Ticks / 10000000 - 62135596800;
    }

    public void SendFakeDanMu(string nickName,int uid,string text)
    {
        string time = DateTime.Now.ToString();
        EventCenter.Broadcast(EnumEventType.OnDanMuReceived,nickName,uid,time,text);
        
    }
    
   /* // <summary>
    /// 将Unix时间戳转换为dateTime格式
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public  DateTime UnixTimeToDateTime(long time)
    {
        if (time < 0)
            throw new ArgumentOutOfRangeException("time is out of range");
 
        return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(time);
    }*/
}


public class ResponseResult{
    public int code;
    public DanMuJson data;
}

public class DanMuJson
{
    public List<Admin> admin;
    public List<Room> room;
}

public class Admin
{
    public string text;
    public int uid;
    public string nickname;
    public string timeline;
}

public class Room
{
    public string text;
    public int uid;
    public string nickname;
    public string timeline;
}