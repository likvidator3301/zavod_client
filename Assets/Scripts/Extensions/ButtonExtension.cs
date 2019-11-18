using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    public static class ButtonExtension
    {
        public static Bounds GetButtonBounds(this Button button)
        {
            return new Bounds(new Vector3(button.transform.position.x,
                                          button.transform.position.y, 0),
                              new Vector3(button.GetComponent<RectTransform>().rect.width,
                                          button.GetComponent<RectTransform>().rect.height));
        }
    }
}
