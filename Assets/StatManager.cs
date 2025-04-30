using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using list = System.Collections.Generic.List<Particle>;
using vector2 = UnityEngine.Vector2;

using static Config;

public class StatManager : MonoBehaviour
{
    // UI Elements
    public TextMeshProUGUI lossRateText; // For Loss Rate and Flow Rate
    public TextMeshProUGUI temperatureText; // For Temperature
    public Button toggleStatsButton; // Button to toggle stats visibility

    // Stats variables
    private float lostLiters = 0f;
    private float volumePerParticle = 0.001f;
    private float temperature = 25f;
    private bool isStatsVisible = true; // Track visibility state

    // References to Simulation data (will be set by Simulation.cs)
    private list particles;
    private float timeSinceStart;

    void Awake()
    {
        // Set up the toggle button
        if (toggleStatsButton != null)
        {
            toggleStatsButton.onClick.AddListener(ToggleStatsVisibility);
            toggleStatsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Hide Stats";
        }
    }

    // Method to toggle stats visibility
    public void ToggleStatsVisibility()
    {
        isStatsVisible = !isStatsVisible;

        // Update UI visibility
        if (lossRateText != null)
        {
            lossRateText.gameObject.SetActive(isStatsVisible);
        }
        if (temperatureText != null)
        {
            temperatureText.gameObject.SetActive(isStatsVisible);
        }

        // Update button text
        if (toggleStatsButton != null)
        {
            toggleStatsButton.GetComponentInChildren<TextMeshProUGUI>().text = isStatsVisible ? "Hide Stats" : "Show Stats";
        }
    }

    // Method to update stats (called by Simulation.cs)
    public void UpdateStats(list particles, float timeSinceStart, float x_min, float x_max)
    {
        if (!isStatsVisible) return; // Skip updates if stats are hidden

        this.particles = particles;
        this.timeSinceStart = timeSinceStart;

        // Calculate lost particles (outside bounds)
        int lostParticles = 0;
        foreach (Particle p in particles)
        {
            if (p.pos.y < BOTTOM || p.pos.x < x_min || p.pos.x > x_max)
                lostParticles++;
        }
        float lostVolume = lostParticles * volumePerParticle;
        float lossRate = (lostVolume / timeSinceStart) * 60f; // L/min

        // Calculate particles flowing across WALL_POS
        int flowingParticles = 0;
        foreach (Particle p in particles)
        {
            if (p.pos.x > WALL_POS && p.vel.x > 0)
                flowingParticles++;
        }
        float flowingVolume = flowingParticles * volumePerParticle;
        float flowRate = (flowingVolume / Time.deltaTime) * 60f; // L/min

        // Update temperature gradually
        float tempChange = Random.Range(-0.2f, 0.2f);
        temperature += tempChange;
        temperature = Mathf.Clamp(temperature, 7f, 16f);

        // Update UI
        if (lossRateText != null)
        {
            lossRateText.text = $"Loss Rate: {lossRate:F2} L/min\nFlow Rate: {flowRate:F2} L/min";
        }

        if (temperatureText != null)
        {
            temperatureText.text = $"Temperature: {temperature:F1} C";
        }
    }
}