using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AccountUI : MonoBehaviour
{
    public Image faceImg;
    public Image topBg;

    public TMP_Text nickName;

    public TMP_Text winRate;
    public Image fillImage;
    public TMP_Text countDownText;

    private void Start()
    {
        fillImage.fillAmount = 0;
        countDownText.text = "";
        nickName.text = "大爷快来玩呀";
        winRate.text = "";
    }

    public void OnPlayerJoined(Player player)
    {
        nickName.text = player.userName;
        StartCoroutine(DownSprite(player.faceUrl,faceImg));
        StartCoroutine(DownSprite(player.top_photo ,topBg));
        winRate.text = "胜率：5/10";

    }
    
    IEnumerator DownSprite(string url,Image image)
    {
        UnityWebRequest wr = new UnityWebRequest(url);
        DownloadHandlerTexture texD1 = new DownloadHandlerTexture(true);
        wr.downloadHandler = texD1;
        yield return wr.SendWebRequest();
        int width = 1920;
        int high = 1080;
        if (!wr.isNetworkError)
        {
            Texture2D tex = new Texture2D(width, high);
            tex = texD1.texture;
            //保存本地          
            Byte[] bytes = tex.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/02.png", bytes);
             
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            image.sprite = sprite;
        }
    }
    
}
