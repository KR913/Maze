using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MazeGame
{
    [RequireComponent(typeof(ScrollRect))]
    public class DynamicScrollView : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private GameObject _item;
        [SerializeField] private float _itemWidth;
        private int _childCount;
        // Start is called before the first frame update
        void Start()
        {
            _childCount = _content.childCount;
            AdjustScrollView(_childCount);
        }

        private void AdjustScrollView(int count)
        {
            _content.sizeDelta = new Vector2(count * _itemWidth, _content.sizeDelta.y);
        }

        public void AddScore(Sprite ic)
        {
            _childCount += 1;
            AdjustScrollView(_childCount);
            var getItem = Instantiate(_item, _content);
            getItem.transform.GetChild(0).GetComponent<Image>().sprite = ic;
        }
    }
}
