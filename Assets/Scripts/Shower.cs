using UnityEngine;
using UnityEngine.UI; // Needed for UI components

public class Shower : MonoBehaviour
{
    public GameObject Simulation;
    public GameObject Base_Particle;
    public Vector2 init_speed = new Vector2(1.0f, 0.0f);
    public float spawn_rate = 50f;
    private float time;

    private bool isRunning = true;

    public Button toggleButton; // Assign in inspector
    public Color runningColor = Color.green;
    public Color stoppedColor = Color.red;

    void Start()
    {
        Simulation = GameObject.Find("Simulation");
        Base_Particle = GameObject.Find("Base_Particle");

        // Set initial button color
        if (toggleButton != null)
            toggleButton.image.color = runningColor;
    }

    void Update()
    {
        if (!isRunning || Simulation.transform.childCount >= 1000)
            return;

        time += Time.deltaTime;
        if (time < 1.0f / spawn_rate)
            return;

        GameObject new_particle = Instantiate(Base_Particle, transform.position, Quaternion.identity);

        var particle = new_particle.GetComponent<Particle>();
        particle.pos = transform.position;
        particle.previous_pos = transform.position;
        particle.visual_pos = transform.position;
        particle.vel = init_speed;

        new_particle.transform.parent = Simulation.transform;

        time = 0.0f;
    }

    public void ToggleShower()
    {
        isRunning = !isRunning;
        spawn_rate = isRunning ? 50f : 0f;

        // Update button color
        if (toggleButton != null)
            toggleButton.image.color = isRunning ? runningColor : stoppedColor;
    }
}
