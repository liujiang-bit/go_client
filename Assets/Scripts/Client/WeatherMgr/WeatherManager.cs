using Skyunion;
using System;
using UnityEngine;

namespace Client
{
    public class WeatherManager : MonoSingleton<WeatherManager>
    {
        public string m_rain_particle = "Rain_01";

        public string m_snow_particle = "snow_01";

        public string m_thunderstorm_particle = "thunderstorm_01";

        public string m_sand_particle = "Sand_01";

        private ParticleSystem m_rain_particle_instance;

        private ParticleSystem m_snow_particle_instance;

        private ParticleSystem m_sand_particle_instance;

        private ParticleSystem m_thunderstorm_particle_instance;

        private AudioHandler m_rain_sfx_handler;

        private AudioHandler m_sand_sfx_handler;

        private bool m_is_sanding;

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

        private void Start()
        {
        }

        public void StartUpdateRain(bool enable_thunder)
        {
            enableThunder = enable_thunder;
            if (CoreUtils.GetGraphicLevel() == CoreUtils.GraphicLevel.HIGH || CoreUtils.GetGraphicLevel() == CoreUtils.GraphicLevel.MEDIUM)
            {
                if (!IsInvoking("UpdateRain"))
                {
                    InvokeRepeating("UpdateRain", 10f, 30f);
                }
            }
            else
            {
                if (IsInvoking("UpdateRain"))
                {
                    CancelInvoke("UpdateRain");
                }
            }
            // ��ȥ�ͷ�ɳЧ��
            //StartSand();
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
                            if (!m_is_snowing && !m_is_rainning && !m_is_thunderstorm && !m_is_sanding)
                            {
                                StartThunderstorm();
                            }
                        }
                        else if (!m_is_snowing && !m_is_rainning && !m_is_thunderstorm && !m_is_sanding)
                        {
                            if (UnityEngine.Random.Range(0, 1) == 0)
                            {
                                StartRain();
                            }
                            else
                            {
                                StartSand();
                            }
                        }
                    }
                    else if (!m_is_snowing && !m_is_rainning && !m_is_thunderstorm && !m_is_sanding)
                    {
                        if (UnityEngine.Random.Range(0, 1) == 0)
                        {
                            StartRain();
                        }
                        else
                        {
                            StartSand();
                        }
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
                else if(m_is_sanding)
                {
                    StopSand();
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
                // �����������
                if (Camera.main && Camera.main.transform.hasChanged)
                {
                    transform.position = Camera.main.transform.position;
                }
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
            Vector2 viewCenter = WorldCamera.Instance().GetViewCenter();
            viewCenter = new Vector2(viewCenter.x % m_snow_grid_width, viewCenter.y % m_snow_grid_width);
            float num = m_snow_grid_width - m_snow_enable_range;
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
                if (m_is_sanding)
                {
                    StopSand();
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
                    if(m_is_snowing)
                    {
                        m_snow_particle_instance.Play();
                    }
                });
            }

            if (m_snow_particle_instance)
            {
                m_snow_particle_instance.Play();
            }
            m_is_snowing = true;
        }

        private void StopSnowing()
        {
            if (m_snow_particle_instance)
            {
                m_snow_particle_instance.Stop();
            }
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
                });
            });
            m_is_rainning = true;
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
        private void StartSand()
        {
            CoreUtils.assetService.Instantiate(m_sand_particle, (gameObject) =>
            {
                gameObject.transform.SetParent(base.transform);
                gameObject.transform.position = base.transform.position + base.transform.forward * m_particle_pos_offset;
                m_sand_particle_instance = gameObject.GetComponent<ParticleSystem>();
                m_sand_particle_instance.Play();
                CoreUtils.audioService.PlayLoop("sfx_env_sand", (handdler) =>
                {
                    m_sand_sfx_handler = handdler;
                });
            });
            m_is_sanding = true;
        }
        private void StopSand()
        {
            if (m_sand_particle_instance != null)
            {
                m_sand_particle_instance.Stop();
                CoreUtils.assetService.Destroy(m_sand_particle_instance.gameObject, 10f);
                CoreUtils.audioService.FadeHandlerVolume(m_sand_sfx_handler, 0f, 1f, true);
                m_sand_particle_instance = null;
            }
            m_is_sanding = false;
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
                });
            });
            m_is_thunderstorm = true;
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
            ClientUtils.lightingManager.UpdateThunderLighting(m_thunder_ambient_color, m_thunder_direction_Color, new_direction_intensity, m_thunder_in_time, m_thunder_out_time, 0f);
        }
    }
}