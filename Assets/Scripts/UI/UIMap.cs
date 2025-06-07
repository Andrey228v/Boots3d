using Assets.Scripts.Resurses;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIMap : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textCountResurs;
        [SerializeField] private TextMeshProUGUI _textCountFreeResurs;
        [SerializeField] private TextMeshProUGUI _textRes;

        public void SetCountResurs(string text)
        {
            _textCountResurs.text = text;
        }

        public void SetCountFreeResurs(string text)
        {
            _textCountFreeResurs.text = text;
        }

        public void SetListRes(HashSet<Resurs> resurs)
        {

        }

    }
}
