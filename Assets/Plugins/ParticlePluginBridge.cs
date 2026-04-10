using UnityEngine;
using System.Runtime.InteropServices;
using System;
 
public class ParticlePluginBridge : MonoBehaviour
{
    // Estructura que debe coincidir con la de C++
    [StructLayout(LayoutKind.Sequential)]
    public struct Particle
    {
        public Vector3 position;
        public Vector3 velocity;
        public float life;
    }
 
    // Importar funciones de C++
    [DllImport("ParticleSystem")]
    private static extern void InitParticles(int count);
 
    [DllImport("ParticleSystem")]
    private static extern void UpdateParticles(float deltaTime, float speed);
 
    [DllImport("ParticleSystem")]
    private static extern IntPtr GetParticles(); // Usamos IntPtr para memoria nativa
 
    [DllImport("ParticleSystem")]
    private static extern int GetParticleCount();
 
    public int particleCount = 1000;
    public float simulationSpeed = 1.0f;
    public GameObject particlePrefab;
 
    private Transform[] particleTransforms;
 
    void Start()
    {
        InitParticles(particleCount);
        
        // Crear visualización en Unity (
        particleTransforms = new Transform[particleCount];
        for (int i = 0; i < particleCount; i++)
        {
            particleTransforms[i] = Instantiate(particlePrefab).transform;
        }
    }
 
    void Update()
    {
        // 1. Actualizar lógica en C++
        UpdateParticles(Time.deltaTime, simulationSpeed);
 
        // 2. Obtener datos de C++
        IntPtr particlePtr = GetParticles();
        int size = Marshal.SizeOf(typeof(Particle));
 
        // 3. Aplicar posiciones a Unity
        for (int i = 0; i < particleCount; i++)
        {
            IntPtr currentParticlePtr = new IntPtr(particlePtr.ToInt64() + i * size);
            Particle p = Marshal.PtrToStructure<Particle>(currentParticlePtr);
            particleTransforms[i].position = p.position;
        }
    }
}