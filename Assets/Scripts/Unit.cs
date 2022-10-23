
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Unit : MonoBehaviour
{
    public enum Colors//Taşların renkleri
    {
        Black,
        Blue,
        Red,
        Yellow,
    }
    public Colors color;
    public int number;//taşların numaraları
    public TMP_Text myText;
    private Vector3 LastMousePosition;
    private bool isRight, isUp;//, colliderController;
    [SerializeField] private float sensitivity = 5f;
    public bool thisSearched;
    #region SetRandomStone

    // public void SetRandomUnitData()//Taşların random olarak sayı ve ranklerini belirliyoruz
    // {
    //     color = (Colors)Random.Range(0, 4);//random renk
    //     number = Random.Range(1, 13);//random sayı
    //     myText.text = number.ToString();
    //     switch (color)
    //     {
    //         case Colors.Black:
    //             myText.color = Color.black;
    //             break;
    //         case Colors.Blue:
    //             myText.color = Color.blue;
    //             break;
    //         case Colors.Red:
    //             myText.color = Color.red;
    //             break;
    //         case Colors.Yellow:
    //             myText.color = Color.yellow;
    //             break;
    //     }
    // }

    #endregion

    private void OnMouseDown()
    {
        LastMousePosition = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        if (LevelController.Instance.isStoneInAnim) return;
        Vector3
            delta = LastMousePosition -
                    Input.mousePosition; //farenin ilk pozisyonunundan son pozisyonunun farkını alıyoruz
        bool currentIsRight = isRight;
        bool currentIsUp = isUp;

        if (delta.x > 0) //delta.x sıfırdan büyükse sağa hareket başladı
        {
            currentIsRight = true;
        }
        else if (delta.x < 0) //delta.x sıfırdan küçükse sola hareket başladı
        {
            currentIsRight = false;
        }

        if (delta.y > 0) //delta.y büyükse sıfırdan yukarı hareket başladı
        {
            currentIsUp = true;
        }
        else if (delta.y < 0) //delta.y küçükse sıfırdan aşşağı hareket başladı
        {
            currentIsUp = false;
        }

        if (currentIsRight != isRight) //sağa hareket bir anda kesilirse
        {
            LastMousePosition = Input.mousePosition; //hareketin değiştiği konumu ilk konum yapıyoruz
            isRight = currentIsRight;
        }

        if (currentIsUp != isUp)
        {
            LastMousePosition = Input.mousePosition;
            isUp = currentIsUp;
        }

        if (Mathf.Abs(delta.x) >
            Mathf.Abs(delta.y)) //sağa sola hareket yukarı aşşağı hareketten büyükse önce sağa sola yap
        {
            if (Mathf.Abs(delta.x) > sensitivity)
            {
                PlaceBrain.Instance.MoveStone(this,new Vector2(isRight ? -1 : 1, 0));
            }
        }
        else
        {
            if (Mathf.Abs(delta.y) > sensitivity)
            {
                PlaceBrain.Instance.MoveStone(this,new Vector2(0, isUp ? -1 : 1));
            }
        }
    }
    
    #region ...

    /*void MoveStone(Vector2 direction)
    {
        x = 0;
        z = 0;
        colliderController = false;
        if (direction.x < 0) //direction.x 0 dan büyükse sağa hareket
        {
            LevelController.Instance.isStoneInAnim = true;
            transform.DOMoveX(transform.position.x + 1.5f, 1f).OnComplete(() =>
            {
                if (!colliderController)
                {
                    pos = transform.position;
                }
                PlaceBrain.Instance.OnUnitChangedPosition(this);

                x = -.2f;
                LevelController.Instance.isStoneInAnim = false;
            });
        }
        else if (direction.x > 0) //directin.x 0 dan küçükse sola hareket
        {
            LevelController.Instance.isStoneInAnim = true;
            transform.DOMoveX(transform.position.x - 1.5f, 1f).OnComplete(() =>
            {
                if (!colliderController)
                {
                    pos = transform.position;
                }
                PlaceBrain.Instance.OnUnitChangedPosition(this);

                x = +.2f;
                LevelController.Instance.isStoneInAnim = false;
            });
        }

        if (direction.y < 0) //direction.y 0 dan büyükse yukarı hareket
        {
            LevelController.Instance.isStoneInAnim = true;
            transform.DOMoveZ(transform.position.z + 2, 1f).OnComplete(() =>
            {
                if (!colliderController)
                {
                    pos = transform.position;
                }
                PlaceBrain.Instance.OnUnitChangedPosition(this);

                z = -.2f;
                LevelController.Instance.isStoneInAnim = false;
            });
        }
        else if (direction.y > 0) //direction.y 0 dan küçükse aşağı hareket
        {
            LevelController.Instance.isStoneInAnim = true;
            transform.DOMoveZ(transform.position.z - 2, 1f).OnComplete(() =>
            {
                if (!colliderController)
                {
                    pos = transform.position;
                }
                PlaceBrain.Instance.OnUnitChangedPosition(this);

                z = .2f;
                LevelController.Instance.isStoneInAnim = false;
            });
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Unit"))
        {
            colliderController = true;
            transform.DOPunchPosition(new Vector3(x, 0, z), .01f, 10, 100f).OnComplete(() =>
            {
                transform.DOMove(pos, 1f);
            });
        }
    }*/

    #endregion
}