using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class GuestMediator : MonoBehaviour
    {
        private float SECONDS = 1f;

        public SandboxRoom[] Rooms;

        private Image _image;
        private int _position = 0;
        private RectTransform _xForm;

        private Vector3 _Position
        {
            get => _xForm.position;
            set => _xForm.position = value;
        }

        public void Visit()
        {
            if (_image.color == Color.gray)
            {
                _image.color = Util.RNG.Generate(3) == 0 ? Color.red : Color.white;
            }
        }

        public void NextRoom()
        {
            _position = (_position + 1) % Rooms.Length;
            Rooms[_position].AddGuest(this);
        }

        public void GoRoom(Vector3 position)
        {
            StopAllCoroutines();
            StartCoroutine(GoCoords(position));
        }

        void Awake()
        {
            _image = GetComponent<Image>();
            _xForm = GetComponent<RectTransform>();
            _image.color = Color.gray;
            _position = 0;
        }

        private IEnumerator GoCoords(Vector3 position)
        {
            Vector3 start = transform.position;
            Vector3 dir = position - start;
            for (float f=0; f < SECONDS; f+=Time.deltaTime)
            {
                _Position = start + (dir * f / SECONDS);
                yield return null;
            }
            _Position = position;
        }
    }
}
