using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/SocketQuest", order = 1)]
public class SocketQuest : Quest
{
    public Mod rune;

    public void Socket(Mod _rune)
    {
        if (_rune.name == rune.name)
        {
            ProgressQuest();
        }
    }

    public void ProgressQuest()
    {
        TutorialTracking.instance.ProgressTutorial();
    }
}
