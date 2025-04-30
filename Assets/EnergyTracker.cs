using UnityEngine;
using TMPro;

public class EnergyTracker : MonoBehaviour
{
    public Simulation1 simulation; // Drag reference to your Simulation script in Inspector
    public TextMeshProUGUI energyText;

    private float energy = 0f;
    private float maxEnergy = 100f; // Max energy when water is on
    private float changeSpeed = 10f; // Energy units per second

    void Update()
    {
        if (simulation == null) 
            return;

        // น้ำเปิด → เพิ่ม energy ทีละน้อย
        if (simulation.isWaterOn)
        {
            energy = Mathf.MoveTowards(energy, maxEnergy, Time.deltaTime * changeSpeed);
        }
        else
        {
            // น้ำปิด → ลด energy ลงไปที่ 0
            energy = Mathf.MoveTowards(energy, 0f, Time.deltaTime * changeSpeed);
        }

        // อัปเดตข้อความบน UI
        if (energyText != null)
        {
            energyText.text = $"Energy Consumption: {energy:F1} kWh";
        }
    }
}
