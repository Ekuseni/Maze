using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Walker : MonoBehaviour
{
    [SerializeField]
    float speed;

    class WalkTask
    {
        public Vector2Int Start;
        public Vector2Int End;

        public WalkTask(Vector3 start, Vector3 end)
        {
            Start = new Vector2Int((int)start.x, (int)start.z);
            End = new Vector2Int((int)end.x, (int)end.z);
        }
    }


    List<Vector3> target = new List<Vector3>();

    Queue<WalkTask> WalkTasks = new Queue<WalkTask>();

    public static Vector3 Target
    {
        set
        {
            _instance.target.Add(value);
            if (_instance.target.Count == 2)
            {
                if (!_instance.isWalking)
                {
                    StartWalk(new WalkTask(_instance.target[0], _instance.target[1]));
                    _instance.target.RemoveAt(0);
                }
                else
                {
                    _instance.WalkTasks.Enqueue(new WalkTask(_instance.target[0], _instance.target[1]));
                    _instance.target.RemoveAt(0);
                }
            }
        }
    }

    static Walker _instance;

    bool isWalking = false;

    static void StartWalk(WalkTask walkTask)
    {

        _instance.isWalking = true;

        Debug.Log("Walking from " + walkTask.Start + " to " + walkTask.End);

        Vector2Int[] path;

        Sequence sequence = DOTween.Sequence();

        if (PathSearch.FindPath(walkTask.Start, walkTask.End, out path))
        {
            for(int i = path.Length - 1; i >= 0; i--)
            {
                sequence.Append(_instance.transform.DOMove(new Vector3(path[i].x, _instance.transform.position.y, path[i].y), 1f / _instance.speed).SetEase(Ease.Linear));
            }
        }

        sequence.AppendCallback(() =>
        {
            if(_instance.WalkTasks.Count != 0)
            {
                StartWalk(_instance.WalkTasks.Dequeue());
            }
            else
            {
                _instance.isWalking = false;
            }
        });
    }

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(_instance.gameObject);
            _instance = this;
        }

        

        target.Add(transform.position);
    }


}
