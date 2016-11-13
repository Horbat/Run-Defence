using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
    Gun _currentGun;
    Transform _gunHolder;

    const string _pathGunHolder = "GunHolder";
    const string _pathGunPrefabs = "Prefabs/Guns/";

    void Awake()
    {
        _gunHolder = transform.Find(_pathGunHolder);
    }

    void Start()
    {
        SetGun("BasicGun");
    }

    void SetGun(string gunName)
    {
        Destroy(_currentGun);

        GameObject link = Resources.Load<GameObject>(_pathGunPrefabs + gunName);
        GameObject newGunTrans = Instantiate(link, _gunHolder.position, Quaternion.identity) as GameObject;
        newGunTrans.transform.SetParent(_gunHolder);

        _currentGun = newGunTrans.GetComponent<Gun>();
    }

    public void Fire()
    {
        if (_currentGun != null)
        {
            _currentGun.Shoot();
        }
    }
}
