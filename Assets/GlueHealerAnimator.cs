using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueHealerAnimator : QuizAnimator
{
    private GlueHealer healer;
    private LineRenderer line;

    void Start()
    {
        base.Start();

        healer = GetComponent<GlueHealer>();
        line = GetComponentInChildren<LineRenderer>();
    }

    protected override void Update()
    {
        base.Update();
        if (healer.target)
        {
            line.enabled = true;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, healer.target.transform.position);
        }
        else
        {
            line.enabled = false;
        }
    }
}
