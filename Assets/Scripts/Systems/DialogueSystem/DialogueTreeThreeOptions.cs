using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(menuName = "Dialogue/DialogueTreeThreeOptions")]

public class DialogueTreeThreeOptions : DialogueTreeBaseObject
{
    private int depth = 0;
    public void ResetDepth()
    {
        depth = 0;
    }
    public override void Traverse(int _direction)
    {
        if(depth == 0) depth++;
        currentID *= 2;
        if (_direction == 0) { currentID += depth; return; }
        if (_direction == 1) { currentID += (depth * 2); return; }
        currentID += 0;


    }

}

