using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeDecoy : Decoy
{
    #region DecoyDamage
    [Header("Explosive attributes")]
    public float _tornadoLifeTime = 0;
    public float _tornadoDamage = 0;
    public GameObject _tornado;
    #endregion

    protected override void decoyTp()
    {
        base.decoyTp();

        if (decoyShadow != null)
        {
            decoyShadow.GetComponent<MineController>().setBomb(_tornadoLifeTime, _tornadoDamage, _tornado, 0, gameObject);
        }


    }

}
