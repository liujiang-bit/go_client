using Skyunion;
using System;
using UnityEngine;

namespace ROK
{
    public class WeatherMgr : MonoBehaviour
    {
        private static WeatherMgr m_instance;

        public string m_rain_particle;

        public string m_snow_particle;

        public string m_thunderstorm_particle;

        private ParticleSystem m_rain_particle_instance;

        private ParticleSystem m_snow_particle_instance;

        private ParticleSystem m_thunderstorm_particle_instance;

        private AudioHandler m_rain_sfx_handler;

        private bool m_is_rainning;

        private bool m_is_snowing;

        private bool m_is_thunderstorm;

        private float m_snow_grid_width = 7200f;

        private float m_snow_enable_range = 150f;

        private float m_particle_pos_offset = 2f;

        private bool enableThunder;

        private float m_random_group_internal;

        private float m_timer;

        public Color m_thunder_ambient_color = Color.white;

        public Color m_thunder_direction_Color = Color.white;

        public float m_thunder_in_time = 0.2f;

        public float m_thunder_out_time = 0.2f;

        public float m_thunder_intensity_min = 0.1f;

        public float m_thunder_intensity_max = 2f;

        public static WeatherMgr GetInstance()
        {
            if (WeatherMgr.m_instance == null)
            {
                WeatherMgr.m_instance = (UnityEngine.Object.FindObjectOfType(typeof(WeatherMgr)) as WeatherMgr);
            }
            return WeatherMgr.m_instance;
        }

        private void Start()
        {
        }

        public void StartUpdateRain(bool enable_thunder)
        {
            enableThunder = enable_thunder;
            if (GraphicSettingMgr.GetGraphicLevel() == GraphicSettingMgr.GraphicLevel.HIGH || GraphicSettingMgr.GetGraphicLevel() == GraphicSettingMgr.GraphicLevel.MEDIUM)
            {
                base.InvokeRepeating("UpdateRain", 10f, 30f);
            }
        }

        private void UpdateRain()
        {
            try
            {
                if (UnityEngine.Random.Range(0, 5) == 0)
                {
                    if (enableThunder)
                    {
                        if (UnityEngine.Random.Range(0, 5) == 0)
                        {
                            if (!m_is_snowing && !m_is_rainning && !m_is_thunderstorm)
                            {
                                StartThunderstorm();
                            }
                        }
                        else if (!m_is_snowing && !m_is_rainning && !m_is_thunderstorm)
                        {
                            StartRain();
                        }
                    }
                    else if (!m_is_snowing && !m_is_rainning && !m_is_thunderstorm)
                    {
                        StartRain();
                    }
                }
                else if (m_is_rainning)
                {
                    StopRain();
                }
                else if (m_is_thunderstorm)
                {
                    StopThunderstorm();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void Update()
        {
            try
            {
                UpdateSnow();
                UpdateThunderstorm();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void UpdateSnow()
        {
            Vector2 viewCenter = WorldCamera.m_instance.GetViewCenter();
            viewCenter = new Vector2(viewCenter.x % m_snow_grid_width, viewCenter.y % m_snow_grid_width);
            float num = m_snow_grid_width - m_snow_enable_range;
            // 目前边界都会下雪
            if (viewCenter.x <= m_snow_enable_range || viewCenter.x >= num || viewCenter.y <= m_snow_enable_range || viewCenter.y >= num)
            {
                if (m_is_rainning)
                {
                    StopRain();
                }
                if (m_is_thunderstorm)
                {
                    StopThunderstorm();
                }
                if (!m_is_snowing)
                {
                    StartSnowing();
                }
            }
            else if (m_is_snowing)
            {
                StopSnowing();
            }
        }

        private void UpdateThunderstorm()
        {
            if (m_is_thunderstorm)
            {
                m_timer += Time.deltaTime;
                if (m_random_group_internal == 0f)
                {
                    m_random_group_internal = UnityEngine.Random.Range(5f, 10f);
                }
                if (m_timer > m_random_group_internal)
                {
                    m_timer = 0f;
                    m_random_group_internal = 0f;
                    PlayGroupThunder();
                }
            }
        }

        private void PlayGroupThunder()
        {
            int num = UnityEngine.Random.Range(3, 6);
            for (int i = 0; i < num; i++)
            {
                base.Invoke("PlayThunder", UnityEngine.Random.Range(0f, 2f));
            }
        }

        private void StartSnowing()
        {
            if (m_snow_particle_instance == null)
            {
                CoreUtils.assetService.Instantiate(m_snow_particle, (gameObject) =>
                {
                    gameObject.transform.SetParent(base.transform);
                    gameObject.transform.position = base.transform.position + base.transform.forward * m_particle_pos_offset;
                    m_snow_particle_instance = gameObject.GetComponent<ParticleSystem>();
                    m_snow_particle_instance.Play();
                    m_is_snowing = true;
                });
                return;
            }
            m_snow_particle_instance.Play();
            m_is_snowing = true;
        }

        private void StopSnowing()
        {
            m_snow_particle_instance.Stop();
            m_is_snowing = false;
        }

        private void StartRain()
        {
            CoreUtils.assetService.Instantiate(m_rain_particle, (gameObject) =>
            {
                gameObject.transform.SetParent(base.transform);
                gameObject.transform.position = base.transform.position + base.transform.forward * m_particle_pos_offset;
                m_rain_particle_instance = gameObject.GetComponent<ParticleSystem>();
                m_rain_particle_instance.Play();
                CoreUtils.audioService.PlayLoop("sfx_env_rain", (handdler) =>
                {
                    m_rain_sfx_handler = handdler;
                    m_is_rainning = true;
                });
            });
        }

        private void StopRain()
        {
            if (m_rain_particle_instance != null)
            {
                m_rain_particle_instance.Stop();
                CoreUtils.assetService.Destroy(m_rain_particle_instance.gameObject, 10f);
                CoreUtils.audioService.FadeHandlerVolume(m_rain_sfx_handler, 0f, 1f, true);
                m_rain_particle_instance = null;
            }
            m_is_rainning = false;
        }

        private void StartThunderstorm()
        {
            CoreUtils.assetService.Instantiate(m_thunderstorm_particle, (gameObject) =>
            {
                gameObject.transform.SetParent(base.transform);
                gameObject.transform.position = base.transform.position + base.transform.forward * m_particle_pos_offset;
                m_thunderstorm_particle_instance = gameObject.GetComponent<ParticleSystem>();
                m_thunderstorm_particle_instance.Play();
                CoreUtils.audioService.PlayLoop("sfx_env_rain", (handdler) =>
                {
                    m_rain_sfx_handler = handdler;
                    m_is_thunderstorm = true;
                });
            });
        }

        private void StopThunderstorm()
        {
            m_thunderstorm_particle_instance.Stop();
            CoreUtils.assetService.Destroy(m_thunderstorm_particle_instance.gameObject, 1f);
            CoreUtils.audioService.FadeHandlerVolume(m_rain_sfx_handler, 0f, 1f, true);
            m_is_thunderstorm = false;
        }

        private void PlayThunder()
        {
            float new_direction_intensity = UnityEngine.Random.Range(m_thunder_intensity_min, m_thunder_intensity_max);
            LightingManager.GetInstance().UpdateThunderLighting(m_thunder_ambient_color, m_thunder_direction_Color, new_direction_intensity, m_thunder_in_time, m_thunder_out_time, 0f);
        }
    }
}