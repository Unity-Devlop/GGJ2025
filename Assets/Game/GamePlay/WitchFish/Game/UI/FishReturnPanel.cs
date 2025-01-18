using System.Collections;
using System.Collections.Generic;
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

    void OnSendFishDie(EventFishDiePush push)
    {
        if (panel.childCount > 0)
        {
            Destroy( panel.GetChild(panel.childCount - 1).gameObject);
        }
    }
    private void OnDestroy()
    {
        Core.Event.UnListen<EventFishDiePush>(OnSendFishDie);
    }
}
