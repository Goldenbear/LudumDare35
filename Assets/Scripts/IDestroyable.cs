using UnityEngine;
using System.Collections;

public interface IDestroyable {
    void TakeDamage(int amount);
    int GetHealth();
}
