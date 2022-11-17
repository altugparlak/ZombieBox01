using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputControl : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private SoundEffects soundEffects;

    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            RaycastHit hit;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = mainCamera.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag == "WeaponUpgrade")
                    {
                        if (gameSession.playerMoney >= gameSession.weaponUpgradeCost)
                        {

                            gameSession.WeaponUpgrade();
                            gameSession.SpendMoney(gameSession.randomSkillCost);
                            soundEffects.PlayUpgradeClickSound();
                        }
                        else
                        {
                            soundEffects.PlayCantUpgradeClickSound();
                        }
                    }

                    if (hit.transform.gameObject.tag == "RandomSkill")
                    {
                        if (gameSession.playerMoney >= gameSession.randomSkillCost)
                        {

                            gameSession.PickRandomSkillandActivate();
                            gameSession.SpendMoney(gameSession.randomSkillCost);
                            soundEffects.PlayUpgradeClickSound();
                        }
                        else
                        {
                            soundEffects.PlayCantUpgradeClickSound();
                        }
                    }

                }

            }
        }

    }

}
