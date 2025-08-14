using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.SceneManagement;

namespace Tc.Exp.ChunkMeshGenerate
{
    [ExecuteInEditMode]
    public class 项目大纲预设器 : MonoBehaviour
    {
        public MC_SO_大纲容器 预设;

        [ContextMenu("一键布置")]
        public void 一键布置()
        {
            if (预设 == null || 预设.预设列表 == null || 预设.预设列表.Count == 0)
            {
                Debug.LogError("未指定大纲预设，或预设为空");
                return;
            }

            // 1. 清除场景除自己外的所有根对象
            清除其他根对象();

            // 2. 检查是否包含 Main Camera 和 Directional Light
            bool hasCamera = false, hasLight = false;
            foreach (var item in 预设.预设列表)
            {
                if (item == null || string.IsNullOrWhiteSpace(item.MyObjName)) continue;
                if (item.MyObjName == "Main Camera") hasCamera = true;
                if (item.MyObjName == "Directional Light") hasLight = true;
            }
            if (!hasCamera || !hasLight)
            {
                Debug.LogError("预设中必须包含 Main Camera 和 Directional Light");
                return;
            }

            // 3. 遍历并创建对象
            foreach (var item in 预设.预设列表)
            {
                if (item == null || string.IsNullOrWhiteSpace(item.MyObjName))
                {
                    Debug.LogWarning("跳过一个无效或未命名的预设项");
                    continue;
                }

                GameObject parent = 生成路径(item.CsObjParent);
                GameObject newObj;

                if (item.MyObjName == "Main Camera")
                {
                    newObj = ObjectFactory.CreateGameObject("Main Camera", typeof(Camera), typeof(AudioListener));
                }
                else if (item.MyObjName == "Directional Light")
                {
                    newObj = ObjectFactory.CreateGameObject("Directional Light", typeof(Light));
                    Light light = newObj.GetComponent<Light>();
                    light.type = LightType.Directional;
                }
                else if (item.MyObjName == "EventSystem")
                {
                    newObj = ObjectFactory.CreateGameObject("EventSystem",
                        typeof(UnityEngine.EventSystems.EventSystem),
                        typeof(UnityEngine.EventSystems.StandaloneInputModule));
                }
                else if (item.MyObjName == "Canvas")
                {
                    newObj = ObjectFactory.CreateGameObject("Canvas",
                        typeof(Canvas),
                        typeof(UnityEngine.UI.CanvasScaler),
                        typeof(UnityEngine.UI.GraphicRaycaster));
                    newObj.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                }
                else
                {
                    newObj = new GameObject(item.MyObjName);
                }

                if (parent != null)
                    newObj.transform.SetParent(parent.transform);

                // 尝试添加脚本
                if (!string.IsNullOrEmpty(item.CsName))
                {
                    Type scriptType = Type.GetType(item.CsName);
                    if (scriptType != null && typeof(MonoBehaviour).IsAssignableFrom(scriptType))
                    {
                        newObj.AddComponent(scriptType);
                    }
                    else
                    {
                        Debug.LogWarning($"找不到类型 {item.CsName} 或类型不是 MonoBehaviour，跳过挂载");
                    }
                }
            }

            Debug.Log("✅ 一键布置完成");
        }

        private void 清除其他根对象()
        {
#if UNITY_EDITOR
            var roots = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var go in roots)
            {
                if (go == this.gameObject) continue;
                DestroyImmediate(go);
            }
#endif
        }

        private GameObject 生成路径(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;

            GameObject parent = null;
            string[] parts = path.Split('/');
            foreach (string part in parts)
            {
                GameObject found = parent == null ? GameObject.Find(part) : parent.transform.Find(part)?.gameObject;
                if (found == null)
                {
                    found = new GameObject(part);
                    if (parent != null)
                        found.transform.SetParent(parent.transform);
                }
                parent = found;
            }
            return parent;
        }
    }
}
