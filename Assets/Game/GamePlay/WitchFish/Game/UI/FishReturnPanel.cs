using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WitchFish;

public class FishReturnPanel : MonoBehaviour
{
    Transform panel;
    // Start is called before the first frame update
    void Start()
    {
        panel = transform.Find("Panel");
        var prefab = transform.Find("Presets/FishReturnItem").gameObject;
        for(int i = 0;i<3;i++) {
            var itemObj = Instantiate(prefab, panel);
        }

        Core.Event.Listen<EventFishDiePush>(OnSendFishDie);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void  OnSendFishDie(EventFishDiePush push)
    {
        if (panel.childCount > 0)
        {
            panel.GetChild(0).gameObject.GetComponent<Animator>().enabled = true;
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f), cancellationToken: destroyCancellationToken);
            Destroy( panel.GetChild(panel.childCount - 1).gameObject);
        }
    }
    private void OnDestroy()
    {
        Core.Event.UnListen<EventFishDiePush>(OnSendFishDie);
    }
}
