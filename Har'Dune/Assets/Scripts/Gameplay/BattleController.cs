﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleController : MonoBehaviour
{
    [Header("Scripts")]
    public ScriptManager scriptManager;

    [Header("Statuses")]
    public bool battleStatus = false;
    public bool attackSelected = false;
    public bool actionSelected = false;
    public bool spellSelected = false;

    [Header("Actions")]
    public bool isGrappling = false;
    public bool isHiding = false;
    public bool isDodging = false;
    public bool isWaiting = false;

    [Header("Attacks")]
    public bool unarmedStrike = false;
    public bool greatswordAttack = false;
    public bool daggerAttack = false;
    public bool quarterstaffAttack = false;
    public bool lightCrossbowAttack = false;
    public bool scimitarAttack = false;
    public bool daggerThrowAttack = false;

    [Header("Spells")]
    public bool druidcraft = false;
    public bool frostbite = false;
    public bool cureWounds = false;
    public bool charmPerson = false;

    public void Awake()
    {
        SetScriptManager();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (scriptManager.scriptBattleController.battleStatus)
            {
                if (attackSelected)
                {
                    scriptManager.scriptWeaponAttack.StartAttack();
                }

                if (actionSelected)
                {
                    scriptManager.scriptGeneralActions.StartAction();
                }

                if (spellSelected)
                {
                    //scriptManager.scriptSpellcasting.StartSpell();
                }
            }
        }
    }

    public void SetScriptManager()
    {
        scriptManager = GameObject.Find("Script Manager").GetComponent<ScriptManager>();
        scriptManager.ConnectScripts();
    }

    public void ResetActionBools()
    {
        //Selections
        attackSelected = false;
        actionSelected = false;
        spellSelected = false;

        //Actions
        isGrappling = false;
        isHiding = false;
        isDodging = false;
        isWaiting = false;

        //Attacks
        greatswordAttack = false;
        daggerAttack = false;
        quarterstaffAttack = false;
        lightCrossbowAttack = false;
        scimitarAttack = false;
        daggerThrowAttack = false;

        //Spells
        druidcraft = false;
        frostbite = false;
        cureWounds = false;
        charmPerson = false;
    }

    public bool isUnitDead(GameObject unitCurrentHeatlh)
    {
        //Unit HP <= 0
        if (unitCurrentHeatlh.GetComponent<UnitController>().currentHP <= 0)
        {
            return true;
        }

        return false;
    }

    public void DestroyObject(GameObject deadUnit)
    {
        Destroy(deadUnit);
    }

    public IEnumerator ReturnAfterAttack(GameObject initiator, Vector3 endPoint)
    {
        float elapsedTime = 0;

        //Move
        while (elapsedTime < .30f)
        {
            initiator.transform.position = Vector3.Lerp(initiator.transform.position, endPoint, (elapsedTime / .25f));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        initiator.GetComponent<UnitController>().SetWaitAnimation();
        initiator.GetComponent<UnitController>().Wait();
    }

    public Vector3 GetDirection(GameObject initiator, GameObject recipient)
    {
        Vector3 startingPosition = initiator.transform.position;
        Vector3 endingPosition = recipient.transform.position;
        return ((endingPosition - startingPosition) / (endingPosition - startingPosition).magnitude).normalized;
    }

    public int RollD20()
    {
        int rollD20 = Random.Range(1, 20);
        return rollD20;
    }

    public int RollD20Advantage()
    {
        int rollOne = RollD20();
        int rollTwo = RollD20();
        int rollD20s = Mathf.Max(rollOne, rollTwo);
        return rollD20s;
    }

    public int RollD20Disadvantage()
    {
        int rollOne = RollD20();
        int rollTwo = RollD20();
        int rollD20s = Mathf.Min(rollOne, rollTwo);
        return rollD20s;
    }

    public int AttackRoll()
    {
        UnitStats selectedUnitStats = scriptManager.scriptTileMap.selectedUnit.GetComponent<UnitStats>();
        UnitController selectedUnitController = scriptManager.scriptTileMap.selectedUnit.GetComponent<UnitController>();

        //Normal
        if (selectedUnitController.GetComponent<UnitController>().unitAttackState == selectedUnitController.GetComponent<UnitController>().GetAttackState(0))
        {
            int attackRoll = RollD20() + selectedUnitStats.attackModifier;
            return attackRoll;
        }

        //Advantage
        else if (selectedUnitController.GetComponent<UnitController>().unitAttackState == selectedUnitController.GetComponent<UnitController>().GetAttackState(1))
        {
            int attackRoll = RollD20Advantage() + selectedUnitStats.attackModifier;
            return attackRoll;
        }

        //Disadvantage
        else if (selectedUnitController.GetComponent<UnitController>().unitAttackState == selectedUnitController.GetComponent<UnitController>().GetAttackState(2))
        {
            int attackRoll = RollD20Disadvantage() + selectedUnitStats.attackModifier;
            return attackRoll;
        }

        return 100;
    }

    public int ActionRoll()
    {
        UnitStats selectedUnitStats = NewMethod();
        UnitController selectedUnitController = scriptManager.scriptTileMap.selectedUnit.GetComponent<UnitController>();

        //Normal
        if (selectedUnitController.GetComponent<UnitController>().unitActionState == selectedUnitController.GetComponent<UnitController>().GetActionState(0))
        {
            int attackRoll = RollD20() + selectedUnitStats.attackModifier;
            return attackRoll;
        }

        //Advantage
        else if (selectedUnitController.GetComponent<UnitController>().unitActionState == selectedUnitController.GetComponent<UnitController>().GetActionState(1))
        {
            int attackRoll = RollD20Advantage() + selectedUnitStats.attackModifier;
            return attackRoll;
        }

        //Disadvantage
        else if (selectedUnitController.GetComponent<UnitController>().unitActionState == selectedUnitController.GetComponent<UnitController>().GetActionState(2))
        {
            int attackRoll = RollD20Disadvantage() + selectedUnitStats.attackModifier;
            return attackRoll;
        }

        return 100;
    }

    private UnitStats NewMethod()
    {
        return scriptManager.scriptTileMap.selectedUnit.GetComponent<UnitStats>();
    }

    public int SpellSaveRoll()
    {
        UnitController selectedUnitController = scriptManager.scriptTileMap.selectedUnit.GetComponent<UnitController>();

        //Normal
        if (selectedUnitController.GetComponent<UnitController>().unitSpellSaveState == selectedUnitController.GetComponent<UnitController>().GetSpellSaveState(0))
        {
            int attackRoll = RollD20();
            return attackRoll;
        }

        //Advantage
        else if (selectedUnitController.GetComponent<UnitController>().unitSpellSaveState == selectedUnitController.GetComponent<UnitController>().GetSpellSaveState(1))
        {
            int attackRoll = RollD20Advantage();
            return attackRoll;
        }

        //Disadvantage
        else if (selectedUnitController.GetComponent<UnitController>().unitSpellSaveState == selectedUnitController.GetComponent<UnitController>().GetSpellSaveState(2))
        {
            int attackRoll = RollD20Disadvantage();
            return attackRoll;
        }

        return 100;
    }

    public int SpellCastRoll()
    {
        UnitController selectedUnitController = scriptManager.scriptTileMap.selectedUnit.GetComponent<UnitController>();

        //Normal
        if (selectedUnitController.GetComponent<UnitController>().unitSpellCastState == selectedUnitController.GetComponent<UnitController>().GetSpellCastState(0))
        {
            int attackRoll = RollD20();
            return attackRoll;
        }

        //Advantage
        else if (selectedUnitController.GetComponent<UnitController>().unitSpellCastState == selectedUnitController.GetComponent<UnitController>().GetSpellCastState(1))
        {
            int attackRoll = RollD20Advantage();
            return attackRoll;
        }

        //Disadvantage
        else if (selectedUnitController.GetComponent<UnitController>().unitSpellCastState == selectedUnitController.GetComponent<UnitController>().GetSpellCastState(2))
        {
            int attackRoll = RollD20Disadvantage();
            return attackRoll;
        }

        return 100;
    }
}