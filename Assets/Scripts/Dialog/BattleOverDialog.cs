using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleOverDialogContext : DialogContext
{
    public int duration;
    public Player player;
    

    public BattleOverDialogContext(int duration, Player player) 
    {
        this.duration = duration;
        this.player = player;
        
    }
}
public class BattleOverDialog : Dialog<BattleOverDialogContext>
{
    public TMP_Text winnerText;
    public Image face;
    public TMP_Text countDownText;
    
    public static void ShowDialog(int duration,Player player, Action onClose)
    {
        var dialog = GetShowingDialog(nameof(BattleOverDialog)) as BattleOverDialog;
        if (dialog != null)
        {
            return;
        }

        DialogUtil.ShowDialogWithContext(nameof(BattleOverDialog), new BattleOverDialogContext(duration, player),null,onClose);
    }

    public override void Show()
    {
        base.Show();
        StartCoroutine(CountDown(dialogContext.duration));
        AccountUI accountUi = GameManager.Instance.uiManager.GetAccountUiByPlayer(dialogContext.player);
        winnerText.text = "胜者: " + accountUi.player.userName;
        face.sprite = accountUi.faceImg.sprite;
    }
    
   

    IEnumerator CountDown(int duration)
    {
        int count = duration;
        while (count > 0)
        {
            
            countDownText.text = count+"秒后开始下一局游戏";
            count--;
            yield return new WaitForSeconds(1);
            
        }
        Close();
        
    }
}
