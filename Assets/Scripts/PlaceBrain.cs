using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlaceBrain : MonoBehaviour
{
    public static PlaceBrain Instance;
    [SerializeField] private List<Place> allPlaces = new List<Place>(); //spawn noktalarını listeliyoruz
    [SerializeField] private GameObject unitPrefab; //taş prefabını alıyoruz

    private void Awake()
    {
        Instance = this;
        //SpawnUnits();
    }


    #region Spawner

    // public void SpawnUnits() //Taşları spawn noktalarında üretiyoruz
    // {
    //     foreach (var place in allPlaces)
    //     {
    //         GameObject
    //             spawnedUnitObject =
    //                 Instantiate(unitPrefab, place.transform); //spawnlayacağımız objeyi ve konumunu nesneye atıyoruz
    //         Unit spawnedUnit = spawnedUnitObject.GetComponent<Unit>(); //spawnlanan objeye unit scriptini atıyoruz
    //         place.unitHolder = spawnedUnit; //spawn noktasının scriptine unit objemizi atıyoruz
    //         spawnedUnit.SetRandomUnitData(); //random aldığımız taş renk ve sayısının değerlerini unit objemize atıyoruz
    //     }
    // }

    #endregion


    public Place GetPlaceWithCoord(Vector2 targetCoords)
    {
        return
            allPlaces.Find(p =>
                p.coords == targetCoords); //spawn noktalarımızın koordinatlarını "targetCoords" vektörüne atıyoruz
    }

    public void
        MoveStone(Unit movedUnit, Vector2 direction) //'movedUnit' unit objemizi atıyoruz-'direction' gidilecek mesafe
    {
        if (LevelController.Instance.isStoneInAnim) return;
        Place
            myPlace = movedUnit.transform
                .GetComponentInParent<Place>(); //parametre olarak aldığımız unit(movedUnit) objemizi 'myPlace' spawn noktamızın child'ı yapıyoruz
        Vector2
            targetCoords =
                myPlace.coords +
                direction; //taşımızın konumuna gidilecek mesafeyi ekleyerek hedef koordinata(targetCoords) atıyoruz
        if (targetCoords.x < 0 || targetCoords.x > 4 || targetCoords.y < 0 ||
            targetCoords.y > 3) //taşlarımızın sahneden çıkmamasını kontrol ediyoruz
        {
            LevelController.Instance.isStoneInAnim = true;
            movedUnit.transform.DOLocalJump(Vector3.zero, .5f, 1, .2f).OnComplete(() =>
            {
                LevelController.Instance.isStoneInAnim = false;
            });
        }

        Place targetPlace = GetPlaceWithCoord(targetCoords); //gitmek istediğimiz noktanın koordinatlarını alıyoruz

        if (targetPlace.unitHolder != null) //gitmek istediğimiz koordinat doluysa giriyoruz
        {
            LevelController.Instance.isStoneInAnim = true;
            movedUnit.transform.DOLocalJump(Vector3.zero, .5f, 1, .2f).OnComplete(
                () => //gitmek istediğimiz coordinat dolu olduğu için taşı olduğu yerde zıplatıyoruz
                {
                    LevelController.Instance.isStoneInAnim = false;
                });
        }
        else //gitmek istediğimiz koordinat boşsa else giriyoruz
        {
            LevelController.Instance.isStoneInAnim = true;
            targetPlace.unitHolder = movedUnit; //gitmek istediğimiz noktaya götürmek istediğimiz taşı atıyoruz
            myPlace.unitHolder = null; //götürmek istediğimiz taş gittikten sonra eski noktanın boş olmasını sağlıyoruz
            movedUnit.transform.parent =
                targetPlace.transform; //hareket ettirdiğimiz taşı gittiğimiz noktanın child yapıyoruz

            movedUnit.transform.DOLocalMove(Vector3.zero, 0.5f).OnComplete(() => //taşın hareketini sağlıyoruz
            {
                Check4Side(targetPlace);
                LevelController.Instance.isStoneInAnim = false;
            });
        }
    }

    
    List<Place> chainSeri = new List<Place>(); //doğru sıralanan seri taşları listelemek için liste oluşturduk
    List<Color> stoneColor = new List<Color>();
    List<Place> chainPer = new List<Place>(); //doğru sıralanmış per taşları listelemek için liste oluşturduk
    private int numarator = 0;

    public void Check4Side(Place startPlace)
    {
        List<Vector2> directions = new List<Vector2>() //bakıcağımız yönleri listeledik
        {
            new Vector2(-1, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(0, -1),
        };
        for (int i = 0; i < directions.Count; i++)
        {
            numarator = i;
            Vector2
                currentTargetCoords =
                    directions[i] +
                    startPlace.coords; //başlangıç koordinasyonumuzla bakacağımız koordinasyonları topladık
            if (currentTargetCoords.x < 0 || currentTargetCoords.x > 4 || currentTargetCoords.y < 0 ||
                currentTargetCoords.y > 3) //dışarı çıkma kontrolü yapıyoruz
            {
                continue;
            }

            Place targetPlace = GetPlaceWithCoord(currentTargetCoords);
            if (targetPlace.unitHolder != null) //hedef nokta doluysa
            {
                if (startPlace.unitHolder.color == targetPlace.unitHolder.color) //renk aynıysa
                {
                    if (startPlace.unitHolder.number + 1 == targetPlace.unitHolder.number ||
                        startPlace.unitHolder.number - 1 == targetPlace.unitHolder.number) //sayılar ardışıksa
                    {
                        if (!chainSeri.Contains(startPlace) && !startPlace.unitHolder.thisSearched)
                        {
                            chainSeri.Add(startPlace);
                            Place.Instance.searchCountSeri++;
                            startPlace.unitHolder.thisSearched = true;
                        }

                        if (!chainSeri.Contains(targetPlace) && !targetPlace.unitHolder.thisSearched)
                        {
                            chainSeri.Add(targetPlace);
                            Place.Instance.searchCountSeri++;
                            targetPlace.unitHolder.thisSearched = true;
                        }

                        if (chainSeri.Count >= 3)
                        {
                            DestroyedStone(chainSeri);
                            break;
                        }
                    }
                }
                else
                {
                    if (startPlace.unitHolder.number == targetPlace.unitHolder.number&&
                        !stoneColor.Contains(targetPlace.unitHolder.myText.color))
                    {
                        if (!chainPer.Contains(startPlace) && !startPlace.unitHolder.thisSearched)
                        {
                            chainPer.Add(startPlace);
                            stoneColor.Add(startPlace.unitHolder.myText.color);
                            Place.Instance.searchCountPer++;
                            startPlace.unitHolder.thisSearched = true;
                        }

                        if (!chainPer.Contains(targetPlace) && !targetPlace.unitHolder.thisSearched)
                        {
                            chainPer.Add(targetPlace);
                            stoneColor.Add(targetPlace.unitHolder.myText.color);
                            Place.Instance.searchCountPer++;
                            targetPlace.unitHolder.thisSearched = true;
                        }

                        if (chainPer.Count >= 3)
                        {
                            DestroyedStone(chainPer);
                            break;
                        }

                    }
                }
            }
            //#region diğer

            // if (i>=directions.Count-1&&chain.Count>0&&controller)
            // {
            //     if (chain.Count<=2)
            //     {
            //         directions = SetTargetList(directions, first);
            //         startPlace = chain[1];
            //         i = -1;
            //     }
            // }

            //#endregion
        }

        if (numarator >= 3 && Place.Instance.searchCountSeri >= 2)
        {
            Place.Instance.searchCountSeri = 0;
            Check4Side(chainSeri[1]);
            foreach (Place place in chainSeri)
            {
                place.unitHolder.thisSearched = false;
            }
            chainSeri.Clear();
            stoneColor.Clear();
        }
        else if (numarator >= 3 && Place.Instance.searchCountPer >= 2)
        {
            Place.Instance.searchCountPer = 0;
            Check4Side(chainPer[1]);
            foreach (Place place in chainPer)
            {
                place.unitHolder.thisSearched = false;
            }
            stoneColor.Clear();
            chainPer.Clear();
        }
    }

    private void DestroyedStone(List<Place> chain)
    {
        Istaka.Instance.Regulator(chain);
        Place.Instance.searchCountSeri = 0;
        Place.Instance.searchCountPer = 0;
        stoneColor.Clear();
        chain.Clear();
    }

    #region Diğer

    // List<Vector2> SetTargetList(List<Vector2> directions,int index)
    // {
    //     switch (index)
    //     {
    //         case 0:
    //             directions.RemoveAt(1);
    //             break;
    //         case 1:
    //             directions.RemoveAt(0);
    //             break;
    //         case 2:
    //             directions.RemoveAt(3);
    //             break;
    //         case 3:
    //             directions.RemoveAt(2);
    //             break;
    //     }
    //     return directions;
    // }

    #endregion
}