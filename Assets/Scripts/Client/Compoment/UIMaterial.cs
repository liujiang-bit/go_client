using System.Collections.Generic;
using UnityEngine;
using Skyunion;
using UnityEngine.UI;

namespace Client
{
    public class UIMaterial : MonoBehaviour
    {
        [ Range (0,1)]
        public float N_mask;
        public Color Color;
        private Material material;
        private void Awake()
        {
            material = this.GetComponent<Image>().material;
        }
        private void Start()
        {
            material.SetFloat("_N_mask", N_mask);
            material.SetColor("_TintColor", Color);
        }
        private void Update()
        {
            material.SetFloat("_N_mask", N_mask);
            material.SetColor("_TintColor", Color);
        }
    }
}