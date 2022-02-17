using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class CountDownDialogContext : DialogContext
{
    public int duration;
    
    public CountDownDialogContext(int duration)
    {
        this.duration = duration;
    }
}

public class CountDownDialog : Dialog<CountDownDialogContext>
{
    public Text text;
     
    public static void ShowDialog(int duration)
    {
        var dialog = GetShowingDialog(nameof(CountDownDialog)) as CountDownDialog;
        if (dialog != null)
        {
            return;
        }

        DialogUtil.ShowDialogWithContext(nameof(CountDownDialog), new CountDownDialogContext(duration));
    }

    public override void Show(Action onComplete)
    {
        
        base.Show(onComplete);
        UnityTimer.Timer.Register(dialogContext.duration, ()=>{Close();}, (time) =>
        {
            text.text = (dialogContext.duration - (int) time)+"";
        } );
    }
}
