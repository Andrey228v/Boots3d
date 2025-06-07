using TMPro;
using UnityEngine;

namespace Assets.Scripts.BasesObjects
{
    public class BaseUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textCountWorker;
        [SerializeField] private TextMeshProUGUI _textCountResurses;

        public void SetCountWorker(int count)
        {
            _textCountWorker.text = count.ToString();
        }

        public void SetCountResurses(int count) 
        {
            _textCountResurses.text = count.ToString();
        }
    }
}
