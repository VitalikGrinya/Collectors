using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitMove : MonoBehaviour
{
    public void Move(Transform target, float speed)
    {
        transform.position += (target.position - transform.position).normalized * speed * Time.deltaTime;
    }
}
