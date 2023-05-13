
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public CSVReader csvReader;
    public double[] actual_postion_x;
    public double[] actual_postion_y;
    public double[] actual_postion_z;
    private TrailRenderer trail;

    void Start()
    {
        Application.targetFrameRate = 100;
        CalculatePositions();
        trail = gameObject.AddComponent<TrailRenderer>();
        trail.time = 10.0f;
        trail.widthCurve = new AnimationCurve(
            new Keyframe(0, 0.1f),
            new Keyframe(1, 0));
        trail.material = new Material(Shader.Find("Sprites/Default"));
    }

    void CalculatePositions()
    {
        actual_postion_x = Integrate(csvReader.datax);
        actual_postion_y = Integrate(csvReader.datay);
        actual_postion_z = Integrate(csvReader.dataz);
    }

    double[] Integrate(List<double> data)
    {
        List<double> zero = new List<double>() { 0 };
        List<double> values = zero.Concat(data).ToList();
        List<double> times = zero.Concat(csvReader.times).ToList();

        double[] integral1 = new double[times.Count];
        double[] integral2 = new double[times.Count];
        integral1[0] = 0;
        integral2[0] = 0;
        double square;

        for (int i = 1; i < times.Count - 1; i++)
        {
            if (values[i] == 0 && values[i - 1] == 0) { integral1[i] = integral1[i-1]; continue; }
            if (values[i - 1] / values[i] >= 0)
            {
                integral1[i] = integral1[i - 1] + (times[i] - times[i - 1]) * (values[i] + values[i - 1]) / 2;
            }
            else
            {
                square = values[i - 1] / (values[i - 1] - values[i]);
                integral1[i] = integral1[i - 1] + ((times[i] - times[i - 1]) / 2) * ((values[i - 1] * square) + (1 - square) * values[i]);
            }
        }

        for (int i = 1; i < times.Count - 1; i++)
        {
            if (values[i] == 0 && values[i - 1] == 0) { integral2[i] = integral2[i-1]; continue; }
            if (integral1[i - 1] / integral1[i] >= 0)
            {
                integral2[i] = integral2[i - 1] + (times[i] - times[i - 1]) * (integral1[i] + integral1[i - 1]) / 2;
            }

            else
            {
                square = integral1[i - 1] / (integral1[i - 1] - integral1[i]);
                integral2[i] = integral2[i - 1] + ((times[i] - times[i - 1]) / 2) * ((integral1[i - 1] * square) + (1 - square) * integral1[i]);
            }
        }


        return integral2;
    }

    private int i = 0;
    private void Update()
    {

        if (i < actual_postion_x.Length)
        {
            transform.position = new Vector3((float)actual_postion_x[i], (float)actual_postion_z[i], (float)actual_postion_y[i]);
            i++;
        }
        else 
        {
            i = 0;
            trail.Clear();
            trail.enabled = true;
        }
    }
}