using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    [SerializeField] private GameObject _fullHeartPrefab;
    [SerializeField] private GameObject _emptyHeartPrefab;
    private int _maxHealth;
    private List<GameObject> _hearts = new List<GameObject>();

    public void SetMaxHealth(int maxHP)
    {
        _maxHealth = maxHP;
        foreach (var heart in _hearts) Destroy(heart);
        _hearts.Clear();

        for (int i = 0; i < _maxHealth; i++)
        {
            GameObject heart = Instantiate(_emptyHeartPrefab, transform);
            _hearts.Add(heart);
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < _hearts.Count; i++)
        {
            Destroy(_hearts[i]);

            GameObject heart = Instantiate(
                i < currentHealth ? _fullHeartPrefab : _emptyHeartPrefab, 
                transform
            );
            _hearts[i] = heart;
        }
    }
}
