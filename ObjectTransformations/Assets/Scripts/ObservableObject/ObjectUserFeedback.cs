using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ObjectUserFeedback : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI functionTextMesh;

    [SerializeField] private Transform[] objectLights;
    [SerializeField] private float lightLerpSpeed;

    private Transform cam_Transform;
    private Transform canvas_Transform;

    private ObjectNavigation navigation;

    private const float ROTATESPEED = 1.5f;

    public bool LightsFlickering { get; private set; } = false;

    private void Awake ()
    {
        cam_Transform = Camera.main.transform;
        canvas_Transform = functionTextMesh.transform.parent;

        navigation = GetComponent<ObjectNavigation>();
        navigation.OnNodeReached += OnNodeReachedFeedback;
    }

    private void FixedUpdate ()
    {
        Vector3 cameraDirection = cam_Transform.position - canvas_Transform.position;
        Vector3 lookDirection = canvas_Transform.forward;
        if (Vector3.Angle(lookDirection, cameraDirection) > 0.5f)
        {
            float step = ROTATESPEED * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(lookDirection, cameraDirection, step, 0);
            canvas_Transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    private IEnumerator FlickerLights ()
    {
        LightsFlickering = true;

        Renderer[] lightRends = new Renderer[objectLights.Length];
        Color[] lightColors = new Color[objectLights.Length];

        for (int i = 0; i < lightRends.Length; i++)
        {
            lightRends[i] = objectLights[i].GetComponent<Renderer>();
            lightColors[i] = lightRends[i].material.color;
        }

        while (!navigation.IsLookingAtTarget)
        {
            float t = Mathf.PingPong(Time.time * lightLerpSpeed, 1f);
            for (int i = 0; i < lightRends.Length; i++)
            {
                Color lerpCol = Color.Lerp(lightColors[i], Color.red, t);
                lightRends[i].material.SetColor("_EmissionColor", lerpCol);
            }
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < lightRends.Length; i++)
        {
            lightRends[i].material.SetColor("_EmissionColor", lightColors[i]);
        }
        LightsFlickering = false;
    }

    /// <summary>
    /// Add name of current function to excecute to text mesh on canvas
    /// </summary>
    /// <param name="name">name of the function</param>
    public void AddFunctionNameToTextMesh (string name)
    {
        functionTextMesh.text += name;
    }

    /// <summary>
    /// Resets the functionText back to an empty string
    /// </summary>
    public void ClearFunctionNameText()
    {
        functionTextMesh.text = "";
    }


    private void OnNodeReachedFeedback (NodeAttributes node)
    {
        if (!navigation.IsLookingAtTarget && !LightsFlickering)
        {
            StartCoroutine(FlickerLights());
        }
    }
}
