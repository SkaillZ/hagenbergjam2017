using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<Item> inventory = new List<Item>();
    public List<string> gameFlags = new List<string>();
    public float timerSeconds = 300f;
    public float timerValue = 0f;
    public bool isTimerRunning = true;
    public int points = 0;

    [HideInInspector] public Player player = null;
    [HideInInspector] public GameUI gameUI;

    public bool startedRoutine = false;
    [HideInInspector] public string deathMessage;

    public void Awake()
    {
        _instance = this;
    }

    public void Update()
    {
        if (isTimerRunning)
        {
            timerValue += Time.deltaTime;
            if (timerValue >= timerSeconds)
            {
                if (!startedRoutine)
                {
                    StartCoroutine(DeathRoutine());
                }
                timerValue = timerSeconds;
                startedRoutine = true;
            }
        }
    }

    public IEnumerator DeathRoutine(bool die = true)
    {
        if (die)
            player.Die();
        player.PlayDeathSound();
        yield return new WaitForSeconds(2f);
        gameUI.ShowResults(points);
    }

    public void GiveItem(Item item)
    {
        if (!HasItem(item))
        {
            inventory.Add(item);
            gameUI.UpdateInventory();
        }
    }

    public void TakeItem(Item item)
    {
        inventory.Remove(item);
        gameUI.UpdateInventory();
    }

    public bool HasItem(Item item)
    {
        return inventory.Contains(item);
    }

    public void SetFlag(string flagName)
    {
        if (!IsFlagSet(flagName))
            gameFlags.Add(flagName);
    }

    public bool IsFlagSet(string flagName)
    {
        return gameFlags.Contains(flagName);
    }

    public int GetRemainingSeconds()
    {
        return Mathf.FloorToInt(timerSeconds - timerValue);
    }

    public void SubtractTime(int seconds)
    {
        timerValue += seconds;
        gameUI.ShowTimeDecrease(seconds);
    }

    public void AddPoints(int points)
    {
        this.points += points;
        gameUI.ShowPointsIncrease(points);
    }
}